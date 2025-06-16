using Dinaup;
using System.ComponentModel.DataAnnotations;
using static DemoUp.MyDinaup.SectionsD.EntidadesD;
using static Dinaup.extensions;

namespace ReadyToBlazor.Models
{

	public class ConfigurarEntidadModel : IValidatableObject
	{

		public Guid ID { get; set; }

		[Required(ErrorMessage = "Se requiere nombre")]
		public string Nombre { get; set; } = "";

		[Required(ErrorMessage = "Se requieren apellidos")]
		public string Apellidos { get; set; } = "";

		[Required(ErrorMessage = "Se requiere NIF/CIF")]
		public string NifCif { get; set; } = "";

		[Required(ErrorMessage = "Se requiere Telefono")]
		public string Telefono { get; set; } = "";

		[Required(ErrorMessage = "Se requiere email de acceso")]
		[EmailAddress(ErrorMessage = "Formato de email inválido")]
		public string Email { get; set; } = "";

		[DataType(DataType.Password)]
		public string Password { get; set; } = "";

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
		public string ConfirmPassword { get; set; } = "";

		public bool CambiarContraseña { get; set; } = false;


 
		public WriteOperation ToWriteOperation()
		{

			// Esto ya debería estar validado por el formulario
		 this.ThrowIf_InvalidDataAnnotations(nameof(ConfigurarEntidadModel));

			var wop = new Dinaup.WriteOperation(ID, new());
			wop.DataMainRow.Add(EntidadesES.TextoPrincipal, Nombre + " " + Apellidos + " <" + Email + ">");
			wop.DataMainRow.Add(EntidadesES.NombrePersonalRazonSocial, Nombre);
			wop.DataMainRow.Add(EntidadesES.Apellidos, Apellidos);
			wop.DataMainRow.Add(EntidadesES.NIF, NifCif);
			wop.DataMainRow.Add(EntidadesES.TelefonoMovil, Telefono);
			if (Password.IsNotEmpty() && ConfirmPassword.IsNotEmpty() && Password == ConfirmPassword && Dinaup.extensions.ValidatePass(Password, Email,LenguajeE.Spanish).IsEmpty())
				wop.DataMainRow.Add(EntidadesES.ContraseñaAccesoSistema, Password);
			return wop;

		}




		// Lógica de validación personalizada
		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{

			if (Dinaup.extensions.IsEmail(Email) == false)
			{
				yield return new ValidationResult("El formato del email es inválido", new[] { nameof(Email) });
			}

			// Si se rellenó uno de los campos, el otro es obligatorio
			if (Password.IsNotEmpty() != ConfirmPassword.IsNotEmpty())
			{
				yield return new ValidationResult("Repite la contraseña es obligatorio si se proporciona una contraseña", new[] { nameof(ConfirmPassword) });
			}
			else if (Password.IsNotEmpty() && ConfirmPassword.IsNotEmpty())
			{
				// Si ambos están rellenados, validar la contraseña
				string motivoPassInvalida = Dinaup.extensions.ValidatePass(Password, Email, Dinaup.LenguajeE.Spanish);
				if (motivoPassInvalida.IsNotEmpty())
				{
					yield return new ValidationResult(motivoPassInvalida, new[] { nameof(Password), nameof(ConfirmPassword) });
				}
			}
		}


	}
}

