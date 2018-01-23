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
using StackExchange.Profiling;


namespace GroupBooking.Web
{
    public partial class SeatSummary : System.Web.UI.Page
    {
        ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogControl log = new LogControl();
        SystemLog SystemLog = new SystemLog();
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

        decimal totalseatfeeallcommit = 0;
        decimal totalamountdueseatfeeallcommit = 0;
        
        decimal totalseatfeeallcommitgoing = 0;
        decimal totalseatfeeallcommitreturn = 0;
        decimal totalamountdueseatfeeallcommitgoing = 0;
        decimal totalamountdueseatfeeallcommitreturn = 0;

        decimal balancedue = 0;
        string TransID;
        EnumFlight eFlight;

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
        protected void Page_Load(object sender, EventArgs e)
        {
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;

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
                using (profiler.Step("LoadData"))
                {
                    LoadData();
                }
            }
        }

        protected void assignSeatCallBack_Callback(object source, CallbackEventArgs e)
        {
            if (e.Parameter != null && e.Parameter.ToLower() == "back")
            {
                string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), TransID, "");
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("../public/bookingdetail.aspx?k=" + hashkey + "&TransID=" + TransID);
                //ASPxWebControl.RedirectOnCallback("addon.aspx");
                return;
            }
            hResult.Value = ValidateSeat();
            e.Result = hResult.Value;

        }

        protected void ClearSession()
        {
            Session["totalseatfeeallcommit"] = null;
            Session["totalseatfeeallcommitgoing"] = null;
            Session["totalseatfeeallcommitreturn"] = null;
            Session["totalamountdueseatfeeallcommit"] = null;
            Session["totalamountdueseatfeeallcommitgoing"] = null;
            Session["totalamountdueseatfeeallcommitreturn"] = null;
            //HttpContext.Current.Session["havebalance"] = null;
            HttpContext.Current.Session["commit"] = null;
            HttpContext.Current.Session["Forfeit"] = null;
            Session["failedassign"] = null;
            Session["foundzero"] = null;
        }

        protected void LoadData()
        {
            List<ABS.Logic.GroupBooking.Booking.BookingTransactionDetail> BookingTransactionDetail = new List<BookingTransactionDetail>();
            try
            {
                int count = 0;
                BookingTransactionDetail = objBooking.Get_TRANSDTL(AgentSet.AgentID, TransID);

                if (Session["dtPassengers"] != null)
                {
                    dtPass = (DataTable)Session["dtPassengers"];
                    Session["dtPassengers"] = dtPass;

                    if (dtPass != null)
                    {
                        lblCurrency.Text = dtPass.Rows[0]["Currency"].ToString();
                        gvPassenger.DataSource = dtPass;
                        gvPassenger.DataBind();
                        gvPassenger.ExpandAll();

                        if (BookingTransactionDetail != null && BookingTransactionDetail.Count > 0)
                        {
                            List<ABS.Logic.GroupBooking.Booking.BookingTransactionDetail> listreturn = BookingTransactionDetail.Where(item => item.Origin != BookingTransactionDetail[0].Origin).ToList();
                            if (listreturn == null || listreturn.Count == 0)
                            {
                                gvPassenger.Columns["ReturnSeat"].Visible = false;
                                if (Regex.Replace(BookingTransactionDetail[0].Transit, @"\s+", "") != "")
                                {
                                    gvPassenger.Columns["DepartConnectingSeat"].Visible = true;
                                    gvPassenger.Columns["ReturnConnectingSeat"].Visible = false;

                                    count = 2;
                                    gvPassenger.Columns["DepartSeat"].Caption = BookingTransactionDetail[0].Origin + " - " + BookingTransactionDetail[0].Transit;
                                    gvPassenger.Columns["DepartConnectingSeat"].Caption = BookingTransactionDetail[0].Transit + " - " + BookingTransactionDetail[0].Destination;

                                }
                                else
                                {
                                    gvPassenger.Columns["DepartConnectingSeat"].Visible = false;
                                    gvPassenger.Columns["ReturnConnectingSeat"].Visible = false;

                                    gvPassenger.Columns["DepartSeat"].Caption = BookingTransactionDetail[0].Origin + " - " + BookingTransactionDetail[0].Destination;
                                    count = 2;
                                }
                            }
                            else
                            {
                                gvPassenger.Columns["ReturnSeat"].Visible = true;
                                if (Regex.Replace(BookingTransactionDetail[0].Transit, @"\s+", "") != "")
                                {
                                    gvPassenger.Columns["DepartConnectingSeat"].Visible = true;
                                    //count = 3;
                                    gvPassenger.Columns["DepartSeat"].Caption = BookingTransactionDetail[0].Origin + " - " + BookingTransactionDetail[0].Transit;
                                    gvPassenger.Columns["DepartConnectingSeat"].Caption = BookingTransactionDetail[0].Transit + " - " + BookingTransactionDetail[0].Destination;

                                    if (Regex.Replace(BookingTransactionDetail[1].Transit, @"\s+", "") != "")
                                    {
                                        count = 4;
                                        gvPassenger.Columns["ReturnConnectingSeat"].Visible = true;
                                        gvPassenger.Columns["ReturnSeat"].Caption = BookingTransactionDetail[1].Origin + " - " + BookingTransactionDetail[1].Transit;
                                        gvPassenger.Columns["ReturnConnectingSeat"].Caption = BookingTransactionDetail[1].Transit + " - " + BookingTransactionDetail[0].Destination;
                                    }
                                    else
                                    {
                                        count = 3;
                                        Session["ReturnConnecting"] = false;
                                        gvPassenger.Columns["ReturnConnectingSeat"].Visible = false;
                                        gvPassenger.Columns["ReturnSeat"].Caption = BookingTransactionDetail[1].Origin + " - " + BookingTransactionDetail[1].Destination;
                                    }
                                }
                                else
                                {
                                    gvPassenger.Columns["DepartConnectingSeat"].Visible = false;
                                    gvPassenger.Columns["DepartSeat"].Caption = BookingTransactionDetail[0].Origin + " - " + BookingTransactionDetail[0].Destination;
                                    if (Regex.Replace(BookingTransactionDetail[1].Transit, @"\s+", "") != "")
                                    {
                                        count = 4;
                                        gvPassenger.Columns["ReturnConnectingSeat"].Visible = true;
                                        gvPassenger.Columns["ReturnSeat"].Caption = BookingTransactionDetail[1].Origin + " - " + BookingTransactionDetail[1].Transit;
                                        gvPassenger.Columns["ReturnConnectingSeat"].Caption = BookingTransactionDetail[1].Transit + " - " + BookingTransactionDetail[0].Destination;
                                    }
                                    else
                                    {
                                        count = 2;
                                        gvPassenger.Columns["ReturnConnectingSeat"].Visible = false;
                                        gvPassenger.Columns["ReturnSeat"].Caption = BookingTransactionDetail[1].Origin + " - " + BookingTransactionDetail[1].Destination;
                                    }
                                }
                            }
                        }
                    }
                }
                Session["count"] = count;

                if (Session["totalamountdueseatfeeall"] != null)
                {
                    lblTotalAmount.Text = (Convert.ToDecimal(Session["totalamountdueseatfeeall"]) - ((Session["Balance"] != null) ? Convert.ToDecimal(Session["Balance"]) : 0)).ToString("N", nfi);
                    if (Convert.ToDecimal(Session["totalamountdueseatfeeall"]) < 0)
                    {
                        hfConfirm.Value = "1";
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

        private bool AssignSeat(List<ABS.Logic.GroupBooking.SeatInfo> _seatInfo, List<ABS.Logic.GroupBooking.SeatInfo> _usassignSeatInfo,
    BookingControl.TemFlight model, EnumFlightType pFlightType, Boolean bAssignSeat = false)
        {
            var profiler = MiniProfiler.Current;
            BookingControl bookingControl = new BookingControl();
            DataTable dtDetail = objBooking.dtTransDetail();
            string Signature;
            DataTable dtPassenger = default(DataTable);
            List<ABS.Logic.GroupBooking.SeatInfo> existingSeatInfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
            ABS.Logic.GroupBooking.SeatInfo seatInfo;
            //change to new add-On table, Tyas
            //dtPassenger = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTable(TransId, true);
            dtPassenger = objBooking.GetAllBK_PASSENGERLISTDataTableNew(TransID, true);

            decimal SeatFee = 0;

            if (Session["listBookingDetail"] == null)
            {
                listBookingDetail = objBooking.GetAllBK_TRANSDTLFilterAll(TransID, 0);
            }
            else
            {
                listBookingDetail = Session["listBookingDetail"] as List<BookingTransactionDetail>;
            }

            int countzeroseat = listBookingDetail.Count(item => Convert.ToDecimal(item.LineSeat) == 0);
            if (countzeroseat > 0) Session["foundzero"] = true;

            if (HttpContext.Current.Session["TransDetail"] != null)
            {
                dtDetail = (DataTable)HttpContext.Current.Session["TransDetail"];
            }

            if (dtDetail.Rows.Count != 0)
            {
                decimal totalamountdueseatfeeallcommitperPNR = 0;
                for (int loopAssign = 0; loopAssign <= 1; loopAssign++)
                {
                    #region "UnAssign SEat => Assign Seat"
                    for (int j = 0; j < dtDetail.Rows.Count; j++)
                    {
                        SeatFee = 0;
                        //if (dtDetail.Rows[j]["Origin"].ToString() == dtDetail.Rows[j]["Origin"].ToString())
                        //{
                        //    totalamountdueseatfeeallcommitperPNR = 0;
                        //}
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
                                BookingUpdateResponseData UnassignResponse = new BookingUpdateResponseData();
                                BookingUpdateResponseData AssignResponse = new BookingUpdateResponseData();
                                totalamountdueseatfeeallcommitperPNR = 0;

                                //Assign New Seat
                                if (_seatInfo == null)
                                {
                                    continue;
                                }

                                int selectedSeatRow = _seatInfo.Count(a => a.SelectedSeat.Trim() != "" && a.RecordLocator.Trim() == BookingDetail.RecordLocator.Trim());

                                if (selectedSeatRow <= 0)
                                {
                                    continue;
                                }

                                int SeatInfoCount1 = selectedSeatRow - 1;
                                int i = 0;
                                int[] aPassengerNumber = new int[SeatInfoCount1 + 1];
                                int[] aPassengerID = new int[SeatInfoCount1 + 1];
                                string[] aUnitDisignator = new string[SeatInfoCount1 + 1];
                                string[] acompartmentDesignator = new string[SeatInfoCount1 + 1];
                                string[] aPNR = new string[SeatInfoCount1 + 1];

                                ABS.Logic.GroupBooking.SeatInfo seatInfoWithAmount = new ABS.Logic.GroupBooking.SeatInfo();
                                foreach (ABS.Logic.GroupBooking.SeatInfo rSeatInfo in _seatInfo)
                                {

                                    if (BookingDetail.RecordLocator.Trim() == rSeatInfo.RecordLocator.Trim() && rSeatInfo.SelectedSeat.Trim() != "")
                                    {
                                        aPassengerNumber[i] = rSeatInfo.PassengerNumber;
                                        aPassengerID[i] = Convert.ToInt32(rSeatInfo.PassengerID);
                                        aUnitDisignator[i] = rSeatInfo.SelectedSeat;
                                        acompartmentDesignator[i] = rSeatInfo.CompartmentDesignator;
                                        i += 1;
                                    }

                                }
                                //End Assign New Seat

                                //Unassign Seat
                                int k = 0;
                                int[] aunassignPassengerNumber = null;
                                int[] aunassignPassengerID = null;
                                string[] aunassignUnitDisignator = null;
                                string[] aunassigncompartmentDesignator = null;
                                string[] aunassignPNR = null;
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
                                            k += 1;
                                        }
                                    }
                                }
                                //End Unassign Seat


                                string STD = "";

                                switch (pFlightType)
                                {
                                    #region "Depart Flight"
                                    case EnumFlightType.DepartFlight:
                                        STD = bookingResp.Journeys[0].Segments[0].STD.ToString();
                                        //added by ketee, check if assign seat = true
                                        if (bAssignSeat)
                                        {
                                            AssignResponse = absNavitaire.AssignSeats(false, Signature, bookingResp.Journeys[0].Segments[0].DepartureStation,
                                                                                    bookingResp.Journeys[0].Segments[0].ArrivalStation, bookingResp.Journeys[0].Segments[0].STD.ToString(),
                                                                                    aPassengerNumber, aPassengerID, aUnitDisignator, acompartmentDesignator, bookingResp.Journeys[0].Segments[0].FlightDesignator.CarrierCode,
                                                                                    bookingResp.Journeys[0].Segments[0].FlightDesignator.FlightNumber, bookingResp.Journeys[0].Segments[0].FlightDesignator.OpSuffix, bookingResp.RecordLocator, "manage");

                                            if (AssignResponse.Success != null)
                                            {
                                                ABS.Navitaire.BookingManager.GetBookingResponse booking = new GetBookingResponse();// absNavitaire.GetBookingByPNR(bookingResp.RecordLocator, Signature);
                                                using (profiler.Step("Navitaire:GetBooking"))
                                                {
                                                    booking = absNavitaire.GetBookingByPNR(bookingResp.RecordLocator, Signature);
                                                }
                                                //string respxml = absNavitaire.GetXMLString(booking);

                                                var route = bookingResp.Journeys[0].Segments[0].DepartureStation + bookingResp.Journeys[0].Segments[0].ArrivalStation;
                                                decimal totalSeatFees = 0;
                                                if (booking != null)
                                                {
                                                    //var xml = bookingControl.SaveBookingStateXML(Signature, Request.PhysicalApplicationPath);
                                                    if (booking.Booking.Passengers.Length > 0)
                                                    {
                                                        foreach (Passenger p in booking.Booking.Passengers)
                                                        {
                                                            if (p.PassengerFees.Length > 0)
                                                            {
                                                                foreach (PassengerFee fee in p.PassengerFees)
                                                                {
                                                                    if (fee.FeeType == FeeType.SeatFee && fee.FlightReference.Contains(route))
                                                                    {
                                                                        if (fee.ServiceCharges.Length > 0)
                                                                        {
                                                                            foreach (BookingServiceCharge charge in fee.ServiceCharges)
                                                                            {
                                                                                if (charge.ChargeType == ChargeType.Discount)
                                                                                    totalSeatFees -= charge.Amount;
                                                                                else if (charge.ChargeType == ChargeType.PromotionDiscount)
                                                                                    totalSeatFees -= charge.Amount;
                                                                                else
                                                                                    totalSeatFees += charge.Amount;
                                                                            }

                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    if (booking.Booking.Journeys.Length > 0 && booking.Booking.Journeys[0].Segments.Length > 0 && booking.Booking.Journeys[0].Segments[0].PaxSeats.Length > 0)
                                                    {
                                                        foreach (PaxSeat pax in booking.Booking.Journeys[0].Segments[0].PaxSeats)
                                                        {
                                                            seatInfo = new ABS.Logic.GroupBooking.SeatInfo();
                                                            //seatInfo.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                                            seatInfo.PassengerID = pax.PassengerNumber.ToString();
                                                            seatInfo.SelectedSeat = pax.UnitDesignator;
                                                            seatInfo.CompartmentDesignator = pax.CompartmentDesignator;
                                                            seatInfo.Deck = pax.PaxSeatInfo.Deck.ToString();
                                                            seatInfo.RecordLocator = BookingDetail.RecordLocator.ToString();
                                                            existingSeatInfo.Add(seatInfo);
                                                        }
                                                    }
                                                }
                                                if (totalSeatFees >= 0)
                                                {
                                                    decimal departFee = Convert.ToDecimal(Session["DepartFlightSeatFees"].ToString());
                                                    totalSeatFees = departFee + totalSeatFees;
                                                    Session["DepartExistingSeatInfo"] = existingSeatInfo;
                                                    Session["DepartFlightSeatFees"] = totalSeatFees;

                                                    //tyas
                                                    //int iIndexDepart = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 1 && p.Signature == Signature);
                                                    //if (iIndexDepart >= 0) listBookingDetail[iIndexDepart].LineSeat = totalSeatFees;
                                                    int iIndexDepart = listBookingDetail.FindIndex(p => p.Origin == bookingResp.Journeys[0].Segments[0].DepartureStation && p.RecordLocator.Trim() == BookingDetail.RecordLocator.Trim());
                                                    if (iIndexDepart >= 0)
                                                    {
                                                        decimal amountdue = AssignResponse.Success.PNRAmount.BalanceDue - ((Session["BalanceDue"] != null) ? Convert.ToDecimal(Session["BalanceDue"]) : 0);
                                                        Session["BalanceDue"] = null;
                                                        if (HttpContext.Current.Session["Forfeit"] != null && Convert.ToBoolean(Session["Forfeit"]) == true)
                                                        {
                                                            amountdue = 0;
                                                        }
                                                        ; if (HttpContext.Current.Session["Commit"] != null)
                                                        {
                                                            if (Convert.ToBoolean(Session["Commit"]) == true)
                                                            {
                                                                totalseatfeeallcommit += totalSeatFees;
                                                                totalseatfeeallcommitgoing += totalSeatFees;
                                                                totalamountdueseatfeeallcommitgoing += amountdue;
                                                                totalamountdueseatfeeallcommit += amountdue;
                                                                totalamountdueseatfeeallcommitperPNR += amountdue;
                                                            }

                                                        }


                                                        //listBookingDetail[iIndexDepart].LineSeat = totalSeatFees;
                                                        listBookingDetail[iIndexDepart].TransID = BookingDetail.TransID;
                                                        listBookingDetail[iIndexDepart].RecordLocator = BookingDetail.RecordLocator;
                                                        listBookingDetail[iIndexDepart].Signature = Signature;
                                                    }


                                                }
                                            }
                                            else
                                            {

                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            if (_usassignSeatInfo != null && _usassignSeatInfo.Count > 0)
                                            {
                                                UnassignResponse = absNavitaire.AssignSeats(true, Signature, bookingResp.Journeys[0].Segments[0].DepartureStation, bookingResp.Journeys[0].Segments[0].ArrivalStation, bookingResp.Journeys[0].Segments[0].STD.ToString(),
                                                    aunassignPassengerNumber, aunassignPassengerID, aunassignUnitDisignator, aunassigncompartmentDesignator,
                                                    bookingResp.Journeys[0].Segments[0].FlightDesignator.CarrierCode, bookingResp.Journeys[0].Segments[0].FlightDesignator.FlightNumber, bookingResp.Journeys[0].Segments[0].FlightDesignator.OpSuffix, bookingResp.RecordLocator, "manage");
                                                if (UnassignResponse.Success != null)
                                                {
                                                    //if (UnassignResponse.Success.PNRAmount.BalanceDue != 0)
                                                    //{
                                                    //    //tyas
                                                    //    decimal amountdue = UnassignResponse.Success.PNRAmount.BalanceDue;
                                                    //    totalamountdueseatfeeallcommitgoing += amountdue;
                                                    //    totalamountdueseatfeeallcommit += amountdue;
                                                    //    totalamountdueseatfeeallcommitperPNR += amountdue;
                                                    //}
                                                    Session["DepartExistingSeatInfo"] = null;
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
                                        if (bAssignSeat)
                                        {
                                            AssignResponse = absNavitaire.AssignSeats(false, Signature, bookingResp.Journeys[1].Segments[0].DepartureStation,
                                            bookingResp.Journeys[1].Segments[0].ArrivalStation, STD, aPassengerNumber, aPassengerID, aUnitDisignator,
                                            acompartmentDesignator, bookingResp.Journeys[1].Segments[0].FlightDesignator.CarrierCode, bookingResp.Journeys[1].Segments[0].FlightDesignator.FlightNumber,
                                            bookingResp.Journeys[1].Segments[0].FlightDesignator.OpSuffix, bookingResp.RecordLocator, "manage");

                                            if (AssignResponse.Success != null)
                                            {
                                                ABS.Navitaire.BookingManager.Booking booking2 = new ABS.Navitaire.BookingManager.Booking();// absNavitaire.GetBookingFromState(Signature, 3);
                                                using (profiler.Step("Navitaire:GetBookingFromState"))
                                                {
                                                    booking2 = absNavitaire.GetBookingFromState(Signature, 3);
                                                }
                                                //string respxml = absNavitaire.GetXMLString(booking2);

                                                var route2 = bookingResp.Journeys[1].Segments[0].DepartureStation + bookingResp.Journeys[1].Segments[0].ArrivalStation;
                                                decimal totalSeatFees2 = 0;
                                                if (booking2 != null)
                                                {
                                                    if (booking2.Passengers.Length > 0)
                                                    {
                                                        foreach (Passenger p in booking2.Passengers)
                                                        {
                                                            if (p.PassengerFees.Length > 0)
                                                            {
                                                                foreach (PassengerFee fee in p.PassengerFees)
                                                                {
                                                                    if (fee.FeeType == FeeType.SeatFee && fee.FlightReference.Contains(route2))
                                                                    {
                                                                        if (fee.ServiceCharges.Length > 0)
                                                                        {
                                                                            foreach (BookingServiceCharge charge in fee.ServiceCharges)
                                                                            {
                                                                                if (charge.ChargeType == ChargeType.Discount)
                                                                                    totalSeatFees2 -= charge.Amount;
                                                                                else if (charge.ChargeType == ChargeType.PromotionDiscount)
                                                                                    totalSeatFees2 -= charge.Amount;
                                                                                else
                                                                                    totalSeatFees2 += charge.Amount;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    if (booking2.Journeys.Length > 1 && booking2.Journeys[1].Segments.Length > 0 && booking2.Journeys[1].Segments[0].PaxSeats.Length > 0)
                                                    {
                                                        foreach (PaxSeat pax in booking2.Journeys[1].Segments[0].PaxSeats)
                                                        {
                                                            seatInfo = new ABS.Logic.GroupBooking.SeatInfo();
                                                            //seatInfo.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                                            seatInfo.PassengerID = pax.PassengerNumber.ToString();
                                                            seatInfo.SelectedSeat = pax.UnitDesignator;
                                                            seatInfo.CompartmentDesignator = pax.CompartmentDesignator;
                                                            seatInfo.Deck = pax.PaxSeatInfo.Deck.ToString();
                                                            seatInfo.RecordLocator = BookingDetail.RecordLocator.ToString();
                                                            existingSeatInfo.Add(seatInfo);
                                                        }
                                                    }
                                                }
                                                if (totalSeatFees2 >= 0)
                                                {
                                                    totalSeatFees2 = Convert.ToDecimal(Session["ReturnFlightSeatFees"].ToString()) + totalSeatFees2;
                                                    Session["ReturnExistingSeatInfo"] = existingSeatInfo;
                                                    Session["ReturnFlightSeatFees"] = totalSeatFees2;

                                                    //tyas
                                                    //int iIndexReturn = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 0 && p.Signature == Signature);
                                                    //if (iIndexReturn >= 0) listBookingDetail[iIndexReturn].LineSeat = totalSeatFees2;
                                                    int iIndexReturn = listBookingDetail.FindIndex(p => p.Origin == bookingResp.Journeys[1].Segments[0].DepartureStation && p.RecordLocator.Trim() == BookingDetail.RecordLocator.Trim());
                                                    if (iIndexReturn >= 0)
                                                    {
                                                        decimal amountdue = AssignResponse.Success.PNRAmount.BalanceDue - ((Session["BalanceDue"] != null) ? Convert.ToDecimal(Session["BalanceDue"]) : 0);
                                                        Session["BalanceDue"] = null;
                                                        if (HttpContext.Current.Session["Forfeit"] != null && Convert.ToBoolean(Session["Forfeit"]) == true)
                                                        {
                                                            amountdue = 0;
                                                        }
                                                        if (HttpContext.Current.Session["Commit"] != null)
                                                        {
                                                            if (Convert.ToBoolean(Session["Commit"]) == true)
                                                            {
                                                                totalseatfeeallcommit += totalSeatFees2;
                                                                totalseatfeeallcommitreturn += totalSeatFees2;
                                                                totalamountdueseatfeeallcommitreturn += amountdue;
                                                                totalamountdueseatfeeallcommit += amountdue;
                                                                totalamountdueseatfeeallcommitperPNR += amountdue;
                                                            }

                                                        }
                                                        //listBookingDetail[iIndexReturn].LineSeat = totalSeatFees2;
                                                        listBookingDetail[iIndexReturn].TransID = BookingDetail.TransID;
                                                        listBookingDetail[iIndexReturn].RecordLocator = BookingDetail.RecordLocator;
                                                        listBookingDetail[iIndexReturn].Signature = Signature;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            if (_usassignSeatInfo != null && _usassignSeatInfo.Count > 0)
                                            {
                                                UnassignResponse = absNavitaire.AssignSeats(true, Signature, bookingResp.Journeys[1].Segments[0].DepartureStation, bookingResp.Journeys[1].Segments[0].ArrivalStation, STD,
                                                    aunassignPassengerNumber, aunassignPassengerID, aunassignUnitDisignator, aunassigncompartmentDesignator,
                                                    bookingResp.Journeys[1].Segments[0].FlightDesignator.CarrierCode, bookingResp.Journeys[1].Segments[0].FlightDesignator.FlightNumber, bookingResp.Journeys[1].Segments[0].FlightDesignator.OpSuffix, bookingResp.RecordLocator, "manage");
                                                if (UnassignResponse.Success != null)
                                                {
                                                    //if (UnassignResponse.Success.PNRAmount.BalanceDue != 0)
                                                    //{
                                                    //    //tyas
                                                    //    decimal amountdue = UnassignResponse.Success.PNRAmount.BalanceDue;
                                                    //    totalamountdueseatfeeallcommitreturn += amountdue;
                                                    //    totalamountdueseatfeeallcommit += amountdue;
                                                    //    totalamountdueseatfeeallcommitperPNR += amountdue;
                                                    //}
                                                    Session["ReturnExistingSeatInfo"] = null;
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
                                        if (bAssignSeat)
                                        {

                                            AssignResponse = absNavitaire.AssignSeats(false, Signature, bookingResp.Journeys[0].Segments[0].DepartureStation, bookingResp.Journeys[0].Segments[0].ArrivalStation, STD, aPassengerNumber, aPassengerID, aUnitDisignator, acompartmentDesignator, bookingResp.Journeys[0].Segments[0].FlightDesignator.CarrierCode, bookingResp.Journeys[0].Segments[0].FlightDesignator.FlightNumber, bookingResp.Journeys[0].Segments[0].FlightDesignator.OpSuffix, bookingResp.RecordLocator, "manage");

                                            if (AssignResponse.Success != null)
                                            {

                                                ABS.Navitaire.BookingManager.Booking booking2 = new ABS.Navitaire.BookingManager.Booking();// absNavitaire.GetBookingFromState(Signature, 3);
                                                using (profiler.Step("Navitaire:GetBookingFromState"))
                                                {
                                                    booking2 = absNavitaire.GetBookingFromState(Signature, 3);
                                                }
                                                var route = bookingResp.Journeys[0].Segments[0].DepartureStation + bookingResp.Journeys[0].Segments[0].ArrivalStation;
                                                decimal totalSeatFees2 = 0;
                                                if (booking2 != null)
                                                {
                                                    if (booking2.Passengers.Length > 0)
                                                    {
                                                        foreach (Passenger p in booking2.Passengers)
                                                        {
                                                            if (p.PassengerFees.Length > 0)
                                                            {
                                                                foreach (PassengerFee fee in p.PassengerFees)
                                                                {
                                                                    if (fee.FeeType == FeeType.SeatFee && fee.FlightReference.Contains(route))
                                                                    {
                                                                        if (fee.ServiceCharges.Length > 0)
                                                                        {
                                                                            foreach (BookingServiceCharge charge in fee.ServiceCharges)
                                                                            {
                                                                                if (charge.ChargeType == ChargeType.Discount)
                                                                                    totalSeatFees2 -= charge.Amount;
                                                                                else if (charge.ChargeType == ChargeType.PromotionDiscount)
                                                                                    totalSeatFees2 -= charge.Amount;
                                                                                else
                                                                                    totalSeatFees2 += charge.Amount;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    if (booking2.Journeys.Length > 0 && booking2.Journeys[0].Segments.Length > 1 && booking2.Journeys[0].Segments[0].PaxSeats.Length > 0)
                                                    {
                                                        foreach (PaxSeat pax in booking2.Journeys[0].Segments[0].PaxSeats)
                                                        {
                                                            seatInfo = new ABS.Logic.GroupBooking.SeatInfo();
                                                            //seatInfo.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                                            seatInfo.PassengerID = pax.PassengerNumber.ToString();
                                                            seatInfo.SelectedSeat = pax.UnitDesignator;
                                                            seatInfo.CompartmentDesignator = pax.CompartmentDesignator;
                                                            seatInfo.Deck = pax.PaxSeatInfo.Deck.ToString();
                                                            seatInfo.RecordLocator = BookingDetail.RecordLocator.ToString();
                                                            existingSeatInfo.Add(seatInfo);
                                                        }
                                                    }

                                                }
                                                if (totalSeatFees2 >= 0)
                                                {
                                                    totalSeatFees2 = Convert.ToDecimal(Session["DepartConnectingFlightSeatFees"].ToString()) + totalSeatFees2;
                                                    Session["DepartConnectingExistingSeatInfo"] = existingSeatInfo;
                                                    SeatFee = totalSeatFees2;

                                                    //tyas
                                                    //int iIndexDepart = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 1 && p.Signature == Signature);
                                                    //if (iIndexDepart >= 0) listBookingDetail[iIndexDepart].LineSeat = totalSeatFees2;
                                                    //enhance by ketee, add seat fees to specific connecting flight field, 20170120
                                                    //if (iIndexDepart >= 0) listBookingDetail[iIndexDepart].LineConnectingSeat = totalSeatFees2;
                                                    int iIndexDepart = listBookingDetail.FindIndex(p => p.Origin == bookingResp.Journeys[0].Segments[0].DepartureStation && p.RecordLocator.Trim() == BookingDetail.RecordLocator.Trim());
                                                    if (iIndexDepart >= 0)
                                                    {
                                                        decimal amountdue = AssignResponse.Success.PNRAmount.BalanceDue - ((Session["BalanceDue"] != null) ? Convert.ToDecimal(Session["BalanceDue"]) : 0);
                                                        Session["BalanceDue"] = null;
                                                        if (HttpContext.Current.Session["Forfeit"] != null && Convert.ToBoolean(Session["Forfeit"]) == true)
                                                        {
                                                            amountdue = 0;
                                                        }
                                                        if (HttpContext.Current.Session["Commit"] != null)
                                                        {
                                                            if (Convert.ToBoolean(Session["Commit"]) == true)
                                                            {
                                                                totalseatfeeallcommit += totalSeatFees2;
                                                                totalseatfeeallcommitgoing += totalSeatFees2;
                                                                totalamountdueseatfeeallcommitgoing += amountdue;
                                                                totalamountdueseatfeeallcommit += amountdue;
                                                                totalamountdueseatfeeallcommitperPNR = amountdue;
                                                            }

                                                        }
                                                        //listBookingDetail[iIndexDepart].LineConnectingSeat = totalSeatFees2;
                                                        listBookingDetail[iIndexDepart].TransID = BookingDetail.TransID;
                                                        listBookingDetail[iIndexDepart].RecordLocator = BookingDetail.RecordLocator;
                                                        listBookingDetail[iIndexDepart].Signature = Signature;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            if (_usassignSeatInfo != null && _usassignSeatInfo.Count > 0)
                                            {
                                                UnassignResponse = absNavitaire.AssignSeats(true, Signature, bookingResp.Journeys[0].Segments[0].DepartureStation, bookingResp.Journeys[0].Segments[0].ArrivalStation, STD,
                                                    aunassignPassengerNumber, aunassignPassengerID, aunassignUnitDisignator, aunassigncompartmentDesignator,
                                                    bookingResp.Journeys[0].Segments[0].FlightDesignator.CarrierCode, bookingResp.Journeys[0].Segments[0].FlightDesignator.FlightNumber, bookingResp.Journeys[0].Segments[0].FlightDesignator.OpSuffix, bookingResp.RecordLocator, "manage");
                                                if (UnassignResponse.Success != null)
                                                {
                                                    //if (UnassignResponse.Success.PNRAmount.BalanceDue != 0)
                                                    //{
                                                    //    //tyas
                                                    //    decimal amountdue = UnassignResponse.Success.PNRAmount.BalanceDue;
                                                    //    totalamountdueseatfeeallcommitgoing += amountdue;
                                                    //    totalamountdueseatfeeallcommit += amountdue;
                                                    //    totalamountdueseatfeeallcommitperPNR += amountdue;
                                                    //}
                                                    Session["DepartConnectingExistingSeatInfo"] = null;
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
                                        if (bAssignSeat)
                                        {

                                            AssignResponse = absNavitaire.AssignSeats(false, Signature, bookingResp.Journeys[0].Segments[1].DepartureStation, bookingResp.Journeys[0].Segments[1].ArrivalStation, STD, aPassengerNumber, aPassengerID, aUnitDisignator, acompartmentDesignator, bookingResp.Journeys[0].Segments[1].FlightDesignator.CarrierCode, bookingResp.Journeys[0].Segments[1].FlightDesignator.FlightNumber, bookingResp.Journeys[0].Segments[1].FlightDesignator.OpSuffix, bookingResp.RecordLocator, "manage");

                                            if (AssignResponse.Success != null)
                                            {

                                                ABS.Navitaire.BookingManager.Booking booking2 = new ABS.Navitaire.BookingManager.Booking();// absNavitaire.GetBookingFromState(Signature, 3);
                                                using (profiler.Step("Navitaire:GetBookingFromState"))
                                                {
                                                    booking2 = absNavitaire.GetBookingFromState(Signature, 3);
                                                }
                                                var route = bookingResp.Journeys[0].Segments[1].DepartureStation + bookingResp.Journeys[0].Segments[1].ArrivalStation;
                                                decimal totalSeatFees2 = 0;
                                                if (booking2 != null)
                                                {
                                                    if (booking2.Passengers.Length > 0)
                                                    {
                                                        foreach (Passenger p in booking2.Passengers)
                                                        {
                                                            if (p.PassengerFees.Length > 0)
                                                            {
                                                                foreach (PassengerFee fee in p.PassengerFees)
                                                                {
                                                                    if (fee.FeeType == FeeType.SeatFee && fee.FlightReference.Contains(route))
                                                                    {
                                                                        if (fee.ServiceCharges.Length > 0)
                                                                        {
                                                                            foreach (BookingServiceCharge charge in fee.ServiceCharges)
                                                                            {
                                                                                if (charge.ChargeType == ChargeType.Discount)
                                                                                    totalSeatFees2 -= charge.Amount;
                                                                                else if (charge.ChargeType == ChargeType.PromotionDiscount)
                                                                                    totalSeatFees2 -= charge.Amount;
                                                                                else
                                                                                    totalSeatFees2 += charge.Amount;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    if (booking2.Journeys.Length > 0 && booking2.Journeys[0].Segments.Length > 1 && booking2.Journeys[0].Segments[1].PaxSeats.Length > 0)
                                                    {
                                                        foreach (PaxSeat pax in booking2.Journeys[0].Segments[1].PaxSeats)
                                                        {
                                                            seatInfo = new ABS.Logic.GroupBooking.SeatInfo();
                                                            //seatInfo.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                                            seatInfo.PassengerID = pax.PassengerNumber.ToString();
                                                            seatInfo.SelectedSeat = pax.UnitDesignator;
                                                            seatInfo.CompartmentDesignator = pax.CompartmentDesignator;
                                                            seatInfo.Deck = pax.PaxSeatInfo.Deck.ToString();
                                                            seatInfo.RecordLocator = BookingDetail.RecordLocator.ToString();
                                                            existingSeatInfo.Add(seatInfo);
                                                        }
                                                    }
                                                }
                                                if (totalSeatFees2 >= 0)
                                                {
                                                    totalSeatFees2 = Convert.ToDecimal(Session["DepartConnectingFlightSeatFees2"].ToString()) + totalSeatFees2;
                                                    Session["DepartConnectingExistingSeatInfo2"] = existingSeatInfo;
                                                    Session["DepartConnectingFlightSeatFees2"] = totalSeatFees2;
                                                    SeatFee = totalSeatFees2;

                                                    //tyas
                                                    //int iIndexDepart = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 1 && p.Signature == Signature);
                                                    //if (iIndexDepart >= 0) listBookingDetail[iIndexDepart].LineSeat = totalSeatFees2;
                                                    //enhance by ketee, add seat fees to specific connecting flight field, 20170120
                                                    //if (iIndexDepart >= 0) listBookingDetail[iIndexDepart].LineConnectingSeat2 = totalSeatFees2;
                                                    int iIndexDepart = listBookingDetail.FindIndex(p => p.Origin == bookingResp.Journeys[0].Segments[1].DepartureStation && p.RecordLocator.Trim() == BookingDetail.RecordLocator.Trim());
                                                    if (iIndexDepart >= 0)
                                                    {
                                                        decimal amountdue = AssignResponse.Success.PNRAmount.BalanceDue - ((Session["BalanceDue"] != null) ? Convert.ToDecimal(Session["BalanceDue"]) : 0);
                                                        Session["BalanceDue"] = null;
                                                        if (HttpContext.Current.Session["Forfeit"] != null && Convert.ToBoolean(Session["Forfeit"]) == true)
                                                        {
                                                            amountdue = 0;
                                                        }
                                                        if (HttpContext.Current.Session["Commit"] != null)
                                                        {
                                                            if (Convert.ToBoolean(Session["Commit"]) == true)
                                                            {
                                                                totalseatfeeallcommit += totalSeatFees2;
                                                                totalseatfeeallcommitgoing += totalSeatFees2;
                                                                totalamountdueseatfeeallcommitgoing += amountdue;
                                                                totalamountdueseatfeeallcommit += amountdue;
                                                                totalamountdueseatfeeallcommitperPNR += amountdue;
                                                            }

                                                        }
                                                        //listBookingDetail[iIndexDepart].LineConnectingSeat2 = totalSeatFees2;
                                                        listBookingDetail[iIndexDepart].TransID = BookingDetail.TransID;
                                                        listBookingDetail[iIndexDepart].RecordLocator = BookingDetail.RecordLocator;
                                                        listBookingDetail[iIndexDepart].Signature = Signature;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            if (_usassignSeatInfo != null && _usassignSeatInfo.Count > 0)
                                            {
                                                UnassignResponse = absNavitaire.AssignSeats(true, Signature, bookingResp.Journeys[0].Segments[1].DepartureStation, bookingResp.Journeys[0].Segments[1].ArrivalStation, STD,
                                                    aunassignPassengerNumber, aunassignPassengerID, aunassignUnitDisignator, aunassigncompartmentDesignator,
                                                    bookingResp.Journeys[0].Segments[1].FlightDesignator.CarrierCode, bookingResp.Journeys[0].Segments[1].FlightDesignator.FlightNumber, bookingResp.Journeys[0].Segments[1].FlightDesignator.OpSuffix, bookingResp.RecordLocator, "manage");
                                                if (UnassignResponse.Success != null)
                                                {
                                                    //if (UnassignResponse.Success.PNRAmount.BalanceDue != 0)
                                                    //{
                                                    //    //tyas
                                                    //    decimal amountdue = UnassignResponse.Success.PNRAmount.BalanceDue;
                                                    //    totalamountdueseatfeeallcommitgoing += amountdue;
                                                    //    totalamountdueseatfeeallcommit += amountdue;
                                                    //    totalamountdueseatfeeallcommitperPNR += amountdue;
                                                    //}
                                                    Session["DepartConnectingExistingSeatInfo2"] = null;
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
                                        if (bAssignSeat)
                                        {
                                            AssignResponse = absNavitaire.AssignSeats(false, Signature, bookingResp.Journeys[1].Segments[0].DepartureStation, bookingResp.Journeys[1].Segments[0].ArrivalStation, STD, aPassengerNumber, aPassengerID, aUnitDisignator, acompartmentDesignator, bookingResp.Journeys[1].Segments[0].FlightDesignator.CarrierCode, bookingResp.Journeys[1].Segments[0].FlightDesignator.FlightNumber, bookingResp.Journeys[1].Segments[0].FlightDesignator.OpSuffix, bookingResp.RecordLocator, "manage");

                                            if (AssignResponse.Success != null)
                                            {

                                                ABS.Navitaire.BookingManager.Booking booking2 = new ABS.Navitaire.BookingManager.Booking();// absNavitaire.GetBookingFromState(Signature, 3);
                                                using (profiler.Step("Navitaire:GetBookingFromState"))
                                                {
                                                    booking2 = absNavitaire.GetBookingFromState(Signature, 3);
                                                }
                                                var route2 = bookingResp.Journeys[1].Segments[0].DepartureStation + bookingResp.Journeys[1].Segments[0].ArrivalStation;
                                                decimal totalSeatFees2 = 0;
                                                if (booking2 != null)
                                                {
                                                    if (booking2.Passengers.Length > 0)
                                                    {
                                                        foreach (Passenger p in booking2.Passengers)
                                                        {
                                                            if (p.PassengerFees.Length > 0)
                                                            {
                                                                foreach (PassengerFee fee in p.PassengerFees)
                                                                {
                                                                    if (fee.FeeType == FeeType.SeatFee && fee.FlightReference.Contains(route2))
                                                                    {
                                                                        if (fee.ServiceCharges.Length > 0)
                                                                        {
                                                                            foreach (BookingServiceCharge charge in fee.ServiceCharges)
                                                                            {
                                                                                if (charge.ChargeType == ChargeType.Discount)
                                                                                    totalSeatFees2 -= charge.Amount;
                                                                                else if (charge.ChargeType == ChargeType.PromotionDiscount)
                                                                                    totalSeatFees2 -= charge.Amount;
                                                                                else
                                                                                    totalSeatFees2 += charge.Amount;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    if (booking2.Journeys.Length > 1 && booking2.Journeys[1].Segments.Length > 1 && booking2.Journeys[1].Segments[0].PaxSeats.Length > 0)
                                                    {
                                                        foreach (PaxSeat pax in booking2.Journeys[1].Segments[0].PaxSeats)
                                                        {
                                                            seatInfo = new ABS.Logic.GroupBooking.SeatInfo();
                                                            //seatInfo.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                                            seatInfo.PassengerID = pax.PassengerNumber.ToString();
                                                            seatInfo.SelectedSeat = pax.UnitDesignator;
                                                            seatInfo.CompartmentDesignator = pax.CompartmentDesignator;
                                                            seatInfo.Deck = pax.PaxSeatInfo.Deck.ToString();
                                                            seatInfo.RecordLocator = BookingDetail.RecordLocator.ToString();
                                                            existingSeatInfo.Add(seatInfo);
                                                        }
                                                    }
                                                }
                                                if (totalSeatFees2 >= 0)
                                                {
                                                    totalSeatFees2 = Convert.ToDecimal(Session["ReturnConnectingFlightSeatFees"].ToString()) + totalSeatFees2;
                                                    Session["ReturnConnectingExistingSeatInfo"] = existingSeatInfo;
                                                    Session["ReturnConnectingFlightSeatFees"] = totalSeatFees2;
                                                    SeatFee = totalSeatFees2;

                                                    //tyas
                                                    //int iIndexReturn = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 0 && p.Signature == Signature);
                                                    ////if (iIndexReturn >= 0) listBookingDetail[iIndexReturn].LineSeat = totalSeatFees2;
                                                    ////enhance by ketee, add seat fees to specific connecting flight field, 20170120
                                                    //if (iIndexReturn >= 0) listBookingDetail[iIndexReturn].LineConnectingSeat = totalSeatFees2;
                                                    int iIndexReturn = listBookingDetail.FindIndex(p => p.Origin == bookingResp.Journeys[1].Segments[0].DepartureStation && p.RecordLocator.Trim() == BookingDetail.RecordLocator.Trim());
                                                    if (iIndexReturn >= 0)
                                                    {
                                                        decimal amountdue = AssignResponse.Success.PNRAmount.BalanceDue - ((Session["BalanceDue"] != null) ? Convert.ToDecimal(Session["BalanceDue"]) : 0);
                                                        Session["BalanceDue"] = null;
                                                        if (HttpContext.Current.Session["Forfeit"] != null && Convert.ToBoolean(Session["Forfeit"]) == true)
                                                        {
                                                            amountdue = 0;
                                                        }
                                                        if (HttpContext.Current.Session["Commit"] != null)
                                                        {
                                                            if (Convert.ToBoolean(Session["Commit"]) == true)
                                                            {
                                                                totalseatfeeallcommit += totalSeatFees2;
                                                                totalseatfeeallcommitreturn += totalSeatFees2;
                                                                totalamountdueseatfeeallcommitreturn += amountdue;
                                                                totalamountdueseatfeeallcommit += amountdue;
                                                                totalamountdueseatfeeallcommitperPNR += amountdue;
                                                            }

                                                        }
                                                        //listBookingDetail[iIndexReturn].LineConnectingSeat = totalSeatFees2;
                                                        listBookingDetail[iIndexReturn].TransID = BookingDetail.TransID;
                                                        listBookingDetail[iIndexReturn].RecordLocator = BookingDetail.RecordLocator;
                                                        listBookingDetail[iIndexReturn].Signature = Signature;

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            if (_usassignSeatInfo != null && _usassignSeatInfo.Count > 0)
                                            {
                                                UnassignResponse = absNavitaire.AssignSeats(true, Signature, bookingResp.Journeys[1].Segments[0].DepartureStation, bookingResp.Journeys[1].Segments[0].ArrivalStation, STD,
                                                    aunassignPassengerNumber, aunassignPassengerID, aunassignUnitDisignator, aunassigncompartmentDesignator,
                                                    bookingResp.Journeys[1].Segments[0].FlightDesignator.CarrierCode, bookingResp.Journeys[1].Segments[0].FlightDesignator.FlightNumber, bookingResp.Journeys[1].Segments[0].FlightDesignator.OpSuffix, bookingResp.RecordLocator, "manage");
                                                if (UnassignResponse.Success != null)
                                                {
                                                    //if (UnassignResponse.Success.PNRAmount.BalanceDue != 0)
                                                    //{
                                                    //    //tyas
                                                    //    decimal amountdue = UnassignResponse.Success.PNRAmount.BalanceDue;
                                                    //    totalamountdueseatfeeallcommitreturn += amountdue;
                                                    //    totalamountdueseatfeeallcommit += amountdue;
                                                    //    totalamountdueseatfeeallcommitperPNR += amountdue;
                                                    //}
                                                    Session["ReturnConnectingExistingSeatInfo"] = null;
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
                                        if (bAssignSeat)
                                        {
                                            AssignResponse = absNavitaire.AssignSeats(false, Signature, bookingResp.Journeys[1].Segments[1].DepartureStation, bookingResp.Journeys[1].Segments[1].ArrivalStation, STD, aPassengerNumber, aPassengerID, aUnitDisignator, acompartmentDesignator, bookingResp.Journeys[1].Segments[1].FlightDesignator.CarrierCode, bookingResp.Journeys[1].Segments[1].FlightDesignator.FlightNumber, bookingResp.Journeys[1].Segments[1].FlightDesignator.OpSuffix, bookingResp.RecordLocator, "manage");

                                            if (AssignResponse.Success != null)
                                            {

                                                ABS.Navitaire.BookingManager.Booking booking2 = new ABS.Navitaire.BookingManager.Booking();// absNavitaire.GetBookingFromState(Signature, 3);
                                                using (profiler.Step("Navitaire:GetBookingFromState"))
                                                {
                                                    booking2 = absNavitaire.GetBookingFromState(Signature, 3);
                                                }
                                                var route2 = bookingResp.Journeys[1].Segments[1].DepartureStation + bookingResp.Journeys[1].Segments[1].ArrivalStation;
                                                decimal totalSeatFees2 = 0;
                                                if (booking2 != null)
                                                {
                                                    if (booking2.Passengers.Length > 0)
                                                    {
                                                        foreach (Passenger p in booking2.Passengers)
                                                        {
                                                            if (p.PassengerFees.Length > 0)
                                                            {
                                                                foreach (PassengerFee fee in p.PassengerFees)
                                                                {
                                                                    if (fee.FeeType == FeeType.SeatFee && fee.FlightReference.Contains(route2))
                                                                    {
                                                                        if (fee.ServiceCharges.Length > 0)
                                                                        {
                                                                            foreach (BookingServiceCharge charge in fee.ServiceCharges)
                                                                            {
                                                                                if (charge.ChargeType == ChargeType.Discount)
                                                                                    totalSeatFees2 -= charge.Amount;
                                                                                else if (charge.ChargeType == ChargeType.PromotionDiscount)
                                                                                    totalSeatFees2 -= charge.Amount;
                                                                                else
                                                                                    totalSeatFees2 += charge.Amount;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    if (booking2.Journeys.Length > 1 && booking2.Journeys[1].Segments.Length > 1 && booking2.Journeys[1].Segments[1].PaxSeats.Length > 0)
                                                    {
                                                        foreach (PaxSeat pax in booking2.Journeys[1].Segments[1].PaxSeats)
                                                        {
                                                            seatInfo = new ABS.Logic.GroupBooking.SeatInfo();
                                                            //seatInfo.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                                            seatInfo.PassengerID = pax.PassengerNumber.ToString();
                                                            seatInfo.SelectedSeat = pax.UnitDesignator;
                                                            seatInfo.CompartmentDesignator = pax.CompartmentDesignator;
                                                            seatInfo.Deck = pax.PaxSeatInfo.Deck.ToString();
                                                            seatInfo.RecordLocator = BookingDetail.RecordLocator.ToString();
                                                            existingSeatInfo.Add(seatInfo);
                                                        }
                                                    }
                                                }
                                                if (totalSeatFees2 >= 0)
                                                {
                                                    totalSeatFees2 = Convert.ToDecimal(Session["ReturnConnectingFlightSeatFees2"].ToString()) + totalSeatFees2;
                                                    Session["ReturnConnectingExistingSeatInfo2"] = existingSeatInfo;
                                                    Session["ReturnConnectingFlightSeatFees2"] = totalSeatFees2;
                                                    SeatFee = totalSeatFees2;

                                                    //tyas
                                                    //int iIndexReturn = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 0 && p.Signature == Signature);
                                                    ////if (iIndexReturn >= 0) listBookingDetail[iIndexReturn].LineSeat = totalSeatFees2;
                                                    ////enhance by ketee, add seat fees to specific connecting flight field, 20170120
                                                    //if (iIndexReturn >= 0) listBookingDetail[iIndexReturn].LineConnectingSeat2 = totalSeatFees2;
                                                    int iIndexReturn = listBookingDetail.FindIndex(p => p.Origin == bookingResp.Journeys[1].Segments[1].DepartureStation && p.RecordLocator.Trim() == BookingDetail.RecordLocator.Trim());
                                                    if (iIndexReturn >= 0)
                                                    {
                                                        decimal amountdue = AssignResponse.Success.PNRAmount.BalanceDue - ((Session["BalanceDue"] != null) ? Convert.ToDecimal(Session["BalanceDue"]) : 0);
                                                        Session["BalanceDue"] = null;
                                                        if (HttpContext.Current.Session["Forfeit"] != null && Convert.ToBoolean(Session["Forfeit"]) == true)
                                                        {
                                                            amountdue = 0;
                                                        }
                                                        if (HttpContext.Current.Session["Commit"] != null)
                                                        {
                                                            if (Convert.ToBoolean(Session["Commit"]) == true)
                                                            {
                                                                totalseatfeeallcommit += totalSeatFees2;
                                                                totalseatfeeallcommitreturn += totalSeatFees2;
                                                                totalamountdueseatfeeallcommitreturn += amountdue;
                                                                totalamountdueseatfeeallcommit += amountdue;
                                                                totalamountdueseatfeeallcommitperPNR += amountdue;
                                                            }

                                                        }
                                                        //listBookingDetail[iIndexReturn].LineConnectingSeat2 = totalSeatFees2;
                                                        listBookingDetail[iIndexReturn].TransID = BookingDetail.TransID;
                                                        listBookingDetail[iIndexReturn].RecordLocator = BookingDetail.RecordLocator;
                                                        listBookingDetail[iIndexReturn].Signature = Signature;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            if (_usassignSeatInfo != null && _usassignSeatInfo.Count > 0)
                                            {
                                                UnassignResponse = absNavitaire.AssignSeats(true, Signature, bookingResp.Journeys[1].Segments[1].DepartureStation, bookingResp.Journeys[1].Segments[1].ArrivalStation, STD,
                                                    aunassignPassengerNumber, aunassignPassengerID, aunassignUnitDisignator, aunassigncompartmentDesignator,
                                                    bookingResp.Journeys[1].Segments[1].FlightDesignator.CarrierCode, bookingResp.Journeys[1].Segments[1].FlightDesignator.FlightNumber, bookingResp.Journeys[1].Segments[1].FlightDesignator.OpSuffix, bookingResp.RecordLocator, "manage");
                                                if (UnassignResponse.Success != null)
                                                {
                                                    //if (UnassignResponse.Success.PNRAmount.BalanceDue != 0)
                                                    //{
                                                    //    //tyas
                                                    //    decimal amountdue = UnassignResponse.Success.PNRAmount.BalanceDue;
                                                    //    totalamountdueseatfeeallcommitreturn += amountdue;
                                                    //    totalamountdueseatfeeallcommit += amountdue;
                                                    //    totalamountdueseatfeeallcommitperPNR += amountdue;
                                                    //}

                                                    Session["ReturnConnectingExistingSeatInfo"] = null;
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

                                if (Session["listDetailCombinePNR"] != null)
                                {
                                    //listDetailCombinePNR = (List<BookingTransactionDetail>)Session["listDetailCombinePNR"];
                                    //foreach (BookingTransactionDetail bkDTL in listDetailCombinePNR)
                                    //{
                                    //    if (BookingDetail.RecordLocator == bkDTL.RecordLocator)
                                    //    {
                                    //        if (HttpContext.Current.Session["Commit"] != null)
                                    //        {
                                    //            if (Convert.ToBoolean(Session["Commit"]) == true)
                                    //            {
                                    //                bkDTL.LineSeat += totalamountdueseatfeeallcommitperPNR;
                                    //                bkDTL.LineTotal += totalamountdueseatfeeallcommitperPNR;
                                    //            }

                                    //        }

                                    //    }
                                    //}

                                    //objBooking.FillChgTransDetail(listDetailCombinePNR, (List<BookingTransactionDetail>)Session["listDetailCombinePNR"]);
                                    Session["totalseatfeeallcommit"] = totalseatfeeallcommit;
                                    Session["totalseatfeeallcommitgoing"] = totalseatfeeallcommitgoing;
                                    Session["totalseatfeeallcommitreturn"] = totalseatfeeallcommitreturn;
                                    Session["totalamountdueseatfeeallcommit"] = totalamountdueseatfeeallcommit;
                                    Session["totalamountdueseatfeeallcommitgoing"] = totalamountdueseatfeeallcommitgoing;
                                    Session["totalamountdueseatfeeallcommitreturn"] = totalamountdueseatfeeallcommitreturn;


                                }

                                objBK_TRANSDTL_Infos = new BookingTransactionDetail();
                                objBK_TRANSDTL_Infos.RecordLocator = BookingDetail.RecordLocator;
                                objBK_TRANSDTL_Infos.Signature = Signature;
                                objListBK_TRANSDTL_Infos.Add(objBK_TRANSDTL_Infos);
                                HttpContext.Current.Session["objListBK_TRANSDTL_Infos"] = objListBK_TRANSDTL_Infos;
                                if (bAssignSeat)
                                {

                                    if (!string.IsNullOrEmpty(TransID))
                                    {
                                        //update passenger seats
                                        List<PassengerData> lstPassenger = new List<PassengerData>();
                                        if (HttpContext.Current.Session["listPassengers"] != null)
                                        {
                                            lstPassenger = (List<PassengerData>)HttpContext.Current.Session["listPassengers"];
                                            foreach (PassengerData pax in lstPassenger)
                                            {
                                                foreach (ABS.Logic.GroupBooking.SeatInfo rSeatInfo in _seatInfo)
                                                {
                                                    if (pax.RecordLocator.Trim() == rSeatInfo.RecordLocator.Trim() &&
                                                        pax.PassengerID == rSeatInfo.PassengerID && rSeatInfo.SelectedSeat.Trim() != "")
                                                    {
                                                        switch (pFlightType)
                                                        {
                                                            case EnumFlightType.DepartFlight:
                                                                pax.DepartSeat = "0_" + rSeatInfo.CompartmentDesignator + "_" + rSeatInfo.Deck + "_" + rSeatInfo.SelectedSeat;
                                                                break;
                                                            case EnumFlightType.DepartConnectingFlight:
                                                                pax.DepartConnectingSeat = "0_" + rSeatInfo.CompartmentDesignator + "_" + rSeatInfo.Deck + "_" + rSeatInfo.SelectedSeat;
                                                                break;
                                                            case EnumFlightType.ReturnFlight:
                                                                pax.ReturnSeat = "0_" + rSeatInfo.CompartmentDesignator + "_" + rSeatInfo.Deck + "_" + rSeatInfo.SelectedSeat;
                                                                break;
                                                            case EnumFlightType.ReturnConnectingFlight:
                                                                pax.ReturnConnectingSeat = "0_" + rSeatInfo.CompartmentDesignator + "_" + rSeatInfo.Deck + "_" + rSeatInfo.SelectedSeat;
                                                                break;
                                                        }
                                                    }
                                                }
                                            }

                                            Session["listPassengers"] = lstPassenger;

                                        }
                                    }
                                }

                                ////begin, update dataclasstrans here
                                //dataClass.Rows[j]["SeatChrg"] = Convert.ToDecimal(dataClass.Rows[j]["SeatChrg"].ToString()) + SeatFee;
                                //dataClass.Rows[j]["FullPrice"] = Convert.ToDecimal(dataClass.Rows[j]["FullPrice"].ToString()) + SeatFee;
                                //HttpContext.Current.Session["dataClassTrans"] = dataClass;
                                ////end, update dataclasstrans here

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

                    bAssignSeat = true;
                }


                Session["TransDetail"] = dtDetail;
                Session["listBookingDetail"] = listBookingDetail;



                return true;
            }
            else
            {
                return false;
            }
        }

        protected string ValidateSeat()
        {
            MessageList msgList = new MessageList();
            bool assignSeatDone = false;
            DataTable dtDetail = new DataTable();
            try
            {
                dtPass = (DataTable)Session["dtPassengers"];
                Page.Validate("Mandatory");
                if (Page.IsValid)
                {
                    int count = 0;
                    if (Session["assignSeatinfoDepartConnectingFlight"] != null)
                    {

                        List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
                        assignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["assignSeatinfoDepartConnectingFlight"];

                        if (assignSeatinfo.Count > 0)
                        {
                            if (assignSeatinfo.Count == dtPass.Rows.Count) count += 1;
                            if (AssignSeat(assignSeatinfo, assignSeatinfo, model, EnumFlightType.DepartConnectingFlight))
                            {
                                Session["assignSeatinfoDepartConnectingFlight"] = null;
                            }
                            else if (Session["failedassign"] != null)
                            {
                                string ms = Session["failedassign"].ToString();
                                Session["failedassign"] = null;
                                ClearSession();
                                return ms;
                            }
                            else
                            {
                                ClearSession();
                                return msgList.Err999997;
                            }
                        }
                    }

                    if (Session["assignSeatinfoDepartConnectingFlight2"] != null)
                    {
                        List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
                        assignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["assignSeatinfoDepartConnectingFlight2"];
                        if (assignSeatinfo.Count > 0)
                        {
                            if (assignSeatinfo.Count == dtPass.Rows.Count) count += 1;
                            if (AssignSeat(assignSeatinfo, assignSeatinfo, model, EnumFlightType.DepartConnectingFlight2))
                            {
                                Session["assignSeatinfoDepartConnectingFlight2"] = null;
                            }
                            else if (Session["failedassign"] != null)
                            {
                                string ms = Session["failedassign"].ToString();
                                Session["failedassign"] = null;
                                ClearSession();
                                return ms;
                            }
                            else
                            {
                                ClearSession();
                                return msgList.Err999997;
                            }
                        }
                    }

                    if (Session["assignSeatinfoReturnConnectingFlight"] != null)
                    {

                        List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
                        assignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["assignSeatinfoReturnConnectingFlight"];
                        if (assignSeatinfo.Count > 0)
                        {
                            if (assignSeatinfo.Count == dtPass.Rows.Count) count += 1;
                            if (AssignSeat(assignSeatinfo, assignSeatinfo, model, EnumFlightType.ReturnConnectingFlight))
                            {
                                Session["assignSeatinfoReturnConnectingFlight"] = null;
                            }
                            else if (Session["failedassign"] != null)
                            {
                                string ms = Session["failedassign"].ToString();
                                Session["failedassign"] = null;
                                ClearSession();
                                return ms;
                            }
                            else
                            {
                                ClearSession();
                                return msgList.Err999997;
                            }
                        }
                    }

                    if (Session["assignSeatinfoReturnConnectingFlight2"] != null)
                    {

                        List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
                        assignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["assignSeatinfoReturnConnectingFlight2"];
                        if (assignSeatinfo.Count > 0)
                        {
                            if (assignSeatinfo.Count == dtPass.Rows.Count) count += 1;
                            if (AssignSeat(assignSeatinfo, assignSeatinfo, model, EnumFlightType.ReturnConnectingFlight2))
                            {
                                Session["assignSeatinfoReturnConnectingFlight2"] = null;
                            }
                            else if (Session["failedassign"] != null)
                            {
                                string ms = Session["failedassign"].ToString();
                                Session["failedassign"] = null;
                                ClearSession();
                                return ms;
                            }
                            else
                            {
                                ClearSession();
                                return msgList.Err999997;
                            }
                        }
                    }

                    if (Session["assignSeatinfoDepartFlight"] != null)
                    {

                        List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
                        assignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["assignSeatinfoDepartFlight"];
                        if (assignSeatinfo.Count > 0)
                        {
                            if (assignSeatinfo.Count == dtPass.Rows.Count) count += 1;
                            if (AssignSeat(assignSeatinfo, assignSeatinfo, model, EnumFlightType.DepartFlight))
                            {
                                Session["assignSeatinfoDepartFlight"] = null;
                            }
                            else if (Session["failedassign"] != null)
                            {
                                string ms = Session["failedassign"].ToString();
                                Session["failedassign"] = null;
                                ClearSession();
                                return ms;
                            }
                            else
                            {
                                ClearSession();
                                return msgList.Err999997;
                            }
                        }
                    }

                    if (Session["assignSeatinfoReturnFlight"] != null)
                    {

                        List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
                        assignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["assignSeatinfoReturnFlight"];
                        if (assignSeatinfo.Count > 0)
                        {
                            if (assignSeatinfo.Count == dtPass.Rows.Count) count += 1;
                            if (AssignSeat(assignSeatinfo, assignSeatinfo, model, EnumFlightType.ReturnFlight))
                            {
                                Session["assignSeatinfoReturnFlight"] = null;
                            }
                            else if (Session["failedassign"] != null)
                            {
                                string ms = Session["failedassign"].ToString();
                                Session["failedassign"] = null;
                                ClearSession();
                                return ms;
                            }
                            else
                            {
                                ClearSession();
                                return msgList.Err999997;
                            }
                        }
                    }

                    assignSeatDone = true;

                    if (assignSeatDone)
                    {
                        //begin, update total amount
                        if (!string.IsNullOrEmpty(TransID))
                        {
                            //tyas
                            listBookingDetail = Session["listBookingDetail"] as List<BookingTransactionDetail>;
                           
                            ABS.Navitaire.APIBooking apiBooking = new ABS.Navitaire.APIBooking("");

                            decimal totalSeatdepart = 0;
                            decimal totalSeatReturn = 0;
                            decimal totalAmountDueSeatdepart = 0;
                            decimal totalAmountDueSeatReturn = 0;
                            decimal totalSPLFee = 0;
                            string depart1 = "", depart2 = "", transit1 = "", transit2 = "", return1 = "", return2 = "";

                            for (int x = 0; x < listBookingDetail.Count; x++)
                            {
                                if (x == 0)
                                {
                                    depart1 = listBookingDetail[x].Origin.Trim();
                                    Session["depart1"] = listBookingDetail[x].Origin.Trim();
                                    transit1 = listBookingDetail[x].Transit.Trim();
                                    Session["transit1"] = listBookingDetail[x].Transit.Trim();
                                    return1 = listBookingDetail[x].Destination.Trim();
                                    Session["return1"] = listBookingDetail[x].Destination.Trim();
                                }
                                else if (x == 1 && listBookingDetail[x].Origin != listBookingDetail[0].Origin)
                                {
                                    depart2 = listBookingDetail[x].Origin.Trim();
                                    Session["depart2"] = listBookingDetail[x].Origin.Trim();
                                    transit2 = listBookingDetail[x].Transit.Trim();
                                    Session["transit2"] = listBookingDetail[x].Transit.Trim();
                                    return2 = listBookingDetail[x].Destination.Trim();
                                    Session["return2"] = listBookingDetail[x].Destination.Trim();
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (Session["listDetailCombinePNR"] != null)
                            {
                                listDetailCombinePNR = (List<BookingTransactionDetail>)Session["listDetailCombinePNR"];
                                foreach (BookingTransactionDetail bkDTL in listDetailCombinePNR)
                                {
                                    ABS.Navitaire.BookingManager.GetBookingResponse book = apiBooking.GetBookingByPNR(bkDTL.RecordLocator);
                                    decimal totSeatdepart = 0;
                                    decimal SPLFee = 0;
                                    decimal totSeatReturn = 0;
                                    decimal totalamountdueDepart = 0;
                                   
                                    decimal totalamountdueReturn = 0;

                                    if (book != null)
                                    {
                                        totSeatdepart = book.Booking.Passengers.Select(p => p.PassengerFees.Where(fee => fee.FeeType == FeeType.SeatFee && fee.FeeOverride != true && (fee.FlightReference.Substring(16, 3) == depart1 || fee.FlightReference.Substring(16, 6) == depart1 + transit1 || fee.FlightReference.Substring(16, 6) == transit1 + return1)).Select(charge => charge.ServiceCharges.Select(x => x.Amount).Sum()).Sum()).Sum();
                                        if (Session["SPLFee_" + bkDTL.RecordLocator] != null)
                                            SPLFee = Convert.ToDecimal(Session["SPLFee_" + bkDTL.RecordLocator]);
                                        totSeatReturn = book.Booking.Passengers.Select(p => p.PassengerFees.Where(fee => fee.FeeType == FeeType.SeatFee && fee.FeeOverride != true && (fee.FlightReference.Substring(16, 3) == depart2 || fee.FlightReference.Substring(16, 6) == depart2 + transit2 || fee.FlightReference.Substring(16, 6) == transit2 + return2)).Select(charge => charge.ServiceCharges.Select(x => x.Amount).Sum()).Sum()).Sum();
                                    }

                                    int iIndexDepart = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 1 && p.RecordLocator == bkDTL.RecordLocator);
                                    int iIndexReturn = listBookingDetail.FindIndex(p => p.SeqNo % 2 == 0 && p.RecordLocator == bkDTL.RecordLocator);
                                    if (iIndexDepart >= 0 && totSeatdepart > 0)
                                    {
                                        totalamountdueDepart = totSeatdepart - listBookingDetail[iIndexDepart].LineSeat;
                                        listBookingDetail[iIndexDepart].LineSeat += totalamountdueDepart;
                                        listBookingDetail[iIndexDepart].LineOth += SPLFee;
                                        listBookingDetail[iIndexDepart].LineTotal += totalamountdueDepart + SPLFee;
                                    }
                                    if (iIndexReturn >= 0 && totSeatReturn > 0)
                                    {
                                        totalamountdueReturn = totSeatReturn - listBookingDetail[iIndexReturn].LineSeat;
                                        listBookingDetail[iIndexReturn].LineSeat += totalamountdueReturn;
                                        listBookingDetail[iIndexReturn].LineTotal += totalamountdueReturn;
                                    }

                                    bkDTL.LineTotal = (from x in listBookingDetail where x.RecordLocator == bkDTL.RecordLocator select x.LineTotal).Sum();
                                    bkDTL.LineSeat = (from x in listBookingDetail where x.RecordLocator == bkDTL.RecordLocator select x.LineSeat).Sum();

                                    totalSeatdepart += totSeatdepart;
                                    totalSeatReturn += totSeatReturn;
                                    totalAmountDueSeatdepart += totalamountdueDepart;
                                    totalSPLFee += SPLFee;
                                    totalAmountDueSeatReturn += totalamountdueReturn;
                                    Session["SPLFee_" + bkDTL.RecordLocator] = null;
                                }
                            }

                            objBooking.FillChgTransDetail(listDetailCombinePNR, (List<BookingTransactionDetail>)Session["listDetailCombinePNR"]);

                            BookingTransactionMain bookingMain = new BookingTransactionMain();
                            bookingMain = objBooking.GetSingleBK_TRANSMAIN(TransID);

                            decimal totalamountdue = (totalAmountDueSeatdepart + totalAmountDueSeatReturn);
                            bookingMain.TransTotalSeat = bookingMain.TransTotalSeat + totalamountdue;
                            bookingMain.TotalAmtGoing = (bookingMain.TotalAmtGoing + totalAmountDueSeatdepart + totalSPLFee);
                            bookingMain.TotalAmtReturn = (bookingMain.TotalAmtReturn + totalAmountDueSeatReturn);
                            bookingMain.TransSubTotal = bookingMain.TotalAmtGoing + bookingMain.TotalAmtReturn;
                            bookingMain.TransTotalAmt = bookingMain.TotalAmtGoing + bookingMain.TotalAmtReturn;

                            objBooking.FillChgTransMain(bookingMain);
                            HttpContext.Current.Session.Remove("bookingMain");
                            HttpContext.Current.Session.Add("bookingMain", bookingMain);

                            objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                            if (Session["totalamountdueseatfeeallcommit"] != null && Convert.ToDecimal(Session["totalamountdueseatfeeallcommit"]) != 0)
                            {
                                if (Session["havebalance"] != null && Convert.ToBoolean(Session["havebalance"]) == true)
                                {
                                    int NextDue = 0;
                                    decimal amountdtl = 0;
                                    decimal amountmain = 0;
                                    if (bookingMain.PaymentAmtEx3 > 0)
                                    {
                                        NextDue = 3;
                                        bookingMain.PaymentAmtEx3 = bookingMain.PaymentAmtEx3 + (Convert.ToDecimal(Session["totalamountdueseatfeeall"]) - ((Session["Balance"] != null) ? Convert.ToDecimal(Session["Balance"]) : 0));
                                        amountmain = bookingMain.PaymentAmtEx3;
                                    }
                                    else if (bookingMain.PaymentAmtEx2 > 0)
                                    {
                                        NextDue = 2;
                                        bookingMain.PaymentAmtEx2 = bookingMain.PaymentAmtEx2 + (Convert.ToDecimal(Session["totalamountdueseatfeeall"]) - ((Session["Balance"] != null) ? Convert.ToDecimal(Session["Balance"]) : 0));
                                        amountmain = bookingMain.PaymentAmtEx2;
                                    }
                                    else
                                    {
                                        NextDue = 1;
                                        bookingMain.PaymentAmtEx1 = bookingMain.PaymentAmtEx1 + (Convert.ToDecimal(Session["totalamountdueseatfeeall"]) - ((Session["Balance"] != null) ? Convert.ToDecimal(Session["Balance"]) : 0));
                                        amountmain = bookingMain.PaymentAmtEx1;
                                    }

                                    if (listBookingDetail[0].PayDueAmount3 > 0)
                                    {
                                        listBookingDetail[0].PayDueAmount3 += (Convert.ToDecimal(Session["totalamountdueseatfeeall"]) - ((Session["Balance"] != null) ? Convert.ToDecimal(Session["Balance"]) : 0));
                                        amountdtl = listBookingDetail[0].PayDueAmount3;
                                    }
                                    else if (listBookingDetail[0].PayDueAmount2 > 0)
                                    {
                                        listBookingDetail[0].PayDueAmount2 += (Convert.ToDecimal(Session["totalamountdueseatfeeall"]) - ((Session["Balance"] != null) ? Convert.ToDecimal(Session["Balance"]) : 0));
                                        amountdtl = listBookingDetail[0].PayDueAmount2;
                                    }
                                    else
                                    {
                                        listBookingDetail[0].PayDueAmount1 += (Convert.ToDecimal(Session["totalamountdueseatfeeall"]) - ((Session["Balance"] != null) ? Convert.ToDecimal(Session["Balance"]) : 0));
                                        amountdtl = listBookingDetail[0].PayDueAmount1;
                                    }
                                    objBooking.SetNextTransAmount(bookingMain.TransID, listBookingDetail[0].RecordLocator, NextDue, amountdtl, amountmain);
                                    Response.RedirectLocation = "bookingdetail.aspx?k=" + Request.QueryString["k"] + "&TransID=" + Request.QueryString["TransID"];
                                }
                                else
                                {
                                    Session["ChgMode"] = "4"; //4= Manage Seats
                                    Session["UnassignSeats"] = null;
                                    ASPxCallback.RedirectOnCallback(GroupBooking.Web.Shared.MySite.PublicPages.Payment);
                                }
                            }
                            else
                            {
                                //if ((Convert.ToDecimal(Session["totalamountdueseatfeeall"]) - ((Session["Balance"] != null) ? Convert.ToDecimal(Session["Balance"]) : 0)) < 0)
                                //{
                                //    decimal ForfeitedAmount = (Convert.ToDecimal(Session["totalamountdueseatfeeall"]) - ((Session["Balance"] != null) ? Convert.ToDecimal(Session["Balance"]) : 0));
                                //    objBooking.SetForfeitedAmount(bookingMain.TransID, listBookingDetail[0].RecordLocator, ForfeitedAmount);

                                //}
                                //objBooking.SaveHeaderDetail(bookingMain, (List<BookingTransactionDetail>)Session["listBookingDetail"], CoreBase.EnumSaveType.Update);
                                Response.RedirectLocation = "bookingdetail.aspx?k=" + Request.QueryString["k"] + "&TransID=" + Request.QueryString["TransID"];

                            }
                            //Response.RedirectLocation = "bookingdetail.aspx?k=" + Request.QueryString["k"] + "&TransID=" + Request.QueryString["TransID"];
                        }
                    }


                    //added by ketee, for conencting flight, total up the seat fees from the connecting seat fees columns
                    //temp remarked


                    //added by ketee, commit seat change
                    //string errMsg = "";
                    //if (HttpContext.Current.Session["TransDetail"] != null)
                    //{
                    //    dtDetail = (DataTable)HttpContext.Current.Session["TransDetail"];
                    //    for (int j = 0; j < dtDetail.Rows.Count; j++)
                    //    {

                    //        ABS.Navitaire.BookingManager.Booking bookingResp = absNavitaire.GetBookingFromState(dtDetail.Rows[j]["SellKey"].ToString(), 2);
                    //        if (bookingResp != null && bookingResp.RecordLocator.Trim() == dtDetail.Rows[j]["RecordLocator"].ToString())
                    //        {
                    //            string xml = absNavitaire.GetXMLString(bookingResp);
                    //            if (absNavitaire.BookingCommitChange(dtDetail.Rows[j]["RecordLocator"].ToString(), dtDetail.Rows[j]["SellKey"].ToString(),
                    //                                                ref errMsg) == false)
                    //                return "Fail to change seat, please try again";
                    //        }
                    //        else
                    //            return "Transaction error, please contact system administrator, sorry for the inconvenience";
                    //    }

                    //}

                    //end, temp remarked
                }

                return "";
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                return null;
                throw ex;
            }
        }
        //            int SeatInfo0Checking = 0;
        //            int SeatInfo1Checking = 0;
        //            int SeatInfo2Checking = 0;
        //            int SeatInfo3Checking = 0;

        //            bool IsOneWay = false;
        //            Page.Validate("Mandatory");
        //            if (Page.IsValid)
        //                        {
        //                            if (Session["transitdepartreturn"] != null)
        //                            {
        //                                if (SeatInfo0Checking == 0 & SeatInfo1Checking == 0 & SeatInfo2Checking == 0 & SeatInfo3Checking == 0)
        //                                {

        //                                    if (SeatInfo0Checking == 0)
        //                                    {

        //                                        if (AssignSeat(assignSeatinfo, assignSeatinfo, model, EnumFlightType.DepartConnectingFlight))
        //                                        {
        //                                            DeleteXML((string)Session["SeatInfo0Xml"]);
        //                                            Session["DepartSeatInfo"] = Session["SeatInfo0"];
        //                                        }
        //                                        else
        //                                        {
        //                                            if (Session["minusamount"] != null)
        //                                            {
        //                                                Session["minusamount"] = null;
        //                                                ClearSession();
        //                                                return "Fail to assign change seat, seat amount cannot less than previous seat fees";

        //                                            }
        //                                            else if (Session["failedassign"] != null)
        //                                            {
        //                                                string ms = Session["failedassign"].ToString();
        //                                                Session["failedassign"] = null;
        //                                                ClearSession();
        //                                                return ms;
        //                                            }
        //                                            else
        //                                            {
        //                                                ClearSession();
        //                                                return msgList.Err999997;
        //                                            }
        //                                        }
        //                                    }
        //                                    if (SeatInfo1Checking == 0)
        //                                    {
        //                                        //List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = null;
        //                                        //if (Session["DepartConnectingExistingSeatInfo2"] != null)
        //                                        //{
        //                                        //    unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartConnectingExistingSeatInfo2"];
        //                                        //}

        //                                        List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
        //                                        List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
        //                                        SeatAvailabilityResponse pAvailableSeatInfo1 = (SeatAvailabilityResponse)Session["pAvailableSeatInfo1"];
        //                                        List<PassengerContainer> newseat = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.ConnectingDepart)) != item.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart))).ToList();
        //                                        if (newseat != null && newseat.Count > 0)
        //                                        {
        //                                            int num = 0;
        //                                            foreach (PassengerContainer pass in newseat)
        //                                            {
        //                                                if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
        //                                                {
        //                                                    List<PassengerContainer> found = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.ConnectingDepart)) == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart))).ToList();
        //                                                    if (found != null && found.Count > 0)
        //                                                    {
        //                                                        foreach (PassengerContainer passfound in found)
        //                                                        {
        //                                                            if (pass.RecordLocator != passfound.RecordLocator)
        //                                                            {
        //                                                                num = 1;
        //                                                                break;
        //                                                            }
        //                                                            else
        //                                                            {
        //                                                                ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                                                assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.ConnectingDepart));
        //                                                                assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart));
        //                                                                assign.RecordLocator = pass.RecordLocator;
        //                                                                assign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                                                assign.PassengerID = pass.PassengerID.ToString();
        //                                                                assign.PassengerNumber = (int)pass.PassengerNumber;
        //                                                                List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = pAvailableSeatInfo1.EquipmentInfos[0].Compartments[0].Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart))).ToList();
        //                                                                assign.SeatGroup = findgroup[0].SeatGroup.ToString();
        //                                                                List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo1.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
        //                                                                foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
        //                                                                {
        //                                                                    for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
        //                                                                    {
        //                                                                        assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
        //                                                                    }
        //                                                                    break;
        //                                                                }
        //                                                                assignSeatinfo.Add(assign);
        //                                                            }
        //                                                        }
        //                                                        if (num == 1) break;
        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                                        assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.ConnectingDepart));
        //                                                        assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart));
        //                                                        assign.RecordLocator = pass.RecordLocator;
        //                                                        assign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                                        assign.PassengerID = pass.PassengerID.ToString();
        //                                                        assign.PassengerNumber = (int)pass.PassengerNumber;
        //                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = pAvailableSeatInfo1.EquipmentInfos[0].Compartments[0].Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart))).ToList();
        //                                                        assign.SeatGroup = findgroup[0].SeatGroup.ToString();
        //                                                        List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo1.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
        //                                                        foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
        //                                                        {
        //                                                            for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
        //                                                            {
        //                                                                assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
        //                                                            }
        //                                                            break;
        //                                                        }
        //                                                        assignSeatinfo.Add(assign);
        //                                                    }
        //                                                }
        //                                            }

        //                                            //foreach (PassengerContainer pass in newseat)
        //                                            //{
        //                                            //    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
        //                                            //    {
        //                                            //        ABS.Logic.GroupBooking.SeatInfo unassign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                            //        unassign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                            //        unassign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                            //        unassign.RecordLocator = pass.RecordLocator;
        //                                            //        unassign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                            //        unassign.PassengerID = pass.PassengerID.ToString();
        //                                            //        unassign.PassengerNumber = (int)pass.PassengerNumber;
        //                                            //        unassignSeatinfo.Add(unassign);
        //                                            //    }
        //                                            //}
        //                                            //unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
        //                                            if (num == 1) return "Fail to assign change seat, cannot swap seat with different PNR";
        //                                        }

        //                                        if (AssignSeat(assignSeatinfo, assignSeatinfo, model, EnumFlightType.DepartConnectingFlight2))
        //                                        {
        //                                            DeleteXML((string)Session["SeatInfo1Xml"]);
        //                                            Session["DepartConnectingSeatInfo"] = Session["SeatInfo1"];
        //                                        }
        //                                        else
        //                                        {
        //                                            if (Session["minusamount"] != null)
        //                                            {
        //                                                Session["minusamount"] = null;
        //                                                ClearSession();
        //                                                return "Fail to assign change seat, seat amount cannot less than previous seat fees";
        //                                            }
        //                                            else if (Session["failedassign"] != null)
        //                                            {
        //                                                string ms = Session["failedassign"].ToString();
        //                                                Session["failedassign"] = null;
        //                                                ClearSession();
        //                                                return ms;
        //                                            }
        //                                            else
        //                                            {
        //                                                ClearSession();
        //                                                return msgList.Err999997;
        //                                            }
        //                                        }
        //                                    }
        //                                    if (SeatInfo2Checking == 0)
        //                                    {
        //                                        List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
        //                                        List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
        //                                        SeatAvailabilityResponse pAvailableSeatInfo2 = (SeatAvailabilityResponse)Session["pAvailableSeatInfo2"];
        //                                        List<PassengerContainer> newseat = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Return)) != item.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
        //                                        if (newseat != null && newseat.Count > 0)
        //                                        {
        //                                            int num = 0;
        //                                            foreach (PassengerContainer pass in newseat)
        //                                            {
        //                                                if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Return)) != "")
        //                                                {
        //                                                    List<PassengerContainer> found = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Return)) == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
        //                                                    if (found != null && found.Count > 0)
        //                                                    {
        //                                                        foreach (PassengerContainer passfound in found)
        //                                                        {
        //                                                            if (pass.RecordLocator != passfound.RecordLocator)
        //                                                            {
        //                                                                num = 1;
        //                                                                break;
        //                                                            }
        //                                                            else
        //                                                            {
        //                                                                ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                                                assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Return));
        //                                                                assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Return));
        //                                                                assign.RecordLocator = pass.RecordLocator;
        //                                                                assign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                                                assign.PassengerID = pass.PassengerID.ToString();
        //                                                                assign.PassengerNumber = (int)pass.PassengerNumber;
        //                                                                List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = pAvailableSeatInfo2.EquipmentInfos[0].Compartments[0].Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
        //                                                                assign.SeatGroup = findgroup[0].SeatGroup.ToString();
        //                                                                List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo2.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
        //                                                                foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
        //                                                                {
        //                                                                    for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
        //                                                                    {
        //                                                                        assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
        //                                                                    }
        //                                                                    break;
        //                                                                }
        //                                                                assignSeatinfo.Add(assign);
        //                                                            }
        //                                                        }
        //                                                        if (num == 1) break;
        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                                        assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Return));
        //                                                        assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Return));
        //                                                        assign.RecordLocator = pass.RecordLocator;
        //                                                        assign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                                        assign.PassengerID = pass.PassengerID.ToString();
        //                                                        assign.PassengerNumber = (int)pass.PassengerNumber;
        //                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = pAvailableSeatInfo2.EquipmentInfos[0].Compartments[0].Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
        //                                                        assign.SeatGroup = findgroup[0].SeatGroup.ToString();
        //                                                        List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo2.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
        //                                                        foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
        //                                                        {
        //                                                            for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
        //                                                            {
        //                                                                assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
        //                                                            }
        //                                                            break;
        //                                                        }
        //                                                        assignSeatinfo.Add(assign);
        //                                                    }
        //                                                }
        //                                            }

        //                                            //foreach (PassengerContainer pass in newseat)
        //                                            //{
        //                                            //    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
        //                                            //    {
        //                                            //        ABS.Logic.GroupBooking.SeatInfo unassign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                            //        unassign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                            //        unassign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                            //        unassign.RecordLocator = pass.RecordLocator;
        //                                            //        unassign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                            //        unassign.PassengerID = pass.PassengerID.ToString();
        //                                            //        unassign.PassengerNumber = (int)pass.PassengerNumber;
        //                                            //        unassignSeatinfo.Add(unassign);
        //                                            //    }
        //                                            //}
        //                                            //unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
        //                                            if (num == 1) return "Fail to assign change seat, cannot swap seat with different PNR";
        //                                        }
        //                                        //List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = null;
        //                                        //if (Session["ReturnConnectingExistingSeatInfo"] != null)
        //                                        //{
        //                                        //    unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnConnectingExistingSeatInfo"];
        //                                        //}

        //                                        if (AssignSeat(assignSeatinfo, assignSeatinfo, model, EnumFlightType.ReturnConnectingFlight))
        //                                        {
        //                                            DeleteXML((string)Session["SeatInfo2Xml"]);
        //                                            Session["ReturnSeatInfo"] = Session["SeatInfo2"];
        //                                        }
        //                                        else
        //                                        {
        //                                            if (Session["minusamount"] != null)
        //                                            {
        //                                                Session["minusamount"] = null;
        //                                                ClearSession();
        //                                                return "Fail to assign change seat, seat amount cannot less than previous seat fees";
        //                                            }
        //                                            else if (Session["failedassign"] != null)
        //                                            {
        //                                                string ms = Session["failedassign"].ToString();
        //                                                Session["failedassign"] = null;
        //                                                ClearSession();
        //                                                return ms;
        //                                            }
        //                                            else
        //                                            {
        //                                                ClearSession();
        //                                                return msgList.Err999997;
        //                                            }
        //                                        }
        //                                    }
        //                                    if (SeatInfo3Checking == 0)
        //                                    {
        //                                        List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
        //                                        List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
        //                                        SeatAvailabilityResponse pAvailableSeatInfo3 = (SeatAvailabilityResponse)Session["pAvailableSeatInfo3"];
        //                                        List<PassengerContainer> newseat = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.ConnectingReturn)) != item.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn))).ToList();
        //                                        if (newseat != null && newseat.Count > 0)
        //                                        {
        //                                            int num = 0;
        //                                            foreach (PassengerContainer pass in newseat)
        //                                            {
        //                                                if (pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn)) != "")
        //                                                {
        //                                                    List<PassengerContainer> found = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.ConnectingReturn)) == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn))).ToList();
        //                                                    if (found != null && found.Count > 0)
        //                                                    {
        //                                                        foreach (PassengerContainer passfound in found)
        //                                                        {
        //                                                            if (pass.RecordLocator != passfound.RecordLocator)
        //                                                            {
        //                                                                num = 1;
        //                                                                break;
        //                                                            }
        //                                                            else
        //                                                            {
        //                                                                ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                                                assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.ConnectingReturn));
        //                                                                assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn));
        //                                                                assign.RecordLocator = pass.RecordLocator;
        //                                                                assign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                                                assign.PassengerID = pass.PassengerID.ToString();
        //                                                                assign.PassengerNumber = (int)pass.PassengerNumber;
        //                                                                List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = pAvailableSeatInfo3.EquipmentInfos[0].Compartments[0].Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn))).ToList();
        //                                                                assign.SeatGroup = findgroup[0].SeatGroup.ToString();
        //                                                                List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo3.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
        //                                                                foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
        //                                                                {
        //                                                                    for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
        //                                                                    {
        //                                                                        assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
        //                                                                    }
        //                                                                    break;
        //                                                                }
        //                                                                assignSeatinfo.Add(assign);
        //                                                            }
        //                                                        }
        //                                                        if (num == 1) break;
        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                                        assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.ConnectingReturn));
        //                                                        assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn));
        //                                                        assign.RecordLocator = pass.RecordLocator;
        //                                                        assign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                                        assign.PassengerID = pass.PassengerID.ToString();
        //                                                        assign.PassengerNumber = (int)pass.PassengerNumber;
        //                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = pAvailableSeatInfo3.EquipmentInfos[0].Compartments[0].Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn))).ToList();
        //                                                        assign.SeatGroup = findgroup[0].SeatGroup.ToString();
        //                                                        List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo3.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
        //                                                        foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
        //                                                        {
        //                                                            for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
        //                                                            {
        //                                                                assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
        //                                                            }
        //                                                            break;
        //                                                        }
        //                                                        assignSeatinfo.Add(assign);
        //                                                    }
        //                                                }
        //                                            }

        //                                            //foreach (PassengerContainer pass in newseat)
        //                                            //{
        //                                            //    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
        //                                            //    {
        //                                            //        ABS.Logic.GroupBooking.SeatInfo unassign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                            //        unassign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                            //        unassign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                            //        unassign.RecordLocator = pass.RecordLocator;
        //                                            //        unassign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                            //        unassign.PassengerID = pass.PassengerID.ToString();
        //                                            //        unassign.PassengerNumber = (int)pass.PassengerNumber;
        //                                            //        unassignSeatinfo.Add(unassign);
        //                                            //    }
        //                                            //}
        //                                            //unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
        //                                            if (num == 1) return "Fail to assign change seat, cannot swap seat with different PNR";
        //                                        }
        //                                        //List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = null;
        //                                        //if (Session["ReturnConnectingExistingSeatInfo2"] != null)
        //                                        //{
        //                                        //    unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnConnectingExistingSeatInfo2"];
        //                                        //}

        //                                        if (AssignSeat(assignSeatinfo, assignSeatinfo, model, EnumFlightType.ReturnConnectingFlight2))
        //                                        {
        //                                            DeleteXML((string)Session["SeatInfo3Xml"]);
        //                                            Session["ReturnConnectingSeatInfo"] = Session["SeatInfo3"];
        //                                        }
        //                                        else
        //                                        {
        //                                            if (Session["minusamount"] != null)
        //                                            {
        //                                                Session["minusamount"] = null;
        //                                                ClearSession();
        //                                                return "Fail to assign change seat, seat amount cannot less than previous seat fees";
        //                                            }
        //                                            else if (Session["failedassign"] != null)
        //                                            {
        //                                                string ms = Session["failedassign"].ToString();
        //                                                Session["failedassign"] = null;
        //                                                ClearSession();
        //                                                return ms;
        //                                            }
        //                                            else
        //                                            {
        //                                                ClearSession();
        //                                                return msgList.Err999997;
        //                                            }
        //                                        }
        //                                    }

        //                                    //Response.Redirect("~/pages/SeatSummary.aspx");
        //                                }
        //                                else
        //                                {
        //                                    ClearSession();
        //                                    return "Please select seat(s) before proceed.";
        //                                    //lblErr.Text = "Please select seat(s) before proceed.";
        //                                    //pnlErr.Visible = true;
        //                                    //Session["ErrorMsg"] = lblErr.Text;
        //                                    //Response.Redirect("~/seats.aspx");
        //                                    //FillFlight(Session["akey"], 0)
        //                                }
        //                            }
        //                            else if (Session["transitreturn"] != null)
        //                            {
        //                                if (SeatInfo0Checking == 0 & SeatInfo2Checking == 0 & SeatInfo3Checking == 0)
        //                                {
        //                                    if (SeatInfo0Checking == 0)
        //                                    {
        //                                        List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
        //                                        List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
        //                                        SeatAvailabilityResponse pAvailableSeatInfo0 = (SeatAvailabilityResponse)Session["pAvailableSeatInfo0"];
        //                                        List<PassengerContainer> newseat = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Depart)) != item.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
        //                                        if (newseat != null && newseat.Count > 0)
        //                                        {
        //                                            int num = 0;
        //                                            foreach (PassengerContainer pass in newseat)
        //                                            {
        //                                                if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
        //                                                {
        //                                                    List<PassengerContainer> found = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Depart)) == pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
        //                                                    if (found != null && found.Count > 0)
        //                                                    {
        //                                                        foreach (PassengerContainer passfound in found)
        //                                                        {
        //                                                            if (pass.RecordLocator != passfound.RecordLocator)
        //                                                            {
        //                                                                num = 1;
        //                                                                break;
        //                                                            }
        //                                                            else
        //                                                            {
        //                                                                ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                                                assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                                                assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                                                assign.RecordLocator = pass.RecordLocator;
        //                                                                assign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                                                assign.PassengerID = pass.PassengerID.ToString();
        //                                                                assign.PassengerNumber = (int)pass.PassengerNumber;
        //                                                                List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = pAvailableSeatInfo0.EquipmentInfos[0].Compartments[0].Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
        //                                                                assign.SeatGroup = findgroup[0].SeatGroup.ToString();
        //                                                                List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo0.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
        //                                                                foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
        //                                                                {
        //                                                                    for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
        //                                                                    {
        //                                                                        assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
        //                                                                    }
        //                                                                    break;
        //                                                                }
        //                                                                assignSeatinfo.Add(assign);
        //                                                            }
        //                                                        }
        //                                                        if (num == 1) break;
        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                                        assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                                        assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                                        assign.RecordLocator = pass.RecordLocator;
        //                                                        assign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                                        assign.PassengerID = pass.PassengerID.ToString();
        //                                                        assign.PassengerNumber = (int)pass.PassengerNumber;
        //                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = pAvailableSeatInfo0.EquipmentInfos[0].Compartments[0].Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
        //                                                        assign.SeatGroup = findgroup[0].SeatGroup.ToString();
        //                                                        List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo0.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
        //                                                        foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
        //                                                        {
        //                                                            for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
        //                                                            {
        //                                                                assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
        //                                                            }
        //                                                            break;
        //                                                        }
        //                                                        assignSeatinfo.Add(assign);
        //                                                    }
        //                                                }
        //                                            }

        //                                            //foreach (PassengerContainer pass in newseat)
        //                                            //{
        //                                            //    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
        //                                            //    {
        //                                            //        ABS.Logic.GroupBooking.SeatInfo unassign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                            //        unassign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                            //        unassign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                            //        unassign.RecordLocator = pass.RecordLocator;
        //                                            //        unassign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                            //        unassign.PassengerID = pass.PassengerID.ToString();
        //                                            //        unassign.PassengerNumber = (int)pass.PassengerNumber;
        //                                            //        unassignSeatinfo.Add(unassign);
        //                                            //    }
        //                                            //}
        //                                            //unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
        //                                            if (num == 1) return "Fail to assign change seat, cannot swap seat with different PNR";
        //                                        }

        //                                        if (AssignSeat(assignSeatinfo, assignSeatinfo, model, EnumFlightType.DepartConnectingFlight))
        //                                        {
        //                                            DeleteXML((string)Session["SeatInfo0Xml"]);
        //                                            Session["DepartSeatInfo"] = Session["SeatInfo0"];
        //                                        }
        //                                        else
        //                                        {
        //                                            if (Session["minusamount"] != null)
        //                                            {
        //                                                Session["minusamount"] = null;
        //                                                ClearSession();
        //                                                return "Fail to assign change seat, seat amount cannot less than previous seat fees";
        //                                            }
        //                                            else if (Session["failedassign"] != null)
        //                                            {
        //                                                string ms = Session["failedassign"].ToString();
        //                                                Session["failedassign"] = null;
        //                                                ClearSession();
        //                                                return ms;
        //                                            }
        //                                            else
        //                                            {
        //                                                ClearSession();
        //                                                return msgList.Err999997;
        //                                            }
        //                                        }
        //                                    }

        //                                    if (SeatInfo2Checking == 0)
        //                                    {
        //                                        List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
        //                                        List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
        //                                        SeatAvailabilityResponse pAvailableSeatInfo2 = (SeatAvailabilityResponse)Session["pAvailableSeatInfo2"];
        //                                        List<PassengerContainer> newseat = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Return)) != item.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
        //                                        if (newseat != null && newseat.Count > 0)
        //                                        {
        //                                            int num = 0;
        //                                            foreach (PassengerContainer pass in newseat)
        //                                            {
        //                                                if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Return)) != "")
        //                                                {
        //                                                    List<PassengerContainer> found = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Return)) == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
        //                                                    if (found != null && found.Count > 0)
        //                                                    {
        //                                                        foreach (PassengerContainer passfound in found)
        //                                                        {
        //                                                            if (pass.RecordLocator != passfound.RecordLocator)
        //                                                            {
        //                                                                num = 1;
        //                                                                break;
        //                                                            }
        //                                                            else
        //                                                            {
        //                                                                ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                                                assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Return));
        //                                                                assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Return));
        //                                                                assign.RecordLocator = pass.RecordLocator;
        //                                                                assign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                                                assign.PassengerID = pass.PassengerID.ToString();
        //                                                                assign.PassengerNumber = (int)pass.PassengerNumber;
        //                                                                List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = pAvailableSeatInfo2.EquipmentInfos[0].Compartments[0].Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
        //                                                                assign.SeatGroup = findgroup[0].SeatGroup.ToString();
        //                                                                List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo2.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
        //                                                                foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
        //                                                                {
        //                                                                    for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
        //                                                                    {
        //                                                                        assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
        //                                                                    }
        //                                                                    break;
        //                                                                }
        //                                                                assignSeatinfo.Add(assign);
        //                                                            }
        //                                                        }
        //                                                        if (num == 1) break;
        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                                        assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Return));
        //                                                        assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Return));
        //                                                        assign.RecordLocator = pass.RecordLocator;
        //                                                        assign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                                        assign.PassengerID = pass.PassengerID.ToString();
        //                                                        assign.PassengerNumber = (int)pass.PassengerNumber;
        //                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = pAvailableSeatInfo2.EquipmentInfos[0].Compartments[0].Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
        //                                                        assign.SeatGroup = findgroup[0].SeatGroup.ToString();
        //                                                        List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo2.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
        //                                                        foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
        //                                                        {
        //                                                            for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
        //                                                            {
        //                                                                assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
        //                                                            }
        //                                                            break;
        //                                                        }
        //                                                        assignSeatinfo.Add(assign);
        //                                                    }
        //                                                }
        //                                            }

        //                                            //foreach (PassengerContainer pass in newseat)
        //                                            //{
        //                                            //    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
        //                                            //    {
        //                                            //        ABS.Logic.GroupBooking.SeatInfo unassign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                            //        unassign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                            //        unassign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                            //        unassign.RecordLocator = pass.RecordLocator;
        //                                            //        unassign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                            //        unassign.PassengerID = pass.PassengerID.ToString();
        //                                            //        unassign.PassengerNumber = (int)pass.PassengerNumber;
        //                                            //        unassignSeatinfo.Add(unassign);
        //                                            //    }
        //                                            //}
        //                                            //unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
        //                                            if (num == 1) return "Fail to assign change seat, cannot swap seat with different PNR";
        //                                        }

        //                                        if (AssignSeat(assignSeatinfo, assignSeatinfo, model, EnumFlightType.ReturnConnectingFlight))
        //                                        {
        //                                            DeleteXML((string)Session["SeatInfo2Xml"]);
        //                                            Session["ReturnSeatInfo"] = Session["SeatInfo2"];
        //                                        }
        //                                        else
        //                                        {
        //                                            if (Session["minusamount"] != null)
        //                                            {
        //                                                Session["minusamount"] = null;
        //                                                ClearSession();
        //                                                return "Fail to assign change seat, seat amount cannot less than previous seat fees";
        //                                            }
        //                                            else if (Session["failedassign"] != null)
        //                                            {
        //                                                string ms = Session["failedassign"].ToString();
        //                                                Session["failedassign"] = null;
        //                                                ClearSession();
        //                                                return ms;
        //                                            }
        //                                            else
        //                                            {
        //                                                ClearSession();
        //                                                return msgList.Err999997;
        //                                            }
        //                                        }
        //                                    }
        //                                    if (SeatInfo3Checking == 0)
        //                                    {
        //                                        List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
        //                                        List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
        //                                        SeatAvailabilityResponse pAvailableSeatInfo3 = (SeatAvailabilityResponse)Session["pAvailableSeatInfo3"];
        //                                        List<PassengerContainer> newseat = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.ConnectingReturn)) != item.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn))).ToList();
        //                                        if (newseat != null && newseat.Count > 0)
        //                                        {
        //                                            int num = 0;
        //                                            foreach (PassengerContainer pass in newseat)
        //                                            {
        //                                                if (pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn)) != "")
        //                                                {
        //                                                    List<PassengerContainer> found = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.ConnectingReturn)) == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn))).ToList();
        //                                                    if (found != null && found.Count > 0)
        //                                                    {
        //                                                        foreach (PassengerContainer passfound in found)
        //                                                        {
        //                                                            if (pass.RecordLocator != passfound.RecordLocator)
        //                                                            {
        //                                                                num = 1;
        //                                                                break;
        //                                                            }
        //                                                            else
        //                                                            {
        //                                                                ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                                                assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.ConnectingReturn));
        //                                                                assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn));
        //                                                                assign.RecordLocator = pass.RecordLocator;
        //                                                                assign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                                                assign.PassengerID = pass.PassengerID.ToString();
        //                                                                assign.PassengerNumber = (int)pass.PassengerNumber;
        //                                                                List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = pAvailableSeatInfo3.EquipmentInfos[0].Compartments[0].Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn))).ToList();
        //                                                                assign.SeatGroup = findgroup[0].SeatGroup.ToString();
        //                                                                List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo3.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
        //                                                                foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
        //                                                                {
        //                                                                    for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
        //                                                                    {
        //                                                                        assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
        //                                                                    }
        //                                                                    break;
        //                                                                }
        //                                                                assignSeatinfo.Add(assign);
        //                                                            }
        //                                                        }
        //                                                        if (num == 1) break;
        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                                        assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.ConnectingReturn));
        //                                                        assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn));
        //                                                        assign.RecordLocator = pass.RecordLocator;
        //                                                        assign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                                        assign.PassengerID = pass.PassengerID.ToString();
        //                                                        assign.PassengerNumber = (int)pass.PassengerNumber;
        //                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = pAvailableSeatInfo3.EquipmentInfos[0].Compartments[0].Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingReturn))).ToList();
        //                                                        assign.SeatGroup = findgroup[0].SeatGroup.ToString();
        //                                                        List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo3.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
        //                                                        foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
        //                                                        {
        //                                                            for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
        //                                                            {
        //                                                                assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
        //                                                            }
        //                                                            break;
        //                                                        }
        //                                                        assignSeatinfo.Add(assign);
        //                                                    }
        //                                                }
        //                                            }

        //                                            //foreach (PassengerContainer pass in newseat)
        //                                            //{
        //                                            //    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
        //                                            //    {
        //                                            //        ABS.Logic.GroupBooking.SeatInfo unassign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                            //        unassign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                            //        unassign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                            //        unassign.RecordLocator = pass.RecordLocator;
        //                                            //        unassign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                            //        unassign.PassengerID = pass.PassengerID.ToString();
        //                                            //        unassign.PassengerNumber = (int)pass.PassengerNumber;
        //                                            //        unassignSeatinfo.Add(unassign);
        //                                            //    }
        //                                            //}
        //                                            //unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
        //                                            if (num == 1) return "Fail to assign change seat, cannot swap seat with different PNR";
        //                                        }

        //                                        if (AssignSeat(assignSeatinfo, assignSeatinfo, model, EnumFlightType.ReturnConnectingFlight2))
        //                                        {
        //                                            DeleteXML((string)Session["SeatInfo3Xml"]);
        //                                            Session["ReturnConnectingSeatInfo"] = Session["SeatInfo3"];
        //                                        }
        //                                        else
        //                                        {
        //                                            if (Session["minusamount"] != null)
        //                                            {
        //                                                Session["minusamount"] = null;
        //                                                ClearSession();
        //                                                return "Fail to assign change seat, seat amount cannot less than previous seat fees";
        //                                            }
        //                                            else if (Session["failedassign"] != null)
        //                                            {
        //                                                string ms = Session["failedassign"].ToString();
        //                                                Session["failedassign"] = null;
        //                                                ClearSession();
        //                                                return ms;
        //                                            }
        //                                            else
        //                                            {
        //                                                ClearSession();
        //                                                return msgList.Err999997;
        //                                            }
        //                                        }
        //                                    }

        //                                    //Response.Redirect("~/pages/SeatSummary.aspx");
        //                                }
        //                                else
        //                                {
        //                                    ClearSession();
        //                                    return "Please select seat(s) before proceed.";
        //                                    //lblErr.Text = "Please select seat(s) before proceed.";
        //                                    //pnlErr.Visible = true;
        //                                    //Session["ErrorMsg"] = lblErr.Text;
        //                                    //Response.Redirect("~/seats.aspx");
        //                                    //FillFlight(Session["akey"], 0)
        //                                }
        //                            }
        //                            else if (Session["transitdepart"] != null)
        //                            {
        //                                if (SeatInfo0Checking == 0 & SeatInfo1Checking == 0 & SeatInfo2Checking == 0)
        //                                {
        //                                    if (SeatInfo0Checking == 0)
        //                                    {
        //                                        List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
        //                                        List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
        //                                        SeatAvailabilityResponse pAvailableSeatInfo0 = (SeatAvailabilityResponse)Session["pAvailableSeatInfo0"];
        //                                        List<PassengerContainer> newseat = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Depart)) != item.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
        //                                        if (newseat != null && newseat.Count > 0)
        //                                        {
        //                                            int num = 0;
        //                                            foreach (PassengerContainer pass in newseat)
        //                                            {
        //                                                if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
        //                                                {
        //                                                    List<PassengerContainer> found = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Depart)) == pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
        //                                                    if (found != null && found.Count > 0)
        //                                                    {
        //                                                        foreach (PassengerContainer passfound in found)
        //                                                        {
        //                                                            if (pass.RecordLocator != passfound.RecordLocator)
        //                                                            {
        //                                                                num = 1;
        //                                                                break;
        //                                                            }
        //                                                            else
        //                                                            {
        //                                                                ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                                                assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                                                assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                                                assign.RecordLocator = pass.RecordLocator;
        //                                                                assign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                                                assign.PassengerID = pass.PassengerID.ToString();
        //                                                                assign.PassengerNumber = (int)pass.PassengerNumber;
        //                                                                List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = pAvailableSeatInfo0.EquipmentInfos[0].Compartments[0].Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
        //                                                                assign.SeatGroup = findgroup[0].SeatGroup.ToString();
        //                                                                List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo0.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
        //                                                                foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
        //                                                                {
        //                                                                    for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
        //                                                                    {
        //                                                                        assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
        //                                                                    }
        //                                                                    break;
        //                                                                }
        //                                                                assignSeatinfo.Add(assign);
        //                                                            }
        //                                                        }
        //                                                        if (num == 1) break;
        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                                        assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                                        assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                                        assign.RecordLocator = pass.RecordLocator;
        //                                                        assign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                                        assign.PassengerID = pass.PassengerID.ToString();
        //                                                        assign.PassengerNumber = (int)pass.PassengerNumber;
        //                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = pAvailableSeatInfo0.EquipmentInfos[0].Compartments[0].Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
        //                                                        assign.SeatGroup = findgroup[0].SeatGroup.ToString();
        //                                                        List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo0.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
        //                                                        foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
        //                                                        {
        //                                                            for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
        //                                                            {
        //                                                                assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
        //                                                            }
        //                                                            break;
        //                                                        }
        //                                                        assignSeatinfo.Add(assign);
        //                                                    }
        //                                                }
        //                                            }

        //                                            //foreach (PassengerContainer pass in newseat)
        //                                            //{
        //                                            //    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
        //                                            //    {
        //                                            //        ABS.Logic.GroupBooking.SeatInfo unassign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                            //        unassign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                            //        unassign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                            //        unassign.RecordLocator = pass.RecordLocator;
        //                                            //        unassign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                            //        unassign.PassengerID = pass.PassengerID.ToString();
        //                                            //        unassign.PassengerNumber = (int)pass.PassengerNumber;
        //                                            //        unassignSeatinfo.Add(unassign);
        //                                            //    }
        //                                            //}
        //                                            //unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
        //                                            if (num == 1) return "Fail to assign change seat, cannot swap seat with different PNR";
        //                                        }

        //                                        if (AssignSeat(assignSeatinfo, assignSeatinfo, model, EnumFlightType.DepartConnectingFlight))
        //                                        {
        //                                            DeleteXML((string)Session["SeatInfo0Xml"]);
        //                                            Session["DepartSeatInfo"] = Session["SeatInfo0"];
        //                                        }
        //                                        else
        //                                        {
        //                                            if (Session["minusamount"] != null)
        //                                            {
        //                                                Session["minusamount"] = null;
        //                                                ClearSession();
        //                                                return "Fail to assign change seat, seat amount cannot less than previous seat fees";
        //                                            }
        //                                            else if (Session["failedassign"] != null)
        //                                            {
        //                                                string ms = Session["failedassign"].ToString();
        //                                                Session["failedassign"] = null;
        //                                                ClearSession();
        //                                                return ms;
        //                                            }
        //                                            else
        //                                            {
        //                                                ClearSession();
        //                                                return msgList.Err999997;
        //                                            }
        //                                        }
        //                                    }
        //                                    if (SeatInfo1Checking == 0)
        //                                    {
        //                                        List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
        //                                        List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
        //                                        SeatAvailabilityResponse pAvailableSeatInfo1 = (SeatAvailabilityResponse)Session["pAvailableSeatInfo1"];
        //                                        List<PassengerContainer> newseat = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.ConnectingDepart)) != item.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart))).ToList();
        //                                        if (newseat != null && newseat.Count > 0)
        //                                        {
        //                                            int num = 0;
        //                                            foreach (PassengerContainer pass in newseat)
        //                                            {
        //                                                if (pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart)) != "")
        //                                                {
        //                                                    List<PassengerContainer> found = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.ConnectingDepart)) == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart))).ToList();
        //                                                    if (found != null && found.Count > 0)
        //                                                    {
        //                                                        foreach (PassengerContainer passfound in found)
        //                                                        {
        //                                                            if (pass.RecordLocator != passfound.RecordLocator)
        //                                                            {
        //                                                                num = 1;
        //                                                                break;
        //                                                            }
        //                                                            else
        //                                                            {
        //                                                                ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                                                assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.ConnectingDepart));
        //                                                                assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart));
        //                                                                assign.RecordLocator = pass.RecordLocator;
        //                                                                assign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                                                assign.PassengerID = pass.PassengerID.ToString();
        //                                                                assign.PassengerNumber = (int)pass.PassengerNumber;
        //                                                                List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = pAvailableSeatInfo1.EquipmentInfos[0].Compartments[0].Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart))).ToList();
        //                                                                assign.SeatGroup = findgroup[0].SeatGroup.ToString();
        //                                                                List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo1.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
        //                                                                foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
        //                                                                {
        //                                                                    for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
        //                                                                    {
        //                                                                        assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
        //                                                                    }
        //                                                                    break;
        //                                                                }
        //                                                                assignSeatinfo.Add(assign);
        //                                                            }
        //                                                        }
        //                                                        if (num == 1) break;
        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                                        assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.ConnectingDepart));
        //                                                        assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart));
        //                                                        assign.RecordLocator = pass.RecordLocator;
        //                                                        assign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                                        assign.PassengerID = pass.PassengerID.ToString();
        //                                                        assign.PassengerNumber = (int)pass.PassengerNumber;
        //                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = pAvailableSeatInfo1.EquipmentInfos[0].Compartments[0].Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.ConnectingDepart))).ToList();
        //                                                        assign.SeatGroup = findgroup[0].SeatGroup.ToString();
        //                                                        List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo1.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
        //                                                        foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
        //                                                        {
        //                                                            for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
        //                                                            {
        //                                                                assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
        //                                                            }
        //                                                            break;
        //                                                        }
        //                                                        assignSeatinfo.Add(assign);
        //                                                    }
        //                                                }
        //                                            }

        //                                            //foreach (PassengerContainer pass in newseat)
        //                                            //{
        //                                            //    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
        //                                            //    {
        //                                            //        ABS.Logic.GroupBooking.SeatInfo unassign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                            //        unassign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                            //        unassign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                            //        unassign.RecordLocator = pass.RecordLocator;
        //                                            //        unassign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                            //        unassign.PassengerID = pass.PassengerID.ToString();
        //                                            //        unassign.PassengerNumber = (int)pass.PassengerNumber;
        //                                            //        unassignSeatinfo.Add(unassign);
        //                                            //    }
        //                                            //}
        //                                            //unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
        //                                            if (num == 1) return "Fail to assign change seat, cannot swap seat with different PNR";
        //                                        }

        //                                        if (AssignSeat(assignSeatinfo, assignSeatinfo, model, EnumFlightType.DepartConnectingFlight2))
        //                                        {
        //                                            DeleteXML((string)Session["SeatInfo1Xml"]);
        //                                            Session["DepartConnectingSeatInfo"] = Session["SeatInfo1"];
        //                                        }
        //                                        else
        //                                        {
        //                                            if (Session["minusamount"] != null)
        //                                            {
        //                                                Session["minusamount"] = null;
        //                                                ClearSession();
        //                                                return "Fail to assign change seat, seat amount cannot less than previous seat fees";
        //                                            }
        //                                            else if (Session["failedassign"] != null)
        //                                            {
        //                                                string ms = Session["failedassign"].ToString();
        //                                                Session["failedassign"] = null;
        //                                                ClearSession();
        //                                                return ms;
        //                                            }
        //                                            else
        //                                            {
        //                                                ClearSession();
        //                                                return msgList.Err999997;
        //                                            }
        //                                        }
        //                                    }
        //                                    if (SeatInfo2Checking == 0)
        //                                    {
        //                                        List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
        //                                        List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
        //                                        SeatAvailabilityResponse pAvailableSeatInfo2 = (SeatAvailabilityResponse)Session["pAvailableSeatInfo2"];
        //                                        List<PassengerContainer> newseat = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Return)) != item.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
        //                                        if (newseat != null && newseat.Count > 0)
        //                                        {
        //                                            int num = 0;
        //                                            foreach (PassengerContainer pass in newseat)
        //                                            {
        //                                                if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Return)) != "")
        //                                                {
        //                                                    List<PassengerContainer> found = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Return)) == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
        //                                                    if (found != null && found.Count > 0)
        //                                                    {
        //                                                        foreach (PassengerContainer passfound in found)
        //                                                        {
        //                                                            if (pass.RecordLocator != passfound.RecordLocator)
        //                                                            {
        //                                                                num = 1;
        //                                                                break;
        //                                                            }
        //                                                            else
        //                                                            {
        //                                                                ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                                                assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Return));
        //                                                                assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Return));
        //                                                                assign.RecordLocator = pass.RecordLocator;
        //                                                                assign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                                                assign.PassengerID = pass.PassengerID.ToString();
        //                                                                assign.PassengerNumber = (int)pass.PassengerNumber;
        //                                                                List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = pAvailableSeatInfo2.EquipmentInfos[0].Compartments[0].Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
        //                                                                assign.SeatGroup = findgroup[0].SeatGroup.ToString();
        //                                                                List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo2.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
        //                                                                foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
        //                                                                {
        //                                                                    for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
        //                                                                    {
        //                                                                        assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
        //                                                                    }
        //                                                                    break;
        //                                                                }
        //                                                                assignSeatinfo.Add(assign);
        //                                                            }
        //                                                        }
        //                                                        if (num == 1) break;
        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                                        assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Return));
        //                                                        assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Return));
        //                                                        assign.RecordLocator = pass.RecordLocator;
        //                                                        assign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                                        assign.PassengerID = pass.PassengerID.ToString();
        //                                                        assign.PassengerNumber = (int)pass.PassengerNumber;
        //                                                        List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = pAvailableSeatInfo2.EquipmentInfos[0].Compartments[0].Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
        //                                                        assign.SeatGroup = findgroup[0].SeatGroup.ToString();
        //                                                        List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo2.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
        //                                                        foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
        //                                                        {
        //                                                            for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
        //                                                            {
        //                                                                assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
        //                                                            }
        //                                                            break;
        //                                                        }
        //                                                        assignSeatinfo.Add(assign);
        //                                                    }
        //                                                }
        //                                            }

        //                                            //foreach (PassengerContainer pass in newseat)
        //                                            //{
        //                                            //    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
        //                                            //    {
        //                                            //        ABS.Logic.GroupBooking.SeatInfo unassign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                            //        unassign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                            //        unassign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                            //        unassign.RecordLocator = pass.RecordLocator;
        //                                            //        unassign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                            //        unassign.PassengerID = pass.PassengerID.ToString();
        //                                            //        unassign.PassengerNumber = (int)pass.PassengerNumber;
        //                                            //        unassignSeatinfo.Add(unassign);
        //                                            //    }
        //                                            //}
        //                                            //unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
        //                                            if (num == 1) return "Fail to assign change seat, cannot swap seat with different PNR";
        //                                        }

        //                                        if (AssignSeat(assignSeatinfo, assignSeatinfo, model, EnumFlightType.ReturnConnectingFlight))
        //                                        {
        //                                            DeleteXML((string)Session["SeatInfo2Xml"]);
        //                                            Session["ReturnSeatInfo"] = Session["SeatInfo2"];
        //                                        }
        //                                        else
        //                                        {
        //                                            if (Session["minusamount"] != null)
        //                                            {
        //                                                Session["minusamount"] = null;
        //                                                ClearSession();
        //                                                return "Fail to assign change seat, seat amount cannot less than previous seat fees";
        //                                            }
        //                                            else if (Session["failedassign"] != null)
        //                                            {
        //                                                string ms = Session["failedassign"].ToString();
        //                                                Session["failedassign"] = null;
        //                                                ClearSession();
        //                                                return ms;
        //                                            }
        //                                            else
        //                                            {
        //                                                ClearSession();
        //                                                return msgList.Err999997;
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    ClearSession();
        //                                    return "Please select seat(s) before proceed.";
        //                                }
        //                            }

        //                            assignSeatDone = true;

        //                        }
        //                        break;
        //                    case EnumFlight.DirectFlight:
        //                        if (resp == null == false)
        //                        {
        //                            //Flight_Info pDepartFlightInfo = new Flight_Info();
        //                            //pDepartFlightInfo = Session["DepartFlightInfo"];

        //                            if (!objGeneral.IsInternationalFlight(resp.Booking.Journeys[0].Segments[0].DepartureStation, resp.Booking.Journeys[0].Segments[0].ArrivalStation, Request.PhysicalApplicationPath))
        //                            {
        //                                //SeatInfo0Checking = 0;
        //                                for (int i = 0; i <= ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"]).Count - 1; i++)
        //                                {
        //                                    lstPassengerContainer[i].setNewUnitDesignator(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"])[i].SelectedSeat, (PassengerContainer.FlightType.Depart));
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (Session["SeatInfo0"] == null == false)
        //                                {
        //                                    for (int i = 0; i <= ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"]).Count - 1; i++)
        //                                    {

        //                                        if (string.IsNullOrEmpty(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"])[i].SelectedSeat))
        //                                        {

        //                                            //SeatInfo0Checking = SeatInfo0Checking + 1;
        //                                        }

        //                                        lstPassengerContainer[i].setNewUnitDesignator(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"])[i].SelectedSeat, (PassengerContainer.FlightType.Depart));

        //                                        //List<PassengerContainer>newseat = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType)EnumFlightType.DepartFlight) == ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"])[i].SelectedSeat.ToString()).ToList();
        //                                        //if (newseat != null && newseat.Count == 0)
        //                                        //{
        //                                        //    objPassengerContainerNew.setUnitDesignator(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"])[i].SelectedSeat, (PassengerContainer.FlightType)EnumFlightType.DepartFlight);
        //                                        //    lstPassengerContainerNew.Add(objPassengerContainerNew);                                            
        //                                        //}

        //                                    }
        //                                    if (SeatInfo0Checking == 0)
        //                                    {
        //                                        //if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"], model, EnumFlightType.DepartFlight))
        //                                        //{
        //                                        //    DeleteXML((string)Session["SeatInfo0Xml"]);
        //                                        //    Session["DepartSeatInfo"] = Session["SeatInfo0"];
        //                                        //}
        //                                    }

        //                                }
        //                                else
        //                                {
        //                                    SeatInfo0Checking = SeatInfo0Checking + 1;
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            //Response.Redirect("~/pages/InvalidPage.aspx")
        //                            SeatInfo0Checking = 0;
        //                        }

        //                        if ((bool)Session["OneWay"] == false)
        //                        {
        //                            //Flight_Info pReturnFlightInfo = new Flight_Info();
        //                            //pReturnFlightInfo = Session["ReturnFlightInfo"];

        //                            if (!objGeneral.IsInternationalFlight(resp.Booking.Journeys[1].Segments[0].DepartureStation, resp.Booking.Journeys[1].Segments[0].ArrivalStation, Request.PhysicalApplicationPath))
        //                            {
        //                                //SeatInfo1Checking = 0;
        //                                for (int j = 0; j <= ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"]).Count - 1; j++)
        //                                {
        //                                    lstPassengerContainer[j].setNewUnitDesignator(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"])[j].SelectedSeat, (PassengerContainer.FlightType.Return));
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (Session["SeatInfo1"] == null == false)
        //                                {
        //                                    for (int j = 0; j <= ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"]).Count - 1; j++)
        //                                    {
        //                                        if (string.IsNullOrEmpty(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"])[j].SelectedSeat))
        //                                        {
        //                                            //SeatInfo1Checking = SeatInfo1Checking + 1;

        //                                        }
        //                                        lstPassengerContainer[j].setNewUnitDesignator(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"])[j].SelectedSeat, (PassengerContainer.FlightType.Return));

        //                                        //List<PassengerContainer> newseat = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType)EnumFlightType.DepartFlight) == ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"])[j].SelectedSeat.ToString()).ToList();
        //                                        //if (newseat != null && newseat.Count == 0)
        //                                        //{
        //                                        //    objPassengerContainerNew.setUnitDesignator(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"])[j].SelectedSeat, (PassengerContainer.FlightType)EnumFlightType.DepartFlight);
        //                                        //    lstPassengerContainerNew.Add(objPassengerContainerNew);
        //                                        //}


        //                                    }
        //                                    if (SeatInfo1Checking == 0)
        //                                    {
        //                                        //if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"], model2, EnumFlightType.ReturnFlight))
        //                                        //{
        //                                        //    DeleteXML((string)Session["SeatInfo1Xml"]);
        //                                        //    Session["ReturnSeatInfo"] = Session["SeatInfo1"];
        //                                        //}
        //                                    }

        //                                }
        //                                else
        //                                {
        //                                    //SeatInfo1Checking = SeatInfo1Checking + 1;
        //                                }
        //                            }

        //                        }
        //                        else
        //                        {
        //                            SeatInfo1Checking = 0;
        //                        }
        //                        Page.Validate("PrimaryMandatory");
        //                        if (Page.IsValid)
        //                        {
        //                            if (SeatInfo0Checking == 0 & SeatInfo1Checking == 0)
        //                            {
        //                                ClearSeatFeeValue();
        //                                if (SeatInfo0Checking == 0 && resp != null)
        //                                {
        //                                    List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
        //                                    List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
        //                                    SeatAvailabilityResponse pAvailableSeatInfo0 = (SeatAvailabilityResponse)Session["pAvailableSeatInfo0"];
        //                                    List<PassengerContainer> newseat = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Depart)) != item.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
        //                                    if (newseat != null && newseat.Count > 0)
        //                                    {
        //                                        int num = 0;
        //                                        foreach (PassengerContainer pass in newseat)
        //                                        {
        //                                            if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
        //                                            {
        //                                                List<PassengerContainer> found = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Depart)) == pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
        //                                                if (found != null && found.Count > 0)
        //                                                {
        //                                                    foreach (PassengerContainer passfound in found)
        //                                                    {
        //                                                        if (pass.RecordLocator != passfound.RecordLocator)
        //                                                        {
        //                                                            num = 1;
        //                                                            break;
        //                                                        }
        //                                                        else
        //                                                        {
        //                                                            ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                                            assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                                            assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                                            assign.RecordLocator = pass.RecordLocator;
        //                                                            assign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                                            assign.PassengerID = pass.PassengerID.ToString();
        //                                                            assign.PassengerNumber = (int)pass.PassengerNumber;
        //                                                            List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = pAvailableSeatInfo0.EquipmentInfos[0].Compartments[0].Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
        //                                                            assign.SeatGroup = findgroup[0].SeatGroup.ToString();
        //                                                            List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo0.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
        //                                                            foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
        //                                                            {
        //                                                                for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
        //                                                                {
        //                                                                    assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
        //                                                                }
        //                                                                break;
        //                                                            }
        //                                                            assignSeatinfo.Add(assign);
        //                                                        }
        //                                                    }
        //                                                    if (num == 1) break;
        //                                                }
        //                                                else
        //                                                {
        //                                                    ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                                    assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                                    assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                                    assign.RecordLocator = pass.RecordLocator;
        //                                                    assign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                                    assign.PassengerID = pass.PassengerID.ToString();
        //                                                    assign.PassengerNumber = (int)pass.PassengerNumber;
        //                                                    List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = pAvailableSeatInfo0.EquipmentInfos[0].Compartments[0].Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart))).ToList();
        //                                                    assign.SeatGroup = findgroup[0].SeatGroup.ToString();
        //                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo0.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
        //                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
        //                                                    {
        //                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
        //                                                        {
        //                                                            assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
        //                                                        }
        //                                                        break;
        //                                                    }
        //                                                    assignSeatinfo.Add(assign);
        //                                                }
        //                                            }
        //                                        }

        //                                        //foreach (PassengerContainer pass in newseat)
        //                                        //{
        //                                        //    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
        //                                        //    {
        //                                        //        ABS.Logic.GroupBooking.SeatInfo unassign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                        //        unassign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                        //        unassign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                        //        unassign.RecordLocator = pass.RecordLocator;
        //                                        //        unassign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                        //        unassign.PassengerID = pass.PassengerID.ToString();
        //                                        //        unassign.PassengerNumber = (int)pass.PassengerNumber;
        //                                        //        unassignSeatinfo.Add(unassign);
        //                                        //    }
        //                                        //}
        //                                        //unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
        //                                        if (num == 1) return "Fail to assign change seat, cannot swap seat with different PNR";
        //                                    }
        //                                    //if (Session["DepartExistingSeatInfo"] != null)
        //                                    //{

        //                                    //    unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
        //                                    //}

        //                                    if (AssignSeat(assignSeatinfo, assignSeatinfo, model, EnumFlightType.DepartFlight))
        //                                    {
        //                                        DeleteXML((string)Session["SeatInfo0Xml"]);
        //                                        Session["DepartSeatInfo"] = Session["SeatInfo0"];
        //                                    }
        //                                    else
        //                                    {
        //                                        if (Session["minusamount"] != null)
        //                                        {
        //                                            Session["minusamount"] = null;
        //                                            ClearSession();
        //                                            return "Fail to assign change seat, seat amount cannot less than previous seat fees";
        //                                        }
        //                                        else if (Session["failedassign"] != null)
        //                                        {
        //                                            string ms = Session["failedassign"].ToString();
        //                                            Session["failedassign"] = null;
        //                                            ClearSession();
        //                                            return ms;
        //                                        }
        //                                        else
        //                                        {
        //                                            ClearSession();
        //                                            return msgList.Err999997;
        //                                        }
        //                                    }
        //                                }

        //                                if (SeatInfo1Checking == 0 && (bool)Session["OneWay"] == false)
        //                                {

        //                                    List<ABS.Logic.GroupBooking.SeatInfo> unassignSeatinfo1 = new List<ABS.Logic.GroupBooking.SeatInfo>();
        //                                    List<ABS.Logic.GroupBooking.SeatInfo> assignSeatinfo1 = new List<ABS.Logic.GroupBooking.SeatInfo>();
        //                                    SeatAvailabilityResponse pAvailableSeatInfo1 = (SeatAvailabilityResponse)Session["pAvailableSeatInfo1"];
        //                                    List<PassengerContainer> newseat = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Return)) != item.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
        //                                    if (newseat != null && newseat.Count > 0)
        //                                    {
        //                                        int num = 0;
        //                                        foreach (PassengerContainer pass in newseat)
        //                                        {
        //                                            if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Return)) != "")
        //                                            {
        //                                                List<PassengerContainer> found = lstPassengerContainer.Where(item => item.getUnitDesignator((PassengerContainer.FlightType.Return)) == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
        //                                                if (found != null && found.Count > 0)
        //                                                {
        //                                                    foreach (PassengerContainer passfound in found)
        //                                                    {
        //                                                        if (pass.RecordLocator != passfound.RecordLocator)
        //                                                        {
        //                                                            num = 1;
        //                                                            break;
        //                                                        }
        //                                                        else
        //                                                        {
        //                                                            ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                                            assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Return));
        //                                                            assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Return));
        //                                                            assign.RecordLocator = pass.RecordLocator;
        //                                                            assign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                                            assign.PassengerID = pass.PassengerID.ToString();
        //                                                            assign.PassengerNumber = (int)pass.PassengerNumber;
        //                                                            List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = pAvailableSeatInfo1.EquipmentInfos[0].Compartments[0].Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
        //                                                            assign.SeatGroup = findgroup[0].SeatGroup.ToString();
        //                                                            List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo1.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
        //                                                            foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
        //                                                            {
        //                                                                for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
        //                                                                {
        //                                                                    assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
        //                                                                }
        //                                                                break;
        //                                                            }
        //                                                            assignSeatinfo1.Add(assign);
        //                                                        }
        //                                                    }
        //                                                    if (num == 1) break;
        //                                                }
        //                                                else
        //                                                {
        //                                                    ABS.Logic.GroupBooking.SeatInfo assign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                                    assign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Return));
        //                                                    assign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Return));
        //                                                    assign.RecordLocator = pass.RecordLocator;
        //                                                    assign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                                    assign.PassengerID = pass.PassengerID.ToString();
        //                                                    assign.PassengerNumber = (int)pass.PassengerNumber;
        //                                                    List<ABS.Navitaire.BookingManager.SeatInfo> findgroup = pAvailableSeatInfo1.EquipmentInfos[0].Compartments[0].Seats.Where(item => item.SeatDesignator == pass.getNewUnitDesignator((PassengerContainer.FlightType.Return))).ToList();
        //                                                    assign.SeatGroup = findgroup[0].SeatGroup.ToString();
        //                                                    List<ABS.Navitaire.BookingManager.SeatGroupPassengerFee> findamount = pAvailableSeatInfo1.SeatGroupPassengerFees.Where(item => item.SeatGroup.ToString() == assign.SeatGroup).ToList();
        //                                                    foreach (ABS.Navitaire.BookingManager.SeatGroupPassengerFee passfindamount in findamount)
        //                                                    {
        //                                                        for (int a = 0; a < passfindamount.PassengerFee.ServiceCharges.Length; a++)
        //                                                        {
        //                                                            assign.SeatAmount += passfindamount.PassengerFee.ServiceCharges[a].Amount;
        //                                                        }
        //                                                        break;
        //                                                    }
        //                                                    assignSeatinfo1.Add(assign);
        //                                                }
        //                                            }
        //                                        }

        //                                        //foreach (PassengerContainer pass in newseat)
        //                                        //{
        //                                        //    if (pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart)) != "")
        //                                        //    {
        //                                        //        ABS.Logic.GroupBooking.SeatInfo unassign = new ABS.Logic.GroupBooking.SeatInfo();
        //                                        //        unassign.CurrentSeat = pass.getUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                        //        unassign.SelectedSeat = pass.getNewUnitDesignator((PassengerContainer.FlightType.Depart));
        //                                        //        unassign.RecordLocator = pass.RecordLocator;
        //                                        //        unassign.CompartmentDesignator = pass.CompartmentDesignator;
        //                                        //        unassign.PassengerID = pass.PassengerID.ToString();
        //                                        //        unassign.PassengerNumber = (int)pass.PassengerNumber;
        //                                        //        unassignSeatinfo.Add(unassign);
        //                                        //    }
        //                                        //}
        //                                        //unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["DepartExistingSeatInfo"];
        //                                        if (num == 1) return "Fail to assign change seat, cannot swap seat with different PNR";
        //                                    }
        //                                    //if (Session["ReturnExistingSeatInfo"] != null)
        //                                    //{
        //                                    //    unassignSeatinfo = (List<ABS.Logic.GroupBooking.SeatInfo>)Session["ReturnExistingSeatInfo"];
        //                                    //}
        //                                    if (AssignSeat(assignSeatinfo1, assignSeatinfo1, model, EnumFlightType.ReturnFlight))
        //                                    //if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"], unassignSeatinfo, model, EnumFlightType.ReturnFlight))
        //                                    {
        //                                        DeleteXML((string)Session["SeatInfo1Xml"]);
        //                                        Session["ReturnSeatInfo"] = Session["SeatInfo1"];
        //                                    }
        //                                    else
        //                                    {
        //                                        if (Session["minusamount"] != null)
        //                                        {
        //                                            Session["minusamount"] = null;
        //                                            ClearSession();
        //                                            return "Fail to assign change seat, seat amount cannot less than previous seat fees";
        //                                        }
        //                                        else if (Session["failedassign"] != null)
        //                                        {
        //                                            string ms = Session["failedassign"].ToString();
        //                                            Session["failedassign"] = null;
        //                                            ClearSession();
        //                                            return ms;
        //                                        }
        //                                        else
        //                                        {
        //                                            ClearSession();
        //                                            return msgList.Err999997;
        //                                        }
        //                                    }
        //                                }



        //                                //end, update total amount
        //                                assignSeatDone = true;
        //                                //return "";
        //                                //Response.Redirect("~/pages/SeatSummary.aspx");
        //                            }
        //                            else
        //                            {
        //                                ClearSession();
        //                                return "Please select seat(s) before proceed.";
        //                                //lblErr.Text = "Please select seat(s) before proceed.";
        //                                //pnlErr.Visible = true;
        //                                //Session["ErrorMsg"] = lblErr.Text;
        //                                //Response.Redirect("~/seats.aspx");
        //                            }
        //                        }


        //                        break;
        //                }
        //            }

        //            if (assignSeatDone)
        //            {
        //                //begin, update total amount
        //                if (!string.IsNullOrEmpty(hID.Value))
        //                {
        //                    decimal numValue;
        //                    decimal TotalSeatDepart = 0;
        //                    decimal TotalSeatReturn = 0;

        //                    string TransID = (string)Session["TransID"];
        //                    //tyas'
        //                    //List<BookingTransactionDetail> listBookingDetail = new List<BookingTransactionDetail>();
        //                    //listBookingDetail = objBooking.GetAllBK_TRANSDTLFilterAll(TransID, 0);
        //                    //tyas
        //                    listBookingDetail = Session["listBookingDetail"] as List<BookingTransactionDetail>;

        //                    if (Session["PNRsCommit"] != null)
        //                    {
        //                        string msg = "";
        //                        DataTable dtPNRs = (DataTable)Session["PNRsCommit"];
        //                        if (dtPNRs.Rows.Count > 0)
        //                        {
        //                            for (int i = 0; i < dtPNRs.Rows.Count; i++)
        //                            {
        //                                if (absNavitaire.BookingCommitChange(dtPNRs.Rows[i]["PNR"].ToString(), dtPNRs.Rows[i]["SessionID"].ToString(), ref msg, dtPNRs.Rows[i]["Currency"].ToString(), true, true) == false)
        //                                {
        //                                    ClearSession();
        //                                    return "Fail to change seat, please try again";
        //                                }
        //                            }
        //                        }

        //                    }

        //                    if (Session["DepartFlightSeatFees"] != null)
        //                    {
        //                        if (decimal.TryParse(Session["DepartFlightSeatFees"].ToString(), out numValue)) TotalSeatDepart = TotalSeatDepart + Convert.ToDecimal(Session["DepartFlightSeatFees"].ToString());
        //                    }
        //                    if (Session["DepartConnectingFlightSeatFees"] != null)
        //                    {
        //                        if (decimal.TryParse(Session["DepartConnectingFlightSeatFees"].ToString(), out numValue)) TotalSeatDepart = TotalSeatDepart + Convert.ToDecimal(Session["DepartConnectingFlightSeatFees"].ToString());
        //                    }
        //                    if (Session["DepartConnectingFlightSeatFees2"] != null)
        //                    {
        //                        if (decimal.TryParse(Session["DepartConnectingFlightSeatFees2"].ToString(), out numValue)) TotalSeatDepart = TotalSeatDepart + Convert.ToDecimal(Session["DepartConnectingFlightSeatFees2"].ToString());
        //                    }

        //                    if (Session["ReturnFlightSeatFees"] != null)
        //                    {
        //                        if (decimal.TryParse(Session["ReturnFlightSeatFees"].ToString(), out numValue)) TotalSeatReturn = TotalSeatReturn + Convert.ToDecimal(Session["ReturnFlightSeatFees"].ToString());
        //                    }
        //                    if (Session["ReturnConnectingFlightSeatFees"] != null)
        //                    {
        //                        if (decimal.TryParse(Session["ReturnConnectingFlightSeatFees"].ToString(), out numValue)) TotalSeatReturn = TotalSeatReturn + Convert.ToDecimal(Session["ReturnConnectingFlightSeatFees"].ToString());
        //                    }
        //                    if (Session["ReturnConnectingFlightSeatFees2"] != null)
        //                    {
        //                        if (decimal.TryParse(Session["ReturnConnectingFlightSeatFees2"].ToString(), out numValue)) TotalSeatReturn = TotalSeatReturn + Convert.ToDecimal(Session["ReturnConnectingFlightSeatFees2"].ToString());
        //                    }

        //                    //decimal TotalAmountGoing = 0;
        //                    //decimal TotalAmountReturn = 0;

        //                    //UpdateTotalAmount(TotalSeatDepart, TotalSeatReturn, ref TotalAmountGoing, ref TotalAmountReturn);


        //                    //if (iIndexDepart >= 0) listBookingDetail[iIndexDepart].LineSeat = TotalSeatDepart;
        //                    //if (iIndexReturn >= 0) listBookingDetail[iIndexReturn].LineSeat = TotalSeatReturn;

        //                    BookingTransactionMain bookingMain = new BookingTransactionMain();
        //                    bookingMain = objBooking.GetSingleBK_TRANSMAIN(TransId);

        //                    //added by ketee, validate if change seat amount > previous amout , not allow to change

        //                    decimal TotalChangeSeatAmount = 0;
        //                    TotalChangeSeatAmount = TotalSeatDepart + TotalSeatReturn;

        //                    //if (TotalChangeSeatAmount < bookingMain.TransTotalSeat)
        //                    //    return "Fail to assign change seat, seat amount cannot less than previous seat fees";

        //                    if (TotalSeatDepart != 0 || TotalSeatReturn != 0)
        //                    {
        //                        objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        //                        if (Session["totalamountdueseatfeeallcommit"] != null && Convert.ToDecimal(Session["totalamountdueseatfeeallcommit"]) != 0)
        //                        {
        //                            decimal totalamountdue = Convert.ToDecimal(Session["totalamountdueseatfeeallcommit"]);
        //                            bookingMain.TransTotalSeat = bookingMain.TransTotalSeat + totalamountdue;
        //                            bookingMain.TotalAmtGoing = bookingMain.TotalAmtGoing + Convert.ToDecimal(Session["totalamountdueseatfeeallcommitgoing"]);
        //                            bookingMain.TotalAmtReturn = bookingMain.TotalAmtReturn + Convert.ToDecimal(Session["totalamountdueseatfeeallcommitreturn"]);
        //                            bookingMain.TransSubTotal = bookingMain.TotalAmtGoing + bookingMain.TotalAmtReturn;
        //                            bookingMain.TransTotalAmt = bookingMain.TotalAmtGoing + bookingMain.TotalAmtReturn;
        //                            foreach (BookingTransactionDetail a in listBookingDetail)
        //                            {
        //                                if (a.LineConnectingSeat != null && a.LineConnectingSeat > 0 && a.LineConnectingSeat2 != null && a.LineConnectingSeat2 > 0)
        //                                {
        //                                    a.LineSeat = a.LineConnectingSeat + a.LineConnectingSeat2;
        //                                }
        //                            }


        //                            objBooking.UpdateTotalSeat(TransId, bookingMain, listBookingDetail);

        //                            if (Session["totalseatfeeall"] != null && Convert.ToDecimal(Session["totalseatfeeall"]) == 0)
        //                            {
        //                                if (Page.IsCallback)
        //                                    ASPxCallback.RedirectOnCallback("BookingDetail.aspx?k=" + HashingKey + "&TransID=" + TransId);
        //                            }
        //                            else
        //                            {
        //                                //totalsum = TotSSRDepart + TotSSRReturn;
        //                                totalamountdue = (Convert.ToDecimal(Session["totalamountdueseatfeeall"]));
        //                                bookingMain.TransTotalSeat = bookingMain.TransTotalSeat + totalamountdue;
        //                                bookingMain.TotalAmtGoing = (bookingMain.TotalAmtGoing + Convert.ToDecimal(Session["totalamountdueseatfeeallgoing"]));
        //                                bookingMain.TotalAmtReturn = (bookingMain.TotalAmtReturn + Convert.ToDecimal(Session["totalamountdueseatfeeallreturn"]));
        //                                bookingMain.TransSubTotal = bookingMain.TotalAmtGoing + bookingMain.TotalAmtReturn;
        //                                bookingMain.TransTotalAmt = bookingMain.TotalAmtGoing + bookingMain.TotalAmtReturn;

        //                                objBooking.FillChgTransMain(bookingMain);
        //                                HttpContext.Current.Session.Remove("bookingMain");
        //                                HttpContext.Current.Session.Add("bookingMain", bookingMain);

        //                                Session["ChgMode"] = "4"; //4= Manage Seats
        //                                ASPxCallback.RedirectOnCallback(GroupBooking.Web.Shared.MySite.PublicPages.Payment);
        //                            }
        //                        }
        //                        else
        //                        {

        //                            //decimal totalsum = (TotSSRDepart + TotSSRReturn);
        //                            decimal totalamountdue = Convert.ToDecimal(Session["totalamountdueseatfeeall"]);
        //                            bookingMain.TransTotalSeat = bookingMain.TransTotalSSR + totalamountdue;
        //                            bookingMain.TotalAmtGoing = (bookingMain.TotalAmtGoing + Convert.ToDecimal(Session["totalamountdueseatfeeallgoing"]));
        //                            bookingMain.TotalAmtReturn = (bookingMain.TotalAmtReturn + Convert.ToDecimal(Session["totalamountdueseatfeeallreturn"]));
        //                            bookingMain.TransSubTotal = bookingMain.TotalAmtGoing + bookingMain.TotalAmtReturn;
        //                            bookingMain.TransTotalAmt = bookingMain.TotalAmtGoing + bookingMain.TotalAmtReturn;

        //                            objBooking.FillChgTransMain(bookingMain);
        //                            HttpContext.Current.Session.Remove("bookingMain");
        //                            HttpContext.Current.Session.Add("bookingMain", bookingMain);

        //                            Session["ChgMode"] = "2"; //1= Manage Add-On
        //                            ASPxCallback.RedirectOnCallback(GroupBooking.Web.Shared.MySite.PublicPages.Payment);
        //                        }

        //                    }
        //                    else
        //                    {
        //                        if (Session["totalamountdueseatfeeallcommit"] != null && Convert.ToDecimal(Session["totalamountdueseatfeeallcommit"]) != 0)
        //                        {
        //                            //decimal totalsum = TotSSRDepartcommit + TotSSRReturncommit;
        //                            decimal totalamountdue = Convert.ToDecimal(Session["totalamountdueseatfeeallcommit"]);
        //                            bookingMain.TransTotalSeat = bookingMain.TransTotalSeat + totalamountdue;
        //                            bookingMain.TotalAmtGoing = bookingMain.TotalAmtGoing + Convert.ToDecimal(Session["totalamountdueseatfeeallcommitgoing"]);
        //                            bookingMain.TotalAmtReturn = bookingMain.TotalAmtReturn + Convert.ToDecimal(Session["totalamountdueseatfeeallcommitreturn"]);
        //                            bookingMain.TransSubTotal = bookingMain.TotalAmtGoing + bookingMain.TotalAmtReturn;
        //                            bookingMain.TransTotalAmt = bookingMain.TotalAmtGoing + bookingMain.TotalAmtReturn;
        //                            bookingMain.PaymentAmtEx2 = bookingMain.PaymentAmtEx2 + totalamountdue;
        //                            listBookingDetail[0].PayDueAmount2 += totalamountdue;
        //                            objBooking.UpdateTotalSeat(TransId, bookingMain, listBookingDetail);


        //                            if (Page.IsCallback)
        //                                ASPxCallback.RedirectOnCallback("BookingDetail.aspx?k=" + HashingKey + "&TransID=" + TransId);
        //                            //objBooking.UpdateTotalSSR(listAll[0].TransID, bookingMain, listBookingDetail);
        //                        }
        //                        else
        //                        {
        //                            if (Page.IsCallback)
        //                                ASPxCallback.RedirectOnCallback("BookingDetail.aspx?k=" + HashingKey + "&TransID=" + TransId);
        //                        }

        //                        //Response.RedirectLocation = "bookingdetail.aspx?k=" + Request.QueryString["k"] + "&TransID=" + Request.QueryString["TransID"];
        //                    }


        //                    //added by ketee, for conencting flight, total up the seat fees from the connecting seat fees columns
        //                    //temp remarked


        //                    //added by ketee, commit seat change
        //                    //string errMsg = "";
        //                    //if (HttpContext.Current.Session["TransDetail"] != null)
        //                    //{
        //                    //    dtDetail = (DataTable)HttpContext.Current.Session["TransDetail"];
        //                    //    for (int j = 0; j < dtDetail.Rows.Count; j++)
        //                    //    {

        //                    //        ABS.Navitaire.BookingManager.Booking bookingResp = absNavitaire.GetBookingFromState(dtDetail.Rows[j]["SellKey"].ToString(), 2);
        //                    //        if (bookingResp != null && bookingResp.RecordLocator.Trim() == dtDetail.Rows[j]["RecordLocator"].ToString())
        //                    //        {
        //                    //            string xml = absNavitaire.GetXMLString(bookingResp);
        //                    //            if (absNavitaire.BookingCommitChange(dtDetail.Rows[j]["RecordLocator"].ToString(), dtDetail.Rows[j]["SellKey"].ToString(),
        //                    //                                                ref errMsg) == false)
        //                    //                return "Fail to change seat, please try again";
        //                    //        }
        //                    //        else
        //                    //            return "Transaction error, please contact system administrator, sorry for the inconvenience";
        //                    //    }

        //                    //}

        //                    //end, temp remarked

        //                    return "";
        //                }
        //                else
        //                {
        //                    return "Fail to assign seat, kindly try again";
        //                }
        //            }
        //            else
        //            {
        //                return "Fail to assign seat, kindly try again later...";
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            //sTraceLog(ex.ToString);
        //            return ex.Message;
        //            //lblErr.Text = ex.ToString();
        //        }
        //    }
    }
}