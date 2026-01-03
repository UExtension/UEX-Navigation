using UExtension.SceneLoader.ScriptableObjects;
using UnityEditor.SceneManagement;
using UnityEngine.UIElements;

namespace UI.Atoms
{
    public class SceneContainerUI : VisualElement
    {
        private SceneContainer _sceneContainer;

        public Label NameLabel { get; }

        public SceneContainerUI()
        {
            AddToClassList("scene-container");
            NameLabel = new Label();
            Add(NameLabel);

            var buttonContainer = new VisualElement();
            buttonContainer.AddToClassList("scene-container__buttons");

            var loadButton = new Button
            {
                text = "Load"
            };
            loadButton.RegisterCallback<ClickEvent>(HandleLoadRoute);
            loadButton.AddToClassList("route-factory__load-button");
            buttonContainer.Add(loadButton);

            Add(buttonContainer);
        }

        public void Initialize(SceneContainer sceneContainer)
        {
            _sceneContainer = sceneContainer;
            NameLabel.text = sceneContainer.name;
        }

        private void HandleLoadRoute(ClickEvent evt)
        {
            EditorSceneManager.OpenScene(_sceneContainer.Path);
        }
    }
}