<%@ Page Title="" Language="C#" MasterPageFile="~/Master/NewAdminMaster.Master" AutoEventWireup="true" CodeBehind="agentdetail.aspx.cs" Inherits="GroupBooking.Web.Administrator.AgentDetail" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=dda9a637d2fffec4"
Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=dda9a637d2fffec4" namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx"  %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=dda9a637d2fffec4" namespace="DevExpress.Web.ASPxRoundPanel" tagprefix="dx"  %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=dda9a637d2fffec4" 
Namespace="DevExpress.Web.ASPxLoadingPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Data.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=dda9a637d2fffec4" TagPrefix="dx" Namespace="DevExpress.Data" %>
<%@ Register assembly="DevExpress.Web.v16.1" namespace="DevExpress.Web.ASPxPanel" tagprefix="dx" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
   <script type="text/javascript" language="javascript">
       function Validate(s, e) {
           if (ASPxClientEdit.ValidateGroup('testGroup'))
               ClientCallbackPanelDemo.PerformCallback('');
       }
    </script>  
    <div id="wrapper4" >
            <div class="div">
            &nbsp;<dx:ASPxLabel ID="lblHeader" Text="Booking Management" runat="server" 
                Font-Bold="True" Font-Size="Medium"></dx:ASPxLabel></div>
    <br />
        <table>
            <tr>
                <td>
                    <div style="float: left; margin-left: 2%" >
                        <dx:ASPxValidationSummary ID="vsValidationSummary1" runat="server"
                        Width="502px" ClientInstanceName="validationSummary" HorizontalAlign="Left" 
                            Height="16px">
                            <ErrorStyle Wrap="False" />
                            <Border BorderColor="Red" BorderStyle="Double" />
                        <Border BorderColor="Red" BorderStyle="None"></Border>
                        </dx:ASPxValidationSummary>
                        <font color="red">
                            <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                        </font>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    
                </td>
            </tr>
        </table>
    </div>
    <div class="agentdiv">

        <br />
        <br />
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="true" ContainerElementID="RPanelData"/>
         <div id="selectMainBody" class="mainBody form">
                <div class="redSectionHeader">
                  <div><asp:Label ID="lblGridHead" runat="server" Text="Agent Data Profile"></asp:Label></div>
                </div>
    
        <dx:ASPxRoundPanel ID="RPanelData" runat="server" HeaderText="Agent Data Profile" Visible="True" Width="100%" ShowHeader="false" ShowDefaultImages="false" Border-BorderColor="#666666" Border-BorderStyle="Solid">            
        <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server">
        <div class="div">
        <table width="100%" >
        <%-- <tr>
        <td colspan="4">--%>
        <%-- <table width="100%">--%>
           <tr >
                <td  style="width: 25%">
                    Title*</td>
                <td  style="width: 30%">             
                    <dx:ASPxComboBox ID="CmbTitle" runat="server" class="Select" 
                        AutoPostBack="false" IncrementalFilteringMode="StartsWith" Width="170px" >
                        <Items>
                            <dx:ListEditItem Text="Mr" Value="Mr" />
                            <dx:ListEditItem Text="Ms" Value="Ms" />
                        </Items>
                        <ValidationSettings>
                                        <RequiredField ErrorText="Title is required" IsRequired="True" />
                                    <RequiredField IsRequired="True" ErrorText="Title is required"></RequiredField>
                                    </ValidationSettings>
                        </dx:ASPxComboBox>
                </td>
                <td style="width: 2%">
                    </td>
                <td  style="width: 25%">
                
                </td>
                 <td >
                  
                </td>
            </tr>
            <tr >
                <td  style="width: 25%">
                    Login Name*</td>
                <td  style="width: 30%">                 
                
                    <dx:ASPxTextBox ID="txt_AgentName" runat="server"  TabIndex="1" MaxLength="50" 
                        Width="150px">
                   <ValidationSettings >
                        <RequiredField ErrorText="User Name is required" IsRequired="True" />                       
                        <RegularExpression ErrorText="User Name is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$" /> 
                                                                           
<RequiredField IsRequired="True" ErrorText="User Name is required"></RequiredField>

<RegularExpression ErrorText="User Name is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$"></RegularExpression>
                                                                           
                   </ValidationSettings>
                </dx:ASPxTextBox>                            
                </td>
                <td style="width: 2%">
                    </td>
                <td  style="width: 25%">

                    Email*
                </td>
                <td >         
                    <dx:ASPxTextBox ID="txt_Email" runat="server" TabIndex="9" MaxLength="266" 
                        Width="170px">
                        <ValidationSettings>
                            <RequiredField ErrorText="E-mail is required" IsRequired="True" />
                            <RegularExpression ErrorText="Invalid e-mail" 
                                ValidationExpression="^(?=.{1,266}$)\w+([-.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
<RequiredField IsRequired="True" ErrorText="E-mail is required"></RequiredField>

<RegularExpression ErrorText="Invalid e-mail" ValidationExpression="^(?=.{1,266}$)\w+([-.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></RegularExpression>
                        </ValidationSettings>
                    </dx:ASPxTextBox>                 
                </td>
                
            </tr>
            <tr >
                <td  style="width: 25%">
                    Travel Agent Licence No
                <td  style="width: 30%" >
                <dx:ASPxTextBox ID="txtAgentNo" runat="server"  TabIndex="2" MaxLength="50" 
                        Width="170px">
                    <ValidationSettings SetFocusOnError="true">                        
                        <RegularExpression ErrorText="Licence Number is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$" />                                                                            
                        <RegularExpression ErrorText="Licence Number is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$"></RegularExpression>                                                                           
                   </ValidationSettings></dx:ASPxTextBox>
               
                    
                    </td>
                 <td  style="width: 2%">
                     &nbsp;</td>
                 <td  style="width: 25%">
                    First Name*
                </td>
                <td >                 
                    <dx:ASPxTextBox ID="txt_FirstName" runat="server"  MaxLength="50" Width="170px"
                        NullText="First Name" TabIndex="10">
                    <ValidationSettings>
                        <RequiredField ErrorText="First Name is required" IsRequired="True" />
                        <RegularExpression ErrorText="Invalid First Name" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$" />


<RequiredField IsRequired="True" ErrorText="First Name is required"></RequiredField>

<RegularExpression ErrorText="Invalid First Name" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$"></RegularExpression>
                   </ValidationSettings>
                </dx:ASPxTextBox>           
                    
                </td>
            </tr>
            <tr >
                <td  style="width: 25%">
                    Country* </td>
                <td  style="width: 30%">               
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <dx:ASPxComboBox ID="cmbCountry" runat="server" AutoPostBack="true" IncrementalFilteringMode="StartsWith"
                                class="Select" OnSelectedIndexChanged="ddlCountry_OnSelectedIndexChanged" 
                                TabIndex="3" Width="170px">
                                <ValidationSettings>
                                    <RequiredField ErrorText="Country is required" IsRequired="True" />
                                </ValidationSettings>
                                <ClientSideEvents ValueChanged="function(s, e) {                                                                                                                                 
                                                                // Prevent multiple presses of the button
                                                                    LoadingPanel.SetText('Loading Data from server...');
                                                                    LoadingPanel.Show();                                                                    
                                                                    setTimeout(function() {LoadingPanel.Hide()},2000);                                                                                                    
                                                             }" />
                            </dx:ASPxComboBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                         
                </td>
                <td  style="width: 2%">
                    &nbsp;</td>
                <td  style="width: 25%">
                    Last Name*
                </td>
                <td >
                        <dx:ASPxTextBox ID="txt_LastName" runat="server" MaxLength="50"  Width="170px"
                        NullText="Last Name" TabIndex="11">
                    <ValidationSettings>
                        <RequiredField ErrorText="Last Name is required" IsRequired="True" />
                        <RegularExpression ErrorText="Invalid Last Name" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$" />

<RequiredField IsRequired="True" ErrorText="Last Name is required"></RequiredField>

<RegularExpression ErrorText="Invalid Last Name" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$"></RegularExpression>

                   </ValidationSettings>
                </dx:ASPxTextBox>   
                    
                </td>
            </tr>
            <tr >
                <td  style="width: 25%">
                    State </td>
                <td  style="width: 30%">               
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <dx:ASPxComboBox ID="cmbState" Width="170px" runat="server" 
                                AutoPostBack="True"  IncrementalFilteringMode="StartsWith"
                                class="Select" TabIndex="4" >
                            </dx:ASPxComboBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                         
                </td>
                
                <td  style="width: 2%">
                    &nbsp;</td>
                
        <td  style="width: 25%">
                    Tel*
                </td>
                <td >            
                    <dx:ASPxTextBox ID="txt_tel" runat="server"  TabIndex="12" MaxLength="20" 
                        Width="170px">
                    <ValidationSettings>
                        <RequiredField ErrorText="Telephone number is required" IsRequired="True" />
                        <RegularExpression ErrorText="Invalid Telephone number" ValidationExpression="^([\d*]{1,20})$" />

                        <RequiredField IsRequired="True" ErrorText="Telephone number is required"></RequiredField>

                        <RegularExpression ErrorText="Invalid Telephone number" ValidationExpression="^([\d*]{1,20})$"></RegularExpression>

                   </ValidationSettings>
                </dx:ASPxTextBox>   
                    
                </td>
            </tr>     


            <tr >
                <td  style="width: 25%">
                    Address*
                </td>
                <td  style="width: 30%">              
                    <dx:ASPxTextBox ID="txtAddress1" runat="server"  TabIndex="5" Width="170px"
                        MaxLength="150" AutoPostBack="True">
                    
                    <ValidationSettings SetFocusOnError="true">
                        <RequiredField ErrorText="Address1 is required" IsRequired="True" />                       
                        <RegularExpression ErrorText="Address1 is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,150})$" /> 
                                                                           
<RequiredField IsRequired="True" ErrorText="Address is required"></RequiredField>

<RegularExpression ErrorText="Address is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,150})$"></RegularExpression>
                                                                           
                   </ValidationSettings>
                </dx:ASPxTextBox>
                        
                </td>
                <td style="width: 2%">
                    &nbsp;</td>
                <td  style="width: 25%">
                    Mobile Phone*
                </td>
                <td >         
                    <dx:ASPxTextBox ID="txt_MobilePhone" runat="server"  TabIndex="13" 
                        MaxLength="20" Width="170px">
                    <ValidationSettings>
                        <RequiredField ErrorText="Mobile Number is required" IsRequired="True" />    
                        <RegularExpression ErrorText="Invalid Mobile Number" ValidationExpression="^([\d*]{1,20})$" />
                        <RequiredField IsRequired="True" ErrorText="Mobile Number is required"></RequiredField>
                        <RegularExpression ErrorText="Invalid Mobile Number" ValidationExpression="^([\d*]{1,20})$"></RegularExpression>
                   </ValidationSettings>
                </dx:ASPxTextBox>     
                    
                </td>
            </tr>
             <tr >
                <td  style="width: 25%">
                  
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td>
                <td  style="width: 30%">                    
                    <dx:ASPxTextBox ID="txtAddress2" runat="server" Height="19px"  MaxLength="150" Width="170px"
                        TabIndex="6">
                    
                    <ValidationSettings SetFocusOnError="true">
                      
                        <RegularExpression ErrorText="Address2 is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,150})$" /> 
                                                                           
<RegularExpression ErrorText="Address2 is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,150})$"></RegularExpression>
                                                                           
                   </ValidationSettings>
                </dx:ASPxTextBox>
                    
                </td>
                 <td style="width: 2%">
                     &nbsp;</td>
                <td  style="width: 25%">
                    Fax
                </td>
                <td >                    
                    <dx:ASPxTextBox ID="txt_Fax" runat="server"  TabIndex="14" MaxLength="20" 
                        Width="170px">
                    <ValidationSettings>
                        <RegularExpression ErrorText="Invalid Fax" ValidationExpression="^([\d*]{1,20})$" />

<RegularExpression ErrorText="Invalid Fax" ValidationExpression="^([\d*]{1,20})$"></RegularExpression>

                   </ValidationSettings>
                </dx:ASPxTextBox>
                </td>
            </tr>
              <tr >
                    <td  style="width: 25%">
                        &nbsp;</td>
                    <td  style="width: 30%">
                        <dx:ASPxTextBox ID="txtAddress3" runat="server" Height="19px" Width="170px"
                        TabIndex="6">
                    
                    <ValidationSettings SetFocusOnError="true">
                      
                        <RegularExpression ErrorText="Address3 is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,150})$" /> 
                                                                           
<RegularExpression ErrorText="Address3 is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,150})$"></RegularExpression>
                                                                           
                   </ValidationSettings>
                </dx:ASPxTextBox>
                        </td>
                    <td  style="width: 2%">
                        &nbsp;</td>
                <td  style="width: 25%">
                    Password*</td>
                <td >              
                    <dx:ASPxTextBox ID="txt_AgentPWD" runat="server" ClientInstanceName="AgentPWD"  
                        MaxLength="16" Width="170px"
                        Password="True" TabIndex="15" >
                        <ClientSideEvents Validation="function(s, e) {AgentPWD2.isValid = (s.GetText() == AgentPWD.GetText());}" />
<ClientSideEvents Validation="function(s, e) {AgentPWD2.isValid = (s.GetText() == AgentPWD.GetText());}"></ClientSideEvents>

                        <ValidationSettings>
                            <RegularExpression ErrorText="Invalid Password" ValidationExpression="^.*(?=.{8,16})(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).*$|^.*(?=.{8,16})(?=.*\d)(?=.*[a-z])(?=.*[@#$%^&+=-_)(*!]).*$|^.*(?=.{8,16})(?=.*\d)(?=.*[A-Z])(?=.*[@#$%^&+=_)(*!]).*$|^.*(?=.{8,16})(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=_)(*!]).*$"></RegularExpression>
                        
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                </td>
            </tr>
             <tr >
                <td  style="width: 25%">
                    City* </td>
                <td  style="width: 30%">                    
                    <dx:ASPxTextBox ID="txtCity" runat="server" TabIndex="7" Width="170px">
                        <ValidationSettings>
                            <RegularExpression ErrorText="Invalid City" 
                                ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$" />
                            <RequiredField ErrorText="City is required" IsRequired="True" />
<RegularExpression ErrorText="Invalid City" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$"></RegularExpression>

<RequiredField IsRequired="True" ErrorText="City is required"></RequiredField>
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                </td>
                 
                 <td  style="width: 2%">
                     &nbsp;</td>
                 
                 <td  style="width: 25%">
                     Re-enter Password*</td>
                <td >                    
                    <dx:ASPxTextBox ID="txt_AgentPWD2" runat="server" 
                        ClientInstanceName="AgentPWD2" Password="True" TabIndex="16"  
                        MaxLength="16" Width="170px">
                        <ClientSideEvents Validation="function(s, e) {e.isValid = (s.GetText() == AgentPWD.GetText());}" />
<ClientSideEvents Validation="function(s, e) {e.isValid = (s.GetText() == AgentPWD.GetText());}"></ClientSideEvents>

                        <ValidationSettings ErrorText="The password is incorrect">
                            <RegularExpression ErrorText="Invalid Password" ValidationExpression="^.*(?=.{8,16})(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).*$|^.*(?=.{8,16})(?=.*\d)(?=.*[a-z])(?=.*[@#$%^&+=-_)(*!]).*$|^.*(?=.{8,16})(?=.*\d)(?=.*[A-Z])(?=.*[@#$%^&+=_)(*!]).*$|^.*(?=.{8,16})(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=_)(*!]).*$"></RegularExpression>
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                   
                </td>
            </tr>
           <tr >
            <td  style="width: 25%">
            
                Postcode</td>
            <td  style="width: 30%">
                <dx:ASPxTextBox ID="txtPostcode" runat="server" TabIndex="8"  MaxLength="10" 
                    Width="170px">
                    <ValidationSettings>
                        <RegularExpression ErrorText="Invalid Postcode" 
                            ValidationExpression="^([\d*]{1,10})$" />
<RegularExpression ErrorText="Invalid Postcode" ValidationExpression="^([\d*]{1,10})$"></RegularExpression>
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </td>
            <td style="width: 2%">
            
            </td>
            <td colspan="2" align="left">
                 <dx:ASPxLabel ID="lblInfo" 
                     Text="Password must contain at least 3 of 4 category: small letter, capital letter, number and special characters" 
                     runat="server" Font-Size="X-Small" Width="250px" 
                         ></dx:ASPxLabel>
            </td>

           </tr>

         
            <tr >
           
                <td colspan="5">
              <hr />     
                </td>
            </tr>
          
               
                <tr >
                    <td  style="width: 25%">
                        Bank Name
                    </td>
                    <td  style="width: 30%">
                       
                        <dx:ASPxTextBox ID="txtBankName" runat="server"  TabIndex="17" MaxLength="200" 
                            Width="170px">
                            <ValidationSettings SetFocusOnError="True">                                
                                <RegularExpression ErrorText="Bank name is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,200})$" />
                                <RegularExpression ErrorText="Bank name is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,200})$"></RegularExpression>
                            </ValidationSettings>
                        </dx:ASPxTextBox>

                    </td>
                    <td  style="width: 2%">
                        &nbsp;</td>
                    <td  style="width: 25%">
                        Country
                    </td>
                    <td >
                       
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <dx:ASPxComboBox ID="cmbCountryBank" runat="server" AutoPostBack="True"  IncrementalFilteringMode="StartsWith"
                                    class="Select" 
                                    OnSelectedIndexChanged="ddlCountryBank_OnSelectedIndexChanged" 
                                    TabIndex="23" Width="170px">

                                    <ClientSideEvents ValueChanged="function(s, e) {                                                                                                                                 
                                                                // Prevent multiple presses of the button
                                                                    LoadingPanel.SetText('Loading Data from server...');
                                                                    LoadingPanel.Show();                                                                    
                                                                    setTimeout(function() {LoadingPanel.Hide()},2000);                                                                                                    
                                                             }" />
                                </dx:ASPxComboBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                       
                    </td>
                </tr>
                <tr >
                    <td  style="width: 25%">
                        Address
                    </td>
                    <td  style="width: 30%">
                        <dx:ASPxTextBox ID="txtBankAddress1" runat="server" TabIndex="18" 
                            MaxLength="150" Width="170px">
                            <ValidationSettings SetFocusOnError="True">
                                <RegularExpression ErrorText="Bank Address1 is invalid" 
                                    ValidationExpression="^([1-zA-Z0-1@.\s]{1,150})$" />
<RegularExpression ErrorText="Bank Address1 is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,150})$"></RegularExpression>
                            </ValidationSettings>
                        </dx:ASPxTextBox>
                    </td>
                    <td  style="width: 2%">
                        &nbsp;</td>
                    <td  style="width: 25%">
                        State
                    </td>
                    <td >
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <dx:ASPxComboBox ID="cmbStateBank" runat="server" AutoPostBack="True"  IncrementalFilteringMode="StartsWith"
                                    class="Select" TabIndex="24" Width="170px">

                                </dx:ASPxComboBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>

                </tr>
                <tr >
                    <td  style="width: 25%">
                    </td>
                    <td  style="width: 30%">
                        <dx:ASPxTextBox ID="txtBankAddress2" runat="server"  TabIndex="19" 
                            MaxLength="150" Width="170px">
                            <ValidationSettings SetFocusOnError="True">
                                <RegularExpression ErrorText="Bank Address2 is invalid" 
                                    ValidationExpression="^([1-zA-Z0-1@.\s]{1,150})$" />
<RegularExpression ErrorText="Bank Address2 is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,150})$"></RegularExpression>
                            </ValidationSettings>
                        </dx:ASPxTextBox>
                    </td>
                    <td style="width: 2%">
                        &nbsp;</td>
                  <td  style="width: 25%">
                        City
                    </td>
                    <td >
                        <dx:ASPxTextBox ID="txtCityBank" runat="server"  TabIndex="25" MaxLength="50" 
                            Width="170px">
                            <ValidationSettings>
                                <RegularExpression ErrorText="Invalid City" 
                                    ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$" />
<RegularExpression ErrorText="Invalid City" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$"></RegularExpression>
                            </ValidationSettings>
                        </dx:ASPxTextBox>
                    </td>
                </tr>
                <tr >
                    <td  style="width: 25%">
                        &nbsp;</td>
                    <td  style="width: 30%">
                        <dx:ASPxTextBox ID="txtBankAddress3" runat="server" TabIndex="20"  
                            MaxLength="150" Width="170px">
                            <ValidationSettings SetFocusOnError="True">
                                <RegularExpression ErrorText="Bank Address3 is invalid" 
                                    ValidationExpression="^([1-zA-Z0-1@.\s]{1,150})$" />
<RegularExpression ErrorText="Bank Address3 is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,150})$"></RegularExpression>
                            </ValidationSettings>
                        </dx:ASPxTextBox>
                    </td>
                    <td style="width: 2%">
                        &nbsp;</td>
                    <td  style="width: 25%">
                        Postcode
                    </td>
                    <td >
                        <dx:ASPxTextBox ID="txtPostcodeBank" runat="server"  TabIndex="26" 
                            MaxLength="20" Width="170px">
                            <ValidationSettings>
                                <RegularExpression ErrorText="Invalid Postcode" 
                                    ValidationExpression="^([1-zA-Z0-1@.\s]{1,20})$" />
<RegularExpression ErrorText="Invalid Postcode" ValidationExpression="^([1-zA-Z0-1@.\s]{1,20})$"></RegularExpression>
                            </ValidationSettings>
                        </dx:ASPxTextBox>
                    </td>
                </tr>               
                <tr >
                    <td  style="width: 25%">
                        Account Name</td>
                    <td  style="width: 30%">
                       
                        <dx:ASPxTextBox ID="txtBankAccountName" runat="server" TabIndex="20" 
                            MaxLength="200" Width="170px">
                            <ValidationSettings>
                                <RegularExpression ErrorText="Invalid Account Name" ValidationExpression="^([1-zA-Z0-1@.\s]{1,200})$" />                                
                                <RegularExpression ErrorText="Invalid Account Name" ValidationExpression="^([1-zA-Z0-1@.\s]{1,200})$"></RegularExpression>
                            </ValidationSettings>
                        </dx:ASPxTextBox>
                        
                    </td>
                       <td  style="width: 2%">
                           &nbsp;</td>
                       <td  style="width: 25%">
                            Operation Group</td>
                    <td >
                       <dx:ASPxCheckBox ID="chkAirAsia" runat="server" Text="AirAsia"  Checked="false" EnableViewState="false" >
                       </dx:ASPxCheckBox> 
                      <dx:ASPxCheckBox ID="chkAirAsiaX" runat="server" Text="AirAsia X"  Checked="false" EnableViewState="false" />
                        </td>
                </tr>
                <tr >
                <td  style="width: 25%">
                    Bank Account No</td>
                <td  style="width: 30%">
                    <dx:ASPxTextBox ID="txtBankAccountno" runat="server" TabIndex="22" 
                        MaxLength="50" Width="170px"
                        >
                        <ValidationSettings>
                            <RegularExpression ErrorText="Invalid Bank Account No" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$" />
                            <RegularExpression ErrorText="Invalid Bank Account No" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$"></RegularExpression>
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                </td>
                <td  style="width: 2%">
                           &nbsp;</td>
                       <td  style="width: 25%">
                           &nbsp;</td>
                    <td >
                        
                        &nbsp;</td>
                </tr>
                <tr >
                    <td  style="width: 25%">
                    
                        </td>         
                        <td  style="width: 30%">
                        <dx:ASPxCheckBox ID="chkActive" runat="server" ClientInstanceName="checkBox" Text="Active" EnableViewState="false" Checked="true">       
                        </dx:ASPxCheckBox>
                        </td>                        
                    <td  style="width: 2%">
                        &nbsp;</td>
                </tr>
             <%--</table>
             </td>
             </tr>--%>
                <tr >
                
                    <td class="style12" style="width: 25%">
                    </td>
                </tr>
                <tr >
                    <td class="style1" colspan="3">
                        &nbsp; 
                    </td>
                </tr>
                <tr >
                    <td class="style1" colspan="3">
                        &nbsp;</td>
            </tr>
                <tr >
                    <td style="text-align: right; width: 25%;" class="style13">
                        
                        
                    </td>
                </tr>
             </table>
             <table>
                <tr>
                    <td>

                                <dx:ASPxButton CssClass="button_2"  ID="btnSave" runat="server" Height="19px" 
                            OnClick="btnSubmit_Click" Text="Save" Width="88px">
                            <ClientSideEvents Click="function(s, e) { 
                                                                if(ASPxClientEdit.AreEditorsValid()) {
                                                                
                                                                // Prevent multiple presses of the button
                                                                    LoadingPanel.SetText(&#39;Sending data to the server...&#39;);
                                                                    LoadingPanel.Show();
                                                                }
                                                             }"></ClientSideEvents>
                        </dx:ASPxButton>
                                </td>
                                <td >
                                <dx:ASPxButton ID="btnSendPassword" runat="server" Text="Email Notification" Height="19px" Width="148px"
                                        CssClass="button_3"  
                                        OnClick="btnSendEmail_Click"> 
                                </dx:ASPxButton>
                                </td>
                                <td >
                                <dx:ASPxButton CssClass="button_2"  ID="BtnPrint" runat="server" Height="19px"  
                                Text="Print" Width="88px" CausesValidation="False" Visible="False">
                            
                        </dx:ASPxButton>
                                </td>
                                <td>
                                <dx:ASPxButton CssClass="button_2"  ID="BtnBack" runat="server" Height="19px" 
                            OnClick="BtnBack_Click" Text="Back" Width="88px" CausesValidation="False">
                            
                            <ClientSideEvents Click="function(s, e) { 
                            if(ASPxClientEdit.AreEditorsValid()) {
                                                                
                            // Prevent multiple presses of the button
                                LoadingPanel.SetText(&#39;Loading data...&#39;);
                                LoadingPanel.Show();
                            }
                            }"></ClientSideEvents>
                        </dx:ASPxButton>
                                </td>
                            </tr>
                        <%--</table>--%>
                        
                        
                    </td>             
                                     
                </tr>
        </table>
        </div>
 </dx:PanelContent>
</PanelCollection>
            
<Border BorderColor="#666666" BorderStyle="Solid" BorderWidth="1px"></Border>
            
            </dx:ASPxRoundPanel>  
    

  

      
                <br />
            </div>
        <br />
</div>
    <script type="text/javascript" language="javascript">
        function Confirm(tags) {
            if (tags == "1") {
                if (confirm("Are you sure move this agent into Blacklist?") == true) {
                    return true;
                }
                else {
                    return false;
                }
            }
            if (tags == "0") {
                if (confirm("Are you sure move out this agent from BlackList?")) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        
    </script>
 </asp:Content>

