namespace BPM.Common {
	public class WorkItem {
		private long _requestId = -1;
		private int _activityId = -1;
		private int _processId = -1;
		private string _title, _eventDdata, _action, _licensorCode, _divisionCode, _role;

	    public long RequestId { get; set; }

	    public long ProcessId { get
	        ; set; }

	    public string Title { get; set; }
		public string Task { get; set; }
		public string Assigner { get; set; }
		public string Action { get; set; }

		/// <summary>
		/// было возвращено на доработку
		/// </summary>
		public bool Reverted { get; set; }

		public string Role { get; set; }

		public long ActivityId { get; set; }

		public int Id { get; set; }

	}
}
