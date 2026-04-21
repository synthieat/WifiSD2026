using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SD.Application
{
    public class BaseHandler
    {

        protected void MapEntityProperties<TSource, TTarget>(TSource source, 
                                                             TTarget target, 
                                                             List<string> excludeProperties = null)
            //where TSource : class, new()
            //where TTarget : class, new()
        {
            var sourceType = source.GetType();
            var targetType = target.GetType();

            if(sourceType.BaseType.FullName != targetType.BaseType.FullName)
            {
                throw new InvalidOperationException("Source and target types must have the same base type.");
            }

            var targetPropertyInfos = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();

            targetPropertyInfos.ForEach(p =>
            {
                if(p.CanWrite && !(excludeProperties ?? []).Contains(p.Name))
                {
                    /* Passende Property aus Quelle (Source) lesen */
                    var sourcePropertyInfo = sourceType.GetProperty(p.Name, BindingFlags.Public | BindingFlags.Instance);
                    if(sourcePropertyInfo != null)
                    {
                        /* Property Wert aus Quell lesen */
                        var sourcePropertyValue = sourcePropertyInfo.GetValue(source, null);

                        /* Ausgelesene Wert in Ziel Property (target) schreibnen */
                        p.SetValue(target, sourcePropertyValue, null);
                    }
                }
            });

        }
    }
}
