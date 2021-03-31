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
    public class DevicesDB : IEnumerable
    {
        readonly SQLiteAsyncConnection _database;
        public DevicesDB(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Device>().Wait();
        }
        public Task<int> SaveTelemetryAsync(Device dev)
        {
            var val = _database.Table<Device>().Where(i => i.deviceICCID == dev.deviceInDate).FirstOrDefaultAsync();

            if (val.Result == null)
            {
                return _database.InsertAsync(dev);
            }
            else
            {
                if (dev.deviceICCID == val.Result.deviceICCID)
                {
                    return _database.UpdateAsync(dev);
                }
                else
                {
                    return _database.InsertAsync(dev);
                }
            }
        }

        public Task<Device> getLastReadingDate()
        {
            return _database.Table<Device>()
               .OrderByDescending(a => a.deviceInDate).FirstOrDefaultAsync();

        }

        public Task<List<Device>> getAllDevices()
        {
            return _database.Table<Device>()
               .OrderByDescending(a => a.deviceICCID).ToListAsync();

        }
        public Task<List<Device>> getLastThreeReadings()
        {
            return _database.Table<Device>()
                .OrderByDescending(a => a.deviceInDate).Take(3).ToListAsync();

        }
        public Task<List<Device>> getAllActiveTelemetry()
        {
            return _database.Table<Device>().OrderByDescending(a => a.deviceInDate).ToListAsync();

        }
        public Task<List<Device>> getTotalBalanceCostPerMonth()
        {
            return _database.Table<Device>().OrderByDescending(a => a.deviceInDate).ToListAsync();

        }
        public Task<int> DeleteTelemetryAsync(Device device)
        {
            return _database.DeleteAsync(device);
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}