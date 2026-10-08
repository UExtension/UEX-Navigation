using System;
using System.Collections.Generic;
using UExtension.Navigation.Route;
using UExtension.Navigation.Route.Tab;
using UExtension.Navigation.RouteFactory;
using UExtension.Navigation.RouteFactory.Tab;
using UExtension.Navigation.Router;
using UExtension.Navigation.Router.Combined;
using UExtension.Navigation.RouterFactory;
using UniTask = Cysharp.Threading.Tasks.UniTask;

namespace UExtension.Navigation
{
    public static class NavigationService

    {
        public static readonly CombinedRouter combinedRouter = new();

        private static bool _logging = true;

        public static bool logging
        {
            get => _logging;
            set
            {
                _logging = value;
                foreach (var router in combinedRouter.routers) router.logging = value;
            }
        }

        public static event Action<ISet<IRouter>> OnRouteLoadStart
        {
            add => combinedRouter.OnRouteLoadStart += value;
            remove => combinedRouter.OnRouteLoadStart -= value;
        }

        public static event Action<ISet<IRouter>> OnRouteLoadEnd
        {
            add => combinedRouter.OnRouteLoadEnd += value;
            remove => combinedRouter.OnRouteLoadEnd -= value;
        }

        /// <inheritdoc cref="IRoute.Push"/>
        public static UniTask Push(params RouterRouteFactory[] routeFactories) => Push(routeFactories.ToRouterRoutes());

        /// <inheritdoc cref="IRoute.Push"/>
        public static UniTask Push(params RouterRoute[] routes)
        {
            foreach (var tuple in routes) combinedRouter.SetCurrentRouter(tuple.router).Push(tuple.route);
            return combinedRouter.Load();
        }

        /// <inheritdoc cref="IRoute.Pop()"/>
        public static UniTask Pop() => Pop(combinedRouter.GetRouterRoutes());

        /// <inheritdoc cref="IRoute.Pop(IRoute)"/>
        public static UniTask Pop(params RouterRoute[] routes)
        {
            foreach (var tuple in routes) combinedRouter.SetCurrentRouter(tuple.router).Pop(tuple.route);
            return combinedRouter.Load();
        }

        /// <inheritdoc cref="IRoute.Navigate"/>
        public static UniTask Navigate(params RouterRouteFactory[] routeFactories) => Navigate(routeFactories.ToRouterRoutes());

        /// <summary>
        /// <inheritdoc cref="IRoute.Navigate"/>
        /// </summary>
        public static UniTask Navigate(params RouterRoute[] routes)
        {
            foreach (var tuple in routes) combinedRouter.SetCurrentRouter(tuple.router).Navigate(tuple.route);
            return combinedRouter.Load();
        }

        /// <inheritdoc cref="SetTab(UExtension.Navigation.Route.Tab.TabRoute,UExtension.Navigation.Route.IRoute)"/>
        public static UniTask SetTab(TabRouteFactory tabRouteFactory, IRouteFactory tabFactory) => SetTab(tabRouteFactory.CreateTyped(), tabFactory.Create());

        /// <summary>
        /// Updates the active tab of the given <see cref="TabRoute"/> and navigates to it. If the given <see cref="TabRoute"/> isn't found in history, it will be pushed on top.
        /// </summary>
        /// <param name="tabRoute">The route to navigate to.</param>
        /// <param name="tab">The tab to set active.</param>
        public static UniTask SetTab(TabRoute tabRoute, IRoute tab) => combinedRouter.SetTab(tabRoute, tab).Load();

        /// <inheritdoc cref="IRoute.GetRoot"/>
        public static IRoute GetRoot() => combinedRouter.GetRoot();

        /// <inheritdoc cref="IRoute.GetTip"/>
        public static IRoute GetTip() => combinedRouter.GetTip();

        /// <inheritdoc cref="IRoute.Search"/>
        public static IRoute Search(IRouteFactory routeFactory) => Search(routeFactory.Create());

        /// <inheritdoc cref="IRoute.Search"/>
        public static IRoute Search(IRoute route) => combinedRouter.Search(route);

        /// <inheritdoc cref="Replace(IRouteFactory)"/>
        public static UniTask Replace(params RouterRouteFactory[] routeFactories) => Replace(routeFactories.ToRouterRoutes());

        /// <summary>
        /// Replaces the tip of the current route stack by the given route.
        /// </summary>
        /// <param name="route">The route replacing the tip.</param>
        public static UniTask Replace(params RouterRoute[] routes)
        {
            foreach (var tuple in routes) combinedRouter.SetCurrentRouter(tuple.router).Replace(tuple.route);
            return combinedRouter.Load();
        }

        /// <inheritdoc cref="Root(IRouteFactory)"/>
        public static UniTask Root(params RouterRouteFactory[] routeFactories) => Root(routeFactories.ToRouterRoutes());

        /// <summary>
        /// Replaces the current root and its history by the given route.
        /// </summary>
        /// <param name="route">The route replacing the root.</param>
        public static UniTask Root(params RouterRoute[] routes)
        {
            foreach (var tuple in routes) combinedRouter.SetCurrentRouter(tuple.router).Root(tuple.route);
            return combinedRouter.Load();
        }

        /// <summary>
        /// Asks the <see cref="SceneLoader"/> to reload the current route tip.
        /// </summary>
        public static UniTask Reload() => combinedRouter.Load();

        public static bool IsRouteReady() => combinedRouter.IsRouteReady();
    }
}