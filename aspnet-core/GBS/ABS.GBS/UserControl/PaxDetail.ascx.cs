using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ABS.Logic.GroupBooking.Booking;
using ABS.Logic.Shared;
using System.Data;
using ABS.GBS.Log;

namespace ABS.GBS.UserControl
{
    public partial class PaxDetail : System.Web.UI.UserControl
    {
        BookingControl objBooking = new BookingControl();
        string TransID = "";
        string RecordLocator = "";
        string FilterPNR = "";
        UserSet AgentSet;
        AdminSet AdminSet;
        SystemLog SystemLog = new SystemLog();

        protected void Page_Load(object sender, EventArgs e)
        {
            TransID = Request.QueryString["TransID"];
            RecordLocator = Request.QueryString["PNR"];

            if (Session["AgentSet"] != null)
            {
                AgentSet = (UserSet)Session["AgentSet"];
            }
            if (Session["AdminSet"] != null)
            {
                AdminSet = (AdminSet)Session["AdminSet"];
            }

            Load_FlightDetails();
        }

        protected void lnkPNR_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string whatPNR = btn.CommandArgument;
            // do what you need here
        }

        protected void lnkBookingID_OnClick(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
            string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), TransID, "");

            if (Session["AgentSet"] != null)
                Response.Redirect("../public/bookingdetail.aspx?k=" + hashkey + "&TransID=" + TransID);
            if (Session["AdminSet"] != null)
                Response.Redirect("../admin/adminbookingdetail.aspx?k=" + hashkey + "&TransID=" + TransID);
        }

        protected void Load_FlightDetails()
        {
            try
            {
                //lblBookingID.Text = TransID;
                lnkBookingID.Text = TransID;

                DataTable dt, dt1;
                DataRow dr;
                dt = objBooking.GetBK_AllPNR(TransID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    dt1 = new DataTable();
                    dt1.Columns.Add("RecordLocator");
                    dt1.Columns.Add("PNR");
                    lnkPNRAll.Text = dt.Rows[0]["PNR"].ToString().Substring(13,2);
                    for (int i = 1; i < dt.Rows.Count; i++){
                        dr = dt1.NewRow();
                        dr[0] = dt.Rows[i]["RecordLocator"].ToString();
                        dr[1] = dt.Rows[i]["PNR"].ToString();
                        dt1.Rows.Add(dr);
                    }
                    //LeftSide PNR List
                    rptPNR.DataSource = dt1;
                    rptPNR.DataBind();
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                throw ex;
            }
            finally
            {
                objBooking = null;
            }
        }

    }
}