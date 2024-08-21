using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsManagement.Entities.ListableContentRelations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NewsManagement.Entities.Cities
{
    public class City : FullAuditedAggregateRoot<int>, IMultiTenant
  {
    public Guid? TenantId { get; set; }
    public string CityName  { get; set; }
    public int CityCode { get; set; }
    public List<ListableContentCity> ListableContentCities { get; set; }

    internal City()
    {

    }

  }
}
