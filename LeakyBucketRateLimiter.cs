using System.Collections.Immutable;

public class LeakyBucketRateLimiter
{
    private readonly int _capacity;
    private readonly double _leakRate;
    private ImmutableDictionary<string, BucketInfo> _buckets;

    public LeakyBucketRateLimiter(int capacity, double leakRate)
    {
        _capacity = capacity;
        _leakRate = leakRate;
        _buckets = ImmutableDictionary<string, BucketInfo>.Empty;
    }

    public (bool, LeakyBucketRateLimiter) HandleRequest(string userId, double timestamp)
    {
        if (!_buckets.ContainsKey(userId))
        {
            var newBucket = new BucketInfo
            {
                Capacity = _capacity,
                Timestamps = [timestamp]
            };
            _buckets = _buckets.Add(userId, newBucket);
        }
        else
        {
            var bucket = _buckets[userId];
            var leakedTimestamps = bucket.Timestamps.Where(t => t > timestamp - (1.0 / _leakRate)).ToImmutableList();
            if (leakedTimestamps.Count < _capacity)
            {
                leakedTimestamps = leakedTimestamps.Add(timestamp);
                var updatedBucket = new BucketInfo
                {
                    Capacity = _capacity,
                    Timestamps = leakedTimestamps
                };
                _buckets = _buckets.SetItem(userId, updatedBucket);
            }
            else
            {
                return (false, this);
            }
        }

        return (true, this);
    }

    public BucketInfo? GetState(string userId)
    {
        return _buckets.ContainsKey(userId) ? _buckets[userId] : null;
    }
}

public class BucketInfo
{
    public int Capacity { get; set; }
    public ImmutableList<double> Timestamps { get; set; } = ImmutableList<double>.Empty;

    public override string ToString()
    {
        return $"Capacity: {Capacity}, Timestamps: [{string.Join(", ", Timestamps)}]";
    }
}
