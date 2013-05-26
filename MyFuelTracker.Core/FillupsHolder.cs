using System;
using System.Collections.Generic;
using MyFuelTracker.Core.Models;

namespace MyFuelTracker.Core
{
	public class FillupsHolder
	{
		public DateTime Timestamp { get; set; }
		public int Version { get; set; }
		public IEnumerable<Fillup> Fillups { get; set; }
	}
}