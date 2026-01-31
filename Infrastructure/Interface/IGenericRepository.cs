using Domaine.Classes;

namespace Infrastructure.Interface
{
    public interface IGenericRepository<T>
    {
        public Task<T> GetById(Guid id);
        public Task<T> Add(T entity);
        public Task<T> Update(T entity);
        public Task<bool> Delete(Guid id);
    }
}
