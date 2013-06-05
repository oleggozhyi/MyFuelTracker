using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MyFuelTracker.Infrastructure;
using MyFuelTracker.Infrastructure.Helpers;
using MyFuelTracker.ViewModels;
using MyFuelTracker.Tests.TestInfrastructure;

namespace MyFuelTracker.Tests
{
	[TestClass]
	public class FillupsGroupingHelperTests
	{
		[TestMethod]
		public void GroupFillups_groups_fillups_by_month_and_year()
		{
			//arrange
			var items = new[]
			{
				new FillupHistoryItemViewModel {FillupDate = 20.Apr(2013)}, //0
				new FillupHistoryItemViewModel {FillupDate = 1.Apr(2013)},  //1
				new FillupHistoryItemViewModel {FillupDate = 28.Feb(2013)}, //2
				new FillupHistoryItemViewModel {FillupDate = 20.Feb(2013)}, //3
				new FillupHistoryItemViewModel {FillupDate = 31.Jan(2013)}, //4
				new FillupHistoryItemViewModel {FillupDate = 25.Jan(2013)}, //5
				new FillupHistoryItemViewModel {FillupDate = 1.Jan(2013)},  //6
				new FillupHistoryItemViewModel {FillupDate = 2.Jan(2012)},  //7
				new FillupHistoryItemViewModel {FillupDate = 1.Jan(2012)},  //8
			};
			//act
			var groupFillups = FillupsGroupingHelper.GroupFillups(items);
			//assert
			groupFillups.Count.ShouldBe(4);

			groupFillups[0].MonthDateTime = 1.Apr(2013);
			CollectionAssert.AreEqual(new[] { items[0], items[1] }, groupFillups[0]);

			groupFillups[1].MonthDateTime = 1.Feb(2013);
			CollectionAssert.AreEqual(new[] { items[2], items[3] }, groupFillups[1]);

			groupFillups[2].MonthDateTime = 1.Jan(2013);
			CollectionAssert.AreEqual(new[] { items[4], items[5], items[6] }, groupFillups[2]);

			groupFillups[3].MonthDateTime = 1.Jan(2012);
			CollectionAssert.AreEqual(new[] { items[7], items[8] }, groupFillups[3]);
		}
	}
}