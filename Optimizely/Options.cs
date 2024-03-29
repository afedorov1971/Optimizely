﻿using CommandLine;

namespace Optimizely
{
	internal class Options
	{
		[Option('p', "poll", Default = false, HelpText = "Test polling mode")]
		public bool TestPolling { get; set; }

		[Option('k', "sdkKey", Required = true, HelpText = "Optimizely sdk key")]
		public string SdkKey { get; set; }

		[Option('f', "feature", Required = true, HelpText = "Optimizely feature toggle")]
		public string FeatureToggle { get; set; }
	}
}
