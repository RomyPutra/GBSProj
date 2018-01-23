<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewPageMaster.Master" AutoEventWireup="true" CodeBehind="divide.aspx.cs" Inherits="GroupBooking.Web.Booking.divide" %>
<%@ Register Src="../UserControl/flightbookingdetail.ascx" TagName="ucflightbookingdetail" TagPrefix="FBD" %>
<%@ Register Src="../UserControl/PaxDetail.ascx" TagName="ucPaxDetail" TagPrefix="PD" %>
<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %> <%-- 20170427 - Sienny --%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <div style="display:block;">
    <FBD:ucflightbookingdetail ID="ucflightbookingdetail" runat="server" />
    <PD:ucPaxDetail ID="ucPaxDetail" runat="server" />
    </div>
<%--<div id="flightDetailWrapper">
<!-- The header to show the depart and return -->
<div id="flightRoute">
<span id="flightDepartReturn">Kuala Lumpur (KUL) to Singapore (SIN)</span>
</div>
<!-- The trip total cost-->
<div id="totalPricing">
 <div class="grayWrapper">
 <div>
 <div id="labelCurrency"><span>TOTAL </span><span id="currencyType">MYR</span></div>
 <div id="totalAmtWrapper">6,600.00</div>
 </div>
 </div>
</div>
<!-- Wrapper for Depart , Connecting, and Return -->
<div id="departReturnWrapper">

 <div id="mainDepartConnectWrapper">
 <div class="flightTitleDepartReturn">DEPART</div>
 
 <div id="departWrapper" class="flightWrapper">
 <div class="flightNo">
 <span>AK117</span>
 </div>
 <div id="depart1To" class="blockFlight">CAN</div>
 <div class="arrowToWrapper"><span class="fa fa-angle-right "></span></div>
 <div id="return1To" class="blockFlight">KUL</div>
 <div class="detailDateFlight">
 <div class="classDate">Mon,30 Jan 2017 2125</div><div class="classDate">Mon,30 Jan 2017 2240</div> 
 </div>
 </div>
 
 <div id="connectWrapperDepart" class="flightWrapper">
 <div class="flightNo">
  <span>AK367</span>
 </div>
   <div id="connectDepart1To" class="blockFlight">KUL</div>
   <div class="arrowToWrapper"><span class="fa fa-angle-right "></span></div>
   <div id="connectDepart1To" class="blockFlight">DPS</div>
    <div class="detailDateFlight">
    <div class="classDate">Mon,30 Jan 2017 2300</div><div class="classDate">Tue,31 Jan 2017 0100</div> 
    </div>   
 </div>

 </div>
 
  <div id="mainReturnConnectWrapper">
 <div class="flightTitleDepartReturn">RETURN</div>
 
 <div id="departWrapper" class="flightWrapper">
 <div class="flightNo">
 <span>AK767</span>
 </div>
 <div id="depart2To" class="blockFlight">DPS</div>
 <div class="arrowToWrapper"><span class="fa fa-angle-right "></span></div>
 <div id="return2To" class="blockFlight">KUL</div>
 <div class="detailDateFlight">
 <div class="classDate">Tue,31 Jan 2017 0130</div><div class="classDate">Tue,31 Jan 2017 0400</div> 
 </div>
 </div>
 
 <div id="connectWrapperDepart" class="flightWrapper">
 <div class="flightNo">
  <span>AK769</span>
 </div>
   <div id="connectDepart2To" class="blockFlight">KUL</div>
   <div class="arrowToWrapper"><span class="fa fa-angle-right "></span></div>
   <div id="connectDepart2To" class="blockFlight">CAN</div>
    <div class="detailDateFlight">
    <div class="classDate">Tue,31 Jan 2017 0430</div><div class="classDate">Tue,31 Jan 2017 0700</div> 
    </div>   
 </div>
 </div>
  </div>
</div>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript">
        // <![CDATA[ 
        function AddSelectedItems() {
            MoveSelectedItems(lbAvailable, lbChoosen);
            UpdateButtonState();
        }
        function AddAllItems() {
            MoveAllItems(lbAvailable, lbChoosen);
            UpdateButtonState();
        }
        function RemoveSelectedItems() {
            MoveSelectedItems(lbChoosen, lbAvailable);
            UpdateButtonState();
        }
        function RemoveAllItems() {
            MoveAllItems(lbChoosen, lbAvailable);
            UpdateButtonState();
        }
        function MoveSelectedItems(srcListBox, dstListBox) {
            srcListBox.BeginUpdate();
            dstListBox.BeginUpdate();
            var items = srcListBox.GetSelectedItems();
            for (var i = items.length - 1; i >= 0; i = i - 1) {
                dstListBox.AddItem(items[i].text, items[i].value);
                srcListBox.RemoveItem(items[i].index);
            }
            srcListBox.EndUpdate();
            dstListBox.EndUpdate();
        }
        function MoveAllItems(srcListBox, dstListBox) {
            srcListBox.BeginUpdate();
            var count = srcListBox.GetItemCount();
            for (var i = 0; i < count; i++) {
                var item = srcListBox.GetItem(i);
                dstListBox.AddItem(item.text, item.value);
            }
            srcListBox.EndUpdate();
            srcListBox.ClearItems();
        }
        function UpdateButtonState() {
            //btnMoveAllItemsToRight.SetEnabled(lbAvailable.GetItemCount() > 0);
            btnMoveAllItemsToLeft.SetEnabled(lbChoosen.GetItemCount() > 0);
            btnMoveSelectedItemsToRight.SetEnabled(lbAvailable.GetSelectedItems().length > 0);
            btnMoveSelectedItemsToLeft.SetEnabled(lbChoosen.GetSelectedItems().length > 0);

        }

        function CloseMsg() {
            var ele = document.getElementById("validationErrorContainerReadAlong");
            var text = document.getElementById("validationErrorContainerReadAlongCloseButton");
            ele.style.display = "none";
        }

        function OnMonthOrYearChanged(ddl) {
            var e = document.getElementById("ContentPlaceHolder2_InfantPanel_ASPxPanel1_ddlBirthDays");
            var ind = e.options[e.selectedIndex].value;
            document.getElementById("hbday").value = ind;
            ddlBirthDays.PerformCallback(ind);

            LoadingPanel.SetText();
            LoadingPanel.Show();
            setTimeout(function () { LoadingPanel.Hide() }, 2000);
        }

        function OnMonthOrYearChangedExp(ddl) {
            var e = document.getElementById("ContentPlaceHolder2_InfantPanel_ASPxPanel2_ddlDaysExp");
            var ind = e.options[e.selectedIndex].value;
            document.getElementById("hext").value = ind;
            ddlDaysExp.PerformCallback(ind);

            LoadingPanel.SetText();
            LoadingPanel.Show();
            setTimeout(function () { LoadingPanel.Hide() }, 2000);
        }

        function OnSelectMonthComplete() {
            document.getElementById("ContentPlaceHolder2_InfantPanel_ASPxPanel1_ddlBirthDays").selectedIndex = document.getElementById("hbday").value;
        }

        function OnSelectExtComplete() {
            document.getElementById("ContentPlaceHolder2_InfantPanel_ASPxPanel2_ddlDaysExp").selectedIndex = document.getElementById("hext").value;
        }

        function OnCallbackComplete(s, e) {
            alert(e.result);
            lblTravalingWith.SetText(" ");
            lblErr.SetText(" ");
            if (e.result != "") {
                InfantPanel.SetEnabled(true);
                lblTravalingWith.SetText(e.result);
            }
            else {
                InfantPanel.SetEnabled(false);
                lblErr.SetText("Selected child passenger is not allow to add infant.");
            }

        }

        if (isPostBack == false) {

            setCookie("parenturl", String(document.referrer), 1);

        }

        function setCookie(c_name, value, exdays) {
            var exdate = new Date();
            exdate.setDate(exdate.getDate() + exdays);
            var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
            document.cookie = c_name + "=" + c_value;
        }

        function getCookie(c_name) {
            var i, x, y, ARRcookies = document.cookie.split(";");
            for (i = 0; i < ARRcookies.length; i++) {
                x = ARRcookies[i].substr(0, ARRcookies[i].indexOf("="));
                y = ARRcookies[i].substr(ARRcookies[i].indexOf("=") + 1);
                x = x.replace(/^\s+|\s+$/g, "");
                if (x == c_name) {
                    return unescape(y);
                }
            }
        }

        function gotoAction(action) {

            var iframeProxy = document.getElementById('iframeProxy');

            // Step 1: put the parameters you want to give to the iframe proxy in the hash
            var src = iframeProxy.src.split('#');

            iframeProxy.src = src[0] + '#' + action;

            // Step 2: change the size of the iframe proxy to execute his resize event (see proxyA.html)
            document.getElementById('iframeProxy').width = parseInt(document.getElementById('iframeProxy').width) + 1;



            return false;

        }

        //tyas
        function gv_Callback(RecordLocator) {
            
            //var locations = location.href + "&recordlocator=" + RecordLocator;
            
            document.getElementById('ctl00_ContentPlaceHolder2_hfpnr').value = RecordLocator;
            //alert(document.getElementById('ctl00_ContentPlaceHolder2_hfpnr').value);
        }

        function settotop() {
            if (navigator.userAgent.toLowerCase().indexOf('firefox') > -1 || navigator.userAgent.toLowerCase().indexOf('chrome') > -1) {
                gotoAction('scroll:red');
            }
            else {
                parent.window.location = "#bookingList";
            }

        }

        function geturnurl() {

            //var parentUrl = String(document.referrer);

            var parentUrl = getCookie("parenturl");

            if (parentUrl != null && parentUrl != "") {

                var b = '';
                var a = parentUrl.split('/');

                if (a.length > 0) {

                    for (var i = 0; i < a.length - 1; i++) {
                        if (b == "") {
                            b = a[i];
                        } else {
                            b += "/" + a[i];
                        }
                    }

                    return b;

                }
            }
            return "";
        }

        function setFrame() {
            var furl = geturnurl();
            document.getElementById('iframeProxy').src = furl + "/proxysss.html";
        }

        //document.body.onload = function () {
        //    document.getElementById("hlnk").value = String(document.referrer);
        //    setFrame();
        //}


        // ]]> 

        //20170428 - Sienny (to get parameter k,transid,recordlocator)
        function getParameterByName(name, url) {
            if (!url) {
                url = window.location.href;
            }
            name = name.replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        }

        var k = getParameterByName('k');
        var TransID = getParameterByName('TransID');
        var RecordLocator = getParameterByName('recordlocator');
        //20170427 - Sienny (to show error message on prompt)
        function OnBtnContinueClick(s, e) {
            if (e.result != "") {
                document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = e.result;
                pcMessage.Show();
                LoadingPanel.Hide();
            } else {
                //alert('k=' + k + '&TransID=' + TransID + '&recordlocator=' + RecordLocator);
                window.location.href = '../public/divideSummary.aspx?k=' + k + '&TransID=' + TransID + '&recordlocator=' + RecordLocator;
            }
        }

    </script>
    
    <%-- 20170427 - Sienny (to show error message on prompt) --%>
    <msg:msgControl ID="msgcontrol" runat="server" />
    <dx:ASPxCallback ID="Callback" runat="server" ClientInstanceName="Callback"  OnCallback="ValidatePopup">
        <ClientSideEvents CallbackComplete="function(s, e) { OnBtnContinueClick(s, e);  }" /> 
    </dx:ASPxCallback>
    <%-- 20170427 - Sienny --%>

    <input type="hidden" id="hURL" value="" runat="server" />
    <asp:HiddenField ID="hfpnr" runat="server" />
    <asp:panel Visible = "false"  ID ="pnlErr" runat="server" Height="40px" style="padding-top: 12px;background: #e32526;text-align:center !important"><dx:ASPxLabel runat="server" ForeColor="White" ID="lblErr" Style="font-size:18px !important"></dx:ASPxLabel></asp:panel>
<%--    <div id="errorDiv" style="position:fixed;vertical-align:central"><dx:ASPxLabel ID="lblErr" runat="server" ClientInstanceName="lblErr" ForeColor="Red">
    </dx:ASPxLabel>
    
    </div>--%>
    <dx:ASPxValidationSummary ID="ASPxValidationSummary1" runat="server" RenderMode="Table" ClientInstanceName="ASPxValidationSummary1">
    </dx:ASPxValidationSummary>
    
    <asp:Panel runat="server" ID="pnlDefault" >
    <div class="page-header">
        <div class="row">
            <div class="col-sm-4">
                <h4>Divide Booking</h4>
                Booking/Divide Booking
            </div>
            <div class="col-sm-8">
                  
            </div>
        </div>
    </div>
    <div class="page-content">
        <%--<div>Please select a PNR</div>--%>
        <div class="col-sm-6" style="padding-left: 40px;">
            <div style="padding-left: 5px; color: #da0004; font-size: 20px; font-weight: 400;">
                <h4>Choose one of PNR(s) below to Divide the booking</h4>
            </div>
            <table id="paymentDetailsTbl" class="table table-bordered">
                <tbody>
                    <tr>
                        <td class="">Booking ID :</td>
                        <td class="">
                            <asp:Label ID="lblTransID" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="" colspan="2">PNR(s) :</td>
                    </tr>
                    <asp:Repeater EnableViewState="true" ID="rptPNR" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td colspan="2">
                                    <div class="btnTransparent" style="width: 100%; padding: 3px 0;">
                                        <asp:LinkButton ID="lnkPNR" ClientIDMode="Static" runat="server"
                                            CommandName="click" CommandArgument='<%#Eval("RecordLocator") %>' Text='<%#Eval("PNR") %>' OnClientClick='<%# String.Format("return gv_Callback(&#39;{0}&#39;);", Eval("RecordLocator")) %>'>
                                        </asp:LinkButton>
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>

                </tbody>
            </table>
        </div>
    </div>
    </asp:Panel>

    <asp:Panel ID="pnlMain" runat="server" Visible="false">
    <div class="page-header">
    <div class="row">
        <div class="col-sm-4">
            <h4>Divide Booking</h4>
            Booking/ Divide Booking
        </div>
          <div class="col-sm-8">
            <table border="0" cellpadding="0" cellspacing="0" style="float:right;">
                 <tr>
                     <td>
                        <dx:ASPxButton ID="btnBackTop" runat="server" Text="Back" CssClass="buttonL backBtn" AutoPostBack="True" onclick="btnBack_Click" >
                            <ClientSideEvents Click="function(s, e) {
                                if (ASPxClientEdit.AreEditorsValid()){
                                    LoadingPanel.SetText(&#39;Please Wait...&#39;);
                                    LoadingPanel.Show();
                                } 
                            }" />
                        </dx:ASPxButton> 
                     </td>
                     <td>
                        <%--<dx:ASPxButton ID="btnContinue" runat="server" Text="Continue" CssClass="buttonL" AutoPostBack="True" onclick="btnContinue_Click" >
                            <ClientSideEvents Click="function(s, e) {
                                if (ASPxClientEdit.AreEditorsValid()){
                                    LoadingPanel.SetText(&#39;Please Wait...&#39;);
                                    LoadingPanel.Show();
                                } 
                            }" />
                        </dx:ASPxButton> --%>

                         <%-- 20170427 - Sienny (change error message from label to prompt) --%>
                         <dx:ASPxButton ID="btnContinue" runat="server" Text="Continue" CssClass="buttonL" AutoPostBack="False" >
                            <ClientSideEvents Click="function(s, e) {
                                if (ASPxClientEdit.AreEditorsValid()){
                                    Callback.PerformCallback();
                                    LoadingPanel.SetText(&#39;Please Wait...&#39;);
                                    LoadingPanel.Show();
                                } 
                            }" />
                        </dx:ASPxButton> 
                     </td>
                 </tr>
             </table>  
          </div>
        </div>
    </div>
    <div class="page-content">
    <div class="itinerary-title">
        <div class="t2">
          <div class="c2">
            <p><span>Booking number :
                  </span><span class="red"><asp:Label  id="lblBookingNo" runat="server">ABC987</asp:Label></span></p>
            
          </div>
        </div>
    </div>
    <div id="itineraryBody" class="mainBody">
        <div class="itinerary-content" xmlns:ms="urn:schemas-microsoft-com:xslt">
            <asp:Panel ID="pnlMainDetails" runat="server" Visible="false">
            <div class="left-col-details">
                <div class="booking-details-title">Booking Details</div>
                <div class="booking-details-table">
                    <div class="customHeader">Flight details</div>
                    <div id="divDepart" runat="server" visible="false" class="RadGrid RadGrid_AirAsiaBooking1 radGridFlightTable">
                        <table class="rgMasterTable" rules="all" style="width: 100%; table-layout: auto; empty-cells: show;" border="0">
                            <colgroup>
                            <col width="104px"/>
                            <col width="341px"/>
                            <col/>
                          </colgroup>
                          <thead>
                            <tr>
                              <th scope="col" class="rgHeader">Depart</th>
                              <th scope="col" class="rgHeader"><asp:Label ID="lblNewDepartCode" runat="server" Text="DepartCode"></asp:Label></th>
                              <th scope="col" class="rgHeader">
                                &nbsp;
                                </th>
                            </tr>
                          </thead>
                          <tbody>
                            <tr class="rgRow" id="ctl00_OptionalHeaderContent_radGridDepartTable_ctl00__0">
                              <td>
                                <div class="left"><asp:Label ID="lblNewDepartFlight" runat="server" Text="DepartFlight" ></asp:Label><br/>
                                <asp:Label ID="lblPromotion" Text=" " runat="server" ></asp:Label><br/></div>
                              </td>
                              <td>
                                <div class="itineraryCustom1">
                                  <div class="left itineraryCustom2">
                                    <div style="font-weight: bold">
                                    <asp:Label ID="lblNewDepartStationFrom" runat="server" Text="Alor Setar (AOR)" ></asp:Label><br/></div>
                                    <asp:Label ID="lblNewDepartAirportFrom" runat="server" Text="Alor Setar" ></asp:Label><br/>
                                    <asp:Label ID="lblNewDepartDateFrom" runat="server" Text="Tuesday, 30 October 2012, 0830 (8:30 AM)" ></asp:Label>
                                    <br/>
                                    </div>
                                  <div class="arrow-right" style="margin: 10px 10px 10px 20px;"></div>
                                </div>
                              </td>
                              <td>
                                <div class="left itineraryCustom2">
                                  <div style="font-weight: bold"><asp:Label ID="lblNewDepartStationTo" runat="server" Text="Kuala Lumpur (KUL)" ></asp:Label><br/></div>
                                  <asp:Label ID="lblNewDepartAirportTo" runat="server" Text="Kuala Lumpur LCCT" ></asp:Label><br/>
                                  <asp:Label ID="lblNewDepartDateTo" runat="server" Text="Tuesday, 30 October 2012, 0935 (9:35 AM)" ></asp:Label><br/></div>
                              </td>
                            </tr>
                          </tbody>
                        </table>
                    </div>

                    <!-- Depart Connecting Flight -->
                    <div id="divDepartC" runat="server" visible="false" class="RadGrid RadGrid_AirAsiaBooking1 radGridFlightTable">
                        <table class="rgMasterTable" rules="all" style="width: 100%; table-layout: auto; empty-cells: show;" border="0">
                            <colgroup>
                            <col width="104px"/>
                            <col width="341px"/>
                            <col/>
                          </colgroup>
                          <tbody>
                            <tr class="rgRow" id="Tr9">
                              <td>
                                <div class="left"><asp:Label ID="lblNewDepartCFlight" runat="server" Text="DepartFlight" ></asp:Label><br/>
                                <asp:Label ID="lblDepartCPromo" Text=" " runat="server" ></asp:Label><br/></div>
                              </td>
                              <td>
                                <div class="itineraryCustom1">
                                  <div class="left itineraryCustom2">
                                    <div style="font-weight: bold">
                                    <asp:Label ID="lblNewDepartCStationFrom" runat="server" Text="Alor Setar (AOR)" ></asp:Label><br/></div>
                                    <asp:Label ID="lblNewDepartCAirportFrom" runat="server" Text="Alor Setar" ></asp:Label><br/>
                                    <asp:Label ID="lblNewDepartCDateFrom" runat="server" Text="Tuesday, 30 October 2012, 0830 (8:30 AM)" ></asp:Label>
                                    <br/>
                                    </div>
                                  <div class="arrow-right" style="margin: 10px 10px 10px 20px;"></div>
                                </div>
                              </td>
                              <td>
                                <div class="left itineraryCustom2">
                                  <div style="font-weight: bold">
                                  <asp:Label ID="lblNewDepartCStationTo" runat="server" Text="Kuala Lumpur (KUL)" ></asp:Label><br/></div>
                                  <asp:Label ID="lblNewDepartCAirportTo" runat="server" Text="Kuala Lumpur LCCT" ></asp:Label><br/>
                                  <asp:Label ID="lblNewDepartCDateTo" runat="server" Text="Tuesday, 30 October 2012, 0935 (9:35 AM)" ></asp:Label><br/></div>
                              </td>
                            </tr>
                          </tbody>
                        </table>
                    </div>

                    <!-- Return Flight -->
                    <div id="divReturn" runat="server" visible="false" class="RadGrid RadGrid_AirAsiaBooking1 radGridFlightTable">
                        <table class="rgMasterTable" rules="all" style="width: 100%; table-layout: auto; empty-cells: show;" border="0">
                            <colgroup>
                            <col width="104px"/>
                            <col width="341px"/>
                            <col/>
                          </colgroup>
                          <thead>
                            <tr>
                              <th scope="col" class="rgHeader">Return</th>
                              <th scope="col" class="rgHeader"><asp:Label ID="lblReturnCode" runat="server" Text="ReturnCode"></asp:Label></th>
                              <th scope="col" class="rgHeader">
                                &nbsp;
                                </th>
                            </tr>
                          </thead>
                          <tbody>
                            <tr class="rgRow" id="Tr8">
                              <td>
                                <div class="left"><asp:Label ID="lblNewReturnFlight" runat="server" Text="DepartFlight" ></asp:Label><br/>
                                <asp:Label ID="lblReturnPromotion" Text=" " runat="server" ></asp:Label><br/></div>
                              </td>
                              <td>
                                <div class="itineraryCustom1">
                                  <div class="left itineraryCustom2">
                                    <div style="font-weight: bold">
                                    <asp:Label ID="lblNewReturnStationFrom" runat="server" Text="Alor Setar (AOR)" ></asp:Label><br/></div>
                                    <asp:Label ID="lblNewReturnAirportFrom" runat="server" Text="Alor Setar" ></asp:Label><br/>
                                    <asp:Label ID="lblNewReturnDateFrom" runat="server" Text="Tuesday, 30 October 2012, 0830 (8:30 AM)" ></asp:Label>
                                    <br/>
                                    </div>
                                  <div class="arrow-right" style="margin: 10px 10px 10px 20px;"></div>
                                </div>
                              </td>
                              <td>
                                <div class="left itineraryCustom2">
                                  <div style="font-weight: bold"><asp:Label ID="lblNewReturnStationTo" runat="server" Text="Kuala Lumpur (KUL)" ></asp:Label><br/></div>
                                  <asp:Label ID="lblNewReturnAirportTo" runat="server" Text="Kuala Lumpur LCCT" ></asp:Label><br/>
                                  <asp:Label ID="lblNewReturnDateTo" runat="server" Text="Tuesday, 30 October 2012, 0935 (9:35 AM)" ></asp:Label><br/></div>
                              </td>
                            </tr>
                          </tbody>
                        </table>
                    </div>

                    <!-- Return Connecting Flight -->
                    <div id="divReturnC" runat="server" visible="false" class="RadGrid RadGrid_AirAsiaBooking1 radGridFlightTable">
                        <table class="rgMasterTable" rules="all" style="width: 100%; table-layout: auto; empty-cells: show;" border="0">
                            <colgroup>
                            <col width="104px"/>
                            <col width="341px"/>
                            <col/>
                          </colgroup>
                          <tbody>
                            <tr class="rgRow" id="Tr10">
                              <td>
                                <div class="left"><asp:Label ID="lblNewReturnCFlight" runat="server" Text="DepartFlight" ></asp:Label><br/>
                                <asp:Label ID="lblReturnCPromo" Text=" " runat="server" ></asp:Label><br/></div>
                              </td>
                              <td>
                                <div class="itineraryCustom1">
                                  <div class="left itineraryCustom2">
                                    <div style="font-weight: bold">
                                    <asp:Label ID="lblNewReturnCStationFrom" runat="server" Text="Alor Setar (AOR)" ></asp:Label><br/></div>
                                    <asp:Label ID="lblNewReturnCAirportFrom" runat="server" Text="Alor Setar" ></asp:Label><br/>
                                    <asp:Label ID="lblNewReturnCDateFrom" runat="server" Text="Tuesday, 30 October 2012, 0830 (8:30 AM)" ></asp:Label>
                                    <br/>
                                    </div>
                                  <div class="arrow-right" style="margin: 10px 10px 10px 20px;"></div>
                                </div>
                              </td>
                              <td>
                                <div class="left itineraryCustom2">
                                  <div style="font-weight: bold">
                                  <asp:Label ID="lblNewReturnCStationTo" runat="server" Text="Kuala Lumpur (KUL)" ></asp:Label><br/></div>
                                  <asp:Label ID="lblNewReturnCAirportTo" runat="server" Text="Kuala Lumpur LCCT" ></asp:Label><br/>
                                  <asp:Label ID="lblNewReturnCDateTo" runat="server" Text="Tuesday, 30 October 2012, 0935 (9:35 AM)" ></asp:Label><br/></div>
                              </td>
                            </tr>
                          </tbody>
                        </table>
                    </div>
                </div>
            </div>
            </asp:Panel>
            <div class="clearAll" xmlns:ms="urn:schemas-microsoft-com:xslt"></div>
            <asp:Panel ID="pnlPassagerInfo" runat="server" Visible="false" >
            <div class="itinerary-content" id="divPassengerDepart" runat="server">
                <div class="left-col-details">
                    <div class="booking-details-table">
                        <div class="RadGrid RadGrid_AirAsiaBooking2">
                            <table id="divideUIWrapper" class="rgMasterTable" rules="all" style="width: 100%; table-layout: auto; empty-cells: show;" border="0">
                            <colgroup>
                                <col/>
                            </colgroup>
                            <thead>
                            <tr>
                              <th scope="col" class="rgHeader" colspan="3">Guest Details</th>
                            </tr>
                          </thead>
                            <tbody>
                                
                                <tr>
                                    <td valign="top" style="width: 35%">
                                        <div class="BottomPadding">
                                            <dx:ASPxLabel ID="lblAvailable" runat="server" Text="Guest(s):" />
                                        </div>
                                        <dx:ASPxListBox ID="lbAvailable" runat="server" ClientInstanceName="lbAvailable"
                                            Width="100%" Height="240px" SelectionMode="CheckColumn">
                           
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonState(); }" />
                            
                                        </dx:ASPxListBox>
                                    </td>
                                    <td valign="middle" align="center" style="padding: 10px; width: 30%">
                                        <div>
                                            <buttonadvancedsearch id="ButtonAdvancedSearch">
                                            <dx:ASPxButton ID="btnMoveSelectedItemsToRight" runat="server" ClientInstanceName="btnMoveSelectedItemsToRight"
                                                AutoPostBack="False" Text="Add>" Width="130px" Height="23px" ClientEnabled="False"
                                                ToolTip="Add selected items">
                                                <ClientSideEvents Click="function(s, e) { AddSelectedItems(); }" />
                                            </dx:ASPxButton>
                                            </buttonadvancedsearch>
                                            <br />
                                        </div>
                        
                                        <div>
                                            <dx:ASPxButton ID="btnMoveSelectedItemsToLeft" runat="server" ClientInstanceName="btnMoveSelectedItemsToLeft"
                                                AutoPostBack="False" Text="<Remove" Width="130px" Height="23px" ClientEnabled="False"
                                                ToolTip="Remove selected items">
                                                <ClientSideEvents Click="function(s, e) { RemoveSelectedItems(); }" />
                                            </dx:ASPxButton>
                                            <br />
                                        </div>
                                        <div class="TopPadding">
                                            <dx:ASPxButton ID="btnMoveAllItemsToLeft" runat="server" ClientInstanceName="btnMoveAllItemsToLeft"
                                                AutoPostBack="False" Text="<<Remove All" Width="130px" Height="23px" ClientEnabled="False"
                                                ToolTip="Remove all items">
                                                <ClientSideEvents Click="function(s, e) { RemoveAllItems(); }" />
                                            </dx:ASPxButton>
                                            <br />
                                        </div>
                                    </td>
                                    <td valign="top" style="width: 35%">
                                        <div class="BottomPadding">
                                            <dx:ASPxLabel ID="lblChosen" runat="server" Text="Selected Guest(s):" />
                                        </div>
                                        <dx:ASPxListBox ID="lbChoosen" runat="server" ClientInstanceName="lbChoosen" Width="100%"
                                            Height="240px" SelectionMode="CheckColumn">
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonState(); }">
                                            </ClientSideEvents>
                                        </dx:ASPxListBox>
                                    </td>
                                </tr>
                            </tbody>
                          </table>
                        </div>
                    </div>
                </div>
            </div>
            </asp:Panel>
        </div>


    </div>
    </div>
    </asp:Panel>
    
      <div class="page-content" style="padding-top:0px;min-height:400px;">
    <asp:Panel ID="pnlmsgerror" 
            runat ="server" Visible ="false" style="position:static; height:1px;overflow:visible " >
 <div id="validationErrorContainerReadAlong" style="" >
  
 <p class="close" >

   <a id="validationErrorContainerReadAlongCloseButton" href="javascript:CloseMsg();"></a> 
 </p>
  
 <div id="validationErrorContainerReadAlongContent">
   
  <h3 class="error">
   ERROR
  </h3>
   
  <div id="validationErrorContainerReadAlongList">
   <ul class="validationErrorList">
    <li class="validationErrorListItem hidden"></li>
    <li class="validationErrorListItem hidden"></li>
    <li class="validationErrorListItem">
     Please agree to the Fare Rules and Terms to continue.
    </li>
    <li class="validationErrorListItem hidden"></li>
    <li class="validationErrorListItem hidden"></li>
  
    <li class="validationErrorListItem hidden"></li>
    <li class="validationErrorListItem hidden"></li>
    <li class="validationErrorListItem hidden"></li>
    <li class="validationErrorListItem hidden"></li>
    <li class="validationErrorListItem hidden"></li>
    <li class="validationErrorListItem hidden"></li>
    <li class="validationErrorListItem hidden"></li>
    <li class="validationErrorListItem hidden"></li>
    <li class="validationErrorListItem hidden"></li>
    <li class="validationErrorListItem hidden"></li>
    <li class="validationErrorListItem hidden fixedValidationError"></li>
   </ul>
  </div>
   
 </div>
  
</div>
 </asp:Panel>
    <asp:Panel ID="pnlAgreement" runat="server" Visible="false">
    <div class="spacer" xmlns:formatterextension="urn:navitaire:formatters:currency" xmlns:akformatterextension="urn:navitaire:formatters:mcc"></div>
                <div id="operatedByInfo" xmlns:formatterextension="urn:navitaire:formatters:currency" xmlns:akformatterextension="urn:navitaire:formatters:mcc">Flight operated by: <span class="operatedByAK">AK - AirAsia</span> |
      <span class="operatedByFD">FD - Thai AirAsia</span> |
      <span class="operatedByQZ">QZ - Indonesia AirAsia</span> |
      <span class="operatedByD7">D7 - AirAsia X</span></div>
      

    <div id="agreementInput" class="sectionBody">
     
    <p id="agreementInputCheckbox" class="formCheckbox">
    
        <dx:ASPxCheckBox ID="CheckBoxAgreement" runat="server" OnCheckedChanged="CheckBoxAgreement_CheckedChanged" AutoPostBack="false">
       
        </dx:ASPxCheckBox>
    <asp:CustomValidator ID="ctvCheckBox" runat="server" ValidationGroup="mandatory" OnServerValidate="ctvAgreement_ServerValidate" ></asp:CustomValidator>
    <asp:CustomValidator ID="ctvListBox" runat="server" ValidationGroup="mandatory" OnServerValidate="ctvListBox_ServerValidate" ></asp:CustomValidator>
    <asp:CustomValidator ID="ctvPassengers" runat="server" ValidationGroup="mandatory" OnServerValidate="ctvPassenger_ServerValidate" ></asp:CustomValidator>
    <%--<asp:CustomValidator ID="ctvRadioList" runat="server" ValidationGroup="mandatory" OnServerValidate="ctvPassengerInfant_ServerValidate" ></asp:CustomValidator>--%>
   
    <span id="CheckBoxAgreementLabel"><label for="ControlGroupSelectView_AgreementInputSelectView_CheckBoxAgreement">
	  Please check this box to confirm that you understand and accept AirAsia's Terms and conditions of carriage and Fare rules to continue.
		</label></span><span id="CheckBoxAgreementLabelAK"><label for="ControlGroupSelectView_AgreementInputSelectView_CheckBoxAgreement">
	  Please check this box to confirm that you understand and accept AirAsia's <a href="javascript:SKYSALES.openTermsAndConditions('AK', 'en');" id="termsAndConditionsLink">Terms and conditions of carriage</a> and <a href="javascript:SKYSALES.openFareRules('en')">Fare rules</a> to continue.
		</label></span><span id="CheckBoxAgreementLabelFD"><label for="ControlGroupSelectView_AgreementInputSelectView_CheckBoxAgreement">
	  Please check this box to confirm that you understand and accept AirAsia's <a href="javascript:SKYSALES.openTermsAndConditions('FD', 'en');" id="termsAndConditionsLink2">Terms and conditions of carriage</a> and <a href="javascript:SKYSALES.openFareRules('en')">Fare rules</a> to continue.
		</label></span><span id="CheckBoxAgreementLabelQZ"><label for="ControlGroupSelectView_AgreementInputSelectView_CheckBoxAgreement">
	  Please check this box to confirm that you understand and accept AirAsia's <a href="javascript:SKYSALES.openTermsAndConditions('QZ', 'en');" id="termsAndConditionsLink3">Terms and conditions of carriage</a> and <a href="javascript:SKYSALES.openFareRules('en')">Fare rules</a> to continue.
		</label></span><span id="CheckBoxAgreementLabelD7" style="display: inline; "><label for="ControlGroupSelectView_AgreementInputSelectView_CheckBoxAgreement">
	    Please check this box to confirm that you understand and accept <b>AirAsia&#39;s</b> 
        and <b>AirAsia X&#39;s</b> <a href="javascript:SKYSALES.openTermsAndConditions('D7', 'en');" id="termsAndConditionsLink4">Terms and conditions of carriage</a> and <a href="javascript:SKYSALES.openFareRules('en')">Fare rules</a> to continue. 
    The booking cannot be cancelled and payments made are not refundable. Details for name and flight changes are clearly specified in 'terms &amp; conditions' and 'fare rules'. Please check carefully before you make payments. The international credit card commission will be charged in addition to the quoted air fare.
		</label></span><span id="CheckBoxAgreementLabelConnecting" style="display: none; "><label for="ControlGroupSelectView_AgreementInputSelectView_CheckBoxAgreement">
	  Please check this box to confirm that you understand and accept <b>AirAsia's</b> and <b>AirAsia X's </b><a href="javascript:SKYSALES.openTermsAndConditions('D7', 'en');" id="termsAndConditionsLink5">Terms and conditions of carriage</a> and <a href="javascript:SKYSALES.openFareRules('en')">Fare rules</a> to continue. 
    The booking cannot be cancelled and payments made are not refundable. Details for name and flight changes are clearly specified in 'terms &amp; conditions' and 'fare rules'. Please check carefully before you make payments. The international credit card commission will be charged in addition to the quoted air fare.
		</label></span>
    </p>
        <div class="clearAll">
        </div>
        <p id="agreement24Hrs" class="formCheckbox">
            <input id="agreement24HrsCheckbox" type="checkbox" />
            <span>
            <label for="agreement24HrsCheckbox">
            I understand that by selecting a flight that departs within the next 24 hours, 
            the meal of my choice will not be guaranteed.
            </label>
            </span>
        </p>
        <div id="Note24Hrs" style="display: none; ">
            <div id="icon24Hrs">
            </div>
            <b>You have selected a flight that departs in 4 hours.</b><br/> Online booking 
            is not available for flights that depart less than 4 hours. Please proceed to 
            the nearest AirAsia office or our sales counter at the airport.
        </div>
        <div class="clearAll">
        </div>
   
    </div>

    <div class="page-header row">
        <div class="col-sm-4">
        </div>
        <div class="col-sm-8">
            <table border="0" cellpadding="0" cellspacing="0" style="float:right;">
                <tr>
                    <td>
                    <dx:ASPxButton ID="btnBackBottom" runat="server" Text="Back" CssClass="buttonL backBtn" AutoPostBack="True" onclick="btnBack_Click" >
                        <ClientSideEvents Click="function(s, e) {
                            if (ASPxClientEdit.AreEditorsValid()){
                                LoadingPanel.SetText(&#39;Please Wait...&#39;);
                                LoadingPanel.Show();
                            } 
                        }" />
                    </dx:ASPxButton> 
                    </td>
                    <td>
                    <%--<dx:ASPxButton ID="btnContinueBottom" runat="server" Text="Continue" CssClass="buttonL" AutoPostBack="True" onclick="btnContinue_Click" >
                        <ClientSideEvents Click="function(s, e) {
                            if (ASPxClientEdit.AreEditorsValid()){
                                LoadingPanel.SetText(&#39;Please Wait...&#39;);
                                LoadingPanel.Show();
                            } 
                        }" />
                    </dx:ASPxButton> --%>

                         <%-- 20170427 - Sienny (change error message from label to prompt) --%>
                         <dx:ASPxButton ID="btnContinueBottom" runat="server" Text="Continue" CssClass="buttonL" AutoPostBack="False" >
                            <ClientSideEvents Click="function(s, e) {
                                if (ASPxClientEdit.AreEditorsValid()){
                                    Callback.PerformCallback();
                                    LoadingPanel.SetText(&#39;Please Wait...&#39;);
                                    LoadingPanel.Show();
                                } 
                            }" />
                        </dx:ASPxButton> 
                    </td>
                </tr>
            </table>       
        </div>
    </div>
    </asp:Panel>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
</asp:Content>
