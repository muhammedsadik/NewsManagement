﻿using NewsManagement.Entities.Newses;
using NewsManagement.Entities.Videos;
using NewsManagement.EntityDtos.GalleryDtos;
using NewsManagement.EntityDtos.PagedAndSortedDtos;
using NewsManagement.EntityDtos.VideoDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Features;

namespace NewsManagement.AppService.Videos
{
  public class VideoAppService : CrudAppService<Video, VideoDto, int, GetListPagedAndSortedDto, CreateVideoDto, UpdateVideoDto>, IVideoAppService
  {
    private readonly VideoManager _videoManager;

    public VideoAppService(IRepository<Video, int> repository, VideoManager videoManager) : base(repository)
    {
      _videoManager = videoManager;
    }

    public override async Task<VideoDto> CreateAsync(CreateVideoDto createVideoDto)
    {
      return await _videoManager.CreateAsync(createVideoDto);
    }

    public async override Task<VideoDto> UpdateAsync(int id, UpdateVideoDto updateVideoDto)
    {
      return await _videoManager.UpdateAsync(id, updateVideoDto);
    }

    protected override async Task<Video> GetEntityByIdAsync(int id)
    {
      await _videoManager.GetEntityByIdAsync(id);

      return await base.GetEntityByIdAsync(id);
    }

    //[RequiresFeature("NewsApp.Video")]
    public async override Task<PagedResultDto<VideoDto>> GetListAsync(GetListPagedAndSortedDto input)
    {
     return await _videoManager.GetListAsync(input);
    }

    public override async Task DeleteAsync(int id)
    {
      await _videoManager.DeleteAsync(id);

      await base.DeleteAsync(id);
    }

    public async Task DeleteHardAsync(int id)
    {
      await _videoManager.DeleteHardAsync(id);
    }
  }
}
