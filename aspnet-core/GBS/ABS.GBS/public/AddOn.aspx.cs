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

namespace GroupBooking.Web
{
    public partial class AddOn : System.Web.UI.Page
    {
        #region declaration
        UserSet MyUserSet;
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogControl log = new LogControl();

        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        Bk_transssr BK_TRANSSSRInfo = new Bk_transssr();
        List<Bk_transssr> listbk_transssrinfo = new List<Bk_transssr>();
        List<Bk_transssr> listbk_transssrinfo1 = new List<Bk_transssr>();
        List<Bk_transssr> listbk_transssrinfo2 = new List<Bk_transssr>();
        List<Bk_transssr> listbk_transssrinfo1t = new List<Bk_transssr>();
        List<Bk_transssr> listbk_transssrinfo2t = new List<Bk_transssr>();

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
        private static int qtyMeal, qtyMeal1, qtyBaggage, qtySport, qtyComfort, qtyDrink, qtyDrink1, qtyDuty = 0;
        private static int qtyMeal2, qtyMeal21, qtyBaggage2, qtySport2, qtyComfort2, qtyDrink2, qtyDrink21, qtyDuty2 = 0;
        private static int num = 0;
        private static int departID = 0;
        private static string ReturnID = "";
        private static int first = 0;
        //added by ketee, currency
        private string Curr = "";
        string SessionID = "";
        string signature = "";

        ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
        ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model2 = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
        #endregion

        #region "Load"
        protected void Page_Load(object sender, EventArgs e)
        {

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
                    MyUserSet = (UserSet)Session["AgentSet"];
                    if (!IsPostBack)
                    {
                        InitializeSetting();
                        GetSellSSR(signature);
                        //if (Convert.ToBoolean(Session["back"]) == true)
                        //{
                        BindLabel();
                        //}
                    }

                    if (Session["TransID"] != null)
                    {
                        hfTransID.Value = Session["TransID"].ToString();
                        string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), Session["TransID"].ToString(), "");
                        hfHashKey.Value = hashkey;
                        GetPassengerList(Session["TransID"].ToString());
                        GetBaggageDepart();
                        GetSportDepart();
                        if (Session["dtDrinkDepart"] != null)
                        {
                            GetDrinkDepart();
                        }
                        if (Session["dtDrinkDepart2"] != null)
                        {
                            GetDrink1Depart();
                        }
                    }
                    else
                    {
                        Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.SessionExpired);
                    }
                    //GetBaggageReturn();
                    //GetSportReturn();

                    HttpCookie cookie2 = Request.Cookies["cookieSearchcondition"];
                    if (cookie2 != null)
                    {
                        if (cookie2.Values["ifOneWay"] != "TRUE")
                        {
                            if (Session["TransID"] != null)
                            {
                                GetPassengerList2(Session["TransID"].ToString());
                                GetBaggageReturn();
                                GetSportReturn();
                                if (Session["dtDrinkReturn"] != null)
                                {
                                    GetDrinkReturn();
                                }
                                if (Session["dtDrinkReturn2"] != null)
                                {
                                    GetDrink1Return();
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
                       
                            BindDefaultBaggage();
                      

                    }
                }
                else
                {
                    if (IsCallback)
                        ASPxCallback.RedirectOnCallback(GroupBooking.Web.Shared.MySite.PublicPages.SessionExpired);
                    else
                        Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.SessionExpired);
                }
            }
            catch (Exception ex)
            {
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
        protected void GetBaggageDepart()
        {
            GridViewDataComboBoxColumn column = (gvPassenger.Columns["Baggage"] as GridViewDataComboBoxColumn);
            DataTable dtBaggage = null;
            dtBaggage = (DataTable)Session["dtBaggageDepart"];
            DataRow[] result = dtBaggage.Select("SSRCode = ''");
            if (result.Length == 0)
            {
                if (first == 1 && first == 3)
                {
                    dtBaggage.Rows.Add("", "", "");
                }
            }
            column.PropertiesComboBox.DataSource = dtBaggage;

            column.PropertiesComboBox.ValueField = "SSRCode";
            column.PropertiesComboBox.TextField = "ConcatenatedField";

            gvPassenger.DataBind();
        }

        protected void GetSportDepart()
        {
            GridViewDataComboBoxColumn column = (gvPassenger.Columns["Sport"] as GridViewDataComboBoxColumn);
            DataTable dtBaggage = null;
            dtBaggage = (DataTable)Session["dtSportDepart"];
            DataRow[] result = dtBaggage.Select("SSRCode = ''");
            if (result.Length == 0)
            {
                dtBaggage.Rows.Add("", "", "");
            }
            column.PropertiesComboBox.DataSource = dtBaggage;

            column.PropertiesComboBox.ValueField = "SSRCode";
            column.PropertiesComboBox.TextField = "ConcatenatedField";

            gvPassenger.DataBind();
        }

        protected void GetDrinkDepart()
        {
            GridViewDataComboBoxColumn column = (gvPassenger.Columns["Drink"] as GridViewDataComboBoxColumn);
            DataTable dtBaggage = null;
            dtBaggage = (DataTable)Session["dtDrinkDepart"];

            column.PropertiesComboBox.DataSource = dtBaggage;

            column.PropertiesComboBox.ValueField = "SSRCode";
            column.PropertiesComboBox.TextField = "ConcatenatedField";

            gvPassenger.DataBind();
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

        protected void GetBaggageReturn()
        {
            GridViewDataComboBoxColumn column = (gvPassenger2.Columns["Baggage"] as GridViewDataComboBoxColumn);
            DataTable dtBaggage = null;
            dtBaggage = (DataTable)Session["dtBaggageReturn"];
            DataRow[] result = dtBaggage.Select("SSRCode = ''");
            if (result.Length == 0)
            {
                if (first == 1 && first == 3)
                {
                    dtBaggage.Rows.Add("", "", "");
                }
            }
            column.PropertiesComboBox.DataSource = dtBaggage;

            column.PropertiesComboBox.ValueField = "SSRCode";
            column.PropertiesComboBox.TextField = "ConcatenatedField";

            gvPassenger2.DataBind();
        }

        protected void GetSportReturn()
        {
            GridViewDataComboBoxColumn column = (gvPassenger2.Columns["Sport"] as GridViewDataComboBoxColumn);
            DataTable dtBaggage = null;
            dtBaggage = (DataTable)Session["dtSportReturn"];
            DataRow[] result = dtBaggage.Select("SSRCode = ''");
            if (result.Length == 0)
            {
                dtBaggage.Rows.Add("", "", "");
            }
            column.PropertiesComboBox.DataSource = dtBaggage;

            column.PropertiesComboBox.ValueField = "SSRCode";
            column.PropertiesComboBox.TextField = "ConcatenatedField";

            gvPassenger2.DataBind();
        }

        protected void GetDrinkReturn()
        {
            GridViewDataComboBoxColumn column = (gvPassenger2.Columns["Drink"] as GridViewDataComboBoxColumn);
            DataTable dtBaggage = null;
            dtBaggage = (DataTable)Session["dtDrinkReturn"];

            column.PropertiesComboBox.DataSource = dtBaggage;

            column.PropertiesComboBox.ValueField = "SSRCode";
            column.PropertiesComboBox.TextField = "ConcatenatedField";

            gvPassenger2.DataBind();
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
            (e.Editor as ASPxTextBox).NullText = "No Meal";
        }

        protected void glMeals1_AutoFilterCellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            (e.Editor as ASPxTextBox).NullText = "Select Meal";
        }

        protected void glMeals2_AutoFilterCellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            (e.Editor as ASPxTextBox).NullText = "Select Meal";
        }

        protected void glMeals22_AutoFilterCellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            (e.Editor as ASPxTextBox).NullText = "Select Meal";
        }



        protected void glMealP1_Init(object sender, EventArgs e)
        {
            try
            {
                if (Session["dtMealDepart"] != null)
                {
                    ASPxGridLookup grdlkMealDepart = sender as ASPxGridLookup;

                    DataTable dtMeal = (DataTable)Session["dtMealDepart"];
                    DataRow[] result = dtMeal.Select("SSRCode = ''");
                    if (result.Length == 0)
                    {
                        dtMeal.Rows.Add("", "", "", "");
                    }
                    DataView dvMeal = dtMeal.DefaultView;

                    grdlkMealDepart.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                    grdlkMealDepart.DataBind();
                }
            }
            catch (Exception ex)
            {
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

                    DataTable dtMeal = (DataTable)Session["dtMealDepart2"];
                    DataRow[] result = dtMeal.Select("SSRCode = ''");
                    if (result.Length == 0)
                    {
                        dtMeal.Rows.Add("", "", "", "");
                    }
                    DataView dvMeal = dtMeal.DefaultView;

                    grdlkMealDepart.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                    grdlkMealDepart.DataBind();
                }
            }
            catch (Exception ex)
            {
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void glDrinkP1_Init(object sender, EventArgs e)
        {
            try
            {
                if (Session["dtDrinkDepart"] != null)
                {
                    ASPxGridLookup grdlkMealDepart = sender as ASPxGridLookup;

                    DataTable dtMeal = (DataTable)Session["dtDrinkDepart"];
                    DataRow[] result = dtMeal.Select("SSRCode = ''");
                    if (result.Length == 0)
                    {
                        dtMeal.Rows.Add("", "", "", "");
                    }
                    DataView dvMeal = dtMeal.DefaultView;

                    grdlkMealDepart.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                    grdlkMealDepart.DataBind();
                }
            }
            catch (Exception ex)
            {
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void glDrinkP11_Init(object sender, EventArgs e)
        {
            try
            {
                if (Session["dtDrinkDepart2"] != null)
                {
                    ASPxGridLookup grdlkMealDepart = sender as ASPxGridLookup;

                    DataTable dtMeal = (DataTable)Session["dtDrinkDepart2"];
                    DataRow[] result = dtMeal.Select("SSRCode = ''");
                    if (result.Length == 0)
                    {
                        dtMeal.Rows.Add("", "", "", "");
                    }
                    DataView dvMeal = dtMeal.DefaultView;

                    grdlkMealDepart.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
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

                    DataTable dtMeal = (DataTable)Session["dtComfortDepart"];
                    DataRow[] result = dtMeal.Select("SSRCode = ''");
                    if (result.Length == 0)
                    {
                        dtMeal.Rows.Add("", "", "", "");
                    }
                    DataView dvMeal = dtMeal.DefaultView;

                    grdlkMealDepart.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                    grdlkMealDepart.DataBind();
                }
            }
            catch (Exception ex)
            {
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

                    DataTable dtDuty = (DataTable)Session["dtDutyDepart"];
                    DataRow[] result = dtDuty.Select("SSRCode = ''");
                    if (result.Length == 0)
                    {
                        dtDuty.Rows.Add("", "", "", "");
                    }
                    DataView dvMeal = dtDuty.DefaultView;

                    grdlkMealDepart.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                    grdlkMealDepart.DataBind();
                }
            }
            catch (Exception ex)
            {
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

                    DataTable dtMeal = (DataTable)Session["dtMealReturn"];
                    DataRow[] result = dtMeal.Select("SSRCode = ''");
                    if (result.Length == 0)
                    {
                        dtMeal.Rows.Add("", "", "", "");
                    }
                    DataView dvMeal = dtMeal.DefaultView;

                    grdlkMealDepart.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                    grdlkMealDepart.DataBind();
                }
            }
            catch (Exception ex)
            {
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

                    DataTable dtMeal = (DataTable)Session["dtMealReturn2"];
                    DataRow[] result = dtMeal.Select("SSRCode = ''");
                    if (result.Length == 0)
                    {
                        dtMeal.Rows.Add("", "", "", "");
                    }
                    DataView dvMeal = dtMeal.DefaultView;

                    grdlkMealDepart.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                    grdlkMealDepart.DataBind();
                }
            }
            catch (Exception ex)
            {
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void glDrinkP21_Init(object sender, EventArgs e)
        {
            try
            {
                if (Session["dtDrinkReturn"] != null)
                {
                    ASPxGridLookup grdlkMealDepart = sender as ASPxGridLookup;

                    DataTable dtMeal = (DataTable)Session["dtDrinkReturn"];
                    DataRow[] result = dtMeal.Select("SSRCode = ''");
                    if (result.Length == 0)
                    {
                        dtMeal.Rows.Add("", "", "", "");
                    }
                    DataView dvMeal = dtMeal.DefaultView;

                    grdlkMealDepart.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                    grdlkMealDepart.DataBind();
                }
            }
            catch (Exception ex)
            {
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void glDrinkP22_Init(object sender, EventArgs e)
        {
            try
            {
                if (Session["dtDrinkReturn2"] != null)
                {
                    ASPxGridLookup grdlkMealDepart = sender as ASPxGridLookup;

                    DataTable dtMeal = (DataTable)Session["dtDrinkReturn2"];
                    DataRow[] result = dtMeal.Select("SSRCode = ''");
                    if (result.Length == 0)
                    {
                        dtMeal.Rows.Add("", "", "", "");
                    }
                    DataView dvMeal = dtMeal.DefaultView;

                    grdlkMealDepart.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                    grdlkMealDepart.DataBind();
                }
            }
            catch (Exception ex)
            {
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

                    DataTable dtMeal = (DataTable)Session["dtComfortReturn"];
                    DataRow[] result = dtMeal.Select("SSRCode = ''");
                    if (result.Length == 0)
                    {
                        dtMeal.Rows.Add("", "", "", "");
                    }
                    DataView dvMeal = dtMeal.DefaultView;

                    grdlkMealDepart.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                    grdlkMealDepart.DataBind();
                }
            }
            catch (Exception ex)
            {
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

                    DataTable dtMeal = (DataTable)Session["dtDutyReturn"];
                    DataRow[] result = dtMeal.Select("SSRCode = ''");
                    if (result.Length == 0)
                    {
                        dtMeal.Rows.Add("", "", "", "");
                    }
                    DataView dvMeal = dtMeal.DefaultView;

                    grdlkMealDepart.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                    grdlkMealDepart.DataBind();
                }
            }
            catch (Exception ex)
            {
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

                BindModel();
                Clearsession();
            }
            catch (Exception ex)
            {
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }
        protected void InitializeForm(String Currency, ArrayList BaggageCode, ArrayList BaggageFee, ArrayList SportCode, ArrayList SportFee, ArrayList ComfortCode, ArrayList ConfortFee, ArrayList ComfortImg, ArrayList MealCode, ArrayList MealFee, ArrayList MealImg, ArrayList MealCode1, ArrayList MealFee1, ArrayList MealImg1, ArrayList DutyCode, ArrayList DutyFee, ArrayList DutyImg, ArrayList DrinkCode, ArrayList DrinkFee, ArrayList DrinkCode1, ArrayList DrinkFee1)
        {
            HttpCookie cookie = Request.Cookies["cookieSearchcondition"];
            try
            {
                DataTable dtBaggage = new DataTable();
                dtBaggage.Columns.Add("SSRCode");
                dtBaggage.Columns.Add("Detail");
                dtBaggage.Columns.Add("Price");
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
                        for (int k = 0; k <= BaggageCode.Count - 1; k++)
                        {
                            if (BaggageCode[k].ToString() == dt.Rows[i]["ItemCode"].ToString())
                            {
                                dtBaggage.Rows.Add(BaggageCode[k].ToString(), dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(BaggageFee[k])).ToString("N", nfi) + " " + Currency);

                            }
                        }

                    }
                }

                DataView dv = dtBaggage.DefaultView;
                cmbBaggage.DataSource = dv.ToTable(true, "SSRCode", "Detail", "Price", "ConcatenatedField");
                cmbBaggage.TextField = "ConcatenatedField";
                cmbBaggage.ValueField = "SSRCode";
                cmbBaggage.DataBind();
                cmbBaggage.NullText = "Select Baggage";
                Session["dtBaggageDepart"] = dtBaggage;


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
                        //dtMeal.Rows.Add("", "", "");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            for (int k = 0; k <= MealCode.Count - 1; k++)
                            {
                                if (MealCode[k] != DBNull.Value)
                                {
                                    if (MealCode[k].ToString() == dt.Rows[i]["ItemCode"].ToString())
                                    {
                                        dtMeal.Rows.Add(MealCode[k].ToString(), dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(MealFee[k])).ToString("N", nfi) + " " + Currency, MealImg[k].ToString());

                                    }
                                }
                            }
                        }
                    }
                }

                DataView dvMeal = dtMeal.DefaultView;
                glMeals.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                glMeals.DataBind();
                if (dtMeal.Rows.Count > 0)
                {
                    glMeals.NullText = "Select Meal";
                }
                else
                {
                    glMeals.NullText = "No Meal";
                }
                Session["dtMealDepart"] = dtMeal;

                if (MealCode1 != null)
                {
                    DataTable dtMeal1 = new DataTable();
                    dtMeal1.Columns.Add("SSRCode");
                    dtMeal1.Columns.Add("Detail");
                    dtMeal1.Columns.Add("Price");
                    dtMeal1.Columns.Add("Images");

                    DataRow rowMeal1 = dtMeal1.NewRow();
                    Detail = "";
                    if (MealCode1.Count > 0 && DrinkCode1.Count >= 1)
                    {
                        foreach (string item in MealCode1)
                        {
                            Detail += "'" + item + "',";
                        }
                        Detail = Detail.Substring(0, Detail.Length - 1);

                        objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                        dt = objBooking.GetDetailSSRbyCode(Detail, "WYM");

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            //dtMeal1.Rows.Add("", "", "");
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                for (int k = 0; k <= MealCode1.Count - 1; k++)
                                {
                                    if (MealCode1[k] != DBNull.Value)
                                    {
                                        if (MealCode1[k].ToString() == dt.Rows[i]["ItemCode"].ToString())
                                        {
                                            dtMeal1.Rows.Add(MealCode1[k].ToString(), dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(MealFee1[k])).ToString("N", nfi) + " " + Currency, MealImg1[k].ToString());

                                        }
                                    }
                                }
                            }
                        }
                    }

                    DataView dvMeal1 = dtMeal1.DefaultView;
                    glMeals1.DataSource = dvMeal1.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                    glMeals1.DataBind();
                    if (dtMeal1.Rows.Count > 0)
                    {
                        glMeals1.NullText = "Select Meal";
                    }
                    else
                    {
                        glMeals1.NullText = "No Meal";
                    }
                    //glMeals1.NullText = "Select Meal";
                    Session["dtMealDepart2"] = dtMeal1;
                    gvPassenger.Columns["Meal"].Caption = "Meal 1";
                }
                else
                {
                    divmeal1.Style.Add("display", "none");
                    gvPassenger.Columns["Meal1"].Visible = false;

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

                    Detail = "";

                    foreach (string item in DrinkCode)
                    {
                        Detail += "'" + item + "',";
                    }

                    //check if not blank
                    if (Detail != "")
                    {
                        Detail = Detail.Substring(0, Detail.Length - 1);
                        objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                        dt = objBooking.GetDetailSSRbyCode(Detail, "WYD");
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            //dtMeal.Rows.Add("", "", "");
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                for (int k = 0; k <= DrinkCode.Count - 1; k++)
                                {
                                    if (DrinkCode[k] != DBNull.Value)
                                    {
                                        if (DrinkCode[k].ToString() == dt.Rows[i]["ItemCode"].ToString())
                                        {
                                            dtDrink.Rows.Add(DrinkCode[k].ToString(), dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(DrinkFee[k])).ToString("N", nfi) + " " + Currency);

                                        }
                                    }
                                }
                            }
                        }
                    }



                    DataView dvDrink = dtDrink.DefaultView;
                    //DataView dv = dtBaggage.DefaultView;
                    cmbDrinks.DataSource = dv.ToTable(true, "SSRCode", "Detail", "Price", "ConcatenatedField");
                    //dtDrink.DefaultView.RowFilter = "SSRCode LIKE 'BW%'";
                    cmbDrinks.DataSource = dtDrink.DefaultView;
                    cmbDrinks.TextField = "ConcatenatedField";
                    cmbDrinks.ValueField = "SSRCode";
                    cmbDrinks.DataBind();
                    cmbDrinks.NullText = "Default Drink";
                    Session["dtDrinkDepart"] = dtDrink;

                }
                else
                {
                    tdDrinks.Style.Add("display", "none");
                    gvPassenger.Columns["Drink"].Visible = false;
                }

                if (DrinkCode1 != null && DrinkCode1.Count >= 1)
                {
                    DataTable dtDrink1 = new DataTable();
                    dtDrink1.Columns.Add("SSRCode");
                    dtDrink1.Columns.Add("Detail");
                    dtDrink1.Columns.Add("Price");
                    dtDrink1.Columns.Add("ConcatenatedField", typeof(string), "Detail + '  ' +Price");

                    DataRow rowDrink1 = dtDrink1.NewRow();

                    Detail = "";
                    foreach (string item in DrinkCode1)
                    {
                        Detail += "'" + item + "',";
                    }

                    //check if not blank
                    if (Detail != "")
                    {
                        Detail = Detail.Substring(0, Detail.Length - 1);

                        objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                        dt = objBooking.GetDetailSSRbyCode(Detail, "WYD");
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            //dtMeal1.Rows.Add("", "", "");
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                for (int k = 0; k <= DrinkCode1.Count - 1; k++)
                                {
                                    if (DrinkCode1[k] != DBNull.Value)
                                    {
                                        if (DrinkCode1[k].ToString() == dt.Rows[i]["ItemCode"].ToString())
                                        {
                                            dtDrink1.Rows.Add(DrinkCode1[k].ToString(), dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(DrinkFee1[k])).ToString("N", nfi) + " " + Currency);

                                        }
                                    }
                                }
                            }
                        }
                    }

                    DataView dvDrink1 = dtDrink1.DefaultView;
                    //dtDrink1.DefaultView.RowFilter = "SSRCode LIKE 'BW%'";
                    cmbDrinks1.DataSource = dv.ToTable(true, "SSRCode", "Detail", "Price", "ConcatenatedField");
                    cmbDrinks1.DataSource = dtDrink1.DefaultView;
                    cmbDrinks1.TextField = "ConcatenatedField";
                    cmbDrinks1.ValueField = "SSRCode";
                    cmbDrinks1.DataBind();
                    cmbDrinks1.NullText = "Default Drink";
                    Session["dtDrinkDepart2"] = dtDrink1;
                    gvPassenger.Columns["Drink"].Caption = "Drink 1";
                }
                else
                {
                    tdDrinks1.Style.Add("display", "none");
                    gvPassenger.Columns["Drink1"].Visible = false;

                }

                DataTable dtSport = new DataTable();
                dtSport.Columns.Add("SSRCode");
                dtSport.Columns.Add("Detail");
                dtSport.Columns.Add("Price");
                dtSport.Columns.Add("ConcatenatedField", typeof(string), "Detail + '  ' +Price");

                //Enumerable.Range(1, 10).ToList().ForEach(i => dtSport.Rows.Add(i, string.Concat("Detail", i), string.Concat("Price", i)));
                DataRow row1 = dtSport.NewRow();

                Detail = "";
                foreach (string item in SportCode)
                {
                    Detail += "'" + item + "',";
                }

                //check if not blank
                if (Detail != "")
                {
                    Detail = Detail.Substring(0, Detail.Length - 1);
                    objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                    dt = objBooking.GetDetailSSRbyCode(Detail, "PBS");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //dtSport.Rows.Add("", "", "");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            for (int k = 0; k <= SportCode.Count - 1; k++)
                            {
                                if (SportCode[k].ToString() == dt.Rows[i]["ItemCode"].ToString())
                                {
                                    dtSport.Rows.Add(SportCode[k].ToString(), dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(SportFee[k])).ToString("N", nfi) + " " + Currency);

                                }
                            }
                        }
                    }
                }



                DataView dvSport = dtSport.DefaultView;
                cmbSport.DataSource = dvSport.ToTable(true, "SSRCode", "Detail", "Price", "ConcatenatedField");

                cmbSport.TextField = "ConcatenatedField";
                cmbSport.ValueField = "SSRCode";
                cmbSport.DataBind();
                cmbSport.NullText = "Select Sport Equipment";
                Session["dtSportDepart"] = dtSport;

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

                //check if not blank
                if (Detail != "")
                {
                    Detail = Detail.Substring(0, Detail.Length - 1);

                    objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                    dt = objBooking.GetDetailSSRbyCode(Detail, "PWH");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //dtDuty.Rows.Add("", "", "", "");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            for (int k = 0; k <= DutyCode.Count - 1; k++)
                            {
                                if (DutyCode[k] != DBNull.Value)
                                {
                                    if (DutyCode[k].ToString() == dt.Rows[i]["ItemCode"].ToString())
                                    {
                                        dtDuty.Rows.Add(DutyCode[k], dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(DutyFee[k])).ToString("N", nfi) + " " + Currency, DutyImg[k].ToString());

                                    }
                                }
                            }
                        }
                    }
                }


                DataView dvDuty = dtDuty.DefaultView;
                glDuty.DataSource = dvDuty.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                glDuty.DataBind();
                glDuty.NullText = "Select Duty Free";
                Session["dtDutyDepart"] = dtDuty;

                DataTable dtComfort = new DataTable();
                dtComfort.Columns.Add("SSRCode");
                dtComfort.Columns.Add("Detail");
                dtComfort.Columns.Add("Price");
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
                        for (int k = 0; k <= ComfortCode.Count - 1; k++)
                        {
                            if (ComfortCode[k].ToString() == dt.Rows[i]["ItemCode"].ToString())
                            {
                                dtComfort.Rows.Add(ComfortCode[k], dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(ConfortFee[k])).ToString("N", nfi) + " " + Currency, ComfortImg[k].ToString());
                            }
                        }
                    }
                }

                DataView dvComfort = dtComfort.DefaultView;
                glComfort.DataSource = dvComfort.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                glComfort.DataBind();
                glComfort.NullText = "Select Comfort Kit";
                Session["dtComfortDepart"] = dtComfort;
            }
            catch (Exception ex)
            {
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void InitializeForm2(String Currency, ArrayList BaggageCode, ArrayList BaggageFee, ArrayList SportCode, ArrayList SportFee, ArrayList ComfortCode, ArrayList ConfortFee, ArrayList ComfortImg, ArrayList MealCode, ArrayList MealFee, ArrayList MealImg, ArrayList MealCode2, ArrayList MealFee2, ArrayList MealImg2, ArrayList DutyCode, ArrayList DutyFee, ArrayList DutyImg, ArrayList DrinkCode, ArrayList DrinkFee, ArrayList DrinkCode2, ArrayList DrinkFee2)
        {

            try
            {
                DataTable dtBaggage = new DataTable();
                dtBaggage.Columns.Add("SSRCode");
                dtBaggage.Columns.Add("Detail");
                dtBaggage.Columns.Add("Price");
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
                        for (int k = 0; k <= BaggageCode.Count - 1; k++)
                        {
                            if (BaggageCode[k].ToString() == dt.Rows[i]["ItemCode"].ToString())
                            {
                                dtBaggage.Rows.Add(BaggageCode[k].ToString(), dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(BaggageFee[k])).ToString("N", nfi) + " " + Currency);

                            }
                        }

                    }
                }

                DataView dv = dtBaggage.DefaultView;
                cmbBaggage2.DataSource = dv.ToTable(true, "SSRCode", "Detail", "Price", "ConcatenatedField");
                cmbBaggage2.TextField = "ConcatenatedField";
                cmbBaggage2.ValueField = "SSRCode";
                cmbBaggage2.DataBind();
                cmbBaggage2.NullText = "Select Baggage";
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
                            for (int k = 0; k <= MealCode.Count - 1; k++)
                            {
                                if (MealCode[k] != DBNull.Value)
                                {
                                    if (MealCode[k].ToString() == dt.Rows[i]["ItemCode"].ToString())
                                    {
                                        dtMeal.Rows.Add(MealCode[k].ToString(), dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(MealFee[k])).ToString("N", nfi) + " " + Currency, MealImg[k].ToString());
                                    }
                                }
                            }
                        }
                    }
                }

                DataView dvMeal = dtMeal.DefaultView;
                glMeals2.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                glMeals2.DataBind();
                if (dtMeal.Rows.Count > 0)
                {
                    glMeals2.NullText = "Select Meal";
                }
                else
                {
                    glMeals2.NullText = "No Meal";
                }
                //glMeals2.NullText = "Select Meal";
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
                                for (int k = 0; k <= MealCode2.Count - 1; k++)
                                {
                                    if (MealCode2[k] != DBNull.Value)
                                    {
                                        if (MealCode2[k].ToString() == dt.Rows[i]["ItemCode"].ToString())
                                        {
                                            dtMeal2.Rows.Add(MealCode2[k].ToString(), dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(MealFee2[k])).ToString("N", nfi) + " " + Currency, MealImg2[k].ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }

                    DataView dvMeal2 = dtMeal2.DefaultView;
                    glMeals22.DataSource = dvMeal2.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                    glMeals22.DataBind();
                    if (dtMeal2.Rows.Count > 0)
                    {
                        glMeals22.NullText = "Select Meal";
                    }
                    else
                    {
                        glMeals22.NullText = "No Meal";
                    }
                    //glMeals22.NullText = "Select Meal";
                    Session["dtMealReturn2"] = dtMeal2;
                    gvPassenger2.Columns["Meal"].Caption = "Meal 1";
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
                            for (int k = 0; k <= DrinkCode.Count - 1; k++)
                            {
                                if (DrinkCode[k] != DBNull.Value)
                                {
                                    if (DrinkCode[k].ToString() == dt.Rows[i]["ItemCode"].ToString())
                                    {
                                        dtDrink.Rows.Add(DrinkCode[k].ToString(), dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(DrinkFee[k])).ToString("N", nfi) + " " + Currency);
                                    }
                                }
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
                    cmbDrinks2.NullText = "Default Drink";
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
                            for (int k = 0; k <= DrinkCode2.Count - 1; k++)
                            {
                                if (DrinkCode2[k] != DBNull.Value)
                                {
                                    if (DrinkCode2[k].ToString() == dt.Rows[i]["ItemCode"].ToString())
                                    {
                                        dtDrink2.Rows.Add(DrinkCode2[k].ToString(), dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(DrinkFee2[k])).ToString("N", nfi) + " " + Currency);
                                    }
                                }
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
                    cmbDrinks22.NullText = "Default Drink";
                    Session["dtDrinkReturn2"] = dtDrink2;
                    gvPassenger2.Columns["Drink"].Caption = "Drink 1";
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
                        for (int k = 0; k <= SportCode.Count - 1; k++)
                        {
                            if (SportCode[k].ToString() == dt.Rows[i]["ItemCode"].ToString())
                            {
                                dtSport.Rows.Add(SportCode[k].ToString(), dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(SportFee[k])).ToString("N", nfi) + " " + Currency);
                            }
                        }
                    }
                }


                DataView dvSport = dtSport.DefaultView;
                cmbSport2.DataSource = dvSport.ToTable(true, "SSRCode", "Detail", "Price", "ConcatenatedField");

                cmbSport2.TextField = "ConcatenatedField";
                cmbSport2.ValueField = "SSRCode";
                cmbSport2.DataBind();
                cmbSport2.NullText = "Select Sport Equipment";
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
                        for (int k = 0; k <= DutyCode.Count - 1; k++)
                        {
                            if (DutyCode[k] != DBNull.Value)
                            {
                                if (DutyCode[k].ToString() == dt.Rows[i]["ItemCode"].ToString())
                                {
                                    dtDuty.Rows.Add(DutyCode[k], dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(DutyFee[k])).ToString("N", nfi) + " " + Currency, DutyImg[k].ToString());

                                }
                            }
                        }
                    }
                }

                DataView dvDuty = dtDuty.DefaultView;
                glDuty2.DataSource = dvDuty.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                glDuty2.DataBind();
                glDuty2.NullText = "Select Duty Free";
                Session["dtDutyReturn"] = dtDuty;

                DataTable dtComfort = new DataTable();
                dtComfort.Columns.Add("SSRCode");
                dtComfort.Columns.Add("Detail");
                dtComfort.Columns.Add("Price");
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
                        for (int k = 0; k <= ComfortCode.Count - 1; k++)
                        {
                            if (ComfortCode[k].ToString() == dt.Rows[i]["ItemCode"].ToString())
                            {
                                dtComfort.Rows.Add(ComfortCode[k], dt.Rows[i]["ItemDesc"], objGeneral.RoundUp(Convert.ToDecimal(ConfortFee[k])).ToString("N", nfi) + " " + Currency, ComfortImg[k].ToString());
                            }
                        }
                    }
                }

                DataView dvComfort = dtComfort.DefaultView;
                glComfort2.DataSource = dvComfort.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                glComfort2.DataBind();
                glComfort2.NullText = "Select Comfort Kit";
                Session["dtComfortReturn"] = dtComfort;
            }
            catch (Exception ex)
            {
                //sTraceLog(ex.ToString);
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

                if (IsNumeric(foundRows[0]["TemFlightPromoDisc"].ToString()))
                { model.TemFlightPromoDisc = Convert.ToDecimal(foundRows[0]["TemFlightPromoDisc"]); }

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

                model.TemFlightPromoCode = foundRows[0]["TemFlightPromoCode"].ToString();
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
            }
        }

        protected void Clearsession()
        {
            Session["IsNew"] = "True";
            //Session["dtGridPass"] = null;
            //Session["dtGridPass2"] = null;
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
        }
        #endregion

        #region "Event"
        protected void BindDefaultBaggage()
        {
            string Detail = "";
            try
            {
                HttpCookie cookie = Request.Cookies["cookieSearchcondition"];
                DataTable dtPass = new DataTable();
                DataTable dtPass2 = new DataTable();

                if (Session["dtGridPass"] != null)
                {
                    dtPass = (DataTable)Session["dtGridPass"];
                }
                if (dtPass != null && dtPass.Rows.Count > 0)
                {
                    if (cookie != null)
                    {
                        if (objGeneral.IsInternationalFlight(cookie.Values["Departure"].ToString(), cookie.Values["Arrival"].ToString(), Request.PhysicalApplicationPath))

                        //if (objBooking.InternationalFlight(cookie.Values["Departure"].ToString(), cookie.Values["Arrival"].ToString()))
                        {
                            DataTable dtdefaultBundle = objBooking.GetDefaultBundle("DEFAULTINTERNATIONALBAGGAGE", model.TemFlightCarrierCode);
                            if (dtdefaultBundle != null && dtdefaultBundle.Rows.Count > 0)
                            {
                                if (Request.QueryString["change"] == null)
                                {
                                    DataTable dtBaggage = (DataTable)Session["dtBaggageDepart"];
                                    for (int i = 0; i <= dtPass.Rows.Count - 1; i++)
                                    {

                                        DataRow[] result = dtBaggage.Select("SSRCode = '" + dtdefaultBundle.Rows[0]["SYSValue"] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            dtPass.Rows[i]["Baggage"] = row[3];
                                            dtPass.Rows[i]["SSRCodeBaggage"] = row[0];
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                        }
                                        dtPass.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                    }
                                    Session["dtGridPass"] = dtPass;

                                    gvPassenger.DataSource = dtPass;
                                    gvPassenger.DataBind();

                                    if (cookie.Values["ifOneWay"] != "TRUE")
                                    {
                                        if (Session["dtGridPass2"] != null)
                                        {
                                            dtPass2 = (DataTable)Session["dtGridPass2"];
                                        }
                                        DataTable dtBaggage2 = (DataTable)Session["dtBaggageReturn"];
                                        for (int i = 0; i <= dtPass2.Rows.Count - 1; i++)
                                        {
                                            DataRow[] result = dtBaggage2.Select("SSRCode = '" + dtdefaultBundle.Rows[0]["SYSValue"] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                dtPass2.Rows[i]["Baggage"] = row[3];
                                                dtPass2.Rows[i]["SSRCodeBaggage"] = row[0];
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            }
                                            dtPass2.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                        }
                                        Session["dtGridPass2"] = dtPass2;
                                        gvPassenger2.DataSource = dtPass2;
                                        gvPassenger2.DataBind();
                                    }
                                }
                                Session["dtdefaultBundle"] = dtdefaultBundle;
                                first = 2; //DefaultInternationalBundle = Yes
                            }
                            else
                            {
                                first = 3; //DefaultInternationalBundle = No
                            }
                            Session["IsNew"] = "false";
                            BindLabel();
                        }
                        else
                        {
                            DataTable dtdefaultBundle = objBooking.GetDefaultBundle("DEFAULTDOMESTICBAGGAGE", model.TemFlightCarrierCode);
                            if (dtdefaultBundle != null && dtdefaultBundle.Rows.Count > 0)
                            {
                                 if (Request.QueryString["change"] == null)
                                {
                                DataTable dtBaggage = (DataTable)Session["dtBaggageDepart"];
                                for (int i = 0; i <= dtPass.Rows.Count - 1; i++)
                                {

                                    DataRow[] result = dtBaggage.Select("SSRCode = '" + dtdefaultBundle.Rows[0]["SYSValue"] + "'");
                                    foreach (DataRow row in result)
                                    {
                                        dtPass.Rows[i]["Baggage"] = row[3];
                                        dtPass.Rows[i]["SSRCodeBaggage"] = row[0];
                                        Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                    }
                                    dtPass.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                }
                                Session["dtGridPass"] = dtPass;
                                gvPassenger.DataSource = dtPass;
                                gvPassenger.DataBind();
                                if (cookie.Values["ifOneWay"] != "TRUE")
                                {
                                    if (Session["dtGridPass2"] != null)
                                    {
                                        dtPass2 = (DataTable)Session["dtGridPass2"];
                                    }
                                    DataTable dtBaggage2 = (DataTable)Session["dtBaggageReturn"];
                                    for (int i = 0; i <= dtPass2.Rows.Count - 1; i++)
                                    {
                                        DataRow[] result = dtBaggage2.Select("SSRCode = '" + dtdefaultBundle.Rows[0]["SYSValue"] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            dtPass2.Rows[i]["Baggage"] = row[3];
                                            dtPass2.Rows[i]["SSRCodeBaggage"] = row[0];
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                        }
                                        dtPass2.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                    }
                                    Session["dtGridPass2"] = dtPass2;
                                    gvPassenger2.DataSource = dtPass2;
                                    gvPassenger2.DataBind();
                                }
                                     }
                                Session["dtdefaultBundle"] = dtdefaultBundle;
                                first = 2; //DefaultDomesticBundle = Yes
                            }
                            else
                            {
                                first = 1; //DefaultDomesticBundle = No
                            }

                            Session["IsNew"] = "false";
                            BindLabel();
                        }
                        Session["CarrierCode"] = model.TemFlightCarrierCode;
                    }
                }

            }
            catch (Exception ex)
            {
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void SSRActionPanel_PerformCallback(object sender, CallbackEventArgsBase e)
        {
            object TotalBaggage, TotalMeal, TotalMeal1, TotalSport, TotalComfort;
            object TotalBaggage2, TotalMeal2, TotalMeal12, TotalSport2, TotalComfort2;

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
                    //TotalDuty = dtPass.Compute("Sum(PriceDuty)", "");



                    if (Session["dtGridPass2"] != null)
                    {
                        DataTable dtPass2 = (DataTable)Session["dtGridPass2"];
                        TotalBaggage2 = dtPass2.Compute("Sum(PriceBaggage)", "");
                        TotalMeal2 = dtPass2.Compute("Sum(PriceMeal)", "");
                        TotalMeal12 = dtPass2.Compute("Sum(PriceMeal1)", "");
                        TotalSport2 = dtPass2.Compute("Sum(PriceSport)", "");
                        TotalComfort2 = dtPass2.Compute("Sum(PriceComfort)", "");
                        //TotalDuty2 = dtPass2.Compute("Sum(PriceDuty)", "");

                        //lblTotalBaggage.Text = (Convert.ToDecimal(TotalBaggage) + Convert.ToDecimal(TotalBaggage2)).ToString("N", nfi);
                        //lblTotalMeal.Text = (Convert.ToDecimal(TotalMeal) + Convert.ToDecimal(TotalMeal2) + Convert.ToDecimal(TotalMeal1) + Convert.ToDecimal(TotalMeal12)).ToString("N", nfi);
                        //lblTotalSport.Text = (Convert.ToDecimal(TotalSport) + Convert.ToDecimal(TotalSport2)).ToString("N", nfi);
                        //lblTotalComfort.Text = (Convert.ToDecimal(TotalComfort) + Convert.ToDecimal(TotalComfort2)).ToString("N", nfi);
                        //lblTotalDuty.Text = (Convert.ToDecimal(TotalDuty) + Convert.ToDecimal(TotalDuty2)).ToString("N", nfi);
                        lblTotalAmount.Text = (Convert.ToDecimal(TotalBaggage) + Convert.ToDecimal(TotalBaggage2) + Convert.ToDecimal(TotalMeal)
                            + Convert.ToDecimal(TotalMeal2) + Convert.ToDecimal(TotalMeal1) + Convert.ToDecimal(TotalMeal12)
                            + Convert.ToDecimal(TotalSport) + Convert.ToDecimal(TotalSport2) + Convert.ToDecimal(TotalComfort)
                            + Convert.ToDecimal(TotalComfort2)).ToString("N", nfi);
                    }
                    else
                    {
                        //lblTotalBaggage.Text = (Convert.ToDecimal(TotalBaggage)).ToString("N", nfi);
                        //lblTotalMeal.Text = (Convert.ToDecimal(TotalMeal) + Convert.ToDecimal(TotalMeal1)).ToString("N", nfi);
                        //lblTotalSport.Text = (Convert.ToDecimal(TotalSport)).ToString("N", nfi);
                        //lblTotalComfort.Text = (Convert.ToDecimal(TotalComfort)).ToString("N", nfi);
                        //lblTotalDuty.Text = (Convert.ToDecimal(TotalDuty)).ToString("N", nfi);
                        lblTotalAmount.Text = (Convert.ToDecimal(TotalBaggage) + Convert.ToDecimal(TotalMeal)
                            + Convert.ToDecimal(TotalMeal1) + Convert.ToDecimal(TotalSport) + Convert.ToDecimal(TotalComfort)).ToString("N", nfi);
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
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void SSRTab1Panel_PerformCallback(object sender, CallbackEventArgsBase e)
        {
            object TotalBaggage, TotalMeal, TotalMeal1, TotalSport, TotalComfort;
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
                    //TotalDuty = dtPass.Compute("Sum(PriceDuty)", "");

                    lblTotalBaggage.Text = (Convert.ToDecimal(TotalBaggage)).ToString("N", nfi);
                    lblTotalMeal.Text = (Convert.ToDecimal(TotalMeal) + Convert.ToDecimal(TotalMeal1)).ToString("N", nfi);
                    lblTotalSport.Text = (Convert.ToDecimal(TotalSport)).ToString("N", nfi);
                    lblTotalComfort.Text = (Convert.ToDecimal(TotalComfort)).ToString("N", nfi);
                    //lblTotalDuty.Text = (Convert.ToDecimal(TotalDuty)).ToString("N", nfi);
                    lblTotalAmount.Text = (Convert.ToDecimal(string.IsNullOrEmpty(lblTotalAmount.Text) ? "0" : lblTotalAmount.Text) + Convert.ToDecimal(TotalBaggage) + Convert.ToDecimal(TotalMeal)
                        + Convert.ToDecimal(TotalMeal1) + Convert.ToDecimal(TotalSport) + Convert.ToDecimal(TotalComfort)).ToString("N", nfi);

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
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void SSRTab2Panel_PerformCallback(object sender, CallbackEventArgsBase e)
        {
            object TotalBaggage, TotalMeal, TotalMeal1, TotalSport, TotalComfort;
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
                    //TotalDuty = dtPass.Compute("Sum(PriceDuty)", "");

                    lblTotalBaggage2.Text = (Convert.ToDecimal(TotalBaggage)).ToString("N", nfi);
                    lblTotalMeal2.Text = (Convert.ToDecimal(TotalMeal) + Convert.ToDecimal(TotalMeal1)).ToString("N", nfi);
                    lblTotalSport2.Text = (Convert.ToDecimal(TotalSport)).ToString("N", nfi);
                    lblTotalComfort2.Text = (Convert.ToDecimal(TotalComfort)).ToString("N", nfi);
                    //lblTotalDuty2.Text = (Convert.ToDecimal(TotalDuty)).ToString("N", nfi);
                    lblTotalAmount.Text = (Convert.ToDecimal(string.IsNullOrEmpty(lblTotalAmount.Text) ? "0" : lblTotalAmount.Text) + Convert.ToDecimal(TotalBaggage) + Convert.ToDecimal(TotalMeal)
                        + Convert.ToDecimal(TotalMeal1) + Convert.ToDecimal(TotalSport) + Convert.ToDecimal(TotalComfort)).ToString("N", nfi);

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
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void BindLabel()
        {
            object TotalBaggage, TotalMeal, TotalMeal1, TotalSport, TotalComfort;
            object TotalBaggage2, TotalMeal2, TotalMeal12, TotalSport2, TotalComfort2;

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
                    //TotalDuty = dtPass.Compute("Sum(PriceDuty)", "");



                    if (Session["dtGridPass2"] != null)
                    {
                        DataTable dtPass2 = (DataTable)Session["dtGridPass2"];
                        TotalBaggage2 = dtPass2.Compute("Sum(PriceBaggage)", "");
                        TotalMeal2 = dtPass2.Compute("Sum(PriceMeal)", "");
                        TotalMeal12 = dtPass2.Compute("Sum(PriceMeal1)", "");
                        TotalSport2 = dtPass2.Compute("Sum(PriceSport)", "");
                        TotalComfort2 = dtPass2.Compute("Sum(PriceComfort)", "");
                        //TotalDuty2 = dtPass2.Compute("Sum(PriceDuty)", "");


                        lblTotalBaggage.Text = (Convert.ToDecimal(TotalBaggage)).ToString("N", nfi);
                        lblTotalMeal.Text = (Convert.ToDecimal(TotalMeal) + Convert.ToDecimal(TotalMeal1)).ToString("N", nfi);
                        lblTotalSport.Text = (Convert.ToDecimal(TotalSport)).ToString("N", nfi);
                        lblTotalComfort.Text = (Convert.ToDecimal(TotalComfort)).ToString("N", nfi);
                        //lblTotalDuty.Text = (Convert.ToDecimal(TotalDuty)).ToString("N", nfi);

                        lblTotalBaggage2.Text = (Convert.ToDecimal(TotalBaggage2)).ToString("N", nfi);
                        lblTotalMeal2.Text = (Convert.ToDecimal(TotalMeal2) + Convert.ToDecimal(TotalMeal1) + Convert.ToDecimal(TotalMeal12)).ToString("N", nfi);
                        lblTotalSport2.Text = (Convert.ToDecimal(TotalSport2)).ToString("N", nfi);
                        lblTotalComfort2.Text = (Convert.ToDecimal(TotalComfort2)).ToString("N", nfi);
                        //lblTotalDuty2.Text = (Convert.ToDecimal(TotalDuty2)).ToString("N", nfi);
                        lblTotalAmount.Text = (Convert.ToDecimal(TotalBaggage) + Convert.ToDecimal(TotalBaggage2) + Convert.ToDecimal(TotalMeal)
                            + Convert.ToDecimal(TotalMeal2) + Convert.ToDecimal(TotalMeal1) + Convert.ToDecimal(TotalMeal12)
                            + Convert.ToDecimal(TotalSport) + Convert.ToDecimal(TotalSport2) + Convert.ToDecimal(TotalComfort)
                            + Convert.ToDecimal(TotalComfort2)).ToString("N", nfi);
                    }
                    else
                    {
                        lblTotalBaggage.Text = (Convert.ToDecimal(TotalBaggage)).ToString("N", nfi);
                        lblTotalMeal.Text = (Convert.ToDecimal(TotalMeal) + Convert.ToDecimal(TotalMeal1)).ToString("N", nfi);
                        lblTotalSport.Text = (Convert.ToDecimal(TotalSport)).ToString("N", nfi);
                        lblTotalComfort.Text = (Convert.ToDecimal(TotalComfort)).ToString("N", nfi);
                        //lblTotalDuty.Text = (Convert.ToDecimal(TotalDuty)).ToString("N", nfi);
                        lblTotalAmount.Text = (Convert.ToDecimal(TotalBaggage) + Convert.ToDecimal(TotalMeal)
                            + Convert.ToDecimal(TotalMeal1) + Convert.ToDecimal(TotalSport) + Convert.ToDecimal(TotalComfort)).ToString("N", nfi);
                    }

                    lblCurrency.Text = Curr;
                }
            }
            catch (Exception ex)
            {
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
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }
        protected void gvPassenger_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
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
                if (Session["dtGridPass"] == null)
                {
                    GetPassengerList(Session["TransID"].ToString());

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
                                        if (first == 2)
                                        {
                                            DataTable dtdefault = (DataTable)Session["dtdefaultBundle"];
                                            if (dtdefault != null && dtdefault.Rows.Count > 0)
                                            {
                                                DataRow[] result = dtBaggage.Select("SSRCode = '" + dtdefault.Rows[0]["SYSValue"] + "'");
                                                foreach (DataRow row in result)
                                                {
                                                    dtPass.Rows[i]["Baggage"] = row[3];
                                                    dtPass.Rows[i]["SSRCodeBaggage"] = row[0];
                                                    Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                }
                                                dtPass.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                            }
                                        }
                                        else
                                        {
                                            dtPass.Rows[i]["Baggage"] = string.Empty;
                                            dtPass.Rows[i]["SSRCodeBaggage"] = string.Empty;
                                            dtPass.Rows[i]["PriceBaggage"] = 0.00;
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
                                            dtPass.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
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
                                            if (first == 2)
                                            {
                                                DataTable dtdefault = (DataTable)Session["dtdefaultBundle"];
                                                if (dtdefault != null && dtdefault.Rows.Count > 0)
                                                {
                                                    DataRow[] result = dtBaggage.Select("SSRCode = '" + dtdefault.Rows[0]["SYSValue"] + "'");
                                                    foreach (DataRow row in result)
                                                    {
                                                        dtPass.Rows[i]["Baggage"] = row[3];
                                                        dtPass.Rows[i]["SSRCodeBaggage"] = row[0];
                                                        Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                    }
                                                    dtPass.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                                }
                                            }
                                            else
                                            {
                                                dtPass.Rows[i]["Baggage"] = string.Empty;
                                                dtPass.Rows[i]["SSRCodeBaggage"] = string.Empty;
                                                dtPass.Rows[i]["PriceBaggage"] = 0.00;
                                            }
                                        }
                                        else
                                        {

                                            dtPass.Rows[i]["Baggage"] = args[2];
                                            dtPass.Rows[i]["SSRCodeBaggage"] = args[3];
                                            DataRow[] result = dtBaggage.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                dtPass.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                            }
                                            //dtPass.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                        }
                                    }
                                    Session["dtGridPass"] = dtPass;
                                    Session["qtyBaggage"] = (Convert.ToInt32(args[1]) + qtyBaggage);
                                    gvPassenger.DataSource = dtPass;
                                    gvPassenger.DataBind();
                                }
                                else
                                {
                                    gvPassenger.JSProperties["cp_result"] = "The quantity of Baggage must be less or equal to total number of passenger.";
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
                                        dtPass.Rows[i]["Meal"] = string.Empty;
                                        dtPass.Rows[i]["SSRCodeMeal"] = string.Empty;
                                        dtPass.Rows[i]["PriceMeal"] = 0.00;

                                    }
                                    else
                                    {
                                        //GetSellSSR(signature);
                                        dtPass.Rows[i]["Meal"] = args[2];
                                        dtPass.Rows[i]["SSRCodeMeal"] = args[3];
                                        DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            dtPass.Rows[i]["PriceMeal"] = Convert.ToDecimal(Detail);
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
                                            dtPass.Rows[i]["Meal"] = string.Empty;
                                            dtPass.Rows[i]["SSRCodeMeal"] = string.Empty;
                                            dtPass.Rows[i]["PriceMeal"] = 0.00;
                                        }
                                        else
                                        {
                                            //GetSellSSR(signature);
                                            dtPass.Rows[i]["Meal"] = args[2];
                                            dtPass.Rows[i]["SSRCodeMeal"] = args[3];
                                            DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                dtPass.Rows[i]["PriceMeal"] = Convert.ToDecimal(Detail);
                                            }
                                        }
                                        //dtPass.Rows[i]["PriceMeal"] = Convert.ToDecimal(Detail);
                                    }
                                    if (Session["dtDrinkDepart"] == null)
                                    {
                                        Session["dtGridPass"] = dtPass;
                                        Session["qtyMeal"] = (Convert.ToInt32(args[1]) + qtyMeal);
                                        gvPassenger.DataSource = dtPass;
                                        gvPassenger.DataBind();
                                    }
                                }
                                else
                                {
                                    gvPassenger.JSProperties["cp_result"] = "The quantity of Meal must be less or equal to total number of passenger.";
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
                                    if (args[2].ToString() == ",")
                                    {
                                        dtPass.Rows[i]["Drink"] = string.Empty;
                                        dtPass.Rows[i]["SSRCodeDrink"] = string.Empty;
                                        dtPass.Rows[i]["PriceDrink"] = 0.00;
                                    }
                                    else
                                    {
                                        string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                        for (int c = 0; c < drinkcode.Length; c++)
                                        {
                                            DataRow[] result = dtDrink.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                            if (result.Length > 0)
                                            {
                                                foreach (DataRow row in result)
                                                {
                                                    Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                    dtPass.Rows[i]["PriceDrink"] = Convert.ToDecimal(Detail);
                                                    dtPass.Rows[i]["Drink"] = row[3];
                                                    dtPass.Rows[i]["SSRCodeDrink"] = row[0];
                                                }
                                                break;
                                            }
                                        }
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
                                            dtPass.Rows[i]["Drink"] = string.Empty;
                                            dtPass.Rows[i]["SSRCodeDrink"] = string.Empty;
                                            dtPass.Rows[i]["PriceDrink"] = 0.00;
                                        }
                                        else
                                        {
                                            string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                            for (int c = 0; c < drinkcode.Length; c++)
                                            {
                                                DataRow[] result = dtDrink.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                                if (result.Length > 0)
                                                {
                                                    foreach (DataRow row in result)
                                                    {
                                                        Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                        dtPass.Rows[i]["PriceDrink"] = Convert.ToDecimal(Detail);
                                                        dtPass.Rows[i]["Drink"] = row[3];
                                                        dtPass.Rows[i]["SSRCodeDrink"] = row[0];
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                        //dtPass.Rows[i]["PriceMeal"] = Convert.ToDecimal(Detail);
                                    }
                                    Session["dtGridPass"] = dtPass;
                                    Session["qtyMeal"] = (Convert.ToInt32(args[1]) + qtyMeal);
                                    Session["qtyDrink"] = (Convert.ToInt32(args[1]) + qtyDrink);
                                    gvPassenger.DataSource = dtPass;
                                    gvPassenger.DataBind();
                                }
                                else
                                {
                                    gvPassenger.JSProperties["cp_result"] = "The quantity of Meal must be less or equal to total number of passenger.";
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
                                        dtPass.Rows[i]["Meal1"] = string.Empty;
                                        dtPass.Rows[i]["SSRCodeMeal1"] = string.Empty;
                                        dtPass.Rows[i]["PriceMeal1"] = 0.00;
                                    }
                                    else
                                    {
                                        dtPass.Rows[i]["Meal1"] = args[2];
                                        dtPass.Rows[i]["SSRCodeMeal1"] = args[3];
                                        DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            dtPass.Rows[i]["PriceMeal1"] = Convert.ToDecimal(Detail);
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
                                            dtPass.Rows[i]["Meal1"] = string.Empty;
                                            dtPass.Rows[i]["SSRCodeMeal1"] = string.Empty;
                                            dtPass.Rows[i]["PriceMeal1"] = 0.00;
                                        }
                                        else
                                        {
                                            dtPass.Rows[i]["Meal1"] = args[2];
                                            dtPass.Rows[i]["SSRCodeMeal1"] = args[3];
                                            DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                dtPass.Rows[i]["PriceMeal1"] = Convert.ToDecimal(Detail);
                                            }
                                            dtPass.Rows[i]["PriceMeal1"] = Convert.ToDecimal(Detail);
                                        }
                                    }
                                    if (Session["dtDrinkDepart2"] == null)
                                    {
                                        Session["dtGridPass"] = dtPass;
                                        Session["qtyMeal1"] = (Convert.ToInt32(args[1]) + qtyMeal1);
                                        gvPassenger.DataSource = dtPass;
                                        gvPassenger.DataBind();
                                    }
                                }
                                else
                                {
                                    gvPassenger.JSProperties["cp_result"] = "The quantity of Meal 2 must be less or equal to total number of passenger.";
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
                                    if (args[2].ToString() == ",")
                                    {
                                        dtPass.Rows[i]["Drink1"] = string.Empty;
                                        dtPass.Rows[i]["SSRCodeDrink1"] = string.Empty;
                                        dtPass.Rows[i]["PriceDrink1"] = 0.00;
                                    }
                                    else
                                    {
                                        string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                        for (int c = 0; c < drinkcode.Length; c++)
                                        {
                                            DataRow[] result = dtDrink.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                            if (result.Length > 0)
                                            {
                                                foreach (DataRow row in result)
                                                {
                                                    Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                    dtPass.Rows[i]["PriceDrink1"] = Convert.ToDecimal(Detail);
                                                    dtPass.Rows[i]["Drink1"] = row[3];
                                                    dtPass.Rows[i]["SSRCodeDrink1"] = row[0];
                                                }
                                                break;
                                            }
                                        }
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
                                            dtPass.Rows[i]["Drink1"] = string.Empty;
                                            dtPass.Rows[i]["SSRCodeDrink1"] = string.Empty;
                                            dtPass.Rows[i]["PriceDrink1"] = 0.00;
                                        }
                                        else
                                        {
                                            string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                            for (int c = 0; c < drinkcode.Length; c++)
                                            {
                                                DataRow[] result = dtDrink.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                                if (result.Length > 0)
                                                {
                                                    foreach (DataRow row in result)
                                                    {
                                                        Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                        dtPass.Rows[i]["PriceDrink1"] = Convert.ToDecimal(Detail);
                                                        dtPass.Rows[i]["Drink1"] = row[3];
                                                        dtPass.Rows[i]["SSRCodeDrink1"] = row[0];
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    Session["dtGridPass"] = dtPass;
                                    Session["qtyMeal1"] = (Convert.ToInt32(args[1]) + qtyMeal1);
                                    Session["qtyDrink1"] = (Convert.ToInt32(args[1]) + qtyDrink1);
                                    gvPassenger.DataSource = dtPass;
                                    gvPassenger.DataBind();
                                }
                                else
                                {
                                    gvPassenger.JSProperties["cp_result"] = "The quantity of Meal 2 must be less or equal to total number of passenger.";
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
                                        dtPass.Rows[i]["Sport"] = string.Empty;
                                        dtPass.Rows[i]["SSRCodeSport"] = string.Empty;
                                        dtPass.Rows[i]["PriceSport"] = 0.00;
                                    }
                                    else
                                    {

                                        dtPass.Rows[i]["Sport"] = args[2];
                                        dtPass.Rows[i]["SSRCodeSport"] = args[3];
                                        DataRow[] result = dtSport.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            dtPass.Rows[i]["PriceSport"] = Convert.ToDecimal(Detail);
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
                                            dtPass.Rows[i]["Sport"] = string.Empty;
                                            dtPass.Rows[i]["SSRCodeSport"] = string.Empty;
                                            dtPass.Rows[i]["PriceSport"] = 0.00;
                                        }
                                        else
                                        {
                                            dtPass.Rows[i]["Sport"] = args[2];
                                            dtPass.Rows[i]["SSRCodeSport"] = args[3];
                                            DataRow[] result = dtSport.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                dtPass.Rows[i]["PriceSport"] = Convert.ToDecimal(Detail);
                                            }
                                        }
                                        //dtPass.Rows[i]["PriceSport"] = Convert.ToDecimal(Detail);
                                    }
                                    Session["dtGridPass"] = dtPass;
                                    Session["qtySport"] = (Convert.ToInt32(args[1]) + qtySport);
                                    gvPassenger.DataSource = dtPass;
                                    gvPassenger.DataBind();
                                }
                                else
                                {
                                    gvPassenger.JSProperties["cp_result"] = "The quantity of Sport must be less or equal to total number of passenger.";
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
                                gvPassenger.JSProperties["cp_result"] = "The quantity of Duty Free must be less or equal to total number of passenger.";
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
                                    dtPass.Rows[i]["Comfort"] = string.Empty;
                                    dtPass.Rows[i]["SSRCodeComfort"] = string.Empty;
                                    dtPass.Rows[i]["PriceComfort"] = 0.00;
                                }
                                else
                                {
                                    dtPass.Rows[i]["Comfort"] = args[2];
                                    dtPass.Rows[i]["SSRCodeComfort"] = args[3];
                                    DataRow[] result = dtComfort.Select("SSRCode = '" + args[3] + "'");
                                    foreach (DataRow row in result)
                                    {
                                        Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                        dtPass.Rows[i]["PriceComfort"] = Convert.ToDecimal(Detail);
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
                                        dtPass.Rows[i]["Comfort"] = string.Empty;
                                        dtPass.Rows[i]["SSRCodeComfort"] = string.Empty;
                                        dtPass.Rows[i]["PriceComfort"] = 0.00;
                                    }
                                    else
                                    {
                                        dtPass.Rows[i]["Comfort"] = args[2];
                                        dtPass.Rows[i]["SSRCodeComfort"] = args[3];
                                        DataRow[] result = dtComfort.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            dtPass.Rows[i]["PriceComfort"] = Convert.ToDecimal(Detail);
                                        }
                                        //dtPass.Rows[i]["PriceComfort"] = Convert.ToDecimal(Detail);
                                    }
                                }
                                Session["dtGridPass"] = dtPass;
                                Session["qtyComfort"] = (Convert.ToInt32(args[1]) + qtyComfort);
                                gvPassenger.DataSource = dtPass;
                                gvPassenger.DataBind();
                            }
                            else
                            {
                                gvPassenger.JSProperties["cp_result"] = "The quantity of Comfort Kit must be less or equal to total number of passenger.";
                                return;
                            }
                        }
                    }
                    else if (args[0] == "Clear")
                    {
                        if (first == 2)
                        {
                            DataTable dtdefault = (DataTable)Session["dtdefaultBundle"];
                            if (dtdefault != null && dtdefault.Rows.Count > 0)
                            {
                                DataTable dtBaggage = (DataTable)Session["dtBaggageDepart"];

                                DataRow[] result = dtBaggage.Select("SSRCode = '" + dtdefault.Rows[0]["SYSValue"] + "'");
                                foreach (DataRow row in result)
                                {
                                    dtPass.Rows[Convert.ToInt16(args[1])]["Baggage"] = row[3];
                                    dtPass.Rows[Convert.ToInt16(args[1])]["SSRCodeBaggage"] = row[0];
                                    Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                }
                                dtPass.Rows[Convert.ToInt16(args[1])]["PriceBaggage"] = Convert.ToDecimal(Detail);

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

                                dtPass.Rows[Convert.ToInt16(args[1])]["Duty"] = string.Empty;
                                dtPass.Rows[Convert.ToInt16(args[1])]["SSRCodeDuty"] = string.Empty;
                                dtPass.Rows[Convert.ToInt16(args[1])]["PriceDuty"] = 0.00;

                                dtPass.Rows[Convert.ToInt16(args[1])]["Comfort"] = string.Empty;
                                dtPass.Rows[Convert.ToInt16(args[1])]["SSRCodeComfort"] = string.Empty;
                                dtPass.Rows[Convert.ToInt16(args[1])]["PriceComfort"] = 0.00;

                                Session["dtGridPass"] = dtPass;
                                gvPassenger.DataSource = dtPass;
                                gvPassenger.DataBind();
                            }
                        }
                        else
                        {
                            dtPass.Rows[Convert.ToInt16(args[1])]["Baggage"] = string.Empty;
                            dtPass.Rows[Convert.ToInt16(args[1])]["SSRCodeBaggage"] = string.Empty;
                            dtPass.Rows[Convert.ToInt16(args[1])]["PriceBaggage"] = 0.00;

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

                            dtPass.Rows[Convert.ToInt16(args[1])]["Duty"] = string.Empty;
                            dtPass.Rows[Convert.ToInt16(args[1])]["SSRCodeDuty"] = string.Empty;
                            dtPass.Rows[Convert.ToInt16(args[1])]["PriceDuty"] = 0.00;

                            dtPass.Rows[Convert.ToInt16(args[1])]["Comfort"] = string.Empty;
                            dtPass.Rows[Convert.ToInt16(args[1])]["SSRCodeComfort"] = string.Empty;
                            dtPass.Rows[Convert.ToInt16(args[1])]["PriceComfort"] = 0.00;

                            Session["dtGridPass"] = dtPass;
                            gvPassenger.DataSource = dtPass;
                            gvPassenger.DataBind();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void gvPassenger2_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
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
                if (Session["dtGridPass2"] == null)
                {
                    GetPassengerList2(Session["TransID"].ToString());
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
                                        if (first == 2)
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
                                                }
                                                dtPass2.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                            }
                                        }
                                        else
                                        {
                                            dtPass2.Rows[i]["Baggage"] = string.Empty;
                                            dtPass2.Rows[i]["SSRCodeBaggage"] = string.Empty;
                                            dtPass2.Rows[i]["PriceBaggage"] = 0.00;
                                        }
                                    }
                                    else
                                    {

                                        dtPass2.Rows[i]["Baggage"] = args[2];
                                        dtPass2.Rows[i]["SSRCodeBaggage"] = args[3];
                                        DataRow[] result2 = dtBaggage.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result2)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            dtPass2.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
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
                                            if (first == 2)
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
                                                    }
                                                    dtPass2.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                                }
                                            }
                                            else
                                            {
                                                dtPass2.Rows[i]["Baggage"] = string.Empty;
                                                dtPass2.Rows[i]["SSRCodeBaggage"] = string.Empty;
                                                dtPass2.Rows[i]["PriceBaggage"] = 0.00;
                                            }
                                        }
                                        else
                                        {

                                            dtPass2.Rows[i]["Baggage"] = args[2];
                                            dtPass2.Rows[i]["SSRCodeBaggage"] = args[3];
                                            DataRow[] result = dtBaggage.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                dtPass2.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                            }
                                            //dtPass2.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                        }
                                    }
                                    Session["dtGridPass2"] = dtPass2;
                                    Session["qtyBaggage2"] = (Convert.ToInt32(args[1]) + qtyBaggage2);
                                    gvPassenger2.DataSource = dtPass2;
                                    gvPassenger2.DataBind();
                                }
                                else
                                {
                                    gvPassenger2.JSProperties["cp_result"] = "The quantity of Baggage must be less or equal to total number of passenger.";
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
                                        dtPass2.Rows[i]["Meal"] = string.Empty;
                                        dtPass2.Rows[i]["SSRCodeMeal"] = string.Empty;
                                        dtPass2.Rows[i]["PriceMeal"] = 0.00;
                                    }
                                    else
                                    {
                                        dtPass2.Rows[i]["Meal"] = args[2];
                                        dtPass2.Rows[i]["SSRCodeMeal"] = args[3];
                                        DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            dtPass2.Rows[i]["PriceMeal"] = Convert.ToDecimal(Detail);
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
                                            dtPass2.Rows[i]["Meal"] = string.Empty;
                                            dtPass2.Rows[i]["SSRCodeMeal"] = string.Empty;
                                            dtPass2.Rows[i]["PriceMeal"] = 0.00;
                                        }
                                        else
                                        {
                                            dtPass2.Rows[i]["Meal"] = args[2];
                                            dtPass2.Rows[i]["SSRCodeMeal"] = args[3];
                                            DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                dtPass2.Rows[i]["PriceMeal"] = Convert.ToDecimal(Detail);
                                            }
                                            //dtPass2.Rows[i]["PriceMeal"] = Convert.ToDecimal(Detail);
                                        }
                                    }
                                    if (Session["dtDrinkReturn"] == null)
                                    {
                                        Session["dtGridPass2"] = dtPass2;
                                        Session["qtyMeal2"] = (Convert.ToInt32(args[1]) + qtyMeal2);
                                        gvPassenger2.DataSource = dtPass2;
                                        gvPassenger2.DataBind();
                                    }
                                }
                                else
                                {
                                    gvPassenger2.JSProperties["cp_result"] = "The quantity of Meal must be less or equal to total number of passenger.";
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
                                    if (args[2].ToString() == ",")
                                    {
                                        dtPass2.Rows[i]["Drink"] = string.Empty;
                                        dtPass2.Rows[i]["SSRCodeDrink"] = string.Empty;
                                        dtPass2.Rows[i]["PriceDrink"] = 0.00;
                                    }
                                    else
                                    {
                                        string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                        for (int c = 0; c < drinkcode.Length; c++)
                                        {
                                            DataRow[] result = dtDrink.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                            if (result.Length > 0)
                                            {
                                                foreach (DataRow row in result)
                                                {
                                                    Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                    dtPass2.Rows[i]["PriceDrink"] = Convert.ToDecimal(Detail);
                                                    dtPass2.Rows[i]["Drink"] = row[3];
                                                    dtPass2.Rows[i]["SSRCodeDrink"] = row[0];
                                                }
                                                break;
                                            }
                                        }
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
                                            dtPass2.Rows[i]["Drink"] = string.Empty;
                                            dtPass2.Rows[i]["SSRCodeDrink"] = string.Empty;
                                            dtPass2.Rows[i]["PriceDrink"] = 0.00;
                                        }
                                        else
                                        {
                                            string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                            for (int c = 0; c < drinkcode.Length; c++)
                                            {
                                                DataRow[] result = dtDrink.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                                if (result.Length > 0)
                                                {
                                                    foreach (DataRow row in result)
                                                    {
                                                        Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                        dtPass2.Rows[i]["PriceDrink"] = Convert.ToDecimal(Detail);
                                                        dtPass2.Rows[i]["Drink"] = row[3];
                                                        dtPass2.Rows[i]["SSRCodeDrink"] = row[0];
                                                    }
                                                    break;
                                                }
                                            }
                                            //dtPass2.Rows[i]["PriceMeal"] = Convert.ToDecimal(Detail);
                                        }
                                    }
                                    Session["dtGridPass2"] = dtPass2;
                                    Session["qtyMeal2"] = (Convert.ToInt32(args[1]) + qtyMeal2);
                                    Session["qtyDrink2"] = (Convert.ToInt32(args[1]) + qtyDrink2);
                                    gvPassenger2.DataSource = dtPass2;
                                    gvPassenger2.DataBind();
                                }
                                else
                                {
                                    gvPassenger2.JSProperties["cp_result"] = "The quantity of Meal must be less or equal to total number of passenger.";
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
                                        dtPass2.Rows[i]["Meal1"] = string.Empty;
                                        dtPass2.Rows[i]["SSRCodeMeal1"] = string.Empty;
                                        dtPass2.Rows[i]["PriceMeal1"] = 0.00;
                                    }
                                    else
                                    {
                                        dtPass2.Rows[i]["Meal1"] = args[2];
                                        dtPass2.Rows[i]["SSRCodeMeal1"] = args[3];
                                        DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            dtPass2.Rows[i]["PriceMeal1"] = Convert.ToDecimal(Detail);
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
                                            dtPass2.Rows[i]["Meal1"] = string.Empty;
                                            dtPass2.Rows[i]["SSRCodeMeal1"] = string.Empty;
                                            dtPass2.Rows[i]["PriceMeal1"] = 0.00;
                                        }
                                        else
                                        {
                                            dtPass2.Rows[i]["Meal1"] = args[2];
                                            dtPass2.Rows[i]["SSRCodeMeal1"] = args[3];
                                            DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                dtPass2.Rows[i]["PriceMeal1"] = Convert.ToDecimal(Detail);
                                            }
                                        }
                                        //dtPass2.Rows[i]["PriceMeal1"] = Convert.ToDecimal(Detail);
                                    }
                                    if (Session["dtDrinkReturn2"] == null)
                                    {
                                        Session["dtGridPass2"] = dtPass2;
                                        Session["qtyMeal21"] = (Convert.ToInt32(args[1]) + qtyMeal21);
                                        gvPassenger2.DataSource = dtPass2;
                                        gvPassenger2.DataBind();
                                    }
                                }
                                else
                                {
                                    gvPassenger2.JSProperties["cp_result"] = "The quantity of Meal 2 must be less or equal to total number of passenger.";
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
                                    if (args[2].ToString() == ",")
                                    {
                                        dtPass2.Rows[i]["Drink1"] = string.Empty;
                                        dtPass2.Rows[i]["SSRCodeDrink1"] = string.Empty;
                                        dtPass2.Rows[i]["PriceDrink1"] = 0.00;
                                    }
                                    else
                                    {
                                        string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                        for (int c = 0; c < drinkcode.Length; c++)
                                        {
                                            DataRow[] result = dtDrink.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                            if (result.Length > 0)
                                            {
                                                foreach (DataRow row in result)
                                                {
                                                    Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                    dtPass2.Rows[i]["PriceDrink1"] = Convert.ToDecimal(Detail);
                                                    dtPass2.Rows[i]["Drink1"] = row[3];
                                                    dtPass2.Rows[i]["SSRCodeDrink1"] = row[0];
                                                }
                                                break;
                                            }
                                        }
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
                                            dtPass2.Rows[i]["Drink1"] = string.Empty;
                                            dtPass2.Rows[i]["SSRCodeDrink1"] = string.Empty;
                                            dtPass2.Rows[i]["PriceDrink1"] = 0.00;
                                        }
                                        else
                                        {
                                            string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                            for (int c = 0; c < drinkcode.Length; c++)
                                            {
                                                DataRow[] result = dtDrink.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                                if (result.Length > 0)
                                                {
                                                    foreach (DataRow row in result)
                                                    {
                                                        Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                        dtPass2.Rows[i]["PriceDrink1"] = Convert.ToDecimal(Detail);
                                                        dtPass2.Rows[i]["Drink1"] = row[3];
                                                        dtPass2.Rows[i]["SSRCodeDrink1"] = row[0];
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                        //dtPass2.Rows[i]["PriceMeal1"] = Convert.ToDecimal(Detail);
                                    }
                                    Session["dtGridPass2"] = dtPass2;
                                    Session["qtyMeal21"] = (Convert.ToInt32(args[1]) + qtyMeal21);
                                    Session["qtyDrink21"] = (Convert.ToInt32(args[1]) + qtyDrink21);
                                    gvPassenger2.DataSource = dtPass2;
                                    gvPassenger2.DataBind();
                                }
                                else
                                {
                                    gvPassenger2.JSProperties["cp_result"] = "The quantity of Meal 2 must be less or equal to total number of passenger.";
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
                                        dtPass2.Rows[i]["Sport"] = string.Empty;
                                        dtPass2.Rows[i]["SSRCodeSport"] = string.Empty;
                                        dtPass2.Rows[i]["PriceSport"] = 0.00;
                                    }
                                    else
                                    {
                                        dtPass2.Rows[i]["Sport"] = args[2];
                                        dtPass2.Rows[i]["SSRCodeSport"] = args[3];
                                        DataRow[] result = dtSport.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            dtPass2.Rows[i]["PriceSport"] = Convert.ToDecimal(Detail);
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
                                            dtPass2.Rows[i]["Sport"] = string.Empty;
                                            dtPass2.Rows[i]["SSRCodeSport"] = string.Empty;
                                            dtPass2.Rows[i]["PriceSport"] = 0.00;
                                        }
                                        else
                                        {
                                            dtPass2.Rows[i]["Sport"] = args[2];
                                            dtPass2.Rows[i]["SSRCodeSport"] = args[3];
                                            DataRow[] result = dtSport.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                dtPass2.Rows[i]["PriceSport"] = Convert.ToDecimal(Detail);
                                            }
                                            //dtPass2.Rows[i]["PriceSport"] = Convert.ToDecimal(Detail);
                                        }
                                    }
                                    Session["dtGridPass2"] = dtPass2;
                                    Session["qtySport2"] = Convert.ToInt32(args[1] + qtySport2);
                                    gvPassenger2.DataSource = dtPass2;
                                    gvPassenger2.DataBind();
                                }
                                else
                                {
                                    gvPassenger2.JSProperties["cp_result"] = "The quantity of Sport must be less or equal to total number of passenger.";
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
                            gvPassenger2.JSProperties["cp_result"] = "The quantity of Duty Free must be less or equal to total number of passenger.";
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
                                    dtPass2.Rows[i]["Comfort"] = string.Empty;
                                    dtPass2.Rows[i]["SSRCodeComfort"] = string.Empty;
                                    dtPass2.Rows[i]["PriceComfort"] = 0.00;
                                }
                                else
                                {
                                    dtPass2.Rows[i]["Comfort"] = args[2];
                                    dtPass2.Rows[i]["SSRCodeComfort"] = args[3];
                                    DataRow[] result = dtComfort.Select("SSRCode = '" + args[3] + "'");
                                    foreach (DataRow row in result)
                                    {
                                        Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                        dtPass2.Rows[i]["PriceComfort"] = Convert.ToDecimal(Detail);
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
                                        dtPass2.Rows[i]["Comfort"] = string.Empty;
                                        dtPass2.Rows[i]["SSRCodeComfort"] = string.Empty;
                                        dtPass2.Rows[i]["PriceComfort"] = 0.00;
                                    }
                                    else
                                    {
                                        dtPass2.Rows[i]["Comfort"] = args[2];
                                        dtPass2.Rows[i]["SSRCodeComfort"] = args[3];
                                        DataRow[] result = dtComfort.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            dtPass2.Rows[i]["PriceComfort"] = Convert.ToDecimal(Detail);
                                        }
                                    }
                                    //dtPass2.Rows[i]["PriceComfort"] = Convert.ToDecimal(Detail);

                                }
                                Session["dtGridPass2"] = dtPass2;
                                Session["qtyComfort2"] = (Convert.ToInt32(args[1]) + qtyComfort2);
                                gvPassenger2.DataSource = dtPass2;
                                gvPassenger2.DataBind();
                            }
                            else
                            {
                                gvPassenger2.JSProperties["cp_result"] = "The quantity of Comfort Kit Free must be less or equal to total number of passenger.";
                                return;
                            }
                        }
                    }
                    else if (args[0] == "Clear")
                    {
                        if (first == 2)
                        {
                            DataTable dtdefault = (DataTable)Session["dtdefaultBundle"];
                            if (dtdefault != null && dtdefault.Rows.Count > 0)
                            {
                                DataTable dtBaggage = (DataTable)Session["dtBaggageDepart"];

                                DataRow[] result = dtBaggage.Select("SSRCode = '" + dtdefault.Rows[0]["SYSValue"] + "'");
                                foreach (DataRow row in result)
                                {
                                    dtPass2.Rows[Convert.ToInt16(args[1])]["Baggage"] = row[3];
                                    dtPass2.Rows[Convert.ToInt16(args[1])]["SSRCodeBaggage"] = row[0];
                                    Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                }
                                dtPass2.Rows[Convert.ToInt16(args[1])]["PriceBaggage"] = Convert.ToDecimal(Detail);

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

                                dtPass2.Rows[Convert.ToInt16(args[1])]["Duty"] = string.Empty;
                                dtPass2.Rows[Convert.ToInt16(args[1])]["SSRCodeDuty"] = string.Empty;
                                dtPass2.Rows[Convert.ToInt16(args[1])]["PriceDuty"] = 0.00;

                                dtPass2.Rows[Convert.ToInt16(args[1])]["Comfort"] = string.Empty;
                                dtPass2.Rows[Convert.ToInt16(args[1])]["SSRCodeComfort"] = string.Empty;
                                dtPass2.Rows[Convert.ToInt16(args[1])]["PriceComfort"] = 0.00;

                                Session["dtGridPass2"] = dtPass2;
                                gvPassenger2.DataSource = dtPass2;
                                gvPassenger2.DataBind();
                            }
                        }
                        else
                        {
                            dtPass2.Rows[Convert.ToInt16(args[1])]["Baggage"] = string.Empty;
                            dtPass2.Rows[Convert.ToInt16(args[1])]["SSRCodeBaggage"] = string.Empty;
                            dtPass2.Rows[Convert.ToInt16(args[1])]["PriceBaggage"] = 0.00;

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

                            dtPass2.Rows[Convert.ToInt16(args[1])]["Duty"] = string.Empty;
                            dtPass2.Rows[Convert.ToInt16(args[1])]["SSRCodeDuty"] = string.Empty;
                            dtPass2.Rows[Convert.ToInt16(args[1])]["PriceDuty"] = 0.00;

                            dtPass2.Rows[Convert.ToInt16(args[1])]["Comfort"] = string.Empty;
                            dtPass2.Rows[Convert.ToInt16(args[1])]["SSRCodeComfort"] = string.Empty;
                            dtPass2.Rows[Convert.ToInt16(args[1])]["PriceComfort"] = 0.00;

                            Session["dtGridPass2"] = dtPass2;
                            gvPassenger2.DataSource = dtPass2;
                            gvPassenger2.DataBind();
                        }
                    }
                }

                //BindLabel();
                //SetMaxValue2();
            }
            catch (Exception ex)
            {
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }
        #endregion

        #region "Function and Procedure"
        protected string GetSellSSR(string signature)
        {
            HttpCookie cookie = Request.Cookies["cookieSearchcondition"];
            ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking(signature);
            BookingControl bookingControl = new BookingControl();

            ABS.Navitaire.BookingManager.Booking responseBookingFromState = bookingControl.GetBookingFromState(signature);
            string xml = GetXMLString(responseBookingFromState);
            if (responseBookingFromState == null || responseBookingFromState.Journeys.Length <= 0)
            {
                Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.InvalidPage);
                return "";
            }

            GetSSRAvailabilityForBookingResponse response = apiBooking.GetSSRAvailabilityForBooking(responseBookingFromState, signature);
            string xmlresponse = GetXMLString(response);
            if (response != null)
            {
                ArrayList BaggageCode = new ArrayList();
                ArrayList BaggageFee = new ArrayList();

                ArrayList SportCode = new ArrayList();
                ArrayList SportFee = new ArrayList();

                ArrayList ComfortCode = new ArrayList();
                ArrayList ComfortFee = new ArrayList();
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

                ArrayList SportCode2 = new ArrayList();
                ArrayList SportFee2 = new ArrayList();

                ArrayList ComfortCode2 = new ArrayList();
                ArrayList ComfortFee2 = new ArrayList();
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

                int d1 = 0, d2 = 0, d3 = 0, d4 = 0;

                List<BookingTransactionDetail> lstTransDetail = new List<BookingTransactionDetail>();
                string depart1 = "", depart2 = "", transit1 = "", transit2 = "", return1 = "", return2 = "";

                if (HttpContext.Current.Session["HashMain"] != null)
                {
                    Hashtable ht = (Hashtable)HttpContext.Current.Session["HashMain"];
                    lstTransDetail = objBooking.GetAllBK_TRANSDTLFilterAll(ht["TransID"].ToString(), 0);
                    if (lstTransDetail == null)
                    {
                        if (Session["Chgsave"] != null)
                        {
                            ArrayList save = (ArrayList)Session["Chgsave"];
                            lstTransDetail = objBooking.GetAllBK_TRANSDTLFilterAll(save[1].ToString(), 0);
                        }
                    }
                    if (lstTransDetail != null)
                    {
                        for (int x = 0; x < lstTransDetail.Count; x++)
                        {
                            if (x == 0)
                            {
                                depart1 = lstTransDetail[x].Origin.Trim();
                                transit1 = lstTransDetail[x].Transit.Trim();
                                return1 = lstTransDetail[x].Destination.Trim();
                            }
                            else if (x == 1)
                            {
                                depart2 = lstTransDetail[x].Origin.Trim();
                                transit2 = lstTransDetail[x].Transit.Trim();
                                return2 = lstTransDetail[x].Destination.Trim();
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }

                String Currency = "";
                SSRAvailabilityForBookingResponse ssrAvailabilityResponseForBooking = response.SSRAvailabilityForBookingResponse;
                Session["GetssrAvailabilityResponseForBooking"] = response;
                xml = GetXMLString(ssrAvailabilityResponseForBooking);

                if (ssrAvailabilityResponseForBooking != null && ssrAvailabilityResponseForBooking.SSRSegmentList.Length != 0)
                {
                    //this entire section is simply to test the computation of SSR pricing for Meal fee in particular as segment based or leg based depending on the results
                    if (cookie != null)
                    {
                        bool IsDepart = false, IsDepartTransit = false, IsDepartTransit2 = false, IsReturn = false, IsReturnTransit = false, IsReturnTransit2 = false;
                        foreach (ABS.Navitaire.BookingManager.SSRSegment SSRSegment in ssrAvailabilityResponseForBooking.SSRSegmentList)
                        {
                            IsDepart = false; IsDepartTransit = false; IsDepartTransit2 = false; IsReturn = false; IsReturnTransit = false; IsReturnTransit2 = false;
                            if (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == return1)
                            {
                                IsDepart = true;
                            }
                            else if (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == transit1)
                            {
                                IsDepartTransit = true;
                                Session["transit"] = true;
                                Session["departTransit"] = transit1;
                                Session["transitdepart"] = true;

                            }
                            else if (SSRSegment.LegKey.DepartureStation == transit1 && SSRSegment.LegKey.ArrivalStation == return1)
                            {
                                IsDepartTransit2 = true;
                                Session["transit"] = true;
                                Session["departTransit"] = transit1;
                                Session["transitdepart"] = true;

                            }
                            else if (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == transit2)
                            {
                                IsDepartTransit = true;
                                Session["transit"] = true;
                                Session["departdifferent"] = true;
                                Session["departTransit"] = transit2;
                                Session["transitdepart"] = true;

                            }
                            else if (SSRSegment.LegKey.DepartureStation == transit2 && SSRSegment.LegKey.ArrivalStation == return1)
                            {
                                IsDepartTransit2 = true;
                                Session["transit"] = true;
                                Session["departdifferent"] = true;
                                Session["departTransit"] = transit2;
                                Session["transitdepart"] = true;

                            }
                            else if (SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == return2)
                            {
                                IsReturn = true;

                            }
                            else if (SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == transit2)
                            {
                                IsReturnTransit = true;
                                Session["transit"] = true;
                                Session["returnTransit"] = transit2;
                                Session["transitreturn"] = true;

                            }
                            else if (SSRSegment.LegKey.DepartureStation == transit2 && SSRSegment.LegKey.ArrivalStation == return2)
                            {
                                IsReturnTransit2 = true;
                                Session["transit"] = true;
                                Session["returnTransit"] = transit2;
                                Session["transitreturn"] = true;

                            }
                            else if (SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == transit1)
                            {
                                IsReturnTransit = true;
                                Session["transit"] = true;
                                Session["returndifferent"] = true;
                                Session["returnTransit"] = transit1;
                                Session["transitreturn"] = true;

                            }
                            else if (SSRSegment.LegKey.DepartureStation == transit1 && SSRSegment.LegKey.ArrivalStation == return2)
                            {
                                IsReturnTransit2 = true;
                                Session["transit"] = true;
                                Session["returndifferent"] = true;
                                Session["returnTransit"] = transit1;
                                Session["transitreturn"] = true;

                            }

                            for (int j = 0; j < SSRSegment.AvailablePaxSSRList.Length; j++) //for SSR index
                            {
                                if (SSRSegment.AvailablePaxSSRList[j].PaxSSRPriceList.Length > 0) //skip the cases where the SSR is totally free, no build in pricing
                                {
                                    for (int l = 0; l < SSRSegment.AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        decimal SSRAmt = 0;

                                        //to compute the meal fee pricing
                                        for (int k = 0; k < SSRSegment.AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (SSRSegment.AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                SSRAmt += SSRSegment.AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                        }
                                        if (SSRSegment.AvailablePaxSSRList[j].SSRCode.Equals("PBAA") || SSRSegment.AvailablePaxSSRList[j].SSRCode.Equals("PBAB") || SSRSegment.AvailablePaxSSRList[j].SSRCode.Equals("PBAC") || SSRSegment.AvailablePaxSSRList[j].SSRCode.Equals("PBAD") || SSRSegment.AvailablePaxSSRList[j].SSRCode.Equals("PBAF"))
                                        {
                                            if (IsDepart == true || IsDepartTransit == true || IsDepartTransit2 == true)
                                            {
                                                int index = BaggageCode.IndexOf(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                                                if (index < 0)
                                                {
                                                    BaggageCode.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                                                    BaggageFee.Add(SSRAmt);
                                                }
                                                else
                                                {
                                                    BaggageFee[index] = Convert.ToDecimal(BaggageFee[index]) + SSRAmt;
                                                }
                                            }
                                            else if (IsReturn == true || IsReturnTransit == true || IsReturnTransit2 == true)
                                            {
                                                int index = BaggageCode2.IndexOf(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                                                if (index < 0)
                                                {
                                                    BaggageCode2.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                                                    BaggageFee2.Add(SSRAmt);
                                                }
                                                else
                                                {
                                                    BaggageFee2[index] = Convert.ToDecimal(BaggageFee2[index]) + SSRAmt;
                                                }
                                            }
                                        }
                                        else if (SSRSegment.AvailablePaxSSRList[j].SSRCode.Equals("PSEA") || SSRSegment.AvailablePaxSSRList[j].SSRCode.Equals("PSEB") || SSRSegment.AvailablePaxSSRList[j].SSRCode.Equals("PSEC") || SSRSegment.AvailablePaxSSRList[j].SSRCode.Equals("PSED") || SSRSegment.AvailablePaxSSRList[j].SSRCode.Equals("PSEF"))
                                        {
                                            if (IsDepart == true || IsDepartTransit == true || IsDepartTransit2 == true)
                                            {
                                                int index = SportCode.IndexOf(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                                                if (index < 0)
                                                {
                                                    SportCode.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                                                    SportFee.Add(SSRAmt);
                                                }
                                                else
                                                {
                                                    SportFee[index] = Convert.ToDecimal(SportFee[index]) + SSRAmt;
                                                }
                                            }
                                            else if (IsReturn == true || IsReturnTransit == true || IsReturnTransit2 == true)
                                            {
                                                int index = SportCode2.IndexOf(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                                                if (index < 0)
                                                {
                                                    SportCode2.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                                                    SportFee2.Add(SSRAmt);
                                                }
                                                else
                                                {
                                                    SportFee2[index] = Convert.ToDecimal(SportFee2[index]) + SSRAmt;
                                                }
                                            }

                                        }
                                        else if (SSRSegment.AvailablePaxSSRList[j].SSRCode.Equals("PCMK"))
                                        {
                                            if (IsDepart == true || IsDepartTransit == true || IsDepartTransit2 == true)
                                            {
                                                int index = ComfortCode.IndexOf(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                                                if (index < 0)
                                                {
                                                    ComfortCode.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                                                    ComfortImage.Add("http://www.airasia.com/images/common/sky-ssr/img_" + SSRSegment.AvailablePaxSSRList[j].SSRCode + ".png");
                                                    ComfortFee.Add(SSRAmt);
                                                }
                                                else
                                                {
                                                    ComfortFee[index] = Convert.ToDecimal(ComfortFee[index]) + SSRAmt;
                                                }
                                            }
                                            else if (IsReturn == true || IsReturnTransit == true || IsReturnTransit2 == true)
                                            {
                                                int index = ComfortCode2.IndexOf(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                                                if (index < 0)
                                                {
                                                    ComfortCode2.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                                                    ComfortImage2.Add("http://www.airasia.com/images/common/sky-ssr/img_" + SSRSegment.AvailablePaxSSRList[j].SSRCode + ".png");
                                                    ComfortFee2.Add(SSRAmt);
                                                }
                                                else
                                                {
                                                    ComfortFee2[index] = Convert.ToDecimal(ComfortFee2[index]) + SSRAmt;
                                                }
                                            }
                                        }
                                        else if (SSRSegment.AvailablePaxSSRList[j].SSRLegList.Length > 0)
                                        {
                                            if (SSRSegment.AvailablePaxSSRList[j].Available > Convert.ToInt16(cookie.Values["PaxNum"]))
                                            {
                                                if (IsDepart == true || IsDepartTransit == true)
                                                {
                                                    MealCode.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                                                    MealImage.Add("http://www.airasia.com/images/common/sky-ssr/img_" + SSRSegment.AvailablePaxSSRList[j].SSRCode + ".png");
                                                    MealFee.Add(SSRAmt);
                                                }
                                                else if (IsDepartTransit2 == true)
                                                {
                                                    MealCode1.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                                                    MealImage1.Add("http://www.airasia.com/images/common/sky-ssr/img_" + SSRSegment.AvailablePaxSSRList[j].SSRCode + ".png");
                                                    MealFee1.Add(SSRAmt);
                                                }
                                                else if (IsReturn == true || IsReturnTransit == true)
                                                {
                                                    MealCode2.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                                                    MealImage2.Add("http://www.airasia.com/images/common/sky-ssr/img_" + SSRSegment.AvailablePaxSSRList[j].SSRCode + ".png");
                                                    MealFee2.Add(SSRAmt);
                                                }
                                                else if (IsReturnTransit2 == true)
                                                {
                                                    MealCode21.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                                                    MealImage21.Add("http://www.airasia.com/images/common/sky-ssr/img_" + SSRSegment.AvailablePaxSSRList[j].SSRCode + ".png");
                                                    MealFee21.Add(SSRAmt);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }

                    GetSSRAvailabilityForBookingResponse responses = apiBooking.GetSSRAvailabilityForBooking(responseBookingFromState, signature);
                    string xmlresponses = GetXMLString(responses);
                    if (responses != null)
                    {
                        SSRAvailabilityForBookingResponse ssrAvailabilityResponseForBookings = responses.SSRAvailabilityForBookingResponse;
                        Session["GetssrAvailabilityResponseForBooking"] = responses;
                        xml = GetXMLString(ssrAvailabilityResponseForBookings);

                        if (ssrAvailabilityResponseForBookings != null && ssrAvailabilityResponseForBookings.SSRSegmentList.Length != 0)
                        {
                            //this entire section is simply to test the computation of SSR pricing for Meal fee in particular as segment based or leg based depending on the results
                            if (cookie != null)
                            {

                                bool IsDepart = false, IsDepartTransit = false, IsDepartTransit2 = false, IsReturn = false, IsReturnTransit = false, IsReturnTransit2 = false;
                                foreach (ABS.Navitaire.BookingManager.SSRSegment SSRSegment in ssrAvailabilityResponseForBookings.SSRSegmentList)
                                {
                                    IsDepart = false; IsDepartTransit = false; IsDepartTransit2 = false; IsReturn = false; IsReturnTransit = false; IsReturnTransit2 = false;
                                    if (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == return1)
                                    {
                                        IsDepart = true;
                                    }
                                    else if (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == transit1)
                                    {
                                        IsDepartTransit = true;
                                        Session["transit"] = true;

                                    }
                                    else if (SSRSegment.LegKey.DepartureStation == transit1 && SSRSegment.LegKey.ArrivalStation == return1)
                                    {
                                        IsDepartTransit2 = true;
                                        Session["transit"] = true;

                                    }
                                    else if (SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == return2)
                                    {
                                        IsReturn = true;

                                    }
                                    else if (SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == transit2)
                                    {
                                        IsReturnTransit = true;
                                        Session["transit"] = true;

                                    }
                                    else if (SSRSegment.LegKey.DepartureStation == transit2 && SSRSegment.LegKey.ArrivalStation == return2)
                                    {
                                        IsReturnTransit2 = true;
                                        Session["transit"] = true;

                                    }

                                    d1 = 0; d2 = 0; d3 = 0; d4 = 0;
                                    for (int j = 0; j < SSRSegment.AvailablePaxSSRList.Length; j++) //for SSR index
                                    {
                                        if (SSRSegment.AvailablePaxSSRList[j].PaxSSRPriceList.Length > 0) //skip the cases where the SSR is totally free, no build in pricing
                                        {
                                            for (int l = 0; l < SSRSegment.AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                            {
                                                decimal SSRAmt = 0;

                                                //to compute the meal fee pricing
                                                bool IsNew = false;
                                                if (SSRSegment.AvailablePaxSSRList[j].SSRCode.Substring(0, 2) == "BW")
                                                {
                                                    if (d1 == 0)
                                                    {
                                                        IsNew = true;
                                                        d1 = 1;
                                                    }
                                                    else
                                                    {
                                                        IsNew = false;
                                                        d1 = 0;
                                                    }
                                                }
                                                else if (SSRSegment.AvailablePaxSSRList[j].SSRCode.Substring(0, 2) == "CD")
                                                {
                                                    if (d2 == 0)
                                                    {
                                                        IsNew = true;
                                                        d2 = 1;
                                                    }
                                                    else
                                                    {
                                                        IsNew = false;
                                                        d2 = 0;
                                                    }
                                                }
                                                else if (SSRSegment.AvailablePaxSSRList[j].SSRCode.Substring(0, 2) == "CF")
                                                {
                                                    if (d3 == 0)
                                                    {
                                                        IsNew = true;
                                                        d3 = 1;
                                                    }
                                                    else
                                                    {
                                                        IsNew = false;
                                                        d3 = 0;
                                                    }
                                                }
                                                else if (SSRSegment.AvailablePaxSSRList[j].SSRCode.Substring(0, 2) == "JC")
                                                {
                                                    if (d4 == 0)
                                                    {
                                                        IsNew = true;
                                                        d4 = 1;
                                                    }
                                                    else
                                                    {
                                                        IsNew = false;
                                                        d4 = 0;
                                                    }
                                                }

                                                if (IsNew)
                                                {
                                                    for (int k = 0; k < SSRSegment.AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                                    {
                                                        if (SSRSegment.AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                            SSRAmt += SSRSegment.AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                                    }
                                                    if (IsDepart == true || IsDepartTransit == true)
                                                    {
                                                        DrinkCode.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                                                        DrinkFee.Add(SSRAmt);
                                                    }
                                                    else if (IsDepartTransit2 == true)
                                                    {
                                                        DrinkCode1.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                                                        DrinkFee1.Add(SSRAmt);
                                                    }
                                                    else if (IsReturn == true || IsReturnTransit == true)
                                                    {
                                                        DrinkCode2.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                                                        DrinkFee2.Add(SSRAmt);
                                                    }
                                                    else if (IsReturnTransit2 == true)
                                                    {
                                                        DrinkCode21.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                                                        DrinkFee21.Add(SSRAmt);
                                                    }
                                                }

                                            }
                                        }
                                    }

                                }
                            }
                        }

                        if (cookie != null)
                        {
                            //
                            Currency = cookie.Values["Currency"];
                            Session["Currency"] = Currency;

                            bool IsDepart = false, IsDepartTransit = false, IsReturn = false, IsReturnTransit = false;
                            foreach (ABS.Navitaire.BookingManager.SSRSegment SSRSegment in ssrAvailabilityResponseForBooking.SSRSegmentList)
                            {
                                if (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == return1)
                                {
                                    IsDepart = true;
                                    InitializeForm(Currency, BaggageCode, BaggageFee, SportCode, SportFee, ComfortCode, ComfortFee, ComfortImage, MealCode, MealFee, MealImage, null, null, null, DutyCode, DutyFee, DutyImage, DrinkCode, DrinkFee, null, null);
                                }
                                else if (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == transit1)
                                {
                                    IsDepartTransit = true;
                                    InitializeForm(Currency, BaggageCode, BaggageFee, SportCode, SportFee, ComfortCode, ComfortFee, ComfortImage, MealCode, MealFee, MealImage, MealCode1, MealFee1, MealImage1, DutyCode, DutyFee, DutyImage, DrinkCode, DrinkFee, DrinkCode1, DrinkFee1);

                                }
                                else if (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == transit2)
                                {
                                    IsDepartTransit = true;
                                    InitializeForm(Currency, BaggageCode, BaggageFee, SportCode, SportFee, ComfortCode, ComfortFee, ComfortImage, MealCode, MealFee, MealImage, MealCode1, MealFee1, MealImage1, DutyCode, DutyFee, DutyImage, DrinkCode, DrinkFee, DrinkCode1, DrinkFee1);

                                }
                                else if (SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == return2)
                                {
                                    IsReturn = true;
                                    InitializeForm2(Currency, BaggageCode2, BaggageFee2, SportCode2, SportFee2, ComfortCode2, ComfortFee2, ComfortImage2, MealCode2, MealFee2, MealImage2, null, null, null, DutyCode2, DutyFee2, DutyImage2, DrinkCode2, DrinkFee2, null, null);
                                }
                                else if (SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == transit2)
                                {
                                    IsReturnTransit = true;
                                    InitializeForm2(Currency, BaggageCode2, BaggageFee2, SportCode2, SportFee2, ComfortCode2, ComfortFee2, ComfortImage2, MealCode2, MealFee2, MealImage2, MealCode21, MealFee21, MealImage21, DutyCode2, DutyFee2, DutyImage2, DrinkCode2, DrinkFee2, DrinkCode21, DrinkFee21);
                                }
                                else if (SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == transit1)
                                {
                                    IsReturnTransit = true;
                                    InitializeForm2(Currency, BaggageCode2, BaggageFee2, SportCode2, SportFee2, ComfortCode2, ComfortFee2, ComfortImage2, MealCode2, MealFee2, MealImage2, MealCode21, MealFee21, MealImage21, DutyCode2, DutyFee2, DutyImage2, DrinkCode2, DrinkFee2, DrinkCode21, DrinkFee21);
                                }
                            }

                        }

                    }
                    else
                    {
                        Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.InvalidPage);
                    }


                }
            }
            return "";
        }

        protected void GetPassengerList(string TransID)
        {
            try
            {
                HttpCookie cookie = Request.Cookies["cookieTemFlight"];
                DataTable dtPass = new DataTable();
                if (Session["dtGridPass"] != null)
                {
                    dtPass = (DataTable)Session["dtGridPass"];
                }
                else
                {
                    if (Request.QueryString["change"] != null)
                    {
                        dtPass = objBooking.GetAllBK_PASSENGERLISTSSRDataTableChangedepart(TransID);
                        if (dtPass == null)
                        {
                            if (Session["Chgsave"] != null)
                            {
                                ArrayList save = (ArrayList)Session["Chgsave"];
                                DataTable dtTransDetail = objBooking.dtTransDetail();
                                if (HttpContext.Current.Session["TransDetailAll"] != null)
                                    dtTransDetail = (DataTable)HttpContext.Current.Session["TransDetailAll"];
                                DataTable dtPassClone = new DataTable();

                                dtPassClone = objBooking.GetAllBK_PASSENGERLISTSSRDataTableChangedepart(save[1].ToString());
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
                }
                if (dtPass != null && dtPass.Rows.Count > 0)
                {
                    if (cookie != null)
                    {
                        if (cookie.Values["ReturnID"] != "")
                        {
                            gvPassenger.DataSource = dtPass;
                            gvPassenger.DataBind();

                            gvPassenger2.DataSource = dtPass;
                            gvPassenger2.DataBind();
                        }
                        else
                        {
                            gvPassenger.DataSource = dtPass;
                            gvPassenger.DataBind();


                        }
                    }
                }
                else
                {
                    Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.InvalidPage);
                }
                Session["dtGridPass"] = dtPass;
            }
            catch (Exception ex)
            {
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }



        protected void GetPassengerList2(string TransID)
        {
            try
            {
                HttpCookie cookie = Request.Cookies["cookieTemFlight"];
                DataTable dtPass2 = new DataTable();
                if (Session["dtGridPass2"] != null)
                {
                    dtPass2 = (DataTable)Session["dtGridPass2"];
                }
                else
                {
                    if (Request.QueryString["change"] != null)
                    {
                        dtPass2 = objBooking.GetAllBK_PASSENGERLISTSSRDataTableChangereturn(TransID, "Return");
                        if (dtPass2 == null)
                        {
                            if (Session["Chgsave"] != null)
                            {
                                ArrayList save = (ArrayList)Session["Chgsave"];
                                DataTable dtTransDetail = objBooking.dtTransDetail();
                                if (HttpContext.Current.Session["TransDetailAll"] != null)
                                    dtTransDetail = (DataTable)HttpContext.Current.Session["TransDetailAll"];
                                DataTable dtPassClone = new DataTable();
                                dtPassClone = objBooking.GetAllBK_PASSENGERLISTSSRDataTableChangereturn(save[1].ToString(), "Return");
                                dtPass2 = dtPassClone.Clone();
                                int x = 0;
                                for (int i = 0; i < dtPassClone.Rows.Count; i++)
                                {
                                    if (dtPassClone.Rows[i]["PNR"].ToString() == dtTransDetail.Rows[0]["RecordLocator"].ToString())
                                    {
                                        dtPass2.ImportRow(dtPassClone.Rows[i]);
                                        dtPass2.Rows[x]["TransID"] = save[0].ToString();
                                        dtPass2.Rows[x]["SeqNo"] = x + 1;
                                        x += 1;
                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        dtPass2 = objBooking.GetAllBK_PASSENGERLISTSSRDataTable(TransID);
                    }
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
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void ValidatePopup(object sender, CallbackEventArgs e)
        {
            List<BookingTransactionDetail> lstTransDetail = new List<BookingTransactionDetail>();
            string depart1 = "", depart2 = "", transit1 = "", transit2 = "", return1 = "", return2 = "";

            if (HttpContext.Current.Session["HashMain"] != null)
            {
                Hashtable ht = (Hashtable)HttpContext.Current.Session["HashMain"];
                lstTransDetail = objBooking.GetAllBK_TRANSDTLFilterAll(ht["TransID"].ToString(), 0);
                if (lstTransDetail == null)
                {
                    if (Session["Chgsave"] != null)
                    {
                        ArrayList save = (ArrayList)Session["Chgsave"];
                        lstTransDetail = objBooking.GetAllBK_TRANSDTLFilterAll(save[1].ToString(), 0);
                    }
                }
                for (int x = 0; x < lstTransDetail.Count; x++)
                {
                    if (x == 0)
                    {
                        depart1 = lstTransDetail[x].Origin.Trim();
                        if (Convert.ToBoolean(Session["departdifferent"]) == true && lstTransDetail[x].Transit.Trim() == "")
                        {
                            transit1 = Session["departTransit"].ToString();
                        }
                        else
                        {
                            transit1 = lstTransDetail[x].Transit.Trim();
                        }
                        return1 = lstTransDetail[x].Destination.Trim();
                    }
                    else if (x == 1)
                    {
                        depart2 = lstTransDetail[x].Origin.Trim();
                        if (Convert.ToBoolean(Session["returndifferent"]) == true && lstTransDetail[x].Transit.Trim() == "")
                        {
                            transit2 = Session["returnTransit"].ToString();
                        }
                        else
                        {
                            transit2 = lstTransDetail[x].Transit.Trim();
                        }
                        //transit2 = lstTransDetail[x].Transit.Trim();
                        return2 = lstTransDetail[x].Destination.Trim();
                    }
                    else
                    {
                        break;
                    }
                }
            }

            dtList1.Columns.Add("SSRCode");
            dtList1.Columns.Add("PassengerID");
            dtList1.Columns.Add("PassengerNo");
            dtList1.Columns.Add("Origin");
            dtList1.Columns.Add("Destination");

            dtList2.Columns.Add("SSRCode");
            dtList2.Columns.Add("PassengerID");
            dtList2.Columns.Add("PassengerNo");
            dtList2.Columns.Add("Origin");
            dtList2.Columns.Add("Destination");

            dtList1t.Columns.Add("SSRCode");
            dtList1t.Columns.Add("PassengerID");
            dtList1t.Columns.Add("PassengerNo");
            dtList1t.Columns.Add("Origin");
            dtList1t.Columns.Add("Destination");

            dtList2t.Columns.Add("SSRCode");
            dtList2t.Columns.Add("PassengerID");
            dtList2t.Columns.Add("PassengerNo");
            dtList2t.Columns.Add("Origin");
            dtList2t.Columns.Add("Destination");

            listbk_transssrinfo = new List<Bk_transssr>();
            listbk_transssrinfo1 = new List<Bk_transssr>();
            listbk_transssrinfo2 = new List<Bk_transssr>();
            listbk_transssrinfo1t = new List<Bk_transssr>();
            listbk_transssrinfo2t = new List<Bk_transssr>();

            MessageList msgList = new MessageList();
            ArrayList aMsgList = new ArrayList();
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
                    if (Convert.ToBoolean(Session["transit"]) == true)
                    {
                        if (Convert.ToBoolean(Session["transitdepart"]) == true)
                        {
                            foreach (DataRow dr in dataClass.Rows)
                            {
                                if (dr["SSRCodeBaggage"].ToString() != "" || dr["SSRCodeSport"].ToString() != ""
                                           || dr["SSRCodeComfort"].ToString() != "" || dr["SSRCodeDuty"].ToString() != "" || dr["SSRCodeMeal"].ToString() != "" || dr["SSRCodeDrink"].ToString() != "")
                                {
                                    if (Session["dtDrinkDepart"] != null)
                                    {
                                        for (int i = 0; i <= 6; i++)
                                        {
                                            BK_TRANSSSRInfo = new Bk_transssr();
                                            BK_TRANSSSRInfo.TransID = dr["TransID"].ToString();
                                            BK_TRANSSSRInfo.RecordLocator = dr["PNR"].ToString();
                                            BK_TRANSSSRInfo.CarrierCode = dr["CarrierCode"].ToString();
                                            BK_TRANSSSRInfo.FlightNo = dr["FlightNo"].ToString();
                                            BK_TRANSSSRInfo.PassengerID = (Convert.ToInt32(dr["PassengerID"])).ToString();
                                            BK_TRANSSSRInfo.SeqNo = (Convert.ToInt32(dr["SeqNo"]) - 1);
                                            BK_TRANSSSRInfo.SubSeqNo = i;
                                            if (i == 0)
                                            {
                                                if (dr["SSRCodeBaggage"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeBaggage"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceBaggage"];
                                                }

                                            }
                                            else if (i == 1)
                                            {
                                                if (dr["SSRCodeMeal"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeMeal"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceMeal"];
                                                }
                                            }
                                            else if (i == 2)
                                            {
                                                if (dr["SSRCodeSport"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeSport"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceSport"];
                                                }
                                            }
                                            else if (i == 3)
                                            {
                                                if (dr["SSRCodeComfort"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeComfort"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceComfort"];
                                                }
                                            }
                                            else if (i == 4)
                                            {
                                                if (dr["SSRCodeDuty"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDuty"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDuty"];
                                                }
                                            }
                                            else if (i == 5)
                                            {
                                                if (dr["SSRCodeDrink"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDrink"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDrink"];
                                                }
                                            }
                                            BK_TRANSSSRInfo.Origin = dr["Origin"].ToString();
                                            if (transit1 != "")
                                                BK_TRANSSSRInfo.Destination = Session["departTransit"].ToString();
                                            else
                                                BK_TRANSSSRInfo.Destination = dr["Destination"].ToString();

                                            listbk_transssrinfo.Add(BK_TRANSSSRInfo);
                                            listbk_transssrinfo1.Add(BK_TRANSSSRInfo);

                                        }
                                    }
                                    else
                                    {
                                        for (int i = 0; i <= 5; i++)
                                        {
                                            BK_TRANSSSRInfo = new Bk_transssr();
                                            BK_TRANSSSRInfo.TransID = dr["TransID"].ToString();
                                            BK_TRANSSSRInfo.RecordLocator = dr["PNR"].ToString();
                                            BK_TRANSSSRInfo.CarrierCode = dr["CarrierCode"].ToString();
                                            BK_TRANSSSRInfo.FlightNo = dr["FlightNo"].ToString();
                                            BK_TRANSSSRInfo.PassengerID = (Convert.ToInt32(dr["PassengerID"])).ToString();
                                            BK_TRANSSSRInfo.SeqNo = (Convert.ToInt32(dr["SeqNo"]) - 1);
                                            BK_TRANSSSRInfo.SubSeqNo = i;
                                            if (i == 0)
                                            {
                                                if (dr["SSRCodeBaggage"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeBaggage"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceBaggage"];
                                                }
                                            }
                                            else if (i == 1)
                                            {
                                                if (dr["SSRCodeMeal"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeMeal"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceMeal"];
                                                }
                                            }
                                            else if (i == 2)
                                            {
                                                if (dr["SSRCodeSport"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeSport"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceSport"];
                                                }
                                            }
                                            else if (i == 3)
                                            {
                                                if (dr["SSRCodeComfort"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeComfort"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceComfort"];
                                                }
                                            }
                                            else if (i == 4)
                                            {
                                                if (dr["SSRCodeDuty"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDuty"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDuty"];
                                                }
                                            }

                                            BK_TRANSSSRInfo.Origin = dr["Origin"].ToString();
                                            if (transit1 != "")
                                                BK_TRANSSSRInfo.Destination = Session["departTransit"].ToString();
                                            else
                                                BK_TRANSSSRInfo.Destination = dr["Destination"].ToString();

                                            listbk_transssrinfo.Add(BK_TRANSSSRInfo);
                                            listbk_transssrinfo1.Add(BK_TRANSSSRInfo);

                                        }
                                    }
                                }


                                if (transit1 != "")
                                {
                                    if (dr["SSRCodeBaggage"].ToString() != "" || dr["SSRCodeSport"].ToString() != ""
                                           || dr["SSRCodeComfort"].ToString() != "" || dr["SSRCodeDuty"].ToString() != "" || dr["SSRCodeMeal1"].ToString() != "")
                                    {
                                        for (int i = 0; i <= 6; i++)
                                        {
                                            BK_TRANSSSRInfo = new Bk_transssr();
                                            BK_TRANSSSRInfo.TransID = dr["TransID"].ToString();
                                            BK_TRANSSSRInfo.RecordLocator = dr["PNR"].ToString();
                                            BK_TRANSSSRInfo.CarrierCode = dr["CarrierCode"].ToString();
                                            BK_TRANSSSRInfo.FlightNo = dr["FlightNo"].ToString();
                                            BK_TRANSSSRInfo.PassengerID = (Convert.ToInt32(dr["PassengerID"])).ToString();
                                            BK_TRANSSSRInfo.SeqNo = (Convert.ToInt32(dr["SeqNo"]) - 1);
                                            BK_TRANSSSRInfo.SubSeqNo = i;
                                            if (i == 0)
                                            {
                                                if (dr["SSRCodeBaggage"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeBaggage"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceBaggage"];
                                                }

                                            }
                                            else if (i == 1)
                                            {
                                                if (dr["SSRCodeMeal1"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeMeal1"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceMeal1"];
                                                }
                                            }
                                            else if (i == 2)
                                            {
                                                if (dr["SSRCodeSport"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeSport"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceSport"];
                                                }
                                            }
                                            else if (i == 3)
                                            {
                                                if (dr["SSRCodeComfort"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeComfort"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceComfort"];
                                                }
                                            }
                                            else if (i == 4)
                                            {
                                                if (dr["SSRCodeDuty"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDuty"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDuty"];
                                                }
                                            }
                                            else if (i == 5)
                                            {
                                                if (dr["SSRCodeDrink1"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDrink1"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDrink1"];
                                                    BK_TRANSSSRInfo.Origin = Session["departTransit"].ToString();
                                                    BK_TRANSSSRInfo.Destination = dr["Destination"].ToString();
                                                }
                                            }
                                            BK_TRANSSSRInfo.Destination = dr["Destination"].ToString();
                                            if (transit1 != "")
                                                BK_TRANSSSRInfo.Origin = Session["departTransit"].ToString();
                                            else
                                                BK_TRANSSSRInfo.Origin = dr["Origin"].ToString();

                                            if (transit1 != "") listbk_transssrinfo.Add(BK_TRANSSSRInfo);
                                            if (transit1 != "") listbk_transssrinfo1t.Add(BK_TRANSSSRInfo);
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            foreach (DataRow dr in dataClass.Rows)
                            {
                                if (dr["SSRCodeBaggage"].ToString() != "" || dr["SSRCodeMeal"].ToString() != "" || dr["SSRCodeSport"].ToString() != ""
                                             || dr["SSRCodeComfort"].ToString() != "" || dr["SSRCodeDuty"].ToString() != "")
                                {
                                    for (int i = 0; i <= 6; i++)
                                    {
                                        BK_TRANSSSRInfo = new Bk_transssr();
                                        BK_TRANSSSRInfo.TransID = dr["TransID"].ToString();
                                        BK_TRANSSSRInfo.RecordLocator = dr["PNR"].ToString();
                                        BK_TRANSSSRInfo.CarrierCode = dr["CarrierCode"].ToString();
                                        BK_TRANSSSRInfo.FlightNo = dr["FlightNo"].ToString();
                                        BK_TRANSSSRInfo.PassengerID = (Convert.ToInt32(dr["PassengerID"])).ToString();
                                        BK_TRANSSSRInfo.SeqNo = (Convert.ToInt32(dr["SeqNo"]) - 1);
                                        BK_TRANSSSRInfo.Origin = dr["Origin"].ToString();
                                        BK_TRANSSSRInfo.Destination = dr["Destination"].ToString();
                                        BK_TRANSSSRInfo.SubSeqNo = i;
                                        if (i == 0)
                                        {
                                            if (dr["SSRCodeBaggage"].ToString() != "")
                                            {
                                                BK_TRANSSSRInfo.SSRCode = dr["SSRCodeBaggage"].ToString();
                                                BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceBaggage"];
                                            }

                                        }
                                        else if (i == 1)
                                        {
                                            if (dr["SSRCodeMeal"].ToString() != "")
                                            {
                                                BK_TRANSSSRInfo.SSRCode = dr["SSRCodeMeal"].ToString();
                                                BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceMeal"];
                                            }
                                        }
                                        else if (i == 2)
                                        {
                                            if (dr["SSRCodeSport"].ToString() != "")
                                            {
                                                BK_TRANSSSRInfo.SSRCode = dr["SSRCodeSport"].ToString();
                                                BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceSport"];
                                            }
                                        }
                                        else if (i == 3)
                                        {
                                            if (dr["SSRCodeComfort"].ToString() != "")
                                            {
                                                BK_TRANSSSRInfo.SSRCode = dr["SSRCodeComfort"].ToString();
                                                BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceComfort"];
                                            }
                                        }
                                        else if (i == 4)
                                        {
                                            if (dr["SSRCodeDuty"].ToString() != "")
                                            {
                                                BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDuty"].ToString();
                                                BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDuty"];
                                            }
                                        }
                                        else if (i == 5)
                                        {
                                            if (dr["SSRCodeDrink"].ToString() != "")
                                            {
                                                BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDrink"].ToString();
                                                BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDrink"];
                                            }
                                        }
                                        listbk_transssrinfo.Add(BK_TRANSSSRInfo);
                                        listbk_transssrinfo1.Add(BK_TRANSSSRInfo);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (DataRow dr in dataClass.Rows)
                        {
                            if (dr["SSRCodeBaggage"].ToString() != "" || dr["SSRCodeMeal"].ToString() != "" || dr["SSRCodeSport"].ToString() != ""
                                         || dr["SSRCodeComfort"].ToString() != "" || dr["SSRCodeDuty"].ToString() != "")
                            {
                                for (int i = 0; i <= 6; i++)
                                {
                                    BK_TRANSSSRInfo = new Bk_transssr();
                                    BK_TRANSSSRInfo.TransID = dr["TransID"].ToString();
                                    BK_TRANSSSRInfo.RecordLocator = dr["PNR"].ToString();
                                    BK_TRANSSSRInfo.CarrierCode = dr["CarrierCode"].ToString();
                                    BK_TRANSSSRInfo.FlightNo = dr["FlightNo"].ToString();
                                    BK_TRANSSSRInfo.PassengerID = (Convert.ToInt32(dr["PassengerID"])).ToString();
                                    BK_TRANSSSRInfo.SeqNo = (Convert.ToInt32(dr["SeqNo"]) - 1);
                                    BK_TRANSSSRInfo.Origin = dr["Origin"].ToString();
                                    BK_TRANSSSRInfo.Destination = dr["Destination"].ToString();
                                    BK_TRANSSSRInfo.SubSeqNo = i;
                                    if (i == 0)
                                    {
                                        if (dr["SSRCodeBaggage"].ToString() != "")
                                        {
                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeBaggage"].ToString();
                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceBaggage"];
                                        }

                                    }
                                    else if (i == 1)
                                    {
                                        if (dr["SSRCodeMeal"].ToString() != "")
                                        {
                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeMeal"].ToString();
                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceMeal"];
                                        }
                                    }
                                    else if (i == 2)
                                    {
                                        if (dr["SSRCodeSport"].ToString() != "")
                                        {
                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeSport"].ToString();
                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceSport"];
                                        }
                                    }
                                    else if (i == 3)
                                    {
                                        if (dr["SSRCodeComfort"].ToString() != "")
                                        {
                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeComfort"].ToString();
                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceComfort"];
                                        }
                                    }
                                    else if (i == 4)
                                    {
                                        if (dr["SSRCodeDuty"].ToString() != "")
                                        {
                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDuty"].ToString();
                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDuty"];
                                        }
                                    }
                                    else if (i == 5)
                                    {
                                        if (dr["SSRCodeDrink"].ToString() != "")
                                        {
                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDrink"].ToString();
                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDrink"];
                                        }
                                    }
                                    listbk_transssrinfo.Add(BK_TRANSSSRInfo);
                                    listbk_transssrinfo1.Add(BK_TRANSSSRInfo);
                                }
                            }
                        }
                    }

                    HttpCookie cookie = Request.Cookies["cookieSearchcondition"];
                    if (cookie != null)
                    {

                        if (cookie.Values["ifOneWay"] != "TRUE")
                        {
                            dataClass = new DataTable();
                            if (Convert.ToBoolean(Session["transit"]) == true)
                            {
                                if (Convert.ToBoolean(Session["transitreturn"]) == true)
                                {
                                    dataClass = (DataTable)HttpContext.Current.Session["dtGridPass2"];
                                    foreach (DataRow dr in dataClass.Rows)
                                    {
                                        if (dr["SSRCodeBaggage"].ToString() != "" || dr["SSRCodeMeal"].ToString() != "" || dr["SSRCodeSport"].ToString() != ""
                                            || dr["SSRCodeComfort"].ToString() != "" || dr["SSRCodeDuty"].ToString() != "")
                                        {
                                            for (int i = 0; i <= 6; i++)
                                            {
                                                BK_TRANSSSRInfo = new Bk_transssr();
                                                BK_TRANSSSRInfo.TransID = dr["TransID"].ToString();
                                                BK_TRANSSSRInfo.RecordLocator = dr["PNR"].ToString();
                                                BK_TRANSSSRInfo.CarrierCode = dr["CarrierCode"].ToString();
                                                BK_TRANSSSRInfo.FlightNo = dr["FlightNo"].ToString();
                                                BK_TRANSSSRInfo.PassengerID = (Convert.ToInt32(dr["PassengerID"])).ToString();
                                                BK_TRANSSSRInfo.SeqNo = (Convert.ToInt32(dr["SeqNo"]) - 1);
                                                BK_TRANSSSRInfo.SubSeqNo = i;
                                                if (i == 0)
                                                {
                                                    if (dr["SSRCodeBaggage"].ToString() != "")
                                                    {
                                                        BK_TRANSSSRInfo.SSRCode = dr["SSRCodeBaggage"].ToString();
                                                        BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceBaggage"];
                                                    }

                                                }
                                                else if (i == 1)
                                                {
                                                    if (dr["SSRCodeMeal"].ToString() != "")
                                                    {
                                                        BK_TRANSSSRInfo.SSRCode = dr["SSRCodeMeal"].ToString();
                                                        BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceMeal"];
                                                    }
                                                }
                                                else if (i == 2)
                                                {
                                                    if (dr["SSRCodeSport"].ToString() != "")
                                                    {
                                                        BK_TRANSSSRInfo.SSRCode = dr["SSRCodeSport"].ToString();
                                                        BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceSport"];
                                                    }
                                                }
                                                else if (i == 3)
                                                {
                                                    if (dr["SSRCodeComfort"].ToString() != "")
                                                    {
                                                        BK_TRANSSSRInfo.SSRCode = dr["SSRCodeComfort"].ToString();
                                                        BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceComfort"];
                                                    }
                                                }
                                                else if (i == 4)
                                                {
                                                    if (dr["SSRCodeDuty"].ToString() != "")
                                                    {
                                                        BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDuty"].ToString();
                                                        BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDuty"];
                                                    }
                                                }
                                                else if (i == 6)
                                                {
                                                    if (dr["SSRCodeDrink"].ToString() != "")
                                                    {
                                                        BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDrink"].ToString();
                                                        BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDrink"];
                                                    }
                                                }

                                                BK_TRANSSSRInfo.Origin = dr["Destination"].ToString();
                                                if (transit2 != "")
                                                    BK_TRANSSSRInfo.Destination = Session["returnTransit"].ToString();
                                                else
                                                    BK_TRANSSSRInfo.Destination = dr["Origin"].ToString();

                                                listbk_transssrinfo.Add(BK_TRANSSSRInfo);
                                                listbk_transssrinfo2.Add(BK_TRANSSSRInfo);
                                            }
                                        }
                                    }
                                    if (transit2 != "")
                                    {

                                        foreach (DataRow dr in dataClass.Rows)
                                        {
                                            if (dr["SSRCodeBaggage"].ToString() != "" || dr["SSRCodeMeal1"].ToString() != "" || dr["SSRCodeSport"].ToString() != ""
                                                || dr["SSRCodeComfort"].ToString() != "" || dr["SSRCodeDuty"].ToString() != "")
                                            {
                                                for (int i = 0; i <= 6; i++)
                                                {
                                                    BK_TRANSSSRInfo = new Bk_transssr();
                                                    BK_TRANSSSRInfo.TransID = dr["TransID"].ToString();
                                                    BK_TRANSSSRInfo.RecordLocator = dr["PNR"].ToString();
                                                    BK_TRANSSSRInfo.CarrierCode = dr["CarrierCode"].ToString();
                                                    BK_TRANSSSRInfo.FlightNo = dr["FlightNo"].ToString();
                                                    BK_TRANSSSRInfo.PassengerID = (Convert.ToInt32(dr["PassengerID"])).ToString();
                                                    BK_TRANSSSRInfo.SeqNo = (Convert.ToInt32(dr["SeqNo"]) - 1);
                                                    BK_TRANSSSRInfo.SubSeqNo = i;
                                                    if (i == 0)
                                                    {
                                                        if (dr["SSRCodeBaggage"].ToString() != "")
                                                        {
                                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeBaggage"].ToString();
                                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceBaggage"];
                                                        }

                                                    }
                                                    else if (i == 1)
                                                    {
                                                        if (dr["SSRCodeMeal1"].ToString() != "")
                                                        {
                                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeMeal1"].ToString();
                                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceMeal1"];
                                                        }
                                                    }
                                                    else if (i == 2)
                                                    {
                                                        if (dr["SSRCodeSport"].ToString() != "")
                                                        {
                                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeSport"].ToString();
                                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceSport"];
                                                        }
                                                    }
                                                    else if (i == 3)
                                                    {
                                                        if (dr["SSRCodeComfort"].ToString() != "")
                                                        {
                                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeComfort"].ToString();
                                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceComfort"];
                                                        }
                                                    }
                                                    else if (i == 4)
                                                    {
                                                        if (dr["SSRCodeDuty"].ToString() != "")
                                                        {
                                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDuty"].ToString();
                                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDuty"];
                                                        }
                                                    }
                                                    else if (i == 5)
                                                    {
                                                        if (dr["SSRCodeDrink1"].ToString() != "")
                                                        {
                                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDrink1"].ToString();
                                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDrink1"];
                                                        }
                                                    }


                                                    BK_TRANSSSRInfo.Destination = dr["Origin"].ToString();
                                                    if (transit2 != "")
                                                        BK_TRANSSSRInfo.Origin = Session["returnTransit"].ToString();
                                                    else
                                                        BK_TRANSSSRInfo.Origin = dr["Destination"].ToString();

                                                    if (transit2 != "") listbk_transssrinfo.Add(BK_TRANSSSRInfo);
                                                    if (transit2 != "") listbk_transssrinfo2t.Add(BK_TRANSSSRInfo);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    dataClass = (DataTable)HttpContext.Current.Session["dtGridPass2"];
                                    foreach (DataRow dr in dataClass.Rows)
                                    {
                                        if (dr["SSRCodeBaggage"].ToString() != "" || dr["SSRCodeMeal"].ToString() != "" || dr["SSRCodeSport"].ToString() != ""
                                             || dr["SSRCodeComfort"].ToString() != "" || dr["SSRCodeDuty"].ToString() != "")
                                        {
                                            for (int i = 0; i <= 6; i++)
                                            {
                                                BK_TRANSSSRInfo = new Bk_transssr();
                                                BK_TRANSSSRInfo.TransID = dr["TransID"].ToString();
                                                BK_TRANSSSRInfo.RecordLocator = dr["PNR"].ToString();
                                                BK_TRANSSSRInfo.CarrierCode = dr["CarrierCode"].ToString();
                                                BK_TRANSSSRInfo.FlightNo = dr["FlightNo"].ToString();
                                                BK_TRANSSSRInfo.Origin = dr["Destination"].ToString();
                                                BK_TRANSSSRInfo.Destination = dr["Origin"].ToString();
                                                BK_TRANSSSRInfo.PassengerID = (Convert.ToInt32(dr["PassengerID"])).ToString();
                                                BK_TRANSSSRInfo.SeqNo = (Convert.ToInt32(dr["SeqNo"]) - 1);
                                                BK_TRANSSSRInfo.SubSeqNo = i;
                                                if (i == 0)
                                                {
                                                    if (dr["SSRCodeBaggage"].ToString() != "")
                                                    {
                                                        BK_TRANSSSRInfo.SSRCode = dr["SSRCodeBaggage"].ToString();
                                                        BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceBaggage"];
                                                    }

                                                }
                                                else if (i == 1)
                                                {
                                                    if (dr["SSRCodeMeal"].ToString() != "")
                                                    {
                                                        BK_TRANSSSRInfo.SSRCode = dr["SSRCodeMeal"].ToString();
                                                        BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceMeal"];
                                                    }
                                                }
                                                else if (i == 2)
                                                {
                                                    if (dr["SSRCodeSport"].ToString() != "")
                                                    {
                                                        BK_TRANSSSRInfo.SSRCode = dr["SSRCodeSport"].ToString();
                                                        BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceSport"];
                                                    }
                                                }
                                                else if (i == 3)
                                                {
                                                    if (dr["SSRCodeComfort"].ToString() != "")
                                                    {
                                                        BK_TRANSSSRInfo.SSRCode = dr["SSRCodeComfort"].ToString();
                                                        BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceComfort"];
                                                    }
                                                }
                                                else if (i == 4)
                                                {
                                                    if (dr["SSRCodeDuty"].ToString() != "")
                                                    {
                                                        BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDuty"].ToString();
                                                        BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDuty"];
                                                    }
                                                }
                                                else if (i == 5)
                                                {
                                                    if (dr["SSRCodeDrink"].ToString() != "")
                                                    {
                                                        BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDrink"].ToString();
                                                        BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDrink"];
                                                    }
                                                }

                                                listbk_transssrinfo.Add(BK_TRANSSSRInfo);
                                                listbk_transssrinfo2.Add(BK_TRANSSSRInfo);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                dataClass = (DataTable)HttpContext.Current.Session["dtGridPass2"];
                                foreach (DataRow dr in dataClass.Rows)
                                {
                                    if (dr["SSRCodeBaggage"].ToString() != "" || dr["SSRCodeMeal"].ToString() != "" || dr["SSRCodeSport"].ToString() != ""
                                         || dr["SSRCodeComfort"].ToString() != "" || dr["SSRCodeDuty"].ToString() != "")
                                    {
                                        for (int i = 0; i <= 6; i++)
                                        {
                                            BK_TRANSSSRInfo = new Bk_transssr();
                                            BK_TRANSSSRInfo.TransID = dr["TransID"].ToString();
                                            BK_TRANSSSRInfo.RecordLocator = dr["PNR"].ToString();
                                            BK_TRANSSSRInfo.CarrierCode = dr["CarrierCode"].ToString();
                                            BK_TRANSSSRInfo.FlightNo = dr["FlightNo"].ToString();
                                            BK_TRANSSSRInfo.Origin = dr["Destination"].ToString();
                                            BK_TRANSSSRInfo.Destination = dr["Origin"].ToString();
                                            BK_TRANSSSRInfo.PassengerID = (Convert.ToInt32(dr["PassengerID"])).ToString();
                                            BK_TRANSSSRInfo.SeqNo = (Convert.ToInt32(dr["SeqNo"]) - 1);
                                            BK_TRANSSSRInfo.SubSeqNo = i;
                                            if (i == 0)
                                            {
                                                if (dr["SSRCodeBaggage"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeBaggage"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceBaggage"];
                                                }

                                            }
                                            else if (i == 1)
                                            {
                                                if (dr["SSRCodeMeal"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeMeal"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceMeal"];
                                                }
                                            }
                                            else if (i == 2)
                                            {
                                                if (dr["SSRCodeSport"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeSport"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceSport"];
                                                }
                                            }
                                            else if (i == 3)
                                            {
                                                if (dr["SSRCodeComfort"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeComfort"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceComfort"];
                                                }
                                            }
                                            else if (i == 4)
                                            {
                                                if (dr["SSRCodeDuty"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDuty"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDuty"];
                                                }
                                            }
                                            else if (i == 5)
                                            {
                                                if (dr["SSRCodeDrink"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDrink"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDrink"];
                                                }
                                            }

                                            listbk_transssrinfo.Add(BK_TRANSSSRInfo);
                                            listbk_transssrinfo2.Add(BK_TRANSSSRInfo);
                                        }
                                    }
                                }
                            }

                        }
                    }
                    listbk_transssrinfo1.RemoveAll(BK_TRANSSSRInfo => BK_TRANSSSRInfo.SSRCode == null || BK_TRANSSSRInfo.SSRCode == "");
                    listbk_transssrinfo2.RemoveAll(BK_TRANSSSRInfo => BK_TRANSSSRInfo.SSRCode == null || BK_TRANSSSRInfo.SSRCode == "");
                    if (Convert.ToBoolean(Session["transit"]) == true)
                    {
                        listbk_transssrinfo1t.RemoveAll(BK_TRANSSSRInfo => BK_TRANSSSRInfo.SSRCode == null || BK_TRANSSSRInfo.SSRCode == "");
                        listbk_transssrinfo2t.RemoveAll(BK_TRANSSSRInfo => BK_TRANSSSRInfo.SSRCode == null || BK_TRANSSSRInfo.SSRCode == "");
                    }
                    listbk_transssrinfo.RemoveAll(BK_TRANSSSRInfo => BK_TRANSSSRInfo.SSRCode == null || BK_TRANSSSRInfo.SSRCode == "");

                    DataRow row = dtList1.NewRow();
                    foreach (Bk_transssr item in listbk_transssrinfo1)
                    {
                        dtList1.Rows.Add(item.SSRCode, item.PassengerID, item.SeqNo, item.Origin, item.Destination);
                    }

                    DataRow row2 = dtList2.NewRow();
                    foreach (Bk_transssr item in listbk_transssrinfo2)
                    {
                        dtList2.Rows.Add(item.SSRCode, item.PassengerID, item.SeqNo, item.Origin, item.Destination);
                    }
                    if (Convert.ToBoolean(Session["transit"]) == true)
                    {
                        DataRow rowt = dtList1t.NewRow();
                        foreach (Bk_transssr item in listbk_transssrinfo1t)
                        {
                            dtList1t.Rows.Add(item.SSRCode, item.PassengerID, item.SeqNo, item.Origin, item.Destination);
                        }

                        DataRow row2t = dtList2t.NewRow();
                        foreach (Bk_transssr item in listbk_transssrinfo2t)
                        {
                            dtList2t.Rows.Add(item.SSRCode, item.PassengerID, item.SeqNo, item.Origin, item.Destination);
                        }
                    }
                    object sumdtList1;
                    object sumdtList1t;
                    object sumdtList2;
                    object sumdtList2t;
                    DataTable dtdefaultBundledom = objBooking.GetDefaultBundle("DEFAULTDOMESTICBAGGAGE", Session["CarrierCode"].ToString());
                    DataTable dtdefaultBundleInt = objBooking.GetDefaultBundle("DEFAULTINTERNATIONALBAGGAGE", Session["CarrierCode"].ToString());
                    if ((dtdefaultBundledom != null && dtdefaultBundledom.Rows.Count > 0) || (dtdefaultBundleInt != null && dtdefaultBundleInt.Rows.Count > 0 && objGeneral.IsInternationalFlight(cookie.Values["Departure"].ToString(), cookie.Values["Arrival"].ToString(), Request.PhysicalApplicationPath)))
                    {
                        if (dtList1.Rows.Count > 0 && dtList2.Rows.Count > 0 && dtList1t.Rows.Count > 0 && dtList2t.Rows.Count > 0)
                        {
                            sumdtList1 = dtList1.Compute("Count(SSRCode)", "SSRCode LIKE 'PB%'");
                            sumdtList1t = dtList1t.Compute("Count(SSRCode)", "SSRCode LIKE 'PB%'");
                            sumdtList2 = dtList2.Compute("Count(SSRCode)", "SSRCode LIKE 'PB%'");
                            sumdtList2t = dtList2t.Compute("Count(SSRCode)", "SSRCode LIKE 'PB%'");

                            if ((Convert.ToDecimal(sumdtList1) == Convert.ToDecimal(cookie.Values["PaxNum"])) && (Convert.ToDecimal(sumdtList1t) == Convert.ToDecimal(cookie.Values["PaxNum"])) && (Convert.ToDecimal(sumdtList2) == Convert.ToDecimal(cookie.Values["PaxNum"])) && (Convert.ToDecimal(sumdtList2t) == Convert.ToDecimal(cookie.Values["PaxNum"])))
                            {
                                SellFlight(dtList1, dtList2, dtList1t, dtList2t, listbk_transssrinfo);
                            }
                            else
                            {
                                e.Result = "Cannot Add SSRs. The assigned Baggage is not equal with Total Passenger.";
                                return;
                            }
                        }
                        else if (dtList1.Rows.Count > 0 && dtList2.Rows.Count > 0 && dtList1t.Rows.Count > 0)
                        {
                            sumdtList1 = dtList1.Compute("Count(SSRCode)", "SSRCode LIKE 'PB%'");
                            sumdtList1t = dtList1t.Compute("Count(SSRCode)", "SSRCode LIKE 'PB%'");
                            sumdtList2 = dtList2.Compute("Count(SSRCode)", "SSRCode LIKE 'PB%'");

                            if ((Convert.ToDecimal(sumdtList1) == Convert.ToDecimal(cookie.Values["PaxNum"])) && (Convert.ToDecimal(sumdtList1t) == Convert.ToDecimal(cookie.Values["PaxNum"])) && (Convert.ToDecimal(sumdtList2) == Convert.ToDecimal(cookie.Values["PaxNum"])))
                            {
                                SellFlight(dtList1, dtList2, dtList1t, dtList2t, listbk_transssrinfo);
                            }
                            else
                            {
                                e.Result = "Cannot Add SSRs. The assigned Baggage is not equal with Total Passenger.";
                                return;
                            }
                        }
                        else if (dtList1.Rows.Count > 0 && dtList2.Rows.Count > 0 && dtList2t.Rows.Count > 0)
                        {
                            sumdtList1 = dtList1.Compute("Count(SSRCode)", "SSRCode LIKE 'PB%'");
                            sumdtList2 = dtList2.Compute("Count(SSRCode)", "SSRCode LIKE 'PB%'");
                            sumdtList2t = dtList2t.Compute("Count(SSRCode)", "SSRCode LIKE 'PB%'");

                            if ((Convert.ToDecimal(sumdtList1) == Convert.ToDecimal(cookie.Values["PaxNum"])) && (Convert.ToDecimal(sumdtList2) == Convert.ToDecimal(cookie.Values["PaxNum"])) && (Convert.ToDecimal(sumdtList2t) == Convert.ToDecimal(cookie.Values["PaxNum"])))
                            {
                                SellFlight(dtList1, dtList2, dtList1t, dtList2t, listbk_transssrinfo);
                            }
                            else
                            {
                                e.Result = "Cannot Add SSRs. The assigned Baggage is not equal with Total Passenger.";
                                return;
                            }
                        }
                        else if (dtList1.Rows.Count > 0 && dtList2.Rows.Count > 0)
                        {
                            sumdtList1 = dtList1.Compute("Count(SSRCode)", "SSRCode LIKE 'PB%'");
                            sumdtList2 = dtList2.Compute("Count(SSRCode)", "SSRCode LIKE 'PB%'");

                            if ((Convert.ToDecimal(sumdtList1) == Convert.ToDecimal(cookie.Values["PaxNum"])) && (Convert.ToDecimal(sumdtList2) == Convert.ToDecimal(cookie.Values["PaxNum"])))
                            {
                                SellFlight(dtList1, dtList2, dtList1t, dtList2t, listbk_transssrinfo);
                            }
                            else
                            {
                                e.Result = "Cannot Add SSRs. The assigned Baggage is not equal with Total Passenger.";
                                return;
                            }
                        }
                        else if (dtList1.Rows.Count > 0 && dtList1t.Rows.Count > 0)
                        {
                            sumdtList1 = dtList1.Compute("Count(SSRCode)", "SSRCode LIKE 'PB%'");
                            sumdtList1t = dtList1t.Compute("Count(SSRCode)", "SSRCode LIKE 'PB%'");

                            if ((Convert.ToDecimal(sumdtList1) == Convert.ToDecimal(cookie.Values["PaxNum"])) && (Convert.ToDecimal(sumdtList1t) == Convert.ToDecimal(cookie.Values["PaxNum"])))
                            {
                                SellFlight(dtList1, dtList2, dtList1t, dtList2t, listbk_transssrinfo);
                            }
                            else
                            {
                                e.Result = "Cannot Add SSRs. The assigned Baggage is not equal with Total Passenger.";
                                return;
                            }
                        }
                        else if (dtList1.Rows.Count > 0)
                        {
                            sumdtList1 = dtList1.Compute("Count(SSRCode)", "SSRCode LIKE 'PB%'");

                            if ((Convert.ToDecimal(sumdtList1) == Convert.ToDecimal(cookie.Values["PaxNum"])))
                            {
                                SellFlight(dtList1, dtList2, dtList1t, dtList2t, listbk_transssrinfo);
                            }
                            else
                            {
                                e.Result = "Cannot Add SSRs. The assigned Baggage is not equal with Total Passenger.";
                                return;
                            }
                        }
                    }
                    else
                    {
                        SellFlight(dtList1, dtList2, dtList1t, dtList2t, listbk_transssrinfo);
                    }


                    if (Request.QueryString["change"] != null)
                    {

                        if (Session["ChgbookHDRInfo"] != null && Session["ChglstbookDTLInfo"] != null)
                        {
                            UpdateAllBookingJourneyDetail();
                        }
                    }
                    e.Result = "";
                }
            }
            catch (Exception ex)
            {
                e.Result = msgList.Err100055;
                log.Error(this, ex);
            }
        }

        private void SellFlight(DataTable dtList1, DataTable dtList2, DataTable dtList1t, DataTable dtList2t, List<ABS.Logic.GroupBooking.Booking.Bk_transssr> listAll)
        {
            ClearSSRFeeValue();

            Session["totalcountpax"] = null;
            Decimal totalSSRdepart = 0;
            Decimal totalSSRReturn = 0;


            decimal TotSSRDepart = 0;
            decimal TotSSRReturn = 0;


            decimal TotalAmountGoing = 0;
            decimal TotalAmountReturn = 0;

            DataRow[] row1t;
            DataRow[] row2t;
            try
            {
                List<BookingTransactionDetail> listBookingDetail = new List<BookingTransactionDetail>();
                BookingTransactionDetail objBK_TRANSDTL_Infos;
                List<BookingTransactionDetail> objListBK_TRANSDTL_Infos = new List<BookingTransactionDetail>();
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

                if (HttpContext.Current.Session["dataClassTrans"] != null)
                {
                    DataTable dataClass = (DataTable)HttpContext.Current.Session["dataClassTrans"];
                    for (int iii = 0; iii < dataClass.Rows.Count; iii++)
                    {
                        SessionID = dataClass.Rows[iii]["SellSignature"].ToString();



                        totalSSRdepart = 0;
                        totalSSRReturn = 0;

                        //String SessionID = Session["signature"].ToString();
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


                            ABS.Navitaire.APIBooking APIBooking = new ABS.Navitaire.APIBooking(SessionID);
                            //ABS.Navitaire.BookingManager.GetAvailabilityResponse response = APIBooking.GetAvailability(temFlight.TemFlightArrival, Convert.ToDateTime(temFlight.TemFlightSta), temFlight.TemFlightCurrencyCode, temFlight.TemFlightDeparture, temFlight.TemFlightPaxNum, ref SessionID);
                            //ABS.Navitaire.BookingManager.GetAvailabilityResponse response2 = APIBooking.GetAvailability(temFlight2.TemFlightArrival, Convert.ToDateTime(temFlight2.TemFlightSta), temFlight2.TemFlightCurrencyCode, temFlight2.TemFlightDeparture, temFlight2.TemFlightPaxNum, ref SessionID);

                            GetSSRAvailabilityForBookingResponse ssrResponse = (GetSSRAvailabilityForBookingResponse)Session["GetssrAvailabilityResponseForBooking"];
                            if (ssrResponse != null)
                            {
                                DataRow[] row1;
                                DataRow[] row2;
                                if (Convert.ToBoolean(Session["transit"]) == true)
                                {
                                    if (iii == 0)
                                    {
                                        row1 = dtList1.Select("CONVERT(PassengerNo, System.Int32) >= 0 AND CONVERT(PassengerNo, System.Int32) < " + Convert.ToInt32(dataClass.Rows[iii]["Quantity"]));
                                        row2 = dtList2.Select("CONVERT(PassengerNo, System.Int32) >= 0 AND CONVERT(PassengerNo, System.Int32) < " + Convert.ToInt32(dataClass.Rows[iii]["Quantity"]));

                                        row1t = dtList1t.Select("CONVERT(PassengerNo, System.Int32) >= 0 AND CONVERT(PassengerNo, System.Int32) < " + Convert.ToInt32(dataClass.Rows[iii]["Quantity"]));
                                        row2t = dtList2t.Select("CONVERT(PassengerNo, System.Int32) >= 0 AND CONVERT(PassengerNo, System.Int32) < " + Convert.ToInt32(dataClass.Rows[iii]["Quantity"]));

                                    }
                                    else
                                    {
                                        if (Session["totalcountpax"] == null)
                                        {
                                            Session["totalcountpax"] = (Convert.ToInt32(dataClass.Rows[iii]["Quantity"]) + Convert.ToInt32(dataClass.Rows[iii - 1]["Quantity"]));
                                            row1 = dtList1.Select("CONVERT(PassengerNo, System.Int32) >= " + Convert.ToInt32(dataClass.Rows[iii - 1]["Quantity"]) + " AND CONVERT(PassengerNo, System.Int32) < " + Convert.ToInt32(Session["totalcountpax"]));

                                            row2 = dtList2.Select("CONVERT(PassengerNo, System.Int32) >= " + Convert.ToInt32(dataClass.Rows[iii - 1]["Quantity"]) + " AND CONVERT(PassengerNo, System.Int32) < " + Convert.ToInt32(Session["totalcountpax"]));

                                            row1t = dtList1t.Select("CONVERT(PassengerNo, System.Int32) >= " + Convert.ToInt32(dataClass.Rows[iii - 1]["Quantity"]) + " AND CONVERT(PassengerNo, System.Int32) < " + Convert.ToInt32(Session["totalcountpax"]));
                                            row2t = dtList2t.Select("CONVERT(PassengerNo, System.Int32) >= " + Convert.ToInt32(dataClass.Rows[iii - 1]["Quantity"]) + " AND CONVERT(PassengerNo, System.Int32) < " + Convert.ToInt32(Session["totalcountpax"]));
                                        }
                                        else
                                        {
                                            row1 = dtList1.Select("CONVERT(PassengerNo, System.Int32) >= " + Convert.ToInt32(Session["totalcountpax"]) + " AND CONVERT(PassengerNo, System.Int32)  < " + (Convert.ToInt32(dataClass.Rows[iii]["Quantity"]) + Convert.ToInt32(Session["totalcountpax"])));
                                            row2 = dtList2.Select("CONVERT(PassengerNo, System.Int32) >= " + Convert.ToInt32(Session["totalcountpax"]) + " AND CONVERT(PassengerNo, System.Int32)  < " + (Convert.ToInt32(dataClass.Rows[iii]["Quantity"]) + Convert.ToInt32(Session["totalcountpax"])));
                                            row1t = dtList1t.Select("CONVERT(PassengerNo, System.Int32) >= " + Convert.ToInt32(Session["totalcountpax"]) + " AND CONVERT(PassengerNo, System.Int32)  < " + (Convert.ToInt32(dataClass.Rows[iii]["Quantity"]) + Convert.ToInt32(Session["totalcountpax"])));
                                            row2t = dtList2t.Select("CONVERT(PassengerNo, System.Int32) >= " + Convert.ToInt32(Session["totalcountpax"]) + " AND CONVERT(PassengerNo, System.Int32)  < " + (Convert.ToInt32(dataClass.Rows[iii]["Quantity"]) + Convert.ToInt32(Session["totalcountpax"])));
                                            Session["totalcountpax"] = (Convert.ToInt32(dataClass.Rows[iii]["Quantity"]) + Convert.ToInt32(Session["totalcountpax"]));
                                            //Session["totalcountpax"] = (Convert.ToInt32(dataClass.Rows[iii]["Quantity"]) + Convert.ToInt32(Session["totalcountpax"]));
                                        }


                                    }
                                    if (Convert.ToBoolean(Session["back"]) != true)
                                    {
                                        if (Request.QueryString["change"] != null)
                                        {
                                            SellResponse responseSSR;
                                            if (row1.Length > 0 || row2.Length > 0 || row1t.Length > 0 || row2t.Length > 0) responseSSR = APIBooking.SellSSRTransit(SessionID, ssrResponse, row1, row2, row1t, row2t, false, listAll[0].RecordLocator, temFlight.TemFlightCurrencyCode, "change");
                                        }
                                        else
                                        {
                                            SellResponse responseSSR = APIBooking.SellSSRTransit(SessionID, ssrResponse, row1, row2, row1t, row2t, false);
                                        }
                                        Session["row1" + iii] = row1;
                                        Session["row2" + iii] = row2;
                                        Session["row1t" + iii] = row1t;
                                        Session["row2t" + iii] = row2t;
                                    }
                                    else
                                    {
                                        // UpdateSSRResponse responseSSR = APIBooking.ReSellSSRTransit(SessionID, response, response2, ssrResponse, row1, row2, row1t, row2t);
                                        CancelResponse responseCancel = APIBooking.CancelSSRTransit(SessionID, ssrResponse, Curr, (DataRow[])Session["row1" + iii], (DataRow[])Session["row2" + iii], (DataRow[])Session["row1t" + iii], (DataRow[])Session["row2t" + iii]);
                                        if (Request.QueryString["change"] != null)
                                        {
                                            SellResponse responseSSR;
                                            if (row1.Length > 0 || row2.Length > 0 || row1t.Length > 0 || row2t.Length > 0) responseSSR = APIBooking.SellSSRTransit(SessionID, ssrResponse, row1, row2, row1t, row2t, true, listAll[0].RecordLocator, temFlight.TemFlightCurrencyCode, "change");
                                        }
                                        else
                                        {
                                            SellResponse responseSSR = APIBooking.SellSSRTransit(SessionID, ssrResponse, row1, row2, row1t, row2t, true);
                                        }
                                        Session["row1" + iii] = row1;
                                        Session["row2" + iii] = row2;
                                        Session["row1t" + iii] = row1t;
                                        Session["row2t" + iii] = row2t;
                                    }
                                }
                                else
                                {
                                    if (iii == 0)
                                    {
                                        row1 = dtList1.Select("CONVERT(PassengerNo, System.Int32) >= 0 AND CONVERT(PassengerNo, System.Int32) < " + Convert.ToInt32(dataClass.Rows[iii]["Quantity"]));
                                        row2 = dtList2.Select("CONVERT(PassengerNo, System.Int32) >= 0 AND CONVERT(PassengerNo, System.Int32) < " + Convert.ToInt32(dataClass.Rows[iii]["Quantity"]));

                                    }
                                    else
                                    {
                                        if (Session["totalcountpax"] == null)
                                        {
                                            Session["totalcountpax"] = (Convert.ToInt32(dataClass.Rows[iii]["Quantity"]) + Convert.ToInt32(dataClass.Rows[iii - 1]["Quantity"]));
                                            row1 = dtList1.Select("CONVERT(PassengerNo, System.Int32) >= " + Convert.ToInt32(dataClass.Rows[iii - 1]["Quantity"]) + " AND CONVERT(PassengerNo, System.Int32) < " + Convert.ToInt32(Session["totalcountpax"]));
                                            row2 = dtList2.Select("CONVERT(PassengerNo, System.Int32) >= " + Convert.ToInt32(dataClass.Rows[iii - 1]["Quantity"]) + " AND CONVERT(PassengerNo, System.Int32) < " + Convert.ToInt32(Session["totalcountpax"]));
                                        }
                                        else
                                        {
                                            //Session["totalcountpax"] = (Convert.ToInt32(dataClass.Rows[iii]["Quantity"]) + Convert.ToInt32(Session["totalcountpax"]));
                                            row1 = dtList1.Select("CONVERT(PassengerNo, System.Int32) >= " + Convert.ToInt32(Session["totalcountpax"]) + " AND CONVERT(PassengerNo, System.Int32)  < " + (Convert.ToInt32(dataClass.Rows[iii]["Quantity"]) + Convert.ToInt32(Session["totalcountpax"])));
                                            row2 = dtList2.Select("CONVERT(PassengerNo, System.Int32) >= " + Convert.ToInt32(Session["totalcountpax"]) + " AND CONVERT(PassengerNo, System.Int32)  < " + (Convert.ToInt32(dataClass.Rows[iii]["Quantity"]) + Convert.ToInt32(Session["totalcountpax"])));
                                            Session["totalcountpax"] = (Convert.ToInt32(dataClass.Rows[iii]["Quantity"]) + Convert.ToInt32(Session["totalcountpax"]));
                                        }




                                    }
                                    if (Convert.ToBoolean(Session["back"]) != true)
                                    {
                                        if (Request.QueryString["change"] != null)
                                        {
                                            SellResponse responseSSR;
                                            if (row1.Length > 0 || row2.Length > 0) responseSSR = APIBooking.SellSSR(SessionID, ssrResponse, row1, row2, false, listAll[0].RecordLocator, temFlight.TemFlightCurrencyCode, "change");
                                        }
                                        else
                                        {
                                            SellResponse responseSSR = APIBooking.SellSSR(SessionID, ssrResponse, row1, row2, false);
                                        }

                                        Session["row1" + iii] = row1;
                                        Session["row2" + iii] = row2;
                                    }
                                    else
                                    {

                                        //ResellSSRResponse resellSSRresponse = APIBooking.ReSellSSR(SessionID);
                                        //ABS.Navitaire.BookingManager.Booking books = APIBooking.GetBookingFromState(SessionID);
                                        CancelResponse responseCancel = APIBooking.CancelSSR(SessionID, ssrResponse, Curr, (DataRow[])Session["row1" + iii], (DataRow[])Session["row2" + iii]);
                                        //UpdateSSRResponse responseSSR = APIBooking.UpdateSSR(SessionID, response, response2, ssrResponse, row1, row2);
                                        if (Request.QueryString["change"] != null)
                                        {
                                            SellResponse responseSSR;
                                            if (row1.Length > 0 || row2.Length > 0) responseSSR = APIBooking.SellSSR(SessionID, ssrResponse, row1, row2, true, listAll[0].RecordLocator, temFlight.TemFlightCurrencyCode, "change");
                                        }
                                        else
                                        {
                                            SellResponse responseSSR = APIBooking.SellSSR(SessionID, ssrResponse, row1, row2, true);
                                        }
                                        Session["row1" + iii] = row1;
                                        Session["row2" + iii] = row2;
                                    }
                                }

                            }
                            ABS.Navitaire.BookingManager.Booking book = APIBooking.GetBookingFromState(SessionID);
                            string xml = GetXMLString(book);

                            for (int i = 0; i < book.Passengers.Length; i++)
                            {
                                for (int ii = 0; ii < book.Passengers[i].PassengerFees.Length; ii++)
                                {
                                    if (book.Passengers[i].PassengerFees[ii].FeeType == FeeType.SSRFee && book.Passengers[i].PassengerFees[ii].SSRCode != "INFT")
                                    {
                                        if (book.Passengers[i].PassengerFees[ii].FlightReference != "")
                                        {
                                            if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == temFlight.TemFlightDeparture + temFlight.TemFlightArrival))
                                            {
                                                for (int y = 0; y < book.Passengers[i].PassengerFees[ii].ServiceCharges.Length; y++)
                                                {
                                                    totalSSRdepart += book.Passengers[i].PassengerFees[ii].ServiceCharges[y].Amount;

                                                }
                                            }
                                            else if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == temFlight.TemFlightArrival + temFlight.TemFlightDeparture))
                                            {
                                                for (int y = 0; y < book.Passengers[i].PassengerFees[ii].ServiceCharges.Length; y++)
                                                {
                                                    totalSSRReturn += book.Passengers[i].PassengerFees[ii].ServiceCharges[y].Amount;
                                                }
                                            }
                                            else if (Session["departTransit"] != null)
                                            {
                                                if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == temFlight.TemFlightDeparture + Session["departTransit"].ToString()))
                                                {
                                                    for (int y = 0; y < book.Passengers[i].PassengerFees[ii].ServiceCharges.Length; y++)
                                                    {
                                                        totalSSRdepart += book.Passengers[i].PassengerFees[ii].ServiceCharges[y].Amount;
                                                    }
                                                }
                                                else if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == Session["departTransit"].ToString() + temFlight.TemFlightArrival))
                                                {
                                                    for (int y = 0; y < book.Passengers[i].PassengerFees[ii].ServiceCharges.Length; y++)
                                                    {
                                                        totalSSRdepart += book.Passengers[i].PassengerFees[ii].ServiceCharges[y].Amount;
                                                    }
                                                }
                                                else if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == temFlight.TemFlightArrival + Session["returnTransit"].ToString()))
                                                {
                                                    for (int y = 0; y < book.Passengers[i].PassengerFees[ii].ServiceCharges.Length; y++)
                                                    {
                                                        totalSSRReturn += book.Passengers[i].PassengerFees[ii].ServiceCharges[y].Amount;
                                                    }
                                                }
                                                else if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == Session["returnTransit"].ToString() + temFlight.TemFlightDeparture))
                                                {
                                                    for (int y = 0; y < book.Passengers[i].PassengerFees[ii].ServiceCharges.Length; y++)
                                                    {
                                                        totalSSRReturn += book.Passengers[i].PassengerFees[ii].ServiceCharges[y].Amount;
                                                    }
                                                }
                                            }
                                        }
                                    }




                                    //if (book.Passengers[i].PassengerFees[ii].FlightReference != "")
                                    //{
                                    //    if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == temFlight.TemFlightDeparture + temFlight.TemFlightArrival))
                                    //    {
                                    //        if (book.Passengers[i].PassengerFees[ii].FeeType.ToString() == "SSRFee")
                                    //            totalSSRdepart += book.Passengers[i].PassengerFees[ii].ServiceCharges[0].Amount;
                                    //    }
                                    //}
                                }
                            }

                            TotSSRDepart += totalSSRdepart;
                            //DataTable dtBDFee = objBooking.dtBreakdownFee();
                            //dtBDFee = (DataTable)HttpContext.Current.Session["dataBDFeeDepart"];
                            //dtBDFee.Rows[0]["SSR"] = Convert.ToDecimal(totalSSRdepart);
                            //HttpContext.Current.Session["dataBDFeeDepart"] = dtBDFee;

                            //strExpr = "TemFlightId = '" + departID + "'";
                            //strSort = "";
                            //dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                            //foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                            //foundRows[0]["TemFlightTotalAmount"] = (Convert.ToDecimal(foundRows[0]["TemFlightTotalAmount"]) + totalSSRdepart);

                            //HttpContext.Current.Session["TempFlight"] = dt;

                            //for (int i = 0; i < book.Passengers.Length; i++)
                            //{
                            //    for (int ii = 0; ii < book.Passengers[i].PassengerFees.Length; ii++)
                            //    {
                            //        if (book.Passengers[i].PassengerFees[ii].FlightReference != "")
                            //        {
                            //            if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == temFlight.TemFlightArrival + temFlight.TemFlightDeparture))
                            //            {
                            //                if (book.Passengers[i].PassengerFees[ii].FeeType == FeeType.SSRFee)
                            //                {
                            //                    for (int y = 0; y < book.Passengers[i].PassengerFees[ii].ServiceCharges.Length; y++)
                            //                    {
                            //                        totalSSRReturn += book.Passengers[i].PassengerFees[ii].ServiceCharges[y].Amount;
                            //                    }
                            //                }
                            //            }
                            //        }

                            //        //if (book.Passengers[i].PassengerFees[ii].FlightReference != "")
                            //        //{
                            //        //    if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == temFlight.TemFlightArrival + temFlight.TemFlightDeparture))
                            //        //    {

                            //        //        if (book.Passengers[i].PassengerFees[ii].FeeType.ToString() == "SSRFee")
                            //        //            totalSSRReturn += book.Passengers[i].PassengerFees[ii].ServiceCharges[0].Amount;
                            //        //    }
                            //        //}
                            //    }
                            //}

                            TotSSRReturn += totalSSRReturn;

                            //dtBDFee = objBooking.dtBreakdownFee();
                            //dtBDFee = (DataTable)HttpContext.Current.Session["dataBDFeeReturn"];
                            //dtBDFee.Rows[0]["SSR"] = Convert.ToDecimal(totalSSRReturn);
                            //HttpContext.Current.Session["dataBDFeeReturn"] = dtBDFee;

                            //strExpr = "TemFlightId = '" + ReturnID + "'";
                            //strSort = "";
                            //dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                            //foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                            //foundRows[0]["TemFlightTotalAmount"] = (Convert.ToDecimal(foundRows[0]["TemFlightTotalAmount"]) + totalSSRReturn);

                            //HttpContext.Current.Session["TempFlight"] = dt;
                        }
                        else
                        {

                            ABS.Navitaire.APIBooking APIBooking = new ABS.Navitaire.APIBooking(SessionID);
                            //ABS.Navitaire.BookingManager.GetAvailabilityResponse response = APIBooking.GetAvailability(temFlight.TemFlightArrival, Convert.ToDateTime(temFlight.TemFlightSta), temFlight.TemFlightCurrencyCode, temFlight.TemFlightDeparture, temFlight.TemFlightPaxNum, ref SessionID);
                            //ABS.Navitaire.BookingManager.GetAvailabilityResponse response2 = APIBooking.GetAvailability(temFlight2.TemFlightArrival, Convert.ToDateTime(temFlight2.TemFlightSta), temFlight2.TemFlightCurrencyCode, temFlight2.TemFlightDeparture, temFlight2.TemFlightPaxNum, ref SessionID);
                            GetSSRAvailabilityForBookingResponse ssrResponse = (GetSSRAvailabilityForBookingResponse)Session["GetssrAvailabilityResponseForBooking"];
                            if (ssrResponse != null)
                            {

                                DataRow[] row1;
                                if (Convert.ToBoolean(Session["transit"]) == true)
                                {
                                    if (iii == 0)
                                    {
                                        row1 = dtList1.Select("CONVERT(PassengerNo, System.Int32) >= 0 AND CONVERT(PassengerNo, System.Int32) < " + Convert.ToInt32(dataClass.Rows[iii]["Quantity"]));

                                        row1t = dtList1t.Select("CONVERT(PassengerNo, System.Int32) >= 0 AND CONVERT(PassengerNo, System.Int32) < " + Convert.ToInt32(dataClass.Rows[iii]["Quantity"]));

                                    }

                                    else
                                    {
                                        if (Session["totalcountpax"] == null)
                                        {
                                            Session["totalcountpax"] = (Convert.ToInt32(dataClass.Rows[iii]["Quantity"]) + Convert.ToInt32(dataClass.Rows[iii - 1]["Quantity"]));
                                            row1 = dtList1.Select("CONVERT(PassengerNo, System.Int32) >= " + Convert.ToInt32(dataClass.Rows[iii - 1]["Quantity"]) + " AND CONVERT(PassengerNo, System.Int32) < " + Convert.ToInt32(Session["totalcountpax"]));

                                            row1t = dtList1t.Select("CONVERT(PassengerNo, System.Int32) >= " + Convert.ToInt32(dataClass.Rows[iii - 1]["Quantity"]) + " AND CONVERT(PassengerNo, System.Int32) < " + Convert.ToInt32(Session["totalcountpax"]));
                                        }
                                        else
                                        {
                                            //Session["totalcountpax"] = (Convert.ToInt32(dataClass.Rows[iii]["Quantity"]) + Convert.ToInt32(Session["totalcountpax"]));
                                            row1 = dtList1.Select("CONVERT(PassengerNo, System.Int32) >= " + Convert.ToInt32(Session["totalcountpax"]) + " AND CONVERT(PassengerNo, System.Int32)  < " + (Convert.ToInt32(dataClass.Rows[iii]["Quantity"]) + Convert.ToInt32(Session["totalcountpax"])));
                                            row1t = dtList1t.Select("CONVERT(PassengerNo, System.Int32) >= " + Convert.ToInt32(Session["totalcountpax"]) + " AND CONVERT(PassengerNo, System.Int32)  < " + (Convert.ToInt32(dataClass.Rows[iii]["Quantity"]) + Convert.ToInt32(Session["totalcountpax"])));
                                            Session["totalcountpax"] = (Convert.ToInt32(dataClass.Rows[iii]["Quantity"]) + Convert.ToInt32(Session["totalcountpax"]));
                                        }


                                    }
                                    if (Convert.ToBoolean(Session["back"]) != true)
                                    {
                                        if (Request.QueryString["change"] != null)
                                        {
                                            SellResponse responseSSR;
                                            if (row1.Length > 0 || row1t.Length > 0) responseSSR = APIBooking.SellSSRTransit(SessionID, ssrResponse, row1, null, row1t, null, false, listAll[0].RecordLocator, temFlight.TemFlightCurrencyCode, "change");
                                        }
                                        else
                                        {
                                            SellResponse responseSSR = APIBooking.SellSSRTransit(SessionID, ssrResponse, row1, null, row1t, null, false);
                                        }
                                        Session["row1" + iii] = row1;
                                        Session["row1t" + iii] = row1t;
                                    }
                                    else
                                    {
                                        //UpdateSSRResponse responseSSR = APIBooking.ReSellSSRTransit(SessionID, response, null, ssrResponse, row1, null, row1t, null);
                                        CancelResponse responseCancel = APIBooking.CancelSSRTransit(SessionID, ssrResponse, Curr, (DataRow[])Session["row1" + iii], null, (DataRow[])Session["row1t" + iii], null);
                                        if (Request.QueryString["change"] != null)
                                        {
                                            SellResponse responseSSR;
                                            if (row1.Length > 0 || row1t.Length > 0) responseSSR = APIBooking.SellSSRTransit(SessionID, ssrResponse, row1, null, row1t, null, true, listAll[0].RecordLocator, temFlight.TemFlightCurrencyCode, "change");
                                        }
                                        else
                                        {
                                            SellResponse responseSSR = APIBooking.SellSSRTransit(SessionID, ssrResponse, row1, null, row1t, null, true);
                                        }
                                        Session["row1" + iii] = row1;
                                        Session["row1t" + iii] = row1t;
                                    }
                                }
                                else
                                {
                                    if (iii == 0)
                                    {
                                        row1 = dtList1.Select("CONVERT(PassengerNo, System.Int32) >= 0 AND CONVERT(PassengerNo, System.Int32) < " + Convert.ToInt32(dataClass.Rows[iii]["Quantity"]));
                                    }

                                    else
                                    {
                                        if (Session["totalcountpax"] == null)
                                        {
                                            Session["totalcountpax"] = (Convert.ToInt32(dataClass.Rows[iii]["Quantity"]) + Convert.ToInt32(dataClass.Rows[iii - 1]["Quantity"]));
                                            row1 = dtList1.Select("CONVERT(PassengerNo, System.Int32) >= " + Convert.ToInt32(dataClass.Rows[iii - 1]["Quantity"]) + " AND CONVERT(PassengerNo, System.Int32)  < " + Convert.ToInt32(Session["totalcountpax"]));
                                        }
                                        else
                                        {
                                            row1 = dtList1.Select("CONVERT(PassengerNo, System.Int32) >= " + Convert.ToInt32(Session["totalcountpax"]) + " AND CONVERT(PassengerNo, System.Int32)  < " + (Convert.ToInt32(dataClass.Rows[iii]["Quantity"]) + Convert.ToInt32(Session["totalcountpax"])));
                                            Session["totalcountpax"] = (Convert.ToInt32(dataClass.Rows[iii]["Quantity"]) + Convert.ToInt32(Session["totalcountpax"]));
                                        }

                                    }
                                    if (Convert.ToBoolean(Session["back"]) != true)
                                    {
                                        if (Request.QueryString["change"] != null)
                                        {
                                            SellResponse responseSSR;
                                            if (row1.Length > 0) responseSSR = APIBooking.SellSSR(SessionID, ssrResponse, row1, null, false, listAll[0].RecordLocator, temFlight.TemFlightCurrencyCode, "change");
                                        }
                                        else
                                        {

                                            SellResponse responseSSR = APIBooking.SellSSR(SessionID, ssrResponse, row1, null, false);
                                        }
                                        Session["row1" + iii] = row1;
                                    }
                                    else
                                    {
                                        //UpdateSSRResponse responseSSR = APIBooking.ReSellSSR(SessionID, response, null, ssrResponse, row1, null);
                                        CancelResponse responseCancel = APIBooking.CancelSSR(SessionID, ssrResponse, Curr, (DataRow[])Session["row1" + iii], null);
                                        if (Request.QueryString["change"] != null)
                                        {
                                            SellResponse responseSSR;
                                            if (row1.Length > 0) responseSSR = APIBooking.SellSSR(SessionID, ssrResponse, row1, null, true, listAll[0].RecordLocator, temFlight.TemFlightCurrencyCode, "change");
                                        }
                                        else
                                        {
                                            SellResponse responseSSR = APIBooking.SellSSR(SessionID, ssrResponse, row1, null, true);
                                        }
                                        Session["row1" + iii] = row1;
                                    }
                                }


                            }
                            ABS.Navitaire.BookingManager.Booking book = APIBooking.GetBookingFromState(SessionID);

                            for (int i = 0; i < book.Passengers.Length; i++)
                            {
                                for (int ii = 0; ii < book.Passengers[i].PassengerFees.Length; ii++)
                                {
                                    if (book.Passengers[i].PassengerFees[ii].FeeType == FeeType.SSRFee && book.Passengers[i].PassengerFees[ii].SSRCode != "INFT")
                                    {
                                        if (book.Passengers[i].PassengerFees[ii].FlightReference != "")
                                        {
                                            if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == temFlight.TemFlightDeparture + temFlight.TemFlightArrival))
                                            {
                                                for (int y = 0; y < book.Passengers[i].PassengerFees[ii].ServiceCharges.Length; y++)
                                                {
                                                    totalSSRdepart += book.Passengers[i].PassengerFees[ii].ServiceCharges[y].Amount;
                                                }
                                            }
                                            else if (Session["departTransit"] != null)
                                            {
                                                if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == temFlight.TemFlightDeparture + Session["departTransit"].ToString()))
                                                {
                                                    for (int y = 0; y < book.Passengers[i].PassengerFees[ii].ServiceCharges.Length; y++)
                                                    {
                                                        totalSSRdepart += book.Passengers[i].PassengerFees[ii].ServiceCharges[y].Amount;
                                                    }
                                                }
                                                else if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == Session["departTransit"].ToString() + temFlight.TemFlightArrival))
                                                {
                                                    for (int y = 0; y < book.Passengers[i].PassengerFees[ii].ServiceCharges.Length; y++)
                                                    {
                                                        totalSSRdepart += book.Passengers[i].PassengerFees[ii].ServiceCharges[y].Amount;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            TotSSRDepart += totalSSRdepart;


                        }

                        if (listAll != null && listAll.Count > 0)
                        {
                            if (ReturnID != "")
                            {
                                int iIndexDepart = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 1 && p.Signature == SessionID);
                                int iIndexReturn = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 0 && p.Signature == SessionID);
                                if (iIndexDepart >= 0) listBookingDetail[iIndexDepart].LineSSR = totalSSRdepart;
                                if (iIndexReturn >= 0) listBookingDetail[iIndexReturn].LineSSR = totalSSRReturn;
                            }
                            else
                            {
                                int iIndexDepart = listBookingDetail.FindIndex(p => p.Signature == SessionID);
                                if (iIndexDepart >= 0) listBookingDetail[iIndexDepart].LineSSR = totalSSRdepart;
                            }

                        }

                        //begin, update dataclasstrans here
                        dataClass.Rows[iii]["SSRChrg"] = Convert.ToDecimal(dataClass.Rows[iii]["SSRChrg"].ToString()) + totalSSRdepart + totalSSRReturn;
                        dataClass.Rows[iii]["FullPrice"] = Convert.ToDecimal(dataClass.Rows[iii]["FullPrice"].ToString()) + totalSSRdepart + totalSSRReturn;
                        //end, update dataclasstrans here
                    }

                    HttpContext.Current.Session["dataClassTrans"] = dataClass;

                    TotalAmountGoing = 0;
                    TotalAmountReturn = 0;

                    int TotalPax = 0, PaxAdult = 0, PaxChild = 0;

                    if (listBookingDetail != null && listBookingDetail.Count > 0)
                    {

                        HttpContext.Current.Session.Remove("ChglstbookDTLInfo");
                        HttpContext.Current.Session.Add("ChglstbookDTLInfo", listBookingDetail);

                        if (Request.QueryString["change"] != null)
                        {
                            objBK_TRANSDTL_Infos = new BookingTransactionDetail();
                            objBK_TRANSDTL_Infos.RecordLocator = listBookingDetail[0].RecordLocator;
                            objBK_TRANSDTL_Infos.Signature = SessionID;
                            objListBK_TRANSDTL_Infos.Add(objBK_TRANSDTL_Infos);
                            HttpContext.Current.Session["objListBK_TRANSDTL_Infos"] = objListBK_TRANSDTL_Infos;
                        }

                        foreach (BookingTransactionDetail b in listBookingDetail)
                        {
                            if (b.Origin == listBookingDetail[0].Origin)
                            {
                                TotalPax += Convert.ToInt32(b.TotalPax);
                                PaxAdult += Convert.ToInt32(b.PaxAdult);
                                PaxChild += Convert.ToInt32(b.PaxChild);
                            }
                        }
                    }
                    else
                    {

                    }

                    UpdateTotalAmount(TotSSRDepart, TotSSRReturn, ref TotalAmountGoing, ref TotalAmountReturn, TotalPax, PaxAdult, PaxChild);

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
                    foreach (Bk_transssr b in listAll)
                    {
                        cnt++;
                        objBooking.SaveSSR(b, CoreBase.EnumSaveType.Update, "", true, cnt == listAll.Count ? true : false);

                    }
                }

                Session["back"] = true;

                if (listAll != null && listAll.Count > 0)
                {

                    BookingTransactionMain bookingMain = new BookingTransactionMain();
                    if (Request.QueryString["change"] != null)
                    {

                        //if (Session["ChgbookHDRInfo"] != null && Session["ChglstbookDTLInfo"] != null)
                        //{
                        //    UpdateAllBookingJourneyDetail();
                        //}
                    }
                    else
                    {
                        bookingMain = objBooking.GetSingleBK_TRANSMAIN(listAll[0].TransID);
                        if (bookingMain != null)
                        {
                            bookingMain.TransTotalSSR = TotSSRDepart + TotSSRReturn;
                            bookingMain.TotalAmtGoing = TotalAmountGoing;
                            bookingMain.TotalAmtReturn = TotalAmountReturn;
                            bookingMain.TransSubTotal = TotalAmountGoing + TotalAmountReturn;
                            bookingMain.TransTotalAmt = TotalAmountGoing + TotalAmountReturn;

                            objBooking.UpdateTotalSSR(listAll[0].TransID, bookingMain, listBookingDetail);

                        }
                    }


                }
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
            }
        }
        #endregion
        public Boolean UpdateAllBookingJourneyDetail()
        {
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

                    ABS.Navitaire.BookingManager.Booking book = APIBooking.GetBookingFromState(SessionID);



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
                                                            objBookingJourneyContainer.ChdPromoDiscChrg = charges.Amount;
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
                                                            objBookingJourneyContainer.ChdPromoDiscChrg += charges.Amount;
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
                                                            objBookingJourneyContainer.ChdPromoDiscChrg = charges.Amount;
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
                                                            objBookingJourneyContainer.AdtPromoDiscChrg += 0 - charges.Amount;
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
                                                            objBookingJourneyContainer.ChdPromoDiscChrg += charges.Amount;
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
                    objBookingJourneyContainer.AdtPromoDiscChrg = 0;
                    objBookingJourneyContainer.ChdDiscChrg = 0;
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
            object TotalBaggage, TotalMeal, TotalMeal1, TotalSport, TotalComfort, TotalDuty;
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

                    dtBDFee.Rows[0]["Baggage"] = Convert.ToDecimal(TotalBaggage);
                    dtBDFee.Rows[0]["Meal"] = Convert.ToDecimal(TotalMeal) + Convert.ToDecimal(TotalMeal1);
                    dtBDFee.Rows[0]["Sport"] = Convert.ToDecimal(TotalSport);
                    dtBDFee.Rows[0]["Comfort"] = Convert.ToDecimal(TotalComfort);
                }

                dtBDFee.Rows[0]["SSR"] = Convert.ToDecimal(TotalSSRDepart);


                TotalAmount = Convert.ToDecimal(dtBDFee.Rows[0]["Fare"]) * TotalPax;
                if (dtBDFee.Rows[0]["Infant"] != DBNull.Value)
                {
                    TotalAmount += Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * PaxAdt + Convert.ToDecimal(dtBDFee.Rows[0]["PaxFee"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Svc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) + Convert.ToDecimal(dtBDFee.Rows[0]["SSR"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Seat"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Infant"]);
                }
                else
                {
                    TotalAmount += Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * PaxAdt + Convert.ToDecimal(dtBDFee.Rows[0]["PaxFee"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Svc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) + Convert.ToDecimal(dtBDFee.Rows[0]["SSR"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Seat"]);
                }
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

                    dtBDFee.Rows[0]["Baggage"] = Convert.ToDecimal(TotalBaggage);
                    dtBDFee.Rows[0]["Meal"] = Convert.ToDecimal(TotalMeal) + Convert.ToDecimal(TotalMeal1);
                    dtBDFee.Rows[0]["Sport"] = Convert.ToDecimal(TotalSport);
                    dtBDFee.Rows[0]["Comfort"] = Convert.ToDecimal(TotalComfort);
                }

                dtBDFee.Rows[0]["SSR"] = Convert.ToDecimal(TotalSSRReturn);

                TotalAmount = Convert.ToDecimal(dtBDFee.Rows[0]["Fare"]) * TotalPax;
                if (dtBDFee.Rows[0]["Infant"] != DBNull.Value)
                {
                    TotalAmount += Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * PaxAdt + Convert.ToDecimal(dtBDFee.Rows[0]["PaxFee"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Svc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) + Convert.ToDecimal(dtBDFee.Rows[0]["SSR"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Seat"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Infant"]);
                }
                else
                {
                    TotalAmount += Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * PaxAdt + Convert.ToDecimal(dtBDFee.Rows[0]["PaxFee"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Svc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) + Convert.ToDecimal(dtBDFee.Rows[0]["SSR"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Seat"]);
                }
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

                ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
                string strExpr;
                string strSort;
                DataTable dt = objBooking.dtFlight();
                strExpr = "TemFlightId = '" + departID + "'";
                strSort = "";
                dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                FillModelFromDataRow(foundRows, ref model);


                if (ReturnID != "")
                {

                    strExpr = "TemFlightId = '" + ReturnID + "'";
                    strSort = "";
                    dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                    foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                    FillModelFromDataRow(foundRows, ref model2);

                }
            }
        }

        private string GetXMLString(object Obj)
        {
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Obj.GetType());
            System.IO.StringWriter writer = new System.IO.StringWriter();
            x.Serialize(writer, Obj);

            return writer.ToString();
        }
        public static bool IsNumeric(string s)
        {
            double result;
            return s != null && Double.TryParse(s, out result);
        }
                    #endregion

        #region BatchEdit
        protected void gvPassenger_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            String Detail = "";
            int meal = 0, drink = 0;
            int meal1 = 0, drink1 = 0;
            DataTable dataPass = new DataTable();
            try
            {
                foreach (var args in e.UpdateValues)
                {
                    DataTable data = Session["dtGridPass"] as DataTable;
                    data.PrimaryKey = new DataColumn[] { (data.Columns["PassengerID"]), (data.Columns["SeqNo"]) };
                    // Create an array for the key values to find.
                    object[] findTheseVals = new object[2];

                    // Set the values of the keys to find.
                    findTheseVals[0] = (args.Keys["PassengerID"]);
                    findTheseVals[1] = (args.Keys["SeqNo"]);

                    DataRow row = data.Rows.Find(findTheseVals);
                    row["PassengerID"] = args.Keys["PassengerID"];
                    row["SeqNo"] = args.Keys["SeqNo"];

                    //if (args.NewValues["Baggage"] != "")
                    //{
                    if (args.NewValues["Baggage"] == "" || args.NewValues["Baggage"] == null)
                    {
                        row["Baggage"] = string.Empty;
                        row["PriceBaggage"] = 0.00;
                        row["SSRCodeBaggage"] = string.Empty;
                    }
                    else
                    {
                        row["SSRCodeBaggage"] = args.NewValues["Baggage"];
                        DataTable dtBaggage = Session["dtBaggageDepart"] as DataTable;
                        if (args.NewValues["Baggage"].ToString().Length == 4)
                        {
                            DataRow[] resultBaggage = dtBaggage.Select("SSRCode = '" + args.NewValues["Baggage"] + "'");
                            foreach (DataRow rows in resultBaggage)
                            {
                                row["Baggage"] = rows["ConcatenatedField"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["PriceBaggage"] = Convert.ToDecimal(Detail);
                                row["SSRCodeBaggage"] = rows["SSRCode"];
                            }
                        }
                        else
                        {
                            DataRow[] resultBaggage = dtBaggage.Select("ConcatenatedField LIKE '" + args.NewValues["Baggage"].ToString().Substring(0, 19) + "%'");
                            foreach (DataRow rows in resultBaggage)
                            {
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceBaggage"]) && row["SSRCodeBaggage"].ToString().Trim() == rows[0].ToString().Trim())
                                {
                                    continue;
                                    //return;
                                }
                                else
                                {
                                    row["Baggage"] = rows["ConcatenatedField"];
                                    row["PriceBaggage"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeBaggage"] = rows["SSRCode"];
                                }
                            }
                        }
                        //}

                    }

                    //if (args.NewValues["Sport"] != "")
                    //{
                    if (args.NewValues["Sport"] == "" || args.NewValues["Sport"] == null)
                    {
                        row["Sport"] = string.Empty;
                        row["PriceSport"] = 0.00;
                        row["SSRCodeSport"] = string.Empty;
                    }
                    else
                    {
                        row["SSRCodeSport"] = args.NewValues["Sport"];
                        DataTable dtSport = Session["dtSportDepart"] as DataTable;
                        if (args.NewValues["Sport"].ToString().Length == 4)
                        {
                            DataRow[] resultSport = dtSport.Select("SSRCode = '" + args.NewValues["Sport"] + "'");
                            foreach (DataRow rows in resultSport)
                            {
                                row["Sport"] = rows["ConcatenatedField"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["PriceSport"] = Convert.ToDecimal(Detail);
                                row["SSRCodeSport"] = rows["SSRCode"];
                            }
                        }

                        else
                        {
                            DataRow[] resultMeal = dtSport.Select("ConcatenatedField LIKE '" + args.NewValues["Sport"].ToString().Substring(0, 20) + "%'");
                            foreach (DataRow rows in resultMeal)
                            {
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceSport"]) && row["SSRCodeSport"].ToString().Trim() == rows[0].ToString().Trim())
                                {
                                    continue;
                                    //return;
                                }
                                else
                                {
                                    row["Sport"] = rows["ConcatenatedField"];
                                    row["PriceSport"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeSport"] = rows["SSRCode"];
                                }
                            }
                        }
                        //}

                    }

                    if (args.NewValues["Meal"] == "" || args.NewValues["Meal"] == null)
                    {
                        row["Meal"] = string.Empty;
                        row["PriceMeal"] = 0.00;
                        row["SSRCodeMeal"] = string.Empty;
                        row["Drink"] = string.Empty;
                        row["PriceDrink"] = 0.00;
                        row["SSRCodeDrink"] = string.Empty;
                    }
                    else
                    {
                        meal = 1;
                        row["SSRCodeMeal"] = args.NewValues["Meal"];
                        DataTable dtMeal = Session["dtMealDepart"] as DataTable;
                        DataTable dtDrink = Session["dtDrinkDepart"] as DataTable;
                        if (args.NewValues["Meal"].ToString().Length == 4)
                        {
                            DataRow[] resultMeal = dtMeal.Select("SSRCode = '" + args.NewValues["Meal"] + "'");
                            foreach (DataRow rows in resultMeal)
                            {
                                row["Meal"] = rows["Detail"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["PriceMeal"] = Convert.ToDecimal(Detail);
                                row["SSRCodeMeal"] = rows["SSRCode"];
                            }

                            if (args.NewValues["Drink"] == null || args.NewValues["Drink"] == "")
                            {
                                string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                for (int c = 0; c < drinkcode.Length; c++)
                                {
                                    DataRow[] resultDrink = dtDrink.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                    if (resultDrink.Length > 0)
                                    {
                                        foreach (DataRow rows in resultDrink)
                                        {
                                            row["Drink"] = rows["ConcatenatedField"];
                                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                            row["PriceDrink"] = Convert.ToDecimal(Detail);
                                            row["SSRCodeDrink"] = rows["SSRCode"];
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            string tmp = Regex.Replace(args.NewValues["Meal"].ToString().Substring(0, 10), "[^ 0-9a-zA-Z]+", "''", RegexOptions.None);
                            DataRow[] resultMeal = dtMeal.Select("Detail LIKE '%" + tmp + "%'");
                            foreach (DataRow rows in resultMeal)
                            {
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceMeal"]) && row["Meal"].ToString().Trim() == rows[1].ToString().Trim())
                                {
                                    continue;
                                }
                                else
                                {
                                    row["Meal"] = rows["Detail"];
                                    row["PriceMeal"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeMeal"] = rows["SSRCode"];
                                }
                            }

                            if (args.NewValues["Drink"] == null || args.NewValues["Drink"] == "")
                            {
                                string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                for (int c = 0; c < drinkcode.Length; c++)
                                {
                                    DataRow[] resultDrink = dtDrink.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                    if (resultDrink.Length > 0)
                                    {
                                        foreach (DataRow rows in resultDrink)
                                        {
                                            row["Drink"] = rows["ConcatenatedField"];
                                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                            row["PriceDrink"] = Convert.ToDecimal(Detail);
                                            row["SSRCodeDrink"] = rows["SSRCode"];
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (args.NewValues["Drink"] == "" || args.NewValues["Drink"] == null)
                    {
                        //row["Drink"] = string.Empty;
                        //row["PriceDrink"] = 0.00;
                        //row["SSRCodeDrink"] = string.Empty;
                    }
                    else
                    {
                        drink = 1;
                        row["SSRCodeDrink"] = args.NewValues["Drink"];
                        DataTable dtSport = Session["dtDrinkDepart"] as DataTable;
                        DataTable dtMeal = Session["dtMealDepart"] as DataTable;
                        if (args.NewValues["Drink"].ToString().Length == 4)
                        {
                            DataRow[] resultSport = dtSport.Select("SSRCode = '" + args.NewValues["Drink"] + "'");
                            foreach (DataRow rows in resultSport)
                            {
                                row["Drink"] = rows["ConcatenatedField"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["PriceDrink"] = Convert.ToDecimal(Detail);
                                row["SSRCodeDrink"] = rows["SSRCode"];
                            }

                            if (args.NewValues["Meal"] == "" || args.NewValues["Meal"] == null)
                            {
                                DataRow[] resultMeal = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                                foreach (DataRow rows in resultMeal)
                                {
                                    row["Meal"] = rows["Detail"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceMeal"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeMeal"] = rows["SSRCode"];
                                }
                            }

                        }

                        else
                        {
                            DataRow[] resultMeal = dtSport.Select("ConcatenatedField LIKE '" + args.NewValues["Drink"].ToString().Substring(0, 10) + "%'");
                            foreach (DataRow rows in resultMeal)
                            {
                                row["Drink"] = rows["ConcatenatedField"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["PriceDrink"] = Convert.ToDecimal(Detail);
                                row["SSRCodeDrink"] = rows["SSRCode"];
                            }

                            if (args.NewValues["Meal"] == "" || args.NewValues["Meal"] == null)
                            {
                                DataRow[] resultMeals = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                                foreach (DataRow rows in resultMeals)
                                {
                                    row["Meal"] = rows["Detail"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceMeal"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeMeal"] = rows["SSRCode"];
                                }
                            }
                        }

                    }

                    if (args.NewValues["Meal1"] == "" || args.NewValues["Meal1"] == null)
                    {
                        row["Meal1"] = string.Empty;
                        row["PriceMeal1"] = 0.00;
                        row["SSRCodeMeal1"] = string.Empty;
                        row["Drink1"] = string.Empty;
                        row["PriceDrink1"] = 0.00;
                        row["SSRCodeDrink1"] = string.Empty;
                    }
                    else
                    {
                        meal1 = 1;
                        row["SSRCodeMeal1"] = args.NewValues["Meal1"];
                        DataTable dtMeal1 = Session["dtMealDepart2"] as DataTable;
                        DataTable dtSport = Session["dtDrinkDepart2"] as DataTable;
                        if (args.NewValues["Meal1"].ToString().Length == 4)
                        {
                            DataRow[] resultMeal1 = dtMeal1.Select("SSRCode = '" + args.NewValues["Meal1"] + "'");
                            foreach (DataRow rows in resultMeal1)
                            {
                                row["Meal1"] = rows["Detail"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["PriceMeal1"] = Convert.ToDecimal(Detail);
                                row["SSRCodeMeal1"] = rows["SSRCode"];
                            }

                            if (args.NewValues["Drink1"] == null || args.NewValues["Drink1"] == "")
                            {
                                string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                for (int c = 0; c < drinkcode.Length; c++)
                                {
                                    DataRow[] resultSport = dtSport.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                    if (resultSport.Length > 0)
                                    {
                                        foreach (DataRow rows in resultSport)
                                        {
                                            row["Drink1"] = rows["ConcatenatedField"];
                                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                            row["PriceDrink1"] = Convert.ToDecimal(Detail);
                                            row["SSRCodeDrink1"] = rows["SSRCode"];
                                        }
                                        break;
                                    }
                                }
                            }
                        }

                        else
                        {
                            string tmp = Regex.Replace(args.NewValues["Meal1"].ToString().Substring(0, 10), "[^ 0-9a-zA-Z]+", "''", RegexOptions.None);
                            DataRow[] resultMeal = dtMeal1.Select("Detail LIKE '%" + tmp + "%'");
                            foreach (DataRow rows in resultMeal)
                            {
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceMeal1"]) && row["Meal1"].ToString().Trim() == rows[1].ToString().Trim())
                                {
                                    continue;
                                }
                                else
                                {
                                    row["Meal1"] = rows["Detail"];
                                    row["PriceMeal1"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeMeal1"] = rows["SSRCode"];
                                }
                            }

                            if (args.NewValues["Drink1"] == null || args.NewValues["Drink1"] == "")
                            {
                                string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                for (int c = 0; c < drinkcode.Length; c++)
                                {
                                    DataRow[] resultSport = dtSport.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                    if (resultSport.Length > 0)
                                    {
                                        foreach (DataRow rows in resultSport)
                                        {
                                            row["Drink1"] = rows["ConcatenatedField"];
                                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                            row["PriceDrink1"] = Convert.ToDecimal(Detail);
                                            row["SSRCodeDrink1"] = rows["SSRCode"];
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (args.NewValues["Drink1"] == null || args.NewValues["Drink1"] == "")
                    {

                    }
                    else
                    {
                        drink1 = 1;
                        row["SSRCodeDrink1"] = args.NewValues["Drink1"];
                        DataTable dtSport = Session["dtDrinkDepart2"] as DataTable;
                        DataTable dtMeal = Session["dtMealDepart2"] as DataTable;
                        if (args.NewValues["Drink1"].ToString().Length == 4)
                        {
                            DataRow[] resultSport = dtSport.Select("SSRCode = '" + args.NewValues["Drink1"] + "'");
                            foreach (DataRow rows in resultSport)
                            {
                                row["Drink1"] = rows["ConcatenatedField"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["PriceDrink1"] = Convert.ToDecimal(Detail);
                                row["SSRCodeDrink1"] = rows["SSRCode"];
                            }

                            if (args.NewValues["Meal1"] == "" || args.NewValues["Meal1"] == null)
                            {
                                DataRow[] resultMeal = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                                foreach (DataRow rows in resultMeal)
                                {
                                    row["Meal1"] = rows["Detail"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceMeal1"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeMeal1"] = rows["SSRCode"];
                                }
                            }
                        }

                        else
                        {
                            DataRow[] resultMeal = dtSport.Select("ConcatenatedField LIKE '" + args.NewValues["Drink1"].ToString().Substring(0, 10) + "%'");
                            foreach (DataRow rows in resultMeal)
                            {
                                row["Drink1"] = rows["ConcatenatedField"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["PriceDrink1"] = Convert.ToDecimal(Detail);
                                row["SSRCodeDrink1"] = rows["SSRCode"];
                            }

                            if (args.NewValues["Meal1"] == "" || args.NewValues["Meal1"] == null)
                            {
                                DataRow[] resultMeals = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                                foreach (DataRow rows in resultMeals)
                                {
                                    row["Meal1"] = rows["Detail"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceMeal1"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeMeal1"] = rows["SSRCode"];
                                }
                            }
                        }

                    }

                    if (args.NewValues["Comfort"] == "" || args.NewValues["Comfort"] == null)
                    {
                        row["Comfort"] = string.Empty;
                        row["PriceComfort"] = 0.00;
                        row["SSRCodeComfort"] = string.Empty;
                    }
                    else
                    {
                        row["SSRCodeComfort"] = args.NewValues["Comfort"];
                        DataTable dtComfort = Session["dtComfortDepart"] as DataTable;
                        if (args.NewValues["Comfort"].ToString().Length == 4)
                        {
                            DataRow[] resultComfort = dtComfort.Select("SSRCode = '" + args.NewValues["Comfort"] + "'");
                            foreach (DataRow rows in resultComfort)
                            {
                                row["Comfort"] = rows["Detail"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["PriceComfort"] = Convert.ToDecimal(Detail);
                                row["SSRCodeComfort"] = rows["SSRCode"];
                            }
                        }

                        else
                        {
                            DataRow[] resultComfort = dtComfort.Select("Detail = '" + args.NewValues["Comfort"].ToString().Substring(0, 11) + "'");
                            foreach (DataRow rows in resultComfort)
                            {
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceComfort"]) && row["SSRCodeComfort"].ToString().Trim() == rows[0].ToString().Trim())
                                {
                                    continue;
                                    //return;
                                }
                                else
                                {
                                    row["Comfort"] = rows["Detail"];
                                    row["PriceComfort"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeComfort"] = rows["SSRCode"];
                                }
                            }
                        }
                        //}
                    }

                    if (args.NewValues["Duty"] != "" && args.NewValues["Duty"] != null)
                    {
                        row["SSRCodeDuty"] = args.NewValues["Duty"];
                        DataTable dtDuty = Session["dtDutyDepart"] as DataTable;
                        DataRow[] resultDuty = dtDuty.Select("SSRCode = '" + args.NewValues["Duty"] + "'");
                        foreach (DataRow rows in resultDuty)
                        {
                            row["Duty"] = rows["Detail"];
                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                            row["PriceDuty"] = Convert.ToDecimal(Detail);
                        }
                    }

                    Session["dtGridPass"] = data;

                }

                e.Handled = true;
            }
            catch (Exception ex)
            {
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
            String Detail = "";
            try
            {
                foreach (var args in e.UpdateValues)
                {
                    DataTable data = Session["dtGridPass2"] as DataTable;
                    data.PrimaryKey = new DataColumn[] { (data.Columns["PassengerID"]), (data.Columns["SeqNo"]) };
                    // Create an array for the key values to find.
                    object[] findTheseVals = new object[2];

                    // Set the values of the keys to find.
                    findTheseVals[0] = (args.Keys["PassengerID"]);
                    findTheseVals[1] = (args.Keys["SeqNo"]);

                    DataRow row = data.Rows.Find(findTheseVals);
                    row["PassengerID"] = args.Keys["PassengerID"];
                    row["SeqNo"] = args.Keys["SeqNo"];

                    //if (args.NewValues["Baggage"] != "")
                    //{
                    if (args.NewValues["Baggage"] == "" || args.NewValues["Baggage"] == null)
                    {
                        row["Baggage"] = string.Empty;
                        row["PriceBaggage"] = 0.00;
                        row["SSRCodeBaggage"] = string.Empty;
                    }
                    else
                    {
                        row["SSRCodeBaggage"] = args.NewValues["Baggage"];
                        DataTable dtBaggage = Session["dtBaggageReturn"] as DataTable;
                        if (args.NewValues["Baggage"].ToString().Length == 4)
                        {
                            DataRow[] resultBaggage = dtBaggage.Select("SSRCode = '" + args.NewValues["Baggage"] + "'");
                            foreach (DataRow rows in resultBaggage)
                            {
                                row["Baggage"] = rows["ConcatenatedField"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["PriceBaggage"] = Convert.ToDecimal(Detail);
                                row["SSRCodeBaggage"] = rows["SSRCode"];
                            }
                        }
                        else
                        {
                            DataRow[] resultBaggage = dtBaggage.Select("ConcatenatedField LIKE '" + args.NewValues["Baggage"].ToString().Substring(0, 19) + "%'");
                            foreach (DataRow rows in resultBaggage)
                            {
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceBaggage"]) && row["SSRCodeBaggage"].ToString().Trim() == rows[0].ToString().Trim())
                                {
                                    continue;
                                    //return;
                                }
                                else
                                {
                                    row["Baggage"] = rows["ConcatenatedField"];
                                    row["PriceBaggage"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeBaggage"] = rows["SSRCode"];
                                }
                            }
                        }
                        //}

                    }

                    //if (args.NewValues["Sport"] != "")
                    //{
                    if (args.NewValues["Sport"] == "" || args.NewValues["Sport"] == null)
                    {
                        row["Sport"] = string.Empty;
                        row["PriceSport"] = 0.00;
                        row["SSRCodeSport"] = string.Empty;
                    }
                    else
                    {
                        row["SSRCodeSport"] = args.NewValues["Sport"];
                        DataTable dtSport = Session["dtSportReturn"] as DataTable;
                        if (args.NewValues["Sport"].ToString().Length == 4)
                        {
                            DataRow[] resultSport = dtSport.Select("SSRCode = '" + args.NewValues["Sport"] + "'");
                            foreach (DataRow rows in resultSport)
                            {
                                row["Sport"] = rows["ConcatenatedField"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["PriceSport"] = Convert.ToDecimal(Detail);
                                row["SSRCodeSport"] = rows["SSRCode"];
                            }
                        }
                        else
                        {
                            DataRow[] resultMeal = dtSport.Select("ConcatenatedField LIKE '" + args.NewValues["Sport"].ToString().Substring(0, 20) + "%'");
                            foreach (DataRow rows in resultMeal)
                            {
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceSport"]) && row["SSRCodeSport"].ToString().Trim() == rows[0].ToString().Trim())
                                {
                                    continue;
                                    //return;
                                }
                                else
                                {
                                    row["Sport"] = rows["ConcatenatedField"];
                                    row["PriceSport"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeSport"] = rows["SSRCode"];
                                }
                            }
                        }
                        //}

                    }

                    //if (args.NewValues["Meal"] != "")
                    //{
                    if (args.NewValues["Meal"] == "" || args.NewValues["Meal"] == null)
                    {
                        row["Meal"] = string.Empty;
                        row["PriceMeal"] = 0.00;
                        row["SSRCodeMeal"] = string.Empty;
                        row["Drink"] = string.Empty;
                        row["PriceDrink"] = 0.00;
                        row["SSRCodeDrink"] = string.Empty;
                    }
                    else
                    {
                        row["SSRCodeMeal"] = args.NewValues["Meal"];
                        DataTable dtMeal = Session["dtMealReturn"] as DataTable;
                        DataTable dtSport = Session["dtDrinkReturn"] as DataTable;
                        if (args.NewValues["Meal"].ToString().Length == 4)
                        {
                            DataRow[] resultMeal = dtMeal.Select("SSRCode = '" + args.NewValues["Meal"] + "'");
                            foreach (DataRow rows in resultMeal)
                            {
                                row["Meal"] = rows["Detail"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["PriceMeal"] = Convert.ToDecimal(Detail);
                                row["SSRCodeMeal"] = rows["SSRCode"];
                            }

                            if (args.NewValues["Drink"] == null || args.NewValues["Drink"] == "")
                            {
                                string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                for (int c = 0; c < drinkcode.Length; c++)
                                {
                                    DataRow[] resultSport = dtSport.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                    if (resultSport.Length > 0)
                                    {
                                        foreach (DataRow rows in resultSport)
                                        {
                                            row["Drink"] = rows["ConcatenatedField"];
                                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                            row["PriceDrink"] = Convert.ToDecimal(Detail);
                                            row["SSRCodeDrink"] = rows["SSRCode"];
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            string tmp = Regex.Replace(args.NewValues["Meal"].ToString().Substring(0, 10), "[^ 0-9a-zA-Z]+", "''", RegexOptions.None);
                            DataRow[] resultMeal = dtMeal.Select("Detail LIKE '%" + tmp + "%'");
                            foreach (DataRow rows in resultMeal)
                            {
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceMeal"]) && row["Meal"].ToString().Trim() == rows[1].ToString().Trim())
                                {
                                    continue;
                                }
                                else
                                {
                                    row["Meal"] = rows["Detail"];
                                    row["PriceMeal"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeMeal"] = rows["SSRCode"];
                                }
                            }
                            if (args.NewValues["Drink"] == "" || args.NewValues["Drink"] == null)
                            {
                                string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                for (int c = 0; c < drinkcode.Length; c++)
                                {
                                    DataRow[] resultSport = dtSport.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                    if (resultSport.Length > 0)
                                    {
                                        foreach (DataRow rows in resultSport)
                                        {
                                            row["Drink"] = rows["ConcatenatedField"];
                                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                            row["PriceDrink"] = Convert.ToDecimal(Detail);
                                            row["SSRCodeDrink"] = rows["SSRCode"];
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        //}

                    }

                    if (args.NewValues["Drink"] == "" || args.NewValues["Drink"] == null)
                    {
                        //row["Drink"] = string.Empty;
                        //row["PriceDrink"] = 0.00;
                        //row["SSRCodeDrink"] = string.Empty;

                    }
                    else
                    {
                        row["SSRCodeDrink"] = args.NewValues["Drink"];
                        DataTable dtSport = Session["dtDrinkReturn"] as DataTable;
                        DataTable dtMeal = Session["dtMealReturn"] as DataTable;
                        if (args.NewValues["Drink"].ToString().Length == 4)
                        {
                            DataRow[] resultSport = dtSport.Select("SSRCode = '" + args.NewValues["Drink"] + "'");
                            foreach (DataRow rows in resultSport)
                            {
                                row["Drink"] = rows["ConcatenatedField"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["PriceDrink"] = Convert.ToDecimal(Detail);
                                row["SSRCodeDrink"] = rows["SSRCode"];
                            }

                            if (args.NewValues["Meal"] == "" || args.NewValues["Meal"] == null)
                            {
                                DataRow[] resultMeal = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                                foreach (DataRow rows in resultMeal)
                                {
                                    row["Meal"] = rows["Detail"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceMeal"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeMeal"] = rows["SSRCode"];
                                }
                            }
                        }
                        else
                        {
                            DataRow[] resultMeal = dtSport.Select("ConcatenatedField LIKE '" + args.NewValues["Drink"].ToString().Substring(0, 10) + "%'");
                            foreach (DataRow rows in resultMeal)
                            {
                                row["Drink"] = rows["ConcatenatedField"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["PriceDrink"] = Convert.ToDecimal(Detail);
                                row["SSRCodeDrink"] = rows["SSRCode"];
                            }

                            if (args.NewValues["Meal"] == "" || args.NewValues["Meal"] == null)
                            {
                                DataRow[] resultMeals = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                                foreach (DataRow rows in resultMeals)
                                {
                                    row["Meal"] = rows["Detail"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceMeal"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeMeal"] = rows["SSRCode"];
                                }
                            }
                        }
                        //}

                    }
                    //if (args.NewValues["Meal1"] != "")
                    //{
                    if (args.NewValues["Meal1"] == "" || args.NewValues["Meal1"] == null)
                    {
                        row["Meal1"] = string.Empty;
                        row["PriceMeal1"] = 0.00;
                        row["SSRCodeMeal1"] = string.Empty;
                        row["Drink1"] = string.Empty;
                        row["PriceDrink1"] = 0.00;
                        row["SSRCodeDrink1"] = string.Empty;
                    }
                    else
                    {
                        row["SSRCodeMeal1"] = args.NewValues["Meal1"];
                        DataTable dtMeal1 = Session["dtMealReturn2"] as DataTable;
                        DataTable dtSport = Session["dtDrinkReturn2"] as DataTable;
                        if (args.NewValues["Meal1"].ToString().Length == 4)
                        {
                            DataRow[] resultMeal1 = dtMeal1.Select("SSRCode = '" + args.NewValues["Meal1"] + "'");
                            foreach (DataRow rows in resultMeal1)
                            {
                                row["Meal1"] = rows["Detail"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["PriceMeal1"] = Convert.ToDecimal(Detail);
                                row["SSRCodeMeal1"] = rows["SSRCode"];
                            }

                            if (args.NewValues["Drink1"] == null || args.NewValues["Drink1"] == "")
                            {
                                string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                for (int c = 0; c < drinkcode.Length; c++)
                                {
                                    DataRow[] resultSport = dtSport.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                    if (resultSport.Length > 0)
                                    {
                                        foreach (DataRow rows in resultSport)
                                        {
                                            row["Drink1"] = rows["ConcatenatedField"];
                                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                            row["PriceDrink1"] = Convert.ToDecimal(Detail);
                                            row["SSRCodeDrink1"] = rows["SSRCode"];
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            string tmp = Regex.Replace(args.NewValues["Meal1"].ToString().Substring(0, 10), "[^ 0-9a-zA-Z]+", "''", RegexOptions.None);
                            DataRow[] resultMeal = dtMeal1.Select("Detail LIKE '%" + tmp + "%'");
                            foreach (DataRow rows in resultMeal)
                            {
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceMeal1"]) && row["Meal1"].ToString().Trim() == rows[1].ToString().Trim())
                                {
                                    continue;
                                }
                                else
                                {
                                    row["Meal1"] = rows["Detail"];
                                    row["PriceMeal1"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeMeal1"] = rows["SSRCode"];
                                }
                            }

                            if (args.NewValues["Drink1"] == null || args.NewValues["Drink1"] == "")
                            {
                                string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                for (int c = 0; c < drinkcode.Length; c++)
                                {
                                    DataRow[] resultSport = dtSport.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                    if (resultSport.Length > 0)
                                    {
                                        foreach (DataRow rows in resultSport)
                                        {
                                            row["Drink1"] = rows["ConcatenatedField"];
                                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                            row["PriceDrink1"] = Convert.ToDecimal(Detail);
                                            row["SSRCodeDrink1"] = rows["SSRCode"];
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        //}

                    }

                    if (args.NewValues["Drink1"] == null || args.NewValues["Drink1"] == "")
                    {
                        //row["Drink1"] = string.Empty;
                        //row["PriceDrink1"] = 0.00;
                        //row["SSRCodeDrink1"] = string.Empty;

                    }
                    else
                    {
                        row["SSRCodeDrink1"] = args.NewValues["Drink1"];
                        DataTable dtSport = Session["dtDrinkReturn2"] as DataTable;
                        DataTable dtMeal = Session["dtMealReturn2"] as DataTable;
                        if (args.NewValues["Drink1"].ToString().Length == 4)
                        {
                            DataRow[] resultSport = dtSport.Select("SSRCode = '" + args.NewValues["Drink1"] + "'");
                            foreach (DataRow rows in resultSport)
                            {
                                row["Drink1"] = rows["ConcatenatedField"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["PriceDrink1"] = Convert.ToDecimal(Detail);
                                row["SSRCodeDrink1"] = rows["SSRCode"];
                            }

                            if (args.NewValues["Meal1"] == "" || args.NewValues["Meal1"] == null)
                            {
                                DataRow[] resultMeal = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                                foreach (DataRow rows in resultMeal)
                                {
                                    row["Meal1"] = rows["Detail"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceMeal1"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeMeal1"] = rows["SSRCode"];
                                }
                            }
                        }
                        else
                        {
                            DataRow[] resultMeal = dtSport.Select("ConcatenatedField LIKE '" + args.NewValues["Drink1"].ToString().Substring(0, 10) + "%'");
                            foreach (DataRow rows in resultMeal)
                            {
                                row["Drink1"] = rows["ConcatenatedField"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["PriceDrink1"] = Convert.ToDecimal(Detail);
                                row["SSRCodeDrink1"] = rows["SSRCode"];
                            }

                            if (args.NewValues["Meal1"] == "" || args.NewValues["Meal1"] == null)
                            {
                                DataRow[] resultMeals = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                                foreach (DataRow rows in resultMeals)
                                {
                                    row["Meal1"] = rows["Detail"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceMeal1"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeMeal1"] = rows["SSRCode"];
                                }
                            }
                        }
                        //}

                    }
                    //if (args.NewValues["Comfort"] != "")
                    //{
                    if (args.NewValues["Comfort"] == "" || args.NewValues["Comfort"] == null)
                    {
                        row["Comfort"] = string.Empty;
                        row["PriceComfort"] = 0.00;
                        row["SSRCodeComfort"] = string.Empty;
                    }
                    else
                    {
                        row["SSRCodeComfort"] = args.NewValues["Comfort"];
                        DataTable dtComfort = Session["dtComfortReturn"] as DataTable;
                        if (args.NewValues["Comfort"].ToString().Length == 4)
                        {
                            DataRow[] resultComfort = dtComfort.Select("SSRCode = '" + args.NewValues["Comfort"] + "'");
                            foreach (DataRow rows in resultComfort)
                            {
                                row["Comfort"] = rows["Detail"];
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                row["PriceComfort"] = Convert.ToDecimal(Detail);
                                row["SSRCodeComfort"] = rows["SSRCode"];
                            }
                        }
                        else
                        {
                            DataRow[] resultComfort = dtComfort.Select("Detail = '" + args.NewValues["Comfort"].ToString().Substring(0, 11) + "'");
                            foreach (DataRow rows in resultComfort)
                            {
                                Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceComfort"]) && row["SSRCodeComfort"].ToString().Trim() == rows[0].ToString().Trim())
                                {
                                    continue;
                                    //return;
                                }
                                else
                                {
                                    row["Comfort"] = rows["Detail"];
                                    row["PriceComfort"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeComfort"] = rows["SSRCode"];
                                }
                            }
                        }
                        //}
                    }

                    if (args.NewValues["Duty"] != "" && args.NewValues["Duty"] != null)
                    {
                        row["SSRCodeDuty"] = args.NewValues["Duty"];
                        DataTable dtDuty = Session["dtDutyReturn"] as DataTable;
                        DataRow[] resultDuty = dtDuty.Select("SSRCode = '" + args.NewValues["Duty"] + "'");
                        foreach (DataRow rows in resultDuty)
                        {
                            row["Duty"] = rows["Detail"];
                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                            row["PriceDuty"] = Convert.ToDecimal(Detail);
                        }
                    }

                    Session["dtGridPass2"] = data;
                }

                e.Handled = true;
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
            }
            finally
            {
                gvPassenger2.DataSource = Session["dtGridPass2"];
                gvPassenger2.DataBind();
            }
        }
        #endregion

    }
}