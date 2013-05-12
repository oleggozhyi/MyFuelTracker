using System.Windows;

namespace MyFuelTracker.Infrastructure
{
	public class MyMessageBox : IMessageBox
	{
		public void Show(string message)
		{
			MessageBox.Show(message);
		}

		public bool Confirm(string message)
		{
			var messageBoxResult = MessageBox.Show(message, "confirm", MessageBoxButton.OKCancel);
			return messageBoxResult == MessageBoxResult.OK;
		}
	}
}