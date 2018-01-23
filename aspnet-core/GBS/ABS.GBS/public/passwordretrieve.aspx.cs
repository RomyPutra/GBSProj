using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using ABS.GBS.Log;

namespace GroupBooking.Web
{
	public partial class passwordretrieve : System.Web.UI.Page
	{
        SystemLog SystemLog = new SystemLog();
		protected void Page_Load(object sender, EventArgs e)
		{

		}

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ABS.GBS.eService.ProcessServiceSoapClient sServices = new ABS.GBS.eService.ProcessServiceSoapClient();
            try
            {
                if (txtEmail.Text == string.Empty)
                {
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.Text = "Please fill in your e-mail address before submitting";
                    txtEmail.IsValid = false;
                }
                if (txtEmail.IsValid)
                {
                    //do send email
                    //eService.ABS.pro

                    string msg = sServices.GroupBookingEmailing(2, "0", txtEmail.Text,"",0, "");
                    if (msg != string.Empty && msg == "Error with EmailBody")
                    {
                        lblErrorMsg.Text = msg;
                        return;
                    }
                    else
                    {
                        Response.Redirect("../Message.aspx?msgID=107", false);
                    }
                }

                
                
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);

            }

        }
	}
}