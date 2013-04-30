using MyFuelTracker.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFuelTracker.Core.ViewModels
{
	public class FillupViewModel : ViewModelBase
	{
		#region Fields

		private DateTime _date;
		private decimal _volume;
		private decimal _price;
		private decimal _odometer;
		private INavigator _navigator;

		#endregion

		#region ctor

		public FillupViewModel()
			: this(new Navigator())
		{ }

		public FillupViewModel(INavigator navigator)
		{
			_navigator = navigator;
		}

		#endregion

		#region Properties

		public DateTime Date
		{
			get { return _date; }
			set { SetProperty(value, ref _date); }
		}

		public decimal Volume
		{
			get { return _volume; }
			set { SetProperty(value, ref _volume); }
		}

		public decimal Price
		{
			get { return _price; }
			set { SetProperty(value, ref _price); }
		}

		public decimal Odometer
		{
			get { return _odometer; }
			set { SetProperty(value, ref _odometer); }
		}

		#endregion

		#region methods

		public void Save()
		{
			//TODO:
			_navigator.GoBack();
		}

		public void Cancel()
		{
			_navigator.GoBack();
		}
		#endregion

	}
}
