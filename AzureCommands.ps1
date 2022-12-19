
# Deploying App from ACR to Azure App Service
#create resource group if required.
# we are using already created resource group which is located at centralUS --name Eshop
Task 1: Create Identity
# create user managed-identity,be default app service uses defaulty managed identity 
# to authenticate with Azure Container Registry , user-assigned identity is optional  
> az identity create --name appServiceContainerAccess --resource-group Eshop

Task 2 : Create ACR (no need to create since we have available in Azure kamileshopcontainerregistry.azurecr.io)
# create azure container registry
#>az acr create --name kamileshopacr --resource-group Eshop --sku Basic --admin-enabled true

Task 3: Retrieve ACR admin credentials 
>az acr credential show --resource-group Eshop --name kamileshopcontainerregistry

Task 4: Push image from local docker to ACR
#step not required since we have already images available in ACR. We know how to push images from local to ACR.


Task 5: Authorize the managed identity for your registry.
# we should provide authorize access to appServiceContainerAccess this managed identity.

# retreive principalId for our managed Identity
principalId=$(az identity show --resource-group Eshop --name appServiceContainerAccess --query principalId --output tsv)

# retreive resource Id for container registery
registryId=$(az acr show --resource-group Eshop --name kamileshopcontainerregistry --query id --output tsv)

#Grant the managed identity permission to access the container registry:
az role assignment create --assignee $principalId --scope $registryId --role "AcrPull"

Task 6: Create Web App

1. Create AppService Plan
az appservice plan create --name kamileshopAppServicePlan --resource-group Eshop --is-linux



----------------------------------------------------------------------------------
Build image through ACR:
1. CLone repo in cloud.
2. Get inside the directory 
3. Use ACR build image commands
    > az acr build --registry kamileshopcontainerregistry --image imagename:v1 .

-----------------------------------------------------


create service principle and assign PullRequest Role to kamileshopacr ACR Registry 
 > az ad sp create-for-rbac --name kamileshopsp --scopes $(az acr show --resource-group Eshop --name kamileshopacr --query "id" --output tsv) --role "
AcrPull"

Password : "password": "o-N8Q~Jb5cULGDgCKAX7uffE_~quYJXTeeswebkF",
UserNsame : 63b83fe4-8fe4-432e-a645-670cc144581e

To get Username : > az ad sp list --display-name kamileshopsp --query "[].appId" --output tsv
63b83fe4-8

Passowrd is visible one time after creating service principle.

To assign new role to existing service principal.

> az role assignment create --assignee $SERVICE_PRINCIPAL_ID --scope $ACR_REGISTRY_ID --role acrpull

-------------------------------------------------------------------------------------------

Create and Assign role using Azure Managed identity
