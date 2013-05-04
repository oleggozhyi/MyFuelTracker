using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using Caliburn.Micro.BindableAppBar;
using Microsoft.Phone.Controls;
using MyFuelTracker.Core;
using MyFuelTracker.Core.DataAccess;
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
			container.Singleton<IFillupService, FillupService>();
			container.Singleton<IFuelTrackerDb, InMemoryFuelTrackerDb>();
			container.Singleton<IStatisticsService, StatisticsService>();

			LogManager.GetLog = type => new DebugLogger(type);

			AddCustomConventions();
		}

		protected override PhoneApplicationFrame CreatePhoneApplicationFrame()
		{
			return  new TransitionFrame();
		}

		static void AddCustomConventions()
		{
			ConventionManager.AddElementConvention<BindableAppBarButton>(Control.IsEnabledProperty, "DataContext", "Click");
			ConventionManager.AddElementConvention<BindableAppBarMenuItem>(Control.IsEnabledProperty, "DataContext", "Click");
		}

		protected override object GetInstance(Type service, string key)
		{
			var instance = container.GetInstance(service, key);
			if (instance == null)
			{
				MessageBox.Show(service + " is not mapped in IoC container");
				throw new KeyNotFoundException(service + " is not mapped in IoC container");
			}

			return instance;
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