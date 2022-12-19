#How to run this file 
# Go to Azure Powershell , upload this file and type .\file_name in command prompt.
# run dir command for list all files.

# Delete file and Folder from dir
# remove-item file_nameOrFolderName 


#Variable Declaration
$resourceGroup="Eshop"
$registryName ="kamileshopacr"
$servicePrincipalName = "eshopDeploymentACRAccess" # we will be creating service principal
$resourceIdACR ="<resource id of ACR>"
$servicePrincipalUserName ="<user name of service principal"  #also Service Principal Id.
$servicePrincipalPassword ="<password of service principal"
$servicePrincipalACRPullRole = "AcrPull"

#keyvault
$eshopKeyVaultName ="kamildemoeshopkeyvault"  #keyvault name
$eshopacraccesspassword ="kamil467eshopacraccesspassword" #key for service principal password
$eshopserviceprinciaplIdkey ="kamil467eshopserviceprinciaplIdkey" #key for service principal Id.

#creating reasourcegroup 
az group create --name  $resourceGroup --location eastus


#create Azure key vault 
az keyvault create --resource-group $resourceGroup --name $eshopKeyVaultName

Write-Debug ("KeyVault has been successfully created:"+$eshopKeyVaultName)

# Create ACR
az acr create --name $registryName --resource-group $resourceGroup --sku Basic --admin-enabled true

Write-Debug ("ACR has been successfully created."+ $registryName)

#set resource Id for ACR.
$resourceIdACR = az acr show --resource-group $resourceGroup --name $registryName --query "id" --output tsv

Write-Debug ("ACR resource ID is :"+ $resourceIdACR)

#create service principal
# assign ACR scope
# assign azure container registry pull access
$servicePrincipalPassword = $(az ad sp create-for-rbac --name $servicePrincipalName --scopes $resourceIdACR --role $servicePrincipalACRPullRole --query "password" --output tsv)

Write-Debug ("Service Principal has been successfully created and scope has been assigned successfully"+ $servicePrincipalName)

$servicePrincipalUserName = $(az ad sp list --display-name $servicePrincipalName --query "[].appId" --output tsv)

Write-Debug ("UserName of Service principal is :"+ $servicePrincipalUserName)


Write-Debug ("Password of SP:"+ $servicePrincipalPassword);


# store service principal password into Azure keyVault so that our app services will use this to read the ACR and perform the deployment
az keyvault secret set --vault-name $eshopKeyVaultName --name $eshopacraccesspassword --value $servicePrincipalPassword


# store service principal's Principal Id in Azure key vault
az keyvault secret set --vault-name $eshopKeyVaultName --name $eshopserviceprinciaplIdkey --value $servicePrincipalUserName



Write-Debug ("cloning demo application from github")

git clone https://github.com/kamil467/acr-build-helloworld-node buildfive

cd buildfive

# Build Process
# ACR tasks internally uses docker build command to build this application
az acr build --registry $registryName --image "testimage:v1" .

# image will be pushed into azure container repository by default as soon as it will be generated
 $registryLoginServer =$registryName + ".azurecr.io"
 $imageName =$registryName + ".azurecr.io/testimage:v1"
 #container deployment
 az container create --resource-group $resourceGroup `
 --name "kamileshopcontainer" `
 --image  $imageName `
 --registry-login-server $registryLoginServer `
 --registry-username $(az keyvault secret show --vault-name $eshopKeyVaultName --name $eshopserviceprinciaplIdkey --query value --output tsv) `
 --registry-password $(az keyvault secret show --vault-name $eshopKeyVaultName --name $eshopacraccesspassword --query value --output tsv) `
 --dns-name-label "acr-task-kamileshopacrdemo" `
 --query "{FQDN:ipAddress.fqdn}" `
 --output table



 ## Delete the resource group after used
 # az group delete --name resourcegroupname