using System.Linq;
using UExtension.Navigation.Components;
using UnityEditor;
using UnityEngine;

namespace UExtension.Navigation.Editor
{
    public static class NavigationEditor
    {
        [MenuItem("UExtension/Navigation/Toggle Logging")]
        public static void ToggleLogging()
        {
            var asset = AssetDatabase
                .FindAssets($"t:{nameof(NavigationActions)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<NavigationActions>)
                .Single();

            asset.SetLogging(!asset.Logging);
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();

            Debug.Log($"{nameof(NavigationService)} Logging: {(NavigationService.Logging ? "ON" : "OFF")}");
        }
    }
}