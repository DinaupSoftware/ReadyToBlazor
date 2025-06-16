using System.Security.Claims;
using Dinaup;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;



namespace DinaZen.Services
{
	public class CurrentUserService
	{



		public event Action? OnChange;
		public DinaupUserSessionDTO? User;
		public bool isInitialized;

		private IJSRuntime JSRuntime { get; set; }
		private DinaupClientC DinaupClient;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private NavigationManager _navigationManager;

		public CurrentUserService(IHttpContextAccessor iHttpContextAccessor, DinaupClientC dinaupClientC, NavigationManager navigationManager, IJSRuntime JSRuntime)
		{
			this._httpContextAccessor = iHttpContextAccessor;
			this.DinaupClient = dinaupClientC;
			this._navigationManager = navigationManager;
			this.JSRuntime = JSRuntime;
		}



		#region "Public Methods"



		public async Task InitializeAsync()
		{

			if (this.isInitialized) return;

			var httpContext = _httpContextAccessor.HttpContext;
			if (httpContext == null)
				throw new Exception("HttpContext no disponible.");

			var dinaup_sessionid = httpContext.Request.Cookies["dinaup_sessionid"];

			if (dinaup_sessionid.IsGUID())
				await RefreshSessionAsync(dinaup_sessionid.ToGUID());


			isInitialized = true;

		}


		public async Task SetSession(DinaupUserSessionDTO usersession, int expirationHours)
		{

			if (usersession.IsNull() || usersession.IsLogued == false)
			{
				await JSRuntime.InvokeVoidAsync("setCookie", "dinaup_sessionid", "", 0);
				User = null;
				NotifyStateChanged();
			}
			else
			{

				await JSRuntime.InvokeVoidAsync("setCookie", "dinaup_sessionid", usersession.SessionId, expirationHours);
				User = usersession;
				NotifyStateChanged();
			}

		}




		/// <summary>
		/// Cierra la sesión actual, elimina la cookie de autenticación y actualiza el estado del usuario
		/// para que los componentes AuthorizeView se actualicen.
		/// </summary>
		public async Task LogoutAsync()
		{
			await JSRuntime.InvokeVoidAsync("setCookie", "dinaup_sessionid", "", 0);
			User = null;
			NotifyStateChanged();

		}






		#endregion









		private void NotifyStateChanged() => OnChange?.Invoke();




		public async Task RefreshSessionAsync(Guid sessionId)
		{


			var httpContext = _httpContextAccessor.HttpContext;
			if (httpContext == null)
				throw new Exception("HttpContext no disponible.");


			if (sessionId == Guid.Empty)
			{

				if (User.IsNotNull())
				{

					User = null;
					await JSRuntime.InvokeVoidAsync("setCookie", "dinaup_sessionid", "", 0);
					NotifyStateChanged();
					return;

				}

			}
			else
			{

				var sessionDetails = await DinaupClient.Session_GetDetailsAsync(sessionId, Dinaup.extensions.GetUserAgent(httpContext), Dinaup.extensions.GetUserIP(httpContext));
				if (sessionDetails != null && sessionDetails.Details.IsNotNull() && sessionDetails.Details.IsLogued)
				{
					User = sessionDetails.Details;
					NotifyStateChanged();
					return;

				}
				else
				{
					User = null;
					await JSRuntime.InvokeVoidAsync("setCookie", "dinaup_sessionid", "", 0);
					NotifyStateChanged();
					return;

				}

			}

		}













	}
}
