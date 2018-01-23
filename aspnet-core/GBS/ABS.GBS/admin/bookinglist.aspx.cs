using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ABS.Logic.GroupBooking.Agent;
using ABS.Logic.GroupBooking;
using ABS.Logic.Shared;
using System.Data;
using ABS.Logic.GroupBooking.Booking;

namespace GroupBooking.Web.admin
{
    public partial class bookinglist : System.Web.UI.Page
    {
        List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain> TransMainData = new List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>();
        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminSet"] != null)
            {
                if (!IsPostBack)
                {
                    //added by ketee, validate param
                    String TransID = Request.QueryString["TransID"];
                    string keySent = Request.QueryString["k"];
                    string action = Request.QueryString["action"];
                    ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();

                    if (TransID != null && keySent != null)
                    {
                        string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), TransID.ToString(), "");
                        if (hashkey != keySent)
                        {
                            Response.Redirect("~/Invalid.aspx");
                            return;
                        }
                        else
                        {
                            if (action.ToLower() == "getlatest")
                            {
                                List<ListTransaction> AllTransaction = new List<ListTransaction>();
                                AllTransaction = objBooking.GetTransactionDetails(TransID);
                                if (AllTransaction != null && AllTransaction.Count > 0)
                                {
                                    ListTransaction lstTrans = AllTransaction[0];

                                    List<BookingTransactionDetail> VoidPNRs = new List<BookingTransactionDetail>();
                                    List<BookingTransactionDetail> ExpiredPNRs = new List<BookingTransactionDetail>();
                                    objBooking.UpdateAllBookingJourneyDetails(lstTrans, lstTrans.AgentUserName, lstTrans.AgentID, ref VoidPNRs, ref ExpiredPNRs, true);
                                }
                            }
                        }
                    } 
                    assignDefaultValue();                   
                }
                else
                LoadGridView();

            }
            else
            {
                Response.Redirect("~/admin/adminlogin.aspx");
            }
            
        }

        protected void assignDefaultValue()
        {
            txtStartDate.Value = DateTime.Now.AddDays(-3);
            txtEndDate.Value = DateTime.Now;
            Session["dtGrid"] = null;
            Session["optemode"] = null;
            Session["agid"] = null;
            LoadDefaultGridView();
        }

        protected void LoadDefaultGridView()
        {
            //if (Session["dtGrid"] == null)
            //{
            //    Session["dtGrid"] = objAgent.GetAllActiveAgent();
            //}
            TransMainData = objBooking.GetAllBK_TRANSMAINFilter(Convert.ToDateTime(txtStartDate.Value),Convert.ToDateTime(txtEndDate.Value),txtTransID.Text,txtAgentID.Text,Convert.ToInt16(cmbStatus.SelectedItem.Value.ToString()), txtRecordLocator.Text);
            gridAgent.DataSource = TransMainData;
            gridAgent.DataBind();
        }

        protected void LoadGridView()
        {
            //if (Session["dtGrid"] == null)
            //{
            //    Session["dtGrid"] = objAgent.GetAllActiveAgent();
            //}
            TransMainData = objBooking.GetAllBK_TRANSMAINFilter(Convert.ToDateTime(txtStartDate.Value), Convert.ToDateTime(txtEndDate.Value), txtTransID.Text, txtAgentID.Text, Convert.ToInt16(cmbStatus.SelectedItem.Value.ToString()), txtRecordLocator.Text);
            Session["dtGrid"] = TransMainData;
            gridAgent.DataSource = (List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>) Session["dtGrid"];
            gridAgent.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            TransMainData = objBooking.GetAllBK_TRANSMAINFilter(Convert.ToDateTime(txtStartDate.Value), Convert.ToDateTime(txtEndDate.Value), txtTransID.Text, txtAgentID.Text, Convert.ToInt16(cmbStatus.SelectedItem.Value.ToString()));
            Session["dtGrid"] = TransMainData;
        }

        protected void gridAgent_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            object rowKey = null;

            if (e.ButtonID == "editBtn")
            {
                rowKey = gridAgent.GetRowValues(e.VisibleIndex, "TransID");
                Session["TransID"] = rowKey;

                ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), rowKey.ToString(), "");
              
                DevExpress.Web.ASPxWebControl.RedirectOnCallback(Shared.MySite.AdminPages.AdminBookingDetail+"?k=" + hashkey + "&TransID=" + rowKey);
            }

            //added by ketee
            if (e.ButtonID == "getLatestBtn")
            {
                object TransID = gridAgent.GetRowValues(e.VisibleIndex, "TransID");

                ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), TransID.ToString(), "");

                DevExpress.Web.ASPxWebControl.RedirectOnCallback(Shared.MySite.AdminPages.ManageBooking + "?action=getlatest&k=" + hashkey + "&TransID=" + TransID);
            }
        }

    }


}