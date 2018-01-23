<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Message.ascx.cs" Inherits="GroupBooking.Web.MessageControl" %>

    <dx:ASPxPopupControl ID="pcMessage" runat="server" CloseAction="CloseButton" Modal="True" 
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pcMessage"
    HeaderText="Group Booking - Message" AllowDragging="True" EnableAnimation="False" 
    EnableViewState="False" min-Height="150px" Width="350px">
    <%--<ClientSideEvents PopUp="function(s, e) { ASPxClientEdit.ClearGroup('entryGroup'); tbLogin.Focus(); }" />--%>
    <contentcollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <dx:ASPxPanel ID="pnlMessage" runat="server" DefaultButton="btOK">
                    <PanelCollection>
                        <dx:PanelContent runat="server">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td valign="top">
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <tr>
                                        <td rowspan="3">
                                            <div>
                                            </div>
                                        </td>
                                        <td valign="top" align="center">
                                            <dx:ASPxLabel ID="lblmsg" ClientInstanceName="lblmsg" runat="server" Font-Size="Larger" Width="320px" 
                                                Wrap="True">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td rowspan="3">
                                            <div >
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div>
                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td align="center">
                                                            <dx:ASPxButton ID="btCancel" runat="server" AutoPostBack="False" 
                                                                CssClass="button_2" Text="Ok" Width="80px" 
                                                                >
                                                                <%--<clientsideevents click="function(s, e) { pcMessage.Hide(); if (typeof($overlay_wrapper) != 'undefined') {hide_overlay();} }" >
                                                            </ClientSideEvents>--%>
                                                            <clientsideevents click="function(s, e) { pcMessage.Hide(); }" />
                                                            </dx:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </tr>
                            </table>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>
                
            </dx:PopupControlContentControl>
        </contentcollection>
    <contentstyle>
    <Paddings PaddingBottom="5px" />
    <Paddings PaddingBottom="5px"></Paddings>
        </contentstyle>
    </dx:ASPxPopupControl>
