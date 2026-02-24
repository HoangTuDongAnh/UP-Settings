# UP-Settings – Documentation

## 1. Mục tiêu

- Chuẩn hoá nơi lưu trữ cấu hình game.
- Editor UI để chỉnh sửa dễ, giảm hardcode.

## 2. Runtime vs Editor

- Runtime asmdef: chứa data/settings dùng khi chạy game.
- Editor asmdef: chứa UI window, pages, tool.

## 3. Page system

- Mỗi page tương ứng 1 nhóm settings (Ads/Audio/IAP/Global…)
- Page được đăng ký qua attribute (SettingsPageAttribute).

Khuyến nghị:
- Mỗi module settings có 1 ScriptableObject root.
- Không nhét logic runtime vào Editor assembly.

## 4. Workflow gợi ý

- Tạo folder `Assets/_Game/Settings`
- Tạo assets cho từng nhóm
- Bootstrap load/validate, hoặc game systems nhận reference.

## 5. Notes

- Khi thêm page mới: tạo asmdef editor page (nếu bạn đang tách theo page) và đảm bảo references đúng.
