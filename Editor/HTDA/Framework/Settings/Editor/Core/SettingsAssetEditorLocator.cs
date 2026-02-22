#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace HTDA.Framework.Settings.Editor.Core
{
    public static class SettingsAssetEditorLocator
    {
        public static T FindOrCreate<T>(string fileName, string folder = null) where T : ScriptableObject
        {
            folder ??= SettingsEditorPaths.GetDefaultSettingsFolder();
            EnsureFolder(folder);

            var assetPath = $"{folder}/{fileName}.asset";
            var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset != null) return asset;

            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return asset;
        }

        public static T Find<T>(string fileName, string folder = null) where T : ScriptableObject
        {
            folder ??= SettingsEditorPaths.GetDefaultSettingsFolder();
            var assetPath = $"{folder}/{fileName}.asset";
            return AssetDatabase.LoadAssetAtPath<T>(assetPath);
        }

        public static void Ping(UnityEngine.Object obj)
        {
            if (obj == null) return;
            EditorGUIUtility.PingObject(obj);
            Selection.activeObject = obj;
        }

        private static void EnsureFolder(string folder)
        {
            // folder like: Assets/__MyGame/Design/Settings
            if (AssetDatabase.IsValidFolder(folder)) return;

            var parts = folder.Split('/');
            if (parts.Length < 2 || parts[0] != "Assets")
                throw new Exception($"Invalid folder path: {folder}");

            var current = "Assets";
            for (int i = 1; i < parts.Length; i++)
            {
                var next = $"{current}/{parts[i]}";
                if (!AssetDatabase.IsValidFolder(next))
                    AssetDatabase.CreateFolder(current, parts[i]);
                current = next;
            }
        }
    }
}
#endif