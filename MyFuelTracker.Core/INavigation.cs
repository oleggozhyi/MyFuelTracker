using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyFuelTracker.Core
{
	public interface INavigator
	{
		void Navigate(string relativeUri);
	}

	public class Navigator : INavigator
	{
		private PhoneApplicationFrame _rootFrame; 

		public PhoneApplicationFrame RootFrame
		{
			get
			{
				if (_rootFrame == null)
				{
					_rootFrame = ((PhoneApplicationFrame)Application.Current.RootVisual);
				}
				return _rootFrame;
			}
		}

		public void Navigate(string relativeUri)
		{
			RootFrame.Navigate(new Uri(relativeUri, UriKind.Relative));
		}
	}
}
