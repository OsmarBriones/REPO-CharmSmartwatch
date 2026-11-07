using CharmCore.Api;

namespace CharmSmartwatch
{
	/// <summary>Steady Pulse: holding a monster never overcharges you.</summary>
	internal sealed class SmartwatchCharm : Charm
	{
		internal const string PrefabName = "Valuable Arctic Smartwatch";

		internal SmartwatchCharm()
			: base("Smartwatch", "Smartwatch - Steady Pulse", defaultWearPercent: 1.5f, defaultCooldownSeconds: null, PrefabName)
		{
			// Extra host-synced numbers for this power go here, for example:
			// Amount = AddSetting("Amount", 30f, 1f, 100f, "Description for the config file.");
		}
	}
}
