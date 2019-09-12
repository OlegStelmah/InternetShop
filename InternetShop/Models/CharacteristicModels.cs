using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternetShop.Models
{
    public class CharacteristicModels
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string Id { get; set; }
        public List<CharacteristicInfoModels> CharacteristicInfoList = new List<CharacteristicInfoModels>();
    }
}