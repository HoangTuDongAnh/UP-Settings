using System;
using System.Collections.Generic;
using UnityEngine;
using HTDA.Framework.Settings.Core;

namespace HTDA.Framework.Settings.Audio
{
    [CreateAssetMenu(menuName = "HTDA/Settings/Audio Settings", fileName = "AudioSettings")]
    public sealed class AudioSettingsAsset : SettingsAssetBase, IValidatableSettings
    {
        [Tooltip("Audio database. Each item is addressable by a string id (ex: \"ui_click\", \"gameover\").")]
        public List<AudioItem> items = new List<AudioItem>();

        [Serializable]
        public class AudioItem
        {
            [Tooltip("Unique key used by runtime audio service. Prefer lowercase_with_underscores.")]
            public string id = "gameover";

            [Tooltip("Base volume (0..1).")]
            [Range(0f, 1f)] public float volume = 1f;

            [Tooltip("Random volume amount (0..1). Final volume = volume ± randomVolume.")]
            [Range(0f, 1f)] public float randomVolume = 0f;

            [Tooltip("Random pitch amount (0..1). Final pitch = 1 ± randomPitch.")]
            [Range(0f, 1f)] public float randomPitch = 0f;

            [Tooltip("Minimum time between calls (seconds) to avoid spam.")]
            [Min(0f)] public float minTimeBetweenCall = 0f;

            [Tooltip("Optional range parameter for spatial / effect usage (meaning depends on your runtime).")]
            [Min(0f)] public float range = 0f;

            [Tooltip("Loop audio clip when played.")]
            public bool loop = false;

            [Tooltip("AudioClip reference.")]
            public AudioClip clip;
        }

        public IEnumerable<string> Validate()
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < items.Count; i++)
            {
                var it = items[i];
                if (it == null)
                {
                    yield return $"[Audio] items[{i}] is null.";
                    continue;
                }

                var id = (it.id ?? "").Trim();
                if (string.IsNullOrEmpty(id))
                {
                    yield return $"[Audio] items[{i}] id is empty.";
                }
                else if (!set.Add(id))
                {
                    yield return $"[Audio] Duplicate id: '{id}'.";
                }

                if (it.clip == null)
                    yield return $"[Audio] items[{i}] '{id}' clip is missing.";
            }
        }
    }
}