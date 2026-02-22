#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace HTDA.Framework.Settings.Editor.Core
{
    public static class SettingsEditorPaths
    {
        // Convention: Assets/__{ProductName}/Design/Settings
        public static string GetDefaultSettingsFolder()
        {
            var project = SanitizeName(PlayerSettings.productName);
            return $"Assets/__{project}/Design/Settings";
        }

        private static string SanitizeName(string s)
        {
            if (string.IsNullOrEmpty(s)) return "MyGame";
            return s.Replace("/", "_").Replace("\\", "_").Trim();
        }
    }
}
#endif