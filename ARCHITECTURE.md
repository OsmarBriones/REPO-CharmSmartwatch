# CharmSmartwatch Architecture

A Charm mod (layer 3 of the PocketCharms family): one valuable, one power. It depends on **Pockets** (storage) and **CharmCore** (registry, wear, cooldown display, host settings, debug mode).

## The power: Steady Pulse

While the Smartwatch is in the owner's pocket, the owner never suffers overcharge while holding a monster. Local effect, nothing is sent over the network.

- `SmartwatchCharm : Charm` registers the prefab name `Valuable Arctic Smartwatch`, wear 1.5% **per second** (`WearsPerSecond = true`, no cooldown) and the `GraceSeconds` setting.
- `Patches/SemiFunc_EnemyOvercharge_Patch` (Harmony prefix on `SemiFunc.EnemyOvercharge`) returns false while `Charm.IsActive` and the grabbed object is `grabbedLocal`. The game calls it only from `EnemyRigidbody.PhysGrabOverCharge` (and `EnemyBombThrowerHead`), and it only adds overcharge for the local grabber, so the prefix runs on the owner's machine and skips the call entirely.
- `SteadyPulse` (a `MonoBehaviour` on the plugin object) decides when the power "works": `PhysGrabber.instance.grabbed` with a `grabbedPhysGrabObject` that has an `enemyRigidbody`, on a level where overcharge exists. After `GraceSeconds` of continuous holding it calls `Charm.WearForSeconds(Time.deltaTime)`.

## Config

| Key | Type | Range | Default | Meaning |
|---|---|---|---|---|
| `WearPercent` | float, % per second | 0-100 | 1.5 | Share of the original value lost per second of holding a monster. |
| `GraceSeconds` | float, seconds | 0-30 | 0 | Continuous holding time before wear starts. 0 is meaningful (wear from the first moment); negative makes no sense. |

## Game members used

Checked by `CharmSmartwatch.Tests/GameContractTests.cs`:

- `SemiFunc.EnemyOvercharge(PhysGrabObject, EnemyParent.Difficulty, float)` (patched), `SemiFunc.MoonLevel()`
- `PhysGrabObject.grabbedLocal`, `PhysGrabObject.enemyRigidbody`
- `PhysGrabber.instance`, `PhysGrabber.grabbed`, `PhysGrabber.grabbedPhysGrabObject`

## Decisions

- Overcharge is blocked by skipping `SemiFunc.EnemyOvercharge` rather than patching `PhysGrabber.PhysGrabOverCharge`, which other sources (for example items) also use; Steady Pulse is specifically about monsters.
- Overcharge only exists from moon level 2 on (`PhysGrabber.PhysGrabOverCharge` checks `SemiFunc.MoonLevel() >= 2`). Below that the power does nothing, so it also does not wear there (the wear check uses the same condition).
- Holding a monster that is not the one being overcharged (for example a monster held by two players) is still "holding a monster" for wear; the game divides overcharge among holders, and we do not try to model that.
- `GraceSeconds` lets short grabs be free. Default 0 keeps the simple behaviour.

## Debug mode

Provided by CharmCore: the `Debug.DebugMode` setting (off by default) places the valuable on the floor at the start of each level and enables `Charm.LogDebug` output.

## Known limits

- Looked up by exact prefab name; a game update that renames the prefab needs a mod update.
- Only prevents overcharge from grabbed monsters, not other overcharge sources.
- Not verified in game (code and build only).

## Runtime hygiene

- **References and level state:** fixed. `SteadyPulse` remembers the holding time and a logged flag; `SmartwatchCharm` now overrides `OnLevelEnded` and resets them through `SteadyPulse.ResetState()`. The charm holds the component only through an instance property; the component lives on the plugin's own persistent object, not on a level object, and no static field holds a game object.
- **Per-frame work:** `SteadyPulse.Update` returns early when the charm is inactive or nothing is held; the check reads the game's own `PhysGrabber.instance` and allocates nothing. The `SemiFunc.EnemyOvercharge` prefix exits on the first checks when the charm is inactive and allocates nothing.
- **Debug logs:** fixed. The `LogDebug` in `Update` (already logged once per hold) is now guarded with `if (Charm.DebugMode)`.
- **Changes to the game:** nothing needed. The mod only skips the game's overcharge call while active; it writes nothing to the game, so there is nothing to restore.
- **Static game objects and Find calls:** none.
- Known limit outside this mod: while the charm is active, CharmCore's `IsActive` and `CharmSetting.Value` build their host-setting key strings on each call; that is CharmCore's code and was left alone.
