using HarmonyLib;

namespace CharmSmartwatch.Patches
{
	/// <summary>
	/// Steady Pulse: skips the overcharge a grabbed monster adds to the local player. The game only applies it when the
	/// monster is grabbed by the local player (<c>grabbedLocal</c>), so this runs on the owner's machine only.
	/// </summary>
	[HarmonyPatch(typeof(SemiFunc), nameof(SemiFunc.EnemyOvercharge))]
	internal static class SemiFunc_EnemyOvercharge_Patch
	{
		[HarmonyPrefix]
		private static bool Prefix(PhysGrabObject _physGrabObject)
		{
			SmartwatchCharm? charm = CharmSmartwatchPlugin.Charm;
			if (charm == null || !charm.IsActive || _physGrabObject == null || !_physGrabObject.grabbedLocal)
			{
				return true;
			}

			return false;
		}
	}
}
