using System;
using System.Linq.Expressions;
using System.Reflection;
using StardewModdingAPI;

namespace EnergyRework.Interfaces
{
	internal sealed class ConfigHelper
	{
		internal IGenericModConfigMenuApi API { get; set; }
		internal IManifest Mod { get; set; }
		internal ITranslationHelper TranslationHelper { get; set; }
		internal ModConfig Config { get; set; }

		internal ConfigHelper(IGenericModConfigMenuApi apiInstance, IManifest modManifest, ITranslationHelper translationHelperInstance, ModConfig config)
		{
			API = apiInstance;
			Mod = modManifest;
			TranslationHelper = translationHelperInstance;
			Config = config;
		}

		internal void AddSetting(string key, Expression<Func<float>> getter, float min = -15f, float max = 0f, float interval = 1f)
		{
			Func<float> newGetter = getter.Compile()!;
			Action<float> newSetter = GetSetterFromGetter(getter);

			API.AddNumberOption(
				mod: Mod,
				name: () => TranslationHelper.Get(key),
				tooltip: () => TranslationHelper.Get(string.Concat(key, ".tooltip")),
				getValue: newGetter,
				setValue: newSetter,
				min: min,
				max: max,
				interval: interval
			);
		}

		internal Action<T> GetSetterFromGetter<T>(Expression<Func<T>> getter)
		{
			var property = (PropertyInfo)((MemberExpression)getter.Body).Member;
			var action = (T value) => property.SetValue(Config, value);
			return action;
		}
	}
}
