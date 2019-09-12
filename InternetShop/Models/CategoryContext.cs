using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace InternetShop.Models
{
    public class CategoryContext
    {
        IMongoDatabase database;
        IGridFSBucket gridFS;

        public CategoryContext()
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

        public IMongoCollection<CategoryModels> Categorys
        {
            get { return database.GetCollection<CategoryModels>("Category"); }
        }

        public async Task<CategoryModels> GetCategory(string id)
        {
            return await Categorys.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CategoryModels>> GetAllCategories()
        {
            var builder = new FilterDefinitionBuilder<CategoryModels>();
            var filter = builder.Empty;
            return await Categorys.Find(filter).ToListAsync();
        }

        public async Task Create(CategoryModels c)
        {
            await Categorys.InsertOneAsync(c);
        }

        public async Task Remove(string id)
        {
            await Categorys.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
        }
        public async Task Update(CategoryModels cat)
        {
            await Categorys.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(cat.Id)), cat);
        }

    }
}