#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using HTDA.Framework.Settings.Core;

namespace HTDA.Framework.Settings.Editor.Core
{
    public abstract class SettingsPageBase<TAsset> : ISettingsPage
        where TAsset : ScriptableObject
    {
        protected TAsset Asset;
        protected SerializedObject SO;

        public abstract string Id { get; }
        public abstract string DisplayName { get; }
        public abstract int Order { get; }
        public abstract string Title { get; }
        protected abstract string AssetFileName { get; }

        // Optional: show a big "Clear Game Data" button (like your screenshot)
        protected virtual bool ShowClearGameDataButton => false;

        public virtual void OnGUI()
        {
            DrawToolbar();

            if (Asset == null)
            {
                EditorGUILayout.HelpBox("Settings asset not found. Click Create to generate it.", MessageType.Info);
                return;
            }

            if (SO == null || SO.targetObject != Asset)
                SO = new SerializedObject(Asset);

            SO.Update();

            DrawValidationIfAny(Asset);

            DrawBody(SO);

            SO.ApplyModifiedProperties();

            if (ShowClearGameDataButton)
                DrawClearGameDataButton();
        }

        protected virtual void DrawBody(SerializedObject so)
        {
            // Generic draw-all (fixed iterator logic)
            var it = so.GetIterator();
            bool enterChildren = true;

            while (it.NextVisible(enterChildren))
            {
                enterChildren = false;

                if (it.propertyPath == "m_Script")
                {
                    using (new EditorGUI.DisabledScope(true))
                        EditorGUILayout.PropertyField(it, true);
                    continue;
                }

                EditorGUILayout.PropertyField(it, true);
            }
        }

        private void DrawToolbar()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Asset", GUILayout.Width(40));
                    Asset = (TAsset)EditorGUILayout.ObjectField(Asset, typeof(TAsset), false);

                    if (GUILayout.Button("Locate", GUILayout.Width(70)))
                    {
                        Asset = SettingsAssetEditorLocator.Find<TAsset>(AssetFileName);
                        if (Asset != null) SettingsEditorActions.OpenInProject(Asset);
                    }

                    if (GUILayout.Button("Create", GUILayout.Width(70)))
                    {
                        Asset = SettingsAssetEditorLocator.FindOrCreate<TAsset>(AssetFileName);
                        SettingsEditorActions.OpenInProject(Asset);
                    }

                    if (GUILayout.Button("Select", GUILayout.Width(70)))
                        SettingsEditorActions.OpenInProject(Asset);

                    if (GUILayout.Button("Folder", GUILayout.Width(70)))
                        SettingsEditorActions.OpenFolderOf(Asset);
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button("Reset", GUILayout.Width(70)))
                        SettingsEditorActions.ResetToDefaults(Asset);

                    if (GUILayout.Button("Export", GUILayout.Width(70)))
                        SettingsEditorActions.ExportJson(Asset);

                    if (GUILayout.Button("Import", GUILayout.Width(70)))
                        SettingsEditorActions.ImportJson(Asset);

                    if (GUILayout.Button("Delete", GUILayout.Width(70)))
                    {
                        SettingsEditorActions.DeleteAsset(Asset);
                        Asset = null;
                        SO = null;
                    }
                }
            }
        }

        private static void DrawValidationIfAny(Object asset)
        {
            if (asset is not IValidatableSettings v) return;

            List<string> errors = null;
            foreach (var e in v.Validate())
            {
                errors ??= new List<string>();
                if (!string.IsNullOrWhiteSpace(e))
                    errors.Add(e);
            }

            if (errors == null || errors.Count == 0) return;

            EditorGUILayout.Space(6);
            EditorGUILayout.HelpBox(string.Join("\n", errors), MessageType.Warning);
            EditorGUILayout.Space(6);
        }

        private void DrawClearGameDataButton()
        {
            EditorGUILayout.Space(10);

            var prev = GUI.backgroundColor;
            GUI.backgroundColor = new Color(0.85f, 0.15f, 0.15f);

            if (GUILayout.Button("Clear All Game Data", GUILayout.Height(28)))
                SettingsEditorActions.ClearGameData_Default();

            GUI.backgroundColor = prev;
        }
    }
}
#endif