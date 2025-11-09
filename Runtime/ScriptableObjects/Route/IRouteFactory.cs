using UExtension.Navigation.Route;

namespace UExtension.Navigation.ScriptableObjects.Route
{
    public interface IRouteFactory
    {
        IRoute Create();

        bool Is(IRoute route);
    }
}