using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GroupBooking.Web.admin
{
    public partial class CancelledBooking : System.Web.UI.Page
    {
        List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain> TransMainData = new List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>();
        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminSet"] != null)
            {
                if (!IsPostBack)
                {
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
            if (cmbField.SelectedItem.Value.ToString() == "AgentID")
                TransMainData = objBooking.GetAllBK_TRANSMAINCancelFilter(Convert.ToDateTime(txtStartDate.Value), Convert.ToDateTime(txtEndDate.Value), txtTransID.Text, txtAgentID.Text, "", 4, txtRecordLocator.Text);
            if (cmbField.SelectedItem.Value.ToString() == "AgentName")
                TransMainData = objBooking.GetAllBK_TRANSMAINCancelFilter(Convert.ToDateTime(txtStartDate.Value), Convert.ToDateTime(txtEndDate.Value), txtTransID.Text, "", txtAgentID.Text, 4, txtRecordLocator.Text);
            gridAgent.DataSource = TransMainData;
            gridAgent.DataBind();
        }
        protected void LoadGridView()
        {
            if (cmbField.SelectedItem.Value.ToString() == "AgentID")
                TransMainData = objBooking.GetAllBK_TRANSMAINCancelFilter(Convert.ToDateTime(txtStartDate.Value), Convert.ToDateTime(txtEndDate.Value), txtTransID.Text, txtAgentID.Text, "", 4, txtRecordLocator.Text);
            if (cmbField.SelectedItem.Value.ToString() == "AgentName")
                TransMainData = objBooking.GetAllBK_TRANSMAINCancelFilter(Convert.ToDateTime(txtStartDate.Value), Convert.ToDateTime(txtEndDate.Value), txtTransID.Text, "", txtAgentID.Text, 4, txtRecordLocator.Text);
            Session["dtGrid"] = TransMainData;
            gridAgent.DataSource = (List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>)Session["dtGrid"];
            gridAgent.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (cmbField.SelectedItem.Value.ToString() == "AgentID")
                TransMainData = objBooking.GetAllBK_TRANSMAINCancelFilter(Convert.ToDateTime(txtStartDate.Value), Convert.ToDateTime(txtEndDate.Value), txtTransID.Text, txtAgentID.Text, "", 4, txtRecordLocator.Text);
            if (cmbField.SelectedItem.Value.ToString() == "AgentName")
                TransMainData = objBooking.GetAllBK_TRANSMAINCancelFilter(Convert.ToDateTime(txtStartDate.Value), Convert.ToDateTime(txtEndDate.Value), txtTransID.Text, "", txtAgentID.Text, 4, txtRecordLocator.Text);
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
        }
    }
}