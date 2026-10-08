using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UExtension.Navigation.Route;
using UExtension.Navigation.Route.Tab;
using UExtension.Navigation.RouteFactory;
using UExtension.Navigation.RouteFactory.Tab;
using UExtension.SceneLoader.ScriptableObjects;

namespace UExtension.Navigation.Router.Combined
{
    public class CombinedRouter : AbstractRouter<CombinedRouter>
    {
        public const string DEFAULT_NAME = "CombinedDefault";

        private readonly Dictionary<string, IRouter> _routers = new();
        public IRouter currentRouter { get; private set; }
        private CancellationTokenSource _cts;

        public event Action<ISet<IRouter>> OnRouteLoadStart = delegate {};
        public event Action<ISet<IRouter>> OnRouteLoadEnd = delegate {};

        public ISet<IRouter> routers => _routers.Values.ToHashSet();
        public override string name { get; }

        public override bool logging
        {
            get => currentRouter.logging;
            set => currentRouter.logging = value;
        }

        public CombinedRouter(string name = null, params IRouter[] routers)
        {
            this.name = name ?? DEFAULT_NAME;

            if (routers.Length > 0)
            {
                currentRouter = routers[0];
                foreach (var router in routers) _routers[router.name] = router;
            }
        }

        public override CombinedRouter Push(IRouteFactory routeFactory)
        {
            currentRouter.Push(routeFactory);
            return this;
        }

        public override CombinedRouter Push(IRoute route)
        {
            currentRouter.Push(route);
            return this;
        }

        public override CombinedRouter Pop()
        {
            currentRouter.Pop();
            return this;
        }

        public override CombinedRouter Pop(IRoute route)
        {
            currentRouter.Pop(route);
            return this;
        }

        public override CombinedRouter Navigate(IRouteFactory routeFactory)
        {
            currentRouter.Navigate(routeFactory);
            return this;
        }

        public override CombinedRouter Navigate(IRoute route)
        {
            currentRouter.Navigate(route);
            return this;
        }

        public override CombinedRouter SetTab(TabRouteFactory tabRouteFactory, IRouteFactory tabFactory)
        {
            currentRouter.SetTab(tabRouteFactory, tabFactory);
            return this;
        }

        public override CombinedRouter SetTab(TabRoute tabRoute, IRoute tab)
        {
            currentRouter.SetTab(tabRoute, tab);
            return this;
        }

        public override IRoute GetRoot() => currentRouter.GetRoot();

        public override IRoute GetTip() => currentRouter.GetTip();

        public override IRoute Search(IRouteFactory routeFactory) => currentRouter.Search(routeFactory);

        public override IRoute Search(IRoute route) => currentRouter.Search(route);

        public override CombinedRouter Replace(IRouteFactory routeFactory)
        {
            currentRouter.Replace(routeFactory);
            return this;
        }

        public override CombinedRouter Replace(IRoute route)
        {
            currentRouter.Replace(route);
            return this;
        }

        public override CombinedRouter Root(IRouteFactory routeFactory)
        {
            currentRouter.Root(routeFactory);
            return this;
        }

        public override CombinedRouter Root(IRoute route)
        {
            currentRouter.Root(route);
            return this;
        }

        public override bool IsRouteReady() => currentRouter.IsRouteReady();

        public override async UniTask Load()
        {
            CancelLoad();
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            var routersSnapshot = routers;

            OnRouteLoadStart.Invoke(routersSnapshot);

            var scenes = new List<SceneContainer>();
            SceneContainer activeScene = null;
            SceneContainer bakingSetActiveScene = null;
            foreach (var route in routersSnapshot.Select(router => router.GetTip()))
            {
                scenes.AddRange(route.Scenes);
                activeScene ??= route.ActiveScene;
                bakingSetActiveScene ??= route.BakingSetActiveScene;
            }

            await SceneLoader.SceneLoader.SetActiveSceneContainers(scenes, cancellationToken: token);

            if (activeScene != null) SceneLoader.SceneLoader.SetActiveScene(activeScene);

            if (bakingSetActiveScene) SceneLoader.SceneLoader.SetBakingSet(bakingSetActiveScene);

            OnRouteLoadEnd.Invoke(routersSnapshot);
        }

        public override void CancelLoad()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        public CombinedRouter SetCurrentRouter(string routerName)
        {
            if (_routers.TryGetValue(routerName, out var router)) currentRouter = router;

            return this;
        }

        public CombinedRouter SetCurrentRouter(IRouter router)
        {
            _routers[router.name] = router;
            currentRouter = router;
            return this;
        }

        public RouterRoute[] GetRouterRoutes() => _routers.Values.Select(r => new RouterRoute { router = r, route = r.GetTip() }).ToArray();
    }
}