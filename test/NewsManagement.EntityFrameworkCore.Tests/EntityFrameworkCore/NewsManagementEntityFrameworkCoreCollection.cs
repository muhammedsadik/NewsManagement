using Xunit;

namespace NewsManagement.EntityFrameworkCore;

[CollectionDefinition(NewsManagementTestConsts.CollectionDefinitionName)]
public class NewsManagementEntityFrameworkCoreCollection : ICollectionFixture<NewsManagementEntityFrameworkCoreFixture>
{

}
