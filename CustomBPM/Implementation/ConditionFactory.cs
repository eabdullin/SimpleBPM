using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using CustomBPM.Attributes;

namespace CustomBPM.Implementation
{
    public class ConditionFactory : IConditionFactory
    {
        private static IDictionary<string, Func<ICondition>> _conditions; 
        static ConditionFactory()
        {
            _conditions = new ConcurrentDictionary<string, Func<ICondition>>();
            FactoryHelper<string, ICondition>.InitFactory(_conditions, typeof (ConditionFactory).Assembly,(value) =>
            {
                CodeAttribute codeAttribute = (CodeAttribute)Attribute.GetCustomAttribute(value, typeof(CodeAttribute));
                return codeAttribute.Code;
            });
        }
        public ICondition Create(string key)
        {
            return _conditions[key]();
        }
    }
}
