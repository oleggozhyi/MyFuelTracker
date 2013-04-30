namespace MyFuelTracker.Core
{
	public interface INavigator
	{
		void Navigate(string relativeUri);
		void GoBack();
	}
}
