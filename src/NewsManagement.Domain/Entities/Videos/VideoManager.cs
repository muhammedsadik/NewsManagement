using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;

namespace NewsManagement.Entities.Videos
{
  public class VideoManager : DomainService
  {
    private readonly IVideoRepository _videoRepository;
    private readonly IObjectMapper _objectMapper;

    public VideoManager(IObjectMapper objectMapper, IVideoRepository videoRepository)
    {
      _objectMapper = objectMapper;
      _videoRepository = videoRepository;
    }



  }
}
