<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectFlightChange.aspx.cs"
    Inherits="GroupBooking.Web.Booking.SelectFlightChange" MasterPageFile="~/Master/NewPageMasterReport.Master" %>

<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolder2">

<script language="JAVASCRIPT" src="../Scripts/jquery-1.4.1.js"></script>
    <script type="text/javascript">
    // <![CDATA[

        function OnCallbackComplete(s, e) {
            if (e.result != "") {
                //lblmsg.SetValue(e.result);
                if (e.result == "Your session has expired.") {
                    window.location.href = '../Invalid.aspx';
                }
                document.getElementById("ctl00_ContentPlaceHolder2_msgcontrol_pcMessage_pnlMessage_lblmsg").innerHTML = e.result;
                pcMessage.Show();
                LoadingPanel.Hide();
            } else {
                window.location.href = '../public/reviewfarechange.aspx';
            }

        }

        function ShowLoginWindow() {
            pcMessage.Show();
        }

        

        function singleSelect(obj, dlistName) {
            //var elem = this.SkySales.elements; // obj.form.elements;
            
            var elem = document.getElementById("aspnetForm");
            var datalistName = dlistName;
            var str;
            
            for (var i = 0; i < elem.length; i++) {
                str += elem[i].id + "=" + obj.id + "," + elem[i].name.split('$')[2] + "=" + datalistName + "\n";
                if (elem[i].type == "radio" && elem[i].id != obj.id && elem[i].name.split('$')[2] == datalistName)// obj.name.subString( 0 , elem[i].name.indexOf('$') )
                {
                    elem[i].checked = false; //把不是触发click事件的radio状态设置为未选 12.       
                }
            }
//            LoadingPanel.SetText('Please wait...');            
//            LoadingPanel.Show();
//            setTimeout(function () { LoadingPanel.Hide() }, 2000);
            
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
<div id="mainContent" style="width: 100%">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td class="tdright">
                <div class="div">
                    <h2>Select flights</h2></div>
     

                <br />
                
                <!-- added by diana 20130902 -->
                <style type="text/css">
                .tdClass
                {
	                font-family:Arial,Helvetica,san-serif;
	                font-size:12px;
                }
                </style>

                <table class="tableright" cellpadding="0" cellspacing="0" width="100%">
                <tr id="tr_Depart" runat="server"  class="tr">
                    <td style="width:100%">
                        <table width="100%">
                            
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
                       <%--     <asp:DataList ID="DataList1" runat="server" Width="100%">
                                            <HeaderStyle BackColor="#333333" ForeColor="White" />
                                            <HeaderTemplate>--%>
                                                <table width="100%">
                                                    <tr  align="left" style="height:30px;background-color:#333333;color:White;font-size:12px;">
                                                                    
                                                                    <td colspan="2" width="77px">
                                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Flight No</td>
                                                                    <td colspan="2" width="163px">
                                                                        Departure
                                                                    </td>
                                                                    
                                                                    <td colspan="2" width="200px">
                                                                         Arrival
                                                                    </td>
                                                                   
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                          <tr>
                                          <td colspan="7">
                                          <dx:ASPxDataView ID="dvSelectFlight" runat="server" Width="100%" 
                                                  PagerSettings-Visible="false" ColumnCount="1" Height="50px" AllowPaging="False" 
                                                  PagerAlign="Left" CssClass="tdClass">

<PagerSettings Visible="False"></PagerSettings>

                                          <ItemStyle Height="0px" HorizontalAlign="Left" Wrap="True" Width="100%" >
                                            <Paddings PaddingLeft="1px" PaddingRight="1px" PaddingTop="5px" PaddingBottom="5px"></Paddings>
                                            
                                          </ItemStyle>
                                            <ItemTemplate>
                                                <table width="100%"  cellpadding="0" cellspacing="0">
                                                <tbody>
                                                <tr >                                      
                                                        <td align="left" colspan="2">&nbsp;&nbsp;&nbsp;
                                                            <%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightCarrierCode").ToString()%>
                                                        
                                                            &nbsp;<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightFlightNumber").ToString()%>&nbsp;&nbsp;<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightCarrierCode2").ToString()%>&nbsp;<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightFlightNumber2").ToString()%></td>
                                                        <td align="left">                                                            
                                                            <%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightDeparture").ToString()%>                                                                    
                                                        </td>
                                                        <td align="left">
                                                            <%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightStd", "{0:dddd, dd MMMM yyyy}")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightStd", "{0:HHmm}")%></td>
                                                        <td align="left">
                                                            <%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightArrival").ToString()%>                                                            
                                                        </td>
                                                        <td align="left">
                                                        <%# (DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightTransit").ToString() != "") ? DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightSta2", "{0:dddd, dd MMMM yyyy}") : DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightSta", "{0:dddd, dd MMMM yyyy}")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%# (DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightTransit").ToString() != "") ? DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightSta2", "{0:HHmm}") : DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightSta", "{0:HHmm}")%></td>
                                                     <td>
                                                        <asp:RadioButton ID="RadioButton1" runat="server" Checked="false" Text="" GroupName="grb1" onclick="javascript:singleSelect(this,'dvSelectFlight')" />
                                                        <asp:HiddenField ID="lbl_list1ID" runat="server" Value='<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightId").ToString()%>'></asp:HiddenField>
                                                    </td>
                                                </tr>
                                                </tbody> 
                                                </table>
                                            </ItemTemplate>                       
                                           </dx:ASPxDataView>                     
                                   <%--         </HeaderTemplate>
                                          
                       
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                        </asp:DataList>--%>
                        </td>
                    </tr>
                </table>
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
                                <tr style="height:30px;background-color:#333333;color:White;font-size:small;">
                                    <td colspan="3">
                                                <table width="100%" >
                                                    <tr class="tdcol" align="left">
                                                                   
                                                        <td colspan="2" width="77px">
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Flight No</td>
                                                        <td colspan="2" width="163px">
                                                            Departure
                                                        </td>
                                                                    
                                                        <td colspan="2" width="200px">
                                                            Arrival
                                                        </td>
                                                                   
                                                        <td>
                                                                    
                                                        </td>
                                                    </tr>
                                    </table>
                                    </td> 
                                    </tr> 
                                    <tr>
                                    <td width="100%"> 
                                    <dx:ASPxDataView ID="gvSelectFlightReturn" runat="server" Width="100%" 
                                        PagerSettings-Visible="false" ColumnCount="1" Height="50px" AllowPaging="False" 
                                        PagerAlign="Left" CssClass="tdClass">

                                <PagerSettings Visible="False"></PagerSettings>

                                <ItemStyle Height="0px" HorizontalAlign="Left" Wrap="True" Width="100%" >
                                <Paddings PaddingLeft="1px" PaddingRight="1px" PaddingTop="5px" PaddingBottom="5px"></Paddings>
                                            
                                </ItemStyle>
                                <ItemTemplate>
                                    <table width="100%"  cellpadding="0" cellspacing="0">
                                    <tbody>
                                    <tr >
                                                    <td align="left" colspan="2">&nbsp;&nbsp;&nbsp;
                                                <%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightCarrierCode").ToString()%>
                                                        
                                                &nbsp;<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightFlightNumber").ToString()%>&nbsp;&nbsp;<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightCarrierCode2").ToString()%>&nbsp;<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightFlightNumber2").ToString()%></td>
                                            <td align="left" >                                                            
                                                <%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightDeparture").ToString()%>                                                                    
                                            </td>
                                            <td align="left">
                                                <%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightStd", "{0:dddd, dd MMMM yyyy}")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightStd", "{0:HHmm}")%></td>
                                            <td align="left" >
                                                <%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightArrival").ToString()%>                                                            
                                            </td>
                                            <td align="left">                                                                    
                                                <%#(DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightTransit").ToString() != "") ? DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightSta2", "{0:dddd, dd MMMM yyyy}") : DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightSta", "{0:dddd, dd MMMM yyyy}")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%#(DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightTransit").ToString() != "") ? DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightSta2", "{0:HHmm}") : DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightSta", "{0:HHmm}")%></td>
                                                    <td >
                                                        <asp:RadioButton ID="RadioButton2" runat="server" Checked="false" GroupName="grb2" Text="" OnClick="javascript:singleSelect(this,'gvSelectFlightReturn')" />
                                                        <asp:HiddenField ID="lbl_list2ID" runat="server" Value='<%#DataBinder.Eval(((IDataItemContainer)Container).DataItem, "TemFlightId").ToString()%>'></asp:HiddenField>
                                                    </td>
                                                </tr>
                                                </tbody>
                                                </table> 
                                    </ItemTemplate>          
                                </dx:ASPxDataView> 
                                    </td>
                                    </tr>
                                    </table>
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
                                        <dx:ASPxButton CssClass="buttonL"  ID="btn_Next" runat="server"  
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
</div>           
       
       
       
</asp:Content>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder3" ></asp:Content>