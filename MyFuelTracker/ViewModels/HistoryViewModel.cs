using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using MyFuelTracker.Core;
using MyFuelTracker.Core.Models;

namespace MyFuelTracker.ViewModels
{
	public class HistoryViewModel : Screen, IUpdatable
	{
		#region fields

		private readonly IFillupService _fillupService;
		private IEnumerable<FillupHistoryItemViewModel> _items;

		#endregion

		#region ctors

		public HistoryViewModel(IFillupService fillupService)
		{
			Debug.WriteLine("HistoryViewModel created");
			DisplayName = "history";
			_fillupService = fillupService;
		}

		#endregion

		#region properties

		public IEnumerable<FillupHistoryItemViewModel> Items
		{
			get { return _items; }
			set
			{
				if (Equals(value, _items)) return;
				_items = value;
				NotifyOfPropertyChange(() => Items);
			}
		}

		#endregion

		#region methods

		protected async override void OnActivate()
		{
			base.OnActivate();
			await UpdateAsync();
		}

		public async Task UpdateAsync()
		{
			var historyItems = await _fillupService.GetHistoryAsync();
			Items = historyItems.Select(i => new FillupHistoryItemViewModel(i)).ToArray();
		}

		#endregion
	}
}
