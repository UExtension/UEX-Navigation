using System.Collections.Generic;
using System.IO;
using UExtension.Navigation.Route;
using UExtension.SceneLoader.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace UExtension.Navigation.ScriptableObjects.Route
{
    public abstract class AbstractRouteFactory : ScriptableObject, IRouteFactory
    {
        [field: Header("Route Configuration"), SerializeField]
        public string Name { get; private set; }

        [field: SerializeField]
        public List<SceneContainer> Scenes { get; private set; }

        [field: SerializeField]
        public SceneContainer ActiveScene { get; private set; }

        [field: SerializeField]
        public SceneContainer BakingSetActiveScene { get; private set; }

        #region Editor

#if UNITY_EDITOR
        private void OnValidate()
        {
            var fileName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(this));
            if (fileName == Name) return;

            Name = fileName;

            if (ActiveScene == null && Scenes.Count > 0)
            {
                ActiveScene = Scenes[0];
            }

            EditorUtility.SetDirty(this);
        }
#endif

        #endregion

        public abstract IRoute Create();

        public bool Is(IRoute route)
        {
            return Name == route.Name;
        }
    }
}