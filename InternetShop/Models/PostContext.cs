using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace InternetShop.Models
{
    public class PostContext
    {

        IMongoDatabase database;
        IGridFSBucket gridFS;

        public PostContext()
        {
            // строка подключения
            string connectionString = ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;
            var connection = new MongoUrlBuilder(connectionString);
            // получаем клиента для взаимодействия с базой данных
            MongoClient client = new MongoClient(connectionString);
            // получаем доступ к самой базе данных
            database = client.GetDatabase(connection.DatabaseName);
            // получаем доступ к файловому хранилищу
            gridFS = new GridFSBucket(database);
        }

        public IMongoCollection<PostModels> Posts
        {
            get { return database.GetCollection<PostModels>("Posts"); }
        }
        public async Task<PostModels> GetPost(string id)
        {
            return await Posts.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
        }

        public async Task Create(PostModels c)
        {
            await Posts.InsertOneAsync(c);
        }

        public async Task Remove(string id)
        {
            await Posts.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
        }
        public async Task Update(PostModels post)
        {
            await Posts.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(post.Id)), post);
        }


        public async Task<IEnumerable<PostModels>> GetPosts(PostFilter pFilter)
        {

            var builder = new FilterDefinitionBuilder<PostModels>();
            var filter = builder.Empty; // фильтр для выборки всех документов
            if (!String.IsNullOrWhiteSpace(pFilter.CategoryId))
            {
                filter = filter & builder.Regex("CategoryId", new BsonRegularExpression(pFilter.CategoryId));
            }
            if (!String.IsNullOrEmpty(pFilter.Header))
            {
                filter = filter & builder.Regex("Header", new BsonRegularExpression(pFilter.Header));
            }
            if(pFilter.KeyWords != null && pFilter.KeyWords.Count > 0)
            {
                filter = filter & builder.All("KeyWordList", pFilter.KeyWords);
            }

            return await Posts.Find(filter).ToListAsync();
        }
        public async Task<byte[]> GetImage(string id)
        {
            return await gridFS.DownloadAsBytesAsync(new ObjectId(id));
        }
        // сохранение изображения
        public async Task StoreImage(string id, Stream imageStream, string imageName)
        {
            PostModels c = await GetPost(id);
            if (c.HasImage())
            {
                // если ранее уже была прикреплена картинка, удаляем ее
                await gridFS.DeleteAsync(new ObjectId(c.ImageId));
            }
            // сохраняем изображение
            ObjectId imageId = await gridFS.UploadFromStreamAsync(imageName, imageStream);
            // обновляем данные по документу
            c.ImageId = imageId.ToString();
            var filter = Builders<PostModels>.Filter.Eq("_id", new ObjectId(c.Id));
            var update = Builders<PostModels>.Update.Set("ImageId", c.ImageId);
            await Posts.UpdateOneAsync(filter, update);
        }
    }
}
