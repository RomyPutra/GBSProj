using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ABS.Logic.GroupBooking.Booking;
using ABS.Logic.GroupBooking;
using System.Configuration;

namespace GroupBooking.Web.Booking
{
    public partial class DivideValidation : System.Web.UI.Page
    {
        #region "Declaration"
        LogControl log = new LogControl();
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

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            BookingControl BookLogic = new BookingControl();
            SessionContextLogic sesscon = new SessionContextLogic();
            List<BookingTransactionDetail> lstBookingDetails = new List<BookingTransactionDetail>();
            List<BookingTransactionDetail> listBookingDetail = new List<BookingTransactionDetail>();
            List<BookingTransactionDetail> lstNewBookingDetails = new List<BookingTransactionDetail>();
            string errMsg = "";

            if (txtNewPNR.Text.Trim().Length != 6 || txtOriPNR.Text.Trim().Length != 6)
                return;
            try
            {
                if (objBooking.GetBookingByPNR(txtOriPNR.Text.Trim(), (ABS.Navitaire.BookingManager.GetBookingResponse)Session["responsedetails"]) == true)
                {
                    BookingContainer = sesscon.GetBookingContainer();
                    passengerContainer = sesscon.GetPassengerContainer();
                    TemppassengerContainer = sesscon.GetTempPassengerContainer();
                    //TempAvapassengerContainer = sesscon.GetTempAvaPassengerContainer();
                    //BookingSingleModel = sesscon.GetBookingContainer();
                    //BookingJourneyContainers = sesscon.GetBookingJourneyContainer();
                    string NewPNR = string.Empty;
                    NewPNR = txtNewPNR.Text.Trim();
                    if (NewPNR == string.Empty && errMsg == "")
                    {
                        log.Warning(this, "Booking Divided Fail: OLD PNR: " + BookingContainer.RecordLocator);
                        Response.Redirect("~/Invalid.aspx", false);
                    }
                    else
                    {
                        log.Info(this, "Booking Divided: OLD PNR: " + BookingContainer.RecordLocator + " - New PNR: " + NewPNR);
                        //Session["responsedetails"] = null;
                        if (objBooking.GetBookingByPNR(NewPNR, (ABS.Navitaire.BookingManager.GetBookingResponse)Session["responsedetails"]) == true)
                        {
                            lblBookingNo.Text = NewPNR;
                            divNewPNR.Style.Add("display", "block");
                            lblPassengerListDesc.Text = "Passenger list";
                            lblNote.Visible = false;
                            TemppassengerContainer = sesscon.GetPassengerContainer();
                            if (TemppassengerContainer != null)
                            {
                                foreach (PassengerContainer objPassenger in TemppassengerContainer)
                                {
                                    lbAvailable.Items.Add(objPassenger.FirstName + " " + objPassenger.LastName, objPassenger.PassengerNumber);
                                }
                            }
                        }
                        
                        if (sesscon.GetBookingContainer().RecordLocator == NewPNR && sesscon.GetPassengerContainer().Count > 0)
                        {
                            //Response.Redirect("~/page/Summary.aspx?recordlocator=" + NewPNR, false);
                            //btnBack.Visible = false;
                            //btnBack.Visible = false;
                        }
                        else
                        {
                            log.Warning(this, "PNRs:" + NewPNR + ":" + sesscon.GetBookingContainer().RecordLocator.ToString() + "; passengers count:" + sesscon.GetPassengerContainer().Count);
                        }

                        
                        //tyas
                        if (BookingContainer != null && BookingContainer.RecordLocator.Trim().Length == 6 && BookingContainer.RecordLocator != NewPNR && (lstNewBookingDetails == null || lstNewBookingDetails.Count <= 0))
                        {
                            lstBookingDetails = objBooking.GetAllBK_TRANSDTLFlightByPNR(BookingContainer.RecordLocator);
                            listBookingDetail = objBooking.GetAllBK_TRANSDTLFilterAll(TransID, 0);
                            lstNewBookingDetails = objBooking.GetAllBK_TRANSDTLFlightByPNR(NewPNR);
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
                                for (int i = 0; i < lstBookingDetails.Count; i++)
                                {
                                    SeqNo = SeqNo + 1;
                                    lstBookingDetails[i].RecordLocator = NewPNR;
                                    lstBookingDetails[i].TransID = TransID;
                                    lstBookingDetails[i].SeqNo = Convert.ToByte(listBookingDetail.Count + SeqNo);
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
                                    lstBookingDetails[i].CreateBy = "System";
                                }
                            }

                            if (objBooking.SaveListBookingDetail(lstBookingDetails, ABS.Logic.Shared.CoreBase.EnumSaveType.Insert) == false)
                            {
                                log.Error(this, "Booking fail to save: OLD PNR: " + BookingContainer.RecordLocator + " : New PNR: " + NewPNR);
                                lblErr.Text = "Fail to save new PNR, please contact system administrator.";
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
                                        log.Error(this, "Fail to Get Latest Update for Transaction : " + lstTrans.TransID);
                                        if (ConfigurationManager.AppSettings["ErrorLogEmail"] != null && ConfigurationManager.AppSettings["ErrorLogEmail"] != "")
                                        {
                                            eWS.CustomEmail(ConfigurationManager.AppSettings["ErrorLogEmail"].ToString(), lstTrans.TransID, "", "Errors Log :" + DateTime.Now.ToString() + " <br/> Fail to Get Latest Update for Transaction : " + lstTrans.TransID);
                                        }
                                    }

                                }

                                Response.Redirect(Shared.MySite.PublicPages.BookingDetail + "?k=" + keySent + "&TransID=" + TransID, false);
                                //if (arrstr.Length <= 0)
                                //{
                                //    log.Error(this, "Booking fail to update passenger list [" + arrstr.Length + "]: OLD PNR: " + BookingContainer.RecordLocator + " : New PNR: " + NewPNR);
                                //    lblErr.Text = "Fail to save new PNR, please contact system administrator.";
                                //}
                                //else
                                //{

                                //}
                            }
                        }
                        else
                        {
                            log.Error(this, "Booking fail to save : OLD PNR: " + BookingContainer.RecordLocator + " : New PNR: " + NewPNR);
                            lblErr.Text = "Fail to save new PNR, please contact system administrator.";
                        }
                    }
                }

                    if (TemppassengerContainer != null)
                {
                    

                }

                if (errMsg != "")
                {
                    lblErr.Text = errMsg;
                }
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                Response.Redirect("~/Invalid.aspx", false);
            }
            finally
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "HidePanel", "LoadingPanel.Hide();", true);
            }
        }

        protected void btnValidate_Click(object sender, EventArgs e)
        {
            BookingControl BookLogic = new BookingControl();
            SessionContextLogic sesscon = new SessionContextLogic();
            List<BookingTransactionDetail> lstBookingDetails = new List<BookingTransactionDetail>();
            List<BookingTransactionDetail> listBookingDetail = new List<BookingTransactionDetail>();
            List<BookingTransactionDetail> lstNewBookingDetails = new List<BookingTransactionDetail>();
            string errMsg = "";

            if (txtNewPNR.Text.Trim().Length != 6 || txtOriPNR.Text.Trim().Length != 6)
                return;
            try
            {
                if (objBooking.GetBookingByPNR(txtOriPNR.Text.Trim(), (ABS.Navitaire.BookingManager.GetBookingResponse)Session["responsedetails"]) == true)
                {
                    passengerContainer = sesscon.GetPassengerContainer();
                    TemppassengerContainer = sesscon.GetTempPassengerContainer();
                    //TempAvapassengerContainer = sesscon.GetTempAvaPassengerContainer();
                    //BookingSingleModel = sesscon.GetBookingContainer();
                    //BookingJourneyContainers = sesscon.GetBookingJourneyContainer();
                    string NewPNR = string.Empty;
                    NewPNR = txtNewPNR.Text.Trim();
                    if (NewPNR == string.Empty && errMsg == "")
                    {
                        log.Warning(this, "Booking Divided Fail: OLD PNR: " + BookingContainer.RecordLocator);
                        Response.Redirect("~/Invalid.aspx", false);
                    }
                    else
                    {
                        log.Info(this, "Booking Divided: OLD PNR: " + BookingContainer.RecordLocator + " - New PNR: " + NewPNR);
                        //Session["responsedetails"] = null;
                        if (objBooking.GetBookingByPNR(NewPNR, (ABS.Navitaire.BookingManager.GetBookingResponse)Session["responsedetails"]) == true)
                        {
                            lblBookingNo.Text = NewPNR;
                            divNewPNR.Style.Add("display", "block");
                            lblPassengerListDesc.Text = "Passenger list";
                            lblNote.Visible = false;
                            TemppassengerContainer = sesscon.GetPassengerContainer();
                            if (TemppassengerContainer != null)
                            {
                                foreach (PassengerContainer objPassenger in TemppassengerContainer)
                                {
                                    lbAvailable.Items.Add(objPassenger.FirstName + " " + objPassenger.LastName, objPassenger.PassengerNumber);
                                }
                            }
                        }
                        if (sesscon.GetBookingContainer().RecordLocator == NewPNR && sesscon.GetPassengerContainer().Count > 0)
                        {
                            lblErr.Text = "";
                            //Response.Redirect("~/page/Summary.aspx?recordlocator=" + NewPNR, false);
                            //btnBack.Visible = false;
                            //btnBack.Visible = false;
                            btnConfirm.Enabled = true;
                        }
                        else
                        {
                            lblErr.Text = "PNRs:" + NewPNR + ":" + sesscon.GetBookingContainer().RecordLocator.ToString() + "; passengers count:" + sesscon.GetPassengerContainer().Count;
                            log.Warning(this, "PNRs:" + NewPNR + ":" + sesscon.GetBookingContainer().RecordLocator.ToString() + "; passengers count:" + sesscon.GetPassengerContainer().Count);
                        }

                    }
                }

                if (TemppassengerContainer != null)
                {


                }

                if (errMsg != "")
                {
                    lblErr.Text = errMsg;
                }
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                Response.Redirect("~/Invalid.aspx", false);
            }
            finally
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "HidePanel", "LoadingPanel.Hide();", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BookingControl BookLogic = new BookingControl();
            PassengerContainer objPassengerContainer = new PassengerContainer();
            SessionContextLogic sesscon = new SessionContextLogic();
            int i = 0;
            try
            {
                TransID = Request.QueryString["TransID"];
                keySent = Request.QueryString["k"];
                ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), TransID.ToString(), "");
                if (hashkey != keySent)
                {
                    Response.Redirect("~/Invalid.aspx");
                    return;
                }

            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                Response.Redirect("~/Invalid.aspx", false);
            }
            finally
            {
                BookLogic.Dispose();
            }
        }
    }
}