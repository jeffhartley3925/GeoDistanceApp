namespace aWhere
{
	using System;
	using System.Text.RegularExpressions;

	public static class Utility
	{
		public static string StripChars(string str)
		{
			// remove negative sign and all alpha numeric characters
			var pattern = "[-A-Za-z]";
			var rgx = new Regex(pattern);
			return rgx.Replace(str, "");
		}

		public static double Degree2Radian(double degree)
		{
			return (degree * Math.PI / 180.0);
		}

		public static double Radian2Degree(double radian)
		{
			return (radian / Math.PI * 180.0);
		}
	}
}
