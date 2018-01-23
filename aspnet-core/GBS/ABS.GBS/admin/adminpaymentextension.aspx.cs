using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using ABS.Logic.GroupBooking.Agent;
using ABS.Logic.GroupBooking;
using ABS.Logic.Shared;
using DevExpress.Web;
using ABS.Logic.GroupBooking.Booking;
using System.Globalization;

namespace GroupBooking.Web.admin
{
    public partial class adminpaymentextension : System.Web.UI.Page
    {
        List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain> TransMainData = new List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>();
        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        ABS.Logic.GroupBooking.Booking.RequestApp ReqInfo = new ABS.Logic.GroupBooking.Booking.RequestApp();
        ABS.Logic.GroupBooking.Settings SetInfo = new ABS.Logic.GroupBooking.Settings();
        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
        AdminSet AdminSet;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminSet"] != null)
            {
                AdminSet = (AdminSet)Session["AdminSet"];
                if (!IsPostBack)
                {
                    assignDefaultValue();
                    Session["dtGridPaymentExpiry"] = null;
                    //TransMainData = objBooking.GetAllTransWithPenalty();
                    TransMainData = objBooking.GetAllTransWithExpiryPayment();

                }
                if (Session["dtGridPaymentExpiry"] != null)
                    TransMainData = (List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>)Session["dtGridPaymentExpiry"];
                if (TransMainData != null)
                {
                    LoadGridView();
                }

            }
            else
            {
                Response.Redirect("~/admin/adminlogin.aspx");
            }
        }

        protected void assignDefaultValue()
        {
            Session["dtGridPaymentExpiry"] = null;
            Session["TransReqTransID"] = null;                        
        }

        protected void LoadGridView()
        {
            Session["dtGridPaymentExpiry"] = TransMainData;
            if (Session["dtGridPaymentExpiry"] != null)
            {
                gvAvaPenalty.DataSource = (List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>)Session["dtGridPaymentExpiry"];
                gvAvaPenalty.DataBind();
            }
            else
            {
                gvAvaPenalty.DataSource = null;
                gvAvaPenalty.DataBind();
            }
        }
        protected void callbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            memoRemarks.Text = "";
            lblTransID.Text = (e.Parameter).ToString();
            Session["TransReqTransID"] = lblTransID.Text;
        }
        protected void SaveRequest()
        {
            string temp = "";
            AdminSet = (AdminSet)Session["AdminSet"];
            SetInfo = objGeneral.GetSingleSYS_PREFT(1, "AA", "REQEXPIRY");
            temp = DateTime.Now.ToString("yyyyMMddHHmmsss");
            ReqInfo.ReqID = temp;
            ReqInfo.Remark = memoRemarks.Text;
            ReqInfo.ReqType = "PE";
            ReqInfo.TransID = Session["TransReqTransID"].ToString();
            ReqInfo.UserID = AdminSet.AdminID;
            ReqInfo.LastSyncBy = AdminSet.AdminID;
            ReqInfo.RequestDate = DateTime.Now;
            ReqInfo.ExpiryDate = (DateTime.Now).AddDays(Convert.ToInt16(SetInfo.SYSValue));
            objBooking.SaveREQAPPL(ReqInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
        }

        protected void btnRequest_Click(object sender, EventArgs e)
        {
            if (ASPxEdit.AreEditorsValid(callbackPanel))
            {
                SaveRequest();
                TransMainData = objBooking.GetAllTransWithExpiryPayment();
                LoadGridView();

            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (ddlFilter.SelectedIndex >= 0 && txtAgentID.Text != "")
            {
                switch (ddlFilter.SelectedItem.Value.ToString())
                {
                    case "AgentID":
                        TransMainData = objBooking.GetAllTransWithExpiryPayment(BookingControl.TransactionFilter.AgentID, txtAgentID.Text);
                        break;
                    case "TransID":
                        TransMainData = objBooking.GetAllTransWithExpiryPayment(BookingControl.TransactionFilter.TransID, txtAgentID.Text);
                        break;
                    case "AgentName":
                        TransMainData = objBooking.GetAllTransWithExpiryPayment(BookingControl.TransactionFilter.AgentName, txtAgentID.Text);
                        break;
                }
                LoadGridView();
            }
        }
 
    }
}