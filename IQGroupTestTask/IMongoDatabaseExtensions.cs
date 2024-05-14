using MongoDB.Driver;

namespace IQGroupTestTask;

public static class IMongoDatabaseExtensions
{
    public static bool HasCollection(this IMongoDatabase database, string collectionsName)
    {
        return database.ListCollectionNames().ToList().Contains(collectionsName);
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

    public static bool TryDropCollections(this IMongoDatabase database, string collectionsName)
    {
        if (!database.HasCollection(collectionsName))
        {
            return false;
        }
        database.DropCollection(collectionsName);
        return true;
    }
}
