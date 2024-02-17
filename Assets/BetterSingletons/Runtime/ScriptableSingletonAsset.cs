using System;
using Better.Extensions.Runtime;
using UnityEditor;
using UnityEngine;

namespace Better.Singletons.Runtime
{
    public abstract class ScriptableSingletonAsset<T> : ScriptableObject where T : ScriptableSingletonAsset<T>
    {
#if UNITY_EDITOR

        private static T _assetInstance;

        public static T AssetInstance
        {
            get
            {
                if (_assetInstance == null)
                {
                    _assetInstance = FindAsset();
                }

                return _assetInstance;
            }
        }

        private static T FindAsset()
        {
            var typeName = typeof(T).Name;
            var filter = $"t:{typeName}";
            var guids = AssetDatabase.FindAssets(filter);

            if (guids.IsNullOrEmpty())
            {
                throw new InvalidOperationException($"[{typeName}] {nameof(FindAsset)}: no asset found");
            }

            if (guids.Length > 1)
            {
                Debug.LogWarning($"[{typeName}] {nameof(FindAsset)}: more than one asset found");
            }

            //TODO: Add BetterResources or AssetDatabaseUtility
            var assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            return AssetDatabase.LoadAssetAtPath<T>(assetPath);
        }

#endif
    }
}