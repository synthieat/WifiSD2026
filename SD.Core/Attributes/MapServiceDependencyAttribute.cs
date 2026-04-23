using System;
using System.Collections.Generic;
using System.Text;

namespace SD.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MapServiceDependencyAttribute : Attribute
    {
        protected string Name;

        public MapServiceDependencyAttribute(string name)
        {
            this.Name = name;
        }
    }
}
