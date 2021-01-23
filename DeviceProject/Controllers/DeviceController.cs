using DeviceListener.Interfaces;
using DeviceListener.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviceProject.Controllers
{
    [Route("api/[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _db;
        private readonly IClientService _clientService;

        public DeviceController(IDeviceService db, IClientService clientService)
        {
            _db = db;
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<IEnumerable<Device>> Get()
        {
            return await _db.GetDevicesAsync();
        }

        [HttpGet, Route("{clientId}")]
        public async Task<IEnumerable<Device>> Get(string clientId)
        {
            var client = await _clientService.GetClientAsync(clientId);
            var devices = await _db.GetAllDevicesOfClientAsync(client);
            return devices;
        }

        [HttpPost]
        public async Task Post(Client client, Device device)
        {
            if (client != null || device != null)
            {
                device.ClientId = client.Id;
                await _db.CreateAsync(client, device);
            }
        }

        [HttpDelete]
        public async Task Delete(Client client, Device device)
        {
            if (client != null || device != null)
            {
                await _db.RemoveAsync(client, device);
            }
        }

        [HttpPut]
        public async Task Update(Client client, Device device)
        {
            device.ClientId = client.Id;
            //await _db.SetDeviceStatus(client, device);
            await _db.UpdateFromUIAsync(client, device);
        }

    }
}
