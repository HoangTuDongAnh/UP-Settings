using System;
using System.Collections.Generic;
using UnityEngine;
using HTDA.Framework.Settings.Core;

namespace HTDA.Framework.Settings.IAP
{
    [CreateAssetMenu(menuName = "HTDA/Settings/IAP Settings", fileName = "IAPSettings")]
    public sealed class IAPSettingsAsset : SettingsAssetBase, IValidatableSettings
    {
        [Header("General")]
        [Tooltip("Global toggle for IAP.")]
        public bool enabled = true;

        [Header("Products")]
        [Tooltip("IAP products list. Runtime IAP service will map by id.")]
        public List<Product> products = new();

        public enum ProductType { Consumable, NonConsumable, Subscription }

        [Serializable]
        public class Product
        {
            [Tooltip("Unique product id used in game logic. Example: 'remove_ads', 'gems_100'.")]
            public string id = "remove_ads";

            [Tooltip("Product type.")]
            public ProductType type = ProductType.NonConsumable;

            [Header("Store IDs")]
            [Tooltip("Google Play product id (optional). If empty, runtime can fallback to 'id'.")]
            public string googlePlayId;

            [Tooltip("Apple App Store product id (optional). If empty, runtime can fallback to 'id'.")]
            public string appleAppStoreId;

            [Header("Reward")]
            [Tooltip("Optional reward descriptor string. Example: 'gems:100' or 'remove_ads:true'.")]
            public string reward;

            [TextArea(1, 4)]
            [Tooltip("Designer note.")]
            public string note;
        }

        public IEnumerable<string> Validate()
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < products.Count; i++)
            {
                var p = products[i];
                if (p == null) { yield return $"[IAP] products[{i}] is null."; continue; }

                var id = (p.id ?? "").Trim();
                if (string.IsNullOrEmpty(id)) yield return $"[IAP] products[{i}] id is empty.";
                else if (!set.Add(id)) yield return $"[IAP] Duplicate id: '{id}'.";
            }
        }
    }
}