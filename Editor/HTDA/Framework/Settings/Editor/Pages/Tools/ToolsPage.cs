#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using HTDA.Framework.Settings.Core;
using HTDA.Framework.Settings.Editor.Core;

// Runtime assets
using HTDA.Framework.Settings.Global;
using HTDA.Framework.Settings.Audio;
using HTDA.Framework.Settings.SupportItems;
using HTDA.Framework.Settings.Tutorials;
using HTDA.Framework.Settings.Ads;
using HTDA.Framework.Settings.IAP;
using HTDA.Framework.Settings.Credits;
using HTDA.Framework.Settings.Databases;

namespace HTDA.Framework.Settings.Editor.Tools
{
    [SettingsPage]
    public sealed class ToolsPage : ISettingsPage
    {
        public string Id => "tools";
        public string DisplayName => "Tools";
        public int Order => 0;
        public string Title => "Settings Tools";

        private Vector2 _scroll;

        public void OnGUI()
        {
            _scroll = EditorGUILayout.BeginScrollView(_scroll);

            EditorGUILayout.HelpBox(
                "Utilities to speed up project setup:\n" +
                "- Create default settings assets\n" +
                "- Open settings folder\n" +
                "- Validate assets (shows warnings in each page)\n" +
                "- Export/Import JSON\n" +
                "- Clear game data (PlayerPrefs default)",
                MessageType.None
            );

            DrawFoldersBox();
            DrawCreateAssetsBox();
            DrawExportImportBox();
            DrawDataActionsBox();

            EditorGUILayout.EndScrollView();
        }

        private static void DrawFoldersBox()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Folders", EditorStyles.boldLabel);

                EditorGUILayout.LabelField("Default Settings Folder:", SettingsEditorPaths.GetDefaultSettingsFolder());

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Ensure Folder Exists"))
                    {
                        // ensure by creating one asset then deleting it if you want
                        var a = SettingsAssetEditorLocator.FindOrCreate<GlobalSettingsAsset>("GlobalSettings");
                        SettingsAssetEditorLocator.Ping(a);
                    }

                    if (GUILayout.Button("Open Folder"))
                    {
                        var a = SettingsAssetEditorLocator.FindOrCreate<GlobalSettingsAsset>("GlobalSettings");
                        SettingsEditorActions.OpenFolderOf(a);
                    }
                }
            }
        }

        private static void DrawCreateAssetsBox()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Create Assets", EditorStyles.boldLabel);

                if (GUILayout.Button("Create ALL default assets"))
                {
                    CreateAll();
                    EditorUtility.DisplayDialog("Create Assets", "Created (or located) default settings assets.", "OK");
                }

                EditorGUILayout.LabelField("Created assets are placed under the default folder convention.");
            }
        }

        private static void DrawExportImportBox()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Export / Import (JSON)", EditorStyles.boldLabel);

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Export Selected"))
                    {
                        var obj = Selection.activeObject;
                        if (obj != null) SettingsEditorActions.ExportJson(obj);
                        else EditorUtility.DisplayDialog("Export", "Select a settings asset first.", "OK");
                    }

                    if (GUILayout.Button("Import Into Selected"))
                    {
                        var obj = Selection.activeObject;
                        if (obj != null) SettingsEditorActions.ImportJson(obj);
                        else EditorUtility.DisplayDialog("Import", "Select a settings asset first.", "OK");
                    }
                }

                if (GUILayout.Button("Export ALL (one-by-one)"))
                {
                    foreach (var a in FindAllAssets())
                    {
                        if (a == null) continue;
                        SettingsEditorActions.ExportJson(a);
                    }
                }

                EditorGUILayout.HelpBox(
                    "Tip: Export/Import uses EditorJsonUtility.\n" +
                    "Keep ids stable so JSON remains compatible across versions.",
                    MessageType.None
                );
            }
        }

        private static void DrawDataActionsBox()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Data Actions", EditorStyles.boldLabel);

                if (GUILayout.Button("Clear All Game Data (PlayerPrefs)"))
                    SettingsEditorActions.ClearGameData_Default();

                EditorGUILayout.HelpBox(
                    "This default implementation clears PlayerPrefs.\n" +
                    "Later you can replace it with your Save.Core service.",
                    MessageType.None
                );
            }
        }

        private static void CreateAll()
        {
            SettingsAssetEditorLocator.FindOrCreate<GlobalSettingsAsset>("GlobalSettings");
            SettingsAssetEditorLocator.FindOrCreate<AudioSettingsAsset>("AudioSettings");
            SettingsAssetEditorLocator.FindOrCreate<SupportItemsSettingsAsset>("SupportItemsSettings");

            SettingsAssetEditorLocator.FindOrCreate<TutorialsSettingsAsset>("TutorialsSettings");
            SettingsAssetEditorLocator.FindOrCreate<AdsSettingsAsset>("AdsSettings");
            SettingsAssetEditorLocator.FindOrCreate<IAPSettingsAsset>("IAPSettings");
            SettingsAssetEditorLocator.FindOrCreate<CreditsSettingsAsset>("CreditsSettings");
            SettingsAssetEditorLocator.FindOrCreate<ContentDatabaseAsset>("ContentDatabase");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static IEnumerable<Object> FindAllAssets()
        {
            yield return SettingsAssetEditorLocator.Find<GlobalSettingsAsset>("GlobalSettings");
            yield return SettingsAssetEditorLocator.Find<AudioSettingsAsset>("AudioSettings");
            yield return SettingsAssetEditorLocator.Find<SupportItemsSettingsAsset>("SupportItemsSettings");
            yield return SettingsAssetEditorLocator.Find<TutorialsSettingsAsset>("TutorialsSettings");
            yield return SettingsAssetEditorLocator.Find<AdsSettingsAsset>("AdsSettings");
            yield return SettingsAssetEditorLocator.Find<IAPSettingsAsset>("IAPSettings");
            yield return SettingsAssetEditorLocator.Find<CreditsSettingsAsset>("CreditsSettings");
            yield return SettingsAssetEditorLocator.Find<ContentDatabaseAsset>("ContentDatabase");
        }
    }
}
#endif