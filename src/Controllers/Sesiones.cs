
using Microsoft.AspNetCore.Mvc;
using Dinaup;




public class DinaupSessionController : Controller
{



	public DinaupClientC DinaupClient { get; set; }

	public DinaupSessionController(DinaupClientC _client )
	{
		DinaupClient = _client;
	}




	[HttpGet("dinaup_account_activation")]
	public async Task<IActionResult> dinaup_account_activation(string token, string emailurl, string cod)
	{



		var userAgent = Dinaup.extensions.GetUserAgent(HttpContext);
		var ip = Dinaup.extensions.GetUserIP(HttpContext);

		var activateParams = new Models.AccountActivationModel();
		activateParams.ActivationCode = cod;
		activateParams.TemporaryAccountId = token.ToGUID();


		try
		{
			var r = await DinaupClient.Session_ActivateAccountAsync(activateParams, userAgent, ip);
			if (r.IsNotNull() && r.userId.IsGUID())
			{


				return Redirect("/Session?t=1");
			}
			else
			{
				return Redirect("/Session?t=0");
			}


		}
		catch (Exception ex)
		{

			return Redirect("/Session?t=0");
		}



	}





	[HttpGet("dinaup_account_recovery")]
	public async Task<IActionResult> dinaup_account_recovery(string token, string login)
	{
		return Redirect("/Sesion?p=recovery&token=" + token + "&login=" + login);
	}









}
