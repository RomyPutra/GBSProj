<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DetailBreakDownAllPax.ascx.cs" Inherits="GroupBooking.Web.UserControl.DetailBreakDownAllPax" %>
<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %>


<script type="text/javascript" src="../Scripts/jquery-1.4.1.js"></script>

<script type="text/javascript">
    function clickDetail() {
        panelBreakdown.SetVisible(true);
        panelBreakdown.PerformCallback();
    }

    function closeBreakdown() {
        panelBreakdown.SetVisible(false);
    }

    function OnEndCallBackBreakdown(s, e) {

    }
</script>

<%--<msg:msgControl ID="msgcontrol" runat="server" />
<div>
    <dx:ASPxCallback ID="CallBackValidation" ClientInstanceName="CallBackValidation" runat="server" OnCallback="ValidatePopup">
        <ClientSideEvents CallbackComplete="OnCallbackComplete" />
    </dx:ASPxCallback>
</div>--%>



<div class="page-content row">

    <div class="col-md-12">
        <dx:ASPxCallbackPanel ID="panelBreakdown" runat="server" RenderMode="Div" ClientVisible="false" Height="100%" SettingsLoadingPanel-ShowImage="false" AutoPostBack="False"
            ClientInstanceName="panelBreakdown" OnCallback="panelBreakdown_Callback" >
            <SettingsLoadingPanel Enabled="false" />
            <ClientSideEvents EndCallback="function(s, e){ OnEndCallBackBreakdown(s, e); }"></ClientSideEvents>
            <PanelCollection>
                <dx:PanelContent>
                    <dx:ASPxButton CssClass="buttonL2 noBgImg" Visible="false" ID="btnCloseBreakdown" runat="server" Text="Close" AutoPostBack="false" ClientInstanceName="btnCloseBreakdown">
                        <ClientSideEvents Click="closeBreakdown()"  />
                    </dx:ASPxButton>
                        <table width="100%" bgcolor="#f7f3f7" class="table table-bordered" id="tblFareBreakdown" runat="server">
                            <tr id="trConnecting" runat="server">
                                <td id="tdConnectingDepart" runat="server">
                                    <div class="infoFlight">
                                        <span class="infoFlightSpan">
                                            <asp:Label ID="Label1" runat="server"></asp:Label>Connecting Charge <asp:Label ID="Label2" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="Label3" runat="server" Visible="false"></asp:Label></span>
                                        <span class="infoFlightSpan algnRight">
                                            <asp:Label ID="lblConnectingDepartTotal" runat="server" Text="0.00"></asp:Label>
                                            <asp:Label ID="lblCurrConnectingDepart" runat="server"></asp:Label></span>
                                    </div>
                                </td>
                                <td id="tdConnectingReturn" runat="server">
                                    <div class="infoFlight">
                                        <span class="infoFlightSpan">
                                            <asp:Label ID="Label6" runat="server">Connecting Charge</asp:Label><asp:Label ID="Label7" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="Label8" runat="server" Visible="false"></asp:Label></span>
                                        <span class="infoFlightSpan algnRight">
                                            <asp:Label ID="lblConnectingReturnTotal" runat="server" Text="0.00"></asp:Label>
                                            <asp:Label ID="lblCurrConnectingReturn" runat="server"></asp:Label></span>
                                    </div>
                                </td>
                            </tr>
                            <tr id="trKlia2" runat="server">
                                <td id="tdDepartKlia2" runat="server">
                                    <div class="infoFlight">
                                        <span class="infoFlightSpan">
                                            <asp:Label ID="lbl_num2" runat="server">Klia2 Fee</asp:Label><asp:Label ID="lbl_taxPrice" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lbl_currency3" runat="server" Visible="false"></asp:Label></span>
                                        <span class="infoFlightSpan algnRight">
                                            <asp:Label ID="lbl_klia2Total" runat="server" Text="0.00"></asp:Label>
                                            <asp:Label ID="lbl_currency2" runat="server"></asp:Label></span>
                                    </div>
                                </td>
                                <td id="tdReturnKlia2" runat="server">
                                    <div class="infoFlight">
                                        <span class="infoFlightSpan">
                                            <asp:Label ID="lbl_InNum2" runat="server">Klia2 Fee </asp:Label><asp:Label ID="lbl_IntaxPrice" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lbl_InCurrency3" runat="server" Visible="false"></asp:Label></span>
                                        <span class="infoFlightSpan algnRight">
                                            <asp:Label ID="lbl_Inklia2Total" runat="server" Text="0.00"></asp:Label>
                                            <asp:Label ID="lbl_Incurrency2" runat="server"></asp:Label></span>
                                    </div>
                                </td>
                            </tr>
                            <tr id="trGST" runat="server">
                                <td id="tdDepartGST" runat="server">
                                    <div class="infoFlight">
                                        <span class="infoFlightSpan">
                                            <asp:Label ID="lbl_num21" runat="server"></asp:Label>GST Charge<asp:Label ID="lbl_PaxFeePrice" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lbl_currency31" runat="server" Visible="false"></asp:Label></span>
                                        <span class="infoFlightSpan algnRight">
                                            <asp:Label ID="lbl_GSTTotal" runat="server" Text="0.00"></asp:Label>
                                            <asp:Label ID="lbl_currency21" runat="server"></asp:Label></span>
                                    </div>
                                </td>
                                <td id="tdReturnGST" runat="server">
                                    <div class="infoFlight">
                                        <span class="infoFlightSpan">
                                            <asp:Label ID="lbl_InNum21" runat="server">GST Charge</asp:Label><asp:Label ID="lbl_InPaxFeePrice" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lbl_InCurrency31" runat="server" Visible="false"></asp:Label></span>
                                        <span class="infoFlightSpan algnRight">
                                            <asp:Label ID="lbl_InGSTTotal" runat="server" Text="0.00"></asp:Label>
                                            <asp:Label ID="lbl_Incurrency21" runat="server"></asp:Label></span>
                                    </div>
                                </td>
                            </tr>
                            <tr id="trAVL" runat="server">
                                <td id="tdDepartAVL" runat="server">
                                    <div class="infoFlight">
                                        <span class="infoFlightSpan">
                                            <asp:Label ID="lbl_num4" runat="server" Visible="false"></asp:Label>AVL Charge
                                            <asp:Label ID="lblSvcChargeOneDepart" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lbl_currency5" runat="server" Visible="false"></asp:Label></span>
                                        <span class="infoFlightSpan algnRight">
                                            <asp:Label ID="lblAVLTotalDepart" runat="server" Text="0.00"></asp:Label>
                                            <asp:Label ID="lblCurrAVLDepart" runat="server"></asp:Label></span>
                                    </div>
                                </td>
                                <td id="tdReturnAVL" runat="server">
                                    <div class="infoFlight">
                                        <span class="infoFlightSpan">
                                            <asp:Label ID="lbl_InNum4" runat="server" Visible="false"></asp:Label>AVL Charge
                                            <asp:Label ID="lblSvcOneReturn" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lbl_InCurrency5" runat="server" Visible="false"></asp:Label></span>
                                        <span class="infoFlightSpan algnRight">
                                            <asp:Label ID="lblAVLTotalReturn" runat="server" Text="0.00"></asp:Label>
                                            <asp:Label ID="lblCurrAVLReturn" runat="server"></asp:Label></span>
                                    </div>
                                </td>
                            </tr>
                            <tr id="trPSF" runat="server">
                                <td id="tdDepartPSF" runat="server">
                                    <div class="infoFlight">
                                        <span class="infoFlightSpan">
                                            <asp:Label ID="lbl_num15" runat="server"></asp:Label>PSF Charge</span>
                                        <span class="infoFlightSpan algnRight">
                                            <asp:Label ID="lblPSFTotalDepart" runat="server" Text="0.00"></asp:Label>
                                            <asp:Label ID="lblCurrPSFDepart" runat="server"></asp:Label></span>
                                    </div>
                                </td>
                                <td id="tdReturnPSF" runat="server">
                                    <div class="infoFlight">
                                        <span class="infoFlightSpan">
                                            <asp:Label ID="lbl_InNum15" runat="server"></asp:Label>PSF Charge</span>
                                        <span class="infoFlightSpan algnRight">
                                            <asp:Label ID="lblPSFTotalReturn" runat="server" Text="0.00"></asp:Label>
                                            <asp:Label ID="lblCurrPSFReturn" runat="server"></asp:Label></span>
                                    </div>
                                </td>
                            </tr>
                            <tr id="trSCF" runat="server">
                                <td id="tdDepartSCF" runat="server">
                                    <div class="infoFlight">
                                        <span class="infoFlightSpan">
                                            <asp:Label ID="lbl_num16" runat="server"></asp:Label>SCF Charge</span>
                                        <span class="infoFlightSpan algnRight">
                                            <asp:Label ID="lblSCFTotalDepart" runat="server" Text="0.00"></asp:Label>
                                            <asp:Label ID="lblCurrSCFDepart" runat="server"></asp:Label></span>
                                    </div>
                                </td>
                                <td id="tdReturnSCF" runat="server">
                                    <div class="infoFlight">
                                        <span class="infoFlightSpan">
                                            <asp:Label ID="lbl_InNum16" runat="server"></asp:Label>SCF Charge</span>
                                        <span class="infoFlightSpan algnRight">
                                            <asp:Label ID="lblSCFTotalReturn" runat="server" Text="0.00"></asp:Label>
                                            <asp:Label ID="lblCurrSCFReturn" runat="server"></asp:Label></span>
                                    </div>
                                </td>
                            </tr>
                            <tr id="trDepartDiscount" runat="server">
                                <td id="tdDiscDepart" runat="server">
                                    <div class="infoFlight">
                                        <span class="infoFlightSpan">
                                            <asp:Label ID="Label4" runat="server"></asp:Label>Discount Charge</span>
                                        <span class="infoFlightSpan algnRight">
                                            <asp:Label ID="lblDiscTotalDepart" runat="server" Text="0.00"></asp:Label>
                                            <asp:Label ID="lblCurrDiscDepart" runat="server"></asp:Label></span>
                                    </div>
                                </td>
                                <td id="tdiscReturn" runat="server">
                                    <div class="infoFlight">
                                        <span class="infoFlightSpan">
                                            <asp:Label ID="Label10" runat="server"></asp:Label>Discount Charge</span>
                                        <span class="infoFlightSpan algnRight">
                                            <asp:Label ID="lblDiscTotalReturn" runat="server" Text="0.00"></asp:Label>
                                            <asp:Label ID="lblCurrDiscReturn" runat="server"></asp:Label></span>
                                    </div>
                                </td>
                            </tr>
                        </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxCallbackPanel>
    </div>
</div>
