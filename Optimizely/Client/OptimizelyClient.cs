using System;
using OptimizelySDK.Config;

namespace Optimizely.Client
{
	public class OptimizelyClient : IOptimizely
		{
			#region fields

			private volatile bool _isDisposed;

			private readonly object _disposeLock = new object();

			private readonly OptimizelySDK.Optimizely _optimizely;

			#endregion

			#region .ctor

			private OptimizelyClient(OptimizelySDK.Optimizely optimizely)
			{
				_optimizely = optimizely;
			}

			~OptimizelyClient()
			{
				Dispose(false);
			}

			#endregion

			#region IOptimizely members

			bool IOptimizely.IsFeatureEnabled(OptimizelyFeatureParameters featureParameters)
			{
				if (featureParameters == null)
				{
					throw new ArgumentNullException(nameof(featureParameters));
				}

				return _optimizely.IsFeatureEnabled(featureParameters.FeatureKey, featureParameters.UserId, featureParameters.Attributes);
			}

			#endregion


			#region IDisposable members

			public void Dispose()
			{
				Dispose(true);
				GC.SuppressFinalize(this);
			}

			protected virtual void Dispose(bool isDisposing)
			{
				if (!isDisposing || _isDisposed)
				{
					return;
				}

				lock (_disposeLock)
				{
					if (_isDisposed)
					{
						return;
					}

					_optimizely?.Dispose();
					_isDisposed = true;
				}
			}

			#endregion


			#region helpers to create a client

			/// <summary>
			/// Create optimizely client from dataFile
			/// </summary>
			/// <param name="dataFile"></param>
			/// <returns></returns>
			public static IOptimizely CreateFromDataFile(string dataFile)
			{
				var optimizely = new OptimizelySDK.Optimizely(dataFile);

				return new OptimizelyClient(optimizely);
			}

			/// <summary>
			/// Create optimizely client by set of parameters. Datafile will be updated from optimizely server 
			/// </summary>
			/// <param name="createParameters"></param>
			/// <returns></returns>
			public static IOptimizely Create(OptimizelyClientCreateParameters createParameters)
			{
				if (createParameters == null)
				{
					throw new ArgumentNullException(nameof(createParameters));
				}

				var builder = new HttpProjectConfigManager.Builder()
					.WithSdkKey(createParameters.SdkKey)
					.WithStartByDefault();

				var projectConfigManager = builder
						.WithPollingInterval(TimeSpan.FromSeconds(createParameters.PollingPeriodInSeconds))
						.Build();

				var optimizely = new OptimizelySDK.Optimizely(projectConfigManager);

				return new OptimizelyClient(optimizely);
			}

			#endregion

		}
}
