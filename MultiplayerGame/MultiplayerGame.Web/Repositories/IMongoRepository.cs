namespace MultiplayerGame.Web.Repositories;

using System.Linq.Expressions;

public interface IMongoRepository<TRootObject> where TRootObject : class {

    Task CreateAsync(TRootObject obj);
    Task CreateRangeAsync(List<TRootObject> objs);
   
    Task<TRootObject?> ReadAsync(Guid id);
    Task<List<TRootObject>> ReadAsync(int start, int count);
    Task<List<TRootObject>> ReadAsync(Expression<Func<TRootObject, bool>> filter);
    Task<List<TRootObject>> ReadAllAsync();

    Task UpdateAsync(Guid id, TRootObject obj);

    Task DeleteAsync(Guid id, TRootObject obj);
   
    Task DeleteRangeAsync(Expression<Func<TRootObject, bool>> filter);
}