namespace DataAccess.Dapper
{
    public interface IBaseRepository<T> where T : class, new()
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(int id);
        Task<int> Add(T entity);
        Task<int> Update(T entity);
        Task<int> Delete(int id);
       
    }
}
