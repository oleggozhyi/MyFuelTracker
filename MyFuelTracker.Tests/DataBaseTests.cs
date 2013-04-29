using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MyFuelTracker.Core.DataAccess;
using MyFuelTracker.Core.Models;

namespace MyFuelTracker.Tests
{
	[TestClass]
	public class DataBaseTests
	{
		[TestInitialize]
		public void Init()
		{
			var context = new FuelTrackerDataContext();
			if(context.DatabaseExists())
				context.DeleteDatabase();
		}

		[TestMethod]
		public void PetrolSaveTest()
		{
			var ensureDatabaseTask = FuelTrackerDataContext.EnsureDatabase();
			ensureDatabaseTask.Wait();
			using (var dataContext = ensureDatabaseTask.Result)
			{
				var p = new Petrol {Name = "Okko"};
				dataContext.Petrols.InsertOnSubmit(p);
				dataContext.SubmitChanges();

				Assert.AreEqual("Okko", dataContext.Petrols.Single().Name);
			}
		}

		[TestMethod]
		public void FillupSaveTest()
		{
			var ensureDatabaseTask = FuelTrackerDataContext.EnsureDatabase();
			ensureDatabaseTask.Wait();
			using (var dataContext = ensureDatabaseTask.Result)
			{

				var f = new Fillup
				        {
					        Date = DateTime.Now,
					        Odometer = 20000,
					        Petrol = new Petrol {Name = "Okko"},
					        Price = 10.29M,
					        Volume = 100
				        };
				dataContext.Fillups.InsertOnSubmit(f);

				dataContext.SubmitChanges();


				var fillup = dataContext.Fillups.Single();
				Assert.AreEqual(20000, fillup.Odometer);
			}
		}
	}
}
