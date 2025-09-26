
// Basic functionality
Console.WriteLine("Running code to test basic functionality...");
var limiter = CreateRateLimiter(capacity: 5, leakRate: 1.0);
(var allowed1_0, limiter) = AllowRequest(limiter, "user1", timestamp: 0);
(var allowed1_1, limiter) = AllowRequest(limiter, "user1", timestamp: 1);
Console.WriteLine($"Debug GetBucketState user1: {GetBucketState(limiter, "user1")}");
Console.WriteLine($"Case 1.1: {(allowed1_0 ? "Passed" : "Failed")}");
Console.WriteLine($"Case 1.2: {(allowed1_1 ? "Passed" : "Failed")}");

// Burst handling
// Multiple rapid requests that should be rejected
Console.WriteLine("Running code to test burst handling...");
(var allowed2_0, limiter) = AllowRequest(limiter, "user2", timestamp: 0);
(var allowed2_1, limiter) = AllowRequest(limiter, "user2", timestamp: 0.1);
(var allowed2_2, limiter) = AllowRequest(limiter, "user2", timestamp: 0.2);
(var allowed2_3, limiter) = AllowRequest(limiter, "user2", timestamp: 0.2);
(var allowed2_4, limiter) = AllowRequest(limiter, "user2", timestamp: 0.4);
(var allowed2_5, limiter) = AllowRequest(limiter, "user2", timestamp: 0.5);
Console.WriteLine($"Debug GetBucketState user2: {GetBucketState(limiter, "user2")}");
Console.WriteLine($"Case 2.1: {(!allowed2_5 ? "Passed" : "Failed")}");

// Time-based leaking  
// Requests separated by time should be allowed after leaking
Console.WriteLine("Running code to test time-based leaking...");
(var allowed3_0, limiter) = AllowRequest(limiter, "user3", timestamp: 0);
(var allowed3_1, limiter) = AllowRequest(limiter, "user3", timestamp: 0.1);
(var allowed3_2, limiter) = AllowRequest(limiter, "user3", timestamp: 0.2);
(var allowed3_3, limiter) = AllowRequest(limiter, "user3", timestamp: 0.2);
(var allowed3_4, limiter) = AllowRequest(limiter, "user3", timestamp: 0.4);
(var allowed3_5, limiter) = AllowRequest(limiter, "user3", timestamp: 1.1);
Console.WriteLine($"Debug GetBucketState user3: {GetBucketState(limiter, "user3")}");
Console.WriteLine($"Case 3.1: {(allowed3_5 ? "Passed" : "Failed")}");

// Multiple users
// Independent bucket behavior
Console.WriteLine("Running code to test multiple users...");
(var allowed4_1_0, limiter) = AllowRequest(limiter, "user4_1", timestamp: 0);
(var allowed4_1_1, limiter) = AllowRequest(limiter, "user4_1", timestamp: 0.2);
(var allowed4_1_2, limiter) = AllowRequest(limiter, "user4_1", timestamp: 0.4);
(var allowed4_2_0, limiter) = AllowRequest(limiter, "user4_2", timestamp: 0.5);
(var allowed4_1_3, limiter) = AllowRequest(limiter, "user4_1", timestamp: 0.6);
(var allowed4_2_1, limiter) = AllowRequest(limiter, "user4_2", timestamp: 0.7);
(var allowed4_1_4, limiter) = AllowRequest(limiter, "user4_1", timestamp: 0.8);
(var allowed4_2_2, limiter) = AllowRequest(limiter, "user4_2", timestamp: 0.9);
(var allowed4_2_3, limiter) = AllowRequest(limiter, "user4_2", timestamp: 1.1);
(var allowed4_2_4, limiter) = AllowRequest(limiter, "user4_2", timestamp: 1.3);
(var allowed4_1_5, limiter) = AllowRequest(limiter, "user4_1", timestamp: 1.4);
Console.WriteLine($"Debug GetBucketState user4_1: {GetBucketState(limiter, "user4_1")}");
Console.WriteLine($"Debug GetBucketState user4_2: {GetBucketState(limiter, "user4_2")}");
(var allowed4_2_5, limiter) = AllowRequest(limiter, "user4_2", timestamp: 1.4);
Console.WriteLine($"Case 4.1: {(allowed4_2_2 ? "Passed" : "Failed")}");
Console.WriteLine($"Case 4.2: {(allowed4_1_5 ? "Passed" : "Failed")}");
Console.WriteLine($"Case 4.3: {(!allowed4_2_5 ? "Passed" : "Failed")}");

// Requests at exact leaking intervals
Console.WriteLine("Running code to test requests at exact leaking intervals...");
(var allowed5_0, limiter) = AllowRequest(limiter, "user5", timestamp: 0);
(var allowed5_1, limiter) = AllowRequest(limiter, "user5", timestamp: 0.2);
(var allowed5_2, limiter) = AllowRequest(limiter, "user5", timestamp: 0.4);
(var allowed5_3, limiter) = AllowRequest(limiter, "user5", timestamp: 0.6);
(var allowed5_4, limiter) = AllowRequest(limiter, "user5", timestamp: 0.8);
(var allowed5_5, limiter) = AllowRequest(limiter, "user5", timestamp: 1.0);
Console.WriteLine($"Debug GetBucketState user5: {GetBucketState(limiter, "user5")}");
Console.WriteLine($"Case 5.1: {(allowed5_5 ? "Passed" : "Failed")}");

// Timestamp that go backwards
Console.WriteLine("Running code to test backward timestamps...");
(var allowed6_0, limiter) = AllowRequest(limiter, "user6", timestamp: 0);
(var allowed6_1, limiter) = AllowRequest(limiter, "user6", timestamp: 0.2);
(var allowed6_2, limiter) = AllowRequest(limiter, "user6", timestamp: 0.4);
(var allowed6_3, limiter) = AllowRequest(limiter, "user6", timestamp: 0.6);
(var allowed6_4, limiter) = AllowRequest(limiter, "user6", timestamp: 0.8);
(var allowed6_5, limiter) = AllowRequest(limiter, "user6", timestamp: 0.5);
Console.WriteLine($"Debug GetBucketState user6: {GetBucketState(limiter, "user6")}");
Console.WriteLine($"Case 6.1: {(!allowed6_5 ? "Passed" : "Failed")}");

// Very large time gaps between requests
Console.WriteLine("Running code to test very large time gaps...");
(var allowed7_0, limiter) = AllowRequest(limiter, "user7", timestamp: 0);
(var allowed7_1, limiter) = AllowRequest(limiter, "user7", timestamp: 1000);
Console.WriteLine($"Debug GetBucketState user7: {GetBucketState(limiter, "user7")}");
Console.WriteLine($"Case 7.1: {(allowed7_1 ? "Passed" : "Failed")}");

// Helper methods
static LeakyBucketRateLimiter CreateRateLimiter(int capacity = 1, double leakRate = 1)
{
    return new LeakyBucketRateLimiter(capacity, leakRate);
}

static (bool, LeakyBucketRateLimiter) AllowRequest(LeakyBucketRateLimiter limiter, string userId, double timestamp)
{
    var (allowed, newLimiter) = limiter.HandleRequest(userId, timestamp);
    return (allowed, newLimiter);
}

static BucketInfo? GetBucketState(LeakyBucketRateLimiter limiter, string userId)
{
    return limiter.GetState(userId);
}
