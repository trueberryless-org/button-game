namespace MultiplayerGame.Web.Repositories;

using System.Linq.Expressions;
using MongoDB.Driver;

public class MongoRepository<TRootObject> : IMongoRepository<TRootObject> where TRootObject : class {

    private readonly IMongoClient _mongoClient;
    public IMongoDatabase? Database { get; set; }
    public IMongoCollection<TRootObject> Collection { get; set; }
    
    public MongoRepository(IMongoClient mongoDbClient) {
        _mongoClient = mongoDbClient;
        Database = _mongoClient.GetDatabase("button-game");
        Collection = Database.GetCollection<TRootObject>(typeof(TRootObject).Name.ToLower());
    }
    
    public async Task CreateAsync(TRootObject obj) {
        await Collection.InsertOneAsync(obj);
    }

    public async Task CreateRangeAsync(List<TRootObject> objs) {
        await Collection.InsertManyAsync(objs);
    }

    public async Task<TRootObject?> ReadAsync(Guid id) {
        var filter = Builders<TRootObject>.Filter.Eq("_id", id);
        return await Collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<List<TRootObject>> ReadAsync(int start, int count) 
        => await Collection.Find(s => true).Skip(start).Limit(count).ToListAsync();

    public async Task<List<TRootObject>> ReadAsync(Expression<Func<TRootObject, bool>> filter) 
       =>  await Collection.Find(filter).ToListAsync();
   

    public async Task<List<TRootObject>> ReadAllAsync() {
        var queryableCollection = Collection.AsQueryable();
        return queryableCollection.ToList();
    }

    public async Task UpdateAsync(Guid id, TRootObject obj) {
        var filter = Builders<TRootObject>.Filter.Eq("_id", id);
        var old = await Collection.Find(filter).FirstAsync();

        if (old is null) return;
        
        await Collection.ReplaceOneAsync(filter, obj);
    }
    
    public async Task DeleteAsync(Guid id, TRootObject obj) {
        var filter = Builders<TRootObject>.Filter.Eq("_id", id);
        var old = await Collection.Find(filter).FirstAsync();
        if (old is null) return;
        
        await Collection.DeleteOneAsync(filter);
    }

    public async Task DeleteRangeAsync(Expression<Func<TRootObject, bool>> filter) {
        await Collection.DeleteManyAsync(filter);
    }
}