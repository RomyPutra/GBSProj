using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Text;
using System.Collections;
using ABS.Logic.GroupBooking.Agent;
using System.Data;
using ABS.Logic.Shared;
using DevExpress.Web;
using ABS.Logic.GroupBooking.Booking;
using ABS.Logic.GroupBooking.Agent;
using ABS.Logic.GroupBooking;

namespace GroupBooking.Web.Administrator
{
    public partial class AgentBlacklist : System.Web.UI.Page
    {
        AgentControl.StrucAgentSet AgentSet;
        AdminSet AdminSet;
        AgentProfile AgentInfo = new ABS.Logic.GroupBooking.Agent.AgentProfile();
        ABS.Logic.GroupBooking.Agent.AgentProfileControl objAgentData = new ABS.Logic.GroupBooking.Agent.AgentProfileControl();
        ABS.Logic.GroupBooking.Agent.AgentControl objAgent = new ABS.Logic.GroupBooking.Agent.AgentControl();
        ABS.Logic.GroupBooking.Booking.RequestApp ReqInfo = new ABS.Logic.GroupBooking.Booking.RequestApp();
        ABS.Logic.GroupBooking.Settings SetInfo = new ABS.Logic.GroupBooking.Settings();
        ABS.Logic.GroupBooking.Agent.RequestApp ReqAgWhite = new ABS.Logic.GroupBooking.Agent.RequestApp();
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
                    LoadGridView();
                }
                //LoadAgentSetting();
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
        protected void setDefaultValue()
        {
            Session["dtGrid"] = null;
            Session["optemode"] = null;
            Session["agid"] = null;
            lblError.Visible = false;

        }

        protected void LoadGridView()
        {
            //if (Session["dtGrid"] == null)
            //{
            //    Session["dtGrid"] = objAgent.GetAllActiveAgent();
            //}
            if (_myState == ControlState.Operator)
            {
                gridAgent.Columns["Action"].Visible = false;
                gridAgent.Columns["Request"].Visible = true;
            }
            if (_myState == ControlState.Supervisor || _myState == ControlState.Manager)
            {
                gridAgent.Columns["Action"].Visible = true;
                gridAgent.Columns["Request"].Visible = false;
            }
            Session["dtGrid"] = objAgent.SearchAgentBlacklistRequest(cmbFilter.SelectedItem.Value.ToString(), txtSearch.Text);
            gridAgent.DataSource = (DataTable)Session["dtGrid"];
            gridAgent.DataBind();
        }

        protected void gvRequest_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            Session["dtGrid"] = objAgent.SearchAgentBlacklistRequest(cmbFilter.SelectedItem.Value.ToString(), txtSearch.Text);
            gridAgent.DataSource = (DataTable)Session["dtGrid"];
            gridAgent.DataBind();
        }

        protected void gridAgent_CommandButtonInitialize(object sender, DevExpress.Web.ASPxGridViewCommandButtonEventArgs e)
        {
            //if (chkBlacklist.Checked)
            //{
            //    if (e.ButtonType == DevExpress.Web.ColumnCommandButtonType.SelectCheckbox)
            //    {
            //        e.Visible = true;
            //    }                

            //}
            //else
            //{
            //    if (e.ButtonType == DevExpress.Web.ColumnCommandButtonType.SelectCheckbox)
            //    {
            //        e.Visible = false;
            //    }                

            //}

        }
        protected void callbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            memoRemarks.Text = "";
            lblTransID.Text = (e.Parameter).ToString();
            Session["TransReq"] = lblTransID.Text;
            AgentInfo = objAgentData.GetSingleAgentProfileByID(lblTransID.Text);
            lblAgentName.Text = AgentInfo.ContactFirstName + " " + AgentInfo.ContactLastName;
        }
        protected void SaveRequest()
        {
            string temp = "";
            AdminSet = (AdminSet)Session["AdminSet"];
            SetInfo = objGeneral.GetSingleSYS_PREFT(1, "AA", "REQEXPIRY");
            temp = DateTime.Now.ToString("yyyyMMddHHmmsss");
            ReqInfo.ReqID = temp;
            ReqInfo.Remark = memoRemarks.Text;
            ReqInfo.ReqType = "W";
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
                Session["dtGrid"] = objAgent.SearchAgentBlacklistRequest(cmbFilter.SelectedItem.Value.ToString(), txtSearch.Text);
                gridAgent.DataSource = (DataTable)Session["dtGrid"];
                gridAgent.DataBind();
            }

        }
        protected void gridAgent_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            object rowKey = null;
            AdminSet = (AdminSet)Session["AdminSet"];
            SetInfo = objGeneral.GetSingleSYS_PREFT(1, "AA", "REQEXPIRY");
            if (e.ButtonID == "editBtn")
            {
                string temp = DateTime.Now.ToString("yyyyMMddHHmmsss");
                rowKey = gridAgent.GetRowValues(e.VisibleIndex, "AgentID");
                Session["agid"] = rowKey;

                ReqAgWhite.ApprovedBy = AdminSet.AdminID;
                ReqAgWhite.UserID = AdminSet.AdminID;
                ReqAgWhite.ReqID = temp;
                ReqAgWhite.ReqType = "W";
                ReqAgWhite.RequestDate = DateTime.Now;
                ReqAgWhite.TransID = Session["agid"].ToString();
                ReqAgWhite.ExpiryDate = (DateTime.Now).AddDays(Convert.ToInt16(SetInfo.SYSValue));
                ReqAgWhite.ApprovedDate = DateTime.Now;
                ReqAgWhite.LastSyncBy = AdminSet.AdminID;
                ReqAgWhite.SyncLastUpd = DateTime.Now;
                ReqAgWhite.SyncCreate = DateTime.Now;
                ReqAgWhite.Remark = AdminSet.AdminID + "Request";

                objAgent.SetAdminWhiteList(rowKey.ToString(), AdminSet.AdminID, ReqAgWhite);
                Session["dtGrid"] = objAgent.SearchAgentBlacklistRequest(cmbFilter.SelectedItem.Value.ToString(), txtSearch.Text);
                gridAgent.DataSource = (DataTable)Session["dtGrid"];
                gridAgent.DataBind();
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //if (listboxBlacklist.Rows > 0)
            //{
            //    UpdateToBlacklist();
            //    gridAgent.Selection.UnselectAll();

            //}
            //else
            //{
            //    lblError.Text = "Please select the Agent !";
            //    lblError.Visible = true;
            //}
        }

        //protected void btnInsert_Click(object sender, EventArgs e)
        //{
        //    Session.Remove("agid");
        //    Session["mode"] = "insert";

        //    Response.Redirect("../admin/agentdetail.aspx",false);
        //}               

        protected void UpdateToBlacklist()
        {
            if (hidClient["black"] != null)
            {
                string[] arrID = ((string)hidClient["black"]).Split(';');

                for (int i = 0; i < arrID.Count(); i++)
                {
                    objAgent.SetWhiteList(arrID[i], AgentSet.LoginName);
                }
                Session["dtGrid"] = objAgent.SearchAgentBlacklistRequest(cmbFilter.SelectedItem.Value.ToString(), txtSearch.Text);
                LoadGridView();
                // listboxBlacklist.Items.Clear();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            SearchData(cmbFilter.SelectedItem.Value.ToString(), txtSearch.Text);
        }

        protected void SearchData(string fieldName, string filter)
        {
            Session["dtGrid"] = objAgent.SearchAgentBlacklistRequest(fieldName, filter);

            LoadGridView();
        }

        protected void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilter.SelectedIndex == 0)
            {
                tdSearch.Visible = false;

            }
            else
            {
                tdSearch.Visible = true;

            }

        }


    }
}


