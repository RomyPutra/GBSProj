<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewAdminMaster.Master" AutoEventWireup="true" CodeBehind="cancelledbooking.aspx.cs" Inherits="GroupBooking.Web.admin.CancelledBooking" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
     <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="true" ContainerElementID="FormPanel"/>
<div id="wrapper4" >
    <div class="page-header row">
                <span style="display:none;">
            &nbsp;<span class="dxeBase" id="ctl00_ContentPlaceHolder2_lblHeader">Admin Main</span>
                    </span>
        <div class="col-sm-4" style="text-align:left;">
        <h4>Cancelled Booking</h4>
        </div>
                    </div>
            <div class="div" style="display:none;">
            &nbsp;<dx:ASPxLabel ID="lblHeader" Text="Cancelled Booking" runat="server" 
                Font-Bold="True" Font-Size="Medium"></dx:ASPxLabel></div>
<br />
 
<table id="filterBooking" width="675" class="cancelBooking">
<tr>

    <td class="tdcms1" style="padding:0px 4px 4px 0px"><dx:ASPxLabel ID="lblTransID" Text="Transaction ID" runat="server"></dx:ASPxLabel></td> 
    <td class="tdcms2" style="padding:0px 4px 4px 0px"> <dx:ASPxTextBox ID="txtTransID" runat="server" ></dx:ASPxTextBox></td>
    <td class="tdcms1" style="padding:0px 4px 4px 0px"><dx:ASPxLabel ID="lblDateStart" Text="Depart Date Start" runat="server"></dx:ASPxLabel></td>
    <td style="padding:1px" class="tdcms2"> 
    <dx:ASPxDateEdit ID="txtStartDate" runat="server" Width="100" 
        EditFormat="Custom" UseMaskBehavior="false" DisplayFormatString="dd MMM yyyy" 
                                            EditFormatString="dd MMM yyyy" 
        />  </td>
    </tr>

    <tr>
    <td class="tdcms1" style="padding:0px 4px 4px 0px"><dx:ASPxLabel ID="lblRecordLocator" Text="Record Locator" runat="server"></dx:ASPxLabel>
     </td> 
    <td class="tdcms2" style="padding:0px 4px 4px 0px"> <dx:ASPxTextBox ID="txtRecordLocator" runat="server" ></dx:ASPxTextBox></td>
    <td class="tdcms1" style="padding:1px"><dx:ASPxLabel ID="lblDateEnd" Text="Depart Date End" runat="server"></dx:ASPxLabel></td>
    <td style="padding:0px 4px 4px 0px" class="tdcms2"> 
        <dx:ASPxDateEdit ID="txtEndDate" runat="server" Width="100" EditFormat="Custom" DisplayFormatString="dd MMM yyyy" EditFormatString="dd MMM yyyy" /> </td>
    
    </tr>

    <tr>
    
    <td style="padding:0px 4px 4px 0px" class="tdcms1">
    <dx:ASPxComboBox ID="cmbField" runat="server" class="Select" AutoPostBack="false" IncrementalFilteringMode="StartsWith" Width="120">
                                            <Items>
                                                <dx:ListEditItem Text="Agent ID" Value="AgentID" Selected="true"/>
                                                <dx:ListEditItem Text="Agent Name" Value="AgentName" />
                                            </Items>
                                            </dx:ASPxComboBox>
    </td> 
   <td style="padding:0px 4px 4px 0px" class="tdcms2"> <dx:ASPxTextBox ID="txtAgentID" runat="server" ></dx:ASPxTextBox></td>
    <td style="padding:0px 4px 4px 0px" colspan="2" class="btnAlRight">
             <dx:ASPxButton CssClass="buttonL2 noBgImg search" ID="btnSearch" runat="server" 
                    Text="Search" OnClick="btnSearch_Click">
                    <ClientSideEvents Click="function(s, e) { LoadingPanel.Show(); }" />  
                </dx:ASPxButton>
    </td>
    </tr>

    <tr>
    <td style="padding:0px 4px 4px 0px" class="tdcms1">&nbsp;</td> <td style="padding:0px 4px 4px 0px" class="tdcms2"> 

        </td>
        <td style="padding:0px 4px 4px 0px"></td>
    </tr>
 </table> 
    
        <div id="selectMainBody" class="mainBody form">
                <div class="redSectionHeader">
                  <div><asp:Label ID="lblGridHead" runat="server" Text="Cancelled Booking List"></asp:Label></div>
                </div>
    
        <dx:ASPxGridView ID="gridAgent" ClientInstanceName="gridBooking" runat="server" 
            KeyFieldName="TransID" Width="100%" 
                    AutoGenerateColumns="False" SettingsBehavior-ColumnResizeMode="Control" OnCustomButtonCallback="gridAgent_CustomButtonCallback">
            <Columns>                                                   
                <dx:GridViewCommandColumn VisibleIndex="0" Caption="Choose" Width="6%">                 
                 <CustomButtons> 
                     <dx:GridViewCommandColumnCustomButton Text="View" ID="editBtn" /> 
                 </CustomButtons> 
                 </dx:GridViewCommandColumn> 
                <dx:GridViewDataColumn FieldName="AgentID" Caption="Agent ID" VisibleIndex="0" Width="18%"/>
                <dx:GridViewDataColumn FieldName="TransID" Caption="Transaction ID" VisibleIndex="1" Width="18%"/>
                <dx:GridViewDataColumn FieldName="AgentName" Caption="Agent Name" VisibleIndex="0" GroupIndex="0" Width="15%" />   
                <dx:GridViewDataDateColumn FieldName="STDDate" Caption="Departure Date" VisibleIndex="3" Width="12%">
                <PropertiesDateEdit DisplayFormatString="dd MMM yyyy"></PropertiesDateEdit>
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataDateColumn FieldName="CancelDate" Caption="Cancel Date" VisibleIndex="4" Width="12%">
                <PropertiesDateEdit DisplayFormatString="dd MMM yyyy HH:mm"></PropertiesDateEdit>
                </dx:GridViewDataDateColumn>                                
                                
                    <dx:GridViewDataSpinEditColumn  FieldName="TransTotalAmt" Caption="Total Amount" VisibleIndex="5" Width="14%">
                     <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                    
                    </dx:GridViewDataSpinEditColumn>                                 
                    <dx:GridViewDataColumn FieldName="Currency" Caption=" " VisibleIndex="6" Width="5%"/>  
            </Columns>
            <SettingsPager AlwaysShowPager="true" ></SettingsPager>
             <Settings ShowGroupFooter="VisibleIfExpanded"  />
                                            <GroupSummary>
                                                <dx:ASPxSummaryItem FieldName="LineTotal" SummaryType="Count" DisplayFormat="{0}" />                                               
                                            </GroupSummary>
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
 </asp:Content>

