
Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Actions
    Public NotInheritable Class WAC_BASELINE
        Inherits Core.CoreControl
        Private Wac_baselineInfo As Wac_baselineInfo = New Wac_baselineInfo
        Private Wac_basecompInfo As Wac_basecompInfo = New Wac_basecompInfo
        Private Log As New SystemLog()

        Public Sub New(ByVal connecn As String)
            ConnectionString = connecn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal ListContWACHdr As List(Of Container.Wac_baseline), ByVal ListContWACDtl As List(Of Container.Wac_basecomp), ByVal ListContWACReceiver As List(Of Container.Wac_wrlist), ByVal ListContWasLabel As List(Of Container.Wac_waslabel), ByVal pType As SQLControl.EnumSQLType, ByRef message As String, ByRef arraylist As ArrayList, Optional ByVal commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim ListSQL As ArrayList = New ArrayList()
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim BaseID As String = ""
            Dim WasCode As String = ""
            Dim WasType As String = ""
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If ListContWACHdr Is Nothing And ListContWACHdr.Count <= 0 Then
                    'Message return
                Else
                    For Each Wac_baselineCont In ListContWACHdr
                        blnExec = False
                        blnFound = False
                        blnFlag = False
                        If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                            StartSQLControl()
                            With Wac_baselineInfo.MyInfo
                                strSQL = BuildSelect(.CheckFields, .TableName, " WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_baselineCont.WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_baselineCont.WasType) & "'")
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
                                    .TableName = "Wac_baseline"
                                    .AddField("WasName", Wac_baselineCont.WasName, SQLControl.EnumDataType.dtStringN)
                                    .AddField("WasDesc", Wac_baselineCont.WasDesc, SQLControl.EnumDataType.dtStringN)
                                    .AddField("BehvType", Wac_baselineCont.BehvType, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("PeriodicComply", Wac_baselineCont.PeriodicComply, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("PeriodicMonths", Wac_baselineCont.PeriodicMonths, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Allow3R", Wac_baselineCont.Allow3R, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("TestingWaste", Wac_baselineCont.TestingWaste, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("AllowDisposal", Wac_baselineCont.AllowDisposal, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("TotalComp", Wac_baselineCont.TotalComp, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("AllowReuse", Wac_baselineCont.AllowReuse, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ProResidue", Wac_baselineCont.ProResidue, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Signature", Wac_baselineCont.Signature, SQLControl.EnumDataType.dtString)
                                    .AddField("ValSGM", Wac_baselineCont.ValSGM, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ValSGX", Wac_baselineCont.ValSGX, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Unit", Wac_baselineCont.Unit, SQLControl.EnumDataType.dtString)
                                    .AddField("CreateDate", Wac_baselineCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("CreateBy", Wac_baselineCont.CreateBy, SQLControl.EnumDataType.dtString)
                                    .AddField("LastUpdate", Wac_baselineCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("UpdateBy", Wac_baselineCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                    .AddField("Active", Wac_baselineCont.Active, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Inuse", Wac_baselineCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Flag", Wac_baselineCont.Flag, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Remark", Wac_baselineCont.Remark, SQLControl.EnumDataType.dtString, SEAL.Data.SQLControl.EnumValidate.cEmpty)
                                    .AddField("IsHost", Wac_baselineCont.IsHost, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("LastSyncBy", Wac_baselineCont.LastSyncBy, SQLControl.EnumDataType.dtString)
                                    BaseID = Wac_baselineCont.BaseID
                                    WasCode = Wac_baselineCont.WasCode
                                    WasType = Wac_baselineCont.WasType
                                    Select Case pType
                                        Case SQLControl.EnumSQLType.stInsert
                                            If blnFound = True And blnFlag = False Then
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "BaseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_baselineCont.BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_baselineCont.WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_baselineCont.WasType) & "'")
                                            Else
                                                If blnFound = False Then
                                                    .AddField("BaseID", Wac_baselineCont.BaseID, SQLControl.EnumDataType.dtString)
                                                    .AddField("WasCode", Wac_baselineCont.WasCode, SQLControl.EnumDataType.dtString)
                                                    .AddField("WasType", Wac_baselineCont.WasType, SQLControl.EnumDataType.dtString)
                                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                                End If
                                            End If
                                        Case SQLControl.EnumSQLType.stUpdate
                                            .AddField("BaseID", Wac_baselineCont.NewBaseID, SQLControl.EnumDataType.dtString)
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "BaseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_baselineCont.BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_baselineCont.WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_baselineCont.WasType) & "'")
                                    End Select
                                End With
                            End If
                        End If
                        ListSQL.Add(strSQL)
                    Next

                    strSQL = "DELETE Wac_basecomp WITH (ROWLOCK) WHERE WasCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, WasCode) & "' AND WasType='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, WasType) & "'"
                    ListSQL.Add(strSQL)
                    strSQL = "DELETE Wac_wrlist WITH (ROWLOCK) WHERE WasCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, WasCode) & "' AND WasType='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, WasType) & "'"
                    ListSQL.Add(strSQL)

                    If ListContWACDtl Is Nothing Or ListContWACDtl.Count <= 0 Then
                        'Message return
                    Else
                        For Each Wac_basecompCont In ListContWACDtl
                            blnExec = False
                            blnFound = False
                            blnFlag = False
                            If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                                StartSQLControl()
                                With objSQL
                                    .TableName = "Wac_basecomp"
                                    .AddField("SeqNo", Wac_basecompCont.SeqNo, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("CPPROP", Wac_basecompCont.CPPROP, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("CPFACTOR", Wac_basecompCont.CPFACTOR, SQLControl.EnumDataType.dtString)
                                    .AddField("CPSGM", Wac_basecompCont.CPSGM, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("CPMIN", Wac_basecompCont.CPMIN, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("CPSGX", Wac_basecompCont.CPSGX, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("CPMAX", Wac_basecompCont.CPMAX, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("CPSMD", Wac_basecompCont.CPSMD, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("CPMED", Wac_basecompCont.CPMED, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("CPSAV", Wac_basecompCont.CPSAV, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("CPAVG", Wac_basecompCont.CPAVG, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("MethodNo", Wac_basecompCont.MethodNo, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("UnitNo", Wac_basecompCont.UnitNo, SQLControl.EnumDataType.dtString)
                                    .AddField("DeletedAttempt", Wac_basecompCont.DeletedAttempt, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("DeletedReason", Wac_basecompCont.DeletedReason, SQLControl.EnumDataType.dtStringN)
                                    .AddField("BCStep", Wac_basecompCont.BCStep, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Dependency", Wac_basecompCont.Dependency, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Mode", Wac_basecompCont.Mode, SQLControl.EnumDataType.dtString)
                                    .AddField("ATMin", Wac_basecompCont.ATMin, SQLControl.EnumDataType.dtString)
                                    .AddField("ATMax", Wac_basecompCont.ATMax, SQLControl.EnumDataType.dtString)
                                    .AddField("ATNX", Wac_basecompCont.ATNX, SQLControl.EnumDataType.dtString)
                                    .AddField("ATTR", Wac_basecompCont.ATTR, SQLControl.EnumDataType.dtString)
                                    .AddField("DDGroup", Wac_basecompCont.DDGroup, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Active", Wac_basecompCont.Active, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("CreateDate", Wac_basecompCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("CreateBy", Wac_basecompCont.CreateBy, SQLControl.EnumDataType.dtString)
                                    .AddField("LastUpdate", Wac_basecompCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("UpdateBy", Wac_basecompCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                    .AddField("ConvertMin", Wac_basecompCont.ConvertMin, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("ConvertMax", Wac_basecompCont.ConvertMax, SQLControl.EnumDataType.dtNumeric)
                                    
                                    If blnFound = False Then
                                        .AddField("BaseID", Wac_basecompCont.BaseID, SQLControl.EnumDataType.dtString)
                                        .AddField("WasCode", Wac_basecompCont.WasCode, SQLControl.EnumDataType.dtString)
                                        .AddField("WasType", Wac_basecompCont.WasType, SQLControl.EnumDataType.dtString)
                                        .AddField("HandlingType", Wac_basecompCont.HandlingType, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("HandlingCode", Wac_basecompCont.HandlingCode, SQLControl.EnumDataType.dtString)
                                        .AddField("DictionaryNo", Wac_basecompCont.DictionaryNo, SQLControl.EnumDataType.dtString)
                                        .AddField("COMPCode", Wac_basecompCont.COMPCode, SQLControl.EnumDataType.dtNumeric)
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                    End If
                                    
                                End With
                            End If
                            ListSQL.Add(strSQL)
                        Next
                    End If

                    If ListContWACReceiver Is Nothing Or ListContWACReceiver.Count <= 0 Then
                        'Message return
                    Else
                        For Each Wac_wrlistCont In ListContWACReceiver
                            blnExec = False
                            blnFound = False
                            blnFlag = False
                            If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                                StartSQLControl()
                                
                                With objSQL
                                    .TableName = "Wac_wrlist"
                                    .AddField("ReqSupp", Wac_wrlistCont.ReqSupp, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Active", Wac_wrlistCont.Active, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("Inuse", Wac_wrlistCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                    .AddField("CreateDate", Wac_wrlistCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("CreateBy", Wac_wrlistCont.CreateBy, SQLControl.EnumDataType.dtString)
                                    .AddField("LastUpdate", Wac_wrlistCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("UpdateBy", Wac_wrlistCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                    .AddField("SyncCreate", Wac_wrlistCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("SyncLastUpd", Wac_wrlistCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                                    .AddField("LastSyncBy", Wac_wrlistCont.LastSyncBy, SQLControl.EnumDataType.dtString)

                                    If blnFound = False Then
                                        .AddField("GeneratorID", Wac_wrlistCont.GeneratorID, SQLControl.EnumDataType.dtString)
                                        .AddField("GeneratorLocID", Wac_wrlistCont.GeneratorLocID, SQLControl.EnumDataType.dtString)
                                        .AddField("WasCode", Wac_wrlistCont.WasCode, SQLControl.EnumDataType.dtString)
                                        .AddField("WasType", Wac_wrlistCont.WasType, SQLControl.EnumDataType.dtString)
                                        .AddField("Allow3R", Wac_wrlistCont.Allow3R, SQLControl.EnumDataType.dtNumeric)
                                        .AddField("ReceiverID", Wac_wrlistCont.ReceiverID, SQLControl.EnumDataType.dtString)
                                        .AddField("ReceiverLocID", Wac_wrlistCont.ReceiverLocID, SQLControl.EnumDataType.dtString)
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                    End If
                                   
                                End With

                            End If
                            ListSQL.Add(strSQL)
                        Next
                    End If

                    strSQL = "DELETE Wac_waslabel WITH (ROWLOCK) WHERE WasCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasCode) & "' AND WasType='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasType) & "'"
                    ListSQL.Add(strSQL)
                    If ListContWasLabel Is Nothing Or ListContWasLabel.Count <= 0 Then
                        'Message return
                    Else
                        For Each Wac_waslabelCont In ListContWasLabel
                            StartSQLControl()
                            With objSQL
                                .TableName = "Wac_waslabel"
                                .AddField("EffectDate", Wac_waslabelCont.EffectDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", Wac_waslabelCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", Wac_waslabelCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", Wac_waslabelCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", Wac_waslabelCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("SyncCreate", Wac_waslabelCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("SyncLastUpd", Wac_waslabelCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                                .AddField("LastSyncBy", Wac_waslabelCont.LastSyncBy, SQLControl.EnumDataType.dtString)
                                .AddField("IsHost", Wac_waslabelCont.IsHost, SQLControl.EnumDataType.dtNumeric)

                                .AddField("BaseID", Wac_waslabelCont.BaseID, SQLControl.EnumDataType.dtString)
                                .AddField("WasCode", Wac_waslabelCont.WasCode, SQLControl.EnumDataType.dtString)
                                .AddField("WasType", Wac_waslabelCont.WasType, SQLControl.EnumDataType.dtString)
                                .AddField("LabelID", Wac_waslabelCont.LabelID, SQLControl.EnumDataType.dtString)
                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                            End With
                            ListSQL.Add(strSQL)
                        Next
                    End If

                    If pType = SQLControl.EnumSQLType.stUpdate Then
                        strSQL = "EXEC [WAC_QualifiedWR_DOE] '','" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, WasCode) & "','" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, WasType) & "','',''"
                        ListSQL.Add(strSQL)

                        strSQL = "EXEC [WAC_QualifiedWR_WG] '','','" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, WasCode) & "','" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, WasType) & "','',''"
                        ListSQL.Add(strSQL)
                    End If

                    Try
                        If commit = True Then
                            objConn.BatchExecute(ListSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                        Else
                            arraylist = ListSQL
                        End If
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
                        Gibraltar.Agent.Log.Error("WAC_ACCMASTER", axExecute.Message & sqlStatement, axExecute.StackTrace)
                        Return False
                    Finally
                        objSQL.Dispose()
                    End Try
                End If
            Catch axAssign As ApplicationException
                message = axAssign.Message.ToString()
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Return False
            Finally
                ListContWACDtl = Nothing
                ListContWACHdr = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
            Return True
        End Function

        'ADD
        Public Overloads Function Insert(ByVal ListContWACHdr As List(Of Container.Wac_baseline), ByVal ListContWACDtl As List(Of Container.Wac_basecomp), ByVal ListContWACReceiver As List(Of Container.Wac_wrlist), ByVal ListContWasLabel As List(Of Container.Wac_waslabel), ByRef message As String, ByRef arraylist As ArrayList, Optional ByVal commit As Boolean = False) As Boolean
            Return Save(ListContWACHdr, ListContWACDtl, ListContWACReceiver, ListContWasLabel, SQLControl.EnumSQLType.stInsert, message, arraylist, commit)
        End Function

        'AMEND
        Public Function Update(ByVal ListContWACHdr As List(Of Container.Wac_baseline), ByVal ListContWACDtl As List(Of Container.Wac_basecomp), ByVal ListContWACReceiver As List(Of Container.Wac_wrlist), ByVal ListContWasLabel As List(Of Container.Wac_waslabel), ByRef message As String, ByRef arraylist As ArrayList, Optional ByVal commit As Boolean = False) As Boolean
            Return Save(ListContWACHdr, ListContWACDtl, ListContWACReceiver, ListContWasLabel, SQLControl.EnumSQLType.stUpdate, message, arraylist, commit)
        End Function

        Public Function Delete(ByVal Wac_baselineCont As Container.Wac_baseline, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If Wac_baselineCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Wac_baselineInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "BaseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_baselineCont.BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_baselineCont.WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_baselineCont.WasType) & "'")
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
                                strSQL = BuildUpdate(Wac_baselineInfo.MyInfo.TableName, " SET Flag = 0" & _
                                " , LastUpdate = GETDATE() , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_baselineCont.UpdateBy) & "' WHERE " & _
                                "BaseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_baselineCont.BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_baselineCont.WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_baselineCont.WasType) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(Wac_baselineInfo.MyInfo.TableName, "BaseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_baselineCont.BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_baselineCont.WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_baselineCont.WasType) & "'")
                        End If

                        Try
                            'execute
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
                Wac_baselineCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetWAC_BASELINE(ByVal BaseID As System.String, ByVal WasCode As System.String, ByVal WasType As System.String) As Container.Wac_baseline
            Dim rWac_baseline As Container.Wac_baseline = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With Wac_baselineInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "BaseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasType) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rWac_baseline = New Container.Wac_baseline
                                rWac_baseline.BaseID = drRow.Item("BaseID")
                                rWac_baseline.WasCode = drRow.Item("WasCode")
                                rWac_baseline.WasType = drRow.Item("WasType")
                                rWac_baseline.WasName = drRow.Item("WasName")
                                rWac_baseline.WasDesc = drRow.Item("WasDesc")
                                rWac_baseline.BehvType = drRow.Item("BehvType")
                                rWac_baseline.PeriodicComply = drRow.Item("PeriodicComply")
                                rWac_baseline.PeriodicMonths = drRow.Item("PeriodicMonths")
                                rWac_baseline.Allow3R = drRow.Item("Allow3R")
                                rWac_baseline.AllowReuse = drRow.Item("AllowReuse")
                                rWac_baseline.ProResidue = drRow.Item("ProResidue")
                                rWac_baseline.TotalComp = drRow.Item("TotalComp")
                                rWac_baseline.Signature = drRow.Item("Signature")
                                rWac_baseline.ValSGM = drRow.Item("ValSGM")
                                rWac_baseline.ValSGX = drRow.Item("ValSGX")
                                rWac_baseline.Unit = drRow.Item("Unit")
                                rWac_baseline.CreateBy = drRow.Item("CreateBy")
                                rWac_baseline.UpdateBy = drRow.Item("UpdateBy")
                                rWac_baseline.Active = drRow.Item("Active")
                                rWac_baseline.Inuse = drRow.Item("Inuse")
                                rWac_baseline.Remark = drRow.Item("Remark")
                                rWac_baseline.rowguid = drRow.Item("rowguid")
                                rWac_baseline.SyncCreate = drRow.Item("SyncCreate")
                                rWac_baseline.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rWac_baseline.IsHost = drRow.Item("IsHost")
                                rWac_baseline.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rWac_baseline = Nothing
                            End If
                        Else
                            rWac_baseline = Nothing
                        End If
                    End With
                End If
                Return rWac_baseline
            Catch ex As Exception
                Throw ex
            Finally
                rWac_baseline = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWAC_BASELINE(ByVal BaseID As System.String, ByVal WasCode As System.String, ByVal WasType As System.String, ByVal DecendingOrder As Boolean) As List(Of Container.Wac_baseline)
            Dim rWac_baseline As Container.Wac_baseline = Nothing
            Dim lstWac_baseline As List(Of Container.Wac_baseline) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With Wac_baselineInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal BaseID As System.String, ByVal WasCode As System.String, ByVal WasType As System.Int32 DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "BaseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasType) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rWac_baseline = New Container.Wac_baseline
                                rWac_baseline.BaseID = drRow.Item("BaseID")
                                rWac_baseline.WasCode = drRow.Item("WasCode")
                                rWac_baseline.WasType = drRow.Item("WasType")
                                rWac_baseline.WasName = drRow.Item("WasName")
                                rWac_baseline.WasDesc = drRow.Item("WasDesc")
                                rWac_baseline.BehvType = drRow.Item("BehvType")
                                rWac_baseline.PeriodicComply = drRow.Item("PeriodicComply")
                                rWac_baseline.PeriodicMonths = drRow.Item("PeriodicMonths")
                                rWac_baseline.Allow3R = drRow.Item("Allow3R")
                                rWac_baseline.TotalComp = drRow.Item("TotalComp")
                                rWac_baseline.Signature = drRow.Item("Signature")
                                rWac_baseline.ValSGM = drRow.Item("ValSGM")
                                rWac_baseline.ValSGX = drRow.Item("ValSGX")
                                rWac_baseline.Unit = drRow.Item("Unit")
                                rWac_baseline.CreateBy = drRow.Item("CreateBy")
                                rWac_baseline.UpdateBy = drRow.Item("UpdateBy")
                                rWac_baseline.Active = drRow.Item("Active")
                                rWac_baseline.Inuse = drRow.Item("Inuse")
                                rWac_baseline.rowguid = drRow.Item("rowguid")
                                rWac_baseline.SyncCreate = drRow.Item("SyncCreate")
                                rWac_baseline.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rWac_baseline.IsHost = drRow.Item("IsHost")
                                rWac_baseline.LastSyncBy = drRow.Item("LastSyncBy")
                            Next
                            lstWac_baseline.Add(rWac_baseline)
                        Else
                            rWac_baseline = Nothing
                        End If
                        Return lstWac_baseline
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rWac_baseline = Nothing
                lstWac_baseline = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWAC_BASELINEWasDesc(ByVal BaseID As System.String, ByVal WasCode As System.String, ByVal WasType As System.String) As Container.Wac_baseline
            Dim rWac_baseline As Container.Wac_baseline = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With Wac_baselineInfo.MyInfo
                        strSQL = "SELECT b.*,cm.CodeDesc FROM WAC_BASELINE b INNER JOIN CODEMASTER cm ON b.WasType = cm.Code AND cm.CodeType='WTY' WHERE BaseID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BaseID) & "' AND WasCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasCode) & "' AND WasType = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasType) & "'"
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rWac_baseline = New Container.Wac_baseline
                                rWac_baseline.BaseID = drRow.Item("BaseID")
                                rWac_baseline.WasCode = drRow.Item("WasCode")
                                rWac_baseline.WasType = drRow.Item("WasType")
                                rWac_baseline.WasName = drRow.Item("WasName")
                                rWac_baseline.WasDesc = drRow.Item("WasDesc")
                                rWac_baseline.BehvType = drRow.Item("BehvType")
                                rWac_baseline.PeriodicComply = drRow.Item("PeriodicComply")
                                rWac_baseline.PeriodicMonths = drRow.Item("PeriodicMonths")
                                rWac_baseline.Allow3R = drRow.Item("Allow3R")
                                rWac_baseline.TestingWaste = drRow.Item("TestingWaste")
                                rWac_baseline.AllowDisposal = drRow.Item("AllowDisposal")
                                rWac_baseline.TotalComp = drRow.Item("TotalComp")
                                rWac_baseline.Signature = drRow.Item("Signature")
                                rWac_baseline.ValSGM = drRow.Item("ValSGM")
                                rWac_baseline.ValSGX = drRow.Item("ValSGX")
                                rWac_baseline.Unit = drRow.Item("Unit")
                                rWac_baseline.CreateBy = drRow.Item("CreateBy")
                                rWac_baseline.UpdateBy = drRow.Item("UpdateBy")
                                rWac_baseline.Active = drRow.Item("Active")
                                rWac_baseline.Inuse = drRow.Item("Inuse")
                                rWac_baseline.Remark = drRow.Item("Remark")
                                rWac_baseline.rowguid = drRow.Item("rowguid")
                                rWac_baseline.SyncCreate = drRow.Item("SyncCreate")
                                rWac_baseline.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rWac_baseline.IsHost = drRow.Item("IsHost")
                                rWac_baseline.LastSyncBy = drRow.Item("LastSyncBy")
                                rWac_baseline.WasTypeDesc = drRow.Item("CodeDesc")
                            Else
                                rWac_baseline = Nothing
                            End If
                        Else
                            rWac_baseline = Nothing
                        End If
                    End With
                End If
                Return rWac_baseline
            Catch ex As Exception
                Throw ex
            Finally
                rWac_baseline = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWAC_BASELINEList(Optional ByVal Active As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_baselineInfo.MyInfo
                    strSQL = "SELECT BL.*, C.CodeDesc FROM WAC_BASELINE BL " & _
                        " INNER JOIN (Select Code, CodeDesc from CODEMASTER Where CodeType='WTY') C " & _
                        " ON BL.WasType=C.Code Where Flag=" & objSQL.ParseValue(SQLControl.EnumDataType.dtNumeric, Active) & _
                        " ORDER BY BL.WASCODE"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetWac_BASELINEListByLocID(ByVal WasteCode As String, ByVal WasteType As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_baselineInfo.MyInfo
                    strSQL = "SELECT * FROM WAC_BASELINE WHERE WasCode='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteCode) & "' AND " & _
                        "WasType='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteType) & "'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function
#End Region
    End Class


    Namespace Container
#Region "Class Container"
        Public Class Wac_baseline
            Public fBaseID As System.String = "BaseID"
            Public fWasCode As System.String = "WasCode"
            Public fWasType As System.String = "WasType"
            Public fWasName As System.String = "WasName"
            Public fWasDesc As System.String = "WasDesc"
            Public fBehvType As System.String = "BehvType"
            Public fPeriodicComply As System.String = "PeriodicComply"
            Public fPeriodicMonths As System.String = "PeriodicMonths"
            Public fAllow3R As System.String = "Allow3R"
            Public fTotalComp As System.String = "TotalComp"
            Public fSignature As System.String = "Signature"
            Public fValSGM As System.String = "ValSGM"
            Public fValSGX As System.String = "ValSGX"
            Public fUnit As System.String = "Unit"
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
            Public fWasTypeDesc As System.String = "WasTypeDesc"

            Protected _BaseID As System.String
            Protected _WasCode As System.String
            Protected _WasType As System.String
            Private _WasName As System.String
            Private _WasDesc As System.String
            Private _BehvType As System.Byte
            Private _PeriodicComply As System.Byte
            Private _PeriodicMonths As System.Int32
            Private _Allow3R As System.Byte
            Private _AllowReuse As System.Byte
            Private _TestingWaste As System.Byte
            Private _ProResidue As System.Byte
            Private _TotalComp As System.Int32
            Private _Signature As System.String
            Private _ValSGM As System.Int32
            Private _ValSGX As System.Int32
            Private _Unit As System.String
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _Active As System.Byte
            Private _Inuse As System.Byte
            Private _Flag As System.Byte
            Private _rowguid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _IsHost As System.Byte
            Private _LastSyncBy As System.String
            Private _WasTypeDesc As System.String
            Private _Remark As System.String
            Private _NewBaseID As System.String
            Private _AllowDisposal As System.Byte

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property AllowDisposal As System.Byte
                Get
                    Return _AllowDisposal
                End Get
                Set(ByVal Value As System.Byte)
                    _AllowDisposal = Value
                End Set
            End Property
            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property Remark As System.String
                Get
                    Return _Remark
                End Get
                Set(ByVal Value As System.String)
                    _Remark = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property WasTypeDesc As System.String
                Get
                    Return _WasTypeDesc
                End Get
                Set(ByVal Value As System.String)
                    _WasTypeDesc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property AllowReuse As System.String
                Get
                    Return _AllowReuse
                End Get
                Set(ByVal Value As System.String)
                    _AllowReuse = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property TestingWaste As System.String
                Get
                    Return _TestingWaste
                End Get
                Set(ByVal Value As System.String)
                    _TestingWaste = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ProResidue As System.String
                Get
                    Return _ProResidue
                End Get
                Set(ByVal Value As System.String)
                    _ProResidue = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property BaseID As System.String
                Get
                    Return _BaseID
                End Get
                Set(ByVal Value As System.String)
                    _BaseID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property NewBaseID As System.String
                Get
                    Return _NewBaseID
                End Get
                Set(ByVal Value As System.String)
                    _NewBaseID = Value
                End Set
            End Property


            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property WasCode As System.String
                Get
                    Return _WasCode
                End Get
                Set(ByVal Value As System.String)
                    _WasCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property WasType As System.String
                Get
                    Return _WasType
                End Get
                Set(ByVal Value As System.String)
                    _WasType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property WasName As System.String
                Get
                    Return _WasName
                End Get
                Set(ByVal Value As System.String)
                    _WasName = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property WasDesc As System.String
                Get
                    Return _WasDesc
                End Get
                Set(ByVal Value As System.String)
                    _WasDesc = Value
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
            Public Property PeriodicComply As System.Byte
                Get
                    Return _PeriodicComply
                End Get
                Set(ByVal Value As System.Byte)
                    _PeriodicComply = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PeriodicMonths As System.Int32
                Get
                    Return _PeriodicMonths
                End Get
                Set(ByVal Value As System.Int32)
                    _PeriodicMonths = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Allow3R As System.Byte
                Get
                    Return _Allow3R
                End Get
                Set(ByVal Value As System.Byte)
                    _Allow3R = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TotalComp As System.Int32
                Get
                    Return _TotalComp
                End Get
                Set(ByVal Value As System.Int32)
                    _TotalComp = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Signature As System.String
                Get
                    Return _Signature
                End Get
                Set(ByVal Value As System.String)
                    _Signature = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ValSGM As System.Int32
                Get
                    Return _ValSGM
                End Get
                Set(ByVal Value As System.Int32)
                    _ValSGM = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ValSGX As System.Int32
                Get
                    Return _ValSGX
                End Get
                Set(ByVal Value As System.Int32)
                    _ValSGX = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Unit As System.String
                Get
                    Return _Unit
                End Get
                Set(ByVal Value As System.String)
                    _Unit = Value
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
            Public Property Flag As System.Byte
                Get
                    Return _Flag
                End Get
                Set(ByVal Value As System.Byte)
                    _Flag = Value
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

#Region "Class Info"
    Public Class Wac_baselineInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "BaseID,WasCode,WasType,WasName,WasDesc,BehvType,PeriodicComply,PeriodicMonths,Allow3R,AllowReuse,ProResidue,TotalComp,Signature,ValSGM,ValSGX,Unit,CreateDate,CreateBy,Remark,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
                .CheckFields = "BehvType,PeriodicComply,Allow3R,Active,Inuse,Flag,IsHost"
                .TableName = "Wac_baseline"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "BaseID,WasCode,WasType,WasName,WasDesc,BehvType,PeriodicComply,PeriodicMonths,Allow3R,AllowReuse,ProResidue,TotalComp,Signature,ValSGM,ValSGX,Unit,CreateDate,CreateBy,Remark,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
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
    Public Class WAC_BASELINEScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "BaseID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "WasCode"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "WasType"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "WasName"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "WasDesc"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "BehvType"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "PeriodicComply"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "PeriodicMonths"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Allow3R"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "TotalComp"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Signature"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ValSGM"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ValSGX"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Unit"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Inuse"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(21, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(22, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(23, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsHost"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(24, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(25, this)

        End Sub

        Public ReadOnly Property BaseID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property
        Public ReadOnly Property WasCode As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property WasType As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property

        Public ReadOnly Property WasName As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property WasDesc As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property BehvType As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property PeriodicComply As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property PeriodicMonths As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property Allow3R As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property TotalComp As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property Signature As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property ValSGM As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property ValSGX As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property Unit As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property Inuse As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(22)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(23)
            End Get
        End Property
        Public ReadOnly Property IsHost As StrucElement
            Get
                Return MyBase.GetItem(24)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(25)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region

End Namespace