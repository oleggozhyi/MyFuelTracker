namespace MyFuelTracker.Infrastructure.UiServices
{
    public interface IProgressIndicatorService
    {
        void AttachIndicatorToView();
        void ShowIndeterminate(string text);
        void Stop();
    }
}