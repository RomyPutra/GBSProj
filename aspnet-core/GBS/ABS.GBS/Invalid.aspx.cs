using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ABS.GBS.Log;
//using ABS.Logic.GroupBooking.Agent;


namespace GroupBooking.Web
{
    public partial class Invalid : System.Web.UI.Page
    {
        SystemLog SystemLog = new SystemLog();
        protected void Page_Load(object sender, EventArgs e)
        {
            //Sharpbrake.Client.IHttpContext contxt = new Sharpbrake.Client.IHttpContext ;
            //contxt.Parameters.Add("Page Load", "testing 123");

            try
            {
                MessageList msgList = new MessageList();
                if (Request.QueryString["err"] != null)
                {
                    string code = Request.QueryString["err"].ToString();
                    switch (code)
                    {
                        case "101":
                            diverrmsg.InnerHtml = msgList.Err100012;
                            break;
                        case "102":
                            diverrmsg.InnerHtml = msgList.Err100011;
                            break;
                        case "108":
                            diverrmsg.InnerHtml = msgList.Err100057;
                            break;
                        case "109":
                            diverrmsg.InnerHtml = msgList.Err100056;
                            break;

                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
            }

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Session["AgentSet"] = null;
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.close()", true);
        }
    }
}