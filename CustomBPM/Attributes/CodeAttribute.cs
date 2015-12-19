using System;

namespace CustomBPM.Attributes
{
    class CodeAttribute:Attribute
    {
        public CodeAttribute(string code)
        {
            Code = code;
        }
        public string Code { get; set; }
    }
}
