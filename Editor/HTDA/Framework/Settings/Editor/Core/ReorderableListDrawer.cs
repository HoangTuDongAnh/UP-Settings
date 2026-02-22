#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace HTDA.Framework.Settings.Editor.Core
{
    internal sealed class ReorderableListDrawer
    {
        private readonly SerializedObject _so;
        private readonly SerializedProperty _listProp;
        private readonly ReorderableList _list;

        public ReorderableListDrawer(SerializedObject so, SerializedProperty listProp, string header)
        {
            _so = so;
            _listProp = listProp;

            _list = new ReorderableList(so, listProp, draggable: true, displayHeader: true, displayAddButton: true, displayRemoveButton: true);
            _list.drawHeaderCallback = rect => EditorGUI.LabelField(rect, header);

            _list.elementHeightCallback = index =>
            {
                var element = _listProp.GetArrayElementAtIndex(index);
                return EditorGUI.GetPropertyHeight(element, true) + 6f;
            };

            _list.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                rect.y += 3;
                rect.height -= 3;

                var element = _listProp.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, element, new GUIContent($"Element {index}"), true);
            };
        }

        public void DoLayout()
        {
            _so.Update();
            _list.DoLayoutList();
            _so.ApplyModifiedProperties();
        }
    }
}
#endif