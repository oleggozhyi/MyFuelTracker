﻿using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Coding4Fun.Toolkit.Controls;

namespace MyFuelTracker.Infrastructure
{
	public class MyMessageBox : IMessageBox
	{
		public void Error(string message, string title = null)
		{
		    Message(Color.FromArgb(255, 255, 100, 100), message, title);
		}

        public void Info(string message, string title = null)
        {
            Message(Color.FromArgb(255, 100, 200, 255), message, title);
        }

	    private void Message(Color bg, string message, string title = null)
	    {

            var toast = new ToastPrompt
            {
                Title = title,
                Message = message,
                Background = new SolidColorBrush(bg),
                MillisecondsUntilHidden = 2000,
                TextWrapping = TextWrapping.Wrap
            };
            toast.Show();
	    }

	    public bool Confirm(string message, string title = null)
		{
			var messageBoxResult = MessageBox.Show(message, title ?? "confirm", MessageBoxButton.OKCancel);
			return messageBoxResult == MessageBoxResult.OK;
		}
	}
}