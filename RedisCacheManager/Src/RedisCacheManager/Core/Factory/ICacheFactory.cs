namespace RedisCacheManager.Core.Factory;

public interface ICacheFactory
{
    ICache<TModel> CreateJsonCache<TModel>();

    ICache<TModel> CreateProtoCache<TModel>() where TModel : IMessage<TModel>, new();
}