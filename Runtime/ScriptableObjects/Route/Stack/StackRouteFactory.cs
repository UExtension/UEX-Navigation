using UExtension.Navigation.Route;
using UExtension.Navigation.Route.Stack;
using UnityEngine;

namespace UExtension.Navigation.ScriptableObjects.Route.Stack
{
    [CreateAssetMenu(menuName = "UExtension/Navigation/StackRoute", fileName = "StackRoute")]
    public class StackRouteFactory : AbstractRouteFactory
    {
        public override IRoute Create()
        {
            return CreateTyped();
        }

        public StackRoute CreateTyped()
        {
            return new StackRoute(Name, Scenes, ActiveScene, BakingSetActiveScene);
        }
    }
}