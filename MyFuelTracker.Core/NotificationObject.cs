
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MyFuelTracker.Core.Annotations;

namespace MyFuelTracker.Core
{
	
	public class NotificationObject : INotifyPropertyChanged
	{
		public NotificationObject()
		{
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}