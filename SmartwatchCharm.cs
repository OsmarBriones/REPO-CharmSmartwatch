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
			// Wear is per second of holding a monster, not per use.
			WearsPerSecond = true;
			GraceSeconds = AddSetting("GraceSeconds", 0f, 0f, 30f, "Seconds you must keep holding a monster before the Smartwatch starts to wear. 0 wears from the first moment.");
		}

		/// <summary>Seconds of continuous holding before wear starts.</summary>
		internal CharmSetting GraceSeconds { get; }

		/// <summary>The effect component on the plugin's own persistent object (not a level object); set by the plugin.</summary>
		internal SteadyPulse? Pulse { get; set; }

		protected override void OnLevelEnded()
		{
			Pulse?.ResetState();
		}
	}
}
