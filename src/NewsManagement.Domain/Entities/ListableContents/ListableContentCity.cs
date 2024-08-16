using NewsManagement.Entities.Cities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace NewsManagement.Entities.ListableContents
{
    public class ListableContentCity : Entity
    {
        public int CityId { get; set; }
        public City City { get; set; }
        public int ListableContentId { get; set; }
        public ListableContent ListableContent { get; set; }


        internal ListableContentCity() { }

        public override object[] GetKeys()
        {
            return new object[] { CityId, ListableContentId };
        }
    }
}
