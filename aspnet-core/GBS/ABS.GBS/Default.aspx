<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="GroupBooking.Web._default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
    <script type="text/javascript">
        //alert('ndasmu');
        window.resizeTo(screen.availWidth, screen.availHeight);
        window.focus();
    </script>
<body>
    <form id="form1" runat="server">
    <div>
    <%

        if (Request.ServerVariables["HTTPS"] == "off" && HttpContext.Current.IsDebuggingEnabled == false)
        {
            Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"] + Request.ServerVariables["UNENCODED_URL"]);
        }
        //else if (Request.ServerVariables["HTTPS"] == "on" && HttpContext.Current.IsDebuggingEnabled == false)
        //{
        //    Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"] + Request.ServerVariables["UNENCODED_URL"]);
        //}
    %>
    </div>
    </form>
</body>
</html>
