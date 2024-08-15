using NewsManagement.Entities.Cities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;

namespace NewsManagement.Entities.Galleries
{
  public class GalleryManager : DomainService
  {
    private readonly IGalleryRepository _galleryRepository;
    private readonly IObjectMapper _objectMapper;

    public GalleryManager(IObjectMapper objectMapper, IGalleryRepository galleryRepository)
    {
      _objectMapper = objectMapper;
      _galleryRepository = galleryRepository;
    }




  }
}
