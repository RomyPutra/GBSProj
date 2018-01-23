<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="flightbookingdetail.ascx.cs" Inherits="ABS.GBS.UserControl.flightbookingdetail" %>

<%--<div class="formWrapperLeftAA" style="max-height:1200px">   
    <div class="sidebarTitle" style="padding: 15px 0;">
    <span class="dxeBase_GBS" id="RootHolder_SideHolder_lblHeader" style="font-size:Medium;font-weight:normal;">
        <div class="width100"> Flight Detail </div>      
    </span>
    </div>

    <div style="" class="secondAlignRight">

        <table id="departureLeft" width="100%">
            <tbody>
                <asp:Repeater EnableViewState="true" ID="rptFlightDetails" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td style="padding-bottom: 3px;" valign="top">
                                <div class="" style="text-align:left;"><span class="departFlight fa fa-plane"></span></div>
                            </td>
                            <td>
                                <div class="" style="text-align:right;">
                                    <span class="biggerSize" id="Span7">
                                        <dx:ASPxLabel ID="lblCarrierCode" runat="server" Text='<%#Eval("CarrierCode")%>'  ClientInstanceName="lblCarrierCode"/>
                                        <dx:ASPxLabel ID="lblFlightNo" runat="server" Text='<%#Eval("FlightNo")%>' ClientInstanceName="lblFlightNo"/>

                                    </span>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-bottom: 3px;" valign="top"> 
                                <label font-size="Small">
                                    <b>
                                        <dx:ASPxLabel ID="lblOrigin" runat="server" Text='<%#Eval("Origin")%>' ClientInstanceName="lblOrigin"/>
                                    </b>
                                </label>
                            </td>
                            <td style="width: 332px; padding:0 0 10px 20px;" valign="top">
                                <div id="RootHolder_SideHolder_Label17" style="float:right;">
                                    <div>
                                        <dx:ASPxLabel ID="lblDepartureDate" runat="server" Text='<%# string.Format("{0:ddd, dd MMM yyyy HH:mm}", Eval("DepatureDate")) %>' ClientInstanceName="lblDepartureDate"/>
                                    </div> 
                                </div>
                            </td>
                        </tr>      
                        <tr>
                            <td style="padding-bottom: 3px;" valign="top"> 
                                <label id="RootHolder_SideHolder_ASPxLabel1" font-size="Small">
                                    <b>
                                        <dx:ASPxLabel ID="lblDestination" runat="server" Text='<%#Eval("Destination")%>' ClientInstanceName="lblDestination"/>
                                    </b>
                                </label>
                            </td>
                            <td style="width: 332px; padding:0 0 10px 20px;">
                                    <div id="RootHolder_SideHolder_Div10" style="float:right;">
                                        <div>
                                            <dx:ASPxLabel ID="lblArrivalDate" runat="server" Text='<%# string.Format("{0:ddd, dd MMM yyyy HH:mm}", Eval("ArrivalDate")) %>' ClientInstanceName="lblArrivalDate"/>
                                        </div> 
                                </div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <div style="height:30px;"></div>
        
    </div>

</div>--%>



<div id="flightDetailWrapper">

    <!-- The header to show the depart and return -->
    <div id="flightRoute">
        <span id="flightDepartReturn"><%--Kuala Lumpur (KUL) to Singapore (SIN)--%>
            <asp:Label ID="lblAgentName" runat="server"></asp:Label>
            <br />
            <asp:Label ID="lblAgentOrg" runat="server"></asp:Label>
        </span>
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
            </div>

            <div class="flightTitleDepartReturn" style="display:inline-block">RETURN</div>
 
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
