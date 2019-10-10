using System;

namespace Optimizely.Client
{
	public interface IOptimizely : IDisposable
	{
		bool IsFeatureEnabled(OptimizelyFeatureParameters featureParameters);
	}
}
