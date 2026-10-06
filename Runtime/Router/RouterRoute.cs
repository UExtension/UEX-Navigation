using UExtension.Navigation.Route;
using UExtension.Navigation.RouteFactory;

namespace UExtension.Navigation.Router
{
    public struct RouterRoute
    {
        public IRouter router;
        public IRoute route;

        public RouterRoute(IRouter router, IRoute route)
        {
            this.router = router;
            this.route = route;
        }

        public RouterRoute(IRouter router, IRouteFactory routeFactory)
        {
            this.router = router;
            route = routeFactory.Create();
        }
    }
}