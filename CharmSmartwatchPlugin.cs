using BepInEx;
using HarmonyLib;
using CharmCore.Api;
using UnityEngine;

namespace CharmSmartwatch
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency("com.osmar.Pockets", BepInDependency.DependencyFlags.HardDependency)]
	[BepInDependency("com.osmar.CharmCore", BepInDependency.DependencyFlags.HardDependency)]
	public class CharmSmartwatchPlugin : BaseUnityPlugin
	{
		public const string PluginGuid = "com.osmar.CharmSmartwatch";
		public const string PluginName = "CharmSmartwatch";
		public const string PluginVersion = "1.0.0";

		internal static SmartwatchCharm? Charm { get; private set; }

		internal static new BepInEx.Logging.ManualLogSource Logger { get; private set; } = null!;

		private void Awake()
		{
			Logger = base.Logger;

			// Prevent the plugin from being deleted
			this.gameObject.transform.parent = null;
			this.gameObject.hideFlags = HideFlags.HideAndDontSave;

			// Registering binds Enabled, WearPercent, GraceSeconds and the Debug section (DebugMode) in this mod's config file.
			SmartwatchCharm charm = new SmartwatchCharm();
			Charm = charm;
			CharmsApi.Register(charm, Config);
			new Harmony(PluginGuid).PatchAll();
			SteadyPulse pulse = this.gameObject.AddComponent<SteadyPulse>();
			pulse.Charm = charm;
			charm.Pulse = pulse;

			Logger.LogInfo($"{PluginName} {PluginVersion} loaded!");
		}
	}
}
