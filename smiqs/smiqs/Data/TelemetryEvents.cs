using smiqs.Models;
using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace smiqs.Data
{
    public class TelemetryEvents : IEnumerable
    {
        readonly SQLiteAsyncConnection _database;
        public ObservableCollection<tblEvents> Telemtries { get; private set; }

        public TelemetryEvents(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<tblEvents>().Wait();
        }

        public Task<int> SaveTelemetryAsync(tblEvents telemetry)
        {
            var val = _database.Table<tblEvents>().Where(i => i.id == telemetry.id).FirstOrDefaultAsync();

            if (val.Result == null)
            {
                return _database.InsertAsync(telemetry);
            }
            else
            {
                if (telemetry.id == val.Result.id)
                {
                    return _database.UpdateAsync(telemetry);
                }
                else
                {
                    return _database.InsertAsync(telemetry);
                }
            }
        }

        public Task<tblEvents> getLastReadingDate()
        {
            return _database.Table<tblEvents>()
               .OrderByDescending(a => a.eventDate).FirstOrDefaultAsync();

        }
        public Task<tblEvents> getLastReadingDate(string iccid,string sensor)
        {
            return _database.Table<tblEvents>().Where(i => i.DeviceID == iccid && i.valveName.Contains(sensor))
               .OrderByDescending(a => a.eventDate).FirstOrDefaultAsync();

        }
        public Task<List<Device>> getAllDevices()
        {
            return _database.Table<Device>()
               .OrderByDescending(a => a.deviceICCID).ToListAsync();

        }
        public Task<List<tblEvents>> getLastThreeReadings()
        {
            return _database.Table<tblEvents>()
                .OrderByDescending(a => a.eventDate).Take(3).ToListAsync();

        }
        public Task<List<tblEvents>> getAllActiveTelemetry()
        {
            return _database.Table<tblEvents>().OrderByDescending(a => a.eventDate).ToListAsync();

        }

        public Task<List<tblEvents>> getAllActiveTelemetry(string iccid)
        {
            return _database.Table<tblEvents>().Where(i => i.DeviceID == iccid && i.elapsedtime != "0").OrderByDescending(a => a.eventDate).ToListAsync();

        }
        public Task<List<tblEvents>> getAllActiveTelemetry(string iccid,string sensor)
        {
            return _database.Table<tblEvents>().Where(i => i.DeviceID == iccid && i.elapsedtime != "0" && i.valveName.Contains(sensor)).OrderByDescending(a => a.eventDate).ToListAsync();

        }
      
        public Task<List<tblEvents>> getTotalBalanceCostPerMonth()
        {
            return _database.Table<tblEvents>().OrderByDescending(a => a.eventDate).ToListAsync();

        }
        public Task<List<tblEvents>> getTotalBalanceCostPerMonth(string iccid)
        {
            return _database.Table<tblEvents>().Where(i => i.DeviceID == iccid).OrderByDescending(a => a.eventDate).ToListAsync();

        }
        public Task<int> DeleteTelemetryAsync(tblEvents telemetry)
        {
            return _database.DeleteAsync(telemetry);
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
