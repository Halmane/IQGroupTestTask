using MongoDB.Bson;
using MongoDB.Driver;

namespace IQGroupTestTask;

public class MongoDatabaseUserService
{
    private readonly ILogger<MongoDatabaseUserService> _logger;
    private readonly MongoClient _client;
    private readonly IMongoDatabase? _database;
    private readonly IMongoCollection<BsonDocument>? _collection;
    private readonly CancellationToken _token;

    public MongoDatabaseUserService(
        ILogger<MongoDatabaseUserService> logger,
        MongoClient client,
        CancellationTokenSource cancellationTokenSource
    )
    {
        _logger = logger;
        _client = client;
        _token = cancellationTokenSource.Token;

        try
        {
            _client.TryCreateDatabase("Users");

            if (!_client.TryGetDatabase("Users", out _database))
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

    public async Task<string> AddUserAsync(string name, string surname)
    {
        var message = "";
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

            if (
                (await _collection.FindAsync(bsonDocument, cancellationToken: _token))
                    .ToList()
                    .Count != 0
            )
            {
                message = "User has been found";
                throw new Exception(message);
            }

            await _database.GetCollection<BsonDocument>("user").InsertOneAsync(bsonDocument);

            message = "User has been added";
            _logger.LogInformation(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }

        return message;
    }
}
