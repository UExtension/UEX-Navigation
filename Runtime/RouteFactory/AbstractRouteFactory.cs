using System.Collections.Generic;
using UExtension.Navigation.Route;
using UExtension.SceneLoader.ScriptableObjects;
using UnityEngine;

namespace UExtension.Navigation.RouteFactory
{
    public abstract class AbstractRouteFactory : ScriptableObject, IRouteFactory
    {
        [field: Header("Route Configuration"), SerializeField]
        public string Name { get; set; }

        [field: SerializeField]
        public List<SceneContainer> Scenes { get; set; }

        [field: SerializeField]
        public SceneContainer ActiveScene { get; set; }

        [field: SerializeField]
        public SceneContainer BakingSetActiveScene { get; set; }

        public abstract IRoute Create();

        public bool Is(IRoute route) => Name == route.Name;
    }
}