<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewAdminMaster.Master" AutoEventWireup="true"
    CodeBehind="agentlist.aspx.cs" Inherits="GroupBooking.Web.Administrator.AgentList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
  <script type="text/javascript">
    // <![CDATA[
      function gridAgent_SelectionChanged(s, e) {
          s.GetSelectedFieldValues("AgentID", GetSelectedFieldValuesCallback);
      }
      function GetSelectedFieldValuesCallback(values) {

          try {

              hidClient.value = "";
              hidClient.Set("black", "");
              for (var i = 0; i < values.length; i++) {
                  if (hidClient.Get("black") == "")
                      hidClient.Set("black", values[i]);
                  else
                      hidClient.Set("black", hidClient.Get("black") + ";" + values[i]);
              }
          }
          catch (e) {
          }
      }

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
                        <dx:PanelContent ID="PanelContent2" runat="server">
                        <dx:ASPxLoadingPanel runat="server" ID="LoadingPanelPop" ClientInstanceName="LoadingPanelPop" Modal="True">
                         </dx:ASPxLoadingPanel>
                          &nbsp;&nbsp;<font color="red"> </font>
                            <table class="tableClass">
                            <tr>
                                <td colspan="2">
                                     <dx:ASPxLabel ID="lblHeadReq" Text=" Blacklist Request" runat="server" Font-Bold="true" Font-Size="Small"></dx:ASPxLabel>
                                </td>
                                </tr>
                                <tr>
                                <td>
                                &nbsp;
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
                                        <td valign="top">
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
                                            <dx:ASPxButton ID="btnRequest" runat="server" AutoPostBack="false" 
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
            &nbsp;<dx:ASPxLabel ID="lblHeader" Text="Agent Management" runat="server" 
                Font-Bold="True" Font-Size="Medium"></dx:ASPxLabel></div>

                <dx:ASPxHiddenField runat="server" ClientInstanceName="hidClient" ID="hidClient">
        
    </dx:ASPxHiddenField>
    <br />
    <table width="100%">
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
                <Items>
                    <dx:ListEditItem Selected="True" Text="All" Value="All" />
                    <dx:ListEditItem Text="Agent ID" Value="AgentID" />
                    <dx:ListEditItem Text="Username" Value="Username" />
                    <dx:ListEditItem Text="Contact First Name" Value="ContactFirstName" />
                    <dx:ListEditItem Text="Contact Last Name" Value="ContactLastName" />
                    <dx:ListEditItem Text="Email" Value="Email" />
                    <dx:ListEditItem Text="Country" Value="countryName" />
                    <dx:ListEditItem Text="License Number" Value="LicenseNo" />
                    <dx:ListEditItem Text="Mobile Number" Value="MobileNo" />
                    <dx:ListEditItem Text="Phone Number" Value="PhoneNo" />
                    <dx:ListEditItem Text="Agent Status" Value="AgStatus" />
                </Items>
            </dx:ASPxComboBox>
            
            </td>
            <td class="style5" colspan="3">
                <dx:ASPxButton CssClass="button_2" ID="btnSearch" runat="server" OnClick="btnSearch_Click" 
                    Text="Search">
                </dx:ASPxButton>
                
            </td>
            <td class="style4" width="210" id="tdSearch" runat="server" Visible="False">
            <dx:ASPxTextBox ID="txtSearch" runat="server" Width="200" >
            </dx:ASPxTextBox>
            
            </td>
            <td class="style5" width="175px" align="right">
                <dx:ASPxButton CssClass="button_3"  ID="btnSubmit" runat="server" Text="Move To Whitelist" 
                        OnClick="btnSubmit_Click" Height="19px" Width="160px"  >
                        <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />                                                           
                     </dx:ASPxButton>
            </td>
            <td width="80">
            
            <dx:ASPxComboBox ID="cmbAgStatus" runat="server" SelectedIndex="0" Width="71"
                OnSelectedIndexChanged="cmbAgStatus_SelectedIndexChanged" AutoPostBack="false" Visible ="false">
                <ClientSideEvents SelectedIndexChanged="function(s, e) { LoadingPanel.Show(); }" />  
                <Items>
                    <dx:ListEditItem Selected="True" Text="Active" Value="Active" />
                    <dx:ListEditItem Text="Inactive" Value="Inactive" />
                    
                </Items>
            </dx:ASPxComboBox>
            
            </td>
            
            
            
            
            </tr>
            <tr>
                <td>
                <dx:ASPxCheckBox ID="chkBlacklist"  runat="server" Text="Blacklist" 
                    TextAlign="Right" ></dx:ASPxCheckBox>
                </td>
            </tr>
            </table>
        </dx:PanelContent> 
        </PanelCollection> 
        </dx:ASPxRoundPanel> 
          
        </td>
          <td rowspan ="1" valign="top" class="style8">
          
          </td>
    </tr>
        <tr>
     
            <td valign="top" class="style11">
          <div id="selectMainBody" class="mainBody form">
                <div class="redSectionHeader">
                  <div><asp:Label ID="lblGridHead" runat="server" Text="Agent List"></asp:Label></div>
                </div>       
        <dx:ASPxGridView ID="gridAgent" ClientInstanceName="gridAgent" runat="server" OnCommandButtonInitialize="gridAgent_CommandButtonInitialize"
         OnCustomCallback="gridAgent_CustomCallback" KeyFieldName="AgentID" Width="100%" OnCustomButtonCallback="gridAgent_CustomButtonCallback"
                    AutoGenerateColumns="False" SettingsBehavior-ColumnResizeMode="Control" >
<ClientSideEvents SelectionChanged="gridAgent_SelectionChanged"></ClientSideEvents>
            <Columns>            
                <dx:GridViewCommandColumn Caption="Choose" ShowSelectCheckbox="True" VisibleIndex="0" Name="rowCheckBox" Visible="false" Width="60px">
                </dx:GridViewCommandColumn>                  
                   <dx:GridViewDataColumn Name="Request" Caption="Request" VisibleIndex="0" Width="60px">
                        <DataItemTemplate>
                        <a href="javascript:void(0);" id="LinkRequest" onclick="OnEdit(this, '<%# Container.KeyValue %>')" onloadeddata="RequestLink_Load">
                        Blacklist</a>
              
                        </DataItemTemplate>
                   </dx:GridViewDataColumn>
                       
                <dx:GridViewCommandColumn VisibleIndex="0" Caption="Action" Width="60px" Visible="false">                 
                 <CustomButtons> 
                     <dx:GridViewCommandColumnCustomButton Text="Edit" ID="editBtn" /> 
                 </CustomButtons>
                 </dx:GridViewCommandColumn> 
                 
                <dx:GridViewDataColumn FieldName="AgentID" Caption = "Agent ID" VisibleIndex="1" Width="130px" />
                <dx:GridViewDataColumn FieldName="Username" Caption = "Username" VisibleIndex="2" Width="205px" />
                <dx:GridViewDataColumn FieldName="ContactFirstName" Caption = "First Name" VisibleIndex="3" Width="125px" />
                <dx:GridViewDataColumn FieldName="ContactLastName" Caption = "Last Name" VisibleIndex="4" Width="125px" />
                <dx:GridViewDataColumn FieldName="Email" Caption = "Email" VisibleIndex="5" Width="250px">
                    <CellStyle Wrap="True">
                    </CellStyle>
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="countryName" Caption="Country name" VisibleIndex="6" Visible="false"/>
                <dx:GridViewDataColumn FieldName="LicenseNo" Caption = "License No." VisibleIndex="7" Width="103px" />                
                <dx:GridViewDataColumn FieldName="MobileNo" Caption = "Mobile no." VisibleIndex="8" Visible="false" />                
                <dx:GridViewDataColumn FieldName="PhoneNo" Caption = "Phone no." VisibleIndex="9" Visible="false" /> 
            </Columns>
            <ClientSideEvents SelectionChanged="gridAgent_SelectionChanged" />
            <Settings ShowHorizontalScrollBar="True" />

<SettingsBehavior ColumnResizeMode="Control"></SettingsBehavior>

<Settings ShowHorizontalScrollBar="True"></Settings>
            <styles>
                <header backcolor="#333333" forecolor="White">
                </header>
            </styles>
        </dx:ASPxGridView>
                
            </td>
            <td class="style12" valign="top">
            <asp:Panel ID="pnlBlacklist" runat="server" Visible="true" >
                 <div>
        <dx:ASPxListBox ID="listboxBlacklist" ClientInstanceName="selList" runat="server" Height="200px" Width="100%">
            <Columns>                
                <dx:ListBoxColumn Caption="Agent ID" Width="50px"/>
            </Columns>
        </dx:ASPxListBox>
    </div>    
     <div class="TopPadding">
            Selected Agents : <span id="selCount" style="font-weight: bold">0</span>
        </div>    
       </asp:Panel>
    </td>
        </tr>
   
   <tr>
        <td align="left" colspan="2">
            <span style="float:left;">
                    </span>                
                    <dx:ASPxButton CssClass="button_3"  ID="btnInsert" runat="server" Text="Insert New Agent" 
                         Height="19px" Width="160px" Visible="False"  >
                        <ClientSideEvents Click="function(s, e) { LoadingPanel.Show(); }" />                                                           
                     </dx:ASPxButton>
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
