<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PaxDetail.ascx.cs" Inherits="ABS.GBS.UserControl.PaxDetail" %>

    <table class="table" style="margin-top: 40px;">
        <tr style="display:inline-block">
            <td>
                <div class="btnTransparent" style="padding: 3px 0;">
                    <div style="color: white; width: 66px;">Booking ID</div>
                    <div style="color: white; width: 66px;font-size: 12px !important;">
                        <%--<dx:ASPxLabel ID="lblBookingID" runat="server" Text="" ClientInstanceName="lblBookingID"/>--%>
                        <asp:LinkButton ID="lnkBookingID" runat="server" Text="" OnClick="lnkBookingID_OnClick"></asp:LinkButton>
                    </div>
                </div>
            </td>
        </tr>
        <tr style="display:inline-block">
            <td>
                <div class="btnTransparent" style="padding: 3px 0;">
                    <div style="color: white; width: 60px;">Total PAX</div>
                    <div style="color: white; width: 60px;font-size: 12px !important;text-align:center;">
                        <asp:LinkButton ID="lnkPNRAll" runat="server" Text="" OnClientClick="return gv_Callback('ALL');" OnClick="lnkPNR_Click"></asp:LinkButton>
                    </div>
                </div>
            </td>
        </tr>

        <asp:Repeater EnableViewState="true" ID="rptPNR" runat="server">
            <ItemTemplate>
                <tr>
                    <td>
                        <div class="btnTransparent" style="width: 100%; padding: 3px 0;">
                            <asp:LinkButton ID="lnkPNR" ClientIDMode="Static" runat ="server" OnClick="lnkPNR_Click"
                                            CommandName="click" CommandArgument='<%#Eval("RecordLocator") %>' Text='<%#Eval("PNR") %>' OnClientClick= '<%# String.Format("return gv_Callback(&#39;{0}&#39;);", Eval("RecordLocator")) %>'>
                            </asp:LinkButton>
                            <%--<dx:ASPxLabel ID="lnkPNR" runat="server" Text='<%#Eval("PNR") %>' ClientInstanceName="lnkPNR"/>--%>
                        </div>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <asp:HiddenField ID="hfSelectedPNR" Value="" runat="server"/>
        <tr>
            <td></td>
        </tr>
    </table>