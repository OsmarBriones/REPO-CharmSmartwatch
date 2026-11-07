using UnityEngine;

namespace CharmSmartwatch
{
	/// <summary>
	/// Steady Pulse: while the owner holds a monster, the Smartwatch wears per second. The overcharge itself is skipped by
	/// <see cref="Patches.SemiFunc_EnemyOvercharge_Patch"/>; this component only decides when the power is "working" so the
	/// wear matches. Everything is local to the owner, so no network is needed.
	/// </summary>
	internal sealed class SteadyPulse : MonoBehaviour
	{
		/// <summary>Overcharge only exists from this moon level on (see PhysGrabber.PhysGrabOverCharge).</summary>
		private const int FirstOverchargeMoonLevel = 2;

		private float heldSeconds;
		private bool logged;

		internal SmartwatchCharm Charm { get; set; } = null!;

		private void Update()
		{
			if (Charm == null || !Charm.IsActive || !IsHoldingMonster())
			{
				heldSeconds = 0f;
				logged = false;
				return;
			}

			heldSeconds += Time.deltaTime;
			if (heldSeconds < Charm.GraceSeconds.Value)
			{
				return;
			}

			Charm.WearForSeconds(Time.deltaTime);
			if (!logged)
			{
				logged = true;
				Charm.LogDebug($"Steady Pulse: holding a monster, overcharge blocked and wear running ({Charm.WearPercent:0.##}%/s)");
			}
		}

		private static bool IsHoldingMonster()
		{
			PhysGrabber? grabber = PhysGrabber.instance;
			if (grabber == null || !grabber.grabbed || grabber.grabbedPhysGrabObject == null || grabber.grabbedPhysGrabObject.enemyRigidbody == null)
			{
				return false;
			}

			// Below this level the game never overcharges, so the power has nothing to block and must not wear.
			return SemiFunc.MoonLevel() >= FirstOverchargeMoonLevel;
		}
	}
}
