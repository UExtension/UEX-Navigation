using System.Linq;
using UExtension.Navigation.Route;
using UExtension.Navigation.Route.Tab;
using UnityEngine;

namespace UExtension.Navigation.RouteFactory.Tab
{
    [CreateAssetMenu(menuName = "UExtension/Navigation/TabRoute", fileName = "TabRoute")]
    public class TabRouteFactory : AbstractRouteFactory
    {
        [field: Header("Tabs Configuration"), SerializeField]
        private AbstractRouteFactory DefaultTab { get; set; }

        [field: SerializeField]
        public AbstractRouteFactory[] Tabs { get; private set; }

        public override IRoute Create()
        {
            return CreateTyped();
        }

        public TabRoute CreateTyped()
        {
            var tabSet = Tabs.ToHashSet();
            tabSet.Add(DefaultTab);
            var tabs = tabSet.Select(factory => factory.Create()).ToArray();
            return new TabRoute(Name, Scenes, ActiveScene, BakingSetActiveScene, tabs, DefaultTab.Create());
        }
    }
}