using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using StackExchange.Profiling;
using ABS.GBS.Log;
using System.Data;

namespace ABS.GBS
{
    public class Global : System.Web.HttpApplication
    {
        SystemLog SystemLog = new SystemLog();
        ABS.Logic.GroupBooking.Booking.BookingControl objBooking;

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            //log4net.Config.XmlConfigurator.Configure();
            //Application["appLocation"] = "GBS Staging";

            DataTable dtArrayBaggage;
            DataTable dtArraySport;
            DataTable dtArrayKit;
            DataTable dtArrayDrink;
            DataTable dtArrayMeal;
            DataTable dtArrayDuty;
            DataTable dtArrayInfant;
            DataTable dtCodeMasterFee;
            //added by romy for insurance
            DataTable dtInsure;

            if (Application["dtArrayBaggage"] == null && Application["dtArrayKit"] == null && Application["dtArrayDrink"] == null && Application["dtArrayMeal"] == null && Application["dtArrayDuty"] == null && Application["dtArrayInfant"] == null && Application["dtArrayInsure"] == null && Application["InsureEnable"] == null)
            {
                objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                using (objBooking)
                {
                    dtArrayBaggage = objBooking.GetDetailSSRbyCat("PBA");
                    dtArraySport = objBooking.GetDetailSSRbyCat("PBS");
                    dtArrayKit = objBooking.GetDetailSSRbyCat("PCM");
                    dtArrayDrink = objBooking.GetDetailSSR("WYD");
                    dtArrayMeal = objBooking.GetDetailSSR("WYM");
                    dtArrayDuty = objBooking.GetDetailSSRbyCat("WCH");
                    dtArrayInfant = objBooking.GetDetailSSR("INF");
                    //added by romy for insurance
                    dtInsure = objBooking.GetInsure("IND");

                    Application["dtArrayBaggage"] = dtArrayBaggage;
                    Application["dtArraySport"] = dtArraySport;
                    Application["dtArrayKit"] = dtArrayKit;
                    Application["dtArrayDrink"] = dtArrayDrink;
                    Application["dtArrayMeal"] = dtArrayMeal;
                    Application["dtArrayDuty"] = dtArrayDuty;
                    Application["dtArrayInfant"] = dtArrayInfant;
                    //added by romy for insurance
                    if (dtInsure != null && dtInsure.Rows[0]["CodeDesc"].ToString() == "1")
                    {
                        DataTable dtArrayInsure = objBooking.GetInsureCode("INS");//added by romy, 20170814, insurance
                        Application["dtArrayInsure"] = dtArrayInsure;//added by romy, 20170814, insurance
                        Application["InsureEnable"] = true;
                    }
                    else
                    {
                        Application["InsureEnable"] = false;
                    }
                }
            }
            if (Application["dtCodeMasterFee"] == null)
            {
                objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
                using (objBooking)
                {
                    dtCodeMasterFee = objBooking.GetDetailFeeCodeMaster("FEE");
                    Application["dtCodeMasterFee"] = dtCodeMasterFee;
                }
            }
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown
            Application.Clear();
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            Exception ex = Server.GetLastError();
            SystemLog.Notifier.Notify(ex);
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

        protected void Application_BeginRequest()
        {
            if (Request.IsLocal || HttpContext.Current.IsDebuggingEnabled)
            {
                MiniProfiler.Start();
            }
        }

        protected void Application_EndRequest()
        {
            MiniProfiler.Stop();
        }
    }
}
