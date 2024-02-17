using System;

namespace Better.Singletons.Runtime
{
    [Serializable]
    public abstract class PocoSingleton<T> where T : PocoSingleton<T>, new()
    {
        private static T _instance;
        
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }

                return _instance;
            }
        }
    }
}