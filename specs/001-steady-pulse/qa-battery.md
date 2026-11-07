# QA battery: Steady Pulse (CharmSmartwatch)

Deploy: Pockets, CharmCore and CharmSmartwatch are in the r2modman `Debug` profile (compare hashes with the builds). Restart the game first.

## Quick start with debug mode
Set `DebugMode = true` in the `Debug` section of `BepInEx/config/com.osmar.CharmSmartwatch.cfg` (off by default). When a level starts, a Smartwatch is placed on the floor about 2.5 m in front of you; the log then also shows the detailed lines. Any level works. To get another one at any moment, set `SpawnNow = true` in the same section while a level is running: the valuable is placed near you and the switch turns itself back off (the file is re-read about once a second).

## Single player
- [ ] Log shows `Charm registered: Smartwatch - Steady Pulse` and `CharmSmartwatch 1.0.0 loaded!`.
- [ ] Store the valuable in a slot: the slot shows its picture and value.
- [ ] On a level with overcharge (moon level 2 or higher), without the Smartwatch: hold a monster and see the overcharge build up.
- [ ] Store the Smartwatch and repeat: no overcharge, the grab beam never overloads.
- [ ] While holding the monster, the Smartwatch value drops about 1.5% of its original value per second; releasing the monster stops the wear.
- [ ] Holding a normal valuable or a cart does not wear the Smartwatch.
- [ ] On an early level (below moon level 2) holding a monster does not wear the Smartwatch.
- [ ] Take the valuable out: the power stops.
- [ ] Use it until the valuable breaks: the slot empties, the power stops, no errors in the log.

## Settings
- [ ] `WearPercent = 0`: no wear. `GraceSeconds = 5`: no wear during the first 5 seconds of each hold, and the timer restarts after releasing.
- [ ] Charm `Enabled = false`: no effect and no wear. CharmCore `Enabled = false`: nothing works.

## Multiplayer (host and client)
- [ ] On a client carrying the Smartwatch the overcharge is blocked and the value drops on both machines.
- [ ] Host settings override the client's.
- [ ] Client leaves and rejoins: no stuck veil.

Log patterns: `Charm registered`, `threw`, `Exception`.
