using Cysharp.Threading.Tasks;
using UExtension.Navigation.Route;
using UExtension.Navigation.Route.Tab;
using UExtension.Navigation.RouteFactory;
using UExtension.Navigation.RouteFactory.Tab;

namespace UExtension.Navigation.Router
{
    public abstract class AbstractRouter<TA> : IRouter<TA> where TA : AbstractRouter<TA>
    {
        public abstract string name { get; }
        public abstract bool logging { get; set; }
        public abstract TA Push(IRouteFactory routeFactory);
        public abstract TA Push(IRoute route);
        public abstract TA Pop();
        public abstract TA Pop(IRoute route);
        public abstract TA Navigate(IRouteFactory routeFactory);
        public abstract TA Navigate(IRoute route);
        public abstract TA SetTab(TabRouteFactory tabRouteFactory, IRouteFactory tabFactory);
        public abstract TA SetTab(TabRoute tabRoute, IRoute tab);
        public abstract TA Replace(IRouteFactory routeFactory);
        public abstract TA Replace(IRoute route);
        public abstract TA Root(IRouteFactory routeFactory);
        public abstract TA Root(IRoute route);
        public abstract IRoute GetRoot();
        public abstract IRoute GetTip();
        public abstract IRoute Search(IRouteFactory routeFactory);
        public abstract IRoute Search(IRoute route);
        public abstract bool IsRouteReady();
        public abstract UniTask Load();
        public abstract void CancelLoad();

        public bool Equals(IRouter other) => other is not null && name == other.name;

        public override bool Equals(object obj) => obj is IRouter other && Equals(other);

        public override int GetHashCode() => name.GetHashCode();
    }
}