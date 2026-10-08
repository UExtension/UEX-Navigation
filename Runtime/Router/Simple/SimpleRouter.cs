using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UExtension.Navigation.Exceptions;
using UExtension.Navigation.Route;
using UExtension.Navigation.Route.Tab;
using UExtension.Navigation.RouteFactory;
using UExtension.Navigation.RouteFactory.Tab;
using UnityEngine;

namespace UExtension.Navigation.Router.Simple
{
    public class SimpleRouter : AbstractRouter<SimpleRouter>
    {
        private const string LOGGING_PREFIX = "<color=#DE00C8>[Navigation]</color>";
        public const string DEFAULT_NAME = "SimpleDefault";
        private IRoute _route;
        private CancellationTokenSource _cts;

        public event Action<string, IRoute> OnRouteLoadStart = delegate {};
        public event Action<string, IRoute> OnRouteLoadEnd = delegate {};

        public override string name { get; }
        public override bool logging { get; set; }

        public SimpleRouter(string name = null, bool logging = true)
        {
            this.logging = logging;
            this.name = name ?? DEFAULT_NAME;
        }

        /// <inheritdoc cref="IRoute.Push"/>
        public override SimpleRouter Push(IRouteFactory routeFactory) => Push(routeFactory.Create());

        /// <inheritdoc cref="IRoute.Push"/>
        public override SimpleRouter Push(IRoute route)
        {
            if (!IsRouteReady()) return Root(route);

            if (logging) Debug.Log($"{LOGGING_PREFIX} Push: {route.Name}");

            return UpdateRoute(_route.Push(route));
        }

        /// <inheritdoc cref="IRoute.Pop()"/>
        public override SimpleRouter Pop() => Pop(_route);

        /// <inheritdoc cref="IRoute.Pop(IRoute)"/>
        public override SimpleRouter Pop(IRoute route)
        {
            if (!IsRouteReady()) throw new RootRouteNotYetInstantiated();

            if (logging) Debug.Log($"{LOGGING_PREFIX} Pop: {route.Name}");

            return UpdateRoute(_route.Pop(route));
        }

        /// <inheritdoc cref="IRoute.Navigate"/>
        public override SimpleRouter Navigate(IRouteFactory routeFactory) => Navigate(routeFactory.Create());

        /// <summary>
        /// <inheritdoc cref="IRoute.Navigate"/>
        /// </summary>
        public override SimpleRouter Navigate(IRoute route)
        {
            if (_route == null) return Root(route);

            if (logging) Debug.Log($"{LOGGING_PREFIX} Navigate: {route.Name}");

            return UpdateRoute(_route.Navigate(route));
        }

        /// <inheritdoc cref="SetTab(UExtension.Navigation.Route.Tab.TabRoute,UExtension.Navigation.Route.IRoute)"/>
        public override SimpleRouter SetTab(TabRouteFactory tabRouteFactory, IRouteFactory tabFactory) => SetTab(tabRouteFactory.CreateTyped(), tabFactory.Create());

        /// <summary>
        /// Updates the active tab of the given <see cref="TabRoute"/> and navigates to it. If the given <see cref="TabRoute"/> isn't found in history, it will be pushed on top.
        /// </summary>
        /// <param name="tabRoute">The route to navigate to.</param>
        /// <param name="tab">The tab to set active.</param>
        public override SimpleRouter SetTab(TabRoute tabRoute, IRoute tab)
        {
            if (logging) Debug.Log($"{LOGGING_PREFIX} Set tab {tab.Name} to {tabRoute.Name}");

            return UpdateRoute(tabRoute.SetActiveTab(tab));
        }

        /// <inheritdoc cref="IRoute.GetRoot"/>
        public override IRoute GetRoot() => _route.GetRoot();

        /// <inheritdoc cref="IRoute.GetTip"/>
        public override IRoute GetTip() => _route.GetTip();

        /// <inheritdoc cref="IRoute.Search"/>
        public override IRoute Search(IRouteFactory routeFactory) => Search(routeFactory.Create());

        /// <inheritdoc cref="IRoute.Search"/>
        public override IRoute Search(IRoute route) => _route.Search(route);

        /// <inheritdoc cref="Replace(IRouteFactory)"/>
        public override SimpleRouter Replace(IRouteFactory routeFactory) => Replace(routeFactory.Create());

        /// <summary>
        /// Replaces the tip of the current route stack by the given route.
        /// </summary>
        /// <param name="route">The route replacing the tip.</param>
        public override SimpleRouter Replace(IRoute route)
        {
            if (logging) Debug.Log($"{LOGGING_PREFIX} Replace: {route.Name}");

            return UpdateRoute(_route.Pop().Push(route));
        }

        /// <inheritdoc cref="Root(IRouteFactory)"/>
        public override SimpleRouter Root(IRouteFactory routeFactory) => Root(routeFactory.Create());

        /// <summary>
        /// Replaces the current root and its history by the given route.
        /// </summary>
        /// <param name="route">The route replacing the root.</param>
        public override SimpleRouter Root(IRoute route)
        {
            if (logging) Debug.Log($"{LOGGING_PREFIX} Root: {route.Name}");

            route.Previous = null;
            route.IsSelfActive = true;

            return UpdateRoute(route);
        }

        public override bool IsRouteReady() => _route != null;

        protected virtual SimpleRouter UpdateRoute(IRoute route)
        {
            _route = route.GetTip();

            if (logging) Debug.Log($"{LOGGING_PREFIX} {_route.GetRoot().ToString()}");
            return this;
        }

        public override void CancelLoad()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        /// <summary>
        /// Asks the <see cref="SceneLoader"/> to load the current route tip.
        /// </summary>
        public override async UniTask Load()
        {
            CancelLoad();
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            OnRouteLoadStart.Invoke(name, _route);
            await SceneLoader.SceneLoader.SetActiveSceneContainers(_route.Scenes, cancellationToken: token);

            if (_route.ActiveScene != null) SceneLoader.SceneLoader.SetActiveScene(_route.ActiveScene);

            if (_route.BakingSetActiveScene != null) SceneLoader.SceneLoader.SetBakingSet(_route.BakingSetActiveScene);

            OnRouteLoadEnd.Invoke(name, _route);
        }
    }
}