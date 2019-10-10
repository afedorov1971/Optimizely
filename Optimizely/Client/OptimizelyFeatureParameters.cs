using System;
using System.Linq;
using OptimizelySDK.Entity;

namespace Optimizely.Client
{
	public class OptimizelyFeatureParameters
	{
		public OptimizelyFeatureParameters(string featureKey)
		{
			if (string.IsNullOrEmpty(featureKey))
			{
				throw new ArgumentException("Feature key can not be null or empty", nameof(featureKey));
			}
			UserId = string.Empty;
			Attributes = new UserAttributes();
			FeatureKey = featureKey;
		}

		public OptimizelyFeatureParameters ForUser(string userId)
		{
			UserId = userId ?? throw new ArgumentNullException(nameof(userId));

			return this;
		}

		public OptimizelyFeatureParameters WithArgument(string argumentKey, object argumentValue)
		{
			if (string.IsNullOrEmpty(argumentKey))
			{
				throw new ArgumentException("Argument key can not be null or empty", nameof(argumentKey));
			}

			Attributes[argumentKey] = argumentValue;
			return this;
		}

		internal UserAttributes Attributes { get; }
		public string UserId { get; private set; }

		public string FeatureKey { get; }

		private string GetAttributesAsString()
		{
			if (Attributes.Count == 0)
			{
				return "Not used";
			}

			var atributes = string.Join(",", Attributes.Select(a => a.ToString()));

			return $"[{atributes}]";

		}
		public override string ToString()
		{
			var userId = UserId == string.Empty ? "Not specified" : UserId;
			return $"Feature key is {FeatureKey}, UserId is {userId}, Attributes {GetAttributesAsString()}";
		}
	}
}
