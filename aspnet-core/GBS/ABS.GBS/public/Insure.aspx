<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewPageMaster.Master" AutoEventWireup="true" CodeBehind="Insure.aspx.cs" Inherits="GroupBooking.Web.Insure" %>

<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %>
<%@ Register Src="../UserControl/flightbookingdetail.ascx" TagName="ucflightbookingdetail" TagPrefix="FBD" %>
<%@ Register Src="../UserControl/PaxDetail.ascx" TagName="ucPaxDetail" TagPrefix="PD" %>

<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v16.1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript" src="../Scripts/Booking/AddOn.js?ver=1.4"></script>

    <script type="text/javascript">
      
    </script>

    <msg:msgControl ID="msgcontrol" runat="server" />
    <div>
        <dx:ASPxCallback ID="CallBackValidation" ClientInstanceName="CallBackValidation" runat="server" OnCallback="ValidatePopup">
            <ClientSideEvents CallbackComplete="OnCallbackCompleteManage" />
        </dx:ASPxCallback>
    </div>
    <div class="row page-header clearfix">
        <div class="col-sm-3">
            <h4 class="mt-0 mb-5">Manage Insurance</h4>
            Booking/Manage Insurance
        </div>
        <div class="col-sm-5">
        </div>
        <div class="col-sm-4">
            <div style="float: right;">
                <dx:ASPxButton CssClass="buttonL" ID="btn_Next" runat="server"
                    Text="Confirm" AutoPostBack="False">
                    <ClientSideEvents Click="function(s, e) { 
                        if (ASPxClientEdit.ValidateGroup('mandatory')) {
                            if (gvPassenger.batchEditApi.HasChanges()) {
                            document.getElementById('ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg').innerHTML = 'Kindly save changes or cancel changes in grid depart details to continue';
                            pcMessage.Show();
                            LoadingPanel.Hide();
                                        return;
                                    }
                                        else if (typeof gvPassenger2 != 'undefined'){
                            if (gvPassenger2.batchEditApi.HasChanges()) {
                            document.getElementById('ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg').innerHTML = 'Kindly save changes or cancel changes in grid return details to continue';
                            pcMessage.Show();
                            LoadingPanel.Hide();
                                        return;}
                                        else{CallBackValidation.PerformCallback();
                                                LoadingPanel.SetText(&#39;Please Wait...&#39;);
                                               LoadingPanel.Show(); }
                                        }
                                    else{CallBackValidation.PerformCallback();
                                                LoadingPanel.SetText(&#39;Please Wait...&#39;);
                                               LoadingPanel.Show(); }                        
                        } else {
                            window.scrollTo(0, 1020);
                            LoadingPanel.Hide();
                        }}" />
                </dx:ASPxButton>
            </div>
        </div>
    </div>


    <div class="widget page-content container-fluid" style="border-color: transparent;">
        <dx:ASPxCallbackPanel ID="SSRActionPanel" runat="server" RenderMode="Div" ClientVisible="true" Height="50%" ClientInstanceName="SSRActionPanel"
            SettingsLoadingPanel-Enabled="false" OnCallback="SSRActionPanel_PerformCallback">
            <ClientSideEvents EndCallback="function(s, e){ SSRActionPanelEndCallBack();}"></ClientSideEvents>
            <PanelCollection>
                <dx:PanelContent>
                    <table id="totalEstimatedFare" class="enhanceFontSize titleAddOn" width="100%" style="padding: 5px;">
                        <tr class="totalFare">
                            <td>
                                <h4>Total Amount</h4>
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

        <dx:ASPxPopupControl ID="popup" ClientInstanceName="popup"
            runat="server" Modal="true" AllowDragging="true"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="true"
            CloseButtonImage-Url="~/Images/AirAsia/close_button_icon.jpg" HeaderText="Passenger Profile" CloseAction="CloseButton">

            <ClientSideEvents Shown="popup_Shown"></ClientSideEvents>
            <CloseButtonImage Url="~/Images/AirAsia/close_button_icon.jpg">
            </CloseButtonImage>

            <ClientSideEvents Shown="popup_Shown" CloseButtonClick="function OnCloseButtonClick(s, e) {
                var parentWindow = window.parent; parentWindow.popup.Hide(); gvPassenger.Refresh();}"></ClientSideEvents>
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                    <dx:ASPxCallbackPanel ID="callbackPanel" ClientInstanceName="callbackPanel" runat="server" Width="566px" Height="400px" 
                        RenderMode="Table" OnCallback="callbackPanel_Callback">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <dx:ASPxLoadingPanel runat="server" ID="LoadingPanelPop" ClientInstanceName="LoadingPanelPop" Modal="True">
                                </dx:ASPxLoadingPanel>

                                <br />
                                <table>
                                    <tr>
                                        <td style="width: 196px; padding: 0px 4px 4px 0px;"></td>
                                    </tr>
                                   <%-- <tr >
                                        <td colspan="3">
                                            <b>Passenger Name:
                                                <dx:ASPxLabel ID="lblPaxName" runat="server" ClientInstanceName="lblPaxName" />
                                            </b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Passenger ID:
                                                <dx:ASPxLabel ID="lblPaxID" runat="server" ClientInstanceName="lblPaxID" />
                                            </b>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td colspan="3" class="tdcol" style="padding: 0px 4px 4px 0px;">
                                            <strong>Infant Details</strong>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="td1" style="width: 196px; padding: 0px 4px 4px 0px;">
                                            <font color="red">*</font>Gender 
                                        </td>
                                        <td class="td2" style="padding: 0px 4px 4px 0px;">
                                            <dx:ASPxComboBox ID="cmbGender" ClientInstanceName="cmbGender" runat="server" class="Select" AutoPostBack="false" IncrementalFilteringMode="StartsWith" Width="100">

                                                <Items>
                                                    <dx:ListEditItem Text="Male" Value="Male" />
                                                    <dx:ListEditItem Text="Female" Value="Female" />
                                                </Items>

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
                                                <ClientSideEvents Validation="DOBValidation" />
<%--                                                <ValidationSettings>
                                                    <RequiredField ErrorText="DOB is required." IsRequired="true" />
                                                    <RequiredField IsRequired="True" ErrorText="DOB is required."></RequiredField>
                                                </ValidationSettings>--%>
                                            </dx:ASPxDateEdit>
                                        </td>
                                        <td style="position: absolute; left: 62%; margin-top: 1px; color: red;">
                                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" ClientInstanceName="lblDOBError" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td style="width: 196px; padding: 0px 4px 4px 0px;">&nbsp;</td>
                                        <td style="padding: 0px 4px 4px 0px;">&nbsp;</td>
                                    </tr>


                                    <% if (hfInternational.Value == "True")
                                       { %>
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
                                        <td style="width: 196px; padding: 0px 4px 4px 0px;"><font color="red">*</font>Expiration Date</td>
                                        <td style="padding: 0px 4px 4px 0px;">
                                            <dx:ASPxDateEdit ID="txtExpired" runat="server" Width="150" EditFormat="Custom"
                                                UseMaskBehavior="false" DisplayFormatString="dd MMM yyyy" ClientInstanceName="txtExpired"
                                                EditFormatString="dd MMM yyyy" DropDownButton-Image-Url="../images/AKBase/calendar-up.gif">
                                                <DropDownButton>
                                                    <Image Url="../images/AKBase/calendar-up.gif"></Image>
                                                </DropDownButton>
                                                <ClientSideEvents Validation="ExpiryDateValidation" />
                                                <%--<ValidationSettings>
                                                    <RequiredField ErrorText="Expiration Date is required." IsRequired="true" />
                                                    <RequiredField IsRequired="True" ErrorText="Expiration Date is required."></RequiredField>
                                                </ValidationSettings>--%>

                                            </dx:ASPxDateEdit>
                                        </td>
                                        <td style="position: absolute; left: 62%; margin-top: 1px; color: red;">
                                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" ClientInstanceName="lblExpiryDateError" />
                                        </td>
                                    </tr>
                                    <% } %>
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
                                            <dx:ASPxButton CssClass="button_2" ID="btCancel" runat="server" Text="Close" AutoPostBack="False">
                                                <ClientSideEvents Click="function OnCloseButtonClick(s, e) {
                                                    var parentWindow = window.parent;
                                                    parentWindow.popup.Hide();}"></ClientSideEvents>
                                            </dx:ASPxButton>
                                        </td>
                                        <td style="width: 110px"></td>
                                        <td>
                                            <dx:ASPxButton CssClass="button_2" ID="btSave" runat="server" Text="Save" ClientInstanceName="btSave"  AutoPostBack="False" CausesValidation="true">
                                                <ClientSideEvents Click="function OnCloseButtonClick(s, e) {
                                                    gvPassenger.PerformCallback('Infant');
                                                    var parentWindow = window.parent;
                                                    parentWindow.popup.Hide();}"></ClientSideEvents>
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

        <div id="tabCtrlWrapper">
                                <div id="home" class="tab-pane fade in active">
                                    <div class="tempCtr">

                                        <div class="row">
                                            <%--<div class="controllerWrapper col-sm-12 col-md-11"> --%>
                                            <div class="controllerWrapper col-sm-12">
                                                <dx:ASPxCallbackPanel ID="SSRTab1Panel" runat="server" RenderMode="Div" ClientVisible="true" Height="50%" ClientInstanceName="SSRTab1Panel"
                                                    SettingsLoadingPanel-Enabled="false" OnCallback="SSRTab1Panel_PerformCallback">
                                                    <%--<ClientSideEvents EndCallback="function(s, e){ SSRActionPanelEndCallBack();}"></ClientSideEvents>--%>
<%--                                                    <PanelCollection>
                                                        <dx:PanelContent>
                                                            <div class="buttonsWrapper col-sm-12 nav nav-pills">
                                                                <div ID="IcnInsure1" runat="server" class="col-sm-2">
                                                                    <a href="#" onclick="showcollapsex()">
                                                                        <div class="btnCtrl roundedCorner">
                                                                            <div class="iconBtn fa fa-heart-o"></div>
                                                                        </div>
                                                                        <div class="title">Insurance</div>
                                                                        <dx:ASPxLabel ID="lblTotalInsure" runat="server" Text='0.00' ClientInstanceName="lblTotalInsure" />
                                                                    </a>
                                                                </div>
                                                            </div>
                                                        </dx:PanelContent>
                                                    </PanelCollection>--%>
                                                </dx:ASPxCallbackPanel>
                                                <div class="contentWrapper col-sm-12">
                                                    <%--div class="tab-content">--%>
                                                    <div id="collapsex"> <%--style="visibility: hidden;">--%>
                                                        <div class="baggageAddonWrapper" style="padding-top: 10px;">

                                                            <div class="mealAll">

                                                                <div class="showDropMeal collapse in" style="margin-bottom: 10px;">
                                                                    <table>
                                                                        <tbody>
                                                                            <tr>
                                                                                <td style="padding-top: 10px;">
                                                                                    <div class="controlledCollapse collapse in">
                                                                                        <dx:ASPxComboBox ID="cmbInsure" ClientInstanceName="cmbInsure" runat="server" class="Select" AutoPostBack="false" IncrementalFilteringMode="StartsWith" Width="296px">
                                                                                            <ClientSideEvents CloseUp="function(s, e) {setTimeout(function() { seInsure.Focus(); }, 0)}" />
                                                                                        </dx:ASPxComboBox>
                                                                                    </div>
                                                                                </td>
                                                                                <td>
                                                                                    <table class="addBaggageInput" style="margin-top: 10px;">
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <input type="checkbox" runat="server" name="cbAllPaxInsure1" id="cbAllPaxInsure1" onchange="javascript:AllPaxInsure1();" value="1" />All Pax
                                                                                                </td>
                                                                                                <td style="padding-left: 10px;">
                                                                                                    <dx:ASPxSpinEdit ID="seInsure" ClientInstanceName="seInsure" runat="server" Number="1" NumberType="Integer" MaxValue="50" Width="50px">
                                                                                                    </dx:ASPxSpinEdit>
                                                                                                </td>
                                                                                                <td style="padding-left: 10px;">
                                                                                                    <button type="button" class="btn btn-danger" onclick="AddInsure()">Assign</button>
                                                                                                </td>
                                                                                                <td style="padding-left: 10px;">
                                                                                                    <button type="button" class="btn btn-danger" onclick="ClearInsure()">Clear All</button>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <%-- </div>--%>
                                                </div>
                                            </div>
                                        </div>


                                    </div>
                                    <br />
                                    <div class="div" style="margin-bottom: 20px;">
                                        <h4>Passenger List</h4>
                                    </div>

                                    <dx:ASPxGridView ID="gvPassenger" runat="server" KeyFieldName="RecordLocator;SeqNo;PassengerID" EnableRowsCache="false" EnableCallBacks="true"
                                        ClientInstanceName="gvPassenger" Width="100%" OnCustomCallback="gvPassenger_CustomCallback" OnDataBinding="gvPassenger_DataBinding"
                                        OnBatchUpdate="gvPassenger_BatchUpdate"
                                        OnCustomButtonInitialize="gvPassenger_CustomButtonInitialize"
                                        OnHtmlDataCellPrepared="gvPassenger_HtmlDataCellPrepared">
                                        <ClientSideEvents EndCallback="function(s, e) { gvPassenger_EndCallback(s, e);}" />

                                        <Columns>
                                            <dx:GridViewDataColumn FieldName="RecordLocator" Caption="PNR" VisibleIndex="0" GroupIndex="0" />
                                            <dx:GridViewDataColumn FieldName="SeqNo" Caption="SeqNo" ReadOnly="true" Width="70px" />
                                            <%--<dx:GridViewDataColumn FieldName="PNR" Caption="PNR" ReadOnly="true" Width="100px" />--%>
                                            <dx:GridViewDataComboBoxColumn FieldName="Title" Caption="Title" ReadOnly="true" VisibleIndex="4" Width="75px">
<%--                                                <PropertiesComboBox ValueType="System.String">
                                                    <Items>
                                                        <dx:ListEditItem Text="Chd" Value="Chd" />
                                                        <dx:ListEditItem Text="Mr" Value="Mr" />
                                                        <dx:ListEditItem Text="Ms" Value="Ms" />
                                                    </Items>
                                                    <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                                </PropertiesComboBox>--%>
                                            </dx:GridViewDataComboBoxColumn>
                                            <%--<dx:GridViewDataColumn FieldName="Gender" Caption="Gender" VisibleIndex="5" Width="60px" />--%>
                                            <dx:GridViewDataComboBoxColumn FieldName="Gender" Caption="Gender" ReadOnly="true" VisibleIndex="5" Width="90px">
<%--                                                <PropertiesComboBox ValueType="System.String">
                                                    <Items>
                                                        <dx:ListEditItem Text="Male" Value="Male" />
                                                        <dx:ListEditItem Text="Female" Value="Female" />
                                                    </Items>
                                                    <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                                </PropertiesComboBox>--%>
                                            </dx:GridViewDataComboBoxColumn>
                                            <dx:GridViewDataDateColumn FieldName="DOB" Caption="DOB" ReadOnly="true" VisibleIndex="6" Width="150px">
                                                <PropertiesDateEdit DisplayFormatString="dd MMM yyyy"> <%--EditFormatString="dd MMM yyyy">--%>
                                                    <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                                </PropertiesDateEdit>
                                            </dx:GridViewDataDateColumn>
                                            <dx:GridViewDataColumn FieldName="PassengerID" Caption="PassengerID" ReadOnly="true" Visible="false" VisibleIndex="0" Width="70px" />

                                            <dx:GridViewDataTextColumn FieldName="FirstName" Caption="First Name" ReadOnly="true" EditFormSettings-Visible="False" Width="200px" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="false">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="LastName" Caption="Last Name" ReadOnly="true" EditFormSettings-Visible="False" Width="200px" />
                                            <dx:GridViewDataTextColumn FieldName="PaxType" Caption="Pax Type" ReadOnly="true" EditFormSettings-Visible="False" Width="120px" />
                                            <dx:GridViewDataComboBoxColumn FieldName="InsureCode" Caption="Insurance" Width="180px" >
                                                
                                                <PropertiesComboBox TextField="Detail" ValueField="SSRCode" ValueType="System.String">
                                                    <%--<ClientSideEvents SelectedIndexChanged="function(s, e) { OnInsuranceChanged(s,e); }"></ClientSideEvents>--%>
                                                </PropertiesComboBox>
                                            </dx:GridViewDataComboBoxColumn>
                                            <dx:GridViewDataTextColumn FieldName="InsureFee1" Caption="Insurance Fee" ReadOnly="true" EditFormSettings-Visible="False" Width="100px" >
                                                <PropertiesTextEdit DisplayFormatString="n2"></PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="Status" Caption="Status" ReadOnly="true" EditFormSettings-Visible="False" Width="200px" />
                                            <dx:GridViewCommandColumn Name="Action" Caption="Action" Width="70px">
                                                <CustomButtons>
                                                    <dx:GridViewCommandColumnCustomButton ID="ClearInsure" runat="server" Text="Clear">
                                                    </dx:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                            </dx:GridViewCommandColumn>
<%--											<dx:GridViewDataTextColumn FieldName="InsureFee" Caption="Insurance Fee" ReadOnly="true" EditFormSettings-Visible="False" Width="200px" >
												<DataItemTemplate>
													<dx:ASPxLabel ID="lblInsureFee" ClientInstanceName="lblInsureFee" runat="server" Text='<%# ShowValue(Container.DataItem, "InsureFee")%>' CellStyle-VerticalAlign="Top" >
													</dx:ASPxLabel>
												</DataItemTemplate>
											</dx:GridViewDataTextColumn>--%>
                                            <%--<dx:GridViewCommandColumn>
                                                <CustomButtons>
                                                    <dx:GridViewCommandColumnCustomButton ID="btnDetails" Text="Details" />
                                                </CustomButtons>
                                            </dx:GridViewCommandColumn>--%>                                        
                                        </Columns>
                                        <ClientSideEvents BatchEditEndEditing="onBatchEditEndEditingManage" CustomButtonClick="OnCustomButtonClick" />
                                        <SettingsPager Mode="ShowPager" PageSize="11" Position="Top" />
                                        <SettingsEditing Mode="Batch" />
                                        <SettingsText CommandBatchEditUpdate="Save Changes and Continue" CommandBatchEditCancel="Cancel" />
                                        <SettingsBehavior AutoExpandAllGroups="true" />
                                    </dx:ASPxGridView>
                                </div>


            </br></br>
        <div id="divSAO" runat="server" style="background-color:#e3e5e5; border-bottom-width:1; padding: 5px 5px 5px 5px; text-align:justify;">
<%--            <span class="ti-check-box"> Flight Cancellation/delay benefit </span></br>
            <span class="ti-check-box"> Loss or Damage to Baggage and Personal Effects Benefits </span></br>
            <span class="ti-check-box"> 24- hour Worldwide Travel Assistance Services </span></br>
            <span class="ti-check-box"> Personal Accident benefit </span></br>
            <img src="https://static.tuneprotect.com/static/enmy.png" alt="" class="avatar mCS_img_loaded" data-pin-nopin="true">
            </br>
				<span style="display: block; ">I confirm that I have read, understood and agreed with the <a href="https://tuneprotect.com/airasia/my/ms/da/english.html" target="_blank">Declarations and Authorisations</a> of the Insurance Application and accept the <a href="https://tuneprotect.com/airasia/travel_protection/policy_wording.php" target="_blank">Terms &amp; Conditions</a> of the Policy. </span>
                <span style="display: block; ">I understand that if I do not wish to buy Travel Insurance, I can deselect the checkbox to remove it from my itinerary. I understand that if I change my flight itinerary, the policy will expire automatically and I am able to purchase a new policy online.</span>
                </br><p class="tncUnderWritten">Note: Travel Insurance is underwritten by Tune Protect Malaysia <span style="font-family: Arial, Verdana; font-size: x-small;"><i>(Tune Insurance Malaysia Berhad 30686-K)</i>.</span></p>--%>
        </div>
        
        <dx:ASPxCheckBox runat="server" ID="chkTerm" CheckBoxStyle-Font-Bold="true" Text="I have read and agree the Declarations and Authorisations, Terms and Conditions." >
        <ValidationSettings ValidationGroup="mandatory" ErrorTextPosition="Top" >
            <RequiredField IsRequired="true" ErrorText="Please indicate that you have read and agree to the Declarations and Authorisations, Terms and Conditions." />
        </ValidationSettings>
        </dx:ASPxCheckBox>

<%--            <dx:ASPxPageControl SkinID="None" Width="100%" EnableViewState="false" ID="TabControl" ClientInstanceName="TabControl"
                runat="server" ActiveTabIndex="0" AutoPostBack="false"
                TabSpacing="0px" ContentStyle-Border-BorderWidth="0"
                EnableHierarchyRecreation="True">

                <TabPages>
                    <dx:TabPage Text="" Name="Tab1" Visible="true">
                        <ContentCollection>
                            <dx:ContentControl ID="ContentControl1" runat="server">

                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
                </TabPages>
            </dx:ASPxPageControl>--%>
        </div>
        <table width="90%">
            <tr>
                <td></td>
            </tr>
        </table>
        <script type="text/javascript">
            $(document).ready(function () {
                $('#home .btnCtrl').on('click', function () {
                    $('#home .activeBtn').removeClass('activeBtn');
                    $(this).parent().addClass('activeBtn');
                });
                $('#home2 .btnCtrl').on('click', function () {
                    $('#home2 .activeBtn').removeClass('activeBtn');
                    $(this).parent().addClass('activeBtn');
                });
            });
        </script>
    </div>
    <asp:HiddenField ID="hfgvPassenger" runat="server" Value="" />
    <asp:HiddenField ID="hfIndex" runat="server" Value="" />
    <asp:HiddenField ID="hfInternational" runat="server" Value="" />
    <asp:HiddenField ID="MaxPax" runat="server" Value="" />
    <asp:HiddenField ID="MinPax" runat="server" Value="" />
    <asp:HiddenField ID="SAOCountryCode" runat="server" Value="VN" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <FBD:ucflightbookingdetail ID="ucflightbookingdetail" runat="server" />
    <PD:ucPaxDetail ID="ucPaxDetail" runat="server" />
</asp:Content>
