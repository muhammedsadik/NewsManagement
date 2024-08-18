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
    public string Title { get; set; }//null olamaz validationda kontrol et
    public string Spot { get; set; }
    public int[] TagIds { get; set; } //null olamaz validationda kontrol et
    public int[]? CityIds { get; set; }
    public int[]? RelatedListableContentIds { get; set; }// ❓ bunun kontrölü nasıl yapılacak
    public Guid? ImageId { get; set; }
    public List<ListableContentCategoryDto> ListableContentCategoryDtos { get; set; }//null olamaz validationda kontrol et
  }
}
