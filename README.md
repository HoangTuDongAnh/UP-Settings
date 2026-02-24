# UP-Settings (HTDA.Framework.Settings)

Bộ Settings/Config theo hướng Editor Window + ScriptableObject để quản lý cấu hình game/module (Ads/Audio/IAP/Global…).

## Thành phần

- Runtime settings modules (Core/Audio/Ads/IAP/Global/…): ScriptableObjects + data containers
- Editor:
    - Settings window
    - Pages system (attribute + page base)
    - Utilities (ReorderableList, paths…)

## Cách dùng (tổng quan)

1) Mở cửa sổ Settings trong Unity (menu của package).
2) Tạo/assign settings assets (ScriptableObject).
3) Truy cập runtime settings từ code game.

> Tuỳ cấu trúc project của bạn, Settings có thể được load trong bootstrap (SceneFlow) hoặc được reference trực tiếp.
