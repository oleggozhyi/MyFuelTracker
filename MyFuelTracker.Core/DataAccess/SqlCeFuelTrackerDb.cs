using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MyFuelTracker.Core.Models;
using Newtonsoft.Json;

namespace MyFuelTracker.Core.DataAccess
{
    public class SqlCeFuelTrackerDb : IFuelTrackerDb
    {
        private bool _initialized;
        private FuelTrackerDataContext _dataContext;

        public Table<Fillup> Fillups { get { return _dataContext.GetTable<Fillup>(); } }

        public SqlCeFuelTrackerDb()
        {
            _dataContext = new FuelTrackerDataContext("Data Source=isostore:/fueltrackerdb.v1.sdf");
        }

        public Task SaveFillupAsync(Fillup fillup)
        {
            return Task.Run(() =>
            {
                if (!Fillups.Any(f => f.Id == fillup.Id))
                    Fillups.InsertOnSubmit(fillup);

                _dataContext.SubmitChanges();
            });
        }

        public Task DeleteFillupAsync(Fillup fillup)
        {
            return Task.Run(() =>
                {
                    Fillups.DeleteOnSubmit(fillup);
                    _dataContext.SubmitChanges();
                });
        }

        public Task<Fillup[]> LoadAllFillupsAsync()
        {
            EnsureDatabase();
            return Task.Run(() => Fillups.OrderBy(f => f.Date).ToArray());
        }

        private void EnsureDatabase()
        {
            if (_initialized)
                return;

            if (!_dataContext.DatabaseExists())
            {
                _dataContext.CreateDatabase();
#if(DEBUG)
                using (var stream =Assembly.GetExecutingAssembly().GetManifestResourceStream("MyFuelTracker.Core.DataAccess.fillups.json"))
                using (var reader = new StreamReader(stream))
                {
                    var fillups = JsonConvert.DeserializeObject<Fillup[]>(reader.ReadToEnd());
                    Fillups.InsertAllOnSubmit(fillups);
                    _dataContext.SubmitChanges();
                }
            }
#endif

            _initialized = true;
        }


    }

}
