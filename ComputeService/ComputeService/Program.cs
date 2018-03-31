using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contracts;
using System.Diagnostics; // za Process

namespace ComputeService
{
	public enum Ports { // enum za portove
		port1 = 10010,
		port2 = 10020,
		port3 = 10030,
		port4 = 10040
	};

	class Program
	{
		#region variables
		private static string _predefinedLocation = "..\\..\\..\\..\\PredefinedLocation";
		private static NetTcpBinding binding = new NetTcpBinding();
		public static IContainer proxy1, proxy2, proxy3, proxy4;
		#endregion

		static void Main(string[] args)
		{
			StartContainers();


			// pause
			Console.ReadKey();
		}

		#region methods
		
		/* pokrece 4 containera na odgovarajucim portovima */
		private static void StartContainers()
		{
			int _counter = 0;

			Process.Start("..\\..\\..\\Container\\bin\\Debug\\Container.exe", Ports.port1.ToString());
			proxy1 = new ChannelFactory<IContainer>(binding, new EndpointAddress("net.tcp://localhost:" + Ports.port1.ToString() + "/Container")).CreateChannel();
			_counter++;

			Process.Start("..\\..\\..\\Container\\bin\\Debug\\Container.exe", Ports.port2.ToString());
			proxy2 = new ChannelFactory<IContainer>(binding, new EndpointAddress("net.tcp://localhost:" + Ports.port2.ToString() + "/Container")).CreateChannel();
			_counter++;

			Process.Start("..\\..\\..\\Container\\bin\\Debug\\Container.exe", Ports.port1.ToString());
			proxy3 = new ChannelFactory<IContainer>(binding, new EndpointAddress("net.tcp://localhost:" + Ports.port3.ToString() + "/Container")).CreateChannel();
			_counter++;

			Process.Start("..\\..\\..\\Container\\bin\\Debug\\Container.exe", Ports.port1.ToString());
			proxy4 = new ChannelFactory<IContainer>(binding, new EndpointAddress("net.tcp://localhost:" + Ports.port4.ToString() + "/Container")).CreateChannel();
			_counter++;

			if(_counter == 4)
			{
				Console.WriteLine("All " + _counter.ToString() + " containers started!");
			}
			else
			{
				Console.WriteLine("Error while starting all containers. CNT: {0}", _counter);
			}
		}

		#endregion
	}
}
