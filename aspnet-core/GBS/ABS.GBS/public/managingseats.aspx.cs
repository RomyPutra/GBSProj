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

namespace GroupBooking.Web
{
	public partial class managingseats : System.Web.UI.Page
	{
        LogControl log = new LogControl();

        string TransID;
        string keySent;
        UserSet AgentSet;

        private static int num = 0;
        private static int departID = 0;
        private static string ReturnID = "";

        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
        ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model = null;
        ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model2 = null;

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
            TransID = Request.QueryString["TransID"];
            keySent = Request.QueryString["k"];

            if (Session["AgentSet"] != null)
            {
                AgentSet = (UserSet)Session["AgentSet"];

                departID = 3;
                ReturnID = "6";
                num = 12;
                BindModel();
                if (!SetAccess())
                {
                    return;
                }
                if (!IsPostBack)
                {
                    intSeatTabSession();
                    if (Session["btnSelected"] == null)
                    {
                        Session["btnSelected"] = 0;
                    }

                    HttpContext.Current.Session["SellSessionID"] = null;
                    Session["objListBK_TRANSDTL_Infos"] = null;

                    HttpContext.Current.Session["BalanceNameChange"] = null;
                }

                //FillFlight(model.TemFlightJourneySellKey, (int)Session["btnSelected"]);
            }
            else
            {
                Response.Redirect(Shared.MySite.PublicPages.InvalidPage);
            }
        }

        private void BindModel()
        {
            if (departID != -1)
            {
                model = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
                ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight temFlight = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
                //string strExpr;
                //string strSort;
                //DataTable dt = objBooking.dtFlight();
                //strExpr = "TemFlightId = '" + departID + "'";
                //strSort = "";
                //dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                //DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                //FillModelFromDataRow(foundRows, ref model);
                FillModelFromDataRow(ref model);

                if (ReturnID != "")
                {
                    model2 = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
                    //strExpr = "TemFlightId = '" + ReturnID + "'";
                    //strSort = "";
                    //dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                    //foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                    //FillModelFromDataRow(foundRows, ref model2);
                    FillModelFromDataRow(ref model);
                }
            }
        }

        //protected void FillModelFromDataRow(DataRow[] foundRows, ref ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model)
        protected void FillModelFromDataRow(ref ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight model)
        {
            try
            {
                model.TemFlightFlightNumber = "701";
                model.TemFlightDate = Convert.ToDateTime("2017-07-01 06:10:00.000");
                model.TemFlightArrival = "2017-07-01 07:15:00.000";
                model.TemFlightCarrierCode = "AK";
                model.TemFlightInternational = "I";
                model.TemFlightJourneySellKey = "HK1J7H";
                model.TemFlightCHDNum = 0;
                model.TemFlightCurrencyCode = "MYR";
                model.TemFlightStd = Convert.ToDateTime("2017-07-01 06:10:00.000");
                model.TemFlightDeparture = "KUL";
                model.TemFlightADTNum = 10;
                model.TemFlightIfReturn = true;
                model.TemFlightPaxNum = 10;
                model.TemFlightSta = Convert.ToDateTime("2017-07-01 07:15:00.000");
                model.TemFlightAgentName = "28282828_Evonne";
                model.TemFlightAveragePrice = Convert.ToDecimal("42.08");
                model.TemFlightTotalAmount = Convert.ToDecimal("42.08");
                model.temFlightfarePrice = Convert.ToDecimal("42.08");
                model.TemFlightApt = Convert.ToDecimal("42.08");
                model.TemFlightFuel = Convert.ToDecimal("42.08");
                model.TemFlightTransit = "";
                model.TemFlightSta2 = Convert.ToDateTime("2017-07-01 06:10:00.000");
                model.TemFlightStd2 = Convert.ToDateTime("2017-07-01 07:15:00.000");
                model.TemFlightCarrierCode2 = "";
                model.TemFlightFlightNumber2 = "";
                model.TemFlightOpSuffix = "";
                model.TemFlightOpSuffix2 = "";
                model.TemFlightSignature = "";
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
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

                hfIsInternational.Value = model.TemFlightInternational.ToUpper();
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
                            ctlDepart.Style["display"] = "";
                            PassengerHeader = "<div id='passengerListHeader" + btnSelected + "' class='redSectionHeader'>";
                            PassengerHeader += "<div>Seat summary</div></div>";
                            PassengerHeader += "<div id='passengerListBody" + btnSelected + "' class='sectionBody'><br/>";
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

                            if (model != null)
                            {
                                //pDepartFlightInfo = Session["DepartFlightInfo"];

                                if (btnSelected == 0)
                                {
                                    if (Session["SeatInfo0Xml"] == null == false)
                                    {
                                        DeleteXML((string)Session["SeatInfo0Xml"]);

                                    }
                                    pAvailableSeatInfo = absNavitaire.GetSeatAvailability(RecordLocator, 0, Signature, model.TemFlightCarrierCode, model.TemFlightFlightNumber, model.TemFlightOpSuffix,
                                                                                            model.TemFlightDeparture, model.TemFlightTransit, (DateTime)model.TemFlightStd, Request.PhysicalApplicationPath);
                                    //Session["Click"] = Nothing
                                    Session["pAvailableSeatInfo0"] = pAvailableSeatInfo;
                                    btnSeatDepart1.Style[HtmlTextWriterStyle.BackgroundImage] = "images/CBackground2.png";
                                }
                                else if (btnSelected == 1)
                                {
                                    if (Session["SeatInfo1Xml"] == null == false)
                                    {
                                        DeleteXML((string)Session["SeatInfo1Xml"]);

                                    }
                                    pAvailableSeatInfo = absNavitaire.GetSeatAvailability(RecordLocator, 0, Signature, model.TemFlightCarrierCode2, model.TemFlightFlightNumber2, model.TemFlightOpSuffix2,
                                                                                            model.TemFlightTransit, model.TemFlightArrival, (DateTime)model.TemFlightStd2, Request.PhysicalApplicationPath);
                                    //Session["Click"] = Nothing
                                    Session["pAvailableSeatInfo1"] = pAvailableSeatInfo;
                                    btnSeatDepart2.Style[HtmlTextWriterStyle.BackgroundImage] = "images/CBackground2.png";
                                }


                                if ((pAvailableSeatInfo != null))
                                {

                                    if (!string.IsNullOrEmpty(absNavitaire.DepartXmlUrl))
                                    {
                                        DepartXml = absNavitaire.DepartXmlUrl;
                                        //pDepartFlightInfo.XmlURL = DepartXml;
                                        //DepartXml = "test4.xml"
                                        DepartPax = model.TemFlightPaxNum;
                                        DepartFromTo = model.TemFlightDeparture + "-" + model.TemFlightTransit;
                                        DepartFromTo2 = model.TemFlightTransit + "-" + model.TemFlightArrival;
                                        DepartFromToShort = model.TemFlightDeparture + "-" + model.TemFlightTransit;
                                        DepartFromToShort2 = model.TemFlightTransit + "-" + model.TemFlightArrival;
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
                            if ((model2 != null))
                            {
                                //pReturnFlightInfo = Session["ReturnFlightInfo"];
                                if (btnSelected == 2)
                                {
                                    if (Session["SeatInfo2Xml"] == null == false)
                                    {
                                        DeleteXML((string)Session["SeatInfo2Xml"]);

                                    }
                                    pAvailableSeatInfo = absNavitaire.GetSeatAvailability(RecordLocator, 0, Signature, model2.TemFlightCarrierCode, model2.TemFlightFlightNumber, model2.TemFlightOpSuffix,
                                                                                            model2.TemFlightDeparture, model2.TemFlightTransit, (DateTime)model2.TemFlightStd, Request.PhysicalApplicationPath);
                                    //Session["Click"] = Nothing
                                    Session["pAvailableSeatInfo2"] = pAvailableSeatInfo;
                                    btnSeatReturn1.Style[HtmlTextWriterStyle.BackgroundImage] = "images/CBackground2.png";
                                }
                                else if (btnSelected == 3)
                                {
                                    if (Session["SeatInfo3Xml"] == null == false)
                                    {
                                        DeleteXML((string)Session["SeatInfo3Xml"]);

                                    }
                                    pAvailableSeatInfo = absNavitaire.GetSeatAvailability(RecordLocator, 0, Signature, model2.TemFlightCarrierCode2, model2.TemFlightFlightNumber2, model2.TemFlightOpSuffix2,
                                                                                            model2.TemFlightTransit, model2.TemFlightArrival, (DateTime)model2.TemFlightStd2, Request.PhysicalApplicationPath);
                                    //Session["Click"] = Nothing
                                    Session["pAvailableSeatInfo3"] = pAvailableSeatInfo;
                                    btnSeatReturn2.Style[HtmlTextWriterStyle.BackgroundImage] = "images/CBackground2.png";
                                }

                                if ((pAvailableSeatInfo != null))
                                {
                                    if (!string.IsNullOrEmpty(absNavitaire.DepartXmlUrl))
                                    {
                                        ReturnXml = absNavitaire.DepartXmlUrl;
                                        //pReturnFlightInfo.XmlURL = ReturnXml;
                                        //DepartXml = "test4.xml"
                                        ReturnPax = model2.TemFlightPaxNum;
                                        ReturnFromTo = model2.TemFlightDeparture + "-" + model2.TemFlightTransit;
                                        ReturnFromTo2 = model2.TemFlightTransit + "-" + model2.TemFlightArrival;
                                        ReturnFromToShort = model2.TemFlightDeparture + "-" + model2.TemFlightTransit;
                                        ReturnFromToShort2 = model2.TemFlightTransit + "-" + model2.TemFlightArrival;
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                                else
                                {
                                    lblErr.Text = "Record not found.";
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
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
                Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.InvalidPage);
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
                //sTraceLog(ex.ToString);
                return "";
            }
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

        private List<ABS.Logic.GroupBooking.SeatInfo> AssignSeatInfo(EnumFlightType flightType, string recordLocator, SeatAvailabilityResponse pAvailableSeatInfo)
        {
            List<ABS.Logic.GroupBooking.SeatInfo> seatInfo = new List<ABS.Logic.GroupBooking.SeatInfo>();
            ABS.Logic.GroupBooking.SeatInfo seatInfo1 = new ABS.Logic.GroupBooking.SeatInfo();
            DataTable dtPassenger = default(DataTable);
            ////AirAsia.Logic.FI_Booking_Logic FI_Booking_Logic = new AirAsia.Logic.FI_Booking_Logic();
            //dtPassenger = FI_Booking_Logic.getAllBookingPassenger(recordLocator);
            dtPassenger = objBooking.GetAllBK_PASSENGERLISTInitDataTable(TransID, true);

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
                                foreach (DataRow drRow in dtPassenger.Rows)
                                {
                                    //drRow = drRow_loopVariable;
                                    seatInfo1 = new ABS.Logic.GroupBooking.SeatInfo();
                                    seatInfo1.PaxName = drRow["FirstName"].ToString() + " " + drRow["LastName"].ToString();
                                    seatInfo1.PassengerID = drRow["PassengerID"].ToString();
                                    seatInfo1.PassengerNumber = Convert.ToInt32(drRow["PassengerID"].ToString());
                                    seatInfo1.Seq = i;
                                    seatInfo1.RecordLocator = drRow["PNR"].ToString();

                                    CompartmentDesignator = "";
                                    Deck = "";
                                    SeatSet = "";
                                    IsHotSeat = 0;

                                    seatInfo1.SelectedSeat = getPassengerDefaultSeat(drRow["PNR"].ToString(), seatInfo1.PassengerID, EnumFlightType.DepartFlight, (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"], ref CompartmentDesignator, ref Deck, ref SeatSet, ref IsHotSeat, i);

                                    seatInfo1.CompartmentDesignator = CompartmentDesignator;
                                    seatInfo1.Deck = Deck;
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
                                    seatInfo1.RecordLocator = drRow["PNR"].ToString();

                                    CompartmentDesignator = "";
                                    Deck = "";
                                    SeatSet = "";
                                    IsHotSeat = 0;

                                    seatInfo1.SelectedSeat = getPassengerDefaultSeat(drRow["PNR"].ToString(), seatInfo1.PassengerID, EnumFlightType.DepartConnectingFlight, (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"], ref CompartmentDesignator, ref Deck, ref SeatSet, ref IsHotSeat, i);

                                    seatInfo1.CompartmentDesignator = CompartmentDesignator;
                                    seatInfo1.Deck = Deck;
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
                                    seatInfo1.RecordLocator = drRow["PNR"].ToString();

                                    CompartmentDesignator = "";
                                    Deck = "";
                                    SeatSet = "";
                                    IsHotSeat = 0;

                                    seatInfo1.SelectedSeat = getPassengerDefaultSeat(drRow["PNR"].ToString(), seatInfo1.PassengerID, EnumFlightType.ReturnFlight, (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo2"], ref CompartmentDesignator, ref Deck, ref SeatSet, ref IsHotSeat, i);

                                    seatInfo1.CompartmentDesignator = CompartmentDesignator;
                                    seatInfo1.Deck = Deck;
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
                                    seatInfo1.RecordLocator = drRow["PNR"].ToString();

                                    CompartmentDesignator = "";
                                    Deck = "";
                                    SeatSet = "";
                                    IsHotSeat = 0;

                                    seatInfo1.SelectedSeat = getPassengerDefaultSeat(drRow["PNR"].ToString(), seatInfo1.PassengerID, EnumFlightType.ReturnConnectingFlight, (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo3"], ref CompartmentDesignator, ref Deck, ref SeatSet, ref IsHotSeat, i);

                                    seatInfo1.CompartmentDesignator = CompartmentDesignator;
                                    seatInfo1.Deck = Deck;
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
                                    seatInfo1.RecordLocator = drRow["PNR"].ToString();

                                    CompartmentDesignator = "";
                                    Deck = "";
                                    SeatSet = "";
                                    IsHotSeat = 0;

                                    seatInfo1.SelectedSeat = getPassengerDefaultSeat(drRow["PNR"].ToString(), seatInfo1.PassengerID, EnumFlightType.DepartFlight, (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo0"], ref CompartmentDesignator, ref Deck, ref SeatSet, ref IsHotSeat, i);

                                    seatInfo1.CompartmentDesignator = CompartmentDesignator;
                                    seatInfo1.Deck = Deck;
                                    seatInfo1.SeatSet = SeatSet;
                                    seatInfo1.IsHotSeat = IsHotSeat;

                                    ////seatInfo1.SelectedSeat = getPassengerDefaultSeat(recordLocator, seatInfo1.PassengerID, EnumFlightType.DepartFlight, Session["SeatInfo0"], seatInfo1.CompartmentDesignator, seatInfo1.Deck, seatInfo1.SeatSet, seatInfo1.IsHotSeat, i);

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
                                    seatInfo1.RecordLocator = drRow["PNR"].ToString();

                                    CompartmentDesignator = "";
                                    Deck = "";
                                    SeatSet = "";
                                    IsHotSeat = 0;

                                    seatInfo1.SelectedSeat = getPassengerDefaultSeat(drRow["PNR"].ToString(), seatInfo1.PassengerID, EnumFlightType.ReturnFlight, (List<ABS.Logic.GroupBooking.SeatInfo>)Session["SeatInfo1"], ref CompartmentDesignator, ref Deck, ref SeatSet, ref IsHotSeat, i);

                                    seatInfo1.CompartmentDesignator = CompartmentDesignator;
                                    seatInfo1.Deck = Deck;
                                    seatInfo1.SeatSet = SeatSet;
                                    seatInfo1.IsHotSeat = IsHotSeat;

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

        private string getPassengerDefaultSeat(string RecordLocator, string PassengerID, EnumFlightType FlightType, List<ABS.Logic.GroupBooking.SeatInfo> SessionSeatInfo,
     ref string CompartmentDesignator, ref string Deck, ref string SeatSet, ref int HotSeat, int PsgNumber)
        {
            try
            {
                ////AABookingManager.Booking BookingInfo = new AABookingManager.Booking();
                int PassengerNumber = 0;
                int iType = 0;
                int iJourney = 0;
                ////AirAsia.Logic.FI_Booking_Logic FI_Booking_Logic = new AirAsia.Logic.FI_Booking_Logic();
                DataTable dtFlightDetails = null;
                DataTable dtPassenger = null;
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
            }
            catch (Exception ex)
            {
                //sTraceLog(ex.ToString);
                return "";
            }
        }
        
        protected void btnSeatDepart1_ServerClick(object sender, EventArgs e)
        {
            pnlErr.Visible = false;
            Session["btnSelected"] = 0;
            Session["ErrorMsg"] = null;
            //'ReservedSeat()
            //Response.Redirect("managingseats.aspx");
            Response.Redirect("managingseats.aspx?k=" + keySent + "&TransID=" + TransID, false);
        }

        protected void btnSeatDepart2_ServerClick(object sender, EventArgs e)
        {
            pnlErr.Visible = false;
            Session["btnSelected"] = 1;
            Session["ErrorMsg"] = null;
            //'ReservedSeat()
            //Response.Redirect("managingseats.aspx");
            Response.Redirect("managingseats.aspx?k=" + keySent + "&TransID=" + TransID, false);
        }
        protected void btnSeatReturn1_ServerClick(object sender, EventArgs e)
        {
            pnlErr.Visible = false;
            Session["btnSelected"] = 2;
            Session["ErrorMsg"] = null;
            //'ReservedSeat()
            //Response.Redirect("managingseats.aspx");
            Response.Redirect("managingseats.aspx?k=" + keySent + "&TransID=" + TransID, false);
        }

        protected void btnSeatReturn2_ServerClick(object sender, EventArgs e)
        {
            pnlErr.Visible = false;
            Session["btnSelected"] = 3;
            Session["ErrorMsg"] = null;
            //'ReservedSeat()
            //Response.Redirect("managingseats.aspx");
            Response.Redirect("managingseats.aspx?k=" + keySent + "&TransID=" + TransID, false);
        }

        protected void assignSeatCallBack_Callback(object source, CallbackEventArgs e)
        {
            if (e.Parameter != null && e.Parameter.ToLower() == "back")
            {
                ASPxWebControl.RedirectOnCallback("managingseats.aspx");
                return;
            }
            //hResult.Value = ValidateSeat();
            e.Result = hResult.Value;
        }
	}
}