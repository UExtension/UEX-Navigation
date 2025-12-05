using UExtension.Navigation.Route;

namespace UExtension.Navigation.RouteFactory
{
    public interface IRouteFactory
    {
        IRoute Create();

        bool Is(IRoute route);
    }
}