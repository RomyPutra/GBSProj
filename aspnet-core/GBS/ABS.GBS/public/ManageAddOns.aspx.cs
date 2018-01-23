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
//using log4net;
using System.Globalization;
using ABS.Navitaire.BookingManager;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Threading;
using DevExpress.Utils;
using System.Web.Services;
using ABS.GBS.Log;
using StackExchange.Profiling;

namespace GroupBooking.Web
{
    public partial class ManageAddOns : System.Web.UI.Page
    {
        #region declaration
        UserSet MyUserSet;
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogControl log = new LogControl();
        SystemLog SystemLog = new SystemLog();

        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        Bk_transssr BK_TRANSSSRInfo = new Bk_transssr();
        List<Bk_transssr> listbk_transssrinfo = new List<Bk_transssr>();
        List<Bk_transssr> listbk_transssrinfo1 = new List<Bk_transssr>();
        List<Bk_transssr> listbk_transssrinfo2 = new List<Bk_transssr>();
        List<Bk_transssr> listbk_transssrinfo1t = new List<Bk_transssr>();
        List<Bk_transssr> listbk_transssrinfo2t = new List<Bk_transssr>();

        Bk_transaddon BK_TRANSADDONInfo = new Bk_transaddon();
        List<Bk_transaddon> listBK_TRANSADDONInfo = new List<Bk_transaddon>();
        List<Bk_transaddon> listBK_TRANSADDONInfo1 = new List<Bk_transaddon>();
        List<Bk_transaddon> listBK_TRANSADDONInfo2 = new List<Bk_transaddon>();
        List<Bk_transaddon> listBK_TRANSADDONInfo1t = new List<Bk_transaddon>();
        List<Bk_transaddon> listBK_TRANSADDONInfo2t = new List<Bk_transaddon>();

        Bk_transaddon BK_TRANSADDONInfoOld = new Bk_transaddon();
        List<Bk_transaddon> listBK_TRANSADDONInfoOld = new List<Bk_transaddon>();
        List<Bk_transaddon> listBK_TRANSADDONInfo1Old = new List<Bk_transaddon>();
        List<Bk_transaddon> listBK_TRANSADDONInfo2Old = new List<Bk_transaddon>();
        List<Bk_transaddon> listBK_TRANSADDONInfo1tOld = new List<Bk_transaddon>();
        List<Bk_transaddon> listBK_TRANSADDONInfo2tOld = new List<Bk_transaddon>();

        List<BookingTransactionDetail> objListBK_TRANSDTL_Infos = new List<BookingTransactionDetail>();
        List<Bk_transaddon> listSSRPNR = new List<Bk_transaddon>();
        BookingTransactionDetail objBK_TRANSDTL_Infos;

        List<BookingTransactionDetail> lstbookFlightInfo = new List<BookingTransactionDetail>();
        List<BookingTransactionDetail> lstRejectedbookDTLInfo = new List<BookingTransactionDetail>();
        List<BookingTransactionDetail> listDetailCombinePNR = new List<BookingTransactionDetail>();

        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
        ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");

        ABS.GBS.eService.ProcessServiceSoapClient eWS = new ABS.GBS.eService.ProcessServiceSoapClient();
        BookingTransactionDetail bookDTLInfo = new BookingTransactionDetail();
        List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
        BookingTransactionMain bookHDRInfo = new BookingTransactionMain();
        ABS.Logic.GroupBooking.Agent.AgentControl objAgent = new ABS.Logic.GroupBooking.Agent.AgentControl();
        ABS.Logic.GroupBooking.Agent.AgentProfileControl objAgentProfile = new ABS.Logic.GroupBooking.Agent.AgentProfileControl();
        ABS.Logic.GroupBooking.Agent.AgentProfile agent = new ABS.Logic.GroupBooking.Agent.AgentProfile();
        PassengerData PassData2 = new PassengerData();
        List<PassengerData> lstPassInfantData = new List<PassengerData>();
        static bool IsInternationalFlight = false;
        DataTable dtTaxFees = new DataTable();
        DataTable dtList1 = new DataTable();
        DataTable dtList2 = new DataTable();
        DataTable dtList1t = new DataTable();
        DataTable dtList2t = new DataTable();
        private static int qtyMeal, qtyMeal1, qtyDrink, qtyDrink1, qtyBaggage, qtySport, qtyComfort, qtyDuty = 0;
        private static int qtyMeal2, qtyMeal21, qtyDrink2, qtyDrink21, qtyBaggage2, qtySport2, qtyComfort2, qtyDuty2 = 0;
        private static int num = 0;
        private static int departID = 0;
        private static string ReturnID = "";
        private static int first = 0;
        private static int firstinit = 0;
        decimal totalamountduegoing = 0;
        decimal totalamountduereturn = 0;
        decimal totalamountduegoingcommit = 0;
        decimal totalamountduereturncommit = 0;
        decimal totalamountduegoinginfant = 0;
        decimal totalamountduereturninfant = 0;
        decimal totalamountduegoingcommitinfant = 0;
        decimal totalamountduereturncommitinfant = 0;
        String TransID = "";
        int havebalance = 0;
        string depart1 = "", depart2 = "", transit1 = "", transit2 = "", return1 = "", return2 = "";
        private string Curr = "";

        static private DataTable dtPassOld = new DataTable();
        static private DataTable dtPass2Old = new DataTable();
        private static int infantmax = 0;

        static bool IsDepart = false, IsDepartTransit = false, IsDepartTransit2 = false, IsReturn = false, IsReturnTransit = false, IsReturnTransit2 = false;

        string custommessage = "";
        #endregion

        #region "Load"
        protected void Page_Load(object sender, EventArgs e)
        {
            string signature = "";
            DataTable dt = new DataTable();
            var profiler = MiniProfiler.Current;
            try
            {
                TransID = Request.QueryString["TransID"];
                ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking("");
                Session["PaxStatus"] = "";
                if (Session["AgentSet"] != null)
                {
                    MyUserSet = (UserSet)Session["AgentSet"];
                    if (!IsPostBack)
                    {
                        ClearSession();
                        using (profiler.Step("GetSellSSR"))
                        {
                            GetSellSSR(signature, TransID);
                        }
                        BindDefaultDrink();
                    }
                    SetTab();

                    if (Session["dtGridPass"] == null)
                    {
                        GetPassengerList(TransID, "Depart");
                    }
                    using (profiler.Step("GetSSRItem"))
                    {
                        GetSSRItem("Depart");
                    }

                    if (Session["dtDrinkDepart2"] != null)
                    {
                        using (profiler.Step("GetDrink1Depart"))
                        {
                            GetDrink1Depart();
                        }
                    }

                    if (Session["OneWay"] != null)
                    {
                        Boolean OneWay = (Boolean)Session["OneWay"];
                        if (OneWay != true)
                        {

                            // GetPassengerList(TransID, "Return");


                            using (profiler.Step("GetSSRItemReturn"))
                            {
                                GetSSRItem("Return");
                            }
                            if (Session["dtDrinkReturn2"] != null)
                            {
                                using (profiler.Step("GetDrink1Return"))
                                {
                                    GetDrink1Return();
                                }
                            }
                        }
                    }


                    if (first == 0)
                    {
                        using (profiler.Step("BindLabel"))
                        {
                            BindLabel();
                        }
                    }
                }
                else
                {
                    Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.SessionExpired);
                }
                gvPassenger.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                gvPassenger2.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }

        }

        #endregion

        #region "Initializer"

        protected void SetTab()
        {
            GetBookingResponse resp = new GetBookingResponse();
            if (Session["resp"] != null)  resp = (GetBookingResponse)Session["resp"];
            if ((IsDepart == true && IsDepartTransit == false && IsReturn == false && IsReturnTransit == false) || (IsDepart == false && IsDepartTransit == true && IsReturn == false && IsReturnTransit == false))
            {
                InitializeSetting(resp.Booking.Journeys[0].Segments[0].DepartureStation, resp.Booking.Journeys[0].Segments[0].ArrivalStation, "", "");
            }
            else if (IsDepart == true && IsDepartTransit == false && IsReturn == true && IsReturnTransit == false)
            {
                InitializeSetting(resp.Booking.Journeys[0].Segments[0].DepartureStation, resp.Booking.Journeys[0].Segments[0].ArrivalStation, resp.Booking.Journeys[1].Segments[0].DepartureStation, resp.Booking.Journeys[1].Segments[0].ArrivalStation);
            }
            else if (IsDepart == false && IsDepartTransit == true && IsReturn == false && IsReturnTransit == true)
            {
                InitializeSetting(resp.Booking.Journeys[0].Segments[0].DepartureStation, resp.Booking.Journeys[0].Segments[1].ArrivalStation, resp.Booking.Journeys[1].Segments[0].DepartureStation, resp.Booking.Journeys[1].Segments[1].ArrivalStation);
            }
            else if (IsDepart == true && IsDepartTransit == false && IsReturn == false && IsReturnTransit == true)
            {
                InitializeSetting(resp.Booking.Journeys[0].Segments[0].DepartureStation, resp.Booking.Journeys[0].Segments[0].ArrivalStation, resp.Booking.Journeys[1].Segments[0].DepartureStation, resp.Booking.Journeys[1].Segments[1].ArrivalStation);
            }
            else if (IsDepart == false && IsDepartTransit == true && IsReturn == true && IsReturnTransit == false)
            {
                InitializeSetting(resp.Booking.Journeys[0].Segments[0].DepartureStation, resp.Booking.Journeys[0].Segments[1].ArrivalStation, resp.Booking.Journeys[1].Segments[0].DepartureStation, resp.Booking.Journeys[1].Segments[0].ArrivalStation);
            }
        }
        private void ClearSession()
        {

            HttpContext.Current.Session.Remove("qtyMeal");
            HttpContext.Current.Session.Remove("qtyBaggage");
            HttpContext.Current.Session.Remove("qtySport");
            HttpContext.Current.Session.Remove("qtyComfort");
            HttpContext.Current.Session.Remove("qtyDuty");
            HttpContext.Current.Session.Remove("dtMeal");
            HttpContext.Current.Session.Remove("dtBaggage");
            HttpContext.Current.Session.Remove("dtSpor");
            HttpContext.Current.Session.Remove("dtComfort");
            HttpContext.Current.Session.Remove("dtDuty");
            HttpContext.Current.Session.Remove("dtGridPass");
            HttpContext.Current.Session.Remove("dtGridPass2");
            HttpContext.Current.Session.Remove("dtGridOld");
            HttpContext.Current.Session.Remove("dtGrid2Old");
            Session["IsNew"] = "true";
            //Add-on
            HttpContext.Current.Session.Remove("dtBaggageDepart");
            HttpContext.Current.Session.Remove("dtBaggageReturn");
            HttpContext.Current.Session.Remove("dtSportReturn");
            HttpContext.Current.Session.Remove("dtSportReturn");
            HttpContext.Current.Session.Remove("dtMealDepart");
            HttpContext.Current.Session.Remove("dtMealDepart2");
            HttpContext.Current.Session.Remove("dtComfortDepart");
            HttpContext.Current.Session.Remove("dtDutyDepart");
            HttpContext.Current.Session.Remove("dtMealReturn");
            HttpContext.Current.Session.Remove("dtMealReturn2");
            HttpContext.Current.Session.Remove("dtComfortReturn");
            HttpContext.Current.Session.Remove("dtDutyReturn");
            HttpContext.Current.Session.Remove("qtyMeal");
            HttpContext.Current.Session.Remove("qtyBaggage");
            HttpContext.Current.Session.Remove("qtySport");
            HttpContext.Current.Session.Remove("qtyComfort");
            HttpContext.Current.Session.Remove("qtyDuty");
            HttpContext.Current.Session.Remove("qtyMeal2");
            HttpContext.Current.Session.Remove("qtyBaggage2");
            HttpContext.Current.Session.Remove("qtySport2");
            HttpContext.Current.Session.Remove("qtyComfort2");
            HttpContext.Current.Session.Remove("qtyDuty2");

            HttpContext.Current.Session.Remove("TotalBaggage");
            HttpContext.Current.Session.Remove("TotalBaggage2");
            HttpContext.Current.Session.Remove("TotalMeal");
            HttpContext.Current.Session.Remove("TotalMeal1");
            HttpContext.Current.Session.Remove("TotalMeal2");
            HttpContext.Current.Session.Remove("TotalMeal12");
            HttpContext.Current.Session.Remove("TotalSport");
            HttpContext.Current.Session.Remove("TotalSport2");
            HttpContext.Current.Session.Remove("TotalComfort");
            HttpContext.Current.Session.Remove("TotalComfort2");
            HttpContext.Current.Session.Remove("TotalInfant");
            HttpContext.Current.Session.Remove("TotalInfant2");
            HttpContext.Current.Session.Remove("transit");
            HttpContext.Current.Session.Remove("departTransit");
            HttpContext.Current.Session.Remove("transitdepart");
            HttpContext.Current.Session.Remove("returnTransit");
            HttpContext.Current.Session.Remove("transitreturn");
            HttpContext.Current.Session.Remove("lstPassInfantData");
            HttpContext.Current.Session.Remove("Type");
            HttpContext.Current.Session.Remove("resp");

            IsDepart = false;
            IsDepartTransit = false;
            IsDepartTransit2 = false;
            IsReturn = false;
            IsReturnTransit = false;
            IsReturnTransit2 = false;  
            //txt_GeustNum.Text = "10";
        }

        protected void GetBaggageDepart()
        {
            GridViewDataComboBoxColumn column = (gvPassenger.Columns["DepartBaggage"] as GridViewDataComboBoxColumn);
            DataTable dtBaggage = null;
            dtBaggage = (DataTable)Session["dtBaggageDepart"];
            DataRow[] result = dtBaggage.Select("SSRCode = ''");
            //if (result.Length == 0)
            //{
            //    if (first == 1 && first == 3)
            //    {
            //        dtBaggage.Rows.Add("", "", "");
            //    }
            //}
            column.PropertiesComboBox.DataSource = dtBaggage;

            column.PropertiesComboBox.ValueField = "SSRCode";
            column.PropertiesComboBox.TextField = "ConcatenatedField";

            gvPassenger.DataBind();
        }

        protected void GetSportDepart()
        {
            GridViewDataComboBoxColumn column = (gvPassenger.Columns["DepartSport"] as GridViewDataComboBoxColumn);
            DataTable dtBaggage = null;
            dtBaggage = (DataTable)Session["dtSportDepart"];
            DataRow[] result = dtBaggage.Select("SSRCode = ''");
            //if (result.Length == 0)
            //{
            //    dtBaggage.Rows.Add("", "", "");
            //}
            column.PropertiesComboBox.DataSource = dtBaggage;

            column.PropertiesComboBox.ValueField = "SSRCode";
            column.PropertiesComboBox.TextField = "ConcatenatedField";

            gvPassenger.DataBind();
        }

        protected void GetDrinkDepart()
        {
            GridViewDataComboBoxColumn column = (gvPassenger.Columns["DepartDrink"] as GridViewDataComboBoxColumn);
            DataTable dtBaggage = null;
            dtBaggage = (DataTable)Session["dtDrinkDepart"];
            DataRow[] result = dtBaggage.Select("SSRCode = ''");
            //if (result.Length == 0)
            //{
            //    dtBaggage.Rows.Add("", "", "");
            //}
            column.PropertiesComboBox.DataSource = dtBaggage;

            column.PropertiesComboBox.ValueField = "SSRCode";
            column.PropertiesComboBox.TextField = "ConcatenatedField";

            gvPassenger.DataBind();
        }

        protected void GetDrink1Depart()
        {
            GridViewDataComboBoxColumn column = (gvPassenger.Columns["ConDepartDrink"] as GridViewDataComboBoxColumn);
            DataTable dtBaggage = null;
            dtBaggage = (DataTable)Session["dtDrinkDepart2"];
            DataRow[] result = dtBaggage.Select("SSRCode = ''");
            //if (result.Length == 0)
            //{
            //    dtBaggage.Rows.Add("", "", "");
            //}
            column.PropertiesComboBox.DataSource = dtBaggage;

            column.PropertiesComboBox.ValueField = "SSRCode";
            column.PropertiesComboBox.TextField = "ConcatenatedField";

            gvPassenger.DataBind();
        }
        protected void GetBaggageReturn()
        {
            GridViewDataComboBoxColumn column = (gvPassenger2.Columns["ReturnBaggage"] as GridViewDataComboBoxColumn);
            DataTable dtBaggage = null;
            dtBaggage = (DataTable)Session["dtBaggageReturn"];
            DataRow[] result = dtBaggage.Select("SSRCode = ''");
            //if (result.Length == 0)
            //{
            //    if (first == 1 && first == 3)
            //    {
            //        dtBaggage.Rows.Add("", "", "");
            //    }
            //}
            column.PropertiesComboBox.DataSource = dtBaggage;

            column.PropertiesComboBox.ValueField = "SSRCode";
            column.PropertiesComboBox.TextField = "ConcatenatedField";

            gvPassenger2.DataBind();
        }

        protected void GetSportReturn()
        {
            GridViewDataComboBoxColumn column = (gvPassenger2.Columns["ReturnSport"] as GridViewDataComboBoxColumn);
            DataTable dtBaggage = null;
            dtBaggage = (DataTable)Session["dtSportReturn"];
            DataRow[] result = dtBaggage.Select("SSRCode = ''");
            //if (result.Length == 0)
            //{
            //    dtBaggage.Rows.Add("", "", "");
            //}
            column.PropertiesComboBox.DataSource = dtBaggage;

            column.PropertiesComboBox.ValueField = "SSRCode";
            column.PropertiesComboBox.TextField = "ConcatenatedField";

            gvPassenger2.DataBind();
        }

        protected void GetDrinkReturn()
        {
            GridViewDataComboBoxColumn column = (gvPassenger2.Columns["ReturnDrink"] as GridViewDataComboBoxColumn);
            DataTable dtBaggage = null;
            dtBaggage = (DataTable)Session["dtDrinkReturn"];
            DataRow[] result = dtBaggage.Select("SSRCode = ''");
            //if (result.Length == 0)
            //{
            //    dtBaggage.Rows.Add("", "", "");
            //}
            column.PropertiesComboBox.DataSource = dtBaggage;

            column.PropertiesComboBox.ValueField = "SSRCode";
            column.PropertiesComboBox.TextField = "ConcatenatedField";

            gvPassenger2.DataBind();
        }

        protected void GetDrink1Return()
        {
            GridViewDataComboBoxColumn column = (gvPassenger2.Columns["ConReturnDrink"] as GridViewDataComboBoxColumn);
            DataTable dtBaggage = null;
            dtBaggage = (DataTable)Session["dtDrinkReturn2"];
            DataRow[] result = dtBaggage.Select("SSRCode = ''");
            //if (result.Length == 0)
            //{
            //    dtBaggage.Rows.Add("", "", "");
            //}
            column.PropertiesComboBox.DataSource = dtBaggage;

            column.PropertiesComboBox.ValueField = "SSRCode";
            column.PropertiesComboBox.TextField = "ConcatenatedField";

            gvPassenger2.DataBind();
        }
        protected void glMeals_AutoFilterCellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            MessageList msgList = new MessageList();
            (e.Editor as ASPxTextBox).NullText = msgList.Err200102;
        }

        protected void glMeals1_AutoFilterCellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            MessageList msgList = new MessageList();
            (e.Editor as ASPxTextBox).NullText = msgList.Err200102;
        }

        protected void glMeals2_AutoFilterCellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            MessageList msgList = new MessageList();
            (e.Editor as ASPxTextBox).NullText = msgList.Err200102;
        }

        protected void glMeals22_AutoFilterCellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            MessageList msgList = new MessageList();
            (e.Editor as ASPxTextBox).NullText = msgList.Err200102;
        }

        protected void glMealP1_Init(object sender, EventArgs e)
        {
            try
            {
                if (Session["dtMealDepart"] != null)
                {
                    ASPxGridLookup grdlkMealDepart = sender as ASPxGridLookup;

                    grdlkMealDepart.DataSource = RetrieveItems((DataTable)Session["dtMealDepart"]);
                    grdlkMealDepart.DataBind();
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void glMealP11_Init(object sender, EventArgs e)
        {
            try
            {
                if (Session["dtMealDepart2"] != null)
                {
                    ASPxGridLookup grdlkMealDepart = sender as ASPxGridLookup;

                    grdlkMealDepart.DataSource = RetrieveItems((DataTable)Session["dtMealDepart2"]);
                    grdlkMealDepart.DataBind();
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void glComfortP1_Init(object sender, EventArgs e)
        {
            try
            {
                if (Session["dtComfortDepart"] != null)
                {
                    ASPxGridLookup grdlkMealDepart = sender as ASPxGridLookup;

                    grdlkMealDepart.DataSource = RetrieveItems((DataTable)Session["dtComfortDepart"]);
                    grdlkMealDepart.DataBind();
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void glDutyFreeP1_Init(object sender, EventArgs e)
        {
            try
            {
                if (Session["dtDutyDepart"] != null)
                {
                    ASPxGridLookup grdlkMealDepart = sender as ASPxGridLookup;

                    grdlkMealDepart.DataSource = RetrieveItems((DataTable)Session["dtDutyDepart"]);
                    grdlkMealDepart.DataBind();
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void glMealP21_Init(object sender, EventArgs e)
        {
            try
            {
                if (Session["dtMealReturn"] != null)
                {
                    ASPxGridLookup grdlkMealDepart = sender as ASPxGridLookup;

                    grdlkMealDepart.DataSource = RetrieveItems((DataTable)Session["dtMealReturn"]);
                    grdlkMealDepart.DataBind();
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void glMealP22_Init(object sender, EventArgs e)
        {
            try
            {
                if (Session["dtMealReturn2"] != null)
                {
                    ASPxGridLookup grdlkMealDepart = sender as ASPxGridLookup;

                    grdlkMealDepart.DataSource = RetrieveItems((DataTable)Session["dtMealReturn2"]);
                    grdlkMealDepart.DataBind();
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void glComfortP2_Init(object sender, EventArgs e)
        {
            try
            {
                if (Session["dtComfortReturn"] != null)
                {
                    ASPxGridLookup grdlkMealDepart = sender as ASPxGridLookup;

                    grdlkMealDepart.DataSource = RetrieveItems((DataTable)Session["dtComfortReturn"]);
                    grdlkMealDepart.DataBind();
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void glDutyFreeP2_Init(object sender, EventArgs e)
        {
            try
            {
                if (Session["dtDutyReturn"] != null)
                {
                    ASPxGridLookup grdlkMealDepart = sender as ASPxGridLookup;

                    grdlkMealDepart.DataSource = RetrieveItems((DataTable)Session["dtDutyReturn"]);
                    grdlkMealDepart.DataBind();
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected DataTable RetrieveItems(DataTable dtSSR)
        {
            DataRow[] result = dtSSR.Select("SSRCode = ''");
            if (result.Length == 0)
            {
                dtSSR.Rows.Add("", "", "", "");
            }
            DataView dvSSR = dtSSR.DefaultView;

            return dvSSR.ToTable(true, "SSRCode", "Detail", "Price", "Images");
        }

        protected void SetMaxValue()
        {
            try
            {
                HttpCookie cookie2 = Request.Cookies["cookieSearchcondition"];
                if (cookie2 != null)
                {
                    if (Session["qtyMeal"] == null)
                    {
                        seMeals.MaxValue = Convert.ToInt32(cookie2.Values["PaxNum"]);
                    }
                    else
                    {
                        seMeals.MaxValue = Convert.ToInt32(cookie2.Values["PaxNum"]) - Convert.ToInt16(Session["qtyMeal"]);
                    }
                    if (Session["qtyBaggage"] == null)
                    {
                        seBaggage.MaxValue = Convert.ToInt32(cookie2.Values["PaxNum"]);
                    }
                    else
                    {
                        seBaggage.MaxValue = Convert.ToInt32(cookie2.Values["PaxNum"]) - Convert.ToInt16(Session["qtyBaggage"]);
                    }
                    if (Session["qtySport"] == null)
                    {
                        seSport.MaxValue = Convert.ToInt32(cookie2.Values["PaxNum"]);
                    }
                    else
                    {
                        seSport.MaxValue = Convert.ToInt32(cookie2.Values["PaxNum"]) - Convert.ToInt16(Session["qtySport"]);
                    }
                    if (Session["qtyComfort"] == null)
                    {
                        seComfort.MaxValue = Convert.ToInt32(cookie2.Values["PaxNum"]);
                    }
                    else
                    {
                        seComfort.MaxValue = Convert.ToInt32(cookie2.Values["PaxNum"]) - Convert.ToInt16(Session["qtyComfort"]);
                    }
                    if (Session["qtyDuty"] == null)
                    {
                        seComfort.MaxValue = Convert.ToInt32(cookie2.Values["PaxNum"]);
                    }
                    else
                    {
                        seDuty.MaxValue = Convert.ToInt32(cookie2.Values["PaxNum"]) - Convert.ToInt16(Session["qtyDuty"]);
                    }

                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void SetMaxValue2()
        {
            try
            {
                HttpCookie cookie2 = Request.Cookies["cookieSearchcondition"];
                if (cookie2 != null)
                {
                    if (Session["qtyMeal2"] == null)
                    {
                        seMeals2.MaxValue = Convert.ToInt32(cookie2.Values["PaxNum"]);
                    }
                    else
                    {
                        seMeals2.MaxValue = Convert.ToInt32(cookie2.Values["PaxNum"]) - Convert.ToInt16(Session["qtyMeal2"]);
                    }
                    if (Session["qtyBaggage2"] == null)
                    {
                        seBaggage2.MaxValue = Convert.ToInt32(cookie2.Values["PaxNum"]);
                    }
                    else
                    {
                        seBaggage2.MaxValue = Convert.ToInt32(cookie2.Values["PaxNum"]) - Convert.ToInt16(Session["qtyBaggage2"]);
                    }
                    if (Session["qtySport2"] == null)
                    {
                        seSport2.MaxValue = Convert.ToInt32(cookie2.Values["PaxNum"]);
                    }
                    else
                    {
                        seSport2.MaxValue = Convert.ToInt32(cookie2.Values["PaxNum"]) - Convert.ToInt16(Session["qtySport2"]);
                    }
                    if (Session["qtyComfort2"] == null)
                    {
                        seComfort2.MaxValue = Convert.ToInt32(cookie2.Values["PaxNum"]);
                    }
                    else
                    {
                        seComfort2.MaxValue = Convert.ToInt32(cookie2.Values["PaxNum"]) - Convert.ToInt16(Session["qtyComfort2"]);
                    }
                    if (Session["qtyDuty2"] == null)
                    {
                        seComfort2.MaxValue = Convert.ToInt32(cookie2.Values["PaxNum"]);
                    }
                    else
                    {
                        seDuty2.MaxValue = Convert.ToInt32(cookie2.Values["PaxNum"]) - Convert.ToInt16(Session["qtyDuty2"]);
                    }

                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }
        protected void InitializeSetting(string Departure1, string Arrival1, string Departure2, string Arrival2)
        {
            try
            {
                string temp = "";
                if (Departure2 != "" && Arrival2 != "")
                {

                    temp = Departure1 + " | " + Arrival1;
                    TabControl.TabPages[0].Text = temp;

                    temp = Departure2 + " | " + Arrival2;
                    TabControl.TabPages[1].Text = temp;
                }
                else
                {
                    temp = Departure1 + " | " + Arrival1;
                    TabControl.TabPages[0].Text = temp;
                    TabControl.TabPages[1].Visible = false;
                }

                //BindModel();
                //Clearsession();
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }
        protected void InitializeForm(String Currency, ArrayList BaggageCode, ArrayList BaggageFee, ArrayList BaggageFeeS1, ArrayList BaggageFeeS2, ArrayList SportCode, ArrayList SportFee, ArrayList SportFeeS1, ArrayList SportFeeS2, ArrayList ComfortCode, ArrayList ConfortFee, ArrayList ConfortFeeS1, ArrayList ConfortFeeS2, ArrayList ComfortImg, ArrayList MealCode, ArrayList MealFee, ArrayList MealImg, ArrayList MealCode1, ArrayList MealFee1, ArrayList MealImg1, ArrayList DutyCode, ArrayList DutyFee, ArrayList DutyImg, ArrayList DrinkCode, ArrayList DrinkFee, ArrayList DrinkCode1, ArrayList DrinkFee1, ArrayList InfantCode, ArrayList InfantFee, ArrayList InfantFeeS1, ArrayList InfantFeeS2, string Flight)
        {
            MessageList msgList = new MessageList();
            HttpCookie cookie = Request.Cookies["cookieSearchcondition"];
            var profiler = MiniProfiler.Current;
            try
            {
                using (profiler.Step("InitializeForm"))
                {
                    DataTable dtBaggage = new DataTable();
                    dtBaggage.Columns.Add("SSRCode");
                    dtBaggage.Columns.Add("Detail");
                    dtBaggage.Columns.Add("Price");
                    dtBaggage.Columns.Add("PriceS1");
                    dtBaggage.Columns.Add("PriceS2");
                    dtBaggage.Columns.Add("ConcatenatedField", typeof(string), "Detail + '  ' +Price");

                    DataRow row = dtBaggage.NewRow();
                    DataTable dt = (DataTable)Application["dtArrayBaggage"];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int b = BaggageCode.IndexOf(dt.Rows[i]["ItemCode"].ToString());
                            if (b >= 0)
                            {
                                dtBaggage.Rows.Add(BaggageCode[b].ToString(), dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(BaggageFeeS1[b]) + Convert.ToDecimal((b < BaggageFeeS2.Count) ? BaggageFeeS2[b].ToString() : "0.00")).ToString("N", nfi) + " " + Currency, Convert.ToDecimal(BaggageFeeS1[b]), Convert.ToDecimal((b < BaggageFeeS2.Count) ? BaggageFeeS2[b].ToString() : "0.00"));
                            }
                        }
                    }

                    DataView dv = dtBaggage.DefaultView;
                    if (Flight == "Depart")
                    {
                        cmbBaggage.DataSource = dv.ToTable(true, "SSRCode", "Detail", "Price", "PriceS1", "PriceS2", "ConcatenatedField");
                        cmbBaggage.TextField = "ConcatenatedField";
                        cmbBaggage.ValueField = "SSRCode";
                        cmbBaggage.DataBind();
                        cmbBaggage.NullText = msgList.Err200101;
                    }
                    else
                    {
                        cmbBaggage2.DataSource = dv.ToTable(true, "SSRCode", "Detail", "Price", "PriceS1", "PriceS2", "ConcatenatedField");
                        cmbBaggage2.TextField = "ConcatenatedField";
                        cmbBaggage2.ValueField = "SSRCode";
                        cmbBaggage2.DataBind();
                        cmbBaggage2.NullText = msgList.Err200101;
                    }
                    Session["dtBaggage" + Flight] = dtBaggage;


                    DataTable dtMeal = new DataTable();
                    dtMeal.Columns.Add("SSRCode");
                    dtMeal.Columns.Add("Detail");
                    dtMeal.Columns.Add("Price");
                    dtMeal.Columns.Add("Images");

                    DataRow rowMeal = dtMeal.NewRow();

                    if (MealCode.Count > 0 && DrinkCode.Count >= 1)
                    {
                        dt = (DataTable)Application["dtArrayMeal"];
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                int b = MealCode.IndexOf(dt.Rows[i]["ItemCode"].ToString());
                                if (b >= 0)
                                {
                                    dtMeal.Rows.Add(MealCode[b].ToString(), dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(MealFee[b])).ToString("N", nfi) + " " + Currency, MealImg[b].ToString());
                                }
                            }
                        }
                    }

                    DataView dvMeal = dtMeal.DefaultView;
                    if (Flight == "Depart")
                    {
                        glMeals.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                        glMeals.DataBind();
                        if (dtMeal.Rows.Count > 0)
                        {
                            glMeals.NullText = msgList.Err200102;
                        }
                        else
                        {
                            glMeals.NullText = msgList.Err200103;
                        }
                    }
                    else
                    {
                        glMeals2.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                        glMeals2.DataBind();
                        if (dtMeal.Rows.Count > 0)
                        {
                            glMeals2.NullText = msgList.Err200102;
                        }
                        else
                        {
                            glMeals2.NullText = msgList.Err200103;
                        }
                    }
                    Session["dtMeal" + Flight] = dtMeal;

                    if (MealCode1 != null)
                    {
                        DataTable dtMeal1 = new DataTable();
                        dtMeal1.Columns.Add("SSRCode");
                        dtMeal1.Columns.Add("Detail");
                        dtMeal1.Columns.Add("Price");
                        dtMeal1.Columns.Add("Images");

                        DataRow rowMeal1 = dtMeal1.NewRow();
                        if (MealCode1.Count > 0 && DrinkCode1.Count >= 1)
                        {
                            dt = (DataTable)Application["dtArrayMeal"];
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    int b = MealCode1.IndexOf(dt.Rows[i]["ItemCode"].ToString());
                                    if (b >= 0)
                                    {
                                        dtMeal1.Rows.Add(MealCode1[b].ToString(), dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(MealFee1[b])).ToString("N", nfi) + " " + Currency, MealImg1[b].ToString());
                                    }
                                }
                            }
                        }

                        DataView dvMeal1 = dtMeal1.DefaultView;
                        if (Flight == "Depart")
                        {
                            glMeals1.DataSource = dvMeal1.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                            glMeals1.DataBind();
                            if (dtMeal1.Rows.Count > 0)
                            {
                                glMeals1.NullText = msgList.Err200102;
                            }
                            else
                            {
                                glMeals1.NullText = msgList.Err200103;
                            }
                            //gvPassenger.Columns["DepartMeal"].Caption = "Meal 1";
                        }
                        else
                        {
                            glMeals22.DataSource = dvMeal1.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                            glMeals22.DataBind();
                            if (dtMeal1.Rows.Count > 0)
                            {
                                glMeals22.NullText = msgList.Err200102;
                            }
                            else
                            {
                                glMeals22.NullText = msgList.Err200103;
                            }
                            //gvPassenger2.Columns["ReturnMeal"].Caption = "Meal 1";
                        }
                        Session["dtMeal" + Flight + "2"] = dtMeal1;

                    }
                    else
                    {
                        if (Flight == "Depart")
                        {
                            divmeal1.Style.Add("display", "none");
                            gvPassenger.Columns["ConDepartMeal"].Visible = false;
                        }
                        else
                        {
                            divmeal2.Style.Add("display", "none");
                            gvPassenger2.Columns["ConReturnMeal"].Visible = false;
                        }

                    }

                    if (DrinkCode.Count >= 1)
                    {
                        //----------------------
                        DataTable dtDrink = new DataTable();
                        dtDrink.Columns.Add("SSRCode");
                        dtDrink.Columns.Add("Detail");
                        dtDrink.Columns.Add("Price");
                        dtDrink.Columns.Add("ConcatenatedField", typeof(string), "Detail + '  ' +Price");

                        DataRow rowDrink = dtDrink.NewRow();
                        dt = (DataTable)Application["dtArrayDrink"];
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                int b = DrinkCode.IndexOf(dt.Rows[i]["ItemCode"].ToString());
                                if (b >= 0)
                                {
                                    dtDrink.Rows.Add(DrinkCode[b].ToString(), dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(DrinkFee[b])).ToString("N", nfi) + " " + Currency);
                                }
                            }
                        }

                        DataView dvDrink = dtDrink.DefaultView;
                        if (Flight == "Depart")
                        {
                            cmbDrinks.DataSource = dv.ToTable(true, "SSRCode", "Detail", "Price", "ConcatenatedField");
                            cmbDrinks.DataSource = dtDrink.DefaultView;
                            cmbDrinks.TextField = "ConcatenatedField";
                            cmbDrinks.ValueField = "SSRCode";
                            cmbDrinks.DataBind();
                            cmbDrinks.NullText = msgList.Err200104;
                        }
                        else
                        {
                            cmbDrinks2.DataSource = dv.ToTable(true, "SSRCode", "Detail", "Price", "ConcatenatedField");
                            cmbDrinks2.DataSource = dtDrink.DefaultView;
                            cmbDrinks2.TextField = "ConcatenatedField";
                            cmbDrinks2.ValueField = "SSRCode";
                            cmbDrinks2.DataBind();
                            cmbDrinks2.NullText = msgList.Err200104;
                        }
                        Session["dtDrink" + Flight] = dtDrink;
                    }
                    else
                    {
                        if (Flight == "Depart")
                        {
                            tdDrinks.Style.Add("display", "none");
                            gvPassenger.Columns["DepartDrink"].Visible = false;
                        }
                        else
                        {
                            tdDrinks2.Style.Add("display", "none");
                            gvPassenger2.Columns["ReturnDrink"].Visible = false;
                        }
                    }

                    if (DrinkCode1 != null && DrinkCode1.Count >= 1)
                    {
                        DataTable dtDrink1 = new DataTable();
                        dtDrink1.Columns.Add("SSRCode");
                        dtDrink1.Columns.Add("Detail");
                        dtDrink1.Columns.Add("Price");
                        dtDrink1.Columns.Add("ConcatenatedField", typeof(string), "Detail + '  ' +Price");

                        DataRow rowDrink1 = dtDrink1.NewRow();
                        dt = (DataTable)Application["dtArrayDrink"];
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                int b = DrinkCode1.IndexOf(dt.Rows[i]["ItemCode"].ToString());
                                if (b >= 0)
                                {
                                    dtDrink1.Rows.Add(DrinkCode1[b].ToString(), dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(DrinkFee1[b])).ToString("N", nfi) + " " + Currency);
                                }
                            }
                        }

                        DataView dvDrink1 = dtDrink1.DefaultView;
                        if (Flight == "Depart")
                        {
                            cmbDrinks1.DataSource = dvDrink1.ToTable(true, "SSRCode", "Detail", "Price", "ConcatenatedField");
                            cmbDrinks1.DataSource = dtDrink1.DefaultView;
                            cmbDrinks1.TextField = "ConcatenatedField";
                            cmbDrinks1.ValueField = "SSRCode";
                            cmbDrinks1.DataBind();
                            cmbDrinks1.NullText = msgList.Err200104;
                            //gvPassenger.Columns["DepartDrink"].Caption = "Drink 1";
                        }
                        else
                        {
                            cmbDrinks22.DataSource = dvDrink1.ToTable(true, "SSRCode", "Detail", "Price", "ConcatenatedField");
                            cmbDrinks22.DataSource = dtDrink1.DefaultView;
                            cmbDrinks22.TextField = "ConcatenatedField";
                            cmbDrinks22.ValueField = "SSRCode";
                            cmbDrinks22.DataBind();
                            cmbDrinks22.NullText = msgList.Err200104;
                            //gvPassenger2.Columns["ConReturnDrink"].Caption = "Drink 1";
                        }
                        Session["dtDrink" + Flight + "2"] = dtDrink1;

                    }
                    else
                    {
                        if (Flight == "Depart")
                        {
                            tdDrinks1.Style.Add("display", "none");
                            gvPassenger.Columns["ConDepartDrink"].Visible = false;
                        }
                        else
                        {
                            tdDrinks22.Style.Add("display", "none");
                            gvPassenger2.Columns["ConReturnDrink"].Visible = false;
                        }

                    }

                    DataTable dtSport = new DataTable();
                    dtSport.Columns.Add("SSRCode");
                    dtSport.Columns.Add("Detail");
                    dtSport.Columns.Add("Price");
                    dtSport.Columns.Add("PriceS1");
                    dtSport.Columns.Add("PriceS2");
                    dtSport.Columns.Add("ConcatenatedField", typeof(string), "Detail + '  ' +Price");

                    DataRow row1 = dtSport.NewRow();
                    dt = (DataTable)Application["dtArraySport"];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int b = SportCode.IndexOf(dt.Rows[i]["ItemCode"].ToString());
                            if (b >= 0)
                            {
                                dtSport.Rows.Add(SportCode[b].ToString(), dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(SportFeeS1[b]) + Convert.ToDecimal((b < SportFeeS2.Count) ? SportFeeS2[b].ToString() : "0.00")).ToString("N", nfi) + " " + Currency, Convert.ToDecimal(SportFeeS1[b]), Convert.ToDecimal((b < SportFeeS2.Count) ? SportFeeS2[b].ToString() : "0.00"));
                            }
                        }
                    }

                    DataView dvSport = dtSport.DefaultView;
                    if (Flight == "Depart")
                    {
                        cmbSport.DataSource = dvSport.ToTable(true, "SSRCode", "Detail", "Price", "PriceS1", "PriceS2", "ConcatenatedField");
                        cmbSport.TextField = "ConcatenatedField";
                        cmbSport.ValueField = "SSRCode";
                        cmbSport.DataBind();
                        cmbSport.NullText = msgList.Err200105;
                    }
                    else
                    {
                        cmbSport2.DataSource = dvSport.ToTable(true, "SSRCode", "Detail", "Price", "PriceS1", "PriceS2", "ConcatenatedField");
                        cmbSport2.TextField = "ConcatenatedField";
                        cmbSport2.ValueField = "SSRCode";
                        cmbSport2.DataBind();
                        cmbSport2.NullText = msgList.Err200105;
                    }
                    Session["dtSport" + Flight] = dtSport;

                    if (InfantCode.Count > 0)
                    {

                        DataTable dtInfant = new DataTable();
                        dtInfant.Columns.Add("SSRCode");
                        dtInfant.Columns.Add("Detail");
                        dtInfant.Columns.Add("Price");
                        dtInfant.Columns.Add("PriceS1");
                        dtInfant.Columns.Add("PriceS2");
                        dtInfant.Columns.Add("ConcatenatedField", typeof(string), "Detail + '  ' +Price");

                        DataRow rowInfant = dtInfant.NewRow();
                        dt = (DataTable)Application["dtArrayInfant"];
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                int b = InfantCode.IndexOf(dt.Rows[i]["ItemCode"].ToString());
                                if (b >= 0)
                                {
                                    dtInfant.Rows.Add(InfantCode[b].ToString(), dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(InfantFeeS1[b]) + Convert.ToDecimal((b < InfantFeeS2.Count) ? InfantFeeS2[b].ToString() : "0.00")).ToString("N", nfi) + " " + Currency, Convert.ToDecimal(InfantFeeS1[b]), Convert.ToDecimal((b < InfantFeeS2.Count) ? InfantFeeS2[b].ToString() : "0.00"));
                                }
                            }
                        }

                        DataView dvInfant = dtInfant.DefaultView;
                        Session["dtInfant" + Flight] = dtInfant;
                    }
                    else
                    {
                        if (Flight == "Depart")
                        {
                            gvPassenger.Columns["DepartInfant"].Visible = false;
                            InfantIcon1.Style.Add("display", "none");
                        }
                        else
                        {
                            gvPassenger2.Columns["ReturnInfant"].Visible = false;
                            InfantIcon2.Style.Add("display", "none");
                        }
                    }

                    DataTable dtDuty = new DataTable();
                    dtDuty.Columns.Add("SSRCode");
                    dtDuty.Columns.Add("Detail");
                    dtDuty.Columns.Add("Price");
                    dtDuty.Columns.Add("Images");

                    DataRow rowDuty = dtBaggage.NewRow();
                    dt = (DataTable)Application["dtArrayDuty"];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int b = DutyCode.IndexOf(dt.Rows[i]["ItemCode"].ToString());
                            if (b >= 0)
                            {
                                dtDuty.Rows.Add(DutyCode[b], dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(DutyFee[b])).ToString("N", nfi) + " " + Currency, DutyImg[b].ToString());
                            }
                        }
                    }


                    DataView dvDuty = dtDuty.DefaultView;
                    if (Flight == "Depart")
                    {
                        glDuty.DataSource = dvDuty.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                        glDuty.DataBind();
                        glDuty.NullText = msgList.Err200106;
                    }
                    else
                    {
                        glDuty2.DataSource = dvDuty.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                        glDuty2.DataBind();
                        glDuty2.NullText = msgList.Err200106;
                    }
                    Session["dtDuty" + Flight] = dtDuty;

                    DataTable dtComfort = new DataTable();
                    dtComfort.Columns.Add("SSRCode");
                    dtComfort.Columns.Add("Detail");
                    dtComfort.Columns.Add("Price");
                    dtComfort.Columns.Add("PriceS1");
                    dtComfort.Columns.Add("PriceS2");
                    dtComfort.Columns.Add("Images");

                    DataRow row2 = dtComfort.NewRow();
                    dt = (DataTable)Application["dtArrayKit"];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int b = ComfortCode.IndexOf(dt.Rows[i]["ItemCode"].ToString());
                            if (b >= 0)
                            {
                                dtComfort.Rows.Add(ComfortCode[b], dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(ConfortFeeS1[b]) + Convert.ToDecimal((b < ConfortFeeS2.Count) ? ConfortFeeS2[b].ToString() : "0.00")).ToString("N", nfi) + " " + Currency, Convert.ToDecimal(ConfortFeeS1[b]), Convert.ToDecimal((b < ConfortFeeS2.Count) ? ConfortFeeS2[b].ToString() : "0.00"), ComfortImg[b].ToString());
                            }
                        }
                    }

                    DataView dvComfort = dtComfort.DefaultView;
                    if (Flight == "Depart")
                    {
                        glComfort.DataSource = dvComfort.ToTable(true, "SSRCode", "Detail", "Price", "PriceS1", "PriceS2", "Images");
                        glComfort.DataBind();
                        glComfort.NullText = msgList.Err200107;
                    }
                    else
                    {
                        glComfort2.DataSource = dvComfort.ToTable(true, "SSRCode", "Detail", "Price", "PriceS1", "PriceS2", "Images");
                        glComfort2.DataBind();
                        glComfort2.NullText = msgList.Err200107;
                    }
                    Session["dtComfort" + Flight] = dtComfort;
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
        }

        protected void InitializeForm2(String Currency, ArrayList BaggageCode, ArrayList BaggageFee, ArrayList BaggageFeeS1, ArrayList BaggageFeeS2, ArrayList SportCode, ArrayList SportFee, ArrayList SportFeeS1, ArrayList SportFeeS2, ArrayList ComfortCode, ArrayList ConfortFee, ArrayList ConfortFeeS1, ArrayList ConfortFeeS2, ArrayList ComfortImg, ArrayList MealCode, ArrayList MealFee, ArrayList MealImg, ArrayList MealCode2, ArrayList MealFee2, ArrayList MealImg2, ArrayList DutyCode, ArrayList DutyFee, ArrayList DutyImg, ArrayList DrinkCode, ArrayList DrinkFee, ArrayList DrinkCode2, ArrayList DrinkFee2)
        {
            MessageList msgList = new MessageList();
            try
            {
                DataTable dtBaggage = new DataTable();
                dtBaggage.Columns.Add("SSRCode");
                dtBaggage.Columns.Add("Detail");
                dtBaggage.Columns.Add("Price");
                dtBaggage.Columns.Add("PriceS1");
                dtBaggage.Columns.Add("PriceS2");
                dtBaggage.Columns.Add("ConcatenatedField", typeof(string), "Detail + '  ' +Price");

                //Enumerable.Range(1, 10).ToList().ForEach(i => dtBaggage.Rows.Add(i, string.Concat("Detail", i), string.Concat("Price", i)));
                DataRow row = dtBaggage.NewRow();

                String Detail = "";
                foreach (string item in BaggageCode)
                {
                    Detail += "'" + item + "',";
                }
                Detail = Detail.Substring(0, Detail.Length - 1);

                objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                DataTable dt = objBooking.GetDetailSSRbyCode(Detail, "PBA");
                if (dt != null && dt.Rows.Count > 0)
                {
                    //dtBaggage.Rows.Add("", "", "");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int b = BaggageCode.IndexOf(dt.Rows[i]["ItemCode"].ToString());
                        if (b >= 0)
                        {
                            dtBaggage.Rows.Add(BaggageCode[b].ToString(), dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(BaggageFee[b])).ToString("N", nfi) + " " + Currency, Convert.ToDecimal(BaggageFeeS1[b]), Convert.ToDecimal((b < BaggageFeeS2.Count) ? BaggageFeeS2[b].ToString() : "0.00"));
                        }

                    }
                }

                DataView dv = dtBaggage.DefaultView;
                cmbBaggage2.DataSource = dv.ToTable(true, "SSRCode", "Detail", "Price", "PriceS1", "PriceS2", "ConcatenatedField");
                cmbBaggage2.TextField = "ConcatenatedField";
                cmbBaggage2.ValueField = "SSRCode";
                cmbBaggage2.DataBind();
                cmbBaggage2.NullText = msgList.Err200101;
                Session["dtBaggageReturn"] = dtBaggage;

                DataTable dtMeal = new DataTable();
                dtMeal.Columns.Add("SSRCode");
                dtMeal.Columns.Add("Detail");
                dtMeal.Columns.Add("Price");
                dtMeal.Columns.Add("Images");

                DataRow rowMeal = dtMeal.NewRow();

                Detail = "";
                if (MealCode.Count > 0 && DrinkCode.Count >= 1)
                {
                    foreach (string item in MealCode)
                    {
                        Detail += "'" + item + "',";
                    }
                    Detail = Detail.Substring(0, Detail.Length - 1);

                    objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                    dt = objBooking.GetDetailSSRbyCode(Detail, "WYM");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //dtMeal.Rows.Add("", "", "", "");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int b = MealCode.IndexOf(dt.Rows[i]["ItemCode"].ToString());
                            if (b >= 0)
                            {
                                dtMeal.Rows.Add(MealCode[b].ToString(), dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(MealFee[b])).ToString("N", nfi) + " " + Currency, MealImg[b].ToString());
                            }
                        }
                    }
                }

                DataView dvMeal = dtMeal.DefaultView;
                glMeals2.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                glMeals2.DataBind();
                if (dtMeal.Rows.Count > 0)
                {
                    glMeals2.NullText = msgList.Err200102;
                }
                else
                {
                    glMeals2.NullText = msgList.Err200103;
                }
                //glMeals2.NullText = msgList.Err200102;
                Session["dtMealReturn"] = dtMeal;

                if (MealCode2 != null)
                {
                    DataTable dtMeal2 = new DataTable();
                    dtMeal2.Columns.Add("SSRCode");
                    dtMeal2.Columns.Add("Detail");
                    dtMeal2.Columns.Add("Price");
                    dtMeal2.Columns.Add("Images");

                    DataRow rowMeal2 = dtMeal2.NewRow();

                    Detail = "";
                    if (MealCode2.Count > 0 && DrinkCode2.Count >= 1)
                    {
                        foreach (string item in MealCode2)
                        {
                            Detail += "'" + item + "',";
                        }
                        Detail = Detail.Substring(0, Detail.Length - 1);

                        objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                        dt = objBooking.GetDetailSSRbyCode(Detail, "WYM");
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            //dtMeal2.Rows.Add("", "", "", "");
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                int b = MealCode2.IndexOf(dt.Rows[i]["ItemCode"].ToString());
                                if (b >= 0)
                                {
                                    dtMeal2.Rows.Add(MealCode2[b].ToString(), dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(MealFee2[b])).ToString("N", nfi) + " " + Currency, MealImg2[b].ToString());
                                }
                            }
                        }
                    }

                    DataView dvMeal2 = dtMeal2.DefaultView;
                    glMeals22.DataSource = dvMeal2.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                    glMeals22.DataBind();
                    if (dtMeal2.Rows.Count > 0)
                    {
                        glMeals22.NullText = msgList.Err200102;
                    }
                    else
                    {
                        glMeals22.NullText = msgList.Err200103;
                    }
                    //glMeals22.NullText = msgList.Err200102;
                    Session["dtMealReturn2"] = dtMeal2;
                    //gvPassenger2.Columns["Meal"].Caption = "Meal 1";
                }
                else
                {
                    divmeal2.Style.Add("display", "none");
                    gvPassenger2.Columns["Meal1"].Visible = false;
                }

                //--------------
                if (DrinkCode.Count >= 1)
                {
                    DataTable dtDrink = new DataTable();
                    dtDrink.Columns.Add("SSRCode");
                    dtDrink.Columns.Add("Detail");
                    dtDrink.Columns.Add("Price");
                    dtDrink.Columns.Add("ConcatenatedField", typeof(string), "Detail + '  ' +Price");

                    DataRow rowDrink = dtDrink.NewRow();

                    Detail = "";
                    foreach (string item in DrinkCode)
                    {
                        Detail += "'" + item + "',";
                    }
                    Detail = Detail.Substring(0, Detail.Length - 1);

                    objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                    dt = objBooking.GetDetailSSRbyCode(Detail, "WYD");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //dtMeal.Rows.Add("", "", "", "");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int b = DrinkCode.IndexOf(dt.Rows[i]["ItemCode"].ToString());
                            if (b >= 0)
                            {
                                dtDrink.Rows.Add(DrinkCode[b].ToString(), dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(DrinkFee[b])).ToString("N", nfi) + " " + Currency);

                            }
                        }
                    }

                    DataView dvDrink = dtDrink.DefaultView;
                    //dtDrink.DefaultView.RowFilter = "SSRCode LIKE 'BW%'";
                    cmbDrinks2.DataSource = dv.ToTable(true, "SSRCode", "Detail", "Price", "ConcatenatedField");
                    cmbDrinks2.DataSource = dtDrink.DefaultView;
                    cmbDrinks2.TextField = "ConcatenatedField";
                    cmbDrinks2.ValueField = "SSRCode";
                    cmbDrinks2.DataBind();
                    cmbDrinks2.NullText = msgList.Err200104;
                    Session["dtDrinkReturn"] = dtDrink;
                }
                else
                {
                    tdDrinks2.Style.Add("display", "none");
                    gvPassenger2.Columns["Drink"].Visible = false;
                }

                if (DrinkCode2 != null && DrinkCode2.Count >= 1)
                {
                    DataTable dtDrink2 = new DataTable();
                    dtDrink2.Columns.Add("SSRCode");
                    dtDrink2.Columns.Add("Detail");
                    dtDrink2.Columns.Add("Price");
                    dtDrink2.Columns.Add("ConcatenatedField", typeof(string), "Detail + '  ' +Price");

                    DataRow rowMeal2 = dtDrink2.NewRow();

                    Detail = "";
                    foreach (string item in DrinkCode2)
                    {
                        Detail += "'" + item + "',";
                    }
                    Detail = Detail.Substring(0, Detail.Length - 1);

                    objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                    dt = objBooking.GetDetailSSRbyCode(Detail, "WYD");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //dtMeal2.Rows.Add("", "", "", "");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int b = DrinkCode2.IndexOf(dt.Rows[i]["ItemCode"].ToString());
                            if (b >= 0)
                            {
                                dtDrink2.Rows.Add(DrinkCode2[b].ToString(), dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(DrinkFee2[b])).ToString("N", nfi) + " " + Currency);

                            }
                        }
                    }

                    DataView dvDrink2 = dtDrink2.DefaultView;
                    //dtDrink2.DefaultView.RowFilter = "SSRCode LIKE 'BW%'";
                    cmbDrinks22.DataSource = dvDrink2.ToTable(true, "SSRCode", "Detail", "Price", "ConcatenatedField");
                    cmbDrinks22.DataSource = dtDrink2.DefaultView;
                    //glDrinks22.DataSource = dvDrink2.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                    cmbDrinks22.TextField = "ConcatenatedField";
                    cmbDrinks22.ValueField = "SSRCode";
                    cmbDrinks22.DataBind();
                    cmbDrinks22.NullText = msgList.Err200104;
                    Session["dtDrinkReturn2"] = dtDrink2;
                    //gvPassenger2.Columns["Drink"].Caption = "Drink 1";
                }
                else
                {
                    tdDrinks22.Style.Add("display", "none");
                    gvPassenger2.Columns["Drink1"].Visible = false;
                }

                DataTable dtSport = new DataTable();
                dtSport.Columns.Add("SSRCode");
                dtSport.Columns.Add("Detail");
                dtSport.Columns.Add("Price");
                dtSport.Columns.Add("PriceS1");
                dtSport.Columns.Add("PriceS2");
                dtSport.Columns.Add("ConcatenatedField", typeof(string), "Detail + '  ' +Price");

                //Enumerable.Range(1, 10).ToList().ForEach(i => dtSport.Rows.Add(i, string.Concat("Detail", i), string.Concat("Price", i)));
                DataRow row1 = dtSport.NewRow();

                Detail = "";
                foreach (string item in SportCode)
                {
                    Detail += "'" + item + "',";
                }
                Detail = Detail.Substring(0, Detail.Length - 1);

                objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                dt = objBooking.GetDetailSSRbyCode(Detail, "PBS");
                if (dt != null && dt.Rows.Count > 0)
                {
                    //dtSport.Rows.Add("", "", "");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int b = SportCode.IndexOf(dt.Rows[i]["ItemCode"].ToString());
                        if (b >= 0)
                        {
                            dtSport.Rows.Add(SportCode[b].ToString(), dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(SportFee[b])).ToString("N", nfi) + " " + Currency, Convert.ToDecimal(SportFeeS1[b]), Convert.ToDecimal((b < SportFeeS2.Count) ? SportFeeS2[b].ToString() : "0.00"));
                        }
                    }
                }


                DataView dvSport = dtSport.DefaultView;
                cmbSport2.DataSource = dvSport.ToTable(true, "SSRCode", "Detail", "Price", "PriceS1", "PriceS2", "ConcatenatedField");

                cmbSport2.TextField = "ConcatenatedField";
                cmbSport2.ValueField = "SSRCode";
                cmbSport2.DataBind();
                cmbSport2.NullText = msgList.Err200105;
                Session["dtSportReturn"] = dtSport;

                DataTable dtDuty = new DataTable();
                dtDuty.Columns.Add("SSRCode");
                dtDuty.Columns.Add("Detail");
                dtDuty.Columns.Add("Price");
                dtDuty.Columns.Add("Images");

                DataRow rowDuty = dtBaggage.NewRow();

                Detail = "";
                foreach (string item in MealCode)
                {
                    Detail += "'" + item + "',";
                }
                Detail = Detail.Substring(0, Detail.Length - 1);

                objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                dt = objBooking.GetDetailSSRbyCode(Detail, "WCH");
                if (dt != null && dt.Rows.Count > 0)
                {
                    //dtDuty.Rows.Add("", "", "");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int b = DutyCode.IndexOf(dt.Rows[i]["ItemCode"].ToString());
                        if (b >= 0)
                        {
                            dtDuty.Rows.Add(DutyCode[b], dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(DutyFee[b])).ToString("N", nfi) + " " + Currency, DutyImg[b].ToString());
                        }
                    }
                }

                DataView dvDuty = dtDuty.DefaultView;
                glDuty2.DataSource = dvDuty.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                glDuty2.DataBind();
                glDuty2.NullText = msgList.Err200106;
                Session["dtDutyReturn"] = dtDuty;

                DataTable dtComfort = new DataTable();
                dtComfort.Columns.Add("SSRCode");
                dtComfort.Columns.Add("Detail");
                dtComfort.Columns.Add("Price");
                dtComfort.Columns.Add("PriceS1");
                dtComfort.Columns.Add("PriceS2");
                dtComfort.Columns.Add("Images");

                DataRow row2 = dtComfort.NewRow();

                Detail = "'" + ComfortCode[0] + "'";


                objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                dt = objBooking.GetDetailSSRbyCode(Detail, "PCM");
                if (dt != null && dt.Rows.Count > 0)
                {
                    //dtComfort.Rows.Add("", "", "", "");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int b = ComfortCode.IndexOf(dt.Rows[i]["ItemCode"].ToString());
                        if (b >= 0)
                        {
                            dtComfort.Rows.Add(ComfortCode[b], dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(ConfortFee[b])).ToString("N", nfi) + " " + Currency, Convert.ToDecimal(ConfortFeeS1[b]), Convert.ToDecimal((b < ConfortFeeS2.Count) ? ConfortFeeS2[b].ToString() : "0.00"), ComfortImg[b].ToString());
                        }
                    }
                }

                DataView dvComfort = dtComfort.DefaultView;
                glComfort2.DataSource = dvComfort.ToTable(true, "SSRCode", "Detail", "Price", "PriceS1", "PriceS2", "Images");
                glComfort2.DataBind();
                glComfort2.NullText = msgList.Err200107;
                Session["dtComfortReturn"] = dtComfort;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }

        }

        protected void Clearsession()
        {
            Session["InfantCode1"] = null;
            Session["InfantCode2"] = null;
            Session["IsNew"] = "True";
            //Session["dtGridPass"] = null;
            //Session["dtGridPass2"] = null;
            Session["dtBaggageDepart"] = null;
            Session["dtBaggageReturn"] = null;
            //Session["Currency"] = null;
            Session["dtMealDepart"] = null;
            Session["dtMealDepart2"] = null;
            Session["dtDutyDepart"] = null;
            Session["dtComfortDepart"] = null;
            Session["dtMealReturn"] = null;
            Session["dtMealReturn2"] = null;
            Session["dtDutyReturn"] = null;
            Session["dtComfortReturn"] = null;
            Session["qtyMeal"] = null;
            Session["qtyMeal1"] = null;
            Session["qtyBaggage"] = null;
            Session["qtySport"] = null;
            Session["qtyComfort"] = null;
            Session["qtyDuty"] = null;
            Session["qtyMeal2"] = null;
            Session["qtyMeal21"] = null;
            Session["qtyBaggage2"] = null;
            Session["qtySport2"] = null;
            Session["qtyComfort2"] = null;
            Session["qtyDuty2"] = null;
            Session["TotalAmountReturn"] = null;
            Session["TotalAmountDepart"] = null;
            //Session["transit"] = null;
            //Session["departTransit"] = null;
        }
        #endregion

        #region "Event"
        protected void gvPassenger_DataBinding(object sender, EventArgs e)
        {
            DataTable dtPass = new DataTable();
            if (Session["dtGridPass"] != null)
            {
                dtPass = (DataTable)Session["dtGridPass"];
            }
            else
            {
                dtPass = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTableNewManage(TransID, false, "");
            }
            (sender as ASPxGridView).DataSource = dtPass;
            HttpContext.Current.Session["dtGridPass"] = dtPass;
        }

        protected void gvPassenger2_DataBinding(object sender, EventArgs e)
        {
            DataTable dtPass2 = new DataTable();
            if (Session["dtGridPass2"] != null)
            {
                dtPass2 = (DataTable)Session["dtGridPass2"];
            }
            else
            {
                dtPass2 = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTableNewManage(TransID, false, "");
            }
            (sender as ASPxGridView).DataSource = dtPass2;
            HttpContext.Current.Session["dtGridPass2"] = dtPass2;
        }

        protected void SSRActionPanel_PerformCallback(object sender, CallbackEventArgsBase e)
        {
            object TotalBaggage, TotalMeal, TotalMeal1, TotalSport, TotalComfort, TotalDuty, TotalInfant;
            object TotalBaggage2, TotalMeal2, TotalMeal12, TotalSport2, TotalComfort2, TotalDuty2, TotalInfant2;

            try
            {
                if (Session["dtGridPass"] != null)
                {
                    DataTable dtPass = (DataTable)Session["dtGridPass"];
                    TotalBaggage = dtPass.Compute("Sum(PriceDepartBaggage)", "");
                    if (TotalBaggage == DBNull.Value)
                    {
                        TotalBaggage = 0;
                    }
                    TotalMeal = dtPass.Compute("Sum(PriceDepartMeal)", "");
                    if (TotalMeal == DBNull.Value)
                    {
                        TotalMeal = 0;
                    }
                    TotalMeal1 = dtPass.Compute("Sum(PriceConDepartMeal)", "");
                    if (TotalMeal1 == DBNull.Value)
                    {
                        TotalMeal1 = 0;
                    }
                    TotalSport = dtPass.Compute("Sum(PriceDepartSport)", "");
                    if (TotalSport == DBNull.Value)
                    {
                        TotalSport = 0;
                    }
                    TotalComfort = dtPass.Compute("Sum(PriceDepartComfort)", "");
                    if (TotalComfort == DBNull.Value)
                    {
                        TotalComfort = 0;
                    }
                    TotalInfant = dtPass.Compute("Sum(PriceDepartInfant)", "");
                    if (TotalInfant == DBNull.Value)
                    {
                        TotalInfant = 0;
                    }
                    //TotalDuty = dtPass.Compute("Sum(PriceDepartDuty)", "");

                    if (Session["TotalBaggage"] != null)
                    {
                        lblTotalBaggage.Text = (Convert.ToDecimal(TotalBaggage) - Convert.ToDecimal(Session["TotalBaggage"])).ToString("N", nfi);
                    }
                    else
                    {
                        lblTotalBaggage.Text = (Convert.ToDecimal(TotalBaggage)).ToString("N", nfi);
                    }

                    if (Session["TotalMeal"] != null && Session["TotalMeal1"] != null)
                    {
                        lblTotalMeal.Text = ((Convert.ToDecimal(TotalMeal) - Convert.ToDecimal(Session["TotalMeal"])) + (Convert.ToDecimal(TotalMeal1) - Convert.ToDecimal(Session["TotalMeal1"]))).ToString("N", nfi);
                    }
                    else
                    {
                        lblTotalMeal.Text = ((Convert.ToDecimal(TotalMeal)) + (Convert.ToDecimal(TotalMeal1))).ToString("N", nfi);
                    }

                    if (Session["TotalSport"] != null)
                    {
                        lblTotalSport.Text = (Convert.ToDecimal(TotalSport) - Convert.ToDecimal(Session["TotalSport"])).ToString("N", nfi);
                    }
                    else
                    {
                        lblTotalSport.Text = (Convert.ToDecimal(TotalSport)).ToString("N", nfi);
                    }

                    if (Session["TotalComfort"] != null)
                    {
                        lblTotalComfort.Text = (Convert.ToDecimal(TotalComfort) - Convert.ToDecimal(Session["TotalComfort"])).ToString("N", nfi);
                    }
                    else
                    {
                        lblTotalComfort.Text = (Convert.ToDecimal(TotalComfort)).ToString("N", nfi);
                    }
                    if (Session["TotalInfant"] != null)
                    {
                        lblTotalInfant.Text = (Convert.ToDecimal(TotalInfant) - Convert.ToDecimal(Session["TotalInfant"])).ToString("N", nfi);
                    }
                    else
                    {
                        lblTotalInfant.Text = (Convert.ToDecimal(TotalInfant)).ToString("N", nfi);
                    }

                    if (Session["dtGridPass2"] != null)
                    {
                        DataTable dtPass2 = (DataTable)Session["dtGridPass2"];
                        TotalBaggage2 = dtPass2.Compute("Sum(PriceReturnBaggage)", "");
                        if (TotalBaggage2 == DBNull.Value)
                        {
                            TotalBaggage2 = 0;
                        }
                        TotalMeal2 = dtPass2.Compute("Sum(PriceReturnMeal)", "");
                        if (TotalMeal2 == DBNull.Value)
                        {
                            TotalMeal2 = 0;
                        }
                        TotalMeal12 = dtPass2.Compute("Sum(PriceConReturnMeal)", "");
                        if (TotalMeal12 == DBNull.Value)
                        {
                            TotalMeal12 = 0;
                        }
                        TotalSport2 = dtPass2.Compute("Sum(PriceReturnSport)", "");
                        if (TotalSport2 == DBNull.Value)
                        {
                            TotalSport2 = 0;
                        }
                        TotalComfort2 = dtPass2.Compute("Sum(PriceReturnComfort)", "");
                        if (TotalComfort2 == DBNull.Value)
                        {
                            TotalComfort2 = 0;
                        }
                        TotalInfant2 = dtPass2.Compute("Sum(PriceReturnInfant)", "");
                        if (TotalInfant2 == DBNull.Value)
                        {
                            TotalInfant2 = 0;
                        }
                        //TotalDuty2 = dtPass2.Compute("Sum(PriceReturnDuty)", "");


                        //lblTotalDuty.Text = (Convert.ToDecimal(TotalDuty)).ToString("N", nfi);
                        //lblTotalAmount.Text = (Convert.ToDecimal(string.IsNullOrEmpty(lblTotalAmount.Text) ? "0" : lblTotalAmount.Text) + Convert.ToDecimal(TotalBaggage) + Convert.ToDecimal(TotalMeal)
                        //    + Convert.ToDecimal(TotalMeal1) + Convert.ToDecimal(TotalSport) + Convert.ToDecimal(TotalComfort)).ToString("N", nfi);

                        if (Session["TotalBaggage2"] != null)
                        {
                            lblTotalBaggage2.Text = (Convert.ToDecimal(TotalBaggage2) - Convert.ToDecimal(Session["TotalBaggage2"])).ToString("N", nfi);
                        }
                        else
                        {
                            lblTotalBaggage2.Text = (Convert.ToDecimal(TotalBaggage2)).ToString("N", nfi);
                        }

                        if (Session["TotalMeal2"] != null && Session["TotalMeal12"] != null)
                        {
                            lblTotalMeal2.Text = ((Convert.ToDecimal(TotalMeal2) - Convert.ToDecimal(Session["TotalMeal2"])) + (Convert.ToDecimal(TotalMeal12) - Convert.ToDecimal(Session["TotalMeal12"]))).ToString("N", nfi);
                        }
                        else
                        {
                            lblTotalMeal2.Text = ((Convert.ToDecimal(TotalMeal2)) + (Convert.ToDecimal(TotalMeal12))).ToString("N", nfi);
                        }

                        if (Session["TotalSport2"] != null)
                        {
                            lblTotalSport2.Text = (Convert.ToDecimal(TotalSport2) - Convert.ToDecimal(Session["TotalSport2"])).ToString("N", nfi);
                        }
                        else
                        {
                            lblTotalSport2.Text = (Convert.ToDecimal(TotalSport2)).ToString("N", nfi);
                        }

                        if (Session["TotalComfort2"] != null)
                        {
                            lblTotalComfort2.Text = (Convert.ToDecimal(TotalComfort2) - Convert.ToDecimal(Session["TotalComfort2"])).ToString("N", nfi);
                        }
                        else
                        {
                            lblTotalComfort2.Text = (Convert.ToDecimal(TotalComfort2)).ToString("N", nfi);
                        }
                        if (Session["TotalInfant2"] != null)
                        {
                            lblTotalInfant2.Text = (Convert.ToDecimal(TotalInfant2) - Convert.ToDecimal(Session["TotalInfant2"])).ToString("N", nfi);
                        }
                        else
                        {
                            lblTotalInfant2.Text = (Convert.ToDecimal(TotalInfant2)).ToString("N", nfi);
                        }
                        //lblTotalBaggage2.Text = (Convert.ToDecimal(TotalBaggage)).ToString("N", nfi);
                        //lblTotalMeal2.Text = (Convert.ToDecimal(TotalMeal) + Convert.ToDecimal(TotalMeal1)).ToString("N", nfi);
                        //lblTotalSport2.Text = (Convert.ToDecimal(TotalSport)).ToString("N", nfi);
                        //lblTotalComfort2.Text = (Convert.ToDecimal(TotalComfort)).ToString("N", nfi);
                        //lblTotalDuty2.Text = (Convert.ToDecimal(TotalDuty)).ToString("N", nfi);
                        //lblTotalAmount.Text = (Convert.ToDecimal(string.IsNullOrEmpty(lblTotalAmount.Text) ? "0" : lblTotalAmount.Text) + Convert.ToDecimal(TotalBaggage) + Convert.ToDecimal(TotalMeal)
                        //    + Convert.ToDecimal(TotalMeal1) + Convert.ToDecimal(TotalSport) + Convert.ToDecimal(TotalComfort)).ToString("N", nfi);
                        //lblTotalBaggage.Text = (Convert.ToDecimal(TotalBaggage) + Convert.ToDecimal(TotalBaggage2)).ToString("N", nfi);
                        //lblTotalMeal.Text = (Convert.ToDecimal(TotalMeal) + Convert.ToDecimal(TotalMeal2) + Convert.ToDecimal(TotalMeal1) + Convert.ToDecimal(TotalMeal12)).ToString("N", nfi);
                        //lblTotalSport.Text = (Convert.ToDecimal(TotalSport) + Convert.ToDecimal(TotalSport2)).ToString("N", nfi);
                        //lblTotalComfort.Text = (Convert.ToDecimal(TotalComfort) + Convert.ToDecimal(TotalComfort2)).ToString("N", nfi);
                        //lblTotalDuty.Text = (Convert.ToDecimal(TotalDuty) + Convert.ToDecimal(TotalDuty2)).ToString("N", nfi);
                        lblTotalAmount.Text = (Convert.ToDecimal(string.IsNullOrEmpty(lblTotalBaggage.Text) ? "0" : lblTotalBaggage.Text) + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalMeal.Text) ? "0" : lblTotalMeal.Text)
                            + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalSport.Text) ? "0" : lblTotalSport.Text) + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalComfort.Text) ? "0" : lblTotalComfort.Text) + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalInfant.Text) ? "0" : lblTotalInfant.Text) + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalBaggage2.Text) ? "0" : lblTotalBaggage2.Text) + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalMeal2.Text) ? "0" : lblTotalMeal2.Text)
                            + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalSport2.Text) ? "0" : lblTotalSport2.Text) + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalComfort2.Text) ? "0" : lblTotalComfort2.Text) + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalInfant2.Text) ? "0" : lblTotalInfant2.Text)).ToString("N", nfi);
                    }
                    else
                    {
                        //lblTotalBaggage.Text = (Convert.ToDecimal(TotalBaggage)).ToString("N", nfi);
                        //lblTotalMeal.Text = (Convert.ToDecimal(TotalMeal) + Convert.ToDecimal(TotalMeal1)).ToString("N", nfi);
                        //lblTotalSport.Text = (Convert.ToDecimal(TotalSport)).ToString("N", nfi);
                        //lblTotalComfort.Text = (Convert.ToDecimal(TotalComfort)).ToString("N", nfi);
                        //lblTotalDuty.Text = (Convert.ToDecimal(TotalDuty)).ToString("N", nfi);
                        //lblTotalAmount.Text = (Convert.ToDecimal(TotalBaggage) + Convert.ToDecimal(TotalMeal)
                        //    + Convert.ToDecimal(TotalMeal1) + Convert.ToDecimal(TotalSport) + Convert.ToDecimal(TotalComfort)).ToString("N", nfi);

                        lblTotalAmount.Text = (Convert.ToDecimal(string.IsNullOrEmpty(lblTotalAmount.Text) ? "0" : lblTotalAmount.Text) + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalBaggage.Text) ? "0" : lblTotalBaggage.Text) + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalMeal.Text) ? "0" : lblTotalMeal.Text)
                            + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalSport.Text) ? "0" : lblTotalSport.Text) + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalComfort.Text) ? "0" : lblTotalComfort.Text) + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalInfant.Text) ? "0" : lblTotalInfant.Text)).ToString("N", nfi);
                    }

                    lblCurrency.Text = Session["Currency"].ToString();
                }

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void SSRTab1Panel_PerformCallback(object sender, CallbackEventArgsBase e)
        {
            object TotalBaggage, TotalMeal, TotalMeal1, TotalSport, TotalComfort, TotalDuty, TotalInfant;
            try
            {
                if (Session["dtGridPass"] != null)
                {
                    DataTable dtPass = (DataTable)Session["dtGridPass"];
                    TotalBaggage = dtPass.Compute("Sum(PriceDepartBaggage)", "");
                    if (TotalBaggage == DBNull.Value)
                    {
                        TotalBaggage = 0;
                    }
                    TotalMeal = dtPass.Compute("Sum(PriceDepartMeal)", "");
                    if (TotalMeal == DBNull.Value)
                    {
                        TotalMeal = 0;
                    }
                    TotalMeal1 = dtPass.Compute("Sum(PriceConDepartMeal)", "");
                    if (TotalMeal1 == DBNull.Value)
                    {
                        TotalMeal1 = 0;
                    }
                    TotalSport = dtPass.Compute("Sum(PriceDepartSport)", "");
                    if (TotalSport == DBNull.Value)
                    {
                        TotalSport = 0;
                    }
                    TotalComfort = dtPass.Compute("Sum(PriceDepartComfort)", "");
                    if (TotalComfort == DBNull.Value)
                    {
                        TotalComfort = 0;
                    }
                    TotalInfant = dtPass.Compute("Sum(PriceDepartInfant)", "");
                    if (TotalInfant == DBNull.Value)
                    {
                        TotalInfant = 0;
                    }
                    // TotalDuty = dtPass.Compute("Sum(PriceDepartDuty)", "");
                    if (Session["TotalBaggage"] != null)
                    {
                        lblTotalBaggage.Text = (Convert.ToDecimal(TotalBaggage) - Convert.ToDecimal(Session["TotalBaggage"])).ToString("N", nfi);
                    }
                    else
                    {
                        lblTotalBaggage.Text = (Convert.ToDecimal(TotalBaggage)).ToString("N", nfi);
                    }

                    if (Session["TotalMeal"] != null && Session["TotalMeal1"] != null)
                    {
                        lblTotalMeal.Text = ((Convert.ToDecimal(TotalMeal) - Convert.ToDecimal(Session["TotalMeal"])) + (Convert.ToDecimal(TotalMeal1) - Convert.ToDecimal(Session["TotalMeal1"]))).ToString("N", nfi);
                    }
                    else
                    {
                        lblTotalMeal.Text = ((Convert.ToDecimal(TotalMeal)) + (Convert.ToDecimal(TotalMeal1))).ToString("N", nfi);
                    }

                    if (Session["TotalSport"] != null)
                    {
                        lblTotalSport.Text = (Convert.ToDecimal(TotalSport) - Convert.ToDecimal(Session["TotalSport"])).ToString("N", nfi);
                    }
                    else
                    {
                        lblTotalSport.Text = (Convert.ToDecimal(TotalSport)).ToString("N", nfi);
                    }

                    if (Session["TotalComfort"] != null)
                    {
                        lblTotalComfort.Text = (Convert.ToDecimal(TotalComfort) - Convert.ToDecimal(Session["TotalComfort"])).ToString("N", nfi);
                    }
                    else
                    {
                        lblTotalComfort.Text = (Convert.ToDecimal(TotalComfort)).ToString("N", nfi);
                    }
                    if (Session["TotalInfant"] != null)
                    {
                        lblTotalInfant.Text = (Convert.ToDecimal(TotalInfant) - Convert.ToDecimal(Session["TotalInfant"])).ToString("N", nfi);
                    }
                    else
                    {
                        lblTotalInfant.Text = (Convert.ToDecimal(TotalInfant)).ToString("N", nfi);
                    }
                    //lblTotalDuty.Text = (Convert.ToDecimal(TotalDuty)).ToString("N", nfi);
                    //lblTotalAmount.Text = (Convert.ToDecimal(string.IsNullOrEmpty(lblTotalAmount.Text) ? "0" : lblTotalAmount.Text) + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalBaggage.Text) ? "0" : lblTotalBaggage.Text) + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalMeal.Text) ? "0" : lblTotalMeal.Text)
                    //        + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalSport.Text) ? "0" : lblTotalSport.Text) + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalComfort.Text) ? "0" : lblTotalComfort.Text)).ToString("N", nfi);

                    lblCurrency.Text = Session["Currency"].ToString();

                    if (Session["InfantCode"] == null)
                    {
                        InfantIcon1.Style.Add("display", "none");
                    }
                }

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void SSRTab2Panel_PerformCallback(object sender, CallbackEventArgsBase e)
        {
            object TotalBaggage, TotalMeal, TotalMeal1, TotalSport, TotalComfort, TotalDuty, TotalInfant;
            try
            {
                if (Session["dtGridPass2"] != null)
                {
                    DataTable dtPass = (DataTable)Session["dtGridPass2"];
                    TotalBaggage = dtPass.Compute("Sum(PriceReturnBaggage)", "");
                    if (TotalBaggage == DBNull.Value)
                    {
                        TotalBaggage = 0;
                    }
                    TotalMeal = dtPass.Compute("Sum(PriceReturnMeal)", "");
                    if (TotalMeal == DBNull.Value)
                    {
                        TotalMeal = 0;
                    }
                    TotalMeal1 = dtPass.Compute("Sum(PriceConReturnMeal)", "");
                    if (TotalMeal1 == DBNull.Value)
                    {
                        TotalMeal1 = 0;
                    }
                    TotalSport = dtPass.Compute("Sum(PriceReturnSport)", "");
                    if (TotalSport == DBNull.Value)
                    {
                        TotalSport = 0;
                    }
                    TotalComfort = dtPass.Compute("Sum(PriceReturnComfort)", "");
                    if (TotalComfort == DBNull.Value)
                    {
                        TotalComfort = 0;
                    }
                    TotalInfant = dtPass.Compute("Sum(PriceReturnInfant)", "");
                    if (TotalInfant == DBNull.Value)
                    {
                        TotalInfant = 0;
                    }

                    if (Session["TotalBaggage2"] != null)
                    {
                        lblTotalBaggage2.Text = (Convert.ToDecimal(TotalBaggage) - Convert.ToDecimal(Session["TotalBaggage2"])).ToString("N", nfi);
                    }
                    else
                    {
                        lblTotalBaggage2.Text = (Convert.ToDecimal(TotalBaggage)).ToString("N", nfi);
                    }

                    if (Session["TotalMeal2"] != null && Session["TotalMeal12"] != null)
                    {
                        lblTotalMeal2.Text = ((Convert.ToDecimal(TotalMeal) - Convert.ToDecimal(Session["TotalMeal2"])) + (Convert.ToDecimal(TotalMeal1) - Convert.ToDecimal(Session["TotalMeal12"]))).ToString("N", nfi);
                    }
                    else
                    {
                        lblTotalMeal2.Text = ((Convert.ToDecimal(TotalMeal)) + (Convert.ToDecimal(TotalMeal1))).ToString("N", nfi);
                    }

                    if (Session["TotalSport2"] != null)
                    {
                        lblTotalSport2.Text = (Convert.ToDecimal(TotalSport) - Convert.ToDecimal(Session["TotalSport2"])).ToString("N", nfi);
                    }
                    else
                    {
                        lblTotalSport2.Text = (Convert.ToDecimal(TotalSport)).ToString("N", nfi);
                    }

                    if (Session["TotalComfort2"] != null)
                    {
                        lblTotalComfort2.Text = (Convert.ToDecimal(TotalComfort) - Convert.ToDecimal(Session["TotalComfort2"])).ToString("N", nfi);
                    }
                    else
                    {
                        lblTotalComfort2.Text = (Convert.ToDecimal(TotalComfort)).ToString("N", nfi);
                    }
                    if (Session["TotalInfant2"] != null)
                    {
                        lblTotalInfant2.Text = (Convert.ToDecimal(TotalInfant) - Convert.ToDecimal(Session["TotalInfant2"])).ToString("N", nfi);
                    }
                    else
                    {
                        lblTotalInfant2.Text = (Convert.ToDecimal(TotalInfant)).ToString("N", nfi);
                    }

                    lblCurrency.Text = Session["Currency"].ToString();

                    if (Session["InfantCode2"] == null)
                    {
                        InfantIcon2.Style.Add("display", "none");
                    }
                }

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void BindDefaultDrink()
        {
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;

            DataTable dtdefaultDrink = new DataTable();
            dtdefaultDrink = objBooking.GetArrayCategory("DEFAULTDRINK");
            Session["defaultdrink"] = dtdefaultDrink;

            UIClass.SetComboStyle(ref cmbNation, UIClass.EnumDefineStyle.CountryCard);
            UIClass.SetComboStyle(ref cmbPassCountry, UIClass.EnumDefineStyle.CountryCard);

            List<ABS.Logic.GroupBooking.Booking.BookingTransactionDetail> BookingTransactionDetail;
            using (profiler.Step("Get_TRANSDTL"))
            {
                BookingTransactionDetail = objBooking.Get_TRANSDTL(MyUserSet.AgentID, TransID);
            }
            using (profiler.Step("IsInternationalFlight"))
            {
                IsInternationalFlight = objGeneral.IsInternationalFlight(BookingTransactionDetail[0].Origin, BookingTransactionDetail[0].Destination, Request.PhysicalApplicationPath);
            }
            hfInternational.Value = IsInternationalFlight.ToString();

            using (profiler.Step("GetSingleBK_TRANSMAIN"))
            {
                bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(TransID);
            }
            Session["bookHDRInfo"] = bookHDRInfo;
        }
        protected void BindLabel()
        {
            object TotalBaggage, TotalMeal, TotalMeal1, TotalSport, TotalComfort, TotalInfant;
            object TotalBaggage2, TotalMeal2, TotalMeal12, TotalSport2, TotalComfort2, TotalInfant2;

            try
            {
                if (Session["dtGridPass"] != null)
                {
                    DataTable dtPass = (DataTable)Session["dtGridPass"];
                    TotalBaggage = dtPass.Compute("Sum(PriceDepartBaggage)", "");
                    if (TotalBaggage == DBNull.Value)
                    {
                        TotalBaggage = 0;
                    }
                    TotalMeal = dtPass.Compute("Sum(PriceDepartMeal)", "");
                    if (TotalMeal == DBNull.Value)
                    {
                        TotalMeal = 0;
                    }
                    TotalMeal1 = dtPass.Compute("Sum(PriceConDepartMeal)", "");
                    if (TotalMeal1 == DBNull.Value)
                    {
                        TotalMeal1 = 0;
                    }
                    TotalSport = dtPass.Compute("Sum(PriceDepartSport)", "");
                    if (TotalSport == DBNull.Value)
                    {
                        TotalSport = 0;
                    }
                    TotalComfort = dtPass.Compute("Sum(PriceDepartComfort)", "");
                    if (TotalComfort == DBNull.Value)
                    {
                        TotalComfort = 0;
                    }
                    TotalInfant = dtPass.Compute("Sum(PriceDepartInfant)", "");
                    if (TotalInfant == DBNull.Value)
                    {
                        TotalInfant = 0;
                    }

                    Decimal TotalAmountDepart = (Convert.ToDecimal(TotalBaggage) + Convert.ToDecimal(TotalMeal)
                            + Convert.ToDecimal(TotalMeal1) + Convert.ToDecimal(TotalSport) + Convert.ToDecimal(TotalComfort) + Convert.ToDecimal(TotalInfant));

                    if (Session["TotalBaggage"] == null)
                    {
                        Session["TotalBaggage"] = TotalBaggage;
                    }

                    if (Session["TotalMeal"] == null)
                    {
                        Session["TotalMeal"] = TotalMeal;
                    }

                    if (Session["TotalMeal1"] == null)
                    {
                        Session["TotalMeal1"] = TotalMeal1;
                    }

                    if (Session["TotalSport"] == null)
                    {
                        Session["TotalSport"] = TotalSport;
                    }

                    if (Session["TotalComfort"] == null)
                    {
                        Session["TotalComfort"] = TotalComfort;
                    }

                    if (Session["TotalInfant"] == null)
                    {
                        Session["TotalInfant"] = TotalInfant;
                    }

                    if (Session["TotalAmountDepart"] == null)
                    {
                        Session["TotalAmountDepart"] = TotalAmountDepart;
                    }
                    //TotalDuty = dtPass.Compute("Sum(PriceDepartDuty)", "");



                    if (Session["dtGridPass2"] != null)
                    {
                        DataTable dtPass2 = (DataTable)Session["dtGridPass2"];
                        TotalBaggage2 = dtPass2.Compute("Sum(PriceReturnBaggage)", "");
                        if (TotalBaggage2 == DBNull.Value)
                        {
                            TotalBaggage2 = 0;
                        }
                        TotalMeal2 = dtPass2.Compute("Sum(PriceReturnMeal)", "");
                        if (TotalMeal2 == DBNull.Value)
                        {
                            TotalMeal2 = 0;
                        }
                        TotalMeal12 = dtPass2.Compute("Sum(PriceConReturnMeal)", "");
                        if (TotalMeal12 == DBNull.Value)
                        {
                            TotalMeal12 = 0;
                        }
                        TotalSport2 = dtPass2.Compute("Sum(PriceReturnSport)", "");
                        if (TotalSport2 == DBNull.Value)
                        {
                            TotalSport2 = 0;
                        }
                        TotalComfort2 = dtPass2.Compute("Sum(PriceReturnComfort)", "");
                        if (TotalComfort2 == DBNull.Value)
                        {
                            TotalComfort2 = 0;
                        }
                        TotalInfant2 = dtPass2.Compute("Sum(PriceReturnInfant)", "");
                        if (TotalInfant2 == DBNull.Value)
                        {
                            TotalInfant2 = 0;
                        }

                        Decimal TotalAmountReturn = (Convert.ToDecimal(TotalBaggage2) + Convert.ToDecimal(TotalMeal2)
                            + Convert.ToDecimal(TotalMeal12) + Convert.ToDecimal(TotalSport2) + Convert.ToDecimal(TotalComfort2) + Convert.ToDecimal(TotalInfant2));

                        if (Session["TotalBaggage2"] == null)
                        {
                            Session["TotalBaggage2"] = TotalBaggage2;
                        }

                        if (Session["TotalMeal2"] == null)
                        {
                            Session["TotalMeal2"] = TotalMeal2;
                        }

                        if (Session["TotalMeal12"] == null)
                        {
                            Session["TotalMeal12"] = TotalMeal12;
                        }

                        if (Session["TotalSport2"] == null)
                        {
                            Session["TotalSport2"] = TotalSport2;
                        }

                        if (Session["TotalComfort2"] == null)
                        {
                            Session["TotalComfort2"] = TotalComfort2;
                        }

                        if (Session["TotalInfant2"] == null)
                        {
                            Session["TotalInfant2"] = TotalInfant2;
                        }

                        if (Session["TotalAmountReturn"] == null)
                        {
                            Session["TotalAmountReturn"] = TotalAmountReturn;
                        }


                        lblTotalBaggage.Text = (Convert.ToDecimal(0)).ToString("N", nfi);
                        lblTotalMeal.Text = (Convert.ToDecimal(0) + Convert.ToDecimal(0)).ToString("N", nfi);
                        lblTotalSport.Text = (Convert.ToDecimal(0)).ToString("N", nfi);
                        lblTotalComfort.Text = (Convert.ToDecimal(0)).ToString("N", nfi);
                        lblTotalInfant.Text = (Convert.ToDecimal(0)).ToString("N", nfi);
                        //lblTotalDuty.Text = (Convert.ToDecimal(TotalDuty)).ToString("N", nfi);

                        lblTotalBaggage2.Text = (Convert.ToDecimal(0)).ToString("N", nfi);
                        lblTotalMeal2.Text = (Convert.ToDecimal(0) + Convert.ToDecimal(0) + Convert.ToDecimal(0)).ToString("N", nfi);
                        lblTotalSport2.Text = (Convert.ToDecimal(0)).ToString("N", nfi);
                        lblTotalComfort2.Text = (Convert.ToDecimal(0)).ToString("N", nfi);
                        lblTotalInfant2.Text = (Convert.ToDecimal(0)).ToString("N", nfi);
                        //lblTotalDuty2.Text = (Convert.ToDecimal(TotalDuty2)).ToString("N", nfi);
                        lblTotalAmount.Text = (Convert.ToDecimal(0) + Convert.ToDecimal(0) + Convert.ToDecimal(0)
                            + Convert.ToDecimal(0) + Convert.ToDecimal(0) + Convert.ToDecimal(0)
                            + Convert.ToDecimal(0) + Convert.ToDecimal(0) + Convert.ToDecimal(0)
                            + Convert.ToDecimal(0)).ToString("N", nfi);



                    }
                    else
                    {


                        lblTotalBaggage.Text = (Convert.ToDecimal(0)).ToString("N", nfi);
                        lblTotalMeal.Text = (Convert.ToDecimal(0) + Convert.ToDecimal(0)).ToString("N", nfi);
                        lblTotalSport.Text = (Convert.ToDecimal(0)).ToString("N", nfi);
                        lblTotalComfort.Text = (Convert.ToDecimal(0)).ToString("N", nfi);
                        lblTotalInfant.Text = (Convert.ToDecimal(0)).ToString("N", nfi);
                        //blTotalDuty.Text = (Convert.ToDecimal(TotalDuty)).ToString("N", nfi);
                        lblTotalAmount.Text = (Convert.ToDecimal(0) + Convert.ToDecimal(0) + Convert.ToDecimal(0) + Convert.ToDecimal(0) + Convert.ToDecimal(0)).ToString("N", nfi);
                    }

                    if (Session["TotalAmountReturn"] == null)
                    {
                        Session["TotalAmountReturn"] = Convert.ToDecimal(0);
                    }

                    lblCurrency.Text = Session["Currency"].ToString();
                }

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void glMeals_Init(object sender, EventArgs e)
        {
            try
            {
                if (Session["dtMealDepart"] != null)
                {
                    DataTable dtMeal = (DataTable)Session["dtMealDepart"];
                    DataView dvMeal = dtMeal.DefaultView;
                    glMeals.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                    glMeals.DataBind();
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void glMeals1_Init(object sender, EventArgs e)
        {
            try
            {
                if (Session["dtMealDepart2"] != null)
                {
                    DataTable dtMeal = (DataTable)Session["dtMealDepart2"];
                    DataView dvMeal = dtMeal.DefaultView;
                    glMeals1.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                    glMeals1.DataBind();
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void glDuty_Init(object sender, EventArgs e)
        {
            try
            {
                if (Session["dtDutyDepart"] != null)
                {
                    DataTable dtDuty = (DataTable)Session["dtDutyDepart"];
                    DataView dvDuty = dtDuty.DefaultView;
                    glDuty.DataSource = dvDuty.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                    glDuty.DataBind();
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void glComfort_Init(object sender, EventArgs e)
        {
            try
            {
                if (Session["dtComfortDepart"] != null)
                {
                    DataTable dtComfort = (DataTable)Session["dtComfortDepart"];
                    DataView dvComfort = dtComfort.DefaultView;
                    glComfort.DataSource = dvComfort.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                    glComfort.DataBind();
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void glMeals2_Init(object sender, EventArgs e)
        {
            try
            {
                if (Session["dtMealReturn"] != null)
                {
                    DataTable dtMeal = (DataTable)Session["dtMealReturn"];
                    DataView dvMeal = dtMeal.DefaultView;
                    glMeals2.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                    glMeals2.DataBind();
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void glMeals22_Init(object sender, EventArgs e)
        {
            try
            {
                if (Session["dtMealReturn2"] != null)
                {
                    DataTable dtMeal = (DataTable)Session["dtMealReturn2"];
                    DataView dvMeal = dtMeal.DefaultView;
                    glMeals22.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                    glMeals22.DataBind();
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void glDuty2_Init(object sender, EventArgs e)
        {
            try
            {
                if (Session["dtDutyReturn"] != null)
                {
                    DataTable dtDuty = (DataTable)Session["dtDutyReturn"];
                    DataView dvDuty = dtDuty.DefaultView;
                    glDuty2.DataSource = dvDuty.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                    glDuty2.DataBind();
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void glComfort2_Init(object sender, EventArgs e)
        {
            try
            {
                if (Session["dtComfortReturn"] != null)
                {
                    DataTable dtComfort = (DataTable)Session["dtComfortReturn"];
                    DataView dvComfort = dtComfort.DefaultView;
                    glComfort2.DataSource = dvComfort.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                    glComfort2.DataBind();
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void gvPassenger_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex < 0)
            {
                return;
            }

            Object Infantcolumn = gvPassenger.GetRowValues(e.VisibleIndex, "DepartInfant");
            Object PassengerIDcolumn = gvPassenger.GetRowValues(e.VisibleIndex, "PassengerID");
            Object SeqNocolumn = gvPassenger.GetRowValues(e.VisibleIndex, "SeqNo");
            Object PNRcolumn = gvPassenger.GetRowValues(e.VisibleIndex, "PNR");

            if (Session["dtInfant"] != null)
            {
                DataTable dtInfant = (DataTable)Session["dtInfant"];
                if (e.ButtonID == "btnDetails")
                {
                    if (dtInfant.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtInfant.Rows.Count; i++)
                        {
                            if (Convert.ToInt16(PassengerIDcolumn).ToString().IndexOf(dtInfant.Rows[i]["PassengerID"].ToString()) >= 0 && PNRcolumn.ToString().IndexOf(dtInfant.Rows[i]["RecordLocator"].ToString()) >= 0)
                            {
                                if (Infantcolumn.ToString() != "" && dtInfant.Rows[i]["FirstName"].ToString() == "Infant" && dtInfant.Rows[i]["LastName"].ToString() == "Infant")
                                {
                                    e.Visible = DefaultBoolean.True;

                                }
                                else
                                {
                                    e.Visible = DefaultBoolean.False;

                                }
                                break;
                            }
                            else
                            {
                                if (Infantcolumn.ToString() != "")
                                {
                                    e.Visible = DefaultBoolean.True;
                                }
                                else
                                {
                                    e.Visible = DefaultBoolean.False;
                                }
                            }
                        }
                    }
                    else
                    {
                        e.Visible = DefaultBoolean.False;
                    }
                }
            }
            else
            {
                if (Infantcolumn.ToString() != "")
                {
                    e.Visible = DefaultBoolean.True;
                }
                else
                {
                    e.Visible = DefaultBoolean.False;
                }
            }

        }

        protected void gvPassenger2_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex < 0)
            {
                return;
            }

            Object Infantcolumn = gvPassenger2.GetRowValues(e.VisibleIndex, "DepartInfant");
            Object PassengerIDcolumn = gvPassenger2.GetRowValues(e.VisibleIndex, "PassengerID");
            Object SeqNocolumn = gvPassenger2.GetRowValues(e.VisibleIndex, "SeqNo");
            Object PNRcolumn = gvPassenger2.GetRowValues(e.VisibleIndex, "PNR");

            if (Session["dtInfant"] != null)
            {
                DataTable dtInfant = (DataTable)Session["dtInfant"];
                if (e.ButtonID == "btnDetails2")
                {
                    if (dtInfant.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtInfant.Rows.Count; i++)
                        {
                            if (Convert.ToInt16(PassengerIDcolumn).ToString().IndexOf(dtInfant.Rows[i]["PassengerID"].ToString()) >= 0 && PNRcolumn.ToString().IndexOf(dtInfant.Rows[i]["RecordLocator"].ToString()) >= 0)
                            {
                                if (Infantcolumn.ToString() != "" && dtInfant.Rows[i]["FirstName"].ToString() == "Infant" && dtInfant.Rows[i]["LastName"].ToString() == "Infant")
                                {
                                    e.Visible = DefaultBoolean.True;

                                }
                                else
                                {
                                    e.Visible = DefaultBoolean.False;

                                }
                                break;
                            }
                            else
                            {
                                if (Infantcolumn.ToString() != "")
                                {
                                    e.Visible = DefaultBoolean.True;
                                }
                                else
                                {
                                    e.Visible = DefaultBoolean.False;
                                }
                            }
                        }
                    }
                    else
                    {
                        e.Visible = DefaultBoolean.False;
                    }
                }
            }
            else
            {
                if (Infantcolumn.ToString() != "")
                {
                    e.Visible = DefaultBoolean.True;
                }
                else
                {
                    e.Visible = DefaultBoolean.False;
                }
            }

        }

        protected void callbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (Session["lstPassInfantData"] != null)
            {
                lstPassInfantData = (List<PassengerData>)Session["lstPassInfantData"];
                string index = hfIndex.Value;
                if (lstPassInfantData.Count > 0 && index != "")
                {

                    List<PassengerData> NewlstPassInfantData = lstPassInfantData.Where(item => item.PassengerID == (Convert.ToInt16(index) - 1).ToString()).ToList();
                    if (NewlstPassInfantData.Count > 0)
                    {
                        foreach (PassengerData PassData in NewlstPassInfantData)
                        {
                            if (PassData.Gender != "")
                            {
                                string a = PassData.Gender;
                                if (a.Length > 1)
                                {
                                    a = a.Substring(0, 1).ToUpper() + a.Substring(1, a.Length - 1).ToLower();
                                    cmbGender.Items.FindByValue(a.ToString()).Selected = true;
                                }
                            }
                            if (PassData.Nationality != "")
                            {
                                string a = PassData.Nationality;
                                if (a.Length > 1)
                                {
                                    //a = a.Substring(0, 1).ToUpper() + a.Substring(1, a.Length - 1).ToLower();
                                    cmbNation.Items.FindByValue(a.ToString()).Selected = true;
                                }
                            }
                            if (PassData.IssuingCountry != "")
                            {
                                string a = PassData.IssuingCountry;
                                if (a.Length > 1)
                                {
                                    //a = a.Substring(0, 1).ToUpper() + a.Substring(1, a.Length - 1).ToLower();
                                    cmbPassCountry.Items.FindByValue(a.ToString()).Selected = true;
                                }
                            }
                            if (PassData.IssuingCountry != "")
                            {
                                string a = PassData.IssuingCountry;
                                if (a.Length > 1)
                                {
                                    //a = a.Substring(0, 1).ToUpper() + a.Substring(1, a.Length - 1).ToLower();
                                    cmbPassCountry.Items.FindByValue(a.ToString()).Selected = true;
                                }
                            }
                            if (PassData.DOB.ToString() != "")
                            {
                                txtDOB.Value = Convert.ToDateTime(PassData.DOB.ToString());
                            }
                            if (PassData.ExpiryDate.ToString() != "")
                            {
                                txtExpired.Value = Convert.ToDateTime(PassData.ExpiryDate.ToString()); ;
                            }

                            txt_FirstName.Text = PassData.FirstName.ToString();
                            txt_LastName.Text = PassData.LastName.ToString();

                            txtPassportNo.Text = PassData.PassportNo.ToString();

                        }
                    }
                    else
                    {
                        cmbGender.SelectedIndex = -1;
                        cmbNation.SelectedIndex = -1;
                        cmbPassCountry.SelectedIndex = -1;
                        txtDOB.Value = null;
                        txtExpired.Value = null;
                        txtPassportNo.Text = "";
                        txt_FirstName.Text = "";
                        txt_LastName.Text = "";
                        if (Session["bookHDRInfo"] != null)
                        {
                            bookHDRInfo = (BookingTransactionMain)Session["bookHDRInfo"];
                            txtDOB.MinDate = DateTime.Parse("1900-01-01");
                            txtDOB.MaxDate = DateTime.Today.AddDays(-1);
                            txtAdt.MinDate = DateTime.Parse("1900-01-01");
                            txtAdt.MaxDate = bookHDRInfo.STDDate.AddYears(-12);
                            txtChd.MinDate = bookHDRInfo.STDDate.AddYears(-12);
                            txtChd.MaxDate = bookHDRInfo.STDDate.AddDays(-9);
                        }
                        //txtExpired.MinDate = bookHDRInfo.STDDate;

                    }
                }
                else
                {
                    cmbGender.SelectedIndex = -1;
                    cmbNation.SelectedIndex = -1;
                    cmbPassCountry.SelectedIndex = -1;
                    txtDOB.Value = null;
                    txtExpired.Value = null;
                    txtPassportNo.Text = "";
                    txt_FirstName.Text = "";
                    txt_LastName.Text = "";
                    if (Session["bookHDRInfo"] != null)
                    {
                        bookHDRInfo = (BookingTransactionMain)Session["bookHDRInfo"];
                        txtDOB.MinDate = DateTime.Parse("1900-01-01");
                        txtDOB.MaxDate = DateTime.Today.AddDays(-1);
                        txtAdt.MinDate = DateTime.Parse("1900-01-01");
                        txtAdt.MaxDate = bookHDRInfo.STDDate.AddYears(-12);
                        txtChd.MinDate = bookHDRInfo.STDDate.AddYears(-12);
                        txtChd.MaxDate = bookHDRInfo.STDDate.AddDays(-9);
                    }
                }
            }
            else
            {
                cmbGender.SelectedIndex = -1;
                cmbNation.SelectedIndex = -1;
                cmbPassCountry.SelectedIndex = -1;
                txtDOB.Value = null;
                txtExpired.Value = null;
                txtPassportNo.Text = "";
                txt_FirstName.Text = "";
                txt_LastName.Text = "";
                if (Session["bookHDRInfo"] != null)
                {
                    bookHDRInfo = (BookingTransactionMain)Session["bookHDRInfo"];
                    txtDOB.MinDate = DateTime.Parse("1900-01-01");
                    txtDOB.MaxDate = bookHDRInfo.STDDate.AddDays(-9);
                    txtAdt.MinDate = DateTime.Parse("1900-01-01");
                    txtAdt.MaxDate = bookHDRInfo.STDDate.AddYears(-12);
                    txtChd.MinDate = bookHDRInfo.STDDate.AddYears(-12);
                    txtChd.MaxDate = bookHDRInfo.STDDate.AddDays(-9);
                }
            }
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            var profiler = MiniProfiler.Current;
            using (profiler.Step("Save"))
            {
                if (ASPxEdit.AreEditorsValid(callbackPanel))
                {
                    string index = hfIndex.Value;
                    DataTable dataPassenger = new DataTable();
                    dataPassenger = (DataTable)HttpContext.Current.Session["dtGridPass"];
                    if (Session["lstPassInfantData"] != null)
                    {
                        lstPassInfantData = (List<PassengerData>)Session["lstPassInfantData"];
                        List<PassengerData> NewlstPassInfantData = lstPassInfantData.Where(item => item.PassengerID == (Convert.ToInt16(index) - 1).ToString()).ToList();
                        if (NewlstPassInfantData.Count > 0)
                        {
                            foreach (PassengerData PassData in NewlstPassInfantData)
                            {
                                PassData.TransID = dataPassenger.Rows[Convert.ToInt16(index) - 1]["TransID"].ToString();
                                PassData.RecordLocator = dataPassenger.Rows[Convert.ToInt16(index) - 1]["PNR"].ToString();
                                PassData.PassengerID = dataPassenger.Rows[Convert.ToInt16(index) - 1]["PassengerID"].ToString();
                                PassData.Title = "INFT";
                                PassData.Gender = cmbGender.SelectedItem.Value.ToString();
                                PassData.FirstName = txt_FirstName.Text;
                                PassData.LastName = txt_LastName.Text;
                                PassData.Nationality = cmbNation.SelectedItem.Value.ToString();
                                PassData.IssuingCountry = cmbPassCountry.SelectedItem.Value.ToString();
                                PassData.DOB = Convert.ToDateTime(txtDOB.Value);

                                if (IsInternationalFlight)
                                {
                                    PassData.PassportNo = txtPassportNo.Text;
                                    PassData.ExpiryDate = Convert.ToDateTime(txtExpired.Value);
                                }
                            }
                        }
                        else
                        {

                            PassData2 = new PassengerData();
                            PassData2.TransID = dataPassenger.Rows[Convert.ToInt16(index) - 1]["TransID"].ToString();
                            PassData2.RecordLocator = dataPassenger.Rows[Convert.ToInt16(index) - 1]["PNR"].ToString();
                            PassData2.PassengerID = dataPassenger.Rows[Convert.ToInt16(index) - 1]["PassengerID"].ToString();
                            PassData2.Title = "INFT";
                            PassData2.Gender = cmbGender.SelectedItem.Value.ToString();
                            PassData2.FirstName = txt_FirstName.Text;
                            PassData2.LastName = txt_LastName.Text;
                            PassData2.Nationality = cmbNation.SelectedItem.Value.ToString();
                            PassData2.IssuingCountry = cmbPassCountry.SelectedItem.Value.ToString();
                            PassData2.DOB = Convert.ToDateTime(txtDOB.Value);
                            if (IsInternationalFlight)
                            {
                                PassData2.PassportNo = txtPassportNo.Text;
                                PassData2.ExpiryDate = Convert.ToDateTime(txtExpired.Value);
                            }

                            lstPassInfantData.Add(PassData2);
                        }
                    }
                    else
                    {

                        PassData2 = new PassengerData();
                        PassData2.TransID = dataPassenger.Rows[Convert.ToInt16(index) - 1]["TransID"].ToString();
                        PassData2.RecordLocator = dataPassenger.Rows[Convert.ToInt16(index) - 1]["PNR"].ToString();
                        PassData2.PassengerID = dataPassenger.Rows[Convert.ToInt16(index) - 1]["PassengerID"].ToString();
                        PassData2.Title = "INFT";
                        PassData2.Gender = cmbGender.SelectedItem.Value.ToString();
                        PassData2.FirstName = txt_FirstName.Text;
                        PassData2.LastName = txt_LastName.Text;
                        PassData2.Nationality = cmbNation.SelectedItem.Value.ToString();
                        PassData2.IssuingCountry = cmbPassCountry.SelectedItem.Value.ToString();
                        PassData2.DOB = Convert.ToDateTime(txtDOB.Value);

                        if (IsInternationalFlight)
                        {
                            PassData2.PassportNo = txtPassportNo.Text;
                            PassData2.ExpiryDate = Convert.ToDateTime(txtExpired.Value);
                        }

                        lstPassInfantData.Add(PassData2);
                    }
                    Session["lstPassInfantData"] = lstPassInfantData;
                }
            }
        }


        [WebMethod]
        public static string DOBValidation(string DOB, string TransID)
        {
            MessageList msgList = new MessageList();
            BookingTransactionMain bookHDRInfo = new BookingTransactionMain();
            ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
            bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(TransID);

            if (DOB == null || DOB == "")
            {
                return msgList.Err300021;
            }
            else if (DOB != "" && Convert.ToDateTime(DOB).AddDays(9) > bookHDRInfo.STDDate)
            {
                return msgList.Err300022;
            }
            else if (DOB != "" && Convert.ToDateTime(DOB) >= DateTime.Today)
            {
                return msgList.Err300034;
            }
            else if (DOB != "" && Convert.ToDateTime(DOB) < DateTime.Now.AddMonths(-24))
            {
                return msgList.Err300023;
            }
            
            else
            {
                return "";
            }
        }

        [WebMethod]
        public static string ExpiryDateValidation(string ExpiryDate, string TransID)
        {
            MessageList msgList = new MessageList();
            BookingTransactionMain bookHDRInfo = new BookingTransactionMain();
            ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
            bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(TransID);

            if (ExpiryDate == null || ExpiryDate == "")
            {
                return msgList.Err300011;
            }
            else if (ExpiryDate != "" && Convert.ToDateTime(ExpiryDate) < bookHDRInfo.STDDate)
            {
                return msgList.Err300012;
            }
            else
            {
                return "";
            }
        }

        protected void gvPassenger_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            MessageList msgList = new MessageList();
            try
            {
                String Detail = "";
                if (Session["qtyMeal"] == null)
                {
                    qtyMeal = 0;
                }
                else
                {
                    qtyMeal = Convert.ToInt32(Session["qtyMeal"]);
                }
                if (Session["qtyMeal1"] == null)
                {
                    qtyMeal1 = 0;
                }
                else
                {
                    qtyMeal1 = Convert.ToInt32(Session["qtyMeal1"]);
                }
                if (Session["qtyDrink"] == null)
                {
                    qtyDrink = 0;
                }
                else
                {
                    qtyDrink = Convert.ToInt32(Session["qtyDrink"]);
                }
                if (Session["qtyDrink1"] == null)
                {
                    qtyDrink1 = 0;
                }
                if (Session["qtyBaggage"] == null)
                {
                    qtyBaggage = 0;
                }
                else
                {
                    qtyBaggage = Convert.ToInt32(Session["qtyBaggage"]);
                }
                if (Session["qtySport"] == null)
                {
                    qtySport = 0;
                }
                else
                {
                    qtySport = Convert.ToInt32(Session["qtySport"]);
                }
                if (Session["qtyComfort"] == null)
                {
                    qtyComfort = 0;
                }
                else
                {
                    qtyComfort = Convert.ToInt32(Session["qtyComfort"]);
                }
                if (Session["qtyDuty"] == null)
                {
                    qtyDuty = 0;
                }
                else
                {
                    qtyDuty = Convert.ToInt32(Session["qtyDuty"]);
                }
                DataTable dtPass = new DataTable();
                dtPass = (DataTable)Session["dtGridPass"];
                if (string.IsNullOrEmpty(e.Parameters))
                {
                    GetPassengerList(Session["TransID"].ToString(), "Depart");

                }
                else
                {
                    var args = e.Parameters.Split('|');
                    if (args[0] == "Baggage")
                    {
                        DataTable dtBaggage = (DataTable)Session["dtBaggageDepart"];
                        if (cbAllPaxBaggage1.Checked == true)
                        {
                            for (int i = 0; i <= dtPass.Rows.Count - 1; i++)
                            {
                                if (args[2].ToString() == "")
                                {
                                    //if (first == 2)
                                    //{
                                    //    DataTable dtdefault = objBooking.GetDefaultBaggage();
                                    //    if (dtdefault != null && dtdefault.Rows.Count > 0)
                                    //    {
                                    //        DataRow[] result = dtBaggage.Select("SSRCode = '" + dtdefault.Rows[0]["SYSValue"] + "'");
                                    //        foreach (DataRow row in result)
                                    //        {
                                    //            dtPass.Rows[i]["DepartBaggage"] = row[3];
                                    //            dtPass.Rows[i]["SSRCodeDepartBaggage"] = row[0];
                                    //            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                    //        }
                                    //        dtPass.Rows[i]["PriceDepartBaggage"] = Convert.ToDecimal(Detail);
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    dtPass.Rows[i]["DepartBaggage"] = string.Empty;
                                    //    dtPass.Rows[i]["SSRCodeDepartBaggage"] = string.Empty;
                                    //    dtPass.Rows[i]["PriceDepartBaggage"] = 0.00;
                                    //}
                                    gvPassenger.JSProperties["cp_result"] = msgList.Err200041;
                                    return;

                                }
                                else
                                {
                                    if (dtPass.Rows[i]["DepartBaggage"] != null && dtPass.Rows[i]["DepartBaggage"].ToString() != "")
                                    {
                                        DataRow[] results2 = dtBaggage.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in results2)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass.Rows[i]["PriceDepartBaggage"]))
                                            {
                                                gvPassenger.JSProperties["cp_result"] = msgList.Err200042;
                                                return;
                                            }
                                            else
                                            {
                                                dtPass.Rows[i]["DepartBaggage"] = args[2];
                                                dtPass.Rows[i]["SSRCodeDepartBaggage"] = args[3];
                                                dtPass.Rows[i]["PriceDepartBaggage"] = Convert.ToDecimal(Detail);
                                                dtPass.Rows[i]["PriceDepartBaggage1"] = Convert.ToDecimal(row[3]);
                                                dtPass.Rows[i]["PriceConDepartBaggage"] = Convert.ToDecimal(row[4]);
                                                dtPass.Rows[i]["IndicatorDepartBaggage"] = "1";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dtPass.Rows[i]["DepartBaggage"] = args[2];
                                        dtPass.Rows[i]["SSRCodeDepartBaggage"] = args[3];
                                        DataRow[] result2 = dtBaggage.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result2)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            dtPass.Rows[i]["PriceDepartBaggage"] = Convert.ToDecimal(Detail);
                                            dtPass.Rows[i]["PriceDepartBaggage1"] = Convert.ToDecimal(row[3]);
                                            dtPass.Rows[i]["PriceConDepartBaggage"] = Convert.ToDecimal(row[4]);
                                            dtPass.Rows[i]["IndicatorDepartBaggage"] = "1";
                                        }
                                    }

                                }
                            }
                            Session["dtGridPass"] = dtPass;
                            Session["qtyBaggage"] = 0;
                            gvPassenger.DataSource = dtPass;
                            gvPassenger.DataBind();
                        }
                        else
                        {
                            if (((Convert.ToInt32(args[1]) + qtyBaggage) - 1) <= dtPass.Rows.Count)
                            {
                                for (int i = qtyBaggage; i <= (Convert.ToInt32(args[1]) + qtyBaggage) - 1; i++)
                                {
                                    if (args[2].ToString() == "")
                                    {
                                        //if (first == 2)
                                        //{
                                        //    DataTable dtdefault = objBooking.GetDefaultBaggage();
                                        //    if (dtdefault != null && dtdefault.Rows.Count > 0)
                                        //    {
                                        //        DataRow[] result = dtBaggage.Select("SSRCode = '" + dtdefault.Rows[0]["SYSValue"] + "'");
                                        //        foreach (DataRow row in result)
                                        //        {
                                        //            dtPass.Rows[i]["DepartBaggage"] = row[3];
                                        //            dtPass.Rows[i]["SSRCodeDepartBaggage"] = row[0];
                                        //            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                        //        }
                                        //        dtPass.Rows[i]["PriceDepartBaggage"] = Convert.ToDecimal(Detail);
                                        //    }
                                        //}
                                        //else
                                        //{
                                        //    dtPass.Rows[i]["DepartBaggage"] = string.Empty;
                                        //    dtPass.Rows[i]["SSRCodeDepartBaggage"] = string.Empty;
                                        //    dtPass.Rows[i]["PriceDepartBaggage"] = 0.00;
                                        //}
                                        gvPassenger.JSProperties["cp_result"] = msgList.Err200041;
                                        return;
                                    }
                                    else
                                    {

                                        if (dtPass.Rows[i]["DepartBaggage"] != null && dtPass.Rows[i]["DepartBaggage"].ToString() != "")
                                        {
                                            DataRow[] results2 = dtBaggage.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in results2)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass.Rows[i]["PriceDepartBaggage"]))
                                                {
                                                    gvPassenger.JSProperties["cp_result"] = msgList.Err200042;
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass.Rows[i]["DepartBaggage"] = args[2];
                                                    dtPass.Rows[i]["SSRCodeDepartBaggage"] = args[3];
                                                    dtPass.Rows[i]["PriceDepartBaggage"] = Convert.ToDecimal(Detail);
                                                    dtPass.Rows[i]["PriceDepartBaggage1"] = Convert.ToDecimal(row[3]);
                                                    dtPass.Rows[i]["PriceConDepartBaggage"] = Convert.ToDecimal(row[4]);
                                                    dtPass.Rows[i]["IndicatorDepartBaggage"] = "1";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            dtPass.Rows[i]["DepartBaggage"] = args[2];
                                            dtPass.Rows[i]["SSRCodeDepartBaggage"] = args[3];
                                            DataRow[] result2 = dtBaggage.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result2)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                dtPass.Rows[i]["PriceDepartBaggage"] = Convert.ToDecimal(Detail);
                                                dtPass.Rows[i]["PriceDepartBaggage1"] = Convert.ToDecimal(row[3]);
                                                dtPass.Rows[i]["PriceConDepartBaggage"] = Convert.ToDecimal(row[4]);
                                                dtPass.Rows[i]["IndicatorDepartBaggage"] = "1";
                                            }
                                        }
                                        //dtPass.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                    }
                                }
                                Session["dtGridPass"] = dtPass;
                                if (args[2] != ",")
                                {
                                    Session["qtyBaggage"] = (Convert.ToInt32(args[1]) + qtyBaggage);
                                }
                                gvPassenger.DataSource = dtPass;
                                gvPassenger.DataBind();
                            }
                            else
                            {
                                gvPassenger.JSProperties["cp_result"] = msgList.Err200043;
                                return;
                            }
                        }

                    }
                    else if (args[0] == "Meal")
                    {
                        DataTable dtMeal = (DataTable)Session["dtMealDepart"];
                        if (cbAllPaxMeal11.Checked == true)
                        {
                            for (int i = 0; i <= dtPass.Rows.Count - 1; i++)
                            {
                                if (args[2].ToString() == ",")
                                {
                                    //dtPass.Rows[i]["DepartMeal"] = string.Empty;
                                    //dtPass.Rows[i]["SSRCodeDepartMeal"] = string.Empty;
                                    //dtPass.Rows[i]["PriceDepartMeal"] = 0.00;
                                    gvPassenger.JSProperties["cp_result"] = msgList.Err200044;
                                    return;
                                }
                                else
                                {
                                    if (dtPass.Rows[i]["DepartMeal"] != null && dtPass.Rows[i]["DepartMeal"].ToString() != "")
                                    {
                                        DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass.Rows[i]["PriceDepartMeal"]))
                                            {
                                                gvPassenger.JSProperties["cp_result"] = msgList.Err200045;
                                                return;
                                            }
                                            else
                                            {
                                                dtPass.Rows[i]["DepartMeal"] = args[2];
                                                dtPass.Rows[i]["SSRCodeDepartMeal"] = args[3];
                                                dtPass.Rows[i]["PriceDepartMeal"] = Convert.ToDecimal(Detail);
                                                dtPass.Rows[i]["IndicatorDepartMeal"] = "1";
                                            }

                                        }
                                    }
                                    else
                                    {
                                        dtPass.Rows[i]["DepartMeal"] = args[2];
                                        dtPass.Rows[i]["SSRCodeDepartMeal"] = args[3];
                                        dtPass.Rows[i]["IndicatorDepartMeal"] = "1";
                                        DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            dtPass.Rows[i]["PriceDepartMeal"] = Convert.ToDecimal(Detail);
                                        }
                                    }

                                }
                            }
                            if (Session["dtDrinkDepart"] == null)
                            {
                                Session["dtGridPass"] = dtPass;
                                Session["qtyMeal"] = 0;
                                gvPassenger.DataSource = dtPass;
                                gvPassenger.DataBind();
                            }
                        }
                        else
                        {
                            if (((Convert.ToInt32(args[1]) + qtyMeal) - 1) <= dtPass.Rows.Count)
                            {
                                for (int i = qtyMeal; i <= (Convert.ToInt32(args[1]) + qtyMeal) - 1; i++)
                                {
                                    if (args[2].ToString() == ",")
                                    {
                                        //dtPass.Rows[i]["DepartMeal"] = string.Empty;
                                        //dtPass.Rows[i]["SSRCodeDepartMeal"] = string.Empty;
                                        //dtPass.Rows[i]["PriceDepartMeal"] = 0.00;
                                        gvPassenger.JSProperties["cp_result"] = msgList.Err200044;
                                        return;
                                    }
                                    else
                                    {
                                        if (dtPass.Rows[i]["DepartMeal"] != null && dtPass.Rows[i]["DepartMeal"].ToString() != "")
                                        {
                                            DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass.Rows[i]["PriceDepartMeal"]))
                                                {
                                                    gvPassenger.JSProperties["cp_result"] = msgList.Err200045;
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass.Rows[i]["DepartMeal"] = args[2];
                                                    dtPass.Rows[i]["SSRCodeDepartMeal"] = args[3];
                                                    dtPass.Rows[i]["PriceDepartMeal"] = Convert.ToDecimal(Detail);
                                                    dtPass.Rows[i]["IndicatorDepartMeal"] = "1";
                                                }

                                            }
                                        }
                                        else
                                        {
                                            dtPass.Rows[i]["DepartMeal"] = args[2];
                                            dtPass.Rows[i]["SSRCodeDepartMeal"] = args[3];
                                            dtPass.Rows[i]["IndicatorDepartMeal"] = "1";
                                            DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                dtPass.Rows[i]["PriceDepartMeal"] = Convert.ToDecimal(Detail);
                                            }
                                        }
                                    }
                                    //dtPass.Rows[i]["PriceMeal"] = Convert.ToDecimal(Detail);
                                }
                                if (Session["dtDrinkDepart"] == null)
                                {
                                    Session["dtGridPass"] = dtPass;
                                    if (args[2] != ",")
                                    {
                                        Session["qtyMeal"] = (Convert.ToInt32(args[1]) + qtyMeal);
                                    }
                                    gvPassenger.DataSource = dtPass;
                                    gvPassenger.DataBind();
                                }
                            }
                            else
                            {
                                gvPassenger.JSProperties["cp_result"] = msgList.Err200046;
                                return;
                            }
                        }
                        if (Session["dtDrinkDepart"] != null)
                        {
                            DataTable dtDrink = (DataTable)Session["dtDrinkDepart"];
                            if (cbAllPaxMeal11.Checked == true)
                            {
                                for (int i = 0; i <= dtPass.Rows.Count - 1; i++)
                                {
                                    if (args[2].ToString() == ",")
                                    {
                                        gvPassenger.JSProperties["cp_result"] = msgList.Err200047;
                                        return;
                                    }
                                    else
                                    {

                                        // if (Session["defaultdrink"] != null)
                                        // {
                                        // DataTable DefaultDrink = (DataTable)Session["defaultdrink"];
                                        for (int c = 0; c < dtDrink.Rows.Count; c++)
                                        {
                                            //  DataRow[] result = dtDrink.Select("SSRCode LIKE '" + DefaultDrink.Rows[c]["SYSValueEx"] + "%'");
                                            //  if (result.Length > 0)
                                            //  {
                                            //  foreach (DataRow row in result)
                                            //{
                                            Detail = dtDrink.Rows[0]["Price"].ToString().Substring(0, dtDrink.Rows[0]["Price"].ToString().Length - 4);
                                            dtPass.Rows[i]["PriceDepartDrink"] = Convert.ToDecimal(Detail);
                                            dtPass.Rows[i]["DepartDrink"] = dtDrink.Rows[0]["ConcatenatedField"];
                                            dtPass.Rows[i]["SSRCodeDepartDrink"] = dtDrink.Rows[0]["SSRCode"];
                                            dtPass.Rows[i]["IndicatorDepartDrink"] = "1";
                                            // }
                                            break;
                                            // }
                                        }
                                        // }
                                    }
                                }
                                Session["dtGridPass"] = dtPass;
                                Session["qtyMeal"] = 0;
                                Session["qtyDrink"] = 0;
                                gvPassenger.DataSource = dtPass;
                                gvPassenger.DataBind();
                            }
                            else
                            {
                                if (((Convert.ToInt32(args[1]) + qtyMeal) - 1) <= dtPass.Rows.Count)
                                {
                                    for (int i = qtyMeal; i <= (Convert.ToInt32(args[1]) + qtyMeal) - 1; i++)
                                    {
                                        if (args[2].ToString() == ",")
                                        {
                                            gvPassenger.JSProperties["cp_result"] = msgList.Err200047;
                                            return;
                                        }
                                        else
                                        {
                                            // if (Session["defaultdrink"] != null)
                                            // {
                                            //DataTable DefaultDrink = (DataTable)Session["defaultdrink"];
                                            for (int c = 0; c < dtDrink.Rows.Count; c++)
                                            {
                                                //DataRow[] result = dtDrink.Select("SSRCode LIKE '" + DefaultDrink.Rows[c]["SYSValueEx"] + "%'");
                                                //if (result.Length > 0)
                                                //{
                                                //foreach (DataRow row in result)
                                                //{
                                                Detail = dtDrink.Rows[0]["Price"].ToString().Substring(0, dtDrink.Rows[0]["Price"].ToString().Length - 4);
                                                dtPass.Rows[i]["PriceDepartDrink"] = Convert.ToDecimal(Detail);
                                                dtPass.Rows[i]["DepartDrink"] = dtDrink.Rows[0]["ConcatenatedField"];
                                                dtPass.Rows[i]["SSRCodeDepartDrink"] = dtDrink.Rows[0]["SSRCode"];
                                                dtPass.Rows[i]["IndicatorDepartDrink"] = "1";
                                                //}
                                                break;
                                                //}
                                            }
                                            //}
                                        }
                                        //dtPass.Rows[i]["PriceMeal"] = Convert.ToDecimal(Detail);
                                    }
                                    Session["dtGridPass"] = dtPass;
                                    if (args[2] != ",")
                                    {
                                        Session["qtyMeal"] = (Convert.ToInt32(args[1]) + qtyMeal);
                                        Session["qtyDrink"] = (Convert.ToInt32(args[1]) + qtyDrink);
                                    }
                                    gvPassenger.DataSource = dtPass;
                                    gvPassenger.DataBind();
                                }
                                else
                                {
                                    gvPassenger.JSProperties["cp_result"] = msgList.Err200046;
                                    return;
                                }
                            }
                        }
                    }
                    else if (args[0] == "Meal1")
                    {
                        DataTable dtMeal = (DataTable)Session["dtMealDepart2"];
                        if (cbAllPaxMeal21.Checked == true)
                        {
                            for (int i = 0; i <= dtPass.Rows.Count - 1; i++)
                            {
                                if (args[2].ToString() == ",")
                                {
                                    //dtPass.Rows[i]["ConDepartMeal"] = string.Empty;
                                    //dtPass.Rows[i]["SSRCodeConDepartMeal"] = string.Empty;
                                    //dtPass.Rows[i]["PriceDepartMeal"] = 0.00;
                                    gvPassenger.JSProperties["cp_result"] = msgList.Err200048;
                                    return;
                                }
                                else
                                {
                                    if (dtPass.Rows[i]["ConDepartMeal"] != null && dtPass.Rows[i]["ConDepartMeal"].ToString() != "")
                                    {
                                        DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass.Rows[i]["PriceConDepartMeal"]))
                                            {
                                                gvPassenger.JSProperties["cp_result"] = msgList.Err200049;
                                                return;
                                            }
                                            else
                                            {
                                                dtPass.Rows[i]["ConDepartMeal"] = args[2];
                                                dtPass.Rows[i]["SSRCodeConDepartMeal"] = args[3];
                                                dtPass.Rows[i]["PriceConDepartMeal"] = Convert.ToDecimal(Detail);
                                                dtPass.Rows[i]["IndicatorConDepartMeal"] = "1";
                                            }

                                        }
                                    }
                                    else
                                    {
                                        dtPass.Rows[i]["ConDepartMeal"] = args[2];
                                        dtPass.Rows[i]["SSRCodeConDepartMeal"] = args[3];
                                        dtPass.Rows[i]["IndicatorConDepartMeal"] = "1";
                                        DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            dtPass.Rows[i]["PriceConDepartMeal"] = Convert.ToDecimal(Detail);
                                        }
                                    }
                                }
                            }
                            if (Session["dtDrinkDepart2"] == null)
                            {
                                Session["dtGridPass"] = dtPass;
                                Session["qtyMeal1"] = 0;
                                gvPassenger.DataSource = dtPass;
                                gvPassenger.DataBind();
                            }
                        }
                        else
                        {
                            if (((Convert.ToInt32(args[1]) + qtyMeal1) - 1) <= dtPass.Rows.Count)
                            {

                                for (int i = qtyMeal1; i <= (Convert.ToInt32(args[1]) + qtyMeal1) - 1; i++)
                                {
                                    if (args[2].ToString() == ",")
                                    {
                                        //dtPass.Rows[i]["ConDepartMeal"] = string.Empty;
                                        //dtPass.Rows[i]["SSRCodeConDepartMeal"] = string.Empty;
                                        //dtPass.Rows[i]["PriceConDepartMeal"] = 0.00;
                                        gvPassenger.JSProperties["cp_result"] = msgList.Err200048;
                                        return;
                                    }
                                    else
                                    {
                                        if (dtPass.Rows[i]["ConDepartMeal"] != null && dtPass.Rows[i]["ConDepartMeal"].ToString() != "")
                                        {
                                            DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass.Rows[i]["PriceConDepartMeal"]))
                                                {
                                                    gvPassenger.JSProperties["cp_result"] = msgList.Err200049;
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass.Rows[i]["ConDepartMeal"] = args[2];
                                                    dtPass.Rows[i]["SSRCodeConDepartMeal"] = args[3];
                                                    dtPass.Rows[i]["PriceConDepartMeal"] = Convert.ToDecimal(Detail);
                                                    dtPass.Rows[i]["IndicatorConDepartMeal"] = "1";
                                                }

                                            }
                                        }
                                        else
                                        {
                                            dtPass.Rows[i]["ConDepartMeal"] = args[2];
                                            dtPass.Rows[i]["SSRCodeConDepartMeal"] = args[3];
                                            dtPass.Rows[i]["IndicatorConDepartMeal"] = "1";
                                            DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                dtPass.Rows[i]["PriceConDepartMeal"] = Convert.ToDecimal(Detail);
                                            }
                                            dtPass.Rows[i]["PriceConDepartMeal"] = Convert.ToDecimal(Detail);
                                        }

                                    }
                                }
                                if (Session["dtDrinkDepart2"] == null)
                                {
                                    Session["dtGridPass"] = dtPass;
                                    if (args[2] != ",")
                                    {
                                        Session["qtyMeal1"] = (Convert.ToInt32(args[1]) + qtyMeal1);
                                    }
                                    gvPassenger.DataSource = dtPass;
                                    gvPassenger.DataBind();
                                }
                            }
                            else
                            {
                                gvPassenger.JSProperties["cp_result"] = msgList.Err200050;
                                return;
                            }
                        }
                        if (Session["dtDrinkDepart2"] != null)
                        {
                            DataTable dtDrink = (DataTable)Session["dtDrinkDepart2"];
                            if (cbAllPaxMeal21.Checked == true)
                            {
                                for (int i = 0; i <= dtPass.Rows.Count - 1; i++)
                                {
                                    if (args[2].ToString() == ",")
                                    {
                                        gvPassenger.JSProperties["cp_result"] = msgList.Err200051;
                                        return;
                                    }
                                    else
                                    {
                                        // if (Session["defaultdrink"] != null)
                                        //{
                                        //DataTable DefaultDrink = (DataTable)Session["defaultdrink"];
                                        for (int c = 0; c < dtDrink.Rows.Count; c++)
                                        {
                                            // DataRow[] result = dtDrink.Select("SSRCode LIKE '" + DefaultDrink.Rows[c]["SYSValueEx"] + "%'");
                                            //if (result.Length > 0)
                                            //{
                                            //foreach (DataRow row in result)
                                            //{
                                            Detail = dtDrink.Rows[0]["Price"].ToString().Substring(0, dtDrink.Rows[0]["Price"].ToString().Length - 4);
                                            dtPass.Rows[i]["PriceConDepartDrink"] = Convert.ToDecimal(Detail);
                                            dtPass.Rows[i]["ConDepartDrink"] = dtDrink.Rows[0]["ConcatenatedField"];
                                            dtPass.Rows[i]["SSRCodeConDepartDrink"] = dtDrink.Rows[0]["SSRCode"];
                                            dtPass.Rows[i]["IndicatorConDepartDrink"] = "1";
                                            //}
                                            break;
                                            //}
                                        }
                                        //}
                                    }
                                }
                                Session["dtGridPass"] = dtPass;
                                Session["qtyMeal1"] = 0;
                                Session["qtyDrink1"] = 0;
                                gvPassenger.DataSource = dtPass;
                                gvPassenger.DataBind();
                            }
                            else
                            {
                                if (((Convert.ToInt32(args[1]) + qtyMeal1) - 1) <= dtPass.Rows.Count)
                                {

                                    for (int i = qtyMeal1; i <= (Convert.ToInt32(args[1]) + qtyMeal1) - 1; i++)
                                    {
                                        if (args[2].ToString() == ",")
                                        {
                                            gvPassenger.JSProperties["cp_result"] = msgList.Err200051;
                                            return;
                                        }
                                        else
                                        {
                                            //if (Session["defaultdrink"] != null)
                                            //{
                                            //DataTable DefaultDrink = (DataTable)Session["defaultdrink"];
                                            for (int c = 0; c < dtDrink.Rows.Count; c++)
                                            {
                                                //DataRow[] result = dtDrink.Select("SSRCode LIKE '" + DefaultDrink.Rows[c]["SYSValueEx"] + "%'");
                                                //if (result.Length > 0)
                                                //{
                                                //foreach (DataRow row in result)
                                                //{
                                                Detail = dtDrink.Rows[0]["Price"].ToString().Substring(0, dtDrink.Rows[0]["Price"].ToString().Length - 4);
                                                dtPass.Rows[i]["PriceConDepartDrink"] = Convert.ToDecimal(Detail);
                                                dtPass.Rows[i]["ConDepartDrink"] = dtDrink.Rows[0]["ConcatenatedField"];
                                                dtPass.Rows[i]["SSRCodeConDepartDrink"] = dtDrink.Rows[0]["SSRCode"];
                                                dtPass.Rows[i]["IndicatorConDepartDrink"] = "1";
                                                //}
                                                break;
                                                //}
                                            }
                                            //}
                                        }
                                    }
                                    Session["dtGridPass"] = dtPass;
                                    if (args[2] != ",")
                                    {
                                        Session["qtyMeal1"] = (Convert.ToInt32(args[1]) + qtyMeal1);
                                        Session["qtyDrink1"] = (Convert.ToInt32(args[1]) + qtyDrink1);
                                    }
                                    gvPassenger.DataSource = dtPass;
                                    gvPassenger.DataBind();
                                }
                                else
                                {
                                    gvPassenger.JSProperties["cp_result"] = msgList.Err200050;
                                    return;
                                }
                            }
                        }
                    }
                    else if (args[0] == "Sport")
                    {
                        DataTable dtSport = (DataTable)Session["dtSportDepart"];
                        if (cbAllPaxSport1.Checked == true)
                        {
                            for (int i = 0; i <= dtPass.Rows.Count - 1; i++)
                            {
                                if (args[2].ToString() == "")
                                {
                                    //dtPass.Rows[i]["DepartSport"] = string.Empty;
                                    //dtPass.Rows[i]["SSRCodeDepartSport"] = string.Empty;
                                    //dtPass.Rows[i]["PriceDepartSport"] = 0.00;
                                    gvPassenger.JSProperties["cp_result"] = msgList.Err200052;
                                    return;
                                }
                                else
                                {
                                    if (dtPass.Rows[i]["DepartSport"] != null && dtPass.Rows[i]["DepartSport"].ToString() != "")
                                    {
                                        DataRow[] result = dtSport.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass.Rows[i]["PriceDepartSport"]))
                                            {
                                                gvPassenger.JSProperties["cp_result"] = msgList.Err200053;
                                                return;
                                            }
                                            else
                                            {
                                                dtPass.Rows[i]["DepartSport"] = args[2];
                                                dtPass.Rows[i]["SSRCodeDepartSport"] = args[3];
                                                dtPass.Rows[i]["PriceDepartSport"] = Convert.ToDecimal(Detail);
                                                dtPass.Rows[i]["PriceDepartSport1"] = Convert.ToDecimal(row[3]);
                                                dtPass.Rows[i]["PriceConDepartSport"] = Convert.ToDecimal(row[4]);
                                                dtPass.Rows[i]["IndicatorDepartSport"] = "1";
                                            }

                                        }
                                    }
                                    else
                                    {
                                        dtPass.Rows[i]["DepartSport"] = args[2];
                                        dtPass.Rows[i]["SSRCodeDepartSport"] = args[3];
                                        DataRow[] result = dtSport.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            dtPass.Rows[i]["PriceDepartSport"] = Convert.ToDecimal(Detail);
                                            dtPass.Rows[i]["PriceDepartSport1"] = Convert.ToDecimal(row[3]);
                                            dtPass.Rows[i]["PriceConDepartSport"] = Convert.ToDecimal(row[4]);
                                            dtPass.Rows[i]["IndicatorDepartSport"] = "1";
                                        }
                                    }

                                }
                            }
                            Session["dtGridPass"] = dtPass;
                            Session["qtySport"] = 0;
                            gvPassenger.DataSource = dtPass;
                            gvPassenger.DataBind();
                        }
                        else
                        {
                            if (((Convert.ToInt32(args[1]) + qtySport) - 1) <= dtPass.Rows.Count)
                            {
                                for (int i = qtySport; i <= (Convert.ToInt32(args[1]) + qtySport) - 1; i++)
                                {
                                    if (args[2].ToString() == "")
                                    {
                                        //dtPass.Rows[i]["DepartSport"] = string.Empty;
                                        //dtPass.Rows[i]["SSRCodeDepartSport"] = string.Empty;
                                        //dtPass.Rows[i]["PriceDepartSport"] = 0.00;
                                        gvPassenger.JSProperties["cp_result"] = msgList.Err200052;
                                        return;
                                    }
                                    else
                                    {
                                        if (dtPass.Rows[i]["DepartSport"] != null && dtPass.Rows[i]["DepartSport"].ToString() != "")
                                        {
                                            DataRow[] result = dtSport.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass.Rows[i]["PriceDepartSport"]))
                                                {
                                                    gvPassenger.JSProperties["cp_result"] = msgList.Err200053;
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass.Rows[i]["DepartSport"] = args[2];
                                                    dtPass.Rows[i]["SSRCodeDepartSport"] = args[3];
                                                    dtPass.Rows[i]["PriceDepartSport"] = Convert.ToDecimal(Detail);
                                                    dtPass.Rows[i]["PriceDepartSport1"] = Convert.ToDecimal(row[3]);
                                                    dtPass.Rows[i]["PriceConDepartSport"] = Convert.ToDecimal(row[4]);
                                                    dtPass.Rows[i]["IndicatorDepartSport"] = "1";
                                                }

                                            }
                                        }
                                        else
                                        {
                                            dtPass.Rows[i]["DepartSport"] = args[2];
                                            dtPass.Rows[i]["SSRCodeDepartSport"] = args[3];
                                            dtPass.Rows[i]["IndicatorDepartSport"] = "1";
                                            DataRow[] result = dtSport.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                dtPass.Rows[i]["PriceDepartSport"] = Convert.ToDecimal(Detail);
                                                dtPass.Rows[i]["PriceDepartSport1"] = Convert.ToDecimal(row[3]);
                                                dtPass.Rows[i]["PriceConDepartSport"] = Convert.ToDecimal(row[4]);

                                            }
                                        }
                                    }
                                    //dtPass.Rows[i]["PriceSport"] = Convert.ToDecimal(Detail);
                                }
                                Session["dtGridPass"] = dtPass;
                                if (args[2] != ",")
                                {
                                    Session["qtySport"] = (Convert.ToInt32(args[1]) + qtySport);
                                }
                                gvPassenger.DataSource = dtPass;
                                gvPassenger.DataBind();
                            }
                            else
                            {
                                gvPassenger.JSProperties["cp_result"] = msgList.Err200054;
                                return;
                            }
                        }
                    }
                    else if (args[0] == "Duty")
                    {
                        DataTable dtDuty = (DataTable)Session["dtDutyDepart"];
                        if (((Convert.ToInt32(args[1]) + qtyDuty) - 1) <= dtPass.Rows.Count)
                        {
                            for (int i = qtyDuty; i <= (Convert.ToInt32(args[1]) + qtyDuty) - 1; i++)
                            {
                                dtPass.Rows[i]["DepartDuty"] = args[2];
                                dtPass.Rows[i]["SSRCodeDepartDuty"] = args[3];
                                DataRow[] result = dtDuty.Select("SSRCode = '" + args[3] + "'");
                                foreach (DataRow row in result)
                                {
                                    Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                    dtPass.Rows[i]["PriceDepartDuty"] = Convert.ToDecimal(Detail);
                                    dtPass.Rows[i]["PriceDepartDuty1"] = Convert.ToDecimal(row[3]);
                                    dtPass.Rows[i]["PriceConDepartDuty"] = Convert.ToDecimal(row[4]);
                                }
                                //dtPass.Rows[i]["PriceDuty"] = Convert.ToDecimal(Detail);
                            }
                            Session["dtGridPass"] = dtPass;
                            Session["qtyDuty"] = (Convert.ToInt32(args[1]) + qtySport);
                            gvPassenger.DataSource = dtPass;
                            gvPassenger.DataBind();
                        }
                        else
                        {
                            gvPassenger.JSProperties["cp_result"] = msgList.Err200055;
                            return;

                        }
                    }
                    else if (args[0] == "Comfort")
                    {
                        DataTable dtComfort = (DataTable)Session["dtComfortDepart"];
                        if (cbAllPaxComfort1.Checked == true)
                        {
                            for (int i = 0; i <= dtPass.Rows.Count - 1; i++)
                            {
                                if (args[2].ToString() == ",")
                                {
                                    //dtPass.Rows[i]["DepartComfort"] = string.Empty;
                                    //dtPass.Rows[i]["SSRCodeDepartComfort"] = string.Empty;
                                    //dtPass.Rows[i]["PriceDepartComfort"] = 0.00;
                                    gvPassenger.JSProperties["cp_result"] = msgList.Err200056;
                                    return;
                                }
                                else
                                {
                                    if (dtPass.Rows[i]["DepartComfort"] != null && dtPass.Rows[i]["DepartComfort"].ToString() != "")
                                    {
                                        DataRow[] result = dtComfort.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass.Rows[i]["PriceDepartComfort"]))
                                            {
                                                gvPassenger.JSProperties["cp_result"] = msgList.Err200057;
                                                return;
                                            }
                                            else
                                            {
                                                dtPass.Rows[i]["DepartComfort"] = args[2];
                                                dtPass.Rows[i]["SSRCodeDepartComfort"] = args[3];
                                                dtPass.Rows[i]["PriceDepartComfort"] = Convert.ToDecimal(Detail);
                                                dtPass.Rows[i]["PriceDepartComfort1"] = Convert.ToDecimal(row[3]);
                                                dtPass.Rows[i]["PriceConDepartComfort"] = Convert.ToDecimal(row[4]);
                                                dtPass.Rows[i]["IndicatorDepartComfort"] = "1";
                                            }

                                        }
                                    }
                                    else
                                    {
                                        dtPass.Rows[i]["DepartComfort"] = args[2];
                                        dtPass.Rows[i]["SSRCodeDepartComfort"] = args[3];
                                        DataRow[] result = dtComfort.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            dtPass.Rows[i]["PriceDepartComfort"] = Convert.ToDecimal(Detail);
                                            dtPass.Rows[i]["PriceDepartComfort1"] = Convert.ToDecimal(row[3]);
                                            dtPass.Rows[i]["PriceConDepartComfort"] = Convert.ToDecimal(row[4]);
                                            dtPass.Rows[i]["IndicatorDepartComfort"] = "1";
                                        }
                                    }
                                }
                            }
                            Session["dtGridPass"] = dtPass;
                            Session["qtyComfort"] = 0;
                            gvPassenger.DataSource = dtPass;
                            gvPassenger.DataBind();
                        }
                        else
                        {
                            if (((Convert.ToInt32(args[1]) + qtyComfort) - 1) <= dtPass.Rows.Count)
                            {
                                for (int i = qtyComfort; i <= (Convert.ToInt32(args[1]) + qtyComfort) - 1; i++)
                                {
                                    if (args[2].ToString() == ",")
                                    {
                                        //dtPass.Rows[i]["DepartComfort"] = string.Empty;
                                        //dtPass.Rows[i]["SSRCodeDepartComfort"] = string.Empty;
                                        //dtPass.Rows[i]["PriceDepartComfort"] = 0.00;
                                        gvPassenger.JSProperties["cp_result"] = msgList.Err200056;
                                        return;
                                    }
                                    else
                                    {
                                        if (dtPass.Rows[i]["DepartComfort"] != null && dtPass.Rows[i]["DepartComfort"].ToString() != "")
                                        {
                                            DataRow[] result = dtComfort.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass.Rows[i]["PriceDepartComfort"]))
                                                {
                                                    gvPassenger.JSProperties["cp_result"] = msgList.Err200057;
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass.Rows[i]["DepartComfort"] = args[2];
                                                    dtPass.Rows[i]["SSRCodeDepartComfort"] = args[3];
                                                    dtPass.Rows[i]["PriceDepartComfort"] = Convert.ToDecimal(Detail);
                                                    dtPass.Rows[i]["PriceDepartComfort1"] = Convert.ToDecimal(row[3]);
                                                    dtPass.Rows[i]["PriceConDepartComfort"] = Convert.ToDecimal(row[4]);
                                                    dtPass.Rows[i]["IndicatorDepartComfort"] = "1";
                                                }

                                            }
                                        }
                                        else
                                        {
                                            dtPass.Rows[i]["DepartComfort"] = args[2];
                                            dtPass.Rows[i]["SSRCodeDepartComfort"] = args[3];
                                            DataRow[] result = dtComfort.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                dtPass.Rows[i]["PriceDepartComfort"] = Convert.ToDecimal(Detail);
                                                dtPass.Rows[i]["PriceDepartComfort1"] = Convert.ToDecimal(row[3]);
                                                dtPass.Rows[i]["PriceConDepartComfort"] = Convert.ToDecimal(row[4]);
                                                dtPass.Rows[i]["IndicatorDepartComfort"] = "1";
                                            }
                                        }
                                        //dtPass.Rows[i]["PriceComfort"] = Convert.ToDecimal(Detail);
                                    }
                                }
                                Session["dtGridPass"] = dtPass;
                                if (args[2] != ",")
                                {
                                    Session["qtyComfort"] = (Convert.ToInt32(args[1]) + qtyComfort);
                                }
                                gvPassenger.DataSource = dtPass;
                                gvPassenger.DataBind();
                            }
                            else
                            {
                                gvPassenger.JSProperties["cp_result"] = msgList.Err200057;
                                return;
                            }
                        }
                    }
                    else if (args[0] == "Infant")
                    {
                        if (ASPxEdit.AreEditorsValid(callbackPanel))
                        {
                            string index = hfIndex.Value;
                            DataTable dataPassenger = new DataTable();
                            dataPassenger = (DataTable)HttpContext.Current.Session["dtGridPass"];
                            if (Session["lstPassInfantData"] != null)
                            {
                                lstPassInfantData = (List<PassengerData>)Session["lstPassInfantData"];
                                List<PassengerData> NewlstPassInfantData = lstPassInfantData.Where(item => item.PassengerID == (Convert.ToInt16(index) - 1).ToString()).ToList();
                                if (NewlstPassInfantData.Count > 0)
                                {
                                    foreach (PassengerData PassData in NewlstPassInfantData)
                                    {
                                        PassData.TransID = dataPassenger.Rows[Convert.ToInt16(index) - 1]["TransID"].ToString();
                                        PassData.RecordLocator = dataPassenger.Rows[Convert.ToInt16(index) - 1]["PNR"].ToString();
                                        PassData.PassengerID = dataPassenger.Rows[Convert.ToInt16(index) - 1]["PassengerID"].ToString();
                                        PassData.Title = "INFT";
                                        PassData.Gender = cmbGender.SelectedItem.Value.ToString();
                                        PassData.FirstName = txt_FirstName.Text;
                                        PassData.LastName = txt_LastName.Text;
                                        PassData.Nationality = cmbNation.SelectedItem.Value.ToString();
                                        PassData.IssuingCountry = cmbPassCountry.SelectedItem.Value.ToString();
                                        PassData.DOB = Convert.ToDateTime(txtDOB.Value);

                                        if (IsInternationalFlight)
                                        {
                                            PassData.PassportNo = txtPassportNo.Text;
                                            PassData.ExpiryDate = Convert.ToDateTime(txtExpired.Value);
                                        }
                                    }
                                }
                                else
                                {

                                    PassData2 = new PassengerData();
                                    PassData2.TransID = dataPassenger.Rows[Convert.ToInt16(index) - 1]["TransID"].ToString();
                                    PassData2.RecordLocator = dataPassenger.Rows[Convert.ToInt16(index) - 1]["PNR"].ToString();
                                    PassData2.PassengerID = dataPassenger.Rows[Convert.ToInt16(index) - 1]["PassengerID"].ToString();
                                    PassData2.Title = "INFT";
                                    PassData2.Gender = cmbGender.SelectedItem.Value.ToString();
                                    PassData2.FirstName = txt_FirstName.Text;
                                    PassData2.LastName = txt_LastName.Text;
                                    PassData2.Nationality = cmbNation.SelectedItem.Value.ToString();
                                    PassData2.IssuingCountry = cmbPassCountry.SelectedItem.Value.ToString();
                                    PassData2.DOB = Convert.ToDateTime(txtDOB.Value);
                                    if (IsInternationalFlight)
                                    {
                                        PassData2.PassportNo = txtPassportNo.Text;
                                        PassData2.ExpiryDate = Convert.ToDateTime(txtExpired.Value);
                                    }

                                    lstPassInfantData.Add(PassData2);
                                }
                            }
                            else
                            {

                                PassData2 = new PassengerData();
                                PassData2.TransID = dataPassenger.Rows[Convert.ToInt16(index) - 1]["TransID"].ToString();
                                PassData2.RecordLocator = dataPassenger.Rows[Convert.ToInt16(index) - 1]["PNR"].ToString();
                                PassData2.PassengerID = dataPassenger.Rows[Convert.ToInt16(index) - 1]["PassengerID"].ToString();
                                PassData2.Title = "INFT";
                                PassData2.Gender = cmbGender.SelectedItem.Value.ToString();
                                PassData2.FirstName = txt_FirstName.Text;
                                PassData2.LastName = txt_LastName.Text;
                                PassData2.Nationality = cmbNation.SelectedItem.Value.ToString();
                                PassData2.IssuingCountry = cmbPassCountry.SelectedItem.Value.ToString();
                                PassData2.DOB = Convert.ToDateTime(txtDOB.Value);

                                if (IsInternationalFlight)
                                {
                                    PassData2.PassportNo = txtPassportNo.Text;
                                    PassData2.ExpiryDate = Convert.ToDateTime(txtExpired.Value);
                                }

                                lstPassInfantData.Add(PassData2);
                            }
                            Session["lstPassInfantData"] = lstPassInfantData;
                        }
                        gvPassenger.JSProperties["cp_result"] = "Infant";
                    }
                    else if (args[0] == "Clear")
                    {
                        dtPass.Rows[Convert.ToInt16(args[1])]["DepartBaggage"] = string.Empty;
                        dtPass.Rows[Convert.ToInt16(args[1])]["SSRCodeDepartBaggage"] = string.Empty;
                        dtPass.Rows[Convert.ToInt16(args[1])]["PriceDepartBaggage"] = 0.00;

                        dtPass.Rows[Convert.ToInt16(args[1])]["DepartMeal"] = string.Empty;
                        dtPass.Rows[Convert.ToInt16(args[1])]["SSRCodeDepartMeal"] = string.Empty;
                        dtPass.Rows[Convert.ToInt16(args[1])]["PriceDepartMeal"] = 0.00;

                        dtPass.Rows[Convert.ToInt16(args[1])]["ConDepartMeal"] = string.Empty;
                        dtPass.Rows[Convert.ToInt16(args[1])]["SSRCodeConDepartMeal"] = string.Empty;
                        dtPass.Rows[Convert.ToInt16(args[1])]["PriceConDepartMeal"] = 0.00;

                        dtPass.Rows[Convert.ToInt16(args[1])]["DepartSport"] = string.Empty;
                        dtPass.Rows[Convert.ToInt16(args[1])]["SSRCodeDepartSport"] = string.Empty;
                        dtPass.Rows[Convert.ToInt16(args[1])]["PriceDepartSport"] = 0.00;

                        dtPass.Rows[Convert.ToInt16(args[1])]["DepartDuty"] = string.Empty;
                        dtPass.Rows[Convert.ToInt16(args[1])]["SSRCodeDepartDuty"] = string.Empty;
                        dtPass.Rows[Convert.ToInt16(args[1])]["PriceDepartDuty"] = 0.00;

                        dtPass.Rows[Convert.ToInt16(args[1])]["DepartComfort"] = string.Empty;
                        dtPass.Rows[Convert.ToInt16(args[1])]["SSRCodeDepartComfort"] = string.Empty;
                        dtPass.Rows[Convert.ToInt16(args[1])]["PriceDepartComfort"] = 0.00;

                        Session["dtGridPass"] = dtPass;
                        gvPassenger.DataSource = dtPass;
                        gvPassenger.DataBind();

                        Session["qtyBaggage"] = Convert.ToInt32(Session["qtyBaggage"]) - 1;
                        Session["qtyMeal"] = Convert.ToInt32(Session["qtyMeal"]) - 1;
                        Session["qtyMeal1"] = Convert.ToInt32(Session["qtyMeal1"]) - 1;
                        Session["qtySport"] = Convert.ToInt32(Session["qtySport"]) - 1;
                        Session["qtyComfort"] = Convert.ToInt32(Session["qtyComfort"]) - 1;
                        Session["qtyDrink"] = Convert.ToInt32(Session["qtyDrink"]) - 1;
                        Session["qtyDrink1"] = Convert.ToInt32(Session["qtyDrink1"]) - 1;
                    }

                }

                //GetPassengerList(TransID, "Depart");
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void gvPassenger2_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            MessageList msgList = new MessageList();
            try
            {
                String Detail = "";
                if (Session["qtyMeal2"] == null)
                {
                    qtyMeal2 = 0;
                }
                else
                {
                    qtyMeal2 = Convert.ToInt32(Session["qtyMeal2"]);
                }
                if (Session["qtyMeal21"] == null)
                {
                    qtyMeal21 = 0;
                }
                else
                {
                    qtyMeal21 = Convert.ToInt32(Session["qtyMeal21"]);
                }
                if (Session["qtyDrink2"] == null)
                {
                    qtyDrink2 = 0;
                }
                else
                {
                    qtyDrink2 = Convert.ToInt32(Session["qtyDrink2"]);
                }
                if (Session["qtyDrink21"] == null)
                {
                    qtyDrink21 = 0;
                }
                else
                {
                    qtyDrink21 = Convert.ToInt32(Session["qtyDrink21"]);
                }
                if (Session["qtyBaggage2"] == null)
                {
                    qtyBaggage2 = 0;
                }
                else
                {
                    qtyBaggage2 = Convert.ToInt32(Session["qtyBaggage2"]);
                }
                if (Session["qtySport2"] == null)
                {
                    qtySport2 = 0;
                }
                else
                {
                    qtySport2 = Convert.ToInt32(Session["qtySport2"]);
                }
                if (Session["qtyComfort2"] == null)
                {
                    qtyComfort2 = 0;
                }
                else
                {
                    qtyComfort2 = Convert.ToInt32(Session["qtyComfort2"]);
                }
                if (Session["qtyDuty2"] == null)
                {
                    qtyDuty2 = 0;
                }
                else
                {
                    qtyDuty2 = Convert.ToInt32(Session["qtyDuty2"]);
                }
                DataTable dtPass2 = new DataTable();
                dtPass2 = (DataTable)Session["dtGridPass2"];
                if (string.IsNullOrEmpty(e.Parameters))
                {
                    GetPassengerList(Session["TransID"].ToString(), "Return");
                }
                else
                {
                    var args = e.Parameters.Split('|');
                    if (args[0] == "Baggage")
                    {
                        DataTable dtBaggage = (DataTable)Session["dtBaggageReturn"];
                        if (cbAllBaggage2.Checked == true)
                        {
                            for (int i = 0; i <= dtPass2.Rows.Count - 1; i++)
                            {
                                if (args[2].ToString() == "")
                                {
                                    //if (first == 2)
                                    //{
                                    //    DataTable dtdefault = objBooking.GetDefaultBaggage();
                                    //    if (dtdefault != null && dtdefault.Rows.Count > 0)
                                    //    {
                                    //        DataRow[] result = dtBaggage.Select("SSRCode = '" + dtdefault.Rows[0]["SYSValue"] + "'");
                                    //        foreach (DataRow row in result)
                                    //        {
                                    //            dtPass2.Rows[i]["ReturnBaggage"] = row[3];
                                    //            dtPass2.Rows[i]["SSRCodeReturnBaggage"] = row[0];
                                    //            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                    //        }
                                    //        dtPass2.Rows[i]["PriceReturnBaggage"] = Convert.ToDecimal(Detail);
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    dtPass2.Rows[i]["ReturnBaggage"] = string.Empty;
                                    //    dtPass2.Rows[i]["SSRCodeReturnBaggage"] = string.Empty;
                                    //    dtPass2.Rows[i]["PriceReturnBaggage"] = 0.00;
                                    //}
                                    gvPassenger2.JSProperties["cp_result"] = msgList.Err200041;
                                    return;
                                }
                                else
                                {
                                    if (dtPass2.Rows[i]["ReturnBaggage"] != null && dtPass2.Rows[i]["ReturnBaggage"].ToString() != "")
                                    {
                                        DataRow[] results2 = dtBaggage.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in results2)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass2.Rows[i]["PriceReturnBaggage"]))
                                            {
                                                gvPassenger2.JSProperties["cp_result"] = msgList.Err200042;
                                                return;
                                            }
                                            else
                                            {
                                                dtPass2.Rows[i]["ReturnBaggage"] = args[2];
                                                dtPass2.Rows[i]["SSRCodeReturnBaggage"] = args[3];
                                                dtPass2.Rows[i]["PriceReturnBaggage"] = Convert.ToDecimal(Detail);
                                                dtPass2.Rows[i]["PriceReturnBaggage1"] = Convert.ToDecimal(row[3]);
                                                dtPass2.Rows[i]["PriceConReturnBaggage"] = Convert.ToDecimal(row[4]);
                                                dtPass2.Rows[i]["IndicatorReturnBaggage"] = "1";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dtPass2.Rows[i]["ReturnBaggage"] = args[2];
                                        dtPass2.Rows[i]["SSRCodeReturnBaggage"] = args[3];
                                        dtPass2.Rows[i]["IndicatorReturnBaggage"] = "1";
                                        DataRow[] result2 = dtBaggage.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result2)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            dtPass2.Rows[i]["PriceReturnBaggage"] = Convert.ToDecimal(Detail);
                                            dtPass2.Rows[i]["PriceReturnBaggage1"] = Convert.ToDecimal(row[3]);
                                            dtPass2.Rows[i]["PriceConReturnBaggage"] = Convert.ToDecimal(row[4]);
                                        }
                                    }
                                }
                            }
                            Session["dtGridPass2"] = dtPass2;
                            Session["qtyBaggage2"] = 0;
                            gvPassenger2.DataSource = dtPass2;
                            gvPassenger2.DataBind();
                        }
                        else
                        {
                            if (((Convert.ToInt32(args[1]) + qtyBaggage2) - 1) <= dtPass2.Rows.Count)
                            {
                                for (int i = qtyBaggage2; i <= (Convert.ToInt32(args[1]) + qtyBaggage2) - 1; i++)
                                {
                                    if (args[2].ToString() == "")
                                    {
                                        //if (first == 2)
                                        //{
                                        //    DataTable dtdefault = objBooking.GetDefaultBaggage();
                                        //    if (dtdefault != null && dtdefault.Rows.Count > 0)
                                        //    {
                                        //        DataRow[] result = dtBaggage.Select("SSRCode = '" + dtdefault.Rows[0]["SYSValue"] + "'");
                                        //        foreach (DataRow row in result)
                                        //        {
                                        //            dtPass2.Rows[i]["ReturnBaggage"] = row[3];
                                        //            dtPass2.Rows[i]["SSRCodeReturnBaggage"] = row[0];
                                        //            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                        //        }
                                        //        dtPass2.Rows[i]["PriceReturnBaggage"] = Convert.ToDecimal(Detail);
                                        //    }
                                        //}
                                        //else
                                        //{
                                        //    dtPass2.Rows[i]["ReturnBaggage"] = string.Empty;
                                        //    dtPass2.Rows[i]["SSRCodeReturnBaggage"] = string.Empty;
                                        //    dtPass2.Rows[i]["PriceReturnBaggage"] = 0.00;
                                        //}
                                        gvPassenger2.JSProperties["cp_result"] = msgList.Err200041;
                                        return;
                                    }
                                    else
                                    {
                                        if (dtPass2.Rows[i]["ReturnBaggage"] != null && dtPass2.Rows[i]["ReturnBaggage"].ToString() != "")
                                        {
                                            DataRow[] results2 = dtBaggage.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in results2)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass2.Rows[i]["PriceReturnBaggage"]))
                                                {
                                                    gvPassenger2.JSProperties["cp_result"] = msgList.Err200042;
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass2.Rows[i]["ReturnBaggage"] = args[2];
                                                    dtPass2.Rows[i]["SSRCodeReturnBaggage"] = args[3];
                                                    dtPass2.Rows[i]["PriceReturnBaggage"] = Convert.ToDecimal(Detail);
                                                    dtPass2.Rows[i]["PriceReturnBaggage1"] = Convert.ToDecimal(row[3]);
                                                    dtPass2.Rows[i]["PriceConReturnBaggage"] = Convert.ToDecimal(row[4]);
                                                    dtPass2.Rows[i]["IndicatorReturnBaggage"] = "1";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            dtPass2.Rows[i]["ReturnBaggage"] = args[2];
                                            dtPass2.Rows[i]["SSRCodeReturnBaggage"] = args[3];
                                            dtPass2.Rows[i]["IndicatorReturnBaggage"] = "1";
                                            DataRow[] result = dtBaggage.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                dtPass2.Rows[i]["PriceReturnBaggage"] = Convert.ToDecimal(Detail);
                                                dtPass2.Rows[i]["PriceReturnBaggage1"] = Convert.ToDecimal(row[3]);
                                                dtPass2.Rows[i]["PriceConReturnBaggage"] = Convert.ToDecimal(row[4]);
                                            }
                                            //dtPass2.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                        }
                                    }
                                }
                                Session["dtGridPass2"] = dtPass2;
                                if (args[2] != ",")
                                {
                                    Session["qtyBaggage2"] = (Convert.ToInt32(args[1]) + qtyBaggage2);
                                }
                                gvPassenger2.DataSource = dtPass2;
                                gvPassenger2.DataBind();
                            }
                            else
                            {
                                gvPassenger2.JSProperties["cp_result"] = msgList.Err200043;
                                return;
                            }
                        }
                    }
                    else if (args[0] == "Meal")
                    {
                        DataTable dtMeal = (DataTable)Session["dtMealReturn"];
                        if (cbAllPaxMeal12.Checked == true)
                        {
                            for (int i = 0; i <= dtPass2.Rows.Count - 1; i++)
                            {
                                if (args[2].ToString() == ",")
                                {
                                    //dtPass2.Rows[i]["ReturnMeal"] = string.Empty;
                                    //dtPass2.Rows[i]["SSRCodeReturnMeal"] = string.Empty;
                                    //dtPass2.Rows[i]["PriceReturnMeal"] = 0.00;
                                    gvPassenger2.JSProperties["cp_result"] = msgList.Err200044;
                                    return;
                                }
                                else
                                {
                                    if (dtPass2.Rows[i]["ReturnMeal"] != null && dtPass2.Rows[i]["ReturnMeal"].ToString() != "")
                                    {
                                        DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass2.Rows[i]["PriceReturnMeal"]))
                                            {
                                                gvPassenger2.JSProperties["cp_result"] = msgList.Err200045;
                                                return;
                                            }
                                            else
                                            {
                                                dtPass2.Rows[i]["ReturnMeal"] = args[2];
                                                dtPass2.Rows[i]["SSRCodeReturnMeal"] = args[3];
                                                dtPass2.Rows[i]["PriceReturnMeal"] = Convert.ToDecimal(Detail);
                                                dtPass2.Rows[i]["IndicatorReturnMeal"] = "1";

                                            }
                                        }
                                    }
                                    else
                                    {
                                        dtPass2.Rows[i]["ReturnMeal"] = args[2];
                                        dtPass2.Rows[i]["SSRCodeReturnMeal"] = args[3];
                                        dtPass2.Rows[i]["IndicatorReturnMeal"] = "1";
                                        DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            dtPass2.Rows[i]["PriceReturnMeal"] = Convert.ToDecimal(Detail);
                                        }
                                    }
                                }
                            }
                            if (Session["dtDrinkReturn"] == null)
                            {
                                Session["dtGridPass2"] = dtPass2;
                                Session["qtyMeal2"] = 0;
                                gvPassenger2.DataSource = dtPass2;
                                gvPassenger2.DataBind();
                            }
                        }
                        else
                        {
                            if (((Convert.ToInt32(args[1]) + qtyMeal2) - 1) <= dtPass2.Rows.Count)
                            {
                                for (int i = qtyMeal2; i <= (Convert.ToInt32(args[1]) + qtyMeal2) - 1; i++)
                                {
                                    if (args[2].ToString() == ",")
                                    {
                                        //dtPass2.Rows[i]["ReturnMeal"] = string.Empty;
                                        //dtPass2.Rows[i]["SSRCodeReturnMeal"] = string.Empty;
                                        //dtPass2.Rows[i]["PriceReturnMeal"] = 0.00;
                                        gvPassenger2.JSProperties["cp_result"] = msgList.Err200044;
                                        return;
                                    }
                                    else
                                    {
                                        if (dtPass2.Rows[i]["ReturnMeal"] != null && dtPass2.Rows[i]["ReturnMeal"].ToString() != "")
                                        {
                                            DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass2.Rows[i]["PriceReturnMeal"]))
                                                {
                                                    gvPassenger2.JSProperties["cp_result"] = msgList.Err200045;
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass2.Rows[i]["ReturnMeal"] = args[2];
                                                    dtPass2.Rows[i]["SSRCodeReturnMeal"] = args[3];
                                                    dtPass2.Rows[i]["PriceReturnMeal"] = Convert.ToDecimal(Detail);
                                                    dtPass2.Rows[i]["IndicatorReturnMeal"] = "1";

                                                }
                                            }
                                        }
                                        else
                                        {
                                            dtPass2.Rows[i]["ReturnMeal"] = args[2];
                                            dtPass2.Rows[i]["SSRCodeReturnMeal"] = args[3];
                                            dtPass2.Rows[i]["IndicatorReturnMeal"] = "1";
                                            DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                dtPass2.Rows[i]["PriceReturnMeal"] = Convert.ToDecimal(Detail);
                                            }
                                            //dtPass2.Rows[i]["PriceMeal"] = Convert.ToDecimal(Detail);
                                        }
                                    }
                                }
                                if (Session["dtDrinkReturn"] == null)
                                {
                                    Session["dtGridPass2"] = dtPass2;
                                    if (args[2] != ",")
                                    {
                                        Session["qtyMeal2"] = (Convert.ToInt32(args[1]) + qtyMeal2);
                                    }
                                    gvPassenger2.DataSource = dtPass2;
                                    gvPassenger2.DataBind();
                                }
                            }
                            else
                            {
                                gvPassenger2.JSProperties["cp_result"] = msgList.Err200046;
                                return;
                            }
                        }
                        if (Session["dtDrinkReturn"] != null)
                        {
                            DataTable dtDrink = (DataTable)Session["dtDrinkReturn"];
                            if (cbAllPaxMeal12.Checked == true)
                            {
                                for (int i = 0; i <= dtPass2.Rows.Count - 1; i++)
                                {
                                    if (args[2].ToString() == ",")
                                    {
                                        gvPassenger2.JSProperties["cp_result"] = msgList.Err200047;
                                        return;
                                    }
                                    else
                                    {

                                        // if (Session["defaultdrink"] != null)
                                        // {
                                        // DataTable DefaultDrink = (DataTable)Session["defaultdrink"];
                                        for (int c = 0; c < dtDrink.Rows.Count; c++)
                                        {
                                            // DataRow[] result = dtDrink.Select("SSRCode LIKE '" + DefaultDrink.Rows[c]["SYSValueEx"] + "%'");
                                            // if (result.Length > 0)
                                            // {
                                            //foreach (DataRow row in result)
                                            //{
                                            Detail = dtDrink.Rows[0]["Price"].ToString().Substring(0, dtDrink.Rows[0]["Price"].ToString().Length - 4);
                                            dtPass2.Rows[i]["PriceReturnDrink"] = Convert.ToDecimal(Detail);
                                            dtPass2.Rows[i]["ReturnDrink"] = dtDrink.Rows[0]["ConcatenatedField"];
                                            dtPass2.Rows[i]["SSRCodeReturnDrink"] = dtDrink.Rows[0]["SSRCode"];
                                            dtPass2.Rows[i]["IndicatorReturnDrink"] = "1";
                                            //}
                                            break;
                                            //}
                                        }
                                        // }
                                    }
                                }
                                Session["dtGridPass2"] = dtPass2;
                                Session["qtyMeal2"] = 0;
                                Session["qtyDrink2"] = 0;
                                gvPassenger2.DataSource = dtPass2;
                                gvPassenger2.DataBind();
                            }
                            else
                            {
                                if (((Convert.ToInt32(args[1]) + qtyMeal2) - 1) <= dtPass2.Rows.Count)
                                {
                                    for (int i = qtyMeal2; i <= (Convert.ToInt32(args[1]) + qtyMeal2) - 1; i++)
                                    {
                                        if (args[2].ToString() == ",")
                                        {
                                            gvPassenger2.JSProperties["cp_result"] = msgList.Err200047;
                                            return;
                                        }
                                        else
                                        {
                                            //if (Session["defaultdrink"] != null)
                                            //{
                                            //  DataTable DefaultDrink = (DataTable)Session["defaultdrink"];
                                            for (int c = 0; c < dtDrink.Rows.Count; c++)
                                            {
                                                // DataRow[] result = dtDrink.Select("SSRCode LIKE '" + DefaultDrink.Rows[c]["SYSValueEx"] + "%'");
                                                // if (result.Length > 0)
                                                //{
                                                // foreach (DataRow row in result)
                                                // {
                                                Detail = dtDrink.Rows[0]["Price"].ToString().Substring(0, dtDrink.Rows[0]["Price"].ToString().Length - 4);
                                                dtPass2.Rows[i]["PriceReturnDrink"] = Convert.ToDecimal(Detail);
                                                dtPass2.Rows[i]["ReturnDrink"] = dtDrink.Rows[0]["ConcatenatedField"];
                                                dtPass2.Rows[i]["SSRCodeReturnDrink"] = dtDrink.Rows[0]["SSRCode"];
                                                dtPass2.Rows[i]["IndicatorReturnDrink"] = "1";
                                                // }
                                                break;
                                                // }
                                            }
                                            //}
                                            //dtPass2.Rows[i]["PriceMeal"] = Convert.ToDecimal(Detail);
                                        }
                                    }
                                    Session["dtGridPass2"] = dtPass2;
                                    if (args[2] != ",")
                                    {
                                        Session["qtyMeal2"] = (Convert.ToInt32(args[1]) + qtyMeal2);
                                        Session["qtyDrink2"] = (Convert.ToInt32(args[1]) + qtyDrink2);
                                    }
                                    gvPassenger2.DataSource = dtPass2;
                                    gvPassenger2.DataBind();
                                }
                                else
                                {
                                    gvPassenger2.JSProperties["cp_result"] = msgList.Err200046;
                                    return;
                                }
                            }
                        }
                    }
                    else if (args[0] == "Meal1")
                    {
                        DataTable dtMeal = (DataTable)Session["dtMealReturn2"];
                        if (cbAllPaxMeal22.Checked == true)
                        {
                            for (int i = 0; i <= dtPass2.Rows.Count - 1; i++)
                            {
                                if (args[2].ToString() == ",")
                                {
                                    //dtPass2.Rows[i]["ConReturnMeal"] = string.Empty;
                                    //dtPass2.Rows[i]["SSRCodeConReturnMeal"] = string.Empty;
                                    //dtPass2.Rows[i]["PriceConReturnMeal"] = 0.00;
                                    gvPassenger2.JSProperties["cp_result"] = msgList.Err200048;
                                    return;
                                }
                                else
                                {
                                    if (dtPass2.Rows[i]["ConReturnMeal"] != null && dtPass2.Rows[i]["ConReturnMeal"].ToString() != "")
                                    {
                                        DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass2.Rows[i]["PriceConReturnMeal"]))
                                            {
                                                gvPassenger2.JSProperties["cp_result"] = msgList.Err200049;
                                                return;
                                            }
                                            else
                                            {
                                                dtPass2.Rows[i]["ConReturnMeal"] = args[2];
                                                dtPass2.Rows[i]["SSRCodeConReturnMeal"] = args[3];
                                                dtPass2.Rows[i]["PriceConReturnMeal"] = Convert.ToDecimal(Detail);
                                                dtPass2.Rows[i]["IndicatorConReturnMeal"] = "1";

                                            }
                                        }
                                    }
                                    else
                                    {
                                        dtPass2.Rows[i]["ConReturnMeal"] = args[2];
                                        dtPass2.Rows[i]["SSRCodeConReturnMeal"] = args[3];
                                        dtPass2.Rows[i]["IndicatorConReturnMeal"] = "1";
                                        DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            dtPass2.Rows[i]["PriceConReturnMeal"] = Convert.ToDecimal(Detail);
                                        }
                                    }
                                }
                            }
                            if (Session["dtDrinkReturn2"] == null)
                            {
                                Session["dtGridPass2"] = dtPass2;
                                Session["qtyMeal21"] = 0;
                                gvPassenger2.DataSource = dtPass2;
                                gvPassenger2.DataBind();
                            }
                        }
                        else
                        {
                            if (((Convert.ToInt32(args[1]) + qtyMeal21) - 1) <= dtPass2.Rows.Count)
                            {
                                for (int i = qtyMeal21; i <= (Convert.ToInt32(args[1]) + qtyMeal21) - 1; i++)
                                {
                                    if (args[2].ToString() == ",")
                                    {
                                        //dtPass2.Rows[i]["ConReturnMeal"] = string.Empty;
                                        //dtPass2.Rows[i]["SSRCodeConReturnMeal"] = string.Empty;
                                        //dtPass2.Rows[i]["PriceConReturnMeal"] = 0.00;
                                        gvPassenger2.JSProperties["cp_result"] = msgList.Err200048;
                                        return;
                                    }
                                    else
                                    {
                                        if (dtPass2.Rows[i]["ConReturnMeal"] != null && dtPass2.Rows[i]["ConReturnMeal"].ToString() != "")
                                        {
                                            DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass2.Rows[i]["PriceConReturnMeal"]))
                                                {
                                                    gvPassenger2.JSProperties["cp_result"] = msgList.Err200049;
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass2.Rows[i]["ConReturnMeal"] = args[2];
                                                    dtPass2.Rows[i]["SSRCodeConReturnMeal"] = args[3];
                                                    dtPass2.Rows[i]["PriceConReturnMeal"] = Convert.ToDecimal(Detail);
                                                    dtPass2.Rows[i]["IndicatorConReturnMeal"] = "1";

                                                }
                                            }
                                        }
                                        else
                                        {
                                            dtPass2.Rows[i]["ConReturnMeal"] = args[2];
                                            dtPass2.Rows[i]["SSRCodeConReturnMeal"] = args[3];
                                            dtPass2.Rows[i]["IndicatorConReturnMeal"] = "1";
                                            DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                dtPass2.Rows[i]["PriceConReturnMeal"] = Convert.ToDecimal(Detail);
                                            }
                                        }
                                    }
                                    //dtPass2.Rows[i]["PriceMeal1"] = Convert.ToDecimal(Detail);
                                }
                                if (Session["dtDrinkReturn2"] == null)
                                {
                                    Session["dtGridPass2"] = dtPass2;
                                    if (args[2] != ",")
                                    {
                                        Session["qtyMeal21"] = (Convert.ToInt32(args[1]) + qtyMeal21);
                                    }
                                    gvPassenger2.DataSource = dtPass2;
                                    gvPassenger2.DataBind();
                                }
                            }
                            else
                            {
                                gvPassenger2.JSProperties["cp_result"] = msgList.Err200050;
                                return;
                            }
                        }
                        if (Session["dtDrinkReturn2"] != null)
                        {
                            DataTable dtDrink = (DataTable)Session["dtDrinkReturn2"];
                            if (cbAllPaxMeal22.Checked == true)
                            {
                                for (int i = 0; i <= dtPass2.Rows.Count - 1; i++)
                                {
                                    if (args[2].ToString() == ",")
                                    {
                                        gvPassenger2.JSProperties["cp_result"] = msgList.Err200051;
                                        return;
                                    }
                                    else
                                    {
                                        // if (Session["defaultdrink"] != null)
                                        //{
                                        // DataTable DefaultDrink = (DataTable)Session["defaultdrink"];
                                        for (int c = 0; c < dtDrink.Rows.Count; c++)
                                        {
                                            // DataRow[] result = dtDrink.Select("SSRCode LIKE '" + DefaultDrink.Rows[c]["SYSValueEx"] + "%'");
                                            //if (result.Length > 0)
                                            //{
                                            //foreach (DataRow row in result)
                                            //{
                                            Detail = dtDrink.Rows[0]["Price"].ToString().Substring(0, dtDrink.Rows[0]["Price"].ToString().Length - 4);
                                            dtPass2.Rows[i]["PriceConReturnDrink"] = Convert.ToDecimal(Detail);
                                            dtPass2.Rows[i]["ConReturnDrink"] = dtDrink.Rows[0]["ConcatenatedField"];
                                            dtPass2.Rows[i]["SSRCodeConReturnDrink"] = dtDrink.Rows[0]["SSRCode"];
                                            dtPass2.Rows[i]["IndicatorConReturnDrink"] = "1";
                                            // }
                                            break;
                                            // }
                                        }
                                        //}
                                    }
                                }
                                Session["dtGridPass2"] = dtPass2;
                                Session["qtyMeal21"] = 0;
                                Session["qtyDrink21"] = 0;
                                gvPassenger2.DataSource = dtPass2;
                                gvPassenger2.DataBind();
                            }
                            else
                            {
                                if (((Convert.ToInt32(args[1]) + qtyMeal21) - 1) <= dtPass2.Rows.Count)
                                {
                                    for (int i = qtyMeal21; i <= (Convert.ToInt32(args[1]) + qtyMeal21) - 1; i++)
                                    {
                                        if (args[2].ToString() == ",")
                                        {
                                            gvPassenger2.JSProperties["cp_result"] = msgList.Err200051;
                                            return;
                                        }
                                        else
                                        {
                                            // if (Session["defaultdrink"] != null)
                                            // {
                                            //DataTable DefaultDrink = (DataTable)Session["defaultdrink"];
                                            for (int c = 0; c < dtDrink.Rows.Count; c++)
                                            {
                                                // DataRow[] result = dtDrink.Select("SSRCode LIKE '" + DefaultDrink.Rows[c]["SYSValueEx"] + "%'");
                                                // if (result.Length > 0)
                                                //{
                                                // foreach (DataRow row in result)
                                                // {
                                                Detail = dtDrink.Rows[0]["Price"].ToString().Substring(0, dtDrink.Rows[0]["Price"].ToString().Length - 4);
                                                dtPass2.Rows[i]["PriceConReturnDrink"] = Convert.ToDecimal(Detail);
                                                dtPass2.Rows[i]["ConReturnDrink"] = dtDrink.Rows[0]["ConcatenatedField"];
                                                dtPass2.Rows[i]["SSRCodeConReturnDrink"] = dtDrink.Rows[0]["SSRCode"];
                                                dtPass2.Rows[i]["IndicatorConReturnDrink"] = "1";
                                                //}
                                                break;
                                                // }
                                            }
                                            //}
                                        }
                                        //dtPass2.Rows[i]["PriceMeal1"] = Convert.ToDecimal(Detail);
                                    }
                                    Session["dtGridPass2"] = dtPass2;
                                    if (args[2] != ",")
                                    {
                                        Session["qtyMeal21"] = (Convert.ToInt32(args[1]) + qtyMeal21);
                                        Session["qtyDrink21"] = (Convert.ToInt32(args[1]) + qtyDrink21);
                                    }
                                    gvPassenger2.DataSource = dtPass2;
                                    gvPassenger2.DataBind();
                                }
                                else
                                {
                                    gvPassenger2.JSProperties["cp_result"] = msgList.Err200050;
                                    return;
                                }
                            }
                        }
                    }
                    else if (args[0] == "Sport")
                    {
                        DataTable dtSport = (DataTable)Session["dtSportReturn"];
                        if (cbAllPaxSport2.Checked == true)
                        {
                            for (int i = 0; i <= dtPass2.Rows.Count - 1; i++)
                            {
                                if (args[2].ToString() == "")
                                {
                                    //dtPass2.Rows[i]["ReturnSport"] = string.Empty;
                                    //dtPass2.Rows[i]["SSRCodeReturnSport"] = string.Empty;
                                    //dtPass2.Rows[i]["PriceReturnSport"] = 0.00;
                                    gvPassenger2.JSProperties["cp_result"] = msgList.Err200052;
                                    return;
                                }
                                else
                                {
                                    if (dtPass2.Rows[i]["ReturnSport"] != null && dtPass2.Rows[i]["ReturnSport"].ToString() != "")
                                    {
                                        DataRow[] result = dtSport.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass2.Rows[i]["PriceReturnSport"]))
                                            {
                                                gvPassenger2.JSProperties["cp_result"] = msgList.Err200053;
                                                return;
                                            }
                                            else
                                            {
                                                dtPass2.Rows[i]["ReturnSport"] = args[2];
                                                dtPass2.Rows[i]["SSRCodeReturnSport"] = args[3];
                                                dtPass2.Rows[i]["PriceReturnSport"] = Convert.ToDecimal(Detail);
                                                dtPass2.Rows[i]["PriceReturnSport1"] = Convert.ToDecimal(row[3]);
                                                dtPass2.Rows[i]["PriceConReturnSport"] = Convert.ToDecimal(row[4]);
                                                dtPass2.Rows[i]["IndicatorReturnSport"] = "1";

                                            }
                                        }
                                    }
                                    else
                                    {
                                        dtPass2.Rows[i]["ReturnSport"] = args[2];
                                        dtPass2.Rows[i]["SSRCodeReturnSport"] = args[3];
                                        DataRow[] result = dtSport.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            dtPass2.Rows[i]["PriceReturnSport"] = Convert.ToDecimal(Detail);
                                            dtPass2.Rows[i]["PriceReturnSport1"] = Convert.ToDecimal(row[3]);
                                            dtPass2.Rows[i]["PriceConReturnSport"] = Convert.ToDecimal(row[4]);
                                            dtPass2.Rows[i]["IndicatorReturnSport"] = "1";
                                        }
                                    }
                                }
                            }
                            Session["dtGridPass2"] = dtPass2;
                            Session["qtySport2"] = 0;
                            gvPassenger2.DataSource = dtPass2;
                            gvPassenger2.DataBind();
                        }
                        else
                        {
                            if (((Convert.ToInt32(args[1]) + qtySport2) - 1) <= dtPass2.Rows.Count)
                            {
                                for (int i = qtySport2; i <= (Convert.ToInt32(args[1]) + qtySport2) - 1; i++)
                                {
                                    if (args[2].ToString() == "")
                                    {
                                        //dtPass2.Rows[i]["ReturnSport"] = string.Empty;
                                        //dtPass2.Rows[i]["SSRCodeReturnSport"] = string.Empty;
                                        //dtPass2.Rows[i]["PriceReturnSport"] = 0.00;
                                        gvPassenger2.JSProperties["cp_result"] = msgList.Err200052;
                                        return;
                                    }
                                    else
                                    {
                                        if (dtPass2.Rows[i]["ReturnSport"] != null && dtPass2.Rows[i]["ReturnSport"].ToString() != "")
                                        {
                                            DataRow[] result = dtSport.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass2.Rows[i]["PriceReturnSport"]))
                                                {
                                                    gvPassenger2.JSProperties["cp_result"] = msgList.Err200053;
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass2.Rows[i]["ReturnSport"] = args[2];
                                                    dtPass2.Rows[i]["SSRCodeReturnSport"] = args[3];
                                                    dtPass2.Rows[i]["PriceReturnSport"] = Convert.ToDecimal(Detail);
                                                    dtPass2.Rows[i]["PriceReturnSport1"] = Convert.ToDecimal(row[3]);
                                                    dtPass2.Rows[i]["PriceConReturnSport"] = Convert.ToDecimal(row[4]);
                                                    dtPass2.Rows[i]["IndicatorReturnSport"] = "1";

                                                }
                                            }
                                        }
                                        else
                                        {
                                            dtPass2.Rows[i]["ReturnSport"] = args[2];
                                            dtPass2.Rows[i]["SSRCodeReturnSport"] = args[3];
                                            dtPass2.Rows[i]["IndicatorReturnSport"] = "1";
                                            DataRow[] result = dtSport.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                dtPass2.Rows[i]["PriceReturnSport"] = Convert.ToDecimal(Detail);
                                                dtPass2.Rows[i]["PriceReturnSport1"] = Convert.ToDecimal(row[3]);
                                                dtPass2.Rows[i]["PriceConReturnSport"] = Convert.ToDecimal(row[4]);
                                            }
                                        }
                                        //dtPass2.Rows[i]["PriceSport"] = Convert.ToDecimal(Detail);
                                    }
                                }
                                Session["dtGridPass2"] = dtPass2;
                                if (args[2] != ",")
                                {
                                    Session["qtySport2"] = Convert.ToInt32(args[1] + qtySport2);
                                }
                                gvPassenger2.DataSource = dtPass2;
                                gvPassenger2.DataBind();
                            }
                            else
                            {
                                gvPassenger2.JSProperties["cp_result"] = msgList.Err200054;
                                return;
                            }
                        }
                    }
                    else if (args[0] == "Duty")
                    {
                        DataTable dtDuty = (DataTable)Session["dtDutyReturn"];
                        if (((Convert.ToInt32(args[1]) + qtyDuty2) - 1) <= dtPass2.Rows.Count)
                        {
                            for (int i = qtyDuty2; i <= Convert.ToInt32(args[1] + qtyDuty2) - 1; i++)
                            {
                                dtPass2.Rows[i]["ReturnDuty"] = args[2];
                                dtPass2.Rows[i]["SSRCodeReturnDuty"] = args[3];
                                DataRow[] result = dtDuty.Select("SSRCode = '" + args[3] + "'");
                                foreach (DataRow row in result)
                                {
                                    Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                    dtPass2.Rows[i]["PriceReturnDuty"] = Convert.ToDecimal(Detail);
                                    dtPass2.Rows[i]["PriceReturnDuty1"] = Convert.ToDecimal(row[3]);
                                    dtPass2.Rows[i]["PriceConReturnDuty"] = Convert.ToDecimal(row[4]);
                                }
                                //dtPass2.Rows[i]["PriceDuty"] = Convert.ToDecimal(Detail);
                            }
                            Session["dtGridPass2"] = dtPass2;
                            Session["qtyDuty2"] = Convert.ToInt32(args[1] + qtyDuty2);
                            gvPassenger2.DataSource = dtPass2;
                            gvPassenger2.DataBind();
                        }
                        else
                        {
                            gvPassenger2.JSProperties["cp_result"] = msgList.Err200055;
                            return;
                        }
                    }
                    else if (args[0] == "Comfort")
                    {
                        DataTable dtComfort = (DataTable)Session["dtComfortReturn"];
                        if (cbAllPaxComfort2.Checked == true)
                        {
                            for (int i = 0; i <= dtPass2.Rows.Count - 1; i++)
                            {
                                if (args[2].ToString() == ",")
                                {
                                    //dtPass2.Rows[i]["ReturnComfort"] = string.Empty;
                                    //dtPass2.Rows[i]["SSRCodeReturnComfort"] = string.Empty;
                                    //dtPass2.Rows[i]["PriceReturnComfort"] = 0.00;
                                    gvPassenger2.JSProperties["cp_result"] = msgList.Err200056;
                                    return;
                                }
                                else
                                {
                                    if (dtPass2.Rows[i]["ReturnComfort"] != null && dtPass2.Rows[i]["ReturnComfort"].ToString() != "")
                                    {
                                        DataRow[] result = dtComfort.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass2.Rows[i]["PriceReturnComfort"]))
                                            {
                                                gvPassenger2.JSProperties["cp_result"] = msgList.Err200057;
                                                return;
                                            }
                                            else
                                            {
                                                dtPass2.Rows[i]["ReturnComfort"] = args[2];
                                                dtPass2.Rows[i]["SSRCodeReturnComfort"] = args[3];
                                                dtPass2.Rows[i]["PriceReturnComfort"] = Convert.ToDecimal(Detail);
                                                dtPass2.Rows[i]["PriceReturnComfort1"] = Convert.ToDecimal(row[3]);
                                                dtPass2.Rows[i]["PriceConReturnComfort"] = Convert.ToDecimal(row[4]);
                                                dtPass2.Rows[i]["IndicatorReturnComfort"] = "1";

                                            }
                                        }
                                    }
                                    else
                                    {
                                        dtPass2.Rows[i]["ReturnComfort"] = args[2];
                                        dtPass2.Rows[i]["SSRCodeReturnComfort"] = args[3];
                                        dtPass2.Rows[i]["IndicatorReturnComfort"] = "1";
                                        DataRow[] result = dtComfort.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            dtPass2.Rows[i]["PriceReturnComfort"] = Convert.ToDecimal(Detail);
                                            dtPass2.Rows[i]["PriceReturnComfort1"] = Convert.ToDecimal(row[3]);
                                            dtPass2.Rows[i]["PriceConReturnComfort"] = Convert.ToDecimal(row[4]);
                                        }
                                    }
                                }
                            }
                            Session["dtGridPass2"] = dtPass2;
                            Session["qtyComfort2"] = 0;
                            gvPassenger2.DataSource = dtPass2;
                            gvPassenger2.DataBind();
                        }
                        else
                        {
                            if (((Convert.ToInt32(args[1]) + qtyComfort2) - 1) <= dtPass2.Rows.Count)
                            {
                                for (int i = qtyComfort2; i <= (Convert.ToInt32(args[1]) + qtyComfort2) - 1; i++)
                                {
                                    if (args[2].ToString() == ",")
                                    {
                                        //dtPass2.Rows[i]["ReturnComfort"] = string.Empty;
                                        //dtPass2.Rows[i]["SSRCodeReturnComfort"] = string.Empty;
                                        //dtPass2.Rows[i]["PriceReturnComfort"] = 0.00;
                                        gvPassenger2.JSProperties["cp_result"] = msgList.Err200056;
                                        return;
                                    }
                                    else
                                    {
                                        if (dtPass2.Rows[i]["ReturnComfort"] != null && dtPass2.Rows[i]["ReturnComfort"].ToString() != "")
                                        {
                                            DataRow[] result = dtComfort.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass2.Rows[i]["PriceReturnComfort"]))
                                                {
                                                    gvPassenger2.JSProperties["cp_result"] = msgList.Err200057;
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass2.Rows[i]["ReturnComfort"] = args[2];
                                                    dtPass2.Rows[i]["SSRCodeReturnComfort"] = args[3];
                                                    dtPass2.Rows[i]["PriceReturnComfort"] = Convert.ToDecimal(Detail);
                                                    dtPass2.Rows[i]["PriceReturnComfort1"] = Convert.ToDecimal(row[3]);
                                                    dtPass2.Rows[i]["PriceConReturnComfort"] = Convert.ToDecimal(row[4]);
                                                    dtPass2.Rows[i]["IndicatorReturnComfort"] = "1";

                                                }
                                            }
                                        }
                                        else
                                        {
                                            dtPass2.Rows[i]["ReturnComfort"] = args[2];
                                            dtPass2.Rows[i]["SSRCodeReturnComfort"] = args[3];
                                            dtPass2.Rows[i]["IndicatorReturnComfort"] = "1";
                                            DataRow[] result = dtComfort.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                dtPass2.Rows[i]["PriceReturnComfort"] = Convert.ToDecimal(Detail);
                                                dtPass2.Rows[i]["PriceReturnComfort1"] = Convert.ToDecimal(row[3]);
                                                dtPass2.Rows[i]["PriceConReturnComfort"] = Convert.ToDecimal(row[4]);
                                            }
                                        }
                                    }
                                    //dtPass2.Rows[i]["PriceComfort"] = Convert.ToDecimal(Detail);

                                }
                                Session["dtGridPass2"] = dtPass2;
                                if (args[2] != ",")
                                {
                                    Session["qtyComfort2"] = (Convert.ToInt32(args[1]) + qtyComfort2);
                                }
                                gvPassenger2.DataSource = dtPass2;
                                gvPassenger2.DataBind();
                            }
                            else
                            {
                                gvPassenger2.JSProperties["cp_result"] = msgList.Err200058;// "The quantity of Comfort Kit Free must be less or equal to total number of passenger.";
                                return;
                            }
                        }
                    }
                    else if (args[0] == "Clear")
                    {

                        dtPass2.Rows[Convert.ToInt16(args[1])]["ReturnBaggage"] = string.Empty;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["SSRCodeReturnBaggage"] = string.Empty;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["PriceReturnBaggage"] = 0.00;

                        dtPass2.Rows[Convert.ToInt16(args[1])]["ReturnMeal"] = string.Empty;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["SSRCodeReturnMeal"] = string.Empty;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["PriceReturnMeal"] = 0.00;

                        dtPass2.Rows[Convert.ToInt16(args[1])]["ConReturnMeal"] = string.Empty;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["SSRCodeReturnMeal"] = string.Empty;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["PriceReturnMeal"] = 0.00;

                        dtPass2.Rows[Convert.ToInt16(args[1])]["ReturnSport"] = string.Empty;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["SSRCodeReturnSport"] = string.Empty;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["PriceReturnSport"] = 0.00;

                        dtPass2.Rows[Convert.ToInt16(args[1])]["ReturnDuty"] = string.Empty;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["SSRCodeReturnDuty"] = string.Empty;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["PriceReturnDuty"] = 0.00;

                        dtPass2.Rows[Convert.ToInt16(args[1])]["ReturnComfort"] = string.Empty;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["SSRCodeReturnComfort"] = string.Empty;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["PriceReturnComfort"] = 0.00;

                        Session["dtGridPass2"] = dtPass2;
                        gvPassenger2.DataSource = dtPass2;
                        gvPassenger2.DataBind();

                        Session["qtyBaggage2"] = Convert.ToInt32(Session["qtyBaggage2"]) - 1;
                        Session["qtyMeal2"] = Convert.ToInt32(Session["qtyMeal2"]) - 1;
                        Session["qtyMeal21"] = Convert.ToInt32(Session["qtyMeal21"]) - 1;
                        Session["qtySport2"] = Convert.ToInt32(Session["qtySport2"]) - 1;
                        Session["qtyComfort2"] = Convert.ToInt32(Session["qtyComfort2"]) - 1;
                        Session["qtyDrink2"] = Convert.ToInt32(Session["qtyDrink2"]) - 1;
                        Session["qtyDrink21"] = Convert.ToInt32(Session["qtyDrink21"]) - 1;
                    }
                }

                //BindLabel();
                //SetMaxValue2();
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void gvPassenger_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {

                if (e.DataColumn.FieldName == "Title")
                {
                    if (e.CellValue != null)
                    {
                        Session["Type"] = e.CellValue.ToString();
                    }
                }

                if (e.DataColumn.FieldName == "Infant")
                {
                    if (Session["Type"] != null)
                    {
                        //change by tyas
                        if (Session["Type"].ToString() == "Chd") //check if still allow to change
                        //if (Convert.ToInt32(drRow["PaxNo"]) == 0)
                        {
                            e.Cell.Attributes.Add("onclick", "event.cancelBubble = true");
                            Session["Type"] = null;
                        }
                        else
                        {

                            e.Cell.Attributes.Add("onclick", "event.cancelBubble = false");
                            Session["Type"] = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
            finally
            {

            }
        }
        #endregion

        #region "Function and Procedure"

        protected void GetInfantDetail(string TransID)
        {
            DataTable dtinfant = new DataTable();
            dtinfant = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTableInfant(TransID);
            if (dtinfant != null && dtinfant.Rows.Count > 0)
            {
                Session["dtInfant"] = dtinfant;
            }
        }
        protected void GetSSRItem(string flight)
        {
            string[] category = new string[] { "Baggage", "Sport", "Drink", "Infant" };
            GridViewDataComboBoxColumn column = new GridViewDataComboBoxColumn();
            for (int p = 0; p < category.Length; p++)
            {
                if (flight == "Depart")
                {
                    column = (gvPassenger.Columns[flight + category[p]] as GridViewDataComboBoxColumn);
                }
                else
                {
                    column = (gvPassenger2.Columns[flight + category[p]] as GridViewDataComboBoxColumn);
                }
                DataTable dtBaggage = null;
                dtBaggage = (DataTable)Session["dt" + category[p] + flight];
                if (dtBaggage != null)
                {
                    DataRow[] result = dtBaggage.Select("SSRCode = ''");
                    //if (result.Length == 0)
                    //{
                    //    if (first == 1 && first == 3)
                    //    {
                    //        dtBaggage.Rows.Add("", "", "");
                    //    }
                    //}
                    column.PropertiesComboBox.DataSource = dtBaggage;

                    column.PropertiesComboBox.ValueField = "SSRCode";
                    column.PropertiesComboBox.TextField = "ConcatenatedField";
                }
            }

            if (flight == "Depart")
            {
                gvPassenger.DataBind();
            }
            else
            {
                gvPassenger2.DataBind();
            }
        }
        protected string GetSellSSR(string signature, string TransID)
        {
            var profiler = MiniProfiler.Current;

            List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
            ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking(signature);
            ABS.Navitaire.APIBooking apiBooking2 = new ABS.Navitaire.APIBooking("");
            BookingControl bookingControl = new BookingControl();
            string SellSessionID = "";
            using (profiler.Step("Navitaire:AgentLogon"))
            {
                SellSessionID = apiBooking.AgentLogon();
            }
            using (profiler.Step("GetAllBK_TRANSDTLCombinePNR"))
            {
                lstbookDTLInfo = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 1, "LEN(RecordLocator) >= 6 AND ");
            }
            int PaxAdult = lstbookDTLInfo.Sum(item => item.PaxAdult);
            int PaxChild = lstbookDTLInfo.Sum(item => item.PaxChild);
            int PaxNum = PaxAdult + PaxChild;
            GetBookingResponse resp;
            using (profiler.Step("GetBookingByPNR"))
            {
                resp = bookingControl.GetBookingByPNR(lstbookDTLInfo[0].RecordLocator, SellSessionID);
            }
            Session["resp"] = resp;
            ABS.Navitaire.BookingManager.Booking responseBookingFromState;// = bookingControl.GetBookingFromState(SellSessionID);
            using (profiler.Step("Navitaire:GetBookingFromState"))
            {
                responseBookingFromState = apiBooking2.GetBookingFromState(SellSessionID,2);
            }
            if (resp != null)
            {
                GetSSRAvailabilityForBookingResponse response;// = apiBooking.GetSSRAvailabilityForBooking(responseBookingFromState, SellSessionID);
                using (profiler.Step("Navitaire:GetSSRAvailabilityForBooking"))
                {
                    response = apiBooking.GetSSRAvailabilityForBooking(responseBookingFromState, SellSessionID);
                }
                if (response != null)
                {
                    ArrayList BaggageCode = new ArrayList();
                    ArrayList BaggageFee = new ArrayList();
                    ArrayList BaggageFeeS1 = new ArrayList();
                    ArrayList BaggageFeeS2 = new ArrayList();
                    int Baggage1 = 0;

                    ArrayList SportCode = new ArrayList();
                    ArrayList SportFee = new ArrayList();
                    ArrayList SportFeeS1 = new ArrayList();
                    ArrayList SportFeeS2 = new ArrayList();
                    int Sport1 = 0;

                    ArrayList ComfortCode = new ArrayList();
                    ArrayList ComfortFee = new ArrayList();
                    ArrayList ComfortFeeS1 = new ArrayList();
                    ArrayList ComfortFeeS2 = new ArrayList();
                    ArrayList ComfortImage = new ArrayList();
                    int Comfort1 = 0;

                    ArrayList InfantCode = new ArrayList();
                    ArrayList InfantFee = new ArrayList();
                    ArrayList InfantFeeS1 = new ArrayList();
                    ArrayList InfantFeeS2 = new ArrayList();
                    int Infant1 = 0;

                    ArrayList MealCode = new ArrayList();
                    ArrayList MealFee = new ArrayList();
                    ArrayList MealImage = new ArrayList();

                    ArrayList DrinkCode = new ArrayList();
                    ArrayList DrinkFee = new ArrayList();

                    ArrayList DutyCode = new ArrayList();
                    ArrayList DutyFee = new ArrayList();
                    ArrayList DutyImage = new ArrayList();


                    ArrayList MealCode1 = new ArrayList();
                    ArrayList MealFee1 = new ArrayList();
                    ArrayList MealImage1 = new ArrayList();

                    ArrayList DrinkCode1 = new ArrayList();
                    ArrayList DrinkFee1 = new ArrayList();

                    ArrayList BaggageCode2 = new ArrayList();
                    ArrayList BaggageFee2 = new ArrayList();
                    ArrayList BaggageFee2S1 = new ArrayList();
                    ArrayList BaggageFee2S2 = new ArrayList();
                    int Baggage2 = 0;

                    ArrayList SportCode2 = new ArrayList();
                    ArrayList SportFee2 = new ArrayList();
                    ArrayList SportFee2S1 = new ArrayList();
                    ArrayList SportFee2S2 = new ArrayList();
                    int Sport2 = 0;

                    ArrayList ComfortCode2 = new ArrayList();
                    ArrayList ComfortFee2 = new ArrayList();
                    ArrayList ComfortFee2S1 = new ArrayList();
                    ArrayList ComfortFee2S2 = new ArrayList();
                    ArrayList ComfortImage2 = new ArrayList();
                    int Comfort2 = 0;
                    //Decimal ComfortAmt2 = 0;

                    ArrayList InfantCode2 = new ArrayList();
                    ArrayList InfantFee2 = new ArrayList();
                    ArrayList InfantFee2S1 = new ArrayList();
                    ArrayList InfantFee2S2 = new ArrayList();
                    int Infant2 = 0;

                    ArrayList MealCode2 = new ArrayList();
                    ArrayList MealFee2 = new ArrayList();
                    ArrayList MealImage2 = new ArrayList();
                    //Decimal MealAmt2 = 0;

                    ArrayList DrinkCode2 = new ArrayList();
                    ArrayList DrinkFee2 = new ArrayList();

                    ArrayList DutyCode2 = new ArrayList();
                    ArrayList DutyFee2 = new ArrayList();
                    ArrayList DutyImage2 = new ArrayList();

                    ArrayList MealCode21 = new ArrayList();
                    ArrayList MealFee21 = new ArrayList();
                    ArrayList MealImage21 = new ArrayList();

                    ArrayList DrinkCode21 = new ArrayList();
                    ArrayList DrinkFee21 = new ArrayList();

                    List<BookingTransactionDetail> lstTransDetail = new List<BookingTransactionDetail>();

                    Session["Currency"] = resp.Booking.CurrencyCode;
                    using (profiler.Step("GetAllBK_TRANSDTLFilterAll"))
                    {
                        lstTransDetail = objBooking.GetAllBK_TRANSDTLFilterAll(TransID, 0);
                    }
                    for (int x = 0; x < lstTransDetail.Count; x++)
                    {
                        if (x == 0)
                        {
                            depart1 = lstTransDetail[x].Origin.Trim();
                            Session["depart1"] = lstTransDetail[x].Origin.Trim();
                            transit1 = lstTransDetail[x].Transit.Trim();
                            Session["transit1"] = lstTransDetail[x].Transit.Trim();
                            return1 = lstTransDetail[x].Destination.Trim();
                            Session["return1"] = lstTransDetail[x].Destination.Trim();
                        }
                        else if (x == 1)
                        {
                            depart2 = lstTransDetail[x].Origin.Trim();
                            Session["depart2"] = lstTransDetail[x].Origin.Trim();
                            transit2 = lstTransDetail[x].Transit.Trim();
                            Session["transit2"] = lstTransDetail[x].Transit.Trim();
                            return2 = lstTransDetail[x].Destination.Trim();
                            Session["return2"] = lstTransDetail[x].Destination.Trim();
                        }
                        else
                        {
                            break;
                        }
                    }

                    SSRAvailabilityForBookingResponse ssrAvailabilityResponseForBooking = response.SSRAvailabilityForBookingResponse;
                    Session["GetssrAvailabilityResponseForBooking"] = response;
                    DataTable dtdefaultBundleBaggage = new DataTable();
                    DataTable dtdefaultBundleSport = new DataTable();
                    DataTable dtdefaultBundleKit = new DataTable();
                    DataTable dtdefaultInfant = new DataTable();

                    if (ssrAvailabilityResponseForBooking != null && ssrAvailabilityResponseForBooking.SSRSegmentList.Length != 0)
                    {
                        dtdefaultBundleBaggage = (DataTable)Application["dtArrayBaggage"];
                        dtdefaultBundleSport = (DataTable)Application["dtArraySport"];
                        dtdefaultBundleKit = (DataTable)Application["dtArrayKit"];
                        dtdefaultInfant = (DataTable)Application["dtArrayInfant"];

                        List<string> lstBaggage = dtdefaultBundleBaggage.AsEnumerable().Select(r => r.Field<string>("ItemCode")).ToList();
                        List<string> lstSport = dtdefaultBundleSport.AsEnumerable().Select(r => r.Field<string>("ItemCode")).ToList();
                        List<string> lstKit = dtdefaultBundleKit.AsEnumerable().Select(r => r.Field<string>("ItemCode")).ToList();
                        List<string> lstInfant = dtdefaultInfant.AsEnumerable().Select(r => r.Field<string>("ItemCode")).ToList();

                        foreach (ABS.Navitaire.BookingManager.SSRSegment SSRSegment in ssrAvailabilityResponseForBooking.SSRSegmentList)
                        {
                            if ((SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == return1) || (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == transit1) || (SSRSegment.LegKey.DepartureStation == transit1 && SSRSegment.LegKey.ArrivalStation == return1))
                            {
                                //depart here 

                                //baggage
                                List<AvailablePaxSSR> paxBaggage = SSRSegment.AvailablePaxSSRList.Where(x => lstBaggage.Contains(x.SSRCode)).ToList();

                                if (paxBaggage != null && paxBaggage.Count > 0)
                                {
                                    int index = paxBaggage.Where(x => BaggageCode.Contains(x.SSRCode)).Count();
                                    if (index <= 0)
                                    {
                                        BaggageCode.AddRange(paxBaggage.Select(x => x.SSRCode).ToList());
                                    }

                                    //if (BaggageFee.Count == 0)
                                    //{
                                    //    BaggageFee.AddRange(ssrAvailabilityResponseForBooking.SSRSegmentList.Where(item => (item.LegKey.DepartureStation == depart1 && item.LegKey.ArrivalStation == return1) || (item.LegKey.DepartureStation == depart1 && item.LegKey.ArrivalStation == transit1) || item.LegKey.DepartureStation == transit1 && item.LegKey.ArrivalStation == return1).Select(x => x.AvailablePaxSSRList.Where(y => lstBaggage.Contains(y.SSRCode)).Select(z => z.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList()).ToList());
                                    //}                                   

                                    if ((SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == return1) || (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == transit1))
                                    {
                                        BaggageFeeS1.AddRange(paxBaggage.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Select(y => y.Amount).Sum()).ToList());
                                    }
                                    else if ((SSRSegment.LegKey.DepartureStation == transit1 && SSRSegment.LegKey.ArrivalStation == return1))
                                    {
                                        BaggageFeeS2.AddRange(paxBaggage.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Select(y => y.Amount).Sum()).ToList());
                                    }


                                }

                                //Meals
                                List<AvailablePaxSSR> paxMealsS1 = SSRSegment.AvailablePaxSSRList.Where(x => x.SSRLegList.Length > 0 && x.Available >= PaxNum && ((SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == return1) || (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == transit1))).ToList();
                                List<AvailablePaxSSR> paxMealsS2 = SSRSegment.AvailablePaxSSRList.Where(x => x.SSRLegList.Length > 0 && x.Available >= PaxNum && (SSRSegment.LegKey.DepartureStation == transit1 && SSRSegment.LegKey.ArrivalStation == return1)).ToList();
                                if (paxMealsS1 != null && paxMealsS1.Count > 0)
                                {
                                    MealFee.AddRange(paxMealsS1.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Select(y => y.Amount).Sum()).ToList());
                                    MealCode.AddRange(paxMealsS1.Select(x => x.SSRCode).ToList());
                                    MealImage.AddRange(paxMealsS1.Select(x => "http://www.airasia.com/images/common/sky-ssr/img_" + x.SSRCode + ".png").ToList());
                                }
                                else if (paxMealsS2 != null && paxMealsS2.Count > 0)
                                {
                                    MealFee1.AddRange(paxMealsS2.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Select(y => y.Amount).Sum()).ToList());
                                    MealCode1.AddRange(paxMealsS2.Select(x => x.SSRCode).ToList());
                                    MealImage1.AddRange(paxMealsS2.Select(x => "http://www.airasia.com/images/common/sky-ssr/img_" + x.SSRCode + ".png").ToList());
                                }

                                //sport
                                List<AvailablePaxSSR> paxSport = SSRSegment.AvailablePaxSSRList.Where(x => lstSport.Contains(x.SSRCode)).ToList();
                                if (paxSport != null && paxSport.Count > 0)
                                {
                                    int index = paxSport.Where(x => SportCode.Contains(x.SSRCode)).Count();
                                    if (index <= 0)
                                    {
                                        SportCode.AddRange(paxSport.Select(x => x.SSRCode).ToList());
                                    }
                                    //SportFee.AddRange(paxSport.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
                                    if ((SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == return1) || (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == transit1))
                                    {
                                        SportFeeS1.AddRange(paxSport.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Select(y => y.Amount).Sum()).ToList());
                                    }
                                    else if ((SSRSegment.LegKey.DepartureStation == transit1 && SSRSegment.LegKey.ArrivalStation == return1))
                                    {
                                        SportFeeS2.AddRange(paxSport.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Select(y => y.Amount).Sum()).ToList());
                                    }
                                }

                                //comfort kit
                                List<AvailablePaxSSR> paxKit = SSRSegment.AvailablePaxSSRList.Where(x => lstKit.Contains(x.SSRCode)).ToList();
                                if (paxKit != null && paxKit.Count > 0)
                                {
                                    int index = paxKit.Where(x => ComfortCode.Contains(x.SSRCode)).Count();
                                    if (index <= 0)
                                    {
                                        ComfortCode.AddRange(paxKit.Select(x => x.SSRCode).ToList());
                                        ComfortImage.AddRange(paxKit.Select(x => "http://www.airasia.com/images/common/sky-ssr/img_" + x.SSRCode + ".png").ToList());
                                    }
                                    //ComfortFee.AddRange(paxKit.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Select(y => y.Amount).Sum()).ToList());

                                    if ((SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == return1) || (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == transit1))
                                    {
                                        ComfortFeeS1.AddRange(paxKit.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Select(y => y.Amount).Sum()).ToList());
                                    }
                                    else if ((SSRSegment.LegKey.DepartureStation == transit1 && SSRSegment.LegKey.ArrivalStation == return1))
                                    {
                                        ComfortFeeS2.AddRange(paxKit.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Select(y => y.Amount).Sum()).ToList());
                                    }
                                }

                                //Infant
                                List<AvailablePaxSSR> paxInfant = SSRSegment.AvailablePaxSSRList.Where(x => lstInfant.Contains(x.SSRCode)).ToList();
                                if (paxInfant != null && paxInfant.Count > 0)
                                {
                                    infantmax = SSRSegment.AvailablePaxSSRList.Where(x => lstInfant.Contains(x.SSRCode)).FirstOrDefault().Available;
                                    int index = paxInfant.Where(x => InfantCode.Contains(x.SSRCode)).Count();
                                    if (index <= 0)
                                    {
                                        InfantCode.AddRange(paxInfant.Select(x => x.SSRCode).ToList());
                                        Session["InfantCode"] = InfantCode;
                                    }
                                    //InfantFee.AddRange(paxInfant.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
                                    if ((SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == return1) || (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == transit1))
                                    {
                                        InfantFeeS1.AddRange(paxInfant.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Select(y => y.Amount).Sum()).ToList());
                                    }
                                    else if ((SSRSegment.LegKey.DepartureStation == transit1 && SSRSegment.LegKey.ArrivalStation == return1))
                                    {
                                        InfantFeeS2.AddRange(paxInfant.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Select(y => y.Amount).Sum()).ToList());
                                    }
                                }
                            }
                            else if ((SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == return2) || (SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == transit2) || (SSRSegment.LegKey.DepartureStation == transit2 && SSRSegment.LegKey.ArrivalStation == return2))
                            {
                                //return here

                                //baggage
                                List<AvailablePaxSSR> paxBaggage = SSRSegment.AvailablePaxSSRList.Where(x => lstBaggage.Contains(x.SSRCode)).ToList();
                                if (paxBaggage != null && paxBaggage.Count > 0)
                                {
                                    int index = paxBaggage.Where(x => BaggageCode2.Contains(x.SSRCode)).Count();
                                    if (index <= 0)
                                    {
                                        BaggageCode2.AddRange(paxBaggage.Select(x => x.SSRCode).ToList());
                                    }
                                    //BaggageFee2.AddRange(paxBaggage.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
                                    //BaggageFee.AddRange(BaggageAmount.Select(x => x.Amount).ToList());

                                    if ((SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == return2) || (SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == transit2))
                                    {
                                        BaggageFee2S1.AddRange(paxBaggage.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Select(y => y.Amount).Sum()).ToList());
                                    }
                                    else if ((SSRSegment.LegKey.DepartureStation == transit2 && SSRSegment.LegKey.ArrivalStation == return2))
                                    {
                                        BaggageFee2S2.AddRange(paxBaggage.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Select(y => y.Amount).Sum()).ToList());
                                    }
                                }

                                //Meals
                                List<AvailablePaxSSR> paxMealsS1 = SSRSegment.AvailablePaxSSRList.Where(x => x.SSRLegList.Length > 0 && x.Available >= PaxNum && ((SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == return2) || (SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == transit2))).ToList();
                                List<AvailablePaxSSR> paxMealsS2 = SSRSegment.AvailablePaxSSRList.Where(x => x.SSRLegList.Length > 0 && x.Available >= PaxNum && (SSRSegment.LegKey.DepartureStation == transit2 && SSRSegment.LegKey.ArrivalStation == return2)).ToList();
                                if (paxMealsS1 != null && paxMealsS1.Count > 0)
                                {
                                    MealFee2.AddRange(paxMealsS1.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Select(y => y.Amount).Sum()).ToList());
                                    MealCode2.AddRange(paxMealsS1.Select(x => x.SSRCode).ToList());
                                    MealImage2.AddRange(paxMealsS1.Select(x => "http://www.airasia.com/images/common/sky-ssr/img_" + x.SSRCode + ".png").ToList());
                                }
                                else if (paxMealsS2 != null && paxMealsS2.Count > 0)
                                {
                                    MealFee21.AddRange(paxMealsS2.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Select(y => y.Amount).Sum()).ToList());
                                    MealCode21.AddRange(paxMealsS2.Select(x => x.SSRCode).ToList());
                                    MealImage21.AddRange(paxMealsS2.Select(x => "http://www.airasia.com/images/common/sky-ssr/img_" + x.SSRCode + ".png").ToList());
                                }

                                //sport
                                List<AvailablePaxSSR> paxSport = SSRSegment.AvailablePaxSSRList.Where(x => lstSport.Contains(x.SSRCode)).ToList();
                                if (paxSport != null && paxSport.Count > 0)
                                {
                                    int index = paxSport.Where(x => SportCode2.Contains(x.SSRCode)).Count();
                                    if (index <= 0)
                                    {
                                        SportCode2.AddRange(paxSport.Select(x => x.SSRCode).ToList());
                                    }
                                    //SportFee2.AddRange(paxSport.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
                                    if ((SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == return2) || (SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == transit2))
                                    {
                                        SportFee2S1.AddRange(paxSport.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Select(y => y.Amount).Sum()).ToList());
                                    }
                                    else if ((SSRSegment.LegKey.DepartureStation == transit2 && SSRSegment.LegKey.ArrivalStation == return2))
                                    {
                                        SportFee2S2.AddRange(paxSport.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Select(y => y.Amount).Sum()).ToList());
                                    }
                                }

                                //comfort kit
                                List<AvailablePaxSSR> paxKit = SSRSegment.AvailablePaxSSRList.Where(x => lstKit.Contains(x.SSRCode)).ToList();
                                if (paxKit != null && paxKit.Count > 0)
                                {
                                    int index = paxKit.Where(x => ComfortCode2.Contains(x.SSRCode)).Count();
                                    if (index <= 0)
                                    {
                                        ComfortCode2.AddRange(paxKit.Select(x => x.SSRCode).ToList());
                                        ComfortImage2.AddRange(paxKit.Select(x => "http://www.airasia.com/images/common/sky-ssr/img_" + x.SSRCode + ".png").ToList());
                                    }
                                    //ComfortFee2.AddRange(paxKit.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());

                                    if ((SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == return2) || (SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == transit2))
                                    {
                                        ComfortFee2S1.AddRange(paxKit.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Select(y => y.Amount).Sum()).ToList());
                                    }
                                    else if ((SSRSegment.LegKey.DepartureStation == transit2 && SSRSegment.LegKey.ArrivalStation == return2))
                                    {
                                        ComfortFee2S2.AddRange(paxKit.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Select(y => y.Amount).Sum()).ToList());
                                    }
                                }

                                //Infant
                                List<AvailablePaxSSR> paxInfant = SSRSegment.AvailablePaxSSRList.Where(x => lstInfant.Contains(x.SSRCode)).ToList();
                                if (paxInfant != null && paxInfant.Count > 0)
                                {
                                    int index = paxInfant.Where(x => InfantCode2.Contains(x.SSRCode)).Count();
                                    if (index <= 0)
                                    {
                                        InfantCode2.AddRange(paxInfant.Select(x => x.SSRCode).ToList());
                                        Session["InfantCode2"] = InfantCode2;
                                    }
                                    //InfantFee2.AddRange(paxInfant.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
                                    if ((SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == return2) || (SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == transit2))
                                    {
                                        InfantFee2S1.AddRange(paxInfant.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Select(y => y.Amount).Sum()).ToList());
                                    }
                                    else if ((SSRSegment.LegKey.DepartureStation == transit2 && SSRSegment.LegKey.ArrivalStation == return2))
                                    {
                                        InfantFee2S2.AddRange(paxInfant.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Select(y => y.Amount).Sum()).ToList());
                                    }
                                }
                            }

                            
                            //for (int j = 0; j < SSRSegment.AvailablePaxSSRList.Length; j++) //for SSR index
                            //{
                            //    if (SSRSegment.AvailablePaxSSRList[j].PaxSSRPriceList.Length > 0) //skip the cases where the SSR is totally free, no build in pricing
                            //    {
                            //        for (int l = 0; l < SSRSegment.AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                            //        {
                            //            decimal SSRAmt = 0;


                            //            for (int k = 0; k < SSRSegment.AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                            //            {
                            //                if (SSRSegment.AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                            //                    SSRAmt += SSRSegment.AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                            //            }
                            //            if ((SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == return1) || (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == transit1) || (SSRSegment.LegKey.DepartureStation == transit1 && SSRSegment.LegKey.ArrivalStation == return1))
                            //            {
                            //                if (dtdefaultBundleBaggage != null && dtdefaultBundleBaggage.Rows.Count > 0 && (dtdefaultBundleBaggage.Select("ItemCode = '" + SSRSegment.AvailablePaxSSRList[j].SSRCode.ToString() + "'").Length != 0))
                            //                {
                            //                    int index = BaggageCode.IndexOf(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                            //                    if (index < 0)
                            //                    {
                            //                        BaggageCode.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                            //                        BaggageFee.Add(SSRAmt);
                            //                        BaggageFeeS1.Add(SSRAmt);

                            //                    }
                            //                    else
                            //                    {
                            //                        BaggageFee[index] = Convert.ToDecimal(BaggageFee[index]) + SSRAmt;
                            //                        BaggageFeeS2.Add(SSRAmt);
                            //                    }
                            //                }
                            //                else if (dtdefaultBundleSport != null && dtdefaultBundleSport.Rows.Count > 0 && (dtdefaultBundleSport.Select("ItemCode = '" + SSRSegment.AvailablePaxSSRList[j].SSRCode.ToString() + "'").Length != 0))
                            //                {
                            //                    int index = SportCode.IndexOf(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                            //                    if (index < 0)
                            //                    {
                            //                        SportCode.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                            //                        SportFee.Add(SSRAmt);
                            //                        SportFeeS1.Add(SSRAmt);
                            //                    }
                            //                    else
                            //                    {
                            //                        SportFee[index] = Convert.ToDecimal(SportFee[index]) + SSRAmt;
                            //                        SportFeeS2.Add(SSRAmt);
                            //                    }
                            //                }
                            //                else if (dtdefaultBundleKit != null && dtdefaultBundleKit.Rows.Count > 0 && (dtdefaultBundleKit.Select("ItemCode = '" + SSRSegment.AvailablePaxSSRList[j].SSRCode.ToString() + "'").Length != 0))
                            //                {
                            //                    int index = ComfortCode.IndexOf(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                            //                    if (index < 0)
                            //                    {
                            //                        ComfortCode.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                            //                        ComfortImage.Add("http://www.airasia.com/images/common/sky-ssr/img_" + SSRSegment.AvailablePaxSSRList[j].SSRCode + ".png");
                            //                        ComfortFee.Add(SSRAmt);
                            //                        ComfortFeeS1.Add(SSRAmt);
                            //                    }
                            //                    else
                            //                    {
                            //                        ComfortFee[index] = Convert.ToDecimal(ComfortFee[index]) + SSRAmt;
                            //                        ComfortFeeS2.Add(SSRAmt);
                            //                    }
                            //                }
                            //                else if (dtdefaultInfant != null && dtdefaultInfant.Rows.Count > 0 && (dtdefaultInfant.Select("ItemCode = '" + SSRSegment.AvailablePaxSSRList[j].SSRCode.ToString() + "'").Length != 0))
                            //                {
                            //                    infantmax = SSRSegment.AvailablePaxSSRList[j].Available;
                            //                    int index = InfantCode.IndexOf(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                            //                    if (index < 0)
                            //                    {
                            //                        InfantCode.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                            //                        InfantFee.Add(SSRAmt);
                            //                        InfantFeeS1.Add(SSRAmt);
                            //                    }
                            //                    else
                            //                    {
                            //                        InfantFee[index] = Convert.ToDecimal(InfantFee[index]) + SSRAmt;
                            //                        InfantFeeS2.Add(SSRAmt);
                            //                    }
                            //                }
                            //                else if (SSRSegment.AvailablePaxSSRList[j].SSRLegList.Length > 0)
                            //                {
                            //                    if (SSRSegment.AvailablePaxSSRList[j].Available > PaxNum)
                            //                    {
                            //                        if ((SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == return1) || (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == transit1))
                            //                        {
                            //                            MealCode.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                            //                            MealImage.Add("http://www.airasia.com/images/common/sky-ssr/img_" + SSRSegment.AvailablePaxSSRList[j].SSRCode + ".png");
                            //                            MealFee.Add(SSRAmt);
                            //                        }
                            //                        else if ((SSRSegment.LegKey.DepartureStation == transit1 && SSRSegment.LegKey.ArrivalStation == return1))
                            //                        {
                            //                            MealCode1.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                            //                            MealImage1.Add("http://www.airasia.com/images/common/sky-ssr/img_" + SSRSegment.AvailablePaxSSRList[j].SSRCode + ".png");
                            //                            MealFee1.Add(SSRAmt);
                            //                        }
                            //                    }
                            //                }
                            //            }
                            //            else if ((SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == return2) || (SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == transit2) || (SSRSegment.LegKey.DepartureStation == transit2 && SSRSegment.LegKey.ArrivalStation == return2))
                            //            {
                            //                if (dtdefaultBundleBaggage != null && dtdefaultBundleBaggage.Rows.Count > 0 && (dtdefaultBundleBaggage.Select("ItemCode = '" + SSRSegment.AvailablePaxSSRList[j].SSRCode.ToString() + "'").Length != 0))
                            //                {
                            //                    int index = BaggageCode2.IndexOf(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                            //                    if (index < 0)
                            //                    {
                            //                        BaggageCode2.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                            //                        BaggageFee2.Add(SSRAmt);
                            //                        BaggageFee2S1.Add(SSRAmt);

                            //                    }
                            //                    else
                            //                    {
                            //                        BaggageFee2[index] = Convert.ToDecimal(BaggageFee2[index]) + SSRAmt;
                            //                        BaggageFee2S2.Add(SSRAmt);
                            //                    }
                            //                }
                            //                else if (dtdefaultBundleSport != null && dtdefaultBundleSport.Rows.Count > 0 && (dtdefaultBundleSport.Select("ItemCode = '" + SSRSegment.AvailablePaxSSRList[j].SSRCode.ToString() + "'").Length != 0))
                            //                {
                            //                    int index = SportCode2.IndexOf(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                            //                    if (index < 0)
                            //                    {
                            //                        SportCode2.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                            //                        SportFee2.Add(SSRAmt);
                            //                        SportFee2S1.Add(SSRAmt);
                            //                    }
                            //                    else
                            //                    {
                            //                        SportFee2[index] = Convert.ToDecimal(SportFee2[index]) + SSRAmt;
                            //                        SportFee2S2.Add(SSRAmt);
                            //                    }
                            //                }
                            //                else if (dtdefaultBundleKit != null && dtdefaultBundleKit.Rows.Count > 0 && (dtdefaultBundleKit.Select("ItemCode = '" + SSRSegment.AvailablePaxSSRList[j].SSRCode.ToString() + "'").Length != 0))
                            //                {
                            //                    int index = ComfortCode2.IndexOf(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                            //                    if (index < 0)
                            //                    {
                            //                        ComfortCode2.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                            //                        ComfortImage2.Add("http://www.airasia.com/images/common/sky-ssr/img_" + SSRSegment.AvailablePaxSSRList[j].SSRCode + ".png");
                            //                        ComfortFee2.Add(SSRAmt);
                            //                        ComfortFee2S1.Add(SSRAmt);

                            //                    }
                            //                    else
                            //                    {
                            //                        ComfortFee2[index] = Convert.ToDecimal(ComfortFee2[index]) + SSRAmt;
                            //                        ComfortFee2S2.Add(SSRAmt);
                            //                    }
                            //                }
                            //                else if (dtdefaultInfant != null && dtdefaultInfant.Rows.Count > 0 && (dtdefaultInfant.Select("ItemCode = '" + SSRSegment.AvailablePaxSSRList[j].SSRCode.ToString() + "'").Length != 0))
                            //                {
                            //                    int index = InfantCode2.IndexOf(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                            //                    if (index < 0)
                            //                    {
                            //                        InfantCode2.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                            //                        InfantFee2.Add(SSRAmt);
                            //                        InfantFee2S1.Add(SSRAmt);
                            //                    }
                            //                    else
                            //                    {
                            //                        InfantFee2[index] = Convert.ToDecimal(InfantFee2[index]) + SSRAmt;
                            //                        InfantFee2S2.Add(SSRAmt);
                            //                    }
                            //                }
                            //                else if (SSRSegment.AvailablePaxSSRList[j].SSRLegList.Length > 0)
                            //                {
                            //                    if (SSRSegment.AvailablePaxSSRList[j].Available > PaxNum)
                            //                    {
                            //                        if ((SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == return2) || (SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == transit2))
                            //                        {
                            //                            MealCode2.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                            //                            MealImage2.Add("http://www.airasia.com/images/common/sky-ssr/img_" + SSRSegment.AvailablePaxSSRList[j].SSRCode + ".png");
                            //                            MealFee2.Add(SSRAmt);
                            //                        }
                            //                        else if ((SSRSegment.LegKey.DepartureStation == transit2 && SSRSegment.LegKey.ArrivalStation == return2))
                            //                        {
                            //                            MealCode21.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                            //                            MealImage21.Add("http://www.airasia.com/images/common/sky-ssr/img_" + SSRSegment.AvailablePaxSSRList[j].SSRCode + ".png");
                            //                            MealFee21.Add(SSRAmt);
                            //                        }
                            //                    }
                            //                }

                            //            }
                            //        }
                            //    }
                            //}

                            //Baggage
                            if (BaggageFeeS2.Count > 0 && Baggage1 == 0)
                            {
                                Baggage1 = 1;
                                BaggageFee.AddRange(BaggageFeeS1.ToArray().Zip(BaggageFeeS2.ToArray(), (x, y) => Convert.ToDecimal(x) + Convert.ToDecimal(y)).ToList());
                            }
                            else
                            {
                                BaggageFee = BaggageFeeS1;
                            }
                            if (BaggageFee2S2.Count > 0 && Baggage2 == 0)
                            {
                                Baggage2 = 1;
                                BaggageFee2.AddRange(BaggageFee2S1.ToArray().Zip(BaggageFee2S2.ToArray(), (x, y) => Convert.ToDecimal(x) + Convert.ToDecimal(y)).ToList());
                            }
                            else
                            {
                                BaggageFee2 = BaggageFee2S1;
                            }

                            //Sport
                            if (SportFeeS2.Count > 0 && Sport1 == 0)
                            {
                                Sport1 = 1;

                                SportFee.AddRange(SportFeeS1.ToArray().Zip(SportFeeS2.ToArray(), (x, y) => Convert.ToDecimal(x) + Convert.ToDecimal(y)).ToList());
                            }
                            else
                            {
                                SportFee = SportFeeS1;
                            }
                            if (SportFee2S2.Count > 0 && Sport2 == 0)
                            {
                                Sport2 = 1;
                                SportFee2.AddRange(SportFee2S1.ToArray().Zip(SportFee2S2.ToArray(), (x, y) => Convert.ToDecimal(x) + Convert.ToDecimal(y)).ToList());
                            }
                            else
                            {
                                SportFee2 = SportFee2S1;
                            }

                            //Comfort
                            if (ComfortFeeS2.Count > 0 && Comfort1 == 0)
                            {
                                Comfort1 = 1;
                                ComfortFee.AddRange(ComfortFeeS1.ToArray().Zip(ComfortFeeS2.ToArray(), (x, y) => Convert.ToDecimal(x) + Convert.ToDecimal(y)).ToList());
                            }
                            else
                            {
                                ComfortFee = ComfortFeeS1;
                            }
                            if (ComfortFee2S2.Count > 0 && Comfort2 == 0)
                            {
                                Comfort2 = 1;
                                ComfortFee2.AddRange(ComfortFee2S1.ToArray().Zip(ComfortFee2S2.ToArray(), (x, y) => Convert.ToDecimal(x) + Convert.ToDecimal(y)).ToList());
                            }
                            else
                            {
                                ComfortFee2 = ComfortFee2S1;
                            }

                            //Infant
                            if (InfantFeeS2.Count > 0 && Infant1 == 0)
                            {
                                Infant1 = 1;
                                InfantFee.AddRange(InfantFeeS1.ToArray().Zip(InfantFeeS2.ToArray(), (x, y) => Convert.ToDecimal(x) + Convert.ToDecimal(y)).ToList());
                            }
                            else
                            {
                                InfantFee = InfantFeeS1;
                            }
                            if (InfantFee2S2.Count > 0 && Infant2 == 0)
                            {
                                Infant2 = 1;
                                InfantFee2.AddRange(InfantFee2S1.ToArray().Zip(InfantFee2S2.ToArray(), (x, y) => Convert.ToDecimal(x) + Convert.ToDecimal(y)).ToList());
                            }
                            else
                            {
                                InfantFee2 = InfantFee2S1;
                            }
                        }

                        GetSSRAvailabilityForBookingResponse responses = new GetSSRAvailabilityForBookingResponse();// apiBooking.GetSSRAvailabilityForBooking(responseBookingFromState, SellSessionID);
                        using (profiler.Step("Navitaire:GetSSRAvailabilityForBooking"))
                        {
                            response = apiBooking.GetSSRAvailabilityForBooking(responseBookingFromState, SellSessionID);
                        }
                        //string xmlresponses = GetXMLString(responses);
                        if (response != null)
                        {
                            SSRAvailabilityForBookingResponse ssrAvailabilityResponseForBookings = response.SSRAvailabilityForBookingResponse;
                            Session["GetssrAvailabilityResponseForBooking"] = response;

                            if (ssrAvailabilityResponseForBookings != null && ssrAvailabilityResponseForBookings.SSRSegmentList.Length != 0)
                            {
                                DataTable dtdefaultBundleDrink = (DataTable)Application["dtArrayDrink"];
                                List<string> lstDrink = dtdefaultBundleDrink.AsEnumerable().Select(r => r.Field<string>("ItemCode")).ToList();
                                foreach (ABS.Navitaire.BookingManager.SSRSegment SSRSegment in ssrAvailabilityResponseForBookings.SSRSegmentList)
                                {
                                    //Drink
                                    List<AvailablePaxSSR> paxDrink = SSRSegment.AvailablePaxSSRList.Where(x => lstDrink.Contains(x.SSRCode)).ToList();
                                    if (paxDrink != null && paxDrink.Count > 0)
                                    {
                                        if ((SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == return1) || (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == transit1) || (SSRSegment.LegKey.DepartureStation == transit1 && SSRSegment.LegKey.ArrivalStation == return1))
                                        {
                                            //depart here 
                                            if ((SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == return1) || (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == transit1))
                                            {
                                                DrinkCode.AddRange(paxDrink.Select(x => x.SSRCode).ToList());
                                                DrinkFee.AddRange(paxDrink.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Select(y => y.Amount).Sum()).ToList());
                                            }
                                            else if ((SSRSegment.LegKey.DepartureStation == transit1 && SSRSegment.LegKey.ArrivalStation == return1))
                                            {
                                                DrinkCode1.AddRange(paxDrink.Select(x => x.SSRCode).ToList());
                                                DrinkFee1.AddRange(paxDrink.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Select(y => y.Amount).Sum()).ToList());
                                            }
                                        }
                                        else if ((SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == return2) || (SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == transit2) || (SSRSegment.LegKey.DepartureStation == transit2 && SSRSegment.LegKey.ArrivalStation == return2))
                                        {
                                            //return here 
                                            if ((SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == return2) || (SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == transit2))
                                            {
                                                DrinkCode2.AddRange(paxDrink.Select(x => x.SSRCode).ToList());
                                                DrinkFee2.AddRange(paxDrink.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Select(y => y.Amount).Sum()).ToList());
                                            }
                                            else if ((SSRSegment.LegKey.DepartureStation == transit2 && SSRSegment.LegKey.ArrivalStation == return2))
                                            {
                                                DrinkCode21.AddRange(paxDrink.Select(x => x.SSRCode).ToList());
                                                DrinkFee21.AddRange(paxDrink.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Select(y => y.Amount).Sum()).ToList());
                                            }
                                        }
                                    }

                                    //for (int j = 0; j < SSRSegment.AvailablePaxSSRList.Length; j++) //for SSR index
                                    //{
                                    //    if (SSRSegment.AvailablePaxSSRList[j].PaxSSRPriceList.Length > 0) //skip the cases where the SSR is totally free, no build in pricing
                                    //    {
                                    //        for (int l = 0; l < SSRSegment.AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    //        {
                                    //            decimal SSRAmt = 0;

                                    //            if (dtdefaultBundle != null && dtdefaultBundle.Rows.Count > 0 && (dtdefaultBundle.Select("ItemCode = '" + SSRSegment.AvailablePaxSSRList[j].SSRCode.ToString() + "'").Length != 0))
                                    //            {
                                    //                for (int k = 0; k < SSRSegment.AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                    //                {
                                    //                    if (SSRSegment.AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                    //                        SSRAmt += SSRSegment.AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                    //                }
                                    //                if ((SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == return1) || (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == transit1))
                                    //                {
                                    //                    if (DrinkCode.Cast<string>().ToList().Where(item => item.StartsWith(SSRSegment.AvailablePaxSSRList[j].SSRCode.Substring(0, 2))).Count() == 0)
                                    //                    {
                                    //                        DrinkCode.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                                    //                        DrinkFee.Add(SSRAmt);
                                    //                    }
                                    //                }
                                    //                else if ((SSRSegment.LegKey.DepartureStation == transit1 && SSRSegment.LegKey.ArrivalStation == return1))
                                    //                {
                                    //                    if (DrinkCode1.Cast<string>().ToList().Where(item => item.StartsWith(SSRSegment.AvailablePaxSSRList[j].SSRCode.Substring(0, 2))).Count() == 0)
                                    //                    {
                                    //                        DrinkCode1.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                                    //                        DrinkFee1.Add(SSRAmt);
                                    //                    }
                                    //                }
                                    //                else if ((SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == return2) || (SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == transit2))
                                    //                {
                                    //                    if (DrinkCode2.Cast<string>().ToList().Where(item => item.StartsWith(SSRSegment.AvailablePaxSSRList[j].SSRCode.Substring(0, 2))).Count() == 0)
                                    //                    {
                                    //                        DrinkCode2.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                                    //                        DrinkFee2.Add(SSRAmt);
                                    //                    }
                                    //                }
                                    //                else if ((SSRSegment.LegKey.DepartureStation == transit2 && SSRSegment.LegKey.ArrivalStation == return2) || (SSRSegment.LegKey.DepartureStation == transit2 && SSRSegment.LegKey.ArrivalStation == return2))
                                    //                {
                                    //                    if (DrinkCode21.Cast<string>().ToList().Where(item => item.StartsWith(SSRSegment.AvailablePaxSSRList[j].SSRCode.Substring(0, 2))).Count() == 0)
                                    //                    {
                                    //                        DrinkCode21.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                                    //                        DrinkFee21.Add(SSRAmt);
                                    //                    }
                                    //                }
                                    //            }

                                    //        }
                                    //    }
                                    //}

                                }
                            }



                            foreach (ABS.Navitaire.BookingManager.SSRSegment SSRSegment in ssrAvailabilityResponseForBooking.SSRSegmentList)
                            {
                                if (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == return1)
                                {
                                    IsDepart = true;
                                    Session["OneWay"] = true;
                                    InitializeForm(Session["Currency"].ToString(), BaggageCode, BaggageFee, BaggageFeeS1, BaggageFeeS2, SportCode, SportFee, SportFeeS1, SportFeeS2, ComfortCode, ComfortFee, ComfortFeeS1, ComfortFeeS2, ComfortImage, MealCode, MealFee, MealImage, null, null, null, DutyCode, DutyFee, DutyImage, DrinkCode, DrinkFee, null, null, InfantCode, InfantFee, InfantFeeS1, InfantFeeS2, "Depart");
                                }
                                else if (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == transit1)
                                {
                                    IsDepartTransit = true;
                                    Session["OneWay"] = true;
                                    InitializeForm(Session["Currency"].ToString(), BaggageCode, BaggageFee, BaggageFeeS1, BaggageFeeS2, SportCode, SportFee, SportFeeS1, SportFeeS2, ComfortCode, ComfortFee, ComfortFeeS1, ComfortFeeS2, ComfortImage, MealCode, MealFee, MealImage, MealCode1, MealFee1, MealImage1, DutyCode, DutyFee, DutyImage, DrinkCode, DrinkFee, DrinkCode1, DrinkFee1, InfantCode, InfantFee, InfantFeeS1, InfantFeeS2, "Depart");

                                }
                                else if (SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == return2)
                                {
                                    IsReturn = true;
                                    Session["OneWay"] = false;
                                    InitializeForm(Session["Currency"].ToString(), BaggageCode2, BaggageFee2, BaggageFee2S1, BaggageFee2S2, SportCode2, SportFee2, SportFee2S1, SportFee2S2, ComfortCode2, ComfortFee2, ComfortFee2S1, ComfortFee2S2, ComfortImage2, MealCode2, MealFee2, MealImage2, null, null, null, DutyCode2, DutyFee2, DutyImage2, DrinkCode2, DrinkFee2, null, null, InfantCode2, InfantFee2, InfantFee2S1, InfantFee2S2, "Return");
                                }
                                else if (SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == transit2)
                                {
                                    IsReturnTransit = true;
                                    Session["OneWay"] = false;
                                    InitializeForm(Session["Currency"].ToString(), BaggageCode2, BaggageFee2, BaggageFee2S1, BaggageFee2S2, SportCode2, SportFee2, SportFee2S1, SportFee2S2, ComfortCode2, ComfortFee2, ComfortFee2S1, ComfortFee2S2, ComfortImage2, MealCode2, MealFee2, MealImage2, MealCode21, MealFee21, MealImage21, DutyCode2, DutyFee2, DutyImage2, DrinkCode2, DrinkFee2, DrinkCode21, DrinkFee21, InfantCode2, InfantFee2, InfantFee2S1, InfantFee2S2, "Return");
                                }

                            }

                            
                        }
                        else
                        {
                            Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.InvalidPage);
                        }


                    }
                }
            }
            return "";
        }

        protected void GetPassengerList(string TransID, string Flight)
        {
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;
            try
            {

                DataTable dtPass = new DataTable();
                using (profiler.Step("GetAllBK_PASSENGERLISTWithSSRDataTableNewManage"))
                {
                    dtPass = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTableNewManage(TransID, false, "");
                }
                if (dtPass != null && dtPass.Rows.Count > 0)
                {
                    dtPassOld = dtPass;
                    dtPass2Old = dtPass;
                    if (Session["dtGridPass"] == null)
                    {
                        gvPassenger.DataSource = dtPass;
                        gvPassenger.DataBind();
                        Session["dtGridPass"] = dtPass;
                    }


                    if (Session["OneWay"] != null)
                    {
                        Boolean OneWay = (Boolean)Session["OneWay"];
                        if (OneWay != true)
                        {
                            if (Session["dtGridPass2"] == null)
                            {
                                gvPassenger2.DataSource = dtPass;
                                gvPassenger2.DataBind();
                                Session["dtGridPass2"] = dtPass;
                            }
                        }
                    }

                    GetInfantDetail(TransID);
                    MaxPax.Value = dtPass.Rows.Count.ToString();
                }
                else
                {
                    Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.InvalidPage);
                }

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }



        protected void GetPassengerList2(string TransID)
        {
            try
            {
                //HttpCookie cookie = Request.Cookies["cookieTemFlight"];
                DataTable dtPass2 = new DataTable();
                if (Session["dtGridPass2"] != null)
                {
                    dtPass2 = (DataTable)Session["dtGridPass2"];
                }
                else
                {
                    dtPass2 = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTableManage(TransID, false, "BK_TRANSDTL.SeqNo % 2 = 0");
                }
                if (dtPass2 != null && dtPass2.Rows.Count > 0)
                {

                    gvPassenger2.DataSource = dtPass2;
                    gvPassenger2.DataBind();

                    Session["dtGridPass2"] = dtPass2;
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void ValidatePopup(object sender, CallbackEventArgs e)
        {
            MessageList msgList = new MessageList();
            DataTable dataClass = new DataTable();
            dataClass = (DataTable)HttpContext.Current.Session["dtGridPass"];
            for (int ii = 0; ii < dataClass.Rows.Count;ii++)
            {
                if (dataClass.Rows[ii]["DepartInfant"].ToString() != "" && dataClass.Rows[ii]["IndicatorDepartInfant"].ToString() == "1")
                {
                    if (HttpContext.Current.Session["lstPassInfantData"] != null)
                    {
                        lstPassInfantData = (List<PassengerData>)HttpContext.Current.Session["lstPassInfantData"];

                        List<PassengerData> NewlstPassInfantData = lstPassInfantData.Where(item => item.PassengerID == (dataClass.Rows[ii]["PassengerID"]).ToString()).ToList();
                        if (NewlstPassInfantData.Count == 0)
                        {
                            e.Result = msgList.Err200108;
                            return;
                        }
                    }
                    else
                    {
                        e.Result = msgList.Err200108;
                        return;
                    }
                }
            }


            List<BookingTransactionDetail> lstTransDetail = new List<BookingTransactionDetail>();

            depart1 = Session["depart1"].ToString();
            transit1 = Session["transit1"].ToString();
            return1 = Session["return1"].ToString();

            if ((Boolean)Session["OneWay"] != true)
            {
                depart2 = Session["depart2"].ToString();
                transit2 = Session["transit2"].ToString();
                return2 = Session["return2"].ToString();
            }


            listbk_transssrinfo = new List<Bk_transssr>();
            listbk_transssrinfo1 = new List<Bk_transssr>();
            listbk_transssrinfo2 = new List<Bk_transssr>();
            listbk_transssrinfo1t = new List<Bk_transssr>();
            listbk_transssrinfo2t = new List<Bk_transssr>();

            ArrayList aMsgList = new ArrayList();
            GetSSRAvailabilityForBookingResponse ssrResponse = (GetSSRAvailabilityForBookingResponse)Session["GetssrAvailabilityResponseForBooking"];
            var profiler = MiniProfiler.Current;
            try
            {
                if (Session["AgentSet"] == null)
                {
                    e.Result = msgList.Err100025;
                    return;
                }
                else
                {
                    dataClass = new DataTable();
                    dataClass = (DataTable)HttpContext.Current.Session["dtGridPass"];
                    foreach (DataRow dr in dataClass.Rows)
                    {
                        if (dr["SSRCodeDepartBaggage"].ToString() != "" || dr["SSRCodeDepartInfant"].ToString() != "" || dr["SSRCodeDepartMeal"].ToString() != "" || dr["SSRCodeDepartDrink"].ToString() != "" || dr["SSRCodeDepartSport"].ToString() != "" || dr["SSRCodeDepartComfort"].ToString() != "" || dr["SSRCodeDepartDuty"].ToString() != "" || dr["SSRCodeDepartMeal"].ToString() != "" || dr["SSRCodeDepartDrink"].ToString() != "")
                        {
                            BK_TRANSADDONInfo = new Bk_transaddon();
                            BK_TRANSADDONInfo.TransID = dr["TransID"].ToString();
                            BK_TRANSADDONInfo.RecordLocator = dr["PNR"].ToString();
                            BK_TRANSADDONInfo.PassengerID = (Convert.ToInt32(dr["PassengerID"])).ToString();
                            BK_TRANSADDONInfo.Segment = 0;
                            BK_TRANSADDONInfo.SeqNo = 0;
                            BK_TRANSADDONInfo.TripMode = 0;
                            BK_TRANSADDONInfo.CarrierCode = ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.CarrierCode;
                            BK_TRANSADDONInfo.FlightNo = ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.FlightNumber;
                            BK_TRANSADDONInfo.Origin = depart1;

                            if (transit1 != "")
                            {
                                BK_TRANSADDONInfo.Destination = transit1;
                            }
                            else
                            {
                                BK_TRANSADDONInfo.Destination = return1;
                            }
                            BK_TRANSADDONInfo.BaggageCode = dr["SSRCodeDepartBaggage"].ToString();
                            BK_TRANSADDONInfo.BaggageAmt = (decimal)dr["PriceDepartBaggage1"];
                            BK_TRANSADDONInfo.IndicatorBaggage = (int)dr["IndicatorDepartBaggage"];
                            BK_TRANSADDONInfo.MealCode1 = dr["SSRCodeDepartMeal"].ToString();
                            BK_TRANSADDONInfo.MealRate1 = (decimal)dr["PriceDepartMeal"];
                            BK_TRANSADDONInfo.MealQty1 = 1;
                            BK_TRANSADDONInfo.IndicatorMeal = (int)dr["IndicatorDepartMeal"];
                            BK_TRANSADDONInfo.MealSubTotal1 = (BK_TRANSADDONInfo.MealRate1 * BK_TRANSADDONInfo.MealQty1);
                            BK_TRANSADDONInfo.MealTotalAmt = (BK_TRANSADDONInfo.MealSubTotal1);
                            BK_TRANSADDONInfo.DrinkCode1 = dr["SSRCodeDepartDrink"].ToString();
                            BK_TRANSADDONInfo.DrinkRate1 = (decimal)dr["PriceDepartDrink"];
                            BK_TRANSADDONInfo.DrinkQty1 = 1;
                            BK_TRANSADDONInfo.IndicatorDrink = (int)dr["IndicatorDepartDrink"];
                            BK_TRANSADDONInfo.DrinkSubTotal1 = (BK_TRANSADDONInfo.DrinkRate1 * BK_TRANSADDONInfo.DrinkQty1);
                            BK_TRANSADDONInfo.DrinkTotalAmt = (BK_TRANSADDONInfo.DrinkSubTotal1);
                            BK_TRANSADDONInfo.SportCode = dr["SSRCodeDepartSport"].ToString();
                            BK_TRANSADDONInfo.SportAmt = (decimal)dr["PriceDepartSport1"];
                            BK_TRANSADDONInfo.IndicatorSport = (int)dr["IndicatorDepartSport"];
                            BK_TRANSADDONInfo.KitCode = dr["SSRCodeDepartComfort"].ToString();
                            BK_TRANSADDONInfo.IndicatorKit = (int)dr["IndicatorDepartComfort"];
                            BK_TRANSADDONInfo.KitAmt = (decimal)dr["PriceDepartComfort1"];
                            BK_TRANSADDONInfo.DutyCode = dr["SSRCodeDepartDuty"].ToString();
                            BK_TRANSADDONInfo.DutyAmt = (decimal)dr["PriceDepartDuty1"];
                            BK_TRANSADDONInfo.InfantCode = dr["SSRCodeDepartInfant"].ToString();
                            BK_TRANSADDONInfo.InfantAmt = (decimal)dr["PriceDepartInfant"];
                            BK_TRANSADDONInfo.IndicatorInfant = (int)dr["IndicatorDepartInfant"];
                            BK_TRANSADDONInfo.TotalAmount = (BK_TRANSADDONInfo.BaggageAmt + BK_TRANSADDONInfo.MealSubTotal1 + BK_TRANSADDONInfo.DrinkSubTotal1 + BK_TRANSADDONInfo.SportAmt + BK_TRANSADDONInfo.KitAmt + BK_TRANSADDONInfo.InfantAmt + BK_TRANSADDONInfo.DutyAmt);
                            BK_TRANSADDONInfo.CreateBy = MyUserSet.UserName;
                            BK_TRANSADDONInfo.LastSyncBy = MyUserSet.UserName;

                            listBK_TRANSADDONInfo.Add(BK_TRANSADDONInfo);
                            listBK_TRANSADDONInfo1.Add(BK_TRANSADDONInfo);
                            if (transit1 != "")
                            {
                                BK_TRANSADDONInfo = new Bk_transaddon();
                                BK_TRANSADDONInfo.TransID = dr["TransID"].ToString();
                                BK_TRANSADDONInfo.RecordLocator = dr["PNR"].ToString();
                                BK_TRANSADDONInfo.PassengerID = (Convert.ToInt32(dr["PassengerID"])).ToString();
                                BK_TRANSADDONInfo.Segment = 1;
                                BK_TRANSADDONInfo.SeqNo = 1;
                                BK_TRANSADDONInfo.TripMode = 0;
                                BK_TRANSADDONInfo.CarrierCode = ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.CarrierCode;
                                BK_TRANSADDONInfo.FlightNo = ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.FlightNumber;
                                BK_TRANSADDONInfo.Origin = transit1;
                                BK_TRANSADDONInfo.Destination = return1;
                                BK_TRANSADDONInfo.BaggageCode = dr["SSRCodeDepartBaggage"].ToString();
                                BK_TRANSADDONInfo.BaggageAmt = (decimal)dr["PriceConDepartBaggage"];
                                BK_TRANSADDONInfo.IndicatorBaggage = (int)dr["IndicatorDepartBaggage"];
                                BK_TRANSADDONInfo.MealCode1 = dr["SSRCodeConDepartMeal"].ToString();
                                BK_TRANSADDONInfo.MealRate1 = (decimal)dr["PriceConDepartMeal"];
                                BK_TRANSADDONInfo.MealQty1 = 1;
                                BK_TRANSADDONInfo.IndicatorMeal = (int)dr["IndicatorConDepartMeal"];
                                BK_TRANSADDONInfo.MealSubTotal1 = (BK_TRANSADDONInfo.MealRate1 * BK_TRANSADDONInfo.MealQty1);
                                BK_TRANSADDONInfo.MealTotalAmt = (BK_TRANSADDONInfo.MealSubTotal1);
                                BK_TRANSADDONInfo.DrinkCode1 = dr["SSRCodeConDepartDrink"].ToString();
                                BK_TRANSADDONInfo.DrinkRate1 = (decimal)dr["PriceConDepartDrink"];
                                BK_TRANSADDONInfo.DrinkQty1 = 1;
                                BK_TRANSADDONInfo.IndicatorDrink = (int)dr["IndicatorConDepartDrink"];
                                BK_TRANSADDONInfo.DrinkSubTotal1 = (BK_TRANSADDONInfo.DrinkRate1 * BK_TRANSADDONInfo.DrinkQty1);
                                BK_TRANSADDONInfo.DrinkTotalAmt = (BK_TRANSADDONInfo.DrinkSubTotal1);
                                BK_TRANSADDONInfo.SportCode = dr["SSRCodeDepartSport"].ToString();
                                BK_TRANSADDONInfo.SportAmt = (decimal)dr["PriceConDepartSport"];
                                BK_TRANSADDONInfo.IndicatorSport = (int)dr["IndicatorDepartSport"];
                                BK_TRANSADDONInfo.KitCode = dr["SSRCodeDepartComfort"].ToString();
                                BK_TRANSADDONInfo.KitAmt = (decimal)dr["PriceConDepartComfort"];
                                BK_TRANSADDONInfo.IndicatorKit = (int)dr["IndicatorDepartComfort"];
                                BK_TRANSADDONInfo.InfantCode = dr["SSRCodeDepartInfant"].ToString();
                                BK_TRANSADDONInfo.InfantAmt = (decimal)dr["PriceConDepartInfant"];
                                BK_TRANSADDONInfo.IndicatorInfant = (int)dr["IndicatorDepartInfant"];
                                BK_TRANSADDONInfo.DutyCode = dr["SSRCodeDepartDuty"].ToString();
                                BK_TRANSADDONInfo.DutyAmt = (decimal)dr["PriceConDepartDuty"];
                                BK_TRANSADDONInfo.TotalAmount = (BK_TRANSADDONInfo.BaggageAmt + BK_TRANSADDONInfo.MealSubTotal1 + BK_TRANSADDONInfo.DrinkSubTotal1 + BK_TRANSADDONInfo.SportAmt + BK_TRANSADDONInfo.KitAmt + BK_TRANSADDONInfo.InfantAmt + BK_TRANSADDONInfo.DutyAmt);
                                BK_TRANSADDONInfo.CreateBy = MyUserSet.UserName;
                                BK_TRANSADDONInfo.LastSyncBy = MyUserSet.UserName;

                                listBK_TRANSADDONInfo.Add(BK_TRANSADDONInfo);
                                listBK_TRANSADDONInfo1t.Add(BK_TRANSADDONInfo);
                            }
                        }
                    }

                    if ((Boolean)Session["OneWay"] != true)
                    {
                        DataTable dataClass2 = new DataTable();
                        dataClass2 = (DataTable)HttpContext.Current.Session["dtGridPass2"];
                        foreach (DataRow dr in dataClass2.Rows)
                        {
                            if (dr["SSRCodeReturnBaggage"].ToString() != "" || dr["SSRCodeReturnInfant"].ToString() != "" || dr["SSRCodeReturnMeal"].ToString() != "" || dr["SSRCodeReturnDrink"].ToString() != "" || dr["SSRCodeReturnSport"].ToString() != "" || dr["SSRCodeReturnComfort"].ToString() != "" || dr["SSRCodeReturnDuty"].ToString() != "" || dr["SSRCodeConReturnMeal"].ToString() != "" || dr["SSRCodeConReturnDrink"].ToString() != "")
                            {
                                BK_TRANSADDONInfo = new Bk_transaddon();
                                BK_TRANSADDONInfo.TransID = dr["TransID"].ToString();
                                BK_TRANSADDONInfo.RecordLocator = dr["PNR"].ToString();
                                BK_TRANSADDONInfo.PassengerID = (Convert.ToInt32(dr["PassengerID"])).ToString();
                                BK_TRANSADDONInfo.Segment = 0;
                                BK_TRANSADDONInfo.SeqNo = 2;
                                BK_TRANSADDONInfo.TripMode = 1;
                                if (transit1 != "")
                                {
                                    BK_TRANSADDONInfo.CarrierCode = ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[2].LegKey.CarrierCode;
                                    BK_TRANSADDONInfo.FlightNo = ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[2].LegKey.FlightNumber;
                                }
                                else
                                {
                                    BK_TRANSADDONInfo.CarrierCode = ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.CarrierCode;
                                    BK_TRANSADDONInfo.FlightNo = ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.FlightNumber;
                                }
                                BK_TRANSADDONInfo.Origin = depart2;
                                if (transit2 != "")
                                {
                                    BK_TRANSADDONInfo.Destination = transit2;
                                }
                                else
                                {
                                    BK_TRANSADDONInfo.Destination = return2;
                                }
                                BK_TRANSADDONInfo.BaggageCode = dr["SSRCodeReturnBaggage"].ToString();
                                BK_TRANSADDONInfo.BaggageAmt = (decimal)dr["PriceReturnBaggage1"];
                                BK_TRANSADDONInfo.IndicatorBaggage = (int)dr["IndicatorReturnBaggage"];
                                BK_TRANSADDONInfo.MealCode1 = dr["SSRCodeReturnMeal"].ToString();
                                BK_TRANSADDONInfo.MealRate1 = (decimal)dr["PriceReturnMeal"];
                                BK_TRANSADDONInfo.MealQty1 = 1;
                                BK_TRANSADDONInfo.IndicatorMeal = (int)dr["IndicatorReturnMeal"];
                                BK_TRANSADDONInfo.MealSubTotal1 = (BK_TRANSADDONInfo.MealRate1 * BK_TRANSADDONInfo.MealQty1);
                                BK_TRANSADDONInfo.MealTotalAmt = (BK_TRANSADDONInfo.MealSubTotal1);
                                BK_TRANSADDONInfo.DrinkCode1 = dr["SSRCodeReturnDrink"].ToString();
                                BK_TRANSADDONInfo.DrinkRate1 = (decimal)dr["PriceReturnDrink"];
                                BK_TRANSADDONInfo.DrinkQty1 = 1;
                                BK_TRANSADDONInfo.IndicatorDrink = (int)dr["IndicatorReturnDrink"];
                                BK_TRANSADDONInfo.DrinkSubTotal1 = (BK_TRANSADDONInfo.DrinkRate1 * BK_TRANSADDONInfo.DrinkQty1);
                                BK_TRANSADDONInfo.DrinkTotalAmt = (BK_TRANSADDONInfo.DrinkSubTotal1);
                                BK_TRANSADDONInfo.SportCode = dr["SSRCodeReturnSport"].ToString();
                                BK_TRANSADDONInfo.SportAmt = (decimal)dr["PriceReturnSport1"];
                                BK_TRANSADDONInfo.IndicatorSport = (int)dr["IndicatorReturnSport"];
                                BK_TRANSADDONInfo.KitCode = dr["SSRCodeReturnComfort"].ToString();
                                BK_TRANSADDONInfo.KitAmt = (decimal)dr["PriceReturnComfort1"];
                                BK_TRANSADDONInfo.InfantCode = dr["SSRCodeReturnInfant"].ToString();
                                BK_TRANSADDONInfo.InfantAmt = (decimal)dr["PriceReturnInfant"];
                                BK_TRANSADDONInfo.IndicatorInfant = (int)dr["IndicatorReturnInfant"];
                                BK_TRANSADDONInfo.IndicatorKit = (int)dr["IndicatorReturnComfort"];
                                BK_TRANSADDONInfo.DutyCode = dr["SSRCodeReturnDuty"].ToString();
                                BK_TRANSADDONInfo.DutyAmt = (decimal)dr["PriceReturnDuty1"];
                                BK_TRANSADDONInfo.TotalAmount = (BK_TRANSADDONInfo.BaggageAmt + BK_TRANSADDONInfo.MealSubTotal1 + BK_TRANSADDONInfo.DrinkSubTotal1 + BK_TRANSADDONInfo.SportAmt + BK_TRANSADDONInfo.KitAmt + BK_TRANSADDONInfo.InfantAmt + BK_TRANSADDONInfo.DutyAmt);
                                BK_TRANSADDONInfo.CreateBy = MyUserSet.UserName;
                                BK_TRANSADDONInfo.LastSyncBy = MyUserSet.UserName;

                                listBK_TRANSADDONInfo.Add(BK_TRANSADDONInfo);
                                listBK_TRANSADDONInfo2.Add(BK_TRANSADDONInfo);
                                if (transit2 != "")
                                {
                                    BK_TRANSADDONInfo = new Bk_transaddon();
                                    BK_TRANSADDONInfo.TransID = dr["TransID"].ToString();
                                    BK_TRANSADDONInfo.RecordLocator = dr["PNR"].ToString();
                                    BK_TRANSADDONInfo.PassengerID = (Convert.ToInt32(dr["PassengerID"])).ToString();
                                    BK_TRANSADDONInfo.Segment = 1;
                                    BK_TRANSADDONInfo.SeqNo = 3;
                                    BK_TRANSADDONInfo.TripMode = 1;
                                    if (transit1 != "")
                                    {
                                        BK_TRANSADDONInfo.CarrierCode = ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[3].LegKey.CarrierCode;
                                        BK_TRANSADDONInfo.FlightNo = ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[3].LegKey.FlightNumber;
                                    }
                                    else
                                    {
                                        BK_TRANSADDONInfo.CarrierCode = ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[2].LegKey.CarrierCode;
                                        BK_TRANSADDONInfo.FlightNo = ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[2].LegKey.FlightNumber;
                                    }
                                    BK_TRANSADDONInfo.Origin = transit2;
                                    BK_TRANSADDONInfo.Destination = return2;
                                    BK_TRANSADDONInfo.BaggageCode = dr["SSRCodeReturnBaggage"].ToString();
                                    BK_TRANSADDONInfo.BaggageAmt = (decimal)dr["PriceConReturnBaggage"];
                                    BK_TRANSADDONInfo.IndicatorBaggage = (int)dr["IndicatorReturnBaggage"];
                                    BK_TRANSADDONInfo.MealCode1 = dr["SSRCodeConReturnMeal"].ToString();
                                    BK_TRANSADDONInfo.MealRate1 = (decimal)dr["PriceConReturnMeal"];
                                    BK_TRANSADDONInfo.MealQty1 = 1;
                                    BK_TRANSADDONInfo.IndicatorMeal = (int)dr["IndicatorConReturnmeal"];
                                    BK_TRANSADDONInfo.MealSubTotal1 = (BK_TRANSADDONInfo.MealRate1 * BK_TRANSADDONInfo.MealQty1);
                                    BK_TRANSADDONInfo.MealTotalAmt = (BK_TRANSADDONInfo.MealSubTotal1);
                                    BK_TRANSADDONInfo.DrinkCode1 = dr["SSRCodeConReturnDrink"].ToString();
                                    BK_TRANSADDONInfo.DrinkRate1 = (decimal)dr["PriceConReturnDrink"];
                                    BK_TRANSADDONInfo.DrinkQty1 = 1;
                                    BK_TRANSADDONInfo.IndicatorDrink = (int)dr["IndicatorConReturnDrink"];
                                    BK_TRANSADDONInfo.DrinkSubTotal1 = (BK_TRANSADDONInfo.DrinkRate1 * BK_TRANSADDONInfo.DrinkQty1);
                                    BK_TRANSADDONInfo.DrinkTotalAmt = (BK_TRANSADDONInfo.DrinkSubTotal1);
                                    BK_TRANSADDONInfo.SportCode = dr["SSRCodeReturnSport"].ToString();
                                    BK_TRANSADDONInfo.SportAmt = (decimal)dr["PriceConReturnSport"];
                                    BK_TRANSADDONInfo.IndicatorSport = (int)dr["IndicatorReturnSport"];
                                    BK_TRANSADDONInfo.KitCode = dr["SSRCodeReturnComfort"].ToString();
                                    BK_TRANSADDONInfo.KitAmt = (decimal)dr["PriceConReturnComfort"];
                                    BK_TRANSADDONInfo.InfantCode = dr["SSRCodeReturnInfant"].ToString();
                                    BK_TRANSADDONInfo.InfantAmt = (decimal)dr["PriceConReturnInfant"];
                                    BK_TRANSADDONInfo.IndicatorInfant = (int)dr["IndicatorReturnInfant"];
                                    BK_TRANSADDONInfo.IndicatorKit = (int)dr["IndicatorReturnComfort"];
                                    BK_TRANSADDONInfo.DutyCode = dr["SSRCodeReturnDuty"].ToString();
                                    BK_TRANSADDONInfo.DutyAmt = (decimal)dr["PriceConReturnDuty"];
                                    BK_TRANSADDONInfo.TotalAmount = (BK_TRANSADDONInfo.BaggageAmt + BK_TRANSADDONInfo.MealSubTotal1 + BK_TRANSADDONInfo.DrinkSubTotal1 + BK_TRANSADDONInfo.SportAmt + BK_TRANSADDONInfo.KitAmt + BK_TRANSADDONInfo.InfantAmt + BK_TRANSADDONInfo.DutyAmt);
                                    BK_TRANSADDONInfo.CreateBy = MyUserSet.UserName;
                                    BK_TRANSADDONInfo.LastSyncBy = MyUserSet.UserName;

                                    listBK_TRANSADDONInfo.Add(BK_TRANSADDONInfo);
                                    listBK_TRANSADDONInfo2t.Add(BK_TRANSADDONInfo);
                                }
                            }
                        }
                    }

                    //Old
                    DataTable dataClassOld = new DataTable();
                    GetPassengerList(TransID, "");
                    dataClassOld = dtPassOld;
                    foreach (DataRow dr in dataClassOld.Rows)
                    {
                        if (dr["SSRCodeDepartBaggage"].ToString() != "" || dr["SSRCodeDepartInfant"].ToString() != "" || dr["SSRCodeDepartMeal"].ToString() != "" || dr["SSRCodeDepartDrink"].ToString() != "" || dr["SSRCodeDepartSport"].ToString() != "" || dr["SSRCodeDepartComfort"].ToString() != "" || dr["SSRCodeDepartDuty"].ToString() != "" || dr["SSRCodeDepartMeal"].ToString() != "" || dr["SSRCodeDepartDrink"].ToString() != "")
                        {
                            BK_TRANSADDONInfo = new Bk_transaddon();
                            BK_TRANSADDONInfo.TransID = dr["TransID"].ToString();
                            BK_TRANSADDONInfo.RecordLocator = dr["PNR"].ToString();
                            BK_TRANSADDONInfo.PassengerID = (Convert.ToInt32(dr["PassengerID"])).ToString();
                            BK_TRANSADDONInfo.Segment = 0;
                            BK_TRANSADDONInfo.SeqNo = 0;
                            BK_TRANSADDONInfo.TripMode = 0;
                            BK_TRANSADDONInfo.CarrierCode = ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.CarrierCode;
                            BK_TRANSADDONInfo.FlightNo = ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.FlightNumber;
                            BK_TRANSADDONInfo.Origin = depart1;

                            if (transit1 != "")
                            {
                                BK_TRANSADDONInfo.Destination = transit1;
                            }
                            else
                            {
                                BK_TRANSADDONInfo.Destination = return1;
                            }
                            BK_TRANSADDONInfo.BaggageCode = dr["SSRCodeDepartBaggage"].ToString();
                            BK_TRANSADDONInfo.BaggageAmt = (decimal)dr["PriceDepartBaggage1"];
                            BK_TRANSADDONInfo.IndicatorBaggage = (int)dr["IndicatorDepartBaggage"];
                            BK_TRANSADDONInfo.MealCode1 = dr["SSRCodeDepartMeal"].ToString();
                            BK_TRANSADDONInfo.MealRate1 = (decimal)dr["PriceDepartMeal"];
                            BK_TRANSADDONInfo.MealQty1 = 1;
                            BK_TRANSADDONInfo.IndicatorMeal = (int)dr["IndicatorDepartMeal"];
                            BK_TRANSADDONInfo.MealSubTotal1 = (BK_TRANSADDONInfo.MealRate1 * BK_TRANSADDONInfo.MealQty1);
                            BK_TRANSADDONInfo.MealTotalAmt = (BK_TRANSADDONInfo.MealSubTotal1);
                            BK_TRANSADDONInfo.DrinkCode1 = dr["SSRCodeDepartDrink"].ToString();
                            BK_TRANSADDONInfo.DrinkRate1 = (decimal)dr["PriceDepartDrink"];
                            BK_TRANSADDONInfo.DrinkQty1 = 1;
                            BK_TRANSADDONInfo.IndicatorDrink = (int)dr["IndicatorDepartDrink"];
                            BK_TRANSADDONInfo.DrinkSubTotal1 = (BK_TRANSADDONInfo.DrinkRate1 * BK_TRANSADDONInfo.DrinkQty1);
                            BK_TRANSADDONInfo.DrinkTotalAmt = (BK_TRANSADDONInfo.DrinkSubTotal1);
                            BK_TRANSADDONInfo.SportCode = dr["SSRCodeDepartSport"].ToString();
                            BK_TRANSADDONInfo.SportAmt = (decimal)dr["PriceDepartSport1"];
                            BK_TRANSADDONInfo.IndicatorSport = (int)dr["IndicatorDepartSport"];
                            BK_TRANSADDONInfo.KitCode = dr["SSRCodeDepartComfort"].ToString();
                            BK_TRANSADDONInfo.IndicatorKit = (int)dr["IndicatorDepartComfort"];
                            BK_TRANSADDONInfo.KitAmt = (decimal)dr["PriceDepartComfort1"];
                            BK_TRANSADDONInfo.InfantCode = dr["SSRCodeDepartInfant"].ToString();
                            BK_TRANSADDONInfo.InfantAmt = (decimal)dr["PriceDepartInfant"];
                            BK_TRANSADDONInfo.IndicatorInfant = (int)dr["IndicatorDepartInfant"];
                            BK_TRANSADDONInfo.DutyCode = dr["SSRCodeDepartDuty"].ToString();
                            BK_TRANSADDONInfo.DutyAmt = (decimal)dr["PriceDepartDuty1"];
                            BK_TRANSADDONInfo.TotalAmount = (BK_TRANSADDONInfo.BaggageAmt + BK_TRANSADDONInfo.MealSubTotal1 + BK_TRANSADDONInfo.DrinkSubTotal1 + BK_TRANSADDONInfo.SportAmt + BK_TRANSADDONInfo.KitAmt + BK_TRANSADDONInfo.InfantAmt + BK_TRANSADDONInfo.DutyAmt);
                            BK_TRANSADDONInfo.CreateBy = MyUserSet.UserName;
                            BK_TRANSADDONInfo.LastSyncBy = MyUserSet.UserName;

                            listBK_TRANSADDONInfo1Old.Add(BK_TRANSADDONInfo);
                            if (transit1 != "")
                            {
                                BK_TRANSADDONInfo = new Bk_transaddon();
                                BK_TRANSADDONInfo.TransID = dr["TransID"].ToString();
                                BK_TRANSADDONInfo.RecordLocator = dr["PNR"].ToString();
                                BK_TRANSADDONInfo.PassengerID = (Convert.ToInt32(dr["PassengerID"])).ToString();
                                BK_TRANSADDONInfo.Segment = 1;
                                BK_TRANSADDONInfo.SeqNo = 1;
                                BK_TRANSADDONInfo.TripMode = 0;
                                BK_TRANSADDONInfo.CarrierCode = ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.CarrierCode;
                                BK_TRANSADDONInfo.FlightNo = ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.FlightNumber;
                                BK_TRANSADDONInfo.Origin = transit1;
                                BK_TRANSADDONInfo.Destination = return1;
                                BK_TRANSADDONInfo.BaggageCode = dr["SSRCodeDepartBaggage"].ToString();
                                BK_TRANSADDONInfo.BaggageAmt = (decimal)dr["PriceConDepartBaggage"];
                                BK_TRANSADDONInfo.IndicatorBaggage = (int)dr["IndicatorDepartBaggage"];
                                BK_TRANSADDONInfo.MealCode1 = dr["SSRCodeConDepartMeal"].ToString();
                                BK_TRANSADDONInfo.MealRate1 = (decimal)dr["PriceConDepartMeal"];
                                BK_TRANSADDONInfo.MealQty1 = 1;
                                BK_TRANSADDONInfo.IndicatorMeal = (int)dr["IndicatorConDepartMeal"];
                                BK_TRANSADDONInfo.MealSubTotal1 = (BK_TRANSADDONInfo.MealRate1 * BK_TRANSADDONInfo.MealQty1);
                                BK_TRANSADDONInfo.MealTotalAmt = (BK_TRANSADDONInfo.MealSubTotal1);
                                BK_TRANSADDONInfo.DrinkCode1 = dr["SSRCodeConDepartDrink"].ToString();
                                BK_TRANSADDONInfo.DrinkRate1 = (decimal)dr["PriceConDepartDrink"];
                                BK_TRANSADDONInfo.DrinkQty1 = 1;
                                BK_TRANSADDONInfo.IndicatorDrink = (int)dr["IndicatorConDepartDrink"];
                                BK_TRANSADDONInfo.DrinkSubTotal1 = (BK_TRANSADDONInfo.DrinkRate1 * BK_TRANSADDONInfo.DrinkQty1);
                                BK_TRANSADDONInfo.DrinkTotalAmt = (BK_TRANSADDONInfo.DrinkSubTotal1);
                                BK_TRANSADDONInfo.SportCode = dr["SSRCodeDepartSport"].ToString();
                                BK_TRANSADDONInfo.SportAmt = (decimal)dr["PriceConDepartSport"];
                                BK_TRANSADDONInfo.IndicatorSport = (int)dr["IndicatorDepartSport"];
                                BK_TRANSADDONInfo.KitCode = dr["SSRCodeDepartComfort"].ToString();
                                BK_TRANSADDONInfo.KitAmt = (decimal)dr["PriceConDepartComfort"];
                                BK_TRANSADDONInfo.InfantCode = dr["SSRCodeDepartInfant"].ToString();
                                BK_TRANSADDONInfo.InfantAmt = (decimal)dr["PriceConDepartInfant"];
                                BK_TRANSADDONInfo.IndicatorInfant = (int)dr["IndicatorDepartInfant"];
                                BK_TRANSADDONInfo.IndicatorKit = (int)dr["IndicatorDepartComfort"];
                                BK_TRANSADDONInfo.IndicatorInfant = (int)dr["IndicatorDepartInfant"];
                                BK_TRANSADDONInfo.DutyCode = dr["SSRCodeDepartDuty"].ToString();
                                BK_TRANSADDONInfo.DutyAmt = (decimal)dr["PriceConDepartDuty"];
                                BK_TRANSADDONInfo.TotalAmount = (BK_TRANSADDONInfo.BaggageAmt + BK_TRANSADDONInfo.MealSubTotal1 + BK_TRANSADDONInfo.DrinkSubTotal1 + BK_TRANSADDONInfo.SportAmt + BK_TRANSADDONInfo.KitAmt + BK_TRANSADDONInfo.InfantAmt + BK_TRANSADDONInfo.DutyAmt);
                                BK_TRANSADDONInfo.CreateBy = MyUserSet.UserName;
                                BK_TRANSADDONInfo.LastSyncBy = MyUserSet.UserName;

                                listBK_TRANSADDONInfo1tOld.Add(BK_TRANSADDONInfo);
                            }
                        }
                    }

                    if ((Boolean)Session["OneWay"] != true)
                    {
                        DataTable dataClass2 = new DataTable();
                        dataClass2 = dtPass2Old;
                        foreach (DataRow dr in dataClass2.Rows)
                        {
                            if (dr["SSRCodeReturnBaggage"].ToString() != "" || dr["SSRCodeReturnInfant"].ToString() != "" || dr["SSRCodeReturnMeal"].ToString() != "" || dr["SSRCodeReturnDrink"].ToString() != "" || dr["SSRCodeReturnSport"].ToString() != "" || dr["SSRCodeReturnComfort"].ToString() != "" || dr["SSRCodeReturnDuty"].ToString() != "" || dr["SSRCodeConReturnMeal"].ToString() != "" || dr["SSRCodeConReturnDrink"].ToString() != "")
                            {
                                BK_TRANSADDONInfo = new Bk_transaddon();
                                BK_TRANSADDONInfo.TransID = dr["TransID"].ToString();
                                BK_TRANSADDONInfo.RecordLocator = dr["PNR"].ToString();
                                BK_TRANSADDONInfo.PassengerID = (Convert.ToInt32(dr["PassengerID"])).ToString();
                                BK_TRANSADDONInfo.Segment = 0;
                                BK_TRANSADDONInfo.SeqNo = 2;
                                BK_TRANSADDONInfo.TripMode = 1;
                                if (transit1 != "")
                                {
                                    BK_TRANSADDONInfo.CarrierCode = ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[2].LegKey.CarrierCode;
                                    BK_TRANSADDONInfo.FlightNo = ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[2].LegKey.FlightNumber;
                                }
                                else
                                {
                                    BK_TRANSADDONInfo.CarrierCode = ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.CarrierCode;
                                    BK_TRANSADDONInfo.FlightNo = ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[1].LegKey.FlightNumber;
                                }
                                BK_TRANSADDONInfo.Origin = depart2;
                                if (transit2 != "")
                                {
                                    BK_TRANSADDONInfo.Destination = transit2;
                                }
                                else
                                {
                                    BK_TRANSADDONInfo.Destination = return2;
                                }
                                BK_TRANSADDONInfo.BaggageCode = dr["SSRCodeReturnBaggage"].ToString();
                                BK_TRANSADDONInfo.BaggageAmt = (decimal)dr["PriceReturnBaggage1"];
                                BK_TRANSADDONInfo.IndicatorBaggage = (int)dr["IndicatorReturnBaggage"];
                                BK_TRANSADDONInfo.MealCode1 = dr["SSRCodeReturnMeal"].ToString();
                                BK_TRANSADDONInfo.MealRate1 = (decimal)dr["PriceReturnMeal"];
                                BK_TRANSADDONInfo.MealQty1 = 1;
                                BK_TRANSADDONInfo.IndicatorMeal = (int)dr["IndicatorReturnMeal"];
                                BK_TRANSADDONInfo.MealSubTotal1 = (BK_TRANSADDONInfo.MealRate1 * BK_TRANSADDONInfo.MealQty1);
                                BK_TRANSADDONInfo.MealTotalAmt = (BK_TRANSADDONInfo.MealSubTotal1);
                                BK_TRANSADDONInfo.DrinkCode1 = dr["SSRCodeReturnDrink"].ToString();
                                BK_TRANSADDONInfo.DrinkRate1 = (decimal)dr["PriceReturnDrink"];
                                BK_TRANSADDONInfo.DrinkQty1 = 1;
                                BK_TRANSADDONInfo.IndicatorDrink = (int)dr["IndicatorReturnDrink"];
                                BK_TRANSADDONInfo.DrinkSubTotal1 = (BK_TRANSADDONInfo.DrinkRate1 * BK_TRANSADDONInfo.DrinkQty1);
                                BK_TRANSADDONInfo.DrinkTotalAmt = (BK_TRANSADDONInfo.DrinkSubTotal1);
                                BK_TRANSADDONInfo.SportCode = dr["SSRCodeReturnSport"].ToString();
                                BK_TRANSADDONInfo.SportAmt = (decimal)dr["PriceReturnSport1"];
                                BK_TRANSADDONInfo.IndicatorSport = (int)dr["IndicatorReturnSport"];
                                BK_TRANSADDONInfo.KitCode = dr["SSRCodeReturnComfort"].ToString();
                                BK_TRANSADDONInfo.KitAmt = (decimal)dr["PriceReturnComfort1"];
                                BK_TRANSADDONInfo.IndicatorKit = (int)dr["IndicatorReturnComfort"];
                                BK_TRANSADDONInfo.InfantCode = dr["SSRCodeReturnInfant"].ToString();
                                BK_TRANSADDONInfo.InfantAmt = (decimal)dr["PriceReturnInfant"];
                                BK_TRANSADDONInfo.IndicatorInfant = (int)dr["IndicatorReturnInfant"];
                                BK_TRANSADDONInfo.DutyCode = dr["SSRCodeReturnDuty"].ToString();
                                BK_TRANSADDONInfo.DutyAmt = (decimal)dr["PriceReturnDuty1"];
                                BK_TRANSADDONInfo.TotalAmount = (BK_TRANSADDONInfo.BaggageAmt + BK_TRANSADDONInfo.MealSubTotal1 + BK_TRANSADDONInfo.DrinkSubTotal1 + BK_TRANSADDONInfo.SportAmt + BK_TRANSADDONInfo.KitAmt + BK_TRANSADDONInfo.InfantAmt + BK_TRANSADDONInfo.DutyAmt);
                                BK_TRANSADDONInfo.CreateBy = MyUserSet.UserName;
                                BK_TRANSADDONInfo.LastSyncBy = MyUserSet.UserName;

                                listBK_TRANSADDONInfo2Old.Add(BK_TRANSADDONInfo);
                                if (transit2 != "")
                                {
                                    BK_TRANSADDONInfo = new Bk_transaddon();
                                    BK_TRANSADDONInfo.TransID = dr["TransID"].ToString();
                                    BK_TRANSADDONInfo.RecordLocator = dr["PNR"].ToString();
                                    BK_TRANSADDONInfo.PassengerID = (Convert.ToInt32(dr["PassengerID"])).ToString();
                                    BK_TRANSADDONInfo.Segment = 1;
                                    BK_TRANSADDONInfo.SeqNo = 3;
                                    BK_TRANSADDONInfo.TripMode = 1;
                                    if (transit1 != "")
                                    {
                                        BK_TRANSADDONInfo.CarrierCode = ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[3].LegKey.CarrierCode;
                                        BK_TRANSADDONInfo.FlightNo = ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[3].LegKey.FlightNumber;
                                    }
                                    else
                                    {
                                        BK_TRANSADDONInfo.CarrierCode = ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[2].LegKey.CarrierCode;
                                        BK_TRANSADDONInfo.FlightNo = ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[2].LegKey.FlightNumber;
                                    }
                                    BK_TRANSADDONInfo.Origin = transit2;
                                    BK_TRANSADDONInfo.Destination = return2;
                                    BK_TRANSADDONInfo.BaggageCode = dr["SSRCodeReturnBaggage"].ToString();
                                    BK_TRANSADDONInfo.BaggageAmt = (decimal)dr["PriceConReturnBaggage"];
                                    BK_TRANSADDONInfo.IndicatorBaggage = (int)dr["IndicatorReturnBaggage"];
                                    BK_TRANSADDONInfo.MealCode1 = dr["SSRCodeConReturnMeal"].ToString();
                                    BK_TRANSADDONInfo.MealRate1 = (decimal)dr["PriceConReturnMeal"];
                                    BK_TRANSADDONInfo.MealQty1 = 1;
                                    BK_TRANSADDONInfo.IndicatorMeal = (int)dr["IndicatorConReturnmeal"];
                                    BK_TRANSADDONInfo.MealSubTotal1 = (BK_TRANSADDONInfo.MealRate1 * BK_TRANSADDONInfo.MealQty1);
                                    BK_TRANSADDONInfo.MealTotalAmt = (BK_TRANSADDONInfo.MealSubTotal1);
                                    BK_TRANSADDONInfo.DrinkCode1 = dr["SSRCodeConReturnDrink"].ToString();
                                    BK_TRANSADDONInfo.DrinkRate1 = (decimal)dr["PriceConReturnDrink"];
                                    BK_TRANSADDONInfo.DrinkQty1 = 1;
                                    BK_TRANSADDONInfo.IndicatorDrink = (int)dr["IndicatorConReturnDrink"];
                                    BK_TRANSADDONInfo.DrinkSubTotal1 = (BK_TRANSADDONInfo.DrinkRate1 * BK_TRANSADDONInfo.DrinkQty1);
                                    BK_TRANSADDONInfo.DrinkTotalAmt = (BK_TRANSADDONInfo.DrinkSubTotal1);
                                    BK_TRANSADDONInfo.SportCode = dr["SSRCodeReturnSport"].ToString();
                                    BK_TRANSADDONInfo.SportAmt = (decimal)dr["PriceConReturnSport"];
                                    BK_TRANSADDONInfo.IndicatorSport = (int)dr["IndicatorReturnSport"];
                                    BK_TRANSADDONInfo.KitCode = dr["SSRCodeReturnComfort"].ToString();
                                    BK_TRANSADDONInfo.KitAmt = (decimal)dr["PriceConReturnComfort"];
                                    BK_TRANSADDONInfo.IndicatorKit = (int)dr["IndicatorReturnComfort"];
                                    BK_TRANSADDONInfo.InfantCode = dr["SSRCodeReturnInfant"].ToString();
                                    BK_TRANSADDONInfo.InfantAmt = (decimal)dr["PriceConReturnInfant"];
                                    BK_TRANSADDONInfo.IndicatorInfant = (int)dr["IndicatorReturnInfant"];
                                    BK_TRANSADDONInfo.DutyCode = dr["SSRCodeReturnDuty"].ToString();
                                    BK_TRANSADDONInfo.DutyAmt = (decimal)dr["PriceConReturnDuty"];
                                    BK_TRANSADDONInfo.TotalAmount = (BK_TRANSADDONInfo.BaggageAmt + BK_TRANSADDONInfo.MealSubTotal1 + BK_TRANSADDONInfo.DrinkSubTotal1 + BK_TRANSADDONInfo.SportAmt + BK_TRANSADDONInfo.KitAmt + BK_TRANSADDONInfo.InfantAmt + BK_TRANSADDONInfo.DutyAmt);
                                    BK_TRANSADDONInfo.CreateBy = MyUserSet.UserName;
                                    BK_TRANSADDONInfo.LastSyncBy = MyUserSet.UserName;

                                    listBK_TRANSADDONInfo2tOld.Add(BK_TRANSADDONInfo);
                                }
                            }
                        }
                    }

                    using (profiler.Step("SellFlight"))
                    {
                        if (SellFlight(listBK_TRANSADDONInfo1, listBK_TRANSADDONInfo2, listBK_TRANSADDONInfo1t, listBK_TRANSADDONInfo2t, listBK_TRANSADDONInfo, listBK_TRANSADDONInfo1Old, listBK_TRANSADDONInfo2Old, listBK_TRANSADDONInfo1tOld, listBK_TRANSADDONInfo2tOld) != "")
                        {
                            e.Result = msgList.Err100055;
                            return;
                        }
                    }
                    e.Result = "";
                }
            }

            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                e.Result = msgList.Err100055;
                log.Error(this, ex);
            }
        }

        private string SellFlight(List<Bk_transaddon> Listing1, List<Bk_transaddon> Listing2, List<Bk_transaddon> Listing1t, List<Bk_transaddon> Listing2t, List<Bk_transaddon> listAll, List<Bk_transaddon> Listing1Old, List<Bk_transaddon> Listing2Old, List<Bk_transaddon> Listing1tOld, List<Bk_transaddon> Listing2tOld)
        {
            MessageList msgList = new MessageList();
            //ClearSSRFeeValue();
            List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
            Session["totalcountpax"] = null;
            Decimal totalSSRdepart = 0;
            Decimal totalSSRReturn = 0;
            Decimal totalInfantdepart = 0;
            Decimal totalInfantReturn = 0;

            depart1 = Session["depart1"].ToString();
            transit1 = Session["transit1"].ToString();
            return1 = Session["return1"].ToString();

            if ((Boolean)Session["OneWay"] != true)
            {
                depart2 = Session["depart2"].ToString();
                transit2 = Session["transit2"].ToString();
                return2 = Session["return2"].ToString();
            }

            decimal TotSSRDepart = 0;
            decimal TotSSRReturn = 0;
            decimal TotSSRDepartcommit = 0;
            decimal TotSSRReturncommit = 0;
            decimal TotInfantDepartcommit = 0;
            decimal TotInfantReturncommit = 0;
            decimal TotInfantDepart = 0;
            decimal TotInfantReturn = 0;

            var profiler = MiniProfiler.Current;
            try
            {
                List<BookingTransactionDetail> listBookingDetail = new List<BookingTransactionDetail>();
                if (listAll != null && listAll.Count > 0)
                {
                    listBookingDetail = objBooking.GetAllBK_TRANSDTLFilterAll(listAll[0].TransID, 0);
                }
                else
                {
                    DataTable dtGridPass = new DataTable();
                    dtGridPass = (DataTable)HttpContext.Current.Session["dtGridPass"];
                    listBookingDetail = objBooking.GetAllBK_TRANSDTLFilterAll(dtGridPass.Rows[0]["TransID"].ToString(), 0);
                }
                List<BookingTransactionDetail> lstBookDTL = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 0, "LEN(RecordLocator) >= 6 AND ");
                List<BookingTransactionDetail> OldlstBookDTL = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 0, "LEN(RecordLocator) >= 6 AND ");
                if (listBookingDetail.Count > 0)
                {
                    int TPax = 0, PaxAdult = 0, PaxChild = 0;
                    String SessionID = "";
                    for (int iii = 0; iii < listBookingDetail.Count; iii++)
                    {
                        if (listBookingDetail[iii].Origin == listBookingDetail[0].Origin)
                        {
                            string PNR = listBookingDetail[iii].RecordLocator.ToString();

                            TPax += Convert.ToInt32(listBookingDetail[iii].TotalPax);
                            PaxAdult += Convert.ToInt32(listBookingDetail[iii].PaxAdult);
                            PaxChild += Convert.ToInt32(listBookingDetail[iii].PaxChild);

                            totalSSRdepart = 0;
                            totalSSRReturn = 0;

                            totalInfantdepart = 0;
                            totalInfantReturn = 0;

                            List<Bk_transaddon> List1 = Listing1.Where(item => item.RecordLocator == listBookingDetail[iii].RecordLocator).ToList();
                            List<Bk_transaddon> List2 = Listing2.Where(item => item.RecordLocator == listBookingDetail[iii].RecordLocator).ToList();
                            List<Bk_transaddon> List1t = Listing1t.Where(item => item.RecordLocator == listBookingDetail[iii].RecordLocator).ToList();
                            List<Bk_transaddon> List2t = Listing2t.Where(item => item.RecordLocator == listBookingDetail[iii].RecordLocator).ToList();

                            List<Bk_transaddon> List1Old = Listing1Old.Where(item => item.RecordLocator == listBookingDetail[iii].RecordLocator).ToList();
                            List<Bk_transaddon> List2Old = Listing2Old.Where(item => item.RecordLocator == listBookingDetail[iii].RecordLocator).ToList();
                            List<Bk_transaddon> List1tOld = Listing1tOld.Where(item => item.RecordLocator == listBookingDetail[iii].RecordLocator).ToList();
                            List<Bk_transaddon> List2tOld = Listing2tOld.Where(item => item.RecordLocator == listBookingDetail[iii].RecordLocator).ToList();


                            ABS.Navitaire.APIBooking APIBooking = new ABS.Navitaire.APIBooking(SessionID);

                            GetSSRAvailabilityForBookingResponse ssrResponse = (GetSSRAvailabilityForBookingResponse)Session["GetssrAvailabilityResponseForBooking"];
                            if (ssrResponse != null)
                            {
                                if (List1.Count > 0 || List1t.Count > 0 || List2.Count > 0 || List2t.Count > 0)
                                {
                                    SessionID = objBooking.SellSSR(listBookingDetail[iii].RecordLocator, ssrResponse, List1, List1t, List2, List2t, List1Old, List1tOld, List2Old, List2tOld);

                                    if (SessionID != "")
                                    {
                                        ABS.Navitaire.BookingManager.Booking book = new ABS.Navitaire.BookingManager.Booking();// APIBooking.GetBookingFromState(SessionID);
                                        using (profiler.Step("Navitaire:GetBookingFromState"))
                                        {
                                            book = APIBooking.GetBookingFromState(SessionID);
                                        }
                                            //string xml = GetXMLString(book);

                                        if (book.BookingSum.BalanceDue > 0)
                                        {
                                            havebalance = 1;
                                            for (int i = 0; i < book.Passengers.Length; i++)
                                            {
                                                for (int ii = 0; ii < book.Passengers[i].PassengerFees.Length; ii++)
                                                {
                                                    if (book.Passengers[i].PassengerFees[ii].FeeType == FeeType.SSRFee && book.Passengers[i].PassengerFees[ii].SSRCode != "INFT")
                                                    {
                                                        if (book.Passengers[i].PassengerFees[ii].FlightReference != "")
                                                        {
                                                            if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 3) == depart1) || (book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == depart1 + transit1) || (book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == transit1 + return1))
                                                            {
                                                                for (int y = 0; y < book.Passengers[i].PassengerFees[ii].ServiceCharges.Length; y++)
                                                                {
                                                                    totalSSRdepart += book.Passengers[i].PassengerFees[ii].ServiceCharges[y].Amount;

                                                                }
                                                            }
                                                            else if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 3) == depart2) || (book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == depart2 + transit2) || (book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == transit2 + return2))
                                                            {
                                                                for (int y = 0; y < book.Passengers[i].PassengerFees[ii].ServiceCharges.Length; y++)
                                                                {
                                                                    totalSSRReturn += book.Passengers[i].PassengerFees[ii].ServiceCharges[y].Amount;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else if (book.Passengers[i].PassengerFees[ii].FeeType == FeeType.SSRFee && book.Passengers[i].PassengerFees[ii].SSRCode == "INFT")
                                                    {
                                                        if (book.Passengers[i].PassengerFees[ii].FlightReference != "")
                                                        {
                                                            if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 3) == depart1) || (book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == depart1 + transit1) || (book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == transit1 + return1))
                                                            {
                                                                for (int y = 0; y < book.Passengers[i].PassengerFees[ii].ServiceCharges.Length; y++)
                                                                {
                                                                    totalInfantdepart += book.Passengers[i].PassengerFees[ii].ServiceCharges[y].Amount;

                                                                }
                                                            }
                                                            else if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 3) == depart2) || (book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == depart2 + transit2) || (book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == transit2 + return2))
                                                            {
                                                                for (int y = 0; y < book.Passengers[i].PassengerFees[ii].ServiceCharges.Length; y++)
                                                                {
                                                                    totalInfantReturn += book.Passengers[i].PassengerFees[ii].ServiceCharges[y].Amount;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            havebalance = 0;
                                        }
                                    }
                                }

                                if (HttpContext.Current.Session["Commit"] != null)
                                {
                                    if (Convert.ToBoolean(Session["Commit"]) == true)
                                    {
                                        TotSSRDepartcommit += totalSSRdepart;
                                        TotSSRReturncommit += totalSSRReturn;
                                        TotInfantDepartcommit += totalInfantdepart;
                                        TotInfantReturncommit += totalInfantReturn;

                                    }
                                    else
                                    {
                                        TotSSRDepart += totalSSRdepart;
                                        TotSSRReturn += totalSSRReturn;
                                        TotInfantDepart += totalInfantdepart;
                                        TotInfantReturn += totalInfantReturn;
                                    }
                                }


                                if (havebalance == 1)
                                {

                                    if (listAll != null && listAll.Count > 0)
                                    {
                                        if ((Boolean)Session["OneWay"] != true)
                                        {
                                            int iIndexDepart = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 1 && p.RecordLocator == PNR);
                                            int iIndexReturn = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 0 && p.RecordLocator == PNR);
                                            if (iIndexDepart >= 0)
                                            {
                                                listBookingDetail[iIndexDepart].TransID = listAll[0].TransID;
                                                listBookingDetail[iIndexDepart].RecordLocator = PNR;
                                                listBookingDetail[iIndexDepart].Signature = SessionID;
                                                decimal totalsum = totalSSRdepart;
                                                decimal totalamountdue = totalsum - listBookingDetail[iIndexDepart].LineSSR;
                                                decimal totalsuminfant = totalInfantdepart;
                                                decimal totalamountdueinfant = totalsuminfant - listBookingDetail[iIndexDepart].LineInfant;
                                                listBookingDetail[iIndexDepart].LineSSR += totalamountdue;
                                                listBookingDetail[iIndexDepart].LineInfant += totalamountdueinfant;
                                                listBookingDetail[iIndexDepart].LineTotal += totalamountdue;
                                                listBookingDetail[iIndexDepart].LineTotal += totalamountdueinfant;
                                                //listBookingDetail[iIndexDepart].PayDueAmount2 += totalamountdue;

                                                if (HttpContext.Current.Session["Commit"] != null)
                                                {
                                                    if (Convert.ToBoolean(Session["Commit"]) == true)
                                                    {
                                                        totalamountduegoingcommit += totalamountdue;
                                                        totalamountduegoingcommitinfant += totalamountdueinfant;
                                                    }
                                                    else
                                                    {
                                                        totalamountduegoing += totalamountdue;
                                                        totalamountduegoinginfant += totalamountdueinfant;
                                                    }
                                                }
                                            }
                                            if (iIndexReturn >= 0)
                                            {
                                                listBookingDetail[iIndexReturn].TransID = listAll[0].TransID;
                                                listBookingDetail[iIndexReturn].RecordLocator = PNR;
                                                listBookingDetail[iIndexReturn].Signature = SessionID;
                                                decimal totalsum = totalSSRReturn;
                                                decimal totalamountdue = totalsum - listBookingDetail[iIndexReturn].LineSSR ;
                                                decimal totalsuminfant = totalInfantReturn;
                                                decimal totalamountdueinfant = totalsuminfant - listBookingDetail[iIndexReturn].LineInfant;
                                                listBookingDetail[iIndexReturn].LineSSR += totalamountdue;
                                                listBookingDetail[iIndexReturn].LineInfant += totalamountdueinfant;
                                                listBookingDetail[iIndexReturn].LineTotal += totalamountdue;
                                                listBookingDetail[iIndexReturn].LineTotal += totalamountdueinfant;
                                                //listBookingDetail[iIndexReturn].PayDueAmount2 += totalamountdue;

                                                if (HttpContext.Current.Session["Commit"] != null)
                                                {
                                                    if (Convert.ToBoolean(Session["Commit"]) == true)
                                                    {
                                                        totalamountduereturncommit += totalamountdue;
                                                        totalamountduereturncommitinfant += totalamountdueinfant;
                                                    }
                                                    else
                                                    {
                                                        totalamountduereturn += totalamountdue;
                                                        totalamountduereturninfant += totalamountdueinfant;
                                                    }
                                                }
                                            }

                                        }
                                        else
                                        {
                                            int iIndexDepart = listBookingDetail.FindIndex(p => p.RecordLocator == PNR);
                                            if (iIndexDepart >= 0)
                                            {
                                                listBookingDetail[iIndexDepart].TransID = listAll[0].TransID;
                                                listBookingDetail[iIndexDepart].RecordLocator = PNR;
                                                listBookingDetail[iIndexDepart].Signature = SessionID;
                                                decimal totalsum = totalSSRdepart;
                                                decimal totalamountdue = totalsum - listBookingDetail[iIndexDepart].LineSSR;
                                                decimal totalsuminfant = totalInfantdepart;
                                                decimal totalamountdueinfant = totalsuminfant - listBookingDetail[iIndexDepart].LineInfant;
                                                listBookingDetail[iIndexDepart].LineSSR += totalamountdue;
                                                listBookingDetail[iIndexDepart].LineInfant += totalamountdueinfant;
                                                listBookingDetail[iIndexDepart].LineTotal += totalamountdue;
                                                listBookingDetail[iIndexDepart].LineTotal += totalamountdueinfant;
                                                //listBookingDetail[iIndexDepart].PayDueAmount2 += totalamountdue;

                                                if (HttpContext.Current.Session["Commit"] != null)
                                                {
                                                    if (Convert.ToBoolean(Session["Commit"]) == true)
                                                    {
                                                        totalamountduegoingcommit += totalamountdue;
                                                        totalamountduegoingcommit += totalamountdueinfant;
                                                    }
                                                    else
                                                    {
                                                        totalamountduegoing += totalamountdue;
                                                        totalamountduegoing += totalamountdueinfant;
                                                    }
                                                }
                                            }
                                        }

                                        HttpContext.Current.Session.Remove("listBookingDetail");
                                        HttpContext.Current.Session.Add("listBookingDetail", listBookingDetail);

                                        int cnt = 0;
                                        if (HttpContext.Current.Session["Commit"] != null)
                                        {
                                            if (Convert.ToBoolean(Session["Commit"]) == true)
                                            {
                                                if (objBooking.UpdateManage(listAll, PNR, "", true) == false)
                                                {
                                                    log.Error(this, "save BK_TRANSADDON failed = " + listAll[0].TransID);
                                                    return msgList.Err999999;
                                                }
                                            }
                                            else
                                            {
                                                HttpContext.Current.Session.Remove("ChgTransSSR");
                                                HttpContext.Current.Session.Add("ChgTransSSR", listAll);

                                                foreach (BookingTransactionDetail bkDTL in lstBookDTL)
                                                {
                                                    if (PNR == bkDTL.RecordLocator)
                                                    {
                                                        if ((Boolean)Session["OneWay"] != true)
                                                        {
                                                            decimal totalsum = totalSSRdepart + totalSSRReturn;
                                                            decimal totalamountdue = totalsum - bkDTL.LineSSR;
                                                            decimal totalsuminfant = totalInfantdepart + totalInfantReturn;
                                                            decimal totalamountdueinfant = totalsuminfant - bkDTL.LineInfant;
                                                            bkDTL.LineSSR += totalamountdue;
                                                            bkDTL.LineInfant += totalamountdueinfant;
                                                            bkDTL.LineTotal += totalamountdue;
                                                            bkDTL.LineTotal += totalamountdueinfant;
                                                        }
                                                        else
                                                        {
                                                            decimal totalsum = totalSSRdepart;
                                                            decimal totalamountdue = totalsum - bkDTL.LineSSR;
                                                            decimal totalsuminfant = totalInfantdepart;
                                                            decimal totalamountdueinfant = totalsuminfant - bkDTL.LineInfant;
                                                            bkDTL.LineSSR += totalamountdue;
                                                            bkDTL.LineInfant += totalamountdueinfant;
                                                            bkDTL.LineTotal += totalamountdue;
                                                            bkDTL.LineTotal += totalamountdueinfant;
                                                        }
                                                    }
                                                    //}
                                                }
                                                objBooking.FillChgTransDetail(lstBookDTL, OldlstBookDTL);

                                                objBK_TRANSDTL_Infos = new BookingTransactionDetail();
                                                objBK_TRANSDTL_Infos.RecordLocator = PNR;
                                                objBK_TRANSDTL_Infos.Signature = SessionID;
                                                objListBK_TRANSDTL_Infos.Add(objBK_TRANSDTL_Infos);
                                                HttpContext.Current.Session["objListBK_TRANSDTL_Infos"] = objListBK_TRANSDTL_Infos;
                                            }
                                        }
                                        else
                                        {

                                            foreach (BookingTransactionDetail bkDTL in lstBookDTL)
                                            {
                                                if (PNR == bkDTL.RecordLocator)
                                                {
                                                    if ((Boolean)Session["OneWay"] != true)
                                                    {
                                                        decimal totalsum = totalSSRdepart + totalSSRReturn;
                                                        decimal totalamountdue = totalsum - bkDTL.LineSSR;
                                                        decimal totalsuminfant = totalInfantdepart + totalInfantReturn;
                                                        decimal totalamountdueinfant = totalsuminfant - bkDTL.LineInfant;
                                                        bkDTL.LineSSR += totalamountdue;
                                                        bkDTL.LineInfant += totalamountdueinfant;
                                                        bkDTL.LineTotal += totalamountdue;
                                                        bkDTL.LineTotal += totalamountdueinfant;
                                                    }
                                                    else
                                                    {
                                                        decimal totalsum = totalSSRdepart;
                                                        decimal totalamountdue = totalsum - bkDTL.LineSSR;
                                                        decimal totalsuminfant = totalInfantdepart;
                                                        decimal totalamountdueinfant = totalsuminfant - bkDTL.LineInfant;
                                                        bkDTL.LineSSR += totalamountdue;
                                                        bkDTL.LineInfant += totalamountdueinfant;
                                                        bkDTL.LineTotal += totalamountdue;
                                                        bkDTL.LineTotal += totalamountdueinfant;
                                                    }
                                                }
                                                //}
                                            }
                                            objBooking.FillChgTransDetail(lstBookDTL);

                                            objBK_TRANSDTL_Infos = new BookingTransactionDetail();
                                            objBK_TRANSDTL_Infos.RecordLocator = PNR;
                                            objBK_TRANSDTL_Infos.Signature = SessionID;
                                            objListBK_TRANSDTL_Infos.Add(objBK_TRANSDTL_Infos);
                                            HttpContext.Current.Session["objListBK_TRANSDTL_Infos"] = objListBK_TRANSDTL_Infos;

                                        }
                                    }
                                }
                            }
                        }

                        //UpdateTotalAmount(TotSSRDepart, TotSSRReturn, ref TotalAmountGoing, ref TotalAmountReturn, TPax, PaxAdult, PaxChild, listBookingDetail);
                    }

                }
                //int cnt = 0;
                if (totalamountduegoing != 0 || totalamountduereturn != 0 || totalamountduegoinginfant != 0 || totalamountduereturninfant != 0)
                {
                    objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();

                    if (listAll != null && listAll.Count > 0)
                    {
                        if (TotSSRDepartcommit != 0 || TotSSRReturncommit != 0 || TotInfantDepartcommit != 0 || TotInfantReturncommit != 0)
                        {
                            BookingTransactionMain bookingMain = new BookingTransactionMain();
                            bookingMain = objBooking.GetSingleBK_TRANSMAIN(listAll[0].TransID);
                            decimal totalsum = TotSSRDepartcommit + TotSSRReturncommit;
                            decimal totalamountdue = (totalamountduegoingcommit + totalamountduereturncommit);
                            decimal totalsuminfant = TotInfantDepartcommit + TotInfantReturncommit;
                            decimal totalamountdueinfant = (totalamountduegoingcommitinfant + totalamountduereturncommitinfant);
                            bookingMain.TransTotalSSR = bookingMain.TransTotalSSR + totalamountdue;
                            bookingMain.TransTotalInfant = bookingMain.TransTotalInfant + totalamountdueinfant;
                            bookingMain.TotalAmtGoing = bookingMain.TotalAmtGoing + totalamountduegoingcommit + totalamountduegoingcommitinfant;
                            bookingMain.TotalAmtReturn = bookingMain.TotalAmtReturn + totalamountduereturncommit + totalamountduereturncommitinfant;
                            bookingMain.TransSubTotal = bookingMain.TotalAmtGoing + bookingMain.TotalAmtReturn;
                            bookingMain.TransTotalAmt = bookingMain.TotalAmtGoing + bookingMain.TotalAmtReturn;
                            objBooking.UpdateTotalSSR(listAll[0].TransID, bookingMain, listBookingDetail);

                            if (TotSSRDepart == 0 && TotSSRReturn == 0 && TotInfantDepart == 0 && TotInfantReturn == 0)
                            {
                                Response.RedirectLocation = "bookingdetail.aspx?k=" + Request.QueryString["k"] + "&TransID=" + Request.QueryString["TransID"];
                            }
                            else
                            {
                                //totalsum = TotSSRDepart + TotSSRReturn;
                                totalamountdue = (totalamountduegoing + totalamountduereturn);
                                totalamountdueinfant = (totalamountduegoinginfant + totalamountduereturninfant);
                                bookingMain.TransTotalSSR = bookingMain.TransTotalSSR + totalamountdue;
                                bookingMain.TransTotalInfant = bookingMain.TransTotalInfant + totalamountdueinfant;
                                bookingMain.TotalAmtGoing = (bookingMain.TotalAmtGoing + totalamountduegoing + totalamountduegoinginfant);
                                bookingMain.TotalAmtReturn = (bookingMain.TotalAmtReturn + totalamountduereturn + totalamountduereturninfant);
                                bookingMain.TransSubTotal = bookingMain.TotalAmtGoing + bookingMain.TotalAmtReturn;
                                bookingMain.TransTotalAmt = bookingMain.TotalAmtGoing + bookingMain.TotalAmtReturn;

                                objBooking.FillChgTransMain(bookingMain);
                                HttpContext.Current.Session.Remove("bookingMain");
                                HttpContext.Current.Session.Add("bookingMain", bookingMain);

                                Session["ChgMode"] = "2"; //1= Manage Add-On

                                if (Session["lstPassInfantData"] != null) lstPassInfantData = (List<PassengerData>)Session["lstPassInfantData"];
                                if (lstPassInfantData.Count > 0)
                                {
                                    HttpContext.Current.Session.Add("lstPassInfantData", lstPassInfantData);
                                    //objBooking.SaveBK_PASSENGERLIST(lstPassInfantData, CoreBase.EnumSaveType.Insert);
                                }
                                ASPxCallback.RedirectOnCallback(GroupBooking.Web.Shared.MySite.PublicPages.Payment);
                            }
                        }
                        else
                        {
                            BookingTransactionMain bookingMain = new BookingTransactionMain();
                            bookingMain = objBooking.GetSingleBK_TRANSMAIN(listAll[0].TransID);
                            //decimal totalsum = (TotSSRDepart + TotSSRReturn);
                            decimal totalamountdue = (totalamountduegoing + totalamountduereturn);
                            decimal totalamountdueinfant = (totalamountduegoinginfant + totalamountduereturninfant);
                            bookingMain.TransTotalSSR = bookingMain.TransTotalSSR + totalamountdue;
                            bookingMain.TransTotalInfant = bookingMain.TransTotalInfant + totalamountdueinfant;
                            bookingMain.TotalAmtGoing = (bookingMain.TotalAmtGoing + totalamountduegoing + totalamountduegoinginfant);
                            bookingMain.TotalAmtReturn = (bookingMain.TotalAmtReturn + totalamountduereturn + totalamountduereturninfant);
                            bookingMain.TransSubTotal = bookingMain.TotalAmtGoing + bookingMain.TotalAmtReturn;
                            bookingMain.TransTotalAmt = bookingMain.TotalAmtGoing + bookingMain.TotalAmtReturn;

                            objBooking.FillChgTransMain(bookingMain);
                            HttpContext.Current.Session.Remove("bookingMain");
                            HttpContext.Current.Session.Add("bookingMain", bookingMain);

                            Session["ChgMode"] = "2"; //1= Manage Add-On

                            if (Session["lstPassInfantData"] != null) lstPassInfantData = (List<PassengerData>)Session["lstPassInfantData"];
                            if (lstPassInfantData.Count > 0)
                            {
                                HttpContext.Current.Session.Add("lstPassInfantData", lstPassInfantData);
                                //objBooking.SaveBK_PASSENGERLIST(lstPassInfantData, CoreBase.EnumSaveType.Insert);
                            }
                            ASPxCallback.RedirectOnCallback(GroupBooking.Web.Shared.MySite.PublicPages.Payment);
                        }
                    }

                }
                else
                {
                    if (TotSSRDepartcommit != 0 || TotSSRReturncommit != 0 || TotInfantDepartcommit != 0 || TotInfantReturncommit != 0)
                    {
                        BookingTransactionMain bookingMain = new BookingTransactionMain();
                        bookingMain = objBooking.GetSingleBK_TRANSMAIN(listAll[0].TransID);
                        //decimal totalsum = TotSSRDepartcommit + TotSSRReturncommit;
                        decimal totalamountdue = (totalamountduegoingcommit + totalamountduereturncommit);
                        decimal totalamountdueinfant = (totalamountduegoingcommitinfant + totalamountduereturncommitinfant);
                        bookingMain.TransTotalSSR = bookingMain.TransTotalSSR + totalamountdue;
                        bookingMain.TransTotalInfant = bookingMain.TransTotalInfant + totalamountdueinfant;
                        bookingMain.TotalAmtGoing = bookingMain.TotalAmtGoing + totalamountduegoingcommit + totalamountduegoingcommitinfant;
                        bookingMain.TotalAmtReturn = bookingMain.TotalAmtReturn + totalamountduereturncommit + totalamountduereturncommitinfant;
                        bookingMain.TransSubTotal = bookingMain.TotalAmtGoing + bookingMain.TotalAmtReturn;
                        bookingMain.TransTotalAmt = bookingMain.TotalAmtGoing + bookingMain.TotalAmtReturn;
                        if (bookingMain.PaymentAmtEx3 > 0)
                        {
                            bookingMain.PaymentAmtEx3 = bookingMain.PaymentAmtEx3 + totalamountdue;
                        }
                        else if (bookingMain.PaymentAmtEx2 > 0)
                        {
                            bookingMain.PaymentAmtEx2 = bookingMain.PaymentAmtEx2 + totalamountdue;
                        }
                        else
                        {
                            bookingMain.PaymentAmtEx1 = bookingMain.PaymentAmtEx1 + totalamountdue;
                        }

                        if (listBookingDetail[0].PayDueAmount3 > 0)
                        {
                            listBookingDetail[0].PayDueAmount3 += totalamountdue;
                        }
                        else if (listBookingDetail[0].PayDueAmount2 > 0)
                        {
                            listBookingDetail[0].PayDueAmount2 += totalamountdue;

                        }
                        else
                        {
                            listBookingDetail[0].PayDueAmount1 += totalamountdue;
                        }
                        objBooking.SaveHeaderDetail(bookingMain, listBookingDetail, CoreBase.EnumSaveType.Update);
                        //objBooking.UpdateTotalSSR(listAll[0].TransID, bookingMain, listBookingDetail);
                    }

                    if (Session["lstPassInfantData"] != null) lstPassInfantData = (List<PassengerData>)Session["lstPassInfantData"];
                    if (lstPassInfantData.Count > 0)
                    {
                        //HttpContext.Current.Session.Add("lstPassInfantData", lstPassInfantData);
                        objBooking.SaveBK_PASSENGERLIST(lstPassInfantData, CoreBase.EnumSaveType.Insert);
                    }
                    Response.RedirectLocation = "bookingdetail.aspx?k=" + Request.QueryString["k"] + "&TransID=" + Request.QueryString["TransID"];
                }
                return "";
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                return msgList.Err999999;
            }
        }

        //private string GetXMLString(object Obj)
        //{
        //    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Obj.GetType());
        //    System.IO.StringWriter writer = new System.IO.StringWriter();
        //    x.Serialize(writer, Obj);

        //    return writer.ToString();
        //}
        public static bool IsNumeric(string s)
        {
            double result;
            return s != null && Double.TryParse(s, out result);
        }
        #endregion

        #region BatchEdit
        protected void gvPassenger_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            MessageList msgList = new MessageList();
            string ssr = "";
            object ssrvalue;
            //Thread.Sleep(2000);
            var profiler = MiniProfiler.Current;
            try
            {
                if (infantmax > 10) infantmax = 10;
                using (profiler.Step("AssignValues"))
                {
                    foreach (var args in e.UpdateValues)
                    {
                        DataTable data = Session["dtGridPass"] as DataTable;
                        DataTable data2 = new DataTable();
                        if (Session["dtGridPass2"] != null)
                        {
                            data2 = Session["dtGridPass2"] as DataTable;
                            data2.PrimaryKey = new DataColumn[] { (data2.Columns["PassengerID"]), (data2.Columns["SeqNo"]) };
                        }
                        data.PrimaryKey = new DataColumn[] { (data.Columns["PassengerID"]), (data.Columns["SeqNo"]) };
                        // Create an array for the key values to find.
                        object[] findTheseVals = new object[2];

                        // Set the values of the keys to find.
                        findTheseVals[0] = (args.Keys["PassengerID"]);
                        findTheseVals[1] = (args.Keys["SeqNo"]);

                        DataRow row = data.Rows.Find(findTheseVals);
                        DataRow row2 = data2.NewRow();
                        if (data2 != null && data2.Rows.Count > 0)
                        {
                            row2 = data2.Rows.Find(findTheseVals);
                        }
                        row["PassengerID"] = args.Keys["PassengerID"];
                        row["SeqNo"] = args.Keys["SeqNo"];

                        int countInfant = Convert.ToInt16(data.Compute("Count(DepartInfant)", "DepartInfant <> ''"));

                        string[] SSRColumns = new string[] { "Baggage", "Sport", "Infant" };
                        for (int p = 0; p < SSRColumns.Length; p++)
                        {
                            string SSRColumn = SSRColumns[p].ToString();
                            if (args.NewValues[SSRColumns[p]] != null && args.NewValues[SSRColumns[p]].ToString() != "" && SSRColumn == "Infant" && (countInfant == infantmax) && args.OldValues[SSRColumns[p]].ToString() == "")
                            {
                                custommessage = "";
                                custommessage = msgList.Err100051.Replace("infantmax", infantmax.ToString());
                                gvPassenger.JSProperties["cp_result"] = custommessage;// "Maximum Infant is " + infantmax + ". " + msgList.Err100051;
                                Session["InfantMax"] = true;
                            }
                            else
                            {
                                AssignValues(row, row2, args.NewValues["Depart" + SSRColumns[p]], ref SSRColumn, "Depart", gvPassenger);
                            }
                        }

                        string[] SSRColumnMeal = new string[] { "DepartMeal", "ConDepartMeal" };
                        for (int p = 0; p < SSRColumnMeal.Length; p++)
                        {
                            string SSRColumn = SSRColumnMeal[p].ToString();
                            if (p == 0)
                            {
                                ssrvalue = args.NewValues["DepartDrink"];
                                ssr = "DepartDrink";
                            }
                            else
                            {
                                ssrvalue = args.NewValues["ConDepartDrink"];
                                ssr = "ConDepartDrink";
                            }

                            AssignValueMeal(row, args.NewValues[SSRColumnMeal[p]], ref SSRColumn, ssrvalue, ref ssr, "Depart", gvPassenger);
                        }

                        string[] SSRColumnDrink = new string[] { "DepartDrink", "ConDepartDrink" };
                        for (int p = 0; p < SSRColumnDrink.Length; p++)
                        {
                            string SSRColumn = SSRColumnDrink[p].ToString();
                            if (p == 0)
                            {
                                ssrvalue = args.NewValues["DepartMeal"];
                                ssr = "DepartMeal";
                            }
                            else
                            {
                                ssrvalue = args.NewValues["ConDepartMeal"];
                                ssr = "ConDepartMeal";
                            }

                            AssignValueDrink(row, args.NewValues[SSRColumnDrink[p]], ref SSRColumn, ssrvalue, ref ssr, "Depart", gvPassenger);
                        }

                        string[] SSRColumns2 = new string[] { "Comfort", "Duty" };
                        for (int p = 0; p < SSRColumns2.Length; p++)
                        {
                            string SSRColumn = SSRColumns2[p].ToString();
                            AssignValue2(row, args.NewValues["Depart" + SSRColumns2[p]], ref SSRColumn, "Depart", gvPassenger);
                        }


                        Session["dtGridPass"] = data;
                        if (data2 != null && data2.Rows.Count > 0)
                        {
                            Session["dtGridPass2"] = data2;
                        }

                    }
                }
                e.Handled = true;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
            finally
            {
                //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Script", "gvPassenger_EndCallback();", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "Script", "gvPassenger_EndCallback();", true);
                gvPassenger.DataSource = Session["dtGridPass"];
                gvPassenger.DataBind();
                //GetPassengerList(TransID, "Depart");
                //ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "gvPassenger_EndCallback();", true);

            }
        }

        protected void gvPassenger2_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            string ssr = "";
            object ssrvalue;
            Thread.Sleep(2000);
            try
            {
                if (infantmax > 10) infantmax = 10;
                foreach (var args in e.UpdateValues)
                {
                    DataTable data = Session["dtGridPass2"] as DataTable;
                    data.PrimaryKey = new DataColumn[] { (data.Columns["PassengerID"]), (data.Columns["SeqNo"]) };
                    DataTable data1 = Session["dtGridPass"] as DataTable;
                    data1.PrimaryKey = new DataColumn[] { (data1.Columns["PassengerID"]), (data1.Columns["SeqNo"]) };
                    // Create an array for the key values to find.
                    object[] findTheseVals = new object[2];

                    // Set the values of the keys to find.
                    findTheseVals[0] = (args.Keys["PassengerID"]);
                    findTheseVals[1] = (args.Keys["SeqNo"]);

                    DataRow row = data.Rows.Find(findTheseVals);
                    DataRow row1 = data1.Rows.Find(findTheseVals);
                    row["PassengerID"] = args.Keys["PassengerID"];
                    row["SeqNo"] = args.Keys["SeqNo"];

                    int countInfant = Convert.ToInt16(data.Compute("Count(ReturnInfant)", "ReturnInfant <> ''"));
                    string[] SSRColumns = new string[] { "Baggage", "Sport", "Infant" };
                    for (int p = 0; p < SSRColumns.Length; p++)
                    {
                        string SSRColumn = SSRColumns[p].ToString();
                        AssignValues(row, row1, args.NewValues["Return" + SSRColumns[p]], ref SSRColumn, "Return", gvPassenger2);
                    }

                    string[] SSRColumnMeal = new string[] { "ReturnMeal", "ConReturnMeal" };
                    for (int p = 0; p < SSRColumnMeal.Length; p++)
                    {
                        string SSRColumn = SSRColumnMeal[p].ToString();
                        if (p == 0)
                        {
                            ssrvalue = args.NewValues["ReturnDrink"];
                            ssr = "ReturnDrink";
                        }
                        else
                        {
                            ssrvalue = args.NewValues["ConReturnDrink"];
                            ssr = "ConReturnDrink";
                        }

                        AssignValueMeal(row, args.NewValues[SSRColumnMeal[p]], ref SSRColumn, ssrvalue, ref ssr, "Return", gvPassenger2);
                    }

                    string[] SSRColumnDrink = new string[] { "ReturnDrink", "ConReturnDrink" };
                    for (int p = 0; p < SSRColumnDrink.Length; p++)
                    {
                        string SSRColumn = SSRColumnDrink[p].ToString();
                        if (p == 0)
                        {
                            ssrvalue = args.NewValues["ReturnMeal"];
                            ssr = "ReturnMeal";
                        }
                        else
                        {
                            ssrvalue = args.NewValues["ConReturnMeal"];
                            ssr = "ConReturnMeal";
                        }

                        AssignValueDrink(row, args.NewValues[SSRColumnDrink[p]], ref SSRColumn, ssrvalue, ref ssr, "Return", gvPassenger2);
                    }

                    string[] SSRColumns2 = new string[] { "Comfort", "Duty" };
                    for (int p = 0; p < SSRColumns2.Length; p++)
                    {
                        string SSRColumn = SSRColumns2[p].ToString();
                        AssignValue2(row, args.NewValues["Return" + SSRColumns2[p]], ref SSRColumn, "Return", gvPassenger2);
                    }


                    Session["dtGridPass2"] = data;
                    Session["dtGridPass"] = data1;
                }

                e.Handled = true;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
            finally
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Script", "gvPassenger2_EndCallback();", true);
                gvPassenger2.DataSource = Session["dtGridPass2"];
                gvPassenger2.DataBind();
                //GetPassengerList(TransID, "Return");
            }
        }


        protected void AssignValues(DataRow row, DataRow row2, object NewValues, ref string SSRColumns, string Flight, ASPxGridView grid)
        {
            MessageList msgList = new MessageList();
            int count = 0;
            String Detail = "";
            String subvalue = "";
            if (NewValues != null && NewValues != "")
            {
                DataTable dtBaggage = Session["dt" + SSRColumns + Flight] as DataTable;
                DataTable dtInfantDepart = Session["dtInfantDepart"] as DataTable;
                DataTable dtInfantReturn = Session["dtInfantReturn"] as DataTable;
                if (NewValues.ToString().Length == 4)
                {
                    if (row[Flight + SSRColumns] != null && row[Flight + SSRColumns].ToString() != "")
                    {
                        DataRow[] resultBaggage = dtBaggage.Select("SSRCode = '" + NewValues + "'");
                        foreach (DataRow rows in resultBaggage)
                        {
                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                            if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["Price" + Flight + SSRColumns]) && row["SSRCode" + Flight + SSRColumns].ToString().Trim() == rows[0].ToString().Trim())
                            {
                                continue;
                                //return;
                            }
                            else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["Price" + Flight + SSRColumns]))
                            {
                                custommessage = "";
                                custommessage = msgList.Err800008.Replace("SSRColumns", SSRColumns);
                                grid.JSProperties["cp_result"] = custommessage;// "Cannot replace " + SSRColumns + " with cheaper fee, please try again";
                                count = 1;
                                //return;
                            }
                            else
                            {
                                row[Flight + SSRColumns] = rows["ConcatenatedField"];
                                row["Price" + Flight + SSRColumns] = Convert.ToDecimal(Detail);
                                row["Price" + Flight + SSRColumns + "1"] = Convert.ToDecimal(rows[3]);
                                row["PriceCon" + Flight + SSRColumns] = Convert.ToDecimal(rows[4]);
                                row["SSRCode" + Flight + SSRColumns] = rows["SSRCode"];
                                row["Indicator" + Flight + SSRColumns] = "1";

                                if (NewValues.ToString() == "INFT" && row2.ItemArray.Length > 0)
                                {
                                    if (Flight == "Depart")
                                    {
                                        DataRow[] resultInfantDepart = dtInfantDepart.Select("SSRCode = '" + NewValues + "'");
                                        foreach (DataRow rows1 in resultInfantDepart)
                                        {
                                            row["Depart" + SSRColumns] = rows1["ConcatenatedField"];
                                            Detail = rows1[2].ToString().Substring(0, rows1[2].ToString().Length - 4);
                                            row["PriceDepart" + SSRColumns] = Convert.ToDecimal(Detail);
                                            row["PriceDepart" + SSRColumns + "1"] = Convert.ToDecimal(rows1[3]);
                                            row["PriceConDepart" + SSRColumns] = Convert.ToDecimal(rows1[4]);
                                            row["SSRCodeDepart" + SSRColumns] = rows1["SSRCode"];
                                            row["IndicatorDepart" + SSRColumns] = "1";
                                            // row["SSRCode" + SSRColumns] = rows["SSRCode"];
                                            //gvPassenger.JSProperties["cp_result"] = "Infant";

                                        }

                                        DataRow[] resultInfantReturn = dtInfantReturn.Select("SSRCode = '" + NewValues + "'");
                                        foreach (DataRow rows2 in resultInfantReturn)
                                        {
                                            row2["Return" + SSRColumns] = rows2["ConcatenatedField"];
                                            Detail = rows2[2].ToString().Substring(0, rows2[2].ToString().Length - 4);
                                            row["PriceReturn" + SSRColumns] = Convert.ToDecimal(Detail);
                                            row["PriceReturn" + SSRColumns + "1"] = Convert.ToDecimal(rows2[3]);
                                            row["PriceConReturn" + SSRColumns] = Convert.ToDecimal(rows2[4]);
                                            row["SSRCodeReturn" + SSRColumns] = rows2["SSRCode"];
                                            row["IndicatorReturn" + SSRColumns] = "1";
                                            gvPassenger.JSProperties["cp_result"] = "Infant";
                                        }
                                    }
                                    else
                                    {
                                        DataRow[] resultInfantReturn = dtInfantReturn.Select("SSRCode = '" + NewValues + "'");
                                        foreach (DataRow rows1 in resultInfantReturn)
                                        {
                                            row["Return" + SSRColumns] = rows1["ConcatenatedField"];
                                            Detail = rows1[2].ToString().Substring(0, rows1[2].ToString().Length - 4);
                                            row["PriceReturn" + SSRColumns] = Convert.ToDecimal(Detail);
                                            row["PriceReturn" + SSRColumns + "1"] = Convert.ToDecimal(rows1[3]);
                                            row["PriceConReturn" + SSRColumns] = Convert.ToDecimal(rows1[4]);
                                            row["SSRCodeReturn" + SSRColumns] = rows1["SSRCode"];
                                            row["IndicatorReturn" + SSRColumns] = "1";
                                            //gvPassenger2.JSProperties["cp_result"] = "Infant";

                                        }

                                        DataRow[] resultInfantDepart = dtInfantDepart.Select("SSRCode = '" + NewValues + "'");
                                        foreach (DataRow rows2 in resultInfantDepart)
                                        {
                                            row2["Depart" + SSRColumns] = rows2["ConcatenatedField"];
                                            Detail = rows2[2].ToString().Substring(0, rows2[2].ToString().Length - 4);
                                            row["PriceDepart" + SSRColumns] = Convert.ToDecimal(Detail);
                                            row["PriceDepart" + SSRColumns + "1"] = Convert.ToDecimal(rows2[3]);
                                            row["PriceConDepart" + SSRColumns] = Convert.ToDecimal(rows2[4]);
                                            row["SSRCodeDepart" + SSRColumns] = rows2["SSRCode"];
                                            row["IndicatorDepart" + SSRColumns] = "1";
                                            gvPassenger2.JSProperties["cp_result"] = "Infant";
                                        }
                                    }
                                }
                            }
                        }


                    }
                    else
                    {
                        DataRow[] resultBaggage = dtBaggage.Select("SSRCode = '" + NewValues + "'");
                        foreach (DataRow rows in resultBaggage)
                        {
                            row[Flight + SSRColumns] = rows["ConcatenatedField"];
                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                            row["Price" + Flight + SSRColumns] = Convert.ToDecimal(Detail);
                            row["Price" + Flight + SSRColumns + "1"] = Convert.ToDecimal(rows[3]);
                            row["PriceCon" + Flight + SSRColumns] = Convert.ToDecimal(rows[4]);
                            row["SSRCode" + Flight + SSRColumns] = rows["SSRCode"];
                            row["Indicator" + Flight + SSRColumns] = "1";
                        }

                        if (NewValues.ToString() == "INFT" && row2.ItemArray.Length > 0)
                        {
                            if (Flight == "Depart")
                            {
                                DataRow[] resultInfantDepart = dtInfantDepart.Select("SSRCode = '" + NewValues + "'");
                                foreach (DataRow rows1 in resultInfantDepart)
                                {
                                    row["Depart" + SSRColumns] = rows1["ConcatenatedField"];
                                    Detail = rows1[2].ToString().Substring(0, rows1[2].ToString().Length - 4);
                                    row["PriceDepart" + SSRColumns] = Convert.ToDecimal(Detail);
                                    row["PriceDepart" + SSRColumns + "1"] = Convert.ToDecimal(rows1[3]);
                                    row["PriceConDepart" + SSRColumns] = Convert.ToDecimal(rows1[4]);
                                    row["SSRCodeDepart" + SSRColumns] = rows1["SSRCode"];
                                    row["IndicatorDepart" + SSRColumns] = "1";
                                    // row["SSRCode" + SSRColumns] = rows["SSRCode"];
                                    //gvPassenger.JSProperties["cp_result"] = "Infant";

                                }

                                DataRow[] resultInfantReturn = dtInfantReturn.Select("SSRCode = '" + NewValues + "'");
                                foreach (DataRow rows2 in resultInfantReturn)
                                {
                                    row2["Return" + SSRColumns] = rows2["ConcatenatedField"];
                                    Detail = rows2[2].ToString().Substring(0, rows2[2].ToString().Length - 4);
                                    row["PriceReturn" + SSRColumns] = Convert.ToDecimal(Detail);
                                    row["PriceReturn" + SSRColumns + "1"] = Convert.ToDecimal(rows2[3]);
                                    row["PriceConReturn" + SSRColumns] = Convert.ToDecimal(rows2[4]);
                                    row["SSRCodeReturn" + SSRColumns] = rows2["SSRCode"];
                                    row["IndicatorReturn" + SSRColumns] = "1";
                                    gvPassenger.JSProperties["cp_result"] = "Infant";
                                }
                            }
                            else
                            {
                                DataRow[] resultInfantReturn = dtInfantReturn.Select("SSRCode = '" + NewValues + "'");
                                foreach (DataRow rows1 in resultInfantReturn)
                                {
                                    row["Return" + SSRColumns] = rows1["ConcatenatedField"];
                                    Detail = rows1[2].ToString().Substring(0, rows1[2].ToString().Length - 4);
                                    row["PriceReturn" + SSRColumns] = Convert.ToDecimal(Detail);
                                    row["PriceReturn" + SSRColumns + "1"] = Convert.ToDecimal(rows1[3]);
                                    row["PriceConReturn" + SSRColumns] = Convert.ToDecimal(rows1[4]);
                                    row["SSRCodeReturn" + SSRColumns] = rows1["SSRCode"];
                                    row["IndicatorReturn" + SSRColumns] = "1";
                                    //gvPassenger2.JSProperties["cp_result"] = "Infant";

                                }

                                DataRow[] resultInfantDepart = dtInfantDepart.Select("SSRCode = '" + NewValues + "'");
                                foreach (DataRow rows2 in resultInfantDepart)
                                {
                                    row2["Depart" + SSRColumns] = rows2["ConcatenatedField"];
                                    Detail = rows2[2].ToString().Substring(0, rows2[2].ToString().Length - 4);
                                    row["PriceDepart" + SSRColumns] = Convert.ToDecimal(Detail);
                                    row["PriceDepart" + SSRColumns + "1"] = Convert.ToDecimal(rows2[3]);
                                    row["PriceConDepart" + SSRColumns] = Convert.ToDecimal(rows2[4]);
                                    row["SSRCodeDepart" + SSRColumns] = rows2["SSRCode"];
                                    row["IndicatorDepart" + SSRColumns] = "1";
                                    gvPassenger2.JSProperties["cp_result"] = "Infant";
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (row[Flight + SSRColumns] != null && row[Flight + SSRColumns].ToString() != "")
                    {
                        if (SSRColumns == "Baggage") subvalue = NewValues.ToString().Substring(0, 20);
                        else if (SSRColumns == "Sport") subvalue = subvalue = NewValues.ToString().Substring(0, 21);
                        else if (SSRColumns == "Infant") subvalue = subvalue = NewValues.ToString().Substring(0, 6);
                        DataRow[] resultBaggage = dtBaggage.Select("Detail = '" + subvalue + "'");
                        foreach (DataRow rows in resultBaggage)
                        {
                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                            if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["Price" + Flight + SSRColumns]) && row["SSRCode" + Flight + SSRColumns].ToString().Trim() == rows[0].ToString().Trim())
                            {
                                continue;
                                //return;
                            }
                            else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["Price" + Flight + SSRColumns]))
                            {
                                custommessage = "";
                                custommessage = msgList.Err800008.Replace("SSRColumns", SSRColumns);
                                grid.JSProperties["cp_result"] = custommessage;// "Cannot replace " + SSRColumns + " with cheaper fee, please try again";
                                count = 1;
                                //return;
                            }
                            else
                            {
                                if (row["SSRCode" + Flight + SSRColumns].ToString().Trim() != rows["SSRCode"].ToString().Trim())
                                {
                                    row[Flight + SSRColumns] = rows["ConcatenatedField"];
                                    row["Price" + Flight + SSRColumns] = Convert.ToDecimal(Detail);
                                    row["Price" + Flight + SSRColumns + "1"] = Convert.ToDecimal(rows[3]);
                                    row["PriceCon" + Flight + SSRColumns] = Convert.ToDecimal(rows[4]);
                                    row["SSRCode" + Flight + SSRColumns] = rows["SSRCode"];
                                    row["Indicator" + Flight + SSRColumns] = "1";
                                }

                                if (NewValues.ToString() == "INFT" && row2.ItemArray.Length > 0)
                                {
                                    if (Flight == "Depart")
                                    {
                                        DataRow[] resultInfantDepart = dtInfantDepart.Select("SSRCode = '" + NewValues + "'");
                                        foreach (DataRow rows1 in resultInfantDepart)
                                        {
                                            row["Depart" + SSRColumns] = rows1["ConcatenatedField"];
                                            Detail = rows1[2].ToString().Substring(0, rows1[2].ToString().Length - 4);
                                            row["PriceDepart" + SSRColumns] = Convert.ToDecimal(Detail);
                                            row["PriceDepart" + SSRColumns + "1"] = Convert.ToDecimal(rows1[3]);
                                            row["PriceConDepart" + SSRColumns] = Convert.ToDecimal(rows1[4]);
                                            row["SSRCodeDepart" + SSRColumns] = rows1["SSRCode"];
                                            row["IndicatorDepart" + SSRColumns] = "1";
                                            // row["SSRCode" + SSRColumns] = rows["SSRCode"];
                                            //gvPassenger.JSProperties["cp_result"] = "Infant";

                                        }

                                        DataRow[] resultInfantReturn = dtInfantReturn.Select("SSRCode = '" + NewValues + "'");
                                        foreach (DataRow rows2 in resultInfantReturn)
                                        {
                                            row2["Return" + SSRColumns] = rows2["ConcatenatedField"];
                                            Detail = rows2[2].ToString().Substring(0, rows2[2].ToString().Length - 4);
                                            row["PriceReturn" + SSRColumns] = Convert.ToDecimal(Detail);
                                            row["PriceReturn" + SSRColumns + "1"] = Convert.ToDecimal(rows2[3]);
                                            row["PriceConReturn" + SSRColumns] = Convert.ToDecimal(rows2[4]);
                                            row["SSRCodeReturn" + SSRColumns] = rows2["SSRCode"];
                                            row["IndicatorReturn" + SSRColumns] = "1";
                                            gvPassenger.JSProperties["cp_result"] = "Infant";
                                        }
                                    }
                                    else
                                    {
                                        DataRow[] resultInfantReturn = dtInfantReturn.Select("SSRCode = '" + NewValues + "'");
                                        foreach (DataRow rows1 in resultInfantReturn)
                                        {
                                            row["Return" + SSRColumns] = rows1["ConcatenatedField"];
                                            Detail = rows1[2].ToString().Substring(0, rows1[2].ToString().Length - 4);
                                            row["PriceReturn" + SSRColumns] = Convert.ToDecimal(Detail);
                                            row["PriceReturn" + SSRColumns + "1"] = Convert.ToDecimal(rows1[3]);
                                            row["PriceConReturn" + SSRColumns] = Convert.ToDecimal(rows1[4]);
                                            row["SSRCodeReturn" + SSRColumns] = rows1["SSRCode"];
                                            row["IndicatorReturn" + SSRColumns] = "1";
                                            //gvPassenger2.JSProperties["cp_result"] = "Infant";

                                        }

                                        DataRow[] resultInfantDepart = dtInfantDepart.Select("SSRCode = '" + NewValues + "'");
                                        foreach (DataRow rows2 in resultInfantDepart)
                                        {
                                            row2["Depart" + SSRColumns] = rows2["ConcatenatedField"];
                                            Detail = rows2[2].ToString().Substring(0, rows2[2].ToString().Length - 4);
                                            row["PriceDepart" + SSRColumns] = Convert.ToDecimal(Detail);
                                            row["PriceDepart" + SSRColumns + "1"] = Convert.ToDecimal(rows2[3]);
                                            row["PriceConDepart" + SSRColumns] = Convert.ToDecimal(rows2[4]);
                                            row["SSRCodeDepart" + SSRColumns] = rows2["SSRCode"];
                                            row["IndicatorDepart" + SSRColumns] = "1";
                                            gvPassenger2.JSProperties["cp_result"] = "Infant";
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (SSRColumns == "Baggage") subvalue = NewValues.ToString().Substring(0, 20);
                        else if (SSRColumns == "Sport") subvalue = subvalue = NewValues.ToString().Substring(0, 21);
                        else if (SSRColumns == "Infant") subvalue = subvalue = NewValues.ToString().Substring(0, 6);
                        DataRow[] resultBaggage = dtBaggage.Select("Detail = '" + subvalue + "'");
                        foreach (DataRow rows in resultBaggage)
                        {
                            if (row["SSRCode" + Flight + SSRColumns].ToString().Trim() != rows["SSRCode"].ToString().Trim())
                            {
                                row[Flight + SSRColumns] = rows["ConcatenatedField"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["Price" + Flight + SSRColumns] = Convert.ToDecimal(Detail);
                                row["Price" + Flight + SSRColumns + "1"] = Convert.ToDecimal(rows[3]);
                                row["PriceCon" + Flight + SSRColumns] = Convert.ToDecimal(rows[4]);
                                row["SSRCode" + Flight + SSRColumns] = rows["SSRCode"];
                                row["Indicator" + Flight + SSRColumns] = "1";
                            }
                        }

                        if (NewValues.ToString() == "INFT" && row2.ItemArray.Length > 0)
                        {
                            if (Flight == "Depart")
                            {
                                DataRow[] resultInfantDepart = dtInfantDepart.Select("SSRCode = '" + NewValues + "'");
                                foreach (DataRow rows1 in resultInfantDepart)
                                {
                                    row["Depart" + SSRColumns] = rows1["ConcatenatedField"];
                                    Detail = rows1[2].ToString().Substring(0, rows1[2].ToString().Length - 4);
                                    row["PriceDepart" + SSRColumns] = Convert.ToDecimal(Detail);
                                    row["PriceDepart" + SSRColumns + "1"] = Convert.ToDecimal(rows1[3]);
                                    row["PriceConDepart" + SSRColumns] = Convert.ToDecimal(rows1[4]);
                                    row["SSRCodeDepart" + SSRColumns] = rows1["SSRCode"];
                                    row["IndicatorDepart" + SSRColumns] = "1";
                                    // row["SSRCode" + SSRColumns] = rows["SSRCode"];
                                    //gvPassenger.JSProperties["cp_result"] = "Infant";

                                }

                                DataRow[] resultInfantReturn = dtInfantReturn.Select("SSRCode = '" + NewValues + "'");
                                foreach (DataRow rows2 in resultInfantReturn)
                                {
                                    row2["Return" + SSRColumns] = rows2["ConcatenatedField"];
                                    Detail = rows2[2].ToString().Substring(0, rows2[2].ToString().Length - 4);
                                    row["PriceReturn" + SSRColumns] = Convert.ToDecimal(Detail);
                                    row["PriceReturn" + SSRColumns + "1"] = Convert.ToDecimal(rows2[3]);
                                    row["PriceConReturn" + SSRColumns] = Convert.ToDecimal(rows2[4]);
                                    row["SSRCodeReturn" + SSRColumns] = rows2["SSRCode"];
                                    row["IndicatorReturn" + SSRColumns] = "1";
                                    gvPassenger.JSProperties["cp_result"] = "Infant";
                                }
                            }
                            else
                            {
                                DataRow[] resultInfantReturn = dtInfantReturn.Select("SSRCode = '" + NewValues + "'");
                                foreach (DataRow rows1 in resultInfantReturn)
                                {
                                    row["Return" + SSRColumns] = rows1["ConcatenatedField"];
                                    Detail = rows1[2].ToString().Substring(0, rows1[2].ToString().Length - 4);
                                    row["PriceReturn" + SSRColumns] = Convert.ToDecimal(Detail);
                                    row["PriceReturn" + SSRColumns + "1"] = Convert.ToDecimal(rows1[3]);
                                    row["PriceConReturn" + SSRColumns] = Convert.ToDecimal(rows1[4]);
                                    row["SSRCodeReturn" + SSRColumns] = rows1["SSRCode"];
                                    row["IndicatorReturn" + SSRColumns] = "1";
                                    //gvPassenger2.JSProperties["cp_result"] = "Infant";

                                }

                                DataRow[] resultInfantDepart = dtInfantDepart.Select("SSRCode = '" + NewValues + "'");
                                foreach (DataRow rows2 in resultInfantDepart)
                                {
                                    row2["Depart" + SSRColumns] = rows2["ConcatenatedField"];
                                    Detail = rows2[2].ToString().Substring(0, rows2[2].ToString().Length - 4);
                                    row["PriceDepart" + SSRColumns] = Convert.ToDecimal(Detail);
                                    row["PriceDepart" + SSRColumns + "1"] = Convert.ToDecimal(rows2[3]);
                                    row["PriceConDepart" + SSRColumns] = Convert.ToDecimal(rows2[4]);
                                    row["SSRCodeDepart" + SSRColumns] = rows2["SSRCode"];
                                    row["IndicatorDepart" + SSRColumns] = "1";
                                    gvPassenger2.JSProperties["cp_result"] = "Infant";
                                }
                            }
                        }
                    }
                }
            }

            SSRColumns = "";
        }

        protected void AssignValueMeal(DataRow row, object NewValues, ref string SSRColumns, object NewValuesDrink, ref string Drink, string Flight, ASPxGridView grid)
        {
            MessageList msgList = new MessageList();
            int count = 0;
            String Detail = "";
            if (NewValues != null && NewValues != "")
            {

                DataTable dtMeal = new DataTable();
                DataTable dtDrink = new DataTable();
                if (SSRColumns == "Con" + Flight + "Meal")
                {
                    dtMeal = Session["dtMeal" + Flight + "2"] as DataTable;
                    dtDrink = Session["dtDrink" + Flight + "2"] as DataTable;
                }
                else
                {
                    dtMeal = Session["dtMeal" + Flight] as DataTable;
                    dtDrink = Session["dtDrink" + Flight] as DataTable;
                }
                if (NewValues.ToString().Length == 4)
                {
                    if (row[SSRColumns] != null && row[SSRColumns].ToString() != "")
                    {
                        DataRow[] resultMeal = dtMeal.Select("SSRCode = '" + NewValues + "'");
                        foreach (DataRow rows in resultMeal)
                        {
                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                            if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["Price" + SSRColumns]) && row[SSRColumns].ToString().Trim() == rows[1].ToString().Trim())
                            {
                                continue;
                                //return;
                            }
                            else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["Price" + SSRColumns]) && row[SSRColumns].ToString().Trim() != rows[1].ToString().Trim())
                            {
                                custommessage = "";
                                custommessage = msgList.Err800008.Replace("SSRColumns", SSRColumns);
                                grid.JSProperties["cp_result"] = custommessage;// "Cannot replace " + SSRColumns + " with cheaper fee, please try again";
                                count = 1;
                                //return;
                            }
                            else
                            {
                                if (row["SSRCode" + SSRColumns].ToString().Trim() != rows["SSRCode"].ToString().Trim())
                                {
                                    row[SSRColumns] = rows["Detail"];
                                    row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                    row["SSRCode" + SSRColumns] = rows["SSRCode"];
                                    row["Indicator" + SSRColumns] = "1";
                                    if (NewValuesDrink == null || NewValuesDrink == "")
                                    {
                                        for (int c = 0; c < dtDrink.Rows.Count; c++)
                                        {

                                            row[Drink] = dtDrink.Rows[0]["ConcatenatedField"];
                                            Detail = dtDrink.Rows[0]["Price"].ToString().Substring(0, dtDrink.Rows[0]["Price"].ToString().Length - 4);
                                            row["Price" + Drink] = Convert.ToDecimal(Detail);
                                            row["SSRCode" + Drink] = dtDrink.Rows[0]["SSRCode"];
                                            row["Indicator" + Drink] = "1";
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }

                        }
                    }
                    else
                    {
                        DataRow[] resultMeal = dtMeal.Select("SSRCode = '" + NewValues + "'");
                        foreach (DataRow rows in resultMeal)
                        {
                            if (row["SSRCode" + SSRColumns].ToString().Trim() != rows["SSRCode"].ToString().Trim())
                            {
                                row[SSRColumns] = rows["Detail"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                row["SSRCode" + SSRColumns] = rows["SSRCode"];
                                row["Indicator" + SSRColumns] = "1";
                            }
                        }


                        if (NewValuesDrink == null || NewValuesDrink == "")
                        {
                            //if (Session["defaultdrink"] != null)
                            // {
                            //DataTable DefaultDrink = (DataTable)Session["defaultdrink"];
                            for (int c = 0; c < dtDrink.Rows.Count; c++)
                            {
                                //DataRow[] resultDrink = dtDrink.Select("SSRCode LIKE '" + DefaultDrink.Rows[c]["SYSValueEx"] + "%'");
                                //if (resultDrink.Length > 0)
                                // {
                                //foreach (DataRow rowz in resultDrink)
                                //{
                                row[Drink] = dtDrink.Rows[0]["ConcatenatedField"];
                                Detail = dtDrink.Rows[0]["Price"].ToString().Substring(0, dtDrink.Rows[0]["Price"].ToString().Length - 4);
                                row["Price" + Drink] = Convert.ToDecimal(Detail);
                                row["SSRCode" + Drink] = dtDrink.Rows[0]["SSRCode"];
                                row["Indicator" + Drink] = "1";
                                //}
                                break;
                                //}
                            }
                            //}
                        }
                    }
                }
                else
                {
                    if (row[SSRColumns] != null && row[SSRColumns].ToString() != "")
                    {
                        string tmp = NewValues.ToString();
                        DataRow[] resultMeal = new DataRow[1];
                        if (tmp.Contains("'") != false)
                        {
                            resultMeal = dtMeal.Select("Detail LIKE '%" + tmp.Substring(0, tmp.IndexOf("'")) + "%'");
                        }
                        else if (tmp.Contains(",") != false)
                        {
                            resultMeal = dtMeal.Select("Detail LIKE '%" + tmp.Substring(0, tmp.IndexOf(",")) + "%'");
                        }
                        else
                        {
                            resultMeal = dtMeal.Select("Detail LIKE '%" + tmp + "%'");
                        }

                        foreach (DataRow rows in resultMeal)
                        {
                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                            if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["Price" + SSRColumns]) && row[SSRColumns].ToString().Trim() == rows[1].ToString().Trim())
                            {
                                continue;
                                //return;
                            }
                            else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["Price" + SSRColumns]) && row[SSRColumns].ToString().Trim() != rows[1].ToString().Trim())
                            {
                                custommessage = "";
                                custommessage = msgList.Err800008.Replace("SSRColumns", SSRColumns);
                                grid.JSProperties["cp_result"] = custommessage;// "Cannot replace " + SSRColumns + " with cheaper fee, please try again";
                                count = 1;
                                //return;
                            }
                            else
                            {
                                if (row["SSRCode" + SSRColumns].ToString().Trim() != rows["SSRCode"].ToString().Trim())
                                {
                                    row[SSRColumns] = rows["Detail"];
                                    row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                    row["SSRCode" + SSRColumns] = rows["SSRCode"];
                                    row["Indicator" + SSRColumns] = "1";
                                    if (NewValuesDrink == "" || NewValuesDrink == null)
                                    {
                                        //if (Session["defaultdrink"] != null)
                                        //{
                                        //DataTable DefaultDrink = (DataTable)Session["defaultdrink"];
                                        for (int c = 0; c < dtDrink.Rows.Count; c++)
                                        {
                                            // DataRow[] resultDrink = dtDrink.Select("SSRCode LIKE '" + DefaultDrink.Rows[c]["SYSValueEx"] + "%'");
                                            //if (resultDrink.Length > 0)
                                            //{
                                            //foreach (DataRow rowz in resultDrink)
                                            //{
                                            row[Drink] = dtDrink.Rows[0]["ConcatenatedField"];
                                            Detail = dtDrink.Rows[0]["Price"].ToString().Substring(0, dtDrink.Rows[0]["Price"].ToString().Length - 4);
                                            row["Price" + Drink] = Convert.ToDecimal(Detail);
                                            row["SSRCode" + Drink] = dtDrink.Rows[0]["SSRCode"];
                                            row["Indicator" + Drink] = "1";
                                            //}
                                            break;
                                            //}
                                        }
                                        //}
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }

                        }
                    }
                    else
                    {
                        string tmp = NewValues.ToString();
                        DataRow[] resultMeal = new DataRow[1];
                        if (tmp.Contains("'") != false)
                        {
                            resultMeal = dtMeal.Select("Detail LIKE '%" + tmp.Substring(0, tmp.IndexOf("'")) + "%'");
                        }
                        else if (tmp.Contains(",") != false)
                        {
                            resultMeal = dtMeal.Select("Detail LIKE '%" + tmp.Substring(0, tmp.IndexOf(",")) + "%'");
                        }
                        else
                        {
                            resultMeal = dtMeal.Select("Detail LIKE '%" + tmp + "%'");
                        }

                        foreach (DataRow rows in resultMeal)
                        {
                            if (row["SSRCode" + SSRColumns].ToString().Trim() != rows["SSRCode"].ToString().Trim())
                            {
                                row[SSRColumns] = rows["Detail"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                row["SSRCode" + SSRColumns] = rows["SSRCode"];
                                row["Indicator" + SSRColumns] = "1";
                            }
                        }

                        if (NewValuesDrink == "" || NewValuesDrink == null)
                        {
                            //if (Session["defaultdrink"] != null)
                            //{
                            //DataTable DefaultDrink = (DataTable)Session["defaultdrink"];
                            for (int c = 0; c < dtDrink.Rows.Count; c++)
                            {
                                //DataRow[] resultDrink = dtDrink.Select("SSRCode LIKE '" + DefaultDrink.Rows[c]["SYSValueEx"] + "%'");
                                //if (resultDrink.Length > 0)
                                //{
                                //foreach (DataRow rowz in resultDrink)
                                //{
                                row[Drink] = dtDrink.Rows[0]["ConcatenatedField"];
                                Detail = dtDrink.Rows[0]["Price"].ToString().Substring(0, dtDrink.Rows[0]["Price"].ToString().Length - 4);
                                row["Price" + Drink] = Convert.ToDecimal(Detail);
                                row["SSRCode" + Drink] = dtDrink.Rows[0]["SSRCode"];
                                row["Indicator" + Drink] = "1";
                                //}
                                break;
                                //}
                            }
                            //}
                        }
                    }
                }

            }

            SSRColumns = "";
        }

        protected void AssignValueDrink(DataRow row, object NewValues, ref string SSRColumns, object NewValuesMeal, ref string Meal, string Flight, ASPxGridView grid)
        {
            MessageList msgList = new MessageList();
            int count = 0;
            String Detail = "";
            if (NewValues != "" && NewValues != null)
            {
                DataTable dtMeal = new DataTable();
                DataTable dtDrink = new DataTable();
                if (SSRColumns == "Con" + Flight + "Drink")
                {
                    dtMeal = Session["dtMeal" + Flight + "2"] as DataTable;
                    dtDrink = Session["dtDrink" + Flight + "2"] as DataTable;
                }
                else
                {
                    dtMeal = Session["dtMeal" + Flight] as DataTable;
                    dtDrink = Session["dtDrink" + Flight] as DataTable;
                }
                if (NewValues.ToString().Length == 4)
                {
                    if (row[SSRColumns] != null && row[SSRColumns].ToString() != "")
                    {
                        DataRow[] resultSport = dtDrink.Select("SSRCode = '" + NewValues + "'");
                        foreach (DataRow rows in resultSport)
                        {
                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                            if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["Price" + SSRColumns]))
                            {
                                custommessage = "";
                                custommessage = msgList.Err800008.Replace("SSRColumns", SSRColumns);
                                grid.JSProperties["cp_result"] = custommessage;// "Cannot replace " + SSRColumns + " with cheaper fee, please try again";
                                count = 1;
                                //return;
                            }
                            else
                            {
                                if (row["SSRCode" + SSRColumns].ToString().Trim() != rows["SSRCode"].ToString().Trim())
                                {
                                    row[SSRColumns] = rows["ConcatenatedField"];
                                    row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                    row["SSRCode" + SSRColumns] = rows["SSRCode"];
                                    row["Indicator" + SSRColumns] = "1";
                                    if (NewValuesMeal == "" || NewValuesMeal == null)
                                    {
                                        DataRow[] resultMeals = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                                        foreach (DataRow rowz in resultMeals)
                                        {
                                            row[Meal] = rowz["Detail"];
                                            Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                            row["Price" + Meal] = Convert.ToDecimal(Detail);
                                            row["SSRCode" + Meal] = rowz["SSRCode"];
                                            row["Indicator" + SSRColumns] = "1";
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {

                        DataRow[] resultSport = dtDrink.Select("SSRCode = '" + NewValues + "'");
                       // DataRow[] resultSport = dtDrink.Select("SSRCode = '" + NewValuesMeal + "'");
                        foreach (DataRow rows in resultSport)
                        {
                            if (row["SSRCode" + SSRColumns].ToString().Trim() != rows["SSRCode"].ToString().Trim())
                            {
                                row[SSRColumns] = rows["ConcatenatedField"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                row["SSRCode" + SSRColumns] = rows["SSRCode"];
                                row["Indicator" + SSRColumns] = "1";
                            }
                        }

                        if (NewValuesMeal == "" || NewValuesMeal == null)
                        {
                            DataRow[] resultMeals = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                            foreach (DataRow rowz in resultMeals)
                            {
                                row[Meal] = rowz["Detail"];
                                Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                row["Price" + Meal] = Convert.ToDecimal(Detail);
                                row["SSRCode" + Meal] = rowz["SSRCode"];
                                row["Indicator" + Meal] = "1";
                            }
                        }
                    }
                }
                else
                {
                    if (row[SSRColumns] != null && row[SSRColumns].ToString() != "")
                    {
                        string tmp = NewValues.ToString();
                        DataRow[] resultMeal = new DataRow[1];
                        if (tmp.Contains("'") != false)
                        {
                            resultMeal = dtMeal.Select("Detail LIKE '%" + tmp.Substring(0, tmp.IndexOf("'")) + "%'");
                        }
                        else if (tmp.Contains(",") != false)
                        {
                            resultMeal = dtMeal.Select("Detail LIKE '%" + tmp.Substring(0, tmp.IndexOf(",")) + "%'");
                        }
                        else
                        {
                            resultMeal = dtMeal.Select("Detail LIKE '%" + tmp + "%'");
                        }
                        //DataRow[] resultMeal = dtSport.Select("Detail = '" + args.NewValues["DepartDrink"].ToString().Substring(0, 21) + "'");
                        foreach (DataRow rows in resultMeal)
                        {
                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                            if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["Price" + SSRColumns]))
                            {
                                custommessage = "";
                                custommessage = msgList.Err800008.Replace("SSRColumns", SSRColumns);
                                grid.JSProperties["cp_result"] = custommessage;// "Cannot replace " + SSRColumns + " with cheaper fee, please try again";
                                count = 1;
                                //return;
                            }
                            else
                            {
                                if (row["SSRCode" + SSRColumns].ToString().Trim() != rows["SSRCode"].ToString().Trim())
                                {
                                    row[SSRColumns] = rows["ConcatenatedField"];
                                    row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                    row["SSRCode" + SSRColumns] = rows["SSRCode"];
                                    row["Indicator" + SSRColumns] = "1";
                                    if (NewValuesMeal == "" || NewValuesMeal == null)
                                    {
                                        DataRow[] resultMeals = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                                        foreach (DataRow rowz in resultMeals)
                                        {
                                            row[Meal] = rowz["Detail"];
                                            Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                            row["Price" + Meal] = Convert.ToDecimal(Detail);
                                            row["SSRCode" + Meal] = rowz["SSRCode"];
                                            row["Indicator" + Meal] = "1";
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        string tmp = NewValues.ToString();
                        DataRow[] resultMeal = new DataRow[1];
                        if (tmp.Contains("'") != false)
                        {
                            resultMeal = dtMeal.Select("Detail LIKE '%" + tmp.Substring(0, tmp.IndexOf("'")) + "%'");
                        }
                        else if (tmp.Contains(",") != false)
                        {
                            resultMeal = dtMeal.Select("Detail LIKE '%" + tmp.Substring(0, tmp.IndexOf(",")) + "%'");
                        }
                        else
                        {
                            resultMeal = dtMeal.Select("Detail LIKE '%" + tmp + "%'");
                        }

                        foreach (DataRow rows in resultMeal)
                        {
                            if (row["SSRCode" + SSRColumns].ToString().Trim() != rows["SSRCode"].ToString().Trim())
                            {
                                row[SSRColumns] = rows["ConcatenatedField"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                row["SSRCode" + SSRColumns] = rows["SSRCode"];
                                row["Indicator" + SSRColumns] = "1";
                            }
                        }

                        if (NewValuesMeal == "" || NewValuesMeal == null)
                        {
                            DataRow[] resultMeals = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                            foreach (DataRow rowz in resultMeals)
                            {
                                row[Meal] = rowz["Detail"];
                                Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                row["Price" + Meal] = Convert.ToDecimal(Detail);
                                row["SSRCode" + Meal] = rowz["SSRCode"];
                                row["Indicator" + Meal] = "1";
                            }
                        }
                    }
                }
            }

            SSRColumns = "";
        }

        protected void AssignValue2(DataRow row, object NewValues, ref string SSRColumns, string Flight, ASPxGridView grid)
        {
            MessageList msgList = new MessageList();
            int count = 0;
            String Detail = "";
            if (NewValues != null && NewValues != "")
            {
                //row["SSRCodeDepartComfort"] = args.NewValues["DepartComfort"];
                DataTable dtComfort = Session["dt" + SSRColumns + Flight] as DataTable;
                if (NewValues.ToString().Length == 4)
                {
                    if (row[Flight + SSRColumns] != null && row[Flight + SSRColumns].ToString() != "")
                    {
                        DataRow[] resultComfort = dtComfort.Select("SSRCode = '" + NewValues + "'");
                        foreach (DataRow rows in resultComfort)
                        {
                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                            if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["Price" + Flight + SSRColumns]) && row["SSRCode" + Flight + SSRColumns].ToString().Trim() == rows[0].ToString().Trim())
                            {
                                continue;
                                //return;
                            }
                            else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["Price" + Flight + SSRColumns]))
                            {
                                custommessage = "";
                                custommessage = msgList.Err800008.Replace("SSRColumns", SSRColumns);
                                grid.JSProperties["cp_result"] = custommessage;// "Cannot replace " + SSRColumns + " with cheaper fee, please try again";
                                count = 1;
                                //return;
                            }
                            else
                            {
                                if (row["SSRCode" + Flight + SSRColumns].ToString().Trim() != rows["SSRCode"].ToString().Trim())
                                {
                                    row[Flight + SSRColumns] = rows["Detail"];
                                    row["Price" + Flight + SSRColumns] = Convert.ToDecimal(Detail);
                                    row["Price" + Flight + SSRColumns + "1"] = Convert.ToDecimal(rows[3]);
                                    row["PriceCon" + Flight + SSRColumns] = Convert.ToDecimal(rows[4]);
                                    row["SSRCode" + Flight + SSRColumns] = rows["SSRCode"];
                                    row["Indicator" + Flight + SSRColumns] = "1";
                                }
                            }
                        }
                    }
                    else
                    {
                        DataRow[] resultComfort = dtComfort.Select("SSRCode = '" + NewValues + "'");
                        foreach (DataRow rows in resultComfort)
                        {
                            if (row["SSRCode" + Flight + SSRColumns].ToString().Trim() != rows["SSRCode"].ToString().Trim())
                            {
                                row[Flight + SSRColumns] = rows["Detail"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["Price" + Flight + SSRColumns] = Convert.ToDecimal(Detail);
                                row["Price" + Flight + SSRColumns + "1"] = Convert.ToDecimal(rows[3]);
                                row["PriceCon" + Flight + SSRColumns] = Convert.ToDecimal(rows[4]);
                                row["SSRCode" + Flight + SSRColumns] = rows["SSRCode"];
                                row["Indicator" + Flight + SSRColumns] = "1";
                            }
                        }
                    }
                }
                else
                {
                    if (row[Flight + SSRColumns] != null && row[Flight + SSRColumns].ToString() != "")
                    {
                        DataRow[] resultComfort = dtComfort.Select("Detail = '" + NewValues.ToString().Substring(0, 11) + "'");
                        foreach (DataRow rows in resultComfort)
                        {
                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                            if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["Price" + Flight + SSRColumns]) && row["SSRCode" + Flight + SSRColumns].ToString().Trim() == rows[0].ToString().Trim())
                            {
                                continue;
                                //return;
                            }
                            else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["Price" + Flight + SSRColumns]))
                            {
                                custommessage = "";
                                custommessage = msgList.Err800008.Replace("SSRColumns", SSRColumns);
                                grid.JSProperties["cp_result"] = custommessage;// "Cannot replace " + SSRColumns + " with cheaper fee, please try again";
                                count = 1;
                                //return;
                            }
                            else
                            {
                                if (row["SSRCode" + Flight + SSRColumns].ToString().Trim() != rows["SSRCode"].ToString().Trim())
                                {
                                    row[Flight + SSRColumns] = rows["Detail"];
                                    row["Price" + Flight + SSRColumns] = Convert.ToDecimal(Detail);
                                    row["Price" + Flight + SSRColumns + "1"] = Convert.ToDecimal(rows[3]);
                                    row["PriceCon" + Flight + SSRColumns] = Convert.ToDecimal(rows[4]);
                                    row["SSRCode" + Flight + SSRColumns] = rows["SSRCode"];
                                    row["Indicator" + Flight + SSRColumns] = "1";
                                }
                            }
                        }
                    }
                    else
                    {
                        DataRow[] resultComfort = dtComfort.Select("Detail = '" + NewValues.ToString().Substring(0, 11) + "'");
                        foreach (DataRow rows in resultComfort)
                        {
                            if (row["SSRCode" + Flight + SSRColumns].ToString().Trim() != rows["SSRCode"].ToString().Trim())
                            {
                                row[Flight + SSRColumns] = rows["Detail"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["Price" + Flight + SSRColumns] = Convert.ToDecimal(Detail);
                                row["Price" + Flight + SSRColumns + "1"] = Convert.ToDecimal(rows[3]);
                                row["PriceCon" + Flight + SSRColumns] = Convert.ToDecimal(rows[4]);
                                row["SSRCode" + Flight + SSRColumns] = rows["SSRCode"];
                                row["Indicator" + Flight + SSRColumns] = "1";
                            }
                        }
                    }
                }
            }
            SSRColumns = "";
        }
        #endregion

        protected void ClearSessionData()
        {
            bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(TransID);
            objBooking.FillDataTableTransMain(bookHDRInfo);
            int transStatus = 0;

            bool callFunction = false;

            if (Session["generatePayment"] == null)
                callFunction = true;
            else if (Session["generatePayment"].ToString() == "")
                callFunction = true;

            if (Session["modePage"].ToString() == "agent")
            {
                if (Session["AgentSet"] != null)
                {
                    objBooking.ClearExpiredJourney(MyUserSet.AgentID, TransID);
                }
                if (Session["AgentSet"] != null && callFunction == true)
                {

                    List<ListTransaction> AllTransaction = new List<ListTransaction>();
                    AllTransaction = objBooking.GetTransactionDetails(TransID);
                    //replace the new get booking from Navitaire
                    if (AllTransaction != null && AllTransaction.Count > 0)
                    {
                        ListTransaction lstTrans = AllTransaction[0];

                        List<BookingTransactionDetail> VoidPNRs = new List<BookingTransactionDetail>();
                        List<BookingTransactionDetail> ExpiredPNRs = new List<BookingTransactionDetail>();
                        if (objBooking.UpdateAllBookingJourneyDetails(lstTrans, lstTrans.AgentUserName.ToString(), lstTrans.AgentID.ToString(), ref VoidPNRs, ref ExpiredPNRs, true) == false)
                        {
                            log.Warning(this, "Fail to Get Latest Update for Transaction - manageaddons.aspx.cs : " + lstTrans.TransID);
                            if (ConfigurationManager.AppSettings["ErrorLogEmail"] != null && ConfigurationManager.AppSettings["ErrorLogEmail"] != "")
                            {
                                eWS.CustomEmail(ConfigurationManager.AppSettings["ErrorLogEmail"].ToString(), lstTrans.TransID, "", "Errors Log :" + DateTime.Now.ToString() + " <br/> Fail to Get Latest Update for Transaction : " + lstTrans.TransID);
                            }
                        }
                    }

                }
            }
            Session["generatePayment"] = "";

            byte TransStatus = 0;

            TransStatus = objBooking.GetTransStatus(TransID);
            if (TransStatus != 0)
            {
                bookHDRInfo.TransStatus = TransStatus;
            }

            if (Session["modePage"].ToString() == "agent")
            {
                if (Session["AgentSet"] != null && (bookHDRInfo.TransStatus == 2 || bookHDRInfo.TransStatus == 3))
                {
                    objBooking.UpdatePassengerDetails(TransID, MyUserSet.AgentName, MyUserSet.AgentID, true);

                    if (bookHDRInfo.TransStatus == 2)
                    {
                        bool GetPassengerComplete = false;
                        GetPassengerComplete = objBooking.CheckCompletePassenger(TransID);
                        if (GetPassengerComplete == true)
                        {
                            objBooking.UpdateTransMainStatus(TransID, 3);

                            TransStatus = 0;
                            TransStatus = objBooking.GetTransStatus(TransID);
                            if (TransStatus != 0)
                            {
                                bookHDRInfo.TransStatus = TransStatus;
                            }
                        }
                    }

                }
            }
            if (bookHDRInfo.TransStatus == 4 || bookHDRInfo.TransStatus == 6 || bookHDRInfo.TransStatus == 7)
            {
                lstRejectedbookDTLInfo = objBooking.GetAllBK_TRANSDTLFilter(TransID, 1);
                listDetailCombinePNR = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 1, "LEN(RecordLocator) >= 6 AND ");

                objBooking.FillDataTableTransDetail(listDetailCombinePNR);
                lstbookFlightInfo = objBooking.GetAllBK_TRANSDTLFlight(TransID, 1);
            }
            else
            {
                if (bookHDRInfo.TransStatus == 9)
                {
                    lstRejectedbookDTLInfo = objBooking.GetAllBK_TRANSDTLFilter(TransID, 1);
                }
                else
                {
                    lstbookDTLInfo = objBooking.GetAllBK_TRANSDTLFilter(TransID, 0);
                }
                if (lstbookDTLInfo == null)
                {
                    lstRejectedbookDTLInfo = objBooking.GetAllBK_TRANSDTLFilter(TransID, 1);
                    listDetailCombinePNR = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 1, "LEN(RecordLocator) >= 6 AND ");
                }
                else
                    listDetailCombinePNR = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 0, "LEN(RecordLocator) >= 6 AND ");

                objBooking.FillDataTableTransDetail(listDetailCombinePNR);
            }

        }

    }
}