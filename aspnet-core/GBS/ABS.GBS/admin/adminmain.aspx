<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewAdminMaster.Master" AutoEventWireup="true" CodeBehind="adminmain.aspx.cs" Inherits="GroupBooking.Web.admin.adminmain" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript">
        // <![CDATA[
           var keyValue;
           function OnEdit(element, key) {
               callbackPanel.SetContentHtml("");
               popup.ShowAtElement(element);
               keyValue = key;
           }
           function popup_Shown(s, e) {
               callbackPanel.PerformCallback(keyValue);
           }
           function OnEditReq(element, key) {
               callbackPanelReq.SetContentHtml("");
               popupReq.ShowAtElement(element);
               keyValue = key;
           }
           function popupReq_Shown(s, e) {
               callbackPanelReq.PerformCallback(keyValue);
           }

        // ]]>
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
                            <table>
                                <tr class="trheight">
                                    <td colspan="3" class="tdcol">
                                         <dx:ASPxLabel ID="lblHeadReq" Text=" Cancel Request" runat="server" Font-Bold="true" Font-Size="Small"></dx:ASPxLabel>
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
                                    <td>&nbsp;</td>
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
                                    <td>&nbsp;</td>
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

    <dx:ASPxPopupControl ID="popupReq" ClientInstanceName="popupReq" runat="server" Modal="true" AllowDragging="true" ShowPageScrollbarWhenModal="true"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="Approval Confirmation" >
            <%-- CloseButtonImage-Url="~/Images/AirAsia/close_button_icon.jpg"--%>  
            <%--CloseAction="CloseButton"--%>

        <ClientSideEvents Shown="popupReq_Shown" />
        <CloseButtonImage Url="~/Images/AirAsia/close_button_icon.jpg"></CloseButtonImage>

        <ClientSideEvents Shown="popupReq_Shown"></ClientSideEvents>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlReq" runat="server">
                <dx:ASPxCallbackPanel ID="callbackPanelReq" ClientInstanceName="callbackPanelReq" runat="server"
                    Width="566px" Height="400px" OnCallback="callbackPanelReq_Callback" RenderMode="Table">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <dx:ASPxLoadingPanel runat="server" ID="LoadingPanelReq" ClientInstanceName="LoadingPanelReq" Modal="True">
                            </dx:ASPxLoadingPanel>    
                            <br />
                                       
                            <table width="100%" class="tableClass">
                                <tr>
                                    <td colspan="3">
                                        <dx:ASPxLabel ID="lblHeadApp" Text=" Cancel Approval" runat="server" Font-Bold="true" Font-Size="Small"></dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="lblApproveIDText" Text="Transaction ID" runat="server"></dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxLabel ID="lblApproveID" runat="server"></dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>    
                                    <td style="width: 196px" valign="top">Reason</td>
                                    <td>
                                        <dx:ASPxMemo ID="MemoApprove" runat="server" Height="102px" MaxLength="200" Width="349px">
                                            <ValidationSettings SetFocusOnError="True">                                                 
                                                <RequiredField ErrorText="Reason is required" IsRequired="True" />
                                                <RequiredField IsRequired="True" ErrorText="Reason is required"></RequiredField>
                                            </ValidationSettings>
                                        </dx:ASPxMemo>    
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                            </table> 

                            <%--added by jiakang--%>
                            <table runat="server" id="tableHistory" width="100%" visible="false">
                                <tr class="trheight">
                                    <td class="tdcol" colspan="3" >
                                        <dx:ASPxLabel ID="lblHeadHistory" Text="History Request" runat="server" Font-Bold="true" Font-Size="Small"></dx:ASPxLabel>
                                    </td>
                                </tr>    
                                <tr>
                                    <td colspan="2">
                                        <dx:ASPxGridView ID="gvReasonHistory" runat="server" AutoGenerateColumns="False" ClientInstanceName="gvReasonHistory" KeyFieldName="ReqID" Width="100%">
                                            <Columns>                                                
                                                <dx:GridViewDataColumn Caption="Request ID" FieldName="ReqID" 
                                                    ShowInCustomizationForm="True" VisibleIndex="1" Visible="false">
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn Caption="Agent ID" FieldName="TransID" 
                                                    ShowInCustomizationForm="True" VisibleIndex="2" Width="110px">
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn Caption="Request Type" FieldName="CodeDesc" VisibleIndex="0" GroupIndex="0" 
                                                    ShowInCustomizationForm="True">
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn Caption="Reason of Request" FieldName="Remark" 
                                                    ShowInCustomizationForm="True" VisibleIndex="3" Width="120px">
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataDateColumn Caption="Request Date" FieldName="RequestDate" 
                                                    ShowInCustomizationForm="True" VisibleIndex="4" Width="110px">
                                                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy"></PropertiesDateEdit>
                                                </dx:GridViewDataDateColumn>
                                                <dx:GridViewDataColumn Caption="Request By" FieldName="UserID" 
                                                    ShowInCustomizationForm="True" VisibleIndex="5">
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn Caption="Approved By" FieldName="ApprovedBy" 
                                                    ShowInCustomizationForm="True" VisibleIndex="6">
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataDateColumn Caption="Approved By" FieldName="ApprovedDate" 
                                                    ShowInCustomizationForm="True" VisibleIndex="7">
                                                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy"></PropertiesDateEdit>
                                                </dx:GridViewDataDateColumn>
                                            </Columns>                                            
                                            <SettingsBehavior ColumnResizeMode="Control"></SettingsBehavior>
                                            <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                                <%-- <GroupSummary>
                                                <dx:ASPxSummaryItem FieldName="LineTotal" SummaryType="Count" DisplayFormat="{0}" />                                               
                                            </GroupSummary>--%>
                                            <TotalSummary>
                                                <dx:ASPxSummaryItem FieldName="LineTotal" SummaryType="Count" DisplayFormat="{0}" />                                               
                                            </TotalSummary>
                                            <Settings ShowHorizontalScrollBar="True" />
                                            <Settings ShowVerticalScrollBar="True" VerticalScrollableHeight="200"></Settings>
                                        </dx:ASPxGridView>
                                    </td>
                                </tr>
                            </table>
                            <br />
                          
                            <table runat="server" id="tableFlight" width="100%" visible="false">
                                <tr >
                                    <td colspan="3" >
                                        <dx:ASPxLabel ID="lblFlight" Text="Detail Flight" runat="server" Font-Bold="true" Font-Size="Small"></dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding:0px 4px 4px 0px;width: 196px">
                                        <dx:ASPxLabel ID="lblFlightNoText" Text="Flight No" runat="server"></dx:ASPxLabel>
                                    </td>
                                    <td style="padding:0px 4px 4px 0px" >
                                        <dx:ASPxLabel ID="lblFlightNo" runat="server"></dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding:0px 4px 4px 0px;width: 196px">
                                        <dx:ASPxLabel ID="lblJourneyText" Text="Journey" runat="server"></dx:ASPxLabel>
                                    </td>
                                    <td >
                                        <dx:ASPxLabel ID="lblJourney" runat="server"></dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding:0px 4px 4px 0px;width: 196px">
                                        <dx:ASPxLabel ID="lblPNRText" Text="Record Locator" runat="server"></dx:ASPxLabel>
                                    </td>
                                    <td >
                                        <dx:ASPxLabel ID="lblPNR" runat="server"></dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding:0px 4px 4px 0px;width: 196px">
                                        <dx:ASPxLabel ID="lblStdDateText" Text="Departure Date" runat="server"></dx:ASPxLabel>
                                    </td>
                                    <td >
                                        <dx:ASPxLabel ID="lblStdDate" runat="server"></dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding:0px 4px 4px 0px;width: 196px">
                                        <dx:ASPxLabel ID="lblTotalText" Text="Total Amount" runat="server"></dx:ASPxLabel>
                                    </td>
                                    <td style="padding:0px 4px 4px 0px">
                                        <dx:ASPxLabel ID="lblTotal" runat="server"></dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr >
                                    <td style="width: 135px" >&nbsp;</td>
                                    <td >&nbsp;</td>
                                </tr>
                            </table>

                            <table runat="server" id="tablePaymentExpiry" width="100%" visible="false">
                                <tr>
                                    <td colspan="3" >
                                        <dx:ASPxLabel ID="lblHeaderPE" Text="Set Payment Expiry" runat="server" Font-Bold="true" Font-Size="Small"></dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
	                                <td style="padding:0px 4px 4px 0px;width:196px">
		                                <dx:ASPxLabel ID="lblPaymentCurrent" Text="Current Expiry" runat="server"></dx:ASPxLabel>&nbsp;&nbsp;&nbsp;
	                                </td>
	                                <td style="padding:0px 4px 4px 0px">
                                        <dx:ASPxLabel ID="lblCurrentExpiryDate" Text="" runat="server"></dx:ASPxLabel>
	                                </td>
                                </tr>                            
                                <tr>
	                                <td style="padding:0px 4px 4px 0px;width:196px">
		                                <dx:ASPxLabel ID="lblPaymentExpiryText" Text="Payment Expiry" runat="server"></dx:ASPxLabel>&nbsp;&nbsp;&nbsp;
	                                </td>
	                                <td style="padding:0px 4px 4px 0px">
                                        <dx:ASPxDateEdit ID="txtPaymentExpiry" runat="server" Width="150" EditFormat="Custom" 
                                            UseMaskBehavior="false" DisplayFormatString="dd MMM yyyy HH:mm" EditFormatString="dd MMM yyyy HH:mm" />
	                                </td>
                                </tr>
                                <tr >
                                    <td>&nbsp;</td>
                                    <td >&nbsp;</td>
                                </tr>
                            </table>

                            <table runat="server" id="tableAgent" width="100%" visible="false" class="tableClass">
                                <tr>
                                    <td colspan="3" style="padding:0px 4px 10px 0px">
                                        <dx:ASPxLabel ID="lblHeadAgent" Text="Detail Agent" runat="server" Font-Bold="true" Font-Size="Small"></dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding:0px 4px 4px 0px;width:196px">
                                        <dx:ASPxLabel ID="lblAgentNameText" Text="Agent Name" runat="server"></dx:ASPxLabel>
                                    </td>
                                    <td style="padding:0px 4px 4px 0px">
                                        <dx:ASPxLabel ID="lblAgentName" runat="server"></dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr >
                                    <td style="padding:0px 4px 4px 0px;width:196px">
                                        <dx:ASPxLabel ID="lblAddressText" Text="Address" runat="server"></dx:ASPxLabel></td>
                                    <td style="padding:0px 4px 4px 0px">
                                        <dx:ASPxLabel ID="lblAddress"  runat="server"></dx:ASPxLabel></td>
                                </tr>
                                <tr >
                                    <td style="padding:0px 4px 4px 0px;width:196px">
                                        <dx:ASPxLabel ID="lblPhoneText" Text="Phone No" runat="server"></dx:ASPxLabel></td>
                                    <td style="padding:0px 4px 4px 0px">
                                        <dx:ASPxLabel ID="lblPhone"  runat="server"></dx:ASPxLabel></td>
                                </tr>                             
                                <tr >
                                    <td style="padding:0px 4px 4px 0px;width:196px">
                                        <dx:ASPxLabel ID="lblJoinDateText" Text="Join Date" runat="server"></dx:ASPxLabel></td>
                                    <td style="padding:0px 4px 4px 0px">
                                        <dx:ASPxLabel ID="lblJoinDate"  runat="server"></dx:ASPxLabel></td>
                                </tr> 
                            </table>

                            <table runat ="server" id="tableAmountPenalty" width ="100%" visible ="false ">
                                <tr>
                                    <td style="padding:0px 4px 4px 0px;width:196px;">                                         
                                        <dx:ASPxLabel ID="lblAmPenaltyText" Text="Amount of Penalty" runat="server"></dx:ASPxLabel>
                                    </td>
                                    <td >
                                        <dx:ASPxLabel ID="lblAmPenalty" runat="server"></dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr >
                                    <td>&nbsp;</td>
                                    <td >&nbsp;</td>
                                </tr>
                            </table>

                            <table runat="server" id="tableBlackList" width="100%" visible="false">
                                <tr>
                                    <td style="padding:0px 4px 4px 0px;width:196px;">                                         
                                        <dx:ASPxLabel ID="lblBlackListDateText" Text="Blacklist Date" runat="server"></dx:ASPxLabel>
                                    </td>
                                    <td >
                                        <dx:ASPxLabel ID="lblBlackListDate" runat="server"></dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr >
                                    <td>&nbsp;</td>
                                    <td >&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="padding:0px 4px 4px 0px;width:196px;">
                                        <dx:ASPxLabel ID="lblBlackListReasonText" Text="Blacklist Reason" runat="server"></dx:ASPxLabel>
                                    </td>
                                    <td >
                                        <dx:ASPxLabel ID="lblBlackListReason" runat="server"></dx:ASPxLabel>
                                    </td>
                                </tr>
                            </table> 
                            <br />

                            <table runat="server" id="tabButton" class="tableClass">
                                <tr>
                                    <td>
                                        <dx:ASPxButton CssClass="button_2" ID="btnApprove" runat="server"  
                                            Text="Approve" AutoPostBack="False" OnClick="btnApprove_Click" CausesValidation="true" Width="114px"   >
                                            <ClientSideEvents Click="function OnCloseButtonClick(s, e) {
                                                if(ASPxClientEdit.AreEditorsValid()) {
                                                var parentWindow = window.parent;
                                                LoadingPanel.SetText(&#39;Please Wait...&#39;);
                                                LoadingPanel.Show();
                                                parentWindow.popupReq.Hide();}}" />                                            
                                            <ClientSideEvents Click="function OnCloseButtonClick(s, e) {
                                                if(ASPxClientEdit.AreEditorsValid()) {
                                                var parentWindow = window.parent;
                                                LoadingPanel.SetText(&#39;Please Wait...&#39;);
                                                LoadingPanel.Show();
                                                parentWindow.popupReq.Hide();}}"></ClientSideEvents>
                                        </dx:ASPxButton>                             
                                    </td>
                                    <td>
                                        <dx:ASPxButton CssClass="button_2" ID="btnReject" runat="server"  
                                        Text="Reject" AutoPostBack="False" OnClick="btnReject_Click" CausesValidation="true" Width="114px"   >
                                            <ClientSideEvents Click="function OnCloseButtonClick(s, e) {
                                                if(ASPxClientEdit.AreEditorsValid()) {
                                                var parentWindow = window.parent;
                                                LoadingPanel.SetText(&#39;Please Wait...&#39;);
                                                LoadingPanel.Show();
                                                parentWindow.popupReq.Hide();}}" />                                            
                                            <ClientSideEvents Click="function OnCloseButtonClick(s, e) {
                                                if(ASPxClientEdit.AreEditorsValid()) {
                                                var parentWindow = window.parent;
                                                LoadingPanel.SetText(&#39;Please Wait...&#39;);
                                                LoadingPanel.Show();
                                                parentWindow.popupReq.Hide();}}"></ClientSideEvents>
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
                &nbsp;<dx:ASPxLabel ID="lblHeader" Text="Admin Main" runat="server"></dx:ASPxLabel>
            </span>
            <div class="col-sm-4" style="text-align:left;">
                <h4>Admin Main</h4>
            </div>
        </div>
        <div class="page-content row" runat="server" id="divAllContents">
            <table width="100%">
                <tr align="right">
                    <td style="width: 135px" ></td> 
                    <td style="width: 148px" ></td>
                    <td></td>
                    <td></td> 
                    <td class="floatRightBtn" >
                        <span style="float:right"> 
                            <dx:ASPxButton CssClass="buttonL2 noBgImg" ID="btnPassenger" runat="server" Text="Passenger" OnClick="btnPassenger_Click" AutoPostBack="true">
                                <ClientSideEvents Click="function(s, e) { LoadingPanel.Show(); }" />  
                            </dx:ASPxButton>
                        </span>
                        <span style="float:right">&nbsp;</span><span style="float:right">
                            <dx:ASPxButton CssClass="buttonL2 noBgImg" ID="btnPayment" runat="server" Text="Payment" OnClick="btnPayment_Click" AutoPostBack="true">
                                <ClientSideEvents Click="function(s, e) { LoadingPanel.Show(); }" />  
                            </dx:ASPxButton>
                        </span>
                    </td>
                </tr>
                <tr>  
                    <td style="width: 115px" >
                        <dx:ASPxComboBox ID="cmbField" runat="server" class="Select" AutoPostBack="false" IncrementalFilteringMode="StartsWith" Width="120">
                            <Items>
                                <dx:ListEditItem Text="Agent ID" Value="AgentID" Selected="true"/>
                                <dx:ListEditItem Text="Agent Name" Value="AgentName" />
                                <dx:ListEditItem Text="Record Locator" Value="RecordLocator" />
                            </Items>
                        </dx:ASPxComboBox>
                    </td> 
                    <td style="width: 148px" > 
                        <dx:ASPxTextBox ID="txtAgentID" runat="server" ></dx:ASPxTextBox>
                    </td>   
                    <td></td>     
                    <td >
                        <dx:ASPxButton CssClass="buttonL2 noBgImg search" ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click">
                            <ClientSideEvents Click="function(s, e) { LoadingPanel.Show(); }" />  
                        </dx:ASPxButton>
                    </td> 
                    <td></td>
                    <td></td>        
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="5">
                        <div id="selectMainBody" class="mainBody form">
                            <div class="redSectionHeader">
                                <div><asp:Label ID="lblGridHead" runat="server" Text="Pending for Payment"></asp:Label></div>
                            </div>
                            <dx:ASPxGridView ID="gvFinishBooking" ClientInstanceName="gridBookingFinish" runat="server"  OnCustomButtonCallback="gvFinishBooking_CustomButtonCallback"
                                KeyFieldName="TransID" Width="100%" AutoGenerateColumns="False" SettingsBehavior-ColumnResizeMode="Control" >
                                <Columns>
                                    <dx:GridViewDataColumn Name="Request" Caption="Request" VisibleIndex="0" Width="58px">
                                        <DataItemTemplate>
                                            <a href="javascript:void(0);" onclick="OnEdit(this, '<%# Container.KeyValue %>')">Cancel</a>
                                        </DataItemTemplate>
                                    </dx:GridViewDataColumn>                                                          
                                    <dx:GridViewDataColumn FieldName="TransID" Caption="Transaction ID" VisibleIndex="1" />
                                    <dx:GridViewDataColumn FieldName="AgentName" Caption="Agent Name" VisibleIndex="1" />
                                    <dx:GridViewDataDateColumn FieldName="BookingDate" Caption="Booking Date" VisibleIndex="2" Width="140px">
                                        <PropertiesDateEdit DisplayFormatString="dd MMM yyyy HH:mm"></PropertiesDateEdit>
                                    </dx:GridViewDataDateColumn>
                                    <dx:GridViewDataDateColumn FieldName="ExpiryDate" Caption="Payment Expiry" VisibleIndex="3" Width="140px">
                                        <PropertiesDateEdit DisplayFormatString="dd MMM yyyy HH:mm"></PropertiesDateEdit>
                                    </dx:GridViewDataDateColumn>
                                    <dx:GridViewDataSpinEditColumn  FieldName="CollectedAmt" 
                                        Caption="Collected Amount" VisibleIndex="7" Width="120px">
                                        <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                    
                                    </dx:GridViewDataSpinEditColumn> 
                                    <dx:GridViewDataSpinEditColumn  FieldName="TransTotalAmt" Caption="Total Amount" VisibleIndex="7">
                                        <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                    
                                    </dx:GridViewDataSpinEditColumn>                         
                                    <dx:GridViewDataColumn FieldName="DueDay" Caption="Due before STD" VisibleIndex="8" Name="DueDay" Width="120px"/>
                                    <dx:GridViewDataSpinEditColumn  FieldName="DueAmt" Caption="Due Amount" VisibleIndex="8" Name="DueAmt">
                                        <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                    
                                    </dx:GridViewDataSpinEditColumn> 
                                    <dx:GridViewCommandColumn VisibleIndex="0" Caption="Action" Width="50px">                 
                                        <CustomButtons> 
                                            <dx:GridViewCommandColumnCustomButton Text="View" ID="viewBtnFinish" /> 
                                        </CustomButtons> 
                                    </dx:GridViewCommandColumn>                  
                                </Columns>
            
                                <SettingsBehavior ColumnResizeMode="Control"></SettingsBehavior>
                                <Settings showverticalscrollbar="True" VerticalScrollableHeight="300"></Settings>
                                <styles>
                                    <header backcolor="#333333" forecolor="White">
                                    </header>
                                </styles>
                            </dx:ASPxGridView>
                            
                            <div><span class="red" >**</span> Please note that <strong>Booking Date</strong> and <strong>Expiry Date</strong> are shown in GMT(+8).</div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <div id="Div1" class="mainBody form">
                            <div class="redSectionHeader">
                                <div><asp:Label ID="LblHeaderReq" runat="server" Text="Request to Approve"></asp:Label></div>
                            </div>
                            <dx:ASPxGridView ID="gvRequest" ClientInstanceName="gvRequest" runat="server" KeyFieldName="ReqID" Width="100%" 
                                AutoGenerateColumns="False" SettingsBehavior-ColumnResizeMode="Control" OnCustomCallback="gvRequest_CustomCallback" >
                                <Columns>
                                    <dx:GridViewDataColumn Name="Approve" Caption=" " VisibleIndex="0" Width="55px">
                                        <DataItemTemplate>
                                            <a  href="javascript:void(0);" onclick="OnEditReq(this, '<%# Container.KeyValue %>')">Approve</a>
                                        </DataItemTemplate>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn FieldName="RequestDesc" Caption="Request Type"  VisibleIndex="0" GroupIndex="0" />
                                    <dx:GridViewDataColumn FieldName="TransID" Caption="Transaction ID" VisibleIndex="1" />
                                    <dx:GridViewDataColumn FieldName="UserName" Caption="User Name" VisibleIndex="2" />
                                    <dx:GridViewDataColumn FieldName="Remark" Caption="Reason" VisibleIndex="3" />
                                    <dx:GridViewDataDateColumn FieldName="RequestDate" Caption="Request Date" VisibleIndex="4">
                                        <PropertiesDateEdit DisplayFormatString="dd MMM yyyy"></PropertiesDateEdit>
                                    </dx:GridViewDataDateColumn>
                                    <dx:GridViewDataColumn FieldName="ExpiryDate" Caption="Payment Expiry" VisibleIndex="5" visible="false"/>
                                    <dx:GridViewDataColumn FieldName="ReqID" Caption="Request ID" VisibleIndex="6" Visible="false" />
                                </Columns>
                                <Settings ShowGroupFooter="VisibleIfExpanded" />
                                <GroupSummary>
                                    <dx:ASPxSummaryItem FieldName="LineTotal" SummaryType="Count" DisplayFormat="{0}" />
                                </GroupSummary>
                                <SettingsBehavior ColumnResizeMode="Control"></SettingsBehavior>
                                <Settings showverticalscrollbar="True" VerticalScrollableHeight="300"></Settings>
                                <styles>
                                    <header backcolor="#333333" forecolor="White"></header>
                                </styles>
                            </dx:ASPxGridView>
                            <div><span class="red" >**</span> Please note that <strong>Booking Date</strong> and <strong>Expiry Date</strong> are shown in GMT(+8).</div>
                        </div>
                    </td>
                </tr>
                <!-- View by Authenticator -->
                <tr>
                    <td colspan="5">
                        <div id="divExpiry" runat="server" class="mainBody form">
                            <div class="redSectionHeader">
                                <div><asp:Label ID="Label1" runat="server" Text="Expiry Transaction Pending to Cancel"></asp:Label></div>
                            </div>
                            <div>
                                <table cellpadding="1" cellspacing="0" border="0">
                                    <tr>
                                        <td>
                                            <dx:ASPxButton ID="btnGetLatest" runat="server" Text="Get Latest For All" onclick="btnGetLatest_Click" Width="144px" CssClass="noBgImg">
                                            </dx:ASPxButton>
                                        </td>
                                        <td>
                                            <dx:ASPxButton ID="btnCancellationAll" runat="server" Text="Cancel Expired Booking For All" onclick="btnCancellationAll_Click" Width="197px" CssClass="noBgImg">
                                            </dx:ASPxButton>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox runat="server" ID="txtPassKey" ></dx:ASPxTextBox>
                                            <input type="hidden" runat="server" id="hPassKey" />
                                        </td>
                                        <%--<td>
                                            <dx:ASPxButton ID="btnLog" runat="server" Visible="true" Text="Info Log" onclick="btnLog_Click" ></dx:ASPxButton>
                                        </td>
                                        <td>
                                            <dx:ASPxButton ID="btnErrorLog" runat="server" Visible="true" Text="Error Log" onclick="btnErrorLog_Click"></dx:ASPxButton>
                                        </td>--%>
                                    </tr>
                                </table>
                            </div>
                            <dx:ASPxGridView runat="server" ID="gvList" OnCustomButtonCallback="gvList_CustomButtonCallback" Border-BorderWidth="1" Border-BorderColor="Black"
                                ClientInstanceName="gvList" Width="100%"  KeyFieldName="TransID" EnableRowsCache="false">
                                <Columns>
                                    <dx:GridViewCommandColumn VisibleIndex="0" Caption="Action" Width="180px">
                                        <CustomButtons>
                                            <dx:GridViewCommandColumnCustomButton Text="View" ID="ViewBtnExpiry" Image-AlternateText="View" Image-ToolTip="View" Image-Width="18px" Image-Url="../Images/AKBase/view_icon.png" /> 
                                            <dx:GridViewCommandColumnCustomButton Text="Get Latest" ID="GetLatestBtnExpiry" Image-AlternateText="Sync Navitaire" Image-ToolTip="Sync Navitaire" Image-Width="16px" Image-Url="../Images/AKBase/live_sync.png" />
                                            <dx:GridViewCommandColumnCustomButton Text="Resend" ID="ResendReminder" Image-AlternateText="Resend Payment Reminder" Image-ToolTip="Resend Payment Reminder" Image-Width="18px" Image-Url="../Images/AKBase/view_icon.png" />
                                        </CustomButtons>
                                    </dx:GridViewCommandColumn>
                                    <dx:GridViewDataColumn FieldName="TransID" Caption="Transaction ID" VisibleIndex="1" Width="110px" />
                                    <dx:GridViewDataColumn FieldName="RecordLocator" Caption="PNRs" VisibleIndex="2" Width="80px" />
                                    <dx:GridViewDataColumn FieldName="Status" Caption="Status" VisibleIndex="3"  Width="110px" />
                                    <dx:GridViewDataDateColumn FieldName="ExpiryDate" Caption="Payment Expiry" VisibleIndex="4" Width="110px">
                                        <PropertiesDateEdit DisplayFormatString="dd MMM yyyy HH:mm"></PropertiesDateEdit>
                                    </dx:GridViewDataDateColumn>
                                    <dx:GridViewDataDateColumn FieldName="STDDate" Caption="STD" VisibleIndex="5" Width="110px">
                                        <PropertiesDateEdit DisplayFormatString="dd MMM yyyy HH:mm"></PropertiesDateEdit>
                                    </dx:GridViewDataDateColumn>
                                    <dx:GridViewDataSpinEditColumn  FieldName="CollectedAmt" Caption="Collected Amt" VisibleIndex="6" Width="90px">
                                        <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                    
                                    </dx:GridViewDataSpinEditColumn> 
                                    <dx:GridViewDataSpinEditColumn  FieldName="TransTotalAmt" Caption="Total Amount" VisibleIndex="7" Width="90px">
                                        <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                    
                                    </dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataDateColumn FieldName="SyncLastUpd" Caption="Last Update" VisibleIndex="8" Width="110px">
                                        <PropertiesDateEdit DisplayFormatString="dd MMM yyyy HH:mm"></PropertiesDateEdit>
                                    </dx:GridViewDataDateColumn>
                                    <dx:GridViewDataColumn FieldName="AgentName" Caption="Agent Name" VisibleIndex="9"  Width="110px" />
                                    <dx:GridViewDataDateColumn FieldName="PaymentDateEx3" Caption="Notify Date" VisibleIndex="10" Width="110px">
                                        <PropertiesDateEdit DisplayFormatString="dd MMM yyyy HH:mm"></PropertiesDateEdit>
                                    </dx:GridViewDataDateColumn>
                                    <dx:GridViewDataDateColumn FieldName="BookingDate" Caption="Booking Date" VisibleIndex="10" Width="110px">
                                        <PropertiesDateEdit DisplayFormatString="dd MMM yyyy HH:mm"></PropertiesDateEdit>
                                    </dx:GridViewDataDateColumn>
                        
                                    <%--<dx:GridViewDataSpinEditColumn  FieldName="ExpiryDay" CellStyle-HorizontalAlign="Center" Caption="Days of Expiry" VisibleIndex="7" Name="ExpiryDay" Width="90px">
                                        <PropertiesSpinEdit DisplayFormatString="n0"></PropertiesSpinEdit>                             
                                    </dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataColumn FieldName="DueDay" Caption="Due before STD" VisibleIndex="8" Name="DueDay" Width="100px"/>
                                    <dx:GridViewDataSpinEditColumn  FieldName="DueAmt" Caption="Due Amount" VisibleIndex="9" Name="DueAmt" Width="80px">
                                        <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                    
                                    </dx:GridViewDataSpinEditColumn>--%>
                                </Columns>
                                <SettingsBehavior ColumnResizeMode="Control"  ></SettingsBehavior>
                                <Settings  ShowFilterRow="true" ShowVerticalScrollBar="true" ShowHorizontalScrollBar="true"></Settings>
                                <SettingsPager Mode="ShowAllRecords" />
                                <styles>
                                    <Row Cursor="pointer" ></Row>
                                    <header backcolor="#333333" forecolor="White"></header>
                                </styles>
                            </dx:ASPxGridView>
                            <div>
                                <dx:ASPxLabel ID="lblTotalExpiry" runat="server" Text=""></dx:ASPxLabel>
                            </div>
                            <dx:ASPxLabel runat="server" ForeColor="Red" ID="lblExpiryMsg" Width="80%" ></dx:ASPxLabel>
                        </div>
                    </td>
                </tr>

                <!-- Added by Romy -->
                <tr>
                    <td colspan="5">
                        <div id="div2" runat="server" class="mainBody form">
                            <div class="redSectionHeader">
                                <div><asp:Label ID="Label2" runat="server" Text="Pending Email to Send"></asp:Label></div>
                            </div>
                            <%--<div>
                                <table cellpadding="1" cellspacing="0" border="0">
                                    <tr>
                                        <td>
                                            <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Get Latest For All" onclick="btnGetLatest_Click" Width="144px" CssClass="noBgImg">
                                            </dx:ASPxButton>
                                        </td>
                                        <td>
                                            <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Cancel Expired Booking For All" onclick="btnCancellationAll_Click" Width="197px" CssClass="noBgImg">
                                            </dx:ASPxButton>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox runat="server" ID="ASPxTextBox1" ></dx:ASPxTextBox>
                                            <input type="hidden" runat="server" id="Hidden1" />
                                        </td>
                                    </tr>
                                </table>
                            </div>--%>
                            <!--OnCustomButtonCallback="gvpendingemail_CustomButtonCallback" -->
                            <dx:ASPxGridView runat="server" ID="gvpendingemail" Border-BorderWidth="1" Border-BorderColor="Black"
                                ClientInstanceName="gvpendingemail" Width="100%"  KeyFieldName="TransID" EnableRowsCache="false">
                                <Columns>
                                    <%--<dx:GridViewCommandColumn VisibleIndex="0" Caption="Action" Width="210px">
                                        <CustomButtons>
                                            <dx:GridViewCommandColumnCustomButton Text="View" ID="ViewBtnEmail" Image-AlternateText="View" Image-ToolTip="View" Image-Width="18px" Image-Url="../Images/AKBase/view_icon.png" /> 
                                            <dx:GridViewCommandColumnCustomButton Text="Get Latest" ID="GetLatestBtnEmail" Image-AlternateText="Sync Navitaire" Image-ToolTip="Sync Navitaire" Image-Width="16px" Image-Url="../Images/AKBase/live_sync.png" />
                                            <dx:GridViewCommandColumnCustomButton Text="Resend" ID="ResendReminderEmail" Image-AlternateText="Resend Payment Reminder" Image-ToolTip="Resend Payment Reminder" Image-Width="18px" Image-Url="../Images/AKBase/view_icon.png" />
                                        </CustomButtons>
                                    </dx:GridViewCommandColumn>--%>
                                    <dx:GridViewDataColumn FieldName="TransID" Caption="Transaction ID" VisibleIndex="1" Width="110px" />
                                    <dx:GridViewDataColumn FieldName="RecordLocator" Caption="PNRs" VisibleIndex="3" Width="80px" />
                                    <dx:GridViewDataColumn FieldName="OrgName" Caption="Agent Name" VisibleIndex="2"  Width="240px" />
                                    <dx:GridViewDataColumn FieldName="EmailAddress" Caption="Email Address" VisibleIndex="4"  Width="250px" />
                                    <dx:GridViewDataColumn FieldName="EmailFailedRemark" Caption="Status" VisibleIndex="5"  Width="200px" />
                                    <dx:GridViewDataDateColumn FieldName="AttemptCountSenderDate" Caption="Send Date" VisibleIndex="6" Width="150px">
                                        <PropertiesDateEdit DisplayFormatString="dd MMM yyyy HH:mm"></PropertiesDateEdit>
                                    </dx:GridViewDataDateColumn>
                        
                                    <%--<dx:GridViewDataSpinEditColumn  FieldName="ExpiryDay" CellStyle-HorizontalAlign="Center" Caption="Days of Expiry" VisibleIndex="7" Name="ExpiryDay" Width="90px">
                                        <PropertiesSpinEdit DisplayFormatString="n0"></PropertiesSpinEdit>                             
                                    </dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataColumn FieldName="DueDay" Caption="Due before STD" VisibleIndex="8" Name="DueDay" Width="100px"/>
                                    <dx:GridViewDataSpinEditColumn  FieldName="DueAmt" Caption="Due Amount" VisibleIndex="9" Name="DueAmt" Width="80px">
                                        <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                    
                                    </dx:GridViewDataSpinEditColumn>--%>
                                </Columns>
                                <SettingsBehavior ColumnResizeMode="Control"  ></SettingsBehavior>
                                <Settings  ShowFilterRow="true" ShowVerticalScrollBar="true" ShowHorizontalScrollBar="true"></Settings>
                                <SettingsPager Mode="ShowAllRecords" />
                                <styles>
                                    <Row Cursor="pointer" ></Row>
                                    <header backcolor="#333333" forecolor="White"></header>
                                </styles>
                            </dx:ASPxGridView>
                            <div>
                                <dx:ASPxLabel ID="lblTotalMail" runat="server" Text=""></dx:ASPxLabel>
                            </div>
                            <!--<dx:ASPxLabel runat="server" ForeColor="Red" ID="ASPxLabel2" Width="80%" ></dx:ASPxLabel>-->
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
</asp:Content>
