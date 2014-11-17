namespace aWhere
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Reflection;

	using Ninject;

	class DistanceFromOrigin
	{
		static void Main(string[] args)
		{
			int? errorCode;
			double? geoDistance;
			GeoPosition geoPosition;
			string inputName = "", inputLatitude = "", inputLongitude = "";

			var geoDistanceResults = new List<GeoDistanceResult>();
			var errorResults = new List<string>();

			var calculationMethod = CalculationMethod;

			using (var file = new StreamReader("../../input.txt"))
			{
				string line;
				while ((line = file.ReadLine()) != null)
				{
					errorCode = null;
					var geoData = line.Split(',');
					if (geoData.Length != 3) errorCode = 0;
					if (errorCode == null)
					{
						inputName = geoData[0].Trim();
						inputLatitude = geoData[1].Trim().ToUpper();
						inputLongitude = geoData[2].Trim().ToUpper();
						if (inputName == string.Empty) errorCode = 1;
						if (inputLatitude == string.Empty) errorCode = 4;
						if (inputLongitude == string.Empty) errorCode = 5;
					}
					geoPosition = null;

					if (errorCode == null)
					{
						geoPosition = new GeoPosition(inputLatitude, inputLongitude, inputName);
						if (geoPosition.Latitude == null) errorCode = 2;
						if (geoPosition.Longitude == null) errorCode = 3;
					}
					geoDistance = null;
					if (errorCode == null)
					{
						geoDistance = calculationMethod.GetDistance((double)geoPosition.Latitude, (double)geoPosition.Longitude);
						if (geoDistance == null) errorCode = 6;
					}
					if (errorCode == null)
					{
						//Console.WriteLine("{0} {1} {2} {3} Kilometers", geoPosition.Name, (double)geoPosition.Latitude, (double)geoPosition.Longitude, geoDistance);
						var geoDistanceResult = new GeoDistanceResult
																  {
																	  Name = inputName,
																	  Latitude = inputLatitude,
																	  Longitude = inputLongitude,
																	  Distance = (double)geoDistance
																  };
						geoDistanceResults.Add(geoDistanceResult);
					}
					else
					{
						var errorResult = HandleError(errorCode, geoData);
						errorResults.Add(errorResult);
						Console.WriteLine(errorResult);
					}
				}
			}
			var results = geoDistanceResults.OrderBy(p => p.Distance);
			WriteOutput(calculationMethod.Name, results, errorResults);

			Console.WriteLine(Environment.NewLine);
			Console.WriteLine("Done");
		}

		private static ICalculationMethod CalculationMethod
		{
			get
			{
				IKernel kernel = new StandardKernel();
				kernel.Load(Assembly.GetExecutingAssembly());
				var calculationMethod = kernel.Get<ICalculationMethod>();
				return calculationMethod;
			}
		}

		private static void WriteOutput(string calculationMethodName , IEnumerable<GeoDistanceResult> results, IEnumerable<string> errorResults)
		{
			var outputPath = GetOutputPath(calculationMethodName);
			using (var outFile = new StreamWriter(outputPath))
			{
				foreach (var result in results)
				{
					outFile.WriteLine("{0},{1},{2},{3} kilometers", result.Name, result.Latitude, result.Longitude, result.Distance);
				}
				foreach (var result in errorResults)
				{
					outFile.WriteLine(result);
				}
			}
		}

		private static string HandleError(int? errorCode, string[] geoData)
		{
			string errorMessage = "";
			switch (errorCode)
			{
				case 0:
					errorMessage = "Incorrect number of parameters";
					break;
				case 1:
					errorMessage = "Location parameter is missing";
					break;
				case 2:
					errorMessage = "Latitude is invalid";
					break;
				case 3:
					errorMessage = "Longitude is invalid";
					break;
				case 4:
					errorMessage = "Latitude is missing";
					break;
				case 5:
					errorMessage = "Longitude is missing";
					break;
				case 6:
					errorMessage = "Distance calculation failed";
					break;
			}
			var errorResult = string.Format("ERROR: {0} - Input: {1} {2} {3}", errorMessage, geoData[0], geoData[1], geoData[2]);
			return errorResult;
		}

		private static string GetOutputPath(string calculationMethodName)
		{
			var now = DateTime.Now;
			var timeStamp = string.Format(
				"{0}-{1}-{2} {3}-{4}-{5}",
				now.Day,
				now.Month,
				now.Year,
				now.Hour,
				now.Minute,
				now.Second);
			var outputPath = string.Format("../../output/{0}_{1}.txt", calculationMethodName, timeStamp);
			return outputPath;
		}
	}
}