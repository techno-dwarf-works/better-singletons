using System.IO;
using UnityEngine;

namespace Better.Singletons.Runtime
{
    public abstract class ScriptableSingletonObject<T> : ScriptableObject where T : ScriptableSingletonObject<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    LoadOrCreate();
                }

                return _instance;
            }
        }

        public static void LoadOrCreate()
        {
            _instance = ScriptableSingletonUtility.LoadOrCreate<T>();
        }
    }
}