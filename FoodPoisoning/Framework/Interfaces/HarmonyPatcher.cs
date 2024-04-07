using System;
using StardewModdingAPI;
using StardewValley;
using SObject = StardewValley.Object;
using FoodPoisoning.Common;

namespace FoodPoisoning.Interfaces
{
	internal class HarmonyPatcher
	{
		private static IMonitor Monitor { get; set; } = null!;

		internal static void InitialiseMonitor(IMonitor monitorInstance)
		{
			Monitor = monitorInstance;
		}

		internal static void DoneEating_PostFix(Farmer __instance)
		{
			try
			{
				var foodItem = __instance.itemToEat;

				if (foodItem is null)
				{
					return;
				}

				Utilities.UpdateFoodConsumption((SObject)foodItem);
			}
			catch (Exception ex)
			{
				Monitor.Log($"[Food Poisoning] Method patch failed! \nReason: {ex}", LogLevel.Error);
			}
		}
	}
}
