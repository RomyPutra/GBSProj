<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchFlightChange.aspx.cs" Inherits="GroupBooking.Web.SearchFlightChange" MasterPageFile="~/Master/NewPageMaster.Master" %>

<%@ Register Assembly="SEAL.WEB" Namespace="SEAL.WEB.UI" TagPrefix="cc1" %>
<%@ MasterType  virtualPath="~/Master/NewPageMaster.Master"%>
<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolder2">

<script type="text/javascript" src="../Scripts/jquery-1.4.1.js"></script>
<script type="text/javascript" src="../Scripts/CalendarChange.js"></script>

<script type="text/javascript">
    
     // <![CDATA[
    window.onload = function () {
        //if (!isPostBack) {

        initDateControl();
        //}

    }

    function initDateControl() {

        var today = new Date();
        var date1 = new Date();
        var date2 = new Date();
        var dd = $('#ctl00_ContentPlaceHolder2_hDepart').val();
        var dr = $('#ctl00_ContentPlaceHolder2_hReturn').val();
        var vReturnOnly = $('#ctl00_ContentPlaceHolder2_hReturnOnly').val();

        if (dd != null && dd != "") {
            //date1.setDate(new Date(dd).getDate());
            date1 = new Date(dd);
        } else {
            date1.setDate(today.getDate() + 2);
        }

        if (dr != null && dr != "") {
            //date2.setDate(new Date(dr).getDate());
            date2 = new Date(dr);
        } else {
            date2.setDate(today.getDate() + 9);
        }

        //window.alert(date1 + " " + date2);
        document.getElementById('ddlMarketMonth1').value = date1.getFullYear() + '-' + ("0" + (date1.getMonth() + 1)).slice(-2);
        var ddl1 = document.getElementById("ddlMarketDay1");
        for (var i = 0; i < ddl1.options.length; i++) {
            if (ddl1.options[i].value == date1.getDate()) {
        if (ddl1.selectedIndex != i) {
        ddl1.selectedIndex = i;
        }
        break;
        }
        }

        var ddm1 = document.getElementById('ddlMarketMonth1');
        if (ddm1.selectedIndex < 0) {
            ddm1.selectedIndex = 0;
        }
        var j = 1;

        var marketDateIdArray = [];
        marketDateIdArray[0] = { "marketDateId": "date_picker_id_1", "marketDayId": "ddlMarketDay1", "marketMonthYearId": "ddlMarketMonth1" };
        var _Return = document.getElementById('<%= tdReturn.ClientID %>');
        if (_Return != null) {
            //document.getElementById('ddlMarketDay2').value = date2.getDate();

            //window.alert(date2.getFullYear() + '-' + ("0" + (date2.getMonth() + 1)).slice(-2));
            document.getElementById('ddlMarketMonth2').value = date2.getFullYear() + '-' + ("0" + (date2.getMonth() + 1)).slice(-2);
            var ddl2 = document.getElementById("ddlMarketDay2");
            for (var i = 0; i < ddl2.options.length; i++) {
                if (ddl2.options[i].value == date2.getDate()) {
                    if (ddl2.selectedIndex != i) {
                        ddl2.selectedIndex = i;
                    }
                    break;
                }
            }
            
            marketDateIdArray[1] = { "marketDateId": "date_picker_id_2", "marketDayId": "ddlMarketDay2", "marketMonthYearId": "ddlMarketMonth2" };
            j = 2;
        }

        var ddm2 = document.getElementById('ddlMarketMonth2');
        if (ddm2.selectedIndex < 0) {
            ddm2.selectedIndex = 0;
        }

        //if return only, then show return datepicker only
        var x = 0;
        if (vReturnOnly == "true") {
            x = 1;
        }

        for (i = x; i < j; i++) {
            var marketDateId = marketDateIdArray[i];
            var marketDate = new SKYSALES.Class.MarketDate();
            marketDate.marketDateCount = j;
            marketDate.init(marketDateId);
        }
        
    }

    function OnCallbackComplete(s, e) {
        if (e.result != "") {
            //lblmsg.SetValue(e.result);
            document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = e.result;
            pcMessage.Show();
            LoadingPanel.Hide();
        } else {
            window.location.href = '../public/selectflightchange.aspx';
        }
    }

    function ShowLoginWindow() {
        pcMessage.Show();
    }

    function OnBtnClick(s, e) {
        if (cal.GetVisible() == false) {
            cal.SetVisible(true);
            s.SetText("Hide");
        }
        else {
            cal.SetVisible(false);
            s.SetText("Show");
        }
    }

    function OnSelectionChanged(s, e) {
        txt.SetValue(s.GetSelectedDate());
        s.SetVisible(false);
        btn.SetText("Show");
    }

    $(document).ready(function () {
        ValidateOneWay();
        //SelectedDate();
    });

    function ValidateOneWay() {
        //SelectedDate();
        if ($('#ctl00_ContentPlaceHolder2_cb_OneWay').is(':checked')) {
            $('#ctl00_ContentPlaceHolder2_tdReturn').hide();
        } else {
            $('#ctl00_ContentPlaceHolder2_tdReturn').show();
        }
    }



    // ]]> 
    </script>

<msg:msgControl ID="msgcontrol" runat="server" />
<input type="hidden" id="hDepart" runat="server" value="" />
<input type="hidden" id="hReturn" runat="server" value="" />
<input type="hidden" id="hReturnOnly" runat="server" value="" />

<div>
<dx:ASPxCallback ID="CallBackValidation" ClientInstanceName="CallBackValidation" runat="server" OnCallback="ValidatePopup">
        <ClientSideEvents CallbackComplete="OnCallbackComplete" />
    </dx:ASPxCallback>
</div>
<div class="search">
    <div class="search-form-top">
    Online Booking is now available up to 48 hours before flights 
    </div>
</div>
<br />
<div style="clear:both"></div>
<div class="search">
    <div class="search-form-top">
    <h2>Search Flights : <asp:Label runat="server" ID="lblPNR"></asp:Label></h2>
    <br/>
    <div class="search-form-top-items-1">
    
    <input type="checkbox" readonly="readonly" runat="server" name="cb_OneWay" id="cb_OneWay" onclick="return false;" value="One Way Only" />One Way Only
    </div>
    </div>

    <div class="search-form-middle">
        <ul>
        <li class="first">
            <div class="col1">
            <div id="marketCityPair_1">
                <cc1:customdropdownlist ID="ddlDeparture" runat="server" Width="184px"
                            onselectedindexchanged="ddlDeparture_SelectedIndexChanged" 
                            AutoPostBack="true" CssClass="chzn-select">
                            <asp:ListItem Text="Origin" ></asp:ListItem>
                            </cc1:customdropdownlist>
            </div>
            </div>
            
            <div class="col2">
                <table>
                    <tbody>
                    <tr>
                    <td>
                        <div id="marketDate_1" class="depart-date left black3">
                             <div id="divDate1" runat="server">
                            </div>
                        </div>
                    </td>
                    <td id="tdReturn" runat="server">
                        <div id="marketDate_2" class="depart-date left black3">
                            <div id="divDate2" runat="server">
                            </div>
                        </div>
                    </td>

                    </tr>
                    </tbody>
                </table>
            </div>
        </li>
        <li class="second">
            <div class="col1">
            <div id="Div1">
                <div id="Div2">
                <div id="currency">
                <p id="destinationStationContainer1" class="multicoldd multicoldd-container">
                <cc1:CustomDropDownList ID="ddlReturn" runat="server" Width="184px" CssClass="chzn-select">
                        <asp:ListItem Text="Destination"></asp:ListItem>
                        </cc1:CustomDropDownList> 
                </p>

                <p class="marginT20">
                    <asp:DropDownList ID="ddl_Currency" runat="server" 
                            Enabled="False" Width="184px">
                            <asp:ListItem Value="USD">Select currency</asp:ListItem>
                           
                        </asp:DropDownList>
                </p>
                </div>
                </div>
<!-- <script type="text/javascript" src="../Scripts/jquery.min.js"></script> -->
<script type="text/javascript" src="../Scripts/chosen.jquery.js"></script>
<link rel="Stylesheet" href="../Styles/chosen.css" />

<script type="text/javascript">
    $(".chzn-select").chosen();
    $(".chzn-select-deselect").chosen({
        allow_single_deselect: true
    });
</script>

            </div>
            </div>

            <div class="col2">
            <table width="350px">
                <tr>
                    <td><asp:TextBox ID="txt_GuestNum" ReadOnly="true"  runat="server"   style= "text-align:right " CssClass="input" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))) {event.returnValue=true;} else{event.returnValue=false;}" >0</asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_ChildNum" ReadOnly="true" runat="server" CssClass="input" style= "text-align:right " OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))) {event.returnValue=true;} else{event.returnValue=false;}" >0</asp:TextBox>
                        
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <font style="font-size:10px;">&nbsp;(Aged 12 years and above)</font>
                    </td>
                    <td>
                        <font style="font-size:10px;">&nbsp;(Aged between 2 years and 11 years)</font>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <a href="javascript:void(0)" onclick="javascript:window.open('http://www.airasia.com/my/en/at-the-airport/special-guests.page','','width=600, height=500,scrollbars=yes')">
                            <font class="td3">Guest under 18 years old</font></a>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <font class="td3">&nbsp;(The maximum booking for seat flight is 50 pax.)</font>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
                
            </div>
        </li>
        </ul>
    </div>
    <table width="650px">
    <tr align="right">
    <td>

        <dx:ASPxButton CssClass="button-3"  
                            ID="btnSubMit" runat="server" Text="Search"  
            AutoPostBack="false">
                         <ClientSideEvents Click="function(s, e) {CallBackValidation.PerformCallback();
                                            LoadingPanel.SetText(&#39;Please Wait...&#39;);
                                           LoadingPanel.Show(); }" />
                        </dx:ASPxButton>
                        

    </td>                       
    </tr>
    </table>
   
</div>

<script type="text/javascript">
    //initDateControl();
</script>
</asp:Content>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder3" ></asp:Content>
