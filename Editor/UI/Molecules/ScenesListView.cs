using System.Collections.Generic;
using System.Linq;
using UExtension.SceneLoader.ScriptableObjects;
using UI.Atoms;
using UnityEditor;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace UI
{
    [UxmlElement]
    public partial class ScenesListView : ListView
    {
        public ScenesListView()
        {
            AddToClassList("scenes-list-view");
            makeItem = () =>
            {
                var routeContainer = new VisualElement();
                routeContainer.AddToClassList("scenes-list-view__item");
                var route = new SceneContainerUI();
                routeContainer.Add(route);
                return routeContainer;
            };
            bindItem = (element, i) =>
            {
                ((SceneContainerUI)element[0]).Initialize(((List<SceneContainer>)itemsSource)[i]);
            };
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