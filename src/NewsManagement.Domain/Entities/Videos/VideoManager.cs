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

namespace NewsManagement.Entities.Videos
{
  public class VideoManager : ListableContentBaseManager<Video, VideoDto, GetListPagedAndSortedDto, CreateVideoDto, UpdateVideoDto>
  {
    private readonly IObjectMapper _objectMapper;
    private readonly ITagRepository _tagRepository;
    private readonly ICityRepository _cityRepository;
    private readonly IVideoRepository _videoRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IListableContentGenericRepository<Video> _genericRepository;
    private readonly IRepository<ListableContentTag> _listableContentTagRepository;
    private readonly IRepository<ListableContentCity> _listableContentCityRepository;
    private readonly IRepository<ListableContentCategory> _listableContentCategoryRepository;
    private readonly IRepository<ListableContentRelation> _listableContentRelationRepository;

    public VideoManager(
      IObjectMapper objectMapper,
      ITagRepository tagRepository,
      ICityRepository cityRepository,
      IVideoRepository videoRepository,
      ICategoryRepository categoryRepository,
      IListableContentGenericRepository<Video> genericRepository,
      IRepository<ListableContentTag> listableContentTagRepository,
      IRepository<ListableContentCity> listableContentCityRepository,
      IRepository<ListableContentCategory> listableContentCategoryRepository,
      IRepository<ListableContentRelation> listableContentRelationRepository
      ) : base(objectMapper, tagRepository, cityRepository, categoryRepository,
          genericRepository, listableContentTagRepository, listableContentCityRepository,
          listableContentCategoryRepository, listableContentRelationRepository
          )
    {
      _objectMapper = objectMapper;
      _tagRepository = tagRepository;
      _cityRepository = cityRepository;
      _videoRepository = videoRepository;
      _genericRepository = genericRepository;
      _categoryRepository = categoryRepository;
      _listableContentTagRepository = listableContentTagRepository;
      _listableContentCityRepository = listableContentCityRepository;
      _listableContentCategoryRepository = listableContentCategoryRepository;
      _listableContentRelationRepository = listableContentRelationRepository;
    }


    public async Task<VideoDto> CreateAsync(CreateVideoDto createVideoDto)
    {
      await CheckCreateInputBaseAsync(createVideoDto);

      var creatingVideo = _objectMapper.Map<CreateVideoDto, Video>(createVideoDto);

      creatingVideo.PublishTime = DateTime.Now;
      creatingVideo.Status = StatusType.PendingReview;
      creatingVideo.listableContentType = ListableContentType.Video;

      //if(createVideoDto.VideoType == VideoType.Link)
      // ❓ VideoType (Physical, Link) kontrolünü yap ve 📩, ayrıca type göre iş kuralları varsa uygula

      var video = await _videoRepository.InsertAsync(creatingVideo);

      await CreateListableContentTagBaseAsync(createVideoDto.TagIds, video.Id);

      await CreateListableContentCityBaseAsync(createVideoDto.CityIds, video.Id);

      await CreateListableContentCategoryBaseAsync(createVideoDto.ListableContentCategoryDtos, video.Id);

      if (createVideoDto.RelatedListableContentIds != null)
        await CreateListableContentRelationBaseAsync(createVideoDto.RelatedListableContentIds, video.Id);

      var videoDto = _objectMapper.Map<Video, VideoDto>(video);

      return videoDto;
    }

    public async Task<VideoDto> UpdateAsync(int id, UpdateVideoDto updateVideoDto)
    {
      await CheckUpdateInputBaseAsync(id, updateVideoDto);

      var updatingVideo = _objectMapper.Map<UpdateVideoDto, Video>(updateVideoDto);

      //if(updateVideoDto.listableContentType != ListableContentType.Video)
      //burada listableContentType kontrolü yap listableContentType değişebilir ona göre yönlendirme yap
      //(burada UpdateVideoDto dan geldiği için status değişemez olması gerekiyor ama ListableContent ten gelirse(!) bunu ele almak gerekir.)

      //if(updateVideoDto.VideoType == VideoType.Physical) burada type değişmiş olabilir. ❗❗❗
      // ❓  VideoType (Physical, Link) kontrolünü yap ve => 📩

      var video = await _videoRepository.InsertAsync(updatingVideo);

      await ReCreateListableContentTagBaseAsync(updateVideoDto.TagIds, video.Id);

      await ReCreateListableContentCityBaseAsync(updateVideoDto.CityIds, video.Id);

      await ReCreateListableContentCategoryBaseAsync(updateVideoDto.ListableContentCategoryDtos, video.Id);

      if (updateVideoDto.RelatedListableContentIds != null)
        await ReCreateListableContentRelationBaseAsync(updateVideoDto.RelatedListableContentIds, video.Id);

      var videoDto = _objectMapper.Map<Video, VideoDto>(video);

      return videoDto;
    }

    public async Task<PagedResultDto<VideoDto>> GetListAsync(GetListPagedAndSortedDto input)
    {
      return await GetListFilterBaseAsync(input);
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
