using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFuelTracker.Core.Annotations;
using MyFuelTracker.Core.DataAccess;
using MyFuelTracker.Core.Models;

namespace MyFuelTracker.Core
{
    public class FillupService : IFillupService
    {
	    #region Fields

	    private readonly IStatisticsService _statisticsService;
	    private readonly IFuelEconomyStrategyProvider _strategyProvider;
	    private IFuelTrackerDb Db { get; set; }
	    private volatile Task<Fillup[]> _loadFillupsTasks;
	    private volatile Task<FillupHistoryItem[]> _calculateHistoryTask;
	    private volatile Task<Statistics> _calculateStatisticsTask;
	    private readonly object _sync1 = new object();
	    private readonly object _sync2 = new object();
	    private readonly object _sync3 = new object();

	    #endregion

	    #region ctor

	    public FillupService(IFuelTrackerDb db, IStatisticsService statisticsService,
	                         IFuelEconomyStrategyProvider strategyProvider)
	    {
		    _statisticsService = statisticsService;
		    _strategyProvider = strategyProvider;
		    Db = db;
	    }

	    #endregion

	    #region Methods

	    public void ClearCache()
	    {
		    _loadFillupsTasks = null;
		    _calculateHistoryTask = null;
		    _calculateStatisticsTask = null;
	    }

	    private async Task<Fillup[]> GetFillupsAsync()
	    {
		    if (_loadFillupsTasks == null)
			    lock (_sync1)
			    {
				    if (_loadFillupsTasks == null)
					    _loadFillupsTasks = Db.LoadAllFillupsAsync();
			    }
		    return await _loadFillupsTasks;
	    }

	    public async Task<FillupHistoryItem[]> GetHistoryAsync()
	    {
		    Fillup[] fillups = await GetFillupsAsync();
		    if (_calculateHistoryTask == null)
			    lock (_sync2)
			    {
				    if (_calculateHistoryTask == null)
					    _calculateHistoryTask = Task.Run(() => CalculateHistory(fillups));
			    }
		    return await _calculateHistoryTask;
	    }

	    public async Task<Statistics> GetStatisticsAsync()
	    {
		    FillupHistoryItem[] fillupHistoryItems = await GetHistoryAsync();
		    if (!fillupHistoryItems.Any())
			    return null;

		    if (_calculateStatisticsTask == null)
			    lock (_sync3)
			    {
				    if (_calculateStatisticsTask == null)
					    _calculateStatisticsTask = _statisticsService.CalculateStatisticsAsync(fillupHistoryItems);
			    }
		    return await _calculateStatisticsTask;
	    }

	    public async Task<Fillup> CreateNewFillupAsync()
	    {
		    Fillup[] fillups = await GetFillupsAsync();
		    var fillup = new Fillup {Id = Guid.NewGuid(), Date = DateTime.Now};
		    var latestFillup = fillups.LastOrDefault();
		    if (latestFillup != null)
		    {
			    var lastOdometerEnd = fillups.OrderBy(f => f.OdometerEnd).Last().OdometerEnd;
			    fillup.OdometerStart = lastOdometerEnd;
			    fillup.OdometerEnd = lastOdometerEnd;
			    fillup.FuelType = latestFillup.FuelType;
			    fillup.Price = latestFillup.Price;
		    }
		    return await Task.FromResult(fillup);
	    }

	    public async Task SaveFillupAsync([NotNull] Fillup fillup)
	    {
		    if (fillup == null) throw new ArgumentNullException("fillup");
		    if (fillup.OdometerEnd <= fillup.OdometerStart)
			    throw new InvalidOperationException("Odometer start should be less than end");

		    await Db.SaveFillupAsync(fillup);
		    ClearCache();
	    }

	    public async Task DeleteFillupAsync(Fillup fillup)
	    {
		    await Db.DeleteFillupAsync(fillup);
		    ClearCache();
	    }

	    public async Task<Fillup> GetFillupAsync(Guid id)
	    {
		    Fillup[] fillups = await GetFillupsAsync();
		    return fillups.First(f => f.Id == id);
	    }

	    public async Task RestoreDataAsync(IEnumerable<Fillup> fillupsData)
	    {
		    await Db.RestoreAsync(fillupsData);
		    ClearCache();
	    }

	    private FillupHistoryItem[] CalculateHistory(Fillup[] fillups)
	    {
			var fuelEconomyStrategy = _strategyProvider.GetCurrentStrategy();
		    var historyItems = new List<FillupHistoryItem>();
		    lock (this)
		    {
			    if (!fillups.Any())
				    return historyItems.ToArray();

			    double partialFillupOdoStart = -1;
			    double partialFillupVolume = 0;
			    foreach (var fillup in fillups)
			    {
				    var historyItem = new FillupHistoryItem {Fillup = fillup};
				    historyItems.Add(historyItem);

				    if (fillup.IsPartial)
				    {
					    if (partialFillupOdoStart < 0)
						    partialFillupOdoStart = fillup.OdometerStart;
					    partialFillupVolume += fillup.Volume;
				    }
				    else
				    {
					    double actualVolume = fillup.Volume + partialFillupVolume;
					    double actualDistance = fillup.OdometerEnd -
					                            (partialFillupOdoStart > 0 ? partialFillupOdoStart : fillup.OdometerStart);
						historyItem.FuelEconomy = fuelEconomyStrategy.CalculateEconomy(actualDistance, actualVolume);

					    partialFillupOdoStart = -1;
					    partialFillupVolume = 0;
				    }
			    }
		    }

		    historyItems.Reverse();
		    return historyItems.ToArray();
	    }

	    #endregion

    }
}
