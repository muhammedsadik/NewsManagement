using NewsManagement.Entities.ListableContents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace NewsManagement.Entities.Categories
{
  public class ListableContentCategory : Entity
  {
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public int ListableContentId { get; set; }
    public ListableContent ListableContent { get; set; }



    internal ListableContentCategory() { }

    public override object[] GetKeys()
    {
      return new object[] { CategoryId, ListableContentId };
    }

  }
}
