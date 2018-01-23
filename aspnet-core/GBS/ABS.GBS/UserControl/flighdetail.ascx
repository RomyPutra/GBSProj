
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="flighdetail.ascx.cs" Inherits="GroupBooking.Web.UserControl.flightdetail" %>

<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %>
<%@ Register Src="../UserControl/BookingBreakdown.ascx" TagName="bookingbreakdown" TagPrefix="bb" %>

<!-- temp remarked by diana 20170213, to prevent disabled textbox -->
<%--<script src="../js/clientclick.js" type="text/javascript"></script>
    <style type="text/css" media="print">
.blankprint { visibility: hidden; display: none }
</style>--%>

<div id="flightDetailWrapper">
    <!-- The header to show the depart and return -->
    <div id="flightRoute">
        <span id="flightDepartReturn"><%--Kuala Lumpur (KUL) to Singapore (SIN)--%>
            <asp:Label ID="lblAgentName" runat="server"></asp:Label>
            <br />
            <asp:Label ID="lblAgentOrg" runat="server"></asp:Label>
        </span>
    </div>

    <!-- The trip total cost-->
    <%--<div id="totalPricing">
        <div>
            <div id="labelCurrency"><span>TOTAL </span><span id="currencyType">MYR</span></div>
            <div id="totalAmtWrapper">6,600.00</div>
        </div>
        </div>
    </div>--%>
    
    <%-- 20170529 - Sienny (Show info amount due and available credit) --%>
    <div style="padding-bottom:20px">
        <bb:bookingbreakdown ID="bookingbreakdown" runat="server" />
    </div>

    <!-- Wrapper for Depart , Connecting, and Return -->
    <div id="departReturnWrapper">

        <div id="mainDepartConnectWrapper">
            <div class="" style="display:inline-block">
                    <span class="departFlight fa fa-plane "></span>
                </div>
 <div class="flightTitleDepartReturn" style="display:inline-block">DEPART</div>
            <div id="departWrapper" class="flightWrapper">
                <div class="flightNo">
                    <span><%--AK117--%>
                        <asp:Label ID="lbl_CarrierCodeOut" runat="server" ></asp:Label><asp:Label ID="lbl_FlightnumberOut" runat="server" ></asp:Label>
                    </span>
                </div>

                <div id="depart1To" class="blockFlight"><%--CAN--%>
                    <asp:Label ID="lbl_DepartureOut" runat="server" ></asp:Label>
                </div>

                <div class="arrowToWrapper">
                    <span class="departFlight fa fa-plane "></span>
                </div>

                <div id="return1To" class="blockFlight"><%--KUL--%>
                    <asp:Label ID="lbl_ArrivalOut" runat="server" ></asp:Label> 
                </div>

                <div class="detailDateFlight">
                    <div class="classDate"><%--Mon,30 Jan 2017 2125--%>
                        <asp:Label ID="lbl_DepartureDateOut" runat="server" ></asp:Label>
                    </div>
                    <div class="classDate"><%--Mon,30 Jan 2017 2240--%>
                        <asp:Label ID="lbl_ArrivalDateOut" runat="server" ></asp:Label>
                    </div> 
                </div>
            </div>
 
            <div id="connectWrapperDepart" class="flightWrapper" runat="server">
                <div class="flightNo">
                    <span><%--AK367--%>
                        <asp:Label ID="lbl_CarrierCodeOut2" runat="server" ></asp:Label><asp:Label ID="lbl_FlightnumberOut2" runat="server" ></asp:Label>
                    </span>
                </div>

                <div id="connectDepart1To" class="blockFlight"><%--KUL--%>
                    <asp:Label ID="lbl_DepartureOut2" runat="server" ></asp:Label>
                </div>

                <div class="arrowToWrapper">
                    <span class="departFlight fa fa-plane "></span>
                </div>

                <div id="Div1" class="blockFlight"><%--DPS--%>
                    <asp:Label ID="lbl_ArrivalOut2" runat="server" ></asp:Label> 
                </div>

                <div class="detailDateFlight">
                    <div class="classDate"><%--Mon,30 Jan 2017 2300--%>
                        <asp:Label ID="lbl_DepartureDateOut2" runat="server" ></asp:Label>
                    </div>
                    <div class="classDate"><%--Tue,31 Jan 2017 0100--%>
                        <asp:Label ID="lbl_ArrivalDateOut2" runat="server" ></asp:Label>
                    </div> 
                </div>   
            </div>
        </div>
 
        <div id="mainReturnConnectWrapper" runat="server">
            <div class="" style="display:inline-block">
                    <span class="returnFlight fa fa-plane "></span>
                </div><div class="flightTitleDepartReturn" style="display:inline-block">RETURN</div>
 
            <div id="returnWrapper" class="flightWrapper">			
                <div class="flightNo">
				    <span><%--AK767--%>
                        <asp:Label ID="lbl_CarrierCodeIN" runat="server" ></asp:Label><asp:Label ID="lbl_FlightnumberIN" runat="server" ></asp:Label>
				    </span>
                </div>
			
                <div id="depart2To" class="blockFlight"><%--DPS--%>
                    <asp:Label ID="lbl_DepartureIN" runat="server" ></asp:Label> 
			    </div>
			
                <div class="arrowToWrapper"><span class="returnFlight fa fa-plane "></span></div>
            
			    <div id="return2To" class="blockFlight"><%--KUL--%>
                    <asp:Label ID="lbl_ArrivalIN" runat="server" ></asp:Label>
			    </div>
			
                <div class="detailDateFlight">
				    <div class="classDate"><%--Tue,31 Jan 2017 0130--%>
                        <asp:Label ID="lbl_DepartureDateIN" runat="server" ></asp:Label>
				    </div>
				    <div class="classDate"><%--Tue,31 Jan 2017 0400--%>
                        <asp:Label ID="lbl_ArrivalDateIN" runat="server" ></asp:Label>
				    </div> 
				</div>
            </div>
 
            <div id="connectWrapperReturn" class="flightWrapper" runat="server">
				<div class="flightNo">
					<span><%--AK769--%>
                        <asp:Label ID="lbl_CarrierCodeIN2" runat="server" ></asp:Label><asp:Label ID="lbl_FlightnumberIN2" runat="server" ></asp:Label>
					</span>
				</div>
				
				<div id="connectDepart2To" class="blockFlight"><%--KUL--%>
                    <asp:Label ID="lbl_DepartureIN2" runat="server" ></asp:Label> 
				</div>
				
				<div class="arrowToWrapper"><span class="returnFlight fa fa-plane "></span></div>
				
				<div id="Div4" class="blockFlight"><%--CAN--%>
                    <asp:Label ID="lbl_ArrivalIN2" runat="server" ></asp:Label> 
				</div>
				
				<div class="detailDateFlight">
					<div class="classDate"><%--Tue,31 Jan 2017 0430--%>
                        <asp:Label ID="lbl_DepartureDateIN2" runat="server" ></asp:Label>
					</div>
					<div class="classDate"><%--Tue,31 Jan 2017 0700--%>
                        <asp:Label ID="lbl_ArrivalDateIN2" runat="server" ></asp:Label>
					</div> 
				</div>   
            </div>
        </div>

        <div id="promoCodeFlight" class="blockPromoCode" runat="server" style="display:none;">
            <span>
                <asp:Label ID="lbl_PromoCode" runat="server"></asp:Label>
            </span>
        </div>
    </div>
</div>

<%--<div class="formWrapperLeftAA" style="max-height:1200px;display:none">   
    <div class="sidebarTitle" style="padding: 15px 0;">
        <span class="dxeBase_GBS" id="RootHolder_SideHolder_lblHeader" style="font-size:Medium;font-weight:normal;">
            <div class="width100"> Flight Detail </div>      
        </span>
    </div>
    <!-- Sidebar info for addOn -->
    <div style="" class="secondAlignRight">

        <table id="departureLeft" width="100%">
            <tbody>
                <tr>
                    <td style="padding-bottom: 3px;" valign="top">
                        <div class="" style="text-align:left;"><span class="departFlight fa fa-plane"></span></div>
                    </td>
                    <td>
                        <div class="" style="text-align:right;">
                            <span class="biggerSize" id="Span7">
                                <asp:Label ID="lbl_CarrierCodeOut" runat="server" ></asp:Label>&nbsp;&nbsp;&nbsp;<asp:Label ID="lbl_FlightnumberOut" runat="server" ></asp:Label>
                            </span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 3px;" valign="top"> 
                        <label font-size="Small">
                            <b>
                            <asp:Label ID="lbl_Departureout" runat="server" ></asp:Label>
                            </b>
                        </label>
                    </td>
                    <td style="width: 332px; padding:0 0 10px 20px;" valign="top">
                        <div id="RootHolder_SideHolder_Label17" style="float:right;">
                            <div>                                                
                                <asp:Label ID="lbl_DepartureDateOut" runat="server" ></asp:Label>
                            </div> 
                        </div>
                    </td>
                </tr>      
                <tr>
                    <td style="padding-bottom: 3px;" valign="top"> 
                        <label id="RootHolder_SideHolder_ASPxLabel1" font-size="Small">
                            <b>
                                <asp:Label ID="lbl_ArrivalOut" runat="server" ></asp:Label> 
                            </b>
                        </label>
                    </td>
                    <td style="width: 332px; padding:0 0 10px 20px;">
                            <div id="RootHolder_SideHolder_Div10" style="float:right;">
                                <div>                                                 
                                    <asp:Label ID="lbl_ArrivalDateout" runat="server" ></asp:Label>
                                </div> 
                        </div>
                    </td>
                </tr>

                <tr runat="server" id="trReturnIcon">
                    <td style="padding-bottom: 3px;" valign="top">
                        <div class="" style="text-align:left;"><span class="returnFlight fa fa-plane"></span></div>
                    </td>
                    <td>
                        <div class="" style="text-align:right;">
                            <span class="biggerSize" id="Span7">
                                <asp:Label ID="lbl_CarrierCodeIN" runat="server" ></asp:Label>&nbsp;&nbsp;&nbsp;<asp:Label ID="lbl_FlightnumberIN" runat="server" ></asp:Label>
                            </span>
                        </div>
                    </td>
                </tr>
                            <br /> 

                <tr id="tr_Return" runat="server">
                    <td style="padding-bottom: 3px;" valign="top"> 
                        <label font-size="Small">
                            <b>
                                <asp:Label ID="lbl_DepartureIN" runat="server" ></asp:Label> 
                            </b>
                        </label>
                    </td>
                    <td style="width: 332px; padding:0 0 10px 20px;" valign="top">
                        <div id="RootHolder_SideHolder_Label17" style="float:right;">
                            <div>
                                <asp:Label ID="lbl_DepartureDateIN" runat="server" ></asp:Label>
                            </div> 
                        </div>
                    </td>
                </tr>      
                <tr id="td_Return" runat="server">
                    <td style="padding-bottom: 3px;" valign="top"> 
                        <label id="RootHolder_SideHolder_ASPxLabel1" font-size="Small">
                            <b>
                                <asp:Label ID="lbl_ArrivalIN" runat="server" ></asp:Label> 
                            </b>
                        </label>
                    </td>
                    <td style="width: 332px; padding:0 0 10px 20px;">
                            <div id="RootHolder_SideHolder_Div10" style="float:right;">
                                <div>
                                    <asp:Label ID="lbl_ArrivalDateIN" runat="server" ></asp:Label>
                                </div> 
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
        <div style="height:30px;"></div>               
    </div>
    <!--End of Info-->
</div>--%>


<div style="display:none">
    <asp:Label ID="LblTransitDepart" runat="server" Text="" ForeColor="Red"/>
    <asp:Label ID="LblTransitReturn" runat="server" Text="" ForeColor="Red"/>
    <asp:Label ID="lbl_GuestNumout" runat="server" ></asp:Label>
    <asp:Label ID="lbl_ChildNumout" runat="server" ></asp:Label>
    <asp:Label ID="lbl_PaxNumout" runat="server"></asp:Label>
    <asp:Label ID="lbl_GuestNumIN" runat="server" ></asp:Label>
    <asp:Label ID="lbl_ChildNumIN" runat="server" ></asp:Label>
    <asp:Label ID="lbl_PaxNumIN" runat="server" ></asp:Label>
    <asp:Label ID="LblReturn" runat="server"  Text=""></asp:Label>
</div>

    <%--<table cellpadding="0" cellspacing="0" width="100%" style="font-family:Arial,Helvetica,san-serif;">
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
                                        <tr align="left">
                                            <td width="100px">
                                               &nbsp;&nbsp;<asp:Label ID="LblGoing" runat="server"  Text="Origin :"></asp:Label>
                                            </td>
                                            <td>
                                               <div>
                                            &nbsp;</div>
                                        <div >
                                            <b>&nbsp;&nbsp;
                                            <br />
                                            
                                                </div>                                        
                                        <div >
                                            &nbsp;&nbsp;</div>
                                            </td>
                                            <td style="border-left: solid 1px #C4C4C4;" align="center">
                                                <div>&nbsp;&nbsp;</div>
                                            </td>
                                            <td style="border-left: solid 1px #C4C4C4;" align="center">
                                                <div>&nbsp;&nbsp;</div>                                                                                 
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;&nbsp;
                                            </td>
                                            <td style="border-left: solid 1px #C4C4C4;">
                                                &nbsp;
                                            </td>
                                            <td style="border-left: solid 1px #C4C4C4;">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr align="left">
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

                                            <b>&nbsp;&nbsp;                                            
                                             </div>
                                             <div >
                                            &nbsp;&nbsp;</div>
                                            </td>
                                            <td style="border-left: solid 1px #C4C4C4;" align="center">
                                                <div >
                                                &nbsp;&nbsp;</div>
                                            </td>
                                            <td style="border-left: solid 1px #C4C4C4;" align="center"><div>
                                                &nbsp;&nbsp;</div>                                        
                                            </td>                                                                                
                                        </tr>     
                                        <tr align="left">
                                            <td>
                                            &nbsp;&nbsp;
                                            </td>
                                            <td>
                                                &nbsp;&nbsp;                                               
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
                                <tr bgcolor="#f7f3f7" align="left">
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
    </table>--%> 
     