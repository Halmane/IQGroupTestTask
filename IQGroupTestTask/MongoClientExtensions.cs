using System.Diagnostics.CodeAnalysis;
using MongoDB.Driver;

namespace IQGroupTestTask;

public static class MongoClientExtensions
{
    public static bool TryDropDatabase(this MongoClient client, string databaseName)
    {
        if (!client.HasDataBase(databaseName)) 
        {
            return false;
        }

        client.DropDatabase(databaseName);

        return true;
    }

    public static bool TryRenameCollectionAsync(
        this MongoClient mongoClient,
        string databaseName,
        string currentCollectionName,
        string newCollectionName
    )
    {
        if (!mongoClient.HasDataBase(databaseName))
        {
            return false;
        }

        var db = mongoClient.GetDatabase(databaseName);

        if (!db.HasCollection(currentCollectionName))
        {
            return false;
        }

        db.RenameCollection(currentCollectionName, newCollectionName);

        return true;
    }

    public static void RenameCollection(
        this MongoClient mongoClient,
        string databaseName,
        string currentCollectionName,
        string newCollectionName
    )
    {
        if (!mongoClient.HasDataBase(databaseName))
        {
            throw new Exception("Database does not exist");
        }

        var db = mongoClient.GetDatabase(databaseName);

        if (!db.HasCollection(currentCollectionName))
        {
            throw new Exception("Collections does not exist");
        }

        db.RenameCollection(currentCollectionName, newCollectionName);
    }

    public static void CreateDatabase(this MongoClient mongoClient, string databaseName)
    {
        if (mongoClient.HasDataBase(databaseName))
        {
            throw new Exception("Database already exists");
        }

        mongoClient.GetDatabase(databaseName);
    }

    public static bool TryCreateDatabase(this MongoClient mongoClient, string databaseName)
    {
        if (!mongoClient.HasDataBase(databaseName))
        {
            mongoClient.GetDatabase(databaseName);
            return true;
        }
        return false;
    }

    public static bool HasDataBase(this MongoClient mongoClient, string databaseName)
    {
        var a = mongoClient.ListDatabaseNames();
        return a.ToList().Contains(databaseName);
    }

    public static bool TryGetDatabase(
        this MongoClient mongoClient,
        [NotNullWhen(true)] out IMongoDatabase? mongoDatabase,
        string databaseName
    )
    {
        mongoDatabase = null;
        if (mongoClient.HasDataBase(databaseName))
        {
            mongoDatabase = mongoClient.GetDatabase(databaseName);
            return true;
        }
        return false;
    }
}
