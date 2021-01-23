using DeviceEnactor.Models;
using DeviceListener.Models;

namespace DeviceListenerServices.Mappers
{
    public static class DeviceMqttToDeviceMapper
    {
        public static Device To(this DeviceMqtt @this)
        {
            return new Device()
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
