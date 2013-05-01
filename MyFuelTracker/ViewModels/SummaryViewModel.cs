using System;
using System.Diagnostics;
using Caliburn.Micro;
using MyFuelTracker.Core;

namespace MyFuelTracker.ViewModels
{
	public class SummaryViewModel : Screen
	{
		#region Fields

		private decimal _avgConsumption;
		private decimal _minConsumption;
		private decimal _maxConsumption;

		#endregion

		#region ctor

		public SummaryViewModel()
		{
			Debug.WriteLine("SummaryViewModel created");
			//For designer support
		}

		#endregion

		#region Properties

		public string DisplayName { get { return "summary"; } }

		public decimal AvgConsumption
		{
			get { return _avgConsumption; }
			set
			{
				if (_avgConsumption != value)
				{
					_avgConsumption = value;
					NotifyOfPropertyChange(() => AvgConsumption);
				}
			}
		}

		public decimal MinConsumption
		{
			get { return _minConsumption; }
			set
			{
				if (_minConsumption != value)
				{
					_minConsumption = value;
					NotifyOfPropertyChange(() => MinConsumption);
				}
			}
		}

		public decimal MaxConsumption
		{
			get { return _maxConsumption; }
			set
			{
				if (_maxConsumption != value)
				{
					_maxConsumption = value;
					NotifyOfPropertyChange(() => MaxConsumption);

				}
			}
		}

		#endregion
	}
}
