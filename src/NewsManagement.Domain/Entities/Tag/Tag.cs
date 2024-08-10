using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsManagement.Entities.ListableContents;
using Volo.Abp.Domain.Entities.Auditing;

namespace NewsManagement.Entities.Tag
{
  public class Tag : FullAuditedAggregateRoot<int>
  {
    public string TagName { get; set; }
  }
}
