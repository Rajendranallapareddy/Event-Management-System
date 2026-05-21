# Variables
$RESOURCE_GROUP="EMS-RG"
$LOCATION="eastus"
$ACR_NAME="emsacr" + (Get-Random -Maximum 9999)
$API_IMAGE="ems-api"
$ANGULAR_IMAGE="ems-angular"
$ACI_API="ems-api-instance"
$ACI_ANGULAR="ems-angular-instance"

# Login
az login
az group create --name $RESOURCE_GROUP --location $LOCATION

# Create ACR
az acr create --resource-group $RESOURCE_GROUP --name $ACR_NAME --sku Basic --admin-enabled true

# Login to ACR
az acr login --name $ACR_NAME

# Build and push backend
docker build -f EMS.API/Dockerfile -t $ACR_NAME.azurecr.io/$API_IMAGE:latest .
docker push $ACR_NAME.azurecr.io/$API_IMAGE:latest

# Build and push frontend
docker build -f Frontend/AngularApp/Dockerfile -t $ACR_NAME.azurecr.io/$ANGULAR_IMAGE:latest ./Frontend/AngularApp
docker push $ACR_NAME.azurecr.io/$ANGULAR_IMAGE:latest

# Deploy API to ACI
az container create --resource-group $RESOURCE_GROUP --name $ACI_API `
  --image $ACR_NAME.azurecr.io/$API_IMAGE:latest `
  --cpu 1 --memory 1.5 --os-type Linux --ports 8080 `
  --environment-variables ASPNETCORE_ENVIRONMENT=Production ConnectionStrings__DefaultConnection="Server=sql-server-instance;Database=EMSFinalDB;User Id=xxx;Password=xxx" `
  --registry-login-server $ACR_NAME.azurecr.io --registry-username (az acr credential show -n $ACR_NAME --query username -o tsv) --registry-password (az acr credential show -n $ACR_NAME --query passwords[0].value -o tsv)

# Deploy Angular to ACI
az container create --resource-group $RESOURCE_GROUP --name $ACI_ANGULAR `
  --image $ACR_NAME.azurecr.io/$ANGULAR_IMAGE:latest `
  --cpu 1 --memory 1.5 --os-type Linux --ports 80

# Get public IPs
az container show --resource-group $RESOURCE_GROUP --name $ACI_API --query ipAddress.ip --output tsv
az container show --resource-group $RESOURCE_GROUP --name $ACI_ANGULAR --query ipAddress.ip --output tsv