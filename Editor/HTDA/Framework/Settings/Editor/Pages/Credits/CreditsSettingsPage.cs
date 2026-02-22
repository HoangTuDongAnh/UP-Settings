#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using HTDA.Framework.Settings.Credits;
using HTDA.Framework.Settings.Editor.Core;

namespace HTDA.Framework.Settings.Editor.Credits
{
    [SettingsPage]
    public sealed class CreditsSettingsPage : SettingsPageBase<CreditsSettingsAsset>
    {
        public override string Id => "credits";
        public override string DisplayName => "Credits";
        public override int Order => 70;
        public override string Title => "Credits Settings";
        protected override string AssetFileName => "CreditsSettings";

        private ReorderableList _list;

        protected override void DrawBody(SerializedObject so)
        {
            var entries = so.FindProperty("entries");

            if (_list == null || _list.serializedProperty != entries)
            {
                _list = new ReorderableList(so, entries, true, true, true, true);
                _list.drawHeaderCallback = r => EditorGUI.LabelField(r, "Entries");
                _list.elementHeightCallback = i => EditorGUI.GetPropertyHeight(entries.GetArrayElementAtIndex(i), true) + 8f;

                _list.drawElementCallback = (r, i, a, f) =>
                {
                    r.y += 4; r.height -= 4;
                    var el = entries.GetArrayElementAtIndex(i);
                    var name = (el.FindPropertyRelative("name").stringValue ?? "").Trim();
                    var role = (el.FindPropertyRelative("role").stringValue ?? "").Trim();
                    var label = string.IsNullOrEmpty(name) ? $"Element {i}" : $"{name}  ({role})";
                    EditorGUI.PropertyField(r, el, new GUIContent(label), true);
                };
            }

            _list.DoLayoutList();

            EditorGUILayout.HelpBox("Credits entries are simple and portable across genres.", MessageType.None);
        }
    }
}
#endif