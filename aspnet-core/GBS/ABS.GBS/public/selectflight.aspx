<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="selectflight.aspx.cs" Inherits="GroupBooking.Web.Booking.SelectAllFlight" MasterPageFile="~/Master/NewPageMaster.Master" %>
<%@ MasterType  virtualPath="~/Master/PageMaster.Master"%>
<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %>
<%@ Register Assembly="SEAL.WEB" Namespace="SEAL.WEB.UI" TagPrefix="cc1" %>


<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolder2">
    <script type="text/javascript" src="../Scripts/jquery-1.4.1.js"></script>
   
    <script type="text/javascript">
        // <![CDATA[
        $(document).ready(function () {
            
            if (document.getElementById("<%= Hidden1.ClientID %>").value != "") {
                ShowLoginWindow(document.getElementById("<%= Hidden1.ClientID %>").value);
                document.getElementById("<%= Hidden1.ClientID %>").value = null;
            }
            //var id = getParameterByName('id');
            //if (id == "INFT") {
            //    if ($('#ctl00_ContentPlaceHolder3_SearchDatePanel_cb_OneWay').is(':checked')) {
            //        $('#ctl00_ContentPlaceHolder3_SearchDatePanel_tdReturn').hide();
            //    } else {
            //        $('#ctl00_ContentPlaceHolder3_SearchDatePanel_tdReturn').show();
            //    }

            //    alert("Cannot add infant");

                //document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = "hallo";

                //pcMessage.Show();
                //LoadingPanel.Hide();
        //    }

        });

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

        function OnCallbackComplete(s, e) {
            if (e.result != "" && e.result != "search") {
                //lblmsg.SetValue(e.result);
                if (e.result == "Your session has expired.") {
                    window.location.href = '../Invalid.aspx';
                }
                document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = e.result;
                pcMessage.Show();
                LoadingPanel.Hide();
            }
            else if (e.result == 'search') {
                window.location.href = '../public/selectflight.aspx';
            }
            else {
                window.location.href = '../public/reviewfare.aspx';
            }
        }

        function OnEndCallBackSearch(s, e) {
            if (typeof s.cp_result != 'undefined' && s.cp_result != "") {
                //lblmsg.SetValue(e.result);
                SelectFlightPanel.SetVisible(false);
                if (typeof btnContinueBottom != "undefined") {
                    btnContinueBottom.SetVisible(false);
                }
                if (typeof buttonL != "undefined") {
                    buttonL.SetVisible(false);
                }
                lblmsg.SetValue(s.cp_result);//document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = e.result;
                pcMessage.Show();
                LoadingPanel.Hide();             
            } else {
                window.location.href = '../public/selectflight.aspx?callback=1';
            }
        }

        //function onbtnSearchClick(s, e) {
        //   window.location.href = '../public/selectflight.aspx';
        //   LoadingPanel.Hide();
        //}
     
        function ShowLoginWindow(message) {
            alert(message);
            if ($('#ctl00_ContentPlaceHolder3_SearchDatePanel_cb_OneWay').is(':checked')) {
                $('#ctl00_ContentPlaceHolder3_SearchDatePanel_tdReturn').hide();
            } else {
                $('#ctl00_ContentPlaceHolder3_SearchDatePanel_tdReturn').show();
            }
            document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = message;
            pcMessage.Show();
            LoadingPanel.Hide();
        }

        function singleSelect(obj, dlistName) {
            //var elem = this.SkySales.elements; // obj.form.elements;
            //window.alert(obj);
            var elem = document.getElementById("aspnetForm");
            var datalistName = dlistName;
            var str;
            for (var i = 0; i < elem.length; i++) {
                //window.alert(elem[i].type);
                str += elem[i].id + "=" + obj.id + "," + elem[i].name.split('$')[3] + "=" + datalistName + "\n";
                //window.alert(elem[i].name.split('$')[3]);
                //window.alert(elem[i].id);
                if (elem[i].type == "radio" && elem[i].id != obj.id && elem[i].name.split('$')[3] == datalistName)// obj.name.subString( 0 , elem[i].name.indexOf('$') )
                {
                    elem[i].checked = false; //把不是触发click事件的radio状态设置为未选 12.       
                }
            }
            //            LoadingPanel.SetText('Please wait...');            
            //            LoadingPanel.Show();
            //            setTimeout(function () { LoadingPanel.Hide() }, 2000);

        }

        $(document).ready(function () {
            ValidateOneWay();
            //SelectedDate();
        });

        function ValidateOneWay() {
            //SelectedDate();
            if ($('#ctl00_ContentPlaceHolder3_SearchDatePanel_cb_OneWay').is(':checked')) {
                $('#ctl00_ContentPlaceHolder3_SearchDatePanel_tdReturn').hide();
                document.getElementById("<%= hOneWay.ClientID %>").value = 1;
            } else {
                $('#ctl00_ContentPlaceHolder3_SearchDatePanel_tdReturn').show();
                document.getElementById("<%= hOneWay.ClientID %>").value = 0;
            }
        }

        function UpdateInfo() {
            //var daysTotal = daEnd.GetRangeDayCount();
            daEnd.Focus();
            daEnd.ShowDropDown();
            daStart.HideDropDown();
            //tbInfo.SetText(daysTotal !== -1 ? daysTotal + ' days' : '');
        }

        function UpdateInfoEnd() {
            //var daysTotal = daEnd.GetRangeDayCount();
            //alert('dateEnd');
            daEnd.Blur();
            daEnd.HideDropDown();
            daStart.HideDropDown();
            //tbInfo.SetText(daysTotal !== -1 ? daysTotal + ' days' : '');
        }

        function onChangeReturnSta() {
            
            var ddl = document.getElementById("<%= ddlReturn.ClientID %>");
            var Text = ddl.options[ddl.selectedIndex].text;
            var Value = ddl.options[ddl.selectedIndex].value;
            //alert(Text + " - " + Value);
            document.getElementById("<%= hfArrival.ClientID %>").value = Value;
            document.getElementById("<%= hfArrivalText.ClientID %>").value = Text;
        }

        var numCallback = 0;
     
        // ]]> 
    </script>

    <msg:msgControl ID="msgcontrol" runat="server" />
    <div>
    <dx:ASPxCallback ID="CallBackValidation" ClientInstanceName="CallBackValidation" runat="server" OnCallback="ValidatePopup">
            <ClientSideEvents CallbackComplete="OnCallbackComplete" />
        </dx:ASPxCallback>
    </div>

<div>
    <div class="">
        
    <div class="row page-header">
        <div class="col-sm-4">
            <h4 class="mt-0 mb-5">Flight Search</h4>
            Booking/Flight Search
        </div>
        <div class="col-sm-8">
            <div align="right" style="padding-top: 9px;" >
                <table id="bookingDetail">
                    <tr>
                        <td>
                            <dx:ASPxButton CssClass="buttonL" ID="btn_Next" runat="server" ClientInstanceName="buttonL" Text="Continue" AutoPostBack="False" Visible="false" >
                                <ClientSideEvents Click="function(s, e) {CallBackValidation.PerformCallback();
                                LoadingPanel.SetText(&#39;Please Wait...&#39;);
                                LoadingPanel.Show(); }" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </div>
        </div>

    </div>

        <div class="row" style="">
            <!--temporary remark-->
       <%--     <table style="width:100%;">
                <tr>
                        <td >
                            <div data-toggle="collapse" data-target="#AdvanceSearchForm" style="width:100%;cursor:pointer;background:#f8f8f8;">
                            <span style="display:inline-block;">
                        <div style="font-size:20px;padding-left:10px;" class="fa fa-search-plus"></div>
                            </span>
                            <span style="display:inline-block;">
                        <h4 class="mt-0 mb-5" style="padding: 12px;">Advanced Search</h4>
                                </span>
                        </div>
                    </td>
                       
                </tr>
            </table>--%>

            <div id="AdvanceSearchForm" class="collapse" style="display:none" >

                <div class="col-md-6">
                    <table>
                        <tr>
                            <td>
                                <div class="checkbox">
                                <label><input type="checkbox" value="">Search by Month</label>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="col-md-6">
                    <table>
                        <tr>
                            <td>
                                <div class="checkbox">
                                <label><input type="checkbox" value="">Same Day Every Month</label>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div class="page-content row">
            <div class="col-md-12">
                <dx:ASPxCallbackPanel ID="SelectFlightPanel" runat="server" RenderMode="Div" ClientVisible="true" Height="100%" SettingsLoadingPanel-ShowImage="false"
                    OnCallback="SelectFlightPanel_Callback" ClientInstanceName="SelectFlightPanel">
                    <SettingsLoadingPanel Enabled="false" />
                    <ClientSideEvents EndCallback="function(s, e){ OnEndCallBackSearch(s, e); }"></ClientSideEvents>
                    <PanelCollection>
                        <dx:PanelContent>
                            <!--Departure-->
                            
                            <div class="availFlight tableStyle bodRad-4 col-sm-12 " runat="server" id="tr_Depart" style="display:none">
                                <div id="departureFlightHeader" class="flightHeader">
                                    <div class="headerPlace">
                                        <h1 class="headerPlaceText">Depart
                                        </h1>
                                    </div>
                                    <div class="flightRoute">
                                        <span class="routeText"><asp:Label ID="lbl_Go1" runat="server"></asp:Label></span>
                                        <span class="arrowTo">
                                            <span class="fa fa-arrow-right"></span>
                                        </span>
                                        <span class="routeText"><asp:Label ID="lbl_Go2" runat="server"></asp:Label></span>
                                    </div>
                                </div>
                                <ul class="weeklyFlightList" id="departDateDiv" runat="server">
                                    
                                </ul>
                                <dx:ASPxDataView ID="dvSelectFlight" runat="server" Width="100%" PagerSettings-Visible="false" ColumnCount="1" Height="50px" AllowPaging="False" PagerAlign="Left" CssClass="tdClass">
                                    <PagerSettings Visible="False"></PagerSettings>

                                    <ItemStyle Height="0px" HorizontalAlign="Left" Wrap="True" Width="100%" >
                                    <Paddings PaddingLeft="1px" PaddingRight="1px" PaddingTop="5px" PaddingBottom="5px"></Paddings>
                                            
                                    </ItemStyle>
                                    <ItemTemplate>
                                        <table width="100%"  cellpadding="0" cellspacing="0">
                                        <tbody>
                                        <tr >                                      
                                            <td align="left" colspan="2">&nbsp;&nbsp;&nbsp;
                                                <%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightCarrierCode").ToString()%>
                                                &nbsp;<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightFlightNumber").ToString()%>&nbsp;&nbsp;<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightCarrierCode2").ToString()%>&nbsp;<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightFlightNumber2").ToString()%></td>
                                            <td align="left">                                                            
                                                <%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightDeparture").ToString()%>                                                                    
                                            </td>
                                            <td align="left">
                                                <%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightStd", "{0:dddd, dd MMMM yyyy}")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightStd", "{0:HHmm}")%></td>
                                            <td align="left">
                                                <%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightArrival").ToString()%>                                                            
                                            </td>
                                            <td align="left">
                                            <%# (DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightTransit").ToString() != "") ? DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightSta2", "{0:dddd, dd MMMM yyyy}") : DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightSta", "{0:dddd, dd MMMM yyyy}")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%# (DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightTransit").ToString() != "") ? DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightSta2", "{0:HHmm}") : DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightSta", "{0:HHmm}")%></td>
                                            <td>
                                                <%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "Insufficient").ToString() != ""? 
                                                            DataBinder.Eval(((IDataItemContainer)Container).DataItem, "Insufficient").ToString():
                                                            ""  %>
                                                <asp:RadioButton ID="RadioButton1" Visible='<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "Insufficient").ToString() != ""?false:true %>' runat="server" Checked="false" Text="" GroupName="grb1" onclick="javascript:singleSelect(this,'dvSelectFlight')" />
                                                <asp:HiddenField ID="lbl_list1ID" runat="server" Value='<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightId").ToString()%>'></asp:HiddenField>
                                            </td>
                                        </tr>
                                        </tbody> 
                                        </table>
                                    </ItemTemplate>
                                    
                                </dx:ASPxDataView>
                                <div id="fg_373">
                                </div>
                                <!-- remark by ketee -->
                                <%--<div id="fg_374">
                                    <input id="INPUT_375" value="true" type="checkbox" name="airAsiaAvailability.MarketValueBundles[0]">
                                    <input type="hidden" name="airAsiaAvailability.MarketValueBundles[0]" value="false" id="INPUT_376">
                                </div>--%>

                            </div>
                            <!--Return-->
                            <div class="availFlight tableStyle bodRad-4 col-sm-12" id="tr_Return" runat="server" style="display:none">
                                <div id="Div4" class="flightHeader">
                                    <div class="headerPlace">
                                        <h1 class="headerPlaceText">Return
                                        </h1>
                                    </div>
                                    <div class="flightRoute">
                                        <span class="routeText"><asp:Label ID="lbl_Return1" runat="server"></asp:Label></span>
                                        <span class="arrowTo">
                                            <span class="fa fa-arrow-right"></span>
                                        </span>
                                        <span class="routeText"><asp:Label ID="lbl_Return2" runat="server"></asp:Label></span>
                                    </div>
                                </div>
                                <ul class="weeklyFlightList" id="returnDateDiv" runat="server">
                                    
                                </ul>
                                <dx:ASPxDataView ID="gvSelectFlightReturn" runat="server" Width="100%" PagerSettings-Visible="false" ColumnCount="1" Height="50px" AllowPaging="False" PagerAlign="Left" CssClass="tdClass">
                                    <PagerSettings Visible="False"></PagerSettings>

                                    <ItemStyle Height="0px" HorizontalAlign="Left" Wrap="True" Width="100%" >
                                    <Paddings PaddingLeft="1px" PaddingRight="1px" PaddingTop="5px" PaddingBottom="5px"></Paddings>
                                            
                                    </ItemStyle>
                                    <ItemTemplate>
                                        <table width="100%"  cellpadding="0" cellspacing="0">
                                            <tbody>
                                                <tr >
                                                    <td align="left" colspan="2">&nbsp;&nbsp;&nbsp;
                                                        <%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightCarrierCode").ToString()%>
                                                        &nbsp;<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightFlightNumber").ToString()%>&nbsp;&nbsp;<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightCarrierCode2").ToString()%>&nbsp;<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightFlightNumber2").ToString()%></td>
                                                    <td align="left" >
                                                        <%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightDeparture").ToString()%></td>
                                                    <td align="left">
                                                        <%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightStd", "{0:dddd, dd MMMM yyyy}")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightStd", "{0:HHmm}")%></td>
                                                    <td align="left" >
                                                        <%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightArrival").ToString()%></td>
                                                    <td align="left">
                                                        <%#(DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightTransit").ToString() != "") ? DataBinder.Eval(Container.DataItem, "TemFlightSta2", "{0:dddd, dd MMMM yyyy}") : DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightSta", "{0:dddd, dd MMMM yyyy}")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%#(DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightTransit").ToString() != "") ? DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightSta2", "{0:HHmm}") : DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightSta", "{0:HHmm}")%></td>
                                                    <td >
                                                        <%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "Insufficient").ToString() != ""? DataBinder.Eval(((IDataItemContainer)Container).DataItem, "Insufficient").ToString(): ""  %>
                                                        <asp:RadioButton ID="RadioButton2" runat="server" Visible='<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "Insufficient").ToString() != ""? false: true %>' Checked="false" GroupName="grb2" Text="" OnClick="javascript:singleSelect(this,'gvSelectFlightReturn')" />
                                                        <asp:HiddenField ID="lbl_list2ID" runat="server" Value='<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightId").ToString()%>'></asp:HiddenField>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </ItemTemplate>
                                </dx:ASPxDataView>
                            </div>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxCallbackPanel>
            </div>
        </div>

        <div class="row page-header clearfix">
            <div class="col-sm-6">

            </div>
            <div class="col-sm-6">
             <div style="float: right">
                <dx:ASPxButton ID="btnContinueBottom" runat="server" ClientInstanceName="btnContinueBottom" 
                    CssClass="buttonL" Text="Continue" AutoPostBack="false" Visible="false" >
                    <ClientSideEvents Click="function(s, e) {CallBackValidation.PerformCallback();
                                LoadingPanel.SetText(&#39;Please Wait...&#39;);
                                LoadingPanel.Show(); }" />
                </dx:ASPxButton>
            </div>
            </div>
        </div>
    </div>

</div>
    

</asp:Content>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder3" >
    <link rel="Stylesheet" href="../Styles/chosen.css" />
    
    <dx:ASPxCallbackPanel ID="SearchDatePanel" runat="server" RenderMode="Div" ClientVisible="true" Height="100%" ClientInstanceName="SearchDatePanel" OnCallback="SearchDate_Callback">
        <ClientSideEvents EndCallback="function(s, e){ formatDDL(); }"></ClientSideEvents>
        <SettingsLoadingPanel Enabled="false" />
        <PanelCollection>
            <dx:PanelContent>
                <div class="padd10" style="background: transparent; padding-bottom: 100px; color: #fff;">
                    <div id="searchFlightForm" class="collapse in sidebar-widget">
                        <ul class="list-unstyled pr-25">
                            <li>
                                <div style="width: 100%;text-align:center;">
                                    <h4 style="margin-top: 0; display: inline-block;">Flight Search</h4>

                                </div>
                                <div class="block clearfix mb-10" style="width: 190px;">
                                     <table id="topPartSearchForm">

                                        <tr>
                                            <td colspan="3"><span style="font-size: 12px; display: block; margin: 10px 0;">
                                                Booking is now available up to 4hrs before flights
                                                </span>
                                                <div runat="server" id="divRestriction" style="display:none; color:Red; font-weight:bold" class="search-form-top">
                                                    <dx:ASPxLabel ID="lblRestriction" runat="server"></dx:ASPxLabel>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="search-form-top">
                                                    <dx:ASPxLabel runat="server" ID="lblMsg" ClientInstanceName="lblMsg" Text="" ForeColor="Red"></dx:ASPxLabel>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="alignRadiobtn">
                                                <input type="checkbox" runat="server" name="cb_OneWay" id="cb_OneWay" onchange="javascript:ValidateOneWay();" value="One Way Only" />One Way Only
                                            </td>
                                            <td class="alignRadiobtn">
                                    
                                            </td>

                                        </tr>
                                    </table>
                                </div>
                            </li>
                            <li>
                                <div>
                        
                                </div>
                            </li>
                            <li>
                                <div class="block clearfix mb-10" style="width: 190px;">
                                    <cc1:customdropdownlist ID="ddlDeparture" runat="server" Width="184px"
                                        AutoPostBack="false" CssClass="chzn-select" TabIndex="1">
                                        <asp:ListItem Text="Origin" ></asp:ListItem>
                                        </cc1:customdropdownlist>
                                </div>
                            </li>
                            <li>
                                <div class="block clearfix mb-10" style="width: 190px;">
                                     <table>
                                        <tr>
                                            <td valign="top">
                                                <div class="controlledCollapse collapse in">
                                                    <cc1:customdropdownlist ID="ddlReturn" runat="server" Width="184px" CssClass="chzn-select">
                                                    <asp:ListItem Text="Destination"  TabIndex="2"></asp:ListItem>
                                                    </cc1:customdropdownlist> 
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" style="padding-top: 9px;">
                                                <div class="coverDropDown">
                                                <asp:DropDownList ID="ddl_Currency" runat="server" 
                                                    Enabled="False" Width="184px">
                                                    <asp:ListItem Value="USD">Select currency</asp:ListItem>
                           
                                                </asp:DropDownList>
                                                </div>
                                            </td>
                                        </tr>

                                    </table>
                                </div>
                            </li>
                            <li>
                                <div class="block clearfix mb-10" style="width: 190px;">
                                    <div id="marketDate_1" class="depart-date left black3" style="display:none;">
                                         <div id="divDate1" runat="server">
                                        </div>
                                    </div>
                                    <dx:ASPxDateEdit ID="daStart" ClientInstanceName="daStart" runat="server" NullText="Depart" OnInit="daStart_Init" EditFormatString="dd/MM/yyyy" Width="184px">
                                        <ClientSideEvents DateChanged="function(s,e) {UpdateInfo();}"></ClientSideEvents>
                                        <ValidationSettings Display="Dynamic" ValidationGroup="SearchPanel" SetFocusOnError="True" CausesValidation="True" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField IsRequired="True" ErrorText="Start date is required"></RequiredField>
                                        </ValidationSettings>
                                    </dx:ASPxDateEdit>
                                </div>
                            </li>
                            <li id="tdReturn" runat="server">
                                <div class="block clearfix mb-10" style="width: 190px;">
                                <div id="marketDate_2" class="depart-date left black3" style="display:none;">
                                                    <div id="divDate2" runat="server">
                                                    </div>
                                                </div>
                                    <dx:ASPxDateEdit ID="daEnd" ClientInstanceName="daEnd" runat="server" NullText="Return" EditFormatString="dd/MM/yyyy" Width="184px">
                                        <DateRangeSettings StartDateEditID="daStart"></DateRangeSettings>
                                        <ClientSideEvents DateChanged="function(s,e) {UpdateInfoEnd();} "></ClientSideEvents>
                                        <ValidationSettings Display="Dynamic" ValidationGroup="SearchPanel" SetFocusOnError="True" CausesValidation="True" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField IsRequired="True" ErrorText="End date is required"></RequiredField>
                                        </ValidationSettings>
                                    </dx:ASPxDateEdit>
                                </div>
                            </li>
                            <li runat="server" visible="false">
                                <div class="block clearfix mb-10" style="width: 190px;">
                                    <dx:ASPxTextBox runat="server" ID="txt_PromoCode" NullText="Enter Promo Code Here" Width="184px" Visible="true" ></dx:ASPxTextBox>
                                </div>
                            </li>
                            <li>
                                <div class="block clearfix mb-10">
                                      <table>
                                        <tr>
                                            <td>
                                                <table id="numOfPeopleTable">
                                                    <tr>
                                                        <td id="guestNumTd">
                                                            <%--<asp:TextBox ID="txt_GuestNum"  runat="server" style= "text-align:right " CssClass="input" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))) {event.returnValue=true;} else{event.returnValue=false;}" ></asp:TextBox>--%>
                                                            <dx:ASPxSpinEdit NumberType="Integer" Width="90px" CssClass="input-sm" HorizontalAlign="Right"  AllowMouseWheel="true" AllowUserInput="true" SpinButtons-ShowIncrementButtons="false"
                                                                runat="server" ID="txt_GeustNum" NullText="Adult" MinValue="0" MaxValue="50" ></dx:ASPxSpinEdit>
                                                        </td>


                                                        <td id="childNumTd" style="padding: 5px;">
                                                            <%--<asp:TextBox ID="txt_ChildNum"   runat="server" CssClass="input" style= "text-align:right " OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))) {event.returnValue=true;} else{event.returnValue=false;}" ></asp:TextBox>--%>
                                                            <dx:ASPxSpinEdit NumberType="Integer" Width="90px" CssClass="input-sm" HorizontalAlign="Right" AllowMouseWheel="true" AllowUserInput="true" SpinButtons-ShowIncrementButtons="false"
                                                                runat="server" ID="txt_ChildNum" NullText="Child" MinValue="0" MaxValue="50" ></dx:ASPxSpinEdit>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id="Td1">
                                                            <%--<asp:TextBox ID="txt_InfantNum"   runat="server" CssClass="input" style= "text-align:right " OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))) {event.returnValue=true;} else{event.returnValue=false;}" ></asp:TextBox>--%>
                                                            <dx:ASPxSpinEdit NumberType="Integer" Width="90px" CssClass="input-sm" HorizontalAlign="Right"  AllowMouseWheel="true" AllowUserInput="true" SpinButtons-ShowIncrementButtons="false" ClientVisible="false"
                                                                runat="server" ID="txt_InfantNum" NullText="Infant" MinValue="0" MaxValue="10" ></dx:ASPxSpinEdit>
                                                        </td>
                                                        <td align="right" valign="bottom" style="padding: 5px;">
                                                            <dx:ASPxButton CssClass="button-3" ID="btnSubMit" runat="server" Text="Search"  
                                                                    AutoPostBack="false">
                                                             <ClientSideEvents Click="function(s, e) {SelectFlightPanel.PerformCallback('search');
                                                                                LoadingPanel.SetText(&#39;Please Wait...&#39;);
                                                                               LoadingPanel.Show(); }" />
                                                            </dx:ASPxButton>
                                                
                                                        </td>
                                                    </tr>
                                                </table>

                                            </td>

                                        </tr>
                                    </table>
                                </div>
                            </li>
                        </ul>

                        <input type="hidden" id="Hidden1" runat="server" />
                        <input type="hidden" id="hfDeparture" runat="server" />
                        <input type="hidden" id="hfArrival" runat="server" />
                        <input type="hidden" id="hfArrivalText" runat="server" />
                        <input type="hidden" id="hfCurrency" runat="server" />
                        <input type="hidden" id="hfdeStart" runat="server" />
                        <input type="hidden" id="hfdeEnd" runat="server" />
                        <input type="hidden" id="hfAdultNum" runat="server" />
                        <input type="hidden" id="hfChildNum" runat="server" />
                        <input type="hidden" id="hfInfantNum" runat="server" />
                        <input type="hidden" id="hOneWay" runat="server" />
                        <input type="hidden" id="hfPromoCode" runat="server" />
                    </div>
                </div>
                
                <script id="dxss_chosen" type="text/javascript" src="../Scripts/chosen.jquery.js"></script>
                <script type="text/javascript">
                    $(document).ready(function () {
                        var partToHighLight = null;
                        var sameGrid = null;

                        $('[type="radio"]').click(function () {
                            partToHighLight = $(this).parent().parent().parent().parent().parent(); //get the active row to be styled
                            var thisGridWrapper = partToHighLight.parent().parent().parent().parent();

                            if (thisGridWrapper.find('.selectedFlight').length != 0) thisGridWrapper.find('.selectedFlight').removeClass('selectedFlight');

                            if ($(this).is(':checked')) {
                                partToHighLight.addClass('selectedFlight');
                                sameGrid = partToHighLight;
                            }


                        });
                    });

                    $(".chzn-select").chosen();
                    $(".chzn-select-deselect").chosen({
                        allow_single_deselect: true
                    });

                    function formatDDL() {
                        if (typeof SearchDatePanel.cp_result != "undefined")
                        {
                            alert("a");
                        }
                        ValidateOneWay();
                        $(".chzn-select").chosen();
                        $(".chzn-select-deselect").chosen({
                            allow_single_deselect: true
                        });
                        //$.getScript('../Scripts/chosen.jquery.js', function () {
                        //    $(".chzn-select").chosen();
                        //    $(".chzn-select-deselect").chosen({
                        //        allow_single_deselect: true
                        //    });
                        //});
                    }
                </script>

            </dx:PanelContent>        
        </PanelCollection>
    </dx:ASPxCallbackPanel>    
</asp:Content>