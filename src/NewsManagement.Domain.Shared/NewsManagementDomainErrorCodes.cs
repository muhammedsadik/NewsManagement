namespace NewsManagement;

public static class NewsManagementDomainErrorCodes
{
  public const string NotFound = "NewsManagement:500";
  public const string AlreadyExists = "NewsManagement:501";
  public const string FilterLimitsError = "NewsManagement:502";
  public const string OnlyOneSubCategory = "NewsManagement:503";
  public const string MainCategoryWithSubCannotBeChanged = "NewsManagement:504";
  public const string WrongTypeSelectionInCreateStatus = "NewsManagement:505";
  public const string RepeatedDataError = "NewsManagement:506";
  public const string DraftStatusCannotHaveaPublishingTime = "NewsManagement:507";
  public const string PendingReviewStatusCannotHaveaPublishingTime = "NewsManagement:508";
  public const string ArchivedStatusCannotHaveaPublishingTime = "NewsManagement:509";
  public const string RejectedStatusCannotHaveaPublishingTime = "NewsManagement:510";
  public const string DeletedStatusCannotHaveaPublishingTime = "NewsManagement:511";
  public const string PublishedStatusMustHaveaPublishingTime = "NewsManagement:512";
  public const string PublishedStatusDatetimeTimeoutError = "NewsManagement:513";
  public const string PublishedStatusDatetimeMustNowOrNull = "NewsManagement:514";
  public const string ScheduledStatusDatetimeMustBeInTheFuture = "NewsManagement:515";
  public const string WithoutParentCategory = "NewsManagement:516";
  public const string CannotAddItself = "NewsManagement:517";




  #region ValidationErrorCodes
  public const string NotInListableContentEnumType = "NewsManagement:v401";
  public const string NotInVideoEnumType = "NewsManagement:v402";
  public const string OnlyOneCategoryIsActiveStatusTrue = "NewsManagement:v403";
  public const string NotInStatusEnumType = "NewsManagement:v404";

  #endregion
}
