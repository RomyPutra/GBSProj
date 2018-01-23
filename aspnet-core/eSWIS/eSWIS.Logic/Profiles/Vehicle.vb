Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Profiles
    Public NotInheritable Class Vehicle
        Inherits Core.CoreControl
        Private VehicleInfo As VehicleInfo = New VehicleInfo
        Private Log As New SystemLog()

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal VehicleCont As Container.Vehicle, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If VehicleCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With VehicleInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "VehicleID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, VehicleCont.VehicleID) & "'")
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
                                .TableName = "VEHICLE WITH (ROWLOCK)"
                                .AddField("RegNo", VehicleCont.RegNo, SQLControl.EnumDataType.dtString)
                                .AddField("Ownership", VehicleCont.Ownership, SQLControl.EnumDataType.dtString)
                                .AddField("CompanyID", VehicleCont.CompanyID, SQLControl.EnumDataType.dtString)
                                .AddField("VehicleType", VehicleCont.VehicleType, SQLControl.EnumDataType.dtString)
                                .AddField("BDM", VehicleCont.BDM, SQLControl.EnumDataType.dtString)
                                .AddField("ManufacturingYear", VehicleCont.ManufacturingYear, SQLControl.EnumDataType.dtString)
                                .AddField("RegistrationYear", VehicleCont.RegistrationYear, SQLControl.EnumDataType.dtString)
                                .AddField("GPSInstallation", VehicleCont.GPSInstallation, SQLControl.EnumDataType.dtNumeric)
                                .AddField("BinLifterInstallation", VehicleCont.BinLifterInstallation, SQLControl.EnumDataType.dtNumeric)
                                .AddField("TruckPainting", VehicleCont.TruckPainting, SQLControl.EnumDataType.dtNumeric)
                                .AddField("RegistrationCard", VehicleCont.RegistrationCard, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Photos", VehicleCont.Photos, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Status", VehicleCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", VehicleCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", VehicleCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", VehicleCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", VehicleCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Inuse", VehicleCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsHost", VehicleCont.IsHost, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", VehicleCont.Flag, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "VehicleID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, VehicleCont.VehicleID) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("VehicleID", VehicleCont.VehicleID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "VehicleID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, VehicleCont.VehicleID) & "'")
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
                                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Vehicle", axExecute.Message & strSQL, axExecute.StackTrace)
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
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Vehicle", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Vehicle", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                VehicleCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Private Function BatchSave(ByVal ListContVehicle As List(Of Profiles.Container.Vehicle), ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()

            BatchSave = False
            Try
                If ListContVehicle Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()

                        If ListContVehicle.Count > 0 Then
                            strSQL = BuildDelete("VEHICLE WITH (ROWLOCK)", "VehicleID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ListContVehicle(0).VehicleID) & "'")
                            ListSQL.Add(strSQL)
                        End If

                    End If

                    For Each VehicleCont In ListContVehicle
                        StartSQLControl()
                        With objSQL
                            .TableName = "VEHICLE WITH (ROWLOCK)"
                            .AddField("RegNo", VehicleCont.RegNo, SQLControl.EnumDataType.dtString)
                            .AddField("Ownership", VehicleCont.Ownership, SQLControl.EnumDataType.dtString)
                            .AddField("CompanyID", VehicleCont.CompanyID, SQLControl.EnumDataType.dtString)
                            .AddField("VehicleType", VehicleCont.VehicleType, SQLControl.EnumDataType.dtString)
                            .AddField("BDM", VehicleCont.BDM, SQLControl.EnumDataType.dtString)
                            .AddField("ManufacturingYear", VehicleCont.ManufacturingYear, SQLControl.EnumDataType.dtString)
                            .AddField("RegistrationYear", VehicleCont.RegistrationYear, SQLControl.EnumDataType.dtString)
                            .AddField("GPSInstallation", VehicleCont.GPSInstallation, SQLControl.EnumDataType.dtNumeric)
                            .AddField("BinLifterInstallation", VehicleCont.BinLifterInstallation, SQLControl.EnumDataType.dtNumeric)
                            .AddField("TruckPainting", VehicleCont.TruckPainting, SQLControl.EnumDataType.dtNumeric)
                            .AddField("RegistrationCard", VehicleCont.RegistrationCard, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Photos", VehicleCont.Photos, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Status", VehicleCont.Status, SQLControl.EnumDataType.dtNumeric)
                            .AddField("CreateDate", VehicleCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("CreateBy", VehicleCont.CreateBy, SQLControl.EnumDataType.dtString)
                            .AddField("LastUpdate", VehicleCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                            .AddField("UpdateBy", VehicleCont.UpdateBy, SQLControl.EnumDataType.dtString)
                            .AddField("Inuse", VehicleCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                            .AddField("IsHost", VehicleCont.IsHost, SQLControl.EnumDataType.dtNumeric)
                            .AddField("Flag", VehicleCont.Flag, SQLControl.EnumDataType.dtString)

                            Select Case pType
                                Case SQLControl.EnumSQLType.stInsert
                                    .AddField("VehicleID", VehicleCont.VehicleID, SQLControl.EnumDataType.dtString)
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)

                                Case SQLControl.EnumSQLType.stUpdate
                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "VehicleID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, VehicleCont.VehicleID) & "'")
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
                        Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Vehicle", axExecute.Message & sqlStatement, axExecute.StackTrace)
                    Finally
                        objSQL.Dispose()
                    End Try
                    Return True

                End If
            Catch axAssign As ApplicationException
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Vehicle", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Vehicle", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                ListContVehicle = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal VehicleCont As Container.Vehicle, ByRef message As String) As Boolean
            Return Save(VehicleCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        Public Function BatchInsert(ByVal ListContVehicle As List(Of Profiles.Container.Vehicle), ByRef message As String) As Boolean
            Return BatchSave(ListContVehicle, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal VehicleCont As Container.Vehicle, ByRef message As String) As Boolean
            Return Save(VehicleCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal VehicleCont As Container.Vehicle, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If VehicleCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With VehicleInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "VehicleID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, VehicleCont.VehicleID) & "'")
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
                                strSQL = BuildUpdate("VEHICLE WITH (ROWLOCK)", " SET Flag = 0" & _
                                " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, VehicleCont.UpdateBy) & "' WHERE" & _
                                "VehicleID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, VehicleCont.VehicleID) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("VEHICLE WITH (ROWLOCK)", "VehicleID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, VehicleCont.VehicleID) & "'")
                        End If

                        Try
                            'execute
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Vehicle", exExecute.Message & strSQL, exExecute.StackTrace)
                            Return False
                            'Throw New ApplicationException("210006 " & exExecute.Message.ToString())
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Vehicle", axDelete.Message, axDelete.StackTrace)
                Return False
                'Throw axDelete
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Vehicle", exDelete.Message, exDelete.StackTrace)
                Return False
                'Throw exDelete
            Finally
                VehicleCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetVehicle(ByVal VehicleID As System.String) As Container.Vehicle
            Dim rVehicle As Container.Vehicle = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With VehicleInfo.MyInfo
                        StartSQLControl()
                        strSQL = BuildSelect(.FieldsList, .TableName, "VehicleID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, VehicleID) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rVehicle = New Container.Vehicle
                                rVehicle.VehicleID = drRow.Item("VehicleID")
                                rVehicle.RegNo = drRow.Item("RegNo")
                                rVehicle.Ownership = drRow.Item("Ownership")
                                rVehicle.CompanyID = drRow.Item("CompanyID")
                                rVehicle.CompanyDesc = drRow.Item("CompanyDesc")
                                rVehicle.VehicleType = drRow.Item("VehicleType")
                                rVehicle.BDM = drRow.Item("BDM")
                                rVehicle.ManufacturingYear = drRow.Item("ManufacturingYear")
                                rVehicle.RegistrationYear = drRow.Item("RegistrationYear")
                                rVehicle.GPSInstallation = drRow.Item("GPSInstallation")
                                rVehicle.BinLifterInstallation = drRow.Item("BinLifterInstallation")
                                rVehicle.TruckPainting = drRow.Item("TruckPainting")
                                rVehicle.RegistrationCard = drRow.Item("RegistrationCard")
                                rVehicle.Photos = drRow.Item("Photos")
                                rVehicle.Status = drRow.Item("Status")
                                rVehicle.CreateBy = drRow.Item("CreateBy")
                                rVehicle.UpdateBy = drRow.Item("UpdateBy")
                                rVehicle.Inuse = drRow.Item("Inuse")
                                rVehicle.rowguid = drRow.Item("rowguid")
                                rVehicle.SyncCreate = drRow.Item("SyncCreate")
                                rVehicle.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rVehicle.IsHost = drRow.Item("IsHost")
                                rVehicle.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rVehicle = Nothing
                            End If
                        Else
                            rVehicle = Nothing
                        End If
                    End With
                End If
                Return rVehicle
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Vehicle", ex.Message, ex.StackTrace)
            Finally
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetVehicle(ByVal VehicleID As System.String, ByVal DecendingOrder As Boolean) As List(Of Container.Vehicle)
            Dim rVehicle As Container.Vehicle = Nothing
            Dim lstVehicle As List(Of Container.Vehicle) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With VehicleInfo.MyInfo
                        StartSQLControl()
                        If DecendingOrder Then
                            strDesc = " Order by ByVal VehicleID As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "VehicleID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, VehicleID) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rVehicle = New Container.Vehicle
                                rVehicle.VehicleID = drRow.Item("VehicleID")
                                rVehicle.RegNo = drRow.Item("RegNo")
                                rVehicle.Ownership = drRow.Item("Ownership")
                                rVehicle.CompanyID = drRow.Item("CompanyID")
                                rVehicle.CompanyDesc = drRow.Item("CompanyDesc")
                                rVehicle.VehicleType = drRow.Item("VehicleType")
                                rVehicle.BDM = drRow.Item("BDM")
                                rVehicle.ManufacturingYear = drRow.Item("ManufacturingYear")
                                rVehicle.RegistrationYear = drRow.Item("RegistrationYear")
                                rVehicle.GPSInstallation = drRow.Item("GPSInstallation")
                                rVehicle.BinLifterInstallation = drRow.Item("BinLifterInstallation")
                                rVehicle.TruckPainting = drRow.Item("TruckPainting")
                                rVehicle.RegistrationCard = drRow.Item("RegistrationCard")
                                rVehicle.Photos = drRow.Item("Photos")
                                rVehicle.Status = drRow.Item("Status")
                                rVehicle.CreateBy = drRow.Item("CreateBy")
                                rVehicle.UpdateBy = drRow.Item("UpdateBy")
                                rVehicle.Inuse = drRow.Item("Inuse")
                                rVehicle.rowguid = drRow.Item("rowguid")
                                rVehicle.SyncCreate = drRow.Item("SyncCreate")
                                rVehicle.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rVehicle.IsHost = drRow.Item("IsHost")
                                rVehicle.LastSyncBy = drRow.Item("LastSyncBy")
                            Next
                            lstVehicle.Add(rVehicle)
                        Else
                            rVehicle = Nothing
                        End If
                        Return lstVehicle
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Vehicle", ex.Message, ex.StackTrace)
            Finally
                rVehicle = Nothing
                lstVehicle = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetTransportVehicle(ByVal CompanyID As System.String) As List(Of Container.Vehicle)
            Dim rVehicle As Container.Vehicle = Nothing
            Dim lstVehicle As List(Of Container.Vehicle) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With VehicleInfo.MyInfo
                        StartSQLControl()
                        strSQL = BuildSelect(.FieldsList, .TableName, " CompanyID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompanyID) & "' AND Status=1 AND Flag=1")

                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            lstVehicle = New List(Of Container.Vehicle)
                            For Each drRow As DataRow In dtTemp.Rows
                                rVehicle = New Container.Vehicle
                                rVehicle.VehicleID = drRow.Item("VehicleID")
                                rVehicle.RegNo = drRow.Item("RegNo")
                                rVehicle.Ownership = drRow.Item("Ownership")
                                rVehicle.CompanyID = drRow.Item("CompanyID")
                                rVehicle.CompanyDesc = drRow.Item("CompanyDesc")
                                rVehicle.VehicleType = drRow.Item("VehicleType")
                                rVehicle.BDM = drRow.Item("BDM")
                                rVehicle.ManufacturingYear = drRow.Item("ManufacturingYear")
                                rVehicle.RegistrationYear = drRow.Item("RegistrationYear")
                                rVehicle.GPSInstallation = drRow.Item("GPSInstallation")
                                rVehicle.BinLifterInstallation = drRow.Item("BinLifterInstallation")
                                rVehicle.TruckPainting = drRow.Item("TruckPainting")
                                rVehicle.RegistrationCard = drRow.Item("RegistrationCard")
                                rVehicle.Photos = drRow.Item("Photos")
                                rVehicle.Status = drRow.Item("Status")
                                rVehicle.CreateBy = drRow.Item("CreateBy")
                                rVehicle.UpdateBy = drRow.Item("UpdateBy")
                                rVehicle.Inuse = drRow.Item("Inuse")
                                rVehicle.rowguid = drRow.Item("rowguid")
                                rVehicle.SyncCreate = drRow.Item("SyncCreate")
                                rVehicle.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rVehicle.IsHost = drRow.Item("IsHost")
                                rVehicle.LastSyncBy = drRow.Item("LastSyncBy")
                                lstVehicle.Add(rVehicle)
                            Next

                        Else
                            rVehicle = Nothing
                        End If
                        Return lstVehicle
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Vehicle", ex.Message, ex.StackTrace)
            Finally
                rVehicle = Nothing
                lstVehicle = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetVehicleCompany(ByVal CompanyID As System.String, ByVal VehicleID As System.String) As Container.Vehicle
            Dim rVehicle As Container.Vehicle = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With VehicleInfo.MyInfo
                        StartSQLControl()
                        strSQL = BuildSelect(.FieldsList, .TableName, "CompanyID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompanyID) & "' AND VehicleID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, VehicleID) & "' AND Status='1'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rVehicle = New Container.Vehicle
                                rVehicle.VehicleID = drRow.Item("VehicleID")
                                rVehicle.RegNo = drRow.Item("RegNo")
                                rVehicle.Ownership = drRow.Item("Ownership")
                                rVehicle.CompanyID = drRow.Item("CompanyID")
                                rVehicle.CompanyDesc = drRow.Item("CompanyDesc")
                                rVehicle.VehicleType = drRow.Item("VehicleType")
                                rVehicle.BDM = drRow.Item("BDM")
                                rVehicle.ManufacturingYear = drRow.Item("ManufacturingYear")
                                rVehicle.RegistrationYear = drRow.Item("RegistrationYear")
                                rVehicle.GPSInstallation = drRow.Item("GPSInstallation")
                                rVehicle.BinLifterInstallation = drRow.Item("BinLifterInstallation")
                                rVehicle.TruckPainting = drRow.Item("TruckPainting")
                                rVehicle.RegistrationCard = drRow.Item("RegistrationCard")
                                rVehicle.Photos = drRow.Item("Photos")
                                rVehicle.Status = drRow.Item("Status")
                                rVehicle.CreateBy = drRow.Item("CreateBy")
                                rVehicle.UpdateBy = drRow.Item("UpdateBy")
                                rVehicle.Inuse = drRow.Item("Inuse")
                                rVehicle.rowguid = drRow.Item("rowguid")
                                rVehicle.SyncCreate = drRow.Item("SyncCreate")
                                rVehicle.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rVehicle.IsHost = drRow.Item("IsHost")
                                rVehicle.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rVehicle = Nothing
                            End If
                        Else
                            rVehicle = Nothing
                        End If
                    End With
                End If
                Return rVehicle
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Vehicle", ex.Message, ex.StackTrace)
            Finally
                rVehicle = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetVehicleList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With VehicleInfo.MyInfo
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

        Public Overloads Function GetVehicleListEntity(Optional ByVal Flg As String = Nothing, Optional ByVal CompanyID As String = Nothing) As Data.DataTable
            Dim strFilter As String = ""
            If StartConnection() = True Then
                StartSQLControl()
                With VehicleInfo.MyInfo
                    strFilter = "Flag ='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Flg) & "'"
                    If CompanyID IsNot Nothing AndAlso CompanyID <> "" Then
                        strFilter &= " AND CompanyID ='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompanyID) & "'"
                    End If
                    strSQL = BuildSelect(.FieldsList, .TableName, strFilter)
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
        Public Class Vehicle
            Public fVehicleID As System.String = "VehicleID"
            Public fRegNo As System.String = "RegNo"
            Public fOwnership As System.String = "Ownership"
            Public fCompanyID As System.String = "CompanyID"
            Public fCompanyDesc As System.String = "CompanyDesc"
            Public fVehicleType As System.String = "VehicleType"
            Public fBDM As System.String = "BDM"
            Public fManufacturingYear As System.String = "ManufacturingYear"
            Public fRegistrationYear As System.String = "RegistrationYear"
            Public fGPSInstallation As System.String = "GPSInstallation"
            Public fBinLifterInstallation As System.String = "BinLifterInstallation"
            Public fTruckPainting As System.String = "TruckPainting"
            Public fRegistrationCard As System.String = "RegistrationCard"
            Public fPhotos As System.String = "Photos"
            Public fStatus As System.String = "Status"
            Public fCreateDate As System.String = "CreateDate"
            Public fCreateBy As System.String = "CreateBy"
            Public fLastUpdate As System.String = "LastUpdate"
            Public fUpdateBy As System.String = "UpdateBy"
            Public fFlag As System.String = "Flag"
            Public fInuse As System.String = "Inuse"
            Public frowguid As System.String = "rowguid"
            Public fSyncCreate As System.String = "SyncCreate"
            Public fSyncLastUpd As System.String = "SyncLastUpd"
            Public fIsHost As System.String = "IsHost"
            Public fLastSyncBy As System.String = "LastSyncBy"

            Protected _VehicleID As System.String
            Private _RegNo As System.String
            Private _Ownership As System.String
            Private _CompanyID As System.String
            Private _CompanyDesc As System.String
            Private _VehicleType As System.String
            Private _BDM As System.String
            Private _ManufacturingYear As System.String
            Private _RegistrationYear As System.String
            Private _GPSInstallation As System.Byte
            Private _BinLifterInstallation As System.Byte
            Private _TruckPainting As System.Byte
            Private _RegistrationCard As System.Byte
            Private _Photos As System.Byte
            Private _Status As System.Byte
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _Inuse As System.Byte
            Private _rowguid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _IsHost As System.Byte
            Private _LastSyncBy As System.String
            Private _Flag As System.Byte
            Private _FromTransporter As System.Byte
            Private _StatusAdd As System.Int16


            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property VehicleID As System.String
                Get
                    Return _VehicleID
                End Get
                Set(ByVal Value As System.String)
                    _VehicleID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RegNo As System.String
                Get
                    Return _RegNo
                End Get
                Set(ByVal Value As System.String)
                    _RegNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Ownership As System.String
                Get
                    Return _Ownership
                End Get
                Set(ByVal Value As System.String)
                    _Ownership = Value
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
            Public Property CompanyDesc As System.String
                Get
                    Return _CompanyDesc
                End Get
                Set(ByVal Value As System.String)
                    _CompanyDesc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property VehicleType As System.String
                Get
                    Return _VehicleType
                End Get
                Set(ByVal Value As System.String)
                    _VehicleType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property BDM As System.String
                Get
                    Return _BDM
                End Get
                Set(ByVal Value As System.String)
                    _BDM = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ManufacturingYear As System.String
                Get
                    Return _ManufacturingYear
                End Get
                Set(ByVal Value As System.String)
                    _ManufacturingYear = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RegistrationYear As System.String
                Get
                    Return _RegistrationYear
                End Get
                Set(ByVal Value As System.String)
                    _RegistrationYear = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property GPSInstallation As System.Byte
                Get
                    Return _GPSInstallation
                End Get
                Set(ByVal Value As System.Byte)
                    _GPSInstallation = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property BinLifterInstallation As System.Byte
                Get
                    Return _BinLifterInstallation
                End Get
                Set(ByVal Value As System.Byte)
                    _BinLifterInstallation = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TruckPainting As System.Byte
                Get
                    Return _TruckPainting
                End Get
                Set(ByVal Value As System.Byte)
                    _TruckPainting = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RegistrationCard As System.Byte
                Get
                    Return _RegistrationCard
                End Get
                Set(ByVal Value As System.Byte)
                    _RegistrationCard = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Photos As System.Byte
                Get
                    Return _Photos
                End Get
                Set(ByVal Value As System.Byte)
                    _Photos = Value
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

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property FromTransporter As System.Byte
                Get
                    Return _FromTransporter
                End Get
                Set(ByVal Value As System.Byte)
                    _FromTransporter = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property StatusAdd As System.Int16
                Get
                    Return _StatusAdd
                End Get
                Set(ByVal Value As System.Int16)
                    _StatusAdd = Value
                End Set
            End Property

        End Class
#End Region
    End Namespace

#Region "Class Info"
    Public Class VehicleInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "VehicleID,RegNo,Ownership,CompanyID, ISNULL((SELECT TOP 1 CompanyName FROM BIZENTITY WITH (NOLOCK) WHERE BizRegID=Vehicle.CompanyID),'') AS CompanyDesc,VehicleType,BDM,ManufacturingYear,RegistrationYear,GPSInstallation,BinLifterInstallation,TruckPainting,RegistrationCard,Photos,Status,CreateDate,CreateBy,LastUpdate,UpdateBy,Flag,Inuse,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
                .CheckFields = "GPSInstallation,BinLifterInstallation,TruckPainting,RegistrationCard,Photos,Status,Flag,Inuse,IsHost"
                .TableName = "VEHICLE WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "VehicleID,RegNo,Ownership,CompanyID,VehicleType,BDM,ManufacturingYear,RegistrationYear,GPSInstallation,BinLifterInstallation,TruckPainting,RegistrationCard,Photos,Status,CreateDate,CreateBy,LastUpdate,UpdateBy,Flag,Inuse,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
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
    Public Class VehicleScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "VehicleID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "RegNo"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Ownership"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CompanyID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "VehicleType"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "BDM"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ManufacturingYear"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "RegistrationYear"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "GPSInstallation"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "BinLifterInstallation"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "TruckPainting"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "RegistrationCard"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Photos"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Status"
                .Length = 1
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
                .FieldName = "Flag"
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
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(21, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(22, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsHost"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(23, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(24, this)

        End Sub

        Public ReadOnly Property VehicleID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property RegNo As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property Ownership As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property CompanyID As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property VehicleType As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property BDM As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property ManufacturingYear As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property RegistrationYear As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property GPSInstallation As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property BinLifterInstallation As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property TruckPainting As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property RegistrationCard As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property Photos As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property Status As StrucElement
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
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property Inuse As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(22)
            End Get
        End Property
        Public ReadOnly Property IsHost As StrucElement
            Get
                Return MyBase.GetItem(23)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(24)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region

End Namespace