using AutoMapper;
using Core.Repository;
using Infrastructure.Models;

namespace Core.BaseService
{
    public abstract class SimpleService<TEntity, TModel, TCreate, TUpdate> : ISimpleService<TEntity, TModel, TCreate, TUpdate>
        where TEntity : BaseEntity
        where TModel : class
        where TCreate : class
        where TUpdate : class
    {
        protected readonly IRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public SimpleService(IRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TModel> Create(TCreate createModel)
        {
            var newModel = _mapper.Map<TEntity>(createModel);

            var model = await _repository.Create(newModel);

            return _mapper.Map<TModel>(model);
        }

        public async Task Delete(int id)
            => await _repository.DeleteById(id);

        public async Task<List<TModel>> GetAll()
        {
            var modelList = await _repository.GetAll();

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
            var updatedEntity = await _repository.Update(_mapper.Map<TEntity>(updateModel));

            return _mapper.Map<TModel>(updatedEntity);
        }
    }
}
