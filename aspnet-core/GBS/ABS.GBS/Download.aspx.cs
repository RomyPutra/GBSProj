using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;

using ABS.Logic.GroupBooking.Agent;
using ABS.Logic.GroupBooking.Booking;

using SEAL.Data;
using DevExpress.Web;
using System.Collections;
using ABS.Logic.Shared;
//using log4net;
using System.Globalization;
using ABS.Navitaire.BookingManager;
using System.Text.RegularExpressions;
using System.IO;

namespace GroupBooking.Web
{
    public partial class Download : System.Web.UI.Page
    {
        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
        protected void Page_Load(object sender, System.EventArgs e)
        {
            //If PageAuthentication() = True Then

            if ((Request.QueryString["f"] != null))
            {
                string filePath = Request.QueryString["f"];
                int LengthFiles = filePath.ToString().Split('\\').Length;
                string filename = filePath.ToString().Split('\\')[LengthFiles - 1];
                string CopyToFolder = Session["DirPathDownload"].ToString() + "Temp\\" + objGeneral.GenerateRandom(6);
                var CopyToPath = Path.Combine(CopyToFolder, filename);
                Page.Response.Clear();
                Page.Response.Buffer = false;
                Page.Response.AppendHeader("Content-Type", "application/xlsx");
                Page.Response.AppendHeader("Content-Transfer-Encoding", "binary");
                Page.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Write, FileShare.Read))
                {
                    using (var sw = new StreamWriter(stream))
                    {
                        //write to the file
                        if (!Directory.Exists(CopyToFolder))
                        {
                            System.IO.Directory.CreateDirectory(CopyToFolder);
                        }
                        if (!System.IO.File.Exists(CopyToPath))
                        {
                            System.IO.File.Copy(filePath, CopyToPath, true);
                        }
                        Page.Response.WriteFile(CopyToPath);
                    }

                }


                if (System.IO.File.Exists(filePath) == true)
                {
                    System.IO.File.Delete(filePath);
                }

                Page.Response.End();
                Page.Response.Close();
            }

        }
    }
}