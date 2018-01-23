using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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


namespace ABS.GBS.UserControl
{
    public partial class flightbookingdetail : System.Web.UI.UserControl
    {

        ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogControl log = new LogControl();

        UserSet AgentSet;
        AdminSet AdminSet;
        List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain> TransMainData = new List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>();
        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        BookingTransactionDetail bookDTLInfo = new BookingTransactionDetail();
        List<BookingTransactionDetail> lstbookFlightInfo = new List<BookingTransactionDetail>();
        List<BookingTransTender> lstbookPaymentInfo = new List<BookingTransTender>();
        List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
        List<BookingTransactionDetail> lstRejectedbookDTLInfo = new List<BookingTransactionDetail>();
        List<BookingTransactionDetail> listDetailCombinePNR = new List<BookingTransactionDetail>();

        BookingTransactionMain bookHDRInfo = new BookingTransactionMain();

        string TransID;

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["NewBooking"] = "false";
            
            TransID = Request.QueryString["TransID"];
            string keySent = Request.QueryString["k"];
            bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(TransID);
            if (Session["AgentSet"] != null)
            {
                AgentSet = (UserSet)Session["AgentSet"];
                if (!IsPostBack)
                {
                    LoadData();
                }
            }

            if (Session["AdminSet"] != null)
            {
                AdminSet = (AdminSet)Session["AdminSet"];
                //bookHDRInfo = objBooking.GetSingleBK_TRANSMAIN(TransID);
                if (!IsPostBack)
                {
                    LoadData();
                }
            }

        }

        protected void LoadData()
        {
            promoCodeFlight.Visible = false;

            if (Session["AgentSet"] != null)
                lblAgentName.Text = AgentSet.AgentName;
            if (Session["AdminSet"] != null)
                lblAgentName.Text = bookHDRInfo.AgentName;

            if (Session["OrganizationName"] != null)
            {
                lblAgentOrg.Text = Session["OrganizationName"].ToString();
            }

            lstbookFlightInfo = objBooking.GetAllBK_TRANSDTLFlightGrpNoSellKey1(TransID, 1);
            Session["lstbookFlightInfo"] = lstbookFlightInfo;
            //rptFlightDetails.DataSource = Session["lstbookFlightInfo"];
            //rptFlightDetails.DataBind();


            mainReturnConnectWrapper.Visible = false;
            //check is not nothing
            if (lstbookFlightInfo != null && lstbookFlightInfo.Count > 0)
            {
                //print all depart
                BookingTransactionDetail DepartDetail = lstbookFlightInfo[0];

                string tempdateA = String.Format("{0:MM/dd/yyyy}", DepartDetail.ArrivalDate);
                if (DepartDetail.Transit != "")
                    tempdateA = String.Format("{0:MM/dd/yyyy}", DepartDetail.ArrivalDate2);
                string tempdateD = String.Format("{0:MM/dd/yyyy}", DepartDetail.DepatureDate);
                TimeSpan ts = Convert.ToDateTime(tempdateA) - Convert.ToDateTime(tempdateD);
                string temp = "";
                if (ts.Days > 0)
                {
                    if (ts.Days == 1) temp = " (+" + ts.TotalDays.ToString() + " day)"; else temp = " (+" + ts.TotalDays.ToString() + " days)";
                }

                lbl_CarrierCodeOut.Text = DepartDetail.CarrierCode;
                lbl_FlightnumberOut.Text = DepartDetail.FlightNo;

                //check if transit
                if (DepartDetail.Transit != "")
                {
                    connectWrapperDepart.Visible = true;

                    lbl_DepartureOut.Text = DepartDetail.Origin;
                    lbl_DepartureDateOut.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", DepartDetail.DepatureDate);
                    lbl_ArrivalOut.Text = DepartDetail.Transit;
                    lbl_ArrivalDateOut.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", DepartDetail.ArrivalDate);

                    lbl_DepartureOut2.Text = DepartDetail.Transit;
                    lbl_DepartureDateOut2.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", DepartDetail.DepatureDate2);
                    lbl_ArrivalOut2.Text = DepartDetail.Destination;
                    lbl_ArrivalDateOut2.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", DepartDetail.ArrivalDate2) + temp;
                }
                else
                {
                    connectWrapperDepart.Visible = false;

                    lbl_DepartureOut.Text = DepartDetail.Origin;
                    lbl_DepartureDateOut.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", DepartDetail.DepatureDate);
                    lbl_ArrivalOut.Text = DepartDetail.Destination;
                    lbl_ArrivalDateOut.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", DepartDetail.ArrivalDate) + temp;
                }

                //check if have return
                if (lstbookFlightInfo.Count > 1)
                {
                    mainReturnConnectWrapper.Visible = true;

                    BookingTransactionDetail ReturnDetail = lstbookFlightInfo[1];

                    if (ReturnDetail.Transit != "")
                        tempdateA = String.Format("{0:MM/dd/yyyy}", ReturnDetail.ArrivalDate2);
                    else
                        tempdateA = String.Format("{0:MM/dd/yyyy}", ReturnDetail.ArrivalDate);
                    tempdateD = String.Format("{0:MM/dd/yyyy}", ReturnDetail.DepatureDate);
                    ts = Convert.ToDateTime(tempdateA) - Convert.ToDateTime(tempdateD);
                    //lblDateReturn.Text = String.Format("{0:dddd, dd MMMM yyyy}", model2.TemFlightStd);
                    temp = "";
                    if (ts.Days > 0)
                    {
                        if (ts.Days == 1) temp = " (+" + ts.TotalDays.ToString() + " day)"; else temp = " (+" + ts.TotalDays.ToString() + " days)";
                    }

                    lbl_CarrierCodeIN.Text = ReturnDetail.CarrierCode;
                    lbl_FlightnumberIN.Text = ReturnDetail.FlightNo;

                    //check if transit
                    if (ReturnDetail.Transit != "")
                    {
                        connectWrapperReturn.Visible = true;

                        lbl_DepartureIN.Text = ReturnDetail.Origin;
                        lbl_DepartureDateIN.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", ReturnDetail.DepatureDate);
                        lbl_ArrivalIN.Text = ReturnDetail.Transit;
                        lbl_ArrivalDateIN.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", ReturnDetail.ArrivalDate);

                        lbl_DepartureIN2.Text = ReturnDetail.Transit;
                        lbl_DepartureDateIN2.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", ReturnDetail.DepatureDate2);
                        lbl_ArrivalIN2.Text = ReturnDetail.Destination;
                        lbl_ArrivalDateIN2.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", ReturnDetail.ArrivalDate2) + temp;
                    }
                    else
                    {
                        connectWrapperReturn.Visible = false;

                        lbl_DepartureIN.Text = ReturnDetail.Origin;
                        lbl_DepartureDateIN.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", ReturnDetail.DepatureDate);
                        lbl_ArrivalIN.Text = ReturnDetail.Destination;
                        lbl_ArrivalDateIN.Text = String.Format("{0:ddd, dd MMM yyyy HH:mm}", ReturnDetail.ArrivalDate) + temp;
                    }
                }

                //check if have promotion discount
                if (DepartDetail.LinePromoDisc < 0)
                {
                    promoCodeFlight.Visible = true;
                    lbl_PromoCode.Text = "PROMO " + bookHDRInfo.PromoCode;
                }
                else
                {
                    promoCodeFlight.Visible = false;
                    lbl_PromoCode.Text = "";
                }
            }
        }

    }
}