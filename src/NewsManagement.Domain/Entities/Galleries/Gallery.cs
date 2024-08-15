using NewsManagement.Entities.ListableContentBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagement.Entities.Galleries
{
  public class Gallery : ListableContent
  {
    public List<GalleryImage> GalleryImage { get; set; }

    internal Gallery() { }
  }
}
