<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="bookingdetail.ascx.cs" Inherits="GroupBooking.Web.UserControl.bookingdetail" %>

<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %>

<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v16.1" %>

<script type="text/javascript">
    // <![CDATA[
    function gvClass_SelectionChanged(s, e) {
        s.GetSelectedFieldValues("TransID", GetSelectedFieldValuesCallback);
    }

    function gvRejectedClass_SelectionChanged(s, e) {
        s.GetSelectedFieldValues("TransID", GetSelectedFieldValuesCallback);
    }

    function ClosePopup() {
        popupControl.Hide();
    }

    function ShowPopup() {
        popupControl.Show();
    }

    function OnCallbackComplete(s, e) {
        if (s.cpIsUpdated != undefined && s.cpIsUpdated != "") {
            document.getElementById("ctl00_ContentPlaceHolder2_bkdetail_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = s.cpIsUpdated;
            s.cpIsUpdated = "";
            pcMessage.Show();
            LoadingPanel.Hide();
        }
    }

    function OnCallbackBegin(s, e) {
        s.cpIsUpdated = "";
    }

    function OnBtnResendClick(s, e) {
        if (e.result != "") {
            //alert(e.result);
            document.getElementById("ctl00_ContentPlaceHolder2_bkdetail_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = "Re-sent Itinerary Successfully";
            pcMessage.Show();
            LoadingPanel.Hide();
        } else {

        }
    }

    // ]]>

</script>

<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="true" ContainerElementID="FormPanel" />
<msg:msgControl ID="msgcontrol" runat="server" />
<dx:ASPxCallback ID="Callback" runat="server" ClientInstanceName="Callback" OnCallback="ValidatePopup">
    <clientsideevents endcallback="function (s, e) { OnBtnResendClick(s,e); }" />
</dx:ASPxCallback>

<dx:ASPxPopupControl ID="popupConfirm" runat="server" ClientInstanceName="popupControl" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
    CloseButtonImage-Url="~/Images/AirAsia/close_button_icon.jpg" Modal="true" AllowDragging="true" HeaderText="Cancel Confirmation" CloseAction="CloseButton" Width="250px">
    <closebuttonimage url="~/Images/AirAsia/close_button_icon.jpg"></closebuttonimage>
    <contentcollection>
        <dx:PopupControlContentControl runat="server">
            <table width="100%">
                <tr>
                    <td>Reason&nbsp;:&nbsp;</td>
                    <td>
                        <dx:ASPxTextBox ID="txtReason" runat="server" MaxLength="255" Size="100" >
                            <ValidationSettings ValidationGroup="cancellation" ErrorTextPosition="Bottom" ValidateOnLeave="true" >
                                <RequiredField IsRequired="true" ErrorText="Please enter the reason" />
                            </ValidationSettings>
                            <ClientSideEvents LostFocus="function(s,e){ if (ASPxClientEdit.ValidateGroup('cancellation')) { btnYes.SetEnabled(true); return true;} else {btnYes.SetEnabled(false); return true;}}" />
                        </dx:ASPxTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan = "2">
                        <br />
                        <!-- The cancellation process cannot be undone, please confirm the action -->
                        In order to cancel this booking, please click <b>Yes</b> to confirm or <b>No</b> to cancel this action.
                    </td>
                </tr>
                <tr>                        
                    <td colspan="2" align="center">
                    <br />
                        <table>
                            <tr>
                                <td>
                                    <dx:ASPxButton ID="btnYes" ClientEnabled="false" runat="server" Text="Yes" Width="50px" AutoPostBack="False" ClientInstanceName="btnYes" OnClick="btYes_Click">
                                        <ClientSideEvents Click="function(s,e){ ClosePopup();}"></ClientSideEvents>
                                    </dx:ASPxButton>
                                </td>
                                <td> &nbsp; </td>
                                <td>
                                    <dx:ASPxButton ID="btnNo" runat="server" Text="No" Width="50px" AutoPostBack="False">
                                        <ClientSideEvents Click="ClosePopup"></ClientSideEvents>
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </dx:PopupControlContentControl>
    </contentcollection>
</dx:ASPxPopupControl>

<div class="row page-header">
    <div class="col-sm-2">
        <h4 class="mt-0 mb-5">Booking Detail</h4>
        Booking/Booking Detail
    </div>
    <%--<div class="col-sm-1" style="text-align: center">
            <div style="color: green; font-size: 20px; font-weight: 700;"></div>
        </div>--%>
    <div class="col-sm-10">
        <div align="right" style="padding-top: 9px;">
            <table id="bookingDetail">
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <span id="paymentAttn" class="ti-bell"></span>
                        <dx:ASPxButton CssClass="buttonL2 noBgImg" ID="btPayFull" runat="server" Text="Need Payment" AutoPostBack="False" OnClick="btPayFull_Click">
                            <ClientSideEvents Click="function(s, e) {  LoadingPanel.SetText(&#39;Please Wait...&#39;); LoadingPanel.Show(); }" />
                        </dx:ASPxButton>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <dx:ASPxButton CssClass="buttonL2 noBgImg" ID="btManageSeats" runat="server" Text="Manage Seats" AutoPostBack="False" OnClick="btManageSeats_Click">
                            <ClientSideEvents Click="function(s, e) {  LoadingPanel.SetText(&#39;Please Wait...&#39;); LoadingPanel.Show(); }" />
                        </dx:ASPxButton>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <dx:ASPxButton CssClass="buttonL2 noBgImg" ID="btManageAddOn" runat="server" Text="Manage Add-On" AutoPostBack="False" OnClick="btManageAddOn_Click">
                            <ClientSideEvents Click="function(s, e) {  LoadingPanel.SetText(&#39;Please Wait...&#39;); LoadingPanel.Show(); }" />
                        </dx:ASPxButton>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <dx:ASPxButton CssClass="buttonL2 noBgImg" ID="btnManageInsure" runat="server" Text="Manage Insurance" AutoPostBack="False" OnClick="btManageInsure_Click">
                            <ClientSideEvents Click="function(s, e) {  LoadingPanel.SetText(&#39;Please Wait...&#39;); LoadingPanel.Show(); }" />
                        </dx:ASPxButton>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <dx:ASPxButton CssClass="buttonL2 noBgImg" ID="btFlightChange" runat="server" Text="Flight Change" AutoPostBack="False" OnClick="btFlightChange_Click">
                            <ClientSideEvents Click="function(s, e) {  LoadingPanel.SetText(&#39;Please Wait...&#39;); LoadingPanel.Show(); }" />
                        </dx:ASPxButton>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <dx:ASPxButton CssClass="buttonL2 noBgImg" ID="btDivide" runat="server" Text="Divide Booking" AutoPostBack="False" OnClick="btDivide_Click">
                            <ClientSideEvents Click="function(s, e) {  LoadingPanel.SetText(&#39;Please Wait...&#39;); LoadingPanel.Show(); }" />
                        </dx:ASPxButton>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <dx:ASPxButton CssClass="buttonL2 noBgImg" ID="btPassenger" runat="server" Text="Upload Passengers" AutoPostBack="False" OnClick="btPassenger_Click">
                            <ClientSideEvents Click="function(s, e) {  LoadingPanel.SetText(&#39;Please Wait...&#39;); LoadingPanel.Show(); }" />
                        </dx:ASPxButton>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <dx:ASPxButton CssClass="buttonL2 noBgImg" ID="btResendItinerary" runat="server" Text="Resend Itinerary" AutoPostBack="False" OnClick="btResendItinerary_Click">
                            <ClientSideEvents Click="function(s, e) {  LoadingPanel.SetText(&#39;Please Wait...&#39;); LoadingPanel.Show(); Callback.PerformCallback(); OnBtnResendClick(); }" />
                        </dx:ASPxButton>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <dx:ASPxButton CssClass="buttonL2 noBgImg" ID="btCancel" runat="server" Text="Cancel Transaction" AutoPostBack="False">
                            <ClientSideEvents Click="ShowPopup" />
                        </dx:ASPxButton>
                    </td>
                    <td style="display: none">
                        <dx:ASPxButton CssClass="buttonL2" ID="btGetLatest" runat="server" Text="Upload Passengers" AutoPostBack="False" OnClick="btGetLatest_Click">
                            <ClientSideEvents Click="function(s, e) {  LoadingPanel.SetText(&#39;Please Wait...&#39;); LoadingPanel.Show(); }" />
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>

<div class="row page-content container-fluid">
    <div class="col-sm-12">
        <div class="row">
            <div class="col-sm-5">
                <table id="totalEstimatedFare" class="enhanceFontSize" width="100%">
                    <tr class="totalFare">
                        <td>
                            <h4>Amount Due</h4>
                        </td>
                        <td style="text-align: right">
                            <h4>
                                <dx:ASPxLabel ID="lblTotalAmount" Text="" runat="server" ClientInstanceName="lblTotalAmount" />
                            </h4>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="col-sm-5">
                <table id="tbTotalPax" width="100%">
                    <tr class="totalFare">
                        <td class="" colspan="2" style="font-weight: 700;">
                            <h4>Total Pax</h4>
                        </td>
                        <td class="" style="padding-top: 5px; float: right">
                            <h4>
                                <asp:Label ID="lbl_num" runat="server"></asp:Label></h4>
                        </td>
                    </tr>
                </table>
            </div>
        </div>

        <div class="row">
            <div id="flightDetailBookingInfo" class="col-sm-5">
                <div class="redSectionHeader">
                    <div>Payment Details</div>
                </div>

                <!-- 02 Feb 2017 - Sienny -->
                <table id="paymentDetailsTbl" class="table table-bordered">
                    <tbody>
                        <tr>
                            <td class="">Booking ID</td>
                            <td class="">
                                <asp:Label ID="lblTransID" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="">Total Paid Amount</td>
                            <td class="">
                                <asp:Label ID="lblAmountPaid" runat="server" Text="0"></asp:Label>&nbsp;
                                <asp:Label ID="lblAmountPaidCurrency" runat="server" Text="MYR"></asp:Label>
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="">Depart Total</td>
                            <td class="">
                                <asp:Label ID="lblDepartTotal" runat="server" Text="0"></asp:Label>
                                &nbsp;
                                <asp:Label ID="lblDepartTotalCurrency" runat="server" Text="MYR"></asp:Label>
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="">Return Total</td>
                            <td class="">
                                <asp:Label ID="lblReturnTotal" runat="server" Text="0"></asp:Label>
                                &nbsp;
                                <asp:Label ID="lblReturnTotalCurrency" runat="server" Text="MYR"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="">Total Fare Amount</td>
                            <td>
                                <span>
                                    <asp:Label ID="lblCurrentTotal" runat="server" Text="0"></asp:Label>&nbsp;
                                    <asp:Label ID="lblCurrentTotalCurrency" runat="server" Text="MYR"></asp:Label>
                                </span>
                            </td>
                        </tr>
                        <tr id="trProcessingFee" runat="server" style="display: none">
                            <td style="">Processing Fee</td>
                            <td>
                                <span>
                                    <%--<asp:Label ID="lblProcessFee" runat="server" Text="0"></asp:Label>&nbsp;
                                    <asp:Label ID="lblCurrencyProcessFee" runat="server" Text="MYR"></asp:Label>--%>
                                </span>
                            </td>
                        </tr>

                    </tbody>
                </table>
                <!-- 02 Feb 2017 - Sienny -->

            </div>

            <div id="flightDetailBookingInfo1" class="col-sm-5" style="padding-left: 10px; margin-left: 0;">
                <div class="redSectionHeader">
                    <div>Payment Schedule</div>
                </div>
                <div id="divPaymentSchedule" runat="server"></div>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-12" style="padding-top: 10px;">
                <dx:ASPxPageControl SkinID="None" Width="100%" EnableViewState="false" ID="TabControl" ClientInstanceName="TabControl"
                    runat="server" ActiveTabIndex="0" AutoPostBack="True" TabSpacing="0px" ContentStyle-Border-BorderWidth="0" EnableHierarchyRecreation="True">
                    <TabPages>
                        <dx:TabPage Text="Fare Details" Name="TabFareDetail" Visible="true">
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl1" runat="server">
                                    <dx:ASPxGridView ID="gvClass" ClientInstanceName="gridClass" runat="server" KeyFieldName="TransID" Width="100%" OnCustomButtonCallback="gvClass_CustomButtonCallback" OnDataBinding="gridCustomers_CustomColumnSort"
                                        AutoGenerateColumns="False" SettingsBehavior-ColumnResizeMode="Control" OnCustomCallback="gvClass_Callback">
                                        <ClientSideEvents EndCallback="OnCallbackComplete" BeginCallback="OnCallbackBegin" />
                                        <Columns>
                                            <dx:GridViewCommandColumn VisibleIndex="0" Caption="Choose" Width="80px" ButtonType="Image" Visible="false">
                                                <CustomButtons>
                                                    <dx:GridViewCommandColumnCustomButton Text="Change" ID="changeBtnFinish" Image-AlternateText="Change Flight" Image-ToolTip="Change Flight" Image-Width="16px" Image-Url="../Images/AKBase/plane.png" />
                                                </CustomButtons>
                                            </dx:GridViewCommandColumn>
                                            <dx:GridViewDataColumn FieldName="Journey" VisibleIndex="1" GroupIndex="0">
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="RecordLocator" Caption="Booking No." VisibleIndex="2" />
                                            <dx:GridViewDataColumn FieldName="FlightNo" Caption="Flight" VisibleIndex="3" Width="60px" />
                                            <dx:GridViewDataColumn FieldName="SeqNo" Caption="SeqNo" VisibleIndex="3" Width="60px" Visible="false" />
                                            <dx:GridViewDataColumn FieldName="FareClass" Caption="Class" VisibleIndex="4" Width="45px" Visible="false" />
                                            <dx:GridViewDataColumn FieldName="TotalPax" Caption="Pax" VisibleIndex="5" Width="60px" />
                                            <dx:GridViewDataSpinEditColumn FieldName="LineFlight" Caption="Flight Fare" VisibleIndex="7">
                                                <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>
                                            </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="LineTax" Caption="Taxes & Fees" VisibleIndex="8">
                                                <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>
                                            </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="LineTotal" Caption="Total Booking" VisibleIndex="9">
                                                <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>
                                            </dx:GridViewDataSpinEditColumn>
                                            <dx:GridViewDataColumn FieldName="Currency" Caption=" " VisibleIndex="10" Width="65px" />
                                        </Columns>

                                        <Settings ShowGroupFooter="VisibleIfExpanded" />
                                        <GroupSummary>
                                            <dx:ASPxSummaryItem FieldName="LineTotal" SummaryType="Sum" DisplayFormat="{0:n2}" ShowInGroupFooterColumn="LineTotal" />
                                            <dx:ASPxSummaryItem FieldName="LineTax" SummaryType="Sum" DisplayFormat="{0:n2}" ShowInGroupFooterColumn="LineTax" />
                                            <dx:ASPxSummaryItem FieldName="LineFlight" SummaryType="Sum" DisplayFormat="{0:n2}" ShowInGroupFooterColumn="LineFlight" />
                                            <dx:ASPxSummaryItem FieldName="TotalPax" SummaryType="Sum" DisplayFormat="{0}" ShowInGroupFooterColumn="TotalPax" />
                                            <dx:ASPxSummaryItem FieldName="SeqNo" SummaryType="Max" DisplayFormat="{0}" ValueDisplayFormat="" ShowInGroupFooterColumn="SeqNo" />
                                        </GroupSummary>
                                        <SettingsBehavior AutoExpandAllGroups="True" ColumnResizeMode="Control"></SettingsBehavior>
                                        <ClientSideEvents SelectionChanged="gvClass_SelectionChanged" />
                                        <Styles>
                                            <Header BackColor="#333333" Font-Size="10pt" ForeColor="White"></Header>
                                        </Styles>
                                    </dx:ASPxGridView>
                                    <%//Added by Ellis 20170316, adding div to hide Rejected Fare Details if got no data%>
                                    <div id="divRejectedFare" runat="server" style="display: none">
                                        <div>Rejected Fare Details</div>
                                        <dx:ASPxGridView ID="gvRejectedClass" ClientInstanceName="gridClass2" runat="server" KeyFieldName="TransID" Width="100%" OnDataBinding="gvRejectedClass_CustomColumnSort"
                                            AutoGenerateColumns="False" SettingsBehavior-ColumnResizeMode="Control" OnCustomCallback="gvRejectedClass_Callback">
                                            <Columns>
                                                <dx:GridViewDataColumn FieldName="Journey" VisibleIndex="0" GroupIndex="0">
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="RecordLocator" Caption="Booking No." VisibleIndex="1" />
                                                <dx:GridViewDataColumn FieldName="FlightNo" Caption="Flight" VisibleIndex="3" Width="60px" />
                                                <dx:GridViewDataColumn FieldName="SeqNo" Caption="SeqNo" VisibleIndex="3" Width="60px" Visible="false" />
                                                <dx:GridViewDataColumn FieldName="FareClass" Caption="Class" VisibleIndex="4" Width="45px" Visible="false" />
                                                <dx:GridViewDataColumn FieldName="TotalPax" Caption="Pax" VisibleIndex="5" Width="60px" />
                                                <dx:GridViewDataSpinEditColumn FieldName="LineFlight" Caption="Flight Fare" VisibleIndex="7">
                                                    <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>
                                                </dx:GridViewDataSpinEditColumn>
                                                <dx:GridViewDataSpinEditColumn FieldName="LineTax" Caption="Taxes & Fees" VisibleIndex="8">
                                                    <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>
                                                </dx:GridViewDataSpinEditColumn>
                                                <dx:GridViewDataSpinEditColumn FieldName="LineTotal" Caption="Total Booking" VisibleIndex="9">
                                                    <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>
                                                </dx:GridViewDataSpinEditColumn>
                                                <dx:GridViewDataColumn FieldName="Currency" Caption=" " VisibleIndex="10" Width="65px" />
                                            </Columns>
                                            <Settings ShowGroupFooter="VisibleIfExpanded" />
                                            <GroupSummary>
                                                <dx:ASPxSummaryItem FieldName="LineTotal" SummaryType="Sum" DisplayFormat="{0:n2}" ShowInGroupFooterColumn="LineTotal" />
                                                <dx:ASPxSummaryItem FieldName="LineTax" SummaryType="Sum" DisplayFormat="{0:n2}" ShowInGroupFooterColumn="LineTax" />
                                                <dx:ASPxSummaryItem FieldName="LineFlight" SummaryType="Sum" DisplayFormat="{0:n2}" ShowInGroupFooterColumn="LineFlight" />
                                                <dx:ASPxSummaryItem FieldName="TotalPax" SummaryType="Sum" DisplayFormat="{0}" ShowInGroupFooterColumn="TotalPax" />
                                                <dx:ASPxSummaryItem FieldName="SeqNo" SummaryType="Max" DisplayFormat="{0}" ValueDisplayFormat="" ShowInGroupFooterColumn="SeqNo" />
                                            </GroupSummary>
                                            <SettingsBehavior ColumnResizeMode="Control"></SettingsBehavior>
                                            <ClientSideEvents SelectionChanged="gvRejectedClass_SelectionChanged" />
                                            <Styles>
                                                <Header BackColor="#333333" Font-Size="10pt" ForeColor="White"></Header>
                                            </Styles>
                                        </dx:ASPxGridView>
                                    </div>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>

                        <dx:TabPage Text="Fare Breakdown" Name="TabFareBreakdown" Visible="true">
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControlFareBreakdown" runat="server">
                                    <dx:ASPxCallbackPanel ID="cbFareBreakdown" ClientInstanceName="cbFareBreakdown" runat="server" Width="100%" OnCallback="cbFareBreakdown_Callback">
                                        <PanelCollection>
                                            <dx:PanelContent ID="pbFareBreakdown" runat="server" Width="100%">
                                                <div runat="server" id="divFareBreakdown" visible="false">
                                                    <table width="100%" bgcolor="#f7f3f7" class="table table-bordered" id="tblFareBreakdown" runat="server">
                                                        <tr id="NameChangeBreakdown">
                                                            <td>
                                                                <div class="infoFlight">
                                                                    <span class="infoFlightSpan">
                                                                        <asp:Label ID="Label1" runat="server" Visible="false"></asp:Label>Name Change<asp:Label ID="Label2" runat="server" Visible="false"></asp:Label>
                                                                        <asp:Label ID="Label3" runat="server" Visible="false"></asp:Label>
                                                                    </span>
                                                                    <span class="infoFlightSpan algnRight">
                                                                        <asp:Label ID="lbl_NameChangeTotal" runat="server"></asp:Label>
                                                                        <asp:Label ID="lbl_currency01" runat="server"></asp:Label>
                                                                    </span>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr id="InsureBreakdown">
                                                            <td>
                                                                <div class="infoFlight">
                                                                    <span class="infoFlightSpan">
                                                                        <asp:Label ID="Label61" runat="server" Visible="false"></asp:Label>Insurance Fee<asp:Label ID="Label62" runat="server" Visible="false"></asp:Label>
                                                                        <asp:Label ID="Label63" runat="server" Visible="false"></asp:Label>
                                                                    </span>
                                                                    <span class="infoFlightSpan algnRight">
                                                                        <asp:Label ID="lbl_InsureFeeTotal" runat="server"></asp:Label>
                                                                        <asp:Label ID="lbl_currency02" runat="server"></asp:Label>
                                                                    </span>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <font color='red'>Depart</font>
                                                            </td>
                                                            <td id="tdReturnTitle" runat="server">
                                                                <font color='red'>Return</font>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td id="tdDepart" runat="server">
                                                                <table class="tablebreakdown">
                                                                    <tr id="trDepartFare" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_num1" runat="server" Visible="false"></asp:Label>Depart Fare<asp:Label ID="lbl_Average" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lbl_currency0" runat="server" Visible="false"></asp:Label>
                                                                                </span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lbl_FlightTotal" runat="server"></asp:Label>
                                                                                    <asp:Label ID="Label4" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trProcessDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label38" runat="server" Visible="false"></asp:Label>Processing Fee<asp:Label ID="Label39" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="Label40" runat="server" Visible="false"></asp:Label>
                                                                                </span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblProcessTotalDepart" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblCurrProcessDepart" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trInfantFareDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_num11" runat="server">Infant Fare</asp:Label><asp:Label ID="lbl_InfantPrice" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lbl_currency11" runat="server" Visible="false"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lbl_InfantTotal" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lbl_currency12" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trAirportTaxDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_num2" runat="server">Airport Tax  </asp:Label><asp:Label ID="lbl_taxPrice" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lbl_currency3" runat="server" Visible="false"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lbl_taxTotal" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lbl_currency2" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trChildAirportDepart" runat="server" visible="false">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_num2CHD" runat="server"></asp:Label>
                                                                                    Child Airport Tax
                                                                                    <asp:Label ID="lbl_taxPriceCHD" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lbl_currency3CHD" runat="server" Visible="false"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lbl_taxTotalCHD" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lbl_currency2CHD" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trPaxServChargeDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_num21" runat="server"></asp:Label>Service Charge
                                                                                    <asp:Label ID="lbl_PaxFeePrice" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lbl_currency31" runat="server" Visible="false"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lbl_PaxFeeTotal" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lbl_currency21" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trFuelTaxDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_num3" runat="server" Visible="false"></asp:Label>Fuel Tax
                                                                                    <asp:Label ID="lblFuelPriceOneDepart" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lbl_currency4" runat="server" Visible="false"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblFuelPriceTotalDepart" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblCurrFuelDepart" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trServChargeDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_num4" runat="server" Visible="false"></asp:Label>Service Charge
                                                                                    <asp:Label ID="lblSvcChargeOneDepart" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lbl_currency5" runat="server" Visible="false"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblSvcChargeTotalDepart" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblCurrSvcDepart" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trVATDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_num5" runat="server" Visible="false"></asp:Label>VAT
                                                                                    <asp:Label ID="lblVATDepart" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lbl_currency6" runat="server" Visible="false"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblVATTotalDepart" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblCurrVATDepart" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <%--<tr id="trBaggageChargeDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_num7" runat="server"></asp:Label>Baggage Charge</span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblBaggageTotalDepart" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblCurrBaggageDepart" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trMealChargeDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_num8" runat="server"></asp:Label>Meal Charge</span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblMealTotalDepart" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblCurrMealDepart" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trSportChargeDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_num9" runat="server"></asp:Label>Sport Equipment Charge</span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblSportTotalDepart" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblCurrSportDepart" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trComfortChargeDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_num10" runat="server"></asp:Label>Comfort Kit Charge</span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblComfortTotalDepart" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblCurrComfortDepart" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>--%>
                                                                    <tr id="trSSRChargeDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label33" runat="server"></asp:Label>SSR Charge</span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblSSRTotalDepart" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblCurrSSRDepart" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trSeatChargeDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label5" runat="server"></asp:Label>Seat Charge</span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblSeatTotalDepart" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblCurrSeatDepart" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trOthChargeDepart" runat="server" visible="false">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_num6" runat="server" Visible="false"></asp:Label>Other Charge
                                                                                    <asp:Label ID="lblOthOneDepart" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lbl_currency7" runat="server" Visible="false"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblOthTotalDepart" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblCurrOthDepart" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trConnectingChargeDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label8" runat="server"></asp:Label>Connecting Charge
                                                                                    <asp:Label ID="Label9" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="Label10" runat="server" Visible="false"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblConnectingDepartTotal" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrConnectingDepart" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trKlia2FeeDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label11" runat="server">Klia2 Fee  </asp:Label><asp:Label ID="Label12" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="Label13" runat="server" Visible="false"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lbl_klia2Total" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrKlia2Depart" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trGSTChargeDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label14" runat="server"></asp:Label>GST Charge
                                                                                    <asp:Label ID="Label15" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="Label16" runat="server" Visible="false"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lbl_GSTTotal" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrGSTDepart" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trAVLChargeDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label17" runat="server" Visible="false"></asp:Label>AVL Charge
                                                                                    <asp:Label ID="Label18" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="Label19" runat="server" Visible="false"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblAVLTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrAVLDepart" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trPSFChargeDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_num15" runat="server"></asp:Label>PSF Charge </span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblPSFTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrPSFDepart" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trSCFChargeDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_num16" runat="server"></asp:Label>SCF Charge </span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblSCFTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrSCFDepart" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trSPLChargeDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label34" runat="server"></asp:Label>SPL Charge </span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblSPLTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrSPLDepart" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trAPSChargeDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label44" runat="server"></asp:Label>APS Charge </span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblAPSTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrAPSDepart" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trCSTChargeDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label46" runat="server"></asp:Label>Central GST Charge</span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblCSTTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrCSTDepart" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trCUTChargeDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label48" runat="server"></asp:Label>CUT Charge </span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblCUTTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrCUTDepart" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trSGIChargeDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label49" runat="server"></asp:Label>Surcharge Charge</span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblSGITotalDepart" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrSGIDepart" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trSSTChargeDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label50" runat="server"></asp:Label>State GST Charge</span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblSSTTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrSSTDepart" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trUDFChargeDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label51" runat="server"></asp:Label>UDF Charge</span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblUDFTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrUDFDepart" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trACFChargeDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label45" runat="server"></asp:Label>ACF Charge</span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblACFTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrACFDepart" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trIADFChargeDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label47" runat="server"></asp:Label>IADF Charge </span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblIADFTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrIADFDepart" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trASCChargeDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label57" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblASCInfoDepart" runat="server"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblASCTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrASCDepart" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trBCLChargeDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label58" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblBCLInfoDepart" runat="server"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblBCLTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrBCLDepart" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trCHGChargeDepart" runat="server" visible="false">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label37" runat="server"></asp:Label>CHG Charge </span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblCHGTotalChargeDepart" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrCHGTotalChargeDepart" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trDiscountChargeDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label20" runat="server"></asp:Label>Discount Charge</span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblDiscTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrDiscDepart" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trPromoDiscDepart" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_num14" runat="server" Visible="false"></asp:Label>Promotion Discount
                                                                                    <asp:Label ID="lblPromoDiscOneDepart" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lbl_currency14" runat="server" Visible="false"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblPromoDiscTotalDepart" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblCurrPromoDiscDepart" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>

                                                                    <asp:Repeater ID="rptFeeDepart" runat="server">
                                                                        <ItemTemplate>
                                                                            <tr>
                                                                                <td>
                                                                                    <div class="infoFlight">
                                                                                        <span class="infoFlightSpan">
                                                                                            <asp:Label ID="Label49" runat="server"></asp:Label>
                                                                                            <asp:Label ID="lblCodeDescDepart" runat="server" Text='<%# Eval("CodeDesc") + " Charge" %>'></asp:Label></span>
                                                                                        <span class="infoFlightSpan algnRight">
                                                                                            <asp:Label ID="lblFeeAmtDepart" runat="server" Text='<%# Eval("FeeAmt", "{0:N2}") %>'></asp:Label>
                                                                                            <asp:Label ID="lblFeeCurrDepart" runat="server"></asp:Label></span>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </table>
                                                            </td>
                                                            <td id="tdReturn" runat="server">
                                                                <table class="tablebreakdown">
                                                                    <tr id="trReturnfare" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_InNum" runat="server" Visible="false"></asp:Label>Return Fare<asp:Label ID="lbl_InAverage" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lbl_InCurrency0" runat="server" Visible="false"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lbl_InFlightTotal" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lbl_Incurrency1" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trInfantfareReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_InNum11" runat="server">Infant Fare</asp:Label><asp:Label ID="lbl_InInfantPrice" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lbl_InCurrency11" runat="server" Visible="false"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lbl_InInfantTotal" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lbl_Incurrency12" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trAirportTaxReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_InNum2" runat="server">Airport Tax  </asp:Label><asp:Label ID="lbl_IntaxPrice" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lbl_InCurrency3" runat="server" Visible="false"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lbl_IntaxTotal" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lbl_Incurrency2" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trChildAirportTaxReturn" runat="server" visible="false">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_InNum2CHD" runat="server"></asp:Label>
                                                                                    Child Airport Tax
                                                                                    <asp:Label ID="lbl_IntaxPriceCHD" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lbl_InCurrency3CHD" runat="server" Visible="false"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lbl_IntaxTotalCHD" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lbl_Incurrency2CHD" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="PaxServChargeReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_InNum21" runat="server"> Service Charge  </asp:Label><asp:Label ID="lbl_InPaxFeePrice" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lbl_InCurrency31" runat="server" Visible="false"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lbl_InPaxFeeTotal" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lbl_Incurrency21" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trFuelTaxReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_InNum3" runat="server" Visible="false"></asp:Label>Fuel Tax
                                                                                    <asp:Label ID="lblFuelOneReturn" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lbl_InCurrency4" runat="server" Visible="false"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblFuelTotalReturn" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblCurrFuelReturn" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trServChargeReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_InNum4" runat="server" Visible="false"></asp:Label>Service Charge
                                                                                    <asp:Label ID="lblSvcOneReturn" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lbl_InCurrency5" runat="server" Visible="false"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblSvcTotalReturn" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblCurrSvcReturn" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trVATReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_InNum5" runat="server" Visible="false"></asp:Label>VAT
                                                                                    <asp:Label ID="lblVATReturn" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lbl_InCurrency6" runat="server" Visible="false"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblVATTotalReturn" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblCurrVATReturn" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trSSRChargeReturn" runat="server">
                                                                        <td>

                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_InNum7" runat="server"></asp:Label>SSR Charge</span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblSSRTotalReturn" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblCurrSSRReturn" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <%--                                             <tr id="trBagggageChargeReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_InNum7" runat="server"></asp:Label>Baggage Charge</span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblBaggageTotalReturn" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblCurrBaggageReturn" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trMealChargeReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_InNum8" runat="server"></asp:Label>Meal Charge</span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblMealTotalReturn" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblCurrMealReturn" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trSportChargeReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_InNum9" runat="server"></asp:Label>Sport Equipment Charge</span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblSportTotalReturn" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblCurrSportReturn" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trComfortChargeReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_InNum10" runat="server"></asp:Label>Comfort Kit Charge</span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblComfortTotalReturn" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblCurrComfortReturn" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>--%>
                                                                    <tr id="trSeatChargeReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label7" runat="server"></asp:Label>Seat Charge</span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblSeatTotalReturn" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblCurrSeatReturn" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trOthChargeReturn" runat="server" visible="false">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_InNum6" runat="server" Visible="false"></asp:Label>Other Charge
                                                                                    <asp:Label ID="lblOthOneReturn" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lbl_InCurrency7" runat="server" Visible="false"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblOthTotalReturn" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblCurrOthReturn" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trConnectingChargeReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label6" runat="server">Connecting Charge </asp:Label><asp:Label ID="Label21" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="Label22" runat="server" Visible="false"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblConnectingReturnTotal" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrConnectingReturn" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trKlia2FeeReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label23" runat="server">Klia2 Fee  </asp:Label><asp:Label ID="Label24" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="Label25" runat="server" Visible="false"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lbl_Inklia2Total" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrKlia2Return" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trGSTChargeReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label26" runat="server">GST Charge </asp:Label><asp:Label ID="Label27" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="Label28" runat="server" Visible="false"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lbl_InGSTTotal" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrGSTReturn" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trAVLChargeReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label29" runat="server" Visible="false"></asp:Label>AVL Charge
                                                                                    <asp:Label ID="Label30" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="Label31" runat="server" Visible="false"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblAVLTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrAVLReturn" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trPSFChargeReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_InNum15" runat="server"></asp:Label>PSF Charge </span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblPSFTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrPSFReturn" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trSCFChargeReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_InNum16" runat="server"></asp:Label>SCF Charge </span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblSCFTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrSCFReturn" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trSPLChargeReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label35" runat="server"></asp:Label>SPL Charge </span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblSPLTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrSPLReturn" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trAPSChargeReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label41" runat="server"></asp:Label>APS Charge </span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblAPSTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrAPSReturn" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trCSTChargeReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label52" runat="server"></asp:Label>Central GST Charge Charge</span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblCSTTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrCSTReturn" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trCUTChargeReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label53" runat="server"></asp:Label>CUT Charge Charge</span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblCUTTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrCUTReturn" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trSGIChargeReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label54" runat="server"></asp:Label>Surcharge Charge</span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblSGITotalReturn" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrSGIReturn" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trSSTChargeReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label55" runat="server"></asp:Label>State GST Charge</span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblSSTTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrSSTReturn" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trUDFChargeReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label56" runat="server"></asp:Label>UDF Charge</span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblUDFTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrUDFReturn" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trACFChargeReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label43" runat="server"></asp:Label>ACF Charge </span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblACFTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrACFReturn" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trIADFChargeReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label42" runat="server"></asp:Label>IADF Charge </span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblIADFTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrIADFReturn" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trASCChargeReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label59" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblASCInfoReturn" runat="server"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblASCTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrASCReturn" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trBCLChargeReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label60" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblBCLInfoReturn" runat="server"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblBCLTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrBCLReturn" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trCHGChargeReturn" runat="server" visible="false">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label36" runat="server"></asp:Label>CHG Charge </span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblCHGTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrCHGTotalReturnn" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trDiscountChargeReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="Label32" runat="server"></asp:Label>Discount Charge</span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblDiscTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                                                    <asp:Label ID="lblCurrDiscReturn" runat="server"></asp:Label></span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trPromoDiscReturn" runat="server">
                                                                        <td>
                                                                            <div class="infoFlight">
                                                                                <span class="infoFlightSpan">
                                                                                    <asp:Label ID="lbl_InNum14" runat="server" Visible="false"></asp:Label>Promotion Discount
                                                                                    <asp:Label ID="lblPromoDiscOneReturn" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lbl_InCurrency14" runat="server" Visible="false"></asp:Label></span>
                                                                                <span class="infoFlightSpan algnRight">
                                                                                    <asp:Label ID="lblPromoDiscTotalReturn" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblCurrPromoDiscReturn" runat="server"></asp:Label>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>

                                                                    <asp:Repeater ID="rptFeeReturn" runat="server">
                                                                        <ItemTemplate>
                                                                            <tr>
                                                                                <td>
                                                                                    <div class="infoFlight">
                                                                                        <span class="infoFlightSpan">
                                                                                            <asp:Label ID="Label49" runat="server"></asp:Label>
                                                                                            <asp:Label ID="lblCodeDescReturn" runat="server" Text='<%# Eval("CodeDesc") + " Charge" %>'></asp:Label></span>
                                                                                        <span class="infoFlightSpan algnRight">
                                                                                            <asp:Label ID="lblFeeAmtReturn" runat="server" Text='<%# Eval("FeeAmt", "{0:N2}") %>'></asp:Label>
                                                                                            <asp:Label ID="lblFeeCurrReturn" runat="server"></asp:Label></span>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div class="infoFlight">
                                                                    <span class="infoFlightSpan boldFont">Total Amount</span>
                                                                    <span class="infoFlightSpan algnRight">
                                                                        <asp:Label ID="lbl_Total" runat="server"></asp:Label>
                                                                        <asp:Label ID="lbl_currency" runat="server"></asp:Label>
                                                                    </span>
                                                                </div>
                                                            </td>
                                                            <td id="tdReturnFare" runat="server">
                                                                <div class="infoFlight">
                                                                    <span class="infoFlightSpan boldFont">Total Amount</span>
                                                                    <span class="infoFlightSpan algnRight">
                                                                        <asp:Label ID="lbl_InTotal" runat="server"></asp:Label>
                                                                        <asp:Label ID="lbl_InCurrency" runat="server"></asp:Label>
                                                                    </span>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>

                                                <%--<dx:ASPxGridView ID="gvFareBreakdown" ClientInstanceName="gridFareBreakdown" runat="server" KeyFieldName="TransID" Width="100%"  OnCustomButtonCallback="gvClass_CustomButtonCallback" OnDataBinding="gridCustomers_CustomColumnSort" AutoGenerateColumns="False" SettingsBehavior-ColumnResizeMode="Control" OnCustomCallback="gvClass_Callback" >
					                    <ClientSideEvents EndCallback="OnCallbackComplete" BeginCallback="OnCallbackBegin" />
					                    <Columns>
						                    <dx:GridViewDataColumn FieldName="TransID" Caption="Booking No." VisibleIndex="0" />
						                    <dx:GridViewDataColumn FieldName="PaxAdult" Caption="Pax Adult" VisibleIndex="1" Width="60px" />
						                    <dx:GridViewDataColumn FieldName="PaxChild" Caption="Pax Child" VisibleIndex="2" Width="60px" />
						                    <dx:GridViewDataSpinEditColumn  FieldName="FarePerPax" Caption="Fare Per Pax" VisibleIndex="3">
							                    <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                    
						                    </dx:GridViewDataSpinEditColumn> 
						                    <dx:GridViewDataSpinEditColumn  FieldName="FareAmount" Caption="Fare Amount" VisibleIndex="4">
							                    <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                    
						                    </dx:GridViewDataSpinEditColumn>
						                    <dx:GridViewDataSpinEditColumn FieldName="LineInfant" Caption="Line Infant" VisibleIndex="5" >              
							                    <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                                                      
						                    </dx:GridViewDataSpinEditColumn> 
						                    <dx:GridViewDataSpinEditColumn FieldName="LineTax" Caption="Taxes & Fees" VisibleIndex="6" >              
							                    <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                                                      
						                    </dx:GridViewDataSpinEditColumn>
						                    <dx:GridViewDataSpinEditColumn FieldName="LineCharge" Caption="Fuel Tax" VisibleIndex="7" >
							                    <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                    
						                    </dx:GridViewDataSpinEditColumn> 
						                    <dx:GridViewDataSpinEditColumn FieldName="LineFee" Caption="Service Charge" VisibleIndex="8" >
							                    <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                    
						                    </dx:GridViewDataSpinEditColumn> 
						                    <dx:GridViewDataSpinEditColumn FieldName="LineVat" Caption="VAT" VisibleIndex="9" >
							                    <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                    
						                    </dx:GridViewDataSpinEditColumn> 
						                    <dx:GridViewDataSpinEditColumn FieldName="LineProcess" Caption="Process Charge" VisibleIndex="10" >
							                    <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                    
						                    </dx:GridViewDataSpinEditColumn> 
						                    <dx:GridViewDataSpinEditColumn FieldName="LineSSR" Caption="SSR" VisibleIndex="11" >
							                    <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                    
						                    </dx:GridViewDataSpinEditColumn> 
						                    <dx:GridViewDataSpinEditColumn FieldName="LineSeat" Caption="Seat Charge" VisibleIndex="12" >
							                    <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                    
						                    </dx:GridViewDataSpinEditColumn> 
						                    <dx:GridViewDataSpinEditColumn FieldName="LineOth" Caption="Other Charge" VisibleIndex="13" >
							                    <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                    
						                    </dx:GridViewDataSpinEditColumn> 
						                    <dx:GridViewDataSpinEditColumn FieldName="LineTotal" Caption="Total Booking" VisibleIndex="14" >
							                    <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                    
						                    </dx:GridViewDataSpinEditColumn> 
					                    </Columns>
					
					                    <ClientSideEvents SelectionChanged="gvClass_SelectionChanged" />
					
					                    <styles>
						                    <header backcolor="#333333" font-size="10pt" forecolor="White">
						                    </header>
					                    </styles>
				                    </dx:ASPxGridView>			--%>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxCallbackPanel>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>

                        <dx:TabPage Text="Payment" Name="TabPayment" Visible="true">
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl2" runat="server">
                                    <div runat="server" id="divPayment" visible="false">
                                        <table width="100%">
                                            <tr style="vertical-align: middle; height: 30px; background-color: #333333; color: White; font-size: small;">
                                                <td>&nbsp;Payment History</td>
                                            </tr>
                                        </table>

                                        <div class="itinhr top"></div>
                                        <table id="paymentHistory" style="color: #737373;">
                                            <tbody>
                                                <tr>
                                                    <td width='100px'><b>Booking No</b></td>
                                                    <td width='150px'><b>Payment Method</b></td>
                                                    <td width='200px'><b>Amount</b></td>
                                                    <td width='200px'><b>Transaction Date</b></td>
                                                    <td width='150px'><b>Reference No</b></td>
                                                    <td width='100px'><b>Status</b></td>
                                                    <td width='200px'><b>Remark</b></td>
                                                </tr>
                                                <asp:Literal ID="ltrPayment" runat="server"></asp:Literal>
                                            </tbody>
                                        </table>
                                        <div class="itinhr bottom"></div>

                                        <table class="paymentDisplay">
                                        </table>
                                    </div>

                                    <%--                                    <dx:ASPxGridView ID="gvPayment" runat="server" ShowSelectCheckbox="True" AutoGenerateColumns="False" Width="100%" KeyFieldName="MerchantID" ClientInstanceName="gvPayment"
                                                                        ClientVisible="True">
                                                                        <SettingsLoadingPanel Mode="Disabled" />
                                                                        <SettingsBehavior EnableRowHotTrack="True" AutoExpandAllGroups="true" ColumnResizeMode="NextColumn" AllowSelectByRowClick="false" AllowFocusedRow="true" />
                                                                        <Columns>

                                                                            <dx:GridViewDataTextColumn FieldName="MerchantID" Caption="MerchantID" VisibleIndex="0" Width="100px" Visible="false"/>
                                                                            <dx:GridViewDataTextColumn FieldName="RecordLocator" Caption="PNR" VisibleIndex="0" Width="200px" />
                                                                            <dx:GridViewDataTextColumn FieldName="TenderDesc" Caption="Payment Method" VisibleIndex="1" Width="200px" />
                                                                            <dx:GridViewDataTextColumn FieldName="TenderAmt" Caption="Amount" VisibleIndex="2" Width="150px" CellStyle-HorizontalAlign="Right">
                                                                                <PropertiesTextEdit DisplayFormatString="#,#.00"/>
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn FieldName="TransDate" Caption="Transaction Date" VisibleIndex="3" Width="200px" />
                                                                            <dx:GridViewDataTextColumn FieldName="RefNo" Caption="Reference No." VisibleIndex="4" Width="150px" />
                                                                            <dx:GridViewDataTextColumn FieldName="TransVoid" Caption="Status" VisibleIndex="5" Width="150px" />

                                                                        </Columns>
                                                                        <SettingsPager Mode="ShowAllRecords" />
                                                                        <Styles Header-Wrap="True" Header-HorizontalAlign="Center" Header-VerticalAlign="Middle">
                                                                            <Row Cursor="pointer" />
                                                                        </Styles>
                                                                    </dx:ASPxGridView>--%>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>

                        <dx:TabPage Text="Passenger List" Name="TabPassengerList" Visible="true">
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl3" runat="server">
                                    <dx:ASPxGridView ID="gvPassenger" ClientInstanceName="gvPassenger" runat="server" KeyFieldName="PassengerID" Width="100%" AutoGenerateColumns="False" SettingsBehavior-ColumnResizeMode="Control" OnCustomCallback="gvPassenger_Callback">
                                        <Columns>
                                            <dx:GridViewDataColumn FieldName="PNR" VisibleIndex="0" GroupIndex="0" />
                                            <dx:GridViewDataColumn FieldName="IssuingCountryName" Caption="Issuing Country" VisibleIndex="0" Width="110px" />
                                            <dx:GridViewDataColumn FieldName="countryName" Caption="Nationality" VisibleIndex="1" Width="90px" />
                                            <dx:GridViewDataColumn FieldName="Title" Caption="Title" VisibleIndex="4" Width="35px" />
                                            <dx:GridViewDataColumn FieldName="Gender" Caption="Gender" VisibleIndex="5" Width="60px" />
                                            <dx:GridViewDataColumn FieldName="FirstName" Caption="First Name" VisibleIndex="6" Width="90px" />
                                            <dx:GridViewDataColumn FieldName="LastName" Caption="Last Name" VisibleIndex="7" Width="90px" />
                                            <dx:GridViewDataDateColumn FieldName="DOB" Caption="DOB" VisibleIndex="8" Width="90px">
                                                <PropertiesDateEdit DisplayFormatString="dd MMM yyyy"></PropertiesDateEdit>
                                            </dx:GridViewDataDateColumn>
                                            <dx:GridViewDataColumn FieldName="PassportNo" Caption="Passport No." VisibleIndex="9" Width="90px" />
                                            <dx:GridViewDataDateColumn FieldName="ExpiryDate" Caption="Expiry Date" VisibleIndex="10" Width="90px">
                                                <PropertiesDateEdit DisplayFormatString="dd MMM yyyy"></PropertiesDateEdit>
                                            </dx:GridViewDataDateColumn>
                                            <dx:GridViewDataColumn FieldName="ChangeCnt" Caption="Changes" VisibleIndex="10" Width="70px">
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="Insurance" Caption="Insurance" VisibleIndex="10" Width="70px">
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="DepartSeat" Caption="DepartSeat" VisibleIndex="11" Width="80px" />
                                            <dx:GridViewDataColumn FieldName="DepartConnectingSeat" Caption="DepartConnectingSeat" VisibleIndex="11" Width="80px" Visible="false" />
                                            <dx:GridViewDataColumn FieldName="ReturnSeat" Caption="ReturnSeat" VisibleIndex="12" Width="80px" />
                                            <dx:GridViewDataColumn FieldName="ReturnConnectingSeat" Caption="ReturnConnectingSeat" VisibleIndex="12" Width="80px" Visible="false" />

                                        </Columns>
                                        <SettingsPager PageSize="50" Mode="ShowAllRecords"></SettingsPager>
                                        <SettingsDetail ExportMode="All" />
                                        <Settings HorizontalScrollBarMode="Auto" ShowGroupFooter="VisibleIfExpanded" />
                                        <GroupSummary>
                                            <dx:ASPxSummaryItem FieldName="LineTotal" SummaryType="Count" DisplayFormat="{0}" ShowInGroupFooterColumn="PNR" />
                                        </GroupSummary>
                                        <SettingsBehavior ColumnResizeMode="Control"></SettingsBehavior>
                                        <Settings ShowGroupFooter="VisibleIfExpanded"></Settings>
                                        <SettingsDetail ExportMode="All"></SettingsDetail>
                                        <Styles>
                                            <Header BackColor="#333333" ForeColor="White"></Header>
                                        </Styles>
                                    </dx:ASPxGridView>

                                    <div id="InfantDiv" runat="server" style="display:none;">
                                        <div style="margin-top: 20px;" id="divInfant" runat="server">
                                            <asp:Label runat="server" ID="Label64"><h4>Infant Details</h4>
                                            <br />
                                            </asp:Label>
                                        </div>
                                        <dx:ASPxGridView ID="gvInfant" ClientInstanceName="gvInfant" runat="server" KeyFieldName="RecordLocator;PassengerID" Width="1051" AutoGenerateColumns="False" SettingsBehavior-ColumnResizeMode="Control" OnCustomCallback="gvInfant_Callback">
                                            <%--                                        OnBatchUpdate="gvInfant_BatchUpdate" OnHtmlDataCellPrepared="gvInfant_HtmlDataCellPrepared" OnCustomCallback="gvInfant_CustomCallback">--%>
                                            <%--                                        <SettingsEditing BatchEditSettings-EditMode="Row"></SettingsEditing>--%>
                                            <Columns>
                                                <dx:GridViewDataColumn FieldName="RecordLocator" Caption="PNR" VisibleIndex="0" GroupIndex="0" />
                                                <%--                                            <dx:GridViewDataColumn FieldName="PaxNo" Caption="Pax No" ReadOnly="true" Width="50px" VisibleIndex="0" />--%>
                                                <dx:GridViewDataColumn FieldName="PassengerID" Caption="PassengerID" Visible="false" VisibleIndex="0" Width="100px" />
                                                <dx:GridViewDataComboBoxColumn FieldName="IssuingCountryName" Caption="Issuing Country" VisibleIndex="0" Width="100px">
                                                </dx:GridViewDataComboBoxColumn>
                                                <dx:GridViewDataComboBoxColumn FieldName="CountryName" Caption="Nationality" VisibleIndex="1" Width="100px">
                                                </dx:GridViewDataComboBoxColumn>
                                                <dx:GridViewDataComboBoxColumn FieldName="Gender" Caption="Gender" VisibleIndex="5" Width="90px">
                                                </dx:GridViewDataComboBoxColumn>
                                                <dx:GridViewDataColumn FieldName="ParentFirstName" Caption="Parent First Name" VisibleIndex="6" Width="150px" />
                                                <dx:GridViewDataColumn FieldName="ParentLastName" Caption="Parent Last Name" VisibleIndex="7" Width="150px" />
                                                <dx:GridViewDataTextColumn FieldName="FirstName" Caption="First Name" VisibleIndex="8" Width="110px">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="LastName" Caption="Last Name" VisibleIndex="9" Width="110px">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataDateColumn FieldName="DOB" Caption="DOB" VisibleIndex="10" Width="90px">
                                                <PropertiesDateEdit DisplayFormatString="dd MMM yyyy"></PropertiesDateEdit>
                                                </dx:GridViewDataDateColumn>
                                                <dx:GridViewDataTextColumn FieldName="PassportNo" Caption="Passport No." VisibleIndex="11" Width="80px">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataDateColumn FieldName="ExpiryDate" Caption="Expiry Date" VisibleIndex="12" Width="80px">
                                                <PropertiesDateEdit DisplayFormatString="dd MMM yyyy"></PropertiesDateEdit>
                                                </dx:GridViewDataDateColumn>
                                            </Columns>

                                            <%--                                        <Templates>
                                                <StatusBar>
                                                    <div style="text-align: right">
                                                        <%--<dx:ASPxHyperLink ID="hlSave" runat="server" Text="Save changes">
                                                                <ClientSideEvents Click="onEditGrid" />
                                                            </dx:ASPxHyperLink>--%
                                                        <dx:ASPxHyperLink ID="hlCancel" runat="server" Text="Cancel changes">
                                                            <ClientSideEvents Click="onCancelGrid" />
                                                        </dx:ASPxHyperLink>
                                                    </div>
                                                </StatusBar>
                                            </Templates>
                                            <SettingsEditing Mode="Batch" />--%>
                                            <SettingsPager PageSize="50" Mode="ShowAllRecords"></SettingsPager>
                                            <SettingsDetail ExportMode="All" />
                                            <Settings HorizontalScrollBarMode="Auto" ShowGroupFooter="VisibleIfExpanded" />
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
                                    </div>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>

                        <dx:TabPage Text="Add-On" Name="TabAddOn" Visible="true">
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl4" runat="server">
                                    <%--                                <table width="100%">
                                 <tr  style="vertical-align:middle;height:30px;background-color:#333333;color:White;font-size:small;">
                                  <td>&nbsp;Summary of Charges</td>
                                </tr></table>
                
                                  <table class="priceDisplay" width="95%" style="background-color:#EEEEEE;">
                                    <tbody>
                                      <asp:Literal ID="ltrSummary" runat="server"></asp:Literal>         
                                    </tbody>
                                  </table>--%>
                                    <dx:ASPxGridView ID="gvAddOn" runat="server" ShowSelectCheckbox="True" AutoGenerateColumns="False" KeyFieldName="SeqNo;PassengerID" Width="100%" ClientInstanceName="gvAddOn"
                                        ClientVisible="True" EnableRowsCache="false" EnableCallBacks="true">
                                        <SettingsLoadingPanel Mode="Disabled" />
                                        <SettingsBehavior EnableRowHotTrack="True" AutoExpandAllGroups="true" AllowGroup="true" ColumnResizeMode="NextColumn" AllowSelectByRowClick="false" AllowFocusedRow="true" />
                                        <Columns>
                                            <dx:GridViewDataColumn FieldName="PNR" Caption="PNR" ReadOnly="true" Width="150px" />
                                            <dx:GridViewDataTextColumn FieldName="PassengerID" Caption="PassengerID" ReadOnly="true" Visible="false" />
                                            <dx:GridViewDataTextColumn FieldName="FlightNo" Caption="FlightNo" ReadOnly="true" Visible="false" />
                                            <dx:GridViewDataTextColumn FieldName="FirstName" Caption="First Name" ReadOnly="true" Width="250px" />
                                            <dx:GridViewDataTextColumn FieldName="LastName" Caption="Last Name" ReadOnly="true" Width="250px" />
                                            <dx:GridViewDataTextColumn FieldName="DepartMeal" Caption="Depart Meal" Width="250px" />
                                            <dx:GridViewDataTextColumn FieldName="ConDepartMeal" Caption="Depart Connecting Meal" Width="250px" />
                                            <dx:GridViewDataTextColumn FieldName="DepartBaggage" Caption="Depart Baggage" Width="250px" />
                                            <dx:GridViewDataTextColumn FieldName="DepartSport" Caption="Depart Sport Equipment" Width="250px" />
                                            <dx:GridViewDataTextColumn FieldName="DepartInsure" Caption="Depart Insure Equipment" Width="250px" Visible="false" />
                                            <dx:GridViewDataTextColumn FieldName="DepartComfort" Caption="Depart Comfort Kit" Width="250px" />
                                            <dx:GridViewDataTextColumn FieldName="DepartInfant" Caption="Depart Infant" Width="250px" />
                                            <dx:GridViewDataTextColumn FieldName="ReturnMeal" Caption="Return Meal" Width="250px" />
                                            <dx:GridViewDataTextColumn FieldName="ConReturnMeal" Caption="Return Connecting Meal" Width="250px" />
                                            <dx:GridViewDataTextColumn FieldName="ReturnBaggage" Caption="Return Baggage" Width="250px" />
                                            <dx:GridViewDataTextColumn FieldName="ReturnSport" Caption="Return Sport Equipment" Width="250px" />
                                            <dx:GridViewDataTextColumn FieldName="ReturnInsure" Caption="Return Insure Equipment" Width="250px" Visible="false" />
                                            <dx:GridViewDataTextColumn FieldName="ReturnComfort" Caption="Return Comfort Kit" Width="250px" />
                                            <dx:GridViewDataTextColumn FieldName="ReturnInfant" Caption="Return Infant" Width="250px" />
                                            <%--dx:GridViewDataTextColumn FieldName="Duty Free" Caption="Duty Free" Width="250px"/>--%>
                                        </Columns>
                                        <SettingsPager Mode="ShowPager" PageSize="60" Position="Top" />
                                        <Settings HorizontalScrollBarMode="Auto" ShowGroupFooter="VisibleIfExpanded" />
                                        <Styles Header-Wrap="True" Header-HorizontalAlign="Center" Header-VerticalAlign="Middle">
                                            <Row Cursor="pointer" />
                                        </Styles>
                                    </dx:ASPxGridView>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                    </TabPages>
                </dx:ASPxPageControl>

                <%--            <ul class="nav nav-tabs">
                                <li class="active">
                                    <a href="#home" data-toggle="tab">Fare Details</a>
                                </li>
                                <li><a href="#menu1" data-toggle="tab">Payment</a></li>
                                <li><a href="#menu2" data-toggle="tab">Passenger List</a></li>
                                <li><a href="#menu3" data-toggle="tab">Add On</a></li>
                            </ul>--%>
                <%--            <div class="tab-content">                
                                <div id="home" class="tab-pane fade in active">
                                    <div>Fare Details</div>
                                    <div style="max-width: 1500px;padding-top: 10px;">
                                                        <dx:ASPxGridView ID="gvClass" ClientInstanceName="gridClass" runat="server" 
                                                            KeyFieldName="TransID" Width="100%"  OnCustomButtonCallback="gvClass_CustomButtonCallback" OnDataBinding="gridCustomers_CustomColumnSort" 
                                                                    AutoGenerateColumns="False" 
                                                            SettingsBehavior-ColumnResizeMode="Control" 
                                                            OnCustomCallback="gvClass_Callback" >
                                                            <ClientSideEvents EndCallback="OnCallbackComplete" BeginCallback="OnCallbackBegin" />
                                                            <Columns>
                                                                <dx:GridViewCommandColumn VisibleIndex="0" Caption="Choose" Width="80px" ButtonType="Image" Visible="false" >                 
                                                                 <CustomButtons>
                                                                     <dx:GridViewCommandColumnCustomButton Text="Change" ID="changeBtnFinish" 
                                                                     Image-AlternateText="Change Flight" Image-ToolTip="Change Flight" 
                                                                     Image-Width="16px" Image-Url="../Images/AKBase/plane.png" />
                                                     
                                                                 </CustomButtons> 
                                                                 </dx:GridViewCommandColumn>
                                                               <dx:GridViewDataColumn FieldName="Journey" VisibleIndex="1" GroupIndex="0" > 

                                                               </dx:GridViewDataColumn>                                                                   
                                                                <dx:GridViewDataColumn FieldName="RecordLocator" Caption="Booking No." VisibleIndex="2" />                                                
                                                                <dx:GridViewDataColumn FieldName="FlightNo" Caption="Flight" VisibleIndex="3" 
                                                                    Width="60px" />
                                                                <dx:GridViewDataColumn FieldName="SeqNo" Caption="SeqNo" VisibleIndex="3" 
                                                                    Width="60px" Visible="false"/>
                                                                <dx:GridViewDataColumn FieldName="FareClass" Caption="Class" VisibleIndex="4" 
                                                                    Width="45px" Visible="false" />
                                                                <dx:GridViewDataColumn FieldName="TotalPax" Caption="Pax" 
                                                                    VisibleIndex="5" Width="60px" />                                                 
                                                                <dx:GridViewDataSpinEditColumn  FieldName="LineFlight" Caption="Flight Fare" VisibleIndex="7">
                                                                <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                    
                                                                </dx:GridViewDataSpinEditColumn> 
                                                                <dx:GridViewDataSpinEditColumn FieldName="LineTax" Caption="Taxes & Fees" VisibleIndex="8" >              
                                                                <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                                                      
                                                                </dx:GridViewDataSpinEditColumn>
                                                                <dx:GridViewDataSpinEditColumn FieldName="LineTotal" Caption="Total Booking" VisibleIndex="9" >
                                                                <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                    
                                                                </dx:GridViewDataSpinEditColumn> 
                                                                <dx:GridViewDataColumn FieldName="Currency" Caption=" " 
                                                                    VisibleIndex="10" Width="65px" />
                                                            </Columns>
                                                          <Settings ShowGroupFooter="VisibleIfExpanded" />
                                                            <GroupSummary>
                                                                <dx:ASPxSummaryItem FieldName="LineTotal" SummaryType="Sum" DisplayFormat="{0:n2}" ShowInGroupFooterColumn="LineTotal"/>
                                                                <dx:ASPxSummaryItem FieldName="LineTax" SummaryType="Sum" DisplayFormat="{0:n2}" ShowInGroupFooterColumn="LineTax"/>
                                                               <dx:ASPxSummaryItem FieldName="LineFlight" SummaryType="Sum" DisplayFormat="{0:n2}" ShowInGroupFooterColumn="LineFlight"/>
                                                                <dx:ASPxSummaryItem FieldName="TotalPax" SummaryType="Sum" DisplayFormat="{0}" ShowInGroupFooterColumn="TotalPax"/>
                                                               <dx:ASPxSummaryItem FieldName="SeqNo" SummaryType="Max" DisplayFormat="{0}" ValueDisplayFormat="" ShowInGroupFooterColumn="SeqNo" />
                                                            </GroupSummary>
                                                          <SettingsBehavior ColumnResizeMode="Control"></SettingsBehavior>
                                                           <ClientSideEvents SelectionChanged="gvClass_SelectionChanged" />
                                                           <styles>
                                                                <header backcolor="#333333" font-size="10pt" forecolor="White">
                                                                </header>
                                                            </styles>
                                                        </dx:ASPxGridView>
                                    </div>
                                    <div>Rejected Fare Details</div>
                                    <div style="max-width: 1500px;padding-top: 10px;">
                                                        <dx:ASPxGridView ID="gvRejectedClass" ClientInstanceName="gridClass2" runat="server" 
                                                            KeyFieldName="TransID" Width="100%" OnDataBinding="gvRejectedClass_CustomColumnSort" 
                                                                    AutoGenerateColumns="False" 
                                                            SettingsBehavior-ColumnResizeMode="Control" 
                                                            OnCustomCallback="gvRejectedClass_Callback" >
                                                            <Columns>    
                                                               <dx:GridViewDataColumn FieldName="Journey" VisibleIndex="0" GroupIndex="0" > 

                                                               </dx:GridViewDataColumn>                                                                   
                                                                <dx:GridViewDataColumn FieldName="RecordLocator" Caption="Booking No." VisibleIndex="1" />                                                
                                                                <dx:GridViewDataColumn FieldName="FlightNo" Caption="Flight" VisibleIndex="3" 
                                                                    Width="60px" />
                                                                <dx:GridViewDataColumn FieldName="SeqNo" Caption="SeqNo" VisibleIndex="3" 
                                                                    Width="60px" Visible="false"/>
                                                                <dx:GridViewDataColumn FieldName="FareClass" Caption="Class" VisibleIndex="4" 
                                                                    Width="45px" Visible="false" />
                                                                <dx:GridViewDataColumn FieldName="TotalPax" Caption="Pax" 
                                                                    VisibleIndex="5" Width="60px" />    
                                                                                                      
                                                                <dx:GridViewDataSpinEditColumn  FieldName="LineFlight" Caption="Flight Fare" VisibleIndex="7">
                                                                <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                    
                                                                </dx:GridViewDataSpinEditColumn> 
                                                                <dx:GridViewDataSpinEditColumn FieldName="LineTax" Caption="Taxes & Fees" VisibleIndex="8" >              
                                                                <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                                                      
                                                                </dx:GridViewDataSpinEditColumn>
                                                                <dx:GridViewDataSpinEditColumn FieldName="LineTotal" Caption="Total Booking" VisibleIndex="9" >
                                                                <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                    
                                                                </dx:GridViewDataSpinEditColumn> 
                                                                <dx:GridViewDataColumn FieldName="Currency" Caption=" " 
                                                                    VisibleIndex="10" Width="65px" />
                                                            </Columns>
                                                          <Settings ShowGroupFooter="VisibleIfExpanded" />
                                                            <GroupSummary>
                                                                <dx:ASPxSummaryItem FieldName="LineTotal" SummaryType="Sum" DisplayFormat="{0:n2}" ShowInGroupFooterColumn="LineTotal"/>
                                                                <dx:ASPxSummaryItem FieldName="LineTax" SummaryType="Sum" DisplayFormat="{0:n2}" ShowInGroupFooterColumn="LineTax"/>
                                                                <dx:ASPxSummaryItem FieldName="LineFlight" SummaryType="Sum" DisplayFormat="{0:n2}" ShowInGroupFooterColumn="LineFlight"/>
                                                                <dx:ASPxSummaryItem FieldName="TotalPax" SummaryType="Sum" DisplayFormat="{0}" ShowInGroupFooterColumn="TotalPax"/>
                                                               <dx:ASPxSummaryItem FieldName="SeqNo" SummaryType="Max" DisplayFormat="{0}" ValueDisplayFormat="" ShowInGroupFooterColumn="SeqNo" />
                                                            </GroupSummary>
                                                          <SettingsBehavior ColumnResizeMode="Control"></SettingsBehavior>
                                                           <ClientSideEvents SelectionChanged="gvRejectedClass_SelectionChanged" />
                                                           <styles>
                                                                <header backcolor="#333333" font-size="10pt" forecolor="White">
                                                                </header>
                                                            </styles>
                                                        </dx:ASPxGridView>
                                    </div>                   
                                </div>
                                <div id="menu1" class="tab-pane fade">
                                    <div style="max-width: 1500px;padding-top: 10px;">
                                        <dx:ASPxGridView ID="gvPayment" runat="server" ShowSelectCheckbox="True" AutoGenerateColumns="False" Width="100%" KeyFieldName="MerchantID" ClientInstanceName="gvPayment"
                                            ClientVisible="True">
                                            <SettingsLoadingPanel Mode="Disabled" />
                                            <SettingsBehavior EnableRowHotTrack="True" AutoExpandAllGroups="true" ColumnResizeMode="NextColumn" AllowSelectByRowClick="false" AllowFocusedRow="true" />
                                            <Columns>

                                                <dx:GridViewDataTextColumn FieldName="MerchantID" Caption="MerchantID" VisibleIndex="0" Width="100px" Visible="false"/>
                                                <dx:GridViewDataTextColumn FieldName="RecordLocator" Caption="PNR" VisibleIndex="0" Width="200px" />
                                                <dx:GridViewDataTextColumn FieldName="TenderDesc" Caption="Payment Method" VisibleIndex="1" Width="200px" />
                                                <dx:GridViewDataTextColumn FieldName="TenderAmt" Caption="Amount" VisibleIndex="2" Width="150px" CellStyle-HorizontalAlign="Right">
                                                    <PropertiesTextEdit DisplayFormatString="#,#.00"/>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="TransDate" Caption="Transaction Date" VisibleIndex="3" Width="200px" />
                                                <dx:GridViewDataTextColumn FieldName="RefNo" Caption="Reference No." VisibleIndex="4" Width="150px" />
                                                <dx:GridViewDataTextColumn FieldName="TransVoid" Caption="Status" VisibleIndex="5" Width="150px" />

                                            </Columns>
                                            <SettingsPager Mode="ShowAllRecords" />
                                            <Styles Header-Wrap="True" Header-HorizontalAlign="Center" Header-VerticalAlign="Middle">
                                                <Row Cursor="pointer" />
                                            </Styles>
                                        </dx:ASPxGridView>
                                    </div>
                                </div>
                                <div id="menu2" class="tab-pane fade">
                                    <div style="max-width: 1500px;padding-top: 10px;">
                                        <dx:ASPxGridView ID="gvPassenger" runat="server" ShowSelectCheckbox="True" AutoGenerateColumns="False" KeyFieldName="RecordLocator;PassengerID" Width="100%" ClientInstanceName="gvPassenger"
                                            ClientVisible="True" EnableRowsCache="false" EnableCallBacks="true"
                                            >
                                            <SettingsLoadingPanel Mode="Disabled" />
                                            <SettingsBehavior EnableRowHotTrack="True" AutoExpandAllGroups="true" AllowGroup="true" ColumnResizeMode="NextColumn" AllowSelectByRowClick="false" AllowFocusedRow="true" />
                                            <Columns>
                                                <dx:GridViewDataColumn FieldName="RecordLocator" GroupIndex="0" />
                                                <dx:GridViewDataTextColumn FieldName="IssuingCountry" Caption="Issuing Country"/>
                                                <dx:GridViewDataTextColumn FieldName="Nationality" Caption="Nationality"/>
                                                <dx:GridViewDataTextColumn FieldName="Title"/>
                                                <dx:GridViewDataTextColumn FieldName="Gender" Caption="Gender"/>
                                                <dx:GridViewDataTextColumn FieldName="FirstName" Caption="First Name"/>
                                                <dx:GridViewDataTextColumn FieldName="LastName" Caption="Last Name"  />
                                                <dx:GridViewDataDateColumn FieldName="DOB"/>
                                                <dx:GridViewDataTextColumn FieldName="PassportNo" Caption="Passport No."/>
                                                <dx:GridViewDataTextColumn FieldName="ExpiryDate" Caption="Expired Date" PropertiesTextEdit-DisplayFormatString="dd/MM/yyyy" />

                                            </Columns>
                                            <SettingsPager Mode="ShowPager" PageSize="60" Position="Top" />
                                            <Styles Header-Wrap="True" Header-HorizontalAlign="Center" Header-VerticalAlign="Middle">
                                                <Row Cursor="pointer" />
                                            </Styles>
                                        </dx:ASPxGridView>
                                    </div>
                                </div>
                                <div id="menu3" class="tab-pane fade">
                                    <div style="max-width: 1500px;padding-top: 10px;">
                                        <dx:ASPxGridView ID="gvAddOn" runat="server" ShowSelectCheckbox="True" AutoGenerateColumns="False" KeyFieldName="RecordLocator;PassengerID" Width="100%" ClientInstanceName="gvAddOn"
                                            ClientVisible="True" EnableRowsCache="false" EnableCallBacks="true">
                                            <SettingsLoadingPanel Mode="Disabled" />
                                            <SettingsBehavior EnableRowHotTrack="True" AutoExpandAllGroups="true" AllowGroup="true" ColumnResizeMode="NextColumn" AllowSelectByRowClick="false" AllowFocusedRow="true" />
                                            <Columns>
                                                <dx:GridViewDataColumn FieldName="RecordLocator" GroupIndex="0" />
                                                <dx:GridViewDataTextColumn FieldName="FirstName" Caption="First Name" ReadOnly="true"/>
                                                <dx:GridViewDataTextColumn FieldName="LastName" Caption="Last Name" ReadOnly="true" />
                                                <dx:GridViewDataTextColumn FieldName="Meal" Caption="Meal" Width="120px"/>
                                                <dx:GridViewDataTextColumn FieldName="Baggage" Caption="Baggage" Width="120px"/>
                                                <dx:GridViewDataTextColumn FieldName="SportEquipment" Caption="Sport Equipment" Width="150px"/>
                                                <dx:GridViewDataTextColumn FieldName="ComfortKit" Caption="Comfort Kit" Width="120px"/>
                                                <dx:GridViewDataTextColumn FieldName="DutyFree" Caption="Duty Free" Width="120px"/>
                                            </Columns>
                                            <SettingsPager Mode="ShowPager" PageSize="60" Position="Top" />
                                            <Styles Header-Wrap="True" Header-HorizontalAlign="Center" Header-VerticalAlign="Middle">
                                                <Row Cursor="pointer" />
                                            </Styles>
                                        </dx:ASPxGridView>
                                    </div>
                                </div>
                            </div>--%>
            </div>
        </div>
    </div>
</div>

<dx:ASPxImage runat="server" ID="imgStatus" ClientInstanceName="imgStatus" Width="0" Height="0"></dx:ASPxImage>
<dx:ASPxImage runat="server" ID="imgMessage" ClientInstanceName="imgMessage" Width="0" Height="0"></dx:ASPxImage>

<%--end of new layout--%>

<div id="wrapper3" style="display: none">
    <div>
        &nbsp;<dx:ASPxLabel ID="lblHeader" Text="Booking Detail" runat="server" Font-Bold="True" Font-Size="Medium"></dx:ASPxLabel>
    </div>
    <hr />
    <table width="100%">
        <tr>
            <td style="width: 135px; padding-bottom: 3px;">&nbsp;<dx:ASPxLabel ID="lblBookingText" runat="server" Text="Transaction No" Font-Size="Small"></dx:ASPxLabel>
            </td>
            <td style="width: 10px; padding-bottom: 3px;">: </td>
            <td style="width: 332px; padding-bottom: 3px;">
                <dx:ASPxLabel ID="lblBookingNum" runat="server" Font-Size="Small"></dx:ASPxLabel>
            </td>
            <td style="height: 14px; width: 257px; padding-bottom: 3px;">&nbsp;<dx:ASPxLabel ID="lblAmountText" runat="server" Text="Amount Due for Payment" Font-Size="Small"></dx:ASPxLabel>
            </td>
            <td style="width: 10px; padding-bottom: 3px;">: </td>
            <td style="height: 14px; width: 400px; padding-bottom: 3px;"><%--<dx:ASPxLabel ID="lblDueAmount" runat="server"  Font-Size="Small"></dx:ASPxLabel>--%></td>
            <td></td>
        </tr>
        <tr>
            <td style="width: 135px; padding-bottom: 3px;">&nbsp;<dx:ASPxLabel ID="lblBookingDateText" runat="server" Text="Booking Date" Font-Size="Small"></dx:ASPxLabel>
            </td>
            <td style="width: 10px; padding-bottom: 3px;">: </td>
            <td style="width: 332px; padding-bottom: 3px;">
                <dx:ASPxLabel ID="lblBookingDate" runat="server" Font-Size="Small"></dx:ASPxLabel>
            </td>
            <td style="width: 257px; padding-bottom: 3px;">&nbsp;<dx:ASPxLabel ID="lblExpiryDateText" runat="server" Text="Expiry Date" Font-Size="Small"></dx:ASPxLabel>
            </td>
            <td style="width: 10px; padding-bottom: 3px;">: </td>
            <td style="width: 180px; padding-bottom: 3px;">
                <dx:ASPxLabel ID="lblExpiryDate" runat="server" Font-Size="Small"></dx:ASPxLabel>
            </td>
            <td></td>
        </tr>
        <tr>
            <td style="width: 135px; padding-bottom: 3px;">&nbsp;<dx:ASPxLabel ID="lblPaxText" runat="server" Text="No. of Pax" Font-Size="Small"></dx:ASPxLabel>
            </td>
            <td style="width: 10px; padding-bottom: 3px;">: </td>
            <td style="width: 332px; padding-bottom: 3px;"><%--<dx:ASPxLabel ID="lblPax" runat="server"  Font-Size="Small"></dx:ASPxLabel>--%></td>
            <td style="width: 257px; padding-bottom: 3px;">&nbsp;<dx:ASPxLabel ID="lblStatusText" runat="server" Text="Status" Font-Size="Small"></dx:ASPxLabel>
            </td>
            <td style="width: 10px; padding-bottom: 3px;">: </td>
            <td style="width: 180px; padding-bottom: 3px;">
                <dx:ASPxLabel ID="lblStatus" runat="server" Font-Size="Small" Font-Bold="true"></dx:ASPxLabel>
            </td>
            <td></td>
        </tr>
        <tr>
            <td style="width: 135px; padding-bottom: 3px;">&nbsp;<dx:ASPxLabel ID="lblRemarks" runat="server" Text="Remarks" Font-Size="Small"></dx:ASPxLabel>
            </td>
            <td style="width: 10px; padding-bottom: 3px;">: </td>
            <td style="width: 332px; padding-bottom: 3px;">
                <dx:ASPxLabel ID="lblReason" runat="server" Font-Size="Small"></dx:ASPxLabel>
            </td>
            <td colspan="2">&nbsp;<dx:ASPxLabel ID="lblNote" runat="server" Text="Note: payment date have expired, kindly contact us to do payment extension" Visible="false" Font-Size="Small"></dx:ASPxLabel>
            </td>
        </tr>
    </table>
    <div style="position: absolute; top: 331px; left: 266px;">
        &nbsp;<font color="red"><dx:ASPxLabel ID="lblMsg" runat="server" ></dx:ASPxLabel></font>
    </div>

    <div align="right">
        <br />
        <%--  <table>
                    <tr>
                    
    <td><dx:ASPxButton CssClass="buttonL2" ID="btPayFull" runat="server"  
        Text="Need Payment" AutoPostBack="False" onclick="btPayFull_Click" >
        <ClientSideEvents Click="function(s, e) {  LoadingPanel.SetText(&#39;Please Wait...&#39;);
        LoadingPanel.Show(); }" />                                            
    </dx:ASPxButton></td><td>&nbsp;</td>
    <td><dx:ASPxButton CssClass="buttonL2" ID="btPassenger" runat="server"  
        Text="Upload Passengers" AutoPostBack="False" onclick="btPassenger_Click" >
        <ClientSideEvents Click="function(s, e) {  LoadingPanel.SetText(&#39;Please Wait...&#39;);
        LoadingPanel.Show(); }" />                                            
    </dx:ASPxButton></td><td>&nbsp;</td>
    <td><dx:ASPxButton CssClass="buttonL2" ID="btCancel" runat="server"
        Text="Cancel Transaction" AutoPostBack="False" >
        <ClientSideEvents Click="ShowPopup" />
    </dx:ASPxButton></td>
    <td style="display:none"><dx:ASPxButton CssClass="buttonL2" ID="btGetLatest" runat="server"  
        Text="Upload Passengers" AutoPostBack="False" onclick="btGetLatest_Click" >
        <ClientSideEvents Click="function(s, e) {  LoadingPanel.SetText(&#39;Please Wait...&#39;);
        LoadingPanel.Show(); }" />                                            
    </dx:ASPxButton></td><td>&nbsp;</td>
    <td>&nbsp;</td>
                    </tr>
                    </table>--%>
    </div>
    <div class="redSectionHeader">
        <div>Flight Details</div>
    </div>
    <table width="100%" border="1" bgcolor="#f7f3f7">
        <tr class="tdcol">
            <td colspan="3"></td>
        </tr>
        <font size="8">
            <asp:Literal ID="ltrFlight" runat="server"></asp:Literal>
        </font>
    </table>
    <br />
    <div class="redSectionHeader">
        <div>Fare Details</div>
    </div>
    <table width="100%" border="2" bgcolor="#f7f3f7">
        <tr bgcolor="#f7f3f7" align="left">
            <td>
                <%--                                       <dx:ASPxGridView ID="gvClass" ClientInstanceName="gridClass" runat="server" 
                                            KeyFieldName="TransID" Width="100%"  OnCustomButtonCallback="gvClass_CustomButtonCallback" OnDataBinding="gridCustomers_CustomColumnSort" 
                                                    AutoGenerateColumns="False" 
                                            SettingsBehavior-ColumnResizeMode="Control" 
                                            OnCustomCallback="gvClass_Callback" >
                                            <ClientSideEvents EndCallback="OnCallbackComplete" BeginCallback="OnCallbackBegin" />
                                            <Columns>
                                                <dx:GridViewCommandColumn VisibleIndex="0" Caption="Choose" Width="80px" ButtonType="Image" Visible="false" >                 
                                                 <CustomButtons>
                                                     <dx:GridViewCommandColumnCustomButton Text="Change" ID="changeBtnFinish" 
                                                     Image-AlternateText="Change Flight" Image-ToolTip="Change Flight" 
                                                     Image-Width="16px" Image-Url="../Images/AKBase/plane.png" />
                                                     
                                                 </CustomButtons> 
                                                 </dx:GridViewCommandColumn>
                                               <dx:GridViewDataColumn FieldName="Journey" VisibleIndex="1" GroupIndex="0" > 

                                               </dx:GridViewDataColumn>                                                                   
                                                <dx:GridViewDataColumn FieldName="RecordLocator" Caption="Booking No." VisibleIndex="2" />                                                
                                                <dx:GridViewDataColumn FieldName="FlightNo" Caption="Flight" VisibleIndex="3" 
                                                    Width="60px" />
                                                <dx:GridViewDataColumn FieldName="SeqNo" Caption="SeqNo" VisibleIndex="3" 
                                                    Width="60px" Visible="false"/>
                                                <dx:GridViewDataColumn FieldName="FareClass" Caption="Class" VisibleIndex="4" 
                                                    Width="45px" Visible="false" />
                                                <dx:GridViewDataColumn FieldName="TotalPax" Caption="Pax" 
                                                    VisibleIndex="5" Width="60px" />                                                 
                                                <dx:GridViewDataSpinEditColumn  FieldName="LineFlight" Caption="Flight Fare" VisibleIndex="7">
                                                <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                    
                                                </dx:GridViewDataSpinEditColumn> 
                                                <dx:GridViewDataSpinEditColumn FieldName="LineTax" Caption="Taxes & Fees" VisibleIndex="8" >              
                                                <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                                                      
                                                </dx:GridViewDataSpinEditColumn>
                                                <dx:GridViewDataSpinEditColumn FieldName="LineTotal" Caption="Total Booking" VisibleIndex="9" >
                                                <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                    
                                                </dx:GridViewDataSpinEditColumn> 
                                                <dx:GridViewDataColumn FieldName="Currency" Caption=" " 
                                                    VisibleIndex="10" Width="65px" />
                                            </Columns>
                                          <Settings ShowGroupFooter="VisibleIfExpanded" />
                                            <GroupSummary>
                                                <dx:ASPxSummaryItem FieldName="LineTotal" SummaryType="Sum" DisplayFormat="{0:n2}" ShowInGroupFooterColumn="LineTotal"/>
                                                <dx:ASPxSummaryItem FieldName="LineTax" SummaryType="Sum" DisplayFormat="{0:n2}" ShowInGroupFooterColumn="LineTax"/>
                                               <dx:ASPxSummaryItem FieldName="LineFlight" SummaryType="Sum" DisplayFormat="{0:n2}" ShowInGroupFooterColumn="LineFlight"/>
                                                <dx:ASPxSummaryItem FieldName="TotalPax" SummaryType="Sum" DisplayFormat="{0}" ShowInGroupFooterColumn="TotalPax"/>
                                               <dx:ASPxSummaryItem FieldName="SeqNo" SummaryType="Max" DisplayFormat="{0}" ValueDisplayFormat="" ShowInGroupFooterColumn="SeqNo" />
                                            </GroupSummary>
                                          <SettingsBehavior ColumnResizeMode="Control"></SettingsBehavior>
                                           <ClientSideEvents SelectionChanged="gvClass_SelectionChanged" />
                                           <styles>
                                                <header backcolor="#333333" font-size="10pt" forecolor="White">
                                                </header>
                                            </styles>
                                        </dx:ASPxGridView>--%>
            </td>
        </tr>
    </table>
    <div id="divUnConfirmed" runat="server" style="display: none">
        <div class="redSectionHeader">
            <div>Rejected Fare Details</div>
        </div>
        <table width="100%" border="2" bgcolor="#f7f3f7">
            <tr bgcolor="#f7f3f7" align="left">
                <td>
                    <%--                                        <dx:ASPxGridView ID="gvRejectedClass" ClientInstanceName="gridClass2" runat="server" 
                                            KeyFieldName="TransID" Width="100%" OnDataBinding="gvRejectedClass_CustomColumnSort" 
                                                    AutoGenerateColumns="False" 
                                            SettingsBehavior-ColumnResizeMode="Control" 
                                            OnCustomCallback="gvRejectedClass_Callback" >
                                            <Columns>    
                                               <dx:GridViewDataColumn FieldName="Journey" VisibleIndex="0" GroupIndex="0" > 

                                               </dx:GridViewDataColumn>                                                                   
                                                <dx:GridViewDataColumn FieldName="RecordLocator" Caption="Booking No." VisibleIndex="1" />                                                
                                                <dx:GridViewDataColumn FieldName="FlightNo" Caption="Flight" VisibleIndex="3" 
                                                    Width="60px" />
                                                <dx:GridViewDataColumn FieldName="SeqNo" Caption="SeqNo" VisibleIndex="3" 
                                                    Width="60px" Visible="false"/>
                                                <dx:GridViewDataColumn FieldName="FareClass" Caption="Class" VisibleIndex="4" 
                                                    Width="45px" Visible="false" />
                                                <dx:GridViewDataColumn FieldName="TotalPax" Caption="Pax" 
                                                    VisibleIndex="5" Width="60px" />    
                                                                                                      
                                                <dx:GridViewDataSpinEditColumn  FieldName="LineFlight" Caption="Flight Fare" VisibleIndex="7">
                                                <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                    
                                                </dx:GridViewDataSpinEditColumn> 
                                                <dx:GridViewDataSpinEditColumn FieldName="LineTax" Caption="Taxes & Fees" VisibleIndex="8" >              
                                                <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                                                      
                                                </dx:GridViewDataSpinEditColumn>
                                                <dx:GridViewDataSpinEditColumn FieldName="LineTotal" Caption="Total Booking" VisibleIndex="9" >
                                                <PropertiesSpinEdit DisplayFormatString="n2"></PropertiesSpinEdit>                                    
                                                </dx:GridViewDataSpinEditColumn> 
                                                <dx:GridViewDataColumn FieldName="Currency" Caption=" " 
                                                    VisibleIndex="10" Width="65px" />
                                            </Columns>
                                          <Settings ShowGroupFooter="VisibleIfExpanded" />
                                            <GroupSummary>
                                                <dx:ASPxSummaryItem FieldName="LineTotal" SummaryType="Sum" DisplayFormat="{0:n2}" ShowInGroupFooterColumn="LineTotal"/>
                                                <dx:ASPxSummaryItem FieldName="LineTax" SummaryType="Sum" DisplayFormat="{0:n2}" ShowInGroupFooterColumn="LineTax"/>
                                                <dx:ASPxSummaryItem FieldName="LineFlight" SummaryType="Sum" DisplayFormat="{0:n2}" ShowInGroupFooterColumn="LineFlight"/>
                                                <dx:ASPxSummaryItem FieldName="TotalPax" SummaryType="Sum" DisplayFormat="{0}" ShowInGroupFooterColumn="TotalPax"/>
                                               <dx:ASPxSummaryItem FieldName="SeqNo" SummaryType="Max" DisplayFormat="{0}" ValueDisplayFormat="" ShowInGroupFooterColumn="SeqNo" />
                                            </GroupSummary>
                                          <SettingsBehavior ColumnResizeMode="Control"></SettingsBehavior>
                                           <ClientSideEvents SelectionChanged="gvRejectedClass_SelectionChanged" />
                                           <styles>
                                                <header backcolor="#333333" font-size="10pt" forecolor="White">
                                                </header>
                                            </styles>
                                        </dx:ASPxGridView>--%>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <br />
    <table width="100%" style="display: none">
        <tr style="vertical-align: middle; height: 30px; background-color: #333333; color: White; font-size: small;">
            <td>&nbsp;Summary of Charges</td>
        </tr>
    </table>
    <table class="priceDisplay" width="95%" style="background-color: #EEEEEE; display: none">
        <tbody>
            <asp:Literal ID="ltrSummary" runat="server"></asp:Literal>
        </tbody>
    </table>
    <br />
    <%--                                <div runat="server" id="divPayment" visible="false">
                             <table width="100%">
                 <tr  style="vertical-align:middle;height:30px;background-color:#333333;color:White;font-size:small;">
                  <td>&nbsp;Payment History</td>
                </tr></table>
                                
                                <div class="itinhr top"></div>
                                  <table style="color: #123cda;font-weight: bold;font-size: 1em;font-family:Arial,Helvetica,san-serif;">
                                  <tbody>
                                     <tr><td width='100px'><b>Booking No</b></td><td width='150px'><b>Payment Method</b></td><td width='200px'><b>Amount</b></td><td width='200px'><b>Transaction Date</b></td><td width='150px'><b>Reference No</b></td><td width='100px'><b>Status</b></td></tr>

                                     <asp:Literal ID="ltrPayment" runat="server"></asp:Literal>         
                                  </tbody>
                                  </table>                                  
                                  <div class="itinhr bottom"></div>
                                  <table class="paymentDisplay">
                                  
                                  </table>
                                </div>
    --%>
    <table>
        <tr>
            <td></td>
        </tr>
    </table>

</div>
