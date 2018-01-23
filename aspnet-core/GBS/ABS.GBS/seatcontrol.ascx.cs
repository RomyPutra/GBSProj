using ABS.Logic.GroupBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABS.GBS
{
    public partial class seatcontrol : System.Web.UI.UserControl
    {
        #region "Declaration"
        public string xmlurl = "GetSeatAvailability_Response.xml";
        public int numberofpassenger = 2;
        public int currentpassengerindex = 1;
        public int overwritepassengerindex = -1;
        public string selectedseat = "";
        public string defaultseat = "";
        protected string self_no = "";
        protected string self_hidden = "";
        #endregion

        #region "Entity"

        private List<SeatInfo> _SeatInfo;
        public List<SeatInfo> SeatInfo
        {
            get { return _SeatInfo; }
            set { _SeatInfo = value; }
        }
        #endregion

        #region "Load Event"
        protected void Page_Load(object sender, EventArgs e)
        {
            int i = 0;
            string[] s = defaultseat.Split(',');
            //For Each key As String In Request.Params
            //Response.Write(key & "=" & Request(key) & "<br>")
            //Next

            if (overwritepassengerindex > 0)
            {
                currentpassengerindex = overwritepassengerindex;
            }
            if (!string.IsNullOrEmpty(Request["currentindex"]))
            {
                currentpassengerindex = Convert.ToInt32(Request["currentindex"]);
            }
            while (i < numberofpassenger)
            {
                if (string.IsNullOrEmpty(selectedseat))
                {
                    selectedseat = Request["APassengerNumber_" + i];
                }
                else
                {
                    selectedseat += "," + Request["APassengerNumber_" + i];
                }
                i += 1;
            }
            i = 0;

            while (i < currentpassengerindex)
            {
                if (i > 0)
                {
                    self_no += "interface_createunitmap += \",\";" + "\r\n";
                }

                if (!string.IsNullOrEmpty(Request["APassengerNumber_" + i]))
                {
                    self_hidden += "<input type='hidden' name='APassengerNumber_" + i + "' id='APassengerNumber_" + i + "' value='" + Request["APassengerNumber_" + i] + "'>";
                }
                else
                {
                    try
                    {
                        self_hidden += "<input type='hidden' name='APassengerNumber_" + i + "' id='APassengerNumber_" + i + "' value='" + s[i] + "'>";
                    }
                    catch (Exception ex)
                    {
                        self_hidden += "<input type='hidden' name='APassengerNumber_" + i + "' id='APassengerNumber_" + i + "' value='" + Request["APassengerNumber_" + i] + "'>";
                    }
                }

                self_no += "interface_createunitmap += \"{\";" + "\r\n";
                self_no += "interface_createunitmap += \"\\\"blockedSSRExists\\\": false,\";" + "\r\n";
                self_no += "//interface_createunitmap += \"\\\"activateId\\\": \\\"activate_0_0\\\",\";" + "\r\n";
                self_no += "interface_createunitmap += \"\\\"hiddenInputId\\\": \\\"APassengerNumber_" + i + "\\\",\";" + "\r\n";
                self_no += "interface_createunitmap += \"\\\"inputId\\\": \\\"BPassengerNumber_" + i + "\\\",\";" + "\r\n";
                self_no += "interface_createunitmap += \"\\\"passengerNumber\\\": \\\"" + i + "\\\",\";" + "\r\n";
                self_no += "interface_createunitmap += \"\\\"passengerName\\\": \\\"X\\\",\";" + "\r\n";
                if (!string.IsNullOrEmpty(Request["APassengerNumber_" + i]))
                {
                    self_no += "interface_createunitmap += \"\\\"unitKey\\\": \\\"" + Request["APassengerNumber_" + i] + "\\\",\";" + "\r\n";
                }
                try
                {
                    self_no += "interface_createunitmap += \"\\\"unitKey\\\": \\\"" + s[i] + "\\\",\";" + "\r\n";
                }
                catch (Exception ex)
                {
                }

                self_no += "interface_createunitmap += \"\\\"departureStation\\\": \\\"KUL\\\",\";" + "\r\n";
                self_no += "interface_createunitmap += \"\\\"arrivalStation\\\": \\\"KCH\\\",\";" + "\r\n";
                self_no += "interface_createunitmap += \"\\\"equipmentIndex\\\": \\\"0\\\",\";" + "\r\n";
                self_no += "interface_createunitmap += \"\\\"journeyIndex\\\": \\\"0\\\",\";" + "\r\n";
                self_no += "interface_createunitmap += \"\\\"segmentIndex\\\": \\\"0\\\",\";" + "\r\n";
                self_no += "interface_createunitmap += \"\\\"segmentDepartureStation\\\": \\\"KUL\\\",\";" + "\r\n";
                self_no += "interface_createunitmap += \"\\\"segmentArrivalStation\\\": \\\"KCH\\\",\";" + "\r\n";
                self_no += "interface_createunitmap += \"\\\"tripInputId\\\": \\\"TripInput\\\",\";" + "\r\n";
                self_no += "interface_createunitmap += \"\\\"notSeatedTogether\\\": \\\"\\\",\";" + "\r\n";
                self_no += "interface_createunitmap += \"\\\"paxSeatFeeId\\\": \\\"PaxSeatFee_0_0\\\",\";" + "\r\n";
                self_no += "interface_createunitmap += \"\\\"notSeatedTogether\\\": \\\"\\\",\";" + "\r\n";
                self_no += "interface_createunitmap += \"\\\"selectedAutoAssignPropertyArray\\\": [],\";" + "\r\n";
                self_no += "interface_createunitmap += \"\\\"selectedFilterPropertyArray\\\":[],\";" + "\r\n";
                self_no += "interface_createunitmap += \"\\\"selectedFilterSsrArray\\\": [],\";" + "\r\n";
                self_no += "interface_createunitmap += \"\\\"unfulfilledPropertyArray\\\":[],\";" + "\r\n";
                self_no += "interface_createunitmap += \"\\\"paxBday\\\": \\\"12/17/1981\\\",\";" + "\r\n";
                self_no += "interface_createunitmap += \"\\\"isActive\\\": true\";" + "\r\n";
                self_no += "interface_createunitmap += \"}\";" + "\r\n";
                i += 1;
            }
        }
        #endregion
    }
}