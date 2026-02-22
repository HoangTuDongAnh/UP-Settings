#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HTDA.Framework.Settings.Editor.Core
{
    public sealed class GameSettingsWindow : EditorWindow
    {
        private const float SidebarWidth = 180f;
        private Vector2 _sidebarScroll;
        private Vector2 _contentScroll;

        private int _selectedIndex = -1;

        [MenuItem("HTDA/Settings/Game Settings", priority = 10)]
        public static void Open()
        {
            var w = GetWindow<GameSettingsWindow>("Game Settings");
            w.minSize = new Vector2(900, 520);
            w.Show();
        }

        private void OnEnable()
        {
            // Ensure registry is built
            var pages = SettingsPageRegistry.Pages;
            if (_selectedIndex < 0 && pages.Count > 0)
                _selectedIndex = 0;
        }

        private void OnGUI()
        {
            DrawLayout();
        }

        private void DrawLayout()
        {
            var pages = SettingsPageRegistry.Pages;

            using (new EditorGUILayout.HorizontalScope())
            {
                DrawSidebar(pages);
                DrawContent(pages);
            }
        }

        private void DrawSidebar(System.Collections.Generic.IReadOnlyList<ISettingsPage> pages)
        {
            using (new GUILayout.VerticalScope(GUILayout.Width(SidebarWidth)))
            {
                DrawSidebarTop();

                _sidebarScroll = EditorGUILayout.BeginScrollView(_sidebarScroll);
                for (int i = 0; i < pages.Count; i++)
                {
                    var isSelected = (i == _selectedIndex);
                    var style = isSelected ? SettingsUIStyles.SidebarItemSelected : SettingsUIStyles.SidebarItem;

                    var rect = GUILayoutUtility.GetRect(new GUIContent(pages[i].DisplayName), style, GUILayout.ExpandWidth(true));

                    // background for selected
                    if (Event.current.type == EventType.Repaint)
                    {
                        var bg = isSelected ? SettingsUIStyles.SidebarSelectedBg : SettingsUIStyles.SidebarBg;
                        EditorGUI.DrawRect(rect, bg);
                    }

                    if (GUI.Button(rect, pages[i].DisplayName, style))
                    {
                        _selectedIndex = i;
                        GUI.FocusControl(null);
                    }
                }
                EditorGUILayout.EndScrollView();

                GUILayout.FlexibleSpace();

                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
                {
                    if (GUILayout.Button("Refresh Pages"))
                    {
                        SettingsPageRegistry.Refresh();
                        var newPages = SettingsPageRegistry.Pages;
                        if (newPages.Count > 0) _selectedIndex = Mathf.Clamp(_selectedIndex, 0, newPages.Count - 1);
                        else _selectedIndex = -1;
                    }

                    if (GUILayout.Button("Ping Package"))
                    {
                        // Try ping package root (best effort)
                        var thisScript = MonoScript.FromScriptableObject(this);
                        if (thisScript != null) EditorGUIUtility.PingObject(thisScript);
                    }
                }
            }
        }

        private void DrawSidebarTop()
        {
            var rect = GUILayoutUtility.GetRect(10, 36, GUILayout.ExpandWidth(true));
            EditorGUI.DrawRect(rect, SettingsUIStyles.SidebarTopColor);
            GUI.Label(rect, "SETTINGS", new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white }
            });
        }

        private void DrawContent(System.Collections.Generic.IReadOnlyList<ISettingsPage> pages)
        {
            using (new GUILayout.VerticalScope(GUILayout.ExpandWidth(true)))
            {
                var page = GetSelectedPage(pages);

                DrawHeaderBar(page?.Title ?? "No Page");

                _contentScroll = EditorGUILayout.BeginScrollView(_contentScroll);
                GUILayout.Space(8);

                if (page == null)
                {
                    EditorGUILayout.HelpBox("No settings pages found.\n\nCreate a page class implementing ISettingsPage and mark it with [SettingsPage].", MessageType.Info);
                }
                else
                {
                    page.OnGUI();
                }

                GUILayout.Space(12);
                EditorGUILayout.EndScrollView();
            }
        }

        private void DrawHeaderBar(string title)
        {
            var rect = GUILayoutUtility.GetRect(10, 34, GUILayout.ExpandWidth(true));
            EditorGUI.DrawRect(rect, SettingsUIStyles.HeaderBarColor);
            GUI.Label(rect, title, SettingsUIStyles.HeaderTitle);
        }

        private ISettingsPage GetSelectedPage(System.Collections.Generic.IReadOnlyList<ISettingsPage> pages)
        {
            if (pages == null || pages.Count == 0) return null;
            if (_selectedIndex < 0) _selectedIndex = 0;
            if (_selectedIndex >= pages.Count) _selectedIndex = pages.Count - 1;
            return pages[_selectedIndex];
        }
    }
}
#endif