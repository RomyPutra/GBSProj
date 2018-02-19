Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared

Namespace TWG
#Region "TWG_TEMPLATE Class"
    Public NotInheritable Class TWG_TEMPLATE
        Inherits Core.CoreControl
        Private Twg_templateInfo As Twg_templateInfo = New Twg_templateInfo

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal Twg_templateCont As Container.Twg_template, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If Twg_templateCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Twg_templateInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "TemplateID = '" & Twg_templateCont.TemplateID & "' AND ReceiverID = '" & Twg_templateCont.ReceiverID & "' AND ReceiverLocID = '" & Twg_templateCont.ReceiverLocID & "' AND WasteCode = '" & Twg_templateCont.WasteCode & "' AND WasteType = '" & Twg_templateCont.WasteType & "' AND WasteName = '" & Twg_templateCont.WasteName & "'")
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
                                .TableName = "Twg_template"
                                .AddField("SeqNo", Twg_templateCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", Twg_templateCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", Twg_templateCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", Twg_templateCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", Twg_templateCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", Twg_templateCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", Twg_templateCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("SyncCreate", Twg_templateCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("SyncLastUpd", Twg_templateCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                                .AddField("LastSyncBy", Twg_templateCont.LastSyncBy, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TemplateID = '" & Twg_templateCont.TemplateID & "' AND ReceiverID = '" & Twg_templateCont.ReceiverID & "' AND ReceiverLocID = '" & Twg_templateCont.ReceiverLocID & "' AND WasteCode = '" & Twg_templateCont.WasteCode & "' AND WasteType = '" & Twg_templateCont.WasteType & "' AND WasteName = '" & Twg_templateCont.WasteName & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("TemplateID", Twg_templateCont.TemplateID, SQLControl.EnumDataType.dtString)
                                                .AddField("ReceiverID", Twg_templateCont.ReceiverID, SQLControl.EnumDataType.dtString)
                                                .AddField("ReceiverLocID", Twg_templateCont.ReceiverLocID, SQLControl.EnumDataType.dtString)
                                                .AddField("WasteCode", Twg_templateCont.WasteCode, SQLControl.EnumDataType.dtString)
                                                .AddField("WasteType", Twg_templateCont.WasteType, SQLControl.EnumDataType.dtString)
                                                .AddField("WasteName", Twg_templateCont.WasteName, SQLControl.EnumDataType.dtStringN)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TemplateID = '" & Twg_templateCont.TemplateID & "' AND ReceiverID = '" & Twg_templateCont.ReceiverID & "' AND ReceiverLocID = '" & Twg_templateCont.ReceiverLocID & "' AND WasteCode = '" & Twg_templateCont.WasteCode & "' AND WasteType = '" & Twg_templateCont.WasteType & "' AND WasteName = '" & Twg_templateCont.WasteName & "'")
                                End Select
                            End With
                            Try
                                If BatchExecute Then
                                    BatchList.Add(strSQL)
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

                    End If
                End If
            Catch axAssign As ApplicationException
                message = axAssign.Message.ToString()
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Return False
            Finally
                Twg_templateCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Private Function Insert(ByVal Twg_templateCont As Container.Twg_template, ByVal ListContTWG_Dtl As List(Of Container.Twg_templatedtl), ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            BatchList = New ArrayList()
            Insert = False
            Try
                If Twg_templateCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Twg_templateInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "TemplateID = '" & Twg_templateCont.TemplateID & "' AND ReceiverID = '" & Twg_templateCont.ReceiverID & "' AND ReceiverLocID = '" & Twg_templateCont.ReceiverLocID & "' AND WasteCode = '" & Twg_templateCont.WasteCode & "' AND WasteType = '" & Twg_templateCont.WasteType & "' AND WasteName = '" & Twg_templateCont.WasteName & "'")
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
                            strSQL = "UPDATE TWG_TEMPLATE SET FLAG='0'"
                            BatchList.Add(strSQL)
                            With objSQL
                                .TableName = "Twg_template"
                                .AddField("SeqNo", Twg_templateCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", Twg_templateCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", Twg_templateCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", Twg_templateCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", Twg_templateCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", Twg_templateCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", Twg_templateCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("SyncCreate", Twg_templateCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("SyncLastUpd", Twg_templateCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                                .AddField("LastSyncBy", Twg_templateCont.LastSyncBy, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TemplateID = '" & Twg_templateCont.TemplateID & "' AND ReceiverID = '" & Twg_templateCont.ReceiverID & "' AND ReceiverLocID = '" & Twg_templateCont.ReceiverLocID & "' AND WasteCode = '" & Twg_templateCont.WasteCode & "' AND WasteType = '" & Twg_templateCont.WasteType & "' AND WasteName = '" & Twg_templateCont.WasteName & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("TemplateID", Twg_templateCont.TemplateID, SQLControl.EnumDataType.dtString)
                                                .AddField("ReceiverID", Twg_templateCont.ReceiverID, SQLControl.EnumDataType.dtString)
                                                .AddField("ReceiverLocID", Twg_templateCont.ReceiverLocID, SQLControl.EnumDataType.dtString)
                                                .AddField("WasteCode", Twg_templateCont.WasteCode, SQLControl.EnumDataType.dtString)
                                                .AddField("WasteType", Twg_templateCont.WasteType, SQLControl.EnumDataType.dtString)
                                                .AddField("WasteName", Twg_templateCont.WasteName, SQLControl.EnumDataType.dtStringN)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TemplateID = '" & Twg_templateCont.TemplateID & "' AND ReceiverID = '" & Twg_templateCont.ReceiverID & "' AND ReceiverLocID = '" & Twg_templateCont.ReceiverLocID & "' AND WasteCode = '" & Twg_templateCont.WasteCode & "' AND WasteType = '" & Twg_templateCont.WasteType & "' AND WasteName = '" & Twg_templateCont.WasteName & "'")
                                End Select
                                BatchList.Add(strSQL)
                            End With

                            If ListContTWG_Dtl Is Nothing Then
                                'Message return
                            Else
                                blnExec = False
                                blnFound = False
                                blnFlag = False

                                For Each Twg_templatedtlCont In ListContTWG_Dtl
                                    StartSQLControl()
                                    With objSQL
                                        .TableName = "Twg_templatedtl"
                                        .AddField("SeqNo", Twg_templatedtlCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("CreateDate", Twg_templatedtlCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                        .AddField("CreateBy", Twg_templatedtlCont.CreateBy, SQLControl.EnumDataType.dtString)
                                        .AddField("LastUpdate", Twg_templatedtlCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                        .AddField("UpdateBy", Twg_templatedtlCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                        .AddField("Active", Twg_templatedtlCont.Active, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("Inuse", Twg_templatedtlCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("SyncCreate", Twg_templatedtlCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                        .AddField("SyncLastUpd", Twg_templatedtlCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                                        .AddField("LastSyncBy", Twg_templatedtlCont.LastSyncBy, SQLControl.EnumDataType.dtString)

                                        Select Case pType
                                            Case SQLControl.EnumSQLType.stInsert
                                                If blnFound = True And blnFlag = False Then
                                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TemplateID = '" & Twg_templatedtlCont.TemplateID & "' AND WasteCode = '" & Twg_templatedtlCont.WasteCode & "' AND WasteType = '" & Twg_templatedtlCont.WasteType & "' AND WasteName = '" & Twg_templatedtlCont.WasteName & "'")
                                                Else
                                                    If blnFound = False Then
                                                        .AddField("TemplateID", Twg_templatedtlCont.TemplateID, SQLControl.EnumDataType.dtString)
                                                        .AddField("WasteCode", Twg_templatedtlCont.WasteCode, SQLControl.EnumDataType.dtString)
                                                        .AddField("WasteType", Twg_templatedtlCont.WasteType, SQLControl.EnumDataType.dtString)
                                                        .AddField("WasteName", Twg_templatedtlCont.WasteName, SQLControl.EnumDataType.dtStringN)
                                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                                    End If
                                                End If
                                            Case SQLControl.EnumSQLType.stUpdate
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "TemplateID = '" & Twg_templatedtlCont.TemplateID & "' AND WasteCode = '" & Twg_templatedtlCont.WasteCode & "' AND WasteType = '" & Twg_templatedtlCont.WasteType & "' AND WasteName = '" & Twg_templatedtlCont.WasteName & "'")
                                        End Select
                                    End With
                                    BatchList.Add(strSQL)
                                Next
                            End If

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

                    End If
                End If
            Catch axAssign As ApplicationException
                message = axAssign.Message.ToString()
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Return False
            Finally
                Twg_templateCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
        'ADD
        Public Function Insert(ByVal Twg_templateCont As Container.Twg_template, ByVal ListContTWG_Dtl As List(Of Container.Twg_templatedtl), ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Insert(Twg_templateCont, ListContTWG_Dtl, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
        End Function

        'AMEND
        Public Function Update(ByVal Twg_templateCont As Container.Twg_template, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(Twg_templateCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        Public Function Delete(ByVal Twg_templateCont As Container.Twg_template, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If Twg_templateCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Twg_templateInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "TemplateID = '" & Twg_templateCont.TemplateID & "' AND ReceiverID = '" & Twg_templateCont.ReceiverID & "' AND ReceiverLocID = '" & Twg_templateCont.ReceiverLocID & "' AND WasteCode = '" & Twg_templateCont.WasteCode & "' AND WasteType = '" & Twg_templateCont.WasteType & "' AND WasteName = '" & Twg_templateCont.WasteName & "'")
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
                                strSQL = BuildUpdate(Twg_templateInfo.MyInfo.TableName, " SET Flag = 0" & _
                                " , LastUpdate = '" & Twg_templateCont.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, Twg_templateCont.UpdateBy) & "' WHERE" & _
                                "TemplateID = '" & Twg_templateCont.TemplateID & "' AND ReceiverID = '" & Twg_templateCont.ReceiverID & "' AND ReceiverLocID = '" & Twg_templateCont.ReceiverLocID & "' AND WasteCode = '" & Twg_templateCont.WasteCode & "' AND WasteType = '" & Twg_templateCont.WasteType & "' AND WasteName = '" & Twg_templateCont.WasteName & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(Twg_templateInfo.MyInfo.TableName, "TemplateID = '" & Twg_templateCont.TemplateID & "' AND ReceiverID = '" & Twg_templateCont.ReceiverID & "' AND ReceiverLocID = '" & Twg_templateCont.ReceiverLocID & "' AND WasteCode = '" & Twg_templateCont.WasteCode & "' AND WasteType = '" & Twg_templateCont.WasteType & "' AND WasteName = '" & Twg_templateCont.WasteName & "'")
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
                Twg_templateCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetTWG_TEMPLATE(ByVal TemplateID As System.String, ByVal ReceiverID As System.String, ByVal ReceiverLocID As System.String, ByVal WasteCode As System.String, ByVal WasteType As System.String, ByVal WasteName As System.String) As Container.Twg_template
            Dim rTwg_template As Container.Twg_template = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With Twg_templateInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "TemplateID = '" & TemplateID & "' AND ReceiverID = '" & ReceiverID & "' AND ReceiverLocID = '" & ReceiverLocID & "' AND WasteCode = '" & WasteCode & "' AND WasteType = '" & WasteType & "' AND WasteName = '" & WasteName & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rTWG_TEMPLATE = New Container.TWG_TEMPLATE
                                rTWG_TEMPLATE.TemplateID = drRow.Item("TemplateID")
                                rTWG_TEMPLATE.ReceiverID = drRow.Item("ReceiverID")
                                rTWG_TEMPLATE.ReceiverLocID = drRow.Item("ReceiverLocID")
                                rTWG_TEMPLATE.WasteCode = drRow.Item("WasteCode")
                                rTWG_TEMPLATE.WasteType = drRow.Item("WasteType")
                                rTWG_TEMPLATE.WasteName = drRow.Item("WasteName")
                                rTwg_template.SeqNo = drRow.Item("SeqNo")
                                If Not IsDBNull(drRow.Item("CreateDate")) Then
                                    rTwg_template.CreateDate = drRow.Item("CreateDate")
                                End If
                                rTwg_template.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rTwg_template.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rTwg_template.UpdateBy = drRow.Item("UpdateBy")
                                rTwg_template.Active = drRow.Item("Active")
                                rTwg_template.Inuse = drRow.Item("Inuse")
                                rTwg_template.rowguid = drRow.Item("rowguid")
                                If Not IsDBNull(drRow.Item("SyncCreate")) Then
                                    rTwg_template.SyncCreate = drRow.Item("SyncCreate")
                                End If
                                If Not IsDBNull(drRow.Item("SyncLastUpd")) Then
                                    rTwg_template.SyncLastUpd = drRow.Item("SyncLastUpd")
                                End If
                                rTwg_template.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rTwg_template = Nothing
                            End If
                        Else
                            rTwg_template = Nothing
                        End If
                    End With
                End If
                Return rTwg_template
            Catch ex As Exception
                Throw ex
            Finally
                rTwg_template = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetTWG_TEMPLATE(ByVal TemplateID As System.String, ByVal ReceiverID As System.String, ByVal ReceiverLocID As System.String, ByVal WasteCode As System.String, ByVal WasteType As System.String, ByVal WasteName As System.String, DecendingOrder As Boolean) As List(Of Container.Twg_template)
            Dim rTwg_template As Container.Twg_template = Nothing
            Dim lstTwg_template As List(Of Container.Twg_template) = New List(Of Container.Twg_template)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With Twg_templateInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by TemplateID, ReceiverID, ReceiverLocID, WasteCode, WasteType, WasteName DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "TemplateID = '" & TemplateID & "' AND ReceiverID = '" & ReceiverID & "' AND ReceiverLocID = '" & ReceiverLocID & "' AND WasteCode = '" & WasteCode & "' AND WasteType = '" & WasteType & "' AND WasteName = '" & WasteName & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rTWG_TEMPLATE = New Container.TWG_TEMPLATE
                                rTWG_TEMPLATE.TemplateID = drRow.Item("TemplateID")
                                rTWG_TEMPLATE.ReceiverID = drRow.Item("ReceiverID")
                                rTWG_TEMPLATE.ReceiverLocID = drRow.Item("ReceiverLocID")
                                rTWG_TEMPLATE.WasteCode = drRow.Item("WasteCode")
                                rTWG_TEMPLATE.WasteType = drRow.Item("WasteType")
                                rTWG_TEMPLATE.WasteName = drRow.Item("WasteName")
                                rTwg_template.SeqNo = drRow.Item("SeqNo")
                                rTwg_template.CreateBy = drRow.Item("CreateBy")
                                rTwg_template.UpdateBy = drRow.Item("UpdateBy")
                                rTwg_template.Active = drRow.Item("Active")
                                rTwg_template.Inuse = drRow.Item("Inuse")
                                rTwg_template.rowguid = drRow.Item("rowguid")
                                rTwg_template.LastSyncBy = drRow.Item("LastSyncBy")
                                lstTwg_template.Add(rTwg_template)
                            Next

                        Else
                            rTwg_template = Nothing
                        End If
                        Return lstTwg_template
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rTwg_template = Nothing
                lstTwg_template = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetTWG_TEMPLATEList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With Twg_templateInfo.MyInfo
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

        Public Overloads Function GetTWG_TEMPLATEShortList(ByVal ShortListing As Boolean) As Data.DataTable
            If StartConnection() = True Then
                With Twg_templateInfo.MyInfo
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

        Public Overloads Function GetAllTWG_TEMPLATE(ByVal ReceiverID As String, ByVal ReceiverLocID As String, ByVal WasteCode As String, ByVal WasteType As String, ByVal WasteName As String) As Data.DataTable
            If StartConnection() = True Then
                With Twg_templateInfo.MyInfo
                    StartSQLControl()
                    strSQL = "SELECT d.WasteCode, d.WasteType, d.WasteName, d.Flag FROM TWG_TEMPLATE h WITH (NOLOCK) INNER JOIN TWG_TEMPLATEDTL d WITH (NOLOCK)" & _
                        " ON h.TemplateID=d.TemplateID WHERE h.ReceiverID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverID) & "'" & _
                        " AND h.ReceiverLocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "'" & _
                        " AND h.WasteCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteCode) & "' AND" & _
                        " h.WasteType='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteType) & "'" & _
                        " AND h.WasteName='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteName) & "' ORDER BY d.SeqNo"
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
#Region "Twg_template Container"
        Public Class Twg_template_FieldName
            Public TemplateID As System.String = "TemplateID"
            Public ReceiverID As System.String = "ReceiverID"
            Public ReceiverLocID As System.String = "ReceiverLocID"
            Public WasteCode As System.String = "WasteCode"
            Public WasteType As System.String = "WasteType"
            Public WasteName As System.String = "WasteName"
            Public SeqNo As System.String = "SeqNo"
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
            Public LastSyncBy As System.String = "LastSyncBy"
        End Class

        Public Class Twg_template
            Protected _TemplateID As System.String
            Protected _ReceiverID As System.String
            Protected _ReceiverLocID As System.String
            Protected _WasteCode As System.String
            Protected _WasteType As System.String
            Protected _WasteName As System.String
            Private _SeqNo As System.Int32
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _Active As System.Byte
            Private _Inuse As System.Byte
            Private _rowguid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _LastSyncBy As System.String

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property TemplateID As System.String
                Get
                    Return _TemplateID
                End Get
                Set(ByVal Value As System.String)
                    _TemplateID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ReceiverID As System.String
                Get
                    Return _ReceiverID
                End Get
                Set(ByVal Value As System.String)
                    _ReceiverID = Value
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
            Public Property SeqNo As System.Int32
                Get
                    Return _SeqNo
                End Get
                Set(ByVal Value As System.Int32)
                    _SeqNo = Value
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
            ''' Non-Mandatory, Allow Null
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
            ''' Non-Mandatory, Allow Null
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
#Region "Twg_template Info"
    Public Class Twg_templateInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "TemplateID,ReceiverID,ReceiverLocID,WasteCode,WasteType,WasteName,SeqNo,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,LastSyncBy"
                .CheckFields = "Active,Inuse,Flag"
                .TableName = "Twg_template"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "TemplateID,ReceiverID,ReceiverLocID,WasteCode,WasteType,WasteName,SeqNo,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,LastSyncBy"
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
#Region "TWG_TEMPLATE Scheme"
    Public Class TWG_TEMPLATEScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "TemplateID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ReceiverID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ReceiverLocID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "WasteCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "WasteType"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)

            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "WasteName"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "SeqNo"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Inuse"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)

        End Sub

        Public ReadOnly Property TemplateID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property
        Public ReadOnly Property ReceiverID As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property ReceiverLocID As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property WasteCode As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property WasteType As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property WasteName As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property

        Public ReadOnly Property SeqNo As StrucElement
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
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region
#End Region

End Namespace