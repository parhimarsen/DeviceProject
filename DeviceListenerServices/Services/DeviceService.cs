using DeviceEnactor.Interfaces;
using DeviceListener.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MQTTnet;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DeviceListener.Interfaces;
using Newtonsoft.Json;
using DeviceEnactor;
using DeviceListenerServices.Mappers;

namespace DeviceListenerServices.Services
{
    public class DeviceService : IDeviceService
    {
        IGridFSBucket gridFS;
        IMongoCollection<Device> Devices;
        ConnectSingleton connectSingleton;

        private readonly IMqttClientService _mqttClientService;

        public DeviceService(IMqttClientService mqttClientService)
        {
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
            Devices = database.GetCollection<Device>("Devices");
        }

        public async Task<IEnumerable<Device>> GetDevicesAsync()
        {
            // строитель фильтров
            var builder = new FilterDefinitionBuilder<Device>();
            var filter = builder.Empty; ;
            return await Devices.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Device>> GetAllDevicesOfClientAsync(Client client)
        {
            return await Devices.Find<Device>(new BsonDocument("ClientId", client.Id)).ToListAsync();
        }

        // добавление документа
        public async Task CreateAsync(Client client, Device device)
        {
            //var clientMqtt = JsonConvert.DeserializeObject<IManagedMqttClient>(client.clientMqttJson);
            //IManagedMqttClient clientMqtt = new MqttFactory().CreateManagedMqttClient();
            //await _mqttClientService.SubscribeAsync(clientMqtt, device.Topic);
            //clientMqtt.UseApplicationMessageReceivedHandler(e => { HandleMessageReceived(client, device, e.ApplicationMessage); });
            Devices.InsertOne(device);
        }
        // удаление документа
        public async Task RemoveAsync(Client client, Device device)
        {
            //var clientMqtt = JsonConvert.DeserializeObject<IManagedMqttClient>(client.clientMqttJson);
            //IManagedMqttClient clientMqtt = new MqttFactory().CreateManagedMqttClient();
            //await _mqttClientService.UnsubscribeAsync(clientMqtt, device.Topic);
            await Devices.DeleteOneAsync(new BsonDocument("_id", new ObjectId(device.Id)));
        }
        // обновление документа
        public async Task UpdateFromUIAsync(Client client, Device device)
        {
            if (device.TypeOfDevice == "Light")
            {
                if (ConnectSingleton.IsConnected == false)
                {
                    await _mqttClientService.ConnectAsync(client.Id);
                    await _mqttClientService.SubscribeAsync(device.Topic);
                    ConnectSingleton.getInstance(true);
                }
                var messagePayload = new MqttApplicationMessageBuilder()
                                     .WithTopic(device.Topic)
                                     .WithPayload(device.Payload)
                                     .WithExactlyOnceQoS()
                                     .WithRetainFlag()
                                     .Build();
                await _mqttClientService.PublishAsync(messagePayload);
            }
            await Devices.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(device.Id)), device);
        }

        public async Task UpdateAsync(Device device)
        {
            await Devices.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(device.Id)), device);
        }

        public async Task SetDeviceStatus(Client client, Device device)
        {
            if (ConnectSingleton.IsConnected == false)
            {
                await _mqttClientService.ConnectAsync(client.Id);
                await _mqttClientService.SubscribeAsync(device.Topic);
                ConnectSingleton.getInstance(true);
            }

            var messagePayload = new MqttApplicationMessageBuilder()
                                 .WithTopic(device.Topic)
                                 .WithPayload(device.Payload)
                                 .WithExactlyOnceQoS()
                                 .WithRetainFlag()
                                 .Build();
            await _mqttClientService.PublishAsync(messagePayload);

            var newDevice = await _mqttClientService.ReceivePayload(device.ToMqtt());

            var check = newDevice.Status;
        }
    }
}
