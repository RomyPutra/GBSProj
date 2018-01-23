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
using DevExpress.Data;
using DevExpress.XtraGrid;
using System.Text.RegularExpressions;
using System.Configuration;
using ABS.Navitaire.BookingManager;
using ABS.GBS.Log;

namespace GroupBooking.Web
{
    public partial class SelectInsurance : System.Web.UI.Page
    {
        ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        SystemLog SystemLog = new SystemLog();
        LogControl log = new LogControl();
        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        UserSet AgentSet;
        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
        DataTable dtPass;
        List<PassengerContainer> lstPassengerContainer = new List<PassengerContainer>();
        List<PassengerContainer> lstPassengerContainerNew = new List<PassengerContainer>();
        ABS.GBS.eService.ProcessServiceSoapClient eWS = new ABS.GBS.eService.ProcessServiceSoapClient();
        List<BookingTransactionDetail> listBookingDetail = new List<BookingTransactionDetail>();
        List<BookingTransactionDetail> listDetailCombinePNR = new List<BookingTransactionDetail>();
        BookingTransactionDetail objBK_TRANSDTL_Infos;
        List<BookingTransactionDetail> objListBK_TRANSDTL_Infos = new List<BookingTransactionDetail>();
        ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model = null;
        ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model2 = null;
        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();


        decimal balancedue = 0;
        string TransID;
        protected void Page_Load(object sender, EventArgs e)
        {
            string keySent = Request.QueryString["k"];
            ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
            TransID = Request.QueryString["TransID"];
            string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), TransID.ToString(), "");

            if (hashkey != keySent)
            {
                Response.Redirect("~/Invalid.aspx");
            }

            if (!IsPostBack)
            {
                if (Session["AgentSet"] != null)
                {
                    AgentSet = (UserSet)Session["AgentSet"];
                }
                ClearSession();
                LoadData();
            }
        }

        protected void ClearSession()
        {

        }

        protected void LoadData()
        {
            List<ABS.Logic.GroupBooking.Booking.BookingTransactionDetail> BookingTransactionDetail = new List<BookingTransactionDetail>();
            try
            {
                BookingTransactionDetail = objBooking.Get_TRANSDTL(AgentSet.AgentID, TransID);

                if (Session["dtPassengers"] != null)
                {
                    dtPass = (DataTable)Session["dtPassengers"];
                    Session["dtPassengers"] = dtPass;

                    if (dtPass != null)
                    {
                        gvPassenger.DataSource = dtPass;
                        gvPassenger.DataBind();
                        gvPassenger.ExpandAll();



                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                throw ex;
            }
        }
    }
}