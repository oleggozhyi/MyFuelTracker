using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using Microsoft.Phone.Controls;
using MyFuelTracker.Core;
using MyFuelTracker.Core.DataAccess;
using MyFuelTracker.Core.Models;
using MyFuelTracker.Infrastructure;
using MyFuelTracker.Infrastructure.UiServices;
using MyFuelTracker.ViewModels;

namespace MyFuelTracker
{
	public class Bootstrapper : PhoneBootstrapper
	{
	    public static Bootstrapper Current
	    {
	        get
	        {
	            var bootstrapper = Application.Current.Resources["bootstrapper"] as Bootstrapper;
                if(bootstrapper == null)
                    throw new InvalidOperationException("bootstrapper was not found in app resources");

	            return bootstrapper;
	        }
	    }

        public PhoneContainer Container { get; set; }

	    protected override void Configure()
		{
			Debug.WriteLine("Configure CM Bootstrapper");

			Container = new PhoneContainer(RootFrame);

			Container.RegisterPhoneServices();
			Container.PerRequest<MainViewModel>();
			Container.PerRequest<StatisticsViewModel>();
			Container.PerRequest<HistoryViewModel>();
			Container.PerRequest<EditFillupViewModel>();
			Container.PerRequest<AddFuelTypeViewModel>();
			Container.PerRequest<DisplayFillupViewModel>();
            Container.PerRequest<BackupToSkyDriveViewModel>();
			Container.PerRequest<RestoreFromSkyDriveViewModel>();
			Container.PerRequest<SettingsViewModel>();
            
			Container.PerRequest<ILog, DebugLogger>();
	        Container.Singleton<AppBarMenuModel>();
			Container.Singleton<UserSetttingsManager>();
			Container.Singleton<IFuelEconomyStrategyProvider, FuelEconomyStrategyProvider>();

			Container.Singleton<IMessageBox, MyMessageBox>();
			Container.Singleton<IFillupService, FillupService>();
			Container.Singleton<IFuelTrackerDb, SqlCeFuelTrackerDb>();
			Container.Singleton<IStatisticsService, StatisticsService>();
	        Container.PerRequest<IProgressIndicatorService, ProgressIndicatorService>();
		    Container.Singleton<IFillupsSerializer, FillupsSerializer>();

			LogManager.GetLog = type => new DebugLogger(type);
		}

		protected override void OnUnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
		{
			base.OnUnhandledException(sender, e);
			if(Debugger.IsAttached)
				Debugger.Break(); 
		}

		protected override PhoneApplicationFrame CreatePhoneApplicationFrame()
		{
			return  new TransitionFrame();
		}

		protected override object GetInstance(Type service, string key)
		{
			var instance = Container.GetInstance(service, key);
			if (instance == null)
			{
				MessageBox.Show(service + " is not mapped in IoC container");
				throw new KeyNotFoundException(service + " is not mapped in IoC container");
			}

			return instance;
		}

		protected override IEnumerable<object> GetAllInstances(Type service)
		{
			return Container.GetAllInstances(service);
		}

		protected override void BuildUp(object instance)
		{
			Container.BuildUp(instance);
		}

	    public T Resolve<T>()
	    {
	        return GetAllInstances(typeof (T)).Cast<T>().Single();
	    }
	}
}