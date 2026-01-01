using System;
using Cysharp.Threading.Tasks;
using UExtension.Navigation.Route;
using UExtension.Navigation.Route.Tab;
using UExtension.Navigation.RouteFactory;
using UExtension.Navigation.RouteFactory.Tab;
using UnityEngine;

namespace UExtension.Navigation
{
    public static class NavigationService
    {
        private const string LoggingPrefix = "<color=#DE00C8>[Navigation]</color>";

        private static IRoute _route;

        public static bool Logging { get; set; } = true;

        public static event Action<IRoute> OnRouteLoadStart = delegate {};
        public static event Action<IRoute> OnRouteLoadEnd = delegate {};

        /// <inheritdoc cref="IRoute.Push"/>
        public static async UniTask Push(IRouteFactory routeFactory)
        {
            await Push(routeFactory.Create());
        }

        /// <inheritdoc cref="IRoute.Push"/>
        public static async UniTask Push(IRoute route)
        {
            _route = _route.Push(route);

            if (Logging)
            {
                Debug.Log($"{LoggingPrefix} Push: {route.Name}");
                Debug.Log($"{LoggingPrefix} {_route.GetRoot().ToString()}");
            }

            await LoadRouteAsync(_route.GetTip());
        }

        /// <summary>
        /// Removes the tip of the current route stack.
        /// </summary>
        public static async UniTask Pop()
        {
            if (Logging)
            {
                Debug.Log($"{LoggingPrefix} Pop: {_route.Name}");
            }

            _route = _route.Pop();

            if (Logging)
            {
                Debug.Log($"{LoggingPrefix} {_route.GetRoot().ToString()}");
            }

            await LoadRouteAsync(_route.GetTip());
        }

        /// <summary>
        /// <inheritdoc cref="IRoute.Navigate"/>
        /// </summary>
        public static async UniTask Navigate(IRouteFactory routeFactory)
        {
            await Navigate(routeFactory.Create());
        }

        /// <summary>
        /// <inheritdoc cref="IRoute.Navigate"/>
        /// </summary>
        public static async UniTask Navigate(IRoute route)
        {
            _route = _route.Navigate(route);

            if (Logging)
            {
                Debug.Log($"{LoggingPrefix} Navigate: {route.Name}");
                Debug.Log($"{LoggingPrefix} {_route.GetRoot().ToString()}");
            }

            await LoadRouteAsync(_route.GetTip());
        }

        /// <inheritdoc cref="SetTab(UExtension.Navigation.Route.Tab.TabRoute,UExtension.Navigation.Route.IRoute)"/>
        public static async UniTask SetTab(TabRouteFactory tabRouteFactory, IRouteFactory tabFactory)
        {
            await SetTab(tabRouteFactory.CreateTyped(), tabFactory.Create());
        }

        /// <summary>
        /// Updates the active tab of the given <see cref="TabRoute"/> and navigates to it. If the given <see cref="TabRoute"/> isn't found in history, it will be pushed on top.
        /// </summary>
        /// <param name="tabRoute">The route to navigate to.</param>
        /// <param name="tab">The tab to set active.</param>
        public static async UniTask SetTab(TabRoute tabRoute, IRoute tab)
        {
            tabRoute.ActiveTab = tab;
            await Navigate(tabRoute);
        }

        /// <inheritdoc cref="IRoute.GetRoot"/>
        public static IRoute GetRoot()
        {
            return _route.GetRoot();
        }

        /// <inheritdoc cref="IRoute.GetTip"/>
        public static IRoute GetTip()
        {
            return _route.GetTip();
        }

        /// <inheritdoc cref="IRoute.Search"/>
        public static IRoute Search(IRouteFactory routeFactory)
        {
            return Search(routeFactory.Create());
        }

        /// <inheritdoc cref="IRoute.Search"/>
        public static IRoute Search(IRoute route)
        {
            return _route.Search(route);
        }

        /// <inheritdoc cref="Replace(IRouteFactory)"/>
        public static async UniTask Replace(IRouteFactory routeFactory)
        {
            await Replace(routeFactory.Create());
        }

        /// <summary>
        /// Replaces the tip of the current route stack by the given route.
        /// </summary>
        /// <param name="route">The route replacing the tip.</param>
        public static async UniTask Replace(IRoute route)
        {
            _route = _route.Pop();
            _route = _route.Push(route);

            if (Logging)
            {
                Debug.Log($"{LoggingPrefix} Replace: {route.Name}");
                Debug.Log($"{LoggingPrefix} {_route.GetRoot().ToString()}");
            }

            await LoadRouteAsync(_route.GetTip());
        }

        /// <inheritdoc cref="Root(IRouteFactory)"/>
        public static async UniTask Root(IRouteFactory routeFactory)
        {
            await Root(routeFactory.Create());
        }

        /// <summary>
        /// Replaces the current root and its history by the given route.
        /// </summary>
        /// <param name="route">The route replacing the root.</param>
        public static async UniTask Root(IRoute route)
        {
            route.Previous = null;
            _route = route.GetTip();

            if (Logging)
            {
                Debug.Log($"{LoggingPrefix} Root: {route.Name}");
                Debug.Log($"{LoggingPrefix} {_route.GetRoot().ToString()}");
            }

            await LoadRouteAsync(_route.GetTip());
        }

        /// <summary>
        /// Asks the <see cref="SceneLoader"/> to reload the current route tip.
        /// </summary>
        public static async UniTask Reload()
        {
            if (Logging)
            {
                Debug.Log($"{LoggingPrefix} Reload: {_route.GetTip().Name}");
                Debug.Log($"{LoggingPrefix} {_route.GetRoot().ToString()}");
            }

            await LoadRouteAsync(_route.GetTip());
        }

        public static bool IsRouteReady()
        {
            return _route != null;
        }

        /// <summary>
        /// Loads the specified route's scene group asynchronously.
        /// </summary>
        /// <param name="route">The route whose associated scene group will be loaded.</param>
        /// <returns>A task that represents the asynchronous operation of loading the scene group.</returns>
        private static async UniTask LoadRouteAsync(IRoute route)
        {
            OnRouteLoadStart.Invoke(route);
            await SceneLoader.SceneLoader.SetActiveSceneContainers(route.Scenes);

            if (route.ActiveScene != null)
            {
                SceneLoader.SceneLoader.SetActiveScene(route.ActiveScene);
            }

            if (route.BakingSetActiveScene != null)
            {
                SceneLoader.SceneLoader.SetBakingSet(route.BakingSetActiveScene);
            }

            OnRouteLoadEnd.Invoke(route);
        }
    }
}