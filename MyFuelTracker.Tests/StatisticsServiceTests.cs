using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MyFuelTracker.Core;
using MyFuelTracker.Core.Models;
using MyFuelTracker.Tests.TestInfrastructure;

namespace MyFuelTracker.Tests
{
	[TestClass]
	public class StatisticsServiceTests
	{
		private const double EPSILON = 0.01;
		[TestMethod]
		public void CalculateStatistics_Test()
		{
			//arrange
			var statisticsService = CreateService();
			var history = new FillupHistoryItem[]
			              {
				              fillup(1.Jan(2013), 12.59, 31.5, 9.50, false),
				              fillup(8.Jan(2013), 12.59, 32.5, 12.50, false),
				              fillup(16.Jan(2013), 12.59, 33.5, 10.21, false),
				              fillup(25.Jan(2013), 12.59, 34.5, 10.56, false),
				              fillup(29.Jan(2013), 12.59, 35.5, 9.95, false),
				              fillup(5.Feb(2013), 12.59, 36.5, 9.73, false),
				              fillup(14.Feb(2013), 12.59, 37.5, 8.90, false),
				              fillup(26.Feb(2013), 12.59, 38.5, 10.11, false),
			              }.Reverse();
			//act
			var statistics = statisticsService.CalculateStatisticsAsync(history).WaitAndReturn();
			//assert
			statistics.AllTimeAvgConsumption.ShouldRoughlyBe((9.50 + 12.50 + 10.21 + 10.56 + 9.95 + 9.73 + 8.90 + 10.11) / 8.0, EPSILON);
			statistics.Last4FillupsAvgConsumption.ShouldRoughlyBe((9.95 + 9.73 + 8.90 + 10.11) / 4.0, EPSILON);
			statistics.LastConsumption.ShouldRoughlyBe(10.11, EPSILON);
			statistics.LastMonthCost.ShouldRoughlyBe((35.5 + 36.5 + 37.5 + 38.5) * 12.59, EPSILON);
			statistics.AllTimeAvgMonthCost.ShouldRoughlyBe((31.5 + 32.5 + 33.5 + 34.5 + 35.5 + 36.5 + 37.5 + 38.5) * 12.59 / ((31.0 + 26.0) / 30.0), EPSILON);
			statistics.MinConsumption.ShouldRoughlyBe(8.90, EPSILON);
			statistics.MaxConsumption.ShouldRoughlyBe(12.50, EPSILON);
		}

		private static StatisticsService CreateService()
		{
			return new StatisticsService();
		}

		private static FillupHistoryItem fillup(DateTime date, double price, double volume, double consumption, bool ispartial)
		{
			return new FillupHistoryItem
				   {
					   Consumption = ispartial ? (double?)null : consumption,
					   Fillup = new Fillup
								{
									Id = Guid.NewGuid(),
									Date = date,
									IsPartial = ispartial,
									Price = price,
									Volume = volume
								}
				   };
		}
	}

}
