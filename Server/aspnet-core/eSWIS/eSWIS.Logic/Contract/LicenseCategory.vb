Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Contract
    Public NotInheritable Class LicenseCategory
        Inherits Core.CoreControl
        Private LicenseCategoryInfo As LicenseCategoryInfo = New LicenseCategoryInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"

        Private Function Save(ByVal LicenseCategoryCont As Container.LicenseCategory, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If LicenseCategoryCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With LicenseCategoryInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "LicenseDesc = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LicenseCategoryCont.LicenseDesc) & "' AND LicenseType='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LicenseCategoryCont.LicenseType) & "'")
                        End With
                        rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)
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
                            message = "Record already exist"
                            'Throw New ApplicationException("210011: Record already exist")
                            'Item found & active
                            'system.Windows.Forms.MessageBox.Show("ab", "ABC",Windows.Forms.MessageBoxButtons.YesNo, Windows.Forms.MessageBoxIconInformation,Windows.Forms.MessageBoxDefaultButton.Button2, Windows.Forms.MessageBoxOptions.DefaultDesktopOnly
                            'MsgBox("Record already exist", MsgBoxStyle.Critical, "Record Exist")
                            Return False
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = "LICENSECATEGORY WITH (ROWLOCK)"
                                .AddField("LicenseDesc", LicenseCategoryCont.LicenseDesc, SQLControl.EnumDataType.dtString)
                                .AddField("LicenseType", LicenseCategoryCont.LicenseType, SQLControl.EnumDataType.dtString)
                                .AddField("Status", LicenseCategoryCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", LicenseCategoryCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("LastUpdate", LicenseCategoryCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", LicenseCategoryCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", LicenseCategoryCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", LicenseCategoryCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsHost", LicenseCategoryCont.IsHost, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", LicenseCategoryCont.Flag, SQLControl.EnumDataType.dtNumeric)


                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            .AddField("CreateBy", LicenseCategoryCont.CreateBy, SQLControl.EnumDataType.dtString)
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LicenseCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LicenseCategoryCont.LicenseCode) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("CreateBy", LicenseCategoryCont.CreateBy, SQLControl.EnumDataType.dtString)
                                                .AddField("LicenseCode", LicenseCategoryCont.LicenseCode, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "LicenseCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LicenseCategoryCont.LicenseCode) & "'")
                                End Select
                            End With
                            Try
                                'execute
                                objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                Return True

                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                    'Throw New ApplicationException("210002 " & axExecute.Message.ToString())
                                Else
                                    message = axExecute.Message.ToString()
                                    'Throw New ApplicationException("210004 " & axExecute.Message.ToString())
                                End If
                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic/Contract/LicenseCategory", axExecute.Message & strSQL, axExecute.StackTrace)
                                Return False

                            Finally
                                objSQL.Dispose()
                            End Try
                        End If

                    End If
                End If
            Catch axAssign As ApplicationException
                'Throw axAssign
                message = message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Contract/LicenseCategory", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Contract/LicenseCategory", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                LicenseCategoryCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function Insert(ByVal LicenseCategoryCont As Container.LicenseCategory, ByRef message As String) As Boolean
            Return Save(LicenseCategoryCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        Public Function Update(ByVal LicenseCategoryCont As Container.LicenseCategory, ByRef message As String) As Boolean
            Return Save(LicenseCategoryCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal LicenseCategoryCont As Container.LicenseCategory, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If LicenseCategoryCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With LicenseCategoryInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "LicenseCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LicenseCategoryCont.LicenseCode) & "'")
                        End With
                        rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)

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

                        If blnFound = True And blnInUse = True Then
                            With objSQL
                                strSQL = BuildUpdate("LICENSECATEGORY WITH (ROWLOCK)", " SET Flag = 0" & _
                                " , LastUpdate = '" & LicenseCategoryCont.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, LicenseCategoryCont.UpdateBy) & "' WHERE " & _
                                " LicenseCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LicenseCategoryCont.LicenseCode) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("LICENSECATEGORY WITH (ROWLOCK)", "LicenseCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LicenseCategoryCont.LicenseCode) & "'")
                        End If

                        Try
                            'execute
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/Contract/LicenseCategory", exExecute.Message & strSQL, exExecute.StackTrace)
                            Return False
                            'Throw New ApplicationException("210006 " & exExecute.Message.ToString())
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Contract/LicenseCategory", axDelete.Message, axDelete.StackTrace)
                Return False
                'Throw axDelete
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Contract/LicenseCategory", exDelete.Message, exDelete.StackTrace)
                Return False
                'Throw exDelete
            Finally
                LicenseCategoryCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetLicenseCategory(ByVal LicenseCategoryCode As System.String) As Container.LicenseCategory
            Dim rLicenseCategory As Container.LicenseCategory = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With LicenseCategoryInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "LicenseCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LicenseCategoryCode) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rLicenseCategory = New Container.LicenseCategory
                                rLicenseCategory.LicenseCode = drRow.Item("LicenseCode")
                                rLicenseCategory.LicenseDesc = drRow.Item("LicenseDesc")
                                rLicenseCategory.LicenseType = drRow.Item("LicenseType")
                                rLicenseCategory.Status = drRow.Item("Status")
                                rLicenseCategory.CreateBy = drRow.Item("CreateBy")
                                rLicenseCategory.UpdateBy = IIf(IsDBNull(drRow.Item("UpdateBy")), "", drRow.Item("UpdateBy"))
                                rLicenseCategory.Active = drRow.Item("Active")
                                rLicenseCategory.Inuse = drRow.Item("Inuse")
                                rLicenseCategory.rowguid = drRow.Item("rowguid")
                                rLicenseCategory.SyncCreate = drRow.Item("SyncCreate")
                                rLicenseCategory.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rLicenseCategory.IsHost = drRow.Item("IsHost")
                                rLicenseCategory.LastSyncBy = drRow.Item("LastSyncBy")
                                rLicenseCategory.CategoryDesc = drRow.Item("CategoryDesc")

                            Else
                                rLicenseCategory = Nothing
                            End If
                        Else
                            rLicenseCategory = Nothing
                        End If
                    End With
                End If
                Return rLicenseCategory
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Contract/LicenseCategory", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rLicenseCategory = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetLicenseCategory(ByVal LicenseCategory As System.String, DecendingOrder As Boolean) As List(Of Container.LicenseCategory)
            Dim rLicenseCategory As Container.LicenseCategory = Nothing
            Dim lstCountry As List(Of Container.LicenseCategory) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With LicenseCategoryInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal CountryCode As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "LicenseCode = '" & LicenseCategory & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rLicenseCategory = New Container.LicenseCategory
                                rLicenseCategory.LicenseCode = drRow.Item("LicenseCode")
                                rLicenseCategory.LicenseDesc = drRow.Item("LicenseDesc")
                                rLicenseCategory.LicenseType = drRow.Item("LicenseType")
                                rLicenseCategory.Status = drRow.Item("Status")
                                rLicenseCategory.CreateBy = drRow.Item("CreateBy")
                                rLicenseCategory.UpdateBy = IIf(IsDBNull(drRow.Item("UpdateBy")), "", drRow.Item("UpdateBy"))
                                rLicenseCategory.Active = drRow.Item("Active")
                                rLicenseCategory.Inuse = drRow.Item("Inuse")
                                rLicenseCategory.rowguid = drRow.Item("rowguid")
                                rLicenseCategory.SyncCreate = drRow.Item("SyncCreate")
                                rLicenseCategory.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rLicenseCategory.IsHost = drRow.Item("IsHost")
                                rLicenseCategory.LastSyncBy = drRow.Item("LastSyncBy")
                                rLicenseCategory.CategoryDesc = drRow.Item("CategoryDesc")

                            Next
                            lstCountry.Add(rLicenseCategory)
                        Else
                            rLicenseCategory = Nothing
                        End If
                        Return lstCountry
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Contract/LicenseCategory", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rLicenseCategory = Nothing
                lstCountry = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetLicenseCategoryList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With LicenseCategoryInfo.MyInfo
                    If SQL = Nothing Or SQL = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName, FieldCond)
                    Else
                        strSQL = SQL
                    End If
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetLicenseCategoryShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With LicenseCategoryInfo.MyInfo
                    If ShortListing Then
                        strSQL = BuildSelect(.ShortList, .TableName, .ShortListCond)
                    Else
                        strSQL = BuildSelect(.Listing, .TableName, .ListingCond)
                    End If
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function
#End Region

    End Class

    Namespace Container
#Region "Class Container"
        Public Class LicenseCategory
            Public fLicenseCode As System.String = "LicenseCode"
            Public fLicenseDesc As System.String = "LicenseDesc"
            Public fLicenseType As System.String = "LicenseType"
            Public fCategoryDesc As System.String = "CategoryDesc"
            Public fStatus As System.String = "Status"
            Public fCreateDate As System.String = "CreateDate"
            Public fCreateBy As System.String = "CreateBy"
            Public fLastUpdate As System.String = "LastUpdate"
            Public fUpdateBy As System.String = "UpdateBy"
            Public fActive As System.String = "Active"
            Public fInuse As System.String = "Inuse"
            Public fFlag As System.String = "Flag"
            Public frowguid As System.String = "rowguid"
            Public fSyncCreate As System.String = "SyncCreate"
            Public fSyncLastUpd As System.String = "SyncLastUpd"
            Public fIsHost As System.String = "IsHost"
            Public fLastSyncBy As System.String = "LastSyncBy"

            Protected _LicenseCode As System.String
            Private _LicenseDesc As System.String
            Private _LicenseType As System.String
            Private _CategoryDesc As System.String
            Private _Status As System.Byte
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _Active As System.Byte
            Private _Inuse As System.Byte
            Private _rowguid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _IsHost As System.Byte
            Private _LastSyncBy As System.String
            Private _Flag As System.Byte


            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property LicenseCode As System.String
                Get
                    Return _LicenseCode
                End Get
                Set(ByVal Value As System.String)
                    _LicenseCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property LicenseDesc As System.String
                Get
                    Return _LicenseDesc
                End Get
                Set(ByVal Value As System.String)
                    _LicenseDesc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property LicenseType As System.String
                Get
                    Return _LicenseType
                End Get
                Set(ByVal Value As System.String)
                    _LicenseType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CategoryDesc As System.String
                Get
                    Return _CategoryDesc
                End Get
                Set(ByVal Value As System.String)
                    _CategoryDesc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Status As System.Byte
                Get
                    Return _Status
                End Get
                Set(ByVal Value As System.Byte)
                    _Status = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property CreateDate As System.DateTime
                Get
                    Return _CreateDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _CreateDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CreateBy As System.String
                Get
                    Return _CreateBy
                End Get
                Set(ByVal Value As System.String)
                    _CreateBy = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property LastUpdate As System.DateTime
                Get
                    Return _LastUpdate
                End Get
                Set(ByVal Value As System.DateTime)
                    _LastUpdate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property UpdateBy As System.String
                Get
                    Return _UpdateBy
                End Get
                Set(ByVal Value As System.String)
                    _UpdateBy = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Active As System.Byte
                Get
                    Return _Active
                End Get
                Set(ByVal Value As System.Byte)
                    _Active = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Inuse As System.Byte
                Get
                    Return _Inuse
                End Get
                Set(ByVal Value As System.Byte)
                    _Inuse = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property rowguid As System.Guid
                Get
                    Return _rowguid
                End Get
                Set(ByVal Value As System.Guid)
                    _rowguid = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property SyncCreate As System.DateTime
                Get
                    Return _SyncCreate
                End Get
                Set(ByVal Value As System.DateTime)
                    _SyncCreate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property SyncLastUpd As System.DateTime
                Get
                    Return _SyncLastUpd
                End Get
                Set(ByVal Value As System.DateTime)
                    _SyncLastUpd = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property IsHost As System.Byte
                Get
                    Return _IsHost
                End Get
                Set(ByVal Value As System.Byte)
                    _IsHost = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property LastSyncBy As System.String
                Get
                    Return _LastSyncBy
                End Get
                Set(ByVal Value As System.String)
                    _LastSyncBy = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Flag As System.Byte
                Get
                    Return _Flag
                End Get
                Set(ByVal Value As System.Byte)
                    _Flag = Value
                End Set
            End Property
        End Class
#End Region
    End Namespace


#Region "Class Info"
    Public Class LicenseCategoryInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "LicenseCode,LicenseDesc,LicenseType,(SELECT CodeDesc FROM CODEMASTER WITH (NOLOCK) WHERE Code=LICENSECATEGORY.LicenseType) AS CategoryDesc,Status,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
                .CheckFields = "Status,Active,Inuse,Flag,IsHost"
                .TableName = "LICENSECATEGORY WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "LicenseCode,LicenseDesc,LicenseType,Status,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
                .ListingCond = Nothing
                .ShortList = Nothing
                .ShortListCond = Nothing
            End With
        End Sub

        Public Function JoinTableField(ByVal Prefix As String, ByVal FieldList As String) As String
            Dim aFieldList As Array
            Dim strFieldList As String = Nothing
            aFieldList = FieldList.Split(",")
            If Not aFieldList Is Nothing Then
                For Each Str As String In aFieldList
                    If strFieldList Is Nothing Then
                        strFieldList = Prefix & "." & Str
                    Else
                        strFieldList &= "," & Prefix & "." & Str
                    End If
                Next
            End If
            aFieldList = Nothing

            Return strFieldList
        End Function
    End Class
#End Region


#Region "Scheme"
    Public Class LicenseCategoryScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LicenseCode"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "LicenseDesc"
                .Length = 100
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LicenseType"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Status"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
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
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
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
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Inuse"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsHost"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)

        End Sub

        Public ReadOnly Property LicenseCode As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property LicenseDesc As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property LicenseType As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property Status As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property Inuse As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property IsHost As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region
End Namespace

