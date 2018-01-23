using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using ABS.Logic.GroupBooking.Agent;
using ABS.Logic.GroupBooking;
using ABS.Logic.Shared;

namespace GroupBooking.Web
{
    public partial class passengerlist : System.Web.UI.Page
    {
        UserSet AgentSet;
        List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain> TransMainData = new List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>();
        List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain> TransMainData2 = new List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>();
        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //SessionContext sesscon = new SessionContext();
            //sesscon.ValidateAgentLogin();
            if (Session["AgentSet"] != null)
            {
                AgentSet = (UserSet)Session["AgentSet"];
                if (!IsPostBack)
                {
                    assignDefaultValue();
                }
                else
                    LoadGridView();
            }
            else
            {
                Response.Redirect("~/public/agentlogin.aspx");
            }
        }

        protected void assignDefaultValue()
        {
            Session["optemode"] = null;
            Session["agid"] = null;
            Session["AdminSet"] = null;
            LoadDefaultGridView();
        }

        protected void LoadDefaultGridView()
        {
            TransMainData = objBooking.GETALLPassengerChangeName(AgentSet.AgentID, "", 3);
            Session["dtGridFinish"] = TransMainData;
            gvFinishBooking.DataSource = TransMainData;
            gvFinishBooking.DataBind();

            TransMainData2 = objBooking.GetAllBK_TRANSMAINStatus(AgentSet.AgentID, "", 3);
            //foreach (ABS.Logic.GroupBooking.Booking.BookingTransactionMain transmain in TransMainData2)
            //{
            //    if (objBooking.AddNameChangeFees(transmain.pnrTemp, transmain.Currency, 0) == true)
            //   {

            //   }
            //}
        }

        protected void LoadGridView()
        {
            //if (Session["dtGrid"] == null)
            //{
            //    Session["dtGrid"] = objAgent.GetAllActiveAgent();
            //}

            if (Session["dtGridFinish"] != null)
            {
                gvFinishBooking.DataSource = (List<ABS.Logic.GroupBooking.Booking.BookingTransactionMain>)Session["dtGridFinish"];
                gvFinishBooking.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //TransMainData = objBooking.GetAllBK_TRANSMAINFilter(Convert.ToDateTime(txtStartDate.Text),Convert.ToDateTime(txtEndDate.Text));
            //Session["dtGrid"] = TransMainData;
        }

        protected void gvFinishBooking_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            object rowKey = null;

            if (e.ButtonID == "viewBtnFinish")
            {
                rowKey = gvFinishBooking.GetRowValues(e.VisibleIndex, "TransID");
                Session["TransID"] = rowKey;

                ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), rowKey.ToString(), "");
              
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("../public/passengerdetail.aspx?k=" + hashkey + "&TransID=" + rowKey);
            }
        }
    }
}