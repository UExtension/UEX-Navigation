using UExtension.Navigation.Route;
using UnityEngine;
using UnityEngine.Events;

namespace UExtension.Navigation.Components
{
    public class NavigationEvents : MonoBehaviour
    {
        [field: SerializeField]
        public UnityEvent<IRoute> OnRouteLoadStart { get; private set; } = new();

        [field: SerializeField]
        public UnityEvent<IRoute> OnRouteLoadEnd { get; private set; } = new();

        private void Awake()
        {
            NavigationService.OnRouteLoadStart += HandleRouteLoadStart;
            NavigationService.OnRouteLoadEnd += HandleRouteLoadEnd;
        }

        private void OnDestroy()
        {
            NavigationService.OnRouteLoadStart -= HandleRouteLoadStart;
            NavigationService.OnRouteLoadEnd -= HandleRouteLoadEnd;
        }

        private void HandleRouteLoadStart(IRoute route)
        {
            OnRouteLoadStart.Invoke(route);
        }

        private void HandleRouteLoadEnd(IRoute route)
        {
            OnRouteLoadEnd.Invoke(route);
        }
    }
}