using System;
using System.Data;
using System.Xml;
using RestSharp;
using uStore.Common.BLL;
using uStoreAPI.PluginBase.RecipientList;

namespace DelayedRecipientList
{
    public class DelayedDownloadRecipientList : RLLogicPluginBase
    {
        #region props
        /// <summary>
        /// Determines whether the Recipient List has price that should be added to the total Order Item price
        /// </summary>
        public override bool HasPrice
        {
            get { return true; }
        }

        /// <summary>
        /// Determines if the Recipient List is editable
        /// </summary>
        public override bool IsEditable
        {
            get { return false; }
        }

        /// <summary>
        /// Determines whether the Recipient List should be duplicated into the Recipient List Repository and be available for other orders through the Recipient List Manager
        /// </summary>
        public override bool IsRepositable
        {
            get { return true; }
        }

        #endregion


        public DelayedDownloadRecipientList(int RecipientListModelID, int storeID, int OrderProductID) : base(RecipientListModelID, storeID, OrderProductID)
        {
        }

        public DelayedDownloadRecipientList(XmlDocument pConfiguration) : base(pConfiguration)
        {
        }

        /// <summary>
        /// Calculates the Recipient List price
        /// </summary>
        public override double calculatePrice()
        {
            ListDataConfig config = ConfigUtils.LoadConfiguration(ConfigurationXML);
            int numberOfRecipients = ConfigUtils.GetMaxRecipientsFromSelectionXml(SelectionXML).Value;

            return Convert.ToDouble(config.Price * numberOfRecipients);
        }

        /// <summary>
        /// Fetches and updates the Recipient List in an order where the Recipient List was set to RecipientListStatus UnderConstruction
        /// Used for delayed Recipient Lists that fetched after order submission.
        /// </summary>
		public override ValidationResult updateListFromProvider()
        {
            ValidationResult result = new ValidationResult(false);

            try
            {
                ListDataConfig config = ConfigUtils.LoadConfiguration(ConfigurationXML);

                // Call API URL to get list data
                string listId       = ConfigUtils.GetListIdFromSelectionXml(SelectionXML);
                string listGUID     = ConfigUtils.GetListGUIDFromSelectionXml(SelectionXML);

				// Call the API url to get the list
                RestClient client   = new RestClient(config.ApiUrl);
                RestRequest request = new RestRequest("", Method.GET);

				request.AddParameter("action", "DownloadList");
				request.AddParameter("externalTrackingType", "ListId");
				request.AddParameter("externalTrackingId", listId);
                request.AddParameter("listGuid", listGUID);
                request.AddParameter("format", "uStore");
				request.AddParameter("type", "xml");

				// Execute the request
				IRestResponse<ListDataResponse> response = client.Execute<ListDataResponse>(request);

				// Is the list ready?
				if (response.Data.success && response.Data.list_count > 0)
				{
					// XML Format
					//
					// Important: Must contain <RecipitentList></RecipitentList>
					// for each record. The spelling is wrong but required
					//
					// <?xml version=\"1.0\" encoding=\"utf-16\"?>
					// <NewDataSet><RecipitentList><first>John</first></RecipitentList></NewDataSet>
					XmlDocument xml = new XmlDocument();
					xml.LoadXml(response.Data.xml_data);

					RecipientListXML    = xml;
					RecipientListStatus = uStoreStatus.StrStatus.Active;
                    NumberOfRecipients  = response.Data.list_count;

					// Save the recipient list to the uStore system
					SaveRecipientList();

					// The returned ValidationResult should be set to true only if the recipient list is ready.
					// This is important in order to avoid moving the order out from Pending Recipient List queue.
	                result.IsValid = true;

					// The following message is being logged by uStore. Not mandatory.
					result.AddInfoMsg("Recipient List plugin sample - delayed recipient list was successfully fetched.");
				}
            }
            catch (Exception e)
            {
                result.AddErrorMsg(e.Message);
            }

            return result;
        }

        /// <summary>
        /// Duplicate the recipient list configuration in product level.
        /// </summary>
        public override void Duplicate(Microsoft.Practices.EnterpriseLibrary.Data.Database db, IDbTransaction transaction, int srcProductID, int srcDocID, int destProductID, int destDocID)
        {
            // This method is called when duplicating a product or a store in the uStore back office.
            // if you save additional settings outside of uStore, you can duplicate them here. Otherwise, you can leave this method empty.
        }

        /// <summary>
        /// DoCheckout is called when the customer checks out.
        /// </summary>
        // public virtual void DoCheckout()
        // {
            // Override DoCheckout if you need to execute a specific action only when the customer checks out (such as actions you want to perform only after the customer was billed).
            // Otherwise you can leave this method empty.
        // }

        /// <summary>
        /// Duplicates the Recipient List selection of one Order Product to another when reordering an Order Product
        /// </summary>
        public override ValidationResult Reorder(OrderProductDetails oldOrderProduct, OrderProductDetails newOrderProduct)
        {
            ValidationResult vr = new ValidationResult(true);
            // Add special logic which is required in order to duplicate the recipient list.
            // If such logic is not required (as in case of this sample), there is no need to override this method.
            return vr;
        }

        /// <summary>
        /// Validates the Recipient List data
        /// </summary>
        public override ValidationResult ValidateConfiguration()
        {
            //Add logic for validation here:
            return new ValidationResult(true);
        }
    }
}
