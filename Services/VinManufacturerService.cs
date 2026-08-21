namespace VinDecoder.Api.Services
{
	public class VinManufacturerService
	{
		private readonly Dictionary<string, string> _manufacturers = new()
		{
			{ "1HG", "Honda" },
			{ "JHM", "Honda" },

			{ "JTD", "Toyota" },
			{ "JT2", "Toyota" },
			{ "4T1", "Toyota" },

			{ "JS2", "Suzuki" },
			{ "JS3", "Suzuki" },
			{ "JS4", "Suzuki" },

			{ "WVW", "Volkswagen" },
			{ "WBA", "BMW" },
			{ "WDB", "Mercedes-Benz" },
			{ "KMH", "Hyundai" },
			{ "KNA", "Kia" },
			{ "JN1", "Nissan" },
			{ "JM1", "Mazda" },
			{ "JF1", "Subaru" },
			{ "JA3", "Mitsubishi" },
			{ "1FA", "Ford" },
			{ "1G1", "Chevrolet" },
			{ "5YJ", "Tesla" },
			{ "YV1", "Volvo" },
			{ "WAU", "Audi" },
			{ "WP0", "Porsche" },
			{ "VF1", "Renault" },
			{ "VF3", "Peugeot" },
			{ "ZFA", "Fiat" }
		};

		public string GetManufacturer(string vin)
		{
			string wmi = vin.Substring(0, 3);
			if (_manufacturers.TryGetValue(wmi, out string? manufacturer))
			{
				return manufacturer;
			}

			return "unknown";
		}
	}
}
