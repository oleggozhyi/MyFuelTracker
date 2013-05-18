using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MyFuelTracker.Core.DataAccess;
using MyFuelTracker.Core.Models;

namespace MyFuelTracker.Core
{
    public class SqlCeFuelTrackerDb : DataContext, IFuelTrackerDb
    {
        private bool _initialized;

        public Table<Fillup> Fillups;

        public SqlCeFuelTrackerDb()
            : base("Data Source=isostore:/fueltrackerdb.v1.sdf")
        {
        }

        public Task SaveFillupAsync(Fillup fillup)
        {
            return Task.Run(() =>
            {
                Fillups.InsertOnSubmit(fillup);
                SubmitChanges();
            });
        }

        public Task DeleteFillupAsync(Fillup fillup)
        {
            return Task.Run(() =>
                {
                    Fillups.DeleteOnSubmit(fillup);
                    SubmitChanges();
                });
        }

        public Task<Fillup[]> LoadAllFillupsAsync()
        {
            EnsureDatabase();
            return Task.Run(() => Fillups.OrderBy(f=>f.Date).ToArray());
        }

        private void EnsureDatabase()
        {
            if (_initialized)
                return;

            if (!DatabaseExists())
            {
                CreateDatabase();
#if(DEBUG)
                using (
                    var stream =
                        Assembly.GetExecutingAssembly()
                                .GetManifestResourceStream("MyFuelTracker.Core.DataAccess.fillups.xml"))
                {
                    var fillups = new List<Fillup>();
                    var xDocument = XDocument.Load(stream);
                    foreach (var f in xDocument.Root.Elements("fillup"))
                    {
                        var fillup = new Fillup();
                        fillups.Add(fillup);
                        fillup.Id = Guid.Parse(f.Element("id").Value);
                        fillup.Date = DateTime.Parse(f.Element("date").Value, CultureInfo.CurrentCulture);
                        fillup.Volume = double.Parse(f.Element("volume").Value, CultureInfo.InvariantCulture);
                        fillup.FuelType = f.Element("petrol").Value;
                        fillup.Price = double.Parse(f.Element("price").Value, CultureInfo.InvariantCulture);
                        fillup.IsPartial = bool.Parse(f.Element("is-partial").Value);
                        fillup.OdometerStart = double.Parse(f.Element("odometer-start").Value,
                                                            CultureInfo.InvariantCulture);
                        fillup.OdometerEnd = double.Parse(f.Element("odometer-end").Value, CultureInfo.InvariantCulture);
                    }

                    Fillups.InsertAllOnSubmit(fillups);
                    SubmitChanges();
                }
            }
#endif

            _initialized = true;
        }


    }

}
