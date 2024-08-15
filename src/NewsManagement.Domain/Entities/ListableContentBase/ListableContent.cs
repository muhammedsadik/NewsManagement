using NewsManagement.Entities.Categories;
using NewsManagement.Entities.Cities;
using NewsManagement.Entities.Tags;
using NewsManagement.EntityConsts.ListableContentConsts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace NewsManagement.Entities.ListableContentBase
{
  public class ListableContent : FullAuditedAggregateRoot<int>
  {
    public string Title { get; set; }
    public string Spot { get; set; }
    public bool Status { get; set; }
    public DateTime PublishTime { get; set; }
    public Guid ImageId { get; set; }//file da kaydet
    public ListableContentType listableContentType { get; set; }
    public ICollection<ListableContentCategory> ListableContentCategories { get; set; }
    public ICollection<ListableContentCity> ListableContentCities { get; set; }
    public ICollection<ListableContentTag> ListableContentTags { get; set; }
    //public ICollection<ListableContent> ListableContents { get; set; }


    internal ListableContent() { }

  }
}
