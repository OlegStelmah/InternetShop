using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternetShop.Models
{
    public class CommentModels
    {
        public string AuthorId;
        public DateTime Date;
        public string Text;
        public List<CommentModels> comments = new List<CommentModels>();
    }
}