<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewPageMaster.Master" AutoEventWireup="true" CodeBehind="passengerdetail.aspx.cs" Inherits="GroupBooking.Web.passengerdetail" %>

<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %>
<%@ Register Src="../UserControl/flightbookingdetail.ascx" TagName="ucflightbookingdetail" TagPrefix="FBD" %>
<%@ Register Src="../UserControl/PaxDetail.ascx" TagName="ucPaxDetail" TagPrefix="PD" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <script type="text/javascript" src="../Scripts/Booking/PassengerDetail.js?ver=1.4"></script>
    <script type='text/javascript'>
        $(document).keyup(function (e) {
            if (e.keyCode == 113) { // F2 key maps to keycode `113`
                $(':focus').blur();
            }
        });
    </script>
    <msg:msgControl ID="msgcontrol" runat="server" />
    <div class="row page-header clearfix">
        <div class="col-sm-3">
            <h4 class="mt-0 mb-5"><span runat="server" id="spanTitle">Passenger Upload</span>
                <%--<dx:ASPxLabel runat="server" ID="lblPassengerDetails" Text="Passenger Upload"></dx:ASPxLabel>--%>
            </h4>
            Booking/<dx:ASPxLabel runat="server" ID="lblPassengerDetailsSub" Text="Passenger Upload"></dx:ASPxLabel>
        </div>
        <div  id="divindicator" runat="server" class="col-sm-3" style="text-align:left !important;padding-top:20px !important;padding-bottom:20px !important;background:#f5f5f5;margin-top:5px;border-radius:4px;">
            <dx:ASPxLabel runat="server" ID="indicator" Text=""></dx:ASPxLabel>
        </div>
        <div class="col-sm-6">
            <div align="right" style="padding-top: 9px;">
                <table id="bookingDetail">
                    <tr>
                        <td>
                            <a  href="../Guide/Upload_Passenger_User_Guide.pdf" target="_blank">
                            <dx:ASPxButton CssClass="buttonL2 noBgImg" ID="btnDlGuide" runat="server" ClientInstanceName="btnDlGuide" UseSubmitBehavior="False" Text="User Guide">
                            </dx:ASPxButton>
                            </a>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <dx:ASPxButton CssClass="buttonL2 noBgImg" ID="btnDl" runat="server" ClientInstanceName="btnDl" UseSubmitBehavior="False" Text="Download" OnClick="btnDl_Click">
                            </dx:ASPxButton>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <dx:ASPxButton CssClass="buttonL2 noBgImg" ID="btnUpload" runat="server" AutoPostBack="False" Text="Upload" ClientInstanceName="btnUpload" ClientEnabled="False">
                                <ClientSideEvents Click="function(s, e) { uploader.Upload(); }"  />
                            </dx:ASPxButton>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <dx:ASPxButton CssClass="buttonL2 noBgImg" ID="btConfirm" runat="server" ClientInstanceName="btConfirm" Text="Confirm" AutoPostBack="False">
                                <%--20170324 - Sienny (full page loading => LoadingPanel.Show(); added --%>
                                <ClientSideEvents Click="function(s, e) { LoadingPanel.Show(); OnBtnConfirmClick(); }" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </div>
        </div>

    </div>

    <div class="row page-header clearfix" style="padding-top: 20px; display: none;" id="mainContainer" runat="server">
    </div>
    <div class="widget page-content container-fluid">
        <div class="row">
            <div class="col-sm-6">
                <div class="div">
                    <h4>Passenger List</h4>
                </div>
                <asp:Label runat="server" ID="lblPassengerNote">Kindly input the passenger detail correctly. <br />
                </asp:Label>
            </div>

            <div class="col-sm-6">
                <div style="display: inline-block; float: right">
                    <div class="row" id="passDetailFilter" runat="server">
                        <div class="" id="divUploadPanel" runat="server">
                            <%--//ammended by Ellis 20170306, removing dropdown list file type
                              <div class="filterInlineBlock">
                              <table>
                                <tr>
                                    <td style="padding-right: 10px;">
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Select File:" AssociatedControlID="uplImage">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox ID="cmbType" runat="server" SelectedIndex="0" Width="71" AutoPostBack="false" Visible="true">
                                            <Items>
                                                <dx:ListEditItem Selected="True" Text="xls" Value="xls" />
                                                <dx:ListEditItem Text="csv" Value="csv" />
                                            </Items>
                                        </dx:ASPxComboBox>
                                    </td>
                                </tr>
                              </table>
                            </div>--%>
                            <div class="filterInlineBlock">
                                <table>
                                    <tr>
                                        <td>
                                            <dx:ASPxUploadControl CssClass="button_2" ID="uplImage" runat="server" ClientInstanceName="uploader" ShowProgressPanel="True"
                                                NullText="Click here to browse files..." Size="35" OnFileUploadComplete="uplImage_FileUploadComplete" Width="100%">
                                                <ClientSideEvents FileUploadComplete="function(s, e) { Uploader_OnFileUploadComplete(s, e); }"
                                                    FileUploadStart="function(s, e) { Uploader_OnUploadStart(); }" TextChanged="function(s, e) { UpdateUploadButton(); }"></ClientSideEvents>
                                                <ValidationSettings MaxFileSize="4194304" AllowedFileExtensions=".xls,.csv">
                                                </ValidationSettings>
                                            </dx:ASPxUploadControl>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%--//ammended by Ellis 20170306, removing dropdown list file type   
                                          <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Allowed file types: .xls , .csv" Font-Size="8pt">--%>
                                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Allowed file types: .xls" Font-Size="8pt">
                                            </dx:ASPxLabel>
                                            <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Maximum file size: 4Mb" Font-Size="8pt">
                                            </dx:ASPxLabel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>

                        <div class="col-sm-6" id="divTotalAmount" runat="server" style="display: none">
                            <dx:ASPxCallbackPanel ID="ActionPanel" runat="server" RenderMode="Div" ClientVisible="true" ClientInstanceName="ActionPanel"
                                OnCallback="ActionPanel_PerformCallback" SettingsLoadingPanel-Enabled="false" SettingsLoadingPanel-ShowImage="false" Width="100%">
                                <ClientSideEvents BeginCallback="function(s,e){LoadingPanel.Show();}" EndCallback="function(s, e){ ActionPanelEndCallBack();}"></ClientSideEvents>
                                <PanelCollection>
                                    <dx:PanelContent>
                                        <table id="totalEstimatedFare" class="enhanceFontSize titleAddOn" width="100%" style="padding: 5px;">
                                            <tr class="totalFare">
                                                <td>
                                                    <h4>Total Name Changes Amount</h4>
                                                </td>
                                                <td style="text-align: right">
                                                    <%--<h4>0.00</h4>--%>
                                                    <dx:ASPxLabel ID="lblTotalAmount" runat="server" Text='0.00' ClientInstanceName="lblTotalAmount" />
                                                    <dx:ASPxLabel ID="lblCurrency" runat="server" Text='' ClientInstanceName="lblCurrency" />
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxCallbackPanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <dx:ASPxCallbackPanel ID="ActionDataPanel" runat="server" RenderMode="Div" ClientVisible="false" Height="50%" ClientInstanceName="ActionDataPanel"
            OnCallback="ActionDataPanel_PerformCallback" SettingsLoadingPanel-Enabled="false">
            <ClientSideEvents EndCallback="OnEndCallBackEctionDataPanel" />
            <PanelCollection>
                <dx:PanelContent>
                    <asp:HiddenField ID="hfMessage" runat="server" Value="" />
                    <ul class="nav nav-pills" id="myTab">
                        <li id="Tabinsertdata" runat="server">
                            <label id="lblData" runat="server">
                            </label>
                            <span class="badge">
                                <dx:ASPxLabel ID="countData" runat="server"
                                    Font-Size="Small" />
                            </span></li>
                        <li style="right: 0;">
                            <div>
                                <table>
                                    <tr>
                                        <td style="padding-left: 20px;">
                                            <dx:ASPxButton CssClass="buttonL2 noBgImg" ID="btnDownloadTemplate" runat="server" Text="Download Incorrect Passenger List" AutoPostBack="false" ClientInstanceName="btnDownloadTemplate">
                                                <ClientSideEvents Click="OnDownloadclicked" />
                                            </dx:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                    </ul>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxCallbackPanel>
        <div class="fullWidthWrapper">
        <dx:ASPxGridView ID="gvPassenger" ClientInstanceName="gvPassenger" runat="server" KeyFieldName="PNR;PassengerID" AutoGenerateColumns="False" SettingsBehavior-ColumnResizeMode="Control"
            OnCellEditorInitialize="gvPassenger_CellEditorInitialize" OnBatchUpdate="gvPassenger_BatchUpdate" OnHtmlDataCellPrepared="gvPassenger_HtmlDataCellPrepared"
            OnCustomCallback="gvPassenger_CustomCallback" SettingsLoadingPanel-ShowImage="false">
            <SettingsEditing BatchEditSettings-EditMode="Row"></SettingsEditing>
            <Columns>

                <%-- 20170526 - Sienny (show row/line number) --%>
                <%--<dx:GridViewDataTextColumn Caption="No" Name="colRowNo" VisibleIndex="0" Width="20px" ReadOnly="true">
                    <DataItemTemplate>
                        <dx:ASPxLabel ID="lblRowNo" runat="server" Text='<%# Container.ItemIndex + 1 %>'>
                        </dx:ASPxLabel>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>--%>
                <dx:GridViewDataColumn FieldName="RowNo" Caption="No" ReadOnly="true" Width="20px" VisibleIndex="0" />

                <%-- <dx:GridViewDataColumn Name="Details" Caption="Details" VisibleIndex="0" Width="50px">
                    <DataItemTemplate>
                        <a href="javascript:void(0);" onclick="OnEdit(this, '<%# Container.KeyValue %>')">
                            Edit</a>
                    </DataItemTemplate>
                </dx:GridViewDataColumn>--%>
                <%-- <dx:GridViewDataColumn FieldName="Nationality" VisibleIndex="0" GroupIndex="0" />                                                                 
                    <dx:GridViewDataColumn FieldName="RecordLocator" Caption="Booking No." 
                        VisibleIndex="1" Width="75px" /> --%>
                <dx:GridViewDataColumn FieldName="PNR" VisibleIndex="0" GroupIndex="0" />
                <dx:GridViewDataColumn FieldName="PassengerID" Caption="PassengerID" Visible="false" VisibleIndex="0" Width="110px" />
                <dx:GridViewDataColumn FieldName="CountChanged" Caption="CountChanged" VisibleIndex="0" Width="0px" EditFormSettings-Visible="False" />
                <%--<dx:GridViewDataColumn FieldName="IssuingCountryName" Caption="Issuing Country" VisibleIndex="0" Width="110px" />--%>
                <dx:GridViewDataComboBoxColumn FieldName="IssuingCountry" Caption="Issuing Country" VisibleIndex="0" Width="110px">
                    <PropertiesComboBox DataSourceID="CountrySource" ValueType="System.String" ValueField="CountryCode" TextField="Name">
                        <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                    </PropertiesComboBox>
                </dx:GridViewDataComboBoxColumn>
                <%--<dx:GridViewDataColumn FieldName="countryName" Caption="Nationality" VisibleIndex="1" Width="110px" />--%>
                <dx:GridViewDataComboBoxColumn FieldName="Nationality" Caption="Nationality" VisibleIndex="1" Width="110px">
                    <PropertiesComboBox DataSourceID="CountrySource" ValueType="System.String" ValueField="CountryCode" TextField="Name">
                        <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                    </PropertiesComboBox>
                </dx:GridViewDataComboBoxColumn>
                <%--<dx:GridViewDataColumn FieldName="Title" Caption="Title" VisibleIndex="4" Width="35px" />--%>
                <dx:GridViewDataComboBoxColumn FieldName="Title" Caption="Title" VisibleIndex="4" Width="75px">
                    <PropertiesComboBox ValueType="System.String">
                        <Items>
                            <dx:ListEditItem Text="CHILD" Value="CHD" />
                            <dx:ListEditItem Text="MR" Value="MR" />
                            <dx:ListEditItem Text="MRS" Value="MRS" />
                            <dx:ListEditItem Text="MS" Value="MS" />
                        </Items>
                        <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                    </PropertiesComboBox>
                </dx:GridViewDataComboBoxColumn>
                <%--<dx:GridViewDataColumn FieldName="Gender" Caption="Gender" VisibleIndex="5" Width="60px" />--%>
                <dx:GridViewDataComboBoxColumn FieldName="Gender" Caption="Gender" VisibleIndex="5" Width="90px">
                    <PropertiesComboBox ValueType="System.String">
                        <Items>
                            <dx:ListEditItem Text="MALE" Value="MALE" />
                            <dx:ListEditItem Text="FEMALE" Value="FEMALE" />
                        </Items>
                        <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                    </PropertiesComboBox>
                </dx:GridViewDataComboBoxColumn>
                <dx:GridViewDataTextColumn FieldName="FirstName" Caption="First Name" VisibleIndex="6" Width="110px">
                    <PropertiesTextEdit>
                        <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                    </PropertiesTextEdit>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="LastName" Caption="Last Name" VisibleIndex="7" Width="110px">
                    <PropertiesTextEdit>
                        <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                    </PropertiesTextEdit>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn FieldName="DOB" Caption="DOB" VisibleIndex="8" Width="90px">
                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy" EditFormatString="dd MMM yyyy">
                        <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                    </PropertiesDateEdit>
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataTextColumn FieldName="PassportNo" Caption="Passport No." VisibleIndex="9" Width="90px">
                    <PropertiesTextEdit>
                        <%--<ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />--%>
                    </PropertiesTextEdit>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn FieldName="ExpiryDate" Caption="Expiry Date" VisibleIndex="10" Width="90px">
                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy" EditFormatString="dd MMM yyyy">
                        <%--<ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />--%>
                    </PropertiesDateEdit>
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataColumn FieldName="ChangeCnt" Caption="Changes" VisibleIndex="10" Width="100px" EditFormSettings-Visible="False">
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="DepartInsure" Caption="Insurance" VisibleIndex="10" Width="100px" EditFormSettings-Visible="False">
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="DepartSeat" Caption="DepartSeat" VisibleIndex="11" Width="80px" EditFormSettings-Visible="False" />
                <dx:GridViewDataColumn FieldName="DepartConnectingSeat" Caption="ReturnConnectingSeat" VisibleIndex="11" Width="80px" EditFormSettings-Visible="False" />

                <dx:GridViewDataColumn FieldName="ReturnConnectingSeat" Caption="ReturnConnectingSeat" VisibleIndex="12" Width="80px" EditFormSettings-Visible="False" />
                <dx:GridViewDataColumn FieldName="ReturnSeat" Caption="ReturnSeat" VisibleIndex="12" Width="80px" EditFormSettings-Visible="False" />

                <dx:GridViewDataColumn FieldName="DepartMeal" Caption="Meal" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="DepartBaggage" Caption="Baggage" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="DepartSportEquipment" Caption="Sport Equipment" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="DepartComfortKit" Caption="Comfort Kit" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="DepartDutyFree" Caption="Duty Free" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />

                <dx:GridViewDataColumn FieldName="ReturnMeal" Caption="Meal" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="ReturnBaggage" Caption="Baggage" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="ReturnSportEquipment" Caption="Sport Equipment" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="ReturnComfortKit" Caption="Comfort Kit" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="ReturnDutyFree" Caption="Duty Free" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />

                <dx:GridViewDataColumn FieldName="ConDepartMeal" Caption="Meal" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="ConDepartBaggage" Caption="Baggage" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="ConDepartSportEquipment" Caption="Sport Equipment" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="ConDepartComfortKit" Caption="Comfort Kit" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="ConDepartDutyFree" Caption="Duty Free" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />

                <dx:GridViewDataColumn FieldName="ConReturnMeal" Caption="Meal" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="ConReturnBaggage" Caption="Baggage" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="ConReturnSportEquipment" Caption="Sport Equipment" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="ConReturnComfortKit" Caption="Comfort Kit" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="ConReturnDutyFree" Caption="Duty Free" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />

                <dx:GridViewDataColumn FieldName="ErrorMsg" Caption="Error Description" VisibleIndex="13" Width="80px" EditFormSettings-Visible="False" Visible="false" />

            </Columns>
            <ClientSideEvents BatchEditRowValidating="OnBatchEditRowValidating" EndCallback="onBatchEditEndEditing" BatchEditEndEditing="BatchEditEndEditing" BeginCallback="onBatchEditBeginCallback" />

            <%--  <Templates>
                <StatusBar>
                    <div style="text-align: right">
                        <%--<dx:ASPxHyperLink ID="hlSave" runat="server" Text="Save changes">
                                <ClientSideEvents Click="onEditGrid" />
                            </dx:ASPxHyperLink>
                        <dx:ASPxHyperLink ID="hlCancel" runat="server" Text="Cancel changes">
                            <ClientSideEvents Click="onCancelGrid" />
                        </dx:ASPxHyperLink>
                    </div>
                </StatusBar>
            </Templates>--%>
            <%--<Settings HorizontalScrollBarMode="Auto" ShowGroupFooter="VisibleIfExpanded" />--%>
            <SettingsEditing Mode="Batch" />

            <SettingsPager PageSize="50" Mode="ShowAllRecords">
            </SettingsPager>
            <SettingsDetail ExportMode="All" />
            <GroupSummary>
                <dx:ASPxSummaryItem FieldName="LineTotal" SummaryType="Count" DisplayFormat="{0}" ShowInGroupFooterColumn="PNR" />
            </GroupSummary>
            <SettingsBehavior AutoExpandAllGroups="True" ColumnResizeMode="Control"></SettingsBehavior>

            <Settings ShowGroupFooter="VisibleIfExpanded"></Settings>
            <SettingsLoadingPanel ShowImage="false" />
            <SettingsLoadingPanel Mode="Disabled" />
            <%--20170324 - Sienny (full page loading added) --%>
            <SettingsDetail ExportMode="All"></SettingsDetail>
            <Styles>
                <Header BackColor="#333333" ForeColor="White"></Header>
            </Styles>
        </dx:ASPxGridView>
        </div>

        <!-- Added by ketee, for Name change used -->
        <%--<dx:ASPxGridView ID="gvPassengerNameChange" ClientInstanceName="gvPassengerNameChange" runat="server" KeyFieldName="PNR;PassengerID"
            Width="100%" AutoGenerateColumns="False" SettingsBehavior-ColumnResizeMode="Control"
            OnBatchUpdate="gvPassenger_BatchUpdate" OnHtmlDataCellPrepared="gvPassenger_HtmlDataCellPrepared"
            OnCustomCallback="gvPassenger_CustomCallback" Visible="false" SettingsLoadingPanel-ShowImage="false">
            <SettingsEditing BatchEditSettings-EditMode="Row"></SettingsEditing>
            <Columns>
                <dx:GridViewDataColumn FieldName="PNR" VisibleIndex="0" GroupIndex="0" />
                <dx:GridViewDataColumn FieldName="PassengerID" Caption="PassengerID" Visible="false" VisibleIndex="0" Width="110px" />
                <dx:GridViewDataColumn FieldName="CountChanged" Caption="CountChanged" VisibleIndex="0" Width="0px" EditFormSettings-Visible="False" />
                <dx:GridViewDataComboBoxColumn FieldName="IssuingCountry" Caption="Issuing Country" VisibleIndex="0" Width="110px" EditFormSettings-Visible="False">
                    <PropertiesComboBox DataSourceID="CountrySource" ValueType="System.String" ValueField="CountryCode" TextField="Name">
                        <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                    </PropertiesComboBox>
                </dx:GridViewDataComboBoxColumn>
                <dx:GridViewDataComboBoxColumn FieldName="Nationality" Caption="Nationality" VisibleIndex="1" Width="110px" EditFormSettings-Visible="False">
                    <PropertiesComboBox DataSourceID="CountrySource" ValueType="System.String" ValueField="CountryCode" TextField="Name">
                        <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                    </PropertiesComboBox>
                </dx:GridViewDataComboBoxColumn>
                <dx:GridViewDataComboBoxColumn FieldName="Title" Caption="Title" VisibleIndex="4" Width="75px" EditFormSettings-Visible="False">
                    <PropertiesComboBox ValueType="System.String">
                        <Items>
                            <dx:ListEditItem Text="Child" Value="Chd" />
                            <dx:ListEditItem Text="Mr" Value="Mr" />
                            <dx:ListEditItem Text="Ms" Value="Ms" />
                        </Items>
                        <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                    </PropertiesComboBox>
                </dx:GridViewDataComboBoxColumn>
                <dx:GridViewDataComboBoxColumn FieldName="Gender" Caption="Gender" VisibleIndex="5" Width="90px" EditFormSettings-Visible="False">
                    <PropertiesComboBox ValueType="System.String">
                        <Items>
                            <dx:ListEditItem Text="Male" Value="Male" />
                            <dx:ListEditItem Text="Female" Value="Female" />
                        </Items>
                        <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                    </PropertiesComboBox>
                </dx:GridViewDataComboBoxColumn>
                <dx:GridViewDataTextColumn FieldName="FirstName" Caption="First Name" VisibleIndex="6" Width="110px">
                    <PropertiesTextEdit>
                        <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                    </PropertiesTextEdit>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="LastName" Caption="Last Name" VisibleIndex="7" Width="110px">
                    <PropertiesTextEdit>
                        <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                    </PropertiesTextEdit>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn FieldName="DOB" Caption="DOB" VisibleIndex="8" Width="90px" EditFormSettings-Visible="False">
                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy" EditFormatString="dd MMM yyyy">
                        <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                    </PropertiesDateEdit>
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataTextColumn FieldName="PassportNo" Caption="Passport No." VisibleIndex="9" Width="90px" EditFormSettings-Visible="False">
                    <PropertiesTextEdit>
                        <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                    </PropertiesTextEdit>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn FieldName="ExpiryDate" Caption="Expiry Date" VisibleIndex="10" Width="90px" EditFormSettings-Visible="False">
                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy" EditFormatString="dd MMM yyyy">
                        <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                    </PropertiesDateEdit>
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataColumn FieldName="ChangeCnt" Caption="Changes" VisibleIndex="10" Width="100px" EditFormSettings-Visible="False">
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="DepartSeat" Caption="DepartSeat" VisibleIndex="11" Width="80px" EditFormSettings-Visible="False" />
                <dx:GridViewDataColumn FieldName="DepartConnectingSeat" Caption="ReturnConnectingSeat" VisibleIndex="11" Width="80px" EditFormSettings-Visible="False" />

                <dx:GridViewDataColumn FieldName="ReturnConnectingSeat" Caption="ReturnConnectingSeat" VisibleIndex="12" Width="80px" EditFormSettings-Visible="False" />
                <dx:GridViewDataColumn FieldName="ReturnSeat" Caption="ReturnSeat" VisibleIndex="12" Width="80px" EditFormSettings-Visible="False" />

                <dx:GridViewDataColumn FieldName="DepartMeal" Caption="Meal" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="DepartBaggage" Caption="Baggage" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="DepartSportEquipment" Caption="Sport Equipment" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="DepartComfortKit" Caption="Comfort Kit" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="DepartDutyFree" Caption="Duty Free" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />

                <dx:GridViewDataColumn FieldName="ReturnMeal" Caption="Meal" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="ReturnBaggage" Caption="Baggage" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="ReturnSportEquipment" Caption="Sport Equipment" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="ReturnComfortKit" Caption="Comfort Kit" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="ReturnDutyFree" Caption="Duty Free" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />

                <dx:GridViewDataColumn FieldName="ConDepartMeal" Caption="Meal" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="ConDepartBaggage" Caption="Baggage" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="ConDepartSportEquipment" Caption="Sport Equipment" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="ConDepartComfortKit" Caption="Comfort Kit" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="ConDepartDutyFree" Caption="Duty Free" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />

                <dx:GridViewDataColumn FieldName="ConReturnMeal" Caption="Meal" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="ConReturnBaggage" Caption="Baggage" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="ConReturnSportEquipment" Caption="Sport Equipment" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="ConReturnComfortKit" Caption="Comfort Kit" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />
                <dx:GridViewDataColumn FieldName="ConReturnDutyFree" Caption="Duty Free" VisibleIndex="12" Width="0px" EditFormSettings-Visible="False" Visible="false" />

            </Columns>
            <ClientSideEvents BatchEditRowValidating="OnBatchEditRowValidating" BatchEditStartEditing="OnBatchStartEdit" EndCallback="onBatchEditEndEditing" />

            <Templates>
                <StatusBar>
                    <div style="text-align: right">
                        <dx:ASPxHyperLink ID="hlCancel" runat="server" Text="Cancel changes">
                            <ClientSideEvents Click="onCancelGrid" />
                        </dx:ASPxHyperLink>
                    </div>
                </StatusBar>
            </Templates>
            <SettingsEditing Mode="Batch" />

            <SettingsPager PageSize="50" Mode="ShowAllRecords">
            </SettingsPager>
            <SettingsDetail ExportMode="All" />
            <Settings ShowGroupFooter="VisibleIfExpanded" />
            <GroupSummary>
                <dx:ASPxSummaryItem FieldName="LineTotal" SummaryType="Count" DisplayFormat="{0}" ShowInGroupFooterColumn="PNR" />
            </GroupSummary>
            <SettingsBehavior AutoExpandAllGroups="True" ColumnResizeMode="Control"></SettingsBehavior>

            <Settings ShowGroupFooter="VisibleIfExpanded"></Settings>

            <SettingsDetail ExportMode="All"></SettingsDetail>
            <Styles>
                <Header BackColor="#333333" ForeColor="White">
                </Header>
            </Styles>
        </dx:ASPxGridView>--%>

        <div style="margin-top: 20px;" id="divInfant" runat="server">
            <asp:Label runat="server" ID="Label12">Kindly input the Infant detail correctly. Infant detail must be entered manually in below table.
                <br />
            </asp:Label>
        </div>
        <!-- Added by Tyas, Add Infant -->
        <dx:ASPxGridView ID="gvInfant" ClientInstanceName="gvInfant" runat="server" KeyFieldName="RecordLocator;PassengerID" Width="1051" AutoGenerateColumns="False" SettingsBehavior-ColumnResizeMode="Control"
            OnBatchUpdate="gvInfant_BatchUpdate" OnHtmlDataCellPrepared="gvInfant_HtmlDataCellPrepared" OnCustomCallback="gvInfant_CustomCallback">
            <SettingsEditing BatchEditSettings-EditMode="Row"></SettingsEditing>
            <Columns>
                <dx:GridViewDataColumn FieldName="RecordLocator" Caption="PNR" VisibleIndex="0" GroupIndex="0" />
                <dx:GridViewDataColumn FieldName="PaxNo" Caption="Pax No" ReadOnly="true" Width="50px" VisibleIndex="0" />
                <dx:GridViewDataColumn FieldName="PassengerID" Caption="PassengerID" Visible="false" VisibleIndex="0" Width="100px" />
                <dx:GridViewDataComboBoxColumn FieldName="IssuingCountry" Caption="Issuing Country" VisibleIndex="0" Width="100px">
                    <PropertiesComboBox DataSourceID="CountrySource" ValueType="System.String" ValueField="CountryCode" TextField="Name">
                        <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                    </PropertiesComboBox>
                </dx:GridViewDataComboBoxColumn>
                <dx:GridViewDataComboBoxColumn FieldName="Nationality" Caption="Nationality" VisibleIndex="1" Width="100px">
                    <PropertiesComboBox DataSourceID="CountrySource" ValueType="System.String" ValueField="CountryCode" TextField="Name">
                        <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                    </PropertiesComboBox>
                </dx:GridViewDataComboBoxColumn>
                <dx:GridViewDataComboBoxColumn FieldName="Gender" Caption="Gender" VisibleIndex="5" Width="90px">
                    <PropertiesComboBox ValueType="System.String">
                        <Items>
                            <dx:ListEditItem Text="MALE" Value="MALE" />
                            <dx:ListEditItem Text="FEMALE" Value="FEMALE" />
                        </Items>
                    </PropertiesComboBox>
                </dx:GridViewDataComboBoxColumn>
                <dx:GridViewDataColumn FieldName="ParentFirstName" Caption="Parent First Name" VisibleIndex="6" Width="110px" />
                <dx:GridViewDataColumn FieldName="ParentLastName" Caption="Parent Last Name" VisibleIndex="7" Width="110px" />
                <dx:GridViewDataTextColumn FieldName="FirstName" Caption="First Name" VisibleIndex="8" Width="100px">
                    <PropertiesTextEdit>
                    </PropertiesTextEdit>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="LastName" Caption="Last Name" VisibleIndex="9" Width="100px">
                    <PropertiesTextEdit>
                    </PropertiesTextEdit>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn FieldName="DOB" Caption="DOB" VisibleIndex="10" Width="90px">
                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy" EditFormatString="dd MMM yyyy">
                    </PropertiesDateEdit>
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataTextColumn FieldName="PassportNo" Caption="Passport No." VisibleIndex="11" Width="80px">
                    <PropertiesTextEdit>
                        <ValidationSettings Display="Dynamic" />
                    </PropertiesTextEdit>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn FieldName="ExpiryDate" Caption="Expiry Date" VisibleIndex="12" Width="80px">
                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy" EditFormatString="dd MMM yyyy">
                        <%--<ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />--%>
                    </PropertiesDateEdit>
                </dx:GridViewDataDateColumn>
            </Columns>
            <ClientSideEvents BatchEditRowValidating="OnBatchEditRowValidatingInfant" RowClick="OnBatchStartEditInfant" EndCallback="onBatchEditEndEditingInfant" />

            <Templates>
                <StatusBar>
                    <div style="text-align: right">
                        <%--<dx:ASPxHyperLink ID="hlSave" runat="server" Text="Save changes">
                                <ClientSideEvents Click="onEditGrid" />
                            </dx:ASPxHyperLink>--%>
                        <dx:ASPxHyperLink ID="hlCancel" runat="server" Text="Cancel changes">
                            <ClientSideEvents Click="onCancelGrid" />
                        </dx:ASPxHyperLink>
                    </div>
                </StatusBar>
            </Templates>
            <SettingsEditing Mode="Batch" />

            <SettingsPager PageSize="50" Mode="ShowAllRecords">
            </SettingsPager>
            <SettingsDetail ExportMode="All" />
            <Settings ShowGroupFooter="VisibleIfExpanded" />
            <GroupSummary>
                <dx:ASPxSummaryItem FieldName="LineTotal" SummaryType="Count" DisplayFormat="{0}" ShowInGroupFooterColumn="PNR" />
            </GroupSummary>
            <SettingsBehavior AutoExpandAllGroups="True" ColumnResizeMode="Control"></SettingsBehavior>

            <Settings ShowGroupFooter="VisibleIfExpanded"></Settings>

            <SettingsDetail ExportMode="All"></SettingsDetail>
            <Styles>
                <Header BackColor="#333333" ForeColor="White">
                </Header>
            </Styles>

        </dx:ASPxGridView>

        <asp:SqlDataSource DataSourceMode="DataReader" ID="CountrySource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnStr%>"
            SelectCommand="SELECT CountryCode, Name FROM COUNTRY WHERE InActive=0"></asp:SqlDataSource>
        <strong><em style="font-size: 11px;">Note. Passport expiry date should be at least 6(six) months from departure date.</em></strong>
        <asp:Label runat="server" ID="imgError" ForeColor="Red" Visible="false"><img src='../images/AKBase/icon-error.gif' width='15px'/></asp:Label>
        <asp:Label runat="server" ID="imgSuccessMsg" ForeColor="Red" Visible="false"><img src='../images/AKBase/icon-success.png' width='15px'/></asp:Label>
        <asp:Literal ID="lblErrorPass" runat="server" Visible="false"></asp:Literal>
        <dx:ASPxLabel ID="lblMsg" ClientInstanceName="lblMsg" runat="server" Visible="false"></dx:ASPxLabel>
        <asp:HiddenField ID="hCommand" runat="server" Value="" />
        <dx:ASPxGridViewExporter ID="exporter" runat="server" GridViewID="gvPassenger" />
        <dx:ASPxPopupControl ID="popup" ClientInstanceName="popup"
            runat="server" Modal="true" AllowDragging="true" OnCustomCallback="gvPassenger_CustomCallback"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="true"
            CloseButtonImage-Url="~/Images/AirAsia/close_button_icon.jpg" HeaderText="Passenger Profile" CloseAction="CloseButton">

            <ClientSideEvents Shown="popup_Shown"></ClientSideEvents>
            <CloseButtonImage Url="~/Images/AirAsia/close_button_icon.jpg">
            </CloseButtonImage>

            <ClientSideEvents Shown="popup_Shown" CloseButtonClick="function OnCloseButtonClick(s, e) {
                var parentWindow = window.parent; parentWindow.popup.Hide(); gvPassenger.Refresh();}"></ClientSideEvents>
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                    <dx:ASPxCallbackPanel ID="callbackPanel" ClientInstanceName="callbackPanel" runat="server"
                        Width="566px" Height="400px" OnCallback="callbackPanel_Callback" RenderMode="Table">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <dx:ASPxLoadingPanel runat="server" ID="LoadingPanelPop" ClientInstanceName="LoadingPanelPop" Modal="True">
                                </dx:ASPxLoadingPanel>
                                <asp:Label runat="server" ID="imgSuccess" ForeColor="Red" Visible="false"><img src='../images/AKBase/icon-success.png' width='15px'/></asp:Label>
                                <font color="blue"><asp:Literal ID="litText" runat="server" Text=""></asp:Literal></font>
                                <br />
                                <table>
                                    <tr>
                                        <td style="width: 196px; padding: 0px 4px 4px 0px;"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="right">
                                            <b>
                                                <asp:Literal ID="ltrNumber" runat="server" Text=""></asp:Literal>
                                            </b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" class="tdcol" style="padding: 0px 4px 4px 0px;">
                                            <strong>Personal Details</strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td1" style="width: 196px; padding: 0px 4px 4px 0px;">
                                            <font color="red">*</font>Title 
                                        </td>
                                        <td class="td2" style="padding: 0px 4px 4px 0px;">
                                            <dx:ASPxComboBox ID="cmbTitle" ClientInstanceName="cmbTitle" runat="server" class="Select" AutoPostBack="false" IncrementalFilteringMode="StartsWith" Width="100">
                                                <ClientSideEvents SelectedIndexChanged="function(s,e){changeGenderProperty()}"></ClientSideEvents>
                                                <Items>
                                                    <dx:ListEditItem Text="CHILD" Value="CHD" />
                                                    <dx:ListEditItem Text="MR" Value="MR" />
                                                    <dx:ListEditItem Text="MRS" Value="MRS" />
                                                    <dx:ListEditItem Text="MS" Value="MS" />
                                                </Items>
                                                <ClientSideEvents SelectedIndexChanged="function(s,e){changeGenderProperty()}" />
                                                <ValidationSettings>
                                                    <RequiredField ErrorText="Title is required" IsRequired="True" />
                                                    <RequiredField IsRequired="True" ErrorText="Title is required"></RequiredField>
                                                </ValidationSettings>
                                            </dx:ASPxComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td1" style="width: 196px; padding: 0px 4px 4px 0px;">
                                            <font color="red">*</font>Gender 
                                        </td>
                                        <td class="td2" style="padding: 0px 4px 4px 0px;">
                                            <dx:ASPxComboBox ID="cmbGender" ClientInstanceName="cmbGender" runat="server" class="Select" AutoPostBack="false" IncrementalFilteringMode="StartsWith" Width="100">
                                                <ClientSideEvents SelectedIndexChanged="function(s,e){changeTitleProperty()}"></ClientSideEvents>
                                                <Items>
                                                    <dx:ListEditItem Text="MALE" Value="MALE" />
                                                    <dx:ListEditItem Text="FEMALE" Value="FEMALE" />
                                                </Items>
                                                <ClientSideEvents SelectedIndexChanged="function(s,e){changeTitleProperty()}" />
                                                <ValidationSettings>
                                                    <RequiredField ErrorText="Gender is required" IsRequired="True" />
                                                    <RequiredField IsRequired="True" ErrorText="Gender is required"></RequiredField>
                                                </ValidationSettings>
                                            </dx:ASPxComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td1" style="width: 196px; padding: 0px 4px 4px 0px;">
                                            <font color="red">*</font>Given Name (as in passport)   
                                        </td>
                                        <td class="td2" style="padding: 0px 4px 4px 0px;">
                                            <asp:HiddenField ID="txt_PrevFirstName" runat="server" />
                                            <asp:HiddenField ID="txt_PrevLastName" runat="server" />
                                            <asp:HiddenField ID="txt_PrevFirstName1" runat="server" />
                                            <asp:HiddenField ID="txt_PrevLastName1" runat="server" />
                                            <asp:HiddenField ID="txt_PrevFirstName2" runat="server" />
                                            <asp:HiddenField ID="txt_PrevLastName2" runat="server" />

                                            <dx:ASPxTextBox ID="txt_FirstName" runat="server" Width="200px"
                                                NullText="Given Name" MaxLength="50">
                                                <ValidationSettings>
                                                    <RequiredField ErrorText="Given Name is required" IsRequired="True" />
                                                    <RegularExpression ErrorText="Invalid Given Name" ValidationExpression="^([a-zA-Z\s]{1,50})$" />

                                                    <RequiredField IsRequired="True" ErrorText="Given Name is required"></RequiredField>
                                                    <RegularExpression ErrorText="Invalid Given Name" ValidationExpression="^([a-zA-Z\s]{1,50})$"></RegularExpression>
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>

                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="td1" style="width: 196px; padding: 0px 4px 4px 0px;">
                                            <font color="red">*</font>Family Name (as is passport)</td>
                                        <td class="td2" style="padding: 0px 4px 4px 0px;">
                                            <dx:ASPxTextBox ID="txt_LastName" runat="server" Width="200px" NullText="Family Name" MaxLength="50">
                                                <ValidationSettings>
                                                    <RequiredField ErrorText="Family Name is required" IsRequired="True" />
                                                    <RegularExpression ErrorText="Invalid Last Name" ValidationExpression="^([a-zA-Z\s]{1,50})$" />

                                                    <RequiredField IsRequired="True" ErrorText="Last Name is required"></RequiredField>
                                                    <RegularExpression ErrorText="Invalid Last Name" ValidationExpression="^([a-zA-Z\s]{1,50})$"></RegularExpression>
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td1" style="width: 196px; padding: 0px 4px 4px 0px;">
                                            <font color="red">*</font>Nationality
                                        </td>
                                        <td class="td2" style="padding: 0px 4px 4px 0px;">
                                            <dx:ASPxComboBox ID="cmbNation" runat="server" AutoPostBack="False" class="Select" IncrementalFilteringMode="StartsWith" Width="200px">
                                                <ValidationSettings>
                                                    <RequiredField IsRequired="true" ErrorText="Nationality is required." />
                                                    <RequiredField IsRequired="True" ErrorText="Nationality is required."></RequiredField>
                                                </ValidationSettings>
                                            </dx:ASPxComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 196px; padding: 0px 4px 4px 0px;"><font color="red">*</font>Date of Birth</td>
                                        <td style="padding: 0px 4px 4px 0px;">
                                            <dx:ASPxDateEdit ClientInstanceName="txtAdt" ClientVisible="false" ID="txtAdt" runat="server" Width="150" EditFormat="Custom"
                                                UseMaskBehavior="false" DisplayFormatString="dd MMM yyyy" EditFormatString="dd MMM yyyy">
                                            </dx:ASPxDateEdit>
                                            <dx:ASPxDateEdit ClientInstanceName="txtChd" ClientVisible="false" ID="txtChd" runat="server" Width="150" EditFormat="Custom"
                                                UseMaskBehavior="false" DisplayFormatString="dd MMM yyyy" EditFormatString="dd MMM yyyy">
                                            </dx:ASPxDateEdit>
                                            <dx:ASPxDateEdit ClientInstanceName="txtDOB" ID="txtDOB" runat="server" Width="150" EditFormat="Custom"
                                                UseMaskBehavior="false" DisplayFormatString="dd MMM yyyy" EditFormatString="dd MMM yyyy" MinDate="1900-01-01"
                                                DropDownButton-Image-Url="../images/AKBase/calendar-up.gif">
                                                <DropDownButton>
                                                    <Image Url="../images/AKBase/calendar-up.gif"></Image>
                                                </DropDownButton>
                                            </dx:ASPxDateEdit>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td style="width: 196px; padding: 0px 4px 4px 0px;">&nbsp;</td>
                                        <td style="padding: 0px 4px 4px 0px;">&nbsp;</td>
                                    </tr>

                                    <tr>
                                        <td colspan="3" style="padding: 0px 4px 4px 0px;">
                                            <strong>Passport details</strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td1" style="width: 196px; padding: 0px 4px 4px 0px;">
                                            <font color="red">*</font>Identification/Passport number 
                                        </td>
                                        <td class="td2" style="padding: 0px 4px 4px 0px;">
                                            <dx:ASPxTextBox ID="txtPassportNo" runat="server" Width="200px" MaxLength="200">
                                                <ValidationSettings SetFocusOnError="true">
                                                    <RequiredField ErrorText="Passport Number is required" IsRequired="True" />
                                                    <RegularExpression ErrorText="Passport Number is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,200})$" />

                                                    <RequiredField IsRequired="True" ErrorText="Passport number is required"></RequiredField>
                                                    <RegularExpression ErrorText="Passport Number is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,200})$"></RegularExpression>
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 196px; padding: 0px 4px 4px 0px;" class="td1">
                                            <font color="red">*</font>Issuing Country

                                        </td>
                                        <td class="td2" style="padding: 0px 4px 4px 0px;">
                                            <dx:ASPxComboBox ID="cmbPassCountry" runat="server" class="Select"
                                                EnableIncrementalFiltering="True" IncrementalFilteringMode="StartsWith"
                                                Width="200px">
                                                <ValidationSettings>
                                                    <RequiredField ErrorText="Issuing Country is required." IsRequired="true" />
                                                    <RequiredField IsRequired="True" ErrorText="Issuing Country is required."></RequiredField>
                                                </ValidationSettings>
                                            </dx:ASPxComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 196px; padding: 0px 4px 4px 0px;"><font color="red">*</font>Expiration Date</td>
                                        <td style="padding: 0px 4px 4px 0px;">
                                            <dx:ASPxDateEdit ID="txtExpired" runat="server" Width="150" EditFormat="Custom"
                                                UseMaskBehavior="false" DisplayFormatString="dd MMM yyyy"
                                                EditFormatString="dd MMM yyyy" DropDownButton-Image-Url="../images/AKBase/calendar-up.gif">
                                                <DropDownButton>
                                                    <Image Url="../images/AKBase/calendar-up.gif"></Image>
                                                </DropDownButton>

                                                <ValidationSettings>
                                                    <RequiredField ErrorText="Expiration Date is required." IsRequired="true" />
                                                    <RequiredField IsRequired="True" ErrorText="Expiration Date is required."></RequiredField>
                                                </ValidationSettings>
                                            </dx:ASPxDateEdit>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 196px">&nbsp;</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            <dx:ASPxButton CssClass="button_2" ID="btPrev" runat="server"
                                                Text="Prev" AutoPostBack="False" OnClick="btPrev_Click" CausesValidation="false">
                                                <ClientSideEvents Click="function(s, e) { LoadingPanelPop.SetText(&#39;Please Wait...&#39;);
                                                    LoadingPanelPop.Show(); }" />
                                                <ClientSideEvents Click="function(s, e) { LoadingPanelPop.SetText(&#39;Please Wait...&#39;);
                                                    LoadingPanelPop.Show(); }"></ClientSideEvents>
                                            </dx:ASPxButton>
                                        </td>
                                        <td></td>
                                        <td>
                                            <dx:ASPxButton CssClass="button_2" ID="btNext" runat="server"
                                                Text="Next" AutoPostBack="False" OnClick="btNext_Click" CausesValidation="false">
                                                <ClientSideEvents Click="function(s, e) { LoadingPanelPop.SetText(&#39;Please Wait...&#39;);
                                                    LoadingPanelPop.Show(); }" />
                                                <ClientSideEvents Click="function(s, e) { LoadingPanelPop.SetText(&#39;Please Wait...&#39;);
                                                    LoadingPanelPop.Show(); }"></ClientSideEvents>
                                            </dx:ASPxButton>
                                        </td>
                                        <td></td>
                                        <td>
                                            <dx:ASPxButton CssClass="button_2" ID="btCancel" runat="server" Text="Close" AutoPostBack="False">
                                                <ClientSideEvents Click="function OnCloseButtonClick(s, e) {
                                                    var parentWindow = window.parent;
                                                    parentWindow.popup.Hide(); gvPassenger.Refresh();}"></ClientSideEvents>
                                            </dx:ASPxButton>
                                        </td>
                                        <td style="width: 110px"></td>
                                        <td>
                                            <dx:ASPxButton CssClass="button_2" ID="btSave" runat="server" Text="Save" AutoPostBack="False" OnClick="btSave_Click" CausesValidation="true">
                                                <ClientSideEvents Click="function(s, e) { 
                                                    if(ASPxClientEdit.AreEditorsValid()) {
                                                    LoadingPanelPop.SetText(&#39;Please Wait...&#39;);
                                                    LoadingPanelPop.Show(); }
                                                    }" />
                                                <ClientSideEvents Click="function(s, e) { if(ASPxClientEdit.AreEditorsValid()) {
                                                    LoadingPanelPop.SetText(&#39;Please Wait...&#39;);
                                                    LoadingPanelPop.Show(); }
                                                    }"></ClientSideEvents>
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
        <dx:ASPxGridView ID="gvExport" ClientInstanceName="gvExport" runat="server" KeyFieldName="PassengerID" Width="100%" AutoGenerateColumns="False" SettingsBehavior-ColumnResizeMode="Control"
            OnCustomCallback="gvPassenger_CustomCallback" Visible="false">
            <Columns>
                <dx:GridViewDataColumn FieldName="PNR" Caption="PNR" VisibleIndex="1" Width="75px" ReadOnly="true" />
                <dx:GridViewDataColumn FieldName="CountryName" Caption="Nationality" VisibleIndex="1" Width="75px" />
                <dx:GridViewDataColumn FieldName="IssuingCountryName" Caption="IssuingCountry" VisibleIndex="2" Width="75px" />
                <dx:GridViewDataColumn FieldName="Title" Caption="Title" VisibleIndex="4" Width="35px" />
                <dx:GridViewDataColumn FieldName="Gender" Caption="Gender" VisibleIndex="5" Width="50px" />
                <dx:GridViewDataColumn FieldName="FirstName" Caption="FirstName" VisibleIndex="6" Width="68px" />
                <dx:GridViewDataColumn FieldName="LastName" Caption="LastName" VisibleIndex="7" Width="68px" />
                <dx:GridViewDataColumn FieldName="DOB" Caption="DOB" VisibleIndex="8" Width="70px" />
                <dx:GridViewDataColumn FieldName="PassportNo" Caption="PassportNo" VisibleIndex="9" Width="68px" />
                <dx:GridViewDataColumn FieldName="ExpiryDate" Caption="ExpiryDate" VisibleIndex="10" Width="65px" />
                <dx:GridViewDataColumn FieldName="ErrorMsg" Caption="Error Description" VisibleIndex="11" Width="80px" Visible="false" />
            </Columns>

            <SettingsPager PageSize="50">
            </SettingsPager>
            <Settings ShowGroupFooter="VisibleIfExpanded" />
            <GroupSummary>
                <dx:ASPxSummaryItem FieldName="LineTotal" SummaryType="Count" DisplayFormat="{0}" ShowInGroupFooterColumn="PNR" />
            </GroupSummary>
            <SettingsBehavior ColumnResizeMode="Control"></SettingsBehavior>

            <Settings ShowGroupFooter="VisibleIfExpanded"></Settings>
            <Styles>
                <Header BackColor="#333333" ForeColor="White">
                </Header>
            </Styles>

        </dx:ASPxGridView>
    </div>
    <%--<input type="hidden" id="hfConfirm" runat="server" />--%>
    <asp:HiddenField ID="hfConfirm" runat="server" />
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <FBD:ucflightbookingdetail ID="ucflightbookingdetail" runat="server" />
    <PD:ucPaxDetail ID="ucPaxDetail" runat="server" />
</asp:Content>
