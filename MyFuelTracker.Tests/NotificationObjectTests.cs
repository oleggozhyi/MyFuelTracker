using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MyFuelTracker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFuelTracker.Tests
{
	[TestClass]
	public class NotificationObjectTests
	{
		[TestMethod]
		public void Notifies_when_valuetype_property_changed()
		{
			//arrange
			var obj = new MyNotificationObject();
			bool notified = false;
			obj.PropertyChanged += (s,e) =>
				{
					notified = true;
					Assert.AreEqual("StringProperty", e.PropertyName);
					Assert.AreEqual(obj, s);
				};
			
			//act
			obj.StringProperty = "new value";

			//assert
			Assert.IsTrue(notified);
			Assert.AreEqual("new value", obj.StringProperty);

		}

		[TestMethod]
		public void Notifies_when_string_property_changed()
		{
			//arrange
			var obj = new MyNotificationObject();
			bool notified = false;
			obj.PropertyChanged += (s, e) =>
			{
				notified = true;
				Assert.AreEqual("IntProperty", e.PropertyName);
				Assert.AreEqual(obj, s);
			};

			//act
			obj.IntProperty = 42;

			//assert
			Assert.IsTrue(notified);
			Assert.AreEqual(42, obj.IntProperty);
		}

		[TestMethod]
		public void Notifies_when_custom_reference_property_changed()
		{
			//arrange
			var obj = new MyNotificationObject();
			bool notified = false;
			obj.PropertyChanged += (s, e) =>
			{
				notified = true;
				Assert.AreEqual("CustomProperty", e.PropertyName);
				Assert.AreEqual(obj, s);
			};

			//act
			var classA = new ClassA();
			obj.CustomProperty = classA; 

			//assert
			Assert.IsTrue(notified);
			Assert.AreEqual(classA, obj.CustomProperty);
		}

		private class ClassA { }
		private class MyNotificationObject : ViewModelBase
		{
			private string _stringProperty;
			private ClassA _customProperty;
			private int _intProperty;

			public string StringProperty
			{
				get { return _stringProperty; }
				set { SetProperty(value, ref _stringProperty); }
			}

			public int IntProperty
			{
				get { return _intProperty; }
				set { SetProperty(value, ref _intProperty); }

			}

			public ClassA CustomProperty
			{
				get { return _customProperty; }
				set { SetProperty(value, ref _customProperty); }
			}
		}
	}
}
