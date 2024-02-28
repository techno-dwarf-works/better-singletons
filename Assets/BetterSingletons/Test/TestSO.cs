using Better.Singletons.Runtime;
using Better.Singletons.Runtime.Attributes;
using UnityEngine;

namespace Test
{
    [ScriptableCreate(nameof(Better) + "/" + nameof(Better.Singletons))]
    [EnsureScriptableInstance]
    public class TestSO : ScriptableSingletonObject<TestSO>
    {
        [SerializeField] private int _type;
    }
}