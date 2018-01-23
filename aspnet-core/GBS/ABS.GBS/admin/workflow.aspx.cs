using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ABS.GBS.admin
{
    public partial class workflow : System.Web.UI.Page
    {
        LogControl log = new LogControl();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["AdminSet"] != null)
                {

                }
                else
                {
                    Response.Redirect("~/admin/adminlogin.aspx");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, ex.Message);
            }
            
        }
    }
}