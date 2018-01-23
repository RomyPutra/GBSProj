<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewPageMaster.Master" AutoEventWireup="true" CodeBehind="seat.aspx.cs" Inherits="GroupBooking.Web.Seat" %>

<%@ Register Src="~/control/Message.ascx" TagName="msgControl" TagPrefix="msg" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
<script src="../Scripts/overlay.js" type="text/javascript"></script>
    
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
            <div class="widget-body">
            <div class=" col-sm-4">
                    <div style="padding: 15px 0;">
            <span class="dxeBase_GBS" id="RootHolder_SideHolder_lblHeader" style="font-size:Medium;font-weight:normal;">PASSENGER LIST</span><span style="padding-left: 20px;font-weight: 700;font-size: 20px;">EB64VW</span>
        </div>
                                            <div>
                         <table id="tblPNRSeatSelection" class="table-striped inputToFocus">
                            <tr><td>Wiliam, Victor</td><td><div class="checkbox" style="width: 56px;float: right;"><input type="text" class="form-control" value=""/></div></td></tr>
                            <tr><td>Ricardo, Silvia</td><td><div class="checkbox" style="width: 56px;float: right;"><input type="text" class="form-control" value=""/></div></td></tr>
                            <tr><td>Hernandez, Jenny</td><td><div class="checkbox" style="width: 56px;float: right;"><input type="text" class="form-control"
                                value=""/></div></td></tr>
                            <tr><td>Meyer,John</td><td><div class="checkbox" style="width: 56px;float: right;"><input type="text" class="form-control"
                                value=""/></div></td></tr>
                            <tr><td>Junior,Patrick</td><td><div class="checkbox" style="width: 56px;float: right;"><input type="text" class="form-control"
                                value=""/></div></td></tr>
                            <tr><td>Monica,Silvy</td><td><div class="checkbox" style="width: 56px;float: right;"><input type="text" class="form-control"
                                value=""/></div></td></tr>
                            <tr><td>Anthony,Bryan</td><td><div class="checkbox" style="width: 56px;float: right;"><input type="text" class="form-control"
                                value=""/></div></td></tr>
                            <tr><td>Hasim,Muhktar</td><td><div class="checkbox" style="width: 56px;float: right;"><input type="text" class="form-control"
                                value=""/></div></td></tr>
                             <tr><td>Kamaruddin,Daud</td><td><div class="checkbox" style="width: 56px;float: right;"><input type="text" class="form-control"
                                value=""/></div></td></tr>
                             <tr><td>Tahir,Hamidah</td><td><div class="checkbox" style="width: 56px;float: right;"><input type="text" class="form-control"
                                value=""/></div></td></tr>
                        </table>
                            </div>
            </div>
            <div class="col-sm-8">
<div id="seatMapMainBody" xmlns:formatterextension="urn:navitaire:formatters:currency">
                <div id="ControlGroupUnitMapView"><input type="hidden" name="ControlGroupUnitMapView$UnitMapViewControl$compartmentDesignatorInput" id="ControlGroupUnitMapView_UnitMapViewControl_compartmentDesignatorInput"><input type="hidden" name="ControlGroupUnitMapView$UnitMapViewControl$deckDesignatorInput" id="ControlGroupUnitMapView_UnitMapViewControl_deckDesignatorInput" value="1"><input type="hidden" name="ControlGroupUnitMapView$UnitMapViewControl$tripInput" id="ControlGroupUnitMapView_UnitMapViewControl_tripInput" value="0"><input type="hidden" name="ControlGroupUnitMapView$UnitMapViewControl$passengerInput" id="ControlGroupUnitMapView_UnitMapViewControl_passengerInput" value="0">
                  <div id="unitMap">
                    <div id="tabSection">
                      <ul class="segmenttabs">
                        <li id="listItem_0" class="selected">
                          <div class="tabTextContainer"><span>Flight 1</span><a class="tabText" id="activate_0_0">KUL&nbsp;-&nbsp;BKI</a></div>
                        </li>
                        
                        
                        <li id="listItem_3">
                          <div class="tabTextContainer"><span>Flight 2</span><a class="tabText" id="activate_3_0">BKI&nbsp;-&nbsp;KUL</a></div>
                        </li>
                      </ul>
                    </div>
                    <div id="deckTabs" class="hidden" style="display: none;">
                      <div id="deckTab_1" class="tab"><label id="deckTabLabel_1">Deck One</label><a id="deckTabA_1" href="javascript:void(0);">Deck One</a></div>
                      <div id="deckTab_2" class="tab"><label id="deckTabLabel_2">Deck Two</label><a id="deckTabA_2" href="javascript:void(0);">Deck Two</a></div>
                    </div>
                    <div id="JetAircraft">
                      <div id="unitMapView" class="unitMapViewJetAircraft">
                        <div id="deck" class="compartment" style="top: 0px; height: 1216px; width: 306px;"><div id="Y_1" class="" style="display: block;"><div class="floor " style="top:16px;left:0px;width:256px; height:1200px;"></div><div class="wallRight" style="top:16px; left:0px; width:16px; height:1200px;"></div><div class="wallLeft" style="top:16px;left:240px;width:16px; height:1200px;"></div>
        <div id="0_Y_1__0" class="unit noRepeat fillBackground" style="top: 16px; left: 16px; width: 80px; height: 48px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LV_Open_0.gif');">
        </div>
        

          <div id="0_Y_1__1" class="unit" style="top: -32px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>A</acronym>
          </div>
        

          <div id="0_Y_1__10" class="unit" style="top: -32px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>E</acronym>
          </div>
        

          <div id="0_Y_1__11" class="unit" style="top: -32px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>F</acronym>
          </div>
        

          <div id="0_Y_1__12" class="unit" style="top: 160px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>2</acronym>
          </div>
        



          <div id="0_Y_1__13" class="unit" style="top: 192px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>3</acronym>
          </div>
        

          <div id="0_Y_1__14" class="unit" style="top: 224px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>4</acronym>
          </div>
        

          <div id="0_Y_1__15" class="unit" style="top: 256px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>5</acronym>
          </div>
        

          <div id="0_Y_1__16" class="unit" style="top: 288px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>6</acronym>
          </div>
        

          <div id="0_Y_1__17" class="unit" style="top: 320px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>7</acronym>
          </div>
        

          <div id="0_Y_1__18" class="unit" style="top: 352px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>8</acronym>
          </div>
        

          <div id="0_Y_1__19" class="unit" style="top: 384px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>9</acronym>
          </div>
        

        <div id="0_Y_1__2" class="unit tile90" style="top: 96px; left: 16px; width: 96px; height: 16px; background-image: url('../content/images/AKBase/equipment/JetAircraft_BH_Open_90.gif');">
        </div>
        

          <div id="0_Y_1__20" class="unit" style="top: 416px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>10</acronym>
          </div>
        

          <div id="0_Y_1__21" class="unit" style="top: 448px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>11</acronym>
          </div>
        

          <div id="0_Y_1__22" class="unit" style="top: 496px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>12</acronym>
          </div>
        

          <div id="0_Y_1__23" class="unit" style="top: 544px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>14</acronym>
          </div>
        

          <div id="0_Y_1__24" class="unit" style="top: 576px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>15</acronym>
          </div>
        

          <div id="0_Y_1__25" class="unit" style="top: 608px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>16</acronym>
          </div>
        

          <div id="0_Y_1__26" class="unit" style="top: 640px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>17</acronym>
          </div>
        

          <div id="0_Y_1__27" class="unit" style="top: 672px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>18</acronym>
          </div>
        

          <div id="0_Y_1__28" class="unit" style="top: 704px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>19</acronym>
          </div>
        

          <div id="0_Y_1__29" class="unit" style="top: 736px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>20</acronym>
          </div>
        

        <div id="0_Y_1__3" class="unit tile90" style="top: 96px; left: 144px; width: 96px; height: 16px; background-image: url('../content/images/AKBase/equipment/JetAircraft_BH_Open_90.gif');">
        </div>
        

          <div id="0_Y_1__30" class="unit" style="top: 768px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>21</acronym>
          </div>
        

          <div id="0_Y_1__31" class="unit" style="top: 800px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>22</acronym>
          </div>
        

          <div id="0_Y_1__32" class="unit" style="top: 832px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>23</acronym>
          </div>
        

          <div id="0_Y_1__33" class="unit" style="top: 864px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>24</acronym>
          </div>
        

          <div id="0_Y_1__34" class="unit" style="top: 896px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>25</acronym>
          </div>
        

          <div id="0_Y_1__35" class="unit" style="top: 928px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>26</acronym>
          </div>
        

          <div id="0_Y_1__36" class="unit" style="top: 960px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>27</acronym>
          </div>
        

          <div id="0_Y_1__37" class="unit" style="top: 992px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>28</acronym>
          </div>
        

          <div id="0_Y_1__38" class="unit" style="top: 1024px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>29</acronym>
          </div>
        

          <div id="0_Y_1__39" class="unit" style="top: 1056px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>30</acronym>
          </div>
        

          <div id="0_Y_1__4" class="unit" style="top: 128px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>1</acronym>
          </div>
        

          <div id="0_Y_1__40" class="unit" style="top: 1088px; left: -32px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>31</acronym>
          </div>
        

          <div id="0_Y_1__5" class="unit" style="top: -32px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>B</acronym>
          </div>
        

        <div id="0_Y_1__6" class="unit noRepeat fillBackground" style="top: 1120px; left: 16px; width: 80px; height: 48px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LV_Open_0.gif');">
        </div>
        

        <div id="0_Y_1__7" class="unit noRepeat fillBackground" style="top: 1120px; left: 160px; width: 80px; height: 48px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LV_Open_0.gif');">
        </div>
        

          <div id="0_Y_1__8" class="unit" style="top: -32px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>C</acronym>
          </div>
        

        <div id="0_Y_1__82" class="unit" style="top: 64px; left: 0px; width: 16px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_EX_Open_0.gif');">
        </div>
        

        <div id="0_Y_1__83" class="unit" style="top: 64px; left: 240px; width: 16px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_EX_Open_180.gif');">
        </div>
        

        <div id="0_Y_1__84" class="unit" style="top: 1168px; left: 0px; width: 16px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_EX_Open_0.gif');">
        </div>
        

        <div id="0_Y_1__85" class="unit" style="top: 1168px; left: 240px; width: 16px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_EX_Open_180.gif');">
        </div>
        

        <div id="0_Y_1__86" class="unit" style="top: 480px; left: 0px; width: 16px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_EX_Open_0.gif');">
        </div>
        

        <div id="0_Y_1__87" class="unit" style="top: 528px; left: 0px; width: 16px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_EX_Open_0.gif');">
        </div>
        

        <div id="0_Y_1__88" class="unit" style="top: 480px; left: 240px; width: 16px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_EX_Open_180.gif');">
        </div>
        

        <div id="0_Y_1__89" class="unit" style="top: 528px; left: 240px; width: 16px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_EX_Open_180.gif');">
        </div>
        

          <div id="0_Y_1__9" class="unit" style="top: -32px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_LR_Open_0.gif');">
            <acronym>D</acronym>
          </div>
        

        <div id="0_Y_1_10A" class="aUnit unitGroup5" style="top: 416px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>10A</acronym>
            <acronym>10A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_10B" class="aUnit unitGroup5" style="top: 416px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>10B</acronym>
            <acronym>10B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_10C" class="aUnit unitGroup5" style="top: 416px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>10C</acronym>
            <acronym>10C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_10D" class="aUnit unitGroup5" style="top: 416px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>10D</acronym>
            <acronym>10D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_10E" class="aUnit unitGroup5" style="top: 416px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>10E</acronym>
            <acronym>10E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_10F" class="aUnit unitGroup5" style="top: 416px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>10F</acronym>
            <acronym>10F</acronym>-->
            
        </div>
        

        <div id="0_Y_1_11A" class="aUnit unitGroup5" style="top: 448px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>11A</acronym>
            <acronym>11A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_11B" class="aUnit unitGroup5" style="top: 448px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>11B</acronym>
            <acronym>11B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_11C" class="aUnit unitGroup5" style="top: 448px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>11C</acronym>
            <acronym>11C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_11D" class="aUnit unitGroup5" style="top: 448px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>11D</acronym>
            <acronym>11D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_11E" class="aUnit unitGroup5" style="top: 448px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>11E</acronym>
            <acronym>11E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_11F" class="aUnit unitGroup5" style="top: 448px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>11F</acronym>
            <acronym>11F</acronym>-->
            
        </div>
        

        <div id="0_Y_1_12A" class="aUnit unitGroup7" style="top: 496px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_AB.gif');">
            <!--<acronym>12A</acronym>
            <acronym>12A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_12B" class="aUnit unitGroup7" style="top: 496px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_AB.gif');">
            <!--<acronym>12B</acronym>
            <acronym>12B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_12C" class="aUnit unitGroup7" style="top: 496px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_AB.gif');">
            <!--<acronym>12C</acronym>
            <acronym>12C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_12D" class="aUnit unitGroup7" style="top: 496px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_AB.gif');">
            <!--<acronym>12D</acronym>
            <acronym>12D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_12E" class="aUnit unitGroup7" style="top: 496px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_AB.gif');">
            <!--<acronym>12E</acronym>
            <acronym>12E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_12F" class="aUnit unitGroup7" style="top: 496px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_AB.gif');">
            <!--<acronym>12F</acronym>
            <acronym>12F</acronym>-->
            
        </div>
        

        <div id="0_Y_1_14A" class="aUnit unitGroup7" style="top: 544px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_AB.gif');">
            <!--<acronym>14A</acronym>
            <acronym>14A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_14B" class="aUnit unitGroup7" style="top: 544px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_AB.gif');">
            <!--<acronym>14B</acronym>
            <acronym>14B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_14C" class="aUnit unitGroup7" style="top: 544px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_AB.gif');">
            <!--<acronym>14C</acronym>
            <acronym>14C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_14D" class="aUnit unitGroup7" style="top: 544px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_AB.gif');">
            <!--<acronym>14D</acronym>
            <acronym>14D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_14E" class="aUnit unitGroup7" style="top: 544px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_AB.gif');">
            <!--<acronym>14E</acronym>
            <acronym>14E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_14F" class="aUnit unitGroup7" style="top: 544px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_AB.gif');">
            <!--<acronym>14F</acronym>
            <acronym>14F</acronym>-->
            
        </div>
        

        <div id="0_Y_1_15A" class="aUnit unitGroup1" style="top: 576px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>15A</acronym>
            <acronym>15A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_15B" class="aUnit unitGroup1" style="top: 576px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>15B</acronym>
            <acronym>15B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_15C" class="aUnit unitGroup1" style="top: 576px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>15C</acronym>
            <acronym>15C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_15D" class="aUnit unitGroup1" style="top: 576px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>15D</acronym>
            <acronym>15D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_15E" class="aUnit unitGroup1" style="top: 576px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>15E</acronym>
            <acronym>15E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_15F" class="aUnit unitGroup1" style="top: 576px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>15F</acronym>
            <acronym>15F</acronym>-->
            
        </div>
        

        <div id="0_Y_1_16A" class="aUnit unitGroup1" style="top: 608px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>16A</acronym>
            <acronym>16A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_16B" class="aUnit unitGroup1" style="top: 608px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>16B</acronym>
            <acronym>16B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_16C" class="aUnit unitGroup1" style="top: 608px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>16C</acronym>
            <acronym>16C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_16D" class="aUnit unitGroup1" style="top: 608px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>16D</acronym>
            <acronym>16D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_16E" class="aUnit unitGroup1" style="top: 608px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>16E</acronym>
            <acronym>16E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_16F" class="aUnit unitGroup1" style="top: 608px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>16F</acronym>
            <acronym>16F</acronym>-->
            
        </div>
        

        <div id="0_Y_1_17A" class="aUnit unitGroup1" style="top: 640px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>17A</acronym>
            <acronym>17A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_17B" class="aUnit unitGroup1" style="top: 640px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>17B</acronym>
            <acronym>17B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_17C" class="aUnit unitGroup1" style="top: 640px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>17C</acronym>
            <acronym>17C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_17D" class="aUnit unitGroup1" style="top: 640px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>17D</acronym>
            <acronym>17D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_17E" class="aUnit unitGroup1" style="top: 640px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>17E</acronym>
            <acronym>17E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_17F" class="aUnit unitGroup1" style="top: 640px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>17F</acronym>
            <acronym>17F</acronym>-->
            
        </div>
        
<!-- regular seat row 18 -->
        <div id="0_Y_1_18A" class="aUnit unitGroup1" style="top: 672px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>18A</acronym>
            <acronym>18A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_18B" class="aUnit unitGroup1" style="top: 672px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>18B</acronym>
            <acronym>18B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_18C" class="aUnit unitGroup1" style="top: 672px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>18C</acronym>
            <acronym>18C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_18D" class="aUnit unitGroup1" style="top: 672px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>18D</acronym>
            <acronym>18D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_18E" class="aUnit unitGroup1" style="top: 672px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>18E</acronym>
            <acronym>18E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_18F" class="aUnit unitGroup1" style="top: 672px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>18F</acronym>
            <acronym>18F</acronym>-->
            
        </div>
        
        <!-- regular seat row 19 -->

        <div id="0_Y_1_19A" class="aUnit unitGroup1" style="top: 704px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>19A</acronym>
            <acronym>19A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_19B" class="aUnit unitGroup1" style="top: 704px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>19B</acronym>
            <acronym>19B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_19C" class="aUnit unitGroup1" style="top: 704px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>19C</acronym>
            <acronym>19C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_19D" class="aUnit unitGroup1" style="top: 704px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>19D</acronym>
            <acronym>19D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_19E" class="aUnit unitGroup1" style="top: 704px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>19E</acronym>
            <acronym>19E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_19F" class="aUnit unitGroup1" style="top: 704px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>19F</acronym>
            <acronym>19F</acronym>-->
            
        </div>
        
<!-- hot seat row 1 -->
        <div id="0_Y_1_1A" class="aUnit unitGroup8" style="top: 128px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_AW.gif');">
            <!--<acronym>1A</acronym>
            <acronym>1A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_1B" class="aUnit unitGroup8" style="top: 128px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_AW.gif');">
            <!--<acronym>1B</acronym>
            <acronym>1B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_1C" class="aUnit unitGroup8" style="top: 128px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_AW.gif');">
            <!--<acronym>1C</acronym>
            <acronym>1C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_1D" class="aUnit unitGroup8" style="top: 128px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_AW.gif');">
            <!--<acronym>1D</acronym>
            <acronym>1D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_1E" class="aUnit unitGroup8" style="top: 128px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_AW.gif');">
            <!--<acronym>1E</acronym>
            <acronym>1E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_1F" class="aUnit unitGroup8" style="top: 128px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_AW.gif');">
            <!--<acronym>1F</acronym>
            <acronym>1F</acronym>-->
            
        </div>

<!-- hot seat row 2 -->
                <div id="0_Y_1_2A" class="aUnit unitGroup2" style="top: 160px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_HS.gif');">
            <!--<acronym>2A</acronym>
            <acronym>2A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_2B" class="aUnit unitGroup2" style="top: 160px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_HS.gif');">
            <!--<acronym>2B</acronym>
            <acronym>2B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_2C" class="aUnit unitGroup2" style="top: 160px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_HS.gif');">
            <!--<acronym>2C</acronym>
            <acronym>2C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_2D" class="aUnit unitGroup2" style="top: 160px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_HS.gif');">
            <!--<acronym>2D</acronym>
            <acronym>2D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_2E" class="aUnit unitGroup2" style="top: 160px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_HS.gif');">
            <!--<acronym>2E</acronym>
            <acronym>2E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_2F" class="aUnit unitGroup2" style="top: 160px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_HS.gif');">
            <!--<acronym>2F</acronym>
            <acronym>2F</acronym>-->
            
        </div>

               <div id="0_Y_1_3A" class="aUnit unitGroup2" style="top: 192px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_HS.gif');">
            <!--<acronym>3A</acronym>
            <acronym>3A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_3B" class="aUnit unitGroup2" style="top: 192px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_HS.gif');">
            <!--<acronym>3B</acronym>
            <acronym>3B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_3C" class="aUnit unitGroup2" style="top: 192px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_HS.gif');">
            <!--<acronym>3C</acronym>
            <acronym>3C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_3D" class="aUnit unitGroup2" style="top: 192px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_HS.gif');">
            <!--<acronym>3D</acronym>
            <acronym>3D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_3E" class="aUnit unitGroup2" style="top: 192px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_HS.gif');">
            <!--<acronym>3E</acronym>
            <acronym>3E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_3F" class="aUnit unitGroup2" style="top: 192px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_HS.gif');">
            <!--<acronym>3F</acronym>
            <acronym>3F</acronym>-->
            
        </div>
        

        <div id="0_Y_1_4A" class="aUnit unitGroup2" style="top: 224px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_HS.gif');">
            <!--<acronym>4A</acronym>
            <acronym>4A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_4B" class="aUnit unitGroup2" style="top: 224px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_HS.gif');">
            <!--<acronym>4B</acronym>
            <acronym>4B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_4C" class="aUnit unitGroup2" style="top: 224px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_HS.gif');">
            <!--<acronym>4C</acronym>
            <acronym>4C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_4D" class="aUnit unitGroup2" style="top: 224px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_HS.gif');">
            <!--<acronym>4D</acronym>
            <acronym>4D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_4E" class="aUnit unitGroup2" style="top: 224px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_HS.gif');">
            <!--<acronym>4E</acronym>
            <acronym>4E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_4F" class="aUnit unitGroup2" style="top: 224px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_HS.gif');">
            <!--<acronym>4F</acronym>
            <acronym>4F</acronym>-->
            
        </div>
        

        <div id="0_Y_1_5A" class="aUnit unitGroup2" style="top: 256px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_HS.gif');">
            <!--<acronym>5A</acronym>
            <acronym>5A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_5B" class="aUnit unitGroup2" style="top: 256px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_HS.gif');">
            <!--<acronym>5B</acronym>
            <acronym>5B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_5C" class="aUnit unitGroup2" style="top: 256px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_HS.gif');">
            <!--<acronym>5C</acronym>
            <acronym>5C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_5D" class="aUnit unitGroup2" style="top: 256px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_HS.gif');">
            <!--<acronym>5D</acronym>
            <acronym>5D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_5E" class="aUnit unitGroup2" style="top: 256px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_HS.gif');">
            <!--<acronym>5E</acronym>
            <acronym>5E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_5F" class="aUnit unitGroup2" style="top: 256px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_HS.gif');">
            <!--<acronym>5F</acronym>
            <acronym>5F</acronym>-->
            
        </div>
        

        <div id="0_Y_1_6A" class="aUnit unitGroup5" style="top: 288px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>6A</acronym>
            <acronym>6A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_6B" class="aUnit unitGroup5" style="top: 288px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>6B</acronym>
            <acronym>6B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_6C" class="aUnit unitGroup5" style="top: 288px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>6C</acronym>
            <acronym>6C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_6D" class="aUnit unitGroup5" style="top: 288px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>6D</acronym>
            <acronym>6D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_6E" class="aUnit unitGroup5" style="top: 288px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>6E</acronym>
            <acronym>6E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_6F" class="aUnit unitGroup5" style="top: 288px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>6F</acronym>
            <acronym>6F</acronym>-->
            
        </div>
        

        <div id="0_Y_1_7A" class="aUnit unitGroup5" style="top: 320px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>7A</acronym>
            <acronym>7A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_7B" class="aUnit unitGroup5" style="top: 320px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>7B</acronym>
            <acronym>7B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_7C" class="aUnit unitGroup5" style="top: 320px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>7C</acronym>
            <acronym>7C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_7D" class="aUnit unitGroup5" style="top: 320px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>7D</acronym>
            <acronym>7D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_7E" class="aUnit unitGroup5" style="top: 320px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>7E</acronym>
            <acronym>7E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_7F" class="aUnit unitGroup5" style="top: 320px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>7F</acronym>
            <acronym>7F</acronym>-->
            
        </div>
        

        <div id="0_Y_1_8A" class="aUnit unitGroup5" style="top: 352px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>8A</acronym>
            <acronym>8A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_8B" class="aUnit unitGroup5" style="top: 352px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>8B</acronym>
            <acronym>8B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_8C" class="aUnit unitGroup5" style="top: 352px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>8C</acronym>
            <acronym>8C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_8D" class="aUnit unitGroup5" style="top: 352px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>8D</acronym>
            <acronym>8D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_8E" class="aUnit unitGroup5" style="top: 352px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>8E</acronym>
            <acronym>8E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_8F" class="aUnit unitGroup5" style="top: 352px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>8F</acronym>
            <acronym>8F</acronym>-->
            
        </div>
        

        <div id="0_Y_1_9A" class="aUnit unitGroup5" style="top: 384px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>9A</acronym>
            <acronym>9A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_9B" class="aUnit unitGroup5" style="top: 384px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>9B</acronym>
            <acronym>9B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_9C" class="aUnit unitGroup5" style="top: 384px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>9C</acronym>
            <acronym>9C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_9D" class="aUnit unitGroup5" style="top: 384px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>9D</acronym>
            <acronym>9D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_9E" class="aUnit unitGroup5" style="top: 384px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>9E</acronym>
            <acronym>9E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_9F" class="aUnit unitGroup5" style="top: 384px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif');">
            <!--<acronym>9F</acronym>
            <acronym>9F</acronym>-->
            
        </div>

        
  <!-- regular seat row 20 -->
        <div id="0_Y_1_20A" class="aUnit unitGroup1" style="top: 736px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>20A</acronym>
            <acronym>20A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_20B" class="aUnit unitGroup1" style="top: 736px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>20B</acronym>
            <acronym>20B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_20C" class="aUnit unitGroup1" style="top: 736px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>20C</acronym>
            <acronym>20C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_20D" class="aUnit unitGroup1" style="top: 736px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>20D</acronym>
            <acronym>20D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_20E" class="aUnit unitGroup1" style="top: 736px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>20E</acronym>
            <acronym>20E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_20F" class="aUnit unitGroup1" style="top: 736px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>20F</acronym>
            <acronym>20F</acronym>-->
            
        </div>
        
  <!-- regular seat row 21 -->
        <div id="0_Y_1_21A" class="aUnit unitGroup1" style="top: 768px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>21A</acronym>
            <acronym>21A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_21B" class="aUnit unitGroup1" style="top: 768px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>21B</acronym>
            <acronym>21B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_21C" class="aUnit unitGroup1" style="top: 768px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>21C</acronym>
            <acronym>21C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_21D" class="aUnit unitGroup1" style="top: 768px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>21D</acronym>
            <acronym>21D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_21E" class="aUnit unitGroup1" style="top: 768px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>21E</acronym>
            <acronym>21E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_21F" class="aUnit unitGroup1" style="top: 768px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>21F</acronym>
            <acronym>21F</acronym>-->
            
        </div>
        

        <div id="0_Y_1_22A" class="aUnit unitGroup1" style="top: 800px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>22A</acronym>
            <acronym>22A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_22B" class="aUnit unitGroup1" style="top: 800px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>22B</acronym>
            <acronym>22B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_22C" class="aUnit unitGroup1" style="top: 800px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>22C</acronym>
            <acronym>22C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_22D" class="aUnit unitGroup1" style="top: 800px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>22D</acronym>
            <acronym>22D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_22E" class="aUnit unitGroup1" style="top: 800px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>22E</acronym>
            <acronym>22E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_22F" class="aUnit unitGroup1" style="top: 800px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>22F</acronym>
            <acronym>22F</acronym>-->
            
        </div>
        

        <div id="0_Y_1_23A" class="aUnit unitGroup1" style="top: 832px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>23A</acronym>
            <acronym>23A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_23B" class="aUnit unitGroup1" style="top: 832px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>23B</acronym>
            <acronym>23B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_23C" class="aUnit unitGroup1" style="top: 832px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>23C</acronym>
            <acronym>23C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_23D" class="aUnit unitGroup1" style="top: 832px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>23D</acronym>
            <acronym>23D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_23E" class="aUnit unitGroup1" style="top: 832px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>23E</acronym>
            <acronym>23E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_23F" class="aUnit unitGroup1" style="top: 832px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>23F</acronym>
            <acronym>23F</acronym>-->
            
        </div>
        

        <div id="0_Y_1_24A" class="aUnit unitGroup1" style="top: 864px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>24A</acronym>
            <acronym>24A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_24B" class="aUnit unitGroup1" style="top: 864px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>24B</acronym>
            <acronym>24B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_24C" class="aUnit unitGroup1" style="top: 864px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>24C</acronym>
            <acronym>24C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_24D" class="aUnit unitGroup1" style="top: 864px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>24D</acronym>
            <acronym>24D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_24E" class="aUnit unitGroup1" style="top: 864px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>24E</acronym>
            <acronym>24E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_24F" class="aUnit unitGroup1" style="top: 864px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>24F</acronym>
            <acronym>24F</acronym>-->
            
        </div>
        

        <div id="0_Y_1_25A" class="aUnit unitGroup1" style="top: 896px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>25A</acronym>
            <acronym>25A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_25B" class="aUnit unitGroup1" style="top: 896px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>25B</acronym>
            <acronym>25B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_25C" class="aUnit unitGroup1" style="top: 896px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>25C</acronym>
            <acronym>25C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_25D" class="aUnit unitGroup1" style="top: 896px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>25D</acronym>
            <acronym>25D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_25E" class="aUnit unitGroup1" style="top: 896px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>25E</acronym>
            <acronym>25E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_25F" class="aUnit unitGroup1" style="top: 896px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>25F</acronym>
            <acronym>25F</acronym>-->
            
        </div>
        

        <div id="0_Y_1_26A" class="aUnit unitGroup1" style="top: 928px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>26A</acronym>
            <acronym>26A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_26B" class="aUnit unitGroup1" style="top: 928px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>26B</acronym>
            <acronym>26B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_26C" class="aUnit unitGroup1" style="top: 928px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>26C</acronym>
            <acronym>26C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_26D" class="aUnit unitGroup1" style="top: 928px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>26D</acronym>
            <acronym>26D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_26E" class="aUnit unitGroup1" style="top: 928px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>26E</acronym>
            <acronym>26E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_26F" class="aUnit unitGroup1" style="top: 928px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>26F</acronym>
            <acronym>26F</acronym>-->
            
        </div>
        

        <div id="0_Y_1_27A" class="aUnit unitGroup1" style="top: 960px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>27A</acronym>
            <acronym>27A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_27B" class="aUnit unitGroup1" style="top: 960px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>27B</acronym>
            <acronym>27B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_27C" class="aUnit unitGroup1" style="top: 960px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>27C</acronym>
            <acronym>27C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_27D" class="aUnit unitGroup1" style="top: 960px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>27D</acronym>
            <acronym>27D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_27E" class="aUnit unitGroup1" style="top: 960px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>27E</acronym>
            <acronym>27E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_27F" class="aUnit unitGroup1" style="top: 960px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>27F</acronym>
            <acronym>27F</acronym>-->
            
        </div>
        

        <div id="0_Y_1_28A" class="aUnit unitGroup1" style="top: 992px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>28A</acronym>
            <acronym>28A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_28B" class="aUnit unitGroup1" style="top: 992px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>28B</acronym>
            <acronym>28B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_28C" class="aUnit unitGroup1" style="top: 992px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>28C</acronym>
            <acronym>28C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_28D" class="aUnit unitGroup1" style="top: 992px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>28D</acronym>
            <acronym>28D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_28E" class="aUnit unitGroup1" style="top: 992px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>28E</acronym>
            <acronym>28E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_28F" class="aUnit unitGroup1" style="top: 992px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>28F</acronym>
            <acronym>28F</acronym>-->
            
        </div>
        

        <div id="0_Y_1_29A" class="aUnit unitGroup1" style="top: 1024px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>29A</acronym>
            <acronym>29A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_29B" class="aUnit unitGroup1" style="top: 1024px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>29B</acronym>
            <acronym>29B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_29C" class="aUnit unitGroup1" style="top: 1024px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>29C</acronym>
            <acronym>29C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_29D" class="aUnit unitGroup1" style="top: 1024px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>29D</acronym>
            <acronym>29D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_29E" class="aUnit unitGroup1" style="top: 1024px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>29E</acronym>
            <acronym>29E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_29F" class="aUnit unitGroup1" style="top: 1024px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>29F</acronym>
            <acronym>29F</acronym>-->
            
        </div>
        


        

        <div id="0_Y_1_30A" class="aUnit unitGroup1" style="top: 1056px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>30A</acronym>
            <acronym>30A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_30B" class="aUnit unitGroup1" style="top: 1056px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>30B</acronym>
            <acronym>30B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_30C" class="aUnit unitGroup1" style="top: 1056px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>30C</acronym>
            <acronym>30C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_30D" class="aUnit unitGroup1" style="top: 1056px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>30D</acronym>
            <acronym>30D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_30E" class="aUnit unitGroup1" style="top: 1056px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>30E</acronym>
            <acronym>30E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_30F" class="aUnit unitGroup1" style="top: 1056px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>30F</acronym>
            <acronym>30F</acronym>-->
            
        </div>
        

        <div id="0_Y_1_31A" class="aUnit unitGroup1" style="top: 1088px; left: 16px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>31A</acronym>
            <acronym>31A</acronym>-->
            
        </div>
        

        <div id="0_Y_1_31B" class="aUnit unitGroup1" style="top: 1088px; left: 48px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>31B</acronym>
            <acronym>31B</acronym>-->
            
        </div>
        

        <div id="0_Y_1_31C" class="aUnit unitGroup1" style="top: 1088px; left: 80px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>31C</acronym>
            <acronym>31C</acronym>-->
            
        </div>
        

        <div id="0_Y_1_31D" class="aUnit unitGroup1" style="top: 1088px; left: 144px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>31D</acronym>
            <acronym>31D</acronym>-->
            
        </div>
        

        <div id="0_Y_1_31E" class="aUnit unitGroup1" style="top: 1088px; left: 176px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>31E</acronym>
            <acronym>31E</acronym>-->
            
        </div>
        

        <div id="0_Y_1_31F" class="aUnit unitGroup1" style="top: 1088px; left: 208px; width: 32px; height: 32px; background-image: url('../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif');">
            <!--<acronym>31F</acronym>
            <acronym>31F</acronym>-->
            
        </div>
        

 
        </div></div>
                      </div>
                    </div>
                  </div>
                <div class="clearAll"></div>
                <div class="spacerMed"></div>
               
               
              
              
                <div id="Div16" class="hidden">
        &lt;div id="[unitKey]" class="unit tile[unitAngle]" style="top:[y]px; left:[x]px; width:[width]px; height:[height]px;" &gt;
        &lt;/div&gt;
        </div>
                <div id="Div17" class="hidden"></div>
                <div id="Div18" class="hidden">
        &lt;div id="[unitKey]" class="unit noRepeat" style="top:[y]px; left:[x]px; width:[width]px; height:[height]px;" &gt;
        &lt;/div&gt;
        </div>
                <div id="Div19" class="hidden">
      &lt;div id="[unitKey]" class="aUnit" style="top:[y]px; left:[x]px; width:[width]px; height:[height]px; border: 1px solid black;"&gt;
      &lt;/div&gt;
      </div>
                <div id="Div20" class="hidden">
        &lt;div id="[unitKey]" class="unit" style="top:[y]px; left:[x]px; width:[width]px; height:[height]px;" &gt;
        &lt;/div&gt;
        </div>
                <div id="Div21" class="hidden">
        &lt;div id="[unitKey]" class="aUnit noRepeat" style="top:[y]px; left:[x]px; width:[width]px; height:[height]px;" &gt;
        &lt;/div&gt;
        </div>
                <div id="Div22" class="hidden">
        &lt;div id="[unitKey]" class="unit" style="top:[y]px; left:[x]px; width:[width]px; height:[height]px;" &gt;
        &lt;/div&gt;
        </div>
                <div id="Div23" class="hidden">
        &lt;div id="[unitKey]" class="unit" style="top:[y]px; left:[x]px; width:[width]px; height:[height]px;" &gt;
        &lt;/div&gt;
        </div>
                <div id="Div24" class="hidden">
        &lt;div id="[unitKey]" class="unit" style="top:[y]px; left:[x]px; width:[width]px; height:[height]px;" &gt;
        &lt;/div&gt;
        </div>
                <div id="Div25" class="hidden">
        &lt;div id="[unitKey]" class="unit" style="top:[y]px; left:[x]px; width:[width]px; height:[height]px;" &gt;
        &lt;/div&gt;
        </div>
                <div id="Div26" class="hidden">
        &lt;div id="[unitKey]" class="aUnit" &gt;
        &lt;/div&gt;
        </div>
                <div id="Div27" class="hidden">
        &lt;div id="[unitKey]" class="aUnit unitGroup[unitGroup]" style="top:[y]px; left:[x]px; width:[width]px; height:[height]px;" &gt;
        &lt;acronym&gt;[unitDesignator]&lt;/acronym&gt;
            &lt;acronym&gt;[unitDesignator]&lt;/acronym&gt;
        &lt;/div&gt;
        </div>
                <div id="Div28" class="hidden">
        &lt;div id="[unitKey]" class="aUnit unitGroup[unitGroup]" style="top:[y]px; left:[x]px; width:[width]px; height:[height]px;" &gt;
        &lt;acronym&gt;[unitDesignator]&lt;/acronym&gt;
            &lt;acronym&gt;[unitDesignator]&lt;/acronym&gt;
        &lt;/div&gt;
        </div>
                <div id="Div29" class="hidden">
        &lt;div id="[unitKey]" class="aUnit unitGroup[unitGroup]" style="top:[y]px; left:[x]px; width:[width]px; height:[height]px;" &gt;
            &lt;acronym&gt;[unitDesignator]&lt;/acronym&gt;
            &lt;acronym&gt;[unitDesignator]&lt;/acronym&gt;
        &lt;/div&gt;
        </div>
                <div id="Div30" class="hidden">
        &lt;div id="[unitKey]" class="aUnit unitGroup[unitGroup]" style="top:[y]px; left:[x]px; width:[width]px; height:[height]px;" &gt;
        &lt;acronym&gt;[unitDesignator]&lt;/acronym&gt;
            &lt;acronym&gt;[unitDesignator]&lt;/acronym&gt;
        &lt;/div&gt;
        </div>
                <div id="Div31" class="hidden">
        &lt;div id="[unitKey]" class="aUnit unitGroup[unitGroup]" style="top:[y]px; left:[x]px; width:[width]px; height:[height]px;" &gt;
        &lt;acronym&gt;[unitDesignator]&lt;/acronym&gt;
            &lt;acronym&gt;[unitDesignator]&lt;/acronym&gt;
        &lt;/div&gt;
        </div>
                <div id="Div32" class="hidden">
        &lt;div id="[unitKey]" class="aUnit unitGroup[unitGroup]" style="top:[y]px; left:[x]px; width:[width]px; height:[height]px;" &gt;
            &lt;acronym&gt;[unitDesignator]&lt;/acronym&gt;
            &lt;acronym&gt;[unitDesignator]&lt;/acronym&gt;
        &lt;/div&gt;
        </div>
                <div id="Div33" class="hidden">
      &lt;p class="formCheckbox"&gt;
        &lt;input type="checkbox" name="[k]_[v]" value="1" /&gt;
        &lt;label&gt;[k]: [v]&lt;/label&gt;
      &lt;/p&gt;
      </div>
                <div id="Div34" class="hidden">
      &lt;p&gt;
        &lt;input type="checkbox" name="[ssrCode]" id="[ssrKey]_checkbox" value="1" /&gt;
        &lt;label for="[ssrKey]_checkbox"&gt;[name] [passengerFee]&lt;/label&gt;
      &lt;/p&gt;
      </div>
                <div id="Div35" class="hidden"> [passengerFee] </div>
                <div id="Div36" class="hidden">
        &lt;p&gt;
            &lt;label&gt;[name]&lt;/label&gt;
        &lt;/p&gt;
        </div>
                <div id="Div37" class="hidden">
            &lt;p&gt;
                &lt;label&gt;[value]&lt;/label&gt;
            &lt;/p&gt;
            </div><span id="Span1"></span><span id="Span2"></span><span id="Span3"></span><span id="Span4"></span><span id="Span5"></span><div id="Div38" class="hidden upsellSM">
                  <div class="confirmSeatDetails">
                    <div id="Div39">
                      <div id="Div40" class="confirmSeatSM">
            &lt;h6&gt;
            [passengerName]: [segmentDepartureStation] - [segmentArrivalStation],
            Seat No.
            [compartmentUnitDesignator]
            &lt;/h6&gt;
            &lt;fieldset id="seatFeeFieldset"&gt;
            &lt;legend&gt;
            Seat Fees:
            &lt;/legend&gt;
            &lt;p&gt;
            &lt;span&gt;[unitFee]&lt;/span&gt;
            &lt;/p&gt;
            &lt;/fieldset&gt;


            &lt;fieldset id="lostAmenitiesFieldset"&gt;
            &lt;legend&gt;
            Lost Ssrs: 
            &lt;/legend&gt;
            &lt;p&gt;
            &lt;span&gt;[lostSsrArray]&lt;/span&gt;
            &lt;/p&gt;
            &lt;/fieldset&gt;

            &lt;fieldset id="addAmenitiesFieldset"&gt;
            &lt;legend&gt;
            Additional Amenities:
            &lt;/legend&gt;
            [ssrArray]
            &lt;/fieldset&gt;
            </div>
                    </div>
                    <p class="floatRightCA"><input type="button" name="ssrCancelButton" value="Cancel" id="Button1" class="button"><input type="button" name="ssrConfirmButton" value="Confirm" id="Button2" class="button"></p>
                  </div>
                </div>
              </div>
     <div id="propertyListBody" class="unitBody" style="margin-left:0;">
        <div class="legendSeatLeft">
            <table class="clearTableHeaders">
                <tbody>
                    <tr>
                        <th colspan="3"><span class="nonStyleHeader" style="margin-left: -10px"><strong>Seat selection key</strong></span></th>
                    </tr>
                    <tr>
                        <td>
                            <img src="../content/images/AKBase/equipment/JetAircraft_NS_Open_0_AW.gif" class="unitGroupKey"></td>
                        <td>Hot Seats(Row1)<br>
                            <strong id="hotSeatRow1">42.40 MYR</strong></td>
                    </tr>
                    <tr>
                        <td>
                            <img src="../content/images/AKBase/equipment/JetAircraft_NS_Open_0_AB.gif" class="unitGroupKey"></td>
                        <td>Hot Seats(Row12-14)<br>
                            <strong id="hotSeatRow12_14">42.40 MYR</strong></td>
                    </tr>
                    <tr>
                        <td>
                            <img src="../content/images/AKBase/equipment/JetAircraft_NS_Open_0_HS.gif" class="unitGroupKey"></td>
                        <td>Hot Seats<br>
                            <strong id="hotSeat">31.80 MYR</strong></td>
                    </tr>
                    <tr>
                        <td>
                            <img src="../content/images/AKBase/equipment/JetAircraft_NS_Open_0_DG.gif" class="unitGroupKey"></td>
                        <td>Standard Seats<br>
                            <strong id="std1">10.60 MYR</strong></td>
                    </tr>
                    <tr>
                        <td>
                            <img src="../content/images/AKBase/equipment/JetAircraft_NS_Open_0.gif" class="unitGroupKey"></td>
                        <td>Standard Seats<br>
                            <strong id="std2">6.36 MYR</strong></td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="legendSeatRight">
            <table>
                <tbody>
                    <tr>
                        <td>
                            <div id="seatLegendBody">
                                <div id="mainLegend">
                                    <ul>
                                        <li>
                                            <img src="../content/images/AKBase/equipment/icon_occupied.gif" alt="">Selected</li>
                                        <li>
                                            <img src="../content/images/AKBase/equipment/JetAircraft_NS_reserved_0.gif" alt="">Occupied</li>
                                    </ul>
                                </div>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
        </div>
            </div>
    </div>
     <span id="showUpSellInputContainer"></span><span id="selectedFilterPropertyInputContainer"></span><span id="selectedAutoAssignPropertyInputContainer"></span><span id="selectedFilterSsrInputContainer"></span><span id="soldSsrInputContainer"></span>
    <div id="confirmSeat" class="hidden upsellSM">
        <div class="confirmSeatDetails">
            <div id="confirmSeatContainer">
                <div id="confirmSeatPaxOptions" class="confirmSeatSM">
                    &lt;h6&gt;
            [passengerName]: [segmentDepartureStation] - [segmentArrivalStation],
            Seat No.
            [compartmentUnitDesignator]
            &lt;/h6&gt;
            &lt;fieldset id="seatFeeFieldset"&gt;
            &lt;legend&gt;
            Seat Fees:
            &lt;/legend&gt;
            &lt;p&gt;
            &lt;span&gt;[unitFee]&lt;/span&gt;
            &lt;/p&gt;
            &lt;/fieldset&gt;


            &lt;fieldset id="lostAmenitiesFieldset"&gt;
            &lt;legend&gt;
            Lost Ssrs: 
            &lt;/legend&gt;
            &lt;p&gt;
            &lt;span&gt;[lostSsrArray]&lt;/span&gt;
            &lt;/p&gt;
            &lt;/fieldset&gt;

            &lt;fieldset id="addAmenitiesFieldset"&gt;
            &lt;legend&gt;
            Additional Amenities:
            &lt;/legend&gt;
            [ssrArray]
            &lt;/fieldset&gt;
                </div>
            </div>
            <p class="floatRightCA">
                <input type="button" name="ssrCancelButton" value="Cancel" id="ssrCancelButton" class="button"><input type="button" name="ssrConfirmButton" value="Confirm" id="ssrConfirmButton" class="button">
            </p>
        </div>
    </div>
    			  <div id="hiddenVal" style="display:none"></div>
				  <div id="hoverContainer" class="animate-opacity">
				  <div id="boxPointyWrapper">
				  <div id="boxPointy" class="fa fa-sort-asc"></div>
				  </div>
               
				  <div class="infoWrapper" style="display:inline-block">
                  <div class="labelHover">Seat No.</div>
                  <div id="textWrapper"></div>
                  </div>

                  <div class="infoWrapper" style="display:inline-block">
                  <div class="labelHover">Price</div>
                  <div id="priceWrapper"></div>
                  </div>
                 
				  </div>

     <script type="text/javascript">
         $(document).ready(function () {
             var seatNum = 0;
             var mySeatId = 0;
             var alreadyReserve = 0;
             var curValue;
             //focus on input
             $('.inputToFocus [type^="text"]').on('focusin', function () {

                 mySeatId = $(this);
                 $(this).data('myValue', $(this).val());
                 //curValue = $(this).val();

             }).on('change', function () {

                 //alert(curValue);
                
                 curValue = $(this).data('myValue');
                 alert(curValue);
                 var seatIDSelected = $(this).val();
                 seatIDSelected = seatIDSelected.toUpperCase();
                 mySeatId = $(this);

                 var seatIDSelected01 = curValue;
                 seatIDSelected01 = seatIDSelected01.toUpperCase();

                 if ($('[id*="_' + seatIDSelected01 + '"]').hasClass('reserved') && curValue != 0) {


                     $('[id*="_' + seatIDSelected01 + '"]').removeClass('reserved');

                     curValue = 0;

                 }
                 $('[id*="_' + seatIDSelected + '"]').addClass('reserved');
            
             });

             //mouseover the seat will grab the seat number, display the hover
             $('.aUnit').mouseover(function () {
                 var myChar = 0;
                 var leftPos = 0;
                 var topPos = 0;
                 var offsetVal = 30;
                 var seatRow = 0;
                 var seatPrice = 0;

                 $('#hoverContainer').css('top', $(this).offset().top + offsetVal);
                 $('#hoverContainer').css('left', $(this).offset().left + offsetVal-65);


                 $(this).contents().filter(function () {
                     return this.nodeType == 8;
                 }).each(function (i, e) {
                     myChar = e.nodeValue;
                 });

                 $('#hiddenVal').html(myChar);
                 seatNum = $('#hiddenVal acronym:first-child').text();
                 seatRow = seatNum.substr(seatNum.length - 1);
                 seatRow = seatNum.split(seatRow);
                 seatRow = seatRow[0] * 1;


                 switch (true) {
                     case seatRow == 1: seatPrice = $('#hotSeatRow1').text(); break;
                     case seatRow >= 12 && seatRow <= 14: seatPrice = $('#hotSeatRow12_14').text(); break;
                     case seatRow >= 2 && seatRow <= 5: seatPrice = $('#hotSeat').text(); break;
                     case seatRow >= 6 && seatRow <= 11: seatPrice = $('#std1').text(); break;
                     case seatRow >= 15 && seatRow <= 31: seatPrice = $('#std2').text(); break;
                 }

                 $('#hoverContainer #priceWrapper').text(seatPrice);
                 $('#hoverContainer #textWrapper').text(seatNum);
                 $('#hoverContainer').css('display', 'block');
             });

             $('.aUnit').mouseout(function () {
                 $('#hoverContainer').css('display', 'none');
             });

             //click the seat will fill in the value in text input
             $('.aUnit').click(function () {
                 if (mySeatId != 0 && $(this).attr('style').indexOf('_Open_') != -1 && !$(this).hasClass('reserved')) {

                     if (mySeatId.val().length == 0) { $(this).addClass('reserved'); mySeatId.val(seatNum); }
                     else {
                         if ($(this).attr('id').indexOf(mySeatId.val()) != -1) {
                             $(this).addClass('reserved');
                             mySeatId.val(null);
                         }
                         else {
                             var seatIDSelected = mySeatId.val();
                             seatIDSelected = seatIDSelected.toUpperCase();
                             $('[id*="_' + seatIDSelected + '"]').removeClass('reserved');
                             $(this).addClass('reserved');
                             mySeatId.val(seatNum);
                         }
                     }
                     mySeatId = 0;
                 }
             });


         });

</script>
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
    ini left panel, untuk menu/ booking details

</asp:Content>
