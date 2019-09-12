using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternetShop.Models
{
    public class PostModels
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id;
        public string CategoryId { get; set; }
        public string UserId { get; set; }
        public string Header { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }

        public List<String> KeyWordList = new List<String>();
        public List<CharacteristicModels> CharacteristicList = new List<CharacteristicModels>();
        public List<CommentModels> CommentList = new List<CommentModels>();

        public string ImageId { get; set; }
        public bool HasImage()
        {
            return !String.IsNullOrWhiteSpace(ImageId);
        }
    }
}