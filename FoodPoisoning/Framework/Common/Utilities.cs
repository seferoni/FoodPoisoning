using System;
using System.Collections.Generic;
using System.Threading.Channels;
using StardewValley;
using SObject = StardewValley.Object;

namespace FoodPoisoning.Common
{
	internal sealed class Utilities
	{
		internal const string nauseatedID = "25";
		private const int conversionFactor = 1000; 
		internal static void ApplyNauseated(SObject foodObject)
		{
			int newDuration = (int)GetDurationByFood(foodObject) * conversionFactor;

			Buff nauseated = new(nauseatedID)
			{
				millisecondsDuration = newDuration,
				glow = Microsoft.Xna.Framework.Color.White
			};

			Game1.player.buffs.Apply(nauseated);
		}

		internal static float GetDurationByFood(SObject foodObject)
		{
			var duration = ModEntry.Config.BaseDuration;

			if (IsFoodHarmful(foodObject))
			{
				duration += ModEntry.Config.HarmfulDurationOffset;
			}

			return duration;
		}

		internal static float GetPercentageChanceByFood(SObject foodObject)
		{
			var chance = ModEntry.Config.BasePoisoningChance;

			if (IsFoodHarmful(foodObject))
			{
				chance += ModEntry.Config.HarmfulChanceOffset;
			}

			return Math.Min(chance, 100f);
		}

		internal static bool IsFoodHarmful(SObject foodObject)
		{
			if (foodObject.Edibility >= 0)
			{
				return false;
			}

			return true;
		}

		internal static bool IsFoodViable(SObject foodObject)
		{
			if (foodObject.Category == SObject.CookingCategory)
			{
				return true;
			}

			if (foodObject.Category == SObject.artisanGoodsCategory)
			{
				return true;
			}

			return false;
		}

		internal static void UpdateFoodConsumption(SObject foodObject)
		{
			Random randomiserInstance = new();

			if (IsFoodViable(foodObject))
			{
				return;
			}

			if (randomiserInstance.Next(1, 100) > GetPercentageChanceByFood(foodObject))
			{
				return;
			}

			ApplyNauseated(foodObject);
		}
	}
}
