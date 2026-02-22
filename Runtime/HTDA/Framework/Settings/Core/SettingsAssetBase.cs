using UnityEngine;

namespace HTDA.Framework.Settings.Core
{
    /// <summary>
    /// Base class for all settings ScriptableObjects.
    /// Keeps minimal metadata and consistency across features.
    /// </summary>
    public abstract class SettingsAssetBase : ScriptableObject, ISettingsAsset
    {
        [SerializeField] private string settingsId = "settings.asset";
        [SerializeField] private string settingsVersion = "0.1.0";

        [TextArea(2, 6)]
        [SerializeField] private string notes;

        public string SettingsId => settingsId;
        public string SettingsVersion => settingsVersion;
        public string Notes => notes;

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            // Keep id non-empty
            if (string.IsNullOrEmpty(settingsId))
                settingsId = GetType().Name;
        }
#endif
    }
}