<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReviewFareChange.aspx.cs" 
    Inherits="GroupBooking.Web.Booking.ReviewFareChange"  MasterPageFile="~/Master/NewPageMasterReport.Master"  %>
<%@ MasterType  virtualPath="~/Master/NewPageMaster.Master"%>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolder2"> 
     <script src="../js/clientclick.js" type="text/javascript"></script>
    <style type="text/css" media="print">
.blankprint { visibility: hidden; display: none }
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
        window.location = "../public/agentmain.aspx";
    }

    // <![CDATA[
    function ShowLoginWindow() {
        LoadingPanel.Hide();
        pcAvgFare.Show();
    }
    function ShowCreateAccountWindow() {
        pcCreateAccount.Show();
        tbUsername.Focus();
    }

    function ConfirmReview(){
        window.location.href = '../public/reviewbookingchange.aspx';
    }

    var demoCounter;

    function demoInit() {
        //demoCounter = 30;
        demoCounter =  parseInt(<%= ctrHdn.Value %>);
        demoUpdate();
    }
    function demoTick() {
        demoCounter -= 1;
        demoUpdate();
    }
    function demoUpdate() {

        if (demoCounter > 0) {
            tmrCount.SetEnabled(true);
            document.getElementById("demoCountdown").innerHTML = demoCounter;
            document.getElementById("countDownBtm").innerHTML = demoCounter;
        }
        else {
            tmrCount.SetEnabled(false);
            window.location = "../public/searchflight.aspx";
        }
    }

    function nextPage() {
        window.location.href = "../public/reviewbooking.aspx"
    }
    // ]]> 
    </script>
    <dx:ASPxTimer ID="tmrCount" runat="server" Interval="1000" 
        ClientInstanceName="tmrCount">
                    <ClientSideEvents Tick="function(s, e) { demoTick(); }" />
                </dx:ASPxTimer>
      <dx:ASPxCallback ID="Callback" runat="server" ClientInstanceName="Callback">
        <ClientSideEvents CallbackComplete="function(s, e) { LoadingPanel.Hide(); }" />
    </dx:ASPxCallback>

    <table cellpadding="0" cellspacing="0" width="100%" style="font-family:Arial,Helvetica,san-serif;">
        <tr>            
            <td class="tdright">
                <div class="div">
                    <h2>Review Flights</h2></div>
                <hr />
                <table class="tableright" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="center">
                        <span class="errorHighlight" runat="server" id="sErrmsg"></span>
                            <table width="100%">
                                <tr class="tdcol">
                                    <td colspan="4" align="left">                                    
                                    <h2>Flight Details</h2>
                                    </td>                                    
                                </tr>
                                <tr>
                                     <td style="width:50%" colspan="4">
                                     <table width="100%" border="0" bgcolor="#f7f3f7">
                                        <tr style="border-top: solid 1px #C4C4C4;border-bottom: solid 1px #C4C4C4;vertical-align:middle;height:30px;background-color:#333333;color:White;font-size:small;">
                                            <td style="height: 30px;">
                                                &nbsp;
                                            </td>
                                            <td style="height: 30px;">
                                                &nbsp;
                                            </td>
                                            <td style="border-left: solid 1px #C4C4C4; height: 30px;" align="center">
                                                <asp:Label ID="lblDepart" runat="server"  Text="Departure" Font-Bold="true"></asp:Label>
                                            </td>
                                            <td style="border-left: solid 1px #C4C4C4; height: 30px;" align="center">
                                                <asp:Label ID="lblArriv" runat="server"  Text="Arrival" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="td_Depart" runat="server" align="left">
                                            <td width="100px">
                                               &nbsp;&nbsp;<asp:Label ID="LblGoing" runat="server"  Text="Origin :"></asp:Label>
                                            </td>
                                            <td>
                                               <div>
                                            &nbsp;</div>
                                        <div >
                                            <b>&nbsp;&nbsp;<asp:Label ID="lbl_CarrierCodeOut" runat="server" ></asp:Label>&nbsp;&nbsp;&nbsp;<asp:Label ID="lbl_FlightnumberOut" runat="server" ></asp:Label> </b> &nbsp;&nbsp;&nbsp; <asp:Label ID="lbl_Departureout" runat="server" ></asp:Label> - <asp:Label ID="lbl_ArrivalOut" runat="server" ></asp:Label>
                                            <br />
                                            
                                                </div>                                        
                                        <div >
                                            &nbsp;&nbsp;<asp:Label ID="LblTransitDepart" runat="server" Text="" ForeColor="Red"/></div>
                                            </td>
                                            <td style="border-left: solid 1px #C4C4C4;" align="center">
                                                <div>&nbsp;&nbsp;<asp:Label ID="lbl_DepartureDateout" runat="server" ></asp:Label></div>
                                            </td>
                                            <td style="border-left: solid 1px #C4C4C4;" align="center">
                                                <div>&nbsp;&nbsp;<asp:Label ID="lbl_ArrivalDateout" runat="server" ></asp:Label></div>                                                                                 
                                            </td>
                                        </tr>
                                        <tr id="td_Depart2" runat="server" align="left">
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;&nbsp;<asp:Label ID="lblDateDepart" runat="server" ></asp:Label>
                                            </td>
                                            <td style="border-left: solid 1px #C4C4C4;">
                                                &nbsp;
                                            </td>
                                            <td style="border-left: solid 1px #C4C4C4;">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr id="td_Depart3" runat="server" align="left">
                                            <td width="100px">
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                            <td style="border-left: solid 1px #C4C4C4;">
                                            &nbsp;
                                            </td>
                                            <td style="border-left: solid 1px #C4C4C4;">
                                            &nbsp;
                                            </td>
                                        </tr>
                                        <tr id="td_Return" runat="server" align="left">
                                            <td>
                                                &nbsp;&nbsp;<asp:Label ID="LblReturn" runat="server"  Text="Destination :"></asp:Label>
                                            </td>
                                            <td>
                                             <div>
                                            &nbsp;</div>
                                        <div >
                                            <b>&nbsp;&nbsp;<asp:Label ID="lbl_CarrierCodeIN" runat="server" ></asp:Label>&nbsp;&nbsp;&nbsp;<asp:Label ID="lbl_FlightnumberIN" runat="server" ></asp:Label></b> &nbsp;&nbsp;&nbsp;<asp:Label ID="lbl_DepartureIN" runat="server" ></asp:Label> - <asp:Label ID="lbl_ArrivalIN" runat="server" ></asp:Label>
                                            <br />                                             
                                             </div>
                                             <div >
                                            &nbsp;&nbsp;<asp:Label ID="LblTransitReturn" runat="server" Text="" ForeColor="Red"/></div>
                                            </td>
                                            <td style="border-left: solid 1px #C4C4C4;" align="center">
                                                <div >
                                                &nbsp;&nbsp;<asp:Label ID="lbl_DepartureDateIN" runat="server" ></asp:Label></div>
                                            </td>
                                            <td style="border-left: solid 1px #C4C4C4;" align="center"><div>
                                                &nbsp;&nbsp;<asp:Label ID="lbl_ArrivalDateIN" runat="server" ></asp:Label></div>                                        
                                            </td>                                                                                
                                        </tr>     
                                        <tr align="left">
                                            <td>
                                            &nbsp;&nbsp;
                                            </td>
                                            <td>
                                                &nbsp;&nbsp;<asp:Label ID="lblDateReturn" runat="server" ></asp:Label>                                               
                                            </td>
                                            <td style="border-left: solid 1px #C4C4C4;">
                                            &nbsp;&nbsp;
                                            </td>
                                            <td style="border-left: solid 1px #C4C4C4;">
                                            &nbsp;&nbsp;
                                            </td>
                                        </tr>                                                                           
                                        <tr id="Tr1" runat="server" align="left">
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td style="border-left: solid 1px #C4C4C4;">&nbsp;</td>
                                            <td style="border-left: solid 1px #C4C4C4;">&nbsp;</td>
                                        </tr>                                                                                
                                     </table>                                                            
                                    </td>                                   
                                </tr>
                                <tr class="tdcol">
                                    <td colspan="4" align="left">
                                        <br />
                                        <h2>Pax Details</h2>
                                    </td>
                                </tr>
                                <tr bgcolor="#f7f3f7" style="background-color:#333333;color:White;height:30px">
                                    <td colspan = "4" >
                                     <div >&nbsp;</div>
                                    </td>
                                </tr>
                                <tr bgcolor="#f7f3f7" >
                                    <td colspan = "4" >
                                    &nbsp;
                                    </td>
                                </tr>
                                <tr id="tr_Depart" runat="server" bgcolor="#f7f3f7" align="left">
                                    <td class="dyncontent">                                         
                                        <div>
                                            &nbsp;&nbsp;<asp:Label ID="lblAdultout" runat="server"  Text="Adult Number :"></asp:Label>&nbsp;&nbsp;&nbsp; <asp:Label ID="lbl_GuestNumout" runat="server" ></asp:Label></div>                                        
                                    </td>
                                    <td colspan="2" class="dyncontent">
                                        <div><asp:Label ID="lblChildout" runat="server"  Text="Child Number :"></asp:Label>&nbsp;&nbsp;&nbsp; <asp:Label ID="lbl_ChildNumout" runat="server" ></asp:Label></div>                                        
                                    </td>
                                    <td class="dyncontent">
                                        <div >
                                            <asp:Label ID="lblTotalOut" runat="server"  Text="Total Pax :"></asp:Label>&nbsp;&nbsp;&nbsp; <asp:Label ID="lbl_PaxNumout" runat="server"></asp:Label></div>                                        
                                    </td>
                                </tr>
                                <tr bgcolor="#f7f3f7">
                                    <td colspan = "4">
                                     <div>&nbsp;</div>
                                    </td>
                                </tr>
                                <tr id="tr_Return" runat="server" style="display:none" bgcolor="#f7f3f7" align="left">                                    
                                    <td>
                                        <div>
                                            &nbsp;</div>                                        
                                            <div>
                                            &nbsp;&nbsp;<asp:Label ID="lblAdultIn" runat="server"  Text="Adult Number :"></asp:Label>&nbsp;&nbsp;&nbsp; <asp:Label ID="lbl_GuestNumIN" runat="server" ></asp:Label></div>                                        
                                    </td>
                                    <td colspan = "2" style="border-left: solid 1px #C4C4C4;">
                                        <div>
                                            &nbsp;&nbsp;<asp:Label ID="lblChildIn" runat="server"  Text="Child Number :"></asp:Label>&nbsp;&nbsp;&nbsp; <asp:Label ID="lbl_ChildNumIN" runat="server" ></asp:Label></div>
                                        
                                    </td>
                                    <td style="border-left: solid 1px #C4C4C4;">
                                        <div>
                                            &nbsp;&nbsp;<asp:Label ID="lblTotalIn" runat="server"  Text="Total Pax :"></asp:Label>&nbsp;&nbsp;&nbsp; <asp:Label ID="lbl_PaxNumIN" runat="server" ></asp:Label></div>                                        
                                    </td>

                                </tr>                                                                
                            </table>
                        </td>                        
                    </tr>                                      
                    <tr class="tr">
                        <td align="center">
                            <table width="90%">
                                <tr>
                                    <td align="right">
                                    <br />
                                        <%-- <asp:Button ID="btn_Next" runat="server" Text="Continue" CssClass="button" OnClick="btn_Next_Click" />--%>
                                        
                                        
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                        
                        </td>
                    </tr>                            
            </table>
            
            
            
            
         </td>
       </tr>
       <tr>
        <td align="right">
            
        </td>
       </tr>
    </table> 
     <div <%--style="position: absolute;left: 950px;left:310px;width: 119px; top: 470px;"--%>>
    <dx:ASPxButton CssClass="buttonL" ID="btShowModal" runat="server"  
                            Text="Continue" AutoPostBack="False" >
                            <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); ShowLoginWindow(); demoInit(); }" />                                            
                        </dx:ASPxButton>
   </div>
    <dx:aspxpopupcontrol ID="pcAvgFare" runat="server" CloseAction="CloseButton" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
        ClientInstanceName="pcAvgFare" CloseButtonImage-Url="~/Images/AirAsia/close_button_icon.jpg"
        AllowDragging="True" EnableAnimation="False"
        EnableViewState="False" HeaderText="" ShowHeader="False" >       
        <HeaderStyle ForeColor="#CC0000" />
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <dx:ASPxPanel ID="pnAvgFare" runat="server" DefaultButton="btOK">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <table class="blankprint" style="BORDER-COLLAPSE: collapse; font-family:Arial,Helvetica,san-serif;" cellspacing="0" cellpadding="3" align="center" border="0" width = "500px">
                                <tr>
                                    <td colspan="2">
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        
                                    </td>
                                </tr>
                                <tr class="tdcol" bgcolor="#f7f3f7" valign="middle">
                                    <td colspan = "2" align="center">
                                        <table width = "500px" style="background-color:#333333;color:White;">
                                            <tr>
                                                <td width="20%">
                                                    &nbsp;
                                                </td>                                                
                                                <td align="center" width="60%" colspan="3">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Average Fare Per Pax:
                                                </td>
                                                <td align="right" width="20%">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="javascript:closepopup();"><img src="../Images/Airasia/close_button_larger_icon.jpg" alt="" align="right" /></a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                   &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="20%">
                                                    &nbsp;
                                                </td>                                                
                                                <td align="right" width="30%">
                                                    <dx:ASPxLabel ID="lblAverageFare" runat="server" 
                                             style="font-size:27px;font-family:Arial,Helvetica,Sans-Serif;" ></dx:ASPxLabel>         
                                             </td>                                                                                                               
                                                <td align="left" width="15%">
                                                    &nbsp;<dx:ASPxLabel ID="lblTotalCurrency" runat="server"
                                            style="font-size:27px;font-family:Arial,Helvetica,Sans-Serif;" ></dx:ASPxLabel> 
                                                </td>
                                                <td align="left" width="10%">
                                                    <div style='text-align:left;'><a id="displayText" href="javascript:toggle();" style="text-decoration:none;font-weight: bold;color:White;font-size:medium">+ </a></div>
                                                </td>                                                                                                                                        
                                                <td align="right" width="25%">
                                                    <div id="Div1" runat="server" style="text-align:right"><span id="demoCountdown" style="font-weight: bold; color: #59A1E9;text-decoration:blink;" >30
                                        </span>&nbsp;seconds</div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <%-- <div style='text-align:right;'><a id="displayText" href="javascript:toggle();" style="text-decoration:none;font-weight: bold;color:White;font-size:medium">+ </a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                   
                                <tr bgcolor="#f7f3f7" class="tdcol">
                                    <td align="right" colspan="2">

                                       </td>
                                </tr>
                                <tr bgcolor="#f7f3f7">
                                    <td>
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr bgcolor="#f7f3f7">
                                    <td align="center">
                                        <dx:ASPxLabel ID="lblDepartFareText" runat="server" Text="Depart :" style=" color:#000000;font-family:Arial,Helvetica,Sans-Serif;">
                                        </dx:ASPxLabel>
                                        <br />
                                        <table>
                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="lblDepartFare" runat="server" style=" color:#000000;font-family:Arial,Helvetica,Sans-Serif;">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxLabel ID="lblDepartCurrency" runat="server" style=" color:#000000;font-family:Arial,Helvetica,Sans-Serif;">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="center">
                                        <dx:ASPxLabel ID="lblReturnFareText" runat="server" Text="Return :" style=" color:#000000;font-family:Arial,Helvetica,Sans-Serif;">
                                        </dx:ASPxLabel>
                                        <br />
                                        <table>
                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="lblReturnFare" runat="server" style=" color:#000000;font-family:Arial,Helvetica,Sans-Serif;">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxLabel ID="lblReturnCurrency" runat="server" style=" color:#000000;font-family:Arial,Helvetica,Sans-Serif;">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr  bgcolor="#f7f3f7">
                                    <td colspan = "2" align = "center">
                                    &nbsp;
                                    </td>
                                </tr>
                                    
                                <tr  bgcolor="#f7f3f7">
                                    <td colspan = "2" align = "left">
                                    <div id="toggleText" style="display: none"> <%-- style="display: none"--%>          
                                        <table width="100%">
    <%-- <tr>
        <td colspan="2" align="center">
             <dx:ASPxLabel ID="lblTextDepart" runat="server" Text="Depart :" ForeColor="Black"></dx:ASPxLabel>
        </td>    
         <td colspan="2" align="center">
             &nbsp;&nbsp;<dx:ASPxLabel ID="lblTextReturn" runat="server" Text="Return :" ForeColor="Black"></dx:ASPxLabel>   
        </td>        
    </tr>--%>
    <tr>        
        <td style="width:30%;" align="right">
            <dx:ASPxLabel ID="lblTextPaxFareDepart" runat="server" Text="Pax Fare : " ForeColor="Black" style="font-family:Arial,Helvetica,Sans-Serif;"></dx:ASPxLabel>
        </td>        
        <td style="width:20%;">
            <dx:ASPxLabel ID="lblPaxFareDepart" runat="server" Text="0" ForeColor="Black" style="font-family:Arial,Helvetica,Sans-Serif;"></dx:ASPxLabel>
        </td>
        <td style="width:1%;">
            &nbsp;
        </td>
         <td style="width:30%;" align="right">
            <dx:ASPxLabel ID="lblTextPaxFareReturn" runat="server" Text="Pax Fare : " ForeColor="Black" style="font-family:Arial,Helvetica,Sans-Serif;"></dx:ASPxLabel>
        </td>        
        <td style="width:19%;">
            <dx:ASPxLabel ID="lblPaxFareReturn" runat="server" Text="0" ForeColor="Black" style="font-family:Arial,Helvetica,Sans-Serif;"></dx:ASPxLabel>
        </td>
    </tr>
    <tr>
        <td style="width:30%;" align="right">             
             <dx:ASPxLabel ID="lblTextTaxFareDepart" runat="server" Text="Airport Tax : " ForeColor="Black" style="font-family:Arial,Helvetica,Sans-Serif;"></dx:ASPxLabel>
        </td>        
        <td style="width:20%;">
            <dx:ASPxLabel ID="lblTaxDepart" runat="server" Text="0" ForeColor="Black" style="font-family:Arial,Helvetica,Sans-Serif;"></dx:ASPxLabel>
        </td >
        <td style="width:1%;">
            &nbsp;
        </td>
         <td style="width:30%;" align="right">
             <dx:ASPxLabel ID="lblTextTaxFareReturn" runat="server" Text="Airport Tax : " ForeColor="Black" style="font-family:Arial,Helvetica,Sans-Serif;"></dx:ASPxLabel>
        </td>        
        <td style="width:19%;">
            <dx:ASPxLabel ID="lblTaxReturn" runat="server" Text="0" ForeColor="Black" style="font-family:Arial,Helvetica,Sans-Serif;"></dx:ASPxLabel>
        </td>
    </tr>
    <tr runat="server" id="trChildTax"  visible="false">
        <td style="width:30%;" align="right">             
            <dx:ASPxLabel ID="lblTextTaxFareDepartChild" runat="server" Text="Airport Tax (Child) : " ForeColor="Black" style="font-family:Arial,Helvetica,Sans-Serif;"></dx:ASPxLabel>
        </td>        
        <td style="width:20%;">
            <dx:ASPxLabel ID="lblTaxDepartChild" runat="server" Text="0" ForeColor="Black" style="font-family:Arial,Helvetica,Sans-Serif;"></dx:ASPxLabel>
        </td >
        <td style="width:1%;">
            &nbsp;
        </td>
         <td style="width:30%;" align="right">
             <dx:ASPxLabel ID="lblTextTaxFareReturnChild" runat="server" Text="Airport Tax (Child) : " ForeColor="Black" style="font-family:Arial,Helvetica,Sans-Serif;"></dx:ASPxLabel>
        </td>        
        <td style="width:19%;">
            <dx:ASPxLabel ID="lblTaxReturnChild" runat="server" Text="0" ForeColor="Black" style="font-family:Arial,Helvetica,Sans-Serif;"></dx:ASPxLabel>
        </td>
    </tr>
    <tr>
        <td style="width:30%;" align="right">             
            <dx:ASPxLabel ID="lblTextFuelDepart" runat="server" Text="Fuel Fee : " ForeColor="Black" style="font-family:Arial,Helvetica,Sans-Serif;"></dx:ASPxLabel>
        </td>        
        <td style="width:20%;">
            <dx:ASPxLabel ID="lblFuelDepart" runat="server" Text="0" ForeColor="Black" style="font-family:Arial,Helvetica,Sans-Serif;"></dx:ASPxLabel>
        </td>
        <td style="width:1%;">
            &nbsp;
        </td>
         <td style="width:30%;" align="right">
            <dx:ASPxLabel ID="lblTextFuelReturn" runat="server" Text="Fuel Fee : " ForeColor="Black" style="font-family:Arial,Helvetica,Sans-Serif;"></dx:ASPxLabel>
        </td>        
        <td style="width:19%;">
            <dx:ASPxLabel ID="lblFuelReturn" runat="server" Text="0" ForeColor="Black" style="font-family:Arial,Helvetica,Sans-Serif;"></dx:ASPxLabel>
        </td>
    </tr>
    <tr>
        <td style="width:30%;" align="right">             
           <dx:ASPxLabel ID="lblTextSvcDepart" runat="server" Text="Service Charge : " ForeColor="Black" style="font-family:Arial,Helvetica,Sans-Serif;"></dx:ASPxLabel>
        </td>        
        <td style="width:20%;">
            <dx:ASPxLabel ID="lblSvcDepart" runat="server" Text="0" ForeColor="Black" style="font-family:Arial,Helvetica,Sans-Serif;"></dx:ASPxLabel>
        </td>
        <td style="width:1%;">
           &nbsp;
        </td>
         <td style="width:30%;" align="right">
            <dx:ASPxLabel ID="lblTextSvcReturn" runat="server" Text="Service Charge : " ForeColor="Black" style="font-family:Arial,Helvetica,Sans-Serif;"></dx:ASPxLabel>
        </td>        
        <td style="width:19%;">
            <dx:ASPxLabel ID="lblSvcReturn" runat="server" Text="0" ForeColor="Black" style="font-family:Arial,Helvetica,Sans-Serif;"></dx:ASPxLabel>
        </td>
    </tr>
    <tr id="trVAT" runat="server" style="display:none">
        <td style="width:30%;" align="right">
            <dx:ASPxLabel ID="lblTextVATDepart" runat="server" Text="VAT : " ForeColor="Black" style="font-family:Arial,Helvetica,Sans-Serif;"></dx:ASPxLabel>
        </td>
        <td style="width:20%;">
            <dx:ASPxLabel ID="lblVATDepart" runat="server" Text="0" ForeColor="Black" style="font-family:Arial,Helvetica,Sans-Serif;"></dx:ASPxLabel>
        </td>
        <td style="width:1%;">
           &nbsp;
        </td>
        <td style="width:30%;" align="right">
            <dx:ASPxLabel ID="lblTextVATReturn" runat="server" Text="VAT : " ForeColor="Black" style="font-family:Arial,Helvetica,Sans-Serif;"></dx:ASPxLabel>
        </td>
        <td style="width:19%;">
            <dx:ASPxLabel ID="lblVATReturn" runat="server" Text="0" ForeColor="Black" style="font-family:Arial,Helvetica,Sans-Serif;"></dx:ASPxLabel>
        </td>
    </tr>
    <tr>
        <td style="width:30%;" align="right">             
           <dx:ASPxLabel ID="lblTextOthDepart" runat="server" Text="Other Fee : " ForeColor="Black" style="font-family:Arial,Helvetica,Sans-Serif;"></dx:ASPxLabel>
        </td>        
        <td style="width:20%;">
            <dx:ASPxLabel ID="lblOthDepart" runat="server" Text="0" ForeColor="Black" style="font-family:Arial,Helvetica,Sans-Serif;"></dx:ASPxLabel>
        </td>
        <td style="width:1%;">
            &nbsp;
        </td>
         <td style="width:30%;" align="right">
             <dx:ASPxLabel ID="lblTextOthReturn" runat="server" Text="Other Fee : " ForeColor="Black" style="font-family:Arial,Helvetica,Sans-Serif;"></dx:ASPxLabel>
        </td>        
        <td style="width:19%;">
            <dx:ASPxLabel ID="lblOthReturn" runat="server" Text="0" ForeColor="Black" style="font-family:Arial,Helvetica,Sans-Serif;"></dx:ASPxLabel>
        </td>
    </tr>    
</table></div></td>
                                </tr>
                                <tr  bgcolor="#f7f3f7">
                                    <td colspan = "2" align ="center">
                                        &nbsp;</td>
                                </tr>
                                <tr  bgcolor="#f7f3f7">
                                    <td colspan = "2" align ="center">
                                        &nbsp; Total Average Fare :
                                        <dx:ASPxLabel ID="lblTotalFare" runat="server" style="font-size:27px;font-family:Arial,Helvetica,Sans-Serif;">
                                        </dx:ASPxLabel>
                                        <dx:ASPxLabel ID="lblTotFareCurrency" runat="server" style="font-size:27px;font-family:Arial,Helvetica,Sans-Serif;">
                                        </dx:ASPxLabel>
                                 </td>
                                 </tr>
                                <tr  bgcolor="#f7f3f7">
                                    <td colspan = "2" align ="center">
                                        &nbsp;</td>
                                </tr>
                                <tr  bgcolor="#f7f3f7">
                                    <td colspan = "2" align ="center">
                                        &nbsp;</td>
                                </tr>
                                <tr bgcolor="#f7f3f7">
                                    <td align="center" colspan="2">
                                        Total Pax :
                                        <dx:ASPxLabel ID="lblTotPax" runat="server" style="font-size:27px;font-family:Arial,Helvetica,Sans-Serif;">
                                        </dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr bgcolor="#f7f3f7">
                                    <td align="center" colspan="2">
                                        Detail Pax :
                                        <dx:ASPxLabel ID="lblDetailPax" runat="server" style="font-size:27px;font-family:Arial,Helvetica,Sans-Serif;">
                                        </dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr bgcolor="#f7f3f7">
                                    <td colspan = "2" align="center">
                                        &nbsp;</td>
                                </tr>
                                <tr  bgcolor="#f7f3f7">
                                    <td colspan = "2" align ="center">
                                        &nbsp;</td>
                                </tr>
                                <tr bgcolor="#f7f3f7">
                                    <td align="center" colspan="2"> 
                                        <dx:ASPxButton CssClass="button-3"  ID="btOK" 
                                            runat="server" Text="Confirm" AutoPostBack="false">
                                             
                                            <ClientSideEvents Click="function(s, e) {LoadingPanel.SetText(&#39;Please Wait...&#39;);LoadingPanel.Show(); Callback.PerformCallback(); pcAvgFare.Hide();ConfirmReview();
                                            }"></ClientSideEvents>
                                        </dx:ASPxButton>
                            <asp:HiddenField ID="ctrHdn" runat="server" />
                                    </td>
                                </tr>                                
                                <tr bgcolor="#f7f3f7">
                                    <td colspan="2" align="center">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr bgcolor="#f7f3f7">
                                    <td colspan="2" align="center">
                                        
                                         <span style="font-size:larger;">You have<b>
                                        <span id="countDownBtm" style="font-weight: bold; color: #59A1E9;text-decoration:blink" >30
                                        </span>
                                        </b>&nbsp;seconds to stay at this page </span>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                    </td>
                                </tr>
                            </table>
                             
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>                
            </dx:PopupControlContentControl>
        </ContentCollection>        
<CloseButtonImage Url="~/Images/AirAsia/close_button_icon.jpg"></CloseButtonImage>

        <ContentStyle>
        <Paddings PaddingBottom="5px" />
        <Paddings PaddingBottom="5px"></Paddings>
        </ContentStyle>
    </dx:aspxpopupcontrol>
     <script  language="javascript" type="text/javascript">
</script>
</asp:Content>