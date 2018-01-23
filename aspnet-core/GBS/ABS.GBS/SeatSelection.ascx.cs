using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ABS.Logic.GroupBooking;
using Microsoft.VisualBasic;
using ABS.GBS.Log;

namespace ABS.GBS
{
    public partial class SeatSelection : System.Web.UI.UserControl
    {
        #region "Declaration"
        public string xmlurl = "xml.xml";
        public int numberofpassenger;
        public int currentpassengerindex = 1;
        public int overwritepassengerindex = -1;
        public string selectedseat = "";
        public string defaultseat = "";
        public decimal seatfee = 0;
        //added by ketee
        public string hotseat = "";
        protected string self_no = "";
        protected string self_hidden = "";
        SystemLog SystemLog = new SystemLog();

        #endregion

        #region "Property"

        private List<SeatInfo> _SeatInfo;
        public List<SeatInfo> SeatInfo
        {
            get { return _SeatInfo; }
            set { _SeatInfo = value; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            int i = 0;
            string[] s = defaultseat.Split(',');
            //added by ketee
            string[] h = hotseat.Split(',');
            //For Each key As String In Request.Params
            //Response.Write(key & "=" & Request(key) & "<br>")
            //Next


            if (overwritepassengerindex > 0)
            {
                currentpassengerindex = overwritepassengerindex;
            }
            if (!string.IsNullOrEmpty(Request["currentindex"]))
            {
                currentpassengerindex = Convert.ToInt16(Request["currentindex"]);
            }

            numberofpassenger = 0;
            if ((SeatInfo != null))
            {
                numberofpassenger = SeatInfo.Count;
            }

            while (i < numberofpassenger)
            {
                if (Request["BPassengerNumber_" + i] != null)
                {
                    SeatInfo[i].SelectedSeat = Request["BPassengerNumber_" + i];
                    //if (Request["BPassengerNumber_" + i + "_Fee"] != null && Request["BPassengerNumber_" + i + "_Fee"] != "") SeatInfo[i].SeatAmount = Convert.ToDecimal(Request["BPassengerNumber_" + i + "_Fee"]);

                    string[] aSelectedSeat;
                    if ((Request["APassengerNumber_" + i] != null))
                    {
                        aSelectedSeat = Request["APassengerNumber_" + i].Split('_');
                        if ((aSelectedSeat != null) && aSelectedSeat.Length > 2 && !string.IsNullOrEmpty(aSelectedSeat[1].ToString()))
                        {
                            SeatInfo[i].CompartmentDesignator = aSelectedSeat[1].ToString();
                            SeatInfo[i].Deck = aSelectedSeat[2].ToString();
                        }
                    }


                    if (string.IsNullOrEmpty(selectedseat))
                    {
                        selectedseat = Request["APassengerNumber_" + i];
                    }
                    else
                    {
                        selectedseat += "," + Request["APassengerNumber_" + i];
                    }
                }
                
                i += 1;
            }

            i = 0;

            while (i < currentpassengerindex)
            {
                if (i > 0)
                {
                    self_no += "interface_createunitmap += \",\";" + System.Environment.NewLine;
                }

                if (!string.IsNullOrEmpty(Request["APassengerNumber_" + i]))
                {
                    self_hidden += "<input type='hidden' name='APassengerNumber_" + i + "' id='APassengerNumber_" + i + "' value='" + Request["APassengerNumber_" + i] + "'>";
                }
                else
                {
                    try
                    {
                        //Amended by Tyas 20170920 to fix Airbrake issue
                        if (i < s.Length)
                        {
                            self_hidden += "<input type='hidden' name='APassengerNumber_" + i + "' id='APassengerNumber_" + i + "' value='" + s[i] + "' >";
                        }
                        //added by ketee
                        if (i < h.Length)
                        {
                            self_hidden += "<input type='hidden' name='HotS_" + i + "' id='HotS_" + i + "' value='" + h[i] + "' >";
                        }

                    }
                    catch (Exception ex)
                    {
                        SystemLog.Notifier.Notify(ex);
                        self_hidden += "<input type='hidden' name='APassengerNumber_" + i + "' id='APassengerNumber_" + i + "' value='" + Request["APassengerNumber_" + i] + "' >";
                        //added by ketee
                        self_hidden += "<input type='hidden' name='HotS_" + i + "' id='HotS_" + i + "' value='" + Request["HotS_" + i] + "' >";
                    }
                }

                self_no += "interface_createunitmap += \"{\";" + System.Environment.NewLine;
                self_no += "interface_createunitmap += \"\\\"blockedSSRExists\\\": false,\";" + System.Environment.NewLine;
                self_no += "//interface_createunitmap += \"\\\"activateId\\\": \\\"activate_0_0\\\",\";" + System.Environment.NewLine;
                self_no += "interface_createunitmap += \"\\\"hiddenInputId\\\": \\\"APassengerNumber_" + i + "\\\",\";" + System.Environment.NewLine;
                self_no += "interface_createunitmap += \"\\\"inputId\\\": \\\"BPassengerNumber_" + i + "\\\",\";" + System.Environment.NewLine;
                //added by ketee
                self_no += "interface_createunitmap += \"\\\"hiddenIsHot\\\": \\\"HotS_" + i + "\\\",\";" + System.Environment.NewLine;
                //--------end ---------
                self_no += "interface_createunitmap += \"\\\"passengerNumber\\\": \\\"" + i + "\\\",\";" + System.Environment.NewLine;
                self_no += "interface_createunitmap += \"\\\"passengerName\\\": \\\"X\\\",\";" + System.Environment.NewLine;
                if (!string.IsNullOrEmpty(Request["APassengerNumber_" + i]))
                {
                    self_no += "interface_createunitmap += \"\\\"unitKey\\\": \\\"" + Request["APassengerNumber_" + i] + "\\\",\";" + System.Environment.NewLine;
                }
                try
                {
                    self_no += "interface_createunitmap += \"\\\"unitKey\\\": \\\"" + s[i] + "\\\",\";" + System.Environment.NewLine;
                }
                catch (Exception ex)
                {
                }

                self_no += "interface_createunitmap += \"\\\"departureStation\\\": \\\"KUL\\\",\";" + System.Environment.NewLine;
                self_no += "interface_createunitmap += \"\\\"arrivalStation\\\": \\\"KCH\\\",\";" + System.Environment.NewLine;
                self_no += "interface_createunitmap += \"\\\"equipmentIndex\\\": \\\"0\\\",\";" + System.Environment.NewLine;
                self_no += "interface_createunitmap += \"\\\"journeyIndex\\\": \\\"0\\\",\";" + System.Environment.NewLine;
                self_no += "interface_createunitmap += \"\\\"segmentIndex\\\": \\\"0\\\",\";" + System.Environment.NewLine;
                self_no += "interface_createunitmap += \"\\\"segmentDepartureStation\\\": \\\"KUL\\\",\";" + System.Environment.NewLine;
                self_no += "interface_createunitmap += \"\\\"segmentArrivalStation\\\": \\\"KCH\\\",\";" + System.Environment.NewLine;
                self_no += "interface_createunitmap += \"\\\"tripInputId\\\": \\\"TripInput\\\",\";" + System.Environment.NewLine;
                self_no += "interface_createunitmap += \"\\\"notSeatedTogether\\\": \\\"\\\",\";" + System.Environment.NewLine;
                self_no += "interface_createunitmap += \"\\\"paxSeatFeeId\\\": \\\"PaxSeatFee_0_0\\\",\";" + System.Environment.NewLine;
                self_no += "interface_createunitmap += \"\\\"notSeatedTogether\\\": \\\"\\\",\";" + System.Environment.NewLine;
                self_no += "interface_createunitmap += \"\\\"selectedAutoAssignPropertyArray\\\": [],\";" + System.Environment.NewLine;
                self_no += "interface_createunitmap += \"\\\"selectedFilterPropertyArray\\\":[],\";" + System.Environment.NewLine;
                self_no += "interface_createunitmap += \"\\\"selectedFilterSsrArray\\\": [],\";" + System.Environment.NewLine;
                self_no += "interface_createunitmap += \"\\\"unfulfilledPropertyArray\\\":[],\";" + System.Environment.NewLine;
                self_no += "interface_createunitmap += \"\\\"paxBday\\\": \\\"12/17/1981\\\",\";" + System.Environment.NewLine;
                self_no += "interface_createunitmap += \"\\\"isActive\\\": true\";" + System.Environment.NewLine;
                self_no += "interface_createunitmap += \"}\";" + System.Environment.NewLine;
                i += 1;
            }
        }
    }

    
}