namespace NewsManagement.Permissions;

public static class NewsManagementPermissions
{
  public const string GroupName = "NewsManagement";

  public static class Tags
  {
    public const string Default = GroupName + ".Tags";
    public const string Create = Default + ".Create";
    public const string Edit = Default + ".Edit";
    public const string Delete = Default + ".Delete";
  }
  
  public static class Cities
  {
    public const string Default = GroupName + ".Cities";
    public const string Create = Default + ".Create";
    public const string Edit = Default + ".Edit";
    public const string Delete = Default + ".Delete";
  }


}
