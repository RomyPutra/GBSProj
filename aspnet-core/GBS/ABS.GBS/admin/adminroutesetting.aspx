<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewAdminMaster.Master" AutoEventWireup="true" CodeBehind="adminroutesetting.aspx.cs" Inherits="GroupBooking.Web.admin.adminroutesetting" %>
<%@ Register Assembly="SEAL.WEB" Namespace="SEAL.WEB.UI" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

 <script type="text/javascript">
    // <![CDATA[

    function OnRowClickFlight(e) {
        //Clear the text selection
        _aspxClearSelection();
        //Unselect all rows
        gvFlight._selectAllRowsOnPage(false);
        //Select the row
        gvFlight.SelectRow(e.visibleIndex, true);
    }
     // ]]>
    </script>
      <div id="wrapper4" width="100%" >
    <div class="div">
            &nbsp;<dx:ASPxLabel ID="lblHeader" Text="Route Setting Page" runat="server" 
                Font-Bold="True" Font-Size="Medium"></dx:ASPxLabel></div>
                <hr />
                <br />
    </div> 
 
  
                <div class="redSectionHeader">
                  <div><asp:Label ID="lblGridHead" runat="server" Text="Route List"></asp:Label></div>
                </div>
  
<dx:ASPxRoundPanel ID="RpanelFlight" ClientInstanceName="RpanelFlight" runat="server" ShowHeader="false" ShowDefaultImages="false"
        Visible="True" Width="100%" Height="200px" Font-Bold="True" 
        HeaderText="Flight Settings" >            
        <PanelCollection>
            <dx:PanelContent ID="pnlFlight" runat="server">
                     <table width="100%">
                     <tr>                     
                        <td width="650px">
                        <dx:ASPxComboBox ID="cmbGRPID" 
                            runat="server"  Visible="true" AutoPostBack="true" OnSelectedIndexChanged="cmbGRPID_OnSelectedIndexChanged">

                        </dx:ASPxComboBox>
                        </td>
                        <td>
                        
                        <dx:ASPxCheckBox ID="chkFilterActive" runat="server" ClientInstanceName="checkBox" Text="Active only" EnableViewState="false" Checked="true">       
                        </dx:ASPxCheckBox>
                        
                        </td>
                     </tr>
                     </table>
                     <br />
                <dx:ASPxGridView ID="gvFlight" runat="server" AutoGenerateColumns="False" 
                    ClientInstanceName="gvFlight" EnableCallBacks="False" EnableViewState="False" 
                    KeyFieldName="SectorSuspendID" OnCustomCallback="gvFlight_CustomCallback" 
                    OnDataBinding="gvFlight_DataBinding" 
                    OnSelectionChanged="gvFlight_SelectionChanged" Width="100%">
                    <ClientSideEvents RowClick="function(s, e) {  LoadingPanel.SetText('Loading...');
                                                                    LoadingPanel.Show();
                                                                    OnRowClickFlight(e); }" />
                    <Columns>
                        <dx:GridViewDataColumn FieldName="SectorSuspendID" ShowInCustomizationForm="True" 
                            Visible="False" VisibleIndex="1" Width="90px">
                        </dx:GridViewDataColumn>                 
                        <dx:GridViewDataColumn Caption="Agent Group" FieldName="AgentGroup" 
                            ShowInCustomizationForm="True" VisibleIndex="2">
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="Origins" FieldName="Origins" 
                            ShowInCustomizationForm="True" VisibleIndex="3" Width="200px">
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="Destination" FieldName="Destination" 
                            ShowInCustomizationForm="True" VisibleIndex="4" Width="200px">
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="Effective Start Date" FieldName="EffectiveStartDate" 
                            ShowInCustomizationForm="True" VisibleIndex="5">
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="Effective End Date" 
                            FieldName="EffectiveEndDate" ShowInCustomizationForm="True" VisibleIndex="6">
                        </dx:GridViewDataColumn>
                    </Columns>
                    <SettingsBehavior ProcessSelectionChangedOnServer="True" />
                    <SettingsPager PageSize="5">
                    </SettingsPager>
                </dx:ASPxGridView>
                <br />
                <table width="100%">
                  <tr>
                    <td class="td1" style="height: 25px">  
                        <dx:ASPxLabel ID="lblGroupCode" runat="server" Text="Operation Group"></dx:ASPxLabel></td>
                    <td class="td2" style="height: 25px">                        
                        <dx:ASPxComboBox ID="cmbOptGroup" 
                            runat="server"  Visible="true" AutoPostBack="true" OnSelectedIndexChanged="cmbOptGroup_OnSelectedIndexChanged">
                        </dx:ASPxComboBox>
                    </td>
                </tr>
                 <tr>
                    <td class="td1" style="height: 25px">  
                        <dx:ASPxLabel ID="lblAgentGroup" runat="server" Text="Agent Group"></dx:ASPxLabel></td>
                    <td class="td2" style="height: 25px">                        
                        <dx:ASPxComboBox ID="cmbAgentGroup" 
                            runat="server"  Visible="true" >
                        </dx:ASPxComboBox>
                    </td>
                </tr>
                <tr>
                    <td class="td1">
                       <dx:ASPxLabel ID="lblOrigins" runat="server" Text="Origins"></dx:ASPxLabel></td>
                    <td class="td2">
                        <cc1:customdropdownlist ID="ddlOrigin" runat="server" Width="184px"
                            OnSelectedIndexChanged="ddlOrigin_SelectedIndexChanged" 
                            AutoPostBack="true" > </cc1:customdropdownlist>                                                                        
                    </td>
                </tr>  
                <tr>
                    <td class="td1">  
                        <dx:ASPxLabel ID="lblDestination" runat="server" Text="Destination"></dx:ASPxLabel></td>
                    <td class="td2">                        
                        <cc1:CustomDropDownList ID="ddlDestination" runat="server" Width="184px">
                        <asp:ListItem Text="Select City"></asp:ListItem>
                        </cc1:CustomDropDownList>                  
                    </td>
                </tr>
                <tr>
                    <td class="td1">  
                        <dx:ASPxLabel ID="lblStartDate" runat="server" Text="Effective Start Date"></dx:ASPxLabel></td>
                    <td class="td2">                        
                         <dx:ASPxDateEdit ID="txtStartDate" runat="server" Width="150" EditFormat="Custom" UseMaskBehavior="false" />         
                    </td>
                </tr>    
                 <tr>
                    <td class="td1">  
                        <dx:ASPxLabel ID="lblEndDate" runat="server" Text="Effective End Date"></dx:ASPxLabel></td>
                    <td class="td2">                        
                         <dx:ASPxDateEdit ID="txtEndDate" runat="server" Width="150" EditFormat="Custom" UseMaskBehavior="false" />         
                    </td>
                </tr>    
                 <tr>
                    <td class="td1">  
                      </td>
                    <td class="td2">                        
                          <dx:ASPxCheckBox ID="chkActive" runat="server" ClientInstanceName="checkBox" Text="Active" EnableViewState="false" Checked="true">       
                        </dx:ASPxCheckBox>    
                    </td>
                </tr>    
                <tr>
                <td>
                &nbsp;
                </td>
                </tr>
                <tr>
                <td class="td1">
                </td>
                <td class="td2">
                <dx:ASPxButton ID="btnSaveAgent" runat="server" CssClass="button_2" 
                              Height="19px" OnClick="btnSaveAgent_Click" CausesValidation="true" ValidationContainerID="RpanelAgent" 
                             Text="Save" Width="88px">
                             <ClientSideEvents Click="function(s, e) { 
                                                                if(ASPxClientEdit.AreEditorsValid()) {
                                                                
                                                                // Prevent multiple presses of the button
                                                                    LoadingPanel.SetText('Sending data to the server...');
                                                                    LoadingPanel.Show();
                                                                }
                                                             }" />
                                <ClientSideEvents Click="function(s, e) { 
                                                                if(ASPxClientEdit.AreEditorsValid()) {
                                                                
                                                                // Prevent multiple presses of the button
                                                                    LoadingPanel.SetText(&#39;Sending data to the server...&#39;);
                                                                    LoadingPanel.Show();
                                                                }
                                                             }"></ClientSideEvents>
                         </dx:ASPxButton>
                </td>
                </tr>
                </table> 
    </dx:PanelContent> 
    </PanelCollection>
    </dx:ASPxRoundPanel>

    
</asp:Content>
