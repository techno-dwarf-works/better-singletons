using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;

namespace Better.Singletons.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    [Conditional(Defines.Editor)]
    public class ScriptableCreateAttribute : Attribute
    {
        public string Path { get; }
        public string Name { get; }
        
        public ScriptableCreateAttribute(string path, string name)
        {
            Path = path;
            Name = name;
        }

        public ScriptableCreateAttribute(string path) : this(path, string.Empty)
        {
            
        }
    }
}