using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CommandLine;
using EP.Core.Optimizely;

namespace Optimizely
{
	internal static class Program
	{
		private static void TestSimpleWithDataFile(string sdkKey, string featureKey)
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
					var enabled1 = oc.IsFeatureEnabled(new OptimizelyFeatureParameters(featureKey).WithArgument("IsLocal", true));

					var enabled2 = oc.IsFeatureEnabled(new OptimizelyFeatureParameters(featureKey).WithArgument("IsLocal", false));

					Console.WriteLine($"{featureKey} audience local is {enabled1}");

					Console.WriteLine($"{featureKey} audience not local is {enabled2}");
				}

				Thread.Sleep(5000);

			} while (true);
		}


		private static void TestWithUpdater(string sdkKey, string featureKey)
		{
			var cfg = OptimizelyClient.Create(new OptimizelyClientCreateParameters(sdkKey).WithPollingPeriod(5));

			do
			{
				Thread.Sleep(5000);

				// Evaluate a feature flag and a variable
				var enabled1 = cfg.IsFeatureEnabled(new OptimizelyFeatureParameters(featureKey).WithArgument("IsLocal", true));

				Console.WriteLine($"{featureKey} audience local is {enabled1}");
				
			} while (true);
		}

		private static void RunWithOptions(Options opt)
		{
			if (opt.TestPolling)
			{
				TestWithUpdater(opt.SdkKey, opt.FeatureToggle);	
			}
			else
			{
				TestSimpleWithDataFile(opt.SdkKey, opt.FeatureToggle);
			}
		}

		private static void Main(string[] args)
		{
			Parser.Default.ParseArguments<Options>(args).WithParsed(RunWithOptions);
		}
	}
}
