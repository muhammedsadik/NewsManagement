using System;
using System.Collections.Generic;
using System.Text;

namespace NewsManagement.EntityConsts.ListableContentConsts
{
  public enum StatusType
  {
    Draft,          // Taslak
    PendingReview,  // İnceleme Bekleyen
    Published,      // Yayınlanmış
    Archived,       // Arşivlenmiş
    Deleted,        // Silinmiş
    Scheduled,      // Planlanmış
    Rejected        // Reddedilmiş
  }
}
