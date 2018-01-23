<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewAdminMaster.Master" AutoEventWireup="true" CodeBehind="~/admin/reportview.aspx.cs" Inherits="GroupBooking.Web.admin.reportview" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>

    <script type="text/javascript">
        // <![CDATA[
    

        // ]]>

        //Set Vertical Scrollable Height on Grid (20170309 - Sienny)
        function OnInit(s, e) {
            AdjustSize();
            document.getElementById("gridContainer").style.visibility = "";
        }
        function OnEndCallback(s, e) {
            AdjustSize();
        }
        function OnControlsInitialized(s, e) {
            ASPxClientUtils.AttachEventToElement(window, "resize", function (evt) {
                AdjustSize();
            });
        }
        function AdjustSize() {
            var height = Math.max(0, document.documentElement.clientHeight);
            grid.SetHeight(height);
        }
        //(20170309 - Sienny)
    </script>

    <div id="wrapper4">
        <div class="div">
            <br />
            <dx:ASPxLabel ID="lblHeader" Text="Report View" runat="server" Font-Bold="True" Font-Size="Medium"></dx:ASPxLabel>
        
            <span style="float:right">
                <dx:ASPxButton ID="btnExport" Text="Export to Excel" runat="server" CssClass="button_2" onclick="btnExport_Click"  >
                </dx:ASPxButton>
            </span>
        </div>
        <hr />
    </div>

    <div id="gridContainer" style="visibility: hidden" class="noPadSpace">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" Width="100%">
        <ContentTemplate>
            <dx:ASPxGridViewExporter ID="exporter" runat="server" GridViewID="grid" />

            <dx:ASPxGridView ID="grid" runat="server" ClientInstanceName="grid" OnCustomCallback="grid_CustomCallback" Width="100%" OnDataBinding="grid_DataBinding" 
                OnDataBound="grid_DataBound" EnableViewState="False" KeyFieldName="RptCode" SettingsPager-PageSize="15">
                                    
                <settings showverticalscrollbar="True" VerticalScrollableHeight="0" VerticalScrollBarMode="Auto"/>
                <settings ShowHorizontalScrollBar ="True"/>
                <Styles>
                    <Header BackColor="#333333" ForeColor="White" Wrap="True" HorizontalAlign="Center">
                    </Header>
                </Styles>

                <%--20170309 Sienny --%>
                <ClientSideEvents Init="OnInit" EndCallback="OnEndCallback" />
                <%--<Settings ShowFooter="True" />--%>
                <%--<TotalSummary>
                    <dx:ASPxSummaryItem SummaryType="Sum" DisplayFormat="{0}"/>
                </TotalSummary>--%>

            </dx:ASPxGridView>

            <%--<br />
            <dx:ASPxButton ID="btnExport" Text="Export to Excel" runat="server" CssClass="button_2" onclick="btnExport_Click"  >
            </dx:ASPxButton>--%>
        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>    
    </asp:UpdatePanel> 
    </div>
</asp:Content>


