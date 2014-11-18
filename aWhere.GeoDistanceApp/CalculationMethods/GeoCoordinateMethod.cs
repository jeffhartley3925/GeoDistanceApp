namespace aWhere
{
	/// <summary>
	/// GeoCoordinateMethod uses the System.Device.Location.GeoCoordinate namespace GetDistanceTo method (See GetPosition 
	/// class where the minus operator is overloaded.) It uses the Haversine method, but you don't have to know what that 
	/// is in order to use this method. It makes things simpler, but it might be considered cheating, so I also implemented 
	/// the Haversine method explicitly.
	/// </summary>
	class GeoCoordinateMethod : CalculationMethodBase, ICalculationMethod
	{
		public string Name { get; set; }

		public GeoCoordinateMethod()
		{
			Name = "GeoCoordinateMethod";
		}

		public double? GetDistance(params double[] args)
		{
			if (args.Length != 2)
			{
				return null;
			}
			var Target = new GeoPosition(args[0], args[1]);

			// GetPosition overrides the minus operator, the logic is there.
			return Origin - Target;
		}
	}
}
