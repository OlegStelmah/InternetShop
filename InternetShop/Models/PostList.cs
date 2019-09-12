using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternetShop.Models
{
    public class PostList
    {
        public IEnumerable<PostModels> Posts { get; set; }

        public IEnumerable<CategoryModels> Categorys { get; set; }

        public PostFilter Filter { get; set; }

        public IPagedList<PostModels> PagedPosts { get; set; }

    }


}