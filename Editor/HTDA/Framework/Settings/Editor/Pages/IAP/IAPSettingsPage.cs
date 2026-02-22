#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using HTDA.Framework.Settings.Editor.Core;
using HTDA.Framework.Settings.IAP;

namespace HTDA.Framework.Settings.Editor.IAP
{
    [SettingsPage]
    public sealed class IAPSettingsPage : SettingsPageBase<IAPSettingsAsset>
    {
        public override string Id => "iap";
        public override string DisplayName => "In-App";
        public override int Order => 60;
        public override string Title => "IAP Settings";
        protected override string AssetFileName => "IAPSettings";

        private ReorderableList _list;

        protected override void DrawBody(SerializedObject so)
        {
            EditorGUILayout.PropertyField(so.FindProperty("enabled"));

            var products = so.FindProperty("products");
            if (_list == null || _list.serializedProperty != products)
            {
                _list = new ReorderableList(so, products, true, true, true, true);
                _list.drawHeaderCallback = r => EditorGUI.LabelField(r, "Products (id → type)");
                _list.elementHeightCallback = i => EditorGUI.GetPropertyHeight(products.GetArrayElementAtIndex(i), true) + 8f;

                _list.drawElementCallback = (r, i, a, f) =>
                {
                    r.y += 4; r.height -= 4;
                    var el = products.GetArrayElementAtIndex(i);
                    var id = (el.FindPropertyRelative("id").stringValue ?? "").Trim();
                    var typeProp = el.FindPropertyRelative("type");
                    var typeName = typeProp.enumDisplayNames[typeProp.enumValueIndex];
                    var label = string.IsNullOrEmpty(id) ? $"Element {i}" : $"{id}  ({typeName})";
                    EditorGUI.PropertyField(r, el, new GUIContent(label), true);
                };
            }

            _list.DoLayoutList();

            EditorGUILayout.HelpBox(
                "Store ids are optional; runtime can fallback to 'id' if needed.\n" +
                "reward is a generic descriptor that your economy layer can interpret.",
                MessageType.None
            );
        }
    }
}
#endif