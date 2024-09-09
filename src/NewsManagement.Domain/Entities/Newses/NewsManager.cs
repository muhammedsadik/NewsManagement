using EasyAbp.FileManagement.Files;
using NewsManagement.Entities.Categories;
using NewsManagement.Entities.Cities;
using NewsManagement.Entities.Galleries;
using NewsManagement.Entities.ListableContentRelations;
using NewsManagement.Entities.ListableContents;
using NewsManagement.Entities.Tags;
using NewsManagement.Entities.Videos;
using NewsManagement.EntityConsts.ListableContentConsts;
using NewsManagement.EntityDtos.GalleryDtos;
using NewsManagement.EntityDtos.NewsDtos;
using NewsManagement.EntityDtos.PagedAndSortedDtos;
using NewsManagement.EntityDtos.VideoDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;
using static NewsManagement.Permissions.NewsManagementPermissions;

namespace NewsManagement.Entities.Newses
{
  public class NewsManager : ListableContentBaseManager<News, NewsDto, GetListPagedAndSortedDto, CreateNewsDto, UpdateNewsDto>
  {
    private readonly IObjectMapper _objectMapper;
    private readonly ITagRepository _tagRepository;
    private readonly IFileRepository _fileRepository;
    private readonly ICityRepository _cityRepository;
    private readonly INewsRepository _newsRepository;
    private readonly IVideoRepository _videoRepository;
    private readonly IGalleryRepository _galleryRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IRepository<NewsDetailImage> _newsDetailImageRepository;
    private readonly IListableContentGenericRepository<News> _genericRepository;
    private readonly IListableContentTagRepository _listableContentTagRepository;
    private readonly IListableContentCityRepository _listableContentCityRepository;
    private readonly IListableContentCategoryRepository _listableContentCategoryRepository;
    private readonly IListableContentRelationRepository _listableContentRelationRepository;


    public NewsManager(
      IObjectMapper objectMapper,
      ITagRepository tagRepository,
      IFileRepository fileRepository,
      INewsRepository newsRepository,
      ICityRepository cityRepository,
      IVideoRepository videoRepository,
      IGalleryRepository galleryRepository,
      ICategoryRepository categoryRepository,
      IRepository<NewsDetailImage> newsDetailImageRepository,
      IListableContentGenericRepository<News> genericRepository,
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
      _newsDetailImageRepository = newsDetailImageRepository;
    }

    public async Task<NewsDto> CreateAsync(CreateNewsDto createNewsDto)
    {
      var creatingNews = await CheckCreateInputBaseAsync(createNewsDto);

      foreach (var imageId in creatingNews.DetailImageIds)
      {
        var images = _fileRepository.GetAsync(imageId.DetailImageId).Result;
      }

      var news = await _genericRepository.InsertAsync(creatingNews, autoSave: true);

      await CreateCrossEntity(createNewsDto, news.Id);

      var newsDto = _objectMapper.Map<News, NewsDto>(news);

      await GetCrossEntityAsync(newsDto);

      return newsDto;
    }

    public async Task<NewsDto> UpdateAsync(int id, UpdateNewsDto updateNewsDto)
    {
      var updatingNews = await CheckUpdateInputBaseAsync(id, updateNewsDto);

      foreach (var imageId in updatingNews.DetailImageIds)
      {
        var images = _fileRepository.GetAsync(imageId.DetailImageId).Result;
      }

      await _newsDetailImageRepository.DeleteAsync(x => x.NewsId == id);
      var news = await _genericRepository.UpdateAsync(updatingNews, autoSave: true);

      await ReCreateCrossEntity(updateNewsDto, news.Id);

      var newsDto = _objectMapper.Map<News, NewsDto>(news);

      await GetCrossEntityAsync(newsDto);

      return newsDto;
    }

    public async Task<PagedResultDto<NewsDto>> GetListAsync(GetListPagedAndSortedDto input)
    {
      var filteredList = await GetListFilterBaseAsync(input);

      foreach (var item in filteredList.Items.ToList())
      {
        var newsDetailImage = await _newsDetailImageRepository.GetListAsync(x => x.NewsId == item.Id);
        item.DetailImageId = newsDetailImage.Select(x => x.DetailImageId).ToList();
      }

      return filteredList;
    }

    public async Task<NewsDto> GetByIdAsync(int id)
    {
      var news = await GetByIdBaseAsync(id);

      var newsDetailImage = await _newsDetailImageRepository.GetListAsync(x => x.NewsId == id);
      news.DetailImageId = newsDetailImage.Select(x => x.DetailImageId).ToList();

      await GetCrossEntityAsync(news);

      return news;
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
