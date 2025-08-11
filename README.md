
# RedisCacheManager

A flexible cache manager for any cache database using a Redis client.

---

## How to Use

### Install via NuGet

```bash
dotnet add package RedisCacheManager --version 1.6.1
````


### Register RedisCacheManager in Your Services

```csharp
using RedisCacheManager.Configuration;

...

builder.Services.AddRedisCacheManager(() => new CacheConfigs("127.0.0.1:6379", 1));
```

---

### Using Cache Abstractions in Your Code

```csharp
public class ProductService : IProductService
{
    private readonly ICache _cache;
    private readonly IProductRepository _productRepository;

    public ProductService(ICache cache, IProductRepository productRepository)
    {
        _cache = cache;
        _productRepository = productRepository;
    }

    public async Task<Product> FindByIdAsync(Guid id)
    {
        string key = $"find-product-{id}";
        var product = await _cache.GetOrSetItemAsync(key, async () => await _productRepository.FindById(id));
        return product;
    }

    public async Task<Product?> FindByIdFromCacheAsync(Guid id)
    {
        string key = $"find-product-{id}";
        var product = await _cache.GetItemAsync<Product>(key);
        return product;
    }

    public async Task<Product?> FindByIdAndCacheAsync(Guid id)
    {
        string key = $"find-product-{id}";
        var product = await _cache.GetItemAsync<Product>(key);
        if (product == null)
        {
            product = await _productRepository.FindById(id);
            if (product != null)
            {
                await _cache.SetItemIfAsync(true, key, product, new CacheDuration(minutes: 2));
            }
        }
        return product;
    }
}
```

---

## Cache Abstractions

The library provides two main abstractions to manage caching:

* `IJsonCache` — Cache management using JSON serialization (recommended for ease of use with models).
* `IProtoCache` — Cache management using Protobuf serialization (recommended for minimizing cache size and improving performance).

Use `JsonCache` if you want simple serialization of your models.
Use `IProtoCache` if you want to optimize cache size and performance with Protobuf.

