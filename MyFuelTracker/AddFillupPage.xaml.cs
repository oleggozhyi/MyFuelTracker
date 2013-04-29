using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MyFuelTracker
{
	public partial class AddFillupPage : PhoneApplicationPage
	{
		public AddFillupPage()
		{
			InitializeComponent();
			gasTypes.ItemsSource = new[] { "Okko Pulls", "Okko 95", "Wog Mustang" };
		}
	}
}