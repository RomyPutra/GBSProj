Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Actions
    Public NotInheritable Class NotificationDetails
        Inherits Core.CoreControl
        Private NotifydtlInfo As NotifydtlInfo = New NotifydtlInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"

        Private Function Save(ByVal NotifydtlCont As Container.Notifydtl, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If NotifydtlCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With NotifydtlInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "TransNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifydtlCont.TransNo) & "' AND TransSeq = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, NotifydtlCont.TransSeq) & "'")
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
                                .TableName = "Notifydtl WITH (ROWLOCK)"
                                .AddField("CompanyID", NotifydtlCont.CompanyID, SQLControl.EnumDataType.dtString)
                                .AddField("LocID", NotifydtlCont.LocID, SQLControl.EnumDataType.dtString)
                                .AddField("TermID", NotifydtlCont.TermID, SQLControl.EnumDataType.dtCustom)
                                .AddField("RecType", NotifydtlCont.RecType, SQLControl.EnumDataType.dtNumeric)
                                .AddField("RefSeq", NotifydtlCont.RefSeq, SQLControl.EnumDataType.dtNumeric)
                                .AddField("DisplayType", NotifydtlCont.DisplayType, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsCal", NotifydtlCont.IsCal, SQLControl.EnumDataType.dtNumeric)
                                .AddField("RefNo", NotifydtlCont.RefNo, SQLControl.EnumDataType.dtStringN)
                                .AddField("ItemCode", NotifydtlCont.ItemCode, SQLControl.EnumDataType.dtString)
                                .AddField("ItmDesc", NotifydtlCont.ItmDesc, SQLControl.EnumDataType.dtStringN)
                                .AddField("BehvType", NotifydtlCont.BehvType, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Qty", NotifydtlCont.Qty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("PackQty", NotifydtlCont.PackQty, SQLControl.EnumDataType.dtNumeric)
                                .AddField("UOM", NotifydtlCont.UOM, SQLControl.EnumDataType.dtString)
                                .AddField("ItmName", NotifydtlCont.ItmName, SQLControl.EnumDataType.dtStringN)
                                .AddField("ItmSource", NotifydtlCont.ItmSource, SQLControl.EnumDataType.dtStringN)
                                .AddField("ItmComponent", NotifydtlCont.ItmComponent, SQLControl.EnumDataType.dtStringN)
                                .AddField("SerialNo", NotifydtlCont.SerialNo, SQLControl.EnumDataType.dtStringN)
                                .AddField("TransVoid", NotifydtlCont.TransVoid, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Posted", NotifydtlCont.Posted, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Status", NotifydtlCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", NotifydtlCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", NotifydtlCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", NotifydtlCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", NotifydtlCont.UpdateBy, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TransNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifydtlCont.TransNo) & "' AND TransSeq = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, NotifydtlCont.TransSeq) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("TransNo", NotifydtlCont.TransNo, SQLControl.EnumDataType.dtString)
                                                .AddField("TransSeq", NotifydtlCont.TransSeq, SQLControl.EnumDataType.dtNumeric)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TransNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifydtlCont.TransNo) & "' AND TransSeq = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, NotifydtlCont.TransSeq) & "'")
                                End Select
                            End With
                            Try
                                objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                Return True

                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                Else
                                    message = axExecute.Message.ToString()
                                End If
                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/NotificationDetails", axExecute.Message & strSQL, axExecute.StackTrace)
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
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/NotificationDetails", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/NotificationDetails", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                NotifydtlCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Private Function BatchSave(ByVal ListNotifydtlCont As List(Of Container.Notifydtl), ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()

            BatchSave = False
            Try
                If ListNotifydtlCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()

                        If ListNotifydtlCont.Count > 0 Then
                            strSQL = BuildDelete("Notifydtl WITH (ROWLOCK)", "TransNo = '" & ListNotifydtlCont(0).TransNo & "'")
                            ListSQL.Add(strSQL)
                        End If

                    End If

                    For Each NotifydtlCont In ListNotifydtlCont
                        With objSQL
                            .TableName = "Notifydtl WITH (ROWLOCK)"
                            .AddField("CompanyID", NotifydtlCont.CompanyID, SQLControl.EnumDataType.dtString)
                            .AddField("LocID", NotifydtlCont.LocID, SQLControl.EnumDataType.dtString)
                            .AddField("StorageID", NotifydtlCont.StorageID, SQLControl.EnumDataType.dtString)
                            .AddField("TermID", NotifydtlCont.TermID, SQLControl.EnumDataType.dtCustom)
                            .AddField("RecType", NotifydtlCont.RecType, SQLControl.EnumDataType.dtNumeric)
                            .AddField("RefSeq", NotifydtlCont.RefSeq, SQLControl.EnumDataType.dtNumeric)
                            .AddField("DisplayType", NotifydtlCont.DisplayType, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsCal", NotifydtlCont.IsCal, SQLControl.EnumDataType.dtNumeric)
                            .AddField("RefNo", NotifydtlCont.RefNo, SQLControl.EnumDataType.dtStringN)
                            .AddField("ItemCode", NotifydtlCont.ItemCode, SQLControl.EnumDataType.dtString)
                            .AddField("ItmDesc", NotifydtlCont.ItmDesc, SQLControl.EnumDataType.dtStringN)
                            .AddField("BehvType", NotifydtlCont.BehvType, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Qty", NotifydtlCont.Qty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("PackQty", NotifydtlCont.PackQty, SQLControl.EnumDataType.dtNumeric)
                            .AddField("UOM", NotifydtlCont.UOM, SQLControl.EnumDataType.dtString)
                            .AddField("ItmName", NotifydtlCont.ItmName, SQLControl.EnumDataType.dtStringN)
                            .AddField("ItmSource", NotifydtlCont.ItmSource, SQLControl.EnumDataType.dtStringN)
                            .AddField("ItmComponent", NotifydtlCont.ItmComponent, SQLControl.EnumDataType.dtStringN)
                            .AddField("SerialNo", NotifydtlCont.SerialNo, SQLControl.EnumDataType.dtStringN)
                            .AddField("TransVoid", NotifydtlCont.TransVoid, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Posted", NotifydtlCont.Posted, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Status", NotifydtlCont.Status, SQLControl.EnumDataType.dtNumeric)
                            .AddField("CreateDate", NotifydtlCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("CreateBy", NotifydtlCont.CreateBy, SQLControl.EnumDataType.dtString)
                            .AddField("LastUpdate", NotifydtlCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("UpdateBy", NotifydtlCont.UpdateBy, SQLControl.EnumDataType.dtString)

                            Select Case pType
                                Case SQLControl.EnumSQLType.stInsert
                                    .AddField("TransNo", NotifydtlCont.TransNo, SQLControl.EnumDataType.dtString)
                                    .AddField("TransSeq", NotifydtlCont.TransSeq, SQLControl.EnumDataType.dtNumeric)
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                Case SQLControl.EnumSQLType.stUpdate
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TransNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifydtlCont.TransNo) & "' AND TransSeq = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, NotifydtlCont.TransSeq) & "'")
                            End Select
                        End With

                        ListSQL.Add(strSQL)
                    Next

                    Try
                        objConn.BatchExecute(ListSQL, CommandType.Text)
                    Catch axExecute As Exception
                        Dim sqlStatement As String = " "
                        If objConn.FailedSQLStatement.Count > 0 Then
                            sqlStatement &= objConn.FailedSQLStatement.Item(0)
                        End If

                        If pType = SQLControl.EnumSQLType.stInsert Then
                            message = axExecute.Message.ToString()
                        Else
                            message = axExecute.Message.ToString()
                        End If

                        Log.Notifier.Notify(axExecute)
                        Gibraltar.Agent.Log.Error("eSWISLogic/Actions/NotificationDetails", axExecute.Message & sqlStatement, axExecute.StackTrace)
                    Finally
                        objSQL.Dispose()
                    End Try
                    Return True

                End If
            Catch axAssign As ApplicationException
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/NotificationDetails", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/NotificationDetails", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                ListNotifydtlCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal NotifydtlCont As Container.Notifydtl, ByRef message As String) As Boolean
            Return Save(NotifydtlCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal NotifydtlCont As Container.Notifydtl, ByRef message As String) As Boolean
            Return Save(NotifydtlCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        'BATCH ADD
        Public Function BatchInsert(ByVal ListNotifydtlCont As List(Of Container.Notifydtl), ByRef message As String) As Boolean
            Return BatchSave(ListNotifydtlCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'BATCH AMEND
        Public Function BatchUpdate(ByVal ListNotifydtlCont As List(Of Container.Notifydtl), ByRef message As String) As Boolean
            Return BatchSave(ListNotifydtlCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal NotifydtlCont As Container.Notifydtl, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If NotifydtlCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With NotifydtlInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "TransNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifydtlCont.TransNo) & "' AND TransSeq = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, NotifydtlCont.TransSeq) & "'")
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
                                strSQL = BuildUpdate("Notifydtl WITH (ROWLOCK)", " SET Flag = 0" & _
                                " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, NotifydtlCont.UpdateBy) & "' WHERE" & _
                                "TransNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifydtlCont.TransNo) & "' AND TransSeq = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, NotifydtlCont.TransSeq) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("Notifydtl WITH (ROWLOCK)", "TransNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, NotifydtlCont.TransNo) & "' AND TransSeq = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, NotifydtlCont.TransSeq) & "'")
                        End If

                        Try
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/Actions/NotificationDetails", exExecute.Message & strSQL, exExecute.StackTrace)
                            Return False
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/NotificationDetails", axDelete.Message, axDelete.StackTrace)
                Return False
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/NotificationDetails", exDelete.Message, exDelete.StackTrace)
                Return False
            Finally
                NotifydtlCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetNotificationDetails(ByVal TransNo As System.String, ByVal TransSeq As System.Int32) As Container.Notifydtl
            Dim rNotifydtl As Container.Notifydtl = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With NotifydtlInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "TransNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, TransNo) & "' AND TransSeq = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, TransSeq) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rNotifydtl = New Container.Notifydtl
                                rNotifydtl.TransNo = drRow.Item("TransNo")
                                rNotifydtl.TransSeq = drRow.Item("TransSeq")
                                rNotifydtl.CompanyID = drRow.Item("CompanyID")
                                rNotifydtl.LocID = drRow.Item("LocID")
                                rNotifydtl.TermID = drRow.Item("TermID")
                                rNotifydtl.RecType = drRow.Item("RecType")
                                rNotifydtl.RefSeq = drRow.Item("RefSeq")
                                rNotifydtl.DisplayType = drRow.Item("DisplayType")
                                rNotifydtl.IsCal = drRow.Item("IsCal")
                                rNotifydtl.RefNo = drRow.Item("RefNo")
                                rNotifydtl.ItemCode = drRow.Item("ItemCode")
                                rNotifydtl.ItmDesc = drRow.Item("ItmDesc")
                                rNotifydtl.TypeCode = drRow.Item("")
                                rNotifydtl.BehvType = drRow.Item("BehvType")
                                rNotifydtl.Qty = drRow.Item("Qty")
                                rNotifydtl.PackQty = drRow.Item("PackQty")
                                rNotifydtl.UOM = drRow.Item("UOMTypeCode")
                                rNotifydtl.ItmSource = drRow.Item("ItmSource")
                                rNotifydtl.ItmComponent = drRow.Item("ItmComponent")
                                rNotifydtl.SerialNo = drRow.Item("SerialNo")
                                rNotifydtl.TransVoid = drRow.Item("TransVoid")
                                rNotifydtl.Posted = drRow.Item("Posted")
                                rNotifydtl.Status = drRow.Item("Status")
                                rNotifydtl.RowGuid = drRow.Item("RowGuid")
                                rNotifydtl.CreateDate = drRow.Item("CreateDate")
                                rNotifydtl.CreateBy = drRow.Item("CreateBy")
                                rNotifydtl.LastUpdate = drRow.Item("LastUpdate")
                                rNotifydtl.UpdateBy = drRow.Item("UpdateBy")
                                rNotifydtl.SyncCreate = drRow.Item("SyncCreate")
                                rNotifydtl.SyncLastUpd = drRow.Item("SyncLastUpd")
                            Else
                                rNotifydtl = Nothing
                            End If
                        Else
                            rNotifydtl = Nothing
                        End If
                    End With
                End If
                Return rNotifydtl
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/NotificationDetails", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rNotifydtl = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetNotificationDetails(ByVal TransNo As System.String, ByVal DecendingOrder As Boolean) As List(Of Container.Notifydtl)
            Dim rNotifydtl As Container.Notifydtl = Nothing
            Dim lstNotifydtl As List(Of Container.Notifydtl) = New List(Of Actions.Container.Notifydtl)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With NotifydtlInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by TransNo, TransSeq DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "TransNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, TransNo) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rNotifydtl = New Container.Notifydtl
                                rNotifydtl.TransNo = drRow.Item("TransNo")
                                rNotifydtl.TransSeq = drRow.Item("TransSeq")
                                rNotifydtl.CompanyID = drRow.Item("CompanyID")
                                rNotifydtl.LocID = drRow.Item("LocID")
                                rNotifydtl.TermID = drRow.Item("TermID")
                                rNotifydtl.RecType = drRow.Item("RecType")
                                rNotifydtl.RefSeq = drRow.Item("RefSeq")
                                rNotifydtl.DisplayType = drRow.Item("DisplayType")
                                rNotifydtl.IsCal = drRow.Item("IsCal")
                                rNotifydtl.RefNo = drRow.Item("RefNo")
                                rNotifydtl.ItemCode = drRow.Item("ItemCode")
                                rNotifydtl.ItmDesc = drRow.Item("ItmDesc")
                                rNotifydtl.ItmName = drRow.Item("ItmName")
                                rNotifydtl.TypeCode = drRow.Item("TypeCode")
                                rNotifydtl.BehvType = drRow.Item("BehvType")
                                rNotifydtl.Qty = drRow.Item("Qty")
                                rNotifydtl.PackQty = drRow.Item("PackQty")
                                rNotifydtl.UOM = drRow.Item("UOM")
                                rNotifydtl.ItmSource = drRow.Item("ItmSource")
                                rNotifydtl.ItmComponent = drRow.Item("ItmComponent")
                                rNotifydtl.SerialNo = drRow.Item("SerialNo")
                                rNotifydtl.TransVoid = drRow.Item("TransVoid")

                                lstNotifydtl.Add(rNotifydtl)
                            Next
                        Else
                            rNotifydtl = Nothing
                        End If
                        Return lstNotifydtl
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/NotificationDetails", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rNotifydtl = Nothing
                lstNotifydtl = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function GetNotificationDtl(ByVal TransNo As String, ByVal RecType As Byte) As List(Of Actions.Container.Notifydtl)
            Dim rNotifyDTL As Container.Notifydtl = Nothing
            Dim listNotifyDTL As List(Of Container.Notifydtl) = Nothing
            Dim dtTemp As DataTable = Nothing
            If StartConnection() = True Then
                StartSQLControl()
                strSQL = "select d.LocID, d.ItemCode, d.ItmName, d.ItmDesc, d.ItmComponent, d.StorageID, d.UOM, c.CodeDesc as ItemType, d.ItmSource, d.Qty, isnull(uom.UOMDesc, '') as PackageType" & _
                            " from NOTIFYDTL d with (nolock) left join UOM with (nolock) on d.UOM=uom.UOMCode" & _
                            " left join (select Code, CodeDesc from CODEMASTER with (nolock) where CodeType='WTY') c on c.Code = d.TypeCode" & _
                            " where d.TransNo='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, TransNo) & _
                            "' and d.RecType=" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, RecType)
                Try
                    dtTemp = objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text)
                    If dtTemp IsNot Nothing AndAlso dtTemp.Rows.Count > 0 Then
                        listNotifyDTL = New List(Of Container.Notifydtl)
                        For Each rowDTL As DataRow In dtTemp.Rows
                            rNotifyDTL = New Container.Notifydtl
                            With rNotifyDTL
                                .LocID = rowDTL.Item("LocID").ToString
                                .ItemCode = rowDTL.Item("ItemCode").ToString
                                .ItmName = rowDTL.Item("ItmName").ToString
                                .ItmDesc = rowDTL.Item("ItmDesc").ToString
                                .ItmComponent = rowDTL.Item("ItmComponent").ToString
                                .StorageID = rowDTL.Item("StorageID").ToString
                                .UOM = rowDTL.Item("UOM").ToString
                                .ItemType = rowDTL.Item("ItemType").ToString
                                .ItmSource = rowDTL.Item("ItmSource").ToString
                                .Qty = rowDTL.Item("Qty").ToString
                                .PackageType = rowDTL.Item("PackageType").ToString
                            End With
                            listNotifyDTL.Add(rNotifyDTL)
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
            Return listNotifyDTL
        End Function

        Public Overloads Function GetNotificationDetailsList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With NotifydtlInfo.MyInfo
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

        Public Overloads Function GetNotificationDetailsShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With NotifydtlInfo.MyInfo
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

        'Add
        Public Overloads Function GetNotificationDetailsCustomList(Optional ByVal TransNo As String = Nothing, Optional ByVal RecType As String = Nothing, Optional ByVal ItemCode As String = Nothing, Optional ByVal ItmName As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With NotifydtlInfo.MyInfo
                    StartSQLControl()

                    'amended by diana 20150120, replace '' with null
                    strSQL = "SELECT D.*, isnull((SELECT TOP 1 StorageAreaCode FROM StorageMaster where LocID=D.LocID AND StorageID=D.StorageID),'') as StorageAreaCode, " & _
                            "isnull((SELECT TOP 1 StorageBin FROM StorageMaster where LocID=D.LocID AND StorageID=D.StorageID),'') as StorageBin,RefSeq AS RecID " & _
                             ", ISNULL(UOM.UOMDesc,'') as PackDesc, ISNULL(UOM.UOMCode,'') as PackUOM," & _
                             " isnull((select CodeDesc from CODEMASTER where CodeType='WTY' and Active=1 and Code=D.typeCode),'') as TypeDesc, displaytype as Residue  " & _
                             " FROM NOTIFYDTL D LEFT JOIN UOM ON UOM.UOMCode = D.UOM And UOM.Flag = 1 WHERE D.CreateDate is not null "

                    If Not TransNo Is Nothing And TransNo <> "" Then
                        strSQL &= " AND TransNo='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, TransNo) & "'"
                    End If

                    If RecType IsNot Nothing And RecType <> "" Then
                        strSQL &= " AND RecType='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, RecType) & "'"
                    End If

                    If ItemCode IsNot Nothing And ItemCode <> "" Then
                        strSQL &= " AND ItemCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, ItemCode) & "'"
                    End If

                    If ItmName IsNot Nothing And ItmName <> "" Then
                        strSQL &= " AND ItmName='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, ItmName) & "' AND TypeCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, ItmName) & "'"
                    End If


                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetNotificationDetailsExport(Optional ByVal Condition As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With NotifydtlInfo.MyInfo

                    'amended by diana 20150120, replace '' with null
                    strSQL = "SELECT *, (SELECT CompanyName FROM BizEntity WHERE BizRegID=H.CompanyID) AS CompanyName " & _
                            "FROM Notifyhdr H WITH (NOLOCK) " & _
                            "INNER JOIN notifydtl d WITH (NOLOCK) on H.transno=d.transno "

                    If Not Condition Is Nothing And Condition <> "" Then strSQL &= " WHERE " & Condition

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetPieChartDashboardCustomList(Optional ByVal Condition As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With NotifydtlInfo.MyInfo

                    strSQL = "select s.ItemCode as WasteCode, sum(i.qtyonhand) as Qty " & _
                    " from itemsummary s " & _
                    " INNER JOIN ITEMLOC i on i.locid=s.locid and i.itemcode=s.itemcode and i.itemname=s.itemname and i.flag=1 " & _
                    " where s.ItemCode <> '' and s.MthCode='" & Now.Month & "' and s.YearCode='" & Now.Year & "' " & Condition & _
                    " group by s.ItemCode"

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        'Add for PieChart
        Public Overloads Function GetPieChartCustomList(Optional ByVal Condition As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With NotifydtlInfo.MyInfo
                    strSQL = "select ItemCode as WasteCode, sum(NotifyDtl.Qty) as Qty from NOTIFYHDR,NOTIFYDTL where notifyhdr.posted = 1 and notifyhdr.flag=1 and notifyhdr.TransNo=notifydtl.transno and ItemCode <> '' " & Condition & "  group by notifydtl.ItemCode"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Function GetWasteCode(ByVal ItemCode As String, ByVal ItemName As String, ByVal LocID As String, ByVal Mass As String) As Boolean
            Dim dtTemp As DataTable
            StartSQLControl()
            Try
                strSQL = "SELECT d.ItemCode,d.ItmName FROM NOTIFYDTL d INNER JOIN NOTIFYHDR h ON d.TRANSNO=h.TRANSNO" & _
                            " WHERE d.ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "' AND d.ItmName = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemName) & "'  AND d.LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' AND h.FLAG='1' AND d.displaytype='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Mass) & "'"

                dtTemp = objConn.Execute(strSQL, CommandType.Text)

                If dtTemp Is Nothing = False Then
                    If dtTemp.Rows.Count > 0 Then
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If

            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/Notification", ex.Message, ex.StackTrace)
            Finally
                EndSQLControl()
            End Try
        End Function

        Public Function GetWasteCode_duplicateName(ByVal ItemCode As String, ByVal ItemName As String, ByVal LocID As String) As Boolean
            Dim dtTemp As DataTable
            Try
                StartSQLControl()
                strSQL = "SELECT d.ItemCode,d.ItmName FROM NOTIFYDTL d INNER JOIN NOTIFYHDR h ON d.TRANSNO=h.TRANSNO" & _
                            " WHERE d.ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemCode) & "' AND d.ItmName = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ItemName) & "'  AND d.LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' AND h.FLAG='1' "

                dtTemp = objConn.Execute(strSQL, CommandType.Text)

                If dtTemp Is Nothing = False Then
                    If dtTemp.Rows.Count > 0 Then
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If

            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Actions/Notification", ex.Message, ex.StackTrace)
            Finally
                EndSQLControl()
            End Try
        End Function

        Public Overloads Function GetNotifByLocID(ByVal LocID As String, ByVal WasteCode As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With NotifydtlInfo.MyInfo
                    strSQL = "SELECT * FROM NOTIFYHDR NHD INNER JOIN NOTIFYDTL NDT ON NHD.TransNo = NDT.TransNo WHERE NHD.LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' AND NDT.ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteCode) & "'"
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
        Public Class Notifydtl
            Public fTransNo As System.String = "TransNo"
            Public fTransSeq As System.String = "TransSeq"
            Public fCompanyID As System.String = "CompanyID"
            Public fLocID As System.String = "LocID"
            Public fStorageID As System.String = "StorageID"
            Public fTermID As System.String = "TermID"
            Public fRecType As System.String = "RecType"
            Public fRefSeq As System.String = "RefSeq"
            Public fDisplayType As System.String = "DisplayType"
            Public fIsCal As System.String = "IsCal"
            Public fRefNo As System.String = "RefNo"
            Public fItemCode As System.String = "ItemCode"
            Public fItmDesc As System.String = "ItmDesc"
            Public fTypeCode As System.String = "TypeCode"
            Public fBehvType As System.String = "BehvType"
            Public fQty As System.String = "Qty"
            Public fPackQty As System.String = "PackQty"
            Public fUOM As System.String = "UOM"
            Public fItmName As System.String = "ItmName"
            Public fItmSource As System.String = "ItmSource"
            Public fItmComponent As System.String = "ItmComponent"
            Public fSerialNo As System.String = "SerialNo"
            Public fTransVoid As System.String = "TransVoid"
            Public fPosted As System.String = "Posted"
            Public fStatus As System.String = "Status"
            Public fRowGuid As System.String = "RowGuid"
            Public fCreateDate As System.String = "CreateDate"
            Public fCreateBy As System.String = "CreateBy"
            Public fLastUpdate As System.String = "LastUpdate"
            Public fUpdateBy As System.String = "UpdateBy"
            Public fSyncCreate As System.String = "SyncCreate"
            Public fSyncLastUpd As System.String = "SyncLastUpd"

            Protected _TransNo As System.String
            Protected _TransSeq As System.Int32
            Private _CompanyID As System.String
            Private _LocID As System.String
            Private _StorageID As System.String
            Private _TermID As System.Int16
            Private _RecType As System.Byte
            Private _RefSeq As System.Int32
            Private _DisplayType As System.Byte
            Private _IsCal As System.Byte
            Private _RefNo As System.String
            Private _ItemCode As System.String
            Private _TypeCode As System.String
            Private _ItmDesc As System.String
            Private _BehvType As System.Byte
            Private _Qty As System.Decimal
            Private _PackQty As System.Decimal
            Private _UOM As System.String
            Private _ItmName As System.String
            Private _ItmSource As System.String
            Private _ItmComponent As System.String
            Private _SerialNo As System.String
            Private _TransVoid As System.Byte
            Private _Posted As System.Byte
            Private _Status As System.Byte
            Private _RowGuid As System.Guid
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
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
            ''' Mandatory
            ''' </summary>
            Public Property TransSeq As System.Int32
                Get
                    Return _TransSeq
                End Get
                Set(ByVal Value As System.Int32)
                    _TransSeq = Value
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
            Public Property StorageID As System.String
                Get
                    Return _StorageID
                End Get
                Set(ByVal Value As System.String)
                    _StorageID = Value
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
            Public Property RecType As System.Byte
                Get
                    Return _RecType
                End Get
                Set(ByVal Value As System.Byte)
                    _RecType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RefSeq As System.Int32
                Get
                    Return _RefSeq
                End Get
                Set(ByVal Value As System.Int32)
                    _RefSeq = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property DisplayType As System.Byte
                Get
                    Return _DisplayType
                End Get
                Set(ByVal Value As System.Byte)
                    _DisplayType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property IsCal As System.Byte
                Get
                    Return _IsCal
                End Get
                Set(ByVal Value As System.Byte)
                    _IsCal = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RefNo As System.String
                Get
                    Return _RefNo
                End Get
                Set(ByVal Value As System.String)
                    _RefNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ItemCode As System.String
                Get
                    Return _ItemCode
                End Get
                Set(ByVal Value As System.String)
                    _ItemCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ItmDesc As System.String
                Get
                    Return _ItmDesc
                End Get
                Set(ByVal Value As System.String)
                    _ItmDesc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property BehvType As System.Byte
                Get
                    Return _BehvType
                End Get
                Set(ByVal Value As System.Byte)
                    _BehvType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Qty As System.Decimal
                Get
                    Return _Qty
                End Get
                Set(ByVal Value As System.Decimal)
                    _Qty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PackQty As System.Decimal
                Get
                    Return _PackQty
                End Get
                Set(ByVal Value As System.Decimal)
                    _PackQty = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property UOM As System.String
                Get
                    Return _UOM
                End Get
                Set(ByVal Value As System.String)
                    _UOM = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property ItmName As System.String
                Get
                    Return _ItmName
                End Get
                Set(ByVal Value As System.String)
                    _ItmName = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ItmSource As System.String
                Get
                    Return _ItmSource
                End Get
                Set(ByVal Value As System.String)
                    _ItmSource = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ItmComponent As System.String
                Get
                    Return _ItmComponent
                End Get
                Set(ByVal Value As System.String)
                    _ItmComponent = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property SerialNo As System.String
                Get
                    Return _SerialNo
                End Get
                Set(ByVal Value As System.String)
                    _SerialNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TransVoid As System.Byte
                Get
                    Return _TransVoid
                End Get
                Set(ByVal Value As System.Byte)
                    _TransVoid = Value
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
            Public Property TypeCode As System.String
                Get
                    Return _TypeCode
                End Get
                Set(ByVal Value As System.String)
                    _TypeCode = Value
                End Set
            End Property

            'Custom Field
            Private _ItemType As System.String
            Private _PackageType As System.String

            Public Property ItemType As System.String
                Get
                    Return _ItemType
                End Get
                Set(ByVal Value As System.String)
                    _ItemType = Value
                End Set
            End Property

            Public Property PackageType As System.String
                Get
                    Return _PackageType
                End Get
                Set(ByVal Value As System.String)
                    _PackageType = Value
                End Set
            End Property

        End Class
#End Region
    End Namespace

#Region "Class Info"
    Public Class NotifydtlInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "TransNo,TransSeq,CompanyID,LocID,TermID,RecType,RefSeq,DisplayType,IsCal,RefNo,ItemCode,ItmDesc,TypeCode,BehvType,Qty,PackQty,UOM,ItmName,ItmSource,ItmComponent,SerialNo,TransVoid,Posted,Status,RowGuid,CreateDate,CreateBy,LastUpdate,UpdateBy,SyncCreate,SyncLastUpd"
                .CheckFields = "RecType,DisplayType,IsCal,BehvType,TransVoid,Posted,Status"
                .TableName = "Notifydtl WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "TransNo,TransSeq,CompanyID,LocID,TermID,RecType,RefSeq,DisplayType,IsCal,RefNo,ItemCode,ItmDesc,TypeCode,BehvType,Qty,PackQty,UOM,ItmName,ItmSource,ItmComponent,SerialNo,TransVoid,Posted,Status,RowGuid,CreateDate,CreateBy,LastUpdate,UpdateBy,SyncCreate,SyncLastUpd"
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
    Public Class NotificationDetailsScheme
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
                .FieldName = "TransSeq"
                .Length = 4
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
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "RecType"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "RefSeq"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "DisplayType"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsCal"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "RefNo"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ItemCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ItmDesc"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "BehvType"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Qty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "PackQty"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UOM"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ItmName"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ItmSource"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ItmComponent"
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "SerialNo"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "TransVoid"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Posted"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(21, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Status"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(22, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "RowGuid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(23, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(24, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(25, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(26, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(27, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(28, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(29, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "TypeCode"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(30, this)

        End Sub

        Public ReadOnly Property TransNo As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property
        Public ReadOnly Property TransSeq As StrucElement
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
        Public ReadOnly Property RecType As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property RefSeq As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property DisplayType As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property IsCal As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property RefNo As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property ItemCode As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property ItmDesc As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property BehvType As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property Qty As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property PackQty As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property UOM As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property ItmName As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property ItmSource As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property ItmComponent As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property SerialNo As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property TransVoid As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property
        Public ReadOnly Property Posted As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property
        Public ReadOnly Property Status As StrucElement
            Get
                Return MyBase.GetItem(22)
            End Get
        End Property
        Public ReadOnly Property RowGuid As StrucElement
            Get
                Return MyBase.GetItem(23)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(24)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(25)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(26)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(27)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(28)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(29)
            End Get
        End Property

        Public ReadOnly Property TypeCode As StrucElement
            Get
                Return MyBase.GetItem(30)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region

End Namespace