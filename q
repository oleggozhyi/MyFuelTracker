[1mdiff --git a/MyFuelTracker/AddFillupPage.xaml b/MyFuelTracker/AddFillupPage.xaml[m
[1mdeleted file mode 100644[m
[1mindex a44ce26..0000000[m
[1m--- a/MyFuelTracker/AddFillupPage.xaml[m
[1m+++ /dev/null[m
[36m@@ -1,82 +0,0 @@[m
[31m-﻿<phone:PhoneApplicationPage[m
[31m-    x:Class="MyFuelTracker.AddFillupPage"[m
[31m-    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"[m
[31m-    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"[m
[31m-     xmlns:col="clr-namespace:System.Collections;assembly=System.Collections.Generic" [m
[31m-    xmlns:phone="clr-namespace:Microsoft.Phone.Controls;assembly=Microsoft.Phone"[m
[31m-    xmlns:shell="clr-namespace:Microsoft.Phone.Shell;assembly=Microsoft.Phone"[m
[31m-    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"[m
[31m-    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"[m
[31m-     xmlns:toolkit="clr-namespace:Microsoft.Phone.Controls;assembly=Microsoft.Phone.Controls.Toolkit"[m
[31m-    FontFamily="{StaticResource PhoneFontFamilyNormal}"[m
[31m-    FontSize="{StaticResource PhoneFontSizeNormal}"[m
[31m-    Foreground="{StaticResource PhoneForegroundBrush}"[m
[31m-    SupportedOrientations="Portrait" Orientation="Portrait"[m
[31m-    mc:Ignorable="d"[m
[31m-    shell:SystemTray.IsVisible="True">[m
[31m-    <phone:PhoneApplicationPage.Resources>[m
[31m-    </phone:PhoneApplicationPage.Resources>[m
[31m-	<phone:PhoneApplicationPage.ApplicationBar>[m
[31m-		<shell:ApplicationBar>[m
[31m-			<shell:ApplicationBarIconButton IconUri="/Assets/AppBar/save.png" IsEnabled="True" Text="Save"/>[m
[31m-			<shell:ApplicationBarIconButton IconUri="/Toolkit.Content/ApplicationBar.Cancel.png" IsEnabled="True" Text="Cancel"/>[m
[31m-		</shell:ApplicationBar>[m
[31m-	</phone:PhoneApplicationPage.ApplicationBar>[m
[31m-    <toolkit:TransitionService.NavigationInTransition>[m
[31m-        <toolkit:NavigationInTransition>[m
[31m-            <toolkit:NavigationInTransition.Backward>[m
[31m-                <toolkit:RotateTransition Mode="In180Clockwise"/>[m
[31m-            </toolkit:NavigationInTransition.Backward>[m
[31m-            <toolkit:NavigationInTransition.Forward>[m
[31m-                <toolkit:RotateTransition Mode="In180Counterclockwise"/>[m
[31m-            </toolkit:NavigationInTransition.Forward>[m
[31m-        </toolkit:NavigationInTransition>[m
[31m-    </toolkit:TransitionService.NavigationInTransition>[m
[31m-    <toolkit:TransitionService.NavigationOutTransition>[m
[31m-        <toolkit:NavigationOutTransition>[m
[31m-            <toolkit:NavigationOutTransition.Backward>[m
[31m-                <toolkit:RotateTransition Mode="Out180Clockwise"/>[m
[31m-            </toolkit:NavigationOutTransition.Backward>[m
[31m-            <toolkit:NavigationOutTransition.Forward>[m
[31m-                <toolkit:RotateTransition Mode="Out180Counterclockwise"/>[m
[31m-            </toolkit:NavigationOutTransition.Forward>[m
[31m-        </toolkit:NavigationOutTransition>[m
[31m-    </toolkit:TransitionService.NavigationOutTransition>[m
[31m-[m
[31m-    <!--LayoutRoot is the root grid where all page content is placed-->[m
[31m-    <Grid x:Name="LayoutRoot" Background="Transparent">[m
[31m-        <Grid.RowDefinitions>[m
[31m-            <RowDefinition Height="Auto"/>[m
[31m-            <RowDefinition Height="*"/>[m
[31m-        </Grid.RowDefinitions>[m
[31m-[m
[31m-        <!--TitlePanel contains the name of the application and page title-->[m
[31m-        <StackPanel Grid.Row="0" Margin="12,17,0,28">[m
[31m-            <TextBlock Text="My Fuel Tracker" Style="{StaticResource PhoneTextNormalStyle}"/>[m
[31m-            <TextBlock Text="add fillup" Margin="9,-7,0,0" Style="{StaticResource PageTitle}"/>[m
[31m-        </StackPanel>[m
[31m-[m
[31m-        <!--ContentPanel - place additional content here-->[m
[31m-        <Grid x:Name="ContentPanel" Grid.Row="1" Margin="12,0,12,0">[m
[31m-        	<Grid.RowDefinitions>[m
[31m-        		<RowDefinition Height="Auto"/>[m
[31m-        		<RowDefinition Height="Auto"/>[m
[31m-        		<RowDefinition Height="Auto"/>[m
[31m-        		<RowDefinition Height="Auto"/>[m
[31m-        		<RowDefinition Height="Auto"/>[m
[31m-        		<RowDefinition Height="Auto"/>[m
[31m-        	</Grid.RowDefinitions>[m
[31m-        	<toolkit:DatePicker VerticalAlignment="Top"/>[m
[31m-               [m
[31m-        	<toolkit:PhoneTextBox TextWrapping="Wrap" VerticalAlignment="Top" Hint="Odometer" Grid.Row="1"/>[m
[31m-        	<toolkit:PhoneTextBox TextWrapping="Wrap" VerticalAlignment="Top" Hint="Volume" Grid.Row="2" InputScope="Number"/>[m
[31m-        	<toolkit:PhoneTextBox TextWrapping="Wrap" VerticalAlignment="Top" Hint="Price" Grid.Row="3" InputScope="Number"/>[m
[31m-        	<toolkit:ListPicker Grid.Row="4" VerticalContentAlignment="Stretch" VerticalAlignment="Center" Name="gasTypes">[m
[31m-               [m
[31m-        	</toolkit:ListPicker>[m
[31m-        	<CheckBox Content="Is partial fillup" Grid.Row="5"/>[m
[31m-               [m
[31m-        </Grid>[m
[31m-    </Grid>[m
[31m-[m
[31m-</phone:PhoneApplicationPage>[m
\ No newline at end of file[m
[1mdiff --git a/MyFuelTracker/AddFillupPage.xaml.cs b/MyFuelTracker/AddFillupPage.xaml.cs[m
[1mdeleted file mode 100644[m
[1mindex 0e0394d..0000000[m
[1m--- a/MyFuelTracker/AddFillupPage.xaml.cs[m
[1m+++ /dev/null[m
[36m@@ -1,21 +0,0 @@[m
[31m-﻿using System;[m
[31m-using System.Collections.Generic;[m
[31m-using System.Linq;[m
[31m-using System.Net;[m
[31m-using System.Windows;[m
[31m-using System.Windows.Controls;[m
[31m-using System.Windows.Navigation;[m
[31m-using Microsoft.Phone.Controls;[m
[31m-using Microsoft.Phone.Shell;[m
[31m-[m
[31m-namespace MyFuelTracker[m
[31m-{[m
[31m-	public partial class AddFillupPage : PhoneApplicationPage[m
[31m-	{[m
[31m-		public AddFillupPage()[m
[31m-		{[m
[31m-			InitializeComponent();[m
[31m-			gasTypes.ItemsSource = new[] { "Okko Pulls", "Okko 95", "Wog Mustang" };[m
[31m-		}[m
[31m-	}[m
[31m-}[m
\ No newline at end of file[m
[1mdiff --git a/MyFuelTracker/MainPage.xaml b/MyFuelTracker/MainPage.xaml[m
[1mdeleted file mode 100644[m
[1mindex 6d8a3b2..0000000[m
[1m--- a/MyFuelTracker/MainPage.xaml[m
[1m+++ /dev/null[m
[36m@@ -1,82 +0,0 @@[m
[31m-﻿<phone:PhoneApplicationPage[m
[31m-    x:Class="MyFuelTracker.MainPage"[m
[31m-    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"[m
[31m-    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"[m
[31m-    xmlns:phone="clr-namespace:Microsoft.Phone.Controls;assembly=Microsoft.Phone"[m
[31m-    xmlns:toolkit="clr-namespace:Microsoft.Phone.Controls;assembly=Microsoft.Phone.Controls.Toolkit"[m
[31m-    xmlns:shell="clr-namespace:Microsoft.Phone.Shell;assembly=Microsoft.Phone"[m
[31m-    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"[m
[31m-    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"[m
[31m-    xmlns:i="clr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity"[m
[31m-    mc:Ignorable="d"[m
[31m-    d:DataContext="{d:DesignData SampleData/MainViewModelSampleData.xaml}"[m
[31m-    FontFamily="{StaticResource PhoneFontFamilyNormal}"[m
[31m-    FontSize="{StaticResource PhoneFontSizeNormal}"[m
[31m-    Foreground="{StaticResource PhoneForegroundBrush}"[m
[31m-    SupportedOrientations="Portrait"  Orientation="Portrait"[m
[31m-    shell:SystemTray.IsVisible="True">[m
[31m-    <toolkit:TransitionService.NavigationInTransition>[m
[31m-        <toolkit:NavigationInTransition>[m
[31m-            <toolkit:NavigationInTransition.Backward>[m
[31m-                <toolkit:RotateTransition Mode="In180Clockwise"/>[m
[31m-            </toolkit:NavigationInTransition.Backward>[m
[31m-            <toolkit:NavigationInTransition.Forward>[m
[31m-                <toolkit:RotateTransition Mode="In180Counterclockwise"/>[m
[31m-            </toolkit:NavigationInTransition.Forward>[m
[31m-        </toolkit:NavigationInTransition>[m
[31m-    </toolkit:TransitionService.NavigationInTransition>[m
[31m-    <toolkit:TransitionService.NavigationOutTransition>[m
[31m-        <toolkit:NavigationOutTransition>[m
[31m-            <toolkit:NavigationOutTransition.Backward>[m
[31m-                <toolkit:RotateTransition Mode="Out180Clockwise"/>[m
[31m-            </toolkit:NavigationOutTransition.Backward>[m
[31m-            <toolkit:NavigationOutTransition.Forward>[m
[31m-                <toolkit:RotateTransition Mode="Out180Counterclockwise"/>[m
[31m-            </toolkit:NavigationOutTransition.Forward>[m
[31m-        </toolkit:NavigationOutTransition>[m
[31m-    </toolkit:TransitionService.NavigationOutTransition>[m
[31m-[m
[31m-    <Grid x:Name="LayoutRoot" Background="Transparent">[m
[31m-        <phone:Pivot Title="My Fuel Tracker" SelectedIndex="0">[m
[31m-            <phone:PivotItem Header="summary">[m
[31m-                <Grid>[m
[31m-                    <Grid.ColumnDefinitions>[m
[31m-                        <ColumnDefinition Width="*" />[m
[31m-                        <ColumnDefinition Width="*" />[m
[31m-                    </Grid.ColumnDefinitions>[m
[31m-                    <Grid.RowDefinitions>[m
[31m-                        <RowDefinition Height="Auto" />[m
[31m-                        <RowDefinition Height="Auto" />[m
[31m-                        <RowDefinition Height="Auto" />[m
[31m-                        <RowDefinition Height="Auto" />[m
[31m-                        <RowDefinition Height="Auto" />[m
[31m-                        <RowDefinition Height="Auto" />[m
[31m-                        <RowDefinition Height="*" />[m
[31m-                    </Grid.RowDefinitions>[m
[31m-                    [m
[31m-                    <TextBlock Grid.ColumnSpan="2" Style="{StaticResource PhoneTextTitle2Style}" Text="{Binding LocalizedResources.AvgConsumptionLabel, Source={StaticResource LocalizedStrings}}"></TextBlock>[m
[31m-                    <TextBlock Grid.ColumnSpan="2" Margin="30 5 5 5" Grid.Row="1" Text="{Binding AvgConsumption}" />[m
[31m-[m
[31m-                    <TextBlock Grid.ColumnSpan="2" Grid.Row="2" Style="{StaticResource PhoneTextTitle2Style}" Text="{Binding LocalizedResources.MinConsumptionLabel, Source={StaticResource LocalizedStrings}}"></TextBlock>[m
[31m-                    <TextBlock Grid.ColumnSpan="2" Grid.Row="3"  Margin="30 5 5 5"  Text="{Binding MinConsumption}" />[m
[31m-[m
[31m-                    <TextBlock Grid.ColumnSpan="2" Grid.Row="4" Style="{StaticResource PhoneTextTitle2Style}" Text="{Binding LocalizedResources.MaxConsumptionLabel, Source={StaticResource LocalizedStrings}}"></TextBlock>[m
[31m-                    <TextBlock Grid.ColumnSpan="2" Grid.Row="5" Margin="30 5 5 5" Text="{Binding MaxConsumption}" />[m
[31m-[m
[31m-                    <toolkit:HubTile  Grid.Row="6" Source="Assets/add.png"  Message="Add fillup" Margin="45,151,10,39">[m
[31m-                        <i:Interaction.Triggers>[m
[31m-                            <i:EventTrigger EventName="Tap">[m
[31m-                                <i:InvokeCommandAction Command="{Binding AddFillupCommand}"/>[m
[31m-                            </i:EventTrigger>[m
[31m-                        </i:Interaction.Triggers>[m
[31m-                    </toolkit:HubTile>[m
[31m-                    <toolkit:HubTile Grid.Row="6" Grid.Column="1" Source="Assets/feature.settings.png" Message="Settings" Margin="10,151,45,39"></toolkit:HubTile>[m
[31m-                </Grid>[m
[31m-            </phone:PivotItem>[m
[31m-[m
[31m-            <phone:PivotItem Header="history">[m
[31m-            </phone:PivotItem>[m
[31m-        </phone:Pivot>[m
[31m-    </Grid>[m
[31m-[m
[31m-</phone:PhoneApplicationPage>[m
\ No newline at end of file[m
[1mdiff --git a/MyFuelTracker/MainPage.xaml.cs b/MyFuelTracker/MainPage.xaml.cs[m
[1mdeleted file mode 100644[m
[1mindex edf427c..0000000[m
[1m--- a/MyFuelTracker/MainPage.xaml.cs[m
[1m+++ /dev/null[m
[36m@@ -1,53 +0,0 @@[m
[31m-﻿using System;[m
[31m-using System.Collections.Generic;[m
[31m-using System.Linq;[m
[31m-using System.Net;[m
[31m-using System.Windows;[m
[31m-using System.Windows.Controls;[m
[31m-using System.Windows.Navigation;[m
[31m-using Microsoft.Phone.Controls;[m
[31m-using Microsoft.Phone.Shell;[m
[31m-using MyFuelTracker.Resources;[m
[31m-[m
[31m-namespace MyFuelTracker[m
[31m-{[m
[31m-	public partial class MainPage : PhoneApplicationPage[m
[31m-	{[m
[31m-		// Constructor[m
[31m-		public MainPage()[m
[31m-		{[m
[31m-			InitializeComponent();[m
[31m-[m
[31m-			// Set the data context of the listbox control to the sample data[m
[31m-			DataContext = App.ViewModel;[m
[31m-[m
[31m-			// Sample code to localize the ApplicationBar[m
[31m-			//BuildLocalizedApplicationBar();[m
[31m-		}[m
[31m-[m
[31m-		// Load data for the ViewModel Items[m
[31m-		protected override void OnNavigatedTo(NavigationEventArgs e)[m
[31m-		{[m
[31m-			if (!App.ViewModel.IsDataLoaded)[m
[31m-			{[m
[31m-				App.ViewModel.LoadData();[m
[31m-			}[m
[31m-		}[m
[31m-[m
[31m-		// Sample code for building a localized ApplicationBar[m
[31m-		//private void BuildLocalizedApplicationBar()[m
[31m-		//{[m
[31m-		//    // Set the page's ApplicationBar to a new instance of ApplicationBar.[m
[31m-		//    ApplicationBar = new ApplicationBar();[m
[31m-[m
[31m-		//    // Create a new button and set the text value to the localized string from AppResources.[m
[31m-		//    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));[m
[31m-		//    appBarButton.Text = AppResources.AppBarButtonText;[m
[31m-		//    ApplicationBar.Buttons.Add(appBarButton);[m
[31m-[m
[31m-		//    // Create a new menu item with the localized string from AppResources.[m
[31m-		//    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);[m
[31m-		//    ApplicationBar.MenuItems.Add(appBarMenuItem);[m
[31m-		//}[m
[31m-	}[m
[31m-}[m
\ No newline at end of file[m
[1mdiff --git a/MyFuelTracker/RelayCommand.cs b/MyFuelTracker/RelayCommand.cs[m
[1mdeleted file mode 100644[m
[1mindex 239d9e5..0000000[m
[1m--- a/MyFuelTracker/RelayCommand.cs[m
[1m+++ /dev/null[m
[36m@@ -1,52 +0,0 @@[m
[31m-﻿using System;[m
[31m-using System.Windows.Input;[m
[31m-[m
[31m-public class RelayCommand : ICommand[m
[31m-{[m
[31m-	Func<object, bool> canExecute;[m
[31m-	Action<object> executeAction;[m
[31m-[m
[31m-	public RelayCommand(Action<object> executeAction)[m
[31m-		: this(executeAction, null)[m
[31m-	{[m
[31m-	}[m
[31m-[m
[31m-	public RelayCommand(Action<object> executeAction, Func<object, bool> canExecute)[m
[31m-	{[m
[31m-		if (executeAction == null)[m
[31m-		{[m
[31m-			throw new ArgumentNullException("executeAction");[m
[31m-		}[m
[31m-		this.executeAction = executeAction;[m
[31m-		this.canExecute = canExecute;[m
[31m-	}[m
[31m-[m
[31m-	public bool CanExecute(object parameter)[m
[31m-	{[m
[31m-		bool result = true;[m
[31m-		Func<object, bool> canExecuteHandler = this.canExecute;[m
[31m-		if (canExecuteHandler != null)[m
[31m-		{[m
[31m-			result = canExecuteHandler(parameter);[m
[31m-		}[m
[31m-[m
[31m-		return result;[m
[31m-	}[m
[31m-[m
[31m-	public event EventHandler CanExecuteChanged;[m
[31m-[m
[31m-	public void RaiseCanExecuteChanged()[m
[31m-	{[m
[31m-		EventHandler handler = this.CanExecuteChanged;[m
[31m-		if (handler != null)[m
[31m-		{[m
[31m-			handler(this, new EventArgs());[m
[31m-		}[m
[31m-	}[m
[31m-[m
[31m-	public void Execute(object parameter)[m
[31m-	{[m
[31m-		this.executeAction(parameter);[m
[31m-	}[m
[31m-[m
[31m-}[m
\ No newline at end of file[m
[1mdiff --git a/MyFuelTracker/ViewModels/ItemViewModel.cs b/MyFuelTracker/ViewModels/ItemViewModel.cs[m
[1mdeleted file mode 100644[m
[1mindex 01161d5..0000000[m
[1m--- a/MyFuelTracker/ViewModels/ItemViewModel.cs[m
[1m+++ /dev/null[m
[36m@@ -1,88 +0,0 @@[m
[31m-﻿using System;[m
[31m-using System.ComponentModel;[m
[31m-using System.Diagnostics;[m
[31m-using System.Net;[m
[31m-using System.Windows;[m
[31m-using System.Windows.Controls;[m
[31m-using System.Windows.Input;[m
[31m-using System.Windows.Media;[m
[31m-using System.Windows.Media.Animation;[m
[31m-[m
[31m-namespace MyFuelTracker.ViewModels[m
[31m-{[m
[31m-	public class ItemViewModel : INotifyPropertyChanged[m
[31m-	{[m
[31m-		private string _lineOne;[m
[31m-		/// <summary>[m
[31m-		/// Sample ViewModel property; this property is used in the view to display its value using a Binding.[m
[31m-		/// </summary>[m
[31m-		/// <returns></returns>[m
[31m-		public string LineOne[m
[31m-		{[m
[31m-			get[m
[31m-			{[m
[31m-				return _lineOne;[m
[31m-			}[m
[31m-			set[m
[31m-			{[m
[31m-				if (value != _lineOne)[m
[31m-				{[m
[31m-					_lineOne = value;[m
[31m-					NotifyPropertyChanged("LineOne");[m
[31m-				}[m
[31m-			}[m
[31m-		}[m
[31m-[m
[31m-		private string _lineTwo;[m
[31m-		/// <summary>[m
[31m-		/// Sample ViewModel property; this property is used in the view to display its value using a Binding.[m
[31m-		/// </summary>[m
[31m-		/// <returns></returns>[m
[31m-		public string LineTwo[m
[31m-		{[m
[31m-			get[m
[31m-			{[m
[31m-				return _lineTwo;[m
[31m-			}[m
[31m-			set[m
[31m-			{[m
[31m-				if (value != _lineTwo)[m
[31m-				{[m
[31m-					_lineTwo = value;[m
[31m-					NotifyPropertyChanged("LineTwo");[m
[31m-				}[m
[31m-			}[m
[31m-		}[m
[31m-[m
[31m-		private string _lineThree;[m
[31m-		/// <summary>[m
[31m-		/// Sample ViewModel property; this property is used in the view to display its value using a Binding.[m
[31m-		/// </summary>[m
[31m-		/// <returns></returns>[m
[31m-		public string LineThree[m
[31m-		{[m
[31m-			get[m
[31m-			{[m
[31m-				return _lineThree;[m
[31m-			}[m
[31m-			set[m
[31m-			{[m
[31m-				if (value != _lineThree)[m
[31m-				{[m
[31m-					_lineThree = value;[m
[31m-					NotifyPropertyChanged("LineThree");[m
[31m-				}[m
[31m-			}[m
[31m-		}[m
[31m-[m
[31m-		public event PropertyChangedEventHandler PropertyChanged;[m
[31m-		private void NotifyPropertyChanged(String propertyName)[m
[31m-		{[m
[31m-			PropertyChangedEventHandler handler = PropertyChanged;[m
[31m-			if (null != handler)[m
[31m-			{[m
[31m-				handler(this, new PropertyChangedEventArgs(propertyName));[m
[31m-			}[m
[31m-		}[m
[31m-	}[m
[31m-}[m
\ No newline at end of file[m
[1mdiff --git a/MyFuelTracker/ViewModels/MainViewModel.cs b/MyFuelTracker/ViewModels/MainViewModel.cs[m
[1mdeleted file mode 100644[m
[1mindex c3c5e6a..0000000[m
[1m--- a/MyFuelTracker/ViewModels/MainViewModel.cs[m
[1m+++ /dev/null[m
[36m@@ -1,147 +0,0 @@[m
[31m-﻿using System;[m
[31m-using System.Collections.ObjectModel;[m
[31m-using System.ComponentModel;[m
[31m-using System.Windows;[m
[31m-using System.Windows.Input;[m
[31m-using Microsoft.Phone.Controls;[m
[31m-using MyFuelTracker.Resources;[m
[31m-[m
[31m-namespace MyFuelTracker.ViewModels[m
[31m-{[m
[31m-	public class MainViewModel : INotifyPropertyChanged[m
[31m-	{[m
[31m-		public MainViewModel()[m
[31m-		{[m
[31m-			this.Items = new ObservableCollection<ItemViewModel>();[m
[31m-			AddFillupCommand = new RelayCommand(_ =>[m
[31m-			                                    {[m
[31m-				                                    ((PhoneApplicationFrame) Application.Current.RootVisual).Navigate([m
[31m-					                                    new Uri("/AddFillupPage.xaml", UriKind.Relative));[m
[31m-				                                    AvgConsumption = 100;[m
[31m-			                                    });[m
[31m-		}[m
[31m-[m
[31m-		/// <summary>[m
[31m-		/// A collection for ItemViewModel objects.[m
[31m-		/// </summary>[m
[31m-		public ObservableCollection<ItemViewModel> Items { get; private set; }[m
[31m-[m
[31m-		private string _sampleProperty = "Sample Runtime Property Value";[m
[31m-		private decimal _avgConsumption;[m
[31m-		private decimal _minConsumption;[m
[31m-		private decimal _maxConsumption;[m
[31m-[m
[31m-		/// <summary>[m
[31m-		/// Sample ViewModel property; this property is used in the view to display its value using a Binding[m
[31m-		/// </summary>[m
[31m-		/// <returns></returns>[m
[31m-		public string SampleProperty[m
[31m-		{[m
[31m-			get[m
[31m-			{[m
[31m-				return _sampleProperty;[m
[31m-			}[m
[31m-			set[m
[31m-			{[m
[31m-				if (value != _sampleProperty)[m
[31m-				{[m
[31m-					_sampleProperty = value;[m
[31m-					NotifyPropertyChanged("SampleProperty");[m
[31m-				}[m
[31m-			}[m
[31m-		}[m
[31m-[m
[31m-		/// <summary>[m
[31m-		/// Sample property that returns a localized string[m
[31m-		/// </summary>[m
[31m-		public string LocalizedSampleProperty[m
[31m-		{[m
[31m-			get[m
[31m-			{[m
[31m-				return AppResources.SampleProperty;[m
[31m-			}[m
[31m-		}[m
[31m-[m
[31m-		public bool IsDataLoaded[m
[31m-		{[m
[31m-			get;[m
[31m-			private set;[m
[31m-		}[m
[31m-[m
[31m-		public decimal AvgConsumption[m
[31m-		{[m
[31m-			get { return _avgConsumption; }[m
[31m-			set[m
[31m-			{[m
[31m-				if (_avgConsumption != value)[m
[31m-				{[m
[31m-					_avgConsumption = value;[m
[31m-					NotifyPropertyChanged("AvgConsumption");[m
[31m-				}[m
[31m-			}[m
[31m-		}[m
[31m-		public decimal MinConsumption[m
[31m-		{[m
[31m-			get { return _minConsumption; }[m
[31m-			set[m
[31m-			{[m
[31m-				if (_minConsumption != value)[m
[31m-				{[m
[31m-					_minConsumption = value;[m
[31m-					NotifyPropertyChanged("MinConsumption");[m
[31m-				}[m
[31m-			}[m
[31m-		}[m
[31m-[m
[31m-		public decimal MaxConsumption[m
[31m-		{[m
[31m-			get { return _maxConsumption; }[m
[31m-			set[m
[31m-			{[m
[31m-				if (_maxConsumption != value)[m
[31m-				{[m
[31m-					_maxConsumption = value;[m
[31m-					NotifyPropertyChanged("MaxConsumption");[m
[31m-				}[m
[31m-			}[m
[31m-		}[m
[31m-[m
[31m-		public ICommand AddFillupCommand { get; set; }[m
[31m-[m
[31m-		/// <summary>[m
[31m-		/// Creates and adds a few ItemViewModel objects into the Items collection.[m
[31m-		/// </summary>[m
[31m-		public void LoadData()[m
[31m-		{[m
[31m-			// Sample data; replace with real data[m
[31m-			this.Items.Add(new ItemViewModel() { LineOne = "runtime one", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });[m
[31m-			this.Items.Add(new ItemViewModel() { LineOne = "runtime two", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });[m
[31m-			this.Items.Add(new ItemViewModel() { LineOne = "runtime three", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });[m
[31m-			this.Items.Add(new ItemViewModel() { LineOne = "runtime four", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });[m
[31m-			this.Items.Add(new ItemViewModel() { LineOne = "runtime five", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });[m
[31m-			this.Items.Add(new ItemViewModel() { LineOne = "runtime six", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });[m
[31m-			this.Items.Add(new ItemViewModel() { LineOne = "runtime seven", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });[m
[31m-			this.Items.Add(new ItemViewModel() { LineOne = "runtime eight", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });[m
[31m-			this.Items.Add(new ItemViewModel() { LineOne = "runtime nine", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });[m
[31m-			this.Items.Add(new ItemViewModel() { LineOne = "runtime ten", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });[m
[31m-			this.Items.Add(new ItemViewModel() { LineOne = "runtime eleven", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });[m
[31m-			this.Items.Add(new ItemViewModel() { LineOne = "runtime twelve", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });[m
[31m-			this.Items.Add(new ItemViewModel() { LineOne = "runtime thirteen", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });[m
[31m-			this.Items.Add(new ItemViewModel() { LineOne = "runtime fourteen", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });[m
[31m-			this.Items.Add(new ItemViewModel() { LineOne = "runtime fifteen", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });[m
[31m-			this.Items.Add(new ItemViewModel() { LineOne = "runtime sixteen", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });[m
[31m-[m
[31m-			this.IsDataLoaded = true;[m
[31m-		}[m
[31m-[m
[31m-		public event PropertyChangedEventHandler PropertyChanged;[m
[31m-		private void NotifyPropertyChanged(String propertyName)[m
[31m-		{[m
[31m-			PropertyChangedEventHandler handler = PropertyChanged;[m
[31m-			if (null != handler)[m
[31m-			{[m
[31m-				handler(this, new PropertyChangedEventArgs(propertyName));[m
[31m-			}[m
[31m-		}[m
[31m-	}[m
[31m-}[m
\ No newline at end of file[m
