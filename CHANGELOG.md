# Changelog

## [Unreleased]

### Added
- Steady Pulse: while the Smartwatch is in your pocket, holding a monster never overcharges you. It wears per second of holding (`WearPercent`, default 1.5) after an optional `GraceSeconds` (default 0).
- `Debug.DebugMode` (off by default): puts the Smartwatch on the floor at the start of each level and writes detailed logs.

### Changed
- Runtime hygiene pass: holding timer reset at level end, debug log in the per-frame effect guarded.
