<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewPageMaster.Master" AutoEventWireup="true" CodeBehind="BookingComplete.aspx.cs" Inherits="GroupBooking.Web.BookingComplete" %>
<%@ Register Src="../UserControl/flightbookingdetail.ascx" TagName="ucflightbookingdetail" TagPrefix="FBD" %>
<%@ Register Src="../UserControl/PaxDetail.ascx" TagName="ucPaxDetail" TagPrefix="PD" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="clsCompleteBooking">
        <div>
            <p class="clsHeading">Your Booking had completed successfully.</p><p>Please click here</p><asp:LinkButton ID="lnkBookingID" CssClass="clsBookingID" runat="server" Text="" OnClick="lnkBookingID_OnClick" style=""></asp:LinkButton><p> to review the booking details.</p>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <FBD:ucflightbookingdetail ID="ucflightbookingdetail" runat="server" />
    <PD:ucPaxDetail ID="ucPaxDetail" runat="server" />   

</asp:Content>