using System.Collections.Generic;
using System.Linq;
using UExtension.Navigation.Editor.UI.Atoms;
using UExtension.Navigation.RouteFactory;
using UnityEditor;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace UExtension.Navigation.Editor.UI.Views
{
    [UxmlElement]
    public partial class RoutesListView : ListView
    {
        public RoutesListView()
        {
            AddToClassList("routes-list-view");
            makeItem = () =>
            {
                var routeContainer = new VisualElement();
                routeContainer.AddToClassList("routes-list-view__item");
                var route = new RouteFactoryAtom();
                routeContainer.Add(route);
                return routeContainer;
            };
            bindItem = (element, i) => { ((RouteFactoryAtom)element[0]).Initialize(((List<AbstractRouteFactory>)itemsSource)[i]); };
            showAlternatingRowBackgrounds = AlternatingRowBackground.ContentOnly;
            virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            selectionChanged += HandleRouteFactorySelected;
        }

        private void HandleRouteFactorySelected(IEnumerable<object> selection)
        {
            var selectedObject = selection.FirstOrDefault() as Object;
            if (selectedObject != null)
            {
                Selection.activeObject = selectedObject;

                EditorGUIUtility.PingObject(selectedObject);
            }
        }
    }
}