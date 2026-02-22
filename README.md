# UP-Settings (HTDA Framework Settings)

A lightweight, modular **Game Settings** package for Unity (UPM), providing:
- A Settings window (Editor) with pages (Global, Audio, Support Items, Tutorials, Ads, IAP, Credits, Databases...)
- Runtime ScriptableObject assets for data
- Editor tooling: create/locate assets, reset, import/export JSON, delete assets, validation warnings

---

## Install (UPM)
Add the package via Git URL or local path:
- `Packages/manifest.json` → `dependencies`:
```json
{
  "com.htda.framework.settings": "https://github.com/<YOUR_ORG>/UP-Settings.git#main"
}
```

---

## Open the Settings Window

In Unity:

- Menu: HTDA → Settings → Game Settings

---

## Default Asset Location

By default, settings assets are created/located under the folder convention implemented by:

- `SettingsEditorPaths.GetDefaultSettingsFolder()`

Example convention:

- `Assets/__<ProjectName>/Design/Settings/`

---

## Pages

Pages are placed under:

- `Editor/Pages/<Feature>/...`

Core editor infrastructure is placed under:

- `Editor/Core/...`

Typical pages:

- Tools: create assets, export/import JSON, clear data

- Global: common flags, economy ranges, feature flags

- Support Items: boosters/powerups database

- Content DB: generic id → prefab/icon database

- Audio: audio items database

- Tutorials: event-driven tutorial definitions

- Ads / IAP / Credits: optional settings modules

---

## Tutorials (Genre-Agnostic Design)

Tutorials are defined as event-driven entries:

- `triggerType`: OnGameStart / OnLevelStart / OnOpenUI / CustomEvent ...

- `conditionKey`: optional string interpreted by your game

- `messageKey` / `content`: used by UI layer

- `playOnce`: runtime should persist completion

This design avoids hardcoding specific game genres (puzzle, RPG, action...).

---

## JSON Import/Export

Each settings asset can be exported/imported using `EditorJsonUtility`.
This is intended for:

- sharing configs between projects

- backup / restore

- quick iteration

---

## Clear Game Data

Default implementation clears:

- `PlayerPrefs.DeleteAll()`

You can later replace this action with your Save/Core service.

---

## Development Notes

- Keep ids stable (lowercase_with_underscores recommended).

- Each `Editor/Pages/<Feature>` folder should contain only one asmdef.

---

## License
[LICENSE.md](LICENSE.md)