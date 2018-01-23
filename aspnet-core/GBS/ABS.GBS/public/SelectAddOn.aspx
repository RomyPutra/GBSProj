<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewPageMaster.Master" AutoEventWireup="true" CodeBehind="SelectAddOn.aspx.cs" Inherits="GroupBooking.Web.AddOns" %>

<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %>
<%@ Register Src="../UserControl/flightbookingdetail.ascx" TagName="ucflightbookingdetail" TagPrefix="FBD" %>
<%@ Register Src="../UserControl/flighdetail.ascx" TagName="flightdetail" TagPrefix="fd" %>

<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v16.1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript" src="../Scripts/Booking/AddOn.js?ver=1.6"></script>

    <script type="text/javascript">
      
    </script>

    <msg:msgControl ID="msgcontrol" runat="server" />
    <div>
        <dx:ASPxCallback ID="CallBackValidation" ClientInstanceName="CallBackValidation" runat="server" OnCallback="ValidatePopup">
            <ClientSideEvents CallbackComplete="OnCallbackComplete" />
        </dx:ASPxCallback>
    </div>
    <div class="row page-header clearfix">
        <div class="col-sm-3">
            <h4 class="mt-0 mb-5">Add-On</h4>
            Booking/Add-On
        </div>
        <!--<div class="col-sm-5">
        </div>-->
        <div class="col-sm-9">
            <div style="float: right;">
                <dx:ASPxButton CssClass="buttonL" ID="btn_Next" runat="server"
                    Text="Continue" AutoPostBack="False">
                    <ClientSideEvents Click="function(s, e) { if (gvPassenger.batchEditApi.HasChanges()) {
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
                                           LoadingPanel.Show(); }}" />
                </dx:ASPxButton>
                <dx:ASPxButton CssClass="buttonSA" ID="btn_CancelChange" runat="server" Text="Cancel" AutoPostBack="False">
                    <ClientSideEvents Click="function(s, e) {gvPassenger.CancelEdit();gvPassenger2.CancelEdit();}" />
                </dx:ASPxButton>
                <dx:ASPxButton CssClass="buttonSA" ID="btn_SaveChange" runat="server" Text="Save Changes" AutoPostBack="False">
                    <ClientSideEvents Click="function(s, e) {gvPassenger.UpdateEdit();gvPassenger2.UpdateEdit();}" />
                </dx:ASPxButton>
            </div>
        </div>
    </div>


    <div class="widget page-content container-fluid" style="border-color: transparent;">
        <dx:ASPxCallbackPanel ID="SSRActionPanel" runat="server" RenderMode="Div" ClientVisible="true" Height="50%" ClientInstanceName="SSRActionPanel"
            OnCallback="SSRActionPanel_PerformCallback" SettingsLoadingPanel-Enabled="false">
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

        <div id="tabCtrlWrapper">
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
                                                <dx:ASPxCallbackPanel ID="SSRTab1Panel" runat="server" RenderMode="Div" ClientVisible="true" Height="50%" ClientInstanceName="SSRTab1Panel"
                                                    OnCallback="SSRTab1Panel_PerformCallback" SettingsLoadingPanel-Enabled="false">
                                                    <%--<ClientSideEvents EndCallback="function(s, e){ SSRActionPanelEndCallBack();}"></ClientSideEvents>--%>
                                                    <PanelCollection>
                                                        <dx:PanelContent>
                                                            <div class="buttonsWrapper col-sm-12 nav nav-pills">
                                                                <div class="col-sm-2">
                                                                    <a href="#" onclick="showcollapse1()">
                                                                        <div class="btnCtrl roundedCorner">

                                                                            <div class="iconBtn fa fa-suitcase"></div>

                                                                        </div>
                                                                        <div class="title">Baggage</div>
                                                                        <dx:ASPxLabel ID="lblTotalBaggage" runat="server" Text='0.00' ClientInstanceName="lblTotalBaggage" />
                                                                    </a>
                                                                </div>
                                                                <div class="col-sm-2">
                                                                    <a href="#" onclick="showcollapse2()">
                                                                        <div class="btnCtrl roundedCorner">

                                                                            <div class="iconBtn fa fa-cutlery"></div>

                                                                        </div>
                                                                        <div class="title">Meal</div>
                                                                        <dx:ASPxLabel ID="lblTotalMeal" runat="server" Text='0.00' ClientInstanceName="lblTotalMeal" />
                                                                    </a>
                                                                </div>
                                                                <div class="col-sm-2">
                                                                    <a href="#" onclick="showcollapse3()">
                                                                        <div class="btnCtrl roundedCorner">

                                                                            <div class="iconBtn fa fa-futbol-o"></div>

                                                                        </div>
                                                                        <div class="title">Sport Equipment</div>
                                                                        <dx:ASPxLabel ID="lblTotalSport" runat="server" Text='0.00' ClientInstanceName="lblTotalSport" />
                                                                    </a>
                                                                </div>
                                                                <div class="col-sm-2">
                                                                    <a href="#" onclick="showcollapse4()">
                                                                        <div class="btnCtrl roundedCorner">

                                                                            <div class="iconBtn fa fa-bed"></div>

                                                                        </div>
                                                                        <div class="title">Comfort Kit</div>
                                                                        <dx:ASPxLabel ID="lblTotalComfort" runat="server" Text='0.00' ClientInstanceName="lblTotalComfort" />
                                                                    </a>
                                                                </div>
                                                                <div class="col-sm-2" runat="server" id="InfantIcon1">
                                                                    <a href="#" onclick="showcollapse6()" title="Please add Infant through the selected passenger Infant column" data-toggle="tooltip" data-placement="bottom">
                                                                        <div class="btnCtrl roundedCorner">

                                                                            <div class="iconBtn fa fa-child"></div>

                                                                        </div>
                                                                        <div class="title">Infant</div>
                                                                        <dx:ASPxLabel ID="lblTotalInfant" runat="server" Text='0.00' ClientInstanceName="lblTotalInfant" />
                                                                    </a>
                                                                </div>
                                                                <div class="col-sm-2" style="display: none">
                                                                    <a href="#" onclick="showcollapse5()">
                                                                        <div class="btnCtrl roundedCorner">

                                                                            <div class="iconBtn fa fa-gift"></div>

                                                                        </div>
                                                                        <div class="title">Duty Free</div>
                                                                        <dx:ASPxLabel ID="lblTotalDuty" runat="server" Text='0.00' ClientInstanceName="lblTotalDuty" />
                                                                    </a>
                                                                </div>
                                                            </div>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dx:ASPxCallbackPanel>
                                                <div class="contentWrapper col-sm-12">
                                                    <%--div class="tab-content">--%>
                                                    <div id="collapse1" style="visibility: hidden;">

                                                        <div class="baggageAddonWrapper">
                                                            <div style="padding-top: 10px;">
                                                                <table>
                                                                    <tbody>
                                                                        <tr>
                                                                            <td style="padding-top: 10px;">
                                                                                <div class="controlledCollapse collapse in">
                                                                                    <dx:ASPxComboBox ID="cmbBaggage" ClientInstanceName="cmbBaggage" runat="server" class="Select" AutoPostBack="false" IncrementalFilteringMode="StartsWith" Width="296px">
                                                                                        <ClientSideEvents CloseUp="function(s, e) {setTimeout(function() { seBaggage.Focus(); }, 0)}" />
                                                                                    </dx:ASPxComboBox>
                                                                                </div>
                                                                            </td>

                                                                            <td>
                                                                                <table class="addBaggageInput" style="margin-top: 10px;">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <input type="checkbox" runat="server" name="cbAllPaxBaggage1" id="cbAllPaxBaggage1" onchange="javascript:AllPaxBaggage1();" value="1" />All Pax
                                                                                            </td>
                                                                                            <td style="padding-left: 10px;">
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
                                                    <div id="collapse2" class="overflowCtr" style="visibility: hidden;">
                                                        <div class="baggageAddonWrapper" style="padding-top: 10px;">
                                                            <div class="mealAll">
                                                                <div class="showDropMeal collapse in" style="margin-bottom: 10px;">
                                                                    <table>
                                                                        <tbody>

                                                                            <tr>
                                                                                <td style="padding-top: 10px;">
                                                                                    <div class="controlledCollapse collapse in">

                                                                                        <dx:ASPxGridLookup ID="glMeals" runat="server" KeyFieldName="SSRCode" Width="300px" EnableRowsCache="false" ClientInstanceName="glMeals"
                                                                                            SelectionMode="Single" TextFormatString="{0} {1}" OnInit="glMeals_Init" OnAutoFilterCellEditorInitialize="glMeals_AutoFilterCellEditorInitialize">
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
                                                                                            <ClientSideEvents RowClick="function(s, e) {setTimeout(function() { cmbDrinks.SetSelectedIndex(0);cmbDrinks.SetEnabled(false);seMeals.Focus(); }, 0)}" />
                                                                                        </dx:ASPxGridLookup>
                                                                                    </div>
                                                                                </td>

                                                                                <td style="padding-top: 10px;" id="tdDrinks" runat="server">
                                                                                    <div class="controlledCollapse collapse in">
                                                                                        <dx:ASPxComboBox ID="cmbDrinks" ClientInstanceName="cmbDrinks" runat="server" class="Select" AutoPostBack="false" IncrementalFilteringMode="StartsWith" Width="296px">
                                                                                        </dx:ASPxComboBox>

                                                                                    </div>
                                                                                </td>

                                                                                <td>
                                                                                    <table class="addBaggageInput">
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <input type="checkbox" runat="server" name="cbAllPaxMeal11" id="cbAllPaxMeal11" onchange="javascript:AllPaxMeal11();" value="1" />All Pax
                                                                                                </td>
                                                                                                <td style="padding-left: 10px;">
                                                                                                    <dx:ASPxSpinEdit ID="seMeals" ClientInstanceName="seMeals" runat="server" Number="1" NumberType="Integer" MinValue="1" MaxValue="50" Width="50px">
                                                                                                    </dx:ASPxSpinEdit>
                                                                                                </td>
                                                                                                <td style="padding-left: 10px;">
                                                                                                    <button type="button" class="btn btn-danger" onclick="AddMeal()">Assign</button>
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

                                                        <div class="baggageAddonWrapper" style="padding-top: 10px;" id="divmeal1" runat="server">
                                                            <div class="mealAll">
                                                                <div class="showDropMeal collapse in" style="margin-bottom: 10px;">
                                                                    <table>
                                                                        <tbody>

                                                                            <tr>
                                                                                <td style="padding-top: 10px;">
                                                                                    <div class="controlledCollapse collapse in">
                                                                                        <dx:ASPxGridLookup ID="glMeals1" runat="server" KeyFieldName="SSRCode" Width="300px" EnableRowsCache="false" ClientInstanceName="glMeals1"
                                                                                            SelectionMode="Single" TextFormatString="{0} {1}" OnInit="glMeals1_Init" OnAutoFilterCellEditorInitialize="glMeals1_AutoFilterCellEditorInitialize">
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
                                                                                            <ClientSideEvents RowClick="function(s, e) {setTimeout(function() { cmbDrinks1.SetSelectedIndex(0);cmbDrinks1.SetEnabled(false);seMeals1.Focus(); }, 0)}" />
                                                                                        </dx:ASPxGridLookup>
                                                                                    </div>
                                                                                </td>
                                                                                <td style="padding-top: 10px;" id="tdDrinks1" runat="server">
                                                                                    <div class="controlledCollapse collapse in">

                                                                                        <dx:ASPxComboBox ID="cmbDrinks1" ClientInstanceName="cmbDrinks1" runat="server" class="Select" AutoPostBack="false" IncrementalFilteringMode="StartsWith" Width="296px">
                                                                                        </dx:ASPxComboBox>

                                                                                    </div>
                                                                                </td>

                                                                                <td>
                                                                                    <table class="addBaggageInput" style="margin-top: 10px;">
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <input type="checkbox" runat="server" name="cbAllPaxMeal21" id="cbAllPaxMeal21" onchange="javascript:AllPaxMeal21();" value="1" />All Pax
                                                                                                </td>
                                                                                                <td style="padding-left: 10px;">
                                                                                                    <dx:ASPxSpinEdit ID="seMeals1" ClientInstanceName="seMeals1" runat="server" Number="1" NumberType="Integer" MinValue="1" MaxValue="50" Width="50px">
                                                                                                    </dx:ASPxSpinEdit>
                                                                                                </td>
                                                                                                <td style="padding-left: 10px;">
                                                                                                    <button type="button" class="btn btn-danger" onclick="AddMeal1()">Assign</button>
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
                                                    <div id="collapse3" style="visibility: hidden;">
                                                        <div class="baggageAddonWrapper" style="padding-top: 10px;">

                                                            <div class="mealAll">

                                                                <div class="showDropMeal collapse in" style="margin-bottom: 10px;">
                                                                    <table>
                                                                        <tbody>

                                                                            <tr>
                                                                                <td style="padding-top: 10px;">
                                                                                    <div class="controlledCollapse collapse in">
                                                                                        <dx:ASPxComboBox ID="cmbSport" ClientInstanceName="cmbSport" runat="server" class="Select" AutoPostBack="false" IncrementalFilteringMode="StartsWith" Width="296px">
                                                                                            <ClientSideEvents CloseUp="function(s, e) {setTimeout(function() { seSport.Focus(); }, 0)}" />
                                                                                        </dx:ASPxComboBox>
                                                                                    </div>

                                                                                </td>
                                                                                <td>
                                                                                    <table class="addBaggageInput" style="margin-top: 10px;">
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <input type="checkbox" runat="server" name="cbAllPaxSport1" id="cbAllPaxSport1" onchange="javascript:AllPaxSport1();" value="1" />All Pax
                                                                                                </td>
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
                                                    <div id="collapse4" style="visibility: hidden;">
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
                                                                                    <ClientSideEvents CloseUp="function(s, e) {setTimeout(function() { seComfort.Focus(); }, 0)}" />
                                                                                </dx:ASPxGridLookup>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <table class="addBaggageInput" style="margin-top: 10px;">
                                                                                <tr>
                                                                                    <td>
                                                                                        <input type="checkbox" runat="server" name="cbAllPaxComfort1" id="cbAllPaxComfort1" onchange="javascript:AllPaxComfort1();" value="1" />All Pax
                                                                                    </td>
                                                                                    <td style="padding-left: 10px;">
                                                                                        <dx:ASPxSpinEdit ID="seComfort" ClientInstanceName="seComfort" runat="server" Number="1" NumberType="Integer" MinValue="1" MaxValue="50" Width="50px">
                                                                                        </dx:ASPxSpinEdit>
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
                                                    <div id="collapse5" style="visibility: hidden;">
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
                                                                                    <ClientSideEvents CloseUp="function(s, e) {setTimeout(function() { seDuty.Focus(); }, 0)}" />
                                                                                </dx:ASPxGridLookup>
                                                                            </div>
                                                                        </td>

                                                                        <td>
                                                                            <table class="addBaggageInput" style="margin-top: 10px;">
                                                                                <tr>
                                                                                    <td>
                                                                                        <input type="checkbox" runat="server" name="cbAllPaxDuty1" id="cbAllPaxDuty1" onchange="javascript:AllPaxDuty1();" value="1" />All Pax
                                                                                    </td>
                                                                                    <td style="padding-left: 10px;">
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
                                                    <div id="collapse6" style="visibility: hidden;">
                                                        <div class="baggageAddonWrapper">

                                                            <div style="padding-top: 10px;">
                                                                <table class="width100">

                                                                    <tr>
                                                                        <td style="padding-top: 10px;">
                                                                            <div class="controlledCollapse collapse in informationBlock">
                                                                                <h4>Please add Infant through the selected passenger Infant column</h4>
                                                                            </div>
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
                                    <div class="div" style="margin-bottom: 20px;">
                                        <h4>Passenger List</h4>
                                    </div>

                                    <dx:ASPxGridView ID="gvPassenger" runat="server" KeyFieldName="SeqNo;PassengerID" EnableRowsCache="false" EnableCallBacks="true"
                                        ClientInstanceName="gvPassenger" Width="100%" OnCustomCallback="gvPassenger_CustomCallback" OnDataBinding="gvPassenger_DataBinding"
                                        OnBatchUpdate="gvPassenger_BatchUpdate" OnHtmlDataCellPrepared="gvPassenger_HtmlDataCellPrepared" >
                                        <ClientSideEvents EndCallback="function(s, e) {gvPassenger_EndCallback(s, e);}" />

                                        <Columns>
                                            <dx:GridViewDataColumn FieldName="PNRs" Caption="PNR" VisibleIndex="0" GroupIndex="0" />
                                            <dx:GridViewDataColumn FieldName="SeqNo" Caption="SeqNo" ReadOnly="true" Width="70px" />
                                            <dx:GridViewDataColumn FieldName="PassengerID" Caption="PassengerID" Visible="false" VisibleIndex="0" Width="70px" />

                                            <dx:GridViewDataTextColumn FieldName="FirstName" Caption="First Name" ReadOnly="true" EditFormSettings-Visible="False" Width="70px">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="LastName" Caption="Last Name" ReadOnly="true" EditFormSettings-Visible="False" Width="70px" />
                                            <dx:GridViewDataTextColumn FieldName="PaxType" Caption="Pax Type" ReadOnly="true" Width="70px" />
                                            <dx:GridViewDataComboBoxColumn FieldName="Baggage" Caption="Baggage" Width="120px">
                                                <PropertiesComboBox TextField="Detail" ValueField="SSRCode" ValueType="System.String">
                                                </PropertiesComboBox>
                                            </dx:GridViewDataComboBoxColumn>
                                            <dx:GridViewDataComboBoxColumn FieldName="Meal" Caption="Meal" Width="120px">
                                                <PropertiesComboBox TextField="Detail" ValueField="SSRCode"
                                                    ValueType="System.String">
                                                </PropertiesComboBox>
                                                <EditItemTemplate>
                                                    <dx:ASPxGridLookup ID="glMealP1" runat="server" KeyFieldName="SSRCode" Width="100%" EnableRowsCache="false" ClientInstanceName="glMealP1"
                                                        SelectionMode="Single" TextFormatString="{0} {1}" OnInit="glMealP1_Init">
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
                                                        <ClientSideEvents RowClick="function(s, e) {LoadingPanel.Show();}" EndCallback="function(s, e) {LoadingPanel.Hide();}" />
                                                    </dx:ASPxGridLookup>
                                                </EditItemTemplate>
                                            </dx:GridViewDataComboBoxColumn>
                                            <dx:GridViewDataComboBoxColumn FieldName="Drink" Caption="Drink" Width="150px">
                                                <PropertiesComboBox TextField="Detail" ValueField="SSRCode" ValueType="System.String">
                                                </PropertiesComboBox>
                                            </dx:GridViewDataComboBoxColumn>
                                            <dx:GridViewDataComboBoxColumn FieldName="Meal1" Caption="Meal 2" Width="120px">
                                                <PropertiesComboBox TextField="Detail" ValueField="SSRCode"
                                                    ValueType="System.String">
                                                </PropertiesComboBox>
                                                <EditItemTemplate>
                                                    <dx:ASPxGridLookup ID="glMealP11" runat="server" KeyFieldName="SSRCode" Width="100%" EnableRowsCache="false" ClientInstanceName="glMealP11"
                                                        SelectionMode="Single" TextFormatString="{0} {1}" OnInit="glMealP11_Init">
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
                                                        <ClientSideEvents RowClick="function(s, e) {LoadingPanel.Show();}" EndCallback="function(s, e) {LoadingPanel.Hide();}" />
                                                    </dx:ASPxGridLookup>
                                                </EditItemTemplate>
                                            </dx:GridViewDataComboBoxColumn>
                                            <dx:GridViewDataComboBoxColumn FieldName="Drink1" Caption="Drink 2" Width="150px">
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
                                                    <dx:ASPxGridLookup ID="glComfortP1" runat="server" KeyFieldName="SSRCode" Width="100%" EnableRowsCache="false" ClientInstanceName="glComfortP1"
                                                        SelectionMode="Single" TextFormatString="{0} {1}" OnInit="glComfortP1_Init">
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
                                                        <ClientSideEvents RowClick="function(s, e) {LoadingPanel.Show();}" EndCallback="function(s, e) {LoadingPanel.Hide();}" />
                                                    </dx:ASPxGridLookup>
                                                </EditItemTemplate>
                                            </dx:GridViewDataComboBoxColumn>
                                            <dx:GridViewDataComboBoxColumn FieldName="Infant" Caption="Infant" Width="120px" CellStyle-CssClass="infantCol">
                                                <PropertiesComboBox TextField="Detail" ValueField="SSRCode" ValueType="System.String" >
                                                </PropertiesComboBox>
                                            </dx:GridViewDataComboBoxColumn>
                                            <dx:GridViewCommandColumn Name="Action" Caption="Action" Width="70px">
                                                <CustomButtons>
                                                    <dx:GridViewCommandColumnCustomButton ID="ClearLink" runat="server" Text="Clear">
                                                    </dx:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                            </dx:GridViewCommandColumn>
                                        </Columns>
                                        <ClientSideEvents BatchEditEndEditing="onBatchEditEndEditing" CustomButtonClick="OnCustomButtonClick" />

                                        <SettingsPager Mode="ShowPager" PageSize="11" Position="Top" />
                                        <SettingsEditing Mode="Batch" />
                                        <SettingsBehavior AutoExpandAllGroups="true" />
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
                                                <dx:ASPxCallbackPanel ID="SSRTab2Panel" runat="server" RenderMode="Div" ClientVisible="true" Height="50%" ClientInstanceName="SSRTab2Panel"
                                                    OnCallback="SSRTab2Panel_PerformCallback" SettingsLoadingPanel-Enabled="false">
                                                    <%--<ClientSideEvents EndCallback="function(s, e){ SSRActionPanelEndCallBack();}"></ClientSideEvents>--%>
                                                    <PanelCollection>
                                                        <dx:PanelContent>
                                                            <div class="buttonsWrapper col-sm-12 nav nav-pills">
                                                                <div class="col-sm-2">
                                                                    <a href="#" onclick="showcollapse12()">
                                                                        <div class="btnCtrl roundedCorner">

                                                                            <div class="iconBtn fa fa-suitcase"></div>


                                                                        </div>
                                                                        <div class="title">Baggage</div>
                                                                        <dx:ASPxLabel ID="lblTotalBaggage2" runat="server" Text='0.00' ClientInstanceName="lblTotalBaggage2" />
                                                                    </a>
                                                                </div>
                                                                <div class="col-sm-2">
                                                                    <a href="#" onclick="showcollapse22()">
                                                                        <div class="btnCtrl roundedCorner">

                                                                            <div class="iconBtn fa fa-cutlery"></div>

                                                                        </div>
                                                                        <div class="title">Meal</div>
                                                                        <dx:ASPxLabel ID="lblTotalMeal2" runat="server" Text='0.00' ClientInstanceName="lblTotalMeal2" />
                                                                    </a>
                                                                </div>
                                                                <div class="col-sm-2">
                                                                    <a href="#" onclick="showcollapse32()">
                                                                        <div class="btnCtrl roundedCorner">

                                                                            <div class="iconBtn fa fa-futbol-o"></div>

                                                                        </div>
                                                                        <div class="title">Sport Equipment</div>
                                                                        <dx:ASPxLabel ID="lblTotalSport2" runat="server" Text='0.00' ClientInstanceName="lblTotalSport2" />
                                                                    </a>
                                                                </div>
                                                                <div class="col-sm-2">
                                                                    <a href="#" onclick="showcollapse42()">
                                                                        <div class="btnCtrl roundedCorner">

                                                                            <div class="iconBtn fa fa-bed"></div>

                                                                        </div>
                                                                        <div class="title">Comfort Kit</div>
                                                                        <dx:ASPxLabel ID="lblTotalComfort2" runat="server" Text='0.00' ClientInstanceName="lblTotalComfort2" />
                                                                    </a>
                                                                </div>
                                                                <div class="col-sm-2" runat="server" id="InfantIcon2">
                                                                    <a href="#" onclick="showcollapse62()" title="Please add Infant through the selected passenger Infant column">
                                                                        <div class="btnCtrl roundedCorner">

                                                                            <div class="iconBtn fa fa-child"></div>

                                                                        </div>
                                                                        <div class="title">Infant</div>
                                                                        <dx:ASPxLabel ID="lblTotalInfant2" runat="server" Text='0.00' ClientInstanceName="lblTotalInfant2" />
                                                                    </a>
                                                                </div>
                                                                <div class="col-sm-2" style="display: none">
                                                                    <div class="btnCtrl roundedCorner">
                                                                        <a href="#" onclick="showcollapse52()">
                                                                            <div class="iconBtn fa fa-gift"></div>
                                                                        </a>
                                                                    </div>
                                                                    <div class="title">Duty Free</div>
                                                                    <dx:ASPxLabel ID="lblTotalDuty2" runat="server" Text='0.00' ClientInstanceName="lblTotalDuty2" />
                                                                </div>
                                                            </div>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dx:ASPxCallbackPanel>
                                                <div class="contentWrapper col-sm-12">
                                                    <div id="collapse12" style="visibility: hidden;">

                                                        <div class="baggageAddonWrapper">
                                                            <div style="padding-top: 10px;">
                                                                <table>
                                                                    <tbody>
                                                                        <tr>
                                                                            <td style="padding-top: 10px;">
                                                                                <div class="controlledCollapse collapse in">
                                                                                    <dx:ASPxComboBox ID="cmbBaggage2" ClientInstanceName="cmbBaggage2" runat="server" class="Select" AutoPostBack="false" IncrementalFilteringMode="StartsWith" Width="296px">
                                                                                        <ClientSideEvents CloseUp="function(s, e) {setTimeout(function() { seBaggage2.Focus(); }, 0)}" />
                                                                                    </dx:ASPxComboBox>
                                                                                </div>
                                                                            </td>

                                                                            <td>
                                                                                <table class="addBaggageInput" style="margin-top: 10px;">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <input type="checkbox" runat="server" name="cbAllPaxBaggage2" id="cbAllBaggage2" onchange="javascript:AllPaxBaggage2();" value="1" />All Pax
                                                                                            </td>
                                                                                            <td style="padding-left: 10px;">
                                                                                                <dx:ASPxSpinEdit ID="seBaggage2" ClientInstanceName="seBaggage2" runat="server" Number="1" NumberType="Integer" MinValue="1" MaxValue="50" Width="50px">
                                                                                                </dx:ASPxSpinEdit>
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
                                                    <div id="collapse22" class="overflowCtr" style="visibility: hidden;">
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
                                                                                            SelectionMode="Single" TextFormatString="{0} {1}" OnInit="glMeals2_Init" OnAutoFilterCellEditorInitialize="glMeals2_AutoFilterCellEditorInitialize">
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
                                                                                            <ClientSideEvents RowClick="function(s, e) {setTimeout(function() { cmbDrinks2.SetSelectedIndex(0);cmbDrinks2.SetEnabled(false);seMeals2.Focus(); }, 0)}" />
                                                                                        </dx:ASPxGridLookup>
                                                                                    </div>
                                                                                </td>
                                                                                <td style="padding-top: 10px;" id="tdDrinks2" runat="server">
                                                                                    <div class="controlledCollapse collapse in">

                                                                                        <dx:ASPxComboBox ID="cmbDrinks2" ClientInstanceName="cmbDrinks2" runat="server" class="Select" AutoPostBack="false" IncrementalFilteringMode="StartsWith" Width="296px">
                                                                                        </dx:ASPxComboBox>

                                                                                    </div>
                                                                                </td>

                                                                                <td>
                                                                                    <table class="addBaggageInput">
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <input type="checkbox" runat="server" name="cbAllPaxMeal12" id="cbAllPaxMeal12" onchange="javascript:AllPaxMeal12();" value="1" />All Pax
                                                                                                </td>
                                                                                                <td style="padding-left: 10px;">
                                                                                                    <dx:ASPxSpinEdit ID="seMeals2" ClientInstanceName="seMeals2" runat="server" Number="1" NumberType="Integer" MinValue="1" MaxValue="50" Width="50px">
                                                                                                    </dx:ASPxSpinEdit>
                                                                                                </td>
                                                                                                <td style="padding-left: 10px;">
                                                                                                    <button type="button" class="btn btn-danger" onclick="AddMeal2()">Assign</button>
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

                                                        <div class="baggageAddonWrapper" style="padding-top: 10px;" id="divmeal2" runat="server">
                                                            <div class="mealAll">
                                                                <div class="showDropMeal collapse in" style="margin-bottom: 10px;">
                                                                    <table>
                                                                        <tbody>

                                                                            <tr>
                                                                                <td style="padding-top: 10px;">
                                                                                    <div class="controlledCollapse collapse in">

                                                                                        <dx:ASPxGridLookup ID="glMeals22" runat="server" KeyFieldName="SSRCode" Width="300px" EnableRowsCache="false" ClientInstanceName="glMeals22"
                                                                                            SelectionMode="Single" TextFormatString="{0} {1}" OnInit="glMeals22_Init" OnAutoFilterCellEditorInitialize="glMeals22_AutoFilterCellEditorInitialize">
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
                                                                                            <ClientSideEvents RowClick="function(s, e) {setTimeout(function() { cmbDrinks22.SetSelectedIndex(0);cmbDrinks22.SetEnabled(false);seMeals22.Focus(); }, 0)}" />
                                                                                        </dx:ASPxGridLookup>
                                                                                    </div>
                                                                                </td>

                                                                                <td style="padding-top: 10px;" id="tdDrinks22" runat="server">
                                                                                    <div class="controlledCollapse collapse in">

                                                                                        <dx:ASPxComboBox ID="cmbDrinks22" ClientInstanceName="cmbDrinks22" runat="server" class="Select" AutoPostBack="false" IncrementalFilteringMode="StartsWith" Width="296px">
                                                                                        </dx:ASPxComboBox>

                                                                                    </div>
                                                                                </td>
                                                                                <td>
                                                                                    <table class="addBaggageInput" style="margin-top: 10px;">
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <input type="checkbox" runat="server" name="cbAllPaxMeal22" id="cbAllPaxMeal22" onchange="javascript:AllPaxMeal22();" value="1" />All Pax
                                                                                                </td>
                                                                                                <td style="padding-left: 10px;">
                                                                                                    <dx:ASPxSpinEdit ID="seMeals22" ClientInstanceName="seMeals22" runat="server" Number="1" NumberType="Integer" MinValue="1" MaxValue="50" Width="50px">
                                                                                                    </dx:ASPxSpinEdit>
                                                                                                </td>
                                                                                                <td style="padding-left: 10px;">
                                                                                                    <button type="button" class="btn btn-danger" onclick="AddMeal21()">Assign</button>
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
                                                    <div id="collapse32" style="visibility: hidden;">
                                                        <div class="baggageAddonWrapper" style="padding-top: 10px;">

                                                            <div class="mealAll">

                                                                <div class="showDropMeal collapse in" style="margin-bottom: 10px;">
                                                                    <table>
                                                                        <tbody>

                                                                            <tr>
                                                                                <td style="padding-top: 10px;">
                                                                                    <div class="controlledCollapse collapse in">
                                                                                        <dx:ASPxComboBox ID="cmbSport2" ClientInstanceName="cmbSport2" runat="server" class="Select" AutoPostBack="false" IncrementalFilteringMode="StartsWith" Width="296px">
                                                                                            <ClientSideEvents CloseUp="function(s, e) {setTimeout(function() { seSport2.Focus(); }, 0)}" />
                                                                                        </dx:ASPxComboBox>
                                                                                    </div>

                                                                                </td>
                                                                                <td>
                                                                                    <table class="addBaggageInput" style="margin-top: 10px;">
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <input type="checkbox" runat="server" name="cbAllPaxSport2" id="cbAllPaxSport2" onchange="javascript:AllPaxSport2();" value="1" />All Pax
                                                                                                </td>
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
                                                    <div id="collapse42" style="visibility: hidden;">
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
                                                                                    <ClientSideEvents RowClick="function(s, e) {setTimeout(function() { seComfort2.Focus(); }, 0)}" />
                                                                                </dx:ASPxGridLookup>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <table class="addBaggageInput" style="margin-top: 10px;">
                                                                                <tr>
                                                                                    <td>
                                                                                        <input type="checkbox" runat="server" name="cbAllPaxComfort2" id="cbAllPaxComfort2" onchange="javascript:AllPaxComfort2();" value="1" />All Pax
                                                                                    </td>
                                                                                    <td style="padding-left: 10px;">
                                                                                        <dx:ASPxSpinEdit ID="seComfort2" ClientInstanceName="seComfort2" runat="server" Number="1" NumberType="Integer" MinValue="1" MaxValue="50" Width="50px">
                                                                                        </dx:ASPxSpinEdit>
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
                                                    <div id="collapse52" style="visibility: hidden;">
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
                                                                                    <ClientSideEvents CloseUp="function(s, e) {setTimeout(function() { seDuty2.Focus(); }, 0)}" />
                                                                                </dx:ASPxGridLookup>
                                                                            </div>
                                                                        </td>

                                                                        <td>
                                                                            <table class="addBaggageInput" style="margin-top: 10px;">
                                                                                <tr>
                                                                                    <td>
                                                                                        <input type="checkbox" runat="server" name="cbAllPaxDuty2" id="cbAllPaxDuty2" onchange="javascript:AllPaxDuty2();" value="1" />All Pax
                                                                                    </td>
                                                                                    <td style="padding-left: 10px;">
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
                                                    <div id="collapse62" style="visibility: hidden;">
                                                        <div class="baggageAddonWrapper">

                                                            <div style="padding-top: 10px;">
                                                                <table class="width100">

                                                                    <tr>
                                                                        <td style="padding-top: 10px;">
                                                                            <div class="controlledCollapse collapse in informationBlock">
                                                                                <h4>Please add Infant through the selected passenger Infant column</h4>
                                                                            </div>
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

                                    <dx:ASPxGridView ID="gvPassenger2" runat="server" KeyFieldName="SeqNo;PassengerID" EnableRowsCache="false" EnableCallBacks="true"
                                        ClientInstanceName="gvPassenger2" Width="100%" OnCustomCallback="gvPassenger2_CustomCallback" OnDataBinding="gvPassenger2_DataBinding"
                                        OnBatchUpdate="gvPassenger2_BatchUpdate" OnHtmlDataCellPrepared="gvPassenger2_HtmlDataCellPrepared" >
                                        <ClientSideEvents EndCallback="function(s, e) { gvPassenger2_EndCallback(s, e);}" BatchEditEndEditing="onBatchEditEndEditinggvPassenger2" CustomButtonClick="OnCustomButtonClick2" />
                                        <Columns>
                                            <dx:GridViewDataColumn FieldName="PNRs" Caption="PNR" VisibleIndex="0" GroupIndex="0" />
                                            <dx:GridViewDataColumn FieldName="SeqNo" Caption="SeqNo" ReadOnly="true" Width="70px" />

                                            <dx:GridViewDataTextColumn FieldName="FirstName" Caption="First Name" ReadOnly="true" Width="70px">
                                                <PropertiesTextEdit>

                                                    <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="LastName" Caption="Last Name" ReadOnly="true" Width="70px" />
                                            <dx:GridViewDataTextColumn FieldName="PaxType" Caption="Pax Type" ReadOnly="true" Width="70px" />
                                            <dx:GridViewDataComboBoxColumn FieldName="Baggage" Caption="Baggage" Width="120px">
                                                <PropertiesComboBox TextField="Detail" ValueField="SSRCode" ValueType="System.String">
                                                </PropertiesComboBox>
                                            </dx:GridViewDataComboBoxColumn>
                                            <dx:GridViewDataComboBoxColumn FieldName="Meal" Caption="Meal" Width="120px">
                                                <PropertiesComboBox TextField="Detail" ValueField="SSRCode"
                                                    ValueType="System.String">
                                                </PropertiesComboBox>
                                                <EditItemTemplate>
                                                    <dx:ASPxGridLookup ID="glMealP21" runat="server" KeyFieldName="SSRCode" Width="100%" EnableRowsCache="false" ClientInstanceName="glMealP21"
                                                        SelectionMode="Single" TextFormatString="{0} {1}" OnInit="glMealP21_Init">
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
                                                        <ClientSideEvents RowClick="function(s, e) {LoadingPanel.Show();}" EndCallback="function(s, e) {LoadingPanel.Hide();}" />
                                                    </dx:ASPxGridLookup>
                                                </EditItemTemplate>
                                            </dx:GridViewDataComboBoxColumn>
                                            <dx:GridViewDataComboBoxColumn FieldName="Drink" Caption="Drink" Width="150px">
                                                <PropertiesComboBox TextField="Detail" ValueField="SSRCode" ValueType="System.String">
                                                </PropertiesComboBox>
                                            </dx:GridViewDataComboBoxColumn>
                                            <dx:GridViewDataComboBoxColumn FieldName="Meal1" Caption="Meal 2" Width="120px">
                                                <PropertiesComboBox TextField="Detail" ValueField="SSRCode"
                                                    ValueType="System.String">
                                                </PropertiesComboBox>
                                                <EditItemTemplate>
                                                    <dx:ASPxGridLookup ID="glMealP22" runat="server" KeyFieldName="SSRCode" Width="100%" EnableRowsCache="false" ClientInstanceName="glMealP22"
                                                        SelectionMode="Single" TextFormatString="{0} {1}" OnInit="glMealP22_Init">
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
                                                        <ClientSideEvents RowClick="function(s, e) {LoadingPanel.Show();}" EndCallback="function(s, e) {LoadingPanel.Hide();}" />
                                                    </dx:ASPxGridLookup>
                                                </EditItemTemplate>
                                            </dx:GridViewDataComboBoxColumn>
                                            <dx:GridViewDataComboBoxColumn FieldName="Drink1" Caption="Drink 2" Width="150px">
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
                                                    <dx:ASPxGridLookup ID="glComfortP2" runat="server" KeyFieldName="SSRCode" Width="100%" EnableRowsCache="false" ClientInstanceName="glComfortP2"
                                                        SelectionMode="Single" TextFormatString="{0} {1}" OnInit="glComfortP2_Init">
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
                                                        <ClientSideEvents RowClick="function(s, e) {LoadingPanel.Show();}" EndCallback="function(s, e) {LoadingPanel.Hide();}" />
                                                    </dx:ASPxGridLookup>
                                                </EditItemTemplate>
                                            </dx:GridViewDataComboBoxColumn>
                                            <dx:GridViewDataComboBoxColumn FieldName="Infant" Caption="Infant" Width="120px" CellStyle-CssClass="infantCol">
                                                <PropertiesComboBox TextField="Detail" ValueField="SSRCode" ValueType="System.String">
                                                </PropertiesComboBox>
                                            </dx:GridViewDataComboBoxColumn>
                                            <dx:GridViewCommandColumn Name="Action" Caption="Action" Width="70px">
                                                <CustomButtons>
                                                    <dx:GridViewCommandColumnCustomButton ID="ClearLink1" runat="server" Text="Clear">
                                                    </dx:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                            </dx:GridViewCommandColumn>
                                        </Columns>
                                        <SettingsPager Mode="ShowPager" PageSize="11" Position="Top" />
                                        <SettingsEditing Mode="Batch" />
                                        <SettingsBehavior AutoExpandAllGroups="true" />
                                    </dx:ASPxGridView>
                                </div>
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
                </TabPages>
            </dx:ASPxPageControl>
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
    <asp:HiddenField ID="hfTransID" runat="server" Value="" />
    <asp:HiddenField ID="hfHashKey" runat="server" Value="" />
    <asp:HiddenField ID="hfInfant" runat="server"/>
    <asp:HiddenField ID="hfgvPassenger2" runat="server" Value="" />
    <asp:HiddenField ID="MaxPax" runat="server" Value="" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <fd:flightdetail ID="flightdetail" runat="server" />
</asp:Content>
