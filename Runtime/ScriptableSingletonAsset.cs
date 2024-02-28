using System;
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
                    _assetInstance = ScriptableSingletonUtility.FindScriptableObject<T>();
                }

                return _assetInstance;
            }
        }

#endif
    }
}