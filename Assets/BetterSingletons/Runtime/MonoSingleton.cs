﻿using System;
using System.Collections.Generic;
using Better.Commons.Runtime.Extensions;
using UnityEngine;

namespace Better.Singletons.Runtime
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                ValidateInstance();
                return _instance;
            }
        }

        private static void ValidateInstance()
        {
            if (_instance != null)
            {
                return;
            }

            var objects = FindObjectsOfType<T>();

            if (objects.IsNullOrEmpty())
            {
                throw new InvalidOperationException("No instance found");
            }

            _instance = objects[0];

            if (objects.Length > 1)
            {
                Debug.LogWarning("More than one instance found. Destroying other instances");

                var others = new List<T>(objects);
                others.Remove(_instance);

                //TODO: Maybe add settings to destroy or reassign instance
                foreach (var item in others)
                {
                    Destroy(item.gameObject);
                }
            }
        }
    }
}