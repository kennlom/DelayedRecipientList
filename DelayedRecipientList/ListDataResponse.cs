using System;

namespace DelayedRecipientList
{
	public class ListDataResponse
	{
        public int list_id { get; set; }
		public bool success { get; set; }
        public int list_count { get; set; }
        public string list_quid { get; set; }
        public string list_name { get; set; }
		public string xml_data { get; set; }
        public int sample_count { get; set; }
        public string sample_info { get; set; }


		public ListDataResponse()
		{
			// Init / reset all
			this.success    = false;
			this.list_id    = 0;
			this.list_count = 0;
			this.list_name  = "";
            this.list_quid  = "";
            this.xml_data   = "";
            this.sample_count = 0;
            this.sample_info = "";
		}
	}
}
