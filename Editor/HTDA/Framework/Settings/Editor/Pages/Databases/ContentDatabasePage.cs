#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using HTDA.Framework.Settings.Databases;
using HTDA.Framework.Settings.Editor.Core;

namespace HTDA.Framework.Settings.Editor.Databases
{
    [SettingsPage]
    public sealed class ContentDatabasePage : SettingsPageBase<ContentDatabaseAsset>
    {
        public override string Id => "content_db";
        public override string DisplayName => "Content DB";
        public override int Order => 25;
        public override string Title => "Content Database";
        protected override string AssetFileName => "ContentDatabase";

        private ReorderableList _list;

        protected override void DrawBody(SerializedObject so)
        {
            var entries = so.FindProperty("entries");
            if (_list == null || _list.serializedProperty != entries)
            {
                _list = new ReorderableList(so, entries, true, true, true, true);
                _list.drawHeaderCallback = r => EditorGUI.LabelField(r, "Entries (id → prefab/icon)");
                _list.elementHeightCallback = i => EditorGUI.GetPropertyHeight(entries.GetArrayElementAtIndex(i), true) + 8f;

                _list.drawElementCallback = (r, i, a, f) =>
                {
                    r.y += 4; r.height -= 4;
                    var el = entries.GetArrayElementAtIndex(i);
                    var id = (el.FindPropertyRelative("id").stringValue ?? "").Trim();
                    var label = string.IsNullOrEmpty(id) ? $"Element {i}" : id;
                    EditorGUI.PropertyField(r, el, new GUIContent(label), true);
                };
            }

            _list.DoLayoutList();

            EditorGUILayout.HelpBox(
                "This is a generic database for most genres.\n" +
                "Use it for items/enemies/ui prefabs etc. with a stable string id.",
                MessageType.None
            );
        }
    }
}
#endif