using System;

namespace UExtension.Navigation.Exceptions
{
    public class RootRouteNotYetInstantiated : Exception
    {
        public RootRouteNotYetInstantiated() : base("Root route not yet instantiated\n Be sure to Push or Root any route before navigating") {}
    }
}