<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewPageMaster.Master" AutoEventWireup="true" CodeBehind="selectseat.aspx.cs" Inherits="GroupBooking.Web.SelectSeat" %>

<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %>

<%@ Register TagPrefix="seat" TagName="select" Src="~/SeatSelection.ascx" %> 

<%@ Register Src="../UserControl/flighdetail.ascx" TagName="flightdetail" TagPrefix="fd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../Scripts/overlay.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        Array.prototype.inArray = function (value) {
            // Returns true if the passed value is found in the
            // array. Returns false if it is not.
            var i;
            for (i = 0; i < this.length; i++) {
                if (this[i] == value) {
                    return true;
                }
            }
            return false;
        };

        function calseat(xx) {
            alert("Seat Select");
            var arr = new Array("1A", "2A", "3A", "4A", "5A");
            if (arr.inArray(xx.value)) {
                alert("Your value is found in the Array");
                
            }
            else {
                alert("Your value is not found in the Array");
                xx.value = "";
                xx.focus();
            }
        }

        var change = getParameterByName('change');
        function OnCallbackComplete(s, e) {
            //alert("test");
            var str = e.result;
            if (str != null && str != "" && str != "Your selected seat(s) has been booked by other user") {
                document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = str;
                pcMessage.Show();
                LoadingPanel.Hide();
            } else if (str != null && str != "" && str == "Your selected seat(s) has been booked by other user"){
                LoadingPanel.Hide();
                popupConfirm.Show();
            }
            else {
                if (change != null) {
                    window.location.href = '../public/ProceedPayment.aspx';
                }
                else {
                    window.location.href = '../public/reviewbooking.aspx';
                }
            }
            return;
        }

        function ClosePopup() {
            popupConfirm.Hide(true);
        }

        function getParameterByName(name, url) {
            if (!url) {
                url = window.location.href;
            }
            name = name.replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        }
    </script>

<dx:ASPxValidationSummary ID="vsValidationSummary1" runat="server" Width="502px" ClientInstanceName="validationSummary" HorizontalAlign="Left" Height="16px">
    <ErrorStyle Wrap="False" />
    <Border BorderColor="Red" BorderStyle="Double" />
    <Border BorderColor="Red" BorderStyle="None"></Border>
</dx:ASPxValidationSummary>
    
<asp:ScriptManager ID="ScriptManager" runat="server" AsyncPostBackTimeout="36000"/>
<msg:msgControl ID="msgcontrol" runat="server" />
<input type="hidden" id="hError" runat="server" value="" />
<input type="hidden" id="hID" runat="server" value="" />
<input type="hidden" id="hCarrierCode" runat="server" value="" />
<asp:UpdatePanel ID="UpdatePanel" runat="server" >
    <contenttemplate>


    </contenttemplate>
    
</asp:UpdatePanel>

<asp:UpdateProgress runat="server" ID="UpdateProgress" AssociatedUpdatePanelID="UpdatePanel" DisplayAfter="0" DynamicLayout="true">
        <ProgressTemplate>
            <div class="overlay2"></div>
            <div class="overlayContent2" >
                
                <img alt="In progress..." src="../Images/Airasia/loading_circle.gif" />
                
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:panel Visible = "false"  ID ="pnlErr" runat="server" Height ="20px"  BackImageUrl ="../images/red_header_bg.gif"><asp:Label runat="server" ForeColor="White" ID="lblErr" ></asp:Label></asp:panel>
    
    <div class="">
    <div class="row page-header clearfix">
        <div class="col-sm-4">
            <h4 class="mt-0 mb-5">Pick A Seat</h4>
            Booking/Pick A Seat
        </div>
        <div class="col-sm-8">
            <div align="right" style="padding-top: 9px;" >
                <table id="bookingDetail">
                    <tr>
                        <td>
                            <dx:ASPxButton CssClass="buttonL backBtn"  ID="btn_back" runat="server" Visible="false"
                                Text="Back" AutoPostBack="False" >
                                <ClientSideEvents Click="function(s, e) { assignSeatCallBack.PerformCallback('back');
                                LoadingPanel.SetText(&#39;Please Wait...&#39;);
                                LoadingPanel.Show(); }" />
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton CssClass="buttonL"  ID="btn_Next" runat="server"  
                                Text="Confirm and continue" AutoPostBack="False" >
                                <ClientSideEvents Click="function(s, e) { assignSeatCallBack.PerformCallback();
                                LoadingPanel.SetText(&#39;Please Wait...&#39;);
                                LoadingPanel.Show(); }" />
                            </dx:ASPxButton>
                            <%--<a onclick="return validate(this); preventDoubleClick();" id="aLinkButtonAssignUnit" class="button_3" 
                    href="javascript:LoadingPanel.Show();assignSeatCallBack.PerformCallback()">Confirm and continue</a>--%>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="row page-content clearfix">
        <div class="row">
            <div class="col-sm-12">
                        
            
   <div class="col-sm-12">

                            <div class="col-sm-5" style="padding-top: 20px; display:none">
                                <table id="totalEstimatedFare" class="enhanceFontSize" width="100%" style="padding: 5px;">
                                    <tr class="totalFare">
                                        <td>
                                            <h4>Total Amount</h4>
                                        </td>
                                        <td style="text-align: right">
                                            <%--<h4>0.00</h4>--%>
                                            <dx:ASPxLabel ID="lblTotalAmount" runat="server" Text='0.00' ClientInstanceName="lblTotalFee" />
                                            <dx:ASPxLabel ID="lblCurrency" runat="server" Text='' ClientInstanceName="lblCurrency" />
                                        </td>
                                    </tr>
                                </table>
                                <table class="flightAverageInfo table table-bordered">
                                    <tr>
                                        <td>Going</td>
                                        <td style="text-align: right">
                                            <dx:ASPxLabel ID="lblFeeGoing" runat="server" Text='0.00' ClientInstanceName="lblFeeGoing" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Going - Connecting</td>
                                        <td style="text-align: right">
                                            <dx:ASPxLabel ID="lblFeeGoing2" runat="server" Text='0.00' ClientInstanceName="lblFeeGoing2" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Return</td>
                                        <td style="text-align: right">
                                            <dx:ASPxLabel ID="lblFeeReturn" runat="server" Text='0.00' ClientInstanceName="lblFeeReturn" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Return - Connecting</td>
                                        <td style="text-align: right">
                                            <dx:ASPxLabel ID="lblFeeReturn2" runat="server" Text='0.00' ClientInstanceName="lblFeeReturn2" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                </div>
            <asp:HiddenField ID="hfIsInternational" runat="server" />
        </div>
    <div class="row">
         <div class="col-sm-12">
               <div class="col-sm-12">
        
        </div>
         </div>
      
    </div>
    <div class="row">
         <div class="col-sm-12">
                     <div class=" col-sm-5">
        <div id="pageTitle" xmlns:formatterextension="urn:navitaire:formatters:currency">
        <%--<h1>Pick A Seat</h1>--%>
        <div class="pickASeat">
        <span class="motoAA">
        AirAsia's Pick A Seat gives you greater flexibility to choose your own seats and guarantees that your travel party stays together.
        </span>
        <br/><br/>
        <span class="motoAA">
        Be the first to get the seats that you 	“want”:
        <ul>
            <li>
            Window seats for better view
            </li>
            <li>
            Aisle seats for better access
            </li>
            <li>
            Hot Seats for extra leg room/priority boarding
            </li>
        </ul>
        </span>
        <p style="padding-top:10px;opacity:0.7;"><b>Disclaimer:</b><br/>
        * All selected seats are subjected to Pick A Seat charges
        <br/>
        * <b>Hot Seats (AK, QZ, &amp; FD flights) with extra legroom are only available on certain rows</b></p>
    </div></div>
                <%--<div style="padding: 15px 0;">
                    <span class="dxeBase_GBS" id="RootHolder_SideHolder_lblHeader" style="font-size:Medium;font-weight:normal;">PASSENGER LIST</span><span style="padding-left: 20px;font-weight: 700;font-size: 20px;">EB64VW</span>
                </div>--%>
                
        <asp:Panel ID ="pnlleft" runat ="server"  Width ="185px">
            
    <div id="PassengerSummary" runat="server" class="table-striped inputToFocus">
    </div>
    </asp:Panel>
    <div id="stepsLegend">
    The system has allocated you these seats. If you'd like to change your seat, feel free to pick a seat that best suits your travel preference:
    <br/><br/><b><u>Steps</u></b><br/>
    1. Click “Reselect” to change your pre-allocated seat.<br/>
    2. Click on the seat of your choice represented by the diagram.<br/>
    3. Repeat steps (1) &amp; (2) for guest(s) who are traveling with you.<br/>
    4. Click on the tabs (top left of diagram) to switch between flights and repeat previous steps.<br/>
    5. Click “Remove” to cancel your seat selection and a random seat will be allocated to you during check-in. <a href="javascript:SKYSALES.openNewWindow('http://www.airasia.com/my/en/booking/bookingguides.page', 'PDS', 'width=600,height=460,left=0,top=0,scrollbars=no,resizable=no,menubar=no')">Click here</a> to view our “How to” guide.
    </div>
        </div>
        <div class="col-sm-7 col-lg-5">
            

    <div id="connectedflightcontrol" runat ="server" style="position:absolute ">
    
     <input type="button" runat="server" class ="ConnectedButton"  id="btnSeatDepart1" onserverclick="btnSeatDepart1_ServerClick" /> <br />
    <input type="button" runat="server" class ="ConnectedButton" id="btnSeatDepart2" onserverclick="btnSeatDepart2_ServerClick" /> <br />
    <input type="button" runat="server" class ="ConnectedButton"  id="btnSeatReturn1" onserverclick="btnSeatReturn1_ServerClick" /><br />
    <input type="button" runat="server" class ="ConnectedButton"  id="btnSeatReturn2" onserverclick="btnSeatReturn2_ServerClick" /><br />
    </div>
    <div id="ctlDepart" runat="server" style="display:none">
    <seat:select runat="server" ID="ss"  />
     
    <div >
    
    </div>
    </div>

    <div id="ControlGroupUnitMapView">
    
    <div class="clearAll"></div>
    </div>
    <div id="propertyListBody" class="unitBody" style="margin-left:0;">
        <div class="legendSeatLeft">
            <table class="clearTableHeaders">
                <tbody>
                    <tr>
                        <th colspan="3"><span class="nonStyleHeader" style="margin-left: -10px"><strong>Seat selection key</strong></span></th>
                    </tr>
                    <tr>
                        <td>
                            <img src="../images/AKBase/equipment/JetAircraft_NS_Open_0_AW.gif" class="unitGroupKey"></td>
                        <td>Hot Seats(Row1)<br>
                            <%--<strong id="hotSeatRow1">42.40 MYR</strong>--%></td>
                    </tr>
                    <tr>
                        <td>
                            <img src="../images/AKBase/equipment/JetAircraft_NS_Open_0_AB.gif" class="unitGroupKey"></td>
                        <td>Hot Seats(Row12-14)<br>
                            <%--<strong id="hotSeatRow12_14">42.40 MYR</strong>--%></td>
                    </tr>
                    <tr>
                        <td>
                            <img src="../images/AKBase/equipment/JetAircraft_NS_Open_0_HS.gif" class="unitGroupKey"></td>
                        <td>Hot Seats<br>
                            <%--<strong id="hotSeat">31.80 MYR</strong>--%></td>
                    </tr>
                    <tr>
                        <td>
                            <img src="../images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif" class="unitGroupKey"></td>
                        <td>Standard Seats<br>
                            <%--<strong id="std1">10.60 MYR</strong>--%></td>
                    </tr>
                    <tr>
                        <td>
                            <img src="../images/AKBase/equipment/JetAircraft_NS_Open_0.gif" class="unitGroupKey"></td>
                        <td>Standard Seats<br>
                            <%--<strong id="std2">6.36 MYR</strong>--%></td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="legendSeatRight">
            <table>
                <tbody>
                    <tr>
                        <td>
                            <div id="seatLegendBody">
                                <div id="mainLegend">
                                    <ul>
                                        <li>
                                            <img src="../images/AKBase/equipment/icon_occupied.gif" alt="">Selected</li>
                                        <li>
                                            <img src="../images/AKBase/equipment/JetAircraft_NS_reserved_0.gif" alt="">Occupied</li>
                                    </ul>
                                </div>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>

    

    <div class="clearAll"></div>
    <div class="spacerMed"></div>
    <div id="unitMapContinueButton"><br/><table>
        <tbody><tr>
            <td></td>
            <td>
                <%--<a onclick="return validate(this); preventDoubleClick();" id="aLinkButtonAssignUnit" class="button_3" 
                    href="javascript:__doPostBack('ctl00$ContentPlaceHolder2$LinkButtonAssignUnit','')">Confirm and continue</a>--%>
                
            </td>
        </tr>
        </tbody></table>
        <div class="spacerMed"></div>
        <div></div>
    </div>
    <div style="display:none"><asp:LinkButton ID="LinkButtonAssignUnit" runat="server" ></asp:LinkButton></div>
    <asp:Label ID="result" runat="server" ></asp:Label>
        </div>
         </div>

    </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
        //    $('[class^="aUnit"]').click(function (e) {
        //        alert('1');
        //    });
        //$(document).on("click", '[class^="aUnit"]', function () {
            //alert($('[class^="aUnit"]').length);
           // });
            //$(document).on("click", '[class^="aUnit"]', function (e) {
            //    console.log('clicked');
            //});
        })

    </script>
    <dx:ASPxCallback runat="server" ID="assignSeatCallBack" ClientInstanceName="assignSeatCallBack" OnCallback="assignSeatCallBack_Callback">
        <ClientSideEvents CallbackComplete="OnCallbackComplete" />
    </dx:ASPxCallback>
    <input type="hidden" runat="server" id="hResult" name="hResult" />
    <dx:ASPxPopupControl ID="popupConfirm" runat="server" ClientInstanceName="popupConfirm"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        CloseButtonImage-Url="~/Images/AirAsia/close_button_icon.jpg"
        Modal="true" AllowDragging="true"
        HeaderText="Confirmation" CloseAction="CloseButton"
        Width="250px">
        <CloseButtonImage Url="~/Images/AirAsia/close_button_icon.jpg"></CloseButtonImage>
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <table width="100%">
                    <tr>
                        <td colspan="2">
                            <br />
                            <!-- The cancellation process cannot be undone, please confirm the action -->
                            <b>Your selected seat(s) has been booked by other user</b> <br />Click 'YES' to continue (system will select to random seat) and click 'NO' to reselect seat(s).
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="btnYes" runat="server" Text="YES" Width="50px" AutoPostBack="False" ClientInstanceName="btnYes">
                                            <ClientSideEvents Click="function(s, e) {ClosePopup();assignSeatCallBack.PerformCallback('selectrandom');
                                LoadingPanel.SetText(&#39;Please Wait...&#39;);
                                LoadingPanel.Show(); }"></ClientSideEvents>

                                        </dx:ASPxButton>
                                    </td>
                                    <td>&nbsp; </td>
                                    <td>
                                        <dx:ASPxButton ID="btnNo" runat="server" Text="NO" Width="50px" AutoPostBack="False">
                                            <%--                                            <ClientSideEvents Click="ClosePopup" />--%>
                                            <ClientSideEvents Click="function(){ClosePopup();}"></ClientSideEvents>
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <span id="SeatFee"></span>
    <fd:flightdetail ID="flightdetail" runat="server" />
</asp:Content>
