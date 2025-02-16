using System;
using System.Collections.Concurrent;
using System.Threading.RateLimiting;
using System.Threading.Tasks;

public class LoginRateLimiter
{


	private static readonly ConcurrentDictionary<string, FixedWindowRateLimiter> _limiters = new();


	public static FixedWindowRateLimiter GetUserLimiter(string userKey)
	{
		return _limiters.GetOrAdd(userKey, _ => new FixedWindowRateLimiter(new FixedWindowRateLimiterOptions { PermitLimit = 4, Window = TimeSpan.FromMinutes(1), QueueProcessingOrder = QueueProcessingOrder.OldestFirst, QueueLimit = 0 }));
	}

	public static async Task<bool> CanAttemptSignInAsync(string email)
	{
		var rateLimiter = GetUserLimiter("login." + email);
		RateLimitLease lease = await rateLimiter.AcquireAsync(1);
		return lease.IsAcquired;
	}
}
