using UExtension.Navigation.Router;

namespace UExtension.Navigation.RouterFactory
{
    public interface IRouterFactory<out T> : IRouterFactory where T : IRouter<T>
    {
        new T Create();
        IRouter IRouterFactory.Create() => Create();
    }

    public interface IRouterFactory
    {
        string Name { get; }

        IRouter Create();

        bool Is(IRouter router);
    }
}