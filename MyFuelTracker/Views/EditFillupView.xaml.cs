using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using Microsoft.Phone.Controls;

namespace MyFuelTracker.Views
{
	public partial class EditFillupView : PhoneApplicationPage
	{
		public EditFillupView()
		{
			InitializeComponent();
		}

		private void EditFillupView_OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			OdometerEnd.Focus();
			int selectionStart = OdometerEnd.Text.Length - 3;
			selectionStart = selectionStart < 0 ? 0 : selectionStart;
			int selectionLength = OdometerEnd.Text.Length >= 3 ? 3 : OdometerEnd.Text.Length;
			OdometerEnd.SelectionStart = selectionStart;
			OdometerEnd.SelectionLength = selectionLength;
		}
	}
}