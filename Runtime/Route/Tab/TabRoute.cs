using System;
using System.Collections.Generic;
using UExtension.SceneLoader.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UExtension.Navigation.Route.Tab
{
    public class TabRoute : AbstractRoute
    {
        private Dictionary<string, IRoute> Tabs { get; } = new();

        private IRoute _activeTab;

        public IRoute ActiveTab
        {
            get => _activeTab;
            private set
            {
                if (_activeTab != null) _activeTab.IsSelfActive = false;

                _activeTab = Tabs[value.Name];
                _activeTab.IsSelfActive = true;
            }
        }

        public TabRoute(string name, List<SceneContainer> scenes, SceneContainer activeScene, SceneContainer bakingSetScene, IRoute[] tabs,
            IRoute activeTab) : base(name, scenes, activeScene, bakingSetScene)
        {
            foreach (var tab in tabs)
            {
                Tabs.Add(tab.Name, tab);
                tab.Previous = this;
                tab.IsSelfActive = false;
            }

            ActiveTab = activeTab;
        }

        public override IRoute Previous
        {
            get => base.Previous;
            set
            {
                base.Previous = value;
                foreach (var (_, tab) in Tabs)
                {
                    tab.Previous = this;
                }
            }
        }

        /// <inheritdoc cref="AbstractRoute.Next"/>
        /// <exception cref="InvalidOperationException">Manually setting the Next route is forbidden. State changes should be performed internally or using Push, Pop, Navigate...</exception>
        public override IRoute Next
        {
            get => ActiveTab;
            set => throw new InvalidOperationException();
        }

        public override IRoute Push(IRoute route)
        {
            return Next.Push(route);
        }

        public override IRoute Pop()
        {
            return Previous?.Pop(this) ?? GetTip();
        }

        public override IRoute Pop(IRoute route)
        {
            if (HasNext() && Next.Equals(route))
            {
                // Tab route and Next can't be separated. If Next is asked to pop, pop the Tab route.
                return Pop();
            }

            return Previous?.Pop(route) ?? GetTip();
        }

        public override IRoute Navigate(IRoute route)
        {
            if (Equals(route))
            {
                ActiveTab = ((TabRoute)route).ActiveTab;
                return GetTip();
            }

            return Previous?.Navigate(route) ?? Push(route);
        }

        public override IRoute GetTip()
        {
            return Next.GetTip();
        }

        /// <summary>
        /// Sets the active tab to the specified route.
        /// </summary>
        /// <param name="tab">The route to set as the active tab.</param>
        /// <returns>The updated route tip after the operation.</returns>
        /// <exception cref="ArgumentException">Thrown when the given route doesn't exist in the Tabs collection.</exception>
        public IRoute SetActiveTab(IRoute tab)
        {
            ActiveTab = tab;
            return GetTip();
        }

        /// <summary>
        /// Tries to set the active tab to the specified route.
        /// </summary>
        /// <param name="tab">The route to set as the active tab.</param>
        /// <returns><see langword="true"/> if the tab has been set; otherwise, <see langword="false"/>.</returns>
        public bool TrySetActiveTab(IRoute tab)
        {
            if (!Tabs.TryGetValue(tab.Name, out var internalTab)) return false;

            ActiveTab = internalTab;
            return true;
        }

        public List<IRoute> GetTabs()
        {
            return new List<IRoute>(Tabs.Values);
        }

        public override bool Equals(IRoute other)
        {
            if (other is TabRoute tabRoute)
            {
                return Name == tabRoute.Name;
            }

            return false;
        }

        public override string ToString()
        {
            var res = "";
            var routeColor = ColorUtility.ToHtmlStringRGBA(new Color(Random.value, Random.value, Random.value));

            foreach (var (name, tab) in Tabs)
            {
                string tabColor = ColorUtility.ToHtmlStringRGBA(new Color(Random.value, Random.value, Random.value));
                res += $"\n<color=#{routeColor}>[{Name}]</color><color=#{tabColor}>({name})</color> => {tab.ToString()}";
            }

            return res;
        }
    }
}