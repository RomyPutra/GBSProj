<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewAdminMaster.Master" AutoEventWireup="true" CodeBehind="adminbookingdetail.aspx.cs" Inherits="GroupBooking.Web.admin.adminbookingdetail" %>

<%@ Register Src="../UserControl/bookingdetail.ascx" TagName="bookingdetail" TagPrefix="BKD" %>
<%@ Register Src="../UserControl/flightbookingdetail.ascx" TagName="ucflightbookingdetail" TagPrefix="FBD" %>
<%@ Register Src="../UserControl/PaxDetail.ascx" TagName="ucPaxDetail" TagPrefix="PD" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <bkd:bookingdetail ID="bkdetail" runat="server" />

    <script type="text/javascript">
        function gv_Callback(RecordLocator) {

            //var locations = location.href + "&recordlocator=" + RecordLocator;

            //document.getElementById('ctl00_ContentPlaceHolder2_hfpnr').value = RecordLocator;
            //alert(document.getElementById('ctl00_ContentPlaceHolder2_hfpnr').value);

            //window.alert(RecordLocator);
            //window.alert(cbFareBreakdown);

            if (typeof cbFareBreakdown != "undefined" && cbFareBreakdown != null) {
                cbFareBreakdown.PerformCallback("Load|" + RecordLocator);
                return false;
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <FBD:ucflightbookingdetail ID="ucflightbookingdetail" runat="server" />
    <PD:ucPaxDetail ID="ucPaxDetail" runat="server" />
</asp:Content>
