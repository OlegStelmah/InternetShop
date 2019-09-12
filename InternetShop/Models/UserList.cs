using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternetShop.Models
{
    public class UserList
    {
        public IEnumerable<UserModels> Users { get; set; }
        public UserFilter Filter { get; set; }
    }

    public class UserFilter
    {
        public string Id { get; set; }
    }

}