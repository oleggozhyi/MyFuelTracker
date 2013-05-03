using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace MyFuelTracker.Tests.TestInfrastructure
{
	public static class UnitTestExtensions
	{
		#region Date helpers

		public static DateTime Jan(this int day, int year) { return new DateTime(year, 1, day); }
		public static DateTime Feb(this int day, int year) { return new DateTime(year, 2, day); }
		public static DateTime Mar(this int day, int year) { return new DateTime(year, 3, day); }
		public static DateTime Apr(this int day, int year) { return new DateTime(year, 4, day); }
		public static DateTime May(this int day, int year) { return new DateTime(year, 5, day); }
		public static DateTime Jun(this int day, int year) { return new DateTime(year, 6, day); }
		public static DateTime Jul(this int day, int year) { return new DateTime(year, 7, day); }
		public static DateTime Aug(this int day, int year) { return new DateTime(year, 8, day); }
		public static DateTime Sep(this int day, int year) { return new DateTime(year, 9, day); }
		public static DateTime Oct(this int day, int year) { return new DateTime(year, 10, day); }
		public static DateTime Nov(this int day, int year) { return new DateTime(year, 11, day); }
		public static DateTime Dec(this int day, int year) { return new DateTime(year, 12, day); }

		#endregion

		public static T WaitAndReturn<T>(this Task<T> task)
		{
			task.Wait(3000);
			return task.Result;
		}

		public static void ShouldBe<T>(this T actual, T expected)
		{
			Assert.AreEqual(expected, actual);
		}

		public static void ShouldRoughlyBe(this double actual, double expected, double epsilon)
		{
			Assert.IsTrue(Math.Abs(actual - expected) < epsilon);
		}

		public static void ShouldNotBe<T>(this T actual, T expected)
		{
			Assert.AreNotEqual(expected, actual);
		}

		public static void ShouldThrow<T>(this Action action) where T : Exception
		{
			bool thrown = false;
			try
			{
				action();

			}
			catch (Exception ex)
			{
				if (ex.InnerException.GetType() != typeof(T))
					throw new AssertFailedException(string.Format("Expected exception of type '{0}' but '{1}' thrown",
						typeof(T).FullName, ex.InnerException.GetType()));

				thrown = true;
			}
			if (!thrown)
				throw new AssertFailedException("No exception was thrown");

		}

		public static void ShouldNotThrow(this Action action)
		{
			try
			{
				action();

			}
			catch (Exception ex)
			{
				throw new AssertFailedException(string.Format("Expected no exception but '{0}' thrown", ex.GetType()));
			}
		}
	}
}