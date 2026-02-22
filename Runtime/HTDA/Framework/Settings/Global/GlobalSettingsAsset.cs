using System;
using System.Collections.Generic;
using UnityEngine;
using HTDA.Framework.Settings.Core;

namespace HTDA.Framework.Settings.Global
{
    [CreateAssetMenu(menuName = "HTDA/Settings/Global Settings", fileName = "GlobalSettings")]
    public sealed class GlobalSettingsAsset : SettingsAssetBase, IValidatableSettings
    {
        [Header("Common")]
        [Tooltip("Generic game-level settings used by many systems (timing, toggles, flags).")]
        public Common common = new Common();

        [Header("Economy")]
        [Tooltip("Generic reward / bonus ranges. Avoid game-specific structures here.")]
        public Economy economy = new Economy();

        [Header("Feature Flags")]
        [Tooltip("Optional list of enabled feature ids (for QA / staged rollout / debug). Example: \"ads\", \"iap\", \"tutorial\".")]
        public List<string> enabledFeatureIds = new List<string>();

        [Serializable]
        public class Common
        {
            [Tooltip("A generic value for session open time or timed feature duration (seconds).")]
            [Min(0)] public int sessionOpenTimeSeconds = 3600;

            [Tooltip("Enable GDPR related flows (consent UI, restricted tracking, etc.).")]
            public bool enableGDPR = false;
        }

        [Serializable]
        public class Economy
        {
            [Tooltip("Min coins bonus used by reward systems.")]
            [Min(0)] public int minCoinsBonus = 0;

            [Tooltip("Max coins bonus used by reward systems.")]
            [Min(0)] public int maxCoinsBonus = 0;

            [Tooltip("Min item bonus used by reward systems.")]
            [Min(0)] public int minItemBonus = 0;

            [Tooltip("Max item bonus used by reward systems.")]
            [Min(0)] public int maxItemBonus = 0;
        }

        public IEnumerable<string> Validate()
        {
            if (economy.minCoinsBonus > economy.maxCoinsBonus)
                yield return "[Global/Economy] minCoinsBonus must be <= maxCoinsBonus.";

            if (economy.minItemBonus > economy.maxItemBonus)
                yield return "[Global/Economy] minItemBonus must be <= maxItemBonus.";

            // feature ids basic check
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < enabledFeatureIds.Count; i++)
            {
                var id = enabledFeatureIds[i];
                if (string.IsNullOrWhiteSpace(id))
                {
                    yield return $"[Global/FeatureFlags] enabledFeatureIds[{i}] is empty.";
                    continue;
                }

                if (!set.Add(id.Trim()))
                    yield return $"[Global/FeatureFlags] Duplicate feature id: '{id}'.";
            }
        }
    }
}