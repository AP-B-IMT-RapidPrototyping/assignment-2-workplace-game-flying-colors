Flying Colors — Security Game

This repository contains a lightweight Godot/C# prototype used for an assignment.

Status

- Playable prototype with movement, interaction, and basic game flow.

Quick notes

- Open the `security-game` folder in Godot 4.6.
- Main scenes are under `security-game/scenes/`.
- Player logic lives in `scenes/Shengyan/PlayerController.cs`, `PlayerInteractor.cs`, and `PlayerStats.cs`.

Notes

- Jumping is omitted as a deliberate design choice.
- Missing audio fallbacks may log warnings when assets are absent.

Developer TODOs

- Add persistence for player stats and smoothing for energy changes.
- Review raycast and world null checks in `PlayerInteractor.cs`.

To run the project, load `security-game/project.godot` in Godot 4.6. The README focuses on entry points and developer TODOs rather than exhaustive build steps.
