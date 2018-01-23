using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ABS.Logic.GroupBooking;

namespace ABS.GBS
{
    public partial class GetBookingTest : System.Web.UI.Page
    {
        ABS.Logic.GroupBooking.BookingMGR objBookingMGR = new ABS.Logic.GroupBooking.BookingMGR();
        string dt = "";
        string hasil = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            BookingMGR.GetBookingResponse bookResp = objBookingMGR.LookUpBooking("Ed6DGE", Request.PhysicalApplicationPath);
            //string a = "a";
            //if (dt == "")
            //{
            //    hasil = null;
            //}
            //else
            //{
            //    hasil = dt;
            //}

        }
    }
}