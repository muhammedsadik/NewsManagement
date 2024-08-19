using System;
using System.Collections.Generic;
using System.Text;

namespace NewsManagement.EntityConsts.ListableContentConsts
{
  public enum StatusType
  {
    Draft,          // Taslak  
    PendingReview,  // İnceleme 
    Archived,       // Arşivlenmiş 
    Published,      // Yayınlanmış  
    Scheduled,      // Planlanmış 
    Deleted,        // Silinmiş  
    Rejected        // Reddedilmiş 
  }
}
