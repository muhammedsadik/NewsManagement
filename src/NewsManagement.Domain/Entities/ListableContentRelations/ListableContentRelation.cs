using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsManagement.Entities.ListableContents;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace NewsManagement.Entities.ListableContentRelations
{
  public class ListableContentRelation : Entity, IMultiTenant
  {
    public Guid? TenantId { get; set; }
    public int ListableContentId { get; set; }
    public ListableContent ListableContent { get; set; }

    public int RelatedListableContentId { get; set; }
    public ListableContent RelatedListableContent { get; set; }

    internal ListableContentRelation() { }


    public override object[] GetKeys()
    {
      return new object[] { ListableContentId, RelatedListableContentId };
    }
  }
}
