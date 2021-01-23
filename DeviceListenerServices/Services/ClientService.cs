using DeviceEnactor.Interfaces;
using DeviceListener.Interfaces;
using DeviceListener.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MQTTnet;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeviceListenerServices.Services
{
    public class ClientService : IClientService
    {
        IGridFSBucket gridFS;
        IMongoCollection<Client> Clients;
        IDeviceService _deviceService;

        private readonly IMqttClientService _mqttClientService;

        public ClientService(IMqttClientService mqttClientService, IDeviceService deviceService)
        {
            _deviceService = deviceService;
            _mqttClientService = mqttClientService;
            string connectionString = "mongodb+srv://Parhim:youra23071966@wt.u0tvq.azure.mongodb.net/DevicesProject?retryWrites=true&w=majority";
            var connection = new MongoUrlBuilder(connectionString);
            // получаем клиента для взаимодействия с базой данных
            MongoClient client = new MongoClient(connectionString);
            // получаем доступ к самой базе данных
            IMongoDatabase database = client.GetDatabase(connection.DatabaseName);
            // получаем доступ к файловому хранилищу
            gridFS = new GridFSBucket(database);
            // обращаемся к коллекции Products
            Clients = database.GetCollection<Client>("Clients");
        }

        public async Task<IEnumerable<Client>> GetClientsAsync()
        {
            // строитель фильтров
            var builder = new FilterDefinitionBuilder<Client>();
            var filter = builder.Empty;
            return await Clients.Find(filter).ToListAsync();
        }

        public async Task<Client> GetClientAsync(string id)
        {
            return await Clients.Find<Client>(client => client.Id == id).FirstOrDefaultAsync();
        }

        // добавление документа
        public async Task CreateAsync(Client client)
        {
            //IManagedMqttClient clientMqtt = new MqttFactory().CreateManagedMqttClient();
            //await _mqttClientService.ConnectAsync(client.Id);
            //client.clientMqttJson = JsonConvert.SerializeObject(clientMqtt);
            await Clients.InsertOneAsync(client);
        }

        public async Task RemoveAsync(Client client)
        {
            var devicesOfClient = await _deviceService.GetAllDevicesOfClientAsync(client);
            foreach (var device in devicesOfClient)
            {
                await _deviceService.RemoveAsync(client, device);
            }
            await Clients.DeleteOneAsync(new BsonDocument("_id", new ObjectId(client.Id)));
        }

        // обновление документа
        public async Task UpdateAsync(Client client)
        {
            await Clients.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(client.Id)), client);
        }
    }
}
