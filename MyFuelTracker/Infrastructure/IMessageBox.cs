namespace MyFuelTracker.Infrastructure
{
	public interface IMessageBox
	{
		void Info(string message, string title = null);
        void Error(string message, string title = null);
		bool Confirm(string message, string title = null);
	}
}
