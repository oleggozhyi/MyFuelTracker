namespace MyFuelTracker.Infrastructure
{
	public interface IMessageBox
	{
		void Show(string message);
		bool Confirm(string message);
	}
}
