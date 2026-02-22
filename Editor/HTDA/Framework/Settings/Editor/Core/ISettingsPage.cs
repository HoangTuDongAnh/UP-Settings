#if UNITY_EDITOR
namespace HTDA.Framework.Settings.Editor.Core
{
    public interface ISettingsPage
    {
        /// <summary>Stable id, e.g. "global", "audio".</summary>
        string Id { get; }

        /// <summary>Shown in sidebar.</summary>
        string DisplayName { get; }

        /// <summary>Sorting order in sidebar.</summary>
        int Order { get; }

        /// <summary>Title shown in header.</summary>
        string Title { get; }

        /// <summary>Draw page UI (IMGUI).</summary>
        void OnGUI();
    }
}
#endif