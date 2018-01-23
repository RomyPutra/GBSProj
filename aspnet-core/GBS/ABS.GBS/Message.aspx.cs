using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using ABS.Logic.GroupBooking.Agent;
using ABS.GBS.Log;

namespace GroupBooking.Web
{
    public partial class Message : System.Web.UI.Page
    {
        SystemLog SystemLog = new SystemLog();
        //AgentControl.StrucAgentSet AgentSet;
        //ABS.Logic.GroupBooking.Agent.AgentControl objAgent = new ABS.Logic.GroupBooking.Agent.AgentControl();

        protected void loadAgentSet()
        { 
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["msgid"] != null)
                {
                    hidMsgID.Value = Request.QueryString["msgid"];
                    switch (Request.QueryString["msgid"])
                    {
                        case "100":
                            lblTitle.Text = "Registration Completed";
                            lblMessage.Text = "Thank you for registering as an agent of our AirAsia group booking system";
                            loadAgentSet();
                            break;
                        case "104":
                            lblTitle.Text = "Registration Failed";
                            lblMessage.Text = "We regret to inform that your agent registration process has failed, please contact our system administrator or try again later";
                            break;
                        case "105":
                            lblTitle.Text = "Update Profile Success";
                            lblMessage.Text = "Your profile has been updated, thank you";
                            break;
                        case "106":
                            lblTitle.Text = "Change Password Success";
                            lblMessage.Text = "Your password has been changed, please login with the new password thank you";
                            break;
                        case "107":
                            lblTitle.Text = "Send Password Success";
                            lblMessage.Text = "Your password has been sent. Please check your e-mail, thank you";
                            break;
                        case "300":
                            lblTitle.Text = "Password Retrival";
                            lblMessage.Text = "Your requested password have been sent to your mailbox, kindly check and login again";
                            break;
                        case "109":
                            lblTitle.Text = "Logout";
                            lblMessage.Text = "Thank you for visiting our Airasia group booking system";
                            break;
                        case "400":
                            lblTitle.Text = "Logout";
                            lblMessage.Text = "Your session is expired, please proceed to login again, thank you.";
                            break;
                        case "108":
                            lblTitle.Text = "Cancel Booking Success";
                            lblMessage.Text = "Your booking has been cancelled.";
                            break;
                        case "110":
                            lblTitle.Text = "Cancel Booking Success";
                            lblMessage.Text = "Your booking has been cancelled.";
                            break;
                        case "4000":
                            lblTitle.Text = "General Error";
                            lblMessage.Text = "We're sorry that your request cannot be process, kindly retry or contact administrator. Thank you";
                            break;
                        case "4003":
                            lblTitle.Text = "Forbidden Access";
                            lblMessage.Text = "We're sorry to inform that your request in access is denied.";
                            break;
                        case "4004":
                            lblTitle.Text = "Page Not Found";
                            lblMessage.Text = "The requested page was not found or mistake in address.";
                            break;
                        case "5000":
                            lblTitle.Text = "Internal System Error";
                            lblMessage.Text = "We're sorry, the server encounter internal error, where your request cannot be continue.";
                            break;
                        default:

                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);

            }
        }
    }
}