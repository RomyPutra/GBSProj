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
using ABS.GBS.Log;
using StackExchange.Profiling;

namespace GroupBooking.Web
{
    public partial class AddOns : System.Web.UI.Page
    {
        #region declaration
        UserSet MyUserSet;
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogControl log = new LogControl();
        SystemLog SystemLog = new SystemLog();
        Boolean IsInternationalFlight = false;
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


        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
        ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");

        BookingTransactionDetail bookDTLInfo = new BookingTransactionDetail();
        List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
        BookingTransactionMain bookHDRInfo = new BookingTransactionMain();
        ABS.Logic.GroupBooking.Agent.AgentControl objAgent = new ABS.Logic.GroupBooking.Agent.AgentControl();
        ABS.Logic.GroupBooking.Agent.AgentProfileControl objAgentProfile = new ABS.Logic.GroupBooking.Agent.AgentProfileControl();
        ABS.Logic.GroupBooking.Agent.AgentProfile agent = new ABS.Logic.GroupBooking.Agent.AgentProfile();
        DataTable dtTaxFees = new DataTable();
        DataTable dtList1 = new DataTable();
        DataTable dtList2 = new DataTable();
        DataTable dtList1t = new DataTable();
        DataTable dtList2t = new DataTable();
        PassengerData PassData2 = new PassengerData();
        List<PassengerData> lstPassInfantData = new List<PassengerData>();
        private static int qtyMeal, qtyMeal1, qtyBaggage, qtySport, qtyComfort, qtyDrink, qtyDrink1, qtyDuty = 0;
        private static int qtyMeal2, qtyMeal21, qtyBaggage2, qtySport2, qtyComfort2, qtyDrink2, qtyDrink21, qtyDuty2 = 0;
        private static int num = 0;
        private static int departID = 0;
        private static string ReturnID = "";
        private static int first = 0;
        private static int infantmax = 0;
        object sumInfant;
        //added by ketee, currency
        private string Curr = "";
        string SessionID = "";
        private static string signature = "";
        private static DataTable datas = new DataTable();
        private static DataTable datas2 = new DataTable();

        ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model = null;
        ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model2 = null;


        string depart1 = "", depart2 = "", transit1 = "", transit2 = "", return1 = "", return2 = "";

        BookingTransactionFees bookFEEInfo = new BookingTransactionFees();
        List<BookingTransactionFees> lstbookFEEInfo = new List<BookingTransactionFees>();

        string custommessage = "";
        #endregion

        #region "Load"
        protected void Page_Load(object sender, EventArgs e)
        {
            //added by romy for profiler
            var profiler = MiniProfiler.Current;

            DataTable dt = new DataTable();
            dt = (DataTable)HttpContext.Current.Session["TempFlight"];

            if (dt != null && dt.Rows.Count > 0)
            {
                Curr = dt.Rows[0]["TemFlightCurrencyCode"].ToString();
            }
            else
            {
                Curr = "";
            }

            //added by ketee, use the selling session to get selled ssr code, 20170119
            if (HttpContext.Current.Session["dataClassTrans"] != null)
            {
                dt = (DataTable)HttpContext.Current.Session["dataClassTrans"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    signature = dt.Rows[0]["SellSignature"].ToString();
                }
            }
            else
            {
                if (IsCallback)
                    ASPxCallback.RedirectOnCallback(GroupBooking.Web.Shared.MySite.PublicPages.SessionExpired);
                else
                    Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.SessionExpired);
            }
            //return;
            try
            {
                ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking("");
                //signature = Session["signature"].ToString();
                Session["PaxStatus"] = "";
                if (Session["AgentSet"] != null)
                {
                    using (profiler.Step("BindModel"))
                    {
                        BindModel();
                    }
                    MyUserSet = (UserSet)Session["AgentSet"];
                    if (!IsPostBack)
                    {
                        using (profiler.Step("InitializeSetting"))
                        {
                            InitializeSetting();
                        }
                        using (profiler.Step("GetSellSSR"))
                        {
                            GetSellSSR(signature);
                        }
                        //BindLabel();

                    }

                    if (Session["TransID"] != null)
                    {
                        hfTransID.Value = Session["TransID"].ToString();
                        using (profiler.Step("Generate"))
                        {
                            string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), Session["TransID"].ToString(), "");
                            hfHashKey.Value = hashkey;
                        }
                        if (Session["dtGridPass"] == null)
                        {
                            using (profiler.Step("GetPassengerList"))
                            {
                                GetPassengerList(Session["TransID"].ToString(), "Depart");
                            }
                        }
                        else
                        {
                            datas = (DataTable)Session["dtGridPass"];
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
                    }
                    else
                    {
                        Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.SessionExpired);
                    }

                    HttpCookie cookie2 = Request.Cookies["cookieSearchcondition"];
                    if (cookie2 != null)
                    {
                        if (cookie2.Values["ifOneWay"] != "TRUE")
                        {
                            if (Session["TransID"] != null)
                            {
                                if (Session["dtGridPass2"] == null)
                                {
                                    using (profiler.Step("GetPassengerList"))
                                    {
                                        GetPassengerList(Session["TransID"].ToString(), "Return");
                                    }
                                }
                                else
                                {
                                    datas2 = (DataTable)Session["dtGridPass2"];
                                }
                                using (profiler.Step("GetSSRItem"))
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
                    }

                    if (Session["IsNew"] != null && Session["IsNew"].ToString() == "True")
                    {
                        first = 0;
                    }
                    if (first == 0 && Convert.ToBoolean(Session["back"]) != true)
                    {
                        using (profiler.Step("BindDefaultBaggage"))
                        {
                            BindDefaultBaggage();
                        }
                    }
                    using (profiler.Step("BindLabel"))
                    {
                        BindLabel();
                    }
                }
                else
                {
                    if (IsCallback)
                        ASPxCallback.RedirectOnCallback(GroupBooking.Web.Shared.MySite.PublicPages.SessionExpired);
                    else
                        Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.SessionExpired);
                }
                gvPassenger.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                gvPassenger2.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //log.Error(this,ex);
                log.Error(this, ex);
                if (IsCallback)
                    ASPxCallback.RedirectOnCallback(GroupBooking.Web.Shared.MySite.PublicPages.SessionExpired);
                else
                    Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.SessionExpired);
            }

        }

        #endregion

        #region "Initializer"
        protected void GetSSRItem(string flight)
        {
            IsInternationalFlight = objGeneral.IsInternationalFlight(model.TemFlightDeparture, model.TemFlightArrival, Request.PhysicalApplicationPath);
            DataTable dtdefaultBundle = new DataTable();
            HttpCookie cookie = Request.Cookies["cookieSearchcondition"];
            string[] category = new string[] { "Baggage", "Sport", "Drink", "Infant" };
            GridViewDataComboBoxColumn column = new GridViewDataComboBoxColumn();
            for (int p = 0; p < category.Length; p++)
            {
                if (flight == "Depart")
                {
                    if (gvPassenger.Columns[category[p]] != null && gvPassenger.Columns[category[p]].Visible)
                    {
                        column = (gvPassenger.Columns[category[p]] as GridViewDataComboBoxColumn);
                    }
                }
                else
                {
                    if (gvPassenger2.Columns[category[p]] != null && gvPassenger2.Columns[category[p]].Visible)
                    {
                        column = (gvPassenger2.Columns[category[p]] as GridViewDataComboBoxColumn);
                    }
                }
                DataTable dtBaggage = null;
                dtBaggage = (DataTable)Session["dt" + category[p] + flight];
                if (dtBaggage != null)
                {
                    DataRow[] result = dtBaggage.Select("SSRCode = ''");
                    if (result.Length == 0)
                    {
                        if (category[p].ToString() != "Drink")
                        {
                            if (Request.QueryString["change"] == null)
                            {
                                if (cookie != null)
                                {
                                    if (first == 0)
                                    {
                                        //log.Warning(this, "First " + model.TemFlightInternational.ToUpper() + " " + Session["TransID"].ToString());
                                        if (category[p].ToString() != "Baggage")
                                        {
                                            dtBaggage.Rows.Add("", "", "");
                                        }
                                        else
                                        {
                                            
                                            if (IsInternationalFlight == true)
                                            {
                                                dtdefaultBundle = objBooking.GetDefaultBundle("DEFAULTINTERNATIONALBAGGAGE", model.TemFlightCarrierCode);
                                                //log.Warning(this, "Second - " + model.TemFlightCarrierCode + " " + Session["TransID"].ToString());
                                            }
                                            else
                                            {
                                                dtdefaultBundle = objBooking.GetDefaultBundle("DEFAULTDOMESTICBAGGAGE", model.TemFlightCarrierCode);
                                            }
                                            if (dtdefaultBundle != null && dtdefaultBundle.Rows.Count > 0)
                                            {
                                                //log.Warning(this, "Third - " + model.TemFlightCarrierCode + " " + Session["TransID"].ToString());
                                                Session["dtdefaultBundle"] = dtdefaultBundle;
                                            }
                                            else
                                            {
                                                //log.Warning(this, "Third no record - " + model.TemFlightCarrierCode + " " + Session["TransID"].ToString());
                                                dtBaggage.Rows.Add("", "", "");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
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

            if (Session["InfantCode"] == null)
            {
                if (flight == "Depart")
                {
                    gvPassenger.Columns["Infant"].Visible = false;
                    InfantIcon1.Style.Add("display", "none");
                }
                else
                {
                    gvPassenger2.Columns["Infant"].Visible = false;
                    InfantIcon2.Style.Add("display", "none");
                }
            }
        }


        protected void GetDrink1Depart()
        {
            GridViewDataComboBoxColumn column = (gvPassenger.Columns["Drink1"] as GridViewDataComboBoxColumn);
            DataTable dtBaggage = null;
            dtBaggage = (DataTable)Session["dtDrinkDepart2"];

            column.PropertiesComboBox.DataSource = dtBaggage;

            column.PropertiesComboBox.ValueField = "SSRCode";
            column.PropertiesComboBox.TextField = "ConcatenatedField";

            gvPassenger.DataBind();
        }

        protected void GetDrink1Return()
        {
            GridViewDataComboBoxColumn column = (gvPassenger2.Columns["Drink1"] as GridViewDataComboBoxColumn);
            DataTable dtBaggage = null;
            dtBaggage = (DataTable)Session["dtDrinkReturn2"];

            column.PropertiesComboBox.DataSource = dtBaggage;

            column.PropertiesComboBox.ValueField = "SSRCode";
            column.PropertiesComboBox.TextField = "ConcatenatedField";

            gvPassenger2.DataBind();
        }

        protected void glMeals_AutoFilterCellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            MessageList msgList = new MessageList();
            (e.Editor as ASPxTextBox).NullText = msgList.Err200103;
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
        protected void InitializeSetting()
        {
            try
            {
                HttpCookie cookie = Request.Cookies["cookieSearchcondition"];
                if (cookie != null)
                {
                    string temp = "";
                    if (cookie.Values["ifOneWay"] != "TRUE")
                    {

                        temp = cookie.Values["Departure"] + " | " + cookie.Values["Arrival"];
                        TabControl.TabPages[0].Text = temp;

                        temp = cookie.Values["Arrival"] + " | " + cookie.Values["Departure"];
                        TabControl.TabPages[1].Text = temp;
                    }
                    else
                    {
                        temp = cookie.Values["Departure"] + " | " + cookie.Values["Arrival"];
                        TabControl.TabPages[0].Text = temp;
                        TabControl.TabPages[1].Visible = false;
                    }


                }

                //BindDefaultDrink();
                Clearsession();
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
            DataTable dtdefaultDrink = new DataTable();
            dtdefaultDrink = objBooking.GetArrayCategory("DEFAULTDRINK");
            Session["defaultdrink"] = dtdefaultDrink;

        }
        protected void InitializeForm(String Currency, ArrayList BaggageCode, ArrayList BaggageFee, ArrayList BaggageFeeS1, ArrayList BaggageFeeS2, ArrayList SportCode, ArrayList SportFee, ArrayList SportFeeS1, ArrayList SportFeeS2, ArrayList ComfortCode, ArrayList ConfortFee, ArrayList ConfortFeeS1, ArrayList ConfortFeeS2, ArrayList ComfortImg, ArrayList MealCode, ArrayList MealFee, ArrayList MealImg, ArrayList MealCode1, ArrayList MealFee1, ArrayList MealImg1, ArrayList DutyCode, ArrayList DutyFee, ArrayList DutyImg, ArrayList DrinkCode, ArrayList DrinkFee, ArrayList DrinkCode1, ArrayList DrinkFee1, ArrayList InfantCode, ArrayList InfantFee, ArrayList InfantFeeS1, ArrayList InfantFeeS2, string Flight)
        {
            MessageList msgList = new MessageList();
            HttpCookie cookie = Request.Cookies["cookieSearchcondition"];
            try
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
                    if (dtBaggage.Rows.Count > 0)
                    {
                        cmbBaggage.DataSource = dv.ToTable(true, "SSRCode", "Detail", "Price", "PriceS1", "PriceS2", "ConcatenatedField");
                        cmbBaggage.TextField = "ConcatenatedField";
                        cmbBaggage.ValueField = "SSRCode";
                        cmbBaggage.DataBind();
                        cmbBaggage.NullText = msgList.Err200101;
                    }
                    else
                    {
                        cmbBaggage.NullText = msgList.Err200160;
                    }
                }
                else
                {
                    if (dtBaggage.Rows.Count > 0)
                    {
                        cmbBaggage2.DataSource = dv.ToTable(true, "SSRCode", "Detail", "Price", "PriceS1", "PriceS2", "ConcatenatedField");
                        cmbBaggage2.TextField = "ConcatenatedField";
                        cmbBaggage2.ValueField = "SSRCode";
                        cmbBaggage2.DataBind();
                        cmbBaggage2.NullText = msgList.Err200101;
                    }
                    else
                    {
                        cmbBaggage2.NullText = msgList.Err200160;
                    }
                    
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
                        //gvPassenger.Columns["Meal"].Caption = "Meal 1";
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
                        //gvPassenger2.Columns["Meal"].Caption = "Meal 1";
                    }
                    Session["dtMeal" + Flight + "2"] = dtMeal1;

                }
                else
                {
                    if (Flight == "Depart")
                    {
                        divmeal1.Style.Add("display", "none");
                        gvPassenger.Columns["Meal1"].Visible = false;
                    }
                    else
                    {
                        divmeal2.Style.Add("display", "none");
                        gvPassenger2.Columns["Meal1"].Visible = false;
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
                        gvPassenger.Columns["Drink"].VisibleIndex = -3;
                    }
                    else
                    {
                        tdDrinks2.Style.Add("display", "none");
                        gvPassenger2.Columns["Drink"].VisibleIndex = -3;
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
                        //gvPassenger.Columns["Drink"].Caption = "Drink 1";
                    }
                    else
                    {
                        cmbDrinks22.DataSource = dvDrink1.ToTable(true, "SSRCode", "Detail", "Price", "ConcatenatedField");
                        cmbDrinks22.DataSource = dtDrink1.DefaultView;
                        cmbDrinks22.TextField = "ConcatenatedField";
                        cmbDrinks22.ValueField = "SSRCode";
                        cmbDrinks22.DataBind();
                        cmbDrinks22.NullText = msgList.Err200104;
                        //gvPassenger2.Columns["Drink"].Caption = "Drink 1";
                    }
                    Session["dtDrink" + Flight + "2"] = dtDrink1;

                }
                else
                {
                    if (Flight == "Depart")
                    {
                        tdDrinks1.Style.Add("display", "none");
                        gvPassenger.Columns["Drink1"].VisibleIndex = -3;
                    }
                    else
                    {
                        tdDrinks22.Style.Add("display", "none");
                        gvPassenger2.Columns["Drink1"].VisibleIndex = -3;
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

                //-----------------------------------------
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
                        gvPassenger.Columns["Infant"].Visible = false;
                        InfantIcon1.Style.Add("display", "none");
                    }
                    else
                    {
                        gvPassenger2.Columns["Infant"].Visible = false;
                        InfantIcon2.Style.Add("display", "none");
                    }
                }
                //if (Flight == "Depart")
                //{
                //    cmbSport.DataSource = dvInfant.ToTable(true, "SSRCode", "Detail", "Price", "PriceS1", "PriceS2", "ConcatenatedField");
                //    cmbSport.TextField = "ConcatenatedField";
                //    cmbSport.ValueField = "SSRCode";
                //    cmbSport.DataBind();
                //    cmbSport.NullText = msgList.Err200105;
                //}
                //else
                //{
                //    cmbSport2.DataSource = dvInfant.ToTable(true, "SSRCode", "Detail", "Price", "PriceS1", "PriceS2", "ConcatenatedField");
                //    cmbSport2.TextField = "ConcatenatedField";
                //    cmbSport2.ValueField = "SSRCode";
                //    cmbSport2.DataBind();
                //    cmbSport2.NullText = msgList.Err200105;
                //}


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
                    //glComfort.NullText = msgList.Err200107;
                    if (dtComfort.Rows.Count > 0)
                    {
                        glComfort.NullText = msgList.Err200107;
                    }
                    else
                    {
                        glComfort.NullText = msgList.Err200059;
                    }
                }
                else
                {
                    glComfort2.DataSource = dvComfort.ToTable(true, "SSRCode", "Detail", "Price", "PriceS1", "PriceS2", "Images");
                    glComfort2.DataBind();
                    //glComfort2.NullText = msgList.Err200107;
                    if (dtComfort.Rows.Count > 0)
                    {
                        glComfort2.NullText = msgList.Err200107;
                    }
                    else
                    {
                        glComfort2.NullText = msgList.Err200059;
                    }
                }
                Session["dtComfort" + Flight] = dtComfort;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
        }

        protected void FillModelFromDataRow(DataRow[] foundRows, ref ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model)
        {
            try
            {
                if (IsNumeric(foundRows[0]["TemFlightId"].ToString()))
                { model.TemFlightId = Convert.ToInt16(foundRows[0]["TemFlightId"]); }
                model.TemFlightFlightNumber = foundRows[0]["TemFlightFlightNumber"].ToString();
                model.TemFlightDate = Convert.ToDateTime(foundRows[0]["TemFlightDate"]);
                model.TemFlightArrival = foundRows[0]["TemFlightArrival"].ToString();
                model.TemFlightCarrierCode = foundRows[0]["TemFlightCarrierCode"].ToString();
                model.TemFlightInternational = foundRows[0]["TemFlightInternational"].ToString();
                //model.TemFlightJourneySellKey = foundRows[0]["TemFlightJourneySellKey"].ToString();
                model.TemFlightCHDNum = Convert.ToInt16(foundRows[0]["TemFlightCHDNum"]);
                model.TemFlightCurrencyCode = foundRows[0]["TemFlightCurrencyCode"].ToString();
                model.TemFlightStd = Convert.ToDateTime(foundRows[0]["TemFlightStd"]);
                model.TemFlightDeparture = foundRows[0]["TemFlightDeparture"].ToString();
                model.TemFlightADTNum = Convert.ToInt16(foundRows[0]["TemFlightADTNum"]);
                model.TemFlightIfReturn = Convert.ToBoolean(foundRows[0]["TemFlightIfReturn"]);
                model.TemFlightPaxNum = Convert.ToInt16(foundRows[0]["TemFlightPaxNum"]);
                model.TemFlightSta = Convert.ToDateTime(foundRows[0]["TemFlightSta"]);
                model.TemFlightAgentName = foundRows[0]["TemFlightAgentName"].ToString();
                if (IsNumeric(foundRows[0]["TemFlightAveragePrice"].ToString()))
                { model.TemFlightAveragePrice = Convert.ToDecimal(foundRows[0]["TemFlightAveragePrice"]); }
                if (IsNumeric(foundRows[0]["TemFlightTotalAmount"].ToString()))
                { model.TemFlightTotalAmount = Convert.ToDecimal(foundRows[0]["TemFlightTotalAmount"]); }
                //amended by diana 20131103 - to insert each fare
                if (IsNumeric(foundRows[0]["TemFlightPromoDisc"].ToString()))
                { model.TemFlightPromoDisc = Convert.ToDecimal(foundRows[0]["TemFlightPromoDisc"]); }
                if (IsNumeric(foundRows[0]["TemFlightFarePrice"].ToString()))
                { model.temFlightfarePrice = Convert.ToDecimal(foundRows[0]["TemFlightFarePrice"]); }
                if (IsNumeric(foundRows[0]["TemFlightAPT"].ToString()))
                { model.TemFlightApt = Convert.ToDecimal(foundRows[0]["TemFlightAPT"]); }
                if (IsNumeric(foundRows[0]["TemFlightFuel"].ToString()))
                { model.TemFlightFuel = Convert.ToDecimal(foundRows[0]["TemFlightFuel"]); }

                if (IsNumeric(foundRows[0]["TemFlightOth"].ToString()))
                { model.TemFlightOth = Convert.ToDecimal(foundRows[0]["TemFlightOth"]); }
                if (IsNumeric(foundRows[0]["TemFlightDisc"].ToString()))
                { model.TemFlightDisc = Convert.ToDecimal(foundRows[0]["TemFlightDisc"]); }

                if (IsNumeric(foundRows[0]["TemFlightServiceCharge"].ToString()))
                { model.TemFlightServiceCharge = Convert.ToDecimal(foundRows[0]["TemFlightServiceCharge"]); }

                model.TemFlightTransit = foundRows[0]["TemFlightTransit"].ToString();
                DateTime sta2;
                if (DateTime.TryParse(foundRows[0]["TemFlightSta2"].ToString(), out sta2))
                    model.TemFlightSta2 = sta2;
                DateTime std2;
                if (DateTime.TryParse(foundRows[0]["TemFlightStd2"].ToString(), out std2))
                    model.TemFlightStd2 = std2;

                model.TemFlightCarrierCode2 = foundRows[0]["TemFlightCarrierCode2"].ToString();
                model.TemFlightFlightNumber2 = foundRows[0]["TemFlightFlightNumber2"].ToString();
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
        }

        protected void Clearsession()
        {
            Session["InfantCode"] = null;
            //Session["IsNew"] = "True";
            //Session["dtGridPass"] = null;
            //Session["dtGridPass2"] = null;
            Session["dtdefaultBundle"] = null;
            Session["dtBaggageDepart"] = null;
            Session["dtBaggageReturn"] = null;
            Session["Currency"] = null;
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
            Session["transit"] = null;
            Session["departTransit"] = null;
            Session["departdifferent"] = null;
            Session["returndifferent"] = null;
            Session["CarrierCode"] = null;
            HttpContext.Current.Session.Remove("transitdepart");
            HttpContext.Current.Session.Remove("returnTransit");
            HttpContext.Current.Session.Remove("transitreturn");
            first = 0;
            Session["dtBaggageDepart"] = null;
            Session["dtBaggageReturn"] = null;
            Session["dtMealDepart"] = null;
            Session["dtMealReturn"] = null;
            Session["dtDrinkDepart"] = null;
            Session["dtDrinkReturn"] = null;
            Session["dtSportDrpart"] = null;
            Session["dtSportReturn"] = null;
            Session["dtInfantDepart"] = null;
            Session["dtInfantReturn"] = null;
            Session["dtComfortDepart"] = null;
            Session["dtComfortReturn"] = null;
        }
        #endregion

        #region "Event"
        protected void BindDefaultBaggage()
        {
            string Detail = "";
            try
            {
                DataTable dtdefaultBundle = new DataTable();
                HttpCookie cookie = Request.Cookies["cookieSearchcondition"];
                DataTable dtPass = new DataTable();
                DataTable dtPass2 = new DataTable();

                if (Session["dtGridPass"] != null)
                {
                    dtPass = (DataTable)Session["dtGridPass"];
                }
                if (dtPass != null && dtPass.Rows.Count > 0)
                {
                    if (Session["dtdefaultBundle"] != null)
                    {
                        //log.Warning(this, "Fourth - " + model.TemFlightCarrierCode + " " + Session["TransID"].ToString());
                        dtdefaultBundle = (DataTable)Session["dtdefaultBundle"];
                        Session["CarrierCode"] = model.TemFlightCarrierCode;

                        if (dtdefaultBundle != null && dtdefaultBundle.Rows.Count > 0)
                        {
                            if (Request.QueryString["change"] == null)
                            {
                                //log.Warning(this, "Fifth - " + model.TemFlightCarrierCode + " " + Session["TransID"].ToString());
                                if (Session["dtBaggageDepart"] != null)
                                {
                                    //log.Warning(this, "Sixth - " + model.TemFlightCarrierCode + " " + Session["TransID"].ToString());
                                    DataTable dtBaggage = (DataTable)Session["dtBaggageDepart"];
                                    for (int i = 0; i <= dtPass.Rows.Count - 1; i++)
                                    {

                                        DataRow[] result = dtBaggage.Select("SSRCode = '" + dtdefaultBundle.Rows[0]["SYSValue"] + "'");
                                        if (result.Length > 0)
                                        {
                                            foreach (DataRow row in result)
                                            {
                                                dtPass.Rows[i]["Baggage"] = row[5];
                                                dtPass.Rows[i]["SSRCodeBaggage"] = row[0];
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                dtPass.Rows[i]["PriceBaggageS1"] = Convert.ToDecimal(row[3]);
                                                dtPass.Rows[i]["PriceBaggageS2"] = Convert.ToDecimal(row[4]);
                                            }
                                            dtPass.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                        }
                                    }
                                    //log.Warning(this, "Seventh - " + model.TemFlightCarrierCode + " " + Session["TransID"].ToString());
                                    Session["dtGridPass"] = dtPass;
                                    gvPassenger.DataSource = dtPass;
                                    gvPassenger.DataBind();
                                }

                                if (cookie.Values["ifOneWay"] != "TRUE")
                                {
                                    if (Session["dtGridPass2"] != null)
                                    {
                                        dtPass2 = (DataTable)Session["dtGridPass2"];
                                    }
                                    if (Session["dtBaggageReturn"] != null)
                                    {
                                        DataTable dtBaggage2 = (DataTable)Session["dtBaggageReturn"];
                                        for (int i = 0; i <= dtPass2.Rows.Count - 1; i++)
                                        {
                                            DataRow[] result = dtBaggage2.Select("SSRCode = '" + dtdefaultBundle.Rows[0]["SYSValue"] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                dtPass2.Rows[i]["Baggage"] = row[5];
                                                dtPass2.Rows[i]["SSRCodeBaggage"] = row[0];
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                dtPass2.Rows[i]["PriceBaggageS1"] = Convert.ToDecimal(row[3]);
                                                dtPass2.Rows[i]["PriceBaggageS2"] = Convert.ToDecimal(row[4]);
                                            }
                                            dtPass2.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                        }
                                        Session["dtGridPass2"] = dtPass2;
                                        gvPassenger2.DataSource = dtPass2;
                                        gvPassenger2.DataBind();
                                    }
                                }
                            }
                            first = 1;
                            //log.Warning(this, "Eighth - " + model.TemFlightCarrierCode + " " + Session["TransID"].ToString());
                        }
                        else
                        {
                            first = 0;
                            //log.Warning(this, "Eighth no record  - " + model.TemFlightCarrierCode + " " + Session["TransID"].ToString());
                        }

                        Session["IsNew"] = "false";

                    }
                    else
                    {
                        //log.Warning(this, "Fourth other - " + model.TemFlightCarrierCode + " " + Session["TransID"].ToString());
                        if (IsInternationalFlight)
                        {
                            dtdefaultBundle = objBooking.GetDefaultBundle("DEFAULTINTERNATIONALBAGGAGE", model.TemFlightCarrierCode);
                            //log.Warning(this, "Fifth other - " + model.TemFlightCarrierCode + " " + Session["TransID"].ToString());
                        }
                        else
                        {
                            dtdefaultBundle = objBooking.GetDefaultBundle("DEFAULTDOMESTICBAGGAGE", model.TemFlightCarrierCode);
                        }

                        Session["dtdefaultBundle"] = dtdefaultBundle;
                        Session["CarrierCode"] = model.TemFlightCarrierCode;

                        if (dtdefaultBundle != null && dtdefaultBundle.Rows.Count > 0)
                        {
                            //log.Warning(this, "Sixth other - " + model.TemFlightCarrierCode + " " + Session["TransID"].ToString());
                            if (Request.QueryString["change"] == null)
                            {
                                if (Session["dtBaggageDepart"] != null)
                                {
                                    //log.Warning(this, "Seventh other - " + model.TemFlightCarrierCode + " " + Session["TransID"].ToString());
                                    DataTable dtBaggage = (DataTable)Session["dtBaggageDepart"];
                                    for (int i = 0; i <= dtPass.Rows.Count - 1; i++)
                                    {

                                        DataRow[] result = dtBaggage.Select("SSRCode = '" + dtdefaultBundle.Rows[0]["SYSValue"] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            dtPass.Rows[i]["Baggage"] = row[5];
                                            dtPass.Rows[i]["SSRCodeBaggage"] = row[0];
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            dtPass.Rows[i]["PriceBaggageS1"] = Convert.ToDecimal(row[3]);
                                            dtPass.Rows[i]["PriceBaggageS2"] = Convert.ToDecimal(row[4]);
                                        }
                                        dtPass.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                    }
                                    Session["dtGridPass"] = dtPass;
                                    gvPassenger.DataSource = dtPass;
                                    gvPassenger.DataBind();
                                }

                                if (cookie.Values["ifOneWay"] != "TRUE")
                                {
                                    if (Session["dtGridPass2"] != null)
                                    {
                                        dtPass2 = (DataTable)Session["dtGridPass2"];
                                    }
                                    if (Session["dtBaggageReturn"] != null)
                                    {
                                        DataTable dtBaggage2 = (DataTable)Session["dtBaggageReturn"];
                                        for (int i = 0; i <= dtPass2.Rows.Count - 1; i++)
                                        {
                                            DataRow[] result = dtBaggage2.Select("SSRCode = '" + dtdefaultBundle.Rows[0]["SYSValue"] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                dtPass2.Rows[i]["Baggage"] = row[5];
                                                dtPass2.Rows[i]["SSRCodeBaggage"] = row[0];
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                dtPass2.Rows[i]["PriceBaggageS1"] = Convert.ToDecimal(row[3]);
                                                dtPass2.Rows[i]["PriceBaggageS2"] = Convert.ToDecimal(row[4]);
                                            }
                                            dtPass2.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                        }
                                        Session["dtGridPass2"] = dtPass2;
                                        gvPassenger2.DataSource = dtPass2;
                                        gvPassenger2.DataBind();
                                    }
                                }
                            }
                            first = 1;
                            //log.Warning(this, "Eighth other - " + model.TemFlightCarrierCode + " " + Session["TransID"].ToString());
                        }
                        else
                        {
                            first = 0;
                            //log.Warning(this, "Eighth other no record - " + model.TemFlightCarrierCode + " " + Session["TransID"].ToString());
                        }

                        Session["IsNew"] = "false";
                    }
                }

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
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
                    TotalBaggage = dtPass.Compute("Sum(PriceBaggage)", "");
                    TotalMeal = dtPass.Compute("Sum(PriceMeal)", "");
                    TotalMeal1 = dtPass.Compute("Sum(PriceMeal1)", "");
                    TotalSport = dtPass.Compute("Sum(PriceSport)", "");
                    TotalComfort = dtPass.Compute("Sum(PriceComfort)", "");
                    TotalInfant = dtPass.Compute("Sum(PriceInfant)", "");
                    //TotalDuty = dtPass.Compute("Sum(PriceDuty)", "");



                    if (Session["dtGridPass2"] != null)
                    {
                        DataTable dtPass2 = (DataTable)Session["dtGridPass2"];
                        TotalBaggage2 = dtPass2.Compute("Sum(PriceBaggage)", "");
                        TotalMeal2 = dtPass2.Compute("Sum(PriceMeal)", "");
                        TotalMeal12 = dtPass2.Compute("Sum(PriceMeal1)", "");
                        TotalSport2 = dtPass2.Compute("Sum(PriceSport)", "");
                        TotalComfort2 = dtPass2.Compute("Sum(PriceComfort)", "");
                        TotalInfant2 = dtPass2.Compute("Sum(PriceInfant)", "");
                        //TotalDuty2 = dtPass2.Compute("Sum(PriceDuty)", "");

                        //lblTotalBaggage.Text = (Convert.ToDecimal(TotalBaggage) + Convert.ToDecimal(TotalBaggage2)).ToString("N", nfi);
                        //lblTotalMeal.Text = (Convert.ToDecimal(TotalMeal) + Convert.ToDecimal(TotalMeal2) + Convert.ToDecimal(TotalMeal1) + Convert.ToDecimal(TotalMeal12)).ToString("N", nfi);
                        //lblTotalSport.Text = (Convert.ToDecimal(TotalSport) + Convert.ToDecimal(TotalSport2)).ToString("N", nfi);
                        //lblTotalComfort.Text = (Convert.ToDecimal(TotalComfort) + Convert.ToDecimal(TotalComfort2)).ToString("N", nfi);
                        //lblTotalDuty.Text = (Convert.ToDecimal(TotalDuty) + Convert.ToDecimal(TotalDuty2)).ToString("N", nfi);
                        lblTotalAmount.Text = (Convert.ToDecimal(TotalBaggage) + Convert.ToDecimal(TotalBaggage2) + Convert.ToDecimal(TotalMeal)
                            + Convert.ToDecimal(TotalMeal2) + Convert.ToDecimal(TotalMeal1) + Convert.ToDecimal(TotalMeal12)
                            + Convert.ToDecimal(TotalSport) + Convert.ToDecimal(TotalSport2) + Convert.ToDecimal(TotalComfort)
                            + Convert.ToDecimal(TotalComfort2) + Convert.ToDecimal(TotalInfant) + Convert.ToDecimal(TotalInfant2)).ToString("N", nfi);
                    }
                    else
                    {
                        //lblTotalBaggage.Text = (Convert.ToDecimal(TotalBaggage)).ToString("N", nfi);
                        //lblTotalMeal.Text = (Convert.ToDecimal(TotalMeal) + Convert.ToDecimal(TotalMeal1)).ToString("N", nfi);
                        //lblTotalSport.Text = (Convert.ToDecimal(TotalSport)).ToString("N", nfi);
                        //lblTotalComfort.Text = (Convert.ToDecimal(TotalComfort)).ToString("N", nfi);
                        //lblTotalDuty.Text = (Convert.ToDecimal(TotalDuty)).ToString("N", nfi);
                        lblTotalAmount.Text = (Convert.ToDecimal(TotalBaggage) + Convert.ToDecimal(TotalMeal)
                            + Convert.ToDecimal(TotalMeal1) + Convert.ToDecimal(TotalSport) + Convert.ToDecimal(TotalComfort) + Convert.ToDecimal(TotalInfant)).ToString("N", nfi);
                    }

                    if (Session["Currency"] != null)
                    {
                        lblCurrency.Text = Session["Currency"].ToString();
                    }
                    else
                    {
                        lblCurrency.Text = Curr;
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

        protected void glDrinksCallback_PerformCallback(object sender, CallbackEventArgsBase e)
        {

            try
            {
                cmbDrinks.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void glDrinks1Callback_PerformCallback(object sender, CallbackEventArgsBase e)
        {


            try
            {
                cmbDrinks1.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void glDrinks2Callback_PerformCallback(object sender, CallbackEventArgsBase e)
        {


            try
            {
                cmbDrinks2.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void glDrinks22Callback_PerformCallback(object sender, CallbackEventArgsBase e)
        {


            try
            {
                cmbDrinks22.SelectedIndex = 0;

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
                    TotalBaggage = dtPass.Compute("Sum(PriceBaggage)", "");
                    TotalMeal = dtPass.Compute("Sum(PriceMeal)", "");
                    TotalMeal1 = dtPass.Compute("Sum(PriceMeal1)", "");
                    TotalSport = dtPass.Compute("Sum(PriceSport)", "");
                    TotalComfort = dtPass.Compute("Sum(PriceComfort)", "");
                    TotalInfant = dtPass.Compute("Sum(PriceInfant)", "");
                    //TotalDuty = dtPass.Compute("Sum(PriceDuty)", "");

                    lblTotalBaggage.Text = (Convert.ToDecimal(TotalBaggage)).ToString("N", nfi);
                    lblTotalMeal.Text = (Convert.ToDecimal(TotalMeal) + Convert.ToDecimal(TotalMeal1)).ToString("N", nfi);
                    lblTotalSport.Text = (Convert.ToDecimal(TotalSport)).ToString("N", nfi);
                    lblTotalComfort.Text = (Convert.ToDecimal(TotalComfort)).ToString("N", nfi);
                    lblTotalInfant.Text = (Convert.ToDecimal(TotalInfant)).ToString("N", nfi);
                    //lblTotalDuty.Text = (Convert.ToDecimal(TotalDuty)).ToString("N", nfi);
                    lblTotalAmount.Text = (Convert.ToDecimal(string.IsNullOrEmpty(lblTotalAmount.Text) ? "0" : lblTotalAmount.Text) + Convert.ToDecimal(TotalBaggage) + Convert.ToDecimal(TotalMeal)
                        + Convert.ToDecimal(TotalMeal1) + Convert.ToDecimal(TotalSport) + Convert.ToDecimal(TotalComfort) + Convert.ToDecimal(TotalInfant)).ToString("N", nfi);

                    if (Session["Currency"] != null)
                    {
                        lblCurrency.Text = Session["Currency"].ToString();
                    }
                    else
                    {
                        lblCurrency.Text = Curr;
                    }

                    //if (Session["InfantCode"] == null)
                    //{
                    //    InfantIcon1.Style.Add("display", "none");
                    //}
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
                    TotalBaggage = dtPass.Compute("Sum(PriceBaggage)", "");
                    TotalMeal = dtPass.Compute("Sum(PriceMeal)", "");
                    TotalMeal1 = dtPass.Compute("Sum(PriceMeal1)", "");
                    TotalSport = dtPass.Compute("Sum(PriceSport)", "");
                    TotalComfort = dtPass.Compute("Sum(PriceComfort)", "");
                    TotalInfant = dtPass.Compute("Sum(PriceInfant)", "");
                    //TotalDuty = dtPass.Compute("Sum(PriceDuty)", "");

                    lblTotalBaggage2.Text = (Convert.ToDecimal(TotalBaggage)).ToString("N", nfi);
                    lblTotalMeal2.Text = (Convert.ToDecimal(TotalMeal) + Convert.ToDecimal(TotalMeal1)).ToString("N", nfi);
                    lblTotalSport2.Text = (Convert.ToDecimal(TotalSport)).ToString("N", nfi);
                    lblTotalComfort2.Text = (Convert.ToDecimal(TotalComfort)).ToString("N", nfi);
                    lblTotalInfant2.Text = (Convert.ToDecimal(TotalInfant)).ToString("N", nfi);
                    //lblTotalDuty2.Text = (Convert.ToDecimal(TotalDuty)).ToString("N", nfi);
                    lblTotalAmount.Text = (Convert.ToDecimal(string.IsNullOrEmpty(lblTotalAmount.Text) ? "0" : lblTotalAmount.Text) + Convert.ToDecimal(TotalBaggage) + Convert.ToDecimal(TotalMeal)
                        + Convert.ToDecimal(TotalMeal1) + Convert.ToDecimal(TotalSport) + Convert.ToDecimal(TotalComfort) + Convert.ToDecimal(TotalInfant)).ToString("N", nfi);

                    if (Session["Currency"] != null)
                    {
                        lblCurrency.Text = Session["Currency"].ToString();
                    }
                    else
                    {
                        lblCurrency.Text = Curr;
                    }

                    //if (Session["InfantCode2"] == null)
                    //{
                    //    InfantIcon2.Style.Add("display", "none");
                    //}
                }

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
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
                    TotalBaggage = dtPass.Compute("Sum(PriceBaggage)", "");
                    TotalMeal = dtPass.Compute("Sum(PriceMeal)", "");
                    TotalMeal1 = dtPass.Compute("Sum(PriceMeal1)", "");
                    TotalSport = dtPass.Compute("Sum(PriceSport)", "");
                    TotalComfort = dtPass.Compute("Sum(PriceComfort)", "");
                    TotalInfant = dtPass.Compute("Sum(PriceInfant)", "");
                    //TotalDuty = dtPass.Compute("Sum(PriceDuty)", "");
                    lblTotalBaggage.Text = (Convert.ToDecimal(TotalBaggage)).ToString("N", nfi);
                    lblTotalMeal.Text = (Convert.ToDecimal(TotalMeal) + Convert.ToDecimal(TotalMeal1)).ToString("N", nfi);
                    lblTotalSport.Text = (Convert.ToDecimal(TotalSport)).ToString("N", nfi);
                    lblTotalComfort.Text = (Convert.ToDecimal(TotalComfort)).ToString("N", nfi);
                    lblTotalInfant.Text = (Convert.ToDecimal(TotalInfant)).ToString("N", nfi);

                    if (Session["dtGridPass2"] != null)
                    {
                        DataTable dtPass2 = (DataTable)Session["dtGridPass2"];
                        TotalBaggage2 = dtPass2.Compute("Sum(PriceBaggage)", "");
                        TotalMeal2 = dtPass2.Compute("Sum(PriceMeal)", "");
                        TotalMeal12 = dtPass2.Compute("Sum(PriceMeal1)", "");
                        TotalSport2 = dtPass2.Compute("Sum(PriceSport)", "");
                        TotalComfort2 = dtPass2.Compute("Sum(PriceComfort)", "");
                        TotalInfant2 = dtPass2.Compute("Sum(PriceInfant)", "");
                        //TotalDuty2 = dtPass2.Compute("Sum(PriceDuty)", "");


                        //lblTotalDuty.Text = (Convert.ToDecimal(TotalDuty)).ToString("N", nfi);

                        lblTotalBaggage2.Text = (Convert.ToDecimal(TotalBaggage2)).ToString("N", nfi);
                        lblTotalMeal2.Text = (Convert.ToDecimal(TotalMeal2) + Convert.ToDecimal(TotalMeal1) + Convert.ToDecimal(TotalMeal12)).ToString("N", nfi);
                        lblTotalSport2.Text = (Convert.ToDecimal(TotalSport2)).ToString("N", nfi);
                        lblTotalComfort2.Text = (Convert.ToDecimal(TotalComfort2)).ToString("N", nfi);
                        lblTotalInfant2.Text = (Convert.ToDecimal(TotalInfant2)).ToString("N", nfi);
                        //lblTotalDuty2.Text = (Convert.ToDecimal(TotalDuty2)).ToString("N", nfi);
                        lblTotalAmount.Text = (Convert.ToDecimal(TotalBaggage) + Convert.ToDecimal(TotalBaggage2) + Convert.ToDecimal(TotalMeal)
                            + Convert.ToDecimal(TotalMeal2) + Convert.ToDecimal(TotalMeal1) + Convert.ToDecimal(TotalMeal12)
                            + Convert.ToDecimal(TotalSport) + Convert.ToDecimal(TotalSport2) + Convert.ToDecimal(TotalComfort)
                            + Convert.ToDecimal(TotalComfort2) + Convert.ToDecimal(TotalInfant) + Convert.ToDecimal(TotalInfant2)).ToString("N", nfi);
                    }
                    else
                    {

                        //lblTotalDuty.Text = (Convert.ToDecimal(TotalDuty)).ToString("N", nfi);
                        lblTotalAmount.Text = (Convert.ToDecimal(TotalBaggage) + Convert.ToDecimal(TotalMeal)
                            + Convert.ToDecimal(TotalMeal1) + Convert.ToDecimal(TotalSport) + Convert.ToDecimal(TotalComfort) + Convert.ToDecimal(TotalInfant)).ToString("N", nfi);
                    }

                    lblCurrency.Text = Curr;
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

        protected void gvPassenger_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {

                if (e.DataColumn.FieldName == "PaxType")
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
                        if (Session["Type"].ToString() == "Child") //check if still allow to change
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

                if (Request.QueryString["change"] != null)
                {
                    if (e.DataColumn.FieldName == "Infant")
                    {
                        e.Cell.Attributes.Add("onclick", "event.cancelBubble = true");
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

        protected void gvPassenger2_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {

                if (e.DataColumn.FieldName == "PaxType")
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
                        if (Session["Type"].ToString() == "Child") //check if still allow to change
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
                else
                {
                    qtyDrink1 = Convert.ToInt32(Session["qtyDrink1"]);
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
                DataTable dtPass2 = new DataTable();
                dtPass2 = (DataTable)Session["dtGridPass2"];
                if (Session["dtGridPass"] == null)
                {
                    GetPassengerList(Session["TransID"].ToString(), "Depart");

                }
                else
                {
                    var args = e.Parameters.Split('|');
                    if (args[0] == "Baggage")
                    {
                        if (Session["dtBaggageDepart"] != null)
                        {
                            DataTable dtBaggage = (DataTable)Session["dtBaggageDepart"];

                            if (cbAllPaxBaggage1.Checked == true)
                            {
                                for (int i = 0; i <= dtPass.Rows.Count - 1; i++)
                                {
                                    if (args[2].ToString() == "")
                                    {
                                        if (Request.QueryString["change"] == null)
                                        {
                                            if (first == 1)
                                            {
                                                DataTable dtdefault = (DataTable)Session["dtdefaultBundle"];
                                                if (dtdefault != null && dtdefault.Rows.Count > 0)
                                                {
                                                    DataRow[] result = dtBaggage.Select("SSRCode = '" + dtdefault.Rows[0]["SYSValue"] + "'");
                                                    foreach (DataRow row in result)
                                                    {
                                                        dtPass.Rows[i]["Baggage"] = row[5];
                                                        dtPass.Rows[i]["SSRCodeBaggage"] = row[0];
                                                        Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                        dtPass.Rows[i]["PriceBaggageS1"] = Convert.ToDecimal(row[3]);
                                                        dtPass.Rows[i]["PriceBaggageS2"] = Convert.ToDecimal(row[4]);
                                                    }
                                                    dtPass.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                                }
                                            }
                                            else
                                            {
                                                dtPass.Rows[i]["Baggage"] = string.Empty;
                                                dtPass.Rows[i]["SSRCodeBaggage"] = string.Empty;
                                                dtPass.Rows[i]["PriceBaggage"] = 0.00;
                                                dtPass.Rows[i]["PriceBaggageS1"] = 0.00;
                                                dtPass.Rows[i]["PriceBaggageS2"] = 0.00;
                                            }
                                        }
                                        else
                                        {
                                            gvPassenger.JSProperties["cp_result"] = msgList.Err200041;
                                            return;
                                        }
                                    }
                                    else
                                    {

                                        dtPass.Rows[i]["Baggage"] = args[2];
                                        dtPass.Rows[i]["SSRCodeBaggage"] = args[3];
                                        DataRow[] result2 = dtBaggage.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result2)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            if (Request.QueryString["change"] != null)
                                            {

                                                if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass.Rows[i]["PriceBaggage"]))
                                                {
                                                    gvPassenger.JSProperties["cp_result"] = msgList.Err200042;
                                                    return;
                                                }
                                                else
                                                {
                                                    //Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                    dtPass.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                                    dtPass.Rows[i]["PriceBaggageS1"] = Convert.ToDecimal(row[3]);
                                                    dtPass.Rows[i]["PriceBaggageS2"] = Convert.ToDecimal(row[4]);
                                                }
                                            }
                                            else
                                            {
                                                //Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                dtPass.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                                dtPass.Rows[i]["PriceBaggageS1"] = Convert.ToDecimal(row[3]);
                                                dtPass.Rows[i]["PriceBaggageS2"] = Convert.ToDecimal(row[4]);
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
                                            //if (Request.QueryString["change"] == null)
                                            //{
                                            //    dtPass.Rows[i]["Baggage"] = string.Empty;
                                            //    dtPass.Rows[i]["SSRCodeBaggage"] = string.Empty;
                                            //    dtPass.Rows[i]["PriceBaggage"] = 0.00;
                                            //    dtPass.Rows[i]["PriceBaggageS1"] = 0.00;
                                            //    dtPass.Rows[i]["PriceBaggageS2"] = 0.00;
                                            //}
                                            //else
                                            //{
                                                gvPassenger.JSProperties["cp_result"] = msgList.Err200041;
                                                return;
                                            //}
                                        }
                                        else
                                        {

                                            dtPass.Rows[i]["Baggage"] = args[2];
                                            dtPass.Rows[i]["SSRCodeBaggage"] = args[3];
                                            DataRow[] result = dtBaggage.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                if (Request.QueryString["change"] != null)
                                                {

                                                    if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass.Rows[i]["PriceBaggage"]))
                                                    {
                                                        gvPassenger.JSProperties["cp_result"] = msgList.Err200042;
                                                        return;
                                                    }
                                                    else
                                                    {
                                                        //Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                        dtPass.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                                        dtPass.Rows[i]["PriceBaggageS1"] = Convert.ToDecimal(row[3]);
                                                        dtPass.Rows[i]["PriceBaggageS2"] = Convert.ToDecimal(row[4]);
                                                    }
                                                }
                                                else
                                                {
                                                    dtPass.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                                    dtPass.Rows[i]["PriceBaggageS1"] = Convert.ToDecimal(row[3]);
                                                    dtPass.Rows[i]["PriceBaggageS2"] = Convert.ToDecimal(row[4]);
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
                    }
                    else if (args[0] == "Meal")
                    {
                        if (Session["dtMealDepart"] != null)
                        {
                            DataTable dtMeal = (DataTable)Session["dtMealDepart"];
                            if (cbAllPaxMeal11.Checked == true)
                            {
                                for (int i = 0; i <= dtPass.Rows.Count - 1; i++)
                                {
                                    if (args[2].ToString() == ",")
                                    {
                                        //if (Request.QueryString["change"] != null)
                                        //{
                                            gvPassenger.JSProperties["cp_result"] = msgList.Err200044;
                                            return;
                                        //}
                                        //else
                                        //{
                                        //    dtPass.Rows[i]["Meal"] = string.Empty;
                                        //    dtPass.Rows[i]["SSRCodeMeal"] = string.Empty;
                                        //    dtPass.Rows[i]["PriceMeal"] = 0.00;
                                        //}
                                    }
                                    else
                                    {
                                        //GetSellSSR(signature);
                                        DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            if (Request.QueryString["change"] != null)
                                            {

                                                if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass.Rows[i]["PriceMeal"]))
                                                {
                                                    gvPassenger.JSProperties["cp_result"] = msgList.Err200045;
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass.Rows[i]["Meal"] = args[2];
                                                    dtPass.Rows[i]["SSRCodeMeal"] = args[3];
                                                    dtPass.Rows[i]["PriceMeal"] = Convert.ToDecimal(Detail);
                                                }
                                            }
                                            else
                                            {
                                                dtPass.Rows[i]["Meal"] = args[2];
                                                dtPass.Rows[i]["SSRCodeMeal"] = args[3];
                                                dtPass.Rows[i]["PriceMeal"] = Convert.ToDecimal(Detail);
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
                                            //if (Request.QueryString["change"] != null)
                                            //{
                                                gvPassenger.JSProperties["cp_result"] = msgList.Err200044;
                                                return;
                                            //}
                                            //else
                                            //{
                                            //    dtPass.Rows[i]["Meal"] = string.Empty;
                                            //    dtPass.Rows[i]["SSRCodeMeal"] = string.Empty;
                                            //    dtPass.Rows[i]["PriceMeal"] = 0.00;
                                            //}
                                        }
                                        else
                                        {
                                            //GetSellSSR(signature);

                                            DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                if (Request.QueryString["change"] != null)
                                                {
                                                        if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass.Rows[i]["PriceMeal"]))
                                                    {
                                                        gvPassenger.JSProperties["cp_result"] = msgList.Err200045;
                                                        return;
                                                    }
                                                    else
                                                    {
                                                        dtPass.Rows[i]["Meal"] = args[2];
                                                        dtPass.Rows[i]["SSRCodeMeal"] = args[3];
                                                        dtPass.Rows[i]["PriceMeal"] = Convert.ToDecimal(Detail);
                                                    }
                                                }
                                                else
                                                {
                                                    dtPass.Rows[i]["Meal"] = args[2];
                                                    dtPass.Rows[i]["SSRCodeMeal"] = args[3];
                                                    dtPass.Rows[i]["PriceMeal"] = Convert.ToDecimal(Detail);
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
                        }
                        if (Session["dtDrinkDepart"] != null)
                        {
                            DataTable dtDrink = (DataTable)Session["dtDrinkDepart"];
                            if (cbAllPaxMeal11.Checked == true)
                            {
                                for (int i = 0; i <= dtPass.Rows.Count - 1; i++)
                                {
                                    if (args[2].ToString() == "")
                                    {
                                        //if (Request.QueryString["change"] != null)
                                        //{
                                            gvPassenger.JSProperties["cp_result"] = msgList.Err200047;
                                            return;
                                        //}
                                        //else
                                        //{
                                        //    dtPass.Rows[i]["Drink"] = string.Empty;
                                        //    dtPass.Rows[i]["SSRCodeDrink"] = string.Empty;
                                        //    dtPass.Rows[i]["PriceDrink"] = 0.00;
                                        //}
                                    }
                                    else
                                    {
                                        //string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                        //if (Session["dtArrayDrink"] != null)
                                        //{
                                        //DataTable DefaultDrink = (DataTable)Session["dtArrayDrink"];
                                        for (int c = 0; c < dtDrink.Rows.Count; c++)
                                        {
                                            // DataRow[] result = dtDrink.Select("SSRCode LIKE '" + DefaultDrink.Rows[c]["SYSValueEx"] + "%'");
                                            // if (result.Length > 0)
                                            //{
                                            //foreach (DataRow row in result)
                                            //{
                                            Detail = dtDrink.Rows[0]["Price"].ToString().Substring(0, dtDrink.Rows[0]["Price"].ToString().Length - 4);
                                            dtPass.Rows[i]["PriceDrink"] = Convert.ToDecimal(Detail);
                                            dtPass.Rows[i]["Drink"] = dtDrink.Rows[0]["ConcatenatedField"];
                                            dtPass.Rows[i]["SSRCodeDrink"] = dtDrink.Rows[0]["SSRCode"];
                                            //}
                                            break;
                                            //}
                                        }
                                        //}
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
                                            //if (Request.QueryString["change"] != null)
                                            //{
                                                gvPassenger.JSProperties["cp_result"] = msgList.Err200047;
                                                return;
                                            //}
                                            //else
                                            //{
                                            //    dtPass.Rows[i]["Drink"] = string.Empty;
                                            //    dtPass.Rows[i]["SSRCodeDrink"] = string.Empty;
                                            //    dtPass.Rows[i]["PriceDrink"] = 0.00;
                                            //}
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
                                                dtPass.Rows[i]["PriceDrink"] = Convert.ToDecimal(Detail);
                                                dtPass.Rows[i]["Drink"] = dtDrink.Rows[0]["ConcatenatedField"];
                                                dtPass.Rows[i]["SSRCodeDrink"] = dtDrink.Rows[0]["SSRCode"];
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
                        if (Session["dtMealDepart2"] != null)
                        {
                            DataTable dtMeal = (DataTable)Session["dtMealDepart2"];
                            if (cbAllPaxMeal21.Checked == true)
                            {
                                for (int i = 0; i <= dtPass.Rows.Count - 1; i++)
                                {
                                    if (args[2].ToString() == ",")
                                    {
                                        //if (Request.QueryString["change"] != null)
                                        //{
                                            gvPassenger.JSProperties["cp_result"] = msgList.Err200048;
                                            return;
                                        //}
                                        //else
                                        //{
                                        //    dtPass.Rows[i]["Meal1"] = string.Empty;
                                        //    dtPass.Rows[i]["SSRCodeMeal1"] = string.Empty;
                                        //    dtPass.Rows[i]["PriceMeal1"] = 0.00;
                                        //}
                                    }
                                    else
                                    {

                                        DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            if (Request.QueryString["change"] != null)
                                            {
                                                if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass.Rows[i]["PriceMeal1"]))
                                                {
                                                    gvPassenger.JSProperties["cp_result"] = msgList.Err200049;
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass.Rows[i]["Meal1"] = args[2];
                                                    dtPass.Rows[i]["SSRCodeMeal1"] = args[3];
                                                    dtPass.Rows[i]["PriceMeal1"] = Convert.ToDecimal(Detail);
                                                }
                                            }
                                            else
                                            {
                                                dtPass.Rows[i]["Meal1"] = args[2];
                                                dtPass.Rows[i]["SSRCodeMeal1"] = args[3];
                                                dtPass.Rows[i]["PriceMeal1"] = Convert.ToDecimal(Detail);
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
                                            //if (Request.QueryString["change"] != null)
                                            //{
                                                gvPassenger.JSProperties["cp_result"] = msgList.Err200048;
                                                return;
                                            //}
                                            //else
                                            //{
                                            //    dtPass.Rows[i]["Meal1"] = string.Empty;
                                            //    dtPass.Rows[i]["SSRCodeMeal1"] = string.Empty;
                                            //    dtPass.Rows[i]["PriceMeal1"] = 0.00;
                                            //}
                                        }
                                        else
                                        {

                                            DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                if (Request.QueryString["change"] != null)
                                                {
                                                    if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass.Rows[i]["PriceMeal1"]))
                                                    {
                                                        gvPassenger.JSProperties["cp_result"] = msgList.Err200049;
                                                        return;
                                                    }
                                                    else
                                                    {
                                                        dtPass.Rows[i]["PriceMeal1"] = Convert.ToDecimal(Detail);
                                                        dtPass.Rows[i]["Meal1"] = args[2];
                                                        dtPass.Rows[i]["SSRCodeMeal1"] = args[3];
                                                    }
                                                }
                                                else
                                                {
                                                    dtPass.Rows[i]["PriceMeal1"] = Convert.ToDecimal(Detail);
                                                    dtPass.Rows[i]["Meal1"] = args[2];
                                                    dtPass.Rows[i]["SSRCodeMeal1"] = args[3];
                                                }
                                            }
                                            //dtPass.Rows[i]["PriceMeal1"] = Convert.ToDecimal(Detail);
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
                        }
                        if (Session["dtDrinkDepart2"] != null)
                        {
                            DataTable dtDrink = (DataTable)Session["dtDrinkDepart2"];
                            if (cbAllPaxMeal21.Checked == true)
                            {
                                for (int i = 0; i <= dtPass.Rows.Count - 1; i++)
                                {
                                    if (args[2].ToString() == "")
                                    {
                                        //if (Request.QueryString["change"] != null)
                                        //{
                                            gvPassenger.JSProperties["cp_result"] = msgList.Err200051;
                                            return;
                                        //}
                                        //else
                                        //{
                                        //    dtPass.Rows[i]["Drink1"] = string.Empty;
                                        //    dtPass.Rows[i]["SSRCodeDrink1"] = string.Empty;
                                        //    dtPass.Rows[i]["PriceDrink1"] = 0.00;
                                        //}
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
                                            dtPass.Rows[i]["PriceDrink1"] = Convert.ToDecimal(Detail);
                                            dtPass.Rows[i]["Drink1"] = dtDrink.Rows[0]["ConcatenatedField"];
                                            dtPass.Rows[i]["SSRCodeDrink1"] = dtDrink.Rows[0]["SSRCode"];
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
                                            //if (Request.QueryString["change"] != null)
                                            //{
                                                gvPassenger.JSProperties["cp_result"] = msgList.Err200051;
                                                return;
                                            //}
                                            //else
                                            //{
                                            //    dtPass.Rows[i]["Drink1"] = string.Empty;
                                            //    dtPass.Rows[i]["SSRCodeDrink1"] = string.Empty;
                                            //    dtPass.Rows[i]["PriceDrink1"] = 0.00;
                                            //}
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
                                                dtPass.Rows[i]["PriceDrink1"] = Convert.ToDecimal(Detail);
                                                dtPass.Rows[i]["Drink1"] = dtDrink.Rows[0]["ConcatenatedField"];
                                                dtPass.Rows[i]["SSRCodeDrink1"] = dtDrink.Rows[0]["SSRCode"];
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
                        if (Session["dtSportDepart"] != null)
                        {
                            DataTable dtSport = (DataTable)Session["dtSportDepart"];
                            if (cbAllPaxSport1.Checked == true)
                            {
                                for (int i = 0; i <= dtPass.Rows.Count - 1; i++)
                                {
                                    if (args[2].ToString() == "")
                                    {
                                        //if (Request.QueryString["change"] != null)
                                        //{
                                            gvPassenger.JSProperties["cp_result"] = msgList.Err200052;
                                            return;
                                        //}
                                        //else
                                        //{
                                        //    dtPass.Rows[i]["Sport"] = string.Empty;
                                        //    dtPass.Rows[i]["SSRCodeSport"] = string.Empty;
                                        //    dtPass.Rows[i]["PriceSport"] = 0.00;
                                        //    dtPass.Rows[i]["PriceSportS1"] = 0.00;
                                        //    dtPass.Rows[i]["PriceSportS2"] = 0.00;
                                        //}
                                    }
                                    else
                                    {

                                        dtPass.Rows[i]["Sport"] = args[2];
                                        dtPass.Rows[i]["SSRCodeSport"] = args[3];
                                        DataRow[] result = dtSport.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            if (Request.QueryString["change"] != null)
                                            {
                                                if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass.Rows[i]["PriceSport"]))
                                                {
                                                    gvPassenger.JSProperties["cp_result"] = msgList.Err200053;
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass.Rows[i]["PriceSport"] = Convert.ToDecimal(Detail);
                                                    dtPass.Rows[i]["PriceSportS1"] = Convert.ToDecimal(row[3]);
                                                    dtPass.Rows[i]["PriceSportS2"] = Convert.ToDecimal(row[4]);
                                                }
                                            }
                                            else
                                            {
                                                dtPass.Rows[i]["PriceSport"] = Convert.ToDecimal(Detail);
                                                dtPass.Rows[i]["PriceSportS1"] = Convert.ToDecimal(row[3]);
                                                dtPass.Rows[i]["PriceSportS2"] = Convert.ToDecimal(row[4]);
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
                                            //if (Request.QueryString["change"] != null)
                                            //{
                                                gvPassenger.JSProperties["cp_result"] = msgList.Err200052;
                                                return;
                                            //}
                                            //else
                                            //{
                                            //    dtPass.Rows[i]["Sport"] = string.Empty;
                                            //    dtPass.Rows[i]["SSRCodeSport"] = string.Empty;
                                            //    dtPass.Rows[i]["PriceSport"] = 0.00;
                                            //    dtPass.Rows[i]["PriceSportS1"] = 0.00;
                                            //    dtPass.Rows[i]["PriceSportS2"] = 0.00;
                                            //}
                                        }
                                        else
                                        {
                                            dtPass.Rows[i]["Sport"] = args[2];
                                            dtPass.Rows[i]["SSRCodeSport"] = args[3];
                                            DataRow[] result = dtSport.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                if (Request.QueryString["change"] != null)
                                                {
                                                    if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass.Rows[i]["PriceSport"]))
                                                    {
                                                        gvPassenger.JSProperties["cp_result"] = msgList.Err200053;
                                                        return;
                                                    }
                                                    else
                                                    {
                                                        dtPass.Rows[i]["PriceSport"] = Convert.ToDecimal(Detail);
                                                        dtPass.Rows[i]["PriceSportS1"] = Convert.ToDecimal(row[3]);
                                                        dtPass.Rows[i]["PriceSportS2"] = Convert.ToDecimal(row[4]);
                                                    }
                                                }
                                                else
                                                {
                                                    dtPass.Rows[i]["PriceSport"] = Convert.ToDecimal(Detail);
                                                    dtPass.Rows[i]["PriceSportS1"] = Convert.ToDecimal(row[3]);
                                                    dtPass.Rows[i]["PriceSportS2"] = Convert.ToDecimal(row[4]);
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
                    }
                    else if (args[0] == "Duty")
                    {
                        if (Session["dtDutyDepart"] != null)
                        {
                            DataTable dtDuty = (DataTable)Session["dtDutyDepart"];
                            if (((Convert.ToInt32(args[1]) + qtyDuty) - 1) <= dtPass.Rows.Count)
                            {
                                for (int i = qtyDuty; i <= (Convert.ToInt32(args[1]) + qtyDuty) - 1; i++)
                                {
                                    dtPass.Rows[i]["Duty"] = args[2];
                                    dtPass.Rows[i]["SSRCodeDuty"] = args[3];
                                    DataRow[] result = dtDuty.Select("SSRCode = '" + args[3] + "'");
                                    foreach (DataRow row in result)
                                    {
                                        Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                        dtPass.Rows[i]["PriceDuty"] = Convert.ToDecimal(Detail);
                                        dtPass.Rows[i]["PriceDutyS1"] = Convert.ToDecimal(row[3]);
                                        dtPass.Rows[i]["PriceDutyS2"] = Convert.ToDecimal(row[4]);
                                    }
                                    //dtPass.Rows[i]["PriceDuty"] = Convert.ToDecimal(Detail);
                                }
                                Session["dtGridPass"] = dtPass;
                                if (args[2] != ",")
                                {
                                    Session["qtyDuty"] = (Convert.ToInt32(args[1]) + qtySport);
                                }
                                gvPassenger.DataSource = dtPass;
                                gvPassenger.DataBind();
                            }
                            else
                            {
                                gvPassenger.JSProperties["cp_result"] = msgList.Err200055;
                                return;
                            }
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
                                    //if (Request.QueryString["change"] != null)
                                    //{
                                        gvPassenger.JSProperties["cp_result"] = msgList.Err200056;
                                        return;
                                    //}
                                    //else
                                    //{
                                    //    dtPass.Rows[i]["Comfort"] = string.Empty;
                                    //    dtPass.Rows[i]["SSRCodeComfort"] = string.Empty;
                                    //    dtPass.Rows[i]["PriceComfort"] = 0.00;
                                    //    dtPass.Rows[i]["PriceComfortS1"] = 0.00;
                                    //    dtPass.Rows[i]["PriceComfortS2"] = 0.00;
                                    //}
                                }
                                else
                                {

                                    DataRow[] result = dtComfort.Select("SSRCode = '" + args[3] + "'");
                                    foreach (DataRow row in result)
                                    {
                                        Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                        if (Request.QueryString["change"] != null)
                                        {
                                            if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass.Rows[i]["PriceComfort"]))
                                            {
                                                gvPassenger.JSProperties["cp_result"] = msgList.Err200057;
                                                return;
                                            }
                                            else
                                            {
                                                dtPass.Rows[i]["Comfort"] = args[2];
                                                dtPass.Rows[i]["SSRCodeComfort"] = args[3];
                                                dtPass.Rows[i]["PriceComfort"] = Convert.ToDecimal(Detail);
                                                dtPass.Rows[i]["PriceComfortS1"] = Convert.ToDecimal(row[3]);
                                                dtPass.Rows[i]["PriceComfortS2"] = Convert.ToDecimal(row[4]);
                                            }
                                        }
                                        else
                                        {
                                            dtPass.Rows[i]["Comfort"] = args[2];
                                            dtPass.Rows[i]["SSRCodeComfort"] = args[3];
                                            dtPass.Rows[i]["PriceComfort"] = Convert.ToDecimal(Detail);
                                            dtPass.Rows[i]["PriceComfortS1"] = Convert.ToDecimal(row[3]);
                                            dtPass.Rows[i]["PriceComfortS2"] = Convert.ToDecimal(row[4]);
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
                                        if (Request.QueryString["change"] != null)
                                        {
                                            if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass.Rows[i]["PriceComfort"]))
                                            {
                                                gvPassenger.JSProperties["cp_result"] = msgList.Err200057;
                                                return;
                                            }
                                            else
                                            {
                                                dtPass.Rows[i]["Comfort"] = string.Empty;
                                                dtPass.Rows[i]["SSRCodeComfort"] = string.Empty;
                                                dtPass.Rows[i]["PriceComfort"] = 0.00;
                                                dtPass.Rows[i]["PriceComfortS1"] = 0.00;
                                                dtPass.Rows[i]["PriceComfortS2"] = 0.00;
                                            }
                                        }
                                        else
                                        {
                                            gvPassenger.JSProperties["cp_result"] = msgList.Err200056;
                                            return;

                                            //dtPass.Rows[i]["Comfort"] = string.Empty;
                                            //dtPass.Rows[i]["SSRCodeComfort"] = string.Empty;
                                            //dtPass.Rows[i]["PriceComfort"] = 0.00;
                                            //dtPass.Rows[i]["PriceComfortS1"] = 0.00;
                                            //dtPass.Rows[i]["PriceComfortS2"] = 0.00;
                                        }
                                    }
                                    else
                                    {

                                        DataRow[] result = dtComfort.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            if (Request.QueryString["change"] != null)
                                            {
                                                if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass.Rows[i]["PriceComfort"]))
                                                {
                                                    gvPassenger.JSProperties["cp_result"] = msgList.Err200057;
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass.Rows[i]["Comfort"] = args[2];
                                                    dtPass.Rows[i]["SSRCodeComfort"] = args[3];
                                                    dtPass.Rows[i]["PriceComfort"] = Convert.ToDecimal(Detail);
                                                    dtPass.Rows[i]["PriceComfortS1"] = Convert.ToDecimal(row[3]);
                                                    dtPass.Rows[i]["PriceComfortS2"] = Convert.ToDecimal(row[4]);
                                                }
                                            }
                                            else
                                            {
                                                dtPass.Rows[i]["Comfort"] = args[2];
                                                dtPass.Rows[i]["SSRCodeComfort"] = args[3];
                                                dtPass.Rows[i]["PriceComfort"] = Convert.ToDecimal(Detail);
                                                dtPass.Rows[i]["PriceComfortS1"] = Convert.ToDecimal(row[3]);
                                                dtPass.Rows[i]["PriceComfortS2"] = Convert.ToDecimal(row[4]);
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
                                gvPassenger.JSProperties["cp_result"] = msgList.Err200058;
                                return;
                            }
                        }
                    }
                    else if (args[0] == "Clear")
                    {
                        if (first == 1)
                        {
                            DataTable dtdefault = (DataTable)Session["dtdefaultBundle"];
                            if (dtdefault != null && dtdefault.Rows.Count > 0)
                            {
                                DataTable dtBaggage = (DataTable)Session["dtBaggageDepart"];

                                DataRow[] result = dtBaggage.Select("SSRCode = '" + dtdefault.Rows[0]["SYSValue"] + "'");
                                foreach (DataRow row in result)
                                {
                                    dtPass.Rows[Convert.ToInt16(args[1])]["Baggage"] = row[5];
                                    dtPass.Rows[Convert.ToInt16(args[1])]["SSRCodeBaggage"] = row[0];
                                    Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                    dtPass.Rows[Convert.ToInt16(args[1])]["PriceBaggageS1"] = Convert.ToDecimal(row[3]);
                                    dtPass.Rows[Convert.ToInt16(args[1])]["PriceBaggageS2"] = Convert.ToDecimal(row[4]);
                                }
                                dtPass.Rows[Convert.ToInt16(args[1])]["PriceBaggage"] = Convert.ToDecimal(Detail);

                            }
                        }
                        else
                        {
                            dtPass.Rows[Convert.ToInt16(args[1])]["Baggage"] = string.Empty;
                            dtPass.Rows[Convert.ToInt16(args[1])]["SSRCodeBaggage"] = string.Empty;
                            dtPass.Rows[Convert.ToInt16(args[1])]["PriceBaggage"] = 0.00;
                            dtPass.Rows[Convert.ToInt16(args[1])]["PriceBaggageS1"] = 0.00;
                            dtPass.Rows[Convert.ToInt16(args[1])]["PriceBaggageS2"] = 0.00;
                            Session["qtyBaggage"] = Convert.ToInt32(Session["qtyBaggage"]) - 1;
                            //qtyBaggage = qtyBaggage - 1;
                        }
                        dtPass.Rows[Convert.ToInt16(args[1])]["Meal"] = string.Empty;
                        dtPass.Rows[Convert.ToInt16(args[1])]["SSRCodeMeal"] = string.Empty;
                        dtPass.Rows[Convert.ToInt16(args[1])]["PriceMeal"] = 0.00;

                        dtPass.Rows[Convert.ToInt16(args[1])]["Meal1"] = string.Empty;
                        dtPass.Rows[Convert.ToInt16(args[1])]["SSRCodeMeal1"] = string.Empty;
                        dtPass.Rows[Convert.ToInt16(args[1])]["PriceMeal1"] = 0.00;

                        dtPass.Rows[Convert.ToInt16(args[1])]["Drink"] = string.Empty;
                        dtPass.Rows[Convert.ToInt16(args[1])]["SSRCodeDrink"] = string.Empty;
                        dtPass.Rows[Convert.ToInt16(args[1])]["PriceDrink"] = 0.00;

                        dtPass.Rows[Convert.ToInt16(args[1])]["Drink1"] = string.Empty;
                        dtPass.Rows[Convert.ToInt16(args[1])]["SSRCodeDrink1"] = string.Empty;
                        dtPass.Rows[Convert.ToInt16(args[1])]["PriceDrink1"] = 0.00;

                        dtPass.Rows[Convert.ToInt16(args[1])]["Sport"] = string.Empty;
                        dtPass.Rows[Convert.ToInt16(args[1])]["SSRCodeSport"] = string.Empty;
                        dtPass.Rows[Convert.ToInt16(args[1])]["PriceSport"] = 0.00;
                        dtPass.Rows[Convert.ToInt16(args[1])]["PriceSportS1"] = 0.00;
                        dtPass.Rows[Convert.ToInt16(args[1])]["PriceSportS2"] = 0.00;

                        dtPass.Rows[Convert.ToInt16(args[1])]["Duty"] = string.Empty;
                        dtPass.Rows[Convert.ToInt16(args[1])]["SSRCodeDuty"] = string.Empty;
                        dtPass.Rows[Convert.ToInt16(args[1])]["PriceDuty"] = 0.00;
                        dtPass.Rows[Convert.ToInt16(args[1])]["PriceDutyS1"] = 0.00;
                        dtPass.Rows[Convert.ToInt16(args[1])]["PriceDutyS2"] = 0.00;

                        dtPass.Rows[Convert.ToInt16(args[1])]["Comfort"] = string.Empty;
                        dtPass.Rows[Convert.ToInt16(args[1])]["SSRCodeComfort"] = string.Empty;
                        dtPass.Rows[Convert.ToInt16(args[1])]["PriceComfort"] = 0.00;
                        dtPass.Rows[Convert.ToInt16(args[1])]["PriceComfortS1"] = 0.00;
                        dtPass.Rows[Convert.ToInt16(args[1])]["PriceComfortS2"] = 0.00;

                        dtPass.Rows[Convert.ToInt16(args[1])]["Infant"] = string.Empty;
                        dtPass.Rows[Convert.ToInt16(args[1])]["SSRCodeInfant"] = string.Empty;
                        dtPass.Rows[Convert.ToInt16(args[1])]["PriceInfant"] = 0.00;
                        dtPass.Rows[Convert.ToInt16(args[1])]["PriceInfantS1"] = 0.00;
                        dtPass.Rows[Convert.ToInt16(args[1])]["PriceInfantS2"] = 0.00;

                        if (dtPass2 != null && dtPass2.Rows[Convert.ToInt16(args[1])]["Infant"].ToString() != string.Empty)
                        {
                            dtPass2.Rows[Convert.ToInt16(args[1])]["Infant"] = string.Empty;
                            dtPass2.Rows[Convert.ToInt16(args[1])]["SSRCodeInfant"] = string.Empty;
                            dtPass2.Rows[Convert.ToInt16(args[1])]["PriceInfant"] = 0.00;
                            dtPass2.Rows[Convert.ToInt16(args[1])]["PriceInfantS1"] = 0.00;
                            dtPass2.Rows[Convert.ToInt16(args[1])]["PriceInfantS2"] = 0.00;
                            gvPassenger.JSProperties["cp_result"] = "Infant";
                            Session["dtGridPass2"] = dtPass2;
                        }


                        Session["dtGridPass"] = dtPass;
                        gvPassenger.DataSource = dtPass;
                        gvPassenger.DataBind();

                        Session["qtyMeal"] = Convert.ToInt32(Session["qtyMeal"]) - 1;
                        Session["qtyMeal1"] = Convert.ToInt32(Session["qtyMeal1"]) - 1;
                        Session["qtySport"] = Convert.ToInt32(Session["qtySport"]) - 1;
                        Session["qtyComfort"] = Convert.ToInt32(Session["qtyComfort"]) - 1;
                        Session["qtyDrink"] = Convert.ToInt32(Session["qtyDrink"]) - 1;
                        Session["qtyDrink1"] = Convert.ToInt32(Session["qtyDrink1"]) - 1;
                        //qtyMeal = qtyMeal - 1;
                        //qtyMeal1 = qtyMeal1 - 1;
                        //qtySport = qtySport - 1;
                        //qtyComfort = qtyComfort - 1;
                        //qtyDrink = qtyDrink - 1;
                        //qtyDrink1 = qtyDrink1 - 1;
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
                DataTable dtPass = new DataTable();
                dtPass = (DataTable)Session["dtGridPass"];
                if (Session["dtGridPass2"] == null)
                {
                    GetPassengerList(Session["TransID"].ToString(), "Return");
                }
                else
                {
                    var args = e.Parameters.Split('|');
                    if (args[0] == "Baggage")
                    {
                        if (Session["dtBaggageReturn"] != null)
                        {
                            DataTable dtBaggage = (DataTable)Session["dtBaggageReturn"];
                            if (cbAllBaggage2.Checked == true)
                            {
                                for (int i = 0; i <= dtPass2.Rows.Count - 1; i++)
                                {
                                    if (args[2].ToString() == "")
                                    {
                                        if (Request.QueryString["change"] == null)
                                        {
                                            if (first == 1)
                                            {
                                                DataTable dtdefault = (DataTable)Session["dtdefaultBundle"];
                                                if (dtdefault != null && dtdefault.Rows.Count > 0)
                                                {
                                                    DataRow[] result = dtBaggage.Select("SSRCode = '" + dtdefault.Rows[0]["SYSValue"] + "'");
                                                    foreach (DataRow row in result)
                                                    {
                                                        dtPass2.Rows[i]["Baggage"] = row[5];
                                                        dtPass2.Rows[i]["SSRCodeBaggage"] = row[0];
                                                        Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                    }
                                                    dtPass2.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                                }
                                            }
                                            else
                                            {
                                                //dtPass2.Rows[i]["Baggage"] = string.Empty;
                                                //dtPass2.Rows[i]["SSRCodeBaggage"] = string.Empty;
                                                //dtPass2.Rows[i]["PriceBaggage"] = 0.00;
                                                //dtPass2.Rows[i]["PriceBaggageS1"] = 0.00;
                                                //dtPass2.Rows[i]["PriceBaggageS2"] = 0.00;
                                                gvPassenger2.JSProperties["cp_result"] = msgList.Err200041;
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            gvPassenger2.JSProperties["cp_result"] = msgList.Err200041;
                                            return;
                                        }
                                    }
                                    else
                                    {


                                        DataRow[] result2 = dtBaggage.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result2)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            if (Request.QueryString["change"] != null)
                                            {
                                                if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass2.Rows[i]["PriceBaggage"]))
                                                {
                                                    gvPassenger2.JSProperties["cp_result"] = msgList.Err200042;
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass2.Rows[i]["Baggage"] = args[2];
                                                    dtPass2.Rows[i]["SSRCodeBaggage"] = args[3];
                                                    dtPass2.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                                    dtPass2.Rows[i]["PriceBaggageS1"] = Convert.ToDecimal(row[3]);
                                                    dtPass2.Rows[i]["PriceBaggageS2"] = Convert.ToDecimal(row[4]);
                                                }
                                            }
                                            else
                                            {
                                                dtPass2.Rows[i]["Baggage"] = args[2];
                                                dtPass2.Rows[i]["SSRCodeBaggage"] = args[3];
                                                dtPass2.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                                dtPass2.Rows[i]["PriceBaggageS1"] = Convert.ToDecimal(row[3]);
                                                dtPass2.Rows[i]["PriceBaggageS2"] = Convert.ToDecimal(row[4]);
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
                                            if (Request.QueryString["change"] != null)
                                            {
                                                gvPassenger2.JSProperties["cp_result"] = msgList.Err200041;
                                                return;
                                            }
                                            else
                                            {
                                                if (first == 1)
                                                {
                                                    DataTable dtdefault = (DataTable)Session["dtdefaultBundle"];
                                                    if (dtdefault != null && dtdefault.Rows.Count > 0)
                                                    {
                                                        DataRow[] result = dtBaggage.Select("SSRCode = '" + dtdefault.Rows[0]["SYSValue"] + "'");
                                                        foreach (DataRow row in result)
                                                        {
                                                            dtPass2.Rows[i]["Baggage"] = row[3];
                                                            dtPass2.Rows[i]["SSRCodeBaggage"] = row[0];
                                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                            dtPass2.Rows[i]["PriceBaggageS1"] = Convert.ToDecimal(row[3]);
                                                            dtPass2.Rows[i]["PriceBaggageS2"] = Convert.ToDecimal(row[4]);
                                                        }
                                                        dtPass2.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);

                                                    }
                                                }
                                                else
                                                {
                                                    //dtPass2.Rows[i]["Baggage"] = string.Empty;
                                                    //dtPass2.Rows[i]["SSRCodeBaggage"] = string.Empty;
                                                    //dtPass2.Rows[i]["PriceBaggage"] = 0.00;
                                                    //dtPass2.Rows[i]["PriceBaggageS1"] = 0.00;
                                                    //dtPass2.Rows[i]["PriceBaggageS2"] = 0.00;
                                                    gvPassenger2.JSProperties["cp_result"] = msgList.Err200041;
                                                    return;
                                                }
                                            }
                                        }
                                        else
                                        {

                                            DataRow[] result = dtBaggage.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                if (Request.QueryString["change"] != null)
                                                {
                                                    if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass2.Rows[i]["PriceBaggage"]))
                                                    {
                                                        gvPassenger2.JSProperties["cp_result"] = msgList.Err200042;
                                                        return;
                                                    }
                                                    else
                                                    {
                                                        dtPass2.Rows[i]["Baggage"] = args[2];
                                                        dtPass2.Rows[i]["SSRCodeBaggage"] = args[3];
                                                        dtPass2.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                                        dtPass2.Rows[i]["PriceBaggageS1"] = Convert.ToDecimal(row[3]);
                                                        dtPass2.Rows[i]["PriceBaggageS2"] = Convert.ToDecimal(row[4]);
                                                    }
                                                }
                                                else
                                                {

                                                    dtPass2.Rows[i]["Baggage"] = args[2];
                                                    dtPass2.Rows[i]["SSRCodeBaggage"] = args[3];
                                                    dtPass2.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                                    dtPass2.Rows[i]["PriceBaggageS1"] = Convert.ToDecimal(row[3]);
                                                    dtPass2.Rows[i]["PriceBaggageS2"] = Convert.ToDecimal(row[4]);
                                                }
                                            }
                                            //dtPass2.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
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
                    }
                    else if (args[0] == "Meal")
                    {
                        if (Session["dtMealReturn"] != null)
                        {
                            DataTable dtMeal = (DataTable)Session["dtMealReturn"];
                            if (cbAllPaxMeal12.Checked == true)
                            {
                                for (int i = 0; i <= dtPass2.Rows.Count - 1; i++)
                                {
                                    if (args[2].ToString() == ",")
                                    {
                                        //if (Request.QueryString["change"] != null)
                                        //{
                                            gvPassenger2.JSProperties["cp_result"] = msgList.Err200044;
                                            return;
                                        //}
                                        //else
                                        //{
                                        //    dtPass2.Rows[i]["Meal"] = string.Empty;
                                        //    dtPass2.Rows[i]["SSRCodeMeal"] = string.Empty;
                                        //    dtPass2.Rows[i]["PriceMeal"] = 0.00;
                                        //}
                                    }
                                    else
                                    {

                                        DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            if (Request.QueryString["change"] != null)
                                            {
                                                //Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass2.Rows[i]["PriceMeal"]))
                                                {
                                                    gvPassenger2.JSProperties["cp_result"] = msgList.Err200045;
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass2.Rows[i]["Meal"] = args[2];
                                                    dtPass2.Rows[i]["SSRCodeMeal"] = args[3];
                                                    dtPass2.Rows[i]["PriceMeal"] = Convert.ToDecimal(Detail);
                                                }
                                            }
                                            else
                                            {
                                                dtPass2.Rows[i]["Meal"] = args[2];
                                                dtPass2.Rows[i]["SSRCodeMeal"] = args[3];
                                                dtPass2.Rows[i]["PriceMeal"] = Convert.ToDecimal(Detail);
                                            }
                                        }
                                    }
                                }
                                if (Session["dtDrinkReturn"] == null)
                                {
                                    Session["dtGridPass2"] = dtPass2;
                                    Session["qtyMeal2"] = 0;
                                    gvPassenger2.DataSource = dtPass2;
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
                                            //if (Request.QueryString["change"] != null)
                                            //{
                                                gvPassenger2.JSProperties["cp_result"] = msgList.Err200044;
                                                return;
                                            //}
                                            //else
                                            //{
                                            //    dtPass2.Rows[i]["Meal"] = string.Empty;
                                            //    dtPass2.Rows[i]["SSRCodeMeal"] = string.Empty;
                                            //    dtPass2.Rows[i]["PriceMeal"] = 0.00;
                                            //}
                                        }
                                        else
                                        {

                                            DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                if (Request.QueryString["change"] != null)
                                                {
                                                    if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass2.Rows[i]["PriceMeal"]))
                                                    {
                                                        gvPassenger2.JSProperties["cp_result"] = msgList.Err200045;
                                                        return;
                                                    }
                                                    else
                                                    {
                                                        dtPass2.Rows[i]["Meal"] = args[2];
                                                        dtPass2.Rows[i]["SSRCodeMeal"] = args[3];
                                                        dtPass2.Rows[i]["PriceMeal"] = Convert.ToDecimal(Detail);
                                                    }
                                                }
                                                else
                                                {
                                                    dtPass2.Rows[i]["Meal"] = args[2];
                                                    dtPass2.Rows[i]["SSRCodeMeal"] = args[3];
                                                    dtPass2.Rows[i]["PriceMeal"] = Convert.ToDecimal(Detail);
                                                }
                                            }
                                            //dtPass2.Rows[i]["PriceMeal"] = Convert.ToDecimal(Detail);
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
                        }
                        if (Session["dtDrinkReturn"] != null)
                        {
                            DataTable dtDrink = (DataTable)Session["dtDrinkReturn"];
                            if (cbAllPaxMeal12.Checked == true)
                            {
                                for (int i = 0; i <= dtPass2.Rows.Count - 1; i++)
                                {
                                    if (args[2].ToString() == "")
                                    {
                                        //if (Request.QueryString["change"] != null)
                                        //{
                                            gvPassenger2.JSProperties["cp_result"] = msgList.Err200047;
                                            return;
                                        //}
                                        //else
                                        //{
                                        //    dtPass2.Rows[i]["Drink"] = string.Empty;
                                        //    dtPass2.Rows[i]["SSRCodeDrink"] = string.Empty;
                                        //    dtPass2.Rows[i]["PriceDrink"] = 0.00;
                                        //}
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
                                            dtPass2.Rows[i]["PriceDrink"] = Convert.ToDecimal(Detail);
                                            dtPass2.Rows[i]["Drink"] = dtDrink.Rows[0]["ConcatenatedField"];
                                            dtPass2.Rows[i]["SSRCodeDrink"] = dtDrink.Rows[0]["SSRCode"];
                                            //}
                                            break;
                                            //}
                                        }
                                        //}
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
                                            //if (Request.QueryString["change"] != null)
                                            //{
                                                gvPassenger2.JSProperties["cp_result"] = msgList.Err200047;
                                                return;
                                            //}
                                            //else
                                            //{
                                            //    dtPass2.Rows[i]["Drink"] = string.Empty;
                                            //    dtPass2.Rows[i]["SSRCodeDrink"] = string.Empty;
                                            //    dtPass2.Rows[i]["PriceDrink"] = 0.00;
                                            //}
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
                                                dtPass2.Rows[i]["PriceDrink"] = Convert.ToDecimal(Detail);
                                                dtPass2.Rows[i]["Drink"] = dtDrink.Rows[0]["ConcatenatedField"];
                                                dtPass2.Rows[i]["SSRCodeDrink"] = dtDrink.Rows[0]["SSRCode"];
                                                //}
                                                break;
                                                //}
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
                        if (Session["dtMealReturn2"] != null)
                        {
                            DataTable dtMeal = (DataTable)Session["dtMealReturn2"];
                            if (cbAllPaxMeal22.Checked == true)
                            {
                                for (int i = 0; i <= dtPass2.Rows.Count - 1; i++)
                                {
                                    if (args[2].ToString() == ",")
                                    {
                                        //if (Request.QueryString["change"] != null)
                                        //{
                                            gvPassenger2.JSProperties["cp_result"] = msgList.Err200048;
                                            return;
                                        //}
                                        //else
                                        //{
                                        //    dtPass2.Rows[i]["Meal1"] = string.Empty;
                                        //    dtPass2.Rows[i]["SSRCodeMeal1"] = string.Empty;
                                        //    dtPass2.Rows[i]["PriceMeal1"] = 0.00;
                                        //}
                                    }
                                    else
                                    {

                                        DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            if (Request.QueryString["change"] != null)
                                            {
                                                if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass2.Rows[i]["PriceMeal1"]))
                                                {
                                                    gvPassenger2.JSProperties["cp_result"] = msgList.Err200049;
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass2.Rows[i]["Meal1"] = args[2];
                                                    dtPass2.Rows[i]["SSRCodeMeal1"] = args[3];
                                                    dtPass2.Rows[i]["PriceMeal1"] = Convert.ToDecimal(Detail);
                                                }
                                            }
                                            else
                                            {
                                                dtPass2.Rows[i]["Meal1"] = args[2];
                                                dtPass2.Rows[i]["SSRCodeMeal1"] = args[3];
                                                dtPass2.Rows[i]["PriceMeal1"] = Convert.ToDecimal(Detail);
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
                                            //if (Request.QueryString["change"] != null)
                                            //{
                                                gvPassenger2.JSProperties["cp_result"] = msgList.Err200048;
                                                return;
                                            //}
                                            //else
                                            //{
                                            //    dtPass2.Rows[i]["Meal1"] = string.Empty;
                                            //    dtPass2.Rows[i]["SSRCodeMeal1"] = string.Empty;
                                            //    dtPass2.Rows[i]["PriceMeal1"] = 0.00;
                                            //}
                                        }
                                        else
                                        {

                                            DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                if (Request.QueryString["change"] != null)
                                                {
                                                    if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass2.Rows[i]["PriceMeal1"]))
                                                    {
                                                        gvPassenger2.JSProperties["cp_result"] = msgList.Err200049;
                                                        return;
                                                    }
                                                    else
                                                    {
                                                        dtPass2.Rows[i]["Meal1"] = args[2];
                                                        dtPass2.Rows[i]["SSRCodeMeal1"] = args[3];
                                                        dtPass2.Rows[i]["PriceMeal1"] = Convert.ToDecimal(Detail);
                                                    }
                                                }
                                                else
                                                {
                                                    dtPass2.Rows[i]["Meal1"] = args[2];
                                                    dtPass2.Rows[i]["SSRCodeMeal1"] = args[3];
                                                    dtPass2.Rows[i]["PriceMeal1"] = Convert.ToDecimal(Detail);
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
                        }
                        if (Session["dtDrinkReturn2"] != null)
                        {
                            DataTable dtDrink = (DataTable)Session["dtDrinkReturn2"];
                            if (cbAllPaxMeal22.Checked == true)
                            {
                                for (int i = 0; i <= dtPass2.Rows.Count - 1; i++)
                                {
                                    if (args[2].ToString() == "")
                                    {
                                        //if (Request.QueryString["change"] != null)
                                        //{
                                            gvPassenger2.JSProperties["cp_result"] = msgList.Err200051;
                                            return;
                                        //}
                                        //else
                                        //{
                                        //    dtPass2.Rows[i]["Drink1"] = string.Empty;
                                        //    dtPass2.Rows[i]["SSRCodeDrink1"] = string.Empty;
                                        //    dtPass2.Rows[i]["PriceDrink1"] = 0.00;
                                        //}
                                    }
                                    else
                                    {
                                        //if (Session["defaultdrink"] != null)
                                        //{
                                        //DataTable DefaultDrink = (DataTable)Session["defaultdrink"];
                                        for (int c = 0; c < dtDrink.Rows.Count; c++)
                                        {
                                            // DataRow[] result = dtDrink.Select("SSRCode LIKE '" + DefaultDrink.Rows[c]["SYSValueEx"] + "%'");
                                            // if (result.Length > 0)
                                            //{
                                            //foreach (DataRow row in result)
                                            //{
                                            Detail = dtDrink.Rows[0]["Price"].ToString().Substring(0, dtDrink.Rows[0]["Price"].ToString().Length - 4);
                                            dtPass2.Rows[i]["PriceDrink1"] = Convert.ToDecimal(Detail);
                                            dtPass2.Rows[i]["Drink1"] = dtDrink.Rows[0]["ConcatenatedField"];
                                            dtPass2.Rows[i]["SSRCodeDrink1"] = dtDrink.Rows[0]["SSRCode"];
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
                                            //dtPass2.Rows[i]["Drink1"] = string.Empty;
                                            //dtPass2.Rows[i]["SSRCodeDrink1"] = string.Empty;
                                            //dtPass2.Rows[i]["PriceDrink1"] = 0.00;
                                            gvPassenger2.JSProperties["cp_result"] = msgList.Err200048;
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
                                                // if (result.Length > 0)
                                                //{
                                                //foreach (DataRow row in result)
                                                //{
                                                Detail = dtDrink.Rows[0]["Price"].ToString().Substring(0, dtDrink.Rows[0]["Price"].ToString().Length - 4);
                                                dtPass2.Rows[i]["PriceDrink1"] = Convert.ToDecimal(Detail);
                                                dtPass2.Rows[i]["Drink1"] = dtDrink.Rows[0]["ConcatenatedField"];
                                                dtPass2.Rows[i]["SSRCodeDrink1"] = dtDrink.Rows[0]["SSRCode"];
                                                //}
                                                break;
                                                //}
                                            }
                                            // }
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
                        if (Session["dtSportReturn"] != null)
                        {
                            DataTable dtSport = (DataTable)Session["dtSportReturn"];
                            if (cbAllPaxSport2.Checked == true)
                            {
                                for (int i = 0; i <= dtPass2.Rows.Count - 1; i++)
                                {
                                    if (args[2].ToString() == "")
                                    {
                                        //if (Request.QueryString["change"] != null)
                                        //{
                                            gvPassenger2.JSProperties["cp_result"] = msgList.Err200052;
                                            return;
                                        //}
                                        //else
                                        //{
                                        //    dtPass2.Rows[i]["Sport"] = string.Empty;
                                        //    dtPass2.Rows[i]["SSRCodeSport"] = string.Empty;
                                        //    dtPass2.Rows[i]["PriceSport"] = 0.00;
                                        //    dtPass2.Rows[i]["PriceSportS1"] = 0.00;
                                        //    dtPass2.Rows[i]["PriceSportS2"] = 0.00;
                                        //}
                                    }
                                    else
                                    {

                                        DataRow[] result = dtSport.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            if (Request.QueryString["change"] != null)
                                            {
                                                if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass2.Rows[i]["PriceSport"]))
                                                {
                                                    gvPassenger2.JSProperties["cp_result"] = msgList.Err200053;
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass2.Rows[i]["Sport"] = args[2];
                                                    dtPass2.Rows[i]["SSRCodeSport"] = args[3];
                                                    dtPass2.Rows[i]["PriceSport"] = Convert.ToDecimal(Detail);
                                                    dtPass2.Rows[i]["PriceSportS1"] = Convert.ToDecimal(row[3]);
                                                    dtPass2.Rows[i]["PriceSportS2"] = Convert.ToDecimal(row[4]);
                                                }
                                            }
                                            else
                                            {
                                                dtPass2.Rows[i]["Sport"] = args[2];
                                                dtPass2.Rows[i]["SSRCodeSport"] = args[3];
                                                dtPass2.Rows[i]["PriceSport"] = Convert.ToDecimal(Detail);
                                                dtPass2.Rows[i]["PriceSportS1"] = Convert.ToDecimal(row[3]);
                                                dtPass2.Rows[i]["PriceSportS2"] = Convert.ToDecimal(row[4]);
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
                                            //if (Request.QueryString["change"] != null)
                                            //{
                                                gvPassenger2.JSProperties["cp_result"] = msgList.Err200052;
                                                return;
                                            //}
                                            //else
                                            //{
                                            //    dtPass2.Rows[i]["Sport"] = string.Empty;
                                            //    dtPass2.Rows[i]["SSRCodeSport"] = string.Empty;
                                            //    dtPass2.Rows[i]["PriceSport"] = 0.00;
                                            //    dtPass2.Rows[i]["PriceSportS1"] = 0.00;
                                            //    dtPass2.Rows[i]["PriceSportS2"] = 0.00;
                                            //}
                                        }
                                        else
                                        {

                                            DataRow[] result = dtSport.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                if (Request.QueryString["change"] != null)
                                                {
                                                    if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass2.Rows[i]["PriceSport"]))
                                                    {
                                                        gvPassenger2.JSProperties["cp_result"] = msgList.Err200053;
                                                        return;
                                                    }
                                                    else
                                                    {
                                                        dtPass2.Rows[i]["Sport"] = args[2];
                                                        dtPass2.Rows[i]["SSRCodeSport"] = args[3];
                                                        dtPass2.Rows[i]["PriceSport"] = Convert.ToDecimal(Detail);
                                                        dtPass2.Rows[i]["PriceSportS1"] = Convert.ToDecimal(row[3]);
                                                        dtPass2.Rows[i]["PriceSportS2"] = Convert.ToDecimal(row[4]);
                                                    }
                                                }
                                                else
                                                {
                                                    dtPass2.Rows[i]["Sport"] = args[2];
                                                    dtPass2.Rows[i]["SSRCodeSport"] = args[3];
                                                    dtPass2.Rows[i]["PriceSport"] = Convert.ToDecimal(Detail);
                                                    dtPass2.Rows[i]["PriceSportS1"] = Convert.ToDecimal(row[3]);
                                                    dtPass2.Rows[i]["PriceSportS2"] = Convert.ToDecimal(row[4]);
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
                    }
                    else if (args[0] == "Duty")
                    {
                        DataTable dtDuty = (DataTable)Session["dtDutyReturn"];
                        if (((Convert.ToInt32(args[1]) + qtyDuty2) - 1) <= dtPass2.Rows.Count)
                        {
                            for (int i = qtyDuty2; i <= Convert.ToInt32(args[1] + qtyDuty2) - 1; i++)
                            {
                                dtPass2.Rows[i]["Duty"] = args[2];
                                dtPass2.Rows[i]["SSRCodeDuty"] = args[3];
                                DataRow[] result = dtDuty.Select("SSRCode = '" + args[3] + "'");
                                foreach (DataRow row in result)
                                {
                                    Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                    dtPass2.Rows[i]["PriceDuty"] = Convert.ToDecimal(Detail);
                                    dtPass2.Rows[i]["PriceDutyS1"] = Convert.ToDecimal(row[3]);
                                    dtPass2.Rows[i]["PriceDutyS2"] = Convert.ToDecimal(row[4]);
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
                                    //if (Request.QueryString["change"] != null)
                                    //{
                                        gvPassenger2.JSProperties["cp_result"] = msgList.Err200056;
                                        return;
                                    //}
                                    //else
                                    //{
                                    //    dtPass2.Rows[i]["Comfort"] = string.Empty;
                                    //    dtPass2.Rows[i]["SSRCodeComfort"] = string.Empty;
                                    //    dtPass2.Rows[i]["PriceComfort"] = 0.00;
                                    //    dtPass2.Rows[i]["PriceComfortS1"] = 0.00;
                                    //    dtPass2.Rows[i]["PriceComfortS2"] = 0.00;
                                    //}
                                }
                                else
                                {

                                    DataRow[] result = dtComfort.Select("SSRCode = '" + args[3] + "'");
                                    foreach (DataRow row in result)
                                    {
                                        Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                        if (Request.QueryString["change"] != null)
                                        {
                                            if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass2.Rows[i]["PriceComfort"]))
                                            {
                                                gvPassenger2.JSProperties["cp_result"] = msgList.Err200057;
                                                return;
                                            }
                                            else
                                            {
                                                dtPass2.Rows[i]["Comfort"] = args[2];
                                                dtPass2.Rows[i]["SSRCodeComfort"] = args[3];
                                                dtPass2.Rows[i]["PriceComfort"] = Convert.ToDecimal(Detail);
                                                dtPass2.Rows[i]["PriceComfortS1"] = Convert.ToDecimal(row[3]);
                                                dtPass2.Rows[i]["PriceComfortS2"] = Convert.ToDecimal(row[4]);
                                            }
                                        }
                                        else
                                        {
                                            dtPass2.Rows[i]["Comfort"] = args[2];
                                            dtPass2.Rows[i]["SSRCodeComfort"] = args[3];
                                            dtPass2.Rows[i]["PriceComfort"] = Convert.ToDecimal(Detail);
                                            dtPass2.Rows[i]["PriceComfortS1"] = Convert.ToDecimal(row[3]);
                                            dtPass2.Rows[i]["PriceComfortS2"] = Convert.ToDecimal(row[4]);
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
                                        //if (Request.QueryString["change"] != null)
                                        //{
                                            gvPassenger2.JSProperties["cp_result"] = msgList.Err200056;
                                            return;
                                        //}
                                        //else
                                        //{
                                        //    dtPass2.Rows[i]["Comfort"] = string.Empty;
                                        //    dtPass2.Rows[i]["SSRCodeComfort"] = string.Empty;
                                        //    dtPass2.Rows[i]["PriceComfort"] = 0.00;
                                        //    dtPass2.Rows[i]["PriceComfortS1"] = 0.00;
                                        //    dtPass2.Rows[i]["PriceComfortS2"] = 0.00;
                                        //}
                                    }
                                    else
                                    {

                                        DataRow[] result = dtComfort.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            if (Request.QueryString["change"] != null)
                                            {
                                                if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass2.Rows[i]["PriceComfort"]))
                                                {
                                                    gvPassenger2.JSProperties["cp_result"] = msgList.Err200057;
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass2.Rows[i]["Comfort"] = args[2];
                                                    dtPass2.Rows[i]["SSRCodeComfort"] = args[3];
                                                    dtPass2.Rows[i]["PriceComfort"] = Convert.ToDecimal(Detail);
                                                    dtPass2.Rows[i]["PriceComfortS1"] = Convert.ToDecimal(row[3]);
                                                    dtPass2.Rows[i]["PriceComfortS2"] = Convert.ToDecimal(row[4]);
                                                }
                                            }
                                            else
                                            {
                                                gvPassenger2.JSProperties["cp_result"] = msgList.Err200056;
                                                return;
                                                //dtPass2.Rows[i]["Comfort"] = args[2];
                                                //dtPass2.Rows[i]["SSRCodeComfort"] = args[3];
                                                //dtPass2.Rows[i]["PriceComfort"] = Convert.ToDecimal(Detail);
                                                //dtPass2.Rows[i]["PriceComfortS1"] = Convert.ToDecimal(row[3]);
                                                //dtPass2.Rows[i]["PriceComfortS2"] = Convert.ToDecimal(row[4]);
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
                                gvPassenger2.JSProperties["cp_result"] = msgList.Err200058;
                                return;
                            }
                        }
                    }
                    else if (args[0] == "Clear")
                    {
                        if (first == 1)
                        {
                            DataTable dtdefault = (DataTable)Session["dtdefaultBundle"];
                            if (dtdefault != null && dtdefault.Rows.Count > 0)
                            {
                                DataTable dtBaggage = (DataTable)Session["dtBaggageDepart"];

                                DataRow[] result = dtBaggage.Select("SSRCode = '" + dtdefault.Rows[0]["SYSValue"] + "'");
                                foreach (DataRow row in result)
                                {
                                    dtPass2.Rows[Convert.ToInt16(args[1])]["Baggage"] = row[5];
                                    dtPass2.Rows[Convert.ToInt16(args[1])]["SSRCodeBaggage"] = row[0];
                                    Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                    dtPass2.Rows[Convert.ToInt16(args[1])]["PriceBaggageS1"] = Convert.ToDecimal(row[3]);
                                    dtPass2.Rows[Convert.ToInt16(args[1])]["PriceBaggageS2"] = Convert.ToDecimal(row[4]);
                                }
                                dtPass2.Rows[Convert.ToInt16(args[1])]["PriceBaggage"] = Convert.ToDecimal(Detail);
                            }
                        }
                        else
                        {
                            dtPass2.Rows[Convert.ToInt16(args[1])]["Baggage"] = string.Empty;
                            dtPass2.Rows[Convert.ToInt16(args[1])]["SSRCodeBaggage"] = string.Empty;
                            dtPass2.Rows[Convert.ToInt16(args[1])]["PriceBaggage"] = 0.00;
                            Session["qtyBaggage2"]=Convert.ToInt32(Session["qtyBaggage2"]) - 1;
                            //qtyBaggage2 = qtyBaggage2 - 1;
                        }
                        dtPass2.Rows[Convert.ToInt16(args[1])]["Meal"] = string.Empty;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["SSRCodeMeal"] = string.Empty;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["PriceMeal"] = 0.00;

                        dtPass2.Rows[Convert.ToInt16(args[1])]["Meal1"] = string.Empty;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["SSRCodeMeal1"] = string.Empty;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["PriceMeal1"] = 0.00;

                        dtPass2.Rows[Convert.ToInt16(args[1])]["Drink"] = string.Empty;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["SSRCodeDrink"] = string.Empty;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["PriceDrink"] = 0.00;

                        dtPass2.Rows[Convert.ToInt16(args[1])]["Drink1"] = string.Empty;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["SSRCodeDrink1"] = string.Empty;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["PriceDrink1"] = 0.00;

                        dtPass2.Rows[Convert.ToInt16(args[1])]["Sport"] = string.Empty;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["SSRCodeSport"] = string.Empty;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["PriceSport"] = 0.00;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["PriceSportS1"] = 0.00;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["PriceSportS2"] = 0.00;

                        dtPass2.Rows[Convert.ToInt16(args[1])]["Duty"] = string.Empty;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["SSRCodeDuty"] = string.Empty;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["PriceDuty"] = 0.00;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["PriceDutyS1"] = 0.00;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["PriceDutyS2"] = 0.00;

                        dtPass2.Rows[Convert.ToInt16(args[1])]["Comfort"] = string.Empty;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["SSRCodeComfort"] = string.Empty;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["PriceComfort"] = 0.00;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["PriceComfortS1"] = 0.00;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["PriceComfortS2"] = 0.00;

                        dtPass2.Rows[Convert.ToInt16(args[1])]["Infant"] = string.Empty;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["SSRCodeInfant"] = string.Empty;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["PriceInfant"] = 0.00;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["PriceInfantS1"] = 0.00;
                        dtPass2.Rows[Convert.ToInt16(args[1])]["PriceInfantS2"] = 0.00;

                        if (dtPass != null && dtPass.Rows[Convert.ToInt16(args[1])]["Infant"].ToString() != string.Empty)
                        {
                            dtPass.Rows[Convert.ToInt16(args[1])]["Infant"] = string.Empty;
                            dtPass.Rows[Convert.ToInt16(args[1])]["SSRCodeInfant"] = string.Empty;
                            dtPass.Rows[Convert.ToInt16(args[1])]["PriceInfant"] = 0.00;
                            dtPass.Rows[Convert.ToInt16(args[1])]["PriceInfantS1"] = 0.00;
                            dtPass.Rows[Convert.ToInt16(args[1])]["PriceInfantS2"] = 0.00;
                            gvPassenger2.JSProperties["cp_result"] = "Infant";
                            Session["dtGridPass"] = dtPass;
                        }

                        Session["dtGridPass2"] = dtPass2;
                        gvPassenger2.DataSource = dtPass2;
                        gvPassenger2.DataBind();

                        Session["qtyMeal2"] = Convert.ToInt32(Session["qtyMeal2"]) - 1;
                        Session["qtyMeal21"] = Convert.ToInt32(Session["qtyMeal21"]) - 1;
                        Session["qtySport2"] = Convert.ToInt32(Session["qtySport2"]) - 1;
                        Session["qtyComfort2"] = Convert.ToInt32(Session["qtyComfort2"]) - 1;
                        Session["qtyDrink2"] = Convert.ToInt32(Session["qtyDrink2"]) - 1;
                        Session["qtyDrink21"] = Convert.ToInt32(Session["qtyDrink21"]) - 1;
                        //qtyMeal2 = qtyMeal2 - 1;
                        //qtyMeal21 = qtyMeal21 - 1;
                        //qtySport2 = qtySport2 - 1;
                        //qtyComfort2 = qtyComfort2 - 1;
                        //qtyDrink2 = qtyDrink2 - 1;
                        //qtyDrink21 = qtyDrink21 - 1;
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
        #endregion

        #region "Function and Procedure"
        protected string GetSellSSR(string signature)
        {
            var profiler = MiniProfiler.Current;
            HttpCookie cookie = Request.Cookies["cookieSearchcondition"];
            ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking(signature);
            BookingControl bookingControl = new BookingControl();

            bool IsDepart = false, IsDepartTransit = false, IsDepartTransit2 = false, IsReturn = false, IsReturnTransit = false, IsReturnTransit2 = false;
            ABS.Navitaire.BookingManager.Booking responseBookingFromState = bookingControl.GetBookingFromState(signature);
            //string xml = GetXMLString(responseBookingFromState);
            if (responseBookingFromState == null || responseBookingFromState.Journeys.Length <= 0)
            {
                Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.InvalidPage);
                return "";
            }

            GetSSRAvailabilityForBookingResponse response = new GetSSRAvailabilityForBookingResponse();
            if (Session["GetssrAvailabilityResponseForBooking"] != null)
            {
                response = (GetSSRAvailabilityForBookingResponse)Session["GetssrAvailabilityResponseForBooking"];
            }
            else
            {
                using (profiler.Step("Navitaire:GetSSRAvailabilityForBooking"))
                {
                    response = apiBooking.GetSSRAvailabilityForBooking(responseBookingFromState, signature);
                }
            }
            //string xmlresponse = GetXMLString(response);
            if (response != null)
            {
                Session["GetssrAvailabilityResponseForBooking"] = response;
                ArrayList BaggageCode = new ArrayList();
                ArrayList BaggageFee = new ArrayList();
                ArrayList BaggageFeeS1 = new ArrayList();
                ArrayList BaggageFeeS2 = new ArrayList();

                ArrayList SportCode = new ArrayList();
                ArrayList SportFee = new ArrayList();
                ArrayList SportFeeS1 = new ArrayList();
                ArrayList SportFeeS2 = new ArrayList();

                ArrayList InfantCode = new ArrayList();
                ArrayList InfantFee = new ArrayList();
                ArrayList InfantFeeS1 = new ArrayList();
                ArrayList InfantFeeS2 = new ArrayList();

                ArrayList ComfortCode = new ArrayList();
                ArrayList ComfortFee = new ArrayList();
                ArrayList ComfortFeeS1 = new ArrayList();
                ArrayList ComfortFeeS2 = new ArrayList();
                ArrayList ComfortImage = new ArrayList();

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

                ArrayList SportCode2 = new ArrayList();
                ArrayList SportFee2 = new ArrayList();
                ArrayList SportFee2S1 = new ArrayList();
                ArrayList SportFee2S2 = new ArrayList();

                ArrayList InfantCode2 = new ArrayList();
                ArrayList InfantFee2 = new ArrayList();
                ArrayList InfantFee2S1 = new ArrayList();
                ArrayList InfantFee2S2 = new ArrayList();

                ArrayList ComfortCode2 = new ArrayList();
                ArrayList ComfortFee2 = new ArrayList();
                ArrayList ComfortFee2S1 = new ArrayList();
                ArrayList ComfortFee2S2 = new ArrayList();
                ArrayList ComfortImage2 = new ArrayList();
                //Decimal ComfortAmt2 = 0;

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

                if (model == null == false)
                {
                    depart1 = model.TemFlightDeparture.Trim();
                    Session["depart1"] = model.TemFlightDeparture.Trim();
                    transit1 = model.TemFlightTransit.Trim();
                    Session["transit1"] = model.TemFlightTransit.Trim();
                    return1 = model.TemFlightArrival.Trim();
                    Session["return1"] = model.TemFlightArrival.Trim();

                }

                if (model2 == null == false)
                {
                    depart2 = model2.TemFlightDeparture.Trim();
                    Session["depart2"] = model2.TemFlightDeparture.Trim();
                    transit2 = model2.TemFlightTransit.Trim();
                    Session["transit2"] = model2.TemFlightTransit.Trim();
                    return2 = model2.TemFlightArrival.Trim();
                    Session["return2"] = model2.TemFlightArrival.Trim();
                }

                String Currency = "";
                SSRAvailabilityForBookingResponse ssrAvailabilityResponseForBooking = response.SSRAvailabilityForBookingResponse;
                //Session["GetssrAvailabilityResponseForBooking"] = response;
                DataTable dtdefaultBaggage = new DataTable();
                DataTable dtdefaultBundleSport = new DataTable();
                DataTable dtdefaultBundleKit = new DataTable();
                DataTable dtdefaultInfant = new DataTable();
                //xml = GetXMLString(ssrAvailabilityResponseForBooking);

                if (ssrAvailabilityResponseForBooking != null && ssrAvailabilityResponseForBooking.SSRSegmentList.Length != 0)
                {
                    if (cookie != null)
                    {
                        dtdefaultBaggage = (DataTable)Application["dtArrayBaggage"];
                        dtdefaultBundleSport = (DataTable)Application["dtArraySport"];
                        dtdefaultBundleKit = (DataTable)Application["dtArrayKit"];
                        dtdefaultInfant = (DataTable)Application["dtArrayInfant"];

                        
                        DataRow[] dr = dtdefaultBaggage.Select("");

                        List<string> lstBaggage = dtdefaultBaggage.AsEnumerable().Select(r => r.Field<string>("ItemCode")).ToList();
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
                                        BaggageFeeS1.AddRange(paxBaggage.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
                                    }
                                    else if ((SSRSegment.LegKey.DepartureStation == transit1 && SSRSegment.LegKey.ArrivalStation == return1))
                                    {
                                        BaggageFeeS2.AddRange(paxBaggage.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
                                    }


                                }

                                //Meals
                                    List<AvailablePaxSSR> paxMealsS1 = SSRSegment.AvailablePaxSSRList.Where(x => x.SSRLegList.Length > 0 && x.Available >= Convert.ToInt16(cookie.Values["PaxNum"]) && ((SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == return1) || (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == transit1))).ToList();
                                    List<AvailablePaxSSR> paxMealsS2 = SSRSegment.AvailablePaxSSRList.Where(x => x.SSRLegList.Length > 0 && x.Available >= Convert.ToInt16(cookie.Values["PaxNum"]) && (SSRSegment.LegKey.DepartureStation == transit1 && SSRSegment.LegKey.ArrivalStation == return1)).ToList();
                                    if (paxMealsS1 != null && paxMealsS1.Count > 0)
                                    {
                                        MealFee.AddRange(paxMealsS1.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
                                        MealCode.AddRange(paxMealsS1.Select(x => x.SSRCode).ToList());
                                        MealImage.AddRange(paxMealsS1.Select(x => "http://www.airasia.com/images/common/sky-ssr/img_" + x.SSRCode + ".png").ToList());
                                    }
                                    else if (paxMealsS2 != null && paxMealsS2.Count > 0)
                                    {
                                        MealFee1.AddRange(paxMealsS2.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
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
                                        SportFeeS1.AddRange(paxSport.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
                                    }
                                    else if ((SSRSegment.LegKey.DepartureStation == transit1 && SSRSegment.LegKey.ArrivalStation == return1))
                                    {
                                        SportFeeS2.AddRange(paxSport.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
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
                                        ComfortFeeS1.AddRange(paxKit.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
                                    }
                                    else if ((SSRSegment.LegKey.DepartureStation == transit1 && SSRSegment.LegKey.ArrivalStation == return1))
                                    {
                                        ComfortFeeS2.AddRange(paxKit.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
                                    }
                                }

                                 //Infant
                                List<AvailablePaxSSR> paxInfant = SSRSegment.AvailablePaxSSRList.Where(x => lstInfant.Contains(x.SSRCode)).ToList();
                                if (paxInfant != null && paxInfant.Count > 0)
                                {
                                    //paxInfant.FirstOrDefault().Available; // to retrieve available of this pax infant
                                    infantmax = SSRSegment.AvailablePaxSSRList.Where(x => lstInfant.Contains(x.SSRCode)).FirstOrDefault().Available;
                                    //(i => i.Available).Where(x => lstInfant.Contains(x.SSRCode));
                                    int index = paxInfant.Where(x => InfantCode.Contains(x.SSRCode)).Count();
                                    if (index <= 0)
                                    {
                                        InfantCode.AddRange(paxInfant.Select(x => x.SSRCode).ToList());
                                        Session["InfantCode"] = InfantCode;
                                        //paxInfant.Select(x => x.Available).ToList(); // to retrieve available as well
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
                                        BaggageFee2S1.AddRange(paxBaggage.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
                                    }
                                    else if ((SSRSegment.LegKey.DepartureStation == transit2 && SSRSegment.LegKey.ArrivalStation == return2))
                                    {
                                        BaggageFee2S2.AddRange(paxBaggage.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
                                    }
                                }

                                //Meals
                                List<AvailablePaxSSR> paxMealsS1 = SSRSegment.AvailablePaxSSRList.Where(x => x.SSRLegList.Length > 0 && x.Available >= Convert.ToInt16(cookie.Values["PaxNum"]) && ((SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == return2) || (SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == transit2))).ToList();
                                List<AvailablePaxSSR> paxMealsS2 = SSRSegment.AvailablePaxSSRList.Where(x => x.SSRLegList.Length > 0 && x.Available >= Convert.ToInt16(cookie.Values["PaxNum"]) && (SSRSegment.LegKey.DepartureStation == transit2 && SSRSegment.LegKey.ArrivalStation == return2)).ToList();
                                if (paxMealsS1 != null && paxMealsS1.Count > 0)
                                {
                                    MealFee2.AddRange(paxMealsS1.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
                                    MealCode2.AddRange(paxMealsS1.Select(x => x.SSRCode).ToList());
                                    MealImage2.AddRange(paxMealsS1.Select(x => "http://www.airasia.com/images/common/sky-ssr/img_" + x.SSRCode + ".png").ToList());
                                }
                                else if (paxMealsS2 != null && paxMealsS2.Count > 0)
                                {
                                    MealFee21.AddRange(paxMealsS2.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
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
                                        SportFee2S1.AddRange(paxSport.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
                                    }
                                    else if ((SSRSegment.LegKey.DepartureStation == transit2 && SSRSegment.LegKey.ArrivalStation == return2))
                                    {
                                        SportFee2S2.AddRange(paxSport.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
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
                                        ComfortFee2S1.AddRange(paxKit.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
                                    }
                                    else if ((SSRSegment.LegKey.DepartureStation == transit2 && SSRSegment.LegKey.ArrivalStation == return2))
                                    {
                                        ComfortFee2S2.AddRange(paxKit.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
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
                                        Session["InfantCode"] = InfantCode2;
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
                               
                        }

                    }

                    //Baggage
                    if (BaggageFeeS2.Count > 0 )
                    {
                        BaggageFee.AddRange(BaggageFeeS1.ToArray().Zip(BaggageFeeS2.ToArray(), (x, y) => Convert.ToDecimal(x) + Convert.ToDecimal(y)).ToList());
                    }
                    else
                    {
                        BaggageFee = BaggageFeeS1;
                    }
                    if (BaggageFee2S2.Count > 0)
                    {
                        BaggageFee2.AddRange(BaggageFee2S1.ToArray().Zip(BaggageFee2S2.ToArray(), (x, y) => Convert.ToDecimal(x) + Convert.ToDecimal(y)).ToList());
                    }
                    else
                    {
                        BaggageFee2 = BaggageFee2S1;
                    }

                    //Sport
                    if (SportFeeS2.Count > 0)
                    {
                        SportFee.AddRange(SportFeeS1.ToArray().Zip(SportFeeS2.ToArray(), (x, y) => Convert.ToDecimal(x) + Convert.ToDecimal(y)).ToList());
                    }
                    else
                    {
                        SportFee = SportFeeS1;
                    }
                    if (SportFee2S2.Count > 0)
                    {
                        SportFee2.AddRange(SportFee2S1.ToArray().Zip(SportFee2S2.ToArray(), (x, y) => Convert.ToDecimal(x) + Convert.ToDecimal(y)).ToList());
                    }
                    else
                    {
                        SportFee2 = SportFee2S1;
                    }

                    //Comfort
                    if (ComfortFeeS2.Count > 0)
                    {
                        ComfortFee.AddRange(ComfortFeeS1.ToArray().Zip(ComfortFeeS2.ToArray(), (x, y) => Convert.ToDecimal(x) + Convert.ToDecimal(y)).ToList());
                    }
                    else
                    {
                        ComfortFee = ComfortFeeS1;
                    }
                    if (ComfortFee2S2.Count > 0)
                    {
                        ComfortFee2.AddRange(ComfortFee2S1.ToArray().Zip(ComfortFee2S2.ToArray(), (x, y) => Convert.ToDecimal(x) + Convert.ToDecimal(y)).ToList());
                    }
                    else
                    {
                        ComfortFee2 = ComfortFee2S1;
                    }

                    //Infant
                    if (InfantFeeS2.Count > 0)
                    {
                        InfantFee.AddRange(InfantFeeS1.ToArray().Zip(InfantFeeS2.ToArray(), (x, y) => Convert.ToDecimal(x) + Convert.ToDecimal(y)).ToList());
                    }
                    else
                    {
                        InfantFee = InfantFeeS1;
                    }
                    if (InfantFee2S2.Count > 0)
                    {
                        InfantFee2.AddRange(InfantFee2S1.ToArray().Zip(InfantFee2S2.ToArray(), (x, y) => Convert.ToDecimal(x) + Convert.ToDecimal(y)).ToList());
                    }
                    else
                    {
                        InfantFee2 = InfantFee2S1;
                    }
                    

                    

                    

                    

                    if (Session["GetssrAvailabilityResponseForBookingDrink"] != null)
                    {
                        response = (GetSSRAvailabilityForBookingResponse)Session["GetssrAvailabilityResponseForBookingDrink"];
                    }
                    else
                    {
                        using (profiler.Step("Navitaire:GetSSRAvailabilityForBooking"))
                        {
                            response = apiBooking.GetSSRAvailabilityForBooking(responseBookingFromState, signature);
                        }
                    }
                    //string xmlresponses = GetXMLString(responses);
                    if (response != null)
                    {
                        SSRAvailabilityForBookingResponse ssrAvailabilityResponseForBookings = response.SSRAvailabilityForBookingResponse;
                        Session["GetssrAvailabilityResponseForBookingDrink"] = response;
                        //xml = GetXMLString(ssrAvailabilityResponseForBookings);

                        if (ssrAvailabilityResponseForBookings != null && ssrAvailabilityResponseForBookings.SSRSegmentList.Length != 0)
                        {
                            //this entire section is simply to test the computation of SSR pricing for Meal fee in particular as segment based or leg based depending on the results
                            if (cookie != null)
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
                                                DrinkFee.AddRange(paxDrink.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
                                            }
                                            else if ((SSRSegment.LegKey.DepartureStation == transit1 && SSRSegment.LegKey.ArrivalStation == return1))
                                            {
                                                DrinkCode1.AddRange(paxDrink.Select(x => x.SSRCode).ToList());
                                                DrinkFee1.AddRange(paxDrink.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
                                            }
                                        }
                                        else if ((SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == return2) || (SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == transit2) || (SSRSegment.LegKey.DepartureStation == transit2 && SSRSegment.LegKey.ArrivalStation == return2))
                                        {
                                            //return here 
                                            if ((SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == return2) || (SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == transit2))
                                            {
                                                DrinkCode2.AddRange(paxDrink.Select(x => x.SSRCode).ToList());
                                                DrinkFee2.AddRange(paxDrink.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
                                            }
                                            else if ((SSRSegment.LegKey.DepartureStation == transit2 && SSRSegment.LegKey.ArrivalStation == return2))
                                            {
                                                DrinkCode21.AddRange(paxDrink.Select(x => x.SSRCode).ToList());
                                                DrinkFee21.AddRange(paxDrink.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
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

                           
                        }

                        if (cookie != null)
                        {
                            //
                            Currency = cookie.Values["Currency"];
                            Session["Currency"] = Currency;

                            foreach (ABS.Navitaire.BookingManager.SSRSegment SSRSegment in ssrAvailabilityResponseForBooking.SSRSegmentList)
                            {
                                if (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == return1)
                                {
                                    IsDepart = true;
                                    InitializeForm(Currency, BaggageCode, BaggageFee, BaggageFeeS1, BaggageFeeS2, SportCode, SportFee, SportFeeS1, SportFeeS2, ComfortCode, ComfortFee, ComfortFeeS1, ComfortFeeS2, ComfortImage, MealCode, MealFee, MealImage, null, null, null, DutyCode, DutyFee, DutyImage, DrinkCode, DrinkFee, null, null, InfantCode, InfantFee, InfantFeeS1, InfantFeeS2, "Depart");
                                }
                                else if (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == transit1)
                                {
                                    IsDepartTransit = true;
                                    InitializeForm(Currency, BaggageCode, BaggageFee, BaggageFeeS1, BaggageFeeS2, SportCode, SportFee, SportFeeS1, SportFeeS2, ComfortCode, ComfortFee, ComfortFeeS1, ComfortFeeS2, ComfortImage, MealCode, MealFee, MealImage, MealCode1, MealFee1, MealImage1, DutyCode, DutyFee, DutyImage, DrinkCode, DrinkFee, DrinkCode1, DrinkFee1, InfantCode, InfantFee, InfantFeeS1, InfantFeeS2, "Depart");

                                }
                                else if (SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == return2)
                                {
                                    IsReturn = true;
                                    InitializeForm(Currency, BaggageCode2, BaggageFee2, BaggageFee2S1, BaggageFee2S2, SportCode2, SportFee2, SportFee2S1, SportFee2S2, ComfortCode2, ComfortFee2, ComfortFee2S1, ComfortFee2S2, ComfortImage2, MealCode2, MealFee2, MealImage2, null, null, null, DutyCode2, DutyFee2, DutyImage2, DrinkCode2, DrinkFee2, null, null, InfantCode2, InfantFee2, InfantFee2S1, InfantFee2S2, "Return");
                                }
                                else if (SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == transit2)
                                {
                                    IsReturnTransit = true;
                                    InitializeForm(Currency, BaggageCode2, BaggageFee2, BaggageFee2S1, BaggageFee2S2, SportCode2, SportFee2, SportFee2S1, SportFee2S2, ComfortCode2, ComfortFee2, ComfortFee2S1, ComfortFee2S2, ComfortImage2, MealCode2, MealFee2, MealImage2, MealCode21, MealFee21, MealImage21, DutyCode2, DutyFee2, DutyImage2, DrinkCode2, DrinkFee2, DrinkCode21, DrinkFee21, InfantCode2, InfantFee2, InfantFee2S1, InfantFee2S2, "Return");
                                }

                            }

                        }

                    }
                    else
                    {
                        Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.InvalidPage, false);
                    }


                }
            }
            return "";
        }

        protected void GetPassengerList(string TransID, string Flight)
        {
            try
            {
                HttpCookie cookie = Request.Cookies["cookieTemFlight"];
                DataTable dtPass = new DataTable();
                if (Request.QueryString["change"] != null)
                {
                    dtPass = objBooking.GetAllBK_PASSENGERLISTSSRDataTableChangeNew(TransID, Flight);
                    if (dtPass == null)
                    {
                        if (Session["Chgsave"] != null)
                        {
                            ArrayList save = (ArrayList)Session["Chgsave"];
                            DataTable dtTransDetail = objBooking.dtTransDetail();
                            if (HttpContext.Current.Session["TransDetailAll"] != null)
                                dtTransDetail = (DataTable)HttpContext.Current.Session["TransDetailAll"];
                            DataTable dtPassClone = new DataTable();

                            dtPassClone = objBooking.GetAllBK_PASSENGERLISTSSRDataTableChangeNew(save[1].ToString(), Flight);
                            dtPass = dtPassClone.Clone();
                            int x = 0;
                            for (int i = 0; i < dtPassClone.Rows.Count; i++)
                            {
                                if (dtPassClone.Rows[i]["PNR"].ToString() == dtTransDetail.Rows[0]["RecordLocator"].ToString())
                                {

                                    dtPass.ImportRow(dtPassClone.Rows[i]);
                                    dtPass.Rows[x]["TransID"] = save[0].ToString();
                                    dtPass.Rows[x]["SeqNo"] = x + 1;
                                    x += 1;
                                }
                            }
                        }
                    }
                }
                else
                {
                    dtPass = objBooking.GetAllBK_PASSENGERLISTSSRDataTable(TransID);
                }

                if (dtPass != null && dtPass.Rows.Count > 0)
                {
                    if (Request.QueryString["change"] == null)
                    {
                        int index = 0;
                        for (int i = 0; i < dtPass.Rows.Count; i++)
                        {
                            if ((i == 0) || (i != 0 && Convert.ToInt16(dtPass.Rows[i]["PNR"].ToString()) > Convert.ToInt16(dtPass.Rows[i - 1]["PNR"].ToString())))
                            {
                                index += 1;
                            }

                            dtPass.Rows[i]["PNRs"] = "(" + index + ")";
                            dtPass.Rows[i]["SeqNo"] = i + 1;
                        }
                    }
                    if (Flight == "Depart")
                    {
                        gvPassenger.DataSource = dtPass;
                        gvPassenger.DataBind();
                        Session["dtGridPass"] = dtPass;
                        if (Request.QueryString["change"] != null)
                        {
                            gvPassenger.Columns["Action"].Visible = false;
                        }
                    }
                    else
                    {
                        gvPassenger2.DataSource = dtPass;
                        gvPassenger2.DataBind();
                        Session["dtGridPass2"] = dtPass;
                        if (Request.QueryString["change"] != null)
                        {
                            gvPassenger2.Columns["Action"].Visible = false;
                        }
                    }
                    MaxPax.Value = dtPass.Rows.Count.ToString();
                }
                else
                {
                    Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.InvalidPage,false);
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }



        //protected void GetPassengerList2(string TransID)
        //{
        //    try
        //    {
        //        HttpCookie cookie = Request.Cookies["cookieTemFlight"];
        //        DataTable dtPass2 = new DataTable();
        //        if (Session["dtGridPass2"] != null)
        //        {
        //            dtPass2 = (DataTable)Session["dtGridPass2"];
        //        }
        //        else
        //        {
        //            if (Request.QueryString["change"] != null)
        //            {
        //                dtPass2 = objBooking.GetAllBK_PASSENGERLISTSSRDataTableChangereturn(TransID, "Return");
        //                if (dtPass2 == null)
        //                {
        //                    if (Session["Chgsave"] != null)
        //                    {
        //                        ArrayList save = (ArrayList)Session["Chgsave"];
        //                        DataTable dtTransDetail = objBooking.dtTransDetail();
        //                        if (HttpContext.Current.Session["TransDetailAll"] != null)
        //                            dtTransDetail = (DataTable)HttpContext.Current.Session["TransDetailAll"];
        //                        DataTable dtPassClone = new DataTable();
        //                        dtPassClone = objBooking.GetAllBK_PASSENGERLISTSSRDataTableChangereturn(save[1].ToString(), "Return");
        //                        dtPass2 = dtPassClone.Clone();
        //                        int x = 0;
        //                        for (int i = 0; i < dtPassClone.Rows.Count; i++)
        //                        {
        //                            if (dtPassClone.Rows[i]["PNR"].ToString() == dtTransDetail.Rows[0]["RecordLocator"].ToString())
        //                            {
        //                                dtPass2.ImportRow(dtPassClone.Rows[i]);
        //                                dtPass2.Rows[x]["TransID"] = save[0].ToString();
        //                                dtPass2.Rows[x]["SeqNo"] = x + 1;
        //                                x += 1;
        //                            }
        //                        }

        //                    }
        //                }
        //            }
        //            else
        //            {
        //                dtPass2 = objBooking.GetAllBK_PASSENGERLISTSSRDataTable(TransID);
        //            }
        //        }
        //        if (dtPass2 != null && dtPass2.Rows.Count > 0)
        //        {

        //            gvPassenger2.DataSource = dtPass2;
        //            gvPassenger2.DataBind();

        //            Session["dtGridPass2"] = dtPass2;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //sTraceLog(ex.ToString);
        //        log.Error(this, ex);
        //    }
        //}

        protected void gvPassenger_DataBinding(object sender, EventArgs e)
        {
            DataTable dtPass = new DataTable();
            if (Session["dtGridPass"] != null)
            {
                dtPass = (DataTable)Session["dtGridPass"];
            }
            else
            {
                dtPass = objBooking.GetAllBK_PASSENGERLISTSSRDataTable(Session["TransID"].ToString());
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
                dtPass2 = objBooking.GetAllBK_PASSENGERLISTSSRDataTable(Session["TransID"].ToString());
            }
            (sender as ASPxGridView).DataSource = dtPass2;
            HttpContext.Current.Session["dtGridPass2"] = dtPass2;
        }

        protected void ValidatePopup(object sender, CallbackEventArgs e)
        {
            List<BookingTransactionDetail> lstTransDetail = new List<BookingTransactionDetail>();

            depart1 = Session["depart1"].ToString();
            transit1 = Session["transit1"].ToString();
            return1 = Session["return1"].ToString();

            if (model2 == null == false)
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

            MessageList msgList = new MessageList();
            ArrayList aMsgList = new ArrayList();
            GetSSRAvailabilityForBookingResponse ssrResponse = (GetSSRAvailabilityForBookingResponse)Session["GetssrAvailabilityResponseForBookingDrink"];
            //added by romy for profiler
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
                    DataTable dataClass = new DataTable();
                    dataClass = (DataTable)HttpContext.Current.Session["dtGridPass"];
                    foreach (DataRow dr in dataClass.Rows)
                    {
                        if (dr["SSRCodeBaggage"].ToString() != "" || dr["SSRCodeMeal"].ToString() != "" || dr["SSRCodeDrink"].ToString() != "" || dr["SSRCodeSport"].ToString() != "" || dr["SSRCodeComfort"].ToString() != "" || dr["SSRCodeDuty"].ToString() != "" || dr["SSRCodeMeal1"].ToString() != "" || dr["SSRCodeDrink1"].ToString() != "" || dr["SSRCodeInfant"].ToString() != "")
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
                            BK_TRANSADDONInfo.BaggageCode = dr["SSRCodeBaggage"].ToString();
                            BK_TRANSADDONInfo.BaggageAmt = (decimal)dr["PriceBaggageS1"];
                            BK_TRANSADDONInfo.MealCode1 = dr["SSRCodeMeal"].ToString();
                            BK_TRANSADDONInfo.MealRate1 = (decimal)dr["PriceMeal"];
                            BK_TRANSADDONInfo.MealQty1 = 1;
                            BK_TRANSADDONInfo.MealSubTotal1 = (BK_TRANSADDONInfo.MealRate1 * BK_TRANSADDONInfo.MealQty1);
                            BK_TRANSADDONInfo.MealTotalAmt = (BK_TRANSADDONInfo.MealSubTotal1);
                            BK_TRANSADDONInfo.DrinkCode1 = dr["SSRCodeDrink"].ToString();
                            BK_TRANSADDONInfo.DrinkRate1 = (decimal)dr["PriceDrink"];
                            BK_TRANSADDONInfo.DrinkQty1 = 1;
                            BK_TRANSADDONInfo.DrinkSubTotal1 = (BK_TRANSADDONInfo.DrinkRate1 * BK_TRANSADDONInfo.DrinkQty1);
                            BK_TRANSADDONInfo.DrinkTotalAmt = (BK_TRANSADDONInfo.DrinkSubTotal1);
                            BK_TRANSADDONInfo.SportCode = dr["SSRCodeSport"].ToString();
                            BK_TRANSADDONInfo.SportAmt = (decimal)dr["PriceSportS1"];
                            BK_TRANSADDONInfo.KitCode = dr["SSRCodeComfort"].ToString();
                            BK_TRANSADDONInfo.KitAmt = (decimal)dr["PriceComfortS1"];
                            BK_TRANSADDONInfo.DutyCode = dr["SSRCodeDuty"].ToString();
                            BK_TRANSADDONInfo.DutyAmt = (decimal)dr["PriceDutyS1"];
                            BK_TRANSADDONInfo.InfantCode = dr["SSRCodeInfant"].ToString();
                            BK_TRANSADDONInfo.InfantAmt = (decimal)dr["PriceInfantS1"];
                            BK_TRANSADDONInfo.TotalAmount = (BK_TRANSADDONInfo.BaggageAmt + BK_TRANSADDONInfo.MealSubTotal1 + BK_TRANSADDONInfo.DrinkSubTotal1 + BK_TRANSADDONInfo.SportAmt + BK_TRANSADDONInfo.KitAmt + BK_TRANSADDONInfo.DutyAmt + BK_TRANSADDONInfo.InfantAmt);
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
                                BK_TRANSADDONInfo.BaggageCode = dr["SSRCodeBaggage"].ToString();
                                BK_TRANSADDONInfo.BaggageAmt = (decimal)dr["PriceBaggageS2"];
                                BK_TRANSADDONInfo.MealCode1 = dr["SSRCodeMeal1"].ToString();
                                BK_TRANSADDONInfo.MealRate1 = (decimal)dr["PriceMeal1"];
                                BK_TRANSADDONInfo.MealQty1 = 1;
                                BK_TRANSADDONInfo.MealSubTotal1 = (BK_TRANSADDONInfo.MealRate1 * BK_TRANSADDONInfo.MealQty1);
                                BK_TRANSADDONInfo.MealTotalAmt = (BK_TRANSADDONInfo.MealSubTotal1);
                                BK_TRANSADDONInfo.DrinkCode1 = dr["SSRCodeDrink1"].ToString();
                                BK_TRANSADDONInfo.DrinkRate1 = (decimal)dr["PriceDrink1"];
                                BK_TRANSADDONInfo.DrinkQty1 = 1;
                                BK_TRANSADDONInfo.DrinkSubTotal1 = (BK_TRANSADDONInfo.DrinkRate1 * BK_TRANSADDONInfo.DrinkQty1);
                                BK_TRANSADDONInfo.DrinkTotalAmt = (BK_TRANSADDONInfo.DrinkSubTotal1);
                                BK_TRANSADDONInfo.SportCode = dr["SSRCodeSport"].ToString();
                                BK_TRANSADDONInfo.SportAmt = (decimal)dr["PriceSportS2"];
                                BK_TRANSADDONInfo.KitCode = dr["SSRCodeComfort"].ToString();
                                BK_TRANSADDONInfo.KitAmt = (decimal)dr["PriceComfortS2"];
                                BK_TRANSADDONInfo.DutyCode = dr["SSRCodeDuty"].ToString();
                                BK_TRANSADDONInfo.DutyAmt = (decimal)dr["PriceDutyS2"];
                                BK_TRANSADDONInfo.InfantCode = dr["SSRCodeInfant"].ToString();
                                BK_TRANSADDONInfo.InfantAmt = (decimal)dr["PriceInfantS2"];
                                BK_TRANSADDONInfo.TotalAmount = (BK_TRANSADDONInfo.BaggageAmt + BK_TRANSADDONInfo.MealSubTotal1 + BK_TRANSADDONInfo.DrinkSubTotal1 + BK_TRANSADDONInfo.SportAmt + BK_TRANSADDONInfo.KitAmt + BK_TRANSADDONInfo.DutyAmt + BK_TRANSADDONInfo.InfantAmt);
                                BK_TRANSADDONInfo.CreateBy = MyUserSet.UserName;
                                BK_TRANSADDONInfo.LastSyncBy = MyUserSet.UserName;

                                listBK_TRANSADDONInfo.Add(BK_TRANSADDONInfo);
                                listBK_TRANSADDONInfo1t.Add(BK_TRANSADDONInfo);
                            }
                        }
                    }

                    if (model2 == null == false)
                    {
                        DataTable dataClass2 = new DataTable();
                        dataClass2 = (DataTable)HttpContext.Current.Session["dtGridPass2"];
                        foreach (DataRow dr in dataClass2.Rows)
                        {
                            if (dr["SSRCodeBaggage"].ToString() != "" || dr["SSRCodeMeal"].ToString() != "" || dr["SSRCodeDrink"].ToString() != "" || dr["SSRCodeSport"].ToString() != "" || dr["SSRCodeComfort"].ToString() != "" || dr["SSRCodeDuty"].ToString() != "" || dr["SSRCodeMeal1"].ToString() != "" || dr["SSRCodeDrink1"].ToString() != "" || dr["SSRCodeInfant"].ToString() != "")
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
                                BK_TRANSADDONInfo.BaggageCode = dr["SSRCodeBaggage"].ToString();
                                BK_TRANSADDONInfo.BaggageAmt = (decimal)dr["PriceBaggageS1"];
                                BK_TRANSADDONInfo.MealCode1 = dr["SSRCodeMeal"].ToString();
                                BK_TRANSADDONInfo.MealRate1 = (decimal)dr["PriceMeal"];
                                BK_TRANSADDONInfo.MealQty1 = 1;
                                BK_TRANSADDONInfo.MealSubTotal1 = (BK_TRANSADDONInfo.MealRate1 * BK_TRANSADDONInfo.MealQty1);
                                BK_TRANSADDONInfo.MealTotalAmt = (BK_TRANSADDONInfo.MealSubTotal1);
                                BK_TRANSADDONInfo.DrinkCode1 = dr["SSRCodeDrink"].ToString();
                                BK_TRANSADDONInfo.DrinkRate1 = (decimal)dr["PriceDrink"];
                                BK_TRANSADDONInfo.DrinkQty1 = 1;
                                BK_TRANSADDONInfo.DrinkSubTotal1 = (BK_TRANSADDONInfo.DrinkRate1 * BK_TRANSADDONInfo.DrinkQty1);
                                BK_TRANSADDONInfo.DrinkTotalAmt = (BK_TRANSADDONInfo.DrinkSubTotal1);
                                BK_TRANSADDONInfo.SportCode = dr["SSRCodeSport"].ToString();
                                BK_TRANSADDONInfo.SportAmt = (decimal)dr["PriceSportS1"];
                                BK_TRANSADDONInfo.KitCode = dr["SSRCodeComfort"].ToString();
                                BK_TRANSADDONInfo.KitAmt = (decimal)dr["PriceComfortS1"];
                                BK_TRANSADDONInfo.DutyCode = dr["SSRCodeDuty"].ToString();
                                BK_TRANSADDONInfo.DutyAmt = (decimal)dr["PriceDutyS1"];
                                BK_TRANSADDONInfo.InfantCode = dr["SSRCodeInfant"].ToString();
                                BK_TRANSADDONInfo.InfantAmt = (decimal)dr["PriceInfantS1"];
                                BK_TRANSADDONInfo.TotalAmount = (BK_TRANSADDONInfo.BaggageAmt + BK_TRANSADDONInfo.MealSubTotal1 + BK_TRANSADDONInfo.DrinkSubTotal1 + BK_TRANSADDONInfo.SportAmt + BK_TRANSADDONInfo.KitAmt + BK_TRANSADDONInfo.DutyAmt + BK_TRANSADDONInfo.InfantAmt);
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
                                    BK_TRANSADDONInfo.BaggageCode = dr["SSRCodeBaggage"].ToString();
                                    BK_TRANSADDONInfo.BaggageAmt = (decimal)dr["PriceBaggageS2"];
                                    BK_TRANSADDONInfo.MealCode1 = dr["SSRCodeMeal1"].ToString();
                                    BK_TRANSADDONInfo.MealRate1 = (decimal)dr["PriceMeal1"];
                                    BK_TRANSADDONInfo.MealQty1 = 1;
                                    BK_TRANSADDONInfo.MealSubTotal1 = (BK_TRANSADDONInfo.MealRate1 * BK_TRANSADDONInfo.MealQty1);
                                    BK_TRANSADDONInfo.MealTotalAmt = (BK_TRANSADDONInfo.MealSubTotal1);
                                    BK_TRANSADDONInfo.DrinkCode1 = dr["SSRCodeDrink1"].ToString();
                                    BK_TRANSADDONInfo.DrinkRate1 = (decimal)dr["PriceDrink1"];
                                    BK_TRANSADDONInfo.DrinkQty1 = 1;
                                    BK_TRANSADDONInfo.DrinkSubTotal1 = (BK_TRANSADDONInfo.DrinkRate1 * BK_TRANSADDONInfo.DrinkQty1);
                                    BK_TRANSADDONInfo.DrinkTotalAmt = (BK_TRANSADDONInfo.DrinkSubTotal1);
                                    BK_TRANSADDONInfo.SportCode = dr["SSRCodeSport"].ToString();
                                    BK_TRANSADDONInfo.SportAmt = (decimal)dr["PriceSportS2"];
                                    BK_TRANSADDONInfo.KitCode = dr["SSRCodeComfort"].ToString();
                                    BK_TRANSADDONInfo.KitAmt = (decimal)dr["PriceComfortS2"];
                                    BK_TRANSADDONInfo.DutyCode = dr["SSRCodeDuty"].ToString();
                                    BK_TRANSADDONInfo.DutyAmt = (decimal)dr["PriceDutyS2"];
                                    BK_TRANSADDONInfo.InfantCode = dr["SSRCodeInfant"].ToString();
                                    BK_TRANSADDONInfo.InfantAmt = (decimal)dr["PriceInfantS2"];
                                    BK_TRANSADDONInfo.TotalAmount = (BK_TRANSADDONInfo.BaggageAmt + BK_TRANSADDONInfo.MealSubTotal1 + BK_TRANSADDONInfo.DrinkSubTotal1 + BK_TRANSADDONInfo.SportAmt + BK_TRANSADDONInfo.KitAmt + BK_TRANSADDONInfo.DutyAmt + BK_TRANSADDONInfo.InfantAmt);
                                    BK_TRANSADDONInfo.CreateBy = MyUserSet.UserName;
                                    BK_TRANSADDONInfo.LastSyncBy = MyUserSet.UserName;

                                    listBK_TRANSADDONInfo.Add(BK_TRANSADDONInfo);
                                    listBK_TRANSADDONInfo2t.Add(BK_TRANSADDONInfo);
                                }
                            }
                        }
                    }



                    HttpCookie cookie = Request.Cookies["cookieSearchcondition"];
                    if (cookie != null)
                    {

                        Decimal sumdtList1 = 0;
                        Decimal sumdtList1t = Convert.ToDecimal(cookie.Values["PaxNum"]);
                        Decimal sumdtList2 = Convert.ToDecimal(cookie.Values["PaxNum"]);
                        Decimal sumdtList2t = Convert.ToDecimal(cookie.Values["PaxNum"]);

                        sumInfant = dataClass.Compute("Count(Infant)", "Infant <> ''");
                        cookie.Values.Add("InfantNum", sumInfant.ToString());
                        Response.AppendCookie(cookie);

                        DataTable dtdefaultBundledom = objBooking.GetDefaultBundle("DEFAULTDOMESTICBAGGAGE", model.TemFlightCarrierCode);
                        DataTable dtdefaultBundleInt = objBooking.GetDefaultBundle("DEFAULTINTERNATIONALBAGGAGE", model.TemFlightCarrierCode);
                        if ((dtdefaultBundledom != null && dtdefaultBundledom.Rows.Count > 0 && IsInternationalFlight == false) || (dtdefaultBundleInt != null && dtdefaultBundleInt.Rows.Count > 0 && IsInternationalFlight))
                        {
                            //log.Warning(this, "Validate first - " + model.TemFlightInternational.ToUpper() + " " + Session["TransID"].ToString());
                            if (listBK_TRANSADDONInfo1.Count > 0) sumdtList1 = (from s in listBK_TRANSADDONInfo1 where s.BaggageCode != null && s.BaggageCode != "" select s.BaggageCode).Count();
                            if (listBK_TRANSADDONInfo2.Count > 0) sumdtList2 = (from s in listBK_TRANSADDONInfo2 where s.BaggageCode != null && s.BaggageCode != "" select s.BaggageCode).Count();
                            if (listBK_TRANSADDONInfo1t.Count > 0) sumdtList1t = (from s in listBK_TRANSADDONInfo1t where s.BaggageCode != null && s.BaggageCode != "" select s.BaggageCode).Count();
                            if (listBK_TRANSADDONInfo2t.Count > 0) sumdtList2t = (from s in listBK_TRANSADDONInfo2t where s.BaggageCode != null && s.BaggageCode != "" select s.BaggageCode).Count();

                            //log.Warning(this, "Validate ( " + sumdtList1 + ") - " + model.TemFlightInternational.ToUpper() + " " + Session["TransID"].ToString());
                            //log.Warning(this, "Validate ( " + Convert.ToDecimal(cookie.Values["PaxNum"]) + ") - " + model.TemFlightInternational.ToUpper() + " " + Session["TransID"].ToString());
                            if ((Convert.ToDecimal(sumdtList1) == Convert.ToDecimal(cookie.Values["PaxNum"])) && (Convert.ToDecimal(sumdtList1t) == Convert.ToDecimal(cookie.Values["PaxNum"])) && (Convert.ToDecimal(sumdtList2) == Convert.ToDecimal(cookie.Values["PaxNum"])) && (Convert.ToDecimal(sumdtList2t) == Convert.ToDecimal(cookie.Values["PaxNum"])))
                            {
                                using (profiler.Step("SellFlight"))
                                {
                                    if (SellFlight(listBK_TRANSADDONInfo1, listBK_TRANSADDONInfo2, listBK_TRANSADDONInfo1t, listBK_TRANSADDONInfo2t, listBK_TRANSADDONInfo) != "")
                                    {
                                        e.Result = msgList.Err100055;
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                if ((Session["dtBaggageDepart"] != null && ((DataTable)Session["dtBaggageDepart"]).Rows.Count > 0))
                                {
                                    //log.Warning(this, "Validate second" + Session["TransID"].ToString());
                                    e.Result = "You are not allow to remove baggage base on the AOC rule, please contact Group Desk for further information.";
                                    return;
                                }
                                else
                                {
                                    //log.Warning(this, "Validate third" + Session["TransID"].ToString());
                                    if (listBK_TRANSADDONInfo1.Count > 0) sumdtList1 = (from s in listBK_TRANSADDONInfo1 where s.MealCode1 != "" || s.DrinkCode1 != "" || s.InfantCode != "" || s.SportCode != "" || s.KitCode != "" select s.BaggageCode).Count();
                                    if (listBK_TRANSADDONInfo2.Count > 0) sumdtList2 = (from s in listBK_TRANSADDONInfo2 where s.MealCode1 != "" || s.DrinkCode1 != "" || s.InfantCode != "" || s.SportCode != "" || s.KitCode != "" select s.BaggageCode).Count();
                                    if (listBK_TRANSADDONInfo1t.Count > 0) sumdtList1t = (from s in listBK_TRANSADDONInfo1t where s.MealCode1 != "" || s.DrinkCode1 != "" || s.InfantCode != "" || s.SportCode != "" || s.KitCode != "" select s.BaggageCode).Count();
                                    if (listBK_TRANSADDONInfo2t.Count > 0) sumdtList2t = (from s in listBK_TRANSADDONInfo2t where s.MealCode1 != "" || s.DrinkCode1 != "" || s.InfantCode != "" || s.SportCode != "" || s.KitCode != "" select s.BaggageCode).Count();

                                    if (sumdtList1 > 0 || sumdtList2 > 0 || sumdtList1t > 0 || sumdtList2t > 0)
                                    {
                                        using (profiler.Step("SellFlight"))
                                        {
                                            if (SellFlight(listBK_TRANSADDONInfo1, listBK_TRANSADDONInfo2, listBK_TRANSADDONInfo1t, listBK_TRANSADDONInfo2t, listBK_TRANSADDONInfo) != "")
                                            {
                                                e.Result = msgList.Err100055;
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            //log.Warning(this, "Validate Fourth" + Session["TransID"].ToString());
                            using (profiler.Step("SellFlight"))
                            {
                                if (SellFlight(listBK_TRANSADDONInfo1, listBK_TRANSADDONInfo2, listBK_TRANSADDONInfo1t, listBK_TRANSADDONInfo2t, listBK_TRANSADDONInfo) != "")
                                {
                                    e.Result = msgList.Err100055;
                                    return;
                                }
                            }
                        }


                        if (Request.QueryString["change"] != null)
                        {

                            if (Session["ChgbookHDRInfo"] != null && Session["ChglstbookDTLInfo"] != null)
                            {
                                using (profiler.Step("UpdateBookingJourney"))
                                {
                                    UpdateAllBookingJourneyDetail();
                                }
                            }
                        }
                        e.Result = "";


                        //20170530 - Sienny (put amount due to session)
                        if (Session["TotalAmountDue"] != null)
                        {
                            Session["TotalAmountDue"] = objGeneral.RoundUp(Convert.ToDecimal(Session["TotalAmountDue"].ToString()) + Convert.ToDecimal(lblTotalAmount.Text)).ToString("N", nfi);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                e.Result = msgList.Err100055;
                log.Error(this, ex);
            }
        }

        private string SellFlight(List<Bk_transaddon> Listing1, List<Bk_transaddon> Listing2, List<Bk_transaddon> Listing1t, List<Bk_transaddon> Listing2t, List<Bk_transaddon> listAll)
        {
            var profiler = MiniProfiler.Current;
            ClearSSRFeeValue();

            MessageList msgList = new MessageList();
            Session["totalcountpax"] = null;
            Decimal totalSSRdepart = 0;
            Decimal totalSSRReturn = 0;
            Decimal totalInfantdepart = 0;
            Decimal totalInfantReturn = 0;

            SellResponse responseSSR = new SellResponse();

            decimal TotSSRDepart = 0;
            decimal TotSSRReturn = 0;
            decimal TotInfantDepart = 0;
            decimal TotInfantReturn = 0;


            decimal TotalAmountGoing = 0;
            decimal TotalAmountReturn = 0;

            //DataTable dtTFOthPassFee = new DataTable();
            //DataTable dtSSRFee = new DataTable();
            //DataRow rowTFOth;
            //if (Session["dataTFOthPassFee"] != null)
            //    dtTFOthPassFee = (DataTable)Session["dataTFOthPassFee"];

            try
            {
                int TotalPax = 0, PaxAdult = 0, PaxChild = 0;

                List<BookingTransactionDetail> listBookingDetail = new List<BookingTransactionDetail>();
                DataTable dataClass = new DataTable();
                if (HttpContext.Current.Session["dataClassTrans"] != null)
                {
                    dataClass = (DataTable)HttpContext.Current.Session["dataClassTrans"];
                }
                List<BookingTransactionDetail> objListBK_TRANSDTL_Infos = new List<BookingTransactionDetail>();
                BookingTransactionDetail objBK_TRANSDTL_Infos;
                if (Request.QueryString["change"] != null)
                {
                    if (Session["ChglstbookDTLInfo"] != null)
                    {
                        listBookingDetail = (List<BookingTransactionDetail>)Session["ChglstbookDTLInfo"];
                    }
                }
                else
                {
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
                }

                HttpCookie cookie = Request.Cookies["cookieTemFlight"];
                if (cookie != null)
                {
                    if (IsNumeric(cookie.Values["list1ID"]))
                    {
                        departID = Convert.ToInt32(cookie.Values["list1ID"]);
                    }
                    else
                    {
                        departID = -1;
                    }

                    ReturnID = cookie.Values["ReturnID"];
                    num = Convert.ToInt32(cookie.Values["PaxNum"]);
                }

                ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
                ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight2 = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();

                string strExpr;
                string strSort;
                DataTable dt = new DataTable();
                strExpr = "TemFlightId = '" + departID + "'";
                strSort = "";

                dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                FillModelFromDataRow(foundRows, ref temFlight);

                string getfare = temFlight.TemFlightServiceCharge.ToString();


                if (ReturnID != "")
                {
                    strExpr = "TemFlightId = '" + ReturnID + "'";
                    strSort = "";

                    dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                    foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                    FillModelFromDataRow(foundRows, ref temFlight2);

                }

                if (listBookingDetail != null)
                {
                    for (int iii = 0; iii < listBookingDetail.Count; iii++)
                    {
                        if (listBookingDetail[iii].Origin == listBookingDetail[0].Origin)
                        {
                            totalSSRdepart = 0;
                            totalSSRReturn = 0;
                            totalInfantdepart = 0;
                            totalInfantReturn = 0;

                            List<Bk_transaddon> List1 = Listing1.Where(item => item.RecordLocator == listBookingDetail[iii].RecordLocator).ToList();
                            List<Bk_transaddon> List2 = Listing2.Where(item => item.RecordLocator == listBookingDetail[iii].RecordLocator).ToList();
                            List<Bk_transaddon> List1t = Listing1t.Where(item => item.RecordLocator == listBookingDetail[iii].RecordLocator).ToList();
                            List<Bk_transaddon> List2t = Listing2t.Where(item => item.RecordLocator == listBookingDetail[iii].RecordLocator).ToList();


                            ABS.Navitaire.APIBooking APIBooking = new ABS.Navitaire.APIBooking(SessionID);

                            GetSSRAvailabilityForBookingResponse ssrResponse = (GetSSRAvailabilityForBookingResponse)Session["GetssrAvailabilityResponseForBookingDrink"];
                            if (ssrResponse != null)
                            {

                                SessionID = listBookingDetail[iii].Signature;
                                if (List1.Count > 0 || List1t.Count > 0 || List2.Count > 0 || List2t.Count > 0)
                                {

                                    if (Convert.ToBoolean(Session["back"]) == true)
                                    {
                                        CancelResponse responseCancel = objBooking.CancelSSR(SessionID, ssrResponse, Curr, (List<Bk_transaddon>)Session["List1" + iii], (List<Bk_transaddon>)Session["List1t" + iii], (List<Bk_transaddon>)Session["List2" + iii], (List<Bk_transaddon>)Session["List2t" + iii]);
                                        if (responseCancel.BookingUpdateResponseData.Success != null)
                                        {
                                            if (Request.QueryString["change"] != null)
                                            {
                                                responseSSR = objBooking.SellSSR(signature, ssrResponse, List1, List1t, List2, List2t, true, listAll[0].RecordLocator, temFlight.TemFlightCurrencyCode, "change");
                                            }
                                            else
                                            {
                                                responseSSR = objBooking.SellSSR(SessionID, ssrResponse, List1, List1t, List2, List2t, true);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Request.QueryString["change"] != null)
                                        {
                                            responseSSR = objBooking.SellSSR(signature, ssrResponse, List1, List1t, List2, List2t, true, listAll[0].RecordLocator, temFlight.TemFlightCurrencyCode, "change");
                                        }
                                        else
                                        {
                                            responseSSR = objBooking.SellSSR(SessionID, ssrResponse, List1, List1t, List2, List2t, false);
                                        }
                                    }


                                    Session["List1" + iii] = List1;
                                    Session["List2" + iii] = List2;
                                    Session["List1t" + iii] = List1t;
                                    Session["List2t" + iii] = List2t;

                                    if (responseSSR != null && responseSSR.BookingUpdateResponseData.Success != null)
                                    {
                                        ABS.Navitaire.BookingManager.Booking book = new ABS.Navitaire.BookingManager.Booking();// APIBooking.GetBookingFromState(SessionID);
                                        using (profiler.Step("Navitaire:GetBookingFromState"))
                                        {
                                            book = APIBooking.GetBookingFromState(SessionID);
                                        }

                                        //string xml = GetXMLString(book);

                                        totalSSRdepart = book.Passengers.Select(p => p.PassengerFees.Where(fee => fee.FeeType == FeeType.SSRFee && fee.FlightReference != "" && fee.SSRCode != "INFT" && (fee.FlightReference.Substring(16, 3) == temFlight.TemFlightDeparture || fee.FlightReference.Substring(16, 6) == temFlight.TemFlightDeparture + temFlight.TemFlightTransit || fee.FlightReference.Substring(16, 6) == temFlight.TemFlightTransit + temFlight.TemFlightArrival)).Select(charge => charge.ServiceCharges.Select(x => x.Amount).Sum()).Sum()).Sum();

                                        totalSSRReturn = book.Passengers.Select(p => p.PassengerFees.Where(fee => fee.FeeType == FeeType.SSRFee && fee.FlightReference != "" && fee.SSRCode != "INFT" && (fee.FlightReference.Substring(16, 3) == temFlight2.TemFlightDeparture || fee.FlightReference.Substring(16, 6) == temFlight2.TemFlightDeparture + temFlight2.TemFlightTransit || fee.FlightReference.Substring(16, 6) == temFlight2.TemFlightTransit + temFlight2.TemFlightArrival)).Select(charge => charge.ServiceCharges.Select(x => x.Amount).Sum()).Sum()).Sum();

                                        totalInfantdepart = book.Passengers.Select(p => p.PassengerFees.Where(fee => fee.FeeType == FeeType.SSRFee && fee.FlightReference != "" && fee.SSRCode == "INFT" && (fee.FlightReference.Substring(16, 3) == temFlight.TemFlightDeparture || fee.FlightReference.Substring(16, 6) == temFlight.TemFlightDeparture + temFlight.TemFlightTransit || fee.FlightReference.Substring(16, 6) == temFlight.TemFlightTransit + temFlight.TemFlightArrival)).Select(charge => charge.ServiceCharges.Select(x => x.Amount).Sum()).Sum()).Sum();

                                        totalInfantReturn = book.Passengers.Select(p => p.PassengerFees.Where(fee => fee.FeeType == FeeType.SSRFee && fee.FlightReference != "" && fee.SSRCode == "INFT" && (fee.FlightReference.Substring(16, 3) == temFlight2.TemFlightDeparture || fee.FlightReference.Substring(16, 6) == temFlight2.TemFlightDeparture + temFlight2.TemFlightTransit || fee.FlightReference.Substring(16, 6) == temFlight2.TemFlightTransit + temFlight2.TemFlightArrival)).Select(charge => charge.ServiceCharges.Select(x => x.Amount).Sum()).Sum()).Sum();

                                        //for (int i = 0; i < book.Passengers.Length; i++)
                                        //{
                                        //    for (int ii = 0; ii < book.Passengers[i].PassengerFees.Length; ii++)
                                        //    {
                                        //        if (book.Passengers[i].PassengerFees[ii].FeeType == FeeType.SSRFee && book.Passengers[i].PassengerFees[ii].SSRCode != "INFT")
                                        //        {
                                        //            if (book.Passengers[i].PassengerFees[ii].FlightReference != "")
                                        //            {
                                        //                if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 3) == temFlight.TemFlightDeparture) || (book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == temFlight.TemFlightDeparture + temFlight.TemFlightTransit) || (book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == temFlight.TemFlightTransit + temFlight.TemFlightArrival))
                                        //                {
                                        //                    for (int y = 0; y < book.Passengers[i].PassengerFees[ii].ServiceCharges.Length; y++)
                                        //                    {
                                        //                        totalSSRdepart += book.Passengers[i].PassengerFees[ii].ServiceCharges[y].Amount;

                                        //                    }
                                        //                }
                                        //                else if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 3) == temFlight2.TemFlightDeparture) || (book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == temFlight2.TemFlightDeparture + temFlight2.TemFlightTransit) || (book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == temFlight2.TemFlightTransit + temFlight2.TemFlightArrival))
                                        //                {
                                        //                    for (int y = 0; y < book.Passengers[i].PassengerFees[ii].ServiceCharges.Length; y++)
                                        //                    {
                                        //                        totalSSRReturn += book.Passengers[i].PassengerFees[ii].ServiceCharges[y].Amount;
                                        //                    }
                                        //                }
                                        //            }
                                        //        }
                                        //        else if (book.Passengers[i].PassengerFees[ii].FeeType == FeeType.SSRFee && book.Passengers[i].PassengerFees[ii].SSRCode == "INFT")
                                        //        {
                                        //            if (book.Passengers[i].PassengerFees[ii].FlightReference != "")
                                        //            {
                                        //                if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 3) == temFlight.TemFlightDeparture) || (book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == temFlight.TemFlightDeparture + temFlight.TemFlightTransit) || (book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == temFlight.TemFlightTransit + temFlight.TemFlightArrival))
                                        //                {
                                        //                    for (int y = 0; y < book.Passengers[i].PassengerFees[ii].ServiceCharges.Length; y++)
                                        //                    {
                                        //                        totalInfantdepart += book.Passengers[i].PassengerFees[ii].ServiceCharges[y].Amount;

                                        //                    }
                                        //                }
                                        //                else if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 3) == temFlight2.TemFlightDeparture) || (book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == temFlight2.TemFlightDeparture + temFlight2.TemFlightTransit) || (book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == temFlight2.TemFlightTransit + temFlight2.TemFlightArrival))
                                        //                {
                                        //                    for (int y = 0; y < book.Passengers[i].PassengerFees[ii].ServiceCharges.Length; y++)
                                        //                    {
                                        //                        totalInfantReturn += book.Passengers[i].PassengerFees[ii].ServiceCharges[y].Amount;
                                        //                    }
                                        //                }
                                        //            }
                                        //        }
                                        //    }
                                        //}

                                        TotSSRDepart += totalSSRdepart;
                                        TotSSRReturn += totalSSRReturn;
                                        TotInfantDepart += totalInfantdepart;
                                        TotInfantReturn += totalInfantReturn;

                                        //#region DtTransFee
                                        //dtSSRFee = objBooking.dtTransFees();

                                        //rowTFOth = dtSSRFee.NewRow();
                                        //rowTFOth["Origin"] = temFlight.TemFlightDeparture;
                                        //rowTFOth["Transit"] = temFlight.TemFlightTransit;
                                        //rowTFOth["Destination"] = temFlight.TemFlightArrival;
                                        //rowTFOth["FeeGroup"] = 1;
                                        //rowTFOth["FeeType"] = "SSR";
                                        //rowTFOth["FeeCode"] = "SSR";
                                        //rowTFOth["FeeQty"] = 1;
                                        //rowTFOth["FeeRate"] = TotSSRDepart + TotInfantDepart;
                                        //rowTFOth["FeeAmt"] = TotSSRDepart + TotInfantDepart;
                                        //dtSSRFee.Rows.Add(rowTFOth);

                                        //rowTFOth = dtSSRFee.NewRow();
                                        //rowTFOth["Origin"] = temFlight2.TemFlightDeparture;
                                        //rowTFOth["Transit"] = temFlight2.TemFlightTransit;
                                        //rowTFOth["Destination"] = temFlight2.TemFlightArrival;
                                        //rowTFOth["FeeGroup"] = 1;
                                        //rowTFOth["FeeType"] = "SSR";
                                        //rowTFOth["FeeCode"] = "SSR";
                                        //rowTFOth["FeeQty"] = 1;
                                        //rowTFOth["FeeRate"] = TotSSRReturn + TotInfantReturn;
                                        //rowTFOth["FeeAmt"] = TotSSRReturn + TotInfantReturn;
                                        //dtSSRFee.Rows.Add(rowTFOth);
                                        //#endregion DtTransFee


                                        if (ReturnID != "")
                                        {
                                            int iIndexDepart = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 1 && p.Signature == SessionID);
                                            int iIndexReturn = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 0 && p.Signature == SessionID);
                                            if (iIndexDepart >= 0)
                                            {
                                                listBookingDetail[iIndexDepart].LineSSR = totalSSRdepart;
                                                listBookingDetail[iIndexDepart].LineInfant = totalInfantdepart;
                                            }
                                            TotalPax += Convert.ToInt32(listBookingDetail[iIndexDepart].TotalPax);
                                            PaxAdult += Convert.ToInt32(listBookingDetail[iIndexDepart].PaxAdult);
                                            PaxChild += Convert.ToInt32(listBookingDetail[iIndexDepart].PaxChild);
                                            if (iIndexReturn >= 0)
                                            {
                                                listBookingDetail[iIndexReturn].LineSSR = totalSSRReturn;
                                                listBookingDetail[iIndexReturn].LineInfant = totalInfantReturn;
                                            }
                                        }
                                        else
                                        {
                                            int iIndexDepart = listBookingDetail.FindIndex(p => p.Signature == SessionID);
                                            if (iIndexDepart >= 0)
                                            {
                                                listBookingDetail[iIndexDepart].LineSSR = totalSSRdepart;
                                                listBookingDetail[iIndexDepart].LineInfant = totalInfantdepart;
                                            }
                                            TotalPax += Convert.ToInt32(listBookingDetail[iIndexDepart].TotalPax);
                                            PaxAdult += Convert.ToInt32(listBookingDetail[iIndexDepart].PaxAdult);
                                            PaxChild += Convert.ToInt32(listBookingDetail[iIndexDepart].PaxChild);
                                        }

                                        DataRow[] result = dataClass.Select("SellSignature = '" + SessionID + "'");
                                        if (result.Length > 0)
                                        {
                                            int SelectedIndex = dataClass.Rows.IndexOf(result[0]);

                                            dataClass.Rows[SelectedIndex]["SSRChrg"] = Convert.ToDecimal(dataClass.Rows[SelectedIndex]["SSRChrg"].ToString()) + totalSSRdepart + totalSSRReturn;
                                            dataClass.Rows[SelectedIndex]["InfantChrg"] = Convert.ToDecimal(dataClass.Rows[SelectedIndex]["InfantChrg"].ToString()) + totalInfantdepart + totalInfantReturn;
                                            dataClass.Rows[SelectedIndex]["FullPrice"] = Convert.ToDecimal(dataClass.Rows[SelectedIndex]["FullPrice"].ToString()) + totalSSRdepart + totalSSRReturn + totalInfantdepart + totalInfantReturn;
                                        }
                                    }
                                    else
                                    {
                                        log.Error(this, "sell SSR failed = " + listAll[0].TransID);
                                        return "failed";
                                    }
                                }
                            }
                        }
                    }
                    HttpContext.Current.Session["dataClassTrans"] = dataClass;
                    if (Request.QueryString["change"] != null)
                    {
                        objBK_TRANSDTL_Infos = new BookingTransactionDetail();
                        objBK_TRANSDTL_Infos.RecordLocator = listBookingDetail[0].RecordLocator;
                        objBK_TRANSDTL_Infos.Signature = SessionID;
                        objListBK_TRANSDTL_Infos.Add(objBK_TRANSDTL_Infos);
                        HttpContext.Current.Session["objListBK_TRANSDTL_Infos"] = objListBK_TRANSDTL_Infos;
                    }
                    UpdateTotalAmount(TotSSRDepart, TotSSRReturn, ref TotalAmountGoing, ref TotalAmountReturn, TotalPax, PaxAdult, PaxChild);
                    HttpContext.Current.Session.Remove("ChglstbookDTLInfo");
                    HttpContext.Current.Session.Add("ChglstbookDTLInfo", listBookingDetail);

                }

                int cnt = 0;
                objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                if (Request.QueryString["change"] != null)
                {
                    HttpContext.Current.Session.Remove("ChgTransSSR");
                    HttpContext.Current.Session.Add("ChgTransSSR", listAll);
                    Session["ChgMode"] = "3"; //3 = Flight Change

                }
                else
                {
                    foreach (Bk_transaddon b in listAll)
                    {
                        cnt++;
                        if (objBooking.Update(b, "", true, cnt == listAll.Count ? true : false) == false)
                        {
                            log.Error(this, "save BK_TRANSADDON failed = " + listAll[0].TransID);
                            return "failed";
                        }
                    }

                    DataTable dataPassenger = new DataTable();
                    dataPassenger = (DataTable)HttpContext.Current.Session["dtGridPass"];
                    for (int i = 0; i < dataPassenger.Rows.Count; i++)
                    {
                        if (dataPassenger.Rows[i]["Infant"].ToString() != "")
                        {
                            PassData2 = new PassengerData();
                            PassData2.TransID = dataPassenger.Rows[i]["TransID"].ToString();
                            PassData2.RecordLocator = dataPassenger.Rows[i]["PNR"].ToString();
                            PassData2.PassengerID = dataPassenger.Rows[i]["PassengerID"].ToString();
                            PassData2.Nationality = dataPassenger.Rows[i]["Nationality"].ToString();
                            PassData2.IssuingCountry = dataPassenger.Rows[i]["IssuingCountry"].ToString();
                            PassData2.Title = "INFT";
                            PassData2.Gender = "Male";
                            PassData2.FirstName = "Infant";
                            PassData2.LastName = "Infant";
                            lstPassInfantData.Add(PassData2);
                        }
                    }

                    if (lstPassInfantData.Count > 0)
                    {
                        objBooking.SaveBK_PASSENGERLIST(lstPassInfantData, CoreBase.EnumSaveType.Insert);
                    }


                    //#region SaveTransFee
                    //if (dtSSRFee != null && dtSSRFee.Rows.Count > 0)
                    //{
                    //    foreach (DataRow drFee in dtSSRFee.Rows)
                    //    {
                    //        bookFEEInfo = new BookingTransactionFees();
                    //        bookFEEInfo.TransID = dataPassenger.Rows[0]["TransID"].ToString();
                    //        bookFEEInfo.RecordLocator = dataPassenger.Rows[0]["PNR"].ToString();
                    //        bookFEEInfo.SeqNo = Convert.ToByte(drFee["SeqNo"].ToString());
                    //        bookFEEInfo.Origin = drFee["Origin"].ToString();
                    //        bookFEEInfo.Transit = drFee["Transit"].ToString();
                    //        bookFEEInfo.Destination = drFee["Destination"].ToString();
                    //        bookFEEInfo.PaxType = drFee["PaxType"].ToString();
                    //        bookFEEInfo.FeeGroup = Convert.ToByte(drFee["FeeGroup"].ToString());
                    //        bookFEEInfo.FeeType = drFee["FeeType"].ToString();
                    //        bookFEEInfo.FeeCode = drFee["FeeCode"].ToString();
                    //        bookFEEInfo.FeeDesc = drFee["FeeDesc"].ToString();
                    //        bookFEEInfo.FeeQty = Convert.ToDouble(drFee["FeeQty"].ToString());
                    //        bookFEEInfo.FeeRate = Convert.ToDecimal(drFee["FeeRate"].ToString());
                    //        bookFEEInfo.FeeAmt = Convert.ToDecimal(drFee["FeeAmt"].ToString());
                    //        bookFEEInfo.Transvoid = 0;
                    //        bookFEEInfo.CreateBy = MyUserSet.AgentID;
                    //        bookFEEInfo.SyncCreate = DateTime.Now;
                    //        bookFEEInfo.SyncLastUpd = DateTime.Now;
                    //        bookFEEInfo.LastSyncBy = MyUserSet.AgentID;

                    //        lstbookFEEInfo.Add(bookFEEInfo);
                    //        objBooking.SaveBK_TRANSFEES(bookFEEInfo, CoreBase.EnumSaveType.Insert);
                    //    }
                    //}
                    //#endregion SaveTransFee
                }

                //Session["back"] = true;

                if (listAll != null && listAll.Count > 0)
                {
                    BookingTransactionMain bookingMain = new BookingTransactionMain();

                    bookingMain = objBooking.GetSingleBK_TRANSMAIN(listAll[0].TransID);
                    if (bookingMain != null)
                    {
                        bookingMain.TransTotalInfant = TotInfantDepart + TotInfantReturn;
                        bookingMain.TransTotalSSR = TotSSRDepart + TotSSRReturn;
                        bookingMain.TotalAmtGoing = TotalAmountGoing;
                        bookingMain.TotalAmtReturn = TotalAmountReturn;
                        bookingMain.TransSubTotal = TotalAmountGoing + TotalAmountReturn;
                        bookingMain.TransTotalAmt = TotalAmountGoing + TotalAmountReturn;

                        objBooking.UpdateTotalSSR(listAll[0].TransID, bookingMain, listBookingDetail);

                    }
                }


                return "";
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                return ex.ToString();
            }
        }
        #endregion
        public Boolean UpdateAllBookingJourneyDetail()
        {
            var profiler = MiniProfiler.Current;
            //ABS.Navitaire.APIBooking ApiBook = new ABS.Navitaire.APIBooking("");
            ABS.Navitaire.BookingManager.GetBookingResponse Response = new ABS.Navitaire.BookingManager.GetBookingResponse();
            ABS.Navitaire.APIBooking APIBooking = new ABS.Navitaire.APIBooking(SessionID);
            try
            {
                string TransID = Session["TransID"].ToString();
                string PNR = "";
                DateTime expiredDate = DateTime.Now.AddDays(1);
                DateTime stdDate = DateTime.Now;

                string strOrigin = "";
                int goingreturn = 0;

                int totalPax = 0, Adt = 0, Chd = 0;
                decimal totalTransAmountAll = 0;
                decimal totalTransAmount = 0;
                decimal totalAmountGoing = 0;
                decimal totalAmountReturn = 0;
                decimal totalTransSubTotal = 0;
                decimal totalTransTotalFee = 0;
                decimal totalTransTotalTax = 0;
                decimal totalTransTotalPaxFee = 0;
                decimal totalTransTotalOth = 0;
                decimal totalTransTotalProcess = 0;
                decimal totalTransTotalSSR = 0;
                decimal totalTransTotalSeat = 0;
                decimal totalTransTotalNameChange = 0;
                decimal totalTransTotalInfant = 0;
                decimal totalTransTotalDisc = 0;
                decimal totalTransTotalPromoDisc = 0;

                BookingContainer BookingContainers = new BookingContainer(); //for booking details
                List<BookingJourneyContainer> listBookingJourneyContainers = new List<BookingJourneyContainer>(); //for journey list
                List<PaymentContainer> listPaymentContainers = new List<PaymentContainer>(); //for payment list
                List<PaymentContainer> lstPaymentContainer = new List<PaymentContainer>();

                List<BookingTransactionDetail> listBookingJourney = new List<BookingTransactionDetail>();
                List<BookingTransTender> listBookTransTenderInfo = new List<BookingTransTender>();
                List<PassengerContainer> lstPassengerContainer = new List<PassengerContainer>();
                PassengerInfantContainer objPassengerInfantModel = new PassengerInfantContainer();
                List<PassengerInfantContainer> lstPassengerInfantModel = new List<PassengerInfantContainer>();
                BookingTransactionDetail lstBooking = new BookingTransactionDetail();

                BookingTransactionDetail bookDTLInfo = new BookingTransactionDetail();
                BookingTransactionMain bookHDRInfo = new BookingTransactionMain();
                BookingTransTender bookTransTenderInfo = new BookingTransTender();

                BookingJourneyContainer objBookingJourneyContainer = new BookingJourneyContainer();
                List<BookingTransactionDetail> lstbookDTLInfoPrev = new List<BookingTransactionDetail>();
                List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
                List<BookingTransactionDetail> listDetailCombinePNR = new List<BookingTransactionDetail>();
                List<BookingJourneyContainer> lstBookingJourneyContainer = new List<BookingJourneyContainer>();
                bookHDRInfo = new BookingTransactionMain();

                PassengerContainer objPassengerContainer = new PassengerContainer();

                bookHDRInfo = (BookingTransactionMain)Session["ChgbookHDRInfo"];
                listDetailCombinePNR = (List<BookingTransactionDetail>)Session["listBookingDetailCombine"];

                if (HttpContext.Current.Session["ChgbookHDRInfo"] != null && HttpContext.Current.Session["TransDetail"] != null)
                {
                    //DataTable dtTransMain = objBooking.dtTransMain();
                    if (HttpContext.Current.Session["ChgbookHDRInfo"] != null)
                        bookHDRInfo = (BookingTransactionMain)HttpContext.Current.Session["ChgbookHDRInfo"];//insert transmain into datatable

                    DataTable dtTransDetail = objBooking.dtTransDetail();
                    if (HttpContext.Current.Session["TransDetailAll"] != null)
                        dtTransDetail = (DataTable)HttpContext.Current.Session["TransDetailAll"];//insert transdtlcombinePNR into datatable



                    List<BookingTransactionDetail> listBookingDetail = new List<BookingTransactionDetail>();
                    if (Session["ChglstbookDTLInfo"] != null)
                    {
                        listBookingDetail = (List<BookingTransactionDetail>)Session["ChglstbookDTLInfo"];
                    }


                    int rowBookingJourneySeqNo = 1;
                    int cnt = 0;
                    int z = 0;


                    PNR = dtTransDetail.Rows[0]["RecordLocator"].ToString();
                    if (PNR.Trim().Length < 6)//will not continue if PNR is not valid
                    {
                        rowBookingJourneySeqNo += Convert.ToInt16(dtTransDetail.Rows[0]["CntRec"].ToString());
                    }

                    ABS.Navitaire.BookingManager.Booking book = new ABS.Navitaire.BookingManager.Booking();// APIBooking.GetBookingFromState(SessionID);
                    using (profiler.Step("Navitaire:GetBookingFromState"))
                    {
                        book = APIBooking.GetBookingFromState(SessionID);
                    }



                    #region "Load Passenger Fee"
                    //retrieve arrival, departure
                    string Departure = "", Arrival = "", Transit = "";


                    #endregion
                    foreach (Passenger itemPassenger in book.Passengers)
                    {
                        if (itemPassenger.PassengerTypeInfos[0].PaxType == "ADT")
                        {
                            objBookingJourneyContainer.AdtPax++;
                        }
                        else
                        {
                            objBookingJourneyContainer.ChdPax++;
                        }
                    }
                    totalPax = objBookingJourneyContainer.AdtPax + objBookingJourneyContainer.ChdPax;
                    Adt = objBookingJourneyContainer.AdtPax;
                    Chd = objBookingJourneyContainer.ChdPax;

                    ArrayList save = (ArrayList)Session["Chgsave"];
                    lstbookDTLInfoPrev = objBooking.GetAllBK_TRANSDTLFilterByPNR(save[1].ToString(), PNR);

                    for (int j = 0; j < book.Journeys.Length; j++)
                    {
                        objBookingJourneyContainer = new BookingJourneyContainer();
                        objBookingJourneyContainer.CurrencyCode = book.CurrencyCode;
                        for (int k = 0; k < book.Journeys[j].Segments.Length; k++)
                        {
                            switch (j)
                            {
                                case 0:
                                    if (k == 0)
                                    {
                                        for (int m = 0; m < book.Journeys[j].Segments[k].Fares[0].PaxFares.Length; m++)
                                        {
                                            if (book.Journeys[j].Segments[k].Fares[0].PaxFares[m].PaxType == "ADT")
                                            {

                                                foreach (BookingServiceCharge charges in book.Journeys[j].Segments[k].Fares[0].PaxFares[m].ServiceCharges)
                                                {
                                                    switch (charges.ChargeType.ToString().ToUpper())
                                                    {
                                                        case "FAREPRICE":
                                                            objBookingJourneyContainer.AdtFarePrice = charges.Amount;
                                                            break;
                                                        case "DISCOUNT":
                                                            objBookingJourneyContainer.AdtDiscChrg = 0 - charges.Amount;
                                                            break;
                                                        case "PROMOTIONDISCOUNT":
                                                            objBookingJourneyContainer.AdtPromoDiscChrg = 0 - charges.Amount;
                                                            break;
                                                        case "TRAVELFEE":
                                                            switch (charges.ChargeCode.ToString().ToUpper())
                                                            {
                                                                case "APT":
                                                                case "ATF":
                                                                case "APTF":
                                                                    objBookingJourneyContainer.AdtTaxChrg = charges.Amount;
                                                                    break;
                                                                case "FUEL":
                                                                    objBookingJourneyContainer.AdtFuelChrg = charges.Amount;
                                                                    break;
                                                                case "PSCH":
                                                                case "PSH":
                                                                    objBookingJourneyContainer.PaxFeeChrg = charges.Amount;
                                                                    break;
                                                                default:
                                                                    objBookingJourneyContainer.AdtServChrg += charges.Amount;
                                                                    break;
                                                            }
                                                            break;
                                                        default:
                                                            objBookingJourneyContainer.AdtServChrg += charges.Amount;
                                                            break;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                foreach (BookingServiceCharge charges in book.Journeys[j].Segments[k].Fares[0].PaxFares[m].ServiceCharges)
                                                {
                                                    switch (charges.ChargeType.ToString().ToUpper())
                                                    {
                                                        case "FAREPRICE":
                                                            objBookingJourneyContainer.ChdFarePrice = charges.Amount;
                                                            break;
                                                        case "DISCOUNT":
                                                            objBookingJourneyContainer.ChdDiscChrg = charges.Amount;
                                                            break;
                                                        case "PROMOTIONDISCOUNT":
                                                            objBookingJourneyContainer.AdtPromoDiscChrg = 0 - charges.Amount;
                                                            break;
                                                        case "TRAVELFEE":
                                                            switch (charges.ChargeCode.ToString().ToUpper())
                                                            {
                                                                case "APT":
                                                                case "ATF":
                                                                case "APTF":
                                                                    objBookingJourneyContainer.ChdTaxChrg = charges.Amount;
                                                                    break;
                                                                case "FUEL":
                                                                    objBookingJourneyContainer.ChdFuelChrg = charges.Amount;
                                                                    break;
                                                                case "PSCH":
                                                                case "PSH":
                                                                    objBookingJourneyContainer.PaxFeeChrg = charges.Amount;
                                                                    break;
                                                                default:
                                                                    objBookingJourneyContainer.ChdServChrg += charges.Amount;
                                                                    break;
                                                            }
                                                            break;
                                                        default:
                                                            objBookingJourneyContainer.ChdServChrg += charges.Amount;
                                                            break;
                                                    }
                                                }

                                            }
                                        }

                                        objBookingJourneyContainer.CarrierCode = book.Journeys[j].Segments[k].FlightDesignator.CarrierCode;
                                        objBookingJourneyContainer.FlightNumber = book.Journeys[j].Segments[k].FlightDesignator.FlightNumber;
                                        objBookingJourneyContainer.STA = book.Journeys[j].Segments[k].STA;
                                        objBookingJourneyContainer.STD = book.Journeys[j].Segments[k].STD;
                                        objBookingJourneyContainer.FareSellKey = book.Journeys[j].Segments[k].Fares[0].FareSellKey;
                                        objBookingJourneyContainer.FareClass = book.Journeys[j].Segments[k].Fares[0].FareClassOfService;
                                        objBookingJourneyContainer.ArrivalStation = book.Journeys[j].Segments[k].ArrivalStation;
                                        objBookingJourneyContainer.DepartureStation = book.Journeys[j].Segments[k].DepartureStation;
                                        objBookingJourneyContainer.OpSuffix = book.Journeys[j].Segments[k].Legs[0].FlightDesignator.OpSuffix;
                                        objBookingJourneyContainer.EquipmentType = book.Journeys[j].Segments[k].Legs[0].LegInfo.EquipmentType;
                                        objBookingJourneyContainer.FlightDesignator = book.Journeys[j].Segments[k].FlightDesignator;

                                    }
                                    if (k == 1)
                                    {
                                        for (int m = 0; m < book.Journeys[j].Segments[k].Fares[0].PaxFares.Length; m++)
                                        {
                                            if (book.Journeys[j].Segments[k].Fares[0].PaxFares[m].PaxType == "ADT")
                                            {

                                                foreach (BookingServiceCharge charges in book.Journeys[j].Segments[k].Fares[0].PaxFares[m].ServiceCharges)
                                                {
                                                    switch (charges.ChargeType.ToString().ToUpper())
                                                    {
                                                        case "FAREPRICE":
                                                            objBookingJourneyContainer.AdtFarePrice += charges.Amount;
                                                            break;
                                                        case "DISCOUNT":
                                                            objBookingJourneyContainer.AdtDiscChrg = 0 - charges.Amount;
                                                            break;
                                                        case "PROMOTIONDISCOUNT":
                                                            objBookingJourneyContainer.AdtPromoDiscChrg = 0 - charges.Amount;
                                                            break;
                                                        case "TRAVELFEE":
                                                            switch (charges.ChargeCode.ToString().ToUpper())
                                                            {
                                                                case "APT":
                                                                case "ATF":
                                                                case "APTF":
                                                                    objBookingJourneyContainer.AdtTaxChrg += charges.Amount;
                                                                    break;
                                                                case "FUEL":
                                                                    objBookingJourneyContainer.AdtFuelChrg += charges.Amount;
                                                                    break;
                                                                case "PSCH":
                                                                case "PSH":
                                                                    objBookingJourneyContainer.PaxFeeChrg += charges.Amount;
                                                                    break;
                                                                default:
                                                                    objBookingJourneyContainer.AdtServChrg += charges.Amount;
                                                                    break;
                                                            }
                                                            break;
                                                        default:
                                                            objBookingJourneyContainer.AdtServChrg += charges.Amount;
                                                            break;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                foreach (BookingServiceCharge charges in book.Journeys[j].Segments[k].Fares[0].PaxFares[m].ServiceCharges)
                                                {
                                                    switch (charges.ChargeType.ToString().ToUpper())
                                                    {
                                                        case "FAREPRICE":
                                                            objBookingJourneyContainer.ChdFarePrice += charges.Amount;
                                                            break;
                                                        case "DISCOUNT":
                                                            objBookingJourneyContainer.ChdDiscChrg += charges.Amount;
                                                            break;
                                                        case "PROMOTIONDISCOUNT":
                                                            objBookingJourneyContainer.AdtPromoDiscChrg = 0 - charges.Amount;
                                                            break;
                                                        case "TRAVELFEE":
                                                            switch (charges.ChargeCode.ToString().ToUpper())
                                                            {
                                                                case "APT":
                                                                case "ATF":
                                                                case "APTF":
                                                                    objBookingJourneyContainer.ChdTaxChrg += charges.Amount;
                                                                    break;
                                                                case "FUEL":
                                                                    objBookingJourneyContainer.ChdFuelChrg += charges.Amount;
                                                                    break;
                                                                case "PSCH":
                                                                case "PSH":
                                                                    objBookingJourneyContainer.PaxFeeChrg += charges.Amount;
                                                                    break;
                                                                default:
                                                                    objBookingJourneyContainer.ChdServChrg += charges.Amount;
                                                                    break;
                                                            }
                                                            break;
                                                        default:
                                                            objBookingJourneyContainer.ChdServChrg += charges.Amount;
                                                            break;
                                                    }
                                                }

                                            }
                                        }
                                        objBookingJourneyContainer.OverridedCarrierCode = book.Journeys[j].Segments[k].FlightDesignator.CarrierCode;
                                        objBookingJourneyContainer.OverridedFlightNumber = book.Journeys[j].Segments[k].FlightDesignator.FlightNumber;
                                        objBookingJourneyContainer.OverridedSTA = book.Journeys[j].Segments[k].STA;
                                        objBookingJourneyContainer.OverridedSTD = book.Journeys[j].Segments[k].STD;
                                        objBookingJourneyContainer.OverridedFareSellKey = book.Journeys[j].Segments[k].Fares[0].FareSellKey;
                                        objBookingJourneyContainer.FareClass = book.Journeys[j].Segments[k].Fares[0].FareClassOfService;
                                        objBookingJourneyContainer.OverridedArrivalStation = book.Journeys[j].Segments[k].ArrivalStation;
                                        objBookingJourneyContainer.OverridedDepartureStation = book.Journeys[j].Segments[k].DepartureStation;
                                        objBookingJourneyContainer.OverridedOpSuffix = book.Journeys[j].Segments[k].Legs[0].FlightDesignator.OpSuffix;
                                        objBookingJourneyContainer.OverridedEquipmentType = book.Journeys[j].Segments[k].Legs[0].LegInfo.EquipmentType;
                                        objBookingJourneyContainer.OverridedFlightDesignator = book.Journeys[j].Segments[k].FlightDesignator;
                                    }
                                    break;
                                case 1:
                                    if (k == 0)
                                    {
                                        for (int m = 0; m < book.Journeys[j].Segments[k].Fares[0].PaxFares.Length; m++)
                                        {
                                            if (book.Journeys[j].Segments[k].Fares[0].PaxFares[m].PaxType == "ADT")
                                            {

                                                foreach (BookingServiceCharge charges in book.Journeys[j].Segments[k].Fares[0].PaxFares[m].ServiceCharges)
                                                {
                                                    switch (charges.ChargeType.ToString().ToUpper())
                                                    {
                                                        case "FAREPRICE":
                                                            objBookingJourneyContainer.AdtFarePrice = charges.Amount;
                                                            break;
                                                        case "DISCOUNT":
                                                            objBookingJourneyContainer.AdtDiscChrg = 0 - charges.Amount;
                                                            break;
                                                        case "PROMOTIONDISCOUNT":
                                                            objBookingJourneyContainer.AdtPromoDiscChrg = 0 - charges.Amount;
                                                            break;
                                                        case "TRAVELFEE":
                                                            switch (charges.ChargeCode.ToString().ToUpper())
                                                            {
                                                                case "APT":
                                                                case "ATF":
                                                                case "APTF":
                                                                    objBookingJourneyContainer.AdtTaxChrg = charges.Amount;
                                                                    break;
                                                                case "FUEL":
                                                                    objBookingJourneyContainer.AdtFuelChrg = charges.Amount;
                                                                    break;
                                                                case "PSCH":
                                                                case "PSH":
                                                                    objBookingJourneyContainer.PaxFeeChrg = charges.Amount;
                                                                    break;
                                                                default:
                                                                    objBookingJourneyContainer.AdtServChrg += charges.Amount;
                                                                    break;
                                                            }
                                                            break;
                                                        default:
                                                            objBookingJourneyContainer.AdtServChrg += charges.Amount;
                                                            break;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                foreach (BookingServiceCharge charges in book.Journeys[j].Segments[k].Fares[0].PaxFares[m].ServiceCharges)
                                                {
                                                    switch (charges.ChargeType.ToString().ToUpper())
                                                    {
                                                        case "FAREPRICE":
                                                            objBookingJourneyContainer.ChdFarePrice = charges.Amount;
                                                            break;
                                                        case "DISCOUNT":
                                                            objBookingJourneyContainer.ChdDiscChrg = charges.Amount;
                                                            break;
                                                        case "PROMOTIONDISCOUNT":
                                                            objBookingJourneyContainer.AdtPromoDiscChrg = 0 - charges.Amount;
                                                            break;
                                                        case "TRAVELFEE":
                                                            switch (charges.ChargeCode.ToString().ToUpper())
                                                            {
                                                                case "APT":
                                                                case "ATF":
                                                                case "APTF":
                                                                    objBookingJourneyContainer.ChdTaxChrg = charges.Amount;
                                                                    break;
                                                                case "FUEL":
                                                                    objBookingJourneyContainer.ChdFuelChrg = charges.Amount;
                                                                    break;
                                                                case "PSCH":
                                                                case "PSH":
                                                                    objBookingJourneyContainer.PaxFeeChrg = charges.Amount;
                                                                    break;
                                                                default:
                                                                    objBookingJourneyContainer.ChdServChrg += charges.Amount;
                                                                    break;
                                                            }
                                                            break;
                                                        default:
                                                            objBookingJourneyContainer.ChdServChrg += charges.Amount;
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                        objBookingJourneyContainer.CarrierCode = book.Journeys[j].Segments[k].FlightDesignator.CarrierCode;
                                        objBookingJourneyContainer.FlightNumber = book.Journeys[j].Segments[k].FlightDesignator.FlightNumber;
                                        objBookingJourneyContainer.STA = book.Journeys[j].Segments[k].STA;
                                        objBookingJourneyContainer.STD = book.Journeys[j].Segments[k].STD;
                                        objBookingJourneyContainer.FareSellKey = book.Journeys[j].Segments[k].Fares[0].FareSellKey;
                                        objBookingJourneyContainer.FareClass = book.Journeys[j].Segments[k].Fares[0].FareClassOfService;
                                        objBookingJourneyContainer.ArrivalStation = book.Journeys[j].Segments[k].ArrivalStation;
                                        objBookingJourneyContainer.DepartureStation = book.Journeys[j].Segments[k].DepartureStation;
                                        objBookingJourneyContainer.OpSuffix = book.Journeys[j].Segments[k].Legs[0].FlightDesignator.OpSuffix;
                                        objBookingJourneyContainer.EquipmentType = book.Journeys[j].Segments[k].Legs[0].LegInfo.EquipmentType;
                                        objBookingJourneyContainer.FlightDesignator = book.Journeys[j].Segments[k].FlightDesignator;
                                    }
                                    if (k == 1)
                                    {
                                        for (int m = 0; m < book.Journeys[j].Segments[k].Fares[0].PaxFares.Length; m++)
                                        {
                                            if (book.Journeys[j].Segments[k].Fares[0].PaxFares[m].PaxType == "ADT")
                                            {

                                                foreach (BookingServiceCharge charges in book.Journeys[j].Segments[k].Fares[0].PaxFares[m].ServiceCharges)
                                                {
                                                    switch (charges.ChargeType.ToString().ToUpper())
                                                    {
                                                        case "FAREPRICE":
                                                            objBookingJourneyContainer.AdtFarePrice += charges.Amount;
                                                            break;
                                                        case "DISCOUNT":
                                                            objBookingJourneyContainer.AdtDiscChrg += 0 - charges.Amount;
                                                            break;
                                                        case "PROMOTIONDISCOUNT":
                                                            objBookingJourneyContainer.AdtPromoDiscChrg = 0 - charges.Amount;
                                                            break;
                                                        case "TRAVELFEE":
                                                            switch (charges.ChargeCode.ToString().ToUpper())
                                                            {
                                                                case "APT":
                                                                case "ATF":
                                                                case "APTF":
                                                                    objBookingJourneyContainer.AdtTaxChrg += charges.Amount;
                                                                    break;
                                                                case "FUEL":
                                                                    objBookingJourneyContainer.AdtFuelChrg += charges.Amount;
                                                                    break;
                                                                case "PSCH":
                                                                case "PSH":
                                                                    objBookingJourneyContainer.PaxFeeChrg += charges.Amount;
                                                                    break;
                                                                default:
                                                                    objBookingJourneyContainer.AdtServChrg += charges.Amount;
                                                                    break;
                                                            }
                                                            break;
                                                        default:
                                                            objBookingJourneyContainer.AdtServChrg += charges.Amount;
                                                            break;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                foreach (BookingServiceCharge charges in book.Journeys[j].Segments[k].Fares[0].PaxFares[m].ServiceCharges)
                                                {
                                                    switch (charges.ChargeType.ToString().ToUpper())
                                                    {
                                                        case "FAREPRICE":
                                                            objBookingJourneyContainer.ChdFarePrice += charges.Amount;
                                                            break;
                                                        case "DISCOUNT":
                                                            objBookingJourneyContainer.ChdDiscChrg += charges.Amount;
                                                            break;
                                                        case "PROMOTIONDISCOUNT":
                                                            objBookingJourneyContainer.AdtPromoDiscChrg = 0 - charges.Amount;
                                                            break;
                                                        case "TRAVELFEE":
                                                            switch (charges.ChargeCode.ToString().ToUpper())
                                                            {
                                                                case "APT":
                                                                case "ATF":
                                                                case "APTF":
                                                                    objBookingJourneyContainer.ChdTaxChrg += charges.Amount;
                                                                    break;
                                                                case "FUEL":
                                                                    objBookingJourneyContainer.ChdFuelChrg += charges.Amount;
                                                                    break;
                                                                case "PSCH":
                                                                case "PSH":
                                                                    objBookingJourneyContainer.PaxFeeChrg += charges.Amount;
                                                                    break;
                                                                default:
                                                                    objBookingJourneyContainer.ChdServChrg += charges.Amount;
                                                                    break;
                                                            }
                                                            break;
                                                        default:
                                                            objBookingJourneyContainer.ChdServChrg += charges.Amount;
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                        objBookingJourneyContainer.OverridedCarrierCode = book.Journeys[j].Segments[k].FlightDesignator.CarrierCode;
                                        objBookingJourneyContainer.OverridedFlightNumber = book.Journeys[j].Segments[k].FlightDesignator.FlightNumber;
                                        objBookingJourneyContainer.OverridedSTA = book.Journeys[j].Segments[k].STA;
                                        objBookingJourneyContainer.OverridedSTD = book.Journeys[j].Segments[k].STD;
                                        objBookingJourneyContainer.OverridedFareSellKey = book.Journeys[j].Segments[k].Fares[0].FareSellKey;
                                        objBookingJourneyContainer.OverridedFareClass = book.Journeys[j].Segments[k].Fares[0].FareClassOfService;
                                        objBookingJourneyContainer.OverridedArrivalStation = book.Journeys[j].Segments[k].ArrivalStation;
                                        objBookingJourneyContainer.OverridedDepartureStation = book.Journeys[j].Segments[k].DepartureStation;
                                        objBookingJourneyContainer.OverridedOpSuffix = book.Journeys[j].Segments[k].Legs[0].FlightDesignator.OpSuffix;
                                        objBookingJourneyContainer.OverridedEquipmentType = book.Journeys[j].Segments[k].Legs[0].LegInfo.EquipmentType;
                                        objBookingJourneyContainer.OverridedFlightDesignator = book.Journeys[j].Segments[k].FlightDesignator;
                                    }
                                    break;
                            }
                        }
                        //load booking Journey
                        objBookingJourneyContainer.RecordLocator = book.RecordLocator;
                        objBookingJourneyContainer.BookingID = book.BookingID;
                        objBookingJourneyContainer.JourneySellKey = book.Journeys[j].JourneySellKey;

                        objBookingJourneyContainer.AdtPax = Adt;
                        objBookingJourneyContainer.ChdPax = Chd;
                        totalPax = objBookingJourneyContainer.AdtPax + objBookingJourneyContainer.ChdPax;

                        bookDTLInfo = new BookingTransactionDetail();
                        bookDTLInfo.TransID = TransID;
                        bookDTLInfo.RecordLocator = book.RecordLocator;
                        bookDTLInfo.SeqNo = (byte)(j + 1);
                        bookDTLInfo.CarrierCode = objBookingJourneyContainer.CarrierCode;
                        bookDTLInfo.Currency = objBookingJourneyContainer.CurrencyCode;
                        bookDTLInfo.FareClass = objBookingJourneyContainer.FareClass;
                        bookDTLInfo.FlightNo = objBookingJourneyContainer.FlightNumber;
                        bookDTLInfo.Origin = objBookingJourneyContainer.DepartureStation;
                        bookDTLInfo.Transit = objBookingJourneyContainer.OverridedDepartureStation;
                        bookDTLInfo.Destination = objBookingJourneyContainer.ArrivalStation;
                        bookDTLInfo.DepatureDate = objBookingJourneyContainer.STD;
                        bookDTLInfo.ArrivalDate = objBookingJourneyContainer.STA;
                        bookDTLInfo.DepatureDate2 = objBookingJourneyContainer.OverridedSTD;
                        bookDTLInfo.ArrivalDate2 = objBookingJourneyContainer.OverridedSTA;
                        bookDTLInfo.SellKey = objBookingJourneyContainer.FareSellKey;
                        bookDTLInfo.OverridedSellKey = objBookingJourneyContainer.OverridedFareSellKey;
                        bookDTLInfo.Signature = SessionID;

                        if (bookDTLInfo.Transit != null && bookDTLInfo.Transit != "")
                        {
                            bookDTLInfo.Destination = objBookingJourneyContainer.OverridedArrivalStation;
                            bookDTLInfo.Transit = objBookingJourneyContainer.OverridedDepartureStation;
                        }

                        bookDTLInfo.PaxAdult = objBookingJourneyContainer.AdtPax;
                        bookDTLInfo.PaxChild = objBookingJourneyContainer.ChdPax;
                        if (objBookingJourneyContainer.AdtFarePrice != 0)
                            bookDTLInfo.FarePerPax = objBookingJourneyContainer.AdtFarePrice;
                        else
                            bookDTLInfo.FarePerPax = objBookingJourneyContainer.ChdFarePrice;

                        bookDTLInfo.LineTax = ((objBookingJourneyContainer.AdtTaxChrg + objBookingJourneyContainer.AdtFuelChrg) * objBookingJourneyContainer.AdtPax) + ((objBookingJourneyContainer.ChdTaxChrg + objBookingJourneyContainer.ChdFuelChrg) * objBookingJourneyContainer.ChdPax);
                        bookDTLInfo.LineOth = (objBookingJourneyContainer.AdtServChrg * objBookingJourneyContainer.AdtPax) + (objBookingJourneyContainer.ChdServChrg * objBookingJourneyContainer.ChdPax) + (objBookingJourneyContainer.OtherFee * (objBookingJourneyContainer.AdtPax + objBookingJourneyContainer.ChdPax)) + (objBookingJourneyContainer.SPLFee * (objBookingJourneyContainer.AdtPax + objBookingJourneyContainer.ChdPax));
                        //bookDTLInfo.LineOth = objBookingJourneyContainer.AdtServChrg * objBookingJourneyContainer.AdtPax + objBookingJourneyContainer.ChdServChrg * objBookingJourneyContainer.ChdPax;
                        bookDTLInfo.LineDisc = (objBookingJourneyContainer.AdtDiscChrg * objBookingJourneyContainer.AdtPax) + (objBookingJourneyContainer.ChdDiscChrg * objBookingJourneyContainer.ChdPax);
                        bookDTLInfo.LinePromoDisc = (objBookingJourneyContainer.AdtPromoDiscChrg * objBookingJourneyContainer.AdtPax) + (objBookingJourneyContainer.ChdPromoDiscChrg * objBookingJourneyContainer.ChdPax);
                        bookDTLInfo.LinePaxFee = (objBookingJourneyContainer.PaxFeeChrg * objBookingJourneyContainer.AdtPax) + (objBookingJourneyContainer.PaxFeeChrg * objBookingJourneyContainer.ChdPax);
                        bookDTLInfo.LineSSR = objBookingJourneyContainer.SSRChrg;
                        bookDTLInfo.LineSeat = objBookingJourneyContainer.SeatChrg;
                        bookDTLInfo.LineNameChange = objBookingJourneyContainer.NameChangeChrg;
                        bookDTLInfo.LineInfant = objBookingJourneyContainer.InfantChrg;

                        bookDTLInfo.LineVAT = objBookingJourneyContainer.VATFee;
                        bookDTLInfo.LineCharge = objBookingJourneyContainer.ChargeFee;

                        bookDTLInfo.LineTotal = (bookDTLInfo.FarePerPax * (bookDTLInfo.PaxAdult + bookDTLInfo.PaxChild)) + bookDTLInfo.LineTax + bookDTLInfo.LinePaxFee + bookDTLInfo.LineOth + bookDTLInfo.LineProcess + bookDTLInfo.LineDisc + bookDTLInfo.LinePromoDisc + bookDTLInfo.LineFee + bookDTLInfo.LineVAT + bookDTLInfo.LineCharge + bookDTLInfo.LineSSR + bookDTLInfo.LineSeat + bookDTLInfo.LineNameChange + bookDTLInfo.LineInfant;

                        bookDTLInfo.Currency = objBookingJourneyContainer.CurrencyCode;
                        bookDTLInfo.CommandType = "update";

                        bookDTLInfo.AttemptCount = 0;
                        bookDTLInfo.TransVoid = 0;
                        if (strOrigin == "") strOrigin = bookDTLInfo.Origin;
                        if (bookDTLInfo.Origin == strOrigin) goingreturn = 0;
                        else goingreturn = 1;
                        if (goingreturn == 0)
                        {
                            totalAmountGoing += bookDTLInfo.LineTotal;
                        }
                        else
                        {
                            totalAmountReturn += bookDTLInfo.LineTotal;
                        }
                        bookDTLInfo.PayDueAmount1 = lstbookDTLInfoPrev[j].PayDueAmount1;
                        bookDTLInfo.PayDueAmount2 = lstbookDTLInfoPrev[j].PayDueAmount2;
                        bookDTLInfo.PayDueAmount3 = lstbookDTLInfoPrev[j].PayDueAmount3;
                        bookDTLInfo.PayDueDate1 = lstbookDTLInfoPrev[j].PayDueDate1;
                        bookDTLInfo.PayDueDate2 = lstbookDTLInfoPrev[j].PayDueDate2;
                        bookDTLInfo.PayDueDate3 = lstbookDTLInfoPrev[j].PayDueDate3;

                        lstbookDTLInfo.Add(bookDTLInfo);
                    }
                    objBookingJourneyContainer.AdtDiscChrg = 0;
                    objBookingJourneyContainer.ChdDiscChrg = 0;
                    objBookingJourneyContainer.AdtPromoDiscChrg = 0;
                    objBookingJourneyContainer.ChdPromoDiscChrg = 0;
                    List<PassengerData> lstPassenger = new List<PassengerData>();
                    PassengerData rowPassenger;
                    //int passengerno = 0;
                    foreach (Passenger itemPassenger in book.Passengers)
                    {
                        rowPassenger = new PassengerData();
                        rowPassenger.PassengerID = itemPassenger.PassengerNumber.ToString();
                        rowPassenger.FirstName = itemPassenger.Names[0].FirstName;
                        rowPassenger.LastName = itemPassenger.Names[0].LastName;
                        rowPassenger.RecordLocator = PNR;


                        lstPassenger.Add(rowPassenger);
                    }
                    if (lstPassenger != null)
                    {
                        HttpContext.Current.Session.Remove("listPassengers");
                        HttpContext.Current.Session.Add("listPassengers", lstPassenger);
                    }


                    if (lstbookDTLInfo.Count > 0)
                    {

                        for (int i = cnt; i < lstbookDTLInfo.Count; i++)
                        {
                            dtTransDetail.Rows[i]["PaxAdult"] = 0;
                            dtTransDetail.Rows[i]["PaxChild"] = 0;
                            dtTransDetail.Rows[i]["LineTotal"] = 0;
                            dtTransDetail.Rows[i]["LineFee"] = 0;
                            dtTransDetail.Rows[i]["LineOth"] = 0;
                            dtTransDetail.Rows[i]["LineProcess"] = 0;
                            dtTransDetail.Rows[i]["LineSSR"] = 0;
                            dtTransDetail.Rows[i]["LineSeat"] = 0;
                            dtTransDetail.Rows[i]["LineNameChange"] = 0;
                            dtTransDetail.Rows[i]["LineInfant"] = 0;
                            dtTransDetail.Rows[i]["LineDisc"] = 0;
                            dtTransDetail.Rows[i]["LinePromoDisc"] = 0;
                            dtTransDetail.Rows[i]["LineTax"] = 0;
                            dtTransDetail.Rows[i]["LinePaxFee"] = 0;
                            BookingTransactionDetail pBookingTransDetail = new BookingTransactionDetail();
                            pBookingTransDetail = lstbookDTLInfo[i];

                            dtTransDetail.Rows[i]["PaxAdult"] = Convert.ToInt16(dtTransDetail.Rows[i]["PaxAdult"].ToString()) + Convert.ToInt16(pBookingTransDetail.PaxAdult.ToString());
                            dtTransDetail.Rows[i]["PaxChild"] = Convert.ToInt16(dtTransDetail.Rows[i]["PaxChild"].ToString()) + Convert.ToInt16(pBookingTransDetail.PaxChild.ToString());
                            dtTransDetail.Rows[i]["LineTotal"] = Convert.ToDecimal(dtTransDetail.Rows[i]["LineTotal"].ToString()) + Convert.ToDecimal(pBookingTransDetail.LineTotal.ToString());
                            dtTransDetail.Rows[i]["LineFee"] = Convert.ToDecimal(dtTransDetail.Rows[i]["LineFee"].ToString()) + Convert.ToDecimal(pBookingTransDetail.LineFee.ToString());
                            dtTransDetail.Rows[i]["LineOth"] = Convert.ToDecimal(dtTransDetail.Rows[i]["LineOth"].ToString()) + Convert.ToDecimal(pBookingTransDetail.LineOth.ToString());
                            dtTransDetail.Rows[i]["LineProcess"] = Convert.ToDecimal(dtTransDetail.Rows[i]["LineProcess"].ToString()) + Convert.ToDecimal(pBookingTransDetail.LineProcess.ToString());
                            dtTransDetail.Rows[i]["LineSSR"] = Convert.ToDecimal(dtTransDetail.Rows[i]["LineSSR"].ToString()) + Convert.ToDecimal(pBookingTransDetail.LineSSR.ToString());
                            dtTransDetail.Rows[i]["LineSeat"] = Convert.ToDecimal(dtTransDetail.Rows[i]["LineSeat"].ToString()) + Convert.ToDecimal(pBookingTransDetail.LineSeat.ToString());
                            dtTransDetail.Rows[i]["LineNameChange"] = Convert.ToDecimal(dtTransDetail.Rows[i]["LineNameChange"].ToString()) + Convert.ToDecimal(pBookingTransDetail.LineNameChange.ToString());
                            dtTransDetail.Rows[i]["LineInfant"] = Convert.ToDecimal(dtTransDetail.Rows[i]["LineInfant"].ToString()) + Convert.ToDecimal(pBookingTransDetail.LineInfant.ToString());
                            dtTransDetail.Rows[i]["LineDisc"] = Convert.ToDecimal(dtTransDetail.Rows[i]["LineDisc"].ToString()) + Convert.ToDecimal(pBookingTransDetail.LineDisc.ToString());
                            dtTransDetail.Rows[i]["LinePromoDisc"] = Convert.ToDecimal(dtTransDetail.Rows[i]["LinePromoDisc"].ToString()) + Convert.ToDecimal(pBookingTransDetail.LinePromoDisc.ToString());
                            dtTransDetail.Rows[i]["LineTax"] = Convert.ToDecimal(dtTransDetail.Rows[i]["LineTax"].ToString()) + Convert.ToDecimal(pBookingTransDetail.LineTax.ToString());
                            dtTransDetail.Rows[i]["LinePaxFee"] = Convert.ToDecimal(dtTransDetail.Rows[i]["LinePaxFee"].ToString()) + Convert.ToDecimal(pBookingTransDetail.LinePaxFee.ToString());

                            totalPax = Convert.ToInt16(dtTransDetail.Rows[i]["PaxAdult"].ToString()) + Convert.ToInt16(dtTransDetail.Rows[i]["PaxChild"].ToString());
                            totalTransAmount += Convert.ToDecimal(dtTransDetail.Rows[i]["LineTotal"].ToString());
                            totalTransTotalFee += Convert.ToDecimal(dtTransDetail.Rows[i]["LineFee"].ToString());
                            totalTransTotalOth += Convert.ToDecimal(dtTransDetail.Rows[i]["LineOth"].ToString());
                            totalTransTotalProcess += Convert.ToDecimal(dtTransDetail.Rows[i]["LineProcess"].ToString());
                            totalTransTotalSSR += Convert.ToDecimal(dtTransDetail.Rows[i]["LineSSR"].ToString());
                            totalTransTotalSeat += Convert.ToDecimal(dtTransDetail.Rows[i]["LineSeat"].ToString());
                            totalTransTotalNameChange += Convert.ToDecimal(dtTransDetail.Rows[i]["LineNameChange"].ToString());
                            totalTransTotalInfant += Convert.ToDecimal(dtTransDetail.Rows[i]["LineInfant"].ToString());
                            totalTransTotalDisc += Convert.ToDecimal(dtTransDetail.Rows[i]["LineDisc"].ToString());
                            totalTransTotalPromoDisc += Convert.ToDecimal(dtTransDetail.Rows[i]["LinePromoDisc"].ToString());
                            totalTransTotalTax += Convert.ToDecimal(dtTransDetail.Rows[i]["LineTax"].ToString());
                            totalTransTotalPaxFee += Convert.ToDecimal(dtTransDetail.Rows[i]["LinePaxFee"].ToString());
                            totalTransSubTotal += Convert.ToDecimal(dtTransDetail.Rows[i]["LineTotal"].ToString());

                            totalTransAmountAll += Convert.ToDecimal(dtTransDetail.Rows[i]["LineTotal"].ToString());
                            cnt = i;
                        }
                        HttpContext.Current.Session["TransDetailAll"] = dtTransDetail;
                    }

                    #region "Payment"
                    listBookingDetail = lstbookDTLInfo;
                    List<BookingTransactionDetail> listBookingDetailCombine = new List<BookingTransactionDetail>();
                    listBookingDetailCombine = (List<BookingTransactionDetail>)Session["listBookingDetailCombine"];
                    foreach (BookingTransactionDetail objBK_TRANSDTL_Info in listBookingDetailCombine)
                    {
                        objBK_TRANSDTL_Info.RecordLocator = listBookingDetail.Min(item => item.RecordLocator);
                        objBK_TRANSDTL_Info.CarrierCode = listBookingDetail.Min(item => item.CarrierCode);
                        objBK_TRANSDTL_Info.FlightNo = listBookingDetail.Min(item => item.FlightNo);
                        objBK_TRANSDTL_Info.DepatureDate = listBookingDetail.Min(item => item.DepatureDate);
                        objBK_TRANSDTL_Info.Origin = listBookingDetail.Min(item => item.Origin);
                        objBK_TRANSDTL_Info.Destination = listBookingDetail.Min(item => item.Destination);
                        objBK_TRANSDTL_Info.Currency = listBookingDetail.Min(item => item.Currency);
                        objBK_TRANSDTL_Info.ArrivalDate = listBookingDetail.Min(item => item.ArrivalDate);
                        objBK_TRANSDTL_Info.Transit = listBookingDetail.Min(item => item.Transit);
                        objBK_TRANSDTL_Info.DepatureDate2 = listBookingDetail.Min(item => item.DepatureDate2);
                        objBK_TRANSDTL_Info.ArrivalDate2 = listBookingDetail.Min(item => item.ArrivalDate2);
                        objBK_TRANSDTL_Info.SellKey = listBookingDetail.Min(item => item.SellKey);
                        objBK_TRANSDTL_Info.Signature = listBookingDetail[0].Signature;
                        objBK_TRANSDTL_Info.PaxChild = listBookingDetail.Max(item => item.PaxChild);
                        objBK_TRANSDTL_Info.PaxAdult = listBookingDetail.Max(item => item.PaxAdult);
                        objBK_TRANSDTL_Info.FarePerPax = listBookingDetail.Sum(item => item.FarePerPax);
                        objBK_TRANSDTL_Info.LineTotal = listBookingDetail.Sum(item => item.LineTotal);
                        objBK_TRANSDTL_Info.LineTax = listBookingDetail.Sum(item => item.LineTax);
                        objBK_TRANSDTL_Info.LinePaxFee = listBookingDetail.Sum(item => item.LinePaxFee);
                        objBK_TRANSDTL_Info.LineFee = listBookingDetail.Sum(item => item.LineFee);
                        objBK_TRANSDTL_Info.LineOth = listBookingDetail.Sum(item => item.LineOth);
                        objBK_TRANSDTL_Info.LineProcess = listBookingDetail.Sum(item => item.LineProcess);
                        objBK_TRANSDTL_Info.LineSSR = listBookingDetail.Sum(item => item.LineSSR);
                        objBK_TRANSDTL_Info.LineSeat = listBookingDetail.Sum(item => item.LineSeat);
                        objBK_TRANSDTL_Info.LineNameChange = listBookingDetail.Sum(item => item.LineNameChange);
                        objBK_TRANSDTL_Info.LineInfant = listBookingDetail.Sum(item => item.LineInfant);
                        objBK_TRANSDTL_Info.LineDisc = listBookingDetail.Sum(item => item.LineDisc);
                        objBK_TRANSDTL_Info.LinePromoDisc = listBookingDetail.Sum(item => item.LinePromoDisc);
                        objBK_TRANSDTL_Info.NextDueDate = DateTime.Now.AddMinutes(10);
                        objBK_TRANSDTL_Info.CollectedAmount = objBK_TRANSDTL_Info.LineTotal - book.BookingSum.BalanceDue;
                        objBK_TRANSDTL_Info.PayDueAmount1 = listBookingDetail.Min(item => item.PayDueAmount1);
                        objBK_TRANSDTL_Info.PayDueAmount2 = listBookingDetail.Min(item => item.PayDueAmount2);
                        objBK_TRANSDTL_Info.PayDueAmount3 = listBookingDetail.Min(item => item.PayDueAmount3);
                        objBK_TRANSDTL_Info.PayDueDate1 = listBookingDetail.Min(item => item.PayDueDate1);
                        objBK_TRANSDTL_Info.PayDueDate2 = listBookingDetail.Min(item => item.PayDueDate2);
                        objBK_TRANSDTL_Info.PayDueDate3 = listBookingDetail.Min(item => item.PayDueDate3);
                        HttpContext.Current.Session.Remove("BookingDetailCombine");
                        HttpContext.Current.Session.Add("BookingDetailCombine", objBK_TRANSDTL_Info);
                    }


                    HttpContext.Current.Session.Remove("ChglstbookDTLInfo");
                    HttpContext.Current.Session.Add("ChglstbookDTLInfo", listBookingDetail);

                    HttpContext.Current.Session.Remove("listBookingDetailCombine");
                    HttpContext.Current.Session.Add("listBookingDetailCombine", listBookingDetailCombine);


                    objBooking.FillDataTableTransDetail(listBookingDetailCombine);
                    objBooking.FillChgTransDetail(listBookingDetailCombine);

                    HttpContext.Current.Session.Remove("ChgbookHDRInfo");
                    HttpContext.Current.Session.Add("ChgbookHDRInfo", bookHDRInfo);
                    objBooking.FillChgTransMain(bookHDRInfo);

                    //objBooking.FillDataTableTransMain(bookingMain);

                }

                return true;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                return false;
            }
            finally
            {

            }
        }

        protected void ClearSSRFeeValue()
        {
            DataTable dataClass = (DataTable)HttpContext.Current.Session["dataClassTrans"];
            if (dataClass != null)
            {
                for (int j = 0; j < dataClass.Rows.Count; j++)
                {
                    dataClass.Rows[j]["FullPrice"] = Convert.ToDecimal(dataClass.Rows[j]["FullPrice"].ToString()) - Convert.ToDecimal(dataClass.Rows[j]["SSRChrg"].ToString());
                    dataClass.Rows[j]["SSRChrg"] = 0;
                }
            }
            HttpContext.Current.Session["dataClassTrans"] = dataClass;
        }

        protected void UpdateTotalAmount(decimal TotalSSRDepart, decimal TotalSSRReturn, ref decimal TotalAmountGoing, ref decimal TotalAmountReturn, int TotalPax, int PaxAdt, int PaxChd)
        {

            HttpCookie cookie = Request.Cookies["cookieTemFlight"];
            if (cookie != null)
            {
                if (IsNumeric(cookie.Values["list1ID"]))
                {
                    departID = Convert.ToInt32(cookie.Values["list1ID"]);
                }
                else
                {
                    departID = -1;
                }

                ReturnID = cookie.Values["ReturnID"];
                num = Convert.ToInt32(cookie.Values["PaxNum"]);
            }

            //update fee
            string strExpr;
            string strSort;
            DataTable dt = new DataTable();
            DataRow[] foundRows;
            object TotalBaggage, TotalMeal, TotalMeal1, TotalSport, TotalComfort, TotalDuty, TotalInfant;
            //depart
            decimal TotalAmount;
            if (Session["dataBDFeeDepart"] != null)
            {
                DataTable dtBDFee = objBooking.dtBreakdownFee();
                dtBDFee = (DataTable)HttpContext.Current.Session["dataBDFeeDepart"];

                if (Session["dtGridPass"] != null)
                {
                    DataTable dtPass = (DataTable)Session["dtGridPass"];
                    TotalBaggage = dtPass.Compute("Sum(PriceBaggage)", "");
                    TotalMeal = dtPass.Compute("Sum(PriceMeal)", "");
                    TotalMeal1 = dtPass.Compute("Sum(PriceMeal1)", "");
                    TotalSport = dtPass.Compute("Sum(PriceSport)", "");
                    TotalComfort = dtPass.Compute("Sum(PriceComfort)", "");
                    TotalDuty = dtPass.Compute("Sum(PriceDuty)", "");
                    TotalInfant = dtPass.Compute("Sum(PriceInfant)", "");

                    dtBDFee.Rows[0]["Baggage"] = Convert.ToDecimal(TotalBaggage);
                    dtBDFee.Rows[0]["Meal"] = Convert.ToDecimal(TotalMeal) + Convert.ToDecimal(TotalMeal1);
                    dtBDFee.Rows[0]["Sport"] = Convert.ToDecimal(TotalSport);
                    dtBDFee.Rows[0]["Comfort"] = Convert.ToDecimal(TotalComfort);
                    dtBDFee.Rows[0]["Infant"] = Convert.ToDecimal(TotalInfant);
                }

                dtBDFee.Rows[0]["SSR"] = Convert.ToDecimal(TotalSSRDepart);


                TotalAmount = Convert.ToDecimal(dtBDFee.Rows[0]["Fare"]) * TotalPax;
                //if (dtBDFee.Rows[0]["Infant"] != DBNull.Value)
                //{
                TotalAmount += Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * PaxAdt + Convert.ToDecimal(dtBDFee.Rows[0]["PaxFee"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Svc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) + Convert.ToDecimal(dtBDFee.Rows[0]["SSR"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Seat"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Infant"]);
                //}
                //else
                //{
                //    TotalAmount += Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * PaxAdt + Convert.ToDecimal(dtBDFee.Rows[0]["PaxFee"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Svc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) + Convert.ToDecimal(dtBDFee.Rows[0]["SSR"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Seat"]);
                //}
                if (Convert.ToInt32(PaxAdt.ToString()) != 0)
                {
                    TotalAmount += Convert.ToDecimal(dtBDFee.Rows[0]["ChApt"]) * PaxChd;
                }
                TotalAmountGoing = TotalAmount;
                HttpContext.Current.Session["dataBDFeeDepart"] = dtBDFee;

                strExpr = "TemFlightId = '" + departID + "'";
                strSort = "";
                dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                foundRows[0]["TemFlightTotalAmount"] = (TotalAmount);
                HttpContext.Current.Session["TempFlight"] = dt;
            }
            //return
            if (Session["dataBDFeeReturn"] != null)
            {
                DataTable dtBDFee = objBooking.dtBreakdownFee();
                dtBDFee = (DataTable)HttpContext.Current.Session["dataBDFeeReturn"];

                if (Session["dtGridPass2"] != null)
                {
                    DataTable dtPass = (DataTable)Session["dtGridPass2"];
                    TotalBaggage = dtPass.Compute("Sum(PriceBaggage)", "");
                    TotalMeal = dtPass.Compute("Sum(PriceMeal)", "");
                    TotalMeal1 = dtPass.Compute("Sum(PriceMeal1)", "");
                    TotalSport = dtPass.Compute("Sum(PriceSport)", "");
                    TotalComfort = dtPass.Compute("Sum(PriceComfort)", "");
                    TotalDuty = dtPass.Compute("Sum(PriceDuty)", "");
                    TotalInfant = dtPass.Compute("Sum(PriceInfant)", "");

                    dtBDFee.Rows[0]["Baggage"] = Convert.ToDecimal(TotalBaggage);
                    dtBDFee.Rows[0]["Meal"] = Convert.ToDecimal(TotalMeal) + Convert.ToDecimal(TotalMeal1);
                    dtBDFee.Rows[0]["Sport"] = Convert.ToDecimal(TotalSport);
                    dtBDFee.Rows[0]["Comfort"] = Convert.ToDecimal(TotalComfort);
                    dtBDFee.Rows[0]["Infant"] = Convert.ToDecimal(TotalInfant);
                }

                dtBDFee.Rows[0]["SSR"] = Convert.ToDecimal(TotalSSRReturn);

                TotalAmount = Convert.ToDecimal(dtBDFee.Rows[0]["Fare"]) * TotalPax;
                //if (dtBDFee.Rows[0]["Infant"] != DBNull.Value)
                //{
                TotalAmount += Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * PaxAdt + Convert.ToDecimal(dtBDFee.Rows[0]["PaxFee"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Svc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) + Convert.ToDecimal(dtBDFee.Rows[0]["SSR"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Seat"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Infant"]);
                //}
                //else
                //{
                //    TotalAmount += Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * PaxAdt + Convert.ToDecimal(dtBDFee.Rows[0]["PaxFee"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Svc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) + Convert.ToDecimal(dtBDFee.Rows[0]["SSR"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Seat"]);
                //}
                if (Convert.ToInt32(PaxChd) != 0)
                {
                    TotalAmount += Convert.ToDecimal(dtBDFee.Rows[0]["ChApt"]) * PaxChd;
                }
                TotalAmountReturn = TotalAmount;
                HttpContext.Current.Session["dataBDFeeReturn"] = dtBDFee;

                strExpr = "TemFlightId = '" + ReturnID + "'";
                strSort = "";
                dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                foundRows[0]["TemFlightTotalAmount"] = (TotalAmount);
                HttpContext.Current.Session["TempFlight"] = dt;
            }
            //update fee

        }

        private void BindModel()
        {
            //added by ketee, check cookie departID, 20170119
            HttpCookie cookie = Request.Cookies["cookieTemFlight"];
            if (cookie != null)
            {
                if (IsNumeric(cookie.Values["list1ID"]))
                {
                    departID = Convert.ToInt32(cookie.Values["list1ID"]);
                }
                else
                {
                    departID = -1;
                }

                ReturnID = cookie.Values["ReturnID"];
                num = Convert.ToInt32(cookie.Values["PaxNum"]);
            }

            if (departID != -1)
            {
                model = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
                ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
                string strExpr;
                string strSort;
                DataTable dt = objBooking.dtFlight();
                strExpr = "TemFlightId = '" + departID + "'";
                strSort = "";
                dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                FillModelFromDataRow(foundRows, ref model);

                decimal total = temFlight.TemFlightTotalAmount;

                if (ReturnID != "")
                {
                    model2 = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
                    strExpr = "TemFlightId = '" + ReturnID + "'";
                    strSort = "";
                    dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                    foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                    FillModelFromDataRow(foundRows, ref model2);

                    total += temFlight.TemFlightTotalAmount;
                }

                //20170530 - Sienny (put amount due to session)
                if (Session["TotalAmountDue"] != null)
                {
                    Session["TotalAmountDue"] = objGeneral.RoundUp(Convert.ToDecimal(Session["TotalAmountDue"].ToString()) + total).ToString("N", nfi);
                }
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

            DataTable dataPass = new DataTable();
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;
            try
            {
                if (infantmax > 10) infantmax = 10;
                DataTable data = new DataTable();
                DataTable data2 = new DataTable();
                if (Session["dtGridPass"] != null)
                {
                    data = Session["dtGridPass"] as DataTable;
                    data.PrimaryKey = new DataColumn[] { (data.Columns["PassengerID"]), (data.Columns["SeqNo"]) };
                }
                if (Session["dtGridPass2"] != null)
                {
                    data2 = Session["dtGridPass2"] as DataTable;
                    data2.PrimaryKey = new DataColumn[] { (data2.Columns["PassengerID"]), (data2.Columns["SeqNo"]) };
                }

                foreach (var args in e.UpdateValues)
                {

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

                    int countInfant = Convert.ToInt16(data.Compute("Count(Infant)", "Infant <> ''"));

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
                            if (Request.QueryString["change"] == null)
                            {
                                using (profiler.Step("AssignValues"))
                                {
                                    AssignValues(row, row2, args.NewValues[SSRColumns[p]], ref SSRColumn, "Depart");
                                }
                            }
                            else
                            {
                                using (profiler.Step("AssignFlightChange"))
                                {
                                    AssignValuesFlightChange(row, row2, args.NewValues[SSRColumns[p]], ref SSRColumn, "Depart", gvPassenger);
                                }
                            }
                        }
                    }

                    string[] SSRColumnMeal = new string[] { "Meal", "Meal1" };
                    for (int p = 0; p < SSRColumnMeal.Length; p++)
                    {
                        string SSRColumn = SSRColumnMeal[p].ToString();
                        if (p == 0)
                        {
                            ssrvalue = args.NewValues["Drink"];
                            ssr = "Drink";
                        }
                        else
                        {
                            ssrvalue = args.NewValues["Drink1"];
                            ssr = "Drink1";
                        }
                        if (Request.QueryString["change"] == null)
                        {
                            using (profiler.Step("AssignValueMeal"))
                            {
                                AssignValueMeal(row, args.NewValues[SSRColumnMeal[p]], ref SSRColumn, ssrvalue, ref ssr, "Depart");
                            }
                        }
                        else
                        {
                            using (profiler.Step("AssignMealFlightChange"))
                            {
                                AssignValueMealFlightChange(row, args.NewValues[SSRColumnMeal[p]], ref SSRColumn, ssrvalue, ref ssr, "Depart", gvPassenger);
                            }
                        }
                    }

                    string[] SSRColumnDrink = new string[] { "Drink", "Drink1" };
                    for (int p = 0; p < SSRColumnDrink.Length; p++)
                    {
                        string SSRColumn = SSRColumnDrink[p].ToString();
                        if (p == 0)
                        {
                            ssrvalue = args.NewValues["Meal"];
                            ssr = "Meal";
                        }
                        else
                        {
                            ssrvalue = args.NewValues["Meal1"];
                            ssr = "Meal1";
                        }

                        if (Request.QueryString["change"] == null)
                        {
                            using (profiler.Step("AssignValueDrink"))
                            {
                                AssignValueDrink(row, args.NewValues[SSRColumnDrink[p]], ref SSRColumn, ssrvalue, ref ssr, "Depart");
                            }
                        }
                        else
                        {
                            using (profiler.Step("AssignDrinkFlightChange"))
                            {
                                AssignValueDrinkFlightChange(row, args.NewValues[SSRColumnDrink[p]], ref SSRColumn, ssrvalue, ref ssr, "Depart", gvPassenger);
                            }
                        }
                    }

                    string[] SSRColumns2 = new string[] { "Comfort", "Duty" };
                    for (int p = 0; p < SSRColumns2.Length; p++)
                    {
                        string SSRColumn = SSRColumns2[p].ToString();
                        if (Request.QueryString["change"] == null)
                        {
                            using (profiler.Step("AssignValue2"))
                            {
                                AssignValue2(row, args.NewValues[SSRColumns2[p]], ref SSRColumn, "Depart");
                            }
                        }
                        else
                        {
                            using (profiler.Step("AssignValue2FlightChange"))
                            {
                                AssignValue2FlightChange(row, args.NewValues[SSRColumns2[p]], ref SSRColumn, "Depart", gvPassenger);
                            }
                        }
                    }

                }

                if (Session["InfantMax"] == null)
                {
                    Session["dtGridPass"] = data;
                    if (data2 != null && data2.Rows.Count > 0)
                    {
                        Session["dtGridPass2"] = data2;
                    }
                }
                else
                {
                    Session["dtGridPass"] = datas;
                    if (datas2 != null && datas2.Rows.Count > 0)
                    {
                        Session["dtGridPass2"] = datas2;
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
                gvPassenger.DataSource = Session["dtGridPass"];
                gvPassenger.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "Script", "gvPassenger_EndCallback();", true);

            }
        }

        protected void gvPassenger2_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            MessageList msgList = new MessageList();
            string ssr = "";
            object ssrvalue;
            try
            {
                if (infantmax > 10) infantmax = 10;

                DataTable data = Session["dtGridPass2"] as DataTable;
                data.PrimaryKey = new DataColumn[] { (data.Columns["PassengerID"]), (data.Columns["SeqNo"]) };
                DataTable data1 = Session["dtGridPass"] as DataTable;
                data1.PrimaryKey = new DataColumn[] { (data1.Columns["PassengerID"]), (data1.Columns["SeqNo"]) };
                foreach (var args in e.UpdateValues)
                {

                    // Create an array for the key values to find.
                    object[] findTheseVals = new object[2];

                    // Set the values of the keys to find.
                    findTheseVals[0] = (args.Keys["PassengerID"]);
                    findTheseVals[1] = (args.Keys["SeqNo"]);

                    DataRow row = data.Rows.Find(findTheseVals);
                    DataRow row1 = data1.Rows.Find(findTheseVals);
                    row["PassengerID"] = args.Keys["PassengerID"];
                    row["SeqNo"] = args.Keys["SeqNo"];

                    int countInfant = Convert.ToInt16(data.Compute("Count(Infant)", "Infant <> ''"));

                    string[] SSRColumns = new string[] { "Baggage", "Sport", "Infant" };
                    for (int p = 0; p < SSRColumns.Length; p++)
                    {
                        string SSRColumn = SSRColumns[p].ToString();
                        if (args.NewValues[SSRColumns[p]] != null && args.NewValues[SSRColumns[p]].ToString() != "" && SSRColumn == "Infant" && (countInfant == infantmax) && args.OldValues[SSRColumns[p]].ToString() == "")
                        {
                            custommessage = "";
                            custommessage = msgList.Err100051.Replace("infantmax", infantmax.ToString());
                            gvPassenger2.JSProperties["cp_result"] = custommessage;
                            //gvPassenger2.JSProperties["cp_result"] = "Maximum Infant is " + infantmax + ". " + msgList.Err100051;
                        }
                        else
                        {
                            if (Request.QueryString["change"] == null)
                            {
                                AssignValues(row, row1, args.NewValues[SSRColumns[p]], ref SSRColumn, "Return");
                            }
                            else
                            {
                                AssignValuesFlightChange(row, row1, args.NewValues[SSRColumns[p]], ref SSRColumn, "Return", gvPassenger2);
                            }
                        }
                    }

                    string[] SSRColumnMeal = new string[] { "Meal", "Meal1" };
                    for (int p = 0; p < SSRColumnMeal.Length; p++)
                    {
                        string SSRColumn = SSRColumnMeal[p].ToString();
                        if (p == 0)
                        {
                            ssrvalue = args.NewValues["Drink"];
                            ssr = "Drink";
                        }
                        else
                        {
                            ssrvalue = args.NewValues["Drink1"];
                            ssr = "Drink1";
                        }
                        if (Request.QueryString["change"] == null)
                        {
                            AssignValueMeal(row, args.NewValues[SSRColumnMeal[p]], ref SSRColumn, ssrvalue, ref ssr, "Return");
                        }
                        else
                        {
                            AssignValueMealFlightChange(row, args.NewValues[SSRColumnMeal[p]], ref SSRColumn, ssrvalue, ref ssr, "Return", gvPassenger2);
                        }
                    }

                    string[] SSRColumnDrink = new string[] { "Drink", "Drink1" };
                    for (int p = 0; p < SSRColumnDrink.Length; p++)
                    {
                        string SSRColumn = SSRColumnDrink[p].ToString();
                        if (p == 0)
                        {
                            ssrvalue = args.NewValues["Meal"];
                            ssr = "Meal";
                        }
                        else
                        {
                            ssrvalue = args.NewValues["Meal1"];
                            ssr = "Meal1";
                        }

                        if (Request.QueryString["change"] == null)
                        {
                            AssignValueDrink(row, args.NewValues[SSRColumnDrink[p]], ref SSRColumn, ssrvalue, ref ssr, "Return");
                        }
                        else
                        {
                            AssignValueDrinkFlightChange(row, args.NewValues[SSRColumnDrink[p]], ref SSRColumn, ssrvalue, ref ssr, "Return", gvPassenger2);
                        }
                    }

                    string[] SSRColumns2 = new string[] { "Comfort", "Duty" };
                    for (int p = 0; p < SSRColumns2.Length; p++)
                    {
                        string SSRColumn = SSRColumns2[p].ToString();
                        if (Request.QueryString["change"] == null)
                        {
                            AssignValue2(row, args.NewValues[SSRColumns2[p]], ref SSRColumn, "Return");
                        }
                        else
                        {
                            AssignValue2FlightChange(row, args.NewValues[SSRColumns2[p]], ref SSRColumn, "Return", gvPassenger2);
                        }
                    }


                }

                Session["dtGridPass2"] = data;
                Session["dtGridPass"] = data1;

                e.Handled = true;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
            finally
            {
                gvPassenger2.DataSource = Session["dtGridPass2"];
                gvPassenger2.DataBind();
            }
        }

        protected void AssignValues(DataRow row, DataRow row2, object NewValues, ref string SSRColumns, string Flight)
        {
            String Detail = "";
            String subvalue = "";
            if (NewValues == null || NewValues == "")
            {
                row[SSRColumns] = string.Empty;
                row["Price" + SSRColumns] = 0.00;
                row["Price" + SSRColumns + "S1"] = 0.00;
                row["Price" + SSRColumns + "S2"] = 0.00;
                row["SSRCode" + SSRColumns] = string.Empty;

                if (SSRColumns == "Infant" && row2.ItemArray.Length > 0)
                {
                    row2[SSRColumns] = string.Empty;
                    row2["Price" + SSRColumns] = 0.00;
                    row2["Price" + SSRColumns + "S1"] = 0.00;
                    row2["Price" + SSRColumns + "S2"] = 0.00;
                    row2["SSRCode" + SSRColumns] = string.Empty;
                    if (Flight == "Depart") gvPassenger.JSProperties["cp_result"] = "Infant";
                    else gvPassenger2.JSProperties["cp_result"] = "Infant";
                }
            }
            else
            {
                row["SSRCode" + SSRColumns] = NewValues;
                DataTable dtSSR = Session["dt" + SSRColumns + Flight] as DataTable;
                DataTable dtInfantDepart = Session["dtInfantDepart"] as DataTable;
                DataTable dtInfantReturn = Session["dtInfantReturn"] as DataTable;

                if (NewValues.ToString().Length == 4)
                {
                    DataRow[] resultSSR = dtSSR.Select("SSRCode = '" + NewValues + "'");
                    foreach (DataRow rows in resultSSR)
                    {
                        row[SSRColumns] = rows["ConcatenatedField"];
                        Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                        row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                        row["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows[3]);
                        row["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows[4]);
                        row["SSRCode" + SSRColumns] = rows["SSRCode"];


                    }

                    if (NewValues.ToString() == "INFT" && row2.ItemArray.Length > 0)
                    {
                        if (Flight == "Depart")
                        {
                            DataRow[] resultInfantDepart = dtInfantDepart.Select("SSRCode = '" + NewValues + "'");
                            foreach (DataRow rows in resultInfantDepart)
                            {
                                row[SSRColumns] = rows["ConcatenatedField"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                row["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows[3]);
                                row["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows[4]);
                                row["SSRCode" + SSRColumns] = rows["SSRCode"];
                                //gvPassenger.JSProperties["cp_result"] = "Infant";

                            }

                            DataRow[] resultInfantReturn = dtInfantReturn.Select("SSRCode = '" + NewValues + "'");
                            foreach (DataRow rows in resultInfantReturn)
                            {
                                row2[SSRColumns] = rows["ConcatenatedField"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row2["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                row2["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows[3]);
                                row2["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows[4]);
                                row2["SSRCode" + SSRColumns] = rows["SSRCode"];
                                gvPassenger.JSProperties["cp_result"] = "Infant";
                            }
                        }
                        else
                        {
                            DataRow[] resultInfantReturn = dtInfantReturn.Select("SSRCode = '" + NewValues + "'");
                            foreach (DataRow rows in resultInfantReturn)
                            {
                                row[SSRColumns] = rows["ConcatenatedField"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                row["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows[3]);
                                row["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows[4]);
                                row["SSRCode" + SSRColumns] = rows["SSRCode"];
                                //gvPassenger2.JSProperties["cp_result"] = "Infant";

                            }

                            DataRow[] resultInfantDepart = dtInfantDepart.Select("SSRCode = '" + NewValues + "'");
                            foreach (DataRow rows in resultInfantDepart)
                            {
                                row2[SSRColumns] = rows["ConcatenatedField"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row2["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                row2["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows[3]);
                                row2["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows[4]);
                                row2["SSRCode" + SSRColumns] = rows["SSRCode"];
                                gvPassenger2.JSProperties["cp_result"] = "Infant";
                            }
                        }
                    }
                }
                else
                {
                    if (SSRColumns == "Baggage") subvalue = NewValues.ToString().Substring(0, 19);
                    else if (SSRColumns == "Sport") subvalue = subvalue = NewValues.ToString().Substring(0, 20);
                    else if (SSRColumns == "Infant") subvalue = subvalue = NewValues.ToString().Substring(0, 6);
                    DataRow[] resultSSR = dtSSR.Select("ConcatenatedField LIKE '" + subvalue + "%'");
                    foreach (DataRow rows in resultSSR)
                    {
                        Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                        if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["Price" + SSRColumns]) && row["SSRCode" + SSRColumns].ToString().Trim() == rows[0].ToString().Trim())
                        {
                            continue;
                            //return;
                        }
                        else
                        {
                            row[SSRColumns] = rows["ConcatenatedField"];
                            row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                            row["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows[3]);
                            row["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows[4]);
                            row["SSRCode" + SSRColumns] = rows["SSRCode"];

                            if (NewValues.ToString() == "INFT" && row2.ItemArray.Length > 0)
                            {
                                if (Flight == "Depart")
                                {
                                    DataRow[] resultInfantDepart = dtInfantDepart.Select("SSRCode = '" + NewValues + "'");
                                    foreach (DataRow rows1 in resultInfantDepart)
                                    {
                                        row[SSRColumns] = rows1["ConcatenatedField"];
                                        Detail = rows1[2].ToString().Substring(0, rows1[2].ToString().Length - 4);
                                        row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                        row["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows1[3]);
                                        row["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows1[4]);
                                        row["SSRCode" + SSRColumns] = rows1["SSRCode"];
                                        //gvPassenger.JSProperties["cp_result"] = "Infant";

                                    }

                                    DataRow[] resultInfantReturn = dtInfantReturn.Select("SSRCode = '" + NewValues + "'");
                                    foreach (DataRow rows2 in resultInfantReturn)
                                    {
                                        row2[SSRColumns] = rows2["ConcatenatedField"];
                                        Detail = rows2[2].ToString().Substring(0, rows2[2].ToString().Length - 4);
                                        row2["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                        row2["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows2[3]);
                                        row2["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows2[4]);
                                        row2["SSRCode" + SSRColumns] = rows2["SSRCode"];
                                        gvPassenger.JSProperties["cp_result"] = "Infant";
                                    }
                                }
                                else
                                {
                                    DataRow[] resultInfantReturn = dtInfantReturn.Select("SSRCode = '" + NewValues + "'");
                                    foreach (DataRow rows1 in resultInfantReturn)
                                    {
                                        row[SSRColumns] = rows1["ConcatenatedField"];
                                        Detail = rows[2].ToString().Substring(0, rows1[2].ToString().Length - 4);
                                        row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                        row["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows1[3]);
                                        row["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows1[4]);
                                        row["SSRCode" + SSRColumns] = rows1["SSRCode"];
                                        //gvPassenger2.JSProperties["cp_result"] = "Infant";

                                    }

                                    DataRow[] resultInfantDepart = dtInfantDepart.Select("SSRCode = '" + NewValues + "'");
                                    foreach (DataRow rows2 in resultInfantDepart)
                                    {
                                        row2[SSRColumns] = rows2["ConcatenatedField"];
                                        Detail = rows2[2].ToString().Substring(0, rows2[2].ToString().Length - 4);
                                        row2["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                        row2["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows2[3]);
                                        row2["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows2[4]);
                                        row2["SSRCode" + SSRColumns] = rows2["SSRCode"];
                                        gvPassenger2.JSProperties["cp_result"] = "Infant";
                                    }
                                }
                            }
                        }
                    }
                }
            }

            SSRColumns = "";
        }

        protected void AssignValuesFlightChange(DataRow row, DataRow row2, object NewValues, ref string SSRColumns, string Flight, ASPxGridView grid)
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
                    if (row[SSRColumns] != null && row[SSRColumns].ToString() != "")
                    {
                        DataRow[] resultBaggage = dtBaggage.Select("SSRCode = '" + NewValues + "'");
                        foreach (DataRow rows in resultBaggage)
                        {
                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                            if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["Price" + SSRColumns]) && row["SSRCode" + SSRColumns].ToString().Trim() == rows[0].ToString().Trim())
                            {
                                continue;
                                //return;
                            }
                            else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["Price" + SSRColumns]))
                            {
                                custommessage = "";
                                custommessage = msgList.Err800008.Replace("SSRColumn", SSRColumns);
                                grid.JSProperties["cp_result"] = custommessage;//"Cannot replace " + SSRColumns + " with cheaper fee, please try again";
                                count = 1;
                                //return;
                            }
                            else
                            {

                                row[SSRColumns] = rows["ConcatenatedField"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                row["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows[3]);
                                row["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows[4]);
                                row["SSRCode" + SSRColumns] = rows["SSRCode"];

                                if (NewValues.ToString() == "INFT" && row2.ItemArray.Length > 0)
                                {
                                    if (Flight == "Depart")
                                    {
                                        DataRow[] resultInfantDepart = dtInfantDepart.Select("SSRCode = '" + NewValues + "'");
                                        foreach (DataRow rows1 in resultInfantDepart)
                                        {
                                            row[SSRColumns] = rows1["ConcatenatedField"];
                                            Detail = rows1[2].ToString().Substring(0, rows1[2].ToString().Length - 4);
                                            row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                            row["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows1[3]);
                                            row["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows1[4]);
                                            row["SSRCode" + SSRColumns] = rows1["SSRCode"];
                                            //gvPassenger.JSProperties["cp_result"] = "Infant";

                                        }

                                        DataRow[] resultInfantReturn = dtInfantReturn.Select("SSRCode = '" + NewValues + "'");
                                        foreach (DataRow rows2 in resultInfantReturn)
                                        {
                                            row2[SSRColumns] = rows2["ConcatenatedField"];
                                            Detail = rows2[2].ToString().Substring(0, rows2[2].ToString().Length - 4);
                                            row2["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                            row2["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows2[3]);
                                            row2["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows2[4]);
                                            row2["SSRCode" + SSRColumns] = rows2["SSRCode"];
                                            gvPassenger.JSProperties["cp_result"] = "Infant";
                                        }
                                    }
                                    else
                                    {
                                        DataRow[] resultInfantReturn = dtInfantReturn.Select("SSRCode = '" + NewValues + "'");
                                        foreach (DataRow rows1 in resultInfantReturn)
                                        {
                                            row[SSRColumns] = rows["ConcatenatedField"];
                                            Detail = rows1[2].ToString().Substring(0, rows1[2].ToString().Length - 4);
                                            row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                            row["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows1[3]);
                                            row["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows1[4]);
                                            row["SSRCode" + SSRColumns] = rows1["SSRCode"];
                                            //gvPassenger2.JSProperties["cp_result"] = "Infant";

                                        }

                                        DataRow[] resultInfantDepart = dtInfantDepart.Select("SSRCode = '" + NewValues + "'");
                                        foreach (DataRow rows2 in resultInfantDepart)
                                        {
                                            row2[SSRColumns] = rows["ConcatenatedField"];
                                            Detail = rows2[2].ToString().Substring(0, rows2[2].ToString().Length - 4);
                                            row2["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                            row2["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows2[3]);
                                            row2["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows2[4]);
                                            row2["SSRCode" + SSRColumns] = rows2["SSRCode"];
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
                            row[SSRColumns] = rows["ConcatenatedField"];
                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                            row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                            row["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows[3]);
                            row["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows[4]);
                            row["SSRCode" + SSRColumns] = rows["SSRCode"];

                            if (NewValues.ToString() == "INFT" && row2.ItemArray.Length > 0)
                            {
                                if (Flight == "Depart")
                                {
                                    DataRow[] resultInfantDepart = dtInfantDepart.Select("SSRCode = '" + NewValues + "'");
                                    foreach (DataRow rows1 in resultInfantDepart)
                                    {
                                        row[SSRColumns] = rows1["ConcatenatedField"];
                                        Detail = rows1[2].ToString().Substring(0, rows1[2].ToString().Length - 4);
                                        row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                        row["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows1[3]);
                                        row["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows1[4]);
                                        row["SSRCode" + SSRColumns] = rows1["SSRCode"];
                                        //gvPassenger.JSProperties["cp_result"] = "Infant";

                                    }

                                    DataRow[] resultInfantReturn = dtInfantReturn.Select("SSRCode = '" + NewValues + "'");
                                    foreach (DataRow rows2 in resultInfantReturn)
                                    {
                                        row2[SSRColumns] = rows2["ConcatenatedField"];
                                        Detail = rows2[2].ToString().Substring(0, rows2[2].ToString().Length - 4);
                                        row2["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                        row2["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows2[3]);
                                        row2["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows2[4]);
                                        row2["SSRCode" + SSRColumns] = rows2["SSRCode"];
                                        gvPassenger.JSProperties["cp_result"] = "Infant";
                                    }
                                }
                                else
                                {
                                    DataRow[] resultInfantReturn = dtInfantReturn.Select("SSRCode = '" + NewValues + "'");
                                    foreach (DataRow rows1 in resultInfantReturn)
                                    {
                                        row[SSRColumns] = rows["ConcatenatedField"];
                                        Detail = rows1[2].ToString().Substring(0, rows1[2].ToString().Length - 4);
                                        row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                        row["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows1[3]);
                                        row["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows1[4]);
                                        row["SSRCode" + SSRColumns] = rows1["SSRCode"];
                                        //gvPassenger2.JSProperties["cp_result"] = "Infant";

                                    }

                                    DataRow[] resultInfantDepart = dtInfantDepart.Select("SSRCode = '" + NewValues + "'");
                                    foreach (DataRow rows2 in resultInfantDepart)
                                    {
                                        row2[SSRColumns] = rows["ConcatenatedField"];
                                        Detail = rows2[2].ToString().Substring(0, rows2[2].ToString().Length - 4);
                                        row2["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                        row2["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows2[3]);
                                        row2["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows2[4]);
                                        row2["SSRCode" + SSRColumns] = rows2["SSRCode"];
                                        gvPassenger2.JSProperties["cp_result"] = "Infant";
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (row[SSRColumns] != null && row[SSRColumns].ToString() != "")
                    {
                        if (SSRColumns == "Baggage") subvalue = NewValues.ToString().Substring(0, 19);
                        else if (SSRColumns == "Sport") subvalue = subvalue = NewValues.ToString().Substring(0, 20);
                        else if (SSRColumns == "Infant") subvalue = subvalue = NewValues.ToString().Substring(0, 6);
                        DataRow[] resultBaggage = dtBaggage.Select("Detail = '" + subvalue + "'");
                        foreach (DataRow rows in resultBaggage)
                        {
                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                            if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["Price" + SSRColumns]) && row["SSRCode" + SSRColumns].ToString().Trim() == rows[0].ToString().Trim())
                            {
                                continue;
                                //return;
                            }
                            else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["Price" + SSRColumns]))
                            {
                                custommessage = "";
                                custommessage = msgList.Err800008.Replace("SSRColumns", SSRColumns);
                                grid.JSProperties["cp_result"] = custommessage;//"Cannot replace " + SSRColumns + " with cheaper fee, please try again";
                                count = 1;
                                //return;
                            }
                            else
                            {
                                if (row["SSRCode" + SSRColumns].ToString().Trim() != rows["SSRCode"].ToString().Trim())
                                {
                                    row[SSRColumns] = rows["ConcatenatedField"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                    row["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows[3]);
                                    row["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows[4]);
                                    row["SSRCode" + SSRColumns] = rows["SSRCode"];

                                    if (NewValues.ToString() == "INFT" && row2.ItemArray.Length > 0)
                                    {
                                        if (Flight == "Depart")
                                        {
                                            DataRow[] resultInfantDepart = dtInfantDepart.Select("SSRCode = '" + NewValues + "'");
                                            foreach (DataRow rows1 in resultInfantDepart)
                                            {
                                                row[SSRColumns] = rows1["ConcatenatedField"];
                                                Detail = rows1[2].ToString().Substring(0, rows1[2].ToString().Length - 4);
                                                row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                                row["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows1[3]);
                                                row["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows1[4]);
                                                row["SSRCode" + SSRColumns] = rows1["SSRCode"];
                                                //gvPassenger.JSProperties["cp_result"] = "Infant";

                                            }

                                            DataRow[] resultInfantReturn = dtInfantReturn.Select("SSRCode = '" + NewValues + "'");
                                            foreach (DataRow rows2 in resultInfantReturn)
                                            {
                                                row2[SSRColumns] = rows2["ConcatenatedField"];
                                                Detail = rows2[2].ToString().Substring(0, rows2[2].ToString().Length - 4);
                                                row2["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                                row2["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows2[3]);
                                                row2["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows2[4]);
                                                row2["SSRCode" + SSRColumns] = rows2["SSRCode"];
                                                gvPassenger.JSProperties["cp_result"] = "Infant";
                                            }
                                        }
                                        else
                                        {
                                            DataRow[] resultInfantReturn = dtInfantReturn.Select("SSRCode = '" + NewValues + "'");
                                            foreach (DataRow rows1 in resultInfantReturn)
                                            {
                                                row[SSRColumns] = rows1["ConcatenatedField"];
                                                Detail = rows1[2].ToString().Substring(0, rows1[2].ToString().Length - 4);
                                                row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                                row["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows1[3]);
                                                row["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows1[4]);
                                                row["SSRCode" + SSRColumns] = rows1["SSRCode"];
                                                //gvPassenger2.JSProperties["cp_result"] = "Infant";

                                            }

                                            DataRow[] resultInfantDepart = dtInfantDepart.Select("SSRCode = '" + NewValues + "'");
                                            foreach (DataRow rows2 in resultInfantDepart)
                                            {
                                                row2[SSRColumns] = rows["ConcatenatedField"];
                                                Detail = rows2[2].ToString().Substring(0, rows2[2].ToString().Length - 4);
                                                row2["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                                row2["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows2[3]);
                                                row2["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows2[4]);
                                                row2["SSRCode" + SSRColumns] = rows2["SSRCode"];
                                                gvPassenger2.JSProperties["cp_result"] = "Infant";
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        if (SSRColumns == "Baggage") subvalue = NewValues.ToString().Substring(0, 19);
                        else if (SSRColumns == "Sport") subvalue = subvalue = NewValues.ToString().Substring(0, 20);
                        else if (SSRColumns == "Infant") subvalue = subvalue = NewValues.ToString().Substring(0, 6);
                        DataRow[] resultBaggage = dtBaggage.Select("Detail = '" + subvalue + "'");
                        foreach (DataRow rows in resultBaggage)
                        {
                            if (row["SSRCode" + SSRColumns].ToString().Trim() != rows["SSRCode"].ToString().Trim())
                            {
                                row[SSRColumns] = rows["ConcatenatedField"];
                                row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                row["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows[3]);
                                row["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows[4]);
                                row["SSRCode" + SSRColumns] = rows["SSRCode"];

                                if (NewValues.ToString() == "INFT" && row2.ItemArray.Length > 0)
                                {
                                    if (Flight == "Depart")
                                    {
                                        DataRow[] resultInfantDepart = dtInfantDepart.Select("SSRCode = '" + NewValues + "'");
                                        foreach (DataRow rows1 in resultInfantDepart)
                                        {
                                            row[SSRColumns] = rows1["ConcatenatedField"];
                                            Detail = rows1[2].ToString().Substring(0, rows1[2].ToString().Length - 4);
                                            row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                            row["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows1[3]);
                                            row["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows1[4]);
                                            row["SSRCode" + SSRColumns] = rows1["SSRCode"];
                                            //gvPassenger.JSProperties["cp_result"] = "Infant";

                                        }

                                        DataRow[] resultInfantReturn = dtInfantReturn.Select("SSRCode = '" + NewValues + "'");
                                        foreach (DataRow rows2 in resultInfantReturn)
                                        {
                                            row2[SSRColumns] = rows2["ConcatenatedField"];
                                            Detail = rows2[2].ToString().Substring(0, rows2[2].ToString().Length - 4);
                                            row2["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                            row2["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows2[3]);
                                            row2["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows2[4]);
                                            row2["SSRCode" + SSRColumns] = rows2["SSRCode"];
                                            gvPassenger.JSProperties["cp_result"] = "Infant";
                                        }
                                    }
                                    else
                                    {
                                        DataRow[] resultInfantReturn = dtInfantReturn.Select("SSRCode = '" + NewValues + "'");
                                        foreach (DataRow rows1 in resultInfantReturn)
                                        {
                                            row[SSRColumns] = rows1["ConcatenatedField"];
                                            Detail = rows1[2].ToString().Substring(0, rows1[2].ToString().Length - 4);
                                            row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                            row["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows1[3]);
                                            row["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows1[4]);
                                            row["SSRCode" + SSRColumns] = rows1["SSRCode"];
                                            //gvPassenger2.JSProperties["cp_result"] = "Infant";

                                        }

                                        DataRow[] resultInfantDepart = dtInfantDepart.Select("SSRCode = '" + NewValues + "'");
                                        foreach (DataRow rows2 in resultInfantDepart)
                                        {
                                            row2[SSRColumns] = rows["ConcatenatedField"];
                                            Detail = rows2[2].ToString().Substring(0, rows2[2].ToString().Length - 4);
                                            row2["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                            row2["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows2[3]);
                                            row2["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows2[4]);
                                            row2["SSRCode" + SSRColumns] = rows2["SSRCode"];
                                            gvPassenger2.JSProperties["cp_result"] = "Infant";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            SSRColumns = "";
        }

        protected void AssignValueMeal(DataRow row, object NewValues, ref string SSRColumns, object NewValuesDrink, ref string Drink, string Flight)
        {
            String Detail = "";
            if (NewValues == null || NewValues == "")
            {
                row[SSRColumns] = string.Empty;
                row["Price" + SSRColumns] = 0.00;
                row["SSRCode" + SSRColumns] = string.Empty;
                row[Drink] = string.Empty;
                row["Price" + Drink] = 0.00;
                row["SSRCode" + Drink] = string.Empty;
            }
            else
            {
                DataTable dtMeal = new DataTable();
                DataTable dtDrink = new DataTable();
                row["SSRCode" + SSRColumns] = NewValues;
                if (SSRColumns == "Meal1")
                {
                    dtMeal = Session["dtMeal" + Flight + "2"] as DataTable;
                    dtDrink = Session["dtDrink" + Flight + "2"] as DataTable;
                }
                else
                {
                    dtMeal = Session["dt" + SSRColumns + Flight] as DataTable;
                    dtDrink = Session["dt" + Drink + Flight] as DataTable;
                }


                if (NewValues.ToString().Length == 4)
                {
                    DataRow[] resultMeal = dtMeal.Select("SSRCode = '" + NewValues + "'");
                    foreach (DataRow rows in resultMeal)
                    {
                        row[SSRColumns] = rows["Detail"];
                        Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                        row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                        row["SSRCode" + SSRColumns] = rows["SSRCode"];
                    }

                    if (NewValuesDrink == null || NewValuesDrink == "")
                    {
                        //if (Session["defaultdrink"] != null)
                        //{
                        //DataTable DefaultDrink = (DataTable)Session["defaultdrink"];
                        for (int c = 0; c < dtDrink.Rows.Count; c++)
                        {
                            //DataRow[] resultDrink = dtDrink.Select("SSRCode LIKE '" + DefaultDrink.Rows[c]["SYSValueEx"] + "%'");
                            //if (resultDrink.Length > 0)
                            //{
                            //foreach (DataRow rows in resultDrink)
                            //{
                            row[Drink] = dtDrink.Rows[0]["ConcatenatedField"];
                            Detail = dtDrink.Rows[0]["Price"].ToString().Substring(0, dtDrink.Rows[0]["Price"].ToString().Length - 4);
                            row["Price" + Drink] = Convert.ToDecimal(Detail);
                            row["SSRCode" + Drink] = dtDrink.Rows[0]["SSRCode"];
                            //}
                            break;
                            //}
                        }
                        //}

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
                        Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                        if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["Price" + SSRColumns]) && row[SSRColumns].ToString().Trim() == rows[1].ToString().Trim())
                        {
                            continue;
                        }
                        else
                        {
                            row[SSRColumns] = rows["Detail"];
                            row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                            row["SSRCode" + SSRColumns] = rows["SSRCode"];
                        }
                    }

                    if (NewValuesDrink == null || NewValuesDrink == "")
                    {
                        //if (Session["defaultdrink"] != null)
                        //{
                        //DataTable DefaultDrink = (DataTable)Session["defaultdrink"];
                        for (int c = 0; c < dtDrink.Rows.Count; c++)
                        {
                            //DataRow[] resultDrink = dtDrink.Select("SSRCode LIKE '" + DefaultDrink.Rows[c]["SYSValueEx"] + "%'");
                            //if (resultDrink.Length > 0)
                            //{
                            //foreach (DataRow rows in resultDrink)
                            //{
                            row[Drink] = dtDrink.Rows[0]["ConcatenatedField"];
                            Detail = dtDrink.Rows[0]["Price"].ToString().Substring(0, dtDrink.Rows[0]["Price"].ToString().Length - 4);
                            row["Price" + Drink] = Convert.ToDecimal(Detail);
                            row["SSRCode" + Drink] = dtDrink.Rows[0]["SSRCode"];
                            //}
                            break;
                            //}
                        }
                        //}
                    }
                }
            }

            SSRColumns = "";
        }

        protected void AssignValueMealFlightChange(DataRow row, object NewValues, ref string SSRColumns, object NewValuesDrink, ref string Drink, string Flight, ASPxGridView grid)
        {
            MessageList msgList = new MessageList();
            int count = 0;
            String Detail = "";
            if (NewValues != null && NewValues != "")
            {

                DataTable dtMeal = new DataTable();
                DataTable dtDrink = new DataTable();
                if (SSRColumns == "Meal1")
                {
                    dtMeal = Session["dtMeal" + Flight + "2"] as DataTable;
                    dtDrink = Session["dtDrink" + Flight + "2"] as DataTable;
                }
                else
                {
                    dtMeal = Session["dt" + SSRColumns + Flight] as DataTable;
                    dtDrink = Session["dt" + Drink + Flight] as DataTable;
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
                                grid.JSProperties["cp_result"] = custommessage;//"Cannot replace " + SSRColumns + " with cheaper fee, please try again";
                                count = 1;
                                //return;
                            }
                            else
                            {
                                if (row["SSRCode" + SSRColumns].ToString().Trim() != rows["SSRCode"].ToString().Trim())
                                {
                                    row[SSRColumns] = rows["Detail"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                    row["SSRCode" + SSRColumns] = rows["SSRCode"];

                                    if (NewValuesDrink == null || NewValuesDrink == "")
                                    {
                                        for (int c = 0; c < dtDrink.Rows.Count; c++)
                                        {

                                            row[Drink] = dtDrink.Rows[0]["ConcatenatedField"];
                                            Detail = dtDrink.Rows[0]["Price"].ToString().Substring(0, dtDrink.Rows[0]["Price"].ToString().Length - 4);
                                            row["Price" + Drink] = Convert.ToDecimal(Detail);
                                            row["SSRCode" + Drink] = dtDrink.Rows[0]["SSRCode"];
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
                                grid.JSProperties["cp_result"] = custommessage;//"Cannot replace " + SSRColumns + " with cheaper fee, please try again";
                                count = 1;
                                //return;
                            }
                            else
                            {
                                if (row["SSRCode" + SSRColumns].ToString().Trim() != rows["SSRCode"].ToString().Trim())
                                {
                                    row[SSRColumns] = rows["Detail"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                    row["SSRCode" + SSRColumns] = rows["SSRCode"];

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
        protected void AssignValueDrink(DataRow row, object NewValues, ref string SSRColumns, object NewValuesMeal, ref string Meal, string Flight)
        {
            String Detail = "";
            if (NewValues == null || NewValues == "")
            {

            }
            //else if (NewValuesMeal == null || NewValuesMeal == "")
            //{
            //    //row[SSRColumns] = string.Empty;
            //    //row["Price" + SSRColumns] = 0.00;
            //    //row["SSRCode" + SSRColumns] = string.Empty;
            //    //row[Meal] = string.Empty;
            //    //row["Price" + Meal] = 0.00;
            //    //row["SSRCode" + Meal] = string.Empty;
            //    DataRow[] resultMeals = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
            //    foreach (DataRow rowz in resultMeals)
            //    {
            //        row[Meal] = rowz["Detail"];
            //        Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
            //        row["Price" + Meal] = Convert.ToDecimal(Detail);
            //        row["SSRCode" + Meal] = rowz["SSRCode"];
            //        row["Indicator" + SSRColumns] = "1";
            //    }
            //}
            else
            {
                DataTable dtMeal = new DataTable();
                DataTable dtDrink = new DataTable();
                if (SSRColumns == "Drink1")
                {
                    dtMeal = Session["dtMeal" + Flight + "2"] as DataTable;
                    dtDrink = Session["dtDrink" + Flight + "2"] as DataTable;
                }
                else
                {
                    dtMeal = Session["dt" + Meal + Flight] as DataTable;
                    dtDrink = Session["dt" + SSRColumns + Flight] as DataTable;
                }
                row["SSRCode" + SSRColumns] = NewValues;

                if (NewValues.ToString().Length == 4)
                {
                    DataRow[] resultSport = dtDrink.Select("SSRCode = '" + NewValues + "'");
                    foreach (DataRow rows in resultSport)
                    {
                        row[SSRColumns] = rows["ConcatenatedField"];
                        Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                        row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                        row["SSRCode" + SSRColumns] = rows["SSRCode"];
                    }

                    if (NewValuesMeal == null || NewValuesMeal == "")
                    {
                        DataRow[] resultMeal = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                        foreach (DataRow rows in resultMeal)
                        {
                            row[Meal] = rows["Detail"];
                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                            row["Price" + Meal] = Convert.ToDecimal(Detail);
                            row["SSRCode" + Meal] = rows["SSRCode"];
                        }
                    }

                }

                else
                {
                    DataRow[] resultMeal = dtDrink.Select("ConcatenatedField LIKE '" + NewValues.ToString().Substring(0, 10) + "%'");
                    foreach (DataRow rows in resultMeal)
                    {
                        row[SSRColumns] = rows["ConcatenatedField"];
                        Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                        row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                        row["SSRCode" + SSRColumns] = rows["SSRCode"];
                    }

                    if (NewValuesMeal == null || NewValuesMeal == "")
                    {
                        DataRow[] resultMeals = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                        foreach (DataRow rows in resultMeals)
                        {
                            row[Meal] = rows["Detail"];
                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                            row["Price" + Meal] = Convert.ToDecimal(Detail);
                            row["SSRCode" + Meal] = rows["SSRCode"];
                        }
                    }
                }

            }

            SSRColumns = "";
        }
        #endregion
        protected void AssignValueDrinkFlightChange(DataRow row, object NewValues, ref string SSRColumns, object NewValuesMeal, ref string Meal, string Flight, ASPxGridView grid)
        {
            MessageList msgList = new MessageList();
            int count = 0;
            String Detail = "";
            if (NewValues != "" && NewValues != null)
            {
                DataTable dtMeal = new DataTable();
                DataTable dtDrink = new DataTable();
                if (SSRColumns == "Drink1")
                {
                    dtMeal = Session["dtMeal" + Flight + "2"] as DataTable;
                    dtDrink = Session["dtDrink" + Flight + "2"] as DataTable;
                }
                else
                {
                    dtMeal = Session["dt" + Meal + Flight] as DataTable;
                    dtDrink = Session["dt" + SSRColumns + Flight] as DataTable;
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
                                grid.JSProperties["cp_result"] = custommessage;//"Cannot replace " + SSRColumns + " with cheaper fee, please try again";
                                count = 1;
                                //return;
                            }
                            else
                            {
                                if (row["SSRCode" + SSRColumns].ToString().Trim() != rows["SSRCode"].ToString().Trim())
                                {
                                    row[SSRColumns] = rows["ConcatenatedField"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                    row["SSRCode" + SSRColumns] = rows["SSRCode"];

                                    if (NewValuesMeal == "" || NewValuesMeal == null)
                                    {
                                        DataRow[] resultMeals = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                                        foreach (DataRow rowz in resultMeals)
                                        {
                                            row[Meal] = rowz["Detail"];
                                            Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                            row["Price" + Meal] = Convert.ToDecimal(Detail);
                                            row["SSRCode" + Meal] = rowz["SSRCode"];
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        DataRow[] resultSport = dtDrink.Select("SSRCode = '" + NewValuesMeal + "'");
                        foreach (DataRow rows in resultSport)
                        {
                            if (row["SSRCode" + SSRColumns].ToString().Trim() != rows["SSRCode"].ToString().Trim())
                            {
                                row[SSRColumns] = rows["ConcatenatedField"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                row["SSRCode" + SSRColumns] = rows["SSRCode"];
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
                                grid.JSProperties["cp_result"] = custommessage;//"Cannot replace " + SSRColumns + " with cheaper fee, please try again";
                                count = 1;
                                //return;
                            }
                            else
                            {
                                if (row["SSRCode" + SSRColumns].ToString().Trim() != rows["SSRCode"].ToString().Trim())
                                {
                                    row[SSRColumns] = rows["ConcatenatedField"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                    row["SSRCode" + SSRColumns] = rows["SSRCode"];

                                    if (NewValuesMeal == "" || NewValuesMeal == null)
                                    {
                                        DataRow[] resultMeals = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                                        foreach (DataRow rowz in resultMeals)
                                        {
                                            row[Meal] = rowz["Detail"];
                                            Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                            row["Price" + Meal] = Convert.ToDecimal(Detail);
                                            row["SSRCode" + Meal] = rowz["SSRCode"];
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
                                //row["Indicator" + SSRColumns] = "1";
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
                                //row["Indicator" + Meal] = "1";
                            }
                        }
                    }
                }
            }

            SSRColumns = "";
        }

        protected void AssignValue2(DataRow row, object NewValues, ref string SSRColumns, string Flight)
        {
            String Detail = "";
            if (NewValues == null || NewValues == "")
            {
                row[SSRColumns] = string.Empty;
                row["Price" + SSRColumns] = 0.00;
                row["Price" + SSRColumns + "S1"] = 0.00;
                row["Price" + SSRColumns + "S2"] = 0.00;
                row["SSRCode" + SSRColumns] = string.Empty;
            }
            else
            {
                row["SSRCode" + SSRColumns] = NewValues;
                DataTable dtComfort = Session["dt" + SSRColumns + Flight] as DataTable;
                if (NewValues.ToString().Length == 4)
                {
                    DataRow[] resultComfort = dtComfort.Select("SSRCode = '" + NewValues + "'");
                    foreach (DataRow rows in resultComfort)
                    {
                        row[SSRColumns] = rows["Detail"];
                        Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                        row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                        row["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows[3]);
                        row["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows[4]);
                        row["SSRCode" + SSRColumns] = rows["SSRCode"];
                    }
                }

                else
                {
                    DataRow[] resultComfort = dtComfort.Select("Detail = '" + NewValues.ToString().Substring(0, 11) + "'");
                    foreach (DataRow rows in resultComfort)
                    {
                        Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                        if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["Price" + SSRColumns]) && row["SSRCode" + SSRColumns].ToString().Trim() == rows[0].ToString().Trim())
                        {
                            continue;
                            //return;
                        }
                        else
                        {
                            row[SSRColumns] = rows["Detail"];
                            row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                            row["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows[3]);
                            row["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows[4]);
                            row["SSRCode" + SSRColumns] = rows["SSRCode"];
                        }
                    }
                }
                //}
            }

            SSRColumns = "";
        }

        protected void AssignValue2FlightChange(DataRow row, object NewValues, ref string SSRColumns, string Flight, ASPxGridView grid)
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
                    if (row[SSRColumns] != null && row[SSRColumns].ToString() != "")
                    {
                        DataRow[] resultComfort = dtComfort.Select("SSRCode = '" + NewValues + "'");
                        foreach (DataRow rows in resultComfort)
                        {
                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                            if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["Price" + SSRColumns]) && row["SSRCode" + SSRColumns].ToString().Trim() == rows[0].ToString().Trim())
                            {
                                continue;
                                //return;
                            }
                            else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["Price" + SSRColumns]))
                            {
                                custommessage = "";
                                custommessage = msgList.Err800008.Replace("SSRColumns", SSRColumns);
                                grid.JSProperties["cp_result"] = custommessage;//"Cannot replace " + SSRColumns + " with cheaper fee, please try again";
                                count = 1;
                                //return;
                            }
                            else
                            {
                                if (row["SSRCode" + SSRColumns].ToString().Trim() != rows["SSRCode"].ToString().Trim())
                                {
                                    row[SSRColumns] = rows["Detail"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                    row["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows[3]);
                                    row["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows[4]);
                                    row["SSRCode" + SSRColumns] = rows["SSRCode"];
                                }
                            }
                        }
                    }
                    else
                    {
                        DataRow[] resultComfort = dtComfort.Select("SSRCode = '" + NewValues + "'");
                        foreach (DataRow rows in resultComfort)
                        {
                            if (row["SSRCode" + SSRColumns].ToString().Trim() != rows["SSRCode"].ToString().Trim())
                            {
                                row[SSRColumns] = rows["Detail"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                row["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows[3]);
                                row["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows[4]);
                                row["SSRCode" + SSRColumns] = rows["SSRCode"];
                            }
                        }
                    }
                }
                else
                {
                    if (row[SSRColumns] != null && row[SSRColumns].ToString() != "")
                    {
                        DataRow[] resultComfort = dtComfort.Select("Detail = '" + NewValues.ToString().Substring(0, 11) + "'");
                        foreach (DataRow rows in resultComfort)
                        {
                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                            if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["Price" + SSRColumns]) && row["SSRCode" + SSRColumns].ToString().Trim() == rows[0].ToString().Trim())
                            {
                                continue;
                                //return;
                            }
                            else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["Price" + SSRColumns]))
                            {
                                custommessage = "";
                                custommessage = msgList.Err800008.Replace("SSRColumns", SSRColumns);
                                grid.JSProperties["cp_result"] = custommessage;//"Cannot replace " + SSRColumns + " with cheaper fee, please try again";
                                count = 1;
                                //return;
                            }
                            else
                            {
                                if (row["SSRCode" + SSRColumns].ToString().Trim() != rows["SSRCode"].ToString().Trim())
                                {
                                    row[SSRColumns] = rows["Detail"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                    row["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows[3]);
                                    row["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows[4]);
                                    row["SSRCode" + SSRColumns] = rows["SSRCode"];
                                }
                            }
                        }
                    }
                    else
                    {
                        DataRow[] resultComfort = dtComfort.Select("Detail = '" + NewValues.ToString().Substring(0, 11) + "'");
                        foreach (DataRow rows in resultComfort)
                        {
                            if (row["SSRCode" + SSRColumns].ToString().Trim() != rows["SSRCode"].ToString().Trim())
                            {
                                row[SSRColumns] = rows["Detail"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["Price" + SSRColumns] = Convert.ToDecimal(Detail);
                                row["Price" + SSRColumns + "S1"] = Convert.ToDecimal(rows[3]);
                                row["Price" + SSRColumns + "S2"] = Convert.ToDecimal(rows[4]);
                                row["SSRCode" + SSRColumns] = rows["SSRCode"];
                            }
                        }
                    }
                }
            }
            SSRColumns = "";
        }
    }
}