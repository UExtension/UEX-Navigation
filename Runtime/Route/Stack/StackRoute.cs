using System.Collections.Generic;
using UExtension.SceneLoader.ScriptableObjects;

namespace UExtension.Navigation.Route.Stack
{
    public class StackRoute : AbstractRoute
    {
        public StackRoute(string name, List<SceneContainer> scenes, SceneContainer activeScene, SceneContainer bakingSetScene) : base(name, scenes, activeScene, bakingSetScene) {}

        public override IRoute Push(IRoute route)
        {
            if (HasNext())
            {
                Next.Previous = null;
            }

            Next = route;
            Next.Previous = this;
            return GetTip();
        }

        public override IRoute Pop()
        {
            return Previous?.Pop(this) ?? GetTip();
        }

        public override IRoute Pop(IRoute route)
        {
            if (HasNext() && Next.Equals(route))
            {
                Next = null;
                return GetTip();
            }

            return Previous?.Pop(route) ?? GetTip();
        }

        public override IRoute Navigate(IRoute route)
        {
            if (Equals(route))
            {
                Next = null;
                return GetTip();
            }

            return Previous?.Navigate(route) ?? GetTip().Push(route);
        }

        public override IRoute GetTip()
        {
            return Next?.GetTip() ?? this;
        }

        public override bool Equals(IRoute other)
        {
            if (other is StackRoute stackRoute)
            {
                return base.Equals(stackRoute);
            }

            return false;
        }

        public override string ToString()
        {
            return $"{base.ToString()} => {Next?.ToString() ?? "|||"}";
        }
    }
}