using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternetShop.Models
{
    public class PostFilter
    {
        public string Header { get; set; }
        public string CategoryId { get; set; }

        public List<string> KeyWords { get; set; }
    }
}