<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewPageMaster.Master" AutoEventWireup="true" CodeBehind="DivideValidation.aspx.cs" Inherits="GroupBooking.Web.Booking.DivideValidation" %>
<%@ Register Src="../UserControl/flightbookingdetail.ascx" TagName="ucflightbookingdetail" TagPrefix="FBD" %>
<%@ Register Src="../UserControl/PaxDetail.ascx" TagName="ucPaxDetail" TagPrefix="PD" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <div style="display:block;">
    <FBD:ucflightbookingdetail ID="ucflightbookingdetail" runat="server" />
    <PD:ucPaxDetail ID="ucPaxDetail" runat="server" />
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <dx:ASPxLabel runat="server" ID="lblErr" ForeColor="Red" ></dx:ASPxLabel><br /><br />

    Original PNR: <dx:ASPxTextBox runat="server" ID="txtOriPNR"></dx:ASPxTextBox> <br />
    New PNR: <dx:ASPxTextBox runat="server" ID="txtNewPNR" ></dx:ASPxTextBox> <br />
    <dx:ASPxButton runat="server" ID="btnValidate" OnClick="btnValidate_Click" Text="Validate"></dx:ASPxButton>

    <div class="itinerary-title" runat="server" id="divNewPNR" style="display:none">
        <div class="t2">
          <div class="c2">
            <p><span>Booking number :
                  </span><span class="red"><asp:Label  id="lblBookingNo" runat="server"></asp:Label></span></p>
            
          </div>
        </div>
    </div>
    <div class="itinerary-content" runat="server" id="divGuest">
        <div class="left-col-details">
            <div class="booking-details-table">
                <div class="RadGrid RadGrid_AirAsiaBooking2">
                    <table class="rgMasterTable" rules="all" style="width: 100%; table-layout: auto; empty-cells: show;" border="0">
                        <colgroup>
                        <col/>
                        </colgroup>
                        <thead>
                        <tr>
                            <th scope="col" class="rgHeader" style="width:100%">
                                <asp:Label runat="server" id="lblPassengerListDesc" Text="Selected&nbsp;Guest(s)&nbsp;to&nbsp;Divide" ></asp:Label>
                            </th>
                        </tr>
                        </thead>
                        <tbody>
                        <tr>
                            <td>
                                <dx:ASPxListBox ID="lbAvailable" runat="server" ClientInstanceName="lbAvailable"
                                Width="100%" Height="200px" >
                                </dx:ASPxListBox>
                            </td>
                        </tr>
                        </tbody>
                    </table>
                </div>
                
                <p style="color:Red; font-size:14px; font-weight:bold" runat="server" id="lblNote">* Once booking is split it cannot be undone. 
                    Click Confirm to continue.</p>
                </div>
        </div>
    </div>

    <dx:ASPxButton runat="server" ID="btnConfirm" Text="Confirm" Enabled="false" OnClick="btnConfirm_Click" ></dx:ASPxButton>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
