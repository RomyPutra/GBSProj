<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BookingBreakdown.ascx.cs" Inherits="ABS.GBS.UserControl.BookingBreakdown" %>

<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %>

<div id="flightdetailwrapper">
    <div id="divBB" class="blockAmountDue" runat="server" >
        <span>
            Amount Due<br />
            <asp:LinkButton ID="lnkAmountDue" runat="server" OnClientClick="clickDetail(); return false;" Visible="true"><asp:Label ID="lblAmountDue" runat="server"></asp:Label>&nbsp;<asp:Label ID="lblAmountDueCurr" runat="server"></asp:Label><br />
            </asp:LinkButton>
        </span>
    </div>
</div>

<aside class="">
    <div class="sidebar-category1">Available Credit [<dx:ASPxLabel runat="server" ID="lblAGCurr" ></dx:ASPxLabel>]</div>
    <div class="sidebar-widget">
        <ul class="list-unstyled pl-20 pr-20">
            <li class="mb-10">
                <div class="block clearfix mb-10">
                    <span class="pull-left fs-12 text-muted"><dx:ASPxLabel runat="server" ID="lblAGLimit" ></dx:ASPxLabel></span>
                </div>
            </li>
        </ul>
    </div>
</aside>