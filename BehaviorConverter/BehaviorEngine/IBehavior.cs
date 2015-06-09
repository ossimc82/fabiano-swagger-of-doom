using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorConverter.BehaviorEngine
{
    public interface IBehavior
    {
        string ClassName { get; }

        int ArgumentsCount { get; }
        int AdditionalArgumentsCount { get; }

        int UsedArgumentsCount { get; }
        int UsedAdditionalArgumentsCount { get; }

        IDictionary<string, int> ArgumentNames { get; }
        IDictionary<string, int> ArgumentValues { get; }

        IArgumentList ArgumentDict { get; }
    }
}
