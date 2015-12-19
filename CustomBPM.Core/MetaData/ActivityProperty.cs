using System.Xml.Serialization;

namespace CustomBPM.Core.MetaData
{
    public class ActivityProperty
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Value { get; set; }
    }
}
