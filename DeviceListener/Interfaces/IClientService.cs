using DeviceListener.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeviceListener.Interfaces
{
    public interface IClientService
    {
        Task<IEnumerable<Client>> GetClientsAsync();
        Task<Client> GetClientAsync(string id);
        Task CreateAsync(Client client);
        Task RemoveAsync(Client client);
        Task UpdateAsync(Client client);
    }
}
