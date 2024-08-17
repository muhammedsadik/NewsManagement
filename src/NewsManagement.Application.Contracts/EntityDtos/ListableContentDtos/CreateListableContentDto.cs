using NewsManagement.EntityConsts.ListableContentConsts;
using NewsManagement.EntityDtos.CityDtos;
using NewsManagement.EntityDtos.TagDtos;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace NewsManagement.EntityDtos.ListableContentDtos
{
  public class CreateListableContentDto : EntityDto
  {
    public string Title { get; set; }
    public string Spot { get; set; }
    public StatusType Status { get; set; }
    public Guid? ImageId { get; set; }
    public int[] TagId { get; set; }
    public int[]? CityCode { get; set; }
    public int[]? RelatedListableContent { get; set; }
    public ListableContentType listableContentType { get; set; }
    public List<ListableContentCategoryDto> ListableContentCategoryDtos { get; set; }
  }
}
