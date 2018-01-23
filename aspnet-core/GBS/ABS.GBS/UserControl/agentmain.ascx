<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="agentmain.ascx.cs" Inherits="ABS.GBS.UserControl.agentmain" %>

<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v16.1" %>
<script language="javascript" type="text/javascript" >
    var loaded = 0;

    //function loadJSON() {
    //    $.ajax({
    //        url: '../frame.aspx',
    //        success: function () {
    //            //alert('Hopla!');
    //        }
    //    });
    //}

    //function test() {
    //    alert("test");
    //}

    //$(document).ready(function () {
    //    loadJSON();
    //});

    function OnCallbackComplete(s, e) {
        if (s.cpIsUpdated != undefined && s.cpIsUpdated != "") {
            document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = s.cpIsUpdated;
            s.cpIsUpdated = "";
            pcMessage.Show();
            LoadingPanel.Hide();
        }
    }
    function OnCallbackBegin(s, e) {
        s.cpIsUpdated = "";
    }

</script>

<msg:msgControl ID="msgcontrol" runat="server" />
<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="true" ContainerElementID="FormPanel"/>

<div id="mainContent" class="widget page-content container-fluid" style="">
<div class="div">
            <%--<dx:ASPxLabel ID="lblHeader" Text="My Booking" runat="server" 
                Font-Bold="True" Font-Size="Medium"></dx:ASPxLabel></div>
                <br />
            <dx:ASPxButton runat="server" ID="btnTopup" Visible="true" text="Top Up"
                onclick="btnTopup_Click"></dx:ASPxButton>
            <div> --%>
            <%--<table>
                <tr>
                    <td>
                        <dx:ASPxTextBox ID="txtPNR" runat="server" Width="170px">
                            </dx:ASPxTextBox>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <dx:ASPxButton ID="btnCancelBooking" runat="server" Text="Cancel Booking" 
                                onclick="btnCancelBooking_Click">
                            </dx:ASPxButton>
                    </td>
                </tr>
            </table>--%>

</div>
    <div id="errorDiv">
        <dx:ASPxLabel ID="lblErr" runat="server" Text="" ForeColor="Red"></dx:ASPxLabel>
    </div>
    
    <div id="topPanelsWrapper" class="row">
        <div class="col-md-2 col-sm-6">
            <a id="A3" runat="server" href="../public/bookingmanagement.aspx" onserverclick="searchExpiryLink_Click"  >
                <div class="widget no-border bg-success clearfix p-15">
                    <div class="pull-left">
                        <h5 class="m-0 text-uppercase">Upcoming Booking Expiry
                        </h5>
                        <div class="fs-18 fw-600 boldingText">
                            <dx:ASPxLabel ID="lblExpiryDate" runat="server" Text="" ClientInstanceName="lblExpiryDate" Visible="true"/>
                        </div>
                        <div class="fs-18 fw-600 boldingText">
                            <dx:ASPxLabel ID="lblBookingExpiry" runat="server" Text="" ClientInstanceName="lblBookingExpiry"/>
                        </div>
                        <div class="desktopIcon">
                           <%-- <div class="ti-agenda"></div>--%>
                        </div>
                    </div>
                    <%--<div id="sp-bar" class="pull-right"></div>--%>
                </div>
            </a>
        </div>
        
        <div class="col-md-2 col-sm-6">
            <a id="A1" runat="server" href="../public/bookingmanagement.aspx" onserverclick="searchPendingLink_Click"  >
                <div class="widget no-border bg-danger clearfix p-15">
                    <div class="pull-left">
                        <h5 class="m-0 text-uppercase">Pending Payment</h5>
                        <div class="fs-18 fw-600">
                           
                        </div>

                        <%-- 20170404 - Sienny (Rest Amount) --%>
                        <div class="fs-18 fw-600">
                            <dx:ASPxLabel ID="lblRestAmount" runat="server" Text="" ClientInstanceName="lblRestAmount"/>
                        </div>
                        <div class="desktopIcon">
                         <dx:ASPxLabel ID="lblPendingPayment" runat="server" Text="" ClientInstanceName="lblPendingPayment"/>
                        <%--<div class="ti-wallet"></div>--%>
                        </div>
                    </div>
                    <%--<div id="sp-line" class="pull-right"></div>--%>
                </div>
            </a>
        </div>
        
        <div class="col-md-2 col-sm-6">
            <a id="seachLink" runat="server" href="../public/bookingmanagement.aspx" onserverclick="searchLink_Click"  >
                <div class="widget no-border bg-info clearfix p-15">
                    <div class="pull-left">
                        <h5 class="m-0 text-uppercase">Pending Passenger Upload</h5>
                        <div class="fs-18 fw-600">
                            
                        </div>
                        <div class="desktopIcon">
                        <dx:ASPxLabel ID="lblPendingPassUpload" runat="server" Text="" ClientInstanceName="lblPendingPassUpload"/>
                      <%--  <div class="ti-user"></div>--%>
                        </div>
                    </div>
                    <%--<i class="fa fa-users fs-36 pull-right"></i>--%>
                </div>
            </a>
        </div>
        
        <%--20170331-Sienny (Upcoming Flight) --%>
        <div class="col-md-2 col-sm-6">
            <a id="searchUpcoming" runat="server" href="../public/bookingmanagement.aspx" onserverclick="searchUpcomingLink_Click"  >
                <div class="widget no-border bg-warning clearfix p-15">
                    <div class="pull-left">
                        <h5 class="m-0 text-uppercase">CONFIRMED</h5><%--Upcoming Flight--%>
                        <div class="fs-18 fw-600">
                           
                        </div>
                        <div class="desktopIcon">
                         <dx:ASPxLabel ID="lblUpcomingFlight" runat="server" Text="" ClientInstanceName="lblUpcomingFlight"/>
                        <%--<div class="ti-check-box"></div>--%>
                        </div>
                    </div>
                    <%--<i class="ti-facebook fs-36 pull-right"></i>--%>
                </div>
            </a>
        </div>
        
        <div class="col-md-2 col-sm-6">
            <a id="A2" runat="server" href="../public/bookingmanagement.aspx" onserverclick="searchCancelLink_Click"  >
                <div class="widget no-border bg-primary clearfix p-15">
                    <div class="pull-left">
                        <h5 class="m-0 text-uppercase">Cancellation</h5>
                        <div class="fs-18 fw-600">
                           
                        </div>
                        <div class="desktopIcon">
                         <dx:ASPxLabel ID="lblCanceled" runat="server" Text="" ClientInstanceName="lblCanceled"/>
                        <%--<div class="ti-close"></div>--%>
                        </div>
                    </div>
                    <%--<i class="ti-facebook fs-36 pull-right"></i>--%>
                </div>
            </a>
        </div>
    </div>
    <%--<div id="banner"><img src="../images/TPAA-Agent-Final.jpg" width="100%" alt="" data-pin-nopin="true"/></div>--%>
    
    <div class="row">
        <div class="widget">
            <div class="widget-body">
                <div class="row mb-15">
                    <div class="col-md-8">
                        <dx:ASPxPageControl ID="BookTabPage" ClientInstanceName="BookTabPage" runat="server" ActiveTabIndex="0" EnableHierarchyRecreation="True" Width="100%" Height="100%">
                            <TabPages>
                                <dx:TabPage Text="Pending Payment">
                                    <ContentCollection>
                                        <dx:ContentControl ID="ContentControl3" runat="server">
                                            <dx:ASPxCallbackPanel ID="ASPxCallbackPanel2" runat="server" RenderMode="Div" Height="100%" ClientInstanceName="ASPxCallbackPanel2">
                                                <ClientSideEvents EndCallback="function(s, e){ numCallback -=1; }"></ClientSideEvents>
                                                <PanelCollection>
                                                    <dx:PanelContent>
                                                        <dx:ASPxGridView ID="gvPendingBooking" ClientInstanceName="gridBookingPending" runat="server"  OnCustomButtonCallback="gvPendingBooking_CustomButtonCallback"
                                                            KeyFieldName="TransID" Width="100%" AutoGenerateColumns="False" SettingsBehavior-ColumnResizeMode="Control" onpageindexchanged="gvPendingBooking_PageIndexChanged">
                                                            <Columns> 
                                                                <dx:GridViewDataColumn FieldName="TransID" SortOrder="Descending" Caption="Transaction ID" VisibleIndex="1" GroupIndex="0" Width="120px"/>
                                                                <dx:GridViewDataColumn FieldName="RecordLocator" Caption="Record Locator" VisibleIndex="1" Width="120px"/>
                                                                <dx:GridViewDataDateColumn FieldName="BookingDate" Caption="Booking Date" VisibleIndex="2" Width="140px">
                                                                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy HH:mm" ></PropertiesDateEdit>
                                                                </dx:GridViewDataDateColumn>
                                                                <dx:GridViewDataDateColumn FieldName="DepartureDate" Caption="STD" VisibleIndex="3" width="140px">
                                                                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy HH:mm" ></PropertiesDateEdit>
                                                                </dx:GridViewDataDateColumn>
                                                                <dx:GridViewDataDateColumn FieldName="ArrivalDate" Caption="STA" VisibleIndex="4" width="140px">
                                                                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy HH:mm" ></PropertiesDateEdit>
                                                                </dx:GridViewDataDateColumn>
                                                                <dx:GridViewDataColumn FieldName="FlightNumber" Caption="Flight" VisibleIndex="5" /> 
                                                                <dx:GridViewDataColumn FieldName="Origin" Caption="Flight From" VisibleIndex="5" />
                                                                <dx:GridViewDataColumn FieldName="Destination" Caption="Flight To" VisibleIndex="5" />
                                                                <dx:GridViewDataDateColumn FieldName="ExpiryDate" Caption="Payment Expiry" VisibleIndex="6" Width="140px">
                                                                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy HH:mm" ></PropertiesDateEdit>
                                                                </dx:GridViewDataDateColumn>
                                                                <dx:GridViewDataColumn FieldName="Currency" Caption="Currency" VisibleIndex="11" />
                                                                <dx:GridViewDataSpinEditColumn  FieldName="CollectedAmt" Caption="Collected Amount" VisibleIndex="7" Width="120px">
                                                                    <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>
                                                                </dx:GridViewDataSpinEditColumn>
                                                                <dx:GridViewDataSpinEditColumn  FieldName="TransTotalAmt" Caption="Total Amount" VisibleIndex="8" Settings-FilterMode="Value" >
                                                                    <PropertiesSpinEdit DisplayFormatString="n2" ></PropertiesSpinEdit>
                                                                </dx:GridViewDataSpinEditColumn>
                                                                <dx:GridViewDataSpinEditColumn  FieldName="TotalAmtAVG" Caption="Average Fare" VisibleIndex="9">
                                                                    <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>
                                                                </dx:GridViewDataSpinEditColumn>
                                                                <dx:GridViewDataSpinEditColumn  FieldName="DueAmt" Caption="Due Amount" VisibleIndex="10">
                                                                    <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>
                                                                </dx:GridViewDataSpinEditColumn>
                                                                <dx:GridViewCommandColumn VisibleIndex="0" Caption="Choose" Width="100px" ButtonType="Image">
                                                                    <CustomButtons>
                                                                        <dx:GridViewCommandColumnCustomButton Text="View" ID="viewBtn" Image-AlternateText="View" Image-ToolTip="View" Image-Width="18px" Image-Url="../Images/AKBase/view_icon.png" />
                                                                        <dx:GridViewCommandColumnCustomButton  Text="Get&nbsp;Latest" ID="getBtn" Image-AlternateText="Sync Navitaire" Image-ToolTip="Sync Navitaire" Image-Width="16px" Image-Url="../Images/AKBase/live_sync.png" />
                                                                    </CustomButtons>
                                                                </dx:GridViewCommandColumn>
                                                            </Columns>
                                                            <GroupSummary>
                                                                <dx:ASPxSummaryItem FieldName="BookingDate" SummaryType="Min" DisplayFormat="Booking Date: {0:dd MMM yyyy HH:mm}"/>
                                                                <dx:ASPxSummaryItem FieldName="DepartureDate" SummaryType="Min" DisplayFormat="STD: {0:dd MMM yyyy HH:mm}"/>
                                                                <dx:ASPxSummaryItem FieldName="DepartReturn" SummaryType="Max" DisplayFormat="{0}" />
                                                                <%--<dx:ASPxSummaryItem FieldName="DepartCity" SummaryType="Max" DisplayFormat="{0}-" />--%>
                                                                <%--<dx:ASPxSummaryItem FieldName="ReturnCity" SummaryType="Max" DisplayFormat="{0}" />--%>
                                                                <dx:ASPxSummaryItem FieldName="TransTotalAmt" SummaryType="Sum"  DisplayFormat="Total Amount: {0:N}" />
                                                                <dx:ASPxSummaryItem FieldName="TransTotalAmt" ShowInGroupFooterColumn="TransTotalAmt" SummaryType="Sum" DisplayFormat="{0:N}"  />
                                                                <dx:ASPxSummaryItem FieldName="CollectedAmt" ShowInGroupFooterColumn="CollectedAmt" SummaryType="Sum" DisplayFormat="{0:N}"  />
                                                                <dx:ASPxSummaryItem FieldName="DueTotalAmt" ShowInGroupFooterColumn="DueAmt" SummaryType="Average" DisplayFormat="{0:N}"  />
                                                            </GroupSummary>
                                                            <Settings ShowGroupPanel="True" ShowGroupedColumns="true" ShowFooter="True" ShowGroupFooter="VisibleIfExpanded" ShowFilterRow="true" ShowVerticalScrollBar="false" ShowHorizontalScrollBar="true" />
                                                            <settingsbehavior AutoExpandAllGroups="True" columnresizemode="Control" />
                                                            <SettingsBehavior ColumnResizeMode="Control"></SettingsBehavior>
                                                            <Settings ShowVerticalScrollBar="False"></Settings>
                                                            <styles>
                                                                <header backcolor="#333333" font-size="10pt" forecolor="White"></header>
                                                            </styles>
                                                        </dx:ASPxGridView>
                                                        
                                                        <div>
                                                            <span class="red" >**</span> Bookings that have flight/s within the cutoff time for making payments online.<br /><span class="red" >**</span> Please note that <strong>Booking Date</strong> and <strong>Expiry Date</strong> are shown in GMT(+8).
                                                        </div>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dx:ASPxCallbackPanel>
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>
                                
                                <dx:TabPage Text="Upload Passenger">
                                    <ContentCollection>
                                        <dx:ContentControl ID="ContentControl1" runat="server">
                                            <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" RenderMode="Div" Height="100%" ClientInstanceName="ASPxCallbackPanel1">
                                                <ClientSideEvents EndCallback="function(s, e){ numCallback -=1; }"></ClientSideEvents>
                                                <PanelCollection>
                                                    <dx:PanelContent>
                                                        <dx:ASPxGridView ID="gvFinishBooking" ClientInstanceName="gridBookingFinish" runat="server"  OnCustomButtonCallback="gvFinishBooking_CustomButtonCallback"
                                                            KeyFieldName="TransID" Width="100%" AutoGenerateColumns="False" SettingsBehavior-ColumnResizeMode="Control" onpageindexchanged="gvFinishBooking_PageIndexChanged" >
                                                            <ClientSideEvents EndCallback="OnCallbackComplete" BeginCallback="OnCallbackBegin" />
                                                            <Columns>
                                                                <dx:GridViewDataColumn FieldName="TransID" SortOrder="Descending" Caption="Transaction ID" VisibleIndex="1" GroupIndex="0" Width="140px"/>
                                                                <dx:GridViewDataColumn FieldName="RecordLocator" Caption="Record Locator" VisibleIndex="1" Width="120px"/>
                                                                <dx:GridViewDataDateColumn FieldName="BookingDate" Caption="Booking Date" VisibleIndex="2" Width="140px">
                                                                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy HH:mm" ></PropertiesDateEdit>
                                                                </dx:GridViewDataDateColumn>
                                                                <dx:GridViewDataDateColumn FieldName="DepartureDate" Caption="STD" VisibleIndex="3" width="140px">
                                                                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy HH:mm" ></PropertiesDateEdit>
                                                                </dx:GridViewDataDateColumn>
                                                                <dx:GridViewDataDateColumn FieldName="ArrivalDate" Caption="STA" VisibleIndex="4" width="140px">
                                                                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy HH:mm" ></PropertiesDateEdit>
                                                                </dx:GridViewDataDateColumn>
                                                                <dx:GridViewDataColumn FieldName="FlightNumber" Caption="Flight" VisibleIndex="5" />
                                                                <dx:GridViewDataColumn FieldName="Origin" Caption="Flight From" VisibleIndex="5" />
                                                                <dx:GridViewDataColumn FieldName="Destination" Caption="Flight To" VisibleIndex="6" />
                                                                <dx:GridViewDataDateColumn FieldName="ExpiryDate" Caption="Payment Expiry" VisibleIndex="0" Visible="false" width="140px">
                                                                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy HH:mm" ></PropertiesDateEdit>
                                                                </dx:GridViewDataDateColumn>
                                                                <dx:GridViewDataColumn FieldName="Currency" Caption="Currency" VisibleIndex="11" />
                                                                <dx:GridViewDataSpinEditColumn  FieldName="CollectedAmt" Caption="Collected Amount" VisibleIndex="7" Width="120px">
                                                                    <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>
                                                                </dx:GridViewDataSpinEditColumn>
                                                                <dx:GridViewDataSpinEditColumn  FieldName="TransTotalAmt" Caption="Total Amount" VisibleIndex="7" >
                                                                    <PropertiesSpinEdit DisplayFormatString="n2" ></PropertiesSpinEdit>
                                                                </dx:GridViewDataSpinEditColumn> 
                                                                <dx:GridViewDataSpinEditColumn  FieldName="TotalAmtAVG" Caption="Average Fare" VisibleIndex="7">
                                                                    <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>
                                                                </dx:GridViewDataSpinEditColumn>
                                                                <dx:GridViewDataColumn FieldName="DueDay" Caption="Due before STD" VisibleIndex="8" />
                                                                <dx:GridViewCommandColumn VisibleIndex="0" Caption="Choose" Width="100px" ButtonType="Image">
                                                                    <CustomButtons>
                                                                        <dx:GridViewCommandColumnCustomButton Text="View" ID="viewBtnFinish" Image-AlternateText="View" Image-ToolTip="View" Image-Width="18px" Image-Url="../Images/AKBase/view_icon.png" />
                                                                        <dx:GridViewCommandColumnCustomButton  Text="Get&nbsp;Latest" ID="getLatestBtn" Image-AlternateText="Sync Navitaire" Image-ToolTip="Sync Navitaire" Image-Width="16px" Image-Url="../Images/AKBase/live_sync.png" />
                                                                        <dx:GridViewCommandColumnCustomButton Text="Change" ID="changeBtnFinish" Image-AlternateText="Change Flight" Image-ToolTip="Change Flight" Image-Width="16px" Image-Url="../Images/AKBase/plane.png" Visibility="Invisible"></dx:GridViewCommandColumnCustomButton>
                                                                    </CustomButtons> 
                                                                </dx:GridViewCommandColumn>
                                                            </Columns>
                                                            <GroupSummary>
                                                                <dx:ASPxSummaryItem FieldName="BookingDate" SummaryType="Min" DisplayFormat="Booking Date: {0:dd MMM yyyy HH:mm}"/>
                                                                <dx:ASPxSummaryItem FieldName="DepartureDate" SummaryType="Min" DisplayFormat="STD: {0:dd MMM yyyy HH:mm}"/>
                                                                <dx:ASPxSummaryItem FieldName="DepartReturn" SummaryType="Max" DisplayFormat="{0}" />
                                                                <dx:ASPxSummaryItem FieldName="TransTotalAmt" SummaryType="Sum"  DisplayFormat="Total Amount: {0:N2}" />
                                                                <dx:ASPxSummaryItem FieldName="TransTotalAmt" ShowInGroupFooterColumn="TransTotalAmt" SummaryType="Sum" DisplayFormat="{0:N2}"  />
                                                                <dx:ASPxSummaryItem FieldName="CollectedAmt" ShowInGroupFooterColumn="CollectedAmt" SummaryType="Sum" DisplayFormat="{0:N2}"  />
                                                            </GroupSummary>
                                                            <Settings ShowFilterRow="true" ShowGroupPanel="True" ShowGroupedColumns="true" ShowFooter="True" ShowGroupFooter="VisibleIfExpanded"  />
                                                            <SettingsBehavior  AutoExpandAllGroups="True" ColumnResizeMode="Control"></SettingsBehavior>
                                                            <Settings showverticalscrollbar="False" ShowHorizontalScrollBar="true"></Settings>
                                                            <styles>
                                                                <header backcolor="#333333" font-size="10pt" forecolor="White"></header>
                                                            </styles>
                                                        </dx:ASPxGridView>
                                                        
                                                        <div>
                                                            <span class="red" >**</span> Please note that <strong>Booking Date</strong> and <strong>Expiry Date</strong> are shown in GMT(+8).
                                                        </div>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dx:ASPxCallbackPanel>
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>
                                                                
                                <%--20170331-Sienny (Upcoming Flight) --%>
                                <dx:TabPage Text="Confirmed">
                                    <ContentCollection>
                                        <dx:ContentControl ID="ContentControl2" runat="server">
                                            <dx:ASPxCallbackPanel ID="ASPxCallbackPanel3" runat="server" RenderMode="Div" Height="100%" ClientInstanceName="ASPxCallbackPanel3">
                                                <ClientSideEvents EndCallback="function(s, e){ numCallback -=1; }"></ClientSideEvents>
                                                <PanelCollection>
                                                    <dx:PanelContent>
                                                        <dx:ASPxGridView ID="gvUpcomingBooking" ClientInstanceName="gridBookingUpcoming" runat="server"  OnCustomButtonCallback="gvUpcomingBooking_CustomButtonCallback"
                                                            KeyFieldName="TransID" Width="100%" AutoGenerateColumns="False" SettingsBehavior-ColumnResizeMode="Control" onpageindexchanged="gvUpcomingBooking_PageIndexChanged" >
                                                            <ClientSideEvents EndCallback="OnCallbackComplete" BeginCallback="OnCallbackBegin" />
                                                            <Columns>
                                                                <dx:GridViewDataColumn FieldName="TransID" SortOrder="Descending" Caption="Transaction ID" VisibleIndex="1" GroupIndex="0" Width="140px"/>
                                                                <dx:GridViewDataColumn FieldName="RecordLocator" Caption="Record Locator" VisibleIndex="1" Width="120px"/>
                                                                <dx:GridViewDataDateColumn FieldName="BookingDate" Caption="Booking Date" VisibleIndex="2" Width="140px">
                                                                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy HH:mm" ></PropertiesDateEdit>
                                                                </dx:GridViewDataDateColumn>
                                                                <dx:GridViewDataDateColumn FieldName="DepartureDate" Caption="STD" VisibleIndex="3" width="140px">
                                                                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy HH:mm" ></PropertiesDateEdit>
                                                                </dx:GridViewDataDateColumn>
                                                                <dx:GridViewDataDateColumn FieldName="ArrivalDate" Caption="STA" VisibleIndex="4" width="140px">
                                                                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy HH:mm" ></PropertiesDateEdit>
                                                                </dx:GridViewDataDateColumn>
                                                                <dx:GridViewDataColumn FieldName="FlightNumber" Caption="Flight" VisibleIndex="5" />
                                                                <dx:GridViewDataColumn FieldName="Origin" Caption="Flight From" VisibleIndex="5" />
                                                                <dx:GridViewDataColumn FieldName="Destination" Caption="Flight To" VisibleIndex="5" />
                                                                <dx:GridViewDataDateColumn FieldName="ExpiryDate" Caption="Payment Expiry" VisibleIndex="0" Visible="false" width="140px">
                                                                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy HH:mm" ></PropertiesDateEdit>
                                                                </dx:GridViewDataDateColumn>
                                                                <dx:GridViewDataColumn FieldName="Currency" Caption="Currency" VisibleIndex="11" />
                                                                <dx:GridViewDataSpinEditColumn  FieldName="CollectedAmt" Caption="Collected Amount" VisibleIndex="7" Width="120px">
                                                                    <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>
                                                                </dx:GridViewDataSpinEditColumn>
                                                                <dx:GridViewDataSpinEditColumn  FieldName="TransTotalAmt" Caption="Total Amount" VisibleIndex="8" >
                                                                    <PropertiesSpinEdit DisplayFormatString="n2" ></PropertiesSpinEdit>
                                                                </dx:GridViewDataSpinEditColumn> 
                                                                <dx:GridViewDataSpinEditColumn  FieldName="TotalAmtAVG" Caption="Average Fare" VisibleIndex="9">
                                                                    <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>
                                                                </dx:GridViewDataSpinEditColumn>
                                                                <dx:GridViewDataColumn FieldName="DueDay" Caption="Due before STD" VisibleIndex="8" />
                                                                <dx:GridViewCommandColumn VisibleIndex="0" Caption="Choose" Width="100px" ButtonType="Image">
                                                                    <CustomButtons>
                                                                        <dx:GridViewCommandColumnCustomButton Text="View" ID="viewButton" Image-AlternateText="View" Image-ToolTip="View" Image-Width="18px" Image-Url="../Images/AKBase/view_icon.png" />
                                                                        <dx:GridViewCommandColumnCustomButton  Text="Get&nbsp;Latest" ID="getLatestButton" Image-AlternateText="Sync Navitaire" Image-ToolTip="Sync Navitaire" Image-Width="16px" Image-Url="../Images/AKBase/live_sync.png" />
                                                                        <%--<dx:GridViewCommandColumnCustomButton Text="Change" ID="GridViewCommandColumnCustomButton3" Image-AlternateText="Change Flight" Image-ToolTip="Change Flight" Image-Width="16px" Image-Url="../Images/AKBase/plane.png" Visibility="Invisible"></dx:GridViewCommandColumnCustomButton>--%>
                                                                    </CustomButtons> 
                                                                </dx:GridViewCommandColumn>
                                                            </Columns>
                                                            <GroupSummary>
                                                                <dx:ASPxSummaryItem FieldName="BookingDate" SummaryType="Min" DisplayFormat="Booking Date: {0:dd MMM yyyy HH:mm}"/>
                                                                <dx:ASPxSummaryItem FieldName="DepartureDate" SummaryType="Min" DisplayFormat="STD: {0:dd MMM yyyy HH:mm}"/>
                                                                <dx:ASPxSummaryItem FieldName="DepartReturn" SummaryType="Max" DisplayFormat="{0}" />
                                                                <dx:ASPxSummaryItem FieldName="TransTotalAmt" SummaryType="Sum"  DisplayFormat="Total Amount: {0:N2}" />
                                                                <dx:ASPxSummaryItem FieldName="TransTotalAmt" ShowInGroupFooterColumn="TransTotalAmt" SummaryType="Sum" DisplayFormat="{0:N2}"  />
                                                                <dx:ASPxSummaryItem FieldName="CollectedAmt" ShowInGroupFooterColumn="CollectedAmt" SummaryType="Sum" DisplayFormat="{0:N2}"  />
                                                            </GroupSummary>
                                                            <Settings ShowFilterRow="true" ShowGroupPanel="True" ShowGroupedColumns="true" ShowFooter="True" ShowGroupFooter="VisibleIfExpanded"  />
                                                            <SettingsBehavior  AutoExpandAllGroups="True" ColumnResizeMode="Control"></SettingsBehavior>
                                                            <Settings ShowVerticalScrollBar="False" ShowHorizontalScrollBar="true"></Settings>
                                                            <styles>
                                                                <header backcolor="#333333" font-size="10pt" forecolor="White"></header>
                                                            </styles>
                                                        </dx:ASPxGridView>
                                                        
                                                        <div>
                                                            <span class="red" >**</span> Please note that <strong>Booking Date</strong> and <strong>Expiry Date</strong> are shown in GMT(+8).
                                                        </div>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dx:ASPxCallbackPanel>
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>

                            </TabPages>
                        </dx:ASPxPageControl>
                    </div>
                    
                    <div class="col-md-4" style="background:#fff;">
                        <div class="widget">
                            <div class="widget-heading">
                                <h3 class="widget-title">Notifications</h3>
                            </div>
                            <div class="widget-body">
                                <ul class="activity-list activity-sm list-unstyled mb-0" id="NotificationDiv" runat="server">
                                    <%--<li class="activity-purple">
                                      <time datetime="2015-12-10T20:50:48+07:00" class="fs-12 text-muted">10 minutes ago</time>
                                      <p class="mt-10 mb-0">Booking <span class="label label-success">201610304455</span> Created</p>
                                    </li>
                                    <li class="activity-danger">
                                      <time datetime="2015-12-10T20:42:40+07:00" class="fs-12 text-muted">15 minutes ago</time>
                                      <p class="mt-10 mb-0">You have 2 booking payment about to <span class="text-danger">expired</span> in 2 days</p>
                                    </li>
                                    <li class="activity-warning">
                                      <time datetime="2015-12-10T20:35:35+07:00" class="fs-12 text-muted">22 minutes ago</time>
                                      <p class="mt-10 mb-0">You have 1 booking payment about to <span class="text-danger">expired</span> in 3 days</p>
                                    </li>
                                    <li class="activity-success">
                                      <time datetime="2015-12-10T20:27:48+07:00" class="fs-12 text-muted">30 minutes ago</time>
                                      <p class="mt-10 mb-0">Booking <span class="label label-success">201602301122</span> Canceled</p>
                                    </li>
                                    <li class="activity-primary">
                                      <time datetime="2015-12-10T20:22:48+07:00" class="fs-12 text-muted">35 minutes ago</time>
                                      <p class="mt-10 mb-0">You have 1 booking pending for upload passenger</p>
                                    </li>--%>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>