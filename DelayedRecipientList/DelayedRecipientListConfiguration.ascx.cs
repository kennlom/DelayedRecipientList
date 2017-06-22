using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Xml;
using uStoreAPI.PluginBase.RecipientList;
using uStore.Common.BLL;
using System.Web.UI.WebControls;

namespace DelayedRecipientList
{
    public partial class DelayedRecipientListConfiguration : RLConfigurationControlBase
    {
        /// <summary>
        /// Initialize the controls on the page.
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // must be after base.OnLoad to let parent fill ConfigurationXML in the first time
            if (!IsPostBack)
            {
                ListDataConfig config = ConfigUtils.LoadConfiguration(ConfigurationXML);

                // Load previous value from configuration xml
                txtAPIUrl.Text          = config.ApiUrl;
                txtMailPieceJSON.Text   = config.JSONData;
                txtPricePerPiece.Text   = config.Price.ToString();
			}
        }

        /// <summary>
        /// Validates the Configuration control
        /// </summary>
        protected override ValidationResult ValidateUI()
        {
            ValidationResult result = base.ValidateUI();

            if (result.IsValid)
            {
				// Possibly try to validate the url, and json
				// result.IsValid = false;
				// result.AddErrorMsg("Invalid JSON");
			}

            return result;
        }

        /// <summary>
        /// Saves the Recipient List configuration
        /// </summary>
        protected override ValidationResult SaveUI()
        {
            // Save the data defined by the user into the configuration xml member. 
            // The framework will take care of saving the xml to the database, and loading it the next time the control is used.
            ConfigurationXML = ConfigUtils.CreateConfigurationXml(txtAPIUrl.Text, txtMailPieceJSON.Text, txtPricePerPiece.Text);
			
			return new ValidationResult(true);
        }
    }
}
