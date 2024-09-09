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
using NewsManagement.Entities.Categories;
using NewsManagement.Entities.Cities;
using NewsManagement.Entities.Tags;
using NewsManagement.Entities.ListableContentRelations;
using NewsManagement.Entities.ListableContents;
using NewsManagement.EntityConsts.ListableContentConsts;
using NewsManagement.Entities.Galleries;
using NewsManagement.Entities.Newses;
using NewsManagement.EntityDtos.GalleryDtos;
using NewsManagement.EntityConsts.VideoConsts;
using EasyAbp.FileManagement.Files;
using NewsManagement.EntityDtos.NewsDtos;

namespace NewsManagement.Entities.Videos
{
  public class VideoManager : ListableContentBaseManager<Video, VideoDto, GetListPagedAndSortedDto, CreateVideoDto, UpdateVideoDto>
  {
    private readonly IObjectMapper _objectMapper;
    private readonly ITagRepository _tagRepository;
    private readonly IFileRepository _fileRepository;
    private readonly ICityRepository _cityRepository;
    private readonly INewsRepository _newsRepository;
    private readonly IVideoRepository _videoRepository;
    private readonly IGalleryRepository _galleryRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IListableContentGenericRepository<Video> _genericRepository;
    private readonly IListableContentTagRepository _listableContentTagRepository;
    private readonly IListableContentCityRepository _listableContentCityRepository;
    private readonly IListableContentCategoryRepository _listableContentCategoryRepository;
    private readonly IListableContentRelationRepository _listableContentRelationRepository;

    public VideoManager(
      IObjectMapper objectMapper,
      ITagRepository tagRepository,
      ICityRepository cityRepository,
      INewsRepository newsRepository,
      IFileRepository fileRepository,
      IVideoRepository videoRepository,
      IGalleryRepository galleryRepository,
      ICategoryRepository categoryRepository,
      IListableContentGenericRepository<Video> genericRepository,
      IListableContentTagRepository listableContentTagRepository,
      IListableContentCityRepository listableContentCityRepository,
      IListableContentCategoryRepository listableContentCategoryRepository,
      IListableContentRelationRepository listableContentRelationRepository
      ) : base(objectMapper, tagRepository, cityRepository, newsRepository,
        videoRepository, galleryRepository, categoryRepository, genericRepository, listableContentTagRepository,
        listableContentCityRepository, listableContentCategoryRepository, listableContentRelationRepository, fileRepository
          )
    {
      _objectMapper = objectMapper;
      _tagRepository = tagRepository;
      _cityRepository = cityRepository;
      _newsRepository = newsRepository;
      _fileRepository = fileRepository;
      _videoRepository = videoRepository;
      _galleryRepository = galleryRepository;
      _genericRepository = genericRepository;
      _categoryRepository = categoryRepository;
      _listableContentTagRepository = listableContentTagRepository;
      _listableContentCityRepository = listableContentCityRepository;
      _listableContentCategoryRepository = listableContentCategoryRepository;
      _listableContentRelationRepository = listableContentRelationRepository;
    }


    public async Task<VideoDto> CreateAsync(CreateVideoDto createVideoDto)
    {
      var creatingVideo = await CheckCreateInputBaseAsync(createVideoDto);

      if (creatingVideo.VideoType == VideoType.Video)
      {
        if (creatingVideo.VideoId == null)
          throw new BusinessException(NewsManagementDomainErrorCodes.VideoIdMustBeExistForVideoType);

        if (creatingVideo.Url != null)
          throw new BusinessException(NewsManagementDomainErrorCodes.UrlMustBeNullForVideoType);

        var images = _fileRepository.GetAsync((Guid)creatingVideo.VideoId).Result;
      }

      if (creatingVideo.VideoType == VideoType.Link)
      {
        if (creatingVideo.Url == null)
          throw new BusinessException(NewsManagementDomainErrorCodes.UrlMustBeExistForLinkType);

        if (creatingVideo.VideoId != null)
          throw new BusinessException(NewsManagementDomainErrorCodes.VideoIdMustBeNullForLinkType);
      }

      var video = await _genericRepository.InsertAsync(creatingVideo, autoSave: true);

      await CreateCrossEntity(createVideoDto, video.Id);

      var videoDto = _objectMapper.Map<Video, VideoDto>(video);

      await GetCrossEntityAsync(videoDto);

      return videoDto;
    }

    public async Task<VideoDto> UpdateAsync(int id, UpdateVideoDto updateVideoDto)
    {
      var updatingVideo = await CheckUpdateInputBaseAsync(id, updateVideoDto);

      if (updatingVideo.VideoType == VideoType.Video)
      {
        if (updatingVideo.VideoId == null)
          throw new BusinessException(NewsManagementDomainErrorCodes.VideoIdMustBeExistForVideoType);

        if (updatingVideo.Url != null)
          throw new BusinessException(NewsManagementDomainErrorCodes.UrlMustBeNullForVideoType);

        var images = _fileRepository.GetAsync((Guid)updatingVideo.VideoId).Result;
      }

      if (updatingVideo.VideoType == VideoType.Link)
      {
        if (updatingVideo.Url == null)
          throw new BusinessException(NewsManagementDomainErrorCodes.UrlMustBeExistForLinkType);

        if (updatingVideo.VideoId != null)
          throw new BusinessException(NewsManagementDomainErrorCodes.VideoIdMustBeNullForLinkType);
      }

      var video = await _genericRepository.UpdateAsync(updatingVideo, autoSave: true);

      await ReCreateCrossEntity(updateVideoDto, video.Id);

      var videoDto = _objectMapper.Map<Video, VideoDto>(video);

      await GetCrossEntityAsync(videoDto);

      return videoDto;
    }

    public async Task<PagedResultDto<VideoDto>> GetListAsync(GetListPagedAndSortedDto input)
    {
      return await GetListFilterBaseAsync(input);
    }

    public async Task<VideoDto> GetByIdAsync(int id)
    {
      var video = await GetByIdBaseAsync(id);

      return video;
    }

    public async Task DeleteAsync(int id)
    {
      await CheckDeleteInputBaseAsync(id);
    }

    public async Task DeleteHardAsync(int id)
    {
      await CheckDeleteHardInputBaseAsync(id);
    }

  }
}
