<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewAdminMaster.Master" AutoEventWireup="true" CodeBehind="bookinglist.aspx.cs" Inherits="GroupBooking.Web.admin.bookinglist" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="true" ContainerElementID="FormPanel"/>
<div id="wrapper4" >
    <div class="page-header row">
                <span style="display:none;">
            &nbsp;<span class="dxeBase" id="ctl00_ContentPlaceHolder2_lblHeader">Admin Main</span>
                    </span>
        <div class="col-sm-4" style="text-align:left;">
        <h4>Booking Management</h4>
        </div>
                    </div>
            <div class="div" style="display:none;">
            &nbsp;<dx:ASPxLabel ID="lblHeader" Text="Booking Management" runat="server" 
                Font-Bold="True" Font-Size="Medium"></dx:ASPxLabel></div>
<br />
 <div class="page-content row">
<table id="filterBooking">
    <tr>
    <td class="tdcms1" style="padding:0px 4px 4px 0px">Transaction ID&nbsp;&nbsp;&nbsp;&nbsp; </td> <td class="tdcms2" style="padding:0px 4px 4px 0px"> <dx:ASPxTextBox ID="txtTransID" runat="server" ></dx:ASPxTextBox></td>
    <td style="padding:1px"></td>
    <td class="tdcms1" style="padding:0px 4px 4px 0px">Booking Date Start&nbsp;&nbsp;&nbsp; </td> <td class="tdcms2" style="padding:0px 4px 4px 0px"> <dx:ASPxDateEdit ID="txtStartDate" runat="server" Width="100" EditFormat="Custom" DisplayFormatString="dd MMM yyyy" EditFormatString="dd MMM yyyy" UseMaskBehavior="false" />  </td>
    </tr>
    
    <tr>
    <td class="tdcms1" style="padding:0px 4px 4px 0px">Record Locator&nbsp;&nbsp;&nbsp;&nbsp; </td> <td class="tdcms2" style="padding:0px 4px 4px 0px"> <dx:ASPxTextBox ID="txtRecordLocator" runat="server" ></dx:ASPxTextBox></td>
    <td style="padding:1px"></td>
    <td class="tdcms1" style="padding:0px 4px 4px 0px">Booking Date End </td> <td class="tdcms2" style="padding:0px 4px 4px 0px"> <dx:ASPxDateEdit ID="txtEndDate" runat="server" Width="100" EditFormat="Custom" DisplayFormatString="dd MMM yyyy" EditFormatString="dd MMM yyyy" UseMaskBehavior="false" /> </td>
    </tr>

    <tr>
    <td class="tdcms1" style="">Agent ID </td> <td class="tdcms2" style=""> <dx:ASPxTextBox ID="txtAgentID" runat="server" ></dx:ASPxTextBox></td>
    <td style="padding:1px"></td>
    <td class="tdcms1" style="">Status </td> <td class="tdcms2" style=""> <dx:ASPxComboBox ID="cmbStatus" 
            runat="server" SelectedIndex="0" >
        <Items>
            <dx:ListEditItem Selected="True" Text="All" Value="0" />            
            <dx:ListEditItem Text="Pending" Value="1" />
            <dx:ListEditItem Text="Pending for Passenger Upload" Value="2" />
            <dx:ListEditItem Text="Confirmed" Value="3" />
            <dx:ListEditItem Text="Cancel" Value="4" /> 
        </Items>
        </dx:ASPxComboBox></td>
        <td style="">
                 <dx:ASPxButton CssClass="buttonL2 noBgImg search" ID="btnSearch" runat="server" 
                    Text="Search" OnClick="btnSearch_Click">
                    <ClientSideEvents Click="function(s, e) { LoadingPanel.Show(); }" />  
                </dx:ASPxButton>
        </td>
        <td style=""></td>
        <td style=""></td>
    </tr>

    <tr>
    <td class="tdcms1" style="padding:0px 4px 4px 0px">
    </td>
    <td style="padding:0px 4px 4px 0px">
    </td>
    <td style="padding:0px 4px 4px 0px"></td>
    <td style="padding:0px 4px 4px 0px"></td>
    <td style="padding:0px 4px 4px 0px"></td>
    </tr>
 </table> 
    
        <div id="selectMainBody" class="mainBody form">
                <%--<div class="redSectionHeader">
                  <div><asp:Label ID="lblGridHead" runat="server" Text="Booking List"></asp:Label></div>
                </div>--%>
    
        <dx:ASPxGridView ID="gridAgent" ClientInstanceName="gridBooking" runat="server" 
            KeyFieldName="TransID" Width="100%" 
                    AutoGenerateColumns="False" SettingsBehavior-ColumnResizeMode="Control" OnCustomButtonCallback="gridAgent_CustomButtonCallback">
            <Columns>            
                <dx:GridViewCommandColumn Caption="Choose" ShowSelectCheckbox="True" VisibleIndex="0" Name="rowCheckBox" Visible="false">
                </dx:GridViewCommandColumn>                  
                 
                <dx:GridViewCommandColumn VisibleIndex="0" Caption="Choose" Width="8%">                 
                 <CustomButtons> 
                     <dx:GridViewCommandColumnCustomButton Text="View" ID="editBtn" />
                     <dx:GridViewCommandColumnCustomButton Text="Get&nbsp;Latest" ID="getLatestBtn" />
                 </CustomButtons> 
                 </dx:GridViewCommandColumn> 
                <dx:GridViewDataColumn FieldName="AgentName" Caption="Agent Name" VisibleIndex="0" GroupIndex="0" />   
                <dx:GridViewDataColumn FieldName="TransID" Caption="Transaction ID" VisibleIndex="1" Width="13%" />
                <dx:GridViewDataDateColumn FieldName="BookingDate" Caption="Booking date" VisibleIndex="2" Width="14%">
                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy HH:mm"></PropertiesDateEdit>
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataDateColumn FieldName="ExpiryDate" Caption="Expiry date" VisibleIndex="3" Width="14%">
                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy HH:mm"></PropertiesDateEdit>
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataColumn FieldName="Status" Caption="Status" VisibleIndex="4" Width="20%" />
                <dx:GridViewDataSpinEditColumn  FieldName="CollectedAmt" 
                            Caption="Collected Amount" VisibleIndex="5" Width="14%">
                            <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                    
                        </dx:GridViewDataSpinEditColumn> 
                            <dx:GridViewDataSpinEditColumn  FieldName="TransTotalAmt" Caption="Total Amount" VisibleIndex="6" Width="14%">
                        <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                    
                            </dx:GridViewDataSpinEditColumn>      
                <dx:GridViewDataColumn FieldName="Currency" Caption=" " VisibleIndex="7" Width="5%" />                
                
            </Columns>
            <Settings ShowGroupFooter="VisibleIfExpanded" />
            <Settings ShowHorizontalScrollBar="true" />
            <SettingsBehavior ColumnResizeMode="Control"></SettingsBehavior>
            <SettingsBehavior ColumnResizeMode="Control"></SettingsBehavior>
            
            <Settings ShowVerticalScrollBar="True" VerticalScrollableHeight="300"/>
            <Styles>
                <Header BackColor="#333333" ForeColor="White">
                </Header>
            </Styles>
        </dx:ASPxGridView>
        
                <div><span class="red" >**</span> Please note that <strong>Booking Date</strong> and <strong>Expiry Date</strong> are shown in GMT(+8).</div>
                    
            </div>
</div>
                
</div>
</asp:Content>

