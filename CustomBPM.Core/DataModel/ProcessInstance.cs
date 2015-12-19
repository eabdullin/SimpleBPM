using System;
using System.Collections.Generic;
using System.Linq;
using Dal.Core.Entities;

namespace CustomBPM.Core.DataModel
{
    public class ProcessInstance : EntityBase<long>
    {
        private ICollection<ActivityInstance> _activities;
        private ICollection<ProcessInstanceField> _fields;
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ActivityInstance> Activities
        {
            get { return _activities ?? (_activities = new List<ActivityInstance>()); }
            set { _activities = value; }
        }

        public virtual ICollection<ProcessInstanceField> Fields
        {
            get { return _fields ?? (_fields = new List<ProcessInstanceField>()); }
            set { _fields = value; }
        }

        public string CurrentActivityCode { get; set; }

        public ActivityInstance CurrentActivity
        {
            get { return Activities.FirstOrDefault(x => x.Code == CurrentActivityCode); }
        }

        public IDictionary<string, string> ConvertFields()
        {
            var fields = Fields.ToDictionary(x => x.Name, x => x.Value);
            fields.Add("Id", Id.ToString());

            return fields;
        }

        public void AddField(string name, string value)
        {
            Fields.Add(new ProcessInstanceField()
            {
                Name = name,
                Value = value
            });
        }
        public ProcessStatus Status { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
