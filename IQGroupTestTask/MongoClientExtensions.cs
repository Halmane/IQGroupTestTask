using System.Diagnostics.CodeAnalysis;
using MongoDB.Driver;

namespace IQGroupTestTask;

public static class MongoClientExtensions
{
    public static async Task<bool> TryDropDatabaseAsync(
        this MongoClient client,
        string databaseName
    )
    {
        if (!await client.HasDataBaseAsync(databaseName))
        {
            return false;
        }

        await client.DropDatabaseAsync(databaseName);

        return true;
    }

    public static async Task<bool> TryRenameCollectionAsync(
        this MongoClient mongoClient,
        string databaseName,
        string currentCollectionName,
        string newCollectionName
    )
    {
        if (!await mongoClient.HasDataBaseAsync(databaseName))
        {
            return false;
        }

        var db = mongoClient.GetDatabase(databaseName);

        if (!await db.HasCollectionAsync(currentCollectionName))
        {
            return false;
        }

        await db.RenameCollectionAsync(currentCollectionName, newCollectionName);

        return true;
    }

    public static async Task RenameCollectionAsync(
        this MongoClient mongoClient,
        string databaseName,
        string currentCollectionName,
        string newCollectionName
    )
    {
        if (!await mongoClient.HasDataBaseAsync(databaseName))
        {
            throw new Exception("Database does not exist");
        }

        var db = mongoClient.GetDatabase(databaseName);

        if (!await db.HasCollectionAsync(currentCollectionName))
        {
            throw new Exception("Collections does not exist");
        }

        await db.RenameCollectionAsync(currentCollectionName, newCollectionName);
    }

    public static async Task CreateDatabaseAsync(this MongoClient mongoClient, string databaseName)
    {
        if (await mongoClient.HasDataBaseAsync(databaseName))
        {
            throw new Exception("Database already exists");
        }

        mongoClient.GetDatabase(databaseName);
    }

    public static async Task<bool> TryCreateDatabaseAsync(
        this MongoClient mongoClient,
        string databaseName
    )
    {
        if (!await mongoClient.HasDataBaseAsync(databaseName))
        {
            mongoClient.GetDatabase(databaseName);
            return true;
        }
        return false;
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

    public static async Task<bool> HasDataBaseAsync(
        this MongoClient mongoClient,
        string databaseName
    )
    {
        return (await mongoClient.ListDatabaseNamesAsync()).ToList().Contains(databaseName);
    }

    public static bool HasDataBase(this MongoClient mongoClient, string databaseName)
    {
        return mongoClient.ListDatabaseNames().ToList().Contains(databaseName);
    }

    public static async Task<IMongoDatabase?> TryGetDatabaseAsync(
        this MongoClient mongoClient,
        string databaseName
    )
    {
        if (await mongoClient.HasDataBaseAsync(databaseName))
        {
            return mongoClient.GetDatabase(databaseName);
        }
        return null;
    }

    public static bool TryGetDatabase(
        this MongoClient mongoClient,
        string databaseName,
        [NotNullWhen(true)] out IMongoDatabase? database
    )
    {
        database = null;
        if (mongoClient.HasDataBase(databaseName))
        {
            database = mongoClient.GetDatabase(databaseName);
            return true;
        }
        return false;
    }
}
