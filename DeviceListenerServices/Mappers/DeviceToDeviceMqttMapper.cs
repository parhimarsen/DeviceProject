using DeviceEnactor.Models;
using DeviceListener.Models;

namespace DeviceListenerServices.Mappers
{
    public static class DeviceToDeviceMqttMapper
    {
        public static DeviceMqtt ToMqtt(this Device @this)
        {
            return new DeviceMqtt()
            {
                Id = @this.Id,
                Name = @this.Name,
                Topic = @this.Topic,
                TypeOfDevice = @this.TypeOfDevice,
                Payload = @this.Payload,
                Status = @this.Status,
                ClientId = @this.ClientId,
            };
        }
    }
}
