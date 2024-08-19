using NewsManagement.EntityConsts.ListableContentConsts;
using NewsManagement.EntityDtos.CityDtos;
using NewsManagement.EntityDtos.TagDtos;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace NewsManagement.EntityDtos.ListableContentDtos
{
  public class UpdateListableContentDto : EntityDto
  {
    public string Title { get; set; }// 0  ✔
    public string Spot { get; set; }
    public int[] TagIds { get; set; }// 1  ✔
    public int[]? CityIds { get; set; }// 2  ✔
    public int[]? RelatedListableContentIds { get; set; }// 3  ✔
    public Guid? ImageId { get; set; }
    public DateTime? PublishTime { get; set; }// 4  ✔
    public StatusType Status { get; set; }// 5  ✔
    public ListableContentType listableContentType { get; set; }// 6 burada type değişirse ne olacak ⚠
    public List<ListableContentCategoryDto> ListableContentCategoryDtos { get; set; }// 7  ✔ //null olamaz validationda kontrol et⚠
  }
}
