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
using ABS.Navitaire.BookingManager;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Threading;
using DevExpress.Utils;
using System.Web.Services;
using ABS.GBS.Log;
using ABS.GBS;

namespace GroupBooking.Web
{
	public partial class InsureNone : System.Web.UI.Page
    {
        #region declaration
        UserSet MyUserSet;
        LogControl log = new LogControl();
        SystemLog SystemLog = new SystemLog();

        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        Bk_transaddon BK_TRANSADDONInfo = new Bk_transaddon();
        List<Bk_transaddon> listBK_TRANSADDONInfo = new List<Bk_transaddon>();
        List<BookingTransactionDetail> objListBK_TRANSDTL_Infos = new List<BookingTransactionDetail>();

        List<BookingTransactionDetail> lstbookFlightInfo = new List<BookingTransactionDetail>();
        List<BookingTransactionDetail> lstRejectedbookDTLInfo = new List<BookingTransactionDetail>();
        List<BookingTransactionDetail> listDetailCombinePNR = new List<BookingTransactionDetail>();

        ABS.GBS.GetPlan GetPlan = new ABS.GBS.GetPlan();
        List<Flight> lstFlight = new List<Flight>();
        List<IPassenger> lstPassenger = new List<IPassenger>();
        List<BookingTransactionDetail> dtFlight = new List<BookingTransactionDetail>();
        TravelPlanResponse RespPlan = new TravelPlanResponse();//added by romy, 20170818, Insurance
        BK_Insure Insureadd = new BK_Insure();
        List<BK_Insure> List_Insureadd = new List<BK_Insure>();
        InsureResponse BK_RespInsure = new InsureResponse();
        List<InsureResponse> listBK_RespInsure = new List<InsureResponse>();
        PurchaseResponse RespPurchase = new PurchaseResponse();

        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
        ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");

        ABS.GBS.eService.ProcessServiceSoapClient eWS = new ABS.GBS.eService.ProcessServiceSoapClient();
        List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
        BookingTransactionMain bookHDRInfo = new BookingTransactionMain();
        ABS.Logic.GroupBooking.Agent.AgentControl objAgent = new ABS.Logic.GroupBooking.Agent.AgentControl();
        ABS.Logic.GroupBooking.Agent.AgentProfileControl objAgentProfile = new ABS.Logic.GroupBooking.Agent.AgentProfileControl();
        ABS.Logic.GroupBooking.Agent.AgentProfile agent = new ABS.Logic.GroupBooking.Agent.AgentProfile();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            string signature = "";
            DataTable dt = new DataTable();
            try
            {
                ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking("");
                Session["PaxStatus"] = "";
                Session["dtGridPassOld"] = "";
                if (Session["AgentSet"] != null)
                {
                    MyUserSet = (UserSet)Session["AgentSet"];
                }
                else
                {
                    Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.SessionExpired);
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
        }
    }
}