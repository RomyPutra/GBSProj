<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Newblankmaster.master" AutoEventWireup="true" 
CodeBehind="passwordretrieve.aspx.cs" Inherits="GroupBooking.Web.passwordretrieve" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table width="800" border="0" cellspacing="0" cellpadding="0">
  <%--<tr>
    <td>
        <a href="../public/agentlogin.aspx"><span style='text-decoration:none;text-underline:none'>
        <img border="0" width="154" height="124" id="aalogo"
        src="../Images/AKBase/aalogo-cropped.gif"></span></a></span></p>
    </td>
  </tr>--%>
  <tr>
    <td>
    <div>
          <div class="div">
            Forgot your password?</div>
        <hr />
        Please enter your email address and click "Submit". Your password will be sent to your email shortly.
        <br />
        <br />
        <table>
        <tr>
        <td>
        Email Address 
        </td>
        <td>
            <dx:ASPxTextBox ID="txtEmail" runat="server" Width="200px"  ><ValidationSettings>
            <RequiredField ErrorText="E-mail is required" IsRequired="True" />
            <RequiredField IsRequired="True" ErrorText="E-mail is required"></RequiredField>
            <RegularExpression ErrorText="Invalid e-mail" ValidationExpression="^(?=.{1,266}$)\w+([-.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></RegularExpression>
            </ValidationSettings>
            </dx:ASPxTextBox>
        </td>
        </tr>
        <tr>
        <td>
        <br />
        </td>
        </tr>
        <tr>
        <td colspan="2">
            <dx:ASPxLabel ID="lblErrorMsg" runat="server" Font-Bold="True" 
                ForeColor="#990000"></dx:ASPxLabel></td>
        </tr>
        <tr>
        <td>
            &nbsp;</td>
        </tr>
        <tr>
        <td colspan="2">
        <dx:ASPxButton  CssClass="button_2" ID="btnSubmit" runat="server" Text="Submit" 
                         Height="19px" Width="88px" onclick="btnSubmit_Click"  >
                <%--<ClientSideEvents Click="function(s, e) { 
                if(ASPxClientEdit.AreEditorsValid()) {
                                                                
                // Prevent multiple presses of the button
                    LoadingPanel.SetText(&#39;Sending data to the server...&#39;);
                    LoadingPanel.Show();
                }
                }"></ClientSideEvents>--%>
                        </dx:ASPxButton>
        </td>
        </tr>
        </table>
    </div>
    </td>
  </tr>
</table>
</asp:Content>
