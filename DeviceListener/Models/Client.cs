using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeviceListener.Models
{
    public class Client
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Display(Name = "Имя")]
        public string Name { get; set; }
        public string clientMqttJson { get; set; }
    }
}
