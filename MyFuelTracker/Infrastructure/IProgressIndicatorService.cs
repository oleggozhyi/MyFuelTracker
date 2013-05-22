namespace MyFuelTracker.Infrastructure
{
    public interface IProgressIndicatorService
    {
        void AttachIndicatorToView();
        void ShowIndeterminate(string text);
        void Stop();
    }
}