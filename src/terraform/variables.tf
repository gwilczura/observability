variable "application" {
  type        = string
  description = "Application name"
}

variable "location" {
  type        = string
  description = "location"
  default     = "West Europe"
}

variable "sku_webapp" {
  type        = string
  description = "app service plan size"
  default     = "B1"
}

variable "sku_publicapi" {
  type        = string
  description = "app service plan size"
  default     = "B1"
}

variable "sku_domains" {
  type        = string
  description = "app service plan size"
  default     = "B1"
}

variable "bff_client_secret"{
  type        = string
  description = "secret for bff application"
  sensitive   = true
}

variable "prices_client_secret"{
  type        = string
  description = "secret for prices application"
  sensitive   = true
}

variable "products_client_secret"{
  type        = string
  description = "secret for products application"
  sensitive   = true
}

variable "stock_client_secret"{
  type        = string
  description = "secret for stock application"
  sensitive   = true
}

variable "domains_sql_admin_password"{
  type        = string
  description = "password for admin - sqladmin"
  sensitive   = true
}