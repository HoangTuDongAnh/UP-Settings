#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace HTDA.Framework.Settings.Editor.Core
{
    public static class SettingsEditorActions
    {
        public static void OpenInProject(Object obj)
        {
            if (obj == null) return;
            Selection.activeObject = obj;
            EditorGUIUtility.PingObject(obj);
        }

        public static void OpenFolderOf(Object obj)
        {
            if (obj == null) return;
            var path = AssetDatabase.GetAssetPath(obj);
            if (string.IsNullOrEmpty(path)) return;

            var full = Path.GetFullPath(path);
            EditorUtility.RevealInFinder(full);
        }

        public static bool Confirm(string title, string message, string ok = "OK", string cancel = "Cancel")
            => EditorUtility.DisplayDialog(title, message, ok, cancel);

        public static void DeleteAsset(Object obj)
        {
            if (obj == null) return;

            var path = AssetDatabase.GetAssetPath(obj);
            if (string.IsNullOrEmpty(path)) return;

            if (!Confirm("Delete Settings Asset", $"Delete asset?\n\n{path}\n\nThis cannot be undone.", "Delete", "Cancel"))
                return;

            AssetDatabase.DeleteAsset(path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void ResetToDefaults(Object obj)
        {
            if (obj == null) return;

            if (!Confirm("Reset to Defaults", "Reset all fields to default values of a new instance?", "Reset", "Cancel"))
                return;

            var type = obj.GetType();
            var temp = ScriptableObject.CreateInstance(type);

            // Copy serialized data (defaults)
            var src = new SerializedObject(temp);
            var dst = new SerializedObject(obj);
            dst.CopyFromSerializedProperty(src.GetIterator()); // not reliable for whole object

            // Proper overwrite: use JSON snapshot
            var json = EditorJsonUtility.ToJson(temp);
            EditorJsonUtility.FromJsonOverwrite(json, obj);

            Object.DestroyImmediate(temp);

            EditorUtility.SetDirty(obj);
            AssetDatabase.SaveAssets();
        }

        public static void ExportJson(Object obj)
        {
            if (obj == null) return;

            var defaultName = obj.name + ".json";
            var file = EditorUtility.SaveFilePanel("Export Settings JSON", Application.dataPath, defaultName, "json");
            if (string.IsNullOrEmpty(file)) return;

            var json = EditorJsonUtility.ToJson(obj, true);
            File.WriteAllText(file, json);

            EditorUtility.DisplayDialog("Export JSON", "Exported successfully.", "OK");
        }

        public static void ImportJson(Object obj)
        {
            if (obj == null) return;

            var file = EditorUtility.OpenFilePanel("Import Settings JSON", Application.dataPath, "json");
            if (string.IsNullOrEmpty(file)) return;

            if (!Confirm("Import JSON", "Import will overwrite current values.\nContinue?", "Import", "Cancel"))
                return;

            var json = File.ReadAllText(file);
            EditorJsonUtility.FromJsonOverwrite(json, obj);

            EditorUtility.SetDirty(obj);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void ClearGameData_Default()
        {
            if (!Confirm("Clear Game Data", "This will clear PlayerPrefs (default). Continue?", "Clear", "Cancel"))
                return;

            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("[HTDA Settings] PlayerPrefs.DeleteAll()");
        }
    }
}
#endif