namespace FoodPoisoning
{
	public sealed class ModConfig
	{
		public int BasePoisoningChance { get; set; } = 30;
		public int BaseDuration { get; set; } = 120;
		public int HarmfulThreshold { get; set; } = 0;
		public int HarmfulChanceOffset { get; set; } = 70;
		public int HarmfulDurationOffset { get; set; } = 60;
	}
}