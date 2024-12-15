# observability
Sample .NET Project containing multiple services created to demonstrate observability capabilities of .NET and Elastic Stack

## disclaimer

This code is not production ready. It has been created for the purposue of demonstation of .NET and ELK integration.

## assumptions

1. You know how to work with ASP.NET and C#
2. You know Azure (App Service, App Registration, Service Bus, Key Vault)
3. You know Terraform

# architecture

System is build as presented on below diagram.
UI is not included.
Infrastructure is Azure Based.

![System View](/resources/observability-system.png)

Each of the four services is built in the same way using Ports and Adapters Architecture.

![Service View](/resources/observability-service.png)

# setup

## prerequisites

1. Visual Studio
2. bash (can be git bash on Windows)
3. terraform
4. [psql](https://www.postgresql.org/docs/current/app-psql.html) (client only, best added to path)
5. Azure Account
    1. create 4 App Registrations - for each app service a dedicated one
    2. create resource group (I name it "admin-rg") and create in it:
        1. storage account and container for teffaform state (check ```backend.conf``` for details)
        2. Key Vault to store secrets
    3. grant yourself and the 4 App Registrations access to the secrets in the Key Vault

## infrastructure

Folder /src/terraform contains a simple terraform setup.
It assumes that:
1. each of the four web applications is using an App Registration (created manually)
2. there is a storage account and container already created for terraform state
3. there is an key vault created outside of the terraform managed infra
4. app registrations have access to secrets in that key vault.

Azure integrated authentication is assumed for local setup.

```
az login
az account list --output table
az account set --subscription [subscription-id]
```

Your chosen subscrition should be selected as the default one.

Terraform can be executed locally using a `terraform.tfvars` file like this:

```
sku_webapp = "B1"
sku_publicapi = "B1"
sku_domains = "B1"
application = "app-prefix"
bff_client_secret = "bff_client_secret"
prices_client_secret = "prices_client_secret"
products_client_secret = "products_client_secret"
stock_client_secret = "stock_client_secret"
domains_sql_admin_password = "password-for-postgres-admin"
```

to initialize run:
```
terraform init -backend-config="backend.conf"
```

App Registration secrets are needed for applications to be able to access the key vault.
They are stored as environment variables.

## application configuration

Applications are reading configuration in this order:

1. Environment variables (applicable when deployed to Azure - secret from ENV grants access to KeyVault)
2. Standard configuration files
3. KeyVault - using secret from ENV on Azure or Visual Studio credentials on local debug.
4. appsettings.local.json - custom config file, ignored by git for additional local overrides

## KeyVault

For simplicity all applications are using the same key vault (without App Configuration in the middle).
Secrets stored in the Key Valt are:

| Key | Service| Description |
|-----|--------|-------------|
|ElasticApm--SecretToken                    | Each          | Elastic APM Configuration - optional  |
|Logging--Elasticsearch--ShipTo--ApiKey     | Each          | Elastic Logging Configuration         |
|Logging--Elasticsearch--ShipTo--CloudId    | Each          | Elastic Loggin Configuration          |
|Bff--ApiKey                                | LoadRunner    | ApiKey needed to access BFF           |
|Bff--ServicePrincipal--ClientSecret        | BFF           | ClientSecret for BFF App Registration |
|Prices--ServicePrincipal--ClientSecret     | Prices        | ClientSecret for Prices App Registration   |
|ConnectionStrings--Prices                  | Prices        | DB Connection String |
|ConnectionStrings--PricesBus               | Prices        | Azure Service Bus ConnectionString |
|Products--ServicePrincipal--ClientSecret   | Products      | ClientSecret for Products App Registration |
|ConnectionStrings--Products                | Products      | DB Connection String |
|ConnectionStrings--ProductsBus             | Products      | Azure Service Bus ConnectionString |
|Stock--ServicePrincipal--ClientSecret      | Stock         | ClientSecret for Stock App Registration |
|ConnectionStrings--Stock                   | Stock         | DB Connection String |
|ConnectionStrings--StockBus                | Stock         | Azure Service Bus ConnectionString |
||||

To access key vault applications need to have Service Principal (App Registration) assigned.
Most of the configuration is stored in appsettings.json file but the Secret is stored as environmental variable.
Make sure you set it up correctly in terraform.

## local setup

Local development uses secrets from the KeyVault - so make sure to setup your account in Visual Studio.

Secrets from Key Vault are also used for Entity Framework migration runs. Fot this you need `az login`

## database

### DB Init
Solution is using PostgreSQL and infra script is creating a single Managed Postgres Server.

You can use the script in `/src/sql` folder to init 3 databased dedicated to each service.

Script will concatenate substrings ```1```, ```2``` and ```3``` to the domain password provided.
```
./setup-db.sh '[db-host-name] '[sqladmin-password]' '[domain-password-prefix]'
```

Then provide the connection strings through the Key Vault.

### Migrations

For each app you can run migrations from the level of Solution file  - `/src/app' folder using dedicated files.

To setup db:
```
db-prices-update.sh

db-products-update.sh

db-stock-update.sh
```

To create new migrations:
```
db-prices-add-migration.sh

db-products-add-migration.sh

db-stock-add-migration.sh
```