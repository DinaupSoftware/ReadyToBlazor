### Instalar windows server


1. Instalar IIS 
 ```
 Install-WindowsFeature -name Web-Server,Web-Http-Redirect,Web-Request-Monitor,Web-Http-Tracing,Web-Filtering,Web-Stat-Compression,Web-Dyn-Compression,Web-WebSockets -IncludeManagementTools
 ```
2. Instalar Net 9
 Instala NET 9 (Hosting Bundle para .NET 9:) https://dotnet.microsoft.com/en-us/download/dotnet/9.0

3. Opcional WebDeploy
    https://www.microsoft.com/en-us/download/details.aspx?id=106070


### Configuración del archivo appsettings.json
Debes crear  `appsettings.json`  
```json
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "AllowedHosts": "*",
    "Dinaup": {
        "APIBaseUrl": "",
        "APIKey": "",
        "APISecret": ""
    },
    "Smtp": {
        "Host": "",
        "Port": 2587,
        "EnableSsl": true,
        "UserName": "",
        "SenderEmail": "",
        "SenderName": "",
        "Password": ""
    }
}
```
 
