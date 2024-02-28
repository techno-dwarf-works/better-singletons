using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Better.Extensions.Runtime;
using Better.Internal.Core.Runtime;
using Better.Singletons.Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace Better.Singletons.Runtime
{
    public class ScriptableInstanceEnsurer : AssetModificationProcessor
    {
        private static List<string> GuidsToDelete;
        private const string MethodName = "LoadOrCreate";

        [InitializeOnLoadMethod]
        private static void OnInitialize()
        {
            EnsureNewInstance();

            GuidsToDelete = new List<string>();

            EditorApplication.projectChanged += OnProjectChanged;
        }

        private static void EnsureNewInstance()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => IsSubclassOfRawGeneric(type, typeof(ScriptableSingletonObject<>)))
                .Where(type => type.GetCustomAttribute<EnsureScriptableInstance>() != null);

            foreach (var type in types)
            {
                var instanceProperty = type.GetMethod(MethodName,
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

                if (instanceProperty != null)
                {
                    instanceProperty.Invoke(null, null);
                }
            }
        }

        private static void OnProjectChanged()
        {
            GuidsToDelete.Clear();
        }

        private static bool IsSubclassOfRawGeneric(Type self, Type generic)
        {
            while (self != null && self != typeof(object))
            {
                var definition = self.IsGenericType ? self.GetGenericTypeDefinition() : self;
                if (generic == definition && self != generic)
                {
                    return true;
                }

                self = self.BaseType;
            }

            return false;
        }

        private static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions option)
        {
            var type = AssetDatabase.GetMainAssetTypeAtPath(assetPath);

            var currentAssetGuid = AssetDatabase.GUIDFromAssetPath(assetPath);

            if (IsSubclassOfRawGeneric(type, typeof(ScriptableSingletonObject<>)))
            {
                var guids = AssetDatabase.FindAssets($"t:{type}");
                var remaining = guids.Except(GuidsToDelete).ToArray();
                if (remaining.Length - 1 < 1)
                {
                    var ensureScriptableInstance = type.GetCustomAttribute<EnsureScriptableInstance>();
                    if (ensureScriptableInstance != null)
                    {
                        EditorUtility.DisplayDialog("Error", "You can't delete last singleton instance", "Ok");
                        return AssetDeleteResult.FailedDelete;
                    }
                }
            }

            GuidsToDelete.Add(currentAssetGuid.ToString());
            return AssetDeleteResult.DidNotDelete;
        }

        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            const string resourcesName = nameof(Resources);

            var type = AssetDatabase.GetMainAssetTypeAtPath(sourcePath);

            var ensureScriptableInstance = type.GetCustomAttribute<EnsureScriptableInstance>();
            if (ensureScriptableInstance != null)
            {
                var folder = Path.GetFileName(Path.GetDirectoryName(destinationPath));
                if (folder.CompareOrdinal(resourcesName))
                {
                    EditorUtility.DisplayDialog("Error", $"You can't move singleton instance from {resourcesName}", "Ok");
                    return AssetMoveResult.FailedMove;
                }
            }

            return AssetMoveResult.DidNotMove;
        }
    }
}