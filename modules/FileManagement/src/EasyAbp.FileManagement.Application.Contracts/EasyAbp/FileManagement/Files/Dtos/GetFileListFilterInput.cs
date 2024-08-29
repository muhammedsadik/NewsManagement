using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyAbp.FileManagement.Files.Dtos
{
  public class GetFileListFilterInput : GetFileListInput
  {
    [CanBeNull]
    public string Filter { get; set; }
  }
}
