using UExtension.Bootstrap.UI;
using UExtension.Navigation.Editor.UI.Molecules;
using UnityEngine.UIElements;

namespace UExtension.Navigation.Editor.UI
{
    [UxmlElement]
    public partial class NavigationHistoryColumnView : VisualElement, IUExtensionTabFactory
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

        public int Order => 2;

        public Tab Create()
        {
            var tab = new Tab("History");
            tab.AddToClassList("navigation-tab");
            tab.Add(new NavigationHistoryColumnView());
            return tab;
        }
    }
}