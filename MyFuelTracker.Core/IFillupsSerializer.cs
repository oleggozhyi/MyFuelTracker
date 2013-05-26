using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFuelTracker.Core.Models;

namespace MyFuelTracker.Core
{
	public interface IFillupsSerializer
	{
		string Serialize(IEnumerable<Fillup> fillups);
		FillupsHolder Deserialize(string fillups);
	}
}
