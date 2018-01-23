using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections;
using System.Web.Security;
using System.Security.Cryptography;
using ABS.Logic.GroupBooking.Agent;
using ABS.Logic.GroupBooking;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using ABS.Navitaire.AgentManager;
using ABS.GBS.Log;
using StackExchange.Profiling;
//using log4net;

namespace GroupBooking.Web
{
    public partial class _default : System.Web.UI.Page
    {
        AgentCategory AgentCat;
        //AgentControl.StrucAgentSet AgentSet;
        ABS.Logic.Shared.UserSet AgentSet = new ABS.Logic.Shared.UserSet();
        ABS.Logic.GroupBooking.Agent.AgentControl objAgent = new ABS.Logic.GroupBooking.Agent.AgentControl();
        ABS.Logic.GroupBooking.Agent.AgentProfileControl objAgentProfile = new ABS.Logic.GroupBooking.Agent.AgentProfileControl();
        AgentCategory agCategoryInfo = new AgentCategory();
        AgentProfile agProfileInfo = new AgentProfile();
        ABS.Logic.GroupBooking.Booking.BookingControl objBooking = new ABS.Logic.GroupBooking.Booking.BookingControl();

        //20170510 - Sienny
        ABS.Logic.GroupBooking.GeneralControl objGeneral = new ABS.Logic.GroupBooking.GeneralControl();
        List<ABS.Logic.GroupBooking.Country_Info> CountryInfoData = new List<ABS.Logic.GroupBooking.Country_Info>();

        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogControl log = new LogControl();

        SystemLog SystemLog = new SystemLog();
        protected void Page_Load(object sender, EventArgs e)
        {
            var profiler = MiniProfiler.Current;
            //return;
            try
            {
                if (Request.QueryString["aid"] == null || Request.QueryString["hashkey"] == null || Request.QueryString["aName"] == null)
                {
                    Session["AgentSet"] = null;
                    Response.Redirect(Shared.MySite.PublicPages.InvalidPage, false);
                    return;
                }
                else if (Request.QueryString["aid"] == "" || Request.QueryString["hashkey"] == "" || Request.QueryString["aName"] == "")
                {
                    Session["AgentSet"] = null;
                    Response.Redirect(Shared.MySite.PublicPages.InvalidPage, false);
                    return;
                }
                GeneralControl objGeneral = new GeneralControl();
                string result = "Unknown permission error, please contact the system administrator.";
                string aid = Request.QueryString["aid"].ToString();
                string hashkey = Request.QueryString["hashkey"].ToString();
                string name = Request.QueryString["aName"].ToString();
                string mac = objGeneral.GenerateMac(ConfigurationManager.AppSettings["hashing"].ToString(), aid, name);
                string OrganizationName = "";
                string OrganizationID = "";
                string CountryCode = ""; //20170510 - Sienny
                MessageList msgList = new MessageList();

                if (mac == hashkey)
                {
                    int code = AuthSkyAgent(name, aid);
                    //temp login with sky agent

                    //amended by diana 20131011 - grab data from db only if exist in navitaire
                    if (code == 100)
                    {
                        code = DirectLogonSkyAgent(name);
                    }

                    if (code == 100)
                    {
                        if (Session["OrganizationName"] != null)
                        {
                            OrganizationName = Session["OrganizationName"].ToString();
                        }
                        if (Session["OrganizationCode"].ToString() != null)
                            OrganizationID = Session["OrganizationCode"].ToString();

                        //20170510 - Sienny
                        if (Session["CountryCode"].ToString() != null)
                            CountryCode = Session["CountryCode"].ToString();

                        code = Shared.Common.AuthPublicAgent(name, "", "skyagent", OrganizationName);
                        result = "";
                        if (code == 100)
                        {
                            agProfileInfo = objAgentProfile.GetSingleAgentProfile(name);
                            if (agProfileInfo != null)
                            {
                                string address1 = "";
                                string address2 = "";
                                string country = "";

                                if (agProfileInfo.Address1 != null) address1 = agProfileInfo.Address1;
                                if (agProfileInfo.Address2 != null) address2 = agProfileInfo.Address2;
                                if (agProfileInfo.Country != null) country = agProfileInfo.Country;

                                Session["complete"] = "";
                                Session["agProfileInfo"] = agProfileInfo;
                                //amend by tyas, remove address2
                                if (address1 == "" || country == "")
                                {
                                    //Remark by ketee, since not able to get from API, we remove the validation, 20160329
                                    //Session["complete"] = "false";
                                }

                                //added by ketee, load new blacklisted agent
                                //LoadNewAgentListIntoBlackList();
                                String DirPathUpload = "C:\\UploadFiles\\";
                                Session["DirPathUpload"] = DirPathUpload;

                                String DirPathDownload = "C:\\DownloadFile\\";
                                Session["DirPathDownload"] = DirPathDownload;

                                //DataTable dtArrayBaggage = objBooking.GetDetailSSRbyCat("PBA");
                                //DataTable dtArraySport = objBooking.GetDetailSSRbyCat("PBS");
                                //DataTable dtArrayKit = objBooking.GetDetailSSRbyCat("PCM");
                                //DataTable dtArrayDrink = objBooking.GetDetailSSR("WYD");
                                //DataTable dtArrayMeal = objBooking.GetDetailSSR("WYM");
                                //DataTable dtArrayDuty = objBooking.GetDetailSSRbyCat("WCH");
                                //DataTable dtArrayInfant = objBooking.GetDetailSSR("INF");

                                //Session["dtArrayBaggage"] = dtArrayBaggage;
                                //Session["dtArraySport"] = dtArraySport;
                                //Session["dtArrayKit"] = dtArrayKit;
                                //Session["dtArrayDrink"] = dtArrayDrink;
                                //Session["dtArrayMeal"] = dtArrayMeal;
                                //Session["dtArrayDuty"] = dtArrayDuty;
                                //Session["dtArrayInfant"] = dtArrayInfant;

                                ////added by romy for insurance
                                //DataTable dtInsure = objBooking.GetInsure("IND");
                                //if (dtInsure != null && dtInsure.Rows[0]["CodeDesc"].ToString() == "1")
                                //{
                                //    DataTable dtArrayInsure = objBooking.GetInsureCode("INS");//added by romy, 20170814, insurance
                                //    Session["dtArrayInsure"] = dtArrayInsure;//added by romy, 20170814, insurance
                                //    Session["InsureEnable"] = true;
                                //}
                                //else
                                //{
                                //    Session["InsureEnable"] = false;
                                //}

                                //insertCityValue(country);

                                Response.Redirect(Shared.MySite.PublicPages.Searchflight, false);
                            }
                        }

                    }

                    if (code == 101)
                    {
                        result = msgList.Err100012;
                    }
                    else
                    {
                        if (code == 102)
                        {
                            result = msgList.Err100011;
                        }
                    }

                    if (code == 108)
                        result = msgList.Err100053;

                    if (code == 1081)
                        result = msgList.Err100053 + ", country code not found, please verify or contact group desk for further information";

                    if (code == 109)
                        result = msgList.Err100053 + ", email address not found";

                    if (result != "")
                    {
                        Session["AgentSet"] = null;
                        Response.Redirect(Shared.MySite.PublicPages.InvalidPage + "?err=" + code, false);
                    }
                }
                else
                {
                    Session["AgentSet"] = null;
                    Response.Redirect(Shared.MySite.PublicPages.InvalidPage, false);
                }
            }
            catch (Exception ex)
            {
                SystemLog.Notifier.Notify(ex);
                Session["AgentSet"] = null;
                Response.Redirect(Shared.MySite.PublicPages.InvalidPage, false);
            }
        }

        private void insertCityValue(string country)
        {


            ABS.Logic.GroupBooking.GeneralControl CountryBase = new ABS.Logic.GroupBooking.GeneralControl();
            List<Country_Info> listCountry = new List<Country_Info>();
            //listCountry = CountryBase.GetAllCountry();

            string[] cityValue;
            string[] cityText;
            string combinedCityValue = "";
            string combinedCityText = "";

            int i = 0;

            cityValue = new string[1];
            cityText = new string[1];
            log.Info(this, "ListCountry Count: " + listCountry.Count);
            cityValue[i] = "[";
            cityText[i] = "[";
            List<Country_Info> listCity = new List<Country_Info>();

            listCity = CountryBase.GetAllState(country);
            int countIndex = 0;

            foreach (Country_Info cityItem in listCity)
            {
                if (countIndex > 0)
                {
                    cityValue[i] += ",";
                    cityText[i] += ",";
                }
                cityValue[i] += "'" + cityItem.provincestatecode + "'";
                cityText[i] += "'" + cityItem.provinceStateName + "'";

                countIndex += 1;
            }
            cityValue[i] += "]";
            cityText[i] += "]";

            i += 1;


            combinedCityValue = "[" + String.Join(",", cityValue) + "]";
            combinedCityText = "[" + String.Join(",", cityText) + "]";

            string[] combinedCity = new string[2];
            combinedCity[0] = combinedCityValue;
            combinedCity[1] = combinedCityText;

            Session["combinedCity"] = combinedCity;

        }
        //public string GenerateMac(string strSharedKey, string strAID, string strName)
        //{
        //    string strMac = string.Empty;
        //    string strEncrypt = strSharedKey + strAID;
        //    strMac = Sha256AddSecret(strEncrypt);

        //    return strMac;
        //}

        //public string Sha256AddSecret(string strChange)
        //{
        //    //Change the syllable into UTF8 code
        //    byte[] pass = Encoding.UTF8.GetBytes(strChange);
        //    SHA256 sha256 = new SHA256CryptoServiceProvider();
        //    byte[] hashValue = sha256.ComputeHash(pass);

        //    return BitConverter.ToString(hashValue).Replace("-", "").ToLower();
        //}

        private int DirectLogonSkyAgent(string Username)
        {
            try
            {
                string psDomain;
                string psName;
                string psPwd;
                psDomain = ConfigurationManager.AppSettings["signature_domain"].ToString();
                psName = ConfigurationManager.AppSettings["signature_username"].ToString();
                psPwd = ConfigurationManager.AppSettings["signature_password"].ToString();
                //signature = SessionManager.Logon();//
                Session["LoginName"] = Username;
                Session["LoginType"] = "SkyAgent";
                Session["LoginPWD"] = "";
                Session["LoginDomain"] = "EXT";
                if (Request.Cookies["cookieLogin"] == null)
                {
                    HttpCookie cookie = new HttpCookie("cookieLoginName");
                    cookie.HttpOnly = true;
                    cookie.Values.Add("LoginName", Username);
                    cookie.Values.Add("PWD", "");
                    cookie.Values.Add("Domain", psDomain);
                    Response.AppendCookie(cookie);
                }

                //check exist on database

                if (objAgentProfile.CheckRecordExist(Username) == 1)
                {

                    agProfileInfo = objAgentProfile.GetSingleAgentProfile(Username);
                    if (agProfileInfo != null)
                    {
                        AgentSet.AgentID = agProfileInfo.AgentID;
                        AgentSet.AgentCategoryID = agProfileInfo.AgentCatgID;
                        AgentSet.AgentName = agProfileInfo.Username;
                        AgentSet.Password = agProfileInfo.Password;
                        Session["SessionLogin"] = agProfileInfo.SyncLastUpd;
                        AgentSet.AgentType = ABS.Logic.Shared.EnumAgentType.SkyAgent;
                    }

                    //check blacklist
                    if (objAgent.VerifyBlackList(AgentSet.AgentID) == true)
                    {
                        //?Error
                        //added by ketee, show error msgbox
                        //MessageList msgList = new MessageList();
                        //ArrayList getmsg = new ArrayList();

                        //getmsg.Add(msgList.Testing.ToString());
                        //msgcontrol.ListMessage = getmsg;

                        return 101;//blacklisted
                    }
                    else
                    {
                        if (agProfileInfo != null)
                        {
                            AgentCat = objAgent.GetAgentCategory(AgentSet.AgentCategoryID);
                            AgentSet.BlacklistDuration = AgentCat.BlacklistDuration;
                            AgentSet.CounterTimer = AgentCat.CounterTimer;
                            AgentSet.MaxEnquiry = AgentCat.MaxEnquiry;
                            AgentSet.MaxSuspend = AgentCat.MaxSuspend;
                            AgentSet.SuspendDuration = AgentCat.SuspendDuration;
                            AgentSet.LoginName = AgentSet.AgentName;
                            AgentSet.Password = agProfileInfo.Password;
                            if (Session["OrganizationName"] != null)
                            {
                                AgentSet.OrganizationName = Session["OrganizationName"].ToString();
                            }
                            if (Session["OrganizationCode"].ToString() != null)
                                AgentSet.OrganicationID = Session["OrganizationCode"].ToString();
                            //else
                            //    AgentSet.OrganicationID = 
                            Session["SessionLogin"] = agProfileInfo.SyncLastUpd;
                            AgentSet.Email = agProfileInfo.Email;
                            AgentSet.Contact = agProfileInfo.MobileNo;
                            if (Session["AGCurrencyCode"] != null && Session["AGCurrencyCode"].ToString() != "")
                            {
                                AgentSet.Currency = Session["AGCurrencyCode"].ToString();
                            }
                            Session["AgentSet"] = AgentSet;

                            return 100;//success
                        }
                        else
                        {
                            return 104;
                        }
                    }
                }
                else
                {
                    return 104;
                }
            }
            catch (Exception ex)
            {
                return 104;
            }
        }

        private int AuthSkyAgent(string Username, string AgentID)
        {
            GeneralControl objGeneral = new GeneralControl();
            string orgEmail = "";
            var profiler = MiniProfiler.Current;
            try
            {

                string psDomain = "";
                string psName = "";
                string psPwd = "";
                string OrganizationName = "";
                string OriganicationID = "";
                string CountryCode = ""; //20170510 - Sienny
                string CurrencyCode = ""; //20170510 - Sienny

                ABS.Navitaire.APIBooking apiNavitaire = new ABS.Navitaire.APIBooking("");
                ABS.Navitaire.APIAgent apiAgent = new ABS.Navitaire.APIAgent();
                psDomain = ConfigurationManager.AppSettings["signature_domain"].ToString();
                psName = ConfigurationManager.AppSettings["signature_username"].ToString();
                psPwd = ConfigurationManager.AppSettings["signature_password"].ToString();

                string signature = "";// apiNavitaire.AgentLogon("Public", psDomain, psName, psPwd);
                using (profiler.Step("Navitaire:AgentLogon"))
                {
                    signature = apiNavitaire.AgentLogon("Public", psDomain, psName, psPwd);
                    //string signature = apiNavitaire.AgentLogon("SkyAgent", psDomain, psName, txtPassword.Text);
                }
                if (signature != "")
                {
                    //signature = SessionManager.Logon();//
                    Session["LoginName"] = Username;
                    Session["LoginType"] = "SkyAgent";
                    Session["LoginPWD"] = "";
                    Session["LoginDomain"] = "EXT";
                    if (Request.Cookies["cookieLogin"] == null)
                    {
                        HttpCookie cookie = new HttpCookie("cookieLoginName");
                        cookie.HttpOnly = true;
                        cookie.Values.Add("LoginName", Username);
                        cookie.Values.Add("PWD", "");
                        cookie.Values.Add("Domain", psDomain);
                        cookie.Values.Add("Signature", signature);
                        Response.AppendCookie(cookie);
                    }

                    ABS.Navitaire.PersonManager.GetPersonResponse PersonResp = new ABS.Navitaire.PersonManager.GetPersonResponse();
                    using (profiler.Step("Navitaire:GetPerson"))
                    {
                        PersonResp = apiAgent.GetPersonByID(Username, signature, AgentID);
                    }
                    //string str = GetXMLString(PersonResp);

                    Session["OrganizationCode"] = "";
                    if (PersonResp != null) //added by diana, to store organizationCode in session
                    {
                        ABS.Navitaire.AgentManager.Agent ag = new ABS.Navitaire.AgentManager.Agent();// apiAgent.GetAgentByID(Username, signature, AgentID);
                        using (profiler.Step("Navitaire:GetAgent"))
                        {
                            ag = apiAgent.GetAgentByID(Username, signature, AgentID);
                        }
                        Session["OrganizationCode"] = ag.AgentIdentifier.OrganizationCode;
                        OriganicationID = ag.AgentIdentifier.OrganizationCode;
                        Session["AGCurrencyCode"] = PersonResp.Person.CurrencyCode;
                    }

                    GetOrganizationResponse org = apiAgent.GetOrganization(Username, signature, AgentID, Session["OrganizationCode"].ToString());
                    if (org != null && org.Organization != null && org.Organization.OrganizationName != "")
                    {
                        OrganizationName = org.Organization.OrganizationName;
                        orgEmail = org.Organization.EmailAddress.Trim();
                    }
                    else
                        return 108;
                    //OrganizationName = apiAgent.GetOrganizationName(Username, signature, AgentID, Session["OrganizationCode"].ToString());
                    Session["OrganizationName"] = OrganizationName;

                    //20170510 - Sienny (country code)
                    if (org != null && org.Organization != null && (org.Organization.Address.CountryCode != null || org.Organization.Address.CountryCode != ""))
                    {
                        CountryCode = org.Organization.Address.CountryCode;
                    }
                    else
                    {
                        //DataTable dtCountryInfo = new DataTable();
                        //if (org.Organization.CurrencyCode != null && org.Organization.CurrencyCode != "")
                        //{
                        //    dtCountryInfo = objGeneral.GetCountryCodeByCurrency(org.Organization.CurrencyCode);
                        //    CountryCode = dtCountryInfo.Rows[0]["DefaultCurrencyCode"].ToString();
                        //}
                        //else
                        //    CountryCode = "";

                        //Show error message if null or empty
                        return 1081;
                    }
                    Session["CountryCode"] = CountryCode;


                    if (objAgentProfile.CheckRecordExist(Username, AgentID) == 1)
                    {
                        agProfileInfo = objAgentProfile.GetSingleAgentProfile(Username, AgentID);
                    }

                    if (agProfileInfo == null)
                    {
                        agProfileInfo = new AgentProfile();
                        agProfileInfo.AgentID = AgentID;
                    }

                    if (PersonResp != null)
                    {
                        if (PersonResp.Person.PersonNameList.Length > 0)
                        {
                            agProfileInfo.Title = PersonResp.Person.PersonNameList[0].Name.Title;
                            agProfileInfo.ContactFirstName = PersonResp.Person.PersonNameList[0].Name.FirstName;
                            agProfileInfo.ContactLastName = PersonResp.Person.PersonNameList[0].Name.LastName;
                        }

                        agProfileInfo.Username = Username;

                        if (PersonResp.Person.PersonEMailList.Length > 0)
                        {
                            //Added by ketee, manually set to custom email address for user VanceInfo_test1
                            if (Username.Trim().ToUpper().Equals("VANCEINFO_TEST1"))
                            {
                                agProfileInfo.Email = System.Configuration.ConfigurationManager.AppSettings["ErrorLogEmail"].ToString();
                            }
                            else
                            {
                                //Added by ketee, enhance pulling correct default email address from agent account , 20170622
                                for (int i = 0; i < PersonResp.Person.PersonEMailList.Length; i++)
                                {
                                    if (PersonResp.Person.PersonEMailList[i].Default == true)
                                    {
                                        agProfileInfo.Email = PersonResp.Person.PersonEMailList[i].EMailAddress;
                                    }
                                }

                                //agProfileInfo.Email = PersonResp.Person.PersonEMailList[0].EMailAddress;
                            }
                            //string xml = GetXMLString(PersonResp);
                        }
                        else
                        {
                            if (orgEmail != "")
                                agProfileInfo.Email = orgEmail;
                            else
                                return 109;
                        }

                        if (PersonResp.Person.PersonPhoneList.Length > 0)
                        {
                            agProfileInfo.PhoneNo = PersonResp.Person.PersonPhoneList[0].Number;
                        }
                        if (PersonResp.Person.PersonAddressList.Length > 0)
                        {
                            agProfileInfo.Address1 = PersonResp.Person.PersonAddressList[0].Address.AddressLine1;
                            agProfileInfo.Address2 = PersonResp.Person.PersonAddressList[0].Address.AddressLine2;
                            agProfileInfo.City = PersonResp.Person.PersonAddressList[0].Address.City;
                            agProfileInfo.State = PersonResp.Person.PersonAddressList[0].Address.ProvinceState;
                            agProfileInfo.Country = PersonResp.Person.PersonAddressList[0].Address.CountryCode;
                            agProfileInfo.Postcode = PersonResp.Person.PersonAddressList[0].Address.PostalCode;
                        }
                        if (PersonResp.Person.PersonContactList.Length > 0)
                        {
                            agProfileInfo.Title = PersonResp.Person.PersonContactList[0].Title;
                            agProfileInfo.Postcode = PersonResp.Person.PersonContactList[0].PostalCode;
                            agProfileInfo.MobileNo = PersonResp.Person.PersonContactList[0].WorkPhone;
                            agProfileInfo.Country = PersonResp.Person.PersonContactList[0].CountryCode;
                            agProfileInfo.Fax = PersonResp.Person.PersonContactList[0].Fax;
                            agProfileInfo.Address1 = PersonResp.Person.PersonContactList[0].AddressLine1;
                            agProfileInfo.Address2 = PersonResp.Person.PersonContactList[0].AddressLine2;
                            agProfileInfo.City = PersonResp.Person.PersonContactList[0].City;
                            agProfileInfo.State = PersonResp.Person.PersonContactList[0].ProvinceState;
                        }

                        if (PersonResp.Person.PersonPhoneList.Length > 0)
                        {
                            if (PersonResp.Person.PersonPhoneList[0].Number != "")
                            {
                                agProfileInfo.MobileNo = PersonResp.Person.PersonPhoneList[0].Number;
                            }
                            else
                            {
                                agProfileInfo.MobileNo = PersonResp.Person.PersonPhoneList[0].PhoneCode;
                            }
                        }

                    }
                    else
                    {
                        return 104;
                    }

                    //check exist on database

                    if (objAgentProfile.CheckRecordExist(Username, AgentID) == 1)
                    {
                        if (agProfileInfo != null)
                        {
                            AgentSet.AgentID = agProfileInfo.AgentID;
                            AgentSet.AgentCategoryID = agProfileInfo.AgentCatgID;
                            AgentSet.AgentName = agProfileInfo.Username;
                            AgentSet.Password = agProfileInfo.Password;
                            AgentSet.AgentType = ABS.Logic.Shared.EnumAgentType.SkyAgent;
                        }


                        //update agent profile
                        //ABS.Navitaire.PersonManager.GetPersonResponse PersonResp = new ABS.Navitaire.PersonManager.GetPersonResponse();
                        //PersonResp = apiAgent.GetPersonByID(Username, signature);
                        //string str = GetXMLString(PersonResp);
                        //if (PersonResp != null)
                        //{

                        //    agProfileInfo.Email = PersonResp.Person.PersonEMailList[0].EMailAddress.ToString();
                        //    agProfileInfo.PhoneNo = PersonResp.Person.PersonPhoneList[0].Number;
                        //    agProfileInfo.Address1 = PersonResp.Person.PersonAddressList[0].Address.AddressLine1;
                        //    agProfileInfo.Address2 = PersonResp.Person.PersonAddressList[0].Address.AddressLine2;
                        //    agProfileInfo.City = PersonResp.Person.PersonAddressList[0].Address.City;
                        //    agProfileInfo.State = PersonResp.Person.PersonAddressList[0].State.ToString();
                        //    agProfileInfo.Title = PersonResp.Person.PersonContactList[0].Title;
                        //    agProfileInfo.Postcode = PersonResp.Person.PersonContactList[0].PostalCode;
                        //    agProfileInfo.MobileNo = PersonResp.Person.PersonContactList[0].WorkPhone;
                        //    agProfileInfo.Country = PersonResp.Person.PersonContactList[0].CountryCode;
                        //    agProfileInfo.Fax = PersonResp.Person.PersonContactList[0].Fax;
                        agProfileInfo.SyncLastUpd = DateTime.Now;
                        agProfileInfo.LastSyncBy = agProfileInfo.AgentID;
                        agProfileInfo.LastModifyDate = DateTime.Now;
                        //    agProfileInfo = objAgentProfile.SaveAgentProfile(agProfileInfo, null, ABS.Logic.GroupBooking.Agent.AgentControl.EnumSaveType.Update);
                        //}

                        //20170411 - Sienny (2 fields added to ag_profile)
                        agProfileInfo.OrgID = Session["OrganizationCode"].ToString();
                        agProfileInfo.OrgName = Session["OrganizationName"].ToString();

                        agProfileInfo.Address3 = Session["CountryCode"].ToString(); //20170510 - Sienny (to store country code)

                        agProfileInfo = objAgentProfile.SaveAgentProfile(agProfileInfo, null, ABS.Logic.GroupBooking.Agent.AgentControl.EnumSaveType.Update);
                        //check blacklist
                        if (objAgent.VerifyBlackList(AgentSet.AgentID) == true)
                        {
                            //?Error
                            //added by ketee, show error msgbox
                            //MessageList msgList = new MessageList();
                            //ArrayList getmsg = new ArrayList();

                            //getmsg.Add(msgList.Testing.ToString());
                            //msgcontrol.ListMessage = getmsg;

                            return 101;//blacklisted
                        }
                        else
                        {
                            if (agProfileInfo != null)
                            {
                                AgentCat = objAgent.GetAgentCategory(AgentSet.AgentCategoryID);
                                AgentSet.BlacklistDuration = AgentCat.BlacklistDuration;
                                AgentSet.CounterTimer = AgentCat.CounterTimer;
                                AgentSet.MaxEnquiry = AgentCat.MaxEnquiry;
                                AgentSet.MaxSuspend = AgentCat.MaxSuspend;
                                AgentSet.SuspendDuration = AgentCat.SuspendDuration;
                                AgentSet.LoginName = AgentSet.AgentName;
                                AgentSet.Password = agProfileInfo.Password;
                                AgentSet.OrganizationName = OrganizationName;
                                AgentSet.OrganicationID = OriganicationID;
                                AgentSet.Email = agProfileInfo.Email;
                                AgentSet.Contact = agProfileInfo.MobileNo;
                                if (Session["AGCurrencyCode"] != null && Session["AGCurrencyCode"].ToString() != "")
                                {
                                    AgentSet.Currency = Session["AGCurrencyCode"].ToString();
                                }
                                else
                                {
                                    Country_Info CT = new Country_Info();
                                    CT = objGeneral.GetCountryInfoByCode(agProfileInfo.Country, "");
                                    if (CT != null && CT.CurrencyCode != "")
                                    {
                                        AgentSet.Currency = CT.CurrencyCode;
                                        //GetAGCredit(AgentSet.Currency);
                                    }
                                }

                                Session["AgentSet"] = AgentSet;

                                return 100;//success
                            }
                            else
                            {
                                return 104;
                            }
                        }
                    }
                    else
                    {
                        //find agent
                        //Agent_Manage agentAPI = new Agent_Manage();

                        //Agent_Manage agentAPI = new Agent_Manage();


                        //ABS.Navitaire.AgentManager.FindAgentsResponse agentresp = apiAgent.NewFindingAgent(Username, signature);


                        ABS.Navitaire.PersonManager.GetPersonResponse agentresp = new ABS.Navitaire.PersonManager.GetPersonResponse();
                        using (profiler.Step("Navitaire:GetPerson"))
                        {
                            PersonResp = apiAgent.GetPersonByID(Username, signature, AgentID);
                        }


                        if (PersonResp != null)
                        {
                            agProfileInfo.AgentCatgID = "99"; //default for skyagent
                            //agProfileInfo.AgentID = agentresp.FindAgentResponseData.FindAgentList[0].AgentID.ToString();
                            //agProfileInfo.ContactFirstName = agentresp.FindAgentResponseData.FindAgentList[0].Name.FirstName;
                            //agProfileInfo.ContactLastName = agentresp.FindAgentResponseData.FindAgentList[0].Name.LastName;

                            agProfileInfo.AgentID = AgentID;
                            agProfileInfo.ContactFirstName = PersonResp.Person.PersonNameList[0].Name.FirstName;
                            agProfileInfo.ContactLastName = PersonResp.Person.PersonNameList[0].Name.LastName;


                            //ABS.Navitaire.PersonManager.GetPersonResponse PersonResp = new ABS.Navitaire.PersonManager.GetPersonResponse();
                            //PersonResp = apiAgent.GetPersonByID(Username, signature);
                            //if (PersonResp != null)
                            //{
                            //    if (PersonResp.Person.PersonEMailList.Length > 0)
                            //    {
                            //        agProfileInfo.Email = PersonResp.Person.PersonEMailList[0].EMailAddress;
                            //    }
                            //    if (PersonResp.Person.PersonPhoneList.Length > 0)
                            //    {
                            //        agProfileInfo.PhoneNo = PersonResp.Person.PersonPhoneList[0].Number;
                            //    }
                            //    if (PersonResp.Person.PersonAddressList.Length > 0)
                            //    {
                            //        agProfileInfo.Address1 = PersonResp.Person.PersonAddressList[0].Address.AddressLine1;
                            //        agProfileInfo.Address2 = PersonResp.Person.PersonAddressList[0].Address.AddressLine2;
                            //        agProfileInfo.City = PersonResp.Person.PersonAddressList[0].Address.City;
                            //        agProfileInfo.State = PersonResp.Person.PersonAddressList[0].State.ToString();
                            //    }
                            //    if (PersonResp.Person.PersonContactList.Length > 0)
                            //    {
                            //        agProfileInfo.Title = PersonResp.Person.PersonContactList[0].Title;
                            //        agProfileInfo.Postcode = PersonResp.Person.PersonContactList[0].PostalCode;
                            //        agProfileInfo.MobileNo = PersonResp.Person.PersonContactList[0].WorkPhone;
                            //        agProfileInfo.Country = PersonResp.Person.PersonContactList[0].CountryCode;
                            //        agProfileInfo.Fax = PersonResp.Person.PersonContactList[0].Fax;
                            //    }
                            //}
                            agProfileInfo.Flag = 0;
                            agProfileInfo.JoinDate = DateTime.Now;
                            agProfileInfo.LastModifyDate = DateTime.Now;
                            agProfileInfo.LastSyncBy = Username; ;
                            agProfileInfo.LicenseNo = "SkyAgent";
                            agProfileInfo.Username = Username;
                            agProfileInfo.Password = "";
                            agProfileInfo.Status = 1;
                            agProfileInfo.SyncCreate = DateTime.Now;
                            agProfileInfo.SyncLastUpd = DateTime.Now;

                            //20170411 - Sienny (2 fields added to ag_profile)
                            agProfileInfo.OrgID = Session["OrganizationCode"].ToString();
                            agProfileInfo.OrgName = Session["OrganizationName"].ToString();

                            agProfileInfo.Address3 = Session["CountryCode"].ToString(); //20170510 - Sienny (to store country code)

                        }
                        //sky agent still not exist on database

                        agProfileInfo = objAgentProfile.SaveAgentProfile(agProfileInfo, null, ABS.Logic.GroupBooking.Agent.AgentControl.EnumSaveType.Insert);
                        if (agProfileInfo != null)
                        {
                            AgentSet.AgentID = agProfileInfo.AgentID;
                            AgentSet.AgentCategoryID = agProfileInfo.AgentCatgID;
                            AgentSet.AgentName = agProfileInfo.Username;
                            AgentSet.AgentType = ABS.Logic.Shared.EnumAgentType.SkyAgent;
                            AgentCat = objAgent.GetAgentCategory(AgentSet.AgentCategoryID);
                            AgentSet.BlacklistDuration = AgentCat.BlacklistDuration;
                            AgentSet.CounterTimer = AgentCat.CounterTimer;
                            AgentSet.MaxEnquiry = AgentCat.MaxEnquiry;
                            AgentSet.MaxSuspend = AgentCat.MaxSuspend;
                            AgentSet.SuspendDuration = AgentCat.SuspendDuration;
                            AgentSet.LoginName = AgentSet.AgentName;
                            AgentSet.OrganizationName = OrganizationName;
                            AgentSet.OrganicationID = OriganicationID;
                            AgentSet.Email = agProfileInfo.Email;
                            AgentSet.Contact = agProfileInfo.MobileNo;
                            if (Session["AGCurrencyCode"] != null && Session["AGCurrencyCode"] != "")
                            {
                                AgentSet.Currency = Session["AGCurrencyCode"].ToString();
                            }

                            Session["AgentSet"] = AgentSet;
                            return 100;//success
                        }
                        else
                        {
                            return 104;
                        }
                    }

                }
                else
                {
                    return 102;//invalid username or password
                }
            }
            catch (Exception ex)
            {
                return 102;//invalid username or password
            }
        }

        private string SaveNewSkyAgent(string Username, string AgentID)
        {
            var profiler = MiniProfiler.Current;
            try
            {

                string psDomain = "";
                string psName = "";
                string psPwd = "";

                ABS.Navitaire.APIBooking apiNavitaire = new ABS.Navitaire.APIBooking("");
                ABS.Navitaire.APIAgent apiAgent = new ABS.Navitaire.APIAgent();
                psDomain = ConfigurationManager.AppSettings["signature_domain"].ToString();
                psName = ConfigurationManager.AppSettings["signature_username"].ToString();
                psPwd = ConfigurationManager.AppSettings["signature_password"].ToString();

                string signature = "";// apiNavitaire.AgentLogon("Public", psDomain, psName, psPwd);
                using (profiler.Step("Navitaire:AgentLogon"))
                {
                    signature = apiNavitaire.AgentLogon("Public", psDomain, psName, psPwd);
                }
                //string signature = apiNavitaire.AgentLogon("SkyAgent", psDomain, psName, txtPassword.Text);
                if (signature != "")
                {
                    //signature = SessionManager.Logon();//
                    //Session["LoginName"] = Username;
                    //Session["LoginType"] = "SkyAgent";
                    //Session["LoginPWD"] = "";
                    //Session["LoginDomain"] = "EXT";
                    //if (Request.Cookies["cookieLogin"] == null)
                    //{
                    //    HttpCookie cookie = new HttpCookie("cookieLoginName");
                    //    cookie.HttpOnly = true;
                    //    cookie.Values.Add("LoginName", Username);
                    //    cookie.Values.Add("PWD", "");
                    //    cookie.Values.Add("Domain", psDomain);
                    //    Response.AppendCookie(cookie);
                    //}

                    ABS.Navitaire.PersonManager.GetPersonResponse PersonResp = new ABS.Navitaire.PersonManager.GetPersonResponse();
                    using (profiler.Step("Navitaire:GetPerson"))
                    {
                        PersonResp = apiAgent.GetPersonByID(Username, signature, AgentID);
                    }
                    //string str = GetXMLString(PersonResp);

                    if (objAgentProfile.CheckRecordExist(Username) == 1)
                    {
                        agProfileInfo = objAgentProfile.GetSingleAgentProfile(Username);
                    }

                    if (PersonResp != null)
                    {
                        if (PersonResp.Person.PersonNameList.Length > 0)
                        {
                            agProfileInfo.Title = PersonResp.Person.PersonNameList[0].Name.Title;
                        }

                        if (PersonResp.Person.PersonEMailList.Length > 0)
                        {
                            //Added by ketee, enhance pulling correct default email address from agent account , 20170622
                            for (int i = 0; i < PersonResp.Person.PersonEMailList.Length; i++)
                            {
                                if (PersonResp.Person.PersonEMailList[i].Default == true)
                                {
                                    agProfileInfo.Email = PersonResp.Person.PersonEMailList[i].EMailAddress;
                                }
                            }
                        }
                        if (PersonResp.Person.PersonPhoneList.Length > 0)
                        {
                            agProfileInfo.PhoneNo = PersonResp.Person.PersonPhoneList[0].Number;
                        }
                        if (PersonResp.Person.PersonAddressList.Length > 0)
                        {
                            agProfileInfo.Address1 = PersonResp.Person.PersonAddressList[0].Address.AddressLine1;
                            agProfileInfo.Address2 = PersonResp.Person.PersonAddressList[0].Address.AddressLine2;
                            agProfileInfo.City = PersonResp.Person.PersonAddressList[0].Address.City;
                            agProfileInfo.State = PersonResp.Person.PersonAddressList[0].Address.ProvinceState;
                            agProfileInfo.Country = PersonResp.Person.PersonAddressList[0].Address.CountryCode;
                            agProfileInfo.Postcode = PersonResp.Person.PersonAddressList[0].Address.PostalCode;
                        }
                        if (PersonResp.Person.PersonContactList.Length > 0)
                        {
                            agProfileInfo.Title = PersonResp.Person.PersonContactList[0].Title;
                            agProfileInfo.Postcode = PersonResp.Person.PersonContactList[0].PostalCode;
                            agProfileInfo.MobileNo = PersonResp.Person.PersonContactList[0].WorkPhone;
                            agProfileInfo.Country = PersonResp.Person.PersonContactList[0].CountryCode;
                            agProfileInfo.Fax = PersonResp.Person.PersonContactList[0].Fax;
                        }

                        if (PersonResp.Person.PersonPhoneList.Length > 0)
                        {
                            if (PersonResp.Person.PersonPhoneList[0].Number != "")
                            {
                                agProfileInfo.MobileNo = PersonResp.Person.PersonPhoneList[0].Number;
                            }
                            else
                            {
                                agProfileInfo.MobileNo = PersonResp.Person.PersonPhoneList[0].PhoneCode;
                            }
                        }

                        //20170411 - Sienny (2 fields added to ag_profile)
                        agProfileInfo.OrgID = Session["OrganizationCode"].ToString();
                        agProfileInfo.OrgName = Session["OrganizationName"].ToString();

                        agProfileInfo.Address3 = Session["CountryCode"].ToString(); //20170510 - Sienny (to store country code)

                    }
                    else
                    {
                        return "";
                    }

                    //check exist on database

                    if (objAgentProfile.CheckRecordExist(Username) == 1)
                    {
                        if (agProfileInfo != null)
                        {
                            AgentSet.AgentID = agProfileInfo.AgentID;
                            AgentSet.AgentCategoryID = agProfileInfo.AgentCatgID;
                            AgentSet.AgentName = agProfileInfo.Username;
                            AgentSet.Password = agProfileInfo.Password;
                            AgentSet.AgentType = ABS.Logic.Shared.EnumAgentType.SkyAgent;
                        }

                        agProfileInfo.SyncLastUpd = DateTime.Now;
                        agProfileInfo.LastSyncBy = agProfileInfo.AgentID;
                        agProfileInfo.LastModifyDate = DateTime.Now;
                        agProfileInfo = objAgentProfile.SaveAgentProfile(agProfileInfo, null, ABS.Logic.GroupBooking.Agent.AgentControl.EnumSaveType.Update);

                        //check blacklist
                        if (objAgent.VerifyBlackList(AgentSet.AgentID) == true)
                        {
                            return "";//blacklisted
                        }
                        else
                        {
                            if (agProfileInfo != null)
                            {
                                AgentCat = objAgent.GetAgentCategory(AgentSet.AgentCategoryID);
                                AgentSet.BlacklistDuration = AgentCat.BlacklistDuration;
                                AgentSet.CounterTimer = AgentCat.CounterTimer;
                                AgentSet.MaxEnquiry = AgentCat.MaxEnquiry;
                                AgentSet.MaxSuspend = AgentCat.MaxSuspend;
                                AgentSet.SuspendDuration = AgentCat.SuspendDuration;
                                AgentSet.LoginName = AgentSet.AgentName;
                                AgentSet.Password = agProfileInfo.Password;
                                //Session["AgentSet"] = AgentSet;

                                return AgentSet.AgentID;//success
                            }
                            else
                            {
                                return "";
                            }
                        }
                    }
                    else
                    {
                        //find agent
                        //ABS.Navitaire.AgentManager.FindAgentsResponse agentresp = apiAgent.NewFindingAgent(Username, signature);

                        ABS.Navitaire.PersonManager.GetPersonResponse agentresp = new ABS.Navitaire.PersonManager.GetPersonResponse();
                        using (profiler.Step("Navitaire:GetPerson"))
                        {
                            PersonResp = apiAgent.GetPersonByID(Username, signature, AgentID);
                        }
                        //agentresp = apiAgent.GetPersonByID(Username, signature, AgentID);

                        if (agentresp != null)
                        {
                            agProfileInfo.AgentCatgID = "99"; //default for skyagent
                            //agProfileInfo.AgentID = agentresp.FindAgentResponseData.FindAgentList[0].AgentID.ToString();
                            //agProfileInfo.ContactFirstName = agentresp.FindAgentResponseData.FindAgentList[0].Name.FirstName;
                            //agProfileInfo.ContactLastName = agentresp.FindAgentResponseData.FindAgentList[0].Name.LastName;

                            agProfileInfo.AgentID = AgentID;
                            agProfileInfo.ContactFirstName = agentresp.Person.PersonNameList[0].Name.FirstName;
                            agProfileInfo.ContactLastName = agentresp.Person.PersonNameList[0].Name.LastName;

                            agProfileInfo.Flag = 0;
                            agProfileInfo.JoinDate = DateTime.Now;
                            agProfileInfo.LastModifyDate = DateTime.Now;
                            agProfileInfo.LastSyncBy = Username; ;
                            agProfileInfo.LicenseNo = "SkyAgent";
                            agProfileInfo.Username = Username;
                            agProfileInfo.Password = "";
                            agProfileInfo.Status = 1;
                            agProfileInfo.SyncCreate = DateTime.Now;
                            agProfileInfo.SyncLastUpd = DateTime.Now;

                            //20170411 - Sienny (2 fields added to ag_profile)
                            agProfileInfo.OrgID = Session["OrganizationCode"].ToString();
                            agProfileInfo.OrgName = Session["OrganizationName"].ToString();

                            agProfileInfo.Address3 = Session["CountryCode"].ToString(); //20170510 - Sienny (to store country code)

                        }
                        //sky agent still not exist on database

                        agProfileInfo = objAgentProfile.SaveAgentProfile(agProfileInfo, null, ABS.Logic.GroupBooking.Agent.AgentControl.EnumSaveType.Insert);
                        if (agProfileInfo != null)
                        {
                            AgentSet.AgentID = agProfileInfo.AgentID;
                            AgentSet.AgentCategoryID = agProfileInfo.AgentCatgID;
                            AgentSet.AgentName = agProfileInfo.Username;
                            AgentSet.AgentType = ABS.Logic.Shared.EnumAgentType.SkyAgent;
                            AgentCat = objAgent.GetAgentCategory(AgentSet.AgentCategoryID);
                            AgentSet.BlacklistDuration = AgentCat.BlacklistDuration;
                            AgentSet.CounterTimer = AgentCat.CounterTimer;
                            AgentSet.MaxEnquiry = AgentCat.MaxEnquiry;
                            AgentSet.MaxSuspend = AgentCat.MaxSuspend;
                            AgentSet.SuspendDuration = AgentCat.SuspendDuration;
                            AgentSet.LoginName = AgentSet.AgentName;
                            //Session["AgentSet"] = AgentSet;
                            return AgentSet.AgentID;//success
                        }
                        else
                        {
                            return "";
                        }
                    }

                }
                else
                {
                    return "";//invalid username or password
                }
            }
            catch
            {
                return "";//invalid username or password
            }
        }

        protected void LoadNewAgentListIntoBlackList()
        {
            String NewAgent = "AAAAKASHWA;AAAAMTSHWA;AAAGPAMHWA;AAAIRBTHWA;AAAJHSBHWA;AAALNILEWA;AAALDYMHWA;AAAMATNHWA;AAANGTVLWA;AARAUDHAWA;AAASTANHWA;AABCCTVHWA;AAAMZATHWA;AACSHLSHWA;AACANTUHWA;AACATTVLWA;AACOMETHWA;AACREATHWA;AADAUNTTWA;AADELIVEWA;AADESTIHWA;AADYNTTCWA;AAEMZEDTWA;AAEXETTHWA;AAFALRTHWA;AAFARETHWA;AAFAZAHHWA;AAFBTRVLWA;AAFELDAWA;AAGEMTTHWA;AAGMLTTHWA;AAGOTZTHWA;AAGREENHWA;AAHANATHWA;AAHAYITHWA;AALEIEVEWA;AAHITTSHWA;AACHINTHWA;AAIMGTTHWA;AAINAYAHWA;AAINNOTHWA;AAINTCSHWA;AAJAGUNGWA;AAJASMIHWA;AAJETHOLWA;AAJJMYYWA;AAKAATTWA;AAKOPTTWA;AAKOPWIPWA;AAKSBTRVWA;AALEADERWA;AALIMORAWA;AALIMRAHWA;AALTSTSHWA;AAMRTTSHWA;AAMAGBTTWA;AAMTBSBHWA;AAMARSLHWA;AAMASERTWA;AAMASTMHWA;AAMAWARHWA;AAMIRITHWA;AAMITRAHWA;AAMTACAHWA;AAMYWAYHWA;AANADHAWA;AANMGTTHWA;AANOBLEHWA;AANUSDISWA;AAOLYMHHWA;AAORIENHWA;AAPANCUHWA;AAPEDATIWA;AADIKARIWA;AAPLGMDHWA;AAPENSTVWA;AAPERMAHWA;AAPROTTHWA;AARAMTTHWA;AARAJATVWA;AARAJATHWA;AARAKYATWA;AARAYHAHWA;AAREDHOLWA;AAGLOBALWA;AASMSTTHWA;AASENETHWA;AASHEHTHWA;AASILATTWA;AASITBTUWA;AASITSBWWA;AASMARTJWA;AASMARTHWA;AASMASTHWA;AACOMMTVWA;AAKEDAWAWA;AASRISTHWA;AASRITVLWA;AASRIHSHWA;AASTRTHWA;AASUNTRHWA;AATAJACOWA;AATHHATAWA;AATHAJTTWA;AATITMHOWA;AATDYNAHWA;AATRIDTHWA;AATRITNHWA;AATPHTTHWA;AATWENTYWA;AAUNITRVWA;AAUTUSANWA;AAVERSUSWA;AAWANTSBWA;AAYGMBTHWA;AAYHATTHWA;AAZAFCOTWA;AAZASSTHWA;AAALITENWA;AAENESTYWA;AAFAMETHWA;AAMIASTHWA;AAMSRPTHWA;AAPKPJMHWA;AAPUTRIHWA;AARAHATHWA;AASHAMTHWA;AATAMARHWA;AATIRAMHWA;AAUCTVLTWA;AAASTTFHWA;AAVCSAKLWA;AAMMGOLDWA;AAFBTSBHWA;AAHONEYHWA;AALAHATHWA;AAPOTOTHWA;AAPRESTHWA;AATLMTTHWA;AAVIPAORWA;AATUMPUHWA;AASTEAMHWA;AAARAHWA;AAALTISHWA;AAHYDRATWA;AAGLOINTWA;AATOUCHTWA;AARAMZAHWA;AAPLAKULWA;AAADAMTHWA;AAAWANTHWA;AAEPACLTWA;AAJUARATWA;AATDKTTHWA;AASMIQTAWA;AATRAVERWA;AAFCTLBUWA;AAKAYAHOWA;AANATTSBWA;AASKYMTSWA;AAZUSBWLWA;AACOLKULWA;AAKOPKULWA;AAMHLKULWA;AATOEVNEWA;AATMTKULWA;AASAKAPJWA;AAASTKULWA;AADESTINWA;AAREJBKIWA;AAGREATTWA;AAUMRKULWA;AAZAUSELWA;AAQUDKCHWA;AALVAKULWA;AAPVCSELWA;AASAFWATWA;AAMATBKIWA;AATOURSBWA;AAPASSAHWA;AAIEDKULWA;AAJRFSELWA;AAMUKAHTWA;AABTTSBHWA;AADINSELWA;AAIBTKULWA;AATAMSELWA;AAUNIUUMWA;AALEIEVMWA;AASOUTKLWA;AAFIVEKLWA;AAQUDSKBWA;AAASIARSWA;AAIRNAKLWA;AARIVERSWA;AAIMTIKLWA;AACITRKBWA;AATIMESWA;AAPHPKLHWA;AAWISDOMWA;AASHEBAPWA;AAHNZSELWA;AAHIJNSEWA;AAANNASHWA;AAKLIATRWA;AABTTKBRWA;AASUBKULWA;AATAJETHWA;AAAATKULWA;AACCTSELWA;AAASPIREWA;AAASHKULWA;AALYSYAWA;AATETPHGWA;AAULTMPPWA;AABMNSELWA;AASTBSELWA;AAAMZSELWA;AAJTSKCHWA;AAUPSITHWA;AAIPROWA;AARBKULWA;AANURSELWA;AARAMZAWA;AAZENITHWA;AASMZKULWA;AAMYPRWA;AAZAMHLDWA;AAMKMWA;AATARIEWA;AARNSKULWA;AATRANSWA;AAKINGTNWA;AAABBTHWA;AAMETROWA;AAAIRUSJWA;AAPELMERWA;AAHUASWKWA;AAAIDILHWA;AAIPHATTWA;AAUTASMZWA;AAUTASTTWA;AACITYTGWA;AASAYBTUWA;AASAYUKCWA;AASAYUKLWA;AASAYUTHWA;AASYUBKIWA;AASYULBUWA;AASTGGTHWA;AASYUPHAWA;AATHTKCHWA;AATHTPENWA;AATRVLTHWA;AADSDKHWA;AADREAMHWA;AAINTERWA;AAINTMYYWA;AAINTBTUWA";
            NewAgent = "AAALTISHWA;AAHYDRATWA;AAGLOINTWA;AATOUCHTWA;AARAMZAHWA;AAPLAKULWA;AAADAMTHWA;AAAWANTHWA;AAEPACLTWA;AAJUARATWA;AATDKTTHWA;AASMIQTAWA;AATRAVERWA;AAFCTLBUWA;AAKAYAHOWA;AANATTSBWA;AASKYMTSWA;AAZUSBWLWA;AACOLKULWA;AAKOPKULWA;AAMHLKULWA;AATOEVNEWA;AATMTKULWA;AASAKAPJWA;AAASTKULWA;AADESTINWA;AAREJBKIWA;AAGREATTWA;AAUMRKULWA;AAZAUSELWA;AAQUDKCHWA;AALVAKULWA;AAPVCSELWA;AASAFWATWA;AAMATBKIWA;AATOURSBWA;AAPASSAHWA;AAIEDKULWA;AAJRFSELWA;AAMUKAHTWA;AABTTSBHWA;AADINSELWA;AAIBTKULWA;AATAMSELWA;AAUNIUUMWA;AALEIEVMWA;AASOUTKLWA;AAFIVEKLWA;AAQUDSKBWA;AAASIARSWA;AAIRNAKLWA;AARIVERSWA;AAIMTIKLWA;AACITRKBWA;AATIMESWA;AAPHPKLHWA;AAWISDOMWA;AASHEBAPWA;AAHNZSELWA;AAHIJNSEWA;AAANNASHWA;AAKLIATRWA;AABTTKBRWA;AASUBKULWA;AATAJETHWA;AAAATKULWA;AACCTSELWA;AAASPIREWA;AAASHKULWA;AALYSYAWA;AATETPHGWA;AAULTMPPWA;AABMNSELWA;AASTBSELWA;AAAMZSELWA;AAJTSKCHWA;AAUPSITHWA;AAIPROWA;AARBKULWA;AANURSELWA;AARAMZAWA;AAZENITHWA;AASMZKULWA;AAMYPRWA;AAZAMHLDWA;AAMKMWA;AATARIEWA;AARNSKULWA;AATRANSWA;AAKINGTNWA;AAABBTHWA;AAMETROWA;AAAIRUSJWA;AAPELMERWA;AAHUASWKWA;AAAIDILHWA;AAIPHATTWA;AAUTASMZWA;AAUTASTTWA;AACITYTGWA;AASAYBTUWA;AASAYUKCWA;AASAYUKLWA;AASAYUTHWA;AASYUBKIWA;AASYULBUWA;AASTGGTHWA;AASYUPHAWA;AATHTKCHWA;AATHTPENWA;AATRVLTHWA;AADSDKHWA;AADREAMHWA;AAINTERWA;AAINTMYYWA;AAINTBTUWA";
            String Success = "";
            string Fail = "";
            string AgentID = "";
            try
            {
                if (AgentSet.AgentID == "9199210")
                {

                    String[] arr = NewAgent.ToString().Split(';');
                    foreach (string a in arr)
                    {
                        AgentID = SaveNewSkyAgent(a.ToString().Trim(), AgentSet.AgentID);
                        if (AgentID != "")
                        {
                            if (Success == "")
                                Success = a;
                            else
                                Success += "," + a;

                            if (SetBlacklist(AgentID))
                            {
                                Success += "-" + AgentID;
                            }
                            else
                            {
                                Success += "-None";
                            }
                        }
                        else
                        {
                            if (Fail == "")
                                Fail = a;
                            else
                                Fail += "," + a;
                        }
                    }
                    log.Info(this, "LoadNewAgentBlackList - success: " + Success + " ~ Fail: " + Fail);
                }
            }
            catch (Exception ex)
            {
                log.Error(this, ex, "Success Agent:" + Success + " ; Fail Agent:" + Fail);
            }
        }

        protected Boolean SetBlacklist(string AgentID)
        {
            ABS.Logic.GroupBooking.Agent.RequestApp ReqInfoAgent = new ABS.Logic.GroupBooking.Agent.RequestApp();
            AgentBlacklistLog agBlacklistInfo = new AgentBlacklistLog();
            AgentActivity agActivityInfo = new AgentActivity();
            string blacklistID = "";
            //blacklistID = objAgent.CheckBlacklistExist(Session["TransReqTransID"].ToString());
            ReqInfoAgent.Remark = "Blacklisted Warrant Agent";
            ReqInfoAgent.ReqType = "B";
            ReqInfoAgent.LastSyncBy = "1001";
            ReqInfoAgent.ApprovedBy = "1001";
            ReqInfoAgent.ApprovedDate = DateTime.Now;
            ReqInfoAgent.ReqID = DateTime.Now.ToString("yyyyMMddHHmmsss");
            ReqInfoAgent.TransID = AgentID;
            ReqInfoAgent.RequestDate = DateTime.Now;
            ReqInfoAgent.ExpiryDate = DateTime.Now.AddYears(Convert.ToInt16(99));
            ReqInfoAgent.UserID = "1001";

            //insert new blacklist
            blacklistID = DateTime.Now.ToString("yyyyMMddHHmmsss");
            agBlacklistInfo.AgentID = AgentID;
            agBlacklistInfo.BlacklistDate = DateTime.Now;
            agBlacklistInfo.BlacklistExpiryDate = DateTime.Now.AddYears(Convert.ToInt16(99));
            agBlacklistInfo.BlacklistID = blacklistID;
            agBlacklistInfo.LastSyncBy = "1001";
            agBlacklistInfo.Status = 1;
            agBlacklistInfo.SyncCreate = DateTime.Now;
            agBlacklistInfo.SyncLastUpd = DateTime.Now;
            agBlacklistInfo.Remark = "Blacklisted by Admin for Warrant Agent";
            agActivityInfo.AgentID = AgentID;
            agActivityInfo.LastBlacklist = agBlacklistInfo.BlacklistDate;
            agActivityInfo.LastFailedLoginAttempt = 0;
            agActivityInfo.TotalFailedLoginAttempt = 0;
            agActivityInfo.LastFailedLoginDate = DateTime.Now;
            agActivityInfo.ExpiryBlacklistDate = agBlacklistInfo.BlacklistExpiryDate;
            agActivityInfo.LastLoginDate = DateTime.Now;

            agActivityInfo.SyncLastUpd = DateTime.Now;
            agBlacklistInfo = objAgent.SaveNewBlacklistApprove(agBlacklistInfo, agActivityInfo, ReqInfoAgent, ABS.Logic.GroupBooking.Agent.AgentControl.EnumSaveType.Insert);
            if (agBlacklistInfo == null || agBlacklistInfo.AgentID != AgentID)
            {
                return false;
            }
            return true;
        }

        //private string GetXMLString(object Obj) 
        //{
        //    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Obj.GetType());
        //    System.IO.StringWriter writer = new System.IO.StringWriter();
        //    x.Serialize(writer, Obj);

        //    return writer.ToString();
        //}
    }
}