using NewsManagement.Entities.Categories;
using NewsManagement.Entities.ListableContentRelations;
using NewsManagement.EntityConsts.ListableContentConsts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NewsManagement.Entities.ListableContents
{
  public class ListableContent : FullAuditedAggregateRoot<int>, IMultiTenant
  {
    public string Spot { get; set; }
    public string Title { get; set; }
    public Guid? ImageId { get; set; }// ❓ bunun kontrölünü yap var mı yok mu
    public Guid? TenantId { get; set; }
    public StatusType Status { get; set; }
    public DateTime PublishTime { get; set; }
    public ListableContentType listableContentType { get; set; }// ❓ bunu ne için kullanacaz. ayrıca create için bunu kalrdırdık
    public ICollection<ListableContentTag> ListableContentTags { get; set; }
    public ICollection<ListableContentCity>? ListableContentCities { get; set; }
    public ICollection<ListableContentCategory> ListableContentCategories { get; set; }


    public ICollection<ListableContentRelation> ListableContentRelations { get; set; }// ❓ bunun kontrölü nasıl yapılacak

    public ListableContent()
    {
      ListableContentRelations = new List<ListableContentRelation>();
    }

  }
}
