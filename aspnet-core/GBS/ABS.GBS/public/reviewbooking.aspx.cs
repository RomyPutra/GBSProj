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
using ABS.GBS.Log;
using StackExchange.Profiling;

namespace GroupBooking.Web.Booking
{
    public partial class ItineraryDetail : System.Web.UI.Page
    {
        #region declaration

        UserSet MyUserSet;
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogControl log = new LogControl();
        SystemLog SystemLog = new SystemLog();

        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
        decimal totalFlightFare, totalFUEFare, totalBaggageFare = 0;

        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
        ABS.Logic.GroupBooking.Agent.AgentControl objAgent = new ABS.Logic.GroupBooking.Agent.AgentControl();
        ABS.Logic.GroupBooking.Agent.AgentProfileControl objAgentProfile = new ABS.Logic.GroupBooking.Agent.AgentProfileControl();

        BookingTransactionDetail bookDTLInfo = new BookingTransactionDetail();
        List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
        BookingTransactionMain bookHDRInfo = new BookingTransactionMain();

        //BookingTransactionTax transTaxInfo = new BookingTransactionTax();

        //BookingTaxFeesControl taxFeesControlInfo = new BookingTaxFeesControl();

        //BookingTransactionFees transFeesInfo = new BookingTransactionFees();
        //List<BookingTransactionFees> lsttransFeesInfo = new List<BookingTransactionFees>();

        ABS.Logic.GroupBooking.Agent.AgentProfile agent = new ABS.Logic.GroupBooking.Agent.AgentProfile();

        string Currency = "USD";
        decimal TotalAmount = 0;
        decimal APT = 0;
        decimal AVGFare = 0;
        string tranID = "";

        //20170425 - Sienny
        string TransID = "";
        DataTable dtAddOn;
        Boolean returnFlight = false;

        DataTable dtTaxFees = new DataTable();

        private static int num = 0;
        private static int departID = 0;
        private static string ReturnID = "";
        private static decimal TotalMoney = 0;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            var profiler = MiniProfiler.Current;
            Session["NewBooking"] = "true";
            if (Session["AgentSet"] != null)
            {
                MyUserSet = (UserSet)Session["AgentSet"];
            }

            //setTaxCode();
            if (!IsPostBack)
            {
                //SavingProcess();
                using (profiler.Step("GBS:InitializeForm"))
                {
                    InitializeForm();
                }

                
            }
            else
            {
                ShowAddOnBreakDown();
            }
        }

        protected void setTaxCode()
        {
            List<BookingTaxFeesControl> listTaxFeesControl = new List<BookingTaxFeesControl>();
            listTaxFeesControl = objBooking.GetAllBK_TAXFEESCONTROL();
            if (listTaxFeesControl.Count > 0)
            {
                dtTaxFees = new DataTable();
                dtTaxFees.Columns.Add("TaxFeesCode");
                dtTaxFees.Columns.Add("TaxFeesGroup");
                dtTaxFees.Columns.Add("TaxFeesRate");

                int ctr = 0;

                foreach (BookingTaxFeesControl objtaxfees in listTaxFeesControl)
                {
                    DataRow row;
                    row = dtTaxFees.NewRow();
                    row["TaxFeesCode"] = objtaxfees.TaxFeesCode.ToString();
                    row["TaxFeesGroup"] = objtaxfees.TaxFeesGroup.ToString();
                    row["TaxFeesRate"] = objtaxfees.TaxFeesRate.ToString();
                    dtTaxFees.Rows.Add(row);
                    ctr += 1;
                }
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
                model.TemFlightJourneySellKey = foundRows[0]["TemFlightJourneySellKey"].ToString();
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
                if (IsNumeric(foundRows[0]["TemFlightFarePrice"].ToString()))
                { model.temFlightfarePrice = Convert.ToDecimal(foundRows[0]["TemFlightFarePrice"]); }
                if (IsNumeric(foundRows[0]["TemFlightAPT"].ToString()))
                { model.TemFlightApt = Convert.ToDecimal(foundRows[0]["TemFlightAPT"]); }
                if (IsNumeric(foundRows[0]["TemFlightFuel"].ToString()))
                { model.TemFlightFuel = Convert.ToDecimal(foundRows[0]["TemFlightFuel"]); }
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
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
        }

        protected void ValidatePopup(object sender, CallbackEventArgs e)
        {
            if (e.Parameter != null && e.Parameter.ToString().ToLower() == "back")
            {
                ASPxWebControl.RedirectOnCallback("SelectSeat.aspx");
                return;
            }
            MessageList msgList = new MessageList();
            ArrayList aMsgList = new ArrayList();
            string TransID = "";
            string PayScheme = "";
            string CurrencyCode = "";
            string CountryCode = "";
            string CarrierCode = "";
            List<BookingTransactionDetail> listBookingDetail = new List<BookingTransactionDetail>();
            List<BookingTransactionDetail> listBookingDetailCombine = new List<BookingTransactionDetail>();
            BookingTransactionMain bookingMain = new BookingTransactionMain();
            PaymentControl objPayment = new PaymentControl();
            PaymentInfo paymentInfo = new PaymentInfo();

            decimal CollectedAmount = 0;
            decimal ServiceChg = 0;
            decimal FullPrice = 0;
            decimal BaseFare = 0;
            decimal AmountDue = 0;

            DateTime PayDueDate1;
            decimal PayDueAmount1 = 0;
            DateTime PayDueDate2;
            decimal PayDueAmount2 = 0;
            DateTime PayDueDate3;
            decimal PayDueAmount3 = 0;

            DateTime PaymentDateEx1;
            decimal PaymentAmtEx1 = 0;
            DateTime PaymentDateEx2;
            decimal PaymentAmtEx2 = 0;
            DateTime PaymentDateEx3;
            decimal PaymentAmtEx3 = 0;

            int Quantity = 0;
            decimal FirstDeposit = 0;
            decimal CurrencyRate = 0;
            int TransTotalPax = 1;
            string AgentCountryCode = "";
            string GroupName = "";

            try
            {
                if (HttpContext.Current.Session["TempFlight"] != null)
                {
                    Hashtable ht = (Hashtable)HttpContext.Current.Session["HashMain"];
                    TransID = ht["TransID"].ToString();
                    bookingMain = objBooking.GetSingleBK_TRANSMAIN(TransID);
                    PayScheme = bookingMain.PayScheme;
                    CurrencyCode = bookingMain.Currency;
                    TransTotalPax = bookingMain.TransTotalPAX;
                }
                else if (HttpContext.Current.Session["TransMain"] != null)
                {
                    DataTable dtTransMain = objBooking.dtTransMain();
                    dtTransMain = (DataTable)HttpContext.Current.Session["TransMain"];
                    TransID = dtTransMain.Rows[0]["TransID"].ToString();
                    PayScheme = dtTransMain.Rows[0]["SchemeCode"].ToString();
                    CurrencyCode = dtTransMain.Rows[0]["Currency"].ToString();
                    //TransTotalPax = Convert.ToInt32(dtTransMain.Rows[0]["TransTotalPAX"].ToString());
                }

                //DataTable dtCountryInfo = objGeneral.GetCountryCodeByCurrency(CurrencyCode);
                //CountryCode = dtCountryInfo.Rows[0]["DefaultCurrencyCode"].ToString();
                if (Session["Country"] != null)
                    CountryCode = Session["Country"].ToString().Substring(0, 2);
                

                listBookingDetail = objBooking.GetAllBK_TRANSDTLFilterAll(TransID, 0);
                listBookingDetailCombine = objBooking.GetAllBK_TRANSDTLFlightGrp(TransID);

                //FullPrice = bookingMain.TransTotalAmt;
                CurrencyRate = bookingMain.ExchangeRate;

                 GroupName = objGeneral.getOPTGroupByCarrierCode(listBookingDetail[0].CarrierCode);
                if (Session["CountryCode"].ToString() != null)
                    AgentCountryCode = Session["CountryCode"].ToString();
                paymentInfo = objPayment.GetPaymentScheme(PayScheme, GroupName, TransID, CountryCode, CurrencyCode, AgentCountryCode);
                
                //CountryCode = listBookingDetail[0].Origin;

                int ind = 0;
                decimal FlightDuration = 0, TotalBaseFare = 0, TotalDeposit = 0, RemainDeposit = 0;
                decimal totMinDeposit = 0, totMaxDeposit = 0, minDeposit = 0, maxDeposit = 0, checkMin = 0, checkMax = 0, curMinDeposit = 0, curMaxDeposit = 0;
                string Origin = "", SellKey = "", Transit = "";
                DataTable dtLimit = new DataTable();
                TotalBaseFare = 0;
                foreach (BookingTransactionDetail bookDetail in listBookingDetailCombine)
                {
                    TotalBaseFare += bookDetail.LineFlight;
                    if (ind == 0)
                    {
                        Origin = bookDetail.Origin;
                        FlightDuration = bookDetail.FlightDuration;
                        SellKey = bookDetail.SellKey;
                        Transit = bookDetail.Transit;
                    }
                }
                if (paymentInfo.PaymentType == "DEPO")
                {
                    dtLimit = objGeneral.getDepositLimit(TransID, TotalBaseFare, TransTotalPax, CurrencyCode, Origin, GroupName, FlightDuration, SellKey, Transit);

                    //check with limit
                    if (dtLimit != null && dtLimit.Rows.Count > 0)
                    {
                        checkMin = Decimal.Parse(dtLimit.Rows[0]["CheckMin"].ToString());
                        minDeposit = Decimal.Parse(dtLimit.Rows[0]["MinDeposit"].ToString()) * TransTotalPax;
                        checkMax = Decimal.Parse(dtLimit.Rows[0]["CheckMax"].ToString());
                        maxDeposit = Decimal.Parse(dtLimit.Rows[0]["MaxDeposit"].ToString()) * TransTotalPax;
                        if (Decimal.Parse(dtLimit.Rows[0]["ValueType"].ToString()) == 0)
                            TotalDeposit = Math.Round(Decimal.Parse(dtLimit.Rows[0]["DepositValueOld"].ToString()) * TransTotalPax, 2);
                        else
                            TotalDeposit = Math.Round(Decimal.Parse(dtLimit.Rows[0]["DepositValue"].ToString()) * TotalBaseFare / 100, 2);

                        //TotalDeposit = Math.Round((TotalDeposit * paymentInfo.Percentage_1) / 100, 2);

                        if (checkMin == 1 || checkMax == 1)
                        {
                            if (checkMin == 1 && TotalDeposit < minDeposit)
                            {
                                TotalDeposit = minDeposit;
                            }
                            else if (checkMax == 1 && TotalDeposit > maxDeposit)
                            {
                                TotalDeposit = maxDeposit;
                            }
                        }
                        RemainDeposit = TotalDeposit;
                    }
                }

                BaseFare = 0;
                int cntIndex = 0;
                int totalpax = 0;
                foreach (BookingTransactionDetail bookDetail in listBookingDetailCombine)
                {
                    FullPrice = bookDetail.LineTotal;
                    BaseFare = bookDetail.LineFlight;
                    //totalpax = bookDetail.PaxAdult + bookDetail.PaxChild;
                    

                    int cnt = 0;
                    foreach (BookingTransactionDetail bkDetail in listBookingDetail)
                    {
                        if (bkDetail.Signature == bookDetail.Signature)
                        {
                            cnt += 1;
                        }
                    }

                    decimal paymentAttempt1 = 0;
                    decimal paymentAttempt2 = 0;
                    decimal paymentAttempt3 = 0;
                    decimal deposit = 0;
                    decimal remainamount = 0;
                    decimal maxDepositPNR = 0, minDepositPNR = 0;

                    CollectedAmount += bookDetail.CollectedAmount;
                    ServiceChg += bookDetail.LineFee;
                    Quantity += bookDetail.PaxAdult + bookDetail.PaxChild;

                    int iIndex = -1;
                    if (cnt >= 2)
                        iIndex = listBookingDetail.FindIndex(p => p.Signature == bookDetail.Signature && p.SeqNo % 2 == 1);
                    else
                        iIndex = listBookingDetail.FindIndex(p => p.Signature == bookDetail.Signature);

                    int iIndex2 = -1;
                    if (cnt >= 2) iIndex2 = listBookingDetail.FindIndex(p => p.Signature == bookDetail.Signature && p.SeqNo % 2 == 0);

                    PayDueDate1 = bookingMain.BookingDate;
                    PaymentDateEx1 = PayDueDate1;
                    bookingMain.PaymentDateEx1 = PayDueDate1;
                    if (iIndex >= 0) listBookingDetail[iIndex].PayDueDate1 = PayDueDate1;
                    if (iIndex2 >= 0) listBookingDetail[iIndex2].PayDueDate1 = PayDueDate1;


                    //if (paymentInfo.PaymentType == "SVCF")
                    //{
                    //    //FirstDeposit = paymentInfo.FirstDeposit;
                    //    //if (CurrencyRate > 0) FirstDeposit = FirstDeposit / CurrencyRate;
                    //    //FirstDeposit = FirstDeposit * Quantity;

                    //    PayDueAmount1 = ServiceChg;


                    //    paymentAttempt1 = Math.Round((FullPrice * paymentInfo.Percentage_1) / 100, 2);
                    //    if (paymentInfo.IsNominal_1 == 1)
                    //    {
                    //        if (deposit == 0)
                    //        {
                    //            //objBK_TRANSDTL_Info.Currency
                    //            deposit = objGeneral.getDepositByDuration(bookDetail.PaxAdult + bookDetail.PaxChild, bookDetail.Currency, bookDetail.Origin, GroupName, bookDetail.FlightDuration, bookDetail.SellKey, bookDetail.Transit);
                    //            //if (CurrencyRate > 0) deposit = deposit / CurrencyRate;
                    //        }
                    //        paymentAttempt1 = deposit + ServiceChg;
                    //        PayDueAmount2 = deposit;
                    //    }
                    //    else if (paymentInfo.Deposit_1 != 0)
                    //    {
                    //        if (deposit == 0)
                    //        {
                    //            deposit = objGeneral.getDeposit(bookDetail.TotalPax, bookDetail.Currency, bookDetail.Origin, bookDetail.Transit);
                    //        }
                    //        paymentAttempt1 = deposit + ServiceChg;
                    //        PayDueAmount2 = deposit;
                    //    }
                    //    else
                    //    {
                    //        PayDueAmount2 = paymentAttempt1 - ServiceChg;
                    //    }

                    //    if (paymentInfo.Code_1 == "DOB")
                    //    {
                    //        PayDueDate2 = bookingMain.BookingDate.AddHours(paymentInfo.Attempt_1);
                    //        PaymentDateEx2 = PayDueDate2;
                    //        bookingMain.PaymentDateEx2 = PayDueDate2;
                    //        if (iIndex >= 0 && PayDueAmount2 > 0) listBookingDetail[iIndex].PayDueDate2 = PayDueDate2;
                    //        if (iIndex2 >= 0 && PayDueAmount2 > 0) listBookingDetail[iIndex2].PayDueDate2 = PayDueDate2;
                    //    }
                    //    else if (paymentInfo.Code_1 == "STD")
                    //    {
                    //        PayDueDate2 = bookDetail.DepatureDate.AddHours(-paymentInfo.Attempt_1);
                    //        PaymentDateEx2 = PayDueDate2;
                    //        bookingMain.PaymentDateEx2 = PayDueDate2;
                    //        if (iIndex >= 0 && PayDueAmount2 > 0) listBookingDetail[iIndex].PayDueDate2 = PayDueDate2;
                    //        if (iIndex2 >= 0 && PayDueAmount2 > 0) listBookingDetail[iIndex2].PayDueDate2 = PayDueDate2;
                    //    }

                    //    PayDueAmount3 = bookDetail.LineTotal - PayDueAmount1 - PayDueAmount2;

                    //    if (paymentInfo.Code_2 == "DOB")
                    //    {
                    //        PayDueDate3 = bookingMain.BookingDate.AddHours(paymentInfo.Attempt_2);
                    //        PaymentDateEx3 = PayDueDate3;
                    //        bookingMain.PaymentDateEx3 = PayDueDate3;
                    //        if (iIndex >= 0 && PayDueAmount3 > 0) listBookingDetail[iIndex].PayDueDate3 = PayDueDate3;
                    //        if (iIndex2 >= 0 && PayDueAmount3 > 0) listBookingDetail[iIndex2].PayDueDate3 = PayDueDate3;
                    //    }
                    //    else if (paymentInfo.Code_2 == "STD")
                    //    {
                    //        PayDueDate3 = bookDetail.DepatureDate.AddHours(-paymentInfo.Attempt_2);
                    //        PaymentDateEx3 = PayDueDate3;
                    //        bookingMain.PaymentDateEx3 = PayDueDate3;
                    //        if (iIndex >= 0 && PayDueAmount3 > 0) listBookingDetail[iIndex].PayDueDate3 = PayDueDate3;
                    //        if (iIndex2 >= 0 && PayDueAmount3 > 0) listBookingDetail[iIndex2].PayDueDate3 = PayDueDate3;
                    //    }

                    //}
                    if (paymentInfo.PaymentType == "DEPO")
                    {
                        //FirstDeposit = paymentInfo.FirstDeposit;
                        //if (CurrencyRate > 0) FirstDeposit = FirstDeposit / CurrencyRate;
                        //FirstDeposit = FirstDeposit * Quantity;

                        deposit = objGeneral.getDepositByDuration(TransID, BaseFare, bookDetail.PaxAdult + bookDetail.PaxChild, bookDetail.Currency, bookDetail.Origin, GroupName, bookDetail.FlightDuration, bookDetail.SellKey, bookDetail.Transit);
                        //dtLimit = objGeneral.getDepositLimit(TransID, BaseFare, bookDetail.PaxAdult + bookDetail.PaxChild, bookDetail.Currency, bookDetail.Origin, GroupName, bookDetail.FlightDuration, bookDetail.SellKey, bookDetail.Transit);

                        ServiceChg = 0;
                        PayDueAmount1 = ServiceChg;


                        //paymentAttempt1 = Math.Round((deposit * paymentInfo.Percentage_1) / 100, 2);

                        if (checkMin == 1 || checkMax == 1)
                        {
                            minDepositPNR = Math.Round((minDeposit) * BaseFare / TotalBaseFare, 2);  //(bookDetail.PaxAdult + bookDetail.PaxChild) / TransTotalPax;
                            maxDepositPNR = Math.Round((maxDeposit) * BaseFare / TotalBaseFare, 2);  //(bookDetail.PaxAdult + bookDetail.PaxChild) / TransTotalPax;

                            curMinDeposit += minDepositPNR;
                            curMaxDeposit += maxDepositPNR;

                            if (curMinDeposit > minDeposit)
                            {
                                minDepositPNR = minDepositPNR - (curMinDeposit - minDeposit);
                                curMinDeposit = minDepositPNR;
                            }
                            if (curMaxDeposit > maxDeposit)
                            {
                                maxDepositPNR = maxDepositPNR - (curMaxDeposit - maxDeposit);
                                curMaxDeposit = maxDeposit;
                            }

                            if (checkMin == 1 && deposit < minDepositPNR)
                            {
                                remainamount = deposit - minDepositPNR;
                                deposit = minDepositPNR;
                            }
                            else if (checkMax == 1 && deposit > maxDepositPNR)
                            {
                                remainamount = deposit - maxDepositPNR;
                                deposit = maxDepositPNR;
                            }
                            if (cntIndex == listBookingDetailCombine.Count - 1)
                            {
                                if (deposit < RemainDeposit)
                                {
                                    remainamount = remainamount - (RemainDeposit - deposit);
                                    deposit = deposit + (RemainDeposit - deposit);
                                }
                                else if (deposit > RemainDeposit)
                                {
                                    remainamount = remainamount + (RemainDeposit - deposit);
                                    deposit = deposit - (RemainDeposit - deposit);
                                }
                            }
                        }

                        paymentAttempt1 = Math.Round((deposit * paymentInfo.Percentage_1) / 100, 2);
                       
                        //check with limit
                        //if (dtLimit != null && dtLimit.Rows.Count > 0)
                        //{
                        //    if (Decimal.Parse(dtLimit.Rows[0]["CheckMin"].ToString()) == 1 && paymentAttempt1 < Decimal.Parse(dtLimit.Rows[0]["MinDeposit"].ToString()))
                        //    {
                        //        remainamount = paymentAttempt1 - Decimal.Parse(dtLimit.Rows[0]["MinDeposit"].ToString());
                        //        paymentAttempt1 = Decimal.Parse(dtLimit.Rows[0]["MinDeposit"].ToString());
                        //    }
                        //    else if (Decimal.Parse(dtLimit.Rows[0]["CheckMax"].ToString()) == 1 && paymentAttempt1 > Decimal.Parse(dtLimit.Rows[0]["MaxDeposit"].ToString()))
                        //    {
                        //        remainamount = paymentAttempt1 - Decimal.Parse(dtLimit.Rows[0]["MaxDeposit"].ToString());
                        //        paymentAttempt1 = Decimal.Parse(dtLimit.Rows[0]["MaxDeposit"].ToString());
                        //    }
                        //}

                        if (paymentInfo.IsNominal_1 == 1)
                        {
                            //if (deposit == 0)
                            //{
                                //objBK_TRANSDTL_Info.Currency
                                deposit = objGeneral.getDepositByDuration(TransID, FullPrice, bookDetail.PaxAdult + bookDetail.PaxChild, bookDetail.Currency, bookDetail.Origin, GroupName, bookDetail.FlightDuration, bookDetail.SellKey, bookDetail.Transit);
                                //if (CurrencyRate > 0) deposit = deposit / CurrencyRate;
                            //}
                            paymentAttempt1 = deposit + ServiceChg;
                            //add validation if full price less than deposit, set to paid in full price, 20170207, by ketee
                            if (FullPrice <= deposit)
                                PayDueAmount1 = FullPrice;
                            else
                                PayDueAmount1 = deposit;
                        }
                        else if (paymentInfo.Deposit_1 != 0)
                        {
                            if (deposit == 0)
                            {
                                deposit = objGeneral.getDeposit(TransID, bookDetail.TotalPax, bookDetail.Currency, bookDetail.Origin, bookDetail.Transit);
                            }
                            paymentAttempt1 = deposit + ServiceChg;
                            //add validation if full price less than deposit, set to paid in full price, 20170207, by ketee
                            //PayDueAmount1 = deposit;
                            if (FullPrice <= deposit)
                                PayDueAmount1 = FullPrice;
                            else
                                PayDueAmount1 = deposit;
                        }
                        else
                        {
                            PayDueAmount1 = paymentAttempt1 - ServiceChg;
                        }


                        if (CurrencyCode == "IDR" || CurrencyCode == "INR")
                        {
                            PayDueAmount1 = Math.Round(PayDueAmount1);
                            //PayDueAmount2 = Math.Round(PayDueAmount2);
                            //PayDueAmount3 = Math.Round(PayDueAmount3);
                        }

                        if (paymentInfo.Code_1 == "DOB")
                        {
                            PayDueDate1 = bookingMain.BookingDate.AddHours(paymentInfo.Attempt_1);
                            PaymentDateEx1 = PayDueDate1;
                            bookingMain.PaymentDateEx1 = PayDueDate1;
                            if (iIndex >= 0 && PayDueAmount1 > 0) listBookingDetail[iIndex].PayDueDate1 = PayDueDate1;
                            if (iIndex2 >= 0 && PayDueAmount1 > 0) listBookingDetail[iIndex2].PayDueDate1 = PayDueDate1;
                        }
                        else if (paymentInfo.Code_1 == "STD")
                        {
                            PayDueDate1 = bookDetail.DepatureDate.AddHours(-paymentInfo.Attempt_1);
                            PaymentDateEx1 = PayDueDate1;
                            bookingMain.PaymentDateEx1 = PayDueDate1;
                            if (iIndex >= 0 && PayDueAmount1 > 0) listBookingDetail[iIndex].PayDueDate1 = PayDueDate1;
                            if (iIndex2 >= 0 && PayDueAmount1 > 0) listBookingDetail[iIndex2].PayDueDate1 = PayDueDate1;
                        }

                        //PayDueAmount2 = bookDetail.LineTotal - PayDueAmount1;
                        if (FullPrice > (PayDueAmount1) && paymentInfo.Percentage_2 == 0)
                        {
                            paymentAttempt2 = FullPrice - (PayDueAmount1);
                            PayDueAmount2 = paymentAttempt2;
                        }
                        else
                        {
                            //paymentAttempt2 = Math.Round((deposit * paymentInfo.Percentage_2) / 100, 2) + remainamount;
                            paymentAttempt2 = Math.Round((deposit * paymentInfo.Percentage_2) / 100, 2);
                            PayDueAmount2 = paymentAttempt2;
                        }

                        if (CurrencyCode == "IDR" || CurrencyCode == "INR")
                        {
                            //PayDueAmount1 = Math.Round(PayDueAmount1);
                            PayDueAmount2 = Math.Round(PayDueAmount2);
                            //PayDueAmount3 = Math.Round(PayDueAmount3);
                        }

                        if (paymentInfo.Code_2 == "DOB")
                        {
                            PayDueDate2 = bookingMain.BookingDate.AddHours(paymentInfo.Attempt_2);
                            PaymentDateEx2 = PayDueDate2;
                            bookingMain.PaymentDateEx2 = PayDueDate2;
                            if (iIndex >= 0 && PayDueAmount2 > 0) listBookingDetail[iIndex].PayDueDate2 = PayDueDate2;
                            if (iIndex2 >= 0 && PayDueAmount2 > 0) listBookingDetail[iIndex2].PayDueDate2 = PayDueDate2;
                        }
                        else if (paymentInfo.Code_2 == "STD")
                        {
                            PayDueDate2 = bookDetail.DepatureDate.AddHours(-paymentInfo.Attempt_2);
                            PaymentDateEx2 = PayDueDate2;
                            bookingMain.PaymentDateEx2 = PayDueDate2;
                            if (iIndex >= 0 && PayDueAmount2 > 0) listBookingDetail[iIndex].PayDueDate2 = PayDueDate2;
                            if (iIndex2 >= 0 && PayDueAmount2 > 0) listBookingDetail[iIndex2].PayDueDate2 = PayDueDate2;
                        }

                        if (FullPrice > (PayDueAmount1 + PayDueAmount2) && paymentInfo.Percentage_3 == 0)
                        {
                            paymentAttempt3 = FullPrice - (PayDueAmount1 + PayDueAmount2);
                            PayDueAmount3 = paymentAttempt3;
                        }
                        else
                        {
                            paymentAttempt3 = Math.Round((deposit * paymentInfo.Percentage_3) / 100, 2);
                            PayDueAmount3 = paymentAttempt3;
                        }

                        if (paymentInfo.Code_3 == "DOB")
                        {
                            PayDueDate3 = bookingMain.BookingDate.AddHours(paymentInfo.Attempt_3);
                            PaymentDateEx3 = PayDueDate3;
                            bookingMain.PaymentDateEx3 = PayDueDate3;
                            if (iIndex >= 0 && PayDueAmount3 > 0) listBookingDetail[iIndex].PayDueDate3 = PayDueDate3;
                            if (iIndex2 >= 0 && PayDueAmount3 > 0) listBookingDetail[iIndex2].PayDueDate3 = PayDueDate3;
                        }
                        else if (paymentInfo.Code_3 == "STD")
                        {
                            PayDueDate3 = bookDetail.DepatureDate.AddHours(-paymentInfo.Attempt_3);
                            PaymentDateEx3 = PayDueDate3;
                            bookingMain.PaymentDateEx3 = PayDueDate3;
                            if (iIndex >= 0 && PayDueAmount3 > 0) listBookingDetail[iIndex].PayDueDate3 = PayDueDate3;
                            if (iIndex2 >= 0 && PayDueAmount3 > 0) listBookingDetail[iIndex2].PayDueDate3 = PayDueDate3;
                        }
                    }
                    else
                    {
                        PayDueAmount1 = FullPrice;
                    }

                    //if (CurrencyCode == "IDR" || CurrencyCode == "INR")
                    //{
                    //    PayDueAmount1 = Math.Round(PayDueAmount1);
                    //    PayDueAmount2 = Math.Round(PayDueAmount2);
                    //    PayDueAmount3 = Math.Round(PayDueAmount3);
                    //}

                    if (PayDueAmount1 > 0) PaymentAmtEx1 += PayDueAmount1;
                    if (PayDueAmount2 > 0) PaymentAmtEx2 += PayDueAmount2;
                    if (PayDueAmount3 > 0) PaymentAmtEx3 += PayDueAmount3;

                    bookingMain.PaymentAmtEx1 = PaymentAmtEx1;
                    bookingMain.PaymentAmtEx2 = PaymentAmtEx2;
                    bookingMain.PaymentAmtEx3 = PaymentAmtEx3;

                    if (iIndex >= 0 && PayDueAmount1 > 0) listBookingDetail[iIndex].PayDueAmount1 = PayDueAmount1;
                    if (iIndex2 >= 0 && PayDueAmount1 > 0) listBookingDetail[iIndex2].PayDueAmount1 = PayDueAmount1;
                    if (iIndex >= 0 && PayDueAmount2 > 0) listBookingDetail[iIndex].PayDueAmount2 = PayDueAmount2;
                    if (iIndex2 >= 0 && PayDueAmount2 > 0) listBookingDetail[iIndex2].PayDueAmount2 = PayDueAmount2;
                    if (iIndex >= 0 && PayDueAmount3 > 0) listBookingDetail[iIndex].PayDueAmount3 = PayDueAmount3;
                    if (iIndex2 >= 0 && PayDueAmount3 > 0) listBookingDetail[iIndex2].PayDueAmount3 = PayDueAmount3;

                    RemainDeposit = RemainDeposit - deposit;
                    cntIndex += 1;
                }

                objBooking.SaveHeaderDetail(bookingMain, listBookingDetail, CoreBase.EnumSaveType.Update);
                //Session["CountryCode"] = CountryCode;
                #region prev
                //ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
                //ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight2 = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();

                //string strExpr;
                //string strSort;
                //DataTable dt = new DataTable();
                //strExpr = "TemFlightId = '" + departID + "'";
                //strSort = "";

                //dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                //DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                //FillModelFromDataRow(foundRows, ref  temFlight);

                //if (ReturnID != "")
                //{
                //    strExpr = "TemFlightId = '" + ReturnID + "'";
                //    strSort = "";

                //    dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                //    foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                //    FillModelFromDataRow(foundRows, ref  temFlight2);

                //    //string LoginType = MyUserSet.AgentType.ToString();  //Session["LoginType"].ToString();
                //    string LoginType = "PublicAgent";
                //    string LoginName = MyUserSet.AgentName;
                //    string LoginPWD = "";
                //    string LoginDomain = "";
                //    /* remark to ag payment process
                //    if (LoginType == "SkyAgent")
                //    {
                //        LoginPWD = Session["LoginPWD"].ToString();
                //        LoginDomain = Session["LoginDomain"].ToString();
                //    }*/
                //    //objBooking.SellFlightByTem(temFlight, temFlight2, LoginType, LoginName, LoginPWD, LoginDomain, "");
                //    objBooking.SellJourney(temFlight, temFlight2, LoginType, LoginName, LoginPWD, LoginDomain, "");
                //}
                //else
                //{                    
                //    //string LoginType = MyUserSet.AgentType.ToString();
                //    string LoginType = "PublicAgent";
                //    string LoginName = MyUserSet.AgentName;
                //    string LoginPWD = "";
                //    string LoginDomain = "";
                //    /* remark to ag payment process
                //    if (LoginType == "SkyAgent")
                //    {
                //        LoginPWD = Session["LoginPWD"].ToString();
                //        LoginDomain = Session["LoginDomain"].ToString();
                //    }*/
                //    objBooking.SellFlightByTem(temFlight, LoginType, LoginName, LoginPWD, LoginDomain, "");

                //}

                //if (SaveData())
                //    e.Result = "";
                //else
                //    e.Result = msgList.Err100031;
                #endregion

                e.Result = "";
            }
            catch (Exception ex)
            {
                //ex.Message = ex.Message + " : custom message: payScheme-" + PayScheme + ", GroupName-" + GroupName + ", TransID-" + TransID + "CountryCode-" + CountryCode + "CurrencyCode-" + CurrencyCode + "AgentCountryCode-" + AgentCountryCode
                SystemLog.Notifier.Notify(ex);
                e.Result = msgList.Err100013;
            }
        }

        protected static bool IsNumeric(string s)
        {
            double result;
            return s != null && Double.TryParse(s, out result);
        }

        #region prevsavedata
        //protected bool SaveData()
        //{

        //    ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();

        //    string strExpr;
        //    string strSort;
        //    string keyCarrier = "";
        //    decimal totalOth = 0; //service charge total
        //    DataTable dt = new DataTable();
        //    Hashtable ht = new Hashtable();

        //    strExpr = "TemFlightId = '" + departID + "'";
        //    strSort = "";
        //    DateTime departDate;
        //    dt = (DataTable)HttpContext.Current.Session["TempFlight"];
        //    DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
        //    FillModelFromDataRow(foundRows, ref  temFlight);

        //    departDate = Convert.ToDateTime(temFlight.TemFlightStd);

        //    Currency = temFlight.TemFlightCurrencyCode.Trim();

        //    if (IsNumeric(lbl_TotalAmount.Text))
        //    {
        //        TotalMoney = Convert.ToDecimal(lbl_TotalAmount.Text);
        //        TotalAmount = Convert.ToDecimal(lbl_TotalAmount.Text);
        //    }

        //    agent = objAgentProfile.GetSingleAgentProfile(MyUserSet.AgentName);

        //    string LoginType = MyUserSet.AgentType.ToString();

        //    int m = 0;
        //    int count = 0;
        //    DataTable dtClass = objBooking.dtClass();
        //    if (HttpContext.Current.Session["dataClass"] != null) { dtClass = (DataTable)HttpContext.Current.Session["dataClass"]; }
        //    count = dtClass.Rows.Count;

        //    byte seqNo = 1;
        //    List<BookingTransactionMain> TransMain = new List<BookingTransactionMain>();

        //    tranID = DateTime.Now.ToString("yyyyMMddHHmmsss");

        //    #region newsavedetail
        //    //Datatable Process 

        //    //string PNR = book.BookingCommit(agent, temClass.TemClassPersonNumber, temClass.TemClassClassPrice.Value, temClass.TemClassSellSignature);

        //    DataTable dataClass = objBooking.dtClass();
        //    dataClass = (DataTable)HttpContext.Current.Session["dataClass"];
        //    foreach (DataRow dr in dataClass.Rows)
        //    {
        //        bookDTLInfo = new BookingTransactionDetail();
        //        string PNR = seqNo.ToString();
        //        bookDTLInfo.RecordLocator = PNR;
        //        bookDTLInfo.TransID = tranID;
        //        bookDTLInfo.SeqNo = seqNo;

        //        if (seqNo == 1)
        //        {
        //            keyCarrier = bookDTLInfo.CarrierCode = dr["CarrierCode"].ToString();
        //            ht.Add("keyCarrier", keyCarrier);
        //        }

        //        //service charge pax
        //        decimal svcCharge = Convert.ToDecimal(objGeneral.getSysValueByKeyAndCarrierCode("SVCCHARGEPAX", keyCarrier)) / 100;
        //        bookDTLInfo.LineOth = svcCharge * Convert.ToDecimal(dr["FullPrice"].ToString());
        //        totalOth += bookDTLInfo.LineOth;

        //        seqNo += 1;
        //        bookDTLInfo.Currency = Currency;
        //        bookDTLInfo.CarrierCode = dr["CarrierCode"].ToString();
        //        bookDTLInfo.FlightNo = dr["FlightNumber"].ToString();
        //        bookDTLInfo.DepatureDate = Convert.ToDateTime(dr["DepartureDate"].ToString());
        //        bookDTLInfo.ArrivalDate = Convert.ToDateTime(dr["ArrivalDate"].ToString());
        //        bookDTLInfo.Origin = dr["Origin"].ToString();
        //        bookDTLInfo.Destination = dr["Destination"].ToString();
        //        bookDTLInfo.LineFee = Convert.ToDecimal(dr["FuelChrg"].ToString());
        //        bookDTLInfo.FareClass = dr["FareClass"].ToString();
        //        bookDTLInfo.PaxAdult = Convert.ToInt16(dr["AdultPax"].ToString());
        //        bookDTLInfo.PaxChild = Convert.ToInt16(dr["ChildPax"].ToString());
        //        bookDTLInfo.LineTotal = Convert.ToDecimal(dr["FullPrice"].ToString());
        //        bookDTLInfo.SyncLastUpd = DateTime.Now;
        //        bookDTLInfo.LastSyncBy = MyUserSet.AgentID;
        //        bookDTLInfo.FarePerPax = Convert.ToDecimal(dr["FarePrice"].ToString());
        //        bookDTLInfo.LineTax = Convert.ToDecimal(dr["TaxChrg"].ToString());                
        //        //totalFlightFare += bookDTLInfo.LineTotal;
        //        totalFlightFare += bookDTLInfo.LineTotal + bookDTLInfo.LineOth; //include service charge

        //        bookDTLInfo.TransVoid = 0;
        //        bookDTLInfo.CreateBy = MyUserSet.AgentID;
        //        bookDTLInfo.SyncCreate = DateTime.Now;

        //        bookDTLInfo.Transit = dr["TemClassTransit"].ToString();
        //        if (bookDTLInfo.Transit != "")
        //        {
        //            bookDTLInfo.ArrivalDate2 = Convert.ToDateTime(dr["TemClassSta2"].ToString());
        //            bookDTLInfo.DepatureDate2 = Convert.ToDateTime(dr["TemClassStd2"].ToString());
        //        }

        //        bookDTLInfo.CollectedAmount = 0;
        //        bookDTLInfo.Signature = dr["SellSignature"].ToString();

        //        // objBooking.SaveBK_TRANSDTL(bookDTLInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
        //        lstbookDTLInfo.Add(bookDTLInfo);

        //        APT += bookDTLInfo.LineTax;
        //    }
        //    // end datatable
        //    #endregion

        //        //save booking header

        //        bookHDRInfo.TransID = bookDTLInfo.TransID;

        //        ht.Add("TransID", bookHDRInfo.TransID);                               

        //        bookHDRInfo.TransType = 0;
        //        bookHDRInfo.AgentID = MyUserSet.AgentID;
        //        bookHDRInfo.AgentCatgID = MyUserSet.AgentCategoryID;
        //        bookHDRInfo.BookingDate = DateTime.Now;
        //        bookHDRInfo.STDDate = Convert.ToDateTime(temFlight.TemFlightStd);

        //        string expirySetting = objGeneral.getSysValueByKeyAndCarrierCode("STDEXPIRY", keyCarrier);                

        //        int sysValue = 0;
        //        if (expirySetting != "")
        //        {
        //            sysValue = Convert.ToInt16(expirySetting);
        //        }

        //        bookHDRInfo.ExpiryDate = departDate.Date.AddDays(-sysValue);

        //        ht.Add("Expiry", bookHDRInfo.ExpiryDate);

        //        bookHDRInfo.TransTotalPAX = Convert.ToInt16(lbl_num.Text);
        //        bookHDRInfo.CollectedAmt = 0; 

        //        bookHDRInfo.TransTotalAmt = totalFlightFare;
        //        bookHDRInfo.TransSubTotal = totalFlightFare;
        //        bookHDRInfo.TransTotalTax = APT;
        //        bookHDRInfo.TransTotalFee = totalFUEFare;
        //        bookHDRInfo.TransTotalOth = totalOth;                
        //        bookHDRInfo.Currency = Currency;
        //        bookHDRInfo.CurrencyPaid = Currency;

        //        bookHDRInfo.TransStatus = 0;
        //        bookHDRInfo.CreateBy = MyUserSet.AgentID;
        //        bookHDRInfo.SyncCreate = DateTime.Now;
        //        bookHDRInfo.SyncLastUpd = DateTime.Now;
        //        bookHDRInfo.LastSyncBy = MyUserSet.AgentID;
        //        bookHDRInfo.TotalAmtAVG = Convert.ToDecimal(lblAverageFare.Text);
        //        bookHDRInfo.TotalAmtGoing = Convert.ToDecimal(lbl_Total.Text);
        //        if (lbl_InTotal.Text != "")
        //        { bookHDRInfo.TotalAmtReturn = Convert.ToDecimal(lbl_InTotal.Text); }
        //        else
        //        { bookHDRInfo.TotalAmtReturn = 0; }


        //        string reminder = objGeneral.getSysValueByKeyAndCarrierCode("REMINDDURA1", keyCarrier);                

        //        if (reminder != "")
        //        {
        //            sysValue = Convert.ToInt16(reminder);
        //        }
        //        bookHDRInfo.CurReminderDate = bookHDRInfo.ExpiryDate.AddDays(-sysValue);

        //        reminder = objGeneral.getSysValueByKeyAndCarrierCode("REMINDDURA2", keyCarrier);                

        //        if (reminder != "")
        //        {
        //            sysValue = Convert.ToInt16(reminder);
        //        }
        //        bookHDRInfo.NextReminderDate = bookHDRInfo.ExpiryDate.AddDays(-sysValue);
        //        bookHDRInfo.ReminderType = 1;

        //        //load max failed payment try
        //        string maxPaymentFail = objGeneral.getSysValueByKeyAndCarrierCode("PAYMENTSUSPEND", keyCarrier);
        //        ht.Add("PaymentSuspend", maxPaymentFail);

        //        HttpContext.Current.Session.Remove("HashMain");
        //        HttpContext.Current.Session.Add("HashMain", ht);

        //        //end save header

        //        //save APTtax into TransTax

        //        /*    
        //        string tCode = "";
        //        decimal tAmount = 0;

        //        for (int i = 0; i < dtTaxFees.Rows.Count; i++)
        //        {
        //            if (dtTaxFees.Rows[i]["TaxFeesGroup"].ToString() == "APT")
        //            {
        //                tCode = dtTaxFees.Rows[i]["TaxFeesCode"].ToString();
        //                tAmount = Convert.ToDecimal(dtTaxFees.Rows[i]["TaxFeesRate"]);
        //            }
        //        }

        //        transTaxInfo.TransID = bookHDRInfo.TransID;

        //        transTaxInfo.TaxCode = Convert.ToInt16(tCode);
        //        transTaxInfo.TaxRate = tAmount;
        //        transTaxInfo.TaxAmt = APT;
        //        transTaxInfo.TransVoid = 0;
        //        transTaxInfo.SyncCreate = DateTime.Now;
        //        transTaxInfo.CreateBy = MyUserSet.AgentID;

        //        //save service charge, fuel charge and baggage charge into transFees

        //        transFeesInfo.TransID = bookHDRInfo.TransID;
        //        transFeesInfo.Transvoid = 0;
        //        transFeesInfo.SyncCreate = DateTime.Now;
        //        transFeesInfo.CreateBy = MyUserSet.AgentID;
        //        for (int i = 0; i < dtTaxFees.Rows.Count; i++)
        //        {
        //            if (dtTaxFees.Rows[i]["TaxFeesGroup"].ToString() != "APT")
        //            {
        //                transFeesInfo.FeesCode = Convert.ToInt16(dtTaxFees.Rows[i]["TaxFeesCode"]);
        //                transFeesInfo.FeesRate = Convert.ToDecimal(dtTaxFees.Rows[i]["TaxFeesRate"]);
        //                if (dtTaxFees.Rows[i]["TaxFeesGroup"].ToString() == "SVC")
        //                {
        //                    transFeesInfo.FeesAmt = TotalMoney * (Convert.ToDecimal(dtTaxFees.Rows[i]["TaxFeesRate"]) / 100);
        //                }
        //                else
        //                    if (dtTaxFees.Rows[i]["TaxFeesGroup"].ToString() == "FUE")
        //                    {
        //                        transFeesInfo.FeesAmt = totalFUEFare;
        //                    }
        //                    else
        //                        if (dtTaxFees.Rows[i]["TaxFeesGroup"].ToString() == "BGG")
        //                        {
        //                            transFeesInfo.FeesAmt = totalBaggageFare;
        //                        }

        //                //objBooking.SaveBK_TRANSFEES(transFeesInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
        //                lsttransFeesInfo.Add(transFeesInfo);
        //            }
        //        }
        //        */

        //        //end save tax and charge
        //        //added by ketee
        //        BookingTransactionMain BookingMain = new BookingTransactionMain();
        //        //BookingMain = objBooking.SaveBooking(bookHDRInfo, lstbookDTLInfo, transTaxInfo, lsttransFeesInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
        //        BookingMain = objBooking.SaveBooking(bookHDRInfo, lstbookDTLInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
        //        if (BookingMain != null && BookingMain.TransID != "")
        //            return true;
        //        else
        //            return false;
        //        //objBooking.SaveBooking(bookHDRInfo, lstbookDTLInfo, transTaxInfo, lsttransFeesInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
        //}
        #endregion

        protected void InitializeForm()
        {
            var profiler = MiniProfiler.Current;
            HttpCookie cookie3 = Request.Cookies["cookieTran"];
            if (cookie3 != null)
            {
                cookie3.Expires = DateTime.Today.AddDays(-1);
                Response.Cookies.Add(cookie3);
            }
            using (profiler.Step("GBS:SetCookie"))
            {
                SetCookie();
            }
            using (profiler.Step("GBS:Bind"))
            {
                Bind();
            }
            using (profiler.Step("GBS:ClearSessionData"))
            {
                objBooking.ClearSessionData();
            }
        }

        private void SetCookie()
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
        }

        private void ShowAddOnBreakDown()
        {
            DataTable dt = new DataTable();
            dt = (DataTable)HttpContext.Current.Session["TempFlight"];

            DataTable dtDepart = dt;
            DataTable dtReturn = dt;
            string strExprDepart;
            string strExprReturn;
            string strSort;
            strExprDepart = "TemFlightId = '" + departID + "'";
            strSort = "";
            DataRow[] foundRowsDepart = dtDepart.Select(strExprDepart, strSort, DataViewRowState.Added);
            DataRow[] foundRowsReturn;


            if (Session["TransID"] != null)
            {
                TransID = (string)Session["TransID"];
            }

            if (ReturnID != "")
            {
                returnFlight = true;
                strExprReturn = "TemFlightID = '" + ReturnID + "'";
                foundRowsReturn = dtReturn.Select(strExprReturn, strSort, DataViewRowState.Added);
            }
            else
            {
                returnFlight = false;
                strExprReturn = "";
                foundRowsReturn = dtReturn.Select(strExprReturn, strSort, DataViewRowState.Added);
            }
            Session["IsReturnFlight"] = returnFlight;

            //change to new add-On table, Tyas
            //dtAddOn = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTableReviewBooking(TransID, false, "", returnFlight);
            dtAddOn = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTableNewManage(TransID, true, "");
            if (dt != null)
            {
                if (dtAddOn != null)
                {
                    if (foundRowsDepart[0]["TemFlightTransit"].ToString().Trim() != "")
                    {
                        gvAddOnDepart.Columns["ConDepartMeal"].Visible = true;
                    }
                    else
                    {
                        gvAddOnDepart.Columns["ConDepartMeal"].Visible = false;
                    }

                    gvAddOnDepart.DataSource = dtAddOn;
                    gvAddOnDepart.DataBind();
                    gvAddOnDepart.ExpandAll();


                    if (Session["IsReturnFlight"] != null && (Boolean)Session["IsReturnFlight"] != false && foundRowsReturn[0]["TemFlightTransit"].ToString().Trim() != "")
                    {
                        gvAddOnReturn.Columns["ConReturnMeal"].Visible = true;
                    }
                    else
                    {
                        gvAddOnReturn.Columns["ConReturnMeal"].Visible = false;
                    }

                    gvAddOnReturn.DataSource = dtAddOn;
                    gvAddOnReturn.DataBind();
                    gvAddOnReturn.ExpandAll();
                }
            }
        }

        private void Bind()
        {
            var profiler = MiniProfiler.Current;
            //20170425 - Sienny (get transid)
            if (Session["TransID"] != null)
            {
                TransID = (string)Session["TransID"];
            }

            if (departID != -1)
            {
                ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
                ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight2 = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
                string strExpr;
                string strSort;
                DataTable dt = objBooking.dtFlight();
                strExpr = "TemFlightId = '" + departID + "'";
                strSort = "";
                dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                using (profiler.Step("GBS:FillModelFromDataRow"))
                {
                    FillModelFromDataRow(foundRows, ref temFlight);
                }
                //Added by Tyas to get from dataclasstrans
                DataTable dataClass = objBooking.dtClass();
                dataClass = (DataTable)HttpContext.Current.Session["dataClassTrans"];

                //lbl_Arrival.Text = objGeneral.GetCityNameByCode(temFlight.TemFlightArrival) + "(" + temFlight.TemFlightArrival + ")";

                lbl_Average.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight.temFlightfarePrice)).ToString("N", nfi);

                num = Convert.ToInt32(temFlight.TemFlightPaxNum.ToString()); //added by diana 20141004, replace cookie

                //commented by diana 20131104
                //if (Convert.ToDecimal(temFlight.TemFlightCHDNum) != 0)
                //    lbl_Average.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight.temFlightfarePrice)/2).ToString("N", nfi);
                //else
                //    lbl_Average.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight.temFlightfarePrice)).ToString("N", nfi);

                lbl_currency.Text = temFlight.TemFlightCurrencyCode.ToString();
                lbl_currency0.Text = temFlight.TemFlightCurrencyCode.ToString();
                lbl_currency1.Text = temFlight.TemFlightCurrencyCode.ToString();
                lbl_currency2.Text = temFlight.TemFlightCurrencyCode.ToString();
                lbl_currency21.Text = temFlight.TemFlightCurrencyCode.ToString();
                lbl_currency3.Text = temFlight.TemFlightCurrencyCode.ToString();
                lbl_currency31.Text = temFlight.TemFlightCurrencyCode.ToString();
                if (Convert.ToDecimal(temFlight.TemFlightCHDNum) != 0) // && (temFlight.TemFlightDeparture.ToString().ToUpper() == "HKG" || temFlight.TemFlightArrival.ToString().ToUpper() == "HKG"))
                {
                    lbl_currency2CHD.Text = temFlight.TemFlightCurrencyCode.ToString();
                    lbl_currency3CHD.Text = temFlight.TemFlightCurrencyCode.ToString();
                }
                lbl_currency4.Text = temFlight.TemFlightCurrencyCode.ToString();
                lbl_currency5.Text = temFlight.TemFlightCurrencyCode.ToString();
                lbl_currency6.Text = temFlight.TemFlightCurrencyCode.ToString();
                lbl_currency7.Text = temFlight.TemFlightCurrencyCode.ToString();
                lblCurrFuelDepart.Text = temFlight.TemFlightCurrencyCode.ToString();
                lblCurrOthDepart.Text = temFlight.TemFlightCurrencyCode.ToString();
                lblCurrSvcDepart.Text = temFlight.TemFlightCurrencyCode.ToString();
                lblCurrBaggageDepart.Text = temFlight.TemFlightCurrencyCode.ToString();
                lblCurrMealDepart.Text = temFlight.TemFlightCurrencyCode.ToString();
                lblCurrSportDepart.Text = temFlight.TemFlightCurrencyCode.ToString();
                lblCurrComfortDepart.Text = temFlight.TemFlightCurrencyCode.ToString();

                lblCurrPromoDiscDepart.Text = temFlight.TemFlightCurrencyCode.ToString();
                lbl_currency14.Text = temFlight.TemFlightCurrencyCode.ToString();

                //added by ketee,
                lblCurrSeatDepart.Text = temFlight.TemFlightCurrencyCode.ToString();
                lblCurrSeatReturn.Text = temFlight.TemFlightCurrencyCode.ToString();

                lbl_currency2.Text = temFlight.TemFlightCurrencyCode;
                lbl_currency21.Text = temFlight.TemFlightCurrencyCode;
                lblCurrAVLDepart.Text = temFlight.TemFlightCurrencyCode;
                lblCurrPSFDepart.Text = temFlight.TemFlightCurrencyCode;
                lblCurrSCFDepart.Text = temFlight.TemFlightCurrencyCode;
                lblCurrConnectingDepart.Text = temFlight.TemFlightCurrencyCode;
                lblCurrDiscDepart.Text = temFlight.TemFlightCurrencyCode;
                lblCurrKlia2Depart.Text = temFlight.TemFlightCurrencyCode;
                lblCurrGSTDepart.Text = temFlight.TemFlightCurrencyCode;
                lblCurrSPLDepart.Text = temFlight.TemFlightCurrencyCode;
                lblCurrAPSDepart.Text = temFlight.TemFlightCurrencyCode;
                lblCurrIADFDepart.Text = temFlight.TemFlightCurrencyCode;
                lblCurrACFDepart.Text = temFlight.TemFlightCurrencyCode;
                lblCurrCSTDepart.Text = temFlight.TemFlightCurrencyCode;
                lblCurrCUTDepart.Text = temFlight.TemFlightCurrencyCode;
                lblCurrSGIDepart.Text = temFlight.TemFlightCurrencyCode;
                lblCurrSSTDepart.Text = temFlight.TemFlightCurrencyCode;
                lblCurrUDFDepart.Text = temFlight.TemFlightCurrencyCode;
                lblCurrASCDepart.Text = temFlight.TemFlightCurrencyCode;
                lblCurrBCLDepart.Text = temFlight.TemFlightCurrencyCode;
                lblCurrIWJRDepart.Text = temFlight.TemFlightCurrencyCode;
                lblCurrVATChargeDepart.Text = temFlight.TemFlightCurrencyCode;
                //lbl_departure.Text = objGeneral.GetCityNameByCode(temFlight.TemFlightDeparture) + "(" + temFlight.TemFlightDeparture + ")";

                decimal totalApt = Convert.ToDecimal(temFlight.TemFlightApt);

                lbl_FlightTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight.temFlightfarePrice) * num).ToString("N", nfi);

                //commented by diana 20131104
                //if (Convert.ToDecimal(temFlight.TemFlightCHDNum) != 0)
                //    lbl_FlightTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight.temFlightfarePrice) * num / 2).ToString("N", nfi);
                //else
                //    lbl_FlightTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight.temFlightfarePrice) * num).ToString("N", nfi);
                HttpCookie cookie2 = Request.Cookies["cookieSearchcondition"];
                if (cookie2 != null)
                {
                    if (cookie2.Values["InfantNum"] != null && Convert.ToInt16(cookie2.Values["InfantNum"]) != 0)
                    {
                        lblTotInfant.Text = cookie2.Values["InfantNum"];

                    }
                    else
                    {
                        trInfantTotal.Style.Add("display", "none");
                    }
                }
                lbl_num.Text = num.ToString();
                lbl_num1.Text = num.ToString();
                //lbl_num21.Text = num.ToString();
                if (Convert.ToDecimal(temFlight.TemFlightCHDNum) != 0) // && (temFlight.TemFlightDeparture.ToString().ToUpper() == "HKG" || temFlight.TemFlightArrival.ToString().ToUpper() == "HKG"))
                {
                    lbl_num2.Text = "Adult Airport Tax ";
                    lbl_num2CHD.Text = temFlight.TemFlightCHDNum.ToString();
                    trChildAirportDepart.Visible = true;
                    trChildAirportTaxReturn.Visible = true;

                }
                else
                {
                    lbl_num2.Text = "Airport Tax ";
                    trChildAirportDepart.Visible = false;
                    trChildAirportTaxReturn.Visible = false;
                }
                lbl_num3.Text = num.ToString();
                lbl_num4.Text = num.ToString();
                lbl_num5.Text = num.ToString();
                lbl_num6.Text = num.ToString();
                //lbl_numtax.Text = num.ToString();

                //lbl_taxPrice.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight.TemFlightApt)).ToString("N", nfi);


                lbl_Total.Text = objGeneral.RoundUp(temFlight.TemFlightTotalAmount).ToString("N", nfi);
                decimal total = temFlight.TemFlightTotalAmount;

                AVGFare = (objGeneral.RoundUp(total / temFlight.TemFlightPaxNum));

                DataTable dtBDFee = objBooking.dtBreakdownFee();
                dtBDFee = (DataTable)HttpContext.Current.Session["dataBDFeeDepart"];

                //lbl_taxTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight.TemFlightApt)).ToString("N", nfi);
                if (Convert.ToDecimal(temFlight.TemFlightCHDNum) != 0) // && (temFlight.TemFlightDeparture.ToString().ToUpper() == "HKG" || temFlight.TemFlightArrival.ToString().ToUpper() == "HKG"))
                {
                    //trInAptCHD.Visible = true;
                    //lbl_taxPrice.Text = Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]).ToString("N", nfi);
                    lbl_taxTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * Convert.ToDecimal(temFlight.TemFlightADTNum)).ToString("N", nfi);
                    //lbl_IntaxPriceCHD.Text = Convert.ToDecimal(dtBDFee.Rows[0]["ChApt"]).ToString("N", nfi);
                    lbl_taxTotalCHD.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["ChApt"]) * Convert.ToDecimal(temFlight.TemFlightCHDNum)).ToString("N", nfi);
                }
                else
                {
                    //trInAptCHD.Visible = false;
                    lbl_IntaxPrice.Text = Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]).ToString("N", nfi);
                    lbl_taxTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * num).ToString("N", nfi);
                }
                ///amended by Diana,
                ///added divide with num of passenger to show single amount 

                lbl_PaxFeePrice.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["PaxFee"]) / num).ToString("N", nfi);
                lbl_PaxFeeTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["PaxFee"])).ToString("N", nfi);

                lblFuelPriceOneDepart.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"]) / num).ToString("N", nfi);
                lblFuelPriceTotalDepart.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"])).ToString("N", nfi);
                HttpCookie cookies = Request.Cookies["cookieSearchcondition"];
                if (cookies != null)
                {
                    if (cookies.Values["InfantNum"] != null && Convert.ToInt16(cookies.Values["InfantNum"]) != 0)
                    {
                        if (Convert.ToDecimal(dtBDFee.Rows[0]["Infant"]) != 0)
                        {
                            lbl_InfantTotal.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Infant"])).ToString("N", nfi);
                            lbl_currency12.Text = temFlight.TemFlightCurrencyCode.ToString();
                        }
                        else
                        {
                            trInfantFareDepart.Style.Add("display", "none");
                            //trInfantfareReturn.Style.Add("display", "none");
                        }

                    }
                    else
                    {
                        trInfantFareDepart.Style.Add("display", "none");
                        //trInfantfareReturn.Style.Add("display", "none");
                    }
                }
                lblSvcChargeOneDepart.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Svc"]) / num).ToString("N", nfi);
                lblSvcChargeTotalDepart.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Svc"])).ToString("N", nfi);

                lblOthOneDepart.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) / num + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"]) / num).ToString("N", nfi);
                lblOthTotalDepart.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"])).ToString("N", nfi);

                lblPromoDiscOneDepart.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"]) / num).ToString("N", nfi);
                lblPromoDiscTotalDepart.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"])).ToString("N", nfi);

                if (Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"]) < 0)
                {
                    trPromoDiscDepart.Visible = true;
                    trPromoDiscReturn.Visible = true;
                }
                else
                {
                    trPromoDiscDepart.Visible = false;
                    trPromoDiscReturn.Visible = false;
                }

                //lblSSRTotalDepart.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["SSR"])).ToString("N", nfi);
                if (dtBDFee.Rows[0]["Baggage"] != DBNull.Value)
                {
                    lblBaggageTotalDepart.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Baggage"])).ToString("N", nfi);
                }
                if (dtBDFee.Rows[0]["Meal"] != DBNull.Value)
                {
                    lblMealTotalDepart.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Meal"])).ToString("N", nfi);
                }
                if (dtBDFee.Rows[0]["Comfort"] != DBNull.Value)
                {
                    lblComfortTotalDepart.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Comfort"])).ToString("N", nfi);
                }
                if (dtBDFee.Rows[0]["Sport"] != DBNull.Value)
                {
                    lblSportTotalDepart.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Sport"])).ToString("N", nfi);
                }
                //added by ketee
                lblVATDepart.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) / num).ToString("N", nfi);
                lblVATTotalDepart.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["VAT"])).ToString("N", nfi);
                lblCurrVATDepart.Text = temFlight.TemFlightCurrencyCode.ToString();

                //added by ketee, 20161229
                //if (Session["DepartFlightSeatFees"] != null)
                //    lblSeatTotalDepart.Text = objGeneral.RoundUp(Convert.ToDecimal(Session["DepartFlightSeatFees"].ToString())).ToString("N", nfi); ;
                lblSeatTotalDepart.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Seat"])).ToString("N", nfi);

                apcAddOnBreakdown.TabPages.FindByName("TabGoing").Text = temFlight.TemFlightDeparture + " | " + temFlight.TemFlightArrival;
                apcAddOnBreakdown.TabPages.FindByName("TabReturn").Text = temFlight.TemFlightArrival + " | " + temFlight.TemFlightDeparture;

                //Boolean returnFlight = false;
                //if (ReturnID != "") returnFlight = true;
                //else returnFlight = false;
                //Session["IsReturnFlight"] = returnFlight;
                //dtAddOn = objBooking.GetAllBK_PASSENGERLISTWithSSRDataTableReviewBooking(TransID, false, "", returnFlight);
                //if (dtAddOn != null)
                //{
                //    if (foundRows[0]["TemFlightTransit"].ToString().Trim() != "")
                //    {
                //        gvAddOnDepart.Columns["ConDepartMeal"].Visible = true;
                //    }
                //    else
                //    {
                //        gvAddOnDepart.Columns["ConDepartMeal"].Visible = false;
                //    }

                //    gvAddOnDepart.DataSource = dtAddOn;
                //    gvAddOnDepart.DataBind();
                //    gvAddOnDepart.ExpandAll();


                //    if (Session["IsReturnFlight"] != null && (Boolean)Session["IsReturnFlight"] != false && foundRows[0]["TemFlightTransit"].ToString().Trim() != "")
                //    {
                //        gvAddOnReturn.Columns["ConReturnMeal"].Visible = true;
                //    }
                //    else
                //    {
                //        gvAddOnReturn.Columns["ConReturnMeal"].Visible = false;
                //    }

                //    gvAddOnReturn.DataSource = dtAddOn;
                //    gvAddOnReturn.DataBind();
                //    gvAddOnReturn.ExpandAll();
                //}
                using (profiler.Step("GBS:ShowAddOnBreakDown"))
                {
                    ShowAddOnBreakDown();
                }

                if (ReturnID != "")
                {
                    ShowReturnColumn(true);
                    //tr_return.Visible = true;
                    //tr_return2.Visible = true;

                    strExpr = "TemFlightId = '" + ReturnID + "'";
                    strSort = "";
                    dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                    foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                    using (profiler.Step("GBS:FillModelFromDataRowReturn"))
                    {
                        FillModelFromDataRow(foundRows, ref temFlight2);
                    }

                    //lbl_InArrival.Text = objGeneral.GetCityNameByCode(temFlight.TemFlightArrival) + "(" + temFlight.TemFlightArrival + ")";
                    lbl_InAverage.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight2.temFlightfarePrice)).ToString("N", nfi);

                    num = Convert.ToInt32(temFlight2.TemFlightPaxNum.ToString()); //added by diana 20141004, replace cookie

                    //if (Convert.ToDecimal(temFlight.TemFlightCHDNum) != 0)
                    //    lbl_InAverage.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight.temFlightfarePrice) / 2).ToString("N", nfi);
                    //else
                    //    lbl_InAverage.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight.temFlightfarePrice)).ToString("N", nfi);

                    lbl_InCurrency.Text = temFlight2.TemFlightCurrencyCode.ToString();
                    lbl_InCurrency0.Text = temFlight2.TemFlightCurrencyCode.ToString();
                    lbl_Incurrency1.Text = temFlight2.TemFlightCurrencyCode.ToString();
                    lbl_Incurrency2.Text = temFlight2.TemFlightCurrencyCode.ToString();
                    lbl_Incurrency21.Text = temFlight2.TemFlightCurrencyCode.ToString();
                    lbl_InCurrency3.Text = temFlight2.TemFlightCurrencyCode.ToString();
                    lbl_InCurrency31.Text = temFlight2.TemFlightCurrencyCode.ToString();
                    if (Convert.ToDecimal(temFlight2.TemFlightCHDNum) != 0) // && (temFlight.TemFlightDeparture.ToString().ToUpper() == "HKG" || temFlight.TemFlightArrival.ToString().ToUpper() == "HKG"))
                    {
                        lbl_Incurrency2CHD.Text = temFlight2.TemFlightCurrencyCode.ToString();
                        lbl_InCurrency3CHD.Text = temFlight2.TemFlightCurrencyCode.ToString();
                    }
                    lbl_InCurrency4.Text = temFlight2.TemFlightCurrencyCode.ToString();
                    lbl_InCurrency5.Text = temFlight2.TemFlightCurrencyCode.ToString();
                    lbl_InCurrency6.Text = temFlight2.TemFlightCurrencyCode.ToString();
                    lbl_InCurrency7.Text = temFlight2.TemFlightCurrencyCode.ToString();
                    lblCurrFuelReturn.Text = temFlight2.TemFlightCurrencyCode.ToString();
                    lblCurrOthReturn.Text = temFlight2.TemFlightCurrencyCode.ToString();
                    lblCurrSvcReturn.Text = temFlight2.TemFlightCurrencyCode.ToString();
                    lblCurrBaggageReturn.Text = temFlight2.TemFlightCurrencyCode.ToString();
                    lblCurrMealReturn.Text = temFlight2.TemFlightCurrencyCode.ToString();
                    lblCurrSportReturn.Text = temFlight2.TemFlightCurrencyCode.ToString();
                    lblCurrComfortReturn.Text = temFlight2.TemFlightCurrencyCode.ToString();

                    lblCurrPromoDiscReturn.Text = temFlight2.TemFlightCurrencyCode.ToString();
                    lbl_InCurrency14.Text = temFlight2.TemFlightCurrencyCode.ToString();

                    lbl_Incurrency2.Text = temFlight2.TemFlightCurrencyCode;
                    lbl_Incurrency21.Text = temFlight2.TemFlightCurrencyCode;
                    lblCurrAVLReturn.Text = temFlight2.TemFlightCurrencyCode;
                    lblCurrPSFReturn.Text = temFlight2.TemFlightCurrencyCode;
                    lblCurrSCFReturn.Text = temFlight2.TemFlightCurrencyCode;
                    lblCurrConnectingReturn.Text = temFlight2.TemFlightCurrencyCode;
                    lblCurrDiscReturn.Text = temFlight2.TemFlightCurrencyCode;
                    lblCurrKlia2Return.Text = temFlight2.TemFlightCurrencyCode;
                    lblCurrGSTReturn.Text = temFlight2.TemFlightCurrencyCode;
                    lblCurrSPLReturn.Text = temFlight2.TemFlightCurrencyCode;
                    lblCurrAPSReturn.Text = temFlight2.TemFlightCurrencyCode;
                    lblCurrIADFReturn.Text = temFlight2.TemFlightCurrencyCode;
                    lblCurrACFReturn.Text = temFlight2.TemFlightCurrencyCode;
                    lblCurrCSTReturn.Text = temFlight2.TemFlightCurrencyCode;
                    lblCurrCUTReturn.Text = temFlight2.TemFlightCurrencyCode;
                    lblCurrSGIReturn.Text = temFlight2.TemFlightCurrencyCode;
                    lblCurrSSTReturn.Text = temFlight2.TemFlightCurrencyCode;
                    lblCurrUDFReturn.Text = temFlight2.TemFlightCurrencyCode;
                    lblCurrASCReturn.Text = temFlight2.TemFlightCurrencyCode;
                    lblCurrBCLReturn.Text = temFlight2.TemFlightCurrencyCode;
                    lblCurrIWJRReturn.Text = temFlight2.TemFlightCurrencyCode;
                    lblCurrVATChargeReturn.Text = temFlight2.TemFlightCurrencyCode;
                    //lbl_InDeparture.Text = objGeneral.GetCityNameByCode(temFlight.TemFlightDeparture) + "(" + temFlight.TemFlightDeparture + ")";

                    totalApt = Convert.ToDecimal(num * temFlight2.TemFlightApt);

                    lbl_InFlightTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight2.temFlightfarePrice) * num).ToString("N", nfi);

                    //commented by diana 20131104
                    //if (Convert.ToDecimal(temFlight.TemFlightCHDNum) != 0)
                    //    lbl_InFlightTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight.temFlightfarePrice) * num / 2).ToString("N", nfi);
                    //else
                    //    lbl_InFlightTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight.temFlightfarePrice) * num).ToString("N", nfi);

                    lbl_InNum.Text = num.ToString();

                    if (Convert.ToDecimal(temFlight2.TemFlightCHDNum) != 0) // && (temFlight.TemFlightDeparture.ToString().ToUpper() == "HKG" || temFlight.TemFlightArrival.ToString().ToUpper() == "HKG"))
                    {
                        lbl_InNum2.Text = "Adult Airport Tax  ";
                        lbl_InNum2CHD.Text = temFlight2.TemFlightCHDNum.ToString();
                    }
                    else
                    {
                        lbl_InNum2.Text = "Airport Tax  ";
                    }
                    lbl_InNum3.Text = num.ToString();
                    lbl_InNum4.Text = num.ToString();
                    lbl_InNum5.Text = num.ToString();
                    lbl_InNum6.Text = num.ToString();
                    //lbl_InNumTax.Text = num.ToString();

                    dtBDFee = (DataTable)HttpContext.Current.Session["dataBDFeeReturn"];
                    //lbl_IntaxPrice.Text = objGeneral.RoundUp(Convert.ToDecimal(temFlight.TemFlightApt)).ToString("N", nfi);
                    //lbl_IntaxTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(num * temFlight.TemFlightApt)).ToString("N", nfi);

                    if (Convert.ToDecimal(temFlight2.TemFlightCHDNum) != 0) // && (temFlight.TemFlightDeparture.ToString().ToUpper() == "HKG" || temFlight.TemFlightArrival.ToString().ToUpper() == "HKG"))
                    {
                        //trInAptCHD.Visible = true;
                        lbl_IntaxPrice.Text = Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]).ToString("N", nfi);
                        lbl_IntaxTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * Convert.ToDecimal(temFlight2.TemFlightADTNum)).ToString("N", nfi);
                        lbl_IntaxPriceCHD.Text = Convert.ToDecimal(dtBDFee.Rows[0]["ChApt"]).ToString("N", nfi);
                        lbl_IntaxTotalCHD.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["ChApt"]) * Convert.ToDecimal(temFlight2.TemFlightCHDNum)).ToString("N", nfi);
                    }
                    else
                    {
                        //trInAptCHD.Visible = false;
                        lbl_IntaxPrice.Text = Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]).ToString("N", nfi);
                        lbl_IntaxTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Apt"]) * num).ToString("N", nfi);
                    }

                    lbl_InPaxFeePrice.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["PaxFee"]) / num).ToString("N", nfi);
                    lbl_InPaxFeeTotal.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["PaxFee"])).ToString("N", nfi);

                    ///amended by Diana,
                    ///added divide with num of passenger to show single amount

                    HttpCookie cookies2 = Request.Cookies["cookieSearchcondition"];
                    if (cookies2 != null)
                    {
                        if (cookies2.Values["InfantNum"] != null && Convert.ToInt16(cookies2.Values["InfantNum"]) != 0)
                        {

                            if (Convert.ToDecimal(dtBDFee.Rows[0]["Infant"]) != 0)
                            {
                                lbl_InInfantTotal.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Infant"])).ToString("N", nfi);
                                lbl_Incurrency12.Text = temFlight2.TemFlightCurrencyCode.ToString();
                            }
                            else
                            {
                                trInfantfareReturn.Style.Add("display", "none");
                            }

                        }
                        else
                        {
                            trInfantfareReturn.Style.Add("display", "none");
                        }
                    }
                    lblFuelOneReturn.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"]) / num).ToString("N", nfi);
                    lblFuelTotalReturn.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Fuel"])).ToString("N", nfi);

                    lblSvcOneReturn.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Svc"]) / num).ToString("N", nfi);
                    lblSvcTotalReturn.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Svc"])).ToString("N", nfi);

                    lblOthOneReturn.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) / num + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"]) / num).ToString("N", nfi);
                    lblOthTotalReturn.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Oth"]) + Convert.ToDecimal(dtBDFee.Rows[0]["Disc"])).ToString("N", nfi);

                    lblPromoDiscOneReturn.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"]) / num).ToString("N", nfi);
                    lblPromoDiscTotalReturn.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["PromoDisc"])).ToString("N", nfi);

                    //lblSSRTotalReturn.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["SSR"])).ToString("N", nfi);
                    //lblBaggageTotalReturn.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["SSR"])).ToString("N", nfi);
                    if (dtBDFee.Rows[0]["Baggage"] != DBNull.Value)
                    {
                        lblBaggageTotalReturn.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Baggage"])).ToString("N", nfi);
                    }
                    else
                    {
                        lblBaggageTotalReturn.Text = "0.00";
                    }
                    if (dtBDFee.Rows[0]["Meal"] != DBNull.Value)
                    {
                        lblMealTotalReturn.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Meal"])).ToString("N", nfi);
                    }
                    else
                    {
                        lblMealTotalReturn.Text = "0.00";
                    }
                    if (dtBDFee.Rows[0]["Sport"] != DBNull.Value)
                    {
                        lblSportTotalReturn.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Sport"])).ToString("N", nfi);
                    }
                    else
                    {
                        lblSportTotalReturn.Text = "0.00";
                    }
                    if (dtBDFee.Rows[0]["Comfort"] != DBNull.Value)
                    {
                        lblComfortTotalReturn.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Comfort"])).ToString("N", nfi);
                    }
                    else
                    {
                        lblComfortTotalReturn.Text = "0.00";
                    }
                    //added by keteelbl_InFlightTotal
                    lblVATReturn.Text = (Convert.ToDecimal(dtBDFee.Rows[0]["VAT"]) / num).ToString("N", nfi);
                    lblVATTotalReturn.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["VAT"])).ToString("N", nfi);
                    lblCurrVATReturn.Text = temFlight2.TemFlightCurrencyCode.ToString();

                    //added by ketee, 20161229
                    //if (Session["ReturnFlightSeatFees"] != null)
                    //    lblSeatTotalReturn.Text = objGeneral.RoundUp(Convert.ToDecimal(Session["ReturnFlightSeatFees"].ToString())).ToString("N", nfi); ;
                    lblSeatTotalReturn.Text = objGeneral.RoundUp(Convert.ToDecimal(dtBDFee.Rows[0]["Seat"])).ToString("N", nfi);
                    lbl_InTotal.Text = objGeneral.RoundUp(temFlight2.TemFlightTotalAmount).ToString("N", nfi);
                    total += temFlight2.TemFlightTotalAmount;

                    AVGFare = objGeneral.RoundUp(total / temFlight2.TemFlightPaxNum);
                }
                else
                {
                    ShowReturnColumn(false);
                    //tr_return.Visible = false;
                    //tr_return2.Visible = false;
                }
                lbl_TotalAmount.Text = objGeneral.RoundUp(total).ToString("N", nfi);
                lbl_TotalCurrency.Text = temFlight.TemFlightCurrencyCode.ToString();

                DataTable dtOth = new DataTable();
                //if (Session["dataTFOth"] != null)
                //{
                //    dtOth = (DataTable)Session["dataTFOth"];
                //}
                using (profiler.Step("GBS:GetAllFeesData"))
                {
                    dtOth = objGeneral.GetAllFeesData(TransID);
                }
                DataTable dtDataFeeCodeCopy = new DataTable();

                if (dtOth != null && dtOth.Rows.Count > 0)
                {
                    //added by ketee, 20170909, check return datatable count
                    if (dtOth.Select("Origin = '" + temFlight.TemFlightDeparture + "'").Length > 0)
                    {
                        dtDataFeeCodeCopy = dtOth.Select("Origin = '" + temFlight.TemFlightDeparture + "'").CopyToDataTable();
                        if (dtDataFeeCodeCopy.Rows.Count > 0)
                        {
                            rptFeeDepart.DataSource = dtDataFeeCodeCopy;
                            rptFeeDepart.DataBind();
                            foreach (RepeaterItem item in rptFeeDepart.Items)
                            {
                                Label lblFeeCurrDepart = item.FindControl("lblFeeCurrDepart") as Label;
                                lblFeeCurrDepart.Text = temFlight.TemFlightCurrencyCode;
                            }
                        }
                    }
                    

                    if (ReturnID != "")
                    {
                        //added by ketee, 20170909, check return datatable count
                        if (dtOth.Select("Origin = '" + temFlight2.TemFlightDeparture + "'").Length > 0)
                        {
                            dtDataFeeCodeCopy = dtOth.Select("Origin = '" + temFlight2.TemFlightDeparture + "'").CopyToDataTable();
                            if (dtDataFeeCodeCopy.Rows.Count > 0)
                            {
                                //dtDataFeeCodeCopy = dtOth.Select("Origin = '" + temFlight2.TemFlightDeparture + "'").CopyToDataTable();
                                rptFeeReturn.DataSource = dtDataFeeCodeCopy;
                                rptFeeReturn.DataBind();
                                foreach (RepeaterItem item in rptFeeReturn.Items)
                                {
                                    Label lblFeeCurrReturn = item.FindControl("lblFeeCurrReturn") as Label;
                                    lblFeeCurrReturn.Text = temFlight2.TemFlightCurrencyCode;
                                }
                            }
                        }
                        
                    }

                    //for (int i = 0; i < dtOth.Rows.Count; i++)
                    //{
                    //    if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "KLIA2" && dtOth.Rows[i]["Origin"].ToString() == temFlight.TemFlightDeparture.ToString())
                    //    {
                    //        lblKLIA2InfoDepart.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //        //tdDepartKlia2.Visible = true;
                    //        lbl_klia2Total.Text = (Convert.ToDecimal(lbl_klia2Total.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //    }
                    //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "GST" && dtOth.Rows[i]["Origin"].ToString() == temFlight.TemFlightDeparture.ToString())
                    //    {
                    //        lblGSTInfoDepart.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //        //tdDepartGST.Visible = true;
                    //        lbl_GSTTotal.Text = (Convert.ToDecimal(lbl_GSTTotal.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //    }
                    //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "AVL" && dtOth.Rows[i]["Origin"].ToString() == temFlight.TemFlightDeparture.ToString())
                    //    {
                    //        lblAVLInfoDepart.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //        //tdDepartAVL.Visible = true;
                    //        lblAVLTotalDepart.Text = (Convert.ToDecimal(lblAVLTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //    }
                    //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "PSF" && dtOth.Rows[i]["Origin"].ToString() == temFlight.TemFlightDeparture.ToString())
                    //    {
                    //        lblPSFInfoDepart.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //        //tdDepartPSF.Visible = true;
                    //        lblPSFTotalDepart.Text = (Convert.ToDecimal(lblPSFTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //    }
                    //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "SCF" && dtOth.Rows[i]["Origin"].ToString() == temFlight.TemFlightDeparture.ToString())
                    //    {
                    //        lblSCFInfoDepart.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //        //tdDepartSCF.Visible = true;
                    //        lblSCFTotalDepart.Text = (Convert.ToDecimal(lblSCFTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //    }
                    //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "" && dtOth.Rows[i]["FeeType"].ToString() == "ConnectionAdjustmentAmount" && dtOth.Rows[i]["Origin"].ToString() == temFlight.TemFlightDeparture.ToString())
                    //    {
                    //        //tdConnectingDepart.Visible = true;
                    //        lblConnectingDepartTotal.Text = (Convert.ToDecimal(lblConnectingDepartTotal.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //    }
                    //    else if (dtOth.Rows[i]["FeeType"].ToString().ToUpper() == "DISCOUNT" && dtOth.Rows[i]["Origin"].ToString() == temFlight.TemFlightDeparture.ToString())
                    //    {
                    //        //tdDepartSCF.Visible = true;
                    //        lblDiscTotalDepart.Text = (Convert.ToDecimal(lblDiscTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //    }
                    //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "SPL" && dtOth.Rows[i]["Origin"].ToString() == temFlight.TemFlightDeparture.ToString())
                    //    {
                    //        lblSPLInfoDepart.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //        //tdDepartSCF.Visible = true;
                    //        lblSPLTotalDepart.Text = (Convert.ToDecimal(lblSPLTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //    }
                    //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "APS" && dtOth.Rows[i]["Origin"].ToString() == temFlight.TemFlightDeparture.ToString())
                    //    {
                    //        lblAPSInfoDepart.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //        //tdDepartSCF.Visible = true;
                    //        lblAPSTotalDepart.Text = (Convert.ToDecimal(lblAPSTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //    }
                    //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "ACF" && dtOth.Rows[i]["Origin"].ToString() == temFlight.TemFlightDeparture.ToString())
                    //    {
                    //        lblACFInfoDepart.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //        //tdDepartSCF.Visible = true;
                    //        lblACFTotalDepart.Text = (Convert.ToDecimal(lblACFTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //    }
                    //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "CGST" && (dtOth.Rows[i]["FeeDesc"].ToString().Substring(0, 3) == temFlight.TemFlightDeparture.ToString()))
                    //    {
                    //        lblCSTInfoDepart.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //        //tdDepartSCF.Visible = true;
                    //        lblCSTTotalDepart.Text = (Convert.ToDecimal(lblCSTTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //    }
                    //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "CUTE" && (dtOth.Rows[i]["FeeDesc"].ToString().Substring(0, 3) == temFlight.TemFlightDeparture.ToString()))
                    //    {
                    //        lblCUTInfoDepart.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //        //tdDepartSCF.Visible = true;
                    //        lblCUTTotalDepart.Text = (Convert.ToDecimal(lblCUTTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //    }
                    //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "SGI" && (dtOth.Rows[i]["FeeDesc"].ToString().Substring(0, 3) == temFlight.TemFlightDeparture.ToString()))
                    //    {
                    //        lblSGIInfoDepart.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //        //tdDepartSCF.Visible = true;
                    //        lblSGITotalDepart.Text = (Convert.ToDecimal(lblSGITotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //    }
                    //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "SGST" && (dtOth.Rows[i]["FeeDesc"].ToString().Substring(0, 3) == temFlight.TemFlightDeparture.ToString()))
                    //    {
                    //        lblSSTInfoDepart.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //        //tdDepartSCF.Visible = true;
                    //        lblSSTTotalDepart.Text = (Convert.ToDecimal(lblSSTTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //    }
                    //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "UDF" && (dtOth.Rows[i]["FeeDesc"].ToString().Substring(0, 3) == temFlight.TemFlightDeparture.ToString()))
                    //    {
                    //        lblUDFInfoDepart.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //        //tdDepartSCF.Visible = true;
                    //        lblUDFTotalDepart.Text = (Convert.ToDecimal(lblUDFTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //    }
                    //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "IADF" && dtOth.Rows[i]["Origin"].ToString() == temFlight.TemFlightDeparture.ToString())
                    //    {
                    //        lblIADFInfoDepart.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //        //tdDepartSCF.Visible = true;
                    //        lblIADFTotalDepart.Text = (Convert.ToDecimal(lblIADFTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //    }
                    //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "ACF" && dtOth.Rows[i]["Origin"].ToString() == temFlight.TemFlightDeparture.ToString())
                    //    {
                    //        lblACFInfoDepart.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //        //tdDepartSCF.Visible = true;
                    //        lblACFTotalDepart.Text = (Convert.ToDecimal(lblACFTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //    }
                    //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "ASC" && dtOth.Rows[i]["Origin"].ToString() == temFlight.TemFlightDeparture.ToString())
                    //    {
                    //        lblASCInfoDepart.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //        //tdDepartSCF.Visible = true;
                    //        lblASCTotalDepart.Text = (Convert.ToDecimal(lblASCTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //    }
                    //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "BCL" && dtOth.Rows[i]["Origin"].ToString() == temFlight.TemFlightDeparture.ToString())
                    //    {
                    //        lblBCLInfoDepart.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //        //tdDepartSCF.Visible = true;
                    //        lblBCLTotalDepart.Text = (Convert.ToDecimal(lblBCLTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //    }
                    //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "IWJR" && dtOth.Rows[i]["Origin"].ToString() == temFlight.TemFlightDeparture.ToString())
                    //    {
                    //        lblIWJRInfoDepart.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //        //tdDepartSCF.Visible = true;
                    //        lblIWJRTotalDepart.Text = (Convert.ToDecimal(lblIWJRTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //    }
                    //    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "VAT" && dtOth.Rows[i]["Origin"].ToString() == temFlight.TemFlightDeparture.ToString())
                    //    {
                    //        lblVATInfoDepart.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //        //tdDepartSCF.Visible = true;
                    //        lblVATChargeTotalDepart.Text = (Convert.ToDecimal(lblVATChargeTotalDepart.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //    }

                    //    if (ReturnID != "")
                    //    {
                    //        if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "KLIA2" && dtOth.Rows[i]["Origin"].ToString() == temFlight2.TemFlightDeparture.ToString())
                    //        {
                    //            lblKLIA2InfoReturn.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //            //tdReturnKlia2.Visible = true;
                    //            lbl_Inklia2Total.Text = (Convert.ToDecimal(lbl_Inklia2Total.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //        }
                    //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "GST" && dtOth.Rows[i]["Origin"].ToString() == temFlight2.TemFlightDeparture.ToString())
                    //        {
                    //            lblGSTInfoReturn.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //            //tdReturnGST.Visible = true;
                    //            lbl_InGSTTotal.Text = (Convert.ToDecimal(lbl_InGSTTotal.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //        }
                    //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "AVL" && dtOth.Rows[i]["Origin"].ToString() == temFlight2.TemFlightDeparture.ToString())
                    //        {
                    //            lblAVLInfoReturn.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //            //tdReturnAVL.Visible = true;
                    //            lblAVLTotalReturn.Text = (Convert.ToDecimal(lblAVLTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //        }
                    //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "PSF" && dtOth.Rows[i]["Origin"].ToString() == temFlight2.TemFlightDeparture.ToString())
                    //        {
                    //            lblPSFInfoReturn.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //            //tdReturnPSF.Visible = true;
                    //            lblPSFTotalReturn.Text = (Convert.ToDecimal(lblPSFTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //        }
                    //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "SCF" && dtOth.Rows[i]["Origin"].ToString() == temFlight2.TemFlightDeparture.ToString())
                    //        {
                    //            lblSCFInfoReturn.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //            //tdReturnSCF.Visible = true;
                    //            lblSCFTotalReturn.Text = (Convert.ToDecimal(lblSCFTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //        }
                    //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "" && dtOth.Rows[i]["FeeType"].ToString() == "ConnectionAdjustmentAmount" && dtOth.Rows[i]["Origin"].ToString() == temFlight2.TemFlightDeparture.ToString())
                    //        {
                    //            //tdConnectingReturn.Visible = true;
                    //            lblConnectingReturnTotal.Text = (Convert.ToDecimal(lblConnectingReturnTotal.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //        }
                    //        else if (dtOth.Rows[i]["FeeCode"].ToString() == "Discount" && dtOth.Rows[i]["Origin"].ToString() == temFlight2.TemFlightDeparture.ToString())
                    //        {
                    //            //tdDepartSCF.Visible = true;
                    //            lblDiscTotalReturn.Text = (Convert.ToDecimal(lblDiscTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //        }
                    //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "SPL" && dtOth.Rows[i]["Origin"].ToString() == temFlight2.TemFlightDeparture.ToString())
                    //        {
                    //            lblSPLInfoReturn.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //            //tdDepartSCF.Visible = true;
                    //            lblSPLTotalReturn.Text = (Convert.ToDecimal(lblSPLTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //        }
                    //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "APS" && dtOth.Rows[i]["Origin"].ToString() == temFlight2.TemFlightDeparture.ToString())
                    //        {
                    //            lblAPSInfoReturn.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //            //tdDepartSCF.Visible = true;
                    //            lblAPSTotalReturn.Text = (Convert.ToDecimal(lblAPSTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //        }
                    //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "ACF" && dtOth.Rows[i]["Origin"].ToString() == temFlight2.TemFlightDeparture.ToString())
                    //        {
                    //            lblACFInfoReturn.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //            //tdDepartSCF.Visible = true;
                    //            lblACFTotalReturn.Text = (Convert.ToDecimal(lblACFTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //        }
                    //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "CGST" && dtOth.Rows[i]["Origin"].ToString() == temFlight2.TemFlightDeparture.ToString())
                    //        {
                    //            lblCSTInfoReturn.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //            //tdDepartSCF.Visible = true;
                    //            lblCSTTotalReturn.Text = (Convert.ToDecimal(lblCSTTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //        }
                    //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "CUTE" && dtOth.Rows[i]["Origin"].ToString() == temFlight2.TemFlightDeparture.ToString())
                    //        {
                    //            lblCUTInfoReturn.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //            //tdReturnSCF.Visible = true;
                    //            lblCUTTotalReturn.Text = (Convert.ToDecimal(lblCUTTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //        }
                    //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "SGI" && dtOth.Rows[i]["Origin"].ToString() == temFlight2.TemFlightDeparture.ToString())
                    //        {
                    //            lblSGIInfoReturn.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //            //tdReturnSCF.Visible = true;
                    //            lblSGITotalReturn.Text = (Convert.ToDecimal(lblSGITotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //        }
                    //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "SGST" && dtOth.Rows[i]["Origin"].ToString() == temFlight2.TemFlightDeparture.ToString())
                    //        {
                    //            lblSSTInfoReturn.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //            //tdReturnSCF.Visible = true;
                    //            lblSSTTotalReturn.Text = (Convert.ToDecimal(lblSSTTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //        }
                    //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "UDF" && dtOth.Rows[i]["Origin"].ToString() == temFlight2.TemFlightDeparture.ToString())
                    //        {
                    //            lblUDFInfoReturn.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //            //tdReturnSCF.Visible = true;
                    //            lblUDFTotalReturn.Text = (Convert.ToDecimal(lblUDFTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //        }
                    //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "IADF" && dtOth.Rows[i]["Origin"].ToString() == temFlight2.TemFlightDeparture.ToString())
                    //        {
                    //            lblIADFInfoReturn.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //            //tdDepartSCF.Visible = true;
                    //            lblIADFTotalReturn.Text = (Convert.ToDecimal(lblIADFTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //        }
                    //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "ASC" && dtOth.Rows[i]["Origin"].ToString() == temFlight2.TemFlightDeparture.ToString())
                    //        {
                    //            lblASCInfoReturn.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //            //tdDepartSCF.Visible = true;
                    //            lblASCTotalReturn.Text = (Convert.ToDecimal(lblASCTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //        }
                    //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "BCL" && dtOth.Rows[i]["Origin"].ToString() == temFlight2.TemFlightDeparture.ToString())
                    //        {
                    //            lblBCLInfoReturn.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //            //tdDepartSCF.Visible = true;
                    //            lblBCLTotalReturn.Text = (Convert.ToDecimal(lblBCLTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //        }
                    //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "IWJR" && dtOth.Rows[i]["Origin"].ToString() == temFlight2.TemFlightDeparture.ToString())
                    //        {
                    //            lblIWJRInfoReturn.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //            //tdDepartSCF.Visible = true;
                    //            lblIWJRTotalReturn.Text = (Convert.ToDecimal(lblIWJRTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //        }
                    //        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "VAT" && dtOth.Rows[i]["Origin"].ToString() == temFlight2.TemFlightDeparture.ToString())
                    //        {
                    //            lblVATInfoReturn.Text = dtOth.Rows[i]["FeeCode"].ToString().ToUpper() + " Charge";
                    //            //tdDepartSCF.Visible = true;
                    //            lblVATChargeTotalReturn.Text = (Convert.ToDecimal(lblVATChargeTotalReturn.Text) + Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"])).ToString("N", nfi);
                    //        }
                    //    }
                    //}
                }

                //20170530 - Sienny (put amount due to session)
                Session["TotalAmountDue"] = objGeneral.RoundUp(total).ToString("N", nfi);

                lblAverageFare.Text = objGeneral.RoundUp(AVGFare).ToString("N", nfi);
                lblAverageCurrency.Text = lbl_TotalCurrency.Text;

                using (profiler.Step("GBS:HideZeroValue"))
                {
                    HideZeroValue();
                }
            }
        }

        protected void HideZeroValue()
        {
            if (lbl_FlightTotal.Text == "" || lbl_FlightTotal.Text == "0.00")
            {
                trDepartFare.Visible = false;
            }
            if (lbl_taxTotal.Text == "" || lbl_taxTotal.Text == "0.00")
            {
                trAirportTaxDepart.Visible = false;
            }
            if (lbl_PaxFeeTotal.Text == "" || lbl_PaxFeeTotal.Text == "0.00")
            {
                trPaxServChargeDepart.Visible = false;
            }
            if (lblFuelPriceTotalDepart.Text == "" || lblFuelPriceTotalDepart.Text == "0.00")
            {
                trFuelTaxDepart.Visible = false;
            }
            if (lblSvcChargeTotalDepart.Text == "" || lblSvcChargeTotalDepart.Text == "0.00")
            {
                trServChargeDepart.Visible = false;
            }
            if (lblCSTTotalDepart.Text == "" || lblCSTTotalDepart.Text == "0.00")
            {
                trCSTChargeDepart.Visible = false;
            }
            if (lblCUTTotalDepart.Text == "" || lblCUTTotalDepart.Text == "0.00")
            {
                trCUTChargeDepart.Visible = false;
            }
            if (lblSGITotalDepart.Text == "" || lblSGITotalDepart.Text == "0.00")
            {
                trSGIChargeDepart.Visible = false;
            }
            if (lblSSTTotalDepart.Text == "" || lblSSTTotalDepart.Text == "0.00")
            {
                trSSTChargeDepart.Visible = false;
            }
            if (lblUDFTotalDepart.Text == "" || lblUDFTotalDepart.Text == "0.00")
            {
                trUDFChargeDepart.Visible = false;
            }
            if (lblVATDepart.Text == "" || lblVATDepart.Text == "0.00")
            {
                trVATDepart.Visible = false;
            }
            if (lblBaggageTotalDepart.Text == "" || lblBaggageTotalDepart.Text == "0.00")
            {
                trBaggageChargeDepart.Visible = false;
            }
            if (lblMealTotalDepart.Text == "" || lblMealTotalDepart.Text == "0.00")
            {
                trMealChargeDepart.Visible = false;
            }
            if (lblSportTotalDepart.Text == "" || lblSportTotalDepart.Text == "0.00")
            {
                trSportChargeDepart.Visible = false;
            }
            if (lblComfortTotalDepart.Text == "" || lblComfortTotalDepart.Text == "0.00")
            {
                trComfortChargeDepart.Visible = false;
            }
            if (lblSeatTotalDepart.Text == "" || lblSeatTotalDepart.Text == "0.00")
            {
                trSeatChargeDepart.Visible = false;
            }
            if (lblOthTotalDepart.Text == "" || lblOthTotalDepart.Text == "0.00")
            {
                trOthChargeDepart.Visible = false;
            }
            if (lblConnectingDepartTotal.Text == "" || lblConnectingDepartTotal.Text == "0.00")
            {
                trConnectingChargeDepart.Visible = false;
            }
            if (lbl_klia2Total.Text == "" || lbl_klia2Total.Text == "0.00")
            {
                trKlia2FeeDepart.Visible = false;
            }
            if (lbl_GSTTotal.Text == "" || lbl_GSTTotal.Text == "0.00")
            {
                trGSTChargeDepart.Visible = false;
            }
            if (lblAVLTotalDepart.Text == "" || lblAVLTotalDepart.Text == "0.00")
            {
                trAVLChargeDepart.Visible = false;
            }
            if (lblPSFTotalDepart.Text == "" || lblPSFTotalDepart.Text == "0.00")
            {
                trPSFChargeDepart.Visible = false;
            }
            if (lblSCFTotalDepart.Text == "" || lblSCFTotalDepart.Text == "0.00")
            {
                trSCFChargeDepart.Visible = false;
            }
            if (lblDiscTotalDepart.Text == "" || lblDiscTotalDepart.Text == "0.00")
            {
                trDiscountChargeDepart.Visible = false;
            }
            if (lblSPLTotalDepart.Text == "" || lblSPLTotalDepart.Text == "0.00")
            {
                trSPLChargeDepart.Visible = false;
            }
            if (lblAPSTotalDepart.Text == "" || lblAPSTotalDepart.Text == "0.00")
            {
                trAPSChargeDepart.Visible = false;
            }
            if (lblACFTotalDepart.Text == "" || lblACFTotalDepart.Text == "0.00")
            {
                trACFChargeDepart.Visible = false;
            }
            if (lblIADFTotalDepart.Text == "" || lblIADFTotalDepart.Text == "0.00")
            {
                trIADFChargeDepart.Visible = false;
            }
            if (lblASCTotalDepart.Text == "" || lblASCTotalDepart.Text == "0.00")
            {
                trASCChargeDepart.Visible = false;
            }
            if (lblBCLTotalDepart.Text == "" || lblBCLTotalDepart.Text == "0.00")
            {
                trBCLChargeDepart.Visible = false;
            }
            if (lblIWJRTotalDepart.Text == "" || lblIWJRTotalDepart.Text == "0.00")
            {
                trIWJRChargeDepart.Visible = false;
            }
            if (lblVATChargeTotalDepart.Text == "" || lblVATChargeTotalDepart.Text == "0.00")
            {
                trVATChargeDepart.Visible = false;
            }
            if (lblAPFTotalDepart.Text == "" || lblAPFTotalDepart.Text == "0.00")
            {
                trAPFChargeDepart.Visible = false;
            }
            if (lblAPFCTotalDepart.Text == "" || lblAPFCTotalDepart.Text == "0.00")
            {
                trAPFCChargeDepart.Visible = false;
            }
            if (lblIPSCTotalDepart.Text == "" || lblIPSCTotalDepart.Text == "0.00")
            {
                trIPSCChargeDepart.Visible = false;
            }
            if (lblISFTotalDepart.Text == "" || lblISFTotalDepart.Text == "0.00")
            {
                trISFChargeDepart.Visible = false;
            }
            if (lblPSCTotalDepart.Text == "" || lblPSCTotalDepart.Text == "0.00")
            {
                trPSCChargeDepart.Visible = false;
            }
            if (lblPromoDiscTotalDepart.Text == "" || lblPromoDiscTotalDepart.Text == "0.00")
            {
                trPromoDiscDepart.Visible = false;
            }
            if (lbl_taxTotalCHD.Text == "" || lbl_taxTotalCHD.Text == "0.00")
            {
                trChildAirportDepart.Visible = false;
            }

            if (lbl_IntaxTotalCHD.Text == "" || lbl_IntaxTotalCHD.Text == "0.00")
            {
                trChildAirportTaxReturn.Visible = false;
            }
            if (lbl_InFlightTotal.Text == "" || lbl_InFlightTotal.Text == "0.00")
            {
                trReturnfare.Visible = false;
            }
            if (lbl_IntaxTotal.Text == "" || lbl_IntaxTotal.Text == "0.00")
            {
                trAirportTaxReturn.Visible = false;
            }
            if (lbl_InPaxFeeTotal.Text == "" || lbl_InPaxFeeTotal.Text == "0.00")
            {
                PaxServChargeReturn.Visible = false;
            }
            if (lblFuelTotalReturn.Text == "" || lblFuelTotalReturn.Text == "0.00")
            {
                trFuelTaxReturn.Visible = false;
            }
            if (lblSvcTotalReturn.Text == "" || lblSvcTotalReturn.Text == "0.00")
            {
                trServChargeReturn.Visible = false;
            }
            if (lblVATReturn.Text == "" || lblVATReturn.Text == "0.00")
            {
                trVATReturn.Visible = false;
            }
            if (lblBaggageTotalReturn.Text == "" || lblBaggageTotalReturn.Text == "0.00")
            {
                trBagggageChargeReturn.Visible = false;
            }
            if (lblMealTotalReturn.Text == "" || lblMealTotalReturn.Text == "0.00")
            {
                trMealChargeReturn.Visible = false;
            }
            if (lblSportTotalReturn.Text == "" || lblSportTotalReturn.Text == "0.00")
            {
                trSportChargeReturn.Visible = false;
            }
            if (lblCSTTotalReturn.Text == "" || lblCSTTotalReturn.Text == "0.00")
            {
                trCSTChargeReturn.Visible = false;
            }
            if (lblCUTTotalReturn.Text == "" || lblCUTTotalReturn.Text == "0.00")
            {
                trCUTChargeReturn.Visible = false;
            }
            if (lblSGITotalReturn.Text == "" || lblSGITotalReturn.Text == "0.00")
            {
                trSGIChargeReturn.Visible = false;
            }
            if (lblSSTTotalReturn.Text == "" || lblSSTTotalReturn.Text == "0.00")
            {
                trSSTChargeReturn.Visible = false;
            }
            if (lblUDFTotalReturn.Text == "" || lblUDFTotalReturn.Text == "0.00")
            {
                trUDFChargeReturn.Visible = false;
            }
            if (lblComfortTotalReturn.Text == "" || lblComfortTotalReturn.Text == "0.00")
            {
                trComfortChargeReturn.Visible = false;
            }
            if (lblSeatTotalReturn.Text == "" || lblSeatTotalReturn.Text == "0.00")
            {
                trSeatChargeReturn.Visible = false;
            }
            if (lblOthTotalReturn.Text == "" || lblOthTotalReturn.Text == "0.00")
            {
                trOthChargeReturn.Visible = false;
            }
            if (lblConnectingReturnTotal.Text == "" || lblConnectingReturnTotal.Text == "0.00")
            {
                trConnectingChargeReturn.Visible = false;
            }
            if (lbl_Inklia2Total.Text == "" || lbl_Inklia2Total.Text == "0.00")
            {
                trKlia2FeeReturn.Visible = false;
            }
            if (lbl_InGSTTotal.Text == "" || lbl_InGSTTotal.Text == "0.00")
            {
                trGSTChargeReturn.Visible = false;
            }
            if (lblAVLTotalReturn.Text == "" || lblAVLTotalReturn.Text == "0.00")
            {
                trAVLChargeReturn.Visible = false;
            }
            if (lblPSFTotalReturn.Text == "" || lblPSFTotalReturn.Text == "0.00")
            {
                trPSFChargeReturn.Visible = false;
            }
            if (lblSCFTotalReturn.Text == "" || lblSCFTotalReturn.Text == "0.00")
            {
                trSCFChargeReturn.Visible = false;
            }
            if (lblSPLTotalReturn.Text == "" || lblSPLTotalReturn.Text == "0.00")
            {
                trSPLChargeReturn.Visible = false;
            }
            if (lblAPSTotalReturn.Text == "" || lblAPSTotalReturn.Text == "0.00")
            {
                trAPSChargeReturn.Visible = false;
            }
            if (lblACFTotalReturn.Text == "" || lblACFTotalReturn.Text == "0.00")
            {
                trACFChargeReturn.Visible = false;
            }
            if (lblIADFTotalReturn.Text == "" || lblIADFTotalReturn.Text == "0.00")
            {
                trIADFChargeReturn.Visible = false;
            }
            if (lblASCTotalReturn.Text == "" || lblASCTotalReturn.Text == "0.00")
            {
                trASCChargeReturn.Visible = false;
            }
            if (lblBCLTotalReturn.Text == "" || lblBCLTotalReturn.Text == "0.00")
            {
                trBCLChargeReturn.Visible = false;
            }
            if (lblIWJRTotalReturn.Text == "" || lblIWJRTotalReturn.Text == "0.00")
            {
                trIWJRChargeReturn.Visible = false;
            }
            if (lblVATChargeTotalReturn.Text == "" || lblVATChargeTotalReturn.Text == "0.00")
            {
                trVATChargeReturn.Visible = false;
            }
            if (lblAPFTotalReturn.Text == "" || lblAPFTotalReturn.Text == "0.00")
            {
                trAPFChargeReturn.Visible = false;
            }
            if (lblAPFCTotalReturn.Text == "" || lblAPFCTotalReturn.Text == "0.00")
            {
                trAPFCChargeReturn.Visible = false;
            }
            if (lblIPSCTotalReturn.Text == "" || lblIPSCTotalReturn.Text == "0.00")
            {
                trIPSCChargeReturn.Visible = false;
            }
            if (lblISFTotalReturn.Text == "" || lblISFTotalReturn.Text == "0.00")
            {
                trISFChargeReturn.Visible = false;
            }
            if (lblPSCTotalReturn.Text == "" || lblPSCTotalReturn.Text == "0.00")
            {
                trPSCChargeReturn.Visible = false;
            }
            if (lblDiscTotalReturn.Text == "" || lblDiscTotalReturn.Text == "0.00")
            {
                trDiscountChargeReturn.Visible = false;
            }
            if (lblPromoDiscTotalReturn.Text == "" || lblPromoDiscTotalReturn.Text == "0.00")
            {
                trPromoDiscReturn.Visible = false;
            }
        }

        protected void btn_Next_Click(object sender, EventArgs e)
        {

        }

        //protected void btn_UpdateCurrency_Click(object sender, EventArgs e)
        //{

        //}

        private void ShowReturnColumn(bool show)
        {
            tdReturnTitle.Visible = show;
            tdReturnFare.Visible = show;
            tdReturn.Visible = show;
            //tdReturnTotal.Visible = show;
            //tdReturnAPT.Visible = show;
            //tdReturnAPTChd.Visible = show;
            //tdReturnFuel.Visible = show;
            //tdReturnPaxFee.Visible = show;
            //tdReturnSvc.Visible = show;
            //tdReturnVAT.Visible = show;
            //tdReturnBaggage.Visible = show;
            //tdReturnMeal.Visible = show;
            //tdReturnSport.Visible = show;
            //tdReturnComfort.Visible = show;
            //tdReturnSeat.Visible = show;
            //tdReturnOther.Visible = show;
            //tdReturnInfant.Visible = show;
            //tdReturnPromoDisc.Visible = show;

            //20170425 - Sienny (popup addon breakdown tab)
            apcAddOnBreakdown.TabPages.FindByName("TabReturn").ClientVisible = show;
        }

        //moved from reiewfare by Agus
        decimal totalServiceFee;
        protected void SavingProcess()
        {
            var profiler = MiniProfiler.Current;
            try
            {


                /*
                if (SaveData())
                    e.Result = "";
                else
                    e.Result = msgList.Err100031;
                */
                using (profiler.Step("GBS:SaveData"))
                {
                    SaveData();
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //e.Result = msgList.Err100013;
            }
        }


        protected bool SaveData()
        {
            var profiler = MiniProfiler.Current;
            ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();

            string strExpr;
            string strSort;
            string keyCarrier = "";
            decimal totalOth = 0; //service charge total
            decimal totalDisc = 0; //discount charge total
            decimal totalPromoDisc = 0;
            DataTable dt = new DataTable();
            Hashtable ht = new Hashtable();

            //payment control
            PaymentControl objPayment = new PaymentControl();

            strExpr = "TemFlightId = '" + departID + "'";
            strSort = "";
            DateTime departDate;
            dt = (DataTable)HttpContext.Current.Session["TempFlight"];
            DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
            using (profiler.Step("GBS:FillModelFromDataRow"))
            {
                FillModelFromDataRow(foundRows, ref temFlight);
            }

            departDate = Convert.ToDateTime(temFlight.TemFlightStd);

            Currency = temFlight.TemFlightCurrencyCode.Trim();
            using (profiler.Step("GBS:GetSingleAgentProfile"))
            {
                agent = objAgentProfile.GetSingleAgentProfile(MyUserSet.AgentName);
            }

            string LoginType = MyUserSet.AgentType.ToString();

            int m = 0;
            int count = 0;
            DataTable dtClass = objBooking.dtClass();
            if (HttpContext.Current.Session["dataClass"] != null) { dtClass = (DataTable)HttpContext.Current.Session["dataClass"]; }
            count = dtClass.Rows.Count;

            byte seqNo = 1;
            List<BookingTransactionMain> TransMain = new List<BookingTransactionMain>();

            tranID = DateTime.Now.ToString("yyyyMMddHHmmsss");

            #region newsavedetail
            //Datatable Process 

            //string PNR = book.BookingCommit(agent, temClass.TemClassPersonNumber, temClass.TemClassClassPrice.Value, temClass.TemClassSellSignature);

            DataTable dataClass = objBooking.dtClass();
            dataClass = (DataTable)HttpContext.Current.Session["dataClass"];
            foreach (DataRow dr in dataClass.Rows)
            {
                bookDTLInfo = new BookingTransactionDetail();
                string PNR = seqNo.ToString();
                bookDTLInfo.RecordLocator = PNR;
                bookDTLInfo.TransID = tranID;
                bookDTLInfo.SeqNo = seqNo;

                if (seqNo == 1)
                {
                    keyCarrier = bookDTLInfo.CarrierCode = dr["CarrierCode"].ToString();
                    ht.Add("keyCarrier", keyCarrier);
                }

                //service charge pax
                //decimal svcCharge = Convert.ToDecimal(objGeneral.getSysValueByKeyAndCarrierCode("SVCCHARGEPAX", keyCarrier)) / 100;
                bookDTLInfo.LineOth = Convert.ToDecimal(dr["OthChrg"].ToString());
                totalOth += bookDTLInfo.LineOth;

                bookDTLInfo.LineDisc = Convert.ToDecimal(dr["DiscChrg"].ToString());
                totalDisc += bookDTLInfo.LineDisc;

                bookDTLInfo.LinePromoDisc = Convert.ToDecimal(dr["PromoDiscChrg"].ToString());
                totalPromoDisc += bookDTLInfo.LinePromoDisc;

                seqNo += 1;
                bookDTLInfo.Currency = Currency;
                bookDTLInfo.CarrierCode = dr["CarrierCode"].ToString();
                bookDTLInfo.FlightNo = dr["FlightNumber"].ToString();
                bookDTLInfo.DepatureDate = Convert.ToDateTime(dr["DepartureDate"].ToString());
                bookDTLInfo.ArrivalDate = Convert.ToDateTime(dr["ArrivalDate"].ToString());
                bookDTLInfo.Origin = dr["Origin"].ToString();
                bookDTLInfo.Destination = dr["Destination"].ToString();

                //bookDTLInfo.LineFee = Convert.ToDecimal(dr["FuelChrg"].ToString());
                bookDTLInfo.LineFee = Convert.ToDecimal(dr["ServChrg"].ToString());
                totalServiceFee += bookDTLInfo.LineFee;

                bookDTLInfo.FareClass = dr["FareClass"].ToString();
                bookDTLInfo.PaxAdult = Convert.ToInt16(dr["AdultPax"].ToString());
                bookDTLInfo.PaxChild = Convert.ToInt16(dr["ChildPax"].ToString());
                bookDTLInfo.LineTotal = Convert.ToDecimal(dr["FullPrice"].ToString());
                bookDTLInfo.SyncLastUpd = DateTime.Now;
                bookDTLInfo.LastSyncBy = MyUserSet.AgentID;

                ////ongoing payment scheme
                //bookDTLInfo.FlightDuration = Convert.ToDecimal(dr["FlightDuration"].ToString());
                //bookDTLInfo.FlightType = Convert.ToInt16(dr["FlightType"].ToString());

                bookDTLInfo.FarePerPax = Convert.ToDecimal(dr["FarePrice"].ToString());
                //bookDTLInfo.FarePerPax = Convert.ToDecimal(dr["FarePrice"].ToString()) / Convert.ToDecimal(dr["Quantity"].ToString());

                bookDTLInfo.LineTax = Convert.ToDecimal(dr["TaxChrg"].ToString()) + Convert.ToDecimal(dr["FuelChrg"].ToString()); //apt + fuel

                //totalFlightFare += bookDTLInfo.LineTotal + bookDTLInfo.LineOth; //include service charge
                totalFlightFare += bookDTLInfo.LineTotal; //include service charge

                bookDTLInfo.TransVoid = 0;
                bookDTLInfo.CreateBy = MyUserSet.AgentID;
                bookDTLInfo.SyncCreate = DateTime.Now;

                bookDTLInfo.Transit = dr["TemClassTransit"].ToString();
                if (bookDTLInfo.Transit != "")
                {
                    bookDTLInfo.ArrivalDate2 = Convert.ToDateTime(dr["TemClassSta2"].ToString());
                    bookDTLInfo.DepatureDate2 = Convert.ToDateTime(dr["TemClassStd2"].ToString());
                }

                bookDTLInfo.CollectedAmount = 0;
                bookDTLInfo.Signature = dr["SellSignature"].ToString();

                // objBooking.SaveBK_TRANSDTL(bookDTLInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
                lstbookDTLInfo.Add(bookDTLInfo);

                APT += bookDTLInfo.LineTax;
            }
            // end datatable
            #endregion

            //save booking header

            bookHDRInfo.TransID = bookDTLInfo.TransID;

            ht.Add("TransID", bookHDRInfo.TransID);

            bookHDRInfo.TransType = 0;
            bookHDRInfo.AgentID = MyUserSet.AgentID;
            bookHDRInfo.AgentCatgID = MyUserSet.AgentCategoryID;
            bookHDRInfo.BookingDate = DateTime.Now;
            bookHDRInfo.STDDate = Convert.ToDateTime(temFlight.TemFlightStd);
            int sysValue = 0;
            using (profiler.Step("GBS:getSysValueByKeyAndCarrierCodeSTDEXPIRY"))
            {
                string expirySetting = objGeneral.getSysValueByKeyAndCarrierCode("STDEXPIRY", keyCarrier);


                
                if (expirySetting != "")
                {
                    sysValue = Convert.ToInt16(expirySetting);
                }

            }
            //set expirydate after scheme is assign in savebooking 
            //bookHDRInfo.ExpiryDate = departDate.Date.AddDays(-sysValue);

            /*
            string tempdate1 = String.Format("{0:MM/dd/yyyy}", departDate.Date );
            string tempdate2 = String.Format("{0:MM/dd/yyyy}", DateTime.Now);
            TimeSpan ts = Convert.ToDateTime(tempdate1) - Convert.ToDateTime(tempdate2);
            
            int tempday = Convert.ToInt32(ts.TotalDays.ToString());
            if (tempday < 2)
                bookHDRInfo.ExpiryDate = DateTime.Now;
            else
                bookHDRInfo.ExpiryDate = departDate.Date.AddDays(-sysValue);
            */

            ht.Add("Expiry", bookHDRInfo.ExpiryDate);


            //bookHDRInfo.TransTotalPAX = Convert.ToInt16(.Text);
            //change to
            HttpCookie cookie = Request.Cookies["cookieTemFlight"];
            bookHDRInfo.TransTotalPAX = Convert.ToInt16(cookie.Values["PaxNum"].ToString());

            bookHDRInfo.CollectedAmt = 0;

            bookHDRInfo.TransTotalAmt = totalFlightFare;
            bookHDRInfo.TransSubTotal = totalFlightFare;
            bookHDRInfo.TransTotalTax = APT;
            bookHDRInfo.TransTotalFee = totalServiceFee;
            bookHDRInfo.TransTotalOth = totalOth;
            bookHDRInfo.TransTotalDisc = totalDisc;
            bookHDRInfo.TransTotalPromoDisc = totalPromoDisc;

            bookHDRInfo.Currency = Currency;
            bookHDRInfo.CurrencyPaid = Currency;

            bookHDRInfo.TransStatus = 0;
            bookHDRInfo.CreateBy = MyUserSet.AgentID;
            bookHDRInfo.SyncCreate = DateTime.Now;
            bookHDRInfo.SyncLastUpd = DateTime.Now;
            bookHDRInfo.LastSyncBy = MyUserSet.AgentName;

            //load fare

            if (HttpContext.Current.Session["Fare"] != null)
            {
                string a = "";
            }
            Hashtable htFare = (Hashtable)HttpContext.Current.Session["Fare"];
            decimal avg = Convert.ToDecimal(htFare["Avg"]);
            decimal dpt = Convert.ToDecimal(htFare["Dpt"]);
            decimal rtn = Convert.ToDecimal(htFare["Rtn"]);

            //bookHDRInfo.TotalAmtAVG = Convert.ToDecimal(lblAverageFare.Text);
            bookHDRInfo.TotalAmtAVG = avg;

            bookHDRInfo.TotalAmtGoing = dpt;
            int is2Way = Convert.ToInt32(Session["is2Way"].ToString());
            if (is2Way == 1)
            { bookHDRInfo.TotalAmtReturn = rtn; }
            else
            { bookHDRInfo.TotalAmtReturn = 0; }

            /*
            bookHDRInfo.TotalAmtGoing = Convert.ToDecimal(lblDepartFare.Text);
            if (LblReturn.Text != "")
            { bookHDRInfo.TotalAmtReturn = Convert.ToDecimal(lblReturnFare.Text); }
            else
            { bookHDRInfo.TotalAmtReturn = 0; }
            */
            using (profiler.Step("GBS:getSysValueByKeyAndCarrierCodeREMINDDURA1"))
            {
                string reminder = objGeneral.getSysValueByKeyAndCarrierCode("REMINDDURA1", keyCarrier);

                if (reminder != "")
                {
                    sysValue = Convert.ToInt16(reminder);
                }
                //bookHDRInfo.CurReminderDate = bookHDRInfo.ExpiryDate.AddDays(-sysValue);
                using (profiler.Step("GBS:getSysValueByKeyAndCarrierCodeREMINDDURA2"))
                {
                    reminder = objGeneral.getSysValueByKeyAndCarrierCode("REMINDDURA2", keyCarrier);

                    if (reminder != "")
                    {
                        sysValue = Convert.ToInt16(reminder);
                    }
                }
            }
            //bookHDRInfo.NextReminderDate = bookHDRInfo.ExpiryDate.AddDays(-sysValue);
            bookHDRInfo.ReminderType = 1;

            //load max failed payment try
            using (profiler.Step("GBS:getSysValueByKeyAndCarrierCodePAYMENTSUSPEND"))
            {
                string maxPaymentFail = objGeneral.getSysValueByKeyAndCarrierCode("PAYMENTSUSPEND", keyCarrier);
                ht.Add("PaymentSuspend", maxPaymentFail);
            }

            //added by ketee, 20170310, for new booking, set isoverride = 1
            ht.Add("IsOverride", "1");

            HttpContext.Current.Session.Remove("HashMain");
            HttpContext.Current.Session.Add("HashMain", ht);

            //end save header

            //added by ketee
            BookingTransactionMain BookingMain = new BookingTransactionMain();
            //BookingMain = objBooking.SaveBooking(bookHDRInfo, lstbookDTLInfo, transTaxInfo, lsttransFeesInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
            using (profiler.Step("GBS:SaveBooking"))
            {
                BookingMain = objBooking.SaveBooking(bookHDRInfo, lstbookDTLInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
            }
                if (BookingMain != null && BookingMain.TransID != "")
                return true;
            else
                return false;
            //objBooking.SaveBooking(bookHDRInfo, lstbookDTLInfo, transTaxInfo, lsttransFeesInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
        }




    }
}