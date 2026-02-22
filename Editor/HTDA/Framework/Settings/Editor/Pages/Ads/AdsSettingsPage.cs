#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using HTDA.Framework.Settings.Ads;
using HTDA.Framework.Settings.Editor.Core;

namespace HTDA.Framework.Settings.Editor.Ads
{
    [SettingsPage]
    public sealed class AdsSettingsPage : SettingsPageBase<AdsSettingsAsset>
    {
        public override string Id => "ads";
        public override string DisplayName => "Advertising";
        public override int Order => 50;
        public override string Title => "Ads Settings";
        protected override string AssetFileName => "AdsSettings";

        private ReorderableList _list;

        protected override void DrawBody(SerializedObject so)
        {
            EditorGUILayout.PropertyField(so.FindProperty("enabled"));
            EditorGUILayout.PropertyField(so.FindProperty("disableInDev"));

            var placements = so.FindProperty("placements");
            if (_list == null || _list.serializedProperty != placements)
            {
                _list = new ReorderableList(so, placements, true, true, true, true);
                _list.drawHeaderCallback = r => EditorGUI.LabelField(r, "Placements (id → type)");
                _list.elementHeightCallback = i => EditorGUI.GetPropertyHeight(placements.GetArrayElementAtIndex(i), true) + 8f;

                _list.drawElementCallback = (r, i, a, f) =>
                {
                    r.y += 4; r.height -= 4;
                    var el = placements.GetArrayElementAtIndex(i);
                    var id = (el.FindPropertyRelative("id").stringValue ?? "").Trim();
                    var typeProp = el.FindPropertyRelative("type");
                    var typeName = typeProp.enumDisplayNames[typeProp.enumValueIndex];
                    var label = string.IsNullOrEmpty(id) ? $"Element {i}" : $"{id}  ({typeName})";
                    EditorGUI.PropertyField(r, el, new GUIContent(label), true);
                };
            }

            _list.DoLayoutList();

            EditorGUILayout.HelpBox(
                "Placements are requested by id in runtime ads service.\n" +
                "Use cooldownSeconds to avoid spamming the same placement.",
                MessageType.None
            );
        }
    }
}
#endif