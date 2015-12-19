using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace CustomBPM.Core.MetaData
{
    public class Activity
    {
        private List<ActivityLink> _allowOutputActivities;
        private List<ActivityLink> _allowInputActivities;
        private List<Condition> _conditions;
		private List<ActivityAction> _actions;
        private List<ActivityProperty> _properties;

        [XmlAttribute]
        public string Code { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public bool IsStart { get; set; }

        [XmlAttribute]
        public bool IsEnd { get; set; }

        [XmlAttribute]
         public bool IsProcessEnd { get; set; }

        public virtual List<ActivityProperty> Properties
        {
            get { return _properties ?? (_properties = new List<ActivityProperty>()); }
            set { _properties = value; }
        }

        public virtual List<ActivityLink> AllowInputActivities
        {
            get { return _allowInputActivities ?? (_allowInputActivities = new List<ActivityLink>()); }
            set { _allowInputActivities = value; }
        }

        public virtual List<ActivityLink> AllowOutputActivities
        {
            get { return _allowOutputActivities ?? (_allowOutputActivities = new List<ActivityLink>()); }
            set { _allowOutputActivities = value; }
        }

        public List<Condition> Conditions
        {
            get { return _conditions ?? (_conditions = new List<Condition>()); }
            set { _conditions = value; }
        }
            
        public virtual List<ActivityAction> ActivityActions
        {
            get { return _actions ?? (_actions = new List<ActivityAction>()); }
            set { _actions = value; }
        }
        
        public string[] AllowRoles { get; set; }

        public bool Allow(Activity activity, string[] roles)
        {
            var result = true;
            result &= AllowInputActivities.Any(x => x.Code == activity.Code);
            result &= activity.AllowOutputActivities.Any(x => x.Code == this.Code);
            result &= AllowRoles.Any(roles.Contains);
            return result;
        }

        public string this[string propertyName]
        {
            get
            {
                ActivityProperty property = _properties.SingleOrDefault(x => x.Name == propertyName);
                return property == null ? null : property.Value;
            }
        }
    }
}
