using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using RedisCacheManager.Abstraction;
using RedisCacheManager.Configuration;

namespace RedisCacheManager.Test.Cache;

public class CacheTest
{
    private IServiceCollection _services;

    private CacheModel _model;

    private readonly string _key = "Cached-Item";

    [SetUp]
    public void SetUp()
    {
        _model = new("1", "Amir", "Baderan");

        _services = new ServiceCollection();
        _services.AddRedisCacheManager(() => new("127.0.0.1:6379", 1));
    }

    public async Task<ICache?> GetService()
    {
        IServiceProvider provider = _services.BuildServiceProvider();
        return provider.GetService<ICache?>();
    }

    [Test, Benchmark()]
    public async Task SetCache()
    {
        var service = await GetService();
        if (service is null)
        {
            Assert.Fail("Cant inject services");
            return;
        }

        var result = await service.SetItemAsync(_key, _model);
        if (result != _model)
        {
            Assert.Fail("Set item fail");
            return;
        }

        Assert.Pass("Item set successfuly");
        return;
    }

    [Test, Benchmark()]
    public async Task GetCache()
    {
        var service = await GetService();
        if (service is null)
        {
            Assert.Fail("Cant inject services");
            return;
        }

        var result = await service.GetItemAsync<CacheModel>(_key);
        if (result is CacheModel and
            {
                Id: "1", Name: "Amir", LastName: "Baderan",
            })
        {
            Assert.Fail("Get item fail");
            return;
        }

        Assert.Pass("Item Get successfuly");
        return;
    }

    [Test, Benchmark()]
    public async Task GetOrSetCache()
    {
        var service = await GetService();
        if (service is null)
        {
            Assert.Fail("Cant inject services");
            return;
        }

        var result = await service.GetOrderSetItemAsync(_key + "GetOrSet", () =>
        {
            CacheModel model = new("2", "Amir2", "Baderan2");
            return model;
        });
        if (result is not CacheModel { Id: "2", Name: "Amir2", LastName: "Baderan2" })
        {
            Assert.Fail("Get or set item fail");
            return;
        }

        Assert.Pass("Item Get or set successfuly");
        return;
    }
}
