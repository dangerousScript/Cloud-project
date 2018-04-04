using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contracts;
using System.Diagnostics; // za Process
using System.IO;
using System.Xml;
using System.Timers;

namespace ComputeService
{
	public enum Ports
	{ // enum za portove
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
		public static string DLLfile;
		public static int port1 = 10010;
		public static int port2 = 10020;
		public static int port3 = 10030;
		public static int port4 = 10040;
		#endregion

		static void Main(string[] args)
		{
			Timer timer = new Timer(10000);
			timer.Start();
			timer.Elapsed += T_Elapsed;

			StartContainers();

			// pause
			Console.ReadKey();
		}

		#region methods
		
		/* pokrece 4 containera na odgovarajucim portovima */
		private static void StartContainers()
		{
			int _counter = 0;

			Process.Start("..\\..\\..\\Container\\bin\\Debug\\Container.exe", port1.ToString());
			proxy1 = new ChannelFactory<IContainer>(binding, new EndpointAddress("net.tcp://localhost:" + port1.ToString() + "/Container")).CreateChannel();
			_counter++;

			Process.Start("..\\..\\..\\Container\\bin\\Debug\\Container.exe", port2.ToString());
			proxy2 = new ChannelFactory<IContainer>(binding, new EndpointAddress("net.tcp://localhost:" + port2.ToString() + "/Container")).CreateChannel();
			_counter++;

			Process.Start("..\\..\\..\\Container\\bin\\Debug\\Container.exe", port3.ToString());
			proxy3 = new ChannelFactory<IContainer>(binding, new EndpointAddress("net.tcp://localhost:" + port3.ToString() + "/Container")).CreateChannel();
			_counter++;

			Process.Start("..\\..\\..\\Container\\bin\\Debug\\Container.exe", port4.ToString());
			proxy4 = new ChannelFactory<IContainer>(binding, new EndpointAddress("net.tcp://localhost:" + port4.ToString() + "/Container")).CreateChannel();
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

		private static int ReadXML()
		{
			int _cnt = -1;
			string xmlFile = "";
			string[] files = Directory.GetFiles(_predefinedLocation);

			if (files[0].Substring(_predefinedLocation.Length + 1).Split('.')[1] == "xml")
			{
				xmlFile = files[0];
			}

			if (files[1].Substring(_predefinedLocation.Length + 1).Split('.')[1] == "xml")
			{
				xmlFile = files[1];
			}

			if (xmlFile == "")
			{
				Console.WriteLine("XML fajl nije na lokaciji!");
			}
			else
			{
				using (XmlReader reader = XmlReader.Create(xmlFile))
				{
					try
					{
						while(reader.Read())
						{
							if(reader.Name == "Instances")
							{
								reader.Read();
								int.TryParse(reader.Value, out _cnt);
								break;
							}
						}
					}
					catch
					{
						Console.WriteLine("XML fajl nije ispravan.");
					}
				}
			}

			return _cnt;
		}

		private static string GetDLL()
		{
			string _rtn = "";
			string[] files = Directory.GetFiles(_predefinedLocation);

			if (files[0].Substring(_predefinedLocation.Length).Split('.')[1] == "dll")
			{
				_rtn = files[0].Substring(_predefinedLocation.Length + 1);
			}

			if (files[1].Substring(_predefinedLocation.Length).Split('.')[1] == "dll")
			{
				_rtn = files[1].Substring(_predefinedLocation.Length + 1);
			}

			return _rtn;
		}

		private static void T_Elapsed(object sender, ElapsedEventArgs e)
		{
			string[] files = Directory.GetFiles(_predefinedLocation);

			if(files.Length != 2)
			{
				Console.WriteLine("Paket nije nadjen!");
			}
			else
			{
				int _cnt = ReadXML();

				if(_cnt <= 4 && _cnt >= 0)
				{
					// validan br instanci
					DLLfile = GetDLL();

					if(DLLfile != "")
					{
						// ok
						string source = _predefinedLocation + "\\" + DLLfile;
						string destination = "..\\..\\..\\..\\Container";

						switch(_cnt)
						{
							case 1:
								try
								{
									File.Copy(source, destination + "1\\" + DLLfile, true);
								}
								catch
								{
									Console.WriteLine("Greska prilikom kopiranja DLL fajlova!");
								}
								proxy1.Load(DLLfile);
								break;
							case 2:
								try
								{
									File.Copy(source, destination + "1\\" + DLLfile, true);
									File.Copy(source, destination + "2\\" + DLLfile, true);
								}
								catch
								{
									Console.WriteLine("Greska prilikom kopiranja DLL fajlova!");
								}
								proxy1.Load(DLLfile);
								proxy2.Load(DLLfile);
								break;
							case 3:
								try
								{
									File.Copy(source, destination + "1\\" + DLLfile, true);
									File.Copy(source, destination + "2\\" + DLLfile, true);
									File.Copy(source, destination + "3\\" + DLLfile, true);
								}
								catch
								{
									Console.WriteLine("Greska prilikom kopiranja DLL fajlova!");
								}
								proxy1.Load(DLLfile);
								proxy2.Load(DLLfile);
								proxy3.Load(DLLfile);
								break;
							case 4:
								try
								{
									File.Copy(source, destination + "1\\" + DLLfile, true);
									File.Copy(source, destination + "2\\" + DLLfile, true);
									File.Copy(source, destination + "3\\" + DLLfile, true);
									File.Copy(source, destination + "4\\" + DLLfile, true);
								}
								catch
								{
									Console.WriteLine("Greska prilikom kopiranja DLL fajlova!");
								}
								proxy1.Load(DLLfile);
								proxy2.Load(DLLfile);
								proxy3.Load(DLLfile);
								proxy4.Load(DLLfile);
								break;
							default:
								break;
						}
					}
					else
					{
						// nije nadjen
						Console.WriteLine("DLL Fajl nije nadjen!");
					}
				}
				else
				{
					// greska
					DirectoryInfo directoryInfo = new DirectoryInfo(_predefinedLocation);

					foreach(FileInfo f in directoryInfo.GetFiles())
					{
						f.Delete();
					}

					Console.WriteLine("Nije validan broj instanci. Fajlovi su izbrisani iz predefinisane lokacije!");
				}
			}
		}
		#endregion
	}
}
