Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Profiles
    Public NotInheritable Class bizlocate
        Inherits Core.CoreControl
        Private BizlocateInfo As BizlocateInfo = New BizlocateInfo
        Private Log As New SystemLog()

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal BizlocateCont As Container.Bizlocate, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If BizlocateCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With BizlocateInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "BizLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizlocateCont.BizLocID) & "' OR (BizRegID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizlocateCont.BizRegID) & "' AND BranchName = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizlocateCont.BranchName) & "')")
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
                            message = "Branch Name already exist"
                            Throw New ApplicationException("210011: Branch Name already exist")
                            Return False
                        Else
                            StartSQLControl()
                            With objSQL
                                .TableName = "BIZLOCATE WITH (ROWLOCK)"
                                .AddField("BizRegID", BizlocateCont.BizRegID, SQLControl.EnumDataType.dtString)
                                .AddField("BranchName", BizlocateCont.BranchName, SQLControl.EnumDataType.dtStringN)
                                .AddField("BranchCode", BizlocateCont.BranchCode, SQLControl.EnumDataType.dtString)
                                .AddField("IndustryType", BizlocateCont.IndustryType, SQLControl.EnumDataType.dtStringN)
                                .AddField("BusinessType", BizlocateCont.BusinessType, SQLControl.EnumDataType.dtStringN)
                                .AddField("AccNo", BizlocateCont.AccNo, SQLControl.EnumDataType.dtString)
                                .AddField("Address1", BizlocateCont.Address1, SQLControl.EnumDataType.dtString)
                                .AddField("Address2", BizlocateCont.Address2, SQLControl.EnumDataType.dtString)
                                .AddField("Address3", BizlocateCont.Address3, SQLControl.EnumDataType.dtString)
                                .AddField("Address4", BizlocateCont.Address4, SQLControl.EnumDataType.dtString)
                                .AddField("PostalCode", BizlocateCont.PostalCode, SQLControl.EnumDataType.dtString)
                                .AddField("ContactPerson", BizlocateCont.ContactPerson, SQLControl.EnumDataType.dtString)
                                .AddField("ContactDesignation", BizlocateCont.ContactDesignation, SQLControl.EnumDataType.dtString)
                                .AddField("ContactEmail", BizlocateCont.ContactEmail, SQLControl.EnumDataType.dtString)
                                .AddField("ContactTelNo", BizlocateCont.ContactTelNo, SQLControl.EnumDataType.dtString)
                                .AddField("ContactMobile", BizlocateCont.ContactMobile, SQLControl.EnumDataType.dtString)
                                .AddField("StoreType", BizlocateCont.StoreType, SQLControl.EnumDataType.dtString)
                                .AddField("Email", BizlocateCont.Email, SQLControl.EnumDataType.dtStringN)
                                .AddField("Tel", BizlocateCont.Tel, SQLControl.EnumDataType.dtString)
                                .AddField("Fax", BizlocateCont.Fax, SQLControl.EnumDataType.dtString)
                                .AddField("Region", BizlocateCont.Region, SQLControl.EnumDataType.dtString)
                                .AddField("Country", BizlocateCont.Country, SQLControl.EnumDataType.dtString)
                                .AddField("State", BizlocateCont.State, SQLControl.EnumDataType.dtString)
                                .AddField("PBT", BizlocateCont.PBT, SQLControl.EnumDataType.dtString)
                                .AddField("City", BizlocateCont.City, SQLControl.EnumDataType.dtString)
                                .AddField("Area", BizlocateCont.Area, SQLControl.EnumDataType.dtString)
                                .AddField("Currency", BizlocateCont.Currency, SQLControl.EnumDataType.dtString)
                                .AddField("StoreStatus", BizlocateCont.StoreStatus, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpPrevBook", BizlocateCont.OpPrevBook, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpTimeStart", BizlocateCont.OpTimeStart, SQLControl.EnumDataType.dtString)
                                .AddField("OpTimeEnd", BizlocateCont.OpTimeEnd, SQLControl.EnumDataType.dtString)
                                .AddField("OpDay1", BizlocateCont.OpDay1, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpDay2", BizlocateCont.OpDay2, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpDay3", BizlocateCont.OpDay3, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpDay4", BizlocateCont.OpDay4, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpDay5", BizlocateCont.OpDay5, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpDay6", BizlocateCont.OpDay6, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpDay7", BizlocateCont.OpDay7, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpBookAlwDY", BizlocateCont.OpBookAlwDY, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpBookAlwHR", BizlocateCont.OpBookAlwHR, SQLControl.EnumDataType.dtNumeric)
                                .AddField("OpBookFirst", BizlocateCont.OpBookFirst, SQLControl.EnumDataType.dtString)
                                .AddField("OpBookLast", BizlocateCont.OpBookLast, SQLControl.EnumDataType.dtString)
                                .AddField("OpBookIntv", BizlocateCont.OpBookIntv, SQLControl.EnumDataType.dtNumeric)
                                .AddField("SalesItemType", BizlocateCont.SalesItemType, SQLControl.EnumDataType.dtStringN)
                                .AddField("InSvcItemType", BizlocateCont.InSvcItemType, SQLControl.EnumDataType.dtStringN)
                                .AddField("GenInSvcID", BizlocateCont.GenInSvcID, SQLControl.EnumDataType.dtNumeric)
                                .AddField("RcpHeader", BizlocateCont.RcpHeader, SQLControl.EnumDataType.dtStringN)
                                .AddField("RcpFooter", BizlocateCont.RcpFooter, SQLControl.EnumDataType.dtStringN)
                                .AddField("PriceLevel", BizlocateCont.PriceLevel, SQLControl.EnumDataType.dtNumeric)
                                .AddField("IsStockTake", BizlocateCont.IsStockTake, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Active", BizlocateCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", BizlocateCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", BizlocateCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("IsHost", BizlocateCont.IsHost, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", BizlocateCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", BizlocateCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Flag", BizlocateCont.Flag, SQLControl.EnumDataType.dtNumeric)
                                .AddField("BankAccount", BizlocateCont.BankAccount, SQLControl.EnumDataType.dtString)
                                .AddField("BranchType", BizlocateCont.BranchType, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateBy", BizlocateCont.CreateBy, SQLControl.EnumDataType.dtString)


                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "BizLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizlocateCont.BizLocID) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("BizLocID", BizlocateCont.BizLocID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "BizLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizlocateCont.BizLocID) & "'")
                                End Select
                            End With
                            Try

                                'ongoing added by diana 20150120, set user to active is location is activated back
                                Dim ListSQL As ArrayList = New ArrayList()
                                ListSQL.Add(strSQL)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stUpdate
                                        If BizlocateCont.Active = 1 Then
                                            strSQL = "UPDATE USRPROFILE WITH (ROWLOCK) Set Status=1 Where Status=0 AND RefID IN (select EmployeeID from EMPLOYEE WITH (NOLOCK) where LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizlocateCont.BizLocID) & "')"
                                            ListSQL.Add(strSQL)

                                            'set employee to flag=1 back
                                            strSQL = "UPDATE EMPLOYEE WITH (ROWLOCK) Set Flag=1 Where Flag=0 AND LocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizlocateCont.BizLocID) & "'"
                                            ListSQL.Add(strSQL)
                                        End If
                                End Select

                                objConn.BatchExecute(ListSQL, CommandType.Text)
                                Return True

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
                                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Location", axExecute.Message & sqlStatement, axExecute.StackTrace)
                                Return True

                            Finally
                                objSQL.Dispose()
                            End Try
                        End If

                    End If
                End If
            Catch axAssign As ApplicationException
                message = axAssign.Message.ToString()
                Log.Notifier.Notify(axAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Location", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Location", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                BizlocateCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal BizlocateCont As Container.Bizlocate, ByRef message As String) As Boolean
            Return Save(BizlocateCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal BizlocateCont As Container.Bizlocate, ByRef message As String) As Boolean
            Return Save(BizlocateCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal BizlocateCont As Container.Bizlocate, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If BizlocateCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With BizlocateInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "BizLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizlocateCont.BizLocID) & "'")
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
                                strSQL = BuildUpdate("BIZLOCATE WITH (ROWLOCK)", " SET Flag = 0" & _
                                " , LastUpdate = '" & BizlocateCont.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizlocateCont.UpdateBy) & "' WHERE" & _
                                "BizLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizlocateCont.BizLocID) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("BIZLOCATE WITH (ROWLOCK)", "BizLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizlocateCont.BizLocID) & "'")
                        End If

                        Try
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Location", exExecute.Message & strSQL, exExecute.StackTrace)
                            Return False
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Location", axDelete.Message, axDelete.StackTrace)
                Return False
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Location", exDelete.Message, exDelete.StackTrace)
                Return False
            Finally
                BizlocateCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'check database
        Public Function IsExistDOE(ByVal AccNo As String) As Boolean
            Dim dtTemp As DataTable
            Try
                StartSQLControl()
                strSQL = "SELECT AccNo FROM BIZLOCATE WITH (NOLOCK) WHERE AccNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, AccNo) & "'"

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
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Location", ex.Message, ex.StackTrace)
            Finally
                EndSQLControl()
            End Try
        End Function

        'check database
        Public Function IsRegisteredDOE(ByVal AccNo As String) As Boolean
            Dim dtTemp As DataTable
            Try
                StartSQLControl()
                strSQL = "SELECT DOEFileNo FROM DOEFILENOREG WITH (NOLOCK) WHERE DOEFileNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, AccNo) & "'"

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
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Location", ex.Message, ex.StackTrace)
            Finally
                EndSQLControl()
            End Try
        End Function

        'check database
        Public Function IsExistEmailLocation(ByVal Email As String) As Boolean
            Dim dtTemp As DataTable
            Try
                StartSQLControl()
                strSQL = "SELECT Email FROM BIZLOCATE WITH (NOLOCK) WHERE Email = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, Email) & "'"

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
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Location", ex.Message, ex.StackTrace)
            Finally
                EndSQLControl()
            End Try
        End Function

        Public Overloads Function GetLocationState(ByVal LocID As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With BizlocateInfo.MyInfo
                    strSQL = BuildSelect(.FieldsList, .TableName, "BizLocID='" & LocID & "'")
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

#End Region

#Region "Data Selection"

        Public Overloads Function GetLocationByBizRegID(ByVal BizRegID As System.String, Optional Filter As String = "") As Container.Bizlocate
            Dim rBizlocate As Container.Bizlocate = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Dim strFilter As String = ""
            If Filter <> "" Then
                strFilter = Filter
            End If

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With BizlocateInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "BizRegID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizRegID) & "'" & strFilter)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rBizlocate = New Container.Bizlocate
                                rBizlocate.BizLocID = drRow.Item("BizLocID")
                                rBizlocate.BizRegID = drRow.Item("BizRegID")
                                rBizlocate.BranchName = drRow.Item("BranchName")
                                rBizlocate.BranchCode = drRow.Item("BranchCode")
                                rBizlocate.IndustryType = drRow.Item("IndustryType")
                                rBizlocate.BusinessType = drRow.Item("BusinessType")
                                rBizlocate.AccNo = drRow.Item("AccNo")
                                rBizlocate.Address1 = drRow.Item("Address1")
                                rBizlocate.Address2 = drRow.Item("Address2")
                                rBizlocate.Address3 = drRow.Item("Address3")
                                rBizlocate.Address4 = drRow.Item("Address4")
                                rBizlocate.PostalCode = drRow.Item("PostalCode")
                                rBizlocate.ContactPerson = drRow.Item("ContactPerson")
                                rBizlocate.ContactDesignation = drRow.Item("ContactDesignation")
                                rBizlocate.ContactEmail = drRow.Item("ContactEmail")
                                rBizlocate.ContactTelNo = drRow.Item("ContactTelNo")
                                rBizlocate.ContactMobile = drRow.Item("ContactMobile")
                                rBizlocate.StoreType = drRow.Item("StoreType")
                                rBizlocate.Email = drRow.Item("Email")
                                rBizlocate.Tel = drRow.Item("Tel")
                                rBizlocate.Fax = drRow.Item("Fax")
                                rBizlocate.Region = drRow.Item("Region")
                                rBizlocate.Country = drRow.Item("Country")
                                rBizlocate.State = drRow.Item("State")
                                rBizlocate.PBT = drRow.Item("PBT")
                                rBizlocate.City = drRow.Item("City")
                                rBizlocate.Area = drRow.Item("Area")
                                rBizlocate.Currency = drRow.Item("Currency")
                                rBizlocate.StoreStatus = drRow.Item("StoreStatus")
                                rBizlocate.OpPrevBook = drRow.Item("OpPrevBook")
                                rBizlocate.OpTimeStart = drRow.Item("OpTimeStart")
                                rBizlocate.OpTimeEnd = drRow.Item("OpTimeEnd")
                                rBizlocate.OpDay1 = drRow.Item("OpDay1")
                                rBizlocate.OpDay2 = drRow.Item("OpDay2")
                                rBizlocate.OpDay3 = drRow.Item("OpDay3")
                                rBizlocate.OpDay4 = drRow.Item("OpDay4")
                                rBizlocate.OpDay5 = drRow.Item("OpDay5")
                                rBizlocate.OpDay6 = drRow.Item("OpDay6")
                                rBizlocate.OpDay7 = drRow.Item("OpDay7")
                                rBizlocate.OpBookAlwDY = drRow.Item("OpBookAlwDY")
                                rBizlocate.OpBookAlwHR = drRow.Item("OpBookAlwHR")
                                rBizlocate.OpBookFirst = drRow.Item("OpBookFirst")
                                rBizlocate.OpBookLast = drRow.Item("OpBookLast")
                                rBizlocate.OpBookIntv = drRow.Item("OpBookIntv")
                                rBizlocate.SalesItemType = drRow.Item("SalesItemType")
                                rBizlocate.InSvcItemType = drRow.Item("InSvcItemType")
                                rBizlocate.GenInSvcID = drRow.Item("GenInSvcID")
                                rBizlocate.RcpHeader = drRow.Item("RcpHeader")
                                rBizlocate.RcpFooter = drRow.Item("RcpFooter")
                                rBizlocate.PriceLevel = drRow.Item("PriceLevel")
                                rBizlocate.IsStockTake = drRow.Item("IsStockTake")
                                rBizlocate.Active = drRow.Item("Active")
                                rBizlocate.Inuse = drRow.Item("Inuse")
                                rBizlocate.IsHost = drRow.Item("IsHost")
                                rBizlocate.UpdateBy = drRow.Item("UpdateBy")
                                rBizlocate.rowguid = drRow.Item("rowguid")
                                rBizlocate.SyncCreate = drRow.Item("SyncCreate")
                                rBizlocate.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rBizlocate.BankAccount = drRow.Item("BankAccount")
                                rBizlocate.BranchType = drRow.Item("BranchType")
                                rBizlocate.CreateBy = drRow.Item("CreateBy")

                                rBizlocate.CountryDesc = IIf(IsDBNull(drRow.Item("CountryDesc")), "", drRow.Item("CountryDesc"))
                                rBizlocate.StateDesc = IIf(IsDBNull(drRow.Item("StateDesc")), "", drRow.Item("StateDesc"))
                                rBizlocate.PBTDesc = IIf(IsDBNull(drRow.Item("PBTDesc")), "", drRow.Item("PBTDesc"))
                                rBizlocate.CityDesc = IIf(IsDBNull(drRow.Item("CityDesc")), "", drRow.Item("CityDesc"))
                                rBizlocate.AreaDesc = IIf(IsDBNull(drRow.Item("AreaDesc")), "", drRow.Item("AreaDesc"))
                                rBizlocate.Designation = IIf(IsDBNull(drRow.Item("Designation")), "", drRow.Item("Designation"))
                                rBizlocate.IndustrytypeDesc = IIf(IsDBNull(drRow.Item("IndustryTypeDesc")), "", drRow.Item("IndustryTypeDesc"))

                            Else
                                rBizlocate = Nothing
                            End If
                        Else
                            rBizlocate = Nothing
                        End If
                    End With
                End If
                Return rBizlocate
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Location", ex.Message, ex.StackTrace)
            Finally
                rBizlocate = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetLocationByPremise(ByVal PremiseID As System.String) As List(Of Container.Bizlocate)
            Dim rLocation As Container.Bizlocate = Nothing
            Dim listLocation As List(Of Container.Bizlocate) = Nothing
            Dim dtTemp As DataTable = Nothing
            If StartConnection() = True Then
                StartSQLControl()
                strSQL = "select L.*, B.CompanyName from BIZLOCATE L with (nolock) inner join BIZENTITY B with (nolock) on B.BizRegID=L.BizRegID " & _
                         " where L.Flag = 1" & _
                         " AND L.BizRegID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, PremiseID) & "'"
                Try
                    dtTemp = objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text)
                    If dtTemp IsNot Nothing AndAlso dtTemp.Rows.Count > 0 Then
                        listLocation = New List(Of Container.Bizlocate)
                        For Each row As DataRow In dtTemp.Rows
                            rLocation = New Container.Bizlocate
                            With rLocation
                                .BizRegID = row.Item("BizRegID").ToString
                                .CompanyName = row.Item("CompanyName").ToString
                                .BizLocID = row.Item("BizLocID").ToString
                                .BranchType = row.Item("BranchType").ToString
                                .BranchName = row.Item("BranchName").ToString
                                .IndustryType = row.Item("IndustryType").ToString
                                .BusinessType = row.Item("BusinessType").ToString
                                .Address1 = row.Item("Address1").ToString
                                .Address2 = row.Item("Address2").ToString
                                .Address3 = row.Item("Address3").ToString
                                .Address4 = row.Item("Address4").ToString
                                .PostalCode = row.Item("PostalCode").ToString
                                .Country = row.Item("Country").ToString
                                .State = row.Item("State").ToString
                                .PBT = row.Item("PBT").ToString
                                .City = row.Item("City").ToString
                                .Area = row.Item("Area").ToString
                                .Tel = row.Item("Tel").ToString
                                .Fax = row.Item("Fax").ToString
                                .Email = row.Item("Email").ToString
                                .ContactPerson = row.Item("ContactPerson").ToString
                                .Designation = row.Item("ContactDesignation").ToString
                                .ContactTelNo = row.Item("ContactTelNo").ToString
                                .ContactEmail = row.Item("ContactEmail").ToString
                                .ContactMobile = row.Item("ContactMobile").ToString
                            End With
                            listLocation.Add(rLocation)
                        Next
                    End If
                Catch ex As Exception
                    Log.Notifier.Notify(ex)
                    Gibraltar.Agent.Log.Error("ISWIS_API/EntityManager", ex.Message & " " & strSQL, ex.StackTrace)
                Finally
                    EndSQLControl()
                End Try
            End If
            EndConnection()
            Return listLocation
        End Function

        Public Overloads Function GetLocationByPremiseeHSRN(ByVal PremiseID As System.String, ByVal DOEFileNo As System.String) As List(Of Container.Bizlocate)
            Dim rLocation As Container.Bizlocate = Nothing
            Dim listLocation As List(Of Container.Bizlocate) = Nothing
            Dim dtTemp As DataTable = Nothing
            If StartConnection() = True Then
                StartSQLControl()
                strSQL = "select L.*, B.CompanyName from BIZLOCATE L with (nolock) inner join BIZENTITY B with (nolock) on B.BizRegID=L.BizRegID " & _
                         " inner join DOEFILENOREG D WITH (NOLOCK) on L.BizRegID = D.RefID and L.AccNo = D.DOEFILENO " & _
                         " where L.Flag = 1" & _
                         " AND D.IDPremis='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, PremiseID) & "'" & _
                         " AND L.AccNo='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, DOEFileNo) & "'"
                Try
                    dtTemp = objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text)
                    If dtTemp IsNot Nothing AndAlso dtTemp.Rows.Count > 0 Then
                        listLocation = New List(Of Container.Bizlocate)
                        For Each row As DataRow In dtTemp.Rows
                            rLocation = New Container.Bizlocate
                            With rLocation
                                .BizRegID = row.Item("BizRegID").ToString
                                .CompanyName = row.Item("CompanyName").ToString
                                .BizLocID = row.Item("BizLocID").ToString
                                .BranchType = row.Item("BranchType").ToString
                                .BranchName = row.Item("BranchName").ToString
                                .IndustryType = row.Item("IndustryType").ToString
                                .BusinessType = row.Item("BusinessType").ToString
                                .Address1 = row.Item("Address1").ToString
                                .Address2 = row.Item("Address2").ToString
                                .Address3 = row.Item("Address3").ToString
                                .Address4 = row.Item("Address4").ToString
                                .PostalCode = row.Item("PostalCode").ToString
                                .Country = row.Item("Country").ToString
                                .State = row.Item("State").ToString
                                .PBT = row.Item("PBT").ToString
                                .City = row.Item("City").ToString
                                .Area = row.Item("Area").ToString
                                .Tel = row.Item("Tel").ToString
                                .Fax = row.Item("Fax").ToString
                                .Email = row.Item("Email").ToString
                                .ContactPerson = row.Item("ContactPerson").ToString
                                .Designation = row.Item("ContactDesignation").ToString
                                .ContactTelNo = row.Item("ContactTelNo").ToString
                                .ContactEmail = row.Item("ContactEmail").ToString
                                .ContactMobile = row.Item("ContactMobile").ToString
                                .Flag = row.Item("Flag").ToString
                                .Active = row.Item("Active").ToString
                            End With
                            listLocation.Add(rLocation)
                        Next
                    End If
                Catch ex As Exception
                    Log.Notifier.Notify(ex)
                    Gibraltar.Agent.Log.Error("ISWIS_API/EntityManager", ex.Message & " " & strSQL, ex.StackTrace)
                Finally
                    EndSQLControl()
                End Try
            End If
            EndConnection()
            Return listLocation
        End Function

        Public Overloads Function GetLocationByDOE(ByVal DOEFileNo As System.String, Optional ByVal Filter As String = "") As Container.Bizlocate
            Dim rBizlocate As Container.Bizlocate = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Dim strFilter As String = ""
            If Filter <> "" Then
                strFilter = Filter
            End If

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With BizlocateInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "AccNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, DOEFileNo) & "'" & strFilter)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rBizlocate = New Container.Bizlocate
                                rBizlocate.BizLocID = drRow.Item("BizLocID")
                                rBizlocate.BizRegID = drRow.Item("BizRegID")
                                rBizlocate.BranchName = drRow.Item("BranchName")
                                rBizlocate.BranchCode = drRow.Item("BranchCode")
                                rBizlocate.IndustryType = drRow.Item("IndustryType")
                                rBizlocate.BusinessType = drRow.Item("BusinessType")
                                rBizlocate.AccNo = drRow.Item("AccNo")
                                rBizlocate.Address1 = drRow.Item("Address1")
                                rBizlocate.Address2 = drRow.Item("Address2")
                                rBizlocate.Address3 = drRow.Item("Address3")
                                rBizlocate.Address4 = drRow.Item("Address4")
                                rBizlocate.PostalCode = drRow.Item("PostalCode")
                                rBizlocate.ContactPerson = drRow.Item("ContactPerson")
                                rBizlocate.ContactDesignation = drRow.Item("ContactDesignation")
                                rBizlocate.ContactEmail = drRow.Item("ContactEmail")
                                rBizlocate.ContactTelNo = drRow.Item("ContactTelNo")
                                rBizlocate.ContactMobile = drRow.Item("ContactMobile")
                                rBizlocate.StoreType = drRow.Item("StoreType")
                                rBizlocate.Email = drRow.Item("Email")
                                rBizlocate.Tel = drRow.Item("Tel")
                                rBizlocate.Fax = drRow.Item("Fax")
                                rBizlocate.Region = drRow.Item("Region")
                                rBizlocate.Country = drRow.Item("Country")
                                rBizlocate.State = drRow.Item("State")
                                rBizlocate.PBT = drRow.Item("PBT")
                                rBizlocate.City = drRow.Item("City")
                                rBizlocate.Area = drRow.Item("Area")
                                rBizlocate.Currency = drRow.Item("Currency")
                                rBizlocate.StoreStatus = drRow.Item("StoreStatus")
                                rBizlocate.OpPrevBook = drRow.Item("OpPrevBook")
                                rBizlocate.OpTimeStart = drRow.Item("OpTimeStart")
                                rBizlocate.OpTimeEnd = drRow.Item("OpTimeEnd")
                                rBizlocate.OpDay1 = drRow.Item("OpDay1")
                                rBizlocate.OpDay2 = drRow.Item("OpDay2")
                                rBizlocate.OpDay3 = drRow.Item("OpDay3")
                                rBizlocate.OpDay4 = drRow.Item("OpDay4")
                                rBizlocate.OpDay5 = drRow.Item("OpDay5")
                                rBizlocate.OpDay6 = drRow.Item("OpDay6")
                                rBizlocate.OpDay7 = drRow.Item("OpDay7")
                                rBizlocate.OpBookAlwDY = drRow.Item("OpBookAlwDY")
                                rBizlocate.OpBookAlwHR = drRow.Item("OpBookAlwHR")
                                rBizlocate.OpBookFirst = drRow.Item("OpBookFirst")
                                rBizlocate.OpBookLast = drRow.Item("OpBookLast")
                                rBizlocate.OpBookIntv = drRow.Item("OpBookIntv")
                                rBizlocate.SalesItemType = drRow.Item("SalesItemType")
                                rBizlocate.InSvcItemType = drRow.Item("InSvcItemType")
                                rBizlocate.GenInSvcID = drRow.Item("GenInSvcID")
                                rBizlocate.RcpHeader = drRow.Item("RcpHeader")
                                rBizlocate.RcpFooter = drRow.Item("RcpFooter")
                                rBizlocate.PriceLevel = drRow.Item("PriceLevel")
                                rBizlocate.IsStockTake = drRow.Item("IsStockTake")
                                rBizlocate.Active = drRow.Item("Active")
                                rBizlocate.Inuse = drRow.Item("Inuse")
                                rBizlocate.IsHost = drRow.Item("IsHost")
                                rBizlocate.UpdateBy = drRow.Item("UpdateBy")
                                rBizlocate.rowguid = drRow.Item("rowguid")
                                rBizlocate.SyncCreate = drRow.Item("SyncCreate")
                                rBizlocate.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rBizlocate.BankAccount = drRow.Item("BankAccount")
                                rBizlocate.BranchType = drRow.Item("BranchType")
                                rBizlocate.CreateBy = drRow.Item("CreateBy")
                                rBizlocate.CountryDesc = IIf(IsDBNull(drRow.Item("CountryDesc")), "", drRow.Item("CountryDesc"))
                                rBizlocate.StateDesc = IIf(IsDBNull(drRow.Item("StateDesc")), "", drRow.Item("StateDesc"))
                                rBizlocate.PBTDesc = IIf(IsDBNull(drRow.Item("PBTDesc")), "", drRow.Item("PBTDesc"))
                                rBizlocate.CityDesc = IIf(IsDBNull(drRow.Item("CityDesc")), "", drRow.Item("CityDesc"))
                                rBizlocate.AreaDesc = IIf(IsDBNull(drRow.Item("AreaDesc")), "", drRow.Item("AreaDesc"))
                                rBizlocate.Designation = IIf(IsDBNull(drRow.Item("Designation")), "", drRow.Item("Designation"))


                            Else
                                rBizlocate = Nothing
                            End If
                        Else
                            rBizlocate = Nothing
                        End If
                    End With
                End If
                Return rBizlocate
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Location", ex.Message, ex.StackTrace)
            Finally
                rBizlocate = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetLocationByIKAS(ByVal DOEFileNo As System.String, Optional ByVal Filter As String = "") As Container.Bizlocate
            Dim rBizlocate As Container.Bizlocate = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Dim strFilter As String = ""
            If Filter <> "" Then
                strFilter = Filter
            End If

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With BizlocateInfo.MyInfo
                        strSQL = "SELECT *, ISNULL((SELECT CountryDesc FROM COUNTRY WHERE CountryCode=d.Country),'') AS CountryDesc " & _
                                ", ISNULL((SELECT StateDesc FROM STATE WHERE CountryCode=d.Country AND StateCode=d.State),'') AS StateDesc " & _
                                ", ISNULL((SELECT PBTDesc FROM PBT WHERE CountryCode=d.Country AND StateCode=d.State AND PBTCode=d.PBT),'') AS PBTDesc " & _
                                ", ISNULL((SELECT CityDesc FROM City WHERE CountryCode=d.Country AND StateCode=d.State AND CityCode=d.City),'') AS CityDesc " & _
                                ", ISNULL((SELECT AreaDesc FROM Area WHERE CountryCode=d.Country AND StateCode=d.State AND CityCode=d.City AND AreaCode=d.Area),'') AS AreaDesc " & _
                                "FROM DOEFILENOREG d WHERE DOEFileNo='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, DOEFileNo) & "'" & strFilter

                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rBizlocate = New Container.Bizlocate
                                rBizlocate.BizLocID = ""
                                rBizlocate.BizRegID = ""
                                rBizlocate.BranchName = drRow.Item("CompanyName")
                                rBizlocate.BranchCode = ""
                                rBizlocate.IndustryType = drRow.Item("IndustryType")
                                rBizlocate.BusinessType = drRow.Item("BusinessType")
                                rBizlocate.AccNo = drRow.Item("DOEFileNo")
                                rBizlocate.Address1 = drRow.Item("Address1")
                                rBizlocate.Address2 = drRow.Item("Address2")
                                rBizlocate.Address3 = drRow.Item("Address3")
                                rBizlocate.Address4 = drRow.Item("Address4")
                                rBizlocate.PostalCode = drRow.Item("PostalCode")
                                rBizlocate.ContactPerson = ""
                                rBizlocate.ContactDesignation = ""
                                rBizlocate.ContactEmail = ""
                                rBizlocate.ContactTelNo = ""
                                rBizlocate.ContactMobile = ""
                                rBizlocate.StoreType = ""
                                rBizlocate.Email = ""
                                rBizlocate.Tel = drRow.Item("TelNo")
                                rBizlocate.Fax = ""
                                rBizlocate.Region = ""
                                rBizlocate.Country = drRow.Item("Country")
                                rBizlocate.State = drRow.Item("State")
                                rBizlocate.PBT = drRow.Item("PBT")
                                rBizlocate.City = drRow.Item("City")
                                rBizlocate.Area = drRow.Item("Area")
                                rBizlocate.Currency = ""
                                rBizlocate.StoreStatus = 0
                                rBizlocate.OpPrevBook = 0
                                rBizlocate.OpTimeStart = ""
                                rBizlocate.OpTimeEnd = ""
                                rBizlocate.OpDay1 = 0
                                rBizlocate.OpDay2 = 0
                                rBizlocate.OpDay3 = 0
                                rBizlocate.OpDay4 = 0
                                rBizlocate.OpDay5 = 0
                                rBizlocate.OpDay6 = 0
                                rBizlocate.OpDay7 = 0
                                rBizlocate.OpBookAlwDY = 0
                                rBizlocate.OpBookAlwHR = 0
                                rBizlocate.OpBookFirst = 0
                                rBizlocate.OpBookLast = 0
                                rBizlocate.OpBookIntv = 0
                                rBizlocate.SalesItemType = 0
                                rBizlocate.InSvcItemType = 0
                                rBizlocate.GenInSvcID = 0
                                rBizlocate.RcpHeader = ""
                                rBizlocate.RcpFooter = ""
                                rBizlocate.PriceLevel = 0
                                rBizlocate.IsStockTake = 0
                                rBizlocate.Active = 1
                                rBizlocate.Inuse = 0
                                rBizlocate.IsHost = "1"
                                rBizlocate.UpdateBy = ""
                                'rBizlocate.rowguid = drRow.Item("rowguid")
                                rBizlocate.SyncCreate = Now()
                                rBizlocate.SyncLastUpd = Now()
                                rBizlocate.BankAccount = ""
                                rBizlocate.BranchType = 0
                                rBizlocate.CreateBy = ""

                                rBizlocate.CountryDesc = IIf(IsDBNull(drRow.Item("CountryDesc")), "", drRow.Item("CountryDesc"))
                                rBizlocate.StateDesc = IIf(IsDBNull(drRow.Item("StateDesc")), "", drRow.Item("StateDesc"))
                                rBizlocate.PBTDesc = IIf(IsDBNull(drRow.Item("PBTDesc")), "", drRow.Item("PBTDesc"))
                                rBizlocate.CityDesc = IIf(IsDBNull(drRow.Item("CityDesc")), "", drRow.Item("CityDesc"))
                                rBizlocate.AreaDesc = IIf(IsDBNull(drRow.Item("AreaDesc")), "", drRow.Item("AreaDesc"))
                                rBizlocate.Designation = ""


                            Else
                                rBizlocate = Nothing
                            End If
                        Else
                            rBizlocate = Nothing
                        End If
                    End With
                End If
                Return rBizlocate
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Location", ex.Message, ex.StackTrace)
            Finally
                rBizlocate = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetLocation(ByVal BizLocID As System.String) As Container.Bizlocate
            Dim rBizlocate As Container.Bizlocate = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With BizlocateInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "BizLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizLocID) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rBizlocate = New Container.Bizlocate
                                rBizlocate.BizLocID = drRow.Item("BizLocID")
                                rBizlocate.BizRegID = drRow.Item("BizRegID")
                                rBizlocate.BranchName = drRow.Item("BranchName")
                                rBizlocate.BranchCode = drRow.Item("BranchCode")
                                rBizlocate.IndustryType = drRow.Item("IndustryType")
                                rBizlocate.BusinessType = drRow.Item("BusinessType")
                                rBizlocate.AccNo = drRow.Item("AccNo")
                                rBizlocate.Address1 = drRow.Item("Address1")
                                rBizlocate.Address2 = drRow.Item("Address2")
                                rBizlocate.Address3 = drRow.Item("Address3")
                                rBizlocate.Address4 = drRow.Item("Address4")
                                rBizlocate.PostalCode = drRow.Item("PostalCode")
                                rBizlocate.ContactPerson = drRow.Item("ContactPerson")
                                rBizlocate.ContactDesignation = drRow.Item("ContactDesignation")
                                rBizlocate.ContactEmail = drRow.Item("ContactEmail")
                                rBizlocate.ContactTelNo = drRow.Item("ContactTelNo")
                                rBizlocate.ContactMobile = drRow.Item("ContactMobile")
                                rBizlocate.StoreType = drRow.Item("StoreType")
                                rBizlocate.Email = drRow.Item("Email")
                                rBizlocate.Tel = drRow.Item("Tel")
                                rBizlocate.Fax = drRow.Item("Fax")
                                rBizlocate.Region = drRow.Item("Region")
                                rBizlocate.Country = drRow.Item("Country")
                                rBizlocate.State = drRow.Item("State")
                                rBizlocate.PBT = drRow.Item("PBT")
                                rBizlocate.City = drRow.Item("City")
                                rBizlocate.Area = drRow.Item("Area")
                                rBizlocate.Currency = drRow.Item("Currency")
                                rBizlocate.StoreStatus = drRow.Item("StoreStatus")
                                rBizlocate.OpPrevBook = drRow.Item("OpPrevBook")
                                rBizlocate.OpTimeStart = drRow.Item("OpTimeStart")
                                rBizlocate.OpTimeEnd = drRow.Item("OpTimeEnd")
                                rBizlocate.OpDay1 = drRow.Item("OpDay1")
                                rBizlocate.OpDay2 = drRow.Item("OpDay2")
                                rBizlocate.OpDay3 = drRow.Item("OpDay3")
                                rBizlocate.OpDay4 = drRow.Item("OpDay4")
                                rBizlocate.OpDay5 = drRow.Item("OpDay5")
                                rBizlocate.OpDay6 = drRow.Item("OpDay6")
                                rBizlocate.OpDay7 = drRow.Item("OpDay7")
                                rBizlocate.OpBookAlwDY = drRow.Item("OpBookAlwDY")
                                rBizlocate.OpBookAlwHR = drRow.Item("OpBookAlwHR")
                                rBizlocate.OpBookFirst = drRow.Item("OpBookFirst")
                                rBizlocate.OpBookLast = drRow.Item("OpBookLast")
                                rBizlocate.OpBookIntv = drRow.Item("OpBookIntv")
                                rBizlocate.SalesItemType = drRow.Item("SalesItemType")
                                rBizlocate.InSvcItemType = drRow.Item("InSvcItemType")
                                rBizlocate.GenInSvcID = drRow.Item("GenInSvcID")
                                rBizlocate.RcpHeader = drRow.Item("RcpHeader")
                                rBizlocate.RcpFooter = drRow.Item("RcpFooter")
                                rBizlocate.PriceLevel = drRow.Item("PriceLevel")
                                rBizlocate.IsStockTake = drRow.Item("IsStockTake")
                                rBizlocate.Active = drRow.Item("Active")
                                rBizlocate.Inuse = drRow.Item("Inuse")
                                rBizlocate.IsHost = drRow.Item("IsHost")
                                rBizlocate.UpdateBy = drRow.Item("UpdateBy")
                                rBizlocate.rowguid = drRow.Item("rowguid")
                                rBizlocate.SyncCreate = drRow.Item("SyncCreate")
                                rBizlocate.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rBizlocate.BankAccount = drRow.Item("BankAccount")
                                rBizlocate.BranchType = drRow.Item("BranchType")
                                rBizlocate.CreateBy = drRow.Item("CreateBy")

                            Else
                                rBizlocate = Nothing
                            End If
                        Else
                            rBizlocate = Nothing
                        End If
                    End With
                End If
                Return rBizlocate
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Location", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rBizlocate = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetIDLocation(ByVal BranchName As System.String, ByVal CompanyID As String) As Container.Bizlocate
            Dim rBizlocate As Container.Bizlocate = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With BizlocateInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "BranchName = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BranchName) & "' AND BizRegID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompanyID) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rBizlocate = New Container.Bizlocate
                                rBizlocate.BizLocID = drRow.Item("BizLocID")

                            Else
                                rBizlocate = Nothing
                            End If
                        Else
                            rBizlocate = Nothing
                        End If
                    End With
                End If
                Return rBizlocate
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Location", ex.Message, ex.StackTrace)
            Finally
                rBizlocate = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetLocation(ByVal BizLocID As System.String, ByVal DecendingOrder As Boolean) As List(Of Container.Bizlocate)
            Dim rBizlocate As Container.Bizlocate = Nothing
            Dim lstBizlocate As List(Of Container.Bizlocate) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With BizlocateInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal BizLocID As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "BizLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizLocID) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rBizlocate = New Container.Bizlocate
                                rBizlocate.BizLocID = drRow.Item("BizLocID")
                                rBizlocate.BizRegID = drRow.Item("BizRegID")
                                rBizlocate.BranchName = drRow.Item("BranchName")
                                rBizlocate.BranchCode = drRow.Item("BranchCode")
                                rBizlocate.IndustryType = drRow.Item("IndustryType")
                                rBizlocate.BusinessType = drRow.Item("BusinessType")
                                rBizlocate.AccNo = drRow.Item("AccNo")
                                rBizlocate.Address1 = drRow.Item("Address1")
                                rBizlocate.Address2 = drRow.Item("Address2")
                                rBizlocate.Address3 = drRow.Item("Address3")
                                rBizlocate.Address4 = drRow.Item("Address4")
                                rBizlocate.PostalCode = drRow.Item("PostalCode")
                                rBizlocate.ContactPerson = drRow.Item("ContactPerson")
                                rBizlocate.ContactDesignation = drRow.Item("ContactDesignation")
                                rBizlocate.ContactEmail = drRow.Item("ContactEmail")
                                rBizlocate.ContactTelNo = drRow.Item("ContactTelNo")
                                rBizlocate.ContactMobile = drRow.Item("ContactMobile")
                                rBizlocate.StoreType = drRow.Item("StoreType")
                                rBizlocate.Email = drRow.Item("Email")
                                rBizlocate.Tel = drRow.Item("Tel")
                                rBizlocate.Fax = drRow.Item("Fax")
                                rBizlocate.Region = drRow.Item("Region")
                                rBizlocate.Country = drRow.Item("Country")
                                rBizlocate.State = drRow.Item("State")
                                rBizlocate.PBT = drRow.Item("PBT")
                                rBizlocate.City = drRow.Item("City")
                                rBizlocate.Area = drRow.Item("Area")
                                rBizlocate.Currency = drRow.Item("Currency")
                                rBizlocate.StoreStatus = drRow.Item("StoreStatus")
                                rBizlocate.OpPrevBook = drRow.Item("OpPrevBook")
                                rBizlocate.OpTimeStart = drRow.Item("OpTimeStart")
                                rBizlocate.OpTimeEnd = drRow.Item("OpTimeEnd")
                                rBizlocate.OpDay1 = drRow.Item("OpDay1")
                                rBizlocate.OpDay2 = drRow.Item("OpDay2")
                                rBizlocate.OpDay3 = drRow.Item("OpDay3")
                                rBizlocate.OpDay4 = drRow.Item("OpDay4")
                                rBizlocate.OpDay5 = drRow.Item("OpDay5")
                                rBizlocate.OpDay6 = drRow.Item("OpDay6")
                                rBizlocate.OpDay7 = drRow.Item("OpDay7")
                                rBizlocate.OpBookAlwDY = drRow.Item("OpBookAlwDY")
                                rBizlocate.OpBookAlwHR = drRow.Item("OpBookAlwHR")
                                rBizlocate.OpBookFirst = drRow.Item("OpBookFirst")
                                rBizlocate.OpBookLast = drRow.Item("OpBookLast")
                                rBizlocate.OpBookIntv = drRow.Item("OpBookIntv")
                                rBizlocate.SalesItemType = drRow.Item("SalesItemType")
                                rBizlocate.InSvcItemType = drRow.Item("InSvcItemType")
                                rBizlocate.GenInSvcID = drRow.Item("GenInSvcID")
                                rBizlocate.RcpHeader = drRow.Item("RcpHeader")
                                rBizlocate.RcpFooter = drRow.Item("RcpFooter")
                                rBizlocate.PriceLevel = drRow.Item("PriceLevel")
                                rBizlocate.IsStockTake = drRow.Item("IsStockTake")
                                rBizlocate.Active = drRow.Item("Active")
                                rBizlocate.Inuse = drRow.Item("Inuse")
                                rBizlocate.IsHost = drRow.Item("IsHost")
                                rBizlocate.UpdateBy = drRow.Item("UpdateBy")
                                rBizlocate.rowguid = drRow.Item("rowguid")
                                rBizlocate.SyncCreate = drRow.Item("SyncCreate")
                                rBizlocate.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rBizlocate.BankAccount = drRow.Item("BankAccount")
                                rBizlocate.BranchType = drRow.Item("BranchType")
                                rBizlocate.CreateBy = drRow.Item("CreateBy")
                            Next
                            lstBizlocate.Add(rBizlocate)
                        Else
                            rBizlocate = Nothing
                        End If
                        Return lstBizlocate
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Location", ex.Message, ex.StackTrace)
            Finally
                rBizlocate = Nothing
                lstBizlocate = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetLocationList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing, Optional ByVal BizregID As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With BizlocateInfo.MyInfo
                    StartSQLControl()
                    If SQL = Nothing Or SQL = String.Empty Then
                        If FieldCond Is Nothing AndAlso BizregID IsNot Nothing AndAlso BizregID <> "" Then
                            FieldCond = " BizRegID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizregID) & "'"
                        End If

                        strSQL = BuildSelect(.FieldsList, .TableName, FieldCond)

                    Else
                        strSQL = SQL
                    End If
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                    EndSQLControl()
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetLocationListEntity(Optional ByVal Flg As String = Nothing, Optional ByVal BranchType As String = Nothing, Optional ByVal BizRegId As String = Nothing, Optional ByVal BizLocId As String = Nothing, Optional ByVal BranchName As String = Nothing) As Data.DataTable
            Dim StrFilter As String = ""
            If StartConnection() = True Then
                StartSQLControl()
                With BizlocateInfo.MyInfo
                    If Flg IsNot Nothing AndAlso Flg <> "" Then
                        StrFilter &= "Flag ='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Flg) & "' "
                    Else
                        StrFilter &= "Flag IN (0,1) "
                    End If

                    If BranchType IsNot Nothing AndAlso BranchType <> "" Then
                        StrFilter &= "AND BranchType='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BranchType) & "' "
                    End If

                    If BizRegId IsNot Nothing AndAlso BizRegId <> "" Then
                        StrFilter &= "AND BizRegID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizRegId) & "' "
                    End If

                    If BizLocId IsNot Nothing AndAlso BizLocId <> "" Then
                        StrFilter &= "AND BizLocID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizLocId) & "' "
                    End If

                    If BranchName IsNot Nothing AndAlso BranchName <> "" Then
                        StrFilter &= "AND BranchName <> '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BranchName) & "' "
                    End If

                    strSQL = BuildSelect(.FieldsList, .TableName, StrFilter)
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function


        Public Overloads Function GetLicense(Optional ByVal ReceiverID As String = Nothing) As Data.DataTable

            If StartConnection() = True Then
                StartSQLControl()
                With BizlocateInfo.MyInfo

                    strSQL = "select l.ContractNo, i.ItemCode, Validityend from LICENSE l " & _
                            "inner join LICENSEITEM i on l.ContractNo=i.ContractNo  " & _
                            "where l.CompanyID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ReceiverID) & "' and Validityend >=GETDATE() "


                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        'Add GetIsStockTake
        Public Overloads Function GetIsStockTake(ByVal LocID As String) As String
            Dim IsStokUse As String = "-1" 'initial

            If StartConnection() = True Then
                StartSQLControl()
                With BizlocateInfo.MyInfo
                    Dim dt As New DataTable

                    strSQL = "SELECT ROW_NUMBER() OVER (ORDER BY L.ITEMCODE) as RecNo," & _
                            " L.ItemCode,L.ItemName, L.QtyOnHand,B.IsHost,IsStockTake " & _
                            " FROM ITEMLOC L " & _
                            " LEFT JOIN STORAGEMASTER S on L.StorageID = S.StorageID AND L.LocID = S.LocID" & _
                            " LEFT JOIN BIZLOCATE B on L.LocID = B.BizLocID" & _
                            " WHERE L.Flag=1 AND L.LocId='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "'" & _
                            " AND L.QtyOnHand = 0 AND B.IsHost = 1 AND IsStockTake = 0" & _
                            " ORDER BY L.ItemCode ASC"
                    dt = objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName)

                    If dt.Rows.Count > 0 Then
                        IsStokUse = dt.Rows(0).Item("IsStockTake")
                    End If
                End With

            End If
            EndSQLControl()
            EndConnection()

            Return IsStokUse

        End Function

        Public Overloads Function GetReceiverListCont(ByVal ReceiverID As String, ByVal ReceiverLocID As String, Optional ByVal isState As Boolean = False) As Container.Bizlocate
            Dim rBizlocate As Container.Bizlocate = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With BizlocateInfo.MyInfo
                        strSQL = "SELECT ROW_NUMBER() OVER (ORDER BY x.CompanyName asc) as RecNo, * " & _
                            " FROM ( " & _
                            " SELECT DISTINCT L.AccNo,B.CompanyName, " & _
                            " ISNULL(SIC.SICDesc,'') as Industry, " & _
                            " (L.Address1 + ' ' + L.Address2 + ' ' + L.Address3 + ' ' + L.Address4) As Address1, " & _
                            " L.ContactPerson,L.ContactDesignation,L.ContactTelNo as ContactPersonTelNo, " & _
                            " L.Fax as ContactPersonFaxNo,L.ContactMobile as ContactPersonMobile,L.ContactEmail as ContactPersonEmail,B.BizRegID as CompanyID,L.BizLocID,L.BranchName " & _
                            " FROM BIZENTITY B WITH (NOLOCK) " & _
                            " INNER JOIN BIZLOCATE L WITH (NOLOCK) on B.BizRegID = L.BizRegID " & _
                            " LEFT JOIN LICENSE LI WITH (NOLOCK) on L.BizLocID = LI.LocID and L.BizRegID=LI.CompanyID  " & _
                            " LEFT JOIN SIC WITH (NOLOCK) ON SIC.SICCode=B.IndustryType " & _
                            " WHERE LI.conttype='R' AND B.StoreStatus=1 AND B.Active=1 AND L.Active=1 AND L.Flag=1 AND L.BizLocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverLocID) & "' AND L.BizRegID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ReceiverID) & "' " & _
                            " AND LI.ValidityEnd >= convert(date,GETDATE()) "
                        If isState Then
                            strSQL &= " AND B.State IN ('12', '13')"
                        Else
                            strSQL &= " AND CompanyType IN ('4','6','7','9')"
                        End If

                        strSQL &= " ) as x ORDER BY CompanyName ASC"

                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rBizlocate = New Container.Bizlocate
                                rBizlocate.AccNo = drRow.Item("AccNo")
                            Else
                                rBizlocate = Nothing
                            End If
                        Else
                            rBizlocate = Nothing
                        End If
                    End With
                End If
                Return rBizlocate
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Location", ex.Message, ex.StackTrace)
            Finally
                rBizlocate = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetTransporterListCont(ByVal TransporterID As String, ByVal TransporterLocID As String, Optional ByVal isState As Boolean = False) As Container.Bizlocate
            Dim rBizlocate As Container.Bizlocate = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With BizlocateInfo.MyInfo
                        strSQL = "SELECT ROW_NUMBER() OVER (ORDER BY x.CompanyName asc) as RecNo, * " & _
                        " FROM ( " & _
                        " SELECT DISTINCT L.AccNo,B.CompanyName, " & _
                        " ISNULL(SIC.SICDesc,'') as Industry, L.Address1, L.ContactPerson, L.ContactDesignation, L.ContactTelNo as ContactPersonTelNo, " & _
                        " L.Fax as ContactPersonFaxNo, L.ContactMobile as ContactPersonMobile, L.ContactEmail as ContactPersonEmail, B.BizRegID as CompanyID, L.BizLocID, L.BranchName " & _
                        " FROM BIZENTITY B WITH (NOLOCK) " & _
                        " INNER JOIN BIZLOCATE L WITH (NOLOCK) on B.BizRegID = L.BizRegID " & _
                        " LEFT JOIN LICENSE LI WITH (NOLOCK) on L.BizLocID = LI.LocID and L.BizRegID=LI.CompanyID  " & _
                        " LEFT JOIN SIC WITH (NOLOCK) ON SIC.SICCode=B.IndustryType " & _
                        " WHERE LI.conttype='T' AND L.Flag=1 AND LI.ValidityEnd >= convert(date,GETDATE()) AND CompanyType IN ('3','5','7','9') AND L.BizLocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, TransporterLocID) & "' AND L.BizRegID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, TransporterID) & "' "

                        If isState Then
                            strSQL &= " AND B.State IN ('12', '13')"
                        End If

                        strSQL &= " ) as x ORDER BY CompanyName ASC"

                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rBizlocate = New Container.Bizlocate
                                rBizlocate.AccNo = drRow.Item("AccNo")
                            Else
                                rBizlocate = Nothing
                            End If
                        Else
                            rBizlocate = Nothing
                        End If
                    End With
                End If
                Return rBizlocate
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Location", ex.Message, ex.StackTrace)
            Finally
                rBizlocate = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetCapacityIndividual(Optional ByVal District As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With BizlocateInfo.MyInfo
                    StartSQLControl()

                    strSQL = "select bl.BizRegID, bl.BizLocID, bl.BranchName, CASE WHEN bl.CapacityLevel = 0 THEN sc.Capacitylevel ELSE bl.CapacityLevel " & _
                        "END AS CapacityLevel, sc.SICDesc from bizlocate bl left join bizentity be on bl.bizregid=be.bizregid " & _
                        "left join sic sc on bl.industrytype=sc.siccode left join CITY c ON bl.City = c.CityCode " & _
                        "WHERE be.CompanyType IN ('4','6','7','9') "

                    If Not District Is Nothing AndAlso District <> "" Then
                        strSQL &= "AND bl.City='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, District) & "'"
                    End If
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetCapacityIndividualbyBizLocID(ByVal BizLocID As String) As Data.DataTable
            If StartConnection() = True Then
                With BizlocateInfo.MyInfo
                    StartSQLControl()

                    strSQL = "select bl.BizRegID, bl.BizLocID, bl.BranchName, CASE WHEN bl.CapacityLevel = 0 THEN sc.Capacitylevel ELSE bl.CapacityLevel " & _
                        "END AS CapacityLevel, sc.SICDesc from bizlocate bl left join bizentity be on bl.bizregid=be.bizregid " & _
                        "left join sic sc on bl.industrytype=sc.siccode left join CITY c ON bl.City = c.CityCode " & _
                        "WHERE bl.BizLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizLocID) & "' "

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetLocationByBranchAndWasteCode(ByVal BranchName As String, ByVal WasteCode As String) As Data.DataTable
            If StartConnection() = True Then
                With BizlocateInfo.MyInfo
                    StartSQLControl()
                    Dim DT As New DataTable
                    strSQL = "SELECT Address1, BL.BizLocID, BL.BizRegID " &
                            " FROM BIZLOCATE BL WITH(NOLOCK) " &
                            " INNER JOIN ITEMLOC I WITH(NOLOCK) ON BL.BizLocID=I.locid " &
                            " WHERE BranchName='" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, BranchName) & "' AND ItemCode='" & WasteCode & "' AND BL.Active = 1" &
                            " GROUP BY Address1, BL.BizLocID, BL.BizRegID"

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


    Namespace Container
#Region "Class Container"
        Public Class Bizlocate
            Public fBizLocID As System.String = "BizLocID"
            Public fBizRegID As System.String = "BizRegID"
            Public fBranchName As System.String = "BranchName"
            Public fBranchCode As System.String = "BranchCode"
            Public fIndustryType As System.String = "IndustryType"
            Public fBusinessType As System.String = "BusinessType"
            Public fAccNo As System.String = "AccNo"
            Public fAddress1 As System.String = "Address1"
            Public fAddress2 As System.String = "Address2"
            Public fAddress3 As System.String = "Address3"
            Public fAddress4 As System.String = "Address4"
            Public fPostalCode As System.String = "PostalCode"
            Public fContactPerson As System.String = "ContactPerson"
            Public fContactDesignation As System.String = "ContactDesignation"
            Public fContactEmail As System.String = "ContactEmail"
            Public fContactTelNo As System.String = "ContactTelNo"
            Public fContactMobile As System.String = "ContactMobile"
            Public fStoreType As System.String = "StoreType"
            Public fEmail As System.String = "Email"
            Public fTel As System.String = "Tel"
            Public fFax As System.String = "Fax"
            Public fRegion As System.String = "Region"
            Public fCountry As System.String = "Country"
            Public fState As System.String = "State"
            Public fPBT As System.String = "PBT"
            Public fCity As System.String = "City"
            Public fArea As System.String = "Area"
            Public fCurrency As System.String = "Currency"
            Public fStoreStatus As System.String = "StoreStatus"
            Public fOpPrevBook As System.String = "OpPrevBook"
            Public fOpTimeStart As System.String = "OpTimeStart"
            Public fOpTimeEnd As System.String = "OpTimeEnd"
            Public fOpDay1 As System.String = "OpDay1"
            Public fOpDay2 As System.String = "OpDay2"
            Public fOpDay3 As System.String = "OpDay3"
            Public fOpDay4 As System.String = "OpDay4"
            Public fOpDay5 As System.String = "OpDay5"
            Public fOpDay6 As System.String = "OpDay6"
            Public fOpDay7 As System.String = "OpDay7"
            Public fOpBookAlwDY As System.String = "OpBookAlwDY"
            Public fOpBookAlwHR As System.String = "OpBookAlwHR"
            Public fOpBookFirst As System.String = "OpBookFirst"
            Public fOpBookLast As System.String = "OpBookLast"
            Public fOpBookIntv As System.String = "OpBookIntv"
            Public fSalesItemType As System.String = "SalesItemType"
            Public fInSvcItemType As System.String = "InSvcItemType"
            Public fGenInSvcID As System.String = "GenInSvcID"
            Public fRcpHeader As System.String = "RcpHeader"
            Public fRcpFooter As System.String = "RcpFooter"
            Public fPriceLevel As System.String = "PriceLevel"
            Public fIsStockTake As System.String = "IsStockTake"
            Public fActive As System.String = "Active"
            Public fInuse As System.String = "Inuse"
            Public fFlag As System.String = "Flag"
            Public fCreateDate As System.String = "CreateDate"
            Public fIsHost As System.String = "IsHost"
            Public fLastUpdate As System.String = "LastUpdate"
            Public fUpdateBy As System.String = "UpdateBy"
            Public frowguid As System.String = "rowguid"
            Public fSyncCreate As System.String = "SyncCreate"
            Public fSyncLastUpd As System.String = "SyncLastUpd"
            Public fBankAccount As System.String = "BankAccount"
            Public fBranchType As System.String = "BranchType"
            Public fCreateBy As System.String = "CreateBy"
            Public fStateDesc As System.String = "StateDesc"
            Public fPBTDesc As System.String = "PBTDesc"
            Public fCityDesc As System.String = "CityDesc"
            Public fAreaDesc As System.String = "AreaDesc"
            Public fCountryDesc As System.String = "CountryDesc"
            Public fDesignation As System.String = "Designation"
            Public fIndustrytypeDesc As System.String = "Industrytype"


            Protected _BizLocID As System.String
            Private _BizRegID As System.String
            Private _BranchName As System.String
            Private _BranchCode As System.String
            Private _IndustryType As System.String
            Private _BusinessType As System.String
            Private _AccNo As System.String
            Private _Address1 As System.String
            Private _Address2 As System.String
            Private _Address3 As System.String
            Private _Address4 As System.String
            Private _PostalCode As System.String
            Private _ContactPerson As System.String
            Private _ContactDesignation As System.String
            Private _ContactEmail As System.String
            Private _ContactTelNo As System.String
            Private _ContactMobile As System.String
            Private _StoreType As System.String
            Private _Email As System.String
            Private _Tel As System.String
            Private _Fax As System.String
            Private _Region As System.String
            Private _Country As System.String
            Private _State As System.String
            Private _PBT As System.String
            Private _City As System.String
            Private _Area As System.String
            Private _Currency As System.String
            Private _StoreStatus As System.Byte
            Private _OpPrevBook As System.Int32
            Private _OpTimeStart As System.String
            Private _OpTimeEnd As System.String
            Private _OpDay1 As System.Byte
            Private _OpDay2 As System.Byte
            Private _OpDay3 As System.Byte
            Private _OpDay4 As System.Byte
            Private _OpDay5 As System.Byte
            Private _OpDay6 As System.Byte
            Private _OpDay7 As System.Byte
            Private _OpBookAlwDY As System.Int32
            Private _OpBookAlwHR As System.Int32
            Private _OpBookFirst As System.String
            Private _OpBookLast As System.String
            Private _OpBookIntv As System.Int32
            Private _SalesItemType As System.String
            Private _InSvcItemType As System.String
            Private _GenInSvcID As System.Byte
            Private _RcpHeader As System.String
            Private _RcpFooter As System.String
            Private _PriceLevel As System.Byte
            'Private _CapacityLevel As System.Decimal
            Private _IsStockTake As System.Byte
            Private _Active As System.Byte
            Private _Inuse As System.Byte
            Private _CreateDate As System.DateTime
            Private _IsHost As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _rowguid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _Flag As System.Byte
            Public _BankAccount As System.String
            Public _BranchType As System.Byte
            Public _CreateBy As System.String
            Public _StateDesc As System.String
            Public _PBTDesc As System.String
            Public _CityDesc As System.String
            Public _AreaDesc As System.String
            Public _CountryDesc As System.String
            Public _Designation As System.String
            Public _IndustrytypeDesc As System.String
            Public _RefID As System.String

            ''' <summary>
            ''' Mandatory
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
            ''' Mandatory
            ''' </summary>
            Public Property IndustrytypeDesc As System.String
                Get
                    Return _IndustrytypeDesc
                End Get
                Set(ByVal Value As System.String)
                    _IndustrytypeDesc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
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
            ''' Mandatory
            ''' </summary>
            Public Property CountryDesc As System.String
                Get
                    Return _CountryDesc
                End Get
                Set(ByVal Value As System.String)
                    _CountryDesc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property StateDesc As System.String
                Get
                    Return _StateDesc
                End Get
                Set(ByVal Value As System.String)
                    _StateDesc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property PBTDesc As System.String
                Get
                    Return _PBTDesc
                End Get
                Set(ByVal Value As System.String)
                    _PBTDesc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property CityDesc As System.String
                Get
                    Return _CityDesc
                End Get
                Set(ByVal Value As System.String)
                    _CityDesc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property AreaDesc As System.String
                Get
                    Return _AreaDesc
                End Get
                Set(ByVal Value As System.String)
                    _AreaDesc = Value
                End Set
            End Property
            ''' <summary>
            ''' Mandatory
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
            ''' Mandatory
            ''' </summary>
            Public Property BranchType As System.String
                Get
                    Return _BranchType
                End Get
                Set(ByVal Value As System.String)
                    _BranchType = Value
                End Set
            End Property


            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property BankAccount As System.String
                Get
                    Return _BankAccount
                End Get
                Set(ByVal Value As System.String)
                    _BankAccount = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property BizLocID As System.String
                Get
                    Return _BizLocID
                End Get
                Set(ByVal Value As System.String)
                    _BizLocID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property BizRegID As System.String
                Get
                    Return _BizRegID
                End Get
                Set(ByVal Value As System.String)
                    _BizRegID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
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
            Public Property BranchCode As System.String
                Get
                    Return _BranchCode
                End Get
                Set(ByVal Value As System.String)
                    _BranchCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property IndustryType As System.String
                Get
                    Return _IndustryType
                End Get
                Set(ByVal Value As System.String)
                    _IndustryType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property BusinessType As System.String
                Get
                    Return _BusinessType
                End Get
                Set(ByVal Value As System.String)
                    _BusinessType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
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
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Address1 As System.String
                Get
                    Return _Address1
                End Get
                Set(ByVal Value As System.String)
                    _Address1 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Address2 As System.String
                Get
                    Return _Address2
                End Get
                Set(ByVal Value As System.String)
                    _Address2 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Address3 As System.String
                Get
                    Return _Address3
                End Get
                Set(ByVal Value As System.String)
                    _Address3 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Address4 As System.String
                Get
                    Return _Address4
                End Get
                Set(ByVal Value As System.String)
                    _Address4 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PostalCode As System.String
                Get
                    Return _PostalCode
                End Get
                Set(ByVal Value As System.String)
                    _PostalCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ContactPerson As System.String
                Get
                    Return _ContactPerson
                End Get
                Set(ByVal Value As System.String)
                    _ContactPerson = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ContactDesignation As System.String
                Get
                    Return _ContactDesignation
                End Get
                Set(ByVal Value As System.String)
                    _ContactDesignation = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ContactEmail As System.String
                Get
                    Return _ContactEmail
                End Get
                Set(ByVal Value As System.String)
                    _ContactEmail = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ContactTelNo As System.String
                Get
                    Return _ContactTelNo
                End Get
                Set(ByVal Value As System.String)
                    _ContactTelNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ContactMobile As System.String
                Get
                    Return _ContactMobile
                End Get
                Set(ByVal Value As System.String)
                    _ContactMobile = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property StoreType As System.String
                Get
                    Return _StoreType
                End Get
                Set(ByVal Value As System.String)
                    _StoreType = Value
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
            Public Property Tel As System.String
                Get
                    Return _Tel
                End Get
                Set(ByVal Value As System.String)
                    _Tel = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Fax As System.String
                Get
                    Return _Fax
                End Get
                Set(ByVal Value As System.String)
                    _Fax = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Region As System.String
                Get
                    Return _Region
                End Get
                Set(ByVal Value As System.String)
                    _Region = Value
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
            Public Property City As System.String
                Get
                    Return _City
                End Get
                Set(ByVal Value As System.String)
                    _City = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Area As System.String
                Get
                    Return _Area
                End Get
                Set(ByVal Value As System.String)
                    _Area = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Currency As System.String
                Get
                    Return _Currency
                End Get
                Set(ByVal Value As System.String)
                    _Currency = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property StoreStatus As System.Byte
                Get
                    Return _StoreStatus
                End Get
                Set(ByVal Value As System.Byte)
                    _StoreStatus = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpPrevBook As System.Int32
                Get
                    Return _OpPrevBook
                End Get
                Set(ByVal Value As System.Int32)
                    _OpPrevBook = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpTimeStart As System.String
                Get
                    Return _OpTimeStart
                End Get
                Set(ByVal Value As System.String)
                    _OpTimeStart = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpTimeEnd As System.String
                Get
                    Return _OpTimeEnd
                End Get
                Set(ByVal Value As System.String)
                    _OpTimeEnd = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpDay1 As System.Byte
                Get
                    Return _OpDay1
                End Get
                Set(ByVal Value As System.Byte)
                    _OpDay1 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpDay2 As System.Byte
                Get
                    Return _OpDay2
                End Get
                Set(ByVal Value As System.Byte)
                    _OpDay2 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpDay3 As System.Byte
                Get
                    Return _OpDay3
                End Get
                Set(ByVal Value As System.Byte)
                    _OpDay3 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpDay4 As System.Byte
                Get
                    Return _OpDay4
                End Get
                Set(ByVal Value As System.Byte)
                    _OpDay4 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpDay5 As System.Byte
                Get
                    Return _OpDay5
                End Get
                Set(ByVal Value As System.Byte)
                    _OpDay5 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpDay6 As System.Byte
                Get
                    Return _OpDay6
                End Get
                Set(ByVal Value As System.Byte)
                    _OpDay6 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpDay7 As System.Byte
                Get
                    Return _OpDay7
                End Get
                Set(ByVal Value As System.Byte)
                    _OpDay7 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpBookAlwDY As System.Int32
                Get
                    Return _OpBookAlwDY
                End Get
                Set(ByVal Value As System.Int32)
                    _OpBookAlwDY = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpBookAlwHR As System.Int32
                Get
                    Return _OpBookAlwHR
                End Get
                Set(ByVal Value As System.Int32)
                    _OpBookAlwHR = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpBookFirst As System.String
                Get
                    Return _OpBookFirst
                End Get
                Set(ByVal Value As System.String)
                    _OpBookFirst = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpBookLast As System.String
                Get
                    Return _OpBookLast
                End Get
                Set(ByVal Value As System.String)
                    _OpBookLast = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property OpBookIntv As System.Int32
                Get
                    Return _OpBookIntv
                End Get
                Set(ByVal Value As System.Int32)
                    _OpBookIntv = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property SalesItemType As System.String
                Get
                    Return _SalesItemType
                End Get
                Set(ByVal Value As System.String)
                    _SalesItemType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property InSvcItemType As System.String
                Get
                    Return _InSvcItemType
                End Get
                Set(ByVal Value As System.String)
                    _InSvcItemType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property GenInSvcID As System.Byte
                Get
                    Return _GenInSvcID
                End Get
                Set(ByVal Value As System.Byte)
                    _GenInSvcID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RcpHeader As System.String
                Get
                    Return _RcpHeader
                End Get
                Set(ByVal Value As System.String)
                    _RcpHeader = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property RcpFooter As System.String
                Get
                    Return _RcpFooter
                End Get
                Set(ByVal Value As System.String)
                    _RcpFooter = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property PriceLevel As System.Byte
                Get
                    Return _PriceLevel
                End Get
                Set(ByVal Value As System.Byte)
                    _PriceLevel = Value
                End Set
            End Property

            ' ''' <summary>
            ' ''' Mandatory, Not Allow Null
            ' ''' </summary>
            'Public Property CapacityLevel As System.Decimal
            '    Get
            '        Return _CapacityLevel
            '    End Get
            '    Set(ByVal Value As System.Decimal)
            '        _CapacityLevel = Value
            '    End Set
            'End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property IsStockTake As System.Byte
                Get
                    Return _IsStockTake
                End Get
                Set(ByVal Value As System.Byte)
                    _IsStockTake = Value
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
            Public Property IsHost As System.String
                Get
                    Return _IsHost
                End Get
                Set(ByVal Value As System.String)
                    _IsHost = Value
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
            Public Property Flag As System.Byte
                Get
                    Return _Flag
                End Get
                Set(ByVal Value As System.Byte)
                    _Flag = Value
                End Set
            End Property

            'Custom Field
            Private _CompanyName As System.String

            Public Property CompanyName As System.String
                Get
                    Return _CompanyName
                End Get
                Set(ByVal Value As System.String)
                    _CompanyName = Value
                End Set
            End Property

        End Class
#End Region
    End Namespace

#Region "Class Info"
    Public Class BizlocateInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "BizLocID,BizRegID,BranchName,BranchCode,IndustryType,BusinessType,AccNo,Address1,Address2,Address3,Address4,PostalCode,ContactPerson,ContactDesignation,ContactEmail,ContactTelNo,ContactMobile,StoreType,Email,Tel,Fax,Region,Country,State,PBT,City,Area,Currency,StoreStatus,OpPrevBook,OpTimeStart,OpTimeEnd,OpDay1,OpDay2,OpDay3,OpDay4,OpDay5,OpDay6,OpDay7,OpBookAlwDY,OpBookAlwHR,OpBookFirst,OpBookLast,OpBookIntv,SalesItemType,InSvcItemType,GenInSvcID,RcpHeader,RcpFooter,PriceLevel,IsStockTake,Active,Inuse,Flag,CreateDate,IsHost,LastUpdate,UpdateBy,rowguid,SyncCreate,SyncLastUpd,BankAccount,BranchType,CreateBy, ISNULL((SELECT TOP 1 SICDesc FROM SIC WITH (NOLOCK) WHERE SICCode=Bizlocate.IndustryType),'') AS IndustryTypeDesc, ISNULL((SELECT TOP 1 SUBSICDesc FROM SUBSIC WITH (NOLOCK) WHERE SICCode=Bizlocate.IndustryType AND SUBSICCode=Bizlocate.BusinessType),'') AS BusinessTypeDesc, (SELECT TOP 1 CountryDesc FROM Country WITH (NOLOCK) WHERE CountryCode=Bizlocate.Country) AS CountryDesc, (SELECT TOP 1 StateDesc FROM State WITH (NOLOCK) WHERE CountryCode=Bizlocate.Country and StateCode=Bizlocate.State) AS StateDesc, ISNULL((SELECT TOP 1 PBTDesc FROM PBT WITH (NOLOCK) WHERE PBTCode=Bizlocate.PBT and StateCode =Bizlocate.State and COUNTRYCode =Bizlocate.Country),'') AS PBTDesc, ISNULL((SELECT TOP 1 CityDesc FROM City WITH (NOLOCK) WHERE CityCode=Bizlocate.City and StateCode =Bizlocate.State and COUNTRYCode =Bizlocate.Country),'') AS CityDesc, (SELECT TOP 1 AreaDesc FROM Area WITH (NOLOCK) WHERE AreaCode=Bizlocate.Area and CityCode=Bizlocate.City and StateCode =Bizlocate.State and COUNTRYCode =Bizlocate.Country) AS AreaDesc,ISNULL((SELECT TOP 1 CodeDesc FROM CodeMaster WITH (NOLOCK) WHERE Code=Bizlocate.ContactDesignation and CodeType='DSN'),'') as Designation, ISNULL((SELECT TOP 1 SICDesc From SIC WITH (NOLOCK) Where SICCode=BIZLOCATE.Industrytype),'') as IndustryTypeDesc"
                .CheckFields = "StoreStatus,OpDay1,OpDay2,OpDay3,OpDay4,OpDay5,OpDay6,OpDay7,GenInSvcID,PriceLevel,IsStockTake,Active,Inuse,Flag"
                .TableName = "BIZLOCATE WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "BizLocID,BizRegID,BranchName,BranchCode,IndustryType,BusinessType,AccNo,Address1,Address2,Address3,Address4,PostalCode,ContactPerson,ContactDesignation,ContactEmail,ContactTelNo,ContactMobile,StoreType,Email,Tel,Fax,Region,Country,State,PBT,City,Area,Currency,StoreStatus,OpPrevBook,OpTimeStart,OpTimeEnd,OpDay1,OpDay2,OpDay3,OpDay4,OpDay5,OpDay6,OpDay7,OpBookAlwDY,OpBookAlwHR,OpBookFirst,OpBookLast,OpBookIntv,SalesItemType,InSvcItemType,GenInSvcID,RcpHeader,RcpFooter,PriceLevel,IsStockTake,Active,Inuse,Flag,CreateDate,IsHost,LastUpdate,UpdateBy,rowguid,SyncCreate,SyncLastUpd,BankAccount,BranchType,CreateBy, ISNULL((SELECT TOP 1 SICDesc FROM SIC WITH (NOLOCK) WHERE SICCode=Bizlocate.IndustryType),'') AS IndustryTypeDesc, ISNULL((SELECT TOP 1 SUBSICDesc FROM SUBSIC WITH (NOLOCK) WHERE SICCode=Bizlocate.IndustryType AND SUBSICCode=Bizlocate.BusinessType),'') AS BusinessTypeDesc, (SELECT TOP 1 CountryDesc FROM Country WITH (NOLOCK) WHERE CountryCode=Bizlocate.Country) AS CountryDesc, (SELECT TOP 1 StateDesc FROM State WITH (NOLOCK) WHERE CountryCode=Bizlocate.Country and StateCode=Bizlocate.State) AS StateDesc, ISNULL((SELECT TOP 1 PBTDesc FROM PBT WITH (NOLOCK) WHERE PBTCode=Bizlocate.PBT and StateCode =Bizlocate.State and COUNTRYCode =Bizlocate.Country),'') AS PBTDesc, ISNULL((SELECT TOP 1 CityDesc FROM City WITH (NOLOCK) WHERE CityCode=Bizlocate.City and StateCode =Bizlocate.State and COUNTRYCode =Bizlocate.Country),'') AS CityDesc, (SELECT TOP 1 AreaDesc FROM Area WITH (NOLOCK) WHERE AreaCode=Bizlocate.Area and CityCode=Bizlocate.City and StateCode =Bizlocate.State and COUNTRYCode =Bizlocate.Country) AS AreaDesc,ISNULL((SELECT TOP 1 CodeDesc FROM CodeMaster WITH (NOLOCK) WHERE Code=Bizlocate.ContactDesignation and CodeType='DSN'),'') as Designation"
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
    Public Class bizlocateScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "BizLocID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "BizRegID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "BranchName"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "BranchCode"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "IndustryType"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "BusinessType"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "AccNo"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Address1"
                .Length = 255
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Address2"
                .Length = 100
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Address3"
                .Length = 40
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Address4"
                .Length = 40
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "PostalCode"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ContactPerson"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ContactDesignation"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ContactEmail"
                .Length = 255
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ContactTelNo"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ContactMobile"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "StoreType"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Email"
                .Length = 255
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Tel"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Fax"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Region"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(21, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Country"
                .Length = 2
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(22, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "State"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(23, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "PBT"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(24, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "City"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(25, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Area"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(26, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Currency"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(27, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "StoreStatus"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(28, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "OpPrevBook"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(29, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "OpTimeStart"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(30, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "OpTimeEnd"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(31, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "OpDay1"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(32, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "OpDay2"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(33, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "OpDay3"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(34, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "OpDay4"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(35, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "OpDay5"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(36, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "OpDay6"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(37, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "OpDay7"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(38, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "OpBookAlwDY"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(39, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "OpBookAlwHR"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(40, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "OpBookFirst"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(41, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "OpBookLast"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(42, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "OpBookIntv"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(43, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "SalesItemType"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(44, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "InSvcItemType"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(45, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "GenInSvcID"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(46, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "RcpHeader"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(47, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "RcpFooter"
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(48, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "PriceLevel"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(49, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsStockTake"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(50, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(51, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Inuse"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(52, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(53, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(54, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "IsHost"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(55, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(56, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(57, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(58, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(59, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(60, this)

        End Sub

        Public ReadOnly Property BizLocID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property BizRegID As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property BranchName As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property BranchCode As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property IndustryType As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property BusinessType As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property AccNo As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property Address1 As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property Address2 As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property Address3 As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property Address4 As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property PostalCode As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property ContactPerson As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property ContactDesignation As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property ContactEmail As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property ContactTelNo As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property ContactMobile As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property StoreType As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property Email As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property Tel As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property Fax As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property
        Public ReadOnly Property Region As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property
        Public ReadOnly Property Country As StrucElement
            Get
                Return MyBase.GetItem(22)
            End Get
        End Property
        Public ReadOnly Property State As StrucElement
            Get
                Return MyBase.GetItem(23)
            End Get
        End Property
        Public ReadOnly Property PBT As StrucElement
            Get
                Return MyBase.GetItem(24)
            End Get
        End Property
        Public ReadOnly Property City As StrucElement
            Get
                Return MyBase.GetItem(25)
            End Get
        End Property
        Public ReadOnly Property Area As StrucElement
            Get
                Return MyBase.GetItem(26)
            End Get
        End Property
        Public ReadOnly Property Currency As StrucElement
            Get
                Return MyBase.GetItem(27)
            End Get
        End Property
        Public ReadOnly Property StoreStatus As StrucElement
            Get
                Return MyBase.GetItem(28)
            End Get
        End Property
        Public ReadOnly Property OpPrevBook As StrucElement
            Get
                Return MyBase.GetItem(29)
            End Get
        End Property
        Public ReadOnly Property OpTimeStart As StrucElement
            Get
                Return MyBase.GetItem(30)
            End Get
        End Property
        Public ReadOnly Property OpTimeEnd As StrucElement
            Get
                Return MyBase.GetItem(31)
            End Get
        End Property
        Public ReadOnly Property OpDay1 As StrucElement
            Get
                Return MyBase.GetItem(32)
            End Get
        End Property
        Public ReadOnly Property OpDay2 As StrucElement
            Get
                Return MyBase.GetItem(33)
            End Get
        End Property
        Public ReadOnly Property OpDay3 As StrucElement
            Get
                Return MyBase.GetItem(34)
            End Get
        End Property
        Public ReadOnly Property OpDay4 As StrucElement
            Get
                Return MyBase.GetItem(35)
            End Get
        End Property
        Public ReadOnly Property OpDay5 As StrucElement
            Get
                Return MyBase.GetItem(36)
            End Get
        End Property
        Public ReadOnly Property OpDay6 As StrucElement
            Get
                Return MyBase.GetItem(37)
            End Get
        End Property
        Public ReadOnly Property OpDay7 As StrucElement
            Get
                Return MyBase.GetItem(38)
            End Get
        End Property
        Public ReadOnly Property OpBookAlwDY As StrucElement
            Get
                Return MyBase.GetItem(39)
            End Get
        End Property
        Public ReadOnly Property OpBookAlwHR As StrucElement
            Get
                Return MyBase.GetItem(40)
            End Get
        End Property
        Public ReadOnly Property OpBookFirst As StrucElement
            Get
                Return MyBase.GetItem(41)
            End Get
        End Property
        Public ReadOnly Property OpBookLast As StrucElement
            Get
                Return MyBase.GetItem(42)
            End Get
        End Property
        Public ReadOnly Property OpBookIntv As StrucElement
            Get
                Return MyBase.GetItem(43)
            End Get
        End Property
        Public ReadOnly Property SalesItemType As StrucElement
            Get
                Return MyBase.GetItem(44)
            End Get
        End Property
        Public ReadOnly Property InSvcItemType As StrucElement
            Get
                Return MyBase.GetItem(45)
            End Get
        End Property
        Public ReadOnly Property GenInSvcID As StrucElement
            Get
                Return MyBase.GetItem(46)
            End Get
        End Property
        Public ReadOnly Property RcpHeader As StrucElement
            Get
                Return MyBase.GetItem(47)
            End Get
        End Property
        Public ReadOnly Property RcpFooter As StrucElement
            Get
                Return MyBase.GetItem(48)
            End Get
        End Property
        Public ReadOnly Property PriceLevel As StrucElement
            Get
                Return MyBase.GetItem(49)
            End Get
        End Property
        Public ReadOnly Property IsStockTake As StrucElement
            Get
                Return MyBase.GetItem(50)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(51)
            End Get
        End Property
        Public ReadOnly Property Inuse As StrucElement
            Get
                Return MyBase.GetItem(52)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(53)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(54)
            End Get
        End Property
        Public ReadOnly Property IsHost As StrucElement
            Get
                Return MyBase.GetItem(55)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(56)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(57)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(58)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(59)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(60)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region

End Namespace