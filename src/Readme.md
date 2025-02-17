



# Ready to Blazor
Este proyecto es una plantilla para desarrollar aplicaciones web conectadas a Dinaup.
Se ha realizado una implementaci�n m�nima que sirva de punto de partida para hacer tu app.




1. Se han implementado los componentes de **Radzen**: https://blazor.radzen.com/?theme=material3 
1. `SessionPage.razor`  (/SessionPage)  es el componente que permite iniciar sesi�n y crear nuevas cuentas (empleados, no clientes).
1. En program.cs creamos un client y se pone com singleton por que es lo que se usar� en toda la aplicaci�n para interactuar con dinaup.
1. Se inicia Dinaup.Logs
1. Si no deseas dar la posibilidad a los usuarios de registrarse o recuperar contrase�as, puedes modificar  `LoginForm.razor`
1. En esta p�gina �nicamente pueden registrarse entidades, los empleados deben darse de alta asociados a la licencia.
1. Puedes enfrentar problemas de ratelimit y concurrencia utilizando la API de dinaup. Puedes ahorrar costes utilizando Pg Sync.
1. hay un controlador HealthCheck para comprobar el estado
1. 
## Requisitos
1. Pon tu clave API en appsettings
1. Debes configurar un email smtp en Dinaup.


## Beneficios
1. Sin preocuparse por el backend
1. Inicio de sesi�n seguro (ratelimit, cifrado..)
1. Crear cuentas
1. Recuperar Contrase�as


 