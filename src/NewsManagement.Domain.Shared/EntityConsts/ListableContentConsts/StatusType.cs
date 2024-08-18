using System;
using System.Collections.Generic;
using System.Text;

namespace NewsManagement.EntityConsts.ListableContentConsts
{
  public enum StatusType
  {
    Draft,          // Taslak => Henüz yayınlanmamış, üzerinde çalışılan haberler için.
    PendingReview,  // İnceleme Bekleyen => İncelenmek üzere gönderilen haberler için.
    Archived,       // Arşivlenmiş => Eski haberler veya erişimden kaldırılmış haberler için.
    Published,      // Yayınlanmış => Yayınlanmış haberler için.
    Scheduled,      // Planlanmış => Belirli bir tarih ve saatte yayınlanması planlanan haberler için.
    Deleted,        // Silinmiş => Silinmiş haberler için.
    Rejected        // Reddedilmiş =>  İnceleme sürecinde reddedilmiş haberler için.
  }
}
