using System.Collections.Generic;
using CustomBPM.Attributes;

namespace CustomBPM.Conditions
{
    [Code("testCondition")]
    class TestCondition:ICondition
    {
        public string Code
        {
            get { return "testCondition"; }
        }

        public bool Execute(out string reasons, IDictionary<string, string> parameters = null)
        {
            reasons = null;
            return true;
        }
    }
}
