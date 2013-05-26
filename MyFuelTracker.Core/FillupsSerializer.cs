using System;
using System.Collections.Generic;
using MyFuelTracker.Core.Models;
using Newtonsoft.Json;

namespace MyFuelTracker.Core
{
	public class FillupsSerializer : IFillupsSerializer
	{
		public const int VERSION = 1;

		public string Serialize(IEnumerable<Fillup> fillups)
		{
			var fillupsHolder = new FillupsHolder {Version = VERSION, Timestamp = DateTime.Now, Fillups = fillups};
			string serializeObject = JsonConvert.SerializeObject(fillupsHolder, Formatting.Indented);
			return serializeObject;
		}

		public FillupsHolder Deserialize(string fillups)
		{
			var deserializeObject = JsonConvert.DeserializeObject<FillupsHolder>(fillups);
			return deserializeObject;
		}
	}
}