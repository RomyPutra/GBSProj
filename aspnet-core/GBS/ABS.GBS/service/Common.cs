using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ABS.Logic.GroupBooking.Agent;
using ABS.Logic.Shared;

namespace GroupBooking.Web.Shared
{
    namespace MySite
    {
        //Central area for the Site web addresses & link, refer below for the include of QueryString
        //e.g. string.Concat(MySite.PublicPages.Profile, "?optmode=1");
        public class PublicPages
        {
            public static string AgentLogin = "~/public/agentlogin.aspx";
            public static string AgentMain = "~/public/agentmain.aspx";
            public static string PasswordRetrieve = "~/public/passwordretrieve.aspx";
            public static string Profile = "~/public/profile.aspx";
            public static string Searchflight = "~/public/searchflight.aspx";
            public static string Selectflight = "~/public/selectflight.aspx";
            public static string ReviewFare = "~/public/reviewfare.aspx";
            public static string ReviewBooking = "~/public/reviewbooking.aspx";
            public static string Payment = "~/public/proceedpayment.aspx";
            public static string Payment2 = "~/public/proceedpayment.aspx";
            public static string Payment3 = "proceedpayment.aspx";
            public static string InvalidPage = "~/Invalid.aspx";
            public static string AgentFlightChange = "~/public/agentflightchange.aspx";
            public static string AgentSearchFlightChange = "~/public/agentsearchflightchange.aspx";
            public static string AgentSelectFlightChange = "~/public/agentselectflightchange.aspx";
            public static string BookingDetail = "~/public/BookingDetail.aspx";
            public static string SearchFlightChange = "~/public/searchflightchange.aspx";
            public static string SelectFlightChange = "~/public/selectflightchange.aspx";
            public static string ReviewFareChange = "~/public/reviewfarechange.aspx";
            public static string ReviewBookingChange = "~/public/reviewbookingchange.aspx";
            public static string PaymentChange = "~/public/proceedpaymentchange.aspx";
            public static string SessionExpired = "~/SessionExpired.aspx";
            public static string DivideSummary = "~/public/divideSummary.aspx";
            public static string BookingComplete = "~/public/BookingComplete.aspx";
        }
        public class AdminPages
        {
            public static string AdminLogin = "~/admin/adminlogin.aspx";
            public static string AdminMain = "~/admin/adminMain.aspx";
            public static string AgentList = "~/admin/agentlist.aspx";
            public static string ManageBooking = "~/admin/bookinglist.aspx";
            public static string CancelledBooking = "~/admin/cancelledbooking.aspx";
            public static string AdminBookingDetail = "~/admin/adminbookingdetail.aspx";
            public static string AdminSearchFlight = "~/admin/adminsearchflight.aspx";
            public static string AdminSelectFlight = "~/admin/adminselectflight.aspx";
        }

    }
    public class Common
    {
        public static int AuthPublicAgent(string Username, string Password, string Type, string OrganizationName)
        {
            AgentCategory AgentCat;
            AgentControl.StrucAgentSet AgentSet;
            ABS.Logic.GroupBooking.Agent.AgentControl objAgent = new ABS.Logic.GroupBooking.Agent.AgentControl();
            ABS.Logic.GroupBooking.Agent.AgentProfileControl objAgentProfile = new ABS.Logic.GroupBooking.Agent.AgentProfileControl();

            AgentActivity agActivityInfo = new AgentActivity();

            AgentBlacklistLog agBlacklistInfo = new AgentBlacklistLog();
            //GroupBooking.Logic.AG_BLACKLIST_Logic agBlacklistLogic = new GroupBooking.Logic.AG_BLACKLIST_Logic();

            EnumAgentType AgentType;
            if (Type.ToString().ToLower() == "skyagent")
                AgentType = EnumAgentType.SkyAgent;
            else
                AgentType = EnumAgentType.PublicAgent;

            if (objAgent.ValidateAgent(Username, Password, AgentType) == true)
            {
                //check blacklist
                if (objAgent.VerifyBlackList(objAgent.AgentSet.AgentID) == true)
                {
                    //blacklisted
                    return 101;
                }
                else
                {
                    AgentSet = objAgent.AgentSet;
                    if (OrganizationName != "")
                    {
                        AgentSet.OrganizationName = OrganizationName;
                    }
                    if (HttpContext.Current.Session["AgentSet"] != null)
                    {
                        ABS.Logic.Shared.UserSet MyUserSet = (UserSet)HttpContext.Current.Session["AgentSet"];
                        AgentSet.Currency = MyUserSet.Currency;
                        AgentSet.Email = MyUserSet.Email;
                        AgentSet.Contact = MyUserSet.Contact;
                    }

                    AgentCat = objAgent.GetAgentCategory(AgentSet.AgentCategoryID);
                    Shared.Common.SetAgentSession(AgentSet,AgentCat);
                    return 100; //success
                }
            }
            else
            {
                return 102; //invalid username or password
            }
        }

        public static int CheckInactive(string Username, string Password)
        {
            ABS.Logic.GroupBooking.Agent.AgentControl objAgent = new ABS.Logic.GroupBooking.Agent.AgentControl();
            ABS.Logic.GroupBooking.Agent.AgentProfileControl objAgentProfile = new ABS.Logic.GroupBooking.Agent.AgentProfileControl();

            AgentActivity agActivityInfo = new AgentActivity();

            AgentBlacklistLog agBlacklistInfo = new AgentBlacklistLog();
            //GroupBooking.Logic.AG_BLACKLIST_Logic agBlacklistLogic = new GroupBooking.Logic.AG_BLACKLIST_Logic();


            if (objAgentProfile.CheckUsernamePasswordExist(Username, Password) == 1)
            {
                //find username
                return 100;
            }
            else
            {
                return 101; //don't find username
            }
        }

        public static void SetAgentSession(AgentControl.StrucAgentSet AgentStruc, AgentCategory Category)
        {
            UserSet myUserSet = new UserSet();
            myUserSet.AgentID = AgentStruc.AgentID;
            myUserSet.AgentName = AgentStruc.AgentName;
            if (Category != null) { myUserSet.AgentCategoryID = Category.AgentCatgID; }
            myUserSet.LoginName = AgentStruc.AgentName;
            myUserSet.OperationGroup = AgentStruc.OperationGroup;
            myUserSet.AgentType = (EnumAgentType)AgentStruc.AgentType;
            myUserSet.BlacklistDuration = Category.BlacklistDuration;
            myUserSet.CounterTimer = Category.CounterTimer;
            myUserSet.MaxEnquiry = Category.MaxEnquiry;
            myUserSet.MaxSuspend = Category.MaxSuspend;
            myUserSet.SuspendDuration = Category.SuspendDuration;
            myUserSet.OrganizationName = AgentStruc.OrganizationName;
            myUserSet.Currency = AgentStruc.Currency;
            myUserSet.Email = AgentStruc.Email;
            myUserSet.Contact = AgentStruc.Contact;

            HttpContext.Current.Session.Add("AgentSet", myUserSet);
        }

        public static void SetAdminSession(AdminControl.StrucAdminSet AgentStruc)
        {
            AdminSet myUserSet = new AdminSet();
            myUserSet.AdminID = AgentStruc.AdminID;
            myUserSet.AdminName = AgentStruc.AdminName;
            myUserSet.AccessLevel = AgentStruc.AccessLevel;
            myUserSet.GroupCode = AgentStruc.GroupCode;
            myUserSet.GroupName = AgentStruc.GroupName;
            myUserSet.AccessLevel = AgentStruc.AccessLevel;
            myUserSet.OperationGroup = AgentStruc.OperationGroup;
            HttpContext.Current.Session.Add("AdminSet", myUserSet);
        }

    }
}