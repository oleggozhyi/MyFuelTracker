using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using MyFuelTracker.Core;

namespace MyFuelTracker.ViewModels
{
	public class HistoryViewModel : Screen
	{
		#region fields


		#endregion

		#region ctors

		public HistoryViewModel()
		{
			Debug.WriteLine("HistoryViewModel created");
		}

		#endregion

		#region properties

		public string DisplayName { get { return "history"; } }

		public ObservableCollection<FillupItemViewModel> Fillups { get; set; }

		#endregion

		#region methods


		#endregion
	}
}
