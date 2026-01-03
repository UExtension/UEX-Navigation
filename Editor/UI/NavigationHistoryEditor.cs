using UExtension.Navigation;
using UExtension.Navigation.Route;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class NavigationHistoryEditor : EditorWindow
    {
        [SerializeField] private VisualTreeAsset visualTreeAsset;

        [MenuItem("UExtension/Navigation/History")]
        public static void ShowExample()
        {
            var wnd = GetWindow<NavigationHistoryEditor>();
            wnd.titleContent = new GUIContent("NavigationHistory");
        }

        public void CreateGUI()
        {
            rootVisualElement.Clear();
            visualTreeAsset.CloneTree(rootVisualElement);
        }
    }
}