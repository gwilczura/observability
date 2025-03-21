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

This application can be deployed to Azure but it in prepared for manual deployment.

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

and `backend.conf`:

```
resource_group_name="admin-rg"
storage_account_name="[someprefix]tfstate" <== use correct name here
container_name="[someprefix]tfstate"       <== and here
key="terraform.tfstate"
```

to initialize run:
```
terraform init -backend-config="backend.conf"
```

To access DB from your local machine you need to edit the main.tf to pass your local ip through the firewall.

```
resource "azurerm_postgresql_flexible_server_firewall_rule" "fr_local" {
  name             = "local"
  server_id        = azurerm_postgresql_flexible_server.domains_storage.id
  start_ip_address = "your.ip.goes.here"
  end_ip_address   = "your.ip.goes.here"
}
```

and then of course
```
terraform plan
terraform apply
```

App Registration secrets are needed for applications to be able to access the key vault.
They are stored as environment variables.

## application configuration

Applications are reading configuration in this order:

1. Environment variables (applicable when deployed to Azure - secret from ENV grants access to KeyVault)
2. Standard configuration files
3. appsettings.local.json - custom config file, ignored by git for additional local overrides
4. KeyVault - using secret from ENV on Azure or Visual Studio credentials on local debug.

### AppSettings

Make sure your `ServicePrincipal` setup is correct. It's important to understand the order in which configuration is being applied.

If you have client secret in environmental variable, and then in app settings you keep it as empty string - then it will be an empty string.

To avoid such override remove the field from JSON completely.

### KeyVault

For simplicity all applications are using the same key vault (without App Configuration in the middle).
Secrets stored in the Key Valt are:

| Key | Service| Description |
|-----|--------|-------------|
|ElasticApm--SecretToken                    | Each          | Elastic APM Configuration - optional  |
|Logging--Elasticsearch--ShipTo--ApiKey     | Each          | Elastic Logging Configuration         |
|Logging--Elasticsearch--ShipTo--CloudId    | Each          | Elastic Loggin Configuration          |
|Bff--ApiKey                                | LoadRunner&BFF| ApiKey needed to access BFF           |
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

App Registrations need to have ```Key Valult Secrets User``` permission.

You need to have ```Key Vault Secrets Officer``` to add the secrets.

To simplify setup you can fun ```setup-keyVault.sh``` from ```src\keyVault``` folder.

```
./setup-keyVault.sh obs-manual-kv
```

To access KeyVault you need to have KeyVaultName configured in the appsettings file or as environment variable.
```
  "KeyVaultName": "some-name"
```

### elastic search API key
what i usually strugle to find is - where to get elastic parameters from :)

so the cloud id you can find on the [deployment management console](https://cloud.elastic.co/deployments/)
but the Key you need to create through Kibana dashboard. 

click `Open Kibana` from youd eployment screen -> Left menu -> Managementt -> Stack Management -> Security -> API Keys

### local setup

Local development uses secrets from the KeyVault - so make sure to setup your account in Visual Studio.

Secrets from Key Vault are also used for Entity Framework migration runs. Fot this you need `az login`

## database

### DB Init
Solution is using PostgreSQL and infra script is creating a single Managed Postgres Server.

You can use the script in `/src/sql` folder to init 3 databased dedicated to each service.

Script will concatenate substrings ```1```, ```2``` and ```3``` to the domain password provided.
```
./setup-db.sh '[db-host-name]' '[sqladmin-password]' '[domain-password-prefix]'
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

# deployment

1. Fulfill prerequisites
2. deploy terraform
3. setup the databases
4. fill key vault
5. run migrations for each domain
6. (optional) run one of the services locally for test
7. deploy 4 host applications to azure manally from Visual Studio.

# debugging

After deployment to Azure if you are struggling with running the app:
1. navigate to your app service
2. go to Development Tools -> SSH -> Go

run:

```
cd..
cd appsvctmp/volatile/logs/runtime/
cat container.log
```

You should see something similar to:

```
2024-12-17T22:57:29.9016036Z Running user provided startup command...
2024-12-17T22:57:42.3114420Z info: Wilczura.Observability.Common.Host.Logging.StartupLog[0] Logger created
2024-12-17T22:57:42.3438827Z info: Wilczura.Observability.Common.Host.Logging.StartupLog[0] Wilczura.Observability.Products.Host | 1.0.7.0
2024-12-17T22:57:43.6511059Z info: Wilczura.Observability.Common.Host.Logging.StartupLog[0] KeyVault: some-name, ClientSecretCredential, [guid]
2024-12-17T22:58:00.8931094Z info: Wilczura.Observability.Common.Host.Logging.StartupLog[0] Source: MemoryConfigurationSource
2024-12-17T22:58:00.9002993Z info: Wilczura.Observability.Common.Host.Logging.StartupLog[0] Source: EnvironmentVariablesConfigurationSource
2024-12-17T22:58:00.9729656Z info: Wilczura.Observability.Common.Host.Logging.StartupLog[0] Source: MemoryConfigurationSource
2024-12-17T22:58:00.9743047Z info: Wilczura.Observability.Common.Host.Logging.StartupLog[0] Source: EnvironmentVariablesConfigurationSource
2024-12-17T22:58:00.9915376Z info: Wilczura.Observability.Common.Host.Logging.StartupLog[0] Source: JsonConfigurationSource, appsettings.json
2024-12-17T22:58:00.9916072Z info: Wilczura.Observability.Common.Host.Logging.StartupLog[0] Source: JsonConfigurationSource, appsettings.Production.json
2024-12-17T22:58:00.9916109Z info: Wilczura.Observability.Common.Host.Logging.StartupLog[0] Source: EnvironmentVariablesConfigurationSource
2024-12-17T22:58:00.9916135Z info: Wilczura.Observability.Common.Host.Logging.StartupLog[0] Source: ChainedConfigurationSource
2024-12-17T22:58:00.9916162Z info: Wilczura.Observability.Common.Host.Logging.StartupLog[0] Source: JsonConfigurationSource, appsettings.local.json
2024-12-17T22:58:00.9916188Z info: Wilczura.Observability.Common.Host.Logging.StartupLog[0] Source: AzureKeyVaultConfigurationSource
2024-12-17T22:58:01.8706128Z Console: Disabling default log providers. Enabling ELK.
```