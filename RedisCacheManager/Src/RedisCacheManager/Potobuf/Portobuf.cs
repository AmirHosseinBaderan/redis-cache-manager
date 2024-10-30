using Google.Protobuf;

namespace RedisCacheManager;

public static class RedisPortobuf
{
    public static byte[] Serialize<T>(this T obj) where T : IMessage<T>
    {
        using MemoryStream ms = new();
        obj.WriteTo(ms);
        return ms.ToArray();
    }

    public static T Deserialize<T>(this byte[] data) where T : IMessage<T>, new()
    {
        T model = new();
        return (T)model.Descriptor.Parser.ParseFrom(data);
    }
}
