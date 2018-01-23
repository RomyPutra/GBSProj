<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="adminselectflight.aspx.cs" Inherits="GroupBooking.Web.admin.adminselectflight" MasterPageFile="~/Master/NewAdminMaster.Master" %>

<%@ MasterType  virtualPath="~/Master/PageMaster.Master"%>
<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

<script type="text/javascript">
    // <![CDATA[

    function OnCallbackComplete(s, e) {
        if (e.result != "") {
            //lblmsg.SetValue(e.result);
            document.getElementById("ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = e.result;
            pcMessage.Show();
            LoadingPanel.Hide();
        } else {
            window.location.href = '../admin/adminmain.aspx';
        }

    }

    function ShowLoginWindow() {
        pcMessage.Show();
    }



    function singleSelect(obj, dlistName) {
        //var elem = this.SkySales.elements; // obj.form.elements;
        var elem = document.getElementById("SkySales");
        var datalistName = dlistName;
        var str;

        for (var i = 0; i < elem.length; i++) {
            str += elem[i].id + "=" + obj.id + "," + elem[i].name.split('$')[2] + "=" + datalistName + "\n";
            if (elem[i].type == "radio" && elem[i].id != obj.id && elem[i].name.split('$')[2] == datalistName)// obj.name.subString( 0 , elem[i].name.indexOf('$') )
            {
                elem[i].checked = false; //把不是触发click事件的radio状态设置为未选 12.       
            }
        }
        LoadingPanel.SetText('Please wait...');
        LoadingPanel.Show();
        setTimeout(function () { LoadingPanel.Hide() }, 2000);

    }
    function clickit() {
        var dom = document.getElementById(tb_Departure).childNodes; //   document.getElementById("MainContent_DataList1");
        var el = event.srcElement;
        if (el.tagName == "INPUT" && el.type.toLowerCase() == "radio") {
            for (i = 0; i < dom.length; i++) {
                if (dom[i].tagName == "INPUT" && dom[i].type.toLowerCase() == "radio") {
                    dom[i].checked = false;
                }
            }
        }
        el.checked = true;
    }
    function clickit2() {
        var dom = document.getElementById(tb_Return).childNodes; // document.getElementById("tr_Return");
        var el = event.srcElement;
        if (el.tagName == "INPUT" && el.type.toLowerCase() == "radio") {
            for (i = 0; i < dom.length; i++) {
                if (dom[i].tagName == "INPUT" && dom[i].type.toLowerCase() == "radio") {
                    dom[i].checked = false;
                }
            }
        }
        el.checked = true;
    }
    // ]]> 
    </script>

<msg:msgControl ID="msgcontrol" runat="server" />
<div>
<dx:ASPxCallback ID="CallBackValidation" ClientInstanceName="CallBackValidation" runat="server" OnCallback="ValidatePopup">
        <ClientSideEvents CallbackComplete="OnCallbackComplete" />
    </dx:ASPxCallback>
</div>

<table cellpadding="0" cellspacing="0" width="850px">
        <tr>
            <td class="tdblank">
                &nbsp;
            </td>
            <td class="tdright">
                <div class="div">
                    Select flights</div>
                <hr />

                <br />
                <table class="tableright" cellpadding="0" cellspacing="0" width="800px">
                    <tr class="tr">
                        <td class="flightbackcolor1">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Airasia/icon_plane.gif" />&nbsp;<font
                                class="goingout">Going out:</font>&nbsp;<asp:Label ID="lbl_Go1" runat="server"></asp:Label>
                            to
                            <asp:Label ID="lbl_Go2" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <table width="100%">
                    <tr>
                        <td>
                            <asp:DataList ID="DataList1" runat="server" Width="100%">
                                            <HeaderTemplate>
                                                <table width="100%">
                                                    <tr class="tdcol" align="left">
                                                                    
                                                                    <td colspan="2" width="100px">
                                                                        &nbsp;&nbsp;&nbsp;Flight Number
                                                                    </td>
                                                                    <td colspan="2" width="200px">
                                                                        Departure
                                                                    </td>
                                                                    
                                                                    <td colspan="2" width="200px">
                                                                         Arrival
                                                                    </td>
                                                                   
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr class="div1">
                                                        <td align="left" colspan="2">&nbsp;&nbsp;&nbsp;
                                                            <%#DataBinder.Eval(Container.DataItem, "TemFlightCarrierCode").ToString()%>
                                                        
                                                            &nbsp;<%#DataBinder.Eval(Container.DataItem, "TemFlightFlightNumber").ToString()%>&nbsp;&nbsp;<%#DataBinder.Eval(Container.DataItem, "TemFlightCarrierCode2").ToString()%>&nbsp;<%#DataBinder.Eval(Container.DataItem, "TemFlightFlightNumber2").ToString()%></td>
                                                        <td align="left">                                                            
                                                            <%#DataBinder.Eval(Container.DataItem, "TemFlightDeparture").ToString()%>                                                                    
                                                        </td>
                                                        <td >
                                                            &nbsp;&nbsp;<%#DataBinder.Eval(Container.DataItem, "TemFlightStd", "{0:dddd, dd MMMM yyyy}")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%#DataBinder.Eval(Container.DataItem, "TemFlightStd", "{0:HHmm}")%></td>
                                                        <td align="left">
                                                            <%#DataBinder.Eval(Container.DataItem, "TemFlightArrival").ToString()%>                                                            
                                                        </td>
                                                        <td >                                                                    
                                                            &nbsp;&nbsp;<%#DataBinder.Eval(Container.DataItem, "TemFlightSta", "{0:dddd, dd MMMM yyyy}")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%#DataBinder.Eval(Container.DataItem, "TemFlightSta", "{0:HHmm}")%></td>
                                                     <td>
                                                        <asp:RadioButton ID="RadioButton1" runat="server" Checked="false" Text="" GroupName="grb1" onclick="javascript:singleSelect(this,'DataList1')" />
                                                        <asp:Label ID="lbl_list1ID" runat="server" Style="display: none" Text='<%#DataBinder.Eval(Container.DataItem, "TemFlightId").ToString()%>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                        </asp:DataList>
                        </td>
                    </tr>
                </table>
                        </td>
                    </tr>
                    <tr class="tr">
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="tr_Return" runat="server"  class="tr">
                        <td style="width:100%">
                            <table width="100%">
                                <tr style="display: none">
                                    <td>
                                        <table cellpadding="0" cellspacing="0" class="table">
                                            <tr>
                                                <td class="td1" rowspan="2">
                                                   
                                                </td>
                                                <td rowspan="2" valign="middle" align="center">
                                                  
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr class="tr">
                                    <td class="flightbackcolor1">
                                        <asp:Image ID="Image4" runat="server" ImageUrl="~/Images/Airasia/icon_plane_left.gif" />&nbsp;<font
                                            class="goingout">Return:</font> &nbsp;
                                        <asp:Label ID="lbl_Return1" runat="server"></asp:Label>
                                        to
                                        <asp:Label ID="lbl_Return2" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr  >
                                    <td id="Td1">
                                        <br />
                                        <table width="100%" id="Table2">
                                            <tr>
                                                <td colspan="3">
                                                    <asp:DataList ID="DataList2" runat="server" Width="100%">
                                                        <HeaderTemplate>
                                                            <table width="100%">
                                                               <tr class="tdcol" align="left">
                                                                   
                                                                    <td colspan="2" width="100px">
                                                                        &nbsp;&nbsp;&nbsp;Flight Number
                                                                    </td>
                                                                    <td colspan="2" width="200px">
                                                                        Departure
                                                                    </td>
                                                                    
                                                                    <td colspan="2" width="200px">
                                                                      Arrival
                                                                    </td>
                                                                   
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr class="div1">
                                                                <td align="left" colspan="2">&nbsp;&nbsp;&nbsp;
                                                            <%#DataBinder.Eval(Container.DataItem, "TemFlightCarrierCode").ToString()%>
                                                        
                                                            &nbsp;<%#DataBinder.Eval(Container.DataItem, "TemFlightFlightNumber").ToString()%>&nbsp;&nbsp;<%#DataBinder.Eval(Container.DataItem, "TemFlightCarrierCode2").ToString()%>&nbsp;<%#DataBinder.Eval(Container.DataItem, "TemFlightFlightNumber2").ToString()%></td>
                                                        <td align="left">                                                            
                                                            <%#DataBinder.Eval(Container.DataItem, "TemFlightDeparture").ToString()%>                                                                    
                                                        </td>
                                                        <td >
                                                            &nbsp;&nbsp;<%#DataBinder.Eval(Container.DataItem, "TemFlightStd", "{0:dddd, dd MMMM yyyy}")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%#DataBinder.Eval(Container.DataItem, "TemFlightStd", "{0:HHmm}")%></td>
                                                        <td align="left">
                                                            <%#DataBinder.Eval(Container.DataItem, "TemFlightArrival").ToString()%>                                                            
                                                        </td>
                                                        <td >                                                                    
                                                            &nbsp;&nbsp;<%#DataBinder.Eval(Container.DataItem, "TemFlightSta", "{0:dddd, dd MMMM yyyy}")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%#DataBinder.Eval(Container.DataItem, "TemFlightSta", "{0:HHmm}")%></td>
                                                                <td>
                                                                    <asp:RadioButton ID="RadioButton2" runat="server" Checked="false" GroupName="grb2" Text="" OnClick="javascript:singleSelect(this,'DataList2')" />
                                                                    <asp:Label ID="lbl_list2ID" runat="server" Style="display: none" Text='<%#DataBinder.Eval(Container.DataItem, "TemFlightId")%>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </table>
                                                        </FooterTemplate>
                                                    </asp:DataList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr class="tr">
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr >
                                    <td>                                         
                                       
                                           
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr class="tr" style="display: none">
                        <td>
                            <font class="flightfont2">Flight operated by: <font color="red">AK - AirAsia </font>
                                | <font color="blue">FD - Thai AirAsia</font> | <font color="#9932CC">QZ - Indonesia
                                    AirAsia</font> | <font color="green">D7 - AirAsia X</font></font>
                        </td>
                    </tr>
                     <tr class="tr">
                        <td >
                            <table width="90%">
                                <tr>
                                    <td>
                                        <dx:ASPxButton CssClass="button_2"  ID="btn_Next" runat="server"  
                                            Text="Continue" AutoPostBack="False" >
                                            <ClientSideEvents Click="function(s, e) {CallBackValidation.PerformCallback();
                                            LoadingPanel.SetText(&#39;Please Wait...&#39;);
                                           LoadingPanel.Show(); }" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                  
                    <tr>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>        
       </table>

</asp:Content> 