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
		public InMemoryFuelTrackerDb(bool loaded)
		{
			_loaded = loaded;
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
			if (!_loaded)
				await LoadTestDataAsync();

			await Task.Factory.StartNew(() => Fillups.Add(fillup));
		}

		public async Task<Fillup[]> LoadAllFillupsAsync()
		{
			if (!_loaded)
				await LoadTestDataAsync();
			return await Task.FromResult(Fillups.ToArray());
		}

		private async Task LoadTestDataAsync()
		{
			await Task.Factory.StartNew(() =>
			{
				using (var stream = Assembly.GetExecutingAssembly()
											.GetManifestResourceStream("MyFuelTracker.Core.DataAccess.fillups.xml"))
				{
					var xDocument = XDocument.Load(stream);
					lock (_locker)
					{

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
			_loaded = true;
		}

		private static DateTime ParseDate(string date)
		{
			return DateTime.Parse(date, CultureInfo.CurrentCulture);
		}
	}
}