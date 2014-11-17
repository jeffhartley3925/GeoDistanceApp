namespace aWhere
{
	using System;

	class HaversineMethod : CalculationMethodBase, ICalculationMethod
	{
		public string Name { get; set; }

		public HaversineMethod()
		{
			Name = "HaversineMethod";
		}

		public double? GetDistance(params double[] args)
		{
			if (args.Length != 2)
			{
				return null;
			}

			var target = new GeoPosition(args[0], args[1]);

			var originLonitude = (double)Origin.Longitude;
			var targetLongitude = (double)target.Longitude;
			var originLatitude = (double)Origin.Latitude;
			var targetLatitude = (double)target.Latitude;

			var theta = originLonitude - targetLongitude;

			var distance =
				Math.Sin(Utility.Degree2Radian(originLatitude)) * Math.Sin(Utility.Degree2Radian(targetLatitude)) +
				Math.Cos(Utility.Degree2Radian(originLatitude)) * Math.Cos(Utility.Degree2Radian(targetLatitude)) * 
				Math.Cos(Utility.Degree2Radian(theta));

			distance = Math.Acos(distance);
			distance = Utility.Radian2Degree(distance);
			distance = Math.Round(distance * 60 * 1.1515 * 1.609344, 2);
			return distance;
		}

	}
}
