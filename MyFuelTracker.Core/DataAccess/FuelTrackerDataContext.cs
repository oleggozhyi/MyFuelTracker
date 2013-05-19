using System.Data.Linq;
using MyFuelTracker.Core.Models;

namespace MyFuelTracker.Core.DataAccess
{
    public class FuelTrackerDataContext : DataContext
    {
        public FuelTrackerDataContext(string fileOrConnection) : base(fileOrConnection)
        {
        }

        public Table<Fillup> Fillups;
    }
}