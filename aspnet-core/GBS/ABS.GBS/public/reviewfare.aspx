<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reviewfare.aspx.cs" Inherits="GroupBooking.Web.Booking.selectFlightAverage" MasterPageFile="~/Master/NewPageMaster.Master" %>

<%@ MasterType VirtualPath="~/Master/NewPageMaster.Master" %>
<%@ Register Src="../UserControl/flighdetail.ascx" TagName="flightdetail" TagPrefix="fd" %>
<%--<%@ Register Src="../UserControl/DetailBreakdownPerpax.ascx" TagName="DetailBreakdown" TagPrefix="db" %>--%>
<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolder2">
    <script src="../js/clientclick.js" type="text/javascript"></script>
    <style type="text/css" media="print">
        .blankprint {
            visibility: hidden;
            display: none;
        }
    </style>
    <script type="text/javascript">
    <!--
    //Disable right click 
    /scripts/
    var message = "Sorry, right-click has been disabled";
    /////////////////////////////////// 
    function clickIE() { if (document.all) { (message); return false; } }
    function clickNS(e) {
        if (document.layers || (document.getElementById && !document.all)) {
            if (e.which == 2 || e.which == 3) { (message); return false; }
        }
    }
    if (document.layers)
    { document.captureEvents(Event.MOUSEDOWN); document.onmousedown = clickNS; }
    else { document.onmouseup = clickNS; document.oncontextmenu = clickIE; }
    document.oncontextmenu = new Function("return false")
    // --> 

    function toggle() {
        var ele = document.getElementById("toggleText");
        var text = document.getElementById("displayText");
        if (ele.style.display == "block") {
            ele.style.display = "none";
            text.innerHTML = "+";

        }
        else {
            ele.style.display = "block";
            text.innerHTML = "-";

        }
    }

    function closepopup() {
        window.location = "../public/searchflight.aspx";
    }

    function ConfirmReview(s, e) {
        clearTimeout(varTmr);
        if (e.result != "") {
            document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = e.result;
            pcMessage.Show();
            LoadingPanel.Hide();
        }
        else {
            //change to new add-On table, Tyas
            //window.location.href = '../public/AddOn.aspx';
            window.location.href = '../public/SelectAddOn.aspx';
        }
        //window.location.href = '../public/SelectSeat.aspx';
    }

    function nextPage() {
        window.location.href = "../public/reviewbooking.aspx";
    }


    // ]]> 
    </script>
    <msg:msgControl ID="msgcontrol" runat="server" />
    <dx:ASPxCallback ID="Callback" runat="server" ClientInstanceName="Callback" OnCallback="ValidatePopup">
        <ClientSideEvents CallbackComplete="function(s, e) { clearTimeout(varTmr); ConfirmReview(s, e);  }" />
    </dx:ASPxCallback>

    <div style="display: none">
        <asp:Label ID="LblReturn" runat="server" Text="Destination :"></asp:Label>
        <asp:Label ID="lbl_GuestNumout" runat="server"></asp:Label>
        <asp:Label ID="lbl_ChildNumout" runat="server"></asp:Label>
        <asp:Label ID="lbl_PaxNumout" runat="server"></asp:Label>
    </div>
    <div class="page-header row">
        <div class="col-sm-4">
            <h4 class="mt-0 mb-5">Fare Review</h4>
            Booking/Fare Review
        </div>
        <div class="col-sm-8 right">
            <%--<table border="0" cellpadding="0" width="9px" cellspacing="0">
        <tr>
            
            <td>
                
            </td>
        </tr>
    </table>--%>
            <dx:ASPxLabel ID="lblMsg" ClientInstanceName="lblMsg" runat="server" Visible="false"></dx:ASPxLabel>
            <asp:HiddenField ID="hCommand" runat="server" Value="" />
            <asp:HiddenField ID="ctrHdn" runat="server" />
            <%-- <asp:ScriptManager runat="server" ID="gbsscriptmanager">
        
    </asp:ScriptManager>
        
        <asp:Label runat="server" ID="lblTimer"></asp:Label>--%>

            <div id="counterAlert">
                <%--<asp:ScriptManager ID="ScriptManager1" runat="server">
     </asp:ScriptManager>
     <asp:Timer ID="Timer1" runat="server" ontick="Timer1_Tick" Interval="1000" Enabled="false">
     </asp:Timer>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
           <ContentTemplate>
               <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
           </ContentTemplate>
               <Triggers>

                   <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />

               </Triggers>
     </asp:UpdatePanel>--%>
                <p>Time Remaining</p>
                min<dx:ASPxLabel runat="server" ID="lblTimer" ClientInstanceName="lblTimer" Text="2:00"></dx:ASPxLabel>
                sec
            </div>



            <dx:ASPxButton CssClass="buttonL" ID="btOK"
                runat="server" Text="Confirm" AutoPostBack="false">
                <ClientSideEvents Click="function(s, e) { clearTimeout(varTmr); LoadingPanel.SetText(&#39;Please Wait...&#39;);LoadingPanel.Show(); Callback.PerformCallback(); 
                    }"></ClientSideEvents>
            </dx:ASPxButton>

        </div>
    </div>

    <div class="col-sm-12">
        <div id="fareInformation" class="col-sm-9">
            <div style="padding-top: 20px;">

                <div id="infoTable" class="tableWrapper specialArrange" style="margin-top: 5px;">
                    <table id="reviewTable" style="width: 100%;" class="table table-bordered">
                        <tr>
                            <td>
                                <table class="reviewLeft">
                                    <tr>

                                        <td colspan="2" style="text-align: center"><span>Total Price:</span></td>
                                        <td><span id="totalAverage" style="font-weight: 700; padding: 0 10px;">
                                            <dx:ASPxLabel ID="lblTotalFare" runat="server"></dx:ASPxLabel>
                                            <dx:ASPxLabel ID="lblTotFareCurrency" runat="server"></dx:ASPxLabel>
                                        </span></td>


                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align: center"><span>Average Fare per Pax:</span></td>
                                        <td><span id="averageFareAmount" style="padding: 0 10px;">
                                            <dx:ASPxLabel ID="lblAverageFare" runat="server"></dx:ASPxLabel>
                                            <dx:ASPxLabel ID="lblTotalCurrency" runat="server"></dx:ASPxLabel>
                                        </span></td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table class="reviewRight">
                                    <tr>
                                        <td colspan="2" style="text-align: center"><span>Total Pax:</span></td>
                                        <td><span id="totalPax" style="padding: 0 10px;">
                                            <dx:ASPxLabel ID="lblTotPax" runat="server"></dx:ASPxLabel>
                                        </span></td>
                                    </tr>
                                    <tr id="trInfantTotal" runat="server">
                                        <td colspan="2" style="text-align: center"><span>Total Infant:</span></td>
                                        <td><span id="totalInfant" style="">
                                            <dx:ASPxLabel ID="lblTotInfant" runat="server"></dx:ASPxLabel>
                                        </span></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>

                </div>
            </div>
            <div id="flightDetailBookingInfoNew" class="" style="">
                <div class="redSectionHeader noShow">
                    <div>Fare Details</div>
                </div>
                <table width="100%" bgcolor="#f7f3f7" class="table table-bordered tdWidth50">

                    <tr>
                        <td><font>Depart</font></td>
                        <td runat="server" id="tdReturnText"><font>Return</font></td>
                    </tr>
                    <tr>
                        <td id="tdDepart" runat="server">
                            <table class="tablebreakdown">
                                <tr id="trPaxfareDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="lbl_num1" runat="server" Visible="false"></asp:Label>
                                                Fare per Pax
                                            </span>
                                            <span class="infoFlightSpan algnRight">
                                                <dx:ASPxLabel ID="lblPaxFareDepart" runat="server"></dx:ASPxLabel>
                                                <asp:Label ID="lbl_FlightTotal" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lbl_currency1" runat="server"></asp:Label>
                                            </span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trAirportTaxDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="lbl_num2" runat="server" Visible="false"> </asp:Label>
                                                Airport Tax per Pax </span>
                                            <span class="infoFlightSpan algnRight">
                                                <dx:ASPxLabel ID="lblTaxDepart" runat="server"></dx:ASPxLabel>
                                                <asp:Label ID="lbl_currency3" runat="server"></asp:Label>
                                                <asp:Label ID="lbl_taxTotal" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lbl_currency2" runat="server" Visible="false"></asp:Label>
                                            </span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trChildAirportTaxDepart" runat="server" visible="false">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="lbl_num2CHD" runat="server" Visible="false"></asp:Label>
                                                Child Airport Tax per Pax</span>
                                            <span class="infoFlightSpan algnRight">
                                                <dx:ASPxLabel ID="lblTaxDepartChild" runat="server"></dx:ASPxLabel>
                                                <asp:Label ID="lbl_currency3CHD" runat="server"></asp:Label>
                                                <asp:Label ID="lbl_taxTotalCHD" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lbl_currency2CHD" runat="server" Visible="false"></asp:Label>
                                            </span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trPaxServChargeDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="lbl_num21" runat="server" Visible="false"> </asp:Label>
                                                Service Charge per Pax </span>
                                            <span class="infoFlightSpan algnRight">
                                                <dx:ASPxLabel ID="lblPaxFeeDepart" runat="server"></dx:ASPxLabel>
                                                <asp:Label ID="lbl_currency31" runat="server"></asp:Label>
                                                <asp:Label ID="lbl_PaxFeeTotal" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lbl_currency21" runat="server" Visible="false"></asp:Label>
                                            </span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trFuelTaxDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="lbl_num3" runat="server" Visible="false"></asp:Label>
                                                Fuel Tax per Pax</span>
                                            <span class="infoFlightSpan algnRight">
                                                <dx:ASPxLabel ID="lblFuelDepart" runat="server"></dx:ASPxLabel>
                                                <asp:Label ID="lbl_currency4" runat="server"></asp:Label>
                                                <asp:Label ID="lblFuelPriceTotalDepart" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblCurrFuelDepart" runat="server" Visible="false"></asp:Label>
                                            </span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trServChargeDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="lbl_num4" runat="server" Visible="false"></asp:Label>
                                                Passenger Service Charge per Pax </span>
                                            <span class="infoFlightSpan algnRight">
                                                <dx:ASPxLabel ID="lblSvcDepart" runat="server"></dx:ASPxLabel>
                                                <asp:Label ID="lbl_currency5" runat="server"></asp:Label>
                                            </span>
                                            <span class="infoFlightSpan algnRight" style="display: none;">
                                                <asp:Label ID="lblSvcChargeTotalDepart" runat="server"></asp:Label>
                                                <asp:Label ID="lblCurrSvcDepart" runat="server"></asp:Label>
                                            </span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trVATDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="lbl_num5" runat="server" Visible="false"></asp:Label>
                                                VAT per Pax</span>
                                            <span class="infoFlightSpan algnRight">
                                                <dx:ASPxLabel ID="lblVATDepart" runat="server"></dx:ASPxLabel>
                                                <asp:Label ID="lbl_currency6" runat="server"></asp:Label>
                                                <asp:Label ID="lblVATTotalDepart" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblCurrVATDepart" runat="server" Visible="false"></asp:Label>
                                            </span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trOthChargeDepart" runat="server" visible="false">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="lbl_num6" runat="server" Visible="false"></asp:Label>
                                                Other Charge per Pax</span>
                                            <span class="infoFlightSpan algnRight">
                                                <dx:ASPxLabel ID="lblOthDepart" runat="server"></dx:ASPxLabel>
                                                <asp:Label ID="lbl_currency7" runat="server"></asp:Label>
                                                <asp:Label ID="lblOthTotalDepart" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblCurrOthDepart" runat="server" Visible="false"></asp:Label>
                                            </span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trConnectingChargeDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label1" runat="server"></asp:Label>
                                                Connecting Charge per Pax
                                                <asp:Label ID="Label2" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="Label3" runat="server" Visible="false"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblConnectingDepartTotal" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrConnectingDepart" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trDiscountChargeDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label12" runat="server"></asp:Label>Discount Charge</span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblDiscTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrDiscDepart" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trPromoDiscountDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="lbl_num9" runat="server" Visible="false"></asp:Label>
                                                Promotion Discount per Pax</span>
                                            <span class="infoFlightSpan algnRight">
                                                <dx:ASPxLabel ID="lblPromoDiscDepart" runat="server"></dx:ASPxLabel>
                                                <asp:Label ID="lbl_currency9" runat="server"></asp:Label>
                                                <asp:Label ID="lblPromoDiscTotalDepart" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblCurrPromoDiscDepart" runat="server" Visible="false"></asp:Label>
                                            </span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trACFChargeDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label31" runat="server"></asp:Label>
                                                <asp:Label ID="lblACFInfoDepart" runat="server"></asp:Label>
                                            </span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblACFTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrACFDepart" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trAPFChargeDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label55" runat="server"></asp:Label>
                                                <asp:Label ID="lblAPFInfoDepart" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblAPFTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrAPFDepart" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trAPFCChargeDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label59" runat="server"></asp:Label>
                                                <asp:Label ID="lblAPFCInfoDepart" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblAPFCTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrAPFCDepart" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr> 
                                <tr id="trAPSChargeDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label9" runat="server"></asp:Label>
                                                <asp:Label ID="lblAPSInfoDepart" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblAPSTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrAPSDepart" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trASCChargeDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label32" runat="server"></asp:Label>
                                                <asp:Label ID="lblASCInfoDepart" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblASCTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrASCDepart" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trAVLChargeDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label10" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblAVLInfoDepart" runat="server"></asp:Label>
                                                <asp:Label ID="lblSvcChargeOneDepart" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="Label11" runat="server" Visible="false"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblAVLTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrAVLDepart" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trBCLChargeDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label41" runat="server"></asp:Label>
                                                <asp:Label ID="lblBCLInfoDepart" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblBCLTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrBCLDepart" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trCSTChargeDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label21" runat="server"></asp:Label>
                                                <asp:Label ID="lblCSTInfoDepart" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblCSTTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrCSTDepart" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trCUTChargeDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label25" runat="server"></asp:Label>
                                                <asp:Label ID="lblCUTInfoDepart" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblCUTTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrCUTDepart" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trGSTChargeDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label7" runat="server"></asp:Label>
                                                <asp:Label ID="lblGSTInfoDepart" runat="server"></asp:Label>
                                                <asp:Label ID="lbl_PaxFeePrice" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="Label8" runat="server" Visible="false"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lbl_GSTTotal" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrGSTDepart" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trIADFChargeDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label18" runat="server"></asp:Label>
                                                <asp:Label ID="lblIADFInfoDepart" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblIADFTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrIADFDepart" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trIPSCChargeDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label58" runat="server"></asp:Label>
                                                <asp:Label ID="lblIPSCInfoDepart" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblIPSCTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrIPSCDepart" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trISFChargeDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label65" runat="server"></asp:Label>
                                                <asp:Label ID="lblISFInfoDepart" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblISFTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrISFDepart" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr> 
                                <tr id="trIWJRChargeDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label35" runat="server"></asp:Label>
                                                <asp:Label ID="lblIWJRInfoDepart" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblIWJRTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrIWJRDepart" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trKlia2FeeDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label4" runat="server"></asp:Label>
                                                <asp:Label ID="lblKLIA2InfoDepart" runat="server"></asp:Label> 
                                                <asp:Label ID="lbl_taxPrice" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="Label5" runat="server" Visible="false"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lbl_klia2Total" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrKlia2Depart" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trPSCChargeDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label60" runat="server"></asp:Label>
                                                <asp:Label ID="lblPSCInfoDepart" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblPSCTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrPSCDepart" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trPSFChargeDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="lbl_num15" runat="server"></asp:Label>
                                                <asp:Label ID="lblPSFInfoDepart" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblPSFTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrPSFDepart" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trSCFChargeDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="lbl_num16" runat="server"></asp:Label>
                                                <asp:Label ID="lblSCFInfoDepart" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblSCFTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrSCFDepart" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trSGIChargeDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label26" runat="server"></asp:Label>
                                                <asp:Label ID="lblSGIInfoDepart" runat="server"></asp:Label>
                                            </span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblSGITotalDepart" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrSGIDepart" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trSPLChargeDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label6" runat="server"></asp:Label>
                                                <asp:Label ID="lblSPLInfoDepart" runat="server"></asp:Label>
                                            </span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblSPLTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrSPLDepart" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trSSTChargeDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label27" runat="server"></asp:Label>
                                                <asp:Label ID="lblSSTInfoDepart" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblSSTTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrSSTDepart" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trUDFChargeDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label28" runat="server"></asp:Label>
                                                <asp:Label ID="lblUDFInfoDepart" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblUDFTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrUDFDepart" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trVATChargeDepart" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label46" runat="server"></asp:Label>
                                                <asp:Label ID="lblVATInfoDepart" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblVATChargeTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrVATChargeDepart" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <%-- <tr>
                                    <td>
                                        <span class="infoFlightSpan boldFont"><font>Total Amount per Pax</font></span>
                                        <span class="infoFlightSpan algnRight">
                                            <dx:ASPxLabel ID="lblDepartFare" runat="server"></dx:ASPxLabel>
                                            <dx:ASPxLabel ID="lblDepartCurrency" runat="server"></dx:ASPxLabel>
                                        </span>
                                    </td>
                                </tr>--%>

                                <asp:Repeater ID="rptFeeDepart" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <div class="infoFlight">
                                                    <span class="infoFlightSpan">
                                                        <asp:Label ID="Label49" runat="server"></asp:Label>
                                                        <asp:Label ID="lblCodeDescDepart" runat="server" Text='<%# Eval("CodeDesc") + " Charge Per Pax" %>'></asp:Label></span>
                                                    <span class="infoFlightSpan algnRight">
                                                        <asp:Label ID="lblFeeAmtDepart" runat="server" Text='<%# Eval("FeeAmtPerPax", "{0:N2}") %>'></asp:Label>
                                                        <asp:Label ID="lblFeeCurrDepart" runat="server"></asp:Label></span>
                                                </div>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </td>
                        <td id="tdReturn" runat="server">
                            <table class="tablebreakdown">
                                <tr id="trPaxfareReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="lbl_InNum" runat="server" Visible="false"></asp:Label>
                                                Fare per Pax 
                                            </span>
                                            <span class="infoFlightSpan algnRight">
                                                <dx:ASPxLabel ID="lblPaxFareReturn" runat="server"></dx:ASPxLabel>
                                                <asp:Label ID="lbl_InCurrency0" runat="server"></asp:Label>
                                                <asp:Label ID="lbl_InFlightTotal" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lbl_Incurrency1" runat="server" Visible="false"></asp:Label>
                                            </span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trAirportTaxReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="lbl_InNum2" runat="server" Visible="false"></asp:Label>
                                                Airport Tax per Pax </span>
                                            <span class="infoFlightSpan algnRight">
                                                <dx:ASPxLabel ID="lblTaxReturn" runat="server"></dx:ASPxLabel>
                                                <asp:Label ID="lbl_InCurrency3" runat="server"></asp:Label>
                                                <asp:Label ID="lbl_IntaxTotal" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lbl_Incurrency2" runat="server" Visible="false"></asp:Label>
                                            </span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trChildAirportTaxReturn" runat="server" visible="false">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="lbl_InNum2CHD" runat="server" Visible="false"></asp:Label>
                                                Child Airport Tax per Pax</span>
                                            <span class="infoFlightSpan algnRight">
                                                <dx:ASPxLabel ID="lblTaxReturnChild" runat="server"></dx:ASPxLabel>
                                                <asp:Label ID="lbl_InCurrency3CHD" runat="server"></asp:Label>
                                                <asp:Label ID="lbl_IntaxTotalCHD" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lbl_Incurrency2CHD" runat="server" Visible="false"></asp:Label>
                                            </span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trPaxServChargeReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="lbl_InNum21" runat="server" Visible="false"></asp:Label>
                                                Service Charge per Pax </span>
                                            <span class="infoFlightSpan algnRight">
                                                <dx:ASPxLabel ID="lblPaxFeeReturn" runat="server"></dx:ASPxLabel>
                                                <asp:Label ID="lbl_InCurrency31" runat="server"></asp:Label>
                                                <asp:Label ID="lbl_InPaxFeeTotal" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lbl_Incurrency21" runat="server" Visible="false"></asp:Label>
                                            </span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trFuelTaxReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="lbl_InNum3" runat="server" Visible="false"></asp:Label>
                                                Fuel Tax per Pax</span>
                                            <span class="infoFlightSpan algnRight">
                                                <dx:ASPxLabel ID="lblFuelReturn" runat="server"></dx:ASPxLabel>
                                                <asp:Label ID="lbl_InCurrency4" runat="server"></asp:Label>
                                                <asp:Label ID="lblFuelTotalReturn" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblCurrFuelReturn" runat="server" Visible="false"></asp:Label>
                                            </span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trServChargeReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="lbl_InNum4" runat="server" Visible="false"></asp:Label>
                                                Passenger Service Charge per Pax </span>
                                            <span class="infoFlightSpan algnRight">
                                                <dx:ASPxLabel ID="lblSvcReturn" runat="server"></dx:ASPxLabel>
                                                <asp:Label ID="lbl_InCurrency5" runat="server"></asp:Label>
                                            </span>
                                            <span class="infoFlightSpan algnRight" style="display: none;">
                                                <asp:Label ID="lblSvcTotalReturn" runat="server"></asp:Label>
                                                <asp:Label ID="lblCurrSvcReturn" runat="server"></asp:Label>
                                            </span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trVATReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="lbl_InNum5" runat="server" Visible="false"></asp:Label>
                                                VAT per Pax</span>
                                            <span class="infoFlightSpan algnRight">
                                                <dx:ASPxLabel ID="lblVATReturn" runat="server"></dx:ASPxLabel>
                                                <asp:Label ID="lbl_InCurrency6" runat="server"></asp:Label>
                                                <asp:Label ID="lblVATTotalReturn" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblCurrVATReturn" runat="server" Visible="false"></asp:Label>
                                            </span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trOthChargeReturn" runat="server" visible="false">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="lbl_InNum6" runat="server" Visible="false"></asp:Label>
                                                Other Charge per Pax</span>
                                            <span class="infoFlightSpan algnRight">
                                                <dx:ASPxLabel ID="lblOthReturn" runat="server"></dx:ASPxLabel>
                                                <asp:Label ID="lbl_InCurrency7" runat="server"></asp:Label>
                                                <asp:Label ID="lblOthTotalReturn" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblCurrOthReturn" runat="server" Visible="false"></asp:Label>
                                            </span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trConnectingChargeReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label13" runat="server">Connecting Charge per Pax</asp:Label><asp:Label ID="Label14" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="Label15" runat="server" Visible="false"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblConnectingReturnTotal" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrConnectingReturn" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trDiscountChargeReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label24" runat="server"></asp:Label>Discount Charge</span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblDiscTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrDiscReturn" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trPromoDiscountReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="lbl_InNum9" runat="server" Visible="false"></asp:Label>
                                                Promotion Discount per Pax</span>
                                            <span class="infoFlightSpan algnRight">
                                                <dx:ASPxLabel ID="lblPromoDiscReturn" runat="server"></dx:ASPxLabel>
                                                <asp:Label ID="lbl_InCurrency9" runat="server"></asp:Label>
                                                <asp:Label ID="lblPromoDiscTotalReturn" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblCurrPromoDiscReturn" runat="server" Visible="false"></asp:Label>
                                            </span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trACFChargeReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label43" runat="server"></asp:Label>
                                                <asp:Label ID="lblACFInfoReturn" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblACFTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrACFReturn" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trAPFChargeReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label56" runat="server"></asp:Label>
                                                <asp:Label ID="lblAPFInfoReturn" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblAPFTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrAPFReturn" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trAPFCChargeReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label61" runat="server"></asp:Label>
                                                <asp:Label ID="lblAPFCInfoReturn" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblAPFCTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrAPFCReturn" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trAPSChargeReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label44" runat="server"></asp:Label>
                                                <asp:Label ID="lblAPSInfoReturn" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblAPSTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrAPSReturn" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trASCChargeReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label30" runat="server"></asp:Label>
                                                <asp:Label ID="lblASCInfoReturn" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblASCTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrASCReturn" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trAVLChargeReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label22" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblAVLInfoReturn" runat="server"></asp:Label>
                                                <asp:Label ID="lblSvcOneReturn" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="Label23" runat="server" Visible="false"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblAVLTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrAVLReturn" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trBCLChargeReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label38" runat="server"></asp:Label>
                                                <asp:Label ID="lblBCLInfoReturn" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblBCLTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrBCLReturn" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trCSTChargeReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label29" runat="server"></asp:Label>
                                                <asp:Label ID="lblCSTInfoReturn" runat="server"></asp:Label>
                                            </span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblCSTTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrCSTReturn" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trCUTChargeReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label33" runat="server"></asp:Label>
                                                <asp:Label ID="lblCUTInfoReturn" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblCUTTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrCUTReturn" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trGSTChargeReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label19" runat="server"></asp:Label>
                                                <asp:Label ID ="lblGSTInfoReturn" runat="server"></asp:Label>
                                                <asp:Label ID="lbl_InPaxFeePrice" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="Label20" runat="server" Visible="false"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lbl_InGSTTotal" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrGSTReturn" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trIADFChargeReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label47" runat="server"></asp:Label>
                                                <asp:Label ID="lblIADFInfoReturn" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblIADFTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrIADFReturn" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trIPSCChargeReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label57" runat="server"></asp:Label>
                                                <asp:Label ID="lblIPSCInfoReturn" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblIPSCTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrIPSCReturn" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trISFChargeReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label63" runat="server"></asp:Label>
                                                <asp:Label ID="lblISFInfoReturn" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblISFTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCUrrISFReturn" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trIWJRChargeReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label36" runat="server"></asp:Label>
                                                <asp:Label ID="lblIWJRInfoReturn" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblIWJRTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrIWJRReturn" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trKlia2FeeReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label16" runat="server"></asp:Label>
                                                <asp:Label ID="lblKLIA2InfoReturn" runat="server"></asp:Label>
                                                <asp:Label ID="lbl_IntaxPrice" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="Label17" runat="server" Visible="false"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lbl_Inklia2Total" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrKlia2Return" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trPSCChargeReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label62" runat="server"></asp:Label>
                                                <asp:Label ID="lblPSCInfoReturn" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblPSCTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrPSCReturn" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trPSFChargeReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="lbl_InNum15" runat="server"></asp:Label>
                                                <asp:Label ID="lblPSFInfoReturn" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblPSFTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrPSFReturn" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trSCFChargeReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="lbl_InNum16" runat="server"></asp:Label>
                                                <asp:Label ID="lblSCFInfoReturn" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblSCFTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrSCFReturn" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trSGIChargeReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label37" runat="server"></asp:Label>
                                                <asp:Label ID="lblSGIInfoReturn" runat="server"></asp:Label>
                                            </span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblSGITotalReturn" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrSGIReturn" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trSPLChargeReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label34" runat="server"></asp:Label>
                                                <asp:Label ID="lblSPLInfoReturn" runat="server"></asp:Label>
                                            </span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblSPLTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrSPLReturn" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trSSTChargeReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label40" runat="server"></asp:Label>
                                                <asp:Label ID="lblSSTInfoReturn" runat="server"></asp:Label>
                                            </span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblSSTTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrSSTReturn" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trUDFChargeReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label45" runat="server"></asp:Label>
                                                <asp:Label ID="lblUDFInfoReturn" runat="server"></asp:Label>
                                            </span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblUDFTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrUDFReturn" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trVATChargeReturn" runat="server">
                                    <td>
                                        <div class="infoFlight">
                                            <span class="infoFlightSpan">
                                                <asp:Label ID="Label49" runat="server"></asp:Label>
                                                <asp:Label ID="lblVATInfoReturn" runat="server"></asp:Label></span>
                                            <span class="infoFlightSpan algnRight">
                                                <asp:Label ID="lblVATChargeTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                <asp:Label ID="lblCurrVATChargeReturn" runat="server"></asp:Label></span>
                                        </div>
                                    </td>
                                </tr>

                                <%-- <tr>
                                    <td id="tdReturnTitle" runat="server">
                                        <span class="infoFlightSpan boldFont"><font>Total Amount per Pax</font></span>
                                        <span class="infoFlightSpan algnRight">
                                            <dx:ASPxLabel ID="lblReturnFare" runat="server"></dx:ASPxLabel>
                                            <dx:ASPxLabel ID="lblReturnCurrency" runat="server"></dx:ASPxLabel>
                                        </span>
                                    </td>
                                </tr>--%>

                                <asp:Repeater ID="rptFeeReturn" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <div class="infoFlight">
                                                    <span class="infoFlightSpan">
                                                        <asp:Label ID="Label49" runat="server"></asp:Label>
                                                        <asp:Label ID="lblCodeDescReturn" runat="server" Text='<%# Eval("CodeDesc") + " Charge Per Pax" %>'></asp:Label></span>
                                                    <span class="infoFlightSpan algnRight">
                                                        <asp:Label ID="lblFeeAmtReturn" runat="server" Text='<%# Eval("FeeAmtPerPax", "{0:N2}") %>'></asp:Label>
                                                        <asp:Label ID="lblFeeCurrReturn" runat="server"></asp:Label></span>
                                                </div>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="infoFlightSpan boldFont"><font>Total Amount per Pax</font></span>
                            <span class="infoFlightSpan algnRight">
                                <dx:ASPxLabel ID="lblDepartFare" runat="server"></dx:ASPxLabel>
                                <dx:ASPxLabel ID="lblDepartCurrency" runat="server"></dx:ASPxLabel>
                            </span>
                        </td>
                        <td id="tdReturnTitle" runat="server">
                            <span class="infoFlightSpan boldFont"><font>Total Amount per Pax</font></span>
                            <span class="infoFlightSpan algnRight">
                                <dx:ASPxLabel ID="lblReturnFare" runat="server"></dx:ASPxLabel>
                                <dx:ASPxLabel ID="lblReturnCurrency" runat="server"></dx:ASPxLabel>
                            </span>
                        </td>
                    </tr>
                </table>
                <%--                <div id="flightDetailBreakdown" class="" style="">
                    <db:DetailBreakdown ID="Detailbreakdown" runat="server" />

                </div>--%>
            </div>

        </div>
        <dx:ASPxImage runat="server" ID="imgStatus" ClientInstanceName="imgStatus" Width="0" Height="0"></dx:ASPxImage>
        <dx:ASPxImage runat="server" ID="imgMessage" ClientInstanceName="imgMessage" Width="0" Height="0"></dx:ASPxImage>
    </div>

    <div class="page-header row">
        <div class="col-sm-4">
        </div>
        <div class="col-sm-8 right">
            <%--<table border="0" cellpadding="0" width="9px" cellspacing="0">
        <tr>
            
            <td>
                
            </td>
        </tr>
    </table>--%>
            <dx:ASPxButton CssClass="buttonL" ID="btnConfirmBottom"
                runat="server" Text="Confirm" AutoPostBack="false">
                <ClientSideEvents Click="function(s, e) { clearTimeout(varTmr); LoadingPanel.SetText(&#39;Please Wait...&#39;);
                        LoadingPanel.Show(); Callback.PerformCallback(); ConfirmReview();
                    }"></ClientSideEvents>
            </dx:ASPxButton>

        </div>
    </div>

    <%--<asp:Timer ID="tTimer" runat="server" Interval="1000" OnTick="tTimer_Tick">
                    </asp:Timer>--%>

    <script type="text/javascript">

        var varTmr;
        var active = 0;
        function countdown() {
            seconds = lblTimer.GetValue();
            var timeArray = seconds.split(/[:]+/);
            var m = timeArray[0];
            var s = checkSecond((timeArray[1] - 1));
            
            if (m > 0 || s > 0) {
                if (s == 59) { m = m - 1; }
                lblTimer.SetText(m + ":" + s);
                varTmr = setTimeout("countdown()", 1000);
            } else {
                window.location.href = '../public/SelectFlight.aspx';
            }
            

            //if (seconds > 0) {
            //    active = 0;
            //    lblTimer.SetText(seconds - 1);
            //    varTmr = setTimeout("countdown()", 1000);
            //} else {
            //    window.location.href = '../public/SelectFlight.aspx';
            //}
        }

        function startTimer() {
            varTmr = setTimeout("countdown()", 1000);
        }

        function checkSecond(sec) {
            if (sec < 10 && sec >= 0) { sec = "0" + sec }; // add zero in front of numbers < 10
            if (sec < 0) { sec = "59" };
            return sec;
        }

        window.onload = startTimer();

    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <fd:flightdetail ID="flightdetail" runat="server" />
</asp:Content>
