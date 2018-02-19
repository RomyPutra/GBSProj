Imports System.Text
Imports System.IO
Imports SEAL.Data
Imports System.Data
Imports System.Security.Cryptography

Imports Sharpbrake.Client
Imports System.Configuration

Namespace General
#Region "Application Settings"
    Public Class AppSettings
        Inherits Core.CoreBase
        Protected Shared htbSettings As Hashtable

        Public Shared Function InitalizeSettings(ByVal SettingsEnum As EnumSettings, ByVal BranchID As String, Optional ByVal TermID As Integer = 1) As Boolean
            Dim rdrSys As SqlClient.SqlDataReader
            Try
                InitalizeSettings = False
                If StartConnection() = True Then
                    If SettingsEnum = EnumSettings.adBackEnd Then
                        strSQL = "SELECT SYSKEY, SYSVALUE FROM SYSPREFB WHERE BranchID='" & BranchID & "'"
                    ElseIf SettingsEnum = EnumSettings.adFrontEnd Then
                        strSQL = "SELECT SYSKEY, SYSVALUE FROM SYSPREFT WHERE BranchID='" & BranchID & "' AND TermID=" & TermID
                    End If
                    rdrSys = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
                    If htbSettings Is Nothing Then htbSettings = New Hashtable
                    htbSettings.Clear()

                    If rdrSys Is Nothing = False Then
                        With rdrSys
                            While .Read
                                htbSettings.Add(.Item("SysKey"), .Item("SysValue"))
                            End While
                            .Close()
                        End With
                    End If
                    InitalizeSettings = True
                End If
                Return InitalizeSettings
            Catch ex As Exception
                Return False
            Finally
                rdrSys = Nothing
                EndConnection()
            End Try
        End Function

        Public Shared Function GetSetting(ByVal Syskey As String) As String
            If htbSettings Is Nothing Then
                Return Nothing
            Else
                If htbSettings.ContainsKey(Syskey) = True Then
                    Return Convert.ToString(htbSettings.Item(Syskey))
                Else
                    Return String.Empty
                End If
            End If
        End Function
    End Class
#End Region

#Region "SysCode Base"
    Public Class SysCodeBase
        Inherits Core.CoreBase
        Protected Shared htbKeys As New Hashtable

#Region "SysCode class in SysCode Base"
        Public Class SysCode
            Protected strCode, strDesc, strUpdBy As String
            Protected strPrefix, strPostfix, strSpCode As String
            Protected strBranch As String
            Protected intRun, intLength, intPos As Integer
            Protected intStatus, intSysID As Integer
            Protected dtLastUpd As DateTime
            Public MyScheme As New MyScheme

            Public Property BranchID() As String
                Get
                    Return strBranch
                End Get
                Set(ByVal Value As String)
                    strBranch = Value.Trim
                End Set
            End Property

            Public Property SysCode() As String
                Get
                    Return strCode
                End Get
                Set(ByVal Value As String)
                    strCode = Value
                End Set
            End Property

            Public Property Description() As String
                Get
                    Return strDesc
                End Get
                Set(ByVal Value As String)
                    strDesc = Value
                End Set
            End Property

            Public Property Prefix() As String
                Get
                    Return strPrefix
                End Get
                Set(ByVal Value As String)
                    strPrefix = Value.Trim
                End Set
            End Property

            Public Property Postfix() As String
                Get
                    Return strPostfix
                End Get
                Set(ByVal Value As String)
                    strPostfix = Value.Trim
                End Set
            End Property

            Public Property SpCode() As String
                Get
                    Return strSpCode
                End Get
                Set(ByVal Value As String)
                    strSpCode = Value.Trim
                End Set
            End Property

            Public Property Runno() As Integer
                Get
                    Return intRun
                End Get
                Set(ByVal Value As Integer)
                    intRun = Value
                    If intRun <= 0 Then intRun = 1
                End Set
            End Property

            Public Property Length() As Integer
                Get
                    Return intLength
                End Get
                Set(ByVal Value As Integer)
                    intLength = Value
                    If intLength <= 0 Then intLength = 1
                End Set
            End Property

            Public Property Pos() As Integer
                Get
                    Return intPos
                End Get
                Set(ByVal Value As Integer)
                    intPos = Value
                    If intPos <= 0 Then intPos = 1
                End Set
            End Property

            Public Property LastUpdate() As DateTime
                Get
                    Return dtLastUpd
                End Get
                Set(ByVal Value As DateTime)
                    dtLastUpd = Value
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return strUpdBy
                End Get
                Set(ByVal Value As String)
                    strUpdBy = Value
                End Set
            End Property

            Public Property Status() As Integer
                Get
                    Return intStatus
                End Get
                Set(ByVal Value As Integer)
                    intStatus = Value
                End Set
            End Property
            Public Property SysID() As Integer
                Get
                    Return intSysID
                End Get
                Set(ByVal Value As Integer)
                    intSysID = Value
                    If intSysID < 0 Then intSysID = 0
                End Set
            End Property
        End Class
#End Region

#Region "Stock Scheme"
        Public Class MyScheme
            Inherits Core.SchemeBase

            Protected Overrides Sub InitializeInfo()
                
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "SysCode"
                    .Length = 20
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(0, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "SysDesc"
                    .Length = 50
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(1, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "Prefix"
                    .Length = 10
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = False
                    .AllowNegative = False
                End With
                MyBase.AddItem(2, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "SpCode"
                    .Length = 10
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(3, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "RunNo"
                    .Length = 10
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(4, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "NoLength"
                    .Length = 2
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(5, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "NoPos"
                    .Length = 2
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(6, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "Postfix"
                    .Length = 10
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(7, this)
            End Sub

            Public ReadOnly Property SysCode() As StrucElement
                Get
                    Return MyBase.GetItem(0)
                End Get
            End Property

            Public ReadOnly Property SysDesc() As StrucElement
                Get
                    Return MyBase.GetItem(1)
                End Get
            End Property
            Public ReadOnly Property Prefix() As StrucElement
                Get
                    Return MyBase.GetItem(2)
                End Get
            End Property
            Public ReadOnly Property SpCode() As StrucElement
                Get
                    Return MyBase.GetItem(3)
                End Get
            End Property

            Public ReadOnly Property RunNo() As StrucElement
                Get
                    Return MyBase.GetItem(4)
                End Get
            End Property
            Public ReadOnly Property NoLength() As StrucElement
                Get
                    Return MyBase.GetItem(5)
                End Get
            End Property

            Public ReadOnly Property NoPos() As StrucElement
                Get
                    Return MyBase.GetItem(6)
                End Get
            End Property
            Public ReadOnly Property Postfix() As StrucElement
                Get
                    Return MyBase.GetItem(7)
                End Get
            End Property
        End Class
#End Region

        Protected Friend Structure StrucTrans
            Dim Prefix As String
            Dim SpCode As String
            Dim Length As Integer
            Dim NoPos As Short
            Dim Runno As Integer
            Dim Postfix As String
        End Structure

        Protected Shared TransFormat As StrucTrans
        'Private Shared blnCached As Boolean

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "BranchID, SysCode, SysDesc, Prefix, SpCode, RunNo, NoLength, NoPos, Postfix, LastUpdate, UpdateBy, Status, SysID"
                .CheckFields = "SysCode"
                .TableName = "SYSCODEB"
                .DefaultCond = "SYSCODEB.Status=1"
                .DefaultOrder = String.Empty
                .Listing = "BranchID, SysCode, SysDesc, Prefix, SpCode, RunNo, NoLength, NoPos, Postfix, LastUpdate, UpdateBy, Status, SysID"
                .ListingCond = String.Empty
                .ShortList = "SysCode, SysDesc, RunNo"
                .ShortListCond = String.Empty
            End With
            ReDim MyFields(1)
            MyBase.AddMyField(0, "SysCode", "SysCode", TypeCode.String)
            MyBase.AddMyField(1, "SysDesc", "SysDesc", TypeCode.String)
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function AssignItem(ByVal objSysCode As SysCode, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim blnExec, blnFound As Boolean
            Dim rdr As SqlClient.SqlDataReader
            AssignItem = False
            Try
                blnExec = False
                blnFound = False
                If objSysCode Is Nothing Then
                    'Message return
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "SysCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objSysCode.SysCode) & "' " & _
                                        "AND BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objSysCode.BranchID) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
                        blnExec = True

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                End If
                                rdr.Close()
                            End With
                        End If
                    End If
                End If

                If blnExec Then
                    If blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                        Throw New ApplicationException("210011")
                    Else
                        StartSQLControl()
                        With objSQL
                            .TableName = MyInfo.TableName
                            .AddField("BranchID", objSysCode.BranchID, SQLControl.EnumDataType.dtString)
                            .AddField("SysDesc", objSysCode.Description, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                            .AddField("Prefix", objSysCode.Prefix, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                            .AddField("SpCode", objSysCode.SpCode, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                            .AddField("RunNo", objSysCode.Runno.ToString, SQLControl.EnumDataType.dtNumeric)
                            .AddField("NoLength", objSysCode.Length.ToString, SQLControl.EnumDataType.dtNumeric)
                            .AddField("NoPos", objSysCode.Pos.ToString, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Postfix", objSysCode.Postfix, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                            .AddField("UpdateBy", objSysCode.UpdateBy, SQLControl.EnumDataType.dtString)
                            .AddField("LastUpdate", objSysCode.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                            .AddField("Status", objSysCode.Status.ToString, SQLControl.EnumDataType.dtNumeric)

                            Select Case pType
                                Case SQLControl.EnumSQLType.stInsert
                                    If blnFound = True Then
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                        "WHERE SysCode='" & .ParseValue(SQLControl.EnumDataType.dtString, objSysCode.SysCode) & "' " & _
                                        "AND BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objSysCode.BranchID) & "'")
                                    Else
                                        If blnFound = False Then
                                            .AddField("SysCode", objSysCode.SysCode, SQLControl.EnumDataType.dtString)
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                        End If
                                    End If
                                Case SQLControl.EnumSQLType.stUpdate
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                    "WHERE SysCode='" & .ParseValue(SQLControl.EnumDataType.dtString, objSysCode.SysCode) & "' " & _
                                        "AND BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objSysCode.BranchID) & "'")
                            End Select
                        End With
                        Try
                            'execute 
                            objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                        Catch axExecute As Exception
                            If pType = SQLControl.EnumSQLType.stInsert Then
                                Throw New ApplicationException("210002")
                            Else
                                Throw New ApplicationException("210004")
                            End If
                        Finally
                            objSQL.Dispose()
                        End Try
                        Return True
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objSysCode = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Add(ByVal objSysCode As SysCode) As Boolean
            Return AssignItem(objSysCode, SQLControl.EnumSQLType.stInsert)
        End Function

        'AMEND
        Public Function Amend(ByVal objSysCode As SysCode) As Boolean
            Return AssignItem(objSysCode, SQLControl.EnumSQLType.stUpdate)
        End Function

        'DELETE
        Public Function Delete(ByVal objSysCode As SysCode) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False

            Try
                If objSysCode Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "SysCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objSysCode.SysCode) & "' " & _
                                        "AND BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objSysCode.BranchID) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                End If
                                .Close()
                            End With
                        End If
                        If blnFound = False Then
                            'Error Message
                            Return False
                        End If
                        If blnFound = True Then
                            With objSQL
                                If objSysCode.SysID = 1 Then
                                    .TableName = .TableName
                                    .AddField("Active", cFlagNonActive.ToString, SQLControl.EnumDataType.dtNumeric)
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                        "WHERE SysCode='" & .ParseValue(SQLControl.EnumDataType.dtString, objSysCode.SysCode) & "' " & _
                                        "AND BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objSysCode.BranchID) & "'")
                                Else
                                    strSQL = BuildDelete(MyInfo.TableName, "SysCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objSysCode.SysCode) & "' " & _
                                        "AND BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objSysCode.BranchID) & "'")
                                End If
                            End With
                        End If
                        Try
                            'execute 
                            objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try
                    End If
                    End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As SystemException
                Throw exDelete
            Finally
                objSysCode = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        'Enquiry
        Function Enquiry(Optional ByVal SQLstmt As String = Nothing) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    If SQLstmt = Nothing Or SQLstmt = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName)
                    Else
                        If SQLstmt.IndexOf("WHERE") = -1 Then
                            strSQL = strSQL.Concat(SQLstmt, " WHERE " & .DefaultCond)
                        Else
                            strSQL = strSQL.Concat(SQLstmt, " AND " & .DefaultCond)
                        End If

                    End If
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetListing() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetShortList() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function
#End Region

        Protected Shared Function CheckySysKey(ByVal Value As String, ByVal Key As String, ByVal KeyReplace As String) As String
            If Value.IndexOf(Key) >= 0 Then
                Return Value.Replace(Key, KeyReplace)
            Else
                Return Value
            End If
        End Function

        Protected Shared Function CheckySysKey(ByVal Value As String) As String
            Dim item As Object
            Dim Key, KeyReplace As String
            If htbKeys Is Nothing = False Then
                For Each item In htbKeys.Keys
                    Key = Convert.ToString(item)
                    KeyReplace = Convert.ToString(htbKeys(item))
                    If Value.IndexOf(Key) >= 0 Then
                        Value = Value.Replace(Key, KeyReplace)
                    End If
                Next
            End If
            Return Value
        End Function

        Public Shared Function GenerateVoucherImportDocCode(ByVal Branch As String) As String
            Dim strImportDocCode, strSQL As String
            'Dim intRunNo As Integer
            'Dim rdr As SqlClient.SqlDataReader
            'With htbKeys
            '    .Clear()
            '    .Add("#DP", Dept)
            '    .Add("#CT", Category)
            'End With

            Try
                If StartConnection() = True Then
                    TransFormat = GetSysCodeFormat(Branch, "ISV")

                    strImportDocCode = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                    TransFormat.Postfix, TransFormat.Runno, TransFormat.NoPos, TransFormat.Length, _
                                    Today, Branch)
                    strImportDocCode = CheckySysKey(strImportDocCode)

                    'Update Run No.
                    strSQL = "UPDATE SYSCODEB SET RunNo=RunNo+1 WHERE BranchID = '" & Branch & "' AND SYSCODE='ISV'"
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

                    Return strImportDocCode
                End If
            Catch errGen As Exception
                Return String.Empty
            Finally
                EndConnection()
            End Try
        End Function

        Public Shared Function GenerateStockID(ByVal Branch As String, ByVal Dept As String, _
                ByVal Category As String, ByVal CurDate As Date) As String
            Dim strStockID, strSQL As String
            Dim intRunNo As Integer
            'Dim rdr As SqlClient.SqlDataReader
            With htbKeys
                .Clear()
                .Add("#DP", Dept)
                .Add("#CT", Category)
            End With

            Try
                If StartConnection() = True Then
                    TransFormat = GetSysCodeFormat(Branch, "STK")

                    strStockID = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                    TransFormat.Postfix, TransFormat.Runno, TransFormat.NoPos, TransFormat.Length, _
                                    CurDate, Branch)
                    strStockID = CheckySysKey(strStockID)

                    'Update Run No.
                    strSQL = "UPDATE SYSCODEB SET RunNo=RunNo+1 WHERE BranchID = '" & Branch & "' AND SYSCODE='STK'"
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

                    Return strStockID
                End If
            Catch errGen As Exception
                Return String.Empty
            Finally
                EndConnection()
            End Try
        End Function

        Public Shared Function GenerateBlockCode(ByVal BranchID As String, ByVal CurDate As Date) As String
            Dim strCompID, strSQL As String
            Dim intRunNo As Integer
            Try
                If StartConnection() = True Then
                    TransFormat = GetSysCodeFormat(BranchID, "BLK")
                    strCompID = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                   TransFormat.Postfix, TransFormat.Runno, TransFormat.NoPos, TransFormat.Length, _
                                   CurDate, BranchID)
                    strSQL = "UPDATE SYSCODEB SET RunNo=RunNo+1 WHERE BranchID = '" & BranchID & "' AND SYSCODE='BLK'"
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                    Return strCompID
                End If
            Catch ex As Exception
                Return String.Empty
            Finally
                EndConnection()
            End Try
        End Function

        Public Shared Function GeneratePlanID(ByVal BranchID As String, ByVal CurDate As Date) As String
            Dim strPlanID, strSQL As String
            Dim intRunNo As Integer
            Try
                If StartConnection() = True Then
                    TransFormat = GetSysCodeFormat(BranchID, "PLN")
                    strPlanID = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                   TransFormat.Postfix, TransFormat.Runno, TransFormat.NoPos, TransFormat.Length, _
                                   CurDate, BranchID)
                    strSQL = "UPDATE SYSCODEB SET RunNo=RunNo+1 WHERE BranchID = '" & BranchID & "' AND SYSCODE='PLN'"
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                    Return strPlanID
                End If
            Catch ex As Exception
                Return String.Empty
            Finally
                EndConnection()
            End Try
        End Function

        Public Shared Function GenerateSuppID(ByVal Branch As String, ByVal CurDate As Date) As String
            Dim strSuppID, strSQL As String
            Dim intRunNo As Integer
            'Dim rdr As SqlClient.SqlDataReader
            Try
                If StartConnection() = True Then
                    TransFormat = GetSysCodeFormat(Branch, "SUP")

                    strSuppID = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                    TransFormat.Postfix, TransFormat.Runno, TransFormat.NoPos, TransFormat.Length, _
                                    CurDate, Branch)
                    'strCustID = CheckySysKey(strCustID)
                    'Update Run No.
                    strSQL = "UPDATE SYSCODEB SET RunNo=RunNo+1 WHERE BranchID = '" & Branch & "' AND SYSCODE='SUP'"
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

                    Return strSuppID
                End If
            Catch errGen As Exception
                Return String.Empty
            Finally
                EndConnection()
            End Try
        End Function

        Public Shared Function GenerateCustID(ByVal Branch As String, ByVal CurDate As Date, Optional ByVal TermID As Integer = -1) As String
            Dim strCustID, strSQL As String
            Dim intRunNo As Integer
            'Dim rdr As SqlClient.SqlDataReader
            Try
                If StartConnection() = True Then
                    TransFormat = GetSysCodeFormat(Branch, "CUS")

                    strCustID = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                    TransFormat.Postfix, TransFormat.Runno, TransFormat.NoPos, TransFormat.Length, _
                                    CurDate, Branch, TermID)
                    'strCustID = CheckySysKey(strCustID)
                    'Update Run No.
                    strSQL = "UPDATE SYSCODEB SET RunNo=RunNo+1 WHERE BranchID = '" & Branch & "' AND SYSCODE='CUS'"
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

                    Return strCustID
                End If
            Catch errGen As Exception
                Return String.Empty
            Finally
                EndConnection()
            End Try
        End Function

        Public Shared Function GenerateCompanyID(ByVal BranchID As String, ByVal CurDate As Date) As String
            Dim strCompID, strSQL As String
            Dim intRunNo As Integer
            Try
                If StartConnection() = True Then
                    TransFormat = GetSysCodeFormat(BranchID, "CMP")
                    strCompID = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                   TransFormat.Postfix, TransFormat.Runno, TransFormat.NoPos, TransFormat.Length, _
                                   CurDate, BranchID)
                    strSQL = "UPDATE SYSCODEB SET RunNo=RunNo+1 WHERE BranchID = '" & BranchID & "' AND SYSCODE='CMP'"
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                    Return strCompID
                End If
            Catch ex As Exception
                Return String.Empty
            Finally
                EndConnection()
            End Try
        End Function

        Public Shared Function GenerateJPPInvoiceID(ByVal BranchID As String, ByVal CurDate As Date) As String
            Dim strCompID, strSQL As String
            Dim intRunNo As Integer
            Try
                If StartConnection() = True Then
                    TransFormat = GetSysCodeFormat(BranchID, "INV")
                    strCompID = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                   TransFormat.Postfix, TransFormat.Runno, TransFormat.NoPos, TransFormat.Length, _
                                   CurDate, BranchID)
                    strSQL = "UPDATE SYSCODEB SET RunNo=RunNo+1 WHERE BranchID = '" & BranchID & "' AND SYSCODE='INV'"
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                    Return strCompID
                End If
            Catch ex As Exception
                Return String.Empty
            Finally
                EndConnection()
            End Try
        End Function

        Public Shared Function GenerateShippingCode(ByVal BranchID As String, ByVal CurDate As Date) As String
            Dim strCompID, strSQL As String
            Dim intRunNo As Integer
            Try
                If StartConnection() = True Then
                    TransFormat = GetSysCodeFormat(BranchID, "SHP")
                    strCompID = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                   TransFormat.Postfix, TransFormat.Runno, TransFormat.NoPos, TransFormat.Length, _
                                   CurDate, BranchID)
                    strSQL = "UPDATE SYSCODEB SET RunNo=RunNo+1 WHERE BranchID = '" & BranchID & "' AND SYSCODE='SHP'"
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                    Return strCompID
                End If
            Catch ex As Exception
                Return String.Empty
            Finally
                EndConnection()
            End Try
        End Function

        Public Shared Function GenerateContractID(ByVal BranchID As String, ByVal CurDate As Date) As String
            Dim strCompID, strSQL As String
            Dim intRunNo As Integer
            Try
                If StartConnection() = True Then
                    TransFormat = GetSysCodeFormat(BranchID, "CRT")
                    strCompID = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                   TransFormat.Postfix, TransFormat.Runno, TransFormat.NoPos, TransFormat.Length, _
                                   CurDate, BranchID)
                    strSQL = "UPDATE SYSCODEB SET RunNo=RunNo+1 WHERE BranchID = '" & BranchID & "' AND SYSCODE='CRT'"
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                    Return strCompID
                End If
            Catch ex As Exception
                Return String.Empty
            Finally
                EndConnection()
            End Try
        End Function

        Public Function GeneratePointCode(ByVal Branch As String, ByVal CurDate As Date) As String
            Dim strPointCode, strSQL As String
            Dim intRunNo As Integer
            'Dim rdr As SqlClient.SqlDataReader
            'With htbKeys
            '    .Clear()
            'End With

            Try
                If StartConnection() = True Then
                    TransFormat = GetSysCodeFormat(Branch, "PC")

                    strPointCode = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                    TransFormat.Postfix, TransFormat.Runno, TransFormat.NoPos, TransFormat.Length, _
                                    CurDate, Branch)
                    'Update Run No.
                    strSQL = "UPDATE SYSCODEB SET RunNo=RunNo+1 WHERE BranchID = '" & Branch & "' AND SYSCODE='PC'"
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

                    Return strPointCode
                End If
            Catch errGen As Exception
                Return String.Empty
            Finally
                EndConnection()
            End Try
        End Function
        Public Shared Function GenerateLPointCode(ByVal Branch As String, ByVal CurDate As Date) As String
            Dim strPointCode, strSQL As String
            Dim intRunNo As Integer
            'Dim rdr As SqlClient.SqlDataReader
            'With htbKeys
            '    .Clear()
            'End With

            Try
                If StartConnection() = True Then
                    TransFormat = GetSysCodeFormat(Branch, "PT")

                    strPointCode = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                    TransFormat.Postfix, TransFormat.Runno, TransFormat.NoPos, TransFormat.Length, _
                                    CurDate, Branch)
                    'Update Run No.
                    strSQL = "UPDATE SYSCODEB SET RunNo=RunNo+1 WHERE BranchID = '" & Branch & "' AND SYSCODE='PT'"
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

                    Return strPointCode
                End If
            Catch errGen As Exception
                Return String.Empty
            Finally
                EndConnection()
            End Try
        End Function

        Public Shared Function GenerateDocCode(ByVal Branch As String, ByVal CurDate As Date) As String
            Dim strDocCode, strSQL As String
            Dim intRunNo As Integer
            'Dim rdr As SqlClient.SqlDataReader
            'With htbKeys
            '    .Clear()
            'End With
            'Dim strPrefix As String = "ADJ"
            Dim strPrefix As String = "TRS"
            Try
                If StartConnection() = True Then
                    TransFormat = GetSysCodeFormat(Branch, strPrefix)

                    strDocCode = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                    TransFormat.Postfix, TransFormat.Runno, TransFormat.NoPos, TransFormat.Length, _
                                    CurDate, Branch)
                    'Update Run No.
                    strSQL = "UPDATE SYSCODEB SET RunNo=RunNo+1 WHERE BranchID = '" & Branch & "' AND SYSCODE='" & strPrefix & "'"
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

                    Return strDocCode
                End If
            Catch errGen As Exception
                Return String.Empty
            Finally
                EndConnection()
            End Try
        End Function

        Public Shared Function GenerateAdjustmentCode(ByVal Branch As String, ByVal CurDate As Date) As String
            Dim strDocCode, strSQL As String
            Dim intRunNo As Integer
            'Dim rdr As SqlClient.SqlDataReader
            'With htbKeys
            '    .Clear()
            'End With

            Try
                If StartConnection() = True Then
                    TransFormat = GetSysCodeFormat(Branch, "ADJ")

                    strDocCode = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                    TransFormat.Postfix, TransFormat.Runno, TransFormat.NoPos, TransFormat.Length, _
                                    CurDate, Branch)
                    'Update Run No.
                    strSQL = "UPDATE SYSCODEB SET RunNo=RunNo+1 WHERE BranchID = '" & Branch & "' AND SYSCODE='ADJ'"
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

                    Return strDocCode
                End If
            Catch errGen As Exception
                Return String.Empty
            Finally
                EndConnection()
            End Try
        End Function

        Public Shared Function GenerateProcCode(ByVal Branch As String, ByVal CurDate As Date) As String
            Dim strDocCode, strSQL As String
            Dim intRunNo As Integer
            Dim strPrefix As String = "PRC"
            Try
                If StartConnection() = True Then
                    TransFormat = GetSysCodeFormat(Branch, strPrefix)

                    strDocCode = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                    TransFormat.Postfix, TransFormat.Runno, TransFormat.NoPos, TransFormat.Length, _
                                    CurDate, Branch)
                    'Update Run No.
                    strSQL = "UPDATE SYSCODEB SET RunNo=RunNo+1 WHERE BranchID = '" & Branch & "' AND SYSCODE='" & strPrefix & "'"
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

                    Return strDocCode
                End If
            Catch errGen As Exception
                Return String.Empty
            Finally
                EndConnection()
            End Try
        End Function

        Public Shared Function GeneratePurchaseDocCode(ByVal Branch As String, ByVal PurchaseType As Integer, ByVal CurDate As Date) As String
            Dim strDocCode, strSQL As String
            Dim intRunNo As Integer
            'Dim rdr As SqlClient.SqlDataReader
            Dim tmpType As String
            'With htbKeys
            '    .Clear()
            'End With

            Try
                If StartConnection() = True Then
                    If PurchaseType = 0 Then    'Purchase Request
                        TransFormat = GetSysCodeFormat(Branch, "PR")
                        tmpType = "PR"
                    ElseIf PurchaseType = 1 Then    'Purchase Order
                        TransFormat = GetSysCodeFormat(Branch, "PO")
                        tmpType = "PO"
                    ElseIf PurchaseType = 2 Then    'Delivery Order
                        TransFormat = GetSysCodeFormat(Branch, "DO")
                        tmpType = "DO"
                    End If

                    strDocCode = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                    TransFormat.Postfix, TransFormat.Runno, TransFormat.NoPos, TransFormat.Length, _
                                    CurDate, Branch)
                    'Update Run No.
                    strSQL = "UPDATE SYSCODEB SET RunNo=RunNo+1 WHERE BranchID = '" & Branch & "' AND SYSCODE = '" & tmpType & "'"
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

                    Return strDocCode
                End If
            Catch errGen As Exception
                Return String.Empty
            Finally
                EndConnection()
            End Try
        End Function

        Public Shared Function GenerateAttentionID(ByVal Branch As String, ByVal CurDate As Date) As String
            Dim strDocCode, strSQL As String
            Dim intRunNo As Integer
            'Dim rdr As SqlClient.SqlDataReader
            'With htbKeys
            '    .Clear()
            'End With

            Try
                If StartConnection() = True Then
                    TransFormat = GetSysCodeFormat(Branch, "ATN")

                    strDocCode = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                    TransFormat.Postfix, TransFormat.Runno, TransFormat.NoPos, TransFormat.Length, _
                                    CurDate, Branch)
                    'Update Run No.
                    strSQL = "UPDATE SYSCODEB SET RunNo=RunNo+1 WHERE BranchID = '" & Branch & "' AND SYSCODE='ATN'"
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

                    Return strDocCode
                End If
            Catch errGen As Exception
                Return String.Empty
            Finally
                EndConnection()
            End Try
        End Function

        'Public Shared Function GenerateDeliveryNo(ByVal Branch As String) As String
        '    Dim strDeliveryNo, strSQL As String
        '    Dim intRunNo As Integer
        '    Dim rdr As SqlClient.SqlDataReader
        '    'With htbKeys
        '    '    .Clear()
        '    'End With

        '    Try
        '        If StartConnection() = True Then
        '            TransFormat = GetSysCodeFormat(Branch, "DO")

        '            strDeliveryNo = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
        '                            TransFormat.Postfix, TransFormat.Runno, TransFormat.NoPos, TransFormat.Length, _
        '                            Branch)
        '            'Update Run No.
        '            strSQL = "UPDATE SYSCODEB SET RunNo=RunNo+1 WHERE BranchID = '" & Branch & "' AND SYSCODE='DO'"
        '            objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

        '            Return strDeliveryNo
        '        End If
        '    Catch errGen As Exception
        '        Return String.Empty
        '    Finally
        '        EndConnection()
        '    End Try
        'End Function

        Public Shared Function GenerateSalesAdjCode(ByVal Branch As String, ByVal CurDate As Date) As String
            Dim strSalesAdjust, strSQL As String
            Dim intRunNo As Integer
            'Dim rdr As SqlClient.SqlDataReader

            Try
                If StartConnection() = True Then
                    TransFormat = GetSysCodeFormat(Branch, "SAJ")

                    strSalesAdjust = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                    TransFormat.Postfix, TransFormat.Runno, TransFormat.NoPos, TransFormat.Length, _
                                    CurDate, Branch)
                    'Update Run No.
                    strSQL = "UPDATE SYSCODEB SET RunNo=RunNo+1 WHERE BranchID = '" & Branch & "' AND  SYSCODE='SAJ'"
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

                    Return strSalesAdjust
                End If
            Catch errGen As Exception
                Return String.Empty
            Finally
                EndConnection()
            End Try
        End Function

        Public Shared Function GenerateCustAnalysisCode(ByVal Branch As String, ByVal CurDate As Date) As String
            Dim strEmpID, strSQL As String
            Dim intRunNo As Integer
            'Dim rdr As SqlClient.SqlDataReader
            'With htbKeys
            '    .Clear()
            'End With

            Try
                If StartConnection() = True Then
                    TransFormat = GetSysCodeFormat(Branch, "CA")

                    strEmpID = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                    TransFormat.Postfix, TransFormat.Runno, TransFormat.NoPos, TransFormat.Length, _
                                    CurDate, Branch)
                    'Update Run No.
                    strSQL = "UPDATE SYSCODEB SET RunNo=RunNo+1 WHERE BranchID = '" & Branch & "' AND SYSCODE='CA'"
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

                    Return strEmpID
                End If
            Catch errGen As Exception
                Return String.Empty
            Finally
                EndConnection()
            End Try
        End Function

        Public Shared Function GenerateCustRequisitCode(ByVal Branch As String, ByVal CurDate As Date, Optional ByVal TermID As Integer = -1) As String
            Dim strEmpID, strSQL As String
            Dim intRunNo As Integer
            'Dim rdr As SqlClient.SqlDataReader
            'With htbKeys
            '    .Clear()
            'End With

            Try
                If StartConnection() = True Then
                    TransFormat = GetSysCodeFormat(Branch, "CRS")

                    strEmpID = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                    TransFormat.Postfix, TransFormat.Runno, TransFormat.NoPos, TransFormat.Length, _
                                    CurDate, Branch, TermID)
                    'Update Run No.
                    strSQL = "UPDATE SYSCODEB SET RunNo=RunNo+1 WHERE BranchID = '" & Branch & "' AND SYSCODE='CRS'"
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

                    Return strEmpID
                End If
            Catch errGen As Exception
                Return String.Empty
            Finally
                EndConnection()
            End Try
        End Function

        Public Function GenerateAccountCode(ByVal Branch As String, ByVal CurDate As Date, Optional ByVal TermID As Integer = -1) As String
            Dim strEmpID, strSQL As String
            Dim intRunNo As Integer
            'Dim rdr As SqlClient.SqlDataReader
            'With htbKeys
            '    .Clear()
            'End With

            Try
                If StartConnection() = True Then
                    TransFormat = GetSysCodeFormat(Branch, "CAT")

                    strEmpID = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                    TransFormat.Postfix, TransFormat.Runno, TransFormat.NoPos, TransFormat.Length, _
                                    CurDate, Branch, TermID)
                    'Update Run No.
                    strSQL = "UPDATE SYSCODEB SET RunNo=RunNo+1 WHERE BranchID = '" & Branch & "' AND SYSCODE='CAT'"
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

                    Return strEmpID
                End If
            Catch errGen As Exception
                Return String.Empty
            Finally
                EndConnection()
            End Try
        End Function

        Public Shared Function GeneratePromotionCode(ByVal Branch As String, ByVal CurDate As Date, Optional ByVal TermID As Integer = -1) As String
            Dim strPromoCode, strSQL As String
            Dim intRunNo As Integer

            Try
                If StartConnection() = True Then
                    TransFormat = GetSysCodeFormat(Branch, "PRC")

                    strPromoCode = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                    TransFormat.Postfix, TransFormat.Runno, TransFormat.NoPos, TransFormat.Length, _
                                    CurDate, Branch, TermID)
                    'Update Run No.
                    strSQL = "UPDATE SYSCODEB SET RunNo=RunNo+1 WHERE BranchID = '" & Branch & "' AND SYSCODE='PRC'"
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

                    Return strPromoCode
                End If
            Catch errGen As Exception
                Return String.Empty
            Finally
                EndConnection()
            End Try
        End Function

        Public Shared Function GenerateEmpID(ByVal Branch As String, ByVal CurDate As Date) As String
            Dim strEmpID, strSQL As String
            Dim intRunNo As Integer
            'Dim rdr As SqlClient.SqlDataReader
            'With htbKeys
            '    .Clear()
            'End With

            Try
                If StartConnection() = True Then
                    TransFormat = GetSysCodeFormat(Branch, "EMP")

                    strEmpID = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                    TransFormat.Postfix, TransFormat.Runno, TransFormat.NoPos, TransFormat.Length, _
                                    CurDate, Branch)
                    'strEmpID = CheckySysKey(strEmpID)
                    'Update Run No.
                    strSQL = "UPDATE SYSCODEB SET RunNo=RunNo+1 WHERE BranchID = '" & Branch & "' AND SYSCODE='EMP'"
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

                    Return strEmpID
                End If
            Catch errGen As Exception
                Return String.Empty
            Finally
                EndConnection()
            End Try
        End Function

        Protected Shared Function GenerateTermCode(ByVal Type As Short, _
        ByVal Branch As String, ByVal TermID As Integer, ByVal CurDate As Date, _
        ByRef CurNo As Integer, Optional ByVal isBackDate As Boolean = False) As String
            Dim strSQL, TransNo, strType, strCol As String
            Dim rdr As SqlClient.SqlDataReader
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    Branch = objSQL.ParseValue(SQLControl.EnumDataType.dtString, Branch)
                    With htbKeys
                        .Clear()
                        .Add("#B", Branch)
                        .Add("#T", TermID)
                    End With

                    Select Case Type
                        Case 0 'Receipt
                            strType = "RC"
                        Case 1 'InService
                            strType = "IS"
                        Case 2 'Appointment
                            strType = "AP"
                            'Case 3
                            '    strType = "ZR"
                        Case 4 'Customer Package ID
                            strType = "PKG"

                        Case 6 'Credit Note No  
                            strType = "CN"

                        Case 7  'Official Receipt
                            strType = "OR"

                            'Case 8  'Attention Notes
                            '    strType = "ATN"


                    End Select
                    strSQL = "SELECT Prefix, SpCode, NoLength, NoPos, " & _
                     "Postfix FROM SYSCODEB WHERE Status=1 AND " & _
                     "BranchID='" & Branch & "' AND SysCode='" & strType & "'"
                    rdr = CType(objDCom.Execute(strSQL, CommandType.Text, CommandBehavior.SingleRow, False), SqlClient.SqlDataReader)

                    If rdr Is Nothing = False Then
                        With rdr
                            If .Read Then
                                TransFormat.Prefix = Convert.ToString(.Item("Prefix"))
                                TransFormat.SpCode = Convert.ToString(.Item("SpCode"))
                                TransFormat.Postfix = Convert.ToString(.Item("Postfix"))
                                TransFormat.NoPos = Convert.ToInt16(.Item("NoPos"))
                                TransFormat.Length = Convert.ToInt32(.Item("NoLength"))
                            End If
                            .Close()
                        End With
                    End If
                    If isBackDate = False Then
                        Select Case Type
                            Case 0 'Receipt
                                strCol = "CurTransRun"
                            Case 1 'InService
                                strCol = "CurInServRun"
                            Case 2 'Appointment
                                strCol = "CurApptRun"
                            Case 3 'ZRead 
                                strCol = "CurZReadRun"
                            Case 4 'Customer Package ID"
                                strCol = "CurCustPkgID"
                            Case 5 'XRead
                                strCol = "CurXReadRun"
                            Case 6 'Credit Note 
                                strCol = "CurCreditNote"
                            Case 7  'Official Receipt
                                strCol = "CurORNo"
                                'Case 8  'Attention Notes
                                '    strCol = "CurAttentionRun"
                        End Select

                        strSQL = "SELECT " & strCol & " FROM SYSTERM " & _
                        "WHERE BranchID='" & Branch & "' AND TermID=" & TermID
                        rdr = CType(objDCom.Execute(strSQL, CommandType.Text, CommandBehavior.SingleRow, False), SqlClient.SqlDataReader)

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    CurNo = Convert.ToInt32(.Item(strCol))
                                    TransNo = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                            TransFormat.Postfix, CurNo, TransFormat.NoPos, TransFormat.Length, _
                                            CurDate, Branch, TermID)
                                End If
                                .Close()
                                UpdateTermCode(Type, Branch, TermID, CurNo)
                            End With
                        End If
                        TransNo = CheckySysKey(TransNo)
                        If Type = 3 Or Type = 5 Then
                            TransNo = CurNo.ToString
                        End If
                        Return TransNo

                    ElseIf isBackDate = True Then
                        Dim rdrBackDate As SqlClient.SqlDataReader
                        Dim intNoLen As Integer
                        Dim CurrentDate As DateTime
                        intNoLen = TransFormat.Length
                        'CurrentDate = Convert.ToDateTime(objSQL.ParseValue(SQLControl.EnumDataType.dtDateOnly, CurDate))
                        CurrentDate = CurDate
                        strSQL = "SELECT TOP 1 TransNo FROM TRANSHDR WHERE BranchID = '" & Branch & "' " & _
"AND TermID = " & TermID & " " & _
"AND Year(TransDate) = " & Year(CurrentDate) & " AND Month(TransDate) = " & Month(CurrentDate) & " " & _
"AND ( TransRemark Like 'BACKDATEREFUND%')" & _
"ORDER BY TransNo"

                        rdrBackDate = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        If rdrBackDate Is Nothing = True Then
                            strSQL = "SELECT TOP 1 TransNo FROM TRANSHDR WHERE BranchID = '" & Branch & "' " & _
    "AND TermID = " & TermID & " " & _
    "AND TransDate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateOnly, CurDate) & " " & _
    "AND (TransRemark = 'BACKDATESALES' OR TransRemark Like 'BACKDATEREFUND%')" & _
    "ORDER BY TransNo"

                            rdrBackDate = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
                        End If
                        If rdrBackDate Is Nothing = False Then
                            With rdrBackDate
                                If .HasRows = True Then
                                    If .Read Then
                                        CurNo = Convert.ToInt32(Right(Convert.ToString(.Item(0)), intNoLen)) - 1
                                    End If
                                Else
                                    Dim i As Integer
                                    Dim strNo As String
                                    For i = 0 To intNoLen - 1
                                        strNo = strNo + "9"
                                    Next
                                    CurNo = Convert.ToInt32(strNo)
                                End If
                                .Close()
                            End With
                            TransNo = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                            TransFormat.Postfix, CurNo, TransFormat.NoPos, TransFormat.Length, _
                                            CurDate, Branch, TermID)
                            TransNo = CheckySysKey(TransNo)
                            Return TransNo
                        End If
                    End If
                End If
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Protected Shared Function UpdateTermCode(ByVal Type As Int16, _
        ByVal Branch As String, ByVal TermID As Integer, ByVal CurNo As Integer) As Boolean
            Dim strSQL, strCol As String
            Try
                If StartConnection(Core.CoreBase.EnumIsoState.StateUpdatetable) = True Then
                    StartSQLControl()
                    Select Case Type
                        Case 0 'Receipt
                            strCol = "CurTransRun"
                        Case 1 'InService
                            strCol = "CurInServRun"
                        Case 2 'Appointment
                            strCol = "CurApptRun"
                        Case 3 'ZRead
                            strCol = "CurZReadRun"
                        Case 4 'Customer Package ID
                            strCol = "CurCustPkgID"
                        Case 5  'XRead
                            strCol = "CurXReadRun"
                        Case 6 'Credit Note
                            strCol = "CurCreditNote"
                        Case 7  'Official Receipt
                            strCol = "CurORNo"
                        Case 8  'Attention Notes
                            strCol = "CurAttentionRun"
                    End Select
                    Branch = objSQL.ParseValue(SQLControl.EnumDataType.dtString, Branch)
                    strSQL = "UPDATE SYSTERM SET " & strCol & "=" & strCol & " + 1 " & _
                            "WHERE BranchID = '" & Branch & "' AND TermID = " & TermID
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                    Return True
                End If
            Catch ex As Exception
                Return False
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Protected Shared Function GenerateNoteCode(ByVal NoteType As String, _
        ByVal Branch As String, ByVal CurDate As Date, ByRef CurNo As Integer) As String
            Dim strSQL, TransNo, strCol As String
            Dim rdr As SqlClient.SqlDataReader
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    Branch = objSQL.ParseValue(SQLControl.EnumDataType.dtString, Branch)
                    With htbKeys
                        .Clear()
                        .Add("#B", Branch)
                        '.Add("#T", TermID)
                    End With

                    strSQL = "SELECT Prefix, SpCode, NoLength, NoPos, " & _
                            "Postfix FROM SYSCODEB WHERE Status = 1 " & _
                            "AND BranchID='" & Branch & "' " & _
                            "AND SysCode = '" & NoteType & "' "

                    rdr = CType(objDCom.Execute(strSQL, CommandType.Text, CommandBehavior.SingleRow, False), SqlClient.SqlDataReader)

                    If rdr Is Nothing = False Then
                        With rdr
                            If .Read Then
                                TransFormat.Prefix = Convert.ToString(.Item("Prefix"))
                                TransFormat.SpCode = Convert.ToString(.Item("SpCode"))
                                TransFormat.Postfix = Convert.ToString(.Item("Postfix"))
                                TransFormat.NoPos = Convert.ToInt16(.Item("NoPos"))
                                TransFormat.Length = Convert.ToInt32(.Item("NoLength"))
                            End If
                            .Close()
                        End With
                    End If


                    Select Case NoteType
                        Case "INV"
                            strCol = "CurINV"
                        Case "PUR"
                            strCol = "CurPUR"
                        Case "CRN"
                            strCol = "CurCRN"
                        Case "DBN"
                            strCol = "CurDBN"
                        Case "PRN"
                            strCol = "CurPRN"
                        Case "PON"
                            strCol = "CurPON"
                        Case "DON"
                            strCol = "CurDON"
                    End Select

                    strSQL = "SELECT " & strCol & " FROM SYSTERM " & _
                    "WHERE BranchID='" & Branch & "' "
                    rdr = CType(objDCom.Execute(strSQL, CommandType.Text, CommandBehavior.SingleRow, False), SqlClient.SqlDataReader)

                    If rdr Is Nothing = False Then
                        With rdr
                            If .Read Then
                                CurNo = Convert.ToInt32(.Item(strCol))
                                TransNo = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                        TransFormat.Postfix, CurNo, TransFormat.NoPos, TransFormat.Length, _
                                        CurDate, Branch)
                            End If
                            .Close()
                            UpdateNoteCode(NoteType, Branch, CurNo)
                        End With
                    End If
                    TransNo = CheckySysKey(TransNo)
                    Return TransNo

                End If
            Catch ex As Exception
                Return Nothing
            Finally
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Protected Shared Function UpdateNoteCode(ByVal NoteType As String, ByVal Branch As String, ByVal CurNo As Integer) As Boolean
            Dim strSQL, strCol As String
            Try
                If StartConnection(Core.CoreBase.EnumIsoState.StateUpdatetable) = True Then
                    StartSQLControl()
                    Select Case NoteType
                        Case "INV"
                            strCol = "CurINV"
                        Case "PUR"
                            strCol = "CurPUR"
                        Case "CRN"
                            strCol = "CurCRN"
                        Case "DBN"
                            strCol = "CurDBN"
                        Case "PRN"
                            strCol = "CurPRN"
                        Case "PON"
                            strCol = "CurPON"
                        Case "DON"
                            strCol = "CurDON"
                    End Select
                    Branch = objSQL.ParseValue(SQLControl.EnumDataType.dtString, Branch)
                    CurNo = CurNo + 1
                    strSQL = "UPDATE SYSTERM SET " & strCol & "=" & CurNo & _
                            " WHERE BranchID='" & Branch & "' "
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                    Return True
                End If
            Catch ex As Exception
                Return False
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Shared Function GenerateZReadCode(ByVal Branch As String, ByVal TermID As Integer, ByVal CurDate As Date, ByRef CurZReadNo As Integer) As String
            Return (GenerateTermCode(3, Branch, TermID, CurDate, CurZReadNo))
        End Function

        Public Shared Function UpdateZReadCode(ByVal Branch As String, ByVal TermID As Integer, ByVal CurZReadNo As Integer) As Boolean
            Return UpdateTermCode(3, Branch, TermID, CurZReadNo)
        End Function

        Public Shared Function GenerateXReadCode(ByVal Branch As String, ByVal TermID As Integer, ByVal CurDate As Date, ByRef CurZReadNo As Integer) As String
            Return (GenerateTermCode(5, Branch, TermID, CurDate, CurZReadNo))
        End Function

        Public Shared Function UpdateXReadCode(ByVal Branch As String, ByVal TermID As Integer, ByVal CurZReadNo As Integer) As Boolean
            Return UpdateTermCode(5, Branch, TermID, CurZReadNo)
        End Function

        Public Shared Function GenerateTransCode(ByVal Branch As String, ByVal TermID As Integer, ByVal CurDate As Date, ByRef CurTransNo As Integer, Optional ByVal isBackDate As Boolean = False) As String
            Return (GenerateTermCode(0, Branch, TermID, CurDate, CurTransNo, isBackDate))
        End Function

        Public Shared Function GenerateORNo(ByVal Branch As String, ByVal TermID As Integer, ByVal CurDate As Date) As String
            Return (GenerateTermCode(7, Branch, TermID, CurDate, 0))
        End Function

        Public Shared Function UpdateTransCode(ByVal Branch As String, ByVal TermID As Integer, ByVal CurTransNo As Integer) As Boolean
            Return UpdateTermCode(0, Branch, TermID, CurTransNo)
        End Function

        Public Shared Function GenerateCustPackageID(ByVal Branch As String, ByVal TermID As Integer, ByVal CurDate As Date, ByRef CurTransNo As Integer) As String
            Return (GenerateTermCode(4, Branch, TermID, CurDate, CurTransNo))
        End Function

        Public Shared Function UpdateCustPackageID(ByVal Branch As String, ByVal TermID As Integer, ByVal CurTransNo As Integer) As Boolean
            Return UpdateTermCode(4, Branch, TermID, CurTransNo)
        End Function

        'Public Shared Function GenerateAttentionID(ByVal Branch As String, ByVal TermID As Integer, ByVal CurDate As Date, ByRef CurAttentionID As Integer) As String
        '    Return (GenerateTermCode(8, Branch, TermID, CurDate, CurAttentionID))
        'End Function

        Public Shared Function GenerateCreditNoteNo(ByVal Branch As String, ByVal TermID As Integer, ByVal CurDate As Date, ByRef CurCreditNoteNo As Integer) As String
            Return (GenerateTermCode(6, Branch, TermID, CurDate, CurCreditNoteNo))
        End Function

        Public Shared Function UpdateCreditNoteNo(ByVal Branch As String, ByVal TermID As Integer, ByVal CurCreditNoteNo As Integer) As Boolean
            Return UpdateTermCode(6, Branch, TermID, CurCreditNoteNo)
        End Function

        Public Shared Function GenerateNoteNo(ByVal Branch As String, ByVal CurDate As Date, ByRef CurNoteNo As Integer, ByVal NoteType As String) As String
            Return GenerateNoteCode(NoteType, Branch, CurDate, CurNoteNo)
        End Function

        Public Shared Function GenerateServiceCode(ByVal Branch As String, ByVal TermID As Integer, ByVal CurDate As Date, ByRef CurInServNo As Integer) As String
            Return (GenerateTermCode(1, Branch, TermID, CurDate, CurInServNo))
        End Function

        Public Shared Function UpdateServiceCode(ByVal Branch As String, ByVal TermID As Integer, ByVal CurInServNo As Integer) As Boolean
            Return UpdateTermCode(1, Branch, TermID, CurInServNo)
        End Function

        Public Shared Function GenerateApptCode(ByVal Branch As String, ByVal TermID As Integer, ByVal CurDate As Date, ByRef CurInServNo As Integer) As String
            Return (GenerateTermCode(2, Branch, TermID, CurDate, CurInServNo))
        End Function

        Public Shared Function UpdateApptCode(ByVal Branch As String, ByVal TermID As Integer, ByVal CurInServNo As Integer) As Boolean
            Return UpdateTermCode(2, Branch, TermID, CurInServNo)
        End Function

        Protected Shared Function GetSysCodeFormat(ByVal Branch As String, ByVal TypeCode As String) As StrucTrans
            Dim rdr As SqlClient.SqlDataReader
            Dim CurFormat As StrucTrans
            Try
                If StartConnection() = True Then
                    strSQL = "SELECT Prefix, SpCode, RunNo, NoLength, NoPos, " & _
                     "Postfix FROM SYSCODEB WHERE Status=1 AND " & _
                     "BranchID='" & Branch & "' AND SysCode='" & TypeCode & "'"
                    rdr = CType(objDCom.Execute(strSQL, CommandType.Text, CommandBehavior.SingleRow, False), SqlClient.SqlDataReader)

                    If rdr Is Nothing = False Then
                        With rdr
                            If .Read Then
                                CurFormat.Prefix = Convert.ToString(.Item("Prefix"))
                                CurFormat.SpCode = Convert.ToString(.Item("SpCode"))
                                CurFormat.Runno = Convert.ToInt32(.Item("RunNo"))
                                CurFormat.Postfix = Convert.ToString(.Item("Postfix"))
                                CurFormat.NoPos = Convert.ToInt16(.Item("NoPos"))
                                CurFormat.Length = Convert.ToInt32(.Item("NoLength"))
                            End If
                            .Close()
                        End With
                    End If
                    Return CurFormat
                End If
            Catch ex As Exception
            Finally
                rdr = Nothing
            End Try
            EndConnection()
        End Function

        Protected Shared Function GenerateFormattedNo(ByVal Prefix As String, ByVal SpCode As String, _
        ByVal Postfix As String, ByVal Runno As Integer, ByVal NoPOS As Int16, _
        ByVal NoLength As Integer, ByVal CurDate As Date, Optional ByVal Branch As String = Nothing, _
        Optional ByVal TermID As Integer = -1) As String
            Dim strFront, strBack, strMid As String
            Dim strCode As String
            Const KeyBranch As String = "#B"
            Const KeyTerm As String = "#T"
            Const KeyYR As String = "#YR"
            Const KeyMT As String = "#MT"
            Const KeyDY As String = "#DY"
            Try
                strFront = Prefix.Trim
                If strFront.Length > 0 Then
                    If IsNothing(Branch) = False Then
                        If TermID <> -1 Then
                            strFront = CheckySysKey(strFront, KeyTerm, TermID.ToString)
                        Else
                            strFront = CheckySysKey(strFront, KeyTerm, String.Empty)
                        End If
                        strFront = CheckySysKey(strFront, KeyBranch, Branch)
                        strFront = CheckySysKey(strFront, KeyYR, CurDate.Year.ToString.Substring(2))
                        strFront = CheckySysKey(strFront, KeyMT, Format(Convert.ToInt64(CurDate.Month), "00"))
                        strFront = CheckySysKey(strFront, KeyDY, Format(Convert.ToInt64(CurDate.Day), "00"))
                    End If
                End If

                strBack = Postfix.Trim
                If strBack.Length > 0 Then
                    If IsNothing(Branch) = False Then
                        If TermID <> -1 Then
                            strBack = CheckySysKey(strBack, KeyTerm, TermID.ToString)
                        Else
                            strBack = CheckySysKey(strBack, KeyTerm, String.Empty)
                        End If
                        strBack = CheckySysKey(strBack, KeyBranch, Branch)
                        strBack = CheckySysKey(strBack, KeyYR, CurDate.Year.ToString.Substring(2))
                        strBack = CheckySysKey(strBack, KeyMT, Format(Convert.ToInt64(CurDate.Month), "00"))
                        strBack = CheckySysKey(strBack, KeyDY, Format(Convert.ToInt64(CurDate.Day), "00"))
                    End If
                End If

                SpCode = SpCode.Trim
                If SpCode.Length > 0 Then
                    If TermID <> -1 Then
                        SpCode = CheckySysKey(SpCode, KeyTerm, TermID.ToString)
                    Else
                        SpCode = CheckySysKey(SpCode, KeyTerm, String.Empty)
                    End If
                    SpCode = CheckySysKey(SpCode, KeyBranch, Branch)
                    SpCode = CheckySysKey(SpCode, KeyYR, CurDate.Year.ToString.Substring(2))
                    SpCode = CheckySysKey(SpCode, KeyMT, Format(Convert.ToInt64(CurDate.Month), "00"))
                    SpCode = CheckySysKey(SpCode, KeyDY, Format(Convert.ToInt64(CurDate.Day), "00"))
                End If

                If NoLength >= Runno.ToString.Length Then
                    strCode = Runno.ToString
                    strCode = strCode.PadLeft(NoLength, Convert.ToChar("0"))
                Else
                    Return String.Empty
                End If

                If NoPOS = 1 Then
                    strMid = SpCode.Concat(strCode, SpCode)
                Else
                    strMid = SpCode.Concat(SpCode, strCode)
                End If
                Return String.Concat(strFront, strMid, strBack)
            Catch errGen As Exception
                Return String.Empty
            End Try
        End Function

        Public Shared Function GenerateStkTakeCode(ByVal Branch As String, ByVal CurDate As Date) As String
            Dim strDocCode, strSQL As String
            Dim intRunNo As Integer
            'Dim rdr As SqlClient.SqlDataReader
            'With htbKeys
            '    .Clear()
            'End With

            Try
                StartConnection()
                TransFormat = GetSysCodeFormat(Branch, "STKTK")

                strDocCode = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                   TransFormat.Postfix, TransFormat.Runno, TransFormat.NoPos, TransFormat.Length, CurDate, Branch)

                'Update Run No.
                strSQL = "UPDATE SYSCODEB SET RunNo=RunNo+1 WHERE BranchID = '" & Branch & "' AND SYSCODE='STKTK'"
                objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                Return strDocCode

            Catch errGen As Exception
                Return String.Empty
            Finally
                EndConnection()
            End Try
        End Function

        Public Shared Function GenerateExpensesNo(ByVal Branch As String, ByVal TermID As Integer, ByVal CurDate As Date) As String
            Dim strExpNo, strSQL As String
            Dim intRunNo As Integer
            'Dim rdr As SqlClient.SqlDataReader

            Try
                StartConnection()
                StartSQLControl()
                TransFormat = GetSysCodeFormat(Branch, "EXP")

                strExpNo = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                TransFormat.Postfix, TransFormat.Runno, TransFormat.NoPos, TransFormat.Length, _
                                CurDate, Branch)
                strExpNo = CheckySysKey(strExpNo)

                'Update Run No.
                strSQL = "UPDATE SYSCODEB SET RunNo=RunNo+1 WHERE BranchID = '" & Branch & "' AND SYSCODE='EXP'"
                objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

                Return strExpNo

            Catch errGen As Exception
                Return String.Empty
            Finally
                EndSQLControl()
                EndConnection()
            End Try

        End Function

        Public Shared Function GenerateCNORNo(ByVal Branch As String, ByVal TermID As Integer, ByVal CurDate As Date) As String
            Dim strExpNo, strSQL As String
            Dim intRunNo As Integer
            'Dim rdr As SqlClient.SqlDataReader

            Try
                StartConnection()
                StartSQLControl()
                TransFormat = GetSysCodeFormat(Branch, "CNOR")

                strExpNo = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                TransFormat.Postfix, TransFormat.Runno, TransFormat.NoPos, TransFormat.Length, _
                                CurDate, Branch)
                strExpNo = CheckySysKey(strExpNo)

                'Update Run No.
                strSQL = "UPDATE SYSCODEB SET RunNo=RunNo+1 WHERE BranchID = '" & Branch & "' AND SYSCODE='CNOR'"
                objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

                Return strExpNo

            Catch errGen As Exception
                Return String.Empty
            Finally
                EndSQLControl()
                EndConnection()
            End Try

        End Function

#Region "GenerateSurveyID"
        'created 3/06/2005 by darren

        Public Shared Function GenerateSurveyID(ByVal Branch As String, ByVal CurDate As Date) As String
            Dim strSurveyID, strSQL As String
            Dim intRunNo As Integer
            Dim rdr As SqlClient.SqlDataReader
            'With htbKeys
            '    .Clear()
            'End With

            Try
                'If StartConnection() = True Then
                StartConnection()
                StartSQLControl()

                TransFormat = GetSysCodeFormat(Branch, "SUR")

                strSurveyID = GenerateFormattedNo(TransFormat.Prefix, TransFormat.SpCode, _
                                TransFormat.Postfix, TransFormat.Runno, TransFormat.NoPos, TransFormat.Length, _
                                CurDate, Branch)
                'strEmpID = CheckySysKey(strEmpID)
                'Update Run No.
                strSQL = "UPDATE SYSCODEB SET RunNo=RunNo+1 WHERE BranchID = '" & Branch & "' AND SYSCODE='SUR'"
                objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

                Return strSurveyID
                'End If
            Catch errGen As Exception
                Return String.Empty
            Finally
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region
    End Class
#End Region

#Region "UOM Base"
    Public Class UOMBase
        Inherits Core.CoreBase

#Region "UOM class in UOM Base"
        Public Class UOM
            Protected strName, strDesc, strUpdBy As String
            Protected intActive, intGroup As Integer
            Protected dblMeasure As Double
            Protected dtLastUpd As DateTime
            Public MyScheme As New MyScheme

            Public Property Code() As String
                Get
                    Return strName
                End Get
                Set(ByVal UOMCode As String)
                    If UOMCode.Trim = String.Empty Then
                        Throw New Exception("Please key in UOM Code")
                    Else
                        strName = UOMCode
                    End If
                End Set
            End Property

            Public Property Description() As String
                Get
                    Return strDesc
                End Get
                Set(ByVal Value As String)
                    strDesc = Value
                End Set
            End Property

            Public Property Group() As Integer
                Get
                    Return intGroup
                End Get
                Set(ByVal Value As Integer)
                    If Value < 0 Then
                        intGroup = 0
                    Else
                        intGroup = Value
                    End If

                End Set
            End Property

            Public Property Measure() As Double
                Get
                    Return dblMeasure
                End Get
                Set(ByVal Value As Double)
                    dblMeasure = Value
                End Set
            End Property

            Public Property LastUpdate() As DateTime
                Get
                    Return dtLastUpd
                End Get
                Set(ByVal Value As DateTime)
                    dtLastUpd = Value
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return strUpdBy
                End Get
                Set(ByVal Value As String)
                    strUpdBy = Value
                End Set
            End Property

            Public Property Active() As Integer
                Get
                    Return intActive
                End Get
                Set(ByVal Value As Integer)
                    intActive = Value
                End Set
            End Property
        End Class
#End Region

#Region "UOM Scheme"
        Public Class MyScheme
            Inherits Core.SchemeBase

            Protected Overrides Sub InitializeInfo()
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "UOMCode"
                    .Length = 3
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(0, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "UOMDesc"
                    .Length = 50
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(1, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "Group"
                    .Length = 1
                    .RegExp = String.Empty
                    .DecPlace = Nothing
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(2, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "Measure"
                    .Length = 10
                    .DecPlace = 10
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(3, this)

            End Sub

            Public ReadOnly Property Code() As StrucElement
                Get
                    Return MyBase.GetItem(0)
                End Get
            End Property

            Public ReadOnly Property UOMDesc() As StrucElement
                Get
                    Return MyBase.GetItem(1)
                End Get
            End Property

            Public ReadOnly Property Group() As StrucElement
                Get
                    Return MyBase.GetItem(2)
                End Get
            End Property

            Public ReadOnly Property Measure() As StrucElement
                Get
                    Return MyBase.GetItem(3)
                End Get
            End Property

            Public Function GetElement(ByVal Key As Integer) As StrucElement
                Return MyBase.GetItem(Key)
            End Function
        End Class
#End Region

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = " UOMCode, UOMDesc, UOMGroup, Measure, LastUpdate, UpdateBy, Active, Inuse, Flag"
                .CheckFields = "INUSE, FLAG"
                .TableName = "UOM"
                .DefaultCond = "FLAG = 1"
                .DefaultOrder = String.Empty
                .Listing = " UOMCode, UOMDesc, UOMGroup, Measure, LastUpdate, UpdateBy, Active, Inuse, Flag"
                .ListingCond = "FLAG = 1"
                .ShortList = "UOMCODE, UOMDESC"
                .ShortListCond = String.Empty
            End With
            ReDim MyFields(1)
            MyBase.AddMyField(0, "UOM Code", "UOMCode", TypeCode.String)
            MyBase.AddMyField(1, "UOM Desc", "UOMDesc", TypeCode.String)
        End Sub

#Region "Data Manipulation-Add,Edit,Del"

        Private Function AssignItem(ByVal objUOM As UOM, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As SqlClient.SqlDataReader
            AssignItem = False
            Try
                If objUOM Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False

                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "UOMCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objUOM.Code) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        blnExec = True

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("Flag")) = 0 Then
                                        'Found but deleted
                                        blnFlag = False
                                    Else
                                        'Found and active
                                        blnFlag = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If
                    End If

                    If blnExec Then
                        If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                            'Item found & active
                            'system.Windows.Forms.MessageBox.Show("ab", "ABC",Windows.Forms.MessageBoxButtons.YesNo, Windows.Forms.MessageBoxIcon.Information,Windows.Forms.MessageBoxDefaultButton.Button2, Windows.Forms.MessageBoxOptions.DefaultDesktopOnly
                            'MsgBox("Record already exist", MsgBoxStyle.Critical, "Record Exist")
                            Return False
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = "UOM"
                                .AddField("UOMDesc", objUOM.Description, SQLControl.EnumDataType.dtStringN)
                                .AddField("UOMGroup", objUOM.Group, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Measure", objUOM.Measure, SQLControl.EnumDataType.dtNumeric)
                                .AddField("UpdateBy", objUOM.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", objUOM.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                .AddField("Active", objUOM.Active.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", cFlagActive.ToString, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                            "WHERE UOMCode='" & .ParseValue(SQLControl.EnumDataType.dtString, objUOM.Code) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("UOMCode", objUOM.Code, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                        "WHERE UOMCode='" & .ParseValue(SQLControl.EnumDataType.dtString, objUOM.Code) & "'")
                                End Select
                            End With
                            Try
                                'execute
                                objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002")
                                Else
                                    Throw New ApplicationException("210004")
                                End If
                            Finally
                                objSQL.Dispose()
                            End Try
                            Return True
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objUOM = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
        'ADD
        Function Add(ByVal objUOM As UOM) As Boolean
            Return AssignItem(objUOM, SQLControl.EnumSQLType.stInsert)
        End Function
        'AMEND
        Function Amend(ByVal objUOM As UOM) As Boolean
            Return AssignItem(objUOM, SQLControl.EnumSQLType.stUpdate)
        End Function
        'DELETE
        Function Delete(ByVal objUOM As UOM) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If objUOM Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "UOMCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objUOM.Code) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("InUse")) = 1 Then
                                        blnInUse = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If

                        If blnFound = False Then
                            'Error Message
                            Return False
                        End If
                        If blnFound = True And blnInUse = True Then
                            strSQL = BuildUpdate(MyInfo.TableName, " SET Flag = " & cFlagNonActive & _
                                " , LastUpdate = '" & objUOM.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, objUOM.UpdateBy) & "'" & _
                                " WHERE UOMCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objUOM.Code) & "'")
                        End If
                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(MyInfo.TableName, "UOMCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objUOM.Code) & "'")
                        End If
                        Try
                            'execute
                            objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try
                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As SystemException
                Throw exDelete
            Finally
                objUOM = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        'Enquiry
        Function Enquiry(Optional ByVal SQLstmt As String = Nothing) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    If SQLstmt = Nothing Or SQLstmt = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName)
                    Else
                        strSQL = SQLstmt
                    End If
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Function GetUOMConversion() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect("UOMCode, UOMDesc, UOMGroup, Measure", .TableName)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetListing() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetShortList() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function
#End Region
        'Clear all data in database
        Function ClearDB() As Boolean
        End Function
    End Class
#End Region

#Region "RoundList Base"
    Public Class RoundListBase
        Inherits Core.CoreBase
        Public Const MyFormCode As String = "GS0201S"
#Region "RoundList Class in RoundList Base"
        Public Class RoundList
            Protected strBranchId, strRoundCode, strDesc, strUpdBy As String
            Protected dblRoundFrom, dblRoundTo, dblAmt As Double
            Protected intType, intActive As Integer
            Protected dtLastUpd As DateTime
            Public MyScheme As New MyScheme
            Public Property BranchId() As String
                Get
                    Return strBranchId
                End Get
                Set(ByVal BranchId As String)
                    If BranchId.Trim = String.Empty Then
                        Throw New Exception("Please key in Round Code")
                    Else
                        strBranchId = BranchId
                    End If
                End Set
            End Property

            Public Property RoundCode() As String
                Get
                    Return strRoundCode
                End Get
                Set(ByVal RoundCode As String)
                    If RoundCode.Trim = String.Empty Then
                        Throw New Exception("Please key in Round Code")
                    Else
                        strRoundCode = RoundCode
                    End If
                End Set
            End Property

            Public Property Description() As String
                Get
                    Return strDesc
                End Get
                Set(ByVal Value As String)
                    strDesc = Value
                End Set
            End Property

            Public Property Type() As Integer
                Get
                    Return intType
                End Get
                Set(ByVal Value As Integer)
                    If Value < 0 Then
                        intType = 0
                    Else
                        intType = Value
                    End If
                End Set
            End Property

            Public Property Active() As Integer
                Get
                    Return intActive
                End Get
                Set(ByVal Value As Integer)
                    If Value < 0 Then
                        intActive = 0
                    Else
                        intActive = Value
                    End If
                End Set
            End Property

            Public Property RoundFrom() As Double
                Get
                    Return dblRoundFrom
                End Get
                Set(ByVal Value As Double)
                    dblRoundFrom = Value
                End Set
            End Property

            Public Property RoundTo() As Double
                Get
                    Return dblRoundTo
                End Get
                Set(ByVal Value As Double)
                    dblRoundTo = Value
                End Set
            End Property

            Public Property Amt() As Double
                Get
                    Return dblAmt
                End Get
                Set(ByVal Value As Double)
                    dblAmt = Value
                End Set
            End Property

            Public Property LastUpdate() As DateTime
                Get
                    Return dtLastUpd
                End Get
                Set(ByVal Value As DateTime)
                    dtLastUpd = Value
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return strUpdBy
                End Get
                Set(ByVal Value As String)
                    strUpdBy = Value
                End Set
            End Property
        End Class
#End Region

#Region "RoundList Scheme"
        Public Class MyScheme
            Inherits Core.SchemeBase
            Protected Overrides Sub InitializeInfo()
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "BranchId"
                    .Length = 10
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(0, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "RoundCode"
                    .Length = 10
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(1, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "Type"
                    .Length = 1
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(2, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "RoundDesc"
                    .Length = 50
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(3, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "RoundFrom"
                    .Length = 9
                    .RegExp = String.Empty
                    .DecPlace = 2
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(4, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "RoundTo"
                    .Length = 9
                    .DecPlace = 2
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(5, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "RoundAmt"
                    .Length = 9
                    .DecPlace = 2
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(6, this)
            End Sub

            Public ReadOnly Property BranchId() As StrucElement
                Get
                    Return MyBase.GetItem(0)
                End Get
            End Property

            Public ReadOnly Property RoundCode() As StrucElement
                Get
                    Return MyBase.GetItem(1)
                End Get
            End Property

            Public ReadOnly Property Type() As StrucElement
                Get
                    Return MyBase.GetItem(2)
                End Get
            End Property

            Public ReadOnly Property RoundDesc() As StrucElement
                Get
                    Return MyBase.GetItem(3)
                End Get
            End Property

            Public ReadOnly Property RoundFrom() As StrucElement
                Get
                    Return MyBase.GetItem(4)
                End Get
            End Property

            Public ReadOnly Property RoundTo() As StrucElement
                Get
                    Return MyBase.GetItem(5)
                End Get
            End Property

            Public ReadOnly Property RoundAmt() As StrucElement
                Get
                    Return MyBase.GetItem(6)
                End Get
            End Property

            Public Function GetElement(ByVal Key As Integer) As StrucElement
                Return MyBase.GetItem(Key)
            End Function
        End Class
#End Region

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = " BranchID, RoundCode, RoundType, RoundDesc, RoundFrom, RoundTo, RoundAmt,CreateDate, CreateBy, LastUpdate, UpdateBy, Active, Inuse, Flag"
                .CheckFields = "INUSE, FLAG"
                .TableName = "ROUNDSETUP"
                .DefaultCond = "FLAG = 1"
                .DefaultOrder = String.Empty
                .Listing = " BranchID, RoundCode, RoundType, RoundDesc, RoundFrom, RoundTo, RoundAmt,CreateDate, CreateBy, LastUpdate, UpdateBy, Active, Inuse, Flag"
                .ListingCond = "FLAG = 1"
                .ShortList = "RoundCode, RoundDesc"
                .ShortListCond = String.Empty
            End With
            ReDim MyFields(1)
            MyBase.AddMyField(0, "Round Code", "UOMCode", TypeCode.String)
            MyBase.AddMyField(1, "Round Desc", "UOMDesc", TypeCode.String)
        End Sub
#Region "Data Manipulation-Add,Edit,Del"
        Private Function AssignItem(ByVal objRoundList As RoundList, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As SqlClient.SqlDataReader
            AssignItem = False
            Try
                If objRoundList Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False

                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "RoundCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoundList.RoundCode) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        blnExec = True

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("Flag")) = 0 Then
                                        'Found but deleted
                                        blnFlag = False
                                    Else
                                        'Found and active
                                        blnFlag = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If
                    End If

                    If blnExec Then
                        If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                            'Item found & active
                            'system.Windows.Forms.MessageBox.Show("ab", "ABC",Windows.Forms.MessageBoxButtons.YesNo, Windows.Forms.MessageBoxIcon.Information,Windows.Forms.MessageBoxDefaultButton.Button2, Windows.Forms.MessageBoxOptions.DefaultDesktopOnly
                            'MsgBox("Record already exist", MsgBoxStyle.Critical, "Record Exist")
                            Return False
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = "ROUNDSETUP"
                                .AddField("BranchID", objRoundList.BranchId.ToString, SQLControl.EnumDataType.dtString)
                                .AddField("RoundCode", objRoundList.RoundCode, SQLControl.EnumDataType.dtString)
                                .AddField("RoundType", objRoundList.Type, SQLControl.EnumDataType.dtNumeric)
                                .AddField("RoundDesc", objRoundList.Description, SQLControl.EnumDataType.dtStringN)
                                .AddField("RoundFrom", objRoundList.RoundFrom.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("RoundTo", objRoundList.RoundTo.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("RoundAmt", objRoundList.Amt.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LastUpdate", objRoundList.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", objRoundList.UpdateBy.ToString, SQLControl.EnumDataType.dtString)
                                .AddField("Active", objRoundList.Active.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", cFlagActive.ToString, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                            "WHERE RoundCode='" & .ParseValue(SQLControl.EnumDataType.dtString, objRoundList.RoundCode) & "'")
                                        Else
                                            If blnFound = False Then
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                        "WHERE RoundCode='" & .ParseValue(SQLControl.EnumDataType.dtString, objRoundList.RoundCode) & "'")
                                End Select
                            End With
                            Try
                                'execute
                                objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002")
                                Else
                                    Throw New ApplicationException("210004")
                                End If
                            Finally
                                objSQL.Dispose()
                            End Try
                            Return True
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objRoundList = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
        'ADD
        Function Add(ByVal objRoundList As RoundList) As Boolean
            Return AssignItem(objRoundList, SQLControl.EnumSQLType.stInsert)
        End Function
        'AMEND
        Function Amend(ByVal objRoundList As RoundList) As Boolean
            Return AssignItem(objRoundList, SQLControl.EnumSQLType.stUpdate)
        End Function
        'DELETE
        Function Delete(ByVal objRoundList As RoundList) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If objRoundList Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "RoundCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoundList.RoundCode) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("InUse")) = 1 Then
                                        blnInUse = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If

                        If blnFound = False Then
                            'Error Message
                            Return False
                        End If
                        If blnFound = True And blnInUse = True Then
                            strSQL = BuildUpdate(MyInfo.TableName, " SET Flag = " & cFlagNonActive & _
                                " , LastUpdate = '" & objRoundList.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoundList.UpdateBy) & "'" & _
                                " WHERE RoundCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoundList.RoundCode) & "'")
                        End If
                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(MyInfo.TableName, "RoundCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoundList.RoundCode) & "'")
                        End If
                        Try
                            'execute
                            objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try
                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As SystemException
                Throw exDelete
            Finally
                objRoundList = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        'Enquiry
        Function Enquiry(Optional ByVal SQLstmt As String = Nothing) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    If SQLstmt = Nothing Or SQLstmt = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName)
                    Else
                        strSQL = SQLstmt
                    End If
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Function GetRoundListConversion() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(" BranchID, RoundCode, RoundType, RoundDesc, RoundFrom, RoundTo, RoundAmt,CreateDate, CreateBy, LastUpdate, UpdateBy, Active, Inuse, Flag", .TableName)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetListing() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetShortList() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function CheckRound(ByVal value As Double, ByVal type As Integer) As Boolean
            'Validate RoundFrom Columns
            If type = 1 Then
                If value > 0.0 And value < 0.1 Then
                    Return True
                Else
                    Return False
                End If
            ElseIf type = 2 Then
                If value > 0.09 And value < 1 Then
                    If value > 0.09 And value < 0.2 Then
                        If value <> 0.1 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 0.19 And value < 0.3 Then
                        If value <> 0.2 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 0.29 And value < 0.4 Then
                        If value <> 0.3 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 0.39 And value < 0.5 Then
                        If value <> 0.4 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 0.49 And value < 0.6 Then
                        If value <> 0.5 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 0.59 And value < 0.7 Then
                        If value <> 0.6 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 0.69 And value < 0.8 Then
                        If value <> 0.7 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 0.79 And value < 0.9 Then
                        If value <> 0.8 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 0.89 And value < 1.0 Then
                        If value <> 0.9 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                Else : Exit Function
                End If
            ElseIf type = 3 Then
                If value > 0.99 And value < 10 Then
                    If value > 0.99 And value < 2.0 Then
                        If value <> 1 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 1.99 And value < 3.0 Then
                        If value <> 2 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 2.99 And value < 4.0 Then
                        If value <> 3 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 3.99 And value < 5.0 Then
                        If value <> 4 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 4.99 And value < 6.0 Then
                        If value <> 5 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 5.99 And value < 7.0 Then
                        If value <> 6 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 6.99 And value < 8.0 Then
                        If value <> 7 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 7.99 And value < 9.0 Then
                        If value <> 8 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 8.99 And value < 10.0 Then
                        If value <> 9 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                Else : Exit Function
                End If
            ElseIf type = 4 Then
                If value > 9.99 And value < 100 Then
                    If value > 9.99 And value < 20 Then
                        If value <> 10.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 19.99 And value < 30 Then
                        If value <> 20.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 29.99 And value < 40 Then
                        If value <> 30.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 39.99 And value < 50 Then
                        If value <> 40.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 49.99 And value < 60 Then
                        If value <> 50.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 59.99 And value < 70 Then
                        If value <> 60.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 69.99 And value < 80 Then
                        If value <> 70.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 79.99 And value < 90 Then
                        If value <> 80.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 89.99 And value < 100 Then
                        If value <> 90.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                Else : Exit Function
                End If
            ElseIf type = 5 Then
                If value > 99.99 And value < 1000 Then
                    If value > 99.99 And value < 200 Then
                        If value <> 100.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 199.99 And value < 300 Then
                        If value <> 200.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 299.99 And value < 400 Then
                        If value <> 300.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 399.99 And value < 500 Then
                        If value <> 400.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 499.99 And value < 600 Then
                        If value <> 500.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 599.99 And value < 700 Then
                        If value <> 600.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 699.99 And value < 800 Then
                        If value <> 700.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 799.99 And value < 900 Then
                        If value <> 800.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 899.99 And value < 1000 Then
                        If value <> 900.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                Else : Exit Function
                End If
            ElseIf type = 6 Then
                If value > 999.99 And value < 10000 Then
                    If value > 999.99 And value < 2000 Then
                        If value <> 1000 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 1999.99 And value < 3000 Then
                        If value <> 2000.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 2999.99 And value < 4000 Then
                        If value <> 3000.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 3999.99 And value < 5000 Then
                        If value <> 4000.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 4999.99 And value < 6000 Then
                        If value <> 5000.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 5999.99 And value < 7000 Then
                        If value <> 6000.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 6999.99 And value < 8000 Then
                        If value <> 7000.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 7999.99 And value < 9000 Then
                        If value <> 8000.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 8999.99 And value < 10000 Then
                        If value <> 9000.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                Else : Exit Function
                End If
            ElseIf type = 7 Then
                If value > 9999.99 And value < 100000 Then
                    If value > 9999.99 And value < 20000 Then
                        If value <> 10000.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 19999.99 And value < 30000 Then
                        If value <> 20000.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 29999.99 And value < 40000 Then
                        If value <> 30000.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 39999.99 And value < 50000 Then
                        If value <> 40000.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 49999.99 And value < 60000 Then
                        If value <> 50000.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 59999.99 And value < 70000 Then
                        If value <> 60000.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 69999.99 And value < 80000 Then
                        If value <> 70000.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 79999.99 And value < 90000 Then
                        If value <> 80000.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    If value > 89999.99 And value < 100000 Then
                        If value <> 90000.0 Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                Else : Exit Function
                End If
            End If
        End Function
#End Region

        'Clear all data in database
        Function ClearDB() As Boolean
        End Function

    End Class
#End Region

#Region "Discount Group Base"
    Public Class DiscountGroupBase
        Inherits Core.CoreBase
#Region "Discount Group Class in DiscountGroupBase"
        Public Class DiscountGroup
            Protected strCreateBy As String
            Protected strDiscGrpDesc, strUpdateBy As String
            Protected intActive, intDiscGroup As Integer
            Protected dtLastUpd As DateTime
            Protected dtCreateDate As DateTime
            Public MyScheme As New MyScheme

            Public Property DiscGroup() As Integer
                Get
                    Return intDiscGroup
                End Get
                Set(ByVal Value As Integer)
                    intDiscGroup = Value
                End Set
            End Property

            Public Property DiscGrpDesc() As String
                Get
                    Return strDiscGrpDesc
                End Get
                Set(ByVal Value As String)
                    strDiscGrpDesc = Value
                End Set
            End Property
            Public Property LastUpdate() As DateTime
                Get
                    Return dtLastUpd
                End Get
                Set(ByVal Value As DateTime)
                    dtLastUpd = Value
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return strUpdateBy
                End Get
                Set(ByVal Value As String)
                    strUpdateBy = Value
                End Set
            End Property

            Public Property Active() As Integer
                Get
                    Return intActive
                End Get
                Set(ByVal Value As Integer)
                    intActive = Value
                End Set
            End Property

            Public Property CreateDate() As DateTime
                Get
                    Return dtCreateDate
                End Get
                Set(ByVal Value As DateTime)
                    dtCreateDate = Value
                End Set
            End Property
            Public Property CreateBy() As String
                Get
                    Return strCreateBy
                End Get
                Set(ByVal Value As String)
                    strCreateBy = Value
                End Set
            End Property
        End Class
#End Region

#Region "Discount Group Scheme"
        Public Class myScheme
            Inherits Core.SchemeBase
            Protected Overrides Sub InitializeInfo()

                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "DiscGrp"
                    .Length = 2
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(0, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "DiscGrpDesc"
                    .Length = 50
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
            End Sub
            Public ReadOnly Property DiscGroup() As StrucElement
                Get
                    Return MyBase.GetItem(0)
                End Get
            End Property

            Public ReadOnly Property DiscGrpDesc() As StrucElement
                Get
                    Return MyBase.GetItem(1)
                End Get
            End Property

            Public Function GetElement(ByVal Key As Integer) As StrucElement
                Return MyBase.GetItem(Key)
            End Function
        End Class

#End Region
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "DiscGrp, DiscGrpDesc, CreateDate, CreateBy, LastUpdate, UpdateBy, Acitve, Inuse, Flag"
                .CheckFields = "Inuse, Flag"
                .TableName = "DISCGROUP"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "DiscGrp, DiscGrpDesc, CreateDate, CreateBy, LastUpdate, UpdateBy, Active, Inuse, Flag"
                .ListingCond = "FLAG = 1"
                .ShortList = "DiscGrp, DiscGrpDesc"
                .ShortListCond = String.Empty
            End With
            ReDim MyFields(1)
            MyBase.AddMyField(0, "Discount Group", "DiscGrp", TypeCode.String)
            MyBase.AddMyField(1, "Discount Group Description", "DiscGrpDesc", TypeCode.String)
        End Sub
#Region "Data Manipulation-Add,Edit,Del"

        Private Function AssignItem(ByVal objDiscountGroup As DiscountGroup, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As SqlClient.SqlDataReader
            AssignItem = False
            Try
                If objDiscountGroup Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "DiscGrp = " & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, objDiscountGroup.DiscGroup.ToString) & "")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        blnExec = True
                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("Flag")) = 0 Then
                                        'Found but deleted
                                        blnFlag = False
                                    Else
                                        'Found and active
                                        blnFlag = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If

                    End If

                    If blnExec Then
                        If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                            Return False
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = MyInfo.TableName
                                .AddField("DiscGrpDesc", objDiscountGroup.DiscGrpDesc, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", objDiscountGroup.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", objDiscountGroup.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", objDiscountGroup.Active.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", cFlagActive.ToString, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                            "WHERE DiscGrp=" & .ParseValue(SQLControl.EnumDataType.dtNumeric, objDiscountGroup.DiscGroup.ToString) & "")
                                        Else
                                            If blnFound = False Then
                                                .AddField("DiscGrp", objDiscountGroup.DiscGroup.ToString, SQLControl.EnumDataType.dtNumeric)
                                                .AddField("CreateDate", objDiscountGroup.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                                .AddField("CreateBy", objDiscountGroup.UpdateBy, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                        "WHERE DiscGrp=" & .ParseValue(SQLControl.EnumDataType.dtNumeric, objDiscountGroup.DiscGroup.ToString) & "")
                                End Select
                            End With
                            Try
                                'execute
                                objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002")
                                Else
                                    Throw New ApplicationException("210004")
                                End If
                            End Try
                            objSQL.Dispose()
                            Return True
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objDiscountGroup = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Function Add(ByVal objDiscountGroup As DiscountGroup) As Boolean
            Return AssignItem(objDiscountGroup, SQLControl.EnumSQLType.stInsert)
        End Function

        Function Amend(ByVal objDiscountGroup As DiscountGroup) As Boolean
            Return AssignItem(objDiscountGroup, SQLControl.EnumSQLType.stUpdate)
        End Function

        Function Delete(ByVal objDiscountGroup As DiscountGroup) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If objDiscountGroup Is Nothing Then
                    'error message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        strSQL = "SELECT DiscGrp, Flag, InUse FROM DISCGROUP WHERE DiscGrp = " & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, objDiscountGroup.DiscGroup.ToString) & ""
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "DiscGrp = " & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, objDiscountGroup.DiscGroup.ToString) & "")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt32(.Item("InUse")) = 1 Then
                                        blnInUse = True 'if record is found and in use
                                    End If
                                End If
                                .Close()
                            End With

                        End If

                        'record is found and in use
                        If blnFound = True And blnInUse = True Then

                            strSQL = BuildUpdate(MyInfo.TableName, " SET Flag = " & cFlagNonActive & _
                                " , LastUpdate = '" & objDiscountGroup.LastUpdate & "' , UpdateBy = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objDiscountGroup.UpdateBy) & "'" & _
                                " WHERE DiscGrp = " & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, objDiscountGroup.DiscGroup.ToString) & "")

                        End If

                        'record exist but not in use
                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(MyInfo.TableName, "DiscGrp = " & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, objDiscountGroup.DiscGroup.ToString) & "")
                        End If
                        Try
                            'execute
                            objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try
                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As SystemException
                Throw exDelete
            Finally
                objDiscountGroup = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try

        End Function
#End Region

#Region "Data Selection"

        'Enquiry
        Function Enquiry(Optional ByVal SQLstmt As String = Nothing) As DataSet
            If StartConnection() = True Then
                With MyInfo

                    If SQLstmt = Nothing Or SQLstmt = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName)
                    Else
                        strSQL = SQLstmt
                    End If
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetListing() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetShortList() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

#End Region

        Function ClearDB() As Boolean
        End Function
    End Class
#End Region

#Region "Discount Base"
    Public Class DiscountBase
        Inherits Core.CoreBase


        Public Enum EnumDiscountType
            ItemPercentage = 0
            ItemAmount = 1
            TransPercentage = 2
            TransAmount = 3
        End Enum

#Region "Discount class in DiscountBase"
        Public Class Discount
            Protected strDiscCode, strDiscDesc, strUpdateBy As String
            Protected dblDefValue, dblMinTrxAmt, dblMaxValue As Double
            Protected intDiscType, intEmpDisc, intAuthReq, intAuthLevel, intDualReceipt, intActive, intDiscGroup As Integer
            Protected dtLastUpd As DateTime
            Protected dtEffDate As DateTime
            Protected dtEndDate As DateTime
            Public MyScheme As New MyScheme

            Public Property Code() As String
                Get
                    Return strDiscCode
                End Get
                Set(ByVal Value As String)
                    If Value.Trim = String.Empty Then
                        Throw New Exception("Please key in Discount Code")
                    Else
                        strDiscCode = Value
                    End If
                End Set
            End Property

            Public Property DiscGroup() As Integer
                Get
                    Return intDiscGroup
                End Get
                Set(ByVal Value As Integer)
                    intDiscGroup = Value
                End Set
            End Property

            Public Property DiscDesc() As String
                Get
                    Return strDiscDesc
                End Get
                Set(ByVal Value As String)
                    strDiscDesc = Value
                End Set
            End Property

            Public Property DiscType() As Integer
                Get
                    Return intDiscType
                End Get
                Set(ByVal Value As Integer)
                    intDiscType = Value
                End Set
            End Property

            Public Property DefValue() As Double
                Get
                    Return dblDefValue
                End Get
                Set(ByVal Value As Double)
                    dblDefValue = Value
                End Set
            End Property

            Public Property MinTrxAmt() As Double
                Get
                    Return dblMinTrxAmt
                End Get
                Set(ByVal Value As Double)
                    dblMinTrxAmt = Value
                End Set
            End Property

            Public Property MaxValue() As Double
                Get
                    Return dblMaxValue
                End Get
                Set(ByVal Value As Double)
                    dblMaxValue = Value
                End Set
            End Property

            Public Property EmpDisc() As Integer
                Get
                    Return intEmpDisc
                End Get
                Set(ByVal Value As Integer)
                    intEmpDisc = Value
                End Set
            End Property

            Public Property AuthReq() As Integer
                Get
                    Return intAuthReq
                End Get
                Set(ByVal Value As Integer)
                    intAuthReq = Value
                End Set
            End Property

            Public Property AuthLevel() As Integer
                Get
                    Return intAuthLevel
                End Get
                Set(ByVal Value As Integer)
                    intAuthLevel = Value
                End Set
            End Property

            Public Property DualReceipt() As Integer
                Get
                    Return intDualReceipt
                End Get
                Set(ByVal Value As Integer)
                    intDualReceipt = Value
                End Set
            End Property

            Public Property LastUpdate() As DateTime
                Get
                    Return dtLastUpd
                End Get
                Set(ByVal Value As DateTime)
                    dtLastUpd = Value
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return strUpdateBy
                End Get
                Set(ByVal Value As String)
                    strUpdateBy = Value
                End Set
            End Property

            Public Property Active() As Integer
                Get
                    Return intActive
                End Get
                Set(ByVal Value As Integer)
                    intActive = Value
                End Set
            End Property

            Public Property EffDate() As DateTime
                Get
                    Return dtEffDate
                End Get
                Set(ByVal Value As DateTime)
                    dtEffDate = Value
                End Set
            End Property

            Public Property EndDate() As DateTime
                Get
                    Return dtEndDate
                End Get
                Set(ByVal Value As DateTime)
                    dtEndDate = Value
                End Set
            End Property
        End Class
#End Region

#Region "Discount Scheme"
        Public Class MyScheme
            Inherits Core.SchemeBase

            Protected Overrides Sub InitializeInfo()
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "DiscCode"
                    .Length = 10
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(0, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "DiscGroup"
                    .Length = 2
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(1, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "DiscDesc"
                    .Length = 50
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(2, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "DefValue"
                    .Length = 8
                    .DecPlace = 2
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(3, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "MinTrxAmt"
                    .Length = 10
                    .DecPlace = 2
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(4, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "MaxValue"
                    .Length = 10
                    .DecPlace = 2
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(5, this)
            End Sub

            Public ReadOnly Property Code() As StrucElement
                Get
                    Return MyBase.GetItem(0)
                End Get
            End Property

            Public ReadOnly Property Group() As StrucElement
                Get
                    Return MyBase.GetItem(1)
                End Get
            End Property

            Public ReadOnly Property Description() As StrucElement
                Get
                    Return MyBase.GetItem(2)
                End Get
            End Property

            Public ReadOnly Property DefValue() As StrucElement
                Get
                    Return MyBase.GetItem(3)
                End Get
            End Property

            Public ReadOnly Property MinTrxAmt() As StrucElement
                Get
                    Return MyBase.GetItem(4)
                End Get
            End Property

            Public ReadOnly Property MaxValue() As StrucElement
                Get
                    Return MyBase.GetItem(5)
                End Get
            End Property

            Public Function GetElement(ByVal Key As Integer) As StrucElement
                Return MyBase.GetItem(Key)
            End Function
        End Class

#End Region

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "DISCCODE, DISCGROUP, DISCDESC, DISCTYPE, DEFVALUE, MINTRXAMT, MAXVALUE, EMPDISC, AUTHREQ, AUTHLEVEL, DUALRECEIPT, LASTUPDATE, UPDATEBY, ACTIVE, INUSE, FLAG, EFFDATE, ENDDATE"
                .CheckFields = "INUSE, FLAG"
                .TableName = "DISCOUNT"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "DiscCode, DiscGroup, DiscDesc, DiscType, DefValue, MinTrxAmt, MaxValue, EmpDisc, AuthReq, AuthLevel, DualReceipt, LastUpdate, UpdateBy, Active, Inuse, Flag, EffDate, EndDate"
                .ListingCond = "FLAG = 1"
                .ShortList = "DISCCODE, DiscDesc"
                .ShortListCond = String.Empty
            End With
            ReDim MyFields(2)
            MyBase.AddMyField(0, "Discount Code", "DiscCode", TypeCode.String)
            MyBase.AddMyField(1, "Discount Desc", "DiscDesc", TypeCode.String)
            MyBase.AddMyField(2, "Discount Group", "DiscGroup", TypeCode.String)
        End Sub
#Region "Data Manipulation-Add,Edit,Del"

        Private Function AssignItem(ByVal objDiscount As Discount, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As SqlClient.SqlDataReader
            AssignItem = False
            Try
                If objDiscount Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "DiscCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objDiscount.Code) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        blnExec = True
                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("Flag")) = 0 Then
                                        'Found but deleted
                                        blnFlag = False
                                    Else
                                        'Found and active
                                        blnFlag = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If

                    End If

                    If blnExec Then
                        If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                            Return False
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = MyInfo.TableName
                                .AddField("DiscDesc", objDiscount.DiscDesc, SQLControl.EnumDataType.dtStringN)
                                .AddField("DiscGroup", objDiscount.DiscGroup.ToString, SQLControl.EnumDataType.dtString)
                                .AddField("DiscType", objDiscount.DiscType.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("DefValue", objDiscount.DefValue.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("MinTrxAmt", objDiscount.MinTrxAmt.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("MaxValue", objDiscount.MaxValue.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("EmpDisc", objDiscount.EmpDisc.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("AuthReq", objDiscount.AuthReq.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("AuthLevel", objDiscount.AuthLevel.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("DualReceipt", objDiscount.DualReceipt.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("UpdateBy", objDiscount.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", objDiscount.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                .AddField("Active", objDiscount.Active.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", cFlagActive.ToString, SQLControl.EnumDataType.dtNumeric)

                                If objDiscount.EffDate <> Nothing And objDiscount.EndDate <> Nothing Then
                                    .AddField("EffDate", objDiscount.EffDate.ToString, SQLControl.EnumDataType.dtDateOnly)
                                    .AddField("EndDate", objDiscount.EndDate.ToString, SQLControl.EnumDataType.dtDateOnly)
                                ElseIf objDiscount.EffDate = Nothing And objDiscount.EndDate = Nothing Then
                                    .AddField("EffDate", "NULL", SQLControl.EnumDataType.dtString)
                                    .AddField("EndDate", "NULL", SQLControl.EnumDataType.dtString)
                                End If

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                            "WHERE DiscCode='" & .ParseValue(SQLControl.EnumDataType.dtString, objDiscount.Code) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("DiscCode", objDiscount.Code, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                        "WHERE DiscCode='" & .ParseValue(SQLControl.EnumDataType.dtString, objDiscount.Code) & "'")
                                End Select
                            End With
                            Try
                                'execute
                                objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                Return True
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002")
                                Else
                                    Throw New ApplicationException("210004")
                                End If
                            Finally
                                objSQL.Dispose()
                            End Try
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objDiscount = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Function Add(ByVal objDiscount As Discount) As Boolean
            Return AssignItem(objDiscount, SQLControl.EnumSQLType.stInsert)
        End Function

        Function Amend(ByVal objDiscount As Discount) As Boolean
            Return AssignItem(objDiscount, SQLControl.EnumSQLType.stUpdate)
        End Function

        Function Delete(ByVal objDiscount As Discount) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If objDiscount Is Nothing Then
                    'error message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        strSQL = "SELECT DiscCode, Flag, InUse FROM DISCOUNT WHERE DiscCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objDiscount.Code) & "'"
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "DiscCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objDiscount.Code) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt32(.Item("InUse")) = 1 Then
                                        blnInUse = True 'if record is found and in use
                                    End If
                                End If
                                .Close()
                            End With

                        End If

                        'record is found and in use
                        If blnFound = True And blnInUse = True Then

                            strSQL = BuildUpdate(MyInfo.TableName, " SET Flag = " & cFlagNonActive & _
                                " , LastUpdate = '" & objDiscount.LastUpdate & "' , UpdateBy = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objDiscount.UpdateBy) & "'" & _
                                " WHERE DiscCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objDiscount.Code) & "'")

                        End If

                        'record exist but not in use
                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(MyInfo.TableName, "DiscCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objDiscount.Code) & "'")
                        End If
                        Try
                            'execute
                            objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try
                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As SystemException
                Throw exDelete
            Finally
                objDiscount = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try

        End Function

#End Region

#Region "Data Selection"

        'Enquiry
        Function Enquiry(Optional ByVal SQLstmt As String = Nothing) As DataSet
            If StartConnection() = True Then
                With MyInfo

                    If SQLstmt = Nothing Or SQLstmt = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName)
                    Else
                        strSQL = SQLstmt
                    End If
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetListing() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetShortList() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

#End Region

        Function ClearDB() As Boolean
        End Function
    End Class
#End Region

#Region "Remark Base"
    Public Class RemarkBase
        Inherits Core.CoreBase

#Region "Remark class in Remark Base"
        Public Class Remark
            Protected strName, strDesc, strUpdBy, strType As String
            Protected intActive As Integer
            Protected dtLastUpd As DateTime
            Protected strRemarkType As String


            Public Sub New()

            End Sub

            Public Property Name() As String
                Get
                    Return strName
                End Get
                Set(ByVal RemarkName As String)
                    If RemarkName.Trim = String.Empty Then
                        Throw New Exception("Please key in Remark Code")
                    Else
                        strName = RemarkName
                    End If
                End Set
            End Property

            Public Property Description() As String
                Get
                    Return strDesc
                End Get
                Set(ByVal Value As String)
                    strDesc = Value
                End Set
            End Property

            Public Property Type() As String
                Get
                    Return strType
                End Get
                Set(ByVal Value As String)
                    strType = Value
                End Set
            End Property

            Public Property RemarkType() As String
                Get
                    Return strRemarkType
                End Get
                Set(ByVal Value As String)
                    strRemarkType = Value
                End Set
            End Property

            Public Property LastUpdate() As DateTime
                Get
                    Return dtLastUpd
                End Get
                Set(ByVal Value As DateTime)
                    dtLastUpd = Value
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return strUpdBy
                End Get
                Set(ByVal Value As String)
                    strUpdBy = Value
                End Set
            End Property

            Public Property Active() As Integer
                Get
                    Return intActive
                End Get
                Set(ByVal Value As Integer)
                    intActive = Value
                End Set
            End Property
        End Class

#End Region

#Region "Remark Scheme"
        Public Class MyScheme
            Inherits Core.SchemeBase

            Protected Overrides Sub InitializeInfo()
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "RemarkCode"
                    .Length = 5
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(0, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "RemarkDesc"
                    .Length = 50
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(1, this)
            End Sub


            Public ReadOnly Property Code() As StrucElement
                Get
                    Return MyBase.GetItem(0)
                End Get
            End Property

            Public ReadOnly Property Description() As StrucElement
                Get
                    Return MyBase.GetItem(1)
                End Get
            End Property



            Public Function GetElement(ByVal Key As Integer) As StrucElement
                Return MyBase.GetItem(Key)
            End Function
        End Class
#End Region

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "REMARKCODE, REMARKDESC, REMARKTYPE, LASTUPDATE, UPDATEBY, ACTIVE, INUSE, FLAG"
                .CheckFields = "INUSE, FLAG"
                .TableName = "Remark"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "REMARKCODE, REMARKDESC, REMARKTYPE, LASTUPDATE, UPDATEBY, ACTIVE, INUSE, FLAG"
                .ListingCond = "FLAG = 1 AND Active = 1"
                .ShortList = String.Empty
                .ShortListCond = String.Empty
            End With
            ReDim MyFields(1)
            MyBase.AddMyField(0, "Remark Code", "RemarkCode", TypeCode.String)
            MyBase.AddMyField(1, "Remark Desc", "RemarkDesc", TypeCode.String)
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function AssignItem(ByVal objRemark As Remark, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As SqlClient.SqlDataReader
            AssignItem = False
            Try
                If objRemark Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "RemarkCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRemark.Name) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
                        blnExec = True

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("Flag")) = 0 Then
                                        'Found but deleted
                                        blnFlag = False
                                    Else
                                        'Found and active
                                        blnFlag = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If
                    End If

                    If blnExec Then
                        If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                            'Item found & active
                            'system.Windows.Forms.MessageBox.Show("ab", "ABC",Windows.Forms.MessageBoxButtons.YesNo, Windows.Forms.MessageBoxIcon.Information,Windows.Forms.MessageBoxDefaultButton.Button2, Windows.Forms.MessageBoxOptions.DefaultDesktopOnly
                            'MsgBox("Record already exist", MsgBoxStyle.Critical, "Record Exist")
                            Return False
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = "REMARK"
                                .AddField("REMARKDesc", objRemark.Description, SQLControl.EnumDataType.dtStringN)
                                .AddField("REMARKType", objRemark.Type, SQLControl.EnumDataType.dtString)
                                .AddField("Active", objRemark.Active.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", cFlagActive.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("UpdateBy", objRemark.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", objRemark.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                            "WHERE remarkCode='" & .ParseValue(SQLControl.EnumDataType.dtString, objRemark.Name) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("remarkCode", objRemark.Name, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                        "WHERE remarkCode='" & .ParseValue(SQLControl.EnumDataType.dtString, objRemark.Name) & "'")
                                End Select
                            End With
                            Try
                                'execute
                                objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002")
                                Else
                                    Throw New ApplicationException("210004")
                                End If
                            Finally
                                objSQL.Dispose()
                            End Try
                            Return True
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objRemark = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Function Add(ByVal objRemark As Remark) As Boolean
            Return AssignItem(objRemark, SQLControl.EnumSQLType.stInsert)
        End Function

        'AMEND
        Function Amend(ByVal objRemark As Remark) As Boolean
            Return AssignItem(objRemark, SQLControl.EnumSQLType.stUpdate)
        End Function
        'DELETE
        Function Delete(ByVal objRemark As Remark) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If objRemark Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "RemarkCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRemark.Name) & "'")
                            rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                            If rdr Is Nothing = False Then
                                With rdr
                                    If .Read Then
                                        blnFound = True
                                        If Convert.ToInt16(.Item("InUse")) = 1 Then
                                            blnInUse = True
                                        End If
                                    End If
                                    .Close()
                                End With
                            End If
                        End With

                        If blnFound = False Then
                            'Error Message
                            Return False
                        End If
                        If blnFound = True And blnInUse = True Then
                            'With objSQL
                            '.TableName = "Remark"
                            '.AddField("Flag", cFlagNonActive.ToString, SQLControl.EnumDataType.dtNumeric)
                            'strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                            '"WHERE RemarkCode='" & .ParseValue(SQLControl.EnumDataType.dtString, objRemark.Name) & "'")
                            'End With
                            strSQL = BuildUpdate(MyInfo.TableName, " SET Flag = " & cFlagNonActive & _
                            " , LastUpdate = '" & objRemark.LastUpdate & "' , UpdateBy = '" & _
                            objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRemark.UpdateBy) & "'" & _
                            " WHERE RemarkCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRemark.Name) & "'")
                        End If
                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(MyInfo.TableName, "RemarkCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRemark.Name) & "'")
                        End If
                        Try
                            'execute
                            objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try
                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As SystemException
                Throw exDelete
            Finally
                objRemark = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        'Enquiry
        Function Enquiry(Optional ByVal SQLstmt As String = Nothing) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    If SQLstmt = Nothing Or SQLstmt = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName)
                    Else
                        strSQL = SQLstmt
                    End If
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function


        Public Function GetListing() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function
        Public Function GetRemarkListing() As DataTable
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), DataTable)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function
        Public Function GetShortList() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function
        Public Function ProcessSearch(ByVal RemarkType As String) As DataSet
            Dim strCondition As String = "RemarkType ='" & RemarkType & "'"
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, strCondition)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

#End Region

        'Clear all data in database
        Function ClearDB() As Boolean
        End Function


        Public Shared Function GetRemarkInfo(ByVal RemarkType As String) As DataTable
            Try
                StartSQLControl()
                StartConnection()

                strSQL = "SELECT RemarkCode, RemarkDesc FROM Remark WHERE RemarkType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, RemarkType) & "' AND Active = 1 AND FLAG = 1"

                Return objDCom.Execute(strSQL, CommandType.Text, True)
            Catch ex As Exception
                Return Nothing
            Finally
                EndConnection()
                EndSQLControl()
            End Try

        End Function
    End Class
#End Region

#Region "History Base"
    Public Class HistoryBase
        Inherits Core.CoreBase

        Public Shared Function GetItemList(ByVal BranchID As String, ByVal CustomerID As String) As DataTable
            Dim strSQL As String
            Dim dtTable As DataTable
            Try
                StartSQLControl()
                StartConnection()
                strSQL = "SELECT D.StkCode, D.StkDesc, D.OrgPrice, D.Qty, D.DiscAmt, H.TransDate, H.TransNo, H.CustPkgID " & _
                            "FROM TRANSDTL D INNER JOIN TRANSHDR H ON D.TransNo = H.TransNo " & _
                            "WHERE (H.CustomerID = '" & CustomerID & "') AND (H.TransStatus = '1') AND (H.TransType = '0') AND (H.CustPkgID = '') AND (H.BranchID = '" & BranchID & "')" & _
                            "ORDER BY D.TransNo DESC"
                dtTable = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text), DataTable)
                Return dtTable
            Catch ex As Exception
                Return Nothing
            Finally
                EndConnection()
                EndSQLControl()
            End Try
        End Function

        Public Shared Function GetTransList(ByVal BranchID As String, ByVal CustomerID As String) As DataTable
            Dim strSQL As String
            Dim dtTable As DataTable
            Try
                StartSQLControl()
                StartConnection()
                strSQL = "SELECT TransDate, TransType, TransAmt, TransNo " & _
                            "FROM TRANSHDR " & _
                            "WHERE (CustomerID = '" & CustomerID & "') AND (TransType IN ('0', '3')) AND BranchID = '" & BranchID & "' AND (TransStatus = 1) " & _
                            "ORDER BY TransNo DESC, TransDate DESC"
                dtTable = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text), DataTable)
                Return dtTable
            Catch ex As Exception
                Return Nothing
            Finally
                EndConnection()
                EndSQLControl()
            End Try
        End Function

    End Class
#End Region

#Region "Tender Base"
    Public Class TenderBase
        Inherits Core.CoreBase

        Public Const CASHID As String = "000"
        Public Const VOUCHERID As String = "599"
        Public Const PREPAIDID As String = "399"
        Public Const CREDITID As String = "999"
        Public Const CREDITNOTEID As String = "998"
        Public Const POINTID As String = "997"
        Public Const VOUCHERNO As String = "996"
        Public Const POINTREDEEM As String = "990"

        Public Enum EnumTenderSelection
            All = 0
            IncOneCash = 1
            ExcCreditPrepaid = 2
            ExcCreditPrepaidIncCN = 3
        End Enum

        Public Enum EnumTenderType
            Cash = 0
            CreditCard = 1
            Voucher = 2
            EFT = 3
            Cheque = 4
            PrepaidCard = 5
            CreditPurchase = 6
            Others = 7
            CreditNote = 8
            Point = 9
            StockVoucher = 10
            RedeemPoints = 11
        End Enum

#Region "Tender class in Tender Base"
        Public Class Tender
            Protected strTenderID, strTenderDesc, strUpdBy As String
            Protected strTenderCurr, strTenderPrmp, strRefNoPrmp As String
            Protected dblPickup1, dblPickup2, dblMinAmt, dblMaxAmt, dblDefaultValue As Double
            Protected intAllowPickup, intAllowFloat, intAllowOverTender, intVoucherPeriod As Integer
            Protected intOpenDrw, intTrackRef As Integer
            Protected intExchgRt, intActive, intTenderType As Integer
            Protected dtLastUpd As DateTime
            Protected dtEffDate As DateTime
            Protected dtEndDate As DateTime

            Public Property ID() As String
                Get
                    ID = strTenderID
                End Get
                Set(ByVal Value As String)
                    If Value.Trim = String.Empty Then
                        Throw New Exception("Please enter Tender ID")
                    Else
                        strTenderID = Value
                    End If
                End Set
            End Property

            Public Property Description() As String
                Get
                    Description = strTenderDesc
                End Get
                Set(ByVal Value As String)
                    If Value.Trim = String.Empty Then
                        Throw New Exception("Please enter Tender Description")
                    Else
                        strTenderDesc = Value
                    End If
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    UpdateBy = strUpdBy
                End Get
                Set(ByVal Value As String)
                    strUpdBy = Value
                End Set
            End Property

            Public Property TenderType() As Integer
                Get
                    TenderType = intTenderType
                End Get
                Set(ByVal Value As Integer)
                    intTenderType = Value
                End Set
            End Property

            Public Property Currency() As String
                Get
                    Currency = strTenderCurr
                End Get
                Set(ByVal Value As String)
                    strTenderCurr = Value
                End Set
            End Property

            Public Property TenderPrompt() As String
                Get
                    TenderPrompt = strTenderPrmp
                End Get
                Set(ByVal Value As String)
                    strTenderPrmp = Value
                End Set
            End Property

            Public Property RefNoPrompt() As String
                Get
                    RefNoPrompt = strRefNoPrmp
                End Get
                Set(ByVal Value As String)
                    strRefNoPrmp = Value
                End Set
            End Property

            Public Property ExchangeRate() As Integer
                Get
                    ExchangeRate = intExchgRt
                End Get
                Set(ByVal Value As Integer)
                    intExchgRt = Value
                End Set
            End Property

            Public Property Pickup1() As Double
                Get
                    Pickup1 = dblPickup1
                End Get
                Set(ByVal Value As Double)
                    dblPickup1 = Value
                End Set
            End Property

            Public Property Pickup2() As Double
                Get
                    Pickup2 = dblPickup2
                End Get
                Set(ByVal Value As Double)
                    dblPickup2 = Value
                End Set
            End Property

            Public Property MinAmount() As Double
                Get
                    MinAmount = dblMinAmt
                End Get
                Set(ByVal Value As Double)
                    dblMinAmt = Value
                End Set
            End Property

            Public Property DefaultValue() As Double
                Get
                    DefaultValue = dblDefaultValue
                End Get
                Set(ByVal Value As Double)
                    dblDefaultValue = Value
                End Set
            End Property

            Public Property MaxAmount() As Double
                Get
                    MaxAmount = dblMaxAmt
                End Get
                Set(ByVal Value As Double)
                    dblMaxAmt = Value
                End Set
            End Property

            Public Property LastUpdate() As DateTime
                Get
                    LastUpdate = dtLastUpd
                End Get
                Set(ByVal Value As DateTime)
                    dtLastUpd = Value
                End Set
            End Property

            Public Property AllowPickup() As Integer
                Get
                    AllowPickup = intAllowPickup
                End Get
                Set(ByVal Value As Integer)
                    intAllowPickup = Value

                End Set
            End Property

            Public Property AllowFloat() As Integer
                Get
                    AllowFloat = intAllowFloat
                End Get
                Set(ByVal Value As Integer)
                    intAllowFloat = Value
                End Set
            End Property

            Public Property AllowOverTender() As Integer
                Get
                    AllowOverTender = intAllowOverTender
                End Get
                Set(ByVal Value As Integer)
                    intAllowOverTender = Value
                End Set
            End Property

            Public Property OpenDrawer() As Integer
                Get
                    OpenDrawer = intOpenDrw
                End Get
                Set(ByVal Value As Integer)
                    intOpenDrw = Value
                End Set
            End Property

            Public Property TrackRef() As Integer
                Get
                    TrackRef = intTrackRef
                End Get
                Set(ByVal Value As Integer)
                    intTrackRef = Value
                End Set
            End Property

            Public Property Active() As Integer
                Get
                    Active = intActive
                End Get
                Set(ByVal Value As Integer)
                    intActive = Value
                End Set
            End Property

            Public Property EffDate() As DateTime
                Get
                    Return dtEffDate
                End Get
                Set(ByVal Value As DateTime)
                    dtEffDate = Value
                End Set
            End Property

            Public Property EndDate() As DateTime
                Get
                    Return dtEndDate
                End Get
                Set(ByVal Value As DateTime)
                    dtEndDate = Value
                End Set
            End Property

            Public Property VoucherPeriod() As Integer
                Get
                    Return intVoucherPeriod
                End Get
                Set(ByVal Value As Integer)
                    intVoucherPeriod = Value
                End Set
            End Property
        End Class
#End Region

#Region "Tender Scheme"
        Public Class MyScheme
            Inherits Core.SchemeBase

            Protected Overrides Sub InitializeInfo()
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "TenderID"
                    .Length = 10
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(0, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "TenderDesc"
                    .Length = 50
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(1, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "TenderPrompt"
                    .Length = 50
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(2, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "RefPrompt"
                    .Length = 50
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(3, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "DefValue"
                    .Length = 10
                    .DecPlace = 2
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(4, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "Pickup1"
                    .Length = 10
                    .DecPlace = 2
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(5, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "Pickup2"
                    .Length = 10
                    .DecPlace = 2
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(6, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "MinTenderAmt"
                    .Length = 10
                    .DecPlace = 2
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(7, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "MaxTenderAmt"
                    .Length = 10
                    .DecPlace = 2
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(8, this)
            End Sub

            Public ReadOnly Property ID() As StrucElement
                Get
                    Return MyBase.GetItem(0)
                End Get
            End Property

            Public ReadOnly Property Description() As StrucElement
                Get
                    Return MyBase.GetItem(1)
                End Get
            End Property


            Public ReadOnly Property TPrompt() As StrucElement
                Get
                    Return MyBase.GetItem(2)
                End Get
            End Property


            Public ReadOnly Property RefPrompt() As StrucElement
                Get
                    Return MyBase.GetItem(3)
                End Get
            End Property


            Public ReadOnly Property DefValue() As StrucElement
                Get
                    Return MyBase.GetItem(4)
                End Get
            End Property


            Public ReadOnly Property Pickup1() As StrucElement
                Get
                    Return MyBase.GetItem(5)
                End Get
            End Property


            Public ReadOnly Property Pickup2() As StrucElement
                Get
                    Return MyBase.GetItem(6)
                End Get
            End Property


            Public ReadOnly Property MinTenderAmt() As StrucElement
                Get
                    Return MyBase.GetItem(7)
                End Get
            End Property


            Public ReadOnly Property MaxTenderAmt() As StrucElement
                Get
                    Return MyBase.GetItem(8)
                End Get
            End Property

            Public Function GetElement(ByVal Key As Integer) As StrucElement
                Return MyBase.GetItem(Key)
            End Function
        End Class

#End Region

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "TenderID, TenderType, TenderDesc, CurrencyCode, TenderPrompt, RefPrompt, DefValue, Pickup1, Pickup2, MinTenderAmt, MaxTenderAmt, AllowPickup, AllowFloat, AllowOverTender, OpenDrawer, TrackRefNo, LastUpdate, UpdateBy, Active, Inuse, Flag, EffDate, EndDate"
                .CheckFields = "INUSE, FLAG"
                .TableName = "Tender"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "TenderID, TenderType, TenderDesc, CurrencyCode, TenderPrompt, RefPrompt, DefValue, Pickup1, Pickup2, MinTenderAmt, MaxTenderAmt, AllowPickup, AllowFloat, AllowOverTender, OpenDrawer, TrackRefNo, LastUpdate, UpdateBy, Active, Inuse, Flag, EffDate, EndDate"
                .ListingCond = "FLAG = 1"
                .ShortList = String.Empty
                .ShortListCond = String.Empty
            End With
            ReDim MyFields(2)
            MyBase.AddMyField(0, "Tender ID", "TenderID", TypeCode.String)
            MyBase.AddMyField(1, "Tender Desc", "TenderDesc", TypeCode.String)
            MyBase.AddMyField(2, "Tender Type", "TenderType", TypeCode.String)
        End Sub

#Region "Data Manipulation-Add,Edit,Del"

        Private Function AssignItem(ByVal objTender As Tender, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As SqlClient.SqlDataReader
            AssignItem = False
            Try
                If objTender Is Nothing Then
                    'msg return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If blnConnected Then
                        If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                            StartSQLControl()
                            With MyInfo
                                strSQL = BuildSelect(.CheckFields, .TableName, "TenderID ='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTender.ID) & "'")
                            End With
                            rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
                            blnExec = True 'executed - select

                            If rdr Is Nothing = False Then
                                With rdr
                                    If .Read Then
                                        'record is found
                                        blnFound = True
                                        If Convert.ToInt32(.Item("Flag")) = 0 Then
                                            'found but deleted
                                            blnFlag = False
                                        Else
                                            'found and active
                                            blnFlag = True
                                        End If
                                    End If
                                    .Close()
                                End With
                            End If
                        End If
                    End If

                    If blnExec Then 'if executed
                        If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                            Return False
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = MyInfo.TableName
                                .AddField("TenderType", objTender.TenderType.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("TenderDesc", objTender.Description, SQLControl.EnumDataType.dtStringN)
                                .AddField("CurrencyCode", objTender.Currency, SQLControl.EnumDataType.dtString)
                                .AddField("TenderPrompt", objTender.TenderPrompt, SQLControl.EnumDataType.dtStringN)
                                .AddField("RefPrompt", objTender.RefNoPrompt, SQLControl.EnumDataType.dtStringN)
                                .AddField("DefValue", objTender.DefaultValue.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Pickup1", objTender.Pickup1.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Pickup2", objTender.Pickup2.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("MinTenderAmt", objTender.MinAmount.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("MaxTenderAmt", objTender.MaxAmount.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("AllowPickup", objTender.AllowPickup.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("AllowFloat", objTender.AllowFloat.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("AllowOverTender", objTender.AllowOverTender.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpenDrawer", objTender.OpenDrawer.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("TrackRefNo", objTender.TrackRef.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LastUpdate", objTender.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", objTender.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", objTender.Active.ToString, SQLControl.EnumDataType.dtNumeric)
                                If objTender.VoucherPeriod = 1 Then
                                    .AddField("EffDate", objTender.EffDate.ToString, SQLControl.EnumDataType.dtDateOnly)
                                    .AddField("EndDate", objTender.EndDate.ToString, SQLControl.EnumDataType.dtDateOnly)
                                End If
                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert 'INSERT
                                        'record found but not deleted
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE TenderID = '" & .ParseValue(SQLControl.EnumDataType.dtString, objTender.ID) & "'")
                                        Else
                                            If blnFound = False Then 'if record not found
                                                .AddField("TenderID", objTender.ID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate 'UPDATE
                                        If objTender.VoucherPeriod = 0 Then
                                            .AddField("EffDate", "NULL", SQLControl.EnumDataType.dtStringN)
                                            .AddField("EndDate", "NULL", SQLControl.EnumDataType.dtStringN)
                                        End If
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE TenderID = '" & .ParseValue(SQLControl.EnumDataType.dtString, objTender.ID) & "'")
                                End Select
                            End With
                            Try
                                'execute
                                objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002")
                                Else
                                    Throw New ApplicationException("210004")
                                End If
                            Finally
                                objSQL.Dispose()
                            End Try
                            Return True
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objTender = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Function Add(ByVal objTender As Tender) As Boolean
            Return AssignItem(objTender, SQLControl.EnumSQLType.stInsert)
        End Function

        Function Amend(ByVal objTender As Tender) As Boolean
            Return AssignItem(objTender, SQLControl.EnumSQLType.stUpdate)
        End Function

        Function Delete(ByVal objTender As Tender) As Boolean
            Dim strSQL As String
            Dim blnFound, blnInUse As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If objTender Is Nothing Then
                    'error message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "TenderID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTender.ID) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt32(.Item("InUse")) = 1 Then
                                        blnInUse = True 'if record is found and in use
                                    End If
                                End If
                                .Close()
                            End With
                        End If

                        'record is found and in use
                        If blnFound = True And blnInUse = True Then

                            strSQL = BuildUpdate(MyInfo.TableName, " SET Flag = " & cFlagNonActive & _
                                " , LastUpdate = '" & objTender.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTender.UpdateBy) & "'" & _
                                " WHERE TenderID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTender.ID) & "'")
                        End If

                        'record exist but not in use
                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(MyInfo.TableName, "TenderID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTender.ID) & "'")
                        End If
                        Try
                            'execute
                            objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try
                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As SystemException
                Throw exDelete
            Finally
                objTender = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

#End Region

#Region "Data Selection"
        'Enquiry
        Function Enquiry(Optional ByVal SQLstmt As String = Nothing) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    If SQLstmt = Nothing Or SQLstmt = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName)
                    Else
                        strSQL = SQLstmt
                    End If
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetListing() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetShortList() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Shared Sub GetTenderDescType(ByVal TenderID As String, ByRef TenderDesc As String, ByRef TenderType As Int32)
            Dim rdrTender As SqlClient.SqlDataReader
            Try
                StartSQLControl()
                StartConnection()

                strSQL = "SELECT TenderDesc, TenderType FROM Tender WHERE TenderID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, TenderID) & "'"
                rdrTender = CType(objDCom.Execute(strSQL, CommandType.Text, CommandBehavior.CloseConnection, False), SqlClient.SqlDataReader)

                If rdrTender Is Nothing = False Then
                    If rdrTender.Read Then
                        TenderDesc = Convert.ToString(rdrTender("TenderDesc"))
                        TenderType = Convert.ToInt32(rdrTender("TenderType"))
                    Else
                        TenderDesc = String.Empty
                        TenderType = 0
                    End If

                    rdrTender.Close()
                End If
            Catch ex As Exception
            Finally
                rdrTender = Nothing
                EndConnection()
                EndSQLControl()
            End Try
        End Sub

        Public Shared Function GetTenderInfo(ByVal TenderSelection As EnumTenderSelection) As DataTable
            Dim rdrPay As SqlClient.SqlDataReader
            Dim dtPay As DataTable
            'Dim dsPay As DataSet
            Dim drRow As DataRow
            Dim strSQL As String

            Try
                StartConnection()
                If dtPay Is Nothing = True Then
                    dtPay = New DataTable
                    With dtPay
                        .Columns.Add("TenderID", Type.GetType("System.String"))
                        .Columns.Add("TenderDescription", Type.GetType("System.String"))
                        .Columns.Add("TenderType", Type.GetType("System.Int32"))
                        .Columns.Add("OpenDrawer", Type.GetType("System.Int32"))
                        .Columns.Add("AllowOverTender", Type.GetType("System.Int32"))
                        .Columns.Add("TrackRefNo", Type.GetType("System.Int32"))
                        .Columns.Add("TrackClrDate", Type.GetType("System.Int32"))
                    End With
                End If

                Select Case TenderSelection
                    Case EnumTenderSelection.All
                        strSQL = "SELECT TenderID, TenderDesc, TenderType, OpenDrawer, AllowOverTender, TrackRefNo, TrackClrDate FROM Tender WHERE TenderType NOT IN (8) AND Active = 1 AND Flag = 1"
                        rdrPay = CType(objDCom.Execute(strSQL, CommandType.Text, CommandBehavior.SingleRow, False), SqlClient.SqlDataReader)
                        If rdrPay Is Nothing = False Then
                            If rdrPay.Read Then
                                drRow = dtPay.NewRow
                                drRow("TenderID") = Convert.ToString(rdrPay("TenderID"))
                                drRow("TenderDescription") = Convert.ToString(rdrPay("TenderDesc"))
                                drRow("TenderType") = Convert.ToInt32(rdrPay("TenderType"))
                                drRow("OpenDrawer") = Convert.ToInt32(rdrPay("OpenDrawer"))
                                drRow("AllowOverTender") = Convert.ToInt32(rdrPay("AllowOverTender"))
                                drRow("TrackRefNo") = Convert.ToInt32(rdrPay("TrackRefNo"))
                                drRow("TrackClrDate") = Convert.ToInt32(rdrPay("TrackClrDate"))

                                dtPay.Rows.Add(drRow)
                                drRow = Nothing

                            End If
                            rdrPay.Close()
                        End If


                    Case EnumTenderSelection.IncOneCash
                        strSQL = "SELECT TenderID, TenderDesc, TenderType, OpenDrawer, AllowOverTender, TrackRefNo, TrackClrDate FROM Tender WHERE TenderID = '000'"
                        rdrPay = CType(objDCom.Execute(strSQL, CommandType.Text, CommandBehavior.SingleRow, False), SqlClient.SqlDataReader)
                        If rdrPay Is Nothing = False Then
                            If rdrPay.Read Then
                                drRow = dtPay.NewRow
                                drRow("TenderID") = Convert.ToString(rdrPay("TenderID"))
                                drRow("TenderDescription") = Convert.ToString(rdrPay("TenderDesc"))
                                drRow("TenderType") = Convert.ToInt32(rdrPay("TenderType"))
                                drRow("OpenDrawer") = Convert.ToInt32(rdrPay("OpenDrawer"))
                                drRow("AllowOverTender") = Convert.ToInt32(rdrPay("AllowOverTender"))
                                drRow("TrackRefNo") = Convert.ToInt32(rdrPay("TrackRefNo"))
                                drRow("TrackClrDate") = Convert.ToInt32(rdrPay("TrackClrDate"))

                                dtPay.Rows.Add(drRow)
                                drRow = Nothing

                            End If
                            rdrPay.Close()
                        End If

                        strSQL = "SELECT TenderID, TenderDesc, TenderType, OpenDrawer, AllowOverTender, TrackRefNo, TrackClrDate FROM TENDER WHERE TenderType NOT IN (0, 8) AND Active = 1 AND Flag = 1"
                        rdrPay = CType(objDCom.Execute(strSQL, CommandType.Text, CommandBehavior.CloseConnection, False), SqlClient.SqlDataReader)
                        If rdrPay Is Nothing = False Then
                            While rdrPay.Read
                                drRow = dtPay.NewRow
                                drRow("TenderID") = Convert.ToString(rdrPay("TenderID"))
                                drRow("TenderDescription") = Convert.ToString(rdrPay("TenderDesc"))
                                drRow("TenderType") = Convert.ToInt32(rdrPay("TenderType"))
                                drRow("OpenDrawer") = Convert.ToInt32(rdrPay("OpenDrawer"))
                                drRow("AllowOverTender") = Convert.ToInt32(rdrPay("AllowOverTender"))
                                drRow("TrackRefNo") = Convert.ToInt32(rdrPay("TrackRefNo"))
                                drRow("TrackClrDate") = Convert.ToInt32(rdrPay("TrackClrDate"))
                                dtPay.Rows.Add(drRow)
                            End While

                            drRow = Nothing
                            rdrPay.Close()
                        End If


                    Case EnumTenderSelection.ExcCreditPrepaid
                        strSQL = "SELECT TenderID, TenderDesc, TenderType, OpenDrawer, AllowOverTender, TrackRefNo, TrackClrDate FROM Tender WHERE TenderID = '000'"
                        rdrPay = CType(objDCom.Execute(strSQL, CommandType.Text, CommandBehavior.SingleRow, False), SqlClient.SqlDataReader)
                        If rdrPay Is Nothing = False Then
                            If rdrPay.Read Then
                                drRow = dtPay.NewRow
                                drRow("TenderID") = Convert.ToString(rdrPay("TenderID"))
                                drRow("TenderDescription") = Convert.ToString(rdrPay("TenderDesc"))
                                drRow("TenderType") = Convert.ToInt32(rdrPay("TenderType"))
                                drRow("OpenDrawer") = Convert.ToInt32(rdrPay("OpenDrawer"))
                                drRow("AllowOverTender") = Convert.ToInt32(rdrPay("AllowOverTender"))
                                drRow("TrackRefNo") = Convert.ToInt32(rdrPay("TrackRefNo"))
                                drRow("TrackClrDate") = Convert.ToInt32(rdrPay("TrackClrDate"))

                                dtPay.Rows.Add(drRow)
                                drRow = Nothing

                            End If
                            rdrPay.Close()
                        End If


                        strSQL = "SELECT TenderID, TenderDesc, TenderType, OpenDrawer, AllowOverTender, TrackRefNo, TrackClrDate FROM TENDER WHERE TenderType NOT IN (0, 5, 6, 8) AND Active = 1 AND Flag = 1"
                        rdrPay = CType(objDCom.Execute(strSQL, CommandType.Text, CommandBehavior.CloseConnection, False), SqlClient.SqlDataReader)
                        If rdrPay Is Nothing = False Then
                            While rdrPay.Read
                                drRow = dtPay.NewRow
                                drRow("TenderID") = Convert.ToString(rdrPay("TenderID"))
                                drRow("TenderDescription") = Convert.ToString(rdrPay("TenderDesc"))
                                drRow("TenderType") = Convert.ToInt32(rdrPay("TenderType"))
                                drRow("OpenDrawer") = Convert.ToInt32(rdrPay("OpenDrawer"))
                                drRow("AllowOverTender") = Convert.ToInt32(rdrPay("AllowOverTender"))
                                drRow("TrackRefNo") = Convert.ToInt32(rdrPay("TrackRefNo"))
                                drRow("TrackClrDate") = Convert.ToInt32(rdrPay("TrackClrDate"))
                                dtPay.Rows.Add(drRow)
                            End While

                            drRow = Nothing
                            rdrPay.Close()
                        End If


                    Case EnumTenderSelection.ExcCreditPrepaidIncCN
                        strSQL = "SELECT TenderID, TenderDesc, TenderType, OpenDrawer, AllowOverTender, TrackRefNo, TrackClrDate FROM Tender WHERE TenderID = '000'"
                        rdrPay = CType(objDCom.Execute(strSQL, CommandType.Text, CommandBehavior.SingleRow, False), SqlClient.SqlDataReader)
                        If rdrPay Is Nothing = False Then
                            If rdrPay.Read Then
                                drRow = dtPay.NewRow
                                drRow("TenderID") = Convert.ToString(rdrPay("TenderID"))
                                drRow("TenderDescription") = Convert.ToString(rdrPay("TenderDesc"))
                                drRow("TenderType") = Convert.ToInt32(rdrPay("TenderType"))
                                drRow("OpenDrawer") = Convert.ToInt32(rdrPay("OpenDrawer"))
                                drRow("AllowOverTender") = Convert.ToInt32(rdrPay("AllowOverTender"))
                                drRow("TrackRefNo") = Convert.ToInt32(rdrPay("TrackRefNo"))
                                drRow("TrackClrDate") = Convert.ToInt32(rdrPay("TrackClrDate"))

                                dtPay.Rows.Add(drRow)
                                drRow = Nothing

                            End If
                            rdrPay.Close()
                        End If


                        strSQL = "SELECT TenderID, TenderDesc, TenderType, OpenDrawer, AllowOverTender, TrackRefNo, TrackClrDate FROM TENDER WHERE TenderType NOT IN (0, 5, 6) AND Active = 1 AND Flag = 1"
                        rdrPay = CType(objDCom.Execute(strSQL, CommandType.Text, CommandBehavior.CloseConnection, False), SqlClient.SqlDataReader)
                        If rdrPay Is Nothing = False Then
                            While rdrPay.Read
                                drRow = dtPay.NewRow
                                drRow("TenderID") = Convert.ToString(rdrPay("TenderID"))
                                drRow("TenderDescription") = Convert.ToString(rdrPay("TenderDesc"))
                                drRow("TenderType") = Convert.ToInt32(rdrPay("TenderType"))
                                drRow("OpenDrawer") = Convert.ToInt32(rdrPay("OpenDrawer"))
                                drRow("AllowOverTender") = Convert.ToInt32(rdrPay("AllowOverTender"))
                                drRow("TrackRefNo") = Convert.ToInt32(rdrPay("TrackRefNo"))
                                drRow("TrackClrDate") = Convert.ToInt32(rdrPay("TrackClrDate"))
                                dtPay.Rows.Add(drRow)
                            End While

                            drRow = Nothing
                            rdrPay.Close()
                        End If

                End Select

                Return dtPay
            Catch ex As Exception

            Finally
                rdrPay = Nothing
                EndConnection()
            End Try
        End Function


#End Region

        Function ClearDB() As Boolean
        End Function
    End Class

#Region "TenderAdjust Base"
    Public Enum enumTenderAdjust
        TenderAdjustHDR = 0
        TenderAdjustDTL = 1
    End Enum
    Public Class TenderAdjustBase
        Inherits Core.CoreBase
        Dim TenderDtl As StrucClassInfo

#Region "TenderAdjust class in TenderAdjust Base"
        Public Class TenderAdjust
            Inherits Core.SingleBase
            Protected strDocCode, strBranchID, strUpdateBy, strTenderID As String
            Protected intFlag As Integer
            Protected dtAdjustDate, dtLastUpdate As DateTime
            Protected dtItem As DataTable
            Public MyScheme As New MyScheme

            Public Property BranchID() As String
                Get
                    Return strBranchID
                End Get
                Set(ByVal Value As String)
                    strBranchID = Value
                End Set
            End Property

            Public Property AdjustDate() As DateTime
                Get
                    Return dtAdjustDate
                End Get
                Set(ByVal Value As DateTime)
                    dtAdjustDate = Value
                End Set
            End Property
            Public Property LastUpdate() As DateTime
                Get
                    Return dtLastUpdate
                End Get
                Set(ByVal Value As DateTime)
                    If Value = Nothing Then
                        dtLastUpdate = Now()
                    Else
                        dtLastUpdate = Value
                    End If
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return strUpdateBy
                End Get
                Set(ByVal Value As String)
                    strUpdateBy = Value
                End Set
            End Property

            Public Property DocCode() As String
                Get
                    Return strDocCode
                End Get
                Set(ByVal Value As String)
                    strDocCode = Value
                End Set
            End Property

            Public Property Flag() As Integer
                Get
                    Return intFlag
                End Get
                Set(ByVal Value As Integer)
                    intFlag = Value
                End Set
            End Property

            Public Property Item() As DataTable
                Get
                    Return dtItem
                End Get
                Set(ByVal Value As DataTable)
                    dtItem = Value
                End Set
            End Property
            Public Property TenderID() As String
                Get
                    Return strTenderID
                End Get
                Set(ByVal Value As String)
                    strTenderID = Value
                End Set
            End Property
        End Class
#End Region

#Region "TenderAdjust Scheme"
        Public Class MyScheme
            Inherits Core.SchemeBase
            Protected Overrides Sub InitializeInfo()
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "TransNo"
                    .Length = 20
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(0, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "AdjustAmt"
                    .Length = 9
                    .DecPlace = 2
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(1, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "Remark"
                    .Length = 200
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(2, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "RefInfo"
                    .Length = 70
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(3, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "Reason"
                    .Length = 200
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(4, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "DocCode"
                    .Length = 20
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(5, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "TenderID"
                    .Length = 20
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(6, this)
            End Sub

            Public ReadOnly Property TransNo() As StrucElement
                Get
                    Return MyBase.GetItem(0)
                End Get
            End Property
            Public ReadOnly Property AdjustAmt() As StrucElement
                Get
                    Return MyBase.GetItem(1)
                End Get
            End Property
            Public ReadOnly Property Remark() As StrucElement
                Get
                    Return MyBase.GetItem(2)
                End Get
            End Property
            Public ReadOnly Property RefInfo() As StrucElement
                Get
                    Return MyBase.GetItem(3)
                End Get
            End Property
            Public ReadOnly Property Reason() As StrucElement
                Get
                    Return MyBase.GetItem(4)
                End Get
            End Property
            Public ReadOnly Property DocCode() As StrucElement
                Get
                    Return MyBase.GetItem(5)
                End Get
            End Property
            Public ReadOnly Property TenderID() As StrucElement
                Get
                    Return MyBase.GetItem(6)
                End Get
            End Property
            Public Function GetElement(ByVal Key As Integer) As StrucElement
                Return MyBase.GetItem(Key)
            End Function

        End Class
#End Region

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "BranchID, DocCode, AdjustDate, CreateDate, CreateBy, LastUpdate, UpdateBy, Flag, Posted , TenderID ,"
                .CheckFields = "FLAG"
                .TableName = "TenderAdjustHDR"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "BranchID, DocCode, AdjustDate, CreateDate, CreateBy, LastUpdate, UpdateBy, Flag, Posted ,TenderID ,"
                .ListingCond = "FLAG = 1"
                .ShortList = String.Empty
                .ShortListCond = String.Empty
            End With
            With TenderDtl
                .FieldsList = "DocCode, TransNo, AdjustType, AdjustAmt,RefInfo, Remark, Reason, TenderID ,"
                .CheckFields = "FLAG"
                .TableName = "TenderAdjustDTL"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "DocCode, TransNo, AdjustType, AdjustAmt,RefInfo, Remark, Reason, TenderID,"
                .ListingCond = "FLAG = 1"
                .ShortList = String.Empty
                .ShortListCond = String.Empty
            End With
            ReDim MyFields(1)
            MyBase.AddMyField(0, "Branch ID", "BranchID", TypeCode.String)
            MyBase.AddMyField(1, "Trans No", "TransNo", TypeCode.String)
        End Sub

#Region "Data Manipulation-Add,Edit,Del"

        Private Function AssignItem(ByVal objTenderAdjust As TenderAdjust, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim arrSQL As ArrayList
            Dim strSQL As String
            Dim i As Integer
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Dim objSysCodeBS As General.SysCodeBase
            AssignItem = False
            Try
                If objTenderAdjust Is Nothing Then
                    'msg return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        arrSQL = New ArrayList
                        If objTenderAdjust.DocCode.ToString = String.Empty And pType = SQLControl.EnumSQLType.stInsert Then
                            objSysCodeBS = New General.SysCodeBase
                            objTenderAdjust.DocCode = objSysCodeBS.GenerateSalesAdjCode(objTenderAdjust.BranchID, Date.Today)
                        End If
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "DocCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTenderAdjust.DocCode) & _
                                    "' AND BranchID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTenderAdjust.BranchID) & "'")
                        End With

                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        blnExec = True
                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("Flag")) = 0 Then
                                        'found but deleted
                                        blnFlag = False
                                    Else
                                        'found and active
                                        blnFlag = True
                                    End If
                                    strSQL = "DELETE FROM TENDERADJUSTDTL WHERE DocCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTenderAdjust.DocCode) & "'"
                                    arrSQL.Add(strSQL)
                                End If
                                .Close()
                            End With
                        End If
                    End If

                    If blnExec Then 'if executed
                        If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = MyInfo.TableName
                                .AddField("AdjustDate", objTenderAdjust.AdjustDate.ToString, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNull)
                                .AddField("LastUpdate", objTenderAdjust.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cNull)
                                .AddField("UpdateBy", objTenderAdjust.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Flag", cFlagActive.ToString, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert 'INSERT
                                        'record found but not deleted
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE DocCode = '" & .ParseValue(SQLControl.EnumDataType.dtString, objTenderAdjust.DocCode) & _
                                            "' AND BranchID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTenderAdjust.BranchID) & "'")
                                        Else
                                            If blnFound = False Then 'if record not found
                                                .AddField("BranchID", objTenderAdjust.BranchID.ToString, SQLControl.EnumDataType.dtString)
                                                .AddField("DocCode", objTenderAdjust.DocCode, SQLControl.EnumDataType.dtString)
                                                .AddField("CreateDate", objTenderAdjust.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                                .AddField("CreateBy", objTenderAdjust.UpdateBy, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate 'UPDATE
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE DocCode='" & .ParseValue(SQLControl.EnumDataType.dtString, objTenderAdjust.DocCode) & _
                                        "' AND BranchID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTenderAdjust.BranchID) & "'")
                                End Select
                                arrSQL.Add(strSQL)
                            End With

                            With objSQL
                                If objTenderAdjust.Item Is Nothing = False Then
                                    If objTenderAdjust.Item.Rows.Count > 0 Then
                                        .TableName = "TENDERADJUSTDTL"
                                        For i = 0 To objTenderAdjust.Item.Rows.Count - 1
                                            .AddField("DocCode", objTenderAdjust.DocCode, SQLControl.EnumDataType.dtString)
                                            .AddField("TransNo", Convert.ToString(objTenderAdjust.Item.Rows(i).Item("TransNo")), SQLControl.EnumDataType.dtString)
                                            .AddField("TenderID", Convert.ToString(objTenderAdjust.Item.Rows(i).Item("TenderID")), SQLControl.EnumDataType.dtString)
                                            .AddField("AdjustType", Convert.ToString(objTenderAdjust.Item.Rows(i).Item("AdjustType")), SQLControl.EnumDataType.dtString)
                                            .AddField("AdjustAmt", Convert.ToString(objTenderAdjust.Item.Rows(i).Item("AdjustAmt")), SQLControl.EnumDataType.dtNumeric)
                                            .AddField("RefInfo", Convert.ToString(objTenderAdjust.Item.Rows(i).Item("RefInfo")), SQLControl.EnumDataType.dtStringN)
                                            .AddField("Remark", Convert.ToString(objTenderAdjust.Item.Rows(i).Item("Remark")), SQLControl.EnumDataType.dtStringN)
                                            .AddField("Reason", Convert.ToString(objTenderAdjust.Item.Rows(i).Item("Reason")), SQLControl.EnumDataType.dtStringN)
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            arrSQL.Add(strSQL)
                                        Next
                                    End If
                                End If
                            End With
                            Try
                                'execute
                                objDCom.BatchExecute(arrSQL, CommandType.Text)
                                Return True
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002")
                                Else
                                    Throw New ApplicationException("210004")
                                End If
                            Finally
                                objSQL.Dispose()
                            End Try
                        End If
                    End If
                End If

            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objTenderAdjust = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try

        End Function
        'ADD
        Function Add(ByVal objTenderAdjust As TenderAdjust) As Boolean
            Return AssignItem(objTenderAdjust, SQLControl.EnumSQLType.stInsert)
        End Function
        'AMEND
        Function Amend(ByVal objTenderAdjust As TenderAdjust) As Boolean
            Return AssignItem(objTenderAdjust, SQLControl.EnumSQLType.stUpdate)
        End Function
        'DELETE
        Function Delete(ByVal objTenderAdjust As TenderAdjust) As Boolean
            Dim strSQL As String
            Dim arySQL As New ArrayList
            Dim blnFound As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            Try
                If objTenderAdjust Is Nothing Then
                    'error message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "DocCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTenderAdjust.DocCode) & _
                            "' AND BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTenderAdjust.BranchID) & "'")

                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                End If
                                .Close()
                            End With
                        End If

                        'record exist
                        If blnFound = True Then
                            strSQL = BuildDelete(MyInfo.TableName, "BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTenderAdjust.BranchID) & _
                                    "' AND DocCode ='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTenderAdjust.DocCode) & "'")
                            arySQL.Add(strSQL)
                            strSQL = BuildDelete(TenderDtl.TableName, "DocCode ='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTenderAdjust.DocCode) & "'")
                            arySQL.Add(strSQL)
                        End If

                        Try
                            'execute
                            objDCom.BatchExecute(arySQL, CommandType.Text, True)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try

                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As Exception
                Throw exDelete
            Finally
                objTenderAdjust = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        'Enquiry
        'Function Enquiry(Optional ByVal SQLstmt As String = Nothing) As DataSet
        '    If StartConnection() = True Then
        '        With MyInfo
        '            If SQLstmt = Nothing Or SQLstmt = String.Empty Then
        '                strSQL = BuildSelect(.FieldsList, .TableName)
        '            Else
        '                strSQL = SQLstmt
        '            End If
        '            Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
        '        End With
        '    Else
        '        Return Nothing
        '    End If
        'End Function
        Function Enquiry(ByVal enumTenderAdjustType As enumTenderAdjust, Optional ByVal BranchID As String = "", Optional ByVal strDocCode As String = Nothing, Optional ByVal Status As Integer = 0) As DataSet
            If StartConnection() = True Then
                StartSQLControl()
                With MyInfo
                    Select Case enumTenderAdjustType
                        Case enumTenderAdjust.TenderAdjustHDR
                            strSQL = "SELECT 0 as Selected, BranchID, DocCode, AdjustDate, CreateDate, CreateBy, LastUpdate, UpdateBy, PostedDate, Posted, Flag FROM TENDERADJUSTHDR WHERE BranchID = '" & BranchID & "' AND Posted = '" & Status & "'"
                        Case enumTenderAdjust.TenderAdjustDTL
                            strSQL = "SELECT TransNo, TenderID, AdjustType, AdjustAmt, RefInfo, Remark, Reason FROM TENDERADJUSTDTL WHERE DocCode = '" & strDocCode & "'"
                    End Select
                    If strSQL Is Nothing = False Then
                        Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                    End If
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Function GetListing() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetShortList() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function RecordSearch(ByVal BranchID As String, ByVal TransNo As String) As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Dim arrSQL As ArrayList
            Dim strSQL As String
            Dim blnFound As Boolean
            'q
            Try
                If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                    StartSQLControl()
                    With MyInfo
                        strSQL = BuildSelect("TransNo", "TRANSHDR", "BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BranchID.Trim) & _
                                "' AND TransNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, TransNo.Trim) & "' AND  Training = 0 AND TransStatus = 1")
                    End With

                    rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                    If rdr Is Nothing = False Then
                        With rdr
                            If .Read Then
                                blnFound = True
                            Else
                                blnFound = False
                            End If
                            .Close()
                        End With
                    End If
                End If
                Return blnFound
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try


        End Function

        Public Function GetTotalTenderAmt(ByVal BranchID As String, ByVal TransNo As String) As Double
            Dim strSQL As String
            Dim dblTotalAmt As Double = 0
            Dim rdr As SqlClient.SqlDataReader
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    strSQL = "SELECT SUM(TenderAmt) AS TotalTenderAmt FROM TRANSTENDER WHERE BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BranchID) & "' " & _
                            "AND TransNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, TransNo) & "'"
                    rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                    If rdr Is Nothing = False Then
                        With rdr
                            If .Read Then
                                If IsNumeric(.Item("TotalTenderAmt")) Then
                                    dblTotalAmt = Convert.ToDouble(.Item("TotalTenderAmt"))
                                Else
                                    dblTotalAmt = 0
                                End If
                            End If
                            .Close()
                        End With
                    End If
                End If
                Return dblTotalAmt
            Catch ex As Exception
                Return 0
            Finally
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try

        End Function
#End Region

#Region "Stock Posting"

        Public Function SalesAdjustPosting(ByVal DocCode As String, ByVal Branch As String) As Boolean
            Dim strSQL As String
            Try
                If StartConnection() = True Then
                    strSQL = "Update TENDERADJUSTHDR SET Posted = 1, PostedDate = getDate() Where DocCode IN (" & DocCode & ") AND BranchID = '" & Branch & "'"
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                    Return True
                Else
                    Return False
                End If
            Catch errSalesAdjustPosting As Exception
                Throw errSalesAdjustPosting
            Finally
                EndConnection()
            End Try
        End Function
#End Region


    End Class
#End Region

#End Region

#Region "Currency Base"
    Public Class CurrencyBase
        Inherits Core.CoreBase

#Region "Currency class in Currency Base"
        Public Class Currency
            Protected strCurrCode, strCurrDesc, strCurrSymb, strUpdBy As String
            Protected dblRate As Double
            Protected intUnit, intActive As Integer
            Protected dtLastUpd As DateTime

            Public Property CurrencyCode() As String
                Get
                    CurrencyCode = strCurrCode
                End Get
                Set(ByVal Value As String)
                    strCurrCode = Value
                End Set
            End Property

            Public Property CurrencyDesc() As String
                Get
                    CurrencyDesc = strCurrDesc
                End Get
                Set(ByVal Value As String)
                    strCurrDesc = Value
                End Set
            End Property

            Public Property CurrencySymbol() As String
                Get
                    CurrencySymbol = strCurrSymb
                End Get
                Set(ByVal Value As String)
                    strCurrSymb = Value
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    UpdateBy = strUpdBy
                End Get
                Set(ByVal Value As String)
                    strUpdBy = Value
                End Set
            End Property

            Public Property ExchangeRate() As Double
                Get
                    ExchangeRate = dblRate
                End Get
                Set(ByVal Value As Double)
                    dblRate = Value
                End Set
            End Property

            Public Property Unit() As Integer
                Get
                    Unit = intUnit
                End Get
                Set(ByVal Value As Integer)
                    intUnit = Value
                End Set
            End Property

            Public Property Active() As Integer
                Get
                    Active = intActive
                End Get
                Set(ByVal Value As Integer)
                    intActive = Value
                End Set
            End Property

            Public Property LastUpdate() As DateTime
                Get
                    LastUpdate = dtLastUpd
                End Get
                Set(ByVal Value As DateTime)
                    dtLastUpd = Value
                End Set
            End Property

        End Class
#End Region

#Region "Currency Scheme"
        Public Class MyScheme
            Inherits Core.SchemeBase

            Protected Overrides Sub InitializeInfo()
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "CurrencyCode"
                    .Length = 3
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(0, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "CurrencyDesc"
                    .Length = 50
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(1, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "Rate"
                    .Length = 4
                    .DecPlace = 4
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(2, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "Unit"
                    .Length = 5
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(3, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "CurrencySymbol"
                    .Length = 10
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(4, this)

            End Sub

            Public ReadOnly Property Code() As StrucElement
                Get
                    Return MyBase.GetItem(0)
                End Get
            End Property

            Public ReadOnly Property Description() As StrucElement
                Get
                    Return MyBase.GetItem(1)
                End Get
            End Property

            Public ReadOnly Property Rate() As StrucElement
                Get
                    Return MyBase.GetItem(2)
                End Get
            End Property

            Public ReadOnly Property Unit() As StrucElement
                Get
                    Return MyBase.GetItem(3)
                End Get
            End Property

            Public ReadOnly Property Symbol() As StrucElement
                Get
                    Return MyBase.GetItem(4)
                End Get
            End Property

            Public Function GetElement(ByVal Key As Integer) As StrucElement
                Return MyBase.GetItem(Key)
            End Function

        End Class

#End Region

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "CURRENCYCODE, CURRENCYDESC, RATE, UNIT, CURRENCYSYMBOL, LASTUPDATE, UPDATEBY, ACTIVE, INUSE, FLAG"
                .CheckFields = "INUSE, FLAG"
                .TableName = "CURRENCY"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "CURRENCYCODE, CURRENCYDESC, RATE, UNIT, CURRENCYSYMBOL, LASTUPDATE, UPDATEBY, ACTIVE, INUSE, FLAG"
                .ListingCond = "FLAG = 1"
                .ShortList = "CURRENCYCODE, CURRENCYDESC"
                .ShortListCond = String.Empty
            End With
            ReDim MyFields(1)
            MyBase.AddMyField(0, "Currency Code", "CurrencyCode", TypeCode.String)
            MyBase.AddMyField(1, "Currency Desc", "CurrencyDesc", TypeCode.String)
        End Sub

#Region "Data Manipulation-Add,Edit,Del"

        Private Function AssignItem(ByVal objCurrency As Currency, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As SqlClient.SqlDataReader
            AssignItem = False
            Try
                If objCurrency Is Nothing Then
                    'msg return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False

                    If blnConnected Then
                        If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                            StartSQLControl()
                            With MyInfo
                                strSQL = BuildSelect(.CheckFields, .TableName, "CurrencyCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCurrency.CurrencyCode) & "'")
                            End With
                            rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                            blnExec = True 'executed - select
                            If rdr Is Nothing = False Then
                                With rdr
                                    If .Read Then
                                        'record is found
                                        blnFound = True
                                        If Convert.ToInt16(.Item("Flag")) = 0 Then
                                            'found but deleted
                                            blnFlag = False
                                        Else
                                            'found and active
                                            blnFlag = True
                                        End If
                                    End If
                                    .Close()
                                End With
                            End If
                        End If
                    End If

                    If blnExec Then 'if executed
                        If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = "CURRENCY"
                                .AddField("CurrencyDesc", objCurrency.CurrencyDesc, SQLControl.EnumDataType.dtStringN)
                                .AddField("Rate", objCurrency.ExchangeRate.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Unit", objCurrency.Unit.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CurrencySymbol", objCurrency.CurrencySymbol, SQLControl.EnumDataType.dtStringN)
                                .AddField("UpdateBy", objCurrency.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", objCurrency.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                .AddField("Active", objCurrency.Active.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", cFlagActive.ToString, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert 'INSERT
                                        'record found but not deleted
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE CurrencyCode = '" & .ParseValue(SQLControl.EnumDataType.dtDateTime, objCurrency.CurrencyCode) & "'")
                                        Else
                                            If blnFound = False Then 'if record not found
                                                .AddField("CurrencyCode", objCurrency.CurrencyCode, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate 'UPDATE
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE CurrencyCode='" & .ParseValue(SQLControl.EnumDataType.dtString, objCurrency.CurrencyCode) & "'")
                                End Select
                            End With
                            Try
                                'execute
                                objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002")
                                Else
                                    Throw New ApplicationException("210004")
                                End If
                            Finally
                                objSQL.Dispose()
                            End Try
                            Return True
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objCurrency = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'Add
        Function Add(ByVal objcurrency As Currency) As Boolean
            Return AssignItem(objcurrency, SQLControl.EnumSQLType.stInsert)
        End Function

        'AMEND
        Function Amend(ByVal objcurrency As Currency) As Boolean
            Return AssignItem(objcurrency, SQLControl.EnumSQLType.stUpdate)
        End Function

        'DELETE
        Function Delete(ByVal objCurrency As Currency) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If objCurrency Is Nothing Then
                    'error message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            'strSQL = "SELECT CurrencyCode, Flag, InUse FROM CURRENCY WHERE CurrencyCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, objCurrency.CurrencyCode) & string.empty
                            strSQL = BuildSelect(.CheckFields, .TableName, "CurrencyCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCurrency.CurrencyCode) & String.Empty & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt32(.Item("InUse")) = 1 Then
                                        blnInUse = True 'if record is found and in use
                                    End If
                                End If
                                .Close()
                            End With
                            'record is found and in use
                            If blnFound = True And blnInUse = True Then
                                strSQL = BuildUpdate(MyInfo.TableName, " SET Flag = " & cFlagNonActive & _
                                " , LastUpdate = '" & objCurrency.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCurrency.UpdateBy) & "'" & _
                                " WHERE CurrencyCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCurrency.CurrencyCode) & "'")
                            End If

                            'record exist but not in use
                            If blnFound = True And blnInUse = False Then
                                strSQL = BuildDelete(MyInfo.TableName, "CurrencyCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCurrency.CurrencyCode) & "'")
                            End If
                            Try
                                'execute
                                objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                Return True
                            Catch exExecute As Exception
                                Throw New ApplicationException("210006")
                            End Try
                        End If
                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As SystemException
                Throw exDelete
            Finally
                objCurrency = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"

        'Enquiry
        Function Enquiry(Optional ByVal SQLstmt As String = Nothing) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    If SQLstmt = Nothing Or SQLstmt = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName)
                    Else
                        strSQL = SQLstmt
                    End If
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetListing() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetShortList() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function
#End Region

        Function ClearDB() As Boolean
        End Function
    End Class
#End Region

#Region "Country Base"
    Public Class CountryBase
        Inherits Core.CoreBase

        Public Enum EnumSelectType
            Country = 0
            State = 1
        End Enum
        Dim State As StrucClassInfo
#Region "Country class in Country Base"
        Public Class Country
            Inherits Core.SingleBase
            Protected strCode, strDesc, strUpdBy As String
            Protected intActive As Integer
            Protected dtLastUpd As DateTime
            Public MyScheme As New MyScheme

            Public Property Code() As String
                Get
                    Return strCode
                End Get
                Set(ByVal CountryCode As String)
                    strCode = CountryCode
                    MyBase.AddItem(0, strCode)
                End Set
            End Property

            Public Property Description() As String
                Get
                    Return strDesc
                End Get
                Set(ByVal Value As String)
                    strDesc = Value
                    MyBase.AddItem(1, strDesc)
                End Set
            End Property

            Public Property LastUpdate() As DateTime
                Get
                    Return dtLastUpd
                End Get
                Set(ByVal Value As DateTime)
                    If Value = Nothing Then
                        dtLastUpd = Now()
                    Else
                        dtLastUpd = Value
                    End If
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return strUpdBy
                End Get
                Set(ByVal Value As String)
                    strUpdBy = Value
                End Set
            End Property

            Public Property Active() As Integer
                Get
                    Return intActive
                End Get
                Set(ByVal Value As Integer)
                    If Value < 0 Then
                        intActive = 0
                    Else
                        intActive = Value
                    End If
                End Set
            End Property
        End Class
#End Region

#Region "State class in CountryState Base"
        Public Class CountryState
            Inherits Core.SingleBase
            Protected strCountryCode, strStateCode, strStateDesc, strUpdateBy As String
            Protected intActive As Integer
            Protected dtLastUpdate As DateTime
            Public MyScheme As New MyScheme

            Public Property CountryCode() As String
                Get
                    Return strCountryCode
                End Get
                Set(ByVal CountryCode As String)
                    strCountryCode = CountryCode
                    MyBase.AddItem(0, strCountryCode)
                End Set
            End Property

            Public Property StateCode() As String
                Get
                    Return strStateCode
                End Get
                Set(ByVal Value As String)
                    strStateCode = Value
                    MyBase.AddItem(1, strStateCode)
                End Set
            End Property

            Public Property StateDesc() As String
                Get
                    Return strStateDesc
                End Get
                Set(ByVal StateDesc As String)
                    strStateDesc = StateDesc
                    MyBase.AddItem(2, strStateCode)
                End Set
            End Property

            Public Property LastUpdate() As DateTime
                Get
                    Return dtLastUpdate
                End Get
                Set(ByVal Value As DateTime)
                    If Value = Nothing Then
                        dtLastUpdate = Now()
                    Else
                        dtLastUpdate = Value
                    End If
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return strUpdateBy
                End Get
                Set(ByVal Value As String)
                    strUpdateBy = Value
                End Set
            End Property

            Public Property Active() As Integer
                Get
                    Return intActive
                End Get
                Set(ByVal Value As Integer)
                    If Value < 0 Then
                        intActive = 0
                    Else
                        intActive = Value
                    End If
                End Set
            End Property
        End Class
#End Region

#Region "Country Scheme"
        Public Class MyScheme
            Inherits Core.SchemeBase

            Protected Overrides Sub InitializeInfo()
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "CountryCode"
                    .Length = 2
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(0, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "CountryDesc"
                    .Length = 50
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(1, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "StateCode"
                    .Length = 5
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(2, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "StateDesc"
                    .Length = 50
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(3, this)
            End Sub

            Public ReadOnly Property Code() As StrucElement
                Get
                    Return MyBase.GetItem(0)
                End Get
            End Property

            Public ReadOnly Property Description() As StrucElement
                Get
                    Return MyBase.GetItem(1)
                End Get
            End Property

            Public ReadOnly Property StateCode() As StrucElement
                Get
                    Return MyBase.GetItem(2)
                End Get
            End Property

            Public ReadOnly Property StateDesc() As StrucElement
                Get
                    Return MyBase.GetItem(3)
                End Get
            End Property

            Public Function GetElement(ByVal Key As Integer) As StrucElement
                Return MyBase.GetItem(Key)
            End Function

            'Public Property LastUpdate() As DateTime
            '    Get
            '        Return dtLastUpd
            '    End Get
            '    Set(ByVal Value As DateTime)
            '        dtLastUpd = Value
            '    End Set
            'End Property

            'Public Property UpdateBy() As String
            '    Get
            '        Return strUpdBy
            '    End Get
            '    Set(ByVal Value As String)
            '        strUpdBy = Value
            '    End Set
            'End Property

            'Public Property Active() As Integer
            '    Get
            '        Return intActive
            '    End Get
            '    Set(ByVal Value As Integer)
            '        intActive = Value
            '    End Set
            'End Property
        End Class

#End Region


        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "COUNTRYCODE, COUNTRYDESC, LASTUPDATE, UPDATEBY, ACTIVE, INUSE, FLAG"
                .CheckFields = "INUSE, FLAG"
                .TableName = "COUNTRY"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "COUNTRYCODE, COUNTRYDESC, LASTUPDATE, UPDATEBY, ACTIVE, INUSE, FLAG"
                .ListingCond = "FLAG = 1"
                .ShortList = String.Empty
                .ShortListCond = String.Empty
            End With
            With State
                .FieldsList = "COUNTRYCODE, STATECODE, STATEDESC, LASTUPDATE, UPDATEBY, ACTIVE, INUSE, FLAG"
                .CheckFields = "INUSE, FLAG"
                .TableName = "STATE"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "COUNTRYCODE, STATECODE, STATEDESC, LASTUPDATE, UPDATEBY, ACTIVE, INUSE, FLAG"
                .ListingCond = "ACTIVE = 1"
                .ShortList = String.Empty
                .ShortListCond = String.Empty
            End With
            ReDim MyFields(1)
            MyBase.AddMyField(0, "Country Code", "CountryCode", TypeCode.String)
            MyBase.AddMyField(1, "Country Desc", "CountryDesc", TypeCode.String)
        End Sub

#Region "Country Data Manipulation-Add,Edit,Del"

        Private Function AssignItem(ByVal objCountry As Country, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As SqlClient.SqlDataReader
            AssignItem = False
            Try
                If objCountry Is Nothing Then
                    'msg return
                Else
                    'Dim test As ArrayList
                    'test.Add("A")
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "CountryCode ='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCountry.Code) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        blnExec = True 'executed - select
                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    'record is found
                                    blnFound = True
                                    If Convert.ToInt16(.Item("Flag")) = 0 Then
                                        'found but deleted
                                        blnFlag = False
                                    Else
                                        'found and active
                                        blnFlag = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If
                    End If

                    If blnExec Then 'if executed
                        If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = MyInfo.TableName
                                .AddField("CountryDesc", objCountry.Description, SQLControl.EnumDataType.dtStringN)
                                .AddField("UpdateBy", objCountry.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", objCountry.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                .AddField("Active", objCountry.Active.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", cFlagActive.ToString, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert 'INSERT
                                        'record found but not deleted
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE CountryCode = '" & .ParseValue(SQLControl.EnumDataType.dtString, objCountry.Code) & "'")
                                        Else
                                            If blnFound = False Then 'if record not found
                                                .AddField("CountryCode", objCountry.Code, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate 'UPDATE
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE CountryCode='" & .ParseValue(SQLControl.EnumDataType.dtString, objCountry.Code) & "'")
                                End Select
                            End With
                            Try
                                'execute
                                objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                Return True
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002")
                                Else
                                    Throw New ApplicationException("210004")
                                End If
                            Finally
                                objSQL.Dispose()
                            End Try
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objCountry = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Function Add(ByVal objCountry As Country) As Boolean
            Return AssignItem(objCountry, SQLControl.EnumSQLType.stInsert)
        End Function

        'AMEND
        Function Amend(ByVal objCountry As Country) As Boolean
            Return AssignItem(objCountry, SQLControl.EnumSQLType.stUpdate)
        End Function

        'DELETE
        Function Delete(ByVal objCountry As Country) As Boolean
            Dim strSQL As String
            Dim arrsql As ArrayList
            Dim blnFound, blnInUse As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If objCountry Is Nothing Then
                    'error message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        arrsql = New ArrayList
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "CountryCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCountry.Code) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt32(.Item("InUse")) = 1 Then
                                        blnInUse = True 'if record is found and in use
                                    End If
                                End If
                                .Close()
                            End With
                        End If

                        'record is found and in use
                        If blnFound = True And blnInUse = True Then
                            strSQL = BuildUpdate(MyInfo.TableName, " SET Flag = " & cFlagNonActive & _
                                " , LastUpdate = '" & objCountry.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCountry.UpdateBy) & "'" & _
                                " WHERE CountryCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCountry.Code) & "'")
                            arrsql.Add(strSQL)
                            strSQL = BuildUpdate(State.TableName, " SET Flag = " & cFlagNonActive & _
                                    " , LastUpdate = '" & objCountry.LastUpdate & "' , UpdateBy = '" & _
                                    objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCountry.UpdateBy) & "'" & _
                                    " WHERE CountryCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCountry.Code) & "'")
                            arrsql.Add(strSQL)
                        End If

                        'record exist but not in use
                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(MyInfo.TableName, "CountryCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCountry.Code) & "'")
                            arrsql.Add(strSQL)
                            strSQL = BuildDelete(State.TableName, "CountryCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCountry.Code) & "'")
                            arrsql.Add(strSQL)
                        End If
                        Try
                            'execute
                            objDCom.BatchExecute(arrsql, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try
                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As Exception
                Throw exDelete
            Finally
                objCountry = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "State Data Manipulation-Add,Edit,Del"
        Private Function StateAssignItem(ByVal objCountryState As CountryState, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As SqlClient.SqlDataReader
            StateAssignItem = False
            Try
                If objCountryState Is Nothing Then
                    'msg return
                Else
                    'Dim test As ArrayList
                    'test.Add("A")
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With State
                            strSQL = BuildSelect(.CheckFields, .TableName, "CountryCode ='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCountryState.CountryCode) & _
                            "' AND StateCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCountryState.StateCode) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        blnExec = True 'executed - select
                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    'record is found
                                    blnFound = True
                                    If Convert.ToInt16(.Item("Flag")) = 0 Then
                                        'found but deleted
                                        blnFlag = False
                                    Else
                                        'found and active
                                        blnFlag = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If
                    End If

                    If blnExec Then 'if executed
                        If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = State.TableName
                                .AddField("StateDesc", objCountryState.StateDesc, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", objCountryState.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", objCountryState.UpdateBy.ToString, SQLControl.EnumDataType.dtString)
                                .AddField("Active", objCountryState.Active.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", cFlagActive.ToString, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert 'INSERT
                                        'record found but not deleted
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE CountryCode = '" & .ParseValue(SQLControl.EnumDataType.dtString, objCountryState.CountryCode) & _
                                            " AND StateCode = ' " & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCountryState.StateCode) & "'")
                                        Else
                                            If blnFound = False Then 'if record not found
                                                .AddField("CountryCode", objCountryState.CountryCode, SQLControl.EnumDataType.dtString)
                                                .AddField("StateCode", objCountryState.StateCode, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate 'UPDATE
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE CountryCode='" & .ParseValue(SQLControl.EnumDataType.dtString, objCountryState.CountryCode) & _
                                        " AND StateCode = ' " & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCountryState.StateCode) & "'")
                                End Select
                            End With
                            Try
                                'execute
                                objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                Return True
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002")
                                Else
                                    Throw New ApplicationException("210004")
                                End If
                            Finally
                                objSQL.Dispose()
                            End Try
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objCountryState = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Function Add(ByVal objCountryState As CountryState) As Boolean
            Return StateAssignItem(objCountryState, SQLControl.EnumSQLType.stInsert)
        End Function

        'DELETE
        Function Delete(ByVal objCountryState As CountryState) As Boolean

            Dim strSQL As String
            Dim blnFound, blnInUse As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If objCountryState Is Nothing Then
                    'error message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With State
                            strSQL = BuildSelect(.CheckFields, .TableName, "CountryCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCountryState.CountryCode) & _
                            "' AND StateCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCountryState.StateCode) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt32(.Item("InUse")) = 1 Then
                                        blnInUse = True 'if record is found and in use
                                    End If
                                End If
                                .Close()
                            End With
                        End If

                        'record is found and in use
                        If blnFound = True And blnInUse = True Then
                            strSQL = BuildUpdate(State.TableName, " SET Flag = " & cFlagNonActive & _
                                " , LastUpdate = '" & objCountryState.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCountryState.UpdateBy) & "'" & _
                                " WHERE CountryCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCountryState.CountryCode) & "' AND " & _
                                "StateCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCountryState.StateCode) & "'")
                        End If

                        'record exist but not in use
                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(State.TableName, "CountryCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCountryState.CountryCode) & "' AND " & _
                                "StateCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCountryState.StateCode) & "'")
                        End If

                        Try
                            'execute
                            objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try

                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As Exception
                Throw exDelete
            Finally
                objCountryState = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        'Enquiry
        Function Enquiry(Optional ByVal SQLstmt As String = Nothing) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    If SQLstmt = Nothing Or SQLstmt = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName)
                    Else
                        strSQL = SQLstmt
                    End If
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Function StatesEnquiry(ByVal CountryCode As String) As DataSet
            If StartConnection() = True Then
                With State
                    strSQL = BuildSelect(.FieldsList, .TableName, "CountryCode = '" & CountryCode & "'")
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetListing() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetShortList() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function
#End Region

        'Clear all data in database

    End Class
#End Region

#Region "Tax Base"
    Public Class TaxBase
        Inherits Core.CoreBase

        Public Enum EnumTaxable
            None = 0
            Exclusive = 1
            Inclusive = 2
        End Enum

        Public Enum EnumTaxType
            SubTotalInclusive = 1
            SubTotalExclusive = 2
            TotalInclusive = 3
            TotalExclusive = 4
            TotalExclusiveBeforeDiscount = 5
        End Enum

#Region "Tax class in TaxBase"
        Public Class Tax
            Protected strTaxCode, strTaxType, strDescription, strTaxInEx As String
            Protected dblTaxAmt, dblTaxPer As Double
            Protected intActive As Integer
            Protected dtLastUpd As DateTime
            Protected strUpdBy As String

            Public Property TaxCode() As String
                Get
                    Return strTaxCode
                End Get
                Set(ByVal Value As String)
                    If Value.Trim = String.Empty Then
                        Throw New Exception("Please key in the Tax Code.")
                    ElseIf Not IsNumeric(Value) Then
                        Throw New Exception("Tax Code must be numeric.")
                    Else
                        strTaxCode = Value
                    End If
                End Set
            End Property

            Public Property TaxType() As String
                Get
                    Return strTaxType
                End Get
                Set(ByVal Value As String)
                    If Value.TrimEnd = String.Empty Then
                        Throw New Exception("Please select the Tax Type.")
                    Else
                        strTaxType = Value
                    End If
                End Set
            End Property

            Public Property Description() As String
                Get
                    Return strDescription
                End Get
                Set(ByVal Value As String)
                    strDescription = Value
                End Set
            End Property

            Public Property TaxAmt() As Double
                Get
                    Return dblTaxAmt
                End Get
                Set(ByVal Value As Double)
                    dblTaxAmt = Value
                End Set
            End Property

            Public Property TaxPer() As Double
                Get
                    Return dblTaxPer
                End Get
                Set(ByVal Value As Double)
                    dblTaxPer = Value
                End Set
            End Property

            Public Property TaxInEx() As String
                Get
                    Return strTaxInEx
                End Get
                Set(ByVal Value As String)
                    strTaxInEx = Value
                End Set
            End Property

            Public Property LastUpdate() As DateTime
                Get
                    Return dtLastUpd
                End Get
                Set(ByVal Value As DateTime)
                    dtLastUpd = Value
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return strUpdBy
                End Get
                Set(ByVal Value As String)
                    strUpdBy = Value
                End Set
            End Property

            Public Property Active() As Integer
                Get
                    Return intActive
                End Get
                Set(ByVal Value As Integer)
                    intActive = Value
                End Set
            End Property

        End Class
#End Region

#Region "Tax Scheme"
        Public Class MyScheme
            Inherits SEAL.Model.Moyenne.SchemeBase

            Protected Overrides Sub InitializeInfo()
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "TaxCode"
                    .Length = 4
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(0, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "TaxDesc"
                    .Length = 50
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(1, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "TaxAmt"
                    .Length = 10
                    .DecPlace = 2
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(2, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "TaxRate"
                    .Length = 4
                    .DecPlace = 4
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(3, this)
            End Sub

            Public ReadOnly Property Code() As StrucElement
                Get
                    Return MyBase.GetItem(0)
                End Get
            End Property

            Public ReadOnly Property Description() As StrucElement
                Get
                    Return MyBase.GetItem(1)
                End Get
            End Property

            Public ReadOnly Property Amount() As StrucElement
                Get
                    Return MyBase.GetItem(2)
                End Get
            End Property

            Public ReadOnly Property Rate() As StrucElement
                Get
                    Return MyBase.GetItem(3)
                End Get
            End Property

            Public Function GetElement(ByVal Key As Integer) As StrucElement
                Return MyBase.GetItem(Key)
            End Function

        End Class

#End Region

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "TAXCODE, TAXGROUP, TAXDESC, TAXAMT, TAXRATE, TAXINEX, LASTUPDATE, UPDATEBY, ACTIVE, INUSE, FLAG"
                .CheckFields = "INUSE, FLAG"
                .TableName = "Tax"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "TAXCODE, TAXGROUP, TAXDESC, TAXAMT, TAXRATE, TAXINEX, LASTUPDATE, UPDATEBY, ACTIVE, INUSE, FLAG"
                .ListingCond = "FLAG = 1"
                .ShortList = "TAXCODE, TAXDESC, TAXAMT, TAXRATE"
                .ShortListCond = "FLAG = 1 AND ACTIVE = 1"
            End With
            ReDim MyFields(1)
            MyBase.AddMyField(0, "Tax Code", "TaxCode", TypeCode.String)
            MyBase.AddMyField(1, "Tax Desc", "TaxDesc", TypeCode.String)
        End Sub

#Region "Data Manipulation-Add,Edit,Del"

        Private Function AssignItem(ByVal objTax As Tax, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As SqlClient.SqlDataReader
            AssignItem = False
            Try
                If objTax Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If blnConnected Then
                        If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                            StartSQLControl()
                            With MyInfo
                                'strSQL = "SELECT TaxCode, TaxGroup, TaxDesc, TaxAmt, TaxRate, TaxInEx, Active, Flag, InUse FROM TAX WHERE TaxCode = '" & _
                                '   objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, objTax.TaxCode) & "'"
                                strSQL = BuildSelect(.CheckFields, .TableName, "TaxCode ='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTax.TaxCode) & "'")
                            End With
                            rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                            blnExec = True

                            If rdr Is Nothing = False Then
                                With rdr
                                    If .Read Then
                                        blnFound = True
                                        If Convert.ToInt16(.Item("Flag")) = 0 Then
                                            blnFlag = False 'Found but deleted
                                        Else
                                            blnFlag = True  'Found and active
                                        End If
                                    End If
                                    .Close()
                                End With
                            End If
                        End If
                    End If

                    If blnExec Then
                        If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                            'Item found & active
                            'system.Windows.Forms.MessageBox.Show("ab", "ABC",Windows.Forms.MessageBoxButtons.YesNo, Windows.Forms.MessageBoxIcon.Information,Windows.Forms.MessageBoxDefaultButton.Button2, Windows.Forms.MessageBoxOptions.DefaultDesktopOnly
                            'MsgBox("Record already exist", MsgBoxStyle.Critical, "Record Exist")
                            Return False
                        Else
                            With objSQL
                                .TableName = "TAX"
                                .AddField("TaxGroup", objTax.TaxType, SQLControl.EnumDataType.dtString)
                                .AddField("TaxDesc", objTax.Description, SQLControl.EnumDataType.dtStringN)
                                .AddField("TaxAmt", objTax.TaxAmt.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("TaxRate", objTax.TaxPer.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("TaxInEx", objTax.TaxInEx, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LastUpdate", objTax.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", objTax.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", objTax.Active.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", cFlagActive.ToString, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                            "WHERE TaxCode ='" & .ParseValue(SQLControl.EnumDataType.dtNumeric, objTax.TaxCode) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("Taxcode", objTax.TaxCode, SQLControl.EnumDataType.dtNumeric)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                             "WHERE TaxCode ='" & .ParseValue(SQLControl.EnumDataType.dtNumeric, objTax.TaxCode) & "'")
                                End Select
                            End With
                            Try
                                'execute 
                                objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002")
                                Else
                                    Throw New ApplicationException("210004")
                                End If
                            Finally
                                objSQL.Dispose()
                            End Try
                            Return True
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objTax = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
        'Add
        Function Add(ByVal objTax As Tax) As Boolean
            Return AssignItem(objTax, SQLControl.EnumSQLType.stInsert)
        End Function
        'Amend
        Function Amend(ByVal objTax As Tax) As Boolean
            Return AssignItem(objTax, SQLControl.EnumSQLType.stUpdate)
        End Function
        'Delete
        Function Delete(ByVal objTax As Tax) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If objTax Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            'strSQL = "SELECT TaxCode, TaxGroup, TaxDesc, TaxAmt, TaxRate, TaxInEx, Flag, InUse FROM TAX WHERE TaxCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, objTax.TaxCode) & "'"
                            strSQL = BuildSelect(.CheckFields, .TableName, "TaxCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, objTax.TaxCode) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("InUse")) = 1 Then
                                        blnInUse = True
                                    End If
                                End If
                                .Close()
                            End With

                            If blnFound = False Then
                                'Error Message
                                Return False
                            End If
                            If blnFound = True And blnInUse = True Then
                                'With objSQL
                                '.TableName = "TAX"
                                '.AddField("UpdateBy", objTax.UpdateBy, SQLControl.EnumDataType.dtString)
                                '.AddField("LastUpdate", objTax.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                '.AddField("Flag", cFlagNonActive.ToString, SQLControl.EnumDataType.dtNumeric)
                                'strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                '    "WHERE TaxCode ='" & .ParseValue(SQLControl.EnumDataType.dtNumeric, objTax.TaxCode) & "'")
                                'End With
                                strSQL = BuildUpdate(MyInfo.TableName, " SET Flag = " & cFlagNonActive & _
                                " , LastUpdate = '" & objTax.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTax.UpdateBy) & "'" & _
                                " WHERE TaxCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTax.TaxCode) & "'")
                            End If
                            If blnFound = True And blnInUse = False Then
                                'strSQL = "DELETE FROM TAX WHERE TaxCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, objTax.TaxCode) & "'"
                                strSQL = BuildDelete(MyInfo.TableName, "TaxCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTax.TaxCode) & "'")
                            End If
                            Try
                                'execute
                                objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                Return True
                            Catch exExecute As Exception
                                Throw New ApplicationException("210006")
                            End Try
                        End If
                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As SystemException
                Throw exDelete
            Finally
                objTax = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        'Enquiry
        Function Enquiry(Optional ByVal SQLstmt As String = Nothing) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    If SQLstmt = Nothing Or SQLstmt = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName)
                    Else
                        strSQL = SQLstmt
                    End If
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetListing() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetNotTransTax(ByVal BranchID As String, ByVal DocCode As String) As DataSet
            If StartConnection() = True Then
                StartConnection()
                strSQL = "SELECT TaxCode, TaxDesc, TaxAmt, TaxRate FROM Tax " & _
                         "WHERE TaxCode NOT IN(SELECT TaxCode FROM STKTRANSTAX " & _
                         "WHERE BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BranchID) & "' " & _
                         "AND DocCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, DocCode) & "')"
                Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, "Tax"), DataSet)
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetShortList() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Shared Function GetInfoByTaxGroup(ByVal TaxGroup As String) As DataTable
            Try
                StartSQLControl()
                StartConnection()

                strSQL = "SELECT TaxCode, TaxGroup, TaxDesc, TaxAmt, TaxRate, TaxInEx FROM Tax " & _
                        "WHERE Active = 1 AND Flag = 1 AND TaxGroup = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, TaxGroup) & "'"

                Return objDCom.Execute(strSQL, CommandType.Text, False)
            Catch ex As Exception
                Return Nothing
            Finally
                EndConnection()
                EndSQLControl()
            End Try

        End Function
#End Region

        Function ClearDB() As Boolean
        End Function
    End Class
#End Region

#Region "CodeMasterBase Base"
    Public Class CodeMasterBase
        Inherits Core.CoreBase

#Region "CodeMaster class in CodeMasterBase"
        Public Class CodeMaster
            Protected strCode, strCodeType, strDescription As String

            Protected intCodeSeq As Integer


            Public Property Code() As String
                Get
                    Return strCode
                End Get
                Set(ByVal Value As String)
                    If Value.Trim = String.Empty Then
                        Throw New Exception("Please key in the Code.")
                    Else
                        strCode = Value
                    End If
                End Set
            End Property

            Public Property CodeType() As String
                Get
                    Return strCodeType
                End Get
                Set(ByVal Value As String)

                    strCodeType = Value

                End Set
            End Property

            Public Property Description() As String
                Get
                    Return strDescription
                End Get
                Set(ByVal Value As String)
                    strDescription = Value
                End Set
            End Property


            Public Property CodeSequence() As Integer
                Get
                    Return intCodeSeq
                End Get
                Set(ByVal Value As Integer)
                    intCodeSeq = Value
                End Set
            End Property

        End Class
#End Region

#Region "Stock Department Scheme"
        Public Class MyScheme
            Inherits Core.SchemeBase

            Protected Overrides Sub InitializeInfo()
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "Code"
                    .Length = 10
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(0, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "Description"
                    .Length = 50
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(1, this)
            End Sub

            Public ReadOnly Property Code() As StrucElement
                Get
                    Return MyBase.GetItem(0)
                End Get
            End Property

            Public ReadOnly Property Description() As StrucElement
                Get
                    Return MyBase.GetItem(1)
                End Get
            End Property

            Public Function GetElement(ByVal Key As Integer) As StrucElement
                Return MyBase.GetItem(Key)
            End Function

        End Class

#End Region

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "Code, CodeDesc, CodeType, CodeSeq, SysCode"
                .CheckFields = "SysCode"
                .TableName = "CODEMASTER"
                .DefaultCond = String.Empty
                .DefaultOrder = "CodeSeq ASC"
                .Listing = "Code, CodeDesc, CodeSeq, SysCode"
                .ListingCond = String.Empty
                .ShortList = "Code, CodeDesc"
                .ShortListCond = String.Empty
            End With
            ReDim MyFields(1)
            MyBase.AddMyField(0, "Code", "Code", TypeCode.String)
            MyBase.AddMyField(1, "Desc", "CodeDesc", TypeCode.String)
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function AssignItem(ByVal objCode As CodeMaster, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim blnExec As Boolean, blnFound As Boolean
            Dim rdr As SqlClient.SqlDataReader
            AssignItem = False
            Try
                If objCode Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False

                    If StartConnection(Core.CoreBase.EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With myinfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "Code = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCode.Code) & "' AND CodeType='" & _
                                    objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCode.CodeType) & "' ")
                        End With
                        'strSQL = "SELECT Code, CodeDesc,CodeSeq FROM Codemaster WHERE Code ='" & _
                        '    objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCode.Code) & "' AND CodeType='" & _
                        '    objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCode.CodeType) & "'"
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
                        blnExec = True
                        If rdr Is Nothing = False Then
                            If rdr.Read Then
                                blnFound = True
                            End If
                            rdr.Close()
                        End If
                    End If

                    If blnExec Then
                        If blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                            'Item found & active
                            'system.Windows.Forms.MessageBox.Show("ab", "ABC",Windows.Forms.MessageBoxButtons.YesNo, Windows.Forms.MessageBoxIcon.Information,Windows.Forms.MessageBoxDefaultButton.Button2, Windows.Forms.MessageBoxOptions.DefaultDesktopOnly
                            'MsgBox("Record already exist", MsgBoxStyle.Critical, "Record Exist")
                            Return False
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = "CODEMASTER"
                                .AddField("CodeDesc", objCode.Description, SQLControl.EnumDataType.dtStringN)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True Then

                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                            "WHERE Code ='" & _
                                             objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCode.Code) & "' AND CodeType='" & _
                                             objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCode.CodeType) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("CodeSeq", objCode.CodeSequence.ToString, SQLControl.EnumDataType.dtNumeric)
                                                .AddField("Code", objCode.Code, SQLControl.EnumDataType.dtString)
                                                .AddField("CodeType", objCode.CodeType, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                               "WHERE Code ='" & _
                                             objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCode.Code) & "' AND CodeType='" & _
                                             objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCode.CodeType) & "'")
                                End Select
                            End With
                            Try
                                'execute 
                                objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002")
                                Else
                                    Throw New ApplicationException("210004")
                                End If
                            Finally
                                objSQL.Dispose()
                            End Try
                            Return True
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objCode = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Function Add(ByVal objCode As CodeMaster) As Boolean
            Return AssignItem(objCode, SQLControl.EnumSQLType.stInsert)
        End Function

        Function Amend(ByVal objCode As CodeMaster) As Boolean
            Return AssignItem(objCode, SQLControl.EnumSQLType.stUpdate)
        End Function

        Public Function AmendSequence(ByVal dsCode As DataSet, ByVal intSeq As Integer, ByVal Codetype As String) As Boolean
            Dim strsql As String
            Dim drData As DataRow
            Dim arySQL As ArrayList
            Try
                If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                    StartSQLControl()
                    If intSeq = 0 Then
                        With objSQL
                            .TableName = "CODEMASTER"
                            .AddField("CodeSeq", Convert.ToString(0), SQLControl.EnumDataType.dtNumeric)
                            strsql = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE Codetype ='" & Codetype & "'")
                        End With
                        objDCom.Execute(strsql, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                    Else
                        arySQL = New ArrayList
                        With objSQL
                            For Each drData In dsCode.Tables(0).Rows
                                .TableName = "CODEMASTER"
                                .AddField("CodeSeq", Convert.ToString(drData(2)), SQLControl.EnumDataType.dtNumeric)
                                arySQL.Add(.BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE Codetype ='" & Codetype & "' AND Code='" & Convert.ToString(drData(0)) & "'"))
                            Next
                            objDCom.BatchExecute(arySQL, CommandType.Text)
                        End With
                    End If
                End If
                Return True
            Catch ex As Exception
                Return False
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function
        'Public Function GetSortedDataset() As DataSet

        '    Try
        '        StartConnection()
        '        StartSQLControl()
        '        strSQL = "SELECT Code, CodeDesc, CodeSeq FROM CodeMaster WHERE CodeType = TAX ORDER BY CodeSeq"
        '        Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, "DsCodeMaster"), DataSet)

        '    Catch ex As Exception
        '        Return Nothing
        '    Finally
        '        EndSQLControl()
        '        EndConnection()
        '    End Try
        'End Function

        Function Delete(ByVal objCode As CodeMaster) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False

            Try
                If objCode Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With myinfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "Code ='" & _
                                    objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCode.Code) & "' AND CodeType='" & _
                                    objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCode.CodeType) & "'")
                        End With
                        'strSQL = "SELECT Code, CodeDesc,CodeSeq FROM Codemaster WHERE Code ='" & _
                        '  objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCode.Code) & "' AND CodeType='" & _
                        '  objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCode.CodeType) & "'"
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        If rdr Is Nothing = False Then
                            If rdr.Read Then
                                blnFound = True
                            End If
                            rdr.Close()
                        End If

                        If blnFound = False Then
                            'Error Message
                            Return False
                        End If

                        If blnFound = True Then
                            strSQL = BuildDelete(MyInfo.TableName, "Code = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCode.Code) & "' AND CODETYPE ='" & _
                                    objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCode.CodeType) & "'")
                            'strSQL = "DELETE FROM CODEMASTER WHERE Code = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCode.Code) & "' AND CODETYPE ='" & _
                            'objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCode.CodeType) & "'"
                        End If
                        Try
                            'execute
                            objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try
                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As SystemException
                Throw exDelete
            Finally
                objCode = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

#End Region

#Region "Data Selection"
        'Enquiry
        Function Enquiry(Optional ByVal SQLstmt As String = Nothing) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    If SQLstmt = Nothing Or SQLstmt = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName, , "CodeSeq")
                    Else
                        strSQL = SQLstmt
                    End If
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetListing() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetShortList() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetShortList(ByVal TypeCode As String) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, String.Concat(.ShortListCond, " AND TypeCode='", TypeCode, "'"))
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Shared Function EnquiryType(ByVal CodeType As String) As DataSet
            Dim strSQL As String
            If StartConnection() = True Then
                strSQL = "SELECT Code, CodeDesc  FROM CODEMASTER WHERE CodeType='" & CodeType & "'"
                Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, "CODEMASTER"), DataSet)
            Else
                Return Nothing
            End If
            EndConnection()
        End Function
#End Region

    End Class
#End Region

#Region "Room Base"
    Public Class RoomBase
        Inherits Core.CoreBase

        Public Enum EnumSelectType
            Room = 0
            Seat = 1
        End Enum

        Dim Seat As StrucClassInfo
#Region "Room Class in Room Base"
        Public Class Room
            Inherits Core.SingleBase
            Protected strCode, strDesc, strUpdBy, strBranchID As String
            Protected intActive As Integer
            Protected dtLastUpd As DateTime
            Public MyScheme As New MyScheme

            Public Property Code() As String
                Get
                    Return strCode
                End Get
                Set(ByVal RoomID As String)
                    strCode = RoomID
                    MyBase.AddItem(0, strCode)
                End Set
            End Property

            Public Property Description() As String
                Get
                    Return strDesc
                End Get
                Set(ByVal Value As String)
                    strDesc = Value
                    MyBase.AddItem(1, strDesc)
                End Set
            End Property

            Public Property BranchID() As String
                Get
                    Return strBranchID
                End Get
                Set(ByVal Value As String)
                    strBranchID = Value
                End Set
            End Property
            Public Property LastUpdate() As DateTime
                Get
                    Return dtLastUpd
                End Get
                Set(ByVal Value As DateTime)
                    If Value = Nothing Then
                        dtLastUpd = Now()
                    Else
                        dtLastUpd = Value
                    End If
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return strUpdBy
                End Get
                Set(ByVal Value As String)
                    strUpdBy = Value
                End Set
            End Property

            Public Property Active() As Integer
                Get
                    Return intActive
                End Get
                Set(ByVal Value As Integer)
                    If Value < 0 Then
                        intActive = 0
                    Else
                        intActive = Value
                    End If
                End Set
            End Property

        End Class
#End Region

#Region "Seat class in RoomSeat base"
        Public Class RoomSeat
            Inherits Core.SingleBase
            Protected strRoomID, strSecID, strSeatID, strSeatDesc, strUpdateBy, strBranchID As String
            Protected intActive As Integer
            Protected dtLastUpdate As DateTime
            Public MyScheme As New MyScheme

            Public Property RoomID() As String
                Get
                    Return strRoomID
                End Get
                Set(ByVal RoomID As String)
                    strRoomID = RoomID
                    MyBase.AddItem(0, strRoomID)
                End Set
            End Property

            'Public Property SecID() As String
            '    Get
            '        Return strSecID
            '    End Get
            '    Set(ByVal SecID As String)
            '        strSecID = SecID
            '        MyBase.AddItem(1, strSecID)
            '    End Set
            'End Property

            Public Property SeatID() As String
                Get
                    Return strSeatID
                End Get
                Set(ByVal Value As String)
                    strSeatID = Value
                    MyBase.AddItem(1, strSeatID)
                End Set
            End Property

            Public Property SeatDesc() As String
                Get
                    Return strSeatDesc
                End Get
                Set(ByVal SeatDesc As String)
                    strSeatDesc = SeatDesc
                    MyBase.AddItem(2, strSeatID)
                End Set
            End Property

            Public Property BranchID() As String
                Get
                    Return strBranchID
                End Get
                Set(ByVal Value As String)
                    strBranchID = Value
                End Set
            End Property

            Public Property LastUpdate() As DateTime
                Get
                    Return dtLastUpdate
                End Get
                Set(ByVal Value As DateTime)
                    If Value = Nothing Then
                        dtLastUpdate = Now()
                    Else
                        dtLastUpdate = Value
                    End If
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return strUpdateBy
                End Get
                Set(ByVal Value As String)
                    strUpdateBy = Value
                End Set
            End Property

            Public Property Active() As Integer
                Get
                    Return intActive
                End Get
                Set(ByVal Value As Integer)
                    If Value < 0 Then
                        intActive = 0
                    Else
                        intActive = Value
                    End If
                End Set
            End Property
        End Class
#End Region

#Region "Room Scheme"
        Public Class MyScheme
            Inherits Core.SchemeBase
            Protected Overrides Sub InitializeInfo()
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "RoomID"
                    .Length = 10
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(0, this)

                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "RoomDesc"
                    .Length = 50
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(1, this)
            End Sub

            Public ReadOnly Property Code() As StrucElement
                Get
                    Return MyBase.GetItem(0)
                End Get
            End Property

            Public ReadOnly Property Description() As StrucElement
                Get
                    Return MyBase.GetItem(1)
                End Get
            End Property

            Public ReadOnly Property SeatID() As StrucElement
                Get
                    Return MyBase.GetItem(2)
                End Get
            End Property

            Public ReadOnly Property SeatDesc() As StrucElement
                Get
                    Return MyBase.GetItem(3)
                End Get
            End Property

            Public Function GetElement(ByVal Key As Integer) As StrucElement
                Return MyBase.GetItem(Key)
            End Function
        End Class
#End Region

      

#Region "Room Data Manipulation-Add,Edit,Del"
        Private Function AssignItem(ByVal objRoom As Room, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As SqlClient.SqlDataReader
            AssignItem = False
            Try
                If objRoom Is Nothing Then
                    'msg return
                Else
                    'Dim test As ArrayList
                    'test.Add("A")
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "RoomID ='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoom.Code) & _
                            "'AND BRANCHID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoom.BranchID) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        blnExec = True 'executed - select
                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    'record is found
                                    blnFound = True
                                    If Convert.ToInt16(.Item("Flag")) = 0 Then
                                        'found but deleted
                                        blnFlag = False
                                    Else
                                        'found and active
                                        blnFlag = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If
                    End If

                    If blnExec Then 'if executed
                        If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = MyInfo.TableName
                                .AddField("RoomDesc", objRoom.Description, SQLControl.EnumDataType.dtStringN)
                                .AddField("UpdateBy", objRoom.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", objRoom.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                .AddField("Active", objRoom.Active.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", cFlagActive.ToString, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert 'INSERT
                                        'record found but not deleted
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE RoomID = '" & .ParseValue(SQLControl.EnumDataType.dtString, objRoom.Code) & _
                                            "'AND BRANCHID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoom.BranchID) & "'")
                                        Else
                                            If blnFound = False Then 'if record not found
                                                .AddField("RoomID", objRoom.Code, SQLControl.EnumDataType.dtString)
                                                .AddField("BranchID", objRoom.BranchID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert, , "WHERE RoomID = '" & .ParseValue(SQLControl.EnumDataType.dtString, objRoom.Code) & _
                                                "'AND BRANCHID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoom.BranchID) & "'")
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate 'UPDATE
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE RoomID='" & .ParseValue(SQLControl.EnumDataType.dtString, objRoom.Code) & _
                                        "'AND BRANCHID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoom.BranchID) & "'")
                                End Select
                            End With
                            Try
                                'execute
                                objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                Return True
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002")
                                Else
                                    Throw New ApplicationException("210004")
                                End If
                            Finally
                                objSQL.Dispose()
                            End Try
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objRoom = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Function Add(ByVal objRoom As Room) As Boolean
            Return AssignItem(objRoom, SQLControl.EnumSQLType.stInsert)
        End Function

        'AMEND
        Function Amend(ByVal objRoom As Room) As Boolean
            Return AssignItem(objRoom, SQLControl.EnumSQLType.stUpdate)
        End Function

        'DELETE
        Function Delete(ByVal objRoom As Room) As Boolean
            Dim strSQL As String
            Dim arrsql As ArrayList
            Dim blnFound, blnInUse As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If objRoom Is Nothing Then
                    'error message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        arrsql = New ArrayList
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "RoomID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoom.Code) & _
                            "'AND BRANCHID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoom.BranchID) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt32(.Item("InUse")) = 1 Then
                                        blnInUse = True 'if record is found and in use
                                    End If
                                End If
                                .Close()
                            End With
                        End If

                        'record is found and in use
                        If blnFound = True And blnInUse = True Then
                            strSQL = BuildUpdate(MyInfo.TableName, " SET Flag = " & cFlagNonActive & _
                                " , LastUpdate = '" & objRoom.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoom.UpdateBy) & "'" & _
                                " WHERE RoomID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoom.Code) & _
                                "'AND BRANCHID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoom.BranchID) & "'")
                            arrsql.Add(strSQL)
                            strSQL = BuildUpdate(Seat.TableName, " SET Flag = " & cFlagNonActive & _
                                    " , LastUpdate = '" & objRoom.LastUpdate & "' , UpdateBy = '" & _
                                    objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoom.UpdateBy) & "'" & _
                                    " WHERE RoomID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoom.Code) & _
                             "'AND BRANCHID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoom.BranchID) & "'")
                            arrsql.Add(strSQL)
                        End If

                        'record exist but not in use
                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(MyInfo.TableName, "RoomID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoom.Code) & _
                            "'AND BRANCHID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoom.BranchID) & "'")
                            arrsql.Add(strSQL)
                            strSQL = BuildDelete(Seat.TableName, "RoomID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoom.Code) & "'")
                             '"'AND BRANCHID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoom.BranchID) & "'")
                            arrsql.Add(strSQL)
                        End If
                        Try
                            'execute
                            objDCom.BatchExecute(arrsql, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try
                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As Exception
                Throw exDelete
            Finally
                objRoom = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Seat Data Manipulation-Add,Edit,Del"
        Private Function SeatAssignItem(ByVal objRoomSeat As RoomSeat, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As SqlClient.SqlDataReader
            SeatAssignItem = False
            Try
                If objRoomSeat Is Nothing Then
                    'msg return
                Else
                    'Dim test As ArrayList
                    'test.Add("A")
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Seat
                            strSQL = BuildSelect(.CheckFields, .TableName, "RoomID ='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoomSeat.RoomID) & _
                            "' AND SeatID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoomSeat.SeatID) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        blnExec = True 'executed - select
                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    'record is found
                                    blnFound = True
                                    If Convert.ToInt16(.Item("Flag")) = 0 Then
                                        'found but deleted
                                        blnFlag = False
                                    Else
                                        'found and active
                                        blnFlag = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If
                    End If

                    If blnExec Then 'if executed
                        If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = Seat.TableName
                                .AddField("SeatDesc", objRoomSeat.SeatDesc, SQLControl.EnumDataType.dtStringN)
                                .AddField("LastUpdate", objRoomSeat.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", objRoomSeat.UpdateBy.ToString, SQLControl.EnumDataType.dtString)
                                .AddField("Active", objRoomSeat.Active.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", cFlagActive.ToString, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert 'INSERT
                                        'record found but not deleted
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE RoomID = '" & .ParseValue(SQLControl.EnumDataType.dtString, objRoomSeat.RoomID) & _
                                            " AND SeatID = ' " & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoomSeat.SeatID) & "'")
                                        Else
                                            If blnFound = False Then 'if record not found
                                                .AddField("BranchID", objRoomSeat.BranchID, SQLControl.EnumDataType.dtString)
                                                .AddField("RoomID", objRoomSeat.RoomID, SQLControl.EnumDataType.dtString)
                                                .AddField("SeatID", objRoomSeat.SeatID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate 'UPDATE
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE RoomID='" & .ParseValue(SQLControl.EnumDataType.dtString, objRoomSeat.RoomID) & _
                                        " AND SeatID = ' " & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoomSeat.SeatID) & "'")
                                End Select
                            End With
                            Try
                                'execute
                                objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                Return True
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002")
                                Else
                                    Throw New ApplicationException("210004")
                                End If
                            Finally
                                objSQL.Dispose()
                            End Try
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objRoomSeat = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Function SeatAdd(ByVal objRoomSeat As RoomSeat) As Boolean
            Return SeatAssignItem(objRoomSeat, SQLControl.EnumSQLType.stInsert)
        End Function

        'DELETE
        Function SeatDelete(ByVal objRoomSeat As RoomSeat) As Boolean

            Dim strSQL As String
            Dim blnFound, blnInUse As Boolean
            Dim rdr As SqlClient.SqlDataReader
            SeatDelete = False
            blnFound = False
            blnInUse = False
            Try
                If objRoomSeat Is Nothing Then
                    'error message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Seat
                            strSQL = BuildSelect(.CheckFields, .TableName, "RoomID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoomSeat.RoomID) & _
                            "' AND SeatID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoomSeat.SeatID) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt32(.Item("InUse")) = 1 Then
                                        blnInUse = True 'if record is found and in use
                                    End If
                                End If
                                .Close()
                            End With
                        End If

                        'record is found and in use
                        If blnFound = True And blnInUse = True Then
                            strSQL = BuildUpdate(Seat.TableName, " SET Flag = " & cFlagNonActive & _
                                " , LastUpdate = '" & objRoomSeat.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoomSeat.UpdateBy) & "'" & _
                                " WHERE RoomID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoomSeat.RoomID) & "' AND " & _
                                "SeatID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoomSeat.SeatID) & "'")
                        End If

                        'record exist but not in use
                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(Seat.TableName, "RoomID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoomSeat.RoomID) & "' AND " & _
                                "SeatID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objRoomSeat.SeatID) & "'")
                        End If

                        Try
                            'execute
                            objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try

                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As Exception
                Throw exDelete
            Finally
                objRoomSeat = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        'Enquiry
        Function Enquiry(Optional ByVal SQLstmt As String = Nothing) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    If SQLstmt = Nothing Or SQLstmt = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName)
                    Else
                        strSQL = SQLstmt
                    End If
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Function SeatEnquiry(ByVal RoomID As String) As DataSet
            If StartConnection() = True Then
                StartSQLControl()
                With Seat
                    strSQL = BuildSelect(.FieldsList, .TableName, "RoomID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, RoomID) & "'")
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetListing() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetShortList() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function
#End Region

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "ROOMID, ROOMDESC, BRANCHID, LASTUPDATE, UPDATEBY, ACTIVE, INUSE, FLAG"
                .CheckFields = "INUSE, FLAG"
                .TableName = "ROOM"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "ROOMID, ROOMDESC, BRANCHID, LASTUPDATE, UPDATEBY, ACTIVE, INUSE, FLAG"
                .ListingCond = "FLAG = 1"
                .ShortList = String.Empty
                .ShortListCond = String.Empty
            End With
            With Seat
                .FieldsList = "ROOMID, SEATID, SEATDESC, LASTUPDATE, UPDATEBY, ACTIVE, INUSE, FLAG"
                .CheckFields = "INUSE, FLAG"
                .TableName = "SEAT"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "ROOMID, SEATID, SEATDESC, LASTUPDATE, UPDATEBY, ACTIVE, INUSE, FLAG"
                .ListingCond = "ACTIVE = 1"
                .ShortList = String.Empty
                .ShortListCond = String.Empty
            End With
            ReDim MyFields(1)
            MyBase.AddMyField(0, "Room ID", "RoomID", TypeCode.String)
            MyBase.AddMyField(1, "Room Desc", "RoomDesc", TypeCode.String)
        End Sub

        Public Shared Function GetCombo() As DataSet
            GetCombo = EnquiryDS("SELECT RoomID, RoomDesc FROM Room WHERE Active = 1 AND Flag = 1")
        End Function

        Public Shared Function RoomShortList() As DataTable
            Try
                StartSQLControl()
                StartConnection()

                strSQL = "SELECT RoomID, RoomDesc FROM Room WHERE Active = 1 AND FLAG = 1"

                Return objDCom.Execute(strSQL, CommandType.Text, True)
            Catch ex As Exception
                Return Nothing
            Finally
                EndConnection()
                EndSQLControl()
            End Try

        End Function


    End Class
#End Region

#Region "Seat Base"
    Public Class SeatBase
        Inherits Core.CoreBase


        Public Shared Function GetCombo(ByVal RoomNo As String) As DataSet
            StartSQLControl()
            GetCombo = EnquiryDS("SELECT SeatID, SeatDesc FROM Seat WHERE Active = 1 AND Flag = 1 AND RoomID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, RoomNo) & "'")
            If objSQL Is Nothing = False Then
                objSQL = Nothing
            End If
        End Function
    End Class
#End Region

#Region "Analysis Answer Set Base"
    Public Class AnalysisAnsSetBase
        Inherits Core.CoreBase

#Region "AnalysisAnsSet class in AnalysisAnsSetBase"
        Public Class AnalysisAnsSet
            Protected strQuestionCode, strUpdBy As String
            Protected dtAnswerValue As DataTable
            Protected intActive As Integer
            Protected dtLastUpd As DateTime
            Public MyScheme As New MyScheme
            Protected arrQuesCode() As String = {"TYPE", "LOTION", "RODS", "MIN", "NEUTR", "H", "SUPPORT", "TINTED SHADE", "PERCENTAGE", "METHOD"}

            Public ReadOnly Property QuestionCodeList() As Array
                Get
                    Return arrQuesCode
                End Get
            End Property


            Public Sub New()
                SetAnswerValueTable()
            End Sub

            Private Sub SetAnswerValueTable()
                If dtAnswerValue Is Nothing = True Then
                    dtAnswerValue = New DataTable
                End If

                With dtAnswerValue
                    .Clear()
                    .Columns.Clear()
                    .Columns.Add("AnsValue", Type.GetType("System.String"))
                End With
            End Sub

            Public Property QuestionCode() As String
                Get
                    Return strQuestionCode
                End Get
                Set(ByVal value As String)
                    If value.Trim = String.Empty Then
                        Throw New Exception("Please key in Answer Code")
                    Else
                        strQuestionCode = value
                    End If
                End Set
            End Property

            Public Property AnswerValue() As DataTable
                Get
                    Return dtAnswerValue
                End Get
                Set(ByVal Value As DataTable)
                    If Value Is Nothing = True Or Value.Rows.Count = 0 Then
                        Throw New Exception("Answer Value cannot be blank")
                    Else
                        dtAnswerValue = Value
                    End If
                End Set
            End Property

            Public Property LastUpdate() As DateTime
                Get
                    Return dtLastUpd
                End Get
                Set(ByVal Value As DateTime)
                    dtLastUpd = Value
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return strUpdBy
                End Get
                Set(ByVal Value As String)
                    strUpdBy = Value
                End Set
            End Property

            Public Property Active() As Integer
                Get
                    Return intActive
                End Get
                Set(ByVal Value As Integer)
                    intActive = Value
                End Set
            End Property

            Public ReadOnly Property DistinctSelection() As String
                Get
                    Return "SELECT DISTINCT QUESCODE, LASTUPDATE, UPDATEBY FROM ANALYSISANSSET"
                End Get
            End Property

            Public ReadOnly Property OrderSeq() As String
                Get
                    Return " ORDER BY SEQNO"
                End Get
            End Property
        End Class
#End Region

#Region "AnalysisAnsSet Scheme"
        Public Class MyScheme
            Inherits Core.SchemeBase

            Protected Overrides Sub InitializeInfo()
                'With this
                '    .DataType = SQLControl.EnumDataType.dtString
                '    .FieldName = "QuesCode"
                '    .Length = 10
                '    .DecPlace = Nothing
                '    .RegExp = String.Empty
                '    .IsMandatory = True
                '    .AllowNegative = False
                'End With
                'MyBase.AddItem(0, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "AnsValue"
                    .Length = 20
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(0, this)
            End Sub

            'Public ReadOnly Property QuestionCode() As StrucElement
            '    Get
            '        Return MyBase.GetItem(0)
            '    End Get
            'End Property

            Public ReadOnly Property AnswerValue() As StrucElement
                Get
                    Return MyBase.GetItem(0)
                End Get
            End Property

            Public Function GetElement(ByVal Key As Integer) As StrucElement
                Return MyBase.GetItem(Key)
            End Function
        End Class
#End Region

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "QUESCODE, ANSVALUE, LASTUPDATE, UPDATEBY, SEQNO"
                .CheckFields = "QUESCODE, ANSVALUE"
                .TableName = "ANALYSISANSSET"
                .DefaultCond = ""
                .DefaultOrder = String.Empty
                .Listing = "QUESCODE, ANSVALUE, LASTUPDATE, UPDATEBY, SEQNO"
                .ListingCond = String.Empty
                .ShortList = "QUESCODE, ANSVALUE"
                .ShortListCond = String.Empty
            End With
            ReDim MyFields(0)
            MyBase.AddMyField(0, "Question Code", "QUESCODE", TypeCode.String)
            'MyBase.AddMyField(1, "Answer Descption", "ANSVALUE", TypeCode.String)
        End Sub

#Region "Data Manipulation-Add,Edit,Del"

        Private Function AssignItem(ByVal objAnswer As AnalysisAnsSet, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As SqlClient.SqlDataReader
            AssignItem = False
            Try
                If objAnswer Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    'blnFlag = False

                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "QUESCODE = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, objAnswer.QuestionCode) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, CommandType.Text, CommandBehavior.SingleRow, False), SqlClient.SqlDataReader)

                        blnExec = True

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                End If
                                .Close()
                            End With
                        End If
                    End If

                    If blnExec Then
                        If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                            'Item found & active
                            'system.Windows.Forms.MessageBox.Show("ab", "ABC",Windows.Forms.MessageBoxButtons.YesNo, Windows.Forms.MessageBoxIcon.Information,Windows.Forms.MessageBoxDefaultButton.Button2, Windows.Forms.MessageBoxOptions.DefaultDesktopOnly
                            'MsgBox("Record already exist", MsgBoxStyle.Critical, "Record Exist")
                            Return False
                        Else
                            StartSQLControl()
                            If objDCom.BatchExecute(SaveAnswerValue(objAnswer, pType), CommandType.Text, True) = True Then
                                Return True
                            Else
                                Return False
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
                Return False
            Finally
                objAnswer = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Private Function SaveAnswerValue(ByVal objAnswer As AnalysisAnsSet, ByVal pType As SQLControl.EnumSQLType) As ArrayList
            Dim drRow As DataRow
            Dim sSQl As String
            Dim arrList As New ArrayList
            Dim iCnt As Int32 = 0

            If pType = SQLControl.EnumSQLType.stUpdate Then
                sSQl = BuildDelete(MyInfo.TableName, "QuesCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objAnswer.QuestionCode) & "'")
                arrList.Add(sSQl)
            End If

            For Each drRow In objAnswer.AnswerValue.Rows
                With objSQL
                    .TableName = myinfo.TableName
                    .AddField("QuesCode", objAnswer.QuestionCode.ToString, SQLControl.EnumDataType.dtStringN)
                    .AddField("AnsValue", Convert.ToString(drRow("AnsValue")), SQLControl.EnumDataType.dtStringN)
                    .AddField("LastUpdate", objAnswer.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                    .AddField("UpdateBy", objAnswer.UpdateBy.ToString, SQLControl.EnumDataType.dtString)
                    iCnt = iCnt + 1
                    .AddField("SeqNo", iCnt.ToString, SQLControl.EnumDataType.dtNumeric)
                    sSQl = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                    arrList.Add(sSQl)
                End With
            Next

            If arrList Is Nothing = False Then
                Return arrList
            Else
                Return Nothing
            End If
        End Function

        'ADD
        Function Add(ByVal objAnswer As AnalysisAnsSet) As Boolean
            Return AssignItem(objAnswer, SQLControl.EnumSQLType.stInsert)
        End Function
        'AMEND
        Function Amend(ByVal objAnswer As AnalysisAnsSet) As Boolean
            Return AssignItem(objAnswer, SQLControl.EnumSQLType.stUpdate)
        End Function
        'DELETE
        Function Delete(ByVal objAnswer As AnalysisAnsSet) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If objAnswer Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "QuesCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, objAnswer.QuestionCode) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, CommandType.Text, CommandBehavior.SingleRow, False), SqlClient.SqlDataReader)

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                End If
                                .Close()
                            End With
                        End If

                        If blnFound = False Then
                            'Error Message
                            Return False
                        End If

                        If blnFound = True Then
                            strSQL = BuildDelete(MyInfo.TableName, "QuesCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, objAnswer.QuestionCode) & "'")
                        End If
                        Try
                            'execute
                            objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try
                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As SystemException
                Throw exDelete
            Finally
                objAnswer = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        'Enquiry
        Function Enquiry(Optional ByVal SQLstmt As String = Nothing) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    If SQLstmt = Nothing Or SQLstmt = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName)
                    Else
                        strSQL = SQLstmt
                    End If
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetListing() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetShortList() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function
#End Region
        'Clear all data in database
        Function ClearDB() As Boolean
        End Function
    End Class
#End Region

#Region "Branch Base"
    Public Class BranchBase
        Inherits Core.CoreBase
        Dim SysPref, SysPrefB As StrucClassInfo

#Region "Branch class in Branch Base"
        Public Class Branch
            Protected strBranchID, strRevGroup, strBranchName, strBranchCode, strAccNo As String
            Protected strAddr1, strAddr2, strAddr3, strAddr4, strPostCode, strContact1, strContact2, strTel, strFax, strEmail, strRegion As String
            Protected strStoreType, strCountry, strState, strCurrency, strSalesType, strInSvcType, strRcpHeader, strRcpFooter As String
            Protected intStoreStatus, intDay1, intDay2, intDay3, intDay4, intDay5, intDay6, intDay7, intBookDayAllow, intBookHourAllow, intBookInterval, intGenInSvcID, intPriceLevel As Int32
            Protected dtBizStartTime, dtBizEndTime, dtBookStartTime, dtBookEndTime As DateTime
            Private dtBranchSetting As DataTable
            Protected dtBranchSysPref As DataTable
            Protected strUpdBy As String
            Protected dtLastUpd As Date
            'Start
            'frmBranchList.vb, SysPrefB
            Public Structure StrucBranchSetting
                Dim AnalysisImagepath As String
                Dim MaxStock As String
                Dim MinStock As String
                Dim NegativeStock As String
                Dim Normal As String
                Dim ReOrderLvl As String
                Dim DefCountry As String
                Dim DefCurrency As String
                'Dim DefCash As String
                'Dim DefCredit As String
                'Dim DefPrepaid As String
            End Structure

            Private MyBranchSetting As StrucBranchSetting

            'Public Overloads Property BranchSetting() As StrucBranchSetting
            '    Get
            '        Return MyBranchSetting
            '    End Get
            '    Set(ByVal Value As StrucBranchSetting)
            '        MyBranchSetting = Value
            '    End Set
            'End Property

            Public Overloads Property BranchSetting() As StrucBranchSetting
                Get
                    Return MyBranchSetting
                End Get
                Set(ByVal BranchValue As StrucBranchSetting)
                    MyBranchSetting = BranchValue
                End Set
            End Property
            ''
            Public Property BranchSysPref() As DataTable
                Get
                    Return dtBranchSysPref
                End Get
                Set(ByVal Value As DataTable)
                    dtBranchSysPref = Value
                End Set
            End Property

            Public Overloads Property BranchSysSet() As DataTable
                Get
                    Return dtBranchSetting
                End Get
                Set(ByVal Value As DataTable)
                    dtBranchSetting = Value
                End Set
            End Property
            'end

            Public Property Currency() As String
                Get
                    Return strCurrency
                End Get
                Set(ByVal Value As String)
                    strCurrency = Value
                End Set
            End Property

            Public Property Region() As String
                Get
                    Return strRegion
                End Get
                Set(ByVal Value As String)
                    strRegion = Value
                End Set
            End Property

            Public Property BizStartTime() As DateTime
                Get
                    Return dtBizStartTime
                End Get
                Set(ByVal Value As DateTime)
                    dtBizStartTime = Value
                End Set
            End Property

            Public Property BizEndTime() As DateTime
                Get
                    Return dtBizEndTime
                End Get
                Set(ByVal Value As DateTime)
                    dtBizEndTime = Value
                End Set
            End Property

            Public Property BookStartTime() As DateTime
                Get
                    Return dtBookStartTime
                End Get
                Set(ByVal Value As DateTime)
                    dtBookStartTime = Value
                End Set
            End Property

            Public Property BookEndTime() As DateTime
                Get
                    Return dtBookEndTime
                End Get
                Set(ByVal Value As DateTime)
                    dtBookEndTime = Value
                End Set
            End Property

            Public Property GenerateInServiceID() As Int32
                Get
                    Return intGenInSvcID
                End Get
                Set(ByVal Value As Int32)
                    intGenInSvcID = Value
                End Set
            End Property

            Public Property BookingInterval() As Int32
                Get
                    Return intBookInterval
                End Get
                Set(ByVal Value As Int32)
                    intBookInterval = Value
                End Set
            End Property

            Public Property BookHourAllow() As Int32
                Get
                    Return intBookHourAllow
                End Get
                Set(ByVal Value As Int32)
                    intBookHourAllow = Value
                End Set
            End Property

            Public Property BookDayAllow() As Int32
                Get
                    Return intBookDayAllow
                End Get
                Set(ByVal Value As Int32)
                    intBookDayAllow = Value
                End Set
            End Property

            Public Property OperatingDay1() As Int32
                Get
                    Return intDay1
                End Get
                Set(ByVal Value As Int32)
                    intDay1 = Value
                End Set
            End Property

            Public Property OperatingDay2() As Int32
                Get
                    Return intDay2
                End Get
                Set(ByVal Value As Int32)
                    intDay2 = Value
                End Set
            End Property

            Public Property OperatingDay3() As Int32
                Get
                    Return intDay3
                End Get
                Set(ByVal Value As Int32)
                    intDay3 = Value
                End Set
            End Property

            Public Property OperatingDay4() As Int32
                Get
                    Return intDay4
                End Get
                Set(ByVal Value As Int32)
                    intDay4 = Value
                End Set
            End Property

            Public Property OperatingDay5() As Int32
                Get
                    Return intDay5
                End Get
                Set(ByVal Value As Int32)
                    intDay5 = Value
                End Set
            End Property

            Public Property OperatingDay6() As Int32
                Get
                    Return intDay6
                End Get
                Set(ByVal Value As Int32)
                    intDay6 = Value
                End Set
            End Property

            Public Property OperatingDay7() As Int32
                Get
                    Return intDay7
                End Get
                Set(ByVal Value As Int32)
                    intDay7 = Value
                End Set
            End Property

            Public Property StoreStatus() As Int32
                Get
                    Return intStoreStatus
                End Get
                Set(ByVal Value As Int32)
                    intStoreStatus = Value
                End Set
            End Property

            Public Property ReceiptFooter() As String
                Get
                    Return strRcpFooter
                End Get
                Set(ByVal Value As String)
                    strRcpFooter = Value
                End Set
            End Property
            Public Property ReceiptHeader() As String
                Get
                    Return strRcpHeader
                End Get
                Set(ByVal Value As String)
                    strRcpHeader = Value
                End Set
            End Property

            Public Property InServiceType() As String
                Get
                    Return strInSvcType
                End Get
                Set(ByVal Value As String)
                    strInSvcType = Value
                End Set
            End Property

            Public Property SalesType() As String
                Get
                    Return strSalesType
                End Get
                Set(ByVal Value As String)
                    strSalesType = Value
                End Set
            End Property

            Public Property State() As String
                Get
                    Return strState
                End Get
                Set(ByVal Value As String)
                    strState = Value
                End Set
            End Property

            Public Property Country() As String
                Get
                    Return strCountry
                End Get
                Set(ByVal Value As String)
                    strCountry = Value
                End Set
            End Property

            Public Property StoreType() As String
                Get
                    Return strStoreType
                End Get
                Set(ByVal Value As String)
                    strStoreType = Value
                End Set
            End Property

            Public Property Address1() As String
                Get
                    Return strAddr1
                End Get
                Set(ByVal Value As String)
                    strAddr1 = Value
                End Set
            End Property

            Public Property Address2() As String
                Get
                    Return strAddr2
                End Get
                Set(ByVal Value As String)
                    strAddr2 = Value
                End Set
            End Property

            Public Property Address3() As String
                Get
                    Return strAddr3
                End Get
                Set(ByVal Value As String)
                    strAddr3 = Value
                End Set
            End Property

            Public Property Address4() As String
                Get
                    Return strAddr4
                End Get
                Set(ByVal Value As String)
                    strAddr4 = Value
                End Set
            End Property

            Public Property PostCode() As String
                Get
                    Return strPostCode
                End Get
                Set(ByVal Value As String)
                    strPostCode = Value
                End Set
            End Property

            Public Property Contact1() As String
                Get
                    Return strContact1
                End Get
                Set(ByVal Value As String)
                    strContact1 = Value
                End Set
            End Property

            Public Property Contact2() As String
                Get
                    Return strContact2
                End Get
                Set(ByVal Value As String)
                    strContact2 = Value
                End Set
            End Property

            Public Property TelNo() As String
                Get
                    Return strTel
                End Get
                Set(ByVal Value As String)
                    strTel = Value
                End Set
            End Property

            Public Property FaxNo() As String
                Get
                    Return strFax
                End Get
                Set(ByVal Value As String)
                    strFax = Value
                End Set
            End Property

            Public Property Email() As String
                Get
                    Return strEmail
                End Get
                Set(ByVal Value As String)
                    strEmail = Value
                End Set
            End Property

            Public Property BranchID() As String
                Get
                    Return strBranchID
                End Get
                Set(ByVal Value As String)
                    strBranchID = Value
                End Set
            End Property

            Public Property RevenueGroup() As String
                Get
                    Return strRevGroup
                End Get
                Set(ByVal Value As String)
                    strRevGroup = Value
                End Set
            End Property

            Public Property BranchName() As String
                Get
                    Return strBranchName
                End Get
                Set(ByVal Value As String)
                    strBranchName = Value
                End Set
            End Property

            Public Property BranchCode() As String
                Get
                    Return strBranchCode
                End Get
                Set(ByVal Value As String)
                    strBranchCode = Value
                End Set
            End Property

            Public Property AccountNo() As String
                Get
                    Return strAccNo
                End Get
                Set(ByVal Value As String)
                    strAccNo = Value
                End Set
            End Property

            Public Property PriceLevel() As Int32
                Get
                    If intPriceLevel < 0 Then intPriceLevel = 0
                    Return intPriceLevel
                End Get
                Set(ByVal Value As Int32)
                    intPriceLevel = Value
                End Set
            End Property

            Public Property LastUpdate() As Date
                Get
                    Return dtLastUpd
                End Get
                Set(ByVal Value As Date)
                    dtLastUpd = Value
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return strUpdBy
                End Get
                Set(ByVal Value As String)
                    strUpdBy = Value
                End Set
            End Property

        End Class
#End Region

#Region "Branch Scheme"
        Public Class MyScheme
            Inherits Core.SchemeBase

            Protected Overrides Sub InitializeInfo()
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "BranchID"
                    .Length = 10
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(0, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "RevGroup"
                    .Length = 10
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(1, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "BranchName"
                    .Length = 50
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(2, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "BranchCode"
                    .Length = 10
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = False
                    .AllowNegative = False
                End With
                MyBase.AddItem(3, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "AccNo"
                    .Length = 15
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = False
                    .AllowNegative = False
                End With
                MyBase.AddItem(4, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "Address1"
                    .Length = 40
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = False
                    .AllowNegative = False
                End With
                MyBase.AddItem(5, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "Address2"
                    .Length = 40
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = False
                    .AllowNegative = False
                End With
                MyBase.AddItem(6, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "Address3"
                    .Length = 40
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = False
                    .AllowNegative = False
                End With
                MyBase.AddItem(7, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "Address4"
                    .Length = 40
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = False
                    .AllowNegative = False
                End With
                MyBase.AddItem(8, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "PostalCode"
                    .Length = 10
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = False
                    .AllowNegative = False
                End With
                MyBase.AddItem(9, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "Contact1"
                    .Length = 16
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = False
                    .AllowNegative = False
                End With
                MyBase.AddItem(10, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "Contact2"
                    .Length = 16
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = False
                    .AllowNegative = False
                End With
                MyBase.AddItem(11, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "Email"
                    .Length = 80
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = False
                    .AllowNegative = False
                End With
                MyBase.AddItem(12, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "Tel"
                    .Length = 16
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = False
                    .AllowNegative = False
                End With
                MyBase.AddItem(13, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "Fax"
                    .Length = 16
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = False
                    .AllowNegative = False
                End With
                MyBase.AddItem(14, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "Region"
                    .Length = 10
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = False
                    .AllowNegative = False
                End With
                MyBase.AddItem(15, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "RcpHeader"
                    .Length = 50
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = False
                    .AllowNegative = False
                End With
                MyBase.AddItem(16, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "RcpFooter"
                    .Length = 50
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = False
                    .AllowNegative = False
                End With
                MyBase.AddItem(17, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "PriceLevel"
                    .Length = 2
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = False
                    .AllowNegative = False
                End With
                MyBase.AddItem(18, this)
            End Sub

            Public ReadOnly Property BranchID() As StrucElement
                Get
                    Return MyBase.GetItem(0)
                End Get
            End Property

            Public ReadOnly Property RevenueGroup() As StrucElement
                Get
                    Return MyBase.GetItem(1)
                End Get
            End Property

            Public ReadOnly Property BranchName() As StrucElement
                Get
                    Return MyBase.GetItem(2)
                End Get
            End Property

            Public ReadOnly Property BranchCode() As StrucElement
                Get
                    Return MyBase.GetItem(3)
                End Get
            End Property

            Public ReadOnly Property AccountNo() As StrucElement
                Get
                    Return MyBase.GetItem(4)
                End Get
            End Property

            Public ReadOnly Property Address1() As StrucElement
                Get
                    Return MyBase.GetItem(5)
                End Get
            End Property

            Public ReadOnly Property Address2() As StrucElement
                Get
                    Return MyBase.GetItem(6)
                End Get
            End Property

            Public ReadOnly Property Address3() As StrucElement
                Get
                    Return MyBase.GetItem(7)
                End Get
            End Property

            Public ReadOnly Property Address4() As StrucElement
                Get
                    Return MyBase.GetItem(8)
                End Get
            End Property

            Public ReadOnly Property PostalCode() As StrucElement
                Get
                    Return MyBase.GetItem(9)
                End Get
            End Property

            Public ReadOnly Property Contact1() As StrucElement
                Get
                    Return MyBase.GetItem(10)
                End Get
            End Property

            Public ReadOnly Property Contact2() As StrucElement
                Get
                    Return MyBase.GetItem(11)
                End Get
            End Property

            Public ReadOnly Property Email() As StrucElement
                Get
                    Return MyBase.GetItem(12)
                End Get
            End Property

            Public ReadOnly Property Tel() As StrucElement
                Get
                    Return MyBase.GetItem(13)
                End Get
            End Property

            Public ReadOnly Property Fax() As StrucElement
                Get
                    Return MyBase.GetItem(14)
                End Get
            End Property

            Public ReadOnly Property Region() As StrucElement
                Get
                    Return MyBase.GetItem(15)
                End Get
            End Property

            Public ReadOnly Property RcpHeader() As StrucElement
                Get
                    Return MyBase.GetItem(16)
                End Get
            End Property

            Public ReadOnly Property RcpFooter() As StrucElement
                Get
                    Return MyBase.GetItem(17)
                End Get
            End Property

            Public ReadOnly Property PriceLevel() As StrucElement
                Get
                    Return MyBase.GetItem(18)
                End Get
            End Property

            Public Function GetElement(ByVal Key As Integer) As StrucElement
                Return MyBase.GetItem(Key)
            End Function

        End Class
#End Region

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "BranchID, RevGroup, BranchName, BranchCode, AccNo, Address1, Address2, Address3, Address4, PostalCode, Contact1, Contact2, StoreType, Email, " & _
                            "Tel, Fax, Region, Country, State, Currency, StoreStatus, LEFT(OPTimeStart,2) + ':' + RIGHT(OPTimeStart,2) AS OPTimeStart, LEFT(OPTimeEnd,2) + ':' + RIGHT(OPTimeEnd,2) AS OPTimeEnd, OpDay1, OpDay2, OpDay3, OpDay4, " & _
                            "OpDay5, OpDay6, OpDay7, OpBookAlwDY, OpBookAlwHR, LEFT(OPBookFirst,2) + ':' + RIGHT(OPBookFirst,2) AS OPBookFirst, LEFT(OPBookLast,2) + ':' + RIGHT(OPBookLast,2) AS OPBookLast, OpBookIntv, SalesItemType, InSvcItemType, GenInSvcID, " & _
                            "RcpHeader, RcpFooter, PriceLevel, Flag"
                .CheckFields = "BranchID, StoreStatus, Flag"
                .TableName = "SYSBRANCH"
                .DefaultCond = "Flag=1"
                .DefaultOrder = String.Empty
                .Listing = "BranchID, RevGroup, BranchName, BranchCode, AccNo, Address1, Address2, Address3, Address4, PostalCode, Contact1, Contact2, StoreType, Email, " & _
                            "Tel, Fax, Region, Country, State, Currency, StoreStatus, LEFT(OPTimeStart,2) + ':' + RIGHT(OPTimeStart,2) AS OPTimeStart, LEFT(OPTimeEnd,2) + ':' + RIGHT(OPTimeEnd,2) AS OPTimeEnd, OpDay1, OpDay2, OpDay3, OpDay4, " & _
                            "OpDay5, OpDay6, OpDay7, OpBookAlwDY, OpBookAlwHR, LEFT(OPBookFirst,2) + ':' + RIGHT(OPBookFirst,2) AS OPBookFirst, LEFT(OPBookLast,2) + ':' + RIGHT(OPBookLast,2) AS OPBookLast, OpBookIntv, SalesItemType, InSvcItemType, GenInSvcID, " & _
                            "RcpHeader, RcpFooter, PriceLevel, Flag"
                .ListingCond = "Flag=1"
                .ShortList = String.Empty
                .ShortListCond = String.Empty
            End With
            With SysPref
                .FieldsList = "SYSKEY, SYSVALUE, SYSID"
                .CheckFields = " SYSID"
                .TableName = "SYSPREF"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "SYSKEY, SYSVALUE, SYSID"
                .ListingCond = " SYSID = 0"
                .ShortList = "SYSKEY, SYSVALUE, SYSID"
                .ShortListCond = " SYSID = 0"
            End With
            With SysPrefB
                .FieldsList = "BRANCHID, SYSKEY, SYSVALUE"
                .CheckFields = "BRANCHID"
                .TableName = "SYSPREFB"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "BRANCHID, SYSKEY, SYSVALUE"
                .ListingCond = String.Empty
                .ShortList = "BRANCHID"
                .ShortListCond = String.Empty
            End With
            ReDim MyFields(1)
            MyBase.AddMyField(0, "Branch ID", "BranchID", TypeCode.String)
            MyBase.AddMyField(1, "Revenue Group", "RevGroup", TypeCode.String)
        End Sub

        Private Function AssignItem(ByVal objBranch As Branch, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Dim arySQL As New ArrayList
            AssignItem = False
            Try
                If objBranch Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objBranch.BranchID) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
                        blnExec = True
                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    'record is found
                                    blnFound = True
                                    If Convert.ToInt16(.Item("Flag")) = 0 Then
                                        'found but deleted
                                        blnFlag = False
                                    Else
                                        'found and active
                                        blnFlag = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If
                    End If

                    If blnExec Then
                        If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                            'Item found & active
                            'system.Windows.Forms.MessageBox.Show("ab", "ABC",Windows.Forms.MessageBoxButtons.YesNo, Windows.Forms.MessageBoxIcon.Information,Windows.Forms.MessageBoxDefaultButton.Button2, Windows.Forms.MessageBoxOptions.DefaultDesktopOnly
                            'MsgBox("Record already exist", MsgBoxStyle.Critical, "Record Exist")
                            Return False
                        Else
                            With objSQL
                                .TableName = "SysBranch"
                                .AddField("RevGroup", objBranch.RevenueGroup, SQLControl.EnumDataType.dtString)
                                .AddField("BranchName", objBranch.BranchName, SQLControl.EnumDataType.dtStringN, SQLControl.EnumValidate.cEmpty)
                                .AddField("BranchCode", objBranch.BranchCode, SQLControl.EnumDataType.dtString)
                                .AddField("AccNo", objBranch.AccountNo, SQLControl.EnumDataType.dtString)
                                .AddField("Address1", objBranch.Address1, SQLControl.EnumDataType.dtStringN, SQLControl.EnumValidate.cEmpty)
                                .AddField("Address2", objBranch.Address2, SQLControl.EnumDataType.dtStringN, SQLControl.EnumValidate.cEmpty)
                                .AddField("Address3", objBranch.Address3, SQLControl.EnumDataType.dtStringN, SQLControl.EnumValidate.cEmpty)
                                .AddField("Address4", objBranch.Address4, SQLControl.EnumDataType.dtStringN, SQLControl.EnumValidate.cEmpty)
                                .AddField("PostalCode", objBranch.PostCode, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                                .AddField("Contact1", objBranch.Contact1, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                                .AddField("Contact2", objBranch.Contact2, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                                .AddField("StoreType", objBranch.StoreType, SQLControl.EnumDataType.dtString)
                                .AddField("Email", objBranch.Email, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                                .AddField("Tel", objBranch.TelNo, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                                .AddField("Fax", objBranch.FaxNo, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                                .AddField("Region", objBranch.Region, SQLControl.EnumDataType.dtStringN, SQLControl.EnumValidate.cEmpty)
                                .AddField("PostalCode", objBranch.PostCode, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                                .AddField("State", objBranch.State, SQLControl.EnumDataType.dtString)
                                .AddField("Country", objBranch.Country, SQLControl.EnumDataType.dtString)
                                .AddField("Currency", objBranch.Currency, SQLControl.EnumDataType.dtString)
                                .AddField("StoreStatus", objBranch.StoreStatus.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpTimeStart", objBranch.BizStartTime.ToString, SQLControl.EnumDataType.dtCustTime4)
                                .AddField("OpTimeEnd", objBranch.BizEndTime.ToString, SQLControl.EnumDataType.dtCustTime4)
                                .AddField("OpDay1", objBranch.OperatingDay1.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpDay2", objBranch.OperatingDay2.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpDay3", objBranch.OperatingDay3.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpDay4", objBranch.OperatingDay4.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpDay5", objBranch.OperatingDay5.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpDay6", objBranch.OperatingDay6.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpDay7", objBranch.OperatingDay7.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpBookAlwDy", objBranch.BookDayAllow.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpBookAlwHr", objBranch.BookHourAllow.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpBookFirst", objBranch.BookStartTime.ToShortTimeString, SQLControl.EnumDataType.dtCustTime4)
                                .AddField("OpBookLast", objBranch.BookEndTime.ToShortTimeString, SQLControl.EnumDataType.dtCustTime4)
                                .AddField("SalesItemType", objBranch.SalesType, SQLControl.EnumDataType.dtString)
                                .AddField("InSvcItemType", objBranch.InServiceType, SQLControl.EnumDataType.dtString)
                                .AddField("GenInSvcID", objBranch.GenerateInServiceID.ToString, SQLControl.EnumDataType.dtString)
                                .AddField("RcpHeader", objBranch.ReceiptHeader, SQLControl.EnumDataType.dtStringN, SQLControl.EnumValidate.cEmpty)
                                .AddField("RcpFooter", objBranch.ReceiptFooter, SQLControl.EnumDataType.dtStringN, SQLControl.EnumValidate.cEmpty)
                                .AddField("PriceLevel", objBranch.PriceLevel.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", "1", SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                            "WHERE BranchID ='" & .ParseValue(SQLControl.EnumDataType.dtString, objBranch.BranchID) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("BranchID", objBranch.BranchID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                             "WHERE BranchID ='" & .ParseValue(SQLControl.EnumDataType.dtString, objBranch.BranchID) & "'")
                                End Select
                                arySQL.Add(strSQL)
                            End With

                            If pType = SQLControl.EnumSQLType.stInsert And blnFound = False Then
                                SaveSysCode(objBranch, arySQL)
                            End If

                            If SaveSysPref(objBranch.BranchID, objBranch.BranchSysPref, _
                            objBranch.BranchSysSet, objBranch.BranchSetting, arySQL) = True Then
                                If objDCom.BatchExecute(arySQL, CommandType.Text, True) = True Then
                                    Return True
                                Else
                                    Return False
                                End If
                            End If
                        End If
                    End If
                End If

            Catch exApply As Exception
                Throw exApply
                Return False
            Finally
                objBranch = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        '''Add-----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="objCust"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 	[Developer] 	2004/03/17	Created
        ''' </history>
        '''-----------------------------------------------------------------------------
        Overloads Function Add(ByVal objBranch As Branch) As Boolean
            Return AssignItem(objBranch, SQLControl.EnumSQLType.stInsert)
        End Function

        Overloads Function Add(ByVal objBranch As Branch, ByVal DataConn As DataAccess) As Boolean
            Return AssignItem(objBranch, SQLControl.EnumSQLType.stInsert)
        End Function

        '''AMEND-----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="objCust"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 	[Developer] 	2004/03/17	Created
        ''' </history>
        '''-----------------------------------------------------------------------------
        Function Amend(ByVal objBranch As Branch) As Boolean
            Return AssignItem(objBranch, SQLControl.EnumSQLType.stUpdate)
        End Function

        '''Delete-----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="objCust"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 	[Developer] 	2004/03/17	Created
        ''' </history>
        '''-----------------------------------------------------------------------------
        Function Delete(ByVal objBranch As Branch) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim arrList As New ArrayList
            Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If objBranch Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objBranch.BranchID) & "'")
                        End With
                        'strSQL = "SELECT CustomerID, Status, Flag, Inuse FROM Customer WHERE CustomerID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCust.CustomerID) & "'"
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    'If Convert.ToInt16(.Item("InUse")) = 1 Then
                                    '    blnInUse = True
                                    'End If
                                End If
                                .Close()
                            End With
                        End If

                        If blnFound = False Then
                            'Error Message
                            Return False
                        End If
                        If blnFound = True Then
                            With objSQL
                                .AddField("Flag", "0", SQLControl.EnumDataType.dtNumeric)
                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, myinfo.TableName, "BranchID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objBranch.BranchID) & "'")
                                arrList.Add(strSQL)
                            End With
                            strSQL = BuildDelete(SysPrefB.TableName, "BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objBranch.BranchID) & "'")
                            arrList.Add(strSQL)
                            strSQL = BuildDelete("SYSCODEB", "BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objBranch.BranchID) & "'")
                            arrList.Add(strSQL)
                        End If
                        'If blnFound = True Then
                        '    strSQL = BuildDelete(MyInfo.TableName, " Bra = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCust.CustomerID) & "'")
                        '    ' strSQL = "DELETE FROM Customer WHERE CustomerID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCust.CustomerID) & "'"
                        'End If
                        Try
                            'execute
                            objDCom.BatchExecute(arrList, CommandType.Text, True)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objBranch = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Private Function SaveSysPref(ByVal BranchID As String, _
                        ByVal BranchSysPref As DataTable, _
                        ByVal BranchSysSet As DataTable, _
                        ByVal BranchSetting As Branch.StrucBranchSetting, ByRef arySQL As ArrayList) As Boolean
            Dim drRow As DataRow
            Dim strSQL, strCond As String
            Dim objBuild As StringBuilder
            
            Try
                objBuild = New StringBuilder
                With objBuild
                    .Append(" BranchID='")
                    .Append(objSQL.ParseValue(SQLControl.EnumDataType.dtString, BranchID))
                    .Append("'")
                    strCond = .ToString
                End With

                strSQL = BuildDelete(SysPrefB.TableName, strCond)
                arySQL.Add(strSQL)

                If BranchSysSet Is Nothing = False Then
                    For Each drRow In BranchSysSet.Rows
                        With objSQL
                            .TableName = SysPrefB.TableName
                            .AddField("BranchID", BranchID, SQLControl.EnumDataType.dtString)
                            .AddField("SysKey", Convert.ToString(drRow.Item("SYSKEY")), SQLControl.EnumDataType.dtString)
                            .AddField("SysValue", Convert.ToString(drRow.Item("SYSVALUE")), SQLControl.EnumDataType.dtString)
                            .AddField("SysSet", Convert.ToString(1), SQLControl.EnumDataType.dtNumeric)
                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                            arySQL.Add(strSQL)
                        End With
                    Next
                End If

                If BranchSysPref Is Nothing = False Then
                    For Each drRow In BranchSysPref.Rows
                        With objSQL
                            .TableName = SysPrefB.TableName
                            .AddField("BranchID", BranchID, SQLControl.EnumDataType.dtString)
                            .AddField("SysKey", Convert.ToString(drRow.Item("SYSKEY")), SQLControl.EnumDataType.dtString)
                            .AddField("SysValue", Convert.ToString(drRow.Item("SYSVALUE")), SQLControl.EnumDataType.dtString)
                            .AddField("SysSet", Convert.ToString(0), SQLControl.EnumDataType.dtNumeric)
                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                            arySQL.Add(strSQL)
                        End With
                    Next
                End If

                'BranchSetting
                With BranchSetting
                    SaveSetting(BranchID, "ANALYSISIMAGEPATH", BranchSetting.AnalysisImagepath, arySQL)
                    SaveSetting(BranchID, "DEFCOUNTRY", BranchSetting.DefCountry, arySQL)
                    SaveSetting(BranchID, "DEFCURRENCY", BranchSetting.DefCurrency, arySQL)
                    SaveSetting(BranchID, "MAXSTOCK", BranchSetting.MaxStock, arySQL)
                    SaveSetting(BranchID, "MINSTOCK", BranchSetting.MinStock, arySQL)
                    SaveSetting(BranchID, "NEGATIVESTOCK", BranchSetting.NegativeStock, arySQL)
                    SaveSetting(BranchID, "NORMAL", BranchSetting.Normal, arySQL)
                    SaveSetting(BranchID, "REORDERLVL", BranchSetting.ReOrderLvl, arySQL)
                    'SaveSetting(BranchID, "DEFCASH", BranchSetting.DefCash.ToString, arySQL)
                    'SaveSetting(BranchID, "DEFCREDIT", BranchSetting.DefCredit.ToString, arySQL)
                    'SaveSetting(BranchID, "DEFPREPAID", BranchSetting.DefPrepaid.ToString, arySQL)
                End With

                Return True
            Catch errSave As Exception
                Return Nothing
            Finally
                objBuild = Nothing
                If BranchSysPref Is Nothing = False Then BranchSysPref.Clear()
                If BranchSysPref Is Nothing = False Then BranchSysPref.Clear()
            End Try
        End Function

        Private Sub SaveSetting(ByVal BranchID As String, _
            ByVal SysKey As String, _
            ByVal SysValue As String, ByRef arySQL As ArrayList)
            If SysValue Is Nothing Then SysValue = String.Empty
            strSQL = "EXEC sp_AddBranchSetting '" & _
            String.Concat(BranchID, "',") & _
            String.Concat("'", SysKey, "','", SysValue, "'")
            arySQL.Add(strSQL)
        End Sub

        Private Sub SaveSysCode(ByVal objBranch As Branch, ByRef arySQL As ArrayList)
            Dim strSQL As String
            Dim rdr As SqlClient.SqlDataReader

            Try
                If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                    StartSQLControl()
                    With MyInfo
                        strSQL = BuildDelete("SYSCODEB", "BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objBranch.BranchID) & "'")
                        arySQL.Add(strSQL)
                        strSQL = BuildSelect("SysCode, SysDesc, Prefix, SpCode, RunNo, NoLength, NoPos, Postfix, SysID, CheckFormat", "SYSCODE")
                    End With
                    rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                    If rdr Is Nothing = False Then
                        With rdr
                            While .Read
                                'record is found
                                objSQL.TableName = " SYSCODEB"
                                objSQL.AddField("BranchID", objBranch.BranchID, SQLControl.EnumDataType.dtString)
                                objSQL.AddField("SysCode", Convert.ToString(.Item("SysCode")), SQLControl.EnumDataType.dtString)
                                objSQL.AddField("SysDesc", Convert.ToString(.Item("SysDesc")), SQLControl.EnumDataType.dtStringN)
                                objSQL.AddField("Prefix", Convert.ToString(.Item("Prefix")), SQLControl.EnumDataType.dtString)
                                objSQL.AddField("SpCode", Convert.ToString(.Item("SpCode")), SQLControl.EnumDataType.dtString)
                                objSQL.AddField("RunNo", Convert.ToString(.Item("RunNo")), SQLControl.EnumDataType.dtNumeric)
                                objSQL.AddField("NoLength", Convert.ToString(.Item("NoLength")), SQLControl.EnumDataType.dtNumeric)
                                objSQL.AddField("NoPos", Convert.ToString(.Item("NoPos")), SQLControl.EnumDataType.dtNumeric)
                                objSQL.AddField("Postfix", Convert.ToString(.Item("Postfix")), SQLControl.EnumDataType.dtNumeric)
                                objSQL.AddField("LastUpdate", objBranch.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                objSQL.AddField("UpdateBy", objBranch.UpdateBy, SQLControl.EnumDataType.dtString)
                                objSQL.AddField("Status", "1", SQLControl.EnumDataType.dtNumeric)
                                objSQL.AddField("SysID", Convert.ToString(.Item("SysID")), SQLControl.EnumDataType.dtNumeric)
                                objSQL.AddField("CheckFormat", Convert.ToString(.Item("CheckFormat")), SQLControl.EnumDataType.dtString)
                                strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert)
                                arySQL.Add(strSQL)
                            End While
                            .Close()
                        End With
                    End If
                End If
            Catch ex As Exception
            Finally
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try

        End Sub

#Region "Data Selection"
        Public Overloads Function Enquiry(ByVal DCom As DataAccess) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.FieldsList, .TableName)
                    strSQL = strSQL & " WHERE Flag = 1"
                    Return CType(DCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetPriceLevel(ByVal BranchID As String) As Integer
            Dim strSQL As String
            Dim intPriceLvl As Integer = 0
            Dim rdr As SqlClient.SqlDataReader
            Try
                If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                    StartSQLControl()
                    With MyInfo
                        strSQL = BuildSelect("PriceLevel", .TableName, "BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BranchID) & "'")
                    End With
                    rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                    If rdr Is Nothing = False Then
                        With rdr
                            If .Read Then
                                intPriceLvl = Convert.ToInt32(.Item("PriceLevel"))
                            End If
                            .Close()
                        End With
                    End If
                End If
                Return intPriceLvl
            Catch ex As Exception
                Return 0
            Finally
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'Enquiry
        Public Overloads Function Enquiry(Optional ByVal SQLstmt As String = Nothing) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    If SQLstmt = Nothing Or SQLstmt = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName)
                        strSQL = strSQL & " WHERE Flag = 1"
                    Else
                        strSQL = SQLstmt
                    End If
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetListing() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetShortList(ByVal SettingOnly As Boolean) As DataSet
            Dim strField, strCond As String
            If StartConnection() = True Then
                With SysPref
                    If SettingOnly Then
                        strField = "SYSVALUE, SYSKEY, SYSDESC"
                        strCond = String.Concat(.ShortListCond, " AND SYSSet=1")
                    Else
                        strField = .ShortList
                        strCond = String.Concat(.ShortListCond, " AND SYSSet=0")
                    End If
                    strSQL = BuildSelect(strField, .TableName, strCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetBranchSetting(ByVal BranchID As String) As Branch.StrucBranchSetting
            Dim dtTemp As DataTable
            Dim drItem As DataRow
            Dim MyBranchSetting As Branch.StrucBranchSetting
            Try
                If StartConnection() = True Then
                    strSQL = BuildSelect("SYSKey, SYSValue", "SYSPREFB", "SYSSet = 2 AND BranchID = '" & BranchID & "'")
                    dtTemp = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, SysPrefB.TableName), DataTable)
                    If dtTemp Is Nothing = False Then
                        For Each drItem In dtTemp.Rows
                            With MyBranchSetting
                                Select Case CType(drItem.Item(0), String)
                                    Case "ANALYSISIMAGEPATH"
                                        .AnalysisImagepath = CType(drItem(1), String)
                                    Case "DEFCOUNTRY"
                                        .DefCountry = CType(drItem(1), String)
                                    Case "DEFCURRENCY"
                                        .DefCurrency = CType(drItem(1), String)
                                    Case "MAXSTOCK"
                                        .MaxStock = CType(drItem(1), String)
                                    Case "MINSTOCK"
                                        .MinStock = CType(drItem(1), String)
                                    Case "NEGATIVESTOCK"
                                        .NegativeStock = CType(drItem(1), String)
                                    Case "NORMAL"
                                        .Normal = CType(drItem(1), String)
                                    Case "REORDERLVL"
                                        .ReOrderLvl = CType(drItem(1), String)
                                End Select
                            End With
                        Next
                    End If
                    Return MyBranchSetting
                End If
            Catch ex As Exception
                Return Nothing
            Finally
                EndConnection()
                EndSQLControl()
            End Try
        End Function

        Public Function GetEditSysPref(ByVal BranchID As String) As DataTable
            Dim dtPrefB, dtPref As DataTable
            Dim drRow, drFind As DataRow

            Try
                If StartConnection() = True Then
                    StartSQLControl()

                    strSQL = "SELECT SYSValue, SYSKey, SYSDesc  FROM SYSPREF WHERE  SYSKey NOT IN (SELECT SYSKey FROM SYSPREFB WHERE BranchID = '" & BranchID & "'  AND SYSSet=0) AND SYSID = 0 AND SYSSet=0" & _
                         " UNION SELECT SYSValue, SYSKey, '' AS SYSDesc  FROM SYSPREFB WHERE BranchID = '" & BranchID & "' AND SYSSet = 0 ORDER BY SYSKey"

                    dtPrefB = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, ), DataTable)

                    strSQL = "SELECT SysKey, SysValue, SysDesc FROM SYSPREF WHERE SysID = 0 AND SYSSet = 2 ORDER BY SYSKey"
                    dtPref = objDCom.Execute(strSQL, CommandType.Text, True)
                Else
                    Return Nothing
                End If

                For Each drRow In dtPrefB.Rows
                    drFind = objSQL.SelectRow(dtPref, "SysKey = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Convert.ToString(drRow("SysKey"))) & "'")
                    If drFind Is Nothing = False Then
                        With drRow
                            .BeginEdit()
                            .Item("SysDesc") = drFind("SysDesc")
                            .EndEdit()
                        End With
                    End If
                    drFind = Nothing
                Next
                Return dtPrefB

            Catch ex As Exception
                Return Nothing
            Finally
                EndConnection()
                EndSQLControl()
            End Try
        End Function

        Public Function GetEditSysSet(ByVal BranchID As String) As DataTable
            Dim dtPrefSetB, dtPrefSet As DataTable
            Dim drRow, drFind As DataRow
            Try
                StartSQLControl()
                StartConnection()

                strSQL = "SELECT SYSValue, SYSKey, SYSDesc  FROM SYSPREF WHERE  SYSKey NOT IN (SELECT SYSKey FROM SYSPREFB WHERE BranchID = '" & BranchID & "'  AND SYSSet=1) AND SYSID = 0 AND SYSSet=1" & _
                         " UNION SELECT SYSValue, SYSKey, '' AS SysDesc  FROM SYSPREFB WHERE BranchID = '" & BranchID & "' AND SYSSet = 1 ORDER BY SYSKey"

                dtPrefSetB = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, ), DataTable)

                strSQL = "SELECT SysValue, SysKey, SysDesc FROM SYSPREF WHERE SysID =0 AND SYSSet =1 ORDER BY SYSKey"
                dtPrefSet = objDCom.Execute(strSQL, CommandType.Text, True)

                For Each drRow In dtPrefSetB.Rows
                    drFind = objSQL.SelectRow(dtPrefSet, "SysKey = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Convert.ToString(drRow("SysKey"))) & "'")
                    If drFind Is Nothing = False Then
                        With drRow
                            .BeginEdit()
                            .Item("SysDesc") = drFind("SysDesc")
                            .EndEdit()
                        End With
                    End If
                    drFind = Nothing
                Next
                Return dtPrefSetB

            Catch ex As Exception
                Return Nothing
            Finally
                EndConnection()
                EndSQLControl()
            End Try

        End Function

        Public Function IsExisted(ByVal BranchID As String) As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Dim blnFound As Boolean
            strSQL = "SELECT * FROM  SYSBRANCH WHERE BRANCHID='" & BranchID & "'"
            Try
                rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
                If rdr Is Nothing = False Then
                    With rdr
                        If .Read Then
                            blnFound = True
                        End If

                    End With
                End If
            Catch ex As Exception
                If rdr Is Nothing = False Then rdr.Close()
            Finally
                If rdr Is Nothing = False Then
                    rdr.Close()
                    rdr = Nothing
                End If
            End Try
            Return blnFound
        End Function

        Public Function GetBranchList(Optional ByVal RevGroup As String = "OL") As DataTable
            Try
                StartConnection()
                StartSQLControl()

                strSQL = "SELECT BranchID, BranchName FROM SYSBRANCH " & _
                        "WHERE Flag = 1 AND RevGroup = '" & RevGroup & "'" & _
                        "ORDER BY BranchID "

                Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, "SYSRPTBRANCH"), DataSet).Tables(0)

            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region
    End Class
#End Region

#Region "Terminal Base"
    Public Class TerminalBase
        Inherits Core.CoreBase
        Dim SysPref As StrucClassInfo
        Dim SysPrefT As StrucClassInfo
        Dim SysBranch As StrucClassInfo

        Public Enum EnumCheckInOut
            CheckIn = 0
            CheckOut = 1
        End Enum

#Region "Terminal class in Terminal Base"
        Public Class Terminal
            Inherits Core.SingleBase
            Protected strBranchID, strTerminalID As String
            Protected intTransRun, intInServRun, intActive As Integer
            Protected dtLastUpd As DateTime
            Protected dtTermSysPref, dtRcpHeader, dtRcpFooter As DataTable
            Protected strSeqH, strDescH, strCode As String
            Private dtSetting As DataTable
            Public Scheme As New MyScheme
            Public Structure StrucTermSetting
                Dim DefCNExpiryDay As Integer
                Dim DefCustomer As String
                Dim DefServer As String
                Dim JobSheetCopies As Integer
                Dim ReceiptCopies As Integer
                Dim RoundingAmt As Integer
                Dim TaxGroup As String
                Dim ZReadHeaderCopies As Integer
                Dim LogoPosition As Integer
                Dim PrintJobSheet As Integer
                Dim PrintReceiptList As Integer
            End Structure
            Private MySetting As StrucTermSetting

            Public Overloads Property TermSetting() As StrucTermSetting
                Get
                    Return MySetting
                End Get
                Set(ByVal Value As StrucTermSetting)
                    MySetting = Value
                End Set
            End Property

            Public Property TermSysPref() As DataTable
                Get
                    Return dtTermSysPref
                End Get
                Set(ByVal Value As DataTable)
                    dtTermSysPref = Value
                End Set
            End Property

            Public Property RcpHeader() As DataTable
                Get
                    Return dtRcpHeader
                End Get
                Set(ByVal Value As DataTable)
                    dtRcpHeader = Value
                End Set
            End Property

            Public Property RcpFooter() As DataTable
                Get
                    Return dtRcpFooter
                End Get
                Set(ByVal Value As DataTable)
                    dtRcpFooter = Value
                End Set
            End Property

            Public Overloads Property TermSysSet() As DataTable
                Get
                    Return dtSetting
                End Get
                Set(ByVal Value As DataTable)
                    dtSetting = Value
                End Set
            End Property

            Public Property SeqH() As String
                Get
                    Return strSeqH
                End Get
                Set(ByVal Value As String)
                    strSeqH = Value
                End Set
            End Property

            Public Property DescH() As String
                Get
                    Return strDescH
                End Get
                Set(ByVal Value As String)
                    strDescH = Value
                End Set
            End Property

            Public Property Code() As String
                Get
                    Return (strCode)
                End Get
                Set(ByVal Value As String)
                    strCode = Value
                End Set
            End Property
            Public Property BranchID() As String
                Get
                    Return strBranchID
                End Get
                Set(ByVal Value As String)
                    strBranchID = Value
                End Set
            End Property

            Public Property TerminalID() As String
                Get
                    Return strTerminalID
                End Get
                Set(ByVal Value As String)
                    strTerminalID = Value
                End Set
            End Property

            Public Property TransRun() As Integer
                Get
                    Return intTransRun
                End Get
                Set(ByVal Value As Integer)
                    intTransRun = Value
                End Set
            End Property

            Public Property InServRun() As Integer
                Get
                    Return intInServRun
                End Get
                Set(ByVal Value As Integer)
                    intInServRun = Value
                End Set
            End Property

            Public Property Active() As Integer
                Get
                    Return intActive
                End Get
                Set(ByVal Value As Integer)
                    If Value < 0 Then
                        intActive = 0
                    Else
                        intActive = Value
                    End If
                End Set
            End Property

            Public Property LastUpdate() As DateTime
                Get
                    Return dtLastUpd
                End Get
                Set(ByVal Value As DateTime)
                    If Value = Nothing Then
                        dtLastUpd = Now()
                    Else
                        dtLastUpd = Value
                    End If
                End Set
            End Property
        End Class
#End Region

#Region "Terminal Scheme"
        Public Class MyScheme
            Inherits Core.SchemeBase

            Protected Overrides Sub InitializeInfo()
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "BranchID"
                    .Length = 20
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(0, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "TerminalID"
                    .Length = 10
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(1, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "CurTransRun"
                    .Length = 8
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(2, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "CurInServRun"
                    .Length = 8
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(3, this)
            End Sub

            Public ReadOnly Property BranchID() As StrucElement
                Get
                    Return MyBase.GetItem(0)
                End Get
            End Property

            Public ReadOnly Property TerminalID() As StrucElement
                Get
                    Return MyBase.GetItem(1)
                End Get
            End Property

            Public ReadOnly Property CurTransRun() As StrucElement
                Get
                    Return MyBase.GetItem(2)
                End Get
            End Property

            Public ReadOnly Property CurInServRun() As StrucElement
                Get
                    Return MyBase.GetItem(3)
                End Get
            End Property

            Public Function GetElement(ByVal key As Integer) As StrucElement
                Return MyBase.GetItem(key)
            End Function

        End Class

#End Region

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "BRANCHID, TERMID, BUSINESSDATE, PREVBIZDATE, CURTRANSRUN, CURINSERVRUN, LASTTRANSRUN, LASTTRANSNO, STATUS, FLAG"
                .CheckFields = " STATUS, FLAG"
                .TableName = "SYSTERM"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "BRANCHID, TERMID, BUSINESSDATE, PREVBIZDATE, CURTRANSRUN, CURINSERVRUN, LASTTRANSRUN, LASTTRANSNO, STATUS, FLAG"
                .ListingCond = "FLAG = 1"
                .ShortList = "BRANCHID, TERMID, BUSINESSDATE, PREVBIZDATE, CURTRANSRUN, CURINSERVRUN, LASTTRANSRUN, LASTTRANSNO, STATUS, FLAG"
                .ShortListCond = "STATUS=1 AND FLAG=1"
            End With
            With SysPref
                .FieldsList = "SYSKEY, SYSDESC, SYSVALUE"
                .CheckFields = "SYSID"
                .TableName = "SYSPREF"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = " SYSDESC, SYSVALUE, SYSKEY, SYSID"
                .ListingCond = " SYSID = 1"
                .ShortList = "SYSKEY, SYSDESC, SYSVALUE"
                .ShortListCond = " SYSID = 1"
            End With
            With SysPrefT
                .FieldsList = "BRANCHID, TERMID, SYSKEY, SYSVALUE"
                .CheckFields = "BRANCHID, TERMID"
                .TableName = "SYSPREFT"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "BRANCHID, TERMID, SYSKEY, SYSVALUE"
                .ListingCond = String.Empty
                .ShortList = "BRANCHID, TERMID"
                .ShortListCond = String.Empty
            End With
            With SysBranch
                .FieldsList = "BranchID, BranchName, Address1, Address2, Address3, Address4, RcpHeader, RcpFooter"
                .CheckFields = String.Empty
                .TableName = "SYSBRANCH"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "BranchID, BranchName, Address1, Address2, Address3, Address4, RcpHeader, RcpFooter"
                .ListingCond = String.Empty
                .ShortList = "BRANCHID"
                .ShortListCond = String.Empty
            End With
            ReDim MyFields(1)
            MyBase.AddMyField(0, "Branch ID", "BranchID", TypeCode.String)
            MyBase.AddMyField(1, "Terminal ID", "TERMID", TypeCode.String)
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function AssignItem(ByVal objTerminal As Terminal, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Dim arySQL As New ArrayList
            Dim drRow As DataRow

            AssignItem = False
            Try
                If objTerminal Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False

                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTerminal.BranchID) & "' AND TERMID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTerminal.TerminalID) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        blnExec = True

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("Flag")) = 0 Then
                                        'Found but deleted
                                        blnFlag = False
                                    Else
                                        'Found and active
                                        blnFlag = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If
                    End If

                    If blnExec Then
                        If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                            Return False
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = "SYSTERM"
                                .AddField("CurTransRun", objTerminal.TransRun.ToString.Trim, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CurInServRun", objTerminal.InServRun.ToString.Trim, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Status", objTerminal.Active.ToString.Trim, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", cFlagActive.ToString, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                            "WHERE BranchID='" & .ParseValue(SQLControl.EnumDataType.dtString, objTerminal.BranchID) & "' AND " & _
                                            "TermID = '" & .ParseValue(SQLControl.EnumDataType.dtString, objTerminal.TerminalID) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("BranchID", objTerminal.BranchID, SQLControl.EnumDataType.dtString)
                                                .AddField("TermID", objTerminal.TerminalID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                        "WHERE BranchID='" & .ParseValue(SQLControl.EnumDataType.dtString, objTerminal.BranchID) & "' AND " & _
                                        "TermID = '" & .ParseValue(SQLControl.EnumDataType.dtString, objTerminal.TerminalID) & "'")
                                End Select
                                arySQL.Add(strSQL)
                            End With

                            If SaveSysPref(objTerminal.BranchID, objTerminal.TerminalID, _
                            objTerminal.TermSysPref, objTerminal.TermSysSet, objTerminal.TermSetting, arySQL) = True Then
                                Try
                                    'execute
                                    objDCom.BatchExecute(arySQL, CommandType.Text, True)
                                Catch axExecute As Exception
                                    If pType = SQLControl.EnumSQLType.stInsert Then
                                        Throw New ApplicationException("210002")
                                    Else
                                        Throw New ApplicationException("210004")
                                    End If
                                Finally
                                    objSQL.Dispose()
                                End Try
                            End If

                            If SaveRcpData(objTerminal.BranchID, objTerminal.RcpHeader, objTerminal.RcpFooter, arySQL) = True Then
                                Try
                                    objDCom.BatchExecute(arySQL, CommandType.Text, True)
                                Catch ex As Exception
                                    If pType = SQLControl.EnumSQLType.stInsert Then
                                        Throw New ApplicationException("210002")
                                    Else
                                        Throw New ApplicationException("210004")
                                    End If
                                Finally
                                    objSQL.Dispose()
                                End Try
                            End If

                            Return True
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objTerminal = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
        'ADD
        Function Add(ByVal objTerminal As Terminal) As Boolean
            Return AssignItem(objTerminal, SQLControl.EnumSQLType.stInsert)
        End Function
        'AMEND
        Function Amend(ByVal objTerminal As Terminal) As Boolean
            Return AssignItem(objTerminal, SQLControl.EnumSQLType.stUpdate)
        End Function
        'DELETE
        Function Delete(ByVal objTerminal As Terminal) As Boolean
            Dim strSQL As String
            Dim arrList As New ArrayList
            Dim blnFound, blnInUse As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If objTerminal Is Nothing Then
                    'error message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTerminal.BranchID) & "' AND TERMID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTerminal.TerminalID) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                End If
                                .Close()
                            End With
                        End If

                        'record is found and in use
                        If blnFound = True Then
                            With objSQL
                                .AddField("Flag", Convert.ToString(0).Trim, SQLControl.EnumDataType.dtNumeric)
                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, myinfo.TableName, "BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTerminal.BranchID) & _
                                                   "' AND TERMID =" & objTerminal.TerminalID)
                                arrList.Add(strSQL)

                            End With

                            'strSQL = BuildUpdate(MyInfo.TableName, "SET Flag = " & cFlagNonActive & _
                            '         " WHERE BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTerminal.BranchID) & _
                            '         "' AND TERMID =" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, objTerminal.TerminalID))
                            'arrList.Add(strSQL)
                            strSQL = BuildDelete(SysPrefT.TableName, "BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTerminal.BranchID) & _
                                    "'  AND TERMID =" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, objTerminal.TerminalID))
                            arrList.Add(strSQL)
                        End If
                        Try
                            'execute
                            objDCom.BatchExecute(arrList, CommandType.Text, True)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try
                    End If
                End If
            Catch axDElete As ApplicationException
                Throw axDElete
            Catch exDelete As SystemException
                Throw exDelete
            Finally
                objTerminal = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Private Function SaveSysPref(ByVal BranchID As String, _
            ByVal TerminalID As String, ByVal TermSysPref As DataTable, _
            ByVal TermSysSet As DataTable, _
            ByVal TermSetting As Terminal.StrucTermSetting, ByRef arySQL As ArrayList) As Boolean
            Dim drRow As DataRow
            Dim strSQL, strCond As String
            Dim objBuild As StringBuilder
            Try
                objBuild = New StringBuilder
                With objBuild
                    .Append(" BranchID='")
                    .Append(objSQL.ParseValue(SQLControl.EnumDataType.dtString, BranchID))
                    .Append("' AND TermID ='")
                    .Append(objSQL.ParseValue(SQLControl.EnumDataType.dtString, TerminalID))
                    .Append("'")
                    strCond = .ToString
                End With
                strSQL = BuildDelete(SysPrefT.TableName, strCond)
                arySQL.Add(strSQL)

                If TermSysSet Is Nothing = False Then
                    For Each drRow In TermSysSet.Rows
                        With objSQL
                            .TableName = SysPrefT.TableName
                            .AddField("BranchID", BranchID, SQLControl.EnumDataType.dtString)
                            .AddField("TermID", TerminalID, SQLControl.EnumDataType.dtString)
                            .AddField("SysKey", Convert.ToString(drRow.Item("SYSKEY")), SQLControl.EnumDataType.dtString)
                            .AddField("SysValue", Convert.ToString(drRow.Item("SYSVALUE")), SQLControl.EnumDataType.dtString)
                            .AddField("SysSet", Convert.ToString(1), SQLControl.EnumDataType.dtNumeric)
                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                            arySQL.Add(strSQL)
                        End With
                    Next
                End If

                If TermSysPref Is Nothing = False Then
                    For Each drRow In TermSysPref.Rows
                        With objSQL
                            .TableName = SysPrefT.TableName
                            .AddField("BranchID", BranchID, SQLControl.EnumDataType.dtString)
                            .AddField("TermID", TerminalID, SQLControl.EnumDataType.dtString)
                            .AddField("SysKey", Convert.ToString(drRow.Item("SYSKEY")), SQLControl.EnumDataType.dtString)
                            .AddField("SysValue", Convert.ToString(drRow.Item("SYSVALUE")), SQLControl.EnumDataType.dtString)
                            .AddField("SysSet", Convert.ToString(0), SQLControl.EnumDataType.dtNumeric)
                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                            arySQL.Add(strSQL)
                        End With
                    Next
                End If

                With TermSetting
                    SaveSetting(BranchID, TerminalID, "DEFCUSTOMER", .DefCustomer, arySQL)
                    SaveSetting(BranchID, TerminalID, "DEFSERVER", .DefServer, arySQL)
                    SaveSetting(BranchID, TerminalID, "DEFCNEXPIRYDAY", Convert.ToString(.DefCNExpiryDay), arySQL)
                    SaveSetting(BranchID, TerminalID, "JOBSHEETCOPIES", .JobSheetCopies.ToString, arySQL)
                    SaveSetting(BranchID, TerminalID, "RECEIPTCOPIES", .ReceiptCopies.ToString, arySQL)
                    SaveSetting(BranchID, TerminalID, "ROUNDINGAMT", .RoundingAmt.ToString, arySQL)
                    SaveSetting(BranchID, TerminalID, "TAXGROUP", .TaxGroup, arySQL)
                    SaveSetting(BranchID, TerminalID, "ZREADHEADERCOPIES", .ZReadHeaderCopies.ToString, arySQL)
                    SaveSetting(BranchID, TerminalID, "LOGOPOSITION", .LogoPosition.ToString, arySQL)
                    SaveSetting(BranchID, TerminalID, "PRINTJOBSHEET", .PrintJobSheet.ToString, arySQL)
                End With

                Return True
            Catch errSave As Exception
                Return Nothing
            Finally
                objBuild = Nothing
                If TermSysPref Is Nothing = False Then TermSysPref.Clear()
                'If TermSysPref Is Nothing = False Then TermSysPref.Clear()
            End Try
        End Function

        Private Sub SaveSetting(ByVal BranchID As String, _
            ByVal TerminalID As String, ByVal SysKey As String, _
            ByVal SysValue As String, ByRef arySQL As ArrayList)
            If SysValue Is Nothing Then SysValue = String.Empty
            strSQL = "EXEC sp_AddTermSetting '" & _
            String.Concat(BranchID, "',", TerminalID) & _
            String.Concat(",'", SysKey, "','", SysValue, "'")
            arySQL.Add(strSQL)
        End Sub

        Private Function SaveRcpData(ByVal BranchID As String, ByVal RcpHeader As DataTable, ByVal RcpFooter As DataTable, ByRef arySQL As ArrayList) As Boolean
            Try
                If RcpHeader Is Nothing = False Then
                    With objSQL
                        .TableName = SysBranch.TableName
                        '.AddField("BranchID", BranchID, SQLControl.EnumDataType.dtString)
                        .AddField("BranchName", Convert.ToString(RcpHeader.Rows(1).Item(0)), SQLControl.EnumDataType.dtString)
                        .AddField("Address1", Convert.ToString(RcpHeader.Rows(2).Item(0)), SQLControl.EnumDataType.dtString)
                        .AddField("Address2", Convert.ToString(RcpHeader.Rows(3).Item(0)), SQLControl.EnumDataType.dtString)
                        .AddField("Address3", Convert.ToString(RcpHeader.Rows(4).Item(0)), SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                        .AddField("Address4", Convert.ToString(RcpHeader.Rows(5).Item(0)), SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                        .AddField("RcpHeader", Convert.ToString(RcpHeader.Rows(0).Item(0)), SQLControl.EnumDataType.dtString)
                        .AddField("RcpFooter", Convert.ToString(RcpFooter.Rows(0).Item(0)), SQLControl.EnumDataType.dtString)
                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE BranchID = '" & BranchID & "'")
                        arySQL.Add(strSQL)
                    End With
                    Return True
                End If
            Catch ex As Exception
                Return False
            End Try
        End Function
#End Region

#Region "Data Selection"
        'Enquiry
        Public Overloads Function Enquiry(Optional ByVal SQLstmt As String = Nothing) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    If SQLstmt = Nothing Or SQLstmt = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName)
                    Else
                        strSQL = SQLstmt
                    End If
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function Enquiry(ByVal DCom As DataAccess) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.FieldsList, .TableName)
                    strSQL = strSQL & " WHERE Flag = 1"
                    Return CType(DCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetListing() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetShortList(ByVal SettingOnly As Boolean) As DataTable
            Dim strField, strCond As String
            If StartConnection() = True Then
                With SysPref
                    If SettingOnly Then
                        strField = "SYSVALUE, SYSKEY, SYSDESC"
                        strCond = String.Concat(.ShortListCond, " AND SYSSet=1")
                    Else
                        strField = .ShortList
                        strCond = String.Concat(.ShortListCond, " AND SYSSet=0")
                    End If
                    strSQL = BuildSelect(strField, .TableName, strCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), DataTable)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetTermSetting(ByVal BranchID As String, ByVal TerminalID As String) As Terminal.StrucTermSetting
            Dim dtTemp As DataTable
            Dim drItem As DataRow
            Dim MySetting As Terminal.StrucTermSetting
            Try
                If StartConnection() = True Then
                    strSQL = BuildSelect("SYSKey, SYSValue", " SYSPREFT", "SYSSet=2 AND BranchID = '" & _
                             BranchID & "' AND TermID = " & TerminalID)
                    dtTemp = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, SysPref.TableName), DataTable)
                    If dtTemp Is Nothing = False Then
                        For Each drItem In dtTemp.Rows
                            With MySetting
                                Select Case CType(drItem.Item(0), String)
                                    Case "DEFCUSTOMER"
                                        .DefCustomer = CType(drItem(1), String)
                                    Case "DEFSERVER"
                                        .DefServer = CType(drItem(1), String)
                                    Case "DEFCNEXPIRYDAY"
                                        .DefCNExpiryDay = CType(drItem(1), Integer)
                                    Case "TAXGROUP"
                                        .TaxGroup = CType(drItem(1), String)
                                    Case "JOBSHEETCOPIES"
                                        .JobSheetCopies = CType(drItem(1), Integer)
                                    Case "RECEIPTCOPIES"
                                        .ReceiptCopies = CType(drItem(1), Integer)
                                    Case "ZREADHEADERCOPIES"
                                        .ZReadHeaderCopies = CType(drItem(1), Integer)
                                    Case "ROUNDINGAMT"
                                        .RoundingAmt = CType(drItem(1), Integer)
                                    Case "LOGOPOSITION"
                                        .LogoPosition = CType(drItem(1), Integer)
                                    Case "PRINTJOBSHEET"
                                        .PrintJobSheet = CType(drItem(1), Integer)
                                End Select
                            End With
                        Next
                    End If
                    Return MySetting
                End If
            Catch ex As Exception
                Return Nothing
            Finally
                EndConnection()
                EndSQLControl()
            End Try
        End Function

        Public Function GetEditSysPref(ByVal BranchID As String, ByVal TerminalID As String) As DataTable
            Dim dtPrefT, dtPref As DataTable
            Dim drRow, drFind As DataRow
            Try
                StartSQLControl()
                StartConnection()
                strSQL = "SELECT SYSKey, SYSDesc, SYSValue FROM SYSPREF WHERE  SYSKey NOT IN (SELECT SYSKey FROM SYSPREFT WHERE BranchID = '" & BranchID & "' AND TermID =" & TerminalID & ") AND SYSID = 1 AND SYSSet=0" & _
                         " UNION SELECT SYSKey, '' AS SysDesc, SYSValue FROM SYSPREFT WHERE BranchID = '" & BranchID & "' AND SYSSet = 0 AND TermID =" & TerminalID & " ORDER BY SYSKey"
                dtPrefT = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, ), DataTable)

                strSQL = "SELECT SysKey, SysValue, SysDesc FROM SYSPREF WHERE SysID = 1 AND SYSSet = 0 ORDER BY SYSKey"
                dtPref = objDCom.Execute(strSQL, CommandType.Text, True)


                For Each drRow In dtPrefT.Rows
                    drFind = objSQL.SelectRow(dtPref, "SysKey = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Convert.ToString(drRow("SysKey"))) & "'")
                    If drFind Is Nothing = False Then
                        With drRow
                            .BeginEdit()
                            .Item("SysDesc") = drFind("SysDesc")
                            .EndEdit()
                        End With
                    End If
                    drFind = Nothing
                Next
                Return dtPrefT

            Catch ex As Exception
                Return Nothing
            Finally
                EndConnection()
                EndSQLControl()
            End Try
        End Function

        Public Function GetEditSysSet(ByVal BranchID As String, ByVal TerminalID As String) As DataTable
            Dim dtPrefSetT, dtPrefSet As DataTable
            Dim drRow, drFind As DataRow
            Try
                StartSQLControl()
                StartConnection()
                strSQL = "SELECT SYSValue, SYSKey, SYSDesc  FROM SYSPREF WHERE  SYSKey NOT IN (SELECT SYSKey FROM SYSPREFT WHERE BranchID = '" & BranchID & _
                         "' AND TermID =" & TerminalID & " AND SYSSet=1) AND SYSID = 1 AND SYSSet=1 " & _
                         " UNION SELECT SYSValue, SYSKey, '' AS SysDesc  FROM SYSPREFT WHERE BranchID = '" & BranchID & "' AND SYSSet = 1 AND TermID =" & TerminalID & " ORDER BY SYSKey"
                dtPrefSetT = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, ), DataTable)

                strSQL = "SELECT SysValue, SysKey, SysDesc FROM SYSPREF WHERE SysID =1 AND SYSSet =1 ORDER BY SYSKey"
                dtPrefSet = objDCom.Execute(strSQL, CommandType.Text, True)

                For Each drRow In dtPrefSetT.Rows
                    drFind = objSQL.SelectRow(dtPrefSet, "SysKey = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Convert.ToString(drRow("SysKey"))) & "'")
                    If drFind Is Nothing = False Then
                        With drRow
                            .BeginEdit()
                            .Item("SysDesc") = drFind("SysDesc")
                            .EndEdit()
                        End With
                    End If
                    drFind = Nothing
                Next
                Return dtPrefSetT

            Catch ex As Exception
                Return Nothing
            Finally
                EndConnection()
                EndSQLControl()
            End Try
        End Function

        Public Function getRcpData(ByVal strBranchID As String) As DataTable
            Dim dtRcpData As DataTable

            Try
                StartSQLControl()
                StartConnection()

                strSQL = "SELECT BranchName, Address1, Address2, Address3, Address4, RcpHeader, RcpFooter FROM SYSBRANCH WHERE BranchID = '" & strBranchID & "'"
                dtRcpData = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, ), DataTable)
                Return dtRcpData
            Catch ex As Exception
                Return Nothing
            Finally
                EndConnection()
                EndSQLControl()
            End Try
        End Function
#End Region

#Region "Setup Receipt Header / Footer"
        Public Function AmendSequence(ByVal dsCode As DataSet, ByVal intSeq As Integer, ByVal Codetype As String) As Boolean
            Dim strsql As String
            Dim drData As DataRow
            Dim arySQL As ArrayList
            Try
                If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                    StartSQLControl()
                    If intSeq = 0 Then
                        With objSQL
                            .TableName = "CODEMASTER"
                            .AddField("CodeSeq", Convert.ToString(0), SQLControl.EnumDataType.dtNumeric)
                            strsql = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE Codetype ='" & Codetype & "'")
                        End With
                        objDCom.Execute(strsql, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                    Else
                        arySQL = New ArrayList
                        With objSQL
                            For Each drData In dsCode.Tables(0).Rows
                                .TableName = "CODEMASTER"
                                .AddField("CodeSeq", Convert.ToString(drData(2)), SQLControl.EnumDataType.dtNumeric)
                                arySQL.Add(.BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE Codetype ='" & Codetype & "' AND Code='" & Convert.ToString(drData(0)) & "'"))
                            Next
                            objDCom.BatchExecute(arySQL, CommandType.Text)
                        End With
                    End If
                End If
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function
#End Region
        Public Shared Function CheckOutStatus(ByVal BranchID As String, ByVal TermID As Int32, Optional ByVal StartLoadForm As Boolean = False) As Boolean
            Dim strSql As String
            Dim rdrCheck As SqlClient.SqlDataReader
            Dim blnOwnTermCheck As Boolean = False
            Dim blnTermCheck As Boolean = False

            Try
                StartSQLControl()
                StartConnection()

                strSql = "SELECT TermID, ApptCheckOut FROM SysTerm WHERE BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BranchID) & "'"
                rdrCheck = CType(objDCom.Execute(strSql, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                If rdrCheck Is Nothing = False Then
                    While rdrCheck.Read
                        If Convert.ToInt32(rdrCheck("TermID")) = TermID Then
                            If Convert.ToInt32(rdrCheck("ApptCheckOut")) = 1 Then
                                blnOwnTermCheck = True
                            Else
                                blnOwnTermCheck = False
                            End If
                        Else
                            If blnTermCheck = False Then
                                If Convert.ToInt32(rdrCheck("ApptCheckOut")) = 1 Then
                                    blnTermCheck = True
                                Else
                                    blnTermCheck = False
                                End If
                            End If
                        End If
                    End While
                    rdrCheck.Close()
                End If
                If StartLoadForm = True Then
                    Select Case True
                        Case (blnOwnTermCheck = True And blnTermCheck = False)
                            Return True
                        Case (blnOwnTermCheck = False And blnTermCheck = False)
                            Return False
                        Case (blnOwnTermCheck = False And blnTermCheck = True)
                            Return False
                        Case (blnOwnTermCheck = True And blnTermCheck = True)
                            Return False
                    End Select
                Else
                    Select Case True
                        Case (blnOwnTermCheck = True And blnTermCheck = False)
                            Return False
                        Case (blnOwnTermCheck = False And blnTermCheck = False)
                            Return False
                        Case (blnOwnTermCheck = False And blnTermCheck = True)
                            Return True
                        Case (blnOwnTermCheck = True And blnTermCheck = True)
                            Return True
                    End Select
                End If
            Catch ex As Exception
                Return False
            Finally
                rdrCheck = Nothing
                EndConnection()
                EndSQLControl()
            End Try
        End Function

        Public Shared Function UpdateCheckStatus(ByVal BranchID As String, ByVal TermID As Int32, ByVal CheckType As General.TerminalBase.EnumCheckInOut) As Boolean
            Dim strSql As String
            Try
                StartSQLControl()
                StartConnection()

                With objSQL
                    If CheckType = EnumCheckInOut.CheckIn Then
                        .AddField("ApptCheckOut", "0", SQLControl.EnumDataType.dtNumeric)
                    End If

                    If CheckType = EnumCheckInOut.CheckOut Then
                        .AddField("ApptCheckOut", "1", SQLControl.EnumDataType.dtNumeric)
                    End If

                    strSql = .BuildSQL(SQLControl.EnumSQLType.stUpdate, "SysTerm", "BranchID = '" & BranchID & "' AND TermID = " & TermID.ToString)
                    objDCom.Execute(strSql, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                    Return True
                End With
            Catch ex As Exception
                Return False
            Finally
                EndConnection()
                EndSQLControl()
            End Try
        End Function

        Public Shared Function UpdateBackgroundStatus(ByVal BranchID As String, ByVal TermID As Int32, ByVal Enable As Boolean) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean = False
            Dim rdr As SqlClient.SqlDataReader
            Dim sysValue As String
            Try
                StartSQLControl()
                StartConnection()

                With objSQL
                    strSQL = BuildSelect("SysValue", "SysPrefT", "Syskey = 'APPLYCUSTOMBACKGROUND' AND BranchID = '" & BranchID & "' AND TermID = " & TermID.ToString)
                    rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
                    If rdr Is Nothing = False Then
                        If rdr.Read Then
                            blnFound = True
                        End If
                        rdr.Close()
                    End If

                    If Enable = True Then
                        .AddField("SysValue", "1", SQLControl.EnumDataType.dtString)
                        sysValue = "1"
                    ElseIf Enable = False Then
                        .AddField("SysValue", "0", SQLControl.EnumDataType.dtString)
                        sysValue = "0"
                    End If


                    If blnFound = True Then
                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, "SysPrefT", "Syskey = 'APPLYCUSTOMBACKGROUND' AND BranchID = '" & BranchID & "' AND TermID = " & TermID.ToString)
                    Else
                        strSQL = "INSERT INTO SYSPREFT (SYSKEY, BranchID, TermID, SysValue) VALUES ('APPLYCUSTOMBACKGROUND','" & BranchID & "','" & TermID.ToString & "', '" & sysValue & "')"
                    End If
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                    Return True
                End With
            Catch ex As Exception
                Return False
            Finally
                EndConnection()
                EndSQLControl()
            End Try
        End Function

    End Class

#End Region

#Region "Report Base"
    Public Class ReportBase
        Inherits Core.CoreBase
        Dim RptTypeInfo As StrucClassInfo
        Dim RptField As StrucClassInfo

 

#Region "Report class in Report Base"
        Public Class Report
            Inherits Core.SingleBase
            Protected strAppID, strRptCode, strRptName, strRptDesc As String
            Protected intRptType, intRptSection As Integer
            Protected strBranchID As String
            Protected dtRptBranch As DataTable

            Public Myscheme As New Myscheme

            Public Property AppID() As String
                Get
                    Return strAppID
                End Get
                Set(ByVal Value As String)
                    strAppID = Value
                    MyBase.AddItem(0, strAppID)
                End Set
            End Property

            Public Property RptCode() As String
                Get
                    Return strRptCode
                End Get
                Set(ByVal Value As String)
                    strRptCode = Value
                    MyBase.AddItem(1, strRptCode)
                End Set
            End Property

            Public Property RptName() As String
                Get
                    Return strRptName
                End Get
                Set(ByVal Value As String)
                    strRptName = Value
                    MyBase.AddItem(2, strRptName)
                End Set
            End Property

            Public Property RptDesc() As String
                Get
                    Return strRptDesc
                End Get
                Set(ByVal Value As String)
                    strRptDesc = Value
                    MyBase.AddItem(3, strRptDesc)
                End Set
            End Property

            Public Property RptType() As Integer
                Get
                    Return intRptType
                End Get
                Set(ByVal Value As Integer)
                    intRptType = Value
                    MyBase.AddItem(4, intRptType)
                End Set
            End Property

            Public Property RptSection() As Integer
                Get
                    Return intRptSection
                End Get
                Set(ByVal Value As Integer)
                    intRptSection = Value
                    MyBase.AddItem(5, intRptSection)
                End Set
            End Property

            Public Property BranchID() As String
                Get
                    Return strBranchID
                End Get
                Set(ByVal Value As String)
                    strBranchID = Value
                End Set
            End Property

            Public Property SelectedReport() As DataTable
                Get
                    Return dtRptBranch
                End Get
                Set(ByVal Value As DataTable)
                    dtRptBranch = Value
                End Set
            End Property

            Public Sub SetRptBranchTable(ByRef dtRptBranch As DataTable)
                If dtRptBranch Is Nothing = True Then
                    dtRptBranch = New DataTable
                End If
                dtRptBranch.Clear()
                dtRptBranch.Columns.Clear()
                dtRptBranch.Columns.Add("ShowBranch", Type.GetType("System.String"))
                dtRptBranch.Columns.Add("BranchID", Type.GetType("System.String"))
            End Sub

        End Class
#End Region

#Region "Report Scheme"
        Public Class MyScheme
            Inherits Core.SchemeBase

            Protected Overrides Sub InitializeInfo()
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "AppID"
                    .Length = 10
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(0, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "RptCode"
                    .Length = 30
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(1, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "RptName"
                    .Length = 30
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(2, this)
            End Sub

            Public ReadOnly Property AppID() As StrucElement
                Get
                    Return MyBase.GetItem(0)
                End Get
            End Property

            Public ReadOnly Property RptCode() As StrucElement
                Get
                    Return MyBase.GetItem(1)
                End Get
            End Property

            Public ReadOnly Property RptName() As StrucElement
                Get
                    Return MyBase.GetItem(2)
                End Get
            End Property

            Public Function GetElement(ByVal Key As Integer) As StrucElement
                Return MyBase.GetItem(Key)
            End Function


        End Class
#End Region

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "APPID, RPTCODE, RPTNAME, RPTDESC, RPTTYPE, RPTSECTION"
                .CheckFields = String.Empty
                .TableName = "SYSREPORT"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "APPID, RPTCODE, RPTNAME, RPTDESC, RPTTYPE, RPTSECTION"
                .ListingCond = String.Empty
                .ShortList = "APPID, RPTCODE, RPTNAME"
                .ShortListCond = String.Empty
            End With
            With RptTypeInfo
                .FieldsList = "RPTTYPE, TYPEDESC"
                .CheckFields = String.Empty
                .TableName = "SYSRPTTYPE"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "RPTTYPE, TYPEDESC"
                .ListingCond = String.Empty
                .ShortList = "RPTTYPE, TYPEDESC"
                .ShortListCond = String.Empty
            End With
            With RptField
                .FieldsList = "APPID, RPTCODE, RptDBField, RPTFIELD, FieldType, FieldDef, FieldAttb"
                .CheckFields = String.Empty
                .TableName = "SYSRPTFIELD"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "APPID, RPTCODE, RptDBField, RPTFIELD"
                .ListingCond = String.Empty
                .ShortList = "RptDBField, RPTFIELD, RPTCODE, FieldType, FieldDef, FieldAttb"
                .ShortListCond = String.Empty
            End With
            ReDim MyFields(1)
            MyBase.AddMyField(0, "Report Code", "RPTCODE", TypeCode.String)
            MyBase.AddMyField(1, "Report Name", "RPTNAME", TypeCode.String)
        End Sub

#Region "Data Selection"
        'Function Enquiry(Optional ByVal SQLstmt As String = Nothing) As DataSet
        '    If StartConnection() = True Then
        '        With MyInfo
        '            If SQLstmt = Nothing Or SQLstmt = String.Empty Then
        '                strSQL = BuildSelect(.FieldsList, .TableName)
        '            Else
        '                strSQL = SQLstmt
        '            End If
        '            Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
        '        End With
        '    Else
        '        Return Nothing
        '    End If
        'End Function

        Public Function GetReportList(ByVal AppID As Integer, ByVal Section As String, Optional ByVal SQLstmt As String = Nothing) As DataSet
            Dim strCond As String
            If StartConnection() = True Then
                'If Section = (0).ToString Or Section = String.Empty Or Section = Nothing Then
                '    strCond = " AND AppID=" & AppID
                'Else
                '    strCond = " AND AppID=" & AppID & " AND RPTSection IN(" & Section & ")"
                'End If
                If Section = (0).ToString Or Section = String.Empty Or Section = Nothing Then
                    strCond = " WHERE AppID=" & AppID
                Else
                    strCond = " WHERE AppID=" & AppID & " AND RPTSection IN(" & Section & ")"
                End If

                With MyInfo
                    If SQLstmt = Nothing Or SQLstmt = String.Empty Then
                        strSQL = BuildSelect(.Listing, .TableName, strCond)
                    Else
                        strSQL = SQLstmt & strCond
                    End If
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetShortList() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetReportField(ByVal AppID As Integer, ByVal RPTCode As String) As DataTable
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With RptField
                        strSQL = BuildSelect(.ShortList, .TableName, "RPTCode ='" & RPTCode & "' AND APPID=" & AppID.ToString)
                        Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), DataTable)
                    End With
                End If
            Catch ex As Exception
                Return Nothing
            Finally
                EndConnection()
                EndSQLControl()
            End Try
        End Function

        Public Overloads Function GetReportField(ByVal RPTCode As String) As DataTable
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With RptField
                        strSQL = BuildSelect(.ShortList, .TableName, "RPTCode ='" & RPTCode & "'")
                        Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), DataTable)
                    End With
                End If
            Catch ex As Exception
                Return Nothing
            Finally
                EndConnection()
                EndSQLControl()
            End Try
        End Function

        Public Overloads Function GetReportType(ByVal RPTCode As String) As DataTable
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With MyInfo
                        strSQL = BuildSelect(.ShortList, .TableName, "RPTCode ='" & RPTCode & "'")
                        Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), DataTable)
                    End With
                End If
            Catch ex As Exception
                Return Nothing
            Finally
                EndConnection()
                EndSQLControl()
            End Try
        End Function

        Public Overloads Function GetReportField(ByVal AppID As Integer) As DataTable
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With RptField

                        strSQL = BuildSelect(.ShortList, .TableName, "APPID=" & AppID.ToString)
                        Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), DataTable)
                    End With
                End If
            Catch ex As Exception
                Return Nothing
            Finally
                EndConnection()
                EndSQLControl()
            End Try
        End Function

        Public Function GetRptTypeDesc(ByVal RptType As Integer) As String
            Dim rdr As SqlClient.SqlDataReader
            Try
                StartConnection()
                StartSQLControl()
                With RptTypeInfo
                    strSQL = BuildSelect(.ShortList, .TableName, "RPTTYPE =" & RptType)
                    rdr = CType(objDCom.Execute(strSQL, CommandType.Text, CommandBehavior.SingleRow, False), SqlClient.SqlDataReader)
                End With

                If rdr Is Nothing = False Then
                    If rdr.Read Then
                        GetRptTypeDesc = Convert.ToString(rdr("TYPEDESC"))
                        rdr.Close()
                    Else
                        rdr.Close()
                        Return Nothing
                    End If
                End If

            Catch ex As Exception

            Finally
                rdr = Nothing
                EndConnection()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetSearchField(ByVal Table As String, ByVal Field As String) As DataTable
            Try
                If StartConnection() = True Then
                    StartSQLControl()


                    strSQL = BuildSelect(Field, Table, , Field)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, Table), DataTable)

                End If
            Catch ex As Exception
                Return Nothing
            Finally
                EndConnection()
                EndSQLControl()
            End Try
        End Function


        Public Function GetReportsFilter(ByVal BranchID As String) As DataSet
            Try
                StartConnection()
                StartSQLControl()

                strSQL = "SELECT BranchID, BranchName FROM SYSBRANCH " & _
                        "WHERE BranchID NOT IN (SELECT ShowBranch FROM SYSRPTBRANCH " & _
                        "WHERE BranchID = '" & BranchID & "') " & _
                        "ORDER BY BranchID "

                Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, "SYSRPTBRANCH"), DataSet)

            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function GetRptBranch(ByVal BranchID As String) As DataSet
            Try
                StartConnection()
                StartSQLControl()

                strSQL = "SELECT SYSRPTBRANCH.ShowBranch, SYSBRANCH.BranchName " & _
                        "FROM SYSBRANCH INNER JOIN SYSRPTBRANCH ON SYSBRANCH.BranchID = SYSRPTBRANCH.ShowBranch " & _
                        "WHERE SYSRPTBRANCH.BranchID = '" & BranchID & "' " & _
                        "ORDER BY SYSRPTBRANCH.ShowBranch "

                Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, "SYSRPTBRANCH"), DataSet)

            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function AmendRptBranch(ByVal objRpt As Report) As Boolean
            Dim arySQL As ArrayList
            Dim drRow As DataRow
            Dim i As Integer
            Dim strSQL As String
            AmendRptBranch = False
            Try
                StartConnection()
                StartSQLControl()

                arySQL = New ArrayList
                arySQL.Add("DELETE FROM SYSRPTBRANCH WHERE BranchID='" & Replace(objRpt.BranchID, "'", "''") & "'")

                If objRpt.SelectedReport Is Nothing = False Then
                    For Each drRow In objRpt.SelectedReport.Rows
                        With objSQL
                            .TableName = "SYSRPTBRANCH"
                            .AddField("ShowBranch", Convert.ToString(drRow.Item(0)), SQLControl.EnumDataType.dtString)
                            .AddField("BranchID", objRpt.BranchID, SQLControl.EnumDataType.dtString)
                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                            arySQL.Add(strSQL)
                        End With
                    Next
                End If
                If objDCom.BatchExecute(arySQL, CommandType.Text) = True Then
                    Return True
                End If

            Catch exAmend As Exception
                Throw New SystemException(exAmend.Message)
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

#End Region
    End Class
#End Region

#Region "Payment Base"
    Public Class PaymentBase
        Inherits Core.CoreBase

#Region "Payment Class in Payment Base"
        Public Class Payment
            Inherits Core.SingleBase
            Protected dtInstall As DataTable

            Public Property Installment() As DataTable
                Get
                    Return dtInstall
                End Get
                Set(ByVal Value As DataTable)
                    dtInstall = Value
                End Set
            End Property

        End Class

#End Region
    End Class
#End Region

#Region "Privilege Base"

    Public Class PrivilegeBase
        Inherits Core.CoreBase

#Region "Privilege class in Privilege Base"
        Public Class Privilege
            Protected strCode, strDesc, strUpdBy As String
            Protected intActive, intPriceLevel, intType As Integer
            Protected dtLastUpd As DateTime

            Public Property Code() As String
                Get
                    Return strCode
                End Get
                Set(ByVal PriCode As String)
                    If PriCode.Trim = String.Empty Then
                        Throw New Exception("Please key in Privilege Code")
                    Else
                        strCode = PriCode
                    End If
                End Set
            End Property

            Public Property Description() As String
                Get
                    Return strDesc
                End Get
                Set(ByVal Value As String)
                    strDesc = Value
                End Set
            End Property

            Public Property Type() As Integer
                Get
                    Return intType
                End Get
                Set(ByVal Value As Integer)
                    intType = Value
                End Set
            End Property


            Public Property LastUpdate() As DateTime
                Get
                    Return dtLastUpd
                End Get
                Set(ByVal Value As DateTime)
                    dtLastUpd = Value
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return strUpdBy
                End Get
                Set(ByVal Value As String)
                    strUpdBy = Value
                End Set
            End Property

            Public Property Active() As Integer
                Get
                    Return intActive
                End Get
                Set(ByVal Value As Integer)
                    intActive = Value
                End Set
            End Property

            Public Property PriceLevel() As Integer
                Get
                    Return intPriceLevel
                End Get
                Set(ByVal Value As Integer)
                    intPriceLevel = Value
                End Set
            End Property
        End Class
#End Region

#Region "Privilege Scheme"
        Public Class MyScheme
            Inherits Core.SchemeBase

            Protected Overrides Sub InitializeInfo()
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "PrivilegeCode"
                    .Length = 10
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(0, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "PrivilegeDesc"
                    .Length = 50
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(1, this)
            End Sub

            Public ReadOnly Property Code() As StrucElement
                Get
                    Return MyBase.GetItem(0)
                End Get
            End Property

            Public ReadOnly Property Description() As StrucElement
                Get
                    Return MyBase.GetItem(1)
                End Get
            End Property

            Public Function GetElement(ByVal Key As Integer) As StrucElement
                Return MyBase.GetItem(Key)
            End Function

        End Class

#End Region

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "PrivilegeCode, PrivilegeDesc, PrivilegeType, PriceLevel, LASTUPDATE, UPDATEBY, ACTIVE, INUSE, FLAG"
                .CheckFields = "INUSE, FLAG"
                .TableName = "Privilege"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "PrivilegeCode, PrivilegeDesc, PrivilegeType,PriceLevel, LASTUPDATE, UPDATEBY, ACTIVE, INUSE, FLAG"
                .ListingCond = "FLAG = 1"
                .ShortList = "PrivilegeCode, PrivilegeDesc"
                .ShortListCond = String.Empty
            End With
            ReDim MyFields(1)
            MyBase.AddMyField(0, "Privilege Code", "PrivilegeCode", TypeCode.String)
            MyBase.AddMyField(1, "Description", "PrivilegeDesc", TypeCode.String)
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function AssignItem(ByVal objPri As Privilege, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As SqlClient.SqlDataReader
            AssignItem = False
            Try
                If objPri Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "PrivilegeCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, objPri.Code) & "' and PrivilegeType = '" & objPri.Type & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
                        blnExec = True

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("Flag")) = 0 Then
                                        'Found but deleted
                                        blnFlag = False
                                    Else
                                        'Found and active
                                        blnFlag = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If
                    End If

                    If blnExec Then
                        If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                            'Item found & active
                            'system.Windows.Forms.MessageBox.Show("ab", "ABC",Windows.Forms.MessageBoxButtons.YesNo, Windows.Forms.MessageBoxIcon.Information,Windows.Forms.MessageBoxDefaultButton.Button2, Windows.Forms.MessageBoxOptions.DefaultDesktopOnly
                            'MsgBox("Record already exist", MsgBoxStyle.Critical, "Record Exist")
                            Return False
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = "Privilege"
                                .AddField("PrivilegeDesc", objPri.Description, SQLControl.EnumDataType.dtStringN)
                                .AddField("PrivilegeType", objPri.Type.ToString, SQLControl.EnumDataType.dtString)
                                .AddField("PriceLevel", objPri.PriceLevel.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("UpdateBy", objPri.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", objPri.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                .AddField("Active", objPri.Active.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", cFlagActive.ToString, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                            "WHERE PrivilegeCode='" & .ParseValue(SQLControl.EnumDataType.dtNumeric, objPri.Code) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("privilegeCode", objPri.Code, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                        "WHERE privilegeCode='" & .ParseValue(SQLControl.EnumDataType.dtString, objPri.Code) & "'")
                                End Select
                            End With
                            Try
                                'execute
                                objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002")
                                Else
                                    Throw New ApplicationException("210004")
                                End If
                            End Try
                            Return True
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objPri = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
        'ADD
        Public Overloads Function Add(ByVal objPri As Privilege) As Boolean
            Return AssignItem(objPri, SQLControl.EnumSQLType.stInsert)
        End Function
        'AMEND
        Public Overloads Function Amend(ByVal objPri As Privilege) As Boolean
            Return AssignItem(objPri, SQLControl.EnumSQLType.stUpdate)
        End Function
        'DELETE
        Public Overloads Function Delete(ByVal objPri As Privilege) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If objPri Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "PrivilegeCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPri.Code) & "'")
                        End With
                        'strSQL = "SELECT PrivilegeCode, Flag, InUse FROM CustPrivilege WHERE PrivilegeCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPri.Name) & "'"
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("InUse")) = 1 Then
                                        blnInUse = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If

                        'record is found and in use
                        If blnFound = True And blnInUse = True Then
                            strSQL = BuildUpdate(MyInfo.TableName, " SET Flag = " & cFlagNonActive & _
                                " , LastUpdate = '" & objPri.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPri.UpdateBy) & "'" & _
                                " WHERE privilegeCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPri.Code) & "'")
                        End If

                        'record exist but not in use
                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(MyInfo.TableName, "privilegeCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPri.Code) & "'")
                        End If
                        Try
                            'execute
                            objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try
                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As SystemException
                Throw exDelete
            Finally
                objPri = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"

        'Enquiry
        Function Enquiry(Optional ByVal SQLstmt As String = Nothing) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    If SQLstmt = Nothing Or SQLstmt = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName)
                    Else
                        strSQL = SQLstmt
                    End If
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Function EnquiryByType(ByVal Type As Integer) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.FieldsList, .TableName, "PrivilegeType = '" & Type & "' AND Flag = 1")
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetListing() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetShortList() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function
#End Region

        'Clear all data in database
        Public Function ClearDB() As Boolean
        End Function
    End Class
#End Region

#Region "Privilege Item Base"
    Public Class PrivilegeItemBase
        Inherits Core.CoreBase

#Region "PrivilegeItem class in Privilege Base"
        Public Class PrivilegeItem
            Protected strPrivCode, strItemCode, strSalesDisCode, strUpdBy As String
            Protected intItemType, intDiscountType As Integer
            Protected dblDiscValue As Double
            Protected dtLastUpd As DateTime

            Public Property PrivilegeCode() As String
                Get
                    Return strPrivCode
                End Get
                Set(ByVal PriName As String)
                    strPrivCode = PriName
                End Set
            End Property

            Public Property ItemCode() As String
                Get
                    Return strItemCode
                End Get
                Set(ByVal Value As String)
                    strItemCode = Value
                End Set
            End Property

            Public Property SalesDiscountCode() As String
                Get
                    Return strSalesDisCode
                End Get
                Set(ByVal Value As String)
                    strSalesDisCode = Value
                End Set
            End Property

            Public Property LastUpdate() As DateTime
                Get
                    Return dtLastUpd
                End Get
                Set(ByVal Value As DateTime)
                    dtLastUpd = Value
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return strUpdBy
                End Get
                Set(ByVal Value As String)
                    strUpdBy = Value
                End Set
            End Property

            Public Property ItemType() As Integer
                Get
                    Return intItemType
                End Get
                Set(ByVal Value As Integer)
                    intItemType = Value
                End Set
            End Property

            Public Property DiscountType() As Integer
                Get
                    Return intDiscountType
                End Get
                Set(ByVal Value As Integer)
                    intDiscountType = Value
                End Set
            End Property

            Public Property DiscountValue() As Double
                Get
                    Return dblDiscValue
                End Get
                Set(ByVal Value As Double)
                    dblDiscValue = Value
                End Set
            End Property
        End Class
#End Region

#Region "Privilege Item Scheme"
        Public Class MyScheme
            Inherits Core.SchemeBase

            Protected Overrides Sub InitializeInfo()
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "StkDiscountValue"
                    .Length = 8
                    .DecPlace = 2
                    .RegExp = String.Empty
                    .IsMandatory = False
                    .AllowNegative = False
                End With
                MyBase.AddItem(0, this)
            End Sub

            Public ReadOnly Property Value() As StrucElement
                Get
                    Return MyBase.GetItem(0)
                End Get
            End Property

            Public Function GetElement(ByVal Key As Integer) As StrucElement
                Return MyBase.GetItem(Key)
            End Function

        End Class

#End Region

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "PrivItemCode, PrivItemType, StkDiscountType, StkDiscountValue, SalesDiscountCode, LASTUPDATE, UPDATEBY"
                .CheckFields = "*"
                .TableName = "PrivItem"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "PrivilegeCode, PrivItemCode, PrivItemType, StkDiscountType, StkDiscountValue, SalesDiscountCode, LASTUPDATE, UPDATEBY"
                .ListingCond = String.Empty
                .ShortList = String.Empty
                .ShortListCond = String.Empty
            End With
        End Sub

#Region "Data Manipulation-Add,Edit,Del"

        Private Function AssignItem(ByVal objPriItem As PrivilegeItem, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim blnExec As Boolean, blnFound As Boolean
            Dim rdr As SqlClient.SqlDataReader
            AssignItem = False
            Try
                If objPriItem Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False

                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "PrivilegeCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPriItem.PrivilegeCode) & "'" & _
                                    " AND PrivitemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPriItem.ItemCode) & "'")
                        End With
                        'strSQL = "SELECT PrivItemType,StkDiscountType,StkDiscountValue, " & _
                        '"SalesDiscountCode,LastUpdate, UpdateBy FROM CustPrivItem " & _
                        '"WHERE PrivilegeCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPriItem.PrivilegeCode) & "'" & _
                        '" AND PrivitemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPriItem.ItemCode) & "'"
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        blnExec = True
                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                End If
                                .Close()
                            End With
                        End If
                    End If

                    If blnExec Then
                        If blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Return False
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = "PrivItem"
                                .AddField("PrivItemType", objPriItem.ItemType.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("StkDiscountType", objPriItem.DiscountType.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("StkDiscountValue", objPriItem.DiscountValue.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("UpdateBy", objPriItem.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", objPriItem.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                .AddField("SalesDiscountCode", objPriItem.SalesDiscountCode, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                            "WHERE PrivilegeCode='" & .ParseValue(SQLControl.EnumDataType.dtString, objPriItem.PrivilegeCode) & "'" & _
                                            " AND PrivItemCode='" & .ParseValue(SQLControl.EnumDataType.dtString, objPriItem.ItemCode) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("privilegeCode", objPriItem.PrivilegeCode, SQLControl.EnumDataType.dtString)
                                                .AddField("privItemCode", objPriItem.ItemCode, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                            "WHERE PrivilegeCode='" & .ParseValue(SQLControl.EnumDataType.dtString, objPriItem.PrivilegeCode) & "'" & _
                                            " AND PrivItemCode='" & .ParseValue(SQLControl.EnumDataType.dtString, objPriItem.ItemCode) & "'")
                                End Select
                            End With
                            Try
                                'execute
                                objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002")
                                Else
                                    Throw New ApplicationException("210004")
                                End If
                            Finally
                                objSQL.Dispose()
                            End Try
                            Return True
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objPriItem = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
        'ADD
        Public Overloads Function Add(ByVal objPriItem As PrivilegeItem) As Boolean
            Return AssignItem(objPriItem, SQLControl.EnumSQLType.stInsert)
        End Function
        'AMEND
        Public Overloads Function Amend(ByVal objPriItem As PrivilegeItem) As Boolean
            Return AssignItem(objPriItem, SQLControl.EnumSQLType.stUpdate)
        End Function
        'DELETE
        Public Overloads Function Delete(ByVal objPriItem As PrivilegeItem) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean

            Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False

            Try
                If objPriItem Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "PrivilegeCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPriItem.PrivilegeCode) & "'" & _
                        " AND PrivItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPriItem.ItemCode) & "'")
                        End With
                        'strSQL = "SELECT PrivItemType,StkDiscountType,StkDiscountValue,SalesDiscountCode " & _
                        '"FROM CustPrivItem WHERE PrivilegeCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPriItem.PrivilegeCode) & "'" & _
                        '" AND PrivItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPriItem.ItemCode) & "'"
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                End If
                                .Close()
                            End With
                        End If

                        If blnFound = False Then
                            'Error Message
                            Return False
                        End If

                        If blnFound = True Then
                            strSQL = BuildDelete(MyInfo.TableName, "PrivilegeCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPriItem.PrivilegeCode) & "'" & _
                            " AND PrivItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPriItem.ItemCode) & "'")
                            'strSQL = "DELETE FROM CustPrivItem WHERE PrivilegeCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPriItem.PrivilegeCode) & "'" & _
                            '" AND PrivItemCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPriItem.ItemCode) & "'"
                        End If
                        Try
                            'execute
                            objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try
                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As SystemException
                Throw exDelete
            Finally
                objPriItem = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"

        'Enquiry
        Function Enquiry(ByVal strPriVCode As String, Optional ByVal SQLstmt As String = Nothing) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    If SQLstmt = Nothing Or SQLstmt = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName, "PrivilegeCode = '" & strPriVCode & "'")
                    Else
                        strSQL = SQLstmt
                    End If
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        'Public Function Enquiry(Optional ByVal EnqSet As EnumPrivSet = EnumPrivSet.PrivilegeHdr) As DataSet
        '    Dim strSQL As String
        '    If blnConnected = True Then
        '        With objDCom
        '            If .OpenConnection() = True Then
        '                Select Case EnqSet
        '                    Case EnumPrivSet.PrivilegeHdr
        '                        strSQL = "SELECT PrivilegeCode, PrivilegeDesc,PriceLevel, LastUpdate, UpdateBy, Active FROM CustPrivilege"
        '                        Return CType(.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, "Pri"), DataSet)
        '                    Case EnumPrivSet.PrivilegeItm
        '                        'Modified 
        '                        'strSQL = "SELECT PrivItemCode,PrivItemType,StkDiscountType,StkDiscountValue,SalesDiscountCode, LastUpdate, UpdateBy FROM CustPrivItem" & _
        '                        '           " WHERE PrivilegeCode = '" & PrivCode & "'"
        '                        'Return CType(.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, "UOM"), DataSet)
        '                End Select
        '            Else
        '                Return Nothing
        '            End If
        '        End With
        '    End If
        'End Function

        Public Function GetListing() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetShortList() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function
#End Region

        'Clear all data in database
        Public Function ClearDB() As Boolean
        End Function

    End Class
#End Region

#Region "Package Base"
    Public Class PackageBase
        Inherits Core.CoreBase
        Dim InstallmentInfo As StrucClassInfo
#Region "Package Class in Package Base"
        Public Class Package
            Inherits Core.SingleBase
            Protected strCreateBy, strUpdateBy, strSchemeDesc, strSchemeNo As String
            Protected intPayPeriod As Integer
            Protected dblInitAmt As Double
            Protected dtCreateDate, dtLastUpdate As Date
            Protected dtInstall As DataTable
            Public MyScheme As New MyScheme

            Public Property SchemeNo() As String
                Get
                    Return strSchemeNo
                End Get
                Set(ByVal Value As String)
                    strSchemeNo = Value
                End Set
            End Property

            Public Property Installment() As DataTable
                Get
                    Return dtInstall
                End Get
                Set(ByVal Value As DataTable)
                    dtInstall = Value
                End Set
            End Property

            Public Property SchemeDesc() As String
                Get
                    Return strSchemeDesc
                End Get
                Set(ByVal Value As String)
                    strSchemeDesc = Value
                End Set
            End Property

            Public Property InitAmt() As Double
                Get
                    Return dblInitAmt
                End Get
                Set(ByVal Value As Double)
                    dblInitAmt = Value
                End Set
            End Property

            Public Property PayPeriod() As Integer
                Get
                    Return intPayPeriod
                End Get
                Set(ByVal Value As Integer)
                    intPayPeriod = Value
                End Set
            End Property

            Public Property CreateDate() As Date
                Get
                    Return dtCreateDate
                End Get
                Set(ByVal Value As Date)
                    dtCreateDate = Value
                End Set
            End Property

            Public Property CreateBy() As String
                Get
                    Return strCreateBy
                End Get
                Set(ByVal Value As String)
                    strCreateBy = Value
                End Set
            End Property

            Public Property LastUpdate() As Date
                Get
                    Return dtLastUpdate
                End Get
                Set(ByVal Value As Date)
                    dtLastUpdate = Value
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return strUpdateBy
                End Get
                Set(ByVal Value As String)
                    strUpdateBy = Value
                End Set
            End Property

        End Class
#End Region

#Region "Package Scheme"
        Public Class MyScheme
            Inherits Core.SchemeBase

            Protected Overrides Sub InitializeInfo()
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "SchemeNo"
                    .Length = 8
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(0, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "SchemeDesc"
                    .Length = 50
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = False
                    .AllowNegative = False
                End With
                MyBase.AddItem(1, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "PayPeriod"
                    .Length = 4
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(2, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "InitAmt"
                    .Length = 9
                    .DecPlace = 2
                    .RegExp = String.Empty
                    .IsMandatory = False
                    .AllowNegative = False
                End With
                MyBase.AddItem(3, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtDateTime
                    .FieldName = "CreateDate"
                    .Length = Nothing
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(4, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "CreateBy"
                    .Length = 20
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(5, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtDateTime
                    .FieldName = "LastUpdate"
                    .Length = Nothing
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(6, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "UpdateBy"
                    .Length = 20
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(7, this)
            End Sub

            Public ReadOnly Property SchemeNo() As StrucElement
                Get
                    Return MyBase.GetItem(0)
                End Get
            End Property

            Public ReadOnly Property SchemeDesc() As StrucElement
                Get
                    Return MyBase.GetItem(1)
                End Get
            End Property

            Public ReadOnly Property PayPeriod() As StrucElement
                Get
                    Return MyBase.GetItem(2)
                End Get
            End Property

            Public ReadOnly Property InitAmt() As StrucElement
                Get
                    Return MyBase.GetItem(3)
                End Get
            End Property

            Public ReadOnly Property CreateDate() As StrucElement
                Get
                    Return MyBase.GetItem(4)
                End Get
            End Property

            Public ReadOnly Property CreateBy() As StrucElement
                Get
                    Return MyBase.GetItem(5)
                End Get
            End Property

            Public ReadOnly Property LastUpdate() As StrucElement
                Get
                    Return MyBase.GetItem(6)
                End Get
            End Property

            Public ReadOnly Property UpdateBy() As StrucElement
                Get
                    Return MyBase.GetItem(7)
                End Get
            End Property

            Public Function GetElement(ByVal Key As Integer) As StrucElement
                Return MyBase.GetItem(Key)
            End Function

        End Class
#End Region
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "SCHEMENO, SCHEMEDESC, PAYPERIOD, INITAMT, CREATEDATE, CREATEBY, LASTUPDATE, UPDATEBY"
                .CheckFields = "SCHEMENO"
                .TableName = "PKGPAYSCHEME"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "SCHEMENO, SCHEMEDESC, PAYPERIOD, INITAMT, CREATEDATE, CREATEBY, LASTUPDATE, UPDATEBY"
                .ListingCond = String.Empty
                .ShortList = "SCHEMENO"
                .ShortListCond = String.Empty
            End With
            With InstallmentInfo
                .FieldsList = "SCHEMENO, PAYID, PAYRATIO, PAYDURA"
                .CheckFields = "SCHEMENO"
                .TableName = "PKGPAYSCHEMEITM"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "SCHEMENO, PAYID, PAYRATIO, PAYDURA"
                .ListingCond = String.Empty
                .ShortList = "SCHEMENO"
                .ShortListCond = String.Empty
            End With
            ReDim MyFields(0)
            MyBase.AddMyField(0, "Scheme No", "SCHEMENO", TypeCode.String)
        End Sub

#Region "Data Manipulation-Add,Edit,Del"

        Private Function AssignItem(ByVal objPackage As Package, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Dim arrList As New ArrayList
            AssignItem = False
            Try
                If objPackage Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False

                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "SchemeNo ='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPackage.SchemeNo.ToString) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        blnExec = True

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                End If
                                .Close()
                            End With
                        End If
                    End If

                    If blnExec Then
                        If blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                            Return False
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = "PKGPAYSCHEME"
                                .AddField("SchemeDesc", objPackage.SchemeDesc, SQLControl.EnumDataType.dtStringN)
                                .AddField("PayPeriod", objPackage.PayPeriod.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("InitAmt", objPackage.InitAmt.ToString, SQLControl.EnumDataType.dtNumeric)
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    .AddField("CreateDate", objPackage.CreateDate.ToString, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("CreateBy", objPackage.CreateBy, SQLControl.EnumDataType.dtString)
                                    .AddField("LastUpdate", objPackage.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("UpdateBy", objPackage.UpdateBy, SQLControl.EnumDataType.dtString)
                                End If
                                If pType = SQLControl.EnumSQLType.stUpdate Then
                                    .AddField("LastUpdate", objPackage.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("UpdateBy", objPackage.UpdateBy, SQLControl.EnumDataType.dtString)
                                End If

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                            "WHERE SchemeNo='" & .ParseValue(SQLControl.EnumDataType.dtString, objPackage.SchemeNo.ToString) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("SchemeNo", Convert.ToString(objPackage.SchemeNo).Trim, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                        "WHERE SchemeNo='" & .ParseValue(SQLControl.EnumDataType.dtString, objPackage.SchemeNo.ToString) & "'")
                                End Select
                                arrList.Add(strSQL)
                            End With
                            SaveInstallment(objPackage, arrList)
                            Try
                                'execute 
                                objDCom.BatchExecute(arrList, CommandType.Text, True)
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002")
                                Else
                                    Throw New ApplicationException("210004")
                                End If
                            Finally
                                objSQL.Dispose()
                            End Try
                            Return True
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objPackage = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
        'ADD
        Function Add(ByVal objPackage As Package) As Boolean
            Return AssignItem(objPackage, SQLControl.EnumSQLType.stInsert)
        End Function
        'AMEND
        Function Amend(ByVal objPackage As Package) As Boolean
            Return AssignItem(objPackage, SQLControl.EnumSQLType.stUpdate)
        End Function
        'DELETE
        Function Delete(ByVal objPackage As Package) As Boolean
            Dim strSQL As String
            Dim arrList As New ArrayList
            Dim blnFound, blnInUse As Boolean
            'Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If objPackage Is Nothing Then
                    'error message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()

                        strSQL = BuildDelete(myinfo.TableName, "SchemeNo=" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, objPackage.SchemeNo.ToString))
                        arrList.Add(strSQL)

                        strSQL = BuildDelete(InstallmentInfo.TableName, "SchemeNo=" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, objPackage.SchemeNo.ToString))
                        arrList.Add(strSQL)

                        Try
                            'execute
                            objDCom.BatchExecute(arrList, CommandType.Text, True)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try
                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As SystemException
                Throw exDelete
            Finally
                objPackage = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        'Enquiry
        Function Enquiry(Optional ByVal SQLstmt As String = Nothing) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    If SQLstmt = Nothing Or SQLstmt = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName)
                    Else
                        strSQL = SQLstmt
                    End If
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetInstall(ByVal SchemeNo As Integer) As DataTable
            Try
                If StartConnection() = True Then
                    With InstallmentInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "SchemeNo =" & SchemeNo)

                        Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), DataTable)
                    End With
                End If
            Catch ex As Exception
            Finally
                EndConnection()
            End Try
        End Function

        Public Function GetAllInstall() As DataTable
            Try
                If StartConnection() = True Then
                    With InstallmentInfo
                        strSQL = BuildSelect(.FieldsList, .TableName)

                        Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), DataTable)
                    End With
                End If
            Catch ex As Exception
            Finally
                EndConnection()
            End Try
        End Function

        Public Function SaveInstallment(ByVal objPackage As Package, ByRef arrlist As ArrayList) As ArrayList
            Dim strSQL As String
            Dim drRow As DataRow
            arrlist.Add("DELETE FROM PKGPAYSCHEMEITM WHERE SchemeNo=" & objPackage.SchemeNo)

            If objPackage.Installment Is Nothing = False Then
                For Each drRow In objPackage.Installment.Rows
                    With objSQL
                        .TableName = "PKGPAYSCHEMEITM"
                        .AddField("SchemeNo", objPackage.SchemeNo.ToString, SQLControl.EnumDataType.dtString)
                        .AddField("PayID", Convert.ToString(drRow.Item(1)), SQLControl.EnumDataType.dtNumeric)
                        .AddField("PayRatio", Convert.ToString(drRow.Item(2)), SQLControl.EnumDataType.dtNumeric)
                        .AddField("PayDura", Convert.ToString(drRow.Item(3)), SQLControl.EnumDataType.dtNumeric)
                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                        arrlist.Add(strSQL)
                    End With
                Next
            End If
            Return arrlist
        End Function

        Public Function GetListing() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetShortList() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function
#End Region

    End Class
#End Region

#Region "Question Base"
    Public Class QuestionBase
        Inherits Core.CoreBase

#Region "Question Class in Question Base"
        Public Class Question
            Protected strQuestion, strUpdBy, QuesID, strAnsGrp As String
            Protected intOpenQuestion As Integer
            Protected dtLastUpd As DateTime
            Public MyScheme As New MyScheme

            Public Property QuestionID() As String
                Get
                    Return QuesID
                End Get
                Set(ByVal QuestionID As String)
                    If QuestionID.Trim = String.Empty Then
                        Throw New Exception("Please key in Question Code")
                    Else
                        QuesID = QuestionID
                    End If
                End Set
            End Property

            Public Property QuestionName() As String
                Get
                    Return strQuestion
                End Get
                Set(ByVal Value As String)
                    strQuestion = Value
                End Set
            End Property

            Public Property LastUpdate() As DateTime
                Get
                    Return dtLastUpd
                End Get
                Set(ByVal Value As DateTime)
                    dtLastUpd = Value
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return strUpdBy
                End Get
                Set(ByVal Value As String)
                    strUpdBy = Value
                End Set
            End Property

            Public Property IsOpenQuestion() As Integer
                Get
                    Return intOpenQuestion
                End Get
                Set(ByVal Value As Integer)
                    intOpenQuestion = Value
                End Set
            End Property

            Public Property AnswerGrp() As String
                Get
                    Return strAnsGrp
                End Get
                Set(ByVal Value As String)
                    strAnsGrp = Value
                End Set
            End Property

        End Class
#End Region

#Region "Question Scheme"
        Public Class MyScheme
            Inherits Core.SchemeBase

            Protected Overrides Sub InitializeInfo()
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "QuestionID"
                    .Length = 4
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(0, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "QuestionName"
                    .Length = 50
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(1, this)
            End Sub

            Public ReadOnly Property QuestionID() As StrucElement
                Get
                    Return MyBase.GetItem(0)
                End Get
            End Property

            Public ReadOnly Property Question() As StrucElement
                Get
                    Return MyBase.GetItem(1)
                End Get
            End Property

            Public Function GetElement(ByVal Key As Integer) As StrucElement
                Return MyBase.GetItem(Key)
            End Function
        End Class
#End Region

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "QUESTIONID, QUESTIONNAME,ISOPENQUES, ANSWERGRP, LASTUPDATE, UPDATEBY, INUSE, FLAG"
                .CheckFields = "INUSE, FLAG"
                .TableName = "QUESTION"
                .DefaultCond = "FLAG = 1"
                .DefaultOrder = String.Empty
                .Listing = "QUESTIONID, QUESTIONNAME,ISOPENQUES, ANSWERGRP ,LASTUPDATE, UPDATEBY, INUSE, FLAG"
                .ListingCond = "FLAG = 1"
                .ShortList = "QUESTIONID, QUESTIONNAME"
                .ShortListCond = String.Empty
            End With
            ReDim MyFields(1)
            MyBase.AddMyField(0, "Question ID", "QuestionID", TypeCode.String)
            MyBase.AddMyField(1, "Question Name", "QuestionName", TypeCode.String)
        End Sub

#Region "Data Manipulation-Add,Edit,Del"

        Private Function AssignItem(ByVal objQuestion As Question, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As SqlClient.SqlDataReader
            AssignItem = False
            Try
                If objQuestion Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False

                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "QuestionID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, objQuestion.QuestionID) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        blnExec = True

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("Flag")) = 0 Then
                                        'Found but deleted
                                        blnFlag = False
                                    Else
                                        'Found and active
                                        blnFlag = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If
                    End If

                    If blnExec Then
                        If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                            'Item found & active
                            'system.Windows.Forms.MessageBox.Show("ab", "ABC",Windows.Forms.MessageBoxButtons.YesNo, Windows.Forms.MessageBoxIcon.Information,Windows.Forms.MessageBoxDefaultButton.Button2, Windows.Forms.MessageBoxOptions.DefaultDesktopOnly
                            'MsgBox("Record already exist", MsgBoxStyle.Critical, "Record Exist")
                            Return False
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = "QUESTION"
                                .AddField("QuestionName", objQuestion.QuestionName, SQLControl.EnumDataType.dtStringN)
                                .AddField("UpdateBy", objQuestion.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", objQuestion.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                .AddField("IsOpenQues", objQuestion.IsOpenQuestion.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("AnswerGrp", objQuestion.AnswerGrp.ToString, SQLControl.EnumDataType.dtString)
                                .AddField("Flag", cFlagActive.ToString, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                            "WHERE QuestionID='" & .ParseValue(SQLControl.EnumDataType.dtNumeric, objQuestion.QuestionID) & "'")
                                        Else
                                            If blnFound = False Then
                                                '.AddField("QuestionID", objQuestion.QuestionID, SQLControl.EnumDataType.dtNumeric)
                                                .AddField("CreateDate", objQuestion.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                                .AddField("CreateBy", objQuestion.UpdateBy, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                        "WHERE QuestionID='" & .ParseValue(SQLControl.EnumDataType.dtNumeric, objQuestion.QuestionID) & "'")
                                End Select
                            End With
                            Try
                                'execute
                                objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002")
                                Else
                                    Throw New ApplicationException("210004")
                                End If
                            Finally
                                objSQL.Dispose()
                            End Try
                            Return True
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objQuestion = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
        'ADD
        Function Add(ByVal objQuestion As Question) As Boolean
            Return AssignItem(objQuestion, SQLControl.EnumSQLType.stInsert)
        End Function
        'AMEND
        Function Amend(ByVal objQuestion As Question) As Boolean
            Return AssignItem(objQuestion, SQLControl.EnumSQLType.stUpdate)
        End Function
        'DELETE
        Function Delete(ByVal objQuestion As Question) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If objQuestion Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "QuestionID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, objQuestion.QuestionID) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("InUse")) = 1 Then
                                        blnInUse = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If

                        If blnFound = False Then
                            'Error Message
                            Return False
                        End If
                        If blnFound = True And blnInUse = True Then
                            strSQL = BuildUpdate(MyInfo.TableName, " SET Flag = " & cFlagNonActive & _
                                " , LastUpdate = '" & objQuestion.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, objQuestion.UpdateBy) & "'" & _
                                " WHERE QuestionID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, objQuestion.QuestionID) & "'")
                        End If
                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(MyInfo.TableName, "QuestionID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, objQuestion.QuestionID) & "'")
                        End If
                        Try
                            'execute
                            objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try
                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As SystemException
                Throw exDelete
            Finally
                objQuestion = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

#End Region

#Region "Data Selection"
        'Enquiry
        Function Enquiry(Optional ByVal SQLstmt As String = Nothing) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    If SQLstmt = Nothing Or SQLstmt = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName)
                    Else
                        strSQL = SQLstmt
                    End If
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetListing() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetShortList() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetAnswer(ByVal AnswerGrpID As String) As DataTable
            Try
                StartConnection()
                StartSQLControl()
                strSQL = "SELECT ANSWER.AnswerName FROM ANSWER INNER JOIN ANSWERGRP ON ANSWER.AnswerID = ANSWERGRP.AnswerID " & _
                         "WHERE (ANSWER.Flag = 1) AND (ANSWERGRP.AnsGroupID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, AnswerGrpID) & "')"
                Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text), DataTable)
            Catch ex As Exception
                Return Nothing
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

        Function ClearDB() As Boolean
        End Function
    End Class
#End Region

#Region "Question Group Base"
    Public Class QuestionGroupBase
        Inherits Core.CoreBase

#Region "QuestionGroup in QuestionGroupBase"
        Public Class QuestionGroup
            Inherits Core.SingleBase

            Protected strQGroupID, strQGroupName, strUpdBy As String
            Protected dtLastUpd As DateTime
            Protected intAnsGroupID As Integer
            Public MyScheme As New MyScheme
            Private dtSelQuestion As DataTable

            Public Sub SetQuestionTable(ByRef dtQuestion As DataTable)
                If dtQuestion Is Nothing = True Then
                    dtQuestion = New DataTable
                End If

                dtQuestion.Clear()
                dtQuestion.Columns.Clear()
                dtQuestion.Columns.Add("QGroupID", Type.GetType("System.String"))
                dtQuestion.Columns.Add("QuestionID", Type.GetType("System.String"))
                dtQuestion.Columns.Add("AnsGroupID", Type.GetType("System.String"))
            End Sub

            Public Property QGroupID() As String
                Get
                    Return strQGroupID
                End Get
                Set(ByVal QGroupID As String)
                    strQGroupID = QGroupID
                    MyBase.AddItem(0, strQGroupID)
                End Set
            End Property

            Public Property QGroupName() As String
                Get
                    Return strQGroupName
                End Get
                Set(ByVal Value As String)
                    strQGroupName = Value
                    MyBase.AddItem(1, strQGroupName)
                End Set
            End Property

            Public Property AndGroupID() As Integer
                Get
                    Return intAnsGroupID
                End Get
                Set(ByVal Value As Integer)
                    intAnsGroupID = Value
                End Set
            End Property

            Public Property LastUpdate() As DateTime
                Get
                    Return dtLastUpd
                End Get
                Set(ByVal Value As DateTime)
                    If Value = Nothing Then
                        dtLastUpd = Now()
                    Else
                        dtLastUpd = Value
                    End If
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return strUpdBy
                End Get
                Set(ByVal Value As String)
                    strUpdBy = Value
                End Set
            End Property

            Public Property SelectedQGroup() As DataTable
                Get
                    Return dtSelQuestion
                End Get
                Set(ByVal Value As DataTable)
                    dtSelQuestion = Value
                End Set
            End Property
        End Class
#End Region

#Region "Question Group Scheme"
        Public Class MyScheme
            Inherits Core.SchemeBase

            Protected Overrides Sub InitializeInfo()
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "QGroupID"
                    .Length = 4
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(0, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "QGroupName"
                    .Length = 30
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(1, this)
            End Sub

            Public ReadOnly Property QGroupID() As StrucElement
                Get
                    Return MyBase.GetItem(0)
                End Get
            End Property

            Public ReadOnly Property QGroupName() As StrucElement
                Get
                    Return MyBase.GetItem(1)
                End Get
            End Property

        End Class

#End Region

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "QGROUPID, QGROUPNAME, LASTUPDATE, UPDATEBY, INUSE, FLAG"
                .CheckFields = "INUSE, FLAG"
                .TableName = "QUESTIONGROUP"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "QGROUPID, QGROUPNAME, LASTUPDATE, UPDATEBY, INUSE, FLAG"
                .ListingCond = "FLAG = 1"
                .ShortList = "QGROUPID, QGROUPNAME"
                .ShortListCond = "FLAG=1"
            End With
            ReDim MyFields(1)
            MyBase.AddMyField(0, "Question Group ID", "QGroupID", TypeCode.String)
            MyBase.AddMyField(1, "Question Group Name", "QGroupName", TypeCode.String)
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function AssignItem(ByVal objQGroup As QuestionGroup, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String

            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As SqlClient.SqlDataReader
            AssignItem = False
            Try
                If objQGroup Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False

                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "QGroupID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, objQGroup.QGroupID) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        blnExec = True

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("Flag")) = 0 Then
                                        'Found but deleted
                                        blnFlag = False
                                    Else
                                        'Found and active
                                        blnFlag = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If
                    End If

                    If blnExec Then
                        If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                            'Item found & active
                            'system.Windows.Forms.MessageBox.Show("ab", "ABC",Windows.Forms.MessageBoxButtons.YesNo, Windows.Forms.MessageBoxIcon.Information,Windows.Forms.MessageBoxDefaultButton.Button2, Windows.Forms.MessageBoxOptions.DefaultDesktopOnly
                            'MsgBox("Record already exist", MsgBoxStyle.Critical, "Record Exist")
                            Return False
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = MyInfo.TableName
                                .AddField("QGroupName", objQGroup.QGroupName, SQLControl.EnumDataType.dtString)
                                .AddField("UpdateBy", objQGroup.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", objQGroup.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                .AddField("Flag", cFlagActive.ToString, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        ''???
                                        If blnFound = True And blnFlag = False Then
                                            If (objQGroup.QGroupID = "0") = False Then
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                                "WHERE QGroupID='" & .ParseValue(SQLControl.EnumDataType.dtNumeric, objQGroup.QGroupID) & "'")
                                            End If
                                        Else
                                            If blnFound = False Then
                                                '.AddField("QGroupID", objQGroup.QGroupID, SQLControl.EnumDataType.dtNumeric)
                                                .AddField("CreateDate", objQGroup.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                                .AddField("CreateBy", objQGroup.UpdateBy, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If

                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                        "WHERE QGroupID='" & .ParseValue(SQLControl.EnumDataType.dtNumeric, objQGroup.QGroupID) & "'")
                                End Select
                            End With
                            Try
                                'execute
                                objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text, , False)
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    rdr = CType(objDCom.Execute("SELECT @@IDENTITY FROM " & MyInfo.TableName, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
                                    If rdr Is Nothing = False Then
                                        If rdr.Read() Then
                                            objQGroup.QGroupID = CType(rdr.Item(0), String)
                                        End If
                                        rdr.Close()
                                        rdr = Nothing
                                    End If
                                End If
                                objDCom.BatchExecute(SaveQuestionGroup(objQGroup.QGroupID, objQGroup.SelectedQGroup), CommandType.Text, True)
                                Return True
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002")
                                Else
                                    Throw New ApplicationException("210004")
                                End If
                            Finally
                                objSQL.Dispose()
                            End Try
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objQGroup = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Function Add(ByVal objQGroup As QuestionGroup) As Boolean
            Return AssignItem(objQGroup, SQLControl.EnumSQLType.stInsert)
        End Function
        'AMEND
        Function Amend(ByVal objQGroup As QuestionGroup) As Boolean
            Return AssignItem(objQGroup, SQLControl.EnumSQLType.stUpdate)
        End Function
        'DELETE
        Function Delete(ByVal objQGroup As QuestionGroup) As Boolean
            Dim strSQL As String
            Dim blnFound, blnInUse As Boolean
            Dim arySQL As New ArrayList
            Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If objQGroup Is Nothing Then
                    'error message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "QGroupID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, objQGroup.QGroupID) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt32(.Item("InUse")) = 1 Then
                                        blnInUse = True 'if record is found and in use
                                    End If
                                End If
                                .Close()
                            End With
                        End If

                        'record is found and in use
                        If blnFound = True And blnInUse = True Then
                            strSQL = BuildUpdate(MyInfo.TableName, "SET Flag = " & cFlagNonActive & _
                                " , LastUpdate = '" & objQGroup.LastUpdate & "' UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, objQGroup.UpdateBy) & "'" & _
                                " WHERE QGroupID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, objQGroup.QGroupID) & "'")
                            arySQL.Add(strSQL)
                        End If

                        'record exist but not in use
                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(MyInfo.TableName, "QGroupID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, objQGroup.QGroupID) & "'")
                            arySQL.Add(strSQL)
                        End If

                        strSQL = BuildDelete("QUESTIONSET", "QGROUPID ='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objQGroup.QGroupID) & "'")
                        arySQL.Add(strSQL)
                        Try
                            'execute
                            objDCom.BatchExecute(arySQL, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try
                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As SystemException
                Throw exDelete
            Finally
                objQGroup = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Private Function SaveQuestionGroup(ByVal QGroupID As String, ByVal dtSelGroup As DataTable) As ArrayList
            Dim arySQL As New ArrayList
            Dim drRow As DataRow
            Dim strSQL As String
            arySQL.Add("DELETE FROM QUESTIONSET WHERE QGroupID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, QGroupID) & "'")

            If dtSelGroup Is Nothing = False Then
                For Each drRow In dtSelGroup.Rows
                    With objSQL
                        .TableName = "QUESTIONSET"
                        .AddField("QGroupID", QGroupID, SQLControl.EnumDataType.dtNumeric)
                        .AddField("QuestionID", Convert.ToString(drRow.Item("QuestionID")), SQLControl.EnumDataType.dtNumeric)
                        .AddField("AnsGroupID", Convert.ToString(drRow.Item("AnsGroupID")), SQLControl.EnumDataType.dtString)
                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                        arySQL.Add(strSQL)
                    End With
                Next
                dtSelGroup.Dispose()
            End If
            Return arySQL
        End Function

        Public Function GetAnsGroup(ByVal QuestionID As String) As String
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    strSQL = "SELECT AnswerGrp FROM QUESTION WHERE QuestionID =" & QuestionID & " AND Flag = 1"
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtScalar, CommandType.Text), String)
                End If
            Catch ex As Exception
                Return Nothing
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function CheckIsOpenQues(ByVal QuestionID As Integer) As Boolean
            Try
                Dim intOpenQues As Integer
                If StartConnection() = True Then
                    StartSQLControl()
                    strSQL = "SELECT IsOpenQues FROM QUESTION WHERE (QuestionID =" & QuestionID & ") AND (Flag = 1)"
                    intOpenQues = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text), Integer)

                    If intOpenQues = 1 Then
                        Return True
                    ElseIf intOpenQues = 0 Then
                        Return False
                    Else
                        Return Nothing
                    End If
                End If
            Catch ex As Exception
                Return Nothing
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

#End Region

#Region "Data Selection"
        Function Enquiry(Optional ByVal SQLstmt As String = Nothing) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    If SQLstmt = Nothing Or SQLstmt = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName)
                    Else
                        strSQL = SQLstmt
                    End If
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetShortList() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetQGroup(ByVal QGroup As String) As DataSet
            If StartConnection() = True Then
                strSQL = "SELECT QGroupID, QGroupName FROM QUESTIONGROUP WHERE QGroupID = " & Int32.Parse(QGroup) & " AND Flag = 1"
                Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, "GROUPSET"), DataSet)
            End If
            EndConnection()
        End Function

        Public Function GetQuestionFilter(ByVal QGroupID As String) As DataSet
            If StartConnection() = True Then
                If QGroupID Is Nothing = False Then
                    strSQL = "SELECT QUESTIONID, QUESTIONNAME FROM QUESTION WHERE " & _
                                            "(QuestionID NOT IN (SELECT DISTINCT IC.QUESTIONID FROM " & _
                                            "QUESTION C, QUESTIONSET IC WHERE C.QUESTIONID = " & _
                                            "IC.QUESTIONID AND IC.QGROUPID = " & Int32.Parse(QGroupID) & ")) "
                Else
                    strSQL = "SELECT QUESTIONID, QUESTIONNAME FROM QUESTION WHERE FLAG = 1"
                End If

                Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, "GROUPSET"), DataSet)
            End If
            EndConnection()
        End Function

        Public Overloads Function GetQuestionSet(ByVal QGroupID As String) As DataSet
            If StartConnection() = True Then
                strSQL = "SELECT DISTINCT C.QUESTIONID, C.QUESTIONNAME FROM QUESTION C " & _
                         "INNER JOIN QUESTIONSET IC ON C.QUESTIONID = IC.QUESTIONID " & _
                         "WHERE (IC.QGROUPID =" & Int32.Parse(QGroupID) & ")"
                Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, "GROUPSET"), DataSet)
            End If
            EndConnection()
        End Function



        Public Shared Function QuestionSetShortList() As DataTable
            Try
                StartSQLControl()
                StartConnection()
                strSQL = "SELECT DISTINCT IC.QGroupID, IC.QuestionID, IC.QGroupID + ' / ' + IC.QuestionID AS QUESTSET FROM QUESTIONSET IC INNER JOIN QUESTIONGROUP I " & _
                        "ON IC.QGroupID = I.QGroupID WHERE  I.Flag = 1 " & _
                        "ORDER BY IC.QGroupID, IC.QuestionID"
                Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, "CUSTOMER"), DataTable)
            Catch ex As Exception
                Return Nothing
            Finally
                EndConnection()
                EndSQLControl()
            End Try
        End Function

        Public Shared Function GetQGroupCombo() As DataTable
            Try
                StartSQLControl()
                StartConnection()
                strSQL = "SELECT QGroupID, QGroupName FROM QUESTIONGROUP WHERE Flag = 1"

                Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, "QUESTIONGROUP"), DataTable)
            Catch ex As Exception

            Finally
                EndConnection()
                EndSQLControl()
            End Try
        End Function

        'Public Function GetIntakeInfo(ByVal IntakeCode As String) As DataTable
        '    Try
        '        StartSQLControl()
        '        StartConnection()
        '        With MyInfo
        '            strSQL = BuildSelect(.FieldsList, .TableName, "IntakeCode ='" & IntakeCode & "' AND STATUS = 1 AND FLAG = 1")
        '            Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName, True), DataTable)
        '        End With

        '    Catch ex As Exception

        '    Finally
        '        EndConnection()
        '        EndSQLControl()
        '    End Try
        'End Function

        Public Shared Function GetQuestionSetCombo(ByVal QGroupID As String) As DataTable
            Try
                StartSQLControl()
                StartConnection()
                strSQL = "SELECT QuestionID, (SELECT QuestionName FROM QUESTION WHERE QuestionID = QUESTIONSET.QuestionID) FROM QUESTIONSET WHERE QGroupID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, QGroupID) & "'"

                Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, "QUESTIONSET"), DataTable)
            Catch ex As Exception

            Finally
                EndConnection()
                EndSQLControl()
            End Try
        End Function
#End Region
    End Class
#End Region

#Region "Answer Base"
    Public Class AnswerBase
        Inherits Core.CoreBase

#Region "Answer Class in Answer Base"
        Public Class Answer
            Protected strAnswer, strUpdBy, strAnswerID As String
            Protected dtLastUpd As DateTime
            Public MyScheme As New MyScheme

            Public Property AnswerID() As String
                Get
                    Return strAnswerID
                End Get
                Set(ByVal AnswerID As String)
                    If AnswerID.Trim = String.Empty Then
                        Throw New Exception("Please key in Answer ID")
                    Else
                        strAnswerID = AnswerID
                    End If
                End Set
            End Property

            Public Property Answer() As String
                Get
                    Return strAnswer
                End Get
                Set(ByVal Value As String)
                    strAnswer = Value
                End Set
            End Property

            Public Property LastUpdate() As DateTime
                Get
                    Return dtLastUpd
                End Get
                Set(ByVal Value As DateTime)
                    dtLastUpd = Value
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return strUpdBy
                End Get
                Set(ByVal Value As String)
                    strUpdBy = Value
                End Set
            End Property
        End Class
#End Region

#Region "Answer Scheme"
        Public Class MyScheme
            Inherits Core.SchemeBase

            Protected Overrides Sub InitializeInfo()
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "AnswerID"
                    .Length = 4
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(0, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "Answer"
                    .Length = 50
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(1, this)
            End Sub

            Public ReadOnly Property AnswerID() As StrucElement
                Get
                    Return MyBase.GetItem(0)
                End Get
            End Property

            Public ReadOnly Property Answer() As StrucElement
                Get
                    Return MyBase.GetItem(1)
                End Get
            End Property

            Public Function GetElement(ByVal Key As Integer) As StrucElement
                Return MyBase.GetItem(Key)
            End Function
        End Class
#End Region

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "AnswerID, AnswerNAME,CreateDate, CreateBy, LASTUPDATE, UPDATEBY,FLAG ,INUSE"
                .CheckFields = "INUSE, FLAG"
                .TableName = "Answer"
                .DefaultCond = "FLAG = 1"
                .DefaultOrder = String.Empty
                .Listing = "AnswerID, AnswerNAME,CreateDate, CreateBy, LASTUPDATE, UPDATEBY,FLAG ,INUSE"
                .ListingCond = "FLAG = 1"
                .ShortList = "AnswerID, AnswerNAME"
                .ShortListCond = String.Empty
            End With
            ReDim MyFields(1)
            MyBase.AddMyField(0, "Answer ID", "AnswerID", TypeCode.String)
            MyBase.AddMyField(1, "Answer", "AnswerName", TypeCode.String)
        End Sub

#Region "Data Manipulation-Add,Edit,Del"

        Private Function AssignItem(ByVal objAnswer As Answer, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As SqlClient.SqlDataReader
            AssignItem = False
            Try
                If objAnswer Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False

                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "AnswerID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, objAnswer.AnswerID) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        blnExec = True

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("Flag")) = 0 Then
                                        blnFlag = False 'Found but deleted
                                    Else
                                        blnFlag = True  'Found and active
                                    End If
                                End If
                                .Close()
                            End With
                        End If
                    End If

                    If blnExec Then
                        If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                            'Item found & active
                            'system.Windows.Forms.MessageBox.Show("ab", "ABC",Windows.Forms.MessageBoxButtons.YesNo, Windows.Forms.MessageBoxIcon.Information,Windows.Forms.MessageBoxDefaultButton.Button2, Windows.Forms.MessageBoxOptions.DefaultDesktopOnly
                            'MsgBox("Record already exist", MsgBoxStyle.Critical, "Record Exist")
                            Return False
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = "Answer"
                                .AddField("AnswerName", objAnswer.Answer, SQLControl.EnumDataType.dtString)
                                .AddField("UpdateBy", objAnswer.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", objAnswer.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                .AddField("Flag", cFlagActive.ToString, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                            "WHERE AnswerID='" & .ParseValue(SQLControl.EnumDataType.dtNumeric, objAnswer.AnswerID) & "'")
                                        Else
                                            If blnFound = False Then
                                                '.AddField("QuestionID", objQuestion.QuestionID, SQLControl.EnumDataType.dtNumeric)
                                                .AddField("CreateDate", objAnswer.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                                .AddField("CreateBy", objAnswer.UpdateBy, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                        "WHERE AnswerID='" & .ParseValue(SQLControl.EnumDataType.dtNumeric, objAnswer.AnswerID) & "'")
                                End Select
                            End With
                            Try
                                'execute
                                objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002")
                                Else
                                    Throw New ApplicationException("210004")
                                End If
                            Finally
                                objSQL.Dispose()
                            End Try
                            Return True
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objAnswer = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
        'ADD
        Function Add(ByVal objAnswer As Answer) As Boolean
            Return AssignItem(objAnswer, SQLControl.EnumSQLType.stInsert)
        End Function
        'AMEND
        Function Amend(ByVal objAnswer As Answer) As Boolean
            Return AssignItem(objAnswer, SQLControl.EnumSQLType.stUpdate)
        End Function
        'DELETE
        Function Delete(ByVal objAnswer As Answer) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If objAnswer Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "AnswerID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, objAnswer.AnswerID) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("InUse")) = 1 Then
                                        blnInUse = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If

                        If blnFound = False Then
                            'Error Message
                            Return False
                        End If
                        If blnFound = True And blnInUse = True Then
                            strSQL = BuildUpdate(MyInfo.TableName, " SET Flag = " & cFlagNonActive & _
                                " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, objAnswer.LastUpdate) & " , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, objAnswer.UpdateBy) & "'" & _
                                " WHERE AnswerID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, objAnswer.AnswerID) & "'")
                        End If
                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(MyInfo.TableName, "AnswerID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, objAnswer.AnswerID) & "'")
                        End If
                        Try
                            'execute
                            objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try
                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As SystemException
                Throw exDelete
            Finally
                objAnswer = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

#End Region

#Region "Data Selection"
        'Enquiry
        Function Enquiry(Optional ByVal SQLstmt As String = Nothing) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    If SQLstmt = Nothing Or SQLstmt = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName)
                    Else
                        strSQL = SQLstmt
                    End If
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetListing() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetShortList() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function
#End Region

    End Class
#End Region

#Region "Answer Group Base"
    Public Class AnswerGroupBase
        Inherits Core.CoreBase

#Region "AnswerGroup in AnswerGroupBase"
        Public Class AnswerGroup
            Inherits Core.SingleBase

            Protected strUpdBy, strAnsGrpID, strCreateBy As String
            Protected dtLastUpd, dtCreateDate As DateTime
            Protected intAnswerID As Integer
            Public MyScheme As New MyScheme
            Private dtSelAnswer As DataTable

            Public Sub SetAnswerTable(ByRef dtAnswer As DataTable)
                If dtAnswer Is Nothing = True Then
                    dtAnswer = New DataTable
                End If

                dtAnswer.Clear()
                dtAnswer.Columns.Clear()
                dtAnswer.Columns.Add("AnsGroupID", Type.GetType("System.String"))
                dtAnswer.Columns.Add("AnswerID", Type.GetType("System.String"))
            End Sub

            Public Property AnsGrpID() As String
                Get
                    Return strAnsGrpID
                End Get
                Set(ByVal AnsGrpID As String)
                    strAnsGrpID = AnsGrpID
                    MyBase.AddItem(0, strAnsGrpID)
                End Set
            End Property

            Public Property AnswerID() As Integer
                Get
                    Return intAnswerID
                End Get
                Set(ByVal Value As Integer)
                    intAnswerID = Value
                End Set
            End Property

            Public Property LastUpdate() As DateTime
                Get
                    Return dtLastUpd
                End Get
                Set(ByVal Value As DateTime)
                    If Value = Nothing Then
                        dtLastUpd = Now()
                    Else
                        dtLastUpd = Value
                    End If
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return strUpdBy
                End Get
                Set(ByVal Value As String)
                    strUpdBy = Value
                End Set
            End Property

            Public Property SelectedAnsGroup() As DataTable
                Get
                    Return dtSelAnswer
                End Get
                Set(ByVal Value As DataTable)
                    dtSelAnswer = Value
                End Set
            End Property
        End Class
#End Region

#Region "Answer Group Scheme"
        Public Class MyScheme
            Inherits Core.SchemeBase

            Protected Overrides Sub InitializeInfo()
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "AnsGrpID"
                    .Length = 20
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(0, this)
            End Sub

            Public ReadOnly Property AnsGrpID() As StrucElement
                Get
                    Return MyBase.GetItem(0)
                End Get
            End Property
        End Class

#End Region

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "DISTINCT ANSGROUPID, LASTUPDATE, UPDATEBY,FLAG ,INUSE"
                .CheckFields = "INUSE, FLAG"
                .TableName = "ANSWERGRP"
                .DefaultCond = "FLAG = 1"
                .DefaultOrder = String.Empty
                .Listing = "DISTINCT ANSGROUPID,LASTUPDATE, UPDATEBY,FLAG ,INUSE"
                .ListingCond = "FLAG = 1"
                .ShortList = "ANSGROUPID"
                .ShortListCond = String.Empty
            End With
            ReDim MyFields(0)
            MyBase.AddMyField(0, "Answer Group ID", "ANSGROUPID", TypeCode.String)
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function AssignItem(ByVal objAnsGrp As AnswerGroup, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String

            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As SqlClient.SqlDataReader
            AssignItem = False
            Try
                If objAnsGrp Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False

                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "AnsGroupID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objAnsGrp.AnsGrpID) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        blnExec = True

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt16(.Item("Flag")) = 0 Then
                                        'Found but deleted
                                        blnFlag = False
                                    Else
                                        'Found and active
                                        blnFlag = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If
                    End If

                    If blnExec Then
                        If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                            Return False
                        Else
                            StartSQLControl()
                            Try
                                'execute
                                objDCom.BatchExecute(SaveAnswerGroup(objAnsGrp, objAnsGrp.SelectedAnsGroup), CommandType.Text, True)
                                Return True
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002")
                                Else
                                    Throw New ApplicationException("210004")
                                End If
                            Finally
                                objSQL.Dispose()
                            End Try
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objAnsGrp = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Function Add(ByVal objAnsGrp As AnswerGroup) As Boolean
            Return AssignItem(objAnsGrp, SQLControl.EnumSQLType.stInsert)
        End Function
        'AMEND
        Function Amend(ByVal objAnsGrp As AnswerGroup) As Boolean
            Return AssignItem(objAnsGrp, SQLControl.EnumSQLType.stUpdate)
        End Function
        'DELETE
        Function Delete(ByVal objAnsGrp As AnswerGroup) As Boolean
            Dim strSQL As String
            Dim blnFound, blnInUse As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If objAnsGrp Is Nothing Then
                    'error message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "AnsGroupID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objAnsGrp.AnsGrpID) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt32(.Item("InUse")) = 1 Then
                                        blnInUse = True 'if record is found and in use
                                    End If
                                End If
                                .Close()
                            End With
                        End If

                        'record is found and in use
                        If blnFound = True And blnInUse = True Then
                            strSQL = BuildUpdate(MyInfo.TableName, "SET Flag = " & cFlagNonActive & _
                                " , LastUpdate = '" & objAnsGrp.LastUpdate & "' UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, objAnsGrp.UpdateBy) & "'" & _
                                " WHERE AnsGroupID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, objAnsGrp.AnsGrpID) & "'")
                        End If

                        'record is found and not in use
                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(MyInfo.TableName, "AnsGroupID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objAnsGrp.AnsGrpID) & "'")
                        End If
                        Try
                            'execute
                            objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try
                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As SystemException
                Throw exDelete
            Finally
                objAnsGrp = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Private Function SaveAnswerGroup(ByVal objAnsGrp As AnswerGroup, ByVal dtSelGroup As DataTable) As ArrayList
            Dim arySQL As New ArrayList
            Dim drRow As DataRow
            Dim strSQL As String
            arySQL.Add("DELETE FROM ANSWERGRP WHERE AnsGroupID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objAnsGrp.AnsGrpID) & "'")

            If dtSelGroup Is Nothing = False Then
                For Each drRow In dtSelGroup.Rows
                    With objSQL
                        .TableName = "ANSWERGRP"
                        .AddField("AnsGroupID", objAnsGrp.AnsGrpID, SQLControl.EnumDataType.dtString)
                        .AddField("AnswerID", Convert.ToString(drRow.Item("AnswerID")), SQLControl.EnumDataType.dtNumeric)
                        .AddField("LastUpdate", objAnsGrp.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                        .AddField("UpdateBy", objAnsGrp.UpdateBy, SQLControl.EnumDataType.dtString)
                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                        arySQL.Add(strSQL)
                        .TableName = "ANSWER"
                        .AddField("InUse", (1).ToString, SQLControl.EnumDataType.dtNumeric)
                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ANSWERID='" & Convert.ToString(drRow.Item("AnswerID")) & "'")
                        arySQL.Add(strSQL)
                    End With
                Next
                dtSelGroup.Dispose()
            End If
            Return arySQL
        End Function

#End Region

#Region "Data Selection"
        Function Enquiry(Optional ByVal SQLstmt As String = Nothing) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    If SQLstmt = Nothing Or SQLstmt = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName)
                    Else
                        strSQL = SQLstmt
                    End If
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetListing() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetShortList() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetAnswerFilter(ByVal AnsGroupID As String) As DataSet
            If StartConnection() = True Then
                If AnsGroupID Is Nothing = False Then
                    strSQL = "SELECT AnswerID, AnswerName FROM ANSWER WHERE (AnswerID NOT IN " & _
                             " (SELECT AG.AnswerID FROM AnswerGrp AG, Answer A " & _
                             " WHERE AG.AnswerID = A.AnswerID AND AG.ANSGROUPID = '" & AnsGroupID & "'))"
                Else
                    strSQL = "SELECT AnswerID, AnswerName FROM ANSWER WHERE FLAG = 1"
                End If

                Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, "GROUPSET"), DataSet)
            End If
            EndConnection()
        End Function

        Public Overloads Function GetAnswerSet(ByVal AnsGoupID As String) As DataSet
            If StartConnection() = True Then
                strSQL = "SELECT ANSWER.AnswerID, ANSWER.AnswerName FROM ANSWER INNER JOIN " & _
                         "ANSWERGRP ON ANSWER.AnswerID = ANSWERGRP.AnswerID " & _
                         "WHERE (ANSWERGRP.AnsGroupID = '" & AnsGoupID & "')"
                Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text), DataSet)
            End If
            EndConnection()
        End Function

#End Region
    End Class
#End Region

#Region "Promo Base"
    Public Class PromoBase
        Inherits Core.CoreBase
        Protected MyInfoPromoBatch As SEAL.Model.Moyenne.CoreBase.StrucClassInfo

        Public Enum EnumPromo
            PromoHDR = 0
            PromoDTL = 1
        End Enum

        Public Enum EnumPromoType
            BuyXFreeY = 1
            BuyXOnePrice = 2
            MarkDown = 3
            MarkUp = 4
        End Enum

        Public Enum EnumPromoItemType
            None = 0
            PromoItem = 1
            FreeItem = 2
            PriceItem = 3
            'DiscItem = 4
        End Enum

        Public Enum EnumPromoLevel
            None = 0
            Stock = 1
            Category = 2
            Brand = 3
        End Enum

#Region "PromoHDR Class in Promo Base"
        Public Class PromoHDR
            Inherits Core.SingleBase
            Protected strPromoCode, strPromoDesc, strUpdateBy As String
            Protected intPromoType, intActive, intPromoLevel As Integer
            Protected dtLastUpdate, dtStartDate, dtEndDate As Date
            Protected dtStartTime, dtEndTime As Date
            Protected dtPromoItem As DataTable
            Protected strPromoBatch, strlevelCode As String
            Protected dblPromoPrice, dblPromoDisc As Double
            Public MyScheme As New MyScheme
            Protected aryBranchID As ArrayList

            Public Property Code() As String
                Get
                    Return strPromoCode
                End Get
                Set(ByVal Value As String)
                    strPromoCode = Value
                End Set
            End Property

            Public Property Description() As String
                Get
                    Return strPromoDesc
                End Get
                Set(ByVal Value As String)
                    strPromoDesc = Value
                End Set
            End Property

            Public Property StartDate() As DateTime
                Get
                    Return dtStartDate
                End Get
                Set(ByVal Value As DateTime)
                    If Value = Nothing Then
                        dtStartDate = Now()
                    Else
                        dtStartDate = Value
                    End If
                End Set
            End Property

            Public Property EndDate() As DateTime
                Get
                    Return dtEndDate
                End Get
                Set(ByVal Value As DateTime)
                    If Value = Nothing Then
                        dtEndDate = Now()
                    Else
                        dtEndDate = Value
                    End If
                End Set
            End Property

            Public Property LastUpdate() As DateTime
                Get
                    Return dtLastUpdate
                End Get
                Set(ByVal Value As DateTime)
                    If Value = Nothing Then
                        dtLastUpdate = Now()
                    Else
                        dtLastUpdate = Value
                    End If
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return strUpdateBy
                End Get
                Set(ByVal Value As String)
                    strUpdateBy = Value
                End Set
            End Property

            Public Property Type() As Integer
                Get
                    Return intPromoType
                End Get
                Set(ByVal Value As Integer)
                    If Value < 0 Then
                        intPromoType = 1
                    Else
                        intPromoType = Value
                    End If
                End Set
            End Property

            Public Property Active() As Integer
                Get
                    Return intActive
                End Get
                Set(ByVal Value As Integer)
                    If Value < 0 Then
                        intActive = 0
                    Else
                        intActive = Value
                    End If
                End Set
            End Property

            Public Property PromoItem() As DataTable
                Get
                    Return dtPromoItem
                End Get
                Set(ByVal Value As DataTable)
                    dtPromoItem = Value
                End Set
            End Property

            Public Property StartTime() As DateTime
                Get
                    Return dtStartTime
                End Get
                Set(ByVal Value As DateTime)
                    If Value = Nothing Then
                        dtStartTime = Now()
                    Else
                        dtStartTime = Value
                    End If
                End Set
            End Property

            Public Property EndTime() As DateTime
                Get
                    Return dtEndTime
                End Get
                Set(ByVal Value As DateTime)
                    If Value = Nothing Then
                        dtEndTime = Now()
                    Else
                        dtEndTime = Value
                    End If
                End Set
            End Property

            Public Property PromoBatch() As String
                Get
                    Return strPromoBatch
                End Get
                Set(ByVal Value As String)
                    strPromoBatch = Value
                End Set
            End Property

            Public Property PromoLevel() As Integer
                Get
                    Return intPromoLevel
                End Get
                Set(ByVal Value As Integer)
                    If Value < 0 Then
                        intPromoLevel = 0
                    Else
                        intPromoLevel = Value
                    End If
                End Set
            End Property

            Public Property PromoPrice() As Double
                Get
                    Return dblPromoPrice
                End Get
                Set(ByVal Value As Double)
                    If Value = 0 Then
                        dblPromoPrice = 0
                    Else
                        dblPromoPrice = Value
                    End If
                End Set
            End Property

            Public Property PromoDisc() As Double
                Get
                    Return dblPromoDisc
                End Get
                Set(ByVal Value As Double)
                    If Value = 0 Then
                        dblPromoDisc = 0
                    Else
                        dblPromoDisc = Value
                    End If
                End Set
            End Property

            Public Property AryBranch() As ArrayList
                Get
                    Return aryBranchID
                End Get
                Set(ByVal Value As ArrayList)
                    aryBranchID = Value
                End Set
            End Property
        End Class
#End Region

#Region "Promo Scheme"
        Public Class MyScheme
            Inherits Core.SchemeBase

            Protected Overrides Sub InitializeInfo()
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "PromoCode"
                    .Length = 20
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(0, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "PromoDesc"
                    .Length = 50
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(1, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "QTY"
                    .Length = 4
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(2, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "PromoPrice"
                    .Length = 8
                    .DecPlace = 2
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(3, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtNumeric
                    .FieldName = "DiscPromo"
                    .Length = 4
                    .DecPlace = 4
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(4, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "StkCode"
                    .Length = 20
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(5, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "PromoBatch"
                    .Length = 20
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(6, this)
            End Sub

            Public ReadOnly Property Code() As StrucElement
                Get
                    Return MyBase.GetItem(0)
                End Get
            End Property

            Public ReadOnly Property Description() As StrucElement
                Get
                    Return MyBase.GetItem(1)
                End Get
            End Property
            Public ReadOnly Property Qty() As StrucElement
                Get
                    Return MyBase.GetItem(2)
                End Get
            End Property

            Public ReadOnly Property Price() As StrucElement
                Get
                    Return MyBase.GetItem(3)
                End Get
            End Property
            Public ReadOnly Property Discount() As StrucElement
                Get
                    Return MyBase.GetItem(4)
                End Get
            End Property

            Public ReadOnly Property StockCode() As StrucElement
                Get
                    Return MyBase.GetItem(5)
                End Get
            End Property

            Public ReadOnly Property PromoBatch() As StrucElement
                Get
                    Return MyBase.GetItem(6)
                End Get
            End Property


            Public Function GetElement(ByVal Key As Integer) As StrucElement
                Return MyBase.GetItem(Key)
            End Function

        End Class

#End Region


        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "PromoCode, PromoBatch, PromoDesc, PromoType, PromoLevel, StartDate, EndDate, StartTime, EndTime, Recurr, PromoPrice, PromoDisc, PromoQty, " & _
                              "MaxSellQty, MinTransAmt, MaxTransAmt, CreateDate, CreateBy, LastUpdate, UpdateBy, Active, Inuse, Flag"
                .CheckFields = "INUSE, FLAG"
                .TableName = "PROMOHDR"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "PromoCode, PromoBatch, PromoDesc, PromoType, PromoLevel, StartDate, EndDate, StartTime, EndTime, Recurr, PromoPrice, PromoDisc, PromoQty, " & _
                              "MaxSellQty, MinTransAmt, MaxTransAmt, CreateDate, CreateBy, LastUpdate, UpdateBy, Active, Inuse, Flag"
                .ListingCond = "FLAG = 1"
                .ShortList = "ACTIVE=1 AND FLAG=1"
                .ShortListCond = String.Empty
            End With
            With MyInfoPromoBatch
                .FieldsList = "PromoBatch, BatchDesc, StartDate, EndDate, CreateDate, CreateBy, LastUpdate, UpdateBy, Active, Inuse, Flag"
                .CheckFields = "INUSE, FLAG, ACTIVE"
                .TableName = "PROMOBATCH"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "PromoBatch, BatchDesc, StartDate, EndDate, CreateDate, CreateBy, LastUpdate, UpdateBy, Active, Inuse, Flag"
                .ShortListCond = "ACTIVE=1 AND FLAG=1"
                .ShortList = String.Empty
                .ShortListCond = String.Empty
            End With
            ReDim MyFields(2)
            MyBase.AddMyField(0, "Promo Code", "PromoCode", TypeCode.String)
            MyBase.AddMyField(1, "Promo Desc", "PromoDesc", TypeCode.String)
            MyBase.AddMyField(2, "Promo Type", "PromoType", TypeCode.String)
        End Sub

#Region "Promo Data Manipulation-Add,Edit,Del"

        Private Function AssignItem(ByVal objPromoHDR As PromoHDR, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim arySQL As ArrayList
            Dim i, intCount As Integer
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As SqlClient.SqlDataReader
            AssignItem = False
            Try
                If objPromoHDR Is Nothing Then
                    'msg return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        arySQL = New ArrayList
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "PromoCode ='" & Replace(objPromoHDR.Code, "'", "''") & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        blnExec = True 'executed - select
                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    'record is found
                                    blnFound = True
                                    strSQL = BuildDelete("PROMODTL", "PromoCode = '" & Replace(objPromoHDR.Code, "'", "''") & "'")
                                    arySQL.Add(strSQL)
                                    If Convert.ToInt16(.Item("Flag")) = 0 Then
                                        'found but deleted
                                        blnFlag = False
                                    Else
                                        'found and active
                                        blnFlag = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If
                    End If

                    If blnExec Then 'if executed
                        If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = MyInfo.TableName
                                .AddField("PromoCode", objPromoHDR.Code, SQLControl.EnumDataType.dtString)
                                .AddField("PromoBatch", objPromoHDR.PromoBatch, SQLControl.EnumDataType.dtString)
                                .AddField("PromoDesc", objPromoHDR.Description, SQLControl.EnumDataType.dtString)
                                .AddField("PromoType", objPromoHDR.Type, SQLControl.EnumDataType.dtNumeric)
                                .AddField("PromoLevel", objPromoHDR.PromoLevel, SQLControl.EnumDataType.dtNumeric)
                                .AddField("StartDate", objPromoHDR.StartDate.Date, SQLControl.EnumDataType.dtDateOnly)
                                .AddField("EndDate", objPromoHDR.EndDate.Date, SQLControl.EnumDataType.dtDateOnly)
                                .AddField("StartTime", objPromoHDR.StartTime, SQLControl.EnumDataType.dtCustTime6)
                                .AddField("EndTime", objPromoHDR.EndTime, SQLControl.EnumDataType.dtCustTime6)
                                .AddField("PromoPrice", objPromoHDR.PromoPrice, SQLControl.EnumDataType.dtNumeric)
                                .AddField("PromoDisc", objPromoHDR.PromoDisc, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Active", objPromoHDR.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LastUpdate", objPromoHDR.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", objPromoHDR.UpdateBy, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert 'INSERT
                                        'record found but not deleted
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE PromoCode = '" & Replace(objPromoHDR.Code, "'", "''") & "'")
                                        Else
                                            If blnFound = False Then 'if record not found
                                                .AddField("PromoCode", objPromoHDR.Code, SQLControl.EnumDataType.dtString)
                                                .AddField("CreateBy", objPromoHDR.UpdateBy, SQLControl.EnumDataType.dtString)
                                                .AddField("CreateDate", objPromoHDR.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate 'UPDATE
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE PromoCode='" & Replace(objPromoHDR.Code, "'", "''") & "'")
                                End Select
                                arySQL.Add(strSQL)
                            End With

                            With objSQL
                                intCount = 0
                                .TableName = "PROMODTL"
                                With objPromoHDR.PromoItem
                                    For i = 0 To .Rows.Count - 1
                                        If CType(.Rows(i).Item(1), Integer) = 1 Then
                                            objSQL.AddField("PromoCode", objPromoHDR.Code, SQLControl.EnumDataType.dtString)
                                            objSQL.AddField("PromoLevelCode", CType(.Rows(i).Item(0), String), SQLControl.EnumDataType.dtString)
                                            objSQL.AddField("PromoSeq", intCount, SQLControl.EnumDataType.dtNumeric)
                                            objSQL.AddField("PItemType", CType(.Rows(i).Item(1), String), SQLControl.EnumDataType.dtNumeric)
                                            objSQL.AddField("PromoQty", CType(.Rows(i).Item(2), String), SQLControl.EnumDataType.dtNumeric)
                                            objSQL.AddField("PromoPrice", CType(.Rows(i).Item(3), String), SQLControl.EnumDataType.dtNumeric)
                                            objSQL.AddField("PromoDisc", CType(.Rows(i).Item(4), String), SQLControl.EnumDataType.dtNumeric)
                                            strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            arySQL.Add(strSQL)
                                            intCount += 1
                                        End If
                                    Next
                                    For i = 0 To .Rows.Count - 1
                                        If CType(.Rows(i).Item(1), Integer) = 2 Then
                                            objSQL.AddField("PromoCode", objPromoHDR.Code, SQLControl.EnumDataType.dtString)
                                            objSQL.AddField("PromoLevelCode", CType(.Rows(i).Item(0), String), SQLControl.EnumDataType.dtString)
                                            objSQL.AddField("PromoSeq", intCount, SQLControl.EnumDataType.dtNumeric)
                                            objSQL.AddField("PItemType", CType(.Rows(i).Item(1), String), SQLControl.EnumDataType.dtNumeric)
                                            objSQL.AddField("PromoQty", CType(.Rows(i).Item(2), String), SQLControl.EnumDataType.dtNumeric)
                                            objSQL.AddField("PromoPrice", CType(.Rows(i).Item(3), String), SQLControl.EnumDataType.dtNumeric)
                                            objSQL.AddField("PromoDisc", CType(.Rows(i).Item(4), String), SQLControl.EnumDataType.dtNumeric)
                                            strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            arySQL.Add(strSQL)
                                            intCount += 1
                                        End If
                                    Next
                                End With
                            End With

                            Try
                                'execute
                                objDCom.BatchExecute(arySQL, CommandType.Text)
                                Return True
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002")
                                Else
                                    Throw New ApplicationException("210004")
                                End If
                            Finally
                                objSQL.Dispose()
                            End Try
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objPromoHDR = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Function Add(ByVal objPromoHDR As PromoHDR) As Boolean
            Return AssignItem(objPromoHDR, SQLControl.EnumSQLType.stInsert)
        End Function

        'AMEND
        Function Amend(ByVal objPromoHDR As PromoHDR) As Boolean
            Return AssignItem(objPromoHDR, SQLControl.EnumSQLType.stUpdate)
        End Function

        'DELETE
        Function Delete(ByVal objPromoHDR As PromoHDR) As Boolean
            Dim strSQL As String
            Dim arrsql As ArrayList
            Dim blnFound, blnInUse As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If objPromoHDR Is Nothing Then
                    'error message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        arrsql = New ArrayList
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "PromoCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPromoHDR.Code) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt32(.Item("InUse")) = 1 Then
                                        blnInUse = True 'if record is found and in use
                                    End If
                                End If
                                .Close()
                            End With
                        End If

                        'record is found and in use
                        If blnFound = True And blnInUse = True Then
                            strSQL = BuildUpdate(MyInfo.TableName, " SET Flag = " & cFlagNonActive & _
                                " , LastUpdate = '" & objPromoHDR.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPromoHDR.UpdateBy) & "'" & _
                                " WHERE PromoCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPromoHDR.Code) & "'")
                            arrsql.Add(strSQL)
                            strSQL = BuildDelete("PROMODTL", "PromoCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPromoHDR.Code) & "'")
                            arrsql.Add(strSQL)
                        End If

                        'record exist but not in use
                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(MyInfo.TableName, "PromoCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPromoHDR.Code) & "'")
                            arrsql.Add(strSQL)
                            strSQL = BuildDelete("PROMODTL", "PromoCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPromoHDR.Code) & "'")
                            arrsql.Add(strSQL)
                        End If
                        Try
                            'execute
                            objDCom.BatchExecute(arrsql, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try
                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As Exception
                Throw exDelete
            Finally
                objPromoHDR = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "PromoBatch Data Manipulation-Add,Edit,Del"

        Private Function AssignItemPromoBatch(ByVal objPromoHDR As PromoHDR, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim arySQL As ArrayList
            Dim i, intCount As Integer
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As SqlClient.SqlDataReader
            AssignItemPromoBatch = False
            Try
                If objPromoHDR Is Nothing Then
                    'msg return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        arySQL = New ArrayList
                        With MyInfoPromoBatch
                            strSQL = BuildSelect(.CheckFields, .TableName, "PromoBatch ='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPromoHDR.PromoBatch) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        blnExec = True 'executed - select
                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    'record is found
                                    blnFound = True
                                    If Convert.ToInt16(.Item("Flag")) = 0 Then
                                        'found but deleted
                                        blnFlag = False
                                    Else
                                        'found and active
                                        blnFlag = True
                                    End If
                                End If
                                .Close()
                            End With
                        End If
                    End If

                    If blnExec Then 'if executed
                        If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Return False
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = MyInfoPromoBatch.TableName
                                .AddField("BatchDesc", objPromoHDR.Description, SQLControl.EnumDataType.dtString)
                                .AddField("StartDate", objPromoHDR.StartDate.ToString, SQLControl.EnumDataType.dtDateOnly)
                                .AddField("EndDate", objPromoHDR.EndDate.ToString, SQLControl.EnumDataType.dtDateOnly)
                                .AddField("LastUpdate", objPromoHDR.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", objPromoHDR.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", objPromoHDR.Active.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", cFlagActive.ToString, SQLControl.EnumDataType.dtNumeric)
                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert 'INSERT
                                        'record found but not deleted
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE PromoBatch = '" & .ParseValue(SQLControl.EnumDataType.dtString, objPromoHDR.PromoBatch) & "'")
                                        Else
                                            If blnFound = False Then 'if record not found
                                                .AddField("PromoBatch", objPromoHDR.PromoBatch, SQLControl.EnumDataType.dtString)
                                                .AddField("CreateBy", objPromoHDR.UpdateBy, SQLControl.EnumDataType.dtString)
                                                .AddField("CreateDate", objPromoHDR.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate 'UPDATE
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE PromoBatch='" & .ParseValue(SQLControl.EnumDataType.dtString, objPromoHDR.PromoBatch) & "'")
                                End Select
                                arySQL.Add(strSQL)
                            End With

                            If objPromoHDR.Active <> 1 Then
                                strSQL = BuildUpdate(MyInfo.TableName, " SET Active = " & cFlagNonActive & _
                                    " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, objPromoHDR.LastUpdate) & " , UpdateBy = '" & _
                                    objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPromoHDR.UpdateBy) & "'" & _
                                    " WHERE PromoBatch = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPromoHDR.PromoBatch) & "'")
                                arySQL.Add(strSQL)
                            End If

                            Try
                                If arySQL.Count > 0 Then
                                    'execute
                                    objDCom.BatchExecute(arySQL, CommandType.Text)
                                End If
                                Return True
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002")
                                Else
                                    Throw New ApplicationException("210004")
                                End If
                            Finally
                                objSQL.Dispose()
                            End Try
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objPromoHDR = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Function AddPromoBatch(ByVal objPromoHDR As PromoHDR) As Boolean
            Return AssignItemPromoBatch(objPromoHDR, SQLControl.EnumSQLType.stInsert)
        End Function

        'AMEND
        Function AmendPromoBatch(ByVal objPromoHDR As PromoHDR) As Boolean
            Return AssignItemPromoBatch(objPromoHDR, SQLControl.EnumSQLType.stUpdate)
        End Function

        'DELETE
        Function DeletePromoBatch(ByVal objPromoHDR As PromoHDR) As Boolean
            Dim strSQL As String
            Dim arrsql As ArrayList
            Dim blnFound, blnInUse As Boolean
            Dim rdr As SqlClient.SqlDataReader
            DeletePromoBatch = False
            blnFound = False
            blnInUse = False
            Try
                If objPromoHDR Is Nothing Then
                    'error message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        arrsql = New ArrayList
                        With MyInfoPromoBatch
                            strSQL = BuildSelect(.CheckFields, .TableName, "PromoBatch = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPromoHDR.PromoBatch) & "'")
                        End With

                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    If Convert.ToInt32(.Item("InUse")) = 1 Then
                                        blnInUse = True 'if record is found and in use
                                    End If
                                End If
                                .Close()
                            End With
                        End If

                        'record is found and in use
                        If blnFound = True And blnInUse = True Then
                            strSQL = BuildUpdate(MyInfoPromoBatch.TableName, " SET Flag = " & cFlagNonActive & _
                                " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, objPromoHDR.LastUpdate) & " , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPromoHDR.UpdateBy) & "'" & _
                                " WHERE PromoBatch = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPromoHDR.PromoBatch) & "'")
                            arrsql.Add(strSQL)
                        End If

                        'record exist but not in use
                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(MyInfoPromoBatch.TableName, "PromoBatch = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPromoHDR.PromoBatch) & "'")
                            arrsql.Add(strSQL)
                        End If

                        strSQL = BuildUpdate(MyInfo.TableName, " SET Flag = " & cFlagNonActive & _
                        " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, objPromoHDR.LastUpdate) & " , UpdateBy = '" & _
                        objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPromoHDR.UpdateBy) & "'" & _
                        " WHERE PromoBatch = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objPromoHDR.PromoBatch) & "'")
                        arrsql.Add(strSQL)

                        Try
                            If arrsql.Count > 0 Then
                                'execute
                                objDCom.BatchExecute(arrsql, CommandType.Text)
                            End If
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try
                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As Exception
                Throw exDelete
            Finally
                objPromoHDR = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Function AddPromoBranch(ByVal objPromoHDR As PromoHDR, ByVal Branch As String) As Boolean
            Return AssignItemPromoBranch(objPromoHDR, SQLControl.EnumSQLType.stInsert, Branch)
        End Function

        Function AssignItemPromoBranch(ByVal objPromoHDR As PromoHDR, ByVal pType As SQLControl.EnumSQLType, ByVal BranchID As String) As Boolean
            Dim strSQL As String
            Dim arySQL As ArrayList
            Dim i, intCount As Integer
            Dim blnExec As Boolean, blnFound As Boolean
            Dim rdr As SqlClient.SqlDataReader
            AssignItemPromoBranch = False
            Try
                If objPromoHDR Is Nothing Then
                    'msg return
                Else
                    blnExec = False
                    blnFound = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        arySQL = New ArrayList
                        strSQL = "SELECT PromoBatch, PromoCode, BranchID FROM PROMOBRANCH " & _
                                "WHERE PromoBatch = '" & Convert.ToString(objPromoHDR.PromoBatch) & "' " & _
                                "AND PromoCode = '" & Convert.ToString(objPromoHDR.Code) & "' " & _
                                "AND BranchID = '" & Convert.ToString(BranchID) & "' "
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        blnExec = True 'executed - select
                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    'record is found
                                    blnFound = True
                                End If
                                .Close()
                            End With
                        End If
                    End If

                    If blnExec Then 'if executed
                        If blnFound = False Then
                            StartSQLControl()
                            With objSQL
                                .TableName = "PROMOBRANCH"
                                .AddField("PromoBatch", objPromoHDR.PromoBatch, SQLControl.EnumDataType.dtString)
                                .AddField("PromoCode", objPromoHDR.Code, SQLControl.EnumDataType.dtString)
                                .AddField("BranchID", Convert.ToString(BranchID), SQLControl.EnumDataType.dtString)
                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                arySQL.Add(strSQL)
                            End With
                        End If

                        Try
                            If arySQL.Count > 0 Then
                                objDCom.BatchExecute(arySQL, CommandType.Text)
                            End If
                            Return True

                        Catch axExecute As Exception
                            If pType = SQLControl.EnumSQLType.stInsert Then
                                Throw New ApplicationException("210002")
                            Else
                                Throw New ApplicationException("210004")
                            End If
                        Finally
                            objSQL.Dispose()
                        End Try
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objPromoHDR = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        'Enquiry
        Function Enquiry(ByVal pType As General.PromoBase.EnumPromo, Optional ByVal DocCode As String = Nothing, Optional ByVal SQLstmt As String = Nothing) As DataSet
            Try
                StartConnection()
                StartSQLControl()

                With MyInfo
                    If pType = EnumPromo.PromoHDR Then
                        strSQL = "SELECT PromoCode, PromoBatch, PromoDesc, PromoType, PromoLevel, StartDate, EndDate, StartTime, EndTime, Recurr, PromoPrice, PromoDisc, PromoQty, " & _
                                  "MaxSellQty, MinTransAmt, MaxTransAmt, CreateDate, CreateBy, LastUpdate, UpdateBy, Active, Inuse, Flag FROM PROMOHDR "
                    ElseIf pType = EnumPromo.PromoDTL Then
                        strSQL = "SELECT PromoLevelCode, PItemType, PromoQty, PromoPrice, PromoDisc FROM PromoDTL WHERE PromoCode = '" & Replace(DocCode, "'", "''") & "'"
                    End If

                    If strSQL Is Nothing = False Then
                        Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                    End If
                End With

            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Function EnquiryPromoBatch(Optional ByVal SQLstmt As String = Nothing) As DataSet
            If StartConnection() = True Then
                With MyInfoPromoBatch
                    If SQLstmt = Nothing Or SQLstmt = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName, .ShortListCond)
                    Else
                        strSQL = SQLstmt
                    End If
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetListing() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetShortList() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
        End Function
#End Region

        Public Shared Function CheckStkCodeExist(ByVal BranchID As String, ByVal StkCode As String) As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Try
                StartConnection()
                StartSQLControl()

                strSQL = "SELECT StkCode FROM STOCK WHERE BranchID = '" & BranchID & "' AND StkCode = '" & StkCode & "' "

                rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                If rdr Is Nothing = False Then
                    If rdr.HasRows = True Then
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If

            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                rdr.Close()
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
        Public Shared Function GetPromoInfo(ByVal BizDate As DateTime, ByVal BranchID As String) As DataSet
            Dim dsPromo As New DataSet
            Dim rdr As SqlClient.SqlDataReader
            Try
                StartConnection()
                StartSQLControl()

                strSQL = "SELECT  PH.PromoCode, PH.PromoBatch, PH.PromoDesc, PH.PromoType, PH.PromoLevel, PH.StartDate, " & _
                    "PH.EndDate, PH.StartTime, PH.EndTime, PH.PromoPrice, PH.PromoDisc, PH.PromoQty, " & _
                    "PH.MaxSellQty, PH.MinTransAmt, PH.MaxTransAmt, PH.CreateDate, PH.CreateBy, PH.LastUpdate, " & _
                    "PH.UpdateBy, PH.Active, PH.Inuse, PH.Flag FROM PROMOHDR PH " & _
                    "INNER JOIN PROMOBRANCH PB ON PH.PromoBatch = PB.PromoBatch AND PH.PromoCode = PB.PromoCode " & _
                    "WHERE StartDate <= " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateOnly, BizDate.ToString) & " AND EndDate >= " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateOnly, BizDate.ToString) & " " & _
                    "AND Active = 1 AND Flag = 1 AND PB.BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BranchID) & "' " & _
                    "ORDER BY PH.PromoLevel, PH.PromoCode"


                rdr = CType(objDCom.Execute(strSQL, CommandType.Text, CommandBehavior.CloseConnection, False), SqlClient.SqlDataReader)

                If rdr Is Nothing = False Then
                    dsPromo.Tables.Add(objDCom.ConvertReaderToTable(rdr, "PromoHdr"))
                    rdr.Close()
                End If

                strSQL = "SELECT PD.PromoCode, PH.PromoType, PH.StartDate, PH.EndDate, PH.StartTime, PH.EndTime, PD.PromoLevelCode, PD.PromoSeq, PD.PromoQty, PD.PItemType, PD.PromoPrice, PD.PromoDisc " & _
                  "FROM PROMODTL PD INNER JOIN PROMOHDR PH ON PD.PromoCode = PH.PromoCode " & _
                  "INNER JOIN PROMOBRANCH PB ON PH.PromoBatch = PB.PromoBatch AND PH.PromoCode = PB.PromoCode " & _
                  "WHERE PH.StartDate <= " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateOnly, BizDate.ToString) & " AND PH.EndDate >= " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateOnly, BizDate.ToString) & " " & _
                  "AND PB.BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BranchID) & "' AND PH.Active = 1 AND PH.Flag = 1"

                rdr = CType(objDCom.Execute(strSQL, CommandType.Text, CommandBehavior.CloseConnection, False), SqlClient.SqlDataReader)
                If rdr Is Nothing = False Then
                    dsPromo.Tables.Add(objDCom.ConvertReaderToTable(rdr, "PromoDtl"))
                    rdr.Close()
                End If

                'strSQL = "SELECT PC.PromoCode, PH.PromoType, PH.StartDate, PH.EndDate, PH.StartTime, PH.EndTime, PC.StkCatg, PC.StkSeq, PC.PromoQty, PC.PItemType, PC.PromoPrice, PC.PromoDisc " & _
                ' "FROM PROMOCATG PC INNER JOIN PROMOHDR PH ON PC.PromoCode = PH.PromoCode " & _
                ' "WHERE PH.StartDate <= " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateOnly, BizDate.ToString) & " AND PH.EndDate >= " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateOnly, BizDate.ToString) & " " & _
                ' "AND PH.Active = 1 AND PH.Flag = 1 AND PH.PromoLevel = " & Convert.ToInt32(EnumPromoLevel.Category).ToString

                'rdr = CType(objDCom.Execute(strSQL, CommandType.Text, CommandBehavior.CloseConnection, False), SqlClient.SqlDataReader)
                'dsPromo.Tables.Add(objDCom.ConvertReaderToTable(rdr, "PromoCatg"))
                'rdr.Close()


                'strSQL = "SELECT PB.PromoCode, PH.PromoType, PH.StartDate, PH.EndDate, PH.StartTime, PH.EndTime, PB.StkBrand, PB.StkSeq, PB.PromoQty, PB.PItemType, PB.PromoPrice, PB.PromoDisc " & _
                '"FROM PROMOBRAND PB INNER JOIN PROMOHDR PH ON PB.PromoCode = PH.PromoCode " & _
                '"WHERE PH.StartDate <= " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateOnly, BizDate.ToString) & " AND PH.EndDate >= " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateOnly, BizDate.ToString) & " " & _
                '"AND PH.Active = 1 AND PH.Flag = 1 AND PH.PromoLevel = " & Convert.ToInt32(EnumPromoLevel.Category).ToString

                'rdr = CType(objDCom.Execute(strSQL, CommandType.Text, CommandBehavior.CloseConnection, False), SqlClient.SqlDataReader)
                'dsPromo.Tables.Add(objDCom.ConvertReaderToTable(rdr, "PromoBrand"))
                'rdr.Close()

                Return dsPromo
            Catch ex As Exception
                Return Nothing
            Finally
                rdr = Nothing
                EndConnection()
                EndSQLControl()
            End Try
        End Function

        Public Overloads Function GetStkCodeList(ByVal StyleCode As String, ByVal BranchID As String) As DataTable
            Try
                If StartConnection() = True Then
                    strSQL = "SELECT StkCode FROM Stock " & _
                            "WHERE SUBSTRING(stkCode, 9, 5) = '" & StyleCode & "' " & _
                            "AND BRANCHID = '" & BranchID & "'"
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text), DataTable)
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Return Nothing
            Finally
                EndConnection()
            End Try
        End Function

        Public Function ValidateRetailPrice(ByVal BranchID As String, ByVal PromoItem As DataTable) As Boolean
            Dim strSQL As String
            Dim aryBranch() As String
            Dim i As Integer
            Dim dtTemp As New DataTable
            Dim drRow, drRow2 As DataRow
            Dim rdr As SqlClient.SqlDataReader

            aryBranch = Split(BranchID, ",")
            For i = 0 To aryBranch.GetUpperBound(0)
                For Each drRow In PromoItem.Rows
                    dtTemp = GetStkCodeList(Convert.ToString(drRow(0)), aryBranch(i))
                    If dtTemp Is Nothing = False Then
                        For Each drRow2 In dtTemp.Rows
                            strSQL = "SELECT STKPRICE.StkCode FROM STKPRICE " & _
                                    "INNER JOIN (SELECT BranchID, StkCode, MAX(EffDate) AS 'EffDate' FROM StkPrice " & _
                                    "WHERE PriceLevel = '0' GROUP BY BranchID, StkCode) Table1 " & _
                                    "ON STKPRICE.BranchID = Table1.BranchID AND STKPRICE.StkCode = Table1.StkCode " & _
                                    "AND STKPRICE.EffDate = Table1.EffDate " & _
                                    "WHERE (STKPRICE.PriceLevel = '0') AND (STKPRICE.StkCode = '" & Convert.ToString(drRow2.Item(0)) & "') " & _
                                    "AND (STKPRICE.SellPrice <> '" & Convert.ToString(drRow.Item(1)) & "') AND (STKPRICE.BranchID = '" & aryBranch(i) & "')"

                            rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                            If rdr Is Nothing = False Then
                                If rdr.HasRows = True Then
                                    rdr.Close()
                                    MsgBox(Convert.ToString(drRow2(0)) & " in " & aryBranch(i) & " not sold at " & Convert.ToString(drRow(1)))
                                    Return False
                                Else
                                    rdr.Close()
                                End If
                            Else
                                rdr.Close()
                            End If
                        Next
                    End If
                Next
            Next
            Return True
        End Function

        'Public Function DeleteCheck(ByVal TableName As String, ByVal Criteria As String, Optional ByVal IsFlag As Boolean = False) As Boolean
        '    Try
        '        Dim strCond As String
        '        Dim rdr As SqlClient.SqlDataReader

        '        If StartConnection() = True Then
        '            StartSQLControl()
        '            If IsFlag = True Then
        '                strCond += " Flag = 0 "
        '            End If
        '            strCond += Criteria
        '            strSQL = BuildSelect("*", TableName, strCond)
        '            rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

        '            If rdr Is Nothing = False Then
        '                With rdr
        '                    If .Read Then
        '                        Return True
        '                    Else
        '                        Return False
        '                    End If
        '                    .Close()
        '                End With
        '            End If

        '        End If

        '    Catch ex As Exception
        '        Return False
        '    Finally
        '        EndSQLControl()
        '        EndConnection()
        '    End Try
        'End Function

        'Clear all data in database
        Public Function ClearDB() As Boolean
        End Function

    End Class
#End Region

#Region "Credit Note"
    Public Class CreditNoteBase
        Inherits Core.CoreBase

        Public Enum EnumCreditNoteStatus
            Active = 0
            Inactive = 1
            UseForPayment = 2
        End Enum

#Region "Credit Note In Credit Note Base"
        Public Class CreditNote
            Inherits Core.SingleBase

            Protected strCreditNote, strUpdateBy As String
            Protected dtExpDate, dtLastUpdate As Date
            Protected intStatus As Int16

            Public Property CreditNote() As String
                Get
                    Return strCreditNote
                End Get
                Set(ByVal Value As String)
                    strCreditNote = Value
                End Set
            End Property

            Public Property ExpiryDate() As Date
                Get
                    Return dtExpDate
                End Get
                Set(ByVal Value As Date)
                    dtExpDate = Value
                End Set
            End Property

            Public Property Status() As Int16
                Get
                    Return intStatus
                End Get
                Set(ByVal Value As Int16)
                    intStatus = Value
                End Set
            End Property

            Public Property LastUpdate() As DateTime
                Get
                    Return dtLastUpdate
                End Get
                Set(ByVal Value As DateTime)
                    dtLastUpdate = Value
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return strUpdateBy
                End Get
                Set(ByVal Value As String)
                    strUpdateBy = Value
                End Set
            End Property

        End Class
#End Region

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "CNNo, CustomerID, AccountNo, BranchID, CustPkgID, CNAmt, ExpiryDate, Status, CNDate, CNTime, CashierID, LastUpdate, UpdateBy"
                .CheckFields = String.Empty
                .TableName = "CREDITNOTELOG"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "CNNo, CustomerID, AccountNo, BranchID, CustPkgID, CNAmt, ExpiryDate, Status, CNDate, CNTime, CashierID, LastUpdate, UpdateBy"
                .ListingCond = String.Empty
                .ShortList = "CNNo, CustomerID, AccountNo, BranchID, CustPkgID, CNAmt, ExpiryDate, Status"
                .ShortListCond = String.Empty
            End With
            ReDim MyFields(1)
            MyBase.AddMyField(0, "Credit Note No.", "CNNo", TypeCode.String)
            MyBase.AddMyField(1, "Customer ID", "CustomerID", TypeCode.String)
        End Sub

#Region "Data Manipulation-Add,Edit,Del"

        Private Function AssignItem(ByVal objCreditNote As CreditNote, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim arrSQL As New ArrayList
            Dim blnExec As Boolean, blnFound As Boolean
            Dim intRtn As Integer
            Dim rdr As SqlClient.SqlDataReader
            AssignItem = False
            Try
                If objCreditNote Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False

                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.FieldsList, .TableName, "CNNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCreditNote.CreditNote) & "'")
                        End With

                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        blnExec = True

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                End If
                                .Close()
                            End With
                        End If
                    End If

                    If blnExec Then
                        If blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                            'Item found & active
                            Return False
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = "CREDITNOTELOG"
                                .AddField("ExpiryDate", objCreditNote.ExpiryDate.ToString, SQLControl.EnumDataType.dtDateOnly)
                                .AddField("Status", objCreditNote.Status.ToString, SQLControl.EnumDataType.dtNumeric)
                                .AddField("UpdateBy", objCreditNote.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", objCreditNote.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert

                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                        "WHERE CNNo='" & .ParseValue(SQLControl.EnumDataType.dtString, objCreditNote.CreditNote) & "'")

                                End Select

                            End With

                            Try
                                'execute
                                objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                Return True
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002")
                                Else
                                    Throw New ApplicationException("210004")
                                End If
                            Finally
                                objSQL.Dispose()
                            End Try
                            Return True
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objCreditNote = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
        'ADD
        Function Add(ByVal objCreditNote As CreditNote) As Boolean
            Return AssignItem(objCreditNote, SQLControl.EnumSQLType.stInsert)
        End Function
        'AMEND
        Function Amend(ByVal objCreditNote As CreditNote) As Boolean
            Return AssignItem(objCreditNote, SQLControl.EnumSQLType.stUpdate)
        End Function
        'DELETE
        Function Delete(ByVal objCreditNote As CreditNote) As Boolean

        End Function
#End Region

#Region "Data Selection"
        Public Shared Sub InsertCreditNote(ByRef arrList As ArrayList, ByRef sCreditNoteNo As String, _
            ByVal CNType As Integer, ByVal BranchID As String, ByVal TermID As Integer, ByVal CustomerID As String, _
            ByVal AcctNo As String, ByVal CNAmount As String, _
            ByVal ExpiryDate As String, ByVal CNDate As String, ByVal CashierID As String, ByVal CurDate As Date, Optional ByVal CustPkgID As String = "")
            Dim strCreditNoteNo As String = String.Empty
            Dim iCreditNote As Integer = 0
            Dim AccountNo As String



            strCreditNoteNo = General.SysCodeBase.GenerateCreditNoteNo(BranchID, TermID, CurDate, iCreditNote)
            sCreditNoteNo = strCreditNoteNo
            StartSQLControl()
            With objSQL
                .TableName = "CreditNoteLog"
                .AddField("CNNo", strCreditNoteNo, SQLControl.EnumDataType.dtString)
                .AddField("CNType", CNType, SQLControl.EnumDataType.dtNumeric)
                .AddField("BranchID", BranchID, SQLControl.EnumDataType.dtString)
                .AddField("CustomerID", CustomerID, SQLControl.EnumDataType.dtString)
                .AddField("AccountNo", AcctNo, SQLControl.EnumDataType.dtString)
                .AddField("CustPkgID", CustPkgID, SQLControl.EnumDataType.dtString)
                .AddField("CNAmt", CNAmount, SQLControl.EnumDataType.dtNumeric)
                If ExpiryDate <> String.Empty Then
                    .AddField("ExpiryDate", ExpiryDate, SQLControl.EnumDataType.dtDateOnly)
                End If
                .AddField("CNDate", CNDate, SQLControl.EnumDataType.dtDateOnly)
                .AddField("CNTime", Convert.ToString(Now), SQLControl.EnumDataType.dtCustTime6)
                .AddField("CashierID", CashierID, SQLControl.EnumDataType.dtString)
                .AddField("LastUpdate", CNDate, SQLControl.EnumDataType.dtDateTime)
                .AddField("UpdateBy", CashierID, SQLControl.EnumDataType.dtString)
                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                arrList.Add(strSQL)
            End With
            General.SysCodeBase.UpdateCreditNoteNo(BranchID, TermID, iCreditNote)
        End Sub
#End Region
#Region "Data Selection"
        Public Shared Sub InsertCreditNote2(ByRef arrList As ArrayList, ByRef strCreditNoteNo As String, _
            ByVal CNType As Integer, ByVal BranchID As String, ByVal TermID As Integer, ByVal CustomerID As String, _
            ByVal AcctNo As String, ByVal CNAmount As String, _
            ByVal ExpiryDate As String, ByVal CNDate As String, ByVal CashierID As String, ByVal CurDate As Date, Optional ByVal CustPkgID As String = "")

            Dim iCreditNote As Integer = 0



            StartSQLControl()
            With objSQL
                .TableName = "CreditNoteLog"
                .AddField("CNNo", strCreditNoteNo, SQLControl.EnumDataType.dtString)
                .AddField("CNType", CNType, SQLControl.EnumDataType.dtNumeric)
                .AddField("BranchID", BranchID, SQLControl.EnumDataType.dtString)
                .AddField("CustomerID", CustomerID, SQLControl.EnumDataType.dtString)
                .AddField("AccountNo", AcctNo, SQLControl.EnumDataType.dtString)
                .AddField("CustPkgID", CustPkgID, SQLControl.EnumDataType.dtString)
                .AddField("CNAmt", CNAmount, SQLControl.EnumDataType.dtNumeric)
                If ExpiryDate <> String.Empty Then
                    .AddField("ExpiryDate", ExpiryDate, SQLControl.EnumDataType.dtDateOnly)
                End If
                .AddField("CNDate", CNDate, SQLControl.EnumDataType.dtDateOnly)
                .AddField("CNTime", Convert.ToString(Now), SQLControl.EnumDataType.dtCustTime6)
                .AddField("CashierID", CashierID, SQLControl.EnumDataType.dtString)
                .AddField("LastUpdate", CNDate, SQLControl.EnumDataType.dtDateTime)
                .AddField("UpdateBy", CashierID, SQLControl.EnumDataType.dtString)
                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                arrList.Add(strSQL)
            End With
            General.SysCodeBase.UpdateCreditNoteNo(BranchID, TermID, iCreditNote)
        End Sub




        Public Shared Sub UpdateCreditNoteStatus(ByRef arrList As ArrayList, ByVal CreditNoteNo As String, ByVal Status As EnumCreditNoteStatus, ByVal UpdateBy As String, ByVal LastUpdate As DateTime)
            Dim sSQL As String
            Try
                StartSQLControl()

                With objSQL
                    .TableName = "CreditNoteLog"
                    .AddField("Status", Convert.ToInt32(Status).ToString, SQLControl.EnumDataType.dtNumeric)
                    .AddField("UpdateBy", UpdateBy, SQLControl.EnumDataType.dtString)
                    .AddField("LastUpdate", LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                    sSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, "CreditNoteLog", "WHERE CNNO = '" & CreditNoteNo & "'")
                    arrList.Add(sSQL)
                End With

            Catch ex As Exception

            End Try
        End Sub

        Public Shared Function GetCreditNoteInfo(ByVal CreditNoteNo As String) As DataSet
            Dim dsReceipt As New DataSet
            Dim rdr As SqlClient.SqlDataReader
            Dim drRow As DataRow
            Dim strSql As String

            Try
                StartSQLControl()
                StartConnection()

                strSql = "SELECT CN.CNNo, CN.BranchID, CN.CustomerID, C.Surname, C.FirstName, CN.AccountNo, CN.CustPkgID, " & _
                    "CP.PkgCode, CP.PkgDesc, CP.PkgType, CN.CNAmt, CN.ExpiryDate, CN.Status, CN.CNDate, CN.CNTime, CN.CashierID, " & _
                    "CN.LastUpdate, CN.UpdateBy, U.UserName FROM CreditNoteLog CN Inner Join Customer C ON CN.CustomerID = C.CustomerID " & _
                    "LEFT OUTER JOIN CustPkgHdr CP ON CN.CustPkgID = CP.CustPkgID INNER JOIN UsrProfile U ON CN.CashierID = U.UserID " & _
                    "WHERE CN.CNNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CreditNoteNo) & "'"

                rdr = CType(objDCom.Execute(strSql, CommandType.Text, CommandBehavior.CloseConnection, False), SqlClient.SqlDataReader)
                If rdr Is Nothing = False Then
                    dsReceipt.Tables.Add(objDCom.ConvertReaderToTable(rdr, "CreditNoteLog"))
                    rdr.Close()
                End If
                Return dsReceipt
            Catch ex As Exception

            Finally
                rdr = Nothing
                EndConnection()
                EndSQLControl()
            End Try
        End Function

        Public Shared Function GetCreditNoteCombo(ByVal CustomerID As String, ByVal AccountNo As String, ByVal BizDate As DateTime) As DataTable
            Dim rdr As SqlClient.SqlDataReader
            Try
                StartSQLControl()
                StartConnection()
                strSQL = "SELECT CNNo, CNAmt, CASE WHEN CNType = 1 THEN 'CN' WHEN cntype = 2 THEN 'CV' END AS CNType " & _
                    "FROM CreditNoteLog WHERE CustomerID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CustomerID) & "' " & _
                    "AND AccountNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, AccountNo) & "' AND Status = 1 " & _
                    "AND ExpiryDate >= " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateOnly, BizDate.ToString)

                rdr = CType(objDCom.Execute(strSQL, CommandType.Text, CommandBehavior.CloseConnection, False), SqlClient.SqlDataReader)
                If rdr Is Nothing = False Then
                    GetCreditNoteCombo = objDCom.ConvertReaderToTable(rdr, "CreditNoteLog")
                    rdr.Close()
                End If

            Catch ex As Exception
                Return Nothing
            Finally
                rdr = Nothing
                EndConnection()
                EndSQLControl()
            End Try
        End Function

        Public Shared Function GetCreditNoteComboList(ByVal CustomerID As String, ByVal AccountNo As String, ByVal BizDate As DateTime) As DataTable

            Dim rdr As SqlClient.SqlDataReader
            Try
                StartSQLControl()
                StartConnection()
                strSQL = "SELECT CNNo, CNAmt, CASE WHEN CNType = 1 THEN 'CN' WHEN cntype = 2 THEN 'CV' END AS CNType " & _
                    "FROM CreditNoteLog WHERE CustomerID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CustomerID) & "' " & _
                    "AND AccountNo IN (" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, AccountNo) & ") AND Status = 1 " & _
                    "AND ExpiryDate >= " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateOnly, BizDate.ToString)

                rdr = CType(objDCom.Execute(strSQL, CommandType.Text, CommandBehavior.CloseConnection, False), SqlClient.SqlDataReader)
                If rdr Is Nothing = False Then
                    GetCreditNoteComboList = objDCom.ConvertReaderToTable(rdr, "CreditNoteLog")
                    rdr.Close()
                End If

            Catch ex As Exception
                Return Nothing
            Finally
                rdr = Nothing
                EndConnection()
                EndSQLControl()
            End Try
        End Function

        Public Shared Function GetCreditNoteAmount(ByVal CreditNoteNo As String) As Double
            'Dim rdr As SqlClient.SqlDataReader
            Dim dblAmt As Double = 0

            Try
                strSQL = "SELECT CNAmt FROM CreditNoteLog WHERE CNNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CreditNoteNo) & "'"

                dblAmt = GetNumeric(strSQL)
                GetCreditNoteAmount = dblAmt
            Catch ex As Exception
                Return 0
            End Try
        End Function

        Public Shared Function GetVoucherInfo(ByVal CreditNoteNo As String) As DataSet
            Dim dsReceipt As New DataSet
            Dim rdr As SqlClient.SqlDataReader
            Dim drRow As DataRow
            Dim strSql As String

            Try
                StartSQLControl()
                StartConnection()

                strSql = "SELECT CN.CNNo, CN.CustomerID, CN.AccountNo, CN.BranchID, CN.CNAmt, CN.ExpiryDate, CN.CNDate, CN.CNTime, C.Surname, C.FirstName " & _
                         "FROM CREDITNOTELOG CN INNER JOIN CUSTOMER C ON CN.CustomerID = C.CustomerID " & _
                         "WHERE CN.CNNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CreditNoteNo) & "'"

                rdr = CType(objDCom.Execute(strSql, CommandType.Text, CommandBehavior.CloseConnection, False), SqlClient.SqlDataReader)
                If rdr Is Nothing = False Then
                    dsReceipt.Tables.Add(objDCom.ConvertReaderToTable(rdr, "CreditNoteLog"))
                    rdr.Close()
                End If
                Return dsReceipt
            Catch ex As Exception

            Finally
                rdr = Nothing
                EndConnection()
                EndSQLControl()
            End Try
        End Function


        'Enquiry
        Function Enquiry(Optional ByVal SQLstmt As String = Nothing) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = "SELECT CREDITNOTELOG.CNNo, CREDITNOTELOG.CustomerID, CUSTOMER.FirstName + ' ' + CUSTOMER.Surname AS CustomerName," & _
                             " CREDITNOTELOG.AccountNo, CREDITNOTELOG.BranchID, CREDITNOTELOG.CustPkgID, CUSTPKGHDR.PkgCode, CUSTPKGHDR.PkgDesc," & _
                             " CREDITNOTELOG.CNAmt, CREDITNOTELOG.ExpiryDate, CREDITNOTELOG.Status, CREDITNOTELOG.CNDate, CREDITNOTELOG.CNTime," & _
                             " CREDITNOTELOG.CashierID, USRPROFILE.UserName, CREDITNOTELOG.LastUpdate, CREDITNOTELOG.UpdateBy" & _
                             " FROM CREDITNOTELOG INNER JOIN" & _
                             " CUSTOMER ON CREDITNOTELOG.CustomerID = CUSTOMER.CustomerID INNER JOIN" & _
                             " CUSTPKGHDR ON CREDITNOTELOG.CustPkgID = CUSTPKGHDR.CustPkgID INNER JOIN" & _
                             " USRPROFILE ON CREDITNOTELOG.CashierID = USRPROFILE.UserID"
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

#End Region

    End Class
#End Region

#Region "SynchronizeRecord Base"
    Public Class SynchronizeRecordBase
        Inherits Core.CoreBase

        Public Enum ActionType
            STOCK = 0
            STKPRICE = 1
            STKCOMBO = 2
            STKMATERIAL = 3
            SYSPREFB = 4
            SYSPREFT = 5
        End Enum

#Region "SynchronizeRecordClass in SynchronizeRecord Base"
        Public Class SynchronizeRecord
            Inherits Core.SingleBase
            Protected strSrcBranchID As String
            Protected strDesBranchID As String
            Protected intType As Integer
            Protected strCondition As String
            Protected dtDate As Date
            Protected intTermID As Integer

            Public Property SrcBranchID() As String
                Get
                    Return strSrcBranchID
                End Get
                Set(ByVal Value As String)
                    strSrcBranchID = Value
                End Set
            End Property

            Public Property DesBranchID() As String
                Get
                    Return strDesBranchID
                End Get
                Set(ByVal Value As String)
                    strDesBranchID = Value
                End Set
            End Property

            Public Property Type() As Integer
                Get
                    Return intType
                End Get
                Set(ByVal Value As Integer)
                    intType = Value
                End Set
            End Property

            Public Property Condition() As String
                Get
                    Return strCondition
                End Get
                Set(ByVal Value As String)
                    strCondition = Value
                End Set
            End Property

            Public Property EffDate() As Date
                Get
                    Return dtDate
                End Get
                Set(ByVal Value As Date)
                    dtDate = Value
                End Set
            End Property

            Public Property TermID() As Integer
                Get
                    Return intTermID
                End Get
                Set(ByVal Value As Integer)
                    intTermID = Value
                End Set
            End Property

        End Class
#End Region

#Region "Function"

        Public Function Duplicate(ByVal objSynchronizeRecord As SynchronizeRecord) As Boolean
            Dim i As Integer
            Try
                StartConnection()
                StartSQLControl()
                If objSynchronizeRecord Is Nothing = False Then
                    objDCom.AddParameter("@BranchIDSrc", ParameterDirection.Input, objSynchronizeRecord.SrcBranchID, SqlDbType.VarChar, 10)
                    objDCom.AddParameter("@BranchIDDes", ParameterDirection.Input, objSynchronizeRecord.DesBranchID, SqlDbType.VarChar, 10)
                    objDCom.AddParameter("@Condition", ParameterDirection.Input, objSynchronizeRecord.Condition, SqlDbType.VarChar, 50)
                    Select Case objSynchronizeRecord.Type
                        Case 0
                            objDCom.AddParameter("@IsUpdated", ParameterDirection.Input, 0, SqlDbType.NVarChar, 1)
                            objDCom.Execute("sp_STOCK", DataAccess.EnumRtnType.rtNone, CommandType.StoredProcedure)
                        Case 1
                            'objDCom.AddParameter("@NowDate", ParameterDirection.Input, objSQL.ParseValue(SQLControl.EnumDataType.dtDateOnly, objSynchronizeRecord.EffDate.ToShortDateString), SqlDbType.VarChar, 40)
                            objDCom.AddParameter("@IsUpdated", ParameterDirection.Input, 0, SqlDbType.NVarChar, 1)
                            objDCom.AddParameter("@NowDate", ParameterDirection.Input, objSynchronizeRecord.EffDate.ToShortDateString, SqlDbType.VarChar, 40)
                            objDCom.AddParameter("@StkCodeList", ParameterDirection.Input, String.Empty, SqlDbType.NVarChar, 4000)
                            objDCom.Execute("sp_STKPRICE", DataAccess.EnumRtnType.rtNone, CommandType.StoredProcedure)
                        Case 2
                            objDCom.AddParameter("@IsUpdated", ParameterDirection.Input, 0, SqlDbType.NVarChar, 1)
                            objDCom.Execute("sp_STKCOMBO", DataAccess.EnumRtnType.rtNone, CommandType.StoredProcedure)
                        Case 3
                            objDCom.Execute("sp_STKMATERIAL", DataAccess.EnumRtnType.rtNone, CommandType.StoredProcedure)
                        Case 4
                            objDCom.Execute("sp_SYSPREFB", DataAccess.EnumRtnType.rtNone, CommandType.StoredProcedure)
                        Case 5
                            objDCom.AddParameter("@TerminalID", ParameterDirection.Input, objSynchronizeRecord.TermID, SqlDbType.VarChar, 1)
                            objDCom.Execute("sp_SYSPREFT", DataAccess.EnumRtnType.rtNone, CommandType.StoredProcedure)
                    End Select
                End If
            Catch ex As Exception
                Return False
            Finally
                EndConnection()
            End Try
        End Function

        Public Function GetTypeItem(ByVal sType As ActionType, Optional ByVal BranchID As String = "") As DataSet
            Dim i As Integer
            Try
                StartConnection()
                StartSQLControl()
                Select Case sType
                    Case ActionType.STOCK
                        strSQL = "SELECT 1 AS SELECTED, StkCode, StockDesc, TypeCode, BehvType FROM STOCK WHERE BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BranchID) & "'"
                    Case ActionType.STKPRICE
                        strSQL = "SELECT DISTINCT 1 AS SELECTED, STKPRICE.StkCode, STOCK.StockDesc, STOCK.TypeCode, STOCK.BehvType FROM STKPRICE INNER JOIN " & _
                                 "STOCK ON STKPRICE.BranchID = STOCK.BranchID AND STKPRICE.StkCode = STOCK.StkCode WHERE STKPRICE.BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BranchID) & "'"
                    Case ActionType.STKCOMBO
                        strSQL = "SELECT DISTINCT 1 AS SELECTED, STKCOMBO.StkCombo, STOCK.StockDesc, STKCOMBO.ComboType, STOCK.TypeCode, STOCK.BehvType FROM STKCOMBO INNER JOIN " & _
                                 "STOCK ON STKCOMBO.BranchID = STOCK.BranchID AND STKCOMBO.StkCombo = STOCK.StkCode WHERE STKCOMBO.BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BranchID) & "'"
                    Case ActionType.STKMATERIAL
                        strSQL = "SELECT DISTINCT 1 AS SELECTED, STKMATERIAL.StkMaterial, STOCK.StockDesc, STOCK.TypeCode, STOCK.BehvType FROM STKMATERIAL INNER JOIN " & _
                                 "STOCK ON STKMATERIAL.BranchID = STOCK.BranchID AND STKMATERIAL.StkMaterial = STOCK.StkCode WHERE STKMATERIAL.BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BranchID) & "'"
                    Case ActionType.SYSPREFB
                        strSQL = "SELECT 1 AS SELECTED, SYSKey, SYSValue, SYSSet FROM SYSPREFB WHERE BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BranchID) & "'"
                    Case ActionType.SYSPREFT
                        strSQL = "SELECT 1 AS SELECTED, TermID, SYSKey, SYSValue, SYSSet FROM SYSPREFT WHERE BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BranchID) & "'"
                End Select
                Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, "TypeItem"), DataSet)
            Catch exGetTypeItem As Exception
                Return Nothing
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function FilterTerminalID(Optional ByVal BranchID As String = "", Optional ByVal TermID As String = "") As DataSet
            Try
                StartConnection()
                StartSQLControl()
                strSQL = "SELECT 1 AS SELECTED, TermID, SYSKey, SYSValue, SYSSet FROM SYSPREFT WHERE BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BranchID) & "'" & " AND TermID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, TermID) & "'"
                Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, "TypeItem"), DataSet)
            Catch ex As Exception
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function Enquiry() As DataSet
            If StartConnection() = True Then
                strSQL = "SELECT 0 as selected, BranchID, BranchName FROM SysBranch WHERE Flag=1"
                If strSQL Is Nothing = False Then
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, "BranchInfo"), DataSet)
                End If
            Else
                Return Nothing
            End If
            EndSQLControl()

        End Function
#End Region

    End Class

#End Region

#Region "GroupBar / ItemBar Menu"
    Public Class GroupItemBar
        Inherits Core.CoreBase

        Public Function MenuBar() As DataTable
            Try
                StartSQLControl()
                StartConnection()

                strSQL = "SELECT SYSMENUGROUP.GroupKey, SYSMENUGROUP.SeqNo, SYSMENUGROUP.GroupDesc, SYSMENUGROUP.GroupType, " & _
                         "SYSMENUITEM.ItemType, SYSMENUITEM.ItemKey, SYSMENUITEM.ItemDesc, SYSMENUITEM.IconIndex " & _
                         "FROM SYSMENUGROUP INNER JOIN SYSMENUITEM ON SYSMENUGROUP.GroupKey = SYSMENUITEM.GroupKey " & _
                         "WHERE (SYSMENUGROUP.Status = 1) AND (SYSMENUITEM.Status = 1) AND (SYSMENUGROUP.GroupType = 10) AND (SYSMENUITEM.ItemType = 10) ORDER BY SYSMENUGROUP.SeqNo, SYSMENUITEM.SeqNoItem"
                Return objDCom.Execute(strSQL, CommandType.Text)
            Catch ex As Exception
                Return Nothing
            Finally
                EndConnection()
                EndSQLControl()
            End Try
        End Function

        Public Function GroupBar() As DataTable
            Try
                StartSQLControl()
                StartConnection()

                strSQL = "SELECT GroupDesc, GroupKey FROM SYSMENUGROUP ORDER BY SeqNo"
                Return objDCom.Execute(strSQL, CommandType.Text)
            Catch ex As Exception
                Return Nothing
            Finally
                EndConnection()
                EndSQLControl()
            End Try
        End Function
    End Class
#End Region

#Region "Round Setup"
    Public Class RoundSetupBase
        Inherits Core.CoreBase


        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "BRANCHID, ROUNDCODE, ROUNDDESC, ROUNDTYPE, ROUNDFROM, ROUNDTO, ROUNDAMT, CREATEDATE, CREATEBY, LASTUPDATE, UPDATEBY, ACTIVE, INUSE, FLAG"
                .CheckFields = "INUSE, FLAG"
                .TableName = "ROUNDSETUP"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "BRANCHID, ROUNDCODE, ROUNDDESC, ROUNDTYPE, ROUNDFROM, ROUNDTO, ROUNDAMT, CREATEDATE, CREATEBY, LASTUPDATE, UPDATEBY, ACTIVE, INUSE, FLAG"
                .ListingCond = "FLAG = 1 AND Active = 1"
                .ShortList = String.Empty
                .ShortListCond = String.Empty
            End With
            ReDim MyFields(1)
            'MyBase.AddMyField(0, "Remark Code", "RemarkCode", TypeCode.String)
            'MyBase.AddMyField(1, "Remark Desc", "RemarkDesc", TypeCode.String)
        End Sub


#Region "Data Selection"
        Public Shared Function GetRoundSetup(ByVal BranchID As String) As DataTable
            Try
                StartSQLControl()
                StartConnection()

                strSQL = "SELECT BRANCHID, ROUNDCODE, ROUNDDESC, ROUNDTYPE, ROUNDFROM, ROUNDTO, ROUNDAMT, CREATEDATE, CREATEBY, LASTUPDATE, UPDATEBY, ACTIVE, INUSE, FLAG FROM ROUNDSETUP " & _
                    "WHERE ACTIVE = 1 AND FLAG = 1 AND BRANCHID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BranchID) & "'"

                Return objDCom.Execute(strSQL, CommandType.Text, True)

            Catch ex As Exception
                Return Nothing
            Finally
                EndConnection()
                EndSQLControl()
            End Try
        End Function
#End Region

    End Class
#End Region

#Region "Card Access"
    Public Class CardAccessBase
        Inherits Core.CoreBase

#Region "CardAccess class in CardAccessBase"
        Public Class CardAccess
            Inherits Core.SingleBase
            Protected strAccessID, strRefID, strCourse, strIntake, strStudentID As String
            Protected strActiveAccessTime, strInActiveAccessTime, strUpdBy As String
            Protected dtCurDate, dtCurTime, dtLastUpd As Date
            Protected intRefType, intAccessAction, intStatus As Integer
            Protected dtDataTable As DataTable

            Public Property AccessID() As String
                Get
                    Return strAccessID
                End Get
                Set(ByVal Value As String)
                    strAccessID = Value
                End Set
            End Property

            Public Property RefID() As String
                Get
                    Return strRefID
                End Get
                Set(ByVal Value As String)
                    strRefID = Value
                End Set
            End Property

            Public Property StudentID() As String
                Get
                    Return strStudentID
                End Get
                Set(ByVal Value As String)
                    strStudentID = Value
                End Set
            End Property

            Public Property Course() As String
                Get
                    Return strCourse
                End Get
                Set(ByVal Value As String)
                    strCourse = Value
                End Set
            End Property

            Public Property Intake() As String
                Get
                    Return strIntake
                End Get
                Set(ByVal Value As String)
                    strIntake = Value
                End Set
            End Property

            Public Property InactiveAccessTime() As String
                Get
                    Return strInActiveAccessTime
                End Get
                Set(ByVal Value As String)
                    strInActiveAccessTime = Value
                End Set
            End Property

            Public Property ActiveAccessTime() As String
                Get
                    Return strActiveAccessTime
                End Get
                Set(ByVal Value As String)
                    strActiveAccessTime = Value
                End Set
            End Property

            Public Property RefType() As Integer
                Get
                    Return intRefType
                End Get
                Set(ByVal Value As Integer)
                    intRefType = Value
                End Set
            End Property

            Public Property AccessAction() As Integer
                Get
                    Return intAccessAction
                End Get
                Set(ByVal Value As Integer)
                    intAccessAction = Value
                End Set
            End Property

            Public Property Status() As Integer
                Get
                    Return intStatus
                End Get
                Set(ByVal Value As Integer)
                    intStatus = Value
                End Set
            End Property

            Public Property CurDate() As Date
                Get
                    Return dtCurDate
                End Get
                Set(ByVal Value As Date)
                    dtCurDate = Value
                End Set
            End Property

            Public Property CurTime() As Date
                Get
                    Return dtCurTime
                End Get
                Set(ByVal Value As Date)
                    dtCurTime = Value
                End Set
            End Property

            Public Property TempDataTable() As DataTable
                Get
                    Return dtDataTable
                End Get
                Set(ByVal Value As DataTable)
                    dtDataTable = Value
                End Set
            End Property

            Public Property LastUpdate() As Date
                Get
                    Return dtLastUpd
                End Get
                Set(ByVal Value As Date)
                    dtLastUpd = Value
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return strUpdBy
                End Get
                Set(ByVal Value As String)
                    strUpdBy = Value
                End Set
            End Property
        End Class
#End Region

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "AccessID, CONVERT(varchar, AccessDate, 103) AS AccessDate, CONVERT(varchar, AccessTime, 108) AS AccessTime, AccessAction, RefID, RefType, Status"
                .CheckFields = String.Empty
                .TableName = "AC_ACCESSLOG"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "AccessID, CONVERT(varchar, AccessDate, 103) AS AccessDate, CONVERT(varchar, AccessTime, 108) AS AccessTime, AccessAction, RefID, RefType, Status"
                .ListingCond = "Status = 1"
                .ShortList = String.Empty
                .ShortListCond = String.Empty
            End With
            ReDim MyFields(1)
        End Sub

#Region "Save Function"
        Public Function LogAccess(ByVal objCardAccess As CardAccess) As Boolean
            Dim strSQL As String
            LogAccess = False
            Try
                If objCardAccess Is Nothing Then
                    'msg return
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With objSQL
                            .TableName = "AC_ACCESSLOG"
                            .AddField("AccessID", objCardAccess.AccessID, SQLControl.EnumDataType.dtString)
                            .AddField("AccessDate", objCardAccess.CurDate.ToLongDateString, SQLControl.EnumDataType.dtDateOnly)
                            .AddField("AccessTime", objCardAccess.CurTime.ToLongTimeString, SQLControl.EnumDataType.dtString)
                            .AddField("AccessAction", objCardAccess.AccessAction, SQLControl.EnumDataType.dtNumeric)
                            .AddField("RefID", objCardAccess.RefID, SQLControl.EnumDataType.dtString)
                            .AddField("RefType", objCardAccess.RefType, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Status", objCardAccess.Status, SQLControl.EnumDataType.dtNumeric)
                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                        End With
                        'execute
                        objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                        Return True
                    End If
                End If
            Catch axLogAccess As ApplicationException
                Throw axLogAccess
            Catch exLogAccess As Exception
                Throw exLogAccess
            Finally
                objCardAccess = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function EditAccess(ByVal objCardAccess As CardAccess) As Boolean
            Dim strSQL As String
            Dim arySQL As ArrayList
            Dim strAccessDate As String
            EditAccess = False
            Try
                If objCardAccess Is Nothing Then
                    'msg return
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        arySQL = New ArrayList
                        strAccessDate = objSQL.ParseValue(SQLControl.EnumDataType.dtDateOnly, objCardAccess.CurDate)
                        If objCardAccess.ActiveAccessTime <> String.Empty Then
                            objSQL.TableName = "AC_ACCESSLOG"
                            objSQL.AddField("Status", 1, SQLControl.EnumDataType.dtNumeric)
                            strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "AC_ACCESSLOG", _
                                     "WHERE AccessDate = " & strAccessDate & " AND AccessTime IN(" & objCardAccess.ActiveAccessTime & ")")
                            If strSQL <> String.Empty Then
                                arySQL.Add(strSQL)
                            End If
                        End If
                        If objCardAccess.InactiveAccessTime <> String.Empty Then
                            objSQL.TableName = "AC_ACCESSLOG"
                            objSQL.AddField("Status", 0, SQLControl.EnumDataType.dtNumeric)
                            strSQL = objSQL.BuildSQL(SQLControl.EnumSQLType.stUpdate, "AC_ACCESSLOG", _
                                     "WHERE AccessDate = " & strAccessDate & " AND AccessTime IN(" & objCardAccess.InactiveAccessTime & ")")
                            If strSQL <> String.Empty Then
                                arySQL.Add(strSQL)
                            End If
                        End If
                        'execute
                        If arySQL.Count > 0 Then
                            objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Else
                            Throw New ApplicationException("")
                        End If
                    End If
                End If
            Catch axEditAccess As ApplicationException
                Throw axEditAccess
                Return False
            Catch exEditAccess As Exception
                Throw exEditAccess
                Return False
            Finally
                objCardAccess = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function ProcessStudentAttendance(ByVal objCardAccess As CardAccess) As Boolean
            Dim strSQL As String
            Dim strTempStudentID As String
            Dim arySQL As ArrayList
            Dim dtTempTime As Date
            Dim dtTempInTime As Date
            Dim dtTempOutTime As Date
            Dim dblDuration As Double
            Dim rdr As SqlClient.SqlDataReader
            ProcessStudentAttendance = False
            Try
                If objCardAccess Is Nothing Then
                    'Message return
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = "SELECT AC_STUDENTCARD.StudentID, AC_ACCESSLOG.AccessTime, AC_COURSESCHEDULEDTL.StartTime, AC_COURSESCHEDULEDTL.EndTime, " & _
                                     "AC_COURSESCHEDULEDTL.TotalDailyHours FROM AC_ACCESSLOG INNER JOIN AC_STUDENTCARD ON AC_ACCESSLOG.AccessID = AC_STUDENTCARD.AccessID INNER JOIN " & _
                                     "AC_STUDENTCOURSE ON AC_STUDENTCARD.StudentID = AC_STUDENTCOURSE.StudentID INNER JOIN AC_COURSESCHEDULEDTL ON AC_STUDENTCOURSE.IntakeCode = AC_COURSESCHEDULEDTL.IntakeCode AND " & _
                                     "AC_STUDENTCOURSE.CourseID = AC_COURSESCHEDULEDTL.CourseID WHERE AC_ACCESSLOG.Status = 1 AND AC_ACCESSLOG.AccessDate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateOnly, objCardAccess.CurDate) & " " & _
                                     "AND AC_STUDENTCOURSE.CourseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCardAccess.Course) & "' AND AC_STUDENTCOURSE.IntakeCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCardAccess.Intake) & "' " & _
                                     "AND AC_COURSESCHEDULEDTL.WeekDays = " & Weekday(objCardAccess.CurDate) & " AND AC_COURSESCHEDULEDTL.Status = 1"

                            If objCardAccess.StudentID.Trim <> String.Empty Then
                                strSQL += " AND AC_STUDENTCARD.StudentID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCardAccess.StudentID) & "'"
                            End If
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        If rdr Is Nothing = False Then
                            arySQL = New ArrayList
                            strSQL = "DELETE FROM AC_STUDENTATTENDANCE WHERE AttendanceDate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateOnly, objCardAccess.CurDate) & " AND " & _
                                     "CourseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCardAccess.Course) & "' AND IntakeCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCardAccess.Intake) & "'"
                            If objCardAccess.StudentID.Trim <> String.Empty Then
                                strSQL += " AND StudentID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCardAccess.StudentID) & "'"
                            End If
                            arySQL.Add(strSQL)
                            With rdr
                                While .Read
                                    If strTempStudentID = String.Empty Then
                                        strTempStudentID = CType(.Item("StudentID"), String)
                                        dtTempTime = ValidateTime(.Item("StartTime"))
                                        dtTempInTime = ValidateTime(.Item("AccessTime"))
                                        If dtTempTime <> Nothing And dtTempInTime <> Nothing Then
                                            If DateDiff(DateInterval.Minute, dtTempInTime, dtTempTime) > 0 Then
                                                dtTempInTime = dtTempTime
                                            End If
                                            dtTempTime = Nothing
                                        End If
                                    Else
                                        If strTempStudentID.Equals(CType(.Item("StudentID"), String)) Then
                                            dtTempTime = ValidateTime(.Item("EndTime"))
                                            dtTempOutTime = ValidateTime(.Item("AccessTime"))
                                            If dtTempTime <> Nothing And dtTempOutTime <> Nothing Then
                                                If DateDiff(DateInterval.Minute, dtTempOutTime, dtTempTime) < 0 Then
                                                    dtTempOutTime = dtTempTime
                                                End If
                                                dtTempTime = Nothing
                                            End If
                                            If dtTempInTime <> Nothing And dtTempOutTime <> Nothing Then
                                                dblDuration = DateDiff(DateInterval.Minute, dtTempInTime, dtTempOutTime)
                                                If dblDuration > 0 Then
                                                    With objSQL
                                                        .TableName = "AC_STUDENTATTENDANCE"
                                                        .AddField("IntakeCode", objCardAccess.Intake, SQLControl.EnumDataType.dtString)
                                                        .AddField("CourseID", objCardAccess.Course, SQLControl.EnumDataType.dtString)
                                                        .AddField("AttendanceDate", objCardAccess.CurDate, SQLControl.EnumDataType.dtDateOnly)
                                                        .AddField("StudentID", strTempStudentID, SQLControl.EnumDataType.dtString)
                                                        .AddField("StartTime", dtTempInTime, SQLControl.EnumDataType.dtDateTime)
                                                        .AddField("EndTime", dtTempOutTime, SQLControl.EnumDataType.dtDateTime)
                                                        .AddField("Duration", dblDuration, SQLControl.EnumDataType.dtNumeric)
                                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                                    End With
                                                    If strSQL <> String.Empty Then
                                                        arySQL.Add(strSQL)
                                                        strSQL = String.Empty
                                                    End If
                                                End If
                                            End If
                                        End If
                                        dtTempInTime = Nothing
                                        dtTempOutTime = Nothing
                                        dblDuration = 0
                                        strTempStudentID = String.Empty
                                    End If
                                End While
                                .Close()
                            End With
                            If arySQL.Count > 0 Then
                                If objDCom.BatchExecute(arySQL, CommandType.Text, True) = True Then
                                    Return True
                                End If
                            Else
                                Return True
                            End If
                        End If
                    End If
                End If
            Catch exAssign As Exception
                Throw exAssign
                Return False
            Finally
                objCardAccess = Nothing
                If rdr Is Nothing = False Then
                    If rdr.IsClosed = False Then
                        rdr.Close()
                        rdr = Nothing
                    End If
                End If
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function FinalizeStudentAttendance(ByVal objCardAccess As CardAccess) As Boolean
            Dim strSQL As String
            Dim arySQL As ArrayList
            Dim drRow As DataRow
            Dim dtDataTable As DataTable
            Dim strTempStudentID As String
            FinalizeStudentAttendance = False
            Try
                If objCardAccess Is Nothing Then
                    'Message return
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        arySQL = New ArrayList
                        strSQL = "SELECT StartTime, EndTime, TotalDailyHours FROM AC_COURSESCHEDULEDTL WHERE Status = 1 AND " & _
                                 "IntakeCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCardAccess.Intake) & "' " & _
                                 "AND CourseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCardAccess.Course) & "' " & _
                                 "AND WeekDays = " & Weekday(objCardAccess.CurDate)
                        dtDataTable = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, "AC_COURSESCHEDULEDTL"), DataTable)

                        strSQL = "DELETE FROM AC_DAILYATTENDANCE WHERE AttendanceDate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateOnly, objCardAccess.CurDate) & " AND " & _
                                 "CourseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCardAccess.Course) & "' AND IntakeCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCardAccess.Intake) & "' " & _
                                 "And Status in('1','3')"
                        arySQL.Add(strSQL)

                        With objSQL
                            For Each drRow In objCardAccess.TempDataTable.Rows
                                .TableName = "AC_DAILYATTENDANCE"
                                .AddField("IntakeCode", objCardAccess.Intake, SQLControl.EnumDataType.dtString)
                                .AddField("CourseID", objCardAccess.Course, SQLControl.EnumDataType.dtString)
                                .AddField("AttendanceDate", objCardAccess.CurDate, SQLControl.EnumDataType.dtDateOnly)
                                .AddField("StudentID", drRow.Item("StudentID"), SQLControl.EnumDataType.dtString)
                                .AddField("ArrivalTime", drRow.Item("ArrivalTime"), SQLControl.EnumDataType.dtDateTime)
                                .AddField("LeaveTime", drRow.Item("LeaveTime"), SQLControl.EnumDataType.dtDateTime)
                                .AddField("TotalTime", drRow.Item("TotalTime"), SQLControl.EnumDataType.dtNumeric)
                                .AddField("Status", CreateStatus(dtDataTable, CType(drRow.Item("TotalTime"), Double)), SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", objCardAccess.LastUpdate, SQLControl.EnumDataType.dtDateOnly)
                                .AddField("CreateBy", objCardAccess.UpdateBy, SQLControl.EnumDataType.dtString)
                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                arySQL.Add(strSQL)
                            Next
                        End With
                        If arySQL.Count > 0 Then
                            If objDCom.BatchExecute(arySQL, CommandType.Text, True) = True Then
                                Return True
                            End If
                        Else
                            Return True
                        End If
                    End If
                End If
            Catch exAssign As Exception
                Throw exAssign
                Return False
            Finally
                objCardAccess = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Private Function CreateStatus(ByVal Table As DataTable, ByVal TotalTime As Double) As Integer
            Dim intStatus As Integer = 0
            Dim DailyTotalTime As Double = 0
            Try
                StartSQLControl()
                If Table Is Nothing = False Then
                    If Table.Rows.Count > 0 Then
                        If IsNumeric(Table.Rows(0).Item("TotalDailyHours")) Then
                            DailyTotalTime = (CType(Table.Rows(0).Item("TotalDailyHours"), Double) * 60)
                            If TotalTime >= DailyTotalTime Then
                                intStatus = 1
                            Else
                                intStatus = 3
                            End If
                        End If
                    End If
                End If
                Return intStatus
            Catch ex As Exception
                Return 0
            End Try
        End Function

        Private Function ValidateTime(ByVal objTime As Object) As DateTime
            Dim dtTempTime As DateTime
            Try
                If IsDate(objTime) Then
                    dtTempTime = CType(objTime, Date)
                Else
                    dtTempTime = Nothing
                End If
                Return dtTempTime
            Catch exValidateTime As Exception
                Return Nothing
            End Try
        End Function

#End Region

#Region "Data Selection"
        'Enquiry
        Function Enquiry(ByVal CurrDate As Date, ByVal sType As Integer) As DataSet
            Dim strDate As String
            If StartConnection() = True Then
                StartSQLControl()
                strDate = objSQL.ParseValue(SQLControl.EnumDataType.dtDateOnly, CurrDate)
                With MyInfo
                    Select Case sType
                        Case 0 'Course
                            strSQL = "SELECT 1 AS Selected, AC_INTAKE.IntakeCode, AC_INTAKE.Remark, AC_INTAKE.CourseID, AC_COURSE.CourseName " & _
                                     "FROM AC_INTAKE INNER JOIN AC_COURSESCHEDULEDTL ON AC_INTAKE.IntakeCode = AC_COURSESCHEDULEDTL.IntakeCode AND " & _
                                     "AC_INTAKE.CourseID = AC_COURSESCHEDULEDTL.CourseID INNER JOIN AC_COURSE ON AC_INTAKE.CourseID = AC_COURSE.CourseID " & _
                                     "WHERE AC_INTAKE.StartDate <= " & strDate & " And AC_INTAKE.EndDate >= " & strDate & " And AC_COURSESCHEDULEDTL.Status = 1 " & _
                                     "And AC_COURSESCHEDULEDTL.WeekDays = " & Weekday(CurrDate) & " AND AC_INTAKE.Status = 1 " & _
                                     "AND AC_INTAKE.Flag = 1 AND AC_COURSE.Flag = 1 AND AC_COURSE.Active = 1"
                        Case 1 'Access
                            strSQL = BuildSelect(.FieldsList, .TableName, "AccessDate = " & strDate & " AND Status = 1")
                    End Select

                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Function GetPreviewAttendance(ByVal objCardAccess As CardAccess) As DataSet
            Dim strDate As String
            If StartConnection() = True Then
                StartSQLControl()
                strDate = objSQL.ParseValue(SQLControl.EnumDataType.dtDateOnly, objCardAccess.CurDate)
                With MyInfo
                    strSQL = "SELECT StudentID, (SELECT TOP 1 CONVERT(varchar, StartTime, 108) AS ArrivalTime FROM AC_STUDENTATTENDANCE " & _
                             "WHERE IntakeCode = AC_STUDENTATTENDANCE.IntakeCode AND AttendanceDate = " & strDate & " AND " & _
                             "StudentID = AC_STUDENTATTENDANCE.StudentID And CourseID = AC_STUDENTATTENDANCE.CourseID ORDER BY StartTime ASC) AS ArrivalTime, " & _
                             "(SELECT TOP 1 CONVERT(varchar, EndTime, 108) AS LeaveTime FROM AC_STUDENTATTENDANCE " & _
                             "WHERE IntakeCode = AC_STUDENTATTENDANCE.IntakeCode AND AttendanceDate = " & strDate & " AND " & _
                             "StudentID = AC_STUDENTATTENDANCE.StudentID AND CourseID = AC_STUDENTATTENDANCE.CourseID ORDER BY EndTime DESC) AS LeaveTime, " & _
                             "SUM(Duration) AS TotalTime FROM AC_STUDENTATTENDANCE"

                    If objCardAccess.Intake <> String.Empty And objCardAccess.Course <> String.Empty Then
                        strSQL += " WHERE IntakeCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCardAccess.Intake) & "' " & _
                                  "AND AttendanceDate = " & strDate & " AND CourseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCardAccess.Course) & "' " & _
                                  "GROUP BY StudentID"
                    End If

                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Overloads Function GetStudentAttendance(ByVal StudentID As String) As DataSet
            Dim strDate As String
            If StartConnection() = True Then
                StartSQLControl()
                With MyInfo
                    strSQL = "SELECT IntakeCode, CourseID, AttendanceDate, CONVERT(varchar, ArrivalTime, 108) AS ArrivalTime, CONVERT(varchar, LeaveTime, 108) AS LeaveTime, TotalTime, Status, CreateDate, CreateBy " & _
                             "FROM AC_DAILYATTENDANCE WHERE StudentID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, StudentID) & "'"
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Overloads Function GetStudentAttendance(ByVal objCardAccess As CardAccess) As DataSet
            Dim strDate As String
            If StartConnection() = True Then
                StartSQLControl()
                strDate = objSQL.ParseValue(SQLControl.EnumDataType.dtDateOnly, objCardAccess.CurDate)
                With MyInfo
                    strSQL = "SELECT CONVERT(varchar, StartTime, 108) AS StartTime, CONVERT(varchar, EndTime, 108) AS EndTime, Duration " & _
                             "FROM AC_STUDENTATTENDANCE WHERE IntakeCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCardAccess.Intake) & "' " & _
                             "AND CourseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCardAccess.Course) & "' " & _
                             "AND AttendanceDate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateOnly, objCardAccess.CurDate) & " " & _
                             "AND StudentID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCardAccess.StudentID) & "'"

                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Function GetStudentAccess(ByVal objCardAccess As CardAccess) As DataSet
            Dim strDate As String
            If StartConnection() = True Then
                StartSQLControl()
                strDate = objSQL.ParseValue(SQLControl.EnumDataType.dtDateOnly, objCardAccess.CurDate)
                With MyInfo
                    strSQL = "SELECT AC_ACCESSLOG.Status, CONVERT(varchar, AC_ACCESSLOG.AccessTime, 108) AS AccessTime, AC_ACCESSLOG.AccessAction, AC_ACCESSLOG.RefID, " & _
                             "AC_ACCESSLOG.RefType FROM AC_ACCESSLOG INNER JOIN AC_STUDENTCARD ON AC_ACCESSLOG.AccessID = AC_STUDENTCARD.AccessID " & _
                             "WHERE AC_ACCESSLOG.AccessDate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateOnly, objCardAccess.CurDate) & " " & _
                             "AND AC_STUDENTCARD.StudentID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCardAccess.StudentID) & "'"

                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

#End Region

    End Class
#End Region

#Region "TaskListBase Base"
    Public Class TaskListBase
        Inherits Core.CoreBase

#Region "TaskList class in TaskListBase"
        Public Class TaskList
            Protected strTaskCode, strTaskDesc As String
            Protected strBranchID, strTaskValue1, strTaskValue2 As String
            Protected dtTaskStartDate, dtTaskEndDate As DateTime
            Protected intTaskStatus As Integer
            Protected dtLastUpdate As DateTime
            Protected strUpdateBy As String

            Public Property TaskCode() As String
                Get
                    Return strTaskCode
                End Get
                Set(ByVal Value As String)
                    If Value.Trim = String.Empty Then
                        Throw New Exception("Please key in the Code.")
                    Else
                        strTaskCode = Value
                    End If
                End Set
            End Property

            Public Property TaskDesc() As String
                Get
                    Return strTaskDesc
                End Get
                Set(ByVal Value As String)
                    strTaskDesc = Value
                End Set
            End Property

            Public Property BranchID() As String
                Get
                    Return strBranchID
                End Get
                Set(ByVal Value As String)
                    strBranchID = Value
                End Set
            End Property

            Public Property TaskValue1() As String
                Get
                    Return strTaskValue1
                End Get
                Set(ByVal Value As String)
                    strTaskValue1 = Value
                End Set
            End Property

            Public Property TaskValue2() As String
                Get
                    Return strTaskValue2
                End Get
                Set(ByVal Value As String)
                    strTaskValue2 = Value
                End Set
            End Property

            Public Property TaskStartDate() As DateTime
                Get
                    Return dtTaskStartDate
                End Get
                Set(ByVal Value As DateTime)
                    dtTaskStartDate = Value
                End Set
            End Property

            Public Property TaskEndDate() As DateTime
                Get
                    Return dtTaskEndDate
                End Get
                Set(ByVal Value As DateTime)
                    dtTaskEndDate = Value
                End Set
            End Property

            Public Property TaskStatus() As Integer
                Get
                    Return intTaskStatus
                End Get
                Set(ByVal Value As Integer)
                    intTaskStatus = Value
                End Set
            End Property

            Public Property LastUpdate() As DateTime
                Get
                    Return dtLastUpdate
                End Get
                Set(ByVal Value As DateTime)
                    dtLastUpdate = Value
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return strUpdateBy
                End Get
                Set(ByVal Value As String)
                    strUpdateBy = Value
                End Set
            End Property
        End Class
#End Region

#Region "Task List Scheme"
        Public Class MyScheme
            Inherits Core.SchemeBase

            Protected Overrides Sub InitializeInfo()
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "TaskCode"
                    .Length = 10
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(0, this)
                With this
                    .DataType = SQLControl.EnumDataType.dtString
                    .FieldName = "TaskDesc"
                    .Length = 100
                    .DecPlace = Nothing
                    .RegExp = String.Empty
                    .IsMandatory = True
                    .AllowNegative = False
                End With
                MyBase.AddItem(1, this)
            End Sub

            Public ReadOnly Property Code() As StrucElement
                Get
                    Return MyBase.GetItem(0)
                End Get
            End Property

            Public ReadOnly Property Description() As StrucElement
                Get
                    Return MyBase.GetItem(1)
                End Get
            End Property

            Public Function GetElement(ByVal Key As Integer) As StrucElement
                Return MyBase.GetItem(Key)
            End Function

        End Class

#End Region

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "BranchID, TaskCode, TaskDesc, TaskStartDate, TaskEndDate, TaskValue1, TaskValue2, Status "
                .CheckFields = ""
                .TableName = "TASKLISTB"
                .DefaultCond = String.Empty
                .DefaultOrder = "BranchID, TaskCode "
                .Listing = "TaskCode, TaskDesc, TaskStartDate, TaskEndDate"
                .ListingCond = String.Empty
                .ShortList = "TaskCode, TaskDesc"
                .ShortListCond = String.Empty
            End With
            ReDim MyFields(1)
            MyBase.AddMyField(0, "TaskCode", "TaskCode", TypeCode.String)
            MyBase.AddMyField(1, "TaskDesc", "TaskDesc", TypeCode.String)
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function AssignItem(ByVal objTaskList As TaskList, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim blnExec As Boolean, blnFound As Boolean
            Dim rdr As SqlClient.SqlDataReader
            AssignItem = False
            Try
                If objTaskList Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False

                    If StartConnection(Core.CoreBase.EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "Code = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTaskList.TaskCode) & "' ")
                        End With
                        'strSQL = "SELECT Code, CodeDesc,CodeSeq FROM Codemaster WHERE Code ='" & _
                        '    objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCode.Code) & "' AND CodeType='" & _
                        '    objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCode.CodeType) & "'"
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
                        blnExec = True
                        If rdr Is Nothing = False Then
                            If rdr.Read Then
                                blnFound = True
                            End If
                            rdr.Close()
                        End If
                    End If

                    If blnExec Then
                        If blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                            'Item found & active
                            'system.Windows.Forms.MessageBox.Show("ab", "ABC",Windows.Forms.MessageBoxButtons.YesNo, Windows.Forms.MessageBoxIcon.Information,Windows.Forms.MessageBoxDefaultButton.Button2, Windows.Forms.MessageBoxOptions.DefaultDesktopOnly
                            'MsgBox("Record already exist", MsgBoxStyle.Critical, "Record Exist")
                            Return False
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = "CODEMASTER"
                                .AddField("CodeDesc", objTaskList.TaskDesc, SQLControl.EnumDataType.dtStringN)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True Then

                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                            "WHERE Code ='" & _
                                             objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTaskList.TaskCode) & "' ")
                                        Else
                                            If blnFound = False Then
                                                '.AddField("CodeSeq", objTaskList.CodeSequence.ToString, SQLControl.EnumDataType.dtNumeric)
                                                '.AddField("Code", objTaskList.Code, SQLControl.EnumDataType.dtString)
                                                '.AddField("CodeType", objTaskList.CodeType, SQLControl.EnumDataType.dtString)
                                                'strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        'strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                        '       "WHERE Code ='" & _
                                        '     objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCode.Code) & "' AND CodeType='" & _
                                        '     objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCode.CodeType) & "'")
                                End Select
                            End With
                            Try
                                'execute 
                                objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002")
                                Else
                                    Throw New ApplicationException("210004")
                                End If
                            Finally
                                objSQL.Dispose()
                            End Try
                            Return True
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objTaskList = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Function Add(ByVal objTaskList As TaskList) As Boolean
            Return AssignItem(objTaskList, SQLControl.EnumSQLType.stInsert)
        End Function

        Function Amend(ByVal objTaskList As TaskList) As Boolean
            Return AssignItem(objTaskList, SQLControl.EnumSQLType.stUpdate)
        End Function

        Function Delete(ByVal objTaskList As TaskList) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False

            Try
                If objTaskList Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "Code ='" & _
                                    objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTaskList.TaskCode) & "' ")

                        End With
                        'strSQL = "SELECT Code, CodeDesc,CodeSeq FROM Codemaster WHERE Code ='" & _
                        '  objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCode.Code) & "' AND CodeType='" & _
                        '  objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCode.CodeType) & "'"
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        If rdr Is Nothing = False Then
                            If rdr.Read Then
                                blnFound = True
                            End If
                            rdr.Close()
                        End If

                        If blnFound = False Then
                            'Error Message
                            Return False
                        End If

                        If blnFound = True Then
                            strSQL = BuildDelete(MyInfo.TableName, "Code = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objTaskList.TaskCode) & "' ")
                            'strSQL = "DELETE FROM CODEMASTER WHERE Code = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCode.Code) & "' AND CODETYPE ='" & _
                            'objSQL.ParseValue(SQLControl.EnumDataType.dtString, objCode.CodeType) & "'"
                        End If
                        Try
                            'execute
                            objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try
                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As SystemException
                Throw exDelete
            Finally
                objTaskList = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function AddTask(ByVal objTaskList As TaskList) As Boolean
            Dim blnFound As Boolean = False
            Dim rdr As SqlClient.SqlDataReader
            Dim arySQL As ArrayList
            Dim dtBranch As DataTable
            Dim drRow As DataRow
            Dim strBranchID As String
            Try
                StartConnection()
                StartSQLControl()

                arySQL = New ArrayList

                If Convert.ToString(objTaskList.BranchID) <> "ALL" Then
                    strSQL = "SELECT BranchID, TaskCode FROM TASKLISTB " & _
                            "WHERE BranchID = '" & Convert.ToString(objTaskList.BranchID) & "' " & _
                            "AND TaskCode = '" & Convert.ToString(objTaskList.TaskCode) & "' "
                    rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                    If rdr Is Nothing = False Then
                        If rdr.HasRows = True Then
                            blnFound = True
                        End If
                        rdr.Close()
                    End If

                    With objSQL
                        .TableName = "TASKLISTB"
                        .AddField("BranchID", Convert.ToString(objTaskList.BranchID), SQLControl.EnumDataType.dtString)
                        .AddField("TaskCode", Convert.ToString(objTaskList.TaskCode), SQLControl.EnumDataType.dtString)
                        .AddField("TaskStartDate", objTaskList.TaskStartDate, SQLControl.EnumDataType.dtDateOnly)
                        .AddField("TaskEndDate", objTaskList.TaskEndDate, SQLControl.EnumDataType.dtDateOnly)
                        .AddField("LastUpdate", objTaskList.LastUpdate, SQLControl.EnumDataType.dtDateOnly)
                        .AddField("UpdateBy", objTaskList.UpdateBy, SQLControl.EnumDataType.dtString)
                        If blnFound = True Then
                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                        " WHERE BranchID = '" & Convert.ToString(objTaskList.BranchID) & "' " & _
                                        " AND TaskCode = '" & Convert.ToString(objTaskList.TaskCode) & "' ")
                        ElseIf blnFound = False Then
                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                        End If
                        arySQL.Add(strSQL)
                    End With

                ElseIf Convert.ToString(objTaskList.BranchID) = "ALL" Then
                    strSQL = "SELECT BranchID FROM SYSBRANCH"
                    dtBranch = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text), DataTable)

                    If dtBranch Is Nothing = False Then
                        For Each drRow In dtBranch.Rows
                            strBranchID = Convert.ToString(drRow.Item(0))

                            strSQL = "SELECT BranchID, TaskCode FROM TASKLISTB " & _
                            "WHERE BranchID = '" & Convert.ToString(strBranchID) & "' " & _
                            "AND TaskCode = '" & Convert.ToString(objTaskList.TaskCode) & "' "
                            rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                            If rdr Is Nothing = False Then
                                If rdr.HasRows = True Then
                                    blnFound = True
                                Else
                                    blnFound = False
                                End If
                                rdr.Close()
                            End If

                            With objSQL
                                .TableName = "TASKLISTB"
                                .AddField("BranchID", Convert.ToString(strBranchID), SQLControl.EnumDataType.dtString)
                                .AddField("TaskCode", Convert.ToString(objTaskList.TaskCode), SQLControl.EnumDataType.dtString)
                                .AddField("TaskStartDate", objTaskList.TaskStartDate, SQLControl.EnumDataType.dtDateOnly)
                                .AddField("TaskEndDate", objTaskList.TaskEndDate, SQLControl.EnumDataType.dtDateOnly)
                                .AddField("LastUpdate", objTaskList.LastUpdate, SQLControl.EnumDataType.dtDateOnly)
                                .AddField("UpdateBy", objTaskList.UpdateBy, SQLControl.EnumDataType.dtString)
                                If blnFound = True Then
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                                " WHERE BranchID = '" & Convert.ToString(strBranchID) & "' " & _
                                                " AND TaskCode = '" & Convert.ToString(objTaskList.TaskCode) & "' ")
                                ElseIf blnFound = False Then
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                End If
                                arySQL.Add(strSQL)
                            End With
                        Next
                    End If
                End If

                If arySQL.Count > 0 Then
                    If objDCom.BatchExecute(arySQL, CommandType.Text) = True Then
                        arySQL.Clear()
                        Return True
                    End If
                Else
                    Return True
                End If

            Catch ex As Exception
                Throw New Exception(ex.Message)
                Return False
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function RemoveTask(ByVal objTaskList As TaskList) As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Dim blnFound As Boolean = False

            Try
                StartConnection()
                StartSQLControl()

                strSQL = "SELECT BranchID, TaskCode FROM TASKLISTB " & _
                        "WHERE BranchID = '" & Convert.ToString(objTaskList.BranchID) & "' " & _
                        "AND TaskCode = '" & Convert.ToString(objTaskList.TaskCode) & "' "

                rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                If rdr Is Nothing = False Then
                    If rdr.HasRows = True Then
                        blnFound = True
                    End If
                    rdr.Close()
                End If

                If blnFound = True Then
                    strSQL = "DELETE FROM TASKLISTB WHERE BranchID = '" & Convert.ToString(objTaskList.BranchID) & "' " & _
                            "AND TaskCode = '" & Convert.ToString(objTaskList.TaskCode) & "' "
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                    Return True
                ElseIf blnFound = False Then
                    Return False
                End If

            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

#End Region

#Region "Data Selection"
        'Enquiry
        Function Enquiry(Optional ByVal SQLstmt As String = Nothing) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    If SQLstmt = Nothing Or SQLstmt = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName, , "CodeSeq")
                    Else
                        strSQL = SQLstmt
                    End If
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetListing() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetShortList() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetShortList(ByVal TypeCode As String) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, String.Concat(.ShortListCond, " AND TypeCode='", TypeCode, "'"))
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Shared Function EnquiryType(ByVal CodeType As String) As DataSet
            Dim strSQL As String
            If StartConnection() = True Then
                strSQL = "SELECT Code, CodeDesc  FROM CODEMASTER WHERE CodeType='" & CodeType & "'"
                Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, "CODEMASTER"), DataSet)
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetTaskList() As DataTable
            Dim tmpDT As DataTable
            Try
                StartConnection()
                StartSQLControl()

                strSQL = "SELECT TaskCode, TaskDesc, TaskValue FROM TASKLIST "

                tmpDT = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text), DataTable)

                If tmpDT Is Nothing = False Then
                    Return tmpDT
                Else
                    Return Nothing
                End If

            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function GetTaskListB(ByVal BranchID As String) As DataTable
            Dim tmpDT As DataTable
            Try
                StartConnection()
                StartSQLControl()

                If BranchID <> String.Empty Then
                    strSQL = "SELECT BranchID, TaskCode, TaskStartDate, TaskEndDate, " & _
                            "TaskValue1, TaskValue2, Status FROM TASKLISTB " & _
                            "WHERE BranchID = '" & BranchID & "' "
                ElseIf BranchID = String.Empty Then
                    strSQL = "SELECT BranchID, TaskCode, TaskStartDate, TaskEndDate, " & _
                            "TaskValue1, TaskValue2, Status FROM TASKLISTB "
                End If

                tmpDT = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text), DataTable)

                If tmpDT Is Nothing = False Then
                    Return tmpDT
                Else
                    Return Nothing
                End If

            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Shared Function FillTerminalTaskList(ByVal objTerminal As Core.TerminalBase, ByVal BranchID As String, ByVal TermID As Integer, ByVal BizDate As Date) As Boolean
            Dim strFields, strSQL, strCond As String
            Dim rdr As SqlClient.SqlDataReader
            Try
                FillTerminalTaskList = False
                StartConnection()
                StartSQLControl()

                strSQL = "SELECT TaskCode, TaskStartDate, TaskEndDate FROM TASKLISTB " & _
                        "WHERE BranchID = '" & Replace(BranchID, "'", "''") & "' "

                rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                If rdr Is Nothing = False Then
                    With rdr
                        While .Read
                            With objTerminal
                                Select Case Convert.ToString(rdr("TaskCode")).ToUpper
                                    Case "TSTKTAKE"
                                        If Convert.ToDateTime(rdr("TaskStartDate")) = Convert.ToDateTime(BizDate) Then
                                            .GenerateStkTakeImg = True
                                        Else
                                            .GenerateStkTakeImg = False
                                        End If

                                    Case "TSTKINCUT"
                                        If (Convert.ToDateTime(rdr("TaskStartDate")) <= Convert.ToDateTime(BizDate)) And _
                                            (Convert.ToDateTime(rdr("TaskEndDate")) >= Convert.ToDateTime(BizDate)) Then
                                            .AllowStkRecieve = False
                                        ElseIf (Convert.ToDateTime(rdr("TaskStartDate")) > Convert.ToDateTime(BizDate)) And _
                                            (Convert.ToDateTime(rdr("TaskEndDate")) < Convert.ToDateTime(BizDate)) Then
                                            .AllowStkRecieve = True
                                        End If

                                    Case "TSTKOUTCUT"
                                        If (Convert.ToDateTime(rdr("TaskStartDate")) <= Convert.ToDateTime(BizDate)) And _
                                            (Convert.ToDateTime(rdr("TaskEndDate")) >= Convert.ToDateTime(BizDate)) Then
                                            .AllowStkTransfer = False
                                        ElseIf (Convert.ToDateTime(rdr("TaskStartDate")) > Convert.ToDateTime(BizDate)) And _
                                           (Convert.ToDateTime(rdr("TaskEndDate")) < Convert.ToDateTime(BizDate)) Then
                                            .AllowStkTransfer = True
                                        End If

                                End Select
                            End With
                            FillTerminalTaskList = True
                        End While
                        .Close()
                    End With
                End If

            Catch errFill As Exception
                Return False
            Finally

            End Try
        End Function

        Public Function ValidateStkInOutCutDate(ByVal InOutType As Integer, ByVal BranchID As String, ByVal BizDate As Date) As Boolean
            Dim strTaskCode As String
            Dim rdr As SqlClient.SqlDataReader
            Try
                StartConnection()
                StartSQLControl()

                If InOutType = 0 Then
                    strTaskCode = "TSTKINCUT"
                ElseIf InOutType = 1 Then
                    strTaskCode = "TSTKOUTCUT"
                End If

                strSQL = "SELECT BranchID, TaskCode, TaskStartDate, TaskEndDate FROM TASKLISTB " & _
                        "WHERE BranchID = '" & BranchID & "' " & _
                        "AND TaskCode = '" & strTaskCode & "' " & _
                        "AND TaskStartDate <= " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateOnly, BizDate) & " " & _
                        "AND TaskEndDate >= " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateOnly, BizDate) & " "

                rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                If rdr Is Nothing = False Then
                    With rdr
                        If .HasRows = True Then
                            Return False
                        End If
                    End With
                    rdr.Close()
                End If

                Return True

            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

    End Class
#End Region

#Region "Customer History"
    Public Class CustHistBase
        Inherits Core.CoreBase

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = ""
                .CheckFields = ""
                .TableName = "CUSTHISTORY"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = ""
                .ListingCond = ""
                .ShortList = ""
                .ShortListCond = String.Empty
            End With
        End Sub

        Public Shared Function getCustHist(ByVal Condition As String) As DataSet
            Try
                StartSQLControl()
                StartConnection()
                strSQL = "SELECT CUSTOMERID,TransNo,stkCode,stkType,BehvType,Qty,OrgPrice,NettPrice,TolAMT" & _
                " FROM CUSTHISTORY" & _
                Condition
                '" WHERE CUSTOMERID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, CUSTID) & "'" & Condition
                Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, "CUSTHISTORY"), DataSet)
            Catch ex As Exception
                Return Nothing
            Finally
                EndConnection()
                EndSQLControl()
            End Try
        End Function

    End Class
#End Region

#Region "Survey Base"
    Public Class SurveyBase
        Inherits Core.CoreBase

#Region "Survey Class In Survey Base"
        Public Class Survey
            'Inherits Core.SingleBase
            Protected strSurveyID, strTransID, strBranchID, strFurniture As String
            Protected intAge, intGender, intRace, intResident As Integer

            Public Property AgeGroup() As Integer
                Get
                    Return intAge
                End Get
                Set(ByVal Value As Integer)
                    intAge = Value
                End Set
            End Property

            Public Property Race() As Integer
                Get
                    Return intRace
                End Get
                Set(ByVal Value As Integer)
                    intRace = Value
                End Set
            End Property

            Public Property TransID() As String
                Get
                    Return strTransID
                End Get
                Set(ByVal Value As String)
                    strTransID = Value
                End Set
            End Property

            Public Property SurveyID() As String
                Get
                    Return strSurveyID
                End Get
                Set(ByVal Value As String)
                    strSurveyID = Value
                End Set
            End Property

            Public Property BranchID() As String
                Get
                    Return strBranchID
                End Get
                Set(ByVal Value As String)
                    strBranchID = Value
                End Set
            End Property

            Public Property Gender() As Integer
                Get
                    Return intGender
                End Get
                Set(ByVal Value As Integer)
                    intGender = Value
                End Set
            End Property

            Public Property Furniture() As String
                Get
                    Return strFurniture
                End Get
                Set(ByVal Value As String)
                    strFurniture = Value
                End Set
            End Property

            Public Property Resident() As Integer
                Get
                    Return intResident
                End Get
                Set(ByVal Value As Integer)
                    intResident = Value
                End Set
            End Property
        End Class
#End Region

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "GENDER, RACE, AGEGROUP"
                .TableName = "SURVEY"
                .Listing = "GENDER, RACE, AGEGROUP"
            End With
            'MDarren : Not sure whether add o nt??
            'MeDim MyFields(2)
            'MyBase.AddMyField(0, "Category Code", "CatgCode", TypeCode.String)
            'MyBase.AddMyField(1, "Description", "CatgDesc", TypeCode.String)
            'MyBase.AddMyField(2, "Department Code", "DeptCode", TypeCode.String)
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function AssignItem(ByVal objSurvey As Survey, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            'Dim rdr As SqlClient.SqlDataReader
            Dim objSurveyBS As New General.SysCodeBase
            Dim arySQL As New ArrayList
            Dim aryFurniture() As String
            Dim intSeqNo, iCnt As Integer

            AssignItem = False

            Try
                objSurvey.SurveyID = objSurveyBS.GenerateSurveyID(objSurvey.BranchID, Date.Today)
                intSeqNo = 0
                With objSQL
                    .TableName = MyInfo.TableName

                    'Gender
                    intSeqNo = intSeqNo + 1
                    .AddField("BranchID", objSurvey.BranchID, SQLControl.EnumDataType.dtString)
                    .AddField("SurveyID", objSurvey.SurveyID, SQLControl.EnumDataType.dtString)
                    .AddField("TransID", objSurvey.TransID, SQLControl.EnumDataType.dtString)
                    .AddField("SeqNo", intSeqNo, SQLControl.EnumDataType.dtNumeric)
                    .AddField("QuestionType", 0, SQLControl.EnumDataType.dtNumeric)
                    .AddField("SelAns", objSurvey.Gender, SQLControl.EnumDataType.dtString)
                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                    arySQL.Add(strSQL)

                    'Race
                    intSeqNo = intSeqNo + 1
                    .AddField("BranchID", objSurvey.BranchID, SQLControl.EnumDataType.dtString)
                    .AddField("SurveyID", objSurvey.SurveyID, SQLControl.EnumDataType.dtString)
                    .AddField("TransID", objSurvey.TransID, SQLControl.EnumDataType.dtString)
                    .AddField("SeqNo", intSeqNo, SQLControl.EnumDataType.dtNumeric)
                    .AddField("QuestionType", 1, SQLControl.EnumDataType.dtNumeric)
                    .AddField("SelAns", objSurvey.Race, SQLControl.EnumDataType.dtString)
                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                    arySQL.Add(strSQL)

                    'Age
                    intSeqNo = intSeqNo + 1
                    .AddField("BranchID", objSurvey.BranchID, SQLControl.EnumDataType.dtString)
                    .AddField("SurveyID", objSurvey.SurveyID, SQLControl.EnumDataType.dtString)
                    .AddField("TransID", objSurvey.TransID, SQLControl.EnumDataType.dtString)
                    .AddField("SeqNo", intSeqNo, SQLControl.EnumDataType.dtNumeric)
                    .AddField("QuestionType", 2, SQLControl.EnumDataType.dtNumeric)
                    .AddField("SelAns", objSurvey.AgeGroup, SQLControl.EnumDataType.dtString)
                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                    arySQL.Add(strSQL)

                    'Furniture
                    If objSurvey.Furniture <> String.Empty Then
                        aryFurniture = Split(objSurvey.Furniture, ",")
                        For iCnt = 0 To aryFurniture.GetUpperBound(0)
                            intSeqNo = intSeqNo + 1
                            .AddField("BranchID", objSurvey.BranchID, SQLControl.EnumDataType.dtString)
                            .AddField("SurveyID", objSurvey.SurveyID, SQLControl.EnumDataType.dtString)
                            .AddField("TransID", objSurvey.TransID, SQLControl.EnumDataType.dtString)
                            .AddField("SeqNo", intSeqNo, SQLControl.EnumDataType.dtNumeric)
                            .AddField("QuestionType", 3, SQLControl.EnumDataType.dtNumeric)
                            .AddField("SelAns", aryFurniture(iCnt), SQLControl.EnumDataType.dtString)
                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                            arySQL.Add(strSQL)
                        Next
                    End If

                    'Resident
                    intSeqNo = intSeqNo + 1
                    .AddField("BranchID", objSurvey.BranchID, SQLControl.EnumDataType.dtString)
                    .AddField("SurveyID", objSurvey.SurveyID, SQLControl.EnumDataType.dtString)
                    .AddField("TransID", objSurvey.TransID, SQLControl.EnumDataType.dtString)
                    .AddField("SeqNo", intSeqNo, SQLControl.EnumDataType.dtNumeric)
                    .AddField("QuestionType", 4, SQLControl.EnumDataType.dtNumeric)
                    .AddField("SelAns", objSurvey.Resident, SQLControl.EnumDataType.dtString)
                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                    arySQL.Add(strSQL)

                End With
                Try
                    objDCom.BatchExecute(arySQL, CommandType.Text)
                    ''objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                Catch axExecute As Exception
                    If pType = SQLControl.EnumSQLType.stInsert Then
                        Throw New ApplicationException("210002")
                    Else
                        Throw New ApplicationException("210004")
                    End If
                Finally
                    objSQL.Dispose()
                End Try
                Return True
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objSurvey = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
        'Private Function AssignItem(ByVal objSurvey As Survey, ByVal pType As SQLControl.EnumSQLType) As Boolean
        '    Dim strSQL As String
        '    Dim rdr As SqlClient.SqlDataReader
        '    Dim objSurveyBS As New General.SysCodeBase

        '    AssignItem = False

        '    Try
        '        objSurvey.SurveyID = objSurveyBS.GenerateSurveyID(objSurvey.BranchID, Date.Today)

        '        With objSQL
        '            .TableName = MyInfo.TableName
        '            .AddField("AgeGroup", objSurvey.AgeGroup, SQLControl.EnumDataType.dtNumeric)
        '            .AddField("Race", objSurvey.Race, SQLControl.EnumDataType.dtNumeric)
        '            .AddField("Gender", objSurvey.Gender, SQLControl.EnumDataType.dtNumeric)
        '            .AddField("TransID", objSurvey.TransID, SQLControl.EnumDataType.dtString)
        '            .AddField("SurveyID", objSurvey.SurveyID, SQLControl.EnumDataType.dtString)
        '            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
        '        End With

        '        Try
        '            objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

        '        Catch axExecute As Exception
        '            If pType = SQLControl.EnumSQLType.stInsert Then
        '                Throw New ApplicationException("210002")
        '            Else
        '                Throw New ApplicationException("210004")
        '            End If
        '        Finally
        '            objSQL.Dispose()
        '        End Try
        '        Return True
        '    Catch axAssign As ApplicationException
        '        Throw axAssign
        '    Catch exAssign As SystemException
        '        Throw exAssign
        '    Finally
        '        objSurvey = Nothing
        '        EndSQLControl()
        '        EndConnection()
        '    End Try
        'End Function
        Function Add(ByVal objSurvey As Survey) As Boolean
            Return AssignItem(objSurvey, SQLControl.EnumSQLType.stInsert)
        End Function

#End Region

#Region "Data Selection"

#End Region

    End Class


#End Region

#Region "Database Manager"
    Public Class DatabaseMan
        Inherits Core.CoreBase
        Private Structure StrucDBValue
            Dim Database As String
            Dim ServerName As String
        End Structure

        Private Shared Sub CheckFolder(ByVal Dir As String)
            Dim objDir As System.IO.Directory
            If objDir.Exists(Dir) = False Then
                objDir.CreateDirectory(Dir)
            End If
        End Sub

        Private Shared Function ParseConnection(ByVal ConnString As String) As StrucDBValue
            Dim DBValue As StrucDBValue
            Dim iStart, iEnd, iRun As Integer
            Try
                If ConnString Is Nothing = False Then
                    If ConnString.Trim = String.Empty Then
                        Return Nothing
                    Else
                        iStart = ConnString.IndexOf("SERVER=")
                        iStart = iStart + 7
                        iEnd = ConnString.IndexOf(";", iStart)
                        DBValue.ServerName = ConnString.Substring(iStart, iEnd - (iStart))

                        iStart = ConnString.IndexOf("DATABASE=")
                        iStart = iStart + 9
                        iEnd = ConnString.IndexOf(";", iStart)
                        DBValue.Database = ConnString.Substring(iStart, iEnd - (iStart))
                        Return DBValue
                    End If
                End If

            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        Public Shared Function BackupDatabase(Optional ByVal Path As String = "", _
            Optional ByVal FileName As String = "", Optional ByVal Copies As Integer = 2) As Boolean
            Dim DBValue As StrucDBValue
            Dim i As Integer
            Dim SavePath, TempPath As String
            DBValue = ParseConnection(objDCom.ConnectionString)
            If Path = String.Empty Then Path = System.AppDomain.CurrentDomain.BaseDirectory & "\DB"
            CheckFolder(Path)
            If FileName = String.Empty Then FileName = DBValue.Database & ".BAK"
            StartSQLControl()
            StartConnection()
            Try
                If Copies <= 1 Then
                    strSQL = "BACKUP DATABASE [" & DBValue.Database & "] TO  DISK = N'" & Path & "\" & _
                              FileName & "' WITH  INIT, NOUNLOAD,  NOSKIP ,  STATS = 20,  NOFORMAT"
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                Else
                    SavePath = Path & "\" & FileName
                    If Copies > 10 Then Copies = 9
                    For i = Copies - 1 To 1 Step -1
                        If i = Copies - 1 Then System.IO.File.Delete(String.Concat(SavePath, "0", Copies - 1))
                        TempPath = String.Concat(SavePath, "0", i - 1)
                        If System.IO.File.Exists(TempPath) Then
                            System.IO.File.Move(TempPath, String.Concat(SavePath, "0", i))
                        End If
                    Next
                    strSQL = "BACKUP DATABASE [" & DBValue.Database & "] TO  DISK = N'" & Path & "\" & _
                          FileName & "' WITH  INIT, NOUNLOAD,  NOSKIP ,  STATS = 20,  NOFORMAT"
                    objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                End If
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Shared Function RestoreDatabase(Optional ByVal Path As String = "", _
            Optional ByVal FileName As String = "") As Boolean
            Dim DBValue As StrucDBValue
            DBValue = ParseConnection(objDCom.ConnectionString)
            If Path = String.Empty Then Path = System.AppDomain.CurrentDomain.BaseDirectory & "\DB"
            CheckFolder(Path)
            If FileName = String.Empty Then FileName = DBValue.Database & ".BAK"
            StartSQLControl()
            StartConnection()
            Try
                strSQL = "RESTORE DATABASE [" & DBValue.Database & "] FROM  DISK = N'DISK = N'" & Path & "\" & _
                          FileName & "' WITH  FILE = 1,  NOUNLOAD ,  STATS = 10,  RECOVERY "
                objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

    End Class
#End Region

#Region "File Manager"
    Public Class FileMan
        Private _AppBackCopy As Integer
        Private _AppCheckFile, _AppChkCode As String

        Public Structure StrucCheckResult
            'Check 0 = Equal, 1= Source Greater, 2=Dest Greater
            Dim Status As Boolean
            Dim VersionCheck As Integer
            Dim ModifyCheck As Integer
            Dim LatestLastWrite As Date
            Dim DestVersion As String
            Dim SourceVersion As String
            Dim DestLastWrite As Date
            Dim SourceLastWrite As Date
            Dim directory As String
        End Structure

        Public Sub New()
            _AppBackCopy = 1
            _AppCheckFile = "FileMan.Info"
            _AppChkCode = "APP"
        End Sub

        Public Property AppBackupCopy() As Integer
            Get
                Return _AppBackCopy
            End Get
            Set(ByVal Value As Integer)
                If Value > 9 Then Value = 9
                If Value < 0 Then Value = 1
                _AppBackCopy = Value
            End Set
        End Property

        Public Property AppBackupCode() As String
            Get
                Return _AppChkCode
            End Get
            Set(ByVal Value As String)
                If Value Is Nothing = False Then
                    If Value <> String.Empty Then
                        _AppChkCode = Value
                    End If
                End If
            End Set
        End Property

        Private Function DeleteDirectory(ByVal DirPath As String) As Boolean
            Dim TargetFile, TargetDir As String
            Try
                For Each TargetDir In System.IO.Directory.GetDirectories(DirPath)
                    For Each TargetFile In System.IO.Directory.GetFiles(TargetDir)
                        System.IO.File.Delete(TargetFile)
                    Next
                    RmDir(TargetDir)
                Next

                For Each TargetFile In System.IO.Directory.GetFiles(DirPath)
                    System.IO.File.Delete(TargetFile)
                Next
                RmDir(DirPath)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function ApplicationBackup(ByVal SourcePath As String, _
            ByVal DestPath As String, ByVal Sync As Boolean, _
            ByVal LastestSourceDate As Date) As Boolean
            Dim AryDir(), DirPath, DirName As String
            Dim i, posSep As Integer
            Dim blnExec As Boolean
            Dim DirPathS, DirPathD, ChkFilePath, MyAppCode As String
            Dim CheckResult As StrucCheckResult
            Dim textfile As System.IO.StreamWriter
            Dim LatestSourceDate, LatestDestDate As Date
            MyAppCode = String.Concat(_AppChkCode, "0")
            ChkFilePath = String.Concat(DestPath, "\", MyAppCode, "0", "\", _AppCheckFile)
            AryDir = System.IO.Directory.GetDirectories(DestPath, (MyAppCode & "*"))
            LatestDestDate = GetChkFileLastModifyDate(ChkFilePath)

            DirPathD = DestPath
            If Date.Compare(LastestSourceDate, LatestDestDate) > 0 Then
                If AryDir.Length > 0 Then
                    'compare App01 folder and source whether which one is the most latest one
                    DirPathS = String.Concat(SourcePath, "\", _AppCheckFile)

                    For i = (AryDir.GetUpperBound(0)) To 0 Step -1
                        DirPathS = String.Concat(DestPath, "\", MyAppCode, i)
                        DirPathD = String.Concat(DestPath, "\", MyAppCode, i + 1)

                        If i <= (_AppBackCopy - 1) Then
                            'Move to App(i+1) folder
                            If System.IO.Directory.Exists(DirPathS) Then
                                If System.IO.Directory.Exists(DirPathD) Then
                                    blnExec = DeleteDirectory(DirPathD)
                                Else
                                    blnExec = True
                                End If
                                Try
                                    If blnExec Then System.IO.Directory.Move(DirPathS, (DirPathD))
                                Catch ex As Exception
                                End Try
                            End If
                        Else
                            DeleteDirectory(DirPathS)
                        End If
                    Next
                End If
                DestPath = String.Concat(DestPath, "\", MyAppCode, "0")
                System.IO.Directory.CreateDirectory(DestPath)
                DirectoryBackup(SourcePath, DestPath, Sync)

                ChkFilePath = String.Concat(DestPath, "\", _AppCheckFile)
                CreateChkFile(ChkFilePath, LastestSourceDate)
            End If
        End Function

        Private Sub CreateChkFile(ByVal ChkFilePath As String, ByVal LastestSourceDate As Date)
            Dim textfile As System.IO.StreamWriter
            If System.IO.File.Exists(ChkFilePath) Then
                System.IO.File.Delete(ChkFilePath)
            End If
            Try
                textfile = System.IO.File.CreateText(ChkFilePath)
                textfile.WriteLine(LastestSourceDate)
                textfile.Close()
            Catch err As Exception
                Throw err
            Finally
                textfile = Nothing
            End Try
        End Sub

        Public Function DirectoryBackup(ByVal SourcePath As String, ByVal DestPath As String, _
            ByVal Sync As Boolean) As Boolean
            Dim i As Integer
            Dim posSep As Integer
            Dim DirPath, FilePath As String
            Dim DirPathS, DirPathD, FilePathS, FilePathD As String
            Dim AryDir(), AryFile() As String
            Dim VerChk, CreChk As Integer 'Check 0 = Equal, 1= Source Greater, 2=Dest Greater
            Dim CheckResult As StrucCheckResult
            Dim LatestDate As Date
            ' Add trailing separators to the supplied paths if they don't exist.
            If SourcePath.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()) = False Then
                SourcePath &= System.IO.Path.DirectorySeparatorChar
            End If

            If DestPath.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()) = False Then
                DestPath &= System.IO.Path.DirectorySeparatorChar
            End If

            ' Recursive switch to continue drilling down into dir structure.
            ' Get a list of directories from the current parent.
            AryDir = System.IO.Directory.GetDirectories(SourcePath)
            For i = 0 To AryDir.GetUpperBound(0)
                DirPathS = AryDir(i)
                If (DirPathS.IndexOf(_AppChkCode & "0") >= 0) = False Then
                    ' Get the position of the last separator in the current path.
                    posSep = DirPathS.LastIndexOf("\")
                    ' Get the path of the source directory.
                    DirPath = DirPathS.Substring((posSep + 1), DirPathS.Length - (posSep + 1))
                    ' Create the new directory in the destination directory.
                    DirPathD = String.Concat(DestPath, DirPath)
                    If System.IO.Directory.Exists(DirPathD) = False Then
                        System.IO.Directory.CreateDirectory(DirPathD)
                    End If
                    ' Since we are in recursive mode, copy the children also
                    DirectoryBackup(DirPathS, DirPathD, True)
                End If
            Next


            ' Get the files from the current parent.
            AryFile = System.IO.Directory.GetFiles(SourcePath)
            ' Copy all files.
            For i = 0 To AryFile.GetUpperBound(0)
                ' Get the position of the trailing separator.
                FilePathS = AryFile(i)
                posSep = FilePathS.LastIndexOf("\")
                ' Get the full path of the source file.
                FilePath = FilePathS.Substring((posSep + 1), FilePathS.Length - (posSep + 1))
                FilePathD = String.Concat(DestPath, FilePath)
                If System.IO.File.Exists(FilePathD) Then
                    CheckResult = FileCompare(FilePathS, FilePathD)
                    If CheckResult.Status = True Then
                        If (VerChk = 0 And CreChk = 0) = False Then
                            Select Case True
                                Case (VerChk = 1) Or (CreChk = 1)
                                    System.IO.File.Copy(AryFile(i), FilePathD, True)

                                Case (VerChk = 2) Or (CreChk = 2)
                                    If Sync Then
                                        System.IO.File.Copy(FilePathD, FilePathS, True)
                                    End If
                            End Select
                        End If
                    End If
                Else
                    'COPY FILES
                    System.IO.File.Copy(FilePathS, FilePathD)
                End If
            Next i
        End Function

        Public Function DirictoryFileLatest(ByVal SourcePath As String, ByRef LatestDate As Date, _
        Optional ByVal ExceptionExtension As ArrayList = Nothing) As Boolean
            Dim i As Integer
            Dim posSep As Integer
            Dim DirPath, FilePath As String
            Dim AryDir(), AryFile() As String
            Dim FileCreateS As Date

            ' Recursive switch to continue drilling down into dir structure.
            ' Get a list of directories from the current parent.
            AryDir = System.IO.Directory.GetDirectories(SourcePath)
            For i = 0 To AryDir.GetUpperBound(0)
                DirPath = AryDir(i)
                If (DirPath.IndexOf(_AppChkCode & "0") >= 0) = False Then
                    DirictoryFileLatest(DirPath, LatestDate, ExceptionExtension)
                End If
            Next

            ' Get the files from the current parent.
            AryFile = System.IO.Directory.GetFiles(SourcePath)
            ' Copy all files.
            For i = 0 To AryFile.GetUpperBound(0)
                ' Get the position of the trailing separator.
                FilePath = AryFile(i)
                If FileExceptionCheck(FilePath, ExceptionExtension) = True Then
                    If System.IO.File.Exists(FilePath) Then
                        FileCreateS = System.IO.File.GetLastWriteTime(FilePath)
                        If LatestDate = Nothing And i = 0 Then LatestDate = FileCreateS
                        If FileCreateS > LatestDate Then
                            LatestDate = FileCreateS
                        End If
                    End If
                End If
            Next i
        End Function

        Private Function FileExceptionCheck(ByVal FilePath As String, ByVal ExpList As ArrayList) As Boolean
            Dim MyFileInfo As System.IO.FileInfo
            Dim FileExtension As String
            If ExpList Is Nothing = False Then
                If ExpList.Count > 0 Then
                    MyFileInfo = New System.IO.FileInfo(FilePath)
                    FileExtension = MyFileInfo.Extension()
                    If ExpList.Contains(FileExtension) = True Then Return False
                End If
            End If
            Return True
        End Function

        Public Function FileCompare(ByVal SourceFile As String, ByVal DestFile As String) As StrucCheckResult
            Dim FileVersion As FileVersionInfo
            Dim FileName, FilePathS, FilePathD As String
            Dim VersionS, VersionD As String
            Dim FileCreateS, FileCreateD As Date
            Dim CheckResult As StrucCheckResult
            Dim VerChk, CreChk As Integer 'Check 0 = Equal, 1= Source Greater, 2=Dest Greater

            If System.IO.File.Exists(SourceFile) And System.IO.File.Exists(DestFile) Then
                'SEARCH FOR VERSION OR CREATE_DATE
                Try
                    FileVersion = FileVersionInfo.GetVersionInfo(SourceFile)
                    VersionS = FileVersion.FileVersion
                    FileCreateS = System.IO.File.GetLastWriteTimeUtc(SourceFile)
                    FileVersion = FileVersionInfo.GetVersionInfo(DestFile)
                    VersionD = FileVersion.FileVersion
                    FileCreateD = System.IO.File.GetLastWriteTimeUtc(DestFile)

                    'Version Compare
                    If (VersionS = VersionD) Then
                        VerChk = 0
                    Else
                        If (VersionS > VersionD) Then
                            VerChk = 1
                        Else
                            VerChk = 2
                        End If
                    End If

                    With CheckResult
                        'Create Date Compare
                        If (FileCreateS = FileCreateD) Then
                            CreChk = 0
                            .LatestLastWrite = FileCreateS
                        Else
                            If (FileCreateS > FileCreateD) Then
                                .LatestLastWrite = FileCreateS
                                CreChk = 1
                            Else
                                .LatestLastWrite = FileCreateD
                                CreChk = 2
                            End If
                        End If

                        .Status = True
                        .ModifyCheck = CreChk
                        .VersionCheck = VerChk
                        .SourceLastWrite = FileCreateS
                        .DestLastWrite = FileCreateD
                        .SourceVersion = VersionS
                        .DestVersion = VersionD
                    End With
                    Return CheckResult
                Catch ex As Exception
                    CheckResult.Status = False
                    Return CheckResult
                Finally
                    FileVersion = Nothing
                End Try
            Else
                CheckResult.Status = False
                Return CheckResult
            End If
        End Function

        Public Function GetChkFileLastModifyDate(ByVal FilePath As String) As Date
            Dim LatestDestDate As Date
            Dim Reader As System.IO.StreamReader
            If System.IO.File.Exists(FilePath) Then
                Reader = New System.IO.StreamReader(FilePath)
                Try
                    LatestDestDate = Convert.ToDateTime(Reader.ReadLine)
                    Reader.Close()
                    Return LatestDestDate
                Catch ex As Exception
                    Return Nothing
                End Try
            Else
                Return Nothing
            End If
        End Function
    End Class
#End Region

#Region "Theme Base"

    Public Class ThemeBase
        Inherits Core.CoreBase

        Private dtTable As DataTable
        Private strSQL As String

        Public Function GetList(Optional ByVal ImageType As String = "") As DataTable
            Try
                strSQL = "SELECT ImageCode, ImageType, ImageDesc, ImageLink FROM SYSTHEME "
                Select Case ImageType
                    Case "Predefine"
                        strSQL = strSQL & "WHERE ImageCode LIKE 'P%'"
                    Case "UserDefine"
                        strSQL = strSQL & "WHERE ImageCode LIKE 'U%'"
                End Select
                objDCom.OpenConnection()
                dtTable = CType(objDCom.Execute(strSQL, SEAL.Data.DataAccess.EnumRtnType.rtDataTable, CommandType.Text), DataTable)
                'objDCom.CloseConnection()
                Return dtTable
            Catch ex As Exception

            End Try
        End Function

        Public Sub SaveImage(ByVal ImageLocation As String)
            Dim strImageCode As String
            'Generate()
            Try
                strSQL = "UPDATE SYSTHEME SET ImageCode = "
            Catch ex As Exception

            End Try
        End Sub

    End Class
#End Region

#Region "Destination Base"
    Public Class DestinationBase
        Inherits Core.CoreBase

#Region "Destination Class in DestinationBase Class"

        Public Class Destination
            Private strDestinationID, strDescription, strAddress1, strAddress2, strAddress3, strAddress4 As String
            Private intSeqNo As Integer
            Private dtLastUpdate As Date
            Private strUpdateBy As String
            Private dtDestinationList As DataTable

#Region "Public Property in Destination Class"

            Public Property DestinationID() As String
                Get
                    Return strDestinationID
                End Get
                Set(ByVal value As String)
                    strDestinationID = value
                End Set
            End Property

            Public Property Description() As String
                Get
                    Return strDescription
                End Get
                Set(ByVal value As String)
                    strDescription = value
                End Set
            End Property

            Public Property Address1() As String
                Get
                    Return strAddress1
                End Get
                Set(ByVal value As String)
                    strAddress1 = value
                End Set
            End Property

            Public Property Address2() As String
                Get
                    Return strAddress2
                End Get
                Set(ByVal value As String)
                    strAddress2 = value
                End Set
            End Property

            Public Property Address3() As String
                Get
                    Return strAddress3
                End Get
                Set(ByVal value As String)
                    strAddress3 = value
                End Set
            End Property

            Public Property Address4() As String
                Get
                    Return strAddress4
                End Get
                Set(ByVal value As String)
                    strAddress4 = value
                End Set
            End Property

            Public Property SeqNo() As Integer
                Get
                    Return intSeqNo
                End Get
                Set(ByVal value As Integer)
                    intSeqNo = value
                End Set
            End Property

            Public Property LastUpdate() As Date
                Get
                    Return dtLastUpdate
                End Get
                Set(ByVal value As Date)
                    dtLastUpdate = value
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return strUpdateBy
                End Get
                Set(ByVal value As String)
                    strUpdateBy = value
                End Set
            End Property

            Public Property DestinationList() As DataTable
                Get
                    Return dtDestinationList
                End Get
                Set(ByVal value As DataTable)
                    dtDestinationList = value
                End Set
            End Property

#End Region

        End Class

#End Region

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "DestinationID, Description, Address1, Address2, Address3, Address4 "
                .CheckFields = "DestinationID, Flag "
                .TableName = "DESTINATION"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty

                .Listing = "DestinationID, Description, Address1, Address2, Address3, Address4, SeqNo "
                .ListingCond = "Flag = 1"
                .ShortList = "DestinationID, Description"
                .ShortListCond = String.Empty
            End With

            ReDim MyFields(1)
            MyBase.AddMyField(0, "DestinationID", "DestinationID", TypeCode.String)
            MyBase.AddMyField(1, "Description", "Description", TypeCode.String)
        End Sub

#Region "Destination Data Manipulation-Add,Edit,Del"

        Private Function AssignItem(ByVal objDestination As Destination, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim blnExec As Boolean, blnFound As Boolean
            Dim blnFlag As Boolean
            Dim rdr As SqlClient.SqlDataReader
            AssignItem = False
            Try
                If objDestination Is Nothing Then
                    'msg return
                Else
                    'Dim test As ArrayList
                    'test.Add("A")
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "DestinationID ='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objDestination.DestinationID) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        blnExec = True 'executed - select
                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    'record is found
                                    blnFound = True
                                    If rdr("Flag").ToString = "1" Then
                                        blnFlag = True
                                    Else
                                        blnFlag = False
                                    End If
                                End If
                                .Close()
                            End With
                        End If
                    End If

                    If blnExec Then 'if executed
                        If blnFound = True And blnFlag = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = MyInfo.TableName
                                .AddField("Description", objDestination.Description, SQLControl.EnumDataType.dtString)
                                .AddField("Address1", objDestination.Address1, SQLControl.EnumDataType.dtString)
                                .AddField("Address2", objDestination.Address2, SQLControl.EnumDataType.dtString)
                                .AddField("Address3", objDestination.Address3, SQLControl.EnumDataType.dtString)
                                .AddField("Address4", objDestination.Address4, SQLControl.EnumDataType.dtString)
                                .AddField("SeqNo", objDestination.SeqNo, SQLControl.EnumDataType.dtNumeric)
                                .AddField("UpdateBy", objDestination.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", objDestination.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)
                                .AddField("Flag", cFlagActive, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cEmpty)
                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert 'INSERT
                                        'record found but not deleted
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE DestinationID = '" & .ParseValue(SQLControl.EnumDataType.dtString, objDestination.DestinationID) & "'")
                                        Else
                                            If blnFound = False Then 'if record not found
                                                .AddField("DestinationID", objDestination.DestinationID, SQLControl.EnumDataType.dtString)
                                                .AddField("CreateBy", objDestination.UpdateBy, SQLControl.EnumDataType.dtString)
                                                .AddField("CreateDate", objDestination.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate 'UPDATE
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE DestinationID='" & .ParseValue(SQLControl.EnumDataType.dtString, objDestination.DestinationID) & "'")
                                End Select
                            End With
                            Try
                                'execute
                                objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                Return True
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002")
                                Else
                                    Throw New ApplicationException("210004")
                                End If
                            Finally
                                objSQL.Dispose()
                            End Try
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objDestination = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Function Add(ByVal objDestination As Destination) As Boolean
            Return AssignItem(objDestination, SQLControl.EnumSQLType.stInsert)
        End Function

        'AMEND
        Function Amend(ByVal objDestination As Destination) As Boolean
            Return AssignItem(objDestination, SQLControl.EnumSQLType.stUpdate)
        End Function

        'DELETE
        Function Delete(ByVal objDestination As Destination) As Boolean
            Dim strSQL As String
            Dim arrsql As ArrayList
            Dim blnFound, blnInUse As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If objDestination Is Nothing Then
                    'error message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        arrsql = New ArrayList
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "DestinationID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objDestination.DestinationID) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                End If
                                .Close()
                            End With
                        End If
 
                        If blnFound = True Then
                            strSQL = "UPDATE DESTINATION SET Flag = 0 WHERE DestinationID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objDestination.DestinationID) & "'"
                            arrsql.Add(strSQL)
                            'strSQL = BuildDelete(MyInfo.TableName, "DestinationID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objDestination.DestinationID) & "'")
                            'arrsql.Add(strSQL)
                        End If
                        Try
                            'execute
                            objDCom.BatchExecute(arrsql, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try
                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As Exception
                Throw exDelete
            Finally
                objDestination = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        'Enquiry
        Function Enquiry(Optional ByVal SQLstmt As String = Nothing) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    If SQLstmt = Nothing Or SQLstmt = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName, .ListingCond)
                    Else
                        strSQL = SQLstmt
                    End If
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetListing() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    strSQL = strSQL & " ORDER BY SeqNo "
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetShortList() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

#End Region

#Region "Others Function"
        Public Function AmendSequence(ByVal dsCode As DataSet, ByVal intSeq As Integer, ByVal DestinationID As String) As Boolean
            Dim strsql As String
            Dim drData As DataRow
            Dim arySQL As ArrayList
            Try
                If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                    StartSQLControl()
                    If intSeq = 0 Then
                        With objSQL
                            .TableName = "Destination"
                            .AddField("SeqNo", Convert.ToString(0), SQLControl.EnumDataType.dtNumeric)
                            strsql = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE DestinationID ='" & DestinationID & "'")
                        End With
                        objDCom.Execute(strsql, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                    Else
                        arySQL = New ArrayList
                        With objSQL
                            For Each drData In dsCode.Tables(0).Rows
                                .TableName = "Destination"
                                .AddField("SeqNo", Convert.ToString(drData("SeqNo")), SQLControl.EnumDataType.dtNumeric)
                                arySQL.Add(.BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE DestinationID ='" & drData("DestinationID").ToString & "'"))
                            Next
                            objDCom.BatchExecute(arySQL, CommandType.Text)
                        End With
                    End If
                End If
                Return True
            Catch ex As Exception
                Return False
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

    End Class
#End Region

#Region "UOMDetail Base"
    Public Class UOMDetailBase
        Inherits Core.CoreBase

        Public Enum EnumSelectType
            UOMHDR = 0
            UOMDTL = 1
            UOMVALUE = 2
        End Enum

#Region "UOM class in UOM Base"
        Public Class UOMDetail
            Private strUOMType, strUOMDesc, strSysCode, strDisplayAs As String
            Private intCodeSeq As Integer
            Private dtUOMDTL As DataTable

            Public Property UOMType() As String
                Get
                    Return strUOMType
                End Get
                Set(ByVal value As String)
                    strUOMType = value
                End Set
            End Property

            Public Property UOMDesc() As String
                Get
                    Return strUOMDesc
                End Get
                Set(ByVal value As String)
                    strUOMDesc = value
                End Set
            End Property

            Public Property CodeSeq() As Integer
                Get
                    Return intCodeSeq
                End Get
                Set(ByVal value As Integer)
                    intCodeSeq = value
                End Set
            End Property

            Public Property SysCode() As String
                Get
                    Return strSysCode
                End Get
                Set(ByVal value As String)
                    strSysCode = value
                End Set
            End Property

            Public Property UOMDTL() As DataTable
                Get
                    Return dtUOMDTL
                End Get
                Set(ByVal value As DataTable)
                    dtUOMDTL = value
                End Set
            End Property

            Public Property DisplayAs() As String
                Get
                    Return strDisplayAs
                End Get
                Set(ByVal value As String)
                    strDisplayAs = value
                End Set
            End Property
        End Class
#End Region

#Region "UOMVALUE class in UOM Base"

        Public Class UOMValue
            'Private strUOMType, strUOMCode, strUOMDesc As String
            'Private dblMeasure As Double
            Private strCode As String
            Private dtLastUpdate As Date
            Private strUpdateBy As String
            Private dtUOMValueList As DataTable

            'Public Property UOMType() As String
            '    Get
            '        Return strUOMType
            '    End Get
            '    Set(ByVal value As String)
            '        strUOMType = value
            '    End Set
            'End Property

            'Public Property UOMCode() As String
            '    Get
            '        Return strUOMCode
            '    End Get
            '    Set(ByVal value As String)
            '        strUOMCode = value
            '    End Set
            'End Property

            'Public Property UOMDesc() As String
            '    Get
            '        Return strUOMDesc
            '    End Get
            '    Set(ByVal value As String)
            '        strUOMDesc = value
            '    End Set
            'End Property

            'Public Property Measure() As Double
            '    Get
            '        Return dblMeasure
            '    End Get
            '    Set(ByVal value As Double)
            '        dblMeasure = value
            '    End Set
            'End Property

            Public Property Code() As String
                Get
                    Return strCode
                End Get
                Set(ByVal value As String)
                    strCode = value
                End Set
            End Property

            Public Property UOMValueList() As DataTable
                Get
                    Return dtUOMValueList
                End Get
                Set(ByVal value As DataTable)
                    dtUOMValueList = value
                End Set
            End Property

            Public Property LastUpdate() As Date
                Get
                    Return dtLastUpdate
                End Get
                Set(ByVal value As Date)
                    dtLastUpdate = value
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return strUpdateBy
                End Get
                Set(ByVal value As String)
                    strUpdateBy = value
                End Set
            End Property

        End Class

#End Region

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = " UOMType, UOMDesc, CodeSeq, DisplayAs, SysCode "
                .CheckFields = "UOMType"
                .TableName = "UOMHDR"
                .DefaultCond = ""
                .DefaultOrder = String.Empty
                .Listing = " UOMType, UOMDesc, DisplayAs, CodeSeq"
                .ListingCond = ""
                .ShortList = "UOMType, UOMDESC"
                .ShortListCond = String.Empty
            End With
            ReDim MyFields(1)
            MyBase.AddMyField(0, "UOM Code", "UOMCode", TypeCode.String)
            MyBase.AddMyField(1, "UOM Desc", "UOMDesc", TypeCode.String)
        End Sub

#Region "Data Manipulation-Add,Edit,Del"

        Private Function AssignItem(ByVal objUOM As UOMDetail, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim arrSQL As New ArrayList
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As SqlClient.SqlDataReader
            AssignItem = False
            Try
                If objUOM Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False

                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "UOMType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objUOM.UOMType) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        blnExec = True

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                End If
                                .Close()
                            End With
                        End If
                    End If



                    If blnExec Then
                        If blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                            Return False
                        Else
                            StartSQLControl()

                            strSQL = BuildDelete("UOMDTL", "UOMType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objUOM.UOMType) & "'")
                            arrSQL.Add(strSQL)
                            With objSQL
                                .TableName = "UOMHDR"
                                .AddField("UOMDesc", objUOM.UOMDesc, SQLControl.EnumDataType.dtStringN, SQLControl.EnumValidate.cEmpty)
                                .AddField("DisplayAs", objUOM.DisplayAs, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                                If objUOM.CodeSeq >= 0 Then
                                    .AddField("CodeSeq", objUOM.CodeSeq, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cEmpty)
                                End If
                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                            "WHERE UOMType='" & .ParseValue(SQLControl.EnumDataType.dtString, objUOM.UOMType) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("UOMType", objUOM.UOMType, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , _
                                        "WHERE UOMType='" & .ParseValue(SQLControl.EnumDataType.dtString, objUOM.UOMType) & "'")
                                End Select
                                arrSQL.Add(strSQL)

                                For Each drRow As DataRow In objUOM.UOMDTL.Rows
                                    .TableName = "UOMDTL"
                                    .AddField("UOMType", objUOM.UOMType, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                                    .AddField("Code", drRow("Code").ToString, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                                    .AddField("CodeDesc", drRow("CodeDesc").ToString, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                                    .AddField("CodeSeq", drRow("CodeSeq").ToString, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                                    .AddField("MeasureType", drRow("MeasureType").ToString, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                    arrSQL.Add(strSQL)
                                Next
                            End With
                            Try
                                'execute
                                objDCom.BatchExecute(arrSQL, CommandType.Text)
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002")
                                Else
                                    Throw New ApplicationException("210004")
                                End If
                            Finally
                                objSQL.Dispose()
                            End Try
                            Return True
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objUOM = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
        'ADD
        Function Add(ByVal objUOMDetail As UOMDetail) As Boolean
            Return AssignItem(objUOMDetail, SQLControl.EnumSQLType.stInsert)
        End Function
        'AMEND
        Function Amend(ByVal objUOMDetail As UOMDetail) As Boolean
            Return AssignItem(objUOMDetail, SQLControl.EnumSQLType.stUpdate)
        End Function
        'DELETE
        Function Delete(ByVal objUOMDetail As UOMDetail) As Boolean
            Dim strSQL As String
            Dim arrSQL As New ArrayList
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If objUOMDetail Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "UOMType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objUOMDetail.UOMType) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                End If
                                .Close()
                            End With
                        End If

                        If blnFound = False Then
                            'Error Message
                            Return False
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(MyInfo.TableName, "UOMType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objUOMDetail.UOMType) & "'")
                            arrSQL.Add(strSQL)
                            strSQL = BuildDelete("UOMDTL", "UOMType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objUOMDetail.UOMType) & "'")
                            arrSQL.Add(strSQL)
                        End If
                        Try
                            'execute
                            objDCom.BatchExecute(arrSQL, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try
                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As SystemException
                Throw exDelete
            Finally
                objUOMDetail = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Private Function AssignUnitItem(ByVal objUOMValue As UOMValue, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim arrSQL As New ArrayList
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As SqlClient.SqlDataReader
            AssignUnitItem = False
            Try
                If objUOMValue Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False

                    StartSQLControl()

                    strSQL = BuildDelete("UOMValue", "Code = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objUOMValue.Code) & "'")
                    arrSQL.Add(strSQL)
                    With objSQL
                        For Each drRow As DataRow In objUOMValue.UOMValueList.Rows
                            'UnitCode, UnitDesc, Measure, CodeSeq
                            .TableName = "UOMValue"
                            .AddField("Code", objUOMValue.Code, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                            .AddField("UnitCode", drRow("UnitCode").ToString, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                            .AddField("UnitDesc", drRow("UnitDesc").ToString, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                            .AddField("Measure", drRow("Measure").ToString, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                            .AddField("CodeSeq", drRow("CodeSeq").ToString, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                            arrSQL.Add(strSQL)
                        Next
                    End With
                    Try
                        'execute
                        objDCom.BatchExecute(arrSQL, CommandType.Text)
                    Catch axExecute As Exception
                        If pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210002")
                        Else
                            Throw New ApplicationException("210004")
                        End If
                    Finally
                        objSQL.Dispose()
                    End Try
                    Return True
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objUOMValue = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
        'ADD
        Function AddUnit(ByVal objUOMValue As UOMValue) As Boolean
            Return AssignUnitItem(objUOMValue, SQLControl.EnumSQLType.stInsert)
        End Function
        'AMEND
        Function AmendUnit(ByVal objUOMValue As UOMValue) As Boolean
            Return AssignUnitItem(objUOMValue, SQLControl.EnumSQLType.stUpdate)
        End Function
        'DELETE
        Function DeleteUnit(ByVal objUOMValue As UOMValue) As Boolean
            Dim strSQL As String
            Dim arrSQL As New ArrayList
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As SqlClient.SqlDataReader
            DeleteUnit = False
            blnFound = False

            Try
                If objUOMValue Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect("Code", "UOMVALUE", "Code = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objUOMValue.Code) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                End If
                                .Close()
                            End With
                        End If

                        If blnFound = False Then
                            'Error Message
                            Return False
                        End If

                        If blnFound = True Then
                            strSQL = BuildDelete("UOMValue", "Code = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objUOMValue.Code) & "'")
                            arrSQL.Add(strSQL)
                            strSQL = BuildDelete("UOMDTL", "Code = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objUOMValue.Code) & "'")
                            arrSQL.Add(strSQL)
                            strSQL = BuildDelete("CODEMASTER", "Code = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objUOMValue.Code) & "' AND CodeType = 'UOM'")
                            arrSQL.Add(strSQL)
                        End If
                        Try
                            'execute
                            objDCom.BatchExecute(arrSQL, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try
                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As SystemException
                Throw exDelete
            Finally
                objUOMValue = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        'Enquiry
        Function Enquiry(Optional ByVal SQLstmt As String = Nothing) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    If SQLstmt = Nothing Or SQLstmt = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName)
                    Else
                        strSQL = SQLstmt
                    End If
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function EnquiryDetail(ByVal UOMType As String) As DataSet
            Try
                StartConnection()
                StartSQLControl()
                strSQL = "SELECT UOMType, Code, CodeDesc, MeasureType, CodeSeq From UOMDTL WHERE UOMType = '" & UOMType & "' " & _
                            "ORDER BY UOMType, CodeSeq "
                Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text), DataSet)
            Catch ex As Exception
                Return Nothing
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function EnquiryMeasureUnit(Optional ByVal Code As String = "") As DataSet
            Try
                StartConnection()
                StartSQLControl()
                strSQL = "SELECT Code, CodeDesc FROM CODEMASTER WHERE CodeType = 'UOM' ORDER BY CodeSeq "

                If Code <> String.Empty Then
                    strSQL = strSQL & "WHERE Code IN ('" & Code & "') "
                End If

                Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text), DataSet)
            Catch ex As Exception
                Return Nothing
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function EnquiryMeasureValue(ByVal Code As String) As DataSet
            Try
                StartConnection()
                StartSQLControl()
                strSQL = "SELECT Code, UnitCode, UnitDesc, Measure, CodeSeq From UOMValue WHERE Code = '" & Code & "' " & _
                            "ORDER BY Code, CodeSeq "
                Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text), DataSet)
            Catch ex As Exception
                Return Nothing
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function GetListing() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetShortList() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetUOMDetail(Optional ByVal CodeType As String = "") As DataSet
            Dim strSQL As String
            Try
                StartConnection()
                StartSQLControl()
                strSQL = "SELECT  DTL.UOMType, DTL.Code, DTL.CodeDesc, DTL.CodeSeq, DTL.MeasureType " & _
                            "FROM UOMDTL AS DTL "
                'strSQL = "SELECT  DTL.UOMType, DTL.Code, DTL.CodeDesc, DTL.CodeSeq, DTL.FieldNumber, DTL.MeasureType, DTL.MeasureCode " & _
                '            "FROM UOMDTL AS DTL"
                If CodeType <> String.Empty Then
                    strSQL = strSQL & "CodeType = '" & CodeType & "' "
                End If
                strSQL = strSQL & "ORDER BY DTL.UOMType"
                Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text), DataSet)
            Catch ex As Exception
                Return Nothing
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function GetUOMMeasure(ByVal MeasureType As String) As DataSet
            Dim strSQL As String
            Try
                StartConnection()
                StartSQLControl()
                strSQL = "SELECT UOMType, Code, CodeDesc, CodeSeq, MeasureType, MeasureCode FROM UOMDETAIL "
                If MeasureType <> String.Empty Then
                    strSQL = strSQL & "CodeType = '" & MeasureType & "'"
                End If
                Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text), DataSet)
            Catch ex As Exception
                Return Nothing
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function GetCombo(ByVal MeasureCode As String) As DataSet
            Dim strSQL As String
            Try
                StartConnection()
                StartSQLControl()
                strSQL = "SELECT UnitCode, UnitDesc FROM UOMVALUE "
                If MeasureCode <> String.Empty Then
                    strSQL = strSQL & "WHERE Code = '" & MeasureCode & "' "
                End If
                strSQL = strSQL & "ORDER BY CodeSeq"
                Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text), DataSet)
            Catch ex As Exception
                Return Nothing
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function GetComboFromCodeMaster(ByVal MeasureCode As String) As DataSet
            Dim strSQL As String
            Try
                StartConnection()
                StartSQLControl()
                strSQL = "SELECT Code, CodeDesc FROM CodeMaster "
                If MeasureCode <> String.Empty Then
                    strSQL = strSQL & "WHERE CodeType = '" & MeasureCode & "' "
                End If
                strSQL = strSQL & "ORDER BY CodeSeq"
                Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text), DataSet)
            Catch ex As Exception
                Return Nothing
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region
        'Clear all data in database
        Function ClearDB() As Boolean
        End Function

    End Class
#End Region

#Region "Document Base"
    Public Class DocumentBase
        Inherits Core.CoreBase

        Public Enum EnumSelectType
            Project = 0
            Document = 1
        End Enum
        Dim State As StrucClassInfo


#Region "Project class in Document Base"
        Public Class Project
            Inherits Core.SingleBase
            'BranchID()
            'ProjectCode()
            'ProjectName()
            'Description()
            'Remark()
            'Status()
            'LastUpdate()
            'UpdateBy()

            Protected strBranchID, strProjectCode, strProjectName, strDescription, strRemark As String
            Protected strUpdateBy As String
            Protected dtLastUpdate As Date
            Protected intStatus As Integer
            Protected dtDocumentList As DataTable

            Public Property BranchID() As String
                Get
                    Return strBranchID
                End Get
                Set(ByVal value As String)
                    strBranchID = value
                End Set
            End Property

            Public Property ProjectCode() As String
                Get
                    Return strProjectCode
                End Get
                Set(ByVal value As String)
                    strProjectCode = value
                End Set
            End Property

            Public Property ProjectName() As String
                Get
                    Return strProjectName
                End Get
                Set(ByVal value As String)
                    strProjectName = value
                End Set
            End Property

            Public Property Description() As String
                Get
                    Return strDescription
                End Get
                Set(ByVal value As String)
                    strDescription = value
                End Set
            End Property

            Public Property Remark() As String
                Get
                    Return strRemark
                End Get
                Set(ByVal value As String)
                    strRemark = value
                End Set
            End Property

            Public Property UpdateBy() As String
                Get
                    Return strUpdateBy
                End Get
                Set(ByVal value As String)
                    strUpdateBy = value
                End Set
            End Property

            Public Property LastUpdate() As Date
                Get
                    Return dtLastUpdate
                End Get
                Set(ByVal value As Date)
                    dtLastUpdate = value
                End Set
            End Property

            Public Property Status() As Integer
                Get
                    Return intStatus
                End Get
                Set(ByVal value As Integer)
                    intStatus = value
                End Set
            End Property

            Public Property DocumentList() As DataTable
                Get
                    Return dtDocumentList
                End Get
                Set(ByVal value As DataTable)
                    dtDocumentList = value
                End Set
            End Property
        End Class

#End Region

        '#Region "Document class in Document Base"
        '        Public Class Document
        '            Inherits Core.SingleBase

        '            Protected strBranchID, strDocCode, strDocName, strDescription, strVersionNo As String
        '            Protected dtDocDate As Date
        '            Protected strDocDestination, strRemark, strUpdateBy As String
        '            Protected dtLastUpdate As Date
        '            Protected intStatus As Integer

        '            Public Property BranchID() As String
        '                Get
        '                    Return strBranchID
        '                End Get
        '                Set(ByVal value As String)
        '                    strBranchID = value
        '                End Set
        '            End Property

        '            Public Property DocCode() As String
        '                Get
        '                    Return strDocCode
        '                End Get
        '                Set(ByVal value As String)
        '                    strDocCode = value
        '                End Set
        '            End Property

        '            Public Property DocName() As String
        '                Get
        '                    Return strDocName
        '                End Get
        '                Set(ByVal value As String)
        '                    strDocName = value
        '                End Set
        '            End Property

        '            Public Property Description() As String
        '                Get
        '                    Return strDescription
        '                End Get
        '                Set(ByVal value As String)
        '                    strDescription = value
        '                End Set
        '            End Property

        '            Public Property VersionNo() As String
        '                Get
        '                    Return strVersionNo
        '                End Get
        '                Set(ByVal value As String)
        '                    strVersionNo = value
        '                End Set
        '            End Property

        '            Public Property DocDate() As Date
        '                Get
        '                    Return dtDocDate
        '                End Get
        '                Set(ByVal value As Date)
        '                    dtDocDate = value
        '                End Set
        '            End Property

        '            Public Property DocDestination() As String
        '                Get
        '                    Return DocDestination
        '                End Get
        '                Set(ByVal value As String)
        '                    DocDestination = value
        '                End Set
        '            End Property

        '            Public Property Remark() As String
        '                Get
        '                    Return strRemark
        '                End Get
        '                Set(ByVal value As String)
        '                    strRemark = value
        '                End Set
        '            End Property

        '            Public Property UpdateBy() As String
        '                Get
        '                    Return strUpdateBy
        '                End Get
        '                Set(ByVal value As String)
        '                    strUpdateBy = value
        '                End Set
        '            End Property

        '            Public Property LastUpdate() As Date
        '                Get
        '                    Return dtLastUpdate
        '                End Get
        '                Set(ByVal value As Date)
        '                    dtLastUpdate = value
        '                End Set
        '            End Property

        '            Public Property Status() As Integer
        '                Get
        '                    Return intStatus
        '                End Get
        '                Set(ByVal value As Integer)
        '                    intStatus = value
        '                End Set
        '            End Property
        '        End Class

        '#End Region

        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "BranchID, ProjectCode, ProjectName, Description, Remark, Status, LastUpdate, UpdateBy "
                .CheckFields = "ProjectCode"
                .TableName = "DOCUMENTHDR"
                .DefaultCond = String.Empty
                .DefaultOrder = String.Empty
                .Listing = "BranchID, ProjectCode, ProjectName, Description, Remark, Status, LastUpdate, UpdateBy "
                .ListingCond = "Status = 1"
                .ShortList = String.Empty
                .ShortListCond = String.Empty
            End With

        End Sub

#Region "Document Data Manipulation-Add,Edit,Del"

        Private Function AssignItem(ByVal objProject As Project, ByVal pType As SQLControl.EnumSQLType) As Boolean
            Dim strSQL As String
            Dim arrSQL As New ArrayList
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As SqlClient.SqlDataReader
            AssignItem = False
            Try
                If objProject Is Nothing Then
                    'msg return
                Else
                    'Dim test As ArrayList
                    'test.Add("A")
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "ProjectCode ='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objProject.ProjectCode) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)

                        blnExec = True 'executed - select
                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    'record is found
                                    blnFound = True
                                End If
                                .Close()
                            End With
                        End If
                    End If

                    If blnFound = True Then
                        strSQL = BuildDelete("DOCUMENTDTL", "ProjectCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objProject.ProjectCode) & "'")
                        arrSQL.Add(strSQL)
                    End If

                    If blnExec Then 'if executed
                        If blnFlag = True And blnFound = True And pType = SQLControl.EnumSQLType.stInsert Then
                            Throw New ApplicationException("210011")
                        Else

                            StartSQLControl()
                            With objSQL
                                .TableName = MyInfo.TableName
                                .AddField("ProjectName", objProject.ProjectName, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                                .AddField("Description", objProject.Description, SQLControl.EnumDataType.dtStringN, SQLControl.EnumValidate.cEmpty)
                                .AddField("Remark", objProject.Remark, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                                .AddField("Status", objProject.Status, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cEmpty)
                                .AddField("UpdateBy", objProject.UpdateBy, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                                .AddField("LastUpdate", objProject.LastUpdate.ToString, SQLControl.EnumDataType.dtDateTime)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert 'INSERT
                                        'record found but not deleted
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE ProjectCode = '" & .ParseValue(SQLControl.EnumDataType.dtString, objProject.ProjectCode) & "'")
                                        Else
                                            If blnFound = False Then 'if record not found
                                                .AddField("ProjectCode", objProject.ProjectCode, SQLControl.EnumDataType.dtString)
                                                .AddField("BranchID", objProject.BranchID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate 'UPDATE
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, , "WHERE ProjectCode='" & .ParseValue(SQLControl.EnumDataType.dtString, objProject.ProjectCode) & "'")
                                End Select
                                arrSQL.Add(strSQL)

                                For Each drRow As DataRow In objProject.DocumentList.Rows
                                    .TableName = "DOCUMENTDTL"
                                    .AddField("BranchID", objProject.BranchID, SQLControl.EnumDataType.dtString)
                                    .AddField("DocGUID", drRow("DocGUID").ToString, SQLControl.EnumDataType.dtString)
                                    .AddField("ProjectCode", objProject.ProjectCode, SQLControl.EnumDataType.dtString)
                                    .AddField("DocCode", drRow("DocCode").ToString, SQLControl.EnumDataType.dtString)
                                    .AddField("DocName", drRow("DocName").ToString, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                                    .AddField("Description", drRow("Description").ToString, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                                    .AddField("VersionNo", drRow("VersionNo").ToString, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                                    .AddField("DocDate", CDate(drRow("DocDate")), SQLControl.EnumDataType.dtDateTime, SQLControl.EnumValidate.cEmpty)
                                    .AddField("DocDestination", drRow("DocDestination").ToString, SQLControl.EnumDataType.dtString, SQLControl.EnumValidate.cEmpty)
                                    .AddField("Status", 1, SQLControl.EnumDataType.dtNumeric, SQLControl.EnumValidate.cEmpty)
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                    arrSQL.Add(strSQL)
                                Next
                            End With
                            Try
                                'execute
                                objDCom.BatchExecute(arrSQL, CommandType.Text)
                                Return True
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002")
                                Else
                                    Throw New ApplicationException("210004")
                                End If
                            Finally
                                objSQL.Dispose()
                            End Try
                        End If
                    End If
                End If
            Catch axAssign As ApplicationException
                Throw axAssign
            Catch exAssign As SystemException
                Throw exAssign
            Finally
                objProject = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Function Add(ByVal objProject As Project) As Boolean
            Return AssignItem(objProject, SQLControl.EnumSQLType.stInsert)
        End Function

        'AMEND
        Function Amend(ByVal objProject As Project) As Boolean
            Return AssignItem(objProject, SQLControl.EnumSQLType.stUpdate)
        End Function

        'DELETE
        Function Delete(ByVal objProject As Project) As Boolean
            Dim strSQL As String
            Dim arrsql As ArrayList
            Dim blnFound, blnInUse As Boolean
            Dim rdr As SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If objProject Is Nothing Then
                    'error message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        arrsql = New ArrayList
                        With MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "ProjectCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objProject.ProjectCode) & "'")
                        End With
                        rdr = CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), SqlClient.SqlDataReader)
                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                End If
                                .Close()
                            End With
                        End If

                        'record exist but not in use
                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(MyInfo.TableName, "ProjectCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objProject.ProjectCode) & "' AND BranchID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, objProject.BranchID) & "'")
                            arrsql.Add(strSQL)
                        End If
                        Try
                            'execute
                            objDCom.BatchExecute(arrsql, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            Throw New ApplicationException("210006")
                        End Try
                    End If
                End If
            Catch axDelete As ApplicationException
                Throw axDelete
            Catch exDelete As Exception
                Throw exDelete
            Finally
                objProject = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        'Enquiry
        Function Enquiry(Optional ByVal SQLstmt As String = Nothing) As DataSet
            If StartConnection() = True Then
                With MyInfo
                    If SQLstmt = Nothing Or SQLstmt = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName)
                    Else
                        strSQL = SQLstmt
                    End If
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetListing() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetShortList() As DataSet
            If StartConnection() = True Then
                With MyInfo
                    strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataset, CommandType.Text, .TableName), DataSet)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetDocumentList(ByVal BranchID As String, ByVal ProjectCode As String) As DataTable
            Try
                StartConnection()
                StartSQLControl()
                strSQL = "SELECT DocGUID, DocCode, DocName, Description, VersionNo, DocDate, DocDestination " & _
                            "FROM DOCUMENTDTL " & _
                            "WHERE (BranchID = '" & BranchID & "') AND (ProjectCode = '" & ProjectCode & "') "
                Return CType(objDCom.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text), DataTable)
            Catch ex As Exception
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

#End Region

        'Clear all data in database

    End Class
#End Region

#Region "URLCrypto"
    Public Class SimpleAES
        ' Change these keys
        Private Key As Byte() = {123, 217, 19, 11, 24, 26, _
         85, 45, 114, 184, 27, 162, _
         37, 112, 222, 209, 241, 24, _
         175, 144, 173, 53, 196, 29, _
         24, 26, 17, 218, 131, 236, _
         53, 209}
        Private Vector As Byte() = {146, 64, 191, 111, 23, 3, _
         113, 119, 231, 121, 251, 112, _
         79, 32, 114, 156}


        Private EncryptorTransform As ICryptoTransform, DecryptorTransform As ICryptoTransform
        Private UTFEncoder As System.Text.UTF8Encoding

        Public Sub New()
            'This is our encryption method
            Dim rm As New RijndaelManaged()

            'Create an encryptor and a decryptor using our encryption method, key, and vector.
            EncryptorTransform = rm.CreateEncryptor(Me.Key, Me.Vector)
            DecryptorTransform = rm.CreateDecryptor(Me.Key, Me.Vector)

            'Used to translate bytes to text and vice versa
            UTFEncoder = New System.Text.UTF8Encoding()
        End Sub

        ''' -------------- Two Utility Methods (not used but may be useful) -----------
        ''' Generates an encryption key.
        Public Shared Function GenerateEncryptionKey() As Byte()
            'Generate a Key.
            Dim rm As New RijndaelManaged()
            rm.GenerateKey()
            Return rm.Key
        End Function

        ''' Generates a unique encryption vector
        Public Shared Function GenerateEncryptionVector() As Byte()
            'Generate a Vector
            Dim rm As New RijndaelManaged()
            rm.GenerateIV()
            Return rm.IV
        End Function


        ''' ----------- The commonly used methods ------------------------------    
        ''' Encrypt some text and return a string suitable for passing in a URL.
        Public Function EncryptToString(TextValue As String) As String
            Return ByteArrToString(Encrypt(TextValue))
        End Function

        ''' Encrypt some text and return an encrypted byte array.
        Public Function Encrypt(TextValue As String) As Byte()
            'Translates our text value into a byte array.
            Dim bytes As [Byte]() = UTFEncoder.GetBytes(TextValue)

            'Used to stream the data in and out of the CryptoStream.
            Dim memoryStream As New MemoryStream()

            '
            '         * We will have to write the unencrypted bytes to the stream,
            '         * then read the encrypted result back from the stream.
            '         

            '#Region "Write the decrypted value to the encryption stream"
            Dim cs As New CryptoStream(memoryStream, EncryptorTransform, CryptoStreamMode.Write)
            cs.Write(bytes, 0, bytes.Length)
            cs.FlushFinalBlock()
            '#End Region

            '#Region "Read encrypted value back out of the stream"
            memoryStream.Position = 0
            Dim encrypted As Byte() = New Byte(memoryStream.Length - 1) {}
            memoryStream.Read(encrypted, 0, encrypted.Length)
            '#End Region

            'Clean up.
            cs.Close()
            memoryStream.Close()

            Return encrypted
        End Function

        ''' The other side: Decryption methods
        Public Function DecryptString(EncryptedString As String) As String
            Return Decrypt(StrToByteArray(EncryptedString))
        End Function

        ''' Decryption when working with byte arrays.    
        Public Function Decrypt(EncryptedValue As Byte()) As String
            '#Region "Write the encrypted value to the decryption stream"
            Dim encryptedStream As New MemoryStream()
            Dim decryptStream As New CryptoStream(encryptedStream, DecryptorTransform, CryptoStreamMode.Write)
            decryptStream.Write(EncryptedValue, 0, EncryptedValue.Length)
            decryptStream.FlushFinalBlock()
            '#End Region

            '#Region "Read the decrypted value from the stream."
            encryptedStream.Position = 0
            Dim decryptedBytes As [Byte]() = New [Byte](encryptedStream.Length - 1) {}
            encryptedStream.Read(decryptedBytes, 0, decryptedBytes.Length)
            encryptedStream.Close()
            '#End Region
            Return UTFEncoder.GetString(decryptedBytes)
        End Function

        ''' Convert a string to a byte array.  NOTE: Normally we'd create a Byte Array from a string using an ASCII encoding (like so).
        '      System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        '      return encoding.GetBytes(str);
        ' However, this results in character values that cannot be passed in a URL.  So, instead, I just
        ' lay out all of the byte values in a long string of numbers (three per - must pad numbers less than 100).
        Public Function StrToByteArray(str As String) As Byte()
            If str.Length = 0 Then
                Throw New Exception("Invalid string value in StrToByteArray")
            End If

            Dim val As Byte
            Dim byteArr As Byte() = New Byte(str.Length / 3 - 1) {}
            Dim i As Integer = 0
            Dim j As Integer = 0
            Do
                val = Byte.Parse(str.Substring(i, 3))
                byteArr(System.Math.Max(System.Threading.Interlocked.Increment(j), j - 1)) = val
                i += 3
            Loop While i < str.Length
            Return byteArr
        End Function

        ' Same comment as above.  Normally the conversion would use an ASCII encoding in the other direction:
        '      System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        '      return enc.GetString(byteArr);    
        Public Function ByteArrToString(byteArr As Byte()) As String
            Dim val As Byte
            Dim tempStr As String = ""
            For i As Integer = 0 To byteArr.GetUpperBound(0)
                val = byteArr(i)
                If val < CByte(10) Then
                    tempStr += "00" + val.ToString()
                ElseIf val < CByte(100) Then
                    tempStr += "0" + val.ToString()
                Else
                    tempStr += val.ToString()
                End If
            Next
            Return tempStr
        End Function

        Public Function GenerateMac(Path As String, SharedKey As String) As String
            Dim strMac As String = String.Empty
            Dim strEncrypt As String = Convert.ToString(Path & SharedKey)
            strMac = Sha256AddSecret(strEncrypt)

            Return strMac
        End Function

        Public Function Sha256AddSecret(strChange As String) As String
            'Change the syllable into UTF8 code
            Dim pass As Byte() = Encoding.UTF8.GetBytes(strChange)
            Dim sha256 As SHA256 = New SHA256CryptoServiceProvider()
            Dim hashValue As Byte() = sha256.ComputeHash(pass)

            Return BitConverter.ToString(hashValue).Replace("-", "").ToLower()
        End Function

    End Class
#End Region

#Region "CascadingComboBox"
    ''' <summary>
    ''' added by diana 20131227 - cascading combo list
    ''' </summary>
    ''' <remarks>determine parent, child, text and value field name, conn string and sql</remarks>
    Public Class CascadingComboBox
        Private _parent As String = String.Empty
        Private _parentValue As String = String.Empty
        Private _parentText As String = String.Empty
        Private _child As String = String.Empty
        Private _childValue As String = String.Empty
        Private _childText As String = String.Empty
        Private _connString As String = String.Empty
        Private _sql As String = String.Empty
        Private _selectType As Integer = 0

#Region "Public Properties"
        Public Property Parent() As String
            Get
                Return _parent
            End Get
            Set(value As String)
                _parent = value
            End Set
        End Property
        Public Property ParentValue() As String
            Get
                Return _parentValue
            End Get
            Set(value As String)
                _parentValue = value
            End Set
        End Property
        Public Property ParentText() As String
            Get
                Return _parentText
            End Get
            Set(value As String)
                _parentText = value
            End Set
        End Property
        Public Property Child() As String
            Get
                Return _child
            End Get
            Set(value As String)
                _child = value
            End Set
        End Property
        Public Property ChildValue() As String
            Get
                Return _childValue
            End Get
            Set(value As String)
                _childValue = value
            End Set
        End Property
        Public Property ChildText() As String
            Get
                Return _childText
            End Get
            Set(value As String)
                _childText = value
            End Set
        End Property
        Public Property ConnString() As String
            Get
                Return _connString
            End Get
            Set(value As String)
                _connString = value
            End Set
        End Property
        Public Property Sql() As String
            Get
                Return _sql
            End Get
            Set(value As String)
                _sql = value
            End Set
        End Property
        Public Property SelectType() As Integer
            Get
                Return _selectType
            End Get
            Set(value As Integer)
                _selectType = value
            End Set
        End Property
#End Region
    End Class
#End Region

#Region "AirBrake"
    Public Class SystemLog
        Protected _Notifier As AirbrakeNotifier

        ''' <summary>
        ''' Mandatory
        ''' </summary>
        Public Property Notifier As AirbrakeNotifier
            Get
                Return _Notifier
            End Get
            Set(ByVal Value As AirbrakeNotifier)
                _Notifier = Value
            End Set
        End Property

        Public Sub New()
            Config()
        End Sub

        Public Overloads Sub Config()
            Dim settings = ConfigurationSettings.AppSettings.AllKeys.Where(Function(key) key.StartsWith("Airbrake", StringComparison.OrdinalIgnoreCase)).ToDictionary(Function(key) key, Function(key) ConfigurationSettings.AppSettings(key))
            Dim airbrakeConfiguration = AirbrakeConfig.Load(settings)
            Notifier = New AirbrakeNotifier(airbrakeConfiguration)
        End Sub
    End Class

#End Region

End Namespace

