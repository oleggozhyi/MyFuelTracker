using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Phone.Shell;

namespace MyFuelTracker.Infrastructure
{
    public class ProgressIndicatorService : IProgressIndicatorService
    {

        public void AttachIndicatorToView()
        {
            SystemTray.ProgressIndicator = new ProgressIndicator(); 
        }

        public void ShowIndeterminate(string text)
        {
            if(SystemTray.ProgressIndicator == null)
                return;
            
            SystemTray.ProgressIndicator.Text = text;
            SystemTray.ProgressIndicator.IsIndeterminate = true;
            SystemTray.ProgressIndicator.IsVisible = true;
        }

        public void Stop()
        {
            if (SystemTray.ProgressIndicator == null)
                return;

            SystemTray.ProgressIndicator.IsVisible = false;
        }
    }
}
