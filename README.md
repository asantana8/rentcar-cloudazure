**Construção de uma Aplicação de Aluguel de Carros totalmente Cloud-Native**

**Documentação De Execução Do Projeto – Lado Azure**

**Azure DB**

![](Aspose.Words.4162d8fb-9fb3-4d14-bf19-1c24d54533f3.001.png)

**Client Request**

![Tela de computador

O conteúdo gerado por IA pode estar incorreto.](Aspose.Words.4162d8fb-9fb3-4d14-bf19-1c24d54533f3.002.png)





**Front-End em Node.js**

![Texto

O conteúdo gerado por IA pode estar incorreto.](Aspose.Words.4162d8fb-9fb3-4d14-bf19-1c24d54533f3.003.png)

**Codigo .NET Functions Azure**

![](Aspose.Words.4162d8fb-9fb3-4d14-bf19-1c24d54533f3.004.png)




**Over View de Todos os Recursos**

![Interface gráfica do usuário, Texto, Aplicativo, Email

O conteúdo gerado por IA pode estar incorreto.](Aspose.Words.4162d8fb-9fb3-4d14-bf19-1c24d54533f3.005.png)

**JSON ARM DE ALGUNS DOS RECURSOS**

**LAB007 | Resource Group**

{

`    `"id": "/subscriptions/653c100c-d96c-4ee6-8cc9-db5eecea84ed/resourceGroups/LAB007",

`    `"name": "LAB007",

`    `"type": "Microsoft.Resources/resourceGroups",

`    `"location": "eastus",

`    `"tags": {

`        `"Grupo007": "Projeto final"

`    `},

`    `"properties": {

`        `"provisioningState": "Succeeded"

`    `},

`    `"apiVersion": "2020-06-01"

}

**acrlab007asantana | Container Register**

{

`    `"sku": {

`        `"name": "Basic",

`        `"tier": "Basic"

`    `},

`    `"type": "Microsoft.ContainerRegistry/registries",

`    `"id": "/subscriptions/653c100c-d96c-4ee6-8cc9-db5eecea84ed/resourceGroups/LAB007/providers/Microsoft.ContainerRegistry/registries/acrlab007asantana",

`    `"name": "acrlab007asantana",

`    `"location": "eastus",

`    `"tags": {},

`    `"properties": {

`        `"loginServer": "acrlab007asantana.azurecr.io",

`        `"creationDate": "2025-06-18T00:09:09.806872Z",

`        `"provisioningState": "Succeeded",

`        `"adminUserEnabled": false,

`        `"policies": {

`            `"quarantinePolicy": {

`                `"status": "disabled"

`            `},

`            `"trustPolicy": {

`                `"type": "Notary",

`                `"status": "disabled"

`            `},

`            `"retentionPolicy": {

`                `"days": 7,

`                `"lastUpdatedTime": "2025-06-18T00:09:21.9152068+00:00",

`                `"status": "disabled"

`            `}

`        `}

`    `},

`    `"apiVersion": "2019-05-01"

}

**akv-asantana-dev-eastus | Key Vault**

{

`    `"id": "/subscriptions/653c100c-d96c-4ee6-8cc9-db5eecea84ed/resourceGroups/LAB007/providers/Microsoft.KeyVault/vaults/akv-asantana-dev-eastus",

`    `"name": "akv-asantana-dev-eastus",

`    `"type": "Microsoft.KeyVault/vaults",

`    `"location": "eastus",

`    `"tags": {},

`    `"properties": {

`        `"sku": {

`            `"family": "A",

`            `"name": "Standard"

`        `},

`        `"tenantId": "e5c29000-8c38-4b00-a096-2d0f6f4296cd",

`        `"networkAcls": {

`            `"bypass": "None",

`            `"defaultAction": "Allow",

`            `"ipRules": [],

`            `"virtualNetworkRules": []

`        `},

`        `"accessPolicies": [],

`        `"enabledForDeployment": false,

`        `"enabledForDiskEncryption": false,

`        `"enabledForTemplateDeployment": false,

`        `"enableSoftDelete": true,

`        `"softDeleteRetentionInDays": 90,

`        `"enableRbacAuthorization": true,

`        `"vaultUri": "https://akv-asantana-dev-eastus.vault.azure.net/",

`        `"provisioningState": "Succeeded"

`    `},

`    `"apiVersion": "2016-10-01"

}






**Telas de Execução da Function**

![Interface gráfica do usuário, Texto, Aplicativo, Email

O conteúdo gerado por IA pode estar incorreto.](Aspose.Words.4162d8fb-9fb3-4d14-bf19-1c24d54533f3.006.png)

![](Aspose.Words.4162d8fb-9fb3-4d14-bf19-1c24d54533f3.007.png)

**CosmosDB Funcionando**

![Interface gráfica do usuário, Texto, Aplicativo, Word

O conteúdo gerado por IA pode estar incorreto.](Aspose.Words.4162d8fb-9fb3-4d14-bf19-1c24d54533f3.008.png)


**Banco de Dados Configurado** 

![Interface gráfica do usuário, Texto, Aplicativo, Email

O conteúdo gerado por IA pode estar incorreto.](Aspose.Words.4162d8fb-9fb3-4d14-bf19-1c24d54533f3.009.png)

**Service Bus e suas Filas**

![Interface gráfica do usuário, Aplicativo

O conteúdo gerado por IA pode estar incorreto.](Aspose.Words.4162d8fb-9fb3-4d14-bf19-1c24d54533f3.010.png)

**Passo Final – Logic App**

![](Aspose.Words.4162d8fb-9fb3-4d14-bf19-1c24d54533f3.011.png)
