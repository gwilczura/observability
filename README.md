# observability
Sample .NET Project containing multiple services created to demonstrate observability capabilities of .NET and Elastic Stack

## disclaimer

This code is not production ready. It has been created for the purposue of demonstation of .NET and ELK integration.

# architecture

System is build as presented on below diagram.

![System View](/resources/observability-system.png)

Each of the four services is built in the same way using Ports and Adapters Architecture.

![Service View](/resources/observability-service.png)

# setup

## infrastructure

Folder /src/terraform contains a simple terraform setup.
It assumes that:
1. each of the four web applications is using an App Registration
2. there is a storage account for terraform state
3. there is an key vault created outside of the terraform managed infra
4. app registrations have access to secrets in that key vault.

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

App Registration secrets are needed for applications to be able to access the key vault.

## application configuration

For simplicity all applications are using the same key vault (without App Configuration in the middle).
Secrets stored in the Key Valt are:

| Key | Service| Description |
|-----|--------|-------------|
|ElasticApm--SecretToken                    | Each          | Elastic Cloud Configuration            |
|Logging--Elasticsearch--ShipTo--ApiKey     | Each          | Elastic Cloud Configuration            |
|Logging--Elasticsearch--ShipTo--CloudId    | Each          | Elastic Cloud Configuration            |
|Logging--Elasticsearch--ShipTo--Password   | Each          | Elastic Cloud Configuration            |
|Logging--Elasticsearch--ShipTo--Username   | Each          | Elastic Cloud Configuration            |
|Bff--ApiKey                                | LoadRunner    | ApiKey needed to access BFF            |
|Bff--ServicePrincipal--ClientSecret        | BFF           | ClientSecret for BFF App Registration            |
|Prices--ServicePrincipal--ClientSecret     | Prices        | ClientSecret for Prices App Registration            |
|ConnectionStrings--Prices                  | Prices        | DB Connection String |
|ConnectionStrings--PricesBus               | Prices        | Azure Service Bus ConnectionString |
|Products--ServicePrincipal--ClientSecret   | Products      | ClientSecret for Products App Registration |
|ConnectionStrings--Products                | Products      | DB Connection String |
|ConnectionStrings--ProductsBus             | Products      | Azure Service Bus ConnectionString |
|Stock--ServicePrincipal--ClientSecret      | Stock         | ClientSecret for Stock App Registration |
|ConnectionStrings--Stock                   | Stock         | DB Connection String |
|ConnectionStrings--StockBus                | Stock         | Azure Service Bus ConnectionString |
||||

## local setup

Local development uses secrets from the KeyVault - so make sure to setup your account in Visual Studio.
Secrets from Key Vault are also used for Entity Framework migration runs.

## database

### DB Init
Solution is using PostgreSQL and infra script is creating a single Managed Postgres Server.

You can use the script in `/src/sql` folder to init 3 databased dedicated to each service.

Then provide the connection strings through the Key Vault.

### Migrations

For each app you can run migrations from the level of Solution file  - `/src/app' folder.

To setup db:
```
dotnet ef database update --project Wilczura.Observability.Products.Adapters.Postgres --startup-project Wilczura.Observability.Products.Host

dotnet ef database update --project Wilczura.Observability.Prices.Adapters.Postgres --startup-project Wilczura.Observability.Prices.Host

dotnet ef database update --project Wilczura.Observability.Stock.Adapters.Postgres --startup-project Wilczura.Observability.Stock.Host
```

To create new migrations:
```

dotnet ef migrations add SomeNewMigration --project Wilczura.Observability.Products.Adapters.Postgres --startup-project Wilczura.Observability.Products.Host
dotnet ef migrations add SomeNewMigration --project Wilczura.Observability.Prices.Adapters.Postgres --startup-project Wilczura.Observability.Prices.Host
dotnet ef migrations add SomeNewMigration --project Wilczura.Observability.Stock.Adapters.Postgres --startup-project Wilczura.Observability.Stock.Host
```