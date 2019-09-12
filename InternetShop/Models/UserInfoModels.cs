using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InternetShop.Models
{
    public class UserInfoModels
    {
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }
        [Display(Name = "Номер телефона")]
        public string Number { get; set; }
        [Display(Name = "Город")]
        public string City { get; set; }

        private Dictionary<string, string> additionalInfoDict = new Dictionary<string, string>() { { "first", "second" } };

        public Dictionary<string, string> AdditionalInfoDict { get { return additionalInfoDict; }  set { additionalInfoDict = value; } }
    }
}