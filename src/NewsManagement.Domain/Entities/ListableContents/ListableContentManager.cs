using NewsManagement.EntityDtos.ListableContentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using NewsManagement.Entities.Galleries;
using NewsManagement.Entities.Newses;
using NewsManagement.Entities.Videos;

namespace NewsManagement.Entities.ListableContents
{
  public class ListableContentManager : DomainService
  {
    private readonly GalleryManager _galleryManager;
    private readonly NewsManager _newsManager;
    private readonly VideoManager _videoManager;
    private readonly IListableContentRepository _listableContentRepository;
    

    public ListableContentManager(
      GalleryManager galleryManager,
      NewsManager newsManager,
      VideoManager videoManager,
      IListableContentRepository listableContentRepository
      
      )
    {
      _galleryManager = galleryManager;
      _newsManager = newsManager;
      _videoManager = videoManager;
      _listableContentRepository = listableContentRepository;
      
    }

    public async Task<ListableContentDto> GetByIdAsync(int id)
    {
      var listableContent = await _listableContentRepository.GetAsync(id);

      ListableContentDto dto = null;

      if (listableContent is Gallery)
      {
        dto = await _galleryManager.GetByIdAsync(id);
      }

      if (listableContent is News)
      {
        dto = await _newsManager.GetByIdAsync(id);
      }

      if (listableContent is Video)
      {
        dto = await _videoManager.GetByIdAsync(id);
      }

      return dto;
    }

  }
}
