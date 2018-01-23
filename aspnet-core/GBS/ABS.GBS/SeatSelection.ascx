<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SeatSelection.ascx.cs" Inherits="ABS.GBS.SeatSelection" %>
<link rel="stylesheet" type="text/css" href="../css/far.css?ver=1.4" />
<script type="text/javascript" src="../js/AKBase/far-min.js"></script>
<%--<script type="text/javascript" src="../js/AKBase/farMinCopy.js"></script>--%>
<script type="text/javascript" src="../js/AKBase/optimost.js"></script>
<script type="text/javascript" src="../js/AKBase/unitMap-min.js"></script>

<style type="text/css">
    #ctl00_ContentPlaceHolder2_connectedflightcontrol
    {
        position: absolute !important;
       
    }
</style>
<div id="JetAircraft">
<div id="unitMapView" class="unitMapViewJetAircraft">
<div id="deck" class="compartment"></div></div></div>

<input type="hidden" name="s_xmlurl" id="s_xmlurl" value="../<%=xmlurl %>" />
<input type="hidden" name="currentindex" id="currentindex" value="<%=currentpassengerindex %>" />
<input type="hidden" name="SeatTotAmt" id="SeatTotAmt" />
<%=self_hidden %>
<script type="text/javascript">
    SKYSALES.datepicker = {};
    SKYSALES.datepicker.datePickerFormat = '';
    SKYSALES.datepicker.datePickerDelimiter = '';

    if (SKYSALES.datepicker.datePickerFormat.length !== 3) {
        SKYSALES.datepicker.datePickerFormat = 'mdy';
        SKYSALES.datepicker.datePickerDelimiter = '/';
    }
    SKYSALES.datepicker.closeText = 'Close';
    SKYSALES.datepicker.prevText = '<<';
    SKYSALES.datepicker.nextText = '>>';
    SKYSALES.datepicker.currentText = 'Today';



    function erase(element, defaultValue) {
        if (element.value == defaultValue) element.value = '';
    }

    function set(element, defaultValue) {
        if (element.value == '') element.value = defaultValue;
    }

    var interface_seats = "";
    var interface_seathidden = "";
    var interface_seatmax = "";
    var interface_feehash = "";
    var interface_createunitmap = "";
    var completed = 0;

    //-------------------------------------------------------------------
    // PORTING OVER PREVIOUS FUNCTION
    //-------------------------------------------------------------------

    function call_interface_createunitmap() {
        if (completed == 0 ||
		document.readyState !== 'complete') {
            setTimeout("call_interface_createunitmap()", 500);
            return;
        }
        if (SKYSALES.Class.UnitFee) {
            eval("SKYSALES.Class.UnitFee.UnitFeeHash = {\n" + interface_feehash + "\n}");
        }
        if (SKYSALES.Class.UnitMapContainer) {
            SKYSALES.Class.UnitMapContainer.createUnitMapContainer = function () {
                var unitMapContainer = SKYSALES.Class.UnitMapContainer();
                 //TEST
                //id$set("test","{" + interface_createunitmap + "}");
                json = JSON.parse("{" + interface_createunitmap + "}");
               
                unitMapContainer.init(json);
            };
            SKYSALES.Class.UnitMapContainer.createUnitMapContainer();
        }
    }

    //-------------------------------------------------------------------

    function id$(id) {
        return document.getElementById(id);
    }

    function id$set(id, value) {
        var obj = id$(id);
        if (obj) {
            if (obj.tagName.toLowerCase() == "div") {
                obj.innerHTML = value;
            }
            if (obj.tagName.toLowerCase() == "span") {
                obj.innerHTML = value;
            }
            if (obj.tagName.toLowerCase() == "a") {
                obj.href = value;
            }
            if (obj.tagName.toLowerCase() == "input" && obj.type.toLowerCase() == "text") {
                obj.value = value;
            }
            if (obj.tagName.toLowerCase() == "input" && obj.type.toLowerCase() == "password") {
                obj.value = value;
            }
            if (obj.tagName.toLowerCase() == "input" && obj.type.toLowerCase() == "hidden") {
                obj.value = value;
            }
            if (obj.tagName.toLowerCase() == "input" && obj.type.toLowerCase() == "checkbox") {
                obj.checked = (value == 'true');
            }
            if (obj.tagName.toLowerCase() == "input" && obj.type.toLowerCase() == "radio") {
                obj.checked = (value == 'true');
            }
            if (obj.tagName.toLowerCase() == "textarea") {
                obj.value = value;
            }
            if (obj.tagName.toLowerCase() == "select") {
                for (i = 0; i < obj.options.length; i++) {
                    if (obj.options[i].value == value) {
                        obj.selectedIndex = i;
                        break;
                    }
                }
            }

        }
    }

    function id$value(id) {
        var obj = id$(id);
        if (obj) {
            if (obj.tagName.toLowerCase() == "div") {
                return obj.innerHTML;
            }
            if (obj.tagName.toLowerCase() == "span") {
                return obj.innerHTML;
            }
            if (obj.tagName.toLowerCase() == "a") {
                return obj.href;
            }
            if (obj.tagName.toLowerCase() == "input" && obj.type.toLowerCase() == "text") {
                return obj.value;
            }
            if (obj.tagName.toLowerCase() == "input" && obj.type.toLowerCase() == "password") {
                return obj.value;
            }
            if (obj.tagName.toLowerCase() == "input" && obj.type.toLowerCase() == "hidden") {
                return obj.value;
            }
            if (obj.tagName.toLowerCase() == "input" && obj.type.toLowerCase() == "checkbox") {
                return obj.value;
            }
            if (obj.tagName.toLowerCase() == "input" && obj.type.toLowerCase() == "radio") {
                return obj.value;
            }
            if (obj.tagName.toLowerCase() == "textarea") {
                return obj.value;
            }
            if (obj.tagName.toLowerCase() == "select") {
                return obj.options[obj.selectedIndex].value;
            }

        }
    }

    //-------------------------------------------------------------------

    function prepare_nodevalue(parent, nodename) {
        var childs = parent.childNodes;
        for (var i = 0; i < childs.length; i++) {
            if (childs[i].nodeName == nodename) {
                if (childs[i].childNodes.length > 0)
                    return childs[i].childNodes[0].nodeValue;
                else
                    return "";
            }
        }
        return "";
    }

    function prepare_interfacing_variable(xmlhttp) {
        interface_feehash = "";
        interface_createunitmap = "";
        var a = xmlhttp.responseXML.documentElement.getElementsByTagName("SeatGroupPassengerFee");
        var b = null;
        var c = null;
        var d = null;
        var e = null;
        var f = null;
        var g = null;
        var h = null;
        var j = null;
        var k = null;
        var l = null;
        var m = null;
        var n = null;
        var o = null;
        var p = null;
        var q = null;
        for (var i = 0; i < a.length; i++) {
            b = a[i].getElementsByTagName("SeatGroup");
            c = a[i].getElementsByTagName("ForeignAmount");
            d = a[i].getElementsByTagName("ForeignCurrencyCode");

            if (c[0].firstChild.nodeValue == "" || c[0].firstChild.nodeValue == "0") {
                interface_feehash += "'" + b[0].firstChild.nodeValue + "':''";
            } else {
                interface_feehash += "'" + b[0].firstChild.nodeValue + "':'" + parseFloat(c[0].firstChild.nodeValue).toFixed(2) + " " + d[0].firstChild.nodeValue + "'";
            }
            if (i != a.length - 1) {
                interface_feehash += "\n,\n";
            }
        }


        interface_createunitmap = "\"blockedSeatMessage\": \"Due to safety regulations, seats by the emergency exit are only available for able bodied adult guests who are between the age of 16 to 64 and are without hearing or visual impairment or other disabilities.Adults traveling with infant(s), children, the pregnant guests, elderly and guests who require special needs are advised to re-select your seats if you would like to keep your travel party together.\",\"noSeatsConfirmationMessage\": \"Do you want to skip seat allocation?\",\"clientId\": \"ControlGroupUnitMapView_UnitMapViewControl\",\"equipmentImagePath\": \"../images/AKBase/equipment/\",\"genericUnitImagePath\": \"images/base/equipment/IconReplicatorServiceFiles/\",\"propertyIconImagePath\": \"images/base/equipment/IconReplicatorServiceFiles/\",\"blankImageName\": \"blank\",\"clientName\": \"ControlGroupUnitMapView$UnitMapViewControl\",\"noUnitsMeetFilterCriteriaMessage\": \"No seats meet the selected criteria.\",\"ssrListJson\": {\"SsrList\":[]},\"ssrFeeListJson\": {\"SsrFeeList\":[]},\"unitMapJson\": [{\"equipmentJson\":";

        a = xmlhttp.responseXML.documentElement.getElementsByTagName("CompartmentDesignator");
        b = xmlhttp.responseXML.documentElement.getElementsByTagName("CompartmentInfo");
        c = xmlhttp.responseXML.documentElement.getElementsByTagName("Compartments");
        if(a[0].firstChild.nodeValue != "Y") {
            c[0].removeChild(b[0]);
        }

        a = xmlhttp.responseXML.documentElement.getElementsByTagName("CompartmentDesignator");
        b = xmlhttp.responseXML.documentElement.getElementsByTagName("CompartmentInfo");
        c = xmlhttp.responseXML.documentElement.getElementsByTagName("Compartments");
        if(a[0].firstChild.nodeValue != "Y") {
            c[0].removeChild(b[0]);
        }

        a = xmlhttp.responseXML.documentElement.getElementsByTagName("EquipmentInfos");
        b = a[0].getElementsByTagName("ArrivalStation");
        c = a[0].getElementsByTagName("CompartmentDesignator");
        d = a[0].getElementsByTagName("Deck");
        e = a[0].getElementsByTagName("Length");
        f = a[0].getElementsByTagName("AvailableUnits");
        interface_createunitmap += "{\"piu\":[3333,786432],\"as\":\"" + b[0].firstChild.nodeValue + "\",\"c\":[{\"cd\":\"" + c[0].firstChild.nodeValue + "\",\"d\":" + d[0].firstChild.nodeValue + ",\"l\":" + e[0].firstChild.nodeValue + ",\"au\":" + f[0].firstChild.nodeValue + ",\"u\":[";

        b = a[0].getElementsByTagName("SeatInfo");
        for (var i = 0; i < b.length; i++) {

            c = prepare_nodevalue(b[i], "Assignable");
            if (c == "") c = 0;
            else
                if (c == "false") {
                    c = 0;
                } else {
                    c = 1;
                }
            d = prepare_nodevalue(b[i], "PropertyBits");
            if (d == "") d = "0,0";
            e = prepare_nodevalue(b[i], "SSRPermissionBits");
            f = prepare_nodevalue(b[i], "CompartmentDesignator");
            g = prepare_nodevalue(b[i], "Height");
            h = prepare_nodevalue(b[i], "SeatSet");
            j = prepare_nodevalue(b[i], "SeatDesignator");
            k = prepare_nodevalue(b[i], "SeatGroup");
            l = prepare_nodevalue(b[i], "SeatType");
            m = prepare_nodevalue(b[i], "SeatAvailability");
            n = prepare_nodevalue(b[i], "SeatAngle");
            o = prepare_nodevalue(b[i], "Width");
            p = prepare_nodevalue(b[i], "X");
            q = prepare_nodevalue(b[i], "Y");
            r = prepare_nodevalue(b[i], "Text");

            interface_createunitmap += "{\"icn\":\"\",\"a\":" + c + ",\"pb\":[" + d + "],\"cd\":\"" + f + "\",\"h\":" + g + ",\"ss\":" + h + ",\"pi\":[-32769,-32769," + g + ",-32769,-32769,-32769,-32769," + o + ",-32769],\"spb\":[" + e + "],\"an\":" + n + ",\"ua\":\"" + m + "\",\"ud\":\"" + j + "\",\"ug\":" + k + ",\"ut\":\"" + l + "\",\"w\":" + o + ",\"x\":" + p + ",\"y\":" + q + ",\"t\":\"" + r + "\"}";

            

            if (i != b.length - 1) {
                interface_createunitmap += ",";
            }
        }

        b = a[0].getElementsByTagName("CompartmentInfo");
        c = b[0].getElementsByTagName("Width");
        var maxwidth = 0;
        for (var i = 0; i < c.length; i++) {
            if(parseInt(maxwidth,10) < parseInt(c[i].firstChild.nodeValue,10)) {
                maxwidth = c[i].firstChild.nodeValue;
            }
        }
        c = maxwidth;

        d = a[0].getElementsByTagName("DepartureStation");
        if (d[0].firstChild == null) d = "";
        else d = d[0].firstChild.nodeValue;

        e = a[0].getElementsByTagName("EquipmentCategory");
        if (e[0].firstChild == null) e = "";
        else e = e[0].firstChild.nodeValue;

        f = a[0].getElementsByTagName("EquipmentType");
        if (f[0].firstChild == null) f = "";
        else f = f[0].firstChild.nodeValue;

        g = a[0].getElementsByTagName("EquipmentTypeSuffix");
        if (g[0].firstChild == null) g = "";
        else g = g[0].firstChild.nodeValue;

        h = a[0].getElementsByTagName("MarketingCode");
        if (h[0].firstChild == null) h = "";
        else h = h[0].firstChild.nodeValue;

        j = a[0].getElementsByTagName("AvailableUnits");
        if (j[0].firstChild == null) j = "";
        else j = j[0].firstChild.nodeValue;

        interface_createunitmap += "],\"w\":" + c + "}],\"ds\":\"" + d + "\",\"ec\":\"JetAircraft\",\"et\":\"" + f + "\",\"ets\":\"" + g + "\",\"mc\":\"" + h + "\",\"au\":" + j + ",\"is\":0,\"ia\":1},";

        a = xmlhttp.responseXML.documentElement.getElementsByTagName("PropertyTypeCodesLookup");

        interface_createunitmap += "\"flattenedPropertyTypeListJson\": {\"FlattenedPropertyTypeList\":[";

        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Aisle\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"AISLE\",\"propertyTypeName\":\"Aisle\",\"searchable\":1,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":1},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Blocked / Inoperable\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"BLOCKED\",\"propertyTypeName\":\"Blocked / Inoperable\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Bulkhead\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"BULKHEAD\",\"propertyTypeName\":\"Bulkhead\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":1},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Forward Facing Seat\",\"physicalProperty\":0,\"propertyCode\":\"FRWRD\",\"propertyTypeCode\":\"DIR\",\"propertyTypeName\":\"Seat Direction\",\"searchable\":1,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Backward Facing Seat\",\"physicalProperty\":0,\"propertyCode\":\"NFRWRD\",\"propertyTypeCode\":\"DIR\",\"propertyTypeName\":\"Seat Direction\",\"searchable\":1,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":4,\"iconContentName\":\"\",\"name\":\"Person with Disability\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"DISABIL\",\"propertyTypeName\":\"Person with Disability\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Dynamic Labeling\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"DYNLAB\",\"propertyTypeName\":\"Dynamic Labeling\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Near Engine\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"ENGINE\",\"propertyTypeName\":\"Near Engine\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Emergency Exit\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"EXITROW\",\"propertyTypeName\":\"Emergency Exit\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":1},";
        interface_createunitmap += "{\"displayPriority\":5,\"iconContentName\":\"root/Equipment/SeatMap/Infant\",\"name\":\"Infant Allowed\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"INFANT\",\"propertyTypeName\":\"Infant Allowed\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":3,\"iconContentName\":\"\",\"name\":\"Near Lavatory\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"LAVATORY\",\"propertyTypeName\":\"Near Lavatory\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":1},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Extra Leg Room\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"LEGROOM\",\"propertyTypeName\":\"Extra Leg Room\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":1},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Free Seat\",\"physicalProperty\":1,\"propertyCode\":\"FREE\",\"propertyTypeCode\":\"MAXRUSE\",\"propertyTypeName\":\"Maximum Seat Re-use\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Taken Before or After\",\"physicalProperty\":1,\"propertyCode\":\"TBOA\",\"propertyTypeCode\":\"MAXRUSE\",\"propertyTypeName\":\"Maximum Seat Re-use\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Taken Before and After\",\"physicalProperty\":1,\"propertyCode\":\"TBAA\",\"propertyTypeCode\":\"MAXRUSE\",\"propertyTypeName\":\"Maximum Seat Re-use\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone  1\",\"physicalProperty\":1,\"propertyCode\":\"ZONE1\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone  2\",\"physicalProperty\":1,\"propertyCode\":\"ZONE2\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone  3\",\"physicalProperty\":1,\"propertyCode\":\"ZONE3\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone  4\",\"physicalProperty\":1,\"propertyCode\":\"ZONE4\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone  5\",\"physicalProperty\":1,\"propertyCode\":\"ZONE5\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone  6\",\"physicalProperty\":1,\"propertyCode\":\"ZONE6\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone  7\",\"physicalProperty\":1,\"propertyCode\":\"ZONE7\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone  8\",\"physicalProperty\":1,\"propertyCode\":\"ZONE8\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone  9\",\"physicalProperty\":1,\"propertyCode\":\"ZONE9\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 10\",\"physicalProperty\":1,\"propertyCode\":\"ZONE10\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 11\",\"physicalProperty\":1,\"propertyCode\":\"ZONE11\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 12\",\"physicalProperty\":1,\"propertyCode\":\"ZONE12\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 13\",\"physicalProperty\":1,\"propertyCode\":\"ZONE13\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 14\",\"physicalProperty\":1,\"propertyCode\":\"ZONE14\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 15\",\"physicalProperty\":1,\"propertyCode\":\"ZONE15\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 16\",\"physicalProperty\":1,\"propertyCode\":\"ZONE16\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 17\",\"physicalProperty\":1,\"propertyCode\":\"ZONE17\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 18\",\"physicalProperty\":1,\"propertyCode\":\"ZONE18\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 19\",\"physicalProperty\":1,\"propertyCode\":\"ZONE19\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 20\",\"physicalProperty\":1,\"propertyCode\":\"ZONE20\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 21\",\"physicalProperty\":1,\"propertyCode\":\"ZONE21\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 22\",\"physicalProperty\":1,\"propertyCode\":\"ZONE22\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 23\",\"physicalProperty\":1,\"propertyCode\":\"ZONE23\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 24\",\"physicalProperty\":1,\"propertyCode\":\"ZONE24\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 25\",\"physicalProperty\":1,\"propertyCode\":\"ZONE25\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 26\",\"physicalProperty\":1,\"propertyCode\":\"ZONE26\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 27\",\"physicalProperty\":1,\"propertyCode\":\"ZONE27\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 28\",\"physicalProperty\":1,\"propertyCode\":\"ZONE28\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":2,\"iconContentName\":\"\",\"name\":\"Extra Oxygen\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"OXYGEN\",\"propertyTypeName\":\"Extra Oxygen\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Airport Check-in Only\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"RESTRICT\",\"propertyTypeName\":\"Airport Check-in Only\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":1,\"iconContentName\":\"\",\"name\":\"Smoking\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"SMOKING\",\"propertyTypeName\":\"Smoking\",\"searchable\":1,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Symmetric Seat\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"SYMETR\",\"propertyTypeName\":\"Symmetric Seat\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"First Class\",\"physicalProperty\":1,\"propertyCode\":\"F\",\"propertyTypeCode\":\"TCC\",\"propertyTypeName\":\"Travel Class Code\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Business Class\",\"physicalProperty\":1,\"propertyCode\":\"C\",\"propertyTypeCode\":\"TCC\",\"propertyTypeName\":\"Travel Class Code\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Economy Class\",\"physicalProperty\":1,\"propertyCode\":\"Y\",\"propertyTypeCode\":\"TCC\",\"propertyTypeName\":\"Travel Class Code\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Window\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"WINDOW\",\"propertyTypeName\":\"Window\",\"searchable\":1,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":1},";
        interface_createunitmap += "{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Over Wing\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"WING\",\"propertyTypeName\":\"Over Wing\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":1}]},";

        interface_createunitmap += "\"ssrCodeListJson\": {\"SsrCodeList\":[]},";
        interface_createunitmap += "\"numericPropertyCodeListJson\": {\"PropertyCodeList\":[\"BRDZONE\",\"CONID\",\"HEIGHT\",\"LR\",\"PAYLOAD\",\"SRVZONE\",\"UNITGRP\",\"WIDTH\",\"WTBAL\"]},";
        interface_createunitmap += "\"numericPropertyListJson\": {\"FlattenedPropertyTypeList\":[{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Boarding Zone\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"BRDZONE\",\"propertyTypeName\":\"Boarding Zone\",\"searchable\":0,\"valueMax\":99,\"valueMin\":0,\"valueType\":\"Numeric\",\"usedOnMap\":1},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Bed Container ID\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"CONID\",\"propertyTypeName\":\"Bed Container ID\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"Numeric\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Height\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"HEIGHT\",\"propertyTypeName\":\"Height\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"Numeric\",\"usedOnMap\":1},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Ruler Label Description\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"LR\",\"propertyTypeName\":\"Ruler Label Description\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"Numeric\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Equipment Payload\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"PAYLOAD\",\"propertyTypeName\":\"Equipment Payload\",\"searchable\":0,\"valueMax\":32767,\"valueMin\":0,\"valueType\":\"Numeric\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Service Zone\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"SRVZONE\",\"propertyTypeName\":\"Service Zone\",\"searchable\":0,\"valueMax\":99,\"valueMin\":0,\"valueType\":\"Numeric\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Seat Group\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"UNITGRP\",\"propertyTypeName\":\"Seat Group\",\"searchable\":0,\"valueMax\":99,\"valueMin\":0,\"valueType\":\"Numeric\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Width\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"WIDTH\",\"propertyTypeName\":\"Width\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"Numeric\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Weight/Balance Zone\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"WTBAL\",\"propertyTypeName\":\"Weight/Balance Zone\",\"searchable\":0,\"valueMax\":99,\"valueMin\":0,\"valueType\":\"Numeric\",\"usedOnMap\":1}]}";
        interface_createunitmap += "},";

        a = xmlhttp.responseXML.documentElement.getElementsByTagName("EquipmentInfos");
        b = a[0].getElementsByTagName("CompartmentInfo");
        c = b[0].getElementsByTagName("Width");
        maxwidth = 0;
        for (var i = 0; i < c.length; i++) {
            if(parseInt(maxwidth,10) < parseInt(c[i].firstChild.nodeValue,10)) {
                maxwidth = c[i].firstChild.nodeValue;
            }
        }
        c = maxwidth;
        
        d = a[0].getElementsByTagName("DepartureStation");
        if (d[0].firstChild == null) d = "";
        else d = d[0].firstChild.nodeValue;

        e = a[0].getElementsByTagName("EquipmentCategory");
        if (e[0].firstChild == null) e = "";
        else e = e[0].firstChild.nodeValue;

        f = a[0].getElementsByTagName("EquipmentType");
        if (f[0].firstChild == null) f = "";
        else f = f[0].firstChild.nodeValue;

        g = a[0].getElementsByTagName("EquipmentTypeSuffix");
        if (g[0].firstChild == null) g = "";
        else g = g[0].firstChild.nodeValue;

        h = a[0].getElementsByTagName("MarketingCode");
        if (h[0].firstChild == null) h = "";
        else h = h[0].firstChild.nodeValue;

        j = a[0].getElementsByTagName("AvailableUnits");
        if (j[0].firstChild == null) j = "";
        else j = j[0].firstChild.nodeValue;

        interface_createunitmap += "{";
        interface_createunitmap += "\"equipmentJson\": {\"piu\":[3333,786432],\"as\":\"\",\"c\":[],\"ds\":\"" + d + "\",\"ec\":\"JetAircraft\",\"et\":\"" + f + "\",\"ets\":\"" + g + "\",\"mc\":\"" + h + "\",\"au\":" + j + ",\"is\":0,\"ia\":0},";

        interface_createunitmap += "\"flattenedPropertyTypeListJson\": {\"FlattenedPropertyTypeList\":[{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Aisle\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"AISLE\",\"propertyTypeName\":\"Aisle\",\"searchable\":1,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":1},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Blocked / Inoperable\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"BLOCKED\",\"propertyTypeName\":\"Blocked / Inoperable\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Bulkhead\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"BULKHEAD\",\"propertyTypeName\":\"Bulkhead\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":1},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Forward Facing Seat\",\"physicalProperty\":0,\"propertyCode\":\"FRWRD\",\"propertyTypeCode\":\"DIR\",\"propertyTypeName\":\"Seat Direction\",\"searchable\":1,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Backward Facing Seat\",\"physicalProperty\":0,\"propertyCode\":\"NFRWRD\",\"propertyTypeCode\":\"DIR\",\"propertyTypeName\":\"Seat Direction\",\"searchable\":1,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":4,\"iconContentName\":\"\",\"name\":\"Person with Disability\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"DISABIL\",\"propertyTypeName\":\"Person with Disability\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Dynamic Labeling\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"DYNLAB\",\"propertyTypeName\":\"Dynamic Labeling\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Near Engine\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"ENGINE\",\"propertyTypeName\":\"Near Engine\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Emergency Exit\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"EXITROW\",\"propertyTypeName\":\"Emergency Exit\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":1},{\"displayPriority\":5,\"iconContentName\":\"root/Equipment/SeatMap/Infant\",\"name\":\"Infant Allowed\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"INFANT\",\"propertyTypeName\":\"Infant Allowed\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":0},{\"displayPriority\":3,\"iconContentName\":\"\",\"name\":\"Near Lavatory\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"LAVATORY\",\"propertyTypeName\":\"Near Lavatory\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":1},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Extra Leg Room\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"LEGROOM\",\"propertyTypeName\":\"Extra Leg Room\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":1},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Free Seat\",\"physicalProperty\":1,\"propertyCode\":\"FREE\",\"propertyTypeCode\":\"MAXRUSE\",\"propertyTypeName\":\"Maximum Seat Re-use\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Taken Before or After\",\"physicalProperty\":1,\"propertyCode\":\"TBOA\",\"propertyTypeCode\":\"MAXRUSE\",\"propertyTypeName\":\"Maximum Seat Re-use\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Taken Before and After\",\"physicalProperty\":1,\"propertyCode\":\"TBAA\",\"propertyTypeCode\":\"MAXRUSE\",\"propertyTypeName\":\"Maximum Seat Re-use\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone  1\",\"physicalProperty\":1,\"propertyCode\":\"ZONE1\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone  2\",\"physicalProperty\":1,\"propertyCode\":\"ZONE2\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone  3\",\"physicalProperty\":1,\"propertyCode\":\"ZONE3\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone  4\",\"physicalProperty\":1,\"propertyCode\":\"ZONE4\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone  5\",\"physicalProperty\":1,\"propertyCode\":\"ZONE5\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone  6\",\"physicalProperty\":1,\"propertyCode\":\"ZONE6\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone  7\",\"physicalProperty\":1,\"propertyCode\":\"ZONE7\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone  8\",\"physicalProperty\":1,\"propertyCode\":\"ZONE8\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone  9\",\"physicalProperty\":1,\"propertyCode\":\"ZONE9\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 10\",\"physicalProperty\":1,\"propertyCode\":\"ZONE10\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 11\",\"physicalProperty\":1,\"propertyCode\":\"ZONE11\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 12\",\"physicalProperty\":1,\"propertyCode\":\"ZONE12\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 13\",\"physicalProperty\":1,\"propertyCode\":\"ZONE13\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 14\",\"physicalProperty\":1,\"propertyCode\":\"ZONE14\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 15\",\"physicalProperty\":1,\"propertyCode\":\"ZONE15\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 16\",\"physicalProperty\":1,\"propertyCode\":\"ZONE16\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 17\",\"physicalProperty\":1,\"propertyCode\":\"ZONE17\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 18\",\"physicalProperty\":1,\"propertyCode\":\"ZONE18\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 19\",\"physicalProperty\":1,\"propertyCode\":\"ZONE19\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 20\",\"physicalProperty\":1,\"propertyCode\":\"ZONE20\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 21\",\"physicalProperty\":1,\"propertyCode\":\"ZONE21\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 22\",\"physicalProperty\":1,\"propertyCode\":\"ZONE22\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 23\",\"physicalProperty\":1,\"propertyCode\":\"ZONE23\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 24\",\"physicalProperty\":1,\"propertyCode\":\"ZONE24\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 25\",\"physicalProperty\":1,\"propertyCode\":\"ZONE25\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 26\",\"physicalProperty\":1,\"propertyCode\":\"ZONE26\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 27\",\"physicalProperty\":1,\"propertyCode\":\"ZONE27\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Zone 28\",\"physicalProperty\":1,\"propertyCode\":\"ZONE28\",\"propertyTypeCode\":\"ODZONE\",\"propertyTypeName\":\"O&D Zone\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":2,\"iconContentName\":\"\",\"name\":\"Extra Oxygen\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"OXYGEN\",\"propertyTypeName\":\"Extra Oxygen\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Airport Check-in Only\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"RESTRICT\",\"propertyTypeName\":\"Airport Check-in Only\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":0},{\"displayPriority\":1,\"iconContentName\":\"\",\"name\":\"Smoking\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"SMOKING\",\"propertyTypeName\":\"Smoking\",\"searchable\":1,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Symmetric Seat\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"SYMETR\",\"propertyTypeName\":\"Symmetric Seat\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"First Class\",\"physicalProperty\":1,\"propertyCode\":\"F\",\"propertyTypeCode\":\"TCC\",\"propertyTypeName\":\"Travel Class Code\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Business Class\",\"physicalProperty\":1,\"propertyCode\":\"C\",\"propertyTypeCode\":\"TCC\",\"propertyTypeName\":\"Travel Class Code\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Economy Class\",\"physicalProperty\":1,\"propertyCode\":\"Y\",\"propertyTypeCode\":\"TCC\",\"propertyTypeName\":\"Travel Class Code\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"List\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Window\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"WINDOW\",\"propertyTypeName\":\"Window\",\"searchable\":1,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":1},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Over Wing\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"WING\",\"propertyTypeName\":\"Over Wing\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"YesNo\",\"usedOnMap\":1}]},";
        interface_createunitmap += "\"ssrCodeListJson\": {\"SsrCodeList\":[]},";
        interface_createunitmap += "\"numericPropertyCodeListJson\": {\"PropertyCodeList\":[\"BRDZONE\",\"CONID\",\"HEIGHT\",\"LR\",\"PAYLOAD\",\"SRVZONE\",\"UNITGRP\",\"WIDTH\",\"WTBAL\"]},";
        interface_createunitmap += "\"numericPropertyListJson\": {\"FlattenedPropertyTypeList\":[{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Boarding Zone\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"BRDZONE\",\"propertyTypeName\":\"Boarding Zone\",\"searchable\":0,\"valueMax\":99,\"valueMin\":0,\"valueType\":\"Numeric\",\"usedOnMap\":1},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Bed Container ID\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"CONID\",\"propertyTypeName\":\"Bed Container ID\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"Numeric\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Height\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"HEIGHT\",\"propertyTypeName\":\"Height\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"Numeric\",\"usedOnMap\":1},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Ruler Label Description\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"LR\",\"propertyTypeName\":\"Ruler Label Description\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"Numeric\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Equipment Payload\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"PAYLOAD\",\"propertyTypeName\":\"Equipment Payload\",\"searchable\":0,\"valueMax\":32767,\"valueMin\":0,\"valueType\":\"Numeric\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Service Zone\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"SRVZONE\",\"propertyTypeName\":\"Service Zone\",\"searchable\":0,\"valueMax\":99,\"valueMin\":0,\"valueType\":\"Numeric\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Seat Group\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"UNITGRP\",\"propertyTypeName\":\"Seat Group\",\"searchable\":0,\"valueMax\":99,\"valueMin\":0,\"valueType\":\"Numeric\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Width\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"WIDTH\",\"propertyTypeName\":\"Width\",\"searchable\":0,\"valueMax\":0,\"valueMin\":0,\"valueType\":\"Numeric\",\"usedOnMap\":0},{\"displayPriority\":0,\"iconContentName\":\"\",\"name\":\"Weight/Balance Zone\",\"physicalProperty\":1,\"propertyCode\":\"\",\"propertyTypeCode\":\"WTBAL\",\"propertyTypeName\":\"Weight/Balance Zone\",\"searchable\":0,\"valueMax\":99,\"valueMin\":0,\"valueType\":\"Numeric\",\"usedOnMap\":1}]}";
        interface_createunitmap += "}";
        interface_createunitmap += "]";

        interface_createunitmap += ",";
        interface_createunitmap += "\"grid\":16,";
        interface_createunitmap += "\"iconMax\": 5,";
        interface_createunitmap += "\"iconMaxLarge\": 5,";
        interface_createunitmap += "\"equipmentCategory\": \"JetAircraft\",";
        interface_createunitmap += "\"soldSsrHash\": ";
        interface_createunitmap += "{}";
        interface_createunitmap += ",";
        interface_createunitmap += "\"showSsrContainerOnInit\":0,";
        interface_createunitmap += "\"submitButtonId\": \"ControlGroupUnitMapView_UnitMapViewControl_LinkButtonAssignUnit\",";
        interface_createunitmap += "\"skipButtonId\": \"ControlGroupUnitMapView_UnitMapViewControl_ButtonSkip\",";
        interface_createunitmap += "\"removeSegmentSeatsId\": \"removeSegmentSeats\",";
        interface_createunitmap += "\"removeAutoSeatsMsg\": \"Are you sure you would like to remove your seat selection(s)? By removing your seat selection(s), we would not be able to guarantee your preferred seat selection or group seating.\",";
        interface_createunitmap += "\"freeLabel\": \"FREE\",";
        interface_createunitmap += "\"showPremiumAlert\": \"\",";
        interface_createunitmap += "\"premiumAlertMsg\": \"Premium Flat Bed seats are only available on D7 flights; whereas AK, FD & QZ flights are fully equipped with Standard seats only.\",";
        interface_createunitmap += "\"unitInputArray\": [";
        <%=self_no %>
        interface_createunitmap += "]";
    }

    //-------------------------------------------------------------------

    //JM : URL NEED TO MODIFY TO REFLECT
    function ajax_input() {
        var xmlhttp;
        if (window.XMLHttpRequest) { xmlhttp = new XMLHttpRequest(); }
        else { xmlhttp = new ActiveXObject("Microsoft.XMLHTTP"); }
        xmlhttp.onreadystatechange = function () {
            if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
                //overwriting
                //alert(xmlhttp.responseText);
                prepare_interfacing_variable(xmlhttp);
                completed = 1;
            }
        }

        xmlhttp.open("GET", id$value("s_xmlurl"), true);
        xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
        xmlhttp.send();
        return false;
    }

    function gotoindex(x) {
        id$("currentindex").value = x;
    }

    call_interface_createunitmap();
    ajax_input();
</script>

<div id="unitTipId" class="unitTip hiddens">&lt;h3&gt; Information; &lt;/h3&gt; 
&lt;p&gt; Number: [unitDesignator] &lt;/p&gt; &lt;p&gt; Status: 
[unitAvailability] &lt;/p&gt; &lt;p&gt; 
Group: [unitGroup] &lt;/p&gt; &lt;p&gt; 
Fee: [unitFee] &lt;/p&gt; &lt;p&gt; 
[unitRemark] &lt;/p&gt; &lt;h3&gt; Amenities: &lt;/h3&gt; [propertyArray] 
[ssrPermissionArray] [numericPropertyArray] </div>
<div id="booleanPropertyUnitTipId" class="hidden">&lt;p&gt; &lt;img 
src="[IconContentName].gif" alt="" /&gt; [name] &lt;/p&gt; </div>
<div id="numericPropertyUnitTipId" class="hidden">&lt;p&gt; [name]: [value] 
&lt;/p&gt; </div>
<div id="booleanPropertyUnitId" class="hidden">&lt;img class="[className]" 
src="[IconContentName].gif" alt="" /&gt; </div>
<div id="seatId" class="hidden">&lt;div id="[unitKey]" class="aUnit 
unitGroup[unitGroup]" style="top:[y]px; left:[x]px; width:[width]px; 
height:[height]px;"&gt; &lt;!--&lt;acronym&gt;[unitDesignator]&lt;/acronym&gt; 
&lt;acronym&gt;[unitDesignator]&lt;/acronym&gt;--&gt; [propertyArray] 
&lt;/div&gt; </div>
<div id="largeSeatId" class="hidden">&lt;div id="[unitKey]" class="aUnit 
unitGroup[unitGroup]" style="top:[y]px; left:[x]px; width:[width]px; 
height:[height]px;" &gt; &lt;acronym&gt;[unitDesignator]&lt;/acronym&gt; 
&lt;acronym&gt;[unitDesignator]&lt;/acronym&gt; [propertyArray] &lt;/div&gt; 
</div>
<div id="labelRulerId" class="hidden">&lt;div id="[unitKey]" class="unit" 
style="top:[y]px; left:[x]px; width:[width]px; height:[height]px;" &gt; 
&lt;acronym&gt;[text]&lt;/acronym&gt; &lt;/div&gt; </div>
<div id="lavatoryId" class="hidden">&lt;div id="[unitKey]" class="unit noRepeat 
fillBackground" style="top:[y]px; left:[x]px; width:[width]px; 
height:[height]px; " &gt; &lt;/div&gt; </div>
<div id="luggageId" class="hidden">&lt;div id="[unitKey]" class="unit noRepeat 
fillBackground" style="top:[y]px; left:[x]px; width:[width]px; 
height:[height]px; " &gt; &lt;/div&gt; </div>
<div id="bulkHeadId" class="hidden">&lt;div id="[unitKey]" class="unit 
tile[unitAngle]" style="top:[y]px; left:[x]px; width:[width]px; 
height:[height]px;" &gt; &lt;/div&gt; </div>
<div id="wingId" class="hidden"></div>
<div id="genericUnitId" class="hidden">&lt;div id="[unitKey]" class="unit noRepeat" 
style="top:[y]px; left:[x]px; width:[width]px; height:[height]px;" &gt; 
&lt;/div&gt; </div>
<div id="seatCompartmentId" class="hidden">&lt;div id="[unitKey]" class="aUnit" 
style="top:[y]px; left:[x]px; width:[width]px; height:[height]px; border: 1px 
solid black;"&gt; &lt;/div&gt; </div>
<div id="windowId" class="hidden">&lt;div id="[unitKey]" class="unit" 
style="top:[y]px; left:[x]px; width:[width]px; height:[height]px;" &gt; 
&lt;/div&gt; </div>
<div id="tableId" class="hidden">&lt;div id="[unitKey]" class="aUnit noRepeat" 
style="top:[y]px; left:[x]px; width:[width]px; height:[height]px;" &gt; 
&lt;/div&gt; </div>
<div id="exitId" class="hidden">&lt;div id="[unitKey]" class="unit" 
style="top:[y]px; left:[x]px; width:[width]px; height:[height]px;" &gt; 
&lt;/div&gt; </div>
<div id="wallId" class="hidden">&lt;div id="[unitKey]" class="unit" 
style="top:[y]px; left:[x]px; width:[width]px; height:[height]px;" &gt; 
&lt;/div&gt; </div>
<div id="doorId" class="hidden">&lt;div id="[unitKey]" class="unit" 
style="top:[y]px; left:[x]px; width:[width]px; height:[height]px;" &gt; 
&lt;/div&gt; </div>
<div id="stairsId" class="hidden">&lt;div id="[unitKey]" class="unit" 
style="top:[y]px; left:[x]px; width:[width]px; height:[height]px;" &gt; 
&lt;/div&gt; </div>
<div id="chouchetteId" class="hidden">&lt;div id="[unitKey]" class="aUnit" &gt; 
&lt;/div&gt; </div>
<div id="bedOneOfOneId" class="hidden">&lt;div id="[unitKey]" class="aUnit 
unitGroup[unitGroup]" style="top:[y]px; left:[x]px; width:[width]px; 
height:[height]px;" &gt; &lt;acronym&gt;[unitDesignator]&lt;/acronym&gt; 
&lt;acronym&gt;[unitDesignator]&lt;/acronym&gt; &lt;/div&gt; </div>
<div id="bedOneOfTwoId" class="hidden">&lt;div id="[unitKey]" class="aUnit 
unitGroup[unitGroup]" style="top:[y]px; left:[x]px; width:[width]px; 
height:[height]px;" &gt; &lt;acronym&gt;[unitDesignator]&lt;/acronym&gt; 
&lt;acronym&gt;[unitDesignator]&lt;/acronym&gt; &lt;/div&gt; </div>
<div id="bedTwoOfTwoId" class="hidden">&lt;div id="[unitKey]" class="aUnit 
unitGroup[unitGroup]" style="top:[y]px; left:[x]px; width:[width]px; 
height:[height]px;" &gt; &lt;acronym&gt;[unitDesignator]&lt;/acronym&gt; 
&lt;acronym&gt;[unitDesignator]&lt;/acronym&gt; &lt;/div&gt; </div>
<div id="bedOneOfThreeId" class="hidden">&lt;div id="[unitKey]" class="aUnit 
unitGroup[unitGroup]" style="top:[y]px; left:[x]px; width:[width]px; 
height:[height]px;" &gt; &lt;acronym&gt;[unitDesignator]&lt;/acronym&gt; 
&lt;acronym&gt;[unitDesignator]&lt;/acronym&gt; &lt;/div&gt; </div>
<div id="bedTwoOfThreeId" class="hidden">&lt;div id="[unitKey]" class="aUnit 
unitGroup[unitGroup]" style="top:[y]px; left:[x]px; width:[width]px; 
height:[height]px;" &gt; &lt;acronym&gt;[unitDesignator]&lt;/acronym&gt; 
&lt;acronym&gt;[unitDesignator]&lt;/acronym&gt; &lt;/div&gt; </div>
<div id="bedThreeOfThreeId" class="hidden">&lt;div id="[unitKey]" class="aUnit 
unitGroup[unitGroup]" style="top:[y]px; left:[x]px; width:[width]px; 
height:[height]px;" &gt; &lt;acronym&gt;[unitDesignator]&lt;/acronym&gt; 
&lt;acronym&gt;[unitDesignator]&lt;/acronym&gt; &lt;/div&gt; </div>
<div id="booleanFilterPropertyId" class="hidden">&lt;p class="formCheckbox"&gt; 
&lt;input type="checkbox" name="[k]_[v]" value="1" /&gt; &lt;label&gt;[k]: 
[v]&lt;/label&gt; &lt;/p&gt; </div>
<div id="ssrId" class="hidden">&lt;p&gt; &lt;input type="checkbox" name="[ssrCode]" 
id="[ssrKey]_checkbox" value="1" /&gt; &lt;label 
for="[ssrKey]_checkbox"&gt;[name] [passengerFee]&lt;/label&gt; &lt;/p&gt; </div>
<div id="ssrFeeId" class="hidden">[passengerFee] </div>
<div id="lostSsrId" class="hidden">&lt;p&gt; &lt;label&gt;[name]&lt;/label&gt; 
&lt;/p&gt; </div>
<div id="unfulfilledPropertyId" class="hidden">&lt;p&gt; 
&lt;label&gt;[value]&lt;/label&gt; &lt;/p&gt; </div><span 
id="showUpSellInputContainer"></span><span 
id="selectedFilterPropertyInputContainer"></span><span 
id="selectedAutoAssignPropertyInputContainer"></span><span 
id="selectedFilterSsrInputContainer"></span><span id="soldSsrInputContainer"></span>
<div id="confirmSeat" class="hidden upsellSM">
<div class="confirmSeatDetails">
<div id="confirmSeatContainer">
<div id="confirmSeatPaxOptions" class="confirmSeatSM">&lt;h6&gt; [passengerName]: </div></div></div></div>
