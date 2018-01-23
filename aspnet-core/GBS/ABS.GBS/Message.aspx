<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Newblankmaster.master" AutoEventWireup="true" CodeBehind="Message.aspx.cs" Inherits="GroupBooking.Web.Message" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
 
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server"> 
    <script type="text/javascript">
    // <![CDATA[
    var demoCounter;
//    function demoGetTextElement(index) {
//        return document.getElementById("demoInfo" + index.toString());
//    }

    function demoInit() {
        demoCounter = 5;
        demoUpdate();
    }
    function demoTick() {
        demoCounter -= 1;
        demoUpdate();
    }
    function demoUpdate() {

        if (demoCounter > 0) {

            document.getElementById("demoCountdown").innerHTML = demoCounter;
            //var msgID = getQuerystring('msgID');
            var msgID = document.getElementById("ctl00_MainContent_hidMsgID").value;
            if (msgID == "100" || msgID == "105" || msgID == "106" || msgID == "108" || msgID == "104" || msgID == "107" || msgID == "109" || msgID == "400" || msgID == "5000") {
                var a = document.getElementById("lnkRedirect");
                a.href = "admin/adminmain.aspx";
                //a.href = "public/agentmain.aspx";
            }
            else if (msgID == "110") {
                var a = document.getElementById("lnkRedirect");
                a.href = "admin/adminmain.aspx";
            }
            else if (msgID == "104" || msgID == "107" || msgID == "109" || msgID == "400") {
                var a = document.getElementById("lnkRedirect");
                a.href = "invalid.aspx";
            }

        }
        else {
            tmrCount.SetEnabled(false);

            var msgID = getQuerystring('msgID');
            //var msgID = document.getElementById("MainContent_hidMsgID").value;
            if (msgID == "100" || msgID == "105" || msgID == "106" || msgID == "108" || msgID == "104" || msgID == "107" || msgID == "109" || msgID == "400" || msgID == "5000") {
                window.location = "admin/adminmain.aspx";
                //window.location = "public/agentmain.aspx";
            }
            else if (msgID == "110") {
                window.location = "admin/adminmain.aspx";
            }
            else if (msgID == "104" || msgID == "107" || msgID == "109" || msgID == "400") {
                window.location = "invalid.aspx";
                //window.location = "public/agentlogin.aspx";
            }
        }
    }

    function getQuerystring(key, default_) {
        if (default_ == null) default_ = "";
        key = key.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regex = new RegExp("[\\?&]" + key + "=([^&#]*)");
        var qs = regex.exec(window.location.href);
        if (qs == null)
            return default_;
        else
            return qs[1];
    }
    // ]]>
    </script>
     <dx:ASPxTimer ID="tmrCount" runat="server" Interval="1000" ClientInstanceName="tmrCount">
                    <ClientSideEvents Init="function(s, e) { demoInit(); }"
                        Tick="function(s, e) { demoTick(); }" />
                </dx:ASPxTimer>
    <input id="hidMsgID" name="hidMsgID" type="hidden" runat="server" />
  <table width="800" border="0" cellspacing="0" cellpadding="0">
    <tr>
      <td>&nbsp;</td>
        
    </tr>
    
    <tr>
      <td><table width="800" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td>&nbsp;</td>
          </tr>
          <%--<tr>
            <td>
                <table cellpadding="0" cellspacing="0" class="table">
                    <tr>
                        <td style="width: 64px" class="wSml3t">
                            &nbsp;</td>
                        <td>
                            <a href="http://www.airasia.com"><span style="text-decoration:none;text-underline:none"><img src="Images/aalogo-cropped.gif" alt="" width="154" height="124" border="0" id="_x0000_i1025" /></span></a>
                        </td>
                    </tr>
                </table>
            </td>
          </tr>--%>
          <tr>
            <td><table width="800px">
                <tr>
                  <td valign="top"><table border="1" cellspacing="0" cellpadding="0" width="776">
                      <tr>
                        <td><table border="0" cellspacing="0" cellpadding="0" width="776">
                            <tr>
                              <td bgcolor="white" align="center"><table border="0" cellspacing="0" cellpadding="0" width="680">
                                  <tr>
                                    <td>&nbsp;</td>
                                  </tr>
                                  <tr>
                                    <td>&nbsp;</td>
                                  </tr>
                                  <tr>
                                    <td>&nbsp;</td>
                                  </tr>
                                  <tr>
                                    <td align="left"><span style='font-size:10.5pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'> <span style='font-size: 10.5pt; font-family:"Arial", "sans-serif";  text-align: left;'>
                                        <dx:ASPxLabel ID="lblTitle" runat="server" Font-Bold="True" Font-Size="Large" 
                                ForeColor="#CC3300"></dx:ASPxLabel>
                                        </span>
                                        </span></td>
                                  </tr>
                                  <tr>
                                    <td align="left">&nbsp;</td>
                                  </tr>
                                  <tr>
                                    <td align="left">&nbsp;</td>
                                  </tr>
                                  <tr>
                                    <td align="left">&nbsp;</td>
                                  </tr>
                                  <tr>
                                    <td align="left"><p>
                                        <span style='font-size:10.5pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'> 
                                        <span style='font-size: 10.5pt; font-family:"Arial", "sans-serif";  text-align: left;'>
                                        <dx:ASPxLabel ID="lblMessage" runat="server" Font-Bold="True" Font-Size="Medium"> </dx:ASPxLabel>
                                        </span><br />
                                        You will be redirect to the&nbsp; main page with <span id="demoCountdown" style="font-weight: bold; color: #59A1E9;" >5</span> seconds, or directly click <a ID="lnkRedirect" style="color:Red;text-decoration:none;" >here</a> to redirect<br />
                                        </span></p></td>
                                  </tr>
                                  <tr>
                                    <td>&nbsp;</td>
                                  </tr>
                                  <tr>
                                    <td>&nbsp;</td>
                                  </tr>
                                  <tr>
                                    <td>&nbsp;</td>
                                  </tr>
                                  <tr>
                                    <td>&nbsp;</td>
                                  </tr>
                                </table></td>
                            </tr>
                          </table></td>
                      </tr>
                    </table></td>
                </tr>
              </table></td>
          </tr>
          <tr>
            <td>&nbsp;</td>
          </tr>
        </table></td>
    </tr>
  </table>
  
</asp:Content>
