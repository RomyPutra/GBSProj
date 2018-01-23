<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewPageMaster.Master" AutoEventWireup="true" CodeBehind="FlightChange.aspx.cs" Inherits="GroupBooking.Web.Booking.FlightChange" %>

<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %>
<%@ Register Assembly="SEAL.WEB" Namespace="SEAL.WEB.UI" TagPrefix="cc1" %>



<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolder2">
    <script type="text/javascript" src="../Scripts/jquery-1.4.1.js"></script>

    <script type="text/javascript">
        // <![CDATA[
        $(document).ready(function () {
            var id = getParameterByName('id');
            //alert(document.getElementById('ctl00_ContentPlaceHolder2_hfSelect').value);
            //if (document.getElementById('ctl00_ContentPlaceHolder2_hfSelect').value == "1"){
            //        document.getElementById('ctl00_ContentPlaceHolder2_tablePNR').style.display = 'none';
            //        document.getElementById('ctl00_ContentPlaceHolder2_btBack').style.display = 'inline-table';
                    
                    
            //}
            //else
            //{
            //document.getElementById('ctl00_ContentPlaceHolder2_btBack').style.display = 'none';
            //        document.getElementById('ctl00_ContentPlaceHolder2_tablePNR').style.display = 'block';
            //}
            if (id == "INFT") {
                if ($('#ctl00_ContentPlaceHolder3_SearchDatePanel_cb_OneWay').is(':checked')) {
                    $('#ctl00_ContentPlaceHolder3_SearchDatePanel_tdReturn').hide();
                } else {
                    $('#ctl00_ContentPlaceHolder3_SearchDatePanel_tdReturn').show();
                }

                alert("Cannot add infant");

                //document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = "hallo";

                //pcMessage.Show();
                //LoadingPanel.Hide();
            }

        });

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
        function OnCallbackComplete(s, e) {
            if (e.result != "" && e.result != "search") {
                //lblmsg.SetValue(e.result);
                if (e.result == "Your session has expired.") {
                    window.location.href = '../Invalid.aspx';
                }
                document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = e.result;
                pcMessage.Show();
                LoadingPanel.Hide();
            }
            else if (e.result == 'search') {
                window.location.href = '../public/FlightChange.aspx?k=' + k + '&TransID=' + TransID;
            }
            else {
                //change to new add-On table, Tyas
                window.location.href = '../public/SelectAddOn.aspx?change=true';
                //window.location.href = '../public/AddOn.aspx?change=true';
            }

        }

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

        function OnEndCallBackSearch(s, e) {
            if (typeof s.cp_result != 'undefined' && s.cp_result != "") {
                //lblmsg.SetValue(e.result);
                SelectFlightPanel.SetVisible(false);
                if (typeof btnContinueBottom != "undefined") {
                    btnContinueBottom.SetVisible(false);
                }
                if (typeof buttonL != "undefined") {
                    buttonL.SetVisible(false);
                }
                lblmsg.SetValue(s.cp_result);//document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = e.result;
                pcMessage.Show();
                LoadingPanel.Hide();

            } else {
                //document.getElementById('ctl00_ContentPlaceHolder2_hfSelect').value = "0";
                window.location.href = '../public/FlightChange.aspx?k=' + k + '&TransID=' + TransID + '&callback=1';
            }
        }

        //function onbtnSearchClick(s, e) {
        //   window.location.href = '../public/selectflight.aspx';
        //   LoadingPanel.Hide();

        //}

        function ShowLoginWindow() {
            pcMessage.Show();
        }

        function singleSelect(obj, dlistName) {
            //var elem = this.SkySales.elements; // obj.form.elements;
            //window.alert(obj.id);
            var index = obj.id.split('_')[4];
            var elem = document.getElementById("aspnetForm");
            var datalistName = dlistName;
            //alert(datalistName);
            var str;
            for (var i = 0; i < elem.length; i++) {
                //window.alert(elem[i].type);
                str += elem[i].id + "=" + obj.id + "," + elem[i].name.split('$')[3] + "=" + datalistName + "\n";
                //window.alert(elem[i].name.split('$')[3]);
                //window.alert("ctl00_ContentPlaceHolder2_SelectFlightPanel_dvSelectFlight_IT" + (i) + "_hfChecked");
                if (elem[i].type == "radio" && elem[i].id != obj.id && elem[i].name.split('$')[3] == datalistName)// obj.name.subString( 0 , elem[i].name.indexOf('$') )
                {
                    elem[i].checked = false; //把不是触发click事件的radio状态设置为未选 12.       
                }
                
            }


        }

        $(document).ready(function () {
            ValidateOneWay();
            //SelectedDate();
        });

        function ValidateOneWay() {
            //SelectedDate();
            if ($('#ctl00_ContentPlaceHolder3_SearchDatePanel_cb_OneWay').is(':checked')) {
                $('#ctl00_ContentPlaceHolder3_SearchDatePanel_tdReturn').hide();
                document.getElementById("<%= hOneWay.ClientID %>").value = 1;
            } else {
                $('#ctl00_ContentPlaceHolder3_SearchDatePanel_tdReturn').show();
                document.getElementById("<%= hOneWay.ClientID %>").value = 0;
            }
        }

        function UpdateInfo() {
            var MonthFrom = daStart.GetValue().getMonth() + 1;
            if (MonthFrom <= 9)
                MonthFrom = '0' + MonthFrom;
            var daysTotal = daStart.GetValue().getFullYear() + '/' + MonthFrom + '/' + daStart.GetValue().getDate();
            document.getElementById('ctl00_ContentPlaceHolder2_hfdaStart').value = daysTotal;
            daEnd.Focus();
            daEnd.ShowDropDown();
            //tbInfo.SetText(daysTotal !== -1 ? daysTotal + ' days' : '');
        }

        function UpdateInfoEnd() {
            //var daysTotal = daEnd.GetRangeDayCount();
            //alert('dateEnd');
            var MonthFrom = daEnd.GetValue().getMonth() + 1;
            if (MonthFrom <= 9)
                MonthFrom = '0' + MonthFrom;
            var daysTotal = daEnd.GetValue().getFullYear() + '/' + MonthFrom + '/' + daEnd.GetValue().getDate();
            document.getElementById('ctl00_ContentPlaceHolder2_hfdaEnd').value = daysTotal;
            daEnd.HideDropDown();
            //tbInfo.SetText(daysTotal !== -1 ? daysTotal + ' days' : '');
        }

        function onChangeReturnSta() {
            var ddl = document.getElementById("<%= ddlReturn.ClientID %>");
         var Text = ddl.options[ddl.selectedIndex].text;
         var Value = ddl.options[ddl.selectedIndex].value;
         //alert(Text + " - " + Value);
         document.getElementById("<%= hfArrival.ClientID %>").value = Value;
         document.getElementById("<%= hfArrivalText.ClientID %>").value = Text;
     }

     function gv_Callback(RecordLocator) {

         //var locations = location.href + "&recordlocator=" + RecordLocator;
            
         document.getElementById('ctl00_ContentPlaceHolder2_hfpnr').value = RecordLocator;
         document.getElementById('ctl00_ContentPlaceHolder2_hfSelect').value = "1";
         //alert(document.getElementById('ctl00_ContentPlaceHolder2_hfpnr').value);
     }

     function ClosePopup() {
         popupConfirm.Hide();
     }

     function ShowPopup() {
         document.getElementById('ctl00_ContentPlaceHolder2_hfContinue').value = '1';
         popupConfirm.Show();
     }

     var numCallback = 0;

     // ]]> 
    </script>

    <msg:msgControl ID="msgcontrol" runat="server" />
    <asp:HiddenField ID="hfpnr" runat="server" />
    <asp:HiddenField ID="hfSelect" runat="server" />
    <asp:HiddenField ID="hfSearch" runat="server" />
    <asp:HiddenField ID="hfContinue" runat="server" />
    <asp:HiddenField ID="hfdaStart" runat="server" />
    <asp:HiddenField ID="hfdaEnd" runat="server" />

    <dx:ASPxPopupControl ID="popupConfirm" runat="server" ClientInstanceName="popupConfirm"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        CloseButtonImage-Url="~/Images/AirAsia/close_button_icon.jpg"
        Modal="true" AllowDragging="true"
        HeaderText="Continue Flight Change Confirmation" CloseAction="CloseButton"
        Width="250px">
        <CloseButtonImage Url="~/Images/AirAsia/close_button_icon.jpg"></CloseButtonImage>
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <table width="100%">
                    <tr style="text-align:center;">
                        <td colspan="2">
                            <br />
                            <!-- The cancellation process cannot be undone, please confirm the action -->
                            Are you sure want to change 
                            <br />
                            the Flight Date and Time ?
                        </td>
                    </tr>
                    <tr style="text-align:center;">
                        <td colspan="2" align="center">
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="btnYes" runat="server" Text="Yes" Width="50px" AutoPostBack="False" ClientInstanceName="btnYes">
                                            <ClientSideEvents Click="function(s, e) {ClosePopup();CallBackValidation.PerformCallback();
                                LoadingPanel.SetText(&#39;Please Wait...&#39;);
                                LoadingPanel.Show(); }"></ClientSideEvents>

                                        </dx:ASPxButton>
                                    </td>
                                    <td>&nbsp; </td>
                                    <td>
                                        <dx:ASPxButton ID="btnNo" runat="server" Text="No" Width="50px" AutoPostBack="False">
                                            <ClientSideEvents Click="ClosePopup" />
                                            <ClientSideEvents Click="ClosePopup"></ClientSideEvents>
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

    <div>
        <dx:ASPxCallback ID="CallBackValidation" ClientInstanceName="CallBackValidation" runat="server" OnCallback="ValidatePopup">
            <ClientSideEvents CallbackComplete="OnCallbackComplete" />
        </dx:ASPxCallback>
    </div>

    <div>
        <div class="">

            <div class="row page-header">
                <div class="col-sm-4">
                    <h4 class="mt-0 mb-5">Flight Change</h4>
                    Booking/Flight CHange
                </div>
                <div class="col-sm-8">
                    <div align="right" style="padding-top: 9px;">
                        <table id="bookingDetail">
                            <tr>
                                <td id="tdBack" runat="server">
                                <dx:ASPxButton ID="btBack" runat="server" Text="Back" CssClass="buttonL backBtn" ClientInstanceName="btBack" 
                                    AutoPostBack="True" onclick="btnBack_Click">
                                    <ClientSideEvents Click="function(s, e) { SelectFlightPanel.SetVisible(false); }" />
                                </dx:ASPxButton>
                            </td>
                                <td>
                                    <dx:ASPxButton CssClass="buttonL" ID="btn_Next" runat="server" ClientInstanceName="buttonL"
                                        Text="Continue" AutoPostBack="False" Visible="false">
                                        <ClientSideEvents Click="function(s, e) {ShowPopup();
                                 }" />
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

            </div>

            <div class="row" style="">
                <!--temporary remark-->
                <%--     <table style="width:100%;">
                <tr>
                        <td >
                            <div data-toggle="collapse" data-target="#AdvanceSearchForm" style="width:100%;cursor:pointer;background:#f8f8f8;">
                            <span style="display:inline-block;">
                        <div style="font-size:20px;padding-left:10px;" class="fa fa-search-plus"></div>
                            </span>
                            <span style="display:inline-block;">
                        <h4 class="mt-0 mb-5" style="padding: 12px;">Advanced Search</h4>
                                </span>
                        </div>
                    </td>
                       
                </tr>
            </table>--%>

                <%--<div id="AdvanceSearchForm" class="collapse" style="display:none" >

                <div class="col-md-6">
                    <table>
                        <tr>
                            <td>
                                <div class="checkbox">
                                <label><input type="checkbox" value="">Search by Month</label>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="col-md-6">
                    <table>
                        <tr>
                            <td>
                                <div class="checkbox">
                                <label><input type="checkbox" value="">Same Day Every Month</label>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>--%>
                <div id="tablePNR" runat="server">
                    <div class="col-sm-5" style="padding-left: 40px;">
                        <div style="padding-left: 5px; color: #da0004; font-size: 20px; font-weight: 400;">
                            <h4>Choose one of PNR(s) below to Change the flight</h4>
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
                                                        CommandName="click" CommandArgument='<%#Eval("RecordLocator") %>' Text='<%#Eval("PNR") %>'
                                                        OnClientClick='<%# String.Format("return gv_Callback(&#39;{0}&#39;);", Eval("RecordLocator")) %>'>
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

            </div>
            <div class="page-content row">
                <div class="col-md-12">
                    <dx:ASPxCallbackPanel ID="SelectFlightPanel" runat="server" RenderMode="Div" ClientVisible="true" Height="100%" SettingsLoadingPanel-ShowImage="false"
                        OnCallback="SelectFlightPanel_Callback" ClientInstanceName="SelectFlightPanel">
                        <SettingsLoadingPanel Enabled="false" />
                        <ClientSideEvents EndCallback="function(s, e){ OnEndCallBackSearch(s, e); }"></ClientSideEvents>
                        <PanelCollection>
                            <dx:PanelContent>
                                <!--Departure-->

                                <div class="availFlight tableStyle bodRad-4 col-sm-12 " runat="server" id="tr_Depart" style="display: none">
                                    <div id="departureFlightHeader" class="flightHeader">
                                        <div class="headerPlace">
                                            <h1 class="headerPlaceText">Depart
                                            </h1>
                                        </div>
                                        <div class="flightRoute">
                                            <span class="routeText">
                                                <asp:Label ID="lbl_Go1" runat="server"></asp:Label></span>
                                            <span class="arrowTo">
                                                <span class="fa fa-arrow-right"></span>
                                            </span>
                                            <span class="routeText">
                                                <asp:Label ID="lbl_Go2" runat="server"></asp:Label></span>
                                        </div>
                                    </div>
                                    <ul class="weeklyFlightList" id="departDateDiv" runat="server">
                                    </ul>
                                    <dx:ASPxDataView ID="dvSelectFlight" runat="server" Width="100%"
                                        PagerSettings-Visible="false" ColumnCount="1" Height="50px" AllowPaging="False"
                                        PagerAlign="Left" CssClass="tdClass">

                                        <PagerSettings Visible="False"></PagerSettings>

                                        <ItemStyle Height="0px" HorizontalAlign="Left" Wrap="True" Width="100%">
                                            <Paddings PaddingLeft="1px" PaddingRight="1px" PaddingTop="5px" PaddingBottom="5px"></Paddings>

                                        </ItemStyle>
                                        <ItemTemplate>
                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                <tbody>
                                                    <tr>
                                                        <td align="left" colspan="2">&nbsp;&nbsp;&nbsp;
                                                    <%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightCarrierCode").ToString()%>
                                                        
                                                    &nbsp;<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightFlightNumber").ToString()%>&nbsp;&nbsp;<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightCarrierCode2").ToString()%>&nbsp;<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightFlightNumber2").ToString()%></td>
                                                        <td align="left">
                                                            <%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightDeparture").ToString()%>                                                                    
                                                        </td>
                                                        <td align="left">
                                                            <%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightStd", "{0:dddd, dd MMMM yyyy}")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightStd", "{0:HHmm}")%></td>
                                                        <td align="left">
                                                            <%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightArrival").ToString()%>                                                            
                                                        </td>
                                                        <td align="left">
                                                            <%# (DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightTransit").ToString() != "") ? DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightSta2", "{0:dddd, dd MMMM yyyy}") : DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightSta", "{0:dddd, dd MMMM yyyy}")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%# (DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightTransit").ToString() != "") ? DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightSta2", "{0:HHmm}") : DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightSta", "{0:HHmm}")%></td>
                                                        <td>
                                                            <%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "Insufficient").ToString() != ""? 
                                                                DataBinder.Eval(((IDataItemContainer)Container).DataItem, "Insufficient").ToString():
                                                                ""  %>
                                                            <asp:RadioButton ID="RadioButton1" Visible='<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "Insufficient").ToString() != ""?false:true %>' runat="server" Text="" GroupName="grb1" onclick="javascript:singleSelect(this,'dvSelectFlight');" />
                                                            <asp:HiddenField ID="hfChecked" runat="server" Value='0'></asp:HiddenField>
                                                            <asp:HiddenField ID="lbl_list1ID" runat="server" Value='<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightId").ToString()%>'></asp:HiddenField>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </ItemTemplate>

                                    </dx:ASPxDataView>
                                    <div id="fg_373">
                                    </div>
                                    <!-- remark by ketee -->
                                    <%--<div id="fg_374">
                                    <input id="INPUT_375" value="true" type="checkbox" name="airAsiaAvailability.MarketValueBundles[0]">
                                    <input type="hidden" name="airAsiaAvailability.MarketValueBundles[0]" value="false" id="INPUT_376">
                                </div>--%>
                                </div>
                                <!--Return-->
                                <div class="availFlight tableStyle bodRad-4 col-sm-12" id="tr_Return" runat="server" style="display: none">
                                    <div id="Div4" class="flightHeader">
                                        <div class="headerPlace">
                                            <h1 class="headerPlaceText">Return
                                            </h1>
                                        </div>
                                        <div class="flightRoute">
                                            <span class="routeText">
                                                <asp:Label ID="lbl_Return1" runat="server"></asp:Label></span>
                                            <span class="arrowTo">
                                                <span class="fa fa-arrow-right"></span>
                                            </span>
                                            <span class="routeText">
                                                <asp:Label ID="lbl_Return2" runat="server"></asp:Label></span>
                                        </div>
                                    </div>
                                    <ul class="weeklyFlightList" id="returnDateDiv" runat="server">
                                    </ul>
                                    <dx:ASPxDataView ID="gvSelectFlightReturn" runat="server" Width="100%"
                                        PagerSettings-Visible="false" ColumnCount="1" Height="50px" AllowPaging="False"
                                        PagerAlign="Left" CssClass="tdClass">

                                        <PagerSettings Visible="False"></PagerSettings>

                                        <ItemStyle Height="0px" HorizontalAlign="Left" Wrap="True" Width="100%">
                                            <Paddings PaddingLeft="1px" PaddingRight="1px" PaddingTop="5px" PaddingBottom="5px"></Paddings>

                                        </ItemStyle>
                                        <ItemTemplate>
                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                <tbody>
                                                    <tr>
                                                        <td align="left" colspan="2">&nbsp;&nbsp;&nbsp;
                                                <%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightCarrierCode").ToString()%>
                                                        
                                                &nbsp;<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightFlightNumber").ToString()%>&nbsp;&nbsp;<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightCarrierCode2").ToString()%>&nbsp;<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightFlightNumber2").ToString()%></td>
                                                        <td align="left">
                                                            <%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightDeparture").ToString()%>                                                                    
                                                        </td>
                                                        <td align="left">
                                                            <%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightStd", "{0:dddd, dd MMMM yyyy}")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightStd", "{0:HHmm}")%></td>
                                                        <td align="left">
                                                            <%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightArrival").ToString()%>                                                            
                                                        </td>
                                                        <td align="left">
                                                            <%#(DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightTransit").ToString() != "") ? DataBinder.Eval(Container.DataItem, "TemFlightSta2", "{0:dddd, dd MMMM yyyy}") : DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightSta", "{0:dddd, dd MMMM yyyy}")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%#(DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightTransit").ToString() != "") ? DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightSta2", "{0:HHmm}") : DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightSta", "{0:HHmm}")%></td>
                                                        <td>

                                                            <%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "Insufficient").ToString() != ""? 
                                                                DataBinder.Eval(((IDataItemContainer)Container).DataItem, "Insufficient").ToString():
                                                                ""  %>
                                                            <asp:RadioButton ID="RadioButton2" runat="server" Visible='<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "Insufficient").ToString() != ""? false: true %>' Checked="false" GroupName="grb2" Text="" OnClick="javascript:singleSelect(this,'gvSelectFlightReturn')" />
                                                            <asp:HiddenField ID="lbl_list2ID" runat="server" Value='<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightId").ToString()%>'></asp:HiddenField>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </ItemTemplate>
                                    </dx:ASPxDataView>


                                </div>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxCallbackPanel>
                </div>
            </div>

            <div class="row page-header clearfix">
                <div class="col-sm-6">
                </div>
                <div class="col-sm-6">
                    <table border="0" cellpadding="0" cellspacing="0" style="float: right;">
                        <tr>
                            <td>
                                <dx:ASPxButton ID="btnContinueBottom" runat="server" ClientInstanceName="btnContinueBottom"
                                    CssClass="buttonL" Text="Continue" AutoPostBack="false" Visible="false">
                                    <ClientSideEvents Click="function(s, e) {ShowPopup();
                               }" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>

                </div>
            </div>
        </div>
    </div>

    </div>


</asp:Content>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder3">
    <link rel="Stylesheet" href="../Styles/chosen.css" />

    <dx:ASPxCallbackPanel ID="SearchDatePanel" runat="server" RenderMode="Div" ClientVisible="true" Height="100%" ClientInstanceName="SearchDatePanel" OnCallback="SearchDate_Callback">
        <ClientSideEvents EndCallback="function(s, e){ formatDDL(); }"></ClientSideEvents>
        <SettingsLoadingPanel Enabled="false" />
        <PanelCollection>
            <dx:PanelContent>
                <div class="padd10" style="background: transparent; padding-bottom: 100px; color: #fff;">
                    <div id="searchFlightForm" class="collapse in sidebar-widget">
                        <ul class="list-unstyled pr-25">
                            <li>
                                <div style="width: 100%; text-align: center;">
                                    <h4 style="margin-top: 0; display: inline-block;">Flight Search</h4>

                                </div>
                                <div class="block clearfix mb-10" style="width: 190px;">
                                    <table id="topPartSearchForm">

                                        <tr>
                                            <td colspan="3"><span style="font-size: 12px; display: block; margin: 10px 0;">Online Booking is now available up to 4hrs before flights
                                            </span>
                                                <div runat="server" id="divRestriction" style="display: none; color: Red; font-weight: bold" class="search-form-top">
                                                    <dx:ASPxLabel ID="lblRestriction" runat="server"></dx:ASPxLabel>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="search-form-top">
                                                    <dx:ASPxLabel runat="server" ID="lblMsg" ClientInstanceName="lblMsg" Text="" ForeColor="Red"></dx:ASPxLabel>
                                                </div>
                                            </td>
                                        </tr>
                                        <%--<tr>
                                <td class="alignRadiobtn">
                                    <input type="checkbox" runat="server" name="cb_OneWay" id="cb_OneWay" onchange="javascript:ValidateOneWay();" value="One Way Only" />One Way Only
                                </td>
                                <td class="alignRadiobtn">
                                    
                                </td>

                            </tr>--%>
                                    </table>
                                </div>
                            </li>
                            <li>
                                <div>
                                </div>
                            </li>
                            <li>
                                <div class="block clearfix mb-10" style="width: 190px;">
                                    <cc1:CustomDropDownList ID="ddlDeparture" runat="server" Width="184px"
                                        AutoPostBack="false" CssClass="chzn-select" TabIndex="1" Enabled="false">
                                        <asp:ListItem Text="Origin"></asp:ListItem>
                                    </cc1:CustomDropDownList>
                                </div>
                            </li>
                            <li>
                                <div class="block clearfix mb-10" style="width: 190px;">
                                    <table>
                                        <tr>
                                            <td valign="top">
                                                <div class="controlledCollapse collapse in">
                                                    <cc1:CustomDropDownList ID="ddlReturn" runat="server" Width="184px" CssClass="chzn-select" Enabled="false">
                                                        <asp:ListItem Text="Destination" TabIndex="2"></asp:ListItem>
                                                    </cc1:CustomDropDownList>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" style="padding-top: 9px;">
                                                <div class="coverDropDown">
                                                    <asp:DropDownList ID="ddl_Currency" runat="server"
                                                        Enabled="False" Width="184px">
                                                        <asp:ListItem Value="USD">Select currency</asp:ListItem>

                                                    </asp:DropDownList>
                                                </div>
                                            </td>
                                        </tr>

                                    </table>
                                </div>
                            </li>
                            <li>
                                <div class="block clearfix mb-10" style="width: 190px;">
                                    <div id="marketDate_1" class="depart-date left black3" style="display: none;">
                                        <div id="divDate1" runat="server">
                                        </div>
                                    </div>
                                    <dx:ASPxDateEdit ID="daStart" ClientInstanceName="daStart" runat="server" NullText="Depart" EditFormatString="dd/MM/yyyy" Width="184px">
                                        <ClientSideEvents DateChanged="function(s,e) {UpdateInfo();}"></ClientSideEvents>
                                        <ValidationSettings Display="Dynamic" ValidationGroup="SearchPanel" SetFocusOnError="True" CausesValidation="True" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField IsRequired="True" ErrorText="Start date is required"></RequiredField>
                                        </ValidationSettings>
                                    </dx:ASPxDateEdit>
                                </div>
                            </li>
                            <% if (Session["returnFlight"] != "false") { %>
                            <li id="tdReturn" runat="server" style="display: block">
                                <div class="block clearfix mb-10" style="width: 190px;">
                                    <div id="marketDate_2" class="depart-date left black3" style="display: none;">
                                        <div id="divDate2" runat="server">
                                        </div>
                                    </div>
                                    <dx:ASPxDateEdit ID="daEnd" ClientInstanceName="daEnd" runat="server" NullText="Return" EditFormatString="dd/MM/yyyy" Width="184px">
                                        <DateRangeSettings StartDateEditID="daStart"></DateRangeSettings>
                                        <ClientSideEvents DateChanged="function(s,e) {UpdateInfoEnd();} "></ClientSideEvents>
                                        <ValidationSettings Display="Dynamic" ValidationGroup="SearchPanel" SetFocusOnError="True" CausesValidation="True" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField IsRequired="True" ErrorText="End date is required"></RequiredField>
                                        </ValidationSettings>
                                    </dx:ASPxDateEdit>
                                </div>
                            </li>
                            <% } %>
                            <li>
                                <div class="block clearfix mb-10">
                                    <table>
                                        <tr>
                                            <td>
                                                <table id="numOfPeopleTable">
                                                    <tr>
                                                        <td id="guestNumTd">
                                                            <%--<asp:TextBox ID="txt_GuestNum"  runat="server" style= "text-align:right " CssClass="input" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))) {event.returnValue=true;} else{event.returnValue=false;}" ></asp:TextBox>--%>
                                                            <dx:ASPxSpinEdit NumberType="Integer" Width="90px" CssClass="input-sm" HorizontalAlign="Right" AllowMouseWheel="true" AllowUserInput="true" SpinButtons-ShowIncrementButtons="false" Enabled="false"
                                                                runat="server" ID="txt_GeustNum" NullText="Adult" MinValue="0" MaxValue="50">
                                                            </dx:ASPxSpinEdit>
                                                        </td>


                                                        <td id="childNumTd" style="padding: 5px;">
                                                            <%--<asp:TextBox ID="txt_ChildNum"   runat="server" CssClass="input" style= "text-align:right " OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))) {event.returnValue=true;} else{event.returnValue=false;}" ></asp:TextBox>--%>
                                                            <dx:ASPxSpinEdit NumberType="Integer" Width="90px" CssClass="input-sm" HorizontalAlign="Right" AllowMouseWheel="true" AllowUserInput="true" SpinButtons-ShowIncrementButtons="false" Enabled="false"
                                                                runat="server" ID="txt_ChildNum" NullText="Child" MinValue="0" MaxValue="50">
                                                            </dx:ASPxSpinEdit>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id="Td1">
                                                            <%--<asp:TextBox ID="txt_InfantNum"   runat="server" CssClass="input" style= "text-align:right " OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))) {event.returnValue=true;} else{event.returnValue=false;}" ></asp:TextBox>--%>
                                                            <dx:ASPxSpinEdit NumberType="Integer" Width="90px" CssClass="input-sm" HorizontalAlign="Right" AllowMouseWheel="true" AllowUserInput="true" SpinButtons-ShowIncrementButtons="false" Enabled="false" ClientVisible="false"
                                                                runat="server" ID="txt_InfantNum" NullText="Infant" MinValue="0" MaxValue="10">
                                                            </dx:ASPxSpinEdit>
                                                        </td>
                                                        <td align="right" valign="bottom" style="padding: 5px;">
                                                            <dx:ASPxButton CssClass="button-3"
                                                                ID="btnSubMit" runat="server" Text="Search"
                                                                AutoPostBack="false">
                                                                <ClientSideEvents Click="function(s, e) {document.getElementById('ctl00_ContentPlaceHolder2_hfSearch').value = '1';SelectFlightPanel.PerformCallback('search');
                                                                    LoadingPanel.SetText(&#39;Please Wait...&#39;);
                                                                   LoadingPanel.Show(); }" />
                                                            </dx:ASPxButton>

                                                        </td>
                                                    </tr>
                                                </table>

                                            </td>

                                        </tr>
                                    </table>
                                </div>
                            </li>
                        </ul>

                        <input type="hidden" id="hfDeparture" runat="server" />
                        <input type="hidden" id="hfArrival" runat="server" />
                        <input type="hidden" id="hfArrivalText" runat="server" />
                        <input type="hidden" id="hfCurrency" runat="server" />

                        <input type="hidden" id="hfdeEnd" runat="server" />
                        <input type="hidden" id="hfAdultNum" runat="server" />
                        <input type="hidden" id="hfChildNum" runat="server" />
                        <input type="hidden" id="hfInfantNum" runat="server" />
                        <input type="hidden" id="hOneWay" runat="server" />
                    </div>
                </div>

                <script id="dxss_chosen" type="text/javascript" src="../Scripts/chosen.jquery.js"></script>
                <script type="text/javascript">
                    $(document).ready(function () {
                        var partToHighLight = null;
                        var sameGrid = null;

                        $('[type="radio"]').click(function () {
                            partToHighLight = $(this).parent().parent().parent().parent().parent(); //get the active row to be styled
                            var thisGridWrapper = partToHighLight.parent().parent().parent().parent();

                            if (thisGridWrapper.find('.selectedFlight').length != 0) thisGridWrapper.find('.selectedFlight').removeClass('selectedFlight');

                            if ($(this).is(':checked')) {
                                partToHighLight.addClass('selectedFlight');
                                sameGrid = partToHighLight;
                            }


                        });
                    });

                    $(".chzn-select").chosen();
                    $(".chzn-select-deselect").chosen({
                        allow_single_deselect: true
                    });

                    function formatDDL() {
                        ValidateOneWay();
                        $(".chzn-select").chosen();
                        $(".chzn-select-deselect").chosen({
                            allow_single_deselect: true
                        });
                        //$.getScript('../Scripts/chosen.jquery.js', function () {
                        //    $(".chzn-select").chosen();
                        //    $(".chzn-select-deselect").chosen({
                        //        allow_single_deselect: true
                        //    });
                        //});
                    }
                </script>

            </dx:PanelContent>

        </PanelCollection>
    </dx:ASPxCallbackPanel>

</asp:Content>
