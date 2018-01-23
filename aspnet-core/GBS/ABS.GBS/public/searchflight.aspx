<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="searchflight.aspx.cs" Inherits="GroupBooking.Web.SearchFlight" MasterPageFile="~/Master/NewPageMaster.Master" %>

<%@ Register Assembly="SEAL.WEB" Namespace="SEAL.WEB.UI" TagPrefix="cc1" %>
<%@ MasterType  virtualPath="~/Master/NewPageMaster.Master"%>
<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %>
<%@ Register Src="../UserControl/agentmain.ascx" TagName="ucagentmain" TagPrefix="AGT" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder3" >
    
<script type="text/javascript" src="../Scripts/jquery-1.4.1.js"></script>
<script type="text/javascript" src="../Scripts/Calendar.js"></script>

<!-- Main Sidebar start-->
      <aside class="main-sidebar">
        <div class="user">
          <div id="companyLogo"><img src="../images/airasia/aalogo.png" alt="" class="avatar img-circle mCS_img_loaded" data-pin-nopin="true"/></div>
          <h4 class="fs-16 text-white mt-15 mb-5 fw-300"><dx:ASPxLabel runat="server" ID="lblAgentName"></dx:ASPxLabel></h4>
          <p class="mb-0 text-muted"><dx:ASPxLabel runat="server" ID="lblAgentOrg" ></dx:ASPxLabel></p>
        </div>
        <dx:ASPxLabel runat="server" ID="lbltest" ></dx:ASPxLabel>
        <div class="sidebar-category">Email Address</div>
        <div class="sidebar-widget">
          <ul class="list-unstyled pl-25 pr-25">
            <li class="mb-20">
              <div class="block clearfix mb-10"><span class="pull-left fs-12 text-muted"><dx:ASPxLabel ID="lblAgentEmail" runat="server" ></dx:ASPxLabel></span>
                <%--<span class="pull-right label label-outline label-warning">65%</span></div>
                <div class="progress progress-xs bg-light mb-0">
                <div role="progressbar" data-transitiongoal="65" class="progress-bar progress-bar-warning"></div>--%>
                </div>
            </li>
            <%--<li class="mb-20">
              <div class="block clearfix mb-10"><span class="pull-left fs-12 text-muted">Bandwidth</span>
                <span class="pull-right label label-outline label-danger">80%</span></div>
              <div class="progress progress-xs bg-light mb-0">
                <div role="progressbar" data-transitiongoal="80" class="progress-bar progress-bar-danger"></div>
              </div>
            </li>--%>
          </ul>
        </div>
        <div class="sidebar-category">Contact Number</div>
        <div class="sidebar-widget">
          <ul class="list-unstyled pl-25 pr-25">
            <li class="mb-20">
              <div class="block clearfix mb-10"><span class="pull-left fs-12 text-muted"><dx:ASPxLabel runat="server" ID="lblContact"></dx:ASPxLabel></span>
              </div>
            </li>
        </ul>
        </div>
        <div class="sidebar-category">Available Credit [<dx:ASPxLabel runat="server" ID="lblAGCurr" ></dx:ASPxLabel>]</div>
        <div class="sidebar-widget">
          <ul class="list-unstyled pl-25 pr-25">
            <li class="mb-20">
              <div class="block clearfix mb-10"><span class="pull-left fs-12 text-muted"><dx:ASPxLabel runat="server" ID="lblAGLimit" ></dx:ASPxLabel></span>
                  <%--<span class="pull-right label label-outline label-warning">65%</span>
                  <div role="progressbar" data-transitiongoal="65" class="progress-bar progress-bar-warning"></div>--%>
              </div>
            </li>
        </ul>
        </div>
      </aside>

<%--<script type="text/javascript">

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
        var dd = $('#ctl00_ContentPlaceHolder3_hDepart').val();
        var dr = $('#ctl00_ContentPlaceHolder3_hReturn').val();

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

        //         document.getElementById('ddlMarketDay1').value = date1.getDate();
        //         document.getElementById('ddlMarketDay2').value = date2.getDate();

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

        var j = 1;

        var marketDateIdArray = [];
        marketDateIdArray[0] = { "marketDateId": "date_picker_id_1", "marketDayId": "ddlMarketDay1", "marketMonthYearId": "ddlMarketMonth1" };
        var _Return = document.getElementById('<%= tdReturn.ClientID %>');
        if (_Return != null) {
            //document.getElementById('ddlMarketDay2').value = date2.getDate();

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

        for (i = 0; i < j; i++) {
            var marketDateId = marketDateIdArray[i];
            var marketDate = new SKYSALES.Class.MarketDate();
            marketDate.marketDateCount = j;
            marketDate.init(marketDateId);
        }
    }

    function OnCallbackComplete(s, e) {
        //window.alert(e.result);
        if (e.result != "") {
            //lblmsg.SetValue(e.result);
            lblmsg.SetValue(e.result);//document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = e.result;
            pcMessage.Show();
            LoadingPanel.Hide();
        } else {
            window.location.href = '../public/selectflight.aspx';
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
        if ($('#ctl00_ContentPlaceHolder3_cb_OneWay').is(':checked')) {
            $('#ctl00_ContentPlaceHolder3_tdReturn').hide();
        } else {
            $('#ctl00_ContentPlaceHolder3_tdReturn').show();
        }
    }



    // ]]> 

    function UpdateInfo() {
        var daysTotal = daEnd.GetRangeDayCount();
        daEnd.ShowDropDown();
        //tbInfo.SetText(daysTotal !== -1 ? daysTotal + ' days' : '');
    }

    function UpdateInfoEnd() {
        var daysTotal = daEnd.GetRangeDayCount();
        daEnd.HideDropDown();
        //tbInfo.SetText(daysTotal !== -1 ? daysTotal + ' days' : '');
    }
    </script>--%>

<msg:msgControl ID="msgcontrol" runat="server" />
<input type="hidden" id="hDepart" runat="server" value="" />
<input type="hidden" id="hReturn" runat="server" value="" />


<div>
    <%--<dx:ASPxCallback ID="CallBackValidation" ClientInstanceName="CallBackValidation" runat="server" OnCallback="ValidatePopup">
        <ClientSideEvents CallbackComplete="OnCallbackComplete" />
    </dx:ASPxCallback>--%>
    <%--<div class="padd10" style="background: transparent; padding-bottom: 100px; color: #fff;">
        <div id="searchFlightForm" class="collapse in sidebar-widget">
            <ul class="list-unstyled pl-25 pr-25">
                <li>
                    <div style="width: 100%;text-align:center;">
                        <h4 style="margin-top: 0; display: inline-block;">Flight Search</h4>

                    </div>
                    <div class="block clearfix mb-10" style="width: 190px;">
                         <table id="topPartSearchForm">

                            <tr>
                                <td colspan="3"><span style="font-size: 12px; display: block; margin: 10px 0;">
                                    Online Booking is now available up to 2hrs before flights
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
                            onselectedindexchanged="ddlDeparture_SelectedIndexChanged" 
                            AutoPostBack="true" CssClass="chzn-select">
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
                                        <cc1:CustomDropDownList ID="ddlReturn" runat="server" Width="184px" CssClass="chzn-select">
                                        <asp:ListItem Text="Destination"></asp:ListItem>
                                        </cc1:CustomDropDownList> 
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="padding-top: 9px;">
                                    <asp:DropDownList ID="ddl_Currency" runat="server" 
                                        Enabled="False" Width="184px">
                                        <asp:ListItem Value="USD">Select currency</asp:ListItem>
                           
                                    </asp:DropDownList>
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
                        <dx:ASPxDateEdit ID="daStart" ClientInstanceName="daStart" runat="server" NullText="Depart" EditFormatString="dd/MM/yyyy" Width="184px">
                            <ClientSideEvents DateChanged="UpdateInfo"></ClientSideEvents>
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
                            <ClientSideEvents DateChanged="UpdateInfoEnd"></ClientSideEvents>
                            <ValidationSettings Display="Dynamic" ValidationGroup="SearchPanel" SetFocusOnError="True" CausesValidation="True" ErrorDisplayMode="ImageWithTooltip">
                                <RequiredField IsRequired="True" ErrorText="End date is required"></RequiredField>
                            </ValidationSettings>
                        </dx:ASPxDateEdit>
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
                                                <asp:TextBox ID="txt_GuestNum"  runat="server" NullText="Adult"  style= "text-align:right " CssClass="input" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))) {event.returnValue=true;} else{event.returnValue=false;}" ></asp:TextBox>
                                            </td>


                                            <td id="childNumTd" style="padding: 5px;">
                                                <asp:TextBox ID="txt_ChildNum" NullText="Child"  runat="server" CssClass="input" style= "text-align:right " OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))) {event.returnValue=true;} else{event.returnValue=false;}" ></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="Td1">
                                                <asp:TextBox ID="txt_InfantNum" NullText="Infant"  runat="server" CssClass="input" style= "text-align:right " OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))) {event.returnValue=true;} else{event.returnValue=false;}" ></asp:TextBox>
                                            </td>
                                            <td align="right" valign="bottom" style="padding: 5px;">
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

                                </td>

                            </tr>
                        </table>
                    </div>
                </li>
            </ul>

            <input type="hidden" id="hfDeparture" runat="server" />
            <input type="hidden" id="hfArrival" runat="server" />
            <input type="hidden" id="hfCurrency" runat="server" />
            <input type="hidden" id="hfdeStart" runat="server" />
            <input type="hidden" id="hfdeEnd" runat="server" />
            <input type="hidden" id="hfAdultNum" runat="server" />
            <input type="hidden" id="hfChildNum" runat="server" />
            <input type="hidden" id="hfInfantNum" runat="server" />
        </div>
    </div>--%>
              <%--  </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>--%>
</div>
<script type="text/javascript" src="../Scripts/chosen.jquery.js"></script>
<link rel="Stylesheet" href="../Styles/chosen.css" />

<script type="text/javascript">
    $(".chzn-select").chosen();
    $(".chzn-select-deselect").chosen({
        allow_single_deselect: true
    });
</script>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolder2">
    <AGT:ucagentmain ID="ucagentmain" runat="server" />
</asp:Content>


