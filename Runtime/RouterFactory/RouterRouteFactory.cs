using System.Linq;
using UExtension.Navigation.RouteFactory;
using UExtension.Navigation.Router;

namespace UExtension.Navigation.RouterFactory
{
    public class RouterRouteFactory
    {
        public IRouterFactory routerFactory;
        public IRouteFactory routeFactory;

        public RouterRoute ToRouterRoute() => new() { router = routerFactory.Create(), route = routeFactory.Create() };
    }

    public static class RouterRouteFactoryExtensions
    {
        public static RouterRoute[] ToRouterRoutes(this RouterRouteFactory[] routerRouteFactories) => routerRouteFactories.Select(t => t.ToRouterRoute()).ToArray();
    }
}