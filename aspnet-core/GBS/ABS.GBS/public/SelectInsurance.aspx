<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewPageMaster.Master" AutoEventWireup="true" CodeBehind="SelectInsurance.aspx.cs" Inherits="GroupBooking.Web.SelectInsurance" %>

<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %>
<%@ Register Src="../UserControl/flightbookingdetail.ascx" TagName="ucflightbookingdetail" TagPrefix="FBD" %>
<%@ Register Src="../UserControl/PaxDetail.ascx" TagName="ucPaxDetail" TagPrefix="PD" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript">
        function Submit() {
            if (ASPxClientEdit.ValidateGroup('mandatory')) {
                if (document.getElementById('ctl00_ContentPlaceHolder2_hfConfirm').value == "1") {
                    popupConfirm.Show();
                    document.getElementById('ctl00_ContentPlaceHolder2_hfConfirm').value = "0";
                }
                else {
                    assignSeatCallBack.PerformCallback();
                    LoadingPanel.Show();
                }
            }
            else {
                window.scrollTo(0, 1020);
                LoadingPanel.Hide();
            }
        }

        function ClosePopup() {
            popupConfirm.Hide();
        }

        function OnCallbackComplete(s, e) {
            //alert("test");
            var str = e.result;
            if (str != null && str != "") {
                document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = str;
                pcMessage.Show();
                LoadingPanel.Hide();
            } else {
                return;
                //window.location.href = '../public/reviewbooking.aspx';
            }
            return;
        }
    </script>
    <msg:msgControl ID="msgcontrol" runat="server" />
    <div class="row page-header clearfix">
        <div class="col-sm-4">
            <h4 class="mt-0 mb-5"><span runat="server" id="spanTitle">Insurance</span>
                <%--<dx:ASPxLabel runat="server" ID="lblPassengerDetails" Text="Passenger Upload"></dx:ASPxLabel>--%>
            </h4>
            Booking/<dx:ASPxLabel runat="server" ID="lblPassengerDetailsSub" Text="Insurance"></dx:ASPxLabel>
        </div>
        <div class="col-sm-8">
            <div align="right" style="padding-top: 9px;">
                <table id="bookingDetail">
                    <tr>
                        <td>
                            <dx:ASPxButton CssClass="buttonL2 noBgImg" ID="btConfirm" runat="server" ClientInstanceName="btConfirm" Text="Confirm" AutoPostBack="False">
                                <%--20170324 - Sienny (full page loading => LoadingPanel.Show(); added --%>
                                <ClientSideEvents Click="function(s, e) { Submit();  }" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </div>
        </div>

    </div>

    <div class="row page-header clearfix" style="padding-top: 20px; display: none;" id="mainContainer" runat="server">
    </div>
    <div class="widget page-content container-fluid">
        <div class="row">
            <div class="col-sm-6">
                <div class="div">
                    <h4>Passenger List</h4>
                </div>

            </div>


        </div>

        <dx:ASPxGridView ID="gvPassenger" ClientInstanceName="gvPassenger" runat="server" KeyFieldName="PassengerID" Width="100%" AutoGenerateColumns="False" SettingsBehavior-ColumnResizeMode="Control">
            <Columns>
                <dx:GridViewDataColumn FieldName="PNR" VisibleIndex="0" GroupIndex="0" />
                <%--<dx:GridViewDataColumn FieldName="PNR" Caption="PNR" Width="90px" />--%>
                <dx:GridViewDataColumn FieldName="IssuingCountryName" Caption="Issuing Country" VisibleIndex="0" Width="110px" />
                <dx:GridViewDataColumn FieldName="countryName" Caption="Nationality" VisibleIndex="1" Width="90px" />
                <dx:GridViewDataColumn FieldName="Title" Caption="Title" VisibleIndex="4" Width="35px" />
                <dx:GridViewDataColumn FieldName="Gender" Caption="Gender" VisibleIndex="5" Width="60px" />
                <dx:GridViewDataColumn FieldName="FirstName" Caption="First Name" VisibleIndex="6" Width="90px" />
                <dx:GridViewDataColumn FieldName="LastName" Caption="Last Name" VisibleIndex="7" Width="90px" />
                <dx:GridViewDataColumn FieldName="Amount" Caption="Amount" VisibleIndex="8" Width="90px" />
                <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="9">
                </dx:GridViewCommandColumn>

            </Columns>
            <SettingsPager PageSize="50" Mode="ShowAllRecords"></SettingsPager>
            <SettingsDetail ExportMode="All" />
            <Settings HorizontalScrollBarMode="Auto" ShowGroupFooter="VisibleIfExpanded" />
            <GroupSummary>
                <dx:ASPxSummaryItem FieldName="LineTotal" SummaryType="Count" DisplayFormat="{0}" ShowInGroupFooterColumn="PNR" />
            </GroupSummary>
            <SettingsBehavior ColumnResizeMode="Control"></SettingsBehavior>
            <Settings ShowGroupFooter="VisibleIfExpanded"></Settings>
            <SettingsDetail ExportMode="All"></SettingsDetail>
            <Styles>
                <Header BackColor="#333333" ForeColor="White"></Header>
            </Styles>
        </dx:ASPxGridView>
    </div>

    <br />

    <div style="padding: 24px;">

        <p id="agreementInputCheckbox" class="formCheckbox" style="background-color: #FFFFCC; border-bottom-width: 1;">
            <br />
            Please confirm that you understand and accept our 
        <b><a href="http://www.airasia.com/my/en/about-us/terms-and-conditions.page" onclick="window.open(this.href, 'mywin','left=20,top=20,width=500,height=500,toolbar=1,resizable=0'); return false;">Terms and conditions of carriage</a></b>, 
        <b><a href="http://www.airasia.com/my/en/our-fares/fare-rules.page" onclick="window.open(this.href, 'mywin','left=20,top=20,width=500,height=500,toolbar=1,resizable=0'); return false;">Fare rules</a></b> and 
        <b><a href="http://www.airasia.com/my/en/about-us/privacy-policy.page" onclick="window.open(this.href, 'mywin','left=20,top=20,width=500,height=500,toolbar=1,resizable=0'); return false;">Privacy policy</a></b> to continue. 
        The booking cannot be cancelled and payments made are not refundable. 
        <br />
            DISCLAIMER: We may refuse carriage of you or your baggage if, in the exercise of our reasonable discretion, we determine either that the 
        payment of your fare is fraudulent or the booking of your seat has been done fraudulently or unlawfully or has been purchased from a person not authorized by us.

        <div>

            <dx:ASPxCheckBox runat="server" ID="chkTerm" CheckBoxStyle-Font-Bold="true" Text="I have read and agree the Terms and Conditions of Carriage and Fare rules.">
                <ValidationSettings ValidationGroup="mandatory" ErrorTextPosition="Top">
                    <RequiredField IsRequired="true" ErrorText="Please indicate that you have read and agree to the Terms and Conditions, Fare Rules and Privacy Policy" />
                </ValidationSettings>
            </dx:ASPxCheckBox>

        </div>
            <br />
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
            <p>
            </p>
        </p>
    </div>
    <br />
    <div class="col-sm-12">
        <div align="right" style="padding-top: 9px;">
            <table id="bookingDetails">
                <tr>
                    <td>
                        <dx:ASPxButton CssClass="buttonL2 noBgImg" ID="ASPxButton1" runat="server" ClientInstanceName="btConfirm" Text="Confirm" AutoPostBack="False">
                            <%--20170324 - Sienny (full page loading => LoadingPanel.Show(); added --%>
                            <ClientSideEvents Click="function(s, e) { Submit();  }" />
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <dx:ASPxCallback runat="server" ID="assignSeatCallBack" ClientInstanceName="assignSeatCallBack" OnCallback="assignSeatCallBack_Callback">
        <ClientSideEvents CallbackComplete="OnCallbackComplete" />
    </dx:ASPxCallback>
    <asp:HiddenField ID="hfConfirm" runat="server" />
    <input type="hidden" runat="server" id="hResult" name="hResult" />
    <dx:ASPxPopupControl ID="popupConfirm" runat="server" ClientInstanceName="popupConfirm"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        CloseButtonImage-Url="~/Images/AirAsia/close_button_icon.jpg"
        Modal="true" AllowDragging="true"
        HeaderText="Forfeited Amount Confirmation" CloseAction="CloseButton"
        Width="250px">
        <CloseButtonImage Url="~/Images/AirAsia/close_button_icon.jpg"></CloseButtonImage>
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <table width="100%">
                    <tr>
                        <td colspan="2">
                            <br />
                            <!-- The cancellation process cannot be undone, please confirm the action -->
                            <b>Your Amount will be forfeited.</b> Are you sure want to confirm ?
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="btnYes" runat="server" Text="Yes" Width="50px" AutoPostBack="False" ClientInstanceName="btnYes">
                                            <ClientSideEvents Click="function(s, e) {ClosePopup();assignSeatCallBack.PerformCallback();
                                LoadingPanel.SetText(&#39;Please Wait...&#39;);
                                LoadingPanel.Show(); }"></ClientSideEvents>

                                        </dx:ASPxButton>
                                    </td>
                                    <td>&nbsp; </td>
                                    <td>
                                        <dx:ASPxButton ID="btnNo" runat="server" Text="No" Width="50px" AutoPostBack="False">
                                            <ClientSideEvents Click="ClosePopup" />
                                            <ClientSideEvents Click="ClosePopup"></ClientSideEvents>
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <FBD:ucflightbookingdetail ID="ucflightbookingdetail" runat="server" />
    <PD:ucPaxDetail ID="ucPaxDetail" runat="server" />
</asp:Content>
