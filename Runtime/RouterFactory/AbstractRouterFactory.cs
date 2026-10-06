using UExtension.Navigation.Router;
using UnityEngine;

namespace UExtension.Navigation.RouterFactory
{
    public abstract class AbstractRouterFactory<TA> : ScriptableObject, IRouterFactory<TA> where TA : IRouter<TA>
    {
        [field: Header("Router Configuration")]
        [field: SerializeField]
        public virtual string Name { get; set; }

        public abstract TA Create();

        public bool Is(IRouter router) => Name == router.name;
    }
}