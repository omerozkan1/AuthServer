using AuthServer.Core.Repository;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using AuthServer.Service.Mapping;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class GenericService<T, TDTO> : IGenericService<T, TDTO> where T : class where TDTO : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<T> _genericRepository;
        public GenericService(IUnitOfWork unitOfWork, IGenericRepository<T> genericRepository)
        {
            _unitOfWork = unitOfWork;
            _genericRepository = genericRepository;
        }
        public async Task<Response<TDTO>> AddAsync(TDTO entity)
        {
            var newEntity = ObjectMapper.Mapper.Map<T>(entity);
            await _genericRepository.AddAsync(newEntity);
            await _unitOfWork.CommitAsync();
            var newDto = ObjectMapper.Mapper.Map<TDTO>(newEntity);
            return Response<TDTO>.Success(newDto, (int)HttpStatusCode.OK);
        }

        public async Task<Response<IEnumerable<TDTO>>> GetAllAsync()
        {
            var products = ObjectMapper.Mapper.Map<List<TDTO>>(await _genericRepository.GetAllAsync());
            return Response<IEnumerable<TDTO>>.Success(products, (int)HttpStatusCode.OK);
        }

        public async Task<Response<TDTO>> GetByIdAsync(int id)
        {
            var product = await _genericRepository.GetByIdAsync(id);
            if (product == null)
                return Response<TDTO>.Fail("Id bulunamadı.", (int)HttpStatusCode.NotFound, true);

            return Response<TDTO>.Success(ObjectMapper.Mapper.Map<TDTO>(product), (int)HttpStatusCode.OK);
        }

        public async Task<Response<NoDataDTO>> Remove(int id)
        {
            var isExist = await _genericRepository.GetByIdAsync(id);
            if (isExist == null)
                return Response<NoDataDTO>.Fail("Id bulunamadı.", (int)HttpStatusCode.NotFound,true);
            _genericRepository.Remove(isExist);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDTO>.Success((int)HttpStatusCode.NoContent);
        }

        public async Task<Response<NoDataDTO>> Update(TDTO entity, int id)
        {
            var isExist = await _genericRepository.GetByIdAsync(id);
            if (isExist == null)
                return Response<NoDataDTO>.Fail("Id bulunamadı.", (int)HttpStatusCode.NotFound, true);

            var updatedEntity = ObjectMapper.Mapper.Map<T>(entity);
            _genericRepository.Update(updatedEntity);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDTO>.Success((int)HttpStatusCode.NoContent);
        }

        public async Task<Response<IEnumerable<TDTO>>> Where(Expression<Func<T, bool>> predicate)
        {
            var list = _genericRepository.Where(predicate);
            return Response<IEnumerable<TDTO>>.Success(ObjectMapper.Mapper.Map<IEnumerable<TDTO>>(await list.ToListAsync()), (int)HttpStatusCode.OK);
        }
    }
}
