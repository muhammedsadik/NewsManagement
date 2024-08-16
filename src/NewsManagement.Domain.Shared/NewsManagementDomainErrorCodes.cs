﻿namespace NewsManagement;

public static class NewsManagementDomainErrorCodes
{
  public const string NotFound = "NewsManagement:500";
  public const string AlreadyExists = "NewsManagement:501";
  public const string FilterLimitsError = "NewsManagement:502";
  public const string JustOneSubCategory = "NewsManagement:503";
  public const string MainCategoryWithSubCannotBeChanged = "NewsManagement:504";




  #region ValidationErrorCodes
  public const string NotInListableContentEnumType = "agency_transfercenter:v401";
  public const string NotInVideoEnumType = "agency_transfercenter:v402";

  
  #endregion
}
