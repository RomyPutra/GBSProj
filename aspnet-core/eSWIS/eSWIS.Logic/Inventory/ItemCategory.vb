Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General


Namespace Inventory
    Public NotInheritable Class ItemCategory
        Inherits Core.CoreControl
        Private ItemCategoryInfo As ItemCategoryInfo = New ItemCategoryInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"

        Private Function Save(ByVal ItemCategoryCont As Container.ItemCategory, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If ItemCategoryCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ItemCategoryInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "CatgCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCategoryCont.CatgCode) & "'")
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
                            Return False
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = "ITEMCATEGORY WITH (ROWLOCK)"
                                .AddField("CatgDesc", ItemCategoryCont.CatgDesc, SQLControl.EnumDataType.dtStringN)
                                .AddField("SalesTarget", ItemCategoryCont.SalesTarget, SQLControl.EnumDataType.dtNumeric)
                                .AddField("DesireStock", ItemCategoryCont.DesireStock, SQLControl.EnumDataType.dtNumeric)
                                .AddField("DualReceipt", ItemCategoryCont.DualReceipt, SQLControl.EnumDataType.dtNumeric)
                                .AddField("PrintLineId", ItemCategoryCont.PrintLineId, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", ItemCategoryCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", ItemCategoryCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", ItemCategoryCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", ItemCategoryCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", ItemCategoryCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", ItemCategoryCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsHost", ItemCategoryCont.IsHost, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", ItemCategoryCont.Flag, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "CatgCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCategoryCont.CatgCode) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("CatgCode", ItemCategoryCont.CatgCode, SQLControl.EnumDataType.dtString)
                                                .AddField("DeptCode", ItemCategoryCont.DeptCode, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "CatgCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCategoryCont.CatgCode) & "'")
                                End Select
                            End With
                            Try
                                objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                Return True

                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    Throw New ApplicationException("210002 " & axExecute.Message.ToString())
                                Else
                                    message = axExecute.Message.ToString()
                                End If
                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic/Inventory/ItemCategory", axExecute.Message & strSQL, axExecute.StackTrace)
                                Return False

                            Finally
                                objSQL.Dispose()
                            End Try
                        End If

                    End If
                End If
            Catch axAssign As ApplicationException
                message = message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Inventory/ItemCategory", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Inventory/ItemCategory", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                ItemCategoryCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function Insert(ByVal ItemCategoryCont As Container.ItemCategory, ByRef message As String) As Boolean
            Return Save(ItemCategoryCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        Public Function Update(ByVal ItemCategoryCont As Container.ItemCategory, ByRef message As String) As Boolean
            Return Save(ItemCategoryCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal ItemCategoryCont As Container.ItemCategory, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If ItemCategoryCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ItemCategoryInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "CatgCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCategoryCont.CatgCode) & "'")
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
                                strSQL = BuildUpdate("ITEMCATEGORY WITH (ROWLOCK)", " SET Flag = 0" & _
                                " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCategoryCont.UpdateBy) & "' WHERE" & _
                                " CatgCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCategoryCont.CatgCode) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("ITEMCATEGORY WITH (ROWLOCK)", "CatgCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCategoryCont.CatgCode) & "'")
                        End If

                        Try
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/Inventory/ItemCategory", exExecute.Message & strSQL, exExecute.StackTrace)
                            Return False
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Inventory/ItemCategory", axDelete.Message, axDelete.StackTrace)
                Return False
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Inventory/ItemCategory", exDelete.Message, exDelete.StackTrace)
                Return False
            Finally
                ItemCategoryCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetItemCategory(ByVal ItemCategoryCode As System.String) As Container.ItemCategory
            Dim rItemCategory As Container.ItemCategory = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With ItemCategoryInfo.MyInfo
                        StartSQLControl()
                        strSQL = BuildSelect(.FieldsList, .TableName, "CatgCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCategoryCode) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rItemCategory = New Container.ItemCategory
                                rItemCategory.DeptCode = drRow.Item("DeptCode")
                                rItemCategory.CatgCode = drRow.Item("CatgCode")
                                rItemCategory.CatgDesc = drRow.Item("CatgDesc")
                                rItemCategory.SalesTarget = drRow.Item("SalesTarget")
                                rItemCategory.DesireStock = drRow.Item("DesireStock")
                                rItemCategory.DualReceipt = drRow.Item("DualReceipt")
                                rItemCategory.PrintLineId = drRow.Item("PrintLineId")
                                rItemCategory.CreateBy = drRow.Item("CreateBy")
                                rItemCategory.UpdateBy = drRow.Item("UpdateBy")
                                rItemCategory.Active = drRow.Item("Active")
                                rItemCategory.Inuse = drRow.Item("Inuse")
                                rItemCategory.rowguid = drRow.Item("rowguid")
                                rItemCategory.SyncCreate = drRow.Item("SyncCreate")
                                rItemCategory.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rItemCategory.IsHost = drRow.Item("IsHost")
                                rItemCategory.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rItemCategory = Nothing
                            End If
                        Else
                            rItemCategory = Nothing
                        End If
                    End With
                End If
                Return rItemCategory
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Inventory/ItemCategory", ex.Message, ex.StackTrace)
            Finally
                rItemCategory = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetItemCategory(ByVal ItemCategoryCode As System.String, ByVal DecendingOrder As Boolean) As List(Of Container.ItemCategory)
            Dim rItemCategory As Container.ItemCategory = Nothing
            Dim lstItemCategory As List(Of Container.ItemCategory) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With ItemCategoryInfo.MyInfo
                        StartSQLControl()
                        If DecendingOrder Then
                            strDesc = " Order by ByVal CatgCode As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "CatgCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ItemCategoryCode) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rItemCategory = New Container.ItemCategory
                                rItemCategory.DeptCode = drRow.Item("DeptCode")
                                rItemCategory.CatgCode = drRow.Item("CatgCode")
                                rItemCategory.CatgDesc = drRow.Item("CatgDesc")
                                rItemCategory.SalesTarget = drRow.Item("SalesTarget")
                                rItemCategory.DesireStock = drRow.Item("DesireStock")
                                rItemCategory.DualReceipt = drRow.Item("DualReceipt")
                                rItemCategory.PrintLineId = drRow.Item("PrintLineId")
                                rItemCategory.CreateBy = drRow.Item("CreateBy")
                                rItemCategory.UpdateBy = drRow.Item("UpdateBy")
                                rItemCategory.Active = drRow.Item("Active")
                                rItemCategory.Inuse = drRow.Item("Inuse")
                                rItemCategory.rowguid = drRow.Item("rowguid")
                                rItemCategory.SyncCreate = drRow.Item("SyncCreate")
                                rItemCategory.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rItemCategory.IsHost = drRow.Item("IsHost")
                                rItemCategory.LastSyncBy = drRow.Item("LastSyncBy")
                            Next
                            lstItemCategory.Add(rItemCategory)
                        Else
                            rItemCategory = Nothing
                        End If
                        Return lstItemCategory
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Inventory/ItemCategory", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rItemCategory = Nothing
                lstItemCategory = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetItemCategoryList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ItemCategoryInfo.MyInfo
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

        Public Overloads Function GetItemCategoryList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With ItemCategoryInfo.MyInfo
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
        Public Class ItemCategory

            Public fDeptCode As System.String = "DeptCode"
            Public fCatgCode As System.String = "CatgCode"
            Public fCatgDesc As System.String = "CatgDesc"
            Public fSalesTarget As System.String = "SalesTarget"
            Public fDesireStock As System.String = "DesireStock"
            Public fDualReceipt As System.String = "DualReceipt"
            Public fPrintLineId As System.String = "PrintLineId"
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

            Protected _DeptCode As System.String
            Private _CatgCode As System.String
            Private _CatgDesc As System.String
            Private _SalesTarget As System.Byte
            Private _DesireStock As System.Byte
            Private _DualReceipt As System.Byte
            Private _PrintLineId As System.Byte
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
            Public Property DeptCode As System.String
                Get
                    Return _DeptCode
                End Get
                Set(ByVal Value As System.String)
                    _DeptCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CatgCode As System.String
                Get
                    Return _CatgCode
                End Get
                Set(ByVal Value As System.String)
                    _CatgCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CatgDesc As System.String
                Get
                    Return _CatgDesc
                End Get
                Set(ByVal Value As System.String)
                    _CatgDesc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property SalesTarget As System.Byte
                Get
                    Return _SalesTarget
                End Get
                Set(ByVal Value As System.Byte)
                    _SalesTarget = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property DesireStock As System.Byte
                Get
                    Return _DesireStock
                End Get
                Set(ByVal Value As System.Byte)
                    _DesireStock = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property DualReceipt As System.Byte
                Get
                    Return _DualReceipt
                End Get
                Set(ByVal Value As System.Byte)
                    _DualReceipt = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PrintLineId As System.Byte
                Get
                    Return _PrintLineId
                End Get
                Set(ByVal Value As System.Byte)
                    _PrintLineId = Value
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
    Public Class ItemCategoryInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "DeptCode,CatgCode,CatgDesc,SalesTarget,DesireStock,DualReceipt,PrintLineId,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
                .CheckFields = "Active,Inuse,Flag,IsHost"
                .TableName = "ITEMCATEGORY WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "DeptCode,CatgCode,CatgDesc,SalesTarget,DesireStock,DualReceipt,PrintLineId,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
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
    Public Class ItemCategoryScheme
        Inherits Core.SchemeBase

        Protected Overrides Sub InitializeInfo()
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "DeptCode"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CatgCode"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "CatgDesc"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "SalesTarget"
                .Length = 12
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "DesireStock"
                .Length = 12
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "DualReceipt"
                .Length = 2
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "PrintLineId"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Inuse"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsHost"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)


        End Sub

        Public ReadOnly Property DeptCode As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property CatgCode As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property CatgDesc As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property SalesTarget As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property DesireStock As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property DualReceipt As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property PrintLineId As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property Inuse As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property IsHost As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region

End Namespace

