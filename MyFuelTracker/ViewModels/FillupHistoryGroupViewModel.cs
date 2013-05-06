using System;
using System.Collections.Generic;

namespace MyFuelTracker.ViewModels
{
	public class FillupHistoryGroupViewModel : List<FillupHistoryItemViewModel>
	{
		public string Month { get { return MonthDateTime.ToString("MMM yyyy"); } }

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