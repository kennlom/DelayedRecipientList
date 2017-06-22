using System;

namespace DelayedRecipientList
{
    public class ListDataConfig
    {
        public decimal Price { get; set; }
        public int OrderProductId { get; set; }
        public int ListID { get; set; }
        public string ListGUID { get; set; }
        public string ApiUrl { get; set; }
        public string JSONData { get; set; }

        public ListDataConfig()
        {
            // Init / reset all
            this.Price          = 0;
            this.OrderProductId = 0;
            this.ListID         = 0;
            this.ListGUID       = "";
            this.ApiUrl         = "";
            this.JSONData       = "";
        }
    }
}
