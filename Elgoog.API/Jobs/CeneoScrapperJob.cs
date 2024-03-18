using Elgoog.API.Services.Interfaces;
using Quartz;
using System;

public class CeneoScrapperJob : IJob
{
    private readonly ICeneoScrapper _ceneoScrapper;

    public CeneoScrapperJob(ICeneoScrapper ceneoScrapper)
    {
        _ceneoScrapper = ceneoScrapper;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _ceneoScrapper.GetProducts("test");
        return Task.CompletedTask;
    }
}
