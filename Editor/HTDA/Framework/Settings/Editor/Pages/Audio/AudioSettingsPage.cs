#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using HTDA.Framework.Settings.Audio;
using HTDA.Framework.Settings.Editor.Core;

namespace HTDA.Framework.Settings.Editor.Audio
{
    [SettingsPage]
    public sealed class AudioSettingsPage : SettingsPageBase<AudioSettingsAsset>
    {
        public override string Id => "audio";
        public override string DisplayName => "Audio";
        public override int Order => 30;
        public override string Title => "Audio Settings";
        protected override string AssetFileName => "AudioSettings";

        private ReorderableList _list;

        protected override void DrawBody(SerializedObject so)
        {
            var items = so.FindProperty("items");
            if (_list == null || _list.serializedProperty != items)
                BuildList(so, items);

            _list.DoLayoutList();

            GUILayout.Space(6);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Sort by id", GUILayout.Width(90)))
                {
                    SortById(items, "id");
                }

                if (GUILayout.Button("Remove empty id", GUILayout.Width(120)))
                {
                    RemoveEmptyId(items, "id");
                }
            }

            EditorGUILayout.HelpBox("Best practice: id = lowercase_with_underscores. Runtime audio service will use this id.", MessageType.None);
        }

        private void BuildList(SerializedObject so, SerializedProperty items)
        {
            _list = new ReorderableList(so, items, true, true, true, true);

            _list.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Audio Items (id → clip)");
            };

            _list.elementHeightCallback = index =>
            {
                var element = items.GetArrayElementAtIndex(index);
                return EditorGUI.GetPropertyHeight(element, true) + 8f;
            };

            _list.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                rect.y += 4;
                rect.height -= 4;

                var element = items.GetArrayElementAtIndex(index);
                var idProp = element.FindPropertyRelative("id");
                var id = (idProp.stringValue ?? "").Trim();
                var label = string.IsNullOrEmpty(id) ? $"Element {index}" : id;

                EditorGUI.PropertyField(rect, element, new GUIContent(label), true);
            };
        }

        private static void SortById(SerializedProperty array, string idField)
        {
            if (array == null || !array.isArray) return;

            // simple stable sort by id via swap (small lists OK)
            for (int i = 0; i < array.arraySize - 1; i++)
            for (int j = i + 1; j < array.arraySize; j++)
            {
                var a = array.GetArrayElementAtIndex(i).FindPropertyRelative(idField).stringValue ?? "";
                var b = array.GetArrayElementAtIndex(j).FindPropertyRelative(idField).stringValue ?? "";
                if (string.Compare(a, b, System.StringComparison.OrdinalIgnoreCase) > 0)
                    array.MoveArrayElement(j, i);
            }

            array.serializedObject.ApplyModifiedProperties();
        }

        private static void RemoveEmptyId(SerializedProperty array, string idField)
        {
            if (array == null || !array.isArray) return;

            for (int i = array.arraySize - 1; i >= 0; i--)
            {
                var id = (array.GetArrayElementAtIndex(i).FindPropertyRelative(idField).stringValue ?? "").Trim();
                if (string.IsNullOrEmpty(id))
                    array.DeleteArrayElementAtIndex(i);
                else
                    array.GetArrayElementAtIndex(i).FindPropertyRelative(idField).stringValue = id;
            }

            array.serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif