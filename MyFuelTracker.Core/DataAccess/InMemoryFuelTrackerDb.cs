using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using MyFuelTracker.Core.Models;

namespace MyFuelTracker.Core.DataAccess
{
	public class InMemoryFuelTrackerDb : IFuelTrackerDb
	{
		private readonly Task _loadHistoryTask;

		public InMemoryFuelTrackerDb(bool loaded)
		{
			_loadHistoryTask = LoadHistoryAsync();
		}

		public InMemoryFuelTrackerDb()
			: this(false)
		{
		}

		private bool _loaded;
		private object _locker = new object();
		public List<Fillup> Fillups = new List<Fillup>();

		public async Task SaveFillupAsync(Fillup fillup)
		{
			if (!_loadHistoryTask.IsCompleted)
				await _loadHistoryTask;

			await Task.Factory.StartNew(() => Fillups.Add(fillup));
		}

		public async Task<Fillup[]> LoadAllFillupsAsync()
		{
			if (!_loadHistoryTask.IsCompleted)
				await _loadHistoryTask;

			return await Task.FromResult(Fillups.ToArray());
		}

		private static DateTime ParseDate(string date)
		{
			return DateTime.Parse(date, CultureInfo.CurrentCulture);
		}

		private Task LoadHistoryAsync()
		{
			return Task.Factory.StartNew(() =>
			{
				using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MyFuelTracker.Core.DataAccess.fillups.xml"))
				{
					var xDocument = XDocument.Load(stream);
					lock (_locker)
					{
						if (_loaded)
							return;
						foreach (var f in xDocument.Root.Elements("fillup"))
						{
							var fillup = new Fillup();
							Fillups.Add(fillup);

							fillup.Date = ParseDate(f.Element("date").Value);
							fillup.Volume = double.Parse(f.Element("volume").Value, CultureInfo.CurrentCulture);
							fillup.Price = double.Parse(f.Element("price").Value, CultureInfo.CurrentCulture);
							fillup.IsPartial = bool.Parse(f.Element("is-partial").Value);
							fillup.OdometerStart = double.Parse(f.Element("odometer-start").Value, CultureInfo.CurrentCulture);
							fillup.OdometerEnd = double.Parse(f.Element("odometer-end").Value, CultureInfo.CurrentCulture);
						}
						Fillups.Reverse();
					}
				}
			});
		}
	}
}