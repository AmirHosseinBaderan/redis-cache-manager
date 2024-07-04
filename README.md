# redis-cache-manager
cache manager for any cache data base with redis client 

# How use this

### install package from nuget
``` cli
dotnet add package  --version 1.0.0
```

### Add Redis Cache manager to your services 

``` Csharp
  using RedisCacheManager.Configuration;

  ....

  builder.Services.AddRedisCacheManager(()=> new CacheConfigs("127.0.0.1:6379",1));
```

#### Use Abstractions in your code 

``` Csharp

public class Product(ICache cache,IProductRepository productRepository) : IProduct
{
    public async Task<Product> FindByIdAsync(Guid id)
    {
        string key = $"find-product-{id}";
        var product = await cache.GetOrderSetItemAsync(id,async()=> await productRepository.FindById(id));
        return product;
    }

    publick async Task<Product?> FindByIdFromCache(Guid id)
    {
        string key = $"find-product-{id}";
        var product = await cache.GetItemAsync<Product>(key);
        return product;
    }

    publick async Task<Product?> FindByIdAndCache(Guid id)
    {
        string key = $"find-product-{id}";
        var product = await cache.GetItemAsync<Product>(key);
        product ??= await productRepository.FindById(id);
        return cache.SetItemIfAsync(product is not null,key,product,new CacheDuration(Minutes:2));
    }
}

```
