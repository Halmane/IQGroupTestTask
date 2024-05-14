using MongoDB.Bson;
using MongoDB.Driver;

namespace IQGroupTestTask;

public static class IMongoDatabaseExtensions
{
    public static async Task<bool> HasCollectionAsync(
        this IMongoDatabase database,
        string collectionsName
    )
    {
        return (await database.ListCollectionNamesAsync()).ToList().Contains(collectionsName);
    }

    public static bool HasCollection(this IMongoDatabase database, string collectionsName)
    {
        return database.ListCollectionNames().ToList().Contains(collectionsName);
    }

    public static async Task<bool> TryCreateCollectionsAsync(
        this IMongoDatabase database,
        string collectionsName
    )
    {
        if (await database.HasCollectionAsync(collectionsName))
        {
            return false;
        }

        await database.CreateCollectionAsync(collectionsName);
        return true;
    }

    public static bool TryCreateCollections(this IMongoDatabase database, string collectionsName)
    {
        if (database.HasCollection(collectionsName))
        {
            return false;
        }

        database.CreateCollection(collectionsName);

        return true;
    }

    public static async Task<bool> TryDropCollectionsAsync(
        this IMongoDatabase database,
        string collectionsName
    )
    {
        if (!await database.HasCollectionAsync(collectionsName))
        {
            return false;
        }
        await database.DropCollectionAsync(collectionsName);
        return true;
    }

    public static async Task<bool> TryUpdateValueAsync(
        this IMongoDatabase database,
        string collectionsName,
        BsonDocument currentValue,
        BsonDocument newValue
    )
    {
        if (!await database.HasCollectionAsync(collectionsName))
        {
            return false;
        }

        var collections = database.GetCollection<BsonDocument>(collectionsName);

        if ((await collections.FindAsync(currentValue)).ToList().Count() == 0)
        {
            return false;
        }

        await collections.UpdateOneAsync(currentValue, newValue);

        return true;
    }
}
