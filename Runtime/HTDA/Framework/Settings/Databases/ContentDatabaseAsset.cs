using System;
using System.Collections.Generic;
using UnityEngine;
using HTDA.Framework.Settings.Core;

namespace HTDA.Framework.Settings.Databases
{
    [CreateAssetMenu(menuName = "HTDA/Settings/Databases/Content Database", fileName = "ContentDatabase")]
    public sealed class ContentDatabaseAsset : SettingsAssetBase, IValidatableSettings
    {
        [Tooltip("Generic content entries usable in most game genres (id + prefab + icon).")]
        public List<Entry> entries = new();

        [Serializable]
        public class Entry
        {
            [Tooltip("Unique id for lookup. Example: 'item_bomb', 'enemy_slime', 'ui_button_primary'.")]
            public string id = "item_01";

            [Tooltip("Optional prefab reference (for spawn / UI preview).")]
            public GameObject prefab;

            [Tooltip("Optional icon (for UI).")]
            public Sprite icon;

            [TextArea(1, 4)]
            [Tooltip("Designer note.")]
            public string note;
        }

        public IEnumerable<string> Validate()
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < entries.Count; i++)
            {
                var e = entries[i];
                if (e == null) { yield return $"[ContentDB] entries[{i}] is null."; continue; }

                var id = (e.id ?? "").Trim();
                if (string.IsNullOrEmpty(id)) yield return $"[ContentDB] entries[{i}] id is empty.";
                else if (!set.Add(id)) yield return $"[ContentDB] Duplicate id: '{id}'.";
            }
        }
    }
}