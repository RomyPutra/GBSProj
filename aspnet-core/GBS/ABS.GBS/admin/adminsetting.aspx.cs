using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using ABS.Logic.Shared;
using DevExpress.Web;
using DevExpress.Data.Filtering;

using ABS.Logic.GroupBooking;
using ABS.Logic.GroupBooking.Agent;
using ABS.Logic.GroupBooking.Booking;
using System.Globalization;
using System.Web.Services;
using System.IO;
using DevExpress.Web.Data;
using ABS.GBS.Log;
using ABS.Navitaire.AgentManager;
using System.Configuration;
using DevExpress.XtraGrid.Columns;

namespace GroupBooking.Web.admin
{
    public partial class settingpage : System.Web.UI.Page
    {
        LogControl log = new LogControl();
        SystemLog SystemLog = new SystemLog();
        Settings SettingVal = new Settings();
        GeneralControl objGeneral = new GeneralControl();
        BookingControl objBooking = new BookingControl();
        AgentCategory SettingAgent = new AgentCategory();
        AgentControl objAgent = new AgentControl();
        PaymentControl objPayment = new PaymentControl();
        PaymentInfo SettingPayment = new PaymentInfo();
        CODEMASTER code = new CODEMASTER();
        List<CODEMASTER> lstOpt = new List<CODEMASTER>();
        Settings newSys_Preft = new Settings();
        AdminSet AdminSet;
        List<object> selectedValues;

        int SYSSet = 0;
        DataTable dt = new DataTable();
        enum ControlState
        {
            Payment = 2,
            Reminder = 3,
            Charge = 4
        }
        ControlState _mystate = new ControlState();

        protected void Page_Load(object sender, EventArgs e)
        {
            //SelectState();
            _mystate = ControlState.Reminder;
            SYSSet = 3;

            if (Session["AdminSet"] != null)
            {
                AdminSet = (AdminSet)Session["AdminSet"];
            }
            else
            {
                Response.Redirect("~/admin/adminlogin.aspx");
            }
            if (!Page.IsPostBack)
            {

                InitValue();
                InitRestriction();
            }
            else
            {
                LoadDisplay();
            }
            ClearSelectionOnOtherPages();
        }

        protected void SelectState()
        {
            /// commented by diana 20130913
            //string tabname = TabControl.ActiveTabPage.Text;            
            //switch (tabname)
            //{
            //    case "Payment":
            //        _mystate = ControlState.Payment;
            //        break;
            //    case "Reminder":
            //        _mystate = ControlState.Reminder;
            //         break;
            //    case "Charge":
            //         _mystate = ControlState.Charge;
            //         break;             
            //}
            //switch (_mystate)
            //{
            //    case ControlState.Payment:
            //        SYSSet = 2;
            //        break;
            //    case ControlState.Reminder:
            //        SYSSet = 3;
            //        break;
            //    case ControlState.Charge:
            //        SYSSet = 4;
            //        break;
            //}
            /// end commented by diana 20130913
        }
        protected void InitRestriction()
        {
            lstOpt = objGeneral.GetAllCODEMASTERFilterCode("RST");
            for (int i = 0; i < lstOpt.Count; i++)
            {
                CODEMASTER code = lstOpt[i];
                if (code.Code == "BOOKFROM")
                {
                    txtStartDate.Value = DateTime.ParseExact(code.CodeDesc, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
                else if (code.Code == "BOOKTO")
                {
                    txtEndDate.Value = DateTime.ParseExact(code.CodeDesc, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
                else if (code.Code == "TRAFROM")
                {
                    txtStartDateTravel.Value = DateTime.ParseExact(code.CodeDesc, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
                else if (code.Code == "TRATO")
                {
                    txtEndDateTravel.Value = DateTime.ParseExact(code.CodeDesc, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }

                else if (code.Code == "IND")
                {
                    if (code.CodeDesc == "1")
                    {
                        chkRestriction.Checked = true;
                        chkRestriction.Text = "Enable";
                    }
                    else
                    {
                        chkRestriction.Checked = false;
                        chkRestriction.Text = "Disable";
                    }
                }
            }
            newSys_Preft = objGeneral.GetSingleSYS_PREFT(1, "AA", "RESTRICTIONNOTE");
            txtRestrictionNote.Text = newSys_Preft.SYSValue;
            newSys_Preft = objGeneral.GetSingleSYS_PREFT(1, "AA", "RESTRICTIONALERT");
            txtRestrictionAlert.Text = newSys_Preft.SYSValue;
        }
        protected void InitValue()
        {
            HttpContext.Current.Session["Grid"] = null;
            HttpContext.Current.Session["GridAgent"] = null;
            HttpContext.Current.Session["GridPayment"] = null;
            HttpContext.Current.Session["gvGB4"] = null;
            HttpContext.Current.Session["SelectChanged"] = 0;
            UIClass.SetComboStyle(ref cmbGRPID, UIClass.EnumDefineStyle.Opt);
            string temp = string.Empty;
            if (AdminSet.OperationGroup.IndexOf("AA,AAX") > -1)
                temp = string.Empty;
            else
            if (AdminSet.OperationGroup.IndexOf("AA") > -1)
                temp = "AA";
            else
            if (AdminSet.OperationGroup.IndexOf("AAX") > -1)
                temp = "AAX";
            lstOpt = objGeneral.GetAllCODEMASTERFilter(temp);
            cmbGRPID.Items.Clear();
            cmbGRPID.TextField = "CodeDesc";
            cmbGRPID.ValueField = "Code";
            cmbGRPID.DataSource = lstOpt;
            cmbGRPID.DataBind();
            cmbGRPID.DropDownWidth = 150;
            cmbGRPID.SelectedIndex = 0;

            UIClass.SetComboStyle(ref cmGRPID, UIClass.EnumDefineStyle.Opt);
            temp = string.Empty;
            if (AdminSet.OperationGroup.IndexOf("AA,AAX") > -1)
                temp = string.Empty;
            else
                if (AdminSet.OperationGroup.IndexOf("AA") > -1)
                temp = "AA";
            else
                    if (AdminSet.OperationGroup.IndexOf("AAX") > -1)
                temp = "AAX";
            lstOpt = objGeneral.GetAllCODEMASTERFilter(temp);
            cmGRPID.Items.Clear();
            cmGRPID.TextField = "CodeDesc";
            cmGRPID.ValueField = "Code";
            cmGRPID.DataSource = lstOpt;
            cmGRPID.DataBind();
            cmGRPID.DropDownWidth = 150;
            cmGRPID.SelectedIndex = 0;


            grid.DataBind();
            gvAgent.DataBind();
            gvPayment.DataBind();
            gvGB4.DataBind();
        }


        protected void LoadDisplay()
        {
            HttpContext.Current.Session["SelectChanged"] = 0;

            GridViewDataComboBoxColumn column = (gvGB4.Columns["OrgName"] as GridViewDataComboBoxColumn);
            DataTable dtOrgID = objGeneral.GetAllOrgID();
            column.PropertiesComboBox.DataSource = dtOrgID;

            column.PropertiesComboBox.ValueField = "OrgID";
            column.PropertiesComboBox.TextField = "OrgName";

            //GridViewDataComboBoxColumn columnCountry = (gvGB4.Columns["Origin"] as GridViewDataComboBoxColumn);

            //columnCountry.PropertiesComboBox.DataSource = dt;

            //columnCountry.PropertiesComboBox.ValueField = "DepartureStation";
            //columnCountry.PropertiesComboBox.TextField = "CustomState";


            gvGB4.DataBind();
        }

        protected void cmbOrigin_Init(object sender, EventArgs e)
        {
            try
            {
                SEAL.WEB.UI.CustomDropDownList cmbOrigin = sender as SEAL.WEB.UI.CustomDropDownList;
                if (cmbOrigin != null && cmbOrigin.Items.Count == 0)
                {
                    DataTable dt = new DataTable();
                    dt = objGeneral.GetLookUpCity("", Request.PhysicalApplicationPath);
                    if (dt == null || dt.Rows.Count <= 0)
                    {
                        dt = objGeneral.ReturnAllCityCustom("");
                    }
                    SEAL.WEB.UI.Control.SetComboCustomStyle(ref cmbOrigin, dt, "CustomState", "DepartureStation", "");

                }
                //if (hfOrigin.Value != null && hfOrigin.Value != "")
                //{
                //    cmbOrigin.SelectedValue = hfOrigin.Value;
                //    cmbOrigin.Enabled = false;
                //}
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                //sTraceLog(ex.ToString);
                log.Error(this, ex);
            }
        }


        protected void TabControl_ActiveTabChanged(object sender, TabControlEventArgs e)
        {
            SelectState();
            txtSysValue.IsValid = true;
            grid.DataBind();
        }

        protected void TabControl_ActiveTabChanging(object sender, TabControlEventArgs e)
        {
            if (HttpContext.Current.Session["SelectChanged"].ToString() != "1")
            {
                lblSYSKey.Text = string.Empty;
                lblWebID.Text = string.Empty;
                txtSysValue.Text = string.Empty;
                lblSettingDesc.Text = string.Empty;
                HttpContext.Current.Session["SelectChanged"] = 0;
            }
        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            dt = objGeneral.GetAllSettingFilter(SYSSet, cmbGRPID.SelectedItem.Value.ToString());
            (sender as ASPxGridView).DataSource = dt;
            HttpContext.Current.Session["Grid"] = dt;
        }

        protected void gvGB4_DataBinding(object sender, EventArgs e)
        {
            if (Session["gvGB4"] == null)
            {
                dt = objGeneral.GetPaxSettingAll();
            }
            else
            {
                dt = (DataTable)Session["gvGB4"];
            }

            if (dt == null)
            {
                dt = new DataTable();
                dt.Columns.Add("CountryCode", typeof(string));
                dt.Columns.Add("EffectiveDate", typeof(DateTime));
                dt.Columns.Add("ExpiryDate", typeof(DateTime));
                dt.Columns.Add("CountryName", typeof(string));
                dt.Columns.Add("Origin", typeof(string));
                dt.Columns.Add("OrgID", typeof(string));
                dt.Columns.Add("OrgName", typeof(string));
                dt.Columns.Add("AgentID", typeof(string));
                dt.Columns.Add("Username", typeof(string));
                dt.Columns.Add("NoofPax", typeof(int));
                dt.Columns.Add("Status", typeof(int));
            }
            (sender as ASPxGridView).DataSource = dt;
            HttpContext.Current.Session["gvGB4"] = dt;
        }

        protected void gvGB4_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {

            //if (e.Column.FieldName == "OrgName")
            //{

            //}
            //else if (e.Column.FieldName == "CountryName")
            //{

            //}

        }

        protected void gvGB4_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                if (e.Parameters == "Bind")
                {
                    dt = objGeneral.GetPaxSettingAll();
                    gvGB4.DataSource = dt;
                    gvGB4.DataBind();
                    HttpContext.Current.Session["gvGB4"] = dt;
                }
                else if (e.Parameters == "DeleteList")
                {
                    GetCheckBoxValuesGB4();
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
        }

        protected void gvGB4_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {

            string AgentID = string.Empty;
            DataTable dtName = new DataTable();
            DataTable dtsingleGB4Setting = new DataTable();
            try
            {
                GB4SETTING gB4SETTING = new GB4SETTING();
                gB4SETTING.AppID = 1;
                ASPxComboBox cmbAgentID = (ASPxComboBox)gvGB4.FindEditRowCellTemplateControl((GridViewDataColumn)gvGB4.Columns[3], "cmbAgentID");
                if (cmbAgentID != null)
                    AgentID = objGeneral.GetAgentIDbyUsername(cmbAgentID.Text);
                gB4SETTING.AgentID = AgentID;
                ASPxTextBox lblCountryCode = (ASPxTextBox)gvGB4.FindEditRowCellTemplateControl((GridViewDataColumn)gvGB4.Columns[4], "lblCountryCode");
                if (lblCountryCode != null && lblCountryCode.Text != "")
                {
                    dtName = objGeneral.GetCountryNameByCode(lblCountryCode.Text.ToString());
                    if (dtName != null && dtName.Rows.Count > 0)
                    {
                        gB4SETTING.CountryName = dtName.Rows[0]["Name"].ToString();
                        gB4SETTING.CountryCode = lblCountryCode.Text.ToString();
                    }
                }
                else
                {
                    gvGB4.JSProperties["cp_result"] = "Country Code can not be empty";
                    e.Cancel = true;
                    return;
                    //gvGB4.CancelEdit();
                }
                if (e.NewValues["OrgName"].ToString() != "")
                {
                    gB4SETTING.OrgID = e.NewValues["OrgName"].ToString();
                }
                else
                {
                    gvGB4.JSProperties["cp_result"] = "Organitation Name can not be empty";
                    e.Cancel = true;
                    return;
                    //gvGB4.CancelEdit();
                }
                SEAL.WEB.UI.CustomDropDownList cmbOrigin = (SEAL.WEB.UI.CustomDropDownList)gvGB4.FindEditRowCellTemplateControl((GridViewDataColumn)gvGB4.Columns[5], "cmbOrigin");
                if (cmbOrigin != null && cmbOrigin.SelectedValue != "0")
                {
                    gB4SETTING.Origin = cmbOrigin.SelectedValue;
                }
                else
                {
                    gvGB4.JSProperties["cp_result"] = "Origin can not be empty";
                    e.Cancel = true;
                    return;
                    //gvGB4.CancelEdit();
                }

                if (e.NewValues["NoofPax"] != null && Convert.ToUInt16(e.NewValues["NoofPax"]) != 0)
                {
                    gB4SETTING.NoofPax = Convert.ToUInt16(e.NewValues["NoofPax"]);
                }
                else
                {
                    gvGB4.JSProperties["cp_result"] = "No of Pax can not equal or less than zero";
                    e.Cancel = true;
                    return;
                    //gvGB4.CancelEdit();
                }
                ASPxCheckBox cbStatus = (ASPxCheckBox)gvGB4.FindEditRowCellTemplateControl((GridViewDataColumn)gvGB4.Columns[7], "cbStatus");
                if (cbStatus != null)
                    if (cbStatus.Checked)
                    {
                        gB4SETTING.status = 1;
                    }
                    else
                    {
                        gB4SETTING.status = 0;
                    }
                gB4SETTING.LastSyncBy = AdminSet.AdminID;
                gB4SETTING.SyncCreate = DateTime.Now;
                gB4SETTING.SyncLastUpd = DateTime.Now;
                ASPxDateEdit daStart = (ASPxDateEdit)gvGB4.FindEditRowCellTemplateControl((GridViewDataColumn)gvGB4.Columns[8], "daStart");
                if (daStart != null && Convert.ToDateTime(daStart.Value) != DateTime.MinValue)
                {
                    gB4SETTING.EffectiveDate = Convert.ToDateTime(daStart.Value);
                }
                else
                {
                    gvGB4.JSProperties["cp_result"] = "Kindly fill in the correct date";
                    e.Cancel = true;
                }
                ASPxDateEdit daEnd = (ASPxDateEdit)gvGB4.FindEditRowCellTemplateControl((GridViewDataColumn)gvGB4.Columns[9], "daEnd");
                if (daEnd != null && Convert.ToDateTime(daEnd.Value) != DateTime.MinValue && Convert.ToDateTime(daEnd.Value) < DateTime.Now)
                {
                    gB4SETTING.ExpiryDate = Convert.ToDateTime(daEnd.Value);
                }
                else
                {
                    gvGB4.JSProperties["cp_result"] = "Effective End Date must be equal or greater than today";
                    e.Cancel = true;
                    return;
                }

                dtsingleGB4Setting = objGeneral.GetSinglePaxSetting(gB4SETTING.CountryCode, gB4SETTING.Origin, gB4SETTING.OrgID, gB4SETTING.AgentID);
                if (dtsingleGB4Setting != null && dtsingleGB4Setting.Rows.Count > 0)
                {
                    gvGB4.JSProperties["cp_result"] = "Record already exist";
                    e.Cancel = true;
                    gvGB4.CancelEdit();
                    return;
                }
                else
                {
                    if (objGeneral.SaveGB4SETTING(gB4SETTING, GeneralControl.SaveType.Insert))
                    {
                        gvGB4.JSProperties["cp_result"] = "Record has been Inserted Successfully";
                        e.Cancel = true;
                        gvGB4.CancelEdit();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }

        }

        protected void gvGB4_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            string AgentID = string.Empty;
            DataTable dtName = new DataTable();
            GB4SETTING gB4SETTINGinfo = new GB4SETTING();
            DataTable dtsingleGB4Setting = new DataTable();
            try
            {
                ASPxTextBox lblCountryCode = (ASPxTextBox)gvGB4.FindEditRowCellTemplateControl((GridViewDataColumn)gvGB4.Columns[4], "lblCountryCode");
                if (lblCountryCode != null)
                {
                    gB4SETTINGinfo.CountryCode = lblCountryCode.Text.ToString();
                }
                if (hfOrigin.Value != "")
                    gB4SETTINGinfo.Origin = hfOrigin.Value;
                gB4SETTINGinfo.OrgID = objGeneral.GetOrgIDCodeByOrgName(e.NewValues["OrgName"].ToString());
                ASPxComboBox cmbAgentID = (ASPxComboBox)gvGB4.FindEditRowCellTemplateControl((GridViewDataColumn)gvGB4.Columns[3], "cmbAgentID");
                if (hfUsername.Value != "")
                    gB4SETTINGinfo.AgentID = AgentID = objGeneral.GetAgentIDbyUsername(hfUsername.Value);
                dtsingleGB4Setting = objGeneral.GetSinglePaxSetting(gB4SETTINGinfo.CountryCode, gB4SETTINGinfo.Origin, gB4SETTINGinfo.OrgID, gB4SETTINGinfo.AgentID);
                if (dtsingleGB4Setting != null && dtsingleGB4Setting.Rows.Count > 0)
                {
                    GB4SETTING gB4SETTING = new GB4SETTING();
                    gB4SETTING.AppID = Convert.ToInt16(dtsingleGB4Setting.Rows[0]["AppID"]);
                    cmbAgentID = (ASPxComboBox)gvGB4.FindEditRowCellTemplateControl((GridViewDataColumn)gvGB4.Columns[3], "cmbAgentID");
                    if (cmbAgentID != null)
                    {
                        AgentID = objGeneral.GetAgentIDbyUsername(cmbAgentID.Text);
                        gB4SETTING.AgentID = AgentID;
                    }
                    gB4SETTING.CountryCode = dtsingleGB4Setting.Rows[0]["CountryCode"].ToString();
                    gB4SETTING.CountryName = dtsingleGB4Setting.Rows[0]["CountryName"].ToString();
                    gB4SETTING.OrgID = dtsingleGB4Setting.Rows[0]["OrgID"].ToString();
                    //SEAL.WEB.UI.CustomDropDownList cmbOrigin = (SEAL.WEB.UI.CustomDropDownList)gvGB4.FindEditRowCellTemplateControl((GridViewDataColumn)gvGB4.Columns[4], "cmbOrigin");
                    //if (cmbOrigin != null)
                    //gB4SETTING.Origin = gB4SETTINGinfo.Origin;
                    SEAL.WEB.UI.CustomDropDownList cmbOrigin = (SEAL.WEB.UI.CustomDropDownList)gvGB4.FindEditRowCellTemplateControl((GridViewDataColumn)gvGB4.Columns[5], "cmbOrigin");
                    if (cmbOrigin != null && cmbOrigin.SelectedValue != "0")
                    {
                        gB4SETTING.Origin = cmbOrigin.SelectedValue;
                    }
                    else
                    {
                        gvGB4.JSProperties["cp_result"] = "Origin can not be empty";
                        e.Cancel = true;
                        return;
                        //gvGB4.CancelEdit();
                    }
                    //gB4SETTING.Origin = dtsingleGB4Setting.Rows[0]["Origin"].ToString();
                    //gB4SETTING.NoofPax = Convert.ToUInt16(e.NewValues["NoofPax"]);
                    if (e.NewValues["NoofPax"] != null && Convert.ToUInt16(e.NewValues["NoofPax"]) != 0)
                    {
                        gB4SETTING.NoofPax = Convert.ToUInt16(e.NewValues["NoofPax"]);
                    }
                    else
                    {
                        gvGB4.JSProperties["cp_result"] = "No of Pax can not equal or less than zero";
                        e.Cancel = true;
                        return;
                        //gvGB4.CancelEdit();
                    }
                    ASPxCheckBox cbStatus = (ASPxCheckBox)gvGB4.FindEditRowCellTemplateControl((GridViewDataColumn)gvGB4.Columns[7], "cbStatus");
                    if (cbStatus != null)
                        if (cbStatus.Checked)
                        {
                            gB4SETTING.status = 1;
                        }
                        else
                        {
                            gB4SETTING.status = 0;
                        }
                    gB4SETTING.LastSyncBy = AdminSet.AdminID;
                    gB4SETTING.SyncCreate = Convert.ToDateTime(dtsingleGB4Setting.Rows[0]["SyncCreate"]);
                    gB4SETTING.SyncLastUpd = DateTime.Now;
                    ASPxDateEdit daStart = (ASPxDateEdit)gvGB4.FindEditRowCellTemplateControl((GridViewDataColumn)gvGB4.Columns[8], "daStart");
                    if (daStart != null && Convert.ToDateTime(daStart.Value) != DateTime.MinValue)
                    {
                        gB4SETTING.EffectiveDate = Convert.ToDateTime(daStart.Value);
                    }
                    else
                    {
                        gvGB4.JSProperties["cp_result"] = "Kindly fill in the correct date";
                        e.Cancel = true;
                        return;
                    }
                    ASPxDateEdit daEnd = (ASPxDateEdit)gvGB4.FindEditRowCellTemplateControl((GridViewDataColumn)gvGB4.Columns[9], "daEnd");
                    if (daEnd != null && Convert.ToDateTime(daEnd.Value) != DateTime.MinValue && Convert.ToDateTime(daEnd.Value) < DateTime.Now)
                    {
                        gB4SETTING.ExpiryDate = Convert.ToDateTime(daEnd.Value);
                    }
                    else
                    {
                        gvGB4.JSProperties["cp_result"] = "Effective End Date must be equal or greater than today";
                        e.Cancel = true;
                        return;
                    }

                    if (objGeneral.SaveGB4SETTING(gB4SETTING, GeneralControl.SaveType.Update, gB4SETTINGinfo))
                    {
                        gvGB4.JSProperties["cp_result"] = "Record has been Updated Successfully";
                        e.Cancel = true;
                        grid.CancelEdit();
                        return;
                    }
                }



            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
        }

        protected void GetCheckBoxValuesGB4()
        {
            DataTable dtsingleGB4Setting = new DataTable();
            DataTable dtCode = new DataTable();
            List<GB4SETTING> lgB4Setting = new List<GB4SETTING>();
            try
            {
                List<string> fieldNames = new List<string>();
                foreach (GridViewColumn column in gvGB4.Columns)
                    if (column is GridViewDataColumn)
                        fieldNames.Add(((GridViewDataColumn)column).FieldName);
                selectedValues = gvGB4.GetSelectedFieldValues(fieldNames.ToArray());
                if (selectedValues.Count > 0)
                {
                    foreach (object[] item in selectedValues)
                    {
                        GB4SETTING gB4SETTING = new GB4SETTING();
                        gB4SETTING.OrgID = objGeneral.GetOrgIDCodeByOrgName(item[0].ToString());

                        //dtCode = objGeneral.GetCountryCodeByName(item[2].ToString());
                        //if (dtCode != null && dtCode.Rows.Count > 0)
                        //{
                        gB4SETTING.CountryCode = item[2].ToString();
                        //}

                        gB4SETTING.Origin = item[3].ToString();
                        lgB4Setting.Add(gB4SETTING);

                    }
                    if (lgB4Setting.Count > 0)
                    {
                        if (objGeneral.DeleteGB4SETTING(lgB4Setting))
                        {
                            gvGB4.JSProperties["cp_result"] = "Record(s) has been Deleted Successfully";
                        }
                    }


                }

            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this, ex);
            }
            finally
            {

            }

        }

        protected void gvGB4_OnBeforeGetCallbackResult(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            if (grid.IsNewRowEditing)
                grid.SettingsText.CommandUpdate = "Add";
            else
                grid.SettingsText.CommandUpdate = "Save";
        }

        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            HttpContext.Current.Session["selectedDataSource"] = Int32.Parse(e.Parameters);
            grid.Selection.UnselectAll();
            grid.Selection.SelectRow(0);
            grid.Columns.Clear();
            grid.AutoGenerateColumns = true;
            grid.KeyFieldName = String.Empty;
            grid.DataBind();
        }
        protected void gvPayment_DataBinding(object sender, EventArgs e)
        {
            dt = objPayment.GetAllScheme(cmGRPID.SelectedItem.Value.ToString(), "12345");
            (sender as ASPxGridView).DataSource = dt;
            HttpContext.Current.Session["GridPayment"] = dt;
        }

        protected void gvPayment_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            HttpContext.Current.Session["selectedDataSource"] = Int32.Parse(e.Parameters);
            gvPayment.Selection.UnselectAll();
            gvPayment.Selection.SelectRow(0);
            gvPayment.Columns.Clear();
            gvPayment.AutoGenerateColumns = true;
            gvPayment.KeyFieldName = String.Empty;
            gvPayment.DataBind();
        }
        protected void gvAgent_DataBinding(object sender, EventArgs e)
        {
            dt = objAgent.GetAllAgentCategory();
            (sender as ASPxGridView).DataSource = dt;
            HttpContext.Current.Session["GridAgent"] = dt;
        }

        protected void gvAgent_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            HttpContext.Current.Session["selectedDataSource"] = Int32.Parse(e.Parameters);
            gvAgent.Selection.UnselectAll();
            gvAgent.Selection.SelectRow(0);
            gvAgent.Columns.Clear();
            gvAgent.AutoGenerateColumns = true;
            gvAgent.KeyFieldName = String.Empty;
            gvAgent.DataBind();
        }

        protected void cmbGRPID_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            dt = objGeneral.GetAllSettingFilter(SYSSet, cmbGRPID.SelectedItem.Value.ToString());
            grid.DataSource = dt;
            grid.DataBind();
            HttpContext.Current.Session["Grid"] = dt;
        }

        protected void cmGRPID_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            dt = objPayment.GetAllScheme(cmGRPID.SelectedItem.Value.ToString(), "12345");
            gvPayment.DataSource = dt;
            gvPayment.DataBind();
            HttpContext.Current.Session["GridPayment"] = dt;
        }

        protected void grid_SelectionChanged(object sender, EventArgs e)
        {
            dt = (DataTable)HttpContext.Current.Session["Grid"];
            string SYSKey = dt.Rows[GetSelectedRowOnTheCurrentPage()]["SYSKey"].ToString();
            short AppID = Convert.ToInt16(dt.Rows[GetSelectedRowOnTheCurrentPage()]["AppID"].ToString());
            string GRPID = dt.Rows[GetSelectedRowOnTheCurrentPage()]["GRPID"].ToString();
            SettingVal = objGeneral.GetSingleSYS_PREFT(AppID, GRPID, SYSKey);
            lblSYSKey.Text = SettingVal.SYSKey;
            try
            {
                Convert.ToInt16(SettingVal.SYSValue);
                txtSysValue.ValidationSettings.RegularExpression.ValidationExpression = "^([\\d*]{1,50})$";
            }
            catch
            {
                txtSysValue.ValidationSettings.RegularExpression.ValidationExpression = "^([1-zA-Z0-1@.\\s]{1,50})$";
            }
            lblWebID.Text = cmbGRPID.Text;
            lblSettingDesc.Text = SettingVal.SYSDesc;
            HttpContext.Current.Session["Setting"] = SettingVal;
            HttpContext.Current.Session["SelectChanged"] = "1";
        }

        protected void gvAgent_SelectionChanged(object sender, EventArgs e)
        {
            dt = (DataTable)HttpContext.Current.Session["GridAgent"];
            string AgentCatgID = dt.Rows[GetAgentSelectedRowOnTheCurrentPage()]["AgentCatgID"].ToString();

            SettingAgent = objAgent.GetAgentCategory(AgentCatgID);
            lblAgentCatgDesc.Text = SettingAgent.AgentCatgDesc;
            txtCounterTimer.Text = SettingAgent.CounterTimer.ToString();
            txtBlacklistDuration.Text = SettingAgent.BlacklistDuration.ToString();
            txtMaxEnquiry.Text = SettingAgent.MaxEnquiry.ToString();
            txtMaxSuspend.Text = SettingAgent.MaxSuspend.ToString();
            txtSuspendDuration.Text = SettingAgent.SuspendDuration.ToString();

            HttpContext.Current.Session["SettingAgent"] = SettingAgent;
            HttpContext.Current.Session["SelectAgentChanged"] = "1";
        }

        protected void gvPayment_SelectionChanged(object sender, EventArgs e)
        {
            dt = (DataTable)HttpContext.Current.Session["GridPayment"];
            string SchemeCode = dt.Rows[GetPaymentSelectedRowOnTheCurrentPage()]["SchemeCode"].ToString();

            SettingPayment = objPayment.GetSinglePAYSCHEME(SchemeCode, "AA", "12345");
            txtCode.Text = SettingPayment.SchemeCode.ToString();
            txtDuration.Text = SettingPayment.Duration.ToString();
            txtMinDuration.Text = SettingPayment.MinDuration.ToString();
            cmbType.Text = SettingPayment.PaymentType.ToString();
            txtAttempt1.Text = SettingPayment.Attempt_1.ToString();
            cmbCode1.Text = SettingPayment.Code_1.ToString();
            txtPerc1.Text = SettingPayment.Percentage_1.ToString();
            txtAttempt2.Text = SettingPayment.Attempt_2.ToString();
            cmbCode2.Text = SettingPayment.Code_2.ToString();
            txtPerc2.Text = SettingPayment.Percentage_2.ToString();
            txtAttempt3.Text = SettingPayment.Attempt_3.ToString();
            cmbCode3.Text = SettingPayment.Code_3.ToString();
            txtPerc3.Text = SettingPayment.Percentage_3.ToString();
            //txtDesc.Text = SettingPayment.Description.ToString();

            lblID.Text = cmGRPID.Text;
            HttpContext.Current.Session["SettingPayment"] = SettingPayment;
            HttpContext.Current.Session["SelectPaymentChanged"] = "1";
        }
        protected void SaveData()
        {
            SettingVal = (Settings)HttpContext.Current.Session["Setting"];
            SettingVal.SYSValue = txtSysValue.Text;
            SettingVal.LastSyncBy = AdminSet.AdminID;
            objGeneral.SaveSYS_PREFT(SettingVal, GeneralControl.SaveType.Update);
            dt = objGeneral.GetAllSettingFilter(SYSSet, cmbGRPID.SelectedItem.Value.ToString());
            grid.DataSource = dt;
            grid.DataBind();
            HttpContext.Current.Session["Grid"] = dt;
        }

        protected void SaveDataAgent()
        {
            SettingAgent = (AgentCategory)HttpContext.Current.Session["SettingAgent"];
            SettingAgent.CounterTimer = Convert.ToInt32(txtCounterTimer.Text);
            SettingAgent.BlacklistDuration = Convert.ToInt32(txtBlacklistDuration.Text);
            SettingAgent.MaxEnquiry = Convert.ToInt32(txtMaxEnquiry.Text);
            SettingAgent.MaxSuspend = Convert.ToInt32(txtMaxSuspend.Text);
            SettingAgent.SuspendDuration = Convert.ToInt32(txtSuspendDuration.Text);
            SettingAgent.LastSyncBy = AdminSet.AdminID;
            objAgent.SaveAgentCategory(SettingAgent, AgentControl.SaveType.Update);
            dt = objAgent.GetAllAgentCategory();
            gvAgent.DataSource = dt;
            gvAgent.DataBind();
            HttpContext.Current.Session["GridAgent"] = dt;
        }

        protected void SaveDataPayment()
        {
            SettingPayment = (PaymentInfo)HttpContext.Current.Session["SettingPayment"];
            SettingPayment.Duration = Convert.ToInt32(txtDuration.Text);
            SettingPayment.MinDuration = Convert.ToInt32(txtMinDuration.Text);
            SettingPayment.PaymentType = cmbType.Text;
            SettingPayment.Attempt_1 = Convert.ToInt32(txtAttempt1.Text);
            SettingPayment.Code_1 = cmbCode1.Text;
            SettingPayment.Percentage_1 = Convert.ToInt32(txtPerc1.Text);
            SettingPayment.Attempt_2 = Convert.ToInt32(txtAttempt2.Text);
            SettingPayment.Code_2 = cmbCode2.Text;
            SettingPayment.Percentage_2 = Convert.ToInt32(txtPerc2.Text);
            SettingPayment.Attempt_3 = Convert.ToInt32(txtAttempt3.Text);
            SettingPayment.Code_3 = cmbCode3.Text;
            SettingPayment.Percentage_3 = Convert.ToInt32(txtPerc3.Text);
            //SettingPayment.Description = txtDesc.Text;
            SettingPayment.LastSyncBy = AdminSet.AdminID;
            objPayment.SavePayment(SettingPayment, cmbGRPID.SelectedItem.Value.ToString(), PaymentControl.SaveType.Update, "12345");
            dt = objPayment.GetAllScheme(cmbGRPID.SelectedItem.Value.ToString(), "12345");
            gvPayment.DataSource = dt;
            gvPayment.DataBind();
            HttpContext.Current.Session["GridPayment"] = dt;
        }

        protected void SaveDataRestriction()
        {
            lstOpt = objGeneral.GetAllCODEMASTERFilterCode("RST");
            for (int i = 0; i < lstOpt.Count; i++)
            {
                code = new CODEMASTER();
                code = lstOpt[i];
                if (code.Code == "BOOKFROM")
                {
                    code.CodeDesc = txtStartDate.Date.ToString("yyyy-MM-dd");
                    code.CodeSeq = code.CodeSeq;
                    code.SysCode = code.SysCode;
                    code.SyncCreate = code.SyncCreate;
                    code.SyncLastUpd = DateTime.Now;
                    code.IsHost = code.IsHost;
                    objGeneral.SaveAllCodeMaster(code, GeneralControl.SaveType.Update);
                }
                else if (code.Code == "BOOKTO")
                {
                    code.CodeDesc = txtEndDate.Date.ToString("yyyy-MM-dd");
                    code.CodeSeq = code.CodeSeq;
                    code.SysCode = code.SysCode;
                    code.SyncCreate = code.SyncCreate;
                    code.SyncLastUpd = DateTime.Now;
                    code.IsHost = code.IsHost;
                    objGeneral.SaveAllCodeMaster(code, GeneralControl.SaveType.Update);
                }
                else if (code.Code == "TRAFROM")
                {
                    code.CodeDesc = txtStartDateTravel.Date.ToString("yyyy-MM-dd");
                    code.CodeSeq = code.CodeSeq;
                    code.SysCode = code.SysCode;
                    code.SyncCreate = code.SyncCreate;
                    code.SyncLastUpd = DateTime.Now;
                    code.IsHost = code.IsHost;
                    objGeneral.SaveAllCodeMaster(code, GeneralControl.SaveType.Update);
                }
                else if (code.Code == "TRATO")
                {
                    code.CodeDesc = txtEndDateTravel.Date.ToString("yyyy-MM-dd");
                    code.CodeSeq = code.CodeSeq;
                    code.SysCode = code.SysCode;
                    code.SyncCreate = code.SyncCreate;
                    code.SyncLastUpd = DateTime.Now;
                    code.IsHost = code.IsHost;
                    objGeneral.SaveAllCodeMaster(code, GeneralControl.SaveType.Update);
                }

                else if (code.Code == "IND")
                {
                    if (chkRestriction.Checked == true)
                    {
                        code.CodeDesc = "1";
                        code.CodeSeq = code.CodeSeq;
                        code.SysCode = code.SysCode;
                        code.SyncCreate = code.SyncCreate;
                        code.SyncLastUpd = DateTime.Now;
                        code.IsHost = code.IsHost;
                        objGeneral.SaveAllCodeMaster(code, GeneralControl.SaveType.Update);
                    }
                    else
                    {
                        code.CodeDesc = "0";
                        code.CodeSeq = code.CodeSeq;
                        code.SysCode = code.SysCode;
                        code.SyncCreate = code.SyncCreate;
                        code.SyncLastUpd = DateTime.Now;
                        code.IsHost = code.IsHost;
                        objGeneral.SaveAllCodeMaster(code, GeneralControl.SaveType.Update);
                    }
                }
            }

            newSys_Preft = objGeneral.GetSingleSYS_PREFT(1, "AA", "RESTRICTIONNOTE");
            newSys_Preft.AppID = newSys_Preft.AppID;
            newSys_Preft.GRPID = newSys_Preft.GRPID;
            newSys_Preft.SYSKey = newSys_Preft.SYSKey;
            if (txtRestrictionNote.Text.ToString() != null)
            {
                newSys_Preft.SYSValue = txtRestrictionNote.Text;
                newSys_Preft.SYSValueEx = txtRestrictionNote.Text;
            }
            else
            {
                newSys_Preft.SYSValue = "";
                newSys_Preft.SYSValueEx = "";
            }
            newSys_Preft.SYSSet = newSys_Preft.SYSSet;
            newSys_Preft.SyncCreate = newSys_Preft.SyncCreate;
            newSys_Preft.SyncLastUpd = DateTime.Now;
            newSys_Preft.IsHost = newSys_Preft.IsHost;
            objGeneral.SaveSYS_PREFTrestrict(newSys_Preft, GeneralControl.SaveType.Update);

            newSys_Preft = objGeneral.GetSingleSYS_PREFT(1, "AA", "RESTRICTIONALERT");
            newSys_Preft.AppID = newSys_Preft.AppID;
            newSys_Preft.GRPID = newSys_Preft.GRPID;
            newSys_Preft.SYSKey = newSys_Preft.SYSKey;
            if (txtRestrictionAlert.Text.ToString() != null)
            {
                newSys_Preft.SYSValue = txtRestrictionAlert.Text;
                newSys_Preft.SYSValueEx = txtRestrictionAlert.Text;
            }
            else
            {
                newSys_Preft.SYSValue = "";
                newSys_Preft.SYSValueEx = "";
            }
            newSys_Preft.SYSSet = newSys_Preft.SYSSet;
            newSys_Preft.SyncCreate = newSys_Preft.SyncCreate;
            newSys_Preft.SyncLastUpd = DateTime.Now;
            newSys_Preft.IsHost = newSys_Preft.IsHost;
            objGeneral.SaveSYS_PREFTrestrict(newSys_Preft, GeneralControl.SaveType.Update);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (lblSYSKey.Text == string.Empty)
            {
                txtSysValue.IsValid = false;
                txtSysValue.ErrorText = "Please choose data first";
            }
            if (ASPxEdit.AreEditorsValid(RPanelData))
            {
                SaveData();
            }
        }
        protected void btnSaveAgent_Click(object sender, EventArgs e)
        {

            if (ASPxEdit.AreEditorsValid(RpanelAgent))
            {
                SaveDataAgent();
            }
        }
        protected void btnSavePayment_Click(object sender, EventArgs e)
        {

            if (ASPxEdit.AreEditorsValid(RpanelPayment))
            {
                SaveDataPayment();
            }
        }
        protected void btnSaveRestriction_Click(object sender, EventArgs e)
        {
            if (ASPxEdit.AreEditorsValid(RpanelRestriction))
            {
                SaveDataRestriction();
            }
        }
        protected void ClearSelectionOnOtherPages()
        {
            if (grid.Selection.Count <= 1) return;
            int curPageSelection = GetSelectedRowOnTheCurrentPage();
            grid.Selection.UnselectAll();
            grid.Selection.SelectRow(curPageSelection);
        }

        protected void ClearAgentSelectionOnOtherPages()
        {
            if (gvAgent.Selection.Count <= 1) return;
            int curPageSelection = GetAgentSelectedRowOnTheCurrentPage();
            gvAgent.Selection.UnselectAll();
            gvAgent.Selection.SelectRow(curPageSelection);
        }

        protected void ClearPaymentSelectionOnOtherPages()
        {
            if (gvPayment.Selection.Count <= 1) return;
            int curPageSelection = GetPaymentSelectedRowOnTheCurrentPage();
            gvPayment.Selection.UnselectAll();
            gvPayment.Selection.SelectRow(curPageSelection);
        }

        public int GetAgentSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = gvAgent.PageIndex * gvAgent.SettingsPager.PageSize;
            for (int i = 0; i < gvAgent.VisibleRowCount; i++)
            {
                if (gvAgent.Selection.IsRowSelected(startIndexOnPage))
                {
                    return startIndexOnPage;
                }

                startIndexOnPage++; // increment
            }
            return -1;
        }
        public int GetPaymentSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = gvPayment.PageIndex * gvPayment.SettingsPager.PageSize;
            for (int i = 0; i < gvPayment.VisibleRowCount; i++)
            {
                if (gvPayment.Selection.IsRowSelected(startIndexOnPage))
                {
                    return startIndexOnPage;
                }

                startIndexOnPage++; // increment
            }
            return -1;
        }
        public int GetSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = grid.PageIndex * grid.SettingsPager.PageSize;
            for (int i = 0; i < grid.VisibleRowCount; i++)
            {
                if (grid.Selection.IsRowSelected(startIndexOnPage))
                {
                    return startIndexOnPage;
                }

                startIndexOnPage++; // increment
            }
            return -1;
        }

        [WebMethod]
        public static string OrgIDChanged(string OrgID)
        {
            StringWriter builder = new StringWriter();
            DataTable dtAgentID = new DataTable();
            GeneralControl objGeneral = new GeneralControl();
            ABS.Navitaire.APIAgent apiAgent = new ABS.Navitaire.APIAgent();
            dtAgentID = objGeneral.GetAllAgentbyOrgID(OrgID);

            string psDomain = "";
            string psName = "";
            string psPwd = "";
            string CountryCode = "";

            ABS.Navitaire.APIBooking apiNavitaire = new ABS.Navitaire.APIBooking("");
            psDomain = ConfigurationManager.AppSettings["signature_domain"].ToString();
            psName = ConfigurationManager.AppSettings["signature_username"].ToString();
            psPwd = ConfigurationManager.AppSettings["signature_password"].ToString();

            string signature = "";// apiNavitaire.AgentLogon("Public", psDomain, psName, psPwd);
            signature = apiNavitaire.AgentLogon("Public", psDomain, psName, psPwd);
            //string signature = apiNavitaire.AgentLogon("SkyAgent", psDomain, psName, txtPassword.Text);
            if (signature != "")
            {
                GetOrganizationResponse org = apiAgent.GetOrganization("", signature, "", OrgID);
                if (org != null && org.Organization != null && (org.Organization.Address.CountryCode != null || org.Organization.Address.CountryCode != ""))
                {
                    CountryCode = org.Organization.Address.CountryCode;
                }
            }

            if (dtAgentID != null && dtAgentID.Rows.Count > 0)
            {
                builder.WriteLine("[");

                for (int i = 0; i < dtAgentID.Rows.Count; i++)
                {
                    builder.WriteLine("{\"CountryCode\":\"" + CountryCode + "\",");
                    builder.WriteLine("\"optionDisplay\":\"" + Convert.ToString(dtAgentID.Rows[i]["Username"]) + "\",");
                    builder.WriteLine("\"optionValue\":\"" + Convert.ToString(dtAgentID.Rows[i]["AgentID"]) + "\"},");
                }
            }
            else if (objGeneral.GetOrgIDCodeByOrgName(OrgID) != "")
            {
                dtAgentID = objGeneral.GetAllAgentbyOrgID(objGeneral.GetOrgIDCodeByOrgName(OrgID));

                if (dtAgentID != null && dtAgentID.Rows.Count > 0)
                {
                    builder.WriteLine("[");
                    for (int i = 0; i < dtAgentID.Rows.Count; i++)
                    {
                        builder.WriteLine("{\"optionDisplay\":\"" + Convert.ToString(dtAgentID.Rows[i]["Username"]) + "\",");
                        builder.WriteLine("\"optionValue\":\"" + Convert.ToString(dtAgentID.Rows[i]["AgentID"]) + "\"},");
                    }
                }
                else
                {
                    builder.WriteLine("[");
                    builder.WriteLine("{\"optionDisplay\":,");
                    builder.WriteLine("\"optionValue\":},");
                }
            }
            else
            {
                builder.WriteLine("[");
                builder.WriteLine("{\"optionDisplay\":,");
                builder.WriteLine("\"optionValue\":},");
            }


            String returnjson = builder.ToString().Substring(0, builder.ToString().Length - 3);
            returnjson = returnjson + "]";
            return returnjson.Replace(System.Environment.NewLine, "").Replace(System.Environment.NewLine, "");
        }


        [WebMethod]
        public static string OnCountryNameChanged(string Country)
        {
            StringWriter builder = new StringWriter();
            DataTable dtAgentID = new DataTable();
            GeneralControl objGeneral = new GeneralControl();
            dtAgentID = objGeneral.GetLookUpCitybyCountry(Country, HttpContext.Current.Request.PhysicalApplicationPath);
            DataView view = new DataView(dtAgentID);
            DataTable distinctValues = view.ToTable(true, "DepartureStation");
            if (distinctValues != null && distinctValues.Rows.Count > 0)
            {
                builder.WriteLine("[");
                for (int i = 0; i < distinctValues.Rows.Count; i++)
                {
                    builder.WriteLine("{\"optionDisplay\":\"" + Convert.ToString(distinctValues.Rows[i]["DepartureStation"]) + "\",");
                    builder.WriteLine("\"optionValue\":\"" + Convert.ToString(distinctValues.Rows[i]["DepartureStation"]) + "\"},");
                }
            }
            else
            {
                builder.WriteLine("{\"optionDisplay\":,");
                builder.WriteLine("\"optionValue\":},");
            }


            String returnjson = builder.ToString().Substring(0, builder.ToString().Length - 3);
            returnjson = returnjson + "]";
            return returnjson.Replace(System.Environment.NewLine, "").Replace(System.Environment.NewLine, "");
        }
    }


}