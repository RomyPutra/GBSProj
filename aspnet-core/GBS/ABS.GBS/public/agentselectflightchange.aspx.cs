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
using ABS.GBS.Log;
//using log4net;

namespace GroupBooking.Web
{
	public partial class agentselectflightchange : System.Web.UI.Page
    {
        #region declaration
        UserSet MyUserSet;
        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogControl log = new LogControl();
        SystemLog SystemLog = new SystemLog();

        BookingTransactionMain bookingMain = new BookingTransactionMain();
        ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            string result = string.Empty;
            if (Session["AgentSet"] != null)
            { MyUserSet = (UserSet)Session["AgentSet"]; }
            if (HttpContext.Current.Session["TransMainData"] != null)
            { bookingMain = (BookingTransactionMain)HttpContext.Current.Session["TransMainData"]; }

            if (!IsPostBack)
            {
                InitializeForm();
            }
        }

        protected void InitializeForm()
        {
            HttpCookie cookie2 = Request.Cookies["cookieTemFlight"];
            if (cookie2 != null)
            {
                cookie2.Expires = DateTime.Today.AddDays(-1);
                Response.Cookies.Add(cookie2);
            }
            ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition model = new ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition();

            HttpCookie cookie = Request.Cookies["cookieSearchcondition"];
            if (cookie != null)
            {
                model.Carrier = cookie.Values["Carrier"];
                model.Departure = cookie.Values["Departure"];
                model.Arrival = cookie.Values["Arrival"];
                bool oneway = false;
                if (cookie.Values["ifOneWay"] == "TRUE")
                {
                    oneway = true;
                    tr_Return.Visible = false;
                }
                else
                {
                    model.ReturnDate = Convert.ToDateTime(cookie.Values["ReturnDate"]);
                }
                model.ifOneWay = oneway;
                model.Currency = cookie.Values["Currency"];
                model.DepartDate = Convert.ToDateTime(cookie.Values["DepartDate"]);
                model.ReturnDate = Convert.ToDateTime(cookie.Values["ReturnDate"]);
                model.GuestNum = Convert.ToInt32(cookie.Values["GuestNum"]);
                model.PaxNum = Convert.ToInt32(cookie.Values["PaxNum"]);
                model.ChildNum = Convert.ToInt32(cookie.Values["ChildNum"]);

                string temp = "";
                temp = cookie.Values["DepartureDetail"];
                lbl_Go1.Text = temp.Replace("|", " | ");

                temp = cookie.Values["ArrivalDetail"];
                lbl_Go2.Text = temp.Replace("|", " | ");

                lbl_Return1.Text = lbl_Go2.Text;
                lbl_Return2.Text = lbl_Go1.Text;

                DataTable tempDt = objBooking.dtFlight();

                BindList1(model, tempDt);
                if (oneway == false)
                {
                    BindList2(model, tempDt);
                    if (DataList1.Items.Count <= 0 || DataList2.Items.Count <= 0)
                    {

                        Response.Redirect(Shared.MySite.PublicPages.AgentSearchFlightChange);
                    }
                }
                else
                {
                    if (DataList1.Items.Count <= 0)
                    {

                        Response.Redirect(Shared.MySite.PublicPages.AgentSearchFlightChange);
                    }
                }

            }
        }

        private void BindList1(ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition model, DataTable tempDt)
        {
            if (HttpContext.Current.Session["tempFlight"] != null)
            {
                string strExpr;
                string strSort;

                DataTable dt = objBooking.dtFlight();

                strExpr = "TemFlightDeparture = '" + model.Departure + "' AND TemFlightArrival = '" + model.Arrival + "' AND TemFlightAgentName = '" + bookingMain.AgentName + "' AND TemFlightIfReturn = false";

                strSort = "TemFlightStd ASC";

                dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                tempDt.Clear();

                foreach (DataRow row in foundRows)
                {
                    tempDt.ImportRow(row);
                }

                tempDt.DefaultView.Sort = "TemFlightStd ASC";

                DataList1.DataSource = tempDt;
                DataList1.DataBind();
            }
        }

        private void BindList2(ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition model, DataTable tempDt)
        {
            if (HttpContext.Current.Session["tempFlight"] != null)
            {
                string strExpr;
                string strSort;
                DataTable dt = new DataTable();

                strExpr = "TemFlightDeparture = '" + model.Arrival + "' AND TemFlightArrival = '" + model.Departure + "' AND TemFlightAgentName = '" + bookingMain.AgentName + "' AND TemFlightIfReturn = true";

                strSort = "TemFlightStd ASC";
                // Use the Select method to find all rows matching the filter.

                dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                tempDt.Clear();

                foreach (DataRow row in foundRows)
                {
                    tempDt.ImportRow(row);
                }

                DataList2.DataSource = tempDt;
                DataList2.DataBind();
            }
        }

        protected void img_Click(object sender, ImageClickEventArgs e)
        {
            tr_Return.Visible = false;
        }

        protected void ValidatePopup(object sender, CallbackEventArgs e)
        {
            try
            {
                MessageList msgList = new MessageList();
                ArrayList aMsgList = new ArrayList();

                string list1ID = "";
                string list2ID = "";
                bool ifSelect = false;
                for (int i = 0; i < this.DataList1.Items.Count; i++)
                {
                    RadioButton rb = this.DataList1.Items[i].FindControl("RadioButton1") as RadioButton;
                    if (rb.Checked)
                    {
                        ifSelect = true;
                        Label l = this.DataList1.Items[i].FindControl("lbl_list1ID") as Label;
                        list1ID = l.Text;
                        break;
                    }
                }
                if (ifSelect)
                {
                    if (tr_Return.Visible == true)
                    {
                        ifSelect = false;
                        for (int i = 0; i < this.DataList2.Items.Count; i++)
                        {
                            RadioButton rb = this.DataList2.Items[i].FindControl("RadioButton2") as RadioButton;
                            if (rb.Checked)
                            {
                                ifSelect = true;
                                Label l = this.DataList2.Items[i].FindControl("lbl_list2ID") as Label;
                                list2ID = l.Text;
                                break;
                            }
                        }
                    }
                    if (ifSelect)
                    {

                        HttpCookie cookie = Request.Cookies["cookieSearchcondition"];

                        //check available seat
                        bool oneWay = true;
                        bool available = false;
                        int availableGoing = 0;
                        int availableReturn = 0;

                        ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition model = new ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition();
                        ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition model2 = new ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition();

                        string strExpr;
                        string strSort;
                        DataTable dt = new DataTable();

                        strExpr = "TemFlightId = '" + list1ID + "'";

                        strSort = "";

                        dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                        DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                        FillModelFromDataRow(foundRows, ref  model);

                        availableGoing = objBooking.CheckAvailableFare(model, "");
                        if (availableGoing == 1)
                        {
                            available = true;
                        }
                        if (list2ID != "" && available == true) //check one way
                        {
                            oneWay = false;

                            strExpr = "TemFlightId = '" + list2ID + "'";

                            strSort = "";

                            dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                            foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                            FillModelFromDataRow(foundRows, ref  model2);

                            availableReturn = objBooking.CheckAvailableFare(model2, "");

                            if (availableReturn == 1)
                            {
                                available = true;
                            }
                            else
                            {
                                available = false;
                            }
                        }

                        if (available == true)
                        {

                            HttpCookie cookie2 = new HttpCookie("cookieTemFlight");
                            cookie2.HttpOnly = true;
                            cookie2.Values.Add("list1ID", list1ID);
                            cookie2.Values.Add("ReturnID", list2ID);
                            cookie2.Values.Add("PaxNum", cookie.Values["PaxNum"]);
                            cookie2.Values.Add("GuestNum", cookie.Values["GuestNum"]);
                            cookie2.Values.Add("ChildNum", cookie.Values["ChildNum"]);
                            Response.AppendCookie(cookie2);

                            e.Result = "";

                            //fill flightModel
                            ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight departModel = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();
                            ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight returnModel = new ABS.Logic.GroupBooking.Booking.BookingControl.TemFlight();

                            DataTable dtModel = objBooking.dtFlight();

                            strExpr = "TemFlightId = '" + list1ID + "'";

                            strSort = "";

                            dtModel = (DataTable)HttpContext.Current.Session["TempFlight"];

                            foundRows = dtModel.Select(strExpr, strSort, DataViewRowState.Added);

                            FillModelFromDataRow(foundRows, ref departModel);

                            if (list2ID != "")
                            {
                                strExpr = "TemFlightId = '" + list2ID + "'";
                                strSort = "";

                                dtModel = (DataTable)HttpContext.Current.Session["TempFlight"];
                                foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                                FillModelFromDataRow(foundRows, ref returnModel);
                            }

                            //fill next journey data
                            string sign = "";

                            //update flight
                            string remark = "";
                            if (HttpContext.Current.Session["ReasonData"] != null)
                            { remark = HttpContext.Current.Session["ReasonData"].ToString(); }

                            List<BookingTransactionDetail> detailCollection = new List<BookingTransactionDetail>();
                            if (HttpContext.Current.Session["ListDetailData"] != null)
                            { detailCollection = (List<BookingTransactionDetail>)HttpContext.Current.Session["ListDetailData"]; }

                            BookingTransactionMain transMain = new BookingTransactionMain();
                            if (HttpContext.Current.Session["TransMainData"] != null)
                            { transMain = (BookingTransactionMain)HttpContext.Current.Session["TransMainData"]; }

                            string errMsg = "";
                            string fareClassDepart = "";
                            string fareClassReturn = "";
                            string prevSign = "";
                            string nextSign = "";

                            string transID = "";
                            DateTime stdDate = DateTime.Now;

                            Boolean flagOneWay = false;
                            if (HttpContext.Current.Session["flagOneWay"] != null)
                            {
                                flagOneWay = (Boolean)HttpContext.Current.Session["flagOneWay"];
                            }

                            string[] listPNR = new string[detailCollection.Count];
                            int idxList = 0;

                            if (detailCollection.Count > 0)
                            {
                                foreach (BookingTransactionDetail detailData in detailCollection)
                                {
                                    if (CheckValidPNR(detailData.RecordLocator, idxList, listPNR) == true)
                                    {
                                        listPNR[idxList] = detailData.RecordLocator;
                                        idxList += 1;

                                        DataTable dtPrevJourney = objBooking.dtKeySignature();
                                        DataTable dtNextJourney = objBooking.dtKeySignature();

                                        //get previous journey sellkey
                                        //dtPrevJourney = absNavitaire.GetKeySignatureByPNR(detailData.RecordLocator, ref errMsg, ref prevSign);
                                       
                                        //get next journey sellkey
                                        departModel.TemFlightADTNum = detailData.PaxAdult;
                                        departModel.TemFlightCHDNum = detailData.PaxChild;
                                        departModel.TemFlightPaxNum = detailData.PaxAdult + detailData.PaxChild;

                                        returnModel.TemFlightADTNum = detailData.PaxAdult;
                                        returnModel.TemFlightCHDNum = detailData.PaxChild;
                                        returnModel.TemFlightPaxNum = detailData.PaxAdult + detailData.PaxChild;
                                        
                                        objBooking.FillNextJourneyData(ref departModel, ref returnModel, "", ref dtNextJourney, flagOneWay, ref fareClassDepart, ref fareClassReturn);
                                        string LoginType = MyUserSet.AgentType.ToString();  //Session["LoginType"].ToString();                                        
                                        string LoginName = MyUserSet.AgentName;
                                        string LoginPWD = "";
                                        string LoginDomain = "";
                                        /* remark to ag payment process
                                        if (LoginType == "SkyAgent")
                                        {
                                            LoginPWD = Session["LoginPWD"].ToString();
                                            LoginDomain = Session["LoginDomain"].ToString();
                                        }*/
                                        //prevSign = absNavitaire.GetBookingByPNR(detailData.RecordLocator, ref errMsg);
                                        string sessID="";
                                        objBooking.UpdateTemFlight(departModel, returnModel, "", ref sessID);

                                        objBooking.SellJourneyMove(departModel, returnModel, LoginType, LoginName, LoginPWD, LoginDomain, sessID, detailData.RecordLocator);
                                        
                                        if (dtPrevJourney.Rows.Count > 1)
                                        {
                                            //2 ways
                                            #region twoways
                                            List<BookingTransactionDetail> listDetail = new List<BookingTransactionDetail>();

                                            //absNavitaire.MoveJourney(dtPrevJourney.Rows[0]["FareSellKey"].ToString(), dtPrevJourney.Rows[0]["JourneySellKey"].ToString(), dtNextJourney.Rows[0]["FareSellKey"].ToString(), dtNextJourney.Rows[0]["JourneySellKey"].ToString(), departModel.TemFlightCurrencyCode, ref errMsg, ref prevSign, dtPrevJourney.Rows[0]["FareSellKeyTransit"].ToString(), dtNextJourney.Rows[0]["FareSellKeyTransit"].ToString());

                                            //absNavitaire.MoveJourney(dtPrevJourney.Rows[1]["FareSellKey"].ToString(), dtPrevJourney.Rows[1]["JourneySellKey"].ToString(), dtNextJourney.Rows[1]["FareSellKey"].ToString(), dtNextJourney.Rows[1]["JourneySellKey"].ToString(), departModel.TemFlightCurrencyCode, ref errMsg, ref prevSign, dtPrevJourney.Rows[1]["FareSellKeyTransit"].ToString(), dtNextJourney.Rows[1]["FareSellKeyTransit"].ToString());

                                            if (absNavitaire.BookingCommit(detailData.RecordLocator, sessID, ref errMsg))
                                            {
                                                //assign detail
                                                //detail depart
                                                detailData.CarrierCode = departModel.TemFlightCarrierCode;
                                                detailData.FlightNo = departModel.TemFlightFlightNumber;
                                                detailData.Origin = departModel.TemFlightDeparture;
                                                detailData.Destination = departModel.TemFlightArrival;
                                                detailData.DepatureDate = Convert.ToDateTime(departModel.TemFlightStd);
                                                detailData.ArrivalDate = Convert.ToDateTime(departModel.TemFlightSta);
                                                detailData.FareClass = fareClassDepart;
                                                detailData.Transit = departModel.TemFlightTransit;
                                                if (departModel.TemFlightTransit != "")
                                                {
                                                    detailData.DepatureDate2 = Convert.ToDateTime(departModel.TemFlightStd2);
                                                    detailData.ArrivalDate2 = Convert.ToDateTime(departModel.TemFlightSta2);
                                                }
                                                detailData.LastSyncBy = MyUserSet.AgentID;
                                                listDetail.Add(detailData);

                                                if (transID == "") // for trans main
                                                {
                                                    transID = detailData.TransID;
                                                    stdDate = detailData.DepatureDate;
                                                }

                                                //detail return
                                                BookingTransactionDetail detailReturn = new BookingTransactionDetail();
                                                detailReturn = objBooking.GetSingleDetail(detailData.TransID, detailData.RecordLocator, detailData.SeqNo);

                                                detailReturn.CarrierCode = returnModel.TemFlightCarrierCode;
                                                detailReturn.FlightNo = returnModel.TemFlightFlightNumber;
                                                detailReturn.Origin = returnModel.TemFlightDeparture;
                                                detailReturn.Destination = returnModel.TemFlightArrival;
                                                detailReturn.DepatureDate = Convert.ToDateTime(returnModel.TemFlightStd);
                                                detailReturn.ArrivalDate = Convert.ToDateTime(returnModel.TemFlightSta);
                                                detailReturn.FareClass = fareClassReturn;
                                                detailReturn.Transit = returnModel.TemFlightTransit;
                                                if (returnModel.TemFlightTransit != "")
                                                {
                                                    detailReturn.DepatureDate2 = Convert.ToDateTime(returnModel.TemFlightStd2);
                                                    detailReturn.ArrivalDate2 = Convert.ToDateTime(returnModel.TemFlightSta2);
                                                }
                                                detailReturn.LastSyncBy = MyUserSet.AgentID;

                                                listDetail.Add(detailReturn);

                                                objBooking.SaveListBookingDetail(listDetail, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Update);

                                            }
                                            else { break; }

                                            #endregion
                                        }
                                        else
                                        {
                                            #region oneway
                                            //1 way
                                            //move journey api
                                            //absNavitaire.MoveJourney(dtPrevJourney.Rows[0]["FareSellKey"].ToString(), dtPrevJourney.Rows[0]["JourneySellKey"].ToString(), dtNextJourney.Rows[0]["FareSellKey"].ToString(), dtNextJourney.Rows[0]["JourneySellKey"].ToString(), departModel.TemFlightCurrencyCode, ref errMsg, ref prevSign, dtPrevJourney.Rows[0]["FareSellKeyTransit"].ToString(), dtNextJourney.Rows[0]["FareSellKeyTransit"].ToString());

                                            if (absNavitaire.BookingCommit(detailData.RecordLocator, sessID, ref errMsg))
                                            {
                                                //update journey data base on PNR
                                                detailData.CarrierCode = departModel.TemFlightCarrierCode;
                                                detailData.FlightNo = departModel.TemFlightFlightNumber;
                                                detailData.Origin = departModel.TemFlightDeparture;
                                                detailData.Destination = departModel.TemFlightArrival;
                                                detailData.DepatureDate = Convert.ToDateTime(departModel.TemFlightStd);
                                                detailData.ArrivalDate = Convert.ToDateTime(departModel.TemFlightSta);
                                                detailData.FareClass = fareClassDepart;
                                                detailData.Transit = departModel.TemFlightTransit;
                                                if (departModel.TemFlightTransit != "")
                                                {
                                                    detailData.DepatureDate2 = Convert.ToDateTime(departModel.TemFlightStd2);
                                                    detailData.ArrivalDate2 = Convert.ToDateTime(departModel.TemFlightSta2);
                                                }
                                                detailData.LastSyncBy = MyUserSet.AgentID;

                                                objBooking.SaveBK_TRANSDTL(detailData, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Update);

                                                if (transID == "") // for trans main
                                                {
                                                    transID = detailData.TransID;
                                                    stdDate = detailData.DepatureDate;
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                if (errMsg == "")
                                {
                                    //success all, update transmain                                    
                                    transMain.STDDate = stdDate;
                                    transMain.LastSyncBy = MyUserSet.AgentID;
                                    transMain.SyncLastUpd = DateTime.Now;
                                    transMain.TransRemark1 = remark;
                                    transMain.TransRemark2 = "Change Flight";
                                    objBooking.SaveBK_TRANSMAIN(transMain, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Update);
                                }
                            }

                        }
                        else
                        {
                            e.Result = msgList.Err100013;
                        }
                    }
                    else
                    {
                        e.Result = msgList.Err100014;
                    }
                }
                else
                {
                    e.Result = msgList.Err100015;

                }

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this,ex);
            }
        }

        // for checking depart and return because return will have same PNR with depart, only need update once
        protected Boolean CheckValidPNR(string PNR, int idx, string[] listPNR)
        {
            Boolean flagValid = true;
            if (idx == 0)
            {
                flagValid = true;
            }
            else
            {
                for (int i = 0; i < listPNR.Length; i++)
                {
                    if (PNR == listPNR[i])
                    {
                        flagValid = false;
                    }
                }
            }
            return flagValid;
        }

        protected void FillModelFromDataRow(DataRow[] foundRows, ref ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition model)
        {
            try
            {
                model.Arrival = foundRows[0]["TemFlightArrival"].ToString();
                model.Carrier = foundRows[0]["TemFlightCarrierCode"].ToString();
                model.ChildNum = Convert.ToInt16(foundRows[0]["TemFlightCHDNum"]);
                model.Currency = foundRows[0]["TemFlightCurrencyCode"].ToString();
                model.DepartDate = Convert.ToDateTime(foundRows[0]["TemFlightStd"]);
                model.Departure = foundRows[0]["TemFlightDeparture"].ToString();
                model.GuestNum = Convert.ToInt16(foundRows[0]["TemFlightADTNum"]);
                model.ifOneWay = Convert.ToBoolean(foundRows[0]["TemFlightIfReturn"]);
                model.PaxNum = Convert.ToInt16(foundRows[0]["TemFlightPaxNum"]);
                model.ReturnDate = Convert.ToDateTime(foundRows[0]["TemFlightSta"]);
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this,ex);
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

                if (IsNumeric(foundRows[0]["TemFlightOth"].ToString()))
                { model.TemFlightOth = Convert.ToDecimal(foundRows[0]["TemFlightOth"]); }
                if (IsNumeric(foundRows[0]["TemFlightServiceCharge"].ToString()))
                { model.TemFlightServiceCharge = Convert.ToDecimal(foundRows[0]["TemFlightServiceCharge"]); }

                model.TemFlightTransit = foundRows[0]["TemFlightTransit"].ToString();
                model.TemFlightSta2 = Convert.ToDateTime(foundRows[0]["TemFlightSta2"]);
                model.TemFlightStd2 = Convert.ToDateTime(foundRows[0]["TemFlightStd2"]);
                model.TemFlightCarrierCode2 = foundRows[0]["TemFlightCarrierCode2"].ToString();
                model.TemFlightFlightNumber2 = foundRows[0]["TemFlightFlightNumber2"].ToString();
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this,ex);
            }
        }

        public static bool IsNumeric(string s)
        {
            double result;
            return s != null && Double.TryParse(s, out result);
        }

        protected void btn_Next_Click(object sender, EventArgs e)
        {

        }
    }
}