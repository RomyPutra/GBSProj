<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewAdminMaster.Master" AutoEventWireup="true" CodeBehind="userlist.aspx.cs" Inherits="GroupBooking.Web.admin.userlist" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
         <script type="text/javascript">
    // <![CDATA[

             var keyValue;
             function OnEdit(element, key) {
                 callbackPanel.SetContentHtml("");
                 popup.ShowAtElement(element);
                 keyValue = key;
             }
             function popup_Shown(s, e) {
                 callbackPanel.PerformCallback(keyValue);
             }
    // ]]>
        </script>
<dx:ASPxPopupControl ID="popup" ClientInstanceName="popup" 
        runat="server" Modal="true" AllowDragging="true" 
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"        
        HeaderText="User Data"  >

        <ClientSideEvents Shown="popup_Shown" />
<CloseButtonImage Url="~/Images/AirAsia/close_button_icon.jpg"></CloseButtonImage>

<ClientSideEvents Shown="popup_Shown"></ClientSideEvents>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
             <dx:ASPxCallbackPanel ID="callbackPanel" ClientInstanceName="callbackPanel" runat="server"
                    Width="398px" Height="324px" OnCallback="callbackPanel_Callback" RenderMode="Table">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                        <dx:ASPxLoadingPanel runat="server" ID="LoadingPanelPop" ClientInstanceName="LoadingPanelPop" Modal="True">
                         </dx:ASPxLoadingPanel>
                            &nbsp;&nbsp;<font color="red"> </font>
                            <table width="100%" class="tableClass">
                            <tr>
                                <td colspan="3">
                                     <dx:ASPxLabel ID="lblHeadReq" Text=" Title" runat="server" Font-Bold="true" Font-Size="Small"></dx:ASPxLabel>
                                </td>
                                </tr>
                                <tr>
                                <td>
                                &nbsp;
                                </td>
                                </tr>
                                <tr>

                                 <td>
                                User ID
                                </td>
                                <td>
                                    <dx:ASPxTextBox ID="txt_UserID" runat="server" Width="200px" TabIndex="1">
                                           <ValidationSettings errordisplaymode="ImageWithTooltip" >
                                                <RequiredField ErrorText="User ID is required" IsRequired="True" />                       
                                                <RegularExpression ErrorText="User ID is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$" /> 
                                                                           
                        <RequiredField IsRequired="True" ErrorText="User ID is required"></RequiredField>

                        <RegularExpression ErrorText="User Name is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$"></RegularExpression>
                                                                           
                                           </ValidationSettings>
                                        </dx:ASPxTextBox>
                                </td>
                              
                                </tr>
                                <tr>
                                    
                                  <td >
                                User Name
                                </td>
                                <td>
                                    <dx:ASPxTextBox ID="txtUsername" runat="server" Width="200px" TabIndex="1" MaxLength="50">
                                           <ValidationSettings errordisplaymode="ImageWithTooltip" >
                                                <RequiredField ErrorText="User Name is required" IsRequired="True" />                       
                                                <RegularExpression ErrorText="User Name is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$" /> 
                                                                           
                                        <RequiredField IsRequired="True" ErrorText="txtRefID is required"></RequiredField>

                                        <RegularExpression ErrorText="User Name is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$"></RegularExpression>
                                                                           
                                           </ValidationSettings>
                                        </dx:ASPxTextBox>
                                </td>

                                </tr>
                                <tr>
                                 <td>
                                        User Password
                                        </td>       
                                               <td>
                                            <dx:ASPxTextBox ID="txtPassword" runat="server" Width="200px" TabIndex="1"  MaxLength="16"
                                                       Password="True">
                                                   <ValidationSettings errordisplaymode="ImageWithTooltip" >
                                                        <RegularExpression ErrorText="Password length should be between 8 to 16 chars and should contain at least one alphabet, one number and two symbols" ValidationExpression="^.*(?=.{8,16})(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).*$|^.*(?=.{8,16})(?=.*\d)(?=.*[a-z])(?=.*[@#$%^&+=]).*$|^.*(?=.{8,16})(?=.*\d)(?=.*[A-Z])(?=.*[@#$%^&+=]).*$|^.*(?=.{8,16})(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=]).*$"></RegularExpression>
                                 
                                                                           
                                                   </ValidationSettings>
                                                </dx:ASPxTextBox>
                                        </td>                                        
                                </tr>
                                <tr>
                                <td>
                                        Reference ID
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txtRefID" runat="server" Width="200px" TabIndex="1" MaxLength="50">
                                                   <ValidationSettings errordisplaymode="ImageWithTooltip" >
                                                        <RequiredField ErrorText="txtRefID is required" IsRequired="True" />                       
                                                        <RegularExpression ErrorText="User Name is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$" /> 
                                                                           
                                                <RequiredField IsRequired="True" ErrorText="User Name is required"></RequiredField>

                                                <RegularExpression ErrorText="User Name is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$"></RegularExpression>
                                                                           
                                                   </ValidationSettings>
                                                </dx:ASPxTextBox>
                                        </td>
                                </tr>
                                 <tr>
                                <td>
                                User Group
                                </td>
                                <td class="dataClass noMinWidth">
                                    <dx:ASPxComboBox runat="server" SelectedIndex="0" AutoPostBack="false" ID="cmbGroup">
                                        <Items>
                                            <dx:ListEditItem Selected="True" Text="Operator" Value="01"></dx:ListEditItem>
                                            <dx:ListEditItem Text="Supervisor" Value="02"></dx:ListEditItem>
                                            <dx:ListEditItem Text="Manager" Value="03"></dx:ListEditItem>
                                            <dx:ListEditItem Text="Viewer" Value="04"></dx:ListEditItem>
                                        </Items>
                                    </dx:ASPxComboBox>
                                </td>                              
                                </tr>
                                <tr>
                                   <td >
                                    Operation Group
                                    </td>
                                    <td>
                                       <dx:ASPxCheckBox ID="chkAirAsia" runat="server" ClientInstanceName="checkBox" Text="AirAsia" EnableViewState="false" Checked="false">       
                                        </dx:ASPxCheckBox>
                                    </td>                               
                                </tr>
                                 <tr>
                                <td>&nbsp;</td>
                                <td> 
                                       <dx:ASPxCheckBox ID="chkAirAsiaX" runat="server" ClientInstanceName="checkBox" Text="AirAsia X" EnableViewState="false" Checked="false">       
                                        </dx:ASPxCheckBox>                                    
                                </td>
                                </tr>
                                 <tr>
                                   <td >
                                    Status
                                    </td>
                                    <td>
                                       <dx:ASPxCheckBox ID="chkActive" runat="server" ClientInstanceName="checkBox" Text="Active" EnableViewState="false" Checked="true">       
                                        </dx:ASPxCheckBox>
                                    </td>                               
                                </tr>
                                <tr>
                                <td>
                                <dx:ASPxButton CssClass="button_2" ID="btnSave" runat="server"  
                                Text="Save"  OnClick="btnSave_Click" CausesValidation="true"  >
                                      <ClientSideEvents Click="function OnCloseButtonClick(s, e) {
                                      if(ASPxClientEdit.AreEditorsValid()) {
                                      var parentWindow = window.parent;
                                      LoadingPanelPop.SetText(&#39;Please Wait...&#39;);
                                      LoadingPanelPop.Show();
                                      parentWindow.popup.Hide();}}" />                                            
                                <ClientSideEvents Click="function OnCloseButtonClick(s, e) {
                                      if(ASPxClientEdit.AreEditorsValid()) {
                                      var parentWindow = window.parent;
                                      LoadingPanelPop.SetText(&#39;Please Wait...&#39;);
                                      LoadingPanelPop.Show();
                                      parentWindow.popup.Hide();}}"></ClientSideEvents>
                                    </dx:ASPxButton>
                                    </td>
                                </tr>
                             </table>
                        </dx:PanelContent> 
                        </PanelCollection> 
                </dx:ASPxCallbackPanel> 
            </dx:PopupControlContentControl> 
        </ContentCollection> 
</dx:ASPxPopupControl> 
  
  
 <div id="wrapper4" >
    <div class="page-header row">
                <span style="display:none;">         
            &nbsp;<dx:ASPxLabel ID="lblHeader" Text="User List" runat="server" 
                Font-Bold="True" Font-Size="Medium"></dx:ASPxLabel>
                    </span>
        <div class="col-sm-4" style="text-align:left;">
        <h4>User List</h4>
        </div>
                    </div>
     <div class="page-content row">
        <table>          
            <tr>
            <td style="width: 100px">
                <dx:ASPxComboBox ID="cmbFilter" runat="server" SelectedIndex="0" 
                    Width="90%">
                    <Items>                        
                        <dx:ListEditItem Selected="True" Text="User ID" Value="UserID" />
                        <dx:ListEditItem Text="Username" Value="Username" />
                    </Items>
                </dx:ASPxComboBox>
            </td>
            <td style="width: 180px">
              <dx:ASPxTextBox ID="txtSearch" runat="server" Width="170px" >
            </dx:ASPxTextBox>
            
            </td>

            <td style="width: 436px">
                <dx:ASPxButton CssClass="buttonL2 noBgImg search" ID="btnSearch" runat="server" OnClick="btnSearch_Click" CausesValidation="False" 
                    Text="Search">
                </dx:ASPxButton>
            </td>
            <td>
            &nbsp;
            </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>      
            <td>
                <font color="red">
                        <dx:ASPxLabel ID="lblMsg" runat="server" Text="Label" Visible="False"></dx:ASPxLabel>
                </font>
            </td>           
            </tr>         
            <tr>
            <td colspan="4">
               <dx:ASPxGridView ID="gridAgent" ClientInstanceName="gridAgent" runat="server" 
                KeyFieldName="UserID" Width="100%" 
                                AutoGenerateColumns="False" SettingsBehavior-ColumnResizeMode="Control" 
                                    >                                   
                <Columns>                           
                    <dx:GridViewDataColumn Name="Edit" Caption="Action" VisibleIndex="0" Width="55px">
                                <DataItemTemplate>
                                    <a href="javascript:void(0);" onclick="OnEdit(this, '<%# Container.KeyValue %>')">
                                        Edit</a>
                                </DataItemTemplate>
                            </dx:GridViewDataColumn>         
                 
                    <dx:GridViewDataColumn FieldName="UserID" Caption = "User ID" VisibleIndex="1" />
                    <dx:GridViewDataColumn FieldName="UserName" Caption = "User Name" VisibleIndex="2" />
                    <dx:GridViewDataColumn FieldName="RefID" Caption = "Reference ID" VisibleIndex="3" />
                    <dx:GridViewDataColumn FieldName="Status" Caption = "Status" VisibleIndex="4" />
                    <dx:GridViewDataColumn FieldName="LastUpdate" Caption = "Last Update" VisibleIndex="5" />
                    <dx:GridViewDataColumn FieldName="UpdateBy" Caption="Update By" VisibleIndex="6" />
                </Columns>
                <%--<SettingsBehavior AllowFocusedRow="True"   AllowMultiSelection ="false" ProcessFocusedRowChangedOnServer ="true" />--%>

<SettingsBehavior ColumnResizeMode="Control"></SettingsBehavior>

                <Settings showverticalscrollbar="True" VerticalScrollableHeight="300"></Settings>
                   <Styles>
                       <Header BackColor="#333333" ForeColor="White">
                       </Header>
                   </Styles>
            </dx:ASPxGridView>            
                       
              
            </td>
            </tr>
            <tr>
            <td>
                <br />
                <!-- amended by diana 20130902 : change devexpress button to html link -->
                <style type="text/css">
                   .divAdd
                   {
                       background-position: -289px -37px;
                       font: normal 12px Tahoma;
                       margin: 2px 0px;
                       border: 0px currentColor;
                       width: 110px;
                       height: 23px;
                       font-size: 12px;
                       font-weight: bold;
                       cursor: pointer;
                       background-image:url("../images/AKBase/sprite3.png");
                       background-color: rgb(238, 238, 238);
                       text-align:center;
                    }
                    .divAdd a 
                    {
                        color: rgb(255, 255, 255);
                        text-decoration:none;
                        position:relative;
                        top:3px;
                    } 
                    .divAdd a:hover
                    {
	                    color: rgb(255, 255, 255);
	                    text-decoration:none;
	                    color: #000000;
                    }
                </style>
                <div class="dxbButton buttonL2 noBgImg">
                    <div>
                        <div class="toPad"><span >
                <a href="javascript:void(0)" onclick="OnEdit(this,'new')" class="btnAdd">Add New</a>
                         </span></div>
                    </div>
                </div>
                

            <%--
            <dx:ASPxButton CssClass="button_2" ID="btnNew" runat="server" AutoPostBack="false" CausesValidation="False" 
                    Text="Add New">
                    <ClientSideEvents Click="function(s, e) { OnEdit(this,'new'); }" />
                </dx:ASPxButton> 
            --%>     
            </td>
            </tr>
        </table> 
    </div>
    

</asp:Content>


