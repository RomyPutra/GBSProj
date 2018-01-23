<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewPageMaster.Master" AutoEventWireup="true" CodeBehind="ProceedPaymentChange.aspx.cs" Inherits="GroupBooking.Web.ProceedPaymentChange" %>

<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../Scripts/overlay.js" type="text/javascript"></script>
    <script type="text/javascript" >
        
        function setContactDetails() {
            //rdlContactPNR.Items.Add("Address: " + Session["Address_" + PNR] + ";Town: " + Session["Town_" + PNR] + ";Country: " + Session["Country_" + PNR] + ";State: " + Session["State_" + PNR] + ";ZipCode:" + Session["ZipCode_" + PNR]);

            var ContactPNRText = rdlContactPNR.GetSelectedItem().text;

            var indexTitle = ContactPNRText.indexOf("Title: ");
            var lengthTitle = 7;
            var indexFirstName = ContactPNRText.indexOf(";FirstName: ");
            var lengthFirstNameTitle = 12;
            var indexLastName = ContactPNRText.indexOf(";LastName: ");
            var lengthLastNameTitle = 11;
            var indexEmail = ContactPNRText.indexOf(";Email: ");
            var lengthEmailTitle = 8;
            var indexPhoneNo = ContactPNRText.indexOf(";PhoneNo: ");
            var lengthPhoneNoTitle = 10;

            var indexAddress = ContactPNRText.indexOf(";Address: ");
            var lengthAddressTitle = 10;
            var indexTown = ContactPNRText.indexOf(";Town: ");
            var lengthTownTitle = 7;
            var indexCountry = ContactPNRText.indexOf(";Country: ");
            var lengthCountryTitle = 10;
            var indexState = ContactPNRText.indexOf(";State: ");
            var lengthStateTitle = 8;
            var indexZipCode = ContactPNRText.indexOf(";ZipCode: ");
            var lengthZipCodeTitle = 10;

            var lengthAll = ContactPNRText.Length;

            var title = ContactPNRText.substring(indexTitle + lengthTitle, indexFirstName);
            var firstname = ContactPNRText.substring(indexFirstName + lengthFirstNameTitle, indexLastName);
            var lastname = ContactPNRText.substring(indexLastName + lengthLastNameTitle, indexEmail);
            var email = ContactPNRText.substring(indexEmail + lengthEmailTitle, indexPhoneNo);
            var phoneno = ContactPNRText.substring(indexPhoneNo + lengthPhoneNoTitle, indexAddress);
            var address = ContactPNRText.substring(indexAddress + lengthAddressTitle, indexTown);
            var town = ContactPNRText.substring(indexTown + lengthTownTitle, indexCountry);
            var country = ContactPNRText.substring(indexCountry + lengthCountryTitle, indexState);
            var state = ContactPNRText.substring(indexState + lengthStateTitle, indexZipCode);
            var zipcode = ContactPNRText.substring(indexZipCode + lengthZipCodeTitle, lengthAll);

            cmbContactTitle.SetValue(title);
            txtContactFirstName.SetText(firstname);
            txtContactLastName.SetText(lastname);
            txtContactEmail.SetText(email);
            txtContactPhone.SetText(phoneno);
            txtContactAddress.SetText(address);
            txtContactTown.SetText(town);

            if (country != "") {
                cmbContactCountryAddress.SetValue(country);
                txtContactCountryAddress.SetText(country);
                showCityItems(cmbContactCountryAddress.GetSelectedIndex());
            }
            if (state != "") {
                cmbContactState.SetValue(state);
                txtContactState.SetText(state);
            }
            txtContactZipCode.SetText(zipcode);

            //__doPostBack("<%=cmbContactCountryAddress.ClientID %>", '');
        }
        function loadCity() {
            showCityItems(cmbContactCountryAddress.GetSelectedIndex());
        }
        function showCityItems(index) {
//            alert(index);
//            alert(city_value[index].length);
            var cnt = city_value[index].length;
            cmbContactState.ClearItems();
            for(var x=0;x<cnt;x++) {
//                alert(city_value[index][x] + " " + city_value[index][x]);
                cmbContactState.AddItem(city_text[index][x], city_value[index][x]);
            }

        }
        function setValueToTextState() {
            txtContactState.SetText(cmbContactState.GetSelectedItem().value);
        }
        function setMinimumPayment() {
            //alert(rdlPNR.GetValue() + " " + rdlPNR.GetSelectedIndex() + " " + rdlPNR.GetSelectedItem().text + " " + rdlPNR.GetItem(rdlPNR.GetSelectedIndex()).value);

            rdlContactPNR.SetSelectedIndex(rdlPNR.GetSelectedIndex());

            var tabMode = "";
            if (document.getElementById('<%=divCreditCard.ClientID%>') != null)
                tabMode = "tabCredit";
            else if (document.getElementById('<%=divAG.ClientID%>') != null)
                tabMode = "tabAG";

            var minAmount = "0.00";
            var amountDue = "0.00";

            var PNRText = rdlPNR.GetSelectedItem().text;
            var indexMinPay = PNRText.indexOf("Min. Payment:");
            if (indexMinPay >= 0) {
                var indexAmount = indexMinPay + 13;
                var indexCurrencyBracket = PNRText.indexOf("(", indexAmount);
                minAmount = PNRText.substring(indexAmount, indexCurrencyBracket);
                txtPayAmount.SetText(minAmount);
                txtMinimumPayment.SetText(minAmount);
                txtMinPay.SetText(minAmount);
                lblMinPay.SetText(minAmount);
                //document.getElementById('<%=lblMinPay.ClientID%>').innerHTML = minAmount;
            }
            else {
                txtPayAmount.SetText("");
                txtMinimumPayment.SetText("");
                txtMinPay.SetText("");
                lblMinPay.SetText("");
            }

            var indexAmountDue = PNRText.indexOf("Amount Due:");
            if (indexAmountDue >= 0) {
                var indexAmount = indexAmountDue + 11;
                var indexCurrencyBracket = PNRText.indexOf("(", indexAmount);
                amountDue = PNRText.substring(indexAmount, indexCurrencyBracket);
                txtDueAmount.SetText(amountDue);
            }
            if (tabMode == "tabAG") {
                txtPayAmount.SetText(amountDue);
                txtMinPay.SetText(amountDue);
                lblMinPay.SetText(amountDue);
            }

            document.getElementById("divMsg").style.visibility = "hidden";

            setContactDetails();
        }

        function OnCallbackCompleteHide(s, e) {
            LoadingPanel.Hide();
        }

        function validate(evt) {
            var theEvent = evt || window.event;
            var key = theEvent.keyCode || theEvent.which;
            key = String.fromCharCode(key);
            var regex = /[0-9]|\./;
            if (!regex.test(key)) {
                theEvent.returnValue = false;
                if (theEvent.preventDefault) theEvent.preventDefault();

            }
        }

        function hide() {
            LoadingPanel.Hide();
        }

        function submitform() {
            DirectDebitForm.submit();
        }

        function SubmitPayment() {
            $("#ctl00_ContentPlaceHolder2_btnProceedPayment").click(function () {
                if (TabControl != undefined) {
                    var tab = TabControl.GetActiveTab();
                    //alert(tab.name);
                    switch (tab.name) {
                        case "TabCredit":
                            if (ASPxClientEdit.ValidateGroup('mandatory') && ASPxClientEdit.ValidateGroup('cc')) {
                                //show_overlay();
                                $("#ctl00_ContentPlaceHolder2_hError").val("0");
                                return;
                            } else {
                                $("#ctl00_ContentPlaceHolder2_hError").val("1");
                                //alert($("#ContentPlaceHolder2_hError").val());
                            }
                            break;
                        case "TabAG":
                            if (ASPxClientEdit.ValidateGroup('mandatory')) {
                                //show_overlay();
                                $("#ctl00_ContentPlaceHolder2_hError").val("0");
                                return;
                            } else {
                                $("#ctl00_ContentPlaceHolder2_hError").val("1");
                                //alert($("#ContentPlaceHolder2_hError").val());
                            }
                            break;
                    }
                    //hide_overlay();
                }
            });
        }

////        function SubmitPayment() {
////            $("#ContentPlaceHolder2_btnProceedPayment").click(function () {
////                var tab = TabControl.GetActiveTab();
////                switch (tab.name) {
////                    case "TabCredit":
////                        if (ASPxClientEdit.ValidateGroup('mandatory') && ASPxClientEdit.ValidateGroup('cc')) {
////                            show_overlay();
////                            $("#ContentPlaceHolder2_hError").val("0");
////                            return;
////                        } else {
////                            $("#ContentPlaceHolder2_hError").val("1");
////                            //alert($("#ContentPlaceHolder2_hError").val());
////                        }
////                        break;
////                    case "TabAG":
////                        if (ASPxClientEdit.ValidateGroup('mandatory')) {
////                            show_overlay();
////                            $("#ContentPlaceHolder2_hError").val("0");
////                            return;
////                        } else {
////                            $("#ContentPlaceHolder2_hError").val("1");
////                            //alert($("#ContentPlaceHolder2_hError").val());
////                        }
////                        break;
////                }
////                //hide_overlay();
////            });
////        }

        function ReplaceNumberWithCommas(yourNumber) {
            //Seperates the components of the number
            if (yourNumber <= 0 || isNaN(yourNumber)) {
                return "";
            }
            
            var n = yourNumber.toString().split(".");
            //Comma-fies the first part
            n[0] = n[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            //Combines the two sections
            return n.join(".");
        }
        // added by diana 20130904
//        function changeTextBoxProperty() {
//            if (cmbCardType.GetSelectedItem().value == "AX") {
//                txtCVV2.GetInputElement().maxLength = 4;
//                txtCVV2.SetText("");
//            }
//            else {
//                txtCVV2.GetInputElement().maxLength=3;
//                txtCVV2.SetText("");
//            }
//        }

</script>

<style type="text/css">
        .overlay2
        {
            position: fixed;
            z-index: 98;
            top: 0px;
            left: 0px;
            right: 0px;
            bottom: 0px;
            background-color: #aaa;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }
        .overlayContent2
        {
          z-index: 99;
          margin: 250px auto;
          position: fixed;
          top: 50%;
          left: 50%;
          margin-top: -50px;
          margin-left: -100px;
        }
        
        
    .style1
    {
        width: 82px;
    }
        
        
    </style>

<dx:ASPxValidationSummary ID="vsValidationSummary1" runat="server" Width="502px" ClientInstanceName="validationSummary" HorizontalAlign="Left" Height="16px">
    <ErrorStyle Wrap="False" />
    <Border BorderColor="Red" BorderStyle="Double" />
    <Border BorderColor="Red" BorderStyle="None"></Border>
</dx:ASPxValidationSummary>
    
<asp:ScriptManager ID="ScriptManager" runat="server" AsyncPostBackTimeout="36000"/>
<input type="hidden" id="hError" runat="server" value="" />
<input type="hidden" id="hID" runat="server" value="" />
<input type="hidden" id="hCarrierCode" runat="server" value="" />
<asp:UpdatePanel ID="UpdatePanel" runat="server" >
    <contenttemplate>
        <msg:msgControl ID="msgcontrol" runat="server" />
        <div ><asp:Label runat="server" ID="lblErrorTop" ForeColor="Red" Visible="false"></asp:Label></div>

        <div id="mainContentHeaderDiv">
            <div id="pageTitle">
            <h1>Review and pay</h1>
            <p>You've almost completed your booking. We recommend 
            that you double check your flight number, date, time, destination and 
            total amount due before you select the mode of payment that best suits 
            you.</p>
            </div>
        </div>

        <div id="selectMainBodyTop" class="mainBody">
        <div class="clearAll"></div>
        </div>
        <div class="newSectionHeader">
            <div>Summary</div>
        </div>
        <div id="paymentSummaryBlock" class="mainPriceDisplayContainer">
        <table width="100%">
        <tr  align="left" style="background-color:#f7f3f7;font-size:12px;font-family:Arial,Helvetica,san-serif;">
        <td>
        <table class="priceDisplay" border="0" cellpadding="0" cellspacing="0" width="100%">
        <tbody>
        <tr>
        <td><strong>Transaction ID</strong></td>
        <td class="amountdesc"></td>
        <td class="priceamountmain">        
            <asp:Label ID="lblTransactionID" runat="server" Text="0"></asp:Label>
        </td>
        </tr><tr>
        <td><strong>Booking PNR</strong></td>
        <td class="amountdesc"></td>
        <td class="priceamountmain">        
            <asp:Label ID="lblRecordLocator" runat="server" Text=""></asp:Label>
        </td>
        </tr>
        </tbody></table>
        <br />
        <table class="priceDisplay">
        <tbody><tr id="trDepart" runat="server">
            <td>
                <strong>Depart Total</strong></td>
            <td class="amountdesc">
            </td>
            <td class="priceamountmain">
                <asp:Label ID="lblDepartTotal" runat="server" Text="0"></asp:Label>
                &nbsp;
                <asp:Label ID="lblDepartTotalCurrency" runat="server" Text="MYR"></asp:Label>
            </td>
            </tr>
            <tr id="trReturn" runat="server">
                <td>
                    <strong>Return Total</strong></td>
                <td class="amountdesc">
                </td>
                <td class="priceamountmain">
                    <asp:Label ID="lblReturnTotal" runat="server" Text="0"></asp:Label>
                    &nbsp;
                    <asp:Label ID="lblReturnTotalCurrency" runat="server" Text="MYR"></asp:Label>
                </td>
            </tr>
        </tbody></table>
        <table class="priceDisplay">
        <tbody><tr>
        <td><strong>Current Total</strong></td>
        <td></td>
        <td class="priceamountmain">
            <asp:Label ID="lblCurrentTotal" runat="server" Text="0" ForeColor="Red"></asp:Label>&nbsp;
            <asp:Label ID="lblCurrentTotalCurrency" runat="server" Text="MYR" ForeColor="Red"></asp:Label>
        </td>
        </tr>
        </tbody></table>
        <br />
        <table class="priceDisplay" border="0" cellpadding="0" cellspacing="0" width="100%">
        <tbody>
        <tr>
        <td><strong>Total Deposit</strong></td>
        <td class="amountdesc"></td>
        <td class="priceamountmain">        
            <asp:Label ID="lblAmountPaid" runat="server" Text="0"></asp:Label>&nbsp;
            <asp:Label ID="lblAmountPaidCurrency" runat="server" Text="MYR"></asp:Label>
        </td>
        </tr>

        <tr>
        <td><strong>Amount Due</strong></td>
        <td class="amountdesc"></td>
        <td class="priceamountmain">        
            <asp:Label ID="lblAmountDue" runat="server" Text="0" ForeColor="Red"></asp:Label>&nbsp;
            <asp:Label ID="lblAmountDueCurrency" runat="server" Text="MYR" ForeColor="Red"></asp:Label>
        </td>
        </tr>
        
        </tbody>
        </table>
        </td>
        </tr>
        </table>
        <br />
        <!-- added by diana 20130902 -->
        <style type="text/css">
        .tdClass
        {
	        font-family:Arial,Helvetica,san-serif;
	        font-size:12px;
        }
        </style>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" id="tblPayment" runat="server" style="visibility:hidden">
        <tbody>
        <tr>
            <td colspan="3"><strong>Select Booking PNR to Pay</strong><br />
            <div id="divPNR" runat="server" width="100%" style="margin-right:0px;padding-right:0px;">
                <dx:ASPxRadioButtonList ID="rdlPNR" ClientInstanceName="rdlPNR" runat="server" RepeatDirection="Vertical" 
                    Border-BorderStyle="None" RepeatColumns="1" RepeatLayout="Table" Paddings-Padding="0" 
                    CssClass="tdClass" Width="740px">
                    <ClientSideEvents SelectedIndexChanged="setMinimumPayment" />
                    <ValidationSettings ValidationGroup="mandatory">
                        <RequiredField ErrorText="Please tick this" IsRequired="true" />
                    </ValidationSettings>
                </dx:ASPxRadioButtonList>
            </div>
            
            <div id="divContactPNR" runat="server" width="100%" style="margin-right:0px;padding-right:0px;">
                <dx:ASPxRadioButtonList ID="rdlContactPNR" ClientVisible="false" ClientInstanceName="rdlContactPNR" runat="server" RepeatDirection="Vertical" 
                    Border-BorderStyle="None" RepeatColumns="1" RepeatLayout="Table" Paddings-Padding="0" 
                    CssClass="tdClass" Width="740px">
                    <%--<ClientSideEvents SelectedIndexChanged="setContactDetails" />--%>
                </dx:ASPxRadioButtonList>
            </div>

            </td>
        </tr>

        <tr>
        <td align="left" style="padding:0px 2px; width:250px" valign="middle">
         &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;<strong><asp:Label ID="lblMinCap" runat="server" Text="Min. Payment: "></asp:Label></strong><dx:ASPxLabel ID="lblMinPay" ClientInstanceName="lblMinPay" ClientEnabled="true" runat="server" Text="0.00" ForeColor="Red"></dx:ASPxLabel>&nbsp;<asp:Label ID="lblMinPayCurrency" runat="server" Text="MYR" ForeColor="Red"></asp:Label></td>

        <td style="padding:0px 2px;width:100px;" align='right' valign="middle"> <strong>Amount To Pay</strong></td>
        <td style="padding:0px 2px;" align="left" valign="middle">
        <dx:ASPxTextBox ID="txtMinimumPayment" ClientEnabled="true" ClientInstanceName="txtMinimumPayment" runat="server" ClientVisible="false" Text="0.00"></dx:ASPxTextBox>
        <dx:ASPxTextBox ID="txtMinPay" ClientEnabled="true" ClientInstanceName="txtMinPay" runat="server" ClientVisible="false" Text="0.00"></dx:ASPxTextBox>
        <dx:ASPxTextBox ID="txtDueAmount" ClientEnabled="true" ClientInstanceName="txtDueAmount" runat="server" ClientVisible="false" Text="0.00"></dx:ASPxTextBox>
        <dx:ASPxTextBox ID="txtPayAmount" ClientEnabled="true" 
                           ClientInstanceName="txtPayAmount" runat="server" Width="120px" MaxLength="30">
                         <%--<ClientSideEvents LostFocus="function(s, e) {s.SetText(ReplaceNumberWithCommas(parseFloat(s.GetText()).toFixed([2])));}" />--%>
                         <%-- code above is not needed to change comma to dot --%>
                        <ValidationSettings ValidationGroup="mandatory">
                        <RequiredField ErrorText=" " IsRequired="True" />
                        <%--<RegularExpression ErrorText="Invalid payment amount" ValidationExpression="^[1-9]\d*(,\d+)?$" />--%>
                        <RegularExpression ErrorText="Invalid payment amount" ValidationExpression="^([\d.,*]{1,20})$"></RegularExpression>
                       
                       <%--<RegularExpression ErrorText="Invalid payment amount" ValidationExpression="^(?:\d{1,14}|\d{1,11}\.\d\d)$"></RegularExpression>--%>
                        
                <RequiredField IsRequired="True" ErrorText="Payment amount is required"></RequiredField>
                   </ValidationSettings>
                </dx:ASPxTextBox></td>
        
        </tr>
        

        </tbody></table>
        <br />
    </div>

    <!-- Payment Methods -->
    <div id="selectMainBody" class="mainBody">    
    <div style="width:100%" id="divMsg"><table><tr><td valign="top">&nbsp;
        <asp:Label runat="server" ID="imgError" ForeColor="Red" Visible="false"><img src='../images/AKBase/icon-error.gif' width='15px'/></asp:Label>
        &nbsp;</td><td>
        <asp:Label runat="server" ID="lblErrorBottom" ForeColor="Red" Visible="true" Width="720px"></asp:Label>
        </td></tr></table>
    </div>
    <div style="float: left; width: 100%; margin-right: 2%">

    <div runat="server" id="divContact" visible="true" style="background-color:#E3E3E3">
        
        <div class="paymentTabNote">
        <h3><font color="red" size="+1">Contact Details</font></h3>
        <p class="paymentTabNote" style="font-family:Arial,Helvetica,san-serif;font-size:11px;padding:0px;margin:10px 0px;">
        Please fill up your contact details.
        </p>
        <table width="100%" class="tblData">
            <tr>
                <td style="width:300px;">
                    *Title
                </td>
                <td style="width: 300px">
                    <dx:ASPxComboBox runat="server" ID="cmbContactTitle" IncrementalFilteringMode="StartsWith"
                         ClientEnabled="true" ClientInstanceName="cmbContactTitle"
                        AllowMouseWheel="False" FilterMinLength="1" SelectedIndex="0">
                        <Items>
                            <dx:ListEditItem Text="MR" Value="MR" Selected="True" />
                            <dx:ListEditItem Text="MS" Value="MS" />
                        </Items>
                    </dx:ASPxComboBox>
                </td>
                <td>     
                    &nbsp;
                </td> 
            </tr>
            <tr>
                <td style="width:300px;">
                    *First Name
                </td>
                <td style="width: 300px">
                    <dx:ASPxTextBox ID="txtContactFirstName" ClientInstanceName="txtContactFirstName" ClientEnabled="true" runat="server" Width="200px" MaxLength="20" 
                        AutoCompleteType="Disabled">
                    <ValidationSettings ValidationGroup="mandatory">
                        <RequiredField IsRequired="True" ErrorText="First Name is required"></RequiredField>
                    </ValidationSettings>
                    </dx:ASPxTextBox>
                </td>
                <td>     
                    &nbsp;
                </td> 
            </tr>
            <tr>
                <td style="width:300px;">
                    *Last Name
                </td>
                <td style="width: 300px">
                    <dx:ASPxTextBox ID="txtContactLastName" ClientInstanceName="txtContactLastName" ClientEnabled="true" runat="server" Width="200px" MaxLength="20" 
                        AutoCompleteType="Disabled">
                    <ValidationSettings ValidationGroup="mandatory">
                        <RequiredField IsRequired="True" ErrorText="Last Name is required"></RequiredField>
                    </ValidationSettings>
                    </dx:ASPxTextBox>
                </td>
                <td>     
                    &nbsp;
                </td> 
            </tr>
            <tr>
                <td style="width:300px;">
                    *Email
                </td>
                <td style="width: 300px">
                    <dx:ASPxTextBox ID="txtContactEmail" ClientInstanceName="txtContactEmail" ClientEnabled="true" runat="server" Width="200px" MaxLength="50" 
                        AutoCompleteType="Disabled">
                    <ValidationSettings ValidationGroup="mandatory">
                        <RequiredField IsRequired="True" ErrorText="Email is required"></RequiredField>
                        <RegularExpression ErrorText="Email is invalid" ValidationExpression="^[\w-.]+(?:\+[\w]*)?@([\w-]+.)+[\w-]{2,4}$"></RegularExpression>
                    </ValidationSettings>
                    </dx:ASPxTextBox>
                </td>
                <td>     
                    &nbsp;
                </td> 
            </tr>
            <tr>
                <td style="width:300px;">
                    *Phone Number
                </td>
                <td style="width: 300px">
                    <dx:ASPxTextBox ID="txtContactPhone" ClientInstanceName="txtContactPhone" ClientEnabled="true" runat="server" Width="200px" MaxLength="20" 
                        AutoCompleteType="Disabled">
                    <ValidationSettings ValidationGroup="mandatory">
                        <RequiredField IsRequired="True" ErrorText="Phone Number is required"></RequiredField>
                    </ValidationSettings>
                    </dx:ASPxTextBox>
                </td>
                <td>     
                    &nbsp;
                </td> 
            </tr>
            <tr>
                <td style="width:300px;">
                    *Street address
                </td>
                <td style="width: 300px">
                    <dx:ASPxTextBox ID="txtContactAddress" ClientInstanceName="txtContactAddress" ClientEnabled="true" runat="server" Width="200px" MaxLength="20" 
                        AutoCompleteType="Disabled">
                    <ValidationSettings ValidationGroup="mandatory">
                        <RequiredField IsRequired="True" ErrorText="Address is required"></RequiredField>
                    </ValidationSettings>
                    </dx:ASPxTextBox>
                </td>
                <td>     
                    &nbsp;
                </td> 
            </tr>
            <tr>
                <td>
                    *Town/City
                </td>
                <td style="width: 300px">
                    <dx:ASPxTextBox ID="txtContactTown" ClientInstanceName="txtContactTown" ClientEnabled="true" runat="server" Width="200px" MaxLength="30" 
                        AutoCompleteType="Disabled">
                        <ValidationSettings ValidationGroup="mandatory">
                            <RequiredField IsRequired="True" ErrorText="Town / city name is required"></RequiredField>
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            
            <tr>
                <td style="width: 300px; height: 19px">
                    *Country
                </td>
                <td >
                <table>
                        <tr>
                            <td class="insideClass"><dx:ASPxComboBox ID="cmbContactCountryAddress" ClientInstanceName="cmbContactCountryAddress" ClientEnabled="true" runat="server" AllowMouseWheel="False" IncrementalFilteringMode="StartsWith"
                    FilterMinLength="1" AutoPostBack="true"
                        onselectedindexchanged="cmbContactCountryAddress_SelectedIndexChanged">
                        <%--<dx:ASPxComboBox ID="cmbContactCountryAddress" ClientInstanceName="cmbContactCountryAddress" ClientEnabled="true" runat="server" AllowMouseWheel="False" IncrementalFilteringMode="StartsWith"
                    FilterMinLength="1">--%>
                        <%--<ClientSideEvents SelectedIndexChanged="function(s, e) { showCityItems(s.GetSelectedIndex()); }" />--%>
                </dx:ASPxComboBox>
                                <dx:ASPxTextBox ID="txtContactCountryAddress" ClientVisible="false" ClientInstanceName="txtContactCountryAddress" ClientEnabled="true" runat="server"></dx:ASPxTextBox>
                </td>
                </tr>
                </table>           
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    *State and zip code
                </td>
                <td style="width: 300px">
                    <table>
                        <tr>
                            <td class="insideClass">
                                <dx:ASPxComboBox ID="cmbContactState" ClientInstanceName="cmbContactState" ClientEnabled="true" runat="server" AllowMouseWheel="False" IncrementalFilteringMode="StartsWith" Width="100px"
                                    FilterMinLength="1">
                                    <ClientSideEvents SelectedIndexChanged="setValueToTextState" />
                                </dx:ASPxComboBox>
                                <dx:ASPxTextBox ID="txtContactState" ClientInstanceName="txtContactState" ClientVisible="false" ClientEnabled="true" runat="server"></dx:ASPxTextBox>
                            </td>
                            <td>
                               <dx:ASPxTextBox ID="txtContactZipCode" ClientInstanceName="txtContactZipCode" ClientEnabled="true" runat="server" Width="50px" MaxLength="30">
                                    <ValidationSettings ValidationGroup="mandatory">
                                        <RequiredField IsRequired="True" ErrorText="Zip Code is required"></RequiredField>
                                    </ValidationSettings>
                                </dx:ASPxTextBox>                                                       
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
            </tr>
        </table>
        </div>   
        </div>
        <br />
    <dx:ASPxPageControl SkinID="None" Width="100%" EnableViewState="false" ID="TabControl" ClientInstanceName="TabControl"
        runat="server" ActiveTabIndex="0" AutoPostBack="True"
        TabSpacing="0px" ContentStyle-Border-BorderWidth="0"  onActiveTabChanged="TabControl_ActiveTabChanged"
        EnableHierarchyRecreation="True" >
 
        <TabPages>
    
            <dx:TabPage Text="Credit Card/Debit Card" Name="TabCredit" Visible="true">        
                <ContentCollection>
                    <dx:ContentControl ID="ContentControl1" runat="server">                  
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>

            <dx:TabPage Text="AG Payment" Name="TabAG" Visible="true">        
                <ContentCollection>
                    <dx:ContentControl ID="ContentControl2" runat="server">                  
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>

            <dx:TabPage Text="Direct Debit" Name="TabDirectDebit" Visible="false">        
                <ContentCollection>
                    <dx:ContentControl ID="ContentControl3" runat="server">                  
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>

        </TabPages>

        <ActiveTabStyle BackColor="#C00000" ForeColor="White" Font-Bold="true"></ActiveTabStyle>
        <tabstyle backcolor="White" font-bold="True"></tabstyle>
        <ContentStyle BackColor="#E3E3E3" >
            <Paddings Padding="0px" PaddingLeft="0px" PaddingTop="0px" PaddingRight="0px" PaddingBottom="0px"></Paddings>
            <Border BorderWidth="0px"></Border>
        </ContentStyle>
    </dx:ASPxPageControl>

    <style type="text/css">
    .tblData tr td
    {
        padding:0px 2px 2px 0px;
        font-family:Arial,Helvetica,san-serif;
        font-size:12px;
    }
    .insideClass tr td
    {
        padding:0px;
    }
    </style>
    <!-- End Payment Methods Tabs-->
    <!-- Payment Methods Div-->
    <div runat="server" id="divCreditCard" visible="true" style="background-color:#E3E3E3">
        <div class="paymentTabNote">
        <table width="100%" class="tblData">
            <tr>
                <td colspan="3" align="center" >
                    <asp:Panel ID="panelProcessFee" runat="server" visible="true">
                    <asp:Label runat="Server" ID="lblTextProcessing" Text="Total Processing Fee" Font-Underline="true"></asp:Label>&nbsp;:&nbsp;<asp:Label runat="server" ID="lblProcessFee" Text = "0"></asp:Label>&nbsp;
                    <asp:Label runat="server" ID="lblCurrencyProcessFee" Text = "MYR"></asp:Label></asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width:300px;">
                    *Card Type
                </td>
                <td style="width: 300px">
                <table>
                        <tr>
                        <!-- OnSelectedIndexChanged="cmbCardType_SelectedIndexChanged" -->
                            <td class="insideClass"><dx:ASPxComboBox runat="server" ID="cmbCardType" IncrementalFilteringMode="StartsWith"
                         ClientEnabled="true" ClientInstanceName="cmbCardType"
                        AllowMouseWheel="False" FilterMinLength="1" SelectedIndex="0">
                        <Items>
                            <%--<dx:ListEditItem Text="American Express" Value="AX" Selected="True" /> --%>
                            <dx:ListEditItem Text="Master Card" Value="MC" Selected="True" />
                            <dx:ListEditItem Text="Visa" Value="VI" />
                        </Items>
                        <%--<ClientSideEvents SelectedIndexChanged="function(s,e){changeTextBoxProperty()}" /> --%>
                    </dx:ASPxComboBox>
                    <%--<dx:ListEditItem Text="BIG Visa Card" Value="BG" />--%>
                        </td>
                    </tr>
                </table>
                </td> 
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    *Card Number
                </td>
                <td style="width: 300px"><dx:ASPxTextBox ID="txtCardNumber" runat="server" Width="200px" MaxLength="20" 
                        AutoCompleteType="Disabled">
                    <ValidationSettings ValidationGroup="cc">
                        <RequiredField ErrorText="Card number is required" IsRequired="True" />
                        <RegularExpression ErrorText="Invalid Card number" ValidationExpression="^([\d*]{1,20})$" >                        
                        </RegularExpression>
                        <RequiredField IsRequired="True" ErrorText="Card number is required"></RequiredField>
                    </ValidationSettings>
                    </dx:ASPxTextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    *Card Holder Name
                </td>
                <td style="width: 300px"><dx:ASPxTextBox ID="txtCardHolderName" runat="server" Width="200px" 
                        MaxLength="30" AutoCompleteType="Disabled">
                        <ValidationSettings ValidationGroup="cc">
                            <RegularExpression ErrorText="Invalid card holder name" ValidationExpression="^([1-zA-Z0-1@.\s]{1,200})$" >
                            </RegularExpression>
                            <RequiredField IsRequired="True" ErrorText="Card holder name is required"></RequiredField>
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    *Expiration Date
                </td>
                <td style="width: 300px">
                    <table>
                        <tr>
                            <td class="insideClass">
                                <dx:ASPxComboBox ID="cmbExpiryMonth" IncrementalFilteringMode="StartsWith"
                                    runat="server" Width="40" SelectedIndex = "0" AllowMouseWheel="False" FilterMinLength="1" CssClass="compClass">
                                    <Items>                            
                                        <dx:ListEditItem Text="1" Value="1" />
                                        <dx:ListEditItem Text="2" Value="2" />
                                        <dx:ListEditItem Text="3" Value="3" />
                                        <dx:ListEditItem Text="4" Value="4" />
                                        <dx:ListEditItem Text="5" Value="5" />
                                        <dx:ListEditItem Text="6" Value="6" />            
                                        <dx:ListEditItem Text="7" Value="7" />
                                        <dx:ListEditItem Text="8" Value="8" />
                                        <dx:ListEditItem Text="9" Value="9" />
                                        <dx:ListEditItem Text="10" Value="10" />
                                        <dx:ListEditItem Text="11" Value="11" />
                                        <dx:ListEditItem Text="12" Value="12" />            

                                    </Items>
                        
                                </dx:ASPxComboBox>
                            </td>
                            <td class="insideClass">
                                <dx:ASPxComboBox ID="cmbExpiryYear" width="60px" runat="server" IncrementalFilteringMode="StartsWith"
                                    SelectedIndex="0" AllowMouseWheel="False" FilterMinLength="1" CssClass="compClass">
                                            
                                </dx:ASPxComboBox>
                                                                                                            
                            </td>
                        </tr>
                    </table>
                                
                               
                </td>
                <td>&nbsp;</td>
            </tr>
                        
            <tr>
                <td>
                    *CVV/CID Number
                </td>
                <td style="width: 300px">
                    <dx:ASPxTextBox ID="txtCVV2" ClientInstanceName="txtCVV2" runat="server" Width="50px" MaxLength="3" 
                        AutoCompleteType="Disabled">
                        <ValidationSettings  ValidationGroup="cc">
                            <RequiredField IsRequired="True" ErrorText="CVV number is required"></RequiredField>
                            <RegularExpression ErrorText="Invalid CVV number" ValidationExpression="^([\d*]{1,20})$" >
                            </RegularExpression>
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    *Card Issuer
                </td>
                <td style="width: 300px">
                    <dx:ASPxTextBox ID="txtCardIssuer" runat="server" Width="200px" MaxLength="30" 
                        AutoCompleteType="Disabled">
                        <ValidationSettings ValidationGroup="cc">
                            <RequiredField IsRequired="True" ErrorText="Card issuer name is required"></RequiredField>
                            <RegularExpression ErrorText="Invalid card issuer name" ValidationExpression="^([1-zA-Z0-1@.\s]{1,200})$" >
                            </RegularExpression>
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    *Card Issuing Country
                </td>
                <td style="width: 300px;">
                <table>
                        <tr>
                            <td class="insideClass"><dx:ASPxComboBox ID="cmbIssuingCountry" runat="server" AllowMouseWheel="False" IncrementalFilteringMode="StartsWith"
                        FilterMinLength="1" CssClass="compClass">
                    </dx:ASPxComboBox>
                            
                </td>
                </td>
                </tr>
                </table>
                <td>&nbsp;</td>
            </tr>
        </table>
        <p class="paymentTabNote" style="font-family:Arial,Helvetica,san-serif;font-size:11px;padding:0px;margin:10px 0px;"><b>Note: </b>Expired or unsupported bank cards will not be listed during payment process.<!-- <br>E-Gift Voucher (EGV) must be redeemed before any other payment methods; any remaining balance has to be paid by credit card only. -->
        </p>
        </div>

        <div class="paymentTabNote">
        <p class="paymentTabNote" style="font-family:Arial,Helvetica,san-serif;font-size:11px;padding:0px;margin:10px 0px;">
        This address must be identical to your credit card billing address. Please enter the first 20 characters of your billing address.
        </p>
        <table width="100%" class="tblData">
            <tr>
                <td style="width:300px;">
                    *Street address
                </td>
                <td style="width: 300px">
                    <dx:ASPxTextBox ID="txtAddress" runat="server" Width="200px" MaxLength="20" 
                        AutoCompleteType="Disabled">
                    <ValidationSettings ValidationGroup="cc">
                        <RequiredField IsRequired="True" ErrorText="Address is required"></RequiredField>
                    </ValidationSettings>
                    </dx:ASPxTextBox>
                </td>
                <td>     
                    &nbsp;
                </td> 
            </tr>
            <tr>
                <td>
                    *Town/City
                </td>
                <td style="width: 300px">
                    <dx:ASPxTextBox ID="txtTown" runat="server" Width="200px" MaxLength="30" 
                        AutoCompleteType="Disabled">
                        <ValidationSettings ValidationGroup="cc">
                            <RequiredField IsRequired="True" ErrorText="Town / city name is required"></RequiredField>
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            
            <tr>
                <td style="width: 300px; height: 19px">
                    *Country
                </td>
                <td >
                <table>
                        <tr>
                            <td class="insideClass"><dx:ASPxComboBox ID="cmbCountryAddress" runat="server" AllowMouseWheel="False" IncrementalFilteringMode="StartsWith"
                    FilterMinLength="1" AutoPostBack="true"
                        onselectedindexchanged="cmbCountryAddress_SelectedIndexChanged">
                </dx:ASPxComboBox>
                </td>
                </tr>
                </table>           
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    *State and zip code
                </td>
                <td style="width: 300px">
                    <table>
                        <tr>
                            <td class="insideClass">
                                <dx:ASPxComboBox ID="cmbState" runat="server" AllowMouseWheel="False" IncrementalFilteringMode="StartsWith" Width="100px"
                                    FilterMinLength="1">
                                </dx:ASPxComboBox>
                            </td>
                            <td>
                               <dx:ASPxTextBox ID="txtZipCode" runat="server" Width="50px" MaxLength="30">
                                    <ValidationSettings ValidationGroup="cc">
                                        <RequiredField IsRequired="True" ErrorText="Zip Code is required"></RequiredField>
                                    </ValidationSettings>
                                </dx:ASPxTextBox>                                                       
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
            </tr>
        </table> 
        <p class="creditCardNote" style="font-family:Arial,Helvetica,san-serif;font-size:11px;padding:0px;margin:10px 0px;">Please be advised that credit card issuing banks may impose additional charges on all CROSS BORDER transactions. CROSS BORDER transactions are defined as transactions whereby the country of the cardholder's bank differs from that of the merchant.
        <br /><br />Please note that the additional charge is not imposed by AirAsia and neither do we benefit from it. You are advised to seek further clarification from your credit card issuing bank should a CROSS BORDER charge be applied to this transaction.</p>
        </div>
        
        <br />     
        </div>

        <!-- End Payment Method DIV-->

        <!--AG Payment DIV-->
        <div runat="server" id="divAG" visible="false" style="background-color:#E3E3E3">
            <div class="paymentTabNote" style="font-family:Arial,Helvetica,san-serif;font-size:12px;">
            <table width="100%">                          
            <tr>
                <td style="width:300px;">
                    Available AG Credit&nbsp;
                </td>

                <td>
                    <dx:ASPxLabel ID="lblAGCreditAmount" runat="server"  MaxLength="30"></dx:ASPxLabel>
                    <asp:Label ID="lblAGCreditCurrency" runat="server" MaxLength="30"></asp:Label>
                </td>
            </tr>

            </table><p class="paymentTabNote" style="font-family:Arial,Helvetica,san-serif;font-size:11px;padding:0px;margin:10px 0px;"><b>Note: </b>Expired or unsupported bank cards will not be listed during payment process.<!-- <br>E-Gift Voucher (EGV) must be redeemed before any other payment methods; any remaining balance has to be paid by credit card only. --></p>
            </div>
            <br />
            </div>
        

        <br />

        <dx:ASPxButton ID="btnProceedPayment" runat="server" ClientInstanceName="btnProceedPayment"
            CssClass="buttonL2" Text="Proceed Payment" onclick="btnProceedPayment_Click" >
            <ClientSideEvents Click="function(s, e) { SubmitPayment(); }"></ClientSideEvents>
        </dx:ASPxButton>


        </div>
    </div>

    

    </contenttemplate>
    
</asp:UpdatePanel>
<asp:UpdateProgress runat="server" ID="UpdateProgress" AssociatedUpdatePanelID="UpdatePanel" DisplayAfter="0" DynamicLayout="true">
        <ProgressTemplate>
            <div class="overlay2"></div>
            <div class="overlayContent2" >
                
                <img alt="In progress..." src="../Images/Airasia/loading_circle.gif" />
                
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <div id="atAGlanceRightContent">
        <div id="FlightDisplayHeader" class="atAGlanceDivHeader"><span>Flight details</span></div>
        <div class="atAGlanceDivBody flightDisplayBody flightDisplayContainer">
        <div class="jsdiv"></div>
        <table>                
        <tr><td><strong><asp:Label runat="server" Text="Depart" ID="lblDepart"></asp:Label></strong></td></tr>
        <tr>
        <td>
        <strong><asp:Label ID="lblDepartOrigin" runat="server" ></asp:Label></strong>
        &nbsp;to&nbsp;
        <strong><asp:Label ID="lblDepartDestination" runat="server" ></asp:Label></strong>
        </td>
        </tr>
        <tr>
        <td>
            <asp:Label ID="lblDateDepart" runat="server" ></asp:Label>
        </td>
        </tr>
        <tr>
        <td>
            <strong>Depart </strong><asp:Label ID="lblDepartStd" runat="server" ></asp:Label> 
            &nbsp; &nbsp; &nbsp; <strong>Arrive</strong>
            <asp:Label ID="lblDepartSta" runat="server" ></asp:Label>                
        </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblTextTransitDepart" runat="server" Text="Transit At" ForeColor="Red"></asp:Label>&nbsp;&nbsp;<asp:Label ID="lblTransitDepart" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>  
        <tr>
            <td>
                <asp:Label ID="lblDateTransitDepart" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        </table>
        <asp:Panel ID="pnlReturn" runat="server">
        <table>
        <tr><td><dt>Return</dt></td></tr>
        <tr>
        <td>
            <strong><asp:Label ID="lblReturnOrigin" runat="server" ></asp:Label></strong>
            &nbsp;to&nbsp;
            <!--Add by Riska BS, 20130115--><strong><asp:Label ID="lblReturnDestination" runat="server" ></asp:Label></strong>                
        </td>
        </tr>
        <tr>
        <td>
            <asp:Label ID="lblDateReturn" runat="server"></asp:Label>                    
        </td>
        </tr>
        <tr>
        <td>
            <strong>Depart </strong> <asp:Label ID="lblReturnStd" runat="server" ></asp:Label> 
            &nbsp; &nbsp; &nbsp; <strong>Arrive </strong> <asp:Label ID="lblReturnSta" runat="server"></asp:Label>                 
        </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblTextTransitReturn" runat="server" Text="Transit At" ForeColor="Red"></asp:Label>&nbsp;&nbsp;<asp:Label ID="lblTransitReturn" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>   
        <tr>
            <td>
                <asp:Label ID="lblTransitDateReturn" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        </table>                
        </asp:Panel>
        </div>
    </div>

    <script type="text/javascript">
//        window.onload = function () {
//            rdlPNR.SetEnabled(true);
//        }
        //rdlPNR.GetInputElement.disabled = false;

        //var city_value = eval('[<% =combinedCityValue %>]');
        //var city_text = eval('[<% =combinedCityText %>]');
        //alert(city_text);
        <% 
        if (tblPayment.Visible == true)
        {
        %>
        document.getElementById("<%=tblPayment.ClientID %>").style.visibility = "visible";
        document.getElementById("<%=rdlPNR.ClientID %>").style.visibility = "visible";
        document.getElementById("<%=txtPayAmount.ClientID %>").style.visibility = "visible";
        <% } %>
    </script>
</asp:Content>
