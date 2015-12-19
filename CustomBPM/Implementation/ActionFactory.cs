using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using CustomBPM.Attributes;

namespace CustomBPM.Implementation
{

    public class ActionFactory :IActionFactory
    {
        private static IDictionary<string, Func<IAction>> _conditions;
        static ActionFactory()
        {
            _conditions = new ConcurrentDictionary<string, Func<IAction>>();
            FactoryHelper<string, IAction>.InitFactory(_conditions, typeof(ActionFactory).Assembly, (value) =>
            {
                CodeAttribute codeAttribute = (CodeAttribute)Attribute.GetCustomAttribute(value, typeof(CodeAttribute));
                return codeAttribute.Code;
            });
        }
        public IAction Create(string key)
        {
            return _conditions[key]();
        }
    }
}
