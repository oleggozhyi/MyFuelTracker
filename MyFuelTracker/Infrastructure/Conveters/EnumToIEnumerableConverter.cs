using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using MyFuelTracker.Resources;

namespace MyFuelTracker.Infrastructure.Conveters
{
	public class EnumToIEnumerableConverter : IValueConverter
	{
		private readonly Dictionary<Type, List<object>> _cache = new Dictionary<Type, List<object>>();

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var type = value.GetType();
			if (!_cache.ContainsKey(type))
			{
				var fields = type.GetFields().Where(field => field.IsLiteral);
				var values = new List<object>();
				foreach (var field in fields)
				{
					var a = ((DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false)).SingleOrDefault();

					if (a == null)
						throw new InvalidOperationException("DescriptionAttribute was not found");

					string val = AppResources.ResourceManager.GetString(a.Description);
					values.Add(val);
				}
				_cache[type] = values;
			}

			return _cache[type];
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}