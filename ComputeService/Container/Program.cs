using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contracts;

namespace Container
{
	class Program
	{
		#region variables
		private static ServiceHost serviceHost;
		public static IWorkerRole proxy;
		public static NetTcpBinding binding;
		public static int port;
		#endregion


		static void Main(string[] args)
		{
			port = int.Parse(args[0]);

			Console.WriteLine("Container {0}", ((port / 10) % 10));

			serviceHost = new ServiceHost(typeof(Container));
			binding = new NetTcpBinding();
			serviceHost.AddServiceEndpoint(typeof(IContainer), binding, new Uri("net.tcp://localhost:" + port.ToString() + "/Container"));
			serviceHost.Open();

			Console.WriteLine("Povezan sa ComputeService; Port: {0}", port);

			// pause
			Console.ReadKey();
		}
	}
}
