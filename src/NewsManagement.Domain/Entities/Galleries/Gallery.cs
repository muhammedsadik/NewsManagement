using NewsManagement.Entities.ListableContents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagement.Entities.Galleries
{
  public class Gallery : ListableContent
  {
    public List<GalleryImage> GalleryImages { get; set; }

    public Gallery() { }
  }
}
