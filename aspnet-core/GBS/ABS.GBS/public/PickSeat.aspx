<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PickSeat.aspx.cs" Inherits="ABS.GBS.PickSeat" %>
<%@ Register TagPrefix="seat" TagName="select" Src="~/seatcontrol.ascx" %> 
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="position:relative"></div>
    <div style="display:block">
    <asp:Button ID="b1" runat="server" Text="Initial Select 1st Passenger" OnClientClick="gotoindex(1)" />
    <asp:Button ID="b2" runat="server" Text="Initial Select 2nd Passenger" OnClientClick="gotoindex(2)" />
    <input type="button" id="APassengerNumber_0_Reselect" value="Reselect 1st Passenger" />
    <input type="button" id="APassengerNumber_1_Reselect" value="Reselect 2nd Passenger" />
    <input type="button" id="APassengerNumber_0_Remove" value="Remove 1st Passenger" />
    <input type="button" id="APassengerNumber_1_Remove" value="Remove 2nd Passenger" />
    <br />
    Show 1st Seat : <asp:TextBox runat="server" ID="BPassengerNumber_0" ClientIDMode="Static"></asp:TextBox>
    <br />
    Show 2nd Seat : <asp:TextBox runat="server" ID="BPassengerNumber_1" ClientIDMode="Static"></asp:TextBox>
    <br />

    <asp:Button ID="done" runat="server" Text="Finish" />
    <br />Selected Seat(s) : <asp:TextBox runat="server" ID="SelectedSeat"></asp:TextBox></div>

    <div id="PassengerSummary" runat="server" class="table-striped inputToFocus">
    </div>

    <div id="connectedflightcontrol" runat ="server" style="position:absolute ">
    
    <input type="button" runat="server" class ="ConnectedButton"  id="btnSeatDepart1" onserverclick="btnSeatDepart1_ServerClick" /> <br />
    <input type="button" runat="server" class ="ConnectedButton" id="btnSeatDepart2" onserverclick="btnSeatDepart2_ServerClick" /> <br />
    <input type="button" runat="server" class ="ConnectedButton"  id="btnSeatReturn1" /><br />
    <input type="button" runat="server" class ="ConnectedButton"  id="btnSeatReturn2" /><br />
    </div>  
    <seat:select runat="server" ID="ss" />
    </form>

    <script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../Scripts/overlay.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        Array.prototype.inArray = function (value) {
            // Returns true if the passed value is found in the
            // array. Returns false if it is not.
            var i;
            for (i = 0; i < this.length; i++) {
                if (this[i] == value) {
                    return true;
                }
            }
            return false;
        };

        function calseat(xx) {
            alert("Seat Select");
            var arr = new Array("1A", "2A", "3A", "4A", "5A");
            if (arr.inArray(xx.value)) {
                alert("Your value is found in the Array");
                
            }
            else {
                alert("Your value is not found in the Array");
                xx.value = "";
                xx.focus();
            }
        }   
    </script>
    <dx:ASPxImage runat="server" ID="imgStatus" ClientInstanceName="imgStatus" Width="0" Height="0" ></dx:ASPxImage>
    <dx:ASPxImage runat="server" ID="imgMessage" ClientInstanceName="imgMessage" Width="0" Height="0" ></dx:ASPxImage>

    <asp:panel Visible = "false"  ID ="pnlErr" runat="server" Height ="20px"  BackImageUrl ="../images/red_header_bg.gif">
        <asp:Label runat="server" ForeColor="White" ID="lblErr" ></asp:Label>
    </asp:panel>
</body>
</html>
