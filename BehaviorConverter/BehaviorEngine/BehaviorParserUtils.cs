using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.logic;

namespace BehaviorConverter.BehaviorEngine
{
    public static class BehaviorParserUtils
    {
        public static T GetParameter<T>(string source, string argumentName, IArgumentList args)
        {
            if (typeof(T) == typeof(Cooldown)) throw new ArgumentException("Use int instead of Cooldown", "T");
            var newSource = source.Replace("(", String.Empty).Replace(")", String.Empty);
            if (!source.StartsWith("("))
                newSource = GetParameters(source);

            var parameters = newSource.Split(new [] {","}, StringSplitOptions.RemoveEmptyEntries);
            var currentIndex = 0;
            bool reachedNamedParameters;
            foreach (var para in parameters)
            {
                string name;
                //Named parameters are always after unnamed parameters, so we can use the arg list to get the index.
                if ((reachedNamedParameters = IsNamedParameter(para.TrimStart(), out name)) && name == argumentName)
                    return ParseParameter<T>(para.TrimStart());

                if (reachedNamedParameters) continue;
                var paraIndex = args.NameToParameter[argumentName].Key;
                if (parameters.Length > paraIndex && paraIndex == currentIndex)
                    return ParseParameter<T>(parameters[paraIndex].TrimStart());
                currentIndex++;
            }
            return args.NameToParameter[argumentName].Value.Key == null ? default(T) : (T)Convert.ChangeType(args.NameToParameter[argumentName].Value.Key, typeof(T));
        }

        private static bool IsNamedParameter(string para, out string name)
        {
            var parameterName = (string)null;
            if (para.IndexOf(":", StringComparison.InvariantCulture) > 0)
                parameterName = para.Split(':')[0];
            name = parameterName;
            return parameterName != null;
        }

        public static T ParseParameter<T>(string para)
        {
            var targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
            //Check if parameter is unnamed
            if (para.IndexOf(":", StringComparison.InvariantCulture) < 0) return (T)Convert.ChangeType(para, targetType);
            return (T)Convert.ChangeType(para.Split(':')[1], targetType);
        }

        public static string GetParameters(string source)
        {
            return source.Remove(source.IndexOf(')')).Remove(0, source.IndexOf('(') + 1);
        }
    }
}
