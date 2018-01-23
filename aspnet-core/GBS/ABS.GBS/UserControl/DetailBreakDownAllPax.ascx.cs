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

using System.Text;
using System.Web.SessionState;
using System.Configuration;
using ABS.GBS.Log;

namespace GroupBooking.Web.UserControl
{
    public partial class DetailBreakDownAllPax : System.Web.UI.UserControl
    {
        #region declaration
        //UserSet MyUserSet;
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogControl log = new LogControl();
        SystemLog SystemLog = new SystemLog();

        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
        ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");

        BookingTransactionDetail bookDTLInfo = new BookingTransactionDetail();
        List<BookingTransactionDetail> lstbookFlightInfo = new List<BookingTransactionDetail>();
        List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
        BookingTransactionMain bookHDRInfo = new BookingTransactionMain();
        ABS.Logic.GroupBooking.Agent.AgentControl objAgent = new ABS.Logic.GroupBooking.Agent.AgentControl();
        ABS.Logic.GroupBooking.Agent.AgentProfileControl objAgentProfile = new ABS.Logic.GroupBooking.Agent.AgentProfileControl();
        ABS.Logic.GroupBooking.Agent.AgentProfile agent = new ABS.Logic.GroupBooking.Agent.AgentProfile();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void panelBreakdown_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            DataTable dtOth = new DataTable();
            if (Session["dataTFOth"] != null)
            {
                dtOth = (DataTable)Session["dataTFOth"];
            }

            HttpCookie cookie = Request.Cookies["cookieTemFlight"];
            if (cookie != null)
            {
                string OutID = "";
                string InID = "";
                int GuestNum = 0;
                int ChildNum = 0;
                int PaxNum = 0;
                OutID = cookie.Values["list1ID"];
                InID = cookie.Values["ReturnID"];
                GuestNum = Convert.ToInt32(cookie.Values["GuestNum"]);
                PaxNum = Convert.ToInt32(cookie.Values["PaxNum"]);
                ChildNum = Convert.ToInt32(cookie.Values["ChildNum"]);

                ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
                ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model2 = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();

                string strExpr;
                string strSort;
                DataTable dt = objBooking.dtFlight();

                strExpr = "TemFlightId = '" + OutID + "'";

                strSort = "";

                dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                if (dt != null)
                {
                    DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);


                    FillModelFromDataRow(foundRows, ref  model);

                    if (InID != "")
                    {
                        strExpr = "TemFlightId = '" + InID + "'";

                        strSort = "";

                        dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                        foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                        FillModelFromDataRow(foundRows, ref  model2);

                    }
                }


                for (int i = 0; i < dtOth.Rows.Count; i++)
                {
                    if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "KLIA2" && dtOth.Rows[i]["Origin"].ToString() == model.TemFlightDeparture.ToString())
                    {
                        //tdDepartKlia2.Visible = true;
                        lbl_klia2Total.Text = Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"]).ToString("N", nfi);

                    }
                    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "GST" && dtOth.Rows[i]["Origin"].ToString() == model.TemFlightDeparture.ToString())
                    {
                        //tdDepartGST.Visible = true;
                        lbl_GSTTotal.Text = Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"]).ToString("N", nfi);

                    }
                    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "AVL" && dtOth.Rows[i]["Origin"].ToString() == model.TemFlightDeparture.ToString())
                    {
                        //tdDepartAVL.Visible = true;
                        lblAVLTotalDepart.Text = Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"]).ToString("N", nfi);

                    }
                    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "PSF" && dtOth.Rows[i]["Origin"].ToString() == model.TemFlightDeparture.ToString())
                    {
                        //tdDepartPSF.Visible = true;
                        lblPSFTotalDepart.Text = Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"]).ToString("N", nfi);

                    }
                    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "SCF" && dtOth.Rows[i]["Origin"].ToString() == model.TemFlightDeparture.ToString())
                    {
                        //tdDepartSCF.Visible = true;
                        lblSCFTotalDepart.Text = Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"]).ToString("N", nfi);

                    }
                    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "Discount" && dtOth.Rows[i]["Origin"].ToString() == model.TemFlightDeparture.ToString())
                    {
                        //tdDepartSCF.Visible = true;
                        lblDiscTotalDepart.Text = Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"]).ToString("N", nfi);

                    }
                    else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "" && dtOth.Rows[i]["FeeType"].ToString().ToUpper() == "ConnectionAdjustmentAmount" && dtOth.Rows[i]["Origin"].ToString() == model.TemFlightDeparture.ToString())
                    {
                        //tdConnectingDepart.Visible = true;
                        lblConnectingDepartTotal.Text = Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"]).ToString("N", nfi);

                    }

                    if (model2 != null)
                    {
                        if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "KLIA2" && dtOth.Rows[i]["Origin"].ToString() == model2.TemFlightDeparture.ToString())
                        {
                            //tdReturnKlia2.Visible = true;
                            lbl_Inklia2Total.Text = Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"]).ToString("N", nfi);

                        }
                        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "GST" && dtOth.Rows[i]["Origin"].ToString() == model2.TemFlightDeparture.ToString())
                        {
                            //tdReturnGST.Visible = true;
                            lbl_InGSTTotal.Text = Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"]).ToString("N", nfi);

                        }
                        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "AVL" && dtOth.Rows[i]["Origin"].ToString() == model2.TemFlightDeparture.ToString())
                        {
                            //tdReturnAVL.Visible = true;
                            lblAVLTotalReturn.Text = Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"]).ToString("N", nfi);

                        }
                        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "PSF" && dtOth.Rows[i]["Origin"].ToString() == model2.TemFlightDeparture.ToString())
                        {
                            //tdReturnPSF.Visible = true;
                            lblPSFTotalReturn.Text = Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"]).ToString("N", nfi);

                        }
                        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "SCF" && dtOth.Rows[i]["Origin"].ToString() == model2.TemFlightDeparture.ToString())
                        {
                            //tdReturnSCF.Visible = true;
                            lblSCFTotalReturn.Text = Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"]).ToString("N", nfi);

                        }
                        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "" && dtOth.Rows[i]["FeeType"].ToString().ToUpper() == "ConnectionAdjustmentAmount" && dtOth.Rows[i]["Origin"].ToString() == model2.TemFlightDeparture.ToString())
                        {
                            //tdConnectingReturn.Visible = true;
                            lblConnectingReturnTotal.Text = Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"]).ToString("N", nfi);

                        }
                        else if (dtOth.Rows[i]["FeeCode"].ToString().ToUpper() == "Discount" && dtOth.Rows[i]["Origin"].ToString() == model2.TemFlightDeparture.ToString())
                        {
                            //tdDepartSCF.Visible = true;
                            lblDiscTotalReturn.Text = Convert.ToDecimal(dtOth.Rows[i]["FeeAmt"]).ToString("N", nfi);

                        }
                    }
                }

                lbl_currency2.Text = model.TemFlightCurrencyCode;
                lbl_currency21.Text = model.TemFlightCurrencyCode;
                lblCurrAVLDepart.Text = model.TemFlightCurrencyCode;
                lblCurrPSFDepart.Text = model.TemFlightCurrencyCode;
                lblCurrSCFDepart.Text = model.TemFlightCurrencyCode;
                lblCurrConnectingDepart.Text = model.TemFlightCurrencyCode;
                lblCurrDiscDepart.Text = model.TemFlightCurrencyCode;
                lbl_Incurrency2.Text = model.TemFlightCurrencyCode;
                lbl_Incurrency21.Text = model.TemFlightCurrencyCode;
                lblCurrAVLReturn.Text = model.TemFlightCurrencyCode;
                lblCurrPSFReturn.Text = model.TemFlightCurrencyCode;
                lblCurrSCFReturn.Text = model.TemFlightCurrencyCode;
                lblCurrConnectingReturn.Text = model.TemFlightCurrencyCode;
                lblCurrDiscReturn.Text = model.TemFlightCurrencyCode;

                if (lbl_klia2Total.Text == "0.00" && lbl_Inklia2Total.Text == "0.00")
                {
                    trKlia2.Visible = false;
                }
                if (lbl_GSTTotal.Text == "0.00" && lbl_InGSTTotal.Text == "0.00")
                {
                    trGST.Visible = false;
                }
                if (lblAVLTotalDepart.Text == "0.00" && lblAVLTotalReturn.Text == "0.00")
                {
                    trAVL.Visible = false;
                }
                if (lblPSFTotalDepart.Text == "0.00" && lblPSFTotalReturn.Text == "0.00")
                {
                    trPSF.Visible = false;
                }
                if (lblSCFTotalDepart.Text == "0.00" && lblSCFTotalReturn.Text == "0.00")
                {
                    trSCF.Visible = false;
                }
                if (lblConnectingDepartTotal.Text == "0.00" && lblConnectingReturnTotal.Text == "0.00")
                {
                    trConnecting.Visible = false;
                }
                if (lblDiscTotalReturn.Text == "0.00" && lblDiscTotalDepart.Text == "0.00")
                {
                    trDepartDiscount.Visible = false;
                }

                if (model2 == null)
                {
                    tdReturnKlia2.Visible = false;
                    tdReturnGST.Visible = false;
                    tdReturnAVL.Visible = false;
                    tdReturnPSF.Visible = false;
                    tdReturnSCF.Visible = false;
                    tdConnectingReturn.Visible = false;
                    tdiscReturn.Visible = false;
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
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
        }

        public static bool IsNumeric(string s)
        {
            double result;
            return s != null && Double.TryParse(s, out result);
        }
    }
}