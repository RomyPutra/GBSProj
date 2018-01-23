using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Collections;
using System.Web.UI.WebControls;
using ABS.Logic.Shared;
using DevExpress.Web;
using System.Globalization;
//using log4net;
using ABS.Logic.GroupBooking.Booking;
using ABS.Navitaire.AccountManager;
using ABS.Navitaire.AgentManager;
using ABS.Navitaire.BookingManager;
using System.Threading;
using System.Text;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Configuration;
using ABS.Logic.GroupBooking;
using ABS.GBS.Log;
using StackExchange.Profiling;

namespace GroupBooking.Web
{
    public partial class ManageSeat : System.Web.UI.Page
    {
        #region declaration
        UserSet MyUserSet;
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogControl log = new LogControl();
        SystemLog SystemLog = new SystemLog();
        PassengerContainer objPassengerContainer = new PassengerContainer();
        PassengerContainer objPassengerContainerNew = new PassengerContainer();
        List<PassengerContainer> lstPassengerContainer = new List<PassengerContainer>();
        List<PassengerContainer> lstPassengerContainerNew = new List<PassengerContainer>();
        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
        ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");

        BookingTransactionDetail bookDTLInfo = new BookingTransactionDetail();
        List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
        List<BookingTransactionDetail> listBookingDetail = new List<BookingTransactionDetail>();
        BookingTransactionMain bookHDRInfo = new BookingTransactionMain();
        ABS.Logic.GroupBooking.Agent.AgentControl objAgent = new ABS.Logic.GroupBooking.Agent.AgentControl();
        ABS.Logic.GroupBooking.Agent.AgentProfileControl objAgentProfile = new ABS.Logic.GroupBooking.Agent.AgentProfileControl();
        decimal totalFlightFare, totalServiceFee, totalBaggageFare = 0, totalServVAT;
        ABS.Logic.GroupBooking.Agent.AgentProfile agent = new ABS.Logic.GroupBooking.Agent.AgentProfile();

        List<BookingTransactionDetail> listDetailCombinePNR = new List<BookingTransactionDetail>();
        BookingTransactionDetail objBK_TRANSDTL_Infos;
        List<BookingTransactionDetail> objListBK_TRANSDTL_Infos = new List<BookingTransactionDetail>();

        List<PassengerData> lstPassenger = new List<PassengerData>();
        PassengerData rowPassenger;

        int havebalance = 0;
        string Currency = "USD";
        decimal APT = 0;
        decimal AVGFare = 0;
        //string tranID = "";
        DataTable dtTaxFees = new DataTable();

        private static int num = 0;
        private static int departID = 0;
        private static string ReturnID = "";
        DataTable dtTransMain;
        string TransId;
        string HashingKey;

        EnumFlight eFlight;

        ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model = null;
        ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model2 = null;

        decimal totalseatfeeall = 0;
        decimal totalamountdueseatfeeall = 0;
        decimal totalamountdueseatfeeallperPNR = 0;
        decimal totalamountdueseatfeeallgoing = 0;
        decimal totalamountdueseatfeeallreturn = 0;
        decimal totalseatfeeallcommit = 0;
        decimal totalamountdueseatfeeallcommit = 0;
        decimal totalamountdueseatfeeallcommitperPNR = 0;
        decimal totalseatfeeallcommitgoing = 0;
        decimal totalseatfeeallcommitreturn = 0;
        decimal totalamountdueseatfeeallcommitgoing = 0;
        decimal totalamountdueseatfeeallcommitreturn = 0;

        //added by ketee, 20170120, save total connecting flight seat fees
        ////string[,] totalConnectingSeatFees;
        ////string[,] totalConnectingSeatFees2;

        public enum EnumFlight
        {
            DirectFlight = 1,
            ConnectingFlight = 2
        }

        public enum EnumFlightType
        {
            DepartFlight = 1,
            ReturnFlight = 2,
            DepartConnectingFlight = 3,
            ReturnConnectingFlight = 4,
            DepartConnectingFlight2 = 5,
            ReturnConnectingFlight2 = 6
        }
        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Expires = 0;
            Response.Cache.SetNoStore();
            Response.AppendHeader("Pragma", "no-cache");
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;
            try
            {
                using (profiler.Step("InitializeForm"))
                {
                    InitializeForm();
                }
                if (!SetAccess())
                {
                    return;
                }
                if (IsPostBack == false)
                {
                    HttpContext.Current.Session.Remove("Balance");
                    using (profiler.Step("intSeatTab"))
                    {
                        intSeatTabSession();
                    }
                    if (Session["btnSelected"] == null)
                    {
                        ClearSession();
                        HttpContext.Current.Session.Remove("PassengerContainer");
                        Session["btnSelected"] = 0;
                        using (profiler.Step("LoadData"))
                        {
                            LoadData(TransId);
                        }
                    }


                }

                using (profiler.Step("FillFlight"))
                {
                    FillFlight("", (int)Session["btnSelected"]);
                }

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
                Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.InvalidPage);
            }
        }
        #endregion

        #region Control
        protected void btnSeatDepart1_ServerClick(object sender, EventArgs e)
        {
            pnlErr.Visible = false;
            Session["btnSelected"] = 0;
            Session["ErrorMsg"] = null;
            //'ReservedSeat()
            string hashkey = Request.QueryString["k"];

            Response.Redirect("ManageSeat.aspx?k=" + hashkey + "&TransID=" + TransId, false);
            //Response.Redirect("ManageSeat.aspx");
        }

        protected void btnSeatDepart2_ServerClick(object sender, EventArgs e)
        {
            pnlErr.Visible = false;
            Session["btnSelected"] = 1;
            Session["ErrorMsg"] = null;
            //'ReservedSeat()
            string hashkey = Request.QueryString["k"];

            Response.Redirect("ManageSeat.aspx?k=" + hashkey + "&TransID=" + TransId, false);
            //Response.Redirect("ManageSeat.aspx");
        }
        protected void btnSeatReturn1_ServerClick(object sender, EventArgs e)
        {
            pnlErr.Visible = false;
            Session["btnSelected"] = 2;
            Session["ErrorMsg"] = null;
            //'ReservedSeat()
            string hashkey = Request.QueryString["k"];

            Response.Redirect("ManageSeat.aspx?k=" + hashkey + "&TransID=" + TransId, false);
            //Response.Redirect("ManageSeat.aspx");
        }

        protected void btnSeatReturn2_ServerClick(object sender, EventArgs e)
        {
            pnlErr.Visible = false;
            Session["btnSelected"] = 3;
            Session["ErrorMsg"] = null;
            //'ReservedSeat()
            string hashkey = Request.QueryString["k"];

            Response.Redirect("ManageSeat.aspx?k=" + hashkey + "&TransID=" + TransId, false);
            //Response.Redirect("ManageSeat.aspx");
        }
        #endregion

        #region Function and Procedure
        protected void InitializeForm()
        {
            try
            {
                TransId = Request.QueryString["TransID"];
                HashingKey = Request.QueryString["k"];
                hID.Value = TransId;
                //SetCookie();
                //BindModel();
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
        }
        private bool SetAccess()
        {
            string depart1 = "", depart2 = "", transit1 = "", transit2 = "", return1 = "", return2 = "";
            try
            {
                int TPax = 0, PaxAdult = 0, PaxChild = 0;
                Boolean returnFlight = false;
                returnFlight = objBooking.IsReturn(TransId, 0);
                Session["OneWay"] = !returnFlight;
                List<BookingTransactionDetail> listBookingDetail = new List<BookingTransactionDetail>();
                listBookingDetail = objBooking.GetAllBK_TRANSDTLFilterAll(TransId, 0);
                lstbookDTLInfo = objBooking.GetAllBK_TRANSDTLCombinePNR(TransId, 1, "LEN(RecordLocator) >= 6 AND ");
                if (lstbookDTLInfo != null && lstbookDTLInfo.Count > 0)
                {
                    PaxAdult = lstbookDTLInfo.Sum(item => item.PaxAdult);
                    PaxChild = lstbookDTLInfo.Sum(item => item.PaxChild);
                    TPax = PaxAdult + PaxChild;
                }
                if (listBookingDetail.Count != 0)
                {
                    for (int i = 0; i < listBookingDetail.Count; i++)
                    {
                        //if (listBookingDetail[i].SeqNo % 2 == 0)
                        //{
                        //    Session["OneWay"] = false;
                        //    //break;
                        //}
                        if (i == 0)
                        {
                            depart1 = listBookingDetail[i].Origin.Trim();
                            transit1 = listBookingDetail[i].Transit.Trim();
                            return1 = listBookingDetail[i].Destination.Trim();
                        }
                        else if (i == 1)
                        {
                            depart2 = listBookingDetail[i].Origin.Trim();
                            transit2 = listBookingDetail[i].Transit.Trim();
                            return2 = listBookingDetail[i].Destination.Trim();
                        }
                        else
                        {
                            //break;
                        }
                    }
                }
                num = TPax;
                if (listBookingDetail.Count != 0)
                {
                    //trim spaces
                    if (transit1 != "" || transit2 != "")
                    {
                        eFlight = EnumFlight.ConnectingFlight;
                    }
                    else
                    {
                        eFlight = EnumFlight.DirectFlight;
                    }
                }
                else if (Session["OneWay"] != null)
                {
                    if (Convert.ToBoolean(Session["OneWay"]) == false)
                    {
                        eFlight = EnumFlight.ConnectingFlight;
                    }
                    else
                    {
                        eFlight = EnumFlight.DirectFlight;
                    }
                }
                else
                {
                    //lblErr.Text = "Flight not found, please contact HelpDesk for further information.";
                    return false;
                }
                if (transit1 == "" && transit2 != "")
                {
                    Session["transitreturn"] = true;
                }
                else if (transit1 != "" && transit2 == "")
                {
                    Session["transitdepart"] = true;
                }
                else if (transit1 != "" && transit2 != "")
                {
                    Session["transitdepartreturn"] = true;
                }
                return true;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                return false;
            }
        }

        protected void LoadData(string TransID)
        {
            List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();


            bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(TransID);
            objBooking.FillDataTableTransMain(bookHDRInfo);

            //amended by diana 20131102 - hide disapproved record locator
            if (Session["NewBooking"].ToString() == "true")
                listDetailCombinePNR = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 0);
            else
                listDetailCombinePNR = objBooking.GetAllBK_TRANSDTLCombinePNR(TransID, 0, "LEN(RecordLocator) >= 6 AND ");

            foreach (BookingTransactionDetail a in listDetailCombinePNR)
            {
                a.SellKey = "";
            }

            objBooking.FillDataTableTransDetail(listDetailCombinePNR);

            Session["listDetailCombinePNR"] = listDetailCombinePNR;

            DataTable dtPassenger = default(DataTable);
            Session["dtPassengers"] = null;
            dtPassenger = objBooking.GetAllBK_PASSENGERLISTDataTableNew(TransId, true);
            if (Session["dtPassengers"] == null)
            {
                Session["dtPassengers"] = dtPassenger;
                //for (int i = 0; i < dtPassenger.Rows.Count; i++)
                //{
                //    rowPassenger = new PassengerData();
                //    rowPassenger.PassengerID = dtPassenger.Rows[i]["PassengerID"].ToString();
                //    rowPassenger.FirstName = dtPassenger.Rows[i]["FirstName"].ToString();
                //    rowPassenger.LastName = dtPassenger.Rows[i]["LastName"].ToString();
                //    rowPassenger.RecordLocator = dtPassenger.Rows[i]["PNR"].ToString().Trim();
                //    rowPassenger.DepartSeat = dtPassenger.Rows[i]["DepartSeatFull"].ToString().Trim();
                //    rowPassenger.ReturnSeat = dtPassenger.Rows[i]["ReturnSeatFull"].ToString().Trim();
                //    rowPassenger.DepartConnectingSeat = dtPassenger.Rows[i]["DepartConnectingSeatFull"].ToString().Trim();
                //    rowPassenger.ReturnConnectingSeat = dtPassenger.Rows[i]["ReturnConnectingSeatFull"].ToString().Trim();

                //    lstPassenger.Add(rowPassenger);

                //}
            }
        }

        //private void SetCookie()
        //{
        //    HttpCookie cookie = Request.Cookies["cookieTemFlight"];
        //    if (cookie != null)
        //    {
        //        if (IsNumeric(cookie.Values["list1ID"]))
        //        {
        //            departID = Convert.ToInt32(cookie.Values["list1ID"]);
        //        }
        //        else
        //        {
        //            departID = -1;
        //        }

        //        ReturnID = cookie.Values["ReturnID"];
        //        num = Convert.ToInt32(cookie.Values["PaxNum"]);
        //    }
        //}

        //private void BindModel()
        //{
        //    if (departID != -1)
        //    {
        //        model = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
        //        ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
        //        string strExpr;
        //        string strSort;
        //        DataTable dt = objBooking.dtFlight();
        //        strExpr = "TemFlightId = '" + departID + "'";
        //        strSort = "";
        //        dt = (DataTable)HttpContext.Current.Session["TempFlight"];
        //        DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
        //        FillModelFromDataRow(foundRows, ref model);


        //        if (ReturnID != "")
        //        {
        //            model2 = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
        //            strExpr = "TemFlightId = '" + ReturnID + "'";
        //            strSort = "";
        //            dt = (DataTable)HttpContext.Current.Session["TempFlight"];
        //            foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
        //            FillModelFromDataRow(foundRows, ref model2);

        //        }
        //    }
        //}

        //protected void FillModelFromDataRow(DataRow[] foundRows, ref ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model)
        //{
        //    try
        //    {
        //        if (IsNumeric(foundRows[0]["TemFlightId"].ToString()))
        //        { model.TemFlightId = Convert.ToInt16(foundRows[0]["TemFlightId"]); }
        //        model.TemFlightFlightNumber = foundRows[0]["TemFlightFlightNumber"].ToString();
        //        model.TemFlightDate = Convert.ToDateTime(foundRows[0]["TemFlightDate"]);
        //        model.TemFlightArrival = foundRows[0]["TemFlightArrival"].ToString();
        //        model.TemFlightCarrierCode = foundRows[0]["TemFlightCarrierCode"].ToString();
        //        model.TemFlightJourneySellKey = foundRows[0]["TemFlightJourneySellKey"].ToString();
        //        model.TemFlightCHDNum = Convert.ToInt16(foundRows[0]["TemFlightCHDNum"]);
        //        model.TemFlightCurrencyCode = foundRows[0]["TemFlightCurrencyCode"].ToString();
        //        model.TemFlightStd = Convert.ToDateTime(foundRows[0]["TemFlightStd"]);
        //        model.TemFlightDeparture = foundRows[0]["TemFlightDeparture"].ToString();
        //        model.TemFlightADTNum = Convert.ToInt16(foundRows[0]["TemFlightADTNum"]);
        //        model.TemFlightIfReturn = Convert.ToBoolean(foundRows[0]["TemFlightIfReturn"]);
        //        model.TemFlightPaxNum = Convert.ToInt16(foundRows[0]["TemFlightPaxNum"]);
        //        model.TemFlightSta = Convert.ToDateTime(foundRows[0]["TemFlightSta"]);
        //        model.TemFlightAgentName = foundRows[0]["TemFlightAgentName"].ToString();
        //        if (IsNumeric(foundRows[0]["TemFlightAveragePrice"].ToString()))
        //        { model.TemFlightAveragePrice = Convert.ToDecimal(foundRows[0]["TemFlightAveragePrice"]); }
        //        if (IsNumeric(foundRows[0]["TemFlightTotalAmount"].ToString()))
        //        { model.TemFlightTotalAmount = Convert.ToDecimal(foundRows[0]["TemFlightTotalAmount"]); }
        //        if (IsNumeric(foundRows[0]["TemFlightFarePrice"].ToString()))
        //        { model.temFlightfarePrice = Convert.ToDecimal(foundRows[0]["TemFlightFarePrice"]); }
        //        if (IsNumeric(foundRows[0]["TemFlightAPT"].ToString()))
        //        { model.TemFlightApt = Convert.ToDecimal(foundRows[0]["TemFlightAPT"]); }
        //        if (IsNumeric(foundRows[0]["TemFlightFuel"].ToString()))
        //        { model.TemFlightFuel = Convert.ToDecimal(foundRows[0]["TemFlightFuel"]); }
        //        model.TemFlightTransit = foundRows[0]["TemFlightTransit"].ToString();
        //        DateTime sta2;
        //        if (DateTime.TryParse(foundRows[0]["TemFlightSta2"].ToString(), out sta2))
        //            model.TemFlightSta2 = sta2;
        //        DateTime std2;
        //        if (DateTime.TryParse(foundRows[0]["TemFlightStd2"].ToString(), out std2))
        //            model.TemFlightStd2 = std2;
        //        model.TemFlightCarrierCode2 = foundRows[0]["TemFlightCarrierCode2"].ToString();
        //        model.TemFlightFlightNumber2 = foundRows[0]["TemFlightFlightNumber2"].ToString();
        //        model.TemFlightOpSuffix = foundRows[0]["TemFlightOpSuffix"].ToString();
        //        model.TemFlightOpSuffix2 = foundRows[0]["TemFlightOpSuffix2"].ToString();
        //        model.TemFlightSignature = foundRows[0]["TemFlightSignature"].ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(this, ex);
        //    }
        //}

        private void intSeatTabSession()
        {
            var profiler = MiniProfiler.Current;
            string SellSessionID = "";
            using (profiler.Step("Navitaire:AgentLogon"))
            {
                SellSessionID = absNavitaire.AgentLogon();
            }
            lstbookDTLInfo = objBooking.GetAllBK_TRANSDTLFilter(TransId, 0);
            BookingControl bookingControl = new BookingControl();
            GetBookingResponse resp = bookingControl.GetBookingByPNR(lstbookDTLInfo[0].RecordLocator, SellSessionID);
            Session["resp"] = resp;
            switch (eFlight)
            {
                case EnumFlight.ConnectingFlight:
                    if (resp == null == false)
                    {
                        if (CheckDefaultSeatExist(EnumFlightType.DepartConnectingFlight))
                        {
                            btnSeatDepart1.Value = resp.Booking.Journeys[0].Segments[0].DepartureStation + " - " + resp.Booking.Journeys[0].Segments[0].ArrivalStation;
                            btnSeatDepart1.Visible = true;
                            if (resp.Booking.Journeys[0].Segments.Length > 1)
                            {
                                btnSeatDepart2.Value = resp.Booking.Journeys[0].Segments[1].DepartureStation + " - " + resp.Booking.Journeys[0].Segments[1].ArrivalStation;
                                btnSeatDepart2.Visible = true;
                            }
                            else
                            {
                                btnSeatDepart2.Visible = false;
                            }


                        }
                        else
                        {
                            btnSeatDepart1.Visible = false;
                            btnSeatDepart2.Visible = false;
                        }
                    }
                    else
                    {
                        //added by ketee
                        btnSeatDepart1.Visible = false;
                        btnSeatDepart2.Visible = false;
                    }
                    if ((bool)Session["OneWay"] == false)
                    {
                        if (CheckDefaultSeatExist(EnumFlightType.ReturnConnectingFlight))
                        {
                            btnSeatReturn1.Value = resp.Booking.Journeys[1].Segments[0].DepartureStation + " - " + resp.Booking.Journeys[1].Segments[0].ArrivalStation;
                            btnSeatReturn1.Visible = true;
                            if (resp.Booking.Journeys[1].Segments.Length > 1)
                            {
                                btnSeatReturn2.Value = resp.Booking.Journeys[1].Segments[1].DepartureStation + " - " + resp.Booking.Journeys[1].Segments[1].ArrivalStation;
                                btnSeatReturn2.Visible = true;
                            }
                            else
                            {
                                btnSeatReturn2.Visible = false;
                            }

                        }
                        else
                        {
                            btnSeatReturn1.Visible = false;
                            btnSeatReturn2.Visible = false;
                        }

                    }
                    else
                    {
                        btnSeatReturn1.Visible = false;
                        btnSeatReturn2.Visible = false;
                    }
                    break;
                case EnumFlight.DirectFlight:
                    if (resp == null == false)
                    {
                        if (CheckDefaultSeatExist(EnumFlightType.DepartFlight))
                        {
                            btnSeatDepart1.Value = resp.Booking.Journeys[0].Segments[0].DepartureStation + " - " + resp.Booking.Journeys[0].Segments[0].ArrivalStation;
                            btnSeatDepart1.Visible = true;
                        }
                        else
                        {
                            btnSeatDepart1.Visible = false;
                        }

                    }
                    else
                    {
                        //added by ketee
                        btnSeatDepart1.Visible = false;
                    }
                    if ((bool)Session["OneWay"] == false)
                    {
                        if (CheckDefaultSeatExist(EnumFlightType.ReturnFlight))
                        {
                            btnSeatReturn1.Value = resp.Booking.Journeys[1].Segments[0].DepartureStation + " - " + resp.Booking.Journeys[1].Segments[0].ArrivalStation;
                            btnSeatReturn1.Visible = true;
                        }
                        else
                        {
                            btnSeatReturn1.Visible = false;
                        }


                    }
                    else
                    {
                        btnSeatReturn1.Visible = false;
                    }
                    btnSeatDepart2.Visible = false;
                    btnSeatReturn2.Visible = false;
                    break;
            }

        }

        private void ClearSession()
        {
            HttpContext.Current.Session.Remove("commit");
            Session["totalseatfeeallcommit"] = null;
            Session["totalseatfeeallcommitgoing"] = null;
            Session["totalseatfeeallcommitreturn"] = null;
            Session["totalamountdueseatfeeallcommit"] = null;
            Session["totalamountdueseatfeeallcommitgoing"] = null;
            Session["totalamountdueseatfeeallcommitreturn"] = null;
            Session["totalseatfeeall"] = null;
            Session["totalamountdueseatfeeall"] = null;
            Session["totalamountdueseatfeeallgoing"] = null;
            Session["totalamountdueseatfeeallreturn"] = null;
            Session["listDetailCombinePNR"] = null;
            Session["havebalance"] = null;
            Session["PNRsCommit"] = null;
            Session["assignSeatinfoDepartConnectingFlight"] = null;
            Session["assignSeatinfoDepartConnectingFlight2"] = null;
            Session["assignSeatinfoReturnConnectingFlight"] = null;
            Session["assignSeatinfoReturnConnectingFlight2"] = null;
            Session["assignSeatinfoDepartFlight"] = null;
            Session["assignSeatinfoReturnFlight"] = null;
            Session["additionalamount"] = null;
        }

        private void FillFlight(string RecordLocator, int btnSelected = 0)
        {
            var profiler = MiniProfiler.Current;
            MessageList msgList = new MessageList();
            ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");
            //ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking("");
            BookingControl bookingControl = new BookingControl();
            try
            {
                //dison
                ////if ((int)Session["LiftStatusDepart"] == 2)
                ////{
                ////    Session["DepartFlightInfo"] = null;
                ////}
                ////if ((int)Session["LiftStatusReturn"] == 2)
                ////{
                ////    Session["ReturnFlightInfo"] = null;
                ////}


                //call function
                string DepartXml = "";
                //"GetSeatAvailability_Response.xml"
                string ReturnXml = "";
                //"GetSeatAvailability_Response.xml"
                int DepartPax = 0;
                int ReturnPax = 0;
                string DepartDefaultSeat = "";
                //0_Y_1_2D,0_Y_1_3D
                string ReturnDefaultSeat = "";
                string DepartFromTo = "";
                //YOGYAKARTA (JOG) - JAKARTA (CGK)
                string DepartFromToShort = "";
                //JOG - CGK
                string DepartFromTo2 = "";
                //YOGYAKARTA (JOG) - JAKARTA (CGK)
                string DepartFromToShort2 = "";
                //JOG - CGK
                //QZ 7342
                string ReturnFromTo = "";
                string ReturnFromToShort = "";
                string ReturnFromTo2 = "";
                string ReturnFromToShort2 = "";
                string PassengerHeader = "";
                string PassengerSum = "";
                //added by ketee
                string hotseat = "";


                //Flight_Info pDepartFlightInfo = new Flight_Info();
                //Flight_Info pReturnFlightInfo = new Flight_Info();
                SeatAvailabilityResponse pAvailableSeatInfo = new SeatAvailabilityResponse();

                //string Signature = (string)Session["signature"];
                string SellSessionID = "";
                using (profiler.Step("Navitaire:AgentLogon"))
                {
                    SellSessionID = absNavitaire.AgentLogon();
                }
                lstbookDTLInfo = objBooking.GetAllBK_TRANSDTLFilter(TransId, 0);

                GetBookingResponse resp = bookingControl.GetBookingByPNR(lstbookDTLInfo[0].RecordLocator, SellSessionID);
                //string xmlstr = absNavitaire.GetXMLString(resp);
                switch (btnSelected)
                {
                    case 0:
                        btnSeatDepart1.Style[HtmlTextWriterStyle.BackgroundImage] = "../images/CBackground2.png";
                        break;
                    case 1:
                        btnSeatDepart2.Style[HtmlTextWriterStyle.BackgroundImage] = "../images/CBackground2.png";
                        break;
                    case 2:
                        btnSeatReturn1.Style[HtmlTextWriterStyle.BackgroundImage] = "../images/CBackground2.png";
                        break;
                    case 3:
                        btnSeatReturn2.Style[HtmlTextWriterStyle.BackgroundImage] = "../images/CBackground2.png";
                        break;
                }

                switch (eFlight)
                {
                    case EnumFlight.DirectFlight:


                        if (btnSelected == 0)
                        {
                            if (Session["SeatInfo0Xml"] == null == false)
                            {
                                DeleteXML((string)Session["SeatInfo0Xml"]);

                            }

                            pAvailableSeatInfo = absNavitaire.GetSeatAvailability(lstbookDTLInfo[0].RecordLocator, 0, SellSessionID, resp.Booking.Journeys[0].Segments[0].FlightDesignator.CarrierCode, resp.Booking.Journeys[0].Segments[0].FlightDesignator.FlightNumber,
                                resp.Booking.Journeys[0].Segments[0].FlightDesignator.OpSuffix, resp.Booking.Journeys[0].Segments[0].DepartureStation, resp.Booking.Journeys[0].Segments[0].ArrivalStation, resp.Booking.Journeys[0].Segments[0].STD, Request.PhysicalApplicationPath);
                            Session["pAvailableSeatInfo0"] = pAvailableSeatInfo;
                            //btnSeatDepart1.Style[HtmlTextWriterStyle.BackgroundImage] = "images/CBackground2.png";
                        }
                        else if (btnSelected == 2)
                        {
                            if (Session["SeatInfo1Xml"] == null == false)
                            {
                                DeleteXML((string)Session["SeatInfo1Xml"]);

                            }
                            pAvailableSeatInfo = absNavitaire.GetSeatAvailability(lstbookDTLInfo[0].RecordLocator, 0, SellSessionID, resp.Booking.Journeys[1].Segments[0].FlightDesignator.CarrierCode, resp.Booking.Journeys[1].Segments[0].FlightDesignator.FlightNumber,
                                resp.Booking.Journeys[1].Segments[0].FlightDesignator.OpSuffix, resp.Booking.Journeys[1].Segments[0].DepartureStation, resp.Booking.Journeys[1].Segments[0].ArrivalStation, resp.Booking.Journeys[1].Segments[0].STD, Request.PhysicalApplicationPath);
                            //Session["Click"] = Nothing
                            Session["pAvailableSeatInfo1"] = pAvailableSeatInfo;
                            //btnSeatReturn1.Style[HtmlTextWriterStyle.BackgroundImage] = "images/CBackground2.png";
                        }


                        if ((pAvailableSeatInfo != null))
                        {

                            if (!string.IsNullOrEmpty(absNavitaire.DepartXmlUrl))
                            {
                                DepartXml = absNavitaire.DepartXmlUrl;
                                if (btnSelected == 0)
                                {
                                    ////pDepartFlightInfo.XmlURL = DepartXml;
                                    //DepartXml = "test4.xml"
                                    DepartPax = num;
                                    DepartFromTo = resp.Booking.Journeys[0].Segments[0].DepartureStation + "-" + resp.Booking.Journeys[0].Segments[0].ArrivalStation;
                                    //DepartFromTo2 = pDepartFlightInfo.FromTo2
                                    DepartFromToShort = resp.Booking.Journeys[0].Segments[0].DepartureStation + "-" + resp.Booking.Journeys[0].Segments[0].ArrivalStation;
                                    //DepartFromToShort2 = pDepartFlightInfo.FromToShort2
                                }
                                else
                                {
                                    ////pReturnFlightInfo.XmlURL = DepartXml;
                                    //DepartXml = "test4.xml"
                                    DepartPax = num;
                                    ReturnFromTo = resp.Booking.Journeys[1].Segments[0].DepartureStation + "-" + resp.Booking.Journeys[1].Segments[0].ArrivalStation;
                                    //DepartFromTo2 = pDepartFlightInfo.FromTo2
                                    ReturnFromToShort = resp.Booking.Journeys[1].Segments[0].DepartureStation + "-" + resp.Booking.Journeys[1].Segments[0].ArrivalStation;
                                    //DepartFromToShort2 = pDepartFlightInfo.FromToShort2
                                }

                            }
                        }
                        else
                        {
                            lblErr.Text = msgList.Err999998;
                            pnlErr.Visible = true;
                            return;
                        }
                        //End If

                        if (!string.IsNullOrEmpty(DepartXml))
                        {
                            ctlDepart.Style["display"] = "";
                            PassengerHeader = "<div id='passengerListHeader" + btnSelected + "' class='redSectionHeader'>";
                            PassengerHeader += "<div>Seat summary</div></div>";
                            PassengerHeader += "<div id='passengerListBody" + btnSelected + "' class='sectionBody'><br/>";
                            if (btnSelected == 0)
                            {
                                PassengerSum = CreateSeatControl(DepartPax, DepartFromToShort, AssignSeatInfo(EnumFlightType.DepartFlight, RecordLocator, (SeatAvailabilityResponse)Session["pAvailableSeatInfo0"]), EnumFlightType.DepartFlight);
                            }
                            else if (btnSelected == 2)
                            {
                                PassengerSum = CreateSeatControl(ReturnPax, ReturnFromToShort, AssignSeatInfo(EnumFlightType.ReturnFlight, RecordLocator, (SeatAvailabilityResponse)Session["pAvailableSeatInfo1"]), EnumFlightType.ReturnFlight);
                            }

                            if (!string.IsNullOrEmpty(PassengerSum))
                            {
                                PassengerSummary.Style["display"] = "";
                                PassengerSummary.InnerHtml = PassengerHeader + PassengerSum + "</div>";
                            }

                            if (btnSelected == 0)
                            {
                                ss.SeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"];

                                Session["SeatInfo0Xml"] = DepartXml;
                            }
                            else if (btnSelected == 2)
                            {
                                ss.SeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"];

                                Session["SeatInfo1Xml"] = DepartXml;
                            }
                            if ((ss.SeatInfo != null))
                            {
                                foreach (ABS.Logic.GroupBooking.SeatInfo RowSeatInfo in ss.SeatInfo)
                                {
                                    if (string.IsNullOrEmpty(DepartDefaultSeat))
                                    {
                                        DepartDefaultSeat = "0_" + RowSeatInfo.CompartmentDesignator + "_" + RowSeatInfo.Deck + "_" + RowSeatInfo.SelectedSeat;
                                        //DepartDefaultSeat = "0_" + RowSeatInfo.CompartmentDesignator + "_" + RowSeatInfo.Deck + "_";
                                        //added by ketee hot seat
                                        hotseat = RowSeatInfo.IsHotSeat.ToString();
                                    }
                                    else
                                    {
                                        DepartDefaultSeat += ",0_" + RowSeatInfo.CompartmentDesignator + "_" + RowSeatInfo.Deck + "_" + RowSeatInfo.SelectedSeat;
                                        //DepartDefaultSeat += ",0_" + RowSeatInfo.CompartmentDesignator + "_" + RowSeatInfo.Deck + "_";
                                        //added by ketee
                                        hotseat += "," + RowSeatInfo.IsHotSeat;
                                    }
                                }
                            }
                            ss.defaultseat = DepartDefaultSeat;
                            //'remark by ketee, no default seat assign
                            ss.hotseat = hotseat;
                            ss.numberofpassenger = DepartPax;
                            ss.overwritepassengerindex = DepartPax;
                            ss.xmlurl = DepartXml;
                        }

                        break;
                    case EnumFlight.ConnectingFlight:
                        if (btnSeatDepart1.Visible == false & btnSeatDepart2.Visible == false)
                        {
                            if (btnSelected == 0)
                            {
                                btnSelected = 2;
                            }
                        }
                        if (btnSelected < 2)
                        {

                            if (resp != null)
                            {
                                //pDepartFlightInfo = Session["DepartFlightInfo"];

                                if (btnSelected == 0)
                                {
                                    if (Session["SeatInfo0Xml"] == null == false)
                                    {
                                        DeleteXML((string)Session["SeatInfo0Xml"]);

                                    }

                                    pAvailableSeatInfo = absNavitaire.GetSeatAvailability(lstbookDTLInfo[0].RecordLocator, 0, SellSessionID, resp.Booking.Journeys[0].Segments[0].FlightDesignator.CarrierCode, resp.Booking.Journeys[0].Segments[0].FlightDesignator.FlightNumber,
                                    resp.Booking.Journeys[0].Segments[0].FlightDesignator.OpSuffix, resp.Booking.Journeys[0].Segments[0].DepartureStation, resp.Booking.Journeys[0].Segments[0].ArrivalStation, resp.Booking.Journeys[0].Segments[0].STD, Request.PhysicalApplicationPath);
                                    //Session["Click"] = Nothing
                                    Session["pAvailableSeatInfo0"] = pAvailableSeatInfo;
                                    //btnSeatDepart1.Style[HtmlTextWriterStyle.BackgroundImage] = "images/CBackground2.png";
                                }
                                else if (btnSelected == 1)
                                {
                                    if (Session["SeatInfo1Xml"] == null == false)
                                    {
                                        DeleteXML((string)Session["SeatInfo1Xml"]);

                                    }
                                    pAvailableSeatInfo = absNavitaire.GetSeatAvailability(RecordLocator, 0, SellSessionID, resp.Booking.Journeys[0].Segments[1].FlightDesignator.CarrierCode, resp.Booking.Journeys[0].Segments[1].FlightDesignator.FlightNumber, resp.Booking.Journeys[0].Segments[1].FlightDesignator.OpSuffix,
                                                                                            resp.Booking.Journeys[0].Segments[1].DepartureStation, resp.Booking.Journeys[0].Segments[1].ArrivalStation, resp.Booking.Journeys[0].Segments[1].STD, Request.PhysicalApplicationPath);
                                    //Session["Click"] = Nothing
                                    Session["pAvailableSeatInfo1"] = pAvailableSeatInfo;
                                    //btnSeatDepart2.Style[HtmlTextWriterStyle.BackgroundImage] = "images/CBackground2.png";
                                }


                                if ((pAvailableSeatInfo != null))
                                {

                                    if (!string.IsNullOrEmpty(absNavitaire.DepartXmlUrl))
                                    {
                                        DepartXml = absNavitaire.DepartXmlUrl;
                                        //pDepartFlightInfo.XmlURL = DepartXml;
                                        //DepartXml = "test4.xml"
                                        DepartPax = num;
                                        DepartFromTo = resp.Booking.Journeys[0].Segments[0].DepartureStation + "-" + resp.Booking.Journeys[0].Segments[0].ArrivalStation;
                                        if (resp.Booking.Journeys[0].Segments.Length > 1)
                                        {
                                            DepartFromTo2 = resp.Booking.Journeys[0].Segments[1].DepartureStation + "-" + resp.Booking.Journeys[0].Segments[1].ArrivalStation;
                                        }
                                        DepartFromToShort = resp.Booking.Journeys[0].Segments[0].DepartureStation + "-" + resp.Booking.Journeys[0].Segments[0].ArrivalStation;
                                        if (resp.Booking.Journeys[0].Segments.Length > 1)
                                        {
                                            DepartFromToShort2 = resp.Booking.Journeys[0].Segments[1].DepartureStation + "-" + resp.Booking.Journeys[0].Segments[1].ArrivalStation;
                                        }
                                    }
                                }
                                else
                                {
                                    lblErr.Text = msgList.Err999998;
                                    pnlErr.Visible = true;
                                    return;
                                }
                                //End If



                            }
                            else
                            {
                            }


                            if (!string.IsNullOrEmpty(DepartXml))
                            {
                                ctlDepart.Style["display"] = "";


                                PassengerHeader = "<div id='passengerListHeader'" + btnSelected + " class='redSectionHeader'>";
                                PassengerHeader += "<div>Seat summary</div></div>";
                                PassengerHeader += "<div id='passengerListBody'" + btnSelected + " class='sectionBody'><br/>";
                                if (btnSelected == 0)
                                {
                                    PassengerSum = CreateSeatControl(DepartPax, DepartFromToShort, AssignSeatInfo(EnumFlightType.DepartFlight, RecordLocator, (SeatAvailabilityResponse)Session["pAvailableSeatInfo0"]), EnumFlightType.DepartFlight);
                                }
                                else if (btnSelected == 1)
                                {
                                    PassengerSum = CreateSeatControl(DepartPax, DepartFromToShort2, AssignSeatInfo(EnumFlightType.DepartConnectingFlight, RecordLocator, (SeatAvailabilityResponse)Session["pAvailableSeatInfo1"]), EnumFlightType.DepartConnectingFlight);
                                }

                                if (!string.IsNullOrEmpty(PassengerSum))
                                {
                                    PassengerSummary.Style["display"] = "";
                                    PassengerSummary.InnerHtml = PassengerHeader + PassengerSum + "</div>";
                                }

                                if (btnSelected == 0)
                                {
                                    ss.SeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"];

                                    Session["SeatInfo0Xml"] = DepartXml;
                                }
                                else if (btnSelected == 1)
                                {
                                    ss.SeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"];

                                    Session["SeatInfo1Xml"] = DepartXml;
                                }
                                if ((ss.SeatInfo != null))
                                {
                                    foreach (ABS.Logic.GroupBooking.SeatInfo RowSeatInfo in ss.SeatInfo)
                                    {
                                        //RowSeatInfo = RowSeatInfo_loopVariable;
                                        if (string.IsNullOrEmpty(DepartDefaultSeat))
                                        {
                                            //DepartDefaultSeat = "0_" & RowSeatInfo.CompartmentDesignator & "_1_" & RowSeatInfo.SelectedSeat
                                            DepartDefaultSeat = "0_" + RowSeatInfo.CompartmentDesignator + "_" + RowSeatInfo.Deck + "_" + RowSeatInfo.SelectedSeat;
                                            //added by ketee
                                            //hotseat = RowSeatInfo.IsHotSeat;
                                        }
                                        else
                                        {
                                            //DepartDefaultSeat &= ",0_" & RowSeatInfo.CompartmentDesignator & "_1_" & RowSeatInfo.SelectedSeat
                                            DepartDefaultSeat += ",0_" + RowSeatInfo.CompartmentDesignator + "_" + RowSeatInfo.Deck + "_" + RowSeatInfo.SelectedSeat;
                                            //added by ketee
                                            //hotseat += "," + RowSeatInfo.IsHotSeat;
                                        }
                                    }
                                }
                                ss.defaultseat = DepartDefaultSeat;
                                //'remark by ketee, no default seat assign
                                ss.hotseat = hotseat;
                                ss.numberofpassenger = DepartPax;
                                ss.overwritepassengerindex = DepartPax;
                                ss.xmlurl = DepartXml;


                            }

                        }
                        else if (btnSelected > 1)
                        {
                            if (((bool)Session["OneWay"] == false))
                            {
                                //pReturnFlightInfo = Session["ReturnFlightInfo"];
                                if (btnSelected == 2)
                                {
                                    if (Session["SeatInfo2Xml"] == null == false)
                                    {
                                        DeleteXML((string)Session["SeatInfo2Xml"]);

                                    }
                                    pAvailableSeatInfo = absNavitaire.GetSeatAvailability(RecordLocator, 0, SellSessionID, resp.Booking.Journeys[1].Segments[0].FlightDesignator.CarrierCode, resp.Booking.Journeys[1].Segments[0].FlightDesignator.FlightNumber, resp.Booking.Journeys[1].Segments[0].FlightDesignator.OpSuffix,
                                                                                            resp.Booking.Journeys[1].Segments[0].DepartureStation, resp.Booking.Journeys[1].Segments[0].ArrivalStation, resp.Booking.Journeys[1].Segments[0].STD, Request.PhysicalApplicationPath);

                                    //Session["Click"] = Nothing
                                    Session["pAvailableSeatInfo2"] = pAvailableSeatInfo;
                                    //btnSeatReturn1.Style[HtmlTextWriterStyle.BackgroundImage] = "images/CBackground2.png";
                                }
                                else if (btnSelected == 3)
                                {
                                    if (Session["SeatInfo3Xml"] == null == false)
                                    {
                                        DeleteXML((string)Session["SeatInfo3Xml"]);

                                    }
                                    pAvailableSeatInfo = absNavitaire.GetSeatAvailability(RecordLocator, 0, SellSessionID, resp.Booking.Journeys[1].Segments[1].FlightDesignator.CarrierCode, resp.Booking.Journeys[1].Segments[1].FlightDesignator.FlightNumber, resp.Booking.Journeys[1].Segments[1].FlightDesignator.OpSuffix,
                                                                                                                                 resp.Booking.Journeys[1].Segments[1].DepartureStation, resp.Booking.Journeys[1].Segments[1].ArrivalStation, resp.Booking.Journeys[1].Segments[1].STD, Request.PhysicalApplicationPath);
                                    //Session["Click"] = Nothing
                                    Session["pAvailableSeatInfo3"] = pAvailableSeatInfo;
                                    //btnSeatReturn2.Style[HtmlTextWriterStyle.BackgroundImage] = "images/CBackground2.png";
                                }

                                if ((pAvailableSeatInfo != null))
                                {
                                    if (!string.IsNullOrEmpty(absNavitaire.DepartXmlUrl))
                                    {
                                        ReturnXml = absNavitaire.DepartXmlUrl;
                                        //pReturnFlightInfo.XmlURL = ReturnXml;
                                        //DepartXml = "test4.xml"
                                        ReturnPax = num;
                                        ReturnFromTo = resp.Booking.Journeys[1].Segments[0].DepartureStation + "-" + resp.Booking.Journeys[1].Segments[0].ArrivalStation;
                                        if (resp.Booking.Journeys[1].Segments.Length > 1)
                                        {
                                            ReturnFromTo2 = resp.Booking.Journeys[1].Segments[1].DepartureStation + "-" + resp.Booking.Journeys[1].Segments[1].ArrivalStation;
                                        }
                                        ReturnFromToShort = resp.Booking.Journeys[1].Segments[0].DepartureStation + "-" + resp.Booking.Journeys[1].Segments[0].ArrivalStation;
                                        if (resp.Booking.Journeys[1].Segments.Length > 1)
                                        {
                                            ReturnFromToShort2 = resp.Booking.Journeys[1].Segments[1].DepartureStation + "-" + resp.Booking.Journeys[1].Segments[1].ArrivalStation;
                                        }
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                                else
                                {
                                    lblErr.Text = msgList.Err999998;
                                    pnlErr.Visible = true;
                                    return;
                                }
                            }

                            if (!string.IsNullOrEmpty(ReturnXml))
                            {
                                ctlDepart.Style["display"] = "";


                                PassengerHeader = "<div id=\"passengerListHeader" + btnSelected + " class=\"redSectionHeader\">";
                                PassengerHeader += "<div>Seat summary</div></div>";
                                PassengerHeader += "<div id=\"passengerListBody" + btnSelected + "class=\"sectionBody\"><br/>";
                                if (btnSelected == 2)
                                {
                                    PassengerSum = CreateSeatControl(ReturnPax, ReturnFromToShort, AssignSeatInfo(EnumFlightType.ReturnFlight, RecordLocator, (SeatAvailabilityResponse)Session["pAvailableSeatInfo2"]), EnumFlightType.ReturnFlight);
                                }
                                else if (btnSelected == 3)
                                {
                                    PassengerSum = CreateSeatControl(ReturnPax, ReturnFromToShort2, AssignSeatInfo(EnumFlightType.ReturnConnectingFlight, RecordLocator, (SeatAvailabilityResponse)Session["pAvailableSeatInfo3"]), EnumFlightType.ReturnConnectingFlight);
                                }

                                if (!string.IsNullOrEmpty(PassengerSum))
                                {
                                    PassengerSummary.Style["display"] = "";
                                    PassengerSummary.InnerHtml = PassengerHeader + PassengerSum + "</div>";
                                }

                                if (btnSelected == 2)
                                {
                                    ss.SeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"];

                                    Session["SeatInfo2Xml"] = ReturnXml;
                                }
                                else if (btnSelected == 3)
                                {
                                    ss.SeatInfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo3"];

                                    Session["SeatInfo3Xml"] = ReturnXml;
                                }
                                if ((ss.SeatInfo != null))
                                {
                                    foreach (ABS.Logic.GroupBooking.SeatInfo RowSeatInfo in ss.SeatInfo)
                                    {
                                        //RowSeatInfo = RowSeatInfo_loopVariable;
                                        if (string.IsNullOrEmpty(ReturnDefaultSeat))
                                        {
                                            ReturnDefaultSeat = "0_" + RowSeatInfo.CompartmentDesignator + "_" + RowSeatInfo.Deck + "_" + RowSeatInfo.SelectedSeat;
                                        }
                                        else
                                        {
                                            ReturnDefaultSeat += ",0_" + RowSeatInfo.CompartmentDesignator + "_" + RowSeatInfo.Deck + "_" + RowSeatInfo.SelectedSeat;
                                        }
                                    }
                                }
                                ss.defaultseat = ReturnDefaultSeat;
                                //'remark by ketee, no default seat assign
                                ss.hotseat = hotseat;
                                ss.numberofpassenger = ReturnPax;
                                ss.overwritepassengerindex = ReturnPax;
                                ss.xmlurl = ReturnXml;

                            }
                        }
                        break;
                }



                if (Session["ErrorMsg"] == null == false)
                {
                    lblErr.Text = Session["ErrorMsg"].ToString();
                    pnlErr.Visible = true;
                    Session["ErrorMsg"] = null;
                }
                else
                {
                    pnlErr.Visible = false;
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
                Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.InvalidPage);
            }
        }

        private List<ABS.Logic.GroupBooking.SeatInfo> AssignSeatInfo(EnumFlightType flightType, string recordLocator, SeatAvailabilityResponse pAvailableSeatInfo)
        {
            List<ABS.Logic.GroupBooking.SeatInfo> seatInfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
            ABS.Logic.GroupBooking.SeatInfo seatInfo1 = new ABS.Logic.GroupBooking.SeatInfo();
            DataTable dtPassenger = default(DataTable);
            DataTable dtInfant = default(DataTable);
            ////AirAsia.Logic.FI_Booking_Logic FI_Booking_Logic = new AirAsia.Logic.FI_Booking_Logic();
            //dtPassenger = FI_Booking_Logic.getAllBookingPassenger(recordLocator);
            //change to new add-On table, Tyas
            //dtPassenger = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTable(TransId);
            dtPassenger = objBooking.GetAllBK_PASSENGERLISTDataTableNew(TransId);
            dtInfant = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTableInfant(TransId);

            if (dtInfant != null && dtInfant.Rows.Count > 0)
            {
                Session["dtInfant"] = dtInfant;
            }
            string CompartmentDesignator = "", Deck = "", SeatSet = "";
            int IsHotSeat = 0;

            int i = 0;

            switch (eFlight)
            {
                case EnumFlight.ConnectingFlight:
                    switch (flightType)
                    {
                        case EnumFlightType.DepartFlight:
                            if ((dtPassenger != null))
                            {
                                lstPassengerContainer = new List<PassengerContainer>();
                                foreach (DataRow drRow in dtPassenger.Rows)
                                {
                                    objPassengerContainer = new PassengerContainer();
                                    //drRow = drRow_loopVariable;
                                    seatInfo1 = new ABS.Logic.GroupBooking.SeatInfo();
                                    seatInfo1.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                    switch (drRow["Title"].ToString().ToUpper())
                                    {
                                        case "INFT":
                                            seatInfo1.PaxType = "INFT";
                                            break;
                                        case "CHD":
                                            seatInfo1.PaxType = "CHD";
                                            break;
                                        default:
                                            seatInfo1.PaxType = "ADT";
                                            break;
                                    }
                                    seatInfo1.PassengerID = drRow["PassengerID"].ToString();
                                    seatInfo1.PassengerNumber = Convert.ToInt32(drRow["PassengerID"].ToString());
                                    seatInfo1.Seq = i;
                                    seatInfo1.RecordLocator = drRow["PNR"].ToString();
                                    seatInfo1.CurrentSeat = drRow["DepartSeat"].ToString();

                                    CompartmentDesignator = "";
                                    Deck = "";
                                    SeatSet = "";
                                    IsHotSeat = 0;

                                    if ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"] != null)
                                    {
                                        seatInfo1.SelectedSeat = getPassengerDefaultSeat(drRow["PNR"].ToString(), seatInfo1.PassengerID, EnumFlightType.DepartFlight, (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"], ref CompartmentDesignator, ref Deck, ref SeatSet, ref IsHotSeat, i);
                                        seatInfo1.CompartmentDesignator = CompartmentDesignator;
                                        seatInfo1.Deck = Deck;
                                    }
                                    else
                                    {
                                        seatInfo1.SelectedSeat = drRow["DepartSeat"].ToString();
                                        seatInfo1.CompartmentDesignator = drRow["CompartmentDesignatorDepartSeat"].ToString();
                                        seatInfo1.Deck = drRow["DeckDepartSeat"].ToString();
                                    }
                                    //objPassengerContainer.setUnitDesignator()
                                    seatInfo1.SeatSet = SeatSet;
                                    seatInfo1.IsHotSeat = IsHotSeat;
                                    //assign default seat
                                    //With pAvailableSeatInfo
                                    //    For Each pSeatInfo As AABookingManager.SeatInfo In .EquipmentInfos(0).Compartments(0).Seats
                                    //        With pSeatInfo
                                    //            If .Assignable = True And .SeatAvailability.ToString.ToUpper = "OPEN" Then
                                    //                Dim SeatDesignator As String = .SeatDesignator
                                    //                seatInfo1.CompartmentDesignator = pAvailableSeatInfo.EquipmentInfos(0).Compartments(0).CompartmentDesignator
                                    //                seatInfo1.SelectedSeat = SeatDesignator

                                    //                Exit For
                                    //            End If
                                    //        End With
                                    //    Next
                                    //End With
                                    objPassengerContainer.setUnitDesignator(drRow["DepartSeat"].ToString(), (PassengerContainer.FlightType.Depart));
                                    objPassengerContainer.RecordLocator = seatInfo1.RecordLocator;
                                    objPassengerContainer.CompartmentDesignator = seatInfo1.CompartmentDesignator;
                                    objPassengerContainer.PassengerID = Convert.ToInt64(seatInfo1.PassengerID);
                                    objPassengerContainer.PassengerNumber = seatInfo1.PassengerNumber;

                                    seatInfo.Add(seatInfo1);

                                    i = i + 1;
                                    lstPassengerContainer.Add(objPassengerContainer);
                                }
                                if (lstPassengerContainer != null)
                                {
                                    HttpContext.Current.Session.Remove("PassengerContainer");
                                    HttpContext.Current.Session.Add("PassengerContainer", lstPassengerContainer);
                                }
                            }
                            else
                            {
                                return null;
                            }

                            ss.SeatInfo = seatInfo;
                            Session["SeatInfo0"] = seatInfo;
                            //added by ketee, 20170310, set existing seatinfo
                            if (Session["DepartConnectingExistingSeatInfo"] == null && seatInfo != null)
                                Session["DepartConnectingExistingSeatInfo"] = seatInfo;
                            break;
                        case EnumFlightType.DepartConnectingFlight:
                            if ((dtPassenger != null))
                            {
                                if (Session["PassengerContainer"] != null)
                                {
                                    lstPassengerContainer = (List<PassengerContainer>)Session["PassengerContainer"];
                                }
                                foreach (DataRow drRow in dtPassenger.Rows)
                                {
                                    //drRow = drRow_loopVariable;
                                    seatInfo1 = new ABS.Logic.GroupBooking.SeatInfo();
                                    seatInfo1.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                    switch (drRow["Title"].ToString().ToUpper())
                                    {
                                        case "INFT":
                                            seatInfo1.PaxType = "INFT";
                                            break;
                                        case "CHD":
                                            seatInfo1.PaxType = "CHD";
                                            break;
                                        default:
                                            seatInfo1.PaxType = "ADT";
                                            break;
                                    }
                                    seatInfo1.PassengerID = drRow["PassengerID"].ToString();
                                    seatInfo1.PassengerNumber = Convert.ToInt32(drRow["PassengerID"].ToString());
                                    seatInfo1.Seq = i;
                                    seatInfo1.RecordLocator = drRow["PNR"].ToString();
                                    seatInfo1.CurrentSeat = drRow["DepartConnectingSeat"].ToString();

                                    CompartmentDesignator = "";
                                    Deck = "";
                                    SeatSet = "";
                                    IsHotSeat = 0;

                                    if ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"] != null)
                                    {
                                        seatInfo1.SelectedSeat = getPassengerDefaultSeat(drRow["PNR"].ToString(), seatInfo1.PassengerID, EnumFlightType.DepartConnectingFlight, (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"], ref CompartmentDesignator, ref Deck, ref SeatSet, ref IsHotSeat, i);
                                        seatInfo1.CompartmentDesignator = CompartmentDesignator;
                                        seatInfo1.Deck = Deck;
                                    }
                                    else
                                    {
                                        seatInfo1.SelectedSeat = drRow["DepartConnectingSeat"].ToString();
                                        seatInfo1.CompartmentDesignator = drRow["CompartmentDesignatorDepartConnectingSeat"].ToString();
                                        seatInfo1.Deck = drRow["DeckDepartConnectingSeat"].ToString();
                                    }
                                    seatInfo1.SeatSet = SeatSet;
                                    seatInfo1.IsHotSeat = IsHotSeat;

                                    //assign default seat
                                    //With pAvailableSeatInfo
                                    //    For Each pSeatInfo As AABookingManager.SeatInfo In .EquipmentInfos(0).Compartments(0).Seats
                                    //        With pSeatInfo
                                    //            If .Assignable = True And .SeatAvailability.ToString.ToUpper = "OPEN" Then
                                    //                Dim SeatDesignator As String = .SeatDesignator
                                    //                seatInfo1.CompartmentDesignator = pAvailableSeatInfo.EquipmentInfos(0).Compartments(0).CompartmentDesignator
                                    //                seatInfo1.SelectedSeat = SeatDesignator

                                    //                Exit For
                                    //            End If
                                    //        End With
                                    //    Next
                                    //End With

                                    seatInfo.Add(seatInfo1);
                                    lstPassengerContainer[i].setUnitDesignator(drRow["DepartConnectingSeat"].ToString(), (PassengerContainer.FlightType.ConnectingDepart));
                                    i = i + 1;
                                }
                                if (lstPassengerContainer != null)
                                {
                                    HttpContext.Current.Session.Remove("PassengerContainer");
                                    HttpContext.Current.Session.Add("PassengerContainer", lstPassengerContainer);
                                }
                            }
                            else
                            {
                                return null;
                            }

                            ss.SeatInfo = seatInfo;
                            Session["SeatInfo1"] = seatInfo;
                            //added by ketee, 20170310, set existing seatinfo
                            if (Session["DepartConnectingExistingSeatInfo2"] == null && seatInfo != null)
                                Session["DepartConnectingExistingSeatInfo2"] = seatInfo;
                            break;
                        case EnumFlightType.ReturnFlight:
                            if ((dtPassenger != null))
                            {
                                if (Session["PassengerContainer"] != null)
                                {
                                    lstPassengerContainer = (List<PassengerContainer>)Session["PassengerContainer"];
                                }
                                foreach (DataRow drRow in dtPassenger.Rows)
                                {
                                    //drRow = drRow_loopVariable;
                                    seatInfo1 = new ABS.Logic.GroupBooking.SeatInfo();
                                    seatInfo1.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                    switch (drRow["Title"].ToString().ToUpper())
                                    {
                                        case "INFT":
                                            seatInfo1.PaxType = "INFT";
                                            break;
                                        case "CHD":
                                            seatInfo1.PaxType = "CHD";
                                            break;
                                        default:
                                            seatInfo1.PaxType = "ADT";
                                            break;
                                    }
                                    seatInfo1.PassengerID = drRow["PassengerID"].ToString();
                                    seatInfo1.PassengerNumber = Convert.ToInt32(drRow["PassengerID"].ToString());
                                    seatInfo1.Seq = i;
                                    seatInfo1.RecordLocator = drRow["PNR"].ToString();
                                    seatInfo1.CurrentSeat = drRow["ReturnSeat"].ToString();

                                    CompartmentDesignator = "";
                                    Deck = "";
                                    SeatSet = "";
                                    IsHotSeat = 0;

                                    if ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"] != null)
                                    {
                                        seatInfo1.SelectedSeat = getPassengerDefaultSeat(drRow["PNR"].ToString(), seatInfo1.PassengerID, EnumFlightType.ReturnFlight, (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"], ref CompartmentDesignator, ref Deck, ref SeatSet, ref IsHotSeat, i);
                                        seatInfo1.CompartmentDesignator = CompartmentDesignator;
                                        seatInfo1.Deck = Deck;
                                    }
                                    else
                                    {
                                        seatInfo1.SelectedSeat = drRow["ReturnSeat"].ToString();
                                        seatInfo1.CompartmentDesignator = drRow["CompartmentDesignatorReturnSeat"].ToString();
                                        seatInfo1.Deck = drRow["DeckReturnSeat"].ToString();
                                    }

                                    seatInfo1.SeatSet = SeatSet;
                                    seatInfo1.IsHotSeat = IsHotSeat;

                                    ////seatInfo1.SelectedSeat = getPassengerDefaultSeat(recordLocator, _QueueCode, seatInfo1.PassengerID, Common.EnumFlightType.ReturnFlight, Session["SeatInfo2"], seatInfo1.CompartmentDesignator, seatInfo1.Deck, seatInfo1.SeatSet, seatInfo1.IsHotSeat, i);
                                    //assign default seat
                                    //With pAvailableSeatInfo
                                    //    For Each pSeatInfo As AABookingManager.SeatInfo In .EquipmentInfos(0).Compartments(0).Seats
                                    //        With pSeatInfo
                                    //            If .Assignable = True And .SeatAvailability.ToString.ToUpper = "OPEN" Then
                                    //                Dim SeatDesignator As String = .SeatDesignator
                                    //                seatInfo1.CompartmentDesignator = pAvailableSeatInfo.EquipmentInfos(0).Compartments(0).CompartmentDesignator
                                    //                seatInfo1.SelectedSeat = SeatDesignator

                                    //                Exit For
                                    //            End If
                                    //        End With
                                    //    Next
                                    //End With

                                    seatInfo.Add(seatInfo1);
                                    lstPassengerContainer[i].setUnitDesignator(drRow["ReturnSeat"].ToString(), (PassengerContainer.FlightType.Return));
                                    i = i + 1;
                                }
                                if (lstPassengerContainer != null)
                                {
                                    HttpContext.Current.Session.Remove("PassengerContainer");
                                    HttpContext.Current.Session.Add("PassengerContainer", lstPassengerContainer);
                                }
                            }
                            else
                            {
                                return null;
                            }

                            ss.SeatInfo = seatInfo;
                            Session["SeatInfo2"] = seatInfo;
                            //added by ketee, 20170310, set existing seatinfo
                            if (Session["ReturnConnectingExistingSeatInfo"] == null && seatInfo != null)
                                Session["ReturnConnectingExistingSeatInfo"] = seatInfo;
                            break;
                        case EnumFlightType.ReturnConnectingFlight:
                            if ((dtPassenger != null))
                            {
                                if (Session["PassengerContainer"] != null)
                                {
                                    lstPassengerContainer = (List<PassengerContainer>)Session["PassengerContainer"];
                                }
                                foreach (DataRow drRow in dtPassenger.Rows)
                                {
                                    //drRow = drRow_loopVariable;
                                    seatInfo1 = new ABS.Logic.GroupBooking.SeatInfo();
                                    seatInfo1.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                    switch (drRow["Title"].ToString().ToUpper())
                                    {
                                        case "INFT":
                                            seatInfo1.PaxType = "INFT";
                                            break;
                                        case "CHD":
                                            seatInfo1.PaxType = "CHD";
                                            break;
                                        default:
                                            seatInfo1.PaxType = "ADT";
                                            break;
                                    }
                                    seatInfo1.PassengerID = drRow["PassengerID"].ToString();
                                    seatInfo1.PassengerNumber = Convert.ToInt32(drRow["PassengerID"].ToString());
                                    seatInfo1.Seq = i;
                                    seatInfo1.RecordLocator = drRow["PNR"].ToString();
                                    seatInfo1.CurrentSeat = drRow["ReturnConnectingSeat"].ToString();

                                    CompartmentDesignator = "";
                                    Deck = "";
                                    SeatSet = "";
                                    IsHotSeat = 0;

                                    if ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo3"] != null)
                                    {
                                        seatInfo1.SelectedSeat = getPassengerDefaultSeat(drRow["PNR"].ToString(), seatInfo1.PassengerID, EnumFlightType.ReturnConnectingFlight, (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo3"], ref CompartmentDesignator, ref Deck, ref SeatSet, ref IsHotSeat, i);
                                        seatInfo1.CompartmentDesignator = CompartmentDesignator;
                                        seatInfo1.Deck = Deck;
                                    }
                                    else
                                    {
                                        seatInfo1.SelectedSeat = drRow["ReturnConnectingSeat"].ToString();
                                        seatInfo1.CompartmentDesignator = drRow["CompartmentDesignatorReturnConnectingSeat"].ToString();
                                        seatInfo1.Deck = drRow["DeckReturnConnectingSeat"].ToString();
                                    }



                                    seatInfo1.SeatSet = SeatSet;
                                    seatInfo1.IsHotSeat = IsHotSeat;

                                    ////seatInfo1.SelectedSeat = getPassengerDefaultSeat(recordLocator, _QueueCode, seatInfo1.PassengerID, Common.EnumFlightType.ReturnConnectingFlight, Session["SeatInfo3"], seatInfo1.CompartmentDesignator, seatInfo1.Deck, seatInfo1.SeatSet, seatInfo1.IsHotSeat, i);
                                    //assign default seat
                                    //With pAvailableSeatInfo
                                    //    For Each pSeatInfo As AABookingManager.SeatInfo In .EquipmentInfos(0).Compartments(0).Seats
                                    //        With pSeatInfo
                                    //            If .Assignable = True And .SeatAvailability.ToString.ToUpper = "OPEN" Then
                                    //                Dim SeatDesignator As String = .SeatDesignator
                                    //                seatInfo1.CompartmentDesignator = pAvailableSeatInfo.EquipmentInfos(0).Compartments(0).CompartmentDesignator
                                    //                seatInfo1.SelectedSeat = SeatDesignator

                                    //                Exit For
                                    //            End If
                                    //        End With
                                    //    Next
                                    //End With

                                    seatInfo.Add(seatInfo1);
                                    lstPassengerContainer[i].setUnitDesignator(drRow["ReturnConnectingSeat"].ToString(), (PassengerContainer.FlightType.ConnectingReturn));
                                    i = i + 1;
                                }
                                if (lstPassengerContainer != null)
                                {
                                    HttpContext.Current.Session.Remove("PassengerContainer");
                                    HttpContext.Current.Session.Add("PassengerContainer", lstPassengerContainer);
                                }
                            }
                            else
                            {
                                return null;
                            }

                            ss.SeatInfo = seatInfo;
                            Session["SeatInfo3"] = seatInfo;
                            //added by ketee, 20170310, set existing seatinfo
                            if (Session["ReturnConnectingExistingSeatInfo2"] == null && seatInfo != null)
                                Session["ReturnConnectingExistingSeatInfo2"] = seatInfo;
                            break;
                    }
                    break;
                case EnumFlight.DirectFlight:
                    switch (flightType)
                    {
                        case EnumFlightType.DepartFlight:
                            if ((dtPassenger != null))
                            {
                                lstPassengerContainer = new List<PassengerContainer>();
                                foreach (DataRow drRow in dtPassenger.Rows)
                                {
                                    objPassengerContainer = new PassengerContainer();
                                    //drRow = drRow_loopVariable;
                                    seatInfo1 = new ABS.Logic.GroupBooking.SeatInfo();
                                    seatInfo1.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                    switch (drRow["Title"].ToString().ToUpper())
                                    {
                                        case "INFT":
                                            seatInfo1.PaxType = "INFT";
                                            break;
                                        case "CHD":
                                            seatInfo1.PaxType = "CHD";
                                            break;
                                        default:
                                            seatInfo1.PaxType = "ADT";
                                            break;
                                    }
                                    seatInfo1.PassengerID = drRow["PassengerID"].ToString();
                                    seatInfo1.PassengerNumber = Convert.ToInt32(drRow["PassengerID"].ToString());
                                    seatInfo1.Seq = i;
                                    seatInfo1.RecordLocator = drRow["PNR"].ToString();
                                    seatInfo1.CurrentSeat = drRow["DepartSeat"].ToString();

                                    CompartmentDesignator = "";
                                    Deck = "";
                                    SeatSet = "";
                                    IsHotSeat = 0;

                                    if ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"] != null)
                                    {
                                        seatInfo1.SelectedSeat = getPassengerDefaultSeat(drRow["PNR"].ToString(), seatInfo1.PassengerID, EnumFlightType.DepartFlight, (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"], ref CompartmentDesignator, ref Deck, ref SeatSet, ref IsHotSeat, i);
                                        seatInfo1.CompartmentDesignator = CompartmentDesignator;
                                        seatInfo1.Deck = Deck;
                                    }
                                    else
                                    {
                                        seatInfo1.SelectedSeat = drRow["DepartSeat"].ToString();
                                        seatInfo1.CompartmentDesignator = drRow["CompartmentDesignatorDepartSeat"].ToString();
                                        seatInfo1.Deck = drRow["DeckDepartSeat"].ToString();
                                    }
                                    seatInfo1.SeatSet = SeatSet;
                                    seatInfo1.IsHotSeat = IsHotSeat;

                                    ////seatInfo1.SelectedSeat = getPassengerDefaultSeat(recordLocator, seatInfo1.PassengerID, EnumFlightType.DepartFlight, Session["SeatInfo0"], seatInfo1.CompartmentDesignator, seatInfo1.Deck, seatInfo1.SeatSet, seatInfo1.IsHotSeat, i);
                                    objPassengerContainer.setUnitDesignator(drRow["DepartSeat"].ToString(), (PassengerContainer.FlightType.Depart));
                                    objPassengerContainer.RecordLocator = seatInfo1.RecordLocator;
                                    objPassengerContainer.CompartmentDesignator = seatInfo1.CompartmentDesignator;
                                    objPassengerContainer.PassengerID = Convert.ToInt64(seatInfo1.PassengerID);
                                    objPassengerContainer.PassengerNumber = seatInfo1.PassengerNumber;
                                    seatInfo.Add(seatInfo1);
                                    i = i + 1;
                                    lstPassengerContainer.Add(objPassengerContainer);
                                }
                                if (lstPassengerContainer != null)
                                {
                                    HttpContext.Current.Session.Remove("PassengerContainer");
                                    HttpContext.Current.Session.Add("PassengerContainer", lstPassengerContainer);
                                }
                            }
                            else
                            {
                                return null;
                            }

                            ss.SeatInfo = seatInfo;
                            Session["SeatInfo0"] = seatInfo;
                            //added by ketee, add existing seat info, 20170310
                            if (Session["DepartExistingSeatInfo"] == null && seatInfo != null)
                            {
                                Session["DepartExistingSeatInfo"] = seatInfo;
                            }

                            break;

                        case EnumFlightType.ReturnFlight:
                            if ((dtPassenger != null))
                            {
                                if (Session["PassengerContainer"] != null)
                                {
                                    lstPassengerContainer = (List<PassengerContainer>)Session["PassengerContainer"];
                                }
                                foreach (DataRow drRow in dtPassenger.Rows)
                                {

                                    //drRow = drRow_loopVariable;
                                    seatInfo1 = new ABS.Logic.GroupBooking.SeatInfo();
                                    seatInfo1.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                    switch (drRow["Title"].ToString().ToUpper())
                                    {
                                        case "INFT":
                                            seatInfo1.PaxType = "INFT";
                                            break;
                                        case "CHD":
                                            seatInfo1.PaxType = "CHD";
                                            break;
                                        default:
                                            seatInfo1.PaxType = "ADT";
                                            break;
                                    }
                                    seatInfo1.PassengerID = drRow["PassengerID"].ToString();
                                    seatInfo1.PassengerNumber = Convert.ToInt32(drRow["PassengerID"].ToString());
                                    seatInfo1.Seq = i;
                                    seatInfo1.RecordLocator = drRow["PNR"].ToString();
                                    seatInfo1.CurrentSeat = drRow["ReturnSeat"].ToString();

                                    CompartmentDesignator = "";
                                    Deck = "";
                                    SeatSet = "";
                                    IsHotSeat = 0;

                                    if ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"] != null)
                                    {
                                        seatInfo1.SelectedSeat = getPassengerDefaultSeat(drRow["PNR"].ToString(), seatInfo1.PassengerID, EnumFlightType.ReturnFlight, (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"], ref CompartmentDesignator, ref Deck, ref SeatSet, ref IsHotSeat, i);
                                        seatInfo1.CompartmentDesignator = CompartmentDesignator;
                                        seatInfo1.Deck = Deck;
                                    }
                                    else
                                    {
                                        seatInfo1.SelectedSeat = drRow["ReturnSeat"].ToString();
                                        seatInfo1.CompartmentDesignator = drRow["CompartmentDesignatorReturnSeat"].ToString();
                                        seatInfo1.Deck = drRow["DeckReturnSeat"].ToString();
                                    }
                                    seatInfo1.SeatSet = SeatSet;
                                    seatInfo1.IsHotSeat = IsHotSeat;

                                    ////seatInfo1.SelectedSeat = getPassengerDefaultSeat(recordLocator, _QueueCode, seatInfo1.PassengerID, Common.EnumFlightType.ReturnFlight, Session["SeatInfo1"], seatInfo1.CompartmentDesignator, seatInfo1.Deck, seatInfo1.SeatSet, seatInfo1.IsHotSeat, i);

                                    seatInfo.Add(seatInfo1);
                                    lstPassengerContainer[i].setUnitDesignator(drRow["ReturnSeat"].ToString(), (PassengerContainer.FlightType.Return));
                                    i = i + 1;
                                }
                                if (lstPassengerContainer != null)
                                {
                                    HttpContext.Current.Session.Remove("PassengerContainer");
                                    HttpContext.Current.Session.Add("PassengerContainer", lstPassengerContainer);
                                }
                            }
                            else
                            {
                                return null;
                            }

                            ss.SeatInfo = seatInfo;
                            Session["SeatInfo1"] = seatInfo;
                            //added by ketee, add existing seat info, 20170310
                            if (Session["ReturnExistingSeatInfo"] == null && seatInfo != null)
                            {
                                Session["ReturnExistingSeatInfo"] = seatInfo;
                            }
                            break;

                    }
                    break;
            }




            return seatInfo;
        }

        private string getPassengerDefaultSeat(string RecordLocator, string PassengerID, EnumFlightType FlightType, List<ABS.Logic.GroupBooking.SeatInfo> SessionSeatInfo,
            ref string CompartmentDesignator, ref string Deck, ref string SeatSet, ref int HotSeat, int PsgNumber)
        {
            try
            {
                ////AABookingManager.Booking BookingInfo = new AABookingManager.Booking();
                int PassengerNumber = 0;
                DataTable dtPassengerSeat = new DataTable();
                ////dtFlightDetails = FI_Booking_Logic.getAllBookingDetails_byRecordLocator(RecordLocator, QueueCode);

                if (SessionSeatInfo == null == false)
                {
                    foreach (ABS.Logic.GroupBooking.SeatInfo drSeatInfo in SessionSeatInfo)
                    {
                        //drSeatInfo = drSeatInfo_loopVariable;
                        if (drSeatInfo.PassengerID == PassengerID && RecordLocator.Trim() == drSeatInfo.RecordLocator.Trim())
                        {
                            PassengerNumber = drSeatInfo.PassengerNumber;
                            Deck = drSeatInfo.Deck;
                            CompartmentDesignator = drSeatInfo.CompartmentDesignator;
                            HotSeat = drSeatInfo.IsHotSeat;
                            ////HotSeatMap = drSeatInfo.HotSeatMap;
                            return drSeatInfo.SelectedSeat;
                            // TODO: might not be correct. Was : Exit For
                        }
                    }

                }

                return "";

                ////////if ((dtFlightDetails != null) & dtFlightDetails.Rows.Count > 0)
                ////////{
                ////////    switch (FlightType)
                ////////    {
                ////////        case Common.EnumFlightType.DepartFlight:
                ////////            dtPassengerSeat = FI_Booking_Logic.getAllBookingDetails_byJourneySellKey(RecordLocator, dtFlightDetails.Rows(0)("JourneySellKey"));
                ////////            break;
                ////////        case Common.EnumFlightType.DepartConnectingFlight:
                ////////            dtPassengerSeat = FI_Booking_Logic.getAllBookingDetails_byJourneySellKey(RecordLocator, dtFlightDetails.Rows(2)("JourneySellKey"));
                ////////            break;
                ////////        case Common.EnumFlightType.ReturnFlight:
                ////////            dtPassengerSeat = FI_Booking_Logic.getAllBookingDetails_byJourneySellKey(RecordLocator, dtFlightDetails.Rows(1)("JourneySellKey"));
                ////////            break;
                ////////        case Common.EnumFlightType.ReturnConnectingFlight:
                ////////            dtPassengerSeat = FI_Booking_Logic.getAllBookingDetails_byJourneySellKey(RecordLocator, dtFlightDetails.Rows(3)("JourneySellKey"));
                ////////            break;
                ////////    }

                ////////    if (SessionSeatInfo == null == false)
                ////////    {
                ////////        foreach (void drSeatInfo_loopVariable in SessionSeatInfo)
                ////////        {
                ////////            drSeatInfo = drSeatInfo_loopVariable;
                ////////            if (drSeatInfo.PassengerID == PassengerID)
                ////////            {
                ////////                PassengerNumber = drSeatInfo.PassengerNumber;
                ////////                Deck = drSeatInfo.Deck;
                ////////                CompartmentDesignator = drSeatInfo.CompartmentDesignator;
                ////////                HotSeat = drSeatInfo.IsHotSeat;
                ////////                HotSeatMap = drSeatInfo.HotSeatMap;
                ////////                return drSeatInfo.SelectedSeat;
                ////////                break; // TODO: might not be correct. Was : Exit For
                ////////            }
                ////////        }

                ////////    }
                ////////    else
                ////////    {
                ////////        if ((dtPassengerSeat != null) && dtPassengerSeat.Rows.Count > 0)
                ////////        {
                ////////            CompartmentDesignator = dtPassengerSeat.Rows(PsgNumber)("CompartmentDesignator");
                ////////            Deck = dtPassengerSeat.Rows(PsgNumber)("Deck");
                ////////            //HotSeat = IsHotSeat(dtPassengerSeat.Rows(PsgNumber)("CarrierCode"), "320", dtPassengerSeat.Rows(PsgNumber)("UnitDesignator"))
                ////////            HotSeat = IsHotSeat(dtPassengerSeat.Rows(PsgNumber)("CarrierCode"), dtPassengerSeat.Rows(PsgNumber)("EquipmentType"), dtPassengerSeat.Rows(PsgNumber)("UnitDesignator"));
                ////////            string str = FormHotSeatString(dtPassengerSeat.Rows(PsgNumber)("CarrierCode"), dtPassengerSeat.Rows(PsgNumber)("EquipmentType"));
                ////////            HotSeatMap = str;
                ////////            //Return dtPassengerSeat.Rows(PsgNumber)("UnitDesignator")
                ////////            return "";
                ////////        }
                ////////        else
                ////////        {
                ////////            return "";
                ////////        }
                ////////    }
                ////////}
                ////////else
                ////////{
                ////////    switch (FlightType)
                ////////    {
                ////////        case Common.EnumFlightType.DepartFlight:
                ////////            iType = 0;
                ////////            iJourney = 0;
                ////////            break;
                ////////        case Common.EnumFlightType.DepartConnectingFlight:
                ////////            iType = 0;
                ////////            iJourney = 1;
                ////////            break;
                ////////        case Common.EnumFlightType.ReturnFlight:
                ////////            iType = 1;
                ////////            iJourney = 0;
                ////////            break;
                ////////        case Common.EnumFlightType.ReturnConnectingFlight:
                ////////            iType = 1;
                ////////            iJourney = 1;
                ////////            break;
                ////////    }

                ////////    if (SessionSeatInfo == null == false)
                ////////    {
                ////////        foreach (void drSeatInfo_loopVariable in SessionSeatInfo)
                ////////        {
                ////////            drSeatInfo = drSeatInfo_loopVariable;
                ////////            if (drSeatInfo.PassengerID == PassengerID)
                ////////            {
                ////////                PassengerNumber = drSeatInfo.PassengerNumber;
                ////////                Deck = drSeatInfo.Deck;
                ////////                CompartmentDesignator = drSeatInfo.CompartmentDesignator;

                ////////                return drSeatInfo.SelectedSeat;
                ////////                break; // TODO: might not be correct. Was : Exit For
                ////////            }
                ////////        }


                ////////    }
                ////////    else
                ////////    {
                ////////        BookingInfo = API.GetBooking(_RecordLocator);
                ////////        if ((BookingInfo != null))
                ////////        {
                ////////            for (int i = 0; i <= BookingInfo.Passengers.Count - 1; i++)
                ////////            {
                ////////                if (BookingInfo.Passengers(i).PassengerID == PassengerID)
                ////////                {
                ////////                    PassengerNumber = BookingInfo.Passengers(i).PassengerNumber;
                ////////                    break; // TODO: might not be correct. Was : Exit For
                ////////                }
                ////////            }

                ////////            for (int i = 0; i <= BookingInfo.Journeys(iType).Segments(iJourney).PaxSeats.Count - 1; i++)
                ////////            {
                ////////                if (BookingInfo.Journeys(iType).Segments(iJourney).PaxSeats(i).PassengerNumber == PassengerNumber)
                ////////                {
                ////////                    CompartmentDesignator = BookingInfo.Journeys(iType).Segments(iJourney).PaxSeats(i).CompartmentDesignator;
                ////////                    //ketee
                ////////                    Deck = BookingInfo.Journeys(iType).Segments(iJourney).PaxSeats(i).PaxSeatInfo.Deck;
                ////////                    //_SeatSet = BookingInfo.Journeys(iType).Segments(0).PaxSeats(i).Penalty
                ////////                    return BookingInfo.Journeys(iType).Segments(iJourney).PaxSeats(i).UnitDesignator;
                ////////                    break; // TODO: might not be correct. Was : Exit For
                ////////                }
                ////////            }

                ////////        }
                ////////        else
                ////////        {
                ////////            return "";
                ////////        }
                ////////    }
                ////////}




            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                return "";
            }
        }

        private string CreateSeatControl(int Pax, string FromTo, List<ABS.Logic.GroupBooking.SeatInfo> _seatInfo, EnumFlightType Type)
        {
            try
            {
                string str = "";
                int i = 0;
                string seatbox = "";
                string selectedBox = "";
                DataTable dtInfant = new DataTable();
                int index = 0;

                seatbox = "BPassengerNumber_";
                selectedBox = "APassengerNumber_";

                str += "<table class=\"clearTableHeaders\"><tbody><tr class=\"market\">";
                str += "<th scope=\"col\">" + FromTo + "</th><th scope=\"col\"></th><th scope=\"col\"></th></tr>";

                foreach (ABS.Logic.GroupBooking.SeatInfo seatInfo in _seatInfo)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        //if (!string.IsNullOrEmpty(seatInfo.SelectedSeat))
                        {
                            string PaxType = seatInfo.PaxType;
                            if (Session["dtInfant"] != null)
                            {
                                dtInfant = (DataTable)Session["dtInfant"];
                                DataRow drInfant = dtInfant.Select("PassengerID = '" + seatInfo.PassengerID + "' AND RecordLocator = '" + seatInfo.RecordLocator.Trim() + "'").FirstOrDefault();
                                if (drInfant != null && drInfant.ItemArray.Length > 0)
                                {
                                    if (PaxType.Trim() != "") PaxType = " (" + PaxType + " + INFT )";
                                }
                                else
                                {
                                    if (PaxType.Trim() != "") PaxType = " (" + PaxType + ")";
                                }
                            }
                            else
                            {
                                if (PaxType.Trim() != "") PaxType = " (" + PaxType + ")";
                            }
                            if (i == 0 || (i != 0 && (seatInfo.RecordLocator.Trim()) != (_seatInfo[i - 1].RecordLocator.Trim())))
                            {
                                index += 1;
                                str += "<tr><td class=\"pnr\" colspan=\"3\" style=\"background-color: #e3e3e3;font-size: small;font-weight: 600;padding: 3px;padding-left: 15px;color: #000;\">  PNR : (" + seatInfo.RecordLocator + ")</td></tr>";
                            }
                            //if (PaxType.Trim() != "") PaxType = " (" + PaxType + ")";

                            if (seatInfo.PaxName.ToString().Trim() == "")
                                str += "<tr><td class=\"passenger\">" + (i + 1) + ". " + "TBA TBA" + PaxType + "</td>";
                            else
                                str += "<tr><td class=\"passenger\">" + (i + 1) + ". " + seatInfo.PaxName + PaxType + "</td>";

                            str += "<td class=\"seatSelect\">";
                            str += "<input type=\"hidden\" name=\"PassengerNumber_" + seatInfo.Seq + "\" id=\"PassengerNumber_" + seatInfo.Seq + "\" value=\"" + seatInfo.PassengerID + "\" class=\"\" >";
                            if (i == 0)
                            {
                                str += "<input name=\"" + seatbox + seatInfo.Seq + "\" type=\"text\" id=\"" + seatbox + seatInfo.Seq + "\" readonly=\"\" class=\"activeUnitInput\" ></td>";
                            }
                            else
                            {
                                str += "<input name=\"" + seatbox + seatInfo.Seq + "\" type=\"text\" id=\"" + seatbox + seatInfo.Seq + "\" readonly=\"\" class=\"\" ></td>";
                            }
                            str += "<td><input name=\"" + seatbox + seatInfo.Seq + "_Fee\" type=\"hidden\" id=\"" + seatbox + seatInfo.Seq + "_Fee\" readonly=\"\" class=\"\" >";
                            str += "<td><input name=\"" + seatbox + seatInfo.Seq + "_HidFee\" type=\"hidden\" id=\"" + seatbox + seatInfo.Seq + "_HidFee\" readonly=\"\" class=\"\" ></td>";
                            str += "</tr>";
                            //remark by ketee, 20170117
                            ////str += "<tr><td style=\"vertical-align:top;\">";
                            ////str += "<input id=\"" + selectedBox + seatInfo.Seq + "_Reselect\" type=\"button\" class=\"button_1\" value=\"Reselect\">";
                            //////str &= "<input id=""APassengerNumber_" & seatInfo.Seq & "_Remove"" type=""button"" class=""button_1"" value=""Remove""></td>"
                            ////str += "</td><td style=\"vertical-align:top;\">" + (seatInfo.IsHotSeat == 1 ? "<img src=\"../images/JetAircraft_NS_Open_0_HS.gif\" class=\"unitGroupKey\">" : "<img src=\"../images/JetAircraft_NS_Open_0.gif\" class=\"unitGroupKey\">") + "</td></tr>";
                        }

                    }
                    i += 1;
                }

                str += "</tbody></table>";

                return str;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                return "";
            }
        }

        //protected void LinkButtonAssignUnit_Click(object sender, EventArgs e)
        protected string ValidateSeat()
        {
            MessageList msgList = new MessageList();
            bool assignSeatDone = false;
            DataTable dtDetail = new DataTable();
            try
            {
                pnlErr.Visible = false;
                int SeatInfo0Checking = 0;
                int SeatInfo1Checking = 0;
                int SeatInfo2Checking = 0;
                int SeatInfo3Checking = 0;

                bool IsOneWay = false;

                GetBookingResponse resp = (GetBookingResponse)Session["resp"];
                lstPassengerContainerNew = new List<PassengerContainer>();
                if (Session["PassengerContainer"] != null)
                {
                    lstPassengerContainer = (List<PassengerContainer>)Session["PassengerContainer"];
                    switch (eFlight)
                    {
                        #region "ConnectingFlight"
                        case EnumFlight.ConnectingFlight:
                            if (resp == null == false)
                            {
                                //Flight_Info pDepartFlightInfo = new Flight_Info();
                                //pDepartFlightInfo = Session["DepartFlightInfo"];
                                if (!objGeneral.IsInternationalFlight(resp.Booking.Journeys[0].Segments[0].DepartureStation, resp.Booking.Journeys[0].Segments[0].ArrivalStation, Request.PhysicalApplicationPath))
                                {
                                    if (Session["SeatInfo0"] == null == false)
                                    {
                                        for (int i = 0; i <= ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"]).Count - 1; i++)
                                        {
                                            lstPassengerContainer[i].setNewUnitDesignator(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"])[i].SelectedSeat, (PassengerContainer.FlightType.Depart));
                                        }
                                    }
                                }
                                else
                                {
                                    if (Session["SeatInfo0"] == null == false)
                                    {

                                        for (int i = 0; i <= ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"]).Count() - 1; i++)
                                        {
                                            if (string.IsNullOrEmpty(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"])[i].SelectedSeat))
                                            {
                                                SeatInfo0Checking = SeatInfo0Checking + 1;
                                            }
                                            lstPassengerContainer[i].setNewUnitDesignator(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"])[i].SelectedSeat, (PassengerContainer.FlightType.Depart));
                                        }
                                    }
                                    else
                                    {
                                        SeatInfo0Checking = SeatInfo0Checking + 1;
                                    }
                                }


                                if (!objGeneral.IsInternationalFlight(resp.Booking.Journeys[0].Segments[0].DepartureStation, resp.Booking.Journeys[0].Segments[0].ArrivalStation, Request.PhysicalApplicationPath))
                                {
                                    //SeatInfo1Checking = 0;
                                    if (Session["SeatInfo1"] == null == false)
                                    {
                                        for (int i = 0; i <= ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"]).Count - 1; i++)
                                        {
                                            lstPassengerContainer[i].setNewUnitDesignator(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"])[i].SelectedSeat, (PassengerContainer.FlightType.ConnectingDepart));
                                        }
                                    }
                                }
                                else
                                {
                                    if (Session["SeatInfo1"] == null == false)
                                    {
                                        for (int i = 0; i <= ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"]).Count - 1; i++)
                                        {
                                            if (string.IsNullOrEmpty(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"])[i].SelectedSeat))
                                            {
                                                SeatInfo1Checking = SeatInfo1Checking + 1;
                                            }
                                            lstPassengerContainer[i].setNewUnitDesignator(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"])[i].SelectedSeat, (PassengerContainer.FlightType.ConnectingDepart));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (Session["ReturnFlightInfo"] == null)
                                {
                                    Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.InvalidPage);
                                }
                            }

                            if ((bool)Session["OneWay"] == false)
                            {
                                if (!objGeneral.IsInternationalFlight(resp.Booking.Journeys[1].Segments[0].DepartureStation, resp.Booking.Journeys[1].Segments[0].ArrivalStation, Request.PhysicalApplicationPath))
                                {
                                    //SeatInfo2Checking = 0;
                                    if (Session["SeatInfo2"] == null == false)
                                    {
                                        for (int i = 0; i <= ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"]).Count - 1; i++)
                                        {
                                            lstPassengerContainer[i].setNewUnitDesignator(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"])[i].SelectedSeat, (PassengerContainer.FlightType.Return));
                                        }
                                    }
                                }
                                else
                                {
                                    if (Session["SeatInfo2"] == null == false)
                                    {
                                        for (int j = 0; j <= ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"]).Count - 1; j++)
                                        {
                                            if (string.IsNullOrEmpty(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"])[j].SelectedSeat))
                                            {
                                                SeatInfo2Checking = SeatInfo2Checking + 1;
                                            }
                                            lstPassengerContainer[j].setNewUnitDesignator(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"])[j].SelectedSeat, (PassengerContainer.FlightType.Return));
                                        }
                                    }
                                }

                                if (!objGeneral.IsInternationalFlight(resp.Booking.Journeys[1].Segments[0].DepartureStation, resp.Booking.Journeys[1].Segments[0].ArrivalStation, Request.PhysicalApplicationPath))
                                {
                                    //SeatInfo3Checking = 0;
                                    if (Session["SeatInfo3"] == null == false)
                                    {
                                        for (int i = 0; i <= ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo3"]).Count - 1; i++)
                                        {
                                            lstPassengerContainer[i].setNewUnitDesignator(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo3"])[i].SelectedSeat, (PassengerContainer.FlightType.ConnectingReturn));
                                        }
                                    }
                                }
                                else
                                {
                                    if (Session["SeatInfo3"] == null == false)
                                    {
                                        for (int j = 0; j <= ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo3"]).Count - 1; j++)
                                        {
                                            if (string.IsNullOrEmpty(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo3"])[j].SelectedSeat))
                                            {
                                                SeatInfo3Checking = SeatInfo3Checking + 1;
                                            }
                                            lstPassengerContainer[j].setNewUnitDesignator(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo3"])[j].SelectedSeat, (PassengerContainer.FlightType.ConnectingReturn));
                                        }
                                    }
                                }
                            }
                            Page.Validate("PrimaryMandatory");
                            if (Page.IsValid)
                            {
                                ClearSeatFeeValue();
                                if (Session["transitdepartreturn"] != null)
                                {
                                    if (SeatInfo0Checking == 0 & SeatInfo1Checking == 0 & SeatInfo2Checking == 0 & SeatInfo3Checking == 0)
                                    {
                                        if (SeatInfo0Checking == 0)
                                        {
                                            List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfoswapmulti = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            SeatAvailabilityResponse pAvailableSeatInfo0 = (SeatAvailabilityResponse)Session["pAvailableSeatInfo0"];
                                            List<PassengerContainer> newseat = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Depart)) != item.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
                                            if (newseat != null && newseat.Count > 0)
                                            {
                                                //int num = 0;
                                                foreach (PassengerContainer pass in newseat)
                                                {
                                                    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
                                                    {
                                                        List<PassengerContainer> found = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Depart)) == pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
                                                        if (found != null && found.Count > 0)
                                                        {
                                                            foreach (PassengerContainer passfound in found)
                                                            {
                                                                if (pass.RecordLocator != passfound.RecordLocator)
                                                                {
                                                                    Decimal start = 0, end = 0;
                                                                    ABS.Logic.GroupBooking.SeatInfo assignswapmulti = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = pAvailableSeatInfo0.EquipmentInfos[0].Compartments.Select(x => x.Seats.Where(item => item.SeatDesignator == passfound.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).FirstOrDefault()).ToList();
                                                                    if (findgroup.Count != 0)
                                                                    {
                                                                        assignswapmulti.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo0.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assignswapmulti.SeatGroup).ToList();
                                                                    start = findamount.Select(x => x.PassengerFee.ServiceCharges.FirstOrDefault().Amount).Sum();
                                                                    ABS.Logic.GroupBooking.SeatInfo assignswapmulti2 = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    List<ABS.Navitaire.BookingManager.SeatInfo> findgroup2 = pAvailableSeatInfo0.EquipmentInfos[0].Compartments.Select(x => x.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).FirstOrDefault()).ToList();
                                                                    if (findgroup2.Count != 0)
                                                                    {
                                                                        assignswapmulti2.SeatGroup = findgroup2[0].SeatGroup.ToString();
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount2 = pAvailableSeatInfo0.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assignswapmulti2.SeatGroup).ToList();
                                                                    end = findamount2.Select(x => x.PassengerFee.ServiceCharges.FirstOrDefault().Amount).Sum();
                                                                    int findsameRecordLocator = assignSeatinfo.Count(item => item.RecordLocator.ToString() != pass.RecordLocator);
                                                                    if (findsameRecordLocator > 0)
                                                                        Session["additionalamount"] = ((Session["additionalamount"] != null) ? Convert.ToDecimal(Session["additionalamount"]) : 0) + (start - end);

                                                                }

                                                                ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                                assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
                                                                assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
                                                                assign.RecordLocator = pass.RecordLocator;
                                                                assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                                assign.PassengerID = pass.PassengerID.ToString();
                                                                assign.PassengerNumber = (int)pass.PassengerNumber;
                                                                foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo0.EquipmentInfos[0].Compartments)
                                                                {
                                                                    List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
                                                                    if (findgroup.Count == 0) continue;
                                                                    else
                                                                    {
                                                                        assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                        break;
                                                                    }
                                                                }
                                                                List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount3 = pAvailableSeatInfo0.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                                foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount3)
                                                                {
                                                                    for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                    {
                                                                        assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                    }
                                                                    break;
                                                                }
                                                                assignSeatinfo.Add(assign);

                                                            }
                                                            // if (num == 1) break;
                                                        }
                                                        else
                                                        {
                                                            ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                            assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
                                                            assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
                                                            assign.RecordLocator = pass.RecordLocator;
                                                            assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                            assign.PassengerID = pass.PassengerID.ToString();
                                                            assign.PassengerNumber = (int)pass.PassengerNumber;
                                                            foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo0.EquipmentInfos[0].Compartments)
                                                            {
                                                                List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
                                                                if (findgroup.Count == 0) continue;
                                                                else
                                                                {
                                                                    assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                    break;
                                                                }
                                                            }
                                                            List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo0.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                            foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
                                                            {
                                                                for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                {
                                                                    assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                }
                                                                break;
                                                            }
                                                            assignSeatinfo.Add(assign);
                                                        }
                                                    }
                                                }

                                                //foreach (PassengerContainer pass in newseat)
                                                //{
                                                //    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
                                                //    {
                                                //        ABS.Logic.GroupBooking.SeatInfo unassign = new ABS.Logic.GroupBooking.SeatInfo();
                                                //        unassign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
                                                //        unassign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
                                                //        unassign.RecordLocator = pass.RecordLocator;
                                                //        unassign.CompartmentDesignator = pass.CompartmentDesignator;
                                                //        unassign.PassengerID = pass.PassengerID.ToString();
                                                //        unassign.PassengerNumber = (int)pass.PassengerNumber;
                                                //        unassignSeatinfo.Add(unassign);
                                                //    }
                                                //}
                                                //unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
                                                //if (num == 1) return "Fail to assign change seat, cannot swap seat with different PNR";
                                            }

                                            Session["assignSeatinfoDepartConnectingFlight"] = assignSeatinfo;
                                            //Session["assignSeatinfoDepartConnectingFlightSwapmulti"] = assignSeatinfoswapmulti;
                                            if (AssignSeat(assignSeatinfo, assignSeatinfo, model, EnumFlightType.DepartConnectingFlight))
                                            {
                                                DeleteXML((string)Session["SeatInfo0Xml"]);
                                                Session["DepartSeatInfo"] = Session["SeatInfo0"];
                                            }
                                            else
                                            {
                                                ClearSession();
                                                return msgList.Err999997;
                                            }
                                        }
                                        if (SeatInfo1Checking == 0)
                                        {
                                            //List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = null;
                                            //if (Session["DepartConnectingExistingSeatInfo2"] != null)
                                            //{
                                            //    unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartConnectingExistingSeatInfo2"];
                                            //}

                                            List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfoswapmulti = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            SeatAvailabilityResponse pAvailableSeatInfo1 = (SeatAvailabilityResponse)Session["pAvailableSeatInfo1"];
                                            List<PassengerContainer> newseat = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.ConnectingDepart)) != item.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart))).ToList();
                                            if (newseat != null && newseat.Count > 0)
                                            {
                                                //int num = 0;
                                                foreach (PassengerContainer pass in newseat)
                                                {
                                                    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
                                                    {
                                                        List<PassengerContainer> found = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.ConnectingDepart)) == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart))).ToList();
                                                        if (found != null && found.Count > 0)
                                                        {
                                                            foreach (PassengerContainer passfound in found)
                                                            {
                                                                if (pass.RecordLocator != passfound.RecordLocator)
                                                                {
                                                                    Decimal start = 0, end = 0;
                                                                    ABS.Logic.GroupBooking.SeatInfo assignswapmulti = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo1.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == passfound.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assignswapmulti.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }

                                                                    //assignswapmulti.SelectedSeat = passfound.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo1.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assignswapmulti.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {

                                                                            start += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    ABS.Logic.GroupBooking.SeatInfo assignswapmulti2 = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo1.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assignswapmulti2.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount2 = pAvailableSeatInfo1.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assignswapmulti2.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount2)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {

                                                                            end += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    int findsameRecordLocator = assignSeatinfo.Count(item => item.RecordLocator.ToString() != pass.RecordLocator);
                                                                    if (findsameRecordLocator > 0)
                                                                        Session["additionalamount"] = ((Session["additionalamount"] != null) ? Convert.ToDecimal(Session["additionalamount"]) : 0) + (start - end);
                                                                    //}
                                                                    //else
                                                                    //{
                                                                    ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.ConnectingDepart));
                                                                    assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart));
                                                                    assign.RecordLocator = pass.RecordLocator;
                                                                    assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                                    assign.PassengerID = pass.PassengerID.ToString();
                                                                    assign.PassengerNumber = (int)pass.PassengerNumber;
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo1.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }

                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount3 = pAvailableSeatInfo1.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount3)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {
                                                                            assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    assignSeatinfo.Add(assign);
                                                                }
                                                                else
                                                                {
                                                                    ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.ConnectingDepart));
                                                                    assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart));
                                                                    assign.RecordLocator = pass.RecordLocator;
                                                                    assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                                    assign.PassengerID = pass.PassengerID.ToString();
                                                                    assign.PassengerNumber = (int)pass.PassengerNumber;
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo1.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount3 = pAvailableSeatInfo1.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount3)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {
                                                                            assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    assignSeatinfo.Add(assign);
                                                                }
                                                            }
                                                            //if (num == 1) break;
                                                        }
                                                        else
                                                        {
                                                            ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                            assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.ConnectingDepart));
                                                            assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart));
                                                            assign.RecordLocator = pass.RecordLocator;
                                                            assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                            assign.PassengerID = pass.PassengerID.ToString();
                                                            assign.PassengerNumber = (int)pass.PassengerNumber;
                                                            foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo1.EquipmentInfos[0].Compartments)
                                                            {
                                                                List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart))).ToList();
                                                                if (findgroup.Count == 0) continue;
                                                                else
                                                                {
                                                                    assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                    break;
                                                                }
                                                            }
                                                            List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo1.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                            foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
                                                            {
                                                                for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                {
                                                                    assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                }
                                                                break;
                                                            }
                                                            assignSeatinfo.Add(assign);
                                                        }
                                                    }
                                                }

                                                //foreach (PassengerContainer pass in newseat)
                                                //{
                                                //    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
                                                //    {
                                                //        ABS.Logic.GroupBooking.SeatInfo unassign = new ABS.Logic.GroupBooking.SeatInfo();
                                                //        unassign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
                                                //        unassign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
                                                //        unassign.RecordLocator = pass.RecordLocator;
                                                //        unassign.CompartmentDesignator = pass.CompartmentDesignator;
                                                //        unassign.PassengerID = pass.PassengerID.ToString();
                                                //        unassign.PassengerNumber = (int)pass.PassengerNumber;
                                                //        unassignSeatinfo.Add(unassign);
                                                //    }
                                                //}
                                                //unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
                                                //if (num == 1) return "Fail to assign change seat, cannot swap seat with different PNR";
                                            }

                                            Session["assignSeatinfoDepartConnectingFlight2"] = assignSeatinfo;
                                            //Session["assignSeatinfoDepartConnectingFlight2swapmulti"] = assignSeatinfoswapmulti;
                                            if (AssignSeat(assignSeatinfo, assignSeatinfo, model, EnumFlightType.DepartConnectingFlight2))
                                            {
                                                DeleteXML((string)Session["SeatInfo1Xml"]);
                                                Session["DepartConnectingSeatInfo"] = Session["SeatInfo1"];
                                            }
                                            else
                                            {

                                                ClearSession();
                                                return msgList.Err999997;
                                            }
                                        }
                                        if (SeatInfo2Checking == 0 && (bool)Session["OneWay"] == false)
                                        {
                                            List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfoswapmulti = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            SeatAvailabilityResponse pAvailableSeatInfo2 = (SeatAvailabilityResponse)Session["pAvailableSeatInfo2"];
                                            List<PassengerContainer> newseat = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Return)) != item.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
                                            if (newseat != null && newseat.Count > 0)
                                            {
                                                //int num = 0;
                                                foreach (PassengerContainer pass in newseat)
                                                {
                                                    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Return)) != "")
                                                    {
                                                        List<PassengerContainer> found = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Return)) == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
                                                        if (found != null && found.Count > 0)
                                                        {
                                                            foreach (PassengerContainer passfound in found)
                                                            {
                                                                if (pass.RecordLocator != passfound.RecordLocator)
                                                                {
                                                                    Decimal start = 0, end = 0;
                                                                    ABS.Logic.GroupBooking.SeatInfo assignswapmulti = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo2.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == passfound.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assignswapmulti.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo2.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assignswapmulti.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {

                                                                            start += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    ABS.Logic.GroupBooking.SeatInfo assignswapmulti2 = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo2.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assignswapmulti2.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount2 = pAvailableSeatInfo2.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assignswapmulti2.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount2)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {

                                                                            end += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    int findsameRecordLocator = assignSeatinfo.Count(item => item.RecordLocator.ToString() != pass.RecordLocator);
                                                                    if (findsameRecordLocator > 0)
                                                                        Session["additionalamount"] = ((Session["additionalamount"] != null) ? Convert.ToDecimal(Session["additionalamount"]) : 0) + (start - end);
                                                                    //}
                                                                    //else
                                                                    //{
                                                                    ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Return));
                                                                    assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Return));
                                                                    assign.RecordLocator = pass.RecordLocator;
                                                                    assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                                    assign.PassengerID = pass.PassengerID.ToString();
                                                                    assign.PassengerNumber = (int)pass.PassengerNumber;
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo2.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount3 = pAvailableSeatInfo2.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount3)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {
                                                                            assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    assignSeatinfo.Add(assign);
                                                                }
                                                                else
                                                                {
                                                                    ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Return));
                                                                    assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Return));
                                                                    assign.RecordLocator = pass.RecordLocator;
                                                                    assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                                    assign.PassengerID = pass.PassengerID.ToString();
                                                                    assign.PassengerNumber = (int)pass.PassengerNumber;
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo2.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount3 = pAvailableSeatInfo2.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount3)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {
                                                                            assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    assignSeatinfo.Add(assign);
                                                                }
                                                            }
                                                            //if (num == 1) break;
                                                        }
                                                        else
                                                        {
                                                            ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                            assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Return));
                                                            assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Return));
                                                            assign.RecordLocator = pass.RecordLocator;
                                                            assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                            assign.PassengerID = pass.PassengerID.ToString();
                                                            assign.PassengerNumber = (int)pass.PassengerNumber;
                                                            foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo2.EquipmentInfos[0].Compartments)
                                                            {
                                                                List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
                                                                if (findgroup.Count == 0) continue;
                                                                else
                                                                {
                                                                    assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                    break;
                                                                }
                                                            }
                                                            List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo2.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                            foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
                                                            {
                                                                for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                {
                                                                    assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                }
                                                                break;
                                                            }
                                                            assignSeatinfo.Add(assign);
                                                        }
                                                    }
                                                }

                                                //foreach (PassengerContainer pass in newseat)
                                                //{
                                                //    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
                                                //    {
                                                //        ABS.Logic.GroupBooking.SeatInfo unassign = new ABS.Logic.GroupBooking.SeatInfo();
                                                //        unassign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
                                                //        unassign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
                                                //        unassign.RecordLocator = pass.RecordLocator;
                                                //        unassign.CompartmentDesignator = pass.CompartmentDesignator;
                                                //        unassign.PassengerID = pass.PassengerID.ToString();
                                                //        unassign.PassengerNumber = (int)pass.PassengerNumber;
                                                //        unassignSeatinfo.Add(unassign);
                                                //    }
                                                //}
                                                //unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
                                                //if (num == 1) return "Fail to assign change seat, cannot swap seat with different PNR";
                                            }
                                            //List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = null;
                                            //if (Session["ReturnConnectingExistingSeatInfo"] != null)
                                            //{
                                            //    unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnConnectingExistingSeatInfo"];
                                            //}
                                            Session["assignSeatinfoReturnConnectingFlight"] = assignSeatinfo;
                                            //Session["assignSeatinfoReturnConnectingFlightswapmulti"] = assignSeatinfoswapmulti;
                                            if (AssignSeat(assignSeatinfo, assignSeatinfo, model, EnumFlightType.ReturnConnectingFlight))
                                            {
                                                DeleteXML((string)Session["SeatInfo2Xml"]);
                                                Session["ReturnSeatInfo"] = Session["SeatInfo2"];
                                            }
                                            else
                                            {
                                                ClearSession();
                                                return msgList.Err999997;
                                            }
                                        }
                                        if (SeatInfo3Checking == 0 && (bool)Session["OneWay"] == false)
                                        {
                                            List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfoswapmulti = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            SeatAvailabilityResponse pAvailableSeatInfo3 = (SeatAvailabilityResponse)Session["pAvailableSeatInfo3"];
                                            List<PassengerContainer> newseat = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.ConnectingReturn)) != item.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn))).ToList();
                                            if (newseat != null && newseat.Count > 0)
                                            {
                                                //int num = 0;
                                                foreach (PassengerContainer pass in newseat)
                                                {
                                                    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn)) != "")
                                                    {
                                                        List<PassengerContainer> found = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.ConnectingReturn)) == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn))).ToList();
                                                        if (found != null && found.Count > 0)
                                                        {
                                                            foreach (PassengerContainer passfound in found)
                                                            {
                                                                if (pass.RecordLocator != passfound.RecordLocator)
                                                                {
                                                                    Decimal start = 0, end = 0;
                                                                    ABS.Logic.GroupBooking.SeatInfo assignswapmulti = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo3.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == passfound.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assignswapmulti.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo3.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assignswapmulti.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {

                                                                            start += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    ABS.Logic.GroupBooking.SeatInfo assignswapmulti2 = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo3.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assignswapmulti2.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount2 = pAvailableSeatInfo3.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assignswapmulti2.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount2)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {

                                                                            end += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    int findsameRecordLocator = assignSeatinfo.Count(item => item.RecordLocator.ToString() != pass.RecordLocator);
                                                                    if (findsameRecordLocator > 0)
                                                                        Session["additionalamount"] = ((Session["additionalamount"] != null) ? Convert.ToDecimal(Session["additionalamount"]) : 0) + (start - end);
                                                                    //}
                                                                    //else
                                                                    //{
                                                                    ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.ConnectingReturn));
                                                                    assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn));
                                                                    assign.RecordLocator = pass.RecordLocator;
                                                                    assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                                    assign.PassengerID = pass.PassengerID.ToString();
                                                                    assign.PassengerNumber = (int)pass.PassengerNumber;
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo3.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount3 = pAvailableSeatInfo3.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount3)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {
                                                                            assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    assignSeatinfo.Add(assign);
                                                                }
                                                                else
                                                                {
                                                                    ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.ConnectingReturn));
                                                                    assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn));
                                                                    assign.RecordLocator = pass.RecordLocator;
                                                                    assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                                    assign.PassengerID = pass.PassengerID.ToString();
                                                                    assign.PassengerNumber = (int)pass.PassengerNumber;
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo3.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount3 = pAvailableSeatInfo3.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount3)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {
                                                                            assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    assignSeatinfo.Add(assign);
                                                                }
                                                            }
                                                            //if (num == 1) break;
                                                        }
                                                        else
                                                        {
                                                            ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                            assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.ConnectingReturn));
                                                            assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn));
                                                            assign.RecordLocator = pass.RecordLocator;
                                                            assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                            assign.PassengerID = pass.PassengerID.ToString();
                                                            assign.PassengerNumber = (int)pass.PassengerNumber;
                                                            foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo3.EquipmentInfos[0].Compartments)
                                                            {
                                                                List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn))).ToList();
                                                                if (findgroup.Count == 0) continue;
                                                                else
                                                                {
                                                                    assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                    break;
                                                                }
                                                            }
                                                            List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo3.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                            foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
                                                            {
                                                                for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                {
                                                                    assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                }
                                                                break;
                                                            }
                                                            assignSeatinfo.Add(assign);
                                                        }
                                                    }
                                                }

                                                //foreach (PassengerContainer pass in newseat)
                                                //{
                                                //    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
                                                //    {
                                                //        ABS.Logic.GroupBooking.SeatInfo unassign = new ABS.Logic.GroupBooking.SeatInfo();
                                                //        unassign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
                                                //        unassign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
                                                //        unassign.RecordLocator = pass.RecordLocator;
                                                //        unassign.CompartmentDesignator = pass.CompartmentDesignator;
                                                //        unassign.PassengerID = pass.PassengerID.ToString();
                                                //        unassign.PassengerNumber = (int)pass.PassengerNumber;
                                                //        unassignSeatinfo.Add(unassign);
                                                //    }
                                                //}
                                                //unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
                                                //if (num == 1) return "Fail to assign change seat, cannot swap seat with different PNR";
                                            }
                                            //List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = null;
                                            //if (Session["ReturnConnectingExistingSeatInfo2"] != null)
                                            //{
                                            //    unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnConnectingExistingSeatInfo2"];
                                            //}
                                            Session["assignSeatinfoReturnConnectingFlight2"] = assignSeatinfo;
                                            //Session["assignSeatinfoReturnConnectingFlight2swapmulti"] = assignSeatinfoswapmulti;
                                            if (AssignSeat(assignSeatinfo, assignSeatinfo, model, EnumFlightType.ReturnConnectingFlight2))
                                            {
                                                DeleteXML((string)Session["SeatInfo3Xml"]);
                                                Session["ReturnConnectingSeatInfo"] = Session["SeatInfo3"];
                                            }
                                            else
                                            {
                                                ClearSession();
                                                return msgList.Err999997;
                                            }
                                        }

                                        //Response.Redirect("~/pages/SeatSummary.aspx");
                                    }
                                    else
                                    {
                                        ClearSession();
                                        return msgList.Err999996;
                                        //lblErr.Text = msgList.Err999996;
                                        //pnlErr.Visible = true;
                                        //Session["ErrorMsg"] = lblErr.Text;
                                        //Response.Redirect("~/seats.aspx");
                                        //FillFlight(Session["akey"], 0)
                                    }
                                }
                                else if (Session["transitreturn"] != null)
                                {
                                    if (SeatInfo0Checking == 0 & SeatInfo2Checking == 0 & SeatInfo3Checking == 0)
                                    {
                                        if (SeatInfo0Checking == 0)
                                        {
                                            List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfoswapmulti = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            SeatAvailabilityResponse pAvailableSeatInfo0 = (SeatAvailabilityResponse)Session["pAvailableSeatInfo0"];
                                            List<PassengerContainer> newseat = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Depart)) != item.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
                                            if (newseat != null && newseat.Count > 0)
                                            {
                                                //int num = 0;
                                                foreach (PassengerContainer pass in newseat)
                                                {
                                                    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
                                                    {
                                                        List<PassengerContainer> found = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Depart)) == pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
                                                        if (found != null && found.Count > 0)
                                                        {
                                                            foreach (PassengerContainer passfound in found)
                                                            {
                                                                if (pass.RecordLocator != passfound.RecordLocator)
                                                                {
                                                                    Decimal start = 0, end = 0;
                                                                    ABS.Logic.GroupBooking.SeatInfo assignswapmulti = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo0.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == passfound.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assignswapmulti.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }

                                                                    //assignswapmulti.SelectedSeat = passfound.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo0.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assignswapmulti.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {

                                                                            start += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    ABS.Logic.GroupBooking.SeatInfo assignswapmulti2 = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo0.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assignswapmulti2.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount2 = pAvailableSeatInfo0.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assignswapmulti2.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount2)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {

                                                                            end += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    int findsameRecordLocator = assignSeatinfo.Count(item => item.RecordLocator.ToString() != pass.RecordLocator);
                                                                    if (findsameRecordLocator > 0)
                                                                        Session["additionalamount"] = ((Session["additionalamount"] != null) ? Convert.ToDecimal(Session["additionalamount"]) : 0) + (start - end);
                                                                    //}
                                                                    //else
                                                                    //{
                                                                    ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
                                                                    assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
                                                                    assign.RecordLocator = pass.RecordLocator;
                                                                    assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                                    assign.PassengerID = pass.PassengerID.ToString();
                                                                    assign.PassengerNumber = (int)pass.PassengerNumber;
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo0.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount3 = pAvailableSeatInfo0.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount3)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {
                                                                            assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    assignSeatinfo.Add(assign);
                                                                }
                                                                else
                                                                {
                                                                    ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
                                                                    assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
                                                                    assign.RecordLocator = pass.RecordLocator;
                                                                    assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                                    assign.PassengerID = pass.PassengerID.ToString();
                                                                    assign.PassengerNumber = (int)pass.PassengerNumber;
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo0.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount3 = pAvailableSeatInfo0.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount3)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {
                                                                            assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    assignSeatinfo.Add(assign);
                                                                }
                                                            }
                                                            // if (num == 1) break;
                                                        }
                                                        else
                                                        {
                                                            ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                            assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
                                                            assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
                                                            assign.RecordLocator = pass.RecordLocator;
                                                            assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                            assign.PassengerID = pass.PassengerID.ToString();
                                                            assign.PassengerNumber = (int)pass.PassengerNumber;
                                                            foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo0.EquipmentInfos[0].Compartments)
                                                            {
                                                                List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
                                                                if (findgroup.Count == 0) continue;
                                                                else
                                                                {
                                                                    assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                    break;
                                                                }
                                                            }
                                                            List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo0.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                            foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
                                                            {
                                                                for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                {
                                                                    assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                }
                                                                break;
                                                            }
                                                            assignSeatinfo.Add(assign);
                                                        }
                                                    }
                                                }

                                                //foreach (PassengerContainer pass in newseat)
                                                //{
                                                //    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
                                                //    {
                                                //        ABS.Logic.GroupBooking.SeatInfo unassign = new ABS.Logic.GroupBooking.SeatInfo();
                                                //        unassign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
                                                //        unassign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
                                                //        unassign.RecordLocator = pass.RecordLocator;
                                                //        unassign.CompartmentDesignator = pass.CompartmentDesignator;
                                                //        unassign.PassengerID = pass.PassengerID.ToString();
                                                //        unassign.PassengerNumber = (int)pass.PassengerNumber;
                                                //        unassignSeatinfo.Add(unassign);
                                                //    }
                                                //}
                                                //unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
                                                //if (num == 1) return "Fail to assign change seat, cannot swap seat with different PNR";
                                            }

                                            Session["assignSeatinfoDepartConnectingFlight"] = assignSeatinfo;
                                            //Session["assignSeatinfoDepartConnectingFlightswapmulti"] = assignSeatinfoswapmulti;
                                            if (AssignSeat(assignSeatinfo, assignSeatinfo, model, EnumFlightType.DepartConnectingFlight))
                                            {
                                                DeleteXML((string)Session["SeatInfo0Xml"]);
                                                Session["DepartSeatInfo"] = Session["SeatInfo0"];
                                            }
                                            else
                                            {
                                                ClearSession();
                                                return msgList.Err999997;
                                            }
                                        }

                                        if (SeatInfo2Checking == 0 && (bool)Session["OneWay"] == false)
                                        {
                                            List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfoswapmulti = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            SeatAvailabilityResponse pAvailableSeatInfo2 = (SeatAvailabilityResponse)Session["pAvailableSeatInfo2"];
                                            List<PassengerContainer> newseat = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Return)) != item.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
                                            if (newseat != null && newseat.Count > 0)
                                            {
                                                //int num = 0;
                                                foreach (PassengerContainer pass in newseat)
                                                {
                                                    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Return)) != "")
                                                    {
                                                        List<PassengerContainer> found = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Return)) == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
                                                        if (found != null && found.Count > 0)
                                                        {
                                                            foreach (PassengerContainer passfound in found)
                                                            {
                                                                if (pass.RecordLocator != passfound.RecordLocator)
                                                                {
                                                                    Decimal start = 0, end = 0;
                                                                    ABS.Logic.GroupBooking.SeatInfo assignswapmulti = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo2.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == passfound.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assignswapmulti.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo2.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assignswapmulti.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {

                                                                            start += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    ABS.Logic.GroupBooking.SeatInfo assignswapmulti2 = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo2.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assignswapmulti2.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount2 = pAvailableSeatInfo2.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assignswapmulti2.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount2)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {

                                                                            end += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    int findsameRecordLocator = assignSeatinfo.Count(item => item.RecordLocator.ToString() != pass.RecordLocator);
                                                                    if (findsameRecordLocator > 0)
                                                                        Session["additionalamount"] = ((Session["additionalamount"] != null) ? Convert.ToDecimal(Session["additionalamount"]) : 0) + (start - end);
                                                                    //}
                                                                    //else
                                                                    //{
                                                                    ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Return));
                                                                    assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Return));
                                                                    assign.RecordLocator = pass.RecordLocator;
                                                                    assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                                    assign.PassengerID = pass.PassengerID.ToString();
                                                                    assign.PassengerNumber = (int)pass.PassengerNumber;
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo2.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount3 = pAvailableSeatInfo2.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount3)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {
                                                                            assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    assignSeatinfo.Add(assign);
                                                                }
                                                                else
                                                                {
                                                                    ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Return));
                                                                    assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Return));
                                                                    assign.RecordLocator = pass.RecordLocator;
                                                                    assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                                    assign.PassengerID = pass.PassengerID.ToString();
                                                                    assign.PassengerNumber = (int)pass.PassengerNumber;
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo2.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    } List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount3 = pAvailableSeatInfo2.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount3)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {
                                                                            assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    assignSeatinfo.Add(assign);
                                                                }
                                                            }
                                                            //if (num == 1) break;
                                                        }
                                                        else
                                                        {
                                                            ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                            assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Return));
                                                            assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Return));
                                                            assign.RecordLocator = pass.RecordLocator;
                                                            assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                            assign.PassengerID = pass.PassengerID.ToString();
                                                            assign.PassengerNumber = (int)pass.PassengerNumber;
                                                            foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo2.EquipmentInfos[0].Compartments)
                                                            {
                                                                List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
                                                                if (findgroup.Count == 0) continue;
                                                                else
                                                                {
                                                                    assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                    break;
                                                                }
                                                            }
                                                            List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo2.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                            foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
                                                            {
                                                                for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                {
                                                                    assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                }
                                                                break;
                                                            }
                                                            assignSeatinfo.Add(assign);
                                                        }
                                                    }
                                                }

                                                //foreach (PassengerContainer pass in newseat)
                                                //{
                                                //    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
                                                //    {
                                                //        ABS.Logic.GroupBooking.SeatInfo unassign = new ABS.Logic.GroupBooking.SeatInfo();
                                                //        unassign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
                                                //        unassign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
                                                //        unassign.RecordLocator = pass.RecordLocator;
                                                //        unassign.CompartmentDesignator = pass.CompartmentDesignator;
                                                //        unassign.PassengerID = pass.PassengerID.ToString();
                                                //        unassign.PassengerNumber = (int)pass.PassengerNumber;
                                                //        unassignSeatinfo.Add(unassign);
                                                //    }
                                                //}
                                                //unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
                                                //if (num == 1) return "Fail to assign change seat, cannot swap seat with different PNR";
                                            }

                                            Session["assignSeatinfoReturnConnectingFlight"] = assignSeatinfo;
                                            //Session["assignSeatinfoReturnConnectingFlightswapmulti"] = assignSeatinfo;
                                            if (AssignSeat(assignSeatinfo, assignSeatinfo, model, EnumFlightType.ReturnConnectingFlight))
                                            {
                                                DeleteXML((string)Session["SeatInfo2Xml"]);
                                                Session["ReturnSeatInfo"] = Session["SeatInfo2"];
                                            }
                                            else
                                            {

                                                ClearSession();
                                                return msgList.Err999997;
                                            }
                                        }
                                        if (SeatInfo3Checking == 0 && (bool)Session["OneWay"] == false)
                                        {
                                            List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfoswapmulti = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            SeatAvailabilityResponse pAvailableSeatInfo3 = (SeatAvailabilityResponse)Session["pAvailableSeatInfo3"];
                                            List<PassengerContainer> newseat = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.ConnectingReturn)) != item.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn))).ToList();
                                            if (newseat != null && newseat.Count > 0)
                                            {
                                                //int num = 0;
                                                foreach (PassengerContainer pass in newseat)
                                                {
                                                    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn)) != "")
                                                    {
                                                        List<PassengerContainer> found = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.ConnectingReturn)) == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn))).ToList();
                                                        if (found != null && found.Count > 0)
                                                        {
                                                            foreach (PassengerContainer passfound in found)
                                                            {
                                                                if (pass.RecordLocator != passfound.RecordLocator)
                                                                {
                                                                    Decimal start = 0, end = 0;
                                                                    ABS.Logic.GroupBooking.SeatInfo assignswapmulti = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo3.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == passfound.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assignswapmulti.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo3.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assignswapmulti.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {

                                                                            start += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    ABS.Logic.GroupBooking.SeatInfo assignswapmulti2 = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo3.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assignswapmulti2.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount2 = pAvailableSeatInfo3.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assignswapmulti2.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount2)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {

                                                                            end += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    int findsameRecordLocator = assignSeatinfo.Count(item => item.RecordLocator.ToString() != pass.RecordLocator);
                                                                    if (findsameRecordLocator > 0)
                                                                        Session["additionalamount"] = ((Session["additionalamount"] != null) ? Convert.ToDecimal(Session["additionalamount"]) : 0) + (start - end);
                                                                    //}
                                                                    //else
                                                                    //{
                                                                    ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.ConnectingReturn));
                                                                    assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn));
                                                                    assign.RecordLocator = pass.RecordLocator;
                                                                    assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                                    assign.PassengerID = pass.PassengerID.ToString();
                                                                    assign.PassengerNumber = (int)pass.PassengerNumber;
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo3.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount3 = pAvailableSeatInfo3.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount3)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {
                                                                            assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    assignSeatinfo.Add(assign);
                                                                }
                                                                else
                                                                {
                                                                    ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.ConnectingReturn));
                                                                    assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn));
                                                                    assign.RecordLocator = pass.RecordLocator;
                                                                    assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                                    assign.PassengerID = pass.PassengerID.ToString();
                                                                    assign.PassengerNumber = (int)pass.PassengerNumber;
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo3.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount3 = pAvailableSeatInfo3.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount3)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {
                                                                            assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    assignSeatinfo.Add(assign);
                                                                }
                                                            }
                                                            // if (num == 1) break;
                                                        }
                                                        else
                                                        {
                                                            ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                            assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.ConnectingReturn));
                                                            assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn));
                                                            assign.RecordLocator = pass.RecordLocator;
                                                            assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                            assign.PassengerID = pass.PassengerID.ToString();
                                                            assign.PassengerNumber = (int)pass.PassengerNumber;
                                                            foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo3.EquipmentInfos[0].Compartments)
                                                            {
                                                                List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn))).ToList();
                                                                if (findgroup.Count == 0) continue;
                                                                else
                                                                {
                                                                    assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                    break;
                                                                }
                                                            }
                                                            List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo3.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                            foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
                                                            {
                                                                for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                {
                                                                    assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                }
                                                                break;
                                                            }
                                                            assignSeatinfo.Add(assign);
                                                        }
                                                    }
                                                }

                                                //foreach (PassengerContainer pass in newseat)
                                                //{
                                                //    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
                                                //    {
                                                //        ABS.Logic.GroupBooking.SeatInfo unassign = new ABS.Logic.GroupBooking.SeatInfo();
                                                //        unassign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
                                                //        unassign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
                                                //        unassign.RecordLocator = pass.RecordLocator;
                                                //        unassign.CompartmentDesignator = pass.CompartmentDesignator;
                                                //        unassign.PassengerID = pass.PassengerID.ToString();
                                                //        unassign.PassengerNumber = (int)pass.PassengerNumber;
                                                //        unassignSeatinfo.Add(unassign);
                                                //    }
                                                //}
                                                //unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
                                                //if (num == 1) return "Fail to assign change seat, cannot swap seat with different PNR";
                                            }

                                            Session["assignSeatinfoReturnConnectingFlight2"] = assignSeatinfo;
                                            //Session["assignSeatinfoReturnConnectingFlight2swapmulti"] = assignSeatinfo;
                                            if (AssignSeat(assignSeatinfo, assignSeatinfo, model, EnumFlightType.ReturnConnectingFlight2))
                                            {
                                                DeleteXML((string)Session["SeatInfo3Xml"]);
                                                Session["ReturnConnectingSeatInfo"] = Session["SeatInfo3"];
                                            }
                                            else
                                            {
                                                ClearSession();
                                                return msgList.Err999997;
                                            }
                                        }

                                        //Response.Redirect("~/pages/SeatSummary.aspx");
                                    }
                                    else
                                    {
                                        ClearSession();
                                        return msgList.Err999996;
                                        //lblErr.Text = msgList.Err999996;
                                        //pnlErr.Visible = true;
                                        //Session["ErrorMsg"] = lblErr.Text;
                                        //Response.Redirect("~/seats.aspx");
                                        //FillFlight(Session["akey"], 0)
                                    }
                                }
                                else if (Session["transitdepart"] != null)
                                {
                                    if (SeatInfo0Checking == 0 & SeatInfo1Checking == 0 & SeatInfo2Checking == 0)
                                    {
                                        if (SeatInfo0Checking == 0)
                                        {
                                            List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfoswapmulti = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            SeatAvailabilityResponse pAvailableSeatInfo0 = (SeatAvailabilityResponse)Session["pAvailableSeatInfo0"];
                                            List<PassengerContainer> newseat = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Depart)) != item.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
                                            if (newseat != null && newseat.Count > 0)
                                            {
                                                //int num = 0;
                                                foreach (PassengerContainer pass in newseat)
                                                {
                                                    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
                                                    {
                                                        List<PassengerContainer> found = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Depart)) == pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
                                                        if (found != null && found.Count > 0)
                                                        {
                                                            foreach (PassengerContainer passfound in found)
                                                            {
                                                                if (pass.RecordLocator != passfound.RecordLocator)
                                                                {
                                                                    Decimal start = 0, end = 0;
                                                                    ABS.Logic.GroupBooking.SeatInfo assignswapmulti = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo0.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == passfound.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assignswapmulti.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo0.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assignswapmulti.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {

                                                                            start += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    ABS.Logic.GroupBooking.SeatInfo assignswapmulti2 = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo0.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assignswapmulti2.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount2 = pAvailableSeatInfo0.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assignswapmulti2.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount2)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {

                                                                            end += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    int findsameRecordLocator = assignSeatinfo.Count(item => item.RecordLocator.ToString() != pass.RecordLocator);
                                                                    if (findsameRecordLocator > 0)
                                                                        Session["additionalamount"] = ((Session["additionalamount"] != null) ? Convert.ToDecimal(Session["additionalamount"]) : 0) + (start - end);
                                                                    //}
                                                                    //else
                                                                    //{
                                                                    ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
                                                                    assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
                                                                    assign.RecordLocator = pass.RecordLocator;
                                                                    assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                                    assign.PassengerID = pass.PassengerID.ToString();
                                                                    assign.PassengerNumber = (int)pass.PassengerNumber;
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo0.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount3 = pAvailableSeatInfo0.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount3)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {
                                                                            assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    assignSeatinfo.Add(assign);
                                                                }
                                                                else
                                                                {
                                                                    ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
                                                                    assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
                                                                    assign.RecordLocator = pass.RecordLocator;
                                                                    assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                                    assign.PassengerID = pass.PassengerID.ToString();
                                                                    assign.PassengerNumber = (int)pass.PassengerNumber;
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo0.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount3 = pAvailableSeatInfo0.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount3)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {
                                                                            assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    assignSeatinfo.Add(assign);
                                                                }
                                                            }
                                                            // if (num == 1) break;
                                                        }
                                                        else
                                                        {
                                                            ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                            assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
                                                            assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
                                                            assign.RecordLocator = pass.RecordLocator;
                                                            assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                            assign.PassengerID = pass.PassengerID.ToString();
                                                            assign.PassengerNumber = (int)pass.PassengerNumber;
                                                            foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo0.EquipmentInfos[0].Compartments)
                                                            {
                                                                List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
                                                                if (findgroup.Count == 0) continue;
                                                                else
                                                                {
                                                                    assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                    break;
                                                                }
                                                            }
                                                            List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo0.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                            foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
                                                            {
                                                                for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                {
                                                                    assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                }
                                                                break;
                                                            }
                                                            assignSeatinfo.Add(assign);
                                                        }
                                                    }
                                                }

                                                //foreach (PassengerContainer pass in newseat)
                                                //{
                                                //    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
                                                //    {
                                                //        ABS.Logic.GroupBooking.SeatInfo unassign = new ABS.Logic.GroupBooking.SeatInfo();
                                                //        unassign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
                                                //        unassign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
                                                //        unassign.RecordLocator = pass.RecordLocator;
                                                //        unassign.CompartmentDesignator = pass.CompartmentDesignator;
                                                //        unassign.PassengerID = pass.PassengerID.ToString();
                                                //        unassign.PassengerNumber = (int)pass.PassengerNumber;
                                                //        unassignSeatinfo.Add(unassign);
                                                //    }
                                                //}
                                                //unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
                                                //if (num == 1) return "Fail to assign change seat, cannot swap seat with different PNR";
                                            }

                                            Session["assignSeatinfoDepartConnectingFlight"] = assignSeatinfo;
                                            //Session["assignSeatinfoDepartConnectingFlightswapmulti"] = assignSeatinfoswapmulti;
                                            if (AssignSeat(assignSeatinfo, assignSeatinfo, model, EnumFlightType.DepartConnectingFlight))
                                            {
                                                DeleteXML((string)Session["SeatInfo0Xml"]);
                                                Session["DepartSeatInfo"] = Session["SeatInfo0"];
                                            }
                                            else
                                            {

                                                ClearSession();
                                                return msgList.Err999997;
                                            }
                                        }
                                        if (SeatInfo1Checking == 0)
                                        {
                                            List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfoswapmulti = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            SeatAvailabilityResponse pAvailableSeatInfo1 = (SeatAvailabilityResponse)Session["pAvailableSeatInfo1"];
                                            List<PassengerContainer> newseat = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.ConnectingDepart)) != item.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart))).ToList();
                                            if (newseat != null && newseat.Count > 0)
                                            {
                                                //int num = 0;
                                                foreach (PassengerContainer pass in newseat)
                                                {
                                                    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart)) != "")
                                                    {
                                                        List<PassengerContainer> found = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.ConnectingDepart)) == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart))).ToList();
                                                        if (found != null && found.Count > 0)
                                                        {
                                                            foreach (PassengerContainer passfound in found)
                                                            {
                                                                if (pass.RecordLocator != passfound.RecordLocator)
                                                                {
                                                                    Decimal start = 0, end = 0;
                                                                    ABS.Logic.GroupBooking.SeatInfo assignswapmulti = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo1.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == passfound.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assignswapmulti.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo1.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assignswapmulti.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {

                                                                            start += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    ABS.Logic.GroupBooking.SeatInfo assignswapmulti2 = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo1.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assignswapmulti2.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount2 = pAvailableSeatInfo1.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assignswapmulti2.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount2)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {

                                                                            end += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    int findsameRecordLocator = assignSeatinfo.Count(item => item.RecordLocator.ToString() != pass.RecordLocator);
                                                                    if (findsameRecordLocator > 0)
                                                                        Session["additionalamount"] = ((Session["additionalamount"] != null) ? Convert.ToDecimal(Session["additionalamount"]) : 0) + (start - end);
                                                                    //}
                                                                    //else
                                                                    //{
                                                                    ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.ConnectingDepart));
                                                                    assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart));
                                                                    assign.RecordLocator = pass.RecordLocator;
                                                                    assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                                    assign.PassengerID = pass.PassengerID.ToString();
                                                                    assign.PassengerNumber = (int)pass.PassengerNumber;
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo1.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount3 = pAvailableSeatInfo1.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount3)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {
                                                                            assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    assignSeatinfo.Add(assign);
                                                                }
                                                                else
                                                                {
                                                                    ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.ConnectingDepart));
                                                                    assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart));
                                                                    assign.RecordLocator = pass.RecordLocator;
                                                                    assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                                    assign.PassengerID = pass.PassengerID.ToString();
                                                                    assign.PassengerNumber = (int)pass.PassengerNumber;
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo1.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount3 = pAvailableSeatInfo1.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount3)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {
                                                                            assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    assignSeatinfo.Add(assign);
                                                                }
                                                            }
                                                            //if (num == 1) break;
                                                        }
                                                        else
                                                        {
                                                            ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                            assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.ConnectingDepart));
                                                            assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart));
                                                            assign.RecordLocator = pass.RecordLocator;
                                                            assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                            assign.PassengerID = pass.PassengerID.ToString();
                                                            assign.PassengerNumber = (int)pass.PassengerNumber;
                                                            foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo1.EquipmentInfos[0].Compartments)
                                                            {
                                                                List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart))).ToList();
                                                                if (findgroup.Count == 0) continue;
                                                                else
                                                                {
                                                                    assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                    break;
                                                                }
                                                            }
                                                            List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo1.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                            foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
                                                            {
                                                                for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                {
                                                                    assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                }
                                                                break;
                                                            }
                                                            assignSeatinfo.Add(assign);
                                                        }
                                                    }
                                                }

                                                //foreach (PassengerContainer pass in newseat)
                                                //{
                                                //    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
                                                //    {
                                                //        ABS.Logic.GroupBooking.SeatInfo unassign = new ABS.Logic.GroupBooking.SeatInfo();
                                                //        unassign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
                                                //        unassign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
                                                //        unassign.RecordLocator = pass.RecordLocator;
                                                //        unassign.CompartmentDesignator = pass.CompartmentDesignator;
                                                //        unassign.PassengerID = pass.PassengerID.ToString();
                                                //        unassign.PassengerNumber = (int)pass.PassengerNumber;
                                                //        unassignSeatinfo.Add(unassign);
                                                //    }
                                                //}
                                                //unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
                                                //if (num == 1) return "Fail to assign change seat, cannot swap seat with different PNR";
                                            }

                                            Session["assignSeatinfoDepartConnectingFlight2"] = assignSeatinfo;
                                            //Session["assignSeatinfoDepartConnectingFlight2swapmulti"] = assignSeatinfoswapmulti;
                                            if (AssignSeat(assignSeatinfo, assignSeatinfo, model, EnumFlightType.DepartConnectingFlight2))
                                            {
                                                DeleteXML((string)Session["SeatInfo1Xml"]);
                                                Session["DepartConnectingSeatInfo"] = Session["SeatInfo1"];
                                            }
                                            else
                                            {

                                                ClearSession();
                                                return msgList.Err999997;
                                            }
                                        }
                                        if (SeatInfo2Checking == 0 && (bool)Session["OneWay"] == false)
                                        {
                                            List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfoswapmulti = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                            SeatAvailabilityResponse pAvailableSeatInfo2 = (SeatAvailabilityResponse)Session["pAvailableSeatInfo2"];
                                            List<PassengerContainer> newseat = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Return)) != item.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
                                            if (newseat != null && newseat.Count > 0)
                                            {
                                                //int num = 0;
                                                foreach (PassengerContainer pass in newseat)
                                                {
                                                    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Return)) != "")
                                                    {
                                                        List<PassengerContainer> found = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Return)) == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
                                                        if (found != null && found.Count > 0)
                                                        {
                                                            foreach (PassengerContainer passfound in found)
                                                            {
                                                                if (pass.RecordLocator != passfound.RecordLocator)
                                                                {
                                                                    Decimal start = 0, end = 0;
                                                                    ABS.Logic.GroupBooking.SeatInfo assignswapmulti = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo2.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == passfound.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assignswapmulti.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo2.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assignswapmulti.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {

                                                                            start += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    ABS.Logic.GroupBooking.SeatInfo assignswapmulti2 = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo2.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assignswapmulti2.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount2 = pAvailableSeatInfo2.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assignswapmulti2.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount2)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {

                                                                            end += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    int findsameRecordLocator = assignSeatinfo.Count(item => item.RecordLocator.ToString() != pass.RecordLocator);
                                                                    if (findsameRecordLocator > 0)
                                                                        Session["additionalamount"] = ((Session["additionalamount"] != null) ? Convert.ToDecimal(Session["additionalamount"]) : 0) + (start - end);
                                                                    //}
                                                                    //else
                                                                    //{
                                                                    ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Return));
                                                                    assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Return));
                                                                    assign.RecordLocator = pass.RecordLocator;
                                                                    assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                                    assign.PassengerID = pass.PassengerID.ToString();
                                                                    assign.PassengerNumber = (int)pass.PassengerNumber;
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo2.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount3 = pAvailableSeatInfo2.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount3)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {
                                                                            assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    assignSeatinfo.Add(assign);
                                                                }
                                                                else
                                                                {
                                                                    ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                                    assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Return));
                                                                    assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Return));
                                                                    assign.RecordLocator = pass.RecordLocator;
                                                                    assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                                    assign.PassengerID = pass.PassengerID.ToString();
                                                                    assign.PassengerNumber = (int)pass.PassengerNumber;
                                                                    foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo2.EquipmentInfos[0].Compartments)
                                                                    {
                                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
                                                                        if (findgroup.Count == 0) continue;
                                                                        else
                                                                        {
                                                                            assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                            break;
                                                                        }
                                                                    }
                                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount3 = pAvailableSeatInfo2.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount3)
                                                                    {
                                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                        {
                                                                            assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                        }
                                                                        break;
                                                                    }
                                                                    assignSeatinfo.Add(assign);
                                                                }
                                                            }
                                                            //if (num == 1) break;
                                                        }
                                                        else
                                                        {
                                                            ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                            assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Return));
                                                            assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Return));
                                                            assign.RecordLocator = pass.RecordLocator;
                                                            assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                            assign.PassengerID = pass.PassengerID.ToString();
                                                            assign.PassengerNumber = (int)pass.PassengerNumber;
                                                            foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo2.EquipmentInfos[0].Compartments)
                                                            {
                                                                List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
                                                                if (findgroup.Count == 0) continue;
                                                                else
                                                                {
                                                                    assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                    break;
                                                                }
                                                            }
                                                            List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo2.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                            foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
                                                            {
                                                                for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                {
                                                                    assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                }
                                                                break;
                                                            }
                                                            assignSeatinfo.Add(assign);
                                                        }
                                                    }
                                                }

                                                //foreach (PassengerContainer pass in newseat)
                                                //{
                                                //    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
                                                //    {
                                                //        ABS.Logic.GroupBooking.SeatInfo unassign = new ABS.Logic.GroupBooking.SeatInfo();
                                                //        unassign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
                                                //        unassign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
                                                //        unassign.RecordLocator = pass.RecordLocator;
                                                //        unassign.CompartmentDesignator = pass.CompartmentDesignator;
                                                //        unassign.PassengerID = pass.PassengerID.ToString();
                                                //        unassign.PassengerNumber = (int)pass.PassengerNumber;
                                                //        unassignSeatinfo.Add(unassign);
                                                //    }
                                                //}
                                                //unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
                                                //if (num == 1) return "Fail to assign change seat, cannot swap seat with different PNR";
                                            }

                                            Session["assignSeatinfoReturnConnectingFlight"] = assignSeatinfo;
                                            //Session["assignSeatinfoReturnConnectingFlightswapmulti"] = assignSeatinfoswapmulti;
                                            if (AssignSeat(assignSeatinfo, assignSeatinfo, model, EnumFlightType.ReturnConnectingFlight))
                                            {
                                                DeleteXML((string)Session["SeatInfo2Xml"]);
                                                Session["ReturnSeatInfo"] = Session["SeatInfo2"];
                                            }
                                            else
                                            {

                                                ClearSession();
                                                return msgList.Err999997;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ClearSession();
                                        return msgList.Err999996;
                                    }
                                }

                                assignSeatDone = true;

                            }
                            break;
                        #endregion

                        #region "DirectFlight"
                        case EnumFlight.DirectFlight:
                            if (resp == null == false)
                            {
                                //Flight_Info pDepartFlightInfo = new Flight_Info();
                                //pDepartFlightInfo = Session["DepartFlightInfo"];

                                if (!objGeneral.IsInternationalFlight(resp.Booking.Journeys[0].Segments[0].DepartureStation, resp.Booking.Journeys[0].Segments[0].ArrivalStation, Request.PhysicalApplicationPath))
                                {
                                    //SeatInfo0Checking = 0;
                                    if (Session["SeatInfo0"] == null == false)
                                    {
                                        for (int i = 0; i <= ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"]).Count - 1; i++)
                                        {
                                            lstPassengerContainer[i].setNewUnitDesignator(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"])[i].SelectedSeat, (PassengerContainer.FlightType.Depart));
                                        }
                                    }
                                }
                                else
                                {
                                    if (Session["SeatInfo0"] == null == false)
                                    {
                                        for (int i = 0; i <= ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"]).Count - 1; i++)
                                        {

                                            if (string.IsNullOrEmpty(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"])[i].SelectedSeat))
                                            {

                                                //SeatInfo0Checking = SeatInfo0Checking + 1;
                                            }

                                            lstPassengerContainer[i].setNewUnitDesignator(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"])[i].SelectedSeat, (PassengerContainer.FlightType.Depart));

                                            //List<PassengerContainer>newseat = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType)EnumFlightType.DepartFlight) == ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"])[i].SelectedSeat.ToString()).ToList();
                                            //if (newseat != null && newseat.Count == 0)
                                            //{
                                            //    objPassengerContainerNew.setUnitDesignator(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"])[i].SelectedSeat, (PassengerContainer.FlightType)EnumFlightType.DepartFlight);
                                            //    lstPassengerContainerNew.Add(objPassengerContainerNew);                                            
                                            //}

                                        }
                                        if (SeatInfo0Checking == 0)
                                        {
                                            //if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"], model, EnumFlightType.DepartFlight))
                                            //{
                                            //    DeleteXML((string)Session["SeatInfo0Xml"]);
                                            //    Session["DepartSeatInfo"] = Session["SeatInfo0"];
                                            //}
                                        }

                                    }
                                    else
                                    {
                                        SeatInfo0Checking = SeatInfo0Checking + 1;
                                    }
                                }
                            }
                            else
                            {
                                //Response.Redirect("~/pages/InvalidPage.aspx")
                                SeatInfo0Checking = 0;
                            }

                            if ((bool)Session["OneWay"] == false)
                            {
                                //Flight_Info pReturnFlightInfo = new Flight_Info();
                                //pReturnFlightInfo = Session["ReturnFlightInfo"];

                                if (!objGeneral.IsInternationalFlight(resp.Booking.Journeys[1].Segments[0].DepartureStation, resp.Booking.Journeys[1].Segments[0].ArrivalStation, Request.PhysicalApplicationPath))
                                {
                                    //SeatInfo1Checking = 0;
                                    if (Session["SeatInfo1"] == null == false)
                                    {
                                        for (int j = 0; j <= ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"]).Count - 1; j++)
                                        {
                                            lstPassengerContainer[j].setNewUnitDesignator(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"])[j].SelectedSeat, (PassengerContainer.FlightType.Return));
                                        }
                                    }
                                }
                                else
                                {
                                    if (Session["SeatInfo1"] == null == false)
                                    {
                                        for (int j = 0; j <= ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"]).Count - 1; j++)
                                        {
                                            if (string.IsNullOrEmpty(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"])[j].SelectedSeat))
                                            {
                                                //SeatInfo1Checking = SeatInfo1Checking + 1;

                                            }
                                            lstPassengerContainer[j].setNewUnitDesignator(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"])[j].SelectedSeat, (PassengerContainer.FlightType.Return));

                                            //List<PassengerContainer> newseat = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType)EnumFlightType.DepartFlight) == ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"])[j].SelectedSeat.ToString()).ToList();
                                            //if (newseat != null && newseat.Count == 0)
                                            //{
                                            //    objPassengerContainerNew.setUnitDesignator(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"])[j].SelectedSeat, (PassengerContainer.FlightType)EnumFlightType.DepartFlight);
                                            //    lstPassengerContainerNew.Add(objPassengerContainerNew);
                                            //}


                                        }
                                        if (SeatInfo1Checking == 0)
                                        {
                                            //if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"], model2, EnumFlightType.ReturnFlight))
                                            //{
                                            //    DeleteXML((string)Session["SeatInfo1Xml"]);
                                            //    Session["ReturnSeatInfo"] = Session["SeatInfo1"];
                                            //}
                                        }

                                    }
                                    else
                                    {
                                        //SeatInfo1Checking = SeatInfo1Checking + 1;
                                    }
                                }

                            }
                            else
                            {
                                SeatInfo1Checking = 0;
                            }
                            Page.Validate("PrimaryMandatory");
                            if (Page.IsValid)
                            {
                                if (SeatInfo0Checking == 0 & SeatInfo1Checking == 0)
                                {
                                    ClearSeatFeeValue();
                                    if (SeatInfo0Checking == 0 && resp != null)
                                    {
                                        List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                        List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                        List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfoswapmulti = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                        SeatAvailabilityResponse pAvailableSeatInfo0 = (SeatAvailabilityResponse)Session["pAvailableSeatInfo0"];
                                        List<PassengerContainer> newseat = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Depart)) != item.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
                                        if (newseat != null && newseat.Count > 0)
                                        {
                                            //int num = 0;
                                            foreach (PassengerContainer pass in newseat)
                                            {
                                                if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
                                                {
                                                    List<PassengerContainer> found = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Depart)) == pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
                                                    if (found != null && found.Count > 0)
                                                    {
                                                        foreach (PassengerContainer passfound in found)
                                                        {
                                                            if (pass.RecordLocator != passfound.RecordLocator)
                                                            {
                                                                Decimal start = 0, end = 0;
                                                                ABS.Logic.GroupBooking.SeatInfo assignswapmulti = new ABS.Logic.GroupBooking.SeatInfo();
                                                                foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo0.EquipmentInfos[0].Compartments)
                                                                {
                                                                    List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == passfound.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
                                                                    if (findgroup.Count == 0) continue;
                                                                    else
                                                                    {
                                                                        assignswapmulti.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                        break;
                                                                    }
                                                                }
                                                                List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo0.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assignswapmulti.SeatGroup).ToList();
                                                                foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
                                                                {
                                                                    for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                    {

                                                                        start += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                    }
                                                                    break;
                                                                }
                                                                ABS.Logic.GroupBooking.SeatInfo assignswapmulti2 = new ABS.Logic.GroupBooking.SeatInfo();
                                                                foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo0.EquipmentInfos[0].Compartments)
                                                                {
                                                                    List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
                                                                    if (findgroup.Count == 0) continue;
                                                                    else
                                                                    {
                                                                        assignswapmulti2.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                        break;
                                                                    }
                                                                }
                                                                List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount2 = pAvailableSeatInfo0.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assignswapmulti2.SeatGroup).ToList();
                                                                foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount2)
                                                                {
                                                                    for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                    {

                                                                        end += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                    }
                                                                    break;
                                                                }
                                                                int findsameRecordLocator = assignSeatinfo.Count(item => item.RecordLocator.ToString() != pass.RecordLocator);
                                                                if (findsameRecordLocator > 0)
                                                                    Session["additionalamount"] = ((Session["additionalamount"] != null) ? Convert.ToDecimal(Session["additionalamount"]) : 0) + (start - end);
                                                                //}
                                                                //else
                                                                //{
                                                                ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                                assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
                                                                assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
                                                                assign.RecordLocator = pass.RecordLocator;
                                                                assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                                assign.PassengerID = pass.PassengerID.ToString();
                                                                assign.PassengerNumber = (int)pass.PassengerNumber;
                                                                foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo0.EquipmentInfos[0].Compartments)
                                                                {
                                                                    List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
                                                                    if (findgroup.Count == 0) continue;
                                                                    else
                                                                    {
                                                                        assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                        break;
                                                                    }
                                                                }
                                                                List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount3 = pAvailableSeatInfo0.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                                foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount3)
                                                                {
                                                                    for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                    {
                                                                        assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                    }
                                                                    break;
                                                                }
                                                                assignSeatinfo.Add(assign);
                                                            }
                                                            else
                                                            {
                                                                ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                                assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
                                                                assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
                                                                assign.RecordLocator = pass.RecordLocator;
                                                                assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                                assign.PassengerID = pass.PassengerID.ToString();
                                                                assign.PassengerNumber = (int)pass.PassengerNumber;
                                                                foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo0.EquipmentInfos[0].Compartments)
                                                                {
                                                                    List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
                                                                    if (findgroup.Count == 0) continue;
                                                                    else
                                                                    {
                                                                        assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                        break;
                                                                    }
                                                                }
                                                                List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount3 = pAvailableSeatInfo0.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                                foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount3)
                                                                {
                                                                    for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                    {
                                                                        assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                    }
                                                                    break;
                                                                }
                                                                assignSeatinfo.Add(assign);
                                                            }
                                                        }
                                                        //if (num == 1) break;
                                                    }
                                                    else
                                                    {
                                                        ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                        assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
                                                        assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
                                                        assign.RecordLocator = pass.RecordLocator;
                                                        assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                        assign.PassengerID = pass.PassengerID.ToString();
                                                        assign.PassengerNumber = (int)pass.PassengerNumber;
                                                        foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo0.EquipmentInfos[0].Compartments)
                                                        {
                                                            List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
                                                            if (findgroup.Count == 0) continue;
                                                            else
                                                            {
                                                                assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                break;
                                                            }
                                                        }
                                                        List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo0.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                        foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
                                                        {
                                                            for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                            {
                                                                assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                            }
                                                            break;
                                                        }
                                                        assignSeatinfo.Add(assign);
                                                    }
                                                }
                                            }

                                            //foreach (PassengerContainer pass in newseat)
                                            //{
                                            //    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
                                            //    {
                                            //        ABS.Logic.GroupBooking.SeatInfo unassign = new ABS.Logic.GroupBooking.SeatInfo();
                                            //        unassign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
                                            //        unassign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
                                            //        unassign.RecordLocator = pass.RecordLocator;
                                            //        unassign.CompartmentDesignator = pass.CompartmentDesignator;
                                            //        unassign.PassengerID = pass.PassengerID.ToString();
                                            //        unassign.PassengerNumber = (int)pass.PassengerNumber;
                                            //        unassignSeatinfo.Add(unassign);
                                            //    }
                                            //}
                                            //unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
                                            //if (num == 1) return "Fail to assign change seat, cannot swap seat with different PNR";
                                        }
                                        //if (Session["DepartExistingSeatInfo"] != null)
                                        //{

                                        //    unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
                                        //}

                                        Session["assignSeatinfoDepartFlight"] = assignSeatinfo;
                                        //Session["assignSeatinfoDepartFlightswapmulti"] = assignSeatinfoswapmulti;
                                        if (AssignSeat(assignSeatinfo, assignSeatinfo, model, EnumFlightType.DepartFlight))
                                        {
                                            DeleteXML((string)Session["SeatInfo0Xml"]);
                                            Session["DepartSeatInfo"] = Session["SeatInfo0"];
                                        }
                                        else
                                        {

                                            ClearSession();
                                            return msgList.Err999997;
                                        }
                                    }

                                    if (SeatInfo1Checking == 0 && (bool)Session["OneWay"] == false)
                                    {

                                        List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo1 = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                        List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo1 = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                        List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfoswapmulti = new List<ABS.Logic.GroupBooking.SeatInfo>();
                                        SeatAvailabilityResponse pAvailableSeatInfo1 = (SeatAvailabilityResponse)Session["pAvailableSeatInfo1"];
                                        List<PassengerContainer> newseat = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Return)) != item.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
                                        if (newseat != null && newseat.Count > 0)
                                        {
                                            //int num = 0;
                                            foreach (PassengerContainer pass in newseat)
                                            {
                                                if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Return)) != "")
                                                {
                                                    List<PassengerContainer> found = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Return)) == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
                                                    if (found != null && found.Count > 0)
                                                    {
                                                        foreach (PassengerContainer passfound in found)
                                                        {
                                                            if (pass.RecordLocator != passfound.RecordLocator)
                                                            {
                                                                Decimal start = 0, end = 0;
                                                                ABS.Logic.GroupBooking.SeatInfo assignswapmulti = new ABS.Logic.GroupBooking.SeatInfo();

                                                                foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo1.EquipmentInfos[0].Compartments)
                                                                {
                                                                    List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == passfound.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
                                                                    if (findgroup.Count == 0) continue;
                                                                    else
                                                                    {
                                                                        assignswapmulti.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                        break;
                                                                    }
                                                                }
                                                                List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo1.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assignswapmulti.SeatGroup).ToList();
                                                                foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
                                                                {
                                                                    for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                    {

                                                                        start += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                    }
                                                                    break;
                                                                }
                                                                ABS.Logic.GroupBooking.SeatInfo assignswapmulti2 = new ABS.Logic.GroupBooking.SeatInfo();
                                                                foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo1.EquipmentInfos[0].Compartments)
                                                                {
                                                                    List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
                                                                    if (findgroup.Count == 0) continue;
                                                                    else
                                                                    {
                                                                        assignswapmulti2.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                        break;
                                                                    }
                                                                }
                                                                List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount2 = pAvailableSeatInfo1.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assignswapmulti2.SeatGroup).ToList();
                                                                foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount2)
                                                                {
                                                                    for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                    {

                                                                        end += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                    }
                                                                    break;
                                                                }
                                                                int findsameRecordLocator = assignSeatinfo1.Count(item => item.RecordLocator.ToString() == pass.RecordLocator);
                                                                if (findsameRecordLocator > 0)
                                                                    Session["additionalamount"] = ((Session["additionalamount"] != null) ? Convert.ToDecimal(Session["additionalamount"]) : 0) + (start - end);
                                                                //}
                                                                //else
                                                                //{
                                                                ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                                assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Return));
                                                                assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Return));
                                                                assign.RecordLocator = pass.RecordLocator;
                                                                assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                                assign.PassengerID = pass.PassengerID.ToString();
                                                                assign.PassengerNumber = (int)pass.PassengerNumber;
                                                                foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo1.EquipmentInfos[0].Compartments)
                                                                {
                                                                    List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
                                                                    if (findgroup.Count == 0) continue;
                                                                    else
                                                                    {
                                                                        assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                        break;
                                                                    }
                                                                }
                                                                List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount3 = pAvailableSeatInfo1.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                                foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount3)
                                                                {
                                                                    for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                    {
                                                                        assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                    }
                                                                    break;
                                                                }
                                                                assignSeatinfo1.Add(assign);
                                                            }
                                                            else
                                                            {
                                                                ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                                assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Return));
                                                                assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Return));
                                                                assign.RecordLocator = pass.RecordLocator;
                                                                assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                                assign.PassengerID = pass.PassengerID.ToString();
                                                                assign.PassengerNumber = (int)pass.PassengerNumber;
                                                                foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo1.EquipmentInfos[0].Compartments)
                                                                {
                                                                    List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
                                                                    if (findgroup.Count == 0) continue;
                                                                    else
                                                                    {
                                                                        assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                        break;
                                                                    }
                                                                }
                                                                List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount3 = pAvailableSeatInfo1.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                                foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount3)
                                                                {
                                                                    for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                                    {
                                                                        assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                                    }
                                                                    break;
                                                                }
                                                                assignSeatinfo1.Add(assign);
                                                            }
                                                        }
                                                        //if (num == 1) break;
                                                    }
                                                    else
                                                    {
                                                        ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
                                                        assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Return));
                                                        assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Return));
                                                        assign.RecordLocator = pass.RecordLocator;
                                                        assign.CompartmentDesignator = pass.CompartmentDesignator;
                                                        assign.PassengerID = pass.PassengerID.ToString();
                                                        assign.PassengerNumber = (int)pass.PassengerNumber;
                                                        foreach (ABS.Navitaire.BookingManager.CompartmentInfo seat in pAvailableSeatInfo1.EquipmentInfos[0].Compartments)
                                                        {
                                                            List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = seat.Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
                                                            if (findgroup.Count == 0) continue;
                                                            else
                                                            {
                                                                assign.SeatGroup = findgroup[0].SeatGroup.ToString();
                                                                break;
                                                            }
                                                        }
                                                        List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo1.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
                                                        foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
                                                        {
                                                            for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
                                                            {
                                                                assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
                                                            }
                                                            break;
                                                        }
                                                        assignSeatinfo1.Add(assign);
                                                    }
                                                }
                                            }

                                            //foreach (PassengerContainer pass in newseat)
                                            //{
                                            //    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
                                            //    {
                                            //        ABS.Logic.GroupBooking.SeatInfo unassign = new ABS.Logic.GroupBooking.SeatInfo();
                                            //        unassign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
                                            //        unassign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
                                            //        unassign.RecordLocator = pass.RecordLocator;
                                            //        unassign.CompartmentDesignator = pass.CompartmentDesignator;
                                            //        unassign.PassengerID = pass.PassengerID.ToString();
                                            //        unassign.PassengerNumber = (int)pass.PassengerNumber;
                                            //        unassignSeatinfo.Add(unassign);
                                            //    }
                                            //}
                                            //unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
                                            //if (num == 1) return "Fail to assign change seat, cannot swap seat with different PNR";
                                        }
                                        //if (Session["ReturnExistingSeatInfo"] != null)
                                        //{
                                        //    unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnExistingSeatInfo"];
                                        //}
                                        Session["assignSeatinfoReturnFlight"] = assignSeatinfo1;
                                        //Session["assignSeatinfoReturnFlightswapmulti"] = assignSeatinfoswapmulti;
                                        if (AssignSeat(assignSeatinfo1, assignSeatinfo1, model, EnumFlightType.ReturnFlight))
                                        //if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"], unassignSeatinfo, model, EnumFlightType.ReturnFlight))
                                        {
                                            DeleteXML((string)Session["SeatInfo1Xml"]);
                                            Session["ReturnSeatInfo"] = Session["SeatInfo1"];
                                        }
                                        else
                                        {

                                            ClearSession();
                                            return msgList.Err999997;
                                        }
                                    }



                                    //end, update total amount
                                    assignSeatDone = true;
                                    //return "";
                                    //Response.Redirect("~/pages/SeatSummary.aspx");
                                }
                                else
                                {
                                    ClearSession();
                                    return msgList.Err999996;
                                    //lblErr.Text = msgList.Err999996;
                                    //pnlErr.Visible = true;
                                    //Session["ErrorMsg"] = lblErr.Text;
                                    //Response.Redirect("~/seats.aspx");
                                }
                            }


                            break;
                            #endregion
                    }
                }

                if (assignSeatDone)
                {
                    //begin, update total amount
                    if (!string.IsNullOrEmpty(hID.Value))
                    {
                        decimal numValue;
                        decimal TotalSeatDepart = 0;
                        decimal TotalSeatReturn = 0;

                        string TransID = (string)Session["TransID"];

                        if (Session["DepartFlightSeatFees"] != null)
                        {
                            if (decimal.TryParse(Session["DepartFlightSeatFees"].ToString(), out numValue)) TotalSeatDepart = TotalSeatDepart + Convert.ToDecimal(Session["DepartFlightSeatFees"].ToString());
                        }
                        if (Session["DepartConnectingFlightSeatFees"] != null)
                        {
                            if (decimal.TryParse(Session["DepartConnectingFlightSeatFees"].ToString(), out numValue)) TotalSeatDepart = TotalSeatDepart + Convert.ToDecimal(Session["DepartConnectingFlightSeatFees"].ToString());
                        }
                        if (Session["DepartConnectingFlightSeatFees2"] != null)
                        {
                            if (decimal.TryParse(Session["DepartConnectingFlightSeatFees2"].ToString(), out numValue)) TotalSeatDepart = TotalSeatDepart + Convert.ToDecimal(Session["DepartConnectingFlightSeatFees2"].ToString());
                        }

                        if (Session["ReturnFlightSeatFees"] != null)
                        {
                            if (decimal.TryParse(Session["ReturnFlightSeatFees"].ToString(), out numValue)) TotalSeatReturn = TotalSeatReturn + Convert.ToDecimal(Session["ReturnFlightSeatFees"].ToString());
                        }
                        if (Session["ReturnConnectingFlightSeatFees"] != null)
                        {
                            if (decimal.TryParse(Session["ReturnConnectingFlightSeatFees"].ToString(), out numValue)) TotalSeatReturn = TotalSeatReturn + Convert.ToDecimal(Session["ReturnConnectingFlightSeatFees"].ToString());
                        }
                        if (Session["ReturnConnectingFlightSeatFees2"] != null)
                        {
                            if (decimal.TryParse(Session["ReturnConnectingFlightSeatFees2"].ToString(), out numValue)) TotalSeatReturn = TotalSeatReturn + Convert.ToDecimal(Session["ReturnConnectingFlightSeatFees2"].ToString());
                        }

                        //if (lstPassenger != null)
                        //{
                        //    HttpContext.Current.Session.Remove("listPassengers");
                        //    HttpContext.Current.Session.Add("listPassengers", lstPassenger);
                        //}
                        if (Session["dtPassengers"] != null && Session["totalamountdueseatfeeall"] != null)
                        {
                            if (Page.IsCallback)
                                ASPxCallback.RedirectOnCallback("SeatSummary.aspx?k=" + HashingKey + "&TransID=" + TransId);
                        }
                        else//added by romy
                        {
                            ASPxCallback.RedirectOnCallback(Shared.MySite.PublicPages.InvalidPage);
                        }

                        return "";
                    }
                    else
                    {
                        return msgList.Err999995;
                    }
                }
                else
                {
                    return msgList.Err999995;
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                return ex.Message;
                //lblErr.Text = ex.ToString();
            }
        }

        private bool AssignSeat(List<ABS.Logic.GroupBooking.SeatInfo> _seatInfo, List<ABS.Logic.GroupBooking.SeatInfo> _usassignSeatInfo,
            BookingControl.TemFlight model, EnumFlightType pFlightType, Boolean bAssignSeat = false)
        {
            var profiler = MiniProfiler.Current;
            BookingControl bookingControl = new BookingControl();
            DataTable dtDetail = objBooking.dtTransDetail();
            string Signature;
            Decimal AmountDueBefore = 0;

            List<ABS.Logic.GroupBooking.SeatInfo> existingSeatInfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
            ABS.Logic.GroupBooking.SeatInfo seatInfo;
            //change to new add-On table, Tyas
            //dtPassenger = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTable(TransId, true);

            decimal SeatFee = 0;

            if (Session["listBookingDetail"] == null)
            {
                listBookingDetail = objBooking.GetAllBK_TRANSDTLFilterAll(TransId, 0);
            }
            else
            {
                listBookingDetail = Session["listBookingDetail"] as List<BookingTransactionDetail>;
            }

            if (HttpContext.Current.Session["TransDetail"] != null)
            {
                dtDetail = (DataTable)HttpContext.Current.Session["TransDetail"];
            }

            if (dtDetail.Rows.Count != 0)
            {
                #region "UnAssign SEat => Assign Seat"
                for (int j = 0; j < dtDetail.Rows.Count; j++)
                {

                    SeatFee = 0;
                    if (dtDetail.Rows[j]["RecordLocator"] != null)
                    {

                        if (dtDetail.Rows[j]["SellKey"] != null && string.IsNullOrEmpty(dtDetail.Rows[j]["SellKey"].ToString()))
                        {
                            using (profiler.Step("Navitaire:AgentLogon"))
                            {
                                Signature = absNavitaire.AgentLogon();
                            }
                            dtDetail.Rows[j]["SellKey"] = Signature;
                        }
                        else
                        {
                            Signature = dtDetail.Rows[j]["SellKey"].ToString();
                        }

                        BookingTransactionDetail BookingDetail = new BookingTransactionDetail();
                        BookingDetail = objBooking.GetBK_TRANSDTLFlightByPNR(dtDetail.Rows[j]["RecordLocator"].ToString().Trim(), 0);
                        ABS.Navitaire.BookingManager.Booking bookingResp = bookingControl.GetBookingFromState(dtDetail.Rows[j]["SellKey"].ToString());

                        if (bookingResp == null || bookingResp.BookingHold == null)
                        {
                            GetBookingResponse resp = bookingControl.GetBookingByPNR(dtDetail.Rows[j]["RecordLocator"].ToString().Trim(), dtDetail.Rows[j]["SellKey"].ToString());
                            if (resp != null && resp.Booking != null && resp.Booking.BookingHold != null)
                                bookingResp = resp.Booking;
                            else
                                return false;
                        }

                        //string initialXml = absNavitaire.GetXMLString(bookingResp);

                        if (BookingDetail == null)
                        {
                            return false;
                        }

                        try
                        {
                            bAssignSeat = false;
                            BookingUpdateResponseData UnassignResponse = new BookingUpdateResponseData();
                            BookingUpdateResponseData AssignResponse = new BookingUpdateResponseData();
                            totalamountdueseatfeeallcommitperPNR = 0;
                            totalamountdueseatfeeallperPNR = 0;




                            //Unassign Seat
                            int k = 0;
                            int[] aunassignPassengerNumber = null;
                            int[] aunassignPassengerID = null;
                            string[] aunassignUnitDisignator = null;
                            string[] aunassigncompartmentDesignator = null;
                            string[] aunassignPNR = null;
                            decimal unassignseatamount = 0;
                            //decimal totalamountdueseatfeeall = 0;
                            if (_usassignSeatInfo != null)
                            {

                                int unassignSeatInfoCount1 = _usassignSeatInfo.Count(a => a.SelectedSeat.Trim() != "" && a.RecordLocator.Trim() == BookingDetail.RecordLocator.Trim());
                                aunassignPassengerNumber = new int[unassignSeatInfoCount1];
                                aunassignPassengerID = new int[unassignSeatInfoCount1];
                                aunassignUnitDisignator = new string[unassignSeatInfoCount1];
                                aunassigncompartmentDesignator = new string[unassignSeatInfoCount1];
                                aunassignPNR = new string[unassignSeatInfoCount1];

                                foreach (ABS.Logic.GroupBooking.SeatInfo rSeatInfo in _usassignSeatInfo)
                                {
                                    if (BookingDetail.RecordLocator.Trim() == rSeatInfo.RecordLocator.Trim() && rSeatInfo.SelectedSeat.Trim() != "")
                                    {
                                        aunassignPassengerNumber[k] = rSeatInfo.PassengerNumber;
                                        aunassignPassengerID[k] = Convert.ToInt32(rSeatInfo.PassengerNumber);
                                        aunassignUnitDisignator[k] = rSeatInfo.CurrentSeat; //.SelectedSeat;
                                        aunassigncompartmentDesignator[k] = rSeatInfo.CompartmentDesignator;
                                        unassignseatamount += rSeatInfo.SeatAmount;
                                        k += 1;
                                    }
                                }
                            }

                            AmountDueBefore += bookingResp.BookingSum.BalanceDue;
                            //End Unassign Seat


                            string STD = "";

                            switch (pFlightType)
                            {
                                #region "Depart Flight"
                                case EnumFlightType.DepartFlight:
                                    STD = bookingResp.Journeys[0].Segments[0].STD.ToString();
                                    //added by ketee, check if assign seat = true
                                    if (bAssignSeat == false)
                                    {
                                        UnassignResponse = absNavitaire.UnAssignSeats(true, Signature, bookingResp.Journeys[0].Segments[0].DepartureStation, bookingResp.Journeys[0].Segments[0].ArrivalStation, bookingResp.Journeys[0].Segments[0].STD.ToString(),
                                               aunassignPassengerNumber, aunassignPassengerID, aunassignUnitDisignator, aunassigncompartmentDesignator,
                                               bookingResp.Journeys[0].Segments[0].FlightDesignator.CarrierCode, bookingResp.Journeys[0].Segments[0].FlightDesignator.FlightNumber, bookingResp.Journeys[0].Segments[0].FlightDesignator.OpSuffix, bookingResp.RecordLocator);

                                        if (UnassignResponse != null)
                                        {
                                            if (UnassignResponse.Success != null)
                                            {

                                                decimal totalSeatFees = 0;

                                                if (UnassignResponse.Success.PNRAmount.BalanceDue != 0)
                                                {
                                                    decimal amountdue = UnassignResponse.Success.PNRAmount.BalanceDue;
                                                    if (Session["Balance"] != null && Convert.ToDecimal(Session["Balance"]) == UnassignResponse.Success.PNRAmount.BalanceDue)
                                                    {
                                                        amountdue = 0;
                                                        Session["Balance"] = null;
                                                    }
                                                    //tyas
                                                    //decimal amountdue = UnassignResponse.Success.PNRAmount.BalanceDue;
                                                    totalSeatFees = unassignseatamount;
                                                    decimal departFee = Convert.ToDecimal(Session["DepartFlightSeatFees"].ToString());
                                                    Session["DepartFlightSeatFees"] = (totalSeatFees + amountdue);
                                                    Session["DepartExistingSeatInfo"] = existingSeatInfo;

                                                    totalamountdueseatfeeall += Convert.ToDecimal(Session["DepartFlightSeatFees"].ToString());
                                                }
                                                else
                                                {
                                                    totalamountdueseatfeeall += unassignseatamount;
                                                }
                                            }
                                            else
                                            {
                                                return false;
                                            }
                                        }

                                    }
                                    break;
                                #endregion

                                #region "Return Flight"
                                case EnumFlightType.ReturnFlight:
                                    STD = bookingResp.Journeys[1].Segments[0].STD.ToString();

                                    //added by ketee, check if assign seat = true
                                    if (bAssignSeat == false)
                                    {
                                        UnassignResponse = absNavitaire.UnAssignSeats(true, Signature, bookingResp.Journeys[1].Segments[0].DepartureStation, bookingResp.Journeys[1].Segments[0].ArrivalStation, STD,
                                               aunassignPassengerNumber, aunassignPassengerID, aunassignUnitDisignator, aunassigncompartmentDesignator,
                                               bookingResp.Journeys[1].Segments[0].FlightDesignator.CarrierCode, bookingResp.Journeys[1].Segments[0].FlightDesignator.FlightNumber, bookingResp.Journeys[1].Segments[0].FlightDesignator.OpSuffix, bookingResp.RecordLocator);

                                        if (UnassignResponse != null)
                                        {
                                            if (UnassignResponse.Success != null)
                                            {
                                                decimal totalSeatFees = 0;

                                                if (UnassignResponse.Success.PNRAmount.BalanceDue != 0)
                                                {
                                                    //tyas
                                                    decimal amountdue = UnassignResponse.Success.PNRAmount.BalanceDue;
                                                    if (Session["Balance"] != null && Convert.ToDecimal(Session["Balance"]) == UnassignResponse.Success.PNRAmount.BalanceDue)
                                                    {
                                                        amountdue = 0;
                                                        Session["Balance"] = null;
                                                    }
                                                    totalSeatFees = unassignseatamount;
                                                    decimal departFee = Convert.ToDecimal(Session["ReturnFlightSeatFees"].ToString());
                                                    Session["ReturnFlightSeatFees"] = (totalSeatFees + amountdue);
                                                    Session["ReturnExistingSeatInfo"] = existingSeatInfo;

                                                    totalamountdueseatfeeall += Convert.ToDecimal(Session["ReturnFlightSeatFees"].ToString());
                                                }
                                                else
                                                {
                                                    totalamountdueseatfeeall += unassignseatamount;
                                                }
                                            }
                                            else
                                            {
                                                return false;
                                            }
                                        }

                                    }
                                    break;
                                #endregion

                                #region "Depart Connecting Flight"
                                case EnumFlightType.DepartConnectingFlight:
                                    STD = bookingResp.Journeys[0].Segments[0].STD.ToString();

                                    //added by ketee, check if assign seat = true
                                    if (bAssignSeat == false)
                                    {
                                        UnassignResponse = absNavitaire.UnAssignSeats(true, Signature, bookingResp.Journeys[0].Segments[0].DepartureStation, bookingResp.Journeys[0].Segments[0].ArrivalStation, STD,
                                                aunassignPassengerNumber, aunassignPassengerID, aunassignUnitDisignator, aunassigncompartmentDesignator,
                                                bookingResp.Journeys[0].Segments[0].FlightDesignator.CarrierCode, bookingResp.Journeys[0].Segments[0].FlightDesignator.FlightNumber, bookingResp.Journeys[0].Segments[0].FlightDesignator.OpSuffix, bookingResp.RecordLocator);

                                        if (UnassignResponse != null)
                                        {
                                            if (UnassignResponse.Success != null)
                                            {
                                                decimal totalSeatFees = 0;

                                                if (UnassignResponse.Success.PNRAmount.BalanceDue != 0)
                                                {
                                                    //tyas
                                                    decimal amountdue = UnassignResponse.Success.PNRAmount.BalanceDue;
                                                    if (Session["Balance"] != null && Convert.ToDecimal(Session["Balance"]) == UnassignResponse.Success.PNRAmount.BalanceDue)
                                                    {
                                                        amountdue = 0;
                                                        Session["Balance"] = null;
                                                    }
                                                    totalSeatFees = unassignseatamount;
                                                    decimal departFee = Convert.ToDecimal(Session["DepartConnectingFlightSeatFees"].ToString());
                                                    Session["DepartConnectingFlightSeatFees"] = (totalSeatFees + amountdue);
                                                    Session["DepartConnectingExistingSeatInfo"] = existingSeatInfo;

                                                    totalamountdueseatfeeall += Convert.ToDecimal(Session["DepartConnectingFlightSeatFees"].ToString());
                                                }
                                                else
                                                {
                                                    totalamountdueseatfeeall += unassignseatamount;
                                                }
                                            }
                                            else
                                            {
                                                return false;
                                            }
                                        }

                                    }

                                    break;
                                #endregion

                                #region "Depart Connecting Flight 2"
                                case EnumFlightType.DepartConnectingFlight2:
                                    STD = bookingResp.Journeys[0].Segments[1].STD.ToString();

                                    //added by ketee, check if assign seat = true
                                    if (bAssignSeat == false)
                                    {
                                        UnassignResponse = absNavitaire.UnAssignSeats(true, Signature, bookingResp.Journeys[0].Segments[1].DepartureStation, bookingResp.Journeys[0].Segments[1].ArrivalStation, STD,
                                               aunassignPassengerNumber, aunassignPassengerID, aunassignUnitDisignator, aunassigncompartmentDesignator,
                                               bookingResp.Journeys[0].Segments[1].FlightDesignator.CarrierCode, bookingResp.Journeys[0].Segments[1].FlightDesignator.FlightNumber, bookingResp.Journeys[0].Segments[1].FlightDesignator.OpSuffix, bookingResp.RecordLocator);

                                        if (UnassignResponse != null)
                                        {
                                            if (UnassignResponse.Success != null)
                                            {
                                                decimal totalSeatFees = 0;

                                                if (UnassignResponse.Success.PNRAmount.BalanceDue != 0)
                                                {
                                                    //tyas
                                                    decimal amountdue = UnassignResponse.Success.PNRAmount.BalanceDue;
                                                    if (Session["Balance"] != null && Convert.ToDecimal(Session["Balance"]) == UnassignResponse.Success.PNRAmount.BalanceDue)
                                                    {
                                                        amountdue = 0;
                                                        Session["Balance"] = null;
                                                    }
                                                    totalSeatFees = unassignseatamount;
                                                    decimal departFee = Convert.ToDecimal(Session["DepartConnectingFlightSeatFees2"].ToString());
                                                    Session["DepartConnectingFlightSeatFees2"] = (totalSeatFees + amountdue);
                                                    Session["DepartConnectingExistingSeatInfo2"] = existingSeatInfo;

                                                    totalamountdueseatfeeall += Convert.ToDecimal(Session["DepartConnectingFlightSeatFees2"].ToString());
                                                }
                                                else
                                                {
                                                    totalamountdueseatfeeall += unassignseatamount;
                                                }
                                            }
                                            else
                                            {
                                                return false;
                                            }
                                        }

                                    }
                                    break;
                                #endregion

                                #region "Return Connecting Flight"
                                case EnumFlightType.ReturnConnectingFlight:
                                    STD = bookingResp.Journeys[1].Segments[0].STD.ToString();

                                    //added by ketee, check if assign seat = true
                                    if (bAssignSeat == false)
                                    {
                                        UnassignResponse = absNavitaire.UnAssignSeats(true, Signature, bookingResp.Journeys[1].Segments[0].DepartureStation, bookingResp.Journeys[1].Segments[0].ArrivalStation, STD,
                                               aunassignPassengerNumber, aunassignPassengerID, aunassignUnitDisignator, aunassigncompartmentDesignator,
                                               bookingResp.Journeys[1].Segments[0].FlightDesignator.CarrierCode, bookingResp.Journeys[1].Segments[0].FlightDesignator.FlightNumber, bookingResp.Journeys[1].Segments[0].FlightDesignator.OpSuffix, bookingResp.RecordLocator);

                                        if (UnassignResponse != null)
                                        {
                                            if (UnassignResponse.Success != null)
                                            {
                                                decimal totalSeatFees = 0;

                                                if (UnassignResponse.Success.PNRAmount.BalanceDue != 0)
                                                {
                                                    //tyas
                                                    decimal amountdue = UnassignResponse.Success.PNRAmount.BalanceDue;
                                                    if (Session["Balance"] != null && Convert.ToDecimal(Session["Balance"]) == UnassignResponse.Success.PNRAmount.BalanceDue)
                                                    {
                                                        amountdue = 0;
                                                        Session["Balance"] = null;
                                                    }
                                                    totalSeatFees = unassignseatamount;
                                                    decimal departFee = Convert.ToDecimal(Session["ReturnConnectingFlightSeatFees"].ToString());
                                                    Session["ReturnConnectingFlightSeatFees"] = (totalSeatFees + amountdue);
                                                    Session["ReturnConnectingExistingSeatInfo"] = existingSeatInfo;

                                                    totalamountdueseatfeeall += Convert.ToDecimal(Session["ReturnConnectingFlightSeatFees"].ToString());
                                                }
                                                else
                                                {
                                                    totalamountdueseatfeeall += unassignseatamount;
                                                }
                                            }
                                            else
                                            {
                                                return false;
                                            }
                                        }

                                    }

                                    break;
                                #endregion

                                #region "Return Connecting Flight2"
                                case EnumFlightType.ReturnConnectingFlight2:
                                    STD = bookingResp.Journeys[1].Segments[1].STD.ToString();

                                    //added by ketee, check if assign seat = true
                                    if (bAssignSeat == false)
                                    {
                                        UnassignResponse = absNavitaire.UnAssignSeats(true, Signature, bookingResp.Journeys[1].Segments[1].DepartureStation, bookingResp.Journeys[1].Segments[1].ArrivalStation, STD,
                                                aunassignPassengerNumber, aunassignPassengerID, aunassignUnitDisignator, aunassigncompartmentDesignator,
                                                bookingResp.Journeys[1].Segments[1].FlightDesignator.CarrierCode, bookingResp.Journeys[1].Segments[1].FlightDesignator.FlightNumber, bookingResp.Journeys[1].Segments[1].FlightDesignator.OpSuffix, bookingResp.RecordLocator);

                                        if (UnassignResponse != null)
                                        {
                                            if (UnassignResponse.Success != null)
                                            {
                                                decimal totalSeatFees = 0;

                                                if (UnassignResponse.Success.PNRAmount.BalanceDue != 0)
                                                {
                                                    //tyas
                                                    decimal amountdue = UnassignResponse.Success.PNRAmount.BalanceDue;
                                                    if (Session["Balance"] != null && Convert.ToDecimal(Session["Balance"]) == UnassignResponse.Success.PNRAmount.BalanceDue)
                                                    {
                                                        amountdue = 0;
                                                        Session["Balance"] = null;
                                                    }
                                                    totalSeatFees = unassignseatamount;
                                                    decimal departFee = Convert.ToDecimal(Session["ReturnConnectingFlightSeatFees2"].ToString());
                                                    Session["ReturnConnectingFlightSeatFees2"] = (totalSeatFees + amountdue);
                                                    Session["ReturnConnectingExistingSeatInfo2"] = existingSeatInfo;

                                                    totalamountdueseatfeeall += Convert.ToDecimal(Session["ReturnConnectingFlightSeatFees2"].ToString());
                                                }
                                                else
                                                {
                                                    totalamountdueseatfeeall += unassignseatamount;
                                                }
                                            }
                                            else
                                            {
                                                return false;
                                            }
                                        }

                                    }

                                    break;
                                #endregion


                            }

                            bAssignSeat = true;
                            if (bAssignSeat)
                            {
                                Session["totalamountdueseatfeeall"] = (totalamountdueseatfeeall + Math.Abs(((Session["additionalamount"] != null) ? Convert.ToDecimal(Session["additionalamount"]) : 0)));
                                if (AmountDueBefore > 0)
                                {
                                    Session["havebalance"] = true;
                                }
                                if (!string.IsNullOrEmpty(hID.Value))
                                {
                                    //update passenger seats
                                    DataTable dtPass = new DataTable();
                                    if (HttpContext.Current.Session["dtPassengers"] != null)
                                    {
                                        dtPass = (DataTable)HttpContext.Current.Session["dtPassengers"];
                                        if (_seatInfo.Count > 0)
                                        {
                                            for (int i = 0; i < dtPass.Rows.Count; i++)
                                            {

                                                foreach (ABS.Logic.GroupBooking.SeatInfo rSeatInfo in _seatInfo)
                                                {
                                                    if (dtPass.Rows[i]["PNR"].ToString().Trim() == rSeatInfo.RecordLocator.Trim() &&
                                                        dtPass.Rows[i]["PassengerID"].ToString() == rSeatInfo.PassengerID && rSeatInfo.SelectedSeat.Trim() != "")
                                                    {
                                                        switch (pFlightType)
                                                        {
                                                            case EnumFlightType.DepartFlight:
                                                                dtPass.Rows[i]["DepartSeat"] = rSeatInfo.SelectedSeat;
                                                                break;
                                                            case EnumFlightType.DepartConnectingFlight:
                                                                dtPass.Rows[i]["DepartConnectingSeat"] = rSeatInfo.SelectedSeat;
                                                                break;
                                                            case EnumFlightType.ReturnFlight:
                                                                dtPass.Rows[i]["ReturnSeat"] = rSeatInfo.SelectedSeat;
                                                                break;
                                                            case EnumFlightType.ReturnConnectingFlight:
                                                                dtPass.Rows[i]["ReturnConnectingSeat"] = rSeatInfo.SelectedSeat;
                                                                break;
                                                        }
                                                    }
                                                }
                                            }
                                            Session["dtPassengers"] = dtPass;
                                        }



                                    }



                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            SystemLog.Notifier.Notify(ex);
                            log.Error(ex, ex.Message);
                            //sTraceLog(ex.ToString);
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }

                }
                #endregion




                return true;
            }
            else
            {
                return false;
            }
        }

        protected void ClearSeatFeeValue()
        {
            //DataTable dataClass = (DataTable)HttpContext.Current.Session["dataClassTrans"];
            //if (dataClass != null)
            //{
            //    ////totalConnectingSeatFees = null;
            //    ////totalConnectingSeatFees = new string[dataClass.Rows.Count, dataClass.Rows.Count];
            //    ////totalConnectingSeatFees2 = null;
            //    ////totalConnectingSeatFees2 = new string[dataClass.Rows.Count, dataClass.Rows.Count];

            //    for (int j = 0; j < dataClass.Rows.Count; j++)
            //    {
            //        dataClass.Rows[j]["FullPrice"] = Convert.ToDecimal(dataClass.Rows[j]["FullPrice"].ToString()) - Convert.ToDecimal(dataClass.Rows[j]["SeatChrg"].ToString());
            //        dataClass.Rows[j]["SeatChrg"] = 0;

            //        ////totalConnectingSeatFees[j, 0] = dataClass.Rows[j]["SellSignature"].ToString();
            //        ////totalConnectingSeatFees2[j, 0] = dataClass.Rows[j]["SellSignature"].ToString();
            //    }
            //}
            //HttpContext.Current.Session["dataClassTrans"] = dataClass;

            Session["DepartFlightSeatFees"] = 0;
            Session["ReturnFlightSeatFees"] = 0;
            Session["DepartConnectingFlightSeatFees"] = 0;
            Session["DepartConnectingFlightSeatFees2"] = 0;
            Session["ReturnConnectingFlightSeatFees"] = 0;
            Session["ReturnConnectingFlightSeatFees2"] = 0;


        }

        protected void UpdateTotalAmount(decimal TotalSeatDepart, decimal TotalSeatReturn, ref decimal TotalAmountGoing, ref decimal TotalAmountReturn)
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

            //depart
            decimal TotalAmount;
            DataTable dtBDFee = objBooking.dtBreakdownFee();
            dtBDFee = (DataTable)HttpContext.Current.Session["dataBDFeeDepart"];
            dtBDFee.Rows[0]["Seat"] = Convert.ToDecimal(TotalSeatDepart);

            TotalAmount = Convert.ToDecimal(dtBDFee.Rows[0]["Fare"]) * model.TemFlightPaxNum;
            if (dtBDFee.Rows[0]["Infant"] != DBNull.Value)
            {
                TotalAmount += Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * model.TemFlightADTNum + Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Svc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) + Convert.ToDecimal(dtBDFee.Rows[0]["SSR"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Seat"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Infant"]);
            }
            else
            {
                TotalAmount += Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * model.TemFlightADTNum + Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Svc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) + Convert.ToDecimal(dtBDFee.Rows[0]["SSR"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Seat"]);
            }
            //TotalAmount += Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * model.TemFlightADTNum + Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Svc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) + Convert.ToDecimal(dtBDFee.Rows[0]["SSR"]) + +Convert.ToDecimal(dtBDFee.Rows[0]["Seat"]);
            if (Convert.ToInt32(model.TemFlightCHDNum.ToString()) != 0)
            {
                TotalAmount += Convert.ToDecimal(dtBDFee.Rows[0]["ChApt"]) * model.TemFlightCHDNum;
            }
            TotalAmountGoing = TotalAmount;
            HttpContext.Current.Session["dataBDFeeDepart"] = dtBDFee;

            strExpr = "TemFlightId = '" + departID + "'";
            strSort = "";
            dt = (DataTable)HttpContext.Current.Session["TempFlight"];
            foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
            foundRows[0]["TemFlightTotalAmount"] = (TotalAmount);
            HttpContext.Current.Session["TempFlight"] = dt;

            //return
            if (model2 == null == false)
            {
                dtBDFee = objBooking.dtBreakdownFee();
                dtBDFee = (DataTable)HttpContext.Current.Session["dataBDFeeReturn"];
                dtBDFee.Rows[0]["Seat"] = Convert.ToDecimal(TotalSeatReturn);

                TotalAmount = Convert.ToDecimal(dtBDFee.Rows[0]["Fare"]) * model.TemFlightPaxNum;
                if (dtBDFee.Rows[0]["Infant"] != DBNull.Value)
                {
                    TotalAmount += Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * model.TemFlightADTNum + Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Svc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) + Convert.ToDecimal(dtBDFee.Rows[0]["SSR"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Seat"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Infant"]);
                }
                else
                {
                    TotalAmount += Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * model.TemFlightADTNum + Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Svc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) + Convert.ToDecimal(dtBDFee.Rows[0]["SSR"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Seat"]);
                }
                //TotalAmount += Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * model.TemFlightADTNum + Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Svc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"]) + Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) + Convert.ToDecimal(dtBDFee.Rows[0]["SSR"]) + +Convert.ToDecimal(dtBDFee.Rows[0]["Seat"]);
                if (Convert.ToInt32(model.TemFlightCHDNum.ToString()) != 0)
                {
                    TotalAmount += Convert.ToDecimal(dtBDFee.Rows[0]["ChApt"]) * model.TemFlightCHDNum;
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

        protected void assignSeatCallBack_Callback(object source, CallbackEventArgs e)
        {
            if (e.Parameter != null && e.Parameter.ToLower() == "back")
            {
                string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), hID.Value, "");
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("../public/bookingdetail.aspx?k=" + hashkey + "&TransID=" + hID.Value);
                //ASPxWebControl.RedirectOnCallback("addon.aspx");
                return;
            }
            hResult.Value = ValidateSeat();
            e.Result = hResult.Value;
            //if (e.Result == "")
            //{
            //    string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), hID.Value, "");
            //    DevExpress.Web.ASPxWebControl.RedirectOnCallback("../public/bookingdetail.aspx?k=" + hashkey + "&TransID=" + hID.Value);
            //}
        }

        public bool DeleteXML(string URL)
        {
            try
            {
                string logFilePath = "";
                logFilePath = Request.PhysicalApplicationPath + URL;
                if (System.IO.File.Exists(logFilePath))
                {
                    System.IO.File.Delete(logFilePath);

                }
                return true;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                return false;
            }
        }

        public bool CheckDefaultSeatExist(EnumFlightType FlightType)
        {
            try
            {
                return true;
                //DataTable dtPassengerDetails = null;
                //dtPassengerDetails = PassengerSeatDetails(FlightType);
                //if ((dtPassengerDetails != null) && dtPassengerDetails.Rows.Count > 0)
                //{
                //    foreach (DataRow rowPassenger in dtPassengerDetails.Rows)
                //    {
                //        if (!string.IsNullOrEmpty(rowPassenger.Item("UnitDesignator").ToString))
                //        {
                //            return true;
                //            break; // TODO: might not be correct. Was : Exit For
                //        }
                //    }
                //    return false;
                //}
                //else
                //{
                //    return false;
                //}

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                return false;
            }
        }

        protected static bool IsNumeric(string s)
        {
            double result;
            return s != null && Double.TryParse(s, out result);
        }

        #endregion


    }
}