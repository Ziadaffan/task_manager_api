using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IGenericService<T>
    {
        public Task<T> GetById(Guid id);
        public Task<T> Add(T entity);
        public Task<T> Update(T entity);
        public Task<bool> Delete(Guid id);
    }
}
