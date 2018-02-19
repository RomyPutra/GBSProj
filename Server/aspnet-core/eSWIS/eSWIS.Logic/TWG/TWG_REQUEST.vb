Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace TWG

#Region "TWG_REQUEST Class"
    Public NotInheritable Class TWG_REQUEST
        Inherits Core.CoreControl
        Private Twg_requestInfo As Twg_requestInfo = New Twg_requestInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"

        Private Function Save(ByVal Twg_requestCont As Container.Twg_request, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If Twg_requestCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Twg_requestInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "RequestID = '" & Twg_requestCont.RequestID & "' AND GeneratorLocID = '" & Twg_requestCont.GeneratorLocID & "' AND ReceiverLocID = '" & Twg_requestCont.ReceiverLocID & "'")
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
                                .TableName = "Twg_request"
                                .AddField("RemarkReject", Twg_requestCont.RemarkReject, SQLControl.EnumDataType.dtStringN)
                                .AddField("WasteCode", Twg_requestCont.WasteCode, SQLControl.EnumDataType.dtString)
                                .AddField("WasteType", Twg_requestCont.WasteType, SQLControl.EnumDataType.dtString)
                                .AddField("WasteName", Twg_requestCont.WasteName, SQLControl.EnumDataType.dtStringN)
                                .AddField("Status", Twg_requestCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Active", Twg_requestCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", Twg_requestCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", Twg_requestCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", Twg_requestCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", Twg_requestCont.UpdateBy, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "RequestID = '" & Twg_requestCont.RequestID & "' AND GeneratorLocID = '" & Twg_requestCont.GeneratorLocID & "' AND ReceiverLocID = '" & Twg_requestCont.ReceiverLocID & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("RequestID", Twg_requestCont.RequestID, SQLControl.EnumDataType.dtString)
                                                .AddField("GeneratorLocID", Twg_requestCont.GeneratorLocID, SQLControl.EnumDataType.dtString)
                                                .AddField("ReceiverLocID", Twg_requestCont.ReceiverLocID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "RequestID = '" & Twg_requestCont.RequestID & "' AND GeneratorLocID = '" & Twg_requestCont.GeneratorLocID & "' AND ReceiverLocID = '" & Twg_requestCont.ReceiverLocID & "'")
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
                Twg_requestCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Private Function SaveRequest(ByVal ListContReqTWG As List(Of Container.Twg_request), ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim ListSQL As ArrayList = New ArrayList()
            SaveRequest = False
            Dim DictionaryNo As String = ""
            Dim WasCode As String = ""
            Dim WasType As String = ""
            Dim LocID As String = ""
            Dim CompanyID As String = ""
            Try
                If ListContReqTWG Is Nothing Or ListContReqTWG.Count <= 0 Then
                    'Message return
                Else
                    StartSQLControl()
                    strSQL = "DELETE TWG_REQUEST WITH (ROWLOCK) WHERE GeneratorLocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, ListContReqTWG(0).GeneratorLocID) & "' AND Status='2'"
                    ListSQL.Add(strSQL)

                    For Each row In ListContReqTWG
                        With objSQL
                            .TableName = "Twg_request"
                            .AddField("RemarkReject", row.RemarkReject, SQLControl.EnumDataType.dtStringN)
                            .AddField("WasteCode", row.WasteCode, SQLControl.EnumDataType.dtString)
                            .AddField("WasteType", row.WasteType, SQLControl.EnumDataType.dtString)
                            .AddField("WasteName", row.WasteName, SQLControl.EnumDataType.dtStringN)
                            .AddField("Status", row.Status, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Active", row.Active, SQLControl.EnumDataType.dtNumeric)
                            .AddField("CreateDate", row.CreateDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("CreateBy", row.CreateBy, SQLControl.EnumDataType.dtString)
                            .AddField("LastUpdate", row.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("UpdateBy", row.UpdateBy, SQLControl.EnumDataType.dtString)
                            Select Case pType
                                Case SQLControl.EnumSQLType.stInsert
                                    .AddField("RequestID", row.RequestID, SQLControl.EnumDataType.dtString)
                                    .AddField("GeneratorLocID", row.GeneratorLocID, SQLControl.EnumDataType.dtString)
                                    .AddField("ReceiverLocID", row.ReceiverLocID, SQLControl.EnumDataType.dtString)
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                            End Select
                        End With
                        ListSQL.Add(strSQL)
                    Next

                    Try
                        objConn.BatchExecute(ListSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                    Catch axExecute As Exception
                        If pType = SQLControl.EnumSQLType.stInsert Then
                            message = axExecute.Message.ToString()
                        Else
                            message = axExecute.Message.ToString()
                        End If

                        Dim sqlStatement As String = " "
                        If objConn.FailedSQLStatement.Count > 0 Then
                            sqlStatement &= objConn.FailedSQLStatement.Item(0)
                        End If

                        Log.Notifier.Notify(axExecute)
                        Gibraltar.Agent.Log.Error("TWG_REQUEST", axExecute.Message & sqlStatement, axExecute.StackTrace)
                        Return False
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
                ListContReqTWG = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal Twg_requestCont As Container.Twg_request, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(Twg_requestCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
        End Function

        'ADD
        Public Function InsertRequest(ByVal ListContReqTWG As List(Of Container.Twg_request), ByRef message As String) As Boolean
            Return SaveRequest(ListContReqTWG, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal Twg_requestCont As Container.Twg_request, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(Twg_requestCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        Public Function Delete(ByVal Twg_requestCont As Container.Twg_request, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If Twg_requestCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Twg_requestInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "RequestID = '" & Twg_requestCont.RequestID & "' AND GeneratorLocID = '" & Twg_requestCont.GeneratorLocID & "' AND ReceiverLocID = '" & Twg_requestCont.ReceiverLocID & "'")
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
                                strSQL = BuildUpdate(Twg_requestInfo.MyInfo.TableName, " SET Flag = 0" & _
                                " , LastUpdate = '" & Twg_requestCont.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_requestCont.UpdateBy) & "' WHERE" & _
                                "RequestID = '" & Twg_requestCont.RequestID & "' AND GeneratorLocID = '" & Twg_requestCont.GeneratorLocID & "' AND ReceiverLocID = '" & Twg_requestCont.ReceiverLocID & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(Twg_requestInfo.MyInfo.TableName, "RequestID = '" & Twg_requestCont.RequestID & "' AND GeneratorLocID = '" & Twg_requestCont.GeneratorLocID & "' AND ReceiverLocID = '" & Twg_requestCont.ReceiverLocID & "'")
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
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Return False
            Finally
                Twg_requestCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetTWG_REQUEST(ByVal RequestID As System.String, ByVal GeneratorLocID As System.String, ByVal ReceiverLocID As System.String) As Container.Twg_request
            Dim rTwg_request As Container.Twg_request = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With Twg_requestInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "RequestID = '" & RequestID & "' AND GeneratorLocID = '" & GeneratorLocID & "' AND ReceiverLocID = '" & ReceiverLocID & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rTwg_request = New Container.Twg_request
                                rTwg_request.RequestID = drRow.Item("RequestID")
                                rTwg_request.GeneratorLocID = drRow.Item("GeneratorLocID")
                                rTwg_request.ReceiverLocID = drRow.Item("ReceiverLocID")
                                rTwg_request.RemarkReject = drRow.Item("RemarkReject")
                                rTwg_request.WasteCode = drRow.Item("WasteCode")
                                rTwg_request.WasteType = drRow.Item("WasteType")
                                rTwg_request.WasteName = drRow.Item("WasteName")
                                rTwg_request.Status = drRow.Item("Status")
                                rTwg_request.Active = drRow.Item("Active")
                                If Not IsDBNull(drRow.Item("CreateDate")) Then
                                    rTwg_request.CreateDate = drRow.Item("CreateDate")
                                End If
                                rTwg_request.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rTwg_request.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rTwg_request.UpdateBy = drRow.Item("UpdateBy")
                            Else
                                rTwg_request = Nothing
                            End If
                        Else
                            rTwg_request = Nothing
                        End If
                    End With
                End If
                Return rTwg_request
            Catch ex As Exception
                Throw ex
            Finally
                rTwg_request = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetTWG_REQUEST(ByVal RequestID As System.String) As Container.Twg_request
            Dim rTwg_request As Container.Twg_request = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With Twg_requestInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "RequestID = '" & RequestID & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rTwg_request = New Container.Twg_request
                                rTwg_request.RequestID = drRow.Item("RequestID")
                                rTwg_request.GeneratorLocID = drRow.Item("GeneratorLocID")
                                rTwg_request.ReceiverLocID = drRow.Item("ReceiverLocID")
                                rTwg_request.RemarkReject = drRow.Item("RemarkReject")
                                rTwg_request.WasteCode = drRow.Item("WasteCode")
                                rTwg_request.WasteType = drRow.Item("WasteType")
                                rTwg_request.WasteName = drRow.Item("WasteName")
                                rTwg_request.Status = drRow.Item("Status")
                                rTwg_request.Active = drRow.Item("Active")
                                If Not IsDBNull(drRow.Item("CreateDate")) Then
                                    rTwg_request.CreateDate = drRow.Item("CreateDate")
                                End If
                                rTwg_request.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rTwg_request.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rTwg_request.UpdateBy = drRow.Item("UpdateBy")
                            Else
                                rTwg_request = Nothing
                            End If
                        Else
                            rTwg_request = Nothing
                        End If
                    End With
                End If
                Return rTwg_request
            Catch ex As Exception
                Throw ex
            Finally
                rTwg_request = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetTWG_REQUEST(ByVal RequestID As System.String, ByVal GeneratorLocID As System.String, ByVal ReceiverLocID As System.String, DecendingOrder As Boolean) As List(Of Container.Twg_request)
            Dim rTwg_request As Container.Twg_request = Nothing
            Dim lstTwg_request As List(Of Container.Twg_request) = New List(Of Container.Twg_request)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With Twg_requestInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by RequestID, GeneratorLocID, ReceiverLocID DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "RequestID = '" & RequestID & "' AND GeneratorLocID = '" & GeneratorLocID & "' AND ReceiverLocID = '" & ReceiverLocID & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rTwg_request = New Container.Twg_request
                                rTwg_request.RequestID = drRow.Item("RequestID")
                                rTwg_request.GeneratorLocID = drRow.Item("GeneratorLocID")
                                rTwg_request.ReceiverLocID = drRow.Item("ReceiverLocID")
                                rTwg_request.RemarkReject = drRow.Item("RemarkReject")
                                rTwg_request.WasteCode = drRow.Item("WasteCode")
                                rTwg_request.WasteType = drRow.Item("WasteType")
                                rTwg_request.WasteName = drRow.Item("WasteName")
                                rTwg_request.Status = drRow.Item("Status")
                                rTwg_request.Active = drRow.Item("Active")
                                rTwg_request.CreateBy = drRow.Item("CreateBy")
                                rTwg_request.UpdateBy = drRow.Item("UpdateBy")
                                lstTwg_request.Add(rTwg_request)
                            Next

                        Else
                            rTwg_request = Nothing
                        End If
                        Return lstTwg_request
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rTwg_request = Nothing
                lstTwg_request = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetTWG_REQUESTList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With Twg_requestInfo.MyInfo
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

        Public Overloads Function GetTWGAcceptance(ByVal RequestID As String) As Data.DataTable
            If StartConnection() = True Then
                With Twg_requestInfo.MyInfo
                    StartSQLControl()
                    strSQL = "SELECT tr.RequestID, tr.GeneratorLocID, tr.WasteCode, CM.CodeDesc as WasteType, tr.Createby AS RequestBy, tr.CreateDate AS RequestDate, tr.UpdateBy, " & _
                        "tr.Status, CASE tr.Status WHEN 0 then 'Pending' WHEN 1 THEN 'Approved' ELSE 'Rejected' END AS StatusDesc " & _
                        "FROM TWG_REQUEST tr WITH (NOLOCK) " & _
                        " LEFT JOIN CODEMASTER CM WITH (NOLOCK) ON CM.CodeType='WTY' AND CM.CodeSeq=tr.WasteType " & _
                        " Where tr.RequestID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, RequestID) & "'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetTWGAcceptanceList(ByVal LocID As String, Optional ByVal Status As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With Twg_requestInfo.MyInfo
                    StartSQLControl()
                    strSQL = "SELECT tr.RequestID, tr.WasteCode, CM.CodeDesc as WasteType, bl.BranchName, bl.BizRegID, tr.Createby AS RequestBy, tr.CreateDate AS RequestDate, tr.UpdateBy, " & _
                        "tr.Status, CASE tr.Status WHEN 0 then 'Pending' WHEN 1 THEN 'Approved' ELSE 'Rejected' END AS StatusDesc " & _
                        "FROM TWG_REQUEST tr WITH (NOLOCK) LEFT JOIN BIZLOCATE bl WITH (NOLOCK) ON tr.GeneratorLocID=bl.BizLocID " & _
                        " LEFT JOIN CODEMASTER CM WITH (NOLOCK) ON CM.CodeType='WTY' AND CM.CodeSeq=tr.WasteType " & _
                        " Where tr.Status='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Status) & "' AND tr.ReceiverLocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

#End Region
    End Class
#End Region

#Region "Container"
    Namespace Container
#Region "Twg_request Container"
        Public Class Twg_request_FieldName
            Public RequestID As System.String = "RequestID"
            Public GeneratorLocID As System.String = "GeneratorLocID"
            Public ReceiverLocID As System.String = "ReceiverLocID"
            Public RemarkReject As System.String = "RemarkReject"
            Public WasteCode As System.String = "WasteCode"
            Public WasteType As System.String = "WasteType"
            Public WasteName As System.String = "WasteName"
            Public Status As System.String = "Status"
            Public Active As System.String = "Active"
            Public Flag As System.String = "Flag"
            Public CreateDate As System.String = "CreateDate"
            Public CreateBy As System.String = "CreateBy"
            Public LastUpdate As System.String = "LastUpdate"
            Public UpdateBy As System.String = "UpdateBy"
        End Class

        Public Class Twg_request
            Protected _RequestID As System.String
            Protected _GeneratorLocID As System.String
            Protected _ReceiverLocID As System.String
            Private _RemarkReject As System.String
            Protected _WasteCode As System.String
            Protected _WasteType As System.String
            Protected _WasteName As System.String
            Private _Status As System.Byte
            Private _Active As System.Byte
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property RequestID As System.String
                Get
                    Return _RequestID
                End Get
                Set(ByVal Value As System.String)
                    _RequestID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property GeneratorLocID As System.String
                Get
                    Return _GeneratorLocID
                End Get
                Set(ByVal Value As System.String)
                    _GeneratorLocID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ReceiverLocID As System.String
                Get
                    Return _ReceiverLocID
                End Get
                Set(ByVal Value As System.String)
                    _ReceiverLocID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RemarkReject As System.String
                Get
                    Return _RemarkReject
                End Get
                Set(ByVal Value As System.String)
                    _RemarkReject = Value
                End Set
            End Property
            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property WasteCode As System.String
                Get
                    Return _WasteCode
                End Get
                Set(ByVal Value As System.String)
                    _WasteCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property WasteType As System.String
                Get
                    Return _WasteType
                End Get
                Set(ByVal Value As System.String)
                    _WasteType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property WasteName As System.String
                Get
                    Return _WasteName
                End Get
                Set(ByVal Value As System.String)
                    _WasteName = Value
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

        End Class
#End Region
    End Namespace
#End Region

#Region "Class Info"
#Region "Twg_request Info"
    Public Class Twg_requestInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "RequestID,GeneratorLocID,ReceiverLocID,RemarkReject,WasteCode,WasteType,WasteName,Status,Active,Flag,CreateDate,CreateBy,LastUpdate,UpdateBy"
                .CheckFields = "Status,Active,Flag"
                .TableName = "Twg_request"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "RequestID,GeneratorLocID,ReceiverLocID,RemarkReject,WasteCode,WasteType,WasteName,Status,Active,Flag,CreateDate,CreateBy,LastUpdate,UpdateBy"
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
#Region "TWG_REQUEST Scheme"
    Public Class TWG_REQUESTScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "RequestID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "GeneratorLocID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ReceiverLocID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "RemarkReject"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "WasteCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "WasteType"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)

            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "WasteName"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)

            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Status"
                .Length = 1
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
                .FieldName = "Flag"
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

        End Sub

        Public ReadOnly Property RequestID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property
        Public ReadOnly Property GeneratorLocID As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property ReceiverLocID As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property

        Public ReadOnly Property RemarkReject As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property WasteCode As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property WasteType As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property WasteName As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property Status As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
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

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region
#End Region

End Namespace