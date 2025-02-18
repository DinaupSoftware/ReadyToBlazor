using Microsoft.AspNetCore.Authentication.Cookies;
using Radzen;
using System.Reflection;

try
{

	var builder = WebApplication.CreateBuilder(args);

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


	var app = builder.Build();

	if (!app.Environment.IsDevelopment())
	{
		app.UseExceptionHandler("/Error");
		app.UseHsts();
	}

	app.UseMiddleware<MaintenanceMiddleware>();
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

