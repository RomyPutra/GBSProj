<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reviewbooking.aspx.cs"
    Inherits="GroupBooking.Web.Booking.ItineraryDetail" MasterPageFile="~/Master/NewPageMaster.Master" %>

<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %>
<%@ Register Src="../UserControl/flightbookingdetail.ascx" TagName="ucflightbookingdetail" TagPrefix="FBD" %>
<%--<%@ Register Src="../UserControl/DetailBreakDownAllPax.ascx" TagName="DetailBreakdown" TagPrefix="db" %>--%>
<%@ Register Src="../UserControl/PaxDetail.ascx" TagName="ucPaxDetail" TagPrefix="PD" %>
<%@ Register Src="../UserControl/flighdetail.ascx" TagName="flightdetail" TagPrefix="fd" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolder2">
    <script type="text/javascript">
        // <![CDATA[

        function OnCallbackComplete(s, e) {
            if (e.result != "") {
                //lblmsg.SetValue(e.result);
                document.getElementById("ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = e.result;
                pcMessage.Show();
                LoadingPanel.Hide();
            } else {

                window.location.href = '../public/proceedpayment.aspx';
            }
        }

        function ShowLoginWindow() {
            pcMessage.Show();
        }

        function ShowAddOnWindow() {
            LoadingPanel.Hide();
            pcAddOnBreakdown.Show();
        }
        // ]]> 
    </script>

    <msg:msgControl ID="msgcontrol" runat="server" />
    <div class="page-header row">
        <div class="col-sm-4">
            <h4 class="mt-0 mb-5">Booking Summary</h4>
            Booking/Booking Summary
        </div>
        <div class="col-sm-8">
            <table border="0" cellpadding="0" cellspacing="0" style="float: right;">
                <tr>
                    <%-- 20170425 - Sienny (add button show addon breakdown) --%>
                    <td>
                        <dx:ASPxButton CssClass="buttonL backBtn" ID="btnAddOnBreakdown" runat="server" Visible="true" Text="Show Add On Breakdown" AutoPostBack="False">
                            <ClientSideEvents Click="function(s, e) { Callback.PerformCallback(); ShowAddOnWindow(); }" />
                        </dx:ASPxButton>
                    </td>

                    <td>
                        <dx:ASPxButton CssClass="buttonL backBtn" ID="btn_back" runat="server" Visible="false" Text="Back" AutoPostBack="False">
                            <ClientSideEvents Click="function(s, e) {CallBackValidation.PerformCallback('back');
                                LoadingPanel.SetText(&#39;Please Wait...&#39;); LoadingPanel.Show(); }" />
                        </dx:ASPxButton>
                    </td>
                    <td>
                        <dx:ASPxButton CssClass="buttonL" ID="btn_Next" runat="server" Text="Continue" AutoPostBack="False">
                            <ClientSideEvents Click="function(s, e) {CallBackValidation.PerformCallback();
                                LoadingPanel.SetText(&#39;Please Wait...&#39;); LoadingPanel.Show(); }" />
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </div>

        <dx:ASPxPopupControl ID="pcAddOnBreakdown" runat="server" CloseOnEscape="true" CloseAction="CloseButton" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="750px" Height="500px" ScrollBars="Auto"
            ClientInstanceName="pcAddOnBreakdown" CloseButtonImage-Url="~/Images/AirAsia/close_button_icon.jpg" AllowDragging="True" EnableAnimation="False" EnableViewState="False" HeaderText="" ShowHeader="False">
            <HeaderStyle ForeColor="#CC0000" />
            <HeaderTemplate>
            </HeaderTemplate>
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControlAddOnBreakDown" runat="server">
                    <div style="float: right">
                        <dx:ASPxImage ID="imgClose" runat="server" ImageUrl="~/Images/AirAsia/close_button_icon.jpg" Cursor="pointer" Visible="true">
                            <ClientSideEvents Click="function(s, e){ pcAddOnBreakdown.Hide(); }" />
                        </dx:ASPxImage>
                    </div>
                    <dx:ASPxPanel ID="pnAddOnBreakdown" runat="server" DefaultButton="btOK" Paddings-PaddingBottom="10px">
                        <PanelCollection>
                            <dx:PanelContent ID="panconAddOnBreakdown" runat="server">
                                <dx:ASPxPageControl ID="apcAddOnBreakdown" runat="server" Width="100%" Height="100%">
                                    <TabPages>
                                        <dx:TabPage Text="Going" Name="TabGoing" Visible="true">
                                            <ContentCollection>
                                                <dx:ContentControl ID="ContentControlTabGoing" runat="server">
                                                    <dx:ASPxGridView ID="gvAddOnDepart" runat="server" ShowSelectCheckbox="True" AutoGenerateColumns="False" KeyFieldName="SeqNo;PassengerID" Width="100%" ClientInstanceName="gvAddOnDepart"
                                                        ClientVisible="True" EnableRowsCache="false" EnableCallBacks="true">
                                                        <SettingsLoadingPanel Mode="Disabled" />
                                                        <SettingsBehavior EnableRowHotTrack="True" AutoExpandAllGroups="true" AllowGroup="true" ColumnResizeMode="NextColumn" AllowSelectByRowClick="false" AllowFocusedRow="true" />
                                                        <Columns>
                                                            <%--<dx:GridViewDataColumn FieldName="PNR" Caption="PNR" ReadOnly="true" Width="150px" />--%>
                                                            <dx:GridViewDataTextColumn FieldName="PassengerID" Caption="PassengerID" ReadOnly="true" Visible="false" />
                                                            <dx:GridViewDataTextColumn FieldName="FlightNo" Caption="FlightNo" ReadOnly="true" Visible="false" />
                                                            <dx:GridViewDataTextColumn FieldName="FirstName" Caption="First Name" ReadOnly="true" Width="100px" />
                                                            <dx:GridViewDataTextColumn FieldName="LastName" Caption="Last Name" ReadOnly="true" Width="100px" />
                                                            <dx:GridViewDataTextColumn FieldName="PaxType" Caption="Pax Type" ReadOnly="true" Width="100px" />
                                                            <dx:GridViewDataTextColumn FieldName="DepartMeal" Caption="Depart Meal" Width="250px" />
                                                            <dx:GridViewDataTextColumn FieldName="ConDepartMeal" Caption="Depart Connecting Meal" Width="250px" />
                                                            <dx:GridViewDataTextColumn FieldName="DepartBaggage" Caption="Depart Baggage" Width="250px" />
                                                            <dx:GridViewDataTextColumn FieldName="DepartSport" Caption="Depart Sport Equipment" Width="250px" />
                                                            <dx:GridViewDataTextColumn FieldName="DepartComfort" Caption="Depart Comfort Kit" Width="250px" />
                                                             <dx:GridViewDataTextColumn FieldName="DepartInfant" Caption="Depart Infant" Width="250px" />
                                                            <%--<dx:GridViewDataTextColumn FieldName="ReturnMeal" Caption="Return Meal" Width="250px"/>--%>
                                                            <%--<dx:GridViewDataTextColumn FieldName="ConReturnMeal" Caption="Return Connecting Meal" Width="250px"/>--%>
                                                            <%--<dx:GridViewDataTextColumn FieldName="ReturnBaggage" Caption="Return Baggage" Width="250px"/>--%>
                                                            <%--<dx:GridViewDataTextColumn FieldName="ReturnSport" Caption="Return Sport Equipment" Width="250px"/>--%>
                                                            <%--<dx:GridViewDataTextColumn FieldName="ReturnComfort" Caption="Return Comfort Kit" Width="250px"/>--%>
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
                                        <dx:TabPage Text="Return" Name="TabReturn" Visible="true">
                                            <ContentCollection>
                                                <dx:ContentControl ID="ContentControlTabReturn" runat="server">
                                                    <dx:ASPxGridView ID="gvAddOnReturn" runat="server" ShowSelectCheckbox="True" AutoGenerateColumns="False" KeyFieldName="SeqNo;PassengerID" Width="100%" ClientInstanceName="gvAddOnReturn"
                                                        ClientVisible="True" EnableRowsCache="false" EnableCallBacks="true">
                                                        <SettingsLoadingPanel Mode="Disabled" />
                                                        <SettingsBehavior EnableRowHotTrack="True" AutoExpandAllGroups="true" AllowGroup="true" ColumnResizeMode="NextColumn" AllowSelectByRowClick="false" AllowFocusedRow="true" />
                                                        <Columns>
                                                            <%--<dx:GridViewDataColumn FieldName="PNR" Caption="PNR" ReadOnly="true" Width="150px" />--%>
                                                            <dx:GridViewDataTextColumn FieldName="PassengerID" Caption="PassengerID" ReadOnly="true" Visible="false" />
                                                            <dx:GridViewDataTextColumn FieldName="FlightNo" Caption="FlightNo" ReadOnly="true" Visible="false" />
                                                            <dx:GridViewDataTextColumn FieldName="FirstName" Caption="First Name" ReadOnly="true" Width="100px" />
                                                            <dx:GridViewDataTextColumn FieldName="LastName" Caption="Last Name" ReadOnly="true" Width="100px" />
                                                            <dx:GridViewDataTextColumn FieldName="PaxType" Caption="Pax Type" ReadOnly="true" Width="100px" />
                                                            <%--<dx:GridViewDataTextColumn FieldName="DepartMeal" Caption="Depart Meal" Width="250px"/>--%>
                                                            <%--<dx:GridViewDataTextColumn FieldName="ConDepartMeal" Caption="Depart Connecting Meal" Width="250px"/>--%>
                                                            <%--<dx:GridViewDataTextColumn FieldName="DepartBaggage" Caption="Depart Baggage" Width="250px"/>--%>
                                                            <%--<dx:GridViewDataTextColumn FieldName="DepartSport" Caption="Depart Sport Equipment" Width="250px"/>--%>
                                                            <%--<dx:GridViewDataTextColumn FieldName="DepartComfort" Caption="Depart Comfort Kit" Width="250px"/>--%>
                                                            <dx:GridViewDataTextColumn FieldName="ReturnMeal" Caption="Return Meal" Width="250px" />
                                                            <dx:GridViewDataTextColumn FieldName="ConReturnMeal" Caption="Return Connecting Meal" Width="250px" />
                                                            <dx:GridViewDataTextColumn FieldName="ReturnBaggage" Caption="Return Baggage" Width="250px" />
                                                            <dx:GridViewDataTextColumn FieldName="ReturnSport" Caption="Return Sport Equipment" Width="250px" />
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
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxPanel>
                </dx:PopupControlContentControl>
            </ContentCollection>
            <CloseButtonImage Url="~/Images/AirAsia/close_button_icon.jpg"></CloseButtonImage>
            <ContentStyle>
                <Paddings PaddingBottom="5px" />
                <Paddings PaddingBottom="5px"></Paddings>
            </ContentStyle>
        </dx:ASPxPopupControl>
    </div>
    <div class="page-content">
        <div class="col-sm-12">
            <div id="fareInformation" class="col-sm-10">
                <div style="padding-top: 10px;">

                    <div id="infoTable" class="tableWrapper specialArrange" style="margin-top: 5px;">
                        <table id="reviewTable" style="width: 100%;" class="table table-bordered">
                            <tbody>
                                <tr>
                                    <td>
                                        <table class="reviewLeft">
                                            <tbody>
                                                <tr>

                                                    <td colspan="2" style="text-align: center"><span>Total Average Fare:</span></td>
                                                    <td><span id="totalAverage" style="font-weight: 700; padding: 0 10px;">
                                                        <asp:Label ID="lbl_TotalAmount" runat="server"></asp:Label>
                                                        <asp:Label ID="lbl_TotalCurrency" runat="server"></asp:Label>
                                                    </span></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="text-align: center"><span>Average Fare:</span></td>
                                                    <td><span id="averageFareAmount" style="padding: 0 10px;">
                                                        <asp:Label ID="lblAverageFare" runat="server"></asp:Label>
                                                        <asp:Label ID="lblAverageCurrency" runat="server"></asp:Label>
                                                    </span></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                    <td>
                                        <table class="reviewRight">
                                            <tbody>
                                                <tr>
                                                    <td colspan="2" style="text-align: center"><span>Total Pax:</span></td>
                                                    <td>
                                                        <asp:Label ID="lbl_num" runat="server"></asp:Label></td>
                                                </tr>
                                                <tr id="trInfantTotal" runat="server">
                                                    <td colspan="2" style="text-align: center"><span>Total Infant:</span></td>
                                                    <td>
                                                        <dx:ASPxLabel ID="lblTotInfant" runat="server"></dx:ASPxLabel>
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

                <div id="flightDetailBookingInfoNew" class="" style="">
                    <div class="redSectionHeader noShow">
                        <div>Fare Details</div>
                    </div>
                    <table width="100%" bgcolor="#f7f3f7" class="table table-bordered">
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
                                                    <asp:Label ID="lbl_currency1" runat="server"></asp:Label>
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
                                    <tr id="trBaggageChargeDepart" runat="server">
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
                                    </tr>
                                    <tr id="trSeatChargeDepart" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label4" runat="server"></asp:Label>Seat Charge</span>
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
                                    <tr id="trACFChargeDepart" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label32" runat="server"></asp:Label>
                                                    <asp:Label ID="lblACFInfoDepart" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblACFTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrACFDepart" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trAPFChargeDepart" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label55" runat="server"></asp:Label>
                                                    <asp:Label ID="lblAPFInfoDepart" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblAPFTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrAPFDepart" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trAPFCChargeDepart" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label59" runat="server"></asp:Label>
                                                    <asp:Label ID="lblAPFCInfoDepart" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblAPFCTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrAPFCDepart" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr> 
                                    <tr id="trAPSChargeDepart" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label44" runat="server"></asp:Label>
                                                    <asp:Label ID="lblAPSInfoDepart" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblAPSTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrAPSDepart" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>                                  
                                    <tr id="trASCChargeDepart" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label46" runat="server"></asp:Label>
                                                    <asp:Label ID="lblASCInfoDepart" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblASCTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrASCDepart" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trAVLChargeDepart" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label17" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblAVLInfoDepart" runat="server"></asp:Label>
                                                    <asp:Label ID="Label18" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="Label19" runat="server" Visible="false"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblAVLTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrAVLDepart" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trBCLChargeDepart" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label48" runat="server"></asp:Label>
                                                    <asp:Label ID="lblBCLInfoDepart" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblBCLTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrBCLDepart" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trCSTChargeDepart" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label33" runat="server"></asp:Label>
                                                    <asp:Label ID="lblCSTInfoDepart" runat="server"></asp:Label></span>
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
                                                    <asp:Label ID="Label36" runat="server"></asp:Label>
                                                    <asp:Label ID="lblCUTInfoDepart" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblCUTTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrCUTDepart" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trGSTChargeDepart" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label14" runat="server"></asp:Label>
                                                    <asp:Label ID ="lblGSTInfoDepart" runat="server"></asp:Label>
                                                    <asp:Label ID="Label15" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="Label16" runat="server" Visible="false"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lbl_GSTTotal" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrGSTDepart" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trIADFChargeDepart" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label47" runat="server"></asp:Label>
                                                    <asp:Label ID="lblIADFInfoDepart" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblIADFTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrIADFDepart" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trIPSCChargeDepart" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label58" runat="server"></asp:Label>
                                                    <asp:Label ID="lblIPSCInfoDepart" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblIPSCTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrIPSCDepart" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trISFChargeDepart" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label65" runat="server"></asp:Label>
                                                    <asp:Label ID="lblISFInfoDepart" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblISFTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrISFDepart" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr> 
                                    <tr id="trIWJRChargeDepart" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label51" runat="server"></asp:Label>
                                                    <asp:Label ID="lblIWJRInfoDepart" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblIWJRTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrIWJRDepart" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>  
                                    <tr id="trKlia2FeeDepart" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label11" runat="server"></asp:Label>
                                                    <asp:Label ID="lblKLIA2InfoDepart" runat="server"></asp:Label> 
                                                    <asp:Label ID="Label12" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="Label13" runat="server" Visible="false"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lbl_klia2Total" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrKlia2Depart" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trPSCChargeDepart" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label60" runat="server"></asp:Label>
                                                    <asp:Label ID="lblPSCInfoDepart" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblPSCTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrPSCDepart" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trPSFChargeDepart" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="lbl_num15" runat="server"></asp:Label>
                                                    <asp:Label ID="lblPSFInfoDepart" runat="server"></asp:Label></span>
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
                                                    <asp:Label ID="lbl_num16" runat="server"></asp:Label>
                                                    <asp:Label ID="lblSCFInfoDepart" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblSCFTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrSCFDepart" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trSGIChargeDepart" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label37" runat="server"></asp:Label>
                                                    <asp:Label ID="lblSGIInfoDepart" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblSGITotalDepart" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrSGIDepart" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trSPLChargeDepart" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label34" runat="server"></asp:Label>
                                                    <asp:Label ID="lblSPLInfoDepart" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblSPLTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrSPLDepart" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trSSTChargeDepart" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label38" runat="server"></asp:Label>
                                                    <asp:Label ID="lblSSTInfoDepart" runat="server"></asp:Label></span>
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
                                                    <asp:Label ID="Label39" runat="server"></asp:Label>
                                                    <asp:Label ID="lblUDFInfoDepart" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblUDFTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrUDFDepart" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trVATChargeDepart" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label52" runat="server"></asp:Label>
                                                    <asp:Label ID="lblVATInfoDepart" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblVATChargeTotalDepart" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrVATChargeDepart" runat="server"></asp:Label></span>
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
                                    <tr id="trBagggageChargeReturn" runat="server">
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
                                    </tr>
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
                                                    <asp:Label ID="Label1" runat="server">Connecting Charge </asp:Label><asp:Label ID="Label2" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="Label3" runat="server" Visible="false"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblConnectingReturnTotal" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrConnectingReturn" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trDiscountChargeReturn" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label28" runat="server"></asp:Label>Discount Charge</span>
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
                                    <tr id="trACFChargeReturn" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label31" runat="server"></asp:Label>
                                                    <asp:Label ID="lblACFInfoReturn" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblACFTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrACFReturn" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trAPFChargeReturn" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label56" runat="server"></asp:Label>
                                                    <asp:Label ID="lblAPFInfoReturn" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblAPFTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrAPFReturn" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trAPFCChargeReturn" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label61" runat="server"></asp:Label>
                                                    <asp:Label ID="lblAPFCInfoReturn" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblAPFCTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrAPFCReturn" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trAPSChargeReturn" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label29" runat="server"></asp:Label>
                                                    <asp:Label ID="lblAPSInfoReturn" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblAPSTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrAPSReturn" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trASCChargeReturn" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label49" runat="server"></asp:Label>
                                                    <asp:Label ID="lblASCInfoReturn" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblASCTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrASCReturn" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trAVLChargeReturn" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label25" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblAVLInfoReturn" runat="server"></asp:Label> 
                                                    <asp:Label ID="Label26" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="Label27" runat="server" Visible="false"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblAVLTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrAVLReturn" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trBCLChargeReturn" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label50" runat="server"></asp:Label>
                                                    <asp:Label ID="lblBCLInfoReturn" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblBCLTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrBCLReturn" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trCSTChargeReturn" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label40" runat="server"></asp:Label>
                                                    <asp:Label ID="lblCSTInfoReturn" runat="server"></asp:Label></span>
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
                                                    <asp:Label ID="Label41" runat="server"></asp:Label>
                                                    <asp:Label ID="lblCUTInfoReturn" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblCUTTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrCUTReturn" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trGSTChargeReturn" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label22" runat="server"></asp:Label>
                                                    <asp:Label ID="lblGSTInfoReturn" runat="server"></asp:Label>
                                                    <asp:Label ID="Label23" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="Label24" runat="server" Visible="false"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lbl_InGSTTotal" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrGSTReturn" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trIADFChargeReturn" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label30" runat="server"></asp:Label>
                                                    <asp:Label ID="lblIADFInfoReturn" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblIADFTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrIADFReturn" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trIPSCChargeReturn" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label57" runat="server"></asp:Label>
                                                    <asp:Label ID="lblIPSCInfoReturn" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblIPSCTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrIPSCReturn" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trISFChargeReturn" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label63" runat="server"></asp:Label>
                                                    <asp:Label ID="lblISFInfoReturn" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblISFTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCUrrISFReturn" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trIWJRChargeReturn" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label53" runat="server"></asp:Label>
                                                    <asp:Label ID="lblIWJRInfoReturn" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblIWJRTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrIWJRReturn" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trKlia2FeeReturn" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label5" runat="server"></asp:Label>
                                                    <asp:Label ID="lblKLIA2InfoReturn" runat="server"></asp:Label>
                                                    <asp:Label ID="Label6" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="Label21" runat="server" Visible="false"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lbl_Inklia2Total" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrKlia2Return" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trPSCChargeReturn" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label62" runat="server"></asp:Label>
                                                    <asp:Label ID="lblPSCInfoReturn" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblPSCTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrPSCReturn" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trPSFChargeReturn" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="lbl_InNum15" runat="server"></asp:Label>
                                                    <asp:Label ID="lblPSFInfoReturn" runat="server"></asp:Label></span>
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
                                                    <asp:Label ID="lbl_InNum16" runat="server"></asp:Label>
                                                    <asp:Label ID="lblSCFInfoReturn" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblSCFTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrSCFReturn" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trSGIChargeReturn" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label42" runat="server"></asp:Label>
                                                    <asp:Label ID="lblSGIInfoReturn" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblSGITotalReturn" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrSGIReturn" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trSPLChargeReturn" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label35" runat="server"></asp:Label>
                                                    <asp:Label ID="lblSPLInfoReturn" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblSPLTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrSPLReturn" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trSSTChargeReturn" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label43" runat="server"></asp:Label>
                                                    <asp:Label ID="lblSSTInfoReturn" runat="server"></asp:Label></span>
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
                                                    <asp:Label ID="Label45" runat="server"></asp:Label>
                                                    <asp:Label ID="lblUDFInfoReturn" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblUDFTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrUDFReturn" runat="server"></asp:Label></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trVATChargeReturn" runat="server">
                                        <td>
                                            <div class="infoFlight">
                                                <span class="infoFlightSpan">
                                                    <asp:Label ID="Label54" runat="server"></asp:Label>
                                                    <asp:Label ID="lblVATInfoReturn" runat="server"></asp:Label></span>
                                                <span class="infoFlightSpan algnRight">
                                                    <asp:Label ID="lblVATChargeTotalReturn" runat="server" Text="0.00"></asp:Label>
                                                    <asp:Label ID="lblCurrVATChargeReturn" runat="server"></asp:Label></span>
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

                    <%--                    <div id="flightDetailBreakdown" class="" style="">
                        <db:DetailBreakdown ID="Detailbreakdown" runat="server" />

                    </div>--%>
                </div>
            </div>
            <dx:ASPxImage runat="server" ID="imgStatus" ClientInstanceName="imgStatus" Width="0" Height="0"></dx:ASPxImage>
            <dx:ASPxImage runat="server" ID="imgMessage" ClientInstanceName="imgMessage" Width="0" Height="0"></dx:ASPxImage>

        </div>

        <div>
            <dx:ASPxCallback ID="CallBackValidation" ClientInstanceName="CallBackValidation" runat="server" OnCallback="ValidatePopup">
                <ClientSideEvents CallbackComplete="OnCallbackComplete" />
            </dx:ASPxCallback>
        </div>
    </div>

    <div class="page-header row">
        <div class="col-sm-4">
        </div>
        <div class="col-sm-8">
            <table border="0" cellpadding="0" cellspacing="0" style="float: right;">
                <tr>
                    <td>
                        <dx:ASPxButton CssClass="buttonL backBtn" ID="btnBackBottom" runat="server" Visible="false"
                            Text="Back" AutoPostBack="False">
                            <ClientSideEvents Click="function(s, e) {CallBackValidation.PerformCallback('back');
                        LoadingPanel.SetText(&#39;Please Wait...&#39;);
                        LoadingPanel.Show(); }" />
                        </dx:ASPxButton>
                    </td>
                    <td>
                        <dx:ASPxButton CssClass="buttonL" ID="btnContinueBottom" runat="server"
                            Text="Continue" AutoPostBack="False">
                            <ClientSideEvents Click="function(s, e) {CallBackValidation.PerformCallback();
                        LoadingPanel.SetText(&#39;Please Wait...&#39;);
                        LoadingPanel.Show(); }" />
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>

        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <fd:flightdetail ID="flightdetail" runat="server" />
    <%--<PD:ucPaxDetail ID="ucPaxDetail" runat="server" /> --%>
</asp:Content>
