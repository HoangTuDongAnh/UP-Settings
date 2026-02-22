#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace HTDA.Framework.Settings.Editor.Core
{
    internal static class SettingsPageRegistry
    {
        private static readonly List<ISettingsPage> _pages = new List<ISettingsPage>(32);
        private static bool _initialized;

        public static IReadOnlyList<ISettingsPage> Pages
        {
            get
            {
                EnsureInitialized();
                return _pages;
            }
        }

        public static void Refresh()
        {
            _initialized = false;
            _pages.Clear();
        }

        private static void EnsureInitialized()
        {
            if (_initialized) return;
            _initialized = true;

            _pages.Clear();

            // Find all types in current AppDomain that:
            // - implement ISettingsPage
            // - have [SettingsPage]
            // - have parameterless ctor
            var types = TypeCache.GetTypesDerivedFrom<ISettingsPage>();

            foreach (var t in types)
            {
                if (t.IsAbstract) continue;
                if (t.GetCustomAttribute<SettingsPageAttribute>() == null) continue;
                if (t.GetConstructor(Type.EmptyTypes) == null) continue;

                try
                {
                    var page = (ISettingsPage)Activator.CreateInstance(t);
                    if (page != null) _pages.Add(page);
                }
                catch
                {
                    // ignore broken pages to avoid crashing the whole window
                }
            }

            _pages.Sort((a, b) =>
            {
                var c = a.Order.CompareTo(b.Order);
                if (c != 0) return c;
                return string.Compare(a.DisplayName, b.DisplayName, StringComparison.OrdinalIgnoreCase);
            });
        }
    }
}
#endif