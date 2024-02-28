using System;
using System.IO;
using System.Reflection;
using Better.Extensions.Runtime;
using Better.Singletons.Runtime.Attributes;
using UnityEngine;

namespace Better.Singletons.Runtime
{
    internal static class ScriptableSingletonUtility
    {
        public static T LoadOrCreate<T>() where T : ScriptableSingletonObject<T>
        {
            var type = typeof(T);
            var name = type.Name;
            var createAttribute = type.GetCustomAttribute<ScriptableCreateAttribute>();

            if (createAttribute != null && !createAttribute.Name.IsNullOrEmpty())
            {
                name = createAttribute.Name;
            }

            var scriptable = Resources.Load<T>(name);

            if (scriptable == null)
            {
#if UNITY_EDITOR
                var path = nameof(Resources);

                if (createAttribute != null && !createAttribute.Path.IsNullOrEmpty())
                {
                    path = Path.Combine(createAttribute.Path, path);
                }

                scriptable = ScriptableObjectUtility.CreateScriptableObjectAsset<T>(path, $"{name}{AssetDatabaseUtility.AssetExtension}");
#else
                scriptable = ScriptableObject.CreateInstance<T>();
#endif
            }

            return scriptable;
        }

#if UNITY_EDITOR

        public static T FindScriptableObject<T>() where T : ScriptableSingletonAsset<T>
        {
            var assets = AssetDatabaseUtility.FindAssetsOfType<T>();
            var typeName = typeof(T).Name;
            if (assets.IsNullOrEmpty())
            {
                DebugUtility.LogException<InvalidOperationException>("No asset found");
                return null;
            }

            if (assets.Length > 1)
            {
                Debug.LogWarning("More than one asset found");
            }

            return assets[0];
        }

#endif
    }
}