<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewAdminMaster.Master" AutoEventWireup="true" CodeBehind="adminpaymentextension.aspx.cs" Inherits="GroupBooking.Web.admin.adminpaymentextension" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript">
        var keyValue;
        function OnEdit(element, key) {
        callbackPanel.SetContentHtml("");
        popup.ShowAtElement(element);
        keyValue = key;
        }
        function popup_Shown(s, e) {
        callbackPanel.PerformCallback(keyValue);
        }
    </script>

    <dx:ASPxPopupControl ID="popup" ClientInstanceName="popup" runat="server" Modal="true" AllowDragging="true" 
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="Request Confirmation" >
        <%-- CloseButtonImage-Url="~/Images/AirAsia/close_button_icon.jpg"--%>  
        <%--CloseAction="CloseButton"--%>

        <ClientSideEvents Shown="popup_Shown" />
        <CloseButtonImage Url="~/Images/AirAsia/close_button_icon.jpg"></CloseButtonImage>

        <ClientSideEvents Shown="popup_Shown"></ClientSideEvents>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <dx:ASPxCallbackPanel ID="callbackPanel" ClientInstanceName="callbackPanel" runat="server"
                    Width="566px" Height="400px" OnCallback="callbackPanel_Callback" RenderMode="Table">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <dx:ASPxLoadingPanel runat="server" ID="LoadingPanelPop" ClientInstanceName="LoadingPanelPop" Modal="True"></dx:ASPxLoadingPanel>
                            &nbsp;&nbsp;<font color="red"> </font>
                            <table class="tableClass">
                                <tr>
                                    <td colspan="3">
                                        <dx:ASPxLabel ID="lblHeadReq" Text=" Payment Extension Request" runat="server" Font-Bold="true" Font-Size="Small"></dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="lblTransIDText" Text="Transaction ID" runat="server"></dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxLabel ID="lblTransID" runat="server"></dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 196px" valign="top">Reason</td>
                                    <td>
                                        <dx:ASPxMemo ID="memoRemarks" runat="server" Height="102px" MaxLength="200" Width="349px">
                                            <ValidationSettings SetFocusOnError="True">                                                 
                                                <RequiredField ErrorText="Reason is required" IsRequired="True" />
                                                 <RequiredField IsRequired="True" ErrorText="Reason is required"></RequiredField>
                                            </ValidationSettings>
                                        </dx:ASPxMemo>    
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxButton CssClass="button_2" ID="btnRequest" runat="server"  
                                            Text="Request" AutoPostBack="False" OnClick="btnRequest_Click" CausesValidation="true"  >
                                            <ClientSideEvents Click="function OnCloseButtonClick(s, e) {
                                                if(ASPxClientEdit.AreEditorsValid()) {
                                                var parentWindow = window.parent;
                                                LoadingPanelPop.SetText(&#39;Please Wait...&#39;);
                                                LoadingPanelPop.Show();
                                                parentWindow.popup.Hide();}}" />                                            
                                            <ClientSideEvents Click="function OnCloseButtonClick(s, e) {
                                                if(ASPxClientEdit.AreEditorsValid()) {
                                                var parentWindow = window.parent;
                                                LoadingPanelPop.SetText(&#39;Please Wait...&#39;);
                                                LoadingPanelPop.Show();
                                                parentWindow.popup.Hide();}}"></ClientSideEvents>
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </dx:PanelContent> 
                    </PanelCollection> 
                </dx:ASPxCallbackPanel> 
            </dx:PopupControlContentControl> 
        </ContentCollection> 
    </dx:ASPxPopupControl>


    <div id="wrapper4" >
        <div class="page-header row">
            <span style="display:none;">         
                &nbsp;<dx:ASPxLabel ID="lblHeader" Text="Payment Extension" runat="server" Font-Bold="True" Font-Size="Medium"></dx:ASPxLabel>
            </span>
            <div class="col-sm-4" style="text-align:left;">
                <h4>Payment Extension</h4>
            </div>
        </div>
        <div class="page-content row">
            <table width ="100%">
                <tr>
                    <td colspan="2">
                        <table cellpadding="0" cellspacing="0" border="0" >
                            <tr>
                                <td style="width: 130px" >
                                    <dx:ASPxComboBox ID="ddlFilter" runat="server" class="Select" AutoPostBack="false" IncrementalFilteringMode="StartsWith" Width="120">
                                        <Items>
                                            <dx:ListEditItem Text="Agent ID" Value="AgentID" Selected="true"/>
                                            <dx:ListEditItem Text="Agent Name" Value="AgentName" />
                                            <dx:ListEditItem Text="Transaction ID" Value="TransID" />
                                        </Items>
                                    </dx:ASPxComboBox>
                                </td> 
                                <td style="width: 148px" > <dx:ASPxTextBox ID="txtAgentID" runat="server" ></dx:ASPxTextBox></td>   
                                <td></td>     
                                <td >
                                    <dx:ASPxButton CssClass="buttonL2 noBgImg search" ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click">
                                        <ClientSideEvents Click="function(s, e) { LoadingPanel.Show(); }" />  
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <dx:ASPxGridView ID="gvAvaPenalty" runat="server" AutoGenerateColumns="False" ClientInstanceName="gvAvaPenalty" KeyFieldName="TransID" Width="100%">
                            <SettingsBehavior ColumnResizeMode="Control"></SettingsBehavior>
                            <GroupSummary>
                                <dx:ASPxSummaryItem FieldName="LineTotal" SummaryType="Count" DisplayFormat="{0}" />                                               
                            </GroupSummary>
                            <Columns>
                                <dx:GridViewDataColumn Name="PaymentExtension" Caption="Action" VisibleIndex="0" Width="60px">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" onclick="OnEdit(this, '<%# Container.KeyValue %>')">Request</a>
                                    </DataItemTemplate>
                                </dx:GridViewDataColumn>                                                 
                                <dx:GridViewDataColumn Caption="Transaction ID" FieldName="TransID" ShowInCustomizationForm="True" VisibleIndex="1">
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn Caption="Agent Full Name" FieldName="AgentName" ShowInCustomizationForm="True" VisibleIndex="2" Width="250px">
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataDateColumn Caption="Booking Date" FieldName="BookingDate" ShowInCustomizationForm="True" VisibleIndex="3">
                                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy HH:mm"></PropertiesDateEdit>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataDateColumn Caption="Payment Expiry" FieldName="ExpiryDate" ShowInCustomizationForm="True" VisibleIndex="3">
                                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy HH:mm"></PropertiesDateEdit>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataDateColumn Caption="Departure Date" FieldName="STDDate" ShowInCustomizationForm="True" VisibleIndex="3">
                                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy"></PropertiesDateEdit>
                                </dx:GridViewDataDateColumn>
                            </Columns>
                                            
                            <settingsbehavior columnresizemode="Control" />                                            
                            <Styles>
                                <Header BackColor="#333333" ForeColor="White"></Header>
                            </Styles>
                        </dx:ASPxGridView>
                        <div style="margin-top:10px;"><span class="red" >**</span> Please note that <strong>Booking Date</strong> and <strong>Expiry Date</strong> are shown in GMT(+8).</div>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>    
        </div>  
    </div>

</asp:Content>