namespace RedisCacheManager.Configuration;

public record CacheDuration(int Milliseconds = 0, int Second = 0, int Minute = 0, int Hour = 0, int Day = 0)
{
    public TimeSpan ToTimeSpan()
        => new(days: Day,
            hours: Hour,
            minutes: Minute,
            seconds: Second,
            milliseconds: Milliseconds);
}
