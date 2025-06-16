using Microsoft.AspNetCore.Authentication.Cookies;
using Radzen;
using ReadyToBlazor.ServicesDinaup;
using System.Reflection;

try
{

	var builder = WebApplication.CreateBuilder(args);

	// Obtiene el entorno y la ruta raíz de la aplicación
	var env = builder.Environment;
	var contentRoot = env.ContentRootPath;

	var localConfig = Path.Combine(contentRoot, "appsettings.json");
	var externalConfig = @"C:\IIS\appsettings.readytoblazor.json";

	// Limpia las fuentes de configuración por defecto (opcional)
	builder.Configuration.Sources.Clear();

	// Si existe appsettings.json en el sitio, lo carga; si no, carga el externo
	if (File.Exists(localConfig))
		builder.Configuration.AddJsonFile(localConfig, optional: false, reloadOnChange: true);
	else if (File.Exists(externalConfig))
		builder.Configuration.AddJsonFile(externalConfig, optional: false, reloadOnChange: true);
	else
		throw new FileNotFoundException("No se encontró ningún fichero de configuración. " + $"Buscados: '{localConfig}' y '{externalConfig}'");

	// (Opcional) Añade otras fuentes: entorno, variables, etc.
	builder.Configuration.AddEnvironmentVariables().AddCommandLine(args);



	builder.Services.AddControllers();
	builder.Services.AddRazorPages();
	builder.Services.AddServerSideBlazor();
	builder.Services.AddRadzenComponents();
	builder.Services.AddHttpContextAccessor();
	builder.Services.AddScoped<CurrentUserService>();


	// Dinaup -> Logs
	var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0";
	Dinaup.Logs.Initialize(nameof(ReadyToBlazor), version, new Dinaup.Logs.ElasticConfig());

	// Dinaup -> Client
	var configDinaup = builder.Configuration.GetSection("Dinaup").Get<DinaupSettings>();
	var configSMTP = builder.Configuration.GetSection("Smtp").Get<SmtpSettings>();

	var dinaupClient = new Dinaup.DinaupClientC(configDinaup.APIBaseUrl, configDinaup.APIKey, configDinaup.APISecret, "*");

	Task.Run(async () => { await dinaupClient.InitializeAsync(5000); });

	builder.Services.AddSingleton(dinaupClient);
	builder.Services.AddSingleton<SMTPClient>(new SMTPClient(configSMTP));
	builder.Services.AddSingleton<SMTPService>();
	builder.Services.AddScoped<LocationService>();


	var app = builder.Build();

	if (!app.Environment.IsDevelopment())
	{
		app.UseExceptionHandler("/Error");
		app.UseHsts();
	}

	app.UseMiddleware< MaintenanceMiddleware>();
	app.UseHttpsRedirection();
	app.UseStaticFiles();
	app.UseRouting();
	app.MapBlazorHub();
	app.MapControllers();
	app.MapFallbackToPage("/_Host");
	app.Run();
}
catch (Exception)
{
	var builder2 = WebApplication.CreateBuilder(args);
	var app2 = builder2.Build();
	app2.UseMiddleware<ErrorFatalMiddleware>();
	app2.Run();


}
finally
{

	Dinaup.Logs.CloseAndFlush();

}

