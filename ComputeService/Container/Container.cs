using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace Container
{
	public class Container : IContainer
	{
		public string Load(string assemblyName)
		{
		    /*
			 * 10010 / 10 = 1001
			 * 1001 % 10 = 1
			 */
			string _Path = Path.GetFullPath("..\\..\\..\\..\\Container" + ((Program.port / 10) % 10).ToString());
			try
			{
				Assembly asm = Assembly.LoadFile(_Path + "\\" + assemblyName);
				if(asm != null)
				{
					Console.WriteLine("Uspesno pokrenut <" + assemblyName + ">");
					IEnumerable<Type> asmb = asm.ExportedTypes;
					string _className = "";

					// getting classname
					foreach (var item in asmb)
					{
						Console.WriteLine("DLL sadrzi: " + item.ToString());
						if(item.ToString().Split('.')[1] == "WorkerRole")
						{
							_className = item.ToString();
						}
					}

					// poziv methode Start iz className
					if(_className.Trim() != "")
					{
						object obj = asm.CreateInstance(_className);
						if(obj != null)
						{
							MethodInfo method = obj.GetType().GetMethod("Start");
							object[] parameters = new object[1];
							parameters[0] = ((Program.port / 10) % 10).ToString();
							method.Invoke(obj, parameters);
						}
					}
					else
					{
						Console.WriteLine("Nije implementiran interface IWorkerRole za novi DLL...");
					}
				}
				else
				{
					Console.WriteLine("Path greska!");
				}
			}
			catch
			{
				Console.WriteLine("Greska pri otvaranju <" + assemblyName + ">");
			}

			return assemblyName;
		}
	}
}
