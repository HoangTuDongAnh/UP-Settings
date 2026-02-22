# HTDA Framework – Settings

This package provides a **Game Settings** workflow for Unity:
- Runtime settings stored as ScriptableObject assets
- Editor window with modular pages
- Utilities: create assets, validation, import/export JSON

---

## Getting Started

### 1) Install
Install via Unity Package Manager (Git URL or local path).

### 2) Open the window
Menu: **HTDA → Settings → Game Settings**

### 3) Create default assets
Go to **Tools** page → click:
- **Create ALL default assets**

Assets are created under the default folder convention defined in:
- `SettingsEditorPaths.GetDefaultSettingsFolder()`

---

## Concepts

### Runtime vs Editor

- **Runtime** settings:
    - `Runtime/<Feature>/*SettingsAsset.cs`
    - Stored as ScriptableObject assets

- **Editor** pages:
    - `Editor/Core/*` (infrastructure)
    - `Editor/Pages/<Feature>/*Page.cs` (UI)

---

## Tutorials (Recommended Workflow)

Tutorials are defined as generic entries:
- `id`: unique tutorial key
- `triggerType`: OnGameStart, OnLevelStart, OnOpenUI, CustomEvent...
- `customEventKey` / `uiId`: required depending on trigger type
- `conditionKey`: optional filter interpreted by your game
- `messageKey` / `content`: used by UI layer
- `playOnce`: runtime should persist completion

This design allows reuse across most game genres.

---

## Validation

Some settings assets implement `IValidatableSettings`.
When the asset is edited in the window, validation warnings appear as a HelpBox.

Common checks:
- empty id
- duplicate id
- missing required fields (e.g., CustomEvent requires customEventKey)

---

## Import/Export JSON

Each settings asset supports JSON import/export using `EditorJsonUtility`.
This is useful for:
- sharing configs across projects
- backups
- quick iteration

---

## Troubleshooting

### “Type or namespace could not be found”
Check `.asmdef` references for the page assembly:
- The page asmdef must reference the runtime feature asmdef.
  Example:
- `HTDA.Framework.Settings.Editor.Ads` → references `HTDA.Framework.Settings.Ads`

### Multiple asmdef in one folder
Unity warns if one folder contains multiple asmdefs.
Ensure each folder contains only one `.asmdef`.