using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;

namespace Better.Singletons.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    [Conditional(Defines.Editor)]
    public class EnsureScriptableInstance : Attribute
    {
        
    }
}