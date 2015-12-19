using System.Xml.Serialization;

namespace CustomBPM.Core.MetaData
{
    public class ActivityLink
    {
        public ActivityLink()
        {
        }

        public ActivityLink(string code, bool isMain)
        {
            Code = code;
            IsMain = isMain;
        }
        [XmlAttribute]
        public string Code { get; set; }
        [XmlAttribute]
        public string ConditionCode { get; set; }
        [XmlAttribute]
        public bool IsMain { get; set; }
    }
}
