using AutoMapper;
using NewsManagement.Entities.Cities;
using NewsManagement.Entities.Tags;
using NewsManagement.EntityDtos.CityDtos;
using NewsManagement.EntityDtos.TagDtos;

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




  }
}
