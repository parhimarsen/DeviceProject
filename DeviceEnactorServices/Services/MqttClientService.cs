using DeviceEnactor.Interfaces;
using DeviceEnactor.Models;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.Text;
using System.Threading.Tasks;

namespace DeviceEnactorServices
{
    public class MqttClientService : IMqttClientService
    {
        string mqttURI = "broker.hivemq.com";
        string mqttUser = "";
        string mqttPassword = "";
        int mqttPort = 1883;
        bool mqttSecure = false;
        static IManagedMqttClient clientMqtt = new MqttFactory().CreateManagedMqttClient();

        public async Task ConnectAsync(string clientId)
        {
            var messageBuilder = new MqttClientOptionsBuilder()
               .WithClientId(clientId)
               .WithCredentials(mqttUser, mqttPassword)
               .WithTcpServer(mqttURI, mqttPort)
               .WithCleanSession();

            var options = mqttSecure
                ? messageBuilder
                .WithTls()
                .Build()
                : messageBuilder
                .Build();

            var managedOptions = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(100))
                .WithClientOptions(options)
                .Build();
            if (clientMqtt != null)
            {
                await clientMqtt.StartAsync(managedOptions);
            }
        }

        public async Task PublishAsync(MqttApplicationMessage message)
        {
            if (clientMqtt != null)
            {
                clientMqtt.PublishAsync(message);
            }
        }

        public async Task SubscribeAsync(string topic, int qos = 1)
        {
            if (clientMqtt != null)
            {
                await clientMqtt.SubscribeAsync(new TopicFilterBuilder()
                    .WithTopic(topic)
                    .WithQualityOfServiceLevel((MQTTnet.Protocol.MqttQualityOfServiceLevel)qos)
                    .Build());
            }
        }

        public async Task UnsubscribeAsync(string topic)
        {
            await clientMqtt.UnsubscribeAsync(topic);
        }

        public async Task<DeviceMqtt> ReceivePayload(DeviceMqtt device)
        {
            clientMqtt.UseApplicationMessageReceivedHandler(e => { device.Status = Encoding.UTF8.GetString(e.ApplicationMessage.Payload); });
            return device;
        }

        private static void HandleMessageReceived(DeviceMqtt device, MqttApplicationMessage applicationMessage)
        {
            device.Status = Encoding.UTF8.GetString(applicationMessage.Payload);
        }


    }
}
