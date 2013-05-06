using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MyFuelTracker.Views
{
	public partial class ValueWithDimention : UserControl
	{
		public ValueWithDimention()
		{
			InitializeComponent();
		}

		public string ValueText
		{
			get { return (string)GetValue(ValueTextProperty); }
			set { SetValue(ValueTextProperty, value); }
		}

		public static readonly DependencyProperty ValueTextProperty =
			DependencyProperty.Register("ValueTextProperty", typeof(string), typeof(ValueWithDimention), new PropertyMetadata(""));

		public string ValueDimension
		{
			get { return (string)GetValue(ValueDimensionProperty); }
			set { SetValue(ValueDimensionProperty, value); }
		}

		public static readonly DependencyProperty ValueDimensionProperty =
			DependencyProperty.Register("ValueDimensionProperty", typeof(string), typeof(ValueWithDimention), new PropertyMetadata(""));


		public Brush ValueTextBrush
		{
			get { return (Brush)GetValue(ValueTextBrushProperty); }
			set { SetValue(ValueTextBrushProperty, value); }
		}

		public static readonly DependencyProperty ValueTextBrushProperty =
			DependencyProperty.Register("ValueTextBrushProperty", typeof(Brush), typeof(ValueWithDimention),
				new PropertyMetadata(new SolidColorBrush(Colors.White)));

		public Brush ValueDimensionBrush
		{
			get { return (Brush)GetValue(ValueDimensionBrushProperty); }
			set { SetValue(ValueDimensionBrushProperty, value); }
		}

		public static readonly DependencyProperty ValueDimensionBrushProperty =
			DependencyProperty.Register("ValueDimensionBrushProperty", typeof(Brush), typeof(ValueWithDimention),
				new PropertyMetadata(new SolidColorBrush(Colors.DarkGray)));

		public double ValueTextSize
		{
			get { return (double)GetValue(ValueTextSizeProperty); }
			set { SetValue(ValueTextSizeProperty, value); }
		}

		public static readonly DependencyProperty ValueTextSizeProperty =
			DependencyProperty.Register("ValueTextSizeProperty", typeof(double), typeof(ValueWithDimention),
				new PropertyMetadata(Application.Current.Resources["PhoneFontSizeMedium"]));

		public double ValueDimensionSize
		{
			get { return (double)GetValue(ValueDimensionSizeProperty); }
			set { SetValue(ValueDimensionSizeProperty, value); }
		}

		public static readonly DependencyProperty ValueDimensionSizeProperty =
			DependencyProperty.Register("ValueDimensionSizeProperty", typeof(double), typeof(ValueWithDimention),
				new PropertyMetadata(Application.Current.Resources["PhoneFontSizeNormal"]));
	}
}
