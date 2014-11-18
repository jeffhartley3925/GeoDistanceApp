using Ninject.Modules;

namespace aWhere
{
	public class Bindings : NinjectModule
	{
		public override void Load()
		{
			// configure IMailSender and ILogging to resolve to their specified concrete implementations
			Bind<ICalculationMethod>().To<GeoCoordinateMethod>();
		}
	}
}
