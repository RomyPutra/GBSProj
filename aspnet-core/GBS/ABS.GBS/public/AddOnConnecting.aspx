<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewPageMaster.Master" AutoEventWireup="true" CodeBehind="AddOnConnecting.aspx.cs" Inherits="GroupBooking.Web.AddOnConnecting" %>

<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %>
<%@ Register Src="../UserControl/flightbookingdetail.ascx" TagName="ucflightbookingdetail" TagPrefix="FBD" %>
<%@ Register Src="../UserControl/flighdetail.ascx" TagName="flightdetail" TagPrefix="fd" %>

<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v16.1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript">
        // <![CDATA[
        function gvPassenger_EndCallback(s, e) { }
        function gvPassenger2_EndCallback(s, e) { }
        function OnCallbackComplete(s, e) {
            if (e.result != "") {
                //lblmsg.SetValue(e.result);
                document.getElementById("ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = e.result;
                pcMessage.Show();
                LoadingPanel.Hide();
            } else {
                window.location.href = '../public/selectseat.aspx';
            }
        }
        function ShowLoginWindow() {
            pcMessage.Show();
        }
        function AddBaggage() {
            var e = document.getElementById("cmbBaggage");
            var strUser = cmbBaggage.GetText();
            var strUserValue = cmbBaggage.GetValue();
            gvPassenger.PerformCallback("Baggage|" + seBaggage.GetValue() + "|" + strUser + "|" + strUserValue);
        }
        function AddMeal() {
            var grid = glMeals.GetGridView();
            grid.GetRowValues(grid.GetFocusedRowIndex(), 'Detail;Price', OnGetRoValues);


        }
        function OnGetRoValues(values) {
            var grid = glMeals.GetGridView();
            var key = grid.GetRowKey(grid.GetFocusedRowIndex());
            gvPassenger.PerformCallback("Meal|" + seMeals.GetValue() + "|" + values + "|" + key);
        }
        function AddSport() {

            var e = document.getElementById("cmbSport");
            var strUser = cmbSport.GetText();
            var strUserValue = cmbSport.GetValue();
            gvPassenger.PerformCallback("Sport|" + seSport.GetValue() + "|" + strUser + "|" + strUserValue);
        }
        function AddDuty() {
            var grid = glDuty.GetGridView();
            grid.GetRowValues(grid.GetFocusedRowIndex(), 'Detail;Price', OnGetRowValues);

        }
        function OnGetRowValues(values) {
            var grid = glDuty.GetGridView();
            var key = grid.GetRowKey(grid.GetFocusedRowIndex());
            gvPassenger.PerformCallback("Duty|" + seDuty.GetValue() + "|" + values + "|" + key);
        }

        function AddComfort() {
            var grid = glComfort.GetGridView();
            grid.GetRowValues(grid.GetFocusedRowIndex(), 'Detail;Price', OnGetRowsValues);
        }

        function OnGetRowsValues(values) {
            var grid = glComfort.GetGridView();
            var key = grid.GetRowKey(grid.GetFocusedRowIndex());
            gvPassenger.PerformCallback("Comfort|" + seComfort.GetValue() + "|" + values + "|" + key);
        }

        function AddBaggage1() {
            var e = document.getElementById("cmbBaggage1");
            var strUser = cmbBaggage1.GetText();
            var strUserValue = cmbBaggage1.GetValue();
            gvPassenger1.PerformCallback("Baggage|" + seBaggage1.GetValue() + "|" + strUser + "|" + strUserValue);
        }
        function AddMeal1() {
            var grid = glMeals1.GetGridView();
            grid.GetRowValues(grid.GetFocusedRowIndex(), 'Detail;Price', OnGetRoValues1);


        }
        function OnGetRoValues1(values) {
            var grid = glMeals1.GetGridView();
            var key = grid.GetRowKey(grid.GetFocusedRowIndex());
            gvPassenger1.PerformCallback("Meal|" + seMeals1.GetValue() + "|" + values + "|" + key);
        }
        function AddSport1() {

            var e = document.getElementById("cmbSport1");
            var strUser = cmbSport1.GetText();
            var strUserValue = cmbSport1.GetValue();
            gvPassenger1.PerformCallback("Sport|" + seSport1.GetValue() + "|" + strUser + "|" + strUserValue);
        }
        function AddDuty1() {
            var grid = glDuty1.GetGridView();
            grid.GetRowValues(grid.GetFocusedRowIndex(), 'Detail;Price', OnGetRowValues1);

        }

        function OnGetRowValues1(values) {
            var grid = glDuty1.GetGridView();
            var key = grid.GetRowKey(grid.GetFocusedRowIndex());
            gvPassenger1.PerformCallback("Duty|" + seDuty1.GetValue() + "|" + values + "|" + key);
        }

        function AddComfort1() {
            var grid = glComfort1.GetGridView();
            grid.GetRowValues(grid.GetFocusedRowIndex(), 'Detail;Price', OnGetRowsValues1);
        }

        function OnGetRowsValues1(values) {
            var grid = glComfort1.GetGridView();
            var key = grid.GetRowKey(grid.GetFocusedRowIndex());
            gvPassenger.PerformCallback("Comfort|" + seComfort1.GetValue() + "|" + values + "|" + key);
        }

        function AddBaggage2() {
            var e = document.getElementById("cmbBaggage2");
            var strUser = cmbBaggage2.GetText();
            var strUserValue = cmbBaggage2.GetValue();
            gvPassenger2.PerformCallback("Baggage|" + seBaggage2.GetValue() + "|" + strUser + "|" + strUserValue);
        }
        function AddMeal2() {
            var grid = glMeals2.GetGridView();
            grid.GetRowValues(grid.GetFocusedRowIndex(), 'Detail;Price', OnGetRoValues2);


        }
        function OnGetRoValues2(values) {
            var grid = glMeals2.GetGridView();
            var key = grid.GetRowKey(grid.GetFocusedRowIndex());
            gvPassenger2.PerformCallback("Meal|" + seMeals2.GetValue() + "|" + values + "|" + key);
        }
        function AddSport2() {

            var e = document.getElementById("cmbSport2");
            var strUser = cmbSport2.GetText();
            var strUserValue = cmbSport2.GetValue();
            gvPassenger2.PerformCallback("Sport|" + seSport2.GetValue() + "|" + strUser + "|" + strUserValue);
        }
        function AddDuty2() {
            var grid = glDuty2.GetGridView();
            grid.GetRowValues(grid.GetFocusedRowIndex(), 'Detail;Price', OnGetRowValues2);

        }

        function OnGetRowValues2(values) {
            var grid = glDuty2.GetGridView();
            var key = grid.GetRowKey(grid.GetFocusedRowIndex());
            gvPassenger2.PerformCallback("Duty|" + seDuty2.GetValue() + "|" + values + "|" + key);
        }

        function AddComfort2() {
            var grid = glComfort2.GetGridView();
            grid.GetRowValues(grid.GetFocusedRowIndex(), 'Detail;Price', OnGetRowsValues2);
        }

        function OnGetRowsValues2(values) {
            var grid = glComfort2.GetGridView();
            var key = grid.GetRowKey(grid.GetFocusedRowIndex());
            gvPassenger2.PerformCallback("Comfort|" + seComfort2.GetValue() + "|" + values + "|" + key);
        }

        function AddBaggage21() {
            var e = document.getElementById("cmbBaggage21");
            var strUser = cmbBaggage21.GetText();
            var strUserValue = cmbBaggage21.GetValue();
            gvPassenger21.PerformCallback("Baggage|" + seBaggage21.GetValue() + "|" + strUser + "|" + strUserValue);
        }
        function AddMeal21() {
            var grid = glMeals21.GetGridView();
            grid.GetRowValues(grid.GetFocusedRowIndex(), 'Detail;Price', OnGetRoValues21);


        }
        function OnGetRoValues21(values) {
            var grid = glMeals21.GetGridView();
            var key = grid.GetRowKey(grid.GetFocusedRowIndex());
            gvPassenger21.PerformCallback("Meal|" + seMeals21.GetValue() + "|" + values + "|" + key);
        }
        function AddSport21() {

            var e = document.getElementById("cmbSport2");
            var strUser = cmbSport21.GetText();
            var strUserValue = cmbSport21.GetValue();
            gvPassenger21.PerformCallback("Sport|" + seSport21.GetValue() + "|" + strUser + "|" + strUserValue);
        }
        function AddDuty21() {
            var grid = glDuty21.GetGridView();
            grid.GetRowValues(grid.GetFocusedRowIndex(), 'Detail;Price', OnGetRowValues21);

        }

        function OnGetRowValues21(values) {
            var grid = glDuty2.GetGridView();
            var key = grid.GetRowKey(grid.GetFocusedRowIndex());
            gvPassenger21.PerformCallback("Duty|" + seDuty21.GetValue() + "|" + values + "|" + key);
        }

        function AddComfort2() {
            var grid = glComfort21.GetGridView();
            grid.GetRowValues(grid.GetFocusedRowIndex(), 'Detail;Price', OnGetRowsValues21);
        }

        function OnGetRowsValues21(values) {
            var grid = glComfort21.GetGridView();
            var key = grid.GetRowKey(grid.GetFocusedRowIndex());
            gvPassenger21.PerformCallback("Comfort|" + seComfort21.GetValue() + "|" + values + "|" + key);
        }

        function showcollapse1(){
            var collapse1 = document.getElementById("collapse1");
            var collapse2 = document.getElementById("collapse2");
            var collapse3 = document.getElementById("collapse3");
            var collapse4 = document.getElementById("collapse4");
            var collapse5 = document.getElementById("collapse5");
            collapse1.style.visibility='visible';
            collapse2.style.visibility='hidden';
            collapse3.style.visibility='hidden';
            collapse4.style.visibility='hidden';
            collapse5.style.visibility='hidden';
        }

        function showcollapse2(){
            var collapse1 = document.getElementById("collapse1");
            var collapse2 = document.getElementById("collapse2");
            var collapse3 = document.getElementById("collapse3");
            var collapse4 = document.getElementById("collapse4");
            var collapse5 = document.getElementById("collapse5");
            collapse1.style.visibility='hidden';
            collapse2.style.visibility='visible';
            collapse3.style.visibility='hidden';
            collapse4.style.visibility='hidden';
            collapse5.style.visibility='hidden';
        }

        function showcollapse3(){
            var collapse1 = document.getElementById("collapse1");
            var collapse2 = document.getElementById("collapse2");
            var collapse3 = document.getElementById("collapse3");
            var collapse4 = document.getElementById("collapse4");
            var collapse5 = document.getElementById("collapse5");
            collapse1.style.visibility='hidden';
            collapse2.style.visibility='hidden';
            collapse3.style.visibility='visible';
            collapse4.style.visibility='hidden';
            collapse5.style.visibility='hidden';
        }

        function showcollapse4(){
            var collapse1 = document.getElementById("collapse1");
            var collapse2 = document.getElementById("collapse2");
            var collapse3 = document.getElementById("collapse3");
            var collapse4 = document.getElementById("collapse4");
            var collapse5 = document.getElementById("collapse5");
            collapse1.style.visibility='hidden';
            collapse2.style.visibility='hidden';
            collapse3.style.visibility='hidden';
            collapse4.style.visibility='visible';
            collapse5.style.visibility='hidden';
        }

        function showcollapse5(){
            var collapse1 = document.getElementById("collapse1");
            var collapse2 = document.getElementById("collapse2");
            var collapse3 = document.getElementById("collapse3");
            var collapse4 = document.getElementById("collapse4");
            var collapse5 = document.getElementById("collapse5");
            collapse1.style.visibility='hidden';
            collapse2.style.visibility='hidden';
            collapse3.style.visibility='hidden';
            collapse4.style.visibility='hidden';
            collapse5.style.visibility='visible';
        }

        function showcollapse11() {
            var collapse1 = document.getElementById("collapse11");
            var collapse2 = document.getElementById("collapse21");
            var collapse3 = document.getElementById("collapse31");
            var collapse4 = document.getElementById("collapse41");
            var collapse5 = document.getElementById("collapse51");
            collapse1.style.visibility = 'visible';
            collapse2.style.visibility = 'hidden';
            collapse3.style.visibility = 'hidden';
            collapse4.style.visibility = 'hidden';
            collapse5.style.visibility = 'hidden';
        }

        function showcollapse21() {
            var collapse1 = document.getElementById("collapse11");
            var collapse2 = document.getElementById("collapse21");
            var collapse3 = document.getElementById("collapse31");
            var collapse4 = document.getElementById("collapse41");
            var collapse5 = document.getElementById("collapse51");
            collapse1.style.visibility = 'hidden';
            collapse2.style.visibility = 'visible';
            collapse3.style.visibility = 'hidden';
            collapse4.style.visibility = 'hidden';
            collapse5.style.visibility = 'hidden';
        }

        function showcollapse31() {
            var collapse1 = document.getElementById("collapse11");
            var collapse2 = document.getElementById("collapse21");
            var collapse3 = document.getElementById("collapse31");
            var collapse4 = document.getElementById("collapse41");
            var collapse5 = document.getElementById("collapse51");
            collapse1.style.visibility = 'hidden';
            collapse2.style.visibility = 'hidden';
            collapse3.style.visibility = 'visible';
            collapse4.style.visibility = 'hidden';
            collapse5.style.visibility = 'hidden';
        }

        function showcollapse41() {
            var collapse1 = document.getElementById("collapse11");
            var collapse2 = document.getElementById("collapse21");
            var collapse3 = document.getElementById("collapse31");
            var collapse4 = document.getElementById("collapse41");
            var collapse5 = document.getElementById("collapse51");
            collapse1.style.visibility = 'hidden';
            collapse2.style.visibility = 'hidden';
            collapse3.style.visibility = 'hidden';
            collapse4.style.visibility = 'visible';
            collapse5.style.visibility = 'hidden';
        }

        function showcollapse51() {
            var collapse1 = document.getElementById("collapse11");
            var collapse2 = document.getElementById("collapse21");
            var collapse3 = document.getElementById("collapse31");
            var collapse4 = document.getElementById("collapse41");
            var collapse5 = document.getElementById("collapse51");
            collapse1.style.visibility = 'hidden';
            collapse2.style.visibility = 'hidden';
            collapse3.style.visibility = 'hidden';
            collapse4.style.visibility = 'hidden';
            collapse5.style.visibility = 'visible';
        }

        function showcollapse12(){
            var collapse12 = document.getElementById("collapse12");
            var collapse22 = document.getElementById("collapse22");
            var collapse32 = document.getElementById("collapse32");
            var collapse42 = document.getElementById("collapse42");
            var collapse52 = document.getElementById("collapse52");
            collapse12.style.visibility='visible';
            collapse22.style.visibility='hidden';
            collapse32.style.visibility='hidden';
            collapse42.style.visibility='hidden';
            collapse52.style.visibility='hidden';
        }

        function showcollapse22(){
            var collapse12 = document.getElementById("collapse12");
            var collapse22 = document.getElementById("collapse22");
            var collapse32 = document.getElementById("collapse32");
            var collapse42 = document.getElementById("collapse42");
            var collapse52 = document.getElementById("collapse52");
            collapse12.style.visibility='hidden';
            collapse22.style.visibility='visible';
            collapse32.style.visibility='hidden';
            collapse42.style.visibility='hidden';
            collapse52.style.visibility='hidden';
        }

        function showcollapse32(){
            var collapse12 = document.getElementById("collapse12");
            var collapse22 = document.getElementById("collapse22");
            var collapse32 = document.getElementById("collapse32");
            var collapse42 = document.getElementById("collapse42");
            var collapse52 = document.getElementById("collapse52");
            collapse12.style.visibility='hidden';
            collapse22.style.visibility='hidden';
            collapse32.style.visibility='visible';
            collapse42.style.visibility='hidden';
            collapse52.style.visibility='hidden';
        }

        function showcollapse42(){
            var collapse12 = document.getElementById("collapse12");
            var collapse22 = document.getElementById("collapse22");
            var collapse32 = document.getElementById("collapse32");
            var collapse42 = document.getElementById("collapse42");
            var collapse52 = document.getElementById("collapse52");
            collapse12.style.visibility='hidden';
            collapse22.style.visibility='hidden';
            collapse32.style.visibility='hidden';
            collapse42.style.visibility='visible';
            collapse52.style.visibility='hidden';
        }

        function showcollapse52(){
            var collapse12 = document.getElementById("collapse12");
            var collapse22 = document.getElementById("collapse22");
            var collapse32 = document.getElementById("collapse32");
            var collapse42 = document.getElementById("collapse42");
            var collapse52 = document.getElementById("collapse52");
            collapse12.style.visibility='hidden';
            collapse22.style.visibility='hidden';
            collapse32.style.visibility='hidden';
            collapse42.style.visibility='hidden';
            collapse52.style.visibility='visible';
        }

        function showcollapse121() {
            var collapse12 = document.getElementById("collapse121");
            var collapse22 = document.getElementById("collapse221");
            var collapse32 = document.getElementById("collapse321");
            var collapse42 = document.getElementById("collapse421");
            var collapse52 = document.getElementById("collapse521");
            collapse12.style.visibility = 'visible';
            collapse22.style.visibility = 'hidden';
            collapse32.style.visibility = 'hidden';
            collapse42.style.visibility = 'hidden';
            collapse52.style.visibility = 'hidden';
        }

        function showcollapse221() {
            var collapse12 = document.getElementById("collapse121");
            var collapse22 = document.getElementById("collapse221");
            var collapse32 = document.getElementById("collapse321");
            var collapse42 = document.getElementById("collapse421");
            var collapse52 = document.getElementById("collapse521");
            collapse12.style.visibility = 'hidden';
            collapse22.style.visibility = 'visible';
            collapse32.style.visibility = 'hidden';
            collapse42.style.visibility = 'hidden';
            collapse52.style.visibility = 'hidden';
        }

        function showcollapse321() {
            var collapse12 = document.getElementById("collapse121");
            var collapse22 = document.getElementById("collapse221");
            var collapse32 = document.getElementById("collapse321");
            var collapse42 = document.getElementById("collapse421");
            var collapse52 = document.getElementById("collapse521");
            collapse12.style.visibility = 'hidden';
            collapse22.style.visibility = 'hidden';
            collapse32.style.visibility = 'visible';
            collapse42.style.visibility = 'hidden';
            collapse52.style.visibility = 'hidden';
        }

        function showcollapse421() {
            var collapse12 = document.getElementById("collapse121");
            var collapse22 = document.getElementById("collapse221");
            var collapse32 = document.getElementById("collapse321");
            var collapse42 = document.getElementById("collapse421");
            var collapse52 = document.getElementById("collapse521");
            collapse12.style.visibility = 'hidden';
            collapse22.style.visibility = 'hidden';
            collapse32.style.visibility = 'hidden';
            collapse42.style.visibility = 'visible';
            collapse52.style.visibility = 'hidden';
        }

        function showcollapse521() {
            var collapse12 = document.getElementById("collapse121");
            var collapse22 = document.getElementById("collapse221");
            var collapse32 = document.getElementById("collapse321");
            var collapse42 = document.getElementById("collapse421");
            var collapse52 = document.getElementById("collapse521");
            collapse12.style.visibility = 'hidden';
            collapse22.style.visibility = 'hidden';
            collapse32.style.visibility = 'hidden';
            collapse42.style.visibility = 'hidden';
            collapse52.style.visibility = 'visible';
        }
        // ]]> 
    </script>

    <msg:msgControl ID="msgcontrol" runat="server" />
    <div>
        <dx:ASPxCallback ID="CallBackValidation" ClientInstanceName="CallBackValidation" runat="server" OnCallback="ValidatePopup">
            <ClientSideEvents CallbackComplete="OnCallbackComplete" />
        </dx:ASPxCallback>
    </div>
    <%--<div class="row">
        <div class="col-sm-12">
            <div id="fareInformation" class="col-sm-6">
                <div style="padding-top: 15px; width: 100%;">
                    <div class="tableWrapper" style="margin-top: 5px;">
                        <table class="table table-bordered">
                            <tr>
                                <td colspan="2" style="text-align: center"><span>Average Fare:</span><span id="averageFareAmount">1,150.00 MYR</span></td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: center"><span>Total Pax:</span><span id="totalPax">50</span></td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: center"><span>Total Average Fare:</span><span id="totalAverage" style="font-weight: 700; padding: 0 10px;">11,500.00 MYR</span></td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="flightDetailBookingInfoNew" class="" style="">
                    <div class="redSectionHeader noShow">
                        <div>Flight Details</div>
                    </div>
                    <table width="650" bgcolor="#f7f3f7" class="table table-bordered">
                        <tr>
                            <td>
                                <font color='red'>Depart</font>
                            </td>
                            <td>
                                <font color='red'>Return</font>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="infoFlight">
                                    <span class="infoFlightSpan">Depart Fare</span>
                                    <span class="infoFlightSpan algnRight">5,750.00 MYR</span>
                                </div>
                            </td>
                            <td>
                                <div class="infoFlight">
                                    <span class="infoFlightSpan">Return Fare</span>
                                    <span class="infoFlightSpan algnRight">5,750.00 MYR</span>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="infoFlight">
                                    <span class="infoFlightSpan">Pax Fare</span>
                                    <span class="infoFlightSpan algnRight">4,750.00</span>
                                </div>
                            </td>
                            <td>
                                <div class="infoFlight">
                                    <span class="infoFlightSpan">Pax Fare</span>
                                    <span class="infoFlightSpan algnRight">4,750.00</span>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="infoFlight">
                                    <span class="infoFlightSpan">Airport Tax</span>
                                    <span class="infoFlightSpan algnRight">0.00</span>
                                </div>
                            </td>
                            <td>
                                <div class="infoFlight">
                                    <span class="infoFlightSpan">Airport Tax</span>
                                    <span class="infoFlightSpan algnRight">0.00</span>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="infoFlight">
                                    <span class="infoFlightSpan">Fuel Fee</span>
                                    <span class="infoFlightSpan algnRight">0.00</span>
                                </div>
                            </td>
                            <td>
                                <div class="infoFlight">
                                    <span class="infoFlightSpan">Fuel Fee</span>
                                    <span class="infoFlightSpan algnRight">0.00</span>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="infoFlight">
                                    <span class="infoFlightSpan">Service Charge</span>
                                    <span class="infoFlightSpan algnRight">200.00</span>
                                </div>
                            </td>
                            <td>
                                <div class="infoFlight">
                                    <span class="infoFlightSpan">Service Charge</span>
                                    <span class="infoFlightSpan algnRight">200.00</span>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="infoFlight">
                                    <span class="infoFlightSpan">Other Fee</span>
                                    <span class="infoFlightSpan algnRight">800.00</span>
                                </div>
                            </td>
                            <td>
                                <div class="infoFlight">
                                    <span class="infoFlightSpan">Other Fee</span>
                                    <span class="infoFlightSpan algnRight">800.00</span>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="col-sm-5" style="padding-top: 20px;">
                <table id="totalEstimatedFare" class="enhanceFontSize" width="100%" style="padding: 5px;">
                    <tr class="totalFare">
                        <td>
                            <h4>Total Amount</h4>
                        </td>
                        <td style="text-align: right">
                            <h4>11,500.00 MYR</h4>
                        </td>
                    </tr>
                </table>
                <table class="flightAverageInfo table table-bordered">
                    <tr>
                        <td>Depart Fare</td>
                        <td style="text-align: right">5,750.00 MYR</td>
                    </tr>
                    <tr>
                        <td>Return Fare</td>
                        <td style="text-align: right">5,750.00 MYR</td>
                    </tr>
                    <tr>
                        <td>Total Baggage Cost</td>
                        <td style="text-align: right">0.00</td>
                    </tr>
                    <tr>
                        <td>Total Meal Cost</td>
                        <td style="text-align: right">0.00</td>
                    </tr>
                    <tr>
                        <td>Total Sport Equipment</td>
                        <td style="text-align: right">0.00</td>
                    </tr>
                    <tr>
                        <td>Total Comfort Kit</td>
                        <td style="text-align: right">0.00</td>
                    </tr>
                    <tr>
                        <td>Total Duty Free Item</td>
                        <td style="text-align: right">0.00</td>
                    </tr>


                </table>
            </div>
        </div>
    </div>--%>
    <%--<ul class="nav nav-tabs">
        <li class="active"><a href="#home" data-toggle="tab">
            <div class="flightIcon_2_1" style="width: 26px; height: 19px; background: url('../content/images/aa-sprites.png') no-repeat -135px -240px; float: left; padding-right: 31px;">
            </div>
            KUL - BKI</a>
        </li>
        <li>
            <a href="#menu" data-toggle="tab">
                <div class="flightIcon_2_1" style="width: 26px; height: 19px; background: url('../content/images/aa-sprites.png') no-repeat -169px -240px; float: left; padding-right: 31px;">
                </div>
                BKI - KUL</a>
        </li>
    </ul>--%>

    <div istyle="float:right;">
                            <dx:ASPxButton CssClass="buttonL" ID="btn_Next" runat="server"
                    Text="Continue" AutoPostBack="False">
                    <ClientSideEvents Click="function(s, e) {CallBackValidation.PerformCallback();
                                            LoadingPanel.SetText(&#39;Please Wait...&#39;);
                                           LoadingPanel.Show(); }" />
                </dx:ASPxButton>
                        </div>

    <dx:ASPxPageControl SkinID="None" Width="100%" EnableViewState="false" ID="TabControl" ClientInstanceName="TabControl"
        runat="server" ActiveTabIndex="0" AutoPostBack="false"
        TabSpacing="0px" ContentStyle-Border-BorderWidth="0"
        EnableHierarchyRecreation="True">

        <TabPages>

            <dx:TabPage Text="" Name="Tab1" Visible="true">
                <ContentCollection>
                    <dx:ContentControl ID="ContentControl1" runat="server">
                        
                        <div id="home" class="tab-pane fade in active">
                            <div class="tempCtr">
                                <div class="row">
                                    <div class="controllerWrapper col-sm-12 col-md-11">
                                        <div class="buttonsWrapper col-sm-12 nav nav-pills">
                                            <div class="col-sm-2">
                                                    <div class="btnCtrl roundedCorner"><a  href="#collapse1" onclick="showcollapse1()">
                                                        <div class="iconBtn fa fa-suitcase"></div>
                                                    </a>
                                                    </div>
                                                <div class="title">Baggage</div>

                                            </div>
                                            <div class="col-sm-2">
                                                <div class="btnCtrl roundedCorner"> <a href="#collapse2" onclick="showcollapse2()">
                                                    <div class="iconBtn fa fa-cutlery"></div>
                                                    </a>
                                                </div>
                                                <div class="title">Meal</div>
                                            </div>
                                            <div class="col-sm-2">
                                                <div class="btnCtrl roundedCorner"> <a href="#collapse3" onclick="showcollapse3()">
                                                    <div class="iconBtn fa fa-futbol-o"></div>
                                                    </a>
                                                </div>
                                                <div class="title">Sport Equipment</div>
                                            </div>
                                            <div class="col-sm-2">
                                                <div class="btnCtrl roundedCorner"> <a href="#collapse4" onclick="showcollapse4()">
                                                    <div class="iconBtn fa fa-bed"></div>
                                                    </a>
                                                </div>
                                                <div class="title">Comfort Kit</div>
                                            </div>
                                            <div class="col-sm-2">
                                                <div class="btnCtrl roundedCorner"> <a href="#collapse5" onclick="showcollapse5()">
                                                    <div class="iconBtn fa fa-gift"></div>
                                                    </a>
                                                </div>
                                                <div class="title">Duty Free</div>
                                            </div>
                                        </div>
                                        <div class="contentWrapper col-sm-12">
                                            <%--div class="tab-content">--%>
                                                <div id="collapse1" style="visibility:hidden;" >

                                                    <div class="baggageAddonWrapper">
                                                        <div style="padding-top: 10px;">
                                                            <table>
                                                                <tbody>
                                                                    <tr>
                                                                        <td style="padding-top: 10px;">
                                                                            <div class="controlledCollapse collapse in">
                                                                                <dx:ASPxComboBox ID="cmbBaggage" ClientInstanceName="cmbBaggage" runat="server" class="Select" AutoPostBack="false" IncrementalFilteringMode="StartsWith" Width="100">
                                                                                </dx:ASPxComboBox>
                                                                            </div>
                                                                        </td>

                                                                        <td>
                                                                            <table class="addBaggageInput" style="margin-top: 10px;">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <div class="QtyWrapper">
                                                                                                <dx:ASPxSpinEdit ID="seBaggage" ClientInstanceName="seBaggage" runat="server" Number="1" NumberType="Integer" MinValue="1" MaxValue="50" Width="50px">
                                                                                                </dx:ASPxSpinEdit>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td style="padding-left: 10px;">
                                                                                            <button type="button" class="btn btn-danger" onclick="AddBaggage()">Assign</button>
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
                                                <div id="collapse2" style="visibility:hidden;" >
                                                    <div class="baggageAddonWrapper" style="padding-top: 10px;">
                                                        <div class="mealAll">
                                                            <div class="showDropMeal collapse in" style="margin-bottom: 10px;">
                                                                <table>
                                                                    <tbody>

                                                                        <tr>
                                                                            <td style="padding-top: 10px;">
                                                                                <div class="controlledCollapse collapse in">

                                                                                    <dx:ASPxGridLookup ID="glMeals" runat="server" KeyFieldName="SSRCode" Width="300px" EnableRowsCache="false" ClientInstanceName="glMeals"
                                                                                        SelectionMode="Single" TextFormatString="{0} {1}" OnInit="glMeals_Init">
                                                                                        <ClearButton DisplayMode="OnHover"></ClearButton>
                                                                                        <%--<ClientSideEvents Init="function (s,e){glMeals_Init(s,e);}" EndCallback="function (s,e){glMeals_EndCallBack(s,e);}" />--%>
                                                                                        <Columns>
                                                                                            <dx:GridViewDataColumn FieldName="Detail" />
                                                                                            <dx:GridViewDataColumn FieldName="Price" />
                                                                                        </Columns>
                                                                                        <GridViewProperties EnableRowsCache="false">
                                                                                            <Templates>
                                                                                                <DataRow>
                                                                                                    <div class="templateContainer">
                                                                                                        <div style="float: left; margin-right: 2px;">
                                                                                                            <img id="img" style="width: 100px; padding: 5px;" runat="server" alt='' src='<%#Eval("Images")%>' />
                                                                                                            <%--<dx:ASPxBinaryImage ID="PhotoBinaryImage" runat="server" Width="100px" Height="100px" Value='<%# Eval("Photo") %>' />--%>
                                                                                                        </div>
                                                                                                        <table class="templateTable">
                                                                                                            <tr>
                                                                                                                <td class="caption">Description:</td>
                                                                                                                <td><%# Eval("Detail")%></td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td class="caption">Price:</td>
                                                                                                                <td><%# Eval("Price")%></td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </div>
                                                                                                </DataRow>
                                                                                            </Templates>
                                                                                            <Settings ShowColumnHeaders="False" VerticalScrollableHeight="300" />
                                                                                            <SettingsPager Mode="ShowAllRecords" />
                                                                                            <SettingsBehavior EnableRowHotTrack="True" />
                                                                                        </GridViewProperties>
                                                                                    </dx:ASPxGridLookup>
                                                                                </div>
                                                                            </td>
                                                                            <td>
                                                                                <div class="QtyWrapper">
                                                                                    <dx:ASPxSpinEdit ID="seMeals" ClientInstanceName="seMeals" runat="server" Number="1" NumberType="Integer" MinValue="1" MaxValue="50" Width="50px">
                                                                                    </dx:ASPxSpinEdit>
                                                                                </div>
                                                                            </td>
                                                                            <td style="padding-left: 10px;">
                                                                                <button type="button" class="btn btn-danger" onclick="AddMeal()">Assign</button>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="collapse3" style="visibility:hidden;" >
                                                    <div class="baggageAddonWrapper" style="padding-top: 10px;">

                                                        <div class="mealAll">

                                                            <div class="showDropMeal collapse in" style="margin-bottom: 10px;">
                                                                <table>
                                                                    <tbody>

                                                                        <tr>
                                                                            <td style="padding-top: 10px;">
                                                                                <div class="controlledCollapse collapse in">
                                                                                    <dx:ASPxComboBox ID="cmbSport" ClientInstanceName="cmbSport" runat="server" class="Select" AutoPostBack="false" IncrementalFilteringMode="StartsWith" Width="100">
                                                                                    </dx:ASPxComboBox>
                                                                                </div>

                                                                            </td>
                                                                            <td>
                                                                                <table class="addBaggageInput" style="margin-top: 10px;">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td style="padding-left: 10px;">
                                                                                                <dx:ASPxSpinEdit ID="seSport" ClientInstanceName="seSport" runat="server" Number="1" NumberType="Integer" MinValue="1" MaxValue="50" Width="50px">
                                                                                                </dx:ASPxSpinEdit>
                                                                                            </td>
                                                                                            <td style="padding-left: 10px;">
                                                                                                <button type="button" class="btn btn-danger" onclick="AddSport()">Assign</button>
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
                                                <div id="collapse4" style="visibility:hidden;" >
                                                    <div class="baggageAddonWrapper">

                                                        <div style="padding-top: 10px;">
                                                            <table>

                                                                <tr>
                                                                    <td style="padding-top: 10px;">
                                                                        <div class="controlledCollapse collapse in">
                                                                            <dx:ASPxGridLookup ID="glComfort" runat="server" KeyFieldName="SSRCode" Width="300px" EnableRowsCache="false" ClientInstanceName="glComfort"
                                                                                SelectionMode="Single" TextFormatString="{0} {1}" OnInit="glComfort_Init">
                                                                                <ClearButton DisplayMode="OnHover"></ClearButton>
                                                                                <Columns>
                                                                                    <dx:GridViewDataColumn FieldName="Detail" />
                                                                                    <dx:GridViewDataColumn FieldName="Price" />
                                                                                </Columns>
                                                                                <GridViewProperties EnableRowsCache="false">
                                                                                    <Templates>
                                                                                        <DataRow>
                                                                                            <div class="templateContainer">
                                                                                                <div style="float: left; margin-right: 2px;">
                                                                                                    <img id="img6" style="width: 100px; padding: 5px;" runat="server" alt='' src='<%#Eval("Images")%>' />
                                                                                                    <%--<dx:ASPxBinaryImage ID="PhotoBinaryImage" runat="server" Width="100px" Height="100px" Value='<%# Eval("Photo") %>' />--%>
                                                                                                </div>
                                                                                                <table class="templateTable">
                                                                                                    <tr>
                                                                                                        <td class="caption">Description:</td>
                                                                                                        <td><%# Eval("Detail")%></td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td class="caption">Price:</td>
                                                                                                        <td><%# Eval("Price")%></td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </div>
                                                                                        </DataRow>
                                                                                    </Templates>
                                                                                    <Settings ShowColumnHeaders="False" VerticalScrollableHeight="300" />
                                                                                    <SettingsPager Mode="ShowAllRecords" />
                                                                                    <SettingsBehavior EnableRowHotTrack="True" />
                                                                                </GridViewProperties>
                                                                            </dx:ASPxGridLookup>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <table class="addBaggageInput" style="margin-top: 10px;">
                                                                            <tr>
                                                                                <td>
                                                                                    <div class="QtyWrapper">
                                                                                        <dx:ASPxSpinEdit ID="seComfort" ClientInstanceName="seComfort" runat="server" Number="1" NumberType="Integer" MinValue="1" MaxValue="50" Width="50px">
                                                                                        </dx:ASPxSpinEdit>
                                                                                    </div>
                                                                                </td>
                                                                                <td style="padding-left: 10px;">
                                                                                    <button type="button" class="btn btn-danger" onclick="AddComfort()">Assign</button>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="collapse5" style="visibility:hidden;" >
                                                    <div class="baggageAddonWrapper">
                                                        <div style="padding-top: 10px;">
                                                            <table>
                                                                <tr>
                                                                    <td style="padding-top: 10px;">
                                                                        <div class="controlledCollapse collapse in">
                                                                            <dx:ASPxGridLookup ID="glDuty" runat="server" KeyFieldName="SSRCode" Width="300px" EnableRowsCache="false" ClientInstanceName="glDuty"
                                                                                SelectionMode="Single" TextFormatString="{0} {1}" OnInit="glDuty_Init">
                                                                                <ClearButton DisplayMode="OnHover"></ClearButton>
                                                                                <Columns>
                                                                                    <dx:GridViewDataColumn FieldName="Detail" />
                                                                                    <dx:GridViewDataColumn FieldName="Price" />
                                                                                </Columns>
                                                                                <GridViewProperties EnableRowsCache="false">
                                                                                    <Templates>
                                                                                        <DataRow>
                                                                                            <div class="templateContainer">
                                                                                                <div style="float: left; margin-right: 2px;">
                                                                                                    <img id="img7" style="width: 100px; padding: 5px;" runat="server" alt='' src='<%#Eval("Images")%>' />
                                                                                                    <%--<dx:ASPxBinaryImage ID="PhotoBinaryImage" runat="server" Width="100px" Height="100px" Value='<%# Eval("Photo") %>' />--%>
                                                                                                </div>
                                                                                                <table class="templateTable">
                                                                                                    <tr>
                                                                                                        <td class="caption">Description:</td>
                                                                                                        <td><%# Eval("Description")%></td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td class="caption">Price:</td>
                                                                                                        <td><%# Eval("Price")%></td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </div>
                                                                                        </DataRow>
                                                                                    </Templates>
                                                                                    <Settings ShowColumnHeaders="False" VerticalScrollableHeight="300" />
                                                                                    <SettingsPager Mode="ShowAllRecords" />
                                                                                    <SettingsBehavior EnableRowHotTrack="True" />
                                                                                </GridViewProperties>
                                                                            </dx:ASPxGridLookup>
                                                                        </div>
                                                                    </td>

                                                                    <td>
                                                                        <table class="addBaggageInput" style="margin-top: 10px;">
                                                                            <tr>
                                                                                <td>
                                                                                    <div class="QtyWrapper">
                                                                                        <dx:ASPxSpinEdit ID="seDuty" ClientInstanceName="seDuty" runat="server" Number="1" NumberType="Integer" MinValue="1" MaxValue="50" Width="50px">
                                                                                        </dx:ASPxSpinEdit>
                                                                                    </div>
                                                                                </td>
                                                                                <td style="padding-left: 10px;">
                                                                                    <button type="button" class="btn btn-danger" onclick="AddDuty()">Assign</button>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div>
                                           <%-- </div>--%>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <br />
                            <div class="div">
                                <h4>Passenger List</h4>
                            </div>

                            <dx:ASPxGridView ID="gvPassenger" runat="server" KeyFieldName="PassengerID" EnableRowsCache="false" EnableCallBacks="true"
                                ClientInstanceName="gvPassenger" Width="100%" OnCustomCallback="gvPassenger_CustomCallback">
                                <ClientSideEvents EndCallback="function(s, e) { gvPassenger_EndCallback(s, e);}" />
                                <Columns>
                                    <dx:GridViewDataColumn FieldName="PNR" Caption="RecordLocator" ReadOnly="true" />

                                    <dx:GridViewDataTextColumn FieldName="FirstName" Caption="First Name" ReadOnly="true">
                                        <PropertiesTextEdit>

                                            <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                        </PropertiesTextEdit>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="LastName" Caption="Last Name" ReadOnly="true" />
                                    <dx:GridViewDataComboBoxColumn FieldName="Meal" Caption="Meal" Width="120px">
                                        <PropertiesComboBox TextField="Detail" ValueField="SSRCode"
                                            ValueType="System.String">
                                        </PropertiesComboBox>
                                        <EditItemTemplate>
                                            <dx:ASPxGridLookup ID="glMealP1" runat="server" KeyFieldName="SSRCode" Width="120px" EnableRowsCache="false" ClientInstanceName="glMealP1"
                                                SelectionMode="Single" TextFormatString="{0} {1}">
                                                <ClearButton DisplayMode="OnHover"></ClearButton>
                                                <Columns>
                                                    <dx:GridViewDataColumn FieldName="Detail" />
                                                    <dx:GridViewDataColumn FieldName="Price" />
                                                </Columns>
                                                <GridViewProperties EnableRowsCache="false">
                                                    <Templates>
                                                        <DataRow>
                                                            <div class="templateContainer">
                                                                <div style="float: left; margin-right: 2px;">
                                                                    <img id="img" style="width: 100px; padding: 5px;" runat="server" alt='' src='<%#Eval("Images")%>' />
                                                                </div>
                                                                <table class="templateTable">
                                                                    <tr>
                                                                        <td class="caption">Description:</td>
                                                                        <td><%# Eval("Detail")%></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="caption">Price:</td>
                                                                        <td><%# Eval("Price")%></td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </DataRow>
                                                    </Templates>
                                                    <Settings ShowColumnHeaders="False" VerticalScrollableHeight="300" />
                                                    <SettingsPager Mode="ShowAllRecords" />
                                                    <SettingsBehavior EnableRowHotTrack="True" />
                                                </GridViewProperties>
                                            </dx:ASPxGridLookup>
                                        </EditItemTemplate>
                                    </dx:GridViewDataComboBoxColumn>
                                    <dx:GridViewDataComboBoxColumn FieldName="Baggage" Caption="Baggage" Width="120px">
                                        <PropertiesComboBox TextField="Detail" ValueField="SSRCode" ValueType="System.String">
                                        </PropertiesComboBox>
                                    </dx:GridViewDataComboBoxColumn>
                                    <dx:GridViewDataComboBoxColumn FieldName="Sport" Caption="Sport Equipment" Width="150px">
                                        <PropertiesComboBox TextField="Detail" ValueField="SSRCode" ValueType="System.String">
                                        </PropertiesComboBox>
                                    </dx:GridViewDataComboBoxColumn>
                                    <dx:GridViewDataComboBoxColumn FieldName="Comfort" Caption="Comfort Kit" Width="120px">
                                        <PropertiesComboBox TextField="Detail" ValueField="SSRCode"
                                            ValueType="System.String">
                                        </PropertiesComboBox>
                                        <EditItemTemplate>
                                            <dx:ASPxGridLookup ID="glComfortP1" runat="server" KeyFieldName="SSRCode" Width="120px" EnableRowsCache="false" ClientInstanceName="glComfortP1"
                                                SelectionMode="Single" TextFormatString="{0} {1}">
                                                <ClearButton DisplayMode="OnHover"></ClearButton>
                                                <Columns>
                                                    <dx:GridViewDataColumn FieldName="Detail" />
                                                    <dx:GridViewDataColumn FieldName="Price" />
                                                </Columns>
                                                <GridViewProperties EnableRowsCache="false">
                                                    <Templates>
                                                        <DataRow>
                                                            <div class="templateContainer">
                                                                <div style="float: left; margin-right: 2px;">
                                                                    <img id="img2" style="width: 100px; padding: 5px;" runat="server" alt='' src='<%#Eval("Images")%>' />
                                                                    <%--<dx:ASPxBinaryImage ID="PhotoBinaryImage" runat="server" Width="100px" Height="100px" Value='<%# Eval("Photo") %>' />--%>
                                                                </div>
                                                                <table class="templateTable">
                                                                    <tr>
                                                                        <td class="caption">Description:</td>
                                                                        <td><%# Eval("Detail")%></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="caption">Price:</td>
                                                                        <td><%# Eval("Price")%></td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </DataRow>
                                                    </Templates>
                                                    <Settings ShowColumnHeaders="False" VerticalScrollableHeight="300" />
                                                    <SettingsPager Mode="ShowAllRecords" />
                                                    <SettingsBehavior EnableRowHotTrack="True" />
                                                </GridViewProperties>
                                            </dx:ASPxGridLookup>
                                        </EditItemTemplate>
                                    </dx:GridViewDataComboBoxColumn>
                                    <dx:GridViewDataComboBoxColumn FieldName="Duty" Caption="Duty Free" Width="120px">
                                        <PropertiesComboBox TextField="Detail" ValueField="SSRCode"
                                            ValueType="System.String">
                                        </PropertiesComboBox>
                                        <EditItemTemplate>
                                            <dx:ASPxGridLookup ID="glDutyFreeP1" runat="server" KeyFieldName="SSRCode" Width="120px" EnableRowsCache="false" ClientInstanceName="glDutyFreeP1"
                                                SelectionMode="Single" TextFormatString="{0} {1}">
                                                <ClearButton DisplayMode="OnHover"></ClearButton>
                                                <Columns>
                                                    <dx:GridViewDataColumn FieldName="Detail" />
                                                    <dx:GridViewDataColumn FieldName="Price" />
                                                </Columns>
                                                <GridViewProperties EnableRowsCache="false">
                                                    <Templates>
                                                        <DataRow>
                                                            <div class="templateContainer">
                                                                <div style="float: left; margin-right: 2px;">
                                                                    <img id="img1" style="width: 100px; padding: 5px;" runat="server" alt='' src='<%#Eval("Images")%>' />
                                                                    <%--<dx:ASPxBinaryImage ID="PhotoBinaryImage" runat="server" Width="100px" Height="100px" Value='<%# Eval("Photo") %>' />--%>
                                                                </div>
                                                                <table class="templateTable">
                                                                    <tr>
                                                                        <td class="caption">Description:</td>
                                                                        <td><%# Eval("Detail")%></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="caption">Price:</td>
                                                                        <td><%# Eval("Price")%></td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </DataRow>
                                                    </Templates>
                                                    <Settings ShowColumnHeaders="False" VerticalScrollableHeight="300" />
                                                    <SettingsPager Mode="ShowAllRecords" />
                                                    <SettingsBehavior EnableRowHotTrack="True" />
                                                </GridViewProperties>
                                            </dx:ASPxGridLookup>
                                        </EditItemTemplate>

                                    </dx:GridViewDataComboBoxColumn>
                                </Columns>
                                <SettingsPager Mode="ShowPager" PageSize="10" Position="Top" />
                                <SettingsEditing Mode="Batch" />
                            </dx:ASPxGridView>
                        </div>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>

            <dx:TabPage Text="" Name="Tab11" Visible="true">
                <ContentCollection>
                    <dx:ContentControl ID="ContentControl3" runat="server">
                        
                        <div id="home1" class="tab-pane fade in active">
                            <div class="tempCtr">
                                <div class="row">
                                    <div class="controllerWrapper col-sm-12 col-md-11">
                                        <div class="buttonsWrapper col-sm-12 nav nav-pills">
                                            <div class="col-sm-2">
                                                    <div class="btnCtrl roundedCorner"><a  href="#collapse11" onclick="showcollapse11()">
                                                        <div class="iconBtn fa fa-suitcase"></div>
                                                    </a>
                                                    </div>
                                                <div class="title">Baggage</div>

                                            </div>
                                            <div class="col-sm-2">
                                                <div class="btnCtrl roundedCorner"> <a href="#collapse21" onclick="showcollapse21()">
                                                    <div class="iconBtn fa fa-cutlery"></div>
                                                    </a>
                                                </div>
                                                <div class="title">Meal</div>
                                            </div>
                                            <div class="col-sm-2">
                                                <div class="btnCtrl roundedCorner"> <a href="#collapse31" onclick="showcollapse31()">
                                                    <div class="iconBtn fa fa-futbol-o"></div>
                                                    </a>
                                                </div>
                                                <div class="title">Sport Equipment</div>
                                            </div>
                                            <div class="col-sm-2">
                                                <div class="btnCtrl roundedCorner"> <a href="#collapse41" onclick="showcollapse41()">
                                                    <div class="iconBtn fa fa-bed"></div>
                                                    </a>
                                                </div>
                                                <div class="title">Comfort Kit</div>
                                            </div>
                                            <div class="col-sm-2">
                                                <div class="btnCtrl roundedCorner"> <a href="#collapse51" onclick="showcollapse51()">
                                                    <div class="iconBtn fa fa-gift"></div>
                                                    </a>
                                                </div>
                                                <div class="title">Duty Free</div>
                                            </div>
                                        </div>
                                        <div class="contentWrapper col-sm-12">
                                            <%--div class="tab-content">--%>
                                                <div id="collapse11" style="visibility:hidden;" >

                                                    <div class="baggageAddonWrapper">
                                                        <div style="padding-top: 10px;">
                                                            <table>
                                                                <tbody>
                                                                    <tr>
                                                                        <td style="padding-top: 10px;">
                                                                            <div class="controlledCollapse collapse in">
                                                                                <dx:ASPxComboBox ID="cmbBaggage1" ClientInstanceName="cmbBaggage1" runat="server" class="Select" AutoPostBack="false" IncrementalFilteringMode="StartsWith" Width="100">
                                                                                </dx:ASPxComboBox>
                                                                            </div>
                                                                        </td>

                                                                        <td>
                                                                            <table class="addBaggageInput" style="margin-top: 10px;">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <div class="QtyWrapper">
                                                                                                <dx:ASPxSpinEdit ID="seBaggage1" ClientInstanceName="seBaggage1" runat="server" Number="1" NumberType="Integer" MinValue="1" MaxValue="50" Width="50px">
                                                                                                </dx:ASPxSpinEdit>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td style="padding-left: 10px;">
                                                                                            <button type="button" class="btn btn-danger" onclick="AddBaggage1()">Assign</button>
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
                                                <div id="collapse21" style="visibility:hidden;" >
                                                    <div class="baggageAddonWrapper" style="padding-top: 10px;">
                                                        <div class="mealAll">
                                                            <div class="showDropMeal collapse in" style="margin-bottom: 10px;">
                                                                <table>
                                                                    <tbody>

                                                                        <tr>
                                                                            <td style="padding-top: 10px;">
                                                                                <div class="controlledCollapse collapse in">

                                                                                    <dx:ASPxGridLookup ID="glMeals1" runat="server" KeyFieldName="SSRCode" Width="300px" EnableRowsCache="false" ClientInstanceName="glMeals1"
                                                                                        SelectionMode="Single" TextFormatString="{0} {1}" OnInit="glMeals1_Init">
                                                                                        <ClearButton DisplayMode="OnHover"></ClearButton>
                                                                                        <%--<ClientSideEvents Init="function (s,e){glMeals_Init(s,e);}" EndCallback="function (s,e){glMeals_EndCallBack(s,e);}" />--%>
                                                                                        <Columns>
                                                                                            <dx:GridViewDataColumn FieldName="Detail" />
                                                                                            <dx:GridViewDataColumn FieldName="Price" />
                                                                                        </Columns>
                                                                                        <GridViewProperties EnableRowsCache="false">
                                                                                            <Templates>
                                                                                                <DataRow>
                                                                                                    <div class="templateContainer">
                                                                                                        <div style="float: left; margin-right: 2px;">
                                                                                                            <img id="img" style="width: 100px; padding: 5px;" runat="server" alt='' src='<%#Eval("Images")%>' />
                                                                                                            <%--<dx:ASPxBinaryImage ID="PhotoBinaryImage" runat="server" Width="100px" Height="100px" Value='<%# Eval("Photo") %>' />--%>
                                                                                                        </div>
                                                                                                        <table class="templateTable">
                                                                                                            <tr>
                                                                                                                <td class="caption">Description:</td>
                                                                                                                <td><%# Eval("Detail")%></td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td class="caption">Price:</td>
                                                                                                                <td><%# Eval("Price")%></td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </div>
                                                                                                </DataRow>
                                                                                            </Templates>
                                                                                            <Settings ShowColumnHeaders="False" VerticalScrollableHeight="300" />
                                                                                            <SettingsPager Mode="ShowAllRecords" />
                                                                                            <SettingsBehavior EnableRowHotTrack="True" />
                                                                                        </GridViewProperties>
                                                                                    </dx:ASPxGridLookup>
                                                                                </div>
                                                                            </td>
                                                                            <td>
                                                                                <div class="QtyWrapper">
                                                                                    <dx:ASPxSpinEdit ID="seMeals1" ClientInstanceName="seMeals1" runat="server" Number="1" NumberType="Integer" MinValue="1" MaxValue="50" Width="50px">
                                                                                    </dx:ASPxSpinEdit>
                                                                                </div>
                                                                            </td>
                                                                            <td style="padding-left: 10px;">
                                                                                <button type="button" class="btn btn-danger" onclick="AddMeal1()">Assign</button>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="collapse31" style="visibility:hidden;" >
                                                    <div class="baggageAddonWrapper" style="padding-top: 10px;">

                                                        <div class="mealAll">

                                                            <div class="showDropMeal collapse in" style="margin-bottom: 10px;">
                                                                <table>
                                                                    <tbody>

                                                                        <tr>
                                                                            <td style="padding-top: 10px;">
                                                                                <div class="controlledCollapse collapse in">
                                                                                    <dx:ASPxComboBox ID="cmbSport1" ClientInstanceName="cmbSport1" runat="server" class="Select" AutoPostBack="false" IncrementalFilteringMode="StartsWith" Width="100">
                                                                                    </dx:ASPxComboBox>
                                                                                </div>

                                                                            </td>
                                                                            <td>
                                                                                <table class="addBaggageInput" style="margin-top: 10px;">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td style="padding-left: 10px;">
                                                                                                <dx:ASPxSpinEdit ID="seSport1" ClientInstanceName="seSport1" runat="server" Number="1" NumberType="Integer" MinValue="1" MaxValue="50" Width="50px">
                                                                                                </dx:ASPxSpinEdit>
                                                                                            </td>
                                                                                            <td style="padding-left: 10px;">
                                                                                                <button type="button" class="btn btn-danger" onclick="AddSport1()">Assign</button>
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
                                                <div id="collapse41" style="visibility:hidden;" >
                                                    <div class="baggageAddonWrapper">

                                                        <div style="padding-top: 10px;">
                                                            <table>

                                                                <tr>
                                                                    <td style="padding-top: 10px;">
                                                                        <div class="controlledCollapse collapse in">
                                                                            <dx:ASPxGridLookup ID="glComfort1" runat="server" KeyFieldName="SSRCode" Width="300px" EnableRowsCache="false" ClientInstanceName="glComfort1"
                                                                                SelectionMode="Single" TextFormatString="{0} {1}" OnInit="glComfort1_Init">
                                                                                <ClearButton DisplayMode="OnHover"></ClearButton>
                                                                                <Columns>
                                                                                    <dx:GridViewDataColumn FieldName="Detail" />
                                                                                    <dx:GridViewDataColumn FieldName="Price" />
                                                                                </Columns>
                                                                                <GridViewProperties EnableRowsCache="false">
                                                                                    <Templates>
                                                                                        <DataRow>
                                                                                            <div class="templateContainer">
                                                                                                <div style="float: left; margin-right: 2px;">
                                                                                                    <img id="img6" style="width: 100px; padding: 5px;" runat="server" alt='' src='<%#Eval("Images")%>' />
                                                                                                    <%--<dx:ASPxBinaryImage ID="PhotoBinaryImage" runat="server" Width="100px" Height="100px" Value='<%# Eval("Photo") %>' />--%>
                                                                                                </div>
                                                                                                <table class="templateTable">
                                                                                                    <tr>
                                                                                                        <td class="caption">Description:</td>
                                                                                                        <td><%# Eval("Detail")%></td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td class="caption">Price:</td>
                                                                                                        <td><%# Eval("Price")%></td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </div>
                                                                                        </DataRow>
                                                                                    </Templates>
                                                                                    <Settings ShowColumnHeaders="False" VerticalScrollableHeight="300" />
                                                                                    <SettingsPager Mode="ShowAllRecords" />
                                                                                    <SettingsBehavior EnableRowHotTrack="True" />
                                                                                </GridViewProperties>
                                                                            </dx:ASPxGridLookup>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <table class="addBaggageInput" style="margin-top: 10px;">
                                                                            <tr>
                                                                                <td>
                                                                                    <div class="QtyWrapper">
                                                                                        <dx:ASPxSpinEdit ID="seComfort1" ClientInstanceName="seComfort1" runat="server" Number="1" NumberType="Integer" MinValue="1" MaxValue="50" Width="50px">
                                                                                        </dx:ASPxSpinEdit>
                                                                                    </div>
                                                                                </td>
                                                                                <td style="padding-left: 10px;">
                                                                                    <button type="button" class="btn btn-danger" onclick="AddComfort1()">Assign</button>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="collapse51" style="visibility:hidden;" >
                                                    <div class="baggageAddonWrapper">
                                                        <div style="padding-top: 10px;">
                                                            <table>
                                                                <tr>
                                                                    <td style="padding-top: 10px;">
                                                                        <div class="controlledCollapse collapse in">
                                                                            <dx:ASPxGridLookup ID="glDuty1" runat="server" KeyFieldName="SSRCode" Width="300px" EnableRowsCache="false" ClientInstanceName="glDuty1"
                                                                                SelectionMode="Single" TextFormatString="{0} {1}" OnInit="glDuty1_Init">
                                                                                <ClearButton DisplayMode="OnHover"></ClearButton>
                                                                                <Columns>
                                                                                    <dx:GridViewDataColumn FieldName="Detail" />
                                                                                    <dx:GridViewDataColumn FieldName="Price" />
                                                                                </Columns>
                                                                                <GridViewProperties EnableRowsCache="false">
                                                                                    <Templates>
                                                                                        <DataRow>
                                                                                            <div class="templateContainer">
                                                                                                <div style="float: left; margin-right: 2px;">
                                                                                                    <img id="img7" style="width: 100px; padding: 5px;" runat="server" alt='' src='<%#Eval("Images")%>' />
                                                                                                    <%--<dx:ASPxBinaryImage ID="PhotoBinaryImage" runat="server" Width="100px" Height="100px" Value='<%# Eval("Photo") %>' />--%>
                                                                                                </div>
                                                                                                <table class="templateTable">
                                                                                                    <tr>
                                                                                                        <td class="caption">Description:</td>
                                                                                                        <td><%# Eval("Description")%></td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td class="caption">Price:</td>
                                                                                                        <td><%# Eval("Price")%></td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </div>
                                                                                        </DataRow>
                                                                                    </Templates>
                                                                                    <Settings ShowColumnHeaders="False" VerticalScrollableHeight="300" />
                                                                                    <SettingsPager Mode="ShowAllRecords" />
                                                                                    <SettingsBehavior EnableRowHotTrack="True" />
                                                                                </GridViewProperties>
                                                                            </dx:ASPxGridLookup>
                                                                        </div>
                                                                    </td>

                                                                    <td>
                                                                        <table class="addBaggageInput" style="margin-top: 10px;">
                                                                            <tr>
                                                                                <td>
                                                                                    <div class="QtyWrapper">
                                                                                        <dx:ASPxSpinEdit ID="seDuty1" ClientInstanceName="seDuty1" runat="server" Number="1" NumberType="Integer" MinValue="1" MaxValue="50" Width="50px">
                                                                                        </dx:ASPxSpinEdit>
                                                                                    </div>
                                                                                </td>
                                                                                <td style="padding-left: 10px;">
                                                                                    <button type="button" class="btn btn-danger" onclick="AddDuty1()">Assign</button>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div>
                                           <%-- </div>--%>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <br />
                            <div class="div">
                                <h4>Passenger List</h4>
                            </div>

                            <dx:ASPxGridView ID="gvPassenger1" runat="server" KeyFieldName="PassengerID" EnableRowsCache="false" EnableCallBacks="true"
                                ClientInstanceName="gvPassenger1" Width="100%" OnCustomCallback="gvPassenger1_CustomCallback">
                                <ClientSideEvents EndCallback="function(s, e) { gvPassenger_EndCallback(s, e);}" />
                                <Columns>
                                    <dx:GridViewDataColumn FieldName="PNR" Caption="RecordLocator" ReadOnly="true" />

                                    <dx:GridViewDataTextColumn FieldName="FirstName" Caption="First Name" ReadOnly="true">
                                        <PropertiesTextEdit>

                                            <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                        </PropertiesTextEdit>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="LastName" Caption="Last Name" ReadOnly="true" />
                                    <dx:GridViewDataComboBoxColumn FieldName="Meal" Caption="Meal" Width="120px">
                                        <PropertiesComboBox TextField="Detail" ValueField="SSRCode"
                                            ValueType="System.String">
                                        </PropertiesComboBox>
                                        <EditItemTemplate>
                                            <dx:ASPxGridLookup ID="glMealP11" runat="server" KeyFieldName="SSRCode" Width="120px" EnableRowsCache="false" ClientInstanceName="glMealP11"
                                                SelectionMode="Single" TextFormatString="{0} {1}">
                                                <ClearButton DisplayMode="OnHover"></ClearButton>
                                                <Columns>
                                                    <dx:GridViewDataColumn FieldName="Detail" />
                                                    <dx:GridViewDataColumn FieldName="Price" />
                                                </Columns>
                                                <GridViewProperties EnableRowsCache="false">
                                                    <Templates>
                                                        <DataRow>
                                                            <div class="templateContainer">
                                                                <div style="float: left; margin-right: 2px;">
                                                                    <img id="img" style="width: 100px; padding: 5px;" runat="server" alt='' src='<%#Eval("Images")%>' />
                                                                </div>
                                                                <table class="templateTable">
                                                                    <tr>
                                                                        <td class="caption">Description:</td>
                                                                        <td><%# Eval("Detail")%></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="caption">Price:</td>
                                                                        <td><%# Eval("Price")%></td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </DataRow>
                                                    </Templates>
                                                    <Settings ShowColumnHeaders="False" VerticalScrollableHeight="300" />
                                                    <SettingsPager Mode="ShowAllRecords" />
                                                    <SettingsBehavior EnableRowHotTrack="True" />
                                                </GridViewProperties>
                                            </dx:ASPxGridLookup>
                                        </EditItemTemplate>
                                    </dx:GridViewDataComboBoxColumn>
                                    <dx:GridViewDataComboBoxColumn FieldName="Baggage" Caption="Baggage" Width="120px">
                                        <PropertiesComboBox TextField="Detail" ValueField="SSRCode" ValueType="System.String">
                                        </PropertiesComboBox>
                                    </dx:GridViewDataComboBoxColumn>
                                    <dx:GridViewDataComboBoxColumn FieldName="Sport" Caption="Sport Equipment" Width="150px">
                                        <PropertiesComboBox TextField="Detail" ValueField="SSRCode" ValueType="System.String">
                                        </PropertiesComboBox>
                                    </dx:GridViewDataComboBoxColumn>
                                    <dx:GridViewDataComboBoxColumn FieldName="Comfort" Caption="Comfort Kit" Width="120px">
                                        <PropertiesComboBox TextField="Detail" ValueField="SSRCode"
                                            ValueType="System.String">
                                        </PropertiesComboBox>
                                        <EditItemTemplate>
                                            <dx:ASPxGridLookup ID="glComfortP11" runat="server" KeyFieldName="SSRCode" Width="120px" EnableRowsCache="false" ClientInstanceName="glComfortP11"
                                                SelectionMode="Single" TextFormatString="{0} {1}">
                                                <ClearButton DisplayMode="OnHover"></ClearButton>
                                                <Columns>
                                                    <dx:GridViewDataColumn FieldName="Detail" />
                                                    <dx:GridViewDataColumn FieldName="Price" />
                                                </Columns>
                                                <GridViewProperties EnableRowsCache="false">
                                                    <Templates>
                                                        <DataRow>
                                                            <div class="templateContainer">
                                                                <div style="float: left; margin-right: 2px;">
                                                                    <img id="img2" style="width: 100px; padding: 5px;" runat="server" alt='' src='<%#Eval("Images")%>' />
                                                                    <%--<dx:ASPxBinaryImage ID="PhotoBinaryImage" runat="server" Width="100px" Height="100px" Value='<%# Eval("Photo") %>' />--%>
                                                                </div>
                                                                <table class="templateTable">
                                                                    <tr>
                                                                        <td class="caption">Description:</td>
                                                                        <td><%# Eval("Detail")%></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="caption">Price:</td>
                                                                        <td><%# Eval("Price")%></td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </DataRow>
                                                    </Templates>
                                                    <Settings ShowColumnHeaders="False" VerticalScrollableHeight="300" />
                                                    <SettingsPager Mode="ShowAllRecords" />
                                                    <SettingsBehavior EnableRowHotTrack="True" />
                                                </GridViewProperties>
                                            </dx:ASPxGridLookup>
                                        </EditItemTemplate>
                                    </dx:GridViewDataComboBoxColumn>
                                    <dx:GridViewDataComboBoxColumn FieldName="Duty" Caption="Duty Free" Width="120px">
                                        <PropertiesComboBox TextField="Detail" ValueField="SSRCode"
                                            ValueType="System.String">
                                        </PropertiesComboBox>
                                        <EditItemTemplate>
                                            <dx:ASPxGridLookup ID="glDutyFreeP11" runat="server" KeyFieldName="SSRCode" Width="120px" EnableRowsCache="false" ClientInstanceName="glDutyFreeP11"
                                                SelectionMode="Single" TextFormatString="{0} {1}">
                                                <ClearButton DisplayMode="OnHover"></ClearButton>
                                                <Columns>
                                                    <dx:GridViewDataColumn FieldName="Detail" />
                                                    <dx:GridViewDataColumn FieldName="Price" />
                                                </Columns>
                                                <GridViewProperties EnableRowsCache="false">
                                                    <Templates>
                                                        <DataRow>
                                                            <div class="templateContainer">
                                                                <div style="float: left; margin-right: 2px;">
                                                                    <img id="img1" style="width: 100px; padding: 5px;" runat="server" alt='' src='<%#Eval("Images")%>' />
                                                                    <%--<dx:ASPxBinaryImage ID="PhotoBinaryImage" runat="server" Width="100px" Height="100px" Value='<%# Eval("Photo") %>' />--%>
                                                                </div>
                                                                <table class="templateTable">
                                                                    <tr>
                                                                        <td class="caption">Description:</td>
                                                                        <td><%# Eval("Detail")%></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="caption">Price:</td>
                                                                        <td><%# Eval("Price")%></td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </DataRow>
                                                    </Templates>
                                                    <Settings ShowColumnHeaders="False" VerticalScrollableHeight="300" />
                                                    <SettingsPager Mode="ShowAllRecords" />
                                                    <SettingsBehavior EnableRowHotTrack="True" />
                                                </GridViewProperties>
                                            </dx:ASPxGridLookup>
                                        </EditItemTemplate>

                                    </dx:GridViewDataComboBoxColumn>
                                </Columns>
                                <SettingsPager Mode="ShowPager" PageSize="10" Position="Top" />
                                <SettingsEditing Mode="Batch" />
                            </dx:ASPxGridView>
                        </div>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>

            <dx:TabPage Text="" Name="Tab2" Visible="true">
                <ContentCollection>
                    <dx:ContentControl ID="ContentControl2" runat="server">
                        <div id="home2">
                            <div class="tempCtr">
                                <div class="row">
                                    <div class="controllerWrapper col-sm-12 col-md-11">
                                        <div class="buttonsWrapper col-sm-12 nav nav-pills">
                                            <div class="col-sm-2">
                                                <div class="btnCtrl roundedCorner"> <a href="#collapse12" onclick="showcollapse12()">
                                                    <div class="iconBtn fa fa-suitcase"></div>
                                                    </a>

                                                </div>
                                                <div class="title">Baggage</div>
                                            </div>
                                            <div class="col-sm-2">
                                                <div class="btnCtrl roundedCorner"> <a href="#collapse22" onclick="showcollapse22()">
                                                    <div class="iconBtn fa fa-cutlery"></div>
                                                    </a>
                                                </div>
                                                <div class="title">Meal</div>
                                            </div>
                                            <div class="col-sm-2">
                                                <div class="btnCtrl roundedCorner"> <a href="#collapse32" onclick="showcollapse32()">
                                                    <div class="iconBtn fa fa-futbol-o"></div>
                                                    </a>
                                                </div>
                                                <div class="title">Sport Equipment</div>
                                            </div>
                                            <div class="col-sm-2">
                                                <div class="btnCtrl roundedCorner"> <a href="#collapse42" onclick="showcollapse42()">
                                                    <div class="iconBtn fa fa-bed"></div>
                                                    </a>
                                                </div>
                                                <div class="title">Comfort Kit</div>
                                            </div>
                                            <div class="col-sm-2">
                                                <div class="btnCtrl roundedCorner"> <a href="#collapse52" onclick="showcollapse52()">
                                                    <div class="iconBtn fa fa-gift"></div>
                                                    </a>
                                                </div>
                                                <div class="title">Duty Free</div>
                                            </div>
                                        </div>
                                        <div class="contentWrapper col-sm-12">
                                            <div id="collapse12" style="visibility:hidden;">

                                                <div class="baggageAddonWrapper">
                                                    <div style="padding-top: 10px;">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td style="padding-top: 10px;">
                                                                        <div class="controlledCollapse collapse in">
                                                                            <dx:ASPxComboBox ID="cmbBaggage2" ClientInstanceName="cmbBaggage2" runat="server" class="Select" AutoPostBack="false" IncrementalFilteringMode="StartsWith" Width="100">
                                                                            </dx:ASPxComboBox>
                                                                        </div>
                                                                    </td>

                                                                    <td>
                                                                        <table class="addBaggageInput" style="margin-top: 10px;">
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div class="QtyWrapper">
                                                                                            <dx:ASPxSpinEdit ID="seBaggage2" ClientInstanceName="seBaggage2" runat="server" Number="1" NumberType="Integer" MinValue="1" MaxValue="50" Width="50px">
                                                                                            </dx:ASPxSpinEdit>
                                                                                        </div>
                                                                                    </td>
                                                                                    <td style="padding-left: 10px;">
                                                                                        <button type="button" class="btn btn-danger" onclick="AddBaggage2()">Assign</button>
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
                                            <div id="collapse22" style="visibility:hidden;">
                                                <div class="baggageAddonWrapper" style="padding-top: 10px;">
                                                    <div class="mealAll">
                                                        <div class="showDropMeal collapse in" style="margin-bottom: 10px;">
                                                            <table>
                                                                <tbody>

                                                                    <tr>
                                                                        <td style="padding-top: 10px;">
                                                                            <div class="controlledCollapse collapse in">

                                                                                <dx:ASPxGridLookup ID="glMeals2" runat="server" KeyFieldName="SSRCode" Width="300px"
                                                                                    EnableRowsCache="false" ClientInstanceName="glMeals2"
                                                                                    SelectionMode="Single" TextFormatString="{0} {1}" OnInit="glMeals2_Init">
                                                                                    <ClearButton DisplayMode="OnHover"></ClearButton>
                                                                                    <Columns>
                                                                                        <dx:GridViewDataColumn FieldName="Detail" />
                                                                                        <dx:GridViewDataColumn FieldName="Price" />
                                                                                    </Columns>
                                                                                    <GridViewProperties EnableRowsCache="false">
                                                                                        <Templates>
                                                                                            <DataRow>
                                                                                                <div class="templateContainer">
                                                                                                    <div style="float: left; margin-right: 2px;">
                                                                                                        <img id="img" style="width: 100px; padding: 5px;" runat="server" alt='' src='<%#Eval("Images")%>' />
                                                                                                    </div>
                                                                                                    <table class="templateTable">
                                                                                                        <tr>
                                                                                                            <td class="caption">Description:</td>
                                                                                                            <td><%# Eval("Detail")%></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="caption">Price:</td>
                                                                                                            <td><%# Eval("Price")%></td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </div>
                                                                                            </DataRow>
                                                                                        </Templates>
                                                                                        <Settings ShowColumnHeaders="False" VerticalScrollableHeight="300" />
                                                                                        <SettingsPager Mode="ShowAllRecords" />
                                                                                        <SettingsBehavior EnableRowHotTrack="True" />
                                                                                    </GridViewProperties>
                                                                                </dx:ASPxGridLookup>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div class="QtyWrapper">
                                                                                <dx:ASPxSpinEdit ID="seMeals2" ClientInstanceName="seMeals2" runat="server" Number="1" NumberType="Integer" MinValue="1" MaxValue="50" Width="50px">
                                                                                </dx:ASPxSpinEdit>
                                                                            </div>
                                                                        </td>
                                                                        <td style="padding-left: 10px;">
                                                                            <button type="button" class="btn btn-danger" onclick="AddMeal2()">Assign</button>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="collapse32" style="visibility:hidden;">
                                                <div class="baggageAddonWrapper" style="padding-top: 10px;">

                                                    <div class="mealAll">

                                                        <div class="showDropMeal collapse in" style="margin-bottom: 10px;">
                                                            <table>
                                                                <tbody>

                                                                    <tr>
                                                                        <td style="padding-top: 10px;">
                                                                            <div class="controlledCollapse collapse in">
                                                                                <dx:ASPxComboBox ID="cmbSport2" ClientInstanceName="cmbSport2" runat="server" class="Select" AutoPostBack="false" IncrementalFilteringMode="StartsWith" Width="100">
                                                                                </dx:ASPxComboBox>
                                                                            </div>

                                                                        </td>
                                                                        <td>
                                                                            <table class="addBaggageInput" style="margin-top: 10px;">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td style="padding-left: 10px;">
                                                                                            <dx:ASPxSpinEdit ID="seSport2" ClientInstanceName="seSport2" runat="server" Number="1" NumberType="Integer" MinValue="1" MaxValue="50" Width="50px">
                                                                                            </dx:ASPxSpinEdit>
                                                                                        </td>
                                                                                        <td style="padding-left: 10px;">
                                                                                            <button type="button" class="btn btn-danger" onclick="AddSport2()">Assign</button>
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
                                            <div id="collapse42" style="visibility:hidden;">
                                                <div class="baggageAddonWrapper">

                                                    <div style="padding-top: 10px;">
                                                        <table>

                                                            <tr>
                                                                <td style="padding-top: 10px;">
                                                                    <div class="controlledCollapse collapse in">
                                                                        <dx:ASPxGridLookup ID="glComfort2" runat="server" KeyFieldName="SSRCode"
                                                                            Width="300px" EnableRowsCache="false" ClientInstanceName="glComfort2"
                                                                            SelectionMode="Single" TextFormatString="{0} {1}" OnInit="glComfort2_Init">
                                                                            <ClearButton DisplayMode="OnHover"></ClearButton>
                                                                            <Columns>
                                                                                <dx:GridViewDataColumn FieldName="Detail" />
                                                                                <dx:GridViewDataColumn FieldName="Price" />
                                                                            </Columns>
                                                                            <GridViewProperties EnableRowsCache="false">
                                                                                <Templates>
                                                                                    <DataRow>
                                                                                        <div class="templateContainer">
                                                                                            <div style="float: left; margin-right: 2px;">
                                                                                                <img id="img6" style="width: 100px; padding: 5px;" runat="server" alt='' src='<%#Eval("Images")%>' />
                                                                                                <%--<dx:ASPxBinaryImage ID="PhotoBinaryImage" runat="server" Width="100px" Height="100px" Value='<%# Eval("Photo") %>' />--%>
                                                                                            </div>
                                                                                            <table class="templateTable">
                                                                                                <tr>
                                                                                                    <td class="caption">Description:</td>
                                                                                                    <td><%# Eval("Detail")%></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td class="caption">Price:</td>
                                                                                                    <td><%# Eval("Price")%></td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </DataRow>
                                                                                </Templates>
                                                                                <Settings ShowColumnHeaders="False" VerticalScrollableHeight="300" />
                                                                                <SettingsPager Mode="ShowAllRecords" />
                                                                                <SettingsBehavior EnableRowHotTrack="True" />
                                                                            </GridViewProperties>
                                                                        </dx:ASPxGridLookup>
                                                                    </div>
                                                                </td>
                                                                <td>
                                                                    <table class="addBaggageInput" style="margin-top: 10px;">
                                                                        <tr>
                                                                            <td>
                                                                                <div class="QtyWrapper">
                                                                                    <dx:ASPxSpinEdit ID="seComfort2" ClientInstanceName="seComfort2" runat="server" Number="1" NumberType="Integer" MinValue="1" MaxValue="50" Width="50px">
                                                                                    </dx:ASPxSpinEdit>
                                                                                </div>
                                                                            </td>
                                                                            <td style="padding-left: 10px;">
                                                                                <button type="button" class="btn btn-danger" onclick="AddComfort2()">Assign</button>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="collapse52" style="visibility:hidden;">
                                                <div class="baggageAddonWrapper">
                                                    <div style="padding-top: 10px;">
                                                        <table>
                                                            <tr>
                                                                <td style="padding-top: 10px;">
                                                                    <div class="controlledCollapse collapse in">
                                                                        <dx:ASPxGridLookup ID="glDuty2" runat="server" KeyFieldName="SSRCode"
                                                                            Width="300px" EnableRowsCache="false" ClientInstanceName="glDuty2"
                                                                            SelectionMode="Single" TextFormatString="{0} {1}" OnInit="glDuty2_Init">
                                                                            <ClearButton DisplayMode="OnHover"></ClearButton>
                                                                            <Columns>
                                                                                <dx:GridViewDataColumn FieldName="Detail" />
                                                                                <dx:GridViewDataColumn FieldName="Price" />
                                                                            </Columns>
                                                                            <GridViewProperties EnableRowsCache="false">
                                                                                <Templates>
                                                                                    <DataRow>
                                                                                        <div class="templateContainer">
                                                                                            <div style="float: left; margin-right: 2px;">
                                                                                                <img id="img7" style="width: 100px; padding: 5px;" runat="server" alt='' src='<%#Eval("Images")%>' />
                                                                                                <%--<dx:ASPxBinaryImage ID="PhotoBinaryImage" runat="server" Width="100px" Height="100px" Value='<%# Eval("Photo") %>' />--%>
                                                                                            </div>
                                                                                            <table class="templateTable">
                                                                                                <tr>
                                                                                                    <td class="caption">Description:</td>
                                                                                                    <td><%# Eval("Description")%></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td class="caption">Price:</td>
                                                                                                    <td><%# Eval("Price")%></td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </DataRow>
                                                                                </Templates>
                                                                                <Settings ShowColumnHeaders="False" VerticalScrollableHeight="300" />
                                                                                <SettingsPager Mode="ShowAllRecords" />
                                                                                <SettingsBehavior EnableRowHotTrack="True" />
                                                                            </GridViewProperties>
                                                                        </dx:ASPxGridLookup>
                                                                    </div>
                                                                </td>

                                                                <td>
                                                                    <table class="addBaggageInput" style="margin-top: 10px;">
                                                                        <tr>
                                                                            <td>
                                                                                <div class="QtyWrapper">
                                                                                    <dx:ASPxSpinEdit ID="seDuty2" runat="server" ClientInstanceName="seDuty2" Number="1" NumberType="Integer" MinValue="1" MaxValue="50" Width="50px">
                                                                                    </dx:ASPxSpinEdit>
                                                                                </div>
                                                                            </td>
                                                                            <td style="padding-left: 10px;">
                                                                                <button type="button" class="btn btn-danger" onclick="AddDuty2()">Assign</button>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <br />
                            <div class="div">
                                <h4>Passenger List</h4>
                            </div>

                            <dx:ASPxGridView ID="gvPassenger2" runat="server" KeyFieldName="PassengerID" EnableRowsCache="false" EnableCallBacks="true"
                                ClientInstanceName="gvPassenger2" Width="100%" OnCustomCallback="gvPassenger2_CustomCallback">
                                <ClientSideEvents EndCallback="function(s, e) { gvPassenger2_EndCallback(s, e);}" />
                                <Columns>
                                    <dx:GridViewDataColumn FieldName="PNR" Caption="RecordLocator" ReadOnly="true" />

                                    <dx:GridViewDataTextColumn FieldName="FirstName" Caption="First Name" ReadOnly="true">
                                        <PropertiesTextEdit>

                                            <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                        </PropertiesTextEdit>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="LastName" Caption="Last Name" ReadOnly="true" />
                                    <dx:GridViewDataComboBoxColumn FieldName="Meal" Caption="Meal" Width="120px">
                                        <PropertiesComboBox TextField="Detail" ValueField="SSRCode"
                                            ValueType="System.String">
                                        </PropertiesComboBox>
                                        <EditItemTemplate>
                                            <dx:ASPxGridLookup ID="glMealP2" runat="server" KeyFieldName="SSRCode" Width="120px" EnableRowsCache="false" ClientInstanceName="glMealP2"
                                                SelectionMode="Single" TextFormatString="{0} {1}">
                                                <ClearButton DisplayMode="OnHover"></ClearButton>
                                                <Columns>
                                                    <dx:GridViewDataColumn FieldName="Detail" />
                                                    <dx:GridViewDataColumn FieldName="Price" />
                                                </Columns>
                                                <GridViewProperties EnableRowsCache="false">
                                                    <Templates>
                                                        <DataRow>
                                                            <div class="templateContainer">
                                                                <div style="float: left; margin-right: 2px;">
                                                                    <img id="img" style="width: 100px; padding: 5px;" runat="server" alt='' src='<%#Eval("Images")%>' />
                                                                </div>
                                                                <table class="templateTable">
                                                                    <tr>
                                                                        <td class="caption">Description:</td>
                                                                        <td><%# Eval("Detail")%></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="caption">Price:</td>
                                                                        <td><%# Eval("Price")%></td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </DataRow>
                                                    </Templates>
                                                    <Settings ShowColumnHeaders="False" VerticalScrollableHeight="300" />
                                                    <SettingsPager Mode="ShowAllRecords" />
                                                    <SettingsBehavior EnableRowHotTrack="True" />
                                                </GridViewProperties>
                                            </dx:ASPxGridLookup>
                                        </EditItemTemplate>
                                    </dx:GridViewDataComboBoxColumn>
                                    <dx:GridViewDataComboBoxColumn FieldName="Baggage" Caption="Baggage" Width="120px">
                                        <PropertiesComboBox TextField="Detail" ValueField="SSRCode" ValueType="System.String">
                                        </PropertiesComboBox>
                                    </dx:GridViewDataComboBoxColumn>
                                    <dx:GridViewDataComboBoxColumn FieldName="Sport" Caption="Sport Equipment" Width="150px">
                                        <PropertiesComboBox TextField="Detail" ValueField="SSRCode" ValueType="System.String">
                                        </PropertiesComboBox>
                                    </dx:GridViewDataComboBoxColumn>
                                    <dx:GridViewDataComboBoxColumn FieldName="Comfort" Caption="Comfort Kit" Width="120px">
                                        <PropertiesComboBox TextField="Detail" ValueField="SSRCode"
                                            ValueType="System.String">
                                        </PropertiesComboBox>
                                        <EditItemTemplate>
                                            <dx:ASPxGridLookup ID="glComfortP2" runat="server" KeyFieldName="SSRCode" Width="120px" EnableRowsCache="false" ClientInstanceName="glComfortP2"
                                                SelectionMode="Single" TextFormatString="{0} {1}">
                                                <ClearButton DisplayMode="OnHover"></ClearButton>
                                                <Columns>
                                                    <dx:GridViewDataColumn FieldName="Detail" />
                                                    <dx:GridViewDataColumn FieldName="Price" />
                                                </Columns>
                                                <GridViewProperties EnableRowsCache="false">
                                                    <Templates>
                                                        <DataRow>
                                                            <div class="templateContainer">
                                                                <div style="float: left; margin-right: 2px;">
                                                                    <img id="img2" style="width: 100px; padding: 5px;" runat="server" alt='' src='<%#Eval("Images")%>' />
                                                                    <%--<dx:ASPxBinaryImage ID="PhotoBinaryImage" runat="server" Width="100px" Height="100px" Value='<%# Eval("Photo") %>' />--%>
                                                                </div>
                                                                <table class="templateTable">
                                                                    <tr>
                                                                        <td class="caption">Description:</td>
                                                                        <td><%# Eval("Detail")%></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="caption">Price:</td>
                                                                        <td><%# Eval("Price")%></td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </DataRow>
                                                    </Templates>
                                                    <Settings ShowColumnHeaders="False" VerticalScrollableHeight="300" />
                                                    <SettingsPager Mode="ShowAllRecords" />
                                                    <SettingsBehavior EnableRowHotTrack="True" />
                                                </GridViewProperties>
                                            </dx:ASPxGridLookup>
                                        </EditItemTemplate>
                                    </dx:GridViewDataComboBoxColumn>
                                    <dx:GridViewDataComboBoxColumn FieldName="Duty" Caption="Duty Free" Width="120px">
                                        <PropertiesComboBox TextField="Detail" ValueField="SSRCode"
                                            ValueType="System.String">
                                        </PropertiesComboBox>
                                        <EditItemTemplate>
                                            <dx:ASPxGridLookup ID="glDutyFreeP2" runat="server" KeyFieldName="SSRCode" Width="120px" EnableRowsCache="false" ClientInstanceName="glDutyFreeP2"
                                                SelectionMode="Single" TextFormatString="{0} {1}">
                                                <ClearButton DisplayMode="OnHover"></ClearButton>
                                                <Columns>
                                                    <dx:GridViewDataColumn FieldName="Detail" />
                                                    <dx:GridViewDataColumn FieldName="Price" />
                                                </Columns>
                                                <GridViewProperties EnableRowsCache="false">
                                                    <Templates>
                                                        <DataRow>
                                                            <div class="templateContainer">
                                                                <div style="float: left; margin-right: 2px;">
                                                                    <img id="img1" style="width: 100px; padding: 5px;" runat="server" alt='' src='<%#Eval("Images")%>' />
                                                                    <%--<dx:ASPxBinaryImage ID="PhotoBinaryImage" runat="server" Width="100px" Height="100px" Value='<%# Eval("Photo") %>' />--%>
                                                                </div>
                                                                <table class="templateTable">
                                                                    <tr>
                                                                        <td class="caption">Description:</td>
                                                                        <td><%# Eval("Detail")%></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="caption">Price:</td>
                                                                        <td><%# Eval("Price")%></td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </DataRow>
                                                    </Templates>
                                                    <Settings ShowColumnHeaders="False" VerticalScrollableHeight="300" />
                                                    <SettingsPager Mode="ShowAllRecords" />
                                                    <SettingsBehavior EnableRowHotTrack="True" />
                                                </GridViewProperties>
                                            </dx:ASPxGridLookup>
                                        </EditItemTemplate>

                                    </dx:GridViewDataComboBoxColumn>
                                </Columns>
                                <SettingsPager Mode="ShowPager" PageSize="10" Position="Top" />
                                <SettingsEditing Mode="Batch" />
                            </dx:ASPxGridView>
                        </div>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>

            <dx:TabPage Text="" Name="Tab21" Visible="true">
                <ContentCollection>
                    <dx:ContentControl ID="ContentControl4" runat="server">
                        <div id="home21">
                            <div class="tempCtr">
                                <div class="row">
                                    <div class="controllerWrapper col-sm-12 col-md-11">
                                        <div class="buttonsWrapper col-sm-12 nav nav-pills">
                                            <div class="col-sm-2">
                                                <div class="btnCtrl roundedCorner"> <a href="#collapse121" onclick="showcollapse121()">
                                                    <div class="iconBtn fa fa-suitcase"></div>
                                                    </a>

                                                </div>
                                                <div class="title">Baggage</div>
                                            </div>
                                            <div class="col-sm-2">
                                                <div class="btnCtrl roundedCorner"> <a href="#collapse221" onclick="showcollapse221()">
                                                    <div class="iconBtn fa fa-cutlery"></div>
                                                    </a>
                                                </div>
                                                <div class="title">Meal</div>
                                            </div>
                                            <div class="col-sm-2">
                                                <div class="btnCtrl roundedCorner"> <a href="#collapse321" onclick="showcollapse321()">
                                                    <div class="iconBtn fa fa-futbol-o"></div>
                                                    </a>
                                                </div>
                                                <div class="title">Sport Equipment</div>
                                            </div>
                                            <div class="col-sm-2">
                                                <div class="btnCtrl roundedCorner"> <a href="#collapse421" onclick="showcollapse421()">
                                                    <div class="iconBtn fa fa-bed"></div>
                                                    </a>
                                                </div>
                                                <div class="title">Comfort Kit</div>
                                            </div>
                                            <div class="col-sm-2">
                                                <div class="btnCtrl roundedCorner"> <a href="#collapse521" onclick="showcollapse521()">
                                                    <div class="iconBtn fa fa-gift"></div>
                                                    </a>
                                                </div>
                                                <div class="title">Duty Free</div>
                                            </div>
                                        </div>
                                        <div class="contentWrapper col-sm-12">
                                            <div id="collapse121" style="visibility:hidden;">

                                                <div class="baggageAddonWrapper">
                                                    <div style="padding-top: 10px;">
                                                        <table>
                                                            <tbody>
                                                                <tr>
                                                                    <td style="padding-top: 10px;">
                                                                        <div class="controlledCollapse collapse in">
                                                                            <dx:ASPxComboBox ID="cmbBaggage21" ClientInstanceName="cmbBaggage21" runat="server" class="Select" AutoPostBack="false" IncrementalFilteringMode="StartsWith" Width="100">
                                                                            </dx:ASPxComboBox>
                                                                        </div>
                                                                    </td>

                                                                    <td>
                                                                        <table class="addBaggageInput" style="margin-top: 10px;">
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div class="QtyWrapper">
                                                                                            <dx:ASPxSpinEdit ID="seBaggage21" ClientInstanceName="seBaggage21" runat="server" Number="1" NumberType="Integer" MinValue="1" MaxValue="50" Width="50px">
                                                                                            </dx:ASPxSpinEdit>
                                                                                        </div>
                                                                                    </td>
                                                                                    <td style="padding-left: 10px;">
                                                                                        <button type="button" class="btn btn-danger" onclick="AddBaggage21()">Assign</button>
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
                                            <div id="collapse221" style="visibility:hidden;">
                                                <div class="baggageAddonWrapper" style="padding-top: 10px;">
                                                    <div class="mealAll">
                                                        <div class="showDropMeal collapse in" style="margin-bottom: 10px;">
                                                            <table>
                                                                <tbody>

                                                                    <tr>
                                                                        <td style="padding-top: 10px;">
                                                                            <div class="controlledCollapse collapse in">

                                                                                <dx:ASPxGridLookup ID="glMeals21" runat="server" KeyFieldName="SSRCode" Width="300px"
                                                                                    EnableRowsCache="false" ClientInstanceName="glMeals21"
                                                                                    SelectionMode="Single" TextFormatString="{0} {1}" OnInit="glMeals21_Init">
                                                                                    <ClearButton DisplayMode="OnHover"></ClearButton>
                                                                                    <Columns>
                                                                                        <dx:GridViewDataColumn FieldName="Detail" />
                                                                                        <dx:GridViewDataColumn FieldName="Price" />
                                                                                    </Columns>
                                                                                    <GridViewProperties EnableRowsCache="false">
                                                                                        <Templates>
                                                                                            <DataRow>
                                                                                                <div class="templateContainer">
                                                                                                    <div style="float: left; margin-right: 2px;">
                                                                                                        <img id="img" style="width: 100px; padding: 5px;" runat="server" alt='' src='<%#Eval("Images")%>' />
                                                                                                    </div>
                                                                                                    <table class="templateTable">
                                                                                                        <tr>
                                                                                                            <td class="caption">Description:</td>
                                                                                                            <td><%# Eval("Detail")%></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="caption">Price:</td>
                                                                                                            <td><%# Eval("Price")%></td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </div>
                                                                                            </DataRow>
                                                                                        </Templates>
                                                                                        <Settings ShowColumnHeaders="False" VerticalScrollableHeight="300" />
                                                                                        <SettingsPager Mode="ShowAllRecords" />
                                                                                        <SettingsBehavior EnableRowHotTrack="True" />
                                                                                    </GridViewProperties>
                                                                                </dx:ASPxGridLookup>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div class="QtyWrapper">
                                                                                <dx:ASPxSpinEdit ID="seMeals21" ClientInstanceName="seMeals21" runat="server" Number="1" NumberType="Integer" MinValue="1" MaxValue="50" Width="50px">
                                                                                </dx:ASPxSpinEdit>
                                                                            </div>
                                                                        </td>
                                                                        <td style="padding-left: 10px;">
                                                                            <button type="button" class="btn btn-danger" onclick="AddMeal21()">Assign</button>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="collapse321" style="visibility:hidden;">
                                                <div class="baggageAddonWrapper" style="padding-top: 10px;">

                                                    <div class="mealAll">

                                                        <div class="showDropMeal collapse in" style="margin-bottom: 10px;">
                                                            <table>
                                                                <tbody>

                                                                    <tr>
                                                                        <td style="padding-top: 10px;">
                                                                            <div class="controlledCollapse collapse in">
                                                                                <dx:ASPxComboBox ID="cmbSport21" ClientInstanceName="cmbSport21" runat="server" class="Select" AutoPostBack="false" IncrementalFilteringMode="StartsWith" Width="100">
                                                                                </dx:ASPxComboBox>
                                                                            </div>

                                                                        </td>
                                                                        <td>
                                                                            <table class="addBaggageInput" style="margin-top: 10px;">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td style="padding-left: 10px;">
                                                                                            <dx:ASPxSpinEdit ID="seSport21" ClientInstanceName="seSport21" runat="server" Number="1" NumberType="Integer" MinValue="1" MaxValue="50" Width="50px">
                                                                                            </dx:ASPxSpinEdit>
                                                                                        </td>
                                                                                        <td style="padding-left: 10px;">
                                                                                            <button type="button" class="btn btn-danger" onclick="AddSport21()">Assign</button>
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
                                            <div id="collapse421" style="visibility:hidden;">
                                                <div class="baggageAddonWrapper">

                                                    <div style="padding-top: 10px;">
                                                        <table>

                                                            <tr>
                                                                <td style="padding-top: 10px;">
                                                                    <div class="controlledCollapse collapse in">
                                                                        <dx:ASPxGridLookup ID="glComfort21" runat="server" KeyFieldName="SSRCode"
                                                                            Width="300px" EnableRowsCache="false" ClientInstanceName="glComfort21"
                                                                            SelectionMode="Single" TextFormatString="{0} {1}" OnInit="glComfort21_Init">
                                                                            <ClearButton DisplayMode="OnHover"></ClearButton>
                                                                            <Columns>
                                                                                <dx:GridViewDataColumn FieldName="Detail" />
                                                                                <dx:GridViewDataColumn FieldName="Price" />
                                                                            </Columns>
                                                                            <GridViewProperties EnableRowsCache="false">
                                                                                <Templates>
                                                                                    <DataRow>
                                                                                        <div class="templateContainer">
                                                                                            <div style="float: left; margin-right: 2px;">
                                                                                                <img id="img6" style="width: 100px; padding: 5px;" runat="server" alt='' src='<%#Eval("Images")%>' />
                                                                                                <%--<dx:ASPxBinaryImage ID="PhotoBinaryImage" runat="server" Width="100px" Height="100px" Value='<%# Eval("Photo") %>' />--%>
                                                                                            </div>
                                                                                            <table class="templateTable">
                                                                                                <tr>
                                                                                                    <td class="caption">Description:</td>
                                                                                                    <td><%# Eval("Detail")%></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td class="caption">Price:</td>
                                                                                                    <td><%# Eval("Price")%></td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </DataRow>
                                                                                </Templates>
                                                                                <Settings ShowColumnHeaders="False" VerticalScrollableHeight="300" />
                                                                                <SettingsPager Mode="ShowAllRecords" />
                                                                                <SettingsBehavior EnableRowHotTrack="True" />
                                                                            </GridViewProperties>
                                                                        </dx:ASPxGridLookup>
                                                                    </div>
                                                                </td>
                                                                <td>
                                                                    <table class="addBaggageInput" style="margin-top: 10px;">
                                                                        <tr>
                                                                            <td>
                                                                                <div class="QtyWrapper">
                                                                                    <dx:ASPxSpinEdit ID="seComfort21" ClientInstanceName="seComfort21" runat="server" Number="1" NumberType="Integer" MinValue="1" MaxValue="50" Width="50px">
                                                                                    </dx:ASPxSpinEdit>
                                                                                </div>
                                                                            </td>
                                                                            <td style="padding-left: 10px;">
                                                                                <button type="button" class="btn btn-danger" onclick="AddComfort21()">Assign</button>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="collapse521" style="visibility:hidden;">
                                                <div class="baggageAddonWrapper">
                                                    <div style="padding-top: 10px;">
                                                        <table>
                                                            <tr>
                                                                <td style="padding-top: 10px;">
                                                                    <div class="controlledCollapse collapse in">
                                                                        <dx:ASPxGridLookup ID="glDuty21" runat="server" KeyFieldName="SSRCode"
                                                                            Width="300px" EnableRowsCache="false" ClientInstanceName="glDuty21"
                                                                            SelectionMode="Single" TextFormatString="{0} {1}" OnInit="glDuty21_Init">
                                                                            <ClearButton DisplayMode="OnHover"></ClearButton>
                                                                            <Columns>
                                                                                <dx:GridViewDataColumn FieldName="Detail" />
                                                                                <dx:GridViewDataColumn FieldName="Price" />
                                                                            </Columns>
                                                                            <GridViewProperties EnableRowsCache="false">
                                                                                <Templates>
                                                                                    <DataRow>
                                                                                        <div class="templateContainer">
                                                                                            <div style="float: left; margin-right: 2px;">
                                                                                                <img id="img7" style="width: 100px; padding: 5px;" runat="server" alt='' src='<%#Eval("Images")%>' />
                                                                                                <%--<dx:ASPxBinaryImage ID="PhotoBinaryImage" runat="server" Width="100px" Height="100px" Value='<%# Eval("Photo") %>' />--%>
                                                                                            </div>
                                                                                            <table class="templateTable">
                                                                                                <tr>
                                                                                                    <td class="caption">Description:</td>
                                                                                                    <td><%# Eval("Description")%></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td class="caption">Price:</td>
                                                                                                    <td><%# Eval("Price")%></td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </DataRow>
                                                                                </Templates>
                                                                                <Settings ShowColumnHeaders="False" VerticalScrollableHeight="300" />
                                                                                <SettingsPager Mode="ShowAllRecords" />
                                                                                <SettingsBehavior EnableRowHotTrack="True" />
                                                                            </GridViewProperties>
                                                                        </dx:ASPxGridLookup>
                                                                    </div>
                                                                </td>

                                                                <td>
                                                                    <table class="addBaggageInput" style="margin-top: 10px;">
                                                                        <tr>
                                                                            <td>
                                                                                <div class="QtyWrapper">
                                                                                    <dx:ASPxSpinEdit ID="seDuty21" runat="server" ClientInstanceName="seDuty21" Number="1" NumberType="Integer" MinValue="1" MaxValue="50" Width="50px">
                                                                                    </dx:ASPxSpinEdit>
                                                                                </div>
                                                                            </td>
                                                                            <td style="padding-left: 10px;">
                                                                                <button type="button" class="btn btn-danger" onclick="AddDuty21()">Assign</button>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <br />
                            <div class="div">
                                <h4>Passenger List</h4>
                            </div>

                            <dx:ASPxGridView ID="gvPassenger21" runat="server" KeyFieldName="PassengerID" EnableRowsCache="false" EnableCallBacks="true"
                                ClientInstanceName="gvPassenger21" Width="100%" OnCustomCallback="gvPassenger21_CustomCallback">
                                <ClientSideEvents EndCallback="function(s, e) { gvPassenger2_EndCallback(s, e);}" />
                                <Columns>
                                    <dx:GridViewDataColumn FieldName="PNR" Caption="RecordLocator" ReadOnly="true" />

                                    <dx:GridViewDataTextColumn FieldName="FirstName" Caption="First Name" ReadOnly="true">
                                        <PropertiesTextEdit>

                                            <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                        </PropertiesTextEdit>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="LastName" Caption="Last Name" ReadOnly="true" />
                                    <dx:GridViewDataComboBoxColumn FieldName="Meal" Caption="Meal" Width="120px">
                                        <PropertiesComboBox TextField="Detail" ValueField="SSRCode"
                                            ValueType="System.String">
                                        </PropertiesComboBox>
                                        <EditItemTemplate>
                                            <dx:ASPxGridLookup ID="glMealP21" runat="server" KeyFieldName="SSRCode" Width="120px" EnableRowsCache="false" ClientInstanceName="glMealP21"
                                                SelectionMode="Single" TextFormatString="{0} {1}">
                                                <ClearButton DisplayMode="OnHover"></ClearButton>
                                                <Columns>
                                                    <dx:GridViewDataColumn FieldName="Detail" />
                                                    <dx:GridViewDataColumn FieldName="Price" />
                                                </Columns>
                                                <GridViewProperties EnableRowsCache="false">
                                                    <Templates>
                                                        <DataRow>
                                                            <div class="templateContainer">
                                                                <div style="float: left; margin-right: 2px;">
                                                                    <img id="img" style="width: 100px; padding: 5px;" runat="server" alt='' src='<%#Eval("Images")%>' />
                                                                </div>
                                                                <table class="templateTable">
                                                                    <tr>
                                                                        <td class="caption">Description:</td>
                                                                        <td><%# Eval("Detail")%></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="caption">Price:</td>
                                                                        <td><%# Eval("Price")%></td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </DataRow>
                                                    </Templates>
                                                    <Settings ShowColumnHeaders="False" VerticalScrollableHeight="300" />
                                                    <SettingsPager Mode="ShowAllRecords" />
                                                    <SettingsBehavior EnableRowHotTrack="True" />
                                                </GridViewProperties>
                                            </dx:ASPxGridLookup>
                                        </EditItemTemplate>
                                    </dx:GridViewDataComboBoxColumn>
                                    <dx:GridViewDataComboBoxColumn FieldName="Baggage" Caption="Baggage" Width="120px">
                                        <PropertiesComboBox TextField="Detail" ValueField="SSRCode" ValueType="System.String">
                                        </PropertiesComboBox>
                                    </dx:GridViewDataComboBoxColumn>
                                    <dx:GridViewDataComboBoxColumn FieldName="Sport" Caption="Sport Equipment" Width="150px">
                                        <PropertiesComboBox TextField="Detail" ValueField="SSRCode" ValueType="System.String">
                                        </PropertiesComboBox>
                                    </dx:GridViewDataComboBoxColumn>
                                    <dx:GridViewDataComboBoxColumn FieldName="Comfort" Caption="Comfort Kit" Width="120px">
                                        <PropertiesComboBox TextField="Detail" ValueField="SSRCode"
                                            ValueType="System.String">
                                        </PropertiesComboBox>
                                        <EditItemTemplate>
                                            <dx:ASPxGridLookup ID="glComfortP21" runat="server" KeyFieldName="SSRCode" Width="120px" EnableRowsCache="false" ClientInstanceName="glComfortP21"
                                                SelectionMode="Single" TextFormatString="{0} {1}">
                                                <ClearButton DisplayMode="OnHover"></ClearButton>
                                                <Columns>
                                                    <dx:GridViewDataColumn FieldName="Detail" />
                                                    <dx:GridViewDataColumn FieldName="Price" />
                                                </Columns>
                                                <GridViewProperties EnableRowsCache="false">
                                                    <Templates>
                                                        <DataRow>
                                                            <div class="templateContainer">
                                                                <div style="float: left; margin-right: 2px;">
                                                                    <img id="img2" style="width: 100px; padding: 5px;" runat="server" alt='' src='<%#Eval("Images")%>' />
                                                                    <%--<dx:ASPxBinaryImage ID="PhotoBinaryImage" runat="server" Width="100px" Height="100px" Value='<%# Eval("Photo") %>' />--%>
                                                                </div>
                                                                <table class="templateTable">
                                                                    <tr>
                                                                        <td class="caption">Description:</td>
                                                                        <td><%# Eval("Detail")%></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="caption">Price:</td>
                                                                        <td><%# Eval("Price")%></td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </DataRow>
                                                    </Templates>
                                                    <Settings ShowColumnHeaders="False" VerticalScrollableHeight="300" />
                                                    <SettingsPager Mode="ShowAllRecords" />
                                                    <SettingsBehavior EnableRowHotTrack="True" />
                                                </GridViewProperties>
                                            </dx:ASPxGridLookup>
                                        </EditItemTemplate>
                                    </dx:GridViewDataComboBoxColumn>
                                    <dx:GridViewDataComboBoxColumn FieldName="Duty" Caption="Duty Free" Width="120px">
                                        <PropertiesComboBox TextField="Detail" ValueField="SSRCode"
                                            ValueType="System.String">
                                        </PropertiesComboBox>
                                        <EditItemTemplate>
                                            <dx:ASPxGridLookup ID="glDutyFreeP21" runat="server" KeyFieldName="SSRCode" Width="120px" EnableRowsCache="false" ClientInstanceName="glDutyFreeP21"
                                                SelectionMode="Single" TextFormatString="{0} {1}">
                                                <ClearButton DisplayMode="OnHover"></ClearButton>
                                                <Columns>
                                                    <dx:GridViewDataColumn FieldName="Detail" />
                                                    <dx:GridViewDataColumn FieldName="Price" />
                                                </Columns>
                                                <GridViewProperties EnableRowsCache="false">
                                                    <Templates>
                                                        <DataRow>
                                                            <div class="templateContainer">
                                                                <div style="float: left; margin-right: 2px;">
                                                                    <img id="img1" style="width: 100px; padding: 5px;" runat="server" alt='' src='<%#Eval("Images")%>' />
                                                                    <%--<dx:ASPxBinaryImage ID="PhotoBinaryImage" runat="server" Width="100px" Height="100px" Value='<%# Eval("Photo") %>' />--%>
                                                                </div>
                                                                <table class="templateTable">
                                                                    <tr>
                                                                        <td class="caption">Description:</td>
                                                                        <td><%# Eval("Detail")%></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="caption">Price:</td>
                                                                        <td><%# Eval("Price")%></td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </DataRow>
                                                    </Templates>
                                                    <Settings ShowColumnHeaders="False" VerticalScrollableHeight="300" />
                                                    <SettingsPager Mode="ShowAllRecords" />
                                                    <SettingsBehavior EnableRowHotTrack="True" />
                                                </GridViewProperties>
                                            </dx:ASPxGridLookup>
                                        </EditItemTemplate>

                                    </dx:GridViewDataComboBoxColumn>
                                </Columns>
                                <SettingsPager Mode="ShowPager" PageSize="10" Position="Top" />
                                <SettingsEditing Mode="Batch" />
                            </dx:ASPxGridView>
                        </div>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
        </TabPages>
    </dx:ASPxPageControl>
    <table width="90%">
        <tr>
            <td>
                
            </td>
        </tr>
    </table>
        <script type="text/javascript">
    $(document).ready(function(){
    $('.btnCtrl').on('click',function(){
    $('.activeBtn').removeClass('activeBtn');
    $(this).parent().addClass('activeBtn');
    });
    });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <%--<FBD:ucflightbookingdetail ID="ucflightbookingdetail" runat="server" />--%>
    <fd:flightdetail ID="flightdetail" runat="server" />
</asp:Content>
