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
   public class TelemetryDB : IEnumerable
    {
        readonly SQLiteAsyncConnection _database;
        public ObservableCollection<Telemetry> Telemtries { get; private set; }
        public TelemetryDB(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Telemetry>().Wait();
        }

        public Task<List<Telemetry>> GetTelemetriesAsync()
        {
            return _database.Table<Telemetry>().ToListAsync();
        }

        public Task<Telemetry> GetTelemetryAsync(int id)
        {
            return _database.Table<Telemetry>()
                            .Where(i => i.telID == id)
                            .FirstOrDefaultAsync();
        }
        public Task<Telemetry> GetTelemetryRecent()
        {
            return _database.Table<Telemetry>()
                .Where(a => a.VA == true && a.elapsedtime != 0).OrderByDescending(a => a.teltimestamp).FirstOrDefaultAsync();
        }
        public Task<List<Telemetry>> GetTelemetrybyDate(DateTime startDateTime,DateTime endDateTime)
        {
          
            return _database.Table<Telemetry>()
                .Where(a => a.teltimestamp >= startDateTime && a.teltimestamp <= endDateTime && a.VA == true && a.elapsedtime != 0).ToListAsync();
        }
       
        public Task<Telemetry> GetTelemetryYear()
        {
            return _database.Table<Telemetry>()
                .Where(a => a.VA == true && a.elapsedtime != 0).OrderByDescending(a => a.teltimestamp).FirstOrDefaultAsync();
        }
        public Task<int> SaveTelemetryAsync(Telemetry telemetry)
        {
            var val =_database.Table<Telemetry>().Where(i => i.telID == telemetry.telID).FirstOrDefaultAsync();

            if (val.Result == null)
            {
                return _database.InsertAsync(telemetry);
            }
            else { 
            if (telemetry.telID == val.Result.telID)
            {
                return _database.UpdateAsync(telemetry);
            }
            else
            {
                return _database.InsertAsync(telemetry);
            }
            }
        }
        public Task<Telemetry> getLastReadingDate()
        {
            return _database.Table<Telemetry>()
               .OrderByDescending(a => a.teltimestamp).FirstOrDefaultAsync();

        }
        public Task<List<Telemetry>> getLastThreeReadings()
        {
            return _database.Table<Telemetry>().Where(a => a.VA == true && a.elapsedtime != 0)
               .OrderByDescending(a => a.teltimestamp).Take(3).ToListAsync();

        }
        public Task<List<Telemetry>> getAllActiveTelemetry()
        {
            return _database.Table<Telemetry>().Where(a => a.VA == true && a.elapsedtime != 0)
               .OrderByDescending(a => a.teltimestamp).ToListAsync();

        }
        public Task<List<Telemetry>> getTotalBalanceCostPerMonth()
        {
            return _database.Table<Telemetry>().Where(a => a.VA == true && a.elapsedtime != 0)
               .OrderByDescending(a => a.teltimestamp).ToListAsync();

        }
        public Task<int> DeleteTelemetryAsync(Telemetry telemetry)
        {
            return _database.DeleteAsync(telemetry);
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}