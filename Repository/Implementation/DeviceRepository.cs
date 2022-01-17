using Domain.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Repository.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System.Linq;

namespace Repository.Implementation
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly string connectionString;

        public DeviceRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("Sqlite");
        }

        public async Task DeleteUserWithDevices(long userId)
        {
            string query = "DELETE FROM Device WHERE Device.UserID = @UserId";

            using SqliteConnection conn = GetConnection();
            object parameters = new { UserId = userId };
            await conn.QueryAsync(query, parameters);
        }

        public async Task<List<Device>> FindAllDevicesAsync(long userId)
        {
            string query = "SELECT * FROM Device WHERE Debice.UserID = @UserId";

            using SqliteConnection conn = GetConnection();
            object parameters = new { UserId = userId };
            IEnumerable<Device> devices = await conn.QueryAsync<Device>(query, parameters);
            return devices.ToList();
        }

        public async Task SaveDeviceAsync(long ownerId, Device device)
        {
            string query = "INSERT INTO Device " +
                "(UserID, Browser, DeviceType, OperatingSystem, IpAddress) " +
                "VALUES (@UserId, @Browser, @Type, @System, @Ip)";

            using SqliteConnection conn = GetConnection();
            object parameters = new
            {
                UserId = ownerId,
                Browser = device.Browser,
                Type = device.DeviceType,
                System = device.OperatingSystem,
                Ip = device.IpAddress
            };
            await conn.QueryAsync(query, parameters);
        }

        public async Task<bool> VerifyDeviceAsync(long ownerId, Device device)
        {
            string query = "SELECT COUNT(1) FROM Device WHERE " +
                "Browser = @Browser AND DeviceType = @DeviceType AND " +
                "OperatingSystem = @System AND IpAddress = @Ip " +
                "AND UserID = @UserId LIMIT 1";

            using SqliteConnection conn = GetConnection();
            object parameters = new
            {
                Browser = device.Browser,
                DeviceType = device.DeviceType,
                System = device.OperatingSystem,
                Ip = device.IpAddress,
                UserId = ownerId
            };
            return await conn.QuerySingleAsync<bool>(query, parameters);
        }

        private SqliteConnection GetConnection()
        {
            return new SqliteConnection(connectionString);
        }
    }
}
