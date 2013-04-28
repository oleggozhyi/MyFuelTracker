using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace MyFuelTracker.Tests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void Failing_Test()
		{
			Assert.Fail("as");
		}

		[TestMethod]
		public void Passing_Test()
		{
			Assert.AreEqual(1, 1);

		}
	}
}
