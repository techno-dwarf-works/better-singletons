using Better.Singletons.Runtime.Utility;
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
                    _instance = ScriptableSingletonUtility.LoadOrCreate<T>();
                }

                return _instance;
            }
        }
    }
}