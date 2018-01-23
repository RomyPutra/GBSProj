using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ABS.Logic.GroupBooking.Booking;
using ABS.Logic.GroupBooking;
using System.Configuration;
using ABS.GBS.Log;
using StackExchange.Profiling;

namespace GroupBooking.Web.Booking
{
	public partial class divideSummary : System.Web.UI.Page
	{
        #region "Declaration"
        LogControl log = new LogControl();
        SystemLog SystemLog = new SystemLog();
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        List<PassengerContainer> passengerContainer = new List<PassengerContainer>();
        List<BookingJourneyContainer> listBookingJourneyContainers = new List<BookingJourneyContainer>(); //for journey list
        List<PassengerContainer> TemppassengerContainer = new List<PassengerContainer>();
        List<PassengerContainer> NewTemppassengerContainer = new List<PassengerContainer>();
        BookingContainer BookingContainer = new BookingContainer();
        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");
        string[] arrstr;
        string keySent, TransID;
        ABS.GBS.eService.ProcessServiceSoapClient eWS = new ABS.GBS.eService.ProcessServiceSoapClient();
        #endregion

        #region "Page Load"

		protected void Page_Load(object sender, EventArgs e)
		{
            //ABS.Logic.SessionLogic SesLogic = new ABS.Logic.SessionLogic();
            BookingControl BookLogic = new BookingControl();
            PassengerContainer objPassengerContainer = new PassengerContainer();
            SessionContextLogic sesscon = new SessionContextLogic();
            int i = 0;
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;
            try
            {
                TransID = Request.QueryString["TransID"];
                keySent = Request.QueryString["k"];
                ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                string hashkey;
                using (profiler.Step("GenerateMac"))
                {
                    hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), TransID.ToString(), "");
                }
                if (hashkey != keySent)
                {
                    Response.Redirect("~/Invalid.aspx");
                    return;
                }

                lbAvailable.Items.Clear();
                TemppassengerContainer = sesscon.GetTempPassengerContainer();

                BookingContainer = sesscon.GetBookingContainer();
                if (TemppassengerContainer != null)
                {
                    arrstr = new string[TemppassengerContainer.Count()];
                    foreach (PassengerContainer objPassenger in TemppassengerContainer)
                    {
                        lbAvailable.Items.Add(objPassenger.FirstName + " " + objPassenger.LastName, objPassenger.PassengerNumber);
                        //update passenger to new pnr 
                        arrstr[i] = objPassenger.PassengerNumber.ToString();
                        i++;
                    }
                }
                else
                {
                    Response.Redirect("~/Invalid.aspx", false);
                }

                //if (!IsPostBack)
                //{    
                    
                //}

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                Response.Redirect("~/Invalid.aspx", false);
            }
            finally
            {
                BookLogic.Dispose();
            }
        }

        #endregion

        #region "Control Event"

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (lblBookingNo.Visible && !string.IsNullOrEmpty(lblBookingNo.Text))
                Response.Redirect("divide.aspx?k=" + keySent + "&TransID=" + TransID, false);
            else
                Response.Redirect("divide.aspx?k=" + keySent + "&TransID=" + TransID + "&recordlocator=" + BookingContainer.RecordLocator, false);
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            BookingControl BookLogic = new BookingControl();
            SessionContextLogic sesscon = new SessionContextLogic();
            List<BookingTransactionDetail> lstBookingDetails = new List<BookingTransactionDetail>();
            List<BookingTransactionDetail> lstOldBookingDetails = new List<BookingTransactionDetail>();
            List<BookingTransactionDetail> listBookingDetail = new List<BookingTransactionDetail>();
            List<BookingTransactionDetail> lstNewBookingDetails = new List<BookingTransactionDetail>();
            string errMsg = "";
            //added by romy, performance monitoring
            var profiler = MiniProfiler.Current;
            MessageList msgList = new MessageList();
            try
            {
                if (TemppassengerContainer != null)
                {
                    string NewPNR = string.Empty;
                    using (profiler.Step("DivideBooking"))
                    {
                        NewPNR = BookLogic.DivideBooking(TemppassengerContainer, BookingContainer, ref errMsg);
                    }
                    if (NewPNR == string.Empty && errMsg == "")
                    {
                        log.Warning(this, "Booking Divided Fail: OLD PNR: " + BookingContainer.RecordLocator);
                        Response.Redirect("~/Invalid.aspx", false);
                    }
                    else
                    {
                        log.Info(this, "Booking Divided: OLD PNR: " + BookingContainer.RecordLocator + " - New PNR: " + NewPNR);
                        //Session["responsedetails"] = null;

                        int DividedPax = 0;
                        using (profiler.Step("GetBookingByPNR_Page"))
                        {
                            if (objBooking.GetBookingByPNR(NewPNR, (ABS.Navitaire.BookingManager.GetBookingResponse)Session["responsedetails"]) == true)
                            {
                                lblBookingNo.Text = NewPNR;
                                divNewPNR.Style.Add("display", "block");
                                lblPassengerListDesc.Text = "Passenger list";
                                lblNote.Visible = false;
                                TemppassengerContainer = sesscon.GetTempPassengerContainer();
                                if (TemppassengerContainer != null)
                                {
                                    DividedPax = TemppassengerContainer.Count;
                                    foreach (PassengerContainer objPassenger in TemppassengerContainer)
                                    {
                                        lbAvailable.Items.Add(objPassenger.FirstName + " " + objPassenger.LastName, objPassenger.PassengerNumber);
                                    }
                                }
                            }
                        }
                        if (sesscon.GetBookingContainer().RecordLocator == NewPNR && sesscon.GetPassengerContainer().Count > 0)
                        {
                            //Response.Redirect("~/page/Summary.aspx?recordlocator=" + NewPNR, false);
                            btnBack.Visible = false;
                            btnBack.Visible = false;
                        }
                        else
                        {
                            log.Warning(this, "PNRs:" + NewPNR + ":" + sesscon.GetBookingContainer().RecordLocator.ToString() + "; passengers count:" + sesscon.GetPassengerContainer().Count);
                        }

                        HttpContext.Current.Session.Remove("OldBookingDetails");

                        lstBookingDetails = objBooking.GetAllBK_TRANSDTLFlightByPNR(BookingContainer.RecordLocator);
                        lstOldBookingDetails = objBooking.GetAllBK_TRANSDTLFilterAll(TransID, 0);
                        listBookingDetail = objBooking.GetAllBK_TRANSDTLFilterAll(TransID, 0); 
                        lstNewBookingDetails = objBooking.GetAllBK_TRANSDTLFlightByPNR(NewPNR);
                        //tyas
                        if (BookingContainer.RecordLocator != NewPNR && ( lstNewBookingDetails == null || lstNewBookingDetails.Count <= 0))
                        {
                            int SeqNo = 0;
                            //remark by Tyas
                            //foreach (BookingTransactionDetail a in lstBookingDetails)
                            //{
                            //    SeqNo = SeqNo + 1;
                            //    a.RecordLocator = NewPNR;
                            //    a.TransID = TransID;
                            //    a.SeqNo = Convert.ToByte(SeqNo);

                            //}

                            //added by tyas
                            if (sesscon.GetBookingJourneyContainer() != null) //grabbing journey details
                            {
                                listBookingJourneyContainers = sesscon.GetBookingJourneyContainer();
                                //lstOldBookingDetails = lstBookingDetails;
                                for (int i = 0; i < lstBookingDetails.Count; i++)
                                {
                                    SeqNo = SeqNo + 1;
                                    lstBookingDetails[i].TransID = TransID;
                                    if (Session["Signature"] != null)
                                    {
                                        lstBookingDetails[i].Signature = Session["Signature"].ToString();
                                    }
                                    else
                                    {
                                        lstBookingDetails[i].Signature = lstBookingDetails[i].Signature;
                                    }
                                    lstBookingDetails[i].FareClass = listBookingJourneyContainers[i].FareClass;
                                    lstBookingDetails[i].FareSellKey = listBookingJourneyContainers[i].FareSellKey;
                                    lstBookingDetails[i].OverridedFareSellKey = listBookingJourneyContainers[i].OverridedFareSellKey;

                                    //added by diana 20170406, to store paydueamount by pax
                                    int TotPax = lstBookingDetails[i].PaxAdult + lstBookingDetails[i].PaxChild;
                                    if (TotPax > 0)
                                    {
                                        lstBookingDetails[i].PayDueAmount1 = lstBookingDetails[i].PayDueAmount1 / TotPax * DividedPax;
                                        lstBookingDetails[i].PayDueAmount2 = lstBookingDetails[i].PayDueAmount2 / TotPax * DividedPax;
                                        lstBookingDetails[i].PayDueAmount3 = lstBookingDetails[i].PayDueAmount3 / TotPax * DividedPax;

                                        for (int y=0;y< lstOldBookingDetails.Count;y++)
                                        {
                                            if (lstBookingDetails[i].SeqNo == lstOldBookingDetails[y].SeqNo && lstBookingDetails[i].RecordLocator == lstOldBookingDetails[y].RecordLocator)
                                            {
                                                lstOldBookingDetails[y].PayDueAmount1 = lstOldBookingDetails[y].PayDueAmount1 - lstBookingDetails[i].PayDueAmount1;
                                                lstOldBookingDetails[y].PayDueAmount2 = lstOldBookingDetails[y].PayDueAmount2 - lstBookingDetails[i].PayDueAmount2;
                                                lstOldBookingDetails[y].PayDueAmount3 = lstOldBookingDetails[y].PayDueAmount3 - lstBookingDetails[i].PayDueAmount3;
                                                break;
                                            }
                                        }

                                        
                                    }
                                    lstBookingDetails[i].CreateBy = "System";
                                    lstBookingDetails[i].RecordLocator = NewPNR;
                                    lstBookingDetails[i].SeqNo = Convert.ToByte(listBookingDetail.Count + SeqNo);

                                    lstOldBookingDetails.Add(lstBookingDetails[i]);
                                }

                                if (lstOldBookingDetails != null && lstOldBookingDetails.Count > 0) HttpContext.Current.Session.Add("OldBookingDetails", lstOldBookingDetails);
                            }

                            using (profiler.Step("SaveListBookingDetail"))
                            {
                                if (objBooking.SaveListBookingDetail(lstBookingDetails, ABS.Logic.Shared.CoreBase.EnumSaveType.Insert) == false)
                                {
                                    log.Error(this, "Booking fail to save: OLD PNR: " + BookingContainer.RecordLocator + " : New PNR: " + NewPNR);
                                    lblErr.Text = msgList.Err300033;
                                }
                                else
                                {
                                    if (arrstr.Length <= 0)
                                    {
                                        log.Error(this, "Booking fail to update passenger list [" + arrstr.Length + "]: OLD PNR: " + BookingContainer.RecordLocator + " : New PNR: " + NewPNR);
                                        lblErr.Text = msgList.Err300033;
                                    }
                                    else
                                    {
                                        objBooking.UpdatePassengerPNRforDivide(BookingContainer.RecordLocator, NewPNR, TransID, arrstr);

                                        List<ListTransaction> AllTransaction = new List<ListTransaction>();
                                        AllTransaction = objBooking.GetTransactionDetails(TransID);
                                        if (AllTransaction != null && AllTransaction.Count > 0)
                                        {
                                            ListTransaction lstTrans = AllTransaction[0];
                                            if (objBooking.UpdateAllBookingJourneyDetailsforDevide(lstTrans, lstTrans.AgentUserName.ToString(), lstTrans.AgentID.ToString(), true) == false)
                                            {
                                                log.Error(this, "Fail to Get Latest Update for Transaction - dividesummary.aspx.cs : " + lstTrans.TransID);
                                                if (ConfigurationManager.AppSettings["ErrorLogEmail"] != null && ConfigurationManager.AppSettings["ErrorLogEmail"] != "")
                                                {
                                                    eWS.CustomEmail(ConfigurationManager.AppSettings["ErrorLogEmail"].ToString(), lstTrans.TransID, "", "Errors Log :" + DateTime.Now.ToString() + " <br/> Fail to Get Latest Update for Transaction : " + lstTrans.TransID);
                                                }
                                            }

                                        }

                                        Response.Redirect(Shared.MySite.PublicPages.BookingDetail + "?k=" + keySent + "&TransID=" + TransID, false);
                                    }
                                }
                            }
                        }
                        else
                        {
                            log.Error(this, "Booking fail to save : OLD PNR: " + BookingContainer.RecordLocator + " : New PNR: " + NewPNR);
                            lblErr.Text = msgList.Err300033;
                        }
                    }
                    
                }

                if (errMsg != "")
                {
                    lblErr.Text = errMsg;
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
                Response.Redirect("~/Invalid.aspx", false);
            }
            finally
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "HidePanel", "LoadingPanel.Hide();", true);
            }
        }

        #endregion

        #region "Function and Procedure"

        #endregion

    }
}