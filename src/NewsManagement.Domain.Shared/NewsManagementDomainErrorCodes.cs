namespace NewsManagement;

public static class NewsManagementDomainErrorCodes
{
  public const string NotFound = "NewsManagement:500";
  public const string AlreadyExists = "NewsManagement:501";
  public const string FilterLimitsError = "NewsManagement:502";
  public const string OnlyOneSubCategory = "NewsManagement:503";
  public const string MainCategoryWithSubCannotBeChanged = "NewsManagement:504";
  public const string WrongTypeSelectionInCreateStatus = "NewsManagement:505";
  public const string RepeatedDataError = "agency_transfercenter:506";
  public const string DraftStatusCannotHaveaPublishingTime = "agency_transfercenter:507";
  public const string PendingReviewStatusCannotHaveaPublishingTime = "agency_transfercenter:508";
  public const string ArchivedStatusCannotHaveaPublishingTime = "agency_transfercenter:509";
  public const string RejectedStatusCannotHaveaPublishingTime = "agency_transfercenter:510";
  public const string DeletedStatusCannotHaveaPublishingTime = "agency_transfercenter:511";
  public const string PublishedStatusMustHaveaPublishingTime = "agency_transfercenter:512";




  public const string IfStatusPublishedDatetimeMustNowOrNull = "NewsManagement:50";




  #region ValidationErrorCodes
  public const string NotInListableContentEnumType = "agency_transfercenter:v401";
  public const string NotInVideoEnumType = "agency_transfercenter:v402";
  public const string OnlyOneCategoryIsActiveStatusTrue = "agency_transfercenter:v403";

  #endregion
}
