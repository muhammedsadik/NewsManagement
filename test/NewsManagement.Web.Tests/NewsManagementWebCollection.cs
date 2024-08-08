using NewsManagement.EntityFrameworkCore;
using Xunit;

namespace NewsManagement;

[CollectionDefinition(NewsManagementTestConsts.CollectionDefinitionName)]
public class NewsManagementWebCollection : NewsManagementEntityFrameworkCoreCollectionFixtureBase
{

}
