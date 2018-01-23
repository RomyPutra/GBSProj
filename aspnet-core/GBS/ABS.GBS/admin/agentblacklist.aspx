<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewAdminMaster.Master" AutoEventWireup="true"
    CodeBehind="~/admin/agentblacklist.aspx.cs" Inherits="GroupBooking.Web.Administrator.AgentBlacklist" %>

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
    // ]]> 
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
                        <dx:PanelContent runat="server">
                        <dx:ASPxLoadingPanel runat="server" ID="LoadingPanelPop" ClientInstanceName="LoadingPanelPop" Modal="True">
                         </dx:ASPxLoadingPanel>
                          &nbsp;&nbsp;<font color="red"> </font>
                            <table class="tableClass">
                            <tr>
                                <td colspan="2">
                                     <dx:ASPxLabel ID="lblHeadReq" Text=" Whitelist Request" runat="server" Font-Bold="true" Font-Size="Small"></dx:ASPxLabel>
                                    <br /><br />
                                </td>
                                </tr>
                                <tr>

                                <td>
                                    <dx:ASPxLabel ID="lblTransIDText" Text="Agent ID" runat="server"></dx:ASPxLabel>
                                </td>
                                <td>
                                    <dx:ASPxLabel ID="lblTransID" runat="server"></dx:ASPxLabel>
                                </td>
                                </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="lblAgentNameText" runat="server" Text="Agent Name">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxLabel ID="lblAgentName" runat="server">
                                            </dx:ASPxLabel>
                                            </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 196px" valign="top">
                                            Reason
                                        </td>
                                        <td>
                                            <dx:ASPxMemo ID="memoRemarks" runat="server" Height="102px" MaxLength="200" 
                                                Width="349px">
                                                <validationsettings setfocusonerror="True">
                                                    <requiredfield errortext="Reason is required" isrequired="True" />
<RequiredField IsRequired="True" ErrorText="Reason is required"></RequiredField>
                                                </validationsettings>
                                            </dx:ASPxMemo>
                                        </td>
                                    </tr>
                                    <tr>
                                    <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxButton ID="btnRequest" runat="server" AutoPostBack="False" 
                                                CssClass="button_2" OnClick="btnRequest_Click" Text="Request">
                                                <clientsideevents click="function OnCloseButtonClick(s, e) {
                                      if(ASPxClientEdit.AreEditorsValid()) {
                                      var parentWindow = window.parent;
                                      LoadingPanelPop.SetText('Please Wait...');
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
    <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="true" ContainerElementID="FormPanel"/>
    <div id="wrapper4" >
            <div class="div">
            &nbsp;<dx:ASPxLabel ID="lblHeader" Text="Blacklisted Agent Management" runat="server" 
                Font-Bold="True" Font-Size="Medium"></dx:ASPxLabel></div>

                <dx:ASPxHiddenField runat="server" ClientInstanceName="hidClient" ID="hidClient">
        
    </dx:ASPxHiddenField>
    <br />
    <tr>
    <td>
   
    </td>
    </tr>
    <tr>
        <td >
        <dx:ASPxRoundPanel ID="RPanelData" runat="server" HeaderText="Search" ShowHeader="false" 
                Visible="True" Width="100%" Border-BorderColor="#666666" Border-BorderWidth="1" Border-BorderStyle="Solid">            
                <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server">
        <table >
        <tr>
        <td width="180">
            
            <dx:ASPxComboBox ID="cmbFilter" runat="server" SelectedIndex="0" 
                OnSelectedIndexChanged="cmbFilter_SelectedIndexChanged" AutoPostBack="true">
                <ClientSideEvents SelectedIndexChanged="function(s, e) { LoadingPanel.Show(); }" />  
<ClientSideEvents SelectedIndexChanged="function(s, e) { LoadingPanel.Show(); }"></ClientSideEvents>
                <Items>
                    <dx:ListEditItem Selected="True" Text="All" Value="All" />
                    <dx:ListEditItem Text="Agent ID" Value="AgentID" />
                    <dx:ListEditItem Text="Username" Value="Username" />
                    <dx:ListEditItem Text="Contact First Name" Value="ContactFirstName" />
                    <dx:ListEditItem Text="Contact Last Name" Value="ContactLastName" />                  
                </Items>
            </dx:ASPxComboBox>
            
            </td>
            <td  width="210" id="tdSearch" runat="server" Visible="False">                                         
                <dx:ASPxTextBox ID="txtSearch" runat="server" Width="200" >
                </dx:ASPxTextBox>            
            </td>
            
            <td class="style5" colspan="3">
                <dx:ASPxButton CssClass="button_2" ID="btnSearch" runat="server" OnClick="btnSearch_Click" AutoPostBack="true"
                    Text="Search">
                </dx:ASPxButton>
                
            </td>
            </table>
        </dx:PanelContent> 
        </PanelCollection> 

<Border BorderColor="#666666" BorderStyle="Solid" BorderWidth="1px"></Border>
        </dx:ASPxRoundPanel> 
          
        </td>
          <td rowspan ="1" valign="top" class="style8">
          
          </td>
    </tr>
        <tr>
     
            <td valign="top" class="style11" height="300px">
          <div id="selectMainBody" class="mainBody form">
                <div class="redSectionHeader">
                  <div><asp:Label ID="lblGridHead" runat="server" Text="Agent List"></asp:Label></div>
                </div>       
        <dx:ASPxGridView ID="gridAgent" ClientInstanceName="gridAgent" runat="server" OnCommandButtonInitialize="gridAgent_CommandButtonInitialize"
            KeyFieldName="AgentID" Width="100%" OnCustomButtonCallback="gridAgent_CustomButtonCallback" 
                    AutoGenerateColumns="False" SettingsBehavior-ColumnResizeMode="Control" >
            <Columns>            
                 
                <dx:GridViewCommandColumn VisibleIndex="0" Caption="Action" Name="Action" Width="60px">                 
                 <CustomButtons> 
                     <dx:GridViewCommandColumnCustomButton Text="Whitelist" ID="editBtn" /> 
                 </CustomButtons> 
                 </dx:GridViewCommandColumn> 
                    <dx:GridViewDataColumn Name="Request" Caption="Request" VisibleIndex="0" Width="60px">
                        <DataItemTemplate>
                        <a href="javascript:void(0);" onclick="OnEdit(this, '<%# Container.KeyValue %>')">
                        Whitelist</a>
                        </DataItemTemplate>
                   </dx:GridViewDataColumn>   
                 
                <dx:GridViewDataColumn FieldName="AgentID" Caption = "Agent ID" VisibleIndex="1" Width="130px" />                
                <dx:GridViewDataColumn FieldName="FullName" Caption = "Agent Name" VisibleIndex="2" Width="150px" />                
                <dx:GridViewDataDateColumn FieldName="BlacklistDate" Caption = "Blacklist Date" VisibleIndex="3">
                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy"></PropertiesDateEdit>
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataColumn FieldName="Remark" Caption = "Reason" VisibleIndex="4"  Width="250px" />
                <dx:GridViewDataDateColumn FieldName="BlacklistExpiryDate" Caption = "Blacklist Expiry" VisibleIndex="5">
                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy HH:mm"></PropertiesDateEdit>
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataColumn FieldName="Email" Caption = "Email" VisibleIndex="6" Width="200px" />

                
                <dx:GridViewDataColumn FieldName="countryName" Caption="Country name" VisibleIndex="7" Visible="false"/>
                <dx:GridViewDataColumn FieldName="LicenseNo" Caption = "License No." VisibleIndex="8" />                
                <dx:GridViewDataColumn FieldName="MobileNo" Caption = "Mobile no." VisibleIndex="9" Visible="false" />                
                <dx:GridViewDataColumn FieldName="PhoneNo" Caption = "Phone no." VisibleIndex="10" Visible="false" /> 
            </Columns>          
            <Settings ShowHorizontalScrollBar="True" />
<SettingsBehavior ColumnResizeMode="Control"></SettingsBehavior>

            <Settings ShowVerticalScrollBar="True" VerticalScrollableHeight="300" />
            <Styles>
                <Header BackColor="#333333" ForeColor="White">
                </Header>
            </Styles>
        </dx:ASPxGridView>
                
            </td>
            <td class="style12" valign="top">
           
    </td>
        </tr>
   
   <tr>
        <td align="left" colspan="2">
            <span style="float:left;">
                    </span>                
        </td>
   </tr>     
   <tr>
        <td colspan="2">
             <dx:ASPxLabel ID="lblError" runat ="server" Visible = "false" ForeColor="Red"> </dx:ASPxLabel>
        </td>
   </tr>
    </table>
   </div>
    
      

</asp:Content>
