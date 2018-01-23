using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Text;
using System.Collections;
using DevExpress.Web;
using ABS.Logic.GroupBooking.Agent;
using System.Data;
using ABS.Logic.GroupBooking.Booking;
using ABS.Logic.Shared;

namespace GroupBooking.Web.admin
{


    public partial class reportlist : System.Web.UI.Page
    {
        AdminSet AdminSet;
        DataTable dt = new DataTable();
        DataTable dtCarriercode = new DataTable();
        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
        List<SysReportField> lstReportField = new List<SysReportField>();
        SysReport ReportDesc = new SysReport();
        DataTable dttemp, dtFilter;
        Hashtable ht = new Hashtable();
        enum ControlState
        {
            StringType = 0,
            DateRange = 1,
            DateSingle = 2,
            CarrierCode = 3,
            MonthYear = 4
        }
        ControlState _mystate = new ControlState();
        String fieldType;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminSet"] != null)
            {
                AdminSet = (AdminSet)Session["AdminSet"];
            }

            if (!Page.IsPostBack)
            {
                InitValue();
            }
            else
            {
                LoadDisplay();
            }
            ClearSelectionOnOtherPages();
        }

        protected void InitValue()
        {
            HttpContext.Current.Session["Grid"] = null;
            HttpContext.Current.Session["dttemp"] = null;
            HttpContext.Current.Session["ht"] = null;
            HttpContext.Current.Session["dtShow"] = null;
            HttpContext.Current.Session["dtName"] = null;
            txtStartDate.Value = DateTime.Now;
            txtEndDate.Value = DateTime.Now;
            grid.DataBind();
            gvAddData.DataSource = null;
            gvAddData.DataBind();
        }


        protected void LoadDisplay()
        {
            lstReportField = (List<SysReportField>)HttpContext.Current.Session["cmbFilter"];
            cmbFilter.Items.Clear();
            cmbFilter.DataSource = lstReportField;
            cmbFilter.TextField = "RptDBField";
            cmbFilter.ValueField = "RptField";
            cmbFilter.DataBind();

            //20170524 - Sienny
            dtCarriercode = objGeneral.GetOPTGroup();
            cmbCarrier.DataSource = dtCarriercode;
            cmbCarrier.TextField = "CarrierCode";
            cmbCarrier.ValueField = "CarrierCode";
            cmbCarrier.DataBind();
            cmbCarrier.Items.Insert(0, new ListEditItem("All", "ALL"));

            if (HttpContext.Current.Session["dttemp"] != null)
            {
                dttemp = dtClass();
                dttemp = (DataTable)HttpContext.Current.Session["dttemp"];
                gvAddData.DataSource = dttemp;
                gvAddData.DataBind();
            }
        }


        protected void grid_DataBinding(object sender, EventArgs e)
        {
            //dt = objBooking.GetAllSYS_REPORT();
            dt = objBooking.GetSYS_REPORTByRole(AdminSet.GroupName);    //20170524 - Sienny
            (sender as ASPxGridView).DataSource = dt;
            HttpContext.Current.Session["Grid"] = dt;
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
 
        protected void grid_SelectionChanged(object sender, EventArgs e)
        {
            ClearScreen();

            cmbFilter.SelectedIndex = -1;
            HttpContext.Current.Session["dttemp"] = null;
            HttpContext.Current.Session["ht"] = null;
            gvAddData.DataSource = (DataTable)HttpContext.Current.Session["dttemp"];
            gvAddData.DataBind();

            dt = (DataTable)HttpContext.Current.Session["Grid"];
            string code = dt.Rows[GetSelectedRowOnTheCurrentPage()]["RptCode"].ToString();
            lstReportField = objBooking.GetAllSYS_RPTFIELD(code);

            cmbFilter.Items.Clear();
            cmbFilter.DataSource = lstReportField;
            cmbFilter.TextField = "RptDBField";
            cmbFilter.ValueField = "RptField";
            cmbFilter.DataBind();

            ReportDesc = objBooking.GetSingleSYS_REPORT(code);
            lblReportType.Text = ReportDesc.RPTTypeName;
            lblReportCode.Text = ReportDesc.RPTCode;
            memoDesc.Text = ReportDesc.RPTDesc;
            HttpContext.Current.Session["cmbFilter"] = lstReportField;

            //20170524 - Sienny
            dtCarriercode = objGeneral.GetOPTGroup();
            cmbCarrier.DataSource = dtCarriercode;
            cmbCarrier.TextField = "CarrierCode";
            cmbCarrier.ValueField = "CarrierCode";
            cmbCarrier.DataBind();
            cmbCarrier.Items.Insert(0, new ListEditItem("All", "ALL"));
        }

        protected void ClearScreen()
        {
            txtFilter.Text = "";
            txtFilter.Visible = false;
            cmbCarrier.SelectedIndex = 0;   
            cmbCarrier.Visible = false;
            txtStartDate.Text = "";
            txtStartDate.Visible = false;
            txtEndDate.Text = "";
            txtEndDate.Visible = false;
            lblStart.Visible = false;
            lblEnd.Visible = false;
            PnlStart.Visible = false;
            PnlEnd.Visible = false;
        }

        protected void cmb_SelectionChanged(object sender, EventArgs e)
        {
            UIClass.SetComboStyle(ref cmbYearStart, UIClass.EnumDefineStyle.Years);
            UIClass.SetComboStyle(ref cmbYearEnd, UIClass.EnumDefineStyle.Years);
            CheckState();
            cmbCarrier.Visible = false;
            txtStartDate.Visible = false;
            txtEndDate.Visible = false;
            txtFilter.Visible = false;
            txtFilter.Text = "";
            cmbMonthEnd.Visible = false;
            cmbMonthStart.Visible = false;
            cmbYearEnd.Visible = false;
            cmbYearStart.Visible = false;
            lblStart.Visible = false;
            lblEnd.Visible = false;            
            lblStart.Visible = false;
            lblEnd.Visible = false;
            PnlEnd.Visible = false;
            PnlStart.Visible = false;
            switch (_mystate)
            {
                case ControlState.StringType:
                    txtFilter.Visible = true;
                    break;
                case ControlState.DateRange:
                    lblStart.Visible = true;
                    lblEnd.Visible = true;
                    txtStartDate.Visible = true;
                    txtEndDate.Visible = true;
                    txtStartDate.Value = DateTime.Now;
                    txtEndDate.Value = DateTime.Now;
                    break;
                case ControlState.DateSingle:
                    txtStartDate.Visible = true;
                    break;
                case ControlState.CarrierCode:
                    cmbCarrier.Visible = true;
                    cmbCarrier.SelectedIndex = 0;   //20170524 - Sienny
                    break;
                case ControlState.MonthYear:
                    PnlStart.Visible = true;
                    PnlEnd.Visible = true;
                    lblStart.Visible = true;
                    lblEnd.Visible = true;
                    cmbMonthStart.Visible = true;
                    cmbMonthEnd.Visible = true;
                    cmbYearStart.Visible = true;
                    cmbYearEnd.Visible = true;                  
                    cmbMonthStart.SelectedIndex = DateTime.Now.Month;
                    cmbMonthEnd.SelectedIndex = DateTime.Now.Month;
                    cmbYearEnd.Items.FindByText(DateTime.Now.Year.ToString()).Selected = true; ;
                    cmbYearStart.Items.FindByText(DateTime.Now.Year.ToString()).Selected = true; ;
                    break;
            }
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (lblReportCode.Text != string.Empty)
            {
                if (HttpContext.Current.Session["Grid"] != null)
                    dt = (DataTable)HttpContext.Current.Session["Grid"];
                string code = dt.Rows[GetSelectedRowOnTheCurrentPage()]["RptCode"].ToString();
                string name = dt.Rows[GetSelectedRowOnTheCurrentPage()]["RptName"].ToString();
                //if (HttpContext.Current.Session["ht"] != null)
                //    ht = (Hashtable)HttpContext.Current.Session["ht"];
                if (HttpContext.Current.Session["dttemp"] != null)
                    //ht = (Hashtable)HttpContext.Current.Session["dttemp"];
                    dtFilter = (DataTable)HttpContext.Current.Session["dttemp"];
                DataTable dtShow = objBooking.GetReportData(code, dtFilter);
                HttpContext.Current.Session["dtShow"] = dtShow;
                HttpContext.Current.Session["dtName"] = name;
                Response.Redirect("reportview.aspx");
            }
        }
        protected void CheckState()
        {
            int i = cmbFilter.SelectedIndex;
            lstReportField = (List<SysReportField>)HttpContext.Current.Session["cmbFilter"];
            if (lstReportField[i].FieldType == 0)
            {
                _mystate = ControlState.StringType;
                fieldType = lstReportField[i].FieldType.ToString();
            }
            if (lstReportField[i].FieldType == 1)
            {
                _mystate = ControlState.DateRange;
                fieldType = lstReportField[i].FieldType.ToString();
            }
            if (lstReportField[i].FieldType == 2)
            {
                _mystate = ControlState.DateSingle;
                fieldType = lstReportField[i].FieldType.ToString();
            }
            if (lstReportField[i].FieldType == 3)
            {
                _mystate = ControlState.CarrierCode;
                fieldType = lstReportField[i].FieldType.ToString();
            }
            if (lstReportField[i].FieldType == 4)
            {
                _mystate = ControlState.MonthYear;
                fieldType = lstReportField[i].FieldType.ToString();
            }
        }

        protected void AssignHashtable()
        {
            switch (_mystate)
            {
                case ControlState.StringType:
                    ht[cmbFilter.Value.ToString()] = txtFilter.Text;
                    break;
                case ControlState.DateRange:
                    ht[cmbFilter.Value.ToString()] = txtStartDate.Text + "-" + txtEndDate.Text;
                    break;
                case ControlState.MonthYear:
                    ht[cmbFilter.Value.ToString()] = cmbYearStart.SelectedItem.Text + cmbMonthStart.SelectedItem.Text + "-" + cmbYearEnd.SelectedItem.Text + cmbMonthEnd.SelectedItem.Text;
                    break;
                case ControlState.DateSingle:
                    ht[cmbFilter.Value.ToString()] = txtStartDate.Text;
                    break;
                case ControlState.CarrierCode:
                    if (cmbCarrier.Value.ToString() == "ALL")
                    {
                        ht[cmbFilter.Value.ToString()] = cmbCarrier.Value.ToString();
                    }
                    else if (ht[cmbFilter.Value.ToString()] != null)
                    {
                        if (ht[cmbFilter.Value.ToString()].ToString() == "ALL")
                        {
                            ht[cmbFilter.Value.ToString()] = cmbCarrier.Value.ToString();
                        }
                        else if (!ht[cmbFilter.Value.ToString()].ToString().Contains(cmbCarrier.Value.ToString()))
                        {
                            ht[cmbFilter.Value.ToString()] = ht[cmbFilter.Value.ToString()] + "," + cmbCarrier.Value.ToString();
                        }
                    }
                    else
                    {
                        ht[cmbFilter.Value.ToString()] = cmbCarrier.Value.ToString();
                    }
                    break;
            }
        }

        protected void AssignAddRow(DataRow dr)
        {
            switch (_mystate)
            {
                case ControlState.StringType:
                    dr["Value"] = txtFilter.Text;
                    break;
                case ControlState.DateRange:
                    dr["Value"] = txtStartDate.Text + "-" + txtEndDate.Text;
                    break;
                case ControlState.MonthYear:
                    dr["Value"] = cmbYearStart.SelectedItem.Text + cmbMonthStart.SelectedItem.Text + "-" + cmbYearEnd.SelectedItem.Text + cmbMonthEnd.SelectedItem.Text;
                    break;
                case ControlState.DateSingle:
                    dr["Value"] = txtStartDate.Text;
                    break;
                case ControlState.CarrierCode:
                    dr["Value"] = cmbCarrier.Value.ToString();
                    break;
            }
        }

        protected void AssignExistingRow(int found)
        {
            switch (_mystate)
            {
                case ControlState.StringType:
                    dttemp.Rows[found]["Value"] = txtFilter.Text;
                    break;
                case ControlState.DateRange:
                    dttemp.Rows[found]["Value"] = txtStartDate.Text + "-" + txtEndDate.Text;
                    break;
                case ControlState.DateSingle:
                    dttemp.Rows[found]["Value"] = txtStartDate.Text;
                    break;
                case ControlState.CarrierCode:
                    if (cmbCarrier.Value.ToString() == "ALL")
                    {
                        dttemp.Rows[found]["Value"] = cmbCarrier.Value.ToString();
                    }
                    else if (dttemp.Rows[found]["Value"] != null)
                    {
                        if (dttemp.Rows[found]["Value"].ToString() == "ALL")
                        {
                            dttemp.Rows[found]["Value"] = cmbCarrier.Value.ToString();
                        }
                        else if (!dttemp.Rows[found]["Value"].ToString().Contains(cmbCarrier.Value.ToString()))
                        {
                            dttemp.Rows[found]["Value"] = dttemp.Rows[found]["Value"] + "," + cmbCarrier.Value.ToString();
                        }
                    }
                    else
                    {
                        dttemp.Rows[found]["Value"] = cmbCarrier.Value.ToString();
                    }
                    break;
                case ControlState.MonthYear:
                    dttemp.Rows[found]["Value"] = cmbYearStart.SelectedItem.Text + cmbMonthStart.SelectedItem.Text + "-" + cmbYearEnd.SelectedItem.Text + cmbMonthEnd.SelectedItem.Text;
                    break;
            }
        }    
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            CheckState();
            {
                dttemp = dtClass();
                if (HttpContext.Current.Session["dttemp"] != null)
                    dttemp = (DataTable)Session["dttemp"];
                if (HttpContext.Current.Session["ht"] != null)
                    ht = (Hashtable)Session["ht"];
                AssignHashtable();
                int i = 0;
                int found = -1;
                foreach (DataRow drtemp in dttemp.Rows)
                {
                    if (drtemp["FieldName"].ToString() == cmbFilter.Text)
                    {
                        found = i;
                    }
                    i++;
                }
                if (found == -1)
                {
                    DataRow dr = dttemp.NewRow();
                    dr["FieldName"] = cmbFilter.Text;
                    AssignAddRow(dr);
                    dr["key"] = cmbFilter.Value.ToString();
                    dr["FieldType"] = fieldType;
                    dttemp.Rows.Add(dr);
                }
                else
                {
                    dttemp.Rows[found]["FieldName"] = cmbFilter.Text;
                    AssignExistingRow(found);
                    dttemp.Rows[found]["key"] = cmbFilter.Value.ToString();
                    dttemp.Rows[found]["FieldType"] = fieldType;
                }
                HttpContext.Current.Session["ht"] = ht;
                HttpContext.Current.Session["dttemp"] = dttemp;
                gvAddData.DataSource = dttemp;
                gvAddData.DataBind();

                //20170524 - Sienny
                ClearScreen();
                cmbFilter.SelectedIndex = -1;
            }            
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            if (gvAddData.Selection.Count > 0)
            {
                int i = GetSelectedRowDT();
                if (HttpContext.Current.Session["dttemp"] != null)
                    dttemp = (DataTable)Session["dttemp"];
                if (HttpContext.Current.Session["ht"] != null)
                    ht = (Hashtable)Session["ht"];
                ht.Remove(dttemp.Rows[i]["key"].ToString());
                dttemp.Rows[i].Delete();
                HttpContext.Current.Session["ht"] = ht;
                HttpContext.Current.Session["dttemp"] = dttemp;
                gvAddData.DataSource = dttemp;
                gvAddData.DataBind();
            }
        }

        protected void ClearSelectionOnOtherPages()
        {
            if (grid.Selection.Count <= 1) return;
            int curPageSelection = GetSelectedRowOnTheCurrentPage();
            grid.Selection.UnselectAll();
            grid.Selection.SelectRow(curPageSelection);
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

        public int GetSelectedRowDT()
        {
            int startIndexOnPage = gvAddData.PageIndex * gvAddData.SettingsPager.PageSize;
            for (int i = 0; i < gvAddData.VisibleRowCount; i++)
            {
                if (gvAddData.Selection.IsRowSelected(startIndexOnPage))
                {
                    return startIndexOnPage;
                }

                startIndexOnPage++; // increment
            }
            return -1;
        }

        public DataTable dtClass()
        {
            // Define the new datatable
            DataTable dt = new DataTable();

            DataColumn dc;
            dc = new DataColumn("FieldName");
            dt.Columns.Add(dc);
            dc = new DataColumn("Value");
            dt.Columns.Add(dc);
            dc = new DataColumn("key");
            dt.Columns.Add(dc);
            dc = new DataColumn("FieldType");
            dt.Columns.Add(dc);
            return dt;
        }
    }
}
