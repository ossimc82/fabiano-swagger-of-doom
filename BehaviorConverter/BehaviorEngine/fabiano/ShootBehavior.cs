using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.logic;
using static BehaviorConverter.BehaviorEngine.BehaviorParserUtils;

namespace BehaviorConverter.BehaviorEngine.fabiano
{
    public class ShootBehavior : IBehavior
    {
        public ShootBehavior(string codeLine)
        {
            ArgumentDict = new ShootBehaviorAgrumentList();
        }

        public string ClassName => "Shoot";

        public int ArgumentsCount { get; }
        public int AdditionalArgumentsCount { get; }
        public int UsedArgumentsCount { get; }
        public int UsedAdditionalArgumentsCount { get; }
        public IDictionary<string, int> ArgumentNames { get; }
        public IDictionary<string, int> ArgumentValues { get; }
        public IArgumentList ArgumentDict { get; }

        public class ShootBehaviorAgrumentList : IArgumentList
        {
            public IDictionary<string, KeyValuePair<int, KeyValuePair<object, Type>>> NameToParameter { get; } = new Dictionary<string, KeyValuePair<int, KeyValuePair<object, Type>>>
            {
                {"radius",          new KeyValuePair<int, KeyValuePair<object, Type>>(0, new KeyValuePair<object, Type>("REQUIRED", typeof(double)))},
                {"count",           new KeyValuePair<int, KeyValuePair<object, Type>>(1, new KeyValuePair<object, Type>(1, typeof(int)))},
                {"shootAngle",      new KeyValuePair<int, KeyValuePair<object, Type>>(2, new KeyValuePair<object, Type>(null, typeof(double?)))},
                {"projectileIndex", new KeyValuePair<int, KeyValuePair<object, Type>>(3, new KeyValuePair<object, Type>(0, typeof(int)))},
                {"fixedAngle",      new KeyValuePair<int, KeyValuePair<object, Type>>(4, new KeyValuePair<object, Type>(null, typeof(double?)))},
                {"angleOffset",     new KeyValuePair<int, KeyValuePair<object, Type>>(5, new KeyValuePair<object, Type>(0, typeof(double)))},
                {"defaultAngle",    new KeyValuePair<int, KeyValuePair<object, Type>>(6, new KeyValuePair<object, Type>(null, typeof(double?)))},
                {"predictive",      new KeyValuePair<int, KeyValuePair<object, Type>>(7, new KeyValuePair<object, Type>(0, typeof(double)))},
                {"coolDownOffset",  new KeyValuePair<int, KeyValuePair<object, Type>>(8, new KeyValuePair<object, Type>(0, typeof(int)))},
                {"coolDown",        new KeyValuePair<int, KeyValuePair<object, Type>>(9, new KeyValuePair<object, Type>(new Cooldown(), typeof(Cooldown)))}
            };
        }
    }
}
