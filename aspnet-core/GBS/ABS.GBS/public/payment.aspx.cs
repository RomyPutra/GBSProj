using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Collections;
using System.Web.UI.WebControls;
using ABS.Logic.Shared;
using DevExpress.Web;
using System.Globalization;
//using log4net;
using ABS.Logic.GroupBooking.Booking;
using ABS.Navitaire.AccountManager;
using ABS.Navitaire.AgentManager;
using System.Threading;
using System.Text;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Configuration;
using ABS.GBS.Log;

namespace GroupBooking.Web
{
	public partial class payment : System.Web.UI.Page
	{
        #region declaration
        private XmlNode _providers;
        private List<LinkButton> lnkButtons = new List<LinkButton>();
        UserSet MyUserSet;
        decimal totalPayment = 0, totalDepart = 0, totalReturn = 0, totalAmount = 0;
        string currency = "";
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogControl log = new LogControl();
        SystemLog SystemLog = new SystemLog();

        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
        ABS.Navitaire.APIBooking absNavitaire = new ABS.Navitaire.APIBooking("");

        BookingTransactionDetail bookDTLInfo = new BookingTransactionDetail();
        BookingTransactionMain bookHDRInfo = new BookingTransactionMain();
        BookingTransTender bookTransTenderInfo = new BookingTransTender();
        List<PassengerData> lstPassenger = new List<PassengerData>();

        ABS.GBS.eService.ProcessServiceSoapClient eWS = new ABS.GBS.eService.ProcessServiceSoapClient();
        List<BookingTransactionDetail> lstbookDTLInfo = new List<BookingTransactionDetail>();
        ABS.Logic.GroupBooking.Agent.AgentProfile agent = new ABS.Logic.GroupBooking.Agent.AgentProfile();
        ABS.Logic.GroupBooking.Agent.AgentProfileControl objAgentProfile = new ABS.Logic.GroupBooking.Agent.AgentProfileControl();
        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
        List<BookingTransactionDetail> lstbookFlightInfo = new List<BookingTransactionDetail>();

        private static int num = 0;
        private static int departID = 0;
        private static string ReturnID = "";

        //added by ketee
        private string PayScheme;

        public enum PaymentType
        {
            CreditCard = 0,
            AG = 1,
            CreditShell = 2,
            DirectDebit = 3
        }


        #endregion

		protected void Page_Load(object sender, EventArgs e)
		{
            try
            {
                Response.Buffer = true;
                Response.ExpiresAbsolute = DateTime.Now.AddDays(-1d);
                Response.Expires = -1500;
                Response.CacheControl = "no-cache";

                if (Session["AgentSet"] != null)
                { MyUserSet = (UserSet)Session["AgentSet"]; }
                else
                { Response.Redirect(Shared.MySite.PublicPages.InvalidPage, false); }

                SessionContext sesscon = new SessionContext();

                if (!IsPostBack)
                {

                }

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this,ex);
            }
		}

        
	}
}