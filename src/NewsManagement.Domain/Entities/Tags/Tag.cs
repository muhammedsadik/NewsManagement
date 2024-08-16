using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsManagement.Entities.ListableContents;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NewsManagement.Entities.Tags
{
  public class Tag : FullAuditedAggregateRoot<int>, IMultiTenant
  {
    public Guid? TenantId { get; set; }
    public string TagName { get; set; }

    public ICollection<ListableContentTag> ListableContentTags { get; set; }


    internal Tag()
    {
    }


  }
}
