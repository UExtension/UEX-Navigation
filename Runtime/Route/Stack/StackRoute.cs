using System.Collections.Generic;
using UExtension.SceneLoader.ScriptableObjects;

namespace UExtension.Navigation.Route.Stack
{
    public class StackRoute : AbstractRoute
    {
        public StackRoute(string name, List<SceneContainer> scenes, SceneContainer activeScene, SceneContainer bakingSetScene) : base(name, scenes, activeScene, bakingSetScene)
        {
        }

        public override IRoute Push(IRoute route)
        {
            if (HasNext()) return Next.Push(route);

            Next = route;
            Next.Previous = this;
            return GetTip();
        }

        /// <inheritdoc cref="AbstractRoute.Pop"/>
        public override IRoute Pop()
        {
            // If I have a next route
            if (HasNext())
            {
                // And it has a next route => ask it to pop
                if (Next.HasNext())
                {
                    return Next.Pop();
                }

                // Otherwise pop it and return me
                Next = null;
                return GetTip();
            }

            // If previous => pop me
            if (HasPrevious()) return Previous.Pop();

            // I'm the root => return me
            return GetTip();
        }

        public override IRoute Navigate(IRoute route)
        {
            // If I'm the route to be navigated to => Pop Next and return me
            if (Equals(route))
            {
                Next = null;
                return GetTip();
            }

            return Previous?.Navigate(route) ?? Push(route);
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