namespace FoodPoisoning
{
	public sealed class ModConfig
	{
		public float BasePoisoningChance { get; set; } = 100f;
		public float BaseDuration { get; set; } = 120f;
		public float HarmfulChanceOffset { get; set; } = 50f;
		public float HarmfulDurationOffset { get; set; } = 2f;
	}
}