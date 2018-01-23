<%@ Page Title="" Language="C#" MasterPageFile="~/Master/PageMasterReport.Master" AutoEventWireup="true" CodeBehind="agentsearchflightchange.aspx.cs" Inherits="GroupBooking.Web.agentsearchflightchange" %>
<%@ Register Assembly="SEAL.WEB" Namespace="SEAL.WEB.UI" TagPrefix="cc1" %>
<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript">
        document.body.onload = function () {
            //if (!isPostBack) {
            initDateControl()
            //}

        }

        function initDateControl() {

            var today = new Date();
            var date1 = new Date();
            var date2 = new Date();
            date1.setDate(today.getDate());
            date2.setDate(today.getDate() + 7);

            document.getElementById('ddlMarketDay1').value = date1.getDate();

            var j = 1;

            var marketDateIdArray = [];
            marketDateIdArray[0] = { "marketDateId": "date_picker_id_1", "marketDayId": "ddlMarketDay1", "marketMonthYearId": "ddlMarketMonth1" };
            var _Return = document.getElementById('<%= trReturn.ClientID %>');
            if (_Return != null) {
                document.getElementById('ddlMarketMonth2').value = date2.getFullYear() + '-' + ("0" + (date2.getMonth() + 1)).slice(-2);
                document.getElementById('ddlMarketDay2').value = date2.getDate();
                marketDateIdArray[1] = { "marketDateId": "date_picker_id_2", "marketDayId": "ddlMarketDay2", "marketMonthYearId": "ddlMarketMonth2" };
                j = 2;
            }

            for (i = 0; i < j; i++) {
                var marketDateId = marketDateIdArray[i];
                var marketDate = new SKYSALES.Class.MarketDate();
                marketDate.marketDateCount = j;
                marketDate.init(marketDateId);
            }
        }

        function OnCallbackComplete(s, e) {
            if (e.result != "") {
                //lblmsg.SetValue(e.result);
                document.getElementById("ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = e.result;
                pcMessage.Show();
                LoadingPanel.Hide();
            } else {
                window.location.href = '../public/agentselectflightchange.aspx';
            }
        }

        function ShowLoginWindow() {
            pcMessage.Show();
        }

        </script>

<msg:msgControl ID="msgcontrol" runat="server" />
<div>
<dx:ASPxCallback ID="CallBackValidation" ClientInstanceName="CallBackValidation" runat="server" OnCallback="ValidatePopup">
        <ClientSideEvents CallbackComplete="OnCallbackComplete" />
    </dx:ASPxCallback>
</div>

<table cellpadding="0" cellspacing="0" width="100%">
    <tr id="tr_Request" runat="server">
        
        <td class="tdblank">
            &nbsp;
        </td>
        <td>
            <div class="div">
                Search flights</div>
            <br /> 
            <table class="tableright">
                <tr class="tr">
                    <td colspan="2">
                    </td>
                </tr>
                <tr>
                    <td class="td1">
                        <font color="red">*</font>From:
                    </td>
                    <td class="td2">
                        <cc1:customdropdownlist ID="ddlDeparture" runat="server" Width="184px"
                            onselectedindexchanged="ddlDeparture_SelectedIndexChanged" 
                            AutoPostBack="true" > </cc1:customdropdownlist> 
                              
                                         
                    </td>
                </tr>  
                <tr>
                    <td class="td1">  
                        <font color="red">*</font>To:
                    </td>
                    <td class="td2">
                        
                        <cc1:CustomDropDownList ID="ddlReturn" runat="server" Width="184px">
                        <asp:ListItem Text="Select City"></asp:ListItem>
                        </cc1:CustomDropDownList>                  
                    </td>
                </tr>
                <tr>
                    <td class="td1">
                    </td>
                    <td class="td2">
                        <asp:CheckBox ID="cb_OneWay" runat="server" Text="One way only" 
                            AutoPostBack="True" oncheckedchanged="cb_OneWay_CheckedChanged" />
                    </td>
                </tr>
                <tr>
                    <td class="td1">
                        Currency:
                    </td>
                    <td class="td2">
                        <asp:DropDownList ID="ddl_Currency" runat="server" CssClass="Select" 
                            Enabled="False" Width="184px">
                            <asp:ListItem Value="USD">Select currency</asp:ListItem>                            
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Depart:
                    </td>
                    <td>    
                        <div id="divDate1" runat="server">
                        </div>                           
                    </td>
                </tr>
                <tr id="trReturn" runat="server" name="trReturn">
                    <td>
                        Return:
                    </td>
                    <td>
                        <div id="divDate2" runat="server">
                        </div>                          
                    </td>
                </tr>
                <%-- 
                <tr>
                    <td class="td1">
                        Guest:
                    </td>
                    <td class="td2">
                        <asp:TextBox ID="txt_GuestNum"  runat="server"   style= "text-align:right " CssClass="input" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))) {event.returnValue=true;} else{event.returnValue=false;}" >0</asp:TextBox>
                        <font class="td3">&nbsp;(Aged 12 years and above)</font> 
                    </td>
                </tr>
                <tr>
                    <td class="td1">
                        Child:
                    </td>
                    <td class="td2"  >
                        <asp:TextBox ID="txt_ChildNum"   runat="server" CssClass="input" style= "text-align:right "  OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))) {event.returnValue=true;} else{event.returnValue=false;}" >0</asp:TextBox>
                        <font class="td3">&nbsp;(Aged between 2 years and 11 years)</font>
                    </td>
                </tr>
                <tr  style="display:none">
                    <td class="td1">
                        Infant:
                    </td>
                    <td class="td2">
                        <asp:TextBox ID="txt_InfantNum" runat="server" CssClass="input" style= "text-align:right "  OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))) {event.returnValue=true;} else{event.returnValue=false;}" >0</asp:TextBox>
                        <font class="td3">&nbsp;(Aged between 9 days and 24 months)</font>
                    </td>
                </tr>
                <tr>
                    <td class="td1">
                    </td>
                    <td class="td3">
                        <a href="javascript:void(0)" onclick="javascript:window.open('http://www.airasia.com/my/en/at-the-airport/special-guests.page','','width=600, height=500,scrollbars=yes')">
                            <font class="td3">Guest under 18 years old</font></a>
                    </td>
                </tr>
                <tr>
                    <td class="td1">
                    </td>
                    <td class="td2">
                    <font class="td3">&nbsp;(The maximum booking for seat flight is 50 pax.)</font>
                    </td>
                </tr>--%>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="td1">
                    </td>
                    <td class="td4">
                        <dx:ASPxButton CssClass="button_3"  
                            ID="btnSubMit" runat="server" Text="Confirm Booking"  AutoPostBack="false"
                             Width="152px" Height="23px" >
                         <ClientSideEvents Click="function(s, e) {CallBackValidation.PerformCallback();
                                            LoadingPanel.SetText(&#39;Please Wait...&#39;);
                                           LoadingPanel.Show(); }" />
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table> 
        </td>
    </tr>
     
</table>

</asp:Content>

