using AutoMapper;
using NewsManagement.Entities.Tag;
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




  }
}
