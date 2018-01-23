<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Master/NewPageMaster.Master" AutoEventWireup="true"
    CodeBehind="bookingmanagement.aspx.cs" Inherits="GroupBooking.Web.BookingManagement" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolder2">
    <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="true" ContainerElementID="FormPanel"/>
    
    <div id="mainContent" style="width: 100%">
        <div class="row page-header" style="margin-bottom:20px;">
            <span style="display:none">
                <dx:ASPxLabel ID="lblHeader" Text="Manage My Booking" runat="server"></dx:ASPxLabel>
            </span>
            <div class="col-sm-4" style="padding-left:30px;padding-top:15px;padding-bottom:15px;text-align:left;">
                <h4>Manage My Booking</h4>
            </div>
        </div>
        
        <div class="page-content">
            <table>
                <tr>
                    <td class="tdcms1" style="padding:1px">Transaction ID&nbsp;&nbsp;&nbsp; </td>
                    <td class="tdcms2" style="padding:1px">
                        <dx:ASPxTextBox ID="txtTransID" runat="server" width="135px"></dx:ASPxTextBox>
                    </td>
                    <td style="padding:1px"></td>
                    <td class="tdcms1" style="padding:1px">&nbsp;&nbsp;Booking Start Date&nbsp;&nbsp;&nbsp; </td>
                    <td class="tdcms2" style="padding:1px">
                        <dx:ASPxDateEdit ID="txtStartDate" runat="server" Width="100" EditFormat="Custom" DisplayFormatString="dd MMM yyyy" EditFormatString="dd MMM yyyy" UseMaskBehavior="false" >
                            <ValidationSettings>
                                <RequiredField IsRequired="True" ErrorText="Start date is required"></RequiredField>
                            </ValidationSettings>
                        </dx:ASPxDateEdit>  
                    </td>
                </tr>
                <tr>
                    <td class="tdcms1" style="padding:1px">PNR&nbsp;&nbsp;&nbsp; </td>
                    <td class="tdcms2" style="padding:1px">
                        <dx:ASPxTextBox ID="txtRecordLocator" runat="server" width="135px"></dx:ASPxTextBox>
                    </td>
                    <td style="padding:1px"></td>
                    <td class="tdcms1" style="padding:1px">&nbsp;&nbsp;Booking End Date</td>
                    <td class="tdcms2" style="padding:1px">
                        <dx:ASPxDateEdit ID="txtEndDate" runat="server" Width="100" EditFormat="Custom" DisplayFormatString="dd MMM yyyy" EditFormatString="dd MMM yyyy" UseMaskBehavior="false" >
                            <ValidationSettings>
                                <RequiredField IsRequired="true" ErrorText="End date is required." />
                            </ValidationSettings>
                        </dx:ASPxDateEdit> 
                    </td>
                </tr>
                <tr>
                    <td class="tdcms1" style="padding:1px">Status </td>
                    <td class="tdcms2" style="padding:1px">
                        <dx:ASPxComboBox ID="cmbStatus" runat="server" SelectedIndex="0" Width="135px">
                            <Items>
                                <dx:ListEditItem Selected="True" Text="All" Value="0" />
                                <dx:ListEditItem Text="Pending" Value="1" />
                                <dx:ListEditItem Text="Pending for Passenger Upload" Value="2" />
                                <dx:ListEditItem Text="Confirmed" Value="3" />
                                <dx:ListEditItem Text="Cancel" Value="4" />
                            </Items>
                        </dx:ASPxComboBox>
                    </td>
                    <td style="padding:1px"></td>
                    <td style="padding:1px" class="btn1Wrapper" colspan="2" align="right">
                        <dx:ASPxButton CssClass="button_1" ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click">
                            <ClientSideEvents Click="function(s, e) { if(ASPxClientEdit.AreEditorsValid()) { LoadingPanel.Show(); }}" />
                        </dx:ASPxButton>
                    </td>
                </tr>
                <tr>
                    <td class="tdcms1" style="padding:1px" colspan="4"></td>
                </tr>
                <tr>
                    <td style="padding:1px">&nbsp;</td>
                </tr>
            </table>
            
            <div id="selectMainBody" class="mainBody form">
                <%--<div>
                    <div class="redSectionHeader">
                        <div>
                            <asp:Label ID="lblGridHead" runat="server" Text="Booking List"></asp:Label>
                        </div>
                    </div>
                </div>--%>

                <%-- 20170403 - Sienny (to get what filter) --%>
                <asp:HiddenField ID="hfSearchFilter" Value="" runat="server"/>

                <dx:ASPxLabel ID="lblExpiryDate" runat="server" Text="" ClientInstanceName="lblExpiryDate" Visible="false"/>
                <dx:ASPxGridView ID="gridAgent" ClientInstanceName="gridBooking" runat="server"  OnCustomButtonCallback="gridAgent_CustomButtonCallback" 
                    KeyFieldName="TransID" Width="100%" AutoGenerateColumns="False" SettingsBehavior-ColumnResizeMode="Control" >
                    
                    <%--20170324 - Sienny (full page loading added) --%>
                    <ClientSideEvents CustomButtonClick="function(s, e) { if(e.buttonID == 'editBtn'){ LoadingPanel.Show(); e.processOnServer = true; } }" />
                    
                    <Columns>
                        <dx:GridViewCommandColumn Caption="Choose" ShowSelectCheckbox="True" VisibleIndex="0" Name="rowCheckBox" Visible="false">
                        </dx:GridViewCommandColumn>
                        <dx:GridViewCommandColumn VisibleIndex="0"  width="6%" Caption="Choose">
                            <CustomButtons>
                                <dx:GridViewCommandColumnCustomButton Text="View" ID="editBtn"/>
                            </CustomButtons>
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataColumn FieldName="TransID" Caption="Transaction ID" VisibleIndex="1" Width="15%" />
                        <dx:GridViewDataDateColumn FieldName="BookingDate" Caption="Booking Date" VisibleIndex="2" Width="14%" >
                            <PropertiesDateEdit DisplayFormatString="dd MMM yyyy HH:mm"></PropertiesDateEdit>
                        </dx:GridViewDataDateColumn>
                        <dx:GridViewDataDateColumn FieldName="ExpiryDate" Caption="Expiry Date" VisibleIndex="3" Width="14%" >
                            <PropertiesDateEdit DisplayFormatString="dd MMM yyyy HH:mm"></PropertiesDateEdit>
                        </dx:GridViewDataDateColumn>
                        <dx:GridViewDataColumn FieldName="Status" Caption="Status" VisibleIndex="4" Width="18%"/>
                        <dx:GridViewDataSpinEditColumn  FieldName="CollectedAmt" Caption="Collected Amount" VisibleIndex="5" Width="14%">
                            <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>
                        </dx:GridViewDataSpinEditColumn>
                        <dx:GridViewDataSpinEditColumn FieldName="TransTotalAmt" Caption="Total Amount" VisibleIndex="6" Width="14%" >
                            <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>
                        </dx:GridViewDataSpinEditColumn>
                        <dx:GridViewDataColumn FieldName="Currency" Caption=" " VisibleIndex="7" Width="5%"/>
                    </Columns>
                    
                    <SettingsBehavior ColumnResizeMode="Control"></SettingsBehavior>
                    <Settings ShowHorizontalScrollBar="True" />
                    <SettingsLoadingPanel Mode="Disabled" />
                    <Styles>
                        <Header BackColor="#333333" ForeColor="White"></Header>
                    </Styles>
                </dx:ASPxGridView>
                
                <div><span class="red" >**</span> Please note that <strong>Booking Date</strong> and <strong>Expiry Date</strong> are shown in GMT(+8).</div>
            </div> <%-- end div selectMainBody --%>
        </div>
    </div> <%-- end div mainContent --%>


<%--  
SellKey : <dx:ASPxTextBox ID="txtPNR" runat="server"></dx:ASPxTextBox>
Amount :<dx:ASPxTextBox ID="txtAmount" runat="server"></dx:ASPxTextBox>
<dx:ASPxButton ID="btnPay" Text="Credit Card Pay" runat="server" onclick="btnPay_Click"></dx:ASPxButton>

Member ID : <dx:ASPxTextBox ID="txtMemberID" runat="server"></dx:ASPxTextBox>
Password : <dx:ASPxTextBox ID="txtPassword" runat="server"></dx:ASPxTextBox>
Credit : <dx:ASPxTextBox ID="txtCredit" runat="server"></dx:ASPxTextBox>
<dx:ASPxButton ID="btnCreditFilePayment" Text="Credit File Pay" runat="server" onclick="btnCreditFilePayment_Click"></dx:ASPxButton>


Credit : <dx:ASPxLabel ID="lblCredit" runat="server"></dx:ASPxLabel>
--%>
</asp:Content>

<asp:Content ID="LeftContent" runat="server" ContentPlaceHolderID ="ContentPlaceHolder3" >
    <aside class="main-sidebar">
        <div class="user">
            <div id="companyLogo">
                <img src="../images/airasia/aalogo.png" alt="" class="avatar img-circle mCS_img_loaded" data-pin-nopin="true"/>
            </div>
            <h4 class="fs-16 text-white mt-15 mb-5 fw-300"><dx:ASPxLabel runat="server" ID="lblAgentName"></dx:ASPxLabel></h4>
            <p class="mb-0 text-muted"><dx:ASPxLabel runat="server" ID="lblAgentOrg" ></dx:ASPxLabel></p>
        </div>
        
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
                    <div class="block clearfix mb-10">
                        <span class="pull-left fs-12 text-muted">Bandwidth</span>
                        <span class="pull-right label label-outline label-danger">80%</span>
                    </div>
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
                    <div class="block clearfix mb-10">
                        <span class="pull-left fs-12 text-muted"><dx:ASPxLabel runat="server" ID="lblContact"></dx:ASPxLabel></span>
                    </div>
                </li>
            </ul>
        </div>
        <div class="sidebar-category">Available Credit [<dx:ASPxLabel runat="server" ID="lblAGCurr" ></dx:ASPxLabel>]</div>
        <div class="sidebar-widget">
            <ul class="list-unstyled pl-25 pr-25">
                <li class="mb-20">
                    <div class="block clearfix mb-10">
                        <span class="pull-left fs-12 text-muted"><dx:ASPxLabel runat="server" ID="lblAGLimit" ></dx:ASPxLabel></span>
                  <%--<span class="pull-right label label-outline label-warning">65%</span>
                  <div role="progressbar" data-transitiongoal="65" class="progress-bar progress-bar-warning"></div>--%>
                    </div>
                </li>
            </ul>
        </div>
    </aside>
</asp:Content>
