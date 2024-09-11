using NewsManagement.Entities.ListableContents;
using NewsManagement.EntityConsts.ListableContentConsts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;
using Volo.Abp.Uow;

namespace NewsManagement.Entities.BackgroundJobs
{
  public class ChangingStatusTypeJob : AsyncBackgroundJob<int>, ITransientDependency
  {
    private readonly IListableContentRepository _repository;
    private readonly IDataFilter<IMultiTenant> _dataFilter;

    public ChangingStatusTypeJob(IListableContentRepository repository, IDataFilter<IMultiTenant> dataFilter)
    {
      _repository = repository;
      _dataFilter = dataFilter;
    }

    public override async Task ExecuteAsync(int args)
    {
      using (_dataFilter.Disable())
      {
        var listableContents = await _repository.GetListAsync(x => x.Status == StatusType.Scheduled && x.PublishTime <= DateTime.Now);

        foreach (var item in listableContents)
        {
          item.Status = StatusType.Published;
          item.PublishTime = DateTime.Now;

          await _repository.UpdateAsync(item, autoSave: true);
        }
      }
    }
  }
}





























/*

public class ChangingStatusTypeJob : ITransientDependency
{
    private readonly IRepository<ListableContent, int> _repository;

    public ChangingStatusTypeJob(IRepository<ListableContent, int> repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync()
    {
        var listableContents = await _repository.GetListAsync(x => x.Status == StatusType.Scheduled && x.PublishTime <= DateTime.Now);

        foreach (var item in listableContents)
        {
            item.Status = StatusType.Published;
            item.PublishTime = DateTime.Now;

            await _repository.UpdateAsync(item, autoSave: true);
        }
    }
}
 
 RecurringJob.AddOrUpdate<ChangingStatusTypeJob>(
    "ChangingStatusTypeJob",
    job => job.ExecuteAsync(),
    Cron.MinuteInterval(1)
);

*/