#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using HTDA.Framework.Settings.Editor.Core;
using HTDA.Framework.Settings.Global;

namespace HTDA.Framework.Settings.Editor.Global
{
    [SettingsPage]
    public sealed class GlobalSettingsPage : SettingsPageBase<GlobalSettingsAsset>
    {
        public override string Id => "global";
        public override string DisplayName => "Global";
        public override int Order => 10;
        public override string Title => "Global Settings";
        protected override string AssetFileName => "GlobalSettings";

        protected override bool ShowClearGameDataButton => true;

        private bool _foldCommon = true;
        private bool _foldEconomy = true;
        private bool _foldFlags = true;

        protected override void DrawBody(SerializedObject so)
        {
            // Draw default script field is handled by base iterator draw,
            // but we do custom UI with sections for better UX.

            var common = so.FindProperty("common");
            var economy = so.FindProperty("economy");
            var flags = so.FindProperty("enabledFeatureIds");

            _foldCommon = EditorGUILayout.BeginFoldoutHeaderGroup(_foldCommon, "Common", SettingsUIStyles.SectionHeader);
            if (_foldCommon) EditorGUILayout.PropertyField(common, true);
            EditorGUILayout.EndFoldoutHeaderGroup();

            GUILayout.Space(6);

            _foldEconomy = EditorGUILayout.BeginFoldoutHeaderGroup(_foldEconomy, "Economy", SettingsUIStyles.SectionHeader);
            if (_foldEconomy) EditorGUILayout.PropertyField(economy, true);
            EditorGUILayout.EndFoldoutHeaderGroup();

            GUILayout.Space(6);

            _foldFlags = EditorGUILayout.BeginFoldoutHeaderGroup(_foldFlags, "Feature Flags", SettingsUIStyles.SectionHeader);
            if (_foldFlags)
            {
                EditorGUILayout.PropertyField(flags, true);

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Sort A→Z", GUILayout.Width(90)))
                    {
                        SortStringArray(flags);
                    }

                    if (GUILayout.Button("Remove Empty", GUILayout.Width(110)))
                    {
                        RemoveEmpty(flags);
                    }
                }

                EditorGUILayout.HelpBox("Tip: use simple ids like 'ads', 'iap', 'tutorial'. Keep lowercase for consistency.", MessageType.None);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private static void SortStringArray(SerializedProperty arrayProp)
        {
            if (arrayProp == null || !arrayProp.isArray) return;
            var list = new System.Collections.Generic.List<string>(arrayProp.arraySize);
            for (int i = 0; i < arrayProp.arraySize; i++)
                list.Add(arrayProp.GetArrayElementAtIndex(i).stringValue ?? "");

            list.Sort(System.StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < list.Count; i++)
                arrayProp.GetArrayElementAtIndex(i).stringValue = list[i];

            arrayProp.serializedObject.ApplyModifiedProperties();
        }

        private static void RemoveEmpty(SerializedProperty arrayProp)
        {
            if (arrayProp == null || !arrayProp.isArray) return;

            for (int i = arrayProp.arraySize - 1; i >= 0; i--)
            {
                var s = (arrayProp.GetArrayElementAtIndex(i).stringValue ?? "").Trim();
                if (string.IsNullOrEmpty(s))
                    arrayProp.DeleteArrayElementAtIndex(i);
                else
                    arrayProp.GetArrayElementAtIndex(i).stringValue = s;
            }

            arrayProp.serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif