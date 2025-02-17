using CommunityToolkit.HighPerformance;
using Microsoft.Win32;
using Minio.DataModel;
using Radzen;
using System;

public class SMTPService
{
    public SMTPClient SMTPClient;


    
	public bool IsAvailable
	{
		get
		{
			return SMTPClient.IsAvailable ;
		}
	}




    public SMTPService(SMTPClient sMTPClient)
    {
        SMTPClient = sMTPClient;
    }

    public async Task SendLink_RecoveryAccount(string emailTo, string link)
	{
		string subject = "Recupera tu cuenta en ReadyToBlazor";
        string body = $@"<p>Hola,</p>
                        <p>Hemos recibido una solicitud para restablecer tu contraseña en ReadyToBlazor.</p>
                        <p>Puedes hacerlo haciendo clic en el siguiente enlace:</p>
                        <p><a href='{link}'>Restablecer contraseña</a></p>
                        <p>Si no solicitaste esto, puedes ignorar este mensaje.</p>
                        <p>¡Saludos!</p>";
        
        await SMTPClient.SendEmailAsync(emailTo, subject, body);
    }

    public async Task SendLink_ActivateAccount(string emailTo, string link)
    {
        string subject = "Activa tu cuenta en ReadyToBlazor";
		string body = $@"<p>¡Bienvenido a ReadyToBlazor!</p>
						<p>Para completar tu registro y activar tu cuenta, por favor haz clic en el siguiente enlace:</p>
						<p><a href='{link}'>Activar cuenta</a></p>
                        <p>Gracias por unirte a nuestra comunidad.</p>
                        <p>¡Nos vemos dentro!</p>";
        
        await SMTPClient.SendEmailAsync(emailTo, subject, body);
    }
}