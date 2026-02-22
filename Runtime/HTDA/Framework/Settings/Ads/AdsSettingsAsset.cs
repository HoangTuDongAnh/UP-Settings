using System;
using System.Collections.Generic;
using UnityEngine;
using HTDA.Framework.Settings.Core;

namespace HTDA.Framework.Settings.Ads
{
    [CreateAssetMenu(menuName = "HTDA/Settings/Ads Settings", fileName = "AdsSettings")]
    public sealed class AdsSettingsAsset : SettingsAssetBase, IValidatableSettings
    {
        [Header("General")]
        [Tooltip("Global toggle for ads.")]
        public bool enabled = true;

        [Tooltip("If true, runtime may disable ads in development builds (optional behavior).")]
        public bool disableInDev = false;

        [Header("Placements")]
        [Tooltip("Ad placements by id. Runtime ads service will request ads by placement id.")]
        public List<Placement> placements = new();

        public enum PlacementType { Rewarded, Interstitial, Banner }

        [Serializable]
        public class Placement
        {
            [Tooltip("Unique placement id. Example: 'rewarded_end_level', 'interstitial_fail', 'banner_home'.")]
            public string id = "rewarded_end_level";

            [Tooltip("Placement type.")]
            public PlacementType type = PlacementType.Rewarded;

            [Tooltip("Enable/disable this placement.")]
            public bool placementEnabled = true;

            [Tooltip("Cooldown seconds between showing this placement.")]
            [Min(0)] public int cooldownSeconds = 0;

            [Tooltip("Optional provider placement key (mediation placement name / ad unit alias).")]
            public string providerPlacementKey;

            [TextArea(1, 4)]
            [Tooltip("Designer note.")]
            public string note;
        }

        public IEnumerable<string> Validate()
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < placements.Count; i++)
            {
                var p = placements[i];
                if (p == null) { yield return $"[Ads] placements[{i}] is null."; continue; }

                var id = (p.id ?? "").Trim();
                if (string.IsNullOrEmpty(id)) yield return $"[Ads] placements[{i}] id is empty.";
                else if (!set.Add(id)) yield return $"[Ads] Duplicate id: '{id}'.";
            }
        }
    }
}