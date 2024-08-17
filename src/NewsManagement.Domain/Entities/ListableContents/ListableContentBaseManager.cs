using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;

namespace NewsManagement.Entities.ListableContents
{
  public abstract class ListableContentBaseManager<TEntity, TEntityDto, TPagedDto, TEntityCreateDto, TEntityUpdateDto> : DomainService
    where TEntity : ListableContent
  {
    private readonly IObjectMapper _objectMapper;
    private readonly IRepository<TEntity, int> _repository;
    protected ListableContentBaseManager(IRepository<TEntity, int> repository, IObjectMapper objectMapper)
    {
      _repository = repository;
      _objectMapper = objectMapper;
    }

    public async Task<TEntityDto> CreateAsync(TEntityCreateDto createDto)
    {
      //var isExist = await _repository.AnyAsync(x => x.Name == createDto.TagName);
      //if (isExist)
      //  throw new AlreadyExistException(typeof(TEntityDto), createDto.TagName);

      var creatingEntity = _objectMapper.Map<TEntityCreateDto, TEntity>(createDto);

      var createdContent = await _repository.InsertAsync(creatingEntity);

      var createdDto = _objectMapper.Map<TEntity, TEntityDto>(creatingEntity);

      return createdDto;
    }

    public async Task<TEntityDto> UpdateAsync(int id, TEntityUpdateDto updateDto)
    {
      var existingContent = await _repository.GetAsync(id);

      //var isExisting = await _repository.AnyAsync(x => x.Name == updateDto.Name && t.Id != id);
      //if (isExisting)
      //  throw new AlreadyExistException(typeof(TEntity), updateDto.Name);

      _objectMapper.Map(updateDto, existingContent);

      var updatedContent = await _repository.UpdateAsync(existingContent);

      var updatedDto = _objectMapper.Map<TEntity, TEntityDto>(updatedContent);

      return updatedDto;
    }

    public async Task<PagedResultDto<TEntityDto>> GetListAsync(TPagedDto input)
    {
      return new PagedResultDto<TEntityDto>();
    }

    public async Task DeleteAsync(int id)
    {
      var isExist = await _repository.AnyAsync(t => t.Id == id);
      if (!isExist)
        throw new EntityNotFoundException(typeof(TEntity), id);
    }

    public async Task DeleteHardAsync(int id)
    {

    }

  }
}
