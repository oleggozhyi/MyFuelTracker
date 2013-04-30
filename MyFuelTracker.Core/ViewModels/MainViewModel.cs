using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFuelTracker.Core.ViewModels
{
	public class MainViewModel
	{
		public MainViewModel()
		{
			Summary = new SummaryViewModel();
		}

		public SummaryViewModel Summary { get; private set; }
	}
}
