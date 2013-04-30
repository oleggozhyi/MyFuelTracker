using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace MyFuelTracker.Core.ViewModels
{
	public class SummaryViewModel : NotificationObject
	{
		#region Fields

		private INavigator _navigator;
		private decimal _avgConsumption;
		private decimal _minConsumption;
		private decimal _maxConsumption;

		#endregion

		#region ctor

		public SummaryViewModel()
			: this(new Navigator())
		{ }

		public SummaryViewModel(INavigator navigator)
		{
			_navigator = navigator;
			AddFillupCommand = new RelayCommand(_ => _navigator.Navigate("/Views/AddFillupPage.xaml"));
		}

		#endregion

		#region Properties

		public decimal AvgConsumption
		{
			get { return _avgConsumption; }
			set
			{
				if (_avgConsumption != value)
				{
					_avgConsumption = value;
					OnPropertyChanged();
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
					OnPropertyChanged();
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
					OnPropertyChanged();
				}
			}
		}

		#endregion

		#region Commands

		public ICommand AddFillupCommand { get; set; }

		#endregion
	}
}
