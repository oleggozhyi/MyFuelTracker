using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MyFuelTracker.Infrastructure
{
    public class DynamicAppBarBehavior : Behavior<PhoneApplicationPage>
    {
        #region Fields

        private bool _appBarInitialized;
        private Pivot _pivot;
        private PhoneApplicationPage _page;
        private object _selected;
        private readonly Dictionary<DynamicAppBarButton, ApplicationBarIconButton> _buttons = new Dictionary<DynamicAppBarButton, ApplicationBarIconButton>();

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
                IsMenuEnabled = true
            };
            _page.Loaded += OnPageLoaded;
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            if (_appBarInitialized)
                return;

            _pivot = _page.FindName(PivotElementName) as Pivot;
            if (_pivot != null)
            {
                _pivot.SelectionChanged += OnPivotSelectionChanged;
                UpdateAppBar(_pivot.SelectedItem as IAppBarButtonsProvider);
                foreach (var item in _pivot.Items)
                {
                    SubscribeToButtonsChange(item as IDynamycButtonsProvider);
                }
            }
            else
            {
                UpdateAppBar(_page.DataContext as IAppBarButtonsProvider);
                SubscribeToButtonsChange(_page.DataContext as IDynamycButtonsProvider);
            }

            _appBarInitialized = true;
        }

        private void SubscribeToButtonsChange(IDynamycButtonsProvider provider)
        {
            if (provider == null)
                return;
            provider.ButtonsChanged += (s, e) => UpdateAppBar(provider);
        }

        private void OnPivotSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_selected == _pivot.SelectedItem)
                return;

            UpdateAppBar(_pivot.SelectedItem as IAppBarButtonsProvider);
            _selected = _pivot.SelectedItem;
        }

        private void UpdateAppBar(IAppBarButtonsProvider appBarButtonsProvider)
        {
            if (appBarButtonsProvider == null)
            {
                _page.ApplicationBar.IsVisible = false;
                return;
            }
            _page.ApplicationBar.IsVisible = true;
            _page.ApplicationBar.Buttons.Clear();

            foreach (var b in appBarButtonsProvider.Buttons)
            {
                ApplicationBarIconButton button;
                if (_buttons.ContainsKey(b))
                    button = _buttons[b];
                else
                {
                    button = new ApplicationBarIconButton { IconUri = new Uri(b.IconUri, UriKind.RelativeOrAbsolute), Text = b.Text };
                    DynamicAppBarButton b1 = b;
                    button.Click += delegate { b1.OnClick(); };
                    _buttons[b] = button;
                }

                _page.ApplicationBar.Buttons.Add(button);
            }
        }

        #endregion
    }

    public interface IAppBarButtonsProvider
    {
        IEnumerable<DynamicAppBarButton> Buttons { get; }
    }

    public interface IDynamycButtonsProvider : IAppBarButtonsProvider
    {
        event EventHandler ButtonsChanged;
    }

    public class DynamicAppBarButton
    {
        public string Text { get; set; }
        public string IconUri { get; set; }
        public System.Action OnClick { get; set; }
    }
}
