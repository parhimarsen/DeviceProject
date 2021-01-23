using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace DeviceListener.Models
{
    public class Device
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Topic { get; set; }
        public string TypeOfDevice { get; set; }
        [Display(Name = "Данные")]
        public string Payload { get; set; }
        public string Status { get; set; }
        public string ClientId { get; set; }
    }
}
