﻿using System;
using StardewModdingAPI;

namespace EnergyRework.Interfaces
{
	public interface IGenericModConfigMenuApi
	{
		void Register(IManifest mod, Action reset, Action save, bool titleScreenOnly = false);
		void AddSectionTitle(IManifest mod, Func<string> text, Func<string>? tooltip = null);
		void AddNumberOption(IManifest mod, Func<float> getValue, Action<float> setValue, Func<string> name, Func<string>? tooltip = null, float? min = null, float? max = null, float? interval = null, Func<float, string>? formatValue = null, string? fieldId = null);
		void Unregister(IManifest mod);
	}
}