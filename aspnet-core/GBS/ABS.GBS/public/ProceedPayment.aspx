<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewPageMaster.Master" AutoEventWireup="true" CodeBehind="ProceedPayment.aspx.cs" Inherits="GroupBooking.Web.ProceedPayment" %>

<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %>
<%@ Register Src="../UserControl/flighdetail.ascx" TagName="flightdetail" TagPrefix="fd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../Scripts/overlay.js" type="text/javascript"></script>
    <script type="text/javascript">

        //$(function () {
        //    $('#sortable').sortable({ handle: 'span' }).disableSelection();
        //});

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
                //alert(country);
                //cmbIssuingCountry.SetValue(country);
                showCityItems(cmbContactCountryAddress.GetSelectedIndex());
                //showCityItems(cmbIssuingCountry.GetSelectedIndex());
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
            for (var x = 0; x < cnt; x++) {
                //                alert(city_value[index][x] + " " + city_value[index][x]);
                cmbContactState.AddItem(city_text[index][x], city_value[index][x]);
            }

        }
        function setValueToTextState() {
            txtContactState.SetText(cmbContactState.GetSelectedItem().value);
        }

        function ChangeTab() {
            txtMinimumPayment.SetText("0.00");
            txtMinPay.SetText("0.00");
            lblMinPay.SetText("0.00");
            txtDueAmount.SetText("0.00");
            lblFullPay.SetText("0.00");
            txtPayAmount.SetText("0.00");

            //var value = (document.getElementById("ctl00_ContentPlaceHolder2_cblPNR_VI").value);
            //var index = value.split("|");
            //if (index.length > 1) {
            //    for (var i in index) {
            //        if (i != 0) {
            //            var PNRText = cblPNR.GetItem(index[i].substring(0, 1)).text;
            //            var indexAmountDue = PNRText.indexOf("Amount Due:");
            //        }
            //    }
            //}
        }

        function roundToTwo(num) {
            return +(Math.round(num + "e+2") + "e-2");
        }

        function setMinimumPayment(s, e) {
            //alert(rdlPNR.GetValue() + " " + rdlPNR.GetSelectedIndex() + " " + rdlPNR.GetSelectedItem().text + " " + rdlPNR.GetItem(rdlPNR.GetSelectedIndex()).value);

            var tabMode = "";
            if (document.getElementById('<%=divCreditCard.ClientID%>') != null) {
                tabMode = "tabCredit";
                rdlContactPNR.SetSelectedIndex(rdlPNR.GetSelectedIndex());
            }
            else if (document.getElementById('<%=divAG.ClientID%>') != null) {
                tabMode = "tabAG";
                rdlContactPNR.SetSelectedIndex(cblPNR.GetSelectedIndex());
            }

            var minAmount = "0.00";
            var amountDue = "0.00";
            var amountDisplay = "0.00";


            if (tabMode == "tabCredit") {
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
                    $('#ctl00_ContentPlaceHolder2_cbFullPayment').removeAttr('disabled', 'disabled');
                }
                else {
                    txtPayAmount.SetText("");
                    txtMinimumPayment.SetText("");
                    txtMinPay.SetText("");
                    lblMinPay.SetText("");
                    $('#ctl00_ContentPlaceHolder2_cbFullPayment').attr('disabled', 'disabled');
                }

                var indexAmountDue = PNRText.indexOf("Amount Due:");
                if (indexAmountDue >= 0) {
                    var indexAmount = indexAmountDue + 11;
                    var indexCurrencyBracket = PNRText.indexOf("(", indexAmount);

                    amountDue = PNRText.substring(indexAmount, indexCurrencyBracket);
                    //alert(amountDue);
                    txtDueAmount.SetText(amountDue);
                    //document.getElementById("lblFullPay").SetText(amountDue);
                    lblFullPay.SetText(amountDue);
                }
            }
            else if (tabMode == "tabAG") {
                if (e.isSelected) {
                    if ($('#ctl00_ContentPlaceHolder2_cbFullPayment').is(':checked')) {
                        var indexAmPay = s.GetItem(e.index).text.indexOf("Amount Due:");
                        if (indexAmPay >= 0) {
                            var indexAmount = indexAmPay + 11;
                            var indexCurrencyBracket = s.GetItem(e.index).text.indexOf("(", indexAmount);
                            minAmount = s.GetItem(e.index).text.substring(indexAmount, indexCurrencyBracket);
                            txtPayAmount.SetText(roundToTwo(txtPayAmount.GetText() == "" ? 0 : parseFloat(txtPayAmount.GetText()) + parseFloat(minAmount.replace(/,/g, ''))));
                            txtDueAmount.SetText(roundToTwo(txtDueAmount.GetText() == "" ? 0 : parseFloat(txtDueAmount.GetText()) + parseFloat(minAmount.replace(/,/g, ''))));
                            $('#ctl00_ContentPlaceHolder2_cbFullPayment').removeAttr('disabled', 'disabled');
                        }

                        var indexMinPay = s.GetItem(e.index).text.indexOf("Min. Payment:");
                        if (indexMinPay >= 0) {
                            var indexAmount = indexMinPay + 13;
                            var indexCurrencyBracket = s.GetItem(e.index).text.indexOf("(", indexAmount);
                            minAmount = s.GetItem(e.index).text.substring(indexAmount, indexCurrencyBracket);
                            //txtPayAmount.SetText(roundToTwo(txtPayAmount.GetText() == "" ? 0 : parseFloat(txtPayAmount.GetText()) + parseFloat(minAmount.replace(/,/g, ''))));
                            txtMinimumPayment.SetText(roundToTwo(txtMinimumPayment.GetText() == "" ? 0 : parseFloat(txtMinimumPayment.GetText()) + parseFloat(minAmount.replace(/,/g, ''))));
                            txtDueAmount.SetText(roundToTwo(txtDueAmount.GetText() == "" ? 0 : parseFloat(txtDueAmount.GetText()) + parseFloat(minAmount.replace(/,/g, ''))));
                            txtMinPay.SetText(roundToTwo(txtMinPay.GetText() == "" ? 0 : parseFloat(txtMinPay.GetText()) + parseFloat(minAmount.replace(/,/g, ''))));
                            lblMinPay.SetText(roundToTwo(lblMinPay.GetText() == "" ? 0 : parseFloat(lblMinPay.GetText()) + parseFloat(minAmount.replace(/,/g, ''))));
                            $('#ctl00_ContentPlaceHolder2_cbFullPayment').removeAttr('disabled', 'disabled');
                        }
                        //else {
                        //    txtPayAmount.SetText("0.00");
                        //    txtDueAmount.SetText("0.00");
                        //    txtMinimumPayment.SetText("0.00");
                        //    txtMinPay.SetText("0.00");
                        //    lblMinPay.SetText("0.00");
                        //    $('#ctl00_ContentPlaceHolder2_cbFullPayment').attr('disabled', 'disabled');
                        //}
                    }
                    else {
                        var indexMinPay = s.GetItem(e.index).text.indexOf("Min. Payment:");
                        if (indexMinPay >= 0) {
                            var indexAmount = indexMinPay + 13;
                            var indexCurrencyBracket = s.GetItem(e.index).text.indexOf("(", indexAmount);
                            minAmount = s.GetItem(e.index).text.substring(indexAmount, indexCurrencyBracket);
                            txtPayAmount.SetText(roundToTwo(txtPayAmount.GetText() == "" ? 0 : parseFloat(txtPayAmount.GetText()) + parseFloat(minAmount.replace(/,/g, ''))));
                            txtDueAmount.SetText(roundToTwo(txtDueAmount.GetText() == "" ? 0 : parseFloat(txtDueAmount.GetText()) + parseFloat(minAmount.replace(/,/g, ''))));
                            txtMinimumPayment.SetText(roundToTwo(txtMinimumPayment.GetText() == "" ? 0 : parseFloat(txtMinimumPayment.GetText()) + parseFloat(minAmount.replace(/,/g, ''))));
                            txtMinPay.SetText(roundToTwo(txtMinPay.GetText() == "" ? 0 : parseFloat(txtMinPay.GetText()) + parseFloat(minAmount.replace(/,/g, ''))));
                            lblMinPay.SetText(roundToTwo(lblMinPay.GetText() == "" ? 0 : parseFloat(lblMinPay.GetText()) + parseFloat(minAmount.replace(/,/g, ''))));
                            $('#ctl00_ContentPlaceHolder2_cbFullPayment').removeAttr('disabled', 'disabled');
                        }
                        //else {
                        //    txtPayAmount.SetText("0.00");
                        //    //txtDueAmount.SetText("");
                        //    txtMinimumPayment.SetText("0.00");
                        //    txtMinPay.SetText("0.00");
                        //    lblMinPay.SetText("0.00");
                        //    $('#ctl00_ContentPlaceHolder2_cbFullPayment').attr('disabled', 'disabled');
                        //}
                    }

                    var indexAmountDue = s.GetItem(e.index).text.indexOf("Amount Due:");
                    if (indexAmountDue >= 0) {
                        var indexAmount = indexAmountDue + 11;
                        var indexCurrencyBracket = s.GetItem(e.index).text.indexOf("(", indexAmount);

                        amountDue = s.GetItem(e.index).text.substring(indexAmount, indexCurrencyBracket);
                        //alert(amountDue);
                        txtDueAmount.SetText(amountDue);
                        //document.getElementById("lblFullPay").SetText(amountDue);
                        lblFullPay.SetText(amountDue);
                    }
                }
                else {
                    if ($('#ctl00_ContentPlaceHolder2_cbFullPayment').is(':checked')) {
                        var indexMinPay = s.GetItem(e.index).text.indexOf("Amount Due:");
                        if (indexMinPay >= 0) {
                            var indexAmount = indexMinPay + 11;
                            var indexCurrencyBracket = s.GetItem(e.index).text.indexOf("(", indexAmount);
                            minAmount = s.GetItem(e.index).text.substring(indexAmount, indexCurrencyBracket);
                            txtPayAmount.SetText(roundToTwo(txtPayAmount.GetText() == "" ? 0 : parseFloat(txtPayAmount.GetText()) - parseFloat(minAmount.replace(/,/g, ''))));
                            txtDueAmount.SetText(roundToTwo(txtDueAmount.GetText() == "" ? 0 : parseFloat(txtDueAmount.GetText()) - parseFloat(minAmount.replace(/,/g, ''))));
                            //txtMinimumPayment.SetText(roundToTwo(txtMinimumPayment.GetText() == "" ? 0 : parseFloat(txtMinimumPayment.GetText()) - parseFloat(minAmount.replace(/,/g, ''))));
                            //txtMinPay.SetText(roundToTwo(txtMinPay.GetText() == "" ? 0 : parseFloat(txtMinPay.GetText()) - parseFloat(minAmount.replace(/,/g, ''))));
                            //lblMinPay.SetText(roundToTwo(lblMinPay.GetText() == "" ? 0 : parseFloat(lblMinPay.GetText()) - parseFloat(minAmount.replace(/,/g, ''))));
                            $('#ctl00_ContentPlaceHolder2_cbFullPayment').removeAttr('disabled', 'disabled');
                        }
                        var indexMinPay = s.GetItem(e.index).text.indexOf("Min. Payment:");
                        if (indexMinPay >= 0) {
                            var indexAmount = indexMinPay + 13;
                            var indexCurrencyBracket = s.GetItem(e.index).text.indexOf("(", indexAmount);
                            minAmount = s.GetItem(e.index).text.substring(indexAmount, indexCurrencyBracket);
                            //txtPayAmount.SetText(roundToTwo(txtPayAmount.GetText() == "" ? 0 : parseFloat(txtPayAmount.GetText()) + parseFloat(minAmount.replace(/,/g, ''))));
                            txtMinimumPayment.SetText(roundToTwo(txtMinimumPayment.GetText() == "" ? 0 : parseFloat(txtMinimumPayment.GetText()) - parseFloat(minAmount.replace(/,/g, ''))));
                            txtDueAmount.SetText(roundToTwo(txtDueAmount.GetText() == "" ? 0 : parseFloat(txtDueAmount.GetText()) - parseFloat(minAmount.replace(/,/g, ''))));
                            txtMinPay.SetText(roundToTwo(txtMinPay.GetText() == "" ? 0 : parseFloat(txtMinPay.GetText()) - parseFloat(minAmount.replace(/,/g, ''))));
                            lblMinPay.SetText(roundToTwo(lblMinPay.GetText() == "" ? 0 : parseFloat(lblMinPay.GetText()) - parseFloat(minAmount.replace(/,/g, ''))));
                            $('#ctl00_ContentPlaceHolder2_cbFullPayment').removeAttr('disabled', 'disabled');
                        }
                        //else {
                        //    txtPayAmount.SetText("0.00");
                        //    txtMinimumPayment.SetText("0.00");
                        //    txtMinPay.SetText("0.00");
                        //    lblMinPay.SetText("0.00");
                        //    $('#ctl00_ContentPlaceHolder2_cbFullPayment').attr('disabled', 'disabled');
                        //}
                    }
                    else {
                        var indexMinPay = s.GetItem(e.index).text.indexOf("Min. Payment:");
                        if (indexMinPay >= 0) {
                            var indexAmount = indexMinPay + 13;
                            var indexCurrencyBracket = s.GetItem(e.index).text.indexOf("(", indexAmount);
                            minAmount = s.GetItem(e.index).text.substring(indexAmount, indexCurrencyBracket);
                            txtPayAmount.SetText(roundToTwo(txtPayAmount.GetText() == "" ? 0 : parseFloat(txtPayAmount.GetText()) - parseFloat(minAmount.replace(/,/g, ''))));
                            txtDueAmount.SetText(roundToTwo(txtDueAmount.GetText() == "" ? 0 : parseFloat(txtDueAmount.GetText()) - parseFloat(minAmount.replace(/,/g, ''))));
                            txtMinimumPayment.SetText(roundToTwo(txtMinimumPayment.GetText() == "" ? 0 : parseFloat(txtMinimumPayment.GetText()) - parseFloat(minAmount.replace(/,/g, ''))));
                            txtMinPay.SetText(roundToTwo(txtMinPay.GetText() == "" ? 0 : parseFloat(txtMinPay.GetText()) - parseFloat(minAmount.replace(/,/g, ''))));
                            lblMinPay.SetText(roundToTwo(lblMinPay.GetText() == "" ? 0 : parseFloat(lblMinPay.GetText()) - parseFloat(minAmount.replace(/,/g, ''))));
                            $('#ctl00_ContentPlaceHolder2_cbFullPayment').removeAttr('disabled', 'disabled');
                        }
                        //else {
                        //    txtPayAmount.SetText("0.00");
                        //    txtMinimumPayment.SetText("0.00");
                        //    txtMinPay.SetText("0.00");
                        //    lblMinPay.SetText("0.00");
                        //    $('#ctl00_ContentPlaceHolder2_cbFullPayment').attr('disabled', 'disabled');
                        //}

                        var indexAmountDue = s.GetItem(e.index).text.indexOf("Amount Due:");
                        if (indexAmountDue >= 0) {
                            var indexAmount = indexAmountDue + 11;
                            var indexCurrencyBracket = s.GetItem(e.index).text.indexOf("(", indexAmount);

                            amountDue = s.GetItem(e.index).text.substring(indexAmount, indexCurrencyBracket);
                            //alert(amountDue);
                            txtDueAmount.SetText(amountDue);
                            //document.getElementById("lblFullPay").SetText(amountDue);
                            lblFullPay.SetText(amountDue);
                        }
                    }
                }

            }
            if (tabMode == "tabAG") {
                if (document.getElementById("lblProcessFee") != null)
                    document.getElementById("lblProcessFee").SetText("0.00");
                //lblProcessFee.SetText("0.00");
            }
            else {
                lblProcessFee.SetText(rdlProcessFee.GetItem(rdlPNR.GetSelectedIndex()).text);
            }
            //if (tabMode == "tabAG") {
            //    txtPayAmount.SetText(amountDue);
            //    txtMinPay.SetText(amountDue);
            //    lblMinPay.SetText(amountDue);
            //}

            document.getElementById("divMsg").style.visibility = "hidden";

            //Amended by Ellis 20170307, to make it possible to use cookies for contact details
            //            setContactDetails();
        }

        function PayFull() {
            var minAmount = "0.00";
            var amountDue = "0.00";
            var amountDisplay = "0.00";

            if (document.getElementById('<%=divCreditCard.ClientID%>') != null) {
                rdlContactPNR.SetSelectedIndex(rdlPNR.GetSelectedIndex());
                var PNRText = rdlPNR.GetSelectedItem().text;
                var indexAmountDue = PNRText.indexOf("Amount Due:");
                if (rdlPNR.GetSelectedIndex() >= 0) {
                    if ($('#ctl00_ContentPlaceHolder2_cbFullPayment').is(':checked')) {
                        if (indexAmountDue >= 0) {
                            var indexAmount = indexAmountDue + 11;
                            var indexCurrencyBracket = PNRText.indexOf("(", indexAmount);
                            amountDue = PNRText.substring(indexAmount, indexCurrencyBracket);
                            txtPayAmount.SetText(amountDue);
                        }
                    }
                    else {
                        txtPayAmount.SetText(lblMinPay.GetValue());
                    }
                }
            }
            else if (document.getElementById('<%=divAG.ClientID%>') != null) {
                var value = (document.getElementById("ctl00_ContentPlaceHolder2_cblPNR_VI").value);
                var index = value.split("|");
                if (index.length > 1) {
                    for (var i in index) {
                        if (i != 0) {
                            var PNRText = cblPNR.GetItem(index[i].substring(0, 1)).text;
                            var indexAmountDue = PNRText.indexOf("Amount Due:");
                            if ($('#ctl00_ContentPlaceHolder2_cbFullPayment').is(':checked')) {
                                if (indexAmountDue >= 0) {
                                    var indexAmount = indexAmountDue + 11;
                                    var indexCurrencyBracket = PNRText.indexOf("(", indexAmount);
                                    amountDue = PNRText.substring(indexAmount, indexCurrencyBracket);
                                    amountDisplay = parseFloat(amountDisplay) + parseFloat(amountDue.replace(/,/g, ''));
                                    txtPayAmount.SetText(roundToTwo(amountDisplay));
                                    txtDueAmount.SetText(roundToTwo(amountDisplay));
                                }
                            }
                            else {
                                txtPayAmount.SetText(lblMinPay.GetValue());
                            }
                        }
                    }
                }
                //for (var i = 0; i < cblPNR.length; i++)
                //{
                //    if (cblPNR[i].isSelected)
                //    {
                //        alert(i);
                //    }
                //}
                //var PNRText = cblPNR.GetItem(cblPNR.GetSelectedIndex()).text;
                //var indexAmountDue = PNRText.indexOf("Amount Due:");
                //if (cblPNR.GetSelectedIndex() >= 0) {
                //        if ($('#ctl00_ContentPlaceHolder2_cbFullPayment').is(':checked')) {
                //            //for (var i in cblPNR) {
                //            //txtPayAmount.SetText(cblPNR.GetItem(i));
                //            //}
                //            //if (indexAmountDue >= 0) {
                //            //    var indexAmount = indexAmountDue + 11;
                //            //    var indexCurrencyBracket = PNRText.indexOf("(", indexAmount);
                //            //    amountDue = PNRText.substring(indexAmount, indexCurrencyBracket);
                //            //    txtPayAmount.SetText(parseFloat(amountDue.replace(/,/g, '')));
                //            //}
                //        }
                //        else {
                //            txtPayAmount.SetText(lblMinPay.GetValue());
                //        }
                //    //if ($('#ctl00_ContentPlaceHolder2_cbFullPayment').is(':checked')) {
                //    //    if (indexAmountDue >= 0) {
                //    //        var indexAmount = indexAmountDue + 11;
                //    //        var indexCurrencyBracket = PNRText.indexOf("(", indexAmount);
                //    //        amountDue = PNRText.substring(indexAmount, indexCurrencyBracket);
                //    //        txtPayAmount.SetText(parseFloat(amountDue.replace(/,/g, '')));
                //    //    }
                //    //}
                //    //else {
                //    //    txtPayAmount.SetText(lblMinPay.GetValue());
                //    //}
                //}
            }



        }

        function OnCallbackCompleteHide(s, e) {
            LoadingPanel.Hide();
        }

        //function validate(evt) {
        //    var theEvent = evt || window.event;
        //    var key = theEvent.keyCode || theEvent.which;
        //    key = String.fromCharCode(key);
        //    var regex = /[0-9]|\./;
        //    if (!regex.test(key)) {
        //        theEvent.returnValue = false;
        //        if (theEvent.preventDefault) theEvent.preventDefault();

        //    }
        //}

        function hide() {
            LoadingPanel.Hide();
        }

        function submitform() {
            DirectDebitForm.submit();
        }

        function updatecbAllPNRState(s, e) {
            var amountDisplay = "0.00";
            var amountDisplayMin = "0.00";

            if (document.getElementById('<%=divAG.ClientID%>') != null) {
                s.SetVisible(true);
                if (s.GetChecked()) {
                    cblPNR.SelectAll();
                    var value = (document.getElementById("ctl00_ContentPlaceHolder2_cblPNR_VI").value);
                    var index = value.split("|");
                    if (index.length > 1) {
                        for (var i in index) {
                            if (i != 0) {
                                var PNRText = cblPNR.GetItem(index[i].substring(0, 1)).text;
                                if ($('#ctl00_ContentPlaceHolder2_cbFullPayment').is(':checked')) {
                                    var indexAmountDue = PNRText.indexOf("Amount Due:");
                                    if (indexAmountDue >= 0) {
                                        var indexAmount = indexAmountDue + 11;
                                        var indexCurrencyBracket = PNRText.indexOf("(", indexAmount);
                                        amountDue = PNRText.substring(indexAmount, indexCurrencyBracket);
                                        amountDisplay = parseFloat(amountDisplay) + parseFloat(amountDue.replace(/,/g, ''));
                                        txtPayAmount.SetText(roundToTwo(amountDisplay));
                                        txtDueAmount.SetText(roundToTwo(amountDisplay));
                                    }

                                    if (PNRText.indexOf("No Min. Payment is Required") < 0 && PNRText.indexOf("Min. Payment:") >= 0) {
                                        var indexAmountDue = PNRText.indexOf("Min. Payment:");
                                        var indexAmount = indexAmountDue + 13;
                                        var indexCurrencyBracket = PNRText.indexOf("(", indexAmount);
                                        amountDue = PNRText.substring(indexAmount, indexCurrencyBracket);
                                        amountDisplayMin = parseFloat(amountDisplayMin) + parseFloat(amountDue.replace(/,/g, ''));
                                        txtMinimumPayment.SetText(roundToTwo(amountDisplayMin));
                                        txtMinPay.SetText(roundToTwo(amountDisplayMin));
                                        lblMinPay.SetText(roundToTwo(amountDisplayMin));
                                    }

                                }
                                else {
                                    if (PNRText.indexOf("No Min. Payment is Required") < 0 && PNRText.indexOf("Min. Payment:") >= 0) {
                                        var indexAmountDue = PNRText.indexOf("Min. Payment:");
                                        var indexAmount = indexAmountDue + 13;
                                        var indexCurrencyBracket = PNRText.indexOf("(", indexAmount);
                                        amountDue = PNRText.substring(indexAmount, indexCurrencyBracket);
                                        amountDisplay = parseFloat(amountDisplay) + parseFloat(amountDue.replace(/,/g, ''));
                                        txtPayAmount.SetText(roundToTwo(amountDisplay));
                                        txtDueAmount.SetText(roundToTwo(amountDisplay));
                                        txtMinimumPayment.SetText(roundToTwo(amountDisplay));
                                        txtMinPay.SetText(roundToTwo(amountDisplay));
                                        lblMinPay.SetText(roundToTwo(amountDisplay));
                                    }

                                }

                                //txtPayAmount.SetText(lblMinPay.GetValue());
                            }
                        }
                    }
                }
                else {
                    cblPNR.UnselectAll();
                    txtPayAmount.SetText("0.00");
                    txtMinimumPayment.SetText("0.00");
                    txtMinPay.SetText("0.00");
                    lblMinPay.SetText("0.00");
                }

            }
            else {
                s.SetVisible(false);
            }
        }



        function SubmitPayment() {
            //alert("submit payment");

            if (typeof TabControl !== "undefined") {
                var tab = TabControl.GetActiveTab();
                //alert(tab.name);
                switch (tab.name) {
                    case "TabCredit":
                        if (ASPxClientEdit.ValidateGroup('mandatory') && ASPxClientEdit.ValidateGroup('cc')) {
                            //show_overlay();
                            //alert("ok");
                            $("#ctl00_ContentPlaceHolder2_hError").val("0");
                            return;
                        } else {
                            //alert("error");
                            $("#ctl00_ContentPlaceHolder2_hError").val("1");
                            //alert($("#ContentPlaceHolder2_hError").val());
                        }
                        break;
                    case "TabAG":
                        $("#ctl00_ContentPlaceHolder2_tabActive").val("ag");
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
            /* $("#ctl00_ContentPlaceHolder2_btnProceedPayment").click(function () {
                 if (TabControl != undefined) {
                     var tab = TabControl.GetActiveTab();
                     //alert(tab.name);
                     switch (tab.name) {
                         case "TabCredit":
                             if (ASPxClientEdit.ValidateGroup('mandatory') && ASPxClientEdit.ValidateGroup('cc')) {
                                 //show_overlay();
                                 alert("ok");
                                 $("#ctl00_ContentPlaceHolder2_hError").val("0");
                                 return;
                             } else {
                                 alert("error");
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
 
             $("#ctl00_ContentPlaceHolder2_btnProceedPaymentBottom").click(function () {
                 if (TabControl != undefined) {
                     var tab = TabControl.GetActiveTab();
                     //alert(tab.name);
                     switch (tab.name) {
                         case "TabCredit":
                             if (ASPxClientEdit.ValidateGroup('mandatory') && ASPxClientEdit.ValidateGroup('cc')) {
                                 //show_overlay();
                                 alert("ok");
                                 $("#ctl00_ContentPlaceHolder2_hError").val("0");
                                 return;
                             } else {
                                 alert("error");
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
                 
             }); */

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

        var varTmr;
        var active = 0;
        function countdown() {
            seconds = lblTimer.GetValue();
            var timeArray = seconds.split(/[:]+/);
            var m = timeArray[0];
            var s = checkSecond((timeArray[1] - 1));

            if (m > 0 || s > 0) {
                if (s == 59) { m = m - 1; }
                lblTimer.SetText(m + ":" + s);
                varTmr = setTimeout("countdown()", 1000);
            } else {
                window.location.href = '../public/SearchFlight.aspx';
            }


            //if (seconds > 0) {
            //    active = 0;
            //    lblTimer.SetText(seconds - 1);
            //    varTmr = setTimeout("countdown()", 1000);
            //} else {
            //    window.location.href = '../public/SelectFlight.aspx';
            //}
        }

        function startTimer() {
            varTmr = setTimeout("countdown()", 1000);
        }

        function checkSecond(sec) {
            if (sec < 10 && sec >= 0) { sec = "0" + sec }; // add zero in front of numbers < 10
            if (sec < 0) { sec = "59" };
            return sec;
        }

        window.onload = startTimer();

    </script>

    <style type="text/css">
        .overlay2 {
            position: fixed;
            z-index: 1000;
            top: 0px;
            left: 0px;
            right: 0px;
            bottom: 0px;
            background-color: #aaa;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }

        .overlayContent2 {
            z-index: 1001;
            margin: 250px auto;
            position: fixed;
            top: 50%;
            left: 50%;
            margin-top: -50px;
            margin-left: -100px;
        }


        .style1 {
            width: 82px;
        }
    </style>

    <div class="row page-header clearfix">
        <div class="col-sm-6">
            <h4 class="mt-0 mb-5">Payment</h4>
            Booking/Payment
        </div>


        <div class="col-sm-6">
            <div align="right">
                <table>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <div id="counterAlert">
                                <p>Time Remaining</p>
                                min<dx:ASPxLabel runat="server" ID="lblTimer" ClientInstanceName="lblTimer" Text="10:00"></dx:ASPxLabel>
                                sec
                            </div>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <dx:ASPxButton ID="btnRefresh" runat="server" ClientInstanceName="btnRefresh" Text="Refresh Page">
                                <ClientSideEvents Click="function(s, e) { clearTimeout(varTmr);LoadingPanel.SetText(&#39;Please Wait...&#39;); LoadingPanel.Show(); $('#ctl00_ContentPlaceHolder2_tabActive').val('ag');location.reload();}"></ClientSideEvents>
                            </dx:ASPxButton>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <%--            <a href="PassengerUpload.aspx" style="text-decoration:none;color:white;"><button type="button" class="btn btn-raised btn-danger">Proceed to Payment</button></a>--%>
                            <dx:ASPxButton ID="btnProceedPayment" runat="server" ClientInstanceName="btnProceedPayment" CssClass="buttonL2" Text="Proceed Payment" OnClick="btnProceedPayment_Click">
                                <ClientSideEvents Click="function(s, e) { clearTimeout(varTmr);LoadingPanel.SetText(&#39;Please Wait...&#39;); LoadingPanel.Show(); SubmitPayment(); }"></ClientSideEvents>
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

    <div class="widget page-content container-fluid mb-0" style="">
        <div class="widget-body" style="padding: 0;">
            <div class="row">
                <div class="col-sm-5" style="">
                    <table id="paymentDue" width="100%">
                        <tr class="totalFare">
                            <td class="" style="font-weight: 700;">
                                <h4>Amount Due</h4>
                            </td>
                            <td class="" style="font-weight: 700; text-align: right;">
                                <h4>
                                    <asp:Label ID="lblAmountDue" runat="server" Text="0" ForeColor="Red"></asp:Label>&nbsp;
                                    <asp:Label ID="lblAmountDueCurrency" runat="server" Text="MYR" ForeColor="Red"></asp:Label>
                                </h4>
                            </td>
                        </tr>
                    </table>

                    <div style="padding-left: 5px; color: #da0004; font-size: 20px; font-weight: 400;">
                        <h4>Payment Details</h4>
                    </div>

                    <table id="paymentDetailsTbl" class="table table-bordered">
                        <tbody>
                            <tr>
                                <td class="">Booking ID</td>
                                <td class="">
                                    <asp:Label ID="lblTransID" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="">Total Paid Amount</td>
                                <td class="">
                                    <asp:Label ID="lblAmountPaid" runat="server" Text="0"></asp:Label>&nbsp;
                                    <asp:Label ID="lblAmountPaidCurrency" runat="server" Text="MYR"></asp:Label>
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td class="">Depart Total</td>
                                <td class="">
                                    <asp:Label ID="lblDepartTotal" runat="server" Text="0"></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="lblDepartTotalCurrency" runat="server" Text="MYR"></asp:Label>
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td class="">Return Total</td>
                                <td class="">
                                    <asp:Label ID="lblReturnTotal" runat="server" Text="0"></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="lblReturnTotalCurrency" runat="server" Text="MYR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="">Total All In Fare Amount</td>
                                <td>
                                    <span>
                                        <asp:Label ID="lblCurrentTotal" runat="server" Text="0"></asp:Label>&nbsp;
                                        <asp:Label ID="lblCurrentTotalCurrency" runat="server" Text="MYR"></asp:Label>
                                    </span>
                                </td>
                            </tr>
                            <tr id="trProcessingFee" runat="server" style="display: none">
                                <td style="">Processing Fee</td>
                                <td>
                                    <span>
                                        <%--<asp:Label ID="lblProcessFee" runat="server" Text="0"></asp:Label>&nbsp;
                                        <asp:Label ID="lblCurrencyProcessFee" runat="server" Text="MYR"></asp:Label>--%>
                                    </span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <div id="flightDetailBookingInfo" class="col-sm-6" style="padding-left: 10px; margin-left: 0;">
                    <%-- 20170419 - Sienny (Add Total Pax Info) --%>
                    <table id="tbTotalPax" width="100%">
                        <tr class="totalFare">
                            <td class="" colspan="2" style="font-weight: 700;">
                                <h4>Total Pax</h4>
                            </td>
                            <td class="" style="padding-top: 5px; float: right">
                                <asp:Label ID="lbl_num" runat="server"></asp:Label></td>
                        </tr>
                    </table>

                    <%--<div style="padding-left: 5px;color: #da0004;font-size:20px;font-weight:400;">
                        <h4>&nbsp;</h4>
                    </div>
                </div>
                
                <div id="flightDetailBookingInfo" class="col-sm-6" style="padding-left: 10px;margin-left:0;">--%>
                    <div style="padding-left: 5px; color: #da0004; font-size: 20px; font-weight: 400;">
                        <h4>Payment Schedule</h4>
                    </div>
                    <div id="divPaymentSchedule" runat="server">
                    </div>
                </div>
            </div>
            <div class="row" style="background: #fafafa; padding: 8px;">
                <div class="col-md-8">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" id="Table1" runat="server">
                        <tbody>
                            <%--                            <tr>
                                <td colspan="3"><strong>Select Booking PNR to Pay</strong><br />
                                </td>
                            </tr>

                            <tr>
                                <td colspan="3">
                                    <input checked="true" type="radio" id="Radio1" runat="server" />PNR : To be Confirmed (Total 10 Pax) Amount Due 2,300.00 MYR , Min. Payment <span style="font-weight:700;">100.00 MYR</span></td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <input type="radio" id="Radio2" runat="server" />PNR : To be Confirmed (Total 20 Pax) Amount Due 4,600.00 MYR , Min. Payment <span style="font-weight:700;">200.00 MYR</span></td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <input type="radio" id="Radio3" runat="server" />PNR : To be Confirmed (Total 20 Pax) Amount Due 4,600.00 MYR , Min. Payment <span style="font-weight:700;">200.00 MYR</span></td>
                            </tr>
                            <tr>
                                <td style="padding: 10px">Min. Payment : <font color="#da0004;">100.00 MYR</font>
                                </td>
                                <td>Amount to Pay : 
                                </td>
                                <td>
                                    <dx:ASPxTextBox ID="TxtAmont" runat="server"></dx:ASPxTextBox>
                                </td>
                            </tr>--%>
                            <tr>
                                <td colspan="3"><strong>Select Booking PNR to Pay</strong><br />
                                    <dx:ASPxCheckBox ID="cbAllPNR" ClientInstanceName="cbAllPNR" runat="server" Text="Select All PNRs" Font-Bold="true" Checked="true">
                                        <ClientSideEvents ValueChanged="updatecbAllPNRState" Init="updatecbAllPNRState" />
                                    </dx:ASPxCheckBox>
                                    <!-- temp set readonly = false, 20170119, by ketee -->
                                    <div id="divPNR" runat="server" width="100%" style="margin-right: 0px; padding-right: 0px;">
                                        <dx:ASPxRadioButtonList ID="rdlPNR" ClientInstanceName="rdlPNR" runat="server" RepeatDirection="Vertical"
                                            Border-BorderStyle="None" RepeatColumns="1" RepeatLayout="Table" Paddings-Padding="0"
                                            CssClass="tdClass" Width="900px">
                                            <ClientSideEvents SelectedIndexChanged="setMinimumPayment" />
                                            <ValidationSettings ValidationGroup="mandatory" ErrorTextPosition="Top">
                                                <RequiredField ErrorText="Please select booking PNR to pay" IsRequired="true" />
                                            </ValidationSettings>
                                        </dx:ASPxRadioButtonList>
                                        <dx:ASPxCheckBoxList ID="cblPNR" ClientInstanceName="cblPNR" runat="server" RepeatDirection="Vertical"
                                            Border-BorderStyle="None" RepeatColumns="1" RepeatLayout="Table" Paddings-Padding="0"
                                            CssClass="tdClass" Width="900px">
                                            <ClientSideEvents SelectedIndexChanged="setMinimumPayment" />
                                            <ValidationSettings ValidationGroup="mandatory" ErrorTextPosition="Top">
                                                <RequiredField ErrorText="Please select booking PNR to pay" IsRequired="true" />
                                            </ValidationSettings>
                                        </dx:ASPxCheckBoxList>
                                        <!-- added by diana 20170211, to show processing fee -->
                                        <div style="display: none">
                                            <dx:ASPxRadioButtonList ID="rdlProcessFee" ClientInstanceName="rdlProcessFee" runat="server" RepeatDirection="Vertical"
                                                Border-BorderStyle="None" RepeatColumns="1" RepeatLayout="Table" Paddings-Padding="0"
                                                CssClass="tdClass" Width="900px">
                                            </dx:ASPxRadioButtonList>
                                        </div>
                                    </div>

                                    <div id="divContactPNR" runat="server" width="100%" style="margin-right: 0px; padding-right: 0px;">
                                        <dx:ASPxRadioButtonList ID="rdlContactPNR" ClientVisible="false" ClientInstanceName="rdlContactPNR" runat="server" RepeatDirection="Vertical"
                                            Border-BorderStyle="None" RepeatColumns="1" RepeatLayout="Table" Paddings-Padding="0"
                                            CssClass="tdClass" Width="740px" ReadOnly="true">
                                        </dx:ASPxRadioButtonList>
                                    </div>

                                </td>
                            </tr>

                            <tr>
                                <td align="left" style="padding: 0px 2px; width: 250px" valign="middle">
                                    <strong>
                                        <asp:Label ID="lblMinCap" runat="server" Text="Min. Payment: "></asp:Label></strong><dx:ASPxLabel ID="lblMinPay" ClientInstanceName="lblMinPay" ClientEnabled="true" runat="server" Text="0.00" ForeColor="Red"></dx:ASPxLabel>
                                    <dx:ASPxLabel ID="lblFullPay" ClientInstanceName="lblFullPay" ClientVisible="false" runat="server" ForeColor="Red"></dx:ASPxLabel>
                                    &nbsp;<asp:Label ID="lblMinPayCurrency" runat="server" Text="MYR" ForeColor="Red"></asp:Label></td>

                                <td style="padding: 0px 2px; width: 125px; padding-right: 5px;" align='right' valign="middle"><strong>Amount To Pay</strong></td>
                                <td style="padding: 0px 2px;" align="left" valign="middle">
                                    <dx:ASPxTextBox ID="txtMinimumPayment" ClientEnabled="true" ClientInstanceName="txtMinimumPayment" runat="server" ClientVisible="false" Text="0.00"></dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="txtMinPay" ClientEnabled="true" ClientInstanceName="txtMinPay" runat="server" ClientVisible="false" Text="0.00"></dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="txtDueAmount" ClientEnabled="true" ClientInstanceName="txtDueAmount" runat="server" ClientVisible="false" Text="0.00"></dx:ASPxTextBox>
                                    <dx:ASPxTextBox ID="txtProcFee" ClientEnabled="true" ClientInstanceName="txtProcFee" runat="server" ClientVisible="false" Text="0.00"></dx:ASPxTextBox>

                                    <div style="display: inline-block; margin-right: 10px; width: 120px; margin-bottom: -3px;">
                                        <dx:ASPxTextBox ID="txtPayAmount" ReadOnly="true"
                                            ClientInstanceName="txtPayAmount" runat="server" Width="120px" MaxLength="30">
                                            <ValidationSettings ValidationGroup="mandatory" ErrorTextPosition="Top">
                                                <RequiredField ErrorText=" " IsRequired="True" />
                                                <RegularExpression ErrorText="Invalid payment amount" ValidationExpression="^([\d.,*]{1,20})$"></RegularExpression>
                                                <RequiredField IsRequired="false" ErrorText="Payment amount is required"></RequiredField>
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </div>
                                    <div style="display: inline-block; margin-right: 10px;">
                                        <input type="checkbox" runat="server" name="cbFullPayment" id="cbFullPayment" onchange="javascript:PayFull();" value="1" />Full Payment
                                    </div>
                                </td>

                            </tr>

                        </tbody>
                    </table>
                </div>
            </div>
            <div class="row" style="background: #fafafa; padding: 8px; margin: 8px 0;">
                <div class="col-md-12" style="padding-top: 10px">

                    <div style="padding-left: 5px; color: #da0004; font-size: 20px; font-weight: 400;">
                        <h4>Contact Details</h4>
                        <p class="paymentTabNote" style="font-family: Arial,Helvetica,san-serif; font-size: 11px; padding: 0px; margin: 10px 0px;">
                            Please fill up your contact details.
                        </p>
                        <dx:ASPxCheckBox Text="Remember Me" ID="chkRememberMe" Checked="true" OnCheckedChanged="RememberMe_CheckedChanged" AutoPostBack="true" runat="server" Style="font-family: Arial,Helvetica,san-serif; font-size: 11px; padding: 0px; margin: 10px 0px;"></dx:ASPxCheckBox>
                    </div>
                    <div class="col-md-12" style="max-width: 920px;">
                        <asp:ScriptManager ID="ScriptManager1" runat="server" />

                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <table id="contactDetailsForm" style="width: 100%; margin-bottom: 20px;" class="table tableNoBorder" runat="server">
                                    <tr>
                                        <td style="padding: 10px;">
                                            <div class="HeaderContact">
                                                <label><font color="#da0004">*</font>Title</label>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="ContentContact">
                                                <dx:ASPxComboBox runat="server" ID="cmbContactTitle" IncrementalFilteringMode="StartsWith"
                                                    ClientEnabled="true" ClientInstanceName="cmbContactTitle"
                                                    AllowMouseWheel="False" FilterMinLength="1" SelectedIndex="0">
                                                    <Items>
                                                        <dx:ListEditItem Text="MR" Value="MR" Selected="True" />
                                                        <dx:ListEditItem Text="MS" Value="MS" />
                                                    </Items>
                                                </dx:ASPxComboBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="HeaderContact">
                                                <label><font color="#da0004">*</font>Email</label>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="ContentContact">
                                                <dx:ASPxTextBox ID="txtContactEmail" ClientInstanceName="txtContactEmail" ClientEnabled="true" runat="server" Width="200px" MaxLength="50"
                                                    AutoCompleteType="Disabled">
                                                    <ValidationSettings ValidationGroup="mandatory">
                                                        <RequiredField IsRequired="True" ErrorText="Email is required"></RequiredField>
                                                        <RegularExpression ErrorText="Email is invalid" ValidationExpression="^[\w-.]+(?:\+[\w]*)?@([\w-]+.)+[\w-]{2,4}$"></RegularExpression>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding: 10px;">
                                            <div class="HeaderContact">
                                                <label><font color="#da0004">*</font>First Name</label>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="ContentContact">
                                                <dx:ASPxTextBox ID="txtContactFirstName" ClientInstanceName="txtContactFirstName" ClientEnabled="true" runat="server" Width="200px" MaxLength="20"
                                                    AutoCompleteType="Disabled">
                                                    <ValidationSettings ValidationGroup="mandatory">
                                                        <RequiredField IsRequired="True" ErrorText="First Name is required"></RequiredField>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="HeaderContact">
                                                <label><font color="#da0004">*</font>Last Name</label>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="ContentContact">
                                                <dx:ASPxTextBox ID="txtContactLastName" ClientInstanceName="txtContactLastName" ClientEnabled="true" runat="server" Width="200px" MaxLength="20"
                                                    AutoCompleteType="Disabled">
                                                    <ValidationSettings ValidationGroup="mandatory">
                                                        <RequiredField IsRequired="True" ErrorText="Last Name is required"></RequiredField>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding: 10px;">
                                            <div class="HeaderContact">
                                                <label><font color="#da0004">*</font>Phone Number</label>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="ContentContact">
                                                <dx:ASPxTextBox ID="txtContactPhone" ClientInstanceName="txtContactPhone" ClientEnabled="true" runat="server" Width="200px" MaxLength="20"
                                                    AutoCompleteType="Disabled">
                                                    <ValidationSettings ValidationGroup="mandatory">
                                                        <RequiredField IsRequired="True" ErrorText="Phone Number is required"></RequiredField>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="HeaderContact">
                                                <label><font color="#da0004">*</font>Street address</label>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="ContentContact">
                                                <dx:ASPxTextBox ID="txtContactAddress" ClientInstanceName="txtContactAddress" ClientEnabled="true" runat="server" Width="200px" MaxLength="20"
                                                    AutoCompleteType="Disabled">
                                                    <ValidationSettings ValidationGroup="mandatory">
                                                        <RequiredField IsRequired="True" ErrorText="Address is required"></RequiredField>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>

                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding: 10px;">
                                            <div class="HeaderContact">
                                                <label><font color="#da0004">*</font>Town/City</label>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="ContentContact">
                                                <dx:ASPxTextBox ID="txtContactTown" ClientInstanceName="txtContactTown" ClientEnabled="true" runat="server" Width="200px" MaxLength="30"
                                                    AutoCompleteType="Disabled">
                                                    <ValidationSettings ValidationGroup="mandatory">
                                                        <RequiredField IsRequired="True" ErrorText="Town / city name is required"></RequiredField>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>

                                            </div>
                                        </td>
                                        <td></td>
                                        <td>
                                            <div class="ContentContact">
                                                <dx:ASPxTextBox ID="txtContactAddress2" ClientInstanceName="txtContactAddress2" ClientEnabled="true" runat="server" Width="200px" MaxLength="20"
                                                    AutoCompleteType="Disabled">
                                                    <ValidationSettings ValidationGroup="mandatory">
                                                        <RequiredField IsRequired="True" ErrorText="Address is required"></RequiredField>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </div>
                                        </td>

                                    </tr>

                                    <tr>
                                        <td>
                                            <div class="HeaderContact">
                                                <label><font color="#da0004">*</font>Country</label>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="ContentContact">

                                                <dx:ASPxComboBox ID="cmbContactCountryAddress" ClientInstanceName="cmbContactCountryAddress" ClientEnabled="true" runat="server" AllowMouseWheel="False" IncrementalFilteringMode="StartsWith"
                                                    FilterMinLength="1" AutoPostBack="true" OnSelectedIndexChanged="cmbContactCountryAddress_SelectedIndexChanged">
                                                </dx:ASPxComboBox>
                                                <dx:ASPxTextBox ID="txtContactCountryAddress" ClientVisible="false" ClientInstanceName="txtContactCountryAddress" ClientEnabled="true" runat="server"></dx:ASPxTextBox>

                                            </div>
                                        </td>
                                        <td style="padding: 10px;">
                                            <div class="HeaderContact">
                                                <label><font color="#da0004">*</font>State and zip code</label>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="ContentContact">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxComboBox ID="cmbContactState" ClientInstanceName="cmbContactState" ClientEnabled="true" runat="server" AllowMouseWheel="False" IncrementalFilteringMode="StartsWith" Width="100px"
                                                                FilterMinLength="1">
                                                                <ClientSideEvents SelectedIndexChanged="setValueToTextState" />
                                                            </dx:ASPxComboBox>
                                                            <dx:ASPxTextBox ID="txtContactState" ClientInstanceName="txtContactState" ClientVisible="false" ClientEnabled="true" runat="server"></dx:ASPxTextBox>
                                                        </td>
                                                        <td style="padding-left: 5px;">
                                                            <dx:ASPxTextBox ID="txtContactZipCode" ClientInstanceName="txtContactZipCode" ClientEnabled="true" runat="server" Width="50px" MaxLength="30">
                                                                <ValidationSettings ValidationGroup="mandatory">
                                                                    <RequiredField IsRequired="True" ErrorText="Zip Code is required"></RequiredField>
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </td>

                                                    </tr>
                                                </table>


                                            </div>
                                        </td>
                                    </tr>

                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 p-0">
                <dx:ASPxPageControl SkinID="None" Width="100%" EnableViewState="false" ID="TabControl" ClientInstanceName="TabControl"
                    runat="server" ActiveTabIndex="0" AutoPostBack="True"
                    TabSpacing="0px" ContentStyle-Border-BorderWidth="0" OnActiveTabChanged="TabControl_ActiveTabChanged"
                    EnableHierarchyRecreation="True">
                    <ClientSideEvents TabClick="ChangeTab" />
                    <TabPages>

                        <dx:TabPage Text="Credit Card/Debit Card" Name="TabCredit" Visible="true">
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl1" runat="server">
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>

                        <dx:TabPage Text="AG/BSP" Name="TabAG" Visible="true">
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
                    <TabStyle BackColor="White" Font-Bold="True"></TabStyle>
                    <ContentStyle BackColor="#E3E3E3">
                        <Paddings Padding="0px" PaddingLeft="0px" PaddingTop="0px" PaddingRight="0px" PaddingBottom="0px"></Paddings>
                        <border borderwidth="0px"></border>
                    </ContentStyle>
                </dx:ASPxPageControl>

            </div>
        </div>


        <%-- End of new design--%>

        <dx:ASPxValidationSummary ID="vsValidationSummary1" runat="server" Width="502px" ClientInstanceName="validationSummary" HorizontalAlign="Left" Height="16px">
            <ErrorStyle Wrap="False" />
            <Border BorderColor="Red" BorderStyle="Double" />
            <Border BorderColor="Red" BorderStyle="None"></Border>
        </dx:ASPxValidationSummary>

        <%--<asp:ScriptManager ID="ScriptManager" runat="server" AsyncPostBackTimeout="36000" />--%>
        <input type="hidden" id="hError" runat="server" value="" />
        <input type="hidden" id="tabActive" runat="server" value="" />
        <input type="hidden" id="hID" runat="server" value="" />
        <input type="hidden" id="hIsOverride" runat="server" value="" />
        <input type="hidden" id="hCarrierCode" runat="server" value="" />
        <%--<asp:UpdatePanel ID="UpdatePanel" runat="server">
            <ContentTemplate>--%>
        <msg:msgControl ID="msgcontrol" runat="server" />
        <div>
            <asp:Label runat="server" ID="lblErrorTop" ForeColor="Red" Visible="false"></asp:Label>
        </div>

        <%--        <div id="mainContentHeaderDiv">
            <div id="pageTitle">
            <h1>Review and pay</h1>
            <p>You've almost completed your booking. We recommend 
            that you double check your flight number, date, time, destination and 
            total amount due before you select the mode of payment that best suits 
            you.</p>
            <p style="color:Red">
                Fare quoted is only inclusive Tax and Booking Fee.  For Add Ons, kindly contact Group Desk
            </p>
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
        </tr>
        </tbody></table>
        <br />--%>
        <table class="priceDisplay" style="display: none">
            <tbody>
                <tr>
                    <td><strong>Transaction ID</strong></td>
                    <td class="amountdesc"></td>
                    <td class="priceamountmain">
                        <asp:Label ID="lblTransactionID" runat="server" Text="0"></asp:Label>
                    </td>
                </tr>
                <tr id="trDepart" runat="server">
                    <td></td>
                </tr>
                <tr id="trReturn" runat="server">
                    <td></td>
                </tr>
            </tbody>
        </table>

        <%--            <td>
                <strong>Depart Total</strong></td>
            <td class="amountdesc">
            </td>
            <td class="priceamountmain">--%>
        <%--                <asp:Label ID="lblDepartTotal" runat="server" Text="0"></asp:Label>
                &nbsp;
                <asp:Label ID="lblDepartTotalCurrency" runat="server" Text="MYR"></asp:Label>--%>
        <%--            </td>
            </tr>
            <tr id="trReturn" runat="server">
                <td>
                    <strong>Return Total</strong></td>
                <td class="amountdesc">
                </td>
                <td class="priceamountmain">--%>
        <%--                    <asp:Label ID="lblReturnTotal" runat="server" Text="0"></asp:Label>
                    &nbsp;
                    <asp:Label ID="lblReturnTotalCurrency" runat="server" Text="MYR"></asp:Label>--%>
        <%--                </td>
            </tr>
        </tbody></table>
        <table class="priceDisplay">
        <tbody><tr>
        <td><strong>Current Total</strong></td>
        <td></td>
        <td class="priceamountmain">--%>
        <%--            <asp:Label ID="lblCurrentTotal" runat="server" Text="0" ForeColor="Red"></asp:Label>&nbsp;
            <asp:Label ID="lblCurrentTotalCurrency" runat="server" Text="MYR" ForeColor="Red"></asp:Label>--%>
        <%--        </td>
        </tr>
        </tbody></table>
        <br />
        <table class="priceDisplay" border="0" cellpadding="0" cellspacing="0" width="100%">
        <tbody>
        <tr>
        <td><strong>Total Deposit</strong></td>
        <td class="amountdesc"></td>
        <td class="priceamountmain">        --%>
        <%--            <asp:Label ID="lblAmountPaid" runat="server" Text="0"></asp:Label>&nbsp;
            <asp:Label ID="lblAmountPaidCurrency" runat="server" Text="MYR"></asp:Label>--%>
        <%--        </td>
        </tr>

        <tr>
        <td><strong>Amount Due</strong></td>
        <td class="amountdesc"></td>
        <td class="priceamountmain">        --%>
        <%--            <asp:Label ID="lblAmountDue" runat="server" Text="0" ForeColor="Red"></asp:Label>&nbsp;
            <asp:Label ID="lblAmountDueCurrency" runat="server" Text="MYR" ForeColor="Red"></asp:Label>--%>
        <%--        </td>
        </tr>
        
        </tbody>
        </table>
        </td>
        </tr>
        </table>
        <br />--%>
        <!-- added by diana 20130902 -->
        <style type="text/css">
            .tdClass {
                font-family: Arial,Helvetica,san-serif;
                font-size: 12px;
            }
        </style>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" id="tblPayment" runat="server" style="visibility: hidden">
            <tbody>
                <%--        <tr>
            <td colspan="3"><strong>Select Booking PNR to Pay</strong><br />
            <div id="divPNR" runat="server" width="100%" style="margin-right:0px;padding-right:0px;">
                <dx:ASPxRadioButtonList ID="rdlPNR" ClientInstanceName="rdlPNR" runat="server" RepeatDirection="Vertical" 
                    Border-BorderStyle="None" RepeatColumns="1" RepeatLayout="Table" Paddings-Padding="0" 
                    CssClass="tdClass" Width="740px" >
                    <ClientSideEvents SelectedIndexChanged="setMinimumPayment" />
                    <ValidationSettings ValidationGroup="mandatory" ErrorTextPosition="Top">
                        <RequiredField ErrorText="Please select booking PNR to pay" IsRequired="true" />
                    </ValidationSettings>
                </dx:ASPxRadioButtonList>
            </div>
            
            <div id="divContactPNR" runat="server" width="100%" style="margin-right:0px;padding-right:0px;">
                <dx:ASPxRadioButtonList ID="rdlContactPNR" ClientVisible="false" ClientInstanceName="rdlContactPNR" runat="server" RepeatDirection="Vertical" 
                    Border-BorderStyle="None" RepeatColumns="1" RepeatLayout="Table" Paddings-Padding="0" 
                    CssClass="tdClass" Width="740px">

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
                        <ValidationSettings ValidationGroup="mandatory" >
                        <RequiredField ErrorText=" " IsRequired="True" />
                        <RegularExpression ErrorText="Invalid payment amount" ValidationExpression="^([\d.,*]{1,20})$"></RegularExpression>                                               
                <RequiredField IsRequired="True" ErrorText="Payment amount is required"></RequiredField>
                   </ValidationSettings>
                </dx:ASPxTextBox></td>
        
        </tr>--%>
            </tbody>
        </table>
    </div>

    <!-- Payment Methods -->
    <div id="selectMainBody" class="mainBody">
        <div style="width: 100%" id="divMsg">
            <table>
                <tr>
                    <td valign="top">&nbsp;
        <asp:Label runat="server" ID="imgError" ForeColor="Red" Visible="false"><img src='../images/AKBase/icon-error.gif' width='15px'/></asp:Label>
                        &nbsp;</td>
                    <td>
                        <asp:Label runat="server" ID="lblErrorBottom" ForeColor="Red" Visible="true" Width="720px"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div style="float: left; width: 100%; margin-right: 2%" class="col-sm-12 m-0 pt-0">

            <div runat="server" id="divContact" visible="true" style="background-color: #E3E3E3">

                <%--        <div class="paymentTabNote">
        <h3><font color="red" size="+1">Contact Details</font></h3>
        <p class="paymentTabNote" style="font-family:Arial,Helvetica,san-serif;font-size:11px;padding:0px;margin:10px 0px;">
        Please fill up your contact details.
        </p>
        <table width="100%" class="tblData">
            <tr>
                <td style="width:300px;">
                    *Title
                </td>
                <td style="width: 300px">--%>
                <%--                    <dx:ASPxComboBox runat="server" ID="cmbContactTitle" IncrementalFilteringMode="StartsWith"
                         ClientEnabled="true" ClientInstanceName="cmbContactTitle"
                        AllowMouseWheel="False" FilterMinLength="1" SelectedIndex="0">
                        <Items>
                            <dx:ListEditItem Text="MR" Value="MR" Selected="True" />
                            <dx:ListEditItem Text="MS" Value="MS" />
                        </Items>
                    </dx:ASPxComboBox>--%>
                <%--                </td>
                <td>     
                    &nbsp;
                </td> 
            </tr>
            <tr>
                <td style="width:300px;">
                    *First Name
                </td>
                <td style="width: 300px">--%>
                <%--                    <dx:ASPxTextBox ID="txtContactFirstName" ClientInstanceName="txtContactFirstName" ClientEnabled="true" runat="server" Width="200px" MaxLength="20" 
                        AutoCompleteType="Disabled">
                    <ValidationSettings ValidationGroup="mandatory">
                        <RequiredField IsRequired="True" ErrorText="First Name is required"></RequiredField>
                    </ValidationSettings>
                    </dx:ASPxTextBox>--%>
                <%--                </td>
                <td>     
                    &nbsp;
                </td> 
            </tr>
            <tr>
                <td style="width:300px;">
                    *Last Name
                </td>
                <td style="width: 300px">--%>
                <%--                    <dx:ASPxTextBox ID="txtContactLastName" ClientInstanceName="txtContactLastName" ClientEnabled="true" runat="server" Width="200px" MaxLength="20" 
                        AutoCompleteType="Disabled">
                    <ValidationSettings ValidationGroup="mandatory">
                        <RequiredField IsRequired="True" ErrorText="Last Name is required"></RequiredField>
                    </ValidationSettings>
                    </dx:ASPxTextBox>--%>
                <%--                </td>
                <td>     
                    &nbsp;
                </td> 
            </tr>
            <tr>
                <td style="width:300px;">
                    *Email
                </td>
                <td style="width: 300px">--%>
                <%--                    <dx:ASPxTextBox ID="txtContactEmail" ClientInstanceName="txtContactEmail" ClientEnabled="true" runat="server" Width="200px" MaxLength="50" 
                        AutoCompleteType="Disabled">
                    <ValidationSettings ValidationGroup="mandatory">
                        <RequiredField IsRequired="True" ErrorText="Email is required"></RequiredField>
                        <RegularExpression ErrorText="Email is invalid" ValidationExpression="^[\w-.]+(?:\+[\w]*)?@([\w-]+.)+[\w-]{2,4}$"></RegularExpression>
                    </ValidationSettings>
                    </dx:ASPxTextBox>--%>
                <%--                </td>
                <td>     
                    &nbsp;
                </td> 
            </tr>
            <tr>
                <td style="width:300px;">
                    *Phone Number
                </td>
                <td style="width: 300px">--%>
                <%--                    <dx:ASPxTextBox ID="txtContactPhone" ClientInstanceName="txtContactPhone" ClientEnabled="true" runat="server" Width="200px" MaxLength="20" 
                        AutoCompleteType="Disabled">
                    <ValidationSettings ValidationGroup="mandatory">
                        <RequiredField IsRequired="True" ErrorText="Phone Number is required"></RequiredField>
                    </ValidationSettings>
                    </dx:ASPxTextBox>--%>
                <%--                </td>
                <td>     
                    &nbsp;
                </td> 
            </tr>
            <tr>
                <td style="width:300px;">
                    *Street address
                </td>
                <td style="width: 300px">--%>
                <%--                    <dx:ASPxTextBox ID="txtContactAddress" ClientInstanceName="txtContactAddress" ClientEnabled="true" runat="server" Width="200px" MaxLength="20" 
                        AutoCompleteType="Disabled">
                    <ValidationSettings ValidationGroup="mandatory">
                        <RequiredField IsRequired="True" ErrorText="Address is required"></RequiredField>
                    </ValidationSettings>
                    </dx:ASPxTextBox>--%>
                <%--                </td>
                <td>     
                    &nbsp;
                </td> 
            </tr>
            <tr>
                <td>
                    *Town/City
                </td>
                <td style="width: 300px">--%>
                <%--                    <dx:ASPxTextBox ID="txtContactTown" ClientInstanceName="txtContactTown" ClientEnabled="true" runat="server" Width="200px" MaxLength="30" 
                        AutoCompleteType="Disabled">
                        <ValidationSettings ValidationGroup="mandatory">
                            <RequiredField IsRequired="True" ErrorText="Town / city name is required"></RequiredField>
                        </ValidationSettings>
                    </dx:ASPxTextBox>--%>
                <%--                </td>
                <td>&nbsp;</td>
            </tr>
            
            <tr>
                <td style="width: 300px; height: 19px">
                    *Country
                </td>
                <td >
                <table>
                        <tr>
                            <td class="insideClass">--%>
                <%--<dx:ASPxComboBox ID="cmbContactCountryAddress" ClientInstanceName="cmbContactCountryAddress" ClientEnabled="true" runat="server" AllowMouseWheel="False" IncrementalFilteringMode="StartsWith"
                    FilterMinLength="1" AutoPostBack="true"
                        onselectedindexchanged="cmbContactCountryAddress_SelectedIndexChanged">
                </dx:ASPxComboBox>
                                <dx:ASPxTextBox ID="txtContactCountryAddress" ClientVisible="false" ClientInstanceName="txtContactCountryAddress" ClientEnabled="true" runat="server"></dx:ASPxTextBox>--%>
                <%--                </td>
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
                            <td class="insideClass">--%>
                <%--                                <dx:ASPxComboBox ID="cmbContactState" ClientInstanceName="cmbContactState" ClientEnabled="true" runat="server" AllowMouseWheel="False" IncrementalFilteringMode="StartsWith" Width="100px"
                                    FilterMinLength="1">
                                    <ClientSideEvents SelectedIndexChanged="setValueToTextState" />
                                </dx:ASPxComboBox>
                                <dx:ASPxTextBox ID="txtContactState" ClientInstanceName="txtContactState" ClientVisible="false" ClientEnabled="true" runat="server"></dx:ASPxTextBox>--%>
                <%--                            </td>
                            <td>--%>
                <%--                               <dx:ASPxTextBox ID="txtContactZipCode" ClientInstanceName="txtContactZipCode" ClientEnabled="true" runat="server" Width="50px" MaxLength="30">
                                    <ValidationSettings ValidationGroup="mandatory">
                                        <RequiredField IsRequired="True" ErrorText="Zip Code is required"></RequiredField>
                                    </ValidationSettings>
                                </dx:ASPxTextBox>                                                       --%>
                <%--                           </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
            </tr>
        </table>
        </div>   
                --%>
            </div>

            <br />
            <%--    <dx:ASPxPageControl SkinID="None" Width="100%" EnableViewState="false" ID="TabControl" ClientInstanceName="TabControl"
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
    </dx:ASPxPageControl>--%>


            <!-- End Payment Methods Tabs-->
            <!-- Payment Methods Div-->

            <div runat="server" id="divCreditCard" visible="true" style="background-color: #E3E3E3">

                <div class="paymentTabNote">
                    <table width="100%" class="tblData">
                        <tr>
                            <td colspan="3">
                                <asp:Panel ID="panelProcessFee" runat="server" Visible="true">
                                    <div>Please be aware that processing fees would be added to Your credit card payment.</div>
                                    <asp:Label runat="Server" ID="lblTextProcessing" Text="Total Processing Fee" Font-Underline="true">
                                    </asp:Label>&nbsp;:&nbsp;<dx:ASPxLabel runat="server" ID="lblProcessFee" ClientInstanceName="lblProcessFee"></dx:ASPxLabel>
                                    &nbsp;
                    <asp:Label runat="server" ID="lblCurrencyProcessFee" Text=""></asp:Label>
                                </asp:Panel>
                            </td>
                        </tr>
                        <%--<tr>
               <td>
                   <asp:Label runat="Server" ID="Label1" Text="Total Amount Pay" Font-Underline="true"></asp:Label>
                   <dx:ASPxLabel runat="server" ID="lblTotalAmountPay" ClientInstanceName="lblTotalAmountPay"></dx:ASPxLabel> &nbsp;
                    <asp:Label runat="server" ID="lblCurrTotalAmountPay" Text = ""></asp:Label>
               </td>
           </tr>--%>
                        <tr>
                            <td colspan="3" align="center">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 300px;">
                                <span class="requiredStar">*</span>Card Type
                            </td>
                            <td style="width: 300px">
                                <table>
                                    <tr>
                                        <!-- OnSelectedIndexChanged="cmbCardType_SelectedIndexChanged" -->
                                        <td class="insideClass">
                                            <dx:ASPxComboBox runat="server" ID="cmbCardType" IncrementalFilteringMode="StartsWith"
                                                ClientEnabled="true" ClientInstanceName="cmbCardType"
                                                AllowMouseWheel="False" FilterMinLength="1" SelectedIndex="0">
                                                <Items>
                                                    <%--<dx:ListEditItem Text="American Express" Value="AX" Selected="True" />--%>
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
                                <span class="requiredStar">*</span>Card Number
                            </td>
                            <td style="width: 300px">
                                <dx:ASPxTextBox ID="txtCardNumber" runat="server" Width="200px" MaxLength="20"
                                    AutoCompleteType="Disabled">
                                    <ValidationSettings ValidationGroup="cc">
                                        <RequiredField ErrorText="Card number is required" IsRequired="True" />
                                        <RegularExpression ErrorText="Invalid Card number" ValidationExpression="^([\d*]{1,20})$"></RegularExpression>
                                        <RequiredField IsRequired="True" ErrorText="Card number is required"></RequiredField>
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <span class="requiredStar">*</span>Card Holder Name
                            </td>
                            <td style="width: 300px">
                                <dx:ASPxTextBox ID="txtCardHolderName" runat="server" Width="200px"
                                    MaxLength="30" AutoCompleteType="Disabled">
                                    <ValidationSettings ValidationGroup="cc">
                                        <RegularExpression ErrorText="Invalid card holder name" ValidationExpression="^([1-zA-Z0-1@.\s]{1,200})$"></RegularExpression>
                                        <RequiredField IsRequired="True" ErrorText="Card holder name is required"></RequiredField>
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <span class="requiredStar">*</span>Expiration Date
                            </td>
                            <td style="width: 300px">
                                <table>
                                    <tr>
                                        <td class="insideClass">
                                            <dx:ASPxComboBox ID="cmbExpiryMonth" IncrementalFilteringMode="StartsWith"
                                                runat="server" Width="40" SelectedIndex="0" AllowMouseWheel="False" FilterMinLength="1" CssClass="compClass">
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
                                            <dx:ASPxComboBox ID="cmbExpiryYear" Width="60px" runat="server" IncrementalFilteringMode="StartsWith"
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
                                <span class="requiredStar">*</span>CVV/CID Number
                            </td>
                            <td style="width: 300px">
                                <dx:ASPxTextBox ID="txtCVV2" ClientInstanceName="txtCVV2" runat="server" Width="50px" MaxLength="3"
                                    AutoCompleteType="Disabled">
                                    <ValidationSettings ValidationGroup="cc">
                                        <RequiredField IsRequired="True" ErrorText="CVV number is required"></RequiredField>
                                        <RegularExpression ErrorText="Invalid CVV number" ValidationExpression="^([\d*]{1,20})$"></RegularExpression>
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <span class="requiredStar">*</span>Card Issuer
                            </td>
                            <td style="width: 300px">
                                <dx:ASPxTextBox ID="txtCardIssuer" runat="server" Width="200px" MaxLength="30"
                                    AutoCompleteType="Disabled">
                                    <ValidationSettings ValidationGroup="cc">
                                        <RequiredField IsRequired="True" ErrorText="Card issuer name is required"></RequiredField>
                                        <RegularExpression ErrorText="Invalid card issuer name" ValidationExpression="^([1-zA-Z0-1@.\s]{1,200})$"></RegularExpression>
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <span class="requiredStar">*</span>Card Issuing Country
                            </td>
                            <td style="width: 300px;">
                                <table>
                                    <tr>
                                        <td class="insideClass">
                                            <dx:ASPxComboBox ID="cmbIssuingCountry" runat="server" AllowMouseWheel="False" IncrementalFilteringMode="StartsWith"
                                                FilterMinLength="1" CssClass="compClass" AutoPostBack="false">
                                            </dx:ASPxComboBox>

                                        </td>
                            </td>
                        </tr>
                    </table>
                    <td>&nbsp;</td>
                    </tr>
        </table>
        <p class="paymentTabNote" style="font-family: Arial,Helvetica,san-serif; font-size: 11px; padding: 0px; margin: 10px 0px;">
            <b>Note: </b>Expired or unsupported bank cards will not be listed during payment process.<!-- <br>E-Gift Voucher (EGV) must be redeemed before any other payment methods; any remaining balance has to be paid by credit card only. -->
        </p>
                </div>


                <div class="paymentTabNote">
                    <p class="paymentTabNote" style="font-family: Arial,Helvetica,san-serif !important; font-size: 13px; padding: 0px; margin: 10px 0px;">
                        This address must be identical to your credit card billing address. Please enter the first 20 characters of your billing address.
                    </p>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <table id="updatePanelTbl" style="width: 664px; margin-bottom: 20px;" class="table tableNoBorder">

                                <tr>
                                    <td>
                                        <span class="requiredStar">*</span>Street address
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtAddress" runat="server" Width="200px" MaxLength="20"
                                            AutoCompleteType="Disabled">
                                            <ValidationSettings ValidationGroup="cc">
                                                <RequiredField IsRequired="True" ErrorText="Address is required"></RequiredField>
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <span class="requiredStar">*</span>Town/City
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="txtTown" runat="server" Width="200px" MaxLength="30"
                                            AutoCompleteType="Disabled">
                                            <ValidationSettings ValidationGroup="cc">
                                                <RequiredField IsRequired="True" ErrorText="Town / city name is required"></RequiredField>
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>

                                </tr>

                                <tr>
                                    <td>
                                        <span class="requiredStar">*</span>Country
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td class="insideClass">
                                                    <dx:ASPxComboBox ID="cmbCountryAddress" runat="server" AllowMouseWheel="False" IncrementalFilteringMode="StartsWith"
                                                        FilterMinLength="1" AutoPostBack="true"
                                                        OnSelectedIndexChanged="cmbCountryAddress_SelectedIndexChanged">
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <span class="requiredStar">*</span>State and zip code
                                    </td>
                                    <td>
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

                                </tr>

                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <p class="creditCardNote" style="font-family: Arial,Helvetica,san-serif !important; font-size: 13px; padding: 0px; margin: 10px 0px;">
                        Please be advised that credit card issuing banks may impose additional charges on all CROSS BORDER transactions. CROSS BORDER transactions are defined as transactions whereby the country of the cardholder's bank differs from that of the merchant.
        <br />
                        <br />
                        Please note that the additional charge is not imposed by AirAsia and neither do we benefit from it. You are advised to seek further clarification from your credit card issuing bank should a CROSS BORDER charge be applied to this transaction.
                    </p>
                </div>

                <br />
            </div>


            <!-- End Payment Method DIV-->

            <!--AG Payment DIV-->
            <div runat="server" id="divAG" visible="false" style="background-color: #E3E3E3">
                <div class="paymentTabNote" style="font-family: Arial,Helvetica,san-serif; font-size: 12px;">
                    <table width="100%">
                        <tr>
                            <td style="width: 300px;">Available AG Credit&nbsp;
                            </td>

                            <td>
                                <dx:ASPxLabel ID="lblAGCreditAmount" runat="server" MaxLength="30"></dx:ASPxLabel>
                                <asp:Label ID="lblAGCreditCurrency" runat="server" MaxLength="30"></asp:Label>
                            </td>
                        </tr>

                    </table>
                    <p class="paymentTabNote" style="font-family: Arial,Helvetica,san-serif; font-size: 11px; padding: 0px; margin: 10px 0px;"><b>Note: </b>Expired or unsupported bank cards will not be listed during payment process.<!-- <br>E-Gift Voucher (EGV) must be redeemed before any other payment methods; any remaining balance has to be paid by credit card only. --></p>
                </div>
                <br />
            </div>


            <br />

            <div>

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

            <div class="row page-header clearfix">
                <div class="col-sm-6">
                </div>
                <div class="col-sm-6">
                    <div style="float: right">
                        <dx:ASPxButton ID="btnProceedPaymentBottom" runat="server" ClientInstanceName="btnProceedPaymentBottom" CssClass="buttonL2" Text="Proceed Payment" OnClick="btnProceedPayment_Click">
                            <ClientSideEvents Click="function(s, e) { clearTimeout(varTmr); LoadingPanel.SetText(&#39;Please Wait...&#39;);
                                                                   LoadingPanel.Show(); SubmitPayment(); }"></ClientSideEvents>
                        </dx:ASPxButton>
                    </div>
                </div>
            </div>

        </div>
    </div>



    <%--</ContentTemplate>

        </asp:UpdatePanel>--%>
    <%--<asp:UpdateProgress runat="server" ID="UpdateProgress" AssociatedUpdatePanelID="UpdatePanel" DisplayAfter="0" DynamicLayout="true">
            <ProgressTemplate>
                <div class="overlay2"></div>
                <div class="overlayContent2">

                    <img alt="In progress..." src="../Images/Airasia/loading_circle.gif" />

                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>--%>
    </div>

    <asp:HiddenField ID="hfAmountDue" runat="server" />

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

    <fd:flightdetail ID="flightdetail" runat="server" />

    <%--<div class="formWrapperLeftAA" style="max-height:1200px">   
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

                                <tr runat="server" id="trReturnIcon">
                                    <td style="padding-bottom: 3px;" valign="top">
                                        <div class="" style="text-align:left;"><span class="returnFlight fa fa-plane"></span></div>
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

    <%--<div id="atAGlanceRightContent" style="display:none">
        <div id="FlightDisplayHeader" class="atAGlanceDivHeader"><span>Flight details</span></div>
        <div class="atAGlanceDivBody flightDisplayBody flightDisplayContainer">
        <div class="jsdiv"></div>
        <table>                
        <tr><td><strong>Depart</strong></td></tr>
        <tr>
        <td>
        <strong><!--<asp:Label ID="lblDepartOrigin" runat="server" ></asp:Label>--></strong>
        &nbsp;to&nbsp;
        <strong><!--<asp:Label ID="lblDepartDestination" runat="server" ></asp:Label>--></strong>
        </td>
        </tr>
        <tr>
        <td>
            <!--<asp:Label ID="lblDateDepart" runat="server" ></asp:Label>-->
        </td>
        </tr>
        <tr>
        <td>
            <strong>Depart </strong><!--<asp:Label ID="lblDepartStd" runat="server" >--></asp:Label> 
            &nbsp; &nbsp; &nbsp; <strong>Arrive</strong>
            <!--<asp:Label ID="lblDepartSta" runat="server" >--></asp:Label>                
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
            <strong><!--<asp:Label ID="lblReturnOrigin" runat="server" ></asp:Label>--></strong>
            &nbsp;to&nbsp;
            <!--Add by Riska BS, 20130115--><!--<strong><asp:Label ID="lblReturnDestination" runat="server" ></asp:Label>--></strong>                
        </td>
        </tr>
        <tr>
        <td>
            <!--<asp:Label ID="lblDateReturn" runat="server"></asp:Label>-->                    
        </td>
        </tr>
        <tr>
        <td>
            <strong>Depart </strong> <!--<asp:Label ID="lblReturnStd" runat="server" >--></asp:Label> 
            &nbsp; &nbsp; &nbsp; <strong>Arrive </strong> <!--<asp:Label ID="lblReturnSta" runat="server">--></asp:Label>                 
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
    </div>--%>

    <script type="text/javascript">

        //document.getElementById("tblPayment").style.display = "";
        //document.getElementById("<%=rdlPNR.ClientID %>").style.display = "";
        //document.getElementById("<%=txtPayAmount.ClientID %>").style.display = "";

    </script>
</asp:Content>
