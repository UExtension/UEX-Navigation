using UnityEngine.UIElements;

namespace UI
{
    [UxmlElement]
    public partial class NavigationHistoryColumnView : VisualElement
    {
        public NavigationHistoryColumnView()
        {
            AddToClassList("navigation-history-column-view");

            var routesListView = new SearchableRoutesListView();
            var navigationHistory = new NavigationHistory();
            
            var scrollView = new ScrollView();
            scrollView.AddToClassList("navigation-history-column-view__scroll-view");
            scrollView.Add(navigationHistory);

            Add(routesListView);
            Add(scrollView);
        }
    }
}