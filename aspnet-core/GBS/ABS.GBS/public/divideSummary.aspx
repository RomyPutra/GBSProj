<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewPageMaster.Master" AutoEventWireup="true" CodeBehind="divideSummary.aspx.cs" Inherits="GroupBooking.Web.Booking.divideSummary" %>
<%@ Register Src="../UserControl/flightbookingdetail.ascx" TagName="ucflightbookingdetail" TagPrefix="FBD" %>
<%@ Register Src="../UserControl/PaxDetail.ascx" TagName="ucPaxDetail" TagPrefix="PD" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <FBD:ucflightbookingdetail ID="ucflightbookingdetail" runat="server" />
    <%--<PD:ucPaxDetail ID="ucPaxDetail" runat="server" />--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript" >
//        if (navigator.userAgent.toLowerCase().indexOf('firefox') > -1) {
//            parent.window.location.hash = "bookingList";
//        }
//        else if (navigator.userAgent.toLowerCase().indexOf('chrome') > -1) {
//            parent.window.location.hash = "bookingList";

//        }
//        else {

//            parent.window.location = "#bookingList";
        //        }
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
            //alert(document.getElementById("hlnk").value);
            //var iframeProxy = document.getElementById('iframeProxy');
            var furl = geturnurl();
            document.getElementById('iframeProxy').src = furl + "/proxysss.html";
        }

        document.body.onload = function () {
            setFrame();
            gotoAction('scroll:red');
        }
    </script>

    <div id="errorDiv"><dx:ASPxLabel ID="lblErr" runat="server" ClientInstanceName="lblErr">
    </dx:ASPxLabel></div>
<div class="mainContent" id="divPassengerDepart" runat="server">
<div class="page-header">
    <div class="row">
        <div class="col-sm-4">
            <h4>Divide Booking</h4>
            Booking/ Divide Booking Summary
        </div>
          <div class="col-sm-8">
               <table width="100%">
                    <tr >
                        <td align="right">
                        <table>
                        <tr>
                        <td>
                        <dx:ASPxButton ID="btnBack" runat="server" CssClass="buttonL backBtn" Text=" Back " onclick="Button1_Click">
                        <ClientSideEvents Click="function(s, e) {
                           
                                LoadingPanel.Show();
                                
                            }" />
                        </dx:ASPxButton>

                        </td>
                        <td>
                        <dx:ASPxButton ID="btnContinue" runat="server" CssClass="buttonL2" Text="Confirm" onclick="btnContinue_Click">
                        <ClientSideEvents Click="function(s, e) {
                            
                                    LoadingPanel.Show();
                        
                            }" />
                        </dx:ASPxButton>
                        </td>
                        </tr>
                        </table>
                        </td>
                    </tr>
                </table>
          </div>
        </div>
    </div>
    <div class="page-content">
    <div class="itinerary-title" runat="server" id="divNewPNR" style="display:none">
        <div class="t2">
          <div class="c2">
            <p><span>Booking number :
                  </span><span class="red"><asp:Label  id="lblBookingNo" runat="server"></asp:Label></span></p>
            
          </div>
        </div>
    </div>
    <div class="itinerary-content" runat="server" id="divGuest">
        <div class="left-col-details">
            <div class="booking-details-table">
                <div class="RadGrid RadGrid_AirAsiaBooking2">
                    <table class="rgMasterTable" rules="all" style="width: 100%; table-layout: auto; empty-cells: show;" border="0">
                        <colgroup>
                        <col/>
                        </colgroup>
                        <thead>
                        <tr>
                            <th scope="col" class="rgHeader" style="width:100%">
                                <asp:Label runat="server" id="lblPassengerListDesc" Text="Selected&nbsp;Guest(s)&nbsp;to&nbsp;Divide" ></asp:Label>
                            </th>
                        </tr>
                        </thead>
                        <tbody>
                        <tr>
                            <td>
                                <dx:ASPxListBox ID="lbAvailable" runat="server" ClientInstanceName="lbAvailable"
                                Width="100%" Height="200px" >
                                </dx:ASPxListBox>
                            </td>
                        </tr>
                        </tbody>
                    </table>
                </div>
                
                <p style="color:Red; font-size:14px; font-weight:bold" runat="server" id="lblNote">* Once booking is split it cannot be undone. 
                    Click Confirm to continue.</p>
                </div>
        </div>
    </div>
        
        
        
    </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
