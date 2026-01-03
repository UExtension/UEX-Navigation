using System.Collections.Generic;
using UExtension.Navigation.Editor.UI.Atoms;
using UExtension.Navigation.Route;
using UExtension.Navigation.Route.Stack;
using UExtension.Navigation.Route.Tab;
using UnityEngine.UIElements;

namespace UExtension.Navigation.Editor.UI.Views
{
    [UxmlElement]
    public partial class NavigationHistoryView : VisualElement
    {
        private readonly List<VisualElement> _rows = new();
        private readonly VisualElement _arrowContainer = new();

        public NavigationHistoryView()
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

        private RouteAtom CreateRoute(IRoute route)
        {
            var routeUI = new RouteAtom(route);

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

        private void CreateStackRouteUI(StackRoute route, RouteAtom routeAtom)
        {
            if (!route.HasNext()) return;

            var nextRouteUI = CreateRoute(route.Next);
            var arrow = new ArrowAtom(routeAtom, nextRouteUI, route.IsActive);
            _arrowContainer.Add(arrow);
        }

        private void CreateTabRouteUI(TabRoute route, RouteAtom routeAtom)
        {
            route.GetTabs().ForEach(tab =>
            {
                var nextRouteUI = CreateRoute(tab);
                var arrow = new ArrowAtom(routeAtom, nextRouteUI, tab.IsActive);
                _arrowContainer.Add(arrow);
            });
        }
    }
}