using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using ABS.Logic.GroupBooking.Agent;
using ABS.Logic.GroupBooking.Booking;
using SEAL.Data;
using DevExpress.Web;
using System.Collections;
using ABS.Logic.Shared;
using System.Globalization;
//using log4net;

namespace GroupBooking.Web.admin
{
    public partial class adminbookingdetail : System.Web.UI.Page
    {
        AdminSet AdminSet;
        string TransID;
        protected void Page_Load(object sender, EventArgs e)
        {
            TransID = Request.QueryString["TransID"];
            string keySent = Request.QueryString["k"];
            if (Request.QueryString["payment"] != null)
            {
                Response.Redirect(Shared.MySite.AdminPages.AdminBookingDetail + "?k=" + keySent + "&TransID=" + TransID, false);
            }

            if (Session["AdminSet"] != null)
            {
                AdminSet = (AdminSet)Session["AdminSet"];
                Session["modePage"] = "admin";
            }
            else
            {
                Response.Redirect(Shared.MySite.AdminPages.AdminLogin);
            }
        }
    }
}