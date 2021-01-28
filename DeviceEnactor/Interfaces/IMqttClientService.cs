using DeviceEnactor.Models;
using MQTTnet;
using System.Threading.Tasks;

namespace DeviceEnactor.Interfaces
{
    public interface IMqttClientService
    {
        Task ConnectAsync(string clientId);
        Task PublishAsync(MqttApplicationMessage message);
        Task SubscribeAsync(string topic, int qos = 1);

        Task UnsubscribeAsync(string topic);
        Task<DeviceMqtt> ReceivePayload(MqttApplicationMessage message, DeviceMqtt device);
    }
}
