using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using ABS.Logic.Shared;

namespace GroupBooking.Web.Master
{
    public partial class BlankMaster : System.Web.UI.MasterPage
    {
        public UserSet MyUserSet;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["optmode"] == "3" || Request.QueryString["optmode"] == "2")
            {
                if (Session["AgentSet"] == null)
                {
                    Response.Redirect(Shared.MySite.PublicPages.AgentLogin);
                }
                else
                {
                    MyUserSet = (UserSet)Session["AgentSet"];
                
                    //added by diana 20131114 - to check session is valid or not in order to avoid multiple login in different browser
                    if (Session["SessionLogin"] != null && MyUserSet != null)
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
            }
        }

        protected void ValidateMsg(object sender, CallbackEventArgs e)
        {
            //Response.Write(e.Parameter);
            //e.Result = "test123";
        }
    }
}