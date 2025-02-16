


using System.ComponentModel.DataAnnotations;

public class RecoverPasswordModel
{
	[Required(ErrorMessage = "El email es obligatorio."), EmailAddress(ErrorMessage = "El email no es válido.")]
	public string Email { get; set; }
}

