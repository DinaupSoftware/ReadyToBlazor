

using Microsoft.AspNetCore.Http;

public class ErrorFatalMiddleware
{
	private readonly RequestDelegate _next;

	public ErrorFatalMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
		Dinaup.Logs.Fatal("Estamos experimentando inconvenientes al iniciar la aplicación. Por favor, inténtelo de nuevo en unos minutos. Gracias por su paciencia.");
		await context.Response.WriteAsync("Estamos experimentando inconvenientes al iniciar la aplicación. Por favor, inténtelo de nuevo en unos minutos. Gracias por su paciencia.");
	}
}