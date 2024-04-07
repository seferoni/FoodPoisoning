using StardewModdingAPI;
using FoodPoisoning.Common;
using FoodPoisoning.Interfaces;
using StardewModdingAPI.Events;
using HarmonyLib;
using StardewValley;

namespace FoodPoisoning
{
	internal sealed class ModEntry : Mod
	{
		internal static ModConfig Config { get; set; } = null!;
		public override void Entry(IModHelper helper)
		{
			Config = Helper.ReadConfig<ModConfig>();
			Harmony harmonyInstance = new(ModManifest.UniqueID);

			harmonyInstance.Patch(
			   original: AccessTools.Method(typeof(Farmer), nameof(Farmer.doneEating)),
			   postfix: new HarmonyMethod(typeof(HarmonyPatcher), nameof(HarmonyPatcher.DoneEating_PostFix))
			);

			helper.Events.GameLoop.GameLaunched += GameLaunched;
		}

		private void GameLaunched(object? sender, GameLaunchedEventArgs e)
		{
			SetupConfig();
		}

		private void SetupConfig()
		{
			var api = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");

			if (api is null)
			{
				return;
			}

			ConfigHelper configHelper = new(api, ModManifest, Helper.Translation, Config);

			api.Register(
				mod: ModManifest,
				reset: () => Config = new(),
				save: () => Helper.WriteConfig(Config)
			);

			api.AddSectionTitle(
				mod: ModManifest,
				text: () => Helper.Translation.Get("title")
			);

			configHelper.AddSetting("base_poisoning_chance", () => Config.BasePoisoningChance);
			configHelper.AddSetting("duration", () => Config.BaseDuration, min: 10f, max: 240f, interval: 2f);
		}
	}
}