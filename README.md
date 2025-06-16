 
![Ready To Blazor](https://github.com/user-attachments/assets/19f6f5a2-4577-4f7e-b357-45cd9b91b8e0)

# Ready To Blazor

Bienvenido a Ready To Blazor, un starter-kit basado en Blazor Server que acelera la creación de aplicaciones empresariales conectadas a Dinaup. En este artículo encontrarás una introducción gradual—desde la pila de componentes hasta ejemplos de código listos para copiar—para que puedas publicar tu primera pantalla en minutos.


- [Aprende a Instalar IIS](http://doc.dinaup.com/instalar-iis-en-windows-server-2025)
- [Introducción Ready To Blazor](https://doc.dinaup.com/desarrollo/open-soruce/ready-to-blazor)
 

## 1. Configuración del archivo appsettings.json
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
 
