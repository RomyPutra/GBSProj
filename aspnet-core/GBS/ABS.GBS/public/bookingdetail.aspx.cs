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

namespace GroupBooking.Web
{
    public partial class bookingdetail : System.Web.UI.Page
    {
        UserSet AgentSet;
        string TransID;
        protected void Page_Load(object sender, EventArgs e)
        {
            //return;
            TransID = Request.QueryString["TransID"];
            string keySent = Request.QueryString["k"];
            if (Request.QueryString["payment"] != null)
            {
                Response.Redirect(Shared.MySite.PublicPages.BookingDetail + "?k=" + keySent + "&TransID=" + TransID, false);
            }

            if (Session["AgentSet"] != null)
            {
                AgentSet = (UserSet)Session["AgentSet"];
                Session["modePage"] = "agent";
            }
            else
            {
                Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.SessionExpired);
            }

            ClearSession();
        }

        //added by diana 20170310, to remove seat session before manage seat
        private void ClearSession()
        {
            HttpContext.Current.Session.Remove("SeatInfo0Xml");
            HttpContext.Current.Session.Remove("SeatInfo1Xml");
            HttpContext.Current.Session.Remove("SeatInfo2Xml");
            HttpContext.Current.Session.Remove("SeatInfo3Xml");
            HttpContext.Current.Session.Remove("pAvailableSeatInfo0");
            HttpContext.Current.Session.Remove("pAvailableSeatInfo1");
            HttpContext.Current.Session.Remove("pAvailableSeatInfo2");
            HttpContext.Current.Session.Remove("pAvailableSeatInfo2");
            HttpContext.Current.Session.Remove("SeatInfo0");
            HttpContext.Current.Session.Remove("SeatInfo1");
            HttpContext.Current.Session.Remove("SeatInfo2");
            HttpContext.Current.Session.Remove("SeatInfo3");
            HttpContext.Current.Session.Remove("ErrorMsg");
            HttpContext.Current.Session.Remove("DepartSeatInfo");
            HttpContext.Current.Session.Remove("ReturnSeatInfo");
            HttpContext.Current.Session.Remove("DepartConnectingSeatInfo");
            HttpContext.Current.Session.Remove("ReturnConnectingSeatInfo");
            HttpContext.Current.Session.Remove("DepartConnectingExistingSeatInfo");
            HttpContext.Current.Session.Remove("DepartConnectingExistingSeatInfo2");
            HttpContext.Current.Session.Remove("ReturnConnectingExistingSeatInfo");
            HttpContext.Current.Session.Remove("ReturnConnectingExistingSeatInfo2");
            HttpContext.Current.Session.Remove("DepartExistingSeatInfo");
            HttpContext.Current.Session.Remove("ReturnExistingSeatInfo");
        }
    }


}