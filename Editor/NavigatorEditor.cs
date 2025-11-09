using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UExtension.Navigation.Editor
{
    [CustomEditor(typeof(Navigator))]
    public class NavigatorEditor : UnityEditor.Editor
    {
        [MenuItem("Assets/Create/UExtension/Navigation/Navigator")]
        private static void CreateAsset()
        {
            var guids = AssetDatabase.FindAssets($"t:{nameof(Navigator)}");
            if (guids.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                Selection.activeObject = AssetDatabase.LoadAssetAtPath<Navigator>(path);
                throw new Exception($"{nameof(Navigator)} already exists");
            }

            var asset = CreateInstance<Navigator>();
            ProjectWindowUtil.CreateAsset(asset, "UEXNavigator.asset");

            Selection.activeObject = asset;
            EditorUtility.FocusProjectWindow();
        }

        [MenuItem("UExtension/Navigation/Toggle Logging")]
        public static void ToggleLogging()
        {
            var asset = AssetDatabase
                .FindAssets($"t:{nameof(Navigator)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<Navigator>)
                .Single();

            asset.ActiveLogging = !asset.ActiveLogging;
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();

            Debug.Log($"{nameof(Navigator)} Logging: {(asset.ActiveLogging ? "ON" : "OFF")}");
        }
    }
}