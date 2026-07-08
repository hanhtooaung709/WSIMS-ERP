# 1. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Solution ထဲမှာ ပါဝင်ပတ်သက်သမျှ အဓိက Folder အားလုံးကို ကူးယူပါမယ်
COPY ERP.Warehouse/ERP.Warehouse.sln ERP.Warehouse/
COPY ERP.Warehouse/ERP.Warehouse.Api/ ERP.Warehouse/ERP.Warehouse.Api/
COPY ERP.Warehouse/ERP.Warehouse.App/ ERP.Warehouse/ERP.Warehouse.App/
COPY ERP.Warehouse/ERP.Warehouse.Models/ ERP.Warehouse/ERP.Warehouse.Models/

# Error တက်စေတဲ့ ပြင်ပ Project ၂ ခုကိုပါ တစ်ခါတည်း Docker ထဲ ဆွဲထည့်ပါမယ်
COPY WSIMS-ERP.Shared/ WSIMS-ERP.Shared/
COPY Module.CommonDbService/ Module.CommonDbService/

RUN dotnet restore ERP.Warehouse/ERP.Warehouse.Api/ERP.Warehouse.Api.csproj
RUN dotnet publish ERP.Warehouse/ERP.Warehouse.Api/ERP.Warehouse.Api.csproj -c Release -o /app/publish

# 2. Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# The app walks up from /app looking for ERP.Warehouse.sln to locate the Config folder
COPY ERP.Warehouse/ERP.Warehouse.sln /app/
COPY Config/appsettings.docker.json /Config/appsettings.json
COPY Config/custom-setting-local.json /Config/
COPY Config/custom-setting-docker.json /Config/

EXPOSE 5066
ENV ASPNETCORE_URLS=http://+:5066
ENTRYPOINT ["dotnet", "ERP.Warehouse.Api.dll"]