using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using ABS.Logic.GroupBooking.Agent;
using ABS.Logic.GroupBooking;
using DevExpress.Web;
using System.Threading;
using ABS.GBS.Log;
namespace GroupBooking.Web.Administrator
{
    public partial class AgentDetail : System.Web.UI.Page
    {
        GeneralControl GeneralControls = new GeneralControl();
        AgentControl.StrucAgentSet AgentSet;
        AgentProfile newAgentProfile = new AgentProfile();
        ABS.Logic.GroupBooking.Agent.AgentControl objAgent = new ABS.Logic.GroupBooking.Agent.AgentControl();
        ABS.Logic.GroupBooking.Agent.AgentProfileControl objAgentProfile = new ABS.Logic.GroupBooking.Agent.AgentProfileControl();
        AgentBankInfo AG_BANK_INFO = new AgentBankInfo();
        ABS.Logic.GroupBooking.Agent.AgentProfileControl AG_BANK_BLL = new ABS.Logic.GroupBooking.Agent.AgentProfileControl();
        AgentProfile AG_PROFILE_INFO = new AgentProfile();
        ABS.Logic.GroupBooking.Agent.AgentProfileControl AG_PROFILE_BLL = new ABS.Logic.GroupBooking.Agent.AgentProfileControl();
        AgentProfile AgentData = new AgentProfile();
        SystemLog SystemLog = new SystemLog();

        //GroupBooking.Logic.CountryBase CountryBase = new GroupBooking.Logic.CountryBase();
        //GroupBooking.Info.Country_Info CountryInfo = new GroupBooking.Info.Country_Info();
        string optmode = "";
        string AgID = "";
        DataTable dt;
        AdminControl.StrucAdminSet AdminSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminSet"] != null)
            {                
                if (Request.QueryString["optmode"] != null && Session["agid"] != null)
                {

                    // AgID = Session["agid"].ToString();
                    AgID = Session["agid"].ToString();
                }
                optmode = Request.QueryString["optmode"];
                //Load Menu
                //comm.LoadMenu(this);
                ApplyValidationSummarySettings();
                ApplyEditorsSettings();

                if (IsPostBack && IsCallback)
                    ASPxEdit.ValidateEditorsInContainer(this.btnSave);
                if (!IsPostBack)
                {
                    #region MasterPage
                    //comm.MasetPageSetting(this);
                    #endregion
                    InitForm();
                }
                else
                {
                    ClearMsg();
                }
            }
            else
                Response.Redirect("~/admin/adminlogin.aspx", false);

        }

        #region BindAgentGroup
        private void InitForm()
        {

            UIClass.SetComboStyle(ref cmbCountry, UIClass.EnumDefineStyle.Country);
            UIClass.SetComboStyle(ref cmbCountryBank, UIClass.EnumDefineStyle.Country);
            
            BindValue();
            #region BindControl
            //BindGrid();             

            if (optmode == "2")
            {
                BindState();
                BindStateBank();
            }
            //IEnumerable<Agent> list = agentbll.getAgentByLikeName("", comm.getStringByObject(Session["Admin"]));
            //if (list != null && list.Count<Agent>() > 0)
            //{
            //    Cache["gvList"] = list;
            //}
            //BindGridView(list, resultGridView);
            lblMsg.Text = "";
            #endregion
        }

        public void BindValue()
        {

            if (optmode == "2")
            {
                AgentData = AG_PROFILE_BLL.GetSingleAgentProfileByID(AgID);
                AgentBankInfo BankData = new AgentBankInfo();
                BankData = AG_BANK_BLL.GetAgentBankInfo(AgID);
                txt_AgentName.Text = AgentData.Username;
                // txt_AgentPWD.Text = AgentData.Password;
                txt_Email.Text = AgentData.Email;
                txt_Fax.Text = AgentData.Fax;
                txt_FirstName.Text = AgentData.ContactFirstName;
                txt_LastName.Text = AgentData.ContactLastName;
                txt_MobilePhone.Text = AgentData.MobileNo;
                txt_tel.Text = AgentData.PhoneNo;
                txtAddress1.Text = AgentData.Address1;
                txtAddress2.Text = AgentData.Address2;
                txtAddress3.Text = AgentData.Address3;
                txtAgentNo.Text = AgentData.LicenseNo;
                if (AgentData.OperationGroup.IndexOf("AA") != -1)
                    chkAirAsia.Checked = true;
                if (AgentData.OperationGroup.IndexOf("AX") != -1)
                    chkAirAsiaX.Checked = true;                
                //txt_AgentPWD.Enabled = false;
                //txt_AgentPWD2.Enabled = false;
                //txt_AgentPWD.Visible = false;
                //txt_AgentPWD2.Visible = false;
                txtCity.Text = AgentData.City;
                txtPostcode.Text = AgentData.Postcode;
                chkActive.Checked = Convert.ToBoolean(AgentData.Status);

                CmbTitle.SelectedIndex = CmbTitle.Items.IndexOfText(AgentData.Title);
                dt = objAgentProfile.GetCountryAndState(AgID);
                if (dt.Rows.Count > 0)
                {
                    cmbCountry.Items.FindByValue(dt.Rows[0]["Country"]).Selected = true;
                    BindState();
                    if (dt.Rows[0]["State"].ToString() != string.Empty)
                    {
                        cmbState.Items.FindByValue(dt.Rows[0]["State"]).Selected = true;
                    }
                }

                txtBankAccountName.Text = BankData.AccountName;
                txtBankAccountno.Text = BankData.AccountNo;
                txtBankAddress1.Text = BankData.Address1;
                txtBankAddress2.Text = BankData.Address2;
                txtBankAddress3.Text = BankData.Address3;
                txtBankName.Text = BankData.BankName;
                txtCityBank.Text = BankData.City;
                txtPostcodeBank.Text = BankData.Postcode;
                dt = objAgentProfile.GetBankCountryAndState(AgID);
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
                ////foreach (GroupBooking.Info.Agent_Info objStock in lstCountry)
                ////{

                //}
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
        private void BingAgentGroup(int agentgroupid)
        {
            //ddlAgentGroup.Items.Clear();

            //IList list = agentbll.BindddlAgentGroup();
            //foreach (object[] o in list)
            //{
            //    ListItem item = new ListItem();
            //    string val = comm.getStringByObject(o[0]);
            //    item.Value = val;
            //    item.Text = comm.getStringByObject(o[1]);
            //    ddlAgentGroup.Items.Add(item);
            //    //this.AgentGroupSelect.Items.Add(item);

            //    if (comm.getIntByObject(val) == agentgroupid)
            //    {
            //        ddlAgentGroup.Items.FindByValue(val).Selected = true;
            //        //AgentGroupSelect.Items.FindByValue(val).Selected = true;
            //    }
            //}
        }
        #endregion

        #region BindGridView
        //public void BindGridView(IEnumerable<Agent> list, GridView gv)
        //{
        //    //if (list==null)
        //    //{
        //    //    comm.BindGridview(gv, null);
        //    //    lblMsg.Text = "No data found, please retry.";
        //    //}
        //    //else
        //    //{
        //    //    comm.BindGridview(gv, list);
        //    //    lblMsg.Text = "";
        //    //}
        //}
        #endregion

        #region resultGridView Event
        protected void resultGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "UpdateRecord")
            //{
            //    int AgentId = comm.getIntByObject(e.CommandArgument);
            //    if (AgentId>0)
            //    {
            //        divList.Visible = true;
            //        DisplayAgent(AgentId);
            //    }
            //}
        }

        protected void resultGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex < 0)
            {
                //this.resultGridView.PageIndex = 0;
            }
            else
            {
                //  this.resultGridView.PageIndex = e.NewPageIndex;
            }
            if (Cache["gvList"] != null)
            {
                //BindGridView((IEnumerable<Agent>)Cache["gvList"], resultGridView);
            }
        }

        protected void resultGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowIndex>-1)
            //{
            //    //business logic
            //    int realIndex = this.resultGridView.AllowPaging ? (this.resultGridView.PageIndex) * this.resultGridView.PageSize + e.Row.RowIndex : e.Row.RowIndex;
            //    string isBlackList = (this.resultGridView.DataSource as DataTable).Rows[realIndex]["IsBlackList"].ToString();
            //    Button btn = (Button)e.Row.FindControl("btnUpdate");

            //    if (btn != null)
            //    {
            //        if (!string.IsNullOrEmpty(isBlackList))
            //        {
            //            if ("False".Equals(isBlackList))
            //            {
            //                btn.Text = comm.moveToBlackList();

            //            }
            //            else
            //            {
            //                btn.Text = comm.movefromBlackList();
            //            }
            //        }
            //    }
            //}
        }
        #endregion

        #region DisplayAgent
        protected void DisplayAgent(int AgentId)
        {
            //Agent agent = agentbll.GetAgent(AgentId);
            //if (agent != null)
            //{
            //    hfAgtID.Value = comm.getStringByObject(agent.AgentId);
            //    lblLoginName.Text = agent.AgentLoginName;
            //    lblUserType.Text = agent.AgentUserType;
            //    lblCompanyName.Text = agent.AgentCompany;
            //    lblCarrierCode.Text = agent.AgentCarrierCode;
            //    lblTitle.Text = agent.AgentTitle;
            //    lblFirstName.Text = agent.AgentFirstName;
            //    lblLastName.Text = agent.AgentLastName;
            //    lblEmail.Text = agent.AgentEmail;
            //    txtTel.Text = agent.AgentTel;
            //    lblFax.Text = agent.AgentFax;
            //    lblCity.Text = agent.AgentCity;
            //    lblCountry.Text = agent.AgentCountry;
            //    lblState.Text = agent.AgentState;
            //    lblMobile.Text = agent.AgentMobilePhone;
            //    //lblStatus.Text = agent.AgentStatus.ToString();
            //    if (agent.AgentStatus)
            //    {
            //        lblStatus.Text = "Active";
            //    }
            //    else
            //    {
            //        lblStatus.Text = "Disabled";
            //    }

            //    txtAddress.Text = agent.AgentAddress; ;

            //    if (agent.AgentStatus)
            //    {
            //        btnSubmit.Text = comm.moveToBlackList();
            //        btnSubmit.Attributes.Add("OnClick", "return Confirm(1)");
            //    }
            //    else 
            //    {
            //        btnSubmit.Text = comm.movefromBlackList();
            //        btnSubmit.Attributes.Add("OnClick", "return Confirm(0)");
            //    }

            //    //Bind DropdownList
            //    BingAgentGroup(agent.AgentAgentGroupID);
            //}
        }
        #endregion

        #region updateBlackList
        private void updateBlackList(int AgentId, int BlackListId)
        {
            //string user = comm.getStringByObject(Session["Admin"]);

            //optmodel.Entity.BlackList blacklist = new optmodel.Entity.BlackList();

            //blacklist.BlackListIsApprove = true;
            //blacklist.BlackListAgencyCompany = lblCompanyName.Text;
            //blacklist.BlackListAgencyGroup = lblCarrierCode.Text;
            //blacklist.BlackListAgentLoginName = lblLoginName.Text;
            //blacklist.BlackListReason = txtReason.Text.Trim();
            //if (btnSubmit.Text== comm.moveToBlackList())
            //{
            //    blacklist.BlackListIsBlackList = true;
            //}
            //else if(btnSubmit.Text== comm.movefromBlackList())
            //{
            //    blacklist.BlackListIsBlackList = false;
            //}
            //blacklist.BlackListCeateBy = user;
            //blacklist.BlackListCreateDate = DateTime.Now.ToString();

            //try
            //{
            //    blackbll.UpdateAgentandInsertBlacklist(blacklist);
            //    lblMsg.Text = "Update Blacklist successful. Please wait for approving.";
            //    Response.Write("<meta http-equiv= 'refresh' content= '3;URL=AgentList.aspx'>");
            //}
            //catch 
            //{
            //    lblMsg.Text = "Update record failure, please retry.";
            //}
        }
        #endregion

        protected void btn_search_Click(object sender, EventArgs e)
        {
            //string LikeName = comm.getStringByObject(this.txtLoginName.Text);
            //IEnumerable<Agent> list = agentbll.getAgentByLikeName(LikeName,comm.getStringByObject(Session["Admin"]));

            //if (list != null)
            //{
            //    Cache["gvList"] = list;
            //}

            //BindGridView(list, resultGridView);
        }


        protected void ASPxCallbackPanelDemo_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            ASPxCallbackPanel callbackPanel = (ASPxCallbackPanel)sender;
            bool isValid = ASPxEdit.ValidateEditorsInContainer(callbackPanel);
        }

        private void ClearMsg()
        {
            lblMsg.Text = "";
        }

        private void AssignValue()
        {
            string temp,optGroup = string.Empty;
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
            AG_PROFILE_INFO.Status = Convert.ToByte(chkActive.Checked);
            if (chkAirAsia.Checked)
                optGroup += "AA";
            if (chkAirAsiaX.Checked)
            {
                if (chkAirAsia.Checked)
                    optGroup += ",";
                optGroup += "AX";
            }
            AG_PROFILE_INFO.OperationGroup = optGroup;
            temp = DateTime.Now.ToString("yyyyMMddHHmmsss");
            if (Request.QueryString["optmode"].ToString() == "2")
            {
                AG_PROFILE_INFO.AgentID = AgID;
                AG_BANK_INFO.AgentID = AgID;
            }
            else if (Request.QueryString["optmode"].ToString() == "1") // insert
            {
                AG_PROFILE_INFO.AgentID = temp;
                AG_BANK_INFO.AgentID = temp;
            }
            AG_PROFILE_INFO.City = txtCity.Text;
            AG_PROFILE_INFO.ContactFirstName = txt_FirstName.Text;
            AG_PROFILE_INFO.ContactLastName = txt_LastName.Text;
            if (cmbCountry.SelectedIndex != -1)
                AG_PROFILE_INFO.Country = cmbCountry.SelectedItem.Value.ToString();
            AG_PROFILE_INFO.Email = txt_Email.Text;
            AG_PROFILE_INFO.Fax = txt_Fax.Text;
            AG_PROFILE_INFO.Flag = 0;
            AG_PROFILE_INFO.LastSyncBy = "System";
            AG_PROFILE_INFO.LicenseNo = txtAgentNo.Text;
            AG_PROFILE_INFO.MobileNo = txt_MobilePhone.Text;
            AgentData = AG_PROFILE_BLL.GetSingleAgentProfileByID(AgID);
            if (Request.QueryString["optmode"].ToString() == "1")
                AG_PROFILE_INFO.Password = txt_AgentPWD.Text;
            if (Request.QueryString["optmode"].ToString() == "2" && txt_AgentPWD.Text != string.Empty)
                AG_PROFILE_INFO.Password = txt_AgentPWD.Text;
            if (Request.QueryString["optmode"].ToString() == "2" && txt_AgentPWD.Text == string.Empty)
                AG_PROFILE_INFO.Password = AgentData.Password;
            AG_PROFILE_INFO.PhoneNo = txt_tel.Text;
            AG_PROFILE_INFO.Postcode = txtPostcode.Text;
            if (cmbState.SelectedIndex != -1)
                AG_PROFILE_INFO.State = cmbState.SelectedItem.Value.ToString();
            AG_PROFILE_INFO.Title = CmbTitle.SelectedItem.Value.ToString();
            AG_PROFILE_INFO.Username = txt_AgentName.Text;

        }
        private void SaveData()
        {
            //assign value

            if (optmode == "1")
            {
                newAgentProfile = AG_PROFILE_BLL.SaveAgentProfile(AG_PROFILE_INFO, AG_BANK_INFO, ABS.Logic.Shared.CoreBase.EnumSaveType.Insert);

            }
            else if (optmode == "2")
            {
                newAgentProfile = AG_PROFILE_BLL.SaveAgentProfile(AG_PROFILE_INFO, AG_BANK_INFO, ABS.Logic.Shared.CoreBase.EnumSaveType.Update);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int flag1 = 0;
            int flag2 = 0;
            int flag3 = 0;
            int flag5 = 0;
            int flag6 = 0;
            int flag7 = 0;
            int flag8 = 0;
            if (txt_AgentPWD.Text == string.Empty && optmode == "1")
            {
                txt_AgentPWD.IsValid = false;
                txt_AgentPWD2.IsValid = false;
                flag3 = 1;
            }
            if (txt_AgentPWD.Text == txt_AgentName.Text && Request.QueryString["optmode"] == "2")
            {
                txt_AgentPWD.IsValid = false;
                flag5 = 1;
            }
            if (txt_AgentPWD.Text == txt_AgentName.Text && Request.QueryString["optmode"] == "1")
            {
                txt_AgentPWD.IsValid = false;
                flag6 = 1;
            }
            if (chkAirAsia.Checked == false && chkAirAsiaX.Checked == false && Request.QueryString["optmode"] == "1")
            {
                chkAirAsia.ValidationSettings.ErrorText = "Please check at least one group";
                chkAirAsia.IsValid = false;
                flag7 = 1;
            }
            if (chkAirAsia.Checked == false && chkAirAsiaX.Checked == false && Request.QueryString["optmode"] == "2")
            {
                chkAirAsia.ValidationSettings.ErrorText = "Please check at least one group";
                chkAirAsia.IsValid = false;
                flag8 = 1;
            }
            List<AgentProfile> lstEmail = new List<AgentProfile>();
            lstEmail = AG_PROFILE_BLL.GetAllAg_Email(txt_Email.Text);
            List<AgentProfile> lstUser = new List<AgentProfile>();
            lstUser = AG_PROFILE_BLL.GetAllUsername(txt_AgentName.Text);
            AgentProfile UserData = new AgentProfile();
            UserData = AG_PROFILE_BLL.GetSingleAgentProfileByID(AgID);

            if (lstEmail != null && optmode == "1")
            {
                txt_Email.IsValid = false;
                flag1 = 1;
            }
            if (lstUser != null && optmode == "1")
            {
                txt_AgentName.IsValid = false;
                flag2 = 1;
            }
            if (lstEmail != null && optmode == "2" && UserData.Email != txt_Email.Text)
            {
                txt_Email.IsValid = false;
                flag1 = 1;
            }
            if (lstUser != null && optmode == "2" && UserData.Username != txt_AgentName.Text)
            {
                txt_AgentName.IsValid = false;
                flag2 = 1;
            }
            if (ASPxEdit.AreEditorsValid(RPanelData))
            {
                AssignValue();
                SaveData();
                Thread.Sleep(2000);
                if (optmode == "1" && newAgentProfile != null)
                {
                    lblMsg.Text = "Insert Success";
                    Session["agid"] = newAgentProfile.AgentID;
                }
                else if (optmode == "2" && newAgentProfile != null)
                    lblMsg.Text = "Update Success";

                lblMsg.Visible = true;
            }
            else
            {
                lblMsg.Text = "Insert Failed, please check the data again";
                if (flag1 == 1 && flag2 == 1)
                    lblMsg.Text = "Insert Failed, the Username and Email already exist";
                else
                    if (flag1 == 1)
                        lblMsg.Text = "Insert Failed, the Email already exist";
                    else
                        if (flag2 == 1)
                            lblMsg.Text = "Insert Failed, the Username already exist";
                        else
                            if (flag3 == 1)
                                lblMsg.Text = "Insert Failed, Password is required";
                            else
                                if (flag5 == 1)
                                    lblMsg.Text = "Update Failed, the new password should not be same with the agent account";
                                else
                                    if (flag6 == 1)
                                        lblMsg.Text = "Insert Failed, the password should not be same with the agent account";
                                    else
                                        if (flag7 == 1)
                                            lblMsg.Text = "Insert Failed, choose at least one operation group";
                                        else                                        
                                        if (flag8 == 1)
                                            lblMsg.Text = "Update Failed, choose at least one operation group";
                lblMsg.Visible = true;
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
        private void ApplyValidationSummarySettings()
        {
            vsValidationSummary1.ShowErrorAsLink = true;
        }
        private void ApplyEditorsSettings()
        {
            //ASPxEdit[] editors = new ASPxEdit[] { txt_AgentName, txt_AgentPWD , txt_AgentPWD2 , txt_Email,};
            //foreach (ASPxEdit editor in editors)
            //    editor.ValidationSettings.SetFocusOnError = true;
        }

        protected void BtnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("agentlist.aspx", false);
        }

        protected void btnSendEmail_Click(object sender, EventArgs e)
        {
            ABS.GBS.eService.ProcessServiceSoapClient eServices = new ABS.GBS.eService.ProcessServiceSoapClient();
            try
            {
                AgentProfile agentDTL = AG_PROFILE_BLL.GetSingleAgentProfileByID(AgID);
                if (agentDTL != null && agentDTL.Email != string.Empty)
                {
                    string msg = eServices.GroupBookingEmailing(1, "0", agentDTL.Email,"",0, "");
                    if (msg != string.Empty && msg == "Error with EmailBody")
                        lblMsg.Text = "Send email failed, please try again or contact our support line.";
                    else
                        lblMsg.Text = "Email sent successfully.";
                }
                else
                {
                    lblMsg.Text = "Send email failed, please try again or contact our support line.";
                }
                lblMsg.Visible = true;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);

            }
        }

    }
}