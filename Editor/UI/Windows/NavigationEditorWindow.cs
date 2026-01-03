using UExtension.Bootstrap.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UExtension.Navigation.Editor.UI.Windows
{
    public class NavigationEditorWindow : EditorWindow, IUExtensionEditorWindow
    {
        [field: SerializeField] public VisualTreeAsset VisualTreeAsset { get; private set; }
        public string Name => "Navigation";
        public int Order => 1;

        [MenuItem("UExtension/Navigation/Editor Window")]
        public static void ShowExample()
        {
            var wnd = GetWindow<NavigationEditorWindow>();
            wnd.titleContent = new GUIContent(wnd.Name);
        }

        public void CreateGUI()
        {
            rootVisualElement.Clear();
            VisualTreeAsset.CloneTree(rootVisualElement);
        }
    }
}