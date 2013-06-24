using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MyFuelTracker.Infrastructure.Helpers;
using MyFuelTracker.ViewModels;

namespace MyFuelTracker.Infrastructure
{
	public class DynamicAppBarBehavior : Behavior<PhoneApplicationPage>
	{
		#region Fields

		private bool _appBarInitialized;
		private Pivot _pivot;
		private PhoneApplicationPage _page;
		private object _selected;

		#endregion

		#region  Properties

		public string PivotElementName
		{
			get { return (string)GetValue(PivotElementNameProperty); }
			set { SetValue(PivotElementNameProperty, value); }
		}

		public static readonly DependencyProperty PivotElementNameProperty =
			DependencyProperty.Register("PivotElementName", typeof(string), typeof(DynamicAppBarBehavior), new PropertyMetadata(String.Empty));

		#endregion

		#region Methods

		protected override void OnAttached()
		{
			base.OnAttached();

			_page = AssociatedObject;
			_page.ApplicationBar = new ApplicationBar
			{
				IsVisible = true,
				Mode = ApplicationBarMode.Default,
				Opacity = 1,
				IsMenuEnabled = true,
				BackgroundColor = Color.FromArgb(220, 40, 40, 40)
			};
			_page.Loaded += (s, e) => Initialize();
		}

		private void Initialize()
		{
			if (_appBarInitialized)
				return;

			_pivot = _page.FindName(PivotElementName) as Pivot;
			if (_pivot != null)
			{
				_pivot.SelectionChanged += OnPivotSelectionChanged;
				UpdateAppBarButtons(_pivot.SelectedItem as IAppBarItemsProvider);
				UpdateAppBarMenu(_pivot.SelectedItem as IAppBarItemsProvider);
				foreach (var item in _pivot.Items)
				{
					SubscribeToButtonsChange(item as IDynamicAppBarItemsProvider);
				}
			}
			else
			{
				UpdateAppBarButtons(_page.DataContext as IAppBarItemsProvider);
				SubscribeToButtonsChange(_page.DataContext as IDynamicAppBarItemsProvider);
				UpdateAppBarMenu(_page.DataContext as IAppBarItemsProvider);
			}

			_appBarInitialized = true;
		}

		private void UpdateAppBarMenu(IAppBarItemsProvider appBarItemsProvider)
		{
			if (appBarItemsProvider == null)
				return;

			_page.ApplicationBar.MenuItems.Clear();
			foreach (var item in appBarItemsProvider.MenuItems)
			{
				var menuItem = new ApplicationBarMenuItem { Text = item.Text };
				var tempItem = item;
				menuItem.Click += (s, e) => tempItem.OnClick();
				_page.ApplicationBar.MenuItems.Add(menuItem);
			}
		}

		private void SubscribeToButtonsChange(IDynamicAppBarItemsProvider provider)
		{
			if (provider == null)
				return;
			provider.AppBarChanged += (s, e) =>
				{
					if (_pivot != null && _pivot.SelectedItem != s)
						return;
					UpdateAppBarButtons(provider);
					UpdateAppBarMenu(provider);
				};
		}

		private void OnPivotSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (_selected == _pivot.SelectedItem)
				return;

			UpdateAppBarButtons(_pivot.SelectedItem as IAppBarItemsProvider);
			_selected = _pivot.SelectedItem;
		}

		private void UpdateAppBarButtons(IAppBarItemsProvider appBarItemsProvider)
		{
			if (appBarItemsProvider == null)
			{
				_page.ApplicationBar.Mode = ApplicationBarMode.Minimized;
				return;
			}
			_page.ApplicationBar.Mode = ApplicationBarMode.Default;
			_page.ApplicationBar.Buttons
					.OfType<IApplicationBarIconButton>()
					.Where(b => appBarItemsProvider.Buttons.All(b1 => b1.Text != b.Text))
					.ToArray()
					.Foreach(_page.ApplicationBar.Buttons.Remove);

			var dynamicAppBarButtons = appBarItemsProvider.Buttons.ToArray();
			for (int i = 0; i < dynamicAppBarButtons.Length; i++)
			{
				var b = dynamicAppBarButtons[i];
				var button = _page.ApplicationBar.Buttons
												.OfType<IApplicationBarIconButton>()
												.FirstOrDefault(but => but.Text == b.Text);
				if (button == null)
				{
					button = new ApplicationBarIconButton {IconUri = new Uri(b.IconUri, UriKind.RelativeOrAbsolute), Text = b.Text};
					DynamicAppBarButton b1 = b;
					button.Click += delegate { b1.OnClick(); };
					_page.ApplicationBar.Buttons.Insert(i, button);
				}
			}

			
		}

		#endregion
	}

	public interface IAppBarItemsProvider
	{
		IEnumerable<DynamicAppBarButton> Buttons { get; }
		IEnumerable<DynamicAppBarItem> MenuItems { get; }
	}

	public interface IDynamicAppBarItemsProvider : IAppBarItemsProvider
	{
		event EventHandler AppBarChanged;
	}

	public class DynamicAppBarButton : DynamicAppBarItem
	{
		public string IconUri { get; set; }
	}

	public class DynamicAppBarItem
	{
		public string Text { get; set; }
		public System.Action OnClick { get; set; }
	}
}
