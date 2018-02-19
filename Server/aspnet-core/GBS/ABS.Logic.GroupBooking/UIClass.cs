using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ABS.Logic.GroupBooking.Agent;
using ABS.Logic.GroupBooking;
using System.Data;
using System.Web.UI.WebControls;
/// <summary>
/// Summary description for Class1
/// </summary>
public class UIClass
{
    public enum EnumDefineStyle : int
    {
        Style1 = 1,
        Style2 = 2,
        Country = 3,
        State = 4,
        City = 5,
        Opt = 6,
        CountryCard = 7,
        Years = 8,
        YearsPlus = 9,
        AgentCategory= 10
    }
    public UIClass()
    {
    }
    public static bool SetComboStyle(ref DevExpress.Web.ASPxComboBox Combo, EnumDefineStyle DefineStyle,
    string DefaultValue = "", string Criteria = "", bool AddEmptyRow = false)
    {
        //return false;
        int[] aryColWidth = null;
        //Combo. = false;
        //Combo.LimitToList = true;
        //Combo.DropMode = C1List.DropModeEnum.Automatic;
        //Combo.AutoCompletion = true;
        //Combo.AutoDropDown = true;
        //Combo.ExtendRightColumn = true;
        //Combo.DropdownPosition = C1List.DropdownPositionEnum.RightDown;
        GeneralControl CountryBase = new GeneralControl();
        AgentControl objAgent = new AgentControl();
        Combo.DropDownWidth = 230;
        List<Country_Info> lstCountry = new List<Country_Info>();
        List<CODEMASTER> lstOpt = new List<CODEMASTER>();
        DataTable dataCountry = new DataTable();

        switch (DefineStyle)
        {

            case EnumDefineStyle.Style1:
                Combo.DropDownWidth = 0;
                //Combo.ColumnHeaders = false;
                //Combo.LimitToList = true;
                //Combo.DropMode = C1List.DropModeEnum.Automatic;
                //Combo.AutoCompletion = true;
                //Combo.AutoDropDown = true;
                //Combo.ExtendRightColumn = true;
                //Combo.DropdownPosition = C1List.DropdownPositionEnum.RightDown;
                return true;
            case EnumDefineStyle.Country:
                aryColWidth = new int[2];
                aryColWidth[0] = 30;
                aryColWidth[1] = 0;
                //SetComboProp(Combo, objSelect.CustomSelect(EnumCustomSelect.Country,, EnumTypeOrder.ByDesc), DefaultValue,, EnumComboStyle.DescOnly, aryColWidth);                
                lstCountry = CountryBase.GetAllCountry();
                //foreach (GroupBooking.Info.Agent_Info objStock in lstCountry)
                //{           
                Combo.Items.Clear();
                Combo.Items.Add("", "");
                Combo.TextField = "countryName";
                Combo.ValueField = "countrycode";
                Combo.DataSource = lstCountry;
                Combo.DataBind();
                Combo.DropDownWidth = 280;
                Combo.SelectedIndex = 0;
                //aryColWidth = null;
                return true;
            case EnumDefineStyle.State:
                aryColWidth = new int[2];
                aryColWidth[0] = 30;
                aryColWidth[1] = 0;
                //SetComboProp(Combo, objSelect.CustomSelect(EnumCustomSelect.Country,, EnumTypeOrder.ByDesc), DefaultValue,, EnumComboStyle.DescOnly, aryColWidth);                
                lstCountry = CountryBase.GetAllState(Criteria);
                //foreach (GroupBooking.Info.Agent_Info objStock in lstCountry)
                //{           
                Combo.Items.Clear();
                Combo.Items.Add("", "");
                Combo.TextField = "provinceStateName";
                Combo.ValueField = "provincestatecode";
                Combo.DataSource = lstCountry;
                Combo.DataBind();
                Combo.DropDownWidth = 280;
                Combo.SelectedIndex = 0;
                //aryColWidth = null;
                return true;
            case EnumDefineStyle.Opt:
                //p(Combo, objSelect.CustomSelect(EnumCustomSelect.State, Criteria, EnumTypeOrder.ByDesc),,, EnumComboStyle.DescOnly);
                //lstOpt = CountryBase.GetAllCODEMASTERFilter();
                Combo.Items.Clear();
                Combo.TextField = "CodeDesc";
                Combo.ValueField = "Code";
                Combo.DataSource = lstOpt;
                Combo.DataBind();
                Combo.DropDownWidth = 150;
                Combo.SelectedIndex = 0;
                return true;
            case EnumDefineStyle.CountryCard:
                aryColWidth = new int[2];
                aryColWidth[0] = 30;
                aryColWidth[1] = 0;
                //SetComboProp(Combo, objSelect.CustomSelect(EnumCustomSelect.Country,, EnumTypeOrder.ByDesc), DefaultValue,, EnumComboStyle.DescOnly, aryColWidth);                
                dataCountry = CountryBase.GetAllCountryCard();
                //foreach (GroupBooking.Info.Agent_Info objStock in lstCountry)
                //{           
                Combo.Items.Clear();
                Combo.Items.Add("", "");
                Combo.TextField = "Name";
                Combo.ValueField = "CountryCode";
                Combo.DataSource = dataCountry;
                Combo.DataBind();
                Combo.DropDownWidth = 280;
                Combo.SelectedIndex = 0;
                //aryColWidth = null;
                return true;
            case EnumDefineStyle.Years:
                Combo.Items.Clear();
                for (int i = 0; i < 20; i++)
                {
                    Combo.Items.Add((DateTime.Now.Year - i).ToString());
                }
                Combo.DropDownWidth = 50;
                //aryColWidth = null;
                return true;
            case EnumDefineStyle.YearsPlus:
                Combo.Items.Clear();
                for (int i = 0; i < 20; i++)
                {
                    Combo.Items.Add((DateTime.Now.Year + i).ToString());
                }
                Combo.DropDownWidth = 50;
                //aryColWidth = null;
                return true;
            case EnumDefineStyle.AgentCategory:
                DataTable dt = objAgent.GetAllAgentCategory();
                //foreach (GroupBooking.Info.Agent_Info objStock in lstCountry)
                //{           
                Combo.Items.Clear();
                Combo.TextField = "AgentCatgDesc";
                Combo.ValueField = "AgentCatgID";
                Combo.DataSource = dt;
                Combo.DataBind();
                Combo.DropDownWidth = 280;
                
                //aryColWidth = null;
                return true;

            default:
                return false;
        }
    }

   

    //if (AddEmptyRow)
    //{
    //    SetEmptyRow(Combo);
    //}

}