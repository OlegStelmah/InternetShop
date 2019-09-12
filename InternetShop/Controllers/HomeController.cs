using InternetShop.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace InternetShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserContext db = new UserContext();
        private readonly PostContext dbPost = new PostContext();
        private readonly CategoryContext dbCategory = new CategoryContext();
        private string tmpPostId { get; set; }
        private string CurrentUserId { get; set; }

        private PostModels tmpProd { get; set; }

        public async Task<ActionResult> UserIndex(UserFilter filter)
        {
            var users = await db.GetUsers(filter.Id);
            var model = new UserList { Users = users, Filter = filter };
            return View(model);
        }

        public async Task<ActionResult> Index(int? page, PostFilter filter)
        {
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            var posts = await dbPost.GetPosts(filter);
            var category = await dbCategory.GetAllCategories(); 
            var model = new PostList { Posts = posts, Filter = filter, PagedPosts = posts.ToPagedList(pageNumber,pageSize), Categorys = category };
            return View(model);
        }
        public async Task<ActionResult> Edit(string id)
        {
            UserModels c = await db.GetUser(id);
            if (c == null)
                return HttpNotFound();
            return View(c);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(UserModels user)
        {
            if (ModelState.IsValid)
            {
                await db.Update(user);
                return RedirectToAction("UserIndex");
            }
            return View(user);
        }
        
        public async Task<ActionResult> EditPost(string userId, string postId)
        {
            UserModels user =await db.GetUser(userId);
            PostModels post = await dbPost.GetPost(postId);
            Container cont = new Container() { Post = post, User = user };
            return View(cont);
        }

        public async Task<ActionResult> DeletePost(string postId)
        {
            await dbPost.Remove(postId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> EditPost(PostModels post)
        {
            post.Date = DateTime.Now;
            await dbPost.Update(post);
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> CreateCharacteristic(string id)
        {
            
            var post = await dbPost.GetPost(id);
            var charac = new CharacteristicModels() { Id = id};
            ViewBag.Id = id;
            Session["id"] = id;
            var map = new CharactContainer() { post = post, character = charac };
            return View(map);
        }
        


        [HttpPost]
        public async Task<ActionResult> CreateCharacteristic(CharactContainer characteristic)
        {
            var cal = Session["id"];
            var post = await dbPost.GetPost((String)cal);
                post.CharacteristicList.Add(characteristic.character);
                await dbPost.Update(post);
                return RedirectToAction("EditPost", new { userId = post.UserId, postId = post.Id });
            
            
        }
        public  ActionResult CreateKeyWord(string id)
        {
            Session["keyId"] = id;
            return View("CreateKeywords");
        }

        [HttpPost]
        public async Task<ActionResult> CreateKeyWord(KeyWordModels model)
        {
            var id = Session["keyId"];
            var post = await dbPost.GetPost((String)id);
            post.KeyWordList.Add(model.Name);
            await dbPost.Update(post);
            return RedirectToAction("EditPost", new { userId = post.UserId, postId = post.Id });
        }
        public async Task<ActionResult> ShowPost(string id)
        {
            var post = await dbPost.GetPost(id);
            var user = await db.GetUser(post.UserId);
            bool b = false;
            if (HttpContext.User.Identity.IsAuthenticated )
            {
                var current = await db.GetUserByLogin(HttpContext.User.Identity.Name);
                if (current.Id.Equals(user.Id))
                {
                    b = true;
                }
            }
            var model = new Container() { Post = post, User = user, isOwner=b };
            return View(model);
        }

        public async Task<ActionResult> UserAdditionalInfoModal(String id)
        {
            UserModels user = await db.GetUser(id);
            if (user != null)
                return PartialView(user.UserInfo);
            return HttpNotFound();
        }
        public async Task<ActionResult> Delete(string id)
        {
            await db.Remove(id);
            return RedirectToAction("UserIndex");
        }

        public async Task<ActionResult> NewPost()
        {
            if(!HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            var user = await db.GetUserByLogin(HttpContext.User.Identity.Name);
            CurrentUserId = user.Id;
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            var category = await dbCategory.GetAllCategories();
            ViewBag.Categorys = category;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewPost(PostModels post)
        {
            if(ModelState.IsValid)
            {
                post.Date = DateTime.Now;
                var user = await db.GetUserByLogin(HttpContext.User.Identity.Name);
                post.UserId = user.Id;
                await dbPost.Create(post);
                return RedirectToAction("Index");
            }
            return View(post);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(UserModels user)
        {
            if(ModelState.IsValid)
            {
                await db.Create(user);
                return RedirectToAction("UserIndex");
            }
            return View(user);
        }
        public async Task<ActionResult> AttachImage(string id)
        {
            PostModels c = await dbPost.GetPost(id);
            if (c == null)
                return HttpNotFound();
            return View(c);
        }
        [HttpPost]
        public async Task<ActionResult> AttachImage(string id, HttpPostedFileBase uploadedFile)
        {
            if (uploadedFile != null)
            {
                await dbPost.StoreImage(id, uploadedFile.InputStream, uploadedFile.FileName);
            }
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> GetImage(string id)
        {
            var image = await dbPost.GetImage(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return File(image, "image/png");
        }
    }
}