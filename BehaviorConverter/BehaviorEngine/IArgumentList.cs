using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorConverter.BehaviorEngine
{
    public interface IArgumentList
    {
        //<parameterName, <index, <defaultValue, parameterType>>>
        IDictionary<string, KeyValuePair<int, KeyValuePair<object, Type>>> NameToParameter { get; } 
    }
}
