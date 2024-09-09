using NewsManagement.EntityConsts.ListableContentConsts;
using NewsManagement.EntityDtos.CategoryDtos;
using NewsManagement.EntityDtos.CityDtos;
using NewsManagement.EntityDtos.TagDtos;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace NewsManagement.EntityDtos.ListableContentDtos
{
  public class ListableContentDto : FullAuditedEntityDto<int>
  {
    public string Title { get; set; }
    public string Spot { get; set; }
    public Guid? ImageId { get; set; }
    public int ViewsCount { get; set; }
    public StatusType Status { get; set; }
    public DateTime? PublishTime { get; set; }
    public ListableContentType ListableContentType { get; set; }
    public List<ReturnTagDto> Tags { get; set; }
    public List<ReturnCityDto> Cities { get; set; }
    public List<ReturnCategoryDto> Categories { get; set; }
    public List<ReturnListableContentRelationDto> ListableContentRelations { get; set; }
  }
}
