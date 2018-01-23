using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SEAL.Model.Moyenne.UI;
using ABS.Logic.GroupBooking;
using ABS.Logic.Shared;

namespace GroupBooking.Web.Master
{
    public partial class NewAdminMaster : System.Web.UI.MasterPage
    {
        //public StateManager StateMan;
        public AdminSet MyUserSet;


        enum ControlState
        {
            Operator = 0,
            Supervisor = 1,
            Manager = 2, 
            Viewer = 3 //20170522 - Sienny (new role)
        }
        ControlState _myState;

        protected void Page_InitComplete(object sender, EventArgs e)
        {
            //StateMan = new StateManager();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session["AdminSet"] != null)
            {
                //KEN
                MyUserSet = (AdminSet)Session["AdminSet"];

                //added by diana 20131114 - to check session is valid or not in order to avoid multiple login in different browser
                if (Session["SessionLogin"] != null)
                {
                    ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                    Boolean valid = objBooking.checkValidAdminSession(MyUserSet.AdminName, (DateTime)Session["SessionLogin"]);
                    if (valid == false)
                    {
                        if (Page.IsCallback)
                            Response.Redirect(Shared.MySite.AdminPages.AdminLogin, false);
                        else
                            Response.Redirect(Shared.MySite.AdminPages.AdminLogin, false);
                        return;
                    }
                }
                else
                {
                    Response.Redirect(Shared.MySite.AdminPages.AdminLogin);
                    return;
                }
                //end added by diana 20131114 - to check session is valid or not in order to avoid multiple login in different browser
                
            }
            else
            {
                // Response.Redirect(Shared.MySite.AdminPages.AdminLogin);
            }
            DevExpress.Web.ASPxWebControl.SetIECompatibilityMode(9);
        }

        protected void InitState()
        {
            switch (MyUserSet.GroupCode)
            {
                case "01":
                    _myState = ControlState.Operator;
                    break;
                case "02":
                    _myState = ControlState.Supervisor;
                    break;
                case "03":
                    _myState = ControlState.Manager;
                    break;
                //20170522 - Sienny (new role)
                case "04":
                    _myState = ControlState.Viewer;
                    break;
            }
        }
        protected void LoadMenu()
        {
            if (_myState == ControlState.Operator)
            {
                //liAddAgent.Visible = false;
                liAdmin.Visible = false;
                liManageAgent.Visible = false;
                liSetting.Visible = false;
                liReport.Visible = false;
            }
            if (_myState == ControlState.Supervisor)
            {
                liAdmin.Visible = false;
                liSetting.Visible = false;
                liReport.Visible = false;
            }

            //20170522 - Sienny (new role shows only admin control - report list)
            if (_myState == ControlState.Viewer)
            {
                liAttention.Visible = false;
                liManageBooking.Visible = false;
                liAgent.Visible = false;
                liAdmin.Visible = false;
                liPaymentExtension.Visible = false;
                liSetting.Visible = false;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            TimeExpiry.TimeOutUrl = Shared.MySite.AdminPages.AdminLogin;
            if (Session["AdminSet"] != null)
            {
                //KEN
                MyUserSet = (AdminSet)Session["AdminSet"];
                lblUsername2.Text = MyUserSet.AdminName;
                InitState();
                LoadMenu();
                // lnkChangePass.Visible = false;
            }
            else
            {
                Response.Redirect(Shared.MySite.AdminPages.AdminLogin);
            }
            string temp = Page.Page.ToString();
            switch (temp)
            {
                //case "ASP.admin_adminmain_aspx":
                //    liAttention.Attributes["class"] = "dropDown2";
                //    break;
                //case "ASP.admin_agentlist_aspx":
                //    liManageAgent.Attributes["class"] = "dropDown2";
                //    break;
                //case "ASP.admin_agentdetail_aspx":
                //    liAddAgent.Attributes["class"] = "dropDown2";
                //    break;
                //case "ASP.admin_cancelledbooking_aspx":
                //    liCancelled.Attributes["class"] = "dropDown2";
                //    break;
                //case "ASP.admin_bookinglist_aspx":
                //    liBooking.Attributes["class"] = "dropDown2";
                //    break;
                //case "ASP.admin_agentblacklist_aspx":
                //    liBlacklisted.Attributes["class"] = "dropDown2";
                //    break;
                //case "ASP.admin_reportlist_aspx":
                //    liReport.Attributes["class"] = "dropDown2";
                //    break;
                //case "ASP.admin_reportview_aspx":
                //    liReport.Attributes["class"] = "dropDown2";
                //    break;
                //case "ASP.admin_userlist_aspx":
                //    liAdmin.Attributes["class"] = "dropDown2";
                //    break;
                //case "ASP.admin_adminpaymentextension_aspx":
                //    liPaymentExtension.Attributes["class"] = "dropDown2";
                //    break;
                //case "ASP.admin_adminchangeflight_aspx":
                //    liChangeFlight.Attributes["class"] = "dropDown2";
                //    break;
                //case "ASP.admin_adminwpenalty_aspx":
                //    liPenalty.Attributes["class"] = "dropDown2";
                //    break;
                //case "ASP.admin_adminsetting_aspx":
                //    liSetting.Attributes["class"] = "dropDown2";
                //    break;
            }
        }
    }
}