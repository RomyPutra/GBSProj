<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Master/NewPageMasterReport.Master" AutoEventWireup="true"
    CodeBehind="agentmain.aspx.cs" Inherits="GroupBooking.Web.Agentmain" %> 
<%@ MasterType  virtualPath="~/Master/PageMaster.Master"%>

<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %>

<%@ Register Src="../UserControl/agentmain.ascx" TagName="ucagentmain" TagPrefix="AGT" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolder2"> 
 
 <AGT:ucagentmain ID="ucagentmain" runat="server" />

</asp:Content>
<asp:Content ID="LeftContent" runat="server" ContentPlaceHolderID ="ContentPlaceHolder3" ></asp:Content>
