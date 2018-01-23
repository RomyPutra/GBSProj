using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;

namespace GroupBooking.Web
{
    public class MessageList
    {
        public string Err100001 = "Invalid Login.";
        public string Err100002 = "The depart date is unavailable.";
        public string Err100003 = "Please make sure that your return date is not earlier than your outbound date.";
        public string Err100004 = "The return date is unavailable.";
        public string Err100005 = "There must be at least ten passenger for the reservation.";
        public string Err100006 = "Pax number(Adult + Child pax) must be between 10 and 50. Please verify the number of passengers";
        public string Err100007 = "Guest number must less than 50. Please verify the number of passengers";
        public string Err100008 = "Child number  must less than 50. Please verify the number of passengers";
        public string Err100009 = "It's forbidden to  enquire for same sector times !";
        public string Err100010 = "Sorry to inform, you are suspend for this booking.";
        public string Err100011 = "Invalid user name and password, please retry.";
        public string Err100012 = "Sorry to inform, you are blacklisted.";
        public string Err100013 = "No seat available on selected date or selected flight.";
        public string Err100014 = "Please select return flight.";
        public string Err100015 = "Please select depart flight.";
        public string Err100016 = "Username is inactive, please retry.";
        public string Err100017 = "Please select origin and destination.";
        public string Err100018 = "No available currency on selected flight.";
        public string Err100019 = "Please fill correct data.";
        public string Err100020 = "Payment process success.";
        public string Err100021 = "Payment process failed.";
        public string Err100022 = "Pax number must be in correct numeric format.";
        public string Err100023 = "Insufficient credit.";
        public string Err100024 = "Cannot process payment. Session expired.";
        public string Err100025 = "Your session has expired.";
        public string Err100026 = "Payment amount must be numeric and more than 0.";
        public string Err100027 = "Reservation at least have 1 adult.";
        //added by ketee
        public string Err100028 = "Transaction failed, please try again.";
        public string Err100029 = "The passenger data is saved successfully.";
        public string Err100030 = "Please fill all the required data.";
        public string Err100031 = "Booking process failed.";

        public string Err100032 = "Invalid expiry date.";
        public string Err100033 = "Payment declined.";
        //public string Err100034 = "An unknown error has occured with the form of payment you have chosen. You have not been charged, but due to system failure we cannot accept this form of payment at this time. Please try another form of payment or try again at a later time.";
        public string Err100034 = "Payment attempt failed.";

        public string Err100035 = "Member not found.";
        public string Err100036 = "Please fill in all mandatory fields.";
        public string Err100037 = "Cannot change passengers data below 6 hours before STD.";
        public string Err100038 = "This route is suspended.";
        public string Err100039 = "Only full payment is allowed.";
        public string Err100040 = "Please fill in a minimum payment amount.";
        public string Err100041 = "There is no flight on the return date.";
        public string Err100042 = "The CVV/CID length is incorrect, it must be between 4 and 4 inclusive.";
        public string Err100043 = "The CVV/CID length is incorrect, it must be between 3 and 3 inclusive.";
        public string Err100044 = "Over paid is not allow for this booking. Kindly revise the amount to continue further in processing.";
        public string Err100045 = "Booking is expired, please kindly rebook the flight.";
        public string Err100046 = "Credit card is declined. Hence, remaining unconfirmed booking has been expired, please kindly rebook the flight.";
        public string Err100047 = "Saving details is not succeed, kindly reload this page.";

        //added by ketee, block period from : 5 Jan - 31 July 2015(425 days) 
        public string Err100048 = "Travel period: 5 January - 31 July 2015 is not available for Group Booking, sorry for the inconveniences caused.";
        //added by ketee, block period from : 5 Jan - 31 July 2015(425 days) 
        public string Err100049 = "Travel period: 1 March - 31 October 2015 is not available for Group Booking, sorry for the inconveniences caused.";
        //added by ketee, block period from : 10 Jun 2015 - 17 Jan 2016(425 days) 
        public string Err100050 = "Travel period: 10 Jun 2015 - 17 Jan 2016 is not available for Group Booking, sorry for the inconveniences caused.";

        //added by ketee, validate Infant
        public string Err100051 = "Maximum Infant is infantmax. Infant number must less or equal than it.";//Custom Message

        public string Err100052 = "Fail to make payment, payment amount exceeded the amount due, please refresh the booking again.";

        public string Err100053 = "Invalid Login";

        public string Err100054 = "Return Flight should be later than Depart Flight.";
        public string Err100055 = "Fail to assign Add-On, kindly try again later.";

        public string Err100056 = "Invalid login due to incomplete profile. Please update your email address in Sky Agent Profile, thank you.";
        public string Err100057 = "Invalid login due to incomplete profile. Please update your organization details in Sky Agent Profile, thank you.";

        public string Err100058 = "Promo Code is invalid";

        public string Err100059 = "Requested Fare Class Service is sold out. Please try in another 30 minutes.";

        public string Err100060 = "Navitaire Server is down. Please try again later.";
        public string Err100061 = "The return flight should not be earlier than the departure flight. Please reselect a later return flight.";
        public string Err100062 = "Fail to move Flight.";
        public string Err100063 = "Booking details not found";
        public string Err100064 = "Sell Journey failed";
        public string Err100065 = "Failed to change flight, insufficient seat";
        public string Err100066 = "Cannot select same flight";
        public string Err100067 = "Pax number(Adult + Child pax) must be between 4 and 50. Please verify the number of passengers";
        public string Err100068 = "booking session expired, please book again";
        public string Err100069 = "No Min. Payment is Required";
        public string Err100070 = "Min. Payment: ";
        public string Err100071 = "Paid";
        public string Err100072 = "Pending";
        public string Err100073 = "Overpaid";
        public string Err100073A = "Your payment is overpaid. Kindly synchronize the booking from the home page.";
        public string Err100074 = "No fare available for agent rules.";

        //AddOn Message
        public string Err200101 = "Select Baggage";
        public string Err200102 = "Select Meal";
        public string Err200103 = "No Meal";
        public string Err200104 = "Default Drink";
        public string Err200105 = "Select Sport Equipment";
        public string Err200106 = "Select Duty Free";
        public string Err200107 = "Select Comfort Kit";
        public string Err200108 = "Please Insert Detail Infant before confirm.";
        public string Err200041 = "Please select at least 1 Baggage from the list";
        public string Err200042 = "Cannot replace baggage with cheaper fee, please try again";
        public string Err200043 = "The quantity of Baggage must be less or equal to total number of passenger.";
        public string Err200044 = "Please select at least 1 Meal from the list";
        public string Err200045 = "Cannot replace Meal with cheaper fee, please try again";
        public string Err200046 = "The quantity of Meal must be less or equal to total number of passenger.";
        public string Err200047 = "Please select at least 1 Drink from the list";
        public string Err200048 = "Please select at least 1 Meal2 from the list";
        public string Err200049 = "Cannot replace Meal 2 with cheaper fee, please try again";
        public string Err200050 = "The quantity of Meal 2 must be less or equal to total number of passenger.";
        public string Err200051 = "Please select at least 1 Drink2 from the list";
        public string Err200052 = "Please select at least 1 Sport Equipment from the list";
        public string Err200053 = "Cannot replace Sport Equipment with cheaper fee, please try again";
        public string Err200054 = "The quantity of Sport must be less or equal to total number of passenger.";
        public string Err200055 = "The quantity of Duty Free must be less or equal to total number of passenger.";
        public string Err200056 = "Please select at least 1 Comfort Kit from the list";
        public string Err200057 = "Cannot replace Comfort Kit with cheaper fee, please try again";
        public string Err200058 = "The quantity of Comfort Kit must be less or equal to total number of passenger.";
        public string Err200059 = "No Comfort Kit";
        public string Err200160 = "No Baggage";

        //Passengerdetail
        public string Err200060 = "Passenger details has been successfully confirmed.";
        public string Err200061 = "Invalid total numbers of passenger uploaded. Please check the number of your passenger or any missing header.";
        public string Err200062 = "Invalid Data,\nPlease make sure data uploaded with correct format.";
        public string Err200063 = "Title must be 'Mr/Ms' for Adult and 'Chd' for Child\n";
        public string Err200064 = "Gender must be Male/Female\n";
        public string Err200065 = "Nationality is required\n";
        public string Err200066 = "IssuingCountry is required\n";
        public string Err200067 = "Invalid DOB, date format [yyyy-MM-dd], eg: 1999-12-31\n";
        public string Err200068 = "Invalid ExpiryDate, date format [yyyy-MM-dd], eg: 1999-12-31\n";
        public string Err200069 = "Invalid PassportNo, PassportNo is required\n";
        public string Err200070 = "Title must be 'CHD' for Child\n";
        public string Err200071 = "Title must be 'Mr/Ms' for Adult\n";
        public string Err200072 = "Invalid DOB, Minimum age for Adult is 13 years.\n";
        public string Err200073 = "Invalid DOB, Maximum age for Child is 12 years.\n";
        public string Err200074 = "Invalid ExpiryDate, Passport Expiry must more than departure date.\n";
        public string Err200075 = "Invalid upload data, please verify total numbers of passengers.";
        public string Err200076 = "No data.";
        public string Err200077 = "Failed to save passenger data, Title for Adult should be 'Mr/Ms' and Title for Child should be 'Child'.<br/> Passenger : ";
        public string Err200078 = "Failed to save passenger data, minimum age is 1 years.<br/> Passenger : ";
        public string Err200079 = "Failed to save passenger data, minimum age for Adult is 13 years.<br/> Passenger : ";
        public string Err200080 = "Failed to save passenger data, maximum age for Child is 12 years.<br/> Passenger : ";
        public string Err200081 = "Failed to save passenger data, Passport Expiry must more than departure date.<br/> Passenger : ";
        public string Err200082 = "Failed to save passenger data, Infant must be less than two year old.<br/> Infant : ";
        public string Err200083 = "Failed to save passenger data, First Name may not begin with number";
        public string Err200084 = "Failed to save passenger data, Last Name may not begin with number";
        public string Err200085 = "Failed to save passenger data, Infant First Name may not begin with number.<br/> Infant :";
        public string Err200086 = "Failed to save passenger data, Infant Last Name may not begin with number.<br/> Infant :";
        public string Err200087 = "Failed to save passenger data, Name must be complete ";
        public string Err200088 = "Failed to save Infant data, DOB must be older than Departure Date.<br/> Infant : ";
        public string Err200089 = "Failed to save Infant data, Nationality is Required.<br/> Infant : ";
        public string Err200090 = "Failed to save Infant data, Kindly insert the required data<br/> Infant : ";
        public string Err200091 = "Failed to save Infant data, Issuing Country is Reqiuired.<br/> Infant : ";
        public string Err200092 = "Failed to save Infant data, Passport No. is required.<br/> Infant : ";
        public string Err200093 = "Failed to save Infant data, Infant Passport Expiry must more than departure date.<br/> Infant : ";
        public string Err200094 = "Failed to save Infant data, Infant Name must be complete.<br/> Infant : ";
        public string Err200095 = "No passenger to be uploaded.";


        //added for Insure message
        public string Err200000 = "Not Eligible";//"Not Applicable";
        public string Err200001 = "Connecting";
        public string Err200002 = "Direct";
        public string Err200003 = "Select Insurance";
        public string Err200021 = "Please select at least one insurance from the list";
        public string Err200022 = "Unable to delete, purchased insurance.";
        public string Err200023 = "Cannot replace Insurance with cheaper fee, please try again";
        public string Err200024 = "The quantity of Insure must be less or equal to total number of passenger.";
        public string Err200025 = "The insure has been assigned.";
        public string Err200026 = "Fail to assign Insurance, kindly try again later.";

        //Validation Passport
        public string Err300011 = "Passport Expiry date is Required";
        public string Err300012 = "Infant Passport Expiry must more than departure date.";

        //Validation DOB
        public string Err300021 = "DOB is Required";
        public string Err300022 = "DOB must be older than Departure Date.";
        public string Err300034 = "DOB cannot more or equal than today date.";
        public string Err300023 = "Infant must be less than two years old.";

        //Divide
        public string Err300024 = "Please select guest. (Note: divide for all guests is not allowed)";
        public string Err300025 = "Minimum 10 passenger must remain in the original flight.";
        public string Err300026 = "No adult passenger is choosen for divide process. Please choose at least 1 adult passenger.";
        public string Err300027 = "No adult passenger in the original flight. Please remain at least 1 adult passenger.";
        public string Err300028 = "Please agree to the Fare Rules and Terms to continue.";
        public string Err300029 = "You may split bookings up to 48 hours before departure provided there are 2 guests per booking.";
        public string Err300030 = "Divide booking only for PNR which have more than 10 Pax.";
        public string Err300031 = "Please make payment before proceed to divide booking";
        public string Err300032 = "This booking had been Web-checked, divide booking is not allowed";
        public string Err300033 = "Fail to save new PNR, please contact system administrator.";

        //Payment
        public string Err999988 = "You have reached the maximum payment attempts. Your booking has been cancelled. Please kindly re-book your flight.";
        public string Err999989 = "You have reached the maximum payment attempts. Please kindly review your booking again and generate valid payment details.";
        public string Err999990 = "Please take note that some PNR still not yet be confirmed, please select and pay at least minimum payment in order to secure your booking. Otherwise the unconfirmed booking will be expired.";

        public string Err999991 = "Passenger data has been saved successfully.";
        public string Err999992 = "incorrect";
        public string Err999993 = "success";
        public string Err999994 = "error";
        public string Err999995 = "Fail to assign seat, kindly try again";
        public string Err999996 = "Please select seat(s) before proceed.";
        public string Err999997 = "Fail to assign seat, kindly try again later.";
        public string Err999998 = "Record not found.";
        public string Err999999 = "Failed";
        public string Err1000000 = "Your selected seat(s) has been booked by other user";

        //Custom Message
        public string Err800001 = "Pax number can not less then string. Please verify the number of passengers";
        public string Err800002 = "Data not match on row countcon, IssuingCountry not found, for Passenger [name]\n";
        public string Err800003 = "Data not match on row countcon, Nationality not found, for Passenger [name]\n";
        public string Err800004 = "Data not match on row countfe, Title must be 'Mr/Ms' for Adult and 'Chd' for Child, for Passenger [name]\n";
        public string Err800005 = "Data not match on row countma, Gender must be Male/Female, for Passenger [name]\n";
        public string Err800006 = "Data not match on row countdob, Invalid DOB, DOB should be between 1 Jan 1900 and maxaddult, for Passenger [name]\n";
        public string Err800007 = "Data not match on row countdob, Invalid DOB, date format [yyyy-MM-dd], eg: 1999-12-31, for Passenger [name]\n";
        public string Err800008 = "Cannot replace SSRColumns with cheaper fee, please try again";
        public string Err800009 = "pax free name change are allowed";
        public string Err800010 = "Name Changed fee will be charged";
    }

    //public class MessageContainer
    //{
    //    private List<MessageContainer> _listMessage = new List<MessageContainer>();

    //    private string _message = string.Empty;


    //    public string Message
    //    {
    //        get { return _message; }
    //        set { _message = value; }
    //    }

    //    //public List<MessageContainer> ListMessage
    //    //{
    //    //    get { return _listMessage; }
    //    //    set { _listMessage = value; }
    //    //}
    //}

    public partial class MessageControl : System.Web.UI.UserControl
    {
        private string errmsg;
        public ArrayList ListMessage
        {
            get { return (ArrayList)ViewState["listmessage"]; }
            set { ViewState["listmessage"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ArrayList lstmsg = (ArrayList)ViewState["listmessage"];
            //string asdf = string.Empty ;
            //if (Session["123"] != null)
            //    asdf = Session["123"].ToString();

            if (lstmsg != null)
            {
                foreach (string msg in lstmsg)
                {
                    lblmsg.Text = lblmsg.Text + msg.ToString();
                }
                
            }
            
        }

        public void CheckMessage(string msg)
        { 
            MessageList msgList = new MessageList();
            if (msg == msgList.Err100034)
            {
                Thread.Sleep(5000);
                Response.Redirect(Shared.MySite.PublicPages.Searchflight, false);
            }
            //public string Err100034 = "Payment attempt failed.";
        }

        public void MessageDisplay(string Message)
        {
            if (Message != string.Empty)
            {
                lblmsg.Text = Message;
                errmsg = Message;
            }
            pcMessage.ShowOnPageLoad = true;            
        }

    //    protected void btCancel_Click(object sender, EventArgs e)
    //    {
    //        MessageList msglist = new MessageList();
    //        pcMessage.ShowOnPageLoad = false;
    //        if (errmsg == msglist.Err100025)
    //        {
    //            Response.Redirect(Shared.MySite.PublicPages.InvalidPage, false);
    //        }
    //    }
    }
}