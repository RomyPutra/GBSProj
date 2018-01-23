<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewPageMaster.Master" AutoEventWireup="true" CodeBehind="InsureNone.aspx.cs" Inherits="GroupBooking.Web.Insure" %>

<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v16.1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript" src="../Scripts/Booking/AddOn.js?ver=1.2"></script>

    <script type="text/javascript">
      
    </script>

<%--    <msg:msgControl ID="msgcontrol" runat="server" />--%>
    <div>
<%--        <dx:ASPxCallback ID="CallBackValidation" ClientInstanceName="CallBackValidation" runat="server" OnCallback="ValidatePopup">
            <ClientSideEvents CallbackComplete="OnCallbackCompleteManage" />
        </dx:ASPxCallback>--%>
    </div>
    <div class="row page-header clearfix">
        <div class="col-sm-3">
            <h4 class="mt-0 mb-5">Manage Insure</h4>
            Booking/Manage Insure
        </div>
        <div class="col-sm-5">
        </div>
        <div class="col-sm-4">
            <div style="float: right;">
                <dx:ASPxButton CssClass="buttonL" ID="btn_Nexts" runat="server"
                    Text="Goto Booking Detail" AutoPostBack="False">
<%--                    <ClientSideEvents Click="function(s, e) { if (gvPassenger.batchEditApi.HasChanges()) {
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
                                           LoadingPanel.Show(); }}" />--%>
                </dx:ASPxButton>
            </div>
        </div>
    </div>


    <div class="widget page-content container-fluid" style="border-color: transparent;">
            <h4 class="mt-0 mb-5">Insurance is not covered for the selected flight</h4> 
    </div>
<%--    <asp:HiddenField ID="hfgvPassenger" runat="server" Value="" />
    <asp:HiddenField ID="hfIndex" runat="server" Value="" />
    <asp:HiddenField ID="hfInternational" runat="server" Value="" />--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
<%--    <FBD:ucflightbookingdetail ID="ucflightbookingdetail" runat="server" />--%>
<%--    <PD:ucPaxDetail ID="ucPaxDetail" runat="server" />--%>
</asp:Content>
