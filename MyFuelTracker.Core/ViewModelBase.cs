
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MyFuelTracker.Core.Annotations;
using System.Collections.Generic;

namespace MyFuelTracker.Core
{
	public class ViewModelBase : INotifyPropertyChanged
	{
		public ViewModelBase()
		{
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}

		protected void SetProperty<T>(T value, ref T fieldToSet,  [CallerMemberName] string propertyName = null)
		{
			if (!EqualityComparer<T>.Default.Equals(value, fieldToSet))
			{
				fieldToSet = value;
				OnPropertyChanged(propertyName);
			}
		}
	}
}