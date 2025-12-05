using UExtension.Navigation.RouteFactory;
using UnityEngine;

namespace UExtension.Navigation.Components
{
    public class NavigationActions : ScriptableObject
    {
        public void Push(AbstractRouteFactory route)
        {
            _ = NavigationService.Push(route);
        }

        public void Navigate(AbstractRouteFactory route)
        {
            _ = NavigationService.Navigate(route);
        }

        public void Pop()
        {
            _ = NavigationService.Pop();
        }

        public void Replace(AbstractRouteFactory route)
        {
            _ = NavigationService.Replace(route);
        }

        public void Root(AbstractRouteFactory route)
        {
            _ = NavigationService.Root(route);
        }
    }
}