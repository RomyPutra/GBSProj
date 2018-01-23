<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewAdminMaster.Master" AutoEventWireup="true"
    CodeBehind="adminsetting.aspx.cs" Inherits="GroupBooking.Web.admin.settingpage" %>

<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %>
<%@ Register Assembly="SEAL.WEB" Namespace="SEAL.WEB.UI" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript">
        // <![CDATA[
        function OnRowClickPayment(e) {
            //Clear the text selection
            _aspxClearSelection();
            //Unselect all rows
            gvPayment._selectAllRowsOnPage(false);
            //Select the row
            gvPayment.SelectRow(e.visibleIndex, true);
        }
        function OnRowClickAgent(e) {
            //Clear the text selection
            _aspxClearSelection();
            //Unselect all rows
            gvAgent._selectAllRowsOnPage(false);
            //Select the row
            gvAgent.SelectRow(e.visibleIndex, true);
        }
        function OnRowClick(e) {
            //Clear the text selection
            _aspxClearSelection();
            //Unselect all rows
            grid._selectAllRowsOnPage(false);
            //Select the row
            grid.SelectRow(e.visibleIndex, true);
        }
        function CheckRestric(s, e) {
            if (s.GetChecked()) {
                chkRestriction.SetText("Enable");
            }
            else {
                chkRestriction.SetText("Disable");
            }

        }

        function updateCheckBoxState(s, e) {
            var checkState = s.GetCheckState();
            var checked = s.GetChecked();
            if (checked == true) {
                Status.SetText("Active");
            }
            else {
                Status.SetText("Inactive");
            }
        }

        function CmbBeginCallback() {
            LoadingPanel.Show();
        }

        function OnButtonClick() {
            LoadingPanel.Hide();
        }

        function OnCustomButtonClick(s, e) {
            gvGB4.AddNewRow();
        }

        function OnOrgIDChanged(s, e) {
            var obj = {};
            obj.OrgID = s.GetValue();
            $.ajax({
                type: 'POST',
                url: 'adminsetting.aspx/OrgIDChanged',
                data: JSON.stringify(obj),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (msg) {
                    var j = JSON.parse(msg.d);
                    console.log(j);
                    cmbAgentID.BeginUpdate();
                    cmbAgentID.ClearItems();
                    cmbAgentID.AddItem('', '');
                    for (var i = 0; i < j.length; i++) {
                        cmbAgentID.AddItem(j[i].optionDisplay, j[i].optionValue);
                    }
                    cmbAgentID.EndUpdate();
                    lblCountryCode.SetText(j[0].CountryCode);
                    lblCountryCode.SetEnabled(false);

                },
                error: function (data) {
                    //alert('Something Went Wrong')
                }
            });
        }

        function OrgIDChanged(value, AgentID) {
            var obj = {};
            obj.OrgID = value;
            $.ajax({
                type: 'POST',
                url: 'adminsetting.aspx/OrgIDChanged',
                data: JSON.stringify(obj),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (msg) {
                    var j = JSON.parse(msg.d);
                    cmbAgentID.BeginUpdate();
                    cmbAgentID.ClearItems();
                    cmbAgentID.AddItem('', '');
                    for (var i = 0; i < j.length; i++) {
                        cmbAgentID.AddItem(j[i].optionDisplay, j[i].optionValue);
                    }
                    cmbAgentID.EndUpdate();
                    cmbAgentID.SetText(AgentID);
                    LoadingPanel.Hide();
                },
                error: function (data) {
                    //alert('Something Went Wrong')
                }
            });
        }

        function OnCountryNameChanged(s, e) {
            var obj = {};
            obj.Country = s.GetValue();
            $.ajax({
                type: 'POST',
                url: 'adminsetting.aspx/OnCountryNameChanged',
                data: JSON.stringify(obj),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (msg) {
                    var j = JSON.parse(msg.d);
                    cmbOrigin.BeginUpdate();
                    cmbOrigin.ClearItems();
                    for (var i = 0; i < j.length; i++) {
                        cmbOrigin.AddItem(j[i].optionDisplay, j[i].optionValue);
                    }
                    cmbOrigin.EndUpdate();
                },
                error: function (data) {
                    //alert('Something Went Wrong')
                }
            });
        }

        function CountryNameChanged(value) {
            var obj = {};
            obj.Country = value;
            $.ajax({
                type: 'POST',
                url: 'adminsetting.aspx/OnCountryNameChanged',
                data: JSON.stringify(obj),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (msg) {
                    var j = JSON.parse(msg.d);
                    cmbOrigin.BeginUpdate();
                    cmbOrigin.ClearItems();
                    for (var i = 0; i < j.length; i++) {
                        cmbOrigin.AddItem(j[i].optionDisplay, j[i].optionValue);
                    }
                    cmbOrigin.EndUpdate();
                },
                error: function (data) {
                    //alert('Something Went Wrong')
                }
            });
        }

        var comand;
        function onBatchEditBeginCallback(s, e) {
            command = e.command;
            LoadingPanel.Show();
            if (command == "STARTEDIT") {
                comand = "STARTEDIT";
            }
            else if (command == "ADDNEWROW") {
                document.getElementById('ctl00_ContentPlaceHolder2_hfOrigin').value = "";
                document.getElementById('ctl00_ContentPlaceHolder2_hfUsername').value = "";

            }
        }
        function BatchEditEndEditing(s, e) {
            command = e.command;
        }

        function onBatchEditEndEditing(s, e) {
            var i = gvGB4.GetFocusedRowIndex();
            if (comand == "STARTEDIT") {
                //LoadingPanel.Hide();
                gvGB4.GetRowValues(gvGB4.GetFocusedRowIndex(), 'Username;EffectiveDate;ExpiryDate;Origin', OnGetRowValuesAgentID);
                comand = "ENDEDIT";
            }
            else if (s.cp_result != "" && typeof s.cp_result != "undefined") {
                if (s.cp_result == "Record(s) has been Deleted Successfully" || s.cp_result == "Record has been Inserted Successfully" || s.cp_result == "Record has been Updated Successfully") {
                    gvGB4.CancelEdit();
                    gvGB4.PerformCallback("Bind");

                    LoadingPanel.Hide();
                    document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = s.cp_result;
                    pcMessage.Show();
                    s.cp_result = "";
                    function changeState(event) {
                        LoadingPanel.Show();
                        window.location.href = '../admin/adminsetting.aspx';
                        LoadingPanel.Hide();
                    }

                    document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_btCancel_CD").addEventListener("click", changeState, false);
                }
                else
                {
                    LoadingPanel.Hide();
                    document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = s.cp_result;
                    pcMessage.Show();
                    s.cp_result = "";
                    if (comand == "ENDEDIT") {
                        lblCountryCode.SetEnabled(false);
                        cmbOrgName.SetEnabled(false);
                    }
                    else {
                        lblCountryCode.SetEnabled(false);
                    }
                }
            }
            else {

                LoadingPanel.Hide();
            }

        }

        function OnGetRowValuesAgentID(values) {
            console.log(values);

            daStart.SetValue(values[1]);
            daEnd.SetValue(values[2]);
            cmbOrgName.SetEnabled(false);
            lblCountryCode.SetEnabled(false);
            OrgIDChanged(cmbOrgName.GetValue(), values[0]);
            setSelectedIndex(document.getElementById("ctl00_ContentPlaceHolder2_RpanelGB4_gvGB4_DXEFL_edit" + gvGB4.GetFocusedRowIndex() + "_5_cmbOrigin"), values[3]);
            document.getElementById('ctl00_ContentPlaceHolder2_hfOrigin').value = values[3];
            document.getElementById('ctl00_ContentPlaceHolder2_hfUsername').value = values[0];
        }

        function setSelectedIndex(s, v) {

            for (var i = 0; i < s.options.length; i++) {

                if (s.options[i].value == v) {

                    s.options[i].selected = true;

                    return;

                }

            }

        }

        function UpdateInfo() {
            daEnd.Focus();
            daEnd.ShowDropDown();
            daStart.HideDropDown();
        }

        function UpdateInfoEnd() {
            daEnd.Blur();
            daEnd.HideDropDown();
            daStart.HideDropDown();
        }

        function OnBatchEditStartEditing() {
        }

        function OnGridInit() {
        }

        function OnSelectionChange(s, e) {
            if (gvGB4.GetSelectedRowCount() > 0) {
                btnDelete.SetText("DELETE (" + gvGB4.GetSelectedRowCount() + ")");
            }
            else {
                btnDelete.SetText("DELETE (0)");
            }
        }

        function OnCustomButtonDeleteClick() {
            if (gvGB4.GetSelectedRowCount() > 0) {
                popupConfirm.Show();
            }
            else {
                document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = "Please Select Record First";
                pcMessage.Show();
            }

        }

        function ClosePopup() {
            popupConfirm.Hide(true);
        }

        function YesClicked() {
            ClosePopup();
            gvGB4.PerformCallback('DeleteList');
            gvGB4.UnselectAllRowsOnPage();
            btnDelete.SetText("DELETE (0)");
            LoadingPanel.Show();
        }

      // ]]>
    </script>
    <msg:msgControl ID="msgcontrol" runat="server" />
    <div id="wrapper4" width="100%">
        <div class="div">
            &nbsp;<dx:ASPxLabel ID="lblHeader" Text="Group Booking System Configuration" runat="server"
                Font-Bold="True" Font-Size="Medium">
            </dx:ASPxLabel>
        </div>
        <hr />
    </div>
    <div class="redSectionHeader">
        <div>
            <asp:Label ID="lblGB4" runat="server" Text="GB2, GB4 Settings"></asp:Label>
        </div>
    </div>
    <dx:ASPxRoundPanel ID="RpanelGB4" ClientInstanceName="RpanelGB4" runat="server" ShowHeader="false" Width="100%"
        Height="200px" Font-Bold="True" HeaderText="GB2, GB4 Settings">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent2" runat="server">
                <table>
                    <tr>
                        <td>
                            <dx:ASPxGridView ID="gvGB4" ClientInstanceName="gvGB4" runat="server" KeyFieldName="OrgID;Origin;CountryCode" OnDataBinding="gvGB4_DataBinding" AutoGenerateColumns="False" SettingsBehavior-ColumnResizeMode="Control" EnableCallBacks="true"
                                SettingsLoadingPanel-ShowImage="false" OnRowInserting="gvGB4_RowInserting" OnRowUpdating="gvGB4_RowUpdating" OnCustomCallback="gvGB4_CustomCallback" Allow OnBeforeGetCallbackResult="gvGB4_OnBeforeGetCallbackResult">
                                <SettingsBehavior EnableRowHotTrack="True" AutoExpandAllGroups="true" ColumnResizeMode="NextColumn"
                                    AllowSelectByRowClick="true" AllowFocusedRow="true" AllowSelectSingleRowOnly="false" />
                                <Columns>
                                    <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="50px" SelectAllCheckboxMode="AllPages">
                                    </dx:GridViewCommandColumn>
                                    <dx:GridViewCommandColumn ShowEditButton="true" Width="50px">
                                    </dx:GridViewCommandColumn>
                                    <dx:GridViewDataComboBoxColumn FieldName="OrgName" Caption="Org Name" Width="250px" PropertiesComboBox-EnableCallbackMode="true" PropertiesComboBox-CallbackPageSize="10">
                                        <PropertiesComboBox ClientInstanceName="cmbOrgName">
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="GB4Form">
                                                <RequiredField IsRequired="True" ErrorText="Organitation is required" />
                                            </ValidationSettings>
                                            <ClientSideEvents Init="CmbBeginCallback" DropDown="OnButtonClick" SelectedIndexChanged="function(s, e){OnOrgIDChanged(s, e);}" />
                                        </PropertiesComboBox>
                                    </dx:GridViewDataComboBoxColumn>
                                    <dx:GridViewDataComboBoxColumn FieldName="Username" Caption="Username" Width="150px">
                                        <EditItemTemplate>
                                            <dx:ASPxComboBox ID="cmbAgentID" runat="server" ClientInstanceName="cmbAgentID">
                                            </dx:ASPxComboBox>
                                        </EditItemTemplate>
                                    </dx:GridViewDataComboBoxColumn>
                                    <dx:GridViewDataTextColumn FieldName="CountryCode" Caption="Country Code" Width="100px">
                                        <EditItemTemplate>
                                            <dx:ASPxTextBox ID="lblCountryCode" runat="server" ClientInstanceName="lblCountryCode" Text='<%# Eval("CountryCode") %>' />
                                        </EditItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataComboBoxColumn FieldName="Origin" Caption="Origin" Width="70px">
                                        <EditItemTemplate>
                                            <cc1:CustomDropDownList ID="cmbOrigin" runat="server" Width="184px"
                                                AutoPostBack="false" CssClass="czn-select" TabIndex="1" OnInit="cmbOrigin_Init">
                                            </cc1:CustomDropDownList>
                                        </EditItemTemplate>

                                    </dx:GridViewDataComboBoxColumn>
                                    <dx:GridViewDataSpinEditColumn FieldName="NoofPax" Width="70px" PropertiesSpinEdit-NumberType="Integer" Caption="No. of Pax" PropertiesSpinEdit-MinValue="1" PropertiesSpinEdit-MaxValue="50" PropertiesSpinEdit-SpinButtons-ShowIncrementButtons="false">
                                        <PropertiesSpinEdit Width="50px">
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True" ErrorDisplayMode="ImageWithTooltip" ValidationGroup="GB4Form">
                                                <RequiredField IsRequired="True" ErrorText="No. of Pax is required" />
                                            </ValidationSettings>
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataCheckColumn FieldName="Status" Caption="Status" Width="50px" CellStyle-HorizontalAlign="Right" CellStyle-VerticalAlign="Middle">
                                        <EditItemTemplate>
                                            <dx:ASPxCheckBox ID="cbStatus" Width="70" runat="server" Checked='<%# Convert.ToInt16(Eval("Status")) == 1 ? true : false %>' ClientInstanceName="Status" Text='<%# Convert.ToInt16(Eval("Status")) == 1 ? "Active" : "Inactive" %>'>
                                                <ClientSideEvents ValueChanged="updateCheckBoxState" />
                                            </dx:ASPxCheckBox>
                                        </EditItemTemplate>
                                    </dx:GridViewDataCheckColumn>
                                    <dx:GridViewDataDateColumn FieldName="EffectiveDate" Caption="Effective Start Date" PropertiesDateEdit-DisplayFormatString="dd MMM yyyy">
                                        <EditItemTemplate>
                                            <dx:ASPxDateEdit ID="daStart" ClientInstanceName="daStart" runat="server" EditFormatString="dd/MM/yyyy">
                                                <ClientSideEvents DateChanged="function(s,e) {UpdateInfo();}"></ClientSideEvents>
                                                <ValidationSettings Display="Dynamic" ValidationGroup="GB4Form" SetFocusOnError="True" CausesValidation="True" ErrorDisplayMode="ImageWithTooltip">
                                                    <RequiredField IsRequired="True" ErrorText="Start date is required"></RequiredField>
                                                </ValidationSettings>
                                            </dx:ASPxDateEdit>
                                        </EditItemTemplate>
                                    </dx:GridViewDataDateColumn>
                                    <dx:GridViewDataDateColumn FieldName="ExpiryDate" Caption="Effective End Date" PropertiesDateEdit-DisplayFormatString="dd MMM yyyy">
                                        <EditItemTemplate>
                                            <dx:ASPxDateEdit ID="daEnd" ClientInstanceName="daEnd" runat="server" EditFormatString="dd/MM/yyyy">
                                                <DateRangeSettings StartDateEditID="daStart"></DateRangeSettings>
                                                <ClientSideEvents DateChanged="function(s,e) {UpdateInfoEnd();} "></ClientSideEvents>
                                                <ValidationSettings Display="Dynamic" ValidationGroup="GB4Form" SetFocusOnError="True" CausesValidation="True" ErrorDisplayMode="ImageWithTooltip">
                                                    <RequiredField IsRequired="True" ErrorText="End date is required"></RequiredField>
                                                </ValidationSettings>
                                            </dx:ASPxDateEdit>
                                        </EditItemTemplate>
                                    </dx:GridViewDataDateColumn>
                                </Columns>
                                <ClientSideEvents SelectionChanged="OnSelectionChange" EndCallback="onBatchEditEndEditing" BeginCallback="onBatchEditBeginCallback" BatchEditStartEditing="OnBatchEditStartEditing" Init="OnGridInit" />
                                <SettingsEditing Mode="EditForm" NewItemRowPosition="Top" />
                                <SettingsPager PageSize="5" Mode="ShowPager">
                                </SettingsPager>
                                <SettingsDetail ExportMode="All" />
                                <Settings ShowGroupFooter="VisibleIfExpanded"></Settings>
                                <SettingsLoadingPanel ShowImage="false" />
                                <SettingsLoadingPanel Mode="Disabled" />
                                <SettingsDetail ExportMode="All"></SettingsDetail>
                                <Styles>
                                    <Header BackColor="#333333" ForeColor="White"></Header>
                                </Styles>
                            </dx:ASPxGridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="float: right; padding-top: 10px;">
                            <dx:ASPxButton ID="btnAddD" runat="server" Text="ADD" Width="120px"
                                AutoPostBack="False" Style="float: right; margin-right: 8px">
                                <ClientSideEvents Click="function(s, e) { OnCustomButtonClick(); }" />
                            </dx:ASPxButton>
                        </td>
                        <td colspan="2" style="float: right; padding-top: 10px;">
                            <dx:ASPxButton ID="btnDelete" runat="server" Text="DELETE (0)" Width="120px" ClientInstanceName="btnDelete"
                                AutoPostBack="False" Style="float: right; margin-right: 8px">
                                <ClientSideEvents Click="function(s, e) { OnCustomButtonDeleteClick(); }" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
                <dx:ASPxPopupControl ID="popupConfirm" runat="server" ClientInstanceName="popupConfirm"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                    CloseButtonImage-Url="~/Images/AirAsia/close_button_icon.jpg"
                    Modal="true" AllowDragging="true"
                    HeaderText="Delete Confirmation" CloseAction="CloseButton"
                    Width="250px">
                    <CloseButtonImage Url="~/Images/AirAsia/close_button_icon.jpg"></CloseButtonImage>
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table width="100%">
                                <tr>
                                    <td colspan="2">Are you sure want to Delete this record(s) ?
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <br />
                                        <table>
                                            <tr>
                                                <td>
                                                    <dx:ASPxButton ID="btnYes" runat="server" Text="Yes" Width="50px" AutoPostBack="False" ClientInstanceName="btnYes">
                                                        <ClientSideEvents Click="function(s, e) {YesClicked();}"></ClientSideEvents>

                                                    </dx:ASPxButton>
                                                </td>
                                                <td>&nbsp; </td>
                                                <td>
                                                    <dx:ASPxButton ID="btnNo" runat="server" Text="No" Width="50px" AutoPostBack="False">
                                                        <ClientSideEvents Click="function(){ClosePopup();}"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    <div class="redSectionHeader">
        <div>
            <asp:Label ID="Label1" runat="server" Text="Booking Restriction Setting"></asp:Label>
        </div>
    </div>
    <dx:ASPxRoundPanel ID="RpanelRestriction" ClientInstanceName="RpanelRestriction"
        runat="server" ShowHeader="false" Visible="True" Width="100%"
        Height="200px" Font-Bold="True" HeaderText="Agent Settings">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server">
                <table width="100%">
                    <tr>
                        <td class="tdcms1" style="padding: 10px 0;">Restriction
                        </td>
                        <td class="tdcms2" style="padding: 1px">
                            <dx:ASPxCheckBox ID="chkRestriction" ClientInstanceName="chkRestriction" Checked="true"
                                Text="Disable" runat="server">
                                <ClientSideEvents CheckedChanged="CheckRestric" />
                            </dx:ASPxCheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdcms1" style="padding: 10px 0" colspan="1">Booking Period
                            <br />
                        </td>
                        <td class="tdcms1" style="padding: 2px 5px; width: 50px">
                            <dx:ASPxLabel ID="lblFromBooking" runat="server" Text="From :">
                            </dx:ASPxLabel>
                        </td>
                        <td class="tdcms2" style="padding: 2px  5px; width: 50px">
                            <dx:ASPxDateEdit ID="txtStartDate" runat="server" Width="100px" EditFormat="Custom"
                                DisplayFormatString="dd MMM yyyy" EditFormatString="dd MMM yyyy" UseMaskBehavior="false">
                                <ValidationSettings>
                                    <RequiredField IsRequired="True" ErrorText="Start Booking Restriction date is required"></RequiredField>
                                </ValidationSettings>
                            </dx:ASPxDateEdit>
                            [00:00:00]
                        </td>
                        <td class="tdcms1" style="padding: 1px; width: 50px">
                            <dx:ASPxLabel ID="lblToBooking" runat="server" Text="To :">
                            </dx:ASPxLabel>
                        </td>
                        <td class="tdcms2" style="padding: 2px  5px;">
                            <dx:ASPxDateEdit ID="txtEndDate" runat="server" Width="100px" EditFormat="Custom"
                                DisplayFormatString="dd MMM yyyy" EditFormatString="dd MMM yyyy" UseMaskBehavior="false">
                                <ValidationSettings>
                                    <RequiredField IsRequired="True" ErrorText="End Booking Restriction date is required"></RequiredField>
                                </ValidationSettings>
                            </dx:ASPxDateEdit>
                            [23:59:59]
                        </td>
                    </tr>
                    <tr>
                        <td class="tdcms1" style="padding: 10px 0" colspan="1">Travel Period
                            <br />
                        </td>
                        <td class="tdcms1" style="padding: 2px 5px">
                            <dx:ASPxLabel ID="lblFromTravel" runat="server" Text="From :">
                            </dx:ASPxLabel>
                        </td>
                        <td class="tdcms2" style="padding: 2px  5px;">
                            <dx:ASPxDateEdit ID="txtStartDateTravel" runat="server" Width="100px" EditFormat="Custom"
                                DisplayFormatString="dd MMM yyyy" EditFormatString="dd MMM yyyy" UseMaskBehavior="false">
                                <ValidationSettings>
                                    <RequiredField IsRequired="True" ErrorText="Start Travel Period is required"></RequiredField>
                                </ValidationSettings>
                            </dx:ASPxDateEdit>
                            [00:00:00]
                        </td>
                        <td class="tdcms1" style="padding: 1px">
                            <dx:ASPxLabel ID="lblToTravel" runat="server" Text="To :">
                            </dx:ASPxLabel>
                        </td>
                        <td class="tdcms2" style="padding: 2px  5px; width: 545px;">
                            <dx:ASPxDateEdit ID="txtEndDateTravel" runat="server" Width="100px" EditFormat="Custom"
                                DisplayFormatString="dd MMM yyyy" EditFormatString="dd MMM yyyy" UseMaskBehavior="false">
                                <ValidationSettings>
                                    <RequiredField IsRequired="True" ErrorText="End Travel Period is required"></RequiredField>
                                </ValidationSettings>
                            </dx:ASPxDateEdit>
                            [23:59:59]
                        </td>
                    </tr>
                    <tr>
                        <td class="tdcms1" style="padding: 1px;">Restriction Note
                        </td>
                        <td class="tdcms2" style="padding: 2px  5px; padding-top: 16px;" colspan="4">
                            <dx:ASPxMemo ID="txtRestrictionNote" runat="server" Width="100%">
                            </dx:ASPxMemo>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdcms1" style="padding: 1px; width: 200px;">Restriction Alert Message
                        </td>
                        <td class="tdcms2" style="padding: 16px  5px;" colspan="4">
                            <dx:ASPxMemo ID="txtRestrictionAlert" runat="server" Width="100%">
                            </dx:ASPxMemo>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px"></td>
                        <td style="padding: 1px">
                            <dx:ASPxButton ID="ASPxButton1" runat="server" CssClass="button_2" Height="19px"
                                OnClick="btnSaveRestriction_Click" CausesValidation="true" ValidationContainerID="RpanelRestriction"
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
    <div class="redSectionHeader">
        <div>
            <asp:Label ID="lblGridHead" runat="server" Text="Agent Setting"></asp:Label>
        </div>
    </div>
    <dx:ASPxRoundPanel ID="RpanelAgent" ClientInstanceName="RpanelAgent" runat="server"
        ShowHeader="false" Visible="True" Width="100%" Height="200px"
        Font-Bold="True" HeaderText="Agent Settings">
        <PanelCollection>
            <dx:PanelContent ID="pnlAgent" runat="server">
                <dx:ASPxGridView ID="gvAgent" runat="server" AutoGenerateColumns="False" ClientInstanceName="gvAgent"
                    EnableCallBacks="False" EnableViewState="False" KeyFieldName="AgentCatgID" OnCustomCallback="gvAgent_CustomCallback"
                    OnDataBinding="gvAgent_DataBinding" OnSelectionChanged="gvAgent_SelectionChanged"
                    Width="100%">
                    <ClientSideEvents RowClick="function(s, e) {  LoadingPanel.SetText('Loading...');
                                                                    LoadingPanel.Show();
                                                                    OnRowClickAgent(e); }" />
                    <Columns>
                        <dx:GridViewDataColumn FieldName="AgentCatgID" ShowInCustomizationForm="True" Visible="False"
                            VisibleIndex="1" Width="70px">
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="Agent Category" FieldName="AgentCatgDesc" ShowInCustomizationForm="True"
                            VisibleIndex="1">
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="Max Enquiry" FieldName="MaxEnquiry" ShowInCustomizationForm="True"
                            VisibleIndex="2">
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="Counter Timer" FieldName="CounterTimer" ShowInCustomizationForm="True"
                            VisibleIndex="3">
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="Max Suspend" FieldName="MaxSuspend" ShowInCustomizationForm="True"
                            VisibleIndex="4">
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="Suspend Duration" FieldName="SuspendDuration" ShowInCustomizationForm="True"
                            VisibleIndex="5">
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="BlackList Duration" FieldName="BlacklistDuration"
                            ShowInCustomizationForm="True" VisibleIndex="6">
                        </dx:GridViewDataColumn>
                    </Columns>
                    <SettingsBehavior ProcessSelectionChangedOnServer="True" />
                    <SettingsPager PageSize="5">
                    </SettingsPager>
                </dx:ASPxGridView>
                <table width="100%">
                    <tr>
                        <td class="tdcms1" style="padding: 1px">
                            <dx:ASPxLabel ID="lblAgentCatgDescText" runat="server" Text="Agent Description">
                            </dx:ASPxLabel>
                        </td>
                        <td class="tdcms2" style="padding: 1px">
                            <dx:ASPxLabel ID="lblAgentCatgDesc" runat="server">
                            </dx:ASPxLabel>
                        </td>
                        <td class="tdcms1" style="padding: 1px">
                            <dx:ASPxLabel ID="lblMaxEnquiry" runat="server" Text="MaxEnquiry">
                            </dx:ASPxLabel>
                        </td>
                        <td class="tdcms2" style="padding: 1px">
                            <dx:ASPxTextBox ID="txtMaxEnquiry" runat="server" MaxLength="50" Width="150px">
                                <ValidationSettings>
                                    <RegularExpression ErrorText="Invalid Value" ValidationExpression="^([\d*]{1,20})$" />
                                    <RequiredField ErrorText="Value is required" IsRequired="True" />
                                    <RegularExpression ErrorText="Invalid Value" ValidationExpression="^([\d*]{1,20})$"></RegularExpression>
                                    <RequiredField IsRequired="True" ErrorText="Value is required"></RequiredField>
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdcms1" style="padding: 1px">
                            <dx:ASPxLabel ID="lblCounterTimer" runat="server" Text="Counter Timer">
                            </dx:ASPxLabel>
                        </td>
                        <td class="tdcms2" style="padding: 1px">
                            <dx:ASPxTextBox ID="txtCounterTimer" runat="server" MaxLength="50" Width="150px">
                                <ValidationSettings>
                                    <RegularExpression ErrorText="Invalid Value" ValidationExpression="^([\d*]{1,20})$" />
                                    <RequiredField ErrorText="Value is required" IsRequired="True" />
                                    <RegularExpression ErrorText="Invalid Value" ValidationExpression="^([\d*]{1,20})$"></RegularExpression>
                                    <RequiredField IsRequired="True" ErrorText="Value is required"></RequiredField>
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                        </td>
                        <td class="tdcms1" style="padding: 1px">
                            <dx:ASPxLabel ID="lblMaxSuspend" runat="server" Text="Max Suspend">
                            </dx:ASPxLabel>
                        </td>
                        <td class="tdcms2" style="padding: 1px">
                            <dx:ASPxTextBox ID="txtMaxSuspend" runat="server" MaxLength="50" Width="150px">
                                <ValidationSettings>
                                    <RegularExpression ErrorText="Invalid Value" ValidationExpression="^([\d*]{1,20})$" />
                                    <RequiredField ErrorText="Value is required" IsRequired="True" />
                                    <RegularExpression ErrorText="Invalid Value" ValidationExpression="^([\d*]{1,20})$"></RegularExpression>
                                    <RequiredField IsRequired="True" ErrorText="Value is required"></RequiredField>
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdcms1" style="padding: 1px">
                            <dx:ASPxLabel ID="lblSuspendDuration" runat="server" Text="Suspend Duration">
                            </dx:ASPxLabel>
                        </td>
                        <td class="tdcms2" style="padding: 1px">
                            <dx:ASPxTextBox ID="txtSuspendDuration" runat="server" MaxLength="50" Width="150px">
                                <ValidationSettings>
                                    <RegularExpression ErrorText="Invalid Value" ValidationExpression="^([\d*]{1,20})$" />
                                    <RequiredField ErrorText="Value is required" IsRequired="True" />
                                    <RegularExpression ErrorText="Invalid Value" ValidationExpression="^([\d*]{1,20})$"></RegularExpression>
                                    <RequiredField IsRequired="True" ErrorText="Value is required"></RequiredField>
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                        </td>
                        <td class="tdcms1" style="padding: 1px">
                            <dx:ASPxLabel ID="lblBlacklistDuration" runat="server" Text="Blacklist Duration">
                            </dx:ASPxLabel>
                        </td>
                        <td class="tdcms2" style="padding: 1px">
                            <dx:ASPxTextBox ID="txtBlacklistDuration" runat="server" MaxLength="50" Width="150px">
                                <ValidationSettings>
                                    <RegularExpression ErrorText="Invalid Value" ValidationExpression="^([\d*]{1,20})$" />
                                    <RequiredField ErrorText="Value is required" IsRequired="True" />
                                    <RegularExpression ErrorText="Invalid Value" ValidationExpression="^([\d*]{1,20})$"></RegularExpression>
                                    <RequiredField IsRequired="True" ErrorText="Value is required"></RequiredField>
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px"></td>
                        <td style="padding: 1px">
                            <dx:ASPxButton ID="btnSaveAgent" runat="server" CssClass="button_2" Height="19px"
                                OnClick="btnSaveAgent_Click" CausesValidation="true" ValidationContainerID="RpanelAgent"
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
    <!-- start from here added by diana 20130911 : payment setting -->
    <div class="redSectionHeader">
        <div>
            <asp:Label ID="lblGridPayment" runat="server" Text="Payment Setting"></asp:Label>
        </div>
    </div>
    <dx:ASPxRoundPanel ID="RpanelPayment" ClientInstanceName="RpanelPayment" runat="server"
        ShowHeader="false" Visible="True" Width="100%" Height="200px"
        Font-Bold="True" HeaderText="Agent Settings">
        <PanelCollection>
            <dx:PanelContent ID="pnlPayment" runat="server">
                <div style="float: right; width: 35%; margin-right: 1%; height: 35px;" align="right">
                    <dx:ASPxComboBox ID="cmGRPID" runat="server" Visible="true" AutoPostBack="true" OnSelectedIndexChanged="cmGRPID_OnSelectedIndexChanged">
                    </dx:ASPxComboBox>
                </div>
                <dx:ASPxGridView ID="gvPayment" runat="server" AutoGenerateColumns="False" ClientInstanceName="gvPayment"
                    EnableCallBacks="False" EnableViewState="False" KeyFieldName="SchemeCode" OnCustomCallback="gvPayment_CustomCallback"
                    OnDataBinding="gvPayment_DataBinding" OnSelectionChanged="gvPayment_SelectionChanged"
                    Width="100%">
                    <ClientSideEvents RowClick="function(s, e) {  LoadingPanel.SetText('Loading...');
                                                                    LoadingPanel.Show();
                                                                    OnRowClickPayment(e); }" />
                    <Columns>
                        <dx:GridViewDataColumn Caption="Code" FieldName="SchemeCode" ShowInCustomizationForm="True"
                            VisibleIndex="1" Width="70px">
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="Dur (hrs)" FieldName="Duration" ShowInCustomizationForm="True"
                            VisibleIndex="2">
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="Min Dur (hrs)" FieldName="MinDuration" ShowInCustomizationForm="True"
                            VisibleIndex="3">
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="Payment Type" FieldName="PaymentType" ShowInCustomizationForm="True"
                            VisibleIndex="3">
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="Att. 1 (hrs)" FieldName="Attempt_1" ShowInCustomizationForm="True"
                            VisibleIndex="4">
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="Code 1" FieldName="Code_1" ShowInCustomizationForm="True"
                            VisibleIndex="4">
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="Perc. 1 (%)" FieldName="Percentage_1" ShowInCustomizationForm="True"
                            VisibleIndex="5">
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="Att. 2 (hrs)" FieldName="Attempt_2" ShowInCustomizationForm="True"
                            VisibleIndex="6">
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="Code 2" FieldName="Code_2" ShowInCustomizationForm="True"
                            VisibleIndex="6">
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="Perc. 2 (%)" FieldName="Percentage_2" ShowInCustomizationForm="True"
                            VisibleIndex="7">
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="Att. 3 (hrs)" FieldName="Attempt_3" ShowInCustomizationForm="True"
                            VisibleIndex="8">
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="Code 3" FieldName="Code_3" ShowInCustomizationForm="True"
                            VisibleIndex="8">
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="Perc. 3 (%)" FieldName="Percentage_3" ShowInCustomizationForm="True"
                            VisibleIndex="9">
                        </dx:GridViewDataColumn>
                        <%-- <dx:GridViewDataColumn Caption="Description" FieldName="Description" 
                            ShowInCustomizationForm="True" VisibleIndex="10">
                        </dx:GridViewDataColumn>
                        --%>
                    </Columns>
                    <SettingsBehavior ProcessSelectionChangedOnServer="True" />
                    <SettingsPager PageSize="5">
                    </SettingsPager>
                </dx:ASPxGridView>
                <br />
                <table>
                    <tr>
                        <td style="padding: 1px; width: 120px">
                            <dx:ASPxLabel ID="lblCarrierGrp" runat="server" Text="Carrier Group">
                            </dx:ASPxLabel>
                        </td>
                        <td style="padding: 1px;" colspan="8">
                            <dx:ASPxLabel ID="lblID" runat="server">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px; width: 140px">
                            <dx:ASPxLabel ID="lblCode" runat="server" Text="Scheme Code">
                            </dx:ASPxLabel>
                        </td>
                        <td style="padding: 1px; width: 80px">
                            <dx:ASPxLabel ID="txtCode" runat="server">
                            </dx:ASPxLabel>
                        </td>
                        <td style="padding: 1px; width: 120px"></td>
                        <td style="padding: 1px; width: 200px"></td>
                        <td style="padding: 1px; width: 120px">
                            <dx:ASPxLabel ID="lblAttempt1" runat="server" Text="Attempt 1">
                            </dx:ASPxLabel>
                        </td>
                        <td style="padding: 1px; width: 60px">
                            <dx:ASPxTextBox ID="txtAttempt1" runat="server" MaxLength="50" Width="50px">
                                <ValidationSettings>
                                    <RegularExpression ErrorText="Invalid Value" ValidationExpression="^([\d*]{1,20})$" />
                                    <RequiredField ErrorText="Value is required" IsRequired="True" />
                                    <RegularExpression ErrorText="Invalid Value" ValidationExpression="^([\d*]{1,20})$"></RegularExpression>
                                    <RequiredField IsRequired="True" ErrorText="Value is required"></RequiredField>
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                        </td>
                        <td style="padding: 1px; width: 60px;">
                            <dx:ASPxComboBox runat="server" ID="cmbCode1" IncrementalFilteringMode="StartsWith"
                                ClientEnabled="true" ClientInstanceName="cmbCode1" AllowMouseWheel="False" FilterMinLength="1"
                                SelectedIndex="0" Width="50px">
                                <Items>
                                    <dx:ListEditItem Text="DOB" Value="DOB" Selected="True" />
                                    <dx:ListEditItem Text="STD" Value="STD" />
                                </Items>
                            </dx:ASPxComboBox>
                        </td>
                        <td style="padding: 1px; width: 120px">
                            <dx:ASPxLabel ID="lblPerc1" runat="server" Text="Percentage 1">
                            </dx:ASPxLabel>
                        </td>
                        <td style="padding: 1px; width: 60px">
                            <dx:ASPxTextBox ID="txtPerc1" runat="server" MaxLength="6" Width="50px">
                                <ValidationSettings>
                                    <RegularExpression ErrorText="Invalid Value" ValidationExpression="^([\d*]{1,20})$" />
                                    <RequiredField ErrorText="Value is required" IsRequired="True" />
                                    <RegularExpression ErrorText="Invalid Value" ValidationExpression="^([\d*]{1,20})$"></RegularExpression>
                                    <RequiredField IsRequired="True" ErrorText="Value is required"></RequiredField>
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px">
                            <dx:ASPxLabel ID="lblDuration" runat="server" Text="Duration">
                            </dx:ASPxLabel>
                        </td>
                        <td style="padding: 1px">
                            <dx:ASPxTextBox ID="txtDuration" runat="server" MaxLength="50" Width="50px">
                                <ValidationSettings>
                                    <RegularExpression ErrorText="Invalid Value" ValidationExpression="^([\d*]{1,20})$" />
                                    <RequiredField ErrorText="Value is required" IsRequired="True" />
                                    <RegularExpression ErrorText="Invalid Value" ValidationExpression="^([\d*]{1,20})$"></RegularExpression>
                                    <RequiredField IsRequired="True" ErrorText="Value is required"></RequiredField>
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                        </td>
                        <td style="padding: 1px">
                            <dx:ASPxLabel ID="lblMinDuration" runat="server" Text="Min. Duration">
                            </dx:ASPxLabel>
                        </td>
                        <td style="padding: 1px">
                            <dx:ASPxTextBox ID="txtMinDuration" runat="server" MaxLength="50" Width="50px">
                                <ValidationSettings>
                                    <RegularExpression ErrorText="Invalid Value" ValidationExpression="^([\d*]{1,20})$" />
                                    <RequiredField ErrorText="Value is required" IsRequired="True" />
                                    <RegularExpression ErrorText="Invalid Value" ValidationExpression="^([\d*]{1,20})$"></RegularExpression>
                                    <RequiredField IsRequired="True" ErrorText="Value is required"></RequiredField>
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                        </td>
                        <td style="padding: 1px">
                            <dx:ASPxLabel ID="lblAttempt2" runat="server" Text="Attempt 2">
                            </dx:ASPxLabel>
                        </td>
                        <td style="padding: 1px">
                            <dx:ASPxTextBox ID="txtAttempt2" runat="server" MaxLength="50" Width="50px">
                                <ValidationSettings>
                                    <RegularExpression ErrorText="Invalid Value" ValidationExpression="^([\d*]{1,20})$" />
                                    <RequiredField ErrorText="Value is required" IsRequired="True" />
                                    <RegularExpression ErrorText="Invalid Value" ValidationExpression="^([\d*]{1,20})$"></RegularExpression>
                                    <RequiredField IsRequired="True" ErrorText="Value is required"></RequiredField>
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                        </td>
                        <td style="padding: 1px">
                            <dx:ASPxComboBox runat="server" ID="cmbCode2" IncrementalFilteringMode="StartsWith"
                                ClientEnabled="true" ClientInstanceName="cmbCode2" AllowMouseWheel="False" FilterMinLength="1"
                                SelectedIndex="0" Width="50px">
                                <Items>
                                    <dx:ListEditItem Text="DOB" Value="DOB" Selected="True" />
                                    <dx:ListEditItem Text="STD" Value="STD" />
                                </Items>
                            </dx:ASPxComboBox>
                        </td>
                        <td style="padding: 1px">
                            <dx:ASPxLabel ID="lblPerc2" runat="server" Text="Percentage 2">
                            </dx:ASPxLabel>
                        </td>
                        <td style="padding: 1px">
                            <dx:ASPxTextBox ID="txtPerc2" runat="server" MaxLength="6" Width="50px">
                                <ValidationSettings>
                                    <RegularExpression ErrorText="Invalid Value" ValidationExpression="^([\d*]{1,20})$" />
                                    <RequiredField ErrorText="Value is required" IsRequired="True" />
                                    <RegularExpression ErrorText="Invalid Value" ValidationExpression="^([\d*]{1,20})$"></RegularExpression>
                                    <RequiredField IsRequired="True" ErrorText="Value is required"></RequiredField>
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px">
                            <dx:ASPxLabel ID="lblType" runat="server" Text="Payment Type">
                            </dx:ASPxLabel>
                        </td>
                        <td style="padding: 1px" colspan="3">
                            <dx:ASPxComboBox runat="server" ID="cmbType" IncrementalFilteringMode="StartsWith"
                                ClientEnabled="true" ClientInstanceName="cmbType" AllowMouseWheel="False" FilterMinLength="1"
                                SelectedIndex="0" Width="60px">
                                <Items>
                                    <dx:ListEditItem Text="SVCF" Value="SVCF" Selected="True" />
                                    <dx:ListEditItem Text="FULL" Value="FULL" />
                                </Items>
                            </dx:ASPxComboBox>
                        </td>
                        <td style="padding: 1px">
                            <dx:ASPxLabel ID="lblAttempt3" runat="server" Text="Attempt 3">
                            </dx:ASPxLabel>
                        </td>
                        <td style="padding: 1px">
                            <dx:ASPxTextBox ID="txtAttempt3" runat="server" MaxLength="50" Width="50px">
                                <ValidationSettings>
                                    <RegularExpression ErrorText="Invalid Value" ValidationExpression="^([\d*]{1,20})$" />
                                    <RequiredField ErrorText="Value is required" IsRequired="True" />
                                    <RegularExpression ErrorText="Invalid Value" ValidationExpression="^([\d*]{1,20})$"></RegularExpression>
                                    <RequiredField IsRequired="True" ErrorText="Value is required"></RequiredField>
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                        </td>
                        <td style="padding: 1px">
                            <dx:ASPxComboBox runat="server" ID="cmbCode3" IncrementalFilteringMode="StartsWith"
                                ClientEnabled="true" ClientInstanceName="cmbCode3" AllowMouseWheel="False" FilterMinLength="1"
                                SelectedIndex="0" Width="50px">
                                <Items>
                                    <dx:ListEditItem Text="DOB" Value="DOB" Selected="True" />
                                    <dx:ListEditItem Text="STD" Value="STD" />
                                </Items>
                            </dx:ASPxComboBox>
                        </td>
                        <td style="padding: 1px">
                            <dx:ASPxLabel ID="lblPerc3" runat="server" Text="Percentage 3">
                            </dx:ASPxLabel>
                        </td>
                        <td style="padding: 1px">
                            <dx:ASPxTextBox ID="txtPerc3" runat="server" MaxLength="6" Width="50px">
                                <ValidationSettings>
                                    <RegularExpression ErrorText="Invalid Value" ValidationExpression="^([\d*]{1,20})$" />
                                    <RequiredField ErrorText="Value is required" IsRequired="True" />
                                    <RegularExpression ErrorText="Invalid Value" ValidationExpression="^([\d*]{1,20})$"></RegularExpression>
                                    <RequiredField IsRequired="True" ErrorText="Value is required"></RequiredField>
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <%--
<tr>
<td style="padding:1px"><dx:ASPxLabel ID="lblDesc" runat="server" Text="Description"></dx:ASPxLabel></td>
<td style="padding:1px" colspan="8"><dx:ASPxTextBox ID="txtDesc" runat="server" MaxLength="255" Width="500px" ></dx:ASPxTextBox> </td>
</tr>
                    --%>
                    <tr>
                        <td style="padding: 1px"></td>
                        <td style="padding: 1px">
                            <dx:ASPxButton ID="btnSavePayment" runat="server" CssClass="button_2" Height="19px"
                                OnClick="btnSavePayment_Click" CausesValidation="true" ValidationContainerID="RpanelPayment"
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
    <!-- end here -->
    <div class="redSectionHeader">
        <div>
            <asp:Label ID="lblHeadGeneral" runat="server" Text="General Setting"></asp:Label>
        </div>
    </div>
    <%--<dx:ASPxPageControl SkinID="None" Width="100%" EnableViewState="false" ID="TabControl"
        runat="server" ActiveTabIndex="1" EnableDefaultAppearance="True" AutoPostBack="True"
            TabSpacing="0px" ContentStyle-Border-BorderWidth="0" 
        EnableHierarchyRecreation="True" onActiveTabChanged="TabControl_ActiveTabChanged"
            OnActiveTabChanging="TabControl_ActiveTabChanging" Height="22px">
        <ContentStyle>
        <Paddings Padding="0px" PaddingBottom="0px" PaddingTop="0px" PaddingLeft="0px" PaddingRight="0px" />
        <Border BorderWidth="1px"/>
        <BorderBottom BorderWidth ="0px" />
        </ContentStyle>
        <TabPages>
             <dx:TabPage Text="Payment">
            </dx:TabPage>
            <dx:TabPage Text="Reminder">
            </dx:TabPage>
             <dx:TabPage Text="Charge">
            </dx:TabPage>                  
        </TabPages> 
  </dx:ASPxPageControl>  
    --%>
    <dx:ASPxRoundPanel ID="RPanelData" ClientInstanceName="RPanelData" runat="server"
        ShowHeader="false" Visible="True" Width="100%" Height="200px">
        <PanelCollection>
            <dx:PanelContent ID="pnlDetail" runat="server">
                <div style="float: right; width: 35%; margin-right: 2%; height: 35px;" align="right">
                    <dx:ASPxComboBox ID="cmbGRPID" runat="server" Visible="true" AutoPostBack="true"
                        OnSelectedIndexChanged="cmbGRPID_OnSelectedIndexChanged">
                    </dx:ASPxComboBox>
                </div>
                <br />
                <br />
                <table height="200px">
                    <tr>
                        <td class="tdcms1" valign="top" style="padding: 1px">&nbsp;&nbsp; Carrier Group
                        </td>
                        <td class="tdcms2" valign="top" style="padding: 1px">
                            <dx:ASPxLabel ID="lblWebID" runat="server">
                            </dx:ASPxLabel>
                            &nbsp;
                        </td>
                        <td style="padding: 1px"></td>
                        <td rowspan="6" valign="top" style="padding: 1px">
                            <dx:ASPxGridView ID="grid" runat="server" ClientInstanceName="grid" OnCustomCallback="grid_CustomCallback"
                                OnDataBinding="grid_DataBinding" EnableViewState="false" KeyFieldName="CompositeKey"
                                OnSelectionChanged="grid_SelectionChanged" EnableCallBacks="false" Width="100%">
                                <ClientSideEvents RowClick="function(s, e) {  LoadingPanel.SetText('Loading...');
                                                                            LoadingPanel.Show();
                                                                            OnRowClick(e); }"></ClientSideEvents>
                                <Columns>
                                    <dx:GridViewDataColumn FieldName="CompositeKey" VisibleIndex="1" Width="70px" Visible="false" />
                                    <dx:GridViewDataColumn FieldName="GRPID" VisibleIndex="1" Width="70px" Visible="false" />
                                    <dx:GridViewDataColumn FieldName="SYSKey" VisibleIndex="2" />
                                    <dx:GridViewDataColumn FieldName="SYSValue" VisibleIndex="3" />
                                </Columns>
                                <SettingsBehavior AllowFocusedRow="False" />
                                <SettingsBehavior AllowMultiSelection="False" />
                                <SettingsBehavior ProcessSelectionChangedOnServer="True" />
                                <ClientSideEvents RowClick="function(s, e) { LoadingPanel.SetText('Loading...');
                                                                            LoadingPanel.Show();
                                                                             OnRowClick(e); }" />
                                <SettingsBehavior ProcessSelectionChangedOnServer="True"></SettingsBehavior>
                                <SettingsPager PageSize="50">
                                </SettingsPager>
                            </dx:ASPxGridView>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdcms1" style="padding: 1px" valign="top">&nbsp;&nbsp; Setting
                        </td>
                        <td class="tdcms2" style="padding: 1px" valign="top">
                            <dx:ASPxLabel ID="lblSYSKey" runat="server">
                            </dx:ASPxLabel>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="padding: 1px">&nbsp;&nbsp; Setting Description&nbsp;&nbsp;&nbsp;
                        </td>
                        <td valign="top" style="padding: 1px">
                            <dx:ASPxLabel ID="lblSettingDesc" runat="server">
                            </dx:ASPxLabel>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="padding: 1px">&nbsp;&nbsp; Setting Value
                        </td>
                        <td valign="top" style="padding: 1px">
                            <dx:ASPxTextBox ID="txtSysValue" runat="server" MaxLength="50" Width="150px">
                                <ValidationSettings>
                                    <RegularExpression ErrorText="Invalid Value" ValidationExpression="^([\d*]{1,20})$" />
                                    <RequiredField ErrorText="Value is required" IsRequired="True" />
                                    <RegularExpression ErrorText="Invalid Value" ValidationExpression="^([\d*]{1,20})$"></RegularExpression>
                                    <RequiredField IsRequired="True" ErrorText="Value is required"></RequiredField>
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 1px">&nbsp;
                        </td>
                        <td valign="top">
                            <dx:ASPxButton ID="btnSave" runat="server" CssClass="button_2" Height="19px" OnClick="btnSubmit_Click"
                                CausesValidation="true" ValidationContainerID="RPanelData" Text="Save" Width="88px">
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
        <%--
        <Border  BorderWidth="1px" BorderColor="#999999" BorderStyle="Solid" />
        <BorderLeft  BorderWidth="1px" BorderStyle="Solid" />
        <BorderTop BorderWidth="0px" />
        <BorderRight BorderWidth="1px" BorderStyle="Solid" />
        <BorderBottom  BorderWidth="1px" BorderStyle="Solid" />
        --%>
    </dx:ASPxRoundPanel>
    <asp:HiddenField ID="hfOrigin" runat="server" Value="" />
    <asp:HiddenField ID="hfUsername" runat="server" Value="" />
</asp:Content>
