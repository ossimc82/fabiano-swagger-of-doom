using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorConverter.Exceptions
{
    public class InvalidBehaviorFileException : Exception
    {
        public InvalidBehaviorFileException(string message)
            : base(message) { }
    }
}
