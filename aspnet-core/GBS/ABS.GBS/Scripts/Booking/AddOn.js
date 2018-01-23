// <![CDATA[
function gvPassenger_EndCallback(s, e) {
    if (gvPassenger.cp_result != "" && typeof gvPassenger.cp_result != "undefined" && gvPassenger.cp_result != "Infant") {
        document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = gvPassenger.cp_result;
        pcMessage.Show();
        LoadingPanel.Hide();
        if (gvPassenger.cp_result.indexOf("Maximum Infant") != -1)
        {
            SSRTab1Panel.PerformCallback();
            SSRActionPanel.PerformCallback();
            gvPassenger2.Refresh();
        }
        gvPassenger.cp_result = "";
    }
    else {
        if (paramCallBack == "Baggage") {
            AddBaggage();
        }
        else if (paramCallBack == "Meal") {
            AddMeal();
        }
        else if (paramCallBack == "Meal1") {
            AddMeal1();
        }
        else if (paramCallBack == "Drink") {
            AddDrink();
        }
        else if (paramCallBack == "Drink1") {
            AddDrink1();
        }
        else if (paramCallBack == "Sport") {
            AddSport();
        }
        else if (paramCallBack == "Insure") {//added by romy, 20170811, insurance
            AddInsure();
        }
        SSRTab1Panel.PerformCallback();
        LoadingPanel.Show();
        SSRActionPanel.PerformCallback();
        if (gvPassenger.cp_result == "Infant") {
            gvPassenger2.Refresh();
            gvPassenger.cp_result = "";
        }
        
    }
}

function gvPassenger2_EndCallback(s, e) {
    if (gvPassenger2.cp_result != "" && typeof gvPassenger2.cp_result != "undefined" && gvPassenger2.cp_result != "Infant") {

        document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = gvPassenger2.cp_result;
        pcMessage.Show();
        LoadingPanel.Hide();
        if (gvPassenger2.cp_result.indexOf("Maximum Infant") != -1) {
            SSRTab2Panel.PerformCallback();
            SSRActionPanel.PerformCallback();
            gvPassenger.Refresh();
        }
        gvPassenger2.cp_result = "";
    }
    else {
        if (paramCallBack2 == "Baggage") {
            AddBaggage2();
        }
        else if (paramCallBack2 == "Meal") {
            AddMeal2();
        }
        else if (paramCallBack2 == "Meal1") {
            AddMeal21();
        }
        else if (paramCallBack2 == "Drink") {
            AddDrink2();
        }
        else if (paramCallBack2 == "Drink1") {
            AddDrink21();
        }
        else if (paramCallBack == "Sport") {
            AddSport2();
        }
        else if (paramCallBack == "Insure") {//added by romy, 20170811, insurance
            AddInsure2();
        }
        //else if (paramCallBack == "Infant") {
        //    AddInfant2();
        //}
        SSRTab2Panel.PerformCallback();
        SSRActionPanel.PerformCallback();
        if (gvPassenger2.cp_result == "Infant") {
            gvPassenger.Refresh();
            gvPassenger2.cp_result = "";
        }
        
        
        
    }
}

 var isPostbackInitiated = false;

        function OnClickBtnSave(s, e) 
        {
            popup.Hide();
            e.processOnServer = !isPostbackInitiated;
            isPostbackInitiated = true;
            return false;
        }

function SSRActionPanelEndCallBack() {
    LoadingPanel.Hide();
   
}
var change = getParameterByName('change');
function OnCallbackComplete(s, e) {
    if (e.result != "") {
        if (e.result == "Your session has expired.") {
            window.location.href = '../SessionExpired.aspx';
        }
        else {
            document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = e.result;
            pcMessage.Show();
            LoadingPanel.Hide();
        }
    }
    else {
        if (change != null) {
            if (document.getElementById('ctl00_ContentPlaceHolder2_hfTransID').value != "" && document.getElementById('ctl00_ContentPlaceHolder2_hfHashKey').value != "") {
                var TransID = document.getElementById('ctl00_ContentPlaceHolder2_hfTransID').value;
                var k = document.getElementById('ctl00_ContentPlaceHolder2_hfHashKey').value
                //alert(TransID);
                //window.location.href = '../public/Bookingdetail.aspx?k=' + k + '&TransID=' + TransID;
                //window.location.href = '../public/ProceedPayment.aspx';
                window.location.href = '../public/selectseat.aspx?change=true';
            }
        }
        else {
            window.location.href = '../public/selectseat.aspx';
        }
    }
}

var k = getParameterByName('k');
var TransID = getParameterByName('TransID');

function OnCallbackCompleteManage(s, e) {
    if (e.result != "") {
        if (e.result == "Your session has expired.") {
            window.location.href = '../SessionExpired.aspx';
        }
        else {
            document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = e.result;
            pcMessage.Show();
            LoadingPanel.Hide();
        }
    } else {
        
        //alert('../public/BookingDetail.aspx?k=' + k + '&TransID=' + TransID);
        //window.location.href = '../public/BookingDetail.aspx?k=' + k + '&TransID=' + TransID;
        
        //window.location.href = '../public/proceedpayment.aspx';
        LoadingPanel.Hide();
        //window.location.href = '../public/ProceedPayment.aspx';
    }
}

function popup_Shown(s, e) {
    //callbackPanel.PerformCallback(keyValue);
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

function ShowLoginWindow() {
    pcMessage.Show();
}

var paramCallBack = "";
var paramCallBack2 = "";

$(document).ready(function () {
    $(document).on('click', '#home .buttonsWrapper > .col-sm-2', function () {
 
        $('#home .activeBtn').removeClass('activeBtn');
        $(this).children('a').addClass('activeBtn');
        console.log('home');
    });
    $(document).on('click', '#home2 .buttonsWrapper > .col-sm-2', function () {
        $('#home2 .activeBtn').removeClass('activeBtn');
        $(this).children('a').addClass('activeBtn');
        console.log('home2');
    });
});


function setIndicator(index, wrapperId) {
    var indexing = index - 1;
    $('#' + wrapperId + ' .buttonsWrapper .col-sm-2').eq(indexing).children('a').addClass('activeBtn');

}

function AddBaggage() {
    var e = document.getElementById("cmbBaggage");
    var strUser = cmbBaggage.GetText();
    var strUserValue = cmbBaggage.GetValue();
    if (gvPassenger.batchEditApi.HasChanges()) {
        gvPassenger.UpdateEdit();
        paramCallBack = "Baggage";
    }
    else {
        gvPassenger.PerformCallback("Baggage|" + seBaggage.GetValue() + "|" + strUser + "|" + strUserValue);
        paramCallBack = "";

    }
    //SSRActionPanel.PerformCallback();
    setIndicator(1, "home");
}

function AddDrink() {
    var e = document.getElementById("cmbDrinks");
    var strUser = cmbDrinks.GetText();
    var strUserValue = cmbDrinks.GetValue();
    if (gvPassenger.batchEditApi.HasChanges()) {
        gvPassenger.UpdateEdit();
        paramCallBack = "Drink";
    }
    //SSRActionPanel.PerformCallback();
    setIndicator(1, "home");
}

function AddDrink1() {
    var e = document.getElementById("cmbDrinks1");
    var strUser = cmbDrinks1.GetText();
    var strUserValue = cmbDrinks1.GetValue();
    if (gvPassenger.batchEditApi.HasChanges()) {
        gvPassenger.UpdateEdit();
        paramCallBack = "Drink1";
    }
    //SSRActionPanel.PerformCallback();
    setIndicator(1, "home");
}

function AddMeal() {
    LoadingPanel.Show();
    var grid = glMeals.GetGridView();
    if (gvPassenger.batchEditApi.HasChanges()) {
        gvPassenger.UpdateEdit();
        paramCallBack = "Meal";
    }
    else {
        grid.GetRowValues(grid.GetFocusedRowIndex(), 'Detail;Price', OnGetRoValues);
        paramCallBack = "";

    }
    setIndicator(2, "home");
}
function OnGetRoValues(values) {
    var grid = glMeals.GetGridView();
    var key = grid.GetRowKey(grid.GetFocusedRowIndex());
    gvPassenger.PerformCallback("Meal|" + seMeals.GetValue() + "|" + values + "|" + key);
    LoadingPanel.Hide();
    //SSRActionPanel.PerformCallback();
}

function AddMeal1() {
    LoadingPanel.Show();
    var grid = glMeals1.GetGridView();
    if (gvPassenger.batchEditApi.HasChanges()) {
        gvPassenger.UpdateEdit();
        paramCallBack = "Meal1";

    }
    else {
        grid.GetRowValues(grid.GetFocusedRowIndex(), 'Detail;Price', OnGetRoValues1);
        paramCallBack = "";

    }
}
function OnGetRoValues1(values) {
    var grid = glMeals1.GetGridView();
    var key = grid.GetRowKey(grid.GetFocusedRowIndex());
    
    gvPassenger.PerformCallback("Meal1|" + seMeals1.GetValue() + "|" + values + "|" + key);
    LoadingPanel.Hide();
    //SSRActionPanel.PerformCallback();
}

function AddSport() {

    var e = document.getElementById("cmbSport");
    var strUser = cmbSport.GetText();
    var strUserValue = cmbSport.GetValue();
    if (gvPassenger.batchEditApi.HasChanges()) {
        gvPassenger.UpdateEdit();
        paramCallBack = "Sport";
    }
    else {
        gvPassenger.PerformCallback("Sport|" + seSport.GetValue() + "|" + strUser + "|" + strUserValue);
        paramCallBack = "";
        
    }
    
    //SSRActionPanel.PerformCallback();
    setIndicator(3, "home");
}
function AddInsure() {//added by romy, 20170811, insurance
    var e = document.getElementById("cmbInsure");
    var strUser = cmbInsure.GetText();
    var strUserValue = cmbInsure.GetValue();
    if (gvPassenger.batchEditApi.HasChanges()) {
        gvPassenger.UpdateEdit();
        paramCallBack = "Insure";
    }
    else {
        gvPassenger.PerformCallback("Insure|" + seInsure.GetValue() + "|" + strUser + "|" + strUserValue);
        paramCallBack = "";

    }

    //SSRActionPanel.PerformCallback();
    setIndicator(3, "home");
}
function ClearInsure() {//added by romy, 20170811, insurance

    var e = document.getElementById("cmbInsure");
    var strUser = cmbInsure.GetText();
    var strUserValue = cmbInsure.GetValue();
        gvPassenger.PerformCallback("ClearAll|" + seInsure.GetValue() + "|" + strUser + "|" + strUserValue);
        paramCallBack = "";

    //SSRActionPanel.PerformCallback();
    setIndicator(3, "home");
}

function AddDuty() {
    var grid = glDuty.GetGridView();
    grid.GetRowValues(grid.GetFocusedRowIndex(), 'Detail;Price', OnGetRowValues);
    setIndicator(5, "home");

}

function OnGetRowValues(values) {
    var grid = glDuty.GetGridView();
    var key = grid.GetRowKey(grid.GetFocusedRowIndex());
    gvPassenger.PerformCallback("Duty|" + seDuty.GetValue() + "|" + values + "|" + key);
    //SSRActionPanel.PerformCallback();
}

function AddComfort() {
    LoadingPanel.Show();
    var grid = glComfort.GetGridView();
    grid.GetRowValues(grid.GetFocusedRowIndex(), 'Detail;Price', OnGetRowsValues);
    setIndicator(4, "home");
    
}

function OnGetRowsValues(values) {
    var grid = glComfort.GetGridView();
    var key = grid.GetRowKey(grid.GetFocusedRowIndex());
    if (gvPassenger.batchEditApi.HasChanges()) {
        gvPassenger.UpdateEdit();
    }
    else {
        gvPassenger.PerformCallback("Comfort|" + seComfort.GetValue() + "|" + values + "|" + key);
    }
    LoadingPanel.Hide();
    //SSRActionPanel.PerformCallback();
}

function AddBaggage2() {
    var e = document.getElementById("cmbBaggage2");
    var strUser = cmbBaggage2.GetText();
    var strUserValue = cmbBaggage2.GetValue();
    if (gvPassenger2.batchEditApi.HasChanges()) {
        gvPassenger2.UpdateEdit();
        paramCallBack2 = "Baggage";
    }
    else {
        gvPassenger2.PerformCallback("Baggage|" + seBaggage2.GetValue() + "|" + strUser + "|" + strUserValue);
    }
    //SSRActionPanel.PerformCallback();
    setIndicator(1, "home2");
}

function AddDrink2() {
    var e = document.getElementById("cmbDrinks2");
    var strUser = cmbDrinks2.GetText();
    var strUserValue = cmbDrinks2.GetValue();
    if (gvPassenger2.batchEditApi.HasChanges()) {
        gvPassenger2.UpdateEdit();
        paramCallBack2 = "Drink";
    }
    else {
        gvPassenger2.PerformCallback("Baggage|" + seBaggage2.GetValue() + "|" + strUser + "|" + strUserValue);
    }
    //SSRActionPanel.PerformCallback();
    setIndicator(1, "home2");
}

function AddDrink21() {
    var e = document.getElementById("cmbDrinks22");
    var strUser = cmbDrinks22.GetText();
    var strUserValue = cmbDrinks22.GetValue();
    if (gvPassenger2.batchEditApi.HasChanges()) {
        gvPassenger2.UpdateEdit();
        paramCallBack2 = "Drink1";
    }
    else {
        gvPassenger2.PerformCallback("Baggage|" + seBaggage2.GetValue() + "|" + strUser + "|" + strUserValue);
    }
    //SSRActionPanel.PerformCallback();
    setIndicator(1, "home2");
}


function AddMeal2() {
    LoadingPanel.Show();
    var grid = glMeals2.GetGridView();
    if (gvPassenger2.batchEditApi.HasChanges()) {
        gvPassenger2.UpdateEdit();
        paramCallBack2 = "Meal";
    }
    else {
        grid.GetRowValues(grid.GetFocusedRowIndex(), 'Detail;Price', OnGetRoValues2);
    }
    //SSRActionPanel.PerformCallback();
    setIndicator(2, "home2");
}
function OnGetRoValues2(values) {
    var grid = glMeals2.GetGridView();
    var key = grid.GetRowKey(grid.GetFocusedRowIndex());
    gvPassenger2.PerformCallback("Meal|" + seMeals2.GetValue() + "|" + values + "|" + key);
    LoadingPanel.Hide();
    //SSRActionPanel.PerformCallback();
}

function AddMeal21() {
    LoadingPanel.Show();
    var grid = glMeals22.GetGridView();
    if (gvPassenger2.batchEditApi.HasChanges()) {
        gvPassenger2.UpdateEdit();
        paramCallBack2 = "Meal1";
    }
    else {
        grid.GetRowValues(grid.GetFocusedRowIndex(), 'Detail;Price', OnGetRoValues21);
    }
    //SSRActionPanel.PerformCallback();
}
function OnGetRoValues21(values) {
    var grid = glMeals22.GetGridView();
    var key = grid.GetRowKey(grid.GetFocusedRowIndex());
    gvPassenger2.PerformCallback("Meal1|" + seMeals22.GetValue() + "|" + values + "|" + key);
    LoadingPanel.Hide();
    //SSRActionPanel.PerformCallback();
}
function AddSport2() {

    var e = document.getElementById("cmbSport2");
    var strUser = cmbSport2.GetText();
    var strUserValue = cmbSport2.GetValue();
    if (gvPassenger2.batchEditApi.HasChanges()) {
        gvPassenger2.UpdateEdit();
        paramCallBack2 = "Sport";
    }
    else {
        gvPassenger2.PerformCallback("Sport|" + seSport2.GetValue() + "|" + strUser + "|" + strUserValue);
    }
    //SSRActionPanel.PerformCallback();
    setIndicator(3, "home2");
}
function AddInsure2() {//added by romy, 20170811, insurance

    var e = document.getElementById("cmbInsure2");
    var strUser = cmbInsure2.GetText();
    var strUserValue = cmbInsure2.GetValue();
    if (gvPassenger2.batchEditApi.HasChanges()) {
        gvPassenger2.UpdateEdit();
        paramCallBack2 = "Insure";
    }
    else {
        gvPassenger2.PerformCallback("Insure|" + seInsure2.GetValue() + "|" + strUser + "|" + strUserValue);
    }
    //SSRActionPanel.PerformCallback();
    setIndicator(3, "home2");
}

function AddDuty2() {
    var grid = glDuty2.GetGridView();
    grid.GetRowValues(grid.GetFocusedRowIndex(), 'Detail;Price', OnGetRowValues2);
    setIndicator(5, "home2");
}

function OnGetRowValues2(values) {
    var grid = glDuty2.GetGridView();
    var key = grid.GetRowKey(grid.GetFocusedRowIndex());
    gvPassenger2.PerformCallback("Duty|" + seDuty2.GetValue() + "|" + values + "|" + key);
    //SSRActionPanel.PerformCallback();
}

function AddComfort2() {
    LoadingPanel.Show();
    var grid = glComfort2.GetGridView();
    grid.GetRowValues(grid.GetFocusedRowIndex(), 'Detail;Price', OnGetRowsValues2);
    setIndicator(4, "home2");
}

function OnGetRowsValues2(values) {
    var grid = glComfort2.GetGridView();
    var key = grid.GetRowKey(grid.GetFocusedRowIndex());
    gvPassenger2.PerformCallback("Comfort|" + seComfort2.GetValue() + "|" + values + "|" + key);
    LoadingPanel.Hide();
    //SSRActionPanel.PerformCallback();
}
function showcollapsex() {
    var collapsex = document.getElementById("collapsex");
    collapsex.style.visibility = 'visible';
}
function showcollapse1() {
    var collapse1 = document.getElementById("collapse1");
    var collapse2 = document.getElementById("collapse2");
    var collapse3 = document.getElementById("collapse3");
    var collapse4 = document.getElementById("collapse4");
    var collapse5 = document.getElementById("collapse5");
    var collapse6 = document.getElementById("collapse6");
    collapse1.style.visibility = 'visible';
    collapse2.style.visibility = 'hidden';
    collapse3.style.visibility = 'hidden';
    collapse4.style.visibility = 'hidden';
    collapse5.style.visibility = 'hidden';
    collapse6.style.visibility = 'hidden';
    $('#home .markBg').removeClass('markBg');
}

function showcollapse2() {
    var collapse1 = document.getElementById("collapse1");
    var collapse2 = document.getElementById("collapse2");
    var collapse3 = document.getElementById("collapse3");
    var collapse4 = document.getElementById("collapse4");
    var collapse5 = document.getElementById("collapse5");
    var collapse6 = document.getElementById("collapse6");
    collapse1.style.visibility = 'hidden';
    collapse2.style.visibility = 'visible';
    collapse3.style.visibility = 'hidden';
    collapse4.style.visibility = 'hidden';
    collapse5.style.visibility = 'hidden';
    collapse6.style.visibility = 'hidden';
    cmbDrinks.SetEnabled(false);
    cmbDrinks1.SetEnabled(false);
    $('#home .markBg').removeClass('markBg');
}

function showcollapse3() {
    var collapse1 = document.getElementById("collapse1");
    var collapse2 = document.getElementById("collapse2");
    var collapse3 = document.getElementById("collapse3");
    var collapse4 = document.getElementById("collapse4");
    var collapse5 = document.getElementById("collapse5");
    var collapse6 = document.getElementById("collapse6");
    collapse1.style.visibility = 'hidden';
    collapse2.style.visibility = 'hidden';
    collapse3.style.visibility = 'visible';
    collapse4.style.visibility = 'hidden';
    collapse5.style.visibility = 'hidden';
    collapse6.style.visibility = 'hidden';
    $('#home .markBg').removeClass('markBg');

}

function showcollapse4() {
    var collapse1 = document.getElementById("collapse1");
    var collapse2 = document.getElementById("collapse2");
    var collapse3 = document.getElementById("collapse3");
    var collapse4 = document.getElementById("collapse4");
    var collapse5 = document.getElementById("collapse5");
    var collapse6 = document.getElementById("collapse6");
    collapse1.style.visibility = 'hidden';
    collapse2.style.visibility = 'hidden';
    collapse3.style.visibility = 'hidden';
    collapse4.style.visibility = 'visible';
    collapse5.style.visibility = 'hidden';
    collapse6.style.visibility = 'hidden';
    $('#home .markBg').removeClass('markBg');
}

function showcollapse5() {
    var collapse1 = document.getElementById("collapse1");
    var collapse2 = document.getElementById("collapse2");
    var collapse3 = document.getElementById("collapse3");
    var collapse4 = document.getElementById("collapse4");
    var collapse5 = document.getElementById("collapse5");
    var collapse6 = document.getElementById("collapse6");
    collapse1.style.visibility = 'hidden';
    collapse2.style.visibility = 'hidden';
    collapse3.style.visibility = 'hidden';
    collapse4.style.visibility = 'hidden';
    collapse5.style.visibility = 'visible';
    collapse6.style.visibility = 'hidden';
    $('#home .markBg').removeClass('markBg');
}

function showcollapse6() {
    var collapse1 = document.getElementById("collapse1");
    var collapse2 = document.getElementById("collapse2");
    var collapse3 = document.getElementById("collapse3");
    var collapse4 = document.getElementById("collapse4");
    var collapse5 = document.getElementById("collapse5");
    var collapse6 = document.getElementById("collapse6");
    collapse1.style.visibility = 'hidden';
    collapse2.style.visibility = 'hidden';
    collapse3.style.visibility = 'hidden';
    collapse4.style.visibility = 'hidden';
    collapse5.style.visibility = 'hidden';
    collapse6.style.visibility = 'visible';
    $('#home .infantCol').addClass('markBg');
    
}

function showcollapse12() {
    var collapse12 = document.getElementById("collapse12");
    var collapse22 = document.getElementById("collapse22");
    var collapse32 = document.getElementById("collapse32");
    var collapse42 = document.getElementById("collapse42");
    var collapse52 = document.getElementById("collapse52");
    var collapse62 = document.getElementById("collapse62");
    collapse12.style.visibility = 'visible';
    collapse22.style.visibility = 'hidden';
    collapse32.style.visibility = 'hidden';
    collapse42.style.visibility = 'hidden';
    collapse52.style.visibility = 'hidden';
    collapse62.style.visibility = 'hidden';
    $('#home2 .markBg').removeClass('markBg');
}

function showcollapse22() {
    var collapse12 = document.getElementById("collapse12");
    var collapse22 = document.getElementById("collapse22");
    var collapse32 = document.getElementById("collapse32");
    var collapse42 = document.getElementById("collapse42");
    var collapse52 = document.getElementById("collapse52");
    var collapse62 = document.getElementById("collapse62");
    collapse12.style.visibility = 'hidden';
    collapse22.style.visibility = 'visible';
    collapse32.style.visibility = 'hidden';
    collapse42.style.visibility = 'hidden';
    collapse52.style.visibility = 'hidden';
    collapse62.style.visibility = 'hidden';
    cmbDrinks2.SetEnabled(false);
    cmbDrinks22.SetEnabled(false);
    $('#home2 .markBg').removeClass('markBg');
}

function showcollapse32() {
    var collapse12 = document.getElementById("collapse12");
    var collapse22 = document.getElementById("collapse22");
    var collapse32 = document.getElementById("collapse32");
    var collapse42 = document.getElementById("collapse42");
    var collapse52 = document.getElementById("collapse52");
    var collapse62 = document.getElementById("collapse62");
    collapse12.style.visibility = 'hidden';
    collapse22.style.visibility = 'hidden';
    collapse32.style.visibility = 'visible';
    collapse42.style.visibility = 'hidden';
    collapse52.style.visibility = 'hidden';
    collapse62.style.visibility = 'hidden';
    $('#home2 .markBg').removeClass('markBg');
}

function showcollapse42() {
    var collapse12 = document.getElementById("collapse12");
    var collapse22 = document.getElementById("collapse22");
    var collapse32 = document.getElementById("collapse32");
    var collapse42 = document.getElementById("collapse42");
    var collapse52 = document.getElementById("collapse52");
    var collapse62 = document.getElementById("collapse62");
    collapse12.style.visibility = 'hidden';
    collapse22.style.visibility = 'hidden';
    collapse32.style.visibility = 'hidden';
    collapse42.style.visibility = 'visible';
    collapse52.style.visibility = 'hidden';
    collapse62.style.visibility = 'hidden';
    $('#home2 .markBg').removeClass('markBg');
}

function showcollapse52() {
    var collapse12 = document.getElementById("collapse12");
    var collapse22 = document.getElementById("collapse22");
    var collapse32 = document.getElementById("collapse32");
    var collapse42 = document.getElementById("collapse42");
    var collapse52 = document.getElementById("collapse52");
    var collapse62 = document.getElementById("collapse62");
    collapse12.style.visibility = 'hidden';
    collapse22.style.visibility = 'hidden';
    collapse32.style.visibility = 'hidden';
    collapse42.style.visibility = 'hidden';
    collapse52.style.visibility = 'visible';
    collapse62.style.visibility = 'hidden';
    $('#home2 .markBg').removeClass('markBg');
}

function showcollapse62() {
    var collapse1 = document.getElementById("collapse12");
    var collapse2 = document.getElementById("collapse22");
    var collapse3 = document.getElementById("collapse32");
    var collapse4 = document.getElementById("collapse42");
    var collapse5 = document.getElementById("collapse52");
    var collapse6 = document.getElementById("collapse62");
    collapse1.style.visibility = 'hidden';
    collapse2.style.visibility = 'hidden';
    collapse3.style.visibility = 'hidden';
    collapse4.style.visibility = 'hidden';
    collapse5.style.visibility = 'hidden';
    collapse6.style.visibility = 'visible';
    $('#home2 .infantCol').addClass('markBg');

}

function onBatchEditEndEditing(s, e) {
    if (paramCallBack == "") {
        var templateColumn = s.GetColumnByField("Meal");
        if (templateColumn != null && templateColumn != "undefined" && templateColumn != "" && typeof glMealP1 !== "undefined") {
            if (!e.rowValues.hasOwnProperty(templateColumn.index))
                return;
            var cellInfo = e.rowValues[templateColumn.index];
            cellInfo.value = glMealP1.GetValue();
            cellInfo.text = glMealP1.GetText();
        }

        var templateColumnMeal1 = s.GetColumnByField("Meal1");
        if (templateColumnMeal1 != null && templateColumnMeal1 != "undefined" && templateColumnMeal1 != "" && typeof glMealP11 !== "undefined") {
            if (!e.rowValues.hasOwnProperty(templateColumnMeal1.index))
                return;
            var cellInfo = e.rowValues[templateColumnMeal1.index];
            cellInfo.value = glMealP11.GetValue();
            cellInfo.text = glMealP11.GetText();
        }

        var templateColumnComfort = s.GetColumnByField("Comfort");
        if (templateColumnComfort != null && templateColumnComfort != "undefined" && templateColumnComfort != "" && typeof glComfortP1 !== "undefined") {
            if (!e.rowValues.hasOwnProperty(templateColumnComfort.index))
                return;
            var cellInfo = e.rowValues[templateColumnComfort.index];
            cellInfo.value = glComfortP1.GetValue();
            cellInfo.text = glComfortP1.GetText();
        }
    }
    
}

function onBatchEditEndEditingManage(s, e) {
    if (paramCallBack == "") {
        var templateColumn = s.GetColumnByField("DepartMeal");
        if (templateColumn != null && templateColumn != "undefined" && templateColumn != "" && typeof glMealP1 !== "undefined") {
            if (!e.rowValues.hasOwnProperty(templateColumn.index))
                return;
            var cellInfo = e.rowValues[templateColumn.index];
            cellInfo.value = glMealP1.GetValue();
            cellInfo.text = glMealP1.GetText();
        }

        var templateColumnMeal1 = s.GetColumnByField("ConDepartMeal");
        if (templateColumnMeal1 != null && templateColumnMeal1 != "undefined" && templateColumnMeal1 != "" && typeof glMealP11 !== "undefined") {
            if (!e.rowValues.hasOwnProperty(templateColumnMeal1.index))
                return;
            var cellInfo = e.rowValues[templateColumnMeal1.index];
            cellInfo.value = glMealP11.GetValue();
            cellInfo.text = glMealP11.GetText();
        }

        var templateColumnComfort = s.GetColumnByField("DepartComfort");
        if (templateColumnComfort != null && templateColumnComfort != "undefined" && templateColumnComfort != "" && typeof glComfortP1 !== "undefined") {
            if (!e.rowValues.hasOwnProperty(templateColumnComfort.index))
                return;
            var cellInfo = e.rowValues[templateColumnComfort.index];
            cellInfo.value = glComfortP1.GetValue();
            cellInfo.text = glComfortP1.GetText();
        }
    }

}

function onBatchEditEndEditingManage2(s, e) {
    if (paramCallBack2 == "") {
        var templateColumn = s.GetColumnByField("ReturnMeal");
        if (templateColumn != null && templateColumn != "undefined" && templateColumn != "" && typeof glMealP21 !== "undefined") {
            if (!e.rowValues.hasOwnProperty(templateColumn.index))
                return;
            var cellInfo = e.rowValues[templateColumn.index];
            cellInfo.value = glMealP21.GetValue();
            cellInfo.text = glMealP21.GetText();
        }

        var templateColumnMeal1 = s.GetColumnByField("ConReturnMeal");
        if (templateColumnMeal1 != null && templateColumnMeal1 != "undefined" && templateColumnMeal1 != "" && typeof glMealP22 !== "undefined") {
            if (!e.rowValues.hasOwnProperty(templateColumnMeal1.index))
                return;
            var cellInfo = e.rowValues[templateColumnMeal1.index];
            cellInfo.value = glMealP22.GetValue();
            cellInfo.text = glMealP22.GetText();
        }

        var templateColumnComfort = s.GetColumnByField("ReturnComfort");
        if (templateColumnComfort != null && templateColumnComfort != "undefined" && templateColumnComfort != "" && typeof glComfortP1 !== "undefined") {
            if (!e.rowValues.hasOwnProperty(templateColumnComfort.index))
                return;
            var cellInfo = e.rowValues[templateColumnComfort.index];
            cellInfo.value = glComfortP2.GetValue();
            cellInfo.text = glComfortP2.GetText();
        }
    }

}

function onBatchEditEndEditinggvPassenger2(s, e) {
    var templateColumn = s.GetColumnByField("Meal");
    if (templateColumn != null && templateColumn != "undefined" && templateColumn != "" && typeof glMealP21 !== "undefined") {
        if (!e.rowValues.hasOwnProperty(templateColumn.index))
            return;
        var cellInfo = e.rowValues[templateColumn.index];
        cellInfo.value = glMealP21.GetValue();
        cellInfo.text = glMealP21.GetText();
    }

    var templateColumnMeal1 = s.GetColumnByField("Meal1");
    if (templateColumnMeal1 != null && templateColumnMeal1 != "undefined" && templateColumnMeal1 != "" && typeof glMealP22 !== "undefined") {
        if (!e.rowValues.hasOwnProperty(templateColumnMeal1.index))
            return;
        var cellInfo = e.rowValues[templateColumnMeal1.index];
        cellInfo.value = glMealP22.GetValue();
        cellInfo.text = glMealP22.GetText();
    }

    var templateColumnComfort = s.GetColumnByField("Comfort");
    if (templateColumnComfort != null && templateColumnComfort != "undefined" && templateColumnComfort != "" && typeof glComfortP2 !== "undefined") {
        if (!e.rowValues.hasOwnProperty(templateColumnComfort.index))
            return;
        var cellInfo = e.rowValues[templateColumnComfort.index];
        cellInfo.value = glComfortP2.GetValue();
        cellInfo.text = glComfortP2.GetText();
    }
}

function OnCustomButtonClick(s, e) {
    e.processOnServer = false;
    if (e.buttonID == 'ClearLink') {
        var key = s.GetRowKey(e.visibleIndex);
        var splitkey = key.split('|');
        gvPassenger.PerformCallback('Clear|' + ((splitkey[0]) - 1));
    }
    else if (e.buttonID == 'ClearInsure') {
        var key = s.GetRowKey(e.visibleIndex);
        var splitkey = key.split('|');
        gvPassenger.PerformCallback('Clear|' + ((splitkey[1])-1));
    }
    else
    {
        if (typeof popup != 'undefined') {
            LoadingPanel.Show();
            popup.Show();
            s.GetRowValues(e.visibleIndex, 'SeqNo', SetLabelPopup);
        }
    }
    
}

function SetLabelPopup(values) {
    //lblPaxName.SetText(values[0] + ' ' + values[1]);
    //lblPaxID.SetText(values[2]);
    document.getElementById('ctl00_ContentPlaceHolder2_hfIndex').value = values;
    //alert(document.getElementById('ctl00_ContentPlaceHolder2_hfIndex').value);
    callbackPanel.PerformCallback();
    LoadingPanel.Hide();
}

function OnCustomButtonClick2(s, e) {
    e.processOnServer = false;
    if (e.buttonID == 'ClearLink1') {
        var key = s.GetRowKey(e.visibleIndex);
        var splitkey = key.split('|');
        gvPassenger2.PerformCallback('Clear|' + ((splitkey[0]) - 1));
    }
    else {
        if (typeof popup != 'undefined') {
            LoadingPanel.Show();
            popup.Show();
            s.GetRowValues(e.visibleIndex, 'SeqNo', SetLabelPopup);
        }
    }

}

function DOBValidation(s, e)
{
    var obj = {};
    obj.DOB = txtDOB.GetValue();
    obj.TransID = getParameterByName('TransID');
    $.ajax({
        type: 'POST',
        url: 'ManageAddOns.aspx/DOBValidation',
        data: JSON.stringify(obj),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (msg) {
            // Notice that msg.d is used to retrieve the result object
            if (msg.d != "")
            {
                lblDOBError.SetText(msg.d);
                btSave.SetEnabled(false);
            }
            else if (lblExpiryDateError.GetText() != "" && msg.d == "")
            {
                lblDOBError.SetText('');
                btSave.SetEnabled(false);
            }
            else
            {
                lblDOBError.SetText('');
                btSave.SetEnabled(true);
            }
        },
        error: function (data) {
            //alert('Something Went Wrong')
        }
    });
}

function ExpiryDateValidation() {
    var obj = {};
    obj.ExpiryDate = txtExpired.GetValue();
    obj.TransID = getParameterByName('TransID');
    $.ajax({
        type: 'POST',
        url: 'ManageAddOns.aspx/ExpiryDateValidation',
        data: JSON.stringify(obj),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (msg) {
            // Notice that msg.d is used to retrieve the result object
            if (msg.d != "") {
                lblExpiryDateError.SetText(msg.d);
                btSave.SetEnabled(false);
            }
            else if (lblDOBError.GetText() != "" && msg.d == "") {
                lblExpiryDateError.SetText('');
                btSave.SetEnabled(false);
            }
            else
            {
                lblExpiryDateError.SetText('');
                btSave.SetEnabled(true);
            }
        },
        error: function (data) {
            //alert('Something Went Wrong')
        }
    });
}

function onCancelGrid(s, e) {
    gvPassenger.CancelEdit();
}

function onCancelGridPass2(s, e) {
    gvPassenger2.CancelEdit();
}
function AllPaxInsure1() {//added by romy, 20170811, insurance
    if ($('#ctl00_ContentPlaceHolder2_cbAllPaxInsure1').is(':checked')) {
        seInsure.SetEnabled(false);
        seInsure.SetText(document.getElementById('ctl00_ContentPlaceHolder2_MaxPax').value);
    }
    else {
        seInsure.SetEnabled(true);
        seInsure.SetText(document.getElementById('ctl00_ContentPlaceHolder2_MinPax').value);
    }
}
function AllPaxBaggage1() {
    if ($('#ctl00_ContentPlaceHolder2_TabControl_cbAllPaxBaggage1').is(':checked')) {
        seBaggage.SetEnabled(false);
        seBaggage.SetText(document.getElementById('ctl00_ContentPlaceHolder2_MaxPax').value);
    }
    else {
        seBaggage.SetEnabled(true);
        seBaggage.SetText('');
    }
}

function AllPaxMeal11() {
    if ($('#ctl00_ContentPlaceHolder2_TabControl_cbAllPaxMeal11').is(':checked')) {
        seMeals.SetEnabled(false);
        seMeals.SetText(document.getElementById('ctl00_ContentPlaceHolder2_MaxPax').value);
    }
    else {
        seMeals.SetEnabled(true);
        seMeals.SetText('');
    }
}

function AllPaxMeal21() {
    if ($('#ctl00_ContentPlaceHolder2_TabControl_cbAllPaxMeal21').is(':checked')) {
        seMeals1.SetEnabled(false);
        seMeals1.SetText(document.getElementById('ctl00_ContentPlaceHolder2_MaxPax').value);
    }
    else {
        seMeals1.SetEnabled(true);
        seMeals1.SetText('');
    }
}

function AllPaxSport1() {
    if ($('#ctl00_ContentPlaceHolder2_TabControl_cbAllPaxSport1').is(':checked')) {
        seSport.SetEnabled(false);
        seSport.SetText(document.getElementById('ctl00_ContentPlaceHolder2_MaxPax').value);
    }
    else {
        seSport.SetEnabled(true);
        seSport.SetText('');
    }
}

function AllPaxComfort1() {
    if ($('#ctl00_ContentPlaceHolder2_TabControl_cbAllPaxComfort1').is(':checked')) {
        seComfort.SetEnabled(false);
        seComfort.SetText(document.getElementById('ctl00_ContentPlaceHolder2_MaxPax').value);
    }
    else {
        seComfort.SetEnabled(true);
        seComfort.SetText('');
    }
}

function AllPaxDuty1() {
    if ($('#ctl00_ContentPlaceHolder2_TabControl_cbAllPaxDuty1').is(':checked')) {
        seDuty.SetEnabled(false);
    }
    else {
        seDuty.SetEnabled(true);
    }
}
function AllPaxInsure2() {//added by romy, 20170811, insurance
    if ($('#ctl00_ContentPlaceHolder2_TabControl_cbAllPaxInsure2').is(':checked')) {
        seInsure2.SetEnabled(false);
    }
    else {
        seInsure2.SetEnabled(true);
    }
}

function AllPaxBaggage2() {
    if ($('#ctl00_ContentPlaceHolder2_TabControl_cbAllBaggage2').is(':checked')) {
        seBaggage2.SetEnabled(false);
        seBaggage2.SetText(document.getElementById('ctl00_ContentPlaceHolder2_MaxPax').value);
    }
    else {
        seBaggage2.SetEnabled(true);
        seBaggage2.SetText('');
    }
}

function AllPaxMeal12() {
    if ($('#ctl00_ContentPlaceHolder2_TabControl_cbAllPaxMeal12').is(':checked')) {
        seMeals2.SetEnabled(false);
        seMeals2.SetText(document.getElementById('ctl00_ContentPlaceHolder2_MaxPax').value);
    }
    else {
        seMeals2.SetEnabled(true);
        seMeals2.SetText('');
    }
}

function AllPaxMeal22() {
    if ($('#ctl00_ContentPlaceHolder2_TabControl_cbAllPaxMeal22').is(':checked')) {
        seMeals22.SetEnabled(false);
        seMeals22.SetText(document.getElementById('ctl00_ContentPlaceHolder2_MaxPax').value);
    }
    else {
        seMeals22.SetEnabled(true);
        seMeals22.SetText('');
    }
}

function AllPaxSport2() {
    if ($('#ctl00_ContentPlaceHolder2_TabControl_cbAllPaxSport2').is(':checked')) {
        seSport2.SetEnabled(false);
        seSport2.SetText(document.getElementById('ctl00_ContentPlaceHolder2_MaxPax').value);
    }
    else {
        seSport2.SetEnabled(true);
        seSport2.SetText('');
    }
}

function AllPaxComfort2() {
    if ($('#ctl00_ContentPlaceHolder2_TabControl_cbAllPaxComfort2').is(':checked')) {
        seComfort2.SetEnabled(false);
        seComfort2.SetText(document.getElementById('ctl00_ContentPlaceHolder2_MaxPax').value);
    }
    else {
        seComfort2.SetEnabled(true);
        seComfort2.SetText('');
    }
}

function AllPaxDuty2() {
    if ($('#ctl00_ContentPlaceHolder2_TabControl_cbAllPaxDuty2').is(':checked')) {
        seDuty2.SetEnabled(false);
    }
    else {
        seDuty2.SetEnabled(true);
    }
}


function hoverToolTip(toolTipObj, elementToPoint) {
    var toolTipContent = toolTipObj.attr("toolTipContent"),
        toolTipTop = toolTipObj.offset().top,
        toolTipLeft = toolTipObj.offset().left,
        screenWidth = $(window).width(),
        screenHeight = $(window).height(),
        leftAddWidth = toolTipLeft + 20,
        rightAddWidth = screenWidth - leftAddWidth,
        contentTextWidth = 0;
    console.log(elementToPoint);
    if (typeof elementToPoint != 'undefined') {
        var markerFly = elementToPoint;
        //check if we the tooltip has markerfly, if has get the class name 
        var markerLeft = $(markerFly).offset().left;
        var markerTop = $(markerFly).offset().top;
        $('body').append("<div class='markerFly'><div class='pointAnimateWrapper'><i class='fa fa-arrow-down'></i></div></div>");
        $('.markerFly').css('top', markerTop - 20 + 'px');
        $('.markerFly').css('left', markerLeft + 'px');
        $('.markerFly').addClass('activeTip');
    }

    if ($('#toolTipHTMLWrapper').length == 0) { $('body').append('<div id="toolTipHTMLWrapper"><span></span></div>') };
    $('#toolTipHTMLWrapper span').html(toolTipContent);
    contentTextWidth = $('#toolTipHTMLWrapper').width();
    $('#toolTipHTMLWrapper').css('top', toolTipTop + 20 + 'px');
    $('#toolTipHTMLWrapper').css('left', toolTipLeft + 'px');
    //count if the space in the right is smaller than the width of content wrapper
    if (rightAddWidth <= contentTextWidth) {
        $('#toolTipHTMLWrapper').css('margin-left', '-' + contentTextWidth + 'px');
    }
    else $('#toolTipHTMLWrapper').css('margin-left', '0');

}

function tool_tip(targetPoint) {
    $('.tool-Tip').mouseover(function (e) {
        //var toolIndex = $(this).index();
        //console.log(toolIndex);
        hoverToolTip($(this), targetPoint);
        $('#toolTipHTMLWrapper').addClass('activeTool');

    });
    $(this).mouseout(function () {
        $('.activeTool').removeClass('activeTool');
        $('.markerFly.active').removeClass('activeTip');
    });
}
function OnInsuranceChanged(s,e) {
    alert('a');

    //var seSellPrice = document.getElementById("gvResourcesAssignment_DXEditor4_I");
    var ContractNo = s.GetValue();
    //seSellPrice.value = (ContractNo);
    //alert('a');
    
    alert(ContractNo);

    var index1 = e.visibleIndex;
    alert(index1);
    var ID = s.GetInputElement().getAttribute('id');
    var arrCheck = ID.split("_");
    console.log(arrCheck);
    var index = arrCheck[arrCheck.length - 1];
    alert(index);
    //ctl00_ContentPlaceHolder2_TabControl_gvPassenger_cell4_9_lblInsureFee

    var colItemNo = 'ctl00_ContentPlaceHolder2_TabControl_gvPassenger_cell' + index + '_9_lblInsureFee';
    var vCboItemNo = ASPxClientControl.GetControlCollection().GetByName(colItemNo);

    //$.ajax({
    //    type: "POST",
    //    url: "ResourceAssignment.aspx/GetContractNo",
    //    data: "{'ContractID':'" + ContractNo + "'}",
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (msg) {
    //        var Temp = msg.d;
    //        seSellPrice.value = (Temp);
    //    },
    //    error: function (data) {
    //    }
    //});
    
}