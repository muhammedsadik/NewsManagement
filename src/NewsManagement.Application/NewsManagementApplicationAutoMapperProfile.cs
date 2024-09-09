﻿using AutoMapper;
using NewsManagement.Entities.Categories;
using NewsManagement.Entities.Cities;
using NewsManagement.Entities.Galleries;
using NewsManagement.Entities.ListableContentRelations;
using NewsManagement.Entities.ListableContents;
using NewsManagement.Entities.Newses;
using NewsManagement.Entities.Tags;
using NewsManagement.Entities.Videos;
using NewsManagement.EntityDtos.CategoryDtos;
using NewsManagement.EntityDtos.CityDtos;
using NewsManagement.EntityDtos.GalleryDtos;
using NewsManagement.EntityDtos.ListableContentDtos;
using NewsManagement.EntityDtos.NewsDtos;
using NewsManagement.EntityDtos.TagDtos;
using NewsManagement.EntityDtos.VideoDtos;
using System.Linq;

namespace NewsManagement;

public class NewsManagementApplicationAutoMapperProfile : Profile
{
  public NewsManagementApplicationAutoMapperProfile()
  {
    #region Tag
    CreateMap<Tag, TagDto>().ReverseMap();
    CreateMap<UpdateTagDto, Tag>().ReverseMap();
    CreateMap<CreateTagDto, Tag>().ReverseMap();
    #endregion

    #region City
    CreateMap<City, CityDto>().ReverseMap();
    CreateMap<UpdateCityDto, City>().ReverseMap();
    CreateMap<CreateCityDto, City>().ReverseMap();
    #endregion

    #region Category
    CreateMap<Category, CategoryDto>().ReverseMap();
    CreateMap<UpdateCategoryDto, Category>().ReverseMap();
    CreateMap<CreateCategoryDto, Category>().ReverseMap();


    #endregion

    #region News
    CreateMap<News, NewsDto>()
    .ForMember(dest => dest.DetailImageId, opt => opt.MapFrom(src => src.DetailImageIds.Select(d => d.DetailImageId).ToList()))
    .ReverseMap()
    .ForMember(dest => dest.DetailImageIds, opt => opt.MapFrom(src => src.DetailImageId.Select(id => new NewsDetailImage { DetailImageId = id }).ToList()));

    CreateMap<UpdateNewsDto, News>().ReverseMap();
    CreateMap<CreateNewsDto, News>().ReverseMap();

    CreateMap<NewsDetailImageDto, NewsDetailImage>().ReverseMap();
    CreateMap<CreateNewsDto, UpdateNewsDto>().ReverseMap();
    #endregion

    #region Gallery
    CreateMap<Gallery, GalleryDto>().ReverseMap();
    CreateMap<UpdateGalleryDto, Gallery>().ReverseMap();
    CreateMap<CreateGalleryDto, Gallery>().ReverseMap();
    CreateMap<GalleryImage, GalleryImageDto>().ReverseMap();

    CreateMap<CreateGalleryDto, UpdateGalleryDto>().ReverseMap();
    #endregion

    #region Video
    CreateMap<Video, VideoDto>().ReverseMap();
    CreateMap<UpdateVideoDto, Video>().ReverseMap();
    CreateMap<CreateVideoDto, Video>().ReverseMap();
    CreateMap<CreateVideoDto, UpdateVideoDto>().ReverseMap();
    #endregion

    #region ListableContent
    CreateMap<ListableContent, ListableContentDto>().ReverseMap();
    CreateMap<UpdateListableContentDto, ListableContent>().ReverseMap();
    CreateMap<CreateListableContentDto, ListableContent>().ReverseMap();

    CreateMap<ListableContentCategory, ReturnCategoryDto>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryId))
        .ForMember(dest => dest.IsPrimary, opt => opt.MapFrom(src => src.IsPrimary))
        .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
        .ForMember(dest => dest.ColorCode, opt => opt.MapFrom(src => src.Category.ColorCode))
        .ForMember(dest => dest.ParentCategoryId, opt => opt.MapFrom(src => src.Category.ParentCategoryId));

    CreateMap<ListableContentTag, ReturnTagDto>()
      .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TagId))
      .ForMember(dest => dest.TagName, opt => opt.MapFrom(src => src.Tag.TagName));

    CreateMap<ListableContentCity, ReturnCityDto>()
      .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CityId))
      .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.CityName))
      .ForMember(dest => dest.CityCode, opt => opt.MapFrom(src => src.City.CityCode));

    CreateMap<ListableContentRelation, ReturnListableContentRelationDto>()
      .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RelatedListableContent.Id))
      .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.RelatedListableContent.Title))
      .ForMember(dest => dest.ViewsCount, opt => opt.MapFrom(src => src.RelatedListableContent.ViewsCount))
      .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.RelatedListableContent.Status));
    #endregion




  }
}
