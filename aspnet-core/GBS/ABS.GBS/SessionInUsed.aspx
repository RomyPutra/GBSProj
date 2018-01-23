<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewBlankMaster.Master" AutoEventWireup="true" CodeBehind="SessionInUsed.aspx.cs" Inherits="ABS.GBS.SessionInUsed" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="overlapImage"></div>
        <table class="newBlankNotif">
            <tr>
                <td style="width:5px"></td>
                <td>
                    <div runat="server" id="diverrmsg1">
                    You are already logged in from another client or you did not logout properly from your previous session.
                    <br />Please login again from skyagent site.
                    <br />
                    Sorry for any inconveniences caused.
                    </div>
                </td>
            </tr>
            
            <tr>
                <td style="width:5px"></td>
                <td>
                <style type="text/css">
                   .divAdd
                   {
                       background-position: -289px -37px;
                       font: normal 12px Tahoma;
                       margin: 2px 0px;
                       border: 0px currentColor;
                       width: 110px;
                       height: 23px;
                       font-size: 12px;
                       font-weight: bold;
                       cursor: pointer;
                       background-image:url("images/AKBase/sprite3.png");
                       background-color: rgb(238, 238, 238);
                       text-align:center;
                    }
                    .divAdd a 
                    {
                        color: rgb(255, 255, 255);
                        text-decoration:none;
                        position:relative;
                        top:3px;
                    } 
                    .divAdd a:hover
                    {
	                    color: rgb(255, 255, 255);
	                    text-decoration:none;
	                    color: #000000;
                    }
                </style>
                <br /><div class="divAdd">
                <a href="javascript:void(0)" onclick="var win = window.open('about:blank','_self');win.close();" class="btnAdd">Close</a>
                </div>
                </td>    
            </tr>
            
        </table>
</asp:Content>
