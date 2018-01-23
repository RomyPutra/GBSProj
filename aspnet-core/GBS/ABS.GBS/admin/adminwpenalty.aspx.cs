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
    public partial class adminwpenalty : System.Web.UI.Page
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
                    Session["dtGrid"] = null;
                    TransMainData = objBooking.GetAllTransWithPenalty();
                    
                }
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
        protected void LoadGridView()
        {
            Session["dtGrid"] = TransMainData;
            if (Session["dtGrid"] != null)
            {
                


                gvAvaPenalty.DataSource = (List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>)Session["dtGrid"];
                gvAvaPenalty.DataBind();
            }
            else
            {
                
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
            ReqInfo.ReqType = "F";
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
                TransMainData = objBooking.GetAllTransWithPenalty();
                LoadGridView();

            }
        }
        
    }
}