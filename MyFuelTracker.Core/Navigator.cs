using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyFuelTracker.Core
{
	public class Navigator : INavigator
	{
		private PhoneApplicationFrame _rootFrame;

		public Navigator(PhoneApplicationFrame rootFrame)
		{
			_rootFrame = rootFrame;
		}

		public void Navigate(string relativeUri)
		{
			_rootFrame.Navigate(new Uri(relativeUri, UriKind.Relative));
		}

		public void GoBack()
		{
			_rootFrame.GoBack();
		}
	}
}
