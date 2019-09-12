using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternetShop.Models
{
    public class KeyWordModels
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id;
        public string Name { get; set; }
        public string Description { get; set; }
    }
}