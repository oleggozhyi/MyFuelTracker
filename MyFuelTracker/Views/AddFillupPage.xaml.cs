using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MyFuelTracker.Core.ViewModels;
using MyFuelTracker.Core;

namespace MyFuelTracker.Views
{
	public partial class AddFillupPage : PhoneApplicationPage
	{
		public EditFillupViewModel ViewModel
		{
			get { return (EditFillupViewModel)DataContext; }
			set { DataContext = value; }
		}

		public AddFillupPage()
		{
			InitializeComponent();
			gasTypes.ItemsSource = new[] { "Okko Pulls", "Okko 95", "Wog Mustang" };
			ViewModel = new EditFillupViewModel((IMyFuelTrackerApp)App.Current);
		}

		private void OnSaveButtonClick(object sender, EventArgs e)
		{
			ViewModel.Save();
		}
		private void OnCancelButtonClick(object sender, EventArgs e)
		{
			ViewModel.Cancel();
		}
	}
}