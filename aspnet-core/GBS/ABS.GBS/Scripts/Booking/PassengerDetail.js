// <![CDATA[
var keyValue;
var isConfirm = "";
var confirmClick = false;
var int = 0;
function OnEdit(element, key) {
    callbackPanel.SetContentHtml("");
    popup.ShowAtElement(element);
    keyValue = key;
}
function popup_Shown(s, e) {
    callbackPanel.PerformCallback(keyValue);
}

function ActionPanelEndCallBack() {
    btConfirm.SetVisible(true);
    LoadingPanel.Hide();
}

function OnEndCallBackEctionDataPanel() {
    var msg = document.getElementById("ctl00_ContentPlaceHolder2_ActionDataPanel_hfMessage").value;
    if (msg != "") {
        var val = msg.split("|");
        if (val[0] == 'download') {
            window.location = '<%= ResolveUrl("~/Download.aspx?f=' + val[1] + '") %>';
            //FileDownloader(document.getElementById('downloadLink'), '<%= ResolveUrl("~/Temp/' + val[1] + '") %>', val[1]);
        }
    }
}

    function Uploader_OnUploadStart() {
        btnUpload.SetEnabled(false);
        LoadingPanel.Show();
    }
    function Uploader_OnFileUploadComplete(s, e) {
        if (e.errorText != "") {
            //ShowMessage(e.errorText);
            document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = e.errorText;
            pcMessage.Show();
            LoadingPanel.Hide();

        }
        else if (e.callbackData == "success") {
            //ShowMessage("File uploading has been successfully completed.");
            document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = "File uploading has been successfully completed.";
            pcMessage.Show();
            LoadingPanel.Hide();
            ActionDataPanel.SetVisible(false);
            gvPassenger.Refresh();
            if (typeof gvInfant !== "undefined") {
                gvInfant.Refresh();
            }
        }
        else if (e.callbackData == "incorrect") {
            //ShowMessage("File uploading has been successfully completed.");
            document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = "Incorrect or Invalid Excel value, please check your value.";
            pcMessage.Show();
            LoadingPanel.Hide();
            btConfirm.SetEnabled(false);
            gvPassenger.Refresh();
            ActionDataPanel.PerformCallback();
            ActionDataPanel.SetVisible(true);

            btnDl.SetVisible(false);
        }
    }
    function ShowMessage(message) {
        //window.setTimeout("alert('" + message + "')", 0);
        alert(message);
    }
    function Uploader_OnFilesUploadComplete(args) {
        UpdateUploadButton();
        LoadingPanel.Hide();
    }
    function UpdateUploadButton() {
        btnUpload.SetEnabled(uploader.GetText(0) != "");
    }
    // added by diana 20130909
    function changeGenderProperty() {
        if (cmbTitle.GetSelectedItem().value == "Mr") {
            cmbGender.SetText("Male");
        }
        else if (cmbTitle.GetSelectedItem().value == "Ms") {
            cmbGender.SetText("Female");
        }
        if (cmbTitle.GetSelectedItem().value == "Chd") {
            txtDOB.SetMinDate(txtChd.GetMinDate());
            txtDOB.SetMaxDate(txtChd.GetMaxDate());
        }
        else {
            txtDOB.SetMinDate(txtAdt.GetMinDate());
            txtDOB.SetMaxDate(txtAdt.GetMaxDate());
        }
    }
    function changeTitleProperty() {
        if (cmbGender.GetSelectedItem().value == "Male") {
            if (cmbTitle.GetSelectedItem().value == "Ms") {
                cmbTitle.SetText("Mr");
            }
        }
        else if (cmbGender.GetSelectedItem().value == "Female") {
            if (cmbTitle.GetSelectedItem().value == "Mr") {
                cmbTitle.SetText("Ms");
            }
        }
    }
    //        function getPreviewImageElement() {
    //            return document.getElementById("previewImage");
    //        }
    // ]]>
    function onBatchEditBeginCallback(s, e)
    {
        ActionDataPanel.SetVisible(false);
        //btnDl.SetVisible(true);
        btConfirm.SetEnabled(true);
    }

    function onEditGrid(s, e) {
        gvPassenger.UpdateEdit();
    }

    function onCancelGrid(s, e) {
        isValid = true;
        gvPassenger.CancelEdit();
    }

    var LastColumnName;
    function OnBatchStartEdit(s, e) {
    
   
    }

    function OnBatchStartEditInfant(s, e) {
        btConfirm.SetVisible(true);
    }

    var isValid = true;
    function OnBatchEditRowValidating(s, e) {
        isValid = true;
        for (var entry in e.validationInfo)
            if (!e.validationInfo[entry].isValid) {
                isValid = false;
                break;
            }
    }

    var isValid = true;
    function OnBatchEditRowValidatingInfant(s, e) {
        isValid = true;
        for (var entry in e.validationInfo)
            if (!e.validationInfo[entry].isValid) {
                isValid = false;
                break;
            }
    }
    function OnDownloadclicked(s, e) {
        LoadingPanel.Show();
        gvPassenger.PerformCallback('download');
    }

    function CompleteFunction() {
           
    
        if (gvPassenger.batchEditApi.HasChanges()) {

            gvPassenger.UpdateEdit();
            //gvInfant.UpdateEdit();
            isConfirm = "confirm";
        }
        else {

            gvPassenger.PerformCallback("confirm");

            isConfirm = "";
        }
           
    }


    function OnBtnConfirmClick() {
        if (isValid == true) {
            if (typeof gvInfant !== "undefined") {
                if (gvInfant.GetVisibleRowsOnPage() > 0) {
                    if (gvInfant.batchEditApi.HasChanges()) {
                    
                        setTimeout(CompleteFunction, 5000);
                        gvInfant.UpdateEdit();
                    }
                    else {
                        if (gvPassenger.batchEditApi.HasChanges()) {
                            gvPassenger.UpdateEdit();
                            isConfirm = "confirm";
                        }
                        else {
                            gvPassenger.PerformCallback("confirm");
                            isConfirm = "";
                        }
                    }
                }
                else {
                    if (gvPassenger.batchEditApi.HasChanges()) {
                        gvPassenger.UpdateEdit();
                        isConfirm = "confirm";
                    }
                    else {
                        gvPassenger.PerformCallback("confirm");
                        isConfirm = "";
                    }
                }
            }
            else {

                if (gvPassenger.batchEditApi.HasChanges()) {
                    gvPassenger.UpdateEdit();
                    isConfirm = "confirm";
                }
                else {
                    gvPassenger.PerformCallback("confirm");
                    isConfirm = "";
                }
            }

        }
        else if (document.getElementById('ctl00_ContentPlaceHolder2_hCommand').value != "") {
            document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = document.getElementById('ctl00_ContentPlaceHolder2_hCommand').value;
            pcMessage.Show();
            LoadingPanel.Hide();
            //document.getElementById('ctl00_ContentPlaceHolder2_hCommand').value = "";
        }
        else {
            document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = "Kindly complete all required data in order to continue";
            pcMessage.Show();
            LoadingPanel.Hide();
            //alert("Kindly complete all required data in order to continue");
        }
        confirmClick = true;
    }

    //document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_btCancel_CD").onclick = function() {
    //    alert("button was clicked");
    //}​

    var k = getParameterByName('k');
    var TransID = getParameterByName('TransID');

    function onBatchEditEndEditing(s, e) {
        if (isConfirm == "confirm") {
            if (s.cp_result != "" && typeof s.cp_result != "undefined") {
                document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = s.cp_result;
                pcMessage.Show();
                LoadingPanel.Hide();
                s.cp_result = "";
            }
            else {
                gvPassenger.PerformCallback("confirm");
                isConfirm = "";
            }
        }
        if (s.cp_result != "" && typeof s.cp_result != "undefined") {
            if (s.cp_result == "Passenger details has been successfully confirmed.") {
                document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = s.cp_result;
                pcMessage.Show();
                LoadingPanel.Hide();
                s.cp_result = "";
                function changeState(event) {
                    LoadingPanel.Show();
                    window.location.href = '../public/BookingDetail.aspx?k=' + k + '&TransID=' + TransID;
                    LoadingPanel.Hide();
                }
                // Event handlers for when we click on a button
                document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_btCancel_CD").addEventListener("click", changeState, false);
            }
            else if (s.cp_result.indexOf("download|") != -1) {
                LoadingPanel.Hide();
                var msg = s.cp_result;
                s.cp_result = "";
                if (msg != "") {
                    var val = msg.split("|");
                    if (val[0] == 'download') {

                        window.location = '../Download.aspx?f=' + val[1];
                        
                        //FileDownloader(document.getElementById('downloadLink'), '<%= ResolveUrl("~/Temp/' + val[1] + '") %>', val[1]);
                    }
                    
                }
            }
            else {
                document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = s.cp_result;
                pcMessage.Show();
                LoadingPanel.Hide();
                s.cp_result = "";
            }
        }
        
    
            //alert(document.getElementById("ctl00_ContentPlaceHolder2_hfConfirm").value);
            if (confirmClick == false && int == 1) {
                ActionPanel.PerformCallback();
                int = 0;
        
            }
            else {
                //confirmClick = false;
            }
        }

        function BatchEditEndEditing(s, e) {
            int = 1;
            //window.setTimeout(function () {
            //    alert(e.visibleIndex);
            //    var ChangeCnt = s.batchEditApi.GetCellValue(e.visibleIndex, "FirstName");
            //    alert(ChangeCnt);
            //    //s.batchEditApi.SetCellValue(e.visibleIndex, "ChangeCnt", ChangeCnt + "x", null, true);
            //    alert("a");
            //    gvPassenger.GetRowValues(e.visibleIndex, 'ChangeCnt', OnGetRowValues);
            //}, 10);
        }

        function OnGetRowValues(values) {
            gvPassenger.SetEditValue("ChangeCnt", "2x");
            //alert(tb);
            //var newItem = values;
            //tb.SetValue(newItem.GetColumnText("ProductPrice"));
            //gvPassenger.SetCellValue(gvPassenger.GetFocusedRowIndex(), "ChangeCnt", ChangeCnt + "x", null, true);
            //SSRActionPanel.PerformCallback();
        }

        function getParameterByName(name, url) {
            if (!url) {
                url = window.location.href;
            }
            name = name.replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        }



        function onBatchEditEndEditingInfant(s, e) {
            if (isConfirm == "confirm") {
                gvInfant.PerformCallback("confirm");
                isConfirm = "";
            }
            if (s.cp_result != "" && typeof s.cp_result != "undefined") {
       
                document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = s.cp_result;
                pcMessage.Show();
                LoadingPanel.Hide();
                s.cp_result = "";
            }
        }