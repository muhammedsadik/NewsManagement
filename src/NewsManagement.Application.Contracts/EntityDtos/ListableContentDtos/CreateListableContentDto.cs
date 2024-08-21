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
    public List<int> TagIds { get; set; } //null olamaz validationda kontrol et
    public List<int> CityIds { get; set; }// not null
    public List<int> RelatedListableContentIds { get; set; }// ❓ bunun kontrölü nasıl (hangi repo da) yapılacak
    public List<ListableContentCategoryDto> ListableContentCategoryDtos { get; set; }// ✔
  }
}
