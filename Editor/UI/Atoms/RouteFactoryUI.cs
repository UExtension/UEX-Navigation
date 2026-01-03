using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UExtension.Navigation;
using UExtension.Navigation.RouteFactory;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Atoms
{
    public class RouteFactoryUI : VisualElement
    {
        private AbstractRouteFactory _routeFactory;

        public Label NameLabel { get; }

        public RouteFactoryUI()
        {
            AddToClassList("route-factory");
            NameLabel = new Label();
            Add(NameLabel);

            var buttonContainer = new VisualElement();
            buttonContainer.AddToClassList("route-factory__buttons");

            if (Application.isPlaying)
            {
                CreatePlayModeButtons().ForEach(buttonContainer.Add);
            }
            else
            {
                CreateEditorModeButtons().ForEach(buttonContainer.Add);
            }

            Add(buttonContainer);
        }

        private List<Button> CreatePlayModeButtons()
        {
            var pushButton = new Button
            {
                text = "Push"
            };
            pushButton.RegisterCallback<ClickEvent>(HandlePushRoute);
            pushButton.AddToClassList("route-factory__push-button");

            var replaceButton = new Button
            {
                text = "Replace"
            };
            replaceButton.RegisterCallback<ClickEvent>(HandleReplaceRoute);
            replaceButton.AddToClassList("route-factory__replace-button");

            var rootButton = new Button
            {
                text = "Root"
            };
            rootButton.RegisterCallback<ClickEvent>(HandleRootRoute);
            rootButton.AddToClassList("route-factory__root-button");

            return new List<Button> { pushButton, replaceButton, rootButton };
        }

        private List<Button> CreateEditorModeButtons()
        {
            var lootButton = new Button
            {
                text = "Load"
            };
            lootButton.RegisterCallback<ClickEvent>(HandleLoadRoute);
            lootButton.AddToClassList("route-factory__load-button");

            return new List<Button> { lootButton };
        }

        private void HandleLoadRoute(ClickEvent evt)
        {
            if (_routeFactory.ActiveScene == null)
            {
                Debug.LogError("No active scene");
                return;
            }

            EditorSceneManager.OpenScene(_routeFactory.ActiveScene.Path);
            foreach (var scene in _routeFactory.Scenes)
            {
                if (_routeFactory.ActiveScene.BuildIndex != scene.BuildIndex)
                {
                    EditorSceneManager.OpenScene(scene.Path, OpenSceneMode.Additive);
                }
            }
        }

        private void HandlePushRoute(ClickEvent evt)
        {
            NavigationService.Push(_routeFactory).Forget();
        }

        private void HandleReplaceRoute(ClickEvent evt)
        {
            NavigationService.Replace(_routeFactory).Forget();
        }

        private void HandleRootRoute(ClickEvent evt)
        {
            NavigationService.Root(_routeFactory).Forget();
        }

        public void Initialize(AbstractRouteFactory routeFactory)
        {
            _routeFactory = routeFactory;
            NameLabel.text = routeFactory.Name;
        }
    }
}