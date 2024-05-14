using MongoDB.Driver;

namespace IQGroupTestTask.Tests;

public class IMongoDatabaseExtensionsTest
{
    private readonly MongoClient _mongoClient;
    private readonly Logger _logger;
    private readonly IMongoDatabase? _database;

    public IMongoDatabaseExtensionsTest(MongoClient mongoClient, Logger logger)
    {
        _mongoClient = mongoClient;
        _logger = logger;
        var databaseName = "IMongoDatabaseExtensionsTest";
        try
        {
            _mongoClient.TryCreateDatabase(databaseName);

            if (!_mongoClient.TryGetDatabase(out _database, databaseName))
            {
                throw new Exception("Database does not exist");
            }

            TryGetFalseFromHasCollection();
            TryGetTrueFromHasCollection();
            _logger.LogInfo("IMongoDatabaseExtensionsTest finished");
        }
        catch (Exception ex)
        {
            _logger.LogError($"IMongoDatabaseExtensionsTest failed: {ex}");
        }
        finally
        {
            _mongoClient.TryDropDatabase(databaseName);
        }
    }

    public void TryGetFalseFromHasCollection()
    {
        var collectionName = "TryGetFalseFromHasCollection";

        try
        {
            if (_database is null)
            {
                throw new Exception("Database does not exist");
            }

            if (_database.HasCollection(collectionName))
            {
                throw new Exception("Сollection has been found");
            }

            _logger.LogInfo("TryGetFalseFromHasCollection completed");
        }
        catch (Exception ex)
        {
            _logger.LogError($"TryGetFalseFromHasCollection failed : {ex}");
        }
    }

    public void TryGetTrueFromHasCollection() 
    {
        var collectionName = "TryGetTrueFromHasCollection";

        try
        {
            if (_database is null)
            {
                throw new Exception("Database does not exist");
            }

            _database.CreateCollection(collectionName);

            if (!_database.HasCollection(collectionName))
            {
                throw new Exception("Сollection does not exist");
            }

            _database.TryDropCollections(collectionName);
            _logger.LogInfo("TryGetTrueFromHasCollection completed");
        }
        catch(Exception ex)
        {
            _logger.LogError($"TryGetTrueFromHasCollection failed : {ex}");
        }
    }
    
}
