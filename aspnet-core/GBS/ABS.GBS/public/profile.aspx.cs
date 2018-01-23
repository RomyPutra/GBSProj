using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Web;
using System.Text.RegularExpressions;
using System.Threading;
using ABS.Logic.GroupBooking.Agent;
using ABS.Logic.GroupBooking;
using ABS.Logic.Shared;
using ABS.GBS.Log;

namespace GroupBooking.Web
{
    public partial class Register : System.Web.UI.Page
    {
        UserSet AgentSet;
        AgentCategory AgentCat;
        //AgentControl.StrucAgentSet AgentSet;
        ABS.Logic.GroupBooking.Agent.AgentProfileControl objAgentProfile = new ABS.Logic.GroupBooking.Agent.AgentProfileControl();

        AgentBankInfo AG_BANK_INFO = new AgentBankInfo();
        AgentProfile AG_PROFILE_INFO = new AgentProfile();
        AgentProfileControl AgentProfile = new AgentProfileControl();
        AgentProfile newAgentProfile = new AgentProfile();
        GeneralControl GeneralControls = new GeneralControl();

        SystemLog SystemLog = new SystemLog();
        string passSave = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadAgentSetting();
            if (!IsPostBack)
            {
                InitForm();
                BindState();
                BindStateBank();
                //test
                //EditProfileMode();
                if (Session["AgentSet"] != null)
                    if (Request.QueryString["optmode"] == "2")
                        EditProfileMode();
            }
            if (Request.QueryString["optmode"] == "3")
                EditProfileMode();

            if (IsCallback)
            {
                Thread.Sleep(500);
            }
        }


        protected void LoadAgentSetting()
        {
            if (Session["AgentSet"] != null)
            {
                AgentSet = (UserSet)Session["AgentSet"];
            }
        }

        protected void EditProfileMode()
        {
            //set view
            if (Request.QueryString["optmode"] == "2")
            {
				lblPassInfo.Visible = false;
                lblInfo.Text = "Please update your infomation as below, your details will be sent to our service to change";
                tblTop.Visible = false;
                headPassword.Visible = false;
                lblHeader.Text = "Update Profile";
                txt_AgentName.Enabled = false;
                //txtAgentNo.Enabled = false;
                txt_AgentPWD.Enabled = false;
                txt_AgentPWD2.Enabled = false;
                txtAddress1.Enabled = true;
                txtAddress2.Enabled = true;
                txtAddress3.Enabled = true;
                cmbCountry.Enabled = true;
                cmbState.Enabled = true;
                txtPostcode.Enabled = true;
                CmbTitle.Enabled = false;
                txt_FirstName.Enabled = true;
                txt_LastName.Enabled = true;
                txt_tel.Enabled = true;
                txt_MobilePhone.Enabled = true;
                txt_Email.Enabled = true;
                txt_Fax.Enabled = true;
                txtBankName.Enabled = true;
                txtBankAddress1.Enabled = true;
                txtBankAddress2.Enabled = true;
                txtBankAddress3.Enabled = true;
                cmbCountryBank.Enabled = true;
                cmbStateBank.Enabled = true;
                txtCityBank.Enabled = true;
                txtPostcodeBank.Enabled = true;
                txtBankAccountName.Enabled = true;
                txtBankAccountno.Enabled = true;

                //load data
                AG_PROFILE_INFO = objAgentProfile.GetSingleAgentProfileByID(AgentSet.AgentID);
                AG_BANK_INFO = objAgentProfile.GetAgentBankInfo(AgentSet.AgentID);
                //txt_AgentName.ValidationSettings.RegularExpression= ""
                txt_AgentName.Text = AG_PROFILE_INFO.Username;
                txtAgentNo.Text = AG_PROFILE_INFO.LicenseNo;
                txt_AgentPWD.Text = AG_PROFILE_INFO.Password;
                txt_AgentPWD2.Text = AG_PROFILE_INFO.Password;
                txt_AgentPWD.Text = AG_PROFILE_INFO.Password;
                txtAddress1.Text = AG_PROFILE_INFO.Address1;
                txtAddress2.Text = AG_PROFILE_INFO.Address2;
                txtAddress3.Text = AG_PROFILE_INFO.Address3;
                LoadCountryAndState(AgentSet.AgentID);
                txtPostcode.Text = AG_PROFILE_INFO.Postcode;
                txtCity.Text = AG_PROFILE_INFO.City;
                LoadTitle(AgentSet.AgentID);
                txt_FirstName.Text = AG_PROFILE_INFO.ContactFirstName;
                txt_LastName.Text = AG_PROFILE_INFO.ContactLastName;
                txt_tel.Text = AG_PROFILE_INFO.PhoneNo;
                txt_MobilePhone.Text = AG_PROFILE_INFO.MobileNo;
                txt_Email.Text = AG_PROFILE_INFO.Email;
                txt_Fax.Text = AG_PROFILE_INFO.Fax;

                if (AG_BANK_INFO != null)
                {
                    txtBankName.Text = AG_BANK_INFO.BankName;
                    txtBankAddress1.Text = AG_BANK_INFO.Address1;
                    txtBankAddress2.Text = AG_BANK_INFO.Address2;
                    txtBankAddress3.Text = AG_BANK_INFO.Address3;
                    LoadBankCountryAndState(AgentSet.AgentID);

                    txtCityBank.Text = AG_BANK_INFO.City;
                    txtPostcodeBank.Text = AG_BANK_INFO.Postcode;
                    txtBankAccountName.Text = AG_BANK_INFO.AccountName;
                    txtBankAccountno.Text = AG_BANK_INFO.AccountNo;
                }
            }
            if (Request.QueryString["optmode"] == "3")
            {
                tblBottom.Style["display"] = "none";
                lblInfo.Text = "Please insert old password and new password, your details will be sent to our service to change";
                lblHeader.Text = "Change Password";
                trLogin1.Visible = false;
                trLogin2.Visible = false;
                trPass1.Visible = true;
                trPass2.Visible = true;
                trPass3.Visible = true;
                headPassword.Visible = true;
                //load data
                AG_PROFILE_INFO = objAgentProfile.GetSingleAgentProfileByID(AgentSet.AgentID);
                AG_BANK_INFO = objAgentProfile.GetAgentBankInfo(AgentSet.AgentID);
                passSave = AG_PROFILE_INFO.Password;
                txt_AgentName.Text = AG_PROFILE_INFO.Username;
                txtAgentNo.Text = AG_PROFILE_INFO.LicenseNo;
                txtAddress1.Text = AG_PROFILE_INFO.Address1;
                txtAddress2.Text = AG_PROFILE_INFO.Address2;
                txtAddress3.Text = AG_PROFILE_INFO.Address3;
                LoadCountryAndState(AgentSet.AgentID);
                txtPostcode.Text = AG_PROFILE_INFO.Postcode;
                txtCity.Text = AG_PROFILE_INFO.City;
                LoadTitle(AgentSet.AgentID);
                txt_FirstName.Text = AG_PROFILE_INFO.ContactFirstName;
                txt_LastName.Text = AG_PROFILE_INFO.ContactLastName;
                txt_tel.Text = AG_PROFILE_INFO.PhoneNo;
                txt_MobilePhone.Text = AG_PROFILE_INFO.MobileNo;
                txt_Email.Text = AG_PROFILE_INFO.Email;
                txt_Fax.Text = AG_PROFILE_INFO.Fax;

                if (AG_BANK_INFO != null)
                {
                    txtBankName.Text = AG_BANK_INFO.BankName;
                    txtBankAddress1.Text = AG_BANK_INFO.Address1;
                    txtBankAddress2.Text = AG_BANK_INFO.Address2;
                    txtBankAddress3.Text = AG_BANK_INFO.Address3;
                    LoadBankCountryAndState(AgentSet.AgentID);

                    txtCityBank.Text = AG_BANK_INFO.City;
                    txtPostcodeBank.Text = AG_BANK_INFO.Postcode;
                    txtBankAccountName.Text = AG_BANK_INFO.AccountName;
                    txtBankAccountno.Text = AG_BANK_INFO.AccountNo;
                }
            }

        }

        protected void LoadTitle(string AgentID)
        {
            string title = objAgentProfile.GetTitle(AgentID);
            if (title != "")
            {
                CmbTitle.Items.FindByValue(title).Selected = true;
            }
        }

        protected void LoadCountryAndState(string AgentID)
        {
            DataTable dt = objAgentProfile.GetCountryAndState(AgentID);
            if (dt.Rows.Count > 0)
            {
                cmbCountry.Items.FindByValue(dt.Rows[0]["Country"]).Selected = true;
                BindState();
                if (dt.Rows[0]["State"].ToString() != string.Empty)
                {
                    cmbState.Items.FindByValue(dt.Rows[0]["State"]).Selected = true;
                }
            }
        }

        protected void LoadBankCountryAndState(string AgentID)
        {
            DataTable dt = objAgentProfile.GetBankCountryAndState(AgentID);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Country"].ToString() != string.Empty)
                {
                    cmbCountryBank.Items.FindByValue(dt.Rows[0]["Country"]).Selected = true;
                    BindStateBank();
                    if (dt.Rows[0]["State"].ToString() != string.Empty)
                    {
                        cmbStateBank.Items.FindByValue(dt.Rows[0]["State"]).Selected = true;
                    }
                }


            }
        }

        private void InitForm()
        {
            UIClass.SetComboStyle(ref cmbCountry, UIClass.EnumDefineStyle.Country);
            UIClass.SetComboStyle(ref cmbCountryBank, UIClass.EnumDefineStyle.Country);
        }

        public void BindState()
        {
            try
            {
                List<Country_Info> lstState = new List<Country_Info>();
                lstState = GeneralControls.GetAllState(cmbCountry.SelectedItem.Value.ToString());
                cmbState.Items.Clear();
                cmbState.TextField = "provinceStateName";
                cmbState.ValueField = "provincestatecode";
                cmbState.DataSource = lstState;
                cmbState.DataBind();
                if (cmbState.Items.Count > 0)
                {
                    cmbState.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);

            }
            finally
            {

            }
        }

        public void BindStateBank()
        {
            try
            {
                List<Country_Info> lstState = new List<Country_Info>();
                lstState = GeneralControls.GetAllState(cmbCountryBank.SelectedItem.Value.ToString());

                cmbStateBank.Items.Clear();
                cmbStateBank.TextField = "provinceStateName";
                cmbStateBank.ValueField = "provincestatecode";
                cmbStateBank.DataSource = lstState;
                cmbStateBank.DataBind();
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);

            }
            finally
            {

            }
        }
        private void AssignValue()
        {
            string temp = "";

            temp = DateTime.Now.ToString("yyyyMMddHHmmsss");
            if (Session["AgentSet"] != null)
            {
                AG_PROFILE_INFO.AgentID = AgentSet.AgentID;
                AG_BANK_INFO.AgentID = AgentSet.AgentID;
            }
            else if (Session["AgentSet"] == null) // insert
            {
                AG_PROFILE_INFO.AgentID = temp;
                AG_BANK_INFO.AgentID = temp;
            }

            switch (Request.QueryString["optmode"])
            {
                case "1":
                    AG_BANK_INFO.AccountName = txtBankAccountName.Text;
                    AG_BANK_INFO.AccountNo = txtBankAccountno.Text;
                    AG_BANK_INFO.Address1 = txtBankAddress1.Text;
                    AG_BANK_INFO.Address2 = txtBankAddress2.Text;
                    AG_BANK_INFO.Address3 = txtBankAddress3.Text;
                    AG_BANK_INFO.BankName = txtBankName.Text;
                    AG_BANK_INFO.City = txtCityBank.Text;
                    if (cmbCountryBank.SelectedIndex != -1)
                        AG_BANK_INFO.Country = cmbCountryBank.SelectedItem.Value.ToString();
                    AG_BANK_INFO.Postcode = txtPostcodeBank.Text;
                    if (cmbStateBank.SelectedIndex != -1)
                        AG_BANK_INFO.State = cmbStateBank.SelectedItem.Value.ToString();
                    AG_BANK_INFO.LastSyncBy = "System";
                    AG_PROFILE_INFO.Address1 = txtAddress1.Text;
                    AG_PROFILE_INFO.Address2 = txtAddress2.Text;
                    AG_PROFILE_INFO.Address3 = txtAddress3.Text;
                    AG_PROFILE_INFO.AgentCatgID = "1";
                    AG_PROFILE_INFO.Password = txt_AgentPWD.Text;
                    AG_PROFILE_INFO.City = txtCity.Text;
                    AG_PROFILE_INFO.ContactFirstName = txt_FirstName.Text;
                    AG_PROFILE_INFO.ContactLastName = txt_LastName.Text;
                    AG_PROFILE_INFO.Country = cmbCountry.SelectedItem.Value.ToString();
                    AG_PROFILE_INFO.Email = txt_Email.Text;
                    AG_PROFILE_INFO.Fax = txt_Fax.Text;
                    AG_PROFILE_INFO.Flag = 0;
                    AG_PROFILE_INFO.LastSyncBy = "System";
                    AG_PROFILE_INFO.LicenseNo = txtAgentNo.Text;
                    AG_PROFILE_INFO.MobileNo = txt_MobilePhone.Text;
                    AG_PROFILE_INFO.PhoneNo = txt_tel.Text;
                    AG_PROFILE_INFO.Postcode = txtPostcode.Text;
                    if (cmbState.SelectedIndex != -1)
                        AG_PROFILE_INFO.State = cmbState.SelectedItem.Value.ToString();
                    AG_PROFILE_INFO.Status = 1;
                    AG_PROFILE_INFO.Title = CmbTitle.SelectedItem.Value.ToString();
                    AG_PROFILE_INFO.Username = txt_AgentName.Text;
                    break;
                case "2":
                    AG_PROFILE_INFO = objAgentProfile.GetSingleAgentProfileByID(AgentSet.AgentID);
                    AG_BANK_INFO.AccountName = txtBankAccountName.Text;
                    AG_BANK_INFO.AccountNo = txtBankAccountno.Text;
                    AG_BANK_INFO.Address1 = txtBankAddress1.Text;
                    AG_BANK_INFO.Address2 = txtBankAddress2.Text;
                    AG_BANK_INFO.Address3 = txtBankAddress3.Text;
                    AG_BANK_INFO.BankName = txtBankName.Text;
                    AG_BANK_INFO.City = txtCityBank.Text;
                    if (cmbCountryBank.SelectedIndex != -1)
                        AG_BANK_INFO.Country = cmbCountryBank.SelectedItem.Value.ToString();
                    AG_BANK_INFO.Postcode = txtPostcodeBank.Text;
                    if (cmbStateBank.SelectedIndex != -1)
                        AG_BANK_INFO.State = cmbStateBank.SelectedItem.Value.ToString();
                    AG_BANK_INFO.LastSyncBy = "System";
                    AG_PROFILE_INFO.Address1 = txtAddress1.Text;
                    AG_PROFILE_INFO.Address2 = txtAddress2.Text;
                    AG_PROFILE_INFO.Address3 = txtAddress3.Text;
                    AG_PROFILE_INFO.AgentCatgID = "1";
                    AG_PROFILE_INFO.City = txtCity.Text;
                    AG_PROFILE_INFO.ContactFirstName = txt_FirstName.Text;
                    AG_PROFILE_INFO.ContactLastName = txt_LastName.Text;
                    AG_PROFILE_INFO.Country = cmbCountry.SelectedItem.Value.ToString();
                    AG_PROFILE_INFO.Email = txt_Email.Text;
                    AG_PROFILE_INFO.Fax = txt_Fax.Text;
                    AG_PROFILE_INFO.Flag = 0;
                    AG_PROFILE_INFO.LastSyncBy = "System";
                    AG_PROFILE_INFO.LicenseNo = txtAgentNo.Text;
                    AG_PROFILE_INFO.MobileNo = txt_MobilePhone.Text;
                    AG_PROFILE_INFO.PhoneNo = txt_tel.Text;
                    AG_PROFILE_INFO.Postcode = txtPostcode.Text;
                    if (cmbState.SelectedIndex != -1)
                        AG_PROFILE_INFO.State = cmbState.SelectedItem.Value.ToString();
                    AG_PROFILE_INFO.Status = 1;
                    AG_PROFILE_INFO.Title = CmbTitle.SelectedItem.Value.ToString();
                    AG_PROFILE_INFO.Username = txt_AgentName.Text;
                    break;
                case "3":
                    AG_PROFILE_INFO.Password = txt_AgentPWD.Text;
                    break;
                default:
                    break;
            }

        }
        private void SaveData()
        {
            //assign value
            AgentProfile = new AgentProfileControl();
            ABS.Logic.GroupBooking.Agent.AgentControl objAgent = new ABS.Logic.GroupBooking.Agent.AgentControl();
            if (Session["AgentSet"] != null)
            {
                newAgentProfile = AgentProfile.SaveAgentProfile(AG_PROFILE_INFO, AG_BANK_INFO, ABS.Logic.Shared.CoreBase.EnumSaveType.Update);
            }
            else
            {
                newAgentProfile = AgentProfile.SaveAgentProfile(AG_PROFILE_INFO, AG_BANK_INFO, ABS.Logic.Shared.CoreBase.EnumSaveType.Insert);
                if (newAgentProfile != null)
                {
                    AgentSet = new UserSet();
                    AgentSet.AgentID = newAgentProfile.AgentID;
                    AgentSet.AgentCategoryID = newAgentProfile.AgentCatgID;
                    AgentSet.AgentName = newAgentProfile.Username;
                    AgentCat = objAgent.GetAgentCategory(AgentSet.AgentCategoryID);

                    AgentSet.BlacklistDuration = AgentCat.BlacklistDuration;
                    AgentSet.CounterTimer = AgentCat.CounterTimer;
                    AgentSet.MaxEnquiry = AgentCat.MaxEnquiry;
                    AgentSet.MaxSuspend = AgentCat.MaxSuspend;
                    AgentSet.SuspendDuration = AgentCat.SuspendDuration;
                    AgentSet.LoginName = AgentSet.AgentName;
                    Session["AgentSet"] = AgentSet;
                }
            }


        }

        protected void ASPxCallbackPanelDemo_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            ASPxCallbackPanel callbackPanel = (ASPxCallbackPanel)sender;
            bool isValid = ASPxEdit.ValidateEditorsInContainer(callbackPanel);
        }
        protected void ASPxPassword_Validation(object sender, ValidationEventArgs e)
        {
            ASPxTextBox txt = sender as ASPxTextBox;
            bool result;
            if (txt.Text == txt_AgentPWD.Text && txt.Text != string.Empty)
                result = true;
            else
                result = false;

            e.IsValid = result;
            e.ErrorText = "The password is not same";
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int flag1 = 0;
            int flag2 = 0;
            int flag3 = 0;
            int flag4 = 0;
            int flag5 = 0;
            int flag6 = 0;
            if (txtCurrentPass.Text != passSave && Request.QueryString["optmode"] == "3")
            {
                txtCurrentPass.IsValid = false;
                flag3 = 1;
            }
            //if (txtCurrentPass.Text == txt_AgentPWD.Text && Request.QueryString["optmode"] == "3")
            //{
            //    txtCurrentPass.IsValid = false;
            //    flag4 = 1;
            //}
            if (txt_AgentPWD.Text == txt_AgentName.Text && Request.QueryString["optmode"] == "3")
            {
                txt_AgentPWD.IsValid = false;
                flag5 = 1;
            }
            if (txt_AgentPWD.Text == txt_AgentName.Text && Request.QueryString["optmode"] == "1")
            {
                txt_AgentPWD.IsValid = false;
                flag6 = 1;
            }
            if (txt_AgentPWD.Text == string.Empty)
            {
                txt_AgentPWD.IsValid = false;
                txt_AgentPWD2.IsValid = false;
            }
            else if (txt_AgentPWD.Text != txt_AgentPWD2.Text)
            {
                txt_AgentPWD.IsValid = false;
                txt_AgentPWD2.IsValid = false;
            }
            else if (txt_AgentPWD.Text == txt_AgentPWD2.Text)
            {
                txt_AgentPWD2.IsValid = true;
            }
            if (Request.QueryString["optmode"] == "2")
            {
                txt_AgentPWD.IsValid = true;
                txt_AgentPWD2.IsValid = true;
            }
            List<AgentProfile> lstEmail = new List<AgentProfile>();
            lstEmail = AgentProfile.GetAllAg_Email(txt_Email.Text);
            List<AgentProfile> lstUser = new List<AgentProfile>();
            lstUser = AgentProfile.GetAllUsername(txt_AgentName.Text);

            AgentProfile UserData = new AgentProfile();
            if (Session["AgentSet"] != null)
            {
                UserData = objAgentProfile.GetSingleAgentProfile(txt_AgentName.Text);
            }

            if (lstEmail != null && Session["AgentSet"] == null)
            {
                txt_Email.ErrorText = "Email's already exist";
                txt_Email.IsValid = false;
                flag1 = 1;
            }
            if (lstUser != null && Session["AgentSet"] == null)
            {
                txt_AgentName.IsValid = false;
                flag2 = 1;
            }
            if (lstEmail != null && Session["AgentSet"] != null && UserData.Email != txt_Email.Text)
            {
                txt_Email.ErrorText = "Email's already exist";
                txt_Email.IsValid = false;
                flag1 = 1;
            }
            if (lstUser != null && Session["AgentSet"] != null && UserData.Username != txt_AgentName.Text)
            {
                txt_AgentName.IsValid = false;
                flag2 = 1;
            }

            ABS.GBS.eService.ProcessServiceSoapClient eServices = new ABS.GBS.eService.ProcessServiceSoapClient();
            try
            {

                if (ASPxEdit.AreEditorsValid(PanelRegister))
                {
                    AssignValue();
                    SaveData();
                    Thread.Sleep(2000);
                    if (newAgentProfile != null && newAgentProfile.AgentID != string.Empty && Session["AgentSet"] != null && Request.QueryString["optmode"] == "1")
                    {
                        string msg = eServices.GroupBookingEmailing(1, "0", newAgentProfile.Email,"",0, "");
                        if (msg != string.Empty && msg == "Error with EmailBody")
                            lblMsg.Text = "Register Failed, please try again or contact our support line.";
                        else
                            Response.Redirect("../Message.aspx?msgID=100&AGID=" + newAgentProfile.AgentID, false);
                    }
                    else if (Session["AgentSet"] == null && Request.QueryString["optmode"] == "1")
                        Response.Redirect("../Message.aspx?msgID=104", false);
                    else if (Request.QueryString["optmode"] == "2")
                        Response.Redirect("../Message.aspx?msgID=105", false);
                    else if (Request.QueryString["optmode"] == "3")
                        Response.Redirect("../Message.aspx?msgID=106", false);
                }
                else
                {
                    lblMsg.Text = "Register Failed, please check the data again";
                    if (flag1 == 1 && flag2 == 1 && Request.QueryString["optmode"] != "3")
                        lblMsg.Text = "Register Failed, the Username and Email already exist";
                    else
                        if (flag1 == 1 && Request.QueryString["optmode"] == "1")
                            lblMsg.Text = "Register Failed, the Email already exist";
                        else
                            if (flag1 == 1 && Request.QueryString["optmode"] == "2")
                                lblMsg.Text = "Update Profile Failed, the Email already exist";
                            else
                                if (flag2 == 1 && Request.QueryString["optmode"] != "3")
                                    lblMsg.Text = "Register Failed, the Username already exist";
                                else
                                    if (flag3 == 1)
                                        lblMsg.Text = "Change Password Failed, the current password is incorrect";
                                    else
                                        if (flag4 == 1)
                                            lblMsg.Text = "Change Password Failed, the new password is same with the old password";
                                        else
                                            if (flag5 == 1)
                                                lblMsg.Text = "Change Password Failed, the new password should not be same with the agent account";
                                            else
                                                if (flag6 == 1)
                                                    lblMsg.Text = "Register Failed, the password should not be same with the agent account";

                    lblMsg.Visible = true;
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);

            }

        }
        protected void ddlCountry_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            BindState();
            Thread.Sleep(1000);

        }
        protected void ddlCountryBank_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            BindStateBank();
            Thread.Sleep(1000);

        }

        protected void BtnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("../public/agentmain.aspx", false);
        }
    }
}