<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Newblankmaster.master" AutoEventWireup="true" CodeBehind="profile.aspx.cs" Inherits="GroupBooking.Web.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <p>
        &nbsp;&nbsp;</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <div class="div">
            <dx:ASPxLabel ID="lblHeader" Text="Agent registration" runat="server" 
                Font-Bold="True" Font-Size="Medium"></dx:ASPxLabel></div>
<dx:aspxpanel ID="PanelRegister" runat="server" >

    <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
        <hr />
        <font color="red">*</font><font style="font-size: 11px;"> Denotes required field</font>
        <br />
        <br />
        <table>
            <tr>
                <td colspan="2" >
                    <dx:ASPxLabel ID="lblInfo" Text="Please register your infomation as below,your details will be sent to our service for evaluation" runat="server" Font-Size="X-Small" width="490"></dx:ASPxLabel>
                </td>
            </tr>
        </table>
        <table width="100%" id="tblTop" runat="server">
            <tr class="trheight" runat="server" ID="trLogin1" style="background-color:#333333;color:White;">
                <td colspan="2" class="tdcol" style="height: 22px">
                    &nbsp;Login Details
                </td>
            </tr>
            <tr class="trheight" runat="server" ID="trLogin2">
                <td class="td1" style="width: 135px">
                    Login Name<font color="red">*</font>
                </td>
                <td class="td2" >                 
                    <dx:ASPxTextBox ID="txt_AgentName" runat="server" Width="200px" MaxLength="50">
                        <MaskHintStyle>
                            <Border BorderColor="#FF3300" />
                        <Border BorderColor="#FF3300"></Border>
                        </MaskHintStyle>
                    <ValidationSettings SetFocusOnError="true">
                    <RequiredField IsRequired="True" ErrorText="Login Name is required"></RequiredField>
                    <RegularExpression ErrorText="Login Name is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$"></RegularExpression>
                   </ValidationSettings>
                </dx:ASPxTextBox>
                </td>
            </tr>
           <tr class="trheight" runat="server" ID="headPassword" visible="false" style="background-color:#333333;color:White;">
              <td colspan="2" class="tdcol" style="height: 22px">
                    &nbsp;Password Change
                </td>

           </tr>
           <tr class="trheight" runat="server"  ID="trPass1" visible="false">
            <td class="td1" style="width: 135px">
                    Enter Current Password<font color="red">*</font>
                </td>
                <td class="td2">                 
                    <dx:ASPxTextBox ID="txtCurrentPass" runat="server" Width="200px" Password="True" ClientInstanceName="AgentCurrentPWD" MaxLength="16" Autocomplete="off">                   
                    <ValidationSettings >             
                    <RequiredField IsRequired="True" ErrorText="Password is required"></RequiredField>
                   </ValidationSettings>
                </dx:ASPxTextBox>
                </td>           
            </tr>
            <tr class="trheight" runat="server"  ID="trPass2">
                <td class="td1" style="width: 135px">
                    Password<font color="red">*</font> 
                </td>
                <td class="td2">                 
                    <dx:ASPxTextBox ID="txt_AgentPWD" runat="server" Width="200px" Password="True" ClientInstanceName="AgentPWD" MaxLength="16" Autocomplete="off">
                     <clientsideevents Validation="function(s, e) {AgentPWD2.isValid = (s.GetText() == AgentPWD2.GetText());}" />
                     <ClientSideEvents Validation="function(s, e) {AgentPWD2.isValid = (s.GetText() == AgentPWD.GetText());}"></ClientSideEvents>
                    <ValidationSettings>
                    <%--<RegularExpression ErrorText="Invalid Password" ValidationExpression="^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])[A-Za-z0-9]{8,16}$"></RegularExpression>--%>
                    <RegularExpression ErrorText="Invalid Password" ValidationExpression="^.*(?=.{8,16})(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).*$|^.*(?=.{8,16})(?=.*\d)(?=.*[a-z])(?=.*[@#$%^&+=-_)(*!]).*$|^.*(?=.{8,16})(?=.*\d)(?=.*[A-Z])(?=.*[@#$%^&+=_)(*!]).*$|^.*(?=.{8,16})(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=_)(*!]).*$"></RegularExpression>
                    <RequiredField IsRequired="True" ErrorText="Password is required"></RequiredField>
                   </ValidationSettings>
                </dx:ASPxTextBox>
                </td>
            </tr>
            <tr class="trheight" runat="server"  ID="trPass3">
                <td class="td1" style="width: 135px">
                    Re-enter Password<font color="red">*</font>      
                </td>
                <td class="td2">          
                    <dx:ASPxTextBox ID="txt_AgentPWD2" runat="server" Width="200px" Password="True" ClientInstanceName="AgentPWD2" MaxLength="16" Autocomplete="off">
                     <clientsideevents Validation="function(s, e) {e.isValid = (s.GetText() == AgentPWD.GetText());}" />
                    <ClientSideEvents Validation="function(s, e) {e.isValid = (s.GetText() == AgentPWD.GetText());}"></ClientSideEvents>
                    <ValidationSettings ErrorText="The password is incorrect">
                    <RegularExpression ErrorText="Invalid Password" ValidationExpression="^.*(?=.{8,16})(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).*$|^.*(?=.{8,16})(?=.*\d)(?=.*[a-z])(?=.*[@#$%^&+=-_)(*!]).*$|^.*(?=.{8,16})(?=.*\d)(?=.*[A-Z])(?=.*[@#$%^&+=_)(*!]).*$|^.*(?=.{8,16})(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=_)(*!]).*$"></RegularExpression>
                     <RequiredField IsRequired="True" ErrorText="Password is required"></RequiredField>                     
                   </ValidationSettings>
                </dx:ASPxTextBox>
                </td>
            </tr>
</table>
 <dx:ASPxLabel ID="lblPassInfo" Text="Password must contain at least 3 of 4 category: 
 Small letter, capital letter, number and special characters" runat="server" Font-Size="X-Small" width="490"
                         ></dx:ASPxLabel>

        <table id="tblBottom" runat="server" width="100%">
            <tr class="trheight" style="background-color:#333333;color:White;">
                <td colspan="2" class="tdcol" style="height: 22px">
                    &nbsp;Mailing Address
                </td>
            </tr>
            <tr class="trheight">
                <td class="td1" style="width: 133px">
                    Address<font color="red">*</font>     
                </td>
                <td class="td2">            
                    <dx:ASPxTextBox ID="txtAddress1" runat="server" Width="200px" MaxLength="100">
                    
                    <ValidationSettings SetFocusOnError="true">
                        <RequiredField ErrorText="Address1 is required" IsRequired="True" />                       
                        <RegularExpression ErrorText="Address1 is invalid" ValidationExpression="^([1-zA-Z0-1@,-/().\s]{1,150})$" /> 
                                                                           
<RequiredField IsRequired="True" ErrorText="Address1 is required"></RequiredField>

<RegularExpression ErrorText="Address is invalid" ValidationExpression="^([1-zA-Z0-1@,-/().\s]{1,150})$"></RegularExpression>
                                                                           
                   </ValidationSettings>
                </dx:ASPxTextBox>
         
                </td>
            </tr>
             <tr class="trheight">
                <td class="td1" style="width: 133px">
                  
                </td>
                <td class="td2">                    
                    <dx:ASPxTextBox ID="txtAddress2" runat="server" Width="200px" MaxLength="100">
                    
                    <ValidationSettings SetFocusOnError="true">
                      
                        <RegularExpression ErrorText="Address2 is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,150})$" /> 
                                                                           
<RegularExpression ErrorText="Address2 is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,150})$"></RegularExpression>
                                                                           
                   </ValidationSettings>
                </dx:ASPxTextBox>
                    
                </td>
            </tr>
             <tr class="trheight">
                <td class="td1" style="width: 133px">
                  
                </td>
                <td class="td2">                    
                    <dx:ASPxTextBox ID="txtAddress3" runat="server" Width="200px" MaxLength="100">
                    
                    <ValidationSettings SetFocusOnError="true">
                      
                        <RegularExpression ErrorText="Address3 is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,150})$" /> 
                                                                           
<RegularExpression ErrorText="Address3 is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,150})$"></RegularExpression>
                                                                           
                   </ValidationSettings>
                </dx:ASPxTextBox>
                    
                </td>
            </tr>
              <tr class="trheight">
                    <td class="td1" style="width: 133px">
                        Country<font color="red">*</font>  
                    </td>
                    <td class="td2"><span style="float:left; width: 210px;">
                      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate> 
                                <dx:ASPxComboBox ID="cmbCountry" runat="server" AutoPostBack="true" IncrementalFilteringMode="StartsWith"
                                    class="Select" OnSelectedIndexChanged="ddlCountry_OnSelectedIndexChanged" 
                                    Width="280px">
                                    <ValidationSettings>
                                        <RequiredField ErrorText="Country is required" IsRequired="True" />
                                    </ValidationSettings>
                                    <ClientSideEvents ValueChanged="function(s, e) {                                                                                                                                 
                                                                // Prevent multiple presses of the button
                                                                    LoadingPanel.SetText(&#39;Loading Data from server...&#39;);
                                                                    LoadingPanel.Show();                                                                    
                                                                    setTimeout(function() {LoadingPanel.Hide()},2000);                                                                                                    
                                                             }"></ClientSideEvents>
                                </dx:ASPxComboBox>
                                </ContentTemplate>
                        </asp:UpdatePanel> 
                            </span>       
                    </td>
            </tr>
             <tr class="trheight">
                <td class="td1" style="width: 133px">
                    State
                </td>
                <td class="td2">                    
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>   
                       
                       <dx:ASPxComboBox ID="cmbState" runat="server" class="Select" AutoPostBack="True" IncrementalFilteringMode="StartsWith">

                        </dx:ASPxComboBox>    
                         </ContentTemplate> 
                     </asp:UpdatePanel>  
                </td>
            </tr>
            <tr class="trheight">
                <td class="td1" style="width: 133px">
                    City<font color="red">*</font> 
                </td>
                <td class="td2">         <span style="float:left; width: 200px;">           
                    <dx:ASPxTextBox ID="txtCity" runat="server" Width="200px">
                    <ValidationSettings>
                        <RequiredField ErrorText="City is required" IsRequired="True" />     
                        <RegularExpression ErrorText="City is required" ValidationExpression="^([1-zA-Z0-1@.\s]{1,20})$" />
<RequiredField IsRequired="True" ErrorText="City is required"></RequiredField>

<RegularExpression ErrorText="Invalid City" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$"></RegularExpression>
                   </ValidationSettings>
              
                </dx:ASPxTextBox>    </span>        
                </td>
            </tr>
           
            <tr class="trheight">
                <td class="td1" style="width: 133px">
                    Postcode
                </td>
                <td class="td2">                    
                    <dx:ASPxTextBox ID="txtPostcode" runat="server" Width="200px"  >
                    <ValidationSettings >
                     <RegularExpression ErrorText="Invalid Postcode" ValidationExpression="^([1-zA-Z0-1@.\s]{1,20})$" />                                                    
                        
                        
<RegularExpression ErrorText="Invalid Postcode" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$"></RegularExpression>
                        
                        
                   </ValidationSettings>
                </dx:ASPxTextBox>
                   
                </td>
            </tr>

            <tr class="trheight" style="background-color:#333333;color:White;">
                <td colspan="2" class="tdcol">
                    &nbsp;Contact Details
                </td>
            </tr>
            <tr class="trheight">
                <td class="td1" style="width: 133px">
                    Title<font color="red">*</font> 
                </td>
                <td class="td2">                   
                    <dx:ASPxComboBox ID="CmbTitle" runat="server" class="Select" AutoPostBack="false" IncrementalFilteringMode="StartsWith">

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
            </tr>
 <tr class="trheight">
                <td class="td1" style="height: 25px; width: 133px;">
                    Travel Agent Licence No
                </td>
                <td class="td2" style="height: 25px">
                <dx:ASPxTextBox ID="txtAgentNo" runat="server" Width="200px">
                    <ValidationSettings SetFocusOnError="true">                     
                        <RegularExpression ErrorText="Licence Number is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$" /> 
                                                                           
<RegularExpression ErrorText="Licence Number is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$"></RegularExpression>
                                                                           
                   </ValidationSettings></dx:ASPxTextBox>
                         
                    
                    </td>
            </tr>
            <tr class="trheight">
                <td class="td1" style="width: 133px">
                    Contact Person<font color="red">*</font>  
                </td>
                <td class="td2">       <span style="float:left; width: 200px;">             
                    <dx:ASPxTextBox ID="txt_FirstName" runat="server" Width="200px" 
                        NullText="First Name" MaxLength="50">
                    <ValidationSettings>
                        <RequiredField ErrorText="First Name is required" IsRequired="True" />
                        <RegularExpression ErrorText="Invalid First Name" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$" />


<RequiredField IsRequired="True" ErrorText="First Name is required"></RequiredField>

<RegularExpression ErrorText="Invalid First Name" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$"></RegularExpression>
                   </ValidationSettings>
                </dx:ASPxTextBox>    </span>       
                    
                </td>
            </tr>

            <tr class="trheight">
                <td class="td1" style="width: 133px">
                    &nbsp;</td>
                <td class="td2"><span style="float:left; width: 200px;">
                        <dx:ASPxTextBox ID="txt_LastName" runat="server" Width="200px" 
                        NullText="Last Name" MaxLength="50">
                    <ValidationSettings>
                        <RequiredField ErrorText="Last Name is required" IsRequired="True" />
                        <RegularExpression ErrorText="Invalid Last Name" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$" />

<RequiredField IsRequired="True" ErrorText="Last Name is required"></RequiredField>

<RegularExpression ErrorText="Invalid Last Name" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$"></RegularExpression>

                   </ValidationSettings>
                </dx:ASPxTextBox>    </span>     
                    
                </td>
            </tr>
            <tr class="trheight">
                <td class="td1" style="width: 133px">
                    Tel<font color="red">*</font>  
                </td>
                <td class="td2">  <span style="float:left; width: 200px;">                  
                    <dx:ASPxTextBox ID="txt_tel" runat="server" Width="200px" MaxLength="20">
                    <ValidationSettings>
                        <RequiredField ErrorText="Telephone number is required" IsRequired="True" />
                        <RegularExpression ErrorText="Invalid Telephone number" ValidationExpression="^([\d*]{1,20})$" />

<RequiredField IsRequired="True" ErrorText="Telephone number is required"></RequiredField>

<RegularExpression ErrorText="Invalid Telephone number" ValidationExpression="^([\d*]{1,20})$"></RegularExpression>

                   </ValidationSettings>
                </dx:ASPxTextBox>    </span>       
                    
                </td>
            </tr>
            <tr class="trheight">
                <td class="td1" style="width: 133px">
                    Mobile Phone <font color="red">*</font>  
                </td>
                <td class="td2">  <span style="float:left; width: 200px;">                  
                    <dx:ASPxTextBox ID="txt_MobilePhone" runat="server" Width="200px" MaxLength="50">
                    <ValidationSettings>                        
                        <RequiredField IsRequired="True" ErrorText="Mobile Number is required"></RequiredField>
                        <RegularExpression ErrorText="Invalid Mobile Number" ValidationExpression="^([1-zA-Z0-1@.\s]{1,20})$" />
                        <RegularExpression ErrorText="Invalid Mobile Number" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$"></RegularExpression>                        
                   </ValidationSettings>
                </dx:ASPxTextBox>    </span>        
                    
                </td>
            </tr>
            <tr class="trheight">
                <td class="td1" style="width: 133px">
                    Email<font color="red">*</font> 
                </td>
                <td class="td2" > <span style="float:left; width: 200px;">                   
                    <dx:ASPxTextBox ID="txt_Email" runat="server" Width="200px" MaxLength="100">
                    <ValidationSettings>
                        <RequiredField ErrorText="E-mail is required" IsRequired="True" />
                        



<RegularExpression ErrorText="Invalid e-mail" ValidationExpression="^(?=.{1,266}$)\w+([-.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />

<RequiredField IsRequired="True" ErrorText="E-mail is required"></RequiredField>

<RegularExpression ErrorText="Invalid e-mail" ValidationExpression="^(?=.{1,266}$)\w+([-.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></RegularExpression>

                   </ValidationSettings>
                </dx:ASPxTextBox>      </span>                                                              
                </td>

            </tr>
            <tr class="trheight">
                <td class="td1" style="width: 133px">
                    Fax
                </td>
                <td class="td2">                    
                    <dx:ASPxTextBox ID="txt_Fax" runat="server" Width="200px" MaxLength="20">
                    <ValidationSettings>
                        <RegularExpression ErrorText="Invalid Fax" ValidationExpression="^([1-zA-Z0-1@.\s]{1,20})$" />
<RegularExpression ErrorText="Invalid Fax" ValidationExpression="^([1-zA-Z0-1@.\s]{1,20})$"></RegularExpression>
                   </ValidationSettings>
                </dx:ASPxTextBox>
                </td>
            </tr>
          
            
              
           
            <tr class="trheight" style="background-color:#333333;color:White;">
                <td colspan="2" class="tdcol">
                    &nbsp;Bank Details
                </td>
            </tr>
              <tr class="trheight">
                <td class="td1" style="width: 133px">
                    Bank Name
                </td>
                <td class="td2"> <span style="float:left; width: 200px;">                   
                    <dx:ASPxTextBox ID="txtBankName" runat="server" Width="200px" MaxLength="200">
                    
                    <ValidationSettings SetFocusOnError="true">
                        <RegularExpression ErrorText="Bank name is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,200})$" />                                                                            
                        <RegularExpression ErrorText="Bank name is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,200})$"></RegularExpression>
                                                                           
                   </ValidationSettings>
                </dx:ASPxTextBox>    </span>        
                    
                </td>
            </tr>
            <tr class="trheight">
                <td class="td1" style="width: 133px">
                    Address
                </td>
                <td class="td2">                    
                    <dx:ASPxTextBox ID="txtBankAddress1" runat="server" Width="200px" MaxLength="150">
                    
                    <ValidationSettings SetFocusOnError="true">                               
                        <RegularExpression ErrorText="Bank Address1 is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,150})$" /> 
                                                                           
<RegularExpression ErrorText="Bank Address1 is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,150})$"></RegularExpression>
                                                                           
                   </ValidationSettings>
                </dx:ASPxTextBox>
                    
                </td>
            </tr>
             <tr class="trheight">
                <td class="td1" style="width: 133px">
                  
                </td>
                <td class="td2">                                    
                    <dx:ASPxTextBox ID="txtBankAddress2" runat="server" Width="200px" MaxLength="150">
                     <ValidationSettings SetFocusOnError="true">                                
                        <RegularExpression ErrorText="Bank Address2 is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,150})$" />                                                                            
                        <RegularExpression ErrorText="Bank Address2 is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,150})$"></RegularExpression>
                   </ValidationSettings>
                </dx:ASPxTextBox>
                    
                </td>
            </tr>            
            <tr class="trheight">
                <td class="td1" style="width: 133px">
                  
                </td>
                <td class="td2">                                    
                   
                    
                  
                    <dx:ASPxTextBox ID="txtBankAddress3" runat="server" MaxLength="150" 
                        Width="200px">
                        <validationsettings setfocusonerror="True">
                            <RegularExpression errortext="Bank Address3 is invalid" 
                                validationexpression="^([1-zA-Z0-1@.\s]{1,150})$" />
                            <RegularExpression ErrorText="Bank Address3 is invalid" ValidationExpression="^([1-zA-Z0-1@.\s]{1,150})$"></RegularExpression>
                        </validationsettings>
                    </dx:ASPxTextBox>
                    </font>                                    
                   
                    
                </td>
            </tr>            
                 <tr class="trheight">
                    <td class="td1" style="width: 133px">
                        Country 
                    </td>
                    <td class="td2"><span style="float:left; width: 210px;">
              <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <dx:ASPxComboBox ID="cmbCountryBank" runat="server" AutoPostBack="True" 
                                    class="Select" IncrementalFilteringMode="StartsWith"
                                    OnSelectedIndexChanged="ddlCountryBank_OnSelectedIndexChanged" Width="280px">
                                   
                                      <ClientSideEvents ValueChanged="function(s, e) {                                                                                                                                 
                                                                // Prevent multiple presses of the button
                                                                    LoadingPanel.SetText(&#39;Loading Data from server...&#39;);
                                                                    LoadingPanel.Show();                                                                    
                                                                    setTimeout(function() {LoadingPanel.Hide()},2000);                                                                                                    
                                                             }"></ClientSideEvents>
                                </dx:ASPxComboBox>                            
                            </ContentTemplate>                            
                        </asp:UpdatePanel>           </span>       

                    
                    </td>
            </tr>
              <tr class="trheight">
                <td class="td1" style="width: 133px">
                    State
                </td>
                <td class="td2">                    
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <dx:ASPxComboBox ID="cmbStateBank" runat="server" class="Select" AutoPostBack="True" IncrementalFilteringMode="StartsWith">
                        
                        </dx:ASPxComboBox>    
                             </ContentTemplate>                            
                        </asp:UpdatePanel>          
                </td>
            </tr>
            <tr class="trheight">
                <td class="td1" style="width: 133px">
                    City
                </td>
                <td class="td2">                    
                    <dx:ASPxTextBox ID="txtCityBank" runat="server" Width="200px" MaxLength="50">
                    <ValidationSettings>
                        <RegularExpression ErrorText="Invalid City" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$" />
<RegularExpression ErrorText="Invalid City" ValidationExpression="^([1-zA-Z0-1@.\s]{1,50})$"></RegularExpression>
                   </ValidationSettings>
              
                </dx:ASPxTextBox>
                </td>
            </tr>
          
            <tr class="trheight">
                <td class="td1" style="width: 133px">
                    Postcode
                </td>
                <td class="td2">                    
                    <dx:ASPxTextBox ID="txtPostcodeBank" runat="server" Width="200px" MaxLength="20">
                    <ValidationSettings>                                        
                     <RegularExpression ErrorText="Invalid Postcode" ValidationExpression="^([1-zA-Z0-1@.\s]{1,20})$" />                                                    
                        
                        
<RegularExpression ErrorText="Invalid Postcode" ValidationExpression="^([1-zA-Z0-1@.\s]{1,20})$"></RegularExpression>
                        
                        
                   </ValidationSettings>
                </dx:ASPxTextBox>
                    
                </td>
            </tr>
            <tr class="trheight">
                <td class="td1" style="width: 133px">
                    Account Name
                </td>
                <td class="td2">    <span style="float:left; width: 200px;">                 
                    <dx:ASPxTextBox ID="txtBankAccountName" runat="server" Width="200px" MaxLength="100">                     
                    <ValidationSettings >
                     <RegularExpression ErrorText="Invalid Account Name" ValidationExpression="^([1-zA-Z0-1@.\s]{1,200})$" />                                                                                                    
                     <RegularExpression ErrorText="Invalid Account Name" ValidationExpression="^([1-zA-Z0-1@.\s]{1,200})$"></RegularExpression>
                        
                   </ValidationSettings>
                </dx:ASPxTextBox>    </span>         
                   
                </td>
            </tr>
            <tr class="trheight">
                <td class="td1" style="width: 133px">
                    Bank Account No
                </td>
                <td class="td2">      <span style="float:left; width: 200px;">               
                    <dx:ASPxTextBox ID="txtBankAccountno" runat="server" Width="200px" MaxLength="50">
              
                    <ValidationSettings>
                     <RegularExpression ErrorText="Invalid Bank Account No" ValidationExpression="^([1-zA-Z0-1@\-\.\s]{1,50})$" />                                                                                                                         
                     <RegularExpression ErrorText="Invalid Bank Account No" ValidationExpression="^([1-zA-Z0-1@\-\.\s]{1,50})$"></RegularExpression>
                        
                   </ValidationSettings>
                </dx:ASPxTextBox>    </span>       
                   
                </td>            
            </tr>
            </table>
            <br />
            <table>
            <tr class="trheight">
                <td colspan="2" class="style1">
                    <center>
                     <font color="red">
                        <asp:Label ID="lblMsg" runat="server" Text="Label" Visible="false"></asp:Label></font>
                     </center>
                </td>
            </tr>
            <tr>
            <td></td>
            </tr>
            <tr class="trheight">
                <td class="td1" style="width: 189px">
                    &nbsp;
                </td>
                <td class="td2">
                    <dx:ASPxButton CssClass="button_2"  ID="btnSubmit" runat="server" Text="Submit"
                        OnClick="btnSubmit_Click" Height="19px" Width="88px"  >
                         <ClientSideEvents Click="function(s, e) { 
                                                                if(ASPxClientEdit.AreEditorsValid()) {
                                                                
                                                                // Prevent multiple presses of the button
                                                                    LoadingPanel.SetText('Sending data to the server...');
                                                                    LoadingPanel.Show();
                                                                }
                                                             }" />
                            <ClientSideEvents Click="function(s, e) { 
                                                                if(ASPxClientEdit.AreEditorsValid()) {
                                                                
                                                                // Prevent multiple presses of the button
                                                                    LoadingPanel.SetText(&#39;Sending data to the server...&#39;);
                                                                    LoadingPanel.Show();
                                                                }
                                                             }"></ClientSideEvents>
                     </dx:ASPxButton>
                   
                     </td>
                     <td>
                     <dx:ASPxButton CssClass="button_2"  ID="BtnBack" runat="server" Height="19px"
                            OnClick="BtnBack_Click" Text="Back" Width="88px" CausesValidation="False">
                            <ClientSideEvents Click="function(s, e) { 
                                                                if(ASPxClientEdit.AreEditorsValid()) {
                                                                
                                                                // Prevent multiple presses of the button
                                                                    LoadingPanel.SetText('Loading Data...');
                                                                    LoadingPanel.Show();
                                                                }
                                                             }" />
                                                                <ClientSideEvents Click="function(s, e) { 
                                                                if(ASPxClientEdit.AreEditorsValid()) {
                                                                
                                                                // Prevent multiple presses of the button
                                                                    LoadingPanel.SetText(&#39;Loading Data...&#39;);
                                                                    LoadingPanel.Show();
                                                                }
                                                             }"></ClientSideEvents>
                        </dx:ASPxButton>
                     </td>
                
            </tr>
        </table>
 
 </dx:PanelContent>
            </PanelCollection>
        </dx:aspxpanel>

</asp:Content>

