using Dal.Core.Entities;

namespace CustomBPM.Core.DataModel
{
    public class ProcessInstanceField :EntityBase<long>
    {
        public virtual ProcessInstance ProcessInstance { get; set; }
        public virtual long ProcessInstanceId { get; set; }

        public string Name { get; set; }
        public string Value { get; set; }
        
    }
}
