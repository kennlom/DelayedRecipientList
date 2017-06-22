using System.Xml;

namespace DelayedRecipientList
{
    internal static class ConfigUtils
    {
		/// <summary>
		/// Creates ConfigurationXml containing recipient list price for one record 
		/// </summary>
		/// /*
		//  public static XmlDocument CreateConfigurationXml(decimal price)
		// {
		//    XmlDocument document = new XmlDocument();
		//     document.LoadXml(string.Format("<Config><Price>{0}</Price></Config>", price));
		//     return document;
		// }
        public static XmlDocument CreateConfigurationXml(string url, string json, string price)
		{
            decimal pricePerPiece = decimal.Parse(price);

			XmlDocument document = new XmlDocument();
            document.LoadXml(string.Format("<Config><Json>{0}</Json><ApiUrl>{1}</ApiUrl><Price>{2}</Price></Config>", json, url, pricePerPiece));
			return document;
		}

        /// <summary>
        /// Loads the configuration.
        /// </summary>
        public static ListDataConfig LoadConfiguration(XmlDocument configurationXml)
        {
            ListDataConfig config = new ListDataConfig();

			if (configurationXml != null)
			{
				XmlNode json = configurationXml.SelectSingleNode("//Json");
                XmlNode price = configurationXml.SelectSingleNode("//Price");
                XmlNode apiUrl = configurationXml.SelectSingleNode("//ApiUrl");

				if (json != null)
				{
                    config.JSONData = json.InnerText;
				}

                if (price != null)
                {
                    config.Price = decimal.Parse(price.InnerText);
                }

                if (apiUrl != null)
                {
                    config.ApiUrl = apiUrl.InnerText;
                }

                return config;
			}

			return null;
        }


        /// <summary>
        /// Create an XML with the customer selections 
        /// </summary>        
        public static XmlDocument CreateSelectionXml(string listId, string listGuid, int maxRecipients, int sampleCount, string sampleInfo)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(string.Format("<Selection><MaxRecipients>{0}</MaxRecipients><ListId>{1}</ListId><ListGUID>{2}</ListGUID><SampleCount>{3}</SampleCount><SampleInfo>{4}</SampleInfo></Selection>", maxRecipients, listId, listGuid, sampleCount, sampleInfo));
            return document;
        }

        /// <summary>
        /// Get max number of recipients as configured in SelectionXml
        /// </summary>        
        public static int? GetMaxRecipientsFromSelectionXml(XmlDocument selectionXml)
        {
            if (selectionXml != null)
            {
                XmlNode node = selectionXml.SelectSingleNode("//MaxRecipients");
                if (node != null)
                {
                    int maxRecipients;
                    if (int.TryParse(node.InnerText, out maxRecipients))
                    {
                        return maxRecipients;
                    }
                }
            }
            return null;
        }

        public static string GetListIdFromSelectionXml(XmlDocument selectionXml)
        {
            if (selectionXml != null)
            {
                XmlNode node = selectionXml.SelectSingleNode("//ListId");
                if (node != null)
                {
                    return node.InnerText;
                }
            }
            return null;
        }

		public static string GetListGUIDFromSelectionXml(XmlDocument selectionXml)
		{
			if (selectionXml != null)
			{
				XmlNode node = selectionXml.SelectSingleNode("//ListGuid");
				if (node != null)
				{
					return node.InnerText;
				}
			}
			return null;
		}


    }
}

