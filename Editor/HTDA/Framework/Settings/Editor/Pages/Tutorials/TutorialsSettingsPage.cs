#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using HTDA.Framework.Settings.Editor.Core;
using HTDA.Framework.Settings.Tutorials;

namespace HTDA.Framework.Settings.Editor.Tutorials
{
    [SettingsPage]
    public sealed class TutorialsSettingsPage : SettingsPageBase<TutorialsSettingsAsset>
    {
        public override string Id => "tutorials";
        public override string DisplayName => "Tutorials";
        public override int Order => 40;
        public override string Title => "Tutorials Settings";
        protected override string AssetFileName => "TutorialsSettings";

        private ReorderableList _list;

        protected override void DrawBody(SerializedObject so)
        {
            EditorGUILayout.PropertyField(so.FindProperty("enabled"));
            EditorGUILayout.PropertyField(so.FindProperty("allowSkipInDev"));

            var tutorials = so.FindProperty("tutorials");
            if (_list == null || _list.serializedProperty != tutorials)
            {
                _list = new ReorderableList(so, tutorials, true, true, true, true);

                _list.drawHeaderCallback = rect =>
                    EditorGUI.LabelField(rect, "Tutorial Definitions (id → trigger)");

                _list.elementHeightCallback = index =>
                    EditorGUI.GetPropertyHeight(tutorials.GetArrayElementAtIndex(index), true) + 8f;

                _list.drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    rect.y += 4;
                    rect.height -= 4;

                    var el = tutorials.GetArrayElementAtIndex(index);

                    var id = (el.FindPropertyRelative("id").stringValue ?? "").Trim();
                    var triggerProp = el.FindPropertyRelative("triggerType");
                    var triggerName = triggerProp.enumDisplayNames[triggerProp.enumValueIndex];

                    var label = string.IsNullOrEmpty(id) ? $"Element {index}" : $"{id}  ({triggerName})";
                    EditorGUI.PropertyField(rect, el, new GUIContent(label), true);
                };
            }

            _list.DoLayoutList();

            EditorGUILayout.HelpBox(
                "Tutorials are genre-agnostic:\n" +
                "- Define WHEN to show (TriggerType / CustomEvent / OnOpenUI)\n" +
                "- Optionally gate with conditionKey (your runtime interprets it)\n" +
                "- messageKey/content are used by UI layer\n" +
                "Runtime should persist completion if playOnce = true.",
                MessageType.None
            );
        }
    }
}
#endif