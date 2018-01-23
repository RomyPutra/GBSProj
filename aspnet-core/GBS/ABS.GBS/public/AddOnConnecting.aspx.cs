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


namespace GroupBooking.Web
{
    public partial class AddOnConnecting : System.Web.UI.Page
    {

        #region declaration
        UserSet MyUserSet;
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogControl log = new LogControl();

        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        Bk_transssr BK_TRANSSSRInfo = new Bk_transssr();
        List<Bk_transssr> listbk_transssrinfo = new List<Bk_transssr>();
        List<Bk_transssr> listbk_transssrinfo1 = new List<Bk_transssr>();
        List<Bk_transssr> listbk_transssrinfo11 = new List<Bk_transssr>();
        List<Bk_transssr> listbk_transssrinfo2 = new List<Bk_transssr>();
        List<Bk_transssr> listbk_transssrinfo21 = new List<Bk_transssr>();

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
        DataTable dtList11 = new DataTable();
        DataTable dtList21 = new DataTable();
        private static int qtyMeal, qtyBaggage, qtySport, qtyComfort, qtyDuty = 0;
        private static int qtyMeal2, qtyBaggage2, qtySport2, qtyComfort2, qtyDuty2 = 0;
        private static int num = 0;
        private static int departID = 0;
        private static string ReturnID = "";
        #endregion

        #region "Load"
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)HttpContext.Current.Session["TempFlight"];

            //return;
            try
            {
                ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking("");
                string signature = Session["signature"].ToString();
                Session["PaxStatus"] = "";
                if (Session["AgentSet"] != null)
                { MyUserSet = (UserSet)Session["AgentSet"]; }
                if (!IsPostBack)
                {
                    GetSellSSR(signature);
                }
                GetPassengerList(Session["TransID"].ToString());
                HttpCookie cookie2 = Request.Cookies["cookieSearchcondition"];
                if (cookie2 != null)
                {
                    if (cookie2.Values["ifOneWay"] != "TRUE")
                    {
                        GetPassengerList2(Session["TransID"].ToString());
                    }
                }
                //SetMaxValue();
                //if (cookie2 != null)
                //{
                //    if (cookie2.Values["ifOneWay"] != "TRUE")
                //    {
                //        SetMaxValue2();
                //    }
                //}
            }
            catch (Exception ex)
            {
                //log.Error(this,ex);
                log.Error(this, ex);
            }

        }
        #endregion

        #region "Initializer"
        protected void SetMaxValue()
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

        protected void SetMaxValue2()
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
        protected void InitializeForm(String Currency, ArrayList BaggageCode, ArrayList BaggageFee, ArrayList SportCode, ArrayList SportFee, ArrayList ComfortCode, ArrayList ConfortFee, ArrayList ComfortImg, ArrayList MealCode, ArrayList MealFee, ArrayList MealImg, ArrayList DutyCode, ArrayList DutyFee, ArrayList DutyImg, String Depart, String Return)
        {

            string temp = "";
            temp = Depart + " | " + Return;
            TabControl.TabPages[0].Text = temp;
            DataTable dtBaggage = new DataTable();
            dtBaggage.Columns.Add("SSRCode");
            dtBaggage.Columns.Add("Detail");
            dtBaggage.Columns.Add("Price");
            dtBaggage.Columns.Add("ConcatenatedField", typeof(string), "Detail + ' : ' +Price");

            //Enumerable.Range(1, 10).ToList().ForEach(i => dtBaggage.Rows.Add(i, string.Concat("Detail", i), string.Concat("Price", i)));
            DataRow row = dtBaggage.NewRow();

            String Detail = "";
            foreach (string item in BaggageCode)
            {
                Detail += "'" + item + "',";
            }
            Detail = Detail.Substring(0, Detail.Length - 1);

            objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
            DataTable dt = objBooking.GetDetailSSRbyCode(Detail);
            if (dt != null && dt.Rows.Count > 0)
            {
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
            cmbBaggage.Text = "No Baggage";
            Session["dtBaggage"] = dtBaggage;

            DataTable dtMeal = new DataTable();
            dtMeal.Columns.Add("SSRCode");
            dtMeal.Columns.Add("Detail");
            dtMeal.Columns.Add("Price");
            dtMeal.Columns.Add("Images");

            DataRow rowMeal = dtMeal.NewRow();

            Detail = "";
            foreach (string item in MealCode)
            {
                Detail += "'" + item + "',";
            }
            Detail = Detail.Substring(0, Detail.Length - 1);

            objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
            dt = objBooking.GetDetailSSRbyCode(Detail);
            if (dt != null && dt.Rows.Count > 0)
            {
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

            DataView dvMeal = dtMeal.DefaultView;
            glMeals.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
            glMeals.DataBind();
            Session["dtMeal"] = dtMeal;

            DataTable dtSport = new DataTable();
            dtSport.Columns.Add("SSRCode");
            dtSport.Columns.Add("Detail");
            dtSport.Columns.Add("Price");
            dtSport.Columns.Add("ConcatenatedField", typeof(string), "Detail + ' : ' +Price");

            //Enumerable.Range(1, 10).ToList().ForEach(i => dtSport.Rows.Add(i, string.Concat("Detail", i), string.Concat("Price", i)));
            DataRow row1 = dtSport.NewRow();

            Detail = "";
            foreach (string item in SportCode)
            {
                Detail += "'" + item + "',";
            }
            Detail = Detail.Substring(0, Detail.Length - 1);

            objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
            dt = objBooking.GetDetailSSRbyCode(Detail);
            if (dt != null && dt.Rows.Count > 0)
            {
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
            cmbSport.DataSource = dvSport.ToTable(true, "SSRCode", "Detail", "Price", "ConcatenatedField");

            cmbSport.TextField = "ConcatenatedField";
            cmbSport.ValueField = "SSRCode";
            cmbSport.DataBind();
            cmbSport.Text = "No Sport Equipment";
            Session["dtSport"] = dtSport;

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
            dt = objBooking.GetDetailSSRbyCode(Detail);
            if (dt != null && dt.Rows.Count > 0)
            {
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
            glDuty.DataSource = dvDuty.ToTable(true, "SSRCode", "Detail", "Price", "Images");
            glDuty.DataBind();
            Session["dtDuty"] = dtDuty;

            DataTable dtComfort = new DataTable();
            dtComfort.Columns.Add("SSRCode");
            dtComfort.Columns.Add("Detail");
            dtComfort.Columns.Add("Price");
            dtComfort.Columns.Add("Images");

            DataRow row2 = dtComfort.NewRow();

            Detail = "'" + ComfortCode[0] + "'";


            objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
            dt = objBooking.GetDetailSSRbyCode(Detail);
            if (dt != null && dt.Rows.Count > 0)
            {
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
            Session["dtComfort"] = dtComfort;
        }

        protected void InitializeForm2(String Currency, ArrayList BaggageCode, ArrayList BaggageFee, ArrayList SportCode, ArrayList SportFee, ArrayList ComfortCode, ArrayList ConfortFee, ArrayList ComfortImg, ArrayList MealCode, ArrayList MealFee, ArrayList MealImg, ArrayList DutyCode, ArrayList DutyFee, ArrayList DutyImg, String Depart, String Return)
        {

            string temp = "";
            temp = Depart + " | " + Return;
            TabControl.TabPages[2].Text = temp;
            DataTable dtBaggage = new DataTable();
            dtBaggage.Columns.Add("SSRCode");
            dtBaggage.Columns.Add("Detail");
            dtBaggage.Columns.Add("Price");
            dtBaggage.Columns.Add("ConcatenatedField", typeof(string), "Detail + ' : ' +Price");

            //Enumerable.Range(1, 10).ToList().ForEach(i => dtBaggage.Rows.Add(i, string.Concat("Detail", i), string.Concat("Price", i)));
            DataRow row = dtBaggage.NewRow();

            String Detail = "";
            foreach (string item in BaggageCode)
            {
                Detail += "'" + item + "',";
            }
            Detail = Detail.Substring(0, Detail.Length - 1);

            objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
            DataTable dt = objBooking.GetDetailSSRbyCode(Detail);
            if (dt != null && dt.Rows.Count > 0)
            {
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
            cmbBaggage2.Text = "No Baggage";
            Session["dtBaggage2"] = dtBaggage;

            DataTable dtMeal = new DataTable();
            dtMeal.Columns.Add("SSRCode");
            dtMeal.Columns.Add("Detail");
            dtMeal.Columns.Add("Price");
            dtMeal.Columns.Add("Images");

            DataRow rowMeal = dtMeal.NewRow();

            Detail = "";
            foreach (string item in MealCode)
            {
                Detail += "'" + item + "',";
            }
            Detail = Detail.Substring(0, Detail.Length - 1);

            objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
            dt = objBooking.GetDetailSSRbyCode(Detail);
            if (dt != null && dt.Rows.Count > 0)
            {
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

            DataView dvMeal = dtMeal.DefaultView;
            glMeals2.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
            glMeals2.DataBind();
            Session["dtMeal2"] = dtMeal;

            DataTable dtSport = new DataTable();
            dtSport.Columns.Add("SSRCode");
            dtSport.Columns.Add("Detail");
            dtSport.Columns.Add("Price");
            dtSport.Columns.Add("ConcatenatedField", typeof(string), "Detail + ' : ' +Price");

            //Enumerable.Range(1, 10).ToList().ForEach(i => dtSport.Rows.Add(i, string.Concat("Detail", i), string.Concat("Price", i)));
            DataRow row1 = dtSport.NewRow();

            Detail = "";
            foreach (string item in SportCode)
            {
                Detail += "'" + item + "',";
            }
            Detail = Detail.Substring(0, Detail.Length - 1);

            objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
            dt = objBooking.GetDetailSSRbyCode(Detail);
            if (dt != null && dt.Rows.Count > 0)
            {
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
            cmbSport2.Text = "No Sport Equipment";
            Session["dtSport2"] = dtSport;

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
            dt = objBooking.GetDetailSSRbyCode(Detail);
            if (dt != null && dt.Rows.Count > 0)
            {
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
            Session["dtDuty2"] = dtDuty;

            DataTable dtComfort = new DataTable();
            dtComfort.Columns.Add("SSRCode");
            dtComfort.Columns.Add("Detail");
            dtComfort.Columns.Add("Price");
            dtComfort.Columns.Add("Images");

            DataRow row2 = dtComfort.NewRow();

            Detail = "'" + ComfortCode[0] + "'";


            objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
            dt = objBooking.GetDetailSSRbyCode(Detail);
            if (dt != null && dt.Rows.Count > 0)
            {
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
            Session["dtComfort2"] = dtComfort;


        }

        protected void InitializeForm1(String Currency, ArrayList BaggageCode, ArrayList BaggageFee, ArrayList SportCode, ArrayList SportFee, ArrayList ComfortCode, ArrayList ConfortFee, ArrayList ComfortImg, ArrayList MealCode, ArrayList MealFee, ArrayList MealImg, ArrayList DutyCode, ArrayList DutyFee, ArrayList DutyImg, String Depart, String Return)
        {

            string temp = "";
            temp = Depart + " | " + Return;
            TabControl.TabPages[1].Text = temp;
            DataTable dtBaggage = new DataTable();
            dtBaggage.Columns.Add("SSRCode");
            dtBaggage.Columns.Add("Detail");
            dtBaggage.Columns.Add("Price");
            dtBaggage.Columns.Add("ConcatenatedField", typeof(string), "Detail + ' : ' +Price");

            //Enumerable.Range(1, 10).ToList().ForEach(i => dtBaggage.Rows.Add(i, string.Concat("Detail", i), string.Concat("Price", i)));
            DataRow row = dtBaggage.NewRow();

            String Detail = "";
            foreach (string item in BaggageCode)
            {
                Detail += "'" + item + "',";
            }
            Detail = Detail.Substring(0, Detail.Length - 1);

            objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
            DataTable dt = objBooking.GetDetailSSRbyCode(Detail);
            if (dt != null && dt.Rows.Count > 0)
            {
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
            cmbBaggage1.DataSource = dv.ToTable(true, "SSRCode", "Detail", "Price", "ConcatenatedField");
            cmbBaggage1.TextField = "ConcatenatedField";
            cmbBaggage1.ValueField = "SSRCode";
            cmbBaggage1.DataBind();
            cmbBaggage1.Text = "No Baggage";
            Session["dtBaggage1"] = dtBaggage;

            DataTable dtMeal = new DataTable();
            dtMeal.Columns.Add("SSRCode");
            dtMeal.Columns.Add("Detail");
            dtMeal.Columns.Add("Price");
            dtMeal.Columns.Add("Images");

            DataRow rowMeal = dtMeal.NewRow();

            Detail = "";
            foreach (string item in MealCode)
            {
                Detail += "'" + item + "',";
            }
            Detail = Detail.Substring(0, Detail.Length - 1);

            objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
            dt = objBooking.GetDetailSSRbyCode(Detail);
            if (dt != null && dt.Rows.Count > 0)
            {
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

            DataView dvMeal = dtMeal.DefaultView;
            glMeals1.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
            glMeals1.DataBind();
            Session["dtMeal1"] = dtMeal;

            DataTable dtSport = new DataTable();
            dtSport.Columns.Add("SSRCode");
            dtSport.Columns.Add("Detail");
            dtSport.Columns.Add("Price");
            dtSport.Columns.Add("ConcatenatedField", typeof(string), "Detail + ' : ' +Price");

            //Enumerable.Range(1, 10).ToList().ForEach(i => dtSport.Rows.Add(i, string.Concat("Detail", i), string.Concat("Price", i)));
            DataRow row1 = dtSport.NewRow();

            Detail = "";
            foreach (string item in SportCode)
            {
                Detail += "'" + item + "',";
            }
            Detail = Detail.Substring(0, Detail.Length - 1);

            objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
            dt = objBooking.GetDetailSSRbyCode(Detail);
            if (dt != null && dt.Rows.Count > 0)
            {
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
            cmbSport1.DataSource = dvSport.ToTable(true, "SSRCode", "Detail", "Price", "ConcatenatedField");

            cmbSport1.TextField = "ConcatenatedField";
            cmbSport1.ValueField = "SSRCode";
            cmbSport1.DataBind();
            cmbSport1.Text = "No Sport Equipment";
            Session["dtSport1"] = dtSport;

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
            dt = objBooking.GetDetailSSRbyCode(Detail);
            if (dt != null && dt.Rows.Count > 0)
            {
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
            glDuty1.DataSource = dvDuty.ToTable(true, "SSRCode", "Detail", "Price", "Images");
            glDuty1.DataBind();
            Session["dtDuty1"] = dtDuty;

            DataTable dtComfort = new DataTable();
            dtComfort.Columns.Add("SSRCode");
            dtComfort.Columns.Add("Detail");
            dtComfort.Columns.Add("Price");
            dtComfort.Columns.Add("Images");

            DataRow row2 = dtComfort.NewRow();

            Detail = "'" + ComfortCode[0] + "'";


            objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
            dt = objBooking.GetDetailSSRbyCode(Detail);
            if (dt != null && dt.Rows.Count > 0)
            {
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
            glComfort1.DataSource = dvComfort.ToTable(true, "SSRCode", "Detail", "Price", "Images");
            glComfort1.DataBind();
            Session["dtComfort1"] = dtComfort;
        }

        protected void InitializeForm21(String Currency, ArrayList BaggageCode, ArrayList BaggageFee, ArrayList SportCode, ArrayList SportFee, ArrayList ComfortCode, ArrayList ConfortFee, ArrayList ComfortImg, ArrayList MealCode, ArrayList MealFee, ArrayList MealImg, ArrayList DutyCode, ArrayList DutyFee, ArrayList DutyImg, String Depart, String Return)
        {

            string temp = "";
            temp = Depart + " | " + Return;
            TabControl.TabPages[3].Text = temp;
            DataTable dtBaggage = new DataTable();
            dtBaggage.Columns.Add("SSRCode");
            dtBaggage.Columns.Add("Detail");
            dtBaggage.Columns.Add("Price");
            dtBaggage.Columns.Add("ConcatenatedField", typeof(string), "Detail + ' : ' +Price");

            //Enumerable.Range(1, 10).ToList().ForEach(i => dtBaggage.Rows.Add(i, string.Concat("Detail", i), string.Concat("Price", i)));
            DataRow row = dtBaggage.NewRow();

            String Detail = "";
            foreach (string item in BaggageCode)
            {
                Detail += "'" + item + "',";
            }
            Detail = Detail.Substring(0, Detail.Length - 1);

            objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
            DataTable dt = objBooking.GetDetailSSRbyCode(Detail);
            if (dt != null && dt.Rows.Count > 0)
            {
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
            cmbBaggage21.DataSource = dv.ToTable(true, "SSRCode", "Detail", "Price", "ConcatenatedField");
            cmbBaggage21.TextField = "ConcatenatedField";
            cmbBaggage21.ValueField = "SSRCode";
            cmbBaggage21.DataBind();
            cmbBaggage21.Text = "No Baggage";
            Session["dtBaggage21"] = dtBaggage;

            DataTable dtMeal = new DataTable();
            dtMeal.Columns.Add("SSRCode");
            dtMeal.Columns.Add("Detail");
            dtMeal.Columns.Add("Price");
            dtMeal.Columns.Add("Images");

            DataRow rowMeal = dtMeal.NewRow();

            Detail = "";
            foreach (string item in MealCode)
            {
                Detail += "'" + item + "',";
            }
            Detail = Detail.Substring(0, Detail.Length - 1);

            objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
            dt = objBooking.GetDetailSSRbyCode(Detail);
            if (dt != null && dt.Rows.Count > 0)
            {
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

            DataView dvMeal = dtMeal.DefaultView;
            glMeals21.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
            glMeals21.DataBind();
            Session["dtMeal21"] = dtMeal;

            DataTable dtSport = new DataTable();
            dtSport.Columns.Add("SSRCode");
            dtSport.Columns.Add("Detail");
            dtSport.Columns.Add("Price");
            dtSport.Columns.Add("ConcatenatedField", typeof(string), "Detail + ' : ' +Price");

            //Enumerable.Range(1, 10).ToList().ForEach(i => dtSport.Rows.Add(i, string.Concat("Detail", i), string.Concat("Price", i)));
            DataRow row1 = dtSport.NewRow();

            Detail = "";
            foreach (string item in SportCode)
            {
                Detail += "'" + item + "',";
            }
            Detail = Detail.Substring(0, Detail.Length - 1);

            objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
            dt = objBooking.GetDetailSSRbyCode(Detail);
            if (dt != null && dt.Rows.Count > 0)
            {
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
            cmbSport21.DataSource = dvSport.ToTable(true, "SSRCode", "Detail", "Price", "ConcatenatedField");

            cmbSport21.TextField = "ConcatenatedField";
            cmbSport21.ValueField = "SSRCode";
            cmbSport21.DataBind();
            cmbSport21.Text = "No Sport Equipment";
            Session["dtSport21"] = dtSport;

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
            dt = objBooking.GetDetailSSRbyCode(Detail);
            if (dt != null && dt.Rows.Count > 0)
            {
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
            glDuty21.DataSource = dvDuty.ToTable(true, "SSRCode", "Detail", "Price", "Images");
            glDuty21.DataBind();
            Session["dtDuty21"] = dtDuty;

            DataTable dtComfort = new DataTable();
            dtComfort.Columns.Add("SSRCode");
            dtComfort.Columns.Add("Detail");
            dtComfort.Columns.Add("Price");
            dtComfort.Columns.Add("Images");

            DataRow row2 = dtComfort.NewRow();

            Detail = "'" + ComfortCode[0] + "'";


            objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
            dt = objBooking.GetDetailSSRbyCode(Detail);
            if (dt != null && dt.Rows.Count > 0)
            {
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
            glComfort21.DataSource = dvComfort.ToTable(true, "SSRCode", "Detail", "Price", "Images");
            glComfort21.DataBind();
            Session["dtComfort21"] = dtComfort;


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
                log.Error(this, ex);
            }
        }
        #endregion

        #region "Event"
        protected void glMeals_Init(object sender, EventArgs e)
        {
            if (Session["dtMeal"] != null)
            {
                DataTable dtMeal = (DataTable)Session["dtMeal"];
                DataView dvMeal = dtMeal.DefaultView;
                glMeals.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                glMeals.DataBind();
            }
        }

        protected void glDuty_Init(object sender, EventArgs e)
        {
            if (Session["dtDuty"] != null)
            {
                DataTable dtDuty = (DataTable)Session["dtDuty"];
                DataView dvDuty = dtDuty.DefaultView;
                glDuty.DataSource = dvDuty.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                glDuty.DataBind();
            }
        }

        protected void glComfort_Init(object sender, EventArgs e)
        {
            if (Session["dtComfort"] != null)
            {
                DataTable dtComfort = (DataTable)Session["dtComfort"];
                DataView dvComfort = dtComfort.DefaultView;
                glComfort.DataSource = dvComfort.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                glComfort.DataBind();
            }
        }

        protected void glMeals1_Init(object sender, EventArgs e)
        {
            if (Session["dtMeal1"] != null)
            {
                DataTable dtMeal = (DataTable)Session["dtMeal1"];
                DataView dvMeal = dtMeal.DefaultView;
                glMeals1.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                glMeals1.DataBind();
            }
        }

        protected void glDuty1_Init(object sender, EventArgs e)
        {
            if (Session["dtDuty1"] != null)
            {
                DataTable dtDuty = (DataTable)Session["dtDuty1"];
                DataView dvDuty = dtDuty.DefaultView;
                glDuty1.DataSource = dvDuty.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                glDuty1.DataBind();
            }
        }

        protected void glComfort1_Init(object sender, EventArgs e)
        {
            if (Session["dtComfort1"] != null)
            {
                DataTable dtComfort = (DataTable)Session["dtComfort1"];
                DataView dvComfort = dtComfort.DefaultView;
                glComfort1.DataSource = dvComfort.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                glComfort1.DataBind();
            }
        }

        protected void glMeals2_Init(object sender, EventArgs e)
        {
            if (Session["dtMeal2"] != null)
            {
                DataTable dtMeal = (DataTable)Session["dtMeal2"];
                DataView dvMeal = dtMeal.DefaultView;
                glMeals2.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                glMeals2.DataBind();
            }
        }

        protected void glDuty2_Init(object sender, EventArgs e)
        {
            if (Session["dtDuty2"] != null)
            {
                DataTable dtDuty = (DataTable)Session["dtDuty2"];
                DataView dvDuty = dtDuty.DefaultView;
                glDuty2.DataSource = dvDuty.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                glDuty2.DataBind();
            }
        }

        protected void glComfort2_Init(object sender, EventArgs e)
        {
            if (Session["dtComfort2"] != null)
            {
                DataTable dtComfort = (DataTable)Session["dtComfort2"];
                DataView dvComfort = dtComfort.DefaultView;
                glComfort2.DataSource = dvComfort.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                glComfort2.DataBind();
            }
        }

        protected void glMeals21_Init(object sender, EventArgs e)
        {
            if (Session["dtMeal21"] != null)
            {
                DataTable dtMeal = (DataTable)Session["dtMeal21"];
                DataView dvMeal = dtMeal.DefaultView;
                glMeals21.DataSource = dvMeal.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                glMeals21.DataBind();
            }
        }

        protected void glDuty21_Init(object sender, EventArgs e)
        {
            if (Session["dtDuty21"] != null)
            {
                DataTable dtDuty = (DataTable)Session["dtDuty21"];
                DataView dvDuty = dtDuty.DefaultView;
                glDuty21.DataSource = dvDuty.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                glDuty21.DataBind();
            }
        }

        protected void glComfort21_Init(object sender, EventArgs e)
        {
            if (Session["dtComfort21"] != null)
            {
                DataTable dtComfort = (DataTable)Session["dtComfort21"];
                DataView dvComfort = dtComfort.DefaultView;
                glComfort21.DataSource = dvComfort.ToTable(true, "SSRCode", "Detail", "Price", "Images");
                glComfort21.DataBind();
            }
        }

        protected void gvPassenger_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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
                    DataTable dtBaggage = (DataTable)Session["dtBaggage"];
                    for (int i = qtyBaggage; i <= (Convert.ToInt32(args[1]) + qtyBaggage) - 1; i++)
                    {
                        dtPass.Rows[i]["Baggage"] = args[2];
                        dtPass.Rows[i]["SSRCodeBaggage"] = args[3];
                        DataRow[] result = dtBaggage.Select("SSRCode = '" + args[3] + "'");
                        foreach (DataRow row in result)
                        {
                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                        }
                        dtPass.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                    }
                    Session["dtGridPass"] = dtPass;
                    Session["qtyBaggage"] = (Convert.ToInt32(args[1]) + qtyBaggage);
                    gvPassenger.DataSource = dtPass;
                    gvPassenger.DataBind();

                }
                else if (args[0] == "Meal")
                {
                    DataTable dtMeal = (DataTable)Session["dtMeal"];
                    for (int i = qtyMeal; i <= (Convert.ToInt32(args[1]) + qtyMeal) - 1; i++)
                    {
                        dtPass.Rows[i]["Meal"] = args[2];
                        dtPass.Rows[i]["SSRCodeMeal"] = args[3];
                        DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                        foreach (DataRow row in result)
                        {
                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                        }
                        dtPass.Rows[i]["PriceMeal"] = Convert.ToDecimal(Detail);
                    }
                    Session["dtGridPass"] = dtPass;
                    Session["qtyMeal"] = (Convert.ToInt32(args[1]) + qtyMeal);
                    gvPassenger.DataSource = dtPass;
                    gvPassenger.DataBind();
                }
                else if (args[0] == "Sport")
                {
                    DataTable dtSport = (DataTable)Session["dtSport"];
                    for (int i = qtySport; i <= (Convert.ToInt32(args[1]) + qtySport) - 1; i++)
                    {
                        dtPass.Rows[i]["Sport"] = args[2];
                        dtPass.Rows[i]["SSRCodeSport"] = args[3];
                        DataRow[] result = dtSport.Select("SSRCode = '" + args[3] + "'");
                        foreach (DataRow row in result)
                        {
                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                        }
                        dtPass.Rows[i]["PriceSport"] = Convert.ToDecimal(Detail);
                    }
                    Session["dtGridPass"] = dtPass;
                    Session["qtySport"] = (Convert.ToInt32(args[1]) + qtySport);
                    gvPassenger.DataSource = dtPass;
                    gvPassenger.DataBind();
                }
                else if (args[0] == "Duty")
                {
                    DataTable dtDuty = (DataTable)Session["dtDuty"];
                    for (int i = qtyDuty; i <= (Convert.ToInt32(args[1]) + qtySport) - 1; i++)
                    {
                        dtPass.Rows[i]["Duty"] = args[2];
                        dtPass.Rows[i]["SSRCodeDuty"] = args[3];
                        DataRow[] result = dtDuty.Select("SSRCode = '" + args[3] + "'");
                        foreach (DataRow row in result)
                        {
                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                        }
                        dtPass.Rows[i]["PriceDuty"] = Convert.ToDecimal(Detail);
                    }
                    Session["dtGridPass"] = dtPass;
                    Session["qtyDuty"] = (Convert.ToInt32(args[1]) + qtySport);
                    gvPassenger.DataSource = dtPass;
                    gvPassenger.DataBind();
                }
                else if (args[0] == "Comfort")
                {
                    DataTable dtComfort = (DataTable)Session["dtComfort"];
                    for (int i = qtyComfort; i <= (Convert.ToInt32(args[1]) + qtyComfort) - 1; i++)
                    {
                        dtPass.Rows[i]["Comfort"] = args[2];
                        dtPass.Rows[i]["SSRCodeComfort"] = args[3];
                        DataRow[] result = dtComfort.Select("SSRCode = '" + args[3] + "'");
                        foreach (DataRow row in result)
                        {
                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                        }
                        dtPass.Rows[i]["PriceComfort"] = Convert.ToDecimal(Detail);
                    }
                    Session["dtGridPass"] = dtPass;
                    Session["qtyComfort"] = (Convert.ToInt32(args[1]) + qtyComfort);
                    gvPassenger.DataSource = dtPass;
                    gvPassenger.DataBind();
                }

            }

            SetMaxValue();
        }

        protected void gvPassenger1_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            String Detail = "";
            if (Session["qtyMeal1"] == null)
            {
                qtyMeal = 0;
            }
            else
            {
                qtyMeal = Convert.ToInt32(Session["qtyMeal1"]);
            }
            if (Session["qtyBaggage1"] == null)
            {
                qtyBaggage = 0;
            }
            else
            {
                qtyBaggage = Convert.ToInt32(Session["qtyBaggage1"]);
            }
            if (Session["qtySport1"] == null)
            {
                qtySport = 0;
            }
            else
            {
                qtySport = Convert.ToInt32(Session["qtySport1"]);
            }
            if (Session["qtyComfort1"] == null)
            {
                qtyComfort = 0;
            }
            else
            {
                qtyComfort = Convert.ToInt32(Session["qtyComfort1"]);
            }
            if (Session["qtyDuty1"] == null)
            {
                qtyDuty = 0;
            }
            else
            {
                qtyDuty = Convert.ToInt32(Session["qtyDuty1"]);
            }
            DataTable dtPass = new DataTable();
            dtPass = (DataTable)Session["dtGridPass1"];
            if (string.IsNullOrEmpty(e.Parameters))
            {
                GetPassengerList(Session["TransID"].ToString());
            }
            else
            {
                var args = e.Parameters.Split('|');
                if (args[0] == "Baggage")
                {
                    DataTable dtBaggage = (DataTable)Session["dtBaggage1"];
                    for (int i = qtyBaggage; i <= (Convert.ToInt32(args[1]) + qtyBaggage) - 1; i++)
                    {
                        dtPass.Rows[i]["Baggage"] = args[2];
                        dtPass.Rows[i]["SSRCodeBaggage"] = args[3];
                        DataRow[] result = dtBaggage.Select("SSRCode = '" + args[3] + "'");
                        foreach (DataRow row in result)
                        {
                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                        }
                        dtPass.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                    }
                    Session["dtGridPass1"] = dtPass;
                    Session["qtyBaggage1"] = (Convert.ToInt32(args[1]) + qtyBaggage);
                    gvPassenger1.DataSource = dtPass;
                    gvPassenger1.DataBind();

                }
                else if (args[0] == "Meal")
                {
                    DataTable dtMeal = (DataTable)Session["dtMeal1"];
                    for (int i = qtyMeal; i <= (Convert.ToInt32(args[1]) + qtyMeal) - 1; i++)
                    {
                        dtPass.Rows[i]["Meal"] = args[2];
                        dtPass.Rows[i]["SSRCodeMeal"] = args[3];
                        DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                        foreach (DataRow row in result)
                        {
                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                        }
                        dtPass.Rows[i]["PriceMeal"] = Convert.ToDecimal(Detail);
                    }
                    Session["dtGridPass1"] = dtPass;
                    Session["qtyMeal1"] = (Convert.ToInt32(args[1]) + qtyMeal);
                    gvPassenger1.DataSource = dtPass;
                    gvPassenger1.DataBind();
                }
                else if (args[0] == "Sport")
                {
                    DataTable dtSport = (DataTable)Session["dtSport1"];
                    for (int i = qtySport; i <= (Convert.ToInt32(args[1]) + qtySport) - 1; i++)
                    {
                        dtPass.Rows[i]["Sport"] = args[2];
                        dtPass.Rows[i]["SSRCodeSport"] = args[3];
                        DataRow[] result = dtSport.Select("SSRCode = '" + args[3] + "'");
                        foreach (DataRow row in result)
                        {
                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                        }
                        dtPass.Rows[i]["PriceSport"] = Convert.ToDecimal(Detail);
                    }
                    Session["dtGridPass1"] = dtPass;
                    Session["qtySport1"] = (Convert.ToInt32(args[1]) + qtySport);
                    gvPassenger1.DataSource = dtPass;
                    gvPassenger1.DataBind();
                }
                else if (args[0] == "Duty")
                {
                    DataTable dtDuty = (DataTable)Session["dtDuty1"];
                    for (int i = qtyDuty; i <= (Convert.ToInt32(args[1]) + qtySport) - 1; i++)
                    {
                        dtPass.Rows[i]["Duty"] = args[2];
                        dtPass.Rows[i]["SSRCodeDuty"] = args[3];
                        DataRow[] result = dtDuty.Select("SSRCode = '" + args[3] + "'");
                        foreach (DataRow row in result)
                        {
                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                        }
                        dtPass.Rows[i]["PriceDuty"] = Convert.ToDecimal(Detail);
                    }
                    Session["dtGridPass1"] = dtPass;
                    Session["qtyDuty1"] = (Convert.ToInt32(args[1]) + qtySport);
                    gvPassenger1.DataSource = dtPass;
                    gvPassenger1.DataBind();
                }
                else if (args[0] == "Comfort")
                {
                    DataTable dtComfort = (DataTable)Session["dtComfort1"];
                    for (int i = qtyComfort; i <= (Convert.ToInt32(args[1]) + qtyComfort) - 1; i++)
                    {
                        dtPass.Rows[i]["Comfort"] = args[2];
                        dtPass.Rows[i]["SSRCodeComfort"] = args[3];
                        DataRow[] result = dtComfort.Select("SSRCode = '" + args[3] + "'");
                        foreach (DataRow row in result)
                        {
                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                        }
                        dtPass.Rows[i]["PriceComfort"] = Convert.ToDecimal(Detail);
                    }
                    Session["dtGridPass1"] = dtPass;
                    Session["qtyComfort1"] = (Convert.ToInt32(args[1]) + qtyComfort);
                    gvPassenger1.DataSource = dtPass;
                    gvPassenger1.DataBind();
                }

            }

            //SetMaxValue();
        }

        protected void gvPassenger2_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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
                    DataTable dtBaggage = (DataTable)Session["dtBaggage2"];
                    for (int i = qtyBaggage2; i <= (Convert.ToInt32(args[1]) + qtyBaggage2) - 1; i++)
                    {
                        dtPass2.Rows[i]["Baggage"] = args[2];
                        dtPass2.Rows[i]["SSRCodeBaggage"] = args[3];
                        DataRow[] result = dtBaggage.Select("SSRCode = '" + args[3] + "'");
                        foreach (DataRow row in result)
                        {
                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                        }
                        dtPass2.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                    }
                    Session["dtGridPass2"] = dtPass2;
                    Session["qtyBaggage2"] = (Convert.ToInt32(args[1]) + qtyBaggage2);
                    gvPassenger2.DataSource = dtPass2;
                    gvPassenger2.DataBind();
                }
                else if (args[0] == "Meal")
                {
                    DataTable dtMeal = (DataTable)Session["dtMeal2"];
                    for (int i = qtyMeal2; i <= (Convert.ToInt32(args[1]) + qtyMeal2) - 1; i++)
                    {
                        dtPass2.Rows[i]["Meal"] = args[2];
                        dtPass2.Rows[i]["SSRCodeMeal"] = args[3];
                        DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                        foreach (DataRow row in result)
                        {
                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                        }
                        dtPass2.Rows[i]["PriceMeal"] = Convert.ToDecimal(Detail);
                    }
                    Session["dtGridPass2"] = dtPass2;
                    Session["qtyMeal2"] = (Convert.ToInt32(args[1]) + qtyMeal2);
                    gvPassenger2.DataSource = dtPass2;
                    gvPassenger2.DataBind();
                }
                else if (args[0] == "Sport")
                {
                    DataTable dtSport = (DataTable)Session["dtSport2"];
                    for (int i = qtySport2; i <= Convert.ToInt32(args[1] + qtySport2) - 1; i++)
                    {
                        dtPass2.Rows[i]["Sport"] = args[2];
                        dtPass2.Rows[i]["SSRCodeSport"] = args[3];
                        DataRow[] result = dtSport.Select("SSRCode = '" + args[3] + "'");
                        foreach (DataRow row in result)
                        {
                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                        }
                        dtPass2.Rows[i]["PriceSport"] = Convert.ToDecimal(Detail);
                    }
                    Session["dtGridPass2"] = dtPass2;
                    Session["qtySport2"] = Convert.ToInt32(args[1] + qtySport2);
                    gvPassenger2.DataSource = dtPass2;
                    gvPassenger2.DataBind();
                }
                else if (args[0] == "Duty")
                {
                    DataTable dtDuty = (DataTable)Session["dtDuty2"];
                    for (int i = qtyDuty2; i <= Convert.ToInt32(args[1] + qtyDuty2) - 1; i++)
                    {
                        dtPass2.Rows[i]["Duty"] = args[2];
                        dtPass2.Rows[i]["SSRCodeDuty"] = args[3];
                        DataRow[] result = dtDuty.Select("SSRCode = '" + args[3] + "'");
                        foreach (DataRow row in result)
                        {
                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                        }
                        dtPass2.Rows[i]["PriceDuty"] = Convert.ToDecimal(Detail);
                    }
                    Session["dtGridPass2"] = dtPass2;
                    Session["qtyDuty2"] = Convert.ToInt32(args[1] + qtyDuty2);
                    gvPassenger2.DataSource = dtPass2;
                    gvPassenger2.DataBind();
                }
                else if (args[0] == "Comfort")
                {
                    DataTable dtComfort = (DataTable)Session["dtComfort2"];
                    for (int i = qtyComfort2; i <= (Convert.ToInt32(args[1]) + qtyComfort2) - 1; i++)
                    {
                        dtPass2.Rows[i]["Comfort"] = args[2];
                        dtPass2.Rows[i]["SSRCodeComfort"] = args[3];
                        DataRow[] result = dtComfort.Select("SSRCode = '" + args[3] + "'");
                        foreach (DataRow row in result)
                        {
                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                        }
                        dtPass2.Rows[i]["PriceComfort"] = Convert.ToDecimal(Detail);

                    }
                    Session["dtGridPass2"] = dtPass2;
                    Session["qtyComfort2"] = (Convert.ToInt32(args[1]) + qtyComfort2);
                    gvPassenger2.DataSource = dtPass2;
                    gvPassenger2.DataBind();
                }
            }
            SetMaxValue2();
        }

        protected void gvPassenger21_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            String Detail = "";
            if (Session["qtyMeal21"] == null)
            {
                qtyMeal2 = 0;
            }
            else
            {
                qtyMeal2 = Convert.ToInt32(Session["qtyMeal21"]);
            }
            if (Session["qtyBaggage21"] == null)
            {
                qtyBaggage2 = 0;
            }
            else
            {
                qtyBaggage2 = Convert.ToInt32(Session["qtyBaggage2"]);
            }
            if (Session["qtySport21"] == null)
            {
                qtySport2 = 0;
            }
            else
            {
                qtySport2 = Convert.ToInt32(Session["qtySport21"]);
            }
            if (Session["qtyComfort21"] == null)
            {
                qtyComfort2 = 0;
            }
            else
            {
                qtyComfort2 = Convert.ToInt32(Session["qtyComfort21"]);
            }
            if (Session["qtyDuty21"] == null)
            {
                qtyDuty2 = 0;
            }
            else
            {
                qtyDuty2 = Convert.ToInt32(Session["qtyDuty21"]);
            }
            DataTable dtPass2 = new DataTable();
            dtPass2 = (DataTable)Session["dtGridPass21"];
            if (string.IsNullOrEmpty(e.Parameters))
            {
                GetPassengerList2(Session["TransID"].ToString());
            }
            else
            {
                var args = e.Parameters.Split('|');
                if (args[0] == "Baggage")
                {
                    DataTable dtBaggage = (DataTable)Session["dtBaggage21"];
                    for (int i = qtyBaggage2; i <= (Convert.ToInt32(args[1]) + qtyBaggage2) - 1; i++)
                    {
                        dtPass2.Rows[i]["Baggage"] = args[2];
                        dtPass2.Rows[i]["SSRCodeBaggage"] = args[3];
                        DataRow[] result = dtBaggage.Select("SSRCode = '" + args[3] + "'");
                        foreach (DataRow row in result)
                        {
                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                        }
                        dtPass2.Rows[i]["PriceBaggage"] = Convert.ToDecimal(Detail);
                    }
                    Session["dtGridPass21"] = dtPass2;
                    Session["qtyBaggage21"] = (Convert.ToInt32(args[1]) + qtyBaggage2);
                    gvPassenger21.DataSource = dtPass2;
                    gvPassenger21.DataBind();
                }
                else if (args[0] == "Meal")
                {
                    DataTable dtMeal = (DataTable)Session["dtMeal21"];
                    for (int i = qtyMeal2; i <= (Convert.ToInt32(args[1]) + qtyMeal2) - 1; i++)
                    {
                        dtPass2.Rows[i]["Meal"] = args[2];
                        dtPass2.Rows[i]["SSRCodeMeal"] = args[3];
                        DataRow[] result = dtMeal.Select("SSRCode = '" + args[3] + "'");
                        foreach (DataRow row in result)
                        {
                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                        }
                        dtPass2.Rows[i]["PriceMeal"] = Convert.ToDecimal(Detail);
                    }
                    Session["dtGridPass21"] = dtPass2;
                    Session["qtyMeal21"] = (Convert.ToInt32(args[1]) + qtyMeal2);
                    gvPassenger21.DataSource = dtPass2;
                    gvPassenger21.DataBind();
                }
                else if (args[0] == "Sport")
                {
                    DataTable dtSport = (DataTable)Session["dtSport21"];
                    for (int i = qtySport2; i <= Convert.ToInt32(args[1] + qtySport2) - 1; i++)
                    {
                        dtPass2.Rows[i]["Sport"] = args[2];
                        dtPass2.Rows[i]["SSRCodeSport"] = args[3];
                        DataRow[] result = dtSport.Select("SSRCode = '" + args[3] + "'");
                        foreach (DataRow row in result)
                        {
                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                        }
                        dtPass2.Rows[i]["PriceSport"] = Convert.ToDecimal(Detail);
                    }
                    Session["dtGridPass21"] = dtPass2;
                    Session["qtySport21"] = Convert.ToInt32(args[1] + qtySport2);
                    gvPassenger21.DataSource = dtPass2;
                    gvPassenger21.DataBind();
                }
                else if (args[0] == "Duty")
                {
                    DataTable dtDuty = (DataTable)Session["dtDuty21"];
                    for (int i = qtyDuty2; i <= Convert.ToInt32(args[1] + qtyDuty2) - 1; i++)
                    {
                        dtPass2.Rows[i]["Duty"] = args[2];
                        dtPass2.Rows[i]["SSRCodeDuty"] = args[3];
                        DataRow[] result = dtDuty.Select("SSRCode = '" + args[3] + "'");
                        foreach (DataRow row in result)
                        {
                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                        }
                        dtPass2.Rows[i]["PriceDuty"] = Convert.ToDecimal(Detail);
                    }
                    Session["dtGridPass21"] = dtPass2;
                    Session["qtyDuty21"] = Convert.ToInt32(args[1] + qtyDuty2);
                    gvPassenger21.DataSource = dtPass2;
                    gvPassenger21.DataBind();
                }
                else if (args[0] == "Comfort")
                {
                    DataTable dtComfort = (DataTable)Session["dtComfort21"];
                    for (int i = qtyComfort2; i <= (Convert.ToInt32(args[1]) + qtyComfort2) - 1; i++)
                    {
                        dtPass2.Rows[i]["Comfort"] = args[2];
                        dtPass2.Rows[i]["SSRCodeComfort"] = args[3];
                        DataRow[] result = dtComfort.Select("SSRCode = '" + args[3] + "'");
                        foreach (DataRow row in result)
                        {
                            Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                        }
                        dtPass2.Rows[i]["PriceComfort"] = Convert.ToDecimal(Detail);

                    }
                    Session["dtGridPass21"] = dtPass2;
                    Session["qtyComfort21"] = (Convert.ToInt32(args[1]) + qtyComfort2);
                    gvPassenger21.DataSource = dtPass2;
                    gvPassenger21.DataBind();
                }
            }
            //SetMaxValue2();
        }
        #endregion

        #region "Function and Procedure"
        protected void GetSellSSR(string signature)
        {
            HttpCookie cookie = Request.Cookies["cookieTemFlight"];
            ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking(signature);
            BookingControl bookingControl = new BookingControl();

            GetSSRAvailabilityForBookingResponse response = apiBooking.GetSSRAvailabilityForBooking(bookingControl.GetBookingFromState(signature), signature);
            string xml = GetXMLString(response);
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

                String Depart1 = "";
                String Arrival1 = "";

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


                String Currency = "";
                SSRAvailabilityForBookingResponse ssrAvailabilityResponseForBooking = response.SSRAvailabilityForBookingResponse;
                Session["GetssrAvailabilityResponseForBooking"] = response;
                //xml = GetXMLString(ssrAvailabilityResponseForBooking);
                //this entire section is simply to test the computation of SSR pricing for Meal fee in particular as segment based or leg based depending on the results
                if (cookie != null)
                {
                    if (cookie.Values["ReturnID"] != "")
                    {
                        if (ssrAvailabilityResponseForBooking.SSRSegmentList.Length == 4)
                        {
                            //segment1 depart
                            Depart = ssrAvailabilityResponseForBooking.SSRSegmentList[0].LegKey.DepartureStation;
                            Arrival = ssrAvailabilityResponseForBooking.SSRSegmentList[0].LegKey.ArrivalStation;
                            for (int j = 0; j < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList.Length; j++) //for SSR index
                            {
                                if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList.Length > 0) //skip the cases where the SSR is totally free, no build in pricing
                                {

                                    if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PBAA") ||
                                        ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PBAB") ||
                                        ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PBAC") ||
                                        ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PBAD") ||
                                        ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PBAF"))
                                    {
                                        BaggageCode.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode);
                                        for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                        {
                                            BaggageAmt = 0;

                                            //to compute the baggage fee pricing
                                            for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                            {
                                                if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                    BaggageAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                                //if (ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                                //    BaggageAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                                //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                                BaggageFee.Add(BaggageAmt);
                                            }

                                        }
                                    }
                                    else if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PSEA") ||
                                            ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PSEB") ||
                                            ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PSEC") ||
                                            ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PSED") ||
                                            ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PSEF"))
                                    {
                                        SportCode.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode);
                                        for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                        {
                                            SportAmt = 0;

                                            //to compute the meal fee pricing
                                            for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                            {
                                                if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                    SportAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                                //if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                                //    SportAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                                //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                                SportFee.Add(SportAmt);
                                            }

                                        }
                                    }
                                    else if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PCMK"))
                                    {
                                        ComfortCode.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode);
                                        ComfortImage.Add("http://www.airasia.com/images/common/sky-ssr/img_" + ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode + ".png");
                                        for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                        {
                                            ComfortAmt = 0;

                                            //to compute the meal fee pricing
                                            for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                            {
                                                if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                    ComfortAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                                //if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                                //    ComfortAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                                //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                                ComfortFee.Add(ComfortAmt);
                                            }

                                        }
                                    }
                                    else if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRLegList.Length > 0)
                                    {
                                        MealCode.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode);
                                        MealImage.Add("http://www.airasia.com/images/common/sky-ssr/img_" + ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode + ".png");
                                        for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                        {
                                            ComfortAmt = 0;

                                            //to compute the meal fee pricing
                                            for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                            {
                                                if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                    MealAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                                //if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                                //    MealAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                                //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                                MealFee.Add(MealAmt);
                                            }

                                        }
                                    }
                                    else if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.IndexOf("DF") != -1)
                                    {
                                        DutyCode.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode);
                                        DutyImage.Add("http://www.airasia.com/images/common/sky-ssr/img_" + ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode + ".png");
                                        for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                        {
                                            ComfortAmt = 0;

                                            //to compute the meal fee pricing
                                            for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                            {
                                                if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                    DutyAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                                //if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                                //    DutyAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                                //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                                DutyFee.Add(DutyAmt);
                                            }

                                        }
                                    }
                                }
                            }

                            //segment2 depart
                            for (int j = 0; j < ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList.Length; j++) //for SSR index
                            {
                                if (ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].SSRLegList.Length > 0)
                                {
                                    MealCode1.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].SSRCode);
                                    MealImage1.Add("http://www.airasia.com/images/common/sky-ssr/img_" + ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].SSRCode + ".png");
                                    for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        MealAmt1 = 0;

                                        //to compute the meal fee pricing
                                        for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                MealAmt1 += ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                            //if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                            //    MealAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            MealFee1.Add(MealAmt1);
                                        }

                                    }
                                }
                            }
                        }

                        //segment1 return
                        Depart2 = ssrAvailabilityResponseForBooking.SSRSegmentList[2].LegKey.DepartureStation;
                        Arrival2 = ssrAvailabilityResponseForBooking.SSRSegmentList[2].LegKey.ArrivalStation;
                        for (int j = 0; j < ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList.Length; j++) //for SSR index
                        {
                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].PaxSSRPriceList.Length > 0) //skip the cases where the SSR is totally free, no build in pricing
                            {

                                if (ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].SSRCode.Equals("PBAA") ||
                                    ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].SSRCode.Equals("PBAB") ||
                                    ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].SSRCode.Equals("PBAC") ||
                                    ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].SSRCode.Equals("PBAD") ||
                                    ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].SSRCode.Equals("PBAF"))
                                {
                                    BaggageCode2.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].SSRCode);
                                    for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        BaggageAmt2 = 0;

                                        //to compute the baggage fee pricing
                                        for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                BaggageAmt2 += ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                            //if (ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                            //    BaggageAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            BaggageFee2.Add(BaggageAmt2);
                                        }

                                    }
                                }
                                else if (ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].SSRCode.Equals("PSEA") ||
                                        ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].SSRCode.Equals("PSEB") ||
                                        ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].SSRCode.Equals("PSEC") ||
                                        ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].SSRCode.Equals("PSED") ||
                                        ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].SSRCode.Equals("PSEF"))
                                {
                                    SportCode2.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].SSRCode);
                                    for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        SportAmt2 = 0;

                                        //to compute the meal fee pricing
                                        for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                SportAmt2 += ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                            //if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                            //    SportAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            SportFee2.Add(SportAmt2);
                                        }

                                    }
                                }
                                else if (ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].SSRCode.Equals("PCMK"))
                                {
                                    ComfortCode2.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].SSRCode);
                                    ComfortImage2.Add("http://www.airasia.com/images/common/sky-ssr/img_" + ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].SSRCode + ".png");
                                    for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        ComfortAmt2 = 0;

                                        //to compute the meal fee pricing
                                        for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                ComfortAmt2 += ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                            //if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                            //    ComfortAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            ComfortFee2.Add(ComfortAmt2);
                                        }

                                    }
                                }
                                else if (ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].SSRLegList.Length > 0)
                                {
                                    MealCode2.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].SSRCode);
                                    MealImage2.Add("http://www.airasia.com/images/common/sky-ssr/img_" + ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].SSRCode + ".png");
                                    for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        ComfortAmt2 = 0;

                                        //to compute the meal fee pricing
                                        for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                MealAmt2 += ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                            //if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                            //    MealAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            MealFee2.Add(MealAmt2);
                                        }

                                    }
                                }
                                else if (ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].SSRCode.IndexOf("DF") != -1)
                                {
                                    DutyCode2.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].SSRCode);
                                    DutyImage2.Add("http://www.airasia.com/images/common/sky-ssr/img_" + ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].SSRCode + ".png");
                                    for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        ComfortAmt2 = 0;

                                        //to compute the meal fee pricing
                                        for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[2].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                DutyAmt2 += ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                            //if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                            //    DutyAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            DutyFee2.Add(DutyAmt2);
                                        }

                                    }
                                }
                            }
                        }

                        //segment2 return
                        for (int j = 0; j < ssrAvailabilityResponseForBooking.SSRSegmentList[3].AvailablePaxSSRList.Length; j++) //for SSR index
                        {
                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[3].AvailablePaxSSRList[j].PaxSSRPriceList.Length > 0) //skip the cases where the SSR is totally free, no build in pricing
                            {

                                if (ssrAvailabilityResponseForBooking.SSRSegmentList[3].AvailablePaxSSRList[j].SSRLegList.Length > 0)
                                {
                                    MealCode21.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[3].AvailablePaxSSRList[j].SSRCode);
                                    MealImage21.Add("http://www.airasia.com/images/common/sky-ssr/img_" + ssrAvailabilityResponseForBooking.SSRSegmentList[3].AvailablePaxSSRList[j].SSRCode + ".png");
                                    for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[3].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        MealAmt21 = 0;

                                        //to compute the meal fee pricing
                                        for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[3].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[3].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                MealAmt21 += ssrAvailabilityResponseForBooking.SSRSegmentList[3].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                            //if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                            //    MealAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            MealFee21.Add(MealAmt21);
                                        }

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //segment1 depart
                        Depart = ssrAvailabilityResponseForBooking.SSRSegmentList[0].LegKey.DepartureStation;
                        Arrival = ssrAvailabilityResponseForBooking.SSRSegmentList[0].LegKey.ArrivalStation;
                        for (int j = 0; j < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList.Length; j++) //for SSR index
                        {
                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList.Length > 0) //skip the cases where the SSR is totally free, no build in pricing
                            {

                                if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PBAA") ||
                                    ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PBAB") ||
                                    ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PBAC") ||
                                    ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PBAD") ||
                                    ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PBAF"))
                                {
                                    BaggageCode.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode);
                                    for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        BaggageAmt = 0;

                                        //to compute the baggage fee pricing
                                        for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                BaggageAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                            //if (ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                            //    BaggageAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            BaggageFee.Add(BaggageAmt);
                                        }

                                    }
                                }
                                else if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PSEA") ||
                                        ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PSEB") ||
                                        ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PSEC") ||
                                        ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PSED") ||
                                        ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PSEF"))
                                {
                                    SportCode.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode);
                                    for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        SportAmt = 0;

                                        //to compute the meal fee pricing
                                        for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                SportAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                            //if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                            //    SportAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            SportFee.Add(SportAmt);
                                        }

                                    }
                                }
                                else if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PCMK"))
                                {
                                    ComfortCode.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode);
                                    ComfortImage.Add("http://www.airasia.com/images/common/sky-ssr/img_" + ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode + ".png");
                                    for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        ComfortAmt = 0;

                                        //to compute the meal fee pricing
                                        for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                ComfortAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                            //if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                            //    ComfortAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            ComfortFee.Add(ComfortAmt);
                                        }

                                    }
                                }
                                else if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRLegList.Length > 0)
                                {
                                    MealCode.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode);
                                    MealImage.Add("http://www.airasia.com/images/common/sky-ssr/img_" + ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode + ".png");
                                    for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        ComfortAmt = 0;

                                        //to compute the meal fee pricing
                                        for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                MealAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                            //if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                            //    MealAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            MealFee.Add(MealAmt);
                                        }

                                    }
                                }
                                else if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.IndexOf("DF") != -1)
                                {
                                    DutyCode.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode);
                                    DutyImage.Add("http://www.airasia.com/images/common/sky-ssr/img_" + ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode + ".png");
                                    for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        ComfortAmt = 0;

                                        //to compute the meal fee pricing
                                        for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                DutyAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                            //if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                            //    DutyAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            DutyFee.Add(DutyAmt);
                                        }

                                    }
                                }
                            }
                        }

                        //segment1 return
                        Depart2 = ssrAvailabilityResponseForBooking.SSRSegmentList[1].LegKey.DepartureStation;
                        Arrival2 = ssrAvailabilityResponseForBooking.SSRSegmentList[1].LegKey.ArrivalStation;
                        for (int j = 0; j < ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList.Length; j++) //for SSR index
                        {
                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList.Length > 0) //skip the cases where the SSR is totally free, no build in pricing
                            {

                                if (ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].SSRCode.Equals("PBAA") ||
                                    ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].SSRCode.Equals("PBAB") ||
                                    ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].SSRCode.Equals("PBAC") ||
                                    ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].SSRCode.Equals("PBAD") ||
                                    ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].SSRCode.Equals("PBAF"))
                                {
                                    BaggageCode2.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].SSRCode);
                                    for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        BaggageAmt2 = 0;

                                        //to compute the baggage fee pricing
                                        for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                BaggageAmt2 += ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                            //if (ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                            //    BaggageAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            BaggageFee2.Add(BaggageAmt2);
                                        }

                                    }
                                }
                                else if (ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].SSRCode.Equals("PSEA") ||
                                        ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].SSRCode.Equals("PSEB") ||
                                        ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].SSRCode.Equals("PSEC") ||
                                        ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].SSRCode.Equals("PSED") ||
                                        ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].SSRCode.Equals("PSEF"))
                                {
                                    SportCode2.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].SSRCode);
                                    for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        SportAmt2 = 0;

                                        //to compute the meal fee pricing
                                        for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                SportAmt2 += ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                            //if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                            //    SportAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            SportFee2.Add(SportAmt2);
                                        }

                                    }
                                }
                                else if (ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].SSRCode.Equals("PCMK"))
                                {
                                    ComfortCode2.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].SSRCode);
                                    ComfortImage2.Add("http://www.airasia.com/images/common/sky-ssr/img_" + ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].SSRCode + ".png");
                                    for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        ComfortAmt2 = 0;

                                        //to compute the meal fee pricing
                                        for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                ComfortAmt2 += ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                            //if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                            //    ComfortAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            ComfortFee2.Add(ComfortAmt2);
                                        }

                                    }
                                }
                                else if (ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].SSRLegList.Length > 0)
                                {
                                    MealCode2.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].SSRCode);
                                    MealImage2.Add("http://www.airasia.com/images/common/sky-ssr/img_" + ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].SSRCode + ".png");
                                    for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        ComfortAmt2 = 0;

                                        //to compute the meal fee pricing
                                        for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                MealAmt2 += ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                            //if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                            //    MealAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            MealFee2.Add(MealAmt2);
                                        }

                                    }
                                }
                                else if (ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].SSRCode.IndexOf("DF") != -1)
                                {
                                    DutyCode2.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].SSRCode);
                                    DutyImage2.Add("http://www.airasia.com/images/common/sky-ssr/img_" + ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].SSRCode + ".png");
                                    for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        ComfortAmt2 = 0;

                                        //to compute the meal fee pricing
                                        for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                DutyAmt2 += ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                            //if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                            //    DutyAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            DutyFee2.Add(DutyAmt2);
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (ssrAvailabilityResponseForBooking.SSRSegmentList.Length == 2)
                    {
                        //segment1 depart
                        Depart = ssrAvailabilityResponseForBooking.SSRSegmentList[0].LegKey.DepartureStation;
                        Arrival = ssrAvailabilityResponseForBooking.SSRSegmentList[0].LegKey.ArrivalStation;
                        for (int j = 0; j < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList.Length; j++) //for SSR index
                        {
                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList.Length > 0) //skip the cases where the SSR is totally free, no build in pricing
                            {

                                if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PBAA") ||
                                    ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PBAB") ||
                                    ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PBAC") ||
                                    ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PBAD") ||
                                    ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PBAF"))
                                {
                                    BaggageCode.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode);
                                    for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        BaggageAmt = 0;

                                        //to compute the baggage fee pricing
                                        for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                BaggageAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                            //if (ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                            //    BaggageAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            BaggageFee.Add(BaggageAmt);
                                        }

                                    }
                                }
                                else if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PSEA") ||
                                        ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PSEB") ||
                                        ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PSEC") ||
                                        ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PSED") ||
                                        ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PSEF"))
                                {
                                    SportCode.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode);
                                    for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        SportAmt = 0;

                                        //to compute the meal fee pricing
                                        for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                SportAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                            //if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                            //    SportAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            SportFee.Add(SportAmt);
                                        }

                                    }
                                }
                                else if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PCMK"))
                                {
                                    ComfortCode.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode);
                                    ComfortImage.Add("http://www.airasia.com/images/common/sky-ssr/img_" + ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode + ".png");
                                    for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        ComfortAmt = 0;

                                        //to compute the meal fee pricing
                                        for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                ComfortAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                            //if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                            //    ComfortAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            ComfortFee.Add(ComfortAmt);
                                        }

                                    }
                                }
                                else if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRLegList.Length > 0)
                                {
                                    MealCode.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode);
                                    MealImage.Add("http://www.airasia.com/images/common/sky-ssr/img_" + ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode + ".png");
                                    for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        ComfortAmt = 0;

                                        //to compute the meal fee pricing
                                        for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                MealAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                            //if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                            //    MealAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            MealFee.Add(MealAmt);
                                        }

                                    }
                                }
                                else if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.IndexOf("DF") != -1)
                                {
                                    DutyCode.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode);
                                    DutyImage.Add("http://www.airasia.com/images/common/sky-ssr/img_" + ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode + ".png");
                                    for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        ComfortAmt = 0;

                                        //to compute the meal fee pricing
                                        for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                DutyAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                            //if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                            //    DutyAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            DutyFee.Add(DutyAmt);
                                        }

                                    }
                                }
                            }
                        }

                        //segment2 depart
                        for (int j = 0; j < ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList.Length; j++) //for SSR index
                        {
                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList.Length > 0) //skip the cases where the SSR is totally free, no build in pricing
                            {

                                if (ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].SSRLegList.Length > 0)
                                {
                                    MealCode1.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].SSRCode);
                                    MealImage1.Add("http://www.airasia.com/images/common/sky-ssr/img_" + ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].SSRCode + ".png");
                                    for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        MealAmt1 = 0;

                                        //to compute the meal fee pricing
                                        for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                MealAmt1 += ssrAvailabilityResponseForBooking.SSRSegmentList[1].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                            //if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                            //    MealAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            MealFee1.Add(MealAmt1);
                                        }

                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        //segment1 depart
                        Depart = ssrAvailabilityResponseForBooking.SSRSegmentList[0].LegKey.DepartureStation;
                        Arrival = ssrAvailabilityResponseForBooking.SSRSegmentList[0].LegKey.ArrivalStation;
                        for (int j = 0; j < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList.Length; j++) //for SSR index
                        {
                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList.Length > 0) //skip the cases where the SSR is totally free, no build in pricing
                            {

                                if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PBAA") ||
                                    ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PBAB") ||
                                    ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PBAC") ||
                                    ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PBAD") ||
                                    ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PBAF"))
                                {
                                    BaggageCode.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode);
                                    for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        BaggageAmt = 0;

                                        //to compute the baggage fee pricing
                                        for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                BaggageAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                            //if (ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                            //    BaggageAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            BaggageFee.Add(BaggageAmt);
                                        }

                                    }
                                }
                                else if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PSEA") ||
                                        ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PSEB") ||
                                        ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PSEC") ||
                                        ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PSED") ||
                                        ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PSEF"))
                                {
                                    SportCode.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode);
                                    for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        SportAmt = 0;

                                        //to compute the meal fee pricing
                                        for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                SportAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                            //if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                            //    SportAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            SportFee.Add(SportAmt);
                                        }

                                    }
                                }
                                else if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.Equals("PCMK"))
                                {
                                    ComfortCode.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode);
                                    ComfortImage.Add("http://www.airasia.com/images/common/sky-ssr/img_" + ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode + ".png");
                                    for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        ComfortAmt = 0;

                                        //to compute the meal fee pricing
                                        for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                ComfortAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                            //if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                            //    ComfortAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            ComfortFee.Add(ComfortAmt);
                                        }

                                    }
                                }
                                else if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRLegList.Length > 0)
                                {
                                    MealCode.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode);
                                    MealImage.Add("http://www.airasia.com/images/common/sky-ssr/img_" + ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode + ".png");
                                    for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        ComfortAmt = 0;

                                        //to compute the meal fee pricing
                                        for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                MealAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                            //if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                            //    MealAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            MealFee.Add(MealAmt);
                                        }

                                    }
                                }
                                else if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode.IndexOf("DF") != -1)
                                {
                                    DutyCode.Add(ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode);
                                    DutyImage.Add("http://www.airasia.com/images/common/sky-ssr/img_" + ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].SSRCode + ".png");
                                    for (int l = 0; l < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList.Length; l++)
                                    {
                                        ComfortAmt = 0;

                                        //to compute the meal fee pricing
                                        for (int k = 0; k < ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges.Length; k++)
                                        {
                                            if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.ServiceCharge)
                                                DutyAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;

                                            //if (ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[0].ChargeType == ChargeType.Tax)
                                            //    DutyAmt += ssrAvailabilityResponseForBooking.SSRSegmentList[0].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            //HttpContext.Current.Session["mealFee"] = ssrAvailabilityResponseForBooking.SSRSegmentList[i].AvailablePaxSSRList[j].PaxSSRPriceList[0].PaxFee.ServiceCharges[k].Amount;
                                            DutyFee.Add(DutyAmt);
                                        }

                                    }
                                }
                            }
                        }
                    }
                }

                if (cookie != null)
                {
                    if (cookie.Values["ReturnID"] != "")
                    {
                        //if (ssrAvailabilityResponseForBooking.SSRSegmentList.Length == 4)
                        //{
                        //    InitializeForm(Currency, BaggageCode, BaggageFee, SportCode, SportFee, ComfortCode, ComfortFee, ComfortImage, MealCode, MealFee, MealImage, MealCode1, MealFee1, MealImage1, DutyCode, DutyFee, DutyImage, Depart, Arrival);
                        //    InitializeForm2(Currency, BaggageCode2, BaggageFee2, SportCode2, SportFee2, ComfortCode2, ComfortFee2, ComfortImage2, MealCode2, MealFee2, MealImage2, MealCode21, MealFee21, MealImage21, DutyCode2, DutyFee2, DutyImage2, Depart2, Arrival2);
                        //}
                        //else
                        //{
                        //    InitializeForm1(Currency, BaggageCode, BaggageFee, SportCode, SportFee, ComfortCode, ComfortFee, ComfortImage, MealCode, MealFee, MealImage, null, null, null, DutyCode, DutyFee, DutyImage, Depart, Arrival);
                        //    InitializeForm2(Currency, BaggageCode2, BaggageFee2, SportCode2, SportFee2, ComfortCode2, ComfortFee2, ComfortImage2, MealCode2, MealFee2, MealImage2, null, null, null, DutyCode2, DutyFee2, DutyImage2, Depart2, Arrival2);
                        //}

                    }
                    else
                    {
                        //if (ssrAvailabilityResponseForBooking.SSRSegmentList.Length == 2)
                        //{
                        //    InitializeForm(Currency, BaggageCode, BaggageFee, SportCode, SportFee, ComfortCode, ComfortFee, ComfortImage, MealCode, MealFee, MealImage, MealCode1, MealFee1, MealImage1, DutyCode, DutyFee, DutyImage, Depart, Arrival);
                        //}
                        //else
                        //{
                        //    InitializeForm(Currency, BaggageCode, BaggageFee, SportCode, SportFee, ComfortCode, ComfortFee, ComfortImage, MealCode, MealFee, MealImage, null, null, null, DutyCode, DutyFee, DutyImage, Depart, Arrival);
                        //}

                    }

                }
            }
        }



        protected void GetPassengerList(string TransID)
        {
            HttpCookie cookie = Request.Cookies["cookieTemFlight"];
            DataTable dtPass = new DataTable();
            if (Session["dtGridPass"] != null)
            {
                dtPass = (DataTable)Session["dtGridPass"];
            }
            else
            {
                dtPass = objBooking.GetAllBK_PASSENGERLISTSSRDataTable(TransID);
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
            Session["dtGridPass"] = dtPass;
        }

        protected void GetPassengerList1(string TransID)
        {
            HttpCookie cookie = Request.Cookies["cookieTemFlight"];
            DataTable dtPass = new DataTable();
            if (Session["dtGridPass1"] != null)
            {
                dtPass = (DataTable)Session["dtGridPass1"];
            }
            else
            {
                dtPass = objBooking.GetAllBK_PASSENGERLISTSSRDataTable(TransID);
            }
            if (dtPass != null && dtPass.Rows.Count > 0)
            {
                if (cookie != null)
                {
                    if (cookie.Values["ReturnID"] != "")
                    {
                        gvPassenger1.DataSource = dtPass;
                        gvPassenger1.DataBind();
                    }
                    else
                    {
                        gvPassenger1.DataSource = dtPass;
                        gvPassenger1.DataBind();


                    }
                }
            }
            Session["dtGridPass1"] = dtPass;
        }

        protected void GetPassengerList2(string TransID)
        {
            HttpCookie cookie = Request.Cookies["cookieTemFlight"];
            DataTable dtPass2 = new DataTable();
            if (Session["dtGridPass2"] != null)
            {
                dtPass2 = (DataTable)Session["dtGridPass2"];
            }
            else
            {
                dtPass2 = objBooking.GetAllBK_PASSENGERLISTSSRDataTable(TransID);
            }
            if (dtPass2 != null && dtPass2.Rows.Count > 0)
            {

                gvPassenger2.DataSource = dtPass2;
                gvPassenger2.DataBind();

                Session["dtGridPass2"] = dtPass2;
            }
        }

        protected void GetPassengerList21(string TransID)
        {
            HttpCookie cookie = Request.Cookies["cookieTemFlight"];
            DataTable dtPass2 = new DataTable();
            if (Session["dtGridPass21"] != null)
            {
                dtPass2 = (DataTable)Session["dtGridPass21"];
            }
            else
            {
                dtPass2 = objBooking.GetAllBK_PASSENGERLISTSSRDataTable(TransID);
            }
            if (dtPass2 != null && dtPass2.Rows.Count > 0)
            {

                gvPassenger21.DataSource = dtPass2;
                gvPassenger21.DataBind();

                Session["dtGridPass21"] = dtPass2;
            }
        }

        protected void ValidatePopup(object sender, CallbackEventArgs e)
        {

            dtList1.Columns.Add("SSRCode");
            dtList1.Columns.Add("PassengerID");

            dtList11.Columns.Add("SSRCode");
            dtList11.Columns.Add("PassengerID");

            dtList2.Columns.Add("SSRCode");
            dtList2.Columns.Add("PassengerID");

            dtList21.Columns.Add("SSRCode");
            dtList21.Columns.Add("PassengerID");


            MessageList msgList = new MessageList();
            ArrayList aMsgList = new ArrayList();
            try
            {
                DataTable dataClass = new DataTable();
                dataClass = (DataTable)HttpContext.Current.Session["dtGridPass1"];
                foreach (DataRow dr in dataClass.Rows)
                {
                    for (int i = 0; i <= 5; i++)
                    {
                        BK_TRANSSSRInfo = new Bk_transssr();
                        BK_TRANSSSRInfo.TransID = dr["TransID"].ToString();
                        BK_TRANSSSRInfo.RecordLocator = dr["PNR"].ToString();
                        BK_TRANSSSRInfo.CarrierCode = dr["CarrierCode"].ToString();
                        BK_TRANSSSRInfo.FlightNo = dr["FlightNo"].ToString();
                        BK_TRANSSSRInfo.Origin = dr["Origin"].ToString();
                        BK_TRANSSSRInfo.Destination = dr["Destination"].ToString();
                        BK_TRANSSSRInfo.PassengerID = dr["PassengerID"].ToString();
                        if (i == 0)
                        {
                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeBaggage"].ToString();
                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceBaggage"];
                        }
                        else if (i == 1)
                        {
                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeMeal"].ToString();
                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceMeal"];
                        }
                        else if (i == 2)
                        {
                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeSport"].ToString();
                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceSport"];
                        }
                        else if (i == 3)
                        {
                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeComfort"].ToString();
                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceComfort"];
                        }
                        else if (i == 4)
                        {
                            BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDuty"].ToString();
                            BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDuty"];
                        }
                        listbk_transssrinfo.Add(BK_TRANSSSRInfo);
                        listbk_transssrinfo1.Add(BK_TRANSSSRInfo);
                    }
                }
                HttpCookie cookie = Request.Cookies["cookieSearchcondition"];
                if (cookie != null)
                {

                    if (cookie.Values["ifOneWay"] != "TRUE")
                    {
                        dataClass = new DataTable();
                        dataClass = (DataTable)HttpContext.Current.Session["dtGridPass2"];
                        foreach (DataRow dr in dataClass.Rows)
                        {
                            for (int i = 0; i <= 5; i++)
                            {
                                BK_TRANSSSRInfo = new Bk_transssr();
                                BK_TRANSSSRInfo.TransID = dr["TransID"].ToString();
                                BK_TRANSSSRInfo.RecordLocator = dr["PNR"].ToString();
                                BK_TRANSSSRInfo.CarrierCode = dr["CarrierCode"].ToString();
                                BK_TRANSSSRInfo.FlightNo = dr["FlightNo"].ToString();
                                BK_TRANSSSRInfo.Origin = dr["Destination"].ToString();
                                BK_TRANSSSRInfo.Destination = dr["Origin"].ToString();
                                BK_TRANSSSRInfo.PassengerID = dr["PassengerID"].ToString();
                                BK_TRANSSSRInfo.SubSeqNo = i;
                                if (i == 0)
                                {
                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeBaggage"].ToString();
                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceBaggage"];

                                }
                                else if (i == 1)
                                {
                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeMeal"].ToString();
                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceMeal"];
                                }
                                else if (i == 2)
                                {
                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeSport"].ToString();
                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceSport"];
                                }
                                else if (i == 3)
                                {
                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeComfort"].ToString();
                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceComfort"];
                                }
                                else if (i == 4)
                                {
                                    BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDuty"].ToString();
                                    BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDuty"];
                                }
                                listbk_transssrinfo.Add(BK_TRANSSSRInfo);
                                listbk_transssrinfo2.Add(BK_TRANSSSRInfo);
                            }
                        }
                    }
                }
                if (Session["dtGridPass1"] != null)
                {
                    dataClass = new DataTable();
                    dataClass = (DataTable)HttpContext.Current.Session["dtGridPass1"];
                    foreach (DataRow dr in dataClass.Rows)
                    {
                        for (int i = 0; i <= 5; i++)
                        {
                            BK_TRANSSSRInfo = new Bk_transssr();
                            BK_TRANSSSRInfo.TransID = dr["TransID"].ToString();
                            BK_TRANSSSRInfo.RecordLocator = dr["PNR"].ToString();
                            BK_TRANSSSRInfo.CarrierCode = dr["CarrierCode"].ToString();
                            BK_TRANSSSRInfo.FlightNo = dr["FlightNo"].ToString();
                            BK_TRANSSSRInfo.Origin = dr["Destination"].ToString();
                            BK_TRANSSSRInfo.Destination = dr["Origin"].ToString();
                            BK_TRANSSSRInfo.PassengerID = dr["PassengerID"].ToString();
                            BK_TRANSSSRInfo.SubSeqNo = i;
                            if (i == 0)
                            {
                                BK_TRANSSSRInfo.SSRCode = dr["SSRCodeBaggage"].ToString();
                                BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceBaggage"];

                            }
                            else if (i == 1)
                            {
                                BK_TRANSSSRInfo.SSRCode = dr["SSRCodeMeal"].ToString();
                                BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceMeal"];
                            }
                            else if (i == 2)
                            {
                                BK_TRANSSSRInfo.SSRCode = dr["SSRCodeSport"].ToString();
                                BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceSport"];
                            }
                            else if (i == 3)
                            {
                                BK_TRANSSSRInfo.SSRCode = dr["SSRCodeComfort"].ToString();
                                BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceComfort"];
                            }
                            else if (i == 4)
                            {
                                BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDuty"].ToString();
                                BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDuty"];
                            }
                            listbk_transssrinfo.Add(BK_TRANSSSRInfo);
                            listbk_transssrinfo11.Add(BK_TRANSSSRInfo);
                        }
                    }
                }
                if (Session["dtGridPass21"] != null)
                {
                    dataClass = new DataTable();
                    dataClass = (DataTable)HttpContext.Current.Session["dtGridPass21"];
                    foreach (DataRow dr in dataClass.Rows)
                    {
                        for (int i = 0; i <= 5; i++)
                        {
                            BK_TRANSSSRInfo = new Bk_transssr();
                            BK_TRANSSSRInfo.TransID = dr["TransID"].ToString();
                            BK_TRANSSSRInfo.RecordLocator = dr["PNR"].ToString();
                            BK_TRANSSSRInfo.CarrierCode = dr["CarrierCode"].ToString();
                            BK_TRANSSSRInfo.FlightNo = dr["FlightNo"].ToString();
                            BK_TRANSSSRInfo.Origin = dr["Destination"].ToString();
                            BK_TRANSSSRInfo.Destination = dr["Origin"].ToString();
                            BK_TRANSSSRInfo.PassengerID = dr["PassengerID"].ToString();
                            BK_TRANSSSRInfo.SubSeqNo = i;
                            if (i == 0)
                            {
                                BK_TRANSSSRInfo.SSRCode = dr["SSRCodeBaggage"].ToString();
                                BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceBaggage"];

                            }
                            else if (i == 1)
                            {
                                BK_TRANSSSRInfo.SSRCode = dr["SSRCodeMeal"].ToString();
                                BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceMeal"];
                            }
                            else if (i == 2)
                            {
                                BK_TRANSSSRInfo.SSRCode = dr["SSRCodeSport"].ToString();
                                BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceSport"];
                            }
                            else if (i == 3)
                            {
                                BK_TRANSSSRInfo.SSRCode = dr["SSRCodeComfort"].ToString();
                                BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceComfort"];
                            }
                            else if (i == 4)
                            {
                                BK_TRANSSSRInfo.SSRCode = dr["SSRCodeDuty"].ToString();
                                BK_TRANSSSRInfo.SSRRate = (decimal)dr["PriceDuty"];
                            }
                            listbk_transssrinfo.Add(BK_TRANSSSRInfo);
                            listbk_transssrinfo21.Add(BK_TRANSSSRInfo);
                        }
                    }
                }
                listbk_transssrinfo1.RemoveAll(BK_TRANSSSRInfo => BK_TRANSSSRInfo.SSRCode == null || BK_TRANSSSRInfo.SSRCode == "");
                listbk_transssrinfo2.RemoveAll(BK_TRANSSSRInfo => BK_TRANSSSRInfo.SSRCode == null || BK_TRANSSSRInfo.SSRCode == "");
                listbk_transssrinfo11.RemoveAll(BK_TRANSSSRInfo => BK_TRANSSSRInfo.SSRCode == null || BK_TRANSSSRInfo.SSRCode == "");
                listbk_transssrinfo21.RemoveAll(BK_TRANSSSRInfo => BK_TRANSSSRInfo.SSRCode == null || BK_TRANSSSRInfo.SSRCode == "");
                listbk_transssrinfo.RemoveAll(BK_TRANSSSRInfo => BK_TRANSSSRInfo.SSRCode == null || BK_TRANSSSRInfo.SSRCode == "");

                DataRow row = dtList1.NewRow();
                foreach (Bk_transssr item in listbk_transssrinfo1)
                {
                    dtList1.Rows.Add(item.SSRCode, item.PassengerID);
                }

                DataRow row2 = dtList2.NewRow();
                foreach (Bk_transssr item in listbk_transssrinfo2)
                {
                    dtList2.Rows.Add(item.SSRCode, item.PassengerID);
                }

                DataRow row1 = dtList11.NewRow();
                foreach (Bk_transssr item in listbk_transssrinfo11)
                {
                    dtList11.Rows.Add(item.SSRCode, item.PassengerID);
                }
                DataRow row21 = dtList21.NewRow();
                foreach (Bk_transssr item in listbk_transssrinfo21)
                {
                    dtList21.Rows.Add(item.SSRCode, item.PassengerID);
                }
                SellFlight(dtList1, dtList2, listbk_transssrinfo);

                e.Result = "";
            }
            catch (Exception ex)
            {
                e.Result = msgList.Err100013;
            }
        }

        private void SellFlight(DataTable dtList1, DataTable dtList2, List<ABS.Logic.GroupBooking.Booking.Bk_transssr> listAll)
        {
            try
            {
                String SessionID = Session["signature"].ToString();
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

                Decimal totalSSRdepart = 0;
                Decimal totalSSRReturn = 0;
                if (ReturnID != "")
                {
                    strExpr = "TemFlightId = '" + ReturnID + "'";
                    strSort = "";

                    dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                    foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                    FillModelFromDataRow(foundRows, ref temFlight2);


                    ABS.Navitaire.APIBooking APIBooking = new ABS.Navitaire.APIBooking(SessionID);
                    ABS.Navitaire.BookingManager.GetAvailabilityResponse response = APIBooking.GetAvailability(temFlight.TemFlightArrival, Convert.ToDateTime(temFlight.TemFlightSta), temFlight.TemFlightCurrencyCode, temFlight.TemFlightDeparture, temFlight.TemFlightPaxNum, ref SessionID);
                    ABS.Navitaire.BookingManager.GetAvailabilityResponse response2 = APIBooking.GetAvailability(temFlight2.TemFlightArrival, Convert.ToDateTime(temFlight2.TemFlightSta), temFlight2.TemFlightCurrencyCode, temFlight2.TemFlightDeparture, temFlight2.TemFlightPaxNum, ref SessionID);
                    if (response != null)
                    {
                        GetSSRAvailabilityForBookingResponse ssrResponse = (GetSSRAvailabilityForBookingResponse)Session["GetssrAvailabilityResponseForBooking"];
                        SellResponse responseSSR = APIBooking.SellSSR(Session["signature"].ToString(), response, response2, ssrResponse, dtList1, dtList2);
                    }
                    ABS.Navitaire.BookingManager.Booking book = APIBooking.GetBookingFromState(SessionID);
                    string xml = GetXMLString(book);

                    for (int i = 0; i < book.Passengers.Length; i++)
                    {
                        for (int ii = 0; ii < book.Passengers[i].PassengerFees.Length; ii++)
                        {
                            if (book.Passengers[i].PassengerFees[ii].FlightReference != "")
                            {
                                if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == temFlight.TemFlightDeparture + temFlight.TemFlightArrival))
                                {
                                    if (book.Passengers[i].PassengerFees[ii].ServiceCharges[0].ChargeCode != "SVCF")
                                        totalSSRdepart += book.Passengers[i].PassengerFees[ii].ServiceCharges[0].Amount;
                                }
                            }
                        }
                    }

                    DataTable dtBDFee = objBooking.dtBreakdownFee();
                    dtBDFee = (DataTable)HttpContext.Current.Session["dataBDFeeDepart"];
                    foreach (DataRow row in dtBDFee.Rows)
                    {
                        row["SSR"] = totalSSRdepart;
                    }
                    HttpContext.Current.Session["dataBDFeeDepart"] = dtBDFee;

                    strExpr = "TemFlightId = '" + departID + "'";
                    strSort = "";
                    dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                    foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                    foundRows[0]["TemFlightTotalAmount"] = (Convert.ToDecimal(foundRows[0]["TemFlightTotalAmount"]) + totalSSRdepart);

                    HttpContext.Current.Session["TempFlight"] = dt;

                    for (int i = 0; i < book.Passengers.Length; i++)
                    {
                        for (int ii = 0; ii < book.Passengers[i].PassengerFees.Length; ii++)
                        {
                            if (book.Passengers[i].PassengerFees[ii].FlightReference != "")
                            {
                                if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 6) == temFlight.TemFlightArrival + temFlight.TemFlightDeparture))
                                {
                                    if (book.Passengers[i].PassengerFees[ii].ServiceCharges[0].ChargeCode != "SVCF")
                                        totalSSRReturn += book.Passengers[i].PassengerFees[ii].ServiceCharges[0].Amount;
                                }
                            }
                        }
                    }

                    dtBDFee = objBooking.dtBreakdownFee();
                    dtBDFee = (DataTable)HttpContext.Current.Session["dataBDFeeReturn"];
                    foreach (DataRow row in dtBDFee.Rows)
                    {
                        row["SSR"] = totalSSRReturn;
                    }
                    HttpContext.Current.Session["dataBDFeeReturn"] = dtBDFee;

                    strExpr = "TemFlightId = '" + ReturnID + "'";
                    strSort = "";
                    dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                    foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                    foundRows[0]["TemFlightTotalAmount"] = (Convert.ToDecimal(foundRows[0]["TemFlightTotalAmount"]) + totalSSRReturn);

                    HttpContext.Current.Session["TempFlight"] = dt;
                }
                else
                {

                    ABS.Navitaire.APIBooking APIBooking = new ABS.Navitaire.APIBooking(SessionID);
                    ABS.Navitaire.BookingManager.GetAvailabilityResponse response = APIBooking.GetAvailability(temFlight.TemFlightArrival, Convert.ToDateTime(temFlight.TemFlightSta), temFlight.TemFlightCurrencyCode, temFlight.TemFlightDeparture, temFlight.TemFlightPaxNum, ref SessionID);
                    //ABS.Navitaire.BookingManager.GetAvailabilityResponse response2 = APIBooking.GetAvailability(temFlight2.TemFlightArrival, Convert.ToDateTime(temFlight2.TemFlightSta), temFlight2.TemFlightCurrencyCode, temFlight2.TemFlightDeparture, temFlight2.TemFlightPaxNum, ref SessionID);
                    if (response != null)
                    {
                        GetSSRAvailabilityForBookingResponse ssrResponse = (GetSSRAvailabilityForBookingResponse)Session["GetssrAvailabilityResponseForBooking"];
                        SellResponse responseSSR = APIBooking.SellSSR(Session["signature"].ToString(), response, null, ssrResponse, dtList1, null);
                    }
                    ABS.Navitaire.BookingManager.Booking book = APIBooking.GetBookingFromState(SessionID);

                    for (int i = 0; i < book.Passengers.Length; i++)
                    {
                        for (int ii = 0; ii < book.Passengers[i].PassengerFees.Length; ii++)
                        {
                            if (book.Passengers[i].PassengerFees[ii].FlightReference != "")
                            {
                                if ((book.Passengers[i].PassengerFees[ii].FlightReference.Substring(16, 3) == temFlight.TemFlightDeparture + temFlight.TemFlightArrival))
                                {
                                    if (book.Passengers[i].PassengerFees[ii].ServiceCharges[0].ChargeCode != "SVCF")
                                        totalSSRdepart += book.Passengers[0].PassengerFees[i].ServiceCharges[0].Amount;
                                }
                            }
                        }
                    }

                    DataTable dtBDFee = objBooking.dtBreakdownFee();
                    dtBDFee = (DataTable)HttpContext.Current.Session["dataBDFeeDepart"];
                    foreach (DataRow row in dtBDFee.Rows)
                    {
                        row["SSR"] = totalSSRdepart;
                    }
                    HttpContext.Current.Session["dataBDFeeDepart"] = dtBDFee;

                    strExpr = "TemFlightId = '" + departID + "'";
                    strSort = "";
                    dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                    foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                    foundRows[0]["TemFlightTotalAmount"] = (Convert.ToDecimal(foundRows[0]["TemFlightTotalAmount"]) + totalSSRdepart);

                    HttpContext.Current.Session["TempFlight"] = dt;
                }
                int cnt = 0;
                objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                foreach (Bk_transssr b in listAll)
                {

                    cnt++;
                    if (objBooking.SaveSSR(b, CoreBase.EnumSaveType.Insert, "", true, cnt == listAll.Count ? true : false))
                    {

                    }
                }

                if (listAll.Count > 0)
                {
                    List<BookingTransactionDetail> listBookingDetail = new List<BookingTransactionDetail>();
                    listBookingDetail = objBooking.GetAllBK_TRANSDTLFilterAll(listAll[0].TransID, 0);

                    int iIndexDepart = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 1);
                    int iIndexReturn = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 0);
                    if (iIndexDepart >= 0) listBookingDetail[iIndexDepart].LineSSR = totalSSRdepart;
                    if (iIndexReturn >= 0) listBookingDetail[iIndexReturn].LineSSR = totalSSRReturn;

                    BookingTransactionMain bookingMain = new BookingTransactionMain();
                    bookingMain = objBooking.GetSingleBK_TRANSMAIN(listAll[0].TransID);
                    bookingMain.TransTotalSSR = totalSSRdepart + totalSSRReturn;
                    bookingMain.TotalAmtGoing += totalSSRdepart;
                    bookingMain.TotalAmtReturn += totalSSRReturn;
                    bookingMain.TransSubTotal += totalSSRdepart + totalSSRReturn;
                    bookingMain.TransTotalAmt += totalSSRdepart + totalSSRReturn;

                    objBooking.UpdateTotalSSR(listAll[0].TransID, bookingMain, listBookingDetail);
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

    }
}