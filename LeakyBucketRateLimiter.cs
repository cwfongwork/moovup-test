using System;

public class LeakyBucketRateLimiter
{
    public LeakyBucketRateLimiter(int capacity, double leakRate)
    {
        // Initialize the leaky bucket with given capacity and leak rate
    }

    public (bool, LeakyBucketRateLimiter) HandleRequest(string userId, double timestamp)
    {
        // Implement the leaky bucket algorithm here
        return (true, this);
    }

    public BucketInfo? GetState(string userId)
    {
        // Return the current state of the bucket for the given user
        return new BucketInfo { Capacity = 0 };
    }
}

public class BucketInfo
{
    public int Capacity { get; set; }

    public override string ToString()
    {
        return $"Capacity: {Capacity}";
    }
}
