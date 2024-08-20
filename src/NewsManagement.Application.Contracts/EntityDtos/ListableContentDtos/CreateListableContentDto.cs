using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace NewsManagement.EntityDtos.ListableContentDtos
{
  public class CreateListableContentDto : EntityDto
  {
    public string Title { get; set; }// ✔
    public string Spot { get; set; }// ✔
    public Guid? ImageId { get; set; }//Video olabilir genel sınıfta bulunbduğu için nullable
    public int[] TagIds { get; set; } //null olamaz validationda kontrol et
    public int[]? CityIds { get; set; }
    public int[]? RelatedListableContentIds { get; set; }// ❓ bunun kontrölü nasıl (hangi repo da) yapılacak
    public List<ListableContentCategoryDto> ListableContentCategoryDtos { get; set; }// ✔
  }
}
