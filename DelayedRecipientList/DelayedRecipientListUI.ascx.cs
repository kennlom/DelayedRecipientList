using System;
using System.Xml;
using RestSharp;
using uStore.Common.BLL;
using uStoreAPI.PluginBase.RecipientList;

namespace DelayedRecipientList
{
    public partial class DelayedRecipientListUI : RLUIPluginBase
    {
        /// <summary>
        /// Recipient list logic plugin
        /// </summary>
        private DelayedDownloadRecipientList LogicPlugin
        {
            get { return (DelayedDownloadRecipientList)IRLLogicPlugin; }
        }

        /// <summary>
        /// Determine if this is the first time the control is loaded.
        /// Due to the dyncmic loading mechanism of the control we cannot rely on 
        /// the IsPostBack flag but rather we need to maintain a flag of our own.
        /// </summary>
        private bool FirstTime
        {
            get
            {
                if (ViewState["FirstTime"] == null)
                {
                    ViewState["FirstTime"] = string.Empty;
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Page load
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FirstTime)
            {
                ListDataConfig config = ConfigUtils.LoadConfiguration(LogicPlugin.ConfigurationXML);

                // Load the mail piece json data from the configuration
                txtMailPieceJSON.Text = config.JSONData;

                // Get api url
                txtApiUrl.Value = config.ApiUrl;

                // Update hidden OrderProductId
                int OrderProductId = LogicPlugin.OrderProductDetails.OrderProductID;
                txtItemOrderProductId.Value = OrderProductId.ToString();
            }
        }

		/// <summary>
		/// Gets the list response.
		/// </summary>
		protected IRestResponse<ListDataResponse> GetListResponse(string action)
		{
			// Get OrderProductId
			int OrderProductId = LogicPlugin.OrderProductDetails.OrderProductID;

			// Load the configuration
			ListDataConfig config = ConfigUtils.LoadConfiguration(LogicPlugin.ConfigurationXML);

			// Call the API url to check the list status
			RestClient client = new RestClient(config.ApiUrl);
			RestRequest request = new RestRequest("", Method.GET);

			request.AddParameter("action", action);
			request.AddParameter("ExternalTrackingType", "OrderProductId");
			request.AddParameter("ExternalTrackingId", OrderProductId);

			// Execute the request
			return client.Execute<ListDataResponse>(request);
		}

        /// <summary>
        /// Validate the user entered the required data and in the correct format.
        /// </summary>
        protected override ValidationResult ValidateUI()
        {
            ValidationResult result = base.ValidateUI();

			if (result.IsValid)
            {
                // Get list status
                IRestResponse<ListDataResponse> response = this.GetListResponse("CheckListStatus");

                // Is the list ready?
                if (response.Data.success)
                {
                    result.IsValid = true;
                }
                else
                {
					result.IsValid = false;
					result.AddErrorMsg("Your recipient list is not uploaded, or it's still processing.");
                }
            }

            return result;
        }

        /// <summary>
        /// Saves the Recipient List selection of the user
        /// </summary>
        protected override ValidationResult SaveUI()
        {
            ValidationResult result = base.ValidateUI();

            if (result.IsValid)
            {
				// Get list status
				IRestResponse<ListDataResponse> response = this.GetListResponse("CheckListStatus");

                if (response.Data.success)
                {
                    LogicPlugin.RecipientListDescription    = response.Data.list_name;                  // Set recipient list name for future use (use unique names)
                    LogicPlugin.RecipientListStatus         = uStoreStatus.StrStatus.UnderConstruction; // Used for delayed Recipient Lists that fetched after order submission.
                    LogicPlugin.NumberOfRecipients          = response.Data.list_count;
                    LogicPlugin.SelectionXML                = ConfigUtils.CreateSelectionXml(response.Data.list_id.ToString(), response.Data.list_quid, response.Data.list_count, response.Data.sample_count, response.Data.sample_info);
					
					// Create sample recipient list records for proofing

					// XML Format
					//
					// Important: Must contain <RecipitentList></RecipitentList>
					// for each record. The spelling is wrong but required
					//
					// <?xml version=\"1.0\" encoding=\"utf-16\"?>
					// <NewDataSet><RecipitentList><first>John</first></RecipitentList></NewDataSet>
					XmlDocument document = new XmlDocument();
					document.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><NewDataSet><RecipitentList><first>John</first><last>Sample1</last><address>1111 Street Name</address><city>Any City</city><state>ST</state><zip>11111</zip><imbarcode>FADATFAFDDFTAAFDTFDTDFDTATDDTTTTAFFTTADTAAFTAAFDTAAADTFATDDFFDDAD</imbarcode><endorse>*************SCH 5-DIGIT 11111</endorse></RecipitentList><RecipitentList><first>John</first><last>Sample2</last><address>2222 Street Name</address><city>Any City</city><state>ST</state><zip>22222</zip><imbarcode>FADATFAFDDFTAAFDTFDTDFDTATDDTTTTAFFTTADTAAFTAAFDTAAADTFATDDFFDDAD</imbarcode><endorse>*************SCH 5-DIGIT 11111</endorse></RecipitentList><RecipitentList><first>John</first><last>Sample3</last><address>3333 Street Name</address><city>Any City</city><state>ST</state><zip>33333</zip><imbarcode>FADATFAFDDFTAAFDTFDTDFDTATDDTTTTAFFTTADTAAFTAAFDTAAADTFATDDFFDDAD</imbarcode><endorse>*************SCH 5-DIGIT 11111</endorse></RecipitentList><RecipitentList><first>John</first><last>Sample4</last><address>4444 Street Name</address><city>Any City</city><state>ST</state><zip>44444</zip><imbarcode>FADATFAFDDFTAAFDTFDTDFDTATDDTTTTAFFTTADTAAFTAAFDTAAADTFATDDFFDDAD</imbarcode><endorse>*************SCH 5-DIGIT 11111</endorse></RecipitentList><RecipitentList><first>John</first><last>Sample5</last><address>5555 Street Name</address><city>Any City</city><state>ST</state><zip>55555</zip><imbarcode>FADATFAFDDFTAAFDTFDTDFDTATDDTTTTAFFTTADTAAFTAAFDTAAADTFATDDFFDDAD</imbarcode><endorse>*************SCH 5-DIGIT 11111</endorse></RecipitentList></NewDataSet>");

					// Setting recipient list data
				    LogicPlugin.RecipientListXML = document;

					// Save the recipient list to the uStore system
					LogicPlugin.SaveRecipientList();
                }
                else
                {
                    result.IsValid = false;
                }
            }

            return result;
        }
    }
}
