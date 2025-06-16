using Dinaup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;



public partial class HealthCheckController : Controller
{


	private readonly IMemoryCache cache;
	private readonly DinaupClientC DinaupClient;

	public HealthCheckController(IMemoryCache _cache, DinaupClientC DinaupClient)
	{
		this.cache = _cache;
		this.DinaupClient = DinaupClient;
	}



	[HttpGet("HealthCheck")]
	public async Task<IResult> getHealthCheck()
	{
		const string cacheKey = "HealthCheckCacheKey";

		if (cache.TryGetValue(cacheKey, out Dictionary<string, bool> components) == false)
		{
			components = new Dictionary<string, bool>();
			components.Add("DinaupClient", this.DinaupClient.IsConnected);
			cache.Set(cacheKey, components, TimeSpan.FromSeconds(10));
		}

		var isHealthy = !components.Values.Contains(false);
		var status = isHealthy ? "Healthy" : "Unhealthy";
		var statusCode = isHealthy ? StatusCodes.Status200OK : StatusCodes.Status503ServiceUnavailable;
		return TypedResults.Json(new { status, components }, statusCode: statusCode);
	}


}