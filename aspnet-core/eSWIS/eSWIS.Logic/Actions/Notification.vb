Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Actions
    Public NotInheritable Class Notification
        Inherits Core.CoreControl
        Private NotifyhdrInfo As NotifyhdrInfo = New NotifyhdrInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"

        Private Function Save(ByVal NotifyhdrCont As Container.Notifyhdr, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal isReactivate As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If NotifyhdrCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With NotifyhdrInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "TransNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifyhdrCont.TransNo) & "'")
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
                                .TableName = "Notifyhdr WITH (ROWLOCK)"
                                .AddField("TransType", NotifyhdrCont.TransType, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CompanyID", NotifyhdrCont.CompanyID, SQLControl.EnumDataType.dtString)
                                .AddField("LocID", NotifyhdrCont.LocID, SQLControl.EnumDataType.dtString)
                                .AddField("TermID", NotifyhdrCont.TermID, SQLControl.EnumDataType.dtCustom)
                                .AddField("PBT", NotifyhdrCont.PBT, SQLControl.EnumDataType.dtString)
                                .AddField("NoteID", NotifyhdrCont.NoteID, SQLControl.EnumDataType.dtString)
                                .AddField("ReferID", NotifyhdrCont.ReferID, SQLControl.EnumDataType.dtString)
                                .AddField("TransDate", NotifyhdrCont.TransDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("TransReasonCode", NotifyhdrCont.TransReasonCode, SQLControl.EnumDataType.dtString)
                                .AddField("TransRemark", NotifyhdrCont.TransRemark, SQLControl.EnumDataType.dtStringN)
                                .AddField("TransStatus", NotifyhdrCont.TransStatus, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Posted", NotifyhdrCont.Posted, SQLControl.EnumDataType.dtNumeric)
                                .AddField("PostDate", NotifyhdrCont.PostDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("EntryID", NotifyhdrCont.EntryID, SQLControl.EnumDataType.dtString)
                                .AddField("AuthID", NotifyhdrCont.AuthID, SQLControl.EnumDataType.dtString)
                                .AddField("AuthPOS", NotifyhdrCont.AuthPOS, SQLControl.EnumDataType.dtString)
                                .AddField("CreateDate", NotifyhdrCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", NotifyhdrCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("ApproveDate", NotifyhdrCont.ApproveDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("DeclineRemark", NotifyhdrCont.DeclineRemark, SQLControl.EnumDataType.dtString)
                                .AddField("ApproveBy", NotifyhdrCont.ApproveBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", NotifyhdrCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", NotifyhdrCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", NotifyhdrCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", NotifyhdrCont.Inuse, SQLControl.EnumDataType.dtNumeric)

                                If isReactivate Then
                                    .AddField("Flag", 1, SQLControl.EnumDataType.dtNumeric)
                                End If

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TransNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifyhdrCont.TransNo) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("TransNo", NotifyhdrCont.TransNo, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TransNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifyhdrCont.TransNo) & "'")
                                End Select
                            End With
                            Try

                                objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                message = "Save Successfully"
                                Return True

                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                Else
                                    message = axExecute.Message.ToString()
                                End If
                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/Notification", axExecute.Message & strSQL, axExecute.StackTrace)
                                Return False

                            Finally
                                objSQL.Dispose()
                            End Try
                        End If

                    End If
                End If
            Catch axAssign As ApplicationException
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/Notification", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/Notification", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                NotifyhdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal NotifyhdrCont As Container.Notifyhdr, ByRef message As String) As Boolean
            Return Save(NotifyhdrCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal NotifyhdrCont As Container.Notifyhdr, ByRef message As String, Optional ByVal isReactivate As Boolean = False) As Boolean
            Return Save(NotifyhdrCont, SQLControl.EnumSQLType.stUpdate, message, isReactivate)
        End Function

        Public Function Delete(ByVal NotifyhdrCont As Container.Notifyhdr, Optional ByVal message As String = Nothing) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If NotifyhdrCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With NotifyhdrInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "TransNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifyhdrCont.TransNo) & "'")
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
                                strSQL = BuildUpdate("Notifyhdr WITH (ROWLOCK)", " SET Flag = 0" & _
                                " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, NotifyhdrCont.UpdateBy) & "' WHERE " & _
                                "TransNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifyhdrCont.TransNo) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("Notifyhdr WITH (ROWLOCK)", "TransNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifyhdrCont.TransNo) & "'")
                        End If

                        Try

                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/Actions/Notification", exExecute.Message & strSQL, exExecute.StackTrace)
                            Return False

                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/Notification", axDelete.Message, axDelete.StackTrace)
                Return False

            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/Notification", exDelete.Message, exDelete.StackTrace)
                Return False

            Finally
                NotifyhdrCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetNotification(ByVal TransNo As System.String) As Container.Notifyhdr
            Dim rNotifyhdr As Container.Notifyhdr = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With NotifyhdrInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "TransNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, TransNo) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rNotifyhdr = New Container.Notifyhdr
                                rNotifyhdr.TransNo = drRow.Item("TransNo")
                                rNotifyhdr.TransType = drRow.Item("TransType")
                                rNotifyhdr.CompanyID = drRow.Item("CompanyID")
                                rNotifyhdr.LocID = drRow.Item("LocID")
                                rNotifyhdr.TermID = drRow.Item("TermID")
                                rNotifyhdr.PBT = drRow.Item("PBT")
                                rNotifyhdr.NoteID = drRow.Item("NoteID")
                                rNotifyhdr.ReferID = drRow.Item("ReferID")
                                rNotifyhdr.TransDate = drRow.Item("TransDate")
                                rNotifyhdr.TransReasonCode = drRow.Item("TransReasonCode")
                                rNotifyhdr.TransRemark = drRow.Item("TransRemark")
                                rNotifyhdr.TransStatus = drRow.Item("TransStatus")
                                rNotifyhdr.Posted = drRow.Item("Posted")
                                rNotifyhdr.PostDate = drRow.Item("PostDate")
                                rNotifyhdr.EntryID = IIf(IsDBNull(drRow.Item("EntryID")), "", drRow.Item("EntryID"))
                                rNotifyhdr.AuthID = drRow.Item("AuthID")
                                rNotifyhdr.AuthPOS = drRow.Item("AuthPOS")
                                rNotifyhdr.CreateDate = drRow.Item("CreateDate")
                                rNotifyhdr.CreateBy = drRow.Item("CreateBy")
                                rNotifyhdr.ApproveDate = IIf(IsDBNull(drRow.Item("ApproveDate")), Now, drRow.Item("ApproveDate"))
                                rNotifyhdr.DeclineRemark = drRow.Item("DeclineRemark")
                                rNotifyhdr.ApproveBy = IIf(IsDBNull(drRow.Item("ApproveBy")), "", drRow.Item("ApproveBy"))
                                rNotifyhdr.LastUpdate = IIf(IsDBNull(drRow.Item("LastUpdate")), Now, drRow.Item("LastUpdate"))
                                rNotifyhdr.UpdateBy = IIf(IsDBNull(drRow.Item("UpdateBy")), "", drRow.Item("UpdateBy"))
                                rNotifyhdr.Active = drRow.Item("Active")
                                rNotifyhdr.Inuse = drRow.Item("Inuse")
                                rNotifyhdr.RowGuid = drRow.Item("RowGuid")
                                rNotifyhdr.SyncCreate = drRow.Item("SyncCreate")
                                rNotifyhdr.SyncLastUpd = IIf(IsDBNull(drRow.Item("SyncLastUpd")), Now, drRow.Item("SyncLastUpd"))
                            Else
                                rNotifyhdr = Nothing
                            End If
                        Else
                            rNotifyhdr = Nothing
                        End If
                    End With
                End If
                Return rNotifyhdr
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/Notification", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rNotifyhdr = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetNotification(ByVal TransNo As System.String, ByVal DecendingOrder As Boolean) As List(Of Container.Notifyhdr)
            Dim rNotifyhdr As Container.Notifyhdr = Nothing
            Dim lstNotifyhdr As List(Of Container.Notifyhdr) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With NotifyhdrInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal TransNo As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "TransNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, TransNo) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rNotifyhdr = New Container.Notifyhdr
                                rNotifyhdr.TransNo = drRow.Item("TransNo")
                                rNotifyhdr.TransType = drRow.Item("TransType")
                                rNotifyhdr.CompanyID = drRow.Item("CompanyID")
                                rNotifyhdr.LocID = drRow.Item("LocID")
                                rNotifyhdr.TermID = drRow.Item("TermID")
                                rNotifyhdr.PBT = drRow.Item("PBT")
                                rNotifyhdr.NoteID = drRow.Item("NoteID")
                                rNotifyhdr.ReferID = drRow.Item("ReferID")
                                rNotifyhdr.TransDate = drRow.Item("TransDate")
                                rNotifyhdr.TransReasonCode = drRow.Item("TransReasonCode")
                                rNotifyhdr.TransRemark = drRow.Item("TransRemark")
                                rNotifyhdr.TransStatus = drRow.Item("TransStatus")
                                rNotifyhdr.Posted = drRow.Item("Posted")
                                rNotifyhdr.PostDate = drRow.Item("PostDate")
                                rNotifyhdr.EntryID = drRow.Item("EntryID")
                                rNotifyhdr.AuthID = drRow.Item("AuthID")
                                rNotifyhdr.AuthPOS = drRow.Item("AuthPOS")
                                rNotifyhdr.CreateDate = drRow.Item("CreateDate")
                                rNotifyhdr.CreateBy = drRow.Item("CreateBy")
                                rNotifyhdr.ApproveDate = drRow.Item("ApproveDate")
                                rNotifyhdr.DeclineRemark = drRow.Item("DeclineRemark")
                                rNotifyhdr.ApproveBy = drRow.Item("ApproveBy")
                                rNotifyhdr.LastUpdate = drRow.Item("LastUpdate")
                                rNotifyhdr.UpdateBy = drRow.Item("UpdateBy")
                                rNotifyhdr.Active = drRow.Item("Active")
                                rNotifyhdr.Inuse = drRow.Item("Inuse")
                                rNotifyhdr.RowGuid = drRow.Item("RowGuid")
                                rNotifyhdr.SyncCreate = drRow.Item("SyncCreate")
                                rNotifyhdr.SyncLastUpd = drRow.Item("SyncLastUpd")
                            Next
                            lstNotifyhdr.Add(rNotifyhdr)
                        Else
                            rNotifyhdr = Nothing
                        End If
                        Return lstNotifyhdr
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/Notification", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rNotifyhdr = Nothing
                lstNotifyhdr = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function GetNotificationByLocation(ByVal LocationID As String, Optional ByVal StartDate As String = Nothing, Optional ByVal EndDate As String = Nothing) As List(Of Container.Notifyhdr)
            Dim rNotifyHDR As Container.Notifyhdr = Nothing
            Dim listNotifyHDR As List(Of Container.Notifyhdr) = Nothing
            Dim dtTemp As DataTable = Nothing
            If StartConnection() = True Then
                StartSQLControl()
                strSQL = "select h.CompanyID, be.CompanyName, h.LocID, bl.BranchName, h.TransNo, h.TransDate, h.ReferID, h.AuthID, h.AuthPOS, h.Posted,  h.TransRemark, ISNULL(h.DeclineRemark,'') AS DeclineRemark " & _
                            " from NOTIFYHDR h with (nolock) left join BIZENTITY be with (nolock) on h.CompanyID=be.BizRegID" & _
                            " left join BIZLOCATE bl with (nolock) on h.LocID=bl.BizLocID" & _
                            " where h.LocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocationID) & "'"

                If StartDate IsNot Nothing AndAlso StartDate <> "" AndAlso EndDate IsNot Nothing AndAlso EndDate <> "" Then
                    strSQL &= " And (convert(date,H.TransDate) >= convert(date, '" & StartDate & "', 103) and convert(date,H.TransDate) <= convert(date, '" & EndDate & "', 103))"
                ElseIf StartDate = "" AndAlso EndDate = "" Then
                    strSQL &= " And (convert(date,H.TransDate) >= convert(date,dateadd(dd,-day(getdate())+1,getdate())) and convert(date,H.TransDate) <= convert(date,getDate()))"
                ElseIf StartDate Is Nothing AndAlso EndDate Is Nothing Then
                    strSQL &= " And (convert(date,H.TransDate) >= convert(date,dateadd(dd,-day(getdate())+1,getdate())) and convert(date,H.TransDate) <= convert(date,getDate()))"
                ElseIf StartDate IsNot Nothing AndAlso StartDate <> "" AndAlso EndDate = "" Then
                    strSQL &= " And (convert(date,H.TransDate) >= convert(date, '" & StartDate & "', 103) and convert(date,H.TransDate) <= convert(date,getDate()))"
                ElseIf StartDate = "" AndAlso EndDate IsNot Nothing AndAlso EndDate <> "" Then
                    strSQL &= " And (convert(date,H.TransDate) >= convert(date,dateadd(dd,-day(getdate())+1,getdate())) and convert(date,H.TransDate) <= convert(date, '" & EndDate & "', 103))"
                ElseIf StartDate IsNot Nothing AndAlso StartDate <> "" AndAlso EndDate Is Nothing Then
                    strSQL &= " And (convert(date,H.TransDate) >= convert(date, '" & StartDate & "', 103) and convert(date,H.TransDate) <= convert(date,getDate()))"
                ElseIf StartDate Is Nothing AndAlso EndDate IsNot Nothing AndAlso EndDate <> "" Then
                    strSQL &= " And (convert(date,H.TransDate) >= convert(date,dateadd(dd,-day(getdate())+1,getdate())) and convert(date,H.TransDate) <= convert(date, '" & EndDate & "', 103))"
                End If

                Try
                    dtTemp = objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text)
                    If dtTemp IsNot Nothing AndAlso dtTemp.Rows.Count > 0 Then
                        listNotifyHDR = New List(Of Container.Notifyhdr)
                        For Each rowHDR As DataRow In dtTemp.Rows
                            rNotifyHDR = New Container.Notifyhdr
                            With rNotifyHDR
                                .CompanyID = rowHDR.Item("CompanyID").ToString
                                .CompanyName = rowHDR.Item("CompanyName").ToString
                                .LocID = rowHDR.Item("LocID").ToString
                                .BranchName = rowHDR.Item("BranchName").ToString
                                .TransNo = rowHDR.Item("TransNo").ToString
                                .TransDate = rowHDR.Item("TransDate").ToString
                                .ReferID = rowHDR.Item("ReferID").ToString
                                .AuthID = rowHDR.Item("AuthID").ToString
                                .AuthPOS = rowHDR.Item("AuthPOS").ToString
                                .Posted = rowHDR.Item("Posted").ToString
                                .TransRemark = rowHDR.Item("TransRemark").ToString
                                .DeclineRemark = rowHDR.Item("DeclineRemark").ToString

                            End With
                            listNotifyHDR.Add(rNotifyHDR)
                        Next
                    End If
                Catch ex As Exception
                    Log.Notifier.Notify(ex)
                    Gibraltar.Agent.Log.Error("Actions/Notification", ex.Message & " " & strSQL, ex.StackTrace)
                Finally
                    EndSQLControl()
                End Try
            End If
            EndConnection()
            Return listNotifyHDR
        End Function

        Public Overloads Function GetNotificationList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With NotifyhdrInfo.MyInfo
                    If SQL = Nothing Or SQL = String.Empty Then
                        strSQL = BuildSelect(.FieldsList, .TableName, FieldCond)
                    Else
                        strSQL = SQL
                    End If
                    Try
                        Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                    Catch ex As Exception
                        Log.Notifier.Notify(ex)
                        Gibraltar.Agent.Log.Error("Actions/Notification", ex.Message & " " & strSQL, ex.StackTrace)
                    End Try
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetNotificationListed(Optional ByVal FieldCond As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With NotifyhdrInfo.MyInfo
                    If StartConnection() = True Then
                        StartSQLControl()
                        strSQL = "SELECT TransNo,TransType,CompanyID,(SELECT BranchName FROM BIZLOCATE WHERE BizLocID=Notifyhdr.LocID) " & _
                            "AS CompanyName,LocID,TermID,PBT,NoteID,ReferID,TransDate,TransReasonCode,TransRemark,TransStatus,CASE WHEN Posted = 0 THEN 'Draft' WHEN Posted = 1 THEN 'Submitted' WHEN Posted = 2 THEN 'Pending Cancellation' WHEN Posted = 3 THEN 'Inactive' END AS Posted,PostDate, " & _
                            "EntryID,AuthID,AuthPOS,CreateDate,CreateBy,ApproveDate, DeclineRemark,ApproveBy,LastUpdate,UpdateBy,Active,Inuse,Flag, " & _
                            "RowGuid,SyncCreate,SyncLastUpd FROM Notifyhdr WITH (NOLOCK) "
                        If FieldCond IsNot Nothing AndAlso FieldCond <> "" Then
                            strSQL &= "WHERE " & FieldCond
                        End If


                        Try
                            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Catch ex As Exception
                            Log.Notifier.Notify(ex)
                            Gibraltar.Agent.Log.Error("Actions/Notification", ex.Message & " " & strSQL, ex.StackTrace)
                        End Try
                    End If
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetNotificationCompanyListed(Optional ByVal FieldCond As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With NotifyhdrInfo.MyInfo
                    If StartConnection() = True Then
                        StartSQLControl()
                        strSQL = "SELECT DISTINCT HDR.TransNo,TransType,HDR.CompanyID,(SELECT BranchName FROM BIZLOCATE WHERE BizLocID=HDR.LocID) " & _
                            "AS CompanyName,DTL.ItemCode,DTL.ItmName,HDR.LocID,HDR.TermID,PBT,NoteID,ReferID,TransDate,TransReasonCode,TransRemark,TransStatus,CASE WHEN HDR.Posted = 0 THEN 'Draft' WHEN HDR.Posted = 1 THEN 'Submitted' WHEN HDR.Posted = 2 THEN 'Pending Cancellation' WHEN HDR.Posted = 3 THEN 'Inactive' END AS Posted,PostDate, " & _
                            "EntryID,AuthID,AuthPOS,HDR.CreateDate,HDR.CreateBy,ApproveDate, DeclineRemark,ApproveBy,HDR.LastUpdate,HDR.UpdateBy,Active,Inuse,Flag, " & _
                            "HDR.RowGuid,HDR.SyncCreate,HDR.SyncLastUpd FROM Notifyhdr HDR WITH (NOLOCK) " & _
                            "INNER JOIN NOTIFYDTL DTL On DTL.TransNo=HDR.TransNo "
                        If FieldCond IsNot Nothing AndAlso FieldCond <> "" Then
                            strSQL &= "WHERE " & FieldCond
                        End If

                        Try
                            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Catch ex As Exception
                            Log.Notifier.Notify(ex)
                            Gibraltar.Agent.Log.Error("Actions/Notification", ex.Message & " " & strSQL, ex.StackTrace)
                        End Try
                    End If
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetNotificationCompanyWesteListed(Optional ByVal FieldCond As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With NotifyhdrInfo.MyInfo
                    If StartConnection() = True Then
                        StartSQLControl()
                        strSQL = "SELECT D.TransNo, D.TransSeq, D.RefSeq, D.ItemCode, D.ItmName, D.ItmSource, S.StorageAreaCode, " & _
                                 "S.StorageBin, D.Qty, D.PackQty,(select UOMDesc from UOM where UOMCode=D.UOM and Active=1) as UOM," & _
                                 "(select CodeDesc from codemaster where codeType='WTY' and Active=1 and Code=D.TypeCode) as WasteType " & _
                                 "FROM NOTIFYDTL D left join STORAGEMASTER S on D.StorageID=S.StorageID "

                        If FieldCond IsNot Nothing AndAlso FieldCond <> "" Then
                            strSQL &= "WHERE D.RecType=2 AND " & FieldCond
                        End If

                        Try
                            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Catch ex As Exception
                            Log.Notifier.Notify(ex)
                            Gibraltar.Agent.Log.Error("Actions/Notification", ex.Message & " " & strSQL, ex.StackTrace)
                        End Try
                    End If
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetNotificationListedWG(Optional ByVal FieldCond As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With NotifyhdrInfo.MyInfo
                    If StartConnection() = True Then
                        StartSQLControl()

                        strSQL = "select H.LOCID, D.ItemCode, C.CODEDESC, D.ItmName, D.TypeCode, N.AuthID as CreateBy, N.TransDate, N.TransDate LastUpdate, N.Posted, N.Flag, N.H_Status, isnull((select CodeDesc from CODEMASTER where CodeType='WTY' and Active=1 and Code=D.typeCode),'') as TypeDes,CASE displaytype WHEN 0 THEN 'Scheduled Waste' ELSE 'Residue' END as Residue" &
                                " FROM NotifyHDR H INNER JOIN NotifyDTL D on H.CompanyID = D.CompanyID AND H.LocID = D.LocID  AND H.TransNO=D.TransNO and D.RecType=2 " &
                                " JOIN ITEMLOC il ON D.LocID = il.LocID AND D.ItemCode = il.ItemCode AND D.ItmName = il.ItemName " &
                                " LEFT JOIN CODEMASTER C on C.Code = D.TypeCode AND CodeType = 'WTY'" &
                                " CROSS APPLY (SELECT TOP 1 XH.AUTHID, XH.TRANSDATE, XH.POSTED, XH.FLAG, CASE XH.POSTED WHEN 3 THEN 'INACTIVE' WHEN 0 THEN 'DRAFT' ELSE 'ACTIVE' END H_STATUS FROM NOTIFYHDR XH WITH (NOLOCK)" &
                                " INNER JOIN NOTIFYDTL XD WITH (NOLOCK) ON XD.TRANSNO=XH.TRANSNO AND XD.RECTYPE=2 WHERE(XH.LOCID = H.LOCID And XD.ITEMCODE = D.ITEMCODE And XD.TYPECODE = D.TYPECODE And XD.ITMNAME = D.ITMNAME) ORDER BY XH.TRANSDATE DESC) N"

                        If FieldCond IsNot Nothing AndAlso FieldCond <> "" Then
                            strSQL &= " WHERE " & FieldCond & " GROUP BY H.LOCID, D.ItemCode, C.CODEDESC, D.ItmName, D.TypeCode, N.AuthID, N.TransDate, N.TransDate, N.Posted, N.Flag, N.H_Status, D.Displaytype "
                        End If
                        Try
                            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Catch ex As Exception
                            Log.Notifier.Notify(ex)
                            Gibraltar.Agent.Log.Error("Actions/Notification", ex.Message & " " & strSQL, ex.StackTrace)
                        End Try
                    End If
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetNotificationListedCompany(Optional ByVal FieldCond As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With NotifyhdrInfo.MyInfo
                    If StartConnection() = True Then
                        StartSQLControl()
                        strSQL = "select H.LOCID, D.ItemCode, C.CODEDESC, D.ItmName, N.Qty, I.QtyOnHand, D.TypeCode, N.AuthID as CreateBy, N.TransDate, N.TransDate LastUpdate, N.Posted, N.Flag, N.H_Status" &
                                " FROM NotifyHDR H INNER JOIN NotifyDTL D on H.CompanyID = D.CompanyID AND H.LocID = D.LocID  AND H.TransNO=D.TransNO and D.RecType=2 " &
                                " INNER JOIN ITEMLOC I on I.LocID = H.LocID AND I.ItemCode = D.ItemCode AND I.ItemName = D.ItmName" &
                                " LEFT JOIN CODEMASTER C on C.Code = D.TypeCode AND CodeType = 'WTY'" &
                                " CROSS APPLY (SELECT TOP 1 XH.AUTHID, XH.TRANSDATE, XD.Qty, XH.POSTED, XH.FLAG, CASE XH.POSTED WHEN 3 THEN 'INACTIVE' WHEN 0 THEN 'DRAFT' ELSE 'ACTIVE' END H_STATUS FROM NOTIFYHDR XH WITH (NOLOCK)" &
                                " INNER JOIN NOTIFYDTL XD WITH (NOLOCK) ON XD.TRANSNO=XH.TRANSNO AND XD.RECTYPE=2 WHERE(XH.LOCID = H.LOCID And XD.ITEMCODE = D.ITEMCODE And XD.TYPECODE = D.TYPECODE And XD.ITMNAME = D.ITMNAME) ORDER BY XH.TRANSDATE DESC) N"

                        If FieldCond IsNot Nothing AndAlso FieldCond <> "" Then
                            strSQL &= " WHERE " & FieldCond & " "
                        End If
                        strSQL &= " GROUP BY H.LOCID, D.ItemCode, C.CODEDESC, D.ItmName, D.TypeCode, N.AuthID, N.TransDate, N.TransDate, N.Posted, N.Flag, N.H_Status, N.Qty, I.QtyOnHand"
                        Try
                            Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Catch ex As Exception
                            Log.Notifier.Notify(ex)
                            Gibraltar.Agent.Log.Error("Actions/Notification", ex.Message & " " & strSQL, ex.StackTrace)
                        End Try
                    End If
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetNotificationListExport(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With NotifyhdrInfo.MyInfo
                    strSQL = "SELECT hdr.TransNo,hdr.ReferID,(SELECT CompanyName FROM BizEntity WHERE BizRegID=hdr.CompanyID) AS CompanyName,dtl.ItemCode,dtl.ItmName, " &
                        " dtl.ItmSource,u.UOMDesc, c.CodeDesc, dtl.Qty, dtl.PackQty, hdr.AuthID, hdr.AuthPOS, CONVERT(VARCHAR(11), hdr.PostDate, 105) AS PostDate, " &
                        " CASE WHEN hdr.Posted=0 THEN 'Draft' WHEN hdr.Posted=1 THEN 'Submitted' WHEN hdr.Posted=2 THEN 'Pending Cancellation' WHEN hdr.Posted=3 " &
                        " THEN 'Inactive' END AS Posted FROM Notifyhdr hdr WITH (NOLOCK) INNER JOIN NOTIFYDTL dtl WITH (NOLOCK) ON hdr.TransNo=dtl.TransNo" &
                        " LEFT JOIN UOM u WITH (NOLOCK) ON u.UOMCode=dtl.UOM LEFT JOIN CODEMASTER c WITH (NOLOCK) ON c.Code=dtl.TypeCode AND c.CodeType='WTY' "
                    If FieldCond IsNot Nothing AndAlso FieldCond <> "" Then
                        strSQL &= FieldCond
                    End If
                    Try
                        Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                    Catch ex As Exception
                        Log.Notifier.Notify(ex)
                        Gibraltar.Agent.Log.Error("Actions/Notification", ex.Message & " " & strSQL, ex.StackTrace)
                    End Try
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetNotificationListWS(ByVal BeginDate As String, ByVal EndDate As String, Optional ByVal CompanyID As String = Nothing, Optional ByVal LocID As String = Nothing) As Data.DataTable
            Dim FieldCond As String = " flag!=0 "
            If StartConnection() = True Then
                StartSQLControl()
                With NotifyhdrInfo.MyInfo
                    FieldCond &= " AND CompanyID='" & CompanyID & "'"

                    FieldCond &= " AND LocID='" & LocID & "'"
                    FieldCond &= " And (TransDate >= '" & BeginDate & "' and TransDate <= '" & EndDate & "') "
                    strSQL = BuildSelect(.FieldsList, .TableName, FieldCond)
                    Try
                        Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                    Catch ex As Exception
                        Log.Notifier.Notify(ex)
                        Gibraltar.Agent.Log.Error("Actions/Notification", ex.Message & " " & strSQL, ex.StackTrace)
                    End Try
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetNotificationShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With NotifyhdrInfo.MyInfo
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

        'Added by Ivan 10 Sep 2014
        Public Overloads Function GetNotificationCustomList(Optional ByVal TransNo As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With NotifyhdrInfo.MyInfo

                    'amended by diana 20140911, load correct details from location
                    strSQL = "SELECT TransNo, ReferID, NoteID, PostDate, Notifyhdr.LocID, TransDate, AccNo, AuthID,NOTIFYHDR.Active as Active, AuthPOS, CompanyID, Posted, TransRemark, ISNULL(DeclineRemark,'') AS DeclineRemark, " &
                        "ISNULL( (SELECT BranchName FROM BIZLOCATE WHERE BizLocID=Notifyhdr.LocID),'') AS CompanyName, " &
                        "ISNULL( (SELECT RegNo FROM BizEntity WHERE BizRegID=Notifyhdr.CompanyID),'') AS RegNo, " &
                        "ISNULL( (SELECT AccNo FROM BizLocate WHERE BizLocID=Notifyhdr.LocID),'') AS AccNo, " &
                        "BranchName, Address1, Address2, Address3, Address4, " &
                        "ISNULL( (SELECT StateDesc FROM STATE WHERE StateCode=BIZLOCATE.State and CountryCode=BIZLOCATE.Country),'') AS StateDesc, " &
                        "ISNULL( (SELECT CityDesc FROM CITY WHERE CityCode=BIZLOCATE.City and CountryCode=BIZLOCATE.Country and StateCode=BIZLOCATE.State),'') AS CityDesc, " &
                        "ISNULL( (SELECT CountryDesc FROM COUNTRY WHERE CountryCode=BIZLOCATE.Country),'') AS CountryDesc " &
                        "FROM NOTIFYHDR INNER JOIN BIZLOCATE ON " &
                        " NOTIFYHDR.LocID=BIZLOCATE.BizLocID"

                    If TransNo IsNot Nothing AndAlso TransNo <> "" Then
                        strSQL &= " WHERE TransNo='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, TransNo) & "'"
                    End If

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
                EndSQLControl()
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetWGNotificationName(Optional ByVal LocID As String = "", Optional ByVal TransNo As String = "", Optional ByVal WasteCode As String = "", Optional ByVal WasteType As String = "", Optional ByVal WasteName As String = "") As Data.DataTable
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With NotifyhdrInfo.MyInfo
                        strSQL = "select d.Qty,d.ItemCode as ItemCode, CodeDesc as WasteType, ItmDesc, ItmName, d.TransNo, ItmSource, h.TransDate,c.CodeSeq as wastype" &
                            " from Notifydtl d INNER JOIN CODEMASTER c ON c.code = CASE d.TypeCode WHEN '' THEN c.Code ELSE d.TypeCode END AND c.CodeType = 'WTY'" &
                            " INNER JOIN NOTIFYHDR h on d.TransNo = h.TransNo AND h.Active=1 AND h.Flag=1 AND h.Posted=1" &
                            " WHERE d.LocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "'"

                        If TransNo IsNot Nothing And TransNo IsNot String.Empty Then
                            strSQL &= " AND d.TransNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, TransNo) & "'"
                        End If

                        If WasteCode IsNot Nothing And WasteCode IsNot String.Empty Then
                            strSQL &= " AND d.ItemCode ='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteCode) & "'"
                        End If

                        If WasteType IsNot Nothing And WasteType IsNot String.Empty Then
                            strSQL &= " and c.Code = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteType) & "'"
                        End If

                        strSQL &= " ORDER BY h.TransDate DESC"
                        Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                    End With
                End If

            Catch axExecute As Exception
                Log.Notifier.Notify(axExecute)
                Gibraltar.Agent.Log.Error("Notification", axExecute.Message & strSQL, axExecute.StackTrace)
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWGNotificationQty(ByVal ItemCode As String, ByVal CompanyID As String, ByVal LocID As String, ByVal WasteType As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With NotifyhdrInfo.MyInfo

                    strSQL = "SELECT ItemName as ItmName FROM ITEMHNDLOC INNER JOIN CODEMASTER on ITEMHNDLOC.TypeCode = CODEMASTER.Code and CodeType = 'WTY' WHERE " &
                       " ITEMHNDLOC.active=1 and ITEMHNDLOC.flag=1 and " &
                       " ITEMHNDLOC.ItemCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "' AND " &
                       " ITEMHNDLOC.LocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "'  " &
                       " and CODEMASTER.CodeDesc='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteType) & "' "

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            End If
            EndSQLControl()
            EndConnection()
        End Function

        'function for ISWIS_API to get notif info on CN
        Public Overloads Function GetWGNotification(ByVal ItemCode As String, ByVal CompanyID As String, ByVal LocID As String, ByVal WasteTypeCode As String) As List(Of Container.Notifydtl)
            Dim dt As DataTable
            Dim rNotifDetail As Container.Notifydtl
            Dim ListNotifDetail As List(Of Container.Notifydtl)
            Try
                ListNotifDetail = Nothing
                If StartConnection() = True Then
                    StartSQLControl()
                    strSQL = "SELECT DTL.ItemCode, DTL.ItmName, DTL.TypeCode, DTL.Qty, ISNULL(CM.CodeDesc ,'') AS WasteType FROM NOTIFYHDR hdr INNER JOIN NOTIFYDTL dtl ON hdr.TransNo=dtl.TransNo and dtl.displaytype=0 " & _
                        " LEFT JOIN CODEMASTER CM WITH(NOLOCK) ON CM.Code=DTL.TypeCode AND CM.CodeType='WTY' WHERE " & _
                        " hdr.posted=1 and hdr.flag=1 and " & _
                        " ItemCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "' AND " & _
                        " dtl.CompanyID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompanyID) & "' AND " & _
                        " dtl.LocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "'  " & _
                        " and CM.Code='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteTypeCode) & "' " & _
                        "  ORDER BY HDR.TRANSDATE DESC"

                    dt = objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text)
                    If dt IsNot Nothing Then
                        ListNotifDetail = New List(Of Container.Notifydtl)
                        For Each row As DataRow In dt.Rows
                            rNotifDetail = New Container.Notifydtl
                            With rNotifDetail
                                .ItemCode = row.Item("ItemCode").ToString
                                .ItmName = row.Item("ItmName").ToString
                                .TypeCode = row.Item("TypeCode").ToString
                                .Qty = row.Item("Qty")
                            End With
                            ListNotifDetail.Add(rNotifDetail)
                        Next
                    End If
                End If
                Return ListNotifDetail
            Catch ex As Exception
                Throw ex
            Finally
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWGNotificationQty_SubmissionReceiver(ByVal ItemCode As String, ByVal CompanyID As String, ByVal LocID As String, ByVal WasteType As String, ByVal WasteName As String, ByVal ReceiverID As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With NotifyhdrInfo.MyInfo

                    strSQL = "SELECT ISNULL(SUM(d.ExpectedQty1),0) ExpectedQty1, ISNULL(SUM(t.ExpectedQty1),0) ExpectedQtyReceiver, " &
                        "E.NickName, BL.BranchName, DTL.TransNo, DTL.ItemCode, DTL.ItmName, DTL.TypeCode, DTL.Qty, ISNULL(CM.CodeDesc ,'') " &
                        "AS WasteType, DTL.CreateBy, DTL.CreateDate FROM NOTIFYHDR hdr WITH(NOLOCK) INNER JOIN NOTIFYDTL dtl WITH(NOLOCK) ON " &
                        "hdr.TransNo=dtl.TransNo LEFT JOIN CODEMASTER CM WITH(NOLOCK) ON CM.Code=DTL.TypeCode AND CM.CodeType='WTY' and " &
                        "CM.Code='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteType) & "' INNER JOIN BIZLOCATE BL WITH(NOLOCK) ON dtl.CompanyID = BL.BizRegID LEFT JOIN TWG_SUBMISSIONHDR c ON " &
                        "c.GeneratorID = dtl.CompanyID LEFT JOIN TWG_SUBMISSIONHDR hd ON c.ReceiverID = hd.ReceiverID and " &
                        "hd.GeneratorID = dtl.CompanyID AND hd.ReceiverID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverID) & "' LEFT JOIN TWG_SUBMISSIONDTL d ON " &
                        "d.SubmissionID = c.SubmissionID LEFT JOIN TWG_SUBMISSIONDTL t ON hd.SubmissionID = t.SubmissionID AND " &
                        "t.WasteCode = d.WasteCode AND t.WasteCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "' INNER JOIN USRPROFILE UP WITH(NOLOCK) ON BL.BizRegID = UP.ParentID " &
                        "INNER JOIN EMPLOYEE E WITH(NOLOCK) ON UP.RefID = E.EmployeeID WHERE dtl.ItemCode= '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "' AND " &
                        "dtl.CompanyID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompanyID) & "' AND  dtl.LocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' and dtl.ItmName='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteName) & "' GROUP BY E.NickName, " &
                        "BL.BranchName, DTL.TransNo, DTL.ItemCode, DTL.ItmName, DTL.TypeCode, DTL.Qty, DTL.CreateBy, DTL.CreateDate, CM.CodeDesc, HDR.TRANSDATE ORDER BY HDR.TRANSDATE DESC "

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetWGNotificationQty_Submission(ByVal ItemCode As String, ByVal CompanyID As String, ByVal LocID As String, ByVal WasteType As String, ByVal WasteName As String, ByVal ReceiverID As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With NotifyhdrInfo.MyInfo
                    strSQL = "SELECT ISNULL(SUM(d.ExpectedQty1),0) ExpectedQty1,c.LastSyncBy as NickName, BL.BranchName, DTL.TransNo, DTL.ItemCode, " &
                        "DTL.ItmName, DTL.TypeCode, DTL.Qty, ISNULL(CM.CodeDesc ,'') AS WasteType, DTL.CreateBy, DTL.CreateDate FROM " &
                        "NOTIFYHDR hdr WITH(NOLOCK) INNER JOIN NOTIFYDTL dtl WITH(NOLOCK) ON hdr.TransNo=dtl.TransNo LEFT JOIN CODEMASTER CM " &
                        "WITH(NOLOCK) ON CM.Code=DTL.TypeCode AND CM.CodeType='WTY' INNER JOIN BIZLOCATE BL WITH(NOLOCK) ON dtl.CompanyID = BL.BizRegID " &
                        "LEFT JOIN TWG_SUBMISSIONHDR c ON c.GeneratorID = dtl.CompanyID  LEFT JOIN TWG_SUBMISSIONDTL d ON d.SubmissionID = c.SubmissionID " &
                        "AND d.WasteCode = dtl.ItemCode WHERE hdr.posted=1 and hdr.flag=1 and  ItemCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "' AND  dtl.CompanyID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompanyID) & "' AND " &
                        "dtl.LocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' and CM.Code='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteType) & "' and dtl.ItmName='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteName) & "' GROUP BY c.LastSyncBy, BL.BranchName, DTL.TransNo, " &
                        "DTL.ItemCode, DTL.ItmName, DTL.TypeCode, DTL.Qty, DTL.CreateBy, DTL.CreateDate, CM.CodeDesc, HDR.TRANSDATE ORDER BY HDR.TRANSDATE DESC "

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetWGNotificationResidue(ByVal LocID As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With NotifyhdrInfo.MyInfo
                    strSQL = "SELECT ItemCode, ItemName, TypeCode, CodeDesc AS TypeDesc, IsSelected, ItemCode +'|'+TypeCode +'|'+ItemName AS Filter" & _
                        " FROM ITEMLOC it WITH (NOLOCK) LEFT JOIN CODEMASTER cm WITH (NOLOCK)ON it.TypeCode=cm.Code AND cm.CodeType='WTY'" & _
                        " WHERE LocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' AND IsSelected='1' AND it.Active='1' AND it.Flag='1'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetWGNotificationDeclineRemark(ByVal LocID As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With NotifyhdrInfo.MyInfo
                    strSQL = "SELECT TOP 1 ApproveBy, DeclineRemark, ApproveDate FROM NOTIFYHDR " & _
                        " WHERE LocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' AND Active='1' AND Flag='1' ORDER By ApproveDate DESC"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            End If
            EndSQLControl()
            EndConnection()
        End Function

#End Region

    End Class


    Namespace Container

#Region "Class Container"
        Public Class Notifyhdr
            Public fTransNo As System.String = "TransNo"
            Public fTransType As System.String = "TransType"
            Public fCompanyID As System.String = "CompanyID"
            Public fCompanyName As System.String = "CompanyName"
            Public fLocID As System.String = "LocID"
            Public fTermID As System.String = "TermID"
            Public fPBT As System.String = "PBT"
            Public fNoteID As System.String = "NoteID"
            Public fReferID As System.String = "ReferID"
            Public fTransDate As System.String = "TransDate"
            Public fTransReasonCode As System.String = "TransReasonCode"
            Public fTransRemark As System.String = "TransRemark"
            Public fTransStatus As System.String = "TransStatus"
            Public fPosted As System.String = "Posted"
            Public fPostDate As System.String = "PostDate"
            Public fEntryID As System.String = "EntryID"
            Public fAuthID As System.String = "AuthID"
            Public fAuthPOS As System.String = "AuthPOS"
            Public fCreateDate As System.String = "CreateDate"
            Public fCreateBy As System.String = "CreateBy"
            Public fApproveDate As System.String = "ApproveDate"
            Public fDeclineRemark As System.String = "DeclineRemark"
            Public fApproveBy As System.String = "ApproveBy"
            Public fLastUpdate As System.String = "LastUpdate"
            Public fResidue As System.String = "Residue"
            Public fUpdateBy As System.String = "UpdateBy"
            Public fActive As System.String = "Active"
            Public fInuse As System.String = "Inuse"
            Public fFlag As System.String = "Flag"
            Public fRowGuid As System.String = "RowGuid"
            Public fSyncCreate As System.String = "SyncCreate"
            Public fSyncLastUpd As System.String = "SyncLastUpd"

            Protected _TransNo As System.String
            Private _TransType As System.Byte
            Private _CompanyID As System.String
            Private _CompanyName As System.String
            Private _LocID As System.String
            Private _TermID As System.Int16
            Private _PBT As System.String
            Private _NoteID As System.String
            Private _ReferID As System.String
            Private _Residue As System.String
            Private _TransDate As System.DateTime
            Private _TransReasonCode As System.String
            Private _TransRemark As System.String
            Private _TransDecline As System.String
            Private _TransStatus As System.Byte
            Private _Posted As System.Byte
            Private _PostDate As System.DateTime
            Private _EntryID As System.String
            Private _AuthID As System.String
            Private _AuthPOS As System.String
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _ApproveDate As System.DateTime
            Private _DeclineRemark As System.String
            Private _ApproveBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _Active As System.Byte
            Private _Inuse As System.Byte
            Private _RowGuid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property TransNo As System.String
                Get
                    Return _TransNo
                End Get
                Set(ByVal Value As System.String)
                    _TransNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TransType As System.Byte
                Get
                    Return _TransType
                End Get
                Set(ByVal Value As System.Byte)
                    _TransType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CompanyID As System.String
                Get
                    Return _CompanyID
                End Get
                Set(ByVal Value As System.String)
                    _CompanyID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CompanyName As System.String
                Get
                    Return _CompanyName
                End Get
                Set(ByVal Value As System.String)
                    _CompanyName = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property LocID As System.String
                Get
                    Return _LocID
                End Get
                Set(ByVal Value As System.String)
                    _LocID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TermID As System.Int16
                Get
                    Return _TermID
                End Get
                Set(ByVal Value As System.Int16)
                    _TermID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PBT As System.String
                Get
                    Return _PBT
                End Get
                Set(ByVal Value As System.String)
                    _PBT = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property NoteID As System.String
                Get
                    Return _NoteID
                End Get
                Set(ByVal Value As System.String)
                    _NoteID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ReferID As System.String
                Get
                    Return _ReferID
                End Get
                Set(ByVal Value As System.String)
                    _ReferID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TransDate As System.DateTime
                Get
                    Return _TransDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _TransDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TransReasonCode As System.String
                Get
                    Return _TransReasonCode
                End Get
                Set(ByVal Value As System.String)
                    _TransReasonCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TransRemark As System.String
                Get
                    Return _TransRemark
                End Get
                Set(ByVal Value As System.String)
                    _TransRemark = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TransDecline As System.String
                Get
                    Return _TransDecline
                End Get
                Set(ByVal Value As System.String)
                    _TransDecline = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TransStatus As System.Byte
                Get
                    Return _TransStatus
                End Get
                Set(ByVal Value As System.Byte)
                    _TransStatus = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Posted As System.Byte
                Get
                    Return _Posted
                End Get
                Set(ByVal Value As System.Byte)
                    _Posted = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PostDate As System.DateTime
                Get
                    Return _PostDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _PostDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property EntryID As System.String
                Get
                    Return _EntryID
                End Get
                Set(ByVal Value As System.String)
                    _EntryID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property AuthID As System.String
                Get
                    Return _AuthID
                End Get
                Set(ByVal Value As System.String)
                    _AuthID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property AuthPOS As System.String
                Get
                    Return _AuthPOS
                End Get
                Set(ByVal Value As System.String)
                    _AuthPOS = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
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
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ApproveDate As System.DateTime
                Get
                    Return _ApproveDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _ApproveDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property DeclineRemark As System.String
                Get
                    Return _DeclineRemark
                End Get
                Set(ByVal Value As System.String)
                    _DeclineRemark = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ApproveBy As System.String
                Get
                    Return _ApproveBy
                End Get
                Set(ByVal Value As System.String)
                    _ApproveBy = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
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
            Public Property RowGuid As System.Guid
                Get
                    Return _RowGuid
                End Get
                Set(ByVal Value As System.Guid)
                    _RowGuid = Value
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

            'Custom field
            Private _BranchName As System.String

            Public Property BranchName As System.String
                Get
                    Return _BranchName
                End Get
                Set(ByVal Value As System.String)
                    _BranchName = Value
                End Set
            End Property
            Public Property Residue As System.String
                Get
                    Return _Residue
                End Get
                Set(ByVal Value As System.String)
                    _Residue = Value
                End Set
            End Property


        End Class
#End Region

    End Namespace

#Region "Class Info"
    Public Class NotifyhdrInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "TransNo,TransType,CompanyID,(SELECT CompanyName FROM BizEntity WHERE BizRegID=Notifyhdr.CompanyID) AS CompanyName,LocID,TermID,PBT,NoteID,ReferID,TransDate,TransReasonCode,TransRemark,TransStatus,Posted,PostDate,EntryID,AuthID,AuthPOS,CreateDate,CreateBy,ApproveDate, DeclineRemark,ApproveBy,LastUpdate,UpdateBy,Active,Inuse,Flag,RowGuid,SyncCreate,SyncLastUpd"
                .CheckFields = "TransType,TransStatus,Posted,Active,Inuse,Flag"
                .TableName = "Notifyhdr WITH (NOLOCK)"

                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "TransNo,TransType,CompanyID,LocID,TermID,NoteID,ReferID,TransDate,TransReasonCode,TransRemark,TransStatus,Posted,PostDate,EntryID,AuthID,AuthPOS,CreateDate,CreateBy,ApproveDate,DeclineRemark,ApproveBy,LastUpdate,UpdateBy,Active,Inuse,Flag,RowGuid,SyncCreate,SyncLastUpd"
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
    Public Class NotificationScheme
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
                .FieldName = "TransType"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CompanyID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LocID"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtCustom
                .FieldName = "TermID"
                .Length = 2
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "PBT"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "NoteID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ReferID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "TransDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "TransReasonCode"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "TransRemark"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "TransStatus"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Posted"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "PostDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "EntryID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "AuthID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "AuthPOS"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "ApproveDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ApproveBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(21, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(22, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(23, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Inuse"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(24, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(25, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "RowGuid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(26, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(27, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(28, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Wastecomponent"
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(29, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Wastesource"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(30, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "WasteName"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(31, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "DeclineRemark"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(32, this)

        End Sub

        Public ReadOnly Property TransNo As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property TransType As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property CompanyID As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property LocID As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property TermID As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property PBT As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property NoteID As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property ReferID As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property TransDate As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property TransReasonCode As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property TransRemark As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property TransStatus As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property Posted As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property PostDate As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property EntryID As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property AuthID As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property AuthPOS As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property ApproveDate As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property ApproveBy As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(22)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(23)
            End Get
        End Property
        Public ReadOnly Property Inuse As StrucElement
            Get
                Return MyBase.GetItem(24)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(25)
            End Get
        End Property
        Public ReadOnly Property RowGuid As StrucElement
            Get
                Return MyBase.GetItem(26)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(27)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(28)
            End Get
        End Property

        Public ReadOnly Property Wastecomponent As StrucElement
            Get
                Return MyBase.GetItem(29)
            End Get
        End Property
        Public ReadOnly Property WasteSource As StrucElement
            Get
                Return MyBase.GetItem(30)
            End Get
        End Property
        Public ReadOnly Property Wastename As StrucElement
            Get
                Return MyBase.GetItem(31)
            End Get
        End Property
        Public ReadOnly Property DeclineRemark As StrucElement
            Get
                Return MyBase.GetItem(32)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class

#End Region

End Namespace
