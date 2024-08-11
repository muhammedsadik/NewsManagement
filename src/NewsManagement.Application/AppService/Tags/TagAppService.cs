using Microsoft.AspNetCore.Authorization;
using NewsManagement.Entities.Tags;
using NewsManagement.EntityDtos.PagedAndSortedDtos;
using NewsManagement.EntityDtos.TagDtos;
using NewsManagement.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NewsManagement.AppService.Tags
{
  [Authorize(NewsManagementPermissions.Tags.Default)]
  public class TagAppService : CrudAppService<Tag, TagDto, int, GetListPagedAndSortedDto, CreateTagDto, UpdateTagDto>, ITagAppService
  {
    private readonly TagManager _tagManager;

    public TagAppService(IRepository<Tag, int> repository, TagManager tagManager) : base(repository)
    {
      _tagManager = tagManager;

    }

    [Authorize(NewsManagementPermissions.Tags.Create)]
    public override async Task<TagDto> CreateAsync(CreateTagDto createTagDto)
    {
      return await _tagManager.CreateAsync(createTagDto);
    }

    [Authorize(NewsManagementPermissions.Tags.Edit)]
    public async override Task<TagDto> UpdateAsync(int id, UpdateTagDto input)
    {
      return await _tagManager.UpdateAsync(id, input);
    }

    public async override Task<PagedResultDto<TagDto>> GetListAsync(GetListPagedAndSortedDto input)
    {
      return await _tagManager.GetListAsync(input);
    }

    [Authorize(NewsManagementPermissions.Tags.Delete)]
    public override async Task DeleteAsync(int id)
    {
      await _tagManager.DeleteAsync(id);

      await base.DeleteAsync(id);
    }

    [Authorize(NewsManagementPermissions.Tags.Delete)]
    public async Task DeleteHardAsync(int id)
    {
      await _tagManager.DeleteHardAsync(id);
    }
  }
}
