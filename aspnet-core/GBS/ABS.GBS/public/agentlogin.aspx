<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Newblankmaster.master" AutoEventWireup="true" 
    CodeBehind="agentlogin.aspx.cs" Inherits="GroupBooking.Web.Agentlogin" %>

<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        // -->
        // <![CDATA
        var test;
        function OnCallbackComplete(s, e) {
            if (e.result != "") {
                //lblmsg.SetValue(e.result);
                //                document.getElementById("MainContent_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = e.result;
                pcMessage.Show();
            } else {
                window.location.href = '../public/agentmain.aspx';
            }
        }

        function ShowLoginWindow() {
            pcMessage.Show();
        }
        // ]]> 
    </script>

<msg:msgControl ID="msgcontrol" runat="server" />
<div>
<table width="776" border="0" cellspacing="0" cellpadding="0" >
  <tr>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
  </tr>
</table>
    <dx:ASPxCallback ID="CallBackValidation" ClientInstanceName="CallBackValidation" runat="server" OnCallback="ValidateLogin">
        <ClientSideEvents CallbackComplete="OnCallbackComplete" />
    </dx:ASPxCallback>
</div>
<div class="WordSection1" >
    <table class="w100 newBlankNotif">
            <tr>
                <td style="width:5px"></td>
                <td>
                    <div runat="server" id="diverrmsg">
                   Your requested page is invalid or may be expired, <br />Please contact AirAsia for futher information.
                    <br />
                    Sorry for any inconveniences caused.
                    
                    </div>
                </td>
            </tr>
            
            <tr>
                <td style="width:5px"></td>
                <td>
                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="buttonL2ASP" Visible="false"
                        onclick="btnClose_Click"  />
                </td>    
            </tr>
            
        </table>
    
</div>
<div class="WordSection1" style="display:none">
  <div align="center">
    <table border=0 cellspacing=0 cellpadding=0 width=776>
      <%--<tr>
        <td style="width: 243px"><p><span style='"Times New Roman"'>
        <a href="../public/agentlogin.aspx"><span style='text-decoration:none;text-underline:none'>
        <img border="0" width="154" height="124" id="aalogo"
        src="../Images/AKBase/aalogo-cropped.gif"></span></a></span></p>
        </td>
        <td valign="bottom">&nbsp;</td>
      </tr>--%>
    </table>
  </div>

  <div >
    <table border=1 cellspacing=0 cellpadding=0 width=776>
      <tr>
        <td><div align=center>
            <table border=0 cellspacing=0 cellpadding=0 width=776>
              <tr>
                <td bgcolor="white"><div align=center>
                    <table border=0 cellspacing=0 cellpadding=0 width=680>
                      <tr>
                        <td align =left>
                            &nbsp;</td>
                      </tr>
                      <tr>
                        <td align =left>
                          <p><span style='font-size:10.5pt;font-family:"Arial","sans-serif"'>
                          <strong>Welcome to the AirAsia Group Booking System</strong></span></p>
                            <p><br></p>
                            <p><span style='font-size:10.5pt;font-family:"Arial","sans-serif"'>We are pleased to announce the new group booking experience for travel agency & general public
                            <span style='font-family:"Arial","sans-serif"'>Want to check make a group booking immediately? Just register yourself at <a
                            href="profile.aspx?optmode=1" target="_blank"><span style='color:red'>here</span></a>.</span><br/>
                          </td>
                      </tr>
                      <tr>
                        <td align =left>
                            &nbsp;</td>
                      </tr>
                      <tr>
                        <td><div align=center>
                            <table>
                                <tr>
                                    <td align="right">
                                        Login Name :
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAgent" runat="server" Width="150px" 
                                            AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        Password :
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="150px" Autocomplete="off"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        Agent Type :
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_AgentType" runat="server" Width="155px">
                                            <asp:ListItem Value="PublicAgent">Public Agent</asp:ListItem>
                                            <asp:ListItem Value="SkyAgent">Sky Agent</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="3">
                                        <asp:LinkButton ID="lb_forgetPwd" runat="server" onclick="lb_forgetPwd_Click">Forget password</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="3">
                                        <dx:ASPxButton CssClass="button_2"
                                            ID="btnLogin" runat="server" AutoPostBack="true" 
                                            Text="Login" >
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="3">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="3">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="3">
                                        &nbsp;</td>
                                </tr>
                            </table>
                            </div>
                          </td>
                      </tr>
                    </table>
                  </div>
                  </td>
              </tr>
            </table>
          </div></td>
      </tr>
    </table>
  </div>
</div>
</asp:Content>
