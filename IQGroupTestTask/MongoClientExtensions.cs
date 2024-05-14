using System.Diagnostics.CodeAnalysis;
using MongoDB.Driver;

namespace IQGroupTestTask;

public static class MongoClientExtensions
{
    public static async Task<bool> TryDropDatabaseAsync(
        this MongoClient client,
        string databaseName,
        CancellationToken token = default
    )
    {
        if (!await client.HasDataBaseAsync(databaseName, token))
        {
            return false;
        }

        await client.DropDatabaseAsync(databaseName, token);

        return true;
    }

    public static async Task<bool> TryRenameCollectionAsync(
        this MongoClient mongoClient,
        string databaseName,
        string currentCollectionName,
        string newCollectionName,
        CancellationToken token = default
    )
    {
        if (!await mongoClient.HasDataBaseAsync(databaseName, token))
        {
            return false;
        }

        var db = mongoClient.GetDatabase(databaseName);

        if (!await db.HasCollectionAsync(currentCollectionName, token))
        {
            return false;
        }

        await db.RenameCollectionAsync(currentCollectionName, newCollectionName, token);

        return true;
    }

    public static async Task RenameCollectionAsync(
        this MongoClient mongoClient,
        string databaseName,
        string currentCollectionName,
        string newCollectionName,
        CancellationToken token = default
    )
    {
        if (!await mongoClient.HasDataBaseAsync(databaseName, token))
        {
            throw new Exception("Database does not exist");
        }

        var db = mongoClient.GetDatabase(databaseName);

        if (!await db.HasCollectionAsync(currentCollectionName, token))
        {
            throw new Exception("Collections does not exist");
        }

        await db.RenameCollectionAsync(currentCollectionName, newCollectionName, token);
    }

    public static async Task CreateDatabaseAsync(
        this MongoClient mongoClient,
        string databaseName,
        CancellationToken token = default
    )
    {
        if (await mongoClient.HasDataBaseAsync(databaseName, token))
        {
            throw new Exception("Database already exists");
        }

        mongoClient.GetDatabase(databaseName);
    }

    public static async Task<bool> TryCreateDatabaseAsync(
        this MongoClient mongoClient,
        string databaseName,
        CancellationToken token = default
    )
    {
        if (!await mongoClient.HasDataBaseAsync(databaseName, token))
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
        string databaseName,
        CancellationToken token = default
    )
    {
        return (await mongoClient.ListDatabaseNamesAsync(token)).ToList().Contains(databaseName);
    }

    public static bool HasDataBase(this MongoClient mongoClient, string databaseName)
    {
        return mongoClient.ListDatabaseNames().ToList().Contains(databaseName);
    }

    public static async Task<IMongoDatabase?> TryGetDatabaseAsync(
        this MongoClient mongoClient,
        string databaseName,
        CancellationToken token = default
    )
    {
        if (await mongoClient.HasDataBaseAsync(databaseName, token))
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
