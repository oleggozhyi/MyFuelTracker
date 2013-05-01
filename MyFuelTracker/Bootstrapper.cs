using System;
using System.Collections.Generic;
using System.Diagnostics;
using Caliburn.Micro;
using Microsoft.Phone.Controls;
using MyFuelTracker.Infrastructure;
using MyFuelTracker.ViewModels;

namespace MyFuelTracker
{
	public class Bootstrapper : PhoneBootstrapper
	{
		PhoneContainer container;

		protected override void Configure()
		{
			Debug.WriteLine("Configure CM Bootstrapper");

			container = new PhoneContainer(RootFrame);

			container.RegisterPhoneServices();
			container.PerRequest<MainViewModel>();
			container.PerRequest<SummaryViewModel>();
			container.PerRequest<HistoryViewModel>();
			container.PerRequest<EditFillupViewModel>();

			container.PerRequest<ILog, DebugLogger>();
			container.Singleton<IMessageBox, MyMessageBox>();

			LogManager.GetLog = type => new DebugLogger(type);

			AddCustomConventions();
		}

		protected override PhoneApplicationFrame CreatePhoneApplicationFrame()
		{
			return  new TransitionFrame();
		}

		static void AddCustomConventions()
		{
			//ellided  
		}

		protected override object GetInstance(Type service, string key)
		{
			return container.GetInstance(service, key);
		}

		protected override IEnumerable<object> GetAllInstances(Type service)
		{
			return container.GetAllInstances(service);
		}

		protected override void BuildUp(object instance)
		{
			container.BuildUp(instance);
		}
	}
}