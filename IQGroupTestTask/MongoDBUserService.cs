using MongoDB.Bson;
using MongoDB.Driver;

namespace IQGroupTestTask;

public class MongoDBUserService
{
    private readonly Logger _logger;
    private readonly MongoClient _client;
    private readonly IMongoDatabase? _database;
    private readonly IMongoCollection<BsonDocument>? _collection;

    public MongoDBUserService(Logger logger, MongoClient client)
    {
        _logger = logger;
        _client = client;

        try
        {
            _client.TryCreateDatabase("Users");

            if (!_client.TryGetDatabase(out _database, "Users"))
            {
                throw new Exception("Database does not exist");
            }

            _database.TryCreateCollections("user");

            _collection = _database.GetCollection<BsonDocument>("user");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }

    public void AddUser(string name, string surname, out string message)
    {
        message = "";
        try
        {
            if (_client is null)
            {
                message = "MongoClient does not exist";
                throw new Exception(message);
            }

            if (_database is null)
            {
                message = "Database does not exist";
                throw new Exception(message);
            }
            var bsonDocument = new BsonDocument { { "name", name }, { "surname", surname } };
            var a = _collection.Find(bsonDocument).CountDocuments();

            if (a != 0)
            {
                message = "User has been found";
                throw new Exception(message);
            }

            _database.GetCollection<BsonDocument>("user").InsertOne(bsonDocument);

            message = "User has been added";
            _logger.LogInfo(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }
}
