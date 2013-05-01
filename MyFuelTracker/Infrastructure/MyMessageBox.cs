using System.Windows;

namespace MyFuelTracker.Infrastructure
{
	public class MyMessageBox : IMessageBox
	{
		public void Show(string message)
		{
			MessageBox.Show(message);
		}
	}
}