using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using ABS.Logic.GroupBooking.Agent;

namespace GroupBooking.Web
{
    public partial class SessionExpired : System.Web.UI.Page
    {
        MessageList msgList = new MessageList();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["msg"] != null && Request.QueryString["msg"].ToString() == "Err100060")
            {
                message.InnerText = msgList.Err100060;
            }
        }
    }
}