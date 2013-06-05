using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Caliburn.Micro;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MyFuelTracker.Infrastructure;

namespace MyFuelTracker.Views
{
	public partial class HistoryView : UserControl, ISelection
	{
		public HistoryView()
		{
			InitializeComponent();
		}

		public IList SelectedItems
		{
			get { return (IList)GetValue(SelectedItemsProperty); }
			set { SetValue(SelectedItemsProperty, value); }
		}

		public event EventHandler SelectionChanged;

		protected virtual void OnSelectionChanged()
		{
			var handler = SelectionChanged;
			if (handler != null) handler(this, EventArgs.Empty);
		}

		public static readonly DependencyProperty SelectedItemsProperty =
			DependencyProperty.Register("SelectedItems", typeof(IList), typeof(HistoryView), new PropertyMetadata(new object[0]));

		
		private void FillupsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SelectedItems = FillupsList.SelectedItems;
			OnSelectionChanged();
		}
	}

	public interface ISelection
	{
		IList SelectedItems { get; }
		event EventHandler SelectionChanged;
	}
}