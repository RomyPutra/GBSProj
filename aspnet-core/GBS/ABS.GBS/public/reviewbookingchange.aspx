<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReviewBookingChange.aspx.cs"
    Inherits="GroupBooking.Web.Booking.ReviewBookingChange" MasterPageFile="~/Master/NewPageMasterReport.Master" %>
	
<%@ MasterType  virtualPath="~/Master/NewPageMaster.Master"%>
<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolder2">    
    <script type="text/javascript">
    // <![CDATA[

    function OnCallbackComplete(s, e) {
        if (e.result != "") {
            //lblmsg.SetValue(e.result);
            document.getElementById("ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = e.result;
            pcMessage.Show();
            LoadingPanel.Hide();
        } else {
            window.location.href = '../public/proceedpaymentchange.aspx';
        }
    }

    function ShowLoginWindow() {
        pcMessage.Show();
    }
    // ]]> 
    </script>

<msg:msgControl ID="msgcontrol" runat="server" />
<div>
<dx:ASPxCallback ID="CallBackValidation" ClientInstanceName="CallBackValidation" runat="server" OnCallback="ValidatePopup">
        <ClientSideEvents CallbackComplete="OnCallbackComplete" />
    </dx:ASPxCallback>
</div>
 
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td class="tdright">
                <div class="div">
                   <h2> Review</h2></div>
                <hr />
                <div>
                    You've almost completed your booking. We recommend that you double check your flight
                    number, date, time, destination and total amount due before you select the mode
                    of payment that best suits you.
                </div>
                <br />
                <style type="text/css">
                .tblData table tr td
                {
                    padding:2px 2px 2px 2px;
                }
                </style>
                <table class="tblData" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="center">
                            <br />
                            <table width="100%" bgcolor="#f7f3f7">
                                <tr  style="vertical-align:middle;height:30px;background-color:#333333;color:White;font-size:small;">
                                    <td class="tdcol">
                                        <h3>&nbsp;Summary</h3>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%" cellspacing="0" cellpadding="0" border="0" bgcolor="#f7f3f7">
                                            <tr id="tr_depart2" runat="server">
                                                <td class="dyncontent" colspan="3">
                                                    <div id="mkt_content1" class="dyncontainer">
                                                        <table class="priceDisplay" width="100%" cellspacing="0" cellpadding="5" border="0" bgcolor="#f7f3f7">
                                                            <tr>
                                                                <td align="left">
                                                                    <b><asp:Label ID="lbl_departure" runat="server"></asp:Label> - <asp:Label ID="lbl_Arrival" runat="server"></asp:Label></b>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <!-- bgcolor="#87CEFA" -->
                                                            <tr>
                                                                <td colspan="2" align="left">
                                                                    <asp:Label ID="lbl_num" runat="server"></asp:Label> Pax @ <asp:Label ID="lbl_Average" runat="server"></asp:Label> <asp:Label ID="lbl_currency0" runat="server"></asp:Label>
                                                                </td>
                                                                <td class="priceamount"  align="right">
                                                                    <asp:Label ID="lbl_FlightTotal" runat="server"></asp:Label> <asp:Label ID="lbl_currency1" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" align="left">
                                                                    <asp:Label ID="lbl_num2" runat="server"> Airport Tax @ </asp:Label><asp:Label ID="lbl_taxPrice" runat="server"></asp:Label> <asp:Label ID="lbl_currency3" runat="server"></asp:Label>
                                                                </td>
                                                                <td class="priceamount"  align="right">
                                                                    <asp:Label ID="lbl_taxTotal" runat="server"></asp:Label> <asp:Label ID="lbl_currency2" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr id="trAptCHD" runat="server">
                                                                <td colspan="2" align="left">
                                                                    <asp:Label ID="lbl_num2CHD" runat="server"></asp:Label> Child Airport Tax @ <asp:Label ID="lbl_taxPriceCHD" runat="server"></asp:Label> <asp:Label ID="lbl_currency3CHD" runat="server"></asp:Label>
                                                                </td>
                                                                <td class="priceamount"  align="right">
                                                                    <asp:Label ID="lbl_taxTotalCHD" runat="server"></asp:Label> <asp:Label ID="lbl_currency2CHD" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" align="left">
                                                                    <asp:Label ID="lbl_num3" runat="server"></asp:Label> Fuel Tax @ <asp:Label ID="lblFuelPriceOneDepart" runat="server"></asp:Label> <asp:Label ID="lbl_currency4" runat="server"></asp:Label>
                                                                </td>
                                                                <td class="priceamount"  align="right">
                                                                    <asp:Label ID="lblFuelPriceTotalDepart" runat="server"></asp:Label> <asp:Label ID="lblCurrFuelDepart" runat="server"></asp:Label>
                                                                </td>
                                                            </tr> 
                                                            <tr>
                                                                <td colspan="2" align="left">
                                                                    <asp:Label ID="lbl_num4" runat="server"></asp:Label> Service Charge @ <asp:Label ID="lblSvcChargeOneDepart" runat="server"></asp:Label> <asp:Label ID="lbl_currency5" runat="server"></asp:Label>
                                                                </td>
                                                                <td class="priceamount"  align="right">
                                                                    <asp:Label ID="lblSvcChargeTotalDepart" runat="server"></asp:Label> <asp:Label ID="lblCurrSvcDepart" runat="server"></asp:Label>
                                                                </td>
                                                            </tr> 
                                                            <tr>
                                                                <td colspan="2" align="left">
                                                                    <asp:Label ID="lbl_num5" runat="server"></asp:Label> VAT @ <asp:Label ID="lblVATDepart" runat="server"></asp:Label> <asp:Label ID="lbl_currency6" runat="server"></asp:Label>
                                                                </td>
                                                                <td class="priceamount"  align="right">
                                                                    <asp:Label ID="lblVATTotalDepart" runat="server"></asp:Label> <asp:Label ID="lblCurrVATDepart" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" align="left">
                                                                   <asp:Label ID="lbl_num6" runat="server"></asp:Label>  Other Charge @ <asp:Label ID="lblOthOneDepart" runat="server"></asp:Label> <asp:Label ID="lbl_currency7" runat="server"></asp:Label>
                                                                </td>
                                                                <td class="priceamount"  align="right">
                                                                    <asp:Label ID="lblOthTotalDepart" runat="server"></asp:Label> <asp:Label ID="lblCurrOthDepart" runat="server"></asp:Label>
                                                                </td>
                                                            </tr> 
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="tr_depart" runat="server">
                                            <td class="dyncontent" colspan="3">
                                                    <div id="Div1" class="dyncontainer">
                                                        <table class="priceDisplay" width="100%" cellspacing="0" cellpadding="5" border="0" bgcolor="#f7f3f7">
                                                            <tr>
                                                                <td align="left">
                                                                <strong>Depart Total</strong>
                                                                </td>
                                                                <td class="amountdesc">
                                                                </td>
                                                                <td class="priceamountmain"  align="right">
                                                                    <a href="javascript:SKYSALES.togglePriceSummary('PriceMarket1');" onclick="javascript:$('#mkt_content1').slideToggle();">
                                                                    </a><b> <asp:Label ID="lbl_Total" runat="server"></asp:Label></b> <asp:Label ID="lbl_currency" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                            </td>
                                                
                                            </tr>
                                            <tr><td colspan="3">&nbsp;</td></tr>
                                            <tr  id="tr_return2" runat="server">
                                                <td class="dyncontent" colspan="3">
                                                    <div id="Div2" class="dyncontainer">
                                                        <table class="priceDisplay" width="100%" cellspacing="0" cellpadding="0" border="0" bgcolor="#f7f3f7">
                                                            <tr>
                                                                <td align="left">
                                                                    <b><asp:Label ID="lbl_InDeparture" runat="server"></asp:Label> - <asp:Label ID="lbl_InArrival" runat="server"></asp:Label></b>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" align="left">
                                                                    <asp:Label ID="lbl_InNum" runat="server"></asp:Label> Pax @ <asp:Label ID="lbl_InAverage" runat="server"></asp:Label> <asp:Label ID="lbl_InCurrency0" runat="server"></asp:Label>
                                                                </td>
                                                                <td class="priceamount"  align="right">
                                                                    <asp:Label ID="lbl_InFlightTotal" runat="server"></asp:Label> <asp:Label ID="lbl_Incurrency1" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" align="left">
                                                                   <asp:Label ID="lbl_InNum2" runat="server"> Airport Tax @ </asp:Label><asp:Label ID="lbl_IntaxPrice" runat="server"></asp:Label> <asp:Label ID="lbl_InCurrency3" runat="server"></asp:Label>
                                                                </td>
                                                                <td class="priceamount"  align="right">
                                                                    <asp:Label ID="lbl_IntaxTotal" runat="server"></asp:Label> <asp:Label ID="lbl_Incurrency2" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr id="trInAptCHD" runat="server">
                                                                <td colspan="2" align="left">
                                                                   <asp:Label ID="lbl_InNum2CHD" runat="server"></asp:Label> Child Airport Tax @ <asp:Label ID="lbl_IntaxPriceCHD" runat="server"></asp:Label> <asp:Label ID="lbl_InCurrency3CHD" runat="server"></asp:Label>
                                                                </td>
                                                                <td class="priceamount"  align="right">
                                                                    <asp:Label ID="lbl_IntaxTotalCHD" runat="server"></asp:Label> <asp:Label ID="lbl_Incurrency2CHD" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                             <tr>
                                                                <td colspan="2" align="left">
                                                                    <asp:Label ID="lbl_InNum3" runat="server"></asp:Label> Fuel Tax @ <asp:Label ID="lblFuelOneReturn" runat="server"></asp:Label> <asp:Label ID="lbl_InCurrency4" runat="server"></asp:Label>
                                                                </td>
                                                                <td class="priceamount"  align="right">
                                                                    <asp:Label ID="lblFuelTotalReturn" runat="server"></asp:Label> <asp:Label ID="lblCurrFuelReturn" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>  
                                                            <tr>
                                                                <td colspan="2" align="left">
                                                                    <asp:Label ID="lbl_InNum4" runat="server"></asp:Label> Service Charge @ <asp:Label ID="lblSvcOneReturn" runat="server"></asp:Label> <asp:Label ID="lbl_InCurrency5" runat="server"></asp:Label>
                                                                </td>
                                                                <td class="priceamount"  align="right">
                                                                    <asp:Label ID="lblSvcTotalReturn" runat="server"></asp:Label> <asp:Label ID="lblCurrSvcReturn" runat="server"></asp:Label>
                                                                </td>
                                                            </tr> 
                                                            <tr>
                                                                <td colspan="2" align="left">
                                                                    <asp:Label ID="lbl_InNum5" runat="server"></asp:Label> VAT @ <asp:Label ID="lblVATReturn" runat="server"></asp:Label> <asp:Label ID="lbl_InCurrency6" runat="server"></asp:Label>
                                                                </td>
                                                                <td class="priceamount"  align="right">
                                                                    <asp:Label ID="lblVATTotalReturn" runat="server"></asp:Label> <asp:Label ID="lblCurrVATReturn" runat="server"></asp:Label>
                                                                </td>
                                                            </tr> 
                                                            <tr>
                                                                <td colspan="2" align="left">
                                                                    <asp:Label ID="lbl_InNum6" runat="server"></asp:Label> Other Charge @ <asp:Label ID="lblOthOneReturn" runat="server"></asp:Label> <asp:Label ID="lbl_InCurrency7" runat="server"></asp:Label>
                                                                </td>
                                                                <td class="priceamount"  align="right">
                                                                    <asp:Label ID="lblOthTotalReturn" runat="server"></asp:Label> <asp:Label ID="lblCurrOthReturn" runat="server"></asp:Label>
                                                                </td>
                                                            </tr> 
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="tr_return" runat="server">
                                                <td class="dyncontent" colspan="3">
                                                    <div id="Div3" class="dyncontainer">
                                                        <table class="priceDisplay" width="100%" cellspacing="0" cellpadding="5" border="0" bgcolor="#f7f3f7">
                                                            <tr>
                                                                <td align="left">
                                                                <strong>Return Total</strong>
                                                                </td>
                                                                <td class="amountdesc">
                                                                </td>
                                                                <td class="priceamountmain"  align="right">
                                                                    <a href="javascript:SKYSALES.togglePriceSummary('PriceMarket1');" onclick="javascript:$('#mkt_content1').slideToggle();">
                                                                    </a><b> <asp:Label ID="lbl_InTotal" runat="server"></asp:Label> </b><asp:Label ID="lbl_InCurrency" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                            </td>
                                                
                                            </tr>
                                            <tr><td colspan="3">&nbsp;</td></tr>
                                            <tr id="tr1" runat="server">
                                                <td class="dyncontent" colspan="3">
                                                    <div id="Div4" class="dyncontainer">
                                                        <table class="priceDisplay" width="100%" cellspacing="0" cellpadding="5" border="0" bgcolor="#f7f3f7">
                                                            <tr>
                                                                <td align="left">
                                                                <strong>Total</strong>
                                                                </td>
                                                                <td class="amountdesc">
                                                                </td>
                                                                <td class="priceamountmain"  align="right">
                                                                    <a href="javascript:SKYSALES.togglePriceSummary('PriceMarket1');" onclick="javascript:$('#mkt_content1').slideToggle();">
                                                                    </a><b> <asp:Label ID="lbl_TotalAmount" runat="server"></asp:Label></b> <asp:Label ID="lbl_TotalCurrency" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                            </td>
                                                
                                            </tr>
                                            <tr>
                                                <td class="dyncontent" colspan="3">
                                                    <div id="Div5" class="dyncontainer">
                                                        <table class="priceDisplay" width="100%" cellspacing="0" cellpadding="5" border="0" bgcolor="#f7f3f7">
                                                            <tr>
                                                                <td align="left">
                                                                <strong>Average Fare</strong>
                                                                </td>
                                                                <td class="amountdesc">
                                                                </td>
                                                                <td align="right">
                                                                    <a href="javascript:SKYSALES.togglePriceSummary('PriceMarket1');" onclick="javascript:$('#mkt_content1').slideToggle();">
                                                                    </a><b> <asp:Label ID="lblAverageFare" runat="server"></asp:Label></b> <asp:Label ID="lblAverageCurrency" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                            </td>
                                                
                                            </tr>
                                            <tr  style="display:none">
                                                <td align="left">
                                                    <strong>Services, fees and insurance</strong>
                                                </td>
                                                <td class="amountdesc">
                                                </td>
                                                <td class="priceamountmain"  align="right">
                                                    220.00 CNY
                                                </td>
                                            </tr>
                                            <tr style="display:none">
                                                <td class="dyncontent" colspan="3">
                                                    <div id="misc_content" class="dyncontainer">
                                                        <table width="100%" cellspacing="0" cellpadding="0" border="0">
                                                            <tr>
                                                                <td colspan="2" align="left">
                                                                    Advance Seat Request Fee
                                                                </td>
                                                                <td class="priceamount"  align="right">
                                                                    15.00 CNY
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" align="left">
                                                                    Advance Seat Request Fee
                                                                </td>
                                                                <td class="priceamount"  align="right">
                                                                    15.00 CNY
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" align="left">
                                                                    Supersize Medium (up to 20KG)
                                                                </td>
                                                                <td class="priceamount"  align="right">
                                                                    75.00 CNY
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" align="left">
                                                                    Supersize Medium (up to 20KG)
                                                                </td>
                                                                <td class="priceamount"  align="right">
                                                                    75.00 CNY
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" align="left">
                                                                    AirAsia Insure
                                                                </td>
                                                                <td class="priceamount"  align="right">
                                                                    40.00 CNY
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="600px"  style="display:none">
                                            <tr>
                                                <td class="priceamounttotalspacer">
                                                </td>
                                                <td>
                                                    <strong>Current total</strong>
                                                </td>
                                                <td class="priceamounttotal" align="right">
                                                    1,680.00 CNY
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                     <tr class="trheight" style="display:none">
                        <td class="tdcol">
                            &nbsp;Payment applied to booking
                        </td>
                    </tr>
                    <tr style="display:none">
                        <td>
                            Please spceify your preferred paid currency to proceed for payment</td>
                    </tr>
                    <tr style="display:none">
                        <td>
                        </td>
                    </tr>
                    <tr style="display:none">
                        <td align="center">
                        <table>
                            <tr>
                                <td>
                                    Currency
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_Currency" runat="server" CssClass="Select">
                                        <asp:ListItem Value="USD">Select currency</asp:ListItem>
                                        <asp:ListItem Value="USD">US Dollar (USD)</asp:ListItem>
                                        <asp:ListItem Value="AUD">Australian Dollar (AUD)</asp:ListItem>
                                        <asp:ListItem Value="MYR">Malaysian Ringgit (MYR)</asp:ListItem>
                                        <asp:ListItem Value="THB">Thailand Baht (THB)</asp:ListItem>
                                        <asp:ListItem Value="CNY" Selected="True">Chinese Renminbi Yuan (CNY)</asp:ListItem>
                                        <asp:ListItem Value="GBP">Sterling (GBP)</asp:ListItem>
                                        <asp:ListItem Value="SGD">Singapore Dollar (SGD)</asp:ListItem>
                                        <asp:ListItem Value="EUR">Euro (EUR)</asp:ListItem>
                                        <asp:ListItem Value="NZD">New Zealand Dollar (NZD)</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <%--<dx:aspxbutton CssClass="button_2"  
                                        ID="btn_UpdateCurrency" runat="server"  
                                        Text="Apply" AutoPostBack="False" OnClick="btn_UpdateCurrency_Click" >
                                    </dx:aspxbutton>--%>
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
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table> 
    <div>
        
    <dx:aspxbutton CssClass="buttonL" ID="btn_Next" runat="server"  
                                            Text="Continue" AutoPostBack="False" >
                                         <ClientSideEvents Click="function(s, e) {CallBackValidation.PerformCallback();
                                            LoadingPanel.SetText(&#39;Please Wait...&#39;);
                                           LoadingPanel.Show(); }" />
                                        </dx:aspxbutton>
    </div>
</asp:Content>
