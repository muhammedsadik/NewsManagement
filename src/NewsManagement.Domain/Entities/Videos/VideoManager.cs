using NewsManagement.Entities.Exceptions;
using NewsManagement.EntityDtos.PagedAndSortedDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;
using NewsManagement.EntityDtos.VideoDtos;
using Volo.Abp.Domain.Repositories;

namespace NewsManagement.Entities.Videos
{
  public class VideoManager : DomainService
  {
    private readonly IVideoRepository _videoRepository;
    private readonly IObjectMapper _objectMapper;

    public VideoManager(IObjectMapper objectMapper, IVideoRepository videoRepository)
    {
      _objectMapper = objectMapper;
      _videoRepository = videoRepository;
    }


    //public async Task<VideoDto> CreateAsync(CreateVideoDto createVideoDto)
    //{
      
    //}

    //public async Task<VideoDto> UpdateAsync(int id, UpdateVideoDto updateVideoDto)
    //{

    //}

    //public async Task<PagedResultDto<VideoDto>> GetListAsync(GetListPagedAndSortedDto input)
    //{

    //}

    public async Task DeleteAsync(int id)
    {

    }

    public async Task DeleteHardAsync(int id)
    {

    }

  }
}
