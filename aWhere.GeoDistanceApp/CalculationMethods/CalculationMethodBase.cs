namespace aWhere
{
	public class CalculationMethodBase
	{
		public CalculationMethodBase()
		{
			Error = "";
		}

		public const double R = 6371.0; // mean earth radius in kilometers

		public GeoPosition Origin = new GeoPosition(0, 0, "Origin");

		public string Error { get; set; }

	}
}
