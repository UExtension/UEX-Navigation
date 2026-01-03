using System.Collections.Generic;
using UExtension.Navigation;
using UExtension.Navigation.Route;
using UExtension.Navigation.Route.Stack;
using UExtension.Navigation.Route.Tab;
using UI.Atoms;
using UnityEngine.UIElements;

namespace UI
{
    [UxmlElement]
    public partial class NavigationHistory : VisualElement
    {
        private readonly List<VisualElement> _rows = new();
        private readonly VisualElement _arrowContainer = new();

        public NavigationHistory()
        {
            AddToClassList("navigation-history");

            NavigationService.OnRouteLoadEnd += HandleRouteChanged;

            GenerateHistory();
        }

        private void HandleRouteChanged(IRoute obj)
        {
            GenerateHistory();
        }

        public void GenerateHistory()
        {
            Clear();
            _rows.Clear();
            _arrowContainer.Clear();

            _arrowContainer.AddToClassList("navigation-history__arrows");
            Add(_arrowContainer);

            if (NavigationService.IsRouteReady())
            {
                CreateRoute(NavigationService.GetRoot());
            }
        }

        private RouteUI CreateRoute(IRoute route)
        {
            var routeUI = new RouteUI(route);

            GetRow(route.Depth).Add(routeUI);

            switch (route)
            {
                case StackRoute stackRoute:
                    CreateStackRouteUI(stackRoute, routeUI);
                    break;
                case TabRoute tabRoute:
                {
                    CreateTabRouteUI(tabRoute, routeUI);
                    break;
                }
            }

            return routeUI;
        }

        private VisualElement GetRow(int depth)
        {
            VisualElement row;
            if (_rows.Count > depth)
            {
                row = _rows[depth];
            }
            else
            {
                row = new VisualElement();
                row.AddToClassList("navigation-history__row");
                Add(row);
                _rows.Add(row);
            }

            return row;
        }

        private void CreateStackRouteUI(StackRoute route, RouteUI routeUI)
        {
            if (!route.HasNext()) return;

            var nextRouteUI = CreateRoute(route.Next);
            var arrow = new ArrowUI(routeUI, nextRouteUI, route.IsActive);
            _arrowContainer.Add(arrow);
        }

        private void CreateTabRouteUI(TabRoute route, RouteUI routeUI)
        {
            route.GetTabs().ForEach(tab =>
            {
                var nextRouteUI = CreateRoute(tab);
                var arrow = new ArrowUI(routeUI, nextRouteUI, tab.IsActive);
                _arrowContainer.Add(arrow);
            });
        }
    }
}