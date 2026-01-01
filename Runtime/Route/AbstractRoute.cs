using System;
using System.Collections.Generic;
using UExtension.SceneLoader.ScriptableObjects;

namespace UExtension.Navigation.Route
{
    public abstract class AbstractRoute : IEquatable<IRoute>, IRoute
    {
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

        public string Name { get; }
        public List<SceneContainer> Scenes { get; }
        public SceneContainer ActiveScene { get; }
        public SceneContainer BakingSetActiveScene { get; }
        public virtual IRoute Previous { get; set; }
        public virtual IRoute Next { get; set; }

        protected bool IsSelfActive { get; set; } = true;

        bool IRoute.IsSelfActive
        {
            get => IsSelfActive;
            set => IsSelfActive = value;
        }

        public virtual bool IsActive => (Previous?.IsActive ?? true) && IsSelfActive;

        public abstract IRoute Push(IRoute route);

        public abstract IRoute Pop();

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