using SharedLibrary.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IGenericService<T, TDTO> where T: class where TDTO:class 
    {
        Task<Response<TDTO>> GetByIdAsync(int id);
        Task<Response<IEnumerable<TDTO>>> GetAllAsync();
        Task<Response<IEnumerable<TDTO>>> Where(Expression<Func<T, bool>> predicate);
        Task<Response<TDTO>> AddAsync(TDTO entity);
        Task<Response<NoDataDTO>> Remove(int id);
        Task<Response<NoDataDTO>> Update(TDTO entity, int id);
    }
}
