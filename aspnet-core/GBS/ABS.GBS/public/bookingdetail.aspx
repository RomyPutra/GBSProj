<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewPageMaster.Master" AutoEventWireup="true" CodeBehind="bookingdetail.aspx.cs" Inherits="GroupBooking.Web.bookingdetail" %>

<%@ Register Src="../UserControl/bookingdetail.ascx" TagName="bookingdetail" TagPrefix="BKD" %>
<%@ Register Src="../UserControl/flightbookingdetail.ascx" TagName="ucflightbookingdetail" TagPrefix="FBD" %>
<%@ Register Src="../UserControl/PaxDetail.ascx" TagName="ucPaxDetail" TagPrefix="PD" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <bkd:bookingdetail ID="bkdetail" runat="server" />

    <script type="text/javascript">
        function gv_Callback(RecordLocator) {

            //var locations = location.href + "&recordlocator=" + RecordLocator;

            //document.getElementById('ctl00_ContentPlaceHolder2_hfpnr').value = RecordLocator;
            //alert(document.getElementById('ctl00_ContentPlaceHolder2_hfpnr').value);

            //window.alert(RecordLocator);
            //window.alert(cbFareBreakdown);

            if (typeof cbFareBreakdown != "undefined" && cbFareBreakdown != null){
                cbFareBreakdown.PerformCallback("Load|" + RecordLocator);
                return false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
<%--    <div class="formWrapperLeftAA" style="max-height:1200px">   
    <div class="sidebarTitle" style="padding: 15px 0;">
    <span class="dxeBase_GBS" id="RootHolder_SideHolder_lblHeader" style="font-size:Medium;font-weight:normal;">
        <div class="width100"> Flight Detail </div>
      
    </span>
    </div>
    <!-- Sidebar info for addOn -->
    <div style="" class="secondAlignRight">

            <table id="departureLeft" width="100%">
                <tbody>
                                <tr>
                                    <td style="padding-bottom: 3px;" valign="top">
                                        <div class="" style="text-align:left;"><span class="departFlight fa fa-plane"></span></div>
                                    </td>
                                    <td>
                                        <div class="" style="text-align:right;">
                                            <span class="biggerSize" id="Span7">
                                                <asp:Label ID="lblDateDepart" runat="server" ></asp:Label>
                                            </span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-bottom: 3px;" valign="top"> 
                                        <label font-size="Small">
                                            <b>
                                                <asp:Label ID="lblDepartOrigin" runat="server" ></asp:Label>
                                                
                                            </b>
                                        </label>
                                    </td>
                                    <td style="width: 332px; padding:0 0 10px 20px;" valign="top">
                                        <div id="RootHolder_SideHolder_Label17" style="float:right;">
                                            <div>                                                
                                                <asp:Label ID="lblDepartStd" runat="server" ></asp:Label> 
                                            </div> 
                                        </div>
                                    </td>
                                </tr>      
                                <tr>
                                    <td style="padding-bottom: 3px;" valign="top"> 
                                        <label id="RootHolder_SideHolder_ASPxLabel1" font-size="Small">
                                            <b>
                                                <asp:Label ID="lblDepartDestination" runat="server" ></asp:Label>
                                            </b>
                                        </label>
                                    </td>
                                    <td style="width: 332px; padding:0 0 10px 20px;">
                                         <div id="RootHolder_SideHolder_Div10" style="float:right;">
                                             <div>                                                 
                                                 <asp:Label ID="lblDepartSta" runat="server" ></asp:Label> 
                                             </div> 
                                        </div>
                                    </td>
                                </tr>

                                <tr>
                                    <td style="padding-bottom: 3px;" valign="top">
                                        <div class="" style="text-align:left;"><span class="departFlight fa fa-plane"></span></div>
                                    </td>
                                    <td>
                                        <div class="" style="text-align:right;">
                                            <span class="biggerSize" id="Span7">
                                                <asp:Label ID="lblDateReturn" runat="server"></asp:Label>
                                            </span>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-bottom: 3px;" valign="top"> 
                                        <label font-size="Small">
                                            <b>
                                                <asp:Label ID="lblReturnOrigin" runat="server" ></asp:Label>
                                            </b>
                                        </label>
                                    </td>
                                    <td style="width: 332px; padding:0 0 10px 20px;" valign="top">
                                        <div id="RootHolder_SideHolder_Label17" style="float:right;">
                                            <div>
                                                <asp:Label ID="lblReturnStd" runat="server" ></asp:Label>
                                            </div> 
                                        </div>
                                    </td>
                                </tr>      
                                <tr>
                                    <td style="padding-bottom: 3px;" valign="top"> 
                                        <label id="RootHolder_SideHolder_ASPxLabel1" font-size="Small">
                                            <b>
                                                <asp:Label ID="lblReturnDestination" runat="server" ></asp:Label>
                                            </b>
                                        </label>
                                    </td>
                                    <td style="width: 332px; padding:0 0 10px 20px;">
                                         <div id="RootHolder_SideHolder_Div10" style="float:right;">
                                             <div>
                                                 <asp:Label ID="lblReturnSta" runat="server" ></asp:Label>
                                             </div> 
                                        </div>
                                    </td>
                                </tr>
                </tbody>
            </table>
            <div style="height:30px;"></div>               
    </div>
    <!--End of Info-->
    </div>--%>

    <FBD:ucflightbookingdetail ID="ucflightbookingdetail" runat="server" />
    <PD:ucPaxDetail ID="ucPaxDetail" runat="server" />   

</asp:Content>