using NewsManagement.Entities.ListableContents;
using NewsManagement.Entities.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace NewsManagement.Entities.ListableContentRelations
{
  public class ListableContentTag : Entity, IMultiTenant
  {
    public Guid? TenantId { get; set; }
    public int TagId { get; set; }
    public Tag Tag { get; set; }
    public int ListableContentId { get; set; }
    public ListableContent ListableContent { get; set; }


    internal ListableContentTag() { }

    public override object[] GetKeys()
    {
      return new object[] { TagId, ListableContentId };
    }
  }
}
