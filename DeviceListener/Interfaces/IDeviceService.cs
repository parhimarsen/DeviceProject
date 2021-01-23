using DeviceListener.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeviceListener.Interfaces
{
    public interface IDeviceService
    {
        Task<IEnumerable<Device>> GetDevicesAsync();
        Task<IEnumerable<Device>> GetAllDevicesOfClientAsync(Client client);
        Task CreateAsync(Client client, Device device);
        Task RemoveAsync(Client client, Device device);
        Task UpdateFromUIAsync(Client client, Device device);
        Task UpdateAsync(Device device);
        Task SetDeviceStatus(Client client, Device device);
    }
}
