using System.Threading.Tasks;

namespace SV.Infrastructure
{
    public interface IDbRepository<TEntity> where TEntity : BaseEntity
    {
        void BeginTransaction();
        void FinishTransaction();
        void ExitTransaction();

        Task<long> AddAsync(TEntity item);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(TEntity entity);

        Task<T> ExecuteSingleResultSPAsync<T>(string query, object param);
        Task<int> ExecuteSPAsync(string query, object param);
        Task<int> ExecuteAsync(string query, object param);
    }
}
