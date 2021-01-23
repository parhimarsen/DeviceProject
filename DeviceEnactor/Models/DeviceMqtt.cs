namespace DeviceEnactor.Models
{
    public class DeviceMqtt
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Topic { get; set; }
        public string TypeOfDevice { get; set; }
        public string Payload { get; set; }
        public string Status { get; set; }
        public string ClientId { get; set; }
    }
}
