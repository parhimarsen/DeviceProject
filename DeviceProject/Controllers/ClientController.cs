using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviceListener.Models;
using DeviceListener.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeviceProject.Controllers
{
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _db;

        public ClientController(IClientService db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IEnumerable<Client>> Get()
        {
            return await _db.GetClientsAsync();
        }

        [HttpPost]
        public async Task Post(Client client)
        {
            //client.Id = Guid.NewGuid().ToString();
            await _db.CreateAsync(client);
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            Client client = await _db.GetClientAsync(id);
            if (client != null)
            {
                await _db.RemoveAsync(client);
            }
        }
    }
}
