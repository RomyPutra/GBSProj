using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//
using System.Data;
//
using ABS.Logic.GroupBooking.Agent;
using ABS.Logic.GroupBooking.Booking;
using SEAL.Data;
//
using DevExpress.Web;
using System.Collections;
using ABS.Logic.Shared;
using ABS.GBS.Log;
//using log4net;

namespace GroupBooking.Web.Booking
{
    public partial class SelectFlightChange : System.Web.UI.Page
    {
        #region declaration
        UserSet MyUserSet;
        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogControl log = new LogControl();
        SystemLog SystemLog = new SystemLog();

        bool ReturnOnly = false;

        #endregion
        
        protected void Page_Load(object sender, EventArgs e)
        {
            string result = string.Empty;
            if (Session["AgentSet"] != null)
            { MyUserSet = (UserSet)Session["AgentSet"]; }
            else
            {
                if (!IsCallback)
                {
                    Response.Redirect(Shared.MySite.PublicPages.InvalidPage, false);
                }
                
            }

            if (Session["ReturnOnly"].ToString() == "true")
            {
                ReturnOnly = true;
            }

            if (!IsPostBack)
            {
                InitializeForm();
            }

            if (Session["PaxMatch"] != null)
            {
                if (Session["PaxMatch"].ToString() == "false")
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "PostbackKey", "<script type='text/javascript'>window.alert('Available pax fare does not match with the passenger number, kindly choose another flight.');</script>");
                }
            }
        }

        protected void InitializeForm()
        {
            if (ReturnOnly == true)
            {
                tr_Depart.Visible = false;
            }
            HttpCookie cookie2 = Request.Cookies["cookieTemFlight"];
            if (cookie2 != null)
            {
                cookie2.Expires = DateTime.Today.AddDays(-1);
                Response.Cookies.Add(cookie2);
            }
            ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition model = new ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition();
            //searchcondition model = new searchcondition();

            HttpCookie cookie = Request.Cookies["cookieSearchcondition"];
            if (cookie != null)
            {                
                model.Carrier = cookie.Values["Carrier"];//   ViewState["Carrier"].ToString();
                model.Departure = cookie.Values["Departure"];// ViewState["Departure"].ToString();
                model.Arrival = cookie.Values["Arrival"];// ViewState["Arrival"].ToString();
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
                model.ifOneWay = oneway;//  Convert.ToBoolean(ViewState["ifOneWay"]);
                model.Currency = cookie.Values["Currency"];// ViewState["Currency"].ToString();
                model.DepartDate = Convert.ToDateTime(cookie.Values["DepartDate"]);// Convert.ToDateTime(ViewState["DepartDate"]);
                model.ReturnDate = Convert.ToDateTime(cookie.Values["ReturnDate"]);//  Convert.ToDateTime(ViewState["ReturnDate"]);
                model.GuestNum = Convert.ToInt32(cookie.Values["GuestNum"]);//  Convert.ToInt32(ViewState["GuestNum"]);
                model.PaxNum = Convert.ToInt32(cookie.Values["PaxNum"]);
                model.ChildNum = Convert.ToInt32(cookie.Values["ChildNum"]);

                string temp = "";
                temp = cookie.Values["DepartureDetail"];
                lbl_Go1.Text = temp.Replace("|", " | ");
                //lbl_Go1.Text = cookie.Values["DepartureDetail"];
                temp = cookie.Values["ArrivalDetail"];
                lbl_Go2.Text = temp.Replace("|", " | ");
                //lbl_Go2.Text = cookie.Values["ArrivalDetail"];

                lbl_Return1.Text = lbl_Go2.Text;
                //lbl_Return1.Text = cookie.Values["ArrivalDetail"];
                lbl_Return2.Text = lbl_Go1.Text;
                //lbl_Return2.Text = cookie.Values["DepartureDetail"];                
                
                DataTable tempDt = objBooking.dtFlight();


                if (ReturnOnly == false)
                {
                    BindList1(model, tempDt);
                }
                if (oneway == false)
                {
                    BindList2(model,tempDt);
                    if (ReturnOnly == true)
                    {
                        if (gvSelectFlightReturn.Items.Count <= 0)
                        {
                            Response.Redirect(Shared.MySite.PublicPages.SearchFlightChange); //need to be checked
                        }
                    }
                    else if (dvSelectFlight.Items.Count <= 0 || gvSelectFlightReturn.Items.Count <= 0)
                    {
                        Response.Redirect(Shared.MySite.PublicPages.SearchFlightChange); //need to be checked
                    }
                }
                else
                {
                    if (dvSelectFlight.Items.Count <= 0)
                    {
                        Response.Redirect(Shared.MySite.PublicPages.SearchFlightChange); //need to be checked
                    }
                }

            }
        }

        private void BindList1(ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition model,DataTable tempDt)
        {
            if (HttpContext.Current.Session["tempFlight"] != null)
            {                
                string strExpr;
                string strSort;

                DataTable dt = objBooking.dtFlight();

                strExpr = "TemFlightDeparture = '" + model.Departure + "' AND TemFlightArrival = '" + model.Arrival + "' AND TemFlightAgentName = '" + MyUserSet.LoginName + "' AND TemFlightIfReturn = false";

                strSort = "TemFlightStd ASC";                

                dt = (DataTable)HttpContext.Current.Session["TempFlight"];
                
                DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);
                
                tempDt.Clear();
                
                foreach (DataRow row in foundRows)
                {
                    tempDt.ImportRow(row);
                }

                tempDt.DefaultView.Sort = "TemFlightStd ASC";

                dvSelectFlight.DataSource = tempDt;
                dvSelectFlight.DataBind();
            }
        }

        private void BindList2(ABS.Logic.GroupBooking.Booking.BookingControl.searchcondition model,DataTable tempDt)
        {
            if (HttpContext.Current.Session["tempFlight"] != null)
            {                
                string strExpr;
                string strSort;
                DataTable dt = new DataTable();

                strExpr = "TemFlightDeparture = '" + model.Arrival + "' AND TemFlightArrival = '" + model.Departure + "' AND TemFlightAgentName = '" + MyUserSet.LoginName + "' AND TemFlightIfReturn = true";

                strSort = "TemFlightStd ASC";
                // Use the Select method to find all rows matching the filter.

                dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                DataRow[] foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                tempDt.Clear();

                foreach (DataRow row in foundRows)
                {
                    tempDt.ImportRow(row);
                }

                gvSelectFlightReturn.DataSource = tempDt;
                gvSelectFlightReturn.DataBind();
            }
        }
       
        protected void img_Click(object sender, ImageClickEventArgs e)
        {//关闭返程航班
            tr_Return.Visible = false;
        }

        protected void ValidatePopup(object sender, CallbackEventArgs e)
        {
            MessageList msgList = new MessageList();
            try
            {
                if (Session["AgentSet"] == null)
                {
                    e.Result = msgList.Err100025;
                    return;
                }

                ArrayList aMsgList = new ArrayList();

                string list1ID = "";
                string list2ID = "";
                bool ifSelect = false;
                if (ReturnOnly == true)
                {
                    ifSelect = true;
                }
                else
                {
                    for (int i = 0; i < this.dvSelectFlight.Items.Count; i++)
                    {
                        RadioButton rb = this.dvSelectFlight.FindItemControl("RadioButton1", dvSelectFlight.Items[i]) as RadioButton;
                        if (rb.Checked)
                        {
                            ifSelect = true;
                            HiddenField l = this.dvSelectFlight.FindItemControl("lbl_list1ID", dvSelectFlight.Items[i]) as HiddenField;
                            list1ID = l.Value;
                            break;
                        }

                    }
                }
                if (ifSelect)
                {
                    if (tr_Return.Visible == true)
                    {
                        ifSelect = false;
                        for (int i = 0; i < this.gvSelectFlightReturn.Items.Count; i++)
                        {
                            RadioButton rb = this.gvSelectFlightReturn.FindItemControl("RadioButton2", gvSelectFlightReturn.Items[i]) as RadioButton;
                            if (rb.Checked)
                            {
                                ifSelect = true;
                                HiddenField l = this.gvSelectFlightReturn.FindItemControl("lbl_list2ID", gvSelectFlightReturn.Items[i]) as HiddenField;
                                list2ID = l.Value;
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
                        DataRow[] foundRows;
                        if (ReturnOnly == true)
                        {
                            available = true;
                            list1ID = "0";
                        }
                        else
                        {
                            strExpr = "TemFlightId = '" + list1ID + "'";

                            strSort = "";

                            dt = (DataTable)HttpContext.Current.Session["TempFlight"];

                            foundRows = dt.Select(strExpr, strSort, DataViewRowState.Added);

                            FillModelFromDataRow(foundRows, ref  model);

                            availableGoing = objBooking.CheckAvailableFare(model, "");
                            if (availableGoing == 1)
                            {
                                available = true;
                            }
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

                        //check booking seat allowance

                        //get maxseat by carriercode
                        string seatQuota = objGeneral.getSysValueByKeyAndCarrierCode("SEATQUOTA", model.Carrier);
                        if (seatQuota == "")
                            seatQuota = "0";
                        int maxSeat = Convert.ToInt16(seatQuota);

                        //get optgroup by carriercode
                        string optGroup = objGeneral.getOPTGroupByCarrierCode(model.Carrier);

                        //get totalBooking for this day by optgroup
                        int totalBook = objBooking.GetTotalBookingByOptGroup(optGroup, model.Carrier, model.FlightNumber);

                        //get available seat
                        int availableSeat = maxSeat - totalBook;

                        if (model.PaxNum > availableSeat)
                        {
                            available = false;
                        }
                        else
                        {
                            available = true;
                        }

                        if (available == true)
                        {
                            HttpCookie cookie2 = new HttpCookie("cookieTemFlight");
                            cookie2.Values.Add("list1ID", list1ID);
                            cookie2.Values.Add("ReturnID", list2ID);
                            cookie2.Values.Add("PaxNum", cookie.Values["PaxNum"]);
                            cookie2.Values.Add("GuestNum", cookie.Values["GuestNum"]);
                            cookie2.Values.Add("ChildNum", cookie.Values["ChildNum"]);
                            Response.AppendCookie(cookie2);

                            e.Result = "";
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
                e.Result = msgList.Err100031;
            }            
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
                //added by ketee
                model.FlightNumber = foundRows[0]["TemFlightFlightNumber"].ToString();   
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this,ex);
            }
        }
        
        protected void btn_Next_Click(object sender, EventArgs e)
        {

        }

    }
}