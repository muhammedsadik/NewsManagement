using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace NewsManagement.Entities.ListableContents
{
  public class ListableContent : FullAuditedAggregateRoot<int>
  {
    public string Title { get; set; }
    public string Spot { get; set; }
    public bool Status { get; set; }
    public DateTime PublishTime { get; set; }
  }
}
