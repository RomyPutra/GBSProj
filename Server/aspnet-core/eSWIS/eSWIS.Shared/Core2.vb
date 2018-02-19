'Imports System.Windows.Forms
'Imports SEAL.Data.DataAccess
'Imports SEAL.Data.SQLControl
'Imports SEAL.Data
Imports System.Text
Imports System.Data
Imports System.Xml.Serialization
Imports System.Configuration
Imports System.Data.SqlClient

Namespace Core2
#Region "User defined enum"

    Public Enum EnumIsoState
        StateReadOnly = 0
        StateUpdatetable = 1
    End Enum

    Public Enum convert_
        percent = 10000
    End Enum

    ''Lily 20-05-2016
    'Enum TransType
    '    TransIn = 0
    '    TransOut = 1
    'End Enum

    ''Lily 20-05-2016
    'Enum SaveType
    '    Draft = 0
    '    Submit = 1
    '    Void = 2
    '    Draft_Transfer = 3
    '    Submit_Transfer = 4
    '    Void_Transfer = 5
    '    Draft_Receive = 6
    '    Draft_Reject = 7
    '    Submit_Receive = 8
    '    Submit_Reject = 9
    '    OnHold = 10
    '    'Void_Receive = 10
    '    Cancel_Receive = 11
    '    Cancel_submit = 12 'Suspend
    '    Draft_BackDate = 13
    '    Submit_BackDate = 14
    '    Cancel_BackDate = 15
    'End Enum

    Public Enum EnumSettings
        adBackEnd = 0
        adFrontEnd = 1
    End Enum

    Public Enum EnumCustomSelect
        Country = 0
        Currency = 1
        StockBrand = 2
        CustomerCategory = 3
        PrivilegeCode = 4
        State = 5
        Branch = 6
        CommissionScheme = 7
        Course = 8
        StockDept = 9
        Stock = 10
        Discount = 11
        StockCategory = 12
        UOM = 13
        Intake = 14
        UserGroup = 15
        DiscountGroup = 16
        CourseIntake = 17
        SchemeNo = 18
        EmpGroup = 19
        AccountType = 20
        AccountNo = 21
        Employee = 22
        CustomStock = 23
        CustomerAccount = 24
        TermID = 25
        Answer = 26
        PromoBatch = 27
        Supplier = 28
        Process = 29
        EmpService = 30
        EmpSales = 31
        SubGroup = 32
        RptBranch = 33
        EmpNickName = 34
        CityCode = 35
        TableNo = 36
        Template = 37
        Rouding = 38
        WarehouseSection = 39
        WarehouseArea = 40
        WarehouseRow = 41
        PaymentScheme = 42
        CustomerType = 43
        PlanID = 44
        WarehouseLevel = 45
        TemplateName = 46
        DataBase = 47
        BankCode = 48
        PaymentMethod = 49
        GLCode = 50
        SysFunction = 51
        SysModule = 52
        FileType = 53
        City = 54
        PBT = 55
        Area = 56
        Company = 57
        Location = 58
        LicenseCategory = 59
        Business = 60
        CompanyState = 61

    End Enum

    Public Enum EnumSelectType
        StockType = 0
        TaxType = 1
        TaxExInc = 2
        DiscType = 3
        TenderType = 4
        RemarkType = 5
        DirectoryType = 6
        TransType = 7
        Condition = 8
        MethodsType = 9
        SalesAdjust = 10
        StockTax = 11
        DuplicateType = 12
        TenderAdjust = 13
        Tender = 14
        EIS = 15
        AnswerGrp = 16
        PrefType = 17
        CommAdjust = 18
        Commission = 19
        StudentStatus = 20
        TransTypeTR = 21
        Month = 22
        PrefItem = 23
        CustomerID = 24
        Department = 25
        AdjType = 26
        MemberType = 27
        RoundType = 28
        Position = 29
        PaymentType = 30
        Sday = 31
        SYear = 32
        ControlType = 33
        TradeType = 34
        PlanID = 35
        PetBreed = 47
        PetType = 48
    End Enum

    Public Enum EnumOptionType
        Salulation = 0
        Sex = 1
        Nationality = 2
        Race = 3
        Religion = 4
        MaritalStatus = 5
        Second = 6
        RefInfo = 7
    End Enum

    Public Enum EnumSelectStatus
        Booking = 1
        Service = 2
        User = 6
        Block = 7
    End Enum

    Public Enum EnumTypeOrder
        ByCode = 0
        ByDesc = 1
        ByType = 2
    End Enum

    Public Enum EnumBuyerType
        Customer = 0
        Employee = 1
        Student = 78
    End Enum

    Public Enum EnumBehaviorType
        Normal = 0
        Combo = 1
        Package = 2
    End Enum

    Public Enum EnumRoundAmt
        RoundUp = 1
        RoundDown = 2
    End Enum

    Public Enum EnumEditStatus
        Update = 0
        Cancel = 1
    End Enum

#End Region

    Public Class CoreBase
        'Inherits SEAL.Model.Moyenne.CoreBase

        Protected Shared blnBranchFilter As Boolean
        Protected SavedValue As Object
        Protected Shared strSQL As String

        Public ReadOnly Property ReturnValue() As Object
            Get
                Return SavedValue
            End Get
        End Property

        Public Shared Property BranchFilter() As Boolean
            Get
                Return blnBranchFilter
            End Get
            Set(ByVal Value As Boolean)
                blnBranchFilter = Value
            End Set
        End Property

        'Public Shared Function SetBranchFilter(ByVal BranchID As String) As String
        '    Dim objSQL As SQLControl
        '    Dim strSQL As String = String.Empty
        '    Try
        '        objSQL = New SQLControl
        '        If BranchID.Trim.Length > 0 Then
        '            strSQL = String.Concat(" BranchID IN('", objSQL.ParseValue(EnumDataType.dtString, BranchID), "')")
        '        End If
        '        Return strSQL
        '        objSQL.Dispose()
        '    Catch errFilter As Exception
        '        Throw New ApplicationException(errFilter.Message)
        '        Return strSQL
        '    End Try
        '    ''If SQLStmt.IndexOf("WHERE") = -1 Then
        '    ''    SQLStmt = String.Concat(SQLStmt, " AND BranchID IN('" & BranchID & "')")
        '    ''Else
        '    ''    If SQLStmt.IndexOf("BranchID =") = -1 Then
        '    ''        SQLStmt = String.Concat(SQLStmt, " WHERE BranchID IN('" & BranchID & "')")
        '    ''    End If
        '    ''End If
        '    ''Return SQLStmt
        'End Function

        Public Shared Function ParseInString(ByVal Value As String) As String
            Dim arrIN() As String
            Dim i As Integer
            If Value.Trim = String.Empty Then
                Return "''"
            Else
                arrIN = Split(Value, ",")
                Value = String.Empty
                For i = 0 To arrIN.GetUpperBound(0)
                    If i > 0 Then
                        Value = String.Concat(Value, ", ")
                    End If
                    Value = String.Concat(Value, "'", arrIN(i).ToString, "'")
                Next
                arrIN = Nothing
                If Value.Trim = String.Empty Then
                    Return "''"
                Else
                    Return Value
                End If
            End If
        End Function

#Region "Data select"
        Public Shared Function CustomSelect(ByVal ConnString As String, ByVal CustomType As EnumCustomSelect, Optional ByVal CustomCriteria As String = Nothing, Optional ByVal CustomCriteria2 As String = Nothing, Optional ByVal CustomCriteria3 As String = Nothing, Optional ByVal TypeOrder As EnumTypeOrder = EnumTypeOrder.ByCode) As DataSet
            CustomSelect = Nothing
            Dim conn As CoreControl = New CoreControl()
            conn.ConnectionString = ConnString

            Select Case CustomType
                Case EnumCustomSelect.Business
                    strSQL = "SELECT SUBSICCode, SUBSICDesc FROM SUBSIC WHERE Active = 1 AND SICCode='" & Replace(CustomCriteria, "'", "''") & "'"
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY SUBSICDesc")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY SUBSICDesc")
                    End If
                Case EnumCustomSelect.Country
                    strSQL = "SELECT CountryCode, CountryDesc FROM Country WHERE Active = 1 AND Flag = 1"
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY CountryCode")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY CountryDesc")
                    End If
                    'strSQL = "SELECT LTRIM(RTRIM(Code)) AS Code, CodeDesc FROM CodeMaster WHERE CodeType = 'CC'"
                    'strSQL &= " ORDER BY CodeDesc, CodeRef, Code"
                Case EnumCustomSelect.State
                    'strSQL = "SELECT StateCode,StateDesc FROM State WHERE CountryCode = (SELECT CountryCode FROM Country WHERE CountryDesc='" & Replace(CustomCriteria, "'", "''") & "' AND Active = 1 AND Flag = 1) AND Active = 1 AND Flag = 1"
                    'If TypeOrder = EnumTypeOrder.ByCode Then
                    '    strSQL = String.Concat(strSQL, " ORDER BY StateCode")
                    'Else
                    '    strSQL = String.Concat(strSQL, " ORDER BY StateDesc")
                    'End If
                    strSQL = "SELECT StateCode,StateDesc FROM State WHERE CountryCode = '" & Replace(CustomCriteria, "'", "''") & "' AND Active = 1 AND Flag = 1"
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY StateCode")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY StateDesc")
                    End If
                    'strSQL = "SELECT LTRIM(RTRIM(Code)) AS Code, CodeDesc FROM CodeMaster WHERE CodeType = 'STA' AND CodeRef = (SELECT Code FROM CodeMaster WHERE CodeDesc='" & Replace(CustomCriteria, "'", "''") & "')"
                    'strSQL &= " ORDER BY CodeDesc, CodeRef, Code"
                Case EnumCustomSelect.PBT
                    strSQL = "SELECT PBTCode,PBTDesc FROM PBT WHERE CountryCode = '" & Replace(CustomCriteria, "'", "''") & "' AND StateCode = '" & Replace(CustomCriteria2, "'", "''") & "' AND Active = 1 AND Flag = 1"
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY PBTCode")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY PBTDesc")
                    End If
                Case EnumCustomSelect.City
                    strSQL = "SELECT CityCode,CityDesc FROM City WHERE CountryCode = '" & Replace(CustomCriteria, "'", "''") & "' AND StateCode = '" & Replace(CustomCriteria2, "'", "''") & "' AND Active = 1 AND Flag = 1"
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY CityCode")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY CityDesc")
                    End If
                Case EnumCustomSelect.Area
                    strSQL = "SELECT AreaCode,AreaDesc FROM Area WHERE CountryCode = '" & Replace(CustomCriteria, "'", "''") & "' AND StateCode = '" & Replace(CustomCriteria2, "'", "''") & "' AND CityCode = '" & Replace(CustomCriteria3, "'", "''") & "' AND Active = 1 AND Flag = 1"
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY AreaCode")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY AreaDesc")
                    End If
                Case EnumCustomSelect.Company
                    strSQL = "SELECT BizRegID, CompanyName FROM BIZENTITY WHERE BizRegID='" & Replace(CustomCriteria, "'", "''") & "' AND Active = 1 AND Flag = 1"
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY BizRegID")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY CompanyName")
                    End If
                Case EnumCustomSelect.CompanyState
                    strSQL = "SELECT BizRegID, CompanyName FROM BIZENTITY WHERE State='" & Replace(CustomCriteria, "'", "''") & "' AND Active = 1 AND Flag = 1"
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY BizRegID")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY CompanyName")
                    End If
                Case EnumCustomSelect.Location
                    If CustomCriteria Is Nothing Then
                        strSQL = "SELECT BizLocID, BranchName FROM BIZLOCATE WHERE Active = 1 AND Flag = 1 "
                    Else
                        strSQL = "SELECT BizLocID, BranchName FROM BIZLOCATE WHERE Active = 1 AND Flag = 1 AND BizRegID = '" & Replace(CustomCriteria, "'", "''") & "'"
                    End If
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY BizLocID")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY BranchName")
                    End If
                Case EnumCustomSelect.LicenseCategory
                    If CustomCriteria Is Nothing Then
                        strSQL = "SELECT LicenseCode, LicenseDesc FROM LICENSECATEGORY WHERE Active = 1 AND Flag = 1 "
                    Else
                        strSQL = "SELECT LicenseCode, LicenseDesc FROM LICENSECATEGORY WHERE Active = 1 AND Flag = 1 AND LicenseType = '" & Replace(CustomCriteria, "'", "''") & "'"

                    End If
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY LicenseCode")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY LicenseDesc")
                    End If
                Case EnumCustomSelect.Employee
                    strSQL = "SELECT EmployeeID, NickName FROM Employee WHERE Flag = 1"

                    If Not CustomCriteria Is Nothing Then
                        If strSQL.ToString().Trim() <> "" Then
                            strSQL &= CustomCriteria
                        Else
                            strSQL &= " AND CompanyID = ''"
                        End If
                    End If

                    If Not CustomCriteria2 Is Nothing Then
                        strSQL &= " AND CompanyID = '" & Replace(CustomCriteria2, "'", "''") & "'"
                    ElseIf Not CustomCriteria Is Nothing Then

                    End If

                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY EmployeeID")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY NickName")
                    End If
                    'Case EnumCustomSelect.City
                    '    strSQL = "SELECT LTRIM(RTRIM(Code)) AS Code, CodeDesc FROM CodeMaster WHERE CodeType = 'CTY' AND CodeRef = (SELECT TOP 1 Code FROM CodeMaster WHERE CodeDesc='" & Replace(CustomCriteria, "'", "''") & "')"
                    '    If CustomCriteria <> Nothing Then
                    '        strSQL &= " ORDER BY CodeDesc, CodeRef, Code"
                    '    End If
                Case EnumCustomSelect.Currency
                    strSQL = "SELECT CurrencyCode, CurrencyDesc FROM Currency WHERE Active = 1 AND Flag = 1"
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY CurrencyCode")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY CurrencyDesc")
                    End If
                Case EnumCustomSelect.StockDept
                    strSQL = "SELECT DEPTCODE, DEPTDESC FROM StkDepartment WHERE Active = 1 AND Flag = 1"
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY DEPTCODE")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY DEPTDESC")
                    End If
                Case EnumCustomSelect.StockCategory
                    strSQL = "SELECT STKCATEGORY.CatgCode, STKCATEGORY.CatgDesc, STKDEPARTMENT.DeptDesc, STKCATEGORY.DeptCode " &
                            "FROM STKCATEGORY INNER JOIN STKDEPARTMENT ON STKCATEGORY.DeptCode = STKDEPARTMENT.DeptCode " &
                            "WHERE STKCATEGORY.Active = 1 AND STKCATEGORY.Flag = 1"
                    If CustomCriteria <> String.Empty Then
                        strSQL = String.Concat(strSQL, " AND STKCATEGORY.DeptCode = '" & CustomCriteria & "'")
                    End If
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY STKCATEGORY.CatgCode")
                    ElseIf TypeOrder = EnumTypeOrder.ByType Then
                        strSQL = String.Concat(strSQL, " ORDER BY STKCATEGORY.DeptCode")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY STKCATEGORY.CatgDesc")
                    End If
                Case EnumCustomSelect.Stock
                    strSQL = "SELECT StkCode, StockDesc FROM STOCK WHERE Active = 1 AND Flag = 1 AND BranchID = '" & CustomCriteria & "'"
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY StkCode")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY StockDesc")
                    End If
                Case EnumCustomSelect.StockBrand
                    strSQL = "SELECT BrandCode, BrandDesc FROM STKBRAND WHERE Active=1"
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY BrandCode")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY BrandDesc")
                    End If
                Case EnumCustomSelect.CustomerCategory
                    strSQL = "SELECT cuscatid as CUSCATGID,cuscatdesc as DES, CASE privilegecode WHEN NULL THEN 'NO' ELSE 'YES' END AS PRIVILEGECODE FROM custcategory " &
                                "where active=1 and flag=1"
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY CUSCATGID")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY DES")
                    End If
                Case EnumCustomSelect.PrivilegeCode
                    strSQL = "SELECT PrivilegeCode, PrivilegeDesc FROM PRIVILEGE  WHERE PrivilegeType = '" & CustomCriteria & "' AND Flag = '1' AND Active = '1'"
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY PrivilegeCode")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY PrivilegeDesc")
                    End If
                Case EnumCustomSelect.Branch
                    strSQL = "SELECT BranchID, BranchName FROM SysBranch WHERE Flag=1"
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY BranchID")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY BranchName")
                    End If
                Case EnumCustomSelect.CommissionScheme
                    strSQL = "SELECT SchemeCode, SchemeDesc FROM COMMSCHEMEHDR WHERE Flag = 1"
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY SchemeCode")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY SchemeDesc")
                    End If

                Case EnumCustomSelect.Discount
                    strSQL = "SELECT DISCCODE, DISCDESC FROM DISCOUNT WHERE Active = 1 AND Flag = 1"
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY DISCCODE")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY DISCDESC")
                    End If
                Case EnumCustomSelect.UOM
                    strSQL = "SELECT UOMCODE, UOMDESC FROM UOM WHERE Active = 1 AND Flag = 1"
                    If CustomCriteria <> String.Empty Then
                        If IsNumeric(CustomCriteria) Then
                            strSQL = String.Concat(strSQL, " AND UOMGroup = ", CustomCriteria)
                        End If
                    End If
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY UOMCODE")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY UOMDESC")
                    End If
                Case EnumCustomSelect.Course
                    strSQL = "SELECT COURSEID, COURSENAME FROM AC_COURSE WHERE Active = 1 AND Flag = 1"
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY COURSEID")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY COURSENAME")
                    End If
                Case EnumCustomSelect.Intake
                    strSQL = "SELECT DISTINCT INTAKECODE, REMARK FROM AC_INTAKE WHERE Status = 1 AND FLAG = 1"
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY INTAKECODE")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY REMARK")
                    End If
                Case EnumCustomSelect.UserGroup
                    strSQL = "SELECT GroupCode, GroupName FROM USRGROUP WHERE Status=1 AND APPID =" & CustomCriteria
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY GroupCode")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY GroupName")
                    End If
                Case EnumCustomSelect.DiscountGroup
                    strSQL = "SELECT DiscGrp, DiscGrpDesc FROM DISCGROUP WHERE Flag = 1"
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY DiscGrp")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY DiscGrpDesc")
                    End If
                Case EnumCustomSelect.CourseIntake
                    strSQL = "SELECT IntakeCode, Remark FROM AC_INTAKE WHERE COURSEID ='" & Replace(CustomCriteria, "'", "''") & "' AND Flag = 1"
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY IntakeCode")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY Remark")
                    End If

                Case EnumCustomSelect.SchemeNo
                    strSQL = "SELECT SchemeNo, SchemeDesc FROM AC_PAYSCHEME WHERE " & CustomCriteria
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " Order BY SchemeNo")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY SchemeDesc")
                    End If

                Case EnumCustomSelect.EmpGroup
                    strSQL = "SELECT EmpGrpID, EmpGrpDesc FROM EMPGROUP WHERE Flag = 1"
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " Order BY EmpGrpID")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY EmpGrpDesc")
                    End If

                Case EnumCustomSelect.AccountType
                    strSQL = "SELECT AcctType, AcctTypeDesc FROM AcctType"
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " Order By AcctType")
                    Else
                        strSQL = String.Concat(strSQL, " Order BY AcctTypeDesc")
                    End If
                Case EnumCustomSelect.AccountNo
                    strSQL = "Select AccountNo FROM Account Where Status = 1 AND AcctType = 0 AND AcctOwner='" & Replace(CustomCriteria, "'", "''") & "'"
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY AccountNo")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY AccountNo")
                    End If

                    'Case EnumCustomSelect.Employee
                    '    strSQL = "SELECT EmployeeID, NickName FROM EMPLOYEE WHERE Flag = 1 AND Status = 1"
                    '    If TypeOrder = EnumTypeOrder.ByCode Then
                    '        strSQL = String.Concat(strSQL, " ORDER BY EmployeeID")
                    '    Else
                    '        strSQL = String.Concat(strSQL, " ORDER BY NickName")
                    '    End If

                Case EnumCustomSelect.CustomStock
                    strSQL = "SELECT StkCode, StockDesc FROM STOCK WHERE Active = 1 AND Flag = 1"
                    If CustomCriteria <> Nothing And CustomCriteria <> String.Empty Then
                        strSQL = String.Concat(strSQL, CustomCriteria)
                    End If
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY StkCode")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY StockDesc")
                    End If

                Case EnumCustomSelect.CustomerAccount
                    strSQL = "SELECT AccountNo FROM CUSTACCOUNT WHERE CustomerID = '" & Replace(CustomCriteria, "'", "''") & "'"

                Case EnumCustomSelect.TermID
                    strSQL = "SELECT TermID FROM SYSTERM WHERE BranchID = '" & Replace(CustomCriteria, "'", "''") & "'"

                Case EnumCustomSelect.Answer
                    strSQL = "SELECT ANSWER.AnswerName FROM ANSWER INNER JOIN ANSWERGRP ON ANSWER.AnswerID = ANSWERGRP.AnswerID " &
                             " WHERE (ANSWERGRP.AnsGroupID = '" & Replace(CustomCriteria, "'", "''") & "')"

                Case EnumCustomSelect.PromoBatch
                    strSQL = "SELECT PromoBatch, BatchDesc FROM PROMOBATCH WHERE Flag = 1 AND Active = 1"

                Case EnumCustomSelect.Supplier
                    strSQL = "SELECT SupplierID, SupplierName FROM SUPPLIER WHERE FLAG = 1 AND Active =1"

                Case EnumCustomSelect.Process
                    strSQL = "SELECT ProcessID FROM COMMISSIONHDR WHERE STATUS = 1"

                Case EnumCustomSelect.EmpService
                    strSQL = "SELECT Distinct(EmployeeID) FROM COMMISSIONDTL WHERE IsCalSales = 0 AND ProcessID = '" & Replace(CustomCriteria, "'", "''") & "'"

                Case EnumCustomSelect.EmpSales
                    strSQL = "SELECT Distinct(EmployeeID) From COMMISSIONQUOTA WHERE ProcessID = '" & Replace(CustomCriteria, "'", "''") & "'"

                Case EnumCustomSelect.SubGroup
                    strSQL = "SELECT SubGrpCode , SubGrpDesc From STKSUBGROUP"
                    If CustomCriteria <> Nothing And CustomCriteria <> String.Empty Then
                        strSQL = String.Concat(strSQL, CustomCriteria)
                    End If
                Case EnumCustomSelect.RptBranch
                    strSQL = "SELECT BranchID, BranchName FROM SysBranch WHERE Flag = 1 " &
                            "AND BranchID IN (SELECT ShowBranch FROM SYSRPTBRANCH WHERE BranchID = '" & CustomCriteria & "') "
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY BranchID")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY BranchName")
                    End If
                Case EnumCustomSelect.EmpNickName
                    strSQL = "SELECT EmployeeID, NickName FROM EMPLOYEE WHERE Flag = 1 AND Status = 1"
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY EmployeeID")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY NickName")
                    End If
                Case EnumCustomSelect.CityCode
                    strSQL = "SELECT CityCode, CityDesc FROM TABLEAREA WHERE Active = 1"
                Case EnumCustomSelect.TableNo
                    strSQL = "SELECT TableNo, TableCatg FROM TABLEINFO "
                    If CustomCriteria <> Nothing And CustomCriteria <> String.Empty Then
                        strSQL = String.Concat(strSQL, CustomCriteria)
                    End If
                Case EnumCustomSelect.Template
                    'strSQL = "SELECT TemplateID, TempName, Message, MsgType, CreateDate FROM SMS_TEMPLATE GROUP BY TempName"
                    strSQL = "SELECT TempName FROM MS_TEMPLATE GROUP BY TempName"
                Case EnumCustomSelect.Rouding
                    strSQL = "SELECT DISTINCT RoundCode AS RoundCode FROM  ROUNDSETUP"

                Case EnumCustomSelect.WarehouseSection
                    strSQL = "SELECT SECTIONCODE, SECTIONDESC FROM WH_SECTION WHERE Active = 1 AND Flag = 1"
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY SECTIONCODE")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY SECTIONDESC")
                    End If

                Case EnumCustomSelect.WarehouseArea
                    strSQL = "SELECT AREACODE, AREADESC FROM WH_AREA WHERE Active = 1 AND Flag = 1"
                    If CustomCriteria <> Nothing Then
                        strSQL = String.Concat(strSQL, CustomCriteria)
                    End If
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY AREACODE")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY AREADESC")
                    End If

                Case EnumCustomSelect.WarehouseRow
                    strSQL = "SELECT CROW, CROW FROM WH_RACK WHERE Active = 1 AND Flag = 1 "
                    If CustomCriteria <> Nothing Then
                        strSQL = String.Concat(strSQL, CustomCriteria)
                    End If
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY CROW")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY CROW")
                    End If

                Case EnumCustomSelect.PaymentScheme
                    strSQL = "SELECT SchemeNo, SchemeDesc FROM PKGPAYSCHEME ORDER BY SchemeNo"

                Case EnumCustomSelect.CustomerType
                    strSQL = "SELECT Code, CodeDesc FROM CodeMaster WHERE CodeType = 'TYP' "

                Case EnumCustomSelect.PlanID
                    strSQL = "SELECT PlanID, PlanDesc FROM STKDISTPLAN ORDER BY PlanID"

                Case EnumCustomSelect.TemplateName
                    strSQL = "SELECT TempName, Message FROM MS_TEMPLATE "
                    If CustomCriteria <> Nothing And CustomCriteria <> String.Empty Then
                        strSQL = String.Concat(strSQL, "WHERE " & CustomCriteria)
                    End If
                    strSQL = String.Concat(strSQL, " ORDER BY TemplateID")

                Case EnumCustomSelect.WarehouseLevel
                    strSQL = "SELECT CLevel, CLevel FROM WH_SHELF WHERE Active = 1 AND Flag = 1 "
                    If CustomCriteria <> Nothing Then
                        strSQL = String.Concat(strSQL, CustomCriteria)
                    End If
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY CLevel")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY CLevel")
                    End If
                Case EnumCustomSelect.DataBase
                    strSQL = "SELECT Code, CodeDesc FROM CodeMaster WHERE CodeType = 'DB' "
                Case EnumCustomSelect.BankCode
                    strSQL = "SELECT Code, CodeDesc FROM CodeMaster WHERE CodeType = 'BNK' "
                    If CustomCriteria <> Nothing Then
                        strSQL = String.Concat(strSQL, CustomCriteria)
                    End If
                Case EnumCustomSelect.PaymentMethod
                    strSQL = "SELECT Code, CodeDesc FROM CodeMaster WHERE CodeType = 'PAY' "
                    If CustomCriteria <> Nothing Then
                        strSQL = String.Concat(strSQL, CustomCriteria)
                    End If
                Case EnumCustomSelect.GLCode
                    strSQL = "SELECT Code, CodeDesc FROM CodeMaster WHERE CodeType = 'GLC' "
                    If CustomCriteria <> Nothing Then
                        strSQL = String.Concat(strSQL, CustomCriteria)
                    End If
                Case EnumCustomSelect.SysFunction
                    strSQL = "SELECT FunctionCode, FunctionName FROM SysFunction WHERE Active = 1 AND Flag = 1 AND APPID =" & CustomCriteria
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY FunctionCode")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY FunctionName")
                    End If
                Case EnumCustomSelect.SysModule
                    strSQL = "SELECT ModuleCode, ModuleName FROM SysModule WHERE Active = 1 AND Flag = 1 AND APPID =" & CustomCriteria
                    If TypeOrder = EnumTypeOrder.ByCode Then
                        strSQL = String.Concat(strSQL, " ORDER BY ModuleCode")
                    Else
                        strSQL = String.Concat(strSQL, " ORDER BY ModuleName")
                    End If
                Case EnumCustomSelect.FileType
                    strSQL = "SELECT Code, CodeDesc FROM CodeMaster WHERE CodeType = 'FIL' "
                    If CustomCriteria <> Nothing Then
                        strSQL = String.Concat(strSQL, CustomCriteria)
                    End If
            End Select

            If strSQL Is Nothing = False Then
                CustomSelect = conn.EnquiryDS(strSQL)
            End If

            Return CustomSelect
        End Function

        'Public Shared Function TypeSelect(ByVal SelectType As EnumSelectType, Optional ByVal TypeCriteria As String = Nothing, Optional ByVal TypeOrder As EnumTypeOrder = EnumTypeOrder.ByCode) As DataSet
        '    Dim strSQL As String = ""
        '    Dim strCodeM As String = "SELECT Code, CodeDesc FROM CODEMASTER "
        '    TypeSelect = Nothing
        '    Dim conn As CoreControl = New CoreControl()
        '    conn.ConnectionString = ConnString

        '    Select Case SelectType
        '        Case EnumSelectType.Month
        '            strSQL = String.Concat(strCodeM, "WHERE CodeType = 'MTH' ORDER BY CodeSeq")
        '        Case EnumSelectType.TaxType
        '            strSQL = String.Concat(strCodeM, "WHERE CodeType = 'TAX' ORDER BY CodeSeq")
        '        Case EnumSelectType.TaxExInc
        '            strSQL = String.Concat(strCodeM, "WHERE CodeType = 'TIX' ORDER BY CodeSeq")
        '        Case EnumSelectType.DirectoryType
        '            strSQL = String.Concat(strCodeM, "WHERE CodeType = 'ACD' ORDER BY CodeSeq")
        '        Case EnumSelectType.TransType
        '            strSQL = String.Concat(strCodeM, "WHERE CodeType = 'TRS' ORDER BY CodeSeq")
        '        Case EnumSelectType.TransTypeTR
        '            strSQL = String.Concat(strCodeM, "WHERE CodeType = 'TRS' AND (Code = 0 OR Code = 1) ORDER BY CodeSeq")
        '        Case EnumSelectType.DiscType
        '            strSQL = "SELECT DiscType, DiscTypeDesc FROM DISCTYPE WHERE Active=1"
        '            If TypeOrder = EnumTypeOrder.ByType Then
        '                strSQL = String.Concat(strSQL, " ORDER BY DiscType")
        '            Else
        '                strSQL = String.Concat(strSQL, " ORDER BY DiscTypeDesc")
        '            End If
        '        Case EnumSelectType.TenderType
        '            strSQL = "SELECT TenderType, TenderTypeDesc FROM TENDERTYPE WHERE Active=1"
        '            If TypeOrder = EnumTypeOrder.ByType Then
        '                strSQL = String.Concat(strSQL, " ORDER BY TenderType")
        '            Else
        '                strSQL = String.Concat(strSQL, " ORDER BY TenderTypeDesc")
        '            End If
        '        Case EnumSelectType.RemarkType
        '            strSQL = "SELECT RemarkType, RemarkTDesc FROM REMARKTYPE WHERE Active=1"
        '            If TypeOrder = EnumTypeOrder.ByType Then
        '                strSQL = String.Concat(strSQL, " ORDER BY RemarkType")
        '            Else
        '                strSQL = String.Concat(strSQL, " ORDER BY RemarkTDesc")
        '            End If
        '        Case EnumSelectType.StockType
        '            strSQL = "SELECT StkType, StkTypeDesc FROM StkType WHERE ACTIVE = 1"
        '            If TypeOrder = EnumTypeOrder.ByType Then
        '                strSQL = String.Concat(strSQL, " ORDER BY StkType")
        '            Else
        '                strSQL = String.Concat(strSQL, " ORDER BY StkTypeDesc")
        '            End If
        '        Case EnumSelectType.Condition
        '            strSQL = String.Concat(strCodeM, "WHERE CodeType = 'CON' ORDER BY CodeSeq")

        '        Case EnumSelectType.MethodsType
        '            strSQL = String.Concat(strCodeM, "WHERE CodeType = 'CMT' ORDER BY CodeSeq")

        '        Case EnumSelectType.SalesAdjust
        '            strSQL = String.Concat(strCodeM, "WHERE CodeType = 'SAJ' ORDER BY CodeSeq")

        '        Case EnumSelectType.CommAdjust
        '            strSQL = String.Concat(strCodeM, "WHERE CodeType = 'CAJ' ORDER BY CodeSeq")

        '        Case EnumSelectType.Commission
        '            strSQL = String.Concat(strCodeM, "WHERE CodeType = 'CTY' ORDER BY CodeSeq")

        '        Case EnumSelectType.StockTax
        '            strSQL = String.Concat(strCodeM, "WHERE CodeType = 'STA' ORDER BY CodeSeq")

        '        Case EnumSelectType.DuplicateType
        '            strSQL = String.Concat(strCodeM, "WHERE CodeType = 'SPR' ORDER BY CodeSeq")

        '        Case EnumSelectType.TenderAdjust
        '            strSQL = String.Concat(strCodeM, "WHERE CodeType = 'TAJ' ORDER BY CodeSeq")

        '        Case EnumSelectType.Tender
        '            strSQL = "SELECT TenderID, TenderDesc FROM TENDER WHERE Active = 1 AND Flag = 1"

        '        Case EnumSelectType.EIS
        '            strSQL = String.Concat(strCodeM, "WHERE CodeType = 'EIS' ORDER BY CodeSeq")

        '        Case EnumSelectType.AnswerGrp
        '            strSQL = "SELECT DISTINCT AnsGroupID FROM ANSWERGRP"
        '        Case EnumSelectType.PrefType
        '            strSQL = "SELECT Code, CodeDesc FROM  CODEMASTER WHERE (CodeType = 'CPF')"
        '        Case EnumSelectType.StudentStatus
        '            strSQL = "SELECT Code, CodeDesc FROM CODEMASTER WHERE (CodeType = 'StS')"
        '        Case EnumSelectType.PrefItem
        '            strSQL = "SELECT Code, CodeDesc FROM  CODEMASTER WHERE (CodeType = 'GDO')"
        '        Case EnumSelectType.CustomerID
        '            strSQL = "SELECT DISTINCT CUSTPREF.PrefCode, EMPLOYEE.NickName FROM CUSTPREF INNER JOIN  EMPLOYEE ON CUSTPREF.PrefCode = EMPLOYEE.EmployeeID WHERE     (CUSTPREF.PrefType = 'EMP')"
        '        Case EnumSelectType.Department
        '            strSQL = "SELECT Code, CodeDesc FROM CODEMASTER WHERE (CodeType = 'SDT')"
        '        Case EnumSelectType.AdjType
        '            strSQL = "SELECT Code, CodeDesc FROM CODEMASTER WHERE (CodeType = 'CVT')"
        '        Case EnumSelectType.MemberType
        '            strSQL = "SELECT Code, CodeDesc FROM CODEMASTER WHERE (CodeType = 'MBT')"
        '        Case EnumSelectType.RoundType
        '            strSQL = "SELECT Code, CodeDesc FROM CODEMASTER WHERE (CodeType = 'RDT')"
        '        Case EnumSelectType.Position
        '            strSQL = String.Concat(strCodeM, " WHERE (CodeType = 'POS') ORDER BY CodeSeq ")
        '        Case EnumSelectType.PaymentType
        '            strSQL = String.Concat(strCodeM, " WHERE (CodeType = 'PAY') ORDER BY CodeSeq ")
        '        Case EnumSelectType.Sday
        '            strSQL = String.Concat(strCodeM, " WHERE (CodeType = 'SHM') ORDER BY CodeSeq ")
        '        Case EnumSelectType.SYear
        '            strSQL = String.Concat(strCodeM, " WHERE (CodeType = 'YRS') ORDER BY CodeSeq ")
        '        Case EnumSelectType.ControlType
        '            strSQL = String.Concat(strCodeM, " WHERE (CodeType = 'CTR') ORDER BY CodeSeq")
        '        Case EnumSelectType.TradeType
        '            strSQL = String.Concat(strCodeM, " WHERE (CodeType = 'TRD') ORDER BY CodeSeq")
        '        Case EnumSelectType.PetBreed
        '            strSQL = String.Concat(strCodeM, "WHERE CodeType = 'PBR' ORDER BY CodeSeq")
        '        Case EnumSelectType.PetType
        '            strSQL = String.Concat(strCodeM, "WHERE CodeType = 'PTY' ORDER BY CodeSeq")
        '    End Select

        '    If strSQL Is Nothing = False Then
        '        TypeSelect = conn.EnquiryDS(strSQL)
        '    End If

        '    Return TypeSelect
        'End Function

        Public Shared Function StatusSelect(ByVal ConnString As String, ByVal SelectStatus As EnumSelectStatus) As DataSet
            Dim conn As CoreControl = New CoreControl()
            conn.ConnectionString = ConnString

            Dim strSQL As String = ""
            Dim strCodeM As String = "SELECT CAST(Code as INT) as Code, CodeDesc FROM CODEMASTER "
            StatusSelect = Nothing

            Select Case SelectStatus
                Case EnumSelectStatus.Booking
                    strSQL = String.Concat(strCodeM, "WHERE CodeType = 'BST' ORDER BY CodeSeq")
                Case EnumSelectStatus.Service
                    strSQL = String.Concat(strCodeM, "WHERE CodeType = 'INS' ORDER BY CodeSeq")
                Case EnumSelectStatus.User
                    strSQL = String.Concat(strCodeM, "WHERE CodeType = 'USR' ORDER BY CodeSeq")
                Case EnumSelectStatus.Block
                    strSQL = String.Concat(strCodeM, "WHERE CodeType = 'BLK' ORDER BY CodeSeq")
            End Select

            If strSQL Is Nothing = False Then
                StatusSelect = conn.EnquiryDS(strSQL)
            End If

            Return StatusSelect
        End Function

        'Public Shared Function OptionSelect(ByVal SelectOption As EnumOptionType) As DataSet
        '    Dim conn As CoreControl = New CoreControl()
        '    conn.ConnectionString = ConnString

        '    Dim strSQL As String = ""
        '    Dim strCodeM As String = "SELECT Code, CodeDesc FROM CODEMASTER "
        '    OptionSelect = Nothing

        '    Try
        '        Select Case SelectOption
        '            Case EnumOptionType.Salulation
        '                strSQL = String.Concat(strCodeM, "WHERE CodeType = 'SAL' ORDER BY CodeSeq")
        '            Case EnumOptionType.Sex
        '                strSQL = String.Concat(strCodeM, "WHERE CodeType = 'GEN' ORDER BY CodeSeq")
        '            Case EnumOptionType.Nationality
        '                strSQL = String.Concat(strCodeM, "WHERE CodeType = 'NAT' ORDER BY CodeSeq")
        '            Case EnumOptionType.Race
        '                strSQL = String.Concat(strCodeM, "WHERE CodeType = 'RAC' ORDER BY CodeSeq")
        '            Case EnumOptionType.Religion
        '                strSQL = String.Concat(strCodeM, "WHERE CodeType = 'REL' ORDER BY CodeSeq")
        '            Case EnumOptionType.MaritalStatus
        '                strSQL = String.Concat(strCodeM, "WHERE CodeType = 'MAR' ORDER BY CodeSeq")
        '            Case EnumOptionType.Second
        '                strSQL = String.Concat(strCodeM, "WHERE CodeType = 'SEC' ORDER BY CodeSeq")
        '                '--------------------------------------
        '            Case EnumOptionType.RefInfo
        '                strSQL = String.Concat(strCodeM, "WHERE CodeType = 'REF' ORDER BY CodeSeq")

        '                'Case EnumOptionType.StockColor
        '                '    strSQL = String.Concat(strCodeM, "WHERE CodeType = 'COL' ORDER BY CodeSeq")
        '        End Select

        '        If strSQL Is Nothing = False Then
        '            OptionSelect = conn.EnquiryDS(strSQL)
        '        End If
        '    Catch errSelect As Exception

        '    End Try

        '    Return OptionSelect
        'End Function

        'Public Shared Function BuildSQLStatement(ByVal TableName As String, ByVal FieldName As String, Optional ByVal Condition As String = Nothing, Optional ByVal OrderBy As String = Nothing) As String
        '    Dim strSQLStatement As String
        '    Try
        '        strSQLStatement = BuildSelect(FieldName, TableName, Condition, OrderBy)
        '        Return strSQLStatement
        '    Catch exBuildSQLStatement As Exception
        '        Return String.Empty
        '    End Try
        'End Function
#End Region
    End Class

    Public Class SchemeBase
        'Inherits SEAL.Model.Moyenne.SchemeBase
    End Class

    Public Class SingleBase
        'Inherits SEAL.Model.Moyenne.SingleBase
    End Class


#Region "Core Control Base Class"
    Public Class CoreControl
        'Inherits SEAL.Model.Moyenne.CoreStandard
        Private _connectionString As String
        Private _dCom As SqlConnection
        Private _BranchID As String
        Private _TermID As Integer

        Public ReadOnly Property Conn As SqlConnection
            Get
                Return _dCom 
            End Get
        End Property

        Public ReadOnly Property CurBranchID() As String
            Get
                Return _BranchID
            End Get
        End Property

        Public ReadOnly Property CurTermID() As Integer
            Get
                Return _TermID
            End Get
        End Property

        Public Property ConnectionString() As String
            Set(value As String)
                _connectionString = value
            End Set
            Get
                Return _connectionString
            End Get
        End Property

#Region "Connection Establishment"
        Public Sub New()
            '_connectionString = ConfigurationManager.ConnectionStrings("conn").ConnectionString
            '_BranchID = CStr(ConfigurationManager.AppSettings("BranchID"))
            '_TermID = CInt(ConfigurationManager.AppSettings("TermID"))
            ConnectionSetup()
        End Sub

        Public Overloads Sub ConnectionSetup()
            Try
                'If objConn Is Nothing = False Then
                '    objConn = Nothing
                'End If
                '_dCom = New SEAL.Data.DataAccess
                '_dCom.ConnectionString = _connectionString
                _dCom = New SqlConnection(_connectionString)
                '_dCom.TimeOutCommand = CInt(ConfigurationManager.AppSettings("Timeout"))
                '_dCom.ConnectionTimeout = 30
                'Me.SetConnection(_dCom)
                'objConn.TimeOutCommand = 15
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Sub

        Protected Function StartConnection(Optional IsolationState As EnumIsoState = EnumIsoState.StateReadOnly, Optional Reset As Boolean = False) As Boolean
            _dCom.Open()
            Return True
        End Function

        Protected Function StartSQLControl() As Boolean
            '_dCom.Open()
            Return True
        End Function

        Protected Function EndSQLControl() As Boolean
            _dCom.Close()
            Return True
        End Function

        Protected Function EndConnection() As Boolean
            _dCom.Close()
            Return True
        End Function
#End Region

#Region "Common Functions"
        Public Function EnquiryDS(SQLStmt As String) As DataSet
            Dim da As SqlDataAdapter = New SqlDataAdapter()
            Dim cmd As SqlCommand = _dCom.CreateCommand()
            cmd.CommandText = SQLStmt
            da.SelectCommand = cmd
            Dim ds As DataSet = New DataSet()

            _dCom.Open()
            da.Fill(ds)
            _dCom.Close()
            Return ds
            'SqlDataAdapter da = New SqlDataAdapter();
            'SqlCommand cmd = conn.CreateCommand();
            'cmd.CommandText = Sql;
            'da.SelectCommand = cmd;
            'DataSet ds = New DataSet();

            'conn.Open();
            'da.Fill(ds);
            'conn.Close();

            'Return ds;
        End Function

        'Public Function ExecuteQuery(ByVal arrList As ArrayList) As Boolean
        '    Dim blnStatus As Boolean = False
        '    Try
        '        StartConnection()
        '        StartSQLControl()
        '        objConn.BatchExecute(arrList, CommandType.Text)
        '        blnStatus = True
        '    Catch ex As Exception
        '        Throw New ApplicationException(ex.Message)
        '    Finally
        '        EndSQLControl()
        '        EndConnection()
        '    End Try
        '    Return blnStatus
        'End Function

#End Region

    End Class
#End Region

#Region "Global Base"
    Public MustInherit Class GlobalBase
#Region "Structures"
        Public Structure StrucDBLogonInfo
            Dim Server As String
            Dim Database As String
            Dim UserID As String
            Dim Password As String
            Dim AppName As String
            Dim TimeOut As Integer
            Dim ReportDir As String
        End Structure

#End Region

        Public Enum EnumAddsStyle
            Simple = 1
            Complete = 2
        End Enum

        Public Enum EnumSavePart
            Part1 = 0
            Part2 = 1
        End Enum

        Protected Shared LocalConnStr As String
        Protected Shared HostConnStr As String
        'Protected Shared LocalConnTyp As EnumConnType = EnumConnType.ctSQLDB
        'Protected Shared HostConnTyp As EnumConnType = EnumConnType.ctSQLDB

#Region "Public Shared"
        Public Shared CurrUserID As String
        Public Shared CurrUserName As String
        Public Shared AuthUserID As String
        Public Shared ProgramStarted As Boolean
        Public Shared ConnectServer As Boolean


        'Friend Shared dComLocal As DataAccess
        'Friend Shared dComHost As DataAccess

        Public Shared DBLogonInfoH As StrucDBLogonInfo
        Public Shared DBLogonInfoL As StrucDBLogonInfo
#End Region

        Public Shared WriteOnly Property LocalConnString() As String
            Set(ByVal Value As String)
                LocalConnStr = Value.Trim
            End Set
        End Property

        Public Shared WriteOnly Property HostConnString() As String
            Set(ByVal Value As String)
                HostConnStr = Value.Trim
            End Set
        End Property

        'Public Shared Property LocalConnType() As EnumConnType
        '    Get
        '        Return LocalConnTyp
        '    End Get
        '    Set(ByVal Value As EnumConnType)
        '        LocalConnTyp = Value
        '    End Set
        'End Property

        'Public Shared Property HostConnType() As EnumConnType
        '    Get
        '        Return HostConnTyp
        '    End Get
        '    Set(ByVal Value As EnumConnType)
        '        HostConnTyp = Value
        '    End Set
        'End Property

        'Public Shared Property BranchFilter() As Boolean
        '    Get
        '        Return bBranchFilter
        '    End Get
        '    Set(ByVal Value As Boolean)
        '        bBranchFilter = Value
        '    End Set
        'End Property

        'Public Shared Function GetBranchFilter(ByVal BranchID As String) As String
        '    Dim objSQL As SQLControl
        '    Dim strSQL As String = String.Empty
        '    Try
        '        objSQL = New SQLControl
        '        If BranchID.Trim.Length > 0 Then
        '            If BranchFilter Then
        '                strSQL = String.Concat(" BranchID IN('", objSQL.ParseValue(EnumDataType.dtString, BranchID), "')")
        '            Else
        '                strSQL = String.Empty
        '            End If
        '        End If
        '        Return strSQL
        '        objSQL.Dispose()
        '    Catch errFilter As Exception
        '        Return strSQL
        '    End Try
        '    ''If SQLStmt.IndexOf("WHERE") = -1 Then
        '    ''    SQLStmt = String.Concat(SQLStmt, " AND BranchID IN('" & BranchID & "')")
        '    ''Else
        '    ''    If SQLStmt.IndexOf("BranchID =") = -1 Then
        '    ''        SQLStmt = String.Concat(SQLStmt, " WHERE BranchID IN('" & BranchID & "')")
        '    ''    End If
        '    ''End If
        '    ''Return SQLStmt
        'End Function

        'Public Shared Property DesiredBranch() As String
        '    Get
        '        Return strDesiredBranch
        '    End Get
        '    Set(ByVal Value As String)
        '        strDesiredBranch = Value
        '    End Set
        'End Property

        'Public Shared ReadOnly Property LocalConn() As DataAccess
        '    Get
        '        If dComLocal Is Nothing Then
        '            dComLocal = New DataAccess
        '            With dComLocal
        '                .AutoClose = True
        '                .ConnectionType = EnumConnType.ctSQLDB
        '                .ConnectionString = LocalConnStr
        '                .IsolationMode = IsolationLevel.Serializable
        '            End With
        '        End If
        '        Return dComLocal
        '    End Get
        'End Property

        'Public Shared ReadOnly Property HostConn() As DataAccess
        '    Get
        '        If dComHost Is Nothing Then
        '            dComHost = New DataAccess
        '            With dComHost
        '                .AutoClose = True
        '                .ConnectionType = EnumConnType.ctSQLDB
        '                .ConnectionString = HostConnStr
        '                .IsolationMode = IsolationLevel.Serializable
        '            End With
        '        End If
        '        Return dComHost
        '    End Get
        'End Property

        Public Shared Function BuildConnectionL() As String
            Dim strConn As String
            With DBLogonInfoL
                If .Server Is Nothing Or .Database Is Nothing Or .UserID Is Nothing Or .AppName Is Nothing Then
                    Return String.Empty
                End If
                strConn = String.Concat("SERVER=", .Server, ";")
                strConn = String.Concat(strConn, "DATABASE=", .Database, ";")
                strConn = String.Concat(strConn, "UID=", .UserID, ";")
                strConn = String.Concat(strConn, "PWD=", .Password, ";")
                strConn = String.Concat(strConn, "APPLICATION NAME=", .AppName, ";")
                strConn = String.Concat(strConn, "CONNECTION TIMEOUT=", .TimeOut)
                Return strConn
            End With
        End Function

        Public Shared Function BuildConnectionH() As String
            Dim strConn As String
            With DBLogonInfoH
                If .Server Is Nothing Or .Database Is Nothing Or .UserID Is Nothing Or .AppName Is Nothing Then
                    Return String.Empty
                End If
                strConn = String.Concat("SERVER=", .Server, ";")
                strConn = String.Concat(strConn, "DATABASE=", .Database, ";")
                strConn = String.Concat(strConn, "UID=", .UserID, ";")
                strConn = String.Concat(strConn, "PWD=", .Password, ";")
                strConn = String.Concat(strConn, "APPLICATION NAME=", .AppName, ";")
                strConn = String.Concat(strConn, "CONNECTION TIMEOUT=", .TimeOut)
                Return strConn
            End With
        End Function

        'Public Shared Function FillBranchInfo(ByRef objConn As DataAccess, ByRef objBranch As BranchBase,
        '    ByVal BranchID As String, Optional ByVal SettingOnly As Boolean = False) As Boolean
        '    Dim strSQL, strTemp As String
        '    Dim rdrBranch As SqlClient.SqlDataReader
        '    Dim AddsInfo As New BranchBase.StrucAddsInfo
        '    Dim objBuild As New StringBuilder
        '    Try
        '        FillBranchInfo = False
        '        If objConn Is Nothing = False Then
        '            If objConn.OpenConnection = True Then
        '                With objBuild
        '                    .Insert(0, "SELECT ")
        '                    .Append("BranchID, BranchName, BranchCode, AccNo, RevGroup, ")
        '                    .Append("Address1, Address2, Address3, Address4, Contact1, Contact2, StoreType, ")
        '                    .Append("Email, Tel, Fax, Region, State, Currency, StoreStatus, OpPrevBook, ")
        '                    .Append("OpTimeStart, OpTimeEnd, OpDay1, OpDay2, OpDay3, OpDay4, OpDay5, OpDay6, OpDay7, ")
        '                    .Append("OpBookAlwDY, OpBookAlwHR, OpBookFirst, OpBookLast, OpBookIntv, SalesItemType, ")
        '                    .Append("InSvcItemType, GenInSvcID, RcpFooter, RcpHeader, PriceLevel ")
        '                    .Append("FROM SYSBRANCH WHERE ")
        '                    .Append("BranchID='" & Replace(BranchID, "'", "''") & "'")
        '                    strSQL = .ToString
        '                End With

        '                rdrBranch = CType(objConn.Execute(strSQL, EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
        '                If rdrBranch Is Nothing = False Then
        '                    With rdrBranch
        '                        If .Read = True Then
        '                            BranchBase.BranchID = Convert.ToString(.Item("BranchID"))
        '                            BranchBase.BranchCode = Convert.ToString(.Item("BranchCode"))
        '                            BranchBase.CompName = Convert.ToString(.Item("BranchName"))
        '                            BranchBase.RevGroup = Convert.ToString(.Item("RevGroup"))
        '                            BranchBase.SalesItemType = Convert.ToString(.Item("SalesItemType"))
        '                            BranchBase.InSvcItemType = Convert.ToString(.Item("InSvcItemType"))
        '                            BranchBase.GenInSvcID = Convert.ToInt16(.Item("GenInSvcID"))


        '                            AddsInfo.CompName = BranchBase.CompName
        '                            AddsInfo.Addrs1 = Convert.ToString(.Item("Address1"))
        '                            AddsInfo.Addrs2 = Convert.ToString(.Item("Address2"))
        '                            AddsInfo.Addrs3 = Convert.ToString(.Item("Address3"))
        '                            AddsInfo.Addrs4 = Convert.ToString(.Item("Address4"))
        '                            BranchBase.AddressInfo = AddsInfo
        '                            BranchBase.RcpHeader = String.Concat(Convert.ToString(.Item("RcpHeader")) & " ", CompileAddress(EnumAddsStyle.Simple, AddsInfo))
        '                            BranchBase.RcpFooter = Convert.ToString(.Item("RcpFooter"))
        '                            strTemp = Convert.ToString(.Item("OpTimeStart"))
        '                            BranchBase.BizStartTime = Left(strTemp, 2) & ":" & Right(strTemp, 2)
        '                            strTemp = Convert.ToString(.Item("OpTimeEnd"))
        '                            BranchBase.BizEndTime = Left(strTemp, 2) & ":" & Right(strTemp, 2)
        '                            BranchBase.PrevDayAllow = Convert.ToInt32(.Item("OpPrevBook"))
        '                            If Convert.ToInt32(.Item("OpDay1")) = 1 Then
        '                                BranchBase.WorkDay1 = True
        '                            Else
        '                                BranchBase.WorkDay1 = False
        '                            End If
        '                            If Convert.ToInt32(.Item("OpDay2")) = 1 Then
        '                                BranchBase.WorkDay2 = True
        '                            Else
        '                                BranchBase.WorkDay2 = False
        '                            End If
        '                            If Convert.ToInt32(.Item("OpDay3")) = 1 Then
        '                                BranchBase.WorkDay3 = True
        '                            Else
        '                                BranchBase.WorkDay3 = False
        '                            End If
        '                            If Convert.ToInt32(.Item("OpDay4")) = 1 Then
        '                                BranchBase.WorkDay4 = True
        '                            Else
        '                                BranchBase.WorkDay4 = False
        '                            End If
        '                            If Convert.ToInt32(.Item("OpDay5")) = 1 Then
        '                                BranchBase.WorkDay5 = True
        '                            Else
        '                                BranchBase.WorkDay5 = False
        '                            End If
        '                            If Convert.ToInt32(.Item("OpDay6")) = 1 Then
        '                                BranchBase.WorkDay6 = True
        '                            Else
        '                                BranchBase.WorkDay6 = False
        '                            End If
        '                            If Convert.ToInt32(.Item("OpDay7")) = 1 Then
        '                                BranchBase.WorkDay7 = True
        '                            Else
        '                                BranchBase.WorkDay7 = False
        '                            End If
        '                            strTemp = Convert.ToString(.Item("OpBookFirst"))
        '                            BranchBase.BookStartTime = Left(strTemp, 2) & ":" & Right(strTemp, 2)
        '                            strTemp = Convert.ToString(.Item("OpBookLast"))
        '                            BranchBase.BookEndTime = Left(strTemp, 2) & ":" & Right(strTemp, 2)
        '                            BranchBase.BookDayAllow = Convert.ToInt32(.Item("OpBookAlwDY"))
        '                            BranchBase.BookHourAllow = Convert.ToInt32(.Item("OpBookAlwHr"))
        '                            BranchBase.BookingInterval = Convert.ToInt32(.Item("OpBookIntv"))
        '                            BranchBase.PriceLevel = Convert.ToInt32(.Item("PriceLevel"))
        '                            FillBranchInfo = True
        '                        End If
        '                        .Close()
        '                    End With
        '                End If

        '                objBuild = Nothing

        '                objBuild = New StringBuilder
        '                With objBuild
        '                    .Insert(0, "SELECT ")
        '                    .Append("SysKey, SysValue ")
        '                    .Append("FROM SYSPREFB WHERE ")
        '                    .Append("BranchID='" & Replace(BranchID, "'", "''") & "'")
        '                    strSQL = .ToString
        '                End With
        '                rdrBranch = CType(objConn.Execute(strSQL, EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
        '                If rdrBranch Is Nothing = False Then
        '                    If SettingBase.InitalizeSettings(rdrBranch) Then
        '                        With objBranch
        '                            'Case "AUTOLISTING"
        '                            'Case "ANALYSISIMAGEPATH"
        '                            .AnalysisPath = CType(.GetSetting("ANALYSISIMAGEPATH"), String)
        '                        End With
        '                    End If
        '                End If
        '                rdrBranch = Nothing
        '            End If
        '        End If
        '    Catch errFill As Exception
        '        Return False
        '    Finally
        '        objBuild = Nothing
        '        rdrBranch = Nothing
        '    End Try
        'End Function

        'Public Shared Function FillTerminalInfo(ByRef objConn As DataAccess, ByRef objTerminal As TerminalBase, ByVal BranchID As String, ByVal TermID As Integer) As Boolean
        '    Dim strFields, strSQL, strCond, strTemp As String
        '    Dim rdrTerminal As SqlClient.SqlDataReader
        '    Dim objBuild As New StringBuilder
        '    Try
        '        FillTerminalInfo = False
        '        If objConn Is Nothing = False Then
        '            If objConn.OpenConnection = True Then

        '                With objBuild
        '                    .Append("SELECT ")
        '                    .Append("SysKey, SysValue ")
        '                    .Append("FROM SYSPREFT WHERE ")
        '                    .Append("BranchID='" & Replace(BranchID, "'", "''") & "' AND TermID=" & TermID)
        '                    strSQL = .ToString
        '                End With
        '                rdrTerminal = CType(objConn.Execute(strSQL, EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
        '                If rdrTerminal Is Nothing = False Then
        '                    With rdrTerminal
        '                        While .Read
        '                            With objTerminal
        '                                Select Case Convert.ToString(rdrTerminal("SysKey")).ToUpper
        '                                    Case "REQCUSTID"
        '                                        If Convert.ToString(rdrTerminal("SysValue")).Trim = "1" Then
        '                                            .ReqCustID = 1
        '                                        Else
        '                                            .ReqCustID = 0
        '                                        End If

        '                                    Case "REQSERVERID"
        '                                        If Convert.ToString(rdrTerminal("SysValue")).Trim = "1" Then
        '                                            .ReqServerID = 1
        '                                        Else
        '                                            .ReqServerID = 0
        '                                        End If

        '                                    Case "RECEIPTCOPIES"
        '                                        If IsNumeric(rdrTerminal("SysValue")) = True Then
        '                                            If Convert.ToInt32(rdrTerminal("SysValue")) >= 0 Then
        '                                                .ReceiptCopies = Convert.ToInt32(rdrTerminal("SysValue"))
        '                                            Else
        '                                                .ReceiptCopies = 0
        '                                            End If
        '                                        Else
        '                                            .ReceiptCopies = 0
        '                                        End If

        '                                    Case "CLOSEMSG"
        '                                        .CloseMsg = Convert.ToString(rdrTerminal("SysValue"))

        '                                    Case "IDLEMSG"
        '                                        .IdleMsg = Convert.ToString(rdrTerminal("SysValue"))

        '                                    Case "OPENMSG"
        '                                        .OpenMsg = Convert.ToString(rdrTerminal("SysValue"))

        '                                    Case "AFTERSALESMSG"
        '                                        .AfterSalesMsg = Convert.ToString(rdrTerminal("SysValue"))

        '                                    Case "LOGOFFMSG"
        '                                        .LogOffMsg = Convert.ToString(rdrTerminal("SysValue"))

        '                                    Case "LOGOPOSITION"
        '                                        If Convert.ToString(rdrTerminal("SysValue")).Trim = "1" Then
        '                                            .LogoPosition = 1 'Align = Left
        '                                        ElseIf Convert.ToString(rdrTerminal("SysValue")).Trim = "2" Then
        '                                            .LogoPosition = 2 'Align = Center
        '                                        ElseIf Convert.ToString(rdrTerminal("SysValue")).Trim = "3" Then
        '                                            .LogoPosition = 3 'Align = Right
        '                                        End If

        '                                    Case "PRINTJOBSHEET"
        '                                        If Convert.ToString(rdrTerminal("SysValue")).Trim = "1" Then
        '                                            .PrintJobSheet = True 'Print JobNote (Receip)
        '                                        ElseIf Convert.ToString(rdrTerminal("SysValue")).Trim = "2" Then
        '                                            .PrintJobSheet = False 'Print JobTicket (A4)
        '                                        End If

        '                                    Case "JOBSHEETCOPIES"
        '                                        If IsNumeric(rdrTerminal("SysValue")) = True Then
        '                                            If Convert.ToInt32(rdrTerminal("SysValue")) >= 0 Then
        '                                                .JobSheetCopies = Convert.ToInt32(rdrTerminal("SysValue"))
        '                                            Else
        '                                                .JobSheetCopies = 0
        '                                            End If
        '                                        Else
        '                                            .JobSheetCopies = 0
        '                                        End If

        '                                    Case "DIRECTPRINTJOBSHEET"
        '                                        If Convert.ToString(rdrTerminal("SysValue")).Trim = "1" Then
        '                                            .DirectPrintJobSheet = True
        '                                        Else
        '                                            .DirectPrintJobSheet = False
        '                                        End If

        '                                    Case "DIRECTPRINTTICKET"
        '                                        If Convert.ToString(rdrTerminal("SysValue")).Trim = "1" Then
        '                                            .DirectPrintTicket = True
        '                                        Else
        '                                            .DirectPrintTicket = False
        '                                        End If

        '                                    Case "INSERVICEVERIFY"
        '                                        If Convert.ToString(rdrTerminal("SysValue")).Trim = "1" Then
        '                                            .InServiceVerify = True
        '                                        Else
        '                                            .InServiceVerify = False
        '                                        End If

        '                                    Case "GREETINGMSG"
        '                                        .GreetingMsg = Convert.ToString(rdrTerminal("SysValue"))

        '                                    Case "ZREADHEADERCOPIES"
        '                                        If IsNumeric(rdrTerminal("SysValue")) = True Then
        '                                            If Convert.ToInt32(rdrTerminal("SysValue")) >= 0 Then
        '                                                .ZReadHeaderCopies = Convert.ToInt32(rdrTerminal("SysValue"))
        '                                            Else
        '                                                .ZReadHeaderCopies = 0
        '                                            End If
        '                                        Else
        '                                            .ZReadHeaderCopies = 0
        '                                        End If

        '                                    Case "TAXGROUP"
        '                                        .TaxGroup = Convert.ToString(rdrTerminal("SysValue"))

        '                                    Case "ROUNDINGAMT"
        '                                        If Convert.ToString(rdrTerminal("SysValue")).Trim = "1" Then
        '                                            .RoundAmt = EnumRoundAmt.RoundUp
        '                                        Else
        '                                            .RoundAmt = EnumRoundAmt.RoundDown
        '                                        End If

        '                                    Case "NORMALSALESVERIFY"
        '                                        If Convert.ToString(rdrTerminal("SysValue")).Trim = "1" Then
        '                                            .NormalSalesVerify = True
        '                                        Else
        '                                            .NormalSalesVerify = False
        '                                        End If

        '                                    Case "REPRINTSELECTION"
        '                                        If Convert.ToString(rdrTerminal("SysValue")).Trim = "1" Then
        '                                            .ReprintSelection = True
        '                                        Else
        '                                            .ReprintSelection = False
        '                                        End If

        '                                    Case "AP:ClosingStyle"
        '                                        If Convert.ToString(rdrTerminal("SysValue")).Trim = String.Empty Then
        '                                            .ClosingStyle = "NM"
        '                                        Else
        '                                            .ClosingStyle = Convert.ToString(rdrTerminal("SysValue")).Trim
        '                                        End If

        '                                    Case "APPTSHOWCOMPLETED"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .ShowCompletedAppt = True
        '                                        Else
        '                                            .ShowCompletedAppt = False
        '                                        End If

        '                                    Case "APPTSHOWCANCELLED"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .ShowCancelledAppt = True
        '                                        Else
        '                                            .ShowCancelledAppt = False
        '                                        End If

        '                                    Case "SHOWSYSTEMAMT"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .ShowSystemAmt = True
        '                                        Else
        '                                            .ShowSystemAmt = False
        '                                        End If

        '                                    Case "DEFCUSTOMER"
        '                                        .DefCustomerID = Convert.ToString(rdrTerminal("SysValue"))

        '                                    Case "DEFSERVER"
        '                                        .DefServerID = Convert.ToString(rdrTerminal("SysValue"))

        '                                    Case "REQREASONCODE"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .ReqReasonCode = True
        '                                        Else
        '                                            .ReqReasonCode = False
        '                                        End If
        '                                    Case "CUSTBRNFILTER"
        '                                        If Convert.ToString(rdrTerminal("SysValue")).Trim = "0" Then
        '                                            .CustomerBranchFilter = False
        '                                        Else
        '                                            .CustomerBranchFilter = True
        '                                        End If

        '                                    Case "SERVERSELECTION"
        '                                        If Convert.ToString(rdrTerminal("SysValue")).Trim = "0" Then
        '                                            .ServerSelection = 0
        '                                        Else
        '                                            .ServerSelection = 1
        '                                        End If

        '                                    Case "DEFCNEXPIRYDAY"
        '                                        If Convert.ToString(rdrTerminal("SysValue")).Trim <> String.Empty Then
        '                                            If Convert.ToInt32(rdrTerminal("SysValue")) <= 0 Then
        '                                                .ServerSelection = 30
        '                                            Else
        '                                                .ServerSelection = Convert.ToInt32(rdrTerminal("SysValue"))
        '                                            End If
        '                                        Else
        '                                            .DefCNExpiryDay = 30
        '                                        End If
        '                                    Case "REQSAVETENANT"
        '                                        If Convert.ToString(rdrTerminal("SysValue")).Trim <> String.Empty Then
        '                                            .ReqSaveTenant = Convert.ToInt32(rdrTerminal("SysValue"))
        '                                        Else
        '                                            .ReqSaveTenant = 0
        '                                        End If

        '                                    Case "PRINTRECEIPTLIST"
        '                                        If IsNumeric((rdrTerminal("SysValue"))) Then
        '                                            .PrintReceiptList = Convert.ToInt32(rdrTerminal("SysValue"))
        '                                        Else
        '                                            .PrintReceiptList = 0
        '                                        End If
        '                                    Case "PRINTITEMSUMMARYLIST"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .PrintItemSummaryList = True
        '                                        Else
        '                                            .PrintItemSummaryList = False
        '                                        End If
        '                                    Case "RECEIPTSTYLE"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .ReceiptStyle = True
        '                                        Else
        '                                            .ReceiptStyle = False
        '                                        End If

        '                                    Case "AUTOMATERIALPOSTING"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .AutoMaterialPosting = True
        '                                        Else
        '                                            .AutoMaterialPosting = False
        '                                        End If
        '                                        'Case "AUTOCOLORHAIRSCALPPOST"
        '                                        '    If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                        '        .AUTOCOLORHAIRSCALPPOST = True
        '                                        '    Else
        '                                        '        .AUTOCOLORHAIRSCALPPOST = False
        '                                        '    End If

        '                                    Case "RESETTRANSNO"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .ResetTransNo = True
        '                                        Else
        '                                            .ResetTransNo = False
        '                                        End If

        '                                    Case "AUTOCONFIRMPAYMENT"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .AutoConfirmPayment = True
        '                                        Else
        '                                            .AutoConfirmPayment = False
        '                                        End If

        '                                    Case "SHOWSURVEY"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .ShowSurvey = True
        '                                        Else
        '                                            .ShowSurvey = False
        '                                        End If

        '                                    Case "ALLOWCHARONTERM"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .RestrictInput = False
        '                                        Else
        '                                            .RestrictInput = True
        '                                        End If

        '                                    Case "PRINTBRANDSALES"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .PrintBrandSales = True
        '                                        Else
        '                                            .PrintBrandSales = False
        '                                        End If

        '                                    Case "PRINTCATGSALES"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .PrintCatgSales = True
        '                                        Else
        '                                            .PrintCatgSales = False
        '                                        End If

        '                                    Case "PRINTDEPTSALES"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .PrintDeptSales = True
        '                                        Else
        '                                            .PrintDeptSales = False
        '                                        End If

        '                                    Case "NOXZREADNO"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .NoXZReadNo = True
        '                                        Else
        '                                            .NoXZReadNo = False
        '                                        End If

        '                                    Case "RETURNSALESZERO"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .ReturnSalesZero = True
        '                                        Else
        '                                            .ReturnSalesZero = False
        '                                        End If

        '                                    Case "FLOATINGTRACK"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) <> String.Empty Then
        '                                            .FloatingTrack = Convert.ToString(.TermID) & "," & Convert.ToString(rdrTerminal("SysValue"))
        '                                        Else
        '                                            .FloatingTrack = Convert.ToString(.TermID)
        '                                        End If

        '                                    Case "DUMMYTERMINAL"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) <> String.Empty Then
        '                                            .DummyTerminal = Convert.ToString(rdrTerminal("SysValue"))
        '                                        Else
        '                                            .DummyTerminal = String.Empty
        '                                        End If

        '                                    Case "TRANSFERFROMTRANS"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .TransferFromTrans = True
        '                                        Else
        '                                            .TransferFromTrans = False
        '                                        End If

        '                                    Case "PRINTEMPSALES"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .PrintEmpSales = True
        '                                        Else
        '                                            .PrintEmpSales = False
        '                                        End If

        '                                    Case "SUMVOUCHERAMT"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .SumVoucherAmt = True
        '                                        Else
        '                                            .SumVoucherAmt = False
        '                                        End If

        '                                    Case "VALIDATECUSTOMER"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .ValidateCustomer = True
        '                                        Else
        '                                            .ValidateCustomer = False
        '                                        End If

        '                                    Case "VALIDATECUSTKEY"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) <> String.Empty Then
        '                                            .ValidateKey = True
        '                                            strTemp = String.Concat("'", Convert.ToString(rdrTerminal("SysValue")), "'")
        '                                            strTemp = strTemp.Replace(",", "','")
        '                                            .ValidateKeys = strTemp
        '                                        Else
        '                                            .ValidateKey = False
        '                                            .ValidateKeys = Nothing
        '                                        End If


        '                                    Case "PRIMARYHOST"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .isPrimaryHost = True
        '                                        Else
        '                                            .isPrimaryHost = False
        '                                        End If

        '                                    Case "REQTABLENO"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .ReqTableNo = 1
        '                                        Else
        '                                            .ReqTableNo = 0
        '                                        End If

        '                                    Case "CHECKSTKRCVDATE"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .CheckStkRcvDate = True
        '                                        Else
        '                                            .CheckStkRcvDate = False
        '                                        End If

        '                                    Case "ALLOWSPECIALDISC"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "0" Then
        '                                            .AllowSpecialDiscount = False
        '                                        Else
        '                                            .AllowSpecialDiscount = True
        '                                        End If
        '                                    Case "DEFVERITYPE"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .DefVeriType = 1
        '                                        ElseIf Convert.ToString(rdrTerminal("SysValue")) = "2" Then
        '                                            .DefVeriType = 2
        '                                        Else
        '                                            .DefVeriType = 0
        '                                        End If
        '                                    Case "POINTROUNDUP"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "0" Then
        '                                            .PointRoundUp = False
        '                                        Else
        '                                            .PointRoundUp = True
        '                                        End If
        '                                    Case "POINTINCLUDETAX"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .PointIncludeTax = False
        '                                        Else
        '                                            .PointIncludeTax = True
        '                                        End If
        '                                    Case "REQNOPAX"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .ReqNoPax = 1
        '                                        Else
        '                                            .ReqNoPax = 0
        '                                        End If
        '                                    Case "REQSHIFT"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .ReqShift = True
        '                                        Else
        '                                            .ReqShift = False
        '                                        End If
        '                                    Case "ALLOWRETURNBELOW"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .AllowReturnBelow = True
        '                                        Else
        '                                            .AllowReturnBelow = False
        '                                        End If
        '                                    Case "GLOBALSEARCHPERMISSION"
        '                                        Select Case Convert.ToString(rdrTerminal("SysValue"))
        '                                            Case "0"
        '                                                .GlobalSearchPermission = 0
        '                                            Case "1"
        '                                                .GlobalSearchPermission = 1
        '                                            Case "2"
        '                                                .GlobalSearchPermission = 2
        '                                        End Select
        '                                    Case "CHECKREFNO"
        '                                        Select Case Convert.ToString(rdrTerminal("SysValue"))
        '                                            Case "0"
        '                                                .CheckRefNo = 0
        '                                            Case "1"
        '                                                .CheckRefNo = 1
        '                                                .JobStreetName = "Job Street No"
        '                                        End Select
        '                                    Case "CUSTDTLFILTER"
        '                                        Select Case Convert.ToString(rdrTerminal("SysValue"))
        '                                            Case "0"
        '                                                .CustomerDetailFilter = 0
        '                                            Case "1"
        '                                                .CustomerDetailFilter = 1
        '                                        End Select
        '                                    Case "PRINTGROSSSALES"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .PrintGrossSales = True
        '                                        Else
        '                                            .PrintGrossSales = False
        '                                        End If
        '                                    Case "REQINVOICE"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .ReqInvoice = True
        '                                        Else
        '                                            .ReqInvoice = False
        '                                        End If
        '                                    Case "REQPETTYCASH"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .ReqPettyCash = True
        '                                        Else
        '                                            .ReqPettyCash = False
        '                                        End If
        '                                    Case "REQVOUCHER"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .ReqVoucher = True
        '                                        Else
        '                                            .ReqVoucher = False
        '                                        End If
        '                                    Case "REQCASHPOINT"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .ReqCashPoint = True
        '                                        Else
        '                                            .ReqCashPoint = False
        '                                        End If
        '                                    Case "REQSUBCUSTOMER"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .ReqSubCustomer = True
        '                                        Else
        '                                            .ReqSubCustomer = False
        '                                        End If
        '                                    Case "APPLYCUSTOMBACKGROUND"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .ApplyCustomBackground = True
        '                                        Else
        '                                            .ApplyCustomBackground = False
        '                                        End If
        '                                    Case "SHOWEMPSALES"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .ShowEmpSales = True
        '                                        Else
        '                                            .ShowEmpSales = False
        '                                        End If

        '                                    Case "REQUIREBIOKEY"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .ReqBioKey = True
        '                                        Else
        '                                            .ReqBioKey = False
        '                                        End If

        '                                    Case "SHOWACADEMIC"
        '                                        If Convert.ToString(rdrTerminal("SysValue")) = "1" Then
        '                                            .ShowAcademic = True
        '                                        Else
        '                                            .ShowAcademic = False
        '                                        End If

        '                                    Case Else
        '                                        If .FloatingTrack = String.Empty Or .FloatingTrack Is Nothing = True Then
        '                                            .FloatingTrack = Convert.ToString(.TermID)
        '                                        End If

        '                                        If .DummyTerminal = String.Empty Or .DummyTerminal Is Nothing = True Then
        '                                            .DummyTerminal = String.Empty
        '                                        End If
        '                                End Select
        '                            End With
        '                            FillTerminalInfo = True
        '                        End While
        '                        .Close()
        '                    End With
        '                End If
        '            End If
        '        End If
        '    Catch errFill As Exception
        '        Return False
        '    Finally
        '        rdrTerminal = Nothing
        '        objBuild = Nothing
        '    End Try
        'End Function

        Public Shared Function CompileAddress(ByVal AddsStyle As EnumAddsStyle, ByVal AddsInfo As BranchBase.StrucAddsInfo) As String
            Dim objBuild As New StringBuilder
            Try
                With objBuild
                    .Append(AddsInfo.CompName)
                    .Append(vbCrLf)
                    .Append(AddsInfo.Addrs1)
                    .Append(vbCrLf)
                    .Append(AddsInfo.Addrs2)
                    .Append(vbCrLf)
                    .Append(AddsInfo.Addrs3)
                    .Append(vbCrLf)
                    .Append(AddsInfo.Addrs4)
                    .Append(vbCrLf)
                    Return .ToString
                End With
            Catch errBuild As Exception
                Return String.Empty
            Finally
                objBuild = Nothing
            End Try
        End Function
    End Class
#End Region

#Region "Application Base"
    Public Class AppBase
#Region "Public Constant"
        Public Const GlobalDateFormat As String = "yyyy/MM/dd"
        Public Const GlobalTimeFormat As String = "HH:mm:ss"
        Public Const DateTimeFormat As String = "dd/MM/yyyy HH:mm:ss"
        Public Const DateFormat As String = "dd/MM/yyyy"
        Public Const TimeFormat As String = "HH:mm"
        Public Const TimeDisplayFormat As String = "hh:mm tt"
        Public Const AmountFormat As String = "0.00"
#End Region
        Public Shared AppTitle As String
        Public Shared RegComp As String
        Public Shared ReportPath As String
        Public Shared LogoPath As String
        Public Shared HostIP As String
        Public Shared WebServices As String
        Public Shared FtpHost As String
        Public Shared FtpPort As Integer = 21
        Public Shared FtpUser As String
        Public Shared FtpPassword As String
        Public Shared FtpConnMode As String = "PASV"
        Public Shared MailFrom As String
        Public Shared MailTo As String
        Public Shared MailCC As String
        Public Shared MailServer As String
        Public Shared SyncStartDate As String
        Public Shared SyncEndDate As String
        Public Shared FullScan As Integer
        Public Shared WebPath As String
        Public Shared WithSerial As String = "0"

        Public Shared Function ConvertToTime(ByVal Time As Object, Optional ByVal TimeFormat As String = "HH:mm:ss") As String
            Dim sTime As String
            Dim TimeIn As String
            If Time Is Nothing = False Then
                TimeIn = Convert.ToString(Time)
                Select Case TimeIn.Length
                    Case 4
                        sTime = Mid(TimeIn, 1, 2) & ":" & Mid(TimeIn, 3, 2)
                    Case 6
                        sTime = Mid(TimeIn, 1, 2) & ":" & Mid(TimeIn, 3, 2) & ":" & Mid(TimeIn, 5, 2)
                    Case Else
                        sTime = String.Empty
                End Select

                If sTime <> String.Empty Then
                    Return Format(Convert.ToDateTime(sTime), TimeFormat)
                Else
                    Return Format(Now, TimeFormat)
                End If
            End If
        End Function
    End Class
#End Region

    Public MustInherit Class SettingBase
        Protected Shared htbSettings As Hashtable

        Public Shared Function InitalizeSettings(ByRef rdrSys As SqlClient.SqlDataReader) As Boolean
            Dim strKey As String
            Try
                If htbSettings Is Nothing Then htbSettings = New Hashtable
                If rdrSys Is Nothing = False Then
                    htbSettings.Clear()
                    With rdrSys
                        While .Read
                            strKey = CType(.Item("SysKey"), String).ToUpper
                            htbSettings.Add(strKey, .Item("SysValue"))
                        End While
                        .Close()
                    End With
                    Return True
                End If
            Catch ex As Exception
                Throw New ApplicationException(ex.Message)
                Return False
            Finally
                rdrSys = Nothing
            End Try
        End Function

        Public Shared Function GetSetting(ByVal Syskey As String) As Object
            If htbSettings Is Nothing Then
                Return Nothing
            Else
                Syskey = Syskey.ToUpper
                If htbSettings.ContainsKey(Syskey) = True Then
                    Return htbSettings.Item(Syskey)
                Else
                    Return String.Empty
                End If
            End If
        End Function
    End Class

#Region "Branch Base"
    Public Class BranchBase
        Inherits SettingBase
        Public Structure StrucAddsInfo
            Dim CompName As String
            Dim Addrs1 As String
            Dim Addrs2 As String
            Dim Addrs3 As String
            Dim Addrs4 As String
            Dim Tel As String
            Dim Fax As String
        End Structure

        Public Shared BranchID As String
        Public Shared BranchCode As String
        Public Shared RevGroup As String
        Public Shared CompName As String
        Public Shared AppHQ As Boolean
        Public Shared AppMode As EnumAppMode
        Public Shared AppModeStr As String
        Public Shared AppEnabled As StrucAppEnabled
        Public Shared SalesItemType As String
        Public Shared InSvcItemType As String
        Public Shared GenInSvcID As Short
        Public Shared AddressInfo As StrucAddsInfo
        Public Shared RcpHeader As String
        Public Shared RcpFooter As String

        Public Shared ConfirmColor As String
        Public Shared TentativeColor As String
        Public Shared ArrivedColor As String
        Public Shared CancelledColor As String
        Public Shared CompletedColor As String
        Public Shared AnalysisPath As String

        Public Shared BizStartTime As String
        Public Shared BizEndTime As String
        Public Shared PrevDayAllow As Integer
        Public Shared BookStartTime As String
        Public Shared BookEndTime As String
        Public Shared WorkDay1 As Boolean
        Public Shared WorkDay2 As Boolean
        Public Shared WorkDay3 As Boolean
        Public Shared WorkDay4 As Boolean
        Public Shared WorkDay5 As Boolean
        Public Shared WorkDay6 As Boolean
        Public Shared WorkDay7 As Boolean
        Public Shared BookingInterval As Integer
        Public Shared BookDayAllow As Integer
        Public Shared BookHourAllow As Integer
        Public Shared PriceLevel As Integer
        Public Shared CustomerBranchFilter As Boolean

        'Public Overloads Shared Function CheckMode(ByVal CharIndex As Integer, ByVal CheckValue As Char) As Boolean
        '    If CheckAppMode(CharIndex) Then
        '        If Convert.ToChar(AppModeStr.Substring(CharIndex, 1).ToUpper) = CheckValue Then
        '            Return True
        '        Else
        '            Return False
        '        End If
        '    End If
        'End Function

        'Public Overloads Shared Function CheckMode(ByVal ReturnIndex As Integer) As Char
        '    If CheckAppMode(ReturnIndex) Then
        '        Return Convert.ToChar(AppModeStr.Substring(ReturnIndex, 1))
        '    Else
        '        Return CType(String.Empty, Char)
        '    End If
        'End Function

        Private Shared Function CheckAppMode(ByVal CharIndex As Integer) As Boolean
            If AppModeStr Is Nothing = False Then
                If CharIndex <= (AppModeStr.Length - 1) Then
                    Return True
                End If
            End If
            Return False
        End Function

        Public Structure StrucAppEnabled
            Dim Customer As Boolean
            Dim Academic As Boolean
            Dim Marketing As Boolean
            Dim Purchasing As Boolean
            Dim Promotion As Boolean
            Dim CustAnalysis As Boolean
            Dim EmpCommission As Boolean
            Dim StkMaterial As Boolean
        End Structure

        Public Enum EnumAppMode
            Retail = 1
            Salon = 2
            Apparel = 3
            Invoicing = 4
        End Enum
    End Class
#End Region

#Region "Terminal Base"
    Public Class TerminalBase
#Region "Structures"
        Public Enum EnumTermMode
            CashRegister = 0
            SubStation = 1
        End Enum
        Public Enum EnumBizType
            Standard = 0
            Beauty = 1
            Pets = 3
        End Enum
        Public Structure StrucItemInfo
            Dim ItemType As String
            Dim ItemValue As String
        End Structure
        Public Enum EnumCustType
            Customer = 0
            Pet = 3
        End Enum

#End Region
        Public Shared ClosingStyle As String
        Public Shared AppMode As String
        Public Shared InServiceSlip As Boolean = False
        Public Shared AllowSales As Boolean = True
        Public Shared TermID As Integer
        Public Shared BizDate As Date
        Public Shared ShiftCode As String = String.Empty
        Public Shared TransNo As String
        Public Shared ReadRunNo As String = String.Empty
        Public Shared GroupAccessLevel As Integer
        Public Shared JournalPath As String = String.Empty
        Public Shared ReceiptLogo As String = String.Empty
        Public Shared DecimalPlace As Short = 2
        Public Shared ApplyCustomBackground As Boolean = False

        Public Shared NodeMainMenu As String
        Public Shared NodePayMenu As String
        Public Shared NodeItemMenu As String
        Public Shared ServerSelection As Integer = 0
        Public Shared FindItem As StrucItemInfo
        Public Shared CustomerID As String = String.Empty
        Public Shared CustType As EnumCustType = EnumCustType.Customer

#Region "Terminal Mode Check Function "
        Public Shared Function CheckClosingStyle() As String
            If ClosingStyle Is Nothing Then
                Return String.Empty
            Else
                Return ClosingStyle.Trim
            End If
        End Function
        Public Overloads Shared Function CheckTermMode(ByVal CharIndex As Integer, ByVal CheckValue As Char) As Boolean
            If CheckAppMode(CharIndex) Then
                If Convert.ToChar(AppMode.Substring(CharIndex, 1).ToUpper) = CheckValue Then
                    Return True
                Else
                    Return False
                End If
            End If
        End Function

        Public Overloads Shared Function CheckTermMode(ByVal ReturnIndex As Integer) As Char
            If CheckAppMode(ReturnIndex) Then
                Return Convert.ToChar(AppMode.Substring(ReturnIndex, 1))
            Else
                Return CType(String.Empty, Char)
            End If
        End Function

        Private Shared Function CheckAppMode(ByVal CharIndex As Integer) As Boolean
            If AppMode Is Nothing = False Then
                If CharIndex <= (AppMode.Length - 1) Then
                    Return True
                End If
            End If
            Return False
        End Function

        Public Shared Function ChangeToAmount(ByVal intAmt As Integer) As Double

            Dim strAmt As String
            Dim TempAmt As String

            strAmt = Convert.ToString(intAmt)

            Select Case strAmt.Length
                Case 0
                    Return 0
                Case 1
                    Return Convert.ToDouble("0.0" & strAmt)
                Case 2
                    Return Convert.ToDouble("0." & strAmt)
                Case Else
                    Try
                        TempAmt = Left(strAmt, strAmt.Length - 2)
                        Return Convert.ToDouble(TempAmt & "." & Right(strAmt, 2))
                    Catch ex As Exception
                        Return 0
                    End Try
            End Select
        End Function

        Public Shared Function ChangeToInteger(ByVal dblAmt As Double) As Integer
            Dim strAmt As String
            Dim TempAmt As String

            strAmt = Replace(FormatNumber(dblAmt, 2), ".", String.Empty)
            strAmt = Replace(strAmt, ",", String.Empty)

            Return Convert.ToInt32(strAmt)
        End Function

        'Public Shared Sub ChangeATMFormat(ByRef txtBox As TextBox)
        '    Dim str As String = txtBox.Text.Trim
        '    If IsNumeric(str) AndAlso CDbl(str) <> 0 Then
        '        txtBox.Text = FormatNumber(ChangeToAmt(Convert.ToInt64(Strings.Replace(str, ".", ""))), 2, , , TriState.False)
        '        txtBox.SelectionStart = txtBox.TextLength
        '    End If
        'End Sub


        'Public Shared Sub DisplayATMFormat(ByVal str As String, ByRef lblInput As Label)
        '    If str.Length > 0 Then
        '        If IsNumeric(str) AndAlso str.Length < 8 AndAlso CDbl(str) <> 0 Then
        '            lblInput.Text = FormatNumber(ChangeToAmt(Convert.ToInt64(Strings.Replace(str, ".", ""))), 2, , , TriState.False)
        '        Else
        '            lblInput.Text = str
        '        End If
        '    Else
        '        lblInput.Text = String.Empty
        '    End If

        'End Sub

        'Private Function ChangeToAmt(ByVal Amt As Long) As Double
        '    If Amt <> 0 Then
        '        Return Amt / 100
        '    End If
        'End Function

#End Region

        '0 - Not open yet
        '1 - Open already with no sales
        '2 - Open already with sales from Job Sheet
        '3 - Open already with sales from Normal Sales
        Public Shared SalesIndicator As Integer
        '0 - info not enter into customer analysis yet
        '1 - info already enter
        Public Shared AnalysisStatus As Integer
        Public Shared InServiceID As String
        Public Shared RecallTransNo As String
        Public Shared CustID As String
        Public Shared CustName As String
        'Academic Fees
        Public Shared FeesCourse As String
        Public Shared FeesMaintenance As String

        '0 - No Cash Register
        '1 - Got Cash Register
        Public Shared TerminalMode As EnumTermMode
        Public Shared BizType As EnumBizType
        Public Shared GreetingMsg As String = String.Empty

        Public Shared OpenMsg As String
        Public Shared CloseMsg As String
        Public Shared IdleMsg As String
        Public Shared AfterSalesMsg As String
        Public Shared LogOffMsg As String
        Public Shared IdleInterval As Integer = 15000
        Public Shared FloatingTrack As String
        Public Shared DummyTerminal As String
        Public Shared isPrimaryHost As Boolean = False

        Public Shared RestrictInput As Boolean
        Public Shared ReqCustID As Short
        Public Shared ReqServerID As Short
        Public Shared ReqReasonCode As Boolean
        Public Shared ReqTableNo As Short
        Public Shared ReqNoPax As Short
        Public Shared ReqShift As Boolean
        Public Shared ReqBioKey As Boolean = False
        Public Shared CustomerBranchFilter As Boolean
        Public Shared SameItemExcPolicy As String
        Public Shared SameItemExcQty As Integer
        Public Shared GlobalSearchPermission As Integer
        Public Shared CheckRefNo As Integer
        Public Shared JobStreetName As String
        Public Shared CustomerDetailFilter As Integer
        Public Shared ReqInvoice As Boolean
        Public Shared ReqPettyCash As Boolean
        Public Shared ReqVoucher As Boolean
        Public Shared ReqCashPoint As Boolean
        Public Shared ReqSubCustomer As Boolean

        Public Shared ReceiptCopies As Integer = 1
        Public Shared ZReadHeaderCopies As Integer = 1
        Public Shared InServiceVerify As Boolean = True
        Public Shared NormalSalesVerify As Boolean = False
        Public Shared SlipStartPageHeight As Integer = 500
        Public Shared SlipWaitInterval As Integer = 20000
        Public Shared ResetTransNo As Boolean = False
        Public Shared AutoConfirmPayment As Boolean = True
        Public Shared ShowSurvey As Boolean = False
        Public Shared SendSMS As Boolean = False
        Public Shared ShowEmpSales As Boolean = True

        Public Shared LogoPosition As Integer = 1
        Public Shared JobSheetCopies As Integer = 1
        Public Shared TicketCopies As Integer = 1
        Public Shared DirectPrintJobSheet As Boolean = True
        Public Shared DirectPrintTicket As Boolean = True
        Public Shared PrintJobSheet As Boolean = True
        Public Shared PrintBrandSales As Boolean
        Public Shared PrintCatgSales As Boolean
        Public Shared PrintDeptSales As Boolean
        Public Shared NoXZReadNo As Boolean
        Public Shared ReturnSalesZero As Boolean
        Public Shared PrintGrossSales As Boolean


        Public Shared TaxGroup As String = String.Empty
        Public Shared CardAccessComm As Integer = 1
        '1-RoundUp
        '2-RoundDown
        Public Shared RoundAmt As EnumRoundAmt = EnumRoundAmt.RoundUp

        'Appointment
        Public Shared ShowCompletedAppt As Boolean = True
        Public Shared ShowCancelledAppt As Boolean = True

        Public Shared ShowSystemAmt As Boolean = True

        'Default Setting
        Public Shared DefCustomerID As String = String.Empty
        Public Shared DefServerID As String = String.Empty
        Public Shared DefCNExpiryDay As Int32 = 30
        Public Shared DefVeriType As Integer = 0
        Public Shared PointIncludeTax As Boolean = False
        Public Shared PointRoundUp As Boolean = True

        Public Shared TenantPrefix As String = String.Empty
        Public Shared ReqSaveTenant As Integer = 0
        Public Shared TenantPath As String = String.Empty
        Public Shared TenantTemp As String = String.Empty

        Public Shared ReprintSelection As Boolean = False
        'Public Shared ReprintNoOfList As Integer = 15
        Public Shared PrintReceiptList As Integer = 0
        Public Shared PrintItemSummaryList As Boolean = False
        Public Shared PrintEmpSales As Boolean = False
        Public Shared SumVoucherAmt As Boolean = False
        Public Shared ReceiptStyle As Boolean = False

        Public Shared AutoMaterialPosting As Boolean = False
        Public Shared ShowAcademic As Boolean = False
        Public Shared TransferFromTrans As Boolean

        'TASK LIST
        Public Shared GenerateStkTakeImg As Boolean
        Public Shared AllowStkRecieve As Boolean
        Public Shared AllowStkTransfer As Boolean
        Public Shared AllowSpecialDiscount As Boolean
        Public Shared AllowReturnBelow As Boolean

        Public Shared ValidateCustomer As Boolean
        Public Shared ValidateKey As Boolean
        Public Shared ValidateKeys As String = Nothing
        Public Shared CheckStkRcvDate As Boolean = False

        Public Shared FPThreshold As Integer = 10

        Public Shared dsMenu As DataSet

        Public Sub New()

        End Sub
    End Class
#End Region
    Public Class Validation
        Public Shared Function ValidateDateValue(ByVal Value As Date) As Boolean
            If Value.ToString = "01/01/0001 00:00:00" Then
                Return False
            ElseIf Value.Year < 1000 Then
                Return False
            Else
                Return True
            End If
        End Function
    End Class

#Region "Calculation Base"
    Public Class Calculation
        Private Shared dblCost, dblPrice, dblMargin As Double

        Public Shared Function CalPriceOnMargin(ByVal BaseCost As Double, ByVal Margin As Double) As Double
            Dim Round As Double
            If Margin = 0 Then
                Return BaseCost
            Else
                dblMargin = (Margin / 100) + 1
                Round = (BaseCost * dblMargin)
                Return Math.Round(Round, 2)
            End If
        End Function

        Public Shared Function CalMarginOnPrice(ByVal BaseCost As Double, ByVal Price As Double) As Double
            Dim Round As Double
            If BaseCost = 0 Then
                Return Price
            Else
                dblMargin = (Price / BaseCost) - 1
                Round = (dblMargin * 100)
                Return Math.Round(Round, 4)
            End If
        End Function

        'Public Overloads Shared Function RoundValue(ByVal Value As Double, ByVal DecimalPlace As Double) As Double
        '    Dim integervalue As Double
        '    Dim decimalvalue As Double
        '    Dim value1 As Double
        '    Dim value2 As Double
        '    Dim value3 As Double
        '    Dim Result As Double
        '    Dim i As Double

        '    integervalue = Int(Value)
        '    decimalvalue = Value - integervalue
        '    i = 1
        '    Do
        '        value1 = decimalvalue * 10
        '        decimalvalue = value1
        '        i = i + 1
        '    Loop While i <= DecimalPlace
        '    value2 = Int(value1)
        '    value3 = (value1 - value2) * 10
        '    If Int(value3) < 5 Then
        '        Result = integervalue + (value2 / (10 ^ DecimalPlace))
        '    ElseIf Int(value3) >= 5 Then
        '        Result = integervalue + ((value2 + 1) / (10 ^ DecimalPlace))
        '    End If
        '    Return Result
        'End Function

        'Public Shared Function TransactionRoundAmount(ByVal Amount As Double, ByVal BranchID As String) As Double
        '    Dim dtRound As DataTable
        '    Dim sAmt1, sAmt2, sAmt3, sAmt4 As String
        '    Dim iLength As Int32
        '    Dim dvRound As DataView
        '    Dim drvRound As DataRowView
        '    Dim sCondition As String
        '    Dim blnRound As Boolean = False
        '    Dim iCount As Int32 = 0


        '    Try
        '        sAmt1 = Replace(FormatNumber(Amount, 2), ",", string.empty)
        '        sAmt2 = sAmt1
        '        sAmt1 = Replace(sAmt1, ".", string.empty)

        '        dtRound = General.RoundSetupBase.GetRoundSetup(BranchID)
        '        iLength = sAmt1.Length

        '        Do While iLength > 0

        '            sAmt3 = Mid(sAmt1, (sAmt1.Length - iLength) + 1)
        '            If sAmt3.Length >= 3 Then
        '                sAmt3 = Mid(sAmt3, 1, sAmt3.Length - 2) & "." & Right(sAmt3, 2)
        '            Else
        '                If sAmt3.Length = 2 Then
        '                    sAmt3 = "0." & sAmt3
        '                Else
        '                    sAmt3 = "0.0" & sAmt3
        '                End If
        '            End If

        '            sCondition = "RoundType = " & iLength.ToString & " AND RoundFrom <= " & sAmt3 & " AND RoundTo >= " & sAmt3
        '            dvRound = New DataView(dtRound, sCondition, "RoundFrom, RoundCode", DataViewRowState.CurrentRows)

        '            If dvRound.Count > 0 Then

        '                If sAmt1.Length = iLength Then
        '                    sAmt4 = String.Empty
        '                Else


        '                    sAmt4 = Mid(sAmt1, 1, sAmt1.Length - iLength)
        '                    For iCount = (sAmt1.Length - iLength) + 1 To sAmt1.Length
        '                        sAmt4 = sAmt4 & "0"
        '                    Next
        '                    sAmt4 = Mid(sAmt4, 1, sAmt4.Length - 2) & "." & Right(sAmt4, 2)

        '                End If


        '                For Each drvRound In dvRound
        '                    If sAmt1.Length = iLength Then
        '                        sAmt4 = Convert.ToString(drvRound("RoundAmt"))
        '                    Else
        '                        sAmt4 = Convert.ToString(Convert.ToDouble(sAmt4) + Convert.ToDouble(drvRound("RoundAmt")))
        '                    End If


        '                    Return Convert.ToDouble(sAmt4)
        '                Next
        '            End If


        '            iLength = iLength - 1
        '            dvRound.Dispose()
        '        Loop

        '        Return Amount

        '    Catch ex As Exception
        '        Return Amount
        '    Finally

        '    End Try
        'End Function

        Public Overloads Shared Function RoundValue(ByVal Value As Double, ByVal DecimalPlace As Integer) As Double
            Dim sign As Integer
            Dim round, scale As Double

            sign = Math.Sign(Value)
            scale = Math.Pow(10.0, DecimalPlace)
            round = Math.Floor(Math.Abs(Value) * scale + 0.5)
            Return (sign * round / scale)
        End Function

        Public Shared Function TransactionRoundAmount(ByVal Amount As Double, ByVal BranchID As String) As Double
            Dim dtRound As DataTable
            Dim sAmt1, sAmt2, sAmt3, sAmt4, sTempAmt As String
            Dim iLength As Int32
            Dim dvRound As DataView
            Dim drvRound As DataRowView
            Dim sCondition As String
            Dim blnRound As Boolean = False
            Dim iCount As Int32 = 0


            Try
                sAmt1 = Replace(FormatNumber(Amount, 2), ",", String.Empty)
                sAmt2 = sAmt1
                sAmt1 = Replace(sAmt1, ".", String.Empty)

                dtRound = General.RoundSetupBase.GetRoundSetup(BranchID)
                iLength = sAmt1.Length

                Do While iLength > 0

                    sAmt3 = Mid(sAmt1, (sAmt1.Length - iLength) + 1)
                    If sAmt3.Length >= 3 Then
                        sTempAmt = Left(sAmt3, 1)
                        sAmt3 = Mid(sAmt3, 1, sAmt3.Length - 2)

                        For iCount = 2 To sAmt3.Length
                            sTempAmt = sTempAmt & "0"
                        Next

                        sAmt3 = sTempAmt & ".00"
                    Else
                        If sAmt3.Length = 2 Then
                            sAmt3 = "0." & Left(sAmt3, 1) & "0"
                        Else
                            sAmt3 = "0.0" & sAmt3
                        End If
                    End If

                    sCondition = "RoundType = " & iLength.ToString & " AND RoundFrom <= " & sAmt3 & " AND RoundTo >= " & sAmt3
                    dvRound = New DataView(dtRound, sCondition, "RoundFrom, RoundCode", DataViewRowState.CurrentRows)

                    If dvRound.Count > 0 Then

                        If sAmt1.Length = iLength Then
                            sAmt4 = String.Empty
                        Else


                            sAmt4 = Mid(sAmt1, 1, sAmt1.Length - iLength)
                            For iCount = (sAmt1.Length - iLength) + 1 To sAmt1.Length
                                sAmt4 = sAmt4 & "0"
                            Next
                            sAmt4 = Mid(sAmt4, 1, sAmt4.Length - 2) & "." & Right(sAmt4, 2)

                        End If


                        For Each drvRound In dvRound
                            If sAmt1.Length = iLength Then
                                sAmt4 = Convert.ToString(drvRound("RoundAmt"))
                            Else
                                sAmt4 = Convert.ToString(Convert.ToDouble(sAmt4) + Convert.ToDouble(drvRound("RoundAmt")))
                            End If


                            Return Convert.ToDouble(sAmt4)
                        Next
                    End If


                    iLength = iLength - 1
                    dvRound.Dispose()
                Loop

                Return Amount

            Catch ex As Exception
                Return Amount
            Finally

            End Try
        End Function
    End Class
#End Region

#Region "ErrorLog "
    Public Class ErrorLog
        Protected strFilePath As String
        Protected Structure StrucErrorSet
            Dim ErrorType As String
            Dim ErrorDateTime As String
            Dim ErrorDesc As String
            Dim ErrorSource As String
            Dim ErrorStack As String
            Dim ErrorCaused As Object
            Dim HelpLink As String
            Dim Option1 As String
            Dim Option2 As String
        End Structure

        Public Sub New(ByVal FilePath As String)
            strFilePath = FilePath
            FileCheck()
        End Sub

        Public Sub CreateLogFile(ByVal FilePath As String)
            Dim ds As System.Data.DataSet = New System.Data.DataSet("ErrorSet")
            Dim dt As System.Data.DataTable = New System.Data.DataTable
            Dim dc0 As System.Data.DataColumn = New System.Data.DataColumn("Error_Type", System.Type.GetType("System.String"))
            Dim dc1 As System.Data.DataColumn = New System.Data.DataColumn("Error_DateTime", System.Type.GetType("System.DateTime"))
            Dim dc2 As System.Data.DataColumn = New System.Data.DataColumn("Error_Description", System.Type.GetType("System.String"))
            Dim dc3 As System.Data.DataColumn = New System.Data.DataColumn("Error_Source", System.Type.GetType("System.String"))
            Dim dc4 As System.Data.DataColumn = New System.Data.DataColumn("Error_Stack_Trace", System.Type.GetType("System.String"))
            Dim dc5 As System.Data.DataColumn = New System.Data.DataColumn("Error_Caused_By_Method", System.Type.GetType("System.String"))
            Dim dc6 As System.Data.DataColumn = New System.Data.DataColumn("Help_Link", System.Type.GetType("System.String"))
            Dim dc7 As System.Data.DataColumn = New System.Data.DataColumn("Optional_Text_1", System.Type.GetType("System.String"))
            Dim dc8 As System.Data.DataColumn = New System.Data.DataColumn("Optional_Text_2", System.Type.GetType("System.String"))
            With dt
                .TableName = "ErrData"
                .Columns.Add(dc0)
                .Columns.Add(dc1)
                .Columns.Add(dc2)
                .Columns.Add(dc3)
                .Columns.Add(dc4)
                .Columns.Add(dc5)
                .Columns.Add(dc6)
                .Columns.Add(dc7)
                .Columns.Add(dc8)
            End With
            ds.Tables.Add(dt)
            ds.WriteXml(FilePath, System.Data.XmlWriteMode.WriteSchema)
        End Sub

        Private Sub FileCheck()
            If Not System.IO.File.Exists(strFilePath) Then
                CreateLogFile(strFilePath)
            End If
        End Sub

        Private Sub SaveErrorLog(ByVal Value As StrucErrorSet)
            Dim ds As System.Data.DataSet = New System.Data.DataSet("ErrorSet")
            Dim drow As System.Data.DataRow
            Try
                FileCheck()
                ds.ReadXml(strFilePath)
                drow = ds.Tables(0).NewRow()
                With Value
                    If .Option1 Is Nothing = True Then .Option1 = String.Empty
                    If .Option2 Is Nothing = True Then .Option2 = String.Empty
                    drow(0) = .ErrorType
                    drow(1) = .ErrorDateTime
                    drow(2) = .ErrorDesc
                    drow(3) = .ErrorSource
                    drow(4) = .ErrorStack
                    drow(5) = .ErrorCaused
                    drow(6) = .HelpLink
                    drow(7) = .Option1
                    drow(8) = .Option2
                End With
                With ds
                    .Tables(0).Rows.Add(drow)
                    .AcceptChanges()
                    .WriteXml(strFilePath, System.Data.XmlWriteMode.WriteSchema)
                End With
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                drow = Nothing
                ds.Dispose()
            End Try
        End Sub

        Private Function ReadErrorLog() As DataSet
            Dim ds As System.Data.DataSet = New System.Data.DataSet("ErrorSet")
            Dim drow As System.Data.DataRow
            Try
                FileCheck()
                ds.ReadXml(strFilePath)
                Return ds
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                drow = Nothing
                ds.Dispose()
            End Try
        End Function

        Private Sub ProcessErrorLog(ByVal ex As Exception,
        ByVal Option1 As String, ByVal Option2 As String)
            Dim ErrorSet As StrucErrorSet
            With ex
                ErrorSet.ErrorType = .GetType.ToString
                ErrorSet.ErrorDateTime = System.DateTime.Now.ToString()
                ErrorSet.ErrorDesc = .Message
                ErrorSet.ErrorSource = .Source
                ErrorSet.ErrorStack = .StackTrace
                ErrorSet.ErrorCaused = .TargetSite
                ErrorSet.HelpLink = .HelpLink
                ErrorSet.Option1 = Option1
                ErrorSet.Option2 = Option2
            End With
            SaveErrorLog(ErrorSet)
        End Sub

        Public Function ReadExceptionLog() As DataSet
            Return ReadErrorLog()
        End Function

        Public Sub SaveExceptionToLog(ByVal ex As Exception,
        Optional ByVal Option1 As String = "", Optional ByVal Option2 As String = "")
            ProcessErrorLog(ex, Option1, Option2)
        End Sub
    End Class
#End Region

#Region "Sync Log "
    Public Class SyncLogger
        Protected strFilePath As String

        Public Sub New(ByVal FilePath As String)
            strFilePath = FilePath
        End Sub

        Public Sub WriteLogFile(ByVal str As String)
            Dim ObjFile As System.IO.File
            Dim objWriter As System.IO.StreamWriter
            Try
                objWriter = ObjFile.AppendText(strFilePath)
                objWriter.WriteLine(str)
                objWriter.Flush()
                objWriter.Close()
            Catch ex As Exception
                objWriter.Close()
            End Try
        End Sub

    End Class


#End Region

End Namespace

