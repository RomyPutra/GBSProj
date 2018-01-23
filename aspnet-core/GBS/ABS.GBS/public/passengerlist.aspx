<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewPageMasterReport.Master" AutoEventWireup="true" CodeBehind="passengerlist.aspx.cs" Inherits="GroupBooking.Web.passengerlist" %>
<%@ MasterType  virtualPath="~/Master/PageMaster.Master"%>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolder2">   
    <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="true" ContainerElementID="FormPanel"/>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <div id="mainContent" style="width: 100%">
        <div id="errorDiv"></div>
        <div class="div">
            <dx:ASPxLabel ID="lblHeader" Text="My Passenger" runat="server" Font-Bold="True" Font-Size="Medium"></dx:ASPxLabel>
        </div>

        <div id="selectMainBody" class="mainBody form">
            <div class="redSectionHeader">
                <div>Passenger Name Change</div>
            </div>
            <dx:ASPxGridView ID="gvFinishBooking" ClientInstanceName="gridBookingFinish" runat="server"  OnCustomButtonCallback="gvFinishBooking_CustomButtonCallback"
                KeyFieldName="TransID" Width="100%" AutoGenerateColumns="False" SettingsBehavior-ColumnResizeMode="Control" >
                <Columns>
                    <dx:GridViewDataColumn FieldName="TransID" Caption="Transaction ID" VisibleIndex="1" Width="140px"/>
                    <dx:GridViewDataColumn FieldName="BookingDate" Caption="Booking Date" VisibleIndex="2" />
                    <dx:GridViewDataColumn FieldName="ExpiryDate" Caption="Payment Expiry" VisibleIndex="3" Visible="false"/>
                    <dx:GridViewDataSpinEditColumn  FieldName="CollectedAmt" Caption="Collected Amount" VisibleIndex="7" Width="120px">
                        <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>
                    </dx:GridViewDataSpinEditColumn>
                    <dx:GridViewDataSpinEditColumn  FieldName="TransTotalAmt" Caption="Total Amount" VisibleIndex="7">
                        <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>
                    </dx:GridViewDataSpinEditColumn>
                    <dx:GridViewDataSpinEditColumn  FieldName="TotalAmtAVG" Caption="Average Fare" VisibleIndex="7">
                        <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>
                    </dx:GridViewDataSpinEditColumn>
                    <dx:GridViewDataColumn FieldName="DueDay" Caption="Due before STD" VisibleIndex="8" />
                    <dx:GridViewCommandColumn VisibleIndex="0" Caption="Action" Width="70px">
                        <CustomButtons>
                            <dx:GridViewCommandColumnCustomButton Text="Edit" ID="viewBtnFinish" />
                        </CustomButtons>
                    </dx:GridViewCommandColumn>
                </Columns>
            
                <SettingsBehavior ColumnResizeMode="Control"></SettingsBehavior>
                <SettingsPager AlwaysShowPager="true" ></SettingsPager>
                <Settings showverticalscrollbar="True"></Settings>
            </dx:ASPxGridView>
        </div> <%-- end div selectMainBody --%>
    </div> <%-- end div mainContent --%>

<%-- 
<dx:ASPxRoundPanel ID="RPanelData" runat="server" HeaderText="Booking List" 
        Visible="True" Width="100%" >            
        <PanelCollection>
<dx:PanelContent ID="PanelContent1" runat="server">
<table>
<tr>

    <td class="tdcms1">Transaction ID </td> <td class="tdcms2"> <dx:ASPxTextBox ID="txtTransID" runat="server" ></dx:ASPxTextBox></td>
    <td></td>
    <td class="tdcms1">Booking Date Start </td> <td class="tdcms2"> <dx:ASPxDateEdit ID="txtStartDate" runat="server" Width="150" EditFormat="Custom" UseMaskBehavior="true" />  </td>
    </tr>
    <tr>
  <td class="tdcms1">Status </td> <td class="tdcms2"> 
        <dx:ASPxComboBox ID="cmbStatus" 
            runat="server" SelectedIndex="0" Width="135px">
        <Items>
            <dx:ListEditItem Selected="True" Text="All" Value="0" />
            <dx:ListEditItem Text="Confirmed" Value="1" />
            <dx:ListEditItem Text="Pending" Value="2" />
            <dx:ListEditItem Text="Deposit" Value="3" />
            <dx:ListEditItem Text="Full Payment" Value="4" />
            <dx:ListEditItem Text="Cancel" Value="5" />
            <dx:ListEditItem Text="Amend" Value="6" />
        </Items>
        </dx:ASPxComboBox></td>
    
    <td></td>
    <td class="tdcms1">Booking Date End </td> <td class="tdcms2"> <dx:ASPxDateEdit ID="txtEndDate" runat="server" Width="150" EditFormat="Custom" UseMaskBehavior="true" /> </td>
    </tr>
    <tr>

  
        <td></td>
    </tr>
    <tr>
    <td class="tdcms1">
    </td>
    <td>
     <dx:ASPxButton CssClass="button_2" ID="btnSearch" runat="server" 
                    Text="Search" OnClick="btnSearch_Click">
                    <ClientSideEvents Click="function(s, e) { LoadingPanel.Show(); }" />  
                </dx:ASPxButton>
    </td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
    </tr>
 </table> 
    <div>
        <dx:ASPxGridView ID="gridAgent" ClientInstanceName="gridBooking" runat="server" 
            KeyFieldName="TransID" Width="800px" 
                    AutoGenerateColumns="False" SettingsBehavior-ColumnResizeMode="Control" >
            <Columns>            
                <dx:GridViewCommandColumn Caption="Choose" ShowSelectCheckbox="True" VisibleIndex="0" Name="rowCheckBox" Visible="false">
                </dx:GridViewCommandColumn>                  
                 
                <dx:GridViewCommandColumn VisibleIndex="0">                 
                 <CustomButtons> 
                     <dx:GridViewCommandColumnCustomButton Text="View" ID="editBtn" /> 
                 </CustomButtons> 
                 </dx:GridViewCommandColumn> 
                 
                <dx:GridViewDataColumn FieldName="TransID" Caption="Transaction ID" VisibleIndex="1" />
                <dx:GridViewDataColumn FieldName="BookingDate" Caption="Booking date" VisibleIndex="2" />
                <dx:GridViewDataColumn FieldName="ExpiryDate" Caption="Expiry date" VisibleIndex="3" />
                <dx:GridViewDataColumn FieldName="Status" Caption="Status" VisibleIndex="4" />
                <dx:GridViewDataColumn FieldName="CollectedAmt" Caption="Collected amount" VisibleIndex="5" />
                <dx:GridViewDataColumn FieldName="TransTotalAmt" Caption="Total amount" VisibleIndex="6" />                               
            </Columns>
            
<SettingsBehavior ColumnResizeMode="Control"></SettingsBehavior>

            <Settings ShowHorizontalScrollBar="True" />
        </dx:ASPxGridView>
                          
</div>
</dx:PanelContent> 
</PanelCollection> 
</dx:ASPxRoundPanel> --%>
</asp:Content>