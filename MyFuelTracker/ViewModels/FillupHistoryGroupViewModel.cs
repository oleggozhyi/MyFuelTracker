using System;
using System.Collections.Generic;
using System.Threading;

namespace MyFuelTracker.ViewModels
{
	public class FillupHistoryGroupViewModel : List<FillupHistoryItemViewModel>
	{
		public string MonthLongName { get { return MonthDateTime.ToString("MMMM yyyy", Thread.CurrentThread.CurrentUICulture); } }

		public DateTime MonthDateTime { get; set; }

		protected bool Equals(FillupHistoryGroupViewModel other)
		{
			return MonthDateTime.Equals(other.MonthDateTime);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((FillupHistoryGroupViewModel) obj);
		}

		public override int GetHashCode()
		{
			return MonthDateTime.GetHashCode();
		}

		public static bool operator ==(FillupHistoryGroupViewModel left, FillupHistoryGroupViewModel right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(FillupHistoryGroupViewModel left, FillupHistoryGroupViewModel right)
		{
			return !Equals(left, right);
		}
	}
}