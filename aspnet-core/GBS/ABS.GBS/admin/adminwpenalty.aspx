<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewAdminMaster.Master" AutoEventWireup="true" CodeBehind="adminwpenalty.aspx.cs" Inherits="GroupBooking.Web.admin.adminwpenalty" %>
<%@ Register Assembly="DevExpress.Data.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=dda9a637d2fffec4" TagPrefix="dx" Namespace="DevExpress.Data" %>
 tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxGridView.v16.1" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=dda9a637d2fffec4" 
Namespace="DevExpress.Web.ASPxLoadingPanel" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.Web.v16.1" namespace="DevExpress.Web.ASPxPanel" tagprefix="dx" %>


<%@ Register assembly="DevExpress.Web.v16.1" namespace="DevExpress.Web.ASPxPopupControl" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v16.1" namespace="DevExpress.Web.ASPxCallbackPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v16.1" namespace="DevExpress.Web.ASPxLoadingPanel" tagprefix="dx" %>


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
<dx:ASPxPopupControl ID="popup" ClientInstanceName="popup" 
        runat="server" Modal="true" AllowDragging="true" 
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"        
        HeaderText="Request Confirmation" >
        <%-- CloseButtonImage-Url="~/Images/AirAsia/close_button_icon.jpg"--%>  
        <%--CloseAction="CloseButton"--%>

        <ClientSideEvents Shown="popup_Shown" />
<CloseButtonImage Url="~/Images/AirAsia/close_button_icon.jpg"></CloseButtonImage>

<ClientSideEvents Shown="popup_Shown"></ClientSideEvents>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
             <dx:ASPxCallbackPanel ID="callbackPanel" ClientInstanceName="callbackPanel" runat="server"
                    Width="566px" Height="400px" OnCallback="callbackPanel_Callback" 
                    RenderMode="Table">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                        <dx:ASPxLoadingPanel runat="server" ID="LoadingPanelPop" ClientInstanceName="LoadingPanelPop" Modal="True">
                         </dx:ASPxLoadingPanel>
                          &nbsp;&nbsp;<font color="red"> </font>
                            <table>
                            <tr class="trheight">
                                <td colspan="3" class="tdcol">
                                     <dx:ASPxLabel ID="lblHeadReq" Text=" Waive Request" runat="server" Font-Bold="true" Font-Size="Small"></dx:ASPxLabel>
                                </td>
                                </tr>
                                <tr>
                                <td>
                                &nbsp;
                                </td>
                                </tr>
                                <tr>

                                <td>
                                    <dx:ASPxLabel ID="lblTransIDText" Text="Transaction ID" runat="server"></dx:ASPxLabel>
                                </td>
                                <td>
                                    <dx:ASPxLabel ID="lblTransID" runat="server"></dx:ASPxLabel>
                                </td>
                                <tr>
                                <td>&nbsp;</td>
                                </tr>
                                </tr>
                                <tr>
                                    
                                    <td style="width: 196px">                                        
                                       Reason
                                    </td>
                                    <td>
                                        <dx:ASPxMemo ID="memoRemarks" runat="server" Height="102px" MaxLength="200" 
                                                     Width="349px">
                                            <ValidationSettings SetFocusOnError="True">                                                 
                                                 <RequiredField ErrorText="Reason is required" IsRequired="True" />
<RequiredField IsRequired="True" ErrorText="Reason is required"></RequiredField>
                                             </ValidationSettings>
                                                 </dx:ASPxMemo>    
                                    </td>

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
            <div class="div">
            &nbsp;<dx:ASPxLabel ID="lblHeader" Text="Waive Penalty" runat="server" 
                Font-Bold="True" Font-Size="Medium"></dx:ASPxLabel></div>                
            <br />
            <br />
            <table width ="100%">
               
                                <tr>
                                    <td colspan="2">
                                        <dx:ASPxGridView ID="gvAvaPenalty" runat="server" 
                                            AutoGenerateColumns="False" ClientInstanceName="gvAvaPenalty" 
                                            KeyFieldName="TransID" Width="100%">
                                            <Columns>
                                            <dx:GridViewDataColumn Name="Waive" Caption=" Waive" VisibleIndex="0" Width="55px">
                                                <DataItemTemplate>
                                                    <a  href="javascript:void(0);" onclick="OnEdit(this, '<%# Container.KeyValue %>')">
                                                        Waive</a>
                                                </DataItemTemplate>
                                            </dx:GridViewDataColumn>                                                 
                                                <dx:GridViewDataColumn Caption="Transaction ID" FieldName="TransID" 
                                                    ShowInCustomizationForm="True" VisibleIndex="1">
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn Caption="Agent Full Name" FieldName="AgentName" 
                                                    ShowInCustomizationForm="True" VisibleIndex="2">
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn Caption="Amount of Penalty" FieldName="TransTotalOth" 
                                                    ShowInCustomizationForm="True" VisibleIndex="3">
                                                </dx:GridViewDataColumn>
                                                
                                            </Columns>
                                            <settingsbehavior columnresizemode="Control" />                                            
                                            <SettingsBehavior ColumnResizeMode="Control"></SettingsBehavior>
                                             <GroupSummary>
                                                <dx:ASPxSummaryItem FieldName="LineTotal" SummaryType="Count" DisplayFormat="{0}" />                                               
                                            </GroupSummary>
                                        </dx:ASPxGridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    
                                        &nbsp;   


                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                             </table>      
         
  
          
</div>

</asp:Content>
