using System;
using Cysharp.Threading.Tasks;
using UExtension.Navigation.Route;
using UExtension.Navigation.Route.Tab;
using UExtension.Navigation.RouteFactory;
using UExtension.Navigation.RouteFactory.Tab;

namespace UExtension.Navigation.Router
{
    public interface IRouter : IEquatable<IRouter>
    {
        string name { get; }
        bool logging { get; set; }

        /// <inheritdoc cref="IRoute.Push"/>
        IRouter Push(IRouteFactory routeFactory);

        /// <inheritdoc cref="IRoute.Push"/>
        IRouter Push(IRoute route);

        /// <inheritdoc cref="IRoute.Pop()"/>
        IRouter Pop();

        /// <inheritdoc cref="IRoute.Pop(IRoute)"/>
        IRouter Pop(IRoute route);

        /// <inheritdoc cref="IRoute.Navigate"/>
        IRouter Navigate(IRouteFactory routeFactory);

        /// <summary>
        /// <inheritdoc cref="IRoute.Navigate"/>
        /// </summary>
        IRouter Navigate(IRoute route);

        /// <inheritdoc cref="SetTab(UExtension.Navigation.Route.Tab.TabRoute,UExtension.Navigation.Route.IRoute)"/>
        IRouter SetTab(TabRouteFactory tabRouteFactory, IRouteFactory tabFactory);

        /// <summary>
        /// Updates the active tab of the given <see cref="TabRoute"/> and navigates to it. If the given <see cref="TabRoute"/> isn't found in history, it will be pushed on top.
        /// </summary>
        /// <param name="tabRoute">The route to navigate to.</param>
        /// <param name="tab">The tab to set active.</param>
        IRouter SetTab(TabRoute tabRoute, IRoute tab);

        /// <inheritdoc cref="IRoute.GetRoot"/>
        IRoute GetRoot();

        /// <inheritdoc cref="IRoute.GetTip"/>
        IRoute GetTip();

        /// <inheritdoc cref="IRoute.Search"/>
        IRoute Search(IRouteFactory routeFactory);

        /// <inheritdoc cref="IRoute.Search"/>
        IRoute Search(IRoute route);

        /// <inheritdoc cref="Replace(IRouteFactory)"/>
        IRouter Replace(IRouteFactory routeFactory);

        /// <summary>
        /// Replaces the tip of the current route stack by the given route.
        /// </summary>
        /// <param name="route">The route replacing the tip.</param>
        IRouter Replace(IRoute route);

        /// <inheritdoc cref="Root(IRouteFactory)"/>
        IRouter Root(IRouteFactory routeFactory);

        /// <summary>
        /// Replaces the current root and its history by the given route.
        /// </summary>
        /// <param name="route">The route replacing the root.</param>
        IRouter Root(IRoute route);

        bool IsRouteReady();

        /// <summary>
        /// Asks the <see cref="SceneLoader"/> to load the current route tip.
        /// </summary>
        UniTask Load();

        /// <summary>
        /// Cancels the current route loading.
        /// </summary>
        void CancelLoad();
    }

    public interface IRouter<out T> : IRouter where T : IRouter<T>
    {
        /// <inheritdoc cref="IRoute.Push"/>
        new T Push(IRouteFactory routeFactory);

        IRouter IRouter.Push(IRouteFactory routeFactory) => Push(routeFactory);

        /// <inheritdoc cref="IRoute.Push"/>
        new T Push(IRoute route);

        IRouter IRouter.Push(IRoute route) => Push(route);

        /// <inheritdoc cref="IRoute.Pop()"/>
        new T Pop();

        IRouter IRouter.Pop() => Pop();

        /// <inheritdoc cref="IRoute.Pop(IRoute)"/>
        new T Pop(IRoute route);

        IRouter IRouter.Pop(IRoute route) => Pop(route);

        /// <inheritdoc cref="IRoute.Navigate"/>
        new T Navigate(IRouteFactory routeFactory);

        IRouter IRouter.Navigate(IRouteFactory routeFactory) => Navigate(routeFactory);

        /// <summary>
        /// <inheritdoc cref="IRoute.Navigate"/>
        /// </summary>
        new T Navigate(IRoute route);

        IRouter IRouter.Navigate(IRoute route) => Navigate(route);

        /// <inheritdoc cref="SetTab(UExtension.Navigation.Route.Tab.TabRoute,UExtension.Navigation.Route.IRoute)"/>
        new T SetTab(TabRouteFactory tabRouteFactory, IRouteFactory tabFactory);

        IRouter IRouter.SetTab(TabRouteFactory tabRouteFactory, IRouteFactory tabFactory) => SetTab(tabRouteFactory, tabFactory);

        /// <summary>
        /// Updates the active tab of the given <see cref="TabRoute"/> and navigates to it. If the given <see cref="TabRoute"/> isn't found in history, it will be pushed on top.
        /// </summary>
        /// <param name="tabRoute">The route to navigate to.</param>
        /// <param name="tab">The tab to set active.</param>
        new T SetTab(TabRoute tabRoute, IRoute tab);

        IRouter IRouter.SetTab(TabRoute tabRoute, IRoute tab) => SetTab(tabRoute, tab);

        /// <inheritdoc cref="Replace(IRouteFactory)"/>
        new T Replace(IRouteFactory routeFactory);

        IRouter IRouter.Replace(IRouteFactory routeFactory) => Replace(routeFactory);

        /// <summary>
        /// Replaces the tip of the current route stack by the given route.
        /// </summary>
        /// <param name="route">The route replacing the tip.</param>
        new T Replace(IRoute route);

        IRouter IRouter.Replace(IRoute route) => Replace(route);

        /// <inheritdoc cref="Root(IRouteFactory)"/>
        new T Root(IRouteFactory routeFactory);

        IRouter IRouter.Root(IRouteFactory routeFactory) => Root(routeFactory);

        /// <summary>
        /// Replaces the current root and its history by the given route.
        /// </summary>
        /// <param name="route">The route replacing the root.</param>
        new T Root(IRoute route);

        IRouter IRouter.Root(IRoute route) => Root(route);
    }
}