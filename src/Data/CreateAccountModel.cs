﻿

using System.ComponentModel.DataAnnotations;

public class CreateAccountModel
{
	[Required(ErrorMessage = "El nombre es obligatorio.")]
	public string Name { get; set; }

	[Required(ErrorMessage = "El email es obligatorio."), EmailAddress(ErrorMessage = "El email no es válido.")]
	public string Email { get; set; }

	[Required(ErrorMessage = "La contraseña es obligatoria.")]
	public string Password { get; set; }

	[Required(ErrorMessage = "Debe confirmar la contraseña."), Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
	public string ConfirmPassword { get; set; }
}
