using NewsManagement.Entities.ListableContantBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;

namespace NewsManagement.Entities.Newses
{
  public class NewsManager : ListableContentBaseManager<News, int>
  {
    private readonly INewsRepository _newsRepository;
    private readonly IObjectMapper _objectMapper;

    public NewsManager(IObjectMapper objectMapper, INewsRepository newsRepository)
    {
      _objectMapper = objectMapper;
      _newsRepository = newsRepository;
    }




  }
}
