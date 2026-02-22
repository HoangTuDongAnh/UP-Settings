using System;
using System.Collections.Generic;
using UnityEngine;
using HTDA.Framework.Settings.Core;

namespace HTDA.Framework.Settings.SupportItems
{
    [CreateAssetMenu(menuName = "HTDA/Settings/Support Items Settings", fileName = "SupportItemsSettings")]
    public sealed class SupportItemsSettingsAsset : SettingsAssetBase, IValidatableSettings
    {
        [Tooltip("Support items / boosters database. Use string id to remain genre-agnostic.")]
        public List<SupportItem> items = new List<SupportItem>();

        [Serializable]
        public class SupportItem
        {
            [Tooltip("Unique key used by runtime. Prefer lowercase_with_underscores.")]
            public string id = "hammer";

            [Tooltip("Icon shown in UI.")]
            public Sprite icon;

            [Header("Economy")]
            [Tooltip("Base price of this item in your soft currency.")]
            [Min(0)] public int price = 0;

            [Tooltip("Starting quantity when player first gets access.")]
            [Min(0)] public int startingQuantity = 0;

            [Tooltip("Quantity gained per purchase.")]
            [Min(1)] public int purchaseQuantity = 1;

            [Header("Requirements")]
            [Tooltip("Generic required level / progress gate.")]
            [Min(0)] public int levelRequired = 0;

            [Header("Effect")]
            [Tooltip("Generic effect value (meaning depends on your gameplay).")]
            [Min(0)] public int bonusValue = 0;

            [Tooltip("Designer note.")]
            public string note;
        }

        public IEnumerable<string> Validate()
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < items.Count; i++)
            {
                var it = items[i];
                if (it == null)
                {
                    yield return $"[SupportItems] items[{i}] is null.";
                    continue;
                }

                var id = (it.id ?? "").Trim();
                if (string.IsNullOrEmpty(id))
                    yield return $"[SupportItems] items[{i}] id is empty.";
                else if (!set.Add(id))
                    yield return $"[SupportItems] Duplicate id: '{id}'.";

                if (it.icon == null)
                    yield return $"[SupportItems] items[{i}] '{id}' icon is missing.";
            }
        }
    }
}