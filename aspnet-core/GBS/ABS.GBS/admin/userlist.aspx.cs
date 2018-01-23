using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ABS.Logic.GroupBooking.Agent;
using ABS.Logic.Shared;
using ABS.Logic.GroupBooking;
using System.Data;
using DevExpress.Web;

namespace GroupBooking.Web.admin
{

    public partial class userlist : System.Web.UI.Page
    {
        USRPROFILE_Info UsrInfo = new USRPROFILE_Info();
        USRAPP_Info UsrAppInfo = new USRAPP_Info();
        USRPROFILE_Info UsrInfoInsert = new USRPROFILE_Info();
        USRAPP_Info UsrAppInfoInsert = new USRAPP_Info();
        AdminSet AdminSet;
        AgentProfileControl AgentProfile = new AgentProfileControl();
        AgentProfile AgProfileInfo = new AgentProfile();
        ABS.Logic.GroupBooking.Agent.AgentControl objAgent = new ABS.Logic.GroupBooking.Agent.AgentControl();
        string tempID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminSet"] != null)
            {
                if (IsCallback && Request.QueryString["optmode"] == "2")
                {
                    Clear_Text();
                }
                if (Request.QueryString["optmode"] == "3")
                {
                    Session.Remove("DataUser");
                }
                if (!IsPostBack)
                {
                    Session["dtGrid"] = null;
                    Session["mode"] = null;
                    Session["agid"] = null;
                }
                LoadAgentSetting();
                LoadGridView();

                if (Session["DataUser"] != null)
                {
                    //FillData();
                    //txt_UserID.Enabled = false;
                    //Added by Ellis 20170310, disable txt_UserID on edit
                    txt_UserID.ReadOnly = true;
                    string temp = txt_UserID.Text;
                    //btnSave.Text = "Save";

                }
                else
                {
                    //btnSave.Text = "Insert";
                    //txt_UserID.Enabled = true;
                    //Added by Ellis 20170310, disable txt_UserID on edit
                    txt_UserID.ReadOnly = false;
                }
                //}
            }
            else
                Response.Redirect("~/admin/adminlogin.aspx");
        }

        protected void FillData()
        {
            DataTable DataUser = (DataTable)Session["DataUser"];
            txt_UserID.Text = DataUser.Rows[0]["UserID"].ToString();
            txtUsername.Text = DataUser.Rows[0]["UserName"].ToString();
            txtPassword.Text = DataUser.Rows[0]["Password"].ToString();
            txtRefID.Text = DataUser.Rows[0]["RefID"].ToString();
            if (DataUser.Rows[0]["OperationGroup"].ToString().IndexOf("AA") != -1)
                chkAirAsia.Checked = true;
            if (DataUser.Rows[0]["OperationGroup"].ToString().IndexOf("AX") != -1)
                chkAirAsiaX.Checked = true;
            //txtRefID.Text = "qwolskerit";
            cmbGroup.Items.FindByText(DataUser.Rows[0]["GroupName"].ToString()).Selected = true;
        }
        protected void LoadAgentSetting()
        {
            lblMsg.Visible = false;
            if (Session["AdminSet"] != null)
            {
                AdminSet = (AdminSet)Session["AdminSet"];
            }
        }

        protected void LoadGridView()
        {
            if (Session["dtGrid"] == null)
                SearchData(cmbFilter.SelectedItem.Value.ToString(), txtSearch.Text);
            else
            {
                gridAgent.DataSource = (DataTable)Session["dtGrid"];
                gridAgent.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchData(cmbFilter.SelectedItem.Value.ToString(), txtSearch.Text);
        }

        protected void SearchData(string fieldName, string filter)
        {
            Session["dtGrid"] = objAgent.SearchAdminList(fieldName, filter);
            LoadGridView();
        }


        protected void callbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            DataTable DataUser = new DataTable();
            if (e.Parameter.ToString() != "")
            {
                if ((e.Parameter).ToString() != "new")
                {
                    //Added by Ellis 20170310, disable txt_UserID on edit
                    txt_UserID.ReadOnly = true;
                    txt_UserID.Text = (e.Parameter).ToString();
                    DataUser = objAgent.GetSingleAdmin((e.Parameter).ToString());
                    Session["DataUser"] = DataUser;
                    lblHeadReq.Text = "Update User";
                    FillData();
                }
                else
                {
                    //Added by Ellis 20170310, enable txt_UserID on add
                    txt_UserID.ReadOnly = false;
                    lblHeadReq.Text = "New User";
                    Clear_Text();
                    Session.Remove("DataUser");
                }
            }
            //Session["TransReqTransID"] = txt_UserID.Text;
        }


        protected void EditClick(object sender, EventArgs e)
        {
            Button btnEdit = (Button)sender;
            GridViewRow gridViewRow = (GridViewRow)btnEdit.NamingContainer;

        }

        protected void AssignValue()
        {
            //string temp = "";
            //temp = DateTime.Now.ToString("yyyyMMddHHmmsss");
            string optGroup = "";
            UsrInfoInsert.UserID = txt_UserID.Text;
            UsrInfoInsert.UserName = txtUsername.Text;
            UsrInfoInsert.RefID = txtRefID.Text;
            UsrInfoInsert.Status = Convert.ToByte(chkActive.Checked);
            UsrInfoInsert.Password = txtPassword.Text;
            UsrInfoInsert.LastSyncBy = "0";
            if (chkAirAsia.Checked)
                optGroup += "AA";
            if (chkAirAsiaX.Checked)
            {
                if (chkAirAsia.Checked)
                    optGroup += ",";
                optGroup += "AX";
            }
            UsrInfoInsert.OperationGroup = optGroup;
            UsrAppInfoInsert.UserID = txt_UserID.Text;
            UsrAppInfoInsert.AppID = 1;
            UsrAppInfoInsert.AccessCode = cmbGroup.SelectedItem.Value.ToString();
            UsrAppInfoInsert.IsInherit = 1;
            UsrAppInfoInsert.LastSyncBy = "0";
        }
        private void SaveData()
        {
            //assign value
            AssignValue();

            //tempID= objAgent. AgentProfile(AG_PROFILE_INFO, AG_BANK_INFO, ABS.Logic.Shared.CoreBase.EnumSaveType.Update);
            if (Session["DataUser"] != null)
            {
                tempID = AgentProfile.SaveUserProfile(UsrInfoInsert, UsrAppInfoInsert, ABS.Logic.Shared.CoreBase.EnumSaveType.Update);
            }
            else
                tempID = AgentProfile.SaveUserProfile(UsrInfoInsert, UsrAppInfoInsert, ABS.Logic.Shared.CoreBase.EnumSaveType.Insert);

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //AssignValue();
            if (chkAirAsia.Checked == false && chkAirAsiaX.Checked == false)
            {
                chkAirAsia.ValidationSettings.ErrorText = "Please check at least one group";
                chkAirAsia.IsValid = false;
                lblMsg.Visible = true;
                lblMsg.Text = btnSave.Text + " failed, please check at least one operation group";
            }
            if (ASPxEdit.AreEditorsValid(popup))
            {

                SaveData();
                if (tempID == null)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = btnSave.Text + " failed, please check the data";
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = btnSave.Text + " success";
                }
                Session.Remove("DataUser");
                //DevExpress.Web.ASPxWebControl.RedirectOnCallback("userlist.aspx?optmode=3");

                SearchData(cmbFilter.SelectedItem.Value.ToString(), txtSearch.Text);
                Clear_Text();
            }
        }
        protected void Clear_Text()
        {
            txt_UserID.Enabled = true;
            txt_UserID.Text = "";
            txtPassword.Text = "";
            txtRefID.Text = "";
            txtUsername.Text = "";
            cmbGroup.SelectedIndex = 0;
            chkActive.Checked = true;
        }
    }
}