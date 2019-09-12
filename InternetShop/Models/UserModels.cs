using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InternetShop.Models
{
    public class UserModels
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Display(Name="Имя пользователя")]
        public string UserName { get; set; }
        [Display(Name = "Email:")]
        public string Email { get; set; }
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        public UserInfoModels UserInfo { get; set; }
    }
}