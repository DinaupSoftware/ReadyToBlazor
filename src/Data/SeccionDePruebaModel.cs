using Dinaup;
using Elasticsearch.Net.Specification.IndicesApi;
using System.ComponentModel.DataAnnotations;
using static DemoUp.MyDinaup.SectionsD.SeccionDePruebasAPID;
using static DemoUp.MyDinaup.SectionsD.SeccionDePruebasAPIListaD;

namespace ReadyToBlazor.Data
{
	public class SeccionDePruebaModel
	{



		public Guid ID;

		[Required(ErrorMessage = "El campo Título es obligatorio.")]
		public string Titulo { get; set; } = string.Empty;
		public decimal CampoDecimal{ get; set; } =  0;
		public DateTime? FechaYHoraConSegundos { get; set; }  
		public TimeOnly? HoraSinSegundos { get; set; }  
		public DateOnly? FechaDePrueba { get; set; }  

		public List<SeccionDePruebaItemModel> Items { get; set; } = new List<SeccionDePruebaItemModel>();

		public void RemoveItem(SeccionDePruebaItemModel item)
		{

			if (item.ID.IsEmpty())
				Items.Remove(item);
			else
				item.Eliminado = true;





		}

		public void AddItem()
		{

			if (Items.Count < 1000)
				Items.Add(new SeccionDePruebaItemModel());

		}


		public SeccionDePruebaModel()
		{
		}
		public SeccionDePruebaModel(DemoUp.MyDinaup.SectionsD.SeccionDePruebasAPID.SeccionDePruebasAPI_WithListC datos)
		{
			ID = datos.MainRow.ID;
			Titulo = datos.MainRow.TextoPrincipal;
			CampoDecimal = datos.MainRow.CampoDecimal.Value;
			FechaYHoraConSegundos = datos.MainRow.FechaYHoraConSegundos_UTC.ToLocalTime_PN();
			HoraSinSegundos = datos.MainRow.HoraSinSegundos;
			FechaDePrueba = datos.MainRow.FechaDePrueba;
			Items = datos.ListRows.Select(i => new SeccionDePruebaItemModel(i)).ToList();

		}






		public Dictionary<string, string> ToDic()
		{
			return new Dictionary<string, string>()
			{
				{ SeccionDePruebasAPIListaES.ID, ID.ToString() },
				{ SeccionDePruebasAPIListaES.TextoPrincipal, Titulo }
			};

		}

		public WriteOperation ToWriteOperation()
		{

			var wOp = new WriteOperation(this.ID);
			wOp.DataMainRow.Add(SeccionDePruebasAPIES.TextoPrincipal, Titulo);
			wOp.DataMainRow.Add(SeccionDePruebasAPIES.CampoDecimal, CampoDecimal.ToSQL());

			if (FechaYHoraConSegundos.HasValue)
				wOp.DataMainRow.Add(SeccionDePruebasAPIES.FechaYHoraConSegundos_UTC, FechaYHoraConSegundos.Value.ToUniversalTime().ToSQL());
			else
				wOp.DataMainRow.Add(SeccionDePruebasAPIES.FechaYHoraConSegundos_UTC, "");

			if (HoraSinSegundos.HasValue)
				wOp.DataMainRow.Add(SeccionDePruebasAPIES.HoraSinSegundos, HoraSinSegundos.Value.ToSQL());
			else
				wOp.DataMainRow.Add(SeccionDePruebasAPIES.HoraSinSegundos, TimeOnly.MinValue.ToSQL());

			if (FechaDePrueba.HasValue)
				wOp.DataMainRow.Add(SeccionDePruebasAPIES.FechaDePrueba, FechaDePrueba.ToSQL());
			else
				wOp.DataMainRow.Add(SeccionDePruebasAPIES.FechaDePrueba, "");


			wOp.DataListRows = Items.Select(i => i.ToDic()).ToList();
			return wOp;


		}





	}



	public class SeccionDePruebaItemModel
	{

		public Guid ID;


		public bool Eliminado;

	   [Required(ErrorMessage = "El campo Título es obligatorio.")]
		public string Titulo { get; set; } = string.Empty;


		public SeccionDePruebaItemModel()
		{
		}
		public SeccionDePruebaItemModel(DemoUp.MyDinaup.SectionsD.SeccionDePruebasAPIListaD.SeccionDePruebasAPIListaC datos)
		{
			this.ID = datos.ID;
			this.Titulo = datos.TextoPrincipal;
			this.Eliminado = datos.Eliminado.Value;

		}


		public Dictionary<string, string> ToDic()
		{
			return new Dictionary<string, string>()
			{
				{ SeccionDePruebasAPIListaES.ID, ID.STR() },
				{  SeccionDePruebasAPIListaES.Eliminado  , Eliminado.STR() },
				{ SeccionDePruebasAPIListaES.TextoPrincipal, Titulo }
			};


		}

	}

}
