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

namespace GroupBooking.Web
{
    public partial class ManageAddOn : System.Web.UI.Page
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

        List<BookingTransactionDetail> objListBK_TRANSDTL_Infos = new List<BookingTransactionDetail>();
        List<Bk_transssr> listSSRPNR = new List<Bk_transssr>();
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
        String TransID = "";
        int havebalance = 0;
        //added by ketee, currency
        private string Curr = "";

        ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
        ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model2 = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
        #endregion

        #region "Load"
        protected void Page_Load(object sender, EventArgs e)
        {
            string signature = "";
            DataTable dt = new DataTable();
            try
            {
                TransID = Request.QueryString["TransID"];
                ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking("");
                //signature = Session["signature"].ToString();
                Session["PaxStatus"] = "";
                if (Session["AgentSet"] != null)
                {
                    MyUserSet = (UserSet)Session["AgentSet"];
                    if (!IsPostBack)
                    {
                        ClearSession();
                        GetSellSSR(signature, TransID);
                        //InitializeSetting();
                        //if (Convert.ToBoolean(Session["back"]) == true)
                        //{

                        BindLabel();
                        //}
                    }

                    GetPassengerList(TransID);

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

                    if (Session["OneWay"] != null)
                    {
                        Boolean OneWay = (Boolean)Session["OneWay"];
                        if (OneWay != true)
                        {
                            GetPassengerList2(TransID);
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


                    if (Session["IsNew"] != null && Session["IsNew"].ToString() == "True")
                    {
                        first = 0;
                    }
                    if (first == 0)
                    {
                        BindDefaultBaggage();

                    }
                }
                else
                {
                    Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.SessionExpired);
                }
            }
            catch (Exception ex)
            {
                //log.Error(this,ex);
                log.Error(this, ex);
                //Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.SessionExpired);
            }

        }

        #endregion

        #region "Initializer"
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
            HttpContext.Current.Session.Remove("transit");
            HttpContext.Current.Session.Remove("departTransit");
            HttpContext.Current.Session.Remove("transitdepart");
            HttpContext.Current.Session.Remove("returnTransit");
            HttpContext.Current.Session.Remove("transitreturn");
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
            (e.Editor as ASPxTextBox).NullText = "Select Meal";
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
                    //if (result.Length == 0)
                    //{
                    //    dtMeal.Rows.Add("", "", "", "");
                    //}
                    DataView dvMeal = dtMeal.DefaultView;

                    grdlkMealDepart.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                    grdlkMealDepart.DataBind();
                    firstinit = 1;
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
                    //if (result.Length == 0)
                    //{
                    //    dtMeal.Rows.Add("", "", "", "");
                    //}
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
                    //if (result.Length == 0)
                    //{
                    //    dtMeal.Rows.Add("", "", "", "");
                    //}
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
                    //if (result.Length == 0)
                    //{
                    //    dtDuty.Rows.Add("", "", "", "");
                    //}
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
                    //if (result.Length == 0)
                    //{
                    //    dtMeal.Rows.Add("", "", "", "");
                    //}
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
                    //if (result.Length == 0)
                    //{
                    //    dtMeal.Rows.Add("", "", "", "");
                    //}
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
                    //if (result.Length == 0)
                    //{
                    //    dtMeal.Rows.Add("", "", "", "");
                    //}
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
                    //if (result.Length == 0)
                    //{
                    //    dtMeal.Rows.Add("", "", "", "");
                    //}
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
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }
        protected void InitializeForm(String Currency, ArrayList BaggageCode, ArrayList BaggageFee, ArrayList SportCode, ArrayList SportFee, ArrayList ComfortCode, ArrayList ConfortFee, ArrayList ComfortImg, ArrayList MealCode, ArrayList MealFee, ArrayList MealImg, ArrayList MealCode1, ArrayList MealFee1, ArrayList MealImg1, ArrayList DutyCode, ArrayList DutyFee, ArrayList DutyImg, ArrayList DrinkCode, ArrayList DrinkFee, ArrayList DrinkCode1, ArrayList DrinkFee1)
        {

            try
            {
                DataTable dtBaggage = new DataTable();
                dtBaggage.Columns.Add("SSRCode");
                dtBaggage.Columns.Add("Detail");
                dtBaggage.Columns.Add("Price");
                dtBaggage.Columns.Add("ConcatenatedField", typeof(string), "Detail + ' : ' +Price");

                //Enumerable.Range(1, 10).ToList().ForEach(i => dtBaggage.Rows.Add(i, string.Concat("Detail", i), string.Concat("Price", i)));
                DataRow row = dtBaggage.NewRow();

                String Detail = "";
                DataTable dt;

                //to avoid object index issue
                if (BaggageCode.Count > 0)
                {
                    foreach (string item in BaggageCode)
                    {
                        Detail += "'" + item + "',";
                    }
                    Detail = Detail.Substring(0, Detail.Length - 1);

                    objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                    dt = objBooking.GetDetailSSRbyCode(Detail, "PBA");
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
                //glMeals.NullText = "Select Meal";
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
                    gvPassenger.Columns["DepartMeal"].Caption = "Meal 1";
                }
                else
                {
                    divmeal1.Style.Add("display", "none");
                    gvPassenger.Columns["ConDepartMeal"].Visible = false;

                }

                //----------------------
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

                    DataView dvDrink = dtDrink.DefaultView;
                    //DataView dv = dtBaggage.DefaultView;
                    cmbDrinks.DataSource = dvDrink.ToTable(true, "SSRCode", "Detail", "Price", "ConcatenatedField");
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
                    gvPassenger.Columns["DepartDrink"].Visible = false;

                }

                if (DrinkCode1 != null && DrinkCode1.Count > 0)
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

                    DataView dvDrink1 = dtDrink1.DefaultView;
                    //dtDrink1.DefaultView.RowFilter = "SSRCode LIKE 'BW%'";
                    cmbDrinks1.DataSource = dvDrink1.ToTable(true, "SSRCode", "Detail", "Price", "ConcatenatedField");
                    cmbDrinks1.DataSource = dtDrink1.DefaultView;
                    cmbDrinks1.TextField = "ConcatenatedField";
                    cmbDrinks1.ValueField = "SSRCode";
                    cmbDrinks1.DataBind();
                    cmbDrinks1.NullText = "Default Drink";
                    Session["dtDrinkDepart2"] = dtDrink1;
                    gvPassenger.Columns["ConDepartDrink"].Caption = "Drink 1";
                }
                else
                {
                    tdDrinks1.Style.Add("display", "none");
                    gvPassenger.Columns["ConDepartDrink"].Visible = false;

                }

                DataTable dtSport = new DataTable();
                dtSport.Columns.Add("SSRCode");
                dtSport.Columns.Add("Detail");
                dtSport.Columns.Add("Price");
                dtSport.Columns.Add("ConcatenatedField", typeof(string), "Detail + ' : ' +Price");

                //Enumerable.Range(1, 10).ToList().ForEach(i => dtSport.Rows.Add(i, string.Concat("Detail", i), string.Concat("Price", i)));
                DataRow row1 = dtSport.NewRow();

                Detail = "";

                if (SportCode != null && SportCode.Count > 0)
                {
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

                DataRow rowDuty = dtDuty.NewRow();

                Detail = "";

                if (DutyCode != null && DutyCode.Count > 0)
                {
                    foreach (string item in DutyCode)
                    {
                        Detail += "'" + item + "',";
                    }
                    Detail = Detail.Substring(0, Detail.Length - 1);

                    objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                    dt = objBooking.GetDetailSSRbyCode(Detail, "WCH");
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
                DataTable dt = new DataTable();

                DataTable dtBaggage = new DataTable();
                dtBaggage.Columns.Add("SSRCode");
                dtBaggage.Columns.Add("Detail");
                dtBaggage.Columns.Add("Price");
                dtBaggage.Columns.Add("ConcatenatedField", typeof(string), "Detail + ' : ' +Price");

                //Enumerable.Range(1, 10).ToList().ForEach(i => dtBaggage.Rows.Add(i, string.Concat("Detail", i), string.Concat("Price", i)));
                DataRow row = dtBaggage.NewRow();

                String Detail = "";

                if (BaggageCode != null && BaggageCode.Count > 0)
                {
                    foreach (string item in BaggageCode)
                    {
                        Detail += "'" + item + "',";
                    }
                    Detail = Detail.Substring(0, Detail.Length - 1);

                    objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                    dt = objBooking.GetDetailSSRbyCode(Detail, "PBA");
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
                    gvPassenger2.Columns["ReturnMeal"].Caption = "Meal 1";
                }
                else
                {
                    divmeal2.Style.Add("display", "none");
                    gvPassenger2.Columns["ConReturnMeal"].Visible = false;
                }

                if (DrinkCode.Count >= 1)
                {
                    //-----------------------
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
                    cmbDrinks2.DataSource = dvDrink.ToTable(true, "SSRCode", "Detail", "Price", "ConcatenatedField");
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
                    gvPassenger2.Columns["ReturnDrink"].Visible = false;
                }
                if (DrinkCode2 != null)
                {
                    DataTable dtDrink2 = new DataTable();
                    dtDrink2.Columns.Add("SSRCode");
                    dtDrink2.Columns.Add("Detail");
                    dtDrink2.Columns.Add("Price");
                    dtDrink2.Columns.Add("ConcatenatedField", typeof(string), "Detail + '  ' +Price");

                    DataRow rowMeal2 = dtDrink2.NewRow();

                    Detail = "";
                    if (DrinkCode2.Count > 0)
                    {
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
                    gvPassenger2.Columns["ReturnDrink"].Caption = "Drink 1";
                }
                else
                {
                    //Amended by Ellis 20170330
                    //tdDrinks2.Style.Add("display", "none");
                    tdDrinks22.Style.Add("display", "none");
                    gvPassenger2.Columns["ConReturnDrink"].Visible = false;
                }

                DataTable dtSport = new DataTable();
                dtSport.Columns.Add("SSRCode");
                dtSport.Columns.Add("Detail");
                dtSport.Columns.Add("Price");
                dtSport.Columns.Add("ConcatenatedField", typeof(string), "Detail + ' : ' +Price");

                //Enumerable.Range(1, 10).ToList().ForEach(i => dtSport.Rows.Add(i, string.Concat("Detail", i), string.Concat("Price", i)));
                DataRow row1 = dtSport.NewRow();

                Detail = "";

                if (SportCode != null && SportCode.Count > 0)
                {
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
                }



                DataView dvSport = dtSport.DefaultView;
                cmbSport2.DataSource = dvSport.ToTable(true, "SSRCode", "Detail", "Price", "ConcatenatedField");

                cmbSport2.TextField = "ConcatenatedField";
                cmbSport2.ValueField = "SSRCode";
                cmbSport2.DataBind();
                cmbSport2.NullText = "No Sport Equipment";
                Session["dtSportReturn"] = dtSport;

                DataTable dtDuty = new DataTable();
                dtDuty.Columns.Add("SSRCode");
                dtDuty.Columns.Add("Detail");
                dtDuty.Columns.Add("Price");
                dtDuty.Columns.Add("Images");

                DataRow rowDuty = dtDuty.NewRow();

                Detail = "";

                if (DutyCode != null && DutyCode.Count > 0)
                {
                    foreach (string item in DutyCode)
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
        protected void Clearsession()
        {
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
        protected void BindDefaultBaggage()
        {
            string Detail = "";
            try
            {
                GetSSRAvailabilityForBookingResponse ssrResponse = (GetSSRAvailabilityForBookingResponse)Session["GetssrAvailabilityResponseForBooking"];
                DataTable dtPass = new DataTable();
                DataTable dtPass2 = new DataTable();

                if (Session["dtGridPass"] != null)
                {
                    dtPass = (DataTable)Session["dtGridPass"];
                }
                if (dtPass != null && dtPass.Rows.Count > 0)
                {
                    ////if (ssrResponse != null)
                    ////{
                    ////    if (objGeneral.IsInternationalFlight(ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.DepartureStation, ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.ArrivalStation, Request.PhysicalApplicationPath))

                    ////    //if (objBooking.InternationalFlight(cookie.Values["Departure"].ToString(), cookie.Values["Arrival"].ToString()))
                    ////    {
                    ////        DataTable dtdefaultBundle = objBooking.GetDefaultBundle("DEFAULTINTERNATIONALBAGGAGE", dtPass.Rows[0]["CarrierCode"].ToString().Trim());
                    ////        if (dtdefaultBundle != null && dtdefaultBundle.Rows.Count > 0)
                    ////        {
                    ////            DataTable dtBaggage = (DataTable)Session["dtBaggageDepart"];
                    ////            for (int i = 0; i <= dtPass.Rows.Count - 1; i++)
                    ////            {

                    ////                DataRow[] result = dtBaggage.Select("SSRCode = '" + dtdefaultBundle.Rows[0]["SYSValue"] + "'");
                    ////                foreach (DataRow row in result)
                    ////                {
                    ////                    dtPass.Rows[i]["DepartBaggage"] = row[3];
                    ////                    dtPass.Rows[i]["SSRCodeDepartBaggage"] = row[0];
                    ////                    Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                    ////                }
                    ////                dtPass.Rows[i]["PriceDepartBaggage"] = Convert.ToDecimal(Detail);
                    ////            }
                    ////            Session["dtGridPass"] = dtPass;
                    ////            gvPassenger.DataSource = dtPass;
                    ////            gvPassenger.DataBind();

                    ////            Boolean OneWay = (Boolean)Session["OneWay"];
                    ////            if (OneWay != true)
                    ////            {
                    ////                if (Session["dtGridPass2"] != null)
                    ////                {
                    ////                    dtPass2 = (DataTable)Session["dtGridPass2"];
                    ////                }
                    ////                DataTable dtBaggage2 = (DataTable)Session["dtBaggageReturn"];
                    ////                for (int i = 0; i <= dtPass2.Rows.Count - 1; i++)
                    ////                {
                    ////                    DataRow[] result = dtBaggage2.Select("SSRCode = '" + dtdefaultBundle.Rows[0]["SYSValue"] + "'");
                    ////                    foreach (DataRow row in result)
                    ////                    {
                    ////                        dtPass2.Rows[i]["ReturnBaggage"] = row[3];
                    ////                        dtPass2.Rows[i]["SSRCodeReturnBaggage"] = row[0];
                    ////                        Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                    ////                    }
                    ////                    dtPass2.Rows[i]["PriceReturnBaggage"] = Convert.ToDecimal(Detail);
                    ////                }
                    ////                Session["dtGridPass2"] = dtPass2;
                    ////                gvPassenger2.DataSource = dtPass2;
                    ////                gvPassenger2.DataBind();
                    ////            }

                    ////            first = 2; //DefaultInternationalBundle = Yes
                    ////        }
                    ////        else
                    ////        {
                    ////            first = 3; //DefaultInternationalBundle = No
                    ////        }
                    //        Session["IsNew"] = "false";
                    //        BindLabel();
                    //    }
                    //    else
                    //    {
                    //        DataTable dtdefaultBundle = objBooking.GetDefaultBundle("DEFAULTDOMESTICBAGGAGE", dtPass.Rows[0]["CarrierCode"].ToString().Trim());
                    //        if (dtdefaultBundle != null && dtdefaultBundle.Rows.Count > 0)
                    //        {
                    //            DataTable dtBaggage = (DataTable)Session["dtBaggageDepart"];
                    //            for (int i = 0; i <= dtPass.Rows.Count - 1; i++)
                    //            {

                    //                DataRow[] result = dtBaggage.Select("SSRCode = '" + dtdefaultBundle.Rows[0]["SYSValue"] + "'");
                    //                foreach (DataRow row in result)
                    //                {
                    //                    dtPass.Rows[i]["DepartBaggage"] = row[3];
                    //                    dtPass.Rows[i]["SSRCodeDepartBaggage"] = row[0];
                    //                    Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                    //                }
                    //                dtPass.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                    //            }
                    //            Session["dtGridPass"] = dtPass;
                    //            gvPassenger.DataSource = dtPass;
                    //            gvPassenger.DataBind();
                    //            Boolean OneWay = (Boolean)Session["OneWay"];
                    //            if (OneWay != true)
                    //            {
                    //                if (Session["dtGridPass2"] != null)
                    //                {
                    //                    dtPass2 = (DataTable)Session["dtGridPass2"];
                    //                }
                    //                DataTable dtBaggage2 = (DataTable)Session["dtBaggageReturn"];
                    //                for (int i = 0; i <= dtPass2.Rows.Count - 1; i++)
                    //                {
                    //                    DataRow[] result = dtBaggage2.Select("SSRCode = '" + dtdefaultBundle.Rows[0]["SYSValue"] + "'");
                    //                    foreach (DataRow row in result)
                    //                    {
                    //                        dtPass2.Rows[i]["ReturnBaggage"] = row[3];
                    //                        dtPass2.Rows[i]["SSRCodeReturnBaggage"] = row[0];
                    //                        Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                    //                    }
                    //                    dtPass2.Rows[i]["PriceReturnBaggage"] = Convert.ToDecimal(Detail);
                    //                }
                    //                Session["dtGridPass2"] = dtPass2;
                    //                gvPassenger2.DataSource = dtPass2;
                    //                gvPassenger2.DataBind();
                    //            }

                    //            first = 4; //DefaultDomesticBundle = Yes
                    //        }
                    //        else
                    //        {
                    //            first = 1; //DefaultDomesticBundle = No
                    //        }

                    Session["IsNew"] = "false";
                    BindLabel();

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
            object TotalBaggage, TotalMeal, TotalMeal1, TotalSport, TotalComfort, TotalDuty;
            object TotalBaggage2, TotalMeal2, TotalMeal12, TotalSport2, TotalComfort2, TotalDuty2;

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
                            + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalSport.Text) ? "0" : lblTotalSport.Text) + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalComfort.Text) ? "0" : lblTotalComfort.Text) + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalBaggage2.Text) ? "0" : lblTotalBaggage2.Text) + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalMeal2.Text) ? "0" : lblTotalMeal2.Text)
                            + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalSport2.Text) ? "0" : lblTotalSport2.Text) + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalComfort2.Text) ? "0" : lblTotalComfort2.Text)).ToString("N", nfi);
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
                            + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalSport.Text) ? "0" : lblTotalSport.Text) + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalComfort.Text) ? "0" : lblTotalComfort.Text)).ToString("N", nfi);
                    }

                    lblCurrency.Text = Session["Currency"].ToString();
                }

            }
            catch (Exception ex)
            {
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void SSRTab1Panel_PerformCallback(object sender, CallbackEventArgsBase e)
        {
            object TotalBaggage, TotalMeal, TotalMeal1, TotalSport, TotalComfort, TotalDuty;
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
                    //lblTotalDuty.Text = (Convert.ToDecimal(TotalDuty)).ToString("N", nfi);
                    //lblTotalAmount.Text = (Convert.ToDecimal(string.IsNullOrEmpty(lblTotalAmount.Text) ? "0" : lblTotalAmount.Text) + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalBaggage.Text) ? "0" : lblTotalBaggage.Text) + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalMeal.Text) ? "0" : lblTotalMeal.Text)
                    //        + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalSport.Text) ? "0" : lblTotalSport.Text) + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalComfort.Text) ? "0" : lblTotalComfort.Text)).ToString("N", nfi);

                    lblCurrency.Text = Session["Currency"].ToString();
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
            object TotalBaggage, TotalMeal, TotalMeal1, TotalSport, TotalComfort, TotalDuty;
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

                    //lblTotalBaggage2.Text = (Convert.ToDecimal(TotalBaggage)).ToString("N", nfi);
                    //lblTotalMeal2.Text = (Convert.ToDecimal(TotalMeal) + Convert.ToDecimal(TotalMeal1)).ToString("N", nfi);
                    //lblTotalSport2.Text = (Convert.ToDecimal(TotalSport)).ToString("N", nfi);
                    //lblTotalComfort2.Text = (Convert.ToDecimal(TotalComfort)).ToString("N", nfi);
                    //lblTotalDuty2.Text = (Convert.ToDecimal(TotalDuty)).ToString("N", nfi);
                    //lblTotalAmount.Text = (Convert.ToDecimal(string.IsNullOrEmpty(lblTotalAmount.Text) ? "0" : lblTotalAmount.Text) + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalBaggage2.Text) ? "0" : lblTotalBaggage2.Text) + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalMeal2.Text) ? "0" : lblTotalMeal2.Text)
                    //         + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalSport2.Text) ? "0" : lblTotalSport2.Text) + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalComfort2.Text) ? "0" : lblTotalComfort2.Text)).ToString("N", nfi);

                    lblCurrency.Text = Session["Currency"].ToString();
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

                    Decimal TotalAmountDepart = (Convert.ToDecimal(TotalBaggage) + Convert.ToDecimal(TotalMeal)
                            + Convert.ToDecimal(TotalMeal1) + Convert.ToDecimal(TotalSport) + Convert.ToDecimal(TotalComfort));

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

                        Decimal TotalAmountReturn = (Convert.ToDecimal(TotalBaggage2) + Convert.ToDecimal(TotalMeal2)
                            + Convert.ToDecimal(TotalMeal12) + Convert.ToDecimal(TotalSport2) + Convert.ToDecimal(TotalComfort2));

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

                        if (Session["TotalAmountReturn"] == null)
                        {
                            Session["TotalAmountReturn"] = TotalAmountReturn;
                        }
                        //TotalDuty2 = dtPass2.Compute("Sum(PriceReturnDuty)", "");


                        //lblTotalBaggage.Text = (Convert.ToDecimal(TotalBaggage)).ToString("N", nfi);
                        //lblTotalMeal.Text = (Convert.ToDecimal(TotalMeal) + Convert.ToDecimal(TotalMeal1)).ToString("N", nfi);
                        //lblTotalSport.Text = (Convert.ToDecimal(TotalSport)).ToString("N", nfi);
                        //lblTotalComfort.Text = (Convert.ToDecimal(TotalComfort)).ToString("N", nfi);
                        ////lblTotalDuty.Text = (Convert.ToDecimal(TotalDuty)).ToString("N", nfi);

                        //lblTotalBaggage2.Text = (Convert.ToDecimal(TotalBaggage2)).ToString("N", nfi);
                        //lblTotalMeal2.Text = (Convert.ToDecimal(TotalMeal2) + Convert.ToDecimal(TotalMeal1) + Convert.ToDecimal(TotalMeal12)).ToString("N", nfi);
                        //lblTotalSport2.Text = (Convert.ToDecimal(TotalSport2)).ToString("N", nfi);
                        //lblTotalComfort2.Text = (Convert.ToDecimal(TotalComfort2)).ToString("N", nfi);
                        ////lblTotalDuty2.Text = (Convert.ToDecimal(TotalDuty2)).ToString("N", nfi);
                        //lblTotalAmount.Text = (Convert.ToDecimal(TotalBaggage) + Convert.ToDecimal(TotalBaggage2) + Convert.ToDecimal(TotalMeal)
                        //    + Convert.ToDecimal(TotalMeal2) + Convert.ToDecimal(TotalMeal1) + Convert.ToDecimal(TotalMeal12)
                        //    + Convert.ToDecimal(TotalSport) + Convert.ToDecimal(TotalSport2) + Convert.ToDecimal(TotalComfort)
                        //    + Convert.ToDecimal(TotalComfort2)).ToString("N", nfi);

                        lblTotalBaggage.Text = (Convert.ToDecimal(0)).ToString("N", nfi);
                        lblTotalMeal.Text = (Convert.ToDecimal(0) + Convert.ToDecimal(0)).ToString("N", nfi);
                        lblTotalSport.Text = (Convert.ToDecimal(0)).ToString("N", nfi);
                        lblTotalComfort.Text = (Convert.ToDecimal(0)).ToString("N", nfi);
                        //lblTotalDuty.Text = (Convert.ToDecimal(TotalDuty)).ToString("N", nfi);

                        lblTotalBaggage2.Text = (Convert.ToDecimal(0)).ToString("N", nfi);
                        lblTotalMeal2.Text = (Convert.ToDecimal(0) + Convert.ToDecimal(0) + Convert.ToDecimal(0)).ToString("N", nfi);
                        lblTotalSport2.Text = (Convert.ToDecimal(0)).ToString("N", nfi);
                        lblTotalComfort2.Text = (Convert.ToDecimal(0)).ToString("N", nfi);
                        //lblTotalDuty2.Text = (Convert.ToDecimal(TotalDuty2)).ToString("N", nfi);
                        lblTotalAmount.Text = (Convert.ToDecimal(0) + Convert.ToDecimal(0) + Convert.ToDecimal(0)
                            + Convert.ToDecimal(0) + Convert.ToDecimal(0) + Convert.ToDecimal(0)
                            + Convert.ToDecimal(0) + Convert.ToDecimal(0) + Convert.ToDecimal(0)
                            + Convert.ToDecimal(0)).ToString("N", nfi);



                    }
                    else
                    {
                        //lblTotalBaggage.Text = (Convert.ToDecimal(TotalBaggage)).ToString("N", nfi);
                        //lblTotalMeal.Text = (Convert.ToDecimal(TotalMeal) + Convert.ToDecimal(TotalMeal1)).ToString("N", nfi);
                        //lblTotalSport.Text = (Convert.ToDecimal(TotalSport)).ToString("N", nfi);
                        //lblTotalComfort.Text = (Convert.ToDecimal(TotalComfort)).ToString("N", nfi);
                        ////blTotalDuty.Text = (Convert.ToDecimal(TotalDuty)).ToString("N", nfi);
                        //lblTotalAmount.Text = (Convert.ToDecimal(TotalBaggage) + Convert.ToDecimal(TotalMeal)
                        //    + Convert.ToDecimal(TotalMeal1) + Convert.ToDecimal(TotalSport) + Convert.ToDecimal(TotalComfort)).ToString("N", nfi);

                        lblTotalBaggage.Text = (Convert.ToDecimal(0)).ToString("N", nfi);
                        lblTotalMeal.Text = (Convert.ToDecimal(0) + Convert.ToDecimal(0)).ToString("N", nfi);
                        lblTotalSport.Text = (Convert.ToDecimal(0)).ToString("N", nfi);
                        lblTotalComfort.Text = (Convert.ToDecimal(0)).ToString("N", nfi);
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
                    GetPassengerList(Session["TransID"].ToString());

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
                                    gvPassenger.JSProperties["cp_result"] = "Cannot insert null baggage, please select 1 from the list";
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
                                                gvPassenger.JSProperties["cp_result"] = "Cannot replace baggage with cheaper fee, please try again";
                                                return;
                                            }
                                            else
                                            {
                                                dtPass.Rows[i]["DepartBaggage"] = args[2];
                                                dtPass.Rows[i]["SSRCodeDepartBaggage"] = args[3];
                                                dtPass.Rows[i]["PriceDepartBaggage"] = Convert.ToDecimal(Detail);
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
                                        gvPassenger.JSProperties["cp_result"] = "Cannot insert null baggage, please select 1 from the list";
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
                                                    gvPassenger.JSProperties["cp_result"] = "Cannot replace baggage with cheaper fee, please try again";
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass.Rows[i]["DepartBaggage"] = args[2];
                                                    dtPass.Rows[i]["SSRCodeDepartBaggage"] = args[3];
                                                    dtPass.Rows[i]["PriceDepartBaggage"] = Convert.ToDecimal(Detail);
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
                                            }
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
                                    gvPassenger.JSProperties["cp_result"] = "Cannot insert null Meal, please select 1 from the list";
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
                                                gvPassenger.JSProperties["cp_result"] = "Cannot replace Meal with cheaper fee, please try again";
                                                return;
                                            }
                                            else
                                            {
                                                dtPass.Rows[i]["DepartMeal"] = args[2];
                                                dtPass.Rows[i]["SSRCodeDepartMeal"] = args[3];
                                                dtPass.Rows[i]["PriceDepartMeal"] = Convert.ToDecimal(Detail);
                                            }

                                        }
                                    }
                                    else
                                    {
                                        dtPass.Rows[i]["DepartMeal"] = args[2];
                                        dtPass.Rows[i]["SSRCodeDepartMeal"] = args[3];
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
                                        gvPassenger.JSProperties["cp_result"] = "Cannot insert null Meal, please select 1 from the list";
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
                                                    gvPassenger.JSProperties["cp_result"] = "Cannot replace Meal with cheaper fee, please try again";
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass.Rows[i]["DepartMeal"] = args[2];
                                                    dtPass.Rows[i]["SSRCodeDepartMeal"] = args[3];
                                                    dtPass.Rows[i]["PriceDepartMeal"] = Convert.ToDecimal(Detail);
                                                }

                                            }
                                        }
                                        else
                                        {
                                            dtPass.Rows[i]["DepartMeal"] = args[2];
                                            dtPass.Rows[i]["SSRCodeDepartMeal"] = args[3];
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
                        if (Session["dtDrinkDepart"] != null)
                        {
                            DataTable dtDrink = (DataTable)Session["dtDrinkDepart"];
                            if (cbAllPaxMeal11.Checked == true)
                            {
                                for (int i = 0; i <= dtPass.Rows.Count - 1; i++)
                                {
                                    if (args[2].ToString() == ",")
                                    {
                                        gvPassenger.JSProperties["cp_result"] = "Cannot insert null Drink, please select 1 from the list";
                                        return;
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
                                                    dtPass.Rows[i]["PriceDepartDrink"] = Convert.ToDecimal(Detail);
                                                    dtPass.Rows[i]["DepartDrink"] = row[3];
                                                    dtPass.Rows[i]["SSRCodeDepartDrink"] = row[0];
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
                                            gvPassenger.JSProperties["cp_result"] = "Cannot insert null Drink, please select 1 from the list";
                                            return;
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
                                                        dtPass.Rows[i]["PriceDepartDrink"] = Convert.ToDecimal(Detail);
                                                        dtPass.Rows[i]["DepartDrink"] = row[3];
                                                        dtPass.Rows[i]["SSRCodeDepartDrink"] = row[0];
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
                                    gvPassenger.JSProperties["cp_result"] = "Cannot insert null Meal 2, please select 1 from the list";
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
                                                gvPassenger.JSProperties["cp_result"] = "Cannot replace Meal 2 with cheaper fee, please try again";
                                                return;
                                            }
                                            else
                                            {
                                                dtPass.Rows[i]["ConDepartMeal"] = args[2];
                                                dtPass.Rows[i]["SSRCodeConDepartMeal"] = args[3];
                                                dtPass.Rows[i]["PriceConDepartMeal"] = Convert.ToDecimal(Detail);
                                            }

                                        }
                                    }
                                    else
                                    {
                                        dtPass.Rows[i]["ConDepartMeal"] = args[2];
                                        dtPass.Rows[i]["SSRCodeConDepartMeal"] = args[3];
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
                                        gvPassenger.JSProperties["cp_result"] = "Cannot insert null Meal 2, please select 1 from the list";
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
                                                    gvPassenger.JSProperties["cp_result"] = "Cannot replace Meal 2 with cheaper fee, please try again";
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass.Rows[i]["ConDepartMeal"] = args[2];
                                                    dtPass.Rows[i]["SSRCodeConDepartMeal"] = args[3];
                                                    dtPass.Rows[i]["PriceConDepartMeal"] = Convert.ToDecimal(Detail);
                                                }

                                            }
                                        }
                                        else
                                        {
                                            dtPass.Rows[i]["ConDepartMeal"] = args[2];
                                            dtPass.Rows[i]["SSRCodeConDepartMeal"] = args[3];
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
                        if (Session["dtDrinkDepart2"] != null)
                        {
                            DataTable dtDrink = (DataTable)Session["dtDrinkDepart2"];
                            if (cbAllPaxMeal21.Checked == true)
                            {
                                for (int i = 0; i <= dtPass.Rows.Count - 1; i++)
                                {
                                    if (args[2].ToString() == ",")
                                    {
                                        gvPassenger.JSProperties["cp_result"] = "Cannot insert null Drink 2, please select 1 from the list";
                                        return;
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
                                                    dtPass.Rows[i]["PriceConDepartDrink"] = Convert.ToDecimal(Detail);
                                                    dtPass.Rows[i]["ConDepartDrink"] = row[3];
                                                    dtPass.Rows[i]["SSRCodeConDepartDrink"] = row[0];
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
                                            gvPassenger.JSProperties["cp_result"] = "Cannot insert null Drink 2, please select 1 from the list";
                                            return;
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
                                                        dtPass.Rows[i]["PriceConDepartDrink"] = Convert.ToDecimal(Detail);
                                                        dtPass.Rows[i]["ConDepartDrink"] = row[3];
                                                        dtPass.Rows[i]["SSRCodeConDepartDrink"] = row[0];
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
                                    gvPassenger.JSProperties["cp_result"] = "Cannot insert null Sport Equipment, please select 1 from the list";
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
                                                gvPassenger.JSProperties["cp_result"] = "Cannot replace Sport Equipment with cheaper fee, please try again";
                                                return;
                                            }
                                            else
                                            {
                                                dtPass.Rows[i]["DepartSport"] = args[2];
                                                dtPass.Rows[i]["SSRCodeDepartSport"] = args[3];
                                                dtPass.Rows[i]["PriceDepartSport"] = Convert.ToDecimal(Detail);
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
                                        gvPassenger.JSProperties["cp_result"] = "Cannot insert null Sport Equipment, please select 1 from the list";
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
                                                    gvPassenger.JSProperties["cp_result"] = "Cannot replace Sport Equipment with cheaper fee, please try again";
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass.Rows[i]["DepartSport"] = args[2];
                                                    dtPass.Rows[i]["SSRCodeDepartSport"] = args[3];
                                                    dtPass.Rows[i]["PriceDepartSport"] = Convert.ToDecimal(Detail);
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
                                            }
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
                                    gvPassenger.JSProperties["cp_result"] = "Cannot insert null Comfort Kit, please select 1 from the list";
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
                                                gvPassenger.JSProperties["cp_result"] = "Cannot replace Comfort Kit with cheaper fee, please try again";
                                                return;
                                            }
                                            else
                                            {
                                                dtPass.Rows[i]["DepartComfort"] = args[2];
                                                dtPass.Rows[i]["SSRCodeDepartComfort"] = args[3];
                                                dtPass.Rows[i]["PriceDepartComfort"] = Convert.ToDecimal(Detail);
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
                                        gvPassenger.JSProperties["cp_result"] = "Cannot insert null Comfort Kit, please select 1 from the list";
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
                                                    gvPassenger.JSProperties["cp_result"] = "Cannot replace Comfort Kit with cheaper fee, please try again";
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass.Rows[i]["DepartComfort"] = args[2];
                                                    dtPass.Rows[i]["SSRCodeDepartComfort"] = args[3];
                                                    dtPass.Rows[i]["PriceDepartComfort"] = Convert.ToDecimal(Detail);
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
                                            }
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
                if (string.IsNullOrEmpty(e.Parameters))
                {
                    GetPassengerList2(Session["TransID"].ToString());
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
                                    gvPassenger2.JSProperties["cp_result"] = "Cannot insert null baggage, please select 1 from the list";
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
                                                gvPassenger2.JSProperties["cp_result"] = "Cannot replace baggage with cheaper fee, please try again";
                                                return;
                                            }
                                            else
                                            {
                                                dtPass2.Rows[i]["ReturnBaggage"] = args[2];
                                                dtPass2.Rows[i]["SSRCodeReturnBaggage"] = args[3];
                                                dtPass2.Rows[i]["PriceReturnBaggage"] = Convert.ToDecimal(Detail);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dtPass2.Rows[i]["ReturnBaggage"] = args[2];
                                        dtPass2.Rows[i]["SSRCodeReturnBaggage"] = args[3];
                                        DataRow[] result2 = dtBaggage.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result2)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            dtPass2.Rows[i]["PriceReturnBaggage"] = Convert.ToDecimal(Detail);
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
                                        gvPassenger2.JSProperties["cp_result"] = "Cannot insert null baggage, please select 1 from the list";
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
                                                    gvPassenger2.JSProperties["cp_result"] = "Cannot replace baggage with cheaper fee, please try again";
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass2.Rows[i]["ReturnBaggage"] = args[2];
                                                    dtPass2.Rows[i]["SSRCodeReturnBaggage"] = args[3];
                                                    dtPass2.Rows[i]["PriceReturnBaggage"] = Convert.ToDecimal(Detail);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            dtPass2.Rows[i]["ReturnBaggage"] = args[2];
                                            dtPass2.Rows[i]["SSRCodeReturnBaggage"] = args[3];
                                            DataRow[] result = dtBaggage.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                dtPass2.Rows[i]["PriceReturnBaggage"] = Convert.ToDecimal(Detail);
                                            }
                                            //dtPass2.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                                        }
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
                                    gvPassenger2.JSProperties["cp_result"] = "Cannot insert null Meal, please select 1 from the list";
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
                                                gvPassenger2.JSProperties["cp_result"] = "Cannot replace Meal with cheaper fee, please try again";
                                                return;
                                            }
                                            else
                                            {
                                                dtPass2.Rows[i]["ReturnMeal"] = args[2];
                                                dtPass2.Rows[i]["SSRCodeReturnMeal"] = args[3];
                                                dtPass2.Rows[i]["PriceReturnMeal"] = Convert.ToDecimal(Detail);

                                            }
                                        }
                                    }
                                    else
                                    {
                                        dtPass2.Rows[i]["ReturnMeal"] = args[2];
                                        dtPass2.Rows[i]["SSRCodeReturnMeal"] = args[3];
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
                                        gvPassenger2.JSProperties["cp_result"] = "Cannot insert null Meal 2, please select 1 from the list";
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
                                                    gvPassenger2.JSProperties["cp_result"] = "Cannot replace Meal with cheaper fee, please try again";
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass2.Rows[i]["ReturnMeal"] = args[2];
                                                    dtPass2.Rows[i]["SSRCodeReturnMeal"] = args[3];
                                                    dtPass2.Rows[i]["PriceReturnMeal"] = Convert.ToDecimal(Detail);

                                                }
                                            }
                                        }
                                        else
                                        {
                                            dtPass2.Rows[i]["ReturnMeal"] = args[2];
                                            dtPass2.Rows[i]["SSRCodeReturnMeal"] = args[3];
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
                        if (Session["dtDrinkReturn"] != null)
                        {
                            DataTable dtDrink = (DataTable)Session["dtDrinkReturn"];
                            if (cbAllPaxMeal12.Checked == true)
                            {
                                for (int i = 0; i <= dtPass2.Rows.Count - 1; i++)
                                {
                                    if (args[2].ToString() == ",")
                                    {
                                        gvPassenger2.JSProperties["cp_result"] = "Cannot insert null Drink, please select 1 from the list";
                                        return;
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
                                                    dtPass2.Rows[i]["PriceReturnDrink"] = Convert.ToDecimal(Detail);
                                                    dtPass2.Rows[i]["ReturnDrink"] = row[3];
                                                    dtPass2.Rows[i]["SSRCodeReturnDrink"] = row[0];
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
                                            gvPassenger2.JSProperties["cp_result"] = "Cannot insert null Return, please select 1 from the list";
                                            return;
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
                                                        dtPass2.Rows[i]["PriceReturnDrink"] = Convert.ToDecimal(Detail);
                                                        dtPass2.Rows[i]["ReturnDrink"] = row[3];
                                                        dtPass2.Rows[i]["SSRCodeReturnDrink"] = row[0];
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
                                    gvPassenger2.JSProperties["cp_result"] = "Cannot insert null Meal 2, please select 1 from the list";
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
                                                gvPassenger2.JSProperties["cp_result"] = "Cannot replace Meal 2 with cheaper fee, please try again";
                                                return;
                                            }
                                            else
                                            {
                                                dtPass2.Rows[i]["ConReturnMeal"] = args[2];
                                                dtPass2.Rows[i]["SSRCodeConReturnMeal"] = args[3];
                                                dtPass2.Rows[i]["PriceConReturnMeal"] = Convert.ToDecimal(Detail);

                                            }
                                        }
                                    }
                                    else
                                    {
                                        dtPass2.Rows[i]["ConReturnMeal"] = args[2];
                                        dtPass2.Rows[i]["SSRCodeConReturnMeal"] = args[3];
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
                                        gvPassenger2.JSProperties["cp_result"] = "Cannot insert null Meal 2, please select 1 from the list";
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
                                                    gvPassenger2.JSProperties["cp_result"] = "Cannot replace Meal 2 with cheaper fee, please try again";
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass2.Rows[i]["ConReturnMeal"] = args[2];
                                                    dtPass2.Rows[i]["SSRCodeConReturnMeal"] = args[3];
                                                    dtPass2.Rows[i]["PriceConReturnMeal"] = Convert.ToDecimal(Detail);

                                                }
                                            }
                                        }
                                        else
                                        {
                                            dtPass2.Rows[i]["ConReturnMeal"] = args[2];
                                            dtPass2.Rows[i]["SSRCodeConReturnMeal"] = args[3];
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
                        if (Session["dtDrinkReturn2"] != null)
                        {
                            DataTable dtDrink = (DataTable)Session["dtDrinkReturn2"];
                            if (cbAllPaxMeal22.Checked == true)
                            {
                                for (int i = 0; i <= dtPass2.Rows.Count - 1; i++)
                                {
                                    if (args[2].ToString() == ",")
                                    {
                                        gvPassenger2.JSProperties["cp_result"] = "Cannot insert null Drink 2, please select 1 from the list";
                                        return;
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
                                                    dtPass2.Rows[i]["PriceConReturnDrink"] = Convert.ToDecimal(Detail);
                                                    dtPass2.Rows[i]["ConReturnDrink"] = row[3];
                                                    dtPass2.Rows[i]["SSRCodeConReturnDrink"] = row[0];
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
                                            gvPassenger2.JSProperties["cp_result"] = "Cannot insert null Drink 2, please select 1 from the list";
                                            return;
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
                                                        dtPass2.Rows[i]["PriceConReturnDrink"] = Convert.ToDecimal(Detail);
                                                        dtPass2.Rows[i]["ConReturnDrink"] = row[3];
                                                        dtPass2.Rows[i]["SSRCodeConReturnDrink"] = row[0];
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
                                    gvPassenger2.JSProperties["cp_result"] = "Cannot insert null Sport Equipment, please select 1 from the list";
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
                                                gvPassenger2.JSProperties["cp_result"] = "Cannot replace Sport Equipment with cheaper fee, please try again";
                                                return;
                                            }
                                            else
                                            {
                                                dtPass2.Rows[i]["ReturnSport"] = args[2];
                                                dtPass2.Rows[i]["SSRCodeReturnSport"] = args[3];
                                                dtPass2.Rows[i]["PriceReturnSport"] = Convert.ToDecimal(Detail);

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
                                        gvPassenger2.JSProperties["cp_result"] = "Cannot insert null Sport Equipment, please select 1 from the list";
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
                                                    gvPassenger2.JSProperties["cp_result"] = "Cannot replace Sport Equipment with cheaper fee, please try again";
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass2.Rows[i]["ReturnSport"] = args[2];
                                                    dtPass2.Rows[i]["SSRCodeReturnSport"] = args[3];
                                                    dtPass2.Rows[i]["PriceReturnSport"] = Convert.ToDecimal(Detail);

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
                                            }
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
                                    //dtPass2.Rows[i]["ReturnComfort"] = string.Empty;
                                    //dtPass2.Rows[i]["SSRCodeReturnComfort"] = string.Empty;
                                    //dtPass2.Rows[i]["PriceReturnComfort"] = 0.00;
                                    gvPassenger2.JSProperties["cp_result"] = "Cannot insert null Comfort Kit, please select 1 from the list";
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
                                                gvPassenger2.JSProperties["cp_result"] = "Cannot replace Comfort Kit with cheaper fee, please try again";
                                                return;
                                            }
                                            else
                                            {
                                                dtPass2.Rows[i]["ReturnComfort"] = args[2];
                                                dtPass2.Rows[i]["SSRCodeReturnComfort"] = args[3];
                                                dtPass2.Rows[i]["PriceReturnComfort"] = Convert.ToDecimal(Detail);

                                            }
                                        }
                                    }
                                    else
                                    {
                                        dtPass2.Rows[i]["ReturnComfort"] = args[2];
                                        dtPass2.Rows[i]["SSRCodeReturnComfort"] = args[3];
                                        DataRow[] result = dtComfort.Select("SSRCode = '" + args[3] + "'");
                                        foreach (DataRow row in result)
                                        {
                                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                            dtPass2.Rows[i]["PriceReturnComfort"] = Convert.ToDecimal(Detail);
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
                                        gvPassenger2.JSProperties["cp_result"] = "Cannot insert null Comfort Kit, please select 1 from the list";
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
                                                    gvPassenger2.JSProperties["cp_result"] = "Cannot replace Comfort Kit with cheaper fee, please try again";
                                                    return;
                                                }
                                                else
                                                {
                                                    dtPass2.Rows[i]["ReturnComfort"] = args[2];
                                                    dtPass2.Rows[i]["SSRCodeReturnComfort"] = args[3];
                                                    dtPass2.Rows[i]["PriceReturnComfort"] = Convert.ToDecimal(Detail);

                                                }
                                            }
                                        }
                                        else
                                        {
                                            dtPass2.Rows[i]["ReturnComfort"] = args[2];
                                            dtPass2.Rows[i]["SSRCodeReturnComfort"] = args[3];
                                            DataRow[] result = dtComfort.Select("SSRCode = '" + args[3] + "'");
                                            foreach (DataRow row in result)
                                            {
                                                Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                dtPass2.Rows[i]["PriceReturnComfort"] = Convert.ToDecimal(Detail);
                                            }
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
        protected void GetSellSSR(string signature, string TransID)
        {
            List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
            ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking(signature);
            BookingControl bookingControl = new BookingControl();
            string SellSessionID = "";
            SellSessionID = apiBooking.AgentLogon();
            lstbookDTLInfo = objBooking.GetAllBK_TRANSDTLFilter(TransID, 0);
            int PaxAdult = lstbookDTLInfo.Sum(item => item.PaxAdult);
            int PaxChild = lstbookDTLInfo.Sum(item => item.PaxChild);
            int PaxNum = PaxAdult + PaxChild;
            GetBookingResponse resp = bookingControl.GetBookingByPNR(lstbookDTLInfo[0].RecordLocator, SellSessionID);
            ABS.Navitaire.BookingManager.Booking responseBookingFromState = bookingControl.GetBookingFromState(SellSessionID);
            if (resp != null)
            {

                GetSSRAvailabilityForBookingResponse response = apiBooking.GetSSRAvailabilityForBooking(responseBookingFromState, SellSessionID);
                string xml = GetXMLString(resp);
                if (response != null)
                {
                    ArrayList BaggageCode = new ArrayList();
                    ArrayList BaggageFee = new ArrayList();
                    Decimal BaggageAmt = 0;

                    ArrayList SportCode = new ArrayList();
                    ArrayList SportFee = new ArrayList();
                    Decimal SportAmt = 0;

                    ArrayList ComfortCode = new ArrayList();
                    ArrayList ComfortFee = new ArrayList();
                    ArrayList ComfortImage = new ArrayList();
                    Decimal ComfortAmt = 0;

                    ArrayList MealCode = new ArrayList();
                    ArrayList MealFee = new ArrayList();
                    ArrayList MealImage = new ArrayList();
                    Decimal MealAmt = 0;

                    ArrayList DrinkCode = new ArrayList();
                    ArrayList DrinkFee = new ArrayList();
                    Decimal DrinkAmt = 0;

                    ArrayList DutyCode = new ArrayList();
                    ArrayList DutyFee = new ArrayList();
                    ArrayList DutyImage = new ArrayList();
                    Decimal DutyAmt = 0;

                    String Depart = "";
                    String Arrival = "";

                    ArrayList MealCode1 = new ArrayList();
                    ArrayList MealFee1 = new ArrayList();
                    ArrayList MealImage1 = new ArrayList();
                    Decimal MealAmt1 = 0;

                    ArrayList DrinkCode1 = new ArrayList();
                    ArrayList DrinkFee1 = new ArrayList();
                    Decimal DrinkAmt1 = 0;

                    ArrayList BaggageCode2 = new ArrayList();
                    ArrayList BaggageFee2 = new ArrayList();
                    Decimal BaggageAmt2 = 0;

                    ArrayList SportCode2 = new ArrayList();
                    ArrayList SportFee2 = new ArrayList();
                    Decimal SportAmt2 = 0;

                    ArrayList ComfortCode2 = new ArrayList();
                    ArrayList ComfortFee2 = new ArrayList();
                    ArrayList ComfortImage2 = new ArrayList();
                    Decimal ComfortAmt2 = 0;

                    ArrayList MealCode2 = new ArrayList();
                    ArrayList MealFee2 = new ArrayList();
                    ArrayList MealImage2 = new ArrayList();
                    Decimal MealAmt2 = 0;

                    ArrayList DrinkCode2 = new ArrayList();
                    ArrayList DrinkFee2 = new ArrayList();
                    Decimal DrinkAmt2 = 0;

                    ArrayList DutyCode2 = new ArrayList();
                    ArrayList DutyFee2 = new ArrayList();
                    ArrayList DutyImage2 = new ArrayList();
                    Decimal DutyAmt2 = 0;

                    String Depart2 = "";
                    String Arrival2 = "";

                    ArrayList MealCode21 = new ArrayList();
                    ArrayList MealFee21 = new ArrayList();
                    ArrayList MealImage21 = new ArrayList();
                    Decimal MealAmt21 = 0;

                    ArrayList DrinkCode21 = new ArrayList();
                    ArrayList DrinkFee21 = new ArrayList();
                    Decimal DrinkAmt21 = 0;

                    int d1 = 0, d2 = 0, d3 = 0, d4 = 0;
                    string depart1 = "", depart2 = "", transit1 = "", transit2 = "", return1 = "", return2 = "";

                    String Currency = "";

                    //xml = GetXMLString(ssrAvailabilityResponseForBooking);
                    //this entire section is simply to test the computation of SSR pricing for Meal fee in particular as segment based or leg based depending on the results
                    if (resp != null)
                    {
                        SSRAvailabilityForBookingResponse ssrAvailabilityResponseForBooking = response.SSRAvailabilityForBookingResponse;
                        Session["GetssrAvailabilityResponseForBooking"] = response;
                        List<BookingTransactionDetail> lstTransDetail = new List<BookingTransactionDetail>();

                        Session["Currency"] = resp.Booking.CurrencyCode;
                        lstTransDetail = objBooking.GetAllBK_TRANSDTLFilterAll(TransID, 0);
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

                        if (ssrAvailabilityResponseForBooking != null && ssrAvailabilityResponseForBooking.SSRSegmentList.Length != 0)
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
                                                if (SSRSegment.AvailablePaxSSRList[j].Available > PaxNum)
                                                {
                                                    if (IsDepart == true || IsDepartTransit == true)
                                                    {
                                                        if (!MealCode.Contains(SSRSegment.AvailablePaxSSRList[j].SSRCode))
                                                        {
                                                            MealCode.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                                                            MealImage.Add("http://www.airasia.com/images/common/sky-ssr/img_" + SSRSegment.AvailablePaxSSRList[j].SSRCode + ".png");
                                                            MealFee.Add(SSRAmt);
                                                        }
                                                    }
                                                    else if (IsDepartTransit2 == true)
                                                    {
                                                        if (!MealCode1.Contains(SSRSegment.AvailablePaxSSRList[j].SSRCode))
                                                        {
                                                            MealCode1.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                                                            MealImage1.Add("http://www.airasia.com/images/common/sky-ssr/img_" + SSRSegment.AvailablePaxSSRList[j].SSRCode + ".png");
                                                            MealFee1.Add(SSRAmt);
                                                        }
                                                    }
                                                    else if (IsReturn == true || IsReturnTransit == true)
                                                    {
                                                        if (!MealCode2.Contains(SSRSegment.AvailablePaxSSRList[j].SSRCode))
                                                        {
                                                            MealCode2.Add(SSRSegment.AvailablePaxSSRList[j].SSRCode);
                                                            MealImage2.Add("http://www.airasia.com/images/common/sky-ssr/img_" + SSRSegment.AvailablePaxSSRList[j].SSRCode + ".png");
                                                            MealFee2.Add(SSRAmt);
                                                        }
                                                    }
                                                    else if (IsReturnTransit2 == true)
                                                    {
                                                        if (!MealCode21.Contains(SSRSegment.AvailablePaxSSRList[j].SSRCode))
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
                        }


                        GetSSRAvailabilityForBookingResponse responses = apiBooking.GetSSRAvailabilityForBooking(responseBookingFromState, SellSessionID);
                        string xmlresponses = GetXMLString(responses);
                        if (responses != null)
                        {
                            SSRAvailabilityForBookingResponse ssrAvailabilityResponseForBookings = responses.SSRAvailabilityForBookingResponse;
                            Session["GetssrAvailabilityResponseForBooking"] = responses;
                            xml = GetXMLString(ssrAvailabilityResponseForBookings);

                            if (ssrAvailabilityResponseForBookings != null && ssrAvailabilityResponseForBookings.SSRSegmentList.Length != 0)
                            {
                                //this entire section is simply to test the computation of SSR pricing for Meal fee in particular as segment based or leg based depending on the results
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

                                    d1 = 0; d2 = 0; d3 = 0;
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


                        bool IsDeparts = false, IsDepartTransits = false, IsReturns = false, IsReturnTransits = false;
                        foreach (ABS.Navitaire.BookingManager.SSRSegment SSRSegment in ssrAvailabilityResponseForBooking.SSRSegmentList)
                        {
                            if (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == return1)
                            {
                                IsDeparts = true;
                                Currency = (string)Session["Currency"];
                                InitializeForm(Currency, BaggageCode, BaggageFee, SportCode, SportFee, ComfortCode, ComfortFee, ComfortImage, MealCode, MealFee, MealImage, null, null, null, DutyCode, DutyFee, DutyImage, DrinkCode, DrinkFee, null, null);
                            }
                            else if (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == transit1)
                            {
                                IsDepartTransits = true;
                                Currency = (string)Session["Currency"];
                                InitializeForm(Currency, BaggageCode, BaggageFee, SportCode, SportFee, ComfortCode, ComfortFee, ComfortImage, MealCode, MealFee, MealImage, MealCode1, MealFee1, MealImage1, DutyCode, DutyFee, DutyImage, DrinkCode, DrinkFee, DrinkCode1, DrinkFee1);

                            }
                            else if (SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == return2)
                            {
                                IsReturns = true;
                                Currency = (string)Session["Currency"];
                                InitializeForm2(Currency, BaggageCode2, BaggageFee2, SportCode2, SportFee2, ComfortCode2, ComfortFee2, ComfortImage2, MealCode2, MealFee2, MealImage2, null, null, null, DutyCode2, DutyFee2, DutyImage2, DrinkCode2, DrinkFee2, null, null);
                            }
                            else if (SSRSegment.LegKey.DepartureStation == depart2 && SSRSegment.LegKey.ArrivalStation == transit2)
                            {
                                IsReturnTransits = true;
                                Currency = (string)Session["Currency"];
                                InitializeForm2(Currency, BaggageCode2, BaggageFee2, SportCode2, SportFee2, ComfortCode2, ComfortFee2, ComfortImage2, MealCode2, MealFee2, MealImage2, MealCode21, MealFee21, MealImage21, DutyCode2, DutyFee2, DutyImage2, DrinkCode2, DrinkFee2, DrinkCode21, DrinkFee21);
                            }
                        }

                        if (IsDeparts == true && IsDepartTransits == false && IsReturns == false && IsReturnTransits == false)
                        {
                            Session["OneWay"] = true;
                            InitializeSetting(resp.Booking.Journeys[0].Segments[0].DepartureStation, resp.Booking.Journeys[0].Segments[0].ArrivalStation, "", "");
                        }
                        else if (IsDeparts == false && IsDepartTransits == true && IsReturns == false && IsReturnTransits == false)
                        {
                            Session["OneWay"] = true;
                            InitializeSetting(resp.Booking.Journeys[0].Segments[0].DepartureStation, resp.Booking.Journeys[0].Segments[1].ArrivalStation, "", "");
                        }
                        else if (IsDeparts == true && IsDepartTransits == false && IsReturns == true && IsReturnTransits == false)
                        {
                            Session["OneWay"] = false;
                            InitializeSetting(resp.Booking.Journeys[0].Segments[0].DepartureStation, resp.Booking.Journeys[0].Segments[0].ArrivalStation, resp.Booking.Journeys[1].Segments[0].DepartureStation, resp.Booking.Journeys[1].Segments[0].ArrivalStation);
                        }
                        else if (IsDeparts == false && IsDepartTransits == true && IsReturns == false && IsReturnTransits == true)
                        {
                            Session["OneWay"] = false;
                            InitializeSetting(resp.Booking.Journeys[0].Segments[0].DepartureStation, resp.Booking.Journeys[0].Segments[1].ArrivalStation, resp.Booking.Journeys[1].Segments[0].DepartureStation, resp.Booking.Journeys[1].Segments[1].ArrivalStation);
                        }
                        else if (IsDeparts == true && IsDepartTransits == false && IsReturns == false && IsReturnTransits == true)
                        {
                            Session["OneWay"] = false;
                            InitializeSetting(resp.Booking.Journeys[0].Segments[0].DepartureStation, resp.Booking.Journeys[0].Segments[0].ArrivalStation, resp.Booking.Journeys[1].Segments[0].DepartureStation, resp.Booking.Journeys[1].Segments[1].ArrivalStation);
                        }
                        else if (IsDeparts == false && IsDepartTransits == true && IsReturns == true && IsReturnTransits == false)
                        {
                            Session["OneWay"] = false;
                            InitializeSetting(resp.Booking.Journeys[0].Segments[0].DepartureStation, resp.Booking.Journeys[0].Segments[1].ArrivalStation, resp.Booking.Journeys[1].Segments[0].DepartureStation, resp.Booking.Journeys[1].Segments[0].ArrivalStation);
                        }
                    }

                }
            }
        }

        protected void GetPassengerList(string TransID)
        {
            try
            {
                if (Session["OneWay"] != null)
                {
                    Boolean OneWay = (Boolean)Session["OneWay"];
                    DataTable dtPass = new DataTable();
                    if (Session["dtGridPass"] != null)
                    {
                        dtPass = (DataTable)Session["dtGridPass"];
                    }
                    else
                    {
                        dtPass = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTableManage(TransID, false, "BK_TRANSDTL.SeqNo % 2 = 1");
                    }
                    if (dtPass != null && dtPass.Rows.Count > 0)
                    {
                        gvPassenger.DataSource = dtPass;
                        gvPassenger.DataBind();

                    }
                    else
                    {
                        Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.InvalidPage);
                    }
                    Session["dtGridPass"] = dtPass;
                }
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
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }

        protected void ValidatePopup(object sender, CallbackEventArgs e)
        {
            dtList1.Columns.Add("PNR");
            dtList1.Columns.Add("SSRCode");
            dtList1.Columns.Add("PassengerID");
            dtList1.Columns.Add("PassengerNo");
            dtList1.Columns.Add("Origin");
            dtList1.Columns.Add("Destination");

            dtList2.Columns.Add("PNR");
            dtList2.Columns.Add("SSRCode");
            dtList2.Columns.Add("PassengerID");
            dtList2.Columns.Add("PassengerNo");
            dtList2.Columns.Add("Origin");
            dtList2.Columns.Add("Destination");

            dtList1t.Columns.Add("PNR");
            dtList1t.Columns.Add("SSRCode");
            dtList1t.Columns.Add("PassengerID");
            dtList1t.Columns.Add("PassengerNo");
            dtList1t.Columns.Add("Origin");
            dtList1t.Columns.Add("Destination");

            dtList2t.Columns.Add("PNR");
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
                    if (Convert.ToBoolean(Session["transitdepart"]) == true)
                    {
                        foreach (DataRow dr in dataClass.Rows)
                        {
                            if (dr["SSRCodeDepartBaggage"].ToString() != "" || dr["SSRCodeDepartSport"].ToString() != ""
                                       || dr["SSRCodeDepartComfort"].ToString() != "" || dr["SSRCodeDepartDuty"].ToString() != "" || dr["SSRCodeDepartMeal"].ToString() != "")
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
                                        if (dr["SSRCodeDepartBaggage"].ToString() != "")
                                        {
                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDepartBaggage"].ToString();
                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDepartBaggage"];
                                            BK_TRANSSSRInfo.Origin = dr["Origin"].ToString();
                                            BK_TRANSSSRInfo.Destination = Session["departTransit"].ToString();
                                        }

                                    }
                                    else if (i == 1)
                                    {
                                        if (dr["SSRCodeDepartMeal"].ToString() != "")
                                        {
                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDepartMeal"].ToString();
                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDepartMeal"];
                                            BK_TRANSSSRInfo.Origin = dr["Origin"].ToString();
                                            BK_TRANSSSRInfo.Destination = Session["departTransit"].ToString();
                                        }
                                    }
                                    else if (i == 2)
                                    {
                                        if (dr["SSRCodeDepartSport"].ToString() != "")
                                        {
                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDepartSport"].ToString();
                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDepartSport"];
                                            BK_TRANSSSRInfo.Origin = dr["Origin"].ToString();
                                            BK_TRANSSSRInfo.Destination = Session["departTransit"].ToString();
                                        }
                                    }
                                    else if (i == 3)
                                    {
                                        if (dr["SSRCodeDepartComfort"].ToString() != "")
                                        {
                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDepartComfort"].ToString();
                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDepartComfort"];
                                            BK_TRANSSSRInfo.Origin = dr["Origin"].ToString();
                                            BK_TRANSSSRInfo.Destination = Session["departTransit"].ToString();
                                        }
                                    }
                                    else if (i == 4)
                                    {
                                        if (dr["SSRCodeDepartDuty"].ToString() != "")
                                        {
                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDepartDuty"].ToString();
                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDepartDuty"];
                                            BK_TRANSSSRInfo.Origin = dr["Origin"].ToString();
                                            BK_TRANSSSRInfo.Destination = Session["departTransit"].ToString();
                                        }
                                    }
                                    else if (i == 5)
                                    {
                                        if (dr["SSRCodeDepartDrink"].ToString() != "")
                                        {
                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDepartDrink"].ToString();
                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDepartDrink"];
                                            BK_TRANSSSRInfo.Origin = dr["Origin"].ToString();
                                            BK_TRANSSSRInfo.Destination = Session["departTransit"].ToString();
                                        }
                                    }

                                    listbk_transssrinfo.Add(BK_TRANSSSRInfo);
                                    listbk_transssrinfo1.Add(BK_TRANSSSRInfo);

                                }
                            }


                            if (dr["SSRCodeDepartBaggage"].ToString() != "" || dr["SSRCodeDepartSport"].ToString() != ""
                                       || dr["SSRCodeDepartComfort"].ToString() != "" || dr["SSRCodeDepartDuty"].ToString() != "" || dr["SSRCodeConDepartMeal"].ToString() != "")
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
                                        if (dr["SSRCodeDepartBaggage"].ToString() != "")
                                        {
                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDepartBaggage"].ToString();
                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDepartBaggage"];
                                            BK_TRANSSSRInfo.Origin = Session["departTransit"].ToString();
                                            BK_TRANSSSRInfo.Destination = dr["Destination"].ToString();
                                        }

                                    }
                                    else if (i == 1)
                                    {
                                        if (dr["SSRCodeConDepartMeal"].ToString() != "")
                                        {
                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeConDepartMeal"].ToString();
                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceConDepartMeal"];
                                            BK_TRANSSSRInfo.Origin = Session["departTransit"].ToString();
                                            BK_TRANSSSRInfo.Destination = dr["Destination"].ToString();
                                        }
                                    }
                                    else if (i == 2)
                                    {
                                        if (dr["SSRCodeDepartSport"].ToString() != "")
                                        {
                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDepartSport"].ToString();
                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDepartSport"];
                                            BK_TRANSSSRInfo.Origin = Session["departTransit"].ToString();
                                            BK_TRANSSSRInfo.Destination = dr["Destination"].ToString();
                                        }
                                    }
                                    else if (i == 3)
                                    {
                                        if (dr["SSRCodeDepartComfort"].ToString() != "")
                                        {
                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDepartComfort"].ToString();
                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDepartComfort"];
                                            BK_TRANSSSRInfo.Origin = Session["departTransit"].ToString();
                                            BK_TRANSSSRInfo.Destination = dr["Destination"].ToString();
                                        }
                                    }
                                    else if (i == 4)
                                    {
                                        if (dr["SSRCodeDepartDuty"].ToString() != "")
                                        {
                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDepartDuty"].ToString();
                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDepartDuty"];
                                            BK_TRANSSSRInfo.Origin = Session["departTransit"].ToString();
                                            BK_TRANSSSRInfo.Destination = dr["Destination"].ToString();
                                        }
                                    }
                                    else if (i == 5)
                                    {
                                        if (dr["SSRCodeConDepartDrink"].ToString() != "")
                                        {
                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeConDepartDrink"].ToString();
                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceConDepartDrink"];
                                            BK_TRANSSSRInfo.Origin = Session["departTransit"].ToString();
                                            BK_TRANSSSRInfo.Destination = dr["Destination"].ToString();
                                        }
                                    }

                                    listbk_transssrinfo.Add(BK_TRANSSSRInfo);
                                    listbk_transssrinfo1t.Add(BK_TRANSSSRInfo);
                                }
                            }
                        }


                    }
                    else
                    {
                        foreach (DataRow dr in dataClass.Rows)
                        {
                            if (dr["SSRCodeDepartBaggage"].ToString() != "" || dr["SSRCodeDepartMeal"].ToString() != "" || dr["SSRCodeDepartSport"].ToString() != ""
                                         || dr["SSRCodeDepartComfort"].ToString() != "" || dr["SSRCodeDepartDuty"].ToString() != "")
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
                                        if (dr["SSRCodeDepartBaggage"].ToString() != "")
                                        {
                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDepartBaggage"].ToString();
                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDepartBaggage"];
                                        }

                                    }
                                    else if (i == 1)
                                    {
                                        if (dr["SSRCodeDepartMeal"].ToString() != "")
                                        {
                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDepartMeal"].ToString();
                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDepartMeal"];
                                        }
                                    }
                                    else if (i == 2)
                                    {
                                        if (dr["SSRCodeDepartSport"].ToString() != "")
                                        {
                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDepartSport"].ToString();
                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDepartSport"];
                                        }
                                    }
                                    else if (i == 3)
                                    {
                                        if (dr["SSRCodeDepartComfort"].ToString() != "")
                                        {
                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDepartComfort"].ToString();
                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDepartComfort"];
                                        }
                                    }
                                    else if (i == 4)
                                    {
                                        if (dr["SSRCodeDepartDuty"].ToString() != "")
                                        {
                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDepartDuty"].ToString();
                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDepartDuty"];
                                        }
                                    }
                                    else if (i == 5)
                                    {
                                        if (dr["SSRCodeDepartDrink"].ToString() != "")
                                        {
                                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDepartDrink"].ToString();
                                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDepartDrink"];
                                        }
                                    }

                                    listbk_transssrinfo.Add(BK_TRANSSSRInfo);
                                    listbk_transssrinfo1.Add(BK_TRANSSSRInfo);
                                }
                            }
                        }
                    }

                    if (Session["OneWay"] != null)
                    {
                        Boolean OneWay = (Boolean)Session["OneWay"];
                        if (OneWay != true)
                        {
                            dataClass = new DataTable();
                            if (Convert.ToBoolean(Session["transitreturn"]) == true)
                            {
                                dataClass = (DataTable)HttpContext.Current.Session["dtGridPass2"];
                                foreach (DataRow dr in dataClass.Rows)
                                {
                                    if (dr["SSRCodeReturnBaggage"].ToString() != "" || dr["SSRCodeReturnMeal"].ToString() != "" || dr["SSRCodeReturnSport"].ToString() != ""
                                        || dr["SSRCodeReturnComfort"].ToString() != "" || dr["SSRCodeReturnDuty"].ToString() != "")
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
                                                if (dr["SSRCodeReturnBaggage"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeReturnBaggage"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceReturnBaggage"];
                                                    BK_TRANSSSRInfo.Origin = dr["Origin"].ToString();
                                                    BK_TRANSSSRInfo.Destination = Session["returnTransit"].ToString();
                                                }

                                            }
                                            else if (i == 1)
                                            {
                                                if (dr["SSRCodeReturnMeal"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeReturnMeal"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceReturnMeal"];
                                                    BK_TRANSSSRInfo.Origin = dr["Origin"].ToString();
                                                    BK_TRANSSSRInfo.Destination = Session["returnTransit"].ToString();
                                                }
                                            }
                                            else if (i == 2)
                                            {
                                                if (dr["SSRCodeReturnSport"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeReturnSport"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceReturnSport"];
                                                    BK_TRANSSSRInfo.Origin = dr["Origin"].ToString();
                                                    BK_TRANSSSRInfo.Destination = Session["returnTransit"].ToString();
                                                }
                                            }
                                            else if (i == 3)
                                            {
                                                if (dr["SSRCodeReturnComfort"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeReturnComfort"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceReturnComfort"];
                                                    BK_TRANSSSRInfo.Origin = dr["Origin"].ToString();
                                                    BK_TRANSSSRInfo.Destination = Session["returnTransit"].ToString();
                                                }
                                            }
                                            else if (i == 4)
                                            {
                                                if (dr["SSRCodeReturnDuty"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeReturnDuty"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceReturnDuty"];
                                                    BK_TRANSSSRInfo.Origin = dr["Origin"].ToString(); ;
                                                    BK_TRANSSSRInfo.Destination = Session["returnTransit"].ToString();
                                                }
                                            }
                                            else if (i == 5)
                                            {
                                                if (dr["SSRCodeReturnDrink"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeReturnDrink"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceReturnDrink"];
                                                    BK_TRANSSSRInfo.Origin = dr["Origin"].ToString();
                                                    BK_TRANSSSRInfo.Destination = Session["returnTransit"].ToString();
                                                }
                                            }

                                            listbk_transssrinfo.Add(BK_TRANSSSRInfo);
                                            listbk_transssrinfo2.Add(BK_TRANSSSRInfo);
                                        }
                                    }
                                }
                                foreach (DataRow dr in dataClass.Rows)
                                {
                                    if (dr["SSRCodeReturnBaggage"].ToString() != "" || dr["SSRCodeConReturnMeal"].ToString() != "" || dr["SSRCodeReturnSport"].ToString() != ""
                                        || dr["SSRCodeReturnComfort"].ToString() != "" || dr["SSRCodeReturnDuty"].ToString() != "")
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
                                                if (dr["SSRCodeReturnBaggage"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeReturnBaggage"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceReturnBaggage"];
                                                    BK_TRANSSSRInfo.Origin = Session["returnTransit"].ToString();
                                                    BK_TRANSSSRInfo.Destination = dr["Destination"].ToString();
                                                }

                                            }
                                            else if (i == 1)
                                            {
                                                if (dr["SSRCodeConReturnMeal"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeConReturnMeal"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceConReturnMeal"];
                                                    BK_TRANSSSRInfo.Origin = Session["returnTransit"].ToString();
                                                    BK_TRANSSSRInfo.Destination = dr["Destination"].ToString();
                                                }
                                            }
                                            else if (i == 2)
                                            {
                                                if (dr["SSRCodeReturnSport"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeReturnSport"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceReturnSport"];
                                                    BK_TRANSSSRInfo.Origin = Session["returnTransit"].ToString();
                                                    BK_TRANSSSRInfo.Destination = dr["Destination"].ToString();
                                                }
                                            }
                                            else if (i == 3)
                                            {
                                                if (dr["SSRCodeReturnComfort"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeReturnComfort"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceReturnComfort"];
                                                    BK_TRANSSSRInfo.Origin = Session["returnTransit"].ToString();
                                                    BK_TRANSSSRInfo.Destination = dr["Destination"].ToString();
                                                }
                                            }
                                            else if (i == 4)
                                            {
                                                if (dr["SSRCodeReturnDuty"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeReturnDuty"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceReturnDuty"];
                                                    BK_TRANSSSRInfo.Origin = Session["returnTransit"].ToString();
                                                    BK_TRANSSSRInfo.Destination = dr["Destination"].ToString();
                                                }
                                            }
                                            else if (i == 5)
                                            {
                                                if (dr["SSRCodeConReturnDrink"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeConReturnDrink"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceConReturnDrink"];
                                                    BK_TRANSSSRInfo.Origin = Session["returnTransit"].ToString();
                                                    BK_TRANSSSRInfo.Destination = dr["Destination"].ToString();
                                                }
                                            }

                                            listbk_transssrinfo.Add(BK_TRANSSSRInfo);
                                            listbk_transssrinfo2t.Add(BK_TRANSSSRInfo);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                dataClass = (DataTable)HttpContext.Current.Session["dtGridPass2"];
                                foreach (DataRow dr in dataClass.Rows)
                                {
                                    if (dr["SSRCodeReturnBaggage"].ToString() != "" || dr["SSRCodeReturnMeal"].ToString() != "" || dr["SSRCodeReturnSport"].ToString() != ""
                                         || dr["SSRCodeReturnComfort"].ToString() != "" || dr["SSRCodeReturnDuty"].ToString() != "")
                                    {
                                        for (int i = 0; i <= 6; i++)
                                        {
                                            BK_TRANSSSRInfo = new Bk_transssr();
                                            BK_TRANSSSRInfo.TransID = dr["TransID"].ToString();
                                            BK_TRANSSSRInfo.RecordLocator = dr["PNR"].ToString();
                                            BK_TRANSSSRInfo.CarrierCode = dr["CarrierCode"].ToString();
                                            BK_TRANSSSRInfo.FlightNo = dr["FlightNo"].ToString();
                                            BK_TRANSSSRInfo.Origin = dr["Origin"].ToString();
                                            BK_TRANSSSRInfo.Destination = dr["Destination"].ToString();
                                            BK_TRANSSSRInfo.PassengerID = (Convert.ToInt32(dr["PassengerID"])).ToString();
                                            BK_TRANSSSRInfo.SeqNo = (Convert.ToInt32(dr["SeqNo"]) - 1);
                                            BK_TRANSSSRInfo.SubSeqNo = i;
                                            if (i == 0)
                                            {
                                                if (dr["SSRCodeReturnBaggage"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeReturnBaggage"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceReturnBaggage"];
                                                }

                                            }
                                            else if (i == 1)
                                            {
                                                if (dr["SSRCodeReturnMeal"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeReturnMeal"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceReturnMeal"];
                                                }
                                            }
                                            else if (i == 2)
                                            {
                                                if (dr["SSRCodeReturnSport"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeReturnSport"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceReturnSport"];
                                                }
                                            }
                                            else if (i == 3)
                                            {
                                                if (dr["SSRCodeReturnComfort"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeReturnComfort"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceReturnComfort"];
                                                }
                                            }
                                            else if (i == 4)
                                            {
                                                if (dr["SSRCodeReturnDuty"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeReturnDuty"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceReturnDuty"];
                                                }
                                            }
                                            else if (i == 5)
                                            {
                                                if (dr["SSRCodeReturnDrink"].ToString() != "")
                                                {
                                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeReturnDrink"].ToString();
                                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceReturnDrink"];
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
                        dtList1.Rows.Add(item.RecordLocator, item.SSRCode, item.PassengerID, item.SeqNo, item.Origin, item.Destination);
                    }

                    DataRow row2 = dtList2.NewRow();
                    foreach (Bk_transssr item in listbk_transssrinfo2)
                    {
                        dtList2.Rows.Add(item.RecordLocator, item.SSRCode, item.PassengerID, item.SeqNo, item.Origin, item.Destination);
                    }
                    if (Convert.ToBoolean(Session["transit"]) == true)
                    {
                        DataRow rowt = dtList1t.NewRow();
                        foreach (Bk_transssr item in listbk_transssrinfo1t)
                        {
                            dtList1t.Rows.Add(item.RecordLocator, item.SSRCode, item.PassengerID, item.SeqNo, item.Origin, item.Destination);
                        }

                        DataRow row2t = dtList2t.NewRow();
                        foreach (Bk_transssr item in listbk_transssrinfo2t)
                        {
                            dtList2t.Rows.Add(item.RecordLocator, item.SSRCode, item.PassengerID, item.SeqNo, item.Origin, item.Destination);
                        }
                    }
                    SellFlight(dtList1, dtList2, dtList1t, dtList2t, listbk_transssrinfo);

                    //ClearSessionData();

                    e.Result = "";
                }
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
            }
        }

        private void SellFlight(DataTable dtList1, DataTable dtList2, DataTable dtList1t, DataTable dtList2t, List<ABS.Logic.GroupBooking.Booking.Bk_transssr> listAll)
        {
            //ClearSSRFeeValue();
            List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
            Session["totalcountpax"] = null;
            Decimal totalSSRdepart = 0;
            Decimal totalSSRReturn = 0;


            decimal TotSSRDepart = 0;
            decimal TotSSRReturn = 0;
            decimal TotSSRDepartcommit = 0;
            decimal TotSSRReturncommit = 0;

            DataRow[] row1t;
            DataRow[] row2t;
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

                    String SessionID = "";
                    for (int iii = 0; iii < listBookingDetail.Count; iii++)
                    {
                        if (listBookingDetail[iii].Origin == listBookingDetail[0].Origin)
                        {
                            string PNR = listBookingDetail[iii].RecordLocator.ToString();

                            totalSSRdepart = 0;
                            totalSSRReturn = 0;

                            if (Session["OneWay"] != null)
                            {
                                Boolean OneWay = (Boolean)Session["OneWay"];
                                if (OneWay != true)
                                {
                                    int TotalPax = Convert.ToInt16(listBookingDetail[iii].PaxAdult) + Convert.ToInt16(listBookingDetail[iii].PaxChild);

                                    ABS.Navitaire.APIBooking APIBooking = new ABS.Navitaire.APIBooking("");
                                    GetSSRAvailabilityForBookingResponse ssrResponse = (GetSSRAvailabilityForBookingResponse)Session["GetssrAvailabilityResponseForBooking"];
                                    DataRow[] row1;
                                    DataRow[] row2;
                                    if (Convert.ToBoolean(Session["transit"]) == true)
                                    {
                                        if (Session["transitdepart"] != null && Session["transitreturn"] == null)
                                        {
                                            row1 = dtList1.Select("PNR = '" + listBookingDetail[iii].RecordLocator + "'");
                                            row2 = dtList2.Select("PNR = '" + listBookingDetail[iii].RecordLocator + "'");
                                            row1t = dtList1t.Select("PNR = '" + listBookingDetail[iii].RecordLocator + "'");
                                            //row2t = dtList2t.Select("PNR = '" + listBookingDetail[iii].RecordLocator + "'");

                                            SessionID = APIBooking.SellSSRTransit(listBookingDetail[iii].RecordLocator, ssrResponse, row1, row2, row1t, null, "transitdepart");
                                        }
                                        else if (Session["transitdepart"] == null && Session["transitreturn"] != null)
                                        {
                                            row1 = dtList1.Select("PNR = '" + listBookingDetail[iii].RecordLocator + "'");
                                            row2 = dtList2.Select("PNR = '" + listBookingDetail[iii].RecordLocator + "'");
                                            //row1t = dtList1t.Select("PNR = '" + listBookingDetail[iii].RecordLocator + "'");
                                            row2t = dtList2t.Select("PNR = '" + listBookingDetail[iii].RecordLocator + "'");

                                            SessionID = APIBooking.SellSSRTransit(listBookingDetail[iii].RecordLocator, ssrResponse, row1, row2, null, row2t, "transitreturn");
                                        }
                                        else
                                        {
                                            row1 = dtList1.Select("PNR = '" + listBookingDetail[iii].RecordLocator + "'");
                                            row2 = dtList2.Select("PNR = '" + listBookingDetail[iii].RecordLocator + "'");
                                            row1t = dtList1t.Select("PNR = '" + listBookingDetail[iii].RecordLocator + "'");
                                            row2t = dtList2t.Select("PNR = '" + listBookingDetail[iii].RecordLocator + "'");

                                            SessionID = APIBooking.SellSSRTransit(listBookingDetail[iii].RecordLocator, ssrResponse, row1, row2, row1t, row2t, "transit");
                                        }


                                    }
                                    else
                                    {

                                        row1 = dtList1.Select("PNR = '" + listBookingDetail[iii].RecordLocator + "'");
                                        row2 = dtList2.Select("PNR = '" + listBookingDetail[iii].RecordLocator + "'");

                                        SessionID = APIBooking.SellSSR(listBookingDetail[iii].RecordLocator, ssrResponse, row1, row2);
                                    }

                                    ABS.Navitaire.BookingManager.Booking book = APIBooking.GetBookingFromState(SessionID);
                                    string xml = GetXMLString(book);

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
                                                        if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == listBookingDetail[iii].Origin + listBookingDetail[iii].Destination))
                                                        {
                                                            for (int y = 0; y < book.Passengers[i].PassengerFees[ii].ServiceCharges.Length; y++)
                                                            {
                                                                totalSSRdepart += book.Passengers[i].PassengerFees[ii].ServiceCharges[y].Amount;

                                                            }
                                                        }
                                                        else if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == listBookingDetail[iii].Destination + listBookingDetail[iii].Origin))
                                                        {
                                                            for (int y = 0; y < book.Passengers[i].PassengerFees[ii].ServiceCharges.Length; y++)
                                                            {
                                                                totalSSRReturn += book.Passengers[i].PassengerFees[ii].ServiceCharges[y].Amount;
                                                            }
                                                        }
                                                        else if (Session["departTransit"] != null)
                                                        {
                                                            if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == listBookingDetail[iii].Origin + Session["departTransit"].ToString()))
                                                            {
                                                                for (int y = 0; y < book.Passengers[i].PassengerFees[ii].ServiceCharges.Length; y++)
                                                                {
                                                                    totalSSRdepart += book.Passengers[i].PassengerFees[ii].ServiceCharges[y].Amount;
                                                                }
                                                            }
                                                            else if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == Session["departTransit"].ToString() + listBookingDetail[iii].Destination))
                                                            {
                                                                for (int y = 0; y < book.Passengers[i].PassengerFees[ii].ServiceCharges.Length; y++)
                                                                {
                                                                    totalSSRdepart += book.Passengers[i].PassengerFees[ii].ServiceCharges[y].Amount;
                                                                }
                                                            }
                                                            else if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == listBookingDetail[iii].Destination + Session["returnTransit"].ToString()))
                                                            {
                                                                for (int y = 0; y < book.Passengers[i].PassengerFees[ii].ServiceCharges.Length; y++)
                                                                {
                                                                    totalSSRReturn += book.Passengers[i].PassengerFees[ii].ServiceCharges[y].Amount;
                                                                }
                                                            }
                                                            else if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == Session["returnTransit"].ToString() + listBookingDetail[iii].Origin))
                                                            {
                                                                for (int y = 0; y < book.Passengers[i].PassengerFees[ii].ServiceCharges.Length; y++)
                                                                {
                                                                    totalSSRReturn += book.Passengers[i].PassengerFees[ii].ServiceCharges[y].Amount;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }


                                        if (HttpContext.Current.Session["Commit"] != null)
                                        {
                                            if (Convert.ToBoolean(Session["Commit"]) == true)
                                            {
                                                TotSSRDepartcommit += totalSSRdepart;
                                                TotSSRReturncommit += totalSSRReturn;
                                            }
                                            else
                                            {
                                                TotSSRDepart += totalSSRdepart;
                                                TotSSRReturn += totalSSRReturn;
                                            }
                                        }



                                    }
                                    else
                                    {
                                        havebalance = 0;
                                    }

                                }
                                else
                                {

                                    ABS.Navitaire.APIBooking APIBooking = new ABS.Navitaire.APIBooking("");
                                    GetSSRAvailabilityForBookingResponse ssrResponse = (GetSSRAvailabilityForBookingResponse)Session["GetssrAvailabilityResponseForBooking"];
                                    DataRow[] row1;
                                    int TotalPax = Convert.ToInt16(listBookingDetail[iii].PaxAdult) + Convert.ToInt16(listBookingDetail[iii].PaxChild);
                                    if (Convert.ToBoolean(Session["transit"]) == true)
                                    {
                                        row1 = dtList1.Select("PNR = '" + listBookingDetail[iii].RecordLocator + "'");

                                        row1t = dtList1t.Select("PNR = '" + listBookingDetail[iii].RecordLocator + "'");

                                        SessionID = APIBooking.SellSSRTransit(listBookingDetail[iii].RecordLocator, ssrResponse, row1, null, row1t, null, "transitdepart");
                                    }
                                    else
                                    {
                                        row1 = dtList1.Select("PNR = '" + listBookingDetail[iii].RecordLocator + "'");

                                        SessionID = APIBooking.SellSSR(listBookingDetail[iii].RecordLocator, ssrResponse, row1, null);
                                    }

                                    ABS.Navitaire.BookingManager.Booking book = APIBooking.GetBookingFromState(SessionID);

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
                                                        if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == listBookingDetail[iii].Origin + listBookingDetail[iii].Destination))
                                                        {
                                                            for (int y = 0; y < book.Passengers[i].PassengerFees[ii].ServiceCharges.Length; y++)
                                                            {
                                                                totalSSRdepart += book.Passengers[i].PassengerFees[ii].ServiceCharges[y].Amount;
                                                            }
                                                        }
                                                        else if (Session["departTransit"] != null)
                                                        {
                                                            if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == listBookingDetail[iii].Origin + Session["departTransit"].ToString()))
                                                            {
                                                                for (int y = 0; y < book.Passengers[i].PassengerFees[ii].ServiceCharges.Length; y++)
                                                                {
                                                                    totalSSRdepart += book.Passengers[i].PassengerFees[ii].ServiceCharges[y].Amount;
                                                                }
                                                            }
                                                            else if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == Session["departTransit"].ToString() + listBookingDetail[iii].Destination))
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


                                        if (HttpContext.Current.Session["Commit"] != null)
                                        {
                                            if (Convert.ToBoolean(Session["Commit"]) == true)
                                            {
                                                TotSSRDepartcommit += totalSSRdepart;
                                            }
                                            else
                                            {
                                                TotSSRDepart += totalSSRdepart;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        havebalance = 0;
                                    }
                                }

                                if (havebalance == 1)
                                {

                                    if (listAll != null && listAll.Count > 0)
                                    {
                                        if (OneWay != true)
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
                                                listBookingDetail[iIndexDepart].LineSSR += totalamountdue;
                                                listBookingDetail[iIndexDepart].LineTotal += totalamountdue;
                                                //listBookingDetail[iIndexDepart].PayDueAmount2 += totalamountdue;

                                                if (HttpContext.Current.Session["Commit"] != null)
                                                {
                                                    if (Convert.ToBoolean(Session["Commit"]) == true)
                                                    {
                                                        totalamountduegoingcommit += totalamountdue;
                                                    }
                                                    else
                                                    {
                                                        totalamountduegoing += totalamountdue;
                                                    }
                                                }
                                            }
                                            if (iIndexReturn >= 0)
                                            {
                                                listBookingDetail[iIndexReturn].TransID = listAll[0].TransID;
                                                listBookingDetail[iIndexReturn].RecordLocator = PNR;
                                                listBookingDetail[iIndexReturn].Signature = SessionID;
                                                decimal totalsum = totalSSRReturn;
                                                decimal totalamountdue = totalsum - listBookingDetail[iIndexReturn].LineSSR;
                                                listBookingDetail[iIndexReturn].LineSSR += totalamountdue;
                                                listBookingDetail[iIndexReturn].LineTotal += totalamountdue;
                                                //listBookingDetail[iIndexReturn].PayDueAmount2 += totalamountdue;

                                                if (HttpContext.Current.Session["Commit"] != null)
                                                {
                                                    if (Convert.ToBoolean(Session["Commit"]) == true)
                                                    {
                                                        totalamountduereturncommit += totalamountdue;
                                                    }
                                                    else
                                                    {
                                                        totalamountduereturn += totalamountdue;
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
                                                listBookingDetail[iIndexDepart].LineSSR += totalamountdue;
                                                listBookingDetail[iIndexDepart].LineTotal += totalamountdue;
                                                //listBookingDetail[iIndexDepart].PayDueAmount2 += totalamountdue;

                                                if (HttpContext.Current.Session["Commit"] != null)
                                                {
                                                    if (Convert.ToBoolean(Session["Commit"]) == true)
                                                    {
                                                        totalamountduegoingcommit += totalamountdue;
                                                    }
                                                    else
                                                    {
                                                        totalamountduegoing += totalamountdue;
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
                                                objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                                                objBooking.SaveSSRManageCommit(listAll, CoreBase.EnumSaveType.Update, PNR, "", true);
                                            }
                                            else
                                            {

                                                foreach (Bk_transssr Bk_transssrCont in listAll)
                                                {
                                                    if (Bk_transssrCont.RecordLocator == PNR)
                                                    {
                                                        Bk_transssr SSRPNR = new Bk_transssr();
                                                        SSRPNR.SeqNo = Bk_transssrCont.SeqNo;
                                                        SSRPNR.SubSeqNo = Bk_transssrCont.SubSeqNo;
                                                        SSRPNR.SSRQty = Bk_transssrCont.SSRQty;
                                                        SSRPNR.SSRRate = Bk_transssrCont.SSRRate;
                                                        SSRPNR.TotAmt1 = Bk_transssrCont.TotAmt1;
                                                        SSRPNR.TotAmt2 = Bk_transssrCont.TotAmt2;
                                                        SSRPNR.TotAmt3 = Bk_transssrCont.TotAmt3;
                                                        SSRPNR.IsOverride = Bk_transssrCont.IsOverride;
                                                        SSRPNR.TransVoid = Bk_transssrCont.TransVoid;
                                                        SSRPNR.AttemptCount = Bk_transssrCont.AttemptCount;
                                                        SSRPNR.CreateBy = Bk_transssrCont.CreateBy;
                                                        SSRPNR.SyncCreate = Bk_transssrCont.SyncCreate;
                                                        SSRPNR.SyncLastUpd = Bk_transssrCont.SyncLastUpd;
                                                        SSRPNR.LastSyncBy = Bk_transssrCont.LastSyncBy;
                                                        SSRPNR.RecordLocator = Bk_transssrCont.RecordLocator;
                                                        SSRPNR.TransID = Bk_transssrCont.TransID;
                                                        SSRPNR.CarrierCode = Bk_transssrCont.CarrierCode;
                                                        SSRPNR.FlightNo = Bk_transssrCont.FlightNo;
                                                        SSRPNR.Origin = Bk_transssrCont.Origin;
                                                        SSRPNR.Destination = Bk_transssrCont.Destination;
                                                        SSRPNR.PassengerID = Bk_transssrCont.PassengerID;
                                                        SSRPNR.SSRCode = Bk_transssrCont.SSRCode;
                                                        listSSRPNR.Add(SSRPNR);
                                                    }
                                                }
                                                HttpContext.Current.Session.Remove("ChgTransSSR");
                                                HttpContext.Current.Session.Add("ChgTransSSR", listSSRPNR);

                                                foreach (BookingTransactionDetail bkDTL in lstBookDTL)
                                                {
                                                    if (PNR == bkDTL.RecordLocator)
                                                    {
                                                        if (OneWay != true)
                                                        {
                                                            decimal totalsum = totalSSRdepart + totalSSRReturn;
                                                            decimal totalamountdue = totalsum - bkDTL.LineSSR;
                                                            bkDTL.LineSSR += totalamountdue;
                                                            bkDTL.LineTotal += totalamountdue;
                                                        }
                                                        else
                                                        {
                                                            decimal totalsum = totalSSRdepart;
                                                            decimal totalamountdue = totalsum - bkDTL.LineSSR;
                                                            bkDTL.LineSSR += totalamountdue;
                                                            bkDTL.LineTotal += totalamountdue;
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
                                                    if (OneWay != true)
                                                    {
                                                        decimal totalsum = totalSSRdepart + totalSSRReturn;
                                                        decimal totalamountdue = totalsum - bkDTL.LineSSR;
                                                        bkDTL.LineSSR += totalamountdue;
                                                        bkDTL.LineTotal += totalamountdue;
                                                    }
                                                    else
                                                    {
                                                        decimal totalsum = totalSSRdepart;
                                                        decimal totalamountdue = totalsum - bkDTL.LineSSR;
                                                        bkDTL.LineSSR += totalamountdue;
                                                        bkDTL.LineTotal += totalamountdue;
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


                        int TPax = 0, PaxAdult = 0, PaxChild = 0;

                        if (listBookingDetail.Count > 0)
                        {
                            foreach (BookingTransactionDetail b in listBookingDetail)
                            {
                                if (b.Origin == listBookingDetail[0].Origin)
                                {
                                    TPax += Convert.ToInt32(b.TotalPax);
                                    PaxAdult += Convert.ToInt32(b.PaxAdult);
                                    PaxChild += Convert.ToInt32(b.PaxChild);
                                }
                            }
                        }
                        else
                        {

                        }

                        //UpdateTotalAmount(TotSSRDepart, TotSSRReturn, ref TotalAmountGoing, ref TotalAmountReturn, TPax, PaxAdult, PaxChild, listBookingDetail);
                    }

                }
                //int cnt = 0;
                if (totalamountduegoing != 0 || totalamountduereturn != 0)
                {
                    objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();

                    if (listAll != null && listAll.Count > 0)
                    {
                        if (TotSSRDepartcommit != 0 && TotSSRReturncommit != 0)
                        {
                            if (TotSSRDepart == 0 && TotSSRReturn == 0)
                            {
                                BookingTransactionMain bookingMain = new BookingTransactionMain();
                                bookingMain = objBooking.GetSingleBK_TRANSMAIN(listAll[0].TransID);
                                decimal totalsum = TotSSRDepartcommit + TotSSRReturncommit;
                                decimal totalamountdue = (totalamountduegoingcommit + totalamountduereturncommit);
                                bookingMain.TransTotalSSR = bookingMain.TransTotalSSR + totalamountdue;
                                bookingMain.TotalAmtGoing = bookingMain.TotalAmtGoing + totalamountduegoingcommit;
                                bookingMain.TotalAmtReturn = bookingMain.TotalAmtReturn + totalamountduereturncommit;
                                bookingMain.TransSubTotal = bookingMain.TotalAmtGoing + bookingMain.TotalAmtReturn;
                                bookingMain.TransTotalAmt = bookingMain.TotalAmtGoing + bookingMain.TotalAmtReturn;
                                objBooking.UpdateTotalSSR(listAll[0].TransID, bookingMain, listBookingDetail);

                                Response.RedirectLocation = "bookingdetail.aspx?k=" + Request.QueryString["k"] + "&TransID=" + Request.QueryString["TransID"];
                            }
                            else
                            {
                                BookingTransactionMain bookingMain = new BookingTransactionMain();
                                bookingMain = objBooking.GetSingleBK_TRANSMAIN(listAll[0].TransID);
                                //decimal totalsum = TotSSRDepartcommit + TotSSRReturncommit;
                                decimal totalamountdue = (totalamountduegoingcommit + totalamountduereturncommit);
                                bookingMain.TransTotalSSR = bookingMain.TransTotalSSR + totalamountdue;
                                bookingMain.TotalAmtGoing = bookingMain.TotalAmtGoing + totalamountduegoingcommit;
                                bookingMain.TotalAmtReturn = bookingMain.TotalAmtReturn + totalamountduereturncommit;
                                bookingMain.TransSubTotal = bookingMain.TotalAmtGoing + bookingMain.TotalAmtReturn;
                                bookingMain.TransTotalAmt = bookingMain.TotalAmtGoing + bookingMain.TotalAmtReturn;
                                objBooking.UpdateTotalSSR(listAll[0].TransID, bookingMain, listBookingDetail);

                                //totalsum = TotSSRDepart + TotSSRReturn;
                                totalamountdue = (totalamountduegoing + totalamountduereturn);
                                bookingMain.TransTotalSSR = bookingMain.TransTotalSSR + totalamountdue;
                                bookingMain.TotalAmtGoing = (bookingMain.TotalAmtGoing + totalamountduegoing);
                                bookingMain.TotalAmtReturn = (bookingMain.TotalAmtReturn + totalamountduereturn);
                                bookingMain.TransSubTotal = bookingMain.TotalAmtGoing + bookingMain.TotalAmtReturn;
                                bookingMain.TransTotalAmt = bookingMain.TotalAmtGoing + bookingMain.TotalAmtReturn;

                                objBooking.FillChgTransMain(bookingMain);
                                HttpContext.Current.Session.Remove("bookingMain");
                                HttpContext.Current.Session.Add("bookingMain", bookingMain);

                                Session["ChgMode"] = "2"; //1= Manage Add-On
                                ASPxCallback.RedirectOnCallback(GroupBooking.Web.Shared.MySite.PublicPages.Payment);
                            }
                        }
                        else
                        {
                            BookingTransactionMain bookingMain = new BookingTransactionMain();
                            bookingMain = objBooking.GetSingleBK_TRANSMAIN(listAll[0].TransID);
                            //decimal totalsum = (TotSSRDepart + TotSSRReturn);
                            decimal totalamountdue = (totalamountduegoing + totalamountduereturn);
                            bookingMain.TransTotalSSR = bookingMain.TransTotalSSR + totalamountdue;
                            bookingMain.TotalAmtGoing = (bookingMain.TotalAmtGoing + totalamountduegoing);
                            bookingMain.TotalAmtReturn = (bookingMain.TotalAmtReturn + totalamountduereturn);
                            bookingMain.TransSubTotal = bookingMain.TotalAmtGoing + bookingMain.TotalAmtReturn;
                            bookingMain.TransTotalAmt = bookingMain.TotalAmtGoing + bookingMain.TotalAmtReturn;

                            objBooking.FillChgTransMain(bookingMain);
                            HttpContext.Current.Session.Remove("bookingMain");
                            HttpContext.Current.Session.Add("bookingMain", bookingMain);

                            Session["ChgMode"] = "2"; //1= Manage Add-On
                            ASPxCallback.RedirectOnCallback(GroupBooking.Web.Shared.MySite.PublicPages.Payment);
                        }
                    }

                }
                else
                {
                    if (TotSSRDepartcommit != 0 && TotSSRReturncommit != 0)
                    {
                        BookingTransactionMain bookingMain = new BookingTransactionMain();
                        bookingMain = objBooking.GetSingleBK_TRANSMAIN(listAll[0].TransID);
                        //decimal totalsum = TotSSRDepartcommit + TotSSRReturncommit;
                        decimal totalamountdue = (totalamountduegoingcommit + totalamountduereturncommit);
                        bookingMain.TransTotalSSR = bookingMain.TransTotalSSR + totalamountdue;
                        bookingMain.TotalAmtGoing = bookingMain.TotalAmtGoing + totalamountduegoingcommit;
                        bookingMain.TotalAmtReturn = bookingMain.TotalAmtReturn + totalamountduereturncommit;
                        bookingMain.TransSubTotal = bookingMain.TotalAmtGoing + bookingMain.TotalAmtReturn;
                        bookingMain.TransTotalAmt = bookingMain.TotalAmtGoing + bookingMain.TotalAmtReturn;
                        bookingMain.PaymentAmtEx2 = bookingMain.PaymentAmtEx2 + totalamountdue;
                        listBookingDetail[0].PayDueAmount2 += totalamountdue;
                        objBooking.SaveHeaderDetail(bookingMain, listBookingDetail, CoreBase.EnumSaveType.Update);
                        //objBooking.UpdateTotalSSR(listAll[0].TransID, bookingMain, listBookingDetail);
                    }

                    Response.RedirectLocation = "bookingdetail.aspx?k=" + Request.QueryString["k"] + "&TransID=" + Request.QueryString["TransID"];
                }
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
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
            int count = 0;
            //Thread.Sleep(2000);
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

                    if (args.NewValues["DepartBaggage"] != "" && args.NewValues["DepartBaggage"] != null)
                    {
                        DataTable dtBaggage = Session["dtBaggageDepart"] as DataTable;
                        if (args.NewValues["DepartBaggage"].ToString().Length == 4)
                        {
                            if (row["DepartBaggage"] != null && row["DepartBaggage"].ToString() != "")
                            {
                                DataRow[] resultBaggage = dtBaggage.Select("SSRCode = '" + args.NewValues["DepartBaggage"] + "'");
                                foreach (DataRow rows in resultBaggage)
                                {
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceDepartBaggage"]) && row["SSRCodeDepartBaggage"].ToString().Trim() == rows[0].ToString().Trim())
                                    {
                                        continue;
                                        //return;
                                    }
                                    else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["PriceDepartBaggage"]))
                                    {
                                        gvPassenger.JSProperties["cp_result"] = "Cannot replace Baggage with cheaper fee, please try again";
                                        count = 1;
                                        //return;
                                    }
                                    else
                                    {
                                        row["DepartBaggage"] = rows["ConcatenatedField"];
                                        row["PriceDepartBaggage"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeDepartBaggage"] = rows["SSRCode"];
                                    }
                                }
                            }
                            else
                            {
                                DataRow[] resultBaggage = dtBaggage.Select("SSRCode = '" + args.NewValues["DepartBaggage"] + "'");
                                foreach (DataRow rows in resultBaggage)
                                {
                                    row["DepartBaggage"] = rows["ConcatenatedField"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceDepartBaggage"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeDepartBaggage"] = rows["SSRCode"];
                                }
                            }
                        }
                        else
                        {
                            if (row["DepartBaggage"] != null && row["DepartBaggage"].ToString() != "")
                            {
                                DataRow[] resultBaggage = dtBaggage.Select("Detail = '" + args.NewValues["DepartBaggage"].ToString().Substring(0, 20) + "'");
                                foreach (DataRow rows in resultBaggage)
                                {
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceDepartBaggage"]) && row["SSRCodeDepartBaggage"].ToString().Trim() == rows[0].ToString().Trim())
                                    {
                                        continue;
                                        //return;
                                    }
                                    else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["PriceDepartBaggage"]))
                                    {
                                        gvPassenger.JSProperties["cp_result"] = "Cannot replace Baggage with cheaper fee, please try again";
                                        count = 1;
                                        //return;
                                    }
                                    else
                                    {
                                        row["DepartBaggage"] = rows["ConcatenatedField"];
                                        row["PriceDepartBaggage"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeDepartBaggage"] = rows["SSRCode"];
                                    }
                                }
                            }
                            else
                            {
                                DataRow[] resultBaggage = dtBaggage.Select("Detail = '" + args.NewValues["DepartBaggage"].ToString().Substring(0, 20) + "'");
                                foreach (DataRow rows in resultBaggage)
                                {
                                    row["DepartBaggage"] = rows["ConcatenatedField"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceDepartBaggage"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeDepartBaggage"] = rows["SSRCode"];
                                }
                            }
                        }
                    }


                    if (args.NewValues["DepartSport"] != "" && args.NewValues["DepartSport"] != null)
                    {

                        DataTable dtSport = Session["dtSportDepart"] as DataTable;
                        if (args.NewValues["DepartSport"].ToString().Length == 4)
                        {
                            if (row["DepartSport"] != null && row["DepartSport"].ToString() != "")
                            {
                                DataRow[] resultSport = dtSport.Select("SSRCode = '" + args.NewValues["DepartSport"] + "'");
                                foreach (DataRow rows in resultSport)
                                {
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceDepartSport"]) && row["SSRCodeDepartSport"].ToString().Trim() == rows[0].ToString().Trim())
                                    {
                                        continue;
                                        //return;
                                    }
                                    else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["PriceDepartSport"]))
                                    {
                                        gvPassenger.JSProperties["cp_result"] = "Cannot replace Sport Equipment with cheaper fee, please try again";
                                        count = 1;
                                        //return;
                                    }
                                    else
                                    {
                                        row["DepartSport"] = rows["ConcatenatedField"];
                                        row["PriceDepartSport"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeDepartSport"] = rows["SSRCode"];
                                    }
                                }
                            }
                            else
                            {
                                DataRow[] resultSport = dtSport.Select("SSRCode = '" + args.NewValues["DepartSport"] + "'");
                                foreach (DataRow rows in resultSport)
                                {
                                    row["DepartSport"] = rows["ConcatenatedField"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceDepartSport"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeDepartSport"] = rows["SSRCode"];
                                }
                            }
                        }
                        else
                        {
                            if (row["DepartSport"] != null && row["DepartSport"].ToString() != "")
                            {
                                DataRow[] resultMeal = dtSport.Select("Detail = '" + args.NewValues["DepartSport"].ToString().Substring(0, 21) + "'");
                                foreach (DataRow rows in resultMeal)
                                {
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceDepartSport"]) && row["SSRCodeDepartSport"].ToString().Trim() == rows[0].ToString().Trim())
                                    {
                                        continue;
                                        //return;
                                    }
                                    else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["PriceDepartSport"]))
                                    {
                                        gvPassenger.JSProperties["cp_result"] = "Cannot replace Sport Equipment with cheaper fee, please try again";
                                        count = 1;
                                        //return;
                                    }
                                    else
                                    {
                                        row["DepartSport"] = rows["ConcatenatedField"];
                                        row["PriceDepartSport"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeDepartSport"] = rows["SSRCode"];
                                    }
                                }
                            }
                            else
                            {
                                DataRow[] resultMeal = dtSport.Select("Detail = '" + args.NewValues["DepartSport"].ToString().Substring(0, 21) + "'");
                                foreach (DataRow rows in resultMeal)
                                {
                                    row["DepartSport"] = rows["ConcatenatedField"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceDepartSport"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeDepartSport"] = rows["SSRCode"];
                                }
                            }
                        }
                    }

                    if (args.NewValues["DepartMeal"] != "" && args.NewValues["DepartMeal"] != null)
                    {

                        DataTable dtMeal = Session["dtMealDepart"] as DataTable;
                        DataTable dtDrink = Session["dtDrinkDepart"] as DataTable;
                        if (args.NewValues["DepartMeal"].ToString().Length == 4)
                        {
                            if (row["DepartMeal"] != null && row["DepartMeal"].ToString() != "")
                            {
                                DataRow[] resultMeal = dtMeal.Select("SSRCode = '" + args.NewValues["DepartMeal"] + "'");
                                foreach (DataRow rows in resultMeal)
                                {
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceDepartMeal"]) && row["DepartMeal"].ToString().Trim() == rows[1].ToString().Trim())
                                    {
                                        continue;
                                        //return;
                                    }
                                    else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["PriceDepartMeal"]) && row["DepartMeal"].ToString().Trim() != rows[1].ToString().Trim())
                                    {
                                        gvPassenger.JSProperties["cp_result"] = "Cannot replace Meal with cheaper fee, please try again";
                                        count = 1;
                                        //return;
                                    }
                                    else
                                    {
                                        row["DepartMeal"] = rows["Detail"];
                                        row["PriceDepartMeal"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeDepartMeal"] = rows["SSRCode"];

                                        if (args.NewValues["DepartDrink"] == "" || args.NewValues["DepartDrink"] == null)
                                        {
                                            string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                            for (int c = 0; c < drinkcode.Length; c++)
                                            {
                                                DataRow[] resultDrink = dtDrink.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                                if (resultDrink.Length > 0)
                                                {
                                                    foreach (DataRow rowz in resultDrink)
                                                    {
                                                        row["DepartDrink"] = rows["ConcatenatedField"];
                                                        Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                                        row["PriceDepartDrink"] = Convert.ToDecimal(Detail);
                                                        row["SSRCodeDepartDrink"] = rows["SSRCode"];
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                DataRow[] resultMeal = dtMeal.Select("SSRCode = '" + args.NewValues["DepartMeal"] + "'");
                                foreach (DataRow rows in resultMeal)
                                {
                                    row["DepartMeal"] = rows["Detail"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceDepartMeal"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeDepartMeal"] = rows["SSRCode"];
                                }

                                if (args.NewValues["DepartDrink"] == "" || args.NewValues["DepartDrink"] == null)
                                {
                                    string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                    for (int c = 0; c < drinkcode.Length; c++)
                                    {
                                        DataRow[] resultDrink = dtDrink.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                        if (resultDrink.Length > 0)
                                        {
                                            foreach (DataRow rowz in resultDrink)
                                            {
                                                row["DepartDrink"] = rowz["ConcatenatedField"];
                                                Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                                row["PriceDepartDrink"] = Convert.ToDecimal(Detail);
                                                row["SSRCodeDepartDrink"] = rowz["SSRCode"];
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (row["DepartMeal"] != null && row["DepartMeal"].ToString() != "")
                            {
                                string tmp = Regex.Replace(args.NewValues["DepartMeal"].ToString().Substring(0, 10), "[^ 0-9a-zA-Z]+", "''", RegexOptions.None);
                                DataRow[] resultMeal = dtMeal.Select("Detail LIKE '%" + tmp + "%'");
                                foreach (DataRow rows in resultMeal)
                                {
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceDepartMeal"]) && row["DepartMeal"].ToString().Trim() == rows[1].ToString().Trim())
                                    {
                                        continue;
                                        //return;
                                    }
                                    else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["PriceDepartMeal"]) && row["DepartMeal"].ToString().Trim() != rows[1].ToString().Trim())
                                    {
                                        gvPassenger.JSProperties["cp_result"] = "Cannot replace Meal with cheaper fee, please try again";
                                        count = 1;
                                        //return;
                                    }
                                    else
                                    {
                                        row["DepartMeal"] = rows["Detail"];
                                        row["PriceDepartMeal"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeDepartMeal"] = rows["SSRCode"];

                                        if (args.NewValues["DepartDrink"] == "" || args.NewValues["DepartDrink"] == null)
                                        {
                                            string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                            for (int c = 0; c < drinkcode.Length; c++)
                                            {
                                                DataRow[] resultDrink = dtDrink.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                                if (resultDrink.Length > 0)
                                                {
                                                    foreach (DataRow rowz in resultDrink)
                                                    {
                                                        row["DepartDrink"] = rows["ConcatenatedField"];
                                                        Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                                        row["PriceDepartDrink"] = Convert.ToDecimal(Detail);
                                                        row["SSRCodeDepartDrink"] = rows["SSRCode"];
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string tmp = Regex.Replace(args.NewValues["DepartMeal"].ToString().Substring(0, 10), "[^ 0-9a-zA-Z]+", "''", RegexOptions.None);
                                DataRow[] resultMeal = dtMeal.Select("Detail LIKE '%" + tmp + "%'");
                                foreach (DataRow rows in resultMeal)
                                {
                                    row["DepartMeal"] = rows["Detail"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceDepartMeal"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeDepartMeal"] = rows["SSRCode"];
                                }

                                if (args.NewValues["DepartDrink"] == "" || args.NewValues["DepartDrink"] == null)
                                {
                                    string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                    for (int c = 0; c < drinkcode.Length; c++)
                                    {
                                        DataRow[] resultDrink = dtDrink.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                        if (resultDrink.Length > 0)
                                        {
                                            foreach (DataRow rowz in resultDrink)
                                            {
                                                row["DepartDrink"] = rowz["ConcatenatedField"];
                                                Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                                row["PriceDepartDrink"] = Convert.ToDecimal(Detail);
                                                row["SSRCodeDepartDrink"] = rowz["SSRCode"];
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                    }
                    if (args.NewValues["DepartDrink"] != "" && args.NewValues["DepartDrink"] != null)
                    {
                        DataTable dtMeal = Session["dtMealDepart"] as DataTable;
                        DataTable dtSport = Session["dtDrinkDepart"] as DataTable;
                        if (args.NewValues["DepartDrink"].ToString().Length == 4)
                        {
                            if (row["DepartDrink"] != null && row["DepartDrink"].ToString() != "")
                            {
                                DataRow[] resultSport = dtSport.Select("SSRCode = '" + args.NewValues["DepartDrink"] + "'");
                                foreach (DataRow rows in resultSport)
                                {
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["PriceDepartDrink"]) )
                                    {
                                        gvPassenger.JSProperties["cp_result"] = "Cannot replace Drink with cheaper fee, please try again";
                                        count = 1;
                                        //return;
                                    }
                                    else
                                    {
                                        row["DepartDrink"] = rows["ConcatenatedField"];
                                        row["PriceDepartDrink"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeDepartDrink"] = rows["SSRCode"];

                                        if (args.NewValues["DepartMeal"] == "" || args.NewValues["DepartMeal"] == null)
                                        {
                                            DataRow[] resultMeals = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                                            foreach (DataRow rowz in resultMeals)
                                            {
                                                row["DepartMeal"] = rowz["Detail"];
                                                Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                                row["PriceDepartMeal"] = Convert.ToDecimal(Detail);
                                                row["SSRCodeDepartMeal"] = rowz["SSRCode"];
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                DataRow[] resultSport = dtSport.Select("SSRCode = '" + args.NewValues["DepartDrink"] + "'");
                                foreach (DataRow rows in resultSport)
                                {
                                    row["DepartDrink"] = rows["ConcatenatedField"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceDepartDrink"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeDepartDrink"] = rows["SSRCode"];
                                }

                                if (args.NewValues["DepartMeal"] == "" || args.NewValues["DepartMeal"] == null)
                                {
                                    DataRow[] resultMeals = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                                    foreach (DataRow rowz in resultMeals)
                                    {
                                        row["DepartMeal"] = rowz["Detail"];
                                        Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                        row["PriceDepartMeal"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeDepartMeal"] = rowz["SSRCode"];
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (row["DepartDrink"] != null && row["DepartDrink"].ToString() != "")
                            {
                                string tmp = Regex.Replace(args.NewValues["DepartDrink"].ToString().Substring(0, 10), "[^ 0-9a-zA-Z]+", "''", RegexOptions.None);
                                DataRow[] resultMeal = dtSport.Select("Detail LIKE '%" + tmp + "%'");
                                //DataRow[] resultMeal = dtSport.Select("Detail = '" + args.NewValues["DepartDrink"].ToString().Substring(0, 21) + "'");
                                foreach (DataRow rows in resultMeal)
                                {
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["PriceDepartDrink"]))
                                    {
                                        gvPassenger.JSProperties["cp_result"] = "Cannot replace Drink with cheaper fee, please try again";
                                        count = 1;
                                        //return;
                                    }
                                    else
                                    {
                                        row["DepartDrink"] = rows["ConcatenatedField"];
                                        row["PriceDepartDrink"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeDepartDrink"] = rows["SSRCode"];

                                        if (args.NewValues["DepartMeal"] == "" || args.NewValues["DepartMeal"] == null)
                                        {
                                            DataRow[] resultMeals = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                                            foreach (DataRow rowz in resultMeals)
                                            {
                                                row["DepartMeal"] = rowz["Detail"];
                                                Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                                row["PriceDepartMeal"] = Convert.ToDecimal(Detail);
                                                row["SSRCodeDepartMeal"] = rowz["SSRCode"];
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string tmp = Regex.Replace(args.NewValues["DepartDrink"].ToString().Substring(0, 10), "[^ 0-9a-zA-Z]+", "''", RegexOptions.None);
                                DataRow[] resultMeal = dtSport.Select("Detail LIKE '%" + tmp + "%'");
                                foreach (DataRow rows in resultMeal)
                                {
                                    row["DepartDrink"] = rows["ConcatenatedField"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceDepartDrink"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeDepartDrink"] = rows["SSRCode"];
                                }

                                if (args.NewValues["DepartMeal"] == "" || args.NewValues["DepartMeal"] == null)
                                {
                                    DataRow[] resultMeals = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                                    foreach (DataRow rowz in resultMeals)
                                    {
                                        row["DepartMeal"] = rowz["Detail"];
                                        Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                        row["PriceDepartMeal"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeDepartMeal"] = rowz["SSRCode"];
                                    }
                                }
                            }
                        }
                    }

                    if (args.NewValues["ConDepartMeal"] != "" && args.NewValues["ConDepartMeal"] != null)
                    {
                        //row["SSRCodeConDepartMeal"] = args.NewValues["ConDepartMeal"];
                        DataTable dtMeal1 = Session["dtMealDepart2"] as DataTable;
                        DataTable dtDrink = Session["dtDrinkDepart2"] as DataTable;
                        if (args.NewValues["ConDepartMeal"].ToString().Length == 4)
                        {
                            if (row["ConDepartMeal"] != null && row["ConDepartMeal"].ToString() != "")
                            {
                                DataRow[] resultMeal1 = dtMeal1.Select("SSRCode = '" + args.NewValues["ConDepartMeal"] + "'");
                                foreach (DataRow rows in resultMeal1)
                                {
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceConDepartMeal"]) && row["ConDepartMeal"].ToString().Trim() == rows[1].ToString().Trim())
                                    {
                                        continue;
                                        //return;
                                    }
                                    else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["PriceConDepartMeal"]) && row["ConDepartMeal"].ToString().Trim() != rows[1].ToString().Trim())
                                    {
                                        gvPassenger.JSProperties["cp_result"] = "Cannot replace Meal 2 with cheaper fee, please try again";
                                        count = 1;
                                        //return;
                                    }
                                    else
                                    {
                                        row["ConDepartMeal"] = rows["Detail"];
                                        row["PriceConDepartMeal"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeConDepartMeal"] = rows["SSRCode"];

                                        if (args.NewValues["ConDepartDrink"] == "" || args.NewValues["ConDepartDrink"] == null)
                                        {
                                            string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                            for (int c = 0; c < drinkcode.Length; c++)
                                            {
                                                DataRow[] resultDrink = dtDrink.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                                if (resultDrink.Length > 0)
                                                {
                                                    foreach (DataRow rowz in resultDrink)
                                                    {
                                                        row["ConDepartDrink"] = rows["ConcatenatedField"];
                                                        Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                                        row["PriceConDepartDrink"] = Convert.ToDecimal(Detail);
                                                        row["SSRCodeConDepartDrink"] = rows["SSRCode"];
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                DataRow[] resultMeal1 = dtMeal1.Select("SSRCode = '" + args.NewValues["ConDepartMeal"] + "'");
                                foreach (DataRow rows in resultMeal1)
                                {
                                    row["ConDepartMeal"] = rows["Detail"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceConDepartMeal"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeConDepartMeal"] = rows["SSRCode"];
                                }

                                if (args.NewValues["ConDepartDrink"] == "" || args.NewValues["ConDepartDrink"] == null)
                                {
                                    string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                    for (int c = 0; c < drinkcode.Length; c++)
                                    {
                                        DataRow[] resultDrink = dtDrink.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                        if (resultDrink.Length > 0)
                                        {
                                            foreach (DataRow rowz in resultDrink)
                                            {
                                                row["ConDepartDrink"] = rowz["ConcatenatedField"];
                                                Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                                row["PriceConDepartDrink"] = Convert.ToDecimal(Detail);
                                                row["SSRCodeConDepartDrink"] = rowz["SSRCode"];
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (row["ConDepartMeal"] != null && row["ConDepartMeal"].ToString() != "")
                            {
                                string tmp = Regex.Replace(args.NewValues["ConDepartMeal"].ToString().Substring(0, 10), "[^ 0-9a-zA-Z]+", "''", RegexOptions.None);
                                DataRow[] resultMeal = dtMeal1.Select("Detail LIKE '%" + tmp + "%'");
                                foreach (DataRow rows in resultMeal)
                                {
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceConDepartMeal"]) && row["ConDepartMeal"].ToString().Trim() == rows[1].ToString().Trim())
                                    {
                                        continue;
                                        //return;
                                    }
                                    else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["PriceConDepartMeal"]) && row["ConDepartMeal"].ToString().Trim() != rows[1].ToString().Trim())
                                    {
                                        gvPassenger.JSProperties["cp_result"] = "Cannot replace Meal 2 with cheaper fee, please try again";
                                        count = 1;
                                        //return;
                                    }
                                    else
                                    {
                                        row["ConDepartMeal"] = rows["Detail"];
                                        row["PriceConDepartMeal"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeConDepartMeal"] = rows["SSRCode"];

                                        if (args.NewValues["ConDepartDrink"] == "" || args.NewValues["ConDepartDrink"] == null)
                                        {
                                            string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                            for (int c = 0; c < drinkcode.Length; c++)
                                            {
                                                DataRow[] resultDrink = dtDrink.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                                if (resultDrink.Length > 0)
                                                {
                                                    foreach (DataRow rowz in resultDrink)
                                                    {
                                                        row["ConDepartDrink"] = rows["ConcatenatedField"];
                                                        Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                                        row["PriceConDepartDrink"] = Convert.ToDecimal(Detail);
                                                        row["SSRCodeConDepartDrink"] = rows["SSRCode"];
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string tmp = Regex.Replace(args.NewValues["ConDepartMeal"].ToString().Substring(0, 10), "[^ 0-9a-zA-Z]+", "''", RegexOptions.None);
                                DataRow[] resultMeal = dtMeal1.Select("Detail LIKE '%" + tmp + "%'");
                                foreach (DataRow rows in resultMeal)
                                {
                                    row["ConDepartMeal"] = rows["Detail"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceConDepartMeal"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeConDepartMeal"] = rows["SSRCode"];
                                }

                                if (args.NewValues["ConDepartDrink"] == "" || args.NewValues["ConDepartDrink"] == null)
                                {
                                    string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                    for (int c = 0; c < drinkcode.Length; c++)
                                    {
                                        DataRow[] resultDrink = dtDrink.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                        if (resultDrink.Length > 0)
                                        {
                                            foreach (DataRow rowz in resultDrink)
                                            {
                                                row["ConDepartDrink"] = rowz["ConcatenatedField"];
                                                Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                                row["PriceConDepartDrink"] = Convert.ToDecimal(Detail);
                                                row["SSRCodeConDepartDrink"] = rowz["SSRCode"];
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                    }

                    if (args.NewValues["ConDepartDrink"] != "" && args.NewValues["ConDepartDrink"] != null)
                    {
                        DataTable dtMeal = Session["dtMealDepart2"] as DataTable;
                        DataTable dtSport = Session["dtDrinkDepart2"] as DataTable;
                        if (args.NewValues["ConDepartDrink"].ToString().Length == 4)
                        {
                            if (row["ConDepartDrink"] != null && row["ConDepartDrink"].ToString() != "")
                            {
                                DataRow[] resultSport = dtSport.Select("SSRCode = '" + args.NewValues["ConDepartDrink"] + "'");
                                foreach (DataRow rows in resultSport)
                                {
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["PriceConDepartDrink"]))
                                    {
                                        gvPassenger.JSProperties["cp_result"] = "Cannot replace Drink 2 with cheaper fee, please try again";
                                        count = 1;
                                        //return;
                                    }
                                    else
                                    {
                                        row["ConDepartDrink"] = rows["ConcatenatedField"];
                                        row["PriceConDepartDrink"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeConDepartDrink"] = rows["SSRCode"];

                                        if (args.NewValues["ConDepartMeal"] == "" || args.NewValues["ConDepartMeal"] == null)
                                        {
                                            DataRow[] resultMeals = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                                            foreach (DataRow rowz in resultMeals)
                                            {
                                                row["ConDepartMeal"] = rowz["Detail"];
                                                Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                                row["PriceConDepartMeal"] = Convert.ToDecimal(Detail);
                                                row["SSRCodeConDepartMeal"] = rowz["SSRCode"];
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                DataRow[] resultSport = dtSport.Select("SSRCode = '" + args.NewValues["ConDepartDrink"] + "'");
                                foreach (DataRow rows in resultSport)
                                {
                                    row["ConDepartDrink"] = rows["ConcatenatedField"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceConDepartDrink"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeConDepartDrink"] = rows["SSRCode"];
                                }

                                if (args.NewValues["ConDepartMeal"] == "" || args.NewValues["ConDepartMeal"] == null)
                                {
                                    DataRow[] resultMeals = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                                    foreach (DataRow rowz in resultMeals)
                                    {
                                        row["ConDepartMeal"] = rowz["Detail"];
                                        Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                        row["PriceConDepartMeal"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeConDepartMeal"] = rowz["SSRCode"];
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (row["ConDepartDrink"] != null && row["ConDepartDrink"].ToString() != "")
                            {
                                string tmp = Regex.Replace(args.NewValues["ConDepartDrink"].ToString().Substring(0, 10), "[^ 0-9a-zA-Z]+", "''", RegexOptions.None);
                                DataRow[] resultMeal = dtSport.Select("Detail LIKE '%" + tmp + "%'");
                                //DataRow[] resultMeal = dtSport.Select("Detail = '" + args.NewValues["DepartDrink"].ToString().Substring(0, 21) + "'");
                                foreach (DataRow rows in resultMeal)
                                {
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["PriceConDepartDrink"]))
                                    {
                                        gvPassenger.JSProperties["cp_result"] = "Cannot replace Drink 2 with cheaper fee, please try again";
                                        count = 1;
                                        //return;
                                    }
                                    else
                                    {
                                        row["ConDepartDrink"] = rows["ConcatenatedField"];
                                        row["PriceConDepartDrink"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeConDepartDrink"] = rows["SSRCode"];

                                        if (args.NewValues["ConDepartMeal"] == "" || args.NewValues["ConDepartMeal"] == null)
                                        {
                                            DataRow[] resultMeals = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                                            foreach (DataRow rowz in resultMeals)
                                            {
                                                row["ConDepartMeal"] = rowz["Detail"];
                                                Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                                row["PriceConDepartMeal"] = Convert.ToDecimal(Detail);
                                                row["SSRCodeConDepartMeal"] = rowz["SSRCode"];
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string tmp = Regex.Replace(args.NewValues["ConDepartDrink"].ToString().Substring(0, 10), "[^ 0-9a-zA-Z]+", "''", RegexOptions.None);
                                DataRow[] resultMeal = dtSport.Select("Detail LIKE '%" + tmp + "%'");
                                foreach (DataRow rows in resultMeal)
                                {
                                    row["ConDepartDrink"] = rows["ConcatenatedField"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceConDepartDrink"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeConDepartDrink"] = rows["SSRCode"];
                                }

                                if (args.NewValues["ConDepartMeal"] == "" || args.NewValues["ConDepartMeal"] == null)
                                {
                                    DataRow[] resultMeals = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                                    foreach (DataRow rowz in resultMeals)
                                    {
                                        row["ConDepartMeal"] = rowz["Detail"];
                                        Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                        row["PriceConDepartMeal"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeConDepartMeal"] = rowz["SSRCode"];
                                    }
                                }
                            }
                        }
                    }

                    if (args.NewValues["DepartComfort"] != "" && args.NewValues["DepartComfort"] != null)
                    {
                        //row["SSRCodeDepartComfort"] = args.NewValues["DepartComfort"];
                        DataTable dtComfort = Session["dtComfortDepart"] as DataTable;
                        if (args.NewValues["DepartComfort"].ToString().Length == 4)
                        {
                            if (row["DepartComfort"] != null && row["DepartComfort"].ToString() != "")
                            {
                                DataRow[] resultComfort = dtComfort.Select("SSRCode = '" + args.NewValues["DepartComfort"] + "'");
                                foreach (DataRow rows in resultComfort)
                                {
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceDepartComfort"]) && row["SSRCodeDepartComfort"].ToString().Trim() == rows[0].ToString().Trim())
                                    {
                                        continue;
                                        //return;
                                    }
                                    else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["PriceDepartComfort"]))
                                    {
                                        gvPassenger.JSProperties["cp_result"] = "Cannot replace Comfort Kit with cheaper fee, please try again";
                                        count = 1;
                                        //return;
                                    }
                                    else
                                    {
                                        row["DepartComfort"] = rows["Detail"];
                                        row["PriceDepartComfort"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeDepartComfort"] = rows["SSRCode"];
                                    }
                                }
                            }
                            else
                            {
                                DataRow[] resultComfort = dtComfort.Select("SSRCode = '" + args.NewValues["DepartComfort"] + "'");
                                foreach (DataRow rows in resultComfort)
                                {
                                    row["DepartComfort"] = rows["Detail"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceDepartComfort"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeDepartComfort"] = rows["SSRCode"];
                                }
                            }
                        }
                        else
                        {
                            if (row["DepartComfort"] != null && row["DepartComfort"].ToString() != "")
                            {
                                DataRow[] resultComfort = dtComfort.Select("Detail = '" + args.NewValues["DepartComfort"].ToString().Substring(0, 11) + "'");
                                foreach (DataRow rows in resultComfort)
                                {
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceDepartComfort"]) && row["SSRCodeDepartComfort"].ToString().Trim() == rows[0].ToString().Trim())
                                    {
                                        continue;
                                        //return;
                                    }
                                    else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["PriceDepartComfort"]))
                                    {
                                        gvPassenger.JSProperties["cp_result"] = "Cannot replace Comfort Kit with cheaper fee, please try again";
                                        count = 1;
                                        //return;
                                    }
                                    else
                                    {
                                        row["DepartComfort"] = rows["Detail"];
                                        row["PriceDepartComfort"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeDepartComfort"] = rows["SSRCode"];
                                    }
                                }
                            }
                            else
                            {
                                DataRow[] resultComfort = dtComfort.Select("Detail = '" + args.NewValues["DepartComfort"].ToString().Substring(0, 11) + "'");
                                foreach (DataRow rows in resultComfort)
                                {
                                    row["DepartComfort"] = rows["Detail"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceDepartComfort"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeDepartComfort"] = rows["SSRCode"];
                                }
                            }
                        }
                    }

                    if (args.NewValues["DepartDuty"] != "" && args.NewValues["DepartDuty"] != null)
                    {
                        row["SSRCodeDepartDuty"] = args.NewValues["DepartDuty"];
                        DataTable dtDuty = Session["dtDutyDepart"] as DataTable;
                        DataRow[] resultDuty = dtDuty.Select("SSRCode = '" + args.NewValues["Duty"] + "'");
                        foreach (DataRow rows in resultDuty)
                        {
                            row["DepartDuty"] = rows["Detail"];
                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                            row["PriceDepartDuty"] = Convert.ToDecimal(Detail);
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
                //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Script", "gvPassenger_EndCallback();", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "Script", "gvPassenger_EndCallback();", true);
                gvPassenger.DataSource = Session["dtGridPass"];
                gvPassenger.DataBind();

                //ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "gvPassenger_EndCallback();", true);

            }
        }

        protected void gvPassenger2_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            String Detail = "";
            Thread.Sleep(2000);
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

                    if (args.NewValues["ReturnBaggage"] != "" && args.NewValues["ReturnBaggage"] != null && args.NewValues["ReturnBaggage"].ToString().Length == 4)
                    {
                        //row["SSRCodeReturnBaggage"] = args.NewValues["ReturnBaggage"];
                        DataTable dtBaggage = Session["dtBaggageReturn"] as DataTable;
                        if (args.NewValues["ReturnBaggage"].ToString().Length == 4)
                        {
                            if (row["ReturnBaggage"] != null && row["ReturnBaggage"].ToString() != "")
                            {
                                DataRow[] resultBaggage = dtBaggage.Select("SSRCode = '" + args.NewValues["ReturnBaggage"] + "'");
                                foreach (DataRow rows in resultBaggage)
                                {
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceReturnBaggage"]) && row["SSRCodeReturnBaggage"].ToString().Trim() == rows[0].ToString().Trim())
                                    {
                                        continue;
                                        //return;
                                    }
                                    else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["PriceReturnBaggage"]))
                                    {
                                        gvPassenger2.JSProperties["cp_result"] = "Cannot replace Baggage with cheaper fee, please try again";
                                        //count = 1;
                                        //return;
                                    }
                                    else
                                    {
                                        row["ReturnBaggage"] = rows["ConcatenatedField"];
                                        row["PriceReturnBaggage"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeReturnBaggage"] = rows["SSRCode"];
                                    }
                                }
                            }
                            else
                            {
                                DataRow[] resultBaggage = dtBaggage.Select("SSRCode = '" + args.NewValues["ReturnBaggage"] + "'");
                                foreach (DataRow rows in resultBaggage)
                                {
                                    row["ReturnBaggage"] = rows["ConcatenatedField"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceReturnBaggage"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeReturnBaggage"] = rows["SSRCode"];
                                }
                            }
                        }
                        else
                        {
                            if (row["ReturnBaggage"] != null && row["ReturnBaggage"].ToString() != "")
                            {
                                DataRow[] resultBaggage = dtBaggage.Select("Detail = '" + args.NewValues["ReturnBaggage"].ToString().Substring(0, 20) + "'");
                                foreach (DataRow rows in resultBaggage)
                                {
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceReturnBaggage"]) && row["SSRCodeReturnBaggage"].ToString().Trim() == rows[0].ToString().Trim())
                                    {
                                        continue;
                                        //return;
                                    }
                                    else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["PriceReturnBaggage"]))
                                    {
                                        gvPassenger2.JSProperties["cp_result"] = "Cannot replace Baggage with cheaper fee, please try again";
                                        //count = 1;
                                        //return;
                                    }
                                    else
                                    {
                                        row["ReturnBaggage"] = rows["ConcatenatedField"];
                                        row["PriceReturnBaggage"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeReturnBaggage"] = rows["SSRCode"];
                                    }
                                }
                            }
                            else
                            {
                                DataRow[] resultBaggage = dtBaggage.Select("Detail = '" + args.NewValues["ReturnBaggage"].ToString().Substring(0, 20) + "'");
                                //DataRow[] resultBaggage = dtBaggage.Select("ConcatenatedField = '" + args.NewValues["Baggage"] + "'");
                                foreach (DataRow rows in resultBaggage)
                                {
                                    row["ReturnBaggage"] = rows["ConcatenatedField"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceReturnBaggage"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeReturnBaggage"] = rows["SSRCode"];
                                }
                            }
                        }

                    }

                    if (args.NewValues["ReturnSport"] != "" && args.NewValues["ReturnSport"] != null)
                    {
                        //row["SSRCodeReturnSport"] = args.NewValues["ReturnSport"];
                        DataTable dtSport = Session["dtSportReturn"] as DataTable;
                        if (args.NewValues["ReturnSport"].ToString().Length == 4)
                        {
                            if (row["ReturnSport"] != null && row["ReturnSport"].ToString() != "")
                            {
                                DataRow[] resultSport = dtSport.Select("SSRCode = '" + args.NewValues["ReturnSport"] + "'");
                                foreach (DataRow rows in resultSport)
                                {
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceReturnSport"]) && row["SSRCodeReturnSport"].ToString().Trim() == rows[0].ToString().Trim())
                                    {
                                        continue;
                                        //return;
                                    }
                                    else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["PriceReturnSport"]))
                                    {
                                        gvPassenger2.JSProperties["cp_result"] = "Cannot replace Sport Equipment with cheaper fee, please try again";
                                        //count = 1;
                                        //return;
                                    }
                                    else
                                    {
                                        row["ReturnSport"] = rows["ConcatenatedField"];
                                        row["PriceReturnSport"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeReturnSport"] = rows["SSRCode"];
                                    }
                                }
                            }
                            else
                            {
                                DataRow[] resultSport = dtSport.Select("SSRCode = '" + args.NewValues["ReturnSport"] + "'");
                                foreach (DataRow rows in resultSport)
                                {
                                    row["ReturnSport"] = rows["ConcatenatedField"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceReturnSport"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeReturnSport"] = rows["SSRCode"];
                                }
                            }
                        }
                        else
                        {
                            if (row["ReturnSport"] != null && row["ReturnSport"].ToString() != "")
                            {
                                DataRow[] resultMeal = dtSport.Select("Detail = '" + args.NewValues["ReturnSport"].ToString().Substring(0, 21) + "'");
                                foreach (DataRow rows in resultMeal)
                                {
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceReturnSport"]) && row["SSRCodeReturnSport"].ToString().Trim() == rows[0].ToString().Trim())
                                    {
                                        continue;
                                        //return;
                                    }
                                    else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["PriceReturnSport"]))
                                    {
                                        gvPassenger2.JSProperties["cp_result"] = "Cannot replace Sport Equipment with cheaper fee, please try again";
                                        //count = 1;
                                        //return;
                                    }
                                    else
                                    {
                                        row["ReturnSport"] = rows["ConcatenatedField"];
                                        row["PriceReturnSport"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeReturnSport"] = rows["SSRCode"];
                                    }
                                }
                            }
                            else
                            {
                                DataRow[] resultMeal = dtSport.Select("Detail = '" + args.NewValues["ReturnSport"].ToString().Substring(0, 21) + "'");
                                //DataRow[] resultMeal = dtSport.Select("ConcatenatedField = '" + args.NewValues["ReturnSport"] + "'");
                                foreach (DataRow rows in resultMeal)
                                {
                                    row["ReturnSport"] = rows["ConcatenatedField"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceReturnSport"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeReturnSport"] = rows["SSRCode"];
                                }
                            }
                        }


                    }

                    if (args.NewValues["ReturnMeal"] != "" && args.NewValues["ReturnMeal"] != null)
                    {
                        //row["SSRCodeReturnMeal"] = args.NewValues["ReturnMeal"];
                        DataTable dtDrink = Session["dtDrinkReturn"] as DataTable;
                        DataTable dtMeal = Session["dtMealReturn"] as DataTable;
                        if (args.NewValues["ReturnMeal"].ToString().Length == 4)
                        {
                            if (row["ReturnMeal"] != null && row["ReturnMeal"].ToString() != "")
                            {
                                DataRow[] resultMeal = dtMeal.Select("SSRCode = '" + args.NewValues["ReturnMeal"] + "'");
                                foreach (DataRow rows in resultMeal)
                                {
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceReturnMeal"]) && row["ReturnMeal"].ToString().Trim() == rows[1].ToString().Trim())
                                    {
                                        continue;
                                        //return;
                                    }
                                    else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["PriceReturnMeal"]) && row["ReturnMeal"].ToString().Trim() != rows[1].ToString().Trim())
                                    {
                                        gvPassenger2.JSProperties["cp_result"] = "Cannot replace Meal with cheaper fee, please try again";
                                        //count = 1;
                                        //return;
                                    }
                                    else
                                    {
                                        row["ReturnMeal"] = rows["Detail"];
                                        row["PriceReturnMeal"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeReturnMeal"] = rows["SSRCode"];

                                        if (args.NewValues["ReturnDrink"] == "" || args.NewValues["ReturnDrink"] == null)
                                        {
                                            string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                            for (int c = 0; c < drinkcode.Length; c++)
                                            {
                                                DataRow[] resultDrink = dtDrink.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                                if (resultDrink.Length > 0)
                                                {
                                                    foreach (DataRow rowz in resultDrink)
                                                    {
                                                        row["ReturnDrink"] = rows["ConcatenatedField"];
                                                        Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                                        row["PriceReturnDrink"] = Convert.ToDecimal(Detail);
                                                        row["SSRCodeReturnDrink"] = rows["SSRCode"];
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                DataRow[] resultMeal = dtMeal.Select("SSRCode = '" + args.NewValues["ReturnMeal"] + "'");
                                foreach (DataRow rows in resultMeal)
                                {
                                    row["ReturnMeal"] = rows["Detail"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceReturnMeal"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeReturnMeal"] = rows["SSRCode"];
                                }

                                if (args.NewValues["ReturnDrink"] == "" || args.NewValues["ReturnDrink"] == null)
                                {
                                    string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                    for (int c = 0; c < drinkcode.Length; c++)
                                    {
                                        DataRow[] resultDrink = dtDrink.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                        if (resultDrink.Length > 0)
                                        {
                                            foreach (DataRow rowz in resultDrink)
                                            {
                                                row["ReturnDrink"] = rowz["ConcatenatedField"];
                                                Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                                row["PriceReturnDrink"] = Convert.ToDecimal(Detail);
                                                row["SSRCodeReturnDrink"] = rowz["SSRCode"];
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (row["ReturnMeal"] != null && row["ReturnMeal"].ToString() != "")
                            {
                                string tmp = Regex.Replace(args.NewValues["ReturnMeal"].ToString().Substring(0, 10), "[^ 0-9a-zA-Z]+", "''", RegexOptions.None);
                                DataRow[] resultMeal = dtMeal.Select("Detail LIKE '%" + tmp + "%'");
                                foreach (DataRow rows in resultMeal)
                                {
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceReturnMeal"]) && row["ReturnMeal"].ToString().Trim() == rows[1].ToString().Trim())
                                    {
                                        continue;
                                        //return;
                                    }
                                    else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["PriceReturnMeal"]) && row["ReturnMeal"].ToString().Trim() != rows[1].ToString().Trim())
                                    {
                                        gvPassenger2.JSProperties["cp_result"] = "Cannot replace Meal with cheaper fee, please try again";
                                        //count = 1;
                                        //return;
                                    }
                                    else
                                    {
                                        row["ReturnMeal"] = rows["Detail"];
                                        row["PriceReturnMeal"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeReturnMeal"] = rows["SSRCode"];

                                        if (args.NewValues["ReturnDrink"] == "" || args.NewValues["ReturnDrink"] == null)
                                        {
                                            string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                            for (int c = 0; c < drinkcode.Length; c++)
                                            {
                                                DataRow[] resultDrink = dtDrink.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                                if (resultDrink.Length > 0)
                                                {
                                                    foreach (DataRow rowz in resultDrink)
                                                    {
                                                        row["ReturnDrink"] = rows["ConcatenatedField"];
                                                        Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                                        row["PriceReturnDrink"] = Convert.ToDecimal(Detail);
                                                        row["SSRCodeReturnDrink"] = rows["SSRCode"];
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string tmp = Regex.Replace(args.NewValues["ReturnMeal"].ToString().Substring(0, 10), "[^ 0-9a-zA-Z]+", "''", RegexOptions.None);
                                DataRow[] resultMeal = dtMeal.Select("Detail LIKE '%" + tmp + "%'");
                                foreach (DataRow rows in resultMeal)
                                {
                                    row["ReturnMeal"] = rows["Detail"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceReturnMeal"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeReturnMeal"] = rows["SSRCode"];
                                }

                                if (args.NewValues["ReturnDrink"] == "" || args.NewValues["ReturnDrink"] == null)
                                {
                                    string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                    for (int c = 0; c < drinkcode.Length; c++)
                                    {
                                        DataRow[] resultDrink = dtDrink.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                        if (resultDrink.Length > 0)
                                        {
                                            foreach (DataRow rowz in resultDrink)
                                            {
                                                row["ReturnDrink"] = rowz["ConcatenatedField"];
                                                Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                                row["PriceReturnDrink"] = Convert.ToDecimal(Detail);
                                                row["SSRCodeReturnDrink"] = rowz["SSRCode"];
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                    }

                    if (args.NewValues["ReturnDrink"] != "" && args.NewValues["ReturnDrink"] != null)
                    {
                        //row["SSRCodeReturnDrink"] = args.NewValues["ReturnDrink"];
                        DataTable dtMeal = Session["dtMealReturn"] as DataTable;
                        DataTable dtSport = Session["dtDrinkReturn"] as DataTable;
                        if (args.NewValues["ReturnDrink"].ToString().Length == 4)
                        {
                            if (row["ReturnDrink"] != null && row["ReturnDrink"].ToString() != "")
                            {
                                DataRow[] resultSport = dtSport.Select("SSRCode = '" + args.NewValues["ReturnDrink"] + "'");
                                foreach (DataRow rows in resultSport)
                                {
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["PriceReturnDrink"]))
                                    {
                                        gvPassenger2.JSProperties["cp_result"] = "Cannot replace Drink with cheaper fee, please try again";
                                        //count = 1;
                                        //return;
                                    }
                                    else
                                    {
                                        row["ReturnDrink"] = rows["ConcatenatedField"];
                                        row["PriceReturnDrink"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeReturnDrink"] = rows["SSRCode"];

                                        if (args.NewValues["ReturnMeal"] == "" || args.NewValues["ReturnMeal"] == null)
                                        {
                                            DataRow[] resultMeals = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                                            foreach (DataRow rowz in resultMeals)
                                            {
                                                row["ReturnMeal"] = rowz["Detail"];
                                                Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                                row["PriceReturnMeal"] = Convert.ToDecimal(Detail);
                                                row["SSRCodeReturnMeal"] = rowz["SSRCode"];
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                DataRow[] resultSport = dtSport.Select("SSRCode = '" + args.NewValues["ReturnDrink"] + "'");
                                foreach (DataRow rows in resultSport)
                                {
                                    row["ReturnDrink"] = rows["ConcatenatedField"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceReturnDrink"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeReturnDrink"] = rows["SSRCode"];
                                }

                                if (args.NewValues["ReturnMeal"] == "" || args.NewValues["ReturnMeal"] == null)
                                {
                                    DataRow[] resultMeals = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                                    foreach (DataRow rowz in resultMeals)
                                    {
                                        row["ReturnMeal"] = rowz["Detail"];
                                        Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                        row["PriceReturnMeal"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeReturnMeal"] = rowz["SSRCode"];
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (row["ReturnDrink"] != null && row["ReturnDrink"].ToString() != "")
                            {
                                string tmp = Regex.Replace(args.NewValues["ReturnDrink"].ToString().Substring(0, 10), "[^ 0-9a-zA-Z]+", "''", RegexOptions.None);
                                DataRow[] resultMeal = dtSport.Select("Detail LIKE '%" + tmp + "%'");
                                foreach (DataRow rows in resultMeal)
                                {
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["PriceReturnDrink"]))
                                    {
                                        gvPassenger2.JSProperties["cp_result"] = "Cannot replace Drink with cheaper fee, please try again";
                                        //count = 1;
                                        //return;
                                    }
                                    else
                                    {
                                        row["ReturnDrink"] = rows["ConcatenatedField"];
                                        row["PriceReturnDrink"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeReturnDrink"] = rows["SSRCode"];

                                        if (args.NewValues["ReturnMeal"] == "" || args.NewValues["ReturnMeal"] == null)
                                        {
                                            DataRow[] resultMeals = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                                            foreach (DataRow rowz in resultMeals)
                                            {
                                                row["ReturnMeal"] = rowz["Detail"];
                                                Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                                row["PriceReturnMeal"] = Convert.ToDecimal(Detail);
                                                row["SSRCodeReturnMeal"] = rowz["SSRCode"];
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string tmp = Regex.Replace(args.NewValues["ReturnDrink"].ToString().Substring(0, 10), "[^ 0-9a-zA-Z]+", "''", RegexOptions.None);
                                DataRow[] resultMeal = dtSport.Select("Detail LIKE '%" + tmp + "%'");
                                foreach (DataRow rows in resultMeal)
                                {
                                    row["ReturnDrink"] = rows["ConcatenatedField"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceReturnDrink"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeReturnDrink"] = rows["SSRCode"];
                                }

                                if (args.NewValues["ReturnMeal"] == "" || args.NewValues["ReturnMeal"] == null)
                                {
                                    DataRow[] resultMeals = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                                    foreach (DataRow rowz in resultMeals)
                                    {
                                        row["ReturnMeal"] = rowz["Detail"];
                                        Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                        row["PriceReturnMeal"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeReturnMeal"] = rowz["SSRCode"];
                                    }
                                }
                            }
                        }


                    }

                    if (args.NewValues["ConReturnMeal"] != "" && args.NewValues["ConReturnMeal"] != null)
                    {
                        //row["SSRCodeConReturnMeal"] = args.NewValues["ConReturnMeal"];
                        DataTable dtDrink = Session["dtDrinkReturn2"] as DataTable;
                        DataTable dtMeal1 = Session["dtMealReturn2"] as DataTable;
                        if (args.NewValues["ConReturnMeal"].ToString().Length == 4)
                        {
                            if (row["ConReturnMeal"] != null && row["ConReturnMeal"].ToString() != "")
                            {
                                DataRow[] resultMeal1 = dtMeal1.Select("SSRCode = '" + args.NewValues["ConReturnMeal"] + "'");
                                foreach (DataRow rows in resultMeal1)
                                {
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceConReturnMeal"]) && row["ConReturnMeal"].ToString().Trim() == rows[1].ToString().Trim())
                                    {
                                        continue;
                                        //return;
                                    }
                                    else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["PriceConReturnMeal"]) && row["ConReturnMeal"].ToString().Trim() != rows[1].ToString().Trim())
                                    {
                                        gvPassenger2.JSProperties["cp_result"] = "Cannot replace Meal 2 with cheaper fee, please try again";
                                        //count = 1;
                                        //return;
                                    }
                                    else
                                    {
                                        row["ConReturnMeal"] = rows["Detail"];
                                        row["PriceConReturnMeal"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeConReturnMeal"] = rows["SSRCode"];

                                        if (args.NewValues["ConReturnDrink"] == "" || args.NewValues["ConReturnDrink"] == null)
                                        {
                                            string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                            for (int c = 0; c < drinkcode.Length; c++)
                                            {
                                                DataRow[] resultDrink = dtDrink.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                                if (resultDrink.Length > 0)
                                                {
                                                    foreach (DataRow rowz in resultDrink)
                                                    {
                                                        row["ConReturnDrink"] = rows["ConcatenatedField"];
                                                        Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                                        row["PriceConReturnDrink"] = Convert.ToDecimal(Detail);
                                                        row["SSRCodeConReturnDrink"] = rows["SSRCode"];
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                DataRow[] resultMeal1 = dtMeal1.Select("SSRCode = '" + args.NewValues["ConReturnMeal"] + "'");
                                foreach (DataRow rows in resultMeal1)
                                {
                                    row["ConReturnMeal"] = rows["Detail"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceConReturnMeal"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeConReturnMeal"] = rows["SSRCode"];
                                }
                                if (args.NewValues["ConReturnDrink"] == "" || args.NewValues["ConReturnDrink"] == null)
                                {
                                    string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                    for (int c = 0; c < drinkcode.Length; c++)
                                    {
                                        DataRow[] resultDrink = dtDrink.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                        if (resultDrink.Length > 0)
                                        {
                                            foreach (DataRow rowz in resultDrink)
                                            {
                                                row["ConReturnDrink"] = rowz["ConcatenatedField"];
                                                Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                                row["PriceConReturnDrink"] = Convert.ToDecimal(Detail);
                                                row["SSRCodeConReturnDrink"] = rowz["SSRCode"];
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (row["ConReturnMeal"] != null && row["ConReturnMeal"].ToString() != "")
                            {
                                string tmp = Regex.Replace(args.NewValues["ConReturnMeal"].ToString().Substring(0, 10), "[^ 0-9a-zA-Z]+", "''", RegexOptions.None);
                                DataRow[] resultMeal = dtMeal1.Select("Detail LIKE '%" + tmp + "%'");
                                foreach (DataRow rows in resultMeal)
                                {
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);

                                     if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceConReturnMeal"]) && row["ConReturnMeal"].ToString().Trim() == rows[1].ToString().Trim())
                                    {
                                        continue;
                                        //return;
                                    }
                                    else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["PriceConReturnMeal"]) && row["ConReturnMeal"].ToString().Trim() != rows[1].ToString().Trim())
                                    {
                                        gvPassenger2.JSProperties["cp_result"] = "Cannot replace Meal 2 with cheaper fee, please try again";
                                        //count = 1;
                                        //return;
                                    }
                                    else
                                    {
                                        row["ConReturnMeal"] = rows["Detail"];
                                        row["PriceConReturnMeal"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeConReturnMeal"] = rows["SSRCode"];

                                        if (args.NewValues["ConReturnDrink"] == "" || args.NewValues["ConReturnDrink"] == null)
                                        {
                                            string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                            for (int c = 0; c < drinkcode.Length; c++)
                                            {
                                                DataRow[] resultDrink = dtDrink.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                                if (resultDrink.Length > 0)
                                                {
                                                    foreach (DataRow rowz in resultDrink)
                                                    {
                                                        row["ConReturnDrink"] = rows["ConcatenatedField"];
                                                        Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                                        row["PriceConReturnDrink"] = Convert.ToDecimal(Detail);
                                                        row["SSRCodeConReturnDrink"] = rows["SSRCode"];
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string tmp = Regex.Replace(args.NewValues["ConReturnMeal"].ToString().Substring(0, 10), "[^ 0-9a-zA-Z]+", "''", RegexOptions.None);
                                DataRow[] resultMeal = dtMeal1.Select("Detail LIKE '%" + tmp + "%'");
                                foreach (DataRow rows in resultMeal)
                                {
                                    row["ConReturnMeal"] = rows["Detail"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceConReturnMeal"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeConReturnMeal"] = rows["SSRCode"];
                                }

                                if (args.NewValues["ConReturnDrink"] == "" || args.NewValues["ConReturnDrink"] == null)
                                {
                                    string[] drinkcode = new string[] { "BW", "CD", "CF", "JC" };
                                    for (int c = 0; c < drinkcode.Length; c++)
                                    {
                                        DataRow[] resultDrink = dtDrink.Select("SSRCode LIKE '" + drinkcode[c] + "%'");
                                        if (resultDrink.Length > 0)
                                        {
                                            foreach (DataRow rowz in resultDrink)
                                            {
                                                row["ConReturnDrink"] = rowz["ConcatenatedField"];
                                                Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                                row["PriceConReturnDrink"] = Convert.ToDecimal(Detail);
                                                row["SSRCodeConReturnDrink"] = rowz["SSRCode"];
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                    }

                    if (args.NewValues["ConReturnDrink"] != "" && args.NewValues["ConReturnDrink"] != null)
                    {
                        //row["SSRCodeConReturnDrink"] = args.NewValues["ConReturnDrink"];
                        DataTable dtMeal = Session["dtMealReturn2"] as DataTable;
                        DataTable dtSport = Session["dtDrinkReturn2"] as DataTable;
                        if (args.NewValues["ConReturnDrink"].ToString().Length == 4)
                        {
                            if (row["ConReturnDrink"] != null && row["ConReturnDrink"].ToString() != "")
                            {
                                DataRow[] resultSport = dtSport.Select("SSRCode = '" + args.NewValues["ConReturnDrink"] + "'");
                                foreach (DataRow rows in resultSport)
                                {
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["PriceConReturnDrink"]))
                                    {
                                        gvPassenger2.JSProperties["cp_result"] = "Cannot replace Drink 2 with cheaper fee, please try again";
                                        //count = 1;
                                        //return;
                                    }
                                    else
                                    {
                                        row["ConReturnDrink"] = rows["ConcatenatedField"];
                                        row["PriceConReturnDrink"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeConReturnDrink"] = rows["SSRCode"];

                                        if (args.NewValues["ConReturnMeal"] == "" || args.NewValues["ConReturnMeal"] == null)
                                        {
                                            DataRow[] resultMeals = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                                            foreach (DataRow rowz in resultMeals)
                                            {
                                                row["ConReturnMeal"] = rowz["Detail"];
                                                Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                                row["PriceConReturnMeal"] = Convert.ToDecimal(Detail);
                                                row["SSRCodeConReturnMeal"] = rowz["SSRCode"];
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                DataRow[] resultSport = dtSport.Select("SSRCode = '" + args.NewValues["ConReturnDrink"] + "'");
                                foreach (DataRow rows in resultSport)
                                {
                                    row["ConReturnDrink"] = rows["ConcatenatedField"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceConReturnDrink"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeConReturnDrink"] = rows["SSRCode"];
                                }

                                if (args.NewValues["ConReturnMeal"] == "" || args.NewValues["ConReturnMeal"] == null)
                                {
                                    DataRow[] resultMeals = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                                    foreach (DataRow rowz in resultMeals)
                                    {
                                        row["ConReturnMeal"] = rowz["Detail"];
                                        Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                        row["PriceConReturnMeal"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeConReturnMeal"] = rowz["SSRCode"];
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (row["ConReturnDrink"] != null && row["ConReturnDrink"].ToString() != "")
                            {
                                string tmp = Regex.Replace(args.NewValues["ConReturnDrink"].ToString().Substring(0, 10), "[^ 0-9a-zA-Z]+", "''", RegexOptions.None);
                                DataRow[] resultMeal = dtSport.Select("Detail LIKE '%" + tmp + "%'");
                                foreach (DataRow rows in resultMeal)
                                {
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["PriceConReturnDrink"]))
                                    {
                                        gvPassenger2.JSProperties["cp_result"] = "Cannot replace Drink 2 with cheaper fee, please try again";
                                        //count = 1;
                                        //return;
                                    }
                                    else
                                    {
                                        row["ConReturnDrink"] = rows["ConcatenatedField"];
                                        row["PriceConReturnDrink"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeConReturnDrink"] = rows["SSRCode"];

                                        if (args.NewValues["ConReturnMeal"] == "" || args.NewValues["ConReturnMeal"] == null)
                                        {
                                            DataRow[] resultMeals = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                                            foreach (DataRow rowz in resultMeals)
                                            {
                                                row["ConReturnMeal"] = rowz["Detail"];
                                                Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                                row["PriceConReturnMeal"] = Convert.ToDecimal(Detail);
                                                row["SSRCodeConReturnMeal"] = rowz["SSRCode"];
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string tmp = Regex.Replace(args.NewValues["ConReturnDrink"].ToString().Substring(0, 10), "[^ 0-9a-zA-Z]+", "''", RegexOptions.None);
                                DataRow[] resultMeal = dtSport.Select("Detail LIKE '%" + tmp + "%'");
                                foreach (DataRow rows in resultMeal)
                                {
                                    row["ConReturnDrink"] = rows["ConcatenatedField"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceConReturnDrink"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeConReturnDrink"] = rows["SSRCode"];
                                }

                                if (args.NewValues["ConReturnMeal"] == "" || args.NewValues["ConReturnMeal"] == null)
                                {
                                    DataRow[] resultMeals = dtMeal.Select("SSRCode = '" + dtMeal.Rows[0]["SSRCode"].ToString() + "'");
                                    foreach (DataRow rowz in resultMeals)
                                    {
                                        row["ConReturnMeal"] = rowz["Detail"];
                                        Detail = rowz[2].ToString().Substring(0, rowz[2].ToString().Length - 4);
                                        row["PriceConReturnMeal"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeConReturnMeal"] = rowz["SSRCode"];
                                    }
                                }
                            }
                        }


                    }


                    if (args.NewValues["ReturnComfort"] != "" && args.NewValues["ReturnComfort"] != null)
                    {
                        //row["SSRCodeReturnComfort"] = args.NewValues["ReturnComfort"];
                        DataTable dtComfort = Session["dtComfortReturn"] as DataTable;
                        if (args.NewValues["ReturnComfort"].ToString().Length == 4)
                        {
                            if (row["ReturnComfort"] != null && row["ReturnComfort"].ToString() != "")
                            {
                                DataRow[] resultComfort = dtComfort.Select("SSRCode = '" + args.NewValues["ReturnComfort"] + "'");
                                foreach (DataRow rows in resultComfort)
                                {
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceReturnComfort"]) && row["SSRCodeReturnComfort"].ToString().Trim() == rows[0].ToString().Trim())
                                    {
                                        continue;
                                        //return;
                                    }
                                    else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["PriceReturnComfort"]))
                                    {
                                        gvPassenger2.JSProperties["cp_result"] = "Cannot replace Comfort Kit with cheaper fee, please try again";
                                        //count = 1;
                                        //return;
                                    }
                                    else
                                    {
                                        row["ReturnComfort"] = rows["Detail"];
                                        row["PriceReturnComfort"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeReturnComfort"] = rows["SSRCode"];
                                    }
                                }
                            }
                            else
                            {
                                DataRow[] resultComfort = dtComfort.Select("SSRCode = '" + args.NewValues["ReturnComfort"] + "'");
                                foreach (DataRow rows in resultComfort)
                                {
                                    row["ReturnComfort"] = rows["Detail"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceReturnComfort"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeReturnComfort"] = rows["SSRCode"];
                                }
                            }
                        }
                        else
                        {
                            if (row["ReturnComfort"] != null && row["ReturnComfort"].ToString() != "")
                            {
                                DataRow[] resultComfort = dtComfort.Select("Detail = '" + args.NewValues["ReturnComfort"].ToString().Substring(0, 11) + "'");
                                foreach (DataRow rows in resultComfort)
                                {
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row["PriceReturnComfort"]) && row["SSRCodeReturnComfort"].ToString().Trim() == rows[0].ToString().Trim())
                                    {
                                        continue;
                                        //return;
                                    }
                                    else if (Convert.ToDecimal(Detail) < Convert.ToDecimal(row["PriceReturnComfort"]))
                                    {
                                        gvPassenger2.JSProperties["cp_result"] = "Cannot replace Comfort Kit with cheaper fee, please try again";
                                        //count = 1;
                                        //return;
                                    }
                                    else
                                    {
                                        row["ReturnComfort"] = rows["Detail"];
                                        row["PriceReturnComfort"] = Convert.ToDecimal(Detail);
                                        row["SSRCodeReturnComfort"] = rows["SSRCode"];
                                    }
                                }
                            }
                            else
                            {
                                DataRow[] resultComfort = dtComfort.Select("Detail = '" + args.NewValues["ReturnComfort"].ToString().Substring(0, 11) + "'");
                                foreach (DataRow rows in resultComfort)
                                {
                                    row["ReturnComfort"] = rows["Detail"];
                                    Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                    row["PriceReturnComfort"] = Convert.ToDecimal(Detail);
                                    row["SSRCodeReturnComfort"] = rows["SSRCode"];
                                }
                            }
                        }
                    }

                    if (args.NewValues["ReturnDuty"] != "" && args.NewValues["ReturnDuty"] != null)
                    {
                        //row["SSRCodeReturnDuty"] = args.NewValues["ReturnDuty"];
                        DataTable dtDuty = Session["dtDutyReturn"] as DataTable;
                        DataRow[] resultDuty = dtDuty.Select("SSRCode = '" + args.NewValues["Duty"] + "'");
                        foreach (DataRow rows in resultDuty)
                        {
                            row["ReturnDuty"] = rows["Detail"];
                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                            row["PriceReturnDuty"] = Convert.ToDecimal(Detail);
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
                ScriptManager.RegisterStartupScript(this, GetType(), "Script", "gvPassenger2_EndCallback();", true);
                gvPassenger2.DataSource = Session["dtGridPass2"];
                gvPassenger2.DataBind();
            }
        }
        #endregion

        protected void ClearSessionData()
        {
            //HttpContext.Current.Session.Remove("TempFlight");
            //HttpContext.Current.Session.Remove("dataClass");
            //HttpContext.Current.Session.Remove("ErrorPayment");
            //HttpContext.Current.Session.Remove("dataClassTrans");

            bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(TransID);
            objBooking.FillDataTableTransMain(bookHDRInfo);
            int transStatus = 0;

            //added by diana 20130923, update flight & passenger details

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
                    //commented by diana 20131114

                    //temp remarked navitaire update
                    //objBooking.UpdateBookingJourneyDetails(TransID, AgentSet.AgentName, AgentSet.AgentID, true);
                    //objBooking.UpdatePaymentDetails(TransID, AgentSet.AgentName, AgentSet.AgentID, true);

                    //replace the new get booking from Navitaire
                    List<ListTransaction> AllTransaction = new List<ListTransaction>();
                    AllTransaction = objBooking.GetTransactionDetails(TransID);
                    if (AllTransaction != null && AllTransaction.Count > 0)
                    {
                        ListTransaction lstTrans = AllTransaction[0];

                        List<BookingTransactionDetail> VoidPNRs = new List<BookingTransactionDetail>();
                        List<BookingTransactionDetail> ExpiredPNRs = new List<BookingTransactionDetail>();
                        if (objBooking.UpdateAllBookingJourneyDetails(lstTrans, lstTrans.AgentUserName.ToString(), lstTrans.AgentID.ToString(), ref VoidPNRs, ref ExpiredPNRs, true) == false)
                        {
                            log.Warning(this, "Fail to Get Latest Update for Transaction - manageaddon.aspx.cs : " + lstTrans.TransID);
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
                    //temp remarked navitaire update
                    objBooking.UpdatePassengerDetails(TransID, MyUserSet.AgentName, MyUserSet.AgentID, true);

                    if (bookHDRInfo.TransStatus == 2)
                    {
                        //added by diana 20140605, if passenger complete, then status should be 3 or confirmed
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
            //end added by diana 20130923


            if (bookHDRInfo.TransStatus == 4 || bookHDRInfo.TransStatus == 6 || bookHDRInfo.TransStatus == 7)
            {
                //lstbookDTLInfo = objBooking.GetAllBK_TRANSDTLFilter(TransID, 0);
                lstRejectedbookDTLInfo = objBooking.GetAllBK_TRANSDTLFilter(TransID, 1);
                listDetailCombinePNR = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 1, "LEN(RecordLocator) >= 6 AND ");

                //FillDataTableTransDetail(lstbookDTLInfo); 
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

                //FillDataTableTransDetail(lstbookDTLInfo); 
                objBooking.FillDataTableTransDetail(listDetailCombinePNR);
                // lstbookFlightInfo = objBooking.GetAllBK_TRANSDTLFlight(TransID);
            }

            //lstbookPaymentInfo = objBooking.GetAllBK_TRANSTENDERFilter(TransID, " BK_TRANSTENDER.TransVoid=0 AND ");

            //Session["dtGridDetail"] = lstbookDTLInfo;
            //Session["dtRejectedGridDetail"] = lstRejectedbookDTLInfo;

            //Boolean returnFlight = false;
            //returnFlight = objBooking.IsReturn(TransID, 0);
            //lstFareBreakdown = objBooking.GetAllBK_TRANSDTLFareBreakdown(TransID, 0);
            //dtFareBreakdown = objBooking.GetAllBK_TRANSDTLFareBreakdown3(TransID, 0, RecordLocator, returnFlight);
            //Session["dtFareBreakdown"] = dtFareBreakdown;
            //lstFareBreakdownReturn = objBooking.GetAllBK_TRANSDTLFlightGrpNoSellKey1(TransID, 0);

            //dtPass = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTable(TransID);
            //Session["dtPassenger"] = dtPass;

            //dtAddOn = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTableManage2(TransID);
            //Session["dtAddOn"] = dtAddOn;
        }

    }
}