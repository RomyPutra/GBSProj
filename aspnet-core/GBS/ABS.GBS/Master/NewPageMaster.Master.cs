using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SEAL.Model.Moyenne.UI;
using ABS.Logic.GroupBooking;
using ABS.Logic.Shared;
using DevExpress.Web;

namespace GroupBooking.Web.Master
{
    public partial class NewPageMaster : System.Web.UI.MasterPage
    {
        protected void InitializeComponent()
        {
            this.Page.PreLoad += new System.EventHandler(this.Page_PreLoad);
        }
        //public StateManager StateMan;
        public UserSet MyUserSet;

        protected void Page_InitComplete(object sender, EventArgs e)
        {
            return;
            //StateMan = new StateManager();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            //return;
            if (Session["AgentSet"] != null)
            {
                //KEN
                MyUserSet = (UserSet)Session["AgentSet"];

                //added by diana 20131114 - to check session is valid or not in order to avoid multiple login in different browser
                if (Session["SessionLogin"] != null && MyUserSet != null)
                {
                    ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                    //Boolean valid = true; //objBooking.checkValidSession(MyUserSet.AgentName, (DateTime)Session["SessionLogin"]);

                    Boolean valid = objBooking.checkValidSession(MyUserSet.AgentName, (DateTime)Session["SessionLogin"]);
                    if (valid == false)
                    {
                        if (Page.IsCallback)
                            ASPxCallback.RedirectOnCallback("../SessionInUsed.aspx");
                        else
                            Response.Redirect("../SessionInUsed.aspx", false);
                        return;
                    }
                }
                else
                {
                    Response.Redirect("../SessionExpired.aspx", false);
                    return;
                }
                //end added by diana 20131114 - to check session is valid or not in order to avoid multiple login in different browser

                //if OLDGBS config value is not empty, show the link
                if (System.Configuration.ConfigurationManager.AppSettings["OLDGBS"] != null && System.Configuration.ConfigurationManager.AppSettings["OLDGBS"].ToString() != "")
                {
                    ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
                    string hashkey = objGeneral.GenerateMac(System.Configuration.ConfigurationManager.AppSettings["hashing"].ToString(), MyUserSet.AgentID.ToString(), MyUserSet.AgentName.ToString());

                    hPreviousGBS.Visible = true;
                    hPreviousGBS.HRef = System.Configuration.ConfigurationManager.AppSettings["OLDGBS"].ToString().Trim() + "?hashkey=" + hashkey.Trim() + "&aid=" + MyUserSet.AgentID.ToString().Trim() + "&aName=" + MyUserSet.AgentName.ToString().Trim();
                }
            }
            else
            {
                try
                {
                    Response.Redirect(Shared.MySite.PublicPages.SessionExpired, false);
                }
                catch(Exception ex)
                {
                    ASPxCallback.RedirectOnCallback(Shared.MySite.PublicPages.SessionExpired);
                }
                
                //return;
            }
            DevExpress.Web.ASPxWebControl.SetIECompatibilityMode(9);
        }

        protected void Page_PreLoad(object sender, System.EventArgs e)
        {
            //return;
            if (Session["AgentSet"] != null)
            {
                //KEN
                MyUserSet = (UserSet)Session["AgentSet"];
            }
            else
            {
                Response.Redirect(Shared.MySite.PublicPages.SessionExpired, false);
                //return;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //return;
            TimeExpiry.TimeOutUrl = "../SessionExpired.aspx";
            HighlightMenu();

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
                if (Page.IsCallback)
                    ASPxCallback.RedirectOnCallback(Shared.MySite.PublicPages.SessionExpired);
                else
                    Response.Redirect(Shared.MySite.PublicPages.SessionExpired, false);
                //return;
            }

            //string temp = Page.Page.ToString();
            //switch (temp)
            //{
            //    case "ASP.public_agentmain_aspx":
            //        liAttention.Attributes["class"] = "dropDown2";
            //        break;
            //    case "ASP.public_bookingmanagement_aspx":
            //        liBooking.Attributes["class"] = "dropDown2";
            //        break;
            //    case "ASP.public_bookingdetail_aspx":
            //        liBooking.Attributes["class"] = "dropDown2";
            //        break;
            //    case "ASP.public_passengerdetail_aspx":
            //        liBooking.Attributes["class"] = "dropDown2";
            //        break;
            //    case "ASP.public_passengerlist_aspx":
            //        // liName.Attributes["class"] = "dropDown2";
            //        break;
            //    case "ASP.public_profile_aspx":
            //        liProfile.Attributes["class"] = "dropDown2";
            //        break;
            //    case "ASP.public_searchflight_aspx":
            //        liSearch.Attributes["class"] = "dropDown2";
            //        break;
            //    case "ASP.public_selectflight_aspx":
            //        liSearch.Attributes["class"] = "dropDown2";
            //        break;
            //    case "ASP.public_reviewfare_aspx":
            //        liSearch.Attributes["class"] = "dropDown2";
            //        break;
            //    case "ASP.public_reviewbooking_aspx":
            //        liSearch.Attributes["class"] = "dropDown2";
            //        break;
            //}   
        }

        protected void searchLink_Click(object sender, EventArgs e)
        {

            Session["SearchFilter"] = "PNR|" + inputSearchFor.Value;
            Response.Redirect("../public/bookingmanagement.aspx", false);

        }

        protected void HighlightMenu()
        {
            //string url = Request.RawUrl.ToUpper();
            //if (url.Contains("SEARCHFLIGHT"))
            //{
            //    homelink.Style["color"] = "#ffffff";
            //    homelink.Style["background-color"] = "rgba(255,255,255,.1)";
            //}
            //else if (url.Contains("SELECTFLIGHT"))
            //{
            //    searchlink.Style["color"] = "#9a9a9a";
            //    searchlink.Style["background-color"] = "#fafafa";
            //}
            //else if (url.Contains("BOOKINGMANAGEMENT"))
            //{
            //    managelink.Style["color"] = "#9a9a9a";
            //    managelink.Style["background-color"] = "#fafafa";
            //}
        }
    }
}