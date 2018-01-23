Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared

Namespace COMPLAINTS
#Region "COMPLAINTCATGDTL Class"
    Public NotInheritable Class COMPLAINTCATGDTL
        Inherits Core.CoreControl
        Private ComplaintcatgdtlInfo As ComplaintcatgdtlInfo = New ComplaintcatgdtlInfo

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal ComplaintcatgdtlCont As Container.Complaintcatgdtl, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If ComplaintcatgdtlCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ComplaintcatgdtlInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "CatgID = '" & ComplaintcatgdtlCont.CatgID & "' AND SeqNo = '" & ComplaintcatgdtlCont.SeqNo & "'")
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
                                .TableName = "Complaintcatgdtl"
                                .AddField("UserID", ComplaintcatgdtlCont.UserID, SQLControl.EnumDataType.dtString)
                                .AddField("RefID", ComplaintcatgdtlCont.RefID, SQLControl.EnumDataType.dtString)
                                .AddField("FirstName", ComplaintcatgdtlCont.FirstName, SQLControl.EnumDataType.dtStringN)
                                .AddField("LastName", ComplaintcatgdtlCont.LastName, SQLControl.EnumDataType.dtStringN)
                                .AddField("Country", ComplaintcatgdtlCont.Country, SQLControl.EnumDataType.dtString)
                                .AddField("State", ComplaintcatgdtlCont.State, SQLControl.EnumDataType.dtString)
                                .AddField("Email", ComplaintcatgdtlCont.Email, SQLControl.EnumDataType.dtStringN)
                                .AddField("Status", ComplaintcatgdtlCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", ComplaintcatgdtlCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", ComplaintcatgdtlCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", ComplaintcatgdtlCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", ComplaintcatgdtlCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", ComplaintcatgdtlCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", ComplaintcatgdtlCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("SyncCreate", ComplaintcatgdtlCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("SyncLastUpd", ComplaintcatgdtlCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                                .AddField("IsHost", ComplaintcatgdtlCont.IsHost, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LastSyncBy", ComplaintcatgdtlCont.LastSyncBy, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "CatgID = '" & ComplaintcatgdtlCont.CatgID & "' AND SeqNo = '" & ComplaintcatgdtlCont.SeqNo & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("CatgID", ComplaintcatgdtlCont.CatgID, SQLControl.EnumDataType.dtString)
                                                .AddField("SeqNo", ComplaintcatgdtlCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "CatgID = '" & ComplaintcatgdtlCont.CatgID & "' AND SeqNo = '" & ComplaintcatgdtlCont.SeqNo & "'")
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
                message = axAssign.Message.ToString()
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Return False
            Finally
                ComplaintcatgdtlCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Private Function SaveBatch(ByVal ListComplaintcatgdtlCont As List(Of Container.Complaintcatgdtl), ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim rdr As System.Data.SqlClient.SqlDataReader
            SaveBatch = False
            Try
                If ListComplaintcatgdtlCont Is Nothing OrElse ListComplaintcatgdtlCont.Count = 0 Then

                    Return False
                Else

                    StartSQLControl()

                    With objSQL
                        .TableName = "COMPLAINTCATGDTL WITH(ROWLOCK)"
                        strSQL = BuildDelete(.TableName, "CatgID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ListComplaintcatgdtlCont(0).CatgID) & "'")
                        BatchList.Add(strSQL)
                    End With

                    For Each ComplaintcatgdtlCont In ListComplaintcatgdtlCont
                        With objSQL
                            .TableName = "COMPLAINTCATGDTL WITH(ROWLOCK)"
                            .AddField("UserID", ComplaintcatgdtlCont.UserID, SQLControl.EnumDataType.dtString)
                            .AddField("RefID", ComplaintcatgdtlCont.RefID, SQLControl.EnumDataType.dtString)
                            .AddField("FirstName", ComplaintcatgdtlCont.FirstName, SQLControl.EnumDataType.dtStringN)
                            .AddField("LastName", ComplaintcatgdtlCont.LastName, SQLControl.EnumDataType.dtStringN)
                            .AddField("Country", ComplaintcatgdtlCont.Country, SQLControl.EnumDataType.dtString)
                            .AddField("State", ComplaintcatgdtlCont.State, SQLControl.EnumDataType.dtString)
                            .AddField("Email", ComplaintcatgdtlCont.Email, SQLControl.EnumDataType.dtStringN)
                            .AddField("Status", ComplaintcatgdtlCont.Status, SQLControl.EnumDataType.dtNumeric)
                            .AddField("CreateDate", ComplaintcatgdtlCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("CreateBy", ComplaintcatgdtlCont.CreateBy, SQLControl.EnumDataType.dtString)
                            .AddField("LastUpdate", ComplaintcatgdtlCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("UpdateBy", ComplaintcatgdtlCont.UpdateBy, SQLControl.EnumDataType.dtString)
                            .AddField("Active", ComplaintcatgdtlCont.Active, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Inuse", ComplaintcatgdtlCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                            .AddField("SyncCreate", ComplaintcatgdtlCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("SyncLastUpd", ComplaintcatgdtlCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                            .AddField("IsHost", ComplaintcatgdtlCont.IsHost, SQLControl.EnumDataType.dtNumeric)
                            .AddField("LastSyncBy", ComplaintcatgdtlCont.LastSyncBy, SQLControl.EnumDataType.dtString)
                            .AddField("CatgID", ComplaintcatgdtlCont.CatgID, SQLControl.EnumDataType.dtString)
                            .AddField("SeqNo", ComplaintcatgdtlCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                        End With
                        BatchList.Add(strSQL)

                    Next


                    Try
                        If BatchExecute Then
                            If Commit Then
                                objConn.BatchExecute(BatchList, CommandType.Text, True)
                            End If
                        Else
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

            Catch axAssign As ApplicationException
                message = axAssign.Message.ToString()
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Return False
            Finally
                ListComplaintcatgdtlCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal ComplaintcatgdtlCont As Container.Complaintcatgdtl, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(ComplaintcatgdtlCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
        End Function

        Public Function InsertBatch(ByVal ListComplaintcatgdtlCont As List(Of Container.Complaintcatgdtl), ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return SaveBatch(ListComplaintcatgdtlCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
        End Function

        'AMEND
        Public Function Update(ByVal ComplaintcatgdtlCont As Container.Complaintcatgdtl, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(ComplaintcatgdtlCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        Public Function Delete(ByVal ComplaintcatgdtlCont As Container.Complaintcatgdtl, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If ComplaintcatgdtlCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ComplaintcatgdtlInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "CatgID = '" & ComplaintcatgdtlCont.CatgID & "' AND SeqNo = '" & ComplaintcatgdtlCont.SeqNo & "'")
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
                                strSQL = BuildUpdate(ComplaintcatgdtlInfo.MyInfo.TableName, " SET Flag = 0" &
                                " , LastUpdate = '" & ComplaintcatgdtlCont.LastUpdate & "' , UpdateBy = '" &
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, ComplaintcatgdtlCont.UpdateBy) & "' WHERE" &
                                "CatgID = '" & ComplaintcatgdtlCont.CatgID & "' AND SeqNo = '" & ComplaintcatgdtlCont.SeqNo & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(ComplaintcatgdtlInfo.MyInfo.TableName, "CatgID = '" & ComplaintcatgdtlCont.CatgID & "' AND SeqNo = '" & ComplaintcatgdtlCont.SeqNo & "'")
                        End If

                        Try
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Return False
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
                ComplaintcatgdtlCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetCOMPLAINTCATGDTL(ByVal CatgID As System.String, ByVal SeqNo As System.Int32) As Container.Complaintcatgdtl
            Dim rComplaintcatgdtl As Container.Complaintcatgdtl = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With ComplaintcatgdtlInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "CatgID = '" & CatgID & "' AND SeqNo = '" & SeqNo & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rComplaintcatgdtl = New Container.Complaintcatgdtl
                                rComplaintcatgdtl.CatgID = drRow.Item("CatgID")
                                rComplaintcatgdtl.SeqNo = drRow.Item("SeqNo")
                                rComplaintcatgdtl.UserID = drRow.Item("UserID")
                                rComplaintcatgdtl.RefID = drRow.Item("RefID")
                                rComplaintcatgdtl.FirstName = drRow.Item("FirstName")
                                rComplaintcatgdtl.LastName = drRow.Item("LastName")
                                rComplaintcatgdtl.Country = drRow.Item("Country")
                                rComplaintcatgdtl.State = drRow.Item("State")
                                rComplaintcatgdtl.Email = drRow.Item("Email")
                                rComplaintcatgdtl.Status = drRow.Item("Status")
                                If Not IsDBNull(drRow.Item("CreateDate")) Then
                                    rComplaintcatgdtl.CreateDate = drRow.Item("CreateDate")
                                End If
                                rComplaintcatgdtl.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rComplaintcatgdtl.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rComplaintcatgdtl.UpdateBy = drRow.Item("UpdateBy")
                                rComplaintcatgdtl.Active = drRow.Item("Active")
                                rComplaintcatgdtl.Inuse = drRow.Item("Inuse")
                                rComplaintcatgdtl.rowguid = drRow.Item("rowguid")
                                rComplaintcatgdtl.SyncCreate = drRow.Item("SyncCreate")
                                rComplaintcatgdtl.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rComplaintcatgdtl.IsHost = drRow.Item("IsHost")
                                rComplaintcatgdtl.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rComplaintcatgdtl = Nothing
                            End If
                        Else
                            rComplaintcatgdtl = Nothing
                        End If
                    End With
                End If
                Return rComplaintcatgdtl
            Catch ex As Exception
                Throw ex
            Finally
                rComplaintcatgdtl = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetCOMPLAINTCATGDTL(ByVal CatgID As System.String, ByVal SeqNo As System.Int32, DecendingOrder As Boolean) As List(Of Container.Complaintcatgdtl)
            Dim rComplaintcatgdtl As Container.Complaintcatgdtl = Nothing
            Dim lstComplaintcatgdtl As List(Of Container.Complaintcatgdtl) = New List(Of Container.Complaintcatgdtl)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With ComplaintcatgdtlInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by CatgID, SeqNo DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "CatgID = '" & CatgID & "' AND SeqNo = '" & SeqNo & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rComplaintcatgdtl = New Container.Complaintcatgdtl
                                rComplaintcatgdtl.CatgID = drRow.Item("CatgID")
                                rComplaintcatgdtl.SeqNo = drRow.Item("SeqNo")
                                rComplaintcatgdtl.UserID = drRow.Item("UserID")
                                rComplaintcatgdtl.RefID = drRow.Item("RefID")
                                rComplaintcatgdtl.FirstName = drRow.Item("FirstName")
                                rComplaintcatgdtl.LastName = drRow.Item("LastName")
                                rComplaintcatgdtl.Country = drRow.Item("Country")
                                rComplaintcatgdtl.State = drRow.Item("State")
                                rComplaintcatgdtl.Email = drRow.Item("Email")
                                rComplaintcatgdtl.Status = drRow.Item("Status")
                                rComplaintcatgdtl.CreateBy = drRow.Item("CreateBy")
                                rComplaintcatgdtl.UpdateBy = drRow.Item("UpdateBy")
                                rComplaintcatgdtl.Active = drRow.Item("Active")
                                rComplaintcatgdtl.Inuse = drRow.Item("Inuse")
                                rComplaintcatgdtl.rowguid = drRow.Item("rowguid")
                                rComplaintcatgdtl.SyncCreate = drRow.Item("SyncCreate")
                                rComplaintcatgdtl.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rComplaintcatgdtl.IsHost = drRow.Item("IsHost")
                                rComplaintcatgdtl.LastSyncBy = drRow.Item("LastSyncBy")
                                lstComplaintcatgdtl.Add(rComplaintcatgdtl)
                            Next

                        Else
                            rComplaintcatgdtl = Nothing
                        End If
                        Return lstComplaintcatgdtl
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rComplaintcatgdtl = Nothing
                lstComplaintcatgdtl = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetCOMPLAINTCATGDTLList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ComplaintcatgdtlInfo.MyInfo
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

#End Region

    End Class
#End Region

#Region "Container"
    Namespace Container
#Region "Complaintcatgdtl Container"
        Public Class Complaintcatgdtl_FieldName
            Public CatgID As System.String = "CatgID"
            Public SeqNo As System.String = "SeqNo"
            Public UserID As System.String = "UserID"
            Public RefID As System.String = "RefID"
            Public FirstName As System.String = "FirstName"
            Public LastName As System.String = "LastName"
            Public Country As System.String = "Country"
            Public State As System.String = "State"
            Public Email As System.String = "Email"
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

        Public Class Complaintcatgdtl
            Protected _CatgID As System.String
            Protected _SeqNo As System.Int32
            Private _UserID As System.String
            Private _RefID As System.String
            Private _FirstName As System.String
            Private _LastName As System.String
            Private _Country As System.String
            Private _State As System.String
            Private _Email As System.String
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
            ''' Mandatory
            ''' </summary>
            Public Property SeqNo As System.Int32
                Get
                    Return _SeqNo
                End Get
                Set(ByVal Value As System.Int32)
                    _SeqNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property UserID As System.String
                Get
                    Return _UserID
                End Get
                Set(ByVal Value As System.String)
                    _UserID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RefID As System.String
                Get
                    Return _RefID
                End Get
                Set(ByVal Value As System.String)
                    _RefID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property FirstName As System.String
                Get
                    Return _FirstName
                End Get
                Set(ByVal Value As System.String)
                    _FirstName = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property LastName As System.String
                Get
                    Return _LastName
                End Get
                Set(ByVal Value As System.String)
                    _LastName = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Country As System.String
                Get
                    Return _Country
                End Get
                Set(ByVal Value As System.String)
                    _Country = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property State As System.String
                Get
                    Return _State
                End Get
                Set(ByVal Value As System.String)
                    _State = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Email As System.String
                Get
                    Return _Email
                End Get
                Set(ByVal Value As System.String)
                    _Email = Value
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
#Region "Complaintcatgdtl Info"
    Public Class ComplaintcatgdtlInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "CatgID,SeqNo,UserID,RefID,FirstName,LastName,Country,State,Email,Status,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
                .CheckFields = "Status,Active,Inuse,Flag,IsHost"
                .TableName = "Complaintcatgdtl"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "CatgID,SeqNo,UserID,RefID,FirstName,LastName,Country,State,Email,Status,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
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
#Region "COMPLAINTCATGDTL Scheme"
    Public Class COMPLAINTCATGDTLScheme
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
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "SeqNo"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UserID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "RefID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "FirstName"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "LastName"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Country"
                .Length = 2
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "State"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Email"
                .Length = 80
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Status"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Inuse"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsHost"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(21, this)

        End Sub

        Public ReadOnly Property CatgID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property
        Public ReadOnly Property SeqNo As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property

        Public ReadOnly Property UserID As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property RefID As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property FirstName As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property LastName As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property Country As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property State As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property Email As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property Status As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property Inuse As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property IsHost As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region
#End Region

End Namespace