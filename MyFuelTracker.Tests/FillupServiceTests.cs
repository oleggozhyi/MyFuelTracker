using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MyFuelTracker.Core;
using MyFuelTracker.Core.DataAccess;
using MyFuelTracker.Core.Models;
using MyFuelTracker.Tests.TestInfrastructure;

namespace MyFuelTracker.Tests
{
	[TestClass]
	public class FillupServiceTests
	{
		private InMemoryFuelTrackerDb DB;

		[TestInitialize]
		public void Init()
		{
			DB = new InMemoryFuelTrackerDb(true);
		}

		[TestMethod]
		public void CreateNewFillup_sets_new_id_and_date()
		{
			//arrange
			var fillupService = CreateService();
			//act
			var fillup = fillupService.CreateNewFillupAsync().WaitAndReturn();
			//assert
			fillup.Id.ShouldNotBe(default(Guid));
			Assert.IsTrue(DateTime.Now - fillup.Date < TimeSpan.FromSeconds(2));
		}

		[TestMethod]
		public void CreateNewFillup_copies_start_and_end_from_previous_fillup()
		{
			//arrange
			var fillupService = CreateService();
			DB.Fillups = new List<Fillup>
			{
				new Fillup {Id = Guid.NewGuid(), Date = 12.Apr(2013), OdometerStart = 2000, OdometerEnd = 2360, Volume = 37.2, Price = 12.59 },
				new Fillup {Id = Guid.NewGuid(), Date = 20.Apr(2013), OdometerStart = 2360, OdometerEnd = 2700, Volume = 35.6, Price = 12.59 },
			};

			//act
			var fillup = fillupService.CreateNewFillupAsync().WaitAndReturn();

			//asseet
			fillup.OdometerStart.ShouldBe(2700);
			fillup.OdometerEnd.ShouldBe(2700);
		}


		[TestMethod]
		public void SaveFillupAsync_throws_if_start_greater_than_end()
		{
			//arrange
			var fillupService = CreateService();
			var fillup = fillupService.CreateNewFillupAsync().WaitAndReturn();

			//act
			Action a = () => fillupService.SaveFillupAsync(fillup).Wait();

			//assert
			a.ShouldThrow<InvalidOperationException>();
		}

		[TestMethod]
		public void SaveFillupAsync_saves_fillup_to_db()
		{
			//arrange
			var fillupService = CreateService();
			var fillup = fillupService.CreateNewFillupAsync().WaitAndReturn();
			fillup.OdometerEnd += 200;

			//act
			fillupService.SaveFillupAsync(fillup).Wait();

			//assert
			DB.Fillups.Single().ShouldBe(fillup);
		}

		[TestMethod]
		public void GetHistoryAsync_calculates_fuel_economy()
		{
			//arrange
			var fillupService = CreateService();
			DB.Fillups = new List<Fillup>
			{
				new Fillup { Id = Guid.NewGuid(), Date = 1.Apr(2013), Price = 12.59, 
					OdometerStart = 1000.0, 
					OdometerEnd = 1500.0, 
					Volume = 45 }
			};

			//act
			var fillupHistoryItem = fillupService.GetHistoryAsync().WaitAndReturn().Single();

			//assert
			fillupHistoryItem.FuelEconomy.HasValue.ShouldBe(true);
			fillupHistoryItem.FuelEconomy.Value.ShouldBe(45.0 * 100.0 / (1500.0 - 1000.0));
		}

		[TestMethod]
		public void GetHistoryAsync_calculates_fuel_economy_for_partial_fillup()
		{
			//arrange
			var fillupService = CreateService();
			DB.Fillups = new List<Fillup>
			{
				new Fillup { Id = Guid.NewGuid(), Date = 13.Apr(2013), Price = 12.59, OdometerStart = 1000.0, OdometerEnd = 1500.0, Volume = 45,
					IsPartial = true }
			};

			//act
			var fillupHistoryItem = fillupService.GetHistoryAsync().WaitAndReturn().Single();
			fillupHistoryItem.FuelEconomy.HasValue.ShouldBe(false);
		}

		[TestMethod]
		public void GetHistoryAsync_returns_history_ordered_by_date_decsending()
		{
			//arrange
			var fillupService = CreateService();
			Guid id1 = Guid.NewGuid(), id2 = Guid.NewGuid(), id3 = Guid.NewGuid();
			DB.Fillups = new List<Fillup>
			{
				new Fillup { Id =id1, Date = 1.Apr(2013), Price = 12.59, OdometerStart = 1000.0, OdometerEnd = 1500.0, Volume = 45, IsPartial = false },
				new Fillup { Id = id3, Date = 2.Apr(2013), Price = 12.59, OdometerStart = 1500, OdometerEnd = 2000, Volume = 20, IsPartial = true },
				new Fillup { Id =id2, Date = 3.Apr(2013), Price = 12.59, OdometerStart = 2000, OdometerEnd = 2200, Volume = 25, IsPartial = true },
			};

			//act
			var historyItems = fillupService.GetHistoryAsync().WaitAndReturn();

			//assert
			historyItems.ElementAt(0).Fillup.Id.ShouldBe(id2);
			historyItems.ElementAt(1).Fillup.Id.ShouldBe(id3);
			historyItems.ElementAt(2).Fillup.Id.ShouldBe(id1);
		}
		[TestMethod]
		public void GetHistoryAsync_calculates_fuel_economy_considering_previous_partial_fillups()
		{
			//arrange
			var fillupService = CreateService();
			DB.Fillups = new List<Fillup>
			{
				new Fillup { Id = Guid.NewGuid(), Date = 1.Apr(2013), Price = 12.59, 
					OdometerStart = 1000.0, OdometerEnd = 1500.0, Volume = 45,
					IsPartial = false },
				new Fillup { Id = Guid.NewGuid(), Date = 2.Apr(2013), Price = 12.59, 
					OdometerStart = 1500, OdometerEnd = 2000, Volume = 20,
					IsPartial = true },
				new Fillup { Id = Guid.NewGuid(), Date = 3.Apr(2013), Price = 12.59, 
					OdometerStart = 2000, OdometerEnd = 2200, Volume = 25,
					IsPartial = true },
				new Fillup { Id = Guid.NewGuid(), Date = 4.Apr(2013), Price = 12.59, 
					OdometerStart = 2200, OdometerEnd = 2700, Volume = 50,
					IsPartial = false }
			};

			//act
			var latestFillup = fillupService.GetHistoryAsync().WaitAndReturn().First();
			latestFillup.FuelEconomy.HasValue.ShouldBe(true);
			latestFillup.FuelEconomy.Value.ShouldBe((25.0 + 20.0 + 50.0) * 100.0 / (2700.0 - 1500.0));
		}


		public FillupService CreateService()
		{
			return new FillupService(DB, new StatisticsService());
		}
	}
}
