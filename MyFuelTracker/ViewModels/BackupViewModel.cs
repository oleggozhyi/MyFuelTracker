using System.Linq;
using System.Windows;
using MyFuelTracker.Core;
using MyFuelTracker.Infrastructure;
using MyFuelTracker.Infrastructure.Helpers;

namespace MyFuelTracker.ViewModels
{
	public class BackupViewModel
	{
		public string BackupDate
		{
			get { return FillupsHolder.Timestamp.ToString("dd MMM yyyy HH:mm:ss"); }
		}

		public string LastFillupDate
		{
			get { return FillupsHolder.Fillups.First().Date.ToString("dd MMM yyyy"); }
		}

		public string LastOdometer
		{
			get { return FillupsHolder.Fillups.First().OdometerEnd.FormatForDisplay(0); }
		}
		public FillupsHolder FillupsHolder { get; set; }
	}
}