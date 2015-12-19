// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Condition.cs" company="Ingenius Systems">
//   Copyright (c) Ingenius Systems
//   Create on 05.05.2015 14:35:45 by Albert Zakiev
// </copyright>
// <summary>
//   Defines the Condition type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Xml.Serialization;

namespace CustomBPM.Core.MetaData
{
    public enum ConditionType
    {
        Input,
        
        Output
    }

    public class Condition
    {
        [XmlAttribute]
        public string Key { get; set; }

        [XmlAttribute]
        public ConditionType Type { get; set; }
    }
}