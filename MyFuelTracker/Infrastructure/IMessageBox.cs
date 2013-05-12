namespace MyFuelTracker.Infrastructure
{
	public interface IMessageBox
	{
		void Show(string message, string title = null);
		bool Confirm(string message, string title = null);
	}
}
