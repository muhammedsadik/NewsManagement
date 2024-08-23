using AutoMapper;
using NewsManagement.Entities.Categories;
using NewsManagement.Entities.Cities;
using NewsManagement.Entities.Galleries;
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
    CreateMap<News, NewsDto>().ReverseMap();
    CreateMap<UpdateNewsDto, News>().ReverseMap();
    CreateMap<CreateNewsDto, News>().ReverseMap();
    #endregion
    
    #region Gallery
    CreateMap<Gallery, GalleryDto>().ReverseMap();
    CreateMap<UpdateGalleryDto, Gallery>().ReverseMap();
    CreateMap<CreateGalleryDto, Gallery>().ReverseMap();
    CreateMap<GalleryImage, GalleryImageDto>().ReverseMap();
    #endregion
    
    #region Video
    CreateMap<Video, VideoDto>().ReverseMap();
    CreateMap<UpdateVideoDto, Video>().ReverseMap();
    CreateMap<CreateVideoDto, Video>().ReverseMap();
    #endregion
    
    #region ListableContent
    CreateMap<ListableContent, ListableContentDto>().ReverseMap();
    CreateMap<UpdateListableContentDto, ListableContent>().ReverseMap();
    CreateMap<CreateListableContentDto, ListableContent>().ReverseMap();
    #endregion




  }
}
