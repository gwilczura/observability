VAULTNAME=$1
               
az keyvault secret set --vault-name $VAULTNAME -n Logging--Elasticsearch--ShipTo--ApiKey    --value ''
az keyvault secret set --vault-name $VAULTNAME -n Logging--Elasticsearch--ShipTo--CloudId   --value ''

az keyvault secret set --vault-name $VAULTNAME -n Bff--ApiKey                               --value ''
az keyvault secret set --vault-name $VAULTNAME -n Bff--ServicePrincipal--ClientSecret       --value ''

az keyvault secret set --vault-name $VAULTNAME -n Prices--ServicePrincipal--ClientSecret    --value ''
az keyvault secret set --vault-name $VAULTNAME -n ConnectionStrings--Prices                 --value ''
az keyvault secret set --vault-name $VAULTNAME -n ConnectionStrings--PricesBus              --value ''

az keyvault secret set --vault-name $VAULTNAME -n Products--ServicePrincipal--ClientSecret  --value ''
az keyvault secret set --vault-name $VAULTNAME -n ConnectionStrings--Products               --value ''
az keyvault secret set --vault-name $VAULTNAME -n ConnectionStrings--ProductsBus            --value ''

az keyvault secret set --vault-name $VAULTNAME -n Stock--ServicePrincipal--ClientSecret     --value ''
az keyvault secret set --vault-name $VAULTNAME -n ConnectionStrings--Stock                  --value ''
az keyvault secret set --vault-name $VAULTNAME -n ConnectionStrings--StockBus               --value ''