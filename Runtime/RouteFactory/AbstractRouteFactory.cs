using System.Collections.Generic;
using UExtension.Navigation.Route;
using UExtension.SceneLoader.ScriptableObjects;
using UnityEngine;

namespace UExtension.Navigation.RouteFactory
{
    public abstract class AbstractRouteFactory : ScriptableObject, IRouteFactory
    {
        [field: Header("Route Configuration"), SerializeField]
        public string Name { get; private set; }

        [field: SerializeField]
        public List<SceneContainer> Scenes { get; private set; }

        [field: SerializeField]
        public SceneContainer ActiveScene { get; private set; }

        [field: SerializeField]
        public SceneContainer BakingSetActiveScene { get; private set; }

        public abstract IRoute Create();

        public bool Is(IRoute route)
        {
            return Name == route.Name;
        }
    }
}