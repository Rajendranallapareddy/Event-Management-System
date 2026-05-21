FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY EMS.sln .
COPY EMS.DAL/*.csproj EMS.DAL/
COPY EMS.BLL/*.csproj EMS.BLL/
COPY EMS.Common/*.csproj EMS.Common/
COPY EMS.API/*.csproj EMS.API/
RUN dotnet restore
COPY . .
RUN dotnet publish EMS.API/EMS.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "EMS.API.dll"]