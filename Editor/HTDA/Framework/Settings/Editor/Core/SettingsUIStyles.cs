#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace HTDA.Framework.Settings.Editor.Core
{
    public static class SettingsUIStyles
    {
        private static GUIStyle _sidebarItem;
        private static GUIStyle _sidebarItemSelected;
        private static GUIStyle _headerTitle;
        private static GUIStyle _sectionHeader;

        public static GUIStyle SidebarItem
            => _sidebarItem ??= new GUIStyle(EditorStyles.label)
            {
                padding = new RectOffset(10, 8, 6, 6),
                fontSize = 12
            };

        public static GUIStyle SidebarItemSelected
            => _sidebarItemSelected ??= new GUIStyle(SidebarItem)
            {
                normal = { textColor = Color.white },
                fontStyle = FontStyle.Bold
            };

        public static GUIStyle HeaderTitle
            => _headerTitle ??= new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 14,
                alignment = TextAnchor.MiddleCenter
            };

        public static GUIStyle SectionHeader
            => _sectionHeader ??= new GUIStyle(EditorStyles.foldoutHeader)
            {
                fontStyle = FontStyle.Bold
            };

        public static Color HeaderBarColor => new Color(0.93f, 0.62f, 0.10f);   // orange-ish
        public static Color SidebarTopColor => new Color(0.18f, 0.55f, 0.16f); // green-ish
        public static Color SidebarSelectedBg => new Color(0.18f, 0.35f, 0.70f);
        public static Color SidebarBg => new Color(0.20f, 0.20f, 0.20f);
        public static Color ContentBg => new Color(0.22f, 0.22f, 0.22f);
    }
}
#endif