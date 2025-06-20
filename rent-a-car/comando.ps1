az acr login --name acrlab007asantana --resource-group LABOO7

docker tag bff-rent-car-local acrlab007asantana.azurecr.io/bff-rent-car-local:v1

docker push acrlab007asantana.azurecr.io/bff-rent-car-local:v1

az containerapp env create --name bff-rent-car-local --resource-group LAB007 --location eastus

az containerapp create --name bff-rent-car --resource-group LAB007 --environment bff-rent-car-local --image acrlab007asantana.azurecr.io/bff-rent-car-local:v1 --registry-server acrlab007asantana.azurecr.io --target-port 3001 --ingress external
#az containerapp create \
#    --name bff-rent-car \
#    --resource-group LABOO7 \
#    --environment bff-rent-car-local \
#    --image acrlab007asantana.azurecr.io/bff-rent-car-local:v1 \
#    --registry-server acrlab007asantana.azurecr.io \
#    --target-port 3001 \
#    --ingress external

# Outros comandos

# Status do container
# az containerapp show --name bff-rent-car --resource-group LAB007

# Logs do container
# az containerapp logs show --name bff-rent-car --resource-group LAB007

# Modificar a imagem do container
# az containerapp update --name bff-rent-car --resource-group LAB007 --target-port 3001

# Verificar URL do container
# az containerapp show --name bff-rent-car --resource-group LAB007 --query properties.configuration.ingress.fqdn

# Verificar credenciais do container
# az containerapp revision list --name bff-rent-car --resource-group LAB007

# Verificar variaveis de ambiente do container
# az containerapp revision show --name bff-rent-car --resource-group LAB007 --revision <revision-name> --query properties.configuration.environmentVariables