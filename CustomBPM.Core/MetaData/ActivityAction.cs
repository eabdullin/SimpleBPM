using System.Xml.Serialization;

namespace CustomBPM.Core.MetaData
{
    public class ActivityAction
    {
        [XmlAttribute]
        public string Code { get; set; }

        [XmlAttribute]
        public ActionType Type { get; set; }
    }

    public enum ActionType
    {
        [XmlEnum(Name = "Input")]
        Input,

        [XmlEnum(Name = "Output")]
        Output,

        [XmlEnum(Name = "ByUser")]
        ByUser
    }
}
