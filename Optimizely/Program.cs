using System;
using System.Threading;
using EP.Core.Optimizely;

namespace Optimizely
{
	internal class Program
	{
		static void TestSimpleWithDataFile(string sdkKey)
		{
			var url = $"https://cdn.optimizely.com/datafiles/{sdkKey}.json";
			
			do
			{
				string datafile;
				using (var webClient = new System.Net.WebClient())
				{

					// To refresh on every request
					webClient.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
					datafile = webClient.DownloadString(url);
				}

				using (var oc = OptimizelyClient.CreateFromDataFile(datafile))
				{

					// Evaluate a feature flag and a variable
					var enabled1 = oc.IsFeatureEnabled(new OptimizelyFeatureParameters("test_feature").WithArgument("IsLocal", true));


					var enabled2 = oc.IsFeatureEnabled(new OptimizelyFeatureParameters("test_feature").WithArgument("IsLocal", false));

					Console.WriteLine($"test_feature audience local is {enabled1}");

					Console.WriteLine($"test_feature audience not local is {enabled2}");
				}

				Thread.Sleep(5000);

			} while (true);
		}


		static void TestWithUpdater(string sdkKey)
		{
			var cfg = OptimizelyClient.Create(new OptimizelyClientCreateParameters(sdkKey).WithPollingPeriod(5));

			do
			{
				Thread.Sleep(5000);

				// Evaluate a feature flag and a variable
				var enabled1 = cfg.IsFeatureEnabled(new OptimizelyFeatureParameters("test_feature").WithArgument("IsLocal", true));

				Console.WriteLine($"test_feature audience local is {enabled1}");
				
			} while (true);
		}
		
		static void Main(string[] args)
		{
			if (args.Length < 1)
			{
				Console.WriteLine("Optimizely sdk key is required");
			}
			var sdkKey = args[0];

			var usePolling = args.Length > 1 && args[1] == "poll";

			if (usePolling)
			{
				TestWithUpdater(sdkKey);	
			}
			else
			{
				TestSimpleWithDataFile(sdkKey);
			}
		}
	}
}
