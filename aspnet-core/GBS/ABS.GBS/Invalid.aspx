<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Newblankmaster.master" AutoEventWireup="true" CodeBehind="Invalid.aspx.cs" Inherits="GroupBooking.Web.Invalid" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="overlapImage"></div>
        <table class="newBlankNotif">
            <tr>
                <td style="width:5px"></td>
                <td>
                    <div runat="server" id="diverrmsg">
                    Your requested page is invalid or may be expired, <br />Please contact AirAsia for further information.
                    <br />
                    Sorry for any inconveniences caused.
            
                    </div>
                </td>
            </tr>
            
            <tr>
                <td style="width:5px"></td>
                <td>
                <dx:ASPxButton CssClass="buttonL2" ID="btnClose" runat="server" Height="19px" Visible="false"
                            OnClick="btnClose_Click" Text="Close" Width="88px"></dx:ASPxButton>
                </td>    
            </tr>
            
        </table>
        <script type="text/javascript">
     
        window.resizeTo(screen.availWidth, screen.availHeight);
        window.focus();
    </script>
</asp:Content>

