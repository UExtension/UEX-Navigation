using System;
using System.Collections.Generic;
using UExtension.SceneLoader.ScriptableObjects;

namespace UExtension.Navigation.Route
{
    public abstract class AbstractRoute : IEquatable<IRoute>, IRoute
    {
        protected IRoute _previous;

        protected AbstractRoute(string name, List<SceneContainer> scenes, SceneContainer activeScene, SceneContainer bakingSetScene)
        {
            Name = name;
            Scenes = scenes;
            ActiveScene = activeScene;
            BakingSetActiveScene = bakingSetScene;
        }

        public virtual bool Equals(IRoute other)
        {
            return Name == other?.Name;
        }

        public virtual string Name { get; }
        public virtual List<SceneContainer> Scenes { get; }
        public virtual SceneContainer ActiveScene { get; }
        public virtual SceneContainer BakingSetActiveScene { get; }

        public virtual IRoute Previous
        {
            get => _previous;
            set
            {
                _previous = value;

                // Calculate the depth
                Depth = _previous?.Depth + 1 ?? 0;
                if (HasNext()) Next.Previous = this;
            }
        }

        public virtual IRoute Next { get; set; }
        public virtual bool IsSelfActive { get; set; } = true;
        public virtual bool IsActive => (Previous?.IsActive ?? true) && IsSelfActive;
        public virtual int Depth { get; protected set; }

        public abstract IRoute Push(IRoute route);
        public abstract IRoute Pop();
        public abstract IRoute Pop(IRoute route);
        public abstract IRoute Navigate(IRoute route);

        public virtual IRoute GetRoot()
        {
            return Previous?.GetRoot() ?? this;
        }

        public abstract IRoute GetTip();

        public IRoute Search(IRoute route)
        {
            return Equals(route) ? this : Previous?.Search(route);
        }

        public virtual bool HasPrevious()
        {
            return Previous != null;
        }

        public virtual bool HasNext()
        {
            return Next != null;
        }

        public new virtual string ToString()
        {
            return $"{Name}";
        }
    }
}