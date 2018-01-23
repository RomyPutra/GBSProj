<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewAdminMaster.Master" AutoEventWireup="true" CodeBehind="reportlist.aspx.cs" Inherits="GroupBooking.Web.admin.reportlist" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">
        // <![CDATA[

        function OnRowClick(e) {
            //Clear the text selection
            //_aspxClearSelection();
            //Unselect all rows
            //grid._selectAllRowsOnPage(false);
            grid.UnselectAllRowsOnPage();
            //Select the row
            //grid.SelectRow(e.visibleIndex, true);
            grid.SelectRowOnPage(e.visibleIndex, true);
        }

        function OnRowClickAdd(e) {
            //Clear the text selection
            //_aspxClearSelection();
            //Unselect all rows
            //gvAddData._selectAllRowsOnPage(false);
            gvAddData.UnselectAllRowsOnPage();
            //Select the row
            //gvAddData.SelectRow(e.visibleIndex, true);
            gvAddData.SelectRowOnPage(e.visibleIndex, true);
        }
        // ]]>
    </script>

    <div id="wrapper4" >
        <div class="div">
        </div>

        <div class="page-header row">
            <span style="display:none;">         
                &nbsp; &nbsp;<dx:ASPxLabel ID="lblHeader" Text="Report List" runat="server" Font-Bold="True" Font-Size="Medium"></dx:ASPxLabel>
            </span>
            <div class="col-sm-4" style="text-align:left;">
                <h4>Report List</h4>
            </div>
        </div>

        <div class="page-content row">
            <input id="HiddenRowIndex" name="HiddenRowIndex" type="hidden" runat="server" />
            <table width="1000">
                <tr>
                    <td valign="top" width="40%">
                        <dx:ASPxGridView ID="grid" runat="server" ClientInstanceName="grid" OnCustomCallback="grid_CustomCallback" OnDataBinding="grid_DataBinding" 
                            EnableViewState="false" KeyFieldName="RptCode" onselectionchanged="grid_SelectionChanged" EnableCallBacks="false" Width="388px" >
                            <Columns>
                                <dx:GridViewDataColumn FieldName="RptCode" VisibleIndex="1" Width="70px" />
                                <dx:GridViewDataColumn FieldName="RptName" VisibleIndex="2" />
                            </Columns>                        
                            <SettingsBehavior AllowFocusedRow="False" />
                            <SettingsBehavior AllowMultiSelection="False" />
                            <SettingsBehavior ProcessSelectionChangedOnServer="True"  />
                            <ClientSideEvents RowClick="function(s, e) {LoadingPanel.Show();  OnRowClick(e); }" />                     
                            <SettingsPager PageSize="50"></SettingsPager> 
                        </dx:ASPxGridView>         
                    </td>
                    <td width="60%">
                        <dx:ASPxRoundPanel ID="RPanelData" runat="server" HeaderText="Report Details" Visible="True" Width="500" >            
                            <PanelCollection>
                                <dx:PanelContent ID="pnlDetail" runat="server">
                                    <table>
                                        <tr>
                                            <td style="width: 150px">Report Type</td>
                                            <td>
                                                <dx:ASPxLabel ID="lblReportType" runat="server"></dx:ASPxLabel><br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td style="width: 150px">Report Code</td>
                                            <td>
                                                <dx:ASPxLabel ID="lblReportCode" runat="server"></dx:ASPxLabel><br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td style="width: 150px">Report Description</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <dx:ASPxMemo ID="memoDesc" runat="server" Width="470px" Height="70px"></dx:ASPxMemo>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="right" colspan="2">
                                                <dx:ASPxButton CssClass="buttonL2 noBgImg" ID="btnShow" runat="server" AutoPostBack="true" Text="Show" onclick="btnShow_Click"  CausesValidation="false"  >
                                                    <ClientSideEvents Click="function(s, e) { LoadingPanel.Show(); }" />  
                                                    <ClientSideEvents Click="function(s, e) { LoadingPanel.Show(); }"></ClientSideEvents>
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 290px">
                                                <div style="display:inline-block;vertical-align:top;padding-top:3px;">Filter by:</div>
                                                <div style="display:inline-block">
                                                    <dx:ASPxComboBox ID="cmbFilter" runat="server" AutoPostBack="True" class="Select" EnableIncrementalFiltering="True" 
                                                        IncrementalFilteringMode="StartsWith" OnSelectedIndexChanged="cmb_SelectionChanged" Width="140px">
                                                        <ClientSideEvents  SelectedIndexChanged="function(s, e) {LoadingPanel.Show();   }" />
                                                        <ValidationSettings ValidationGroup="valComboFilter">
                                                            <RequiredField ErrorText="Filter by column is required" IsRequired="True" />
                                                            <RequiredField IsRequired="True" ErrorText="Filter by column is required"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                </div>
                                                <asp:UpdatePanel ID="UpdPanelCmb" runat="server">
                                                </asp:UpdatePanel>
                                            </td>
                                            <td> <!-- added by jia kang -->
                                                <dx:ASPxLabel ID="lblStart" runat="server" Text="Start" visible="false"></dx:ASPxLabel>
                                                <asp:UpdatePanel ID="UpdPanelFilter" runat="server">
                                                    <ContentTemplate>
                                                        <dx:ASPxTextBox ID="txtFilter"  runat="server" Visible="false">
                                                            <ClientSideEvents KeyUp="function(s, e) {s.SetText(s.GetText().toUpperCase());}" />
                                                        </dx:ASPxTextBox>
                                                        <dx:ASPxComboBox ID="cmbCarrier" runat="server" SelectedIndex="0" Visible="false">
                                                            <%--<Items>
                                                                <dx:ListEditItem Selected="True" Text="All" Value="ALL" />
                                                                <dx:ListEditItem Text="AK" Value="AK" />
                                                                <dx:ListEditItem Text="D7" Value="D7" />
                                                                <dx:ListEditItem Text="FD" Value="FD" />
                                                                <dx:ListEditItem Text="QZ" Value="QZ" />
                                                                <dx:ListEditItem Text="PQ" Value="PQ" />
                                                                <dx:ListEditItem Text="JW" Value="JW" />
                                                                <dx:ListEditItem Text="XJ" Value="XJ" />
                                                                <dx:ListEditItem Text="Z2" Value="Z2" />
                                                                <dx:ListEditItem Text="I5" Value="I5" />
                                                            </Items>--%>
                                                        </dx:ASPxComboBox>
                                                        <dx:ASPxDateEdit ID="txtStartDate" runat="server" Width="150" EditFormat="Custom" UseMaskBehavior="false" Visible="false" DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy"/> 
                                                        <asp:Panel ID="PnlStart" runat="server" Visible="false">
                                                            <table>
                                                                <tr>
                                                                    <td>Month : </td>
                                                                    <td style="padding-left:2px;padding-right:2px">
                                                                        <dx:ASPxComboBox ID="cmbMonthStart" width="40" runat="server" SelectedIndex="0" Visible="false">
                                                                            <Items>
                                                                                <%--<dx:ListEditItem Selected="True" Text="All" Value="0" />--%>
                                                                                <dx:ListEditItem Text="01" Value="01" />
                                                                                <dx:ListEditItem Text="02" Value="02" />
                                                                                <dx:ListEditItem Text="03" Value="03" />
                                                                                <dx:ListEditItem Text="04" Value="04" />
                                                                                <dx:ListEditItem Text="05" Value="05" />
                                                                                <dx:ListEditItem Text="06" Value="06" />            
                                                                                <dx:ListEditItem Text="07" Value="07" />
                                                                                <dx:ListEditItem Text="08" Value="08" />
                                                                                <dx:ListEditItem Text="09" Value="09" />
                                                                                <dx:ListEditItem Text="10" Value="10" />
                                                                                <dx:ListEditItem Text="11" Value="11" />
                                                                                <dx:ListEditItem Text="12" Value="12" />       
                                                                            </Items>
                                                                        </dx:ASPxComboBox>
                                                                    </td>
                                                                    <td>Year :</td>
                                                                    <td style="padding-left:3px">
                                                                        <dx:ASPxComboBox ID="cmbYearStart" width="60" runat="server" SelectedIndex="0" Visible="false">
                                                                        </dx:ASPxComboBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 19px">&nbsp;</td>
                                            <td style="height: 19px">
                                                <dx:ASPxLabel ID="lblEnd" runat="server" Text="End" visible="false"></dx:ASPxLabel>
                                                <asp:UpdatePanel ID="UpdPanelEnd" runat="server">
                                                    <ContentTemplate>
                                                        <dx:ASPxDateEdit ID="txtEndDate" runat="server" Width="150" EditFormat="Custom" UseMaskBehavior="false" Visible="false" DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy"/>  
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <asp:Panel ID="PnlEnd" runat="server" Visible="false">
                                                    <table>
                                                        <tr>
                                                            <td>Month : </td>
                                                            <td style="padding-left:2px;padding-right:2px;">
                                                                <dx:ASPxComboBox ID="cmbMonthEnd" runat="server" SelectedIndex="0" Visible="False" Width="40px">
                                                                    <Items>
                                                                        <%--<dx:ListEditItem Selected="True" Text="All" Value="0" />--%>
                                                                        <dx:ListEditItem Text="01" Value="01" />
                                                                        <dx:ListEditItem Text="02" Value="02" />
                                                                        <dx:ListEditItem Text="03" Value="03" />
                                                                        <dx:ListEditItem Text="04" Value="04" />
                                                                        <dx:ListEditItem Text="05" Value="05" />
                                                                        <dx:ListEditItem Text="06" Value="06" />
                                                                        <dx:ListEditItem Text="07" Value="07" />
                                                                        <dx:ListEditItem Text="08" Value="08" />
                                                                        <dx:ListEditItem Text="09" Value="09" />
                                                                        <dx:ListEditItem Text="10" Value="10" />
                                                                        <dx:ListEditItem Text="11" Value="11" />
                                                                        <dx:ListEditItem Text="12" Value="12" />
                                                                    </Items>
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                            <td>Year :</td>
                                                            <td style="padding-left:3px">
                                                                <dx:ASPxComboBox ID="cmbYearEnd" width="60" runat="server" SelectedIndex="0" Visible="false">
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dx:ASPxButton CssClass="buttonL2 noBgImg" ID="btnAdd" runat="server" AutoPostBack="true" Text="Add" onclick="btnAdd_Click"   CausesValidation="true">
                                                    <ClientSideEvents Click="function(s, e) {if(ASPxClientEdit.AreEditorsValid()) { LoadingPanel.Show(); }}" />  
                                                    <ClientSideEvents Click="function(s, e) {if(ASPxClientEdit.AreEditorsValid()) { LoadingPanel.Show(); }}"></ClientSideEvents>
                                                </dx:ASPxButton>       
                                            </td>
                                            <td>
                                                <dx:ASPxButton CssClass="buttonL2 noBgImg" ID="btnRemove" runat="server" AutoPostBack="true" Text="Remove" onclick="btnRemove_Click"   >
                                                    <ClientSideEvents Click="function(s, e) { if(ASPxClientEdit.AreEditorsValid()) {LoadingPanel.Show(); }}" />  
                                                    <ClientSideEvents Click="function(s, e) { if(ASPxClientEdit.AreEditorsValid()) {LoadingPanel.Show(); }}"></ClientSideEvents>
                                                </dx:ASPxButton>       
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <dx:ASPxGridView ID="gvAddData" runat="server" ClientInstanceName="gvAddData" KeyFieldName="FieldName" Width="475px" >
                                                    <Columns>
                                                        <dx:GridViewDataColumn FieldName="FieldName" VisibleIndex="1" Width="90px"/>
                                                        <dx:GridViewDataColumn FieldName="Value" VisibleIndex="2" />
                                                    </Columns>
                                                    <SettingsBehavior AllowFocusedRow="False" />
                                                    <SettingsBehavior AllowMultiSelection="False" />
                                                    <SettingsBehavior ProcessSelectionChangedOnServer="True"  />
                                                    <ClientSideEvents RowClick="function(s, e) { OnRowClickAdd(e); }" />
                                                    <SettingsBehavior ProcessSelectionChangedOnServer="True"></SettingsBehavior>
                                                </dx:ASPxGridView>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:PanelContent> 
                            </PanelCollection> 
                        </dx:ASPxRoundPanel> 
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>


