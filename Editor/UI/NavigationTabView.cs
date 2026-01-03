using UnityEngine.UIElements;

namespace UI
{
    [UxmlElement]
    public partial class NavigationTabView : VisualElement
    {
        public NavigationTabView()
        {
            AddToClassList("navigation-tab-view");

            var tabView = new TabView();
            tabView.AddToClassList("navigation-tab-view__tab-view");
            Add(tabView);

            var scenesTab = new Tab("Scenes");
            scenesTab.AddToClassList("navigation-tab");
            var routesTab = new Tab("Routes");
            routesTab.AddToClassList("navigation-tab");
            var historyTab = new Tab("History");
            historyTab.AddToClassList("navigation-tab");

            scenesTab.Add(new SearchableScenesListView());
            routesTab.Add(new SearchableRoutesListView());
            historyTab.Add(new NavigationHistoryColumnView());

            tabView.Add(scenesTab);
            tabView.Add(routesTab);
            tabView.Add(historyTab);
        }
    }
}