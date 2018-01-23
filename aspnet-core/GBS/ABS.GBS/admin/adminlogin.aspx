<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Newblankmaster.master" AutoEventWireup="true" CodeBehind="adminlogin.aspx.cs" Inherits="GroupBooking.Web.Admin.adminlogin" %>

<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
    // <![CDATA[
        var test;

        function OnCallbackComplete(s, e) {
            if (e.result != "") {
                //lblmsg.SetValue(e.result);
                document.getElementById("MainContent_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = e.result;
                pcMessage.Show();
            } else {
                window.location.href = 'agentlist.aspx';
            }
        }

        function ShowLoginWindow() {
            pcMessage.Show();
        }
    // ]]> 
    </script>

<msg:msgControl ID="msgcontrol" runat="server" />
<div class="overlapImage login"></div>
<div>
    
<table width="776" border="0" cellspacing="0" cellpadding="0" style="display:none;">
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
<div id="loginPanel" class="WordSection1">
  <%--<div align=center>
    <table border=0 cellspacing=0 cellpadding=0 width=776>
      <tr>
        <td style="width: 243px"><p><span style='"Times New Roman"'>
        <a href="http://www.airasia.com"><span style='text-decoration:none;text-underline:none'>
        <img border="0" width="154" height="124" id="aalogo"
        src="../Images/AKBase/aalogo-cropped.gif"></span></a></span></p></td>
        <td valign=bottom>&nbsp;</td>
      </tr>
    </table>
  </div>
  </span>--%>
    <%
        if (Request.ServerVariables["HTTPS"] == "off" && HttpContext.Current.IsDebuggingEnabled == false)
        {
            Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"] + Request.ServerVariables["UNENCODED_URL"]);
        }
    %>
  <div >
    <table border=0 cellspacing=0 cellpadding=0 width="100%">
      <tr>
        <td><div align=center>
            <table id="loginBlock" border=0 cellspacing=0 cellpadding=0 width="100%">
              <tr>
                <td><div align=center>
                    <table border=0 cellspacing=0 cellpadding=0 width="100%">
                      <tr>
                        <td align =left>
                            &nbsp;</td>
                      </tr>
                      <tr>
                        <td align =left>
                          <p><span class="headerLogin" style='font-size:10.5pt;font-family:"Arial","sans-serif"'>
                          <strong>Welcome to the AirAsia Group Booking System Administrative Area</strong></span></p>
                            <p><br></p>
                            <p><span id="attnLogin" style='font-size:10.5pt;font-family:"Arial","sans-serif"'>This is a restricted area strictly for authorized user only, please consult your system administrator for details</span>
                          </td>
                      </tr>
                      <tr>
                        <td align="center"><div style="padding:10px;">
                            <table id="inputWrapper" class="tdClass">
                                <tr>
                                    <td align="right">
                                        Username :
                                                   <td>
                                                       &nbsp;</td>
                                    <td>
                                        <asp:TextBox ID="txtAgent" runat="server" Width="250px" AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        Password :
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                    <td>
                                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="250px" Autocomplete="off"></asp:TextBox>
                                    </td>
                                </tr>
                                    <td colspan="3">
                                        <div class="buttonLogin">
                                        <dx:ASPxButton CssClass="buttonL2"  
                                            ID="btnLogin" runat="server" AutoPostBack="true" 
                                            Text="  Login  "  >
<%--                                            <ClientSideEvents Click="function(s, e) {CallBackValidation.PerformCallback(); }" />
--%>                                        </dx:ASPxButton>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="3">
                                        <%--<asp:Button ID="btn_Logon" runat="server" Text ="  Login " 
                     onclick="btn_Logon_Click" Width="80px" />--%>
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
                  <div align=center></div></td>
              </tr>
            </table>
          </div></td>
      </tr>
    </table>
  </div>
</div>

</asp:Content>
