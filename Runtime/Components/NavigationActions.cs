using Cysharp.Threading.Tasks;
using UExtension.Navigation.RouteFactory;
using UExtension.Navigation.RouteFactory.Tab;
using UnityEngine;

namespace UExtension.Navigation.Components
{
    public class NavigationActions : ScriptableObject
    {
        public void Push(AbstractRouteFactory route)
        {
            NavigationService.Push(route).Forget();
        }

        public void Pop()
        {
            NavigationService.Pop().Forget();
        }

        public void Navigate(AbstractRouteFactory route)
        {
            NavigationService.Navigate(route).Forget();
        }

        public void SetTab(TabRouteFactory tabRouteFactory, AbstractRouteFactory tabFactory)
        {
            NavigationService.SetTab(tabRouteFactory, tabFactory).Forget();
        }

        public void Replace(AbstractRouteFactory route)
        {
            NavigationService.Replace(route).Forget();
        }

        public void Root(AbstractRouteFactory route)
        {
            NavigationService.Root(route).Forget();
        }

        public void Reload()
        {
            NavigationService.Reload().Forget();
        }
    }
}