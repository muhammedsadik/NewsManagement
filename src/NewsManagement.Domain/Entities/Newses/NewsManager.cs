﻿using NewsManagement.Entities.Categories;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;

namespace NewsManagement.Entities.Newses
{
  public class NewsManager : ListableContentBaseManager<News, NewsDto, GetListPagedAndSortedDto, CreateNewsDto, UpdateNewsDto>
  {
    private readonly IObjectMapper _objectMapper;
    private readonly ITagRepository _tagRepository;
    private readonly ICityRepository _cityRepository;
    private readonly INewsRepository _newsRepository;
    private readonly IVideoRepository _videoRepository;
    private readonly IGalleryRepository _galleryRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IListableContentGenericRepository<News> _genericRepository;
    private readonly IRepository<ListableContentTag> _listableContentTagRepository;
    private readonly IRepository<ListableContentCity> _listableContentCityRepository;
    private readonly IRepository<ListableContentCategory> _listableContentCategoryRepository;
    private readonly IRepository<ListableContentRelation> _listableContentRelationRepository;


    public NewsManager(
      IObjectMapper objectMapper,
      ITagRepository tagRepository,
      ICityRepository cityRepository,
      INewsRepository newsRepository,
      IVideoRepository videoRepository,
      IGalleryRepository galleryRepository,
      ICategoryRepository categoryRepository,
      IListableContentGenericRepository<News> genericRepository,
      IRepository<ListableContentTag> listableContentTagRepository,
      IRepository<ListableContentCity> listableContentCityRepository,
      IRepository<ListableContentCategory> listableContentCategoryRepository,
      IRepository<ListableContentRelation> listableContentRelationRepository
      ) : base(objectMapper, tagRepository, cityRepository, newsRepository,
        videoRepository, galleryRepository, categoryRepository, genericRepository, listableContentTagRepository,
        listableContentCityRepository, listableContentCategoryRepository, listableContentRelationRepository
          )
    {
      _objectMapper = objectMapper;
      _tagRepository = tagRepository;
      _cityRepository = cityRepository;
      _newsRepository = newsRepository;
      _videoRepository = videoRepository;
      _galleryRepository = galleryRepository;
      _genericRepository = genericRepository;
      _categoryRepository = categoryRepository;
      _listableContentTagRepository = listableContentTagRepository;
      _listableContentCityRepository = listableContentCityRepository;
      _listableContentCategoryRepository = listableContentCategoryRepository;
      _listableContentRelationRepository = listableContentRelationRepository;
    }

    public async Task<NewsDto> CreateAsync(CreateNewsDto createNewsDto)
    {
      await CheckCreateInputBaseAsync(createNewsDto);

      var creatingNews = _objectMapper.Map<CreateNewsDto, News>(createNewsDto);

      creatingNews.PublishTime = DateTime.Now;
      creatingNews.Status = StatusType.PendingReview;
      creatingNews.listableContentType = ListableContentType.News;

      // updateNewsDto.DetailImageId  kontrolü
      // ❓ DetailImageId ye ait bir item varmı kontrolünü yap ve => 📩

      var news = await _genericRepository.InsertAsync(creatingNews, autoSave: true);

      await CreateCrossEntity(createNewsDto, news.Id);

      var newsDto = _objectMapper.Map<News, NewsDto>(news);

      return newsDto;
    }

    public async Task<NewsDto> UpdateAsync(int id, UpdateNewsDto updateNewsDto)
    {
      var updatingNews = await CheckUpdateInputBaseAsync(id, updateNewsDto);

      updatingNews.listableContentType = ListableContentType.News;
     
      // updateNewsDto.DetailImageId  kontrolü
      // ❓ DetailImageId ye ait bir item varmı kontrolünü yap ve => 📩

      var news = await _genericRepository.UpdateAsync(updatingNews, autoSave: true);

      await ReCreateCrossEntity(updateNewsDto, news.Id);

      var newsDto = _objectMapper.Map<News, NewsDto>(news);

      return newsDto;
    }

    public async Task<PagedResultDto<NewsDto>> GetListAsync(GetListPagedAndSortedDto input)
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
