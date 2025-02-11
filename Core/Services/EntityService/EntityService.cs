using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.EntitiesQueryUtilities.QueryParameters;
using Core.Repositories;

namespace Core.Services.EntityService
{
    public abstract class EntityService<TEntity, TModel, TCreate, TUpdate, TSortBy, TFilterBy> : IEntityService<TEntity, TModel, TCreate, TUpdate>
        where TEntity : BaseModel
        where TModel : class
        where TCreate : class
        where TUpdate : BaseModel
        where TSortBy : struct, Enum
        where TFilterBy : struct, Enum

    {
        protected readonly IRepository<TEntity> _repository;
        protected readonly IMapper _mapper;
        protected readonly IQueryParametersFactory<TEntity, TSortBy, TFilterBy> _queryParametersFactory;

        protected EntityService(IRepository<TEntity> repository, IMapper mapper, IQueryParametersFactory<TEntity, TSortBy, TFilterBy> queryParametersFactory)
        {
            _repository = repository;
            _mapper = mapper;
            _queryParametersFactory = queryParametersFactory;
        }

        public async Task<TModel> Create(TCreate createModel)
        {
            var newModel = _mapper.Map<TEntity>(createModel);

            var model = await _repository.Create(newModel);

            return _mapper.Map<TModel>(model);
        }

        public async Task Delete(int id)
            => await _repository.DeleteById(id);

        public async Task<bool> DoesExist(int id)
            => await _repository.DoesExist(id);

        public async Task<PagedList<TModel>> Fetch(QueryParameters<TEntity> queryParameters)
        {
            var totalElementCount = await _repository.Count(queryParameters);

            queryParameters.PagingInfo.UpdatePaging(totalElementCount);
            var entitiesList = await _repository.ExecuteQuery(queryParameters);

            return new PagedList<TModel>
            {
                PageInfo = queryParameters.PagingInfo,
                Items = _mapper.Map<List<TModel>>(entitiesList)
            };
        }

        public async Task<List<TModel>> GetAll()
        {
            var allEntitiesQuery = _queryParametersFactory.CreateGetAllQuery();

            var modelList = await _repository.ExecuteQuery(allEntitiesQuery);

            return _mapper.Map<List<TModel>>(modelList);
        }

        public async Task<TModel?> GetById(int id)
        {
            var entity = await _repository.GetById(id);

            if (entity is not null)
            {
                return _mapper.Map<TModel>(entity);
            }

            return null;
        }

        public async Task<TModel> Update(TUpdate updateModel)
        {
            var existingModel = await _repository.GetById(updateModel.Id);

            _mapper.Map(updateModel, existingModel);

            var updatedEntity = await _repository.Update(_mapper.Map<TEntity>(existingModel));

            return _mapper.Map<TModel>(updatedEntity);
        }
    }
}
