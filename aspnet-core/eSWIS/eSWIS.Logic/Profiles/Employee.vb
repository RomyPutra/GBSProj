Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Profiles
    Public NotInheritable Class Employee
        Inherits Core.CoreControl
        Private EmployeeInfo As EmployeeInfo = New EmployeeInfo
        Private UserProfileInfo As UserSecurity.UserProfileInfo = New UserSecurity.UserProfileInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub


#Region "Data Manipulation-Add,Edit,Del"

        'function for Driver Vehicle
        Private Function UpdateStatusAdds(ByVal ListDriverCont As List(Of Container.Employee), ByVal ListVehicleCont As List(Of Profiles.Container.Vehicle), ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()
            UpdateStatusAdds = False

            If ListDriverCont Is Nothing AndAlso ListDriverCont.Count > 0 Then
                'Message return
            Else
                StartSQLControl()

                For Each row As Container.Employee In ListDriverCont
                    strSQL = "UPDATE EMPLOYEE SET StatusAdd = 1" & _
                        " WHERE EmployeeID='" & row.EmployeeID & "'"
                    ListSQL.Add(strSQL)
                Next
            End If
            If ListVehicleCont Is Nothing AndAlso ListVehicleCont.Count > 0 Then
                'Message return
            Else
                StartSQLControl()

                For Each row As Profiles.Container.Vehicle In ListVehicleCont
                    strSQL = "UPDATE Vehicle SET StatusAdd = 1" & _
                        " WHERE VehicleID='" & row.VehicleID & "'"
                    ListSQL.Add(strSQL)
                Next
            End If

            Try
                objConn.BatchExecute(ListSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
            Catch axAssign As ApplicationException
                message = axAssign.Message.ToString()
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Return False
            Finally
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
            Return True
        End Function

        Public Function SaveDriverVehicle(ByVal ListDriverCont As List(Of Container.Employee), ByVal ListVehicleCont As List(Of Profiles.Container.Vehicle), ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()
            SaveDriverVehicle = False

            Try
                StartSQLControl()

                'save all details
                If ListDriverCont IsNot Nothing AndAlso ListDriverCont.Count > 0 Then

                    blnExec = False
                    blnFound = False
                    blnFlag = False

                    For Each EmployeeCont In ListDriverCont
                        Dim EmployeeInfo As Profiles.EmployeeInfo = New Profiles.EmployeeInfo
                        With EmployeeInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "NRICNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeCont.NRICNo) & "'")
                        End With
                        rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)
                        blnExec = True

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                End If
                                .Close()
                            End With
                        End If
                    Next
                    For Each EmployeeCont In ListDriverCont
                        With objSQL
                            .TableName = "EMPLOYEE WITH (ROWLOCK)"
                            .AddField("NickName", EmployeeCont.NickName, SQLControl.EnumDataType.dtString) 'used
                            .AddField("NRICNo", EmployeeCont.NRICNo, SQLControl.EnumDataType.dtStringN) 'used
                            .AddField("Status", EmployeeCont.Status, SQLControl.EnumDataType.dtNumeric) 'used
                            .AddField("CreateDate", EmployeeCont.CreateDate, SQLControl.EnumDataType.dtDateTime) 'used
                            .AddField("CreateBy", EmployeeCont.CreateBy, SQLControl.EnumDataType.dtString) 'used
                            .AddField("CompanyID", EmployeeCont.CompanyID, SQLControl.EnumDataType.dtString) 'used
                            .AddField("locID", EmployeeCont.LocID, SQLControl.EnumDataType.dtString) 'used
                            .AddField("Flag", EmployeeCont.Flag, SQLControl.EnumDataType.dtNumeric) 'used
                            .AddField("Designation", EmployeeCont.Designation, SQLControl.EnumDataType.dtString) 'used
                            .AddField("FromTransporter", EmployeeCont.FromTransporter, SQLControl.EnumDataType.dtNumeric) 'used
                            .AddField("StatusAdd", EmployeeCont.StatusAdd, SQLControl.EnumDataType.dtNumeric) 'used

                            If blnFound = True Then
                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "EmployeeID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeCont.EmployeeID) & "'")
                            Else
                                .AddField("EmployeeID", EmployeeCont.EmployeeID, SQLControl.EnumDataType.dtString)
                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                            End If
                        End With
                        ListSQL.Add(strSQL)
                    Next
                End If
                If ListVehicleCont IsNot Nothing AndAlso ListVehicleCont.Count > 0 Then

                    blnExec = False
                    blnFound = False
                    blnFlag = False

                    objSQL.ClearFields()
                    For Each VehicleCont In ListVehicleCont
                        Dim VehicleInfo As Profiles.VehicleInfo = New Profiles.VehicleInfo
                        With VehicleInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "VehicleID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, VehicleCont.VehicleID) & "'")
                        End With
                        rdr = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataReader, CommandType.Text, , False), Data.SqlClient.SqlDataReader)
                        blnExec = True

                        If rdr Is Nothing = False Then
                            With rdr
                                If .Read Then
                                    blnFound = True
                                End If
                                .Close()
                            End With
                        End If
                    Next
                    For Each VehicleCont In ListVehicleCont
                        With objSQL
                            .TableName = "VEHICLE WITH (ROWLOCK)"
                            .AddField("RegNo", VehicleCont.RegNo, SQLControl.EnumDataType.dtString) 'used
                            .AddField("CompanyID", VehicleCont.CompanyID, SQLControl.EnumDataType.dtString) 'used
                            .AddField("CreateDate", VehicleCont.CreateDate, SQLControl.EnumDataType.dtDateTime) 'used
                            .AddField("CreateBy", VehicleCont.CreateBy, SQLControl.EnumDataType.dtString) 'used
                            .AddField("LastSyncBy", VehicleCont.LastSyncBy, SQLControl.EnumDataType.dtString) 'used
                            .AddField("Flag", VehicleCont.Flag, SQLControl.EnumDataType.dtString) 'used
                            .AddField("Status", VehicleCont.Status, SQLControl.EnumDataType.dtString) 'used
                            .AddField("FromTransporter", VehicleCont.FromTransporter, SQLControl.EnumDataType.dtNumeric) 'used
                            .AddField("StatusAdd", VehicleCont.StatusAdd, SQLControl.EnumDataType.dtNumeric) 'used

                            If blnFound = True Then
                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "VehicleID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, VehicleCont.VehicleID) & "'")
                            Else
                                .AddField("VehicleID", VehicleCont.VehicleID, SQLControl.EnumDataType.dtString)
                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                            End If
                        End With
                        ListSQL.Add(strSQL)
                    Next
                End If

                Try
                    objConn.BatchExecute(ListSQL, CommandType.Text)
                    Return True
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
                    Gibraltar.Agent.Log.Error("Employee", axExecute.Message & sqlStatement, axExecute.StackTrace)
                    Return False
                Finally
                    objSQL.Dispose()
                End Try

            Catch axAssign As ApplicationException
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("Employee", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("Employee", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally

                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Private Function Save(ByVal EmployeeCont As Container.Employee, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Dim ListSQL As ArrayList = New ArrayList()

            Try
                If EmployeeCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With EmployeeInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "EmployeeID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeCont.EmployeeID) & "'")
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
                                .TableName = "EMPLOYEE WITH (ROWLOCK)"
                                .AddField("NickName", EmployeeCont.NickName, SQLControl.EnumDataType.dtString)
                                .AddField("SurName", EmployeeCont.SurName, SQLControl.EnumDataType.dtStringN)
                                .AddField("FirstName", EmployeeCont.FirstName, SQLControl.EnumDataType.dtStringN)
                                .AddField("Salutation", EmployeeCont.Salutation, SQLControl.EnumDataType.dtString)
                                .AddField("Sex", EmployeeCont.Sex, SQLControl.EnumDataType.dtString)
                                .AddField("DOB", EmployeeCont.DOB, SQLControl.EnumDataType.dtDateTime)
                                .AddField("PlaceOfBirth", EmployeeCont.PlaceOfBirth, SQLControl.EnumDataType.dtStringN)
                                .AddField("NRICNo", EmployeeCont.NRICNo, SQLControl.EnumDataType.dtStringN)
                                .AddField("Nationality", EmployeeCont.Nationality, SQLControl.EnumDataType.dtString)
                                .AddField("Race", EmployeeCont.Race, SQLControl.EnumDataType.dtString)
                                .AddField("Religion", EmployeeCont.Religion, SQLControl.EnumDataType.dtString)
                                .AddField("Marital", EmployeeCont.Marital, SQLControl.EnumDataType.dtString)
                                .AddField("CoAddress1", EmployeeCont.CoAddress1, SQLControl.EnumDataType.dtString)
                                .AddField("CoAddress2", EmployeeCont.CoAddress2, SQLControl.EnumDataType.dtString)
                                .AddField("CoAddress3", EmployeeCont.CoAddress3, SQLControl.EnumDataType.dtString)
                                .AddField("CoAddress4", EmployeeCont.CoAddress4, SQLControl.EnumDataType.dtString)
                                .AddField("CoPostalCode", EmployeeCont.CoPostalCode, SQLControl.EnumDataType.dtString)
                                .AddField("CoState", EmployeeCont.CoState, SQLControl.EnumDataType.dtString)
                                .AddField("CoCountry", EmployeeCont.CoCountry, SQLControl.EnumDataType.dtString)
                                .AddField("PnAddress1", EmployeeCont.PnAddress1, SQLControl.EnumDataType.dtString)
                                .AddField("PnAddress2", EmployeeCont.PnAddress2, SQLControl.EnumDataType.dtString)
                                .AddField("PnAddress3", EmployeeCont.PnAddress3, SQLControl.EnumDataType.dtString)
                                .AddField("PnAddress4", EmployeeCont.PnAddress4, SQLControl.EnumDataType.dtString)
                                .AddField("PnPostalCode", EmployeeCont.PnPostalCode, SQLControl.EnumDataType.dtString)
                                .AddField("PnState", EmployeeCont.PnState, SQLControl.EnumDataType.dtString)
                                .AddField("PnCountry", EmployeeCont.PnCountry, SQLControl.EnumDataType.dtString)
                                .AddField("EmerContactPerson", EmployeeCont.EmerContactPerson, SQLControl.EnumDataType.dtStringN)
                                .AddField("EmerContactNo", EmployeeCont.EmerContactNo, SQLControl.EnumDataType.dtString)
                                .AddField("EmailAddress", EmployeeCont.EmailAddress, SQLControl.EnumDataType.dtStringN)
                                .AddField("Designation", EmployeeCont.Designation, SQLControl.EnumDataType.dtStringN)
                                .AddField("ForeignLocal", EmployeeCont.ForeignLocal, SQLControl.EnumDataType.dtString)
                                .AddField("CommID", EmployeeCont.CommID, SQLControl.EnumDataType.dtString)
                                .AddField("Salary", EmployeeCont.Salary, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OffDay", EmployeeCont.OffDay, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Overtime", EmployeeCont.Overtime, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Leave", EmployeeCont.Leave, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Levy", EmployeeCont.Levy, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Allergies", EmployeeCont.Allergies, SQLControl.EnumDataType.dtStringN)
                                .AddField("ClerkNo", EmployeeCont.ClerkNo, SQLControl.EnumDataType.dtStringN)
                                .AddField("DateHired", EmployeeCont.DateHired, SQLControl.EnumDataType.dtDateTime)
                                .AddField("DateLeft", EmployeeCont.DateLeft, SQLControl.EnumDataType.dtDateTime)
                                .AddField("TransportAllowance", EmployeeCont.TransportAllowance, SQLControl.EnumDataType.dtStringN)
                                .AddField("ServiceAllowance", EmployeeCont.ServiceAllowance, SQLControl.EnumDataType.dtStringN)
                                .AddField("OtherAllowance", EmployeeCont.OtherAllowance, SQLControl.EnumDataType.dtStringN)
                                .AddField("Remarks", EmployeeCont.Remarks, SQLControl.EnumDataType.dtStringN)
                                .AddField("PrivilegeCode", EmployeeCont.PrivilegeCode, SQLControl.EnumDataType.dtString)
                                .AddField("Status", EmployeeCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", EmployeeCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", EmployeeCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", EmployeeCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", EmployeeCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Inuse", EmployeeCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsHost", EmployeeCont.IsHost, SQLControl.EnumDataType.dtNumeric)
                                .AddField("AccessLvl", EmployeeCont.AccessLvl, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CompanyID", EmployeeCont.CompanyID, SQLControl.EnumDataType.dtString)
                                .AddField("locID", EmployeeCont.LocID, SQLControl.EnumDataType.dtString)
                                .AddField("Flag", EmployeeCont.Flag, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "EmployeeID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeCont.EmployeeID) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("EmployeeID", EmployeeCont.EmployeeID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                        ListSQL.Add(strSQL)

                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "EmployeeID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeCont.EmployeeID) & "'")
                                        ListSQL.Add(strSQL)  'update status employee


                                        If message = "DeleteEmpbranch" Then
                                            With objSQL
                                                .TableName = "EMPBRANCH WITH (ROWLOCK)"
                                                strSQL = BuildDelete(.TableName, "EmployeeID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeCont.EmployeeID) & "'")
                                                ListSQL.Add(strSQL)
                                            End With
                                            
                                        End If
                                End Select
                            End With
                            Try
                                objConn.BatchExecute(ListSQL, CommandType.Text)
                                Return True

                            Catch axExecute As Exception

                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                Else
                                    message = axExecute.Message.ToString()
                                End If
                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Employee", axExecute.Message & strSQL, axExecute.StackTrace)
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
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Employee", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Employee", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                EmployeeCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function InsertDriverVehicle(ByVal ListDriverCont As List(Of Container.Employee), ByVal ListVehicleCont As List(Of Profiles.Container.Vehicle), ByRef message As String) As Boolean
            Return SaveDriverVehicle(ListDriverCont, ListVehicleCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        Public Function UpdateStatusAdd(ByVal ListDriverCont As List(Of Container.Employee), ByVal ListVehicleCont As List(Of Profiles.Container.Vehicle), ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return UpdateStatusAdds(ListDriverCont, ListVehicleCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        'ADD
        Public Function Insert(ByVal EmployeeCont As Container.Employee, ByRef message As String) As Boolean
            Return Save(EmployeeCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal EmployeeCont As Container.Employee, ByRef message As String) As Boolean
            Return Save(EmployeeCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal EmployeeCont As Container.Employee, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Dim ListSQL As ArrayList = New ArrayList()

            Try
                If EmployeeCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With EmployeeInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "EmployeeID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeCont.EmployeeID) & "'")
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

                        With objSQL
                            strSQL = BuildUpdate("EMPLOYEE WITH (ROWLOCK)", " SET Flag = 0" & _
                            " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , UpdateBy = '" & _
                            objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeCont.UpdateBy) & "' WHERE " & _
                            "EmployeeID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeCont.EmployeeID) & "'")
                            ListSQL.Add(strSQL)

                            strSQL = BuildUpdate("USRPROFILE WITH (ROWLOCK)", " SET Status = 0" & _
                            " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , UpdateBy = '" & _
                            objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeCont.UpdateBy) & "' WHERE " & _
                            "RefID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeCont.EmployeeID) & "'")
                            ListSQL.Add(strSQL)
                        End With

                        Try
                            objConn.BatchExecute(ListSQL, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            Dim sqlStatement As String = " "
                            If objConn.FailedSQLStatement.Count > 0 Then
                                sqlStatement &= objConn.FailedSQLStatement.Item(0)
                            End If

                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Employee", exExecute.Message & sqlStatement, exExecute.StackTrace)
                            Return False
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Employee", axDelete.Message, axDelete.StackTrace)
                Return False
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Employee", exDelete.Message, exDelete.StackTrace)
                Return False
            Finally
                EmployeeCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function UpdateAssignSupportList(ByRef message As String, Optional ByVal State As String = Nothing) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim strFilter As String = ""
            UpdateAssignSupportList = False
            blnFound = False
            blnInUse = False
            Try

                If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                    StartSQLControl()
                    If State IsNot Nothing Then
                        strFilter = "WHERE COSTATE='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, State) & "'"
                    End If
                    strSQL = "UPDATE EMPLOYEE WITH (ROWLOCK) SET CommID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, "0") & "', LastUpdate=getdate() " & strFilter

                    objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)

                End If

            Catch exExecute As ApplicationException
                message = exExecute.Message.ToString()
                Log.Notifier.Notify(exExecute)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/SupportList", exExecute.Message, exExecute.StackTrace)
                Return False
            Catch exExecute As Exception
                message = exExecute.Message.ToString()
                Log.Notifier.Notify(exExecute)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/SupportList", exExecute.Message, exExecute.StackTrace)
                Return False
            Finally

                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Function IsUserExist(ByVal UserID As String) As String
            Dim strSQL As String

            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim dt As New DataTable
            Dim temp As String = ""

            Try

                If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                    StartSQLControl()

                    strSQL = "SELECT E.EmployeeID FROM EMPLOYEE E, USRPROFILE U WHERE E.LocID = U.ParentID+'1' AND E.EmployeeID=U.RefID AND U.UserName='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UserID) & "'"

                    'execute
                    dt = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, "EMPLOYEE"), Data.DataTable)

                    If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                        temp = dt.Rows(0)("EmployeeID").ToString
                    End If
                End If

            Catch exExecute As Exception
                Log.Notifier.Notify(exExecute)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Employee", exExecute.Message, exExecute.StackTrace)

            Finally

                rdr = Nothing
                dt = Nothing
                EndSQLControl()
                EndConnection()
            End Try

            Return temp
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetActivityLog(Optional ByVal FieldCond As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With EmployeeInfo.MyInfo
                    StartSQLControl()
                    strSQL = "select UserID,LogID,DeviceID,SessionID,Remark,LogDate,'http://maps.google.com/maps/api/staticmap?center=' + latitude +',' +longitude + '&zoom=15&size=400x400&sensor=false&markers=color:green|' +latitude+',' +longitude  as Map,latitude +',' +longitude as LMap from mob_actlog  " & _
                            " WHERE " & FieldCond & " ORDER BY LogDate DESC"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
                EndSQLControl()
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetEmployeeListMobile(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With EmployeeInfo.MyInfo
                    StartSQLControl()
                    strSQL = "SELECT UV.UserID, EmployeeID, NickName,UV.SyncCreate,UV.SyncCreate,UV.SyncLastUpd,UV.LastSyncBy,Remark,case when UV.Status='0' then 'Pending' when UV.Status='1' then 'Approved' ELSE 'Rejected' End as Status " & _
                            " FROM USRVERIFY UV WITH(NOLOCK) " & _
                            " LEFT JOIN USRPROFILE UP ON UV.UserID=UP.UserID " & _
                            " LEFT JOIN EMPLOYEE E ON UP.RefID=E.EmployeeID " & _
                            " LEFT JOIN USRAPP WITH (NOLOCK) ON UP.UserID = USRAPP.UserID " & _
                            " LEFT JOIN USRGROUP g WITH (NOLOCK) ON g.GroupCode = USRAPP.AccessCode AND g.AppID = USRAPP.APPID " & _
                            " WHERE " & FieldCond & " ORDER BY UV.SYNCCREATE DESC"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
                EndSQLControl()
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetEmployeeMobile(ByVal UserID As System.String, ByVal EmployeeID As System.String) As Container.Employee
            Dim rEmployee As Container.Employee = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With EmployeeInfo.MyInfo
                        strSQL = BuildSelect("CodeDesc,UV.UserID as UserID, UV.Verikey as DeviceID, EmployeeID, NickName,UV.SyncCreate,UV.SyncCreate,UV.SyncLastUpd,UV.LastSyncBy,Remark,UV.DeviceBrand,case when UV.Status='0' then 'Pending' when UV.Status='1' then 'Approved' Else 'Rejected' End as Status", "USRVERIFY UV WITH(NOLOCK) INNER JOIN USRPROFILE UP WITH(NOLOCK) ON UV.UserID=UP.UserID  INNER JOIN EMPLOYEE E WITH(NOLOCK) ON UP.RefID=E.EmployeeID INNER JOIN CODEMASTER CD WITH(NOLOCK) ON CD.Code=UV.veritype AND CODETYPE='MBY'", "EmployeeID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeID) & "' AND UV.UserID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UserID) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rEmployee = New Container.Employee
                                rEmployee.EmployeeID = drRow.Item("EmployeeID")
                                rEmployee.UserID = drRow.Item("UserID")
                                rEmployee.NickName = drRow.Item("NickName")
                                rEmployee.SyncCreate = drRow.Item("SyncCreate")
                                rEmployee.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rEmployee.LastSyncBy = drRow.Item("LastSyncBy")
                                rEmployee.StatusApprove = drRow.Item("Status")
                                rEmployee.Remark = drRow.Item("Remark")
                                rEmployee.Device = drRow.Item("CodeDesc")
                                rEmployee.DeviceID = drRow.Item("DeviceID")
                                rEmployee.Brand = drRow.Item("DeviceBrand")

                            Else
                                rEmployee = Nothing
                            End If
                        Else
                            rEmployee = Nothing
                        End If
                    End With
                End If
                Return rEmployee
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Employee", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rEmployee = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetEmployeeEmailState(ByVal State As String, ByVal Country As String) As Data.DataTable
            Try
                If StartConnection() = True Then
                    With EmployeeInfo.MyInfo
                        StartSQLControl()
                        strSQL = "SELECT ROW_NUMBER() OVER(ORDER BY e.PnState, e.NickName ASC) AS RecNo, " &
                                 " e.EmployeeID, " &
                                 " s.StateDesc, " &
                                 " UPPER(e.NickName) AS SurName, " &
                                 " e.EmailAddress, " &
                                 " e.EmerContactNo, " &
                                 " e.PnAddress1 + CASE e.PnAddress2 WHEN '' THEN '' ELSE ',' + e.PnAddress2 END + CASE e.PnAddress3 WHEN '' THEN '' ELSE ',' + e.PnAddress3 END + CASE e.PnAddress4 WHEN '' THEN '' ELSE ',' + e.PnAddress4 END AS Address " &
                                 " FROM EMPLOYEE e WITH (NOLOCK) " &
                                 " INNER JOIN STATE s WITH (NOLOCK) on s.CountryCode = e.PnCountry and s.StateCode = e.PnState " &
                                 " WHERE e.Flag = 1 and e.CompanyID = '' and e.LocID = '' and e.CommID = 1 AND s.StateCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, State) & "'" &
                                 " AND s.CountryCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, Country) & "'"
                        Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                    End With
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Employee", ex.Message & " " & strSQL, ex.StackTrace)
            Finally
                EndSQLControl()
                EndConnection()
            End Try
            Return Nothing
        End Function

        Public Overloads Function GetEmployee(ByVal EmployeeID As System.String, Optional ByVal msg As String = Nothing) As Container.Employee
            Dim rEmployee As Container.Employee = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With EmployeeInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "EmployeeID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeID) & "'" & msg)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rEmployee = New Container.Employee
                                rEmployee.EmployeeID = drRow.Item("EmployeeID")
                                rEmployee.AccNo = drRow.Item("AccNo")
                                rEmployee.BranchName = drRow.Item("BranchName")
                                rEmployee.NickName = drRow.Item("NickName")
                                rEmployee.SurName = drRow.Item("SurName")
                                rEmployee.FirstName = drRow.Item("FirstName")
                                rEmployee.Salutation = drRow.Item("Salutation")
                                rEmployee.Sex = drRow.Item("Sex")
                                rEmployee.DOB = drRow.Item("DOB")
                                rEmployee.PlaceOfBirth = drRow.Item("PlaceOfBirth")
                                rEmployee.NRICNo = drRow.Item("NRICNo")
                                rEmployee.Nationality = drRow.Item("Nationality")
                                rEmployee.Race = drRow.Item("Race")
                                rEmployee.Religion = drRow.Item("Religion")
                                rEmployee.Marital = drRow.Item("Marital")
                                rEmployee.CoAddress1 = drRow.Item("CoAddress1")
                                rEmployee.CoAddress2 = drRow.Item("CoAddress2")
                                rEmployee.CoAddress3 = drRow.Item("CoAddress3")
                                rEmployee.CoAddress4 = drRow.Item("CoAddress4")
                                rEmployee.CoPostalCode = drRow.Item("CoPostalCode")
                                rEmployee.CoState = drRow.Item("CoState")
                                rEmployee.CoCountry = drRow.Item("CoCountry")
                                rEmployee.PnAddress1 = drRow.Item("PnAddress1")
                                rEmployee.PnAddress2 = drRow.Item("PnAddress2")
                                rEmployee.PnAddress3 = drRow.Item("PnAddress3")
                                rEmployee.PnAddress4 = drRow.Item("PnAddress4")
                                rEmployee.PnPostalCode = drRow.Item("PnPostalCode")
                                rEmployee.PnState = drRow.Item("PnState")
                                rEmployee.PnCountry = drRow.Item("PnCountry")
                                rEmployee.Designation = drRow.Item("Designation")
                                rEmployee.Salary = drRow.Item("Salary")
                                rEmployee.OffDay = drRow.Item("OffDay")
                                rEmployee.Overtime = drRow.Item("Overtime")
                                rEmployee.Leave = drRow.Item("Leave")
                                rEmployee.Levy = drRow.Item("Levy")
                                rEmployee.Allergies = drRow.Item("Allergies")
                                rEmployee.ClerkNo = drRow.Item("ClerkNo")
                                rEmployee.TransportAllowance = drRow.Item("TransportAllowance")
                                rEmployee.ServiceAllowance = drRow.Item("ServiceAllowance")
                                rEmployee.OtherAllowance = drRow.Item("OtherAllowance")
                                rEmployee.Remarks = drRow.Item("Remarks")
                                rEmployee.PrivilegeCode = drRow.Item("PrivilegeCode")
                                rEmployee.Status = drRow.Item("Status")
                                rEmployee.CreateBy = drRow.Item("CreateBy")
                                rEmployee.UpdateBy = drRow.Item("UpdateBy")
                                rEmployee.Inuse = drRow.Item("Inuse")
                                rEmployee.rowguid = drRow.Item("rowguid")
                                rEmployee.SyncCreate = drRow.Item("SyncCreate")
                                rEmployee.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rEmployee.IsHost = drRow.Item("IsHost")
                                rEmployee.LastSyncBy = drRow.Item("LastSyncBy")
                                rEmployee.AccessLvl = drRow.Item("AccessLvl")
                                rEmployee.CompanyID = drRow.Item("CompanyID")
                                rEmployee.LocID = drRow.Item("LocID")
                                rEmployee.EmerContactNo = drRow.Item("EmerContactNo")
                                rEmployee.EmailAddress = drRow.Item("EmailAddress")

                            Else
                                rEmployee = Nothing
                            End If
                        Else
                            rEmployee = Nothing
                        End If
                    End With
                End If
                Return rEmployee
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Employee", ex.Message, ex.StackTrace)
            Finally
                rEmployee = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetTransportDriver(ByVal CompanyID As System.String) As List(Of Container.Employee)
            Dim rEmployee As Container.Employee = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstDriver As New List(Of Container.Employee)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With EmployeeInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, " CompanyID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompanyID) & "' AND Status=1 AND Designation=16 AND Flag=1")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                lstDriver = New List(Of Container.Employee)
                                For Each row As DataRow In dtTemp.Rows
                                    rEmployee = New Container.Employee
                                    With rEmployee
                                        rEmployee = New Container.Employee
                                        rEmployee.EmployeeID = row.Item("EmployeeID")
                                        rEmployee.AccNo = row.Item("AccNo")
                                        rEmployee.BranchName = row.Item("BranchName")
                                        rEmployee.NickName = row.Item("NickName")
                                        rEmployee.SurName = row.Item("SurName")
                                        rEmployee.FirstName = row.Item("FirstName")
                                        rEmployee.Salutation = row.Item("Salutation")
                                        rEmployee.Sex = row.Item("Sex")
                                        rEmployee.DOB = row.Item("DOB")
                                        rEmployee.PlaceOfBirth = row.Item("PlaceOfBirth")
                                        rEmployee.NRICNo = row.Item("NRICNo")
                                        rEmployee.Nationality = row.Item("Nationality")
                                        rEmployee.Race = row.Item("Race")
                                        rEmployee.Religion = row.Item("Religion")
                                        rEmployee.Marital = row.Item("Marital")
                                        rEmployee.CoAddress1 = row.Item("CoAddress1")
                                        rEmployee.CoAddress2 = row.Item("CoAddress2")
                                        rEmployee.CoAddress3 = row.Item("CoAddress3")
                                        rEmployee.CoAddress4 = row.Item("CoAddress4")
                                        rEmployee.CoPostalCode = row.Item("CoPostalCode")
                                        rEmployee.CoState = row.Item("CoState")
                                        rEmployee.CoCountry = row.Item("CoCountry")
                                        rEmployee.PnAddress1 = row.Item("PnAddress1")
                                        rEmployee.PnAddress2 = row.Item("PnAddress2")
                                        rEmployee.PnAddress3 = row.Item("PnAddress3")
                                        rEmployee.PnAddress4 = row.Item("PnAddress4")
                                        rEmployee.PnPostalCode = row.Item("PnPostalCode")
                                        rEmployee.PnState = row.Item("PnState")
                                        rEmployee.PnCountry = row.Item("PnCountry")
                                        rEmployee.Designation = row.Item("Designation")
                                        rEmployee.Salary = row.Item("Salary")
                                        rEmployee.OffDay = row.Item("OffDay")
                                        rEmployee.Overtime = row.Item("Overtime")
                                        rEmployee.Leave = row.Item("Leave")
                                        rEmployee.Levy = row.Item("Levy")
                                        rEmployee.Allergies = row.Item("Allergies")
                                        rEmployee.ClerkNo = row.Item("ClerkNo")
                                        rEmployee.TransportAllowance = row.Item("TransportAllowance")
                                        rEmployee.ServiceAllowance = row.Item("ServiceAllowance")
                                        rEmployee.OtherAllowance = row.Item("OtherAllowance")
                                        rEmployee.Remarks = row.Item("Remarks")
                                        rEmployee.PrivilegeCode = row.Item("PrivilegeCode")
                                        rEmployee.Status = row.Item("Status")
                                        rEmployee.CreateBy = row.Item("CreateBy")
                                        rEmployee.UpdateBy = row.Item("UpdateBy")
                                        rEmployee.Inuse = row.Item("Inuse")
                                        rEmployee.rowguid = row.Item("rowguid")
                                        rEmployee.SyncCreate = row.Item("SyncCreate")
                                        rEmployee.SyncLastUpd = row.Item("SyncLastUpd")
                                        rEmployee.IsHost = row.Item("IsHost")
                                        rEmployee.LastSyncBy = row.Item("LastSyncBy")
                                        rEmployee.AccessLvl = row.Item("AccessLvl")
                                        rEmployee.CompanyID = row.Item("CompanyID")
                                        rEmployee.LocID = row.Item("LocID")
                                        rEmployee.EmerContactNo = row.Item("EmerContactNo")
                                        rEmployee.EmailAddress = row.Item("EmailAddress")
                                    End With
                                    lstDriver.Add(rEmployee)
                                Next
                            End If
                        End If
                    End With
                End If
                Return lstDriver
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Employee", ex.Message, ex.StackTrace)
            Finally
                rEmployee = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetEmployeeByNRIC(ByVal NRIC As System.String) As Container.Employee
            Dim rEmployee As Container.Employee = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With EmployeeInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "NRICNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, NRIC) & "' AND Flag=1 AND Designation='16'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rEmployee = New Container.Employee
                                rEmployee.EmployeeID = drRow.Item("EmployeeID")
                                rEmployee.AccNo = drRow.Item("AccNo")
                                rEmployee.BranchName = drRow.Item("BranchName")
                                rEmployee.NickName = drRow.Item("NickName")
                                rEmployee.SurName = drRow.Item("SurName")
                                rEmployee.FirstName = drRow.Item("FirstName")
                                rEmployee.Salutation = drRow.Item("Salutation")
                                rEmployee.Sex = drRow.Item("Sex")
                                rEmployee.DOB = drRow.Item("DOB")
                                rEmployee.PlaceOfBirth = drRow.Item("PlaceOfBirth")
                                rEmployee.NRICNo = drRow.Item("NRICNo")
                                rEmployee.Nationality = drRow.Item("Nationality")
                                rEmployee.Race = drRow.Item("Race")
                                rEmployee.Religion = drRow.Item("Religion")
                                rEmployee.Marital = drRow.Item("Marital")
                                rEmployee.CoAddress1 = drRow.Item("CoAddress1")
                                rEmployee.CoAddress2 = drRow.Item("CoAddress2")
                                rEmployee.CoAddress3 = drRow.Item("CoAddress3")
                                rEmployee.CoAddress4 = drRow.Item("CoAddress4")
                                rEmployee.CoPostalCode = drRow.Item("CoPostalCode")
                                rEmployee.CoState = drRow.Item("CoState")
                                rEmployee.CoCountry = drRow.Item("CoCountry")
                                rEmployee.PnAddress1 = drRow.Item("PnAddress1")
                                rEmployee.PnAddress2 = drRow.Item("PnAddress2")
                                rEmployee.PnAddress3 = drRow.Item("PnAddress3")
                                rEmployee.PnAddress4 = drRow.Item("PnAddress4")
                                rEmployee.PnPostalCode = drRow.Item("PnPostalCode")
                                rEmployee.PnState = drRow.Item("PnState")
                                rEmployee.PnCountry = drRow.Item("PnCountry")
                                rEmployee.Designation = drRow.Item("Designation")
                                rEmployee.Salary = drRow.Item("Salary")
                                rEmployee.OffDay = drRow.Item("OffDay")
                                rEmployee.Overtime = drRow.Item("Overtime")
                                rEmployee.Leave = drRow.Item("Leave")
                                rEmployee.Levy = drRow.Item("Levy")
                                rEmployee.Allergies = drRow.Item("Allergies")
                                rEmployee.ClerkNo = drRow.Item("ClerkNo")
                                rEmployee.TransportAllowance = drRow.Item("TransportAllowance")
                                rEmployee.ServiceAllowance = drRow.Item("ServiceAllowance")
                                rEmployee.OtherAllowance = drRow.Item("OtherAllowance")
                                rEmployee.Remarks = drRow.Item("Remarks")
                                rEmployee.PrivilegeCode = drRow.Item("PrivilegeCode")
                                rEmployee.Status = drRow.Item("Status")
                                rEmployee.CreateBy = drRow.Item("CreateBy")
                                rEmployee.UpdateBy = drRow.Item("UpdateBy")
                                rEmployee.Inuse = drRow.Item("Inuse")
                                rEmployee.rowguid = drRow.Item("rowguid")
                                rEmployee.SyncCreate = drRow.Item("SyncCreate")
                                rEmployee.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rEmployee.IsHost = drRow.Item("IsHost")
                                rEmployee.LastSyncBy = drRow.Item("LastSyncBy")
                                rEmployee.AccessLvl = drRow.Item("AccessLvl")
                                rEmployee.CompanyID = drRow.Item("CompanyID")
                                rEmployee.LocID = drRow.Item("LocID")
                                rEmployee.EmerContactNo = drRow.Item("EmerContactNo")
                                rEmployee.EmailAddress = drRow.Item("EmailAddress")

                            Else
                                rEmployee = Nothing
                            End If
                        Else
                            rEmployee = Nothing
                        End If
                    End With
                End If
                Return rEmployee
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Employee", ex.Message, ex.StackTrace)
            Finally
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetEmployeeByBizRegID(ByVal CompanyID As System.String) As Container.Employee
            Dim rEmployee As Container.Employee = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With EmployeeInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "CompanyID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompanyID) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rEmployee = New Container.Employee
                                rEmployee.EmployeeID = drRow.Item("EmployeeID")
                                rEmployee.AccNo = drRow.Item("AccNo")
                                rEmployee.BranchName = drRow.Item("BranchName")
                                rEmployee.NickName = drRow.Item("NickName")
                                rEmployee.SurName = drRow.Item("SurName")
                                rEmployee.FirstName = drRow.Item("FirstName")
                                rEmployee.Salutation = drRow.Item("Salutation")
                                rEmployee.Sex = drRow.Item("Sex")
                                rEmployee.DOB = drRow.Item("DOB")
                                rEmployee.PlaceOfBirth = drRow.Item("PlaceOfBirth")
                                rEmployee.NRICNo = drRow.Item("NRICNo")
                                rEmployee.Nationality = drRow.Item("Nationality")
                                rEmployee.Race = drRow.Item("Race")
                                rEmployee.Religion = drRow.Item("Religion")
                                rEmployee.Marital = drRow.Item("Marital")
                                rEmployee.CoAddress1 = drRow.Item("CoAddress1")
                                rEmployee.CoAddress2 = drRow.Item("CoAddress2")
                                rEmployee.CoAddress3 = drRow.Item("CoAddress3")
                                rEmployee.CoAddress4 = drRow.Item("CoAddress4")
                                rEmployee.CoPostalCode = drRow.Item("CoPostalCode")
                                rEmployee.CoState = drRow.Item("CoState")
                                rEmployee.CoCountry = drRow.Item("CoCountry")
                                rEmployee.PnAddress1 = drRow.Item("PnAddress1")
                                rEmployee.PnAddress2 = drRow.Item("PnAddress2")
                                rEmployee.PnAddress3 = drRow.Item("PnAddress3")
                                rEmployee.PnAddress4 = drRow.Item("PnAddress4")
                                rEmployee.PnPostalCode = drRow.Item("PnPostalCode")
                                rEmployee.PnState = drRow.Item("PnState")
                                rEmployee.PnCountry = drRow.Item("PnCountry")
                                rEmployee.Designation = drRow.Item("Designation")
                                rEmployee.Salary = drRow.Item("Salary")
                                rEmployee.OffDay = drRow.Item("OffDay")
                                rEmployee.Overtime = drRow.Item("Overtime")
                                rEmployee.Leave = drRow.Item("Leave")
                                rEmployee.Levy = drRow.Item("Levy")
                                rEmployee.Allergies = drRow.Item("Allergies")
                                rEmployee.ClerkNo = drRow.Item("ClerkNo")
                                rEmployee.TransportAllowance = drRow.Item("TransportAllowance")
                                rEmployee.ServiceAllowance = drRow.Item("ServiceAllowance")
                                rEmployee.OtherAllowance = drRow.Item("OtherAllowance")
                                rEmployee.Remarks = drRow.Item("Remarks")
                                rEmployee.PrivilegeCode = drRow.Item("PrivilegeCode")
                                rEmployee.Status = drRow.Item("Status")
                                rEmployee.CreateBy = drRow.Item("CreateBy")
                                rEmployee.UpdateBy = drRow.Item("UpdateBy")
                                rEmployee.Inuse = drRow.Item("Inuse")
                                rEmployee.rowguid = drRow.Item("rowguid")
                                rEmployee.SyncCreate = drRow.Item("SyncCreate")
                                rEmployee.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rEmployee.IsHost = drRow.Item("IsHost")
                                rEmployee.LastSyncBy = drRow.Item("LastSyncBy")
                                rEmployee.AccessLvl = drRow.Item("AccessLvl")
                                rEmployee.CompanyID = drRow.Item("CompanyID")
                                rEmployee.LocID = drRow.Item("LocID")
                                rEmployee.EmerContactNo = drRow.Item("EmerContactNo")
                                rEmployee.EmailAddress = drRow.Item("EmailAddress")

                            Else
                                rEmployee = Nothing
                            End If
                        Else
                            rEmployee = Nothing
                        End If
                    End With
                End If
                Return rEmployee
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Employee", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rEmployee = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetEmployee(ByVal EmployeeID As System.String, ByVal DecendingOrder As Boolean) As List(Of Container.Employee)
            Dim rEmployee As Container.Employee = Nothing
            Dim lstEmployee As List(Of Container.Employee) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With EmployeeInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal EmployeeID As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "EmployeeID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, EmployeeID) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rEmployee = New Container.Employee
                                rEmployee.EmployeeID = drRow.Item("EmployeeID")
                                rEmployee.AccNo = drRow.Item("AccNo")
                                rEmployee.BranchName = drRow.Item("BranchName")
                                rEmployee.NickName = drRow.Item("NickName")
                                rEmployee.SurName = drRow.Item("SurName")
                                rEmployee.FirstName = drRow.Item("FirstName")
                                rEmployee.Salutation = drRow.Item("Salutation")
                                rEmployee.Sex = drRow.Item("Sex")
                                rEmployee.PlaceOfBirth = drRow.Item("PlaceOfBirth")
                                rEmployee.NRICNo = drRow.Item("NRICNo")
                                rEmployee.Nationality = drRow.Item("Nationality")
                                rEmployee.Race = drRow.Item("Race")
                                rEmployee.Religion = drRow.Item("Religion")
                                rEmployee.Marital = drRow.Item("Marital")
                                rEmployee.CoAddress1 = drRow.Item("CoAddress1")
                                rEmployee.CoAddress2 = drRow.Item("CoAddress2")
                                rEmployee.CoAddress3 = drRow.Item("CoAddress3")
                                rEmployee.CoAddress4 = drRow.Item("CoAddress4")
                                rEmployee.CoPostalCode = drRow.Item("CoPostalCode")
                                rEmployee.CoState = drRow.Item("CoState")
                                rEmployee.CoCountry = drRow.Item("CoCountry")
                                rEmployee.PnAddress1 = drRow.Item("PnAddress1")
                                rEmployee.PnAddress2 = drRow.Item("PnAddress2")
                                rEmployee.PnAddress3 = drRow.Item("PnAddress3")
                                rEmployee.PnAddress4 = drRow.Item("PnAddress4")
                                rEmployee.PnPostalCode = drRow.Item("PnPostalCode")
                                rEmployee.PnState = drRow.Item("PnState")
                                rEmployee.PnCountry = drRow.Item("PnCountry")
                                rEmployee.Designation = drRow.Item("Designation")
                                rEmployee.Salary = drRow.Item("Salary")
                                rEmployee.OffDay = drRow.Item("OffDay")
                                rEmployee.Overtime = drRow.Item("Overtime")
                                rEmployee.Leave = drRow.Item("Leave")
                                rEmployee.Levy = drRow.Item("Levy")
                                rEmployee.Allergies = drRow.Item("Allergies")
                                rEmployee.ClerkNo = drRow.Item("ClerkNo")
                                rEmployee.TransportAllowance = drRow.Item("TransportAllowance")
                                rEmployee.ServiceAllowance = drRow.Item("ServiceAllowance")
                                rEmployee.OtherAllowance = drRow.Item("OtherAllowance")
                                rEmployee.Remarks = drRow.Item("Remarks")
                                rEmployee.PrivilegeCode = drRow.Item("PrivilegeCode")
                                rEmployee.Status = drRow.Item("Status")
                                rEmployee.CreateBy = drRow.Item("CreateBy")
                                rEmployee.UpdateBy = drRow.Item("UpdateBy")
                                rEmployee.Inuse = drRow.Item("Inuse")
                                rEmployee.rowguid = drRow.Item("rowguid")
                                rEmployee.SyncCreate = drRow.Item("SyncCreate")
                                rEmployee.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rEmployee.IsHost = drRow.Item("IsHost")
                                rEmployee.LastSyncBy = drRow.Item("LastSyncBy")
                                rEmployee.AccessLvl = drRow.Item("AccessLvl")
                                rEmployee.CompanyID = drRow.Item("CompanyID")
                                rEmployee.LocID = drRow.Item("LocID")
                            Next
                            lstEmployee.Add(rEmployee)
                        Else
                            rEmployee = Nothing
                        End If
                        Return lstEmployee
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Employee", ex.Message, ex.StackTrace)
            Finally
                rEmployee = Nothing
                lstEmployee = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetEmployeeEmailList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With EmployeeInfo.MyInfo
                    If SQL = Nothing Or SQL = String.Empty Then
                        strSQL = " SELECT EMAILADDRESS FROM EMPLOYEE emp WITH (NOLOCK) " & _
                                 " INNER JOIN USRPROFILE usr WITH (NOLOCK) ON emp.EmployeeID = usr.RefID" & _
                                 " INNER JOIN USRAPP app WITH (NOLOCK) ON usr.UserID = app.UserID "
                        If FieldCond IsNot Nothing AndAlso FieldCond <> "" Then
                            strSQL &= " WHERE " & FieldCond & ""
                        End If
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

        Public Overloads Function GetEmployeeList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With EmployeeInfo.MyInfo
                    If SQL = Nothing Or SQL = String.Empty Then
                        strSQL = "select DISTINCT e.*, l.BranchName, PnState, StateDesc as State, NickName as OfficerName, isnull(convert(varchar,LastLogin,103),'') as LastLogin" &
                           " from employee e inner join usrprofile u on e.employeeid = u.refID inner join bizlocate l on l.bizlocID = e.locID " &
                           " inner join state s on e.PnState = s.statecode "
                        If FieldCond IsNot Nothing AndAlso FieldCond <> "" Then
                            strSQL &= " WHERE " & FieldCond & ""
                        End If
                        strSQL &= " order by lastlogin asc"
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

        Public Overloads Function GetEmployeeListEntity(Optional ByVal Flag As String = Nothing, Optional ByVal CompanyID As String = Nothing, Optional ByVal LocID As String = Nothing, Optional ByVal Designation As String = Nothing, Optional ByVal Stats As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With EmployeeInfo.MyInfo
                    strSQL = "SELECT DISTINCT e.*, l.BranchName, PnState, StateDesc as State, NickName as OfficerName, " &
                             " isnull(convert(varchar,LastLogin,103),'') as LastLogin" &
                             " FROM employee e WITH (NOLOCK) INNER JOIN usrprofile u WITH (NOLOCK) ON e.employeeid = u.refID " &
                             " INNER JOIN bizlocate l WITH (NOLOCK) on l.bizlocID = e.locID " &
                             " INNER JOIN state s WITH (NOLOCK) on e.PnState = s.statecode WHERE "

                    If Flag IsNot Nothing AndAlso Flag <> "" Then
                        strSQL &= " e.Flag = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, Flag) & "' "
                    End If

                    If CompanyID IsNot Nothing AndAlso CompanyID <> "" Then
                        strSQL &= "AND e.CompanyID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompanyID) & "' "
                    End If

                    If LocID IsNot Nothing AndAlso LocID <> "" Then
                        strSQL &= "AND  e.LocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' "
                    End If

                    If Designation IsNot Nothing AndAlso Designation <> "" Then
                        strSQL &= "AND  e.Designation != '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Designation) & "' "
                    End If

                    If Stats IsNot Nothing AndAlso Stats <> "" Then
                        strSQL &= "AND  e.Status = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Stats) & "' "
                    End If

                    strSQL &= " Order By LastLogin ASC"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetEmployeeProfileList(Optional ByVal Flag As String = Nothing, Optional ByVal CompanyID As String = Nothing, Optional ByVal CoState As String = Nothing, Optional ByVal Designation As String = Nothing, Optional ByVal Stats As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With EmployeeInfo.MyInfo
                   
                    strSQL = "SELECT e.*, PnState, StateDesc as State, NickName as OfficerName,  CASE WHEN LastLogin IS NULL THEN '' ELSE CONVERT(varchar(10), LastLogin, 103) END as LastLoginDate" &
                            " FROM employee e WITH (NOLOCK) LEFT JOIN usrprofile u WITH (NOLOCK) on e.employeeid = u.refID " &
                            " LEFT JOIN state s WITH (NOLOCK) on e.PnState = s.statecode " &
                            " WHERE e.Flag = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, Flag) & "'" &
                            " AND e.CoState = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CoState) & "'" &
                            " AND e.Designation != '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Designation) & "'" &
                            " AND e.Status != '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, Stats) & "'"
                    If CompanyID IsNot Nothing AndAlso CompanyID <> "0" AndAlso CompanyID <> "" Then
                        strSQL &= " AND e.CompanyID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtNumeric, CompanyID) & "'" & ""
                    End If

                    strSQL &= " Order BY lastlogin ASC"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetTop10Employee(ByVal Location As String, Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With EmployeeInfo.MyInfo

                    If Location <> "" AndAlso Location <> "0" Then
                        strSQL = "select top 10 PnState, StateDesc as State, NickName as OfficerName, isnull(convert(varchar,LastLogin,103),'') as LastLoginDate " &
                            " from employee e inner join usrprofile u on e.employeeid = u.refID " &
                            " inner join state s on e.PnState = s.statecode " &
                            " where e.companyid = '' and e.locid = '' AND Designation != '16' And e.PnState = '" & Location & "'  order by lastlogin desc"
                    Else
                        strSQL = " ;WITH cte AS(" &
                                 " Select ROW_NUMBER() OVER (PARTITION BY PnState ORDER BY LastLogin DESC) AS rn," &
                                 " PnState, StateDesc as State, NickName as OfficerName, isnull(convert(varchar,LastLogin,103),'') as LastLoginDate " &
                                 " from employee e " &
                                 " inner join usrprofile u on e.employeeid = u.refID " &
                                 " inner join state s on e.PnState = s.statecode  " &
                                 " where e.companyid = '' and e.locid = '' AND Designation != '16' and LastLogin is not null)" &
                                " SELECT PnState, State, OfficerName, LastLoginDate FROM cte" &
                                " WHERE rn = 1"
                    End If
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetEmployeeSupportList(Optional ByVal Flg As String = Nothing, Optional ByVal Designation As String = Nothing, Optional ByVal CompanyID As String = Nothing, Optional ByVal CoState As String = Nothing, Optional ByVal CommID As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With EmployeeInfo.MyInfo
                    strSQL = "SELECT DISTINCT EmployeeID, NickName, isnull(StateDesc, isnull(PnState,'')) as StateDesc, EmailAddress, EmerContactNo, CommID " &
                             "FROM EMPLOYEE WITH (NOLOCK) " &
                             "LEFT OUTER JOIN STATE WITH (NOLOCK) ON PnState = StateCode and COUNTRYCode = 'MY' " &
                             "WHERE CompanyID='' and LocID='' "
                    If Designation IsNot Nothing AndAlso Designation <> "" Then
                        strSQL &= "AND Designation != '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Designation) & "' "
                    End If

                    strSQL &= "AND CompanyID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CompanyID) & "' "

                    If CoState IsNot Nothing AndAlso CoState <> "" Then
                        strSQL &= "AND CoState = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CoState) & "' "
                    End If

                    If CommID IsNot Nothing AndAlso CommID <> "" Then
                        strSQL &= "AND CommID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, CommID) & "' "
                    End If

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        'Add
        Public Overloads Function GetEmployeeCustomList(Optional ByVal Condition As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With EmployeeInfo.MyInfo

                    strSQL = "SELECT * FROM EMPLOYEE WITH (NOLOCK) "

                    If Not Condition Is Nothing And Condition <> "" Then strSQL &= " WHERE " & Condition

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetVehicleCustomList(Optional ByVal Condition As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With EmployeeInfo.MyInfo

                    strSQL = "SELECT * FROM VEHICLE WITH (NOLOCK) "

                    If Not Condition Is Nothing And Condition <> "" Then strSQL &= " WHERE " & Condition

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetEmployeeDesignation(ByVal ID As String, Optional FieldCond As String = Nothing) As Container.Employee
            Dim rEmployee As Container.Employee = Nothing
            Dim dtTemp As DataTable = Nothing
            Try
                If StartConnection() = True Then
                    With EmployeeInfo.MyInfo
                        StartSQLControl()
                        strSQL = "SELECT e.NickName, e.EmployeeID, ISNULL(cm.CodeDesc,'') AS CodeDesc FROM EMPLOYEE e WITH (NOLOCK)" &
                            " LEFT JOIN UsrProfile up WITH(NOLOCK) ON up.RefID=e.EmployeeID" &
                            " LEFT JOIN CodeMaster cm ON e.Designation=cm.Code AND cm.CodeType='DSN'" &
                            " WHERE e.Flag=1 AND up.UserID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ID) & "'"

                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rEmployee = New Container.Employee
                                rEmployee.EmployeeID = drRow.Item("EmployeeID")
                                rEmployee.NickName = drRow.Item("NickName")
                                rEmployee.Designation = drRow.Item("CodeDesc")
                            Else
                                rEmployee = Nothing
                            End If
                        Else
                            rEmployee = Nothing
                        End If
                    End With
                End If
                Return rEmployee
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Employee", ex.Message, ex.StackTrace)
            Finally
                rEmployee = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'Add
        Public Overloads Function GetSupportList(Optional ByVal Condition As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With EmployeeInfo.MyInfo

                    strSQL = "SELECT ROW_NUMBER() OVER(ORDER BY e.PnState, e.NickName ASC) AS RecNo, " &
                        " e.EmployeeID, " &
                        " s.StateDesc, " &
                        " UPPER(e.NickName) AS SurName, " &
                        " e.EmailAddress, " &
                        " e.EmerContactNo, " &
                        " e.PnAddress1 + CASE e.PnAddress2 WHEN '' THEN '' ELSE ',' + e.PnAddress2 END + CASE e.PnAddress3 WHEN '' THEN '' ELSE ',' + e.PnAddress3 END + CASE e.PnAddress4 WHEN '' THEN '' ELSE ',' + e.PnAddress4 END AS Address " &
                        " FROM EMPLOYEE e WITH (NOLOCK) " &
                        " INNER JOIN STATE s WITH (NOLOCK) on s.CountryCode = e.PnCountry and s.StateCode = e.PnState " &
                        " WHERE e.Flag = 1 and e.CompanyID = '' and e.LocID = '' and e.CommID = 1 "

                    If Not Condition Is Nothing And Condition <> "" Then strSQL &= " WHERE " & Condition

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetEmployeeNickName(ByVal State As String, Optional ByVal strFilter As String = "") As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With EmployeeInfo.MyInfo

                    strSQL = "SELECT EmployeeID, NickName FROM EMPLOYEE WHERE PNSTATE = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, State) & "' AND COMPANYID='' AND Flag=1 "

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetEmployeeEmail(ByVal EmployeeID As String, Optional ByVal strFilter As String = "") As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With EmployeeInfo.MyInfo

                    strSQL = "SELECT EmployeeID,NickName,EmailAddress FROM EMPLOYEE WHERE EmployeeID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, EmployeeID) & "' AND COMPANYID='' AND Flag=1"

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetDesignation(ByVal DSNCode As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                strSQL = "SELECT CodeDesc FROM CodeMaster WITH (NOLOCK) WHERE CodeType = 'DSN' And Code = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, DSNCode) & "'"
                Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text), Data.DataTable)
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
        Public Class Employee
            Public fEmployeeID As System.String = "EmployeeID"
            Public fAccNo As System.String = "AccNo"
            Public fBranchName As System.String = "BranchName"
            Public fNickName As System.String = "NickName"
            Public fSurName As System.String = "SurName"
            Public fFirstName As System.String = "FirstName"
            Public fSalutation As System.String = "Salutation"
            Public fSex As System.String = "Sex"
            Public fDOB As System.String = "DOB"
            Public fPlaceOfBirth As System.String = "PlaceOfBirth"
            Public fNRICNo As System.String = "NRICNo"
            Public fNationality As System.String = "Nationality"
            Public fRace As System.String = "Race"
            Public fReligion As System.String = "Religion"
            Public fMarital As System.String = "Marital"
            Public fCoAddress1 As System.String = "CoAddress1"
            Public fCoAddress2 As System.String = "CoAddress2"
            Public fCoAddress3 As System.String = "CoAddress3"
            Public fCoAddress4 As System.String = "CoAddress4"
            Public fCoPostalCode As System.String = "CoPostalCode"
            Public fCoState As System.String = "CoState"
            Public fCoCountry As System.String = "CoCountry"
            Public fPnAddress1 As System.String = "PnAddress1"
            Public fPnAddress2 As System.String = "PnAddress2"
            Public fPnAddress3 As System.String = "PnAddress3"
            Public fPnAddress4 As System.String = "PnAddress4"
            Public fPnPostalCode As System.String = "PnPostalCode"
            Public fPnState As System.String = "PnState"
            Public fPnCountry As System.String = "PnCountry"
            Public fEmerContactPerson As System.String = "EmerContactPerson"
            Public fEmerContactNo As System.String = "EmerContactNo"
            Public fEmailAddress As System.String = "EmailAddress"
            Public fDesignation As System.String = "Designation"
            Public fForeignLocal As System.String = "ForeignLocal"
            Public fCommID As System.String = "CommID"
            Public fSalary As System.String = "Salary"
            Public fOffDay As System.String = "OffDay"
            Public fOvertime As System.String = "Overtime"
            Public fLeave As System.String = "Leave"
            Public fLevy As System.String = "Levy"
            Public fAllergies As System.String = "Allergies"
            Public fClerkNo As System.String = "ClerkNo"
            Public fDateHired As System.String = "DateHired"
            Public fDateLeft As System.String = "DateLeft"
            Public fTransportAllowance As System.String = "TransportAllowance"
            Public fServiceAllowance As System.String = "ServiceAllowance"
            Public fOtherAllowance As System.String = "OtherAllowance"
            Public fRemarks As System.String = "Remarks"
            Public fPrivilegeCode As System.String = "PrivilegeCode"
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
            Public fAccessLvl As System.String = "AccessLvl"
            Public fCompanyID As System.String = "CompanyID"
            Public fLocID As System.String = "LocID"
            Public fUserID As System.String = "UserID"


            Protected _EmployeeID As System.String
            Private _AccNo As System.String
            Private _BranchName As System.String
            Private _NickName As System.String
            Private _SurName As System.String
            Private _FirstName As System.String
            Private _Salutation As System.String
            Private _Sex As System.String
            Private _DOB As System.DateTime
            Private _PlaceOfBirth As System.String
            Private _NRICNo As System.String
            Private _Nationality As System.String
            Private _Race As System.String
            Private _Religion As System.String
            Private _Marital As System.String
            Private _CoAddress1 As System.String
            Private _CoAddress2 As System.String
            Private _CoAddress3 As System.String
            Private _CoAddress4 As System.String
            Private _CoPostalCode As System.String
            Private _CoState As System.String
            Private _CoCountry As System.String
            Private _PnAddress1 As System.String
            Private _PnAddress2 As System.String
            Private _PnAddress3 As System.String
            Private _PnAddress4 As System.String
            Private _PnPostalCode As System.String
            Private _PnState As System.String
            Private _PnCountry As System.String
            Private _EmerContactPerson As System.String
            Private _EmerContactNo As System.String
            Private _EmailAddress As System.String
            Private _Designation As System.String
            Private _ForeignLocal As System.String
            Private _CommID As System.String
            Private _Salary As System.Decimal
            Private _OffDay As System.Int32
            Private _Overtime As System.Int32
            Private _Leave As System.Int32
            Private _Levy As System.Decimal
            Private _Allergies As System.String
            Private _ClerkNo As System.String
            Private _DateHired As System.DateTime
            Private _DateLeft As System.DateTime
            Private _TransportAllowance As System.String
            Private _ServiceAllowance As System.String
            Private _OtherAllowance As System.String
            Private _Remarks As System.String
            Private _PrivilegeCode As System.String
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
            Private _AccessLvl As System.Byte
            Private _CompanyID As System.String
            Private _LocID As System.String
            Private _flag As System.Byte
            Private _UserID As System.String
            Private _Remark As System.String
            Private _StatusApprove As System.String
            Private _Device As System.String
            Private _DeviceID As System.String
            Private _Brand As System.String
            Private _FromTransporter As System.Byte
            Private _StatusAdd As System.Int16


            ''' <summary>
            ''' Mandatory
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
            ''' Mandatory
            ''' </summary>
            Public Property EmployeeID As System.String
                Get
                    Return _EmployeeID
                End Get
                Set(ByVal Value As System.String)
                    _EmployeeID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property AccNo As System.String
                Get
                    Return _AccNo
                End Get
                Set(ByVal Value As System.String)
                    _AccNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property BranchName As System.String
                Get
                    Return _BranchName
                End Get
                Set(ByVal Value As System.String)
                    _BranchName = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property NickName As System.String
                Get
                    Return _NickName
                End Get
                Set(ByVal Value As System.String)
                    _NickName = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property SurName As System.String
                Get
                    Return _SurName
                End Get
                Set(ByVal Value As System.String)
                    _SurName = Value
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
            Public Property Salutation As System.String
                Get
                    Return _Salutation
                End Get
                Set(ByVal Value As System.String)
                    _Salutation = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Sex As System.String
                Get
                    Return _Sex
                End Get
                Set(ByVal Value As System.String)
                    _Sex = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property DOB As System.DateTime
                Get
                    Return _DOB
                End Get
                Set(ByVal Value As System.DateTime)
                    _DOB = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PlaceOfBirth As System.String
                Get
                    Return _PlaceOfBirth
                End Get
                Set(ByVal Value As System.String)
                    _PlaceOfBirth = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property NRICNo As System.String
                Get
                    Return _NRICNo
                End Get
                Set(ByVal Value As System.String)
                    _NRICNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Nationality As System.String
                Get
                    Return _Nationality
                End Get
                Set(ByVal Value As System.String)
                    _Nationality = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Race As System.String
                Get
                    Return _Race
                End Get
                Set(ByVal Value As System.String)
                    _Race = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Religion As System.String
                Get
                    Return _Religion
                End Get
                Set(ByVal Value As System.String)
                    _Religion = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Marital As System.String
                Get
                    Return _Marital
                End Get
                Set(ByVal Value As System.String)
                    _Marital = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CoAddress1 As System.String
                Get
                    Return _CoAddress1
                End Get
                Set(ByVal Value As System.String)
                    _CoAddress1 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CoAddress2 As System.String
                Get
                    Return _CoAddress2
                End Get
                Set(ByVal Value As System.String)
                    _CoAddress2 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CoAddress3 As System.String
                Get
                    Return _CoAddress3
                End Get
                Set(ByVal Value As System.String)
                    _CoAddress3 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CoAddress4 As System.String
                Get
                    Return _CoAddress4
                End Get
                Set(ByVal Value As System.String)
                    _CoAddress4 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CoPostalCode As System.String
                Get
                    Return _CoPostalCode
                End Get
                Set(ByVal Value As System.String)
                    _CoPostalCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CoState As System.String
                Get
                    Return _CoState
                End Get
                Set(ByVal Value As System.String)
                    _CoState = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CoCountry As System.String
                Get
                    Return _CoCountry
                End Get
                Set(ByVal Value As System.String)
                    _CoCountry = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PnAddress1 As System.String
                Get
                    Return _PnAddress1
                End Get
                Set(ByVal Value As System.String)
                    _PnAddress1 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PnAddress2 As System.String
                Get
                    Return _PnAddress2
                End Get
                Set(ByVal Value As System.String)
                    _PnAddress2 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PnAddress3 As System.String
                Get
                    Return _PnAddress3
                End Get
                Set(ByVal Value As System.String)
                    _PnAddress3 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PnAddress4 As System.String
                Get
                    Return _PnAddress4
                End Get
                Set(ByVal Value As System.String)
                    _PnAddress4 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PnPostalCode As System.String
                Get
                    Return _PnPostalCode
                End Get
                Set(ByVal Value As System.String)
                    _PnPostalCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PnState As System.String
                Get
                    Return _PnState
                End Get
                Set(ByVal Value As System.String)
                    _PnState = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PnCountry As System.String
                Get
                    Return _PnCountry
                End Get
                Set(ByVal Value As System.String)
                    _PnCountry = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property EmerContactPerson As System.String
                Get
                    Return _EmerContactPerson
                End Get
                Set(ByVal Value As System.String)
                    _EmerContactPerson = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property EmerContactNo As System.String
                Get
                    Return _EmerContactNo
                End Get
                Set(ByVal Value As System.String)
                    _EmerContactNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property EmailAddress As System.String
                Get
                    Return _EmailAddress
                End Get
                Set(ByVal Value As System.String)
                    _EmailAddress = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Designation As System.String
                Get
                    Return _Designation
                End Get
                Set(ByVal Value As System.String)
                    _Designation = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property ForeignLocal As System.String
                Get
                    Return _ForeignLocal
                End Get
                Set(ByVal Value As System.String)
                    _ForeignLocal = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property CommID As System.String
                Get
                    Return _CommID
                End Get
                Set(ByVal Value As System.String)
                    _CommID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Salary As System.Decimal
                Get
                    Return _Salary
                End Get
                Set(ByVal Value As System.Decimal)
                    _Salary = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OffDay As System.Int32
                Get
                    Return _OffDay
                End Get
                Set(ByVal Value As System.Int32)
                    _OffDay = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Overtime As System.Int32
                Get
                    Return _Overtime
                End Get
                Set(ByVal Value As System.Int32)
                    _Overtime = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Leave As System.Int32
                Get
                    Return _Leave
                End Get
                Set(ByVal Value As System.Int32)
                    _Leave = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Levy As System.Decimal
                Get
                    Return _Levy
                End Get
                Set(ByVal Value As System.Decimal)
                    _Levy = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Allergies As System.String
                Get
                    Return _Allergies
                End Get
                Set(ByVal Value As System.String)
                    _Allergies = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ClerkNo As System.String
                Get
                    Return _ClerkNo
                End Get
                Set(ByVal Value As System.String)
                    _ClerkNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property DateHired As System.DateTime
                Get
                    Return _DateHired
                End Get
                Set(ByVal Value As System.DateTime)
                    _DateHired = Value
                End Set
            End Property

            ''' <summary>
            ''' Non-Mandatory, Allow Null
            ''' </summary>
            Public Property DateLeft As System.DateTime
                Get
                    Return _DateLeft
                End Get
                Set(ByVal Value As System.DateTime)
                    _DateLeft = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property TransportAllowance As System.String
                Get
                    Return _TransportAllowance
                End Get
                Set(ByVal Value As System.String)
                    _TransportAllowance = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ServiceAllowance As System.String
                Get
                    Return _ServiceAllowance
                End Get
                Set(ByVal Value As System.String)
                    _ServiceAllowance = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OtherAllowance As System.String
                Get
                    Return _OtherAllowance
                End Get
                Set(ByVal Value As System.String)
                    _OtherAllowance = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Remarks As System.String
                Get
                    Return _Remarks
                End Get
                Set(ByVal Value As System.String)
                    _Remarks = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PrivilegeCode As System.String
                Get
                    Return _PrivilegeCode
                End Get
                Set(ByVal Value As System.String)
                    _PrivilegeCode = Value
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
            Public Property AccessLvl As System.Byte
                Get
                    Return _AccessLvl
                End Get
                Set(ByVal Value As System.Byte)
                    _AccessLvl = Value
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
            Public Property Flag As System.Byte
                Get
                    Return _flag
                End Get
                Set(ByVal Value As System.Byte)
                    _flag = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property StatusApprove As System.String
                Get
                    Return _StatusApprove
                End Get
                Set(ByVal Value As System.String)
                    _StatusApprove = Value
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
            Public Property Device As System.String
                Get
                    Return _Device
                End Get
                Set(ByVal Value As System.String)
                    _Device = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property DeviceID As System.String
                Get
                    Return _DeviceID
                End Get
                Set(ByVal Value As System.String)
                    _DeviceID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property Brand As System.String
                Get
                    Return _Brand
                End Get
                Set(ByVal Value As System.String)
                    _Brand = Value
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
    Public Class EmployeeInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "EmployeeID,ISNULL((SELECT  Distinct(AccNo) FROM BIZLOCATE WITH (NOLOCK) WHERE BizLocID=Employee.LocID),'') AS AccNo, ISNULL((SELECT  Distinct(BranchName) FROM BIZLOCATE WITH (NOLOCK) WHERE BizLocID=Employee.LocID),'') AS BranchName, NickName,SurName,FirstName,Salutation,Sex,DOB,PlaceOfBirth,NRICNo,Nationality,Race,Religion,Marital,CoAddress1,CoAddress2,CoAddress3,CoAddress4,CoPostalCode,CoState,CoCountry,PnAddress1,PnAddress2,PnAddress3,PnAddress4,PnPostalCode,PnState,PnCountry,EmerContactPerson,EmerContactNo,EmailAddress,Designation,ForeignLocal,CommID,Salary,OffDay,Overtime,Leave,Levy,Allergies,ClerkNo,DateHired,DateLeft,TransportAllowance,ServiceAllowance,OtherAllowance,Remarks,PrivilegeCode,Status,CreateDate,CreateBy,LastUpdate,UpdateBy,Flag,Inuse,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy,AccessLvl,CompanyID,LocID,ISNULL((select top 1 userid from USRPROFILE WITH (NOLOCK) where ParentID=employee.companyid and refid=employee.employeeid),'') as UserID"
                .CheckFields = "Status,Flag,Inuse,IsHost,AccessLvl"
                .TableName = "EMPLOYEE WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "EmployeeID,NickName,SurName,FirstName,Salutation,Sex,DOB,PlaceOfBirth,NRICNo,Nationality,Race,Religion,Marital,CoAddress1,CoAddress2,CoAddress3,CoAddress4,CoPostalCode,CoState,CoCountry,PnAddress1,PnAddress2,PnAddress3,PnAddress4,PnPostalCode,PnState,PnCountry,EmerContactPerson,EmerContactNo,EmailAddress,Designation,ForeignLocal,CommID,Salary,OffDay,Overtime,Leave,Levy,Allergies,ClerkNo,DateHired,DateLeft,TransportAllowance,ServiceAllowance,OtherAllowance,Remarks,PrivilegeCode,Status,CreateDate,CreateBy,LastUpdate,UpdateBy,Flag,Inuse,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy,AccessLvl,CompanyID,LocID,(select userid from USRPROFILE WITH (NOLOCK) where ParentID=employee.companyid and refid=employee.employeeid),ISNULL((select top 1 userid from USRPROFILE WITH (NOLOCK) where ParentID=employee.companyid and refid=employee.employeeid),'') as UserID"
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
    Public Class EmployeeScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "EmployeeID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "NickName"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "SurName"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "FirstName"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Salutation"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Sex"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "DOB"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "PlaceOfBirth"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "NRICNo"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Nationality"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Race"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Religion"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Marital"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CoAddress1"
                .Length = 40
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CoAddress2"
                .Length = 40
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CoAddress3"
                .Length = 40
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CoAddress4"
                .Length = 40
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CoPostalCode"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CoState"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CoCountry"
                .Length = 2
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "PnAddress1"
                .Length = 40
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "PnAddress2"
                .Length = 40
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(21, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "PnAddress3"
                .Length = 40
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(22, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "PnAddress4"
                .Length = 40
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(23, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "PnPostalCode"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(24, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "PnState"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(25, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "PnCountry"
                .Length = 2
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(26, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "EmerContactPerson"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(27, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "EmerContactNo"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(28, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "EmailAddress"
                .Length = 80
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(29, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Designation"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(30, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ForeignLocal"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(31, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CommID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(32, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Salary"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(33, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "OffDay"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(34, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Overtime"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(35, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Leave"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(36, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Levy"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(37, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Allergies"
                .Length = 70
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(38, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ClerkNo"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(39, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "DateHired"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(40, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "DateLeft"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(41, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "TransportAllowance"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(42, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ServiceAllowance"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(43, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "OtherAllowance"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(44, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Remarks"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(45, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "PrivilegeCode"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(46, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Status"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(47, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(48, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(49, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(50, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(51, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(52, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Inuse"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(53, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(54, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(55, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(56, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsHost"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(57, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(58, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "AccessLvl"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(59, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CompanyID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(60, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LocID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(61, this)
        End Sub

        Public ReadOnly Property EmployeeID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property NickName As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property SurName As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property FirstName As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property Salutation As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property Sex As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property DOB As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property PlaceOfBirth As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property NRICNo As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property Nationality As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property Race As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property Religion As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property Marital As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property CoAddress1 As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property CoAddress2 As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property CoAddress3 As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property CoAddress4 As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property CoPostalCode As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property CoState As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property CoCountry As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property PnAddress1 As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property
        Public ReadOnly Property PnAddress2 As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property
        Public ReadOnly Property PnAddress3 As StrucElement
            Get
                Return MyBase.GetItem(22)
            End Get
        End Property
        Public ReadOnly Property PnAddress4 As StrucElement
            Get
                Return MyBase.GetItem(23)
            End Get
        End Property
        Public ReadOnly Property PnPostalCode As StrucElement
            Get
                Return MyBase.GetItem(24)
            End Get
        End Property
        Public ReadOnly Property PnState As StrucElement
            Get
                Return MyBase.GetItem(25)
            End Get
        End Property
        Public ReadOnly Property PnCountry As StrucElement
            Get
                Return MyBase.GetItem(26)
            End Get
        End Property
        Public ReadOnly Property EmerContactPerson As StrucElement
            Get
                Return MyBase.GetItem(27)
            End Get
        End Property
        Public ReadOnly Property EmerContactNo As StrucElement
            Get
                Return MyBase.GetItem(28)
            End Get
        End Property
        Public ReadOnly Property EmailAddress As StrucElement
            Get
                Return MyBase.GetItem(29)
            End Get
        End Property
        Public ReadOnly Property Designation As StrucElement
            Get
                Return MyBase.GetItem(30)
            End Get
        End Property
        Public ReadOnly Property ForeignLocal As StrucElement
            Get
                Return MyBase.GetItem(31)
            End Get
        End Property
        Public ReadOnly Property CommID As StrucElement
            Get
                Return MyBase.GetItem(32)
            End Get
        End Property
        Public ReadOnly Property Salary As StrucElement
            Get
                Return MyBase.GetItem(33)
            End Get
        End Property
        Public ReadOnly Property OffDay As StrucElement
            Get
                Return MyBase.GetItem(34)
            End Get
        End Property
        Public ReadOnly Property Overtime As StrucElement
            Get
                Return MyBase.GetItem(35)
            End Get
        End Property
        Public ReadOnly Property Leave As StrucElement
            Get
                Return MyBase.GetItem(36)
            End Get
        End Property
        Public ReadOnly Property Levy As StrucElement
            Get
                Return MyBase.GetItem(37)
            End Get
        End Property
        Public ReadOnly Property Allergies As StrucElement
            Get
                Return MyBase.GetItem(38)
            End Get
        End Property
        Public ReadOnly Property ClerkNo As StrucElement
            Get
                Return MyBase.GetItem(39)
            End Get
        End Property
        Public ReadOnly Property DateHired As StrucElement
            Get
                Return MyBase.GetItem(40)
            End Get
        End Property
        Public ReadOnly Property DateLeft As StrucElement
            Get
                Return MyBase.GetItem(41)
            End Get
        End Property
        Public ReadOnly Property TransportAllowance As StrucElement
            Get
                Return MyBase.GetItem(42)
            End Get
        End Property
        Public ReadOnly Property ServiceAllowance As StrucElement
            Get
                Return MyBase.GetItem(43)
            End Get
        End Property
        Public ReadOnly Property OtherAllowance As StrucElement
            Get
                Return MyBase.GetItem(44)
            End Get
        End Property
        Public ReadOnly Property Remarks As StrucElement
            Get
                Return MyBase.GetItem(45)
            End Get
        End Property
        Public ReadOnly Property PrivilegeCode As StrucElement
            Get
                Return MyBase.GetItem(46)
            End Get
        End Property
        Public ReadOnly Property Status As StrucElement
            Get
                Return MyBase.GetItem(47)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(48)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(49)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(50)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(51)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(52)
            End Get
        End Property
        Public ReadOnly Property Inuse As StrucElement
            Get
                Return MyBase.GetItem(53)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(54)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(55)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(56)
            End Get
        End Property
        Public ReadOnly Property IsHost As StrucElement
            Get
                Return MyBase.GetItem(57)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(58)
            End Get
        End Property
        Public ReadOnly Property AccessLvl As StrucElement
            Get
                Return MyBase.GetItem(59)
            End Get
        End Property
        Public ReadOnly Property CompanyID As StrucElement
            Get
                Return MyBase.GetItem(60)
            End Get
        End Property

        Public ReadOnly Property LocID As StrucElement
            Get
                Return MyBase.GetItem(61)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region

End Namespace