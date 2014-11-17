namespace aWhere
{
	interface ICalculationMethod
	{
		string Name { get; set; }
		double? GetDistance(params double[] args);
	}
}
