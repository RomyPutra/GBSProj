using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Web;
using ABS.Logic.GroupBooking.Booking;
using ABS.GBS.Log;

namespace GroupBooking.Web.admin
{

    public partial class reportview : System.Web.UI.Page
    {
        DataTable dt = new DataTable();
        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();
        SysReport ReportSys = new SysReport();
        SysCode ReportCode = new SysCode();
        List<SysReportField> lstReportField = new List<SysReportField>();
        SystemLog SystemLog = new SystemLog();
        Hashtable ht = new Hashtable();
        enum ControlState
        {
            SalesPerformance = 0,
            BookingCancellation = 1,
            BalanceOverdue = 2,
            SalesDetail = 3,
            BookingSchedule = 4,
            WeeklySales = 5,
            DailyFareOverride = 6
        }
        ControlState _myState;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                InitState();
                InitDisplay();
                InitValue();
                grid.DataBind();
                if ((grid.Columns["Record Locator"] != null) && (grid.Columns["Agent Name"] != null))
                {
                    grid.Columns["Record Locator"].Width = new Unit(300, UnitType.Pixel);
                    grid.Columns["Agent Name"].Width = new Unit(200, UnitType.Pixel);
                }

                else if (grid.Columns["Record Locator"] != null)
                    grid.Columns["Record Locator"].Width = new Unit(300, UnitType.Pixel);

                else if (grid.Columns["Agent Name"] != null)
                    grid.Columns["Agent Name"].Width = new Unit(200, UnitType.Pixel);

                if (grid.Columns["Transaction ID"] != null)
                {
                    grid.Columns["Transaction ID"].Width = new Unit(150, UnitType.Pixel);
                    grid.Columns["Record Locator"].Width = new Unit(150, UnitType.Pixel);
                    grid.Columns["Organization ID"].Width = new Unit(100, UnitType.Pixel);
                    grid.Columns["Organization Name"].Width = new Unit(200, UnitType.Pixel);
                }

                if (grid.Columns["Username"] != null)
                    grid.Columns["Username"].Width = new Unit(200, UnitType.Pixel);

                if (grid.Columns["Country Code"] != null)
                    grid.Columns["Country Code"].Width = new Unit(60, UnitType.Pixel);

                if (grid.Columns["Carrier Code"] != null)
                    grid.Columns["Carrier Code"].Width = new Unit(50, UnitType.Pixel);

                if (grid.Columns["Total Pax"] != null)
                    grid.Columns["Total Pax"].Width = new Unit(50, UnitType.Pixel);

                if (grid.Columns["NDO"] != null)
                    grid.Columns["NDO"].Width = new Unit(50, UnitType.Pixel);
            }
            else
            {
                grid.DataBind();
                if ((grid.Columns["Record Locator"] != null) && (grid.Columns["Agent Name"] != null))
                {
                    grid.Columns["Record Locator"].Width = new Unit(300, UnitType.Pixel);
                    grid.Columns["Agent Name"].Width = new Unit(200, UnitType.Pixel);
                }

                else if (grid.Columns["Record Locator"] != null)
                    grid.Columns["Record Locator"].Width = new Unit(300, UnitType.Pixel);

                else if (grid.Columns["Agent Name"] != null)
                    grid.Columns["Agent Name"].Width = new Unit(200, UnitType.Pixel);

                if (grid.Columns["Transaction ID"] != null)
                {
                    grid.Columns["Transaction ID"].Width = new Unit(150, UnitType.Pixel);
                    grid.Columns["Record Locator"].Width = new Unit(150, UnitType.Pixel);
                    grid.Columns["Organization ID"].Width = new Unit(100, UnitType.Pixel);
                    grid.Columns["Organization Name"].Width = new Unit(200, UnitType.Pixel);
                }

                if (grid.Columns["Username"] != null)
                    grid.Columns["Username"].Width = new Unit(200, UnitType.Pixel);

                if (grid.Columns["Country Code"] != null)
                    grid.Columns["Country Code"].Width = new Unit(60, UnitType.Pixel);

                if (grid.Columns["Carrier Code"] != null)
                    grid.Columns["Carrier Code"].Width = new Unit(50, UnitType.Pixel);

                if (grid.Columns["Total Pax"] != null)
                    grid.Columns["Total Pax"].Width = new Unit(50, UnitType.Pixel);

                if (grid.Columns["NDO"] != null)
                    grid.Columns["NDO"].Width = new Unit(50, UnitType.Pixel);
            }

        }

        protected void InitState()
        {
            string stquery = Request.QueryString["optmode"];

            if (stquery == "0")
            {
                _myState = ControlState.SalesPerformance;
            }
            else if (stquery == "1")
            {
                _myState = ControlState.BookingCancellation;
            }
            else if (stquery == "2")
            {
                _myState = ControlState.BalanceOverdue;
            }
            else if (stquery == "3")
            {
                _myState = ControlState.SalesDetail;
            }
            else if (stquery == "4")
            {
                _myState = ControlState.BookingSchedule;
            }
            else if (stquery == "5")
            {
                _myState = ControlState.WeeklySales;
            }
            else if (stquery == "6")
            {
                _myState = ControlState.DailyFareOverride;
            }
        }

        protected void InitDisplay()
        {
            switch (_myState)
            {
                case ControlState.SalesPerformance:
                    ReportSys = objBooking.GetSingleSYS_REPORT(ReportCode.Performance);
                    lstReportField = objBooking.GetAllSYS_RPTFIELD(ReportCode.Performance);
                    //trDateRange.Visible = true;
                    //trTextBox.Visible = true;
                    //lblStartDate.Text = "Start Booking Date";
                    //lblEndDate.Text = "End Booking Date";                    
                    //lblString.Text = "Sector";
                    //lblSingleDate.Visible = false;
                    //txtSingleDate.Visible = false;
                    //lblNumber.Visible = false;
                    //txtNumber.Visible = false;
                    break;
                case ControlState.BookingCancellation:
                    ReportSys = objBooking.GetSingleSYS_REPORT(ReportCode.Cancellation);
                    lstReportField = objBooking.GetAllSYS_RPTFIELD(ReportCode.Cancellation);
                    //trDateRange.Visible = true;
                    //trTextBox.Visible = true;
                    //lblStartDate.Text = "Start Cancel Date";
                    //lblEndDate.Text = "End Cancel Date";
                    //lblString.Text = "Sector";
                    //lblSingleDate.Visible = false;
                    //txtSingleDate.Visible = false;
                    //lblNumber.Visible = false;
                    //txtNumber.Visible = false;
                    break;
                case ControlState.BalanceOverdue:
                    ReportSys = objBooking.GetSingleSYS_REPORT(ReportCode.BalanceOver);
                    lstReportField = objBooking.GetAllSYS_RPTFIELD(ReportCode.BalanceOver);
                    //trDateRange.Visible = false;
                    //trTextBox.Visible = true;
                    //lblNumber.Text = "Days Different";
                    //lblString.Text = "Sector";    
                    //lblSingleDate.Visible = false;
                    //txtSingleDate.Visible = false;
                    break;
                case ControlState.SalesDetail:
                    ReportSys = objBooking.GetSingleSYS_REPORT(ReportCode.Details);
                    lstReportField = objBooking.GetAllSYS_RPTFIELD(ReportCode.Details);
                    //trDateRange.Visible = true;
                    //trTextBox.Visible = false;
                    //lblStartDate.Text = "Start Booking Date";
                    //lblEndDate.Text = "End Booking Date";            
                    //lblSingleDate.Visible = false;
                    //txtSingleDate.Visible = false;
                    break;
                case ControlState.BookingSchedule:
                    ReportSys = objBooking.GetSingleSYS_REPORT(ReportCode.Schedule);
                    lstReportField = objBooking.GetAllSYS_RPTFIELD(ReportCode.Schedule);
                    //trDateRange.Visible = true;
                    //trTextBox.Visible = false;
                    //lblStartDate.Text = "Start Booking Date";
                    //lblEndDate.Text = "End Booking Date";
                    //lblSingleDate.Text = "Travel Date";
                    //lblSingleDate.Visible = true;
                    //txtSingleDate.Visible = true;
                    break;
                case ControlState.WeeklySales:
                    ReportSys = objBooking.GetSingleSYS_REPORT(ReportCode.Weekly);
                    lstReportField = objBooking.GetAllSYS_RPTFIELD(ReportCode.Weekly);
                    //trDateRange.Visible = true;
                    //trTextBox.Visible = false;
                    //lblStartDate.Text = "Start Booking Date";
                    //lblEndDate.Text = "End Booking Date";                    
                    //lblSingleDate.Visible = false;
                    //txtSingleDate.Visible = false;
                    break;
                case ControlState.DailyFareOverride:
                    ReportSys = objBooking.GetSingleSYS_REPORT(ReportCode.Daily);
                    lstReportField = objBooking.GetAllSYS_RPTFIELD(ReportCode.Daily);
                    //trDateRange.Visible = true;
                    //trTextBox.Visible = false;
                    //lblStartDate.Text = "Start Booking Date";
                    //lblEndDate.Text = "End Booking Date";                    
                    //lblSingleDate.Visible = false;
                    //txtSingleDate.Visible = false;
                    break;
            }
            lblHeader.Text = HttpContext.Current.Session["dtName"].ToString();
        }

        protected void InitValue()
        {
            //txtStartDate.Value = DateTime.Now;
            //txtEndDate.Value = DateTime.Now;
            //txtSingleDate.Value = DateTime.Now;

        }


        protected void grid_DataBinding(object sender, EventArgs e)
        {
            //switch (_myState)
            //{
            //    case ControlState.SalesPerformance:
            //        foreach (SysReportField RptField in lstReportField)
            //        {
            //            if (RptField.RptDBField == "CarrierCode")
            //                ht[RptField.RPTField] = cmbCarrier.Value;
            //            if (RptField.RptDBField == "FirstSector")
            //                ht[RptField.RPTField] = txtString.Text ;
            //        }

            //        dt = objBooking.GetReportData(ReportCode.Performance,ht);                    
            //         Session["grid"] = dt;
            //        break;
            //    case ControlState.BookingCancellation:

            //        break;
            //    case ControlState.BalanceOverdue:

            //        break;
            //    case ControlState.SalesDetail:

            //        break;
            //    case ControlState.BookingSchedule:

            //        break;
            //    case ControlState.WeeklySales:

            //        break;
            //    case ControlState.DailyFareOverride:

            //        break;
            //}

            //dt = (DataTable)HttpContext.Current.Session["dtShow"];
            
            //start here
            dt = (DataTable)HttpContext.Current.Session["dtShow"];
            if (dt != null)
            {
                int colcount = dt.Columns.Count;
                GridViewDataTextColumn[] tc = new GridViewDataTextColumn[colcount];
                int i = 0;
                foreach (DataColumn dc in dt.Columns)
                {
                    tc[i] = new GridViewDataTextColumn();
                    tc[i].Caption = dc.ColumnName;
                    tc[i].FieldName = dc.ColumnName;

                    switch (dc.DataType.FullName)
                    {
                        case "System.String":
                            tc[i].CellStyle.HorizontalAlign = HorizontalAlign.Left;
                            break;
                        case "System.DateTime":
                        case "System.Date":
                            tc[i].CellStyle.HorizontalAlign = HorizontalAlign.Left;
                            tc[i].PropertiesEdit.DisplayFormatString = "dd MMM yyyy";
                            break;
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                            tc[i].CellStyle.HorizontalAlign = HorizontalAlign.Right;
                            tc[i].PropertiesEdit.DisplayFormatString = "n0";
                            break;
                        case "System.Double":
                        case "System.Decimal":
                            tc[i].CellStyle.HorizontalAlign = HorizontalAlign.Right;
                            tc[i].PropertiesEdit.DisplayFormatString = "n2";
                            break;
                    }
                    i++;
                }
                grid.Columns.AddRange(tc);
                grid.DataSource = dt;
                if (dt != null)
                {
                    grid.Settings.ShowFooter = true;
                }
                else
                {
                    grid.Settings.ShowFooter = false;
                }
            }
            //end here
            (sender as ASPxGridView).DataSource = dt;
        }

        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            Session["selectedDataSource"] = Int32.Parse(e.Parameters);

            grid.Columns.Clear();
            grid.AutoGenerateColumns = true;
            grid.KeyFieldName = String.Empty;

            
            grid.DataBind();
        }


        protected void btnExport_Click(object sender, EventArgs e)
        {           
            try
            {
                //gvTemp.DataSource = (DataTable)HttpContext.Current.Session["dtShow"];
                //gvTemp.DataBind();
                //Utility.ExportExcel(this, gvTemp, "TransPaymentReport");
                exporter.GridViewID = "grid";
                exporter.FileName = "ReportData";
                exporter.WriteXlsToResponse();

                // Response.End;
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);

            }
        }


        protected void btnShow_Click(object sender, EventArgs e)
        {
            grid.DataBind();
        }

        protected void grid_DataBound(object sender, EventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;              
            if (!Page.IsPostBack)
            {
                    // TO insert summary in Footer
                    int i = 0;
                foreach (GridViewDataColumn column in grid.Columns)
                {
                    if (i == 0)
                        {
                            ASPxSummaryItem totalSummary = new ASPxSummaryItem();
                            totalSummary.FieldName = column.FieldName;
                            totalSummary.ShowInColumn = column.FieldName;
                            totalSummary.SummaryType = DevExpress.Data.SummaryItemType.Custom;
                            totalSummary.DisplayFormat = "Grand Total {0}";
                            grid.TotalSummary.Add(totalSummary);
                            //column.PropertiesEdit.DisplayFormatString = "Total";
                             
                            i += 1;
                        }
                    Type columnType = grid.GetRowValues(0, column.FieldName).GetType();
                    if (columnType == typeof(int))
                    {

                        ASPxSummaryItem totalSummary = new ASPxSummaryItem();
                        totalSummary.FieldName = column.FieldName;
                        totalSummary.ShowInColumn = column.FieldName;
                        totalSummary.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        totalSummary.DisplayFormat = "{0:n0}";
                        grid.TotalSummary.Add(totalSummary);
                    }
                    else if (columnType == typeof(Decimal))                        
                    {
                        ASPxSummaryItem totalSummary = new ASPxSummaryItem();
                        totalSummary.FieldName = column.FieldName;
                        totalSummary.ShowInColumn = column.FieldName;
                        totalSummary.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        totalSummary.DisplayFormat = "{0:n2}";
                        grid.TotalSummary.Add(totalSummary);
                    }
                    
                }
            }
        }

        
    }
}