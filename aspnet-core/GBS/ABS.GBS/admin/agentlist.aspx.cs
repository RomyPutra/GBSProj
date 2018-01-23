using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Text;
using System.Collections;
using ABS.Logic.GroupBooking.Agent;
using DevExpress.Web;
using System.Data;
using ABS.Logic.Shared;

namespace GroupBooking.Web.Administrator
{
    public partial class AgentList : System.Web.UI.Page
    {
        AgentControl.StrucAgentSet AgentSet;
        AdminSet AdminSet;
        AgentProfile AgentInfo = new ABS.Logic.GroupBooking.Agent.AgentProfile();
        AgentProfile AgProfileInfo = new AgentProfile();
        ABS.Logic.GroupBooking.Agent.AgentControl objAgent = new ABS.Logic.GroupBooking.Agent.AgentControl();
        ABS.Logic.GroupBooking.Agent.AgentProfileControl objAgentData = new ABS.Logic.GroupBooking.Agent.AgentProfileControl();
        ABS.Logic.GroupBooking.Booking.RequestApp ReqInfo = new ABS.Logic.GroupBooking.Booking.RequestApp();
        ABS.Logic.GroupBooking.Settings SetInfo = new ABS.Logic.GroupBooking.Settings();
        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();

        enum ControlState
        {
            Operator = 0,
            Supervisor = 1,
            Manager = 2, 
            Viewer = 3 //20170522 - Sienny (new role)
        }
        ControlState _myState;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminSet"] != null)
            {
                InitState();
                if (!IsPostBack)
                {
                    setDefaultValue();
                    LoadGridViewDefault();
                }
                LoadAgentSetting();
                LoadGridView();

            }
            else
            {
                Response.Redirect("~/admin/adminlogin.aspx");
            }

        }

        protected void LoadAgentSetting()
        {
            if (Session["AgentSet2"] != null)
            {
                AgentSet = (AgentControl.StrucAgentSet)Session["AgentSet2"];
            }
        }

        protected void setDefaultValue()
        {
            Session["dtGrid"] = null;
            Session["optemode"] = null;
            Session["agid"] = null;
            chkBlacklist.Checked = false;
            lblError.Visible = false;
            pnlBlacklist.Visible = false;
            btnSubmit.Visible = false;
        }

        protected void LoadGridViewDefault()
        {
            if (_myState == ControlState.Operator)
            {
                gridAgent.Columns["Action"].Visible = false;
                gridAgent.Columns["Request"].Visible = true;
                chkBlacklist.Visible = false;
            }
            if (_myState == ControlState.Supervisor || _myState == ControlState.Manager)
            {
                gridAgent.Columns["Action"].Visible = false;
                gridAgent.Columns["Request"].Visible = true;
                chkBlacklist.Visible = true;
            }
            SearchData(cmbFilter.SelectedItem.Value.ToString(), txtSearch.Text);
            gridAgent.DataSource = (DataTable)Session["dtGrid"];
            gridAgent.DataBind();

        }

        protected void LoadGridView()
        {

            gridAgent.DataSource = (DataTable)Session["dtGrid"];
            gridAgent.DataBind();

        }

        protected void gridAgent_CommandButtonInitialize(object sender, DevExpress.Web.ASPxGridViewCommandButtonEventArgs e)
        {
            if (chkBlacklist.Checked)
            {
                if (e.ButtonType == DevExpress.Web.ColumnCommandButtonType.SelectCheckbox)
                {
                    e.Visible = true;


                }

            }
            else
            {
                if (e.ButtonType == DevExpress.Web.ColumnCommandButtonType.SelectCheckbox)
                {
                    e.Visible = false;
                }

            }

        }

        protected void InitState()
        {
            AdminSet = (AdminSet)Session["AdminSet"];
            switch (AdminSet.GroupCode)
            {
                case "01":
                    _myState = ControlState.Operator;
                    break;
                case "02":
                    _myState = ControlState.Supervisor;
                    break;
                case "03":
                    _myState = ControlState.Manager;
                    break;
                //20170522 - Sienny (new role)
                case "04":
                    _myState = ControlState.Viewer;
                    break;
            }
        }

        protected void gridAgent_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            object rowKey = null;

            if (e.ButtonID == "editBtn")
            {
                rowKey = gridAgent.GetRowValues(e.VisibleIndex, "AgentID");
                Session["agid"] = rowKey;
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("../Admin/agentdetail.aspx?optmode=2");
            }
        }
        protected void callbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            memoRemarks.Text = "";
            lblTransID.Text = (e.Parameter).ToString();
            Session["TransReq"] = lblTransID.Text;
            AgentInfo = objAgentData.GetSingleAgentProfileByID(lblTransID.Text);
            lblAgentName.Text = AgentInfo.ContactFirstName + " " + AgentInfo.ContactLastName;
        }

        protected void gridAgent_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            Session["dtGrid"] = objAgent.SearchAgentBlacklistRequest(cmbFilter.SelectedItem.Value.ToString(), txtSearch.Text);
            gridAgent.DataSource = (DataTable)Session["dtGrid"];
            gridAgent.DataBind();
        }

        protected void SaveRequest()
        {
            string temp = "";
            AdminSet = (AdminSet)Session["AdminSet"];
            SetInfo = objGeneral.GetSingleSYS_PREFT(1, "AA", "REQEXPIRY");
            temp = DateTime.Now.ToString("yyyyMMddHHmmsss");
            ReqInfo.ReqID = temp;
            ReqInfo.Remark = memoRemarks.Text;
            ReqInfo.ReqType = "B";
            ReqInfo.TransID = Session["TransReq"].ToString();
            ReqInfo.UserID = AdminSet.AdminID;
            ReqInfo.LastSyncBy = AdminSet.AdminID;
            ReqInfo.RequestDate = DateTime.Now;
            ReqInfo.ExpiryDate = (DateTime.Now).AddDays(Convert.ToInt16(SetInfo.SYSValue));
            objBooking.SaveREQAPPL(ReqInfo, ABS.Logic.GroupBooking.Booking.BookingControl.EnumSaveType.Insert);
        }
        protected void btnRequest_Click(object sender, EventArgs e)
        {
            if (ASPxEdit.AreEditorsValid(callbackPanel))
            {

                SaveRequest();
                Session["dtGrid"] = objAgent.SearchAgentDataAdmin(cmbFilter.SelectedItem.Value.ToString(), txtSearch.Text);
                gridAgent.DataSource = (DataTable)Session["dtGrid"];
                gridAgent.DataBind();
            }

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (listboxBlacklist.Rows > 0)
            {
                UpdateToBlacklist();
                gridAgent.Selection.UnselectAll();

            }
            else
            {
                lblError.Text = "Please select the Agent !";
                lblError.Visible = true;
            }
        }

        protected void RequestLink_Load(object sender, EventArgs e)
        {
            GridViewDataItemTemplateContainer c = ((ASPxHyperLink)sender).NamingContainer
                as GridViewDataItemTemplateContainer;
            // ((ASPxHyperLink)sender).Enabled = false;

        }

        protected void UpdateToBlacklist()
        {
            if (gridAgent.Selection.Count > 0)
                if (hidClient["black"] != null)
                {
                    string[] arrID = ((string)hidClient["black"]).Split(';');

                    for (int i = 0; i < arrID.Count(); i++)
                    {
                        objAgent.SetWhiteList(arrID[i], AgentSet.LoginName);
                    }
                    Session["dtGrid"] = objAgent.SearchAgentBlacklist(cmbFilter.SelectedItem.Value.ToString(), txtSearch.Text);
                    LoadGridView();
                    listboxBlacklist.Items.Clear();
                }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //added by jia kang
            if (chkBlacklist.Checked)
            {
                gridAgent.Columns["rowCheckBox"].Visible = true;
                gridAgent.Columns["Request"].Visible = false;
            }

            else if (cmbFilter.SelectedIndex == 10 && cmbAgStatus.SelectedIndex == 1)
            {
                gridAgent.Columns["rowCheckBox"].Visible = false;
                gridAgent.Columns["Request"].Visible = false;
            }
            else
            {
                gridAgent.Columns["rowCheckBox"].Visible = false;
                gridAgent.Columns["Request"].Visible = true;
            }
            SearchData(cmbFilter.SelectedItem.Value.ToString(), txtSearch.Text);


        }

        protected void SearchData(string fieldName, string filter)
        {
            if (chkBlacklist.Checked && cmbFilter.SelectedIndex != 10)
            {
                Session["dtGrid"] = objAgent.SearchAgentBlacklist(fieldName, filter);
                pnlBlacklist.Visible = false;
                btnSubmit.Visible = true;
            }
            else if (cmbFilter.SelectedIndex == 10 && chkBlacklist.Checked == false)
            {
                Session["dtGrid"] = objAgent.SearchInactAgent(cmbAgStatus.SelectedItem.ToString());

                listboxBlacklist.Items.Clear();
                pnlBlacklist.Style["display"] = "none";
                btnSubmit.Visible = false;
            }
            else if (cmbFilter.SelectedIndex == 10 && chkBlacklist.Checked == true)
            {
                Session["dtGrid"] = objAgent.SearchInactBlackAgent(cmbAgStatus.SelectedItem.ToString());
                pnlBlacklist.Visible = false;
                btnSubmit.Visible = true;
            }

            else
            {
                Session["dtGrid"] = objAgent.SearchAgentDataAdmin(fieldName, filter);

                listboxBlacklist.Items.Clear();
                pnlBlacklist.Style["display"] = "none";
                btnSubmit.Visible = false;
            }
            LoadGridView();
        }

        protected void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSubmit.Visible = false;
            cmbAgStatus.Visible = false;
            if (cmbFilter.SelectedIndex == 0)
            {

                tdSearch.Visible = false;
                if (chkBlacklist.Checked == true)
                {
                    btnSubmit.Visible = true;
                }
            }
            else if (cmbFilter.SelectedIndex == 10)
            {

                tdSearch.Visible = false;
                cmbFilter.Visible = true;
                cmbAgStatus.Visible = true;
                txtSearch.Visible = false;
                if (chkBlacklist.Checked == true)
                {
                    btnSubmit.Visible = true;
                }



            }
            else
            {


                tdSearch.Visible = true;
                txtSearch.Visible = true;
                if (chkBlacklist.Checked == true)
                {
                    btnSubmit.Visible = true;
                }

            }

        }
        protected void cmbAgStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cmbAgStatus.SelectedIndex == 1)
            {
                gridAgent.Columns["Request"].Visible = false;
                SearchData(cmbFilter.SelectedItem.Value.ToString(), txtSearch.Text);
            }

            else
            {

                gridAgent.Columns["Request"].Visible = true;
                SearchData(cmbFilter.SelectedItem.Value.ToString(), txtSearch.Text);

            }
        }


    }
}


