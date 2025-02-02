﻿namespace NewsManagement.Permissions;

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
  
  public static class Categories
  {
    public const string Default = GroupName + ".Categories";
    public const string Create = Default + ".Create";
    public const string Edit = Default + ".Edit";
    public const string Delete = Default + ".Delete";
  }
  
  public static class Videos
  {
    public const string Default = GroupName + ".Videos";
    public const string Create = Default + ".Create";
    public const string Edit = Default + ".Edit";
    public const string Delete = Default + ".Delete";
  }
  
  public static class Galleries
  {
    public const string Default = GroupName + ".Galleries";
    public const string Create = Default + ".Create";
    public const string Edit = Default + ".Edit";
    public const string Delete = Default + ".Delete";
  }
  
  public static class Newses
  {
    public const string Default = GroupName + ".Newses";
    public const string Create = Default + ".Create";
    public const string Edit = Default + ".Edit";
    public const string Delete = Default + ".Delete";
  }
  


  /*public static class ListableContents
  {
    public const string Default = GroupName + ".ListableContents";
    public const string Create = Default + ".Create";
    public const string Edit = Default + ".Edit";
    public const string Delete = Default + ".Delete";
  }*/


}
