using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
	public class WorkerRole : IWorkerRole
	{
		public void Start(string containerID)
		{
			Console.WriteLine("Kontejner " + containerID + " je pozvao metodu Start()...");
		}

		public void Stop()
		{
			throw new NotImplementedException();
		}
	}
}
