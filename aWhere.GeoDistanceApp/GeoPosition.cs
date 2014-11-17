namespace aWhere
{
	using System;
	using System.Device.Location;
	using System.Text.RegularExpressions;

	/// <summary>
	/// This class is primarily used to validate the latitude and longitude and massage the sign of the 
	/// latitude and longitude before they get passed to the distance calculation method.
	/// </summary>
	public class GeoPosition
	{
		public string Name { get; set; }
		public double? Latitude { get; set; }
		public double? Longitude { get; set; }

		public GeoPosition()
		{

		}

		public GeoPosition(double latitude, double longitude, string name = "")
		{
			Name = name;
			this.Longitude = longitude;
			this.Latitude = latitude;
		}

		public GeoPosition(string inputLatitude, string inputLongitude, string name = "")
		{
			this.Name = name;
			this.Latitude = null;
			this.Longitude = null;

			double? latitude = null;
			double? longitude = null;
			double number;

			bool result;

			// strip negative sign and the E S W N characters. negative sign added back later if necessary
			var lat = Utility.StripChars(inputLatitude);
			var lon = Utility.StripChars(inputLongitude);

			// make sure what's left is a number
			result = double.TryParse(lat, out number);
			if (result) latitude = number;

			result = double.TryParse(lon, out number);
			if (result) longitude = number;

			// validate, then correct the sign
			if (latitude != null)
			{
				if (latitude <= 90)
				{
					this.Latitude = GetSign(inputLatitude) * latitude;
				}
			}

			if (longitude != null)
			{
				if (longitude <= 180)
				{
					this.Longitude = GetSign(inputLongitude) * longitude;
				}
			}
		}

		// override the minus operator
		public static double operator -(GeoPosition geoPosition1, GeoPosition geoPosition2)
		{
			var originCoordinate = new GeoCoordinate(
				double.Parse(geoPosition1.Latitude.ToString()),
				double.Parse(geoPosition1.Longitude.ToString()));

			var targetCoordinate = new GeoCoordinate(
				double.Parse(geoPosition2.Latitude.ToString()),
				double.Parse(geoPosition2.Longitude.ToString()));

			return Math.Round(originCoordinate.GetDistanceTo(targetCoordinate) / 1000, 2);
		}

		private static int GetSign(string str)
		{
			// return a sign multiplier
			if (str.Substring(0, 1) == "-") return -1;
			if (str.IndexOf("S", System.StringComparison.Ordinal) > 0) return -1;
			if (str.IndexOf("W", System.StringComparison.Ordinal) > 0) return -1;
			return 1;
		}
	}
}
