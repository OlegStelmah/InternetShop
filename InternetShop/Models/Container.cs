using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternetShop.Models
{
    public class Container
    {
        public PostModels Post { get; set; }
        public UserModels User { get; set; }

        public bool isOwner = false;
    }
}