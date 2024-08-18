namespace NewsManagement;

public static class NewsManagementDomainErrorCodes
{
  public const string NotFound = "NewsManagement:500";
  public const string AlreadyExists = "NewsManagement:501";
  public const string FilterLimitsError = "NewsManagement:502";
  public const string OnlyOneSubCategory = "NewsManagement:503";
  public const string MainCategoryWithSubCannotBeChanged = "NewsManagement:504";
  public const string WrongTypeSelectionInCreateStatus = "NewsManagement:505";
  public const string IfStatusPublishedDatetimeMustNowOrNull = "NewsManagement:506";


  public const string RepeatedDataError = "agency_transfercenter:";// <= zamanı gelince kod ver


  #region ValidationErrorCodes
  public const string NotInListableContentEnumType = "agency_transfercenter:v401";
  public const string NotInVideoEnumType = "agency_transfercenter:v402";
  public const string OnlyOneCategoryIsActiveStatusTrue = "agency_transfercenter:v403";

  #endregion
}
