using UExtension.Navigation.ScriptableObjects.Route;
using UnityEngine;

namespace UExtension.Navigation.ScriptableObjects
{
    public class NavigationActions : ScriptableObject
    {
        public void Push(AbstractRouteFactory route)
        {
            _ = Navigator.Push(route);
        }

        public void Navigate(AbstractRouteFactory route)
        {
            _ = Navigator.Navigate(route);
        }

        public void Pop()
        {
            _ = Navigator.Pop();
        }

        public void Replace(AbstractRouteFactory route)
        {
            _ = Navigator.Replace(route);
        }

        public void Root(AbstractRouteFactory route)
        {
            _ = Navigator.Root(route);
        }
    }
}