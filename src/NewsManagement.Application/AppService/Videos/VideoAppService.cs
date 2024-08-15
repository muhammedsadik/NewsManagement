﻿using NewsManagement.Entities.Videos;
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
      return await base.CreateAsync(createVideoDto);
    }

    public async override Task<VideoDto> UpdateAsync(int id, UpdateVideoDto updateVideoDto)
    {
      return await base.UpdateAsync(id, updateVideoDto);
    }

    public async override Task<PagedResultDto<VideoDto>> GetListAsync(GetListPagedAndSortedDto input)
    {
      throw new NotImplementedException();
    }

    public override async Task DeleteAsync(int id)
    {

      await base.DeleteAsync(id);
    }

    public Task DeleteHardAsync(int id)
    {
      throw new NotImplementedException();
    }
  }
}
