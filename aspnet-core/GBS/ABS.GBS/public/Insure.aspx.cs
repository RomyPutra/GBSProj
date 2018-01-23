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
using StackExchange.Profiling;

namespace GroupBooking.Web
{
    public partial class Insure : System.Web.UI.Page
    {
        #region declaration
        UserSet MyUserSet;
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogControl log = new LogControl();
        SystemLog SystemLog = new SystemLog();

        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        //Bk_transssr BK_TRANSSSRInfo = new Bk_transssr();
        //List<Bk_transssr> listbk_transssrinfo = new List<Bk_transssr>();
        //List<Bk_transssr> listbk_transssrinfo1 = new List<Bk_transssr>();
        //List<Bk_transssr> listbk_transssrinfo2 = new List<Bk_transssr>();
        //List<Bk_transssr> listbk_transssrinfo1t = new List<Bk_transssr>();
        //List<Bk_transssr> listbk_transssrinfo2t = new List<Bk_transssr>();

        Bk_transaddon BK_TRANSADDONInfo = new Bk_transaddon();
        List<Bk_transaddon> listBK_TRANSADDONInfo = new List<Bk_transaddon>();
        //List<Bk_transaddon> listBK_TRANSADDONInfo1 = new List<Bk_transaddon>();
        //List<Bk_transaddon> listBK_TRANSADDONInfo2 = new List<Bk_transaddon>();
        //List<Bk_transaddon> listBK_TRANSADDONInfo1t = new List<Bk_transaddon>();
        //List<Bk_transaddon> listBK_TRANSADDONInfo2t = new List<Bk_transaddon>();

        //Bk_transaddon BK_TRANSADDONInfoOld = new Bk_transaddon();
        //List<Bk_transaddon> listBK_TRANSADDONInfoOld = new List<Bk_transaddon>();
        //List<Bk_transaddon> listBK_TRANSADDONInfo1Old = new List<Bk_transaddon>();
        //List<Bk_transaddon> listBK_TRANSADDONInfo2Old = new List<Bk_transaddon>();
        //List<Bk_transaddon> listBK_TRANSADDONInfo1tOld = new List<Bk_transaddon>();
        //List<Bk_transaddon> listBK_TRANSADDONInfo2tOld = new List<Bk_transaddon>();

        List<BookingTransactionDetail> objListBK_TRANSDTL_Infos = new List<BookingTransactionDetail>();
        //List<Bk_transaddon> listSSRPNR = new List<Bk_transaddon>();
        //BookingTransactionDetail objBK_TRANSDTL_Infos;

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

        List<BookingTransactionDetail> lstBookDTL = new List<BookingTransactionDetail>();
        List<BookingTransactionDetail> OldlstBookDTL = new List<BookingTransactionDetail>();

        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
        ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");

        ABS.GBS.eService.ProcessServiceSoapClient eWS = new ABS.GBS.eService.ProcessServiceSoapClient();
        //BookingTransactionDetail bookDTLInfo = new BookingTransactionDetail();
        List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
        BookingTransactionMain bookHDRInfo = new BookingTransactionMain();
        ABS.Logic.GroupBooking.Agent.AgentControl objAgent = new ABS.Logic.GroupBooking.Agent.AgentControl();
        ABS.Logic.GroupBooking.Agent.AgentProfileControl objAgentProfile = new ABS.Logic.GroupBooking.Agent.AgentProfileControl();
        ABS.Logic.GroupBooking.Agent.AgentProfile agent = new ABS.Logic.GroupBooking.Agent.AgentProfile();
        PassengerData PassData2 = new PassengerData();
        List<PassengerData> lstPassInfantData = new List<PassengerData>();
        static bool IsInternationalFlight = false;
        //DataTable dtTaxFees = new DataTable();
        //DataTable dtList1 = new DataTable();
        //DataTable dtList2 = new DataTable();
        //DataTable dtList1t = new DataTable();
        //DataTable dtList2t = new DataTable();
        //private static int qtyMeal, qtyMeal1, qtyDrink, qtyDrink1, qtyBaggage, qtySport, qtyInsure, qtyComfort, qtyDuty = 0;//edited by romy, 20170811, insurance
        //private static int qtyMeal2, qtyMeal21, qtyDrink2, qtyDrink21, qtyBaggage2, qtySport2, qtyInsure2, qtyComfort2, qtyDuty2 = 0;//edited by romy, 20170811, insurance
        private static int qtyInsure;
        //private static int num = 0;
        //private static int departID = 0;
        //private static string ReturnID = "";
        private static int first = 0;
        //private static int firstinit = 0;
        //decimal totalamountduegoing = 0;
        //decimal totalamountduereturn = 0;
        decimal totalamountduegoingcommit = 0;
        //decimal totalamountduereturncommit = 0;
        //decimal totalamountduegoinginfant = 0;
        //decimal totalamountduereturninfant = 0;
        //decimal totalamountduegoingcommitinfant = 0;
        //decimal totalamountduereturncommitinfant = 0;
        String TransID = "";
        //int havebalance = 0;
        string depart1 = "", depart2 = "", transit1 = "", transit2 = "", return1 = "", return2 = "";
        //private string Curr = "";

        string SellSessionID = "";
        private static decimal DefFee = 0;
        bool Cleared = false;

        static private DataTable dtPassOld = new DataTable();
        static private DataTable dtPass2Old = new DataTable();
        private static int infantmax = 0;

        static bool IsDepart = false, IsDepartTransit = false, IsDepartTransit2 = false, IsReturn = false, IsReturnTransit = false, IsReturnTransit2 = false;

        DataTable dtInsure = new DataTable();//added by romy, 20170811, insurance
        #endregion

        #region "Load"
        protected void Page_Load(object sender, EventArgs e)
        {
            string signature = "";
            DataTable dt = new DataTable();
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;
            try
            {
                TransID = Request.QueryString["TransID"];
                ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking("");
                Session["PaxStatus"] = "";
                Session["dtGridPassOld"] = "";
                if (Session["AgentSet"] != null)
                {
                    MyUserSet = (UserSet)Session["AgentSet"];
                    if (!IsPostBack)
                    {
                        using (profiler.Step("GBS : ClearSession"))
                        {
                            ClearSession();
                        }

                        using (profiler.Step(" GBS : InitializeForm"))
                        {
                            InitializeForm("Depart");
                        }
                        //using (profiler.Step("GetSellSSR"))
                        //{
                        //    GetSellSSR(signature, TransID);
                        //}
                        //BindDefaultDrink();
                    }
                    //using (profiler.Step("SetTab"))
                    //{
                    //    SetTab();
                    //}

                    if (Session["dtGridPass"] == null)
                    {
                        using (profiler.Step("GetPassengerList"))
                        {
                            GetPassengerList(TransID, "Depart");
                        }
                    }

                    using (profiler.Step("GetSSRItem"))
                    {
                        GetSSRItem("Depart");
                    }

                    if (Session["OneWay"] != null)
                    {
                        Boolean OneWay = (Boolean)Session["OneWay"];
                        if (OneWay != true)
                        {
                            using (profiler.Step("GetSSRItem"))
                            {
                                GetSSRItem("Return");
                            }
                        }
                    }

                    using (profiler.Step("GetSSRItem"))
                    {
                        GetSSRItem("Depart");
                    }
                    System.Data.DataTable GetConfirm;//added by romy, 20170815, Insurance
                    using (profiler.Step("GetUpcomingFlightInsure"))
                    {
                        GetConfirm = objBooking.GetUpcomingFlightInsure(MyUserSet.AgentID.ToString(), TransID.ToString());//added by romy, 20170815, Insurance
                    }
                    if (GetConfirm == null)
                    {
                        //IcnInsure1.Style.Add("display", "none");
                        gvPassenger.Columns["DepartInsure"].Visible = false;
                    }
                    else if (GetConfirm != null && GetConfirm.Rows[0]["UpcomingFlight"].ToString() != "3")
                    {
                        //IcnInsure1.Style.Add("display", "none");
                        gvPassenger.Columns["DepartInsure"].Visible = false;
                    }

                    //DataTable dtPass = new DataTable();
                    //dtPass = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTableNewManage(TransID, false, "");
                    //Session["dtGridPassOld"] = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTableNewManage(TransID, false, "");
                    using (profiler.Step("GetAllBK_PASSENGERLISTdataTable(PassOld)"))
                    {
                        Session["dtGridPassOld"] = objBooking.GetAllBK_PASSENGERLISTdataTable(TransID);
                    }
                    DataTable data = Session["dtGridPassOld"] as DataTable;
                    Session["TotalInsure"] = data.Compute("Sum(InsureFee)", "");
                    int i = 0;
                    int ii = 0;
                    foreach (DataRow dr in data.Rows)
                    {
                        if (data.Rows[i]["InsureCode"].ToString() != "")
                        {
                            ii++;
                            Session["qtyInsure"] = ii;
                        }
                        i++;
                    }
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

        #endregion

        #region "Initializer"
        protected void SetTab()
        {
            GetBookingResponse resp = new GetBookingResponse();
            if (Session["resp"] != null) resp = (GetBookingResponse)Session["resp"];
            if ((IsDepart == true && IsDepartTransit == false && IsReturn == false && IsReturnTransit == false) || (IsDepart == false && IsDepartTransit == true && IsReturn == false && IsReturnTransit == false))
            {
                InitializeSetting(resp.Booking.Journeys[0].Segments[0].DepartureStation, resp.Booking.Journeys[0].Segments[0].ArrivalStation);
            }
            else if (IsDepart == true && IsDepartTransit == false && IsReturn == true && IsReturnTransit == false)
            {
                InitializeSetting(resp.Booking.Journeys[0].Segments[0].DepartureStation, resp.Booking.Journeys[0].Segments[0].ArrivalStation);
                //InitializeSetting(resp.Booking.Journeys[0].Segments[0].DepartureStation, resp.Booking.Journeys[0].Segments[0].ArrivalStation, resp.Booking.Journeys[1].Segments[0].DepartureStation, resp.Booking.Journeys[1].Segments[0].ArrivalStation);
            }
            else if (IsDepart == false && IsDepartTransit == true && IsReturn == false && IsReturnTransit == true)
            {
                InitializeSetting(resp.Booking.Journeys[0].Segments[0].DepartureStation, resp.Booking.Journeys[0].Segments[0].ArrivalStation);
                //InitializeSetting(resp.Booking.Journeys[0].Segments[0].DepartureStation, resp.Booking.Journeys[0].Segments[1].ArrivalStation, resp.Booking.Journeys[1].Segments[0].DepartureStation, resp.Booking.Journeys[1].Segments[1].ArrivalStation);
            }
            else if (IsDepart == true && IsDepartTransit == false && IsReturn == false && IsReturnTransit == true)
            {
                InitializeSetting(resp.Booking.Journeys[0].Segments[0].DepartureStation, resp.Booking.Journeys[0].Segments[0].ArrivalStation);
                //InitializeSetting(resp.Booking.Journeys[0].Segments[0].DepartureStation, resp.Booking.Journeys[0].Segments[0].ArrivalStation, resp.Booking.Journeys[1].Segments[0].DepartureStation, resp.Booking.Journeys[1].Segments[1].ArrivalStation);
            }
            else if (IsDepart == false && IsDepartTransit == true && IsReturn == true && IsReturnTransit == false)
            {
                InitializeSetting(resp.Booking.Journeys[0].Segments[0].DepartureStation, resp.Booking.Journeys[0].Segments[0].ArrivalStation);
                //InitializeSetting(resp.Booking.Journeys[0].Segments[0].DepartureStation, resp.Booking.Journeys[0].Segments[1].ArrivalStation, resp.Booking.Journeys[1].Segments[0].DepartureStation, resp.Booking.Journeys[1].Segments[0].ArrivalStation);
            }
        }
        private void ClearSession()
        {
            Session["dtGridPass"] = null;
            Session["dtGridPassOld"] = null;
            Session["TotalInsure"] = null;
            Session["qtyInsure"] = null;
            Session["resp"] = null;
            Session["RespInsure"] = null;
            Session["InfantCheck"] = null;
            Session["dtDefaultInsure"] = null;
            Session["dtOldInsure"] = null;
            Session["Currency"] = null;
            Session["DepartRoute"] = null;
            Session["dtInsureDepart"] = null;
            //Session["IsNew"] = "True";
            //Session["dtGridPass"] = null;
            //Session["dtBaggageDepart"] = null;
            //Session["dtBaggageReturn"] = null;
            //Session["Currency"] = null;
            //Session["qtyInsure"] = null;//added by romy, 20170811, insurance
            //Session["TotalAmountReturn"] = null;
            //Session["TotalAmountDepart"] = null;
            HttpContext.Current.Session.Remove("qtyInsure");//added by romy, 20170811, insurance
            HttpContext.Current.Session.Remove("dtBaggage");
            HttpContext.Current.Session.Remove("dtGridPass");
            Session["IsNew"] = "true";
            HttpContext.Current.Session.Remove("dtInsureDepart");//added by romy, 20170811, insurance
            HttpContext.Current.Session.Remove("qtyInsure");//added by romy, 20170811, insurance
            HttpContext.Current.Session.Remove("qtyInsure2");//added by romy, 20170811, insurance
            HttpContext.Current.Session.Remove("TotalInsure");//added by romy, 20170811, insurance
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
        }
        protected void GetInsureDepart()//added by romy, 20170811, insurance
        {
            System.Data.DataTable GetConfirm;//added by romy, 20170815, Insurance
            GridViewDataComboBoxColumn column = (gvPassenger.Columns["DepartInsure"] as GridViewDataComboBoxColumn);
            DataTable dtBaggage = null;
            dtBaggage = (DataTable)Session["dtInsureDepart"];
            DataRow[] result = dtBaggage.Select("SSRCode = ''");
            //DataRow[] result = dtBaggage.Select("FeeCode = ''");
            //if (result.Length == 0)
            //{
            //    dtBaggage.Rows.Add("", "", "");
            //}
            column.PropertiesComboBox.DataSource = dtBaggage;
            column.PropertiesComboBox.ValueField = "SSRCode";
            //column.PropertiesComboBox.ValueField = "FeeCode";
            column.PropertiesComboBox.TextField = "ConcatenatedField";

            GetConfirm = objBooking.GetUpcomingFlightInsure(MyUserSet.AgentID.ToString(), TransID.ToString());//added by romy, 20170815, Insurance
            if (GetConfirm == null)
            {
                column.Visible = false;
            }
            else if (GetConfirm != null && GetConfirm.Rows[0]["UpcomingFlight"].ToString() != "3")
            {
                column.Visible = false;
            }

            gvPassenger.DataBind();
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
                    if (Session["qtyInsure"] == null)//added by romy, 20170811, insurance
                    {
                        seInsure.MaxValue = Convert.ToInt32(cookie2.Values["PaxNum"]);
                    }
                    else
                    {
                        seInsure.MaxValue = Convert.ToInt32(cookie2.Values["PaxNum"]) - Convert.ToInt16(Session["qtyInsure"]);
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

        protected void InsureDefault(string Code, string Descript, List<InsureResponse> InsureAmount, DataTable InsureDescript = null)
        {
            var profiler = MiniProfiler.Current;
            MessageList msgList = new MessageList();
            try
            {
                int promt = 0, j = 0, Once = 0;
                //String Detail = "";
                object[] findTheseVals = new object[2];
                Session["dtDefaultInsure"] = null;
                //Session["dtOldInsure"] = null;
                DataTable dataResp = new DataTable();
                if (Session["RespInsure"] != null)
                {
                    //datax=(DataTable)Session["RespInsure"];
                    dataResp = GetPlan.ConvertToDataTable((List<InsureResponse>)Session["RespInsure"]);
                    dataResp.PrimaryKey = new DataColumn[] { (dataResp.Columns["PNR"]), (dataResp.Columns["PassengerNumber"]) };
                }
                DataTable dtPass = new DataTable();
                DataTable dtInsured = new DataTable();
                dtInsured = objBooking.GetAllBK_PASSENGERLISTdataTable(TransID);
                dtPass = objBooking.GetAllBK_PASSENGERLISTdataTable(TransID);
                DataTable dtInfant = new DataTable();
                dtInfant = (DataTable)Session["InfantCheck"];
                //DataTable dtInsure = (DataTable)Session["dtInsureDepart"];

                for (int i = 0; i <= dtPass.Rows.Count - 1; i++)
                {
                    if (Code == null && Descript == null && InsureAmount == null && dtPass.Rows[i]["InsureCode"].ToString() == "")
                    {
                        dtPass.Rows[i]["Status"] = msgList.Err200000;
                        dtPass.Rows[i]["IndicatorClear"] = 1;
                    }
                    else if (Code == null && Descript == null && InsureAmount == null && dtPass.Rows[i]["InsureCode"].ToString() != "" && Once == 0)
                    {
                        DataRow[] result = InsureDescript.Select("ItemCode = '" + dtPass.Rows[i]["InsureCode"] + "'");
                        if (result.Length > 0)
                        {
                            foreach (DataRow row in result)
                            {
                                dtInsure.Rows.Add(row[0], row[1], " ", " ", Convert.ToDecimal("0.00"));
                                Once++;
                            }
                        }
                    }
                    if (Descript == "")
                    {
                        gvPassenger.JSProperties["cp_result"] = msgList.Err200021;
                        return;
                    }
                    else
                    {
                        if (dtPass.Rows[i]["InsureCode"].ToString() == "")
                        {
                            findTheseVals[0] = (dtPass.Rows[i]["RecordLocator"]);
                            findTheseVals[1] = (dtPass.Rows[i]["PassengerID"]);
                            decimal Amount = 0;
                            //string Amount = "";
                            if (InsureAmount != null && InsureAmount.Where(p => p.PNR == findTheseVals[0].ToString() && p.PassengerNumber == findTheseVals[1].ToString()).Count() > 0)
                                Amount = InsureAmount.Where(p => p.PNR == findTheseVals[0].ToString() && p.PassengerNumber == findTheseVals[1].ToString()).FirstOrDefault().Amount;
                            Amount = Amount + Convert.ToDecimal(0.00);
                            //Amount = InsureAmount.Where(p => p.PNR == findTheseVals[0].ToString() && p.PassengerNumber == findTheseVals[1].ToString()).FirstOrDefault().Amount.ToString();
                            DataRow rowResp = dataResp.NewRow();
                            if (dataResp != null && dataResp.Rows.Count > 0)
                            {
                                rowResp = dataResp.Rows.Find(findTheseVals);
                            }
                            if (rowResp != null)
                            {
                                dtPass.Rows[i]["InsureCode"] = Code;
                                dtPass.Rows[i]["InsureFee"] = string.Format("{0:0.00}", Amount);// Math.Round(Convert.ToDecimal(Amount), 2);
                                dtPass.Rows[i]["InsureFee1"] = Amount == 0 ? "" : string.Format("{0:0.00}", Amount).ToString();
                                promt++;
                            }
                            else
                            {
                                dtPass.Rows[i]["InsureFee1"] = "";
                                dtPass.Rows[i]["InsureCode"] = msgList.Err200000;
                                dtPass.Rows[i]["Status"] = msgList.Err200000;
                                dtPass.Rows[i]["IndicatorClear"] = 1;
                            }
                        }
                    }
                }
                //if (dtPass.Rows.Count == promt)
                //{
                //    gvPassenger.JSProperties["cp_result"] = msgList.Err200022;
                //}
                seInsure.MaxValue = promt;
                MaxPax.Value = promt.ToString();
                if (promt > 0) MinPax.Value = "1";
                //seInsure.MaxValue = dtPass.Rows.Count;
                //MaxPax.Value = dtPass.Rows.Count.ToString();
                promt = 0;
                Session["dtDefaultInsure"] = dtPass;
                Session["dtOldInsure"] = dtInsured;
                DefFee = (Convert.ToDecimal(dtPass.Compute("Sum(InsureFee)", "")) - Convert.ToDecimal(dtInsured.Compute("Sum(InsureFee)", "")));
                lblTotalAmount.Text = (Convert.ToDecimal(string.IsNullOrEmpty(lblTotalAmount.Text) ? "0" : lblTotalAmount.Text) + (Convert.ToDecimal(dtPass.Compute("Sum(InsureFee)", "")) - Convert.ToDecimal(dtInsured.Compute("Sum(InsureFee)", "")))).ToString("N", nfi);
                //DefFee = (Convert.ToDecimal(dtPass.Compute("Sum(string.IsNullOrEmpty(InsureFee) ? 0 : InsureFee)", "")) - Convert.ToDecimal(dtInsured.Compute("Sum(string.IsNullOrEmpty(InsureFee) ? 0 : InsureFee)", "")));
                //lblTotalAmount.Text = (Convert.ToDecimal(string.IsNullOrEmpty(lblTotalAmount.Text) ? "0" : lblTotalAmount.Text) + (Convert.ToDecimal(dtPass.Compute("Sum(string.IsNullOrEmpty(InsureFee) ? 0 : InsureFee)", "")) - Convert.ToDecimal(dtInsured.Compute("Sum(string.IsNullOrEmpty(InsureFee) ? 0 : InsureFee)", "")))).ToString("N", nfi);
                lblCurrency.Text = Session["Currency"].ToString();
                if (first == 0)
                {
                    using (profiler.Step("BindLabel"))
                    {
                        BindLabel();
                    }
                }

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
        }
        protected void InitializeSetting(string Departure1, string Arrival1)
        {
            try
            {
                string temp = "";
                if (Departure1 != "" && Arrival1 != "")
                {
                    temp = Departure1 + " | " + Arrival1;
                    //TabControl.TabPages[0].Text = temp;
                    //TabControl.TabPages[1].Visible = false;
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
        protected void InitializeForm(String Flight)//edited by romy, 20170811, insurance
        {
            List<BookingTransactionDetail> lstTransDetail = new List<BookingTransactionDetail>();
            String Currency = "";
            MessageList msgList = new MessageList();
            var profiler = MiniProfiler.Current;
            HttpCookie cookie = Request.Cookies["cookieSearchcondition"];
            try
            {
                string strHTML = "", FilterPass = "";
                BaseResource SAOTune = new BaseResource();
                List<BookingTransactionDetail> lstFlights;
                using (profiler.Step("GBS : GetAllBK_TRANSDTLCombinePNR"))
                {
                    lstFlights = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 0, "LEN(RecordLocator) >= 6 AND ");
                }
                DataTable dtPass, dtPassIn;
                using (profiler.Step("GBS : BookingDetailFilter"))
                {
                    dtFlight = objBooking.BookingDetailFilter(TransID);
                }

                using (profiler.Step("GBS : LoadSAOXML"))
                {
                    SAOTune = objGeneral.LoadSAOXML(Request.PhysicalApplicationPath, objBooking.GetCountryCode(dtFlight[0].Origin.ToString(), Request.PhysicalApplicationPath).ToString());
                }

                using (profiler.Step("GBS : IsInternationalFlight"))
                {
                    IsInternationalFlight = objGeneral.IsInternationalFlight(dtFlight[0].Origin.ToString(), dtFlight[0].Destination.ToString(), Request.PhysicalApplicationPath);
                }
                //SAOTune = objGeneral.LoadSAOXML(Request.PhysicalApplicationPath, SAOCountryCode.Value);
                if (SAOTune != null)
                {
                    using (profiler.Step("GBS : Loop SAOXML"))
                    {
                        foreach (Insurance Sao in SAOTune.Insurances[0].Insurance)
                        {
                            if (Sao.CultureCode == "en-GB")
                            {
                                if (IsInternationalFlight)
                                {
                                    strHTML += "<span class='ti-check-box'></span> " + Sao.Itnl[0].CheckItems[0].CheckItem[0].ToString() + "</br>";
                                    strHTML += "<span class='ti-check-box'></span> " + Sao.Itnl[0].CheckItems[0].CheckItem[1].ToString() + "</br>";
                                    strHTML += "<span class='ti-check-box'></span> " + Sao.Itnl[0].CheckItems[0].CheckItem[2].ToString() + "</br>";
                                    strHTML += "<span class='ti-check-box'></span> " + Sao.Itnl[0].CheckItems[0].CheckItem[3].ToString() + "</br>";
                                    strHTML += "<img src = '" + Sao.Itnl[0].UpsellUrl.ToString() + "' class='avatar mCS_img_loaded' data-pin-nopin='true'></br>";
                                    strHTML += Sao.Itnl[0].ConfirmContent.ToString();
                                }
                                else
                                {
                                    strHTML += "<span class='ti-check-box'></span> " + Sao.Dom[0].CheckItems[0].CheckItem[0].ToString() + "</br>";
                                    strHTML += "<span class='ti-check-box'></span> " + Sao.Dom[0].CheckItems[0].CheckItem[1].ToString() + "</br>";
                                    strHTML += "<span class='ti-check-box'></span> " + Sao.Dom[0].CheckItems[0].CheckItem[2].ToString() + "</br>";
                                    strHTML += "<span class='ti-check-box'></span> " + Sao.Dom[0].CheckItems[0].CheckItem[3].ToString() + "</br>";
                                    strHTML += "<img src = '" + Sao.Dom[0].UpsellUrl.ToString() + "' class='avatar mCS_img_loaded' data-pin-nopin='true'></br>";
                                    strHTML += Sao.Dom[0].ConfirmContent.ToString();
                                }
                            }
                        }
                    }
                }
                divSAO.InnerHtml = strHTML;

                if (IsInternationalFlight)
                {
                    FilterPass = " (p.FirstName <> 'TBA' AND p.LastName <> 'TBA' AND p.PassportNo <> 'TBA')";
                }
                else
                {
                    FilterPass = " (p.FirstName <> 'TBA' AND p.LastName <> 'TBA')";
                }

                using (profiler.Step("GBS : GetAllBK_PASSENGERLISTWithSSRDataTableNewManage"))
                {
                    dtPass = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTableNewManage(TransID, false, FilterPass);
                }

                using (profiler.Step("GBS : GetAllBK_PASSENGERLISTWithInfant"))
                {
                    dtPassIn = objBooking.GetAllBK_PASSENGERLISTWithInfant(TransID, false, FilterPass);
                }
                //added by romy for Double Premium Amount
                Session["InfantCheck"] = dtPass;

                int SeqNo = 1;
                using (profiler.Step("GBS : Loop list from GetAllBK_TRANSDTLCombinePNR"))
                {
                    for (int p = 0; p < lstFlights.Count; p++)
                    {
                        if (Session["Currency"] == null)
                        {
                            Currency = lstFlights[0].Currency;
                            Session["Currency"] = Currency;
                        }
                        using (profiler.Step("GBS : Loop datatable from GetAllBK_PASSENGERLISTWithInfant"))
                        {
                            foreach (DataRow drRow in dtPassIn.Rows)
                            {
                                if ((string)drRow["PNR"] == lstFlights[p].RecordLocator && (drRow["PriceDepartInsure"] != null && drRow["PriceDepartInsure"].ToString() == "0.00" && drRow["Title"].ToString() != "INFT"))
                                {
                                    IPassenger passenger = new IPassenger();
                                    passenger.FirstName = (string)drRow["FirstName"];
                                    passenger.LastName = (string)drRow["LastName"];
                                    passenger.DOB = ((DateTime)drRow["DOB"]).ToString("yyyy-MM-dd HH:mm:ss");
                                    passenger.CountryOfResidence = (string)drRow["IssuingCountry"];
                                    passenger.CurrencyCode = lstFlights[0].Currency;
                                    passenger.Nationality = (string)drRow["Nationality"];
                                    passenger.Gender = (string)drRow["Gender"];
                                    passenger.IdentityType = "Passport";
                                    passenger.PassengerNumber = (string)drRow["PassengerID"]; //when pass, is actually pass this pass id
                                    lstPassenger.Add(passenger);
                                }
                                else if ((string)drRow["PNR"] == lstFlights[p].RecordLocator && (drRow["PriceDepartInsure"] != null && drRow["PriceDepartInsure"].ToString() == "0.00" && drRow["Title"].ToString() == "INFT"))
                                {
                                    DataRow[] PassID = dtPassIn.Select("PassengerID = '" + (string)drRow["PassengerID"] + "' and PNR = '" + (string)drRow["PNR"] + "' and Title <> 'INFT' and PriceDepartInsure = 0.00");
                                    if (PassID != null && PassID.Length > 0)
                                    {
                                        IPassenger passenger = new IPassenger();
                                        passenger.FirstName = (string)drRow["FirstName"];
                                        passenger.LastName = (string)drRow["LastName"];
                                        passenger.DOB = ((DateTime)drRow["DOB"]).ToString("yyyy-MM-dd HH:mm:ss");
                                        passenger.CountryOfResidence = (string)drRow["IssuingCountry"];
                                        passenger.CurrencyCode = lstFlights[0].Currency;
                                        passenger.Nationality = (string)drRow["Nationality"];
                                        passenger.Gender = (string)drRow["Gender"];
                                        passenger.IdentityType = "Passport";
                                        foreach (DataRow row in PassID)
                                        {
                                            passenger.PassengerNumber = "10" + (string)drRow["PassengerID"]; //when pass, is actually pass this pass id
                                        }
                                        lstPassenger.Add(passenger);
                                    }
                                }
                            }
                        }

                        int TPax = 0, PaxAdult = 0, PaxChild = 0, r = 0;
                        String SessionID = "";
                        Flight flight = new Flight();

                        using (profiler.Step("GBS : Loop datatable from BookingDetailFilter"))
                        {
                            for (int fff = 0; fff < dtFlight.Count; fff++)
                            {
                                if (dtFlight[fff].RecordLocator == lstFlights[p].RecordLocator && r == 0)
                                {
                                    flight.DepartCountryCode = objBooking.GetCountryCode(dtFlight[fff].Origin.ToString(), Request.PhysicalApplicationPath).ToString();
                                    flight.ArrivalCountryCode = objBooking.GetCountryCode(dtFlight[fff].Destination.ToString(), Request.PhysicalApplicationPath).ToString();
                                    flight.DepartStationCode = dtFlight[fff].Origin;
                                    flight.ArrivalStationCode = dtFlight[fff].Destination;
                                    flight.DepartAirlineCode = dtFlight[fff].CarrierCode.Substring(0, 2);
                                    flight.DepartDateTime = dtFlight[fff].DepatureDate.ToString("yyyy-MM-dd HH:mm:ss");
                                    flight.DepartFlightNo = dtFlight[fff].FlightNo.ToString();
                                    if (dtFlight[fff].Transit != null && dtFlight[fff].Transit.ToString().Trim() != "")
                                    {
                                        flight.FlightType = msgList.Err200001;
                                    }
                                    else
                                    {
                                        flight.FlightType = msgList.Err200002;
                                    }
                                    flight.FlightDates = "STA";
                                    flight.TotalFlightHour = dtFlight[fff].FlightDuration.ToString();
                                    r++;
                                }
                                else if (dtFlight[fff].RecordLocator == lstFlights[p].RecordLocator && r == 1)
                                {
                                    flight.ReturnAirlineCode = dtFlight[fff].CarrierCode.Substring(0, 2);
                                    flight.ReturnDateTime = dtFlight[fff].DepatureDate.ToString("yyyy-MM-dd HH:mm:ss");
                                    flight.ReturnFlightNo = dtFlight[fff].FlightNo.ToString();
                                }
                            }
                        }
                        lstFlight.Add(flight);
                        //added by romy for Double Premium Amount
                        Session["DepartRoute"] = flight.DepartCountryCode;

                        using (profiler.Step("Tune : GetAvailablePlan"))
                        {
                            RespPlan = GetPlan.GetAvailablePlan(dtFlight[0].Currency.ToString(), lstPassenger, lstFlight);
                        }

                        using (profiler.Step("Tune : Loop the response from GetAvailablePlan"))
                        {
                            for (int rp = 0; rp < RespPlan.QualifiedPassenger.Count; rp++)
                            {
                                if (RespPlan.QualifiedPassenger[rp].PassengerNumber.ToString().Length < 3)
                                {
                                    BK_RespInsure = new InsureResponse();
                                    BK_RespInsure.SeqNo = SeqNo.ToString();
                                    BK_RespInsure.PNR = lstFlights[p].RecordLocator;
                                    BK_RespInsure.PassengerNumber = RespPlan.QualifiedPassenger[rp].PassengerNumber.ToString(); //when retrieve, actually retrieve back this pass id
                                    BK_RespInsure.FeeCode = RespPlan.SSRFeeCode.ToString();
                                    BK_RespInsure.IsQualified = RespPlan.QualifiedPassenger[rp].IsPassengerQualified.ToString();
                                    string InfantId = "10" + RespPlan.QualifiedPassenger[rp].PassengerNumber.ToString();
                                    if (RespPlan.QualifiedPassenger.Where(x => x.PassengerNumber.ToString() == InfantId).FirstOrDefault() != null)
                                    {
                                        BK_RespInsure.Amount = Convert.ToDecimal(RespPlan.QualifiedPassenger[rp].PassengerPremiumAmount + RespPlan.QualifiedPassenger.Where(x => x.PassengerNumber.ToString() == InfantId).FirstOrDefault().PassengerPremiumAmount);
                                        //BK_RespInsure.Amount = Convert.ToDecimal(RespPlan.QualifiedPassenger[rp].PassengerPremiumAmount * 2);
                                    }
                                    else
                                    {
                                        BK_RespInsure.Amount = Convert.ToDecimal(RespPlan.QualifiedPassenger[rp].PassengerPremiumAmount);
                                    }
                                    listBK_RespInsure.Add(BK_RespInsure);
                                    SeqNo++;
                                }
                            }
                        }
                        lstPassenger.Clear();
                        lstFlight.Clear();
                    }
                }
                Session["RespInsure"] = listBK_RespInsure;
                dtInsure.Columns.Add("FeeCode");
                dtInsure.Columns.Add("Detail");
                dtInsure.Columns.Add("Price");
                dtInsure.Columns.Add("PriceS1");
                dtInsure.Columns.Add("PriceS2");
                dtInsure.Columns.Add("ConcatenatedField", typeof(string), "Detail");
                DataRow rowInsure = dtInsure.NewRow();
                DataTable dt = (DataTable)Application["dtArrayInsure"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (listBK_RespInsure != null)
                        {
                            string Feecode = listBK_RespInsure[0].FeeCode;
                            if (Feecode == dt.Rows[i]["ItemCode"].ToString())
                            {
                                dtInsure.Rows.Add(listBK_RespInsure[0].FeeCode, dt.Rows[i]["ItemDesc"], Convert.ToDecimal(listBK_RespInsure[0].Amount).ToString("N", nfi) + " " + Currency, listBK_RespInsure[0].Amount, Convert.ToDecimal("0.00"));
                                using (profiler.Step("GBS : InsureDefault 1"))
                                {
                                    InsureDefault(listBK_RespInsure[0].FeeCode, dt.Rows[i]["ItemDesc"].ToString(), listBK_RespInsure);
                                }
                            }
                        }
                        else if (RespPlan.SSRFeeCode == null && i == 0)
                        {
                            using (profiler.Step("GBS : InsureDefault 2 (if RespPlan.SSRFeeCode == null)"))
                            {
                                InsureDefault(null, null, null, dt);
                            }
                        }
                    }
                }

                DataView dvInsure = dtInsure.DefaultView;//added by romy, 20170811, insurance
                if (Flight == "Depart")
                {
                    cmbInsure.DataSource = dvInsure.ToTable(true, "FeeCode", "Detail", "Price", "PriceS1", "PriceS2", "ConcatenatedField");
                    cmbInsure.TextField = "ConcatenatedField";
                    cmbInsure.ValueField = "FeeCode";
                    cmbInsure.DataBind();
                    cmbInsure.NullText = msgList.Err200003;
                }
                Session["dtInsure" + Flight] = dtInsure;

                using (profiler.Step("GBS : GetAllBK_TRANSDTLFilterAll"))
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
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }

        }
        protected void Clearsession()
        {
            Session["IsNew"] = "True";
            Session["dtGridPass"] = null;
            //Session["dtGridPass2"] = null;
            Session["dtBaggageDepart"] = null;
            Session["dtBaggageReturn"] = null;
            Session["Currency"] = null;
            //Session["dtMealDepart"] = null;
            //Session["dtMealDepart2"] = null;
            //Session["dtDutyDepart"] = null;
            //Session["dtComfortDepart"] = null;
            //Session["dtMealReturn"] = null;
            //Session["dtMealReturn2"] = null;
            //Session["dtDutyReturn"] = null;
            //Session["dtComfortReturn"] = null;
            //Session["qtyMeal"] = null;
            //Session["qtyMeal1"] = null;
            //Session["qtyBaggage"] = null;
            //Session["qtySport"] = null;
            Session["qtyInsure"] = null;//added by romy, 20170811, insurance
            //Session["qtyComfort"] = null;
            //Session["qtyDuty"] = null;
            //Session["qtyMeal2"] = null;
            //Session["qtyMeal21"] = null;
            //Session["qtyBaggage2"] = null;
            //Session["qtySport2"] = null;
            //Session["qtyInsure2"] = null;//added by romy, 20170811, insurance
            //Session["qtyComfort2"] = null;
            //Session["qtyDuty2"] = null;
            Session["TotalAmountReturn"] = null;
            Session["TotalAmountDepart"] = null;
            //Session["transit"] = null;
            //Session["departTransit"] = null;
        }
        #endregion

        #region "Event"
        //public object ShowValue(object container, string expression)
        //{
        //    try
        //    {
        //        return DataBinder.Eval(container, expression);
        //    }
        //    catch (Exception ex)
        //    {
        //        SystemLog.Notifier.Notify(ex);
        //        //sTraceLog(ex.ToString);
        //        log.Error(this, ex);
        //    }
        //}
        public object ShowValue(object container, string expression)
        {
            try
            {
                return DataBinder.Eval(container, expression);
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
            return "";
        }
        protected void gvPassenger_DataBinding(object sender, EventArgs e)
        {
            var profiler = MiniProfiler.Current;
            using (profiler.Step("Binding"))
            {
                DataTable dtPass = new DataTable();
                if (Session["dtGridPass"] != null)
                {
                    dtPass = (DataTable)Session["dtGridPass"];
                }
                else
                {
                    //dtPass = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTableNewManage(TransID, false, "");
                    dtPass = objBooking.GetAllBK_PASSENGERLISTdataTable(TransID);
                }
                //if (Session["dtOldInsure"] != null && Cleared != false)
                //{
                //    dtPass = (DataTable)Session["dtOldInsure"];
                //    Session["dtOldInsure"] = null;
                //    Cleared = false;
                //    //dtPass = dtPass2Old;
                //    //dtPass2Old = null;
                //}
                if (Session["dtDefaultInsure"] != null)
                {
                    dtPass = (DataTable)Session["dtDefaultInsure"];
                }
                (sender as ASPxGridView).DataSource = dtPass;
                HttpContext.Current.Session["dtGridPass"] = dtPass;
            }
        }
        protected void SSRActionPanel_PerformCallback(object sender, CallbackEventArgsBase e)
        {
            //object TotalBaggage, TotalMeal, TotalMeal1, TotalSport, TotalInsure, TotalComfort, TotalDuty, TotalInfant;//edited by romy, 20170811, insurance
            //object TotalBaggage2, TotalMeal2, TotalMeal12, TotalSport2, TotalInsure2, TotalComfort2, TotalDuty2, TotalInfant2;//edited by romy, 20170811, insurance
            object TotalInsure;
            try
            {
                if (Session["dtGridPass"] != null)
                {
                    DataTable dtPass = (DataTable)Session["dtGridPass"];
                    //TotalBaggage = dtPass.Compute("Sum(PriceDepartBaggage)", "");
                    //if (TotalBaggage == DBNull.Value)
                    //{
                    //    TotalBaggage = 0;
                    //}
                    //TotalMeal = dtPass.Compute("Sum(PriceDepartMeal)", "");
                    //if (TotalMeal == DBNull.Value)
                    //{
                    //    TotalMeal = 0;
                    //}
                    //TotalMeal1 = dtPass.Compute("Sum(PriceConDepartMeal)", "");
                    //if (TotalMeal1 == DBNull.Value)
                    //{
                    //    TotalMeal1 = 0;
                    //}
                    //TotalSport = dtPass.Compute("Sum(PriceDepartSport)", "");
                    //if (TotalSport == DBNull.Value)
                    //{
                    //    TotalSport = 0;
                    //}
                    //TotalInsure = dtPass.Compute("Sum(InsureFee)", "");//added by romy, 20170811, insurance
                    TotalInsure = dtPass.Compute("Sum(InsureFee)", "");//added by romy, 20170811, insurance
                    if (TotalInsure == DBNull.Value)
                    {
                        TotalInsure = 0;
                    }
                    //TotalComfort = dtPass.Compute("Sum(PriceDepartComfort)", "");
                    //if (TotalComfort == DBNull.Value)
                    //{
                    //    TotalComfort = 0;
                    //}
                    //TotalInfant = dtPass.Compute("Sum(PriceDepartInfant)", "");
                    //if (TotalInfant == DBNull.Value)
                    //{
                    //    TotalInfant = 0;
                    //}
                    //if (Session["TotalInsure"] != null)//added by romy, 20170811, insurance
                    //{
                    //    lblTotalInsure.Text = (Convert.ToDecimal(TotalInsure) - Convert.ToDecimal(Session["TotalInsure"])).ToString("N", nfi);
                    //}
                    //else
                    //{
                    //    lblTotalInsure.Text = (Convert.ToDecimal(TotalInsure)).ToString("N", nfi);
                    //}
                    if (Session["dtGridPass"] != null)
                    {
                        //lblTotalAmount.Text = (Convert.ToDecimal(string.IsNullOrEmpty(lblTotalAmount.Text) ? "0" : lblTotalAmount.Text) + Convert.ToDecimal(string.IsNullOrEmpty(lblTotalInsure.Text) ? "0" : lblTotalInsure.Text)).ToString("N", nfi);//edited by romy, 20170811, insurance
                        lblTotalAmount.Text = (Convert.ToDecimal(string.IsNullOrEmpty(lblTotalAmount.Text) ? "0" : lblTotalAmount.Text) - DefFee + (Convert.ToDecimal(TotalInsure) - Convert.ToDecimal(Session["TotalInsure"]))).ToString("N", nfi);//edited by romy, 20170811, insurance
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
            //object TotalBaggage, TotalMeal, TotalMeal1, TotalSport, TotalInsure, TotalComfort, TotalDuty, TotalInfant;//edited by romy, 20170811, insurance
            object TotalInsure;
            try
            {
                if (Session["dtGridPass"] != null)
                {
                    DataTable dtPass = (DataTable)Session["dtGridPass"];
                    //TotalBaggage = dtPass.Compute("Sum(PriceDepartBaggage)", "");
                    //if (TotalBaggage == DBNull.Value)
                    //{
                    //    TotalBaggage = 0;
                    //}
                    //TotalMeal = dtPass.Compute("Sum(PriceDepartMeal)", "");
                    //if (TotalMeal == DBNull.Value)
                    //{
                    //    TotalMeal = 0;
                    //}
                    //TotalMeal1 = dtPass.Compute("Sum(PriceConDepartMeal)", "");
                    //if (TotalMeal1 == DBNull.Value)
                    //{
                    //    TotalMeal1 = 0;
                    //}
                    //TotalSport = dtPass.Compute("Sum(PriceDepartSport)", "");
                    //if (TotalSport == DBNull.Value)
                    //{
                    //    TotalSport = 0;
                    //}
                    //TotalInsure = dtPass.Compute("Sum(InsureFee)", "");//added by romy, 20170811, insurance
                    TotalInsure = dtPass.Compute("Sum(InsureFee)", "");//added by romy, 20170811, insurance
                    if (TotalInsure == DBNull.Value)
                    {
                        TotalInsure = 0;
                    }
                    //TotalComfort = dtPass.Compute("Sum(PriceDepartComfort)", "");
                    //if (TotalComfort == DBNull.Value)
                    //{
                    //    TotalComfort = 0;
                    //}
                    //TotalInfant = dtPass.Compute("Sum(PriceDepartInfant)", "");
                    //if (TotalInfant == DBNull.Value)
                    //{
                    //    TotalInfant = 0;
                    //}
                    //if (Session["TotalInsure"] != null)//added by romy, 20170811, insurance
                    //{
                    //    lblTotalInsure.Text = (Convert.ToDecimal(TotalInsure) - Convert.ToDecimal(Session["TotalInsure"])).ToString("N", nfi);
                    //}
                    //else
                    //{
                    //    lblTotalInsure.Text = (Convert.ToDecimal(TotalInsure)).ToString("N", nfi);
                    //}
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
        protected void BindLabel()
        {
            //object TotalBaggage, TotalMeal, TotalMeal1, TotalSport, TotalInsure, TotalComfort, TotalInfant;//edited by romy, 20170811, insurance
            //object TotalBaggage2, TotalMeal2, TotalMeal12, TotalSport2, TotalInsure2, TotalComfort2, TotalInfant2;//edited by romy, 20170811, insurance
            object TotalInsure;
            try
            {
                if (Session["dtGridPass"] != null)
                {
                    DataTable dtPass = (DataTable)Session["dtGridPass"];
                    //TotalBaggage = dtPass.Compute("Sum(PriceDepartBaggage)", "");
                    //if (TotalBaggage == DBNull.Value)
                    //{
                    //    TotalBaggage = 0;
                    //}
                    //TotalMeal = dtPass.Compute("Sum(PriceDepartMeal)", "");
                    //if (TotalMeal == DBNull.Value)
                    //{
                    //    TotalMeal = 0;
                    //}
                    //TotalMeal1 = dtPass.Compute("Sum(PriceConDepartMeal)", "");
                    //if (TotalMeal1 == DBNull.Value)
                    //{
                    //    TotalMeal1 = 0;
                    //}
                    //TotalSport = dtPass.Compute("Sum(PriceDepartSport)", "");
                    //if (TotalSport == DBNull.Value)
                    //{
                    //    TotalSport = 0;
                    //}
                    //TotalInsure = dtPass.Compute("Sum(InsureFee)", "");//added by romy, 20170811, insurance
                    TotalInsure = dtPass.Compute("Sum(InsureFee)", "");//added by romy, 20170811, insurance
                    if (TotalInsure == DBNull.Value)
                    {
                        TotalInsure = 0;
                    }
                    //TotalComfort = dtPass.Compute("Sum(PriceDepartComfort)", "");
                    //if (TotalComfort == DBNull.Value)
                    //{
                    //    TotalComfort = 0;
                    //}
                    //TotalInfant = dtPass.Compute("Sum(PriceDepartInfant)", "");
                    //if (TotalInfant == DBNull.Value)
                    //{
                    //    TotalInfant = 0;
                    //}

                    Decimal TotalAmountDepart = Convert.ToDecimal(TotalInsure);//edited by romy, 20170811, insurance

                    //if (Session["TotalBaggage"] == null)
                    //{
                    //    Session["TotalBaggage"] = TotalBaggage;
                    //}

                    //if (Session["TotalMeal"] == null)
                    //{
                    //    Session["TotalMeal"] = TotalMeal;
                    //}

                    //if (Session["TotalMeal1"] == null)
                    //{
                    //    Session["TotalMeal1"] = TotalMeal1;
                    //}

                    //if (Session["TotalSport"] == null)
                    //{
                    //    Session["TotalSport"] = TotalSport;
                    //}

                    if (Session["TotalInsure"] == null)//added by romy, 20170811, insurance
                    {
                        Session["TotalInsure"] = TotalInsure;
                    }

                    //if (Session["TotalComfort"] == null)
                    //{
                    //    Session["TotalComfort"] = TotalComfort;
                    //}

                    //if (Session["TotalInfant"] == null)
                    //{
                    //    Session["TotalInfant"] = TotalInfant;
                    //}

                    if (Session["TotalAmountDepart"] == null)
                    {
                        Session["TotalAmountDepart"] = TotalAmountDepart;
                    }

                    if (Session["dtGridPass"] != null)
                    {
                        //lblTotalInsure.Text = (Convert.ToDecimal(0)).ToString("N", nfi);//added by romy, 20170811, insurance
                        lblTotalAmount.Text = (Convert.ToDecimal(0)).ToString("N", nfi);
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
        protected void gvPassenger_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex < 0)
            {
                return;
            }

            if (e.ButtonID == "ClearInsure")
            {
                if (((ASPxGridView)sender).GetRowValues(e.VisibleIndex, "Status").ToString() == "Not Eligible")
                    e.Visible = DefaultBoolean.False;
                else if (((ASPxGridView)sender).GetRowValues(e.VisibleIndex, "Status").ToString() == "Insured")
                    e.Visible = DefaultBoolean.False;
            }

            //Object Infantcolumn = gvPassenger.GetRowValues(e.VisibleIndex, "DepartInfant");
            Object PassengerIDcolumn = gvPassenger.GetRowValues(e.VisibleIndex, "PassengerID");
            Object SeqNocolumn = gvPassenger.GetRowValues(e.VisibleIndex, "SeqNo");
            Object PNRcolumn = gvPassenger.GetRowValues(e.VisibleIndex, "RecordLocator");

            //if (Session["dtInfant"] != null)
            //{
            //    DataTable dtInfant = (DataTable)Session["dtInfant"];
            //    if (e.ButtonID == "btnDetails")
            //    {
            //        if (dtInfant.Rows.Count > 0)
            //        {
            //            for (int i = 0; i < dtInfant.Rows.Count; i++)
            //            {
            //                if (Convert.ToInt16(PassengerIDcolumn).ToString().IndexOf(dtInfant.Rows[i]["PassengerID"].ToString()) >= 0 && PNRcolumn.ToString().IndexOf(dtInfant.Rows[i]["RecordLocator"].ToString()) >= 0)
            //                {
            //                    if (Infantcolumn.ToString() != "" && dtInfant.Rows[i]["FirstName"].ToString() == "Infant" && dtInfant.Rows[i]["LastName"].ToString() == "Infant")
            //                    {
            //                        e.Visible = DefaultBoolean.True;

            //                    }
            //                    else
            //                    {
            //                        e.Visible = DefaultBoolean.False;

            //                    }
            //                    break;
            //                }
            //                else
            //                {
            //                    if (Infantcolumn.ToString() != "")
            //                    {
            //                        e.Visible = DefaultBoolean.True;
            //                    }
            //                    else
            //                    {
            //                        e.Visible = DefaultBoolean.False;
            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {
            //            e.Visible = DefaultBoolean.False;
            //        }
            //    }
            //}
            //else
            //{
            //    if (Infantcolumn.ToString() != "")
            //    {
            //        e.Visible = DefaultBoolean.True;
            //    }
            //    else
            //    {
            //        e.Visible = DefaultBoolean.False;
            //    }
            //}

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
                            txtDOB.MaxDate = bookHDRInfo.STDDate.AddDays(-9);
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
                        txtDOB.MaxDate = bookHDRInfo.STDDate.AddDays(-9);
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
                                PassData.RecordLocator = dataPassenger.Rows[Convert.ToInt16(index) - 1]["RecordLocator"].ToString();
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
                            PassData2.RecordLocator = dataPassenger.Rows[Convert.ToInt16(index) - 1]["RecordLocator"].ToString();
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
                        PassData2.RecordLocator = dataPassenger.Rows[Convert.ToInt16(index) - 1]["RecordLocator"].ToString();
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
                string strHTML = "";
                BaseResource SAOTune = new BaseResource();

                int promt = 0, j = 0;
                object[] findTheseVals = new object[2];
                DataTable dataResp = new DataTable();
                if (Session["RespInsure"] != null)
                {
                    //datax=(DataTable)Session["RespInsure"];
                    dataResp = GetPlan.ConvertToDataTable((List<InsureResponse>)Session["RespInsure"]);
                    dataResp.PrimaryKey = new DataColumn[] { (dataResp.Columns["PNR"]), (dataResp.Columns["PassengerNumber"]) };
                }

                String Detail = "";
                if (Session["qtyInsure"] == null)//added by romy, 20170811, insurance
                {
                    qtyInsure = 0;
                }
                else
                {
                    qtyInsure = Convert.ToInt32(Session["qtyInsure"]);
                }

                //added by romy for Double Premium Amount
                DataTable dtInfant = new DataTable();
                //DataTable dtSysPrefttry = new DataTable();
                //dtSysPrefttry = (DataTable)Session["dtSysPrefttry"];
                dtInfant = (DataTable)Session["InfantCheck"];

                DataTable dtInsured = new DataTable();
                dtInsured = (DataTable)Session["dtOldInsure"];
                DataTable dtPass = new DataTable();
                dtPass = (DataTable)Session["dtGridPass"];
                if (string.IsNullOrEmpty(e.Parameters))
                {
                    GetPassengerList(Session["TransID"].ToString(), "Depart");

                }
                else
                {
                    var args = e.Parameters.Split('|');
                    if (args[0] == "Insure")//added by romy, 20170811, insurance
                    {
                        DataTable dtInsure = (DataTable)Session["dtInsureDepart"];
                        if (cbAllPaxInsure1.Checked == true)
                        {
                            //int AbleInsure = dtPass.Rows.Count - 1;
                            for (int i = 0; i <= dtPass.Rows.Count - 1; i++)
                            {
                                if (args[2].ToString() == "")
                                {
                                    gvPassenger.JSProperties["cp_result"] = msgList.Err200021;
                                    return;
                                }
                                else
                                {
                                    if (dtInsured.Rows[i]["InsureCode"] == string.Empty)
                                    {
                                        if (dtPass.Rows[i]["InsureCode"] != null && dtPass.Rows[i]["InsureCode"].ToString() != "")
                                        {
                                            DataRow[] result = dtInsure.Select("FeeCode = '" + args[3] + "'");
                                            findTheseVals[0] = (dtPass.Rows[i]["RecordLocator"]);
                                            findTheseVals[1] = (dtPass.Rows[i]["PassengerID"]);
                                            DataRow rowResp = dataResp.NewRow();
                                            if (dataResp != null && dataResp.Rows.Count > 0)
                                            {
                                                rowResp = dataResp.Rows.Find(findTheseVals);
                                            }
                                            if (rowResp != null)
                                            {
                                                foreach (DataRow row in result)
                                                {
                                                    Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                    if (Convert.ToDecimal(rowResp[5]) < Convert.ToDecimal(dtPass.Rows[i]["InsureFee"]))
                                                    {
                                                        gvPassenger.JSProperties["cp_result"] = msgList.Err200023;
                                                        return;
                                                    }
                                                    else
                                                    {
                                                        promt++;
                                                        //dtPass.Rows[i]["DepartInsure"] = args[2];
                                                        dtPass.Rows[i]["InsureCode"] = args[3];
                                                        //added by romy for Double Premium Amount
                                                        //if (dtSysPrefttry != null && Session["DepartRoute"] != null && dtSysPrefttry.Rows[0]["SYSValueEx"].ToString() == Session["DepartRoute"].ToString() && dtInfant.Rows[i]["DepartInfant"].ToString() != "")
                                                        //if (rowResp[4].ToString() == "True" && dtInfant.Rows[i]["DepartInfant"].ToString() != "")
                                                        //{
                                                        //    dtPass.Rows[i]["InsureFee"] = Math.Round(Convert.ToDecimal(Detail) * 2, 2);
                                                        //}
                                                        //else {
                                                        //    dtPass.Rows[i]["InsureFee"] = Math.Round(Convert.ToDecimal(Detail), 2);
                                                        //}
                                                        dtPass.Rows[i]["InsureFee"] = string.Format("{0:0.00}", rowResp[5]);//string.Format("{0:0.00}", rowResp[5]);
                                                        dtPass.Rows[i]["Status"] = "Added Insurance";
                                                        dtPass.Rows[i]["InsureFee1"] = rowResp[5] == null ? "" : string.Format("{0:0.00}", rowResp[5]).ToString();
                                                        //dtPass.Rows[i]["PriceDepartInsure1"] = Convert.ToDecimal(row[3]);
                                                        //dtPass.Rows[i]["PriceConDepartInsure"] = Convert.ToDecimal(row[4]);
                                                        //dtPass.Rows[i]["IndicatorDepartInsure"] = "1";
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            findTheseVals[0] = (dtPass.Rows[i]["RecordLocator"]);
                                            findTheseVals[1] = (dtPass.Rows[i]["PassengerID"]);

                                            DataRow rowResp = dataResp.NewRow();
                                            if (dataResp != null && dataResp.Rows.Count > 0)
                                            {
                                                rowResp = dataResp.Rows.Find(findTheseVals);
                                            }
                                            if (rowResp != null)
                                            {
                                                //dtPass.Rows[i]["DepartInsure"] = args[2];
                                                dtPass.Rows[i]["InsureCode"] = args[3];
                                                //dtPass.Rows[i]["IndicatorDepartInsure"] = "1";
                                                DataRow[] result = dtInsure.Select("FeeCode = '" + args[3] + "'");
                                                foreach (DataRow row in result)
                                                {
                                                    Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                    //added by romy for Double Premium Amount
                                                    //if (dtSysPrefttry != null && Session["DepartRoute"] != null && dtSysPrefttry.Rows[0]["SYSValueEx"].ToString() == Session["DepartRoute"].ToString() && dtInfant.Rows[i]["DepartInfant"].ToString() != "")
                                                    //if (rowResp[4].ToString() == "True" && dtInfant.Rows[i]["DepartInfant"].ToString() != "")
                                                    //{
                                                    //    dtPass.Rows[i]["InsureFee"] = Math.Round(Convert.ToDecimal(Detail) * 2, 2);
                                                    //}
                                                    //else {
                                                    //    dtPass.Rows[i]["InsureFee"] = Math.Round(Convert.ToDecimal(Detail), 2);
                                                    //}
                                                    dtPass.Rows[i]["InsureFee"] = string.Format("{0:0.00}", rowResp[5]);
                                                    dtPass.Rows[i]["Status"] = "Added Insurance";
                                                    dtPass.Rows[i]["InsureFee1"] = rowResp[5] == null ? "" : string.Format("{0:0.00}", rowResp[5]).ToString();
                                                    //dtPass.Rows[i]["InsureFee"] = Math.Round(Convert.ToDecimal(Detail), 2);
                                                    //dtPass.Rows[i]["PriceDepartInsure1"] = Convert.ToDecimal(row[3]);
                                                    //dtPass.Rows[i]["PriceConDepartInsure"] = Convert.ToDecimal(row[4]);

                                                }
                                            }
                                            else
                                            {
                                                dtPass.Rows[i]["InsureCode"] = msgList.Err200000;
                                                dtPass.Rows[i]["Status"] = msgList.Err200000;
                                                dtPass.Rows[i]["IndicatorClear"] = 1;
                                                //AbleInsure++;
                                            }
                                        }

                                    }
                                }
                            }
                            if (dtPass.Rows.Count == promt)
                            {
                                gvPassenger.JSProperties["cp_result"] = msgList.Err200025;
                            }
                            promt = 0;
                            Session["dtGridPass"] = dtPass;
                            Session["qtyInsure"] = 0;
                            gvPassenger.DataSource = dtPass;
                            gvPassenger.DataBind();
                        }
                        else
                        {
                            if ((Convert.ToInt32(args[1]) + qtyInsure) <= dtPass.Rows.Count)
                            {
                                for (int i = 0; i <= dtPass.Rows.Count - 1; i++)
                                {
                                    if (args[2].ToString() == "")
                                    {
                                        gvPassenger.JSProperties["cp_result"] = msgList.Err200021;
                                        return;
                                    }
                                    else
                                    {
                                        if ((dtPass.Rows[i]["InsureCode"] == null || dtPass.Rows[i]["InsureCode"].ToString() == "") && j < (Convert.ToInt32(args[1])))
                                        {
                                            findTheseVals[0] = (dtPass.Rows[i]["RecordLocator"]);
                                            findTheseVals[1] = (dtPass.Rows[i]["PassengerID"]);

                                            DataRow rowResp = dataResp.NewRow();
                                            if (dataResp != null && dataResp.Rows.Count > 0)
                                            {
                                                rowResp = dataResp.Rows.Find(findTheseVals);
                                            }
                                            if (rowResp != null)
                                            {
                                                //dtPass.Rows[i]["DepartInsure"] = args[2];
                                                dtPass.Rows[i]["InsureCode"] = args[3];
                                                //dtPass.Rows[i]["IndicatorDepartInsure"] = "1";
                                                DataRow[] result = dtInsure.Select("FeeCode = '" + args[3] + "'");
                                                foreach (DataRow row in result)
                                                {
                                                    Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                                                    //added by romy for Double Premium Amount
                                                    //if (dtSysPrefttry != null && Session["DepartRoute"] != null && dtSysPrefttry.Rows[0]["SYSValueEx"].ToString() == Session["DepartRoute"].ToString() && dtInfant.Rows[i]["DepartInfant"].ToString() != "")
                                                    //if (rowResp[4].ToString() == "True" && dtInfant.Rows[i]["DepartInfant"].ToString() != "")
                                                    //{
                                                    //    dtPass.Rows[i]["InsureFee"] = Math.Round(Convert.ToDecimal(Detail) * 2, 2);
                                                    //}
                                                    //else {
                                                    //    dtPass.Rows[i]["InsureFee"] = Math.Round(Convert.ToDecimal(Detail), 2);
                                                    //}
                                                    dtPass.Rows[i]["InsureFee"] = string.Format("{0:0.00}", rowResp[5]);
                                                    dtPass.Rows[i]["Status"] = "Added Insurance";
                                                    dtPass.Rows[i]["InsureFee1"] = rowResp[5] == null ? "" : string.Format("{0:0.00}", rowResp[5]).ToString();
                                                }
                                                j++;
                                            }
                                            else
                                            {
                                                dtPass.Rows[i]["InsureCode"] = msgList.Err200000;
                                                dtPass.Rows[i]["Status"] = msgList.Err200000;
                                                dtPass.Rows[i]["IndicatorClear"] = 1;
                                            }
                                            //j++;
                                        }
                                    }
                                }
                                Session["dtGridPass"] = dtPass;
                                Session["qtyInsure"] = (Convert.ToInt32(args[1]) + qtyInsure);
                                gvPassenger.DataSource = dtPass;
                                gvPassenger.DataBind();
                            }
                            else
                            {
                                gvPassenger.JSProperties["cp_result"] = msgList.Err200024;
                                return;
                            }
                            //if (((Convert.ToInt32(args[1]) + qtyInsure) - 1) <= dtPass.Rows.Count)
                            //{
                            //    for (int i = qtyInsure; i <= (Convert.ToInt32(args[1]) + qtyInsure) - 1; i++)
                            //    {
                            //        if (args[2].ToString() == "")
                            //        {
                            //            gvPassenger.JSProperties["cp_result"] = msgList.Err200021;
                            //            return;
                            //        }
                            //        else
                            //        {
                            //            if (dtPass.Rows[i]["InsureCode"] != null && dtPass.Rows[i]["InsureCode"].ToString() != "")
                            //            {
                            //                DataRow[] result = dtInsure.Select("FeeCode = '" + args[3] + "'");
                            //                foreach (DataRow row in result)
                            //                {
                            //                    Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                            //                    if (Convert.ToDecimal(Detail) < Convert.ToDecimal(dtPass.Rows[i]["InsureFee"]))
                            //                    {
                            //                        gvPassenger.JSProperties["cp_result"] = msgList.Err200023;
                            //                        return;
                            //                    }
                            //                    else
                            //                    {
                            //                        findTheseVals[0] = (dtPass.Rows[i]["RecordLocator"]);
                            //                        findTheseVals[1] = (dtPass.Rows[i]["PassengerID"]);
                            //                        DataRow rowResp = dataResp.NewRow();
                            //                        if (dataResp != null && dataResp.Rows.Count > 0)
                            //                        {
                            //                            rowResp = dataResp.Rows.Find(findTheseVals);
                            //                        }
                            //                        if (rowResp != null)
                            //                        {
                            //                            //dtPass.Rows[i]["DepartInsure"] = args[2];
                            //                            dtPass.Rows[i]["InsureCode"] = args[3];
                            //                            //added by romy for Double Premium Amount
                            //                            //if (dtSysPrefttry != null && Session["DepartRoute"] != null && dtSysPrefttry.Rows[0]["SYSValueEx"].ToString() == Session["DepartRoute"].ToString() && dtInfant.Rows[i]["DepartInfant"].ToString() != "")
                            //                            if (rowResp[4].ToString() == "True" && dtInfant.Rows[i]["DepartInfant"].ToString() != "")
                            //                            {
                            //                                dtPass.Rows[i]["InsureFee"] = Math.Round(Convert.ToDecimal(Detail) * 2, 2);
                            //                            }
                            //                            else {
                            //                                dtPass.Rows[i]["InsureFee"] = Math.Round(Convert.ToDecimal(Detail), 2);
                            //                            }
                            //                            //dtPass.Rows[i]["PriceDepartInsure1"] = Convert.ToDecimal(row[3]);
                            //                            //dtPass.Rows[i]["PriceConDepartInsure"] = Convert.ToDecimal(row[4]);
                            //                            //dtPass.Rows[i]["IndicatorDepartInsure"] = "1";
                            //                        }
                            //                    }

                            //                }
                            //            }
                            //            else
                            //            {
                            //                findTheseVals[0] = (dtPass.Rows[i]["RecordLocator"]);
                            //                findTheseVals[1] = (dtPass.Rows[i]["PassengerID"]);

                            //                DataRow rowResp = dataResp.NewRow();
                            //                if (dataResp != null && dataResp.Rows.Count > 0)
                            //                {
                            //                    rowResp = dataResp.Rows.Find(findTheseVals);
                            //                }
                            //                if (rowResp != null)
                            //                {
                            //                    //dtPass.Rows[i]["DepartInsure"] = args[2];
                            //                    dtPass.Rows[i]["InsureCode"] = args[3];
                            //                    //dtPass.Rows[i]["IndicatorDepartInsure"] = "1";
                            //                    DataRow[] result = dtInsure.Select("FeeCode = '" + args[3] + "'");
                            //                    foreach (DataRow row in result)
                            //                    {
                            //                        Detail = row[2].ToString().Substring(0, row[2].ToString().Length - 4);
                            //                        //added by romy for Double Premium Amount
                            //                        //if (dtSysPrefttry != null && Session["DepartRoute"] != null && dtSysPrefttry.Rows[0]["SYSValueEx"].ToString() == Session["DepartRoute"].ToString() && dtInfant.Rows[i]["DepartInfant"].ToString() != "")
                            //                        if (rowResp[4].ToString() == "True" && dtInfant.Rows[i]["DepartInfant"].ToString() != "")
                            //                        {
                            //                            dtPass.Rows[i]["InsureFee"] = Math.Round(Convert.ToDecimal(Detail) * 2, 2);
                            //                        }
                            //                        else {
                            //                            dtPass.Rows[i]["InsureFee"] = Math.Round(Convert.ToDecimal(Detail), 2);
                            //                        }
                            //                        //dtPass.Rows[i]["InsureFee"] = Math.Round(Convert.ToDecimal(Detail), 2);
                            //                        //dtPass.Rows[i]["PriceDepartInsure1"] = Convert.ToDecimal(row[3]);
                            //                        //dtPass.Rows[i]["PriceConDepartInsure"] = Convert.ToDecimal(row[4]);

                            //                    }
                            //                } else {
                            //                    dtPass.Rows[i]["InsureCode"]= msgList.Err200000;
                            //                }
                            //            }
                            //        }
                            //    }
                            //    Session["dtGridPass"] = dtPass;
                            //    Session["qtyInsure"] = (Convert.ToInt32(args[1]) + qtyInsure);
                            //    gvPassenger.DataSource = dtPass;
                            //    gvPassenger.DataBind();
                            //}
                            //else
                            //{
                            //    gvPassenger.JSProperties["cp_result"] = msgList.Err200024;
                            //    return;
                            //}
                        }
                    }
                    else if (args[0] == "Clear")
                    {
                        //dtPass.Rows[Convert.ToInt16(args[1])]["DepartInsure"] = string.Empty;//added by romy, 20170811, insurance
                        if (dtInsured.Rows[Convert.ToInt16(args[1])]["InsureCode"] != string.Empty)
                        {
                            gvPassenger.JSProperties["cp_result"] = msgList.Err200022;
                        }
                        else
                        {
                            dtPass.Rows[Convert.ToInt16(args[1])]["Status"] = string.Empty;
                            dtPass.Rows[Convert.ToInt16(args[1])]["InsureCode"] = string.Empty;
                            dtPass.Rows[Convert.ToInt16(args[1])]["InsureFee"] = 0.00;
                            dtPass.Rows[Convert.ToInt16(args[1])]["InsureFee1"] = string.Empty;
                        }

                        //Session["dtDefaultInsure"] = null;
                        //DataTable dtInsured = new DataTable();
                        //dtInsured = (DataTable)Session["dtOldInsure"];
                        //lblTotalAmount.Text = (Convert.ToDecimal(0) + (Convert.ToDecimal(dtPass.Compute("Sum(InsureFee)", "")) - Convert.ToDecimal(dtInsured.Compute("Sum(InsureFee)", "")))).ToString("N", nfi);
                        //BindLabel();
                        //Cleared = true;
                        Session["qtyInsure"] = (qtyInsure - 1);
                        Session["dtGridPass"] = dtPass;
                        gvPassenger.DataSource = dtPass; //(DataTable)Session["dtOldInsure"];
                        gvPassenger.DataBind();
                        //Session["qtyInsure"] = Convert.ToInt32(Session["qtyInsure"]) - 1;
                    }
                    else if (args[0] == "ClearAll")
                    {
                        for (int i = 0; i < dtInsured.Rows.Count; i++)
                        {
                            if (dtInsured.Rows[i]["InsureCode"] == string.Empty && dtPass.Rows[i]["IndicatorClear"].ToString() != "1")
                            {
                                dtPass.Rows[i]["Status"] = string.Empty;
                                dtPass.Rows[i]["InsureCode"] = string.Empty;
                                dtPass.Rows[i]["InsureFee"] = 0.00;
                                dtPass.Rows[i]["InsureFee1"] = string.Empty;
                            }
                        }
                        cbAllPaxInsure1.Checked = false;
                        Session["qtyInsure"] = 0;
                        Session["dtGridPass"] = dtPass;
                        gvPassenger.DataSource = dtPass;
                        gvPassenger.DataBind();
                    }
                    //else if (args[0] == "SAO")
                    //{
                    //    divSAO.InnerHtml = "";
                    //    SAOTune = objGeneral.LoadSAOXML(Request.PhysicalApplicationPath, args[3].ToString());
                    //    if (SAOTune != null)
                    //    {
                    //        foreach (Insurance Sao in SAOTune.Insurances[0].Insurance)
                    //        {
                    //            if (Sao.CultureCode == "en-GB")
                    //            {
                    //                if (IsInternationalFlight)
                    //                {
                    //                    strHTML += "<span class='ti-check-box'></span> " + Sao.Itnl[0].CheckItems[0].CheckItem[0].ToString() + "</br>";
                    //                    strHTML += "<span class='ti-check-box'></span> " + Sao.Itnl[0].CheckItems[0].CheckItem[1].ToString() + "</br>";
                    //                    strHTML += "<span class='ti-check-box'></span> " + Sao.Itnl[0].CheckItems[0].CheckItem[2].ToString() + "</br>";
                    //                    strHTML += "<span class='ti-check-box'></span> " + Sao.Itnl[0].CheckItems[0].CheckItem[3].ToString() + "</br>";
                    //                    strHTML += "<img src = '" + Sao.Itnl[0].UpsellUrl.ToString() + "' class='avatar mCS_img_loaded' data-pin-nopin='true'></br>";
                    //                    strHTML += Sao.Itnl[0].ConfirmContent.ToString();
                    //                }
                    //                else
                    //                {
                    //                    strHTML += "<span class='ti-check-box'></span> " + Sao.Dom[0].CheckItems[0].CheckItem[0].ToString() + "</br>";
                    //                    strHTML += "<span class='ti-check-box'></span> " + Sao.Dom[0].CheckItems[0].CheckItem[1].ToString() + "</br>";
                    //                    strHTML += "<span class='ti-check-box'></span> " + Sao.Dom[0].CheckItems[0].CheckItem[2].ToString() + "</br>";
                    //                    strHTML += "<span class='ti-check-box'></span> " + Sao.Dom[0].CheckItems[0].CheckItem[3].ToString() + "</br>";
                    //                    strHTML += "<img src = '" + Sao.Dom[0].UpsellUrl.ToString() + "' class='avatar mCS_img_loaded' data-pin-nopin='true'></br>";
                    //                    strHTML += Sao.Dom[0].ConfirmContent.ToString();
                    //                }
                    //            }
                    //        }
                    //    }
                    //    divSAO.InnerHtml = strHTML;
                    //}

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

        protected void gvPassenger_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {

                if (e.DataColumn.FieldName == "InsureCode")
                {
                    if (e.CellValue.ToString() == "INSA" || e.CellValue.ToString() == "INSB" || e.CellValue.ToString() == "INSC")
                    {
                        e.Cell.CssClass = "Travel";
                    }
                }

                if (e.DataColumn.FieldName == "Status")
                {
                    if (e.CellValue.ToString() == "Added Insurance")
                    {
                        e.Cell.CssClass = "AbleInsure";
                    }
                    else if (e.CellValue.ToString() == "Not Eligible")
                    {
                        e.Cell.CssClass = "NoInsure";
                    }
                    else if (e.CellValue.ToString() == "Insured")
                    {
                        e.Cell.CssClass = "Insured";
                    }
                }

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
            //string[] category = new string[] { "Baggage", "Sport", "Insure", "Drink", "Infant" };//edited by romy, 20170811, insurance
            GridViewDataComboBoxColumn column = new GridViewDataComboBoxColumn();
            //for (int p = 0; p < category.Length; p++)
            //{

            //if (flight == "Depart")
            //{
            //    column = (gvPassenger.Columns[flight + "Insure"] as GridViewDataComboBoxColumn);
            //}
            column = (gvPassenger.Columns["InsureCode"] as GridViewDataComboBoxColumn);
            DataTable dtBaggage = null;
            dtBaggage = (DataTable)Session["dt" + "Insure" + flight];
            if (dtBaggage != null)
            {
                //for (int p = 0;p<dtBaggage.Rows.Count;p++)
                //{
                DataRow[] result = dtBaggage.Select("FeeCode = ''");
                column.PropertiesComboBox.DataSource = dtBaggage;
                column.PropertiesComboBox.ValueField = "FeeCode";
                column.PropertiesComboBox.TextField = "ConcatenatedField";
                //}
            }
            //}

            if (flight == "Depart")
            {
                gvPassenger.DataBind();
            }
        }
        protected string GetSellSSR(string signature, string TransID)
        {
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;

            List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
            ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking(signature);
            ABS.Navitaire.APIBooking apiBooking2 = new ABS.Navitaire.APIBooking("");
            BookingControl bookingControl = new BookingControl();

            //added by romy for Double Premium Amount
            DataTable dtSysPrefttry = new DataTable();
            using (profiler.Step("GBS : GetSysPreftByPromoCode"))
            {
                dtSysPrefttry = objBooking.GetSysPreftByPromoCode("PREMIUMINSURE");
            }
            if (dtSysPrefttry != null && dtSysPrefttry.Rows[0]["SYSset"].ToString() == "1" && dtSysPrefttry.Rows.Count > 0)
            {
                Session["dtSysPrefttry"] = dtSysPrefttry;
            }
            else
            {
                Session["dtSysPrefttry"] = null;
            }

            using (profiler.Step("Navitaire : AgentLogon"))
            {
                SellSessionID = apiBooking.AgentLogon();
            }
            using (profiler.Step("GBS : lstbookDTLInfo"))
            {
                lstbookDTLInfo = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 1, "LEN(RecordLocator) >= 6 AND ");
            }
            int PaxAdult = lstbookDTLInfo.Sum(item => item.PaxAdult);
            int PaxChild = lstbookDTLInfo.Sum(item => item.PaxChild);
            int PaxNum = PaxAdult + PaxChild;
            GetBookingResponse resp;
            using (profiler.Step("Navitaire : GetBookingByPNR"))
            {
                resp = bookingControl.GetBookingByPNR(lstbookDTLInfo[0].RecordLocator, SellSessionID);
            }
            Session["resp"] = resp;
            ABS.Navitaire.BookingManager.Booking responseBookingFromState;// = bookingControl.GetBookingFromState(SellSessionID);
            using (profiler.Step("Navitaire : responseBookingFromState"))
            {
                //responseBookingFromState = bookingControl.GetBookingFromState(SellSessionID);
                responseBookingFromState = apiBooking2.GetBookingFromState(SellSessionID, 2);
                //string xml = GetXMLString(responseBookingFromState);
            }
            if (resp != null)
            {
                GetSSRAvailabilityForBookingResponse response;
                using (profiler.Step("Navitaire : GetSSRAvailabilityForBooking_Page"))
                {
                    response = apiBooking.GetSSRAvailabilityForBooking(responseBookingFromState, SellSessionID);
                }
                if (response != null)
                {
                    ArrayList InsureCode = new ArrayList();//added by romy, 20170811, insurance
                    ArrayList InsureFee = new ArrayList();
                    ArrayList InsureFeeS1 = new ArrayList();
                    ArrayList InsureFeeS2 = new ArrayList();
                    List<BookingTransactionDetail> lstTransDetail = new List<BookingTransactionDetail>();

                    Session["Currency"] = resp.Booking.CurrencyCode;
                    using (profiler.Step("GBS : GetAllBK_TRANSDTLFilterAll"))
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
                        else
                        {
                            break;
                        }
                    }

                    SSRAvailabilityForBookingResponse ssrAvailabilityResponseForBooking = response.SSRAvailabilityForBookingResponse;
                    Session["GetssrAvailabilityResponseForBooking"] = response;
                    DataTable dtdefaultBundleInsure = new DataTable();//added by romy, 20170811, insurance

                    if (ssrAvailabilityResponseForBooking != null && ssrAvailabilityResponseForBooking.SSRSegmentList.Length != 0)
                    {
                        dtdefaultBundleInsure = (DataTable)Application["dtArrayInsure"];//added by romy, 20170811, insurance

                        List<string> lstInsure = dtdefaultBundleInsure.AsEnumerable().Select(r => r.Field<string>("ItemCode")).ToList();

                        foreach (ABS.Navitaire.BookingManager.SSRSegment SSRSegment in ssrAvailabilityResponseForBooking.SSRSegmentList)
                        {
                            if ((SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == return1) || (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == transit1) || (SSRSegment.LegKey.DepartureStation == transit1 && SSRSegment.LegKey.ArrivalStation == return1))
                            {
                                //depart here 
                                //insure
                                List<AvailablePaxSSR> paxInsure = SSRSegment.AvailablePaxSSRList.Where(x => lstInsure.Contains(x.SSRCode)).ToList();
                                if (paxInsure != null && paxInsure.Count > 0)
                                {
                                    int index = paxInsure.Where(x => InsureCode.Contains(x.SSRCode)).Count();
                                    if (index <= 0)
                                    {
                                        InsureCode.AddRange(paxInsure.Select(x => x.SSRCode).ToList());
                                    }
                                    //InsureFee.AddRange(paxInsure.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
                                    if ((SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == return1) || (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == transit1))
                                    {
                                        InsureFeeS1.AddRange(paxInsure.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
                                    }
                                    else if ((SSRSegment.LegKey.DepartureStation == transit1 && SSRSegment.LegKey.ArrivalStation == return1))
                                    {
                                        InsureFeeS2.AddRange(paxInsure.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
                                    }
                                }
                            }
                            //Insure
                            if (InsureFeeS2.Count > 0)
                            {
                                InsureFee.AddRange(InsureFeeS1.ToArray().Zip(InsureFeeS2.ToArray(), (x, y) => Convert.ToDecimal(x) + Convert.ToDecimal(y)).ToList());
                            }
                            else
                            {
                                InsureFee = InsureFeeS1;
                            }
                        }
                        GetSSRAvailabilityForBookingResponse responses;
                        using (profiler.Step("GetSSRAvailabilityForBooking"))
                        {
                            responses = apiBooking.GetSSRAvailabilityForBooking(responseBookingFromState, SellSessionID);
                            //string xmlresponses = GetXMLString(responses);
                        }
                        if (responses != null)
                        {
                            SSRAvailabilityForBookingResponse ssrAvailabilityResponseForBookings = responses.SSRAvailabilityForBookingResponse;
                            Session["GetssrAvailabilityResponseForBooking"] = responses;

                            //if (ssrAvailabilityResponseForBookings != null && ssrAvailabilityResponseForBookings.SSRSegmentList.Length != 0)
                            //{
                            //    DataTable dtdefaultBundleDrink = (DataTable)Session["dtArrayDrink"];
                            //    List<string> lstDrink = dtdefaultBundleDrink.AsEnumerable().Select(r => r.Field<string>("ItemCode")).ToList();
                            //    foreach (ABS.Navitaire.BookingManager.SSRSegment SSRSegment in ssrAvailabilityResponseForBookings.SSRSegmentList)
                            //    {
                            //        //Drink
                            //        List<AvailablePaxSSR> paxDrink = SSRSegment.AvailablePaxSSRList.Where(x => lstDrink.Contains(x.SSRCode)).ToList();
                            //        if (paxDrink != null && paxDrink.Count > 0)
                            //        {
                            //            if ((SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == return1) || (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == transit1) || (SSRSegment.LegKey.DepartureStation == transit1 && SSRSegment.LegKey.ArrivalStation == return1))
                            //            {
                            //                //depart here 
                            //                if ((SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == return1) || (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == transit1))
                            //                {
                            //                    //DrinkCode.AddRange(paxDrink.Select(x => x.SSRCode).ToList());
                            //                    //DrinkFee.AddRange(paxDrink.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
                            //                }
                            //                else if ((SSRSegment.LegKey.DepartureStation == transit1 && SSRSegment.LegKey.ArrivalStation == return1))
                            //                {
                            //                    //DrinkCode1.AddRange(paxDrink.Select(x => x.SSRCode).ToList());
                            //                    //DrinkFee1.AddRange(paxDrink.Select(x => x.PaxSSRPriceList[0].PaxFee.ServiceCharges.Where(item => item.ChargeType == ChargeType.ServiceCharge).Select(y => y.Amount).Sum()).ToList());
                            //                }
                            //            }
                            //        }
                            //    }
                            //}

                            foreach (ABS.Navitaire.BookingManager.SSRSegment SSRSegment in ssrAvailabilityResponseForBooking.SSRSegmentList)
                            {
                                if (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == return1)
                                {
                                    IsDepart = true;
                                    Session["OneWay"] = true;
                                    using (profiler.Step("InitializeForm"))
                                    {
                                        //InitializeForm(Session["Currency"].ToString(), InsureCode, InsureFee, InsureFeeS1, InsureFeeS2, "Depart");//edited by romy, 20170811, insurance
                                    }
                                }
                                else if (SSRSegment.LegKey.DepartureStation == depart1 && SSRSegment.LegKey.ArrivalStation == transit1)
                                {
                                    IsDepartTransit = true;
                                    Session["OneWay"] = true;
                                    using (profiler.Step("InitializeForm"))
                                    {
                                        //InitializeForm(Session["Currency"].ToString(), InsureCode, InsureFee, InsureFeeS1, InsureFeeS2, "Depart");//edited by romy, 20170811, insurance
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
                //                }
            }
            return "";
        }

        protected void GetPassengerList(string TransID, string Flight)
        {
            var profiler = MiniProfiler.Current;
            try
            {
                DataTable dtPass = new DataTable();

                //dtPass = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTableNewManage(TransID, false, "");
                using (profiler.Step("GetAllBK_PASSENGERLISTdataTable"))
                {
                    dtPass = objBooking.GetAllBK_PASSENGERLISTdataTable(TransID);
                }
                if (dtPass != null && dtPass.Rows.Count > 0)
                {
                    //dtPassOld = dtPass;
                    //dtPass2Old = dtPass;
                    if (Session["dtGridPass"] == null)
                    {
                        Session["dtGridPass"] = dtPass;
                        if (Session["dtDefaultInsure"] != null)
                        {
                            dtPass = (DataTable)Session["dtDefaultInsure"];
                        }
                        gvPassenger.DataSource = dtPass;
                        gvPassenger.DataBind();
                    }

                    //GetInfantDetail(TransID);
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

        protected void ValidatePopup(object sender, CallbackEventArgs e)
        {
            var profiler = MiniProfiler.Current;
            //BookingTransactionMain bookHDRInfo = new BookingTransactionMain();
            //ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
            //using (profiler.Step("GetSingleBK_TRANSMAIN"))
            //{
            //    bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(TransID);
            //}
            DataTable dataClass = new DataTable();
            dataClass = (DataTable)HttpContext.Current.Session["dtGridPass"];
            //for (int ii = 0; ii < dataClass.Rows.Count; ii++)
            //{
            //if (dataClass.Rows[ii]["DepartInfant"].ToString() != "" && dataClass.Rows[ii]["IndicatorDepartInfant"].ToString() == "1")
            //{
            //    if (HttpContext.Current.Session["lstPassInfantData"] != null)
            //    {
            //        lstPassInfantData = (List<PassengerData>)HttpContext.Current.Session["lstPassInfantData"];

            //        List<PassengerData> NewlstPassInfantData = lstPassInfantData.Where(item => item.PassengerID == (dataClass.Rows[ii]["PassengerID"]).ToString()).ToList();
            //        if (NewlstPassInfantData.Count == 0)
            //        {
            //            e.Result = "Please Insert Detail Infant before confirm.";
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        e.Result = "Please Insert Detail Infant before confirm.";
            //        return;
            //    }
            //}
            //}

            List<BookingTransactionDetail> lstTransDetail = new List<BookingTransactionDetail>();

            depart1 = Session["depart1"].ToString();
            transit1 = Session["transit1"].ToString();
            return1 = Session["return1"].ToString();

            //if ((Boolean)Session["OneWay"] != true)
            //{
            //    depart2 = Session["depart2"].ToString();
            //    transit2 = Session["transit2"].ToString();
            //    return2 = Session["return2"].ToString();
            //}

            MessageList msgList = new MessageList();
            ArrayList aMsgList = new ArrayList();
            GetSSRAvailabilityForBookingResponse ssrResponse = (GetSSRAvailabilityForBookingResponse)Session["GetssrAvailabilityResponseForBooking"];
            try
            {
                using (profiler.Step("ValidatePopup"))
                {
                    ArrayList PNR = new ArrayList();

                    //Validate Session
                    if (HttpContext.Current.Session["objListBK_TRANSDTL_Infos"] != null)
                    {
                        List<BookingTransactionDetail> objListBK_TRANSDTL_Infos = (List<BookingTransactionDetail>)HttpContext.Current.Session["objListBK_TRANSDTL_Infos"];
                        foreach (BookingTransactionDetail bkDetail in objListBK_TRANSDTL_Infos)
                        {
                            objBooking.CancelSellRequest(bkDetail.Signature);
                        }
                        HttpContext.Current.Session["objListBK_TRANSDTL_Infos"] = null;
                        HttpContext.Current.Session["SellSessionID"] = null;
                    }
                    //endValidate Session


                    if (Session["AgentSet"] == null)
                    {
                        e.Result = msgList.Err100025;
                        return;
                    }
                    else
                    {
                        DataTable dataClassold = new DataTable();
                        dataClassold = (DataTable)HttpContext.Current.Session["dtGridPassOld"];
                        int i = 0;
                        dataClass = new DataTable();
                        dataClass = (DataTable)HttpContext.Current.Session["dtGridPass"];
                        foreach (DataRow dr in dataClass.Rows)
                        {
                            if (dr["InsureCode"].ToString() != "" && dr["InsureCode"].ToString() != dataClassold.Rows[i]["InsureCode"].ToString() && dr["InsureCode"].ToString() != msgList.Err200000)
                            {
                                //insert total insure fee ke BK_TRANSDTL.insurefee dan BK_TRANSHDR.Transtotalinsure
                                if (dr["InsureFee"] != null)
                                {
                                    //bookHDRInfo.TransTotalAmt = bookHDRInfo.TransTotalAmt + Convert.ToDecimal(dr["InsureFee"]);
                                    //bookHDRInfo.TransTotalFee = bookHDRInfo.TransTotalFee + Convert.ToDecimal(dr["InsureFee"]);
                                    //bookHDRInfo.TransSubTotal = bookHDRInfo.TransSubTotal + Convert.ToDecimal(dr["InsureFee"]);
                                    //bookHDRInfo.TransTotalInsure = bookHDRInfo.TransTotalInsure + Convert.ToDecimal(dr["InsureFee"]);
                                    //if (Convert.ToDecimal(bookHDRInfo.TotalAmtReturn) == 0)
                                    //{
                                    //    bookHDRInfo.TotalAmtGoing = bookHDRInfo.TotalAmtGoing + Convert.ToDecimal(dr["InsureFee"]);
                                    //}
                                    //bookHDRInfo.TotalAmtAVG = bookHDRInfo.TotalAmtAVG + (Convert.ToDecimal(dr["InsureFee"]) / bookHDRInfo.TransTotalPAX);

                                    //objBooking.FillChgTransMain(bookHDRInfo);
                                    //HttpContext.Current.Session.Remove("bookingMain");
                                    //HttpContext.Current.Session.Add("bookingMain", bookHDRInfo);

                                    //ArrayList PNR = new ArrayList();
                                    lstBookDTL = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 0, "LEN(RecordLocator) >= 6 AND ");
                                    OldlstBookDTL = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 0, "LEN(RecordLocator) >= 6 AND ");
                                    decimal totalsum = 0;
                                    foreach (BookingTransactionDetail bkDTL in lstBookDTL)
                                    {
                                        PNR.Add(bkDTL.RecordLocator);
                                        object sumObject;
                                        object sumObjectold;
                                        sumObject = dataClass.Compute("Sum(InsureFee)", "RecordLocator = '" + bkDTL.RecordLocator + "'");// AND PaxNo = 1");
                                        sumObjectold = dataClassold.Compute("Sum(InsureFee)", "RecordLocator = '" + bkDTL.RecordLocator + "'");// AND PaxNo = 1");
                                        if ((!(sumObject is DBNull)))// && (!(sumObject2 is DBNull)))
                                        {
                                            totalsum = (decimal)sumObject - (decimal)sumObjectold;// + (decimal)sumObject2;
                                        }
                                        else
                                        {
                                            totalsum = 0;
                                        }
                                        //bkDTL.LineTotal += totalsum;
                                        //bkDTL.LineNameChange += totalsum;
                                        //bkDTL.LineInsureFee += totalsum;
                                    }
                                    //objBooking.FillChgTransDetail(lstBookDTL, OldlstBookDTL);
                                }

                                BK_TRANSADDONInfo = new Bk_transaddon();
                                BK_TRANSADDONInfo.TransID = dr["TransID"].ToString();
                                BK_TRANSADDONInfo.RecordLocator = dr["RecordLocator"].ToString();
                                BK_TRANSADDONInfo.PassengerID = (Convert.ToInt32(dr["PassengerID"])).ToString();
                                BK_TRANSADDONInfo.Segment = 0;
                                BK_TRANSADDONInfo.SeqNo = 0;
                                BK_TRANSADDONInfo.TripMode = 0;
                                //BK_TRANSADDONInfo.CarrierCode = dr["CarrierCode"].ToString(); //ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.CarrierCode;
                                //BK_TRANSADDONInfo.FlightNo = dr["FlightNo"].ToString(); //ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.FlightNumber;
                                BK_TRANSADDONInfo.Origin = depart1;

                                if (transit1 != "")
                                {
                                    BK_TRANSADDONInfo.Destination = transit1;
                                }
                                else
                                {
                                    BK_TRANSADDONInfo.Destination = return1;
                                }
                                BK_TRANSADDONInfo.InsureCode = dr["InsureCode"].ToString();//added by romy, 20170811, insurance
                                BK_TRANSADDONInfo.InsureAmt = (decimal)dr["InsureFee"];
                                //BK_TRANSADDONInfo.IndicatorInsure = (int)dr["IndicatorDepartInsure"];
                                BK_TRANSADDONInfo.TotalAmount = BK_TRANSADDONInfo.InsureAmt;
                                BK_TRANSADDONInfo.CreateBy = MyUserSet.UserName;
                                BK_TRANSADDONInfo.LastSyncBy = MyUserSet.UserName;

                                listBK_TRANSADDONInfo.Add(BK_TRANSADDONInfo);
                                if (transit1 != "")
                                {
                                    BK_TRANSADDONInfo = new Bk_transaddon();
                                    BK_TRANSADDONInfo.TransID = dr["TransID"].ToString();
                                    BK_TRANSADDONInfo.RecordLocator = dr["RecordLocator"].ToString();
                                    BK_TRANSADDONInfo.PassengerID = (Convert.ToInt32(dr["PassengerID"])).ToString();
                                    BK_TRANSADDONInfo.Segment = 1;
                                    BK_TRANSADDONInfo.SeqNo = 1;
                                    BK_TRANSADDONInfo.TripMode = 0;
                                    //BK_TRANSADDONInfo.CarrierCode = dr["CarrierCode"].ToString(); //ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.CarrierCode;
                                    //BK_TRANSADDONInfo.FlightNo = dr["FlightNo"].ToString(); //ssrResponse.SSRAvailabilityForBookingResponse.SSRSegmentList[0].LegKey.FlightNumber;
                                    BK_TRANSADDONInfo.Origin = transit1;
                                    BK_TRANSADDONInfo.Destination = return1;
                                    BK_TRANSADDONInfo.InsureCode = dr["InsureCode"].ToString();//added by romy, 20170811, insurance
                                    BK_TRANSADDONInfo.InsureAmt = (decimal)dr["InsureFee"];
                                    //BK_TRANSADDONInfo.IndicatorInsure = (int)dr["IndicatorDepartInsure"];
                                    BK_TRANSADDONInfo.TotalAmount = BK_TRANSADDONInfo.InsureAmt;//edited by romy, 20170811, insurance
                                    BK_TRANSADDONInfo.CreateBy = MyUserSet.UserName;
                                    BK_TRANSADDONInfo.LastSyncBy = MyUserSet.UserName;

                                    listBK_TRANSADDONInfo.Add(BK_TRANSADDONInfo);
                                }

                                HttpContext.Current.Session.Remove("ChgTransSSR");
                                HttpContext.Current.Session.Add("ChgTransSSR", listBK_TRANSADDONInfo);
                            }
                            i++;
                            //InsertBK_List1 //By Romy
                        }

                        using (profiler.Step("SellFlight"))
                        {
                            if (SellFlight(listBK_TRANSADDONInfo) != "")//InSure_edited by romy
                            {
                                e.Result = msgList.Err200026;
                                return;
                            }
                        }
                        e.Result = "";
                    }
                }
            }

            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                e.Result = msgList.Err200026;
                log.Error(this, ex);
            }
        }

        private string SellFlight(List<Bk_transaddon> listAll)//InSure_edited by romy
        {
            MessageList msgList = new MessageList();
            //ClearSSRFeeValue();
            ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking("");
            BookingControl bookingControl = new BookingControl();
            List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
            List<BookingTransactionDetail> listBookingDetail = new List<BookingTransactionDetail>();
            List<BookingTransactionDetail> objListBK_TRANSDTL_Infos = new List<BookingTransactionDetail>();
            PassengerData PassData = new PassengerData();
            List<PassengerData> ListPassData = new List<PassengerData>();
            List<BookingTransactionDetail> lstAllDTL = new List<BookingTransactionDetail>();
            List<BookingTransactionDetail> lstAllDTLOLD = new List<BookingTransactionDetail>();
            DataTable dataClass = new DataTable();
            DataTable dataClassold = new DataTable();
            DataTable dtPass = new DataTable();
            if (Session["dtGridPass"] != null) dtPass = (DataTable)Session["dtGridPass"];
            //DataTable dtPass = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTableNewManage(TransID);
            Session["totalcountpax"] = null;
            Decimal totalSSRdepart = 0;
            Decimal totalSSRdepartInsure = 0;//added by romy, 20170823, Insure

            depart1 = Session["depart1"].ToString();
            transit1 = Session["transit1"].ToString();

            //if ((Boolean)Session["OneWay"] != true)
            //{
            //    depart2 = Session["depart2"].ToString();
            //    transit2 = Session["transit2"].ToString();
            //}

            decimal TotSSRDepart = 0;
            decimal TotSSRDepartcommit = 0;

            int insuretest = 0;
            int RespError = 0;
            decimal BalanceDueFinal = 0;
            decimal totalsumDtl = 0;
            var profiler = MiniProfiler.Current;
            BookingTransactionMain bookHDRInfo = new BookingTransactionMain();
            ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
            using (profiler.Step("GetSingleBK_TRANSMAIN"))
            {
                bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(TransID);
            }
            try
            {
                if (listAll.Count > 0)
                {
                    dataClass = (DataTable)HttpContext.Current.Session["dtGridPass"];
                    dataClassold = (DataTable)HttpContext.Current.Session["dtGridPassOld"];
                    using (profiler.Step("AddInsureFees"))
                    {
                        for (int ppp = 0; ppp < listAll.Count; ppp++)
                        {
                            //objBooking.AddInsureFees(SellSessionID, dtPass.Rows[0]["Currency"].ToString(), Convert.ToInt32(listAll[ppp].PassengerID), listAll[ppp].InsureCode, listAll[ppp].RecordLocator);
                            objBooking.AddInsureFees(SellSessionID, Session["Currency"].ToString(), Convert.ToInt32(listAll[ppp].PassengerID), listAll[ppp].InsureCode, listAll[ppp].RecordLocator);
                        }
                    }
                    objListBK_TRANSDTL_Infos = (List<BookingTransactionDetail>)HttpContext.Current.Session["objListBK_TRANSDTL_Infos"];

                    using (profiler.Step("OverrideFee"))
                    {
                        foreach (BookingTransactionDetail a in objListBK_TRANSDTL_Infos)
                        {
                            ABS.Navitaire.BookingManager.Booking responseBookingFromState = bookingControl.GetBookingFromState(a.Signature);
                            //string xml = GetXMLString(responseBookingFromState);
                            for (int ppp = 0; ppp < listAll.Count; ppp++)
                            {
                                if (listAll[ppp].RecordLocator == a.RecordLocator)
                                {
                                    short FeeNumber = responseBookingFromState.Passengers.Where(z => z.PassengerNumber == Convert.ToInt32(listAll[ppp].PassengerID)).Select(x => x.PassengerFees.Where(n => n.FeeCode == listAll[ppp].InsureCode).FirstOrDefault().FeeNumber).FirstOrDefault();
                                    objBooking.OverrideFee(a.Signature.ToString(), listAll[ppp].PassengerID.ToString(), Convert.ToDecimal(listAll[ppp].InsureAmt), listAll[ppp].RecordLocator.ToString(), FeeNumber);
                                }
                            }
                        }
                    }

                    listDetailCombinePNR = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 1, "LEN(RecordLocator) >= 6 AND ");
                    listBookingDetail = objBooking.GetAllBK_TRANSDTLFilterAll(dataClass.Rows[0]["TransID"].ToString(), 0);
                    if (Convert.ToDecimal(dataClass.Compute("Sum(InsureFee)", "")) != 0 && Convert.ToDecimal(dataClassold.Compute("Sum(InsureFee)", "")) != 0)
                    {
                        TotSSRDepartcommit += Convert.ToDecimal(dataClass.Compute("Sum(InsureFee)", "")) - Convert.ToDecimal(dataClassold.Compute("Sum(InsureFee)", ""));
                    }
                    else if (Convert.ToDecimal(dataClass.Compute("Sum(InsureFee)", "")) != 0)
                    {
                        TotSSRDepartcommit += Convert.ToDecimal(dataClass.Compute("Sum(InsureFee)", ""));
                    }

                    foreach (BookingTransactionDetail a in objListBK_TRANSDTL_Infos)
                    {
                        //after sellfee/addinsurefee
                        ABS.Navitaire.BookingManager.Booking AfterOverride = bookingControl.GetBookingFromState(a.Signature);
                        string xmls = GetXMLString(AfterOverride);

                        //Checking Changes, direct payment or detailbooking
                        if (AfterOverride.BookingSum.BalanceDue != null && Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue) > 0)
                        {
                            if (TotSSRDepartcommit != 0)
                            {
                                for (int dc = 0; dc < dtPass.Rows.Count; dc++)
                                {
                                    if (listAll.Where(x => x.PassengerID == dtPass.Rows[dc]["PassengerID"].ToString() && x.RecordLocator == dtPass.Rows[dc]["RecordLocator"].ToString()).Count() > 0)
                                    {
                                        PassData = new PassengerData();
                                        PassData.TransID = (string)dtPass.Rows[dc]["TransID"];
                                        PassData.PassengerID = (string)dtPass.Rows[dc]["PassengerID"];
                                        PassData.RecordLocator = (string)dtPass.Rows[dc]["RecordLocator"];
                                        PassData.InsureCode = listAll.Where(x => x.PassengerID == dtPass.Rows[dc]["PassengerID"].ToString() && x.RecordLocator == dtPass.Rows[dc]["RecordLocator"].ToString()).FirstOrDefault().InsureCode;
                                        PassData.InsureFee = listAll.Where(x => x.PassengerID == dtPass.Rows[dc]["PassengerID"].ToString() && x.RecordLocator == dtPass.Rows[dc]["RecordLocator"].ToString()).FirstOrDefault().InsureAmt;
                                        PassData.InsureDate = DateTime.Now;
                                        PassData.MaxChange = (int)dtPass.Rows[dc]["MaxChange"];
                                        PassData.MaxPax1 = (int)dtPass.Rows[dc]["MaxPax1"];
                                        PassData.MaxPax2 = (int)dtPass.Rows[dc]["MaxPax2"];
                                        PassData.ChangeCount = (int)dtPass.Rows[dc]["ChangeCount"];
                                        ListPassData.Add(PassData);
                                    }
                                }
                                HttpContext.Current.Session.Remove("lstPassengerData");
                                HttpContext.Current.Session.Add("lstPassengerData", ListPassData);
                                //BookingTransactionMain bookingMain = new BookingTransactionMain();
                                //bookingMain = objBooking.GetSingleBK_TRANSMAIN(dataClass.Rows[0]["TransID"].ToString());
                                //decimal totalsum = TotSSRDepartcommit;
                                //decimal totalamountdue = totalamountduegoingcommit;
                                //bookingMain.TransTotalSSR = bookingMain.TransTotalSSR + totalamountdue;
                                //bookingMain.TotalAmtGoing = bookingMain.TotalAmtGoing + totalamountduegoingcommit;

                                //objBooking.UpdateTotalSSR(dataClass.Rows[0]["TransID"].ToString(), bookingMain, listBookingDetail);
                            }
                            bookHDRInfo.TransTotalAmt = bookHDRInfo.TransTotalAmt + Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue);
                            bookHDRInfo.TransTotalFee = bookHDRInfo.TransTotalFee + Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue);
                            bookHDRInfo.TransSubTotal = bookHDRInfo.TransSubTotal + Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue);
                            bookHDRInfo.TransTotalInsure = bookHDRInfo.TransTotalInsure + Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue);
                            if (Convert.ToDecimal(bookHDRInfo.TotalAmtReturn) == 0)
                            {
                                bookHDRInfo.TotalAmtGoing = bookHDRInfo.TotalAmtGoing + Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue);
                            }
                            bookHDRInfo.TotalAmtAVG = bookHDRInfo.TotalAmtAVG + (Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue) / bookHDRInfo.TransTotalPAX);
                            objBooking.FillChgTransMain(bookHDRInfo);
                            HttpContext.Current.Session.Add("bookingMain", bookHDRInfo);
                            //List<BookingTransactionDetail> lstBookDTL = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 0, "RecordLocator = '" + a.RecordLocator + "' AND ");
                            //foreach (BookingTransactionDetail bkDTL in lstBookDTL)
                            //{
                            //    bkDTL.LineTotal += Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue);
                            //    bkDTL.LineNameChange += Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue);
                            //    bkDTL.LineInsureFee += Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue);
                            //}
                            var tmpOld = OldlstBookDTL.Where(t => t.TransID == TransID && t.RecordLocator == a.RecordLocator);
                            if (tmpOld.ToList() != null)
                            {
                                var listOld = tmpOld.ToList();
                                lstAllDTLOLD.Add(listOld.FirstOrDefault());
                            }
                            var tmp = lstBookDTL.Where(t => t.TransID == TransID && t.RecordLocator == a.RecordLocator);
                            if (tmp.ToList() != null)
                            {
                                //lstBookDTL.Where(t => t.TransID == TransID && t.RecordLocator == a.RecordLocator).FirstOrDefault().LineTotal += Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue);
                                //lstBookDTL.Where(t => t.TransID == TransID && t.RecordLocator == a.RecordLocator).FirstOrDefault().LineNameChange += Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue);
                                //lstBookDTL.Where(t => t.TransID == TransID && t.RecordLocator == a.RecordLocator).FirstOrDefault().LineInsureFee += Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue);
                                var list = tmp.ToList();
                                list.FirstOrDefault().LineTotal += Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue);
                                list.FirstOrDefault().LineNameChange += Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue);
                                list.FirstOrDefault().LineInsureFee += Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue);

                                lstAllDTL.Add(list.FirstOrDefault());
                            }
                            //objBooking.FillChgTransDetail(lstBookDTL);
                        }
                        else if (AfterOverride.BookingSum.BalanceDue != null && Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue) == 0)
                        {
                            if (TotSSRDepartcommit != 0)
                            {
                                for (int dc = 0; dc < dtPass.Rows.Count; dc++)
                                {
                                    if (listAll.Where(x => x.PassengerID == dtPass.Rows[dc]["PassengerID"].ToString() && x.RecordLocator == dtPass.Rows[dc]["RecordLocator"].ToString()).Count() > 0)
                                    {
                                        PassData = new PassengerData();
                                        PassData.TransID = (string)dtPass.Rows[dc]["TransID"];
                                        PassData.PassengerID = (string)dtPass.Rows[dc]["PassengerID"];
                                        PassData.RecordLocator = (string)dtPass.Rows[dc]["RecordLocator"];
                                        PassData.InsureCode = listAll.Where(x => x.PassengerID == dtPass.Rows[dc]["PassengerID"].ToString() && x.RecordLocator == dtPass.Rows[dc]["RecordLocator"].ToString()).FirstOrDefault().InsureCode;
                                        PassData.InsureFee = listAll.Where(x => x.PassengerID == dtPass.Rows[dc]["PassengerID"].ToString() && x.RecordLocator == dtPass.Rows[dc]["RecordLocator"].ToString()).FirstOrDefault().InsureAmt;
                                        PassData.InsureDate = DateTime.Now;
                                        PassData.MaxChange = (int)dtPass.Rows[dc]["MaxChange"];
                                        PassData.MaxPax1 = (int)dtPass.Rows[dc]["MaxPax1"];
                                        PassData.MaxPax2 = (int)dtPass.Rows[dc]["MaxPax2"];
                                        PassData.ChangeCount = (int)dtPass.Rows[dc]["ChangeCount"];
                                        ListPassData.Add(PassData);
                                    }
                                }
                                objBooking.SaveBK_PASSENGERLIST(ListPassData, CoreBase.EnumSaveType.Update);

                                BookingTransactionMain bookingMain = new BookingTransactionMain();
                                bookingMain = objBooking.GetSingleBK_TRANSMAIN(dataClass.Rows[0]["TransID"].ToString());
                                decimal totalsum = TotSSRDepartcommit;
                                decimal totalamountdue = totalamountduegoingcommit;
                                bookingMain.TransTotalSSR = bookingMain.TransTotalSSR + totalamountdue;
                                bookingMain.TotalAmtGoing = bookingMain.TotalAmtGoing + totalamountduegoingcommit;
                                bookingMain.PaymentAmtEx2 = bookingMain.PaymentAmtEx2 + totalamountdue;
                                listBookingDetail[0].PayDueAmount2 += totalamountdue;

                                objBooking.SaveHeaderDetail(bookingMain, listBookingDetail, CoreBase.EnumSaveType.Update);
                            }
                            bookHDRInfo.TransTotalAmt = bookHDRInfo.TransTotalAmt + Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue);
                            bookHDRInfo.TransTotalFee = bookHDRInfo.TransTotalFee + Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue);
                            bookHDRInfo.TransSubTotal = bookHDRInfo.TransSubTotal + Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue);
                            bookHDRInfo.TransTotalInsure = bookHDRInfo.TransTotalInsure + Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue);
                            if (Convert.ToDecimal(bookHDRInfo.TotalAmtReturn) == 0)
                            {
                                bookHDRInfo.TotalAmtGoing = bookHDRInfo.TotalAmtGoing + Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue);
                            }
                            bookHDRInfo.TotalAmtAVG = bookHDRInfo.TotalAmtAVG + (Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue) / bookHDRInfo.TransTotalPAX);
                            objBooking.FillChgTransMain(bookHDRInfo);
                            HttpContext.Current.Session.Add("bookingMain", bookHDRInfo);
                            //List<BookingTransactionDetail> lstBookDTL = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 0, "RecordLocator = '" + a.RecordLocator + "' AND ");
                            //foreach (BookingTransactionDetail bkDTL in lstBookDTL)
                            //{
                            //    bkDTL.LineTotal += Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue);
                            //    bkDTL.LineNameChange += Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue);
                            //    bkDTL.LineInsureFee += Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue);

                            //}                            
                            var tmpOld = OldlstBookDTL.Where(t => t.TransID == TransID && t.RecordLocator == a.RecordLocator);
                            if (tmpOld.ToList() != null)
                            {
                                var listOld = tmpOld.ToList();
                                lstAllDTLOLD.Add(listOld.FirstOrDefault());
                            }
                            var tmp = lstBookDTL.Where(t => t.TransID == TransID && t.RecordLocator == a.RecordLocator);
                            if (tmp.ToList() != null)
                            {
                                //lstBookDTL.Where(t => t.TransID == TransID && t.RecordLocator == a.RecordLocator).FirstOrDefault().LineTotal += Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue);
                                //lstBookDTL.Where(t => t.TransID == TransID && t.RecordLocator == a.RecordLocator).FirstOrDefault().LineNameChange += Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue);
                                //lstBookDTL.Where(t => t.TransID == TransID && t.RecordLocator == a.RecordLocator).FirstOrDefault().LineInsureFee += Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue);
                                var list = tmp.ToList();
                                list.FirstOrDefault().LineTotal += Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue);
                                list.FirstOrDefault().LineNameChange += Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue);
                                list.FirstOrDefault().LineInsureFee += Convert.ToDecimal(AfterOverride.BookingSum.BalanceDue);

                                lstAllDTL.Add(list.FirstOrDefault());
                            }

                            //objBooking.FillChgTransDetail(lstBookDTL);
                        }
                        BalanceDueFinal = BalanceDueFinal + AfterOverride.BookingSum.BalanceDue;
                    }

                    HashSet<string> diffids = new HashSet<string>(lstAllDTL.Select(s => s.RecordLocator));
                    var results = listDetailCombinePNR.Where(m => !diffids.Contains(m.RecordLocator)).ToList();
                    if (results != null && results.Count > 0)
                    {
                        lstAllDTL.AddRange(results);
                        //lstAllDTLOLD.AddRange(results);
                    }

                    objBooking.FillChgTransDetail(lstAllDTL, listDetailCombinePNR);

                    if (BalanceDueFinal > 0)
                    {
                        Session["ChgMode"] = "5"; //1= Manage Add-On					
                        ASPxCallback.RedirectOnCallback(GroupBooking.Web.Shared.MySite.PublicPages.Payment);
                    }
                    else
                    {
                        for (int ppp = 0; ppp < listAll.Count; ppp++)
                        {
                            RespPurchase = GetPlan.Purchase(listAll[ppp].RecordLocator, "1");
                            if (RespPurchase.ErrorCode != "00000")
                            {
                                RespError++;
                            }
                        }
                        if (RespError == 0)
                        {
                            Response.RedirectLocation = "bookingdetail.aspx?k=" + Request.QueryString["k"] + "&TransID=" + Request.QueryString["TransID"];
                        }
                    }
                }
                else
                {
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
            MessageList msgList = new MessageList();
            string ssr = "";
            object ssrvalue;
            //Thread.Sleep(2000);
            var profiler = MiniProfiler.Current;
            try
            {
                //added by romy for Double Premium Amount
                DataTable dtInfant = new DataTable();
                dtInfant = (DataTable)Session["InfantCheck"];
                dtInfant.PrimaryKey = new DataColumn[] { (dtInfant.Columns["PNR"]), (dtInfant.Columns["PassengerID"]) };

                if (infantmax > 10) infantmax = 10;
                foreach (var args in e.UpdateValues)
                {
                    DataTable data = Session["dtGridPass"] as DataTable;
                    //DataTable data2 = new DataTable();
                    DataTable dataResp = new DataTable();
                    if (Session["RespInsure"] != null)
                    {
                        //datax=(DataTable)Session["RespInsure"];
                        dataResp = GetPlan.ConvertToDataTable((List<InsureResponse>)Session["RespInsure"]);
                        dataResp.PrimaryKey = new DataColumn[] { (dataResp.Columns["PNR"]), (dataResp.Columns["PassengerNumber"]) };
                    }

                    data.PrimaryKey = new DataColumn[] { (data.Columns["RecordLocator"]), (data.Columns["PassengerID"]) };

                    // Create an array for the key values to find.
                    object[] findTheseVals = new object[2];

                    // Set the values of the keys to find.
                    findTheseVals[0] = (args.Keys["RecordLocator"]);
                    findTheseVals[1] = (args.Keys["PassengerID"]);

                    DataRow row = data.Rows.Find(findTheseVals);
                    //added by romy for Double Premium Amount
                    DataRow rowInf = dtInfant.Rows.Find(findTheseVals);
                    //DataRow row2 = data2.NewRow();
                    DataRow rowResp = dataResp.NewRow();
                    if (dataResp != null && dataResp.Rows.Count > 0)
                    {
                        rowResp = dataResp.Rows.Find(findTheseVals);
                    }
                    row["RecordLocator"] = args.Keys["RecordLocator"];
                    row["PassengerID"] = args.Keys["PassengerID"];

                    string SSRColumn = "Insure";
                    if (rowResp != null)
                    {
                        using (profiler.Step("AssignValues"))
                        {
                            AssignValues(row, rowResp, rowInf, args.NewValues["InsureCode"], ref SSRColumn, "Depart", gvPassenger);
                        }
                    }
                    else
                    {
                        row["InsureCode"] = msgList.Err200000;
                        row["Status"] = msgList.Err200000;
                        row["IndicatorClear"] = 1;
                    }
                    Session["dtGridPass"] = data;
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
        protected void AssignValues(DataRow row, DataRow row2, DataRow rowInf, object NewValues, ref string SSRColumns, string Flight, ASPxGridView grid)
        {
            MessageList msgList = new MessageList();
            int count = 0;
            String Detail = "";
            String subvalue = "";
            //added by romy for Double Premium Amount
            DataTable dtSysPrefttry = new DataTable();
            dtSysPrefttry = (DataTable)Session["dtSysPrefttry"];

            if (NewValues != null && NewValues != "")
            {
                DataTable dtBaggage = Session["dt" + SSRColumns + Flight] as DataTable;
                //DataTable dtInfantDepart = Session["dtInfantDepart"] as DataTable;
                //DataTable dtInfantReturn = Session["dtInfantReturn"] as DataTable;
                if (NewValues.ToString().Length == 4)
                {
                    if (row[SSRColumns + "Code"] != null && row[SSRColumns + "Code"].ToString() != "")
                    {
                        DataRow[] resultBaggage = dtBaggage.Select("FeeCode = '" + NewValues + "'");
                        foreach (DataRow rows in resultBaggage)
                        {
                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                            if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row[SSRColumns + "Fee"]) && row[SSRColumns + "Code"].ToString().Trim() == rows[0].ToString().Trim())
                            {
                                continue;
                                //return;
                            }
                            else if (Convert.ToDecimal(row2["Amount"]) < Convert.ToDecimal(row[SSRColumns + "Fee"]))
                            {
                                grid.JSProperties["cp_result"] = msgList.Err200023;
                                count = 1;
                                //return;
                            }
                            else
                            {
                                //row[Flight + SSRColumns] = rows["ConcatenatedField"];
                                //row["Price" + Flight + SSRColumns] = Convert.ToDecimal(Detail);
                                //row["Price" + Flight + SSRColumns + "1"] = Convert.ToDecimal(rows[3]);
                                //row["PriceCon" + Flight + SSRColumns] = Convert.ToDecimal(rows[4]);

                                //added by romy for Double Premium Amount
                                //if (dtSysPrefttry != null && Session["DepartRoute"] != null && dtSysPrefttry.Rows[0]["SYSValueEx"].ToString() == Session["DepartRoute"].ToString() && rowInf["DepartInfant"].ToString() != "")
                                //if (row2[4].ToString() == "True" && rowInf["DepartInfant"].ToString() != "")
                                //{
                                //    row[SSRColumns + "Fee"] = Math.Round(Convert.ToDecimal(row2[5])*2, 2);
                                //}
                                //else {
                                //    row[SSRColumns + "Fee"] = Math.Round(Convert.ToDecimal(row2[5]), 2);
                                //}
                                row[SSRColumns + "Fee"] = string.Format("{0:0.00}", row2["Amount"]); //string.Format("{0:0.00}", row2["Amount"]);
                                row["Status"] = "Added Insurance";
                                //row[SSRColumns + "Fee"] = Math.Round(Convert.ToDecimal(row2[4]), 2); //Convert.ToDecimal(row2[4]);
                                //row["Price" + Flight + SSRColumns + "1"] = Math.Round(Convert.ToDecimal(row2[4]), 2); //Convert.ToDecimal(row2[4]);
                                //row["PriceCon" + Flight + SSRColumns] = Math.Round(Convert.ToDecimal(row2[4]), 2); //Convert.ToDecimal(row2[4]);
                                row[SSRColumns + "Code"] = rows["FeeCode"];
                                //row["Indicator" + Flight + SSRColumns] = "1";
                                row[SSRColumns + "Fee1"] = row2["Amount"] == null ? "" : string.Format("{0:0.00}", row2["Amount"]).ToString();
                            }
                        }
                    }
                    else
                    {
                        DataRow[] resultBaggage = dtBaggage.Select("FeeCode = '" + NewValues + "'");
                        foreach (DataRow rows in resultBaggage)
                        {
                            //row[Flight + SSRColumns] = rows["ConcatenatedField"];
                            //Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                            ////row["Price" + Flight + SSRColumns] = Convert.ToDecimal(Detail);
                            //row["Price" + Flight + SSRColumns + "1"] = Convert.ToDecimal(rows[3]);
                            //row["PriceCon" + Flight + SSRColumns] = Convert.ToDecimal(rows[4]);

                            //added by romy for Double Premium Amount
                            //if (dtSysPrefttry != null && Session["DepartRoute"] != null && dtSysPrefttry.Rows[0]["SYSValueEx"].ToString() == Session["DepartRoute"].ToString() && rowInf["DepartInfant"].ToString() != "")
                            //if (row2[4].ToString() == "True" && rowInf["DepartInfant"].ToString() != "")
                            //{
                            //    row[SSRColumns + "Fee"] = Math.Round(Convert.ToDecimal(row2[5]) * 2, 2);
                            //}
                            //else {
                            //    row[SSRColumns + "Fee"] = Math.Round(Convert.ToDecimal(row2[5]), 2);
                            //}
                            row[SSRColumns + "Fee"] = string.Format("{0:0.00}", row2["Amount"]);
                            row["Status"] = "Added Insurance";
                            //row[SSRColumns + "Fee"] = Math.Round(Convert.ToDecimal(row2[4]), 2); //Convert.ToDecimal(row2[4]);
                            //row["Price" + Flight + SSRColumns + "1"] = Math.Round(Convert.ToDecimal(row2[4]), 2); //Convert.ToDecimal(row2[4]);
                            //row["PriceCon" + Flight + SSRColumns] = Math.Round(Convert.ToDecimal(row2[4]), 2); //Convert.ToDecimal(row2[4]);
                            row[SSRColumns + "Code"] = rows["FeeCode"];
                            //row["Indicator" + Flight + SSRColumns] = "1";
                            row[SSRColumns + "Fee1"] = row2["Amount"] == null ? "" : string.Format("{0:0.00}", row2["Amount"]).ToString();
                        }
                    }
                }
                else
                {
                    if (row[Flight + SSRColumns] != null && row[Flight + SSRColumns].ToString() != "")
                    {
                        //if (SSRColumns == "Baggage") subvalue = NewValues.ToString().Substring(0, 20);
                        //else if (SSRColumns == "Sport") subvalue = subvalue = NewValues.ToString().Substring(0, 21);
                        //else if (SSRColumns == "Insure") subvalue = subvalue = NewValues.ToString().Substring(0, 23);//added by romy, 20170811, insurance
                        //else if (SSRColumns == "Infant") subvalue = subvalue = NewValues.ToString().Substring(0, 6);
                        if (SSRColumns == "Insure") subvalue = subvalue = NewValues.ToString().Substring(0, 16);
                        DataRow[] resultBaggage = dtBaggage.Select("Detail = '" + subvalue + "'");
                        foreach (DataRow rows in resultBaggage)
                        {
                            Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                            if (Convert.ToDecimal(Detail) != Convert.ToDecimal(row[SSRColumns + "Fee"]) && row[SSRColumns + "Code"].ToString().Trim() == rows[0].ToString().Trim())
                            {
                                continue;
                                //return;
                            }
                            else if (Convert.ToDecimal(row2["Amount"]) < Convert.ToDecimal(row[SSRColumns + "Fee"]))
                            {
                                grid.JSProperties["cp_result"] = msgList.Err200023;
                                count = 1;
                                //return;
                            }
                            else
                            {
                                if (row[SSRColumns + "Code"].ToString().Trim() != rows["FeeCode"].ToString().Trim())
                                {
                                    //row[Flight + SSRColumns] = rows["ConcatenatedField"];
                                    //row["Price" + Flight + SSRColumns] = Convert.ToDecimal(Detail);
                                    //row["Price" + Flight + SSRColumns + "1"] = Convert.ToDecimal(rows[3]);
                                    //row["PriceCon" + Flight + SSRColumns] = Convert.ToDecimal(rows[4]);

                                    //added by romy for Double Premium Amount
                                    //if (dtSysPrefttry != null && Session["DepartRoute"] != null && dtSysPrefttry.Rows[0]["SYSValueEx"].ToString() == Session["DepartRoute"].ToString() && rowInf["DepartInfant"].ToString() != "")
                                    //if (row2[4].ToString() == "True" && rowInf["DepartInfant"].ToString() != "")
                                    //{
                                    //    row[SSRColumns + "Fee"] = Math.Round(Convert.ToDecimal(row2[5]) * 2, 2);
                                    //}
                                    //else {
                                    //    row[SSRColumns + "Fee"] = Math.Round(Convert.ToDecimal(row2[5]), 2);
                                    //}
                                    row[SSRColumns + "Fee"] = string.Format("{0:0.00}", row2["Amount"]);
                                    row["Status"] = "Added Insurance";
                                    //row[SSRColumns + "Fee"] = Math.Round(Convert.ToDecimal(row2[4]), 2); //Convert.ToDecimal(row2[4]);
                                    //row["Price" + Flight + SSRColumns + "1"] = Math.Round(Convert.ToDecimal(row2[4]), 2); //Convert.ToDecimal(row2[4]);
                                    //row["PriceCon" + Flight + SSRColumns] = Math.Round(Convert.ToDecimal(row2[4]), 2); //Convert.ToDecimal(row2[4]);
                                    row[SSRColumns + "Code"] = rows["FeeCode"];
                                    //row["Indicator" + Flight + SSRColumns] = "1";
                                    row[SSRColumns + "Fee1"] = row2["Amount"] == null ? "" : string.Format("{0:0.00}", row2["Amount"]).ToString();
                                }
                            }
                        }
                    }
                    else
                    {
                        //if (SSRColumns == "Baggage") subvalue = NewValues.ToString().Substring(0, 20);
                        //else if (SSRColumns == "Sport") subvalue = subvalue = NewValues.ToString().Substring(0, 21);
                        //else if (SSRColumns == "Insure") subvalue = subvalue = NewValues.ToString().Substring(0, 23);//added by romy, 20170811, insurance
                        //else if (SSRColumns == "Infant") subvalue = subvalue = NewValues.ToString().Substring(0, 6);
                        if (SSRColumns == "Insure") subvalue = subvalue = NewValues.ToString().Substring(0, 16);
                        DataRow[] resultBaggage = dtBaggage.Select("Detail = '" + subvalue + "'");
                        foreach (DataRow rows in resultBaggage)
                        {
                            if (row["SSRCode" + Flight + SSRColumns].ToString().Trim() != rows["FeeCode"].ToString().Trim())
                            {
                                //row[Flight + SSRColumns] = rows["ConcatenatedField"];
                                //Detail = rows[2].ToString().Substring(0, rows[2].ToString().Length - 4);
                                ////row["Price" + Flight + SSRColumns] = Convert.ToDecimal(Detail);
                                //row["Price" + Flight + SSRColumns + "1"] = Convert.ToDecimal(rows[3]);
                                //row["PriceCon" + Flight + SSRColumns] = Convert.ToDecimal(rows[4]);

                                //added by romy for Double Premium Amount
                                //if (dtSysPrefttry != null && Session["DepartRoute"] != null && dtSysPrefttry.Rows[0]["SYSValueEx"].ToString() == Session["DepartRoute"].ToString() && rowInf["DepartInfant"].ToString() != "")
                                //if (row2[4].ToString() == "True" && rowInf["DepartInfant"].ToString() != "")
                                //{
                                //    row[SSRColumns + "Fee"] = Math.Round(Convert.ToDecimal(row2[5]) * 2, 2);
                                //}
                                //else {
                                //    row[SSRColumns + "Fee"] = Math.Round(Convert.ToDecimal(row2[5]), 2);
                                //}
                                row[SSRColumns + "Fee"] = string.Format("{0:0.00}", row2["Amount"]);
                                row["Status"] = "Added Insurance";
                                //row[SSRColumns + "Fee"] = Math.Round(Convert.ToDecimal(row2[4]), 2); //Convert.ToDecimal(row2[4]);
                                //row["Price" + Flight + SSRColumns + "1"] = Math.Round(Convert.ToDecimal(row2[4]), 2); //Convert.ToDecimal(row2[4]);
                                //row["PriceCon" + Flight + SSRColumns] = Math.Round(Convert.ToDecimal(row2[4]), 2); //Convert.ToDecimal(row2[4]);
                                row[SSRColumns + "Code"] = rows["FeeCode"];
                                //row["Indicator" + Flight + SSRColumns] = "1";
                                row[SSRColumns + "Fee1"] = row2["Amount"] == null ? "" : string.Format("{0:0.00}", row2["Amount"]).ToString();
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