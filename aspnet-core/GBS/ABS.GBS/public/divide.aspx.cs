using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ABS.Logic.GroupBooking.Agent;
using ABS.Logic.GroupBooking;
using ABS.Logic.GroupBooking.Booking;
using ABS.Logic.Shared;
using System.Data;
using DevExpress.Web;
using ABS.GBS.Log;
using StackExchange.Profiling;

namespace GroupBooking.Web.Booking
{
    public partial class divide : System.Web.UI.Page
    {
        #region "Declaration"
        LogControl log = new LogControl();
        SystemLog SystemLog = new SystemLog();
        UserSet AgentSet;
        List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain> TransMainData = new List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>();
        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");
        string keySent, TransID, PNR, formtype;

        List<PassengerContainer> passengerContainer = new List<PassengerContainer>();
        List<PassengerContainer> TemppassengerContainer = new List<PassengerContainer>();
        List<PassengerContainer> TempAvapassengerContainer = new List<PassengerContainer>();
        List<BookingJourneyContainer> BookingJourneyContainers = new List<BookingJourneyContainer>();
        BookingContainer BookingSingleModel = new BookingContainer();
        List<PassengerInfantContainer> tempPassengerInfantModel = new List<PassengerInfantContainer>();
        List<Country_Info> CountryInfo = new List<Country_Info>();
        List<LocationContainer> LocationInfo = new List<LocationContainer>();

        //SessionContextLogic sesscon = new SessionContextLogic();


        //20170428 - Sienny
        bool cekIsErrorCheckbox = false;
        bool cekIsErrorPassenger = false;
        bool cekIsErrorListbox = false;
        #endregion

        #region "Page Load Event"
        protected void Page_Load(object sender, EventArgs e)
        {
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;
            try
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Expires = 0;
                Response.Cache.SetNoStore();
                Response.AddHeader("pragma", "no-store");
                HttpContext.Current.Response.AddHeader("p3p", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");
                PassengerContainer objPassengerContainer = new PassengerContainer();
                SessionContextLogic sesscon = new SessionContextLogic();

                TransID = Request.QueryString["TransID"];
                keySent = Request.QueryString["k"];
                ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                string hashkey;
                using (profiler.Step("GenerateMac"))
                {
                    hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), TransID.ToString(), "");
                }
                //tyas
                //PNR = Request.QueryString["recordlocator"].ToString();
                //Added by Ellis 20170329
                if(hfpnr.Value == "" && Request.QueryString["recordlocator"] == null)
                {
                    if (!IsPostBack && !IsCallback)
                    {
                        if (Request.QueryString["callback"] == null)
                        {
                            using (profiler.Step("ShowData"))
                            {
                                ShowData();
                            }
                            return;
                        }
                    }
                }
                //End of added by Ellis 20170329

                if (hfpnr.Value != "")
                {
                    PNR = hfpnr.Value;
                    hfpnr.Value = "";
                    Response.Redirect("divide.aspx?k=" + hashkey + "&TransID=" + TransID + "&recordlocator=" + PNR, false);
                }

                if (!IsPostBack && Request.QueryString["recordlocator"].ToString() != null)
                {
                    PNR = Request.QueryString["recordlocator"].ToString();
                    //hfpnr.Value = "";
                }

                formtype = "divide";
                if (hashkey != keySent)
                {
                    Response.Redirect("~/Invalid.aspx");
                }


                if (Session["AgentSet"] != null)
                {
                    AgentSet = (UserSet)Session["AgentSet"];
                    if (!IsPostBack)
                    {
                        //clear session
                        sesscon.Reset();
                        //assignDefaultValue();
                        ClientScript.RegisterClientScriptBlock(GetType(), "IsPostBack", "var isPostBack = false;", true);
                        //ViewState["formtype"] = formtype;
                        using (profiler.Step("GetBookingByPNR_Page"))
                        {
                            if (objBooking.GetBookingByPNR(PNR, (ABS.Navitaire.BookingManager.GetBookingResponse)Session["responsedetails"]) == true)
                            {
                                using (profiler.Step("SessconStep"))
                                {
                                    passengerContainer = sesscon.GetPassengerContainer();
                                    TemppassengerContainer = sesscon.GetTempPassengerContainer();
                                    TempAvapassengerContainer = sesscon.GetTempAvaPassengerContainer();
                                    BookingSingleModel = sesscon.GetBookingContainer();
                                    BookingJourneyContainers = sesscon.GetBookingJourneyContainer();
                                }
                                if (ValidateFirst())
                                {
                                    lblBookingNo.Text = PNR;
                                    setForm(formtype, true);
                                    //loadLocation();
                                    lbAvailable.Items.Clear();
                                    using (profiler.Step("PopulateBookingDetails"))
                                    {
                                        PopulateBookingDetails();
                                    }

                                    switch (formtype)
                                    {
                                        case "divide":
                                            foreach (PassengerContainer objPassenger in passengerContainer)
                                            {
                                                if (TemppassengerContainer == null || TemppassengerContainer.FindIndex(item => item.PassengerNumber.ToString() == objPassenger.PassengerNumber.ToString()) < 0)
                                                    lbAvailable.Items.Add(objPassenger.FirstName + " " + objPassenger.LastName + " (" + objPassenger.PaxType + ")", objPassenger.PassengerNumber);
                                            }

                                            if (TemppassengerContainer != null)
                                            {
                                                foreach (PassengerContainer objPassenger in TemppassengerContainer)
                                                {
                                                    lbChoosen.Items.Add(objPassenger.FirstName + " " + objPassenger.LastName, objPassenger.PassengerNumber);
                                                }
                                            }

                                            //ctvRadioList.Enabled = false;
                                            //ctvDOB.Enabled = false;
                                            break;
                                            //case "infant":
                                            //    if (sesscon.GetAllowAddInfant() == "1")
                                            //    {
                                            //        foreach (PassengerContainer objPassenger in passengerContainer)
                                            //        {
                                            //            rdListPassenger.Items.Add(objPassenger.FirstName + " " + objPassenger.LastName + " (" + objPassenger.PaxType + ")", objPassenger.PassengerNumber);
                                            //        }
                                            //        loadCountry(ddlCountry);
                                            //        loadCountry(ddlPassportCountry);
                                            //        loadYear(ddlBirthYears, ddlBirthMonths, ddlBirthDays);
                                            //        loadYearExp(ddlYearsExp, ddlMonthsExp, ddlDaysExp);

                                            //        ctvListBox.Enabled = false;
                                            //        ctvPassengers.Enabled = false;
                                            //        divPassengerinfantlist.Style["display"] = "";
                                            //    }
                                            //    else
                                            //    {
                                            //        string flight = "";
                                            //        flight = sesscon.GetAllowAddInfant_FlightDesignator();
                                            //        btnContinue.Visible = false;
                                            //        CheckBoxAgreement.Enabled = false;
                                            //        lblErr.Text = "Sorry, the maximum number of infants on flight " + flight + " has been reached.";
                                            //    }
                                            //    break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(GetType(), "IsPostBack", "var isPostBack = true;", true);
                        //if (ViewState["formtype"] != null)
                        //    formtype = ViewState["formtype"].ToString();
                        //else
                        //{
                        //    //log.Error("Invalid formtype :" + formtype);
                        //    Response.Redirect("~/Invalid.aspx", false);
                        //}
                        ClearMsg();

                        passengerContainer = sesscon.GetPassengerContainer();
                        TemppassengerContainer = sesscon.GetTempPassengerContainer();
                        TempAvapassengerContainer = sesscon.GetTempAvaPassengerContainer();
                        BookingSingleModel = sesscon.GetBookingContainer();
                        BookingJourneyContainers = sesscon.GetBookingJourneyContainer();
                        //BindCountry();
                        using (profiler.Step("loadLocation"))
                        {
                            loadLocation();
                        }
                        using (profiler.Step("setForm"))
                        {
                            setForm(formtype, true);
                        }
                        using (profiler.Step("PopulateBookingDetails"))
                        {
                            PopulateBookingDetails();
                        }
                    }
                    //LoadGridView();

                    

                }
                else
                {
                    Response.Redirect(GroupBooking.Web.Shared.MySite.PublicPages.SessionExpired);
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);

            }
            finally
            {

            }
        }

        #endregion

        #region "Event Control"
        protected void CheckBoxAgreement_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxAgreement.Checked)
            {
                cekIsErrorCheckbox = false;//20170428 - Sienny
                pnlmsgerror.Visible = false;
            }
            else
            {
                cekIsErrorCheckbox = true;//20170428 - Sienny
                pnlmsgerror.Visible = true;
            }
        }

        protected void ctvAgreement_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            pnlmsgerror.Visible = false;
            if (CheckBoxAgreement.Checked)
            {
                e.IsValid = true;
                cekIsErrorCheckbox = false;//20170428 - Sienny
            }
            else
            {
                e.IsValid = false;
                cekIsErrorCheckbox = true;//20170428 - Sienny
                pnlmsgerror.Visible = true;
            }
        }

        protected void ctvListBox_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            MessageList msgList = new MessageList();
            if (lbAvailable.Items.Count > 0 && lbChoosen.Items.Count > 0)
            {
                e.IsValid = true;
                cekIsErrorListbox = false;//20170428 - Sienny
            }
            else
            {
                e.IsValid = false;
                cekIsErrorListbox = true;//20170428 - Sienny
                lblErr.Text = msgList.Err300024;
                pnlErr.Visible = true;
            }
        }

        protected void ctvPassenger_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            MessageList msgList = new MessageList();
            e.IsValid = false;
            cekIsErrorPassenger = true;//20170428 - Sienny
            //added by ketee, check available pax in the original flight
            if (passengerContainer.Count - lbChoosen.Items.Count < 10)
            {
                lblErr.Text = msgList.Err300025;
                pnlErr.Visible = true;
                return;
            }

            //validate passenger in available box
            
            for (int i = 0; i < lbAvailable.Items.Count; i++)
            {
                int iIndex = passengerContainer.FindIndex(item => item.PassengerNumber.ToString() == lbAvailable.Items[i].Value.ToString());
                if (iIndex >= 0)
                {
                    if (passengerContainer[iIndex].PaxType.ToString() != "CHD")
                    {
                        e.IsValid = true;
                        cekIsErrorPassenger = false;//20170428 - Sienny
                        break;
                    }
                }
            }

            if (e.IsValid == true)
            {
                e.IsValid = false;
                cekIsErrorPassenger = true;//20170428 - Sienny
                for (int i = 0; i < lbChoosen.Items.Count; i++)
                {
                    int iIndex = passengerContainer.FindIndex(item => item.PassengerNumber.ToString() == lbChoosen.Items[i].Value.ToString());
                    if (iIndex >= 0)
                    {
                        if (passengerContainer[iIndex].PaxType.ToString() != "CHD" && passengerContainer[iIndex].WCHR == 0)
                        {
                            e.IsValid = true;
                            e.IsValid = true;
                            cekIsErrorPassenger = false;//20170428 - Sienny
                            cekIsErrorPassenger = false;//20170428 - Sienny
                            break;
                        }
                    }
                }
                if (e.IsValid == false)
                {
                    cekIsErrorPassenger = true;//20170428 - Sienny
                    lblErr.Text = msgList.Err300026;
                    pnlErr.Visible = true;
                }
            }
            else
            {
                cekIsErrorPassenger = true;//20170428 - Sienny
                lblErr.Text = msgList.Err300027;
                pnlErr.Visible = true;
            }
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            SessionContextLogic sesscon = new SessionContextLogic();
            List<PassengerContainer> tempPassengerContainer = new List<PassengerContainer>();
            List<PassengerContainer> PassengerContainer = new List<PassengerContainer>();
            PassengerInfantContainer PassengerInfantModel = new PassengerInfantContainer();
            try
            {
                PassengerContainer = sesscon.GetPassengerContainer();

                if (formtype == "divide")
                {
                    //load selected passenger into temp model
                    for (int i = 0; i < lbChoosen.Items.Count; i++)
                    {
                        int iIndex = 0;
                        iIndex = PassengerContainer.FindIndex(item => item.PassengerNumber.ToString() == lbChoosen.Items[i].Value.ToString());
                        tempPassengerContainer.Add(PassengerContainer[iIndex]);
                    }
                }

                //if (formtype == "infant")
                //{
                //    if (rdListPassenger.Value == null)
                //    {
                //        lblErr.Text = "No guest been selected.";
                //        ClientScript.RegisterClientScriptBlock(GetType(), "HidePanel", "LoadingPanel.Hide();", true);
                //        return;
                //    }

                //    if (PassengerContainer[Convert.ToInt32(rdListPassenger.SelectedItem.Value)] != null)
                //        tempPassengerContainer.Add(PassengerContainer[Convert.ToInt32(rdListPassenger.SelectedItem.Value)]);
                //    else
                //    {
                //        lblErr.Text = "No guest been selected.";
                //        ClientScript.RegisterClientScriptBlock(GetType(), "HidePanel", "LoadingPanel.Hide();", true);
                //        return;
                //    }

                //    //load infant details into tempInfant container
                //    PassengerInfantContainer NewInfantContainer = new PassengerInfantContainer();
                //    NewInfantContainer.RecordLocator = BookingSingleModel.RecordLocator;
                //    NewInfantContainer.PassengerID = Convert.ToInt16(rdListPassenger.SelectedItem.Value);

                //    NewInfantContainer.Nationality = ddlCountry.SelectedItem.Value.ToString();
                //    NewInfantContainer.FirstName = txtSurName.Text;
                //    NewInfantContainer.LastName = txtGivenName.Text;
                //    NewInfantContainer.Gender = ddlGender.SelectedItem.Value.ToString();
                //    if (ddlBirthYears.SelectedIndex > 0 && ddlBirthMonths.SelectedIndex > 0 && ddlBirthDays.SelectedIndex > 0)
                //    {
                //        NewInfantContainer.DOB = Convert.ToDateTime(ddlBirthYears.SelectedItem.Value.ToString() + "-" + ddlBirthMonths.SelectedItem.Value.ToString() + "-" + ddlBirthDays.SelectedItem.Value.ToString());
                //    }
                //    else
                //    {
                //        lblErr.Text = "Infant's age should be between 9 days old and less than 24 months at time of travel.";
                //        ClientScript.RegisterClientScriptBlock(GetType(), "HidePanel", "LoadingPanel.Hide();", true);
                //        return;
                //    }
                //    NewInfantContainer.PassportNumber = txtPassportNum.Text;
                //    if (ddlPassportCountry.SelectedIndex >= 0)
                //        NewInfantContainer.IssueCountry = ddlPassportCountry.SelectedItem.Value.ToString();

                //    if ((ddlYearsExp.SelectedIndex > 0 || ddlMonthsExp.SelectedIndex > 0 || ddlDaysExp.SelectedIndex > 0))
                //    {
                //        if (ddlYearsExp.SelectedIndex > 0 && ddlMonthsExp.SelectedIndex > 0 && ddlDaysExp.SelectedIndex > 0)
                //        {
                //            NewInfantContainer.ExpiryDate = Convert.ToDateTime(ddlYearsExp.SelectedItem.Value.ToString() + "-" + ddlMonthsExp.SelectedItem.Value.ToString() + "-" + ddlDaysExp.SelectedItem.Value.ToString());

                //        }
                //        else
                //        {
                //            lblErr.Text = "Invalid Expiry Date.";
                //            ClientScript.RegisterClientScriptBlock(GetType(), "HidePanel", "LoadingPanel.Hide();", true);
                //            return;
                //        }
                //    }

                //    tempPassengerInfantModel.Add(NewInfantContainer);
                //}

                Validate("mandatory");
                if (Page.IsValid && tempPassengerContainer != null && formtype == "divide")
                {
                    sesscon.SetTempPassengerContainer(tempPassengerContainer);
                    Response.Redirect(Shared.MySite.PublicPages.DivideSummary + "?k=" + keySent + "&TransID=" + TransID + "&recordlocator=" + PNR, false);
                }
                //else if (Page.IsValid && tempPassengerContainer != null && tempPassengerInfantModel != null && formtype == "infant")
                //{
                //    sesscon.SetTempPassengerContainer(tempPassengerContainer);
                //    sesscon.SetTempPassengerInfantContainer(tempPassengerInfantModel);
                //    Response.Redirect(Shared.MySite.PublicPages.DivideSummary);
                //}
                else
                {
                    //if (lblErr.Text == string.Empty)
                    //    lblErr.Text = "No passenger is selected.";
                }

                //ClientScript.RegisterClientScriptBlock(GetType(), "HidePanel", "LoadingPanel.Hide();", true);
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                //ClientScript.RegisterClientScriptBlock(GetType(), "HidePanel", "LoadingPanel.Hide();", true);
            }
            finally
            {
                sesscon = null;
                tempPassengerContainer = null;
                PassengerContainer = null;
                //ClientScript.RegisterClientScriptBlock(GetType(), "HidePanel", "LoadingPanel.Hide();", true);
            }
        }

        //20170427 - Sienny
        protected void ValidatePopup(object sender, CallbackEventArgs e)
        {
            MessageList msgList = new MessageList();
            try
            {
                if (!cekIsErrorCheckbox && !cekIsErrorListbox && !cekIsErrorPassenger)
                {                    
                    SessionContextLogic sesscon = new SessionContextLogic();
                    List<PassengerContainer> tempPassengerContainer = new List<PassengerContainer>();
                    List<PassengerContainer> PassengerContainer = new List<PassengerContainer>();
                    PassengerInfantContainer PassengerInfantModel = new PassengerInfantContainer();

                    PassengerContainer = sesscon.GetPassengerContainer();

                    if (formtype == "divide")
                    {
                        //load selected passenger into temp model
                        for (int i = 0; i < lbChoosen.Items.Count; i++)
                        {
                            int iIndex = 0;
                            iIndex = PassengerContainer.FindIndex(item => item.PassengerNumber.ToString() == lbChoosen.Items[i].Value.ToString());
                            tempPassengerContainer.Add(PassengerContainer[iIndex]);
                        }
                    }

                    //Validate("mandatory");
                    e.Result = "";

                    if (Page.IsValid && tempPassengerContainer != null && formtype == "divide")
                    {
                        sesscon.SetTempPassengerContainer(tempPassengerContainer);
                        if (Page.IsCallback)
                            DevExpress.Web.ASPxWebControl.RedirectOnCallback(Shared.MySite.PublicPages.DivideSummary + "?k=" + keySent + "&TransID=" + TransID + "&recordlocator=" + PNR);
                        else
                            Response.Redirect(Shared.MySite.PublicPages.DivideSummary + "?k=" + keySent + "&TransID=" + TransID + "&recordlocator=" + PNR, false);
                    }
                }
                else if (cekIsErrorListbox || cekIsErrorPassenger)
                {
                    if (lblErr.Text != "")
                        e.Result = lblErr.Text;
                        pnlErr.Visible = true;
                }
                else if (cekIsErrorCheckbox)
                {
                    if (pnlmsgerror.Visible)
                        e.Result = msgList.Err300028;
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
        }

        //Added by Ellis 20170329
        protected void btnBack_Click(object sender, EventArgs e)
        {
            TransID = Request.QueryString["TransID"];
            keySent = Request.QueryString["k"];
            Response.Redirect("divide.aspx?k=" + keySent + "&TransID=" + TransID , false);
        }
        //End of added by Ellis 20170329
        #endregion

        #region "Initializer"

        #endregion

        #region "Function and Procedure"
        //Added by Ellis 20170329
        protected void ShowData()
        {
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;
            try
            {
                //lblBookingID.Text = TransID;
                lblTransID.Text = TransID;

                DataTable dt;
                using (profiler.Step("GetBK_AllPNR"))
                {
                    dt = objBooking.GetBK_AllPNR(TransID);
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    //LeftSide PNR List
                    Session["ListPNR"] = dt;
                    dt.DefaultView.RowFilter = "RecordLocator <> 'All' AND PNR <> 'All'";
                    rptPNR.DataSource = dt.DefaultView;
                    rptPNR.DataBind();
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                throw ex;
            }
            finally
            {
                objBooking = null;
            }
        }
        //End of added by Ellis 20170329
        protected Boolean ValidateFirst()
        {
            MessageList msgList = new MessageList();
            BookingControl BookLogic = new BookingControl();
            try
            {
                if (formtype == "divide")
                {
                    if (BookingJourneyContainers.Count > 0 && (BookingJourneyContainers[0].STD - DateTime.Now).TotalHours <= 48)
                    {
                        lblErr.Text = msgList.Err300029;
                        //Added by Ellis 20170329
                        pnlErr.Visible = true;
                        ShowData();
                        return false;
                    }

                    if (passengerContainer != null && BookingSingleModel != null)
                    {
                        //verify numbers of passengers , not allow divide booking with more than 9 ppl or less than 2 ppl
                        //modify condition by ketee, not alow divide booking with pax less than 10 ppls
                        if (passengerContainer.Count <= 10)
                        {
                            lblErr.Text = msgList.Err300030;
                            //Added by Ellis 20170329
                            pnlErr.Visible = true;
                            ShowData();
                            return false;
                        }
                        int totalPax = passengerContainer.Count;
                        foreach (PassengerContainer objPassenger in passengerContainer)
                        {
                            int age = 0;
                            if (objPassenger.DOB != null)
                            {
                                age = DateTime.Now.Year - objPassenger.DOB.Year;
                                if (age >= 0)
                                {
                                    if (DateTime.Now.Month < objPassenger.DOB.Month || DateTime.Now.Month == objPassenger.DOB.Month && DateTime.Now.Day < objPassenger.DOB.Day) age--;
                                }
                                else
                                {
                                    age = 16;
                                }
                            }
                            //Check on passenger Pax Type if is Child and age <= 15 and with wheel chair, this passenger consider not an Adult
                            if (objPassenger.PaxType.ToString() == "CHD" || age <= 15 || objPassenger.WCHR == 1)
                            {
                                totalPax--;
                            }
                        }

                        //Divide Booking must contain at least 2 Adult passengers
                        if (totalPax <= 1)
                        {
                            lblErr.Text = msgList.Err300030;
                            //Added by Ellis 20170329
                            pnlErr.Visible = true;
                            ShowData();
                            return false;
                        }

                        //Validate on the payment status
                        if (objBooking.GetBookingPayment(BookingSingleModel.RecordLocator) == false)
                        {
                            lblErr.Text = msgList.Err300031;
                            //Added by Ellis 20170329
                            pnlErr.Visible = true;
                            ShowData();
                            return false;
                        }

                        //validate on web check in status
                        foreach (PassengerContainer rowPasssenger in passengerContainer)
                        {
                            if (rowPasssenger.getLiftStatus(PassengerContainer.FlightType.Depart).ToString().ToLower() == "checkedin")
                            {
                                lblErr.Text = msgList.Err300032;
                                //Added by Ellis 20170329
                                pnlErr.Visible = true;
                                ShowData();
                                return false;
                            }

                        }

                    }
                    else
                    {
                        return false;
                    }
                }
                else if (formtype == "infant")
                {
                    if (passengerContainer == null || BookingSingleModel == null)
                    {
                        lblErr.Text = msgList.Err100063;
                        pnlErr.Visible = true;
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                Response.Redirect("~/Invalid.aspx", false);
                return false;
            }
        }

        protected void ClearMsg()
        {
            lblErr.Text = "";
            pnlErr.Visible = false;
            //lblMsg.Text = "";
        }

        protected void setForm(string formtype, Boolean visible)
        {
            switch (formtype)
            {
                //case "infant":
                //    pnlMain.Visible = visible;
                //    pnlPassengerInfant.Visible = visible;
                //    pnlAgreement.Visible = visible;
                //    tblInfant.Style["display"] = "";
                //    break;
                case "divide":
                    pnlMain.Visible = visible;
                    pnlPassagerInfo.Visible = visible;
                    pnlAgreement.Visible = visible;
                    //tblInfant.Style["display"] = "none";
                    pnlDefault.Visible = !visible;
                    break;
            }
        }

        protected void PopulateBookingDetails()
        {
            try
            {
                if (BookingSingleModel != null)
                {
                    lblBookingNo.Text = BookingSingleModel.RecordLocator.ToString();

                }
                int i = 0;
                int icount = 0;

                if (BookingJourneyContainers != null)
                {
                    foreach (BookingJourneyContainer objBookingourney in BookingJourneyContainers)
                    {
                        switch (i)
                        {
                            case 0:
                                divDepart.Visible = true;
                                icount = 0;
                                lblNewDepartCode.Text = objBookingourney.DepartureStation + " - " + objBookingourney.ArrivalStation;
                                lblNewDepartFlight.Text = objBookingourney.CarrierCode + " " + objBookingourney.FlightNumber;
                                lblNewDepartStationFrom.Text = getLocationName(objBookingourney.DepartureStation) + "(" + objBookingourney.DepartureStation + ")";
                                lblNewDepartDateFrom.Text = objBookingourney.STD.ToString("dddd, dd MMMM yyyy, HHmm (h:mm tt)");

                                if (objBookingourney.DepartureStation.ToLower().ToString() == "kul")
                                {
                                    lblNewDepartAirportFrom.Text = getLocationName(objBookingourney.DepartureStation) + " LCCT";
                                }
                                else
                                {
                                    lblNewDepartAirportFrom.Text = getLocationName(objBookingourney.DepartureStation);
                                }

                                lblNewDepartStationTo.Text = getLocationName(objBookingourney.ArrivalStation) + "(" + objBookingourney.ArrivalStation + ")";
                                lblNewDepartDateTo.Text = objBookingourney.STA.ToString("dddd, dd MMMM yyyy, HHmm (h:mm tt)");
                                if (objBookingourney.ArrivalStation.ToLower().ToString() == "kul")
                                {
                                    lblNewDepartAirportTo.Text = getLocationName(objBookingourney.ArrivalStation) + " LCCT";
                                }
                                else
                                {
                                    lblNewDepartAirportTo.Text = getLocationName(objBookingourney.ArrivalStation);
                                }

                                if (objBookingourney.JourneySellKey.ToString().Contains("^"))
                                {
                                    lblNewDepartCode.Text = objBookingourney.DepartureStation + " - " + objBookingourney.ArrivalStation + " - " + objBookingourney.OverridedArrivalStation;
                                    lblNewDepartCFlight.Text = objBookingourney.OverridedCarrierCode + " " + objBookingourney.OverridedFlightNumber;
                                    lblNewDepartCStationFrom.Text = getLocationName(objBookingourney.OverridedDepartureStation) + "(" + objBookingourney.OverridedDepartureStation + ")";
                                    lblNewDepartCDateFrom.Text = objBookingourney.OverridedSTD.ToString("dddd, dd MMMM yyyy, HHmm (h:mm tt)");
                                    if (objBookingourney.OverridedDepartureStation.ToLower().ToString() == "kul")
                                    {
                                        lblNewDepartCAirportFrom.Text = getLocationName(objBookingourney.OverridedDepartureStation) + " LCCT";
                                    }
                                    else
                                    {
                                        lblNewDepartCAirportFrom.Text = getLocationName(objBookingourney.OverridedDepartureStation);
                                    }

                                    lblNewDepartCStationTo.Text = getLocationName(objBookingourney.OverridedArrivalStation) + "(" + objBookingourney.OverridedArrivalStation + ")";
                                    lblNewDepartCDateTo.Text = objBookingourney.OverridedSTA.ToString("dddd, dd MMMM yyyy, HHmm (h:mm tt)");
                                    if (objBookingourney.OverridedArrivalStation.ToLower().ToString() == "kul")
                                    {
                                        lblNewDepartCAirportTo.Text = getLocationName(objBookingourney.OverridedArrivalStation) + " LCCT";
                                    }
                                    else
                                    {
                                        lblNewDepartCAirportTo.Text = getLocationName(objBookingourney.OverridedArrivalStation);
                                    }
                                    divDepartC.Visible = true;
                                    icount = 2;
                                }
                                break;

                            case 1:
                                divReturn.Visible = true;
                                icount = 1;
                                lblReturnCode.Text = objBookingourney.DepartureStation + " - " + objBookingourney.ArrivalStation;
                                lblNewReturnFlight.Text = objBookingourney.CarrierCode + " " + objBookingourney.FlightNumber;
                                lblNewReturnStationFrom.Text = getLocationName(objBookingourney.DepartureStation) + "(" + objBookingourney.DepartureStation + ")";
                                lblNewReturnDateFrom.Text = objBookingourney.STD.ToString("dddd, dd MMMM yyyy, HHmm (h:mm tt)");
                                if (objBookingourney.DepartureStation.ToLower().ToString() == "kul")
                                {
                                    lblNewReturnAirportFrom.Text = getLocationName(objBookingourney.DepartureStation) + " LCCT";
                                }
                                else
                                {
                                    lblNewReturnAirportFrom.Text = getLocationName(objBookingourney.DepartureStation);
                                }

                                lblNewReturnStationTo.Text = getLocationName(objBookingourney.ArrivalStation) + "(" + objBookingourney.ArrivalStation + ")";
                                lblNewReturnDateTo.Text = objBookingourney.STA.ToString("dddd, dd MMMM yyyy, HHmm (h:mm tt)");
                                if (objBookingourney.ArrivalStation.ToLower().ToString() == "kul")
                                {
                                    lblNewReturnAirportTo.Text = getLocationName(objBookingourney.ArrivalStation) + " LCCT";
                                }
                                else
                                {
                                    lblNewReturnAirportTo.Text = getLocationName(objBookingourney.ArrivalStation);
                                }

                                if (objBookingourney.JourneySellKey.ToString().Contains("^"))
                                {
                                    lblReturnCode.Text = objBookingourney.DepartureStation + " - " + objBookingourney.ArrivalStation + " - " + objBookingourney.OverridedArrivalStation;
                                    lblNewReturnCFlight.Text = objBookingourney.OverridedCarrierCode + " " + objBookingourney.OverridedFlightNumber;
                                    lblNewReturnCStationFrom.Text = getLocationName(objBookingourney.OverridedDepartureStation) + "(" + objBookingourney.OverridedDepartureStation + ")";
                                    lblNewReturnCDateFrom.Text = objBookingourney.OverridedSTD.ToString("dddd, dd MMMM yyyy, HHmm (h:mm tt)");
                                    if (objBookingourney.OverridedDepartureStation.ToLower().ToString() == "kul")
                                    {
                                        lblNewReturnCAirportFrom.Text = getLocationName(objBookingourney.OverridedDepartureStation) + " LCCT";
                                    }
                                    else
                                    {
                                        lblNewReturnCAirportFrom.Text = getLocationName(objBookingourney.OverridedDepartureStation);
                                    }

                                    lblNewReturnCStationTo.Text = getLocationName(objBookingourney.OverridedArrivalStation) + "(" + objBookingourney.OverridedArrivalStation + ")";
                                    lblNewReturnCDateTo.Text = objBookingourney.OverridedSTA.ToString("dddd, dd MMMM yyyy, HHmm (h:mm tt)");
                                    if (objBookingourney.OverridedArrivalStation.ToLower().ToString() == "kul")
                                    {
                                        lblNewReturnCAirportTo.Text = getLocationName(objBookingourney.OverridedArrivalStation) + " LCCT";
                                    }
                                    else
                                    {
                                        lblNewReturnCAirportTo.Text = getLocationName(objBookingourney.OverridedArrivalStation);
                                    }
                                    icount = 3;
                                    divReturnC.Visible = true;
                                }
                                break;
                        }

                        i++;

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

        private string getLocationName(string LocationCode)
        {
            int iIndex = 0;
            if ((iIndex = LocationInfo.FindIndex(a => a.LocationCode == LocationCode)) >= 0)
            {
                string stdStation = string.Empty;
                stdStation = LocationInfo[iIndex].Name.ToString();
                return stdStation;
            }
            else
                return LocationCode;
        }

        private void loadLocation()
        {
            BookingControl bookingLogic = new BookingControl();
            GeneralControl GeneralLogic = new GeneralControl();
            DataTable dt = new DataTable();
            LocationInfo = bookingLogic.getLocation();

        }
        #endregion
    }
}