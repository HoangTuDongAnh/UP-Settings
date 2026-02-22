using System;
using System.Collections.Generic;
using UnityEngine;
using HTDA.Framework.Settings.Core;

namespace HTDA.Framework.Settings.Tutorials
{
    [CreateAssetMenu(menuName = "HTDA/Settings/Tutorials Settings", fileName = "TutorialsSettings")]
    public sealed class TutorialsSettingsAsset : SettingsAssetBase, IValidatableSettings
    {
        [Header("General")]
        [Tooltip("Global toggle for the entire tutorial system.")]
        public bool enabled = true;

        [Tooltip("If true, runtime can skip tutorials in development builds (optional behavior).")]
        public bool allowSkipInDev = true;

        [Header("Tutorial Definitions")]
        [Tooltip("Tutorial definitions are genre-agnostic. Runtime decides when/how to display them.")]
        public List<TutorialDefinition> tutorials = new();

        public enum TriggerType
        {
            Manual,
            OnGameStart,
            OnLevelStart,
            OnLevelComplete,
            OnOpenUI,
            CustomEvent
        }

        public enum DisplayType
        {
            Toast,
            Popup,
            Overlay,
            Highlight,
            Custom
        }

        [Serializable]
        public class TutorialDefinition
        {
            [Tooltip("Unique tutorial id. Used by runtime to mark completed. Prefer lowercase_with_underscores.")]
            public string id = "first_time";

            [Tooltip("Enable/disable this tutorial without deleting it.")]
            public bool tutorialEnabled = true;

            [Header("Trigger")]
            [Tooltip("When to consider showing this tutorial.")]
            public TriggerType triggerType = TriggerType.OnGameStart;

            [Tooltip("Required if triggerType = CustomEvent. Example: 'inventory_opened', 'boss_spawned'.")]
            public string customEventKey;

            [Tooltip("Optional UI screen id used when triggerType = OnOpenUI. Example: 'shop', 'settings'.")]
            public string uiId;

            [Tooltip("Optional condition key for your game (ex: 'level>=5', 'coins<100'). Runtime interprets this.")]
            public string conditionKey;

            [Header("Content")]
            [Tooltip("How the tutorial is presented. Runtime UI layer decides actual visuals.")]
            public DisplayType displayType = DisplayType.Popup;

            [Tooltip("Optional localization key or message id used by your localization system.")]
            public string messageKey = "TUTORIAL_FIRST_TIME";

            [Tooltip("Optional extra asset referenced by UI (sprite/prefab/scriptable).")]
            public UnityEngine.Object content;

            [Tooltip("If true, tutorial will only play once per profile (runtime should persist completion).")]
            public bool playOnce = true;

            [Tooltip("Optional priority for selection ordering if multiple tutorials match at once.")]
            public int priority = 0;

            [TextArea(2, 6)]
            [Tooltip("Designer note.")]
            public string note;
        }

        public IEnumerable<string> Validate()
        {
            if (tutorials == null) yield break;

            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < tutorials.Count; i++)
            {
                var t = tutorials[i];
                if (t == null)
                {
                    yield return $"[Tutorials] tutorials[{i}] is null.";
                    continue;
                }

                var id = (t.id ?? "").Trim();
                if (string.IsNullOrEmpty(id))
                    yield return $"[Tutorials] tutorials[{i}] id is empty.";
                else if (!set.Add(id))
                    yield return $"[Tutorials] Duplicate id: '{id}'.";

                if (t.triggerType == TriggerType.CustomEvent && string.IsNullOrWhiteSpace(t.customEventKey))
                    yield return $"[Tutorials] '{id}' requires customEventKey (TriggerType.CustomEvent).";

                if (t.triggerType == TriggerType.OnOpenUI && string.IsNullOrWhiteSpace(t.uiId))
                    yield return $"[Tutorials] '{id}' requires uiId (TriggerType.OnOpenUI).";

                if (string.IsNullOrWhiteSpace(t.messageKey))
                    yield return $"[Tutorials] '{id}' messageKey is empty (recommended to set).";
            }
        }
    }
}