using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using DevExpress.Web;

//using log4net;
using SEAL.Data;

using ABS.Logic.GroupBooking.Agent;
using ABS.Logic.GroupBooking.Booking;
using ABS.Logic.GroupBooking;
using ABS.Logic.Shared;
using SEAL.WEB;
using ABS.GBS.Log;

namespace GroupBooking.Web.admin
{
    public partial class adminroutesetting : System.Web.UI.Page
    {
        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogControl log = new LogControl();
        SystemLog SystemLog = new SystemLog();

        //        Settings SettingVal = new Settings();
        GeneralControl objGeneral = new GeneralControl();
        SectorSuspend SettingFlight = new SectorSuspend();
        BookingControl objFlight = new BookingControl();
        List<CODEMASTER> lstOpt = new List<CODEMASTER>();
        AdminSet AdminSet;

        //int SYSSet = 0;
        DataTable dt = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {

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
            }
            else
            {
                // LoadDisplay();

            }
            ClearFlightSelectionOnOtherPages();
        }

        protected void chkFilter_CheckedChanged(object sender, EventArgs e)
        {
            dt = objFlight.GetAllSECTORSUSPENDFilter(cmbGRPID.SelectedItem.Value.ToString(), Convert.ToByte(chkFilterActive.Checked));
            gvFlight.DataSource = dt;
            HttpContext.Current.Session["GridFlight"] = dt;
            dt = null;
        }

        protected void InitValue()
        {
            HttpContext.Current.Session["Grid"] = null;
            HttpContext.Current.Session["GridFlight"] = null;
            HttpContext.Current.Session["SelectChanged"] = 0;
            UIClass.SetComboStyle(ref cmbGRPID, UIClass.EnumDefineStyle.Opt);
            UIClass.SetComboStyle(ref cmbOptGroup, UIClass.EnumDefineStyle.Opt);
            UIClass.SetComboStyle(ref cmbAgentGroup, UIClass.EnumDefineStyle.AgentCategory);
            DataTable dt = new DataTable();
            dt = objGeneral.ReturnAllCityCustom("");
            SEAL.WEB.UI.Control.SetComboCustomStyle(ref ddlOrigin, dt, "CustomState", "CityCode", "Select City");
            txtStartDate.Value = DateTime.Now;
            txtEndDate.Value = DateTime.Now.AddDays(1);
            string temp = string.Empty;
            if (AdminSet.OperationGroup.IndexOf("AA,AX") > -1)
                temp = string.Empty;
            else
                if (AdminSet.OperationGroup.IndexOf("AA") > -1)
                    temp = "AA";
                else
                    if (AdminSet.OperationGroup.IndexOf("AX") > -1)
                        temp = "AX";
            lstOpt = objGeneral.GetAllCODEMASTERFilter(temp);
            cmbGRPID.Items.Clear();
            cmbGRPID.TextField = "CodeDesc";
            cmbGRPID.ValueField = "Code";
            cmbGRPID.DataSource = lstOpt;
            cmbGRPID.DataBind();
            cmbGRPID.DropDownWidth = 150;
            cmbGRPID.SelectedIndex = 0;
            cmbOptGroup.Items.Clear();
            cmbOptGroup.TextField = "CodeDesc";
            cmbOptGroup.ValueField = "Code";
            cmbOptGroup.DataSource = lstOpt;
            cmbOptGroup.DataBind();
            cmbOptGroup.DropDownWidth = 150;
            cmbOptGroup.SelectedIndex = 0;
            gvFlight.DataBind();
            dt = null;
        }

        protected void gvFlight_DataBinding(object sender, EventArgs e)
        {
            dt = objFlight.GetAllSECTORSUSPENDFilter(cmbGRPID.SelectedItem.Value.ToString(), Convert.ToByte(chkFilterActive.Checked));
            (sender as ASPxGridView).DataSource = dt;
            HttpContext.Current.Session["GridFlight"] = dt;
            dt = null;
        }

        protected void gvFlight_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            HttpContext.Current.Session["selectedDataSource"] = Int32.Parse(e.Parameters);
            gvFlight.Selection.UnselectAll();
            gvFlight.Selection.SelectRow(0);
            gvFlight.Columns.Clear();
            gvFlight.AutoGenerateColumns = true;
            gvFlight.KeyFieldName = String.Empty;
            gvFlight.DataBind();
        }

        protected void cmbGRPID_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            dt = objFlight.GetAllSECTORSUSPENDFilter(cmbGRPID.SelectedItem.Value.ToString(), Convert.ToByte(chkFilterActive.Checked)); gvFlight.DataSource = dt;
            gvFlight.DataBind();
            HttpContext.Current.Session["Grid"] = dt;
            dt = null;
        }

        protected void gvFlight_SelectionChanged(object sender, EventArgs e)
        {
            dt = (DataTable)HttpContext.Current.Session["GridFlight"];
            if (gvFlight.Selection.Count > 0)
            {
                string SectorID = dt.Rows[GetFlightSelectedRowOnTheCurrentPage()]["SectorSuspendID"].ToString();
                SettingFlight = objFlight.GetSingleSECTORSUSPEND(SectorID);
                cmbOptGroup.Items.FindByValue(SettingFlight.CarrierCode).Selected = true;
                cmbAgentGroup.Items.FindByValue(SettingFlight.AgentGroup).Selected = true;
                //ddlOrigin.SelectedIndex = -1;
                //ddlDestination.SelectedIndex = -1;
                txtStartDate.Value = SettingFlight.EffectiveStartDate;
                txtEndDate.Value = SettingFlight.EffectiveEndDate;
                ddlOrigin.SelectedIndex = -1;
                ddlDestination.SelectedIndex = -1;
                ddlOrigin.Items.FindByValue(SettingFlight.Origins).Selected = true;
                dt = objGeneral.ReturnAllCityCustom(ddlOrigin.SelectedItem.Value);
                SEAL.WEB.UI.Control.SetComboCustomStyle(ref ddlDestination, dt, "CustomState", "CityCode", "Select City");
                ddlDestination.Items.FindByValue(SettingFlight.Destination).Selected = true;
                HttpContext.Current.Session["SettingFlight"] = SettingFlight;
                HttpContext.Current.Session["SelectFlightChanged"] = "1";
            }
            dt = null;
        }

        protected void SaveDataFlight()
        {
            if (gvFlight.Selection.Count > 0)
                SettingFlight = (SectorSuspend)HttpContext.Current.Session["SettingFlight"];
            else
                SettingFlight.SectorSuspendID = DateTime.Now.ToString("yyyyMMddHHmmsss");
            SettingFlight.CarrierCode = cmbOptGroup.SelectedItem.Value.ToString();
            SettingFlight.AgentGroup = cmbAgentGroup.SelectedItem.Value.ToString();
            SettingFlight.Origins = ddlOrigin.SelectedItem.Value.ToString();
            SettingFlight.Destination = ddlDestination.SelectedItem.Value.ToString();
            SettingFlight.EffectiveStartDate = txtStartDate.Date;
            SettingFlight.EffectiveEndDate = txtEndDate.Date;
            SettingFlight.Active = Convert.ToByte(chkActive.Checked);
            SettingFlight.LastSyncBy = AdminSet.AdminID;
            if (gvFlight.Selection.Count > 0)
                objFlight.SaveSECTORSUSPEND(SettingFlight, BookingControl.EnumSaveType.Update);
            else
                objFlight.SaveSECTORSUSPEND(SettingFlight, BookingControl.EnumSaveType.Insert);
            dt = objFlight.GetAllSECTORSUSPENDFilter(cmbGRPID.SelectedItem.Value.ToString(), Convert.ToByte(chkFilterActive.Checked));
            gvFlight.DataSource = dt;
            gvFlight.DataBind();
            HttpContext.Current.Session["GridFlight"] = dt;
        }

        protected void btnSaveFlight_Click(object sender, EventArgs e)
        {

            if (ASPxEdit.AreEditorsValid(RpanelFlight))
            {
                SaveDataFlight();
                gvFlight.Selection.UnselectAll();
                txtStartDate.Value = DateTime.Now;
                txtEndDate.Value = DateTime.Now.AddDays(1);
                ddlOrigin.SelectedIndex = -1;
                ddlDestination.SelectedIndex = -1;
            }
        }

        protected void ClearFlightSelectionOnOtherPages()
        {
            if (gvFlight.Selection.Count <= 1) return;
            int curPageSelection = GetFlightSelectedRowOnTheCurrentPage();
            gvFlight.Selection.UnselectAll();
            gvFlight.Selection.SelectRow(curPageSelection);
        }

        public int GetFlightSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = gvFlight.PageIndex * gvFlight.SettingsPager.PageSize;
            for (int i = 0; i < gvFlight.VisibleRowCount; i++)
            {
                if (gvFlight.Selection.IsRowSelected(startIndexOnPage))
                {
                    return startIndexOnPage;
                }

                startIndexOnPage++; // increment
            }
            return -1;
        }

        protected void ddlOrigin_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //UIClass.SetComboCustomStyle(ref ddlReturn, UIClass.EnumDefineStyle.City, string.Empty, ddlDeparture.SelectedItem.Value, false);
                dt = objGeneral.ReturnAllCityCustom(ddlOrigin.SelectedItem.Value);
                SEAL.WEB.UI.Control.SetComboCustomStyle(ref ddlDestination, dt, "CustomState", "CityCode", "Select City");
                //BindCurrency(ddlDeparture.SelectedItem.Value);
                dt = null;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                log.Error(this,ex);
            }
        }

        protected void cmbOptGroup_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void btnSaveAgent_Click(object sender, EventArgs e)
        {

        }

    }
}