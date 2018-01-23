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
    public partial class PageMaster : System.Web.UI.MasterPage
    {
        protected void InitializeComponent()
        {
            this.Page.PreLoad += new System.EventHandler(this.Page_PreLoad);
        }
        //public StateManager StateMan;
        public UserSet MyUserSet;

        protected void Page_InitComplete(object sender, EventArgs e)
        {
            //StateMan = new StateManager();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session["AgentSet"] != null)
            {
                //KEN
                MyUserSet = (UserSet)Session["AgentSet"];

                //added by diana 20131114 - to check session is valid or not in order to avoid multiple login in different browser
                if (Session["SessionLogin"] != null)
                {
                    ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                    Boolean valid = objBooking.checkValidSession(MyUserSet.AgentName, (DateTime)Session["SessionLogin"]);
                    if (valid == false)
                    {
                        Response.Redirect("../SessionExpired.aspx", false);
                        return;
                    }
                }
                else
                {
                    Response.Redirect("../SessionExpired.aspx", false);
                    return;
                }
                //end added by diana 20131114 - to check session is valid or not in order to avoid multiple login in different browser
                
            }
            else
            {
                Response.Redirect(Shared.MySite.PublicPages.SessionExpired);
            }
            DevExpress.Web.ASPxWebControl.SetIECompatibilityMode(9);
        }

        protected void Page_PreLoad(object sender, System.EventArgs e)
        {
            if (Session["AgentSet"] != null)
            {
                //KEN
                MyUserSet = (UserSet)Session["AgentSet"];
            }
            else
            {
                Response.Redirect(Shared.MySite.PublicPages.SessionExpired);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            TimeExpiry.TimeOutUrl =  "../SessionExpired.aspx";
            if (Session["AgentSet"] != null)
            {
                //KEN
                MyUserSet = (UserSet)Session["AgentSet"];
                lblUsername2.Text = MyUserSet.AgentName;

                if (MyUserSet.AgentType != EnumAgentType.PublicAgent)
                {
                    //hide manageprofile
                    //treeMenu.Nodes.FindByText("Manage Profile").Visible = false;
                    //ulMenu.Style["display"] = "none";
                    //lnkChangePass.Visible = false;
                    aChangePwd.Visible = false;
                    liProfile.Visible = false;
                    aLogout.InnerText = "Return and Close";
                    aLogout.HRef = "javascript:var ans= window.confirm('Are you sure?'); if (ans) {var win = window.open('about:blank','_self');win.close();}";
                }
            }
            else
            {
                Response.Redirect(Shared.MySite.PublicPages.SessionExpired);
            }

            string temp = Page.Page.ToString();
            switch (temp)
            {
                case "ASP.public_agentmain_aspx":
                    liAttention.Attributes["class"] = "dropDown2";
                    break;
                case "ASP.public_bookingmanagement_aspx":
                    liBooking.Attributes["class"] = "dropDown2";
                    break;
                case "ASP.public_bookingdetail_aspx":
                    liBooking.Attributes["class"] = "dropDown2";
                    break;
                case "ASP.public_passengerdetail_aspx":
                    liBooking.Attributes["class"] = "dropDown2";
                    break;
                case "ASP.public_passengerlist_aspx":
                   // liName.Attributes["class"] = "dropDown2";
                    break;
                case "ASP.public_profile_aspx":
                    liProfile.Attributes["class"] = "dropDown2";
                    break;
                case "ASP.public_searchflight_aspx":
                    liSearch.Attributes["class"] = "dropDown2";
                    break;
                case "ASP.public_selectflight_aspx":
                    liSearch.Attributes["class"] = "dropDown2";
                    break;
                case "ASP.public_reviewfare_aspx":
                    liSearch.Attributes["class"] = "dropDown2";
                    break;
                case "ASP.public_reviewbooking_aspx":
                    liSearch.Attributes["class"] = "dropDown2";
                    break;              
            }        

        }


    }
}