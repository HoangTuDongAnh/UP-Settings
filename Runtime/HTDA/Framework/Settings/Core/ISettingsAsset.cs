namespace HTDA.Framework.Settings.Core
{
    /// <summary>
    /// Marker interface for all settings assets in HTDA settings system.
    /// </summary>
    public interface ISettingsAsset
    {
        string SettingsId { get; }
        string SettingsVersion { get; }
    }
}