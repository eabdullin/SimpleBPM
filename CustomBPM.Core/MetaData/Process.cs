using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace CustomBPM.Core.MetaData
{
    public class Process
    {
        private List<ProcessField> _fields;
        private List<Activity> _activities;
        
        [XmlAttribute]
        public string Code { get; set; }
        [XmlAttribute]
        public string Name { get; set; }

        public virtual List<Activity> Activities
        {
            get { return _activities ?? (_activities = new List<Activity>()); }
            set { _activities = value; }
        }

        public virtual List<ProcessField> Fields
        {
            get { return _fields ?? (_fields = new List<ProcessField>()); }
            set { _fields = value; }
        }

        /// <summary>
        /// Сгенрировать основной воркфлоу процесса
        /// </summary>
        /// <returns></returns>
        public List<Activity> GetMainWorkFlow()
        {
            List<Activity> result = new List<Activity>();
            var activity = _activities.First(x => x.IsStart);
            result.Add(activity);
            while (!activity.IsEnd)
            {
                //пытаемся найти следующий активити
                activity =_activities.First(
                    x => x.Code == activity.AllowOutputActivities.First(o => o.IsMain).Code //находим в выходных ссылках, то которое помечено главным
                        && x.AllowInputActivities.Any(i => i.Code == activity.Code && i.IsMain));//и проверяем что у него был вход с этого активити
                result.Add(activity);
            }
            return result;
        } 
    }
}
