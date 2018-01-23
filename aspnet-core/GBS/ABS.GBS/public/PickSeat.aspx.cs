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

namespace ABS.GBS
{
    public partial class PickSeat : System.Web.UI.Page
    {
        #region "Declaration"
        UserSet MyUserSet;
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogControl log = new LogControl();

        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
        ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");

        BookingTransactionDetail bookDTLInfo = new BookingTransactionDetail();
        List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
        BookingTransactionMain bookHDRInfo = new BookingTransactionMain();
        ABS.Logic.GroupBooking.Agent.AgentControl objAgent = new ABS.Logic.GroupBooking.Agent.AgentControl();
        ABS.Logic.GroupBooking.Agent.AgentProfileControl objAgentProfile = new ABS.Logic.GroupBooking.Agent.AgentProfileControl();
        decimal totalFlightFare, totalServiceFee, totalBaggageFare = 0, totalServVAT;
        ABS.Logic.GroupBooking.Agent.AgentProfile agent = new ABS.Logic.GroupBooking.Agent.AgentProfile();

        string Currency = "USD";
        decimal APT = 0;
        decimal AVGFare = 0;
        string tranID = "";
        DataTable dtTaxFees = new DataTable();

        private static int num = 0;
        private static int departID = 0;
        private static string ReturnID = "";
        DataTable dtTransMain;
        string TransId;

        EnumFlight eFlight;

        ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
        ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model2 = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();

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
            ReturnConnectingFlight = 4
        }
        #endregion

        #region "Load Event"
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Expires = 0;
            Response.Cache.SetNoStore();
            Response.AppendHeader("Pragma", "no-cache");

            ss.numberofpassenger = 2;
            ss.overwritepassengerindex = 2;
            ss.xmlurl = "../XML/effecad8-6146-443f-bc2d-f61400e37b00.xml";
            ss.xmlurl = "../GetSeatAvailability_Response";
            if (!Page.IsPostBack)
            {
                ss.defaultseat = "0_Y_1_2D,0_Y_1_2E";
            }

            return;
            try
            {
                InitializeForm();
                if (!SetAccess())
                {
                    return;
                }
                if (IsPostBack == false)
                {
                    intSeatTabSession();
                    if (Session["btnSelected"] == null)
                    {
                        Session["btnSelected"] = 0;
                    }
                }

                FillFlight(model.TemFlightJourneySellKey, (int)Session["btnSelected"]);

            }
            catch (Exception ex)
            {
                //sTraceLog(ex.ToString);
                Response.Redirect("~/page/agentmain.aspx");
            }
        }
        #endregion

        #region "Controls"

        #endregion

        #region "Function & Procedure"
        protected void InitializeForm()
        {
            try
            {
                if (Session["TransID"] != null)
                {
                    TransId = (string)Session["TransID"];
                }

                SetCookie();
                BindModel();
            }
            catch (Exception ex)
            {
                //log.Error(this, ex);
            }
        }
        private bool SetAccess()
        {
            try
            {
                if (model != null)
                {
                    if (model.TemFlightJourneySellKey.Contains("^"))
                    {
                        eFlight = EnumFlight.ConnectingFlight;
                    }
                    else
                    {
                        eFlight = EnumFlight.DirectFlight;
                    }
                }
                else if (model2 != null)
                {
                    if (model2.TemFlightJourneySellKey.Contains("^"))
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
                return true;
            }
            catch (Exception ex)
            {
                return false;
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

        private void BindModel()
        {
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
                model.TemFlightOpSuffix = foundRows[0]["TemFlightOpSuffix"].ToString();
                model.TemFlightOpSuffix2 = foundRows[0]["TemFlightOpSuffix2"].ToString();
                model.TemFlightSignature = foundRows[0]["TemFlightSignature"].ToString();
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
            }
        }

        private void intSeatTabSession()
        {
            switch (eFlight)
            {
                case EnumFlight.ConnectingFlight:
                    if (model == null == false)
                    {
                        if (CheckDefaultSeatExist(EnumFlightType.DepartConnectingFlight))
                        {
                            btnSeatDepart1.Value = model.TemFlightDeparture + " - " + model.TemFlightTransit;
                            btnSeatDepart2.Value = model.TemFlightTransit + " - " + model.TemFlightArrival;
                            btnSeatDepart1.Visible = true;
                            btnSeatDepart2.Visible = true;
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
                    if (model2 == null == false)
                    {
                        if (CheckDefaultSeatExist(EnumFlightType.ReturnConnectingFlight))
                        {
                            btnSeatReturn1.Value = model2.TemFlightDeparture + " - " + model2.TemFlightTransit;
                            btnSeatReturn2.Value = model2.TemFlightTransit + " - " + model2.TemFlightArrival;
                            btnSeatReturn1.Visible = true;
                            btnSeatReturn2.Visible = true;
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
                    if (model == null == false)
                    {
                        if (CheckDefaultSeatExist(EnumFlightType.DepartFlight))
                        {
                            btnSeatDepart1.Value = model.TemFlightDeparture + " - " + model.TemFlightArrival;
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
                    if (model2 == null == false)
                    {
                        if (CheckDefaultSeatExist(EnumFlightType.ReturnFlight))
                        {
                            btnSeatDepart2.Value = model2.TemFlightDeparture + " - " + model2.TemFlightArrival;
                            btnSeatDepart2.Visible = true;
                        }
                        else
                        {
                            btnSeatDepart2.Visible = false;
                        }


                    }
                    else
                    {
                        btnSeatDepart2.Visible = false;
                    }
                    btnSeatReturn1.Visible = false;
                    btnSeatReturn2.Visible = false;
                    break;
            }

        }

        private void FillFlight(string RecordLocator, int btnSelected = 0)
        {
            ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");

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
                string FlightCode = "";
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

                string Signature = (string)Session["signature"];

                switch (eFlight)
                {
                    case EnumFlight.DirectFlight:

                        if ((model != null))
                        {
                            //pDepartFlightInfo = Session["DepartFlightInfo"];
                            //pReturnFlightInfo = Session["ReturnFlightInfo"];
                            //If pDepartFlightInfo.XmlURL <> "" Then
                            //    DepartXml = pDepartFlightInfo.XmlURL
                            //    DepartPax = pDepartFlightInfo.Pax
                            //    DepartFromToShort = pDepartFlightInfo.FromToShort
                            //Else

                            if (btnSelected == 0)
                            {
                                if (Session["SeatInfo0Xml"] == null == false)
                                {
                                    DeleteXML((string)Session["SeatInfo0Xml"]);

                                }
                                pAvailableSeatInfo = absNavitaire.GetSeatAvailability(RecordLocator, 0, Signature, model.TemFlightCarrierCode, model.TemFlightFlightNumber,
                                    model.TemFlightOpSuffix, model.TemFlightDeparture, model.TemFlightArrival, (DateTime)model.TemFlightStd, Request.PhysicalApplicationPath);
                                //Session["Click"] = Nothing
                                Session["pAvailableSeatInfo0"] = pAvailableSeatInfo;
                                btnSeatDepart1.Style[HtmlTextWriterStyle.BackgroundImage] = "../images/CBackground2.png";
                            }
                            else if (btnSelected == 1)
                            {
                                if (Session["SeatInfo1Xml"] == null == false)
                                {
                                    DeleteXML((string)Session["SeatInfo1Xml"]);

                                }
                                pAvailableSeatInfo = absNavitaire.GetSeatAvailability(RecordLocator, 0, Signature, model2.TemFlightCarrierCode, model2.TemFlightFlightNumber,
                                    model2.TemFlightOpSuffix, model2.TemFlightDeparture, model2.TemFlightArrival, (DateTime)model2.TemFlightStd, Request.PhysicalApplicationPath);
                                //Session["Click"] = Nothing
                                Session["pAvailableSeatInfo1"] = pAvailableSeatInfo;
                                btnSeatDepart2.Style[HtmlTextWriterStyle.BackgroundImage] = "../images/CBackground2.png";
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
                                        DepartPax = model.TemFlightPaxNum;
                                        DepartFromTo = model.TemFlightDeparture + "-" + model.TemFlightArrival;
                                        //DepartFromTo2 = pDepartFlightInfo.FromTo2
                                        DepartFromToShort = model.TemFlightDeparture + "-" + model.TemFlightArrival;
                                        //DepartFromToShort2 = pDepartFlightInfo.FromToShort2
                                    }
                                    else
                                    {
                                        ////pReturnFlightInfo.XmlURL = DepartXml;
                                        //DepartXml = "test4.xml"
                                        DepartPax = model2.TemFlightPaxNum;
                                        ReturnFromTo = model2.TemFlightDeparture + "-" + model2.TemFlightArrival;
                                        //DepartFromTo2 = pDepartFlightInfo.FromTo2
                                        ReturnFromToShort = model2.TemFlightDeparture + "-" + model2.TemFlightArrival;
                                        //DepartFromToShort2 = pDepartFlightInfo.FromToShort2
                                    }

                                }
                            }
                            else
                            {
                                lblErr.Text = "Record not found.";
                                pnlErr.Visible = true;
                                return;
                            }
                            //End If


                        }
                        else
                        {
                            //if ((Session["ReturnFlightInfo"] != null))
                            //{
                            //    //pReturnFlightInfo = Session["ReturnFlightInfo"];
                            //    if (Session["SeatInfo1Xml"] == null == false)
                            //    {
                            //        DeleteXML((string)Session["SeatInfo1Xml"]);

                            //    }
                            //    pAvailableSeatInfo = absNavitaire.GetSeatAvailability(RecordLocator, 0, pReturnFlightInfo, Common.EnumFlightType.ReturnFlight);
                            //    //Session["Click"] = Nothing
                            //    Session["pAvailableSeatInfo1"] = pAvailableSeatInfo;
                            //    btnSeatDepart2.Style(HtmlTextWriterStyle.BackgroundImage) = "images/CBackground2.png";

                            //    if ((pAvailableSeatInfo != null))
                            //    {
                            //        if (btnSeatDepart1.Visible == false)
                            //        {
                            //            btnSelected = 1;
                            //        }
                            //        if (!string.IsNullOrEmpty(API.DepartXmlUrl))
                            //        {
                            //            DepartXml = API.DepartXmlUrl;
                            //            if (btnSelected == 0)
                            //            {
                            //                pDepartFlightInfo.XmlURL = DepartXml;
                            //                //DepartXml = "test4.xml"
                            //                DepartPax = pDepartFlightInfo.Pax;
                            //                DepartFromTo = pDepartFlightInfo.FromTo;
                            //                //DepartFromTo2 = pDepartFlightInfo.FromTo2
                            //                DepartFromToShort = pDepartFlightInfo.FromToShort;
                            //                //DepartFromToShort2 = pDepartFlightInfo.FromToShort2
                            //            }
                            //            else
                            //            {
                            //                pReturnFlightInfo.XmlURL = DepartXml;
                            //                //DepartXml = "test4.xml"
                            //                DepartPax = pReturnFlightInfo.Pax;
                            //                ReturnFromTo = pReturnFlightInfo.FromTo;
                            //                //DepartFromTo2 = pDepartFlightInfo.FromTo2
                            //                ReturnFromToShort = pReturnFlightInfo.FromToShort;
                            //                //DepartFromToShort2 = pDepartFlightInfo.FromToShort2
                            //            }
                            //        }
                            //    }
                            //    else
                            //    {
                            //        lblErr.Text = "Record not found.";
                            //        pnlErr.Visible = true;
                            //        return;
                            //    }
                            //}
                        }
                        if (!string.IsNullOrEmpty(DepartXml))
                        {
                            ////ctlDepart.Style["display"] = "";
                            PassengerHeader = "<div id=\"passengerListHeader" + btnSelected + " class=\"redSectionHeader\">";
                            PassengerHeader += "<div>Seat summary</div></div>";
                            PassengerHeader += "<div id=\"passengerListBody" + btnSelected + " class=\"sectionBody\"><br/>";
                            if (btnSelected == 0)
                            {
                                PassengerSum = CreateSeatControl(DepartPax, DepartFromToShort, AssignSeatInfo(EnumFlightType.DepartFlight, RecordLocator, (SeatAvailabilityResponse)Session["pAvailableSeatInfo0"]), EnumFlightType.DepartFlight);
                            }
                            else if (btnSelected == 1)
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
                            else if (btnSelected == 1)
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
                                        //DepartDefaultSeat = "0_" & RowSeatInfo.CompartmentDesignator & "_" & RowSeatInfo.Deck & "_" & RowSeatInfo.SelectedSeat
                                        DepartDefaultSeat = "0_" + RowSeatInfo.CompartmentDesignator + "_" + RowSeatInfo.Deck + "_";
                                        //added by ketee hot seat
                                        hotseat = RowSeatInfo.IsHotSeat.ToString();
                                    }
                                    else
                                    {
                                        //DepartDefaultSeat &= ",0_" & RowSeatInfo.CompartmentDesignator & "_" & RowSeatInfo.Deck & "_" & RowSeatInfo.SelectedSeat
                                        DepartDefaultSeat += ",0_" + RowSeatInfo.CompartmentDesignator + "_" + RowSeatInfo.Deck + "_";
                                        //added by ketee
                                        hotseat += "," + RowSeatInfo.IsHotSeat;
                                    }
                                }
                            }
                            ss.defaultseat = DepartDefaultSeat;
                            //'remark by ketee, no default seat assign
                            //ss.hotseat = hotseat;
                            ss.numberofpassenger = DepartPax;
                            ss.overwritepassengerindex = DepartPax;
                            ss.xmlurl = "..\\" + DepartXml;
                        }

                        break;
                        ////case Common.EnumFlight.ConnectingFlight:
                        ////    if (btnSeatDepart1.Visible == false & btnSeatDepart2.Visible == false)
                        ////    {
                        ////        if (btnSelected == 0)
                        ////        {
                        ////            btnSelected = 2;
                        ////        }
                        ////    }
                        ////    if (btnSelected < 2)
                        ////    {

                        ////        if ((Session["DepartFlightInfo"] != null))
                        ////        {
                        ////            pDepartFlightInfo = Session["DepartFlightInfo"];

                        ////            if (btnSelected == 0)
                        ////            {
                        ////                if (Session["SeatInfo0Xml"] == null == false)
                        ////                {
                        ////                    API.DeleteXML(Session["SeatInfo0Xml"]);

                        ////                }
                        ////                pAvailableSeatInfo = API.GetSeatAvailability(RecordLocator, 0, pDepartFlightInfo, Common.EnumFlightType.DepartConnectingFlight);
                        ////                //Session["Click"] = Nothing
                        ////                Session["pAvailableSeatInfo0"] = pAvailableSeatInfo;
                        ////                btnSeatDepart1.Style(HtmlTextWriterStyle.BackgroundImage) = "images/CBackground2.png";
                        ////            }
                        ////            else if (btnSelected == 1)
                        ////            {
                        ////                if (Session["SeatInfo1Xml"] == null == false)
                        ////                {
                        ////                    API.DeleteXML(Session["SeatInfo1Xml"]);

                        ////                }
                        ////                pAvailableSeatInfo = API.GetSeatAvailability2(RecordLocator, 0, pDepartFlightInfo, Common.EnumFlightType.DepartConnectingFlight);
                        ////                //Session["Click"] = Nothing
                        ////                Session["pAvailableSeatInfo1"] = pAvailableSeatInfo;
                        ////                btnSeatDepart2.Style(HtmlTextWriterStyle.BackgroundImage) = "images/CBackground2.png";
                        ////            }


                        ////            if ((pAvailableSeatInfo != null))
                        ////            {

                        ////                if (!string.IsNullOrEmpty(API.DepartXmlUrl))
                        ////                {
                        ////                    DepartXml = API.DepartXmlUrl;
                        ////                    pDepartFlightInfo.XmlURL = DepartXml;
                        ////                    //DepartXml = "test4.xml"
                        ////                    DepartPax = pDepartFlightInfo.Pax;
                        ////                    DepartFromTo = pDepartFlightInfo.FromTo;
                        ////                    DepartFromTo2 = pDepartFlightInfo.FromTo2;
                        ////                    DepartFromToShort = pDepartFlightInfo.FromToShort;
                        ////                    DepartFromToShort2 = pDepartFlightInfo.FromToShort2;
                        ////                }
                        ////            }
                        ////            else
                        ////            {
                        ////                lblErr.Text = "Record not found.";
                        ////                pnlErr.Visible = true;
                        ////                return;
                        ////            }
                        ////            //End If



                        ////        }
                        ////        else
                        ////        {
                        ////        }


                        ////        if (!string.IsNullOrEmpty(DepartXml))
                        ////        {
                        ////            ctlDepart.Style.Item("display") = "";


                        ////            PassengerHeader = "<div id=\"passengerListHeader" + btnSelected + " class=\"redSectionHeader\">";
                        ////            PassengerHeader += "<div>Seat summary</div></div>";
                        ////            PassengerHeader += "<div id=\"passengerListBody" + btnSelected + " class=\"sectionBody\"><br/>";
                        ////            if (btnSelected == 0)
                        ////            {
                        ////                PassengerSum = CreateSeatControl(DepartPax, DepartFromToShort, AssignSeatInfo(Common.EnumFlightType.DepartFlight, RecordLocator, Session["pAvailableSeatInfo0"]), Common.EnumFlightType.DepartFlight);
                        ////            }
                        ////            else if (btnSelected == 1)
                        ////            {
                        ////                PassengerSum = CreateSeatControl(DepartPax, DepartFromToShort2, AssignSeatInfo(Common.EnumFlightType.DepartConnectingFlight, RecordLocator, Session["pAvailableSeatInfo1"]), Common.EnumFlightType.DepartConnectingFlight);
                        ////            }

                        ////            if (!string.IsNullOrEmpty(PassengerSum))
                        ////            {
                        ////                PassengerSummary.Style.Item("display") = "";
                        ////                PassengerSummary.InnerHtml = PassengerHeader + PassengerSum + "</div>";
                        ////            }

                        ////            if (btnSelected == 0)
                        ////            {
                        ////                ss.SeatInfo = Session["SeatInfo0"];

                        ////                Session["SeatInfo0Xml"] = DepartXml;
                        ////            }
                        ////            else if (btnSelected == 1)
                        ////            {
                        ////                ss.SeatInfo = Session["SeatInfo1"];

                        ////                Session["SeatInfo1Xml"] = DepartXml;
                        ////            }
                        ////            if ((ss.SeatInfo != null))
                        ////            {
                        ////                foreach (void RowSeatInfo_loopVariable in ss.SeatInfo)
                        ////                {
                        ////                    RowSeatInfo = RowSeatInfo_loopVariable;
                        ////                    if (string.IsNullOrEmpty(DepartDefaultSeat))
                        ////                    {
                        ////                        //DepartDefaultSeat = "0_" & RowSeatInfo.CompartmentDesignator & "_1_" & RowSeatInfo.SelectedSeat
                        ////                        DepartDefaultSeat = "0_" + RowSeatInfo.CompartmentDesignator + "_" + RowSeatInfo.Deck + "_";
                        ////                        //added by ketee
                        ////                        hotseat = RowSeatInfo.IsHotSeat;
                        ////                    }
                        ////                    else
                        ////                    {
                        ////                        //DepartDefaultSeat &= ",0_" & RowSeatInfo.CompartmentDesignator & "_1_" & RowSeatInfo.SelectedSeat
                        ////                        DepartDefaultSeat += ",0_" + RowSeatInfo.CompartmentDesignator + "_" + RowSeatInfo.Deck + "_";
                        ////                        //added by ketee
                        ////                        hotseat += "," + RowSeatInfo.IsHotSeat;
                        ////                    }
                        ////                }
                        ////            }
                        ////            ss.defaultseat = DepartDefaultSeat;
                        ////            //'remark by ketee, no default seat assign
                        ////            ss.hotseat = hotseat;
                        ////            ss.numberofpassenger = DepartPax;
                        ////            ss.overwritepassengerindex = DepartPax;
                        ////            ss.xmlurl = DepartXml;


                        ////        }

                        ////    }
                        ////    else if (btnSelected > 1)
                        ////    {
                        ////        if ((Session["ReturnFlightInfo"] != null))
                        ////        {
                        ////            pReturnFlightInfo = Session["ReturnFlightInfo"];
                        ////            if (btnSelected == 2)
                        ////            {
                        ////                if (Session["SeatInfo2Xml"] == null == false)
                        ////                {
                        ////                    API.DeleteXML(Session["SeatInfo2Xml"]);

                        ////                }
                        ////                pAvailableSeatInfo = API.GetSeatAvailability(RecordLocator, 0, pReturnFlightInfo, Common.EnumFlightType.ReturnConnectingFlight);
                        ////                //Session["Click"] = Nothing
                        ////                Session["pAvailableSeatInfo2"] = pAvailableSeatInfo;
                        ////                btnSeatReturn1.Style(HtmlTextWriterStyle.BackgroundImage) = "images/CBackground2.png";
                        ////            }
                        ////            else if (btnSelected == 3)
                        ////            {
                        ////                if (Session["SeatInfo3Xml"] == null == false)
                        ////                {
                        ////                    API.DeleteXML(Session["SeatInfo3Xml"]);

                        ////                }
                        ////                pAvailableSeatInfo = API.GetSeatAvailability2(RecordLocator, 0, pReturnFlightInfo, Common.EnumFlightType.ReturnConnectingFlight);
                        ////                //Session["Click"] = Nothing
                        ////                Session["pAvailableSeatInfo3"] = pAvailableSeatInfo;
                        ////                btnSeatReturn2.Style(HtmlTextWriterStyle.BackgroundImage) = "images/CBackground2.png";
                        ////            }

                        ////            if ((pAvailableSeatInfo != null))
                        ////            {
                        ////                if (!string.IsNullOrEmpty(API.DepartXmlUrl))
                        ////                {
                        ////                    ReturnXml = API.DepartXmlUrl;
                        ////                    pReturnFlightInfo.XmlURL = ReturnXml;
                        ////                    //DepartXml = "test4.xml"
                        ////                    ReturnPax = pReturnFlightInfo.Pax;
                        ////                    ReturnFromTo = pReturnFlightInfo.FromTo;
                        ////                    ReturnFromTo2 = pDepartFlightInfo.FromTo2;
                        ////                    ReturnFromToShort = pReturnFlightInfo.FromToShort;
                        ////                    ReturnFromToShort2 = pReturnFlightInfo.FromToShort2;
                        ////                }
                        ////                else
                        ////                {
                        ////                    return;
                        ////                }
                        ////            }
                        ////            else
                        ////            {
                        ////                lblErr.Text = "Record not found.";
                        ////                pnlErr.Visible = true;
                        ////                return;
                        ////            }
                        ////        }

                        ////        if (!string.IsNullOrEmpty(ReturnXml))
                        ////        {
                        ////            ctlDepart.Style.Item("display") = "";


                        ////            PassengerHeader = "<div id=\"passengerListHeader" + btnSelected + " class=\"redSectionHeader\">";
                        ////            PassengerHeader += "<div>Seat summary</div></div>";
                        ////            PassengerHeader += "<div id=\"passengerListBody" + btnSelected + "class=\"sectionBody\"><br/>";
                        ////            if (btnSelected == 2)
                        ////            {
                        ////                PassengerSum = CreateSeatControl(ReturnPax, ReturnFromToShort, AssignSeatInfo(Common.EnumFlightType.ReturnFlight, RecordLocator, Session["pAvailableSeatInfo2"]), Common.EnumFlightType.ReturnFlight);
                        ////            }
                        ////            else if (btnSelected == 3)
                        ////            {
                        ////                PassengerSum = CreateSeatControl(ReturnPax, ReturnFromToShort2, AssignSeatInfo(Common.EnumFlightType.ReturnConnectingFlight, RecordLocator, Session["pAvailableSeatInfo3"]), Common.EnumFlightType.ReturnConnectingFlight);
                        ////            }

                        ////            if (!string.IsNullOrEmpty(PassengerSum))
                        ////            {
                        ////                PassengerSummary.Style.Item("display") = "";
                        ////                PassengerSummary.InnerHtml = PassengerHeader + PassengerSum + "</div>";
                        ////            }

                        ////            if (btnSelected == 2)
                        ////            {
                        ////                ss.SeatInfo = Session["SeatInfo2"];

                        ////                Session["SeatInfo2Xml"] = ReturnXml;
                        ////            }
                        ////            else if (btnSelected == 3)
                        ////            {
                        ////                ss.SeatInfo = Session["SeatInfo3"];

                        ////                Session["SeatInfo3Xml"] = ReturnXml;
                        ////            }
                        ////            if ((ss.SeatInfo != null))
                        ////            {
                        ////                foreach (void RowSeatInfo_loopVariable in ss.SeatInfo)
                        ////                {
                        ////                    RowSeatInfo = RowSeatInfo_loopVariable;
                        ////                    if (string.IsNullOrEmpty(ReturnDefaultSeat))
                        ////                    {
                        ////                        ReturnDefaultSeat = "0_" + RowSeatInfo.CompartmentDesignator + "_" + RowSeatInfo.Deck + "_";
                        ////                    }
                        ////                    else
                        ////                    {
                        ////                        ReturnDefaultSeat += ",0_" + RowSeatInfo.CompartmentDesignator + "_" + RowSeatInfo.Deck + "_";
                        ////                    }
                        ////                }
                        ////            }
                        ////            ss.defaultseat = ReturnDefaultSeat;
                        ////            //'remark by ketee, no default seat assign
                        ////            ss.hotseat = hotseat;
                        ////            ss.numberofpassenger = ReturnPax;
                        ////            ss.overwritepassengerindex = ReturnPax;
                        ////            ss.xmlurl = ReturnXml;

                        ////        }
                        ////    }
                        ////    break;
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
                //sTraceLog(ex.ToString);
                Response.Redirect("~/pages/InvalidPage.aspx");
            }
        }

        private List<ABS.Logic.GroupBooking.SeatInfo> AssignSeatInfo(EnumFlightType flightType, string recordLocator, SeatAvailabilityResponse pAvailableSeatInfo)
        {
            List<ABS.Logic.GroupBooking.SeatInfo> seatInfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
            ABS.Logic.GroupBooking.SeatInfo seatInfo1 = new ABS.Logic.GroupBooking.SeatInfo();
            DataTable dtPassenger = default(DataTable);
            ////AirAsia.Logic.FI_Booking_Logic FI_Booking_Logic = new AirAsia.Logic.FI_Booking_Logic();
            //dtPassenger = FI_Booking_Logic.getAllBookingPassenger(recordLocator);
            dtPassenger = objBooking.GetAllBK_PASSENGERLISTInitDataTable(TransId, true);

            int i = 0;

            switch (eFlight)
            {
                case EnumFlight.ConnectingFlight:
                    switch (flightType)
                    {
                        case EnumFlightType.DepartFlight:
                            if ((dtPassenger != null))
                            {
                                foreach (DataRow drRow in dtPassenger.Rows)
                                {
                                    //drRow = drRow_loopVariable;
                                    seatInfo1 = new ABS.Logic.GroupBooking.SeatInfo();
                                    seatInfo1.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                    seatInfo1.PassengerID = drRow["PassengerID"].ToString();
                                    seatInfo1.PassengerNumber = Convert.ToInt32(drRow["PassengerID"].ToString());
                                    seatInfo1.Seq = i;

                                    ////seatInfo1.SelectedSeat = getPassengerDefaultSeat(recordLocator, _QueueCode, seatInfo1.PassengerID, EnumFlightType.DepartFlight, Session["SeatInfo0"], seatInfo1.CompartmentDesignator, seatInfo1.Deck, seatInfo1.SeatSet, seatInfo1.IsHotSeat, i);
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
                                    i = i + 1;
                                }
                            }
                            else
                            {
                                return null;
                            }

                            ss.SeatInfo = seatInfo;
                            Session["SeatInfo0"] = seatInfo;
                            break;
                        case EnumFlightType.DepartConnectingFlight:
                            if ((dtPassenger != null))
                            {
                                foreach (DataRow drRow in dtPassenger.Rows)
                                {
                                    //drRow = drRow_loopVariable;
                                    seatInfo1 = new ABS.Logic.GroupBooking.SeatInfo();
                                    seatInfo1.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                    seatInfo1.PassengerID = drRow["PassengerID"].ToString();
                                    seatInfo1.PassengerNumber = Convert.ToInt32(drRow["PassengerID"].ToString());
                                    seatInfo1.Seq = i;

                                    ////seatInfo1.SelectedSeat = getPassengerDefaultSeat(recordLocator, _QueueCode, seatInfo1.PassengerID, Common.EnumFlightType.DepartConnectingFlight, Session["SeatInfo1"], seatInfo1.CompartmentDesignator, seatInfo1.Deck, seatInfo1.SeatSet, seatInfo1.IsHotSeat, i);
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
                                    i = i + 1;
                                }
                            }
                            else
                            {
                                return null;
                            }

                            ss.SeatInfo = seatInfo;
                            Session["SeatInfo1"] = seatInfo;
                            break;
                        case EnumFlightType.ReturnFlight:
                            if ((dtPassenger != null))
                            {
                                foreach (DataRow drRow in dtPassenger.Rows)
                                {
                                    //drRow = drRow_loopVariable;
                                    seatInfo1 = new ABS.Logic.GroupBooking.SeatInfo();
                                    seatInfo1.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                    seatInfo1.PassengerID = drRow["PassengerID"].ToString();
                                    seatInfo1.PassengerNumber = Convert.ToInt32(drRow["PassengerID"].ToString());
                                    seatInfo1.Seq = i;
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
                                    i = i + 1;
                                }

                            }
                            else
                            {
                                return null;
                            }

                            ss.SeatInfo = seatInfo;
                            Session["SeatInfo2"] = seatInfo;
                            break;
                        case EnumFlightType.ReturnConnectingFlight:
                            if ((dtPassenger != null))
                            {
                                foreach (DataRow drRow in dtPassenger.Rows)
                                {
                                    //drRow = drRow_loopVariable;
                                    seatInfo1 = new ABS.Logic.GroupBooking.SeatInfo();
                                    seatInfo1.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                    seatInfo1.PassengerID = drRow["PassengerID"].ToString();
                                    seatInfo1.PassengerNumber = Convert.ToInt32(drRow["PassengerID"].ToString());
                                    seatInfo1.Seq = i;
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
                                    i = i + 1;
                                }
                            }
                            else
                            {
                                return null;
                            }

                            ss.SeatInfo = seatInfo;
                            Session["SeatInfo3"] = seatInfo;
                            break;
                    }
                    break;
                case EnumFlight.DirectFlight:
                    switch (flightType)
                    {
                        case EnumFlightType.DepartFlight:
                            if ((dtPassenger != null))
                            {
                                foreach (DataRow drRow in dtPassenger.Rows)
                                {
                                    //drRow = drRow_loopVariable;
                                    seatInfo1 = new ABS.Logic.GroupBooking.SeatInfo();
                                    seatInfo1.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                    seatInfo1.PassengerID = drRow["PassengerID"].ToString();
                                    seatInfo1.PassengerNumber = Convert.ToInt32(drRow["PassengerID"].ToString());
                                    seatInfo1.Seq = i;

                                    ////seatInfo1.SelectedSeat = getPassengerDefaultSeat(recordLocator, _QueueCode, seatInfo1.PassengerID, Common.EnumFlightType.DepartFlight, Session["SeatInfo0"], seatInfo1.CompartmentDesignator, seatInfo1.Deck, seatInfo1.SeatSet, seatInfo1.IsHotSeat, i);

                                    seatInfo.Add(seatInfo1);
                                    i = i + 1;
                                }
                            }
                            else
                            {
                                return null;
                            }

                            ss.SeatInfo = seatInfo;
                            Session["SeatInfo0"] = seatInfo;
                            break;

                        case EnumFlightType.ReturnFlight:
                            if ((dtPassenger != null))
                            {
                                foreach (DataRow drRow in dtPassenger.Rows)
                                {
                                    //drRow = drRow_loopVariable;
                                    seatInfo1 = new ABS.Logic.GroupBooking.SeatInfo();
                                    seatInfo1.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                    seatInfo1.PassengerID = drRow["PassengerID"].ToString();
                                    seatInfo1.PassengerNumber = Convert.ToInt32(drRow["PassengerID"].ToString());
                                    seatInfo1.Seq = i;
                                    ////seatInfo1.SelectedSeat = getPassengerDefaultSeat(recordLocator, _QueueCode, seatInfo1.PassengerID, Common.EnumFlightType.ReturnFlight, Session["SeatInfo1"], seatInfo1.CompartmentDesignator, seatInfo1.Deck, seatInfo1.SeatSet, seatInfo1.IsHotSeat, i);

                                    seatInfo.Add(seatInfo1);
                                    i = i + 1;
                                }

                            }
                            else
                            {
                                return null;
                            }

                            ss.SeatInfo = seatInfo;
                            Session["SeatInfo1"] = seatInfo;
                            break;

                    }
                    break;
            }



            return seatInfo;
        }

        private string CreateSeatControl(int Pax, string FromTo, List<ABS.Logic.GroupBooking.SeatInfo> _seatInfo, EnumFlightType Type)
        {
            try
            {
                string str = "";
                int i = 0;
                string seatbox = "";
                string selectedBox = "";
                seatbox = "BPassengerNumber_";
                selectedBox = "APassengerNumber_";

                str += "<table class=\"clearTableHeaders\"><tbody><tr class=\"market\">";
                str += "<th scope=\"col\">" + FromTo + "</th><th scope=\"col\"></th></tr>";

                foreach (ABS.Logic.GroupBooking.SeatInfo seatInfo in _seatInfo)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        //if (!string.IsNullOrEmpty(seatInfo.SelectedSeat))
                        {
                            str += "<tr><td class=\"passenger\">" + seatInfo.PaxName + "</td>";
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
                            str += "</tr><tr><td style=\"vertical-align:top;\">";
                            str += "<input id=\"" + selectedBox + seatInfo.Seq + "_Reselect\" type=\"button\" class=\"button_1\" value=\"Reselect\">";
                            //str &= "<input id=""APassengerNumber_" & seatInfo.Seq & "_Remove"" type=""button"" class=""button_1"" value=""Remove""></td>"
                            str += "</td><td style=\"vertical-align:top;\">" + (seatInfo.IsHotSeat == 1 ? "<img src=\"../images/JetAircraft_NS_Open_0_HS.gif\" class=\"unitGroupKey\">" : "<img src=\"../images/JetAircraft_NS_Open_0.gif\" class=\"unitGroupKey\">") + "</td></tr>";
                        }

                    }
                    i += 1;
                }

                str += "</tbody></table>";

                return str;
            }
            catch (Exception ex)
            {
                //sTraceLog(ex.ToString);
                return "";
            }
        }

        protected void LinkButtonAssignUnit_Click(object sender, EventArgs e)
        {
            try
            {
                pnlErr.Visible = false;
                int SeatInfo0Checking = 0;
                int SeatInfo1Checking = 0;
                int SeatInfo2Checking = 0;
                int SeatInfo3Checking = 0;

                bool IsOneWay = false;

                switch (eFlight)
                {
                    case EnumFlight.ConnectingFlight:
                        if (model == null == false)
                        {
                            //Flight_Info pDepartFlightInfo = new Flight_Info();
                            //pDepartFlightInfo = Session["DepartFlightInfo"];
                            if (Session["SeatInfo0"] == null == false)
                            {

                                for (int i = 0; i <= ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"]).Count() - 1; i++)
                                {
                                    if (string.IsNullOrEmpty(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"])[i].SelectedSeat))
                                    {
                                        //lblErr.Text = "Please select the seat(s) for Depart."
                                        //pnlErr.Visible = True
                                        //Session["ErrorMsg"] = lblErr.Text
                                        SeatInfo0Checking = SeatInfo0Checking + 1;
                                        //Response.Redirect("~/SeatConnected.aspx")
                                        //Exit Sub
                                    }
                                }
                                if (SeatInfo0Checking == 0)
                                {
                                    if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"], model, EnumFlightType.DepartFlight))
                                    {
                                        DeleteXML((string)Session["SeatInfo0Xml"]);
                                        Session["DepartSeatInfo"] = Session["SeatInfo0"];
                                    }
                                }
                            }
                            else
                            {
                                SeatInfo0Checking = SeatInfo0Checking + 1;
                                //lblErr.Text = "Please select Depart seat before proceed."
                                //pnlErr.Visible = True
                                //Session["ErrorMsg"] = lblErr.Text
                                //Response.Redirect("~/SeatConnected.aspx")
                            }
                            if (Session["SeatInfo1"] == null == false)
                            {
                                for (int i = 0; i <= ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"]).Count - 1; i++)
                                {
                                    if (string.IsNullOrEmpty(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"])[i].SelectedSeat))
                                    {
                                        //lblErr.Text = "Please select the seat(s) for Depart."
                                        //pnlErr.Visible = True
                                        //Session["ErrorMsg"] = lblErr.Text
                                        SeatInfo1Checking = SeatInfo1Checking + 1;
                                        //Response.Redirect("~/SeatConnected.aspx")
                                        //Exit Sub
                                    }
                                }
                                if (SeatInfo1Checking == 0)
                                {
                                    if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"], model, EnumFlightType.DepartConnectingFlight))
                                    {
                                        DeleteXML((string)Session["SeatInfo1Xml"]);
                                        Session["DepartConnectingSeatInfo"] = Session["SeatInfo1"];
                                    }
                                }

                            }
                            else
                            {
                                SeatInfo1Checking = SeatInfo1Checking + 1;
                                //lblErr.Text = "Please select Depart seat before proceed."
                                //pnlErr.Visible = True
                                //Session["ErrorMsg"] = lblErr.Text
                                //Response.Redirect("~/SeatConnected.aspx")

                            }
                        }
                        else
                        {
                            if (Session["ReturnFlightInfo"] == null)
                            {
                                Response.Redirect("~/pages/InvalidPage.aspx");
                            }
                        }

                        if (Session["ReturnFlightInfo"] == null == false)
                        {
                            //Flight_Info pReturnFlightInfo = new Flight_Info();
                            //pReturnFlightInfo = Session["ReturnFlightInfo"];
                            if (Session["SeatInfo2"] == null == false)
                            {
                                for (int j = 0; j <= ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"]).Count - 1; j++)
                                {
                                    if (string.IsNullOrEmpty(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"])[j].SelectedSeat))
                                    {
                                        //lblErr.Text = "Please select the seat(s) for Return."
                                        //pnlErr.Visible = True

                                        //Session["ErrorMsg"] = lblErr.Text
                                        SeatInfo2Checking = SeatInfo2Checking + 1;
                                        //Response.Redirect("~/SeatConnected.aspx")
                                        //Exit Sub
                                    }
                                }
                                if (SeatInfo2Checking == 0)
                                {
                                    if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"], model2, EnumFlightType.ReturnFlight))
                                    {
                                        DeleteXML((string)Session["SeatInfo2Xml"]);
                                        Session["ReturnSeatInfo"] = Session["SeatInfo2"];
                                    }
                                }

                            }
                            else
                            {
                                SeatInfo2Checking = SeatInfo2Checking + 1;
                                // lblErr.Text = "Please select Return seat before proceed."
                                // pnlErr.Visible = True

                                // Session["ErrorMsg"] = lblErr.Text
                                //Response.Redirect("~/SeatConnected.aspx")
                                //Exit Sub
                            }
                            if (Session["SeatInfo3"] == null == false)
                            {
                                for (int j = 0; j <= ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo3"]).Count - 1; j++)
                                {
                                    if (string.IsNullOrEmpty(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo3"])[j].SelectedSeat))
                                    {
                                        //lblErr.Text = "Please select the seat(s) for Return."
                                        //pnlErr.Visible = True
                                        // Session["ErrorMsg"] = lblErr.Text
                                        SeatInfo3Checking = SeatInfo3Checking + 1;
                                        //Response.Redirect("~/SeatConnected.aspx")
                                        return;
                                    }

                                }
                                if (SeatInfo3Checking == 0)
                                {
                                    if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo3"], model2, EnumFlightType.ReturnConnectingFlight))
                                    {
                                        DeleteXML((string)Session["SeatInfo3Xml"]);
                                        Session["ReturnConnectingSeatInfo"] = Session["SeatInfo3"];
                                    }
                                }

                            }
                            else
                            {
                                SeatInfo3Checking = SeatInfo3Checking + 1;
                                //lblErr.Text = "Please select Return seat before proceed."
                                //pnlErr.Visible = True

                                // Session["ErrorMsg"] = lblErr.Text
                                //Response.Redirect("~/SeatConnected.aspx")
                                //Exit Sub
                            }

                        }
                        Page.Validate("PrimaryMandatory");
                        if (Page.IsValid)
                        {
                            if (SeatInfo0Checking == 0 & SeatInfo1Checking == 0 & SeatInfo2Checking == 0 & SeatInfo3Checking == 0)
                            {
                                //API.DeleteXML(Session["SeatInfo0Xml"])
                                //API.DeleteXML(Session["SeatInfo1Xml"])
                                //API.DeleteXML(Session["SeatInfo2Xml"])
                                //API.DeleteXML(Session["SeatInfo3Xml"])
                                Response.Redirect("~/pages/SeatSummary.aspx");
                            }
                            else
                            {
                                lblErr.Text = "Please select seat(s) before proceed.";
                                pnlErr.Visible = true;
                                Session["ErrorMsg"] = lblErr.Text;
                                Response.Redirect("~/seats.aspx");
                                //FillFlight(Session["akey"], 0)
                            }

                        }
                        break;
                    case EnumFlight.DirectFlight:
                        if (Session["DepartFlightInfo"] == null == false)
                        {
                            //Flight_Info pDepartFlightInfo = new Flight_Info();
                            //pDepartFlightInfo = Session["DepartFlightInfo"];
                            if (Session["SeatInfo0"] == null == false)
                            {
                                for (int i = 0; i <= ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"]).Count - 1; i++)
                                {
                                    if (string.IsNullOrEmpty(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"])[i].SelectedSeat))
                                    {
                                        SeatInfo0Checking = SeatInfo0Checking + 1;
                                    }
                                }
                                if (SeatInfo0Checking == 0)
                                {
                                    if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"], model, EnumFlightType.DepartFlight))
                                    {
                                        DeleteXML((string)Session["SeatInfo0Xml"]);
                                        Session["DepartSeatInfo"] = Session["SeatInfo0"];
                                    }
                                }
                            }
                            else
                            {
                                SeatInfo0Checking = SeatInfo0Checking + 1;
                            }
                        }
                        else
                        {
                            //Response.Redirect("~/pages/InvalidPage.aspx")
                            SeatInfo0Checking = 0;
                        }

                        if (Session["ReturnFlightInfo"] == null == false)
                        {
                            //Flight_Info pReturnFlightInfo = new Flight_Info();
                            //pReturnFlightInfo = Session["ReturnFlightInfo"];
                            if (Session["SeatInfo1"] == null == false)
                            {
                                for (int j = 0; j <= ((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"]).Count - 1; j++)
                                {
                                    if (string.IsNullOrEmpty(((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"])[j].SelectedSeat))
                                    {
                                        SeatInfo1Checking = SeatInfo1Checking + 1;

                                    }
                                }
                                if (SeatInfo1Checking == 0)
                                {
                                    if (AssignSeat((List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"], model2, EnumFlightType.ReturnFlight))
                                    {
                                        DeleteXML((string)Session["SeatInfo1Xml"]);
                                        Session["ReturnSeatInfo"] = Session["SeatInfo1"];
                                    }
                                }

                            }
                            else
                            {
                                SeatInfo1Checking = SeatInfo1Checking + 1;
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
                                Response.Redirect("~/pages/SeatSummary.aspx");
                            }
                            else
                            {
                                lblErr.Text = "Please select seat(s) before proceed.";
                                pnlErr.Visible = true;
                                Session["ErrorMsg"] = lblErr.Text;
                                Response.Redirect("~/seats.aspx");
                            }
                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                //sTraceLog(ex.ToString);
                lblErr.Text = ex.ToString();
            }
        }

        private bool AssignSeat(List<ABS.Logic.GroupBooking.SeatInfo> _seatInfo, BookingControl.TemFlight model, EnumFlightType pFlightType)
        {
            string Signature = (string)Session["signature"];

            try
            {
                //Dim pDepartFlightInfo As AirAsia.Base.Flight_Info
                //Dim pReturnFlightInfo As AirAsia.Base.Flight_Info
                BookingUpdateResponseData AssignResponse = new BookingUpdateResponseData();
                //pDepartFlightInfo = Session["DepartFlightInfo"]
                //pReturnFlightInfo = Session["ReturnFlightInfo"]
                int SeatInfoCount1 = _seatInfo.Count - 1;
                //Dim SeatInfoCount1 As Integer = Session["SeatInfo0"].count - 1
                //Dim SeatInfoCount2 As Integer = Session["SeatInfo2"].count - 1
                int i = 0;
                int[] aPassengerNumber = new int[SeatInfoCount1 + 1];
                int[] aPassengerID = new int[SeatInfoCount1 + 1];
                string[] aUnitDisignator = new string[SeatInfoCount1 + 1];
                string[] acompartmentDesignator = new string[SeatInfoCount1 + 1];
                //Dim bPassengerNumber(SeatInfoCount2) As Integer
                //Dim bPassengerID(SeatInfoCount2) As Integer
                //Dim bUnitDisignator(SeatInfoCount2) As String
                //Dim bcompartmentDesignator(SeatInfoCount2) As String
                foreach (ABS.Logic.GroupBooking.SeatInfo rSeatInfo in _seatInfo)
                {
                    //rSeatInfo = rSeatInfo_loopVariable;
                    //ReDim aPassengerNumber(i)
                    //ReDim aPassengerID(i)
                    //ReDim aUnitDisignator(i)
                    //ReDim acompartmentDesignator(i)
                    aPassengerNumber[i] = rSeatInfo.PassengerNumber;
                    aPassengerID[i] = Convert.ToInt32(rSeatInfo.PassengerID);
                    aUnitDisignator[i] = rSeatInfo.SelectedSeat;
                    acompartmentDesignator[i] = rSeatInfo.CompartmentDesignator;
                    i += 1;
                }

                string STD = model.TemFlightStd.ToString();

                switch (pFlightType)
                {
                    case EnumFlightType.DepartFlight:
                    case EnumFlightType.ReturnFlight:
                        AssignResponse = absNavitaire.AssignSeats(Signature, model.TemFlightDeparture, model.TemFlightArrival, STD, aPassengerNumber, aPassengerID, aUnitDisignator, acompartmentDesignator, model.TemFlightCarrierCode, model.TemFlightFlightNumber, model.TemFlightOpSuffix);
                        break;
                        ////case EnumFlightType.DepartConnectingFlight:
                        ////case EnumFlightType.ReturnConnectingFlight:
                        ////    AssignResponse = API.AssignSeats(pFlightInfo.DepartureStation2, pFlightInfo.ArrivalStation2, pFlightInfo.STD2, aPassengerNumber, aPassengerID, aUnitDisignator, acompartmentDesignator, pFlightInfo.CarrierCode2, pFlightInfo.FlightNumber2, pFlightInfo.OpSuffix2);
                        ////    break;
                }

                return true;

            }
            catch (Exception ex)
            {
                //sTraceLog(ex.ToString);
                return false;
            }
        }

        protected void btnSeatDepart1_ServerClick(object sender, EventArgs e)
        {
            pnlErr.Visible = false;
            Session["btnSelected"] = 0;
            Session["ErrorMsg"] = null;
            //'ReservedSeat()
            Response.Redirect("PickSeat.aspx");
        }

        protected void btnSeatDepart2_ServerClick(object sender, EventArgs e)
        {
            pnlErr.Visible = false;
            Session["btnSelected"] = 1;
            Session["ErrorMsg"] = null;
            //'ReservedSeat()
            Response.Redirect("PickSeat.aspx");
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