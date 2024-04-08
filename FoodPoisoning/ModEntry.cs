using StardewModdingAPI;
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
			configHelper.AddSetting("duration", () => Config.BaseDuration, min: 10, max: 240, interval: 2);
			configHelper.AddSetting("harmful_edibility_threshold", () => Config.HarmfulThreshold, min: -300, max: 100, interval: 5);
			configHelper.AddSetting("harmful_chance_offset", () => Config.HarmfulChanceOffset);
			configHelper.AddSetting("harmful_duration_offset", () => Config.HarmfulDurationOffset, min: 10, max: 120, interval: 2);
		}
	}
}