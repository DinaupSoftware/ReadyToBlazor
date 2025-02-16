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
 
