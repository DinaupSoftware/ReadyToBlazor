
> [!NOTE]  
> ?? Convención en Continua Mejora
>
> Esta convención está en constante evolución con el objetivo de facilitar y agilizar el desarrollo de aplicaciones basadas en Ready to Blazor.
>
> Todos los colaboradores e integradores de Dinaup son bienvenidos a proponer cambios, añadir nuevas buenas prácticas o sugerir mejoras. ¡Tu feedback es clave para que esta plantilla siga creciendo y adaptándose a vuestras necesidades!

-----


# Ready to Blazor
Este proyecto es una plantilla para desarrollar aplicaciones web conectadas a Dinaup.
Se ha realizado una implementación mínima que sirva de punto de partida para hacer tu app.


1. Se han implementado los componentes de **Radzen**: https://blazor.radzen.com/?theme=material3 
2. `SessionPage.razor`  (/SessionPage)  es el componente que permite iniciar sesión y crear nuevas cuentas (empleados, no clientes).
3. En `program.cs` creamos un DinaupClient y se pone com singleton por que es lo que se usará en toda la aplicación para interactuar con dinaup.
4. En `program.cs` Se inicia `Dinaup.Logs`, que es compatible con elastic search y kibana. (Internamente usa serilog) es importante usar este loggin porque es el que usan todas las app cerificadas de Dinaup.
5. Si no deseas dar la posibilidad a los usuarios de registrarse o recuperar contraseñas, puedes modificar  `LoginForm.razor`
6. En esta página únicamente pueden registrarse `Entidades`, los empleados deben darse de alta asociados a la licencia.
7. Puedes enfrentar problemas de ratelimit y concurrencia utilizando la API de dinaup. Puedes ahorrar costes utilizando `Postgre Sync` (Solo es necesario preocuparse por esto si hablamos de volumenes realmente grandes, como una tienda online).
8. Hay un controlador `HealthCheck` para comprobar el estado que permite conectarlo a herramientas de monitorización. (La respuesta también está estandarizada)
9. En `appsettings.json` también está para configurar SMTP, recomendamos encarecidamente AWS o Resend, pero no es necesario para la certificación.
10. No usar `!` en ifs: `if(!string.IsNullOrEmpty(Password))` tiene muy mala legibilidad.
11. Usar `String.IsEmpty( )` o `String.IsNotEmpty()`
11. Usar `Object.IsNull( )` o `Object.IsNotNull()`
12. Para validar contraseñas `Dinaup.extensions.ValidatePass`
12. Para validar emails `Dinaup.extensions.ValidatePass`
13. Usamos Models con `DataAnnotations`  ejemplo `ConfigurarEntidadModel.cs`

## Estandarizacion

 


 
## Requisitos
1. Pon los credenciales API y SMTP en appsettings


## Proyecto
Importar Dinaup.extensions
![](https://cdn.dinaup.com/up/202505/image.png)


## _Imports.razor
1. `@inject CurrentUserService CurrentUserService` datos dela sesión actual.
2. `@inject NotificationService NotificationService` se usa para dar feedback al usuario. ( https://blazor.radzen.com/notification?theme=material3 )
3. `@inject SMTPService SMTPService` se utiliza para enviar los diferentes tipos de emails.
4. `@using static Dinaup.extensions` métodos y extensiones de Dinaup **Importante para conversión**


## Formatos.
- `DateOnly` para Fechas.
- `TimeOnly` para Horas.
- `DateTime` para Fechas y Horas.
    - Las fechas y horas (datetime) las guardamos como UTC, las convertimos a local a la hora de mostrarlo en la UI.
    - Se debe especificar el Kind.
- `Decimal` decimales.
- `Integer` enteros.
- `Long` no recomendado.
- `Boolean` aceptado.
- `float`, `double`, `single`:  **NUNCA, no están aceptados**.


## Conversiones [Dinaup.extensions]

### Conversión a Texto
- Siempre que sea posible, usamos `.STR()`  sobre `.ToString()`.
- `.ToString()` se considera inestable porque puede dar resultados diferentes dependiendo del Culture del Thread.
- `Decimal.STR()`: Siempre devuelve formato en-Es `1.23`
- `Boolean.STR()`: Siempre devuelve formato `1` o `0`
- `String.STR()`: Si es `Null`  no da `NullReferenceException` sino que devuelve `String.Empty`
- `DateTime.STR()`: ISO 
- `DateOnly.STR()`: ISO 
 
### Conversión a Decimal
- `String.DEC()`: Si la conversión falla produce exception.
- `String.DEC(0)`: Si la conversión fala, devuelve 0. (o el valor que se indique)
- `String.INT()`: Si la conversión falla produce exception.
- `String.INT(0)`: Si la conversión fala, devuelve 0. (o el valor que se indique)
 
 
 
 
 ## UI

 - Cuando se guarda una acción los botones se ponene `IsBusy="true"`
 
 ```
 @inject NotificationService NotificationService

<RadzenStack Orientation="Orientation.Horizontal" AlignItems="AlignItems.Center" JustifyContent="JustifyContent.Center" Gap="3rem" Wrap="FlexWrap.Wrap" class="rz-p-12">
    <RadzenButton   IsBusy=@busy Click=@OnBusyClick Text="Save" />
</RadzenStack>

@code {
    bool busy;
    
    private void OnClick(string text)
    {
        NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Info, Summary = "Button Clicked", Detail = text });
    }

    async Task OnBusyClick()
    {
        busy = true;
        await Task.Delay(2000);
        busy = false;
    }
}
```

- Inputs 
```
	<RadzenFormField class="w-100 m-2" Text="Teléfono" Variant="Variant.Flat">
			<Start>
				<RadzenIcon Icon="call" />
			</Start>
			<ChildContent>
				<RadzenTextBox Name="AdminAccountTelefono" @bind-Value=@Data.Telefono Style="width:100%" />
			</ChildContent>
			<Helper>
				<RadzenDataAnnotationValidator Component="AdminAccountTelefono" />
			</Helper>
		</RadzenFormField>
```

- Formularios
Se debe indicar TItem 
```
	<RadzenTemplateForm Data=@Data TItem="ConfiguracionAdministradorModel" Submit=@Guardar >

```


 
## Acceso a Dinaup


- `/ServicesDinaup`
 > Se ha creado la carpeta `/ServicesDinaup`, el nombre hará que siempre esté junto a `Services`.
 > Tal vez sería más acertado el patrón  `Repository` en la mayoría de casos, `Service` representa mayor flexibilidad.


- Definicion de Servicios

Utilizando el nombre de la Sección de Dinaup, ejemplo:
 
- /ServicesDinaup/`Entidades`Service.cs
- /ServicesDinaup/`TiposDeVenta`Service.cs
- /ServicesDinaup/`Ventas`Service.cs
- /ServicesDinaup/`Compras`Service.cs
- /ServicesDinaup/`Productos`Service.cs
- /ServicesDinaup/`Impuestos`Service.cs
- /ServicesDinaup/`Retenciones`Service.cs


