using System;
using System.Collections.Generic;
using UnityEngine;
using HTDA.Framework.Settings.Core;

namespace HTDA.Framework.Settings.Credits
{
    [CreateAssetMenu(menuName = "HTDA/Settings/Credits Settings", fileName = "CreditsSettings")]
    public sealed class CreditsSettingsAsset : SettingsAssetBase, IValidatableSettings
    {
        [Tooltip("Credits entries displayed in your Credits screen.")]
        public List<Entry> entries = new();

        [Serializable]
        public class Entry
        {
            [Tooltip("Person / team name.")]
            public string name;

            [Tooltip("Role / contribution.")]
            public string role;

            [Tooltip("Optional URL (portfolio, company site).")]
            public string url;

            [Tooltip("Sorting order (lower first).")]
            public int order = 0;
        }

        public IEnumerable<string> Validate()
        {
            for (int i = 0; i < entries.Count; i++)
            {
                var e = entries[i];
                if (e == null) { yield return $"[Credits] entries[{i}] is null."; continue; }
                if (string.IsNullOrWhiteSpace(e.name))
                    yield return $"[Credits] entries[{i}] name is empty.";
            }
        }
    }
}