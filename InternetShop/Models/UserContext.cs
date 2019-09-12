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
    public class UserContext
    {

        IMongoDatabase database;
        IGridFSBucket gridFS;

        public UserContext()
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

        // обращаемся к коллекции пользователей
        public IMongoCollection<UserModels> Users
        {
            get { return database.GetCollection<UserModels>("Users"); }
        }

        public async Task<UserModels> GetUser(string id)
        {
            return await Users.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
        }

        public async Task<UserModels> GetUserByLogin(string login)
        {
            return await Users.Find(new BsonDocument("Email", new BsonRegularExpression(login))).FirstOrDefaultAsync();
        }

        public async Task Create(UserModels c)
        {
            await Users.InsertOneAsync(c);
        }

        public async Task Remove(string id)
        {
            await Users.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
        }
        public async Task Update(UserModels user)
        {
            await Users.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(user.Id)), user);
        }


        public async Task<IEnumerable<UserModels>> GetUsers(string id)
        {

            var builder = new FilterDefinitionBuilder<UserModels>();
            var filter = builder.Empty; // фильтр для выборки всех документов
            // фильтр по имени
            if (!String.IsNullOrWhiteSpace(id))
            {
                return await Users.Find(new BsonDocument("_id", new ObjectId(id))).ToListAsync();
            }

            return await Users.Find(filter).ToListAsync();
        }
    }
}