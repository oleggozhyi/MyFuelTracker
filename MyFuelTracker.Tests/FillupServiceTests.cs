using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MyFuelTracker.Core;
using MyFuelTracker.Core.Models;

namespace MyFuelTracker.Tests
{
	[TestClass]
	public class FillupServiceTests
	{
		
		[TestMethod]
		public void Returns_saved_fillups()
		{
			//arrange
			var fillupService = CreateService();
			fillupService.SaveFillupAsync(new Fillup {Id = 123, Volume = 100}).Wait();
			fillupService.SaveFillupAsync(new Fillup { Id = 122, Volume = 134 }).Wait();

			//act
			var task = fillupService.GetHistoryAsync();
			task.Wait();
			var fillups = task.Result;

			//assert
			Assert.IsNotNull(fillups.SingleOrDefault(f => f.Fillup.Id == 123 && f.Fillup.Volume == 100));
			Assert.IsNotNull(fillups.SingleOrDefault(f => f.Fillup.Id == 122 && f.Fillup.Volume == 134));
		}

		[TestMethod]
		public void Returns_saved_fillups_in_sorted_by_date_descending()
		{
			//arrange
			var fillupService = CreateService();
			fillupService.SaveFillupAsync(new Fillup { Id = 1,  Date = new DateTime(2013, 01, 20) }).Wait();
			fillupService.SaveFillupAsync(new Fillup { Id = 2, Date = new DateTime(2013, 01, 01) }).Wait();
			fillupService.SaveFillupAsync(new Fillup { Id = 3, Date = new DateTime(2013, 01, 25) }).Wait();

			//act
			var task = fillupService.GetHistoryAsync();
			task.Wait();
			var fillups = task.Result;

			//assert
			Assert.AreEqual(3, fillups.ElementAt(0).Fillup.Id);
			Assert.AreEqual(1, fillups.ElementAt(1).Fillup.Id);
			Assert.AreEqual(2, fillups.ElementAt(2).Fillup.Id);
		}

		public FillupService CreateService()
		{
			return new FillupService();
		}
	}
}
