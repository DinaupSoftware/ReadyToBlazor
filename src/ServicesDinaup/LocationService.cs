using Dinaup;
using DinaZen;
using Microsoft.Extensions.Caching.Memory;
using static DemoUp.MyDinaup.Reports.FuncionalidadD;
using static DemoUp.MyDinaup.SectionsD;

namespace ReadyToBlazor.ServicesDinaup
{
	public partial class LocationService
	{


		private DinaupClientC _client = null;
		private CurrentUserService _currentUser = null;
		private readonly IMemoryCache _cache;
		private static readonly SemaphoreSlim _gate = new(1, 1);


		public LocationService(
			 DinaupClientC client,
			 CurrentUserService currentUser,
			 IMemoryCache cache)
		{
			_client = client ?? throw new ArgumentNullException(nameof(client));
			_currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
			_cache = cache ?? throw new ArgumentNullException(nameof(cache));
		}

		#region API público -----------------------------------------------------------------------
	 
		public async Task<List<APIPaisesC.APIPaises_RowC>> GetAllPaisesAsync()
		{
			var rpt = new APIPaisesC();
			await rpt.ExecuteQueryAsync(_client, 1, 1000, string.Empty, adminMode: true);
			return rpt.Rows;
		}
		public Task<Guid> GetPaisIdAsync(string pais = default) =>
			GetOrCreateAsync(
				cacheKey: $"pais:{pais}",
				findAsync: () => FindPaisAsync(pais),
				createAsync: () => CreatePaisAsync(pais));

		public Task<Guid> GetProvinciaIdAsync(string provincia = default) =>
			GetOrCreateAsync(
				cacheKey: $"provincia:{provincia}",
				findAsync: () => FindProvinciaAsync(provincia),
				createAsync: () => CreateProvinciaAsync(provincia));

		public Task<Guid> GetMunicipioIdAsync(string municipio = default) =>
			GetOrCreateAsync(
				cacheKey: $"municipio:{municipio}",
				findAsync: () => FindMunicipioAsync(municipio),
				createAsync: () => CreateMunicipioAsync(municipio));

		public Task<Guid> GetCodigoPostalIdAsync(string cp = default) =>
			GetOrCreateAsync(
				cacheKey: $"cp:{cp}",
				findAsync: () => FindCpAsync(cp),
				createAsync: () => CreateCpAsync(cp));
		 

		#endregion

		#region Búsqueda y creación concreta de cada entidad ---------------------------------------

		private async Task<Guid> FindPaisAsync(string pais)
		{
			var rpt = new APIPaisesC();
			rpt.AddFilter(PaisesD.PaisesES.TextoPrincipal, "=", pais);
			await rpt.ExecuteQueryAsync(_client, 1, 2, pais, adminMode: true);

			return rpt.Rows
					  .FirstOrDefault(x => x.TextoPrincipal.Normalized() == pais.Normalized())
					  ?.ID ?? Guid.Empty;
		}



		private async Task<Guid> FindProvinciaAsync(string provincia)
		{
			var rpt = new APIProvinciasC();
			await rpt.ExecuteQueryAsync(_client, 1, 23, provincia, adminMode: true);
			return rpt.Rows.FirstOrDefault(x => x.TextoPrincipal.Normalized() == provincia.Normalized())?.ID ?? Guid.Empty;
		}


		private async Task<Guid> FindMunicipioAsync(string municipio)
		{
			var rpt = new APIMunicipiosC();
			rpt.AddFilter(MunicipiosD.MunicipiosES.TextoPrincipal, "=", municipio);
			await rpt.ExecuteQueryAsync(_client, 1, 1000, municipio, adminMode: true);

			return rpt.Rows
					  .FirstOrDefault(x => x.TextoPrincipal.Normalized() == municipio.Normalized())
					  ?.ID ?? Guid.Empty;
		}


		private async Task<Guid> FindCpAsync(string cp)
		{
			var rpt = new APICodigosPostalesC();
			rpt.AddFilter(CodigosPostalesD.CodigosPostalesES.CodigoPostal, "=", cp);
			await rpt.ExecuteQueryAsync(_client, 1, 10, adminMode: true);

			return rpt.Rows.FirstOrDefault()?.ID ?? Guid.Empty;
		}


		#endregion


		#region "Create"
		private async Task<Guid> CreateCpAsync(string cp)
		{
			var op = new WriteOperation(string.Empty);
			op.DataMainRow.Add(CodigosPostalesD.CodigosPostalesES.TextoPrincipal, cp);
			op.DataMainRow.Add(CodigosPostalesD.CodigosPostalesES.CodigoPostal, cp);
			await _client.RunWriteOperationAsync(_currentUser.User, CodigosPostalesD.CodigosPostalesES._SectionID, op, true);
			op.EnsureSuccess();
			//_logger.LogInformation("Creado código postal {CP} con ID {Id}", cp, op.WriteOperationResult.RowID);
			return op.WriteOperationResult.RowID;
		}
		private async Task<Guid> CreateMunicipioAsync(string municipio)
		{
			var op = new WriteOperation(string.Empty);
			op.DataMainRow.Add(MunicipiosD.MunicipiosES.TextoPrincipal, municipio);
			op.DataMainRow.Add(MunicipiosD.MunicipiosES.Etiqueta, municipio);
			await _client.RunWriteOperationAsync(_currentUser.User, MunicipiosD.MunicipiosES._SectionID, op, true);
			op.EnsureSuccess();
			//_logger.LogInformation("Creado municipio {Municipio} con ID {Id}", municipio, op.WriteOperationResult.RowID);
			return op.WriteOperationResult.RowID;
		}
		private async Task<Guid> CreateProvinciaAsync(string provincia)
		{
			var op = new WriteOperation(string.Empty);
			op.DataMainRow.Add(ProvinciasD.ProvinciasES.TextoPrincipal, provincia);
			await _client.RunWriteOperationAsync(_currentUser.User, ProvinciasD.ProvinciasES._SectionID, op, true);
			op.EnsureSuccess();
			//_logger.LogInformation("Creada provincia {Provincia} con ID {Id}", provincia, op.WriteOperationResult.RowID);
			return op.WriteOperationResult.RowID;
		}
		private async Task<Guid> CreatePaisAsync(string pais)
		{
			var op = new WriteOperation(string.Empty);
			op.DataMainRow.Add(PaisesD.PaisesES.TextoPrincipal, pais);
			await _client.RunWriteOperationAsync(_currentUser.User, PaisesD.PaisesES._SectionID, op, true);
			op.EnsureSuccess();
			//_logger.LogInformation("Creado país {Pais} con ID {Id}", pais, op.WriteOperationResult.RowID);
			return op.WriteOperationResult.RowID;
		}

		#endregion

		#region Helper genérico -------------------------------------------------------------------

		/// <summary>
		/// Devuelve el ID existente o lo crea si no está, almacenándolo en caché.
		/// </summary>
		private async Task<Guid> GetOrCreateAsync(string cacheKey, Func<Task<Guid>> findAsync, Func<Task<Guid>> createAsync)
		{
			if (_cache.TryGetValue<Guid>(cacheKey, out var cached))
				return cached;

			await _gate.WaitAsync(4000);
			try
			{
				if (_cache.TryGetValue<Guid>(cacheKey, out cached))
					return cached;

				var id = await findAsync();

				if (id == Guid.Empty)
					id = await createAsync();

				_cache.Set(cacheKey, id, TimeSpan.FromHours(12));

				return id;
			}
			finally
			{
				_gate.Release();
			}
		}

		#endregion

	}
}
