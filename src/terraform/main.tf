terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=3.112.0"
    }
  }
  backend "azurerm" {}
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "rg" {
  name     = "${var.application}-rg"
  location = "${var.location}"
}

resource "azurerm_service_plan" "splan_webapp"{
  name                = "${var.application}-plan-webapp"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  os_type             = "Linux"
  sku_name            = "${var.sku_webapp}"
}

resource "azurerm_linux_web_app" "webapp_bff"{
  name                  = "${var.application}-bff"
  resource_group_name   = azurerm_resource_group.rg.name
  location              = azurerm_service_plan.splan_webapp.location
  service_plan_id       = azurerm_service_plan.splan_webapp.id
  https_only            = "true"
  site_config {
    application_stack {
        dotnet_version = "8.0"
    }
    cors {
      allowed_origins = toset(["https://${var.application}-ui.azurewebsites.net"])
    }
    health_check_path = "health"
    app_command_line = "dotnet Wilczura.Observability.Bff.Host.dll"
  }
  app_settings = {
    BFF__ServicePrincipal__ClientSecret = "${var.bff_client_secret}"
  }
}

resource "azurerm_linux_web_app" "webapp_ui"{
  name                  = "${var.application}-ui"
  resource_group_name   = azurerm_resource_group.rg.name
  location              = azurerm_service_plan.splan_webapp.location
  service_plan_id       = azurerm_service_plan.splan_webapp.id
  https_only            = "true"
  site_config {
    application_stack {
        node_version = "18-lts"
    }
    app_command_line = "pm2 serve /home/site/wwwroot --no-daemon --spa"
  }
  app_settings = {
    ENV_VERSION = "X-OBSRV-T1-UI"
  }
}

resource "azurerm_service_plan" "splan_publicapi"{
  name                = "${var.application}-publicapi-plan"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  os_type             = "Linux"
  sku_name            = "${var.sku_publicapi}"
}

resource "azurerm_service_plan" "splan_domains_prices"{
  name                = "${var.application}-domains-prices-plan"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  os_type             = "Linux"
  sku_name            = "${var.sku_domains}"
}

resource "azurerm_linux_web_app" "domians_prices_api"{
  name                  = "${var.application}-prices-api"
  resource_group_name   = azurerm_resource_group.rg.name
  location              = azurerm_service_plan.splan_domains_prices.location
  service_plan_id       = azurerm_service_plan.splan_domains_prices.id
  https_only            = "true"
  site_config {
    application_stack {
        dotnet_version = "8.0"
    }
    health_check_path = "health"
    app_command_line = "dotnet Wilczura.Observability.Prices.Host.dll"
  }
  app_settings = {
    Prices__ServicePrincipal__ClientSecret = "${var.prices_client_secret}"
  }
}

resource "azurerm_service_plan" "splan_domains_products"{
  name                = "${var.application}-domains-products-plan"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  os_type             = "Linux"
  sku_name            = "${var.sku_domains}"
}

resource "azurerm_linux_web_app" "domians_products_api"{
  name                  = "${var.application}-products-api"
  resource_group_name   = azurerm_resource_group.rg.name
  location              = azurerm_service_plan.splan_domains_products.location
  service_plan_id       = azurerm_service_plan.splan_domains_products.id
  https_only            = "true"
  site_config {
    application_stack {
        dotnet_version = "8.0"
    }
    health_check_path = "health"
    app_command_line = "dotnet Wilczura.Observability.Products.Host.dll"
  }
  app_settings = {
    Products__ServicePrincipal__ClientSecret = "${var.products_client_secret}"
  }
}

resource "azurerm_service_plan" "splan_domains_stock"{
  name                = "${var.application}-domains-stock-plan"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  os_type             = "Linux"
  sku_name            = "${var.sku_domains}"
}

resource "azurerm_linux_web_app" "domians_stock_api"{
  name                  = "${var.application}-stock-api"
  resource_group_name   = azurerm_resource_group.rg.name
  location              = azurerm_service_plan.splan_domains_stock.location
  service_plan_id       = azurerm_service_plan.splan_domains_stock.id
  https_only            = "true"
  site_config {
    application_stack {
        dotnet_version = "8.0"
    }
    health_check_path = "health"
    app_command_line = "dotnet Wilczura.Observability.Stock.Host.dll"
  }
  app_settings = {
    Stock__ServicePrincipal__ClientSecret = "${var.stock_client_secret}"
  }
}

#################
## STORAGE
#################

resource "azurerm_resource_group" "rg_storage" {
  name     = "${var.application}-storage-rg"
  location = "${var.location}"
}

resource "azurerm_postgresql_flexible_server" "domains_storage" {
  name                          = "${var.application}-domains-pgsql"
  resource_group_name           = azurerm_resource_group.rg_storage.name
  location                      = azurerm_resource_group.rg_storage.location
  version                       = "16"
  public_network_access_enabled = true
  administrator_login           = "sqladmin"
  administrator_password        = "${var.domains_sql_admin_password}"
  zone                          = 3

  storage_tier = "P4"

  sku_name   = "B_Standard_B1ms"
}

resource "azurerm_postgresql_flexible_server_firewall_rule" "fr_azure" {
  name             = "azure"
  server_id        = azurerm_postgresql_flexible_server.domains_storage.id
  start_ip_address = "0.0.0.0"
  end_ip_address   = "0.0.0.0"
}

#################
## QUEUES
#################

resource "azurerm_resource_group" "rg_massages" {
  name     = "${var.application}-messages-rg"
  location = "${var.location}"
}

resource "azurerm_servicebus_namespace" "sb_domains" {
  name                = "${var.application}-domains-servicebus"
  location            = azurerm_resource_group.rg_massages.location
  resource_group_name = azurerm_resource_group.rg_massages.name
  sku                 = "Standard"

  tags = {
    source = "terraform"
  }
}