using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using Dinaup;
using Radzen;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ReadyToBlazor.Pages.Sesion
{
	public partial class LoginForm : ComponentBase
	{




		private LoginModel loginModel = new LoginModel();
		private CreateAccountModel createAccountModel = new CreateAccountModel();
		private RecoverPasswordModel recoverPasswordModel = new RecoverPasswordModel();
		private ChangePasswordWithRecoveryCodeModel changePasswordWithRecoveryCodeModel = new ChangePasswordWithRecoveryCodeModel();




		// This is used for the UI to show and hide messages
		private bool isActivationEmailSent;
		private bool isRecoveryEmailSent;
		private bool ischangePasswordWithRecoveryCodeModel;




		private async Task LoginSubmit()
		{

			try
			{
				isActivationEmailSent = false;
				isRecoveryEmailSent = false;
				ischangePasswordWithRecoveryCodeModel = false;
				await _LoginSubmit();
			}
			catch (Exception ex)
			{

				NotificationService.Notify(NotificationSeverity.Error, "", ex.GetDescription());
			}
		}
		private async Task Logout()
		{

			try
			{
				isActivationEmailSent = false;
				isRecoveryEmailSent = false;
				ischangePasswordWithRecoveryCodeModel = false;
				await _Logout();
			}
			catch (Exception ex)
			{

				NotificationService.Notify(NotificationSeverity.Warning, "", ex.GetDescription());
				return;
			}

		}
		private async Task CreateAccountSubmit()
		{

			try
			{
				isActivationEmailSent = false;
				isRecoveryEmailSent = false;
				ischangePasswordWithRecoveryCodeModel = false;
				await _CreateAccountSubmit();
			}
			catch (Exception ex)
			{

				NotificationService.Notify(NotificationSeverity.Warning, "", ex.GetDescription());
				return;
			}

		}
		private async Task RecoverPasswordSubmit()
		{
			try
			{
				isActivationEmailSent = false;
				isRecoveryEmailSent = false;
				ischangePasswordWithRecoveryCodeModel = false;
				await _RecoverPasswordSubmit();
			}
			catch (Exception ex)
			{

				NotificationService.Notify(NotificationSeverity.Warning, "", ex.GetDescription());
				return;
			}

		}
		private async Task ChangePasswordWithCodeSubmit()
		{
			try
			{

				isActivationEmailSent = false;
				isRecoveryEmailSent = false;
				ischangePasswordWithRecoveryCodeModel = false;
				await _ChangePasswordWithCodeSubmit();
			}
			catch (Exception ex)
			{

				NotificationService.Notify(NotificationSeverity.Warning, "", ex.GetDescription());
				return;
			}

		}






		private async Task _LoginSubmit()
		{

			var validationResults = new List<ValidationResult>();
			bool isValid = Validator.TryValidateObject(loginModel, new ValidationContext(loginModel), validationResults, true);
			if (isValid == false)
			{
				NotificationService.Notify(NotificationSeverity.Warning, "", string.Join(" ", validationResults.Select(r => r.ErrorMessage)));
				return;
			}

			if (await LoginRateLimiter.CanAttemptSignInAsync(loginModel.Email) == false)
			{
				NotificationService.Notify(NotificationSeverity.Warning, "", "Se han superado el número máximo de intentos de inicio de sesión, espera unos minutos.");
				return;

			}


			try
			{
				var userObj = await DinaupClient.Session_SignInAsync(loginModel.Email, loginModel.Password, Dinaup.extensions.GetUserAgent(HttpContextAccessor.HttpContext), Dinaup.extensions.GetUserIP(HttpContextAccessor.HttpContext), true);
				if (userObj == null)
				{
					NotificationService.Notify(NotificationSeverity.Warning, "", "Credenciales incorrectas");
					return;
				}

				await CurrentUserService.SetSession(userObj.SessionDetails, loginModel.RememberMe ? 8 : 0);
			}
			catch (Exception ex)
			{

				NotificationService.Notify(NotificationSeverity.Warning, "", ex.GetDescription());
				return;
			}


		}



		private async Task _Logout()
		{

			await CurrentUserService.LogoutAsync();
		}



		private async Task _CreateAccountSubmit()
		{


			var validationResults = new List<ValidationResult>();
			bool isValid = Validator.TryValidateObject(createAccountModel, new ValidationContext(createAccountModel), validationResults, true);
			if (isValid == false)
			{
				NotificationService.Notify(NotificationSeverity.Warning, "", string.Join(" ", validationResults.Select(r => r.ErrorMessage)));
				return;
			}



			try
			{




				var createAccountResult = await DinaupClient.Session_CreateAccountAsync(createAccountModel.Name, createAccountModel.Email, createAccountModel.Password,
					Dinaup.extensions.GetUserAgent(HttpContextAccessor.HttpContext), Dinaup.extensions.GetUserIP(HttpContextAccessor.HttpContext));

				var uri = new Uri(Navigation.Uri);
				var activationLink = $"{uri.Scheme}://{uri.Host}{(uri.IsDefaultPort ? "" : $":{uri.Port}")}/";
				activationLink += $"dinaup_account_activation?token={createAccountResult.TemporalaccountId.ToURL()}&email={createAccountModel.Email.ToURL()}&cod={createAccountResult.Code.ToURL()}";

				await SMTPService.SendLink_ActivateAccount(createAccountModel.Email, activationLink);

				isActivationEmailSent = true;



			}
			catch (Exception ex)
			{

				NotificationService.Notify(NotificationSeverity.Error, "", ex.GetDescription());
			}

		}



		private async Task _ChangePasswordWithCodeSubmit()
		{



			var validationResults = new List<ValidationResult>();
			bool isValid = Validator.TryValidateObject(changePasswordWithRecoveryCodeModel, new ValidationContext(changePasswordWithRecoveryCodeModel), validationResults, true);
			if (isValid == false)
			{
				NotificationService.Notify(NotificationSeverity.Warning, "", string.Join(" ", validationResults.Select(r => r.ErrorMessage)));
				return;
			}



			var createAccountResult = await DinaupClient.Session_ChangePasswordWithCodeAsync(changePasswordWithRecoveryCodeModel.Code, changePasswordWithRecoveryCodeModel.Password, Dinaup.extensions.GetUserAgent(HttpContextAccessor.HttpContext), Dinaup.extensions.GetUserIP(HttpContextAccessor.HttpContext));

			if (createAccountResult.Ok)
			{

				p = "login";
				ischangePasswordWithRecoveryCodeModel = true;

			}


		}


		// Función para procesar la recuperación de contraseña
		private async Task _RecoverPasswordSubmit()
		{

			// Validamos el modelo
			var validationResults = new List<ValidationResult>();
			bool isValid = Validator.TryValidateObject(recoverPasswordModel, new ValidationContext(recoverPasswordModel), validationResults, true);
			if (isValid == false)
			{
				NotificationService.Notify(NotificationSeverity.Warning, "", string.Join(" ", validationResults.Select(r => r.ErrorMessage)));
				return;
			}


			var createAccountResult = await DinaupClient.Session_RecoverPasswordAsync(recoverPasswordModel.Email, Dinaup.extensions.GetUserAgent(HttpContextAccessor.HttpContext), Dinaup.extensions.GetUserIP(HttpContextAccessor.HttpContext));
			var uri = new Uri(Navigation.Uri);
			var recoveryLink = $"{uri.Scheme}://{uri.Host}{(uri.IsDefaultPort ? "" : $":{uri.Port}")}/";
			recoveryLink += $"dinaup_account_recovery?token={createAccountResult}&login={recoverPasswordModel.Email.ToURL()}";
			await SMTPService.SendLink_RecoveryAccount(recoverPasswordModel.Email, recoveryLink);
			isRecoveryEmailSent = true;

		}






		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();

			if (p == "recovery")
			{
				changePasswordWithRecoveryCodeModel.Email = login;
				changePasswordWithRecoveryCodeModel.Code = token;
			}

		}






	}
}
