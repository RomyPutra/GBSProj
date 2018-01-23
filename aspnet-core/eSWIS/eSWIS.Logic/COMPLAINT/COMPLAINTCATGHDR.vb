Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Namespace COMPLAINTS
#Region "COMPLAINTCATGHDR Class"
    Public NotInheritable Class COMPLAINTCATGHDR
        Inherits Core.CoreControl
        Private ComplaintcatghdrInfo As ComplaintcatghdrInfo = New ComplaintcatghdrInfo

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal ComplaintcatghdrCont As Container.Complaintcatghdr, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If ComplaintcatghdrCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ComplaintcatghdrInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "CatgID = '" & ComplaintcatghdrCont.CatgID & "'")
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
                                .TableName = "Complaintcatghdr"
                                .AddField("CatgName", ComplaintcatghdrCont.CatgName, SQLControl.EnumDataType.dtStringN)
                                .AddField("CatgDesc", ComplaintcatghdrCont.CatgDesc, SQLControl.EnumDataType.dtStringN)
                                .AddField("Status", ComplaintcatghdrCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", ComplaintcatghdrCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", ComplaintcatghdrCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", ComplaintcatghdrCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", ComplaintcatghdrCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", ComplaintcatghdrCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", ComplaintcatghdrCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                '  .AddField("rowguid", ComplaintcatghdrCont.rowguid, SQLControl.EnumDataType.dtString)
                                .AddField("SyncCreate", ComplaintcatghdrCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("SyncLastUpd", ComplaintcatghdrCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                                .AddField("IsHost", ComplaintcatghdrCont.IsHost, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LastSyncBy", ComplaintcatghdrCont.LastSyncBy, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "CatgID = '" & ComplaintcatghdrCont.CatgID & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("CatgID", ComplaintcatghdrCont.CatgID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "CatgID = '" & ComplaintcatghdrCont.CatgID & "'")
                                End Select
                            End With
                            Try
                                If BatchExecute Then
                                    BatchList.Add(strSQL)
                                    If Commit Then
                                        objConn.BatchExecute(BatchList, CommandType.Text, True)
                                    End If
                                Else
                                    'execute
                                    objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                End If
                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                    Throw New ApplicationException("210002 " & axExecute.Message.ToString())
                                Else
                                    message = axExecute.Message.ToString()
                                    Throw New ApplicationException("210004 " & axExecute.Message.ToString())
                                End If
                            Finally
                                objSQL.Dispose()
                            End Try
                            Return True
                        End If

                    End If
                End If
            Catch axAssign As ApplicationException
                'Throw axAssign
                message = axAssign.Message.ToString()
                Return False
            Catch exAssign As SystemException
                'Throw exAssign
                message = exAssign.Message.ToString()
                Return False
            Finally
                ComplaintcatghdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal ComplaintcatghdrCont As Container.Complaintcatghdr, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(ComplaintcatghdrCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
        End Function

        'AMEND
        Public Function Update(ByVal ComplaintcatghdrCont As Container.Complaintcatghdr, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(ComplaintcatghdrCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        Public Function Delete(ByVal ComplaintcatghdrCont As Container.Complaintcatghdr, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If ComplaintcatghdrCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ComplaintcatghdrInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "CatgID = '" & ComplaintcatghdrCont.CatgID & "'")
                        End With
                        rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                    'Remark, Fatoni, 2017 03 06, For Delete manage inquiry change flag = 0
                                    'If Convert.ToInt16(.Item("InUse")) = 1 Then
                                    '    blnInUse = True
                                    'End If
                                End If
                                .Close()
                            End With
                        End If

                        If blnFound = True Then
                            With objSQL
                                strSQL = BuildUpdate(ComplaintcatghdrInfo.MyInfo.TableName, " SET Flag = 0" &
                                " , LastUpdate = '" & ComplaintcatghdrCont.LastUpdate & "' , UpdateBy = '" &
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, ComplaintcatghdrCont.UpdateBy) & "' WHERE" &
                                " CatgID = '" & ComplaintcatghdrCont.CatgID & "'")
                            End With
                        End If

                        'If blnFound = True And blnInUse = False Then
                        '    strSQL = BuildDelete(ComplaintcatghdrInfo.MyInfo.TableName, "CatgID = '" & ComplaintcatghdrCont.CatgID & "'")
                        'End If

                        Try
                            'execute
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Return False
                            'Throw New ApplicationException("210006 " & exExecute.Message.ToString())
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Return False
                'Throw axDelete
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Return False
                'Throw exDelete
            Finally
                ComplaintcatghdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetCOMPLAINTCATGHDR(ByVal CatgID As System.String) As Container.Complaintcatghdr
            Dim rComplaintcatghdr As Container.Complaintcatghdr = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With ComplaintcatghdrInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "CatgID = '" & CatgID & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rComplaintcatghdr = New Container.Complaintcatghdr
                                rComplaintcatghdr.CatgID = drRow.Item("CatgID")
                                rComplaintcatghdr.CatgName = drRow.Item("CatgName")
                                rComplaintcatghdr.CatgDesc = drRow.Item("CatgDesc")
                                rComplaintcatghdr.Status = drRow.Item("Status")
                                If Not IsDBNull(drRow.Item("CreateDate")) Then
                                    rComplaintcatghdr.CreateDate = drRow.Item("CreateDate")
                                End If
                                rComplaintcatghdr.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rComplaintcatghdr.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rComplaintcatghdr.UpdateBy = drRow.Item("UpdateBy")
                                rComplaintcatghdr.Active = drRow.Item("Active")
                                rComplaintcatghdr.Inuse = drRow.Item("Inuse")
                                rComplaintcatghdr.rowguid = drRow.Item("rowguid")
                                rComplaintcatghdr.SyncCreate = drRow.Item("SyncCreate")
                                rComplaintcatghdr.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rComplaintcatghdr.IsHost = drRow.Item("IsHost")
                                rComplaintcatghdr.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rComplaintcatghdr = Nothing
                            End If
                        Else
                            rComplaintcatghdr = Nothing
                        End If
                    End With
                End If
                Return rComplaintcatghdr
            Catch ex As Exception
                Throw ex
            Finally
                rComplaintcatghdr = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetCOMPLAINTCATGHDR(ByVal CatgID As System.String, DecendingOrder As Boolean) As List(Of Container.Complaintcatghdr)
            Dim rComplaintcatghdr As Container.Complaintcatghdr = Nothing
            Dim lstComplaintcatghdr As List(Of Container.Complaintcatghdr) = New List(Of Container.Complaintcatghdr)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With ComplaintcatghdrInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by CatgID DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "CatgID = '" & CatgID & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rComplaintcatghdr = New Container.Complaintcatghdr
                                rComplaintcatghdr.CatgID = drRow.Item("CatgID")
                                rComplaintcatghdr.CatgName = drRow.Item("CatgName")
                                rComplaintcatghdr.CatgDesc = drRow.Item("CatgDesc")
                                rComplaintcatghdr.Status = drRow.Item("Status")
                                rComplaintcatghdr.CreateBy = drRow.Item("CreateBy")
                                rComplaintcatghdr.UpdateBy = drRow.Item("UpdateBy")
                                rComplaintcatghdr.Active = drRow.Item("Active")
                                rComplaintcatghdr.Inuse = drRow.Item("Inuse")
                                rComplaintcatghdr.rowguid = drRow.Item("rowguid")
                                rComplaintcatghdr.SyncCreate = drRow.Item("SyncCreate")
                                rComplaintcatghdr.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rComplaintcatghdr.IsHost = drRow.Item("IsHost")
                                rComplaintcatghdr.LastSyncBy = drRow.Item("LastSyncBy")
                                lstComplaintcatghdr.Add(rComplaintcatghdr)
                            Next

                        Else
                            rComplaintcatghdr = Nothing
                        End If
                        Return lstComplaintcatghdr
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rComplaintcatghdr = Nothing
                lstComplaintcatghdr = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function ManageInquiryList(ByVal inactive As String, Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ComplaintcatghdrInfo.MyInfo
                    If SQL = Nothing Or SQL = String.Empty Then
                        strSQL = "SELECT CatgID, CatgName, CatgDesc, CreateBy, CreateDate, UpdateBy, LastUpdate, CASE WHEN CH.Active=1 THEN 'Active' ELSE 'In-Active' END as Status, CodeDesc as CatgNameDesc " &
                                " FROM COMPLAINTCATGHDR CH WITH(NOLOCK) " &
                                " INNER JOIN CODEMASTER CM WITH(NOLOCK) ON CH.CatgName=CM.Code AND CM.CodeType='FAQ' " &
                                " WHERE Flag=1 AND CH.Active = '" & inactive & "'"
                    Else
                        strSQL = BuildSelect(.FieldsList, .TableName, FieldCond)
                    End If
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetCOMPLAINTCATGHDRList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ComplaintcatghdrInfo.MyInfo
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

        Public Overloads Function GetCOMPLAINTCATGHDRShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With ComplaintcatghdrInfo.MyInfo
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
#End Region

#Region "Container"
    Namespace Container
#Region "Complaintcatghdr Container"
        Public Class Complaintcatghdr_FieldName
            Public CatgID As System.String = "CatgID"
            Public CatgName As System.String = "CatgName"
            Public CatgDesc As System.String = "CatgDesc"
            Public Status As System.String = "Status"
            Public CreateDate As System.String = "CreateDate"
            Public CreateBy As System.String = "CreateBy"
            Public LastUpdate As System.String = "LastUpdate"
            Public UpdateBy As System.String = "UpdateBy"
            Public Active As System.String = "Active"
            Public Inuse As System.String = "Inuse"
            Public Flag As System.String = "Flag"
            Public rowguid As System.String = "rowguid"
            Public SyncCreate As System.String = "SyncCreate"
            Public SyncLastUpd As System.String = "SyncLastUpd"
            Public IsHost As System.String = "IsHost"
            Public LastSyncBy As System.String = "LastSyncBy"
        End Class

        Public Class Complaintcatghdr
            Protected _CatgID As System.String
            Private _CatgName As System.String
            Private _CatgDesc As System.String
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

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property CatgID As System.String
                Get
                    Return _CatgID
                End Get
                Set(ByVal Value As System.String)
                    _CatgID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CatgName As System.String
                Get
                    Return _CatgName
                End Get
                Set(ByVal Value As System.String)
                    _CatgName = Value
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

        End Class
#End Region

    End Namespace
#End Region

#Region "Class Info"
#Region "Complaintcatghdr Info"
    Public Class ComplaintcatghdrInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "CatgID,CatgName,CatgDesc,Status,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
                .CheckFields = "Status,Active,Inuse,Flag,IsHost"
                .TableName = "Complaintcatghdr"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "CatgID,CatgName,CatgDesc,Status,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
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
#End Region

#Region "Scheme"
#Region "COMPLAINTCATGHDR Scheme"
    Public Class COMPLAINTCATGHDRScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CatgID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "CatgName"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "CatgDesc"
                .Length = 200
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

        Public ReadOnly Property CatgID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property CatgName As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property CatgDesc As StrucElement
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
#End Region

End Namespace