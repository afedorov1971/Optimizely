using System;

namespace Optimizely.Client
{
	public class OptimizelyClientCreateParameters
	{
		public static readonly int DefaultPollingPeriodInSeconds = 30;

		public OptimizelyClientCreateParameters(string sdkKey)
		{
			if (string.IsNullOrEmpty(sdkKey))
			{
				throw new ArgumentException("sdkKey can not be null or empty", nameof(sdkKey));
			}
			PollingPeriodInSeconds = DefaultPollingPeriodInSeconds;
			SdkKey = sdkKey;
		}

		/// <summary>
		/// Specify the polling period to get dataFile
		/// </summary>
		/// <param name="pollingPeriodInSeconds"></param>
		/// <returns></returns>
		public OptimizelyClientCreateParameters WithPollingPeriod(int pollingPeriodInSeconds)
		{
			if (pollingPeriodInSeconds <= 0)
			{
				throw new ArgumentException("Polling period should be positive", nameof(pollingPeriodInSeconds));
			}

			PollingPeriodInSeconds = pollingPeriodInSeconds;
			return this;
		}

		/// <summary>
		/// Polling period to get dataFile in seconds
		/// </summary>
		public int PollingPeriodInSeconds { get; private set; }

		/// <summary>
		/// Sdk key user to compose url to get dataFile
		/// </summary>
		public string SdkKey { get; }
	}
}
