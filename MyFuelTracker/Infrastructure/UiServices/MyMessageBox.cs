﻿using System.Windows;
using System.Windows.Media;
using Coding4Fun.Toolkit.Controls;
using MyFuelTracker.Resources;

namespace MyFuelTracker.Infrastructure.UiServices
{
	public class MyMessageBox : IMessageBox
	{
		public void Error(string message, string title = null)
		{
		    Message(Color.FromArgb(255, 200, 40, 40), message, title);
		}

        public void Info(string message, string title = null)
        {
            Message(Color.FromArgb(255, 0, 80, 200), message, title);
        }

	    private void Message(Color bg, string message, string title = null)
	    {

            var toast = new ToastPrompt
            {
                Title = title,
                Message = message,
                Background = new SolidColorBrush(bg),
                MillisecondsUntilHidden = 2000,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 24
            };
            toast.Show();
	    }

	    public bool Confirm(string message, string title = null)
		{
			var messageBoxResult = MessageBox.Show(message, title ?? AppResources.Confirm, MessageBoxButton.OKCancel);
			return messageBoxResult == MessageBoxResult.OK;
		}
	}
}