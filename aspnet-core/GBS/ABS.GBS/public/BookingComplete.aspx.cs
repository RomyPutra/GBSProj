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

namespace GroupBooking.Web
{
	public partial class BookingComplete : System.Web.UI.Page
	{
        UserSet AgentSet;
        string TransID;
		protected void Page_Load(object sender, EventArgs e)
		{
            TransID = Request.QueryString["TransID"];
            string keySent = Request.QueryString["k"];

            lnkBookingID.Text = TransID;

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

        protected void lnkBookingID_OnClick(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
            string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), TransID, "");

            if (Session["AgentSet"] != null)
                Response.Redirect("../public/bookingdetail.aspx?k=" + hashkey + "&TransID=" + TransID + "&payment=1", false);
            if (Session["AdminSet"] != null)
                Response.Redirect("../admin/adminbookingdetail.aspx?k=" + hashkey + "&TransID=" + TransID + "&payment=1", false);
        }
	}
}