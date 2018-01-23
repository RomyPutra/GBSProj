using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ABS.Logic.Shared;
using DevExpress.Web;
using ABS.Logic.GroupBooking.Booking;
using System.Data;

namespace GroupBooking.Web.admin
{
    public partial class adminchangeflight : System.Web.UI.Page
    {

        #region declaration 
        List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain> TransMainData = new List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>();
        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        ABS.Logic.GroupBooking.Booking.RequestApp ReqInfo = new ABS.Logic.GroupBooking.Booking.RequestApp();
        ABS.Logic.GroupBooking.Settings SetInfo = new ABS.Logic.GroupBooking.Settings();
        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
        AdminSet AdminSet;
        ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");
        
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminSet"] != null)
            {
                AdminSet = (AdminSet)Session["AdminSet"];
                if (!IsPostBack)
                {
                    assignDefaultValue();
                    Session["dtGridFlightChange"] = null;
                    clearSession();
                }                                    
                    LoadGridView();
                
                /*
                if (TransMainData != null)
                {
                    LoadGridView();
                }
                 */
            }
            else
            {
                Response.Redirect(Shared.MySite.AdminPages.AdminLogin,false);
            }
        }

        protected void assignDefaultValue()
        {
            Session["dtGridFlightChange"] = null;
            Session["TransReqTransID"] = null;
        }

        protected void clearSession()
        {            
            HttpContext.Current.Session.Remove("TransMainData");
            HttpContext.Current.Session.Remove("ListDetailData");
            HttpContext.Current.Session.Remove("ReasonData");
            HttpContext.Current.Session.Remove("flagOneWay");
        }

        protected void LoadGridView()
        {
            
            if (Session["dtGridFlightChange"] != null)
            {
                gvChangeFlight.DataSource = (List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>)Session["dtGridFlightChange"];
                gvChangeFlight.DataBind();
            }
            else
            {
                TransMainData = objBooking.GetAllTransFlightChange();
                Session["dtGridFlightChange"] = TransMainData;
                gvChangeFlight.DataSource = (List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>)Session["dtGridFlightChange"];
                gvChangeFlight.DataBind();
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
            #region prev
            /*
            string temp = "";
            AdminSet = (AdminSet)Session["AdminSet"];
            SetInfo = objGeneral.GetSingleSYS_PREFT(1, "AA", "REQEXPIRY");
            temp = DateTime.Now.ToString("yyyyMMddHHmmsss");
            ReqInfo.ReqID = temp;
            ReqInfo.Remark = memoRemarks.Text;
            ReqInfo.ReqType = "CF";
            ReqInfo.TransID = Session["TransReqTransID"].ToString();
            ReqInfo.UserID = AdminSet.AdminID;
            ReqInfo.LastSyncBy = AdminSet.AdminID;
            ReqInfo.RequestDate = DateTime.Now;
            ReqInfo.ExpiryDate = (DateTime.Now).AddDays(Convert.ToInt16(SetInfo.SYSValue));
            objBooking.SaveREQAPPL(ReqInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
             */ 
            #endregion 
                     
            AdminSet = (AdminSet)Session["AdminSet"];

            HttpContext.Current.Session.Remove("ReasonData");
            HttpContext.Current.Session.Add("ReasonData", memoRemarks.Text);             

            List<BookingTransactionDetail> detailCollection = new List<BookingTransactionDetail>();
            detailCollection = objBooking.BookingDetailFilter(Session["TransReqTransID"].ToString());

            CheckDepartReturn(detailCollection);

            HttpContext.Current.Session.Remove("ListDetailData");
            HttpContext.Current.Session.Add("ListDetailData", detailCollection);

            BookingTransactionMain transMain = new BookingTransactionMain();
            transMain = objBooking.GetSingleBK_TRANSMAIN(Session["TransReqTransID"].ToString());

            HttpContext.Current.Session.Remove("TransMainData");
            HttpContext.Current.Session.Add("TransMainData", transMain);

            Response.Redirect(Shared.MySite.AdminPages.AdminSearchFlight, false);
            /*
            DataTable dtPrevJourney = objBooking.dtKeySignature();
            DataTable dtNextJourney = objBooking.dtKeySignature();
            string errMsg = "";
            
            if (detailCollection.Count > 0)
            {
                foreach (BookingTransactionDetail detailData in detailCollection)
                {
                    dtPrevJourney = absNavitaire.GetKeySignatureByPNR(detailData.RecordLocator, ref errMsg);                    

                }
            }*/
        }

        protected void CheckDepartReturn(List<BookingTransactionDetail> detailCollection)
        {
            string firstPNR = detailCollection.ElementAt(0).RecordLocator.ToString();
            int flagCtr = 0;
            Boolean flagOneWay = true;
            foreach (BookingTransactionDetail detailData in detailCollection)
            {
                if (flagCtr != 0)
                {
                    if (firstPNR == detailData.RecordLocator.ToString())
                    {
                        flagOneWay = false;
                        break;
                    }
                }
                flagCtr += 1;
            }
            HttpContext.Current.Session.Remove("flagOneWay");
            HttpContext.Current.Session.Add("flagOneWay",flagOneWay);
        }

        protected void btnRequest_Click(object sender, EventArgs e)
        {
            if (ASPxEdit.AreEditorsValid(callbackPanel))
            {
                
                if (CheckFlightChangeRules())
                {
                    SaveRequest();
                    TransMainData = objBooking.GetAllTransFlightChange();
                    LoadGridView();
                }
                else
                {
                    string error = "Currently Flight Change Not Available";
                    lblError.Text = error;
                    lblError.Visible = true;
                }
            }
        }

        protected Boolean CheckFlightChangeRules()
        {
            Boolean status = true;

            List<BookingTransactionDetail> detailCollection = new List<BookingTransactionDetail>();
            detailCollection = objBooking.BookingDetailFilter(Session["TransReqTransID"].ToString());

            string carrier = detailCollection.ElementAt(0).CarrierCode.ToString();

            //get lowerBound
            int lowerBound = Convert.ToInt32(objGeneral.getSysValueByKeyAndCarrierCode("CHGBOUNDLOWER", carrier));

            if (lowerBound == 0)
            {
                status = false;                
            }
            else
            { 
                //get upperBound
                int upperBound = Convert.ToInt32(objGeneral.getSysValueByKeyAndCarrierCode("CHGBOUNDUPPER", carrier));

                //get total flight change count 
                int totalFlightChange = objBooking.getTotalFlightChangeCount();

                if (totalFlightChange < upperBound)
                {
                    status = true;
                }
                else
                {
                    status = false;
                }
            }
            
            return status;
        }
    }
}