using MongoDB.Driver;

namespace IQGroupTestTask.Tests;

public class MongoClientExtensionsTest
{
    private readonly MongoClient _mongoClient;
    private readonly Logger _logger;

    public MongoClientExtensionsTest(MongoClient mongoClient, Logger logger)
    {
        _mongoClient = mongoClient;
        _logger = logger;

    }
}
