using UnityEditor;
using UnityEngine.UIElements;

namespace UExtension.Navigation.Editor.UI.Views
{
    [UxmlElement]
    public partial class NavigationHistoryColumnView : VisualElement
    {
        public SearchableRoutesListView RoutesListView { get; }

        public NavigationHistoryColumnView()
        {
            AddToClassList("navigation-history-column-view");

            RoutesListView = new SearchableRoutesListView();
            Add(RoutesListView);

            if (EditorApplication.isPlaying)
            {
                AddNavigationHistoryView();
                RoutesListView.AddToClassList("searchable-routes-list-view--side");
            }

            EditorApplication.playModeStateChanged += HandlePlayModeStatedChanged;
        }

        private void HandlePlayModeStatedChanged(PlayModeStateChange obj)
        {
            switch (obj)
            {
                case PlayModeStateChange.EnteredEditMode:
                    if (childCount > 1)
                    {
                        RemoveAt(1);
                        RoutesListView.RemoveFromClassList("searchable-routes-list-view--side");
                    }

                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    if (childCount < 2)
                    {
                        AddNavigationHistoryView();
                        RoutesListView.AddToClassList("searchable-routes-list-view--side");
                    }

                    break;
            }
        }

        private void AddNavigationHistoryView()
        {
            var scrollView = new ScrollView();
            scrollView.AddToClassList("navigation-history-column-view__scroll-view");
            scrollView.Add(new NavigationHistoryView());
            Add(scrollView);
        }
    }
}