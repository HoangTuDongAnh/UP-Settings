#if UNITY_EDITOR
using System;

namespace HTDA.Framework.Settings.Editor.Core
{
    /// <summary>
    /// Mark a class as a settings page so EditorCore can discover it.
    /// Class must implement ISettingsPage and have a parameterless constructor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SettingsPageAttribute : Attribute
    {
    }
}
#endif