using System;
using System.Collections.Generic;
using System.Linq;
using UExtension.Bootstrap.UI;
using UExtension.Navigation.RouteFactory;
using UnityEditor;
using UnityEngine.UIElements;

namespace UExtension.Navigation.Editor.UI.Molecules
{
    [UxmlElement]
    public partial class SearchableRoutesListView : VisualElement
    {
        private readonly RoutesListView listView;
        private readonly List<AbstractRouteFactory> routeFactories = new();
        private readonly List<AbstractRouteFactory> filteredFactories = new();
        
        public SearchableRoutesListView()
        {
            AddToClassList("searchable-routes-list-view");
            var searchField = CreateSearchField();
            Add(searchField);

            routeFactories.Clear();
            routeFactories.AddRange(FindAbstractRouteFactories());

            filteredFactories.Clear();
            filteredFactories.AddRange(routeFactories);

            listView = new RoutesListView
            {
                itemsSource = filteredFactories
            };
            Add(listView);
        }

        private TextField CreateSearchField()
        {
            TextField searchField = new();
            searchField.AddToClassList("searchable-routes-list-view__search-field");
            searchField.textEdition.placeholder = "Search";
            searchField.RegisterValueChangedCallback(HandleSearchFieldChanged);

            return searchField;
        }

        private void HandleSearchFieldChanged(ChangeEvent<string> evt)
        {
            if (evt.newValue.StartsWith(evt.previousValue, StringComparison.InvariantCultureIgnoreCase))
            {
                filteredFactories.RemoveAll(routeFactory =>
                    !routeFactory.Name.ToLower().Contains(evt.newValue.ToLower()));
            }
            else
            {
                filteredFactories.Clear();
                filteredFactories.AddRange(
                    routeFactories.Where(routeFactory => routeFactory.Name.ToLower().Contains(evt.newValue.ToLower()))
                );
            }

            listView.RefreshItems();
        }

        public static List<AbstractRouteFactory> FindAbstractRouteFactories()
        {
            var assets = new List<AbstractRouteFactory>();
            var guids = AssetDatabase.FindAssets($"t:{nameof(AbstractRouteFactory)}");

            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<AbstractRouteFactory>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }

            return assets;
        }
    }
}