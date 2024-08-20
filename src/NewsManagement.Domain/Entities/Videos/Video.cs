using NewsManagement.Entities.ListableContents;
using NewsManagement.EntityConsts.VideoConsts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagement.Entities.Videos
{
  public class Video : ListableContent
  {
    public VideoType VideoType { get; set; }// bunun hakkında bilgi sahibi ol.
    public string? Url { get; set; }
    //eğer url değilde fiziksel video ise onun property sini al ❓

    public Video() { }

  }
}
