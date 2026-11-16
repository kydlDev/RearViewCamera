# Changelog

## 2.0.1

- Changed: the menus of this mod take their text sizes, text colours and font from KydlCore. Needs KydlCore 1.1.1 or newer.
- Changed: all the **Enable** switches are now together in `[General]` and have new names: `Enable mod` (was `EnableAll`). The values you had set are taken over; the old entries are removed from your config file.

### Testing
- Unit tests: 98 passed. Integration tests: 32 passed.
- Property-based tests (FsCheck, included in the unit tests above): 5 property checks with generated inputs.
- Coverage of the testable code: 100%. Mutation testing (Stryker): 98.3% of the mutants are caught by the tests.
- Performance benchmarks (BenchmarkDotNet): 5.
- Static analysis (SonarQube): no Medium or High issues.
- In game with DrakesTestLab: the full suite of 5 tests per run, run 3 times in a row on the release build, all green (the release requirement).

## 2.0.0
- Tested in game with the full TestLab suite (5 tests): 3 green runs in a row on 2026-11-05, plus the ecosystem run with all 12 KydlDev mods loaded together.
- Version check with servers: you can still join any server without RearViewCamera; when the server has it too, both must have exactly the same version.
- KydlCore hub (F6): the mod shows as "Rear View Camera" with its Settings tab.
- Internal rebuild on the KydlDev standard (no change to the keys, settings or the picture). Checked in game on 2026-11-04 (clean start and quit with all mods loaded, no errors from this mod).
- Breaking: RearViewCamera now needs the KydlCore mod (hard dependency). Install KydlCore as well.
- The rear-view window and its notices are drawn on KydlCore's one UI panel; the mod has no panel, log hook or input lock of its own any more.
- All settings are on the automatic Settings tab of RearViewCamera in the KY hub window (F6).
- `[General] EnableAll` and `[DebugLog] Enable` as before.

## 1.0.0
- First release.
- Small rear-view window under the minimap (16:9, 640x360 to 960x540, default 800x450): key N, toggle or hold.
- Full-screen rear view while J is held, rendered at up to 1080p; your character keeps facing forward.
- Status-effect icons: the window goes under them, behind them or on top of them, following where they really are.
- Hidden while the inventory, the menu, a loading screen or the hidden HUD is up, back afterwards; on the large map it can only be closed.
- Keys do nothing while chat, the console or a text field is open.
- The rear camera uses the game camera's settings and colour grading.
- The camera rises over a creature standing right behind you.
- Key check against the game and other mods, with Home / End as backup keys and messages about the conflict.
- Settings apply at once while playing; nothing is rendered while the rear view is hidden.
- Safety net: after errors the rear view steps down to simpler rendering and finally switches off until restart, with a note.
