﻿Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Profiles
    Public NotInheritable Class Company
        Inherits Core.CoreControl
        Private CompanyInfo As CompanyInfo = New CompanyInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal CompanyCont As Container.Company, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal LocationCont As Container.Bizlocate = Nothing) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Dim ListSQL As ArrayList = New ArrayList()
            Try
                If CompanyCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With CompanyInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "BizRegID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompanyCont.BizRegID) & "'")
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
                                .TableName = "BIZENTITY WITH (ROWLOCK)"
                                .AddField("CompanyName", CompanyCont.CompanyName, SQLControl.EnumDataType.dtStringN)
                                .AddField("CompanyType", CompanyCont.CompanyType, SQLControl.EnumDataType.dtString)
                                .AddField("Industrytype", CompanyCont.Industrytype, SQLControl.EnumDataType.dtString)
                                .AddField("BusinessType", CompanyCont.BusinessType, SQLControl.EnumDataType.dtString)
                                .AddField("RegNo", CompanyCont.RegNo, SQLControl.EnumDataType.dtStringN)
                                .AddField("AcctNo", CompanyCont.AcctNo, SQLControl.EnumDataType.dtString)
                                .AddField("Address1", CompanyCont.Address1, SQLControl.EnumDataType.dtString)
                                .AddField("Address2", CompanyCont.Address2, SQLControl.EnumDataType.dtString)
                                .AddField("Address3", CompanyCont.Address3, SQLControl.EnumDataType.dtString)
                                .AddField("Address4", CompanyCont.Address4, SQLControl.EnumDataType.dtString)
                                .AddField("PostalCode", CompanyCont.PostalCode, SQLControl.EnumDataType.dtString)
                                .AddField("State", CompanyCont.State, SQLControl.EnumDataType.dtString)
                                .AddField("Country", CompanyCont.Country, SQLControl.EnumDataType.dtString)
                                .AddField("PBT", CompanyCont.PBT, SQLControl.EnumDataType.dtString)
                                .AddField("City", CompanyCont.City, SQLControl.EnumDataType.dtString)
                                .AddField("Area", CompanyCont.Area, SQLControl.EnumDataType.dtString)
                                .AddField("TelNo", CompanyCont.TelNo, SQLControl.EnumDataType.dtString)
                                .AddField("FaxNo", CompanyCont.FaxNo, SQLControl.EnumDataType.dtString)
                                .AddField("Email", CompanyCont.Email, SQLControl.EnumDataType.dtStringN)
                                .AddField("CoWebsite", CompanyCont.CoWebsite, SQLControl.EnumDataType.dtStringN)
                                .AddField("ContactPerson", CompanyCont.ContactPerson, SQLControl.EnumDataType.dtStringN)
                                .AddField("ContactDesignation", CompanyCont.ContactDesignation, SQLControl.EnumDataType.dtStringN)
                                .AddField("ContactPersonEmail", CompanyCont.ContactPersonEmail, SQLControl.EnumDataType.dtStringN)
                                .AddField("ContactPersonTelNo", CompanyCont.ContactPersonTelNo, SQLControl.EnumDataType.dtString)
                                .AddField("ContactPersonFaxNo", CompanyCont.ContactPersonFaxNo, SQLControl.EnumDataType.dtString)
                                .AddField("ContactPersonMobile", CompanyCont.ContactPersonMobile, SQLControl.EnumDataType.dtString)
                                .AddField("Remark", CompanyCont.Remark, SQLControl.EnumDataType.dtStringN)
                                .AddField("Active", CompanyCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", CompanyCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", CompanyCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", CompanyCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", CompanyCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", CompanyCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Flag", CompanyCont.Flag, SQLControl.EnumDataType.dtNumeric)
                                .AddField("BizGrp", CompanyCont.BizGrp, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Status", CompanyCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("KKM", CompanyCont.KKM, SQLControl.EnumDataType.dtNumeric)


                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "BizRegID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompanyCont.BizRegID) & "'")
                                            ListSQL.Add(strSQL)
                                        Else
                                            If blnFound = False Then
                                                .AddField("BizRegID", CompanyCont.BizRegID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                                ListSQL.Add(strSQL)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "BizRegID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompanyCont.BizRegID) & "'")
                                        ListSQL.Add(strSQL)
                                End Select
                            End With
                            If LocationCont Is Nothing Then
                                'Message return
                            Else
                                blnExec = False
                                blnFound = False
                                blnFlag = False
                                If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                                    StartSQLControl()
                                    With CompanyInfo.MyInfo
                                        strSQL = BuildSelect(.CheckFields, "BIZLOCATE WITH (ROWLOCK)", "BizLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocationCont.BizLocID) & "' OR (BizRegID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocationCont.BizRegID) & "' AND BranchName = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocationCont.BranchName) & "')")
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
                                            .AddField("BizRegID", LocationCont.BizRegID, SQLControl.EnumDataType.dtString)
                                            .AddField("BranchName", LocationCont.BranchName, SQLControl.EnumDataType.dtStringN)
                                            .AddField("BranchCode", LocationCont.BranchCode, SQLControl.EnumDataType.dtString)
                                            .AddField("IndustryType", LocationCont.IndustryType, SQLControl.EnumDataType.dtStringN)
                                            .AddField("BusinessType", LocationCont.BusinessType, SQLControl.EnumDataType.dtStringN)
                                            .AddField("AccNo", LocationCont.AccNo, SQLControl.EnumDataType.dtString)
                                            .AddField("Address1", LocationCont.Address1, SQLControl.EnumDataType.dtString)
                                            .AddField("Address2", LocationCont.Address2, SQLControl.EnumDataType.dtString)
                                            .AddField("Address3", LocationCont.Address3, SQLControl.EnumDataType.dtString)
                                            .AddField("Address4", LocationCont.Address4, SQLControl.EnumDataType.dtString)
                                            .AddField("PostalCode", LocationCont.PostalCode, SQLControl.EnumDataType.dtString)
                                            .AddField("ContactPerson", LocationCont.ContactPerson, SQLControl.EnumDataType.dtString)
                                            .AddField("ContactDesignation", LocationCont.ContactDesignation, SQLControl.EnumDataType.dtString)
                                            .AddField("ContactEmail", LocationCont.ContactEmail, SQLControl.EnumDataType.dtString)
                                            .AddField("ContactTelNo", LocationCont.ContactTelNo, SQLControl.EnumDataType.dtString)
                                            .AddField("ContactMobile", LocationCont.ContactMobile, SQLControl.EnumDataType.dtString)
                                            .AddField("StoreType", LocationCont.StoreType, SQLControl.EnumDataType.dtString)
                                            .AddField("Email", LocationCont.Email, SQLControl.EnumDataType.dtStringN)
                                            .AddField("Tel", LocationCont.Tel, SQLControl.EnumDataType.dtString)
                                            .AddField("Fax", LocationCont.Fax, SQLControl.EnumDataType.dtString)
                                            .AddField("Region", LocationCont.Region, SQLControl.EnumDataType.dtString)
                                            .AddField("Country", LocationCont.Country, SQLControl.EnumDataType.dtString)
                                            .AddField("State", LocationCont.State, SQLControl.EnumDataType.dtString)
                                            .AddField("PBT", LocationCont.PBT, SQLControl.EnumDataType.dtString)
                                            .AddField("City", LocationCont.City, SQLControl.EnumDataType.dtString)
                                            .AddField("Area", LocationCont.Area, SQLControl.EnumDataType.dtString)
                                            .AddField("Currency", LocationCont.Currency, SQLControl.EnumDataType.dtString)
                                            .AddField("StoreStatus", LocationCont.StoreStatus, SQLControl.EnumDataType.dtNumeric)
                                            .AddField("OpPrevBook", LocationCont.OpPrevBook, SQLControl.EnumDataType.dtNumeric)
                                            .AddField("OpTimeStart", LocationCont.OpTimeStart, SQLControl.EnumDataType.dtString)
                                            .AddField("OpTimeEnd", LocationCont.OpTimeEnd, SQLControl.EnumDataType.dtString)
                                            .AddField("OpDay1", LocationCont.OpDay1, SQLControl.EnumDataType.dtNumeric)
                                            .AddField("OpDay2", LocationCont.OpDay2, SQLControl.EnumDataType.dtNumeric)
                                            .AddField("OpDay3", LocationCont.OpDay3, SQLControl.EnumDataType.dtNumeric)
                                            .AddField("OpDay4", LocationCont.OpDay4, SQLControl.EnumDataType.dtNumeric)
                                            .AddField("OpDay5", LocationCont.OpDay5, SQLControl.EnumDataType.dtNumeric)
                                            .AddField("OpDay6", LocationCont.OpDay6, SQLControl.EnumDataType.dtNumeric)
                                            .AddField("OpDay7", LocationCont.OpDay7, SQLControl.EnumDataType.dtNumeric)
                                            .AddField("OpBookAlwDY", LocationCont.OpBookAlwDY, SQLControl.EnumDataType.dtNumeric)
                                            .AddField("OpBookAlwHR", LocationCont.OpBookAlwHR, SQLControl.EnumDataType.dtNumeric)
                                            .AddField("OpBookFirst", LocationCont.OpBookFirst, SQLControl.EnumDataType.dtString)
                                            .AddField("OpBookLast", LocationCont.OpBookLast, SQLControl.EnumDataType.dtString)
                                            .AddField("OpBookIntv", LocationCont.OpBookIntv, SQLControl.EnumDataType.dtNumeric)
                                            .AddField("SalesItemType", LocationCont.SalesItemType, SQLControl.EnumDataType.dtStringN)
                                            .AddField("InSvcItemType", LocationCont.InSvcItemType, SQLControl.EnumDataType.dtStringN)
                                            .AddField("GenInSvcID", LocationCont.GenInSvcID, SQLControl.EnumDataType.dtNumeric)
                                            .AddField("RcpHeader", LocationCont.RcpHeader, SQLControl.EnumDataType.dtStringN)
                                            .AddField("RcpFooter", LocationCont.RcpFooter, SQLControl.EnumDataType.dtStringN)
                                            .AddField("PriceLevel", LocationCont.PriceLevel, SQLControl.EnumDataType.dtNumeric)
                                            .AddField("IsStockTake", LocationCont.IsStockTake, SQLControl.EnumDataType.dtNumeric)
                                            .AddField("Active", LocationCont.Active, SQLControl.EnumDataType.dtNumeric)
                                            .AddField("Inuse", LocationCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                            .AddField("LastUpdate", LocationCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                            .AddField("UpdateBy", LocationCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                            .AddField("Flag", LocationCont.Flag, SQLControl.EnumDataType.dtNumeric)
                                            .AddField("BankAccount", LocationCont.BankAccount, SQLControl.EnumDataType.dtString)
                                            .AddField("BranchType", LocationCont.BranchType, SQLControl.EnumDataType.dtNumeric)
                                            .AddField("CreateBy", LocationCont.CreateBy, SQLControl.EnumDataType.dtString)

                                            Select Case pType
                                                Case SQLControl.EnumSQLType.stInsert
                                                    If blnFound = True And blnFlag = False Then
                                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "BizLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocationCont.BizLocID) & "'")
                                                        ListSQL.Add(strSQL)
                                                    Else
                                                        If blnFound = False Then
                                                            .AddField("BizLocID", LocationCont.BizLocID, SQLControl.EnumDataType.dtString)
                                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                                            ListSQL.Add(strSQL)
                                                        End If
                                                    End If
                                                Case SQLControl.EnumSQLType.stUpdate
                                                    strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "BizRegID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocationCont.BizRegID) & "'")
                                                    ListSQL.Add(strSQL)
                                            End Select
                                        End With
                                    End If
                                End If
                            End If

                            Try
                                objConn.BatchExecute(ListSQL, CommandType.Text, True)
                                Return True

                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                Else
                                    message = axExecute.Message.ToString()
                                End If
                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Company", axExecute.Message & strSQL, axExecute.StackTrace)
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
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Company", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Company", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                CompanyCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal CompanyCont As Container.Company, ByRef message As String) As Boolean
            Return Save(CompanyCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal CompanyCont As Container.Company, ByRef message As String, Optional ByVal LocationCont As Container.Bizlocate = Nothing) As Boolean
            Return Save(CompanyCont, SQLControl.EnumSQLType.stUpdate, message, LocationCont)
        End Function

        Public Function Delete(ByVal CompanyCont As Container.Company, ByRef message As String) As Boolean
            Dim ListSQL As ArrayList = New ArrayList()
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If CompanyCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With CompanyInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "BizRegID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompanyCont.BizRegID) & "'")
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

                        If blnFound = True Then
                            With objSQL
                                strSQL = BuildUpdate("BIZENTITY WITH (ROWLOCK)", " SET Flag = 0, Active = 0" & _
                                " , LastUpdate = " & objSQL.ParseValue(SQLControl.EnumDataType.dtDateTime, Now) & " , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompanyCont.UpdateBy) & "' WHERE " & _
                                " BizRegID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompanyCont.BizRegID) & "'")

                                ListSQL.Add(strSQL)
                            End With

                            'Set user profiles to inactive
                            strSQL = "UPDATE USRPROFILE WITH (ROWLOCK) SET STATUS=0 WHERE PARENTID='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompanyCont.BizRegID) & "'"

                            ListSQL.Add(strSQL)
                        End If

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
                            Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Company", exExecute.Message & sqlStatement, exExecute.StackTrace)
                            Return False
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Company", axDelete.Message, axDelete.StackTrace)
                Return False
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Company", exDelete.Message, exDelete.StackTrace)
                Return False
            Finally
                CompanyCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'frengky check database
        Public Function IsExistRocno(ByVal RegNo As String) As Boolean
            Dim dtTemp As DataTable
            Try
                StartSQLControl()
                strSQL = "SELECT RegNo FROM BIZENTITY WITH (NOLOCK) WHERE RegNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, RegNo) & "'"

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
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Company", ex.Message, ex.StackTrace)
            Finally
                EndSQLControl()
            End Try
        End Function

        Public Function IsEmptyRocNo(ByVal BizRegID As String) As Boolean
            Dim isEmpty = False
            Dim dtTemp As DataTable

            Try
                StartSQLControl()
                strSQL = "SELECT isnull(RegNo,'') as RegNo FROM BIZENTITY WITH (NOLOCK) WHERE BizRegID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizRegID) & "'"

                dtTemp = objConn.Execute(strSQL, CommandType.Text)

                If dtTemp Is Nothing = False Then
                    If dtTemp.Rows.Count > 0 Then
                        If dtTemp.Rows(0).Item("RegNo") = "" Then
                            isEmpty = True

                        End If

                    End If

                End If

            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Company", ex.Message, ex.StackTrace)
                'Throw New Exception(ex.Message)
            Finally
                EndSQLControl()
            End Try

            Return isEmpty
        End Function

        'Add for make SerialNo in CN mandatory if return True
        Public Function IsReqSupp(ByVal CompanyID As String) As Boolean
            Dim dtTemp As DataTable
            Try
                StartSQLControl()
                strSQL = "SELECT BizRegID FROM BIZENTITY WITH (NOLOCK) WHERE BizRegID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompanyID) & "' AND ReqSupp=1"

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
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Company", ex.Message, ex.StackTrace)
            Finally
                EndSQLControl()
            End Try
        End Function

        'Add for make SerialNo in CN mandatory if return True
        Public Function IsExistEmailCompany(ByVal Email As String) As Boolean
            Dim dtTemp As DataTable
            Try
                StartSQLControl()
                strSQL = "SELECT Email FROM BIZENTITY WITH (NOLOCK) WHERE Email = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, Email) & "'"

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
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Company", ex.Message, ex.StackTrace)
            Finally
                EndSQLControl()
            End Try
        End Function

        'Add 
        Public Function IsExistEmailContactPerson(ByVal Email As String) As Boolean
            Dim dtTemp As DataTable
            Try
                StartSQLControl()
                strSQL = "SELECT ContactPersonEmail FROM BIZENTITY WITH (NOLOCK) WHERE ContactPersonEmail = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtStringN, Email) & "'"
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
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Company", ex.Message, ex.StackTrace)
            Finally
                EndSQLControl()
            End Try
        End Function

#End Region

#Region "Data Selection"

        Public Overloads Function GetGenerator(ByVal preName As String) As List(Of Profiles.container_company)
            Dim dt As New Data.DataTable
            Dim ListGenerator As List(Of container_company) = New List(Of container_company)

            Try
                If StartConnection() = True Then
                    With CompanyInfo.MyInfo
                        StartSQLControl()

                        strSQL = "select distinct isnull(branchname, '-') as [Company Name], isnull(bizlocid, '-') as [Location Detail], isnull(bl.address1, '-') as [Address], isnull(s.statedesc, '-') as [State], isnull(c.countrydesc, '-') as [Country], isnull(e.nickname, '-') as [PIC], isnull(be.telno, '-') as [Tel No], isnull(be.faxno, '-') as [Fax No], isnull(be.contactpersontelno, '-') as [Mobile No], isnull(be.ContactPersonEmail, '-') as [Email] " & _
                                 "from bizlocate bl " & _
                                 "left join bizentity be on bl.bizregid = be.bizregid " & _
                                 "left join state s on be.state = s.statecode " & _
                                 "left join country c on be.country = c.countrycode " & _
                                 "left join employee e on bl.bizlocid = e.locid " & _
                                 "where branchname = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, preName) & "'"


                        dt = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)

                        If dt Is Nothing = False Then
                            For Each drRow As DataRow In dt.Rows
                                Dim container As container_company = New Profiles.container_company
                                container.Name = drRow.Item("Company Name").ToString
                                container.LocID = drRow.Item("Location Detail").ToString
                                container.Address = drRow.Item("Address").ToString
                                container.State = drRow.Item("State").ToString
                                container.Country = drRow.Item("Country").ToString
                                container.PIC = drRow.Item("PIC").ToString
                                container.TelNo = drRow.Item("Tel No").ToString
                                container.FaxNo = drRow.Item("Fax No").ToString
                                container.MobileNo = drRow.Item("Mobile No").ToString
                                container.Email = drRow.Item("Email").ToString

                                ListGenerator.Add(container)
                            Next

                            Return ListGenerator
                        Else
                            Return Nothing
                        End If

                    End With
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Company", ex.Message & " " & strSQL, ex.StackTrace)
            Finally
                EndSQLControl()
                EndConnection()
            End Try
            Return Nothing
        End Function

        Public Overloads Function GetLicense(Optional ByVal keyword As String = "") As Data.DataTable
            Try
                If StartConnection() = True Then
                    With CompanyInfo.MyInfo
                        StartSQLControl()
                        Dim strFilter As String = ""

                        strSQL = "SELECT DISTINCT (e.COMPANYNAME + ' : ' + l.Address1 + ' ' + l.Address2 + ' ' + l.Address3 + ' ' + l.Address4 + ' ' + l.PostalCode + ' ' + ISNULL(a.AreaDesc, '') + ' ' + s.StateDesc) AS CONTRACTOR, c.CONTRACTNO AS [LICENSE NO], cm.CODEDESC + ' : ' + lc.LICENSEDESC AS [TYPE OF LICENSE],(SELECT Case ContType When 'R' then (select dbo.GetWasteCode(c.ContractNo)) When 'T' then (SELECT dbo.GetRegNoVehicle(c.CompanyID)) End ) AS [WASTE GROUP], c.ValidityStart, c.ValidityEnd, c.Flag " & _
                                 "FROM BIZENTITY e WITH (NOLOCK) " & _
                                 "LEFT JOIN BIZLOCATE l WITH (NOLOCK) on e.BizRegID=l.BizRegID " & _
                                 "LEFT JOIN LICENSE c WITH (NOLOCK) ON e.BIZREGID = c.COMPANYID AND c.LOCID = l.BIZLOCID " & _
                                 "LEFT JOIN LICENSECATEGORY lc WITH (NOLOCK) ON lc.LICENSECODE = c.CONTCATEGORY " & _
                                 "LEFT JOIN STATE s WITH (NOLOCK) ON s.STATECODE = e.STATE AND s.COUNTRYCODE = e.COUNTRY " & _
                                 "LEFT JOIN AREA a WITH (NOLOCK) ON a.AreaCode = e.Area AND a.StateCode = e.State AND a.CountryCode = e.COUNTRY " & _
                                 "LEFT JOIN CODEMASTER cm WITH (NOLOCK) ON cm.CODETYPE = 'LTY' AND cm.CODE = c.CONTTYPE " & _
                                 "LEFT JOIN VEHICLE v WITH (NOLOCK) on c.CompanyID = v.CompanyID LEFT JOIN LICENSEITEM I WITH (NOLOCK) on c.ContractNo = I.ContractNo " & _
                                 "WHERE c.ValidityStart<='2015-09-28 23:59:59' AND c.ValidityEnd>='2015-09-28 00:00:00' AND c.Flag=1 and c.ContType is not null"

                        If (Not keyword Is Nothing And keyword <> "") Then
                            strFilter &= " AND l.BranchName like '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, keyword) & "'"
                            strFilter &= " OR c.contractno like '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, keyword) & "'"


                            strSQL = "SELECT DISTINCT (e.COMPANYNAME + ' : ' + l.Address1 + ' ' + l.Address2 + ' ' + l.Address3 + ' ' + l.Address4 + ' ' + l.PostalCode + ' ' + ISNULL(a.AreaDesc, '') + ' ' + s.StateDesc) AS CONTRACTOR, c.CONTRACTNO AS [LICENSE NO], cm.CODEDESC + ' : ' + lc.LICENSEDESC AS [TYPE OF LICENSE],(SELECT Case ContType When 'R' then (select dbo.GetWasteCode(c.ContractNo)) When 'T' then (SELECT dbo.GetRegNoVehicle(c.CompanyID)) End ) AS [WASTE GROUP], c.ValidityStart, c.ValidityEnd, c.Flag " & _
                                 "FROM BIZENTITY e WITH (NOLOCK) " & _
                                 "LEFT JOIN BIZLOCATE l WITH (NOLOCK) on e.BizRegID=l.BizRegID " & _
                                 "LEFT JOIN LICENSE c WITH (NOLOCK) ON e.BIZREGID = c.COMPANYID AND c.LOCID = l.BIZLOCID " & _
                                 "LEFT JOIN LICENSECATEGORY lc WITH (NOLOCK) ON lc.LICENSECODE = c.CONTCATEGORY " & _
                                 "LEFT JOIN STATE s WITH (NOLOCK) ON s.STATECODE = e.STATE AND s.COUNTRYCODE = e.COUNTRY " & _
                                 "LEFT JOIN AREA a WITH (NOLOCK) ON a.AreaCode = e.Area AND a.StateCode = e.State AND a.CountryCode = e.COUNTRY " & _
                                 "LEFT JOIN CODEMASTER cm WITH (NOLOCK) ON cm.CODETYPE = 'LTY' AND cm.CODE = c.CONTTYPE " & _
                                 "LEFT JOIN VEHICLE v WITH (NOLOCK) on c.CompanyID = v.CompanyID LEFT JOIN LICENSEITEM I WITH (NOLOCK) on c.ContractNo = I.ContractNo " & _
                                 "WHERE c.ValidityStart<='2015-09-28 23:59:59' AND c.ValidityEnd>='2015-09-28 00:00:00' AND c.Flag=1 and c.ContType is not null" & strFilter
                        End If
                        Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                    End With
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Company", ex.Message & " " & strSQL, ex.StackTrace)
                'Throw ex
            Finally
                EndSQLControl()
                EndConnection()
            End Try
            Return Nothing
        End Function

        Public Overloads Function GetNotification(ByVal preid As String, ByVal prename As String, ByVal refno As String, ByVal wastecode As String) As List(Of container_company)
            Dim dt As New Data.DataTable
            Dim ListNotification As List(Of container_company) = New List(Of container_company)

            Try
                If StartConnection() = True Then
                    With CompanyInfo.MyInfo
                        StartSQLControl()
                        Dim strFilter As String = ""

                        strSQL = "SELECT  d.companyid as [ID], be.companyname as [Name], h.locid as [LocID], h.transno as [Reference No], h.transdate as [Date], D.itemcode as [Waste Code], " & _
                                 "D.itmname as [Waste Name], D.itmcomponent as [Waste Component], isnull (c.codedesc, '') as [Waste Type],  D.itmsource as [Source of Waste], D.qty as [Quantity(MT/per month)], isnull (uom.uomdesc, '') as [Package Type] " & _
                                 "FROM NOTIFYDTL D " & _
                                 "LEFT JOIN UOM ON UOM.UOMCode = D.UOM " & _
                                 "LEFT JOIN notifyhdr h WITH (NOLOCK) on d.locid = h.locid " & _
                                 "LEFT JOIN codemaster c with (nolock) on c.codetype = 'WTY' and c.code = D.typecode " & _
                                 "LEFT JOIN bizentity be with (nolock) on be.bizregid = d.companyid " & _
                                 "LEFT JOIN bizlocate bl with (nolock) on bl.bizlocid = d.locid " & _
                                 "where rectype = '2' "

                        If (Not preid Is Nothing And preid <> "") Or (Not prename Is Nothing And prename <> "") Or (Not refno Is Nothing And refno <> "") Then
                            If preid <> "" Then
                                strFilter &= " AND d.companyid like '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, preid) & "' "
                            End If

                            If prename <> "" Then
                                strFilter &= " AND be.companyname like '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, prename) & "' "
                            End If

                            If refno <> "" Then
                                strFilter &= " AND h.transno like '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, refno) & "' "
                            End If

                            If wastecode <> "" Then
                                strFilter &= " AND d.itemcode like '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, wastecode) & "' "
                            End If

                            strSQL = "SELECT  d.companyid as [ID], be.companyname as [Name], h.locid as [LocID], h.transno as [Reference No], h.transdate as [Date], D.itemcode as [Waste Code], " & _
                                "D.itmname as [Waste Name], D.itmcomponent as [Waste Component], isnull (c.codedesc, '') as [Waste Type],  D.itmsource as [Source of Waste], D.qty as [Quantity(MT/per month)], isnull (uom.uomdesc, '') as [Package Type] " & _
                                "FROM NOTIFYDTL D " & _
                                "LEFT JOIN UOM ON UOM.UOMCode = D.UOM " & _
                                "LEFT JOIN notifyhdr h WITH (NOLOCK) on d.locid = h.locid " & _
                                "LEFT JOIN codemaster c with (nolock) on c.codetype = 'WTY' and c.code = D.typecode " & _
                                "LEFT JOIN bizentity be with (nolock) on be.bizregid = d.companyid " & _
                                "LEFT JOIN bizlocate bl with (nolock) on bl.bizlocid = d.locid " & _
                                "where rectype = '2' " & strFilter
                        End If

                        dt = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)

                        If dt Is Nothing = False Then
                            For Each drRow As DataRow In dt.Rows
                                Dim container As container_company = New eSWIS.Logic.Profiles.container_company
                                container.ID = drRow.Item("ID").ToString
                                container.Name = drRow.Item("Name").ToString
                                container.LocID = drRow.Item("LocID").ToString
                                container.ReferenceNo = drRow.Item("Reference No").ToString
                                container.Dates = drRow.Item("Date").ToString
                                container.WasteCode = drRow.Item("Waste Code").ToString
                                container.WasteName = drRow.Item("Waste Name").ToString
                                container.WasteComponent = drRow.Item("Waste Component").ToString
                                container.WasteType = drRow.Item("Waste Type").ToString
                                container.SourceOfWaste = drRow.Item("Source of Waste").ToString
                                container.Qty = drRow.Item("Quantity(MT/per month)").ToString
                                container.PackageType = drRow.Item("Package Type").ToString

                                ListNotification.Add(container)
                            Next

                            Return ListNotification
                        Else
                            Return Nothing
                        End If

                    End With
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Company", ex.Message & " " & strSQL, ex.StackTrace)
                'Throw ex
            Finally
                EndSQLControl()
                EndConnection()
            End Try
            Return Nothing
        End Function

        Public Overloads Function GetFacility(ByVal bizregid As String, ByVal companyName As String, ByVal stateCode As String, ByVal cityCode As String, ByVal areaCode As String, ByVal licenseNo As String, ByVal typeoflicense As String, ByVal licensecategory As String, ByVal vehicleno As String, ByVal wastecode As String, ByVal contcategory As String, ByVal conttype As String) As List(Of container_company)
            Dim dt As New Data.DataTable
            Dim ListFacility As List(Of container_company) = New List(Of container_company)

            Try
                If StartConnection() = True Then
                    With CompanyInfo.MyInfo
                        StartSQLControl()

                        strSQL = "SELECT * FROM ( SELECT TABLE1.* FROM ( SELECT DISTINCT bizlocid AS PREMISE_ID, (e.COMPANYNAME) AS PREMISE_NAME, (l.Address1 + ' ' + l.Address2 + ' ' + " & _
                                 "l.Address3 + ' ' + l.Address4 + ' ' + ISNULL(a.AreaDesc, '')) as [ADDRESS], " & _
                                 "c.CONTRACTNO AS [LICENSE NO], isnull(cm.CODEDESC + ' : ', '') + isnull(lc.LICENSEDESC, '') AS [TYPE OF LICENSE], " & _
                                 "isnull((SELECT Case ContType When 'R' then (select dbo.GetWasteCode(c.ContractNo)) When 'T' then " & _
                                 "(SELECT dbo.GetRegNoVehicle(c.CompanyID)) End ), '') AS [WASTE GROUP], isnull(TelNo, '') as TelNo, isnull(FaxNo, '') as FaxNo, isnull(c.validityend, '') as [Expired Date], isnull(l.postalcode, '') as [Postal Code], isnull(s.statedesc, '') as [State] " & _
                                 "FROM BIZENTITY e WITH (NOLOCK) " & _
                                 "LEFT JOIN BIZLOCATE l WITH (NOLOCK) on e.BizRegID=l.BizRegID " & _
                                 "LEFT JOIN LICENSE c WITH (NOLOCK) ON e.BIZREGID = c.COMPANYID AND c.LOCID = l.BIZLOCID " & _
                                 "LEFT JOIN LICENSECATEGORY lc WITH (NOLOCK) ON lc.LICENSECODE = c.CONTCATEGORY " & _
                                 "LEFT JOIN STATE s WITH (NOLOCK) ON s.STATECODE = e.STATE AND s.COUNTRYCODE = e.COUNTRY " & _
                                 "LEFT JOIN AREA a WITH (NOLOCK) ON a.AreaCode = e.Area AND a.StateCode = e.State AND a.CountryCode = e.COUNTRY " & _
                                 "LEFT JOIN CODEMASTER cm WITH (NOLOCK) ON cm.CODETYPE = 'LTY' AND cm.CODE = c.CONTTYPE " & _
                                 "LEFT JOIN VEHICLE v WITH (NOLOCK) on c.CompanyID = v.CompanyID LEFT JOIN LICENSEITEM I WITH (NOLOCK) on c.ContractNo = I.ContractNo " & _
                                 "WHERE c.ValidityStart<='" & String.Format("{0:yyyy-MM-dd}", Now()) & " 23:59:59' AND c.ValidityEnd>='" & String.Format("{0:yyyy-MM-dd}", Now()) & " 00:00:00' AND c.Flag=1 and c.ContType is not null "
                        strSQL &= ") AS TABLE1 " & _
                                ") AS RESULT ORDER BY PREMISE_NAME ASC"

                        If (Not bizregid Is Nothing AndAlso bizregid <> "") Or (Not companyName Is Nothing AndAlso companyName <> "") Or _
                           (Not stateCode Is Nothing AndAlso stateCode <> "") Or (Not cityCode Is Nothing AndAlso cityCode <> "") Or _
                           (Not areaCode Is Nothing AndAlso areaCode <> "") Or (Not licenseNo Is Nothing AndAlso licenseNo <> "") Or _
                           (Not typeoflicense Is Nothing AndAlso typeoflicense <> "") Or (Not licensecategory Is Nothing AndAlso licensecategory <> "") Or _
                           (Not vehicleno Is Nothing AndAlso vehicleno <> "") Or (Not wastecode Is Nothing AndAlso wastecode <> "") Or _
                           (Not contcategory Is Nothing AndAlso contcategory <> "") Or (Not conttype Is Nothing AndAlso conttype <> "") Then
                            Dim strFilter As String = ""

                            If bizregid <> "" Then
                                strFilter &= " AND bizlocid = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, bizregid) & "'"
                            End If

                            If companyName <> "" Then
                                strFilter &= " AND e.companyname like '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, companyName) & "'"
                            End If

                            If stateCode <> "" Then
                                strFilter &= " AND e.state = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, stateCode) & "'"
                            End If

                            If cityCode <> "" Then
                                strFilter &= " AND e.city = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, cityCode) & "'"
                            End If

                            If areaCode <> "" Then
                                strFilter &= " AND e.area = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, areaCode) & "'"
                            End If

                            If licenseNo <> "" Then
                                strFilter &= " AND  c.ContractNo LIKE '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, licenseNo) & "'"
                            End If

                            If typeoflicense <> "" Then
                                strFilter &= " AND c.ContType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, typeoflicense) & "'"
                            End If

                            If licensecategory <> "" Then
                                strFilter &= " AND c.ContCategory = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, licensecategory) & "'"
                            End If

                            If vehicleno <> "" Then
                                strFilter &= " AND v.RegNo LIKE '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, vehicleno) & "'"
                            End If

                            If wastecode <> "" Then
                                strFilter &= " AND i.ItemCode in('" & wastecode & "')"
                            End If

                            If contcategory <> "" Then
                                strFilter &= " AND c.ContCategory in('" & contcategory & "')"
                            End If

                            If conttype <> "" Then
                                strFilter &= " AND c.ContType = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, conttype) & "'"
                            End If

                            strSQL = "SELECT * FROM ( SELECT TABLE1.* FROM ( SELECT DISTINCT bizlocid AS PREMISE_ID, (e.COMPANYNAME) AS PREMISE_NAME, (l.Address1 + ' ' + l.Address2 + ' ' + " & _
                                 "l.Address3 + ' ' + l.Address4 + ' ' + ' ' + ISNULL(a.AreaDesc, '')) as [ADDRESS], " & _
                                 "c.CONTRACTNO AS [LICENSE NO], isnull(cm.CODEDESC + ' : ', '') + isnull(lc.LICENSEDESC, '') AS [TYPE OF LICENSE], " & _
                                 "isnull((SELECT Case ContType When 'R' then (select dbo.GetWasteCode(c.ContractNo)) When 'T' then " & _
                                 "(SELECT dbo.GetRegNoVehicle(c.CompanyID)) End ), '') AS [WASTE GROUP], isnull(TelNo, '') as TelNo, isnull(FaxNo, '') as FaxNo, isnull(c.validityend, '') as [Expired Date], isnull(l.postalcode, '') as [Postal Code], isnull(s.statedesc, '') as [State] " & _
                                 "FROM BIZENTITY e WITH (NOLOCK) " & _
                                 "LEFT JOIN BIZLOCATE l WITH (NOLOCK) on e.BizRegID=l.BizRegID " & _
                                 "LEFT JOIN LICENSE c WITH (NOLOCK) ON e.BIZREGID = c.COMPANYID AND c.LOCID = l.BIZLOCID " & _
                                 "LEFT JOIN LICENSECATEGORY lc WITH (NOLOCK) ON lc.LICENSECODE = c.CONTCATEGORY " & _
                                 "LEFT JOIN STATE s WITH (NOLOCK) ON s.STATECODE = e.STATE AND s.COUNTRYCODE = e.COUNTRY " & _
                                 "LEFT JOIN AREA a WITH (NOLOCK) ON a.AreaCode = e.Area AND a.StateCode = e.State AND a.CountryCode = e.COUNTRY " & _
                                 "LEFT JOIN CODEMASTER cm WITH (NOLOCK) ON cm.CODETYPE = 'LTY' AND cm.CODE = c.CONTTYPE " & _
                                 "LEFT JOIN VEHICLE v WITH (NOLOCK) on c.CompanyID = v.CompanyID LEFT JOIN LICENSEITEM I WITH (NOLOCK) on c.ContractNo = I.ContractNo " & _
                                 "WHERE c.ValidityStart<='" & String.Format("{0:yyyy-MM-dd}", Now()) & " 23:59:59' AND c.ValidityEnd>='" & String.Format("{0:yyyy-MM-dd}", Now()) & " 00:00:00' AND c.Flag=1 and c.ContType is not null " & strFilter
                            strSQL &= ") AS TABLE1 " & _
                                    ") AS RESULT ORDER BY PREMISE_NAME ASC"
                        End If

                        dt = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)

                        If dt Is Nothing = False Then
                            For Each drRow As DataRow In dt.Rows
                                Dim container As container_company = New Profiles.container_company
                                container.ID = drRow.Item("PREMISE_ID").ToString
                                container.Name = drRow.Item("PREMISE_NAME").ToString
                                container.Address = drRow.Item("ADDRESS").ToString
                                container.LicenseNo = drRow.Item("LICENSE NO").ToString
                                container.TypeOfLicense = drRow.Item("TYPE OF LICENSE").ToString
                                container.WasteGroup = drRow.Item("WASTE GROUP").ToString
                                container.TelNo = drRow.Item("TelNo").ToString
                                container.FaxNo = drRow.Item("FaxNo").ToString
                                container.ExpiredDate = String.Format("{0:dd/MM/yyyy}", drRow.Item("Expired Date"))
                                container.PostalCode = drRow.Item("Postal Code").ToString
                                container.State = drRow.Item("State").ToString
                                ListFacility.Add(container)
                            Next

                            Return ListFacility
                        Else
                            Return Nothing
                        End If

                    End With
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eCNLogic/Profiles/Company", ex.Message & " " & strSQL, ex.StackTrace)
            Finally
                EndSQLControl()
                EndConnection()
            End Try
            Return Nothing
        End Function

        Public Overloads Function GetCompanyByROC(ByVal ROCNo As System.String) As Container.Company
            Dim rCompany As Container.Company = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With CompanyInfo.MyInfo
                        StartSQLControl()
                        strSQL = BuildSelect(.FieldsList, .TableName, "RegNo = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ROCNo) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rCompany = New Container.Company
                                rCompany.BizRegID = drRow.Item("BizRegID")
                                rCompany.CompanyName = drRow.Item("CompanyName")
                                rCompany.CompanyType = drRow.Item("CompanyType")
                                rCompany.Industrytype = drRow.Item("Industrytype")
                                rCompany.BusinessType = drRow.Item("BusinessType")
                                rCompany.RegNo = drRow.Item("RegNo")
                                rCompany.AcctNo = drRow.Item("AcctNo")
                                rCompany.Address1 = drRow.Item("Address1")
                                rCompany.Address2 = drRow.Item("Address2")
                                rCompany.Address3 = drRow.Item("Address3")
                                rCompany.Address4 = drRow.Item("Address4")
                                rCompany.PostalCode = drRow.Item("PostalCode")
                                rCompany.State = drRow.Item("State")
                                rCompany.Country = drRow.Item("Country")
                                rCompany.TelNo = drRow.Item("TelNo")
                                rCompany.FaxNo = drRow.Item("FaxNo")
                                rCompany.Email = drRow.Item("Email")
                                rCompany.CoWebsite = drRow.Item("CoWebsite")
                                rCompany.ContactPerson = drRow.Item("ContactPerson")
                                rCompany.ContactDesignation = drRow.Item("ContactDesignation")
                                rCompany.ContactPersonEmail = drRow.Item("ContactPersonEmail")
                                rCompany.ContactPersonTelNo = drRow.Item("ContactPersonTelNo")
                                rCompany.ContactPersonFaxNo = drRow.Item("ContactPersonFaxNo")
                                rCompany.ContactPersonMobile = drRow.Item("ContactPersonMobile")
                                rCompany.Remark = drRow.Item("Remark")
                                rCompany.Active = drRow.Item("Active")
                                rCompany.Inuse = drRow.Item("Inuse")
                                rCompany.CreateBy = drRow.Item("CreateBy")
                                rCompany.UpdateBy = drRow.Item("UpdateBy")
                                rCompany.rowguid = drRow.Item("rowguid")
                                rCompany.BizGrp = drRow.Item("BizGrp")
                                rCompany.Status = drRow.Item("Status")

                                rCompany.CreateDate = If(IsDBNull(drRow.Item("CreateDate")), Now, drRow.Item("CreateDate"))
                                rCompany.PBTDesc = If(IsDBNull(drRow.Item("PBTDesc")), "", drRow.Item("PBTDesc"))
                                rCompany.CityDesc = If(IsDBNull(drRow.Item("CityDesc")), "", drRow.Item("CityDesc"))
                                rCompany.StateDesc = If(IsDBNull(drRow.Item("StateDesc")), "", drRow.Item("StateDesc"))
                                rCompany.AreaDesc = If(IsDBNull(drRow.Item("AreaDesc")), "", drRow.Item("AreaDesc"))
                                rCompany.CountryDesc = If(IsDBNull(drRow.Item("CountryDesc")), "", drRow.Item("CountryDesc"))
                                rCompany.Designation = If(IsDBNull(drRow.Item("Designation")), "", drRow.Item("designation"))
                                rCompany.IndustrytypeDesc = If(IsDBNull(drRow.Item("IndustryTypeDesc")), "", drRow.Item("IndustryTypeDesc"))
                                rCompany.BusinessTypeDesc = If(IsDBNull(drRow.Item("BusinessTypeDesc")), "", drRow.Item("BusinessTypeDesc"))


                                'Ivan,17 July 2014,Add if function to avoid error DBNull
                                rCompany.PBT = If(IsDBNull(drRow.Item("PBT")), "", drRow.Item("PBT"))
                                rCompany.City = If(IsDBNull(drRow.Item("City")), "", drRow.Item("City"))
                                rCompany.Area = If(IsDBNull(drRow.Item("Area")), "", drRow.Item("Area"))

                            Else
                                rCompany = Nothing
                            End If
                        Else
                            rCompany = Nothing
                        End If
                    End With
                End If
                Return rCompany
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Company", ex.Message, ex.StackTrace)
            Finally
                rCompany = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetCompany(ByVal BizRegID As System.String) As Container.Company
            Dim rCompany As Container.Company = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With CompanyInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "BizRegID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizRegID) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rCompany = New Container.Company
                                rCompany.BizRegID = drRow.Item("BizRegID")
                                rCompany.CompanyName = drRow.Item("CompanyName")
                                rCompany.CompanyType = drRow.Item("CompanyType")
                                rCompany.Industrytype = drRow.Item("Industrytype")
                                rCompany.BusinessType = drRow.Item("BusinessType")
                                rCompany.RegNo = drRow.Item("RegNo")
                                rCompany.AcctNo = drRow.Item("AcctNo")
                                rCompany.Address1 = drRow.Item("Address1")
                                rCompany.Address2 = drRow.Item("Address2")
                                rCompany.Address3 = drRow.Item("Address3")
                                rCompany.Address4 = drRow.Item("Address4")
                                rCompany.PostalCode = drRow.Item("PostalCode")
                                rCompany.State = drRow.Item("State")
                                rCompany.Country = drRow.Item("Country")
                                rCompany.TelNo = drRow.Item("TelNo")
                                rCompany.FaxNo = drRow.Item("FaxNo")
                                rCompany.Email = drRow.Item("Email")
                                rCompany.CoWebsite = drRow.Item("CoWebsite")
                                rCompany.ContactPerson = drRow.Item("ContactPerson")
                                rCompany.ContactDesignation = drRow.Item("ContactDesignation")
                                rCompany.ContactPersonEmail = drRow.Item("ContactPersonEmail")
                                rCompany.ContactPersonTelNo = drRow.Item("ContactPersonTelNo")
                                rCompany.ContactPersonFaxNo = drRow.Item("ContactPersonFaxNo")
                                rCompany.ContactPersonMobile = drRow.Item("ContactPersonMobile")
                                rCompany.Remark = drRow.Item("Remark")
                                rCompany.Active = drRow.Item("Active")
                                rCompany.Inuse = drRow.Item("Inuse")
                                rCompany.CreateBy = drRow.Item("CreateBy")
                                rCompany.UpdateBy = drRow.Item("UpdateBy")
                                rCompany.rowguid = drRow.Item("rowguid")
                                rCompany.BizGrp = drRow.Item("BizGrp")
                                rCompany.Status = drRow.Item("Status")
                                rCompany.CreateDate = If((drRow.Item("CreateDate")) Is DBNull.Value, DateTime.Now, drRow.Item("CreateDate"))
                                rCompany.PBTDesc = If((drRow.Item("PBTDesc")) Is DBNull.Value, "", drRow.Item("PBTDesc"))
                                rCompany.CityDesc = If((drRow.Item("CityDesc")) Is DBNull.Value, "", drRow.Item("CityDesc"))
                                rCompany.StateDesc = If((drRow.Item("StateDesc")) Is DBNull.Value, "", drRow.Item("StateDesc"))
                                rCompany.AreaDesc = If((drRow.Item("AreaDesc")) Is DBNull.Value, "", drRow.Item("AreaDesc"))
                                rCompany.CountryDesc = If((drRow.Item("CountryDesc")) Is DBNull.Value, "", drRow.Item("CountryDesc"))
                                rCompany.Designation = If((drRow.Item("Designation")) Is DBNull.Value, "", drRow.Item("designation"))
                                rCompany.IndustrytypeDesc = If((drRow.Item("IndustryTypeDesc")) Is DBNull.Value, "", drRow.Item("IndustryTypeDesc"))


                                'Ivan,17 July 2014,Add if function to avoid error DBNull
                                rCompany.PBT = If((drRow.Item("PBT")) Is DBNull.Value, "", drRow.Item("PBT"))
                                rCompany.City = If((drRow.Item("City")) Is DBNull.Value, "", drRow.Item("City"))
                                rCompany.Area = If((drRow.Item("Area")) Is DBNull.Value, "", drRow.Item("Area"))
                                rCompany.KKM = If((drRow.Item("KKM")) Is DBNull.Value, "", drRow.Item("KKM"))

                            Else
                                rCompany = Nothing
                            End If
                        Else
                            rCompany = Nothing
                        End If
                    End With
                End If
                Return rCompany
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Company", ex.Message, ex.StackTrace)
                Return Nothing
            Finally
                rCompany = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetCompany(ByVal BizRegID As System.String, ByVal DecendingOrder As Boolean) As List(Of Container.Company)
            Dim rCompany As Container.Company = Nothing
            Dim lstCompany As List(Of Container.Company) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With CompanyInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal BizRegID As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "BizRegID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizRegID) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rCompany = New Container.Company
                                rCompany.BizRegID = drRow.Item("BizRegID")
                                rCompany.CompanyName = drRow.Item("CompanyName")
                                rCompany.CompanyType = drRow.Item("CompanyType")
                                rCompany.Industrytype = drRow.Item("Industrytype")
                                rCompany.BusinessType = drRow.Item("BusinessType")
                                rCompany.RegNo = drRow.Item("RegNo")
                                rCompany.AcctNo = drRow.Item("AcctNo")
                                rCompany.Address1 = drRow.Item("Address1")
                                rCompany.Address2 = drRow.Item("Address2")
                                rCompany.Address3 = drRow.Item("Address3")
                                rCompany.Address4 = drRow.Item("Address4")
                                rCompany.PostalCode = drRow.Item("PostalCode")
                                rCompany.State = drRow.Item("State")
                                rCompany.Country = drRow.Item("Country")
                                rCompany.TelNo = drRow.Item("TelNo")
                                rCompany.FaxNo = drRow.Item("FaxNo")
                                rCompany.Email = drRow.Item("Email")
                                rCompany.CoWebsite = drRow.Item("CoWebsite")
                                rCompany.ContactPerson = drRow.Item("ContactPerson")
                                rCompany.ContactDesignation = drRow.Item("ContactDesignation")
                                rCompany.ContactPersonEmail = drRow.Item("ContactPersonEmail")
                                rCompany.ContactPersonTelNo = drRow.Item("ContactPersonTelNo")
                                rCompany.ContactPersonFaxNo = drRow.Item("ContactPersonFaxNo")
                                rCompany.ContactPersonMobile = drRow.Item("ContactPersonMobile")
                                rCompany.Remark = drRow.Item("Remark")
                                rCompany.Active = drRow.Item("Active")
                                rCompany.Inuse = drRow.Item("Inuse")
                                rCompany.CreateBy = drRow.Item("CreateBy")
                                rCompany.UpdateBy = drRow.Item("UpdateBy")
                                rCompany.rowguid = drRow.Item("rowguid")
                                rCompany.PBT = drRow.Item("PBT")
                                rCompany.City = drRow.Item("City")
                                rCompany.Area = drRow.Item("Area")
                                rCompany.Status = drRow.Item("Status")
                                rCompany.BizGrp = drRow.Item("BizGrp")


                            Next
                            lstCompany.Add(rCompany)
                        Else
                            rCompany = Nothing
                        End If
                        Return lstCompany
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Company", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rCompany = Nothing
                lstCompany = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetCompanyList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing, Optional ByVal OrderCond As String = Nothing, Optional ByVal isFacility As Boolean = False) As Data.DataTable
            If StartConnection() = True Then
                With CompanyInfo.MyInfo
                    If SQL = Nothing Or SQL = String.Empty Then
                        If isFacility Then
                            strSQL = "SELECT DISTINCT e.*" &
                                    " FROM BIZENTITY e WITH (NOLOCK)" &
                                    " LEFT JOIN BIZLOCATE l WITH (NOLOCK) on e.BizRegID=l.BizRegID" &
                                    " LEFT JOIN LICENSE c WITH (NOLOCK) ON e.BIZREGID = c.COMPANYID AND c.LOCID = l.BIZLOCID" &
                                    " LEFT JOIN LICENSECATEGORY lc WITH (NOLOCK) ON lc.LICENSECODE = c.CONTCATEGORY" &
                                    " LEFT JOIN STATE s WITH (NOLOCK) ON s.STATECODE = e.STATE AND s.COUNTRYCODE = e.COUNTRY" &
                                    " LEFT JOIN CODEMASTER cm WITH (NOLOCK) ON cm.CODETYPE = 'LTY' AND cm.CODE = c.CONTTYPE" &
                                    " LEFT JOIN VEHICLE v WITH (NOLOCK) on c.CompanyID = v.CompanyID" &
                                    " LEFT JOIN LICENSEITEM I WITH (NOLOCK) on c.ContractNo = I.ContractNo "

                            If Not FieldCond Is Nothing And FieldCond <> "" Then strSQL &= " WHERE " & FieldCond

                        Else
                            strSQL = BuildSelect(.FieldsList, .TableName, FieldCond)
                        End If
                    Else
                        strSQL = SQL
                    End If

                    If Not OrderCond Is Nothing Then
                        strSQL &= " ORDER BY " & OrderCond
                    End If

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetCompanyListAPI(Optional ByVal CompanyName As String = "") As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With CompanyInfo.MyInfo
                    strSQL = BuildSelect(.FieldsList, .TableName, "")

                    If CompanyName IsNot Nothing AndAlso CompanyName <> "" Then
                        strSQL &= " WHERE  Replace(CompanyName,' ','') =  Replace('" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompanyName) & "',' ','')"
                    End If

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetCompanyList_report(Optional ByVal Flg As String = Nothing, Optional ByVal ValidDate As String = Nothing, Optional ByVal State As String = Nothing, Optional ByVal City As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With CompanyInfo.MyInfo
                    strSQL = "SELECT DISTINCT e.BizRegID, e.RegNo, e.CompanyName, e.CreateDate, e.UpdateBy, e.LastUpdate, e.Active" &
                            " FROM BIZENTITY e WITH (NOLOCK)" &
                            " LEFT JOIN BIZLOCATE l WITH (NOLOCK) on e.BizRegID=l.BizRegID" &
                            " LEFT JOIN LICENSE c WITH (NOLOCK) ON e.BIZREGID = c.COMPANYID AND c.LOCID = l.BIZLOCID" &
                            " LEFT JOIN LICENSECATEGORY lc WITH (NOLOCK) ON lc.LICENSECODE = c.CONTCATEGORY" &
                            " LEFT JOIN STATE s WITH (NOLOCK) ON s.STATECODE = e.STATE AND s.COUNTRYCODE = e.COUNTRY" &
                            " LEFT JOIN CODEMASTER cm WITH (NOLOCK) ON cm.CODETYPE = 'LTY' AND cm.CODE = c.CONTTYPE" &
                            " LEFT JOIN VEHICLE v WITH (NOLOCK) on c.CompanyID = v.CompanyID" &
                            " LEFT JOIN LICENSEITEM I WITH (NOLOCK) on c.ContractNo = I.ContractNo "
                    If Flg IsNot Nothing AndAlso ValidDate IsNot Nothing AndAlso ValidDate IsNot Nothing AndAlso ValidDate IsNot Nothing Then
                        strSQL &= " WHERE "
                    End If

                    If Flg IsNot Nothing Then
                        strSQL &= " c.Flag = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Flg) & "' "
                    End If

                    strSQL &= " AND c.ContType is not null"

                    If ValidDate IsNot Nothing Then
                        strSQL &= " AND c.ValidityStart <= '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ValidDate) & " 00:00:00'" &
                                  " AND c.ValidityEnd >= '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, ValidDate) & " 23:59:59'"
                    End If

                    If State IsNot Nothing Then
                        strSQL &= " AND l.state = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, State) & "' "
                    End If

                    If City IsNot Nothing Then
                        strSQL &= " AND l.city = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, City) & "' "
                    End If

                    strSQL &= "ORDER BY CompanyName"

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetCompanyDashboard(Optional ByVal Condition As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With CompanyInfo.MyInfo
                    strSQL = "SELECT TOP 5 BL.BranchName AS 'BranchName',BL.AccNo as 'DOEFileNo' , *, " &
                                " ISNULL(S.StateDesc,'') as StateDesc, " &
                                " ISNULL(P.PBTDesc,'') as PBTDesc, " &
                                " ISNULL(C.CityDesc,'') as CityDesc, " &
                                " ISNULL(CO.CountryDesc,'') as CountryDesc, " &
                                " ROW_NUMBER() OVER (ORDER BY CompanyName asc) as RecNo, " &
                                " ISNULL(SIC.SICDesc,'') as Industry " &
                                " FROM BIZENTITY BE WITH (NOLOCK) " &
                                " INNER JOIN BIZLOCATE BL WITH (NOLOCK) ON BL.BizRegID =BE.BizRegID " &
                                " LEFT JOIN STATE S WITH (NOLOCK) ON S.StateCode=BE.State and S.CountryCode=BE.Country " &
                                " LEFT JOIN PBT P WITH (NOLOCK) ON P.PBTCode=BE.PBT and P.CountryCode=BE.Country and P.StateCode=BE.State " &
                                " LEFT JOIN CITY C WITH (NOLOCK) ON C.CityCode=BE.City and C.CountryCode=BE.Country and C.StateCode=BE.State " &
                                " LEFT JOIN COUNTRY CO WITH (NOLOCK) ON CO.CountryCode=BE.Country " &
                                " LEFT JOIN SIC WITH (NOLOCK) ON SIC.SICCode=BE.IndustryType "
                    If Not Condition Is Nothing And Condition <> "" Then strSQL &= " WHERE " & Condition

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetCompanyEntity(Optional ByVal State As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With CompanyInfo.MyInfo
                    strSQL = "SELECT distinct b.BizRegID, b.CompanyType, RegNo, l.branchname as CompanyName, b.CreateDate, b.UpdateBy, b.LastUpdate, CASE WHEN b.Active=1 AND b.Flag=1 THEN 'Active' Else 'In-Active' END AS Active, if(KKM='0', 'Not Included', 'Included') as KKM" &
                            " FROM BIZENTITY B WITH (NOLOCK) " &
                            " INNER JOIN BIZLOCATE L WITH (NOLOCK) ON L.BizRegID =B.BizRegID  " &
                            " where b.bizregid = l.bizregid "
                    If State IsNot Nothing Then
                        strSQL &= " AND l.state = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, State) & "' "
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
        Public Overloads Function GetCompanyCustomList(Optional ByVal Condition As String = Nothing, Optional ByVal _flag_active As String = Nothing, Optional ByVal _UserState As String = Nothing, Optional ByVal _approvalstatus_temp As String = Nothing, Optional ByVal _paramState As String = Nothing, Optional ByVal _date1 As String = Nothing, Optional ByVal _date2 As String = Nothing, Optional ByVal _recno As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With CompanyInfo.MyInfo

                    strSQL = "SELECT B.*, " & _
                        " CASE B.status WHEN 0 THEN 'Pending' WHEN 1 THEN 'Approved' ELSE 'Declined' END as _StatusApproval, " & _
                        " ISNULL(S.StateDesc,'') as StateDesc, " & _
                        " ISNULL(P.PBTDesc,'') as PBTDesc, " & _
                        " ISNULL(C.CityDesc,'') as CityDesc, " & _
                        " ISNULL(CO.CountryDesc,'') as CountryDesc, " & _
                        " ROW_NUMBER() OVER (ORDER BY B.CompanyName asc) as RecNo, " & _
                        " ISNULL(SIC.SICDesc,'') as Industry " & _
                        " FROM BIZENTITY B WITH (NOLOCK) " & _
                        " LEFT JOIN STATE S WITH (NOLOCK) ON S.StateCode=B.State and S.CountryCode=B.Country " & _
                        " LEFT JOIN PBT P WITH (NOLOCK) ON P.PBTCode=B.PBT and P.CountryCode=B.Country and P.StateCode=B.State " & _
                        " LEFT JOIN CITY C WITH (NOLOCK) ON C.CityCode=B.City and C.CountryCode=B.Country and C.StateCode=B.State " & _
                        " LEFT JOIN COUNTRY CO WITH (NOLOCK) ON CO.CountryCode=B.Country " & _
                        " LEFT JOIN SIC WITH (NOLOCK) ON SIC.SICCode=B.IndustryType " & _
                        " WHERE "

                    strSQL &= " B.Flag='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, _flag_active) & "'"

                    If Not _UserState Is Nothing And _UserState <> "" Then
                        strSQL &= " AND State='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, _UserState) & "'"
                    End If


                    If Not _approvalstatus_temp Is Nothing And _approvalstatus_temp <> "" Then
                        If _approvalstatus_temp = "0" Then
                            strSQL &= "  AND B.Status =" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, _approvalstatus_temp)
                        Else
                            strSQL &= "  AND B.Status >=" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, _approvalstatus_temp)
                        End If
                    End If

                    If Not _paramState Is Nothing And _paramState <> "" Then
                        strSQL &= " AND State='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, _paramState) & "'"
                    End If

                    If _date1 <> "" And _date2 <> "" Then
                        strSQL &= " And (b.CreateDate >= '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, _date1) & "' and b.CreateDate <= '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, _date2) & "') "
                    End If

                    If _recno <> "" And Not _recno Is Nothing Then
                        strSQL &= " AND RegNo LIKE '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, _recno) & "%' "
                    End If

                    strSQL &= " ORDER BY CreateDate DESC, LastUpdate DESC"

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndSQLControl()
            EndConnection()

        End Function

        'Add
        Public Overloads Function GetContractorCustomList(Optional ByVal BranchName As String = Nothing, Optional ByVal ContractNo As String = Nothing, Optional ByVal RegNo As String = Nothing, Optional ByVal BizLocID As String = Nothing, Optional ByVal CategoryID As String = Nothing, Optional ByVal Status As String = Nothing, Optional ByVal WasteCode As String = Nothing, Optional ByVal State As String = Nothing, Optional ByVal LicenseCategory As String = Nothing, Optional ByVal City As String = Nothing, Optional ByVal Area As String = Nothing, Optional ByVal TypeOfLicense As String = Nothing) As Data.DataTable
            Try
                If StartConnection() = True Then
                    With CompanyInfo.MyInfo
                        StartSQLControl()
                        strSQL = "SELECT * FROM ( " & _
                                "SELECT ROW_NUMBER() OVER (ORDER BY CONTRACTOR) AS [#], TABLE1.* FROM ( " & _
                                "SELECT DISTINCT (case when l.BRANCHNAME is null then d.COMPANYNAME + ' : ' + d.Address1 + ' ' + d.Address2 + ' ' + d.Address3 + ' ' + d.Address4 + ' ' + d.PostalCode else l.BRANCHNAME + ' : ' + l.Address1 + ' ' + l.Address2 + ' ' + l.Address3 + ' ' + l.Address4 + ' ' + l.PostalCode  end) AS CONTRACTOR,c.CONTRACTNO AS [LICENSE NO],cm.CODEDESC + ' : ' + lc.LICENSEDESC AS [TYPE OF LICENSE],(SELECT Case ContType When 'R' then (select dbo.GetWasteCode(c.ContractNo)) When 'T' then (SELECT dbo.GetRegNoVehicle(c.CompanyID)) End ) AS [WASTE GROUP],e.TelNo, e.FaxNo, CONVERT(VARCHAR(24),c.ValidityEnd,103) as ValidityEnd " & _
                                "FROM LICENSE c WITH (NOLOCK) " & _
                                "LEFT JOIN  BIZENTITY e WITH (NOLOCK) ON e.BIZREGID = c.COMPANYID " & _
                                "LEFT JOIN BIZLOCATE l WITH (NOLOCK) on l.BizRegID = e.BizRegID AND c.LOCID = l.BIZLOCID  " & _
                                "LEFT JOIN DOEFILENOREG d WITH (NOLOCK) on d.DOEFileNo=c.FileNo and d.IDPREMIS=c.CompanyID " & _
                                "LEFT JOIN LICENSECATEGORY lc WITH (NOLOCK) ON lc.LICENSECODE = c.CONTCATEGORY " & _
                                "LEFT JOIN STATE s WITH (NOLOCK) ON s.STATECODE = e.STATE AND s.COUNTRYCODE = e.COUNTRY " & _
                                "LEFT JOIN AREA a WITH (NOLOCK) ON a.AreaCode = e.Area AND a.StateCode = e.State AND a.CountryCode = e.COUNTRY " & _
                                "LEFT JOIN CODEMASTER cm WITH (NOLOCK) ON cm.CODETYPE = 'LTY' AND cm.CODE = c.CONTTYPE " & _
                                "LEFT JOIN VEHICLE v WITH (NOLOCK) on c.CompanyID = v.CompanyID " & _
                                "LEFT JOIN LICENSEITEM I WITH (NOLOCK) on c.ContractNo = I.ContractNo " & _
                                "WHERE c.ValidityStart<='" & String.Format("{0:yyyy-MM-dd}", Now()) & " 23:59:59' AND c.ValidityEnd>='" & String.Format("{0:yyyy-MM-dd}", Now()) & " 00:00:00' AND c.Flag=1 and c.ContType is not null and (e.CompanyName is not null OR d.CompanyName is not null) "

                        If BizLocID IsNot Nothing And BizLocID <> String.Empty Then
                            strSQL &= " and l.BizLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, BizLocID) & "'"
                        End If

                        strSQL &= ") AS TABLE1 " & _
                                ") AS RESULT"

                        Dim strFilter As String = ""
                        If Not BranchName Is Nothing AndAlso BranchName <> "" Then
                            strFilter &= " AND (d.COMPANYNAME LIKE '%" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, BranchName) & "%' or l.BRANCHNAME LIKE '%" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, BranchName) & "%' )"
                        End If
                        If Not ContractNo Is Nothing AndAlso ContractNo <> "" Then
                            strFilter &= " AND  c.ContractNo LIKE '%" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, ContractNo) & "%'"
                        End If
                        If Not RegNo Is Nothing AndAlso RegNo <> "" Then
                            strFilter &= " AND v.RegNo LIKE '%" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, RegNo) & "%'"
                        End If
                        If Not CategoryID Is Nothing AndAlso CategoryID <> "" Then
                            strFilter &= " and c.ContCategory in('" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, CategoryID) & "')"
                        End If

                        If Not Status Is Nothing AndAlso Status <> "" Then
                            strFilter &= " and c.ContType ='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, Status) & "'"
                        End If

                        If Not WasteCode Is Nothing AndAlso WasteCode <> "" Then
                            strFilter &= " and i.ItemCode in('" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, WasteCode) & "')"
                        End If

                        'Company state
                        If Not State Is Nothing AndAlso State <> "" Then
                            strFilter &= " AND (e.State ='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, State) & "' OR d.State='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, State) & "' )"
                        End If

                        'Company license category
                        If Not LicenseCategory Is Nothing AndAlso LicenseCategory <> "" Then
                            strFilter &= " AND c.ContCategory ='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, LicenseCategory) & "'"
                        End If

                        'Company city
                        If Not City Is Nothing AndAlso City <> "" Then
                            strFilter &= " AND (e.CIty ='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, City) & "' or d.city ='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, City) & "')"
                        End If

                        'Company area
                        If Not Area Is Nothing AndAlso Area <> "" Then
                            strFilter &= " AND (e.Area ='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, Area) & "' or d.area='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, Area) & "' )"
                        End If

                        'Company type of license
                        If Not TypeOfLicense Is Nothing AndAlso TypeOfLicense <> "" Then
                            strFilter &= " AND c.ContType ='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtStringN, TypeOfLicense) & "'"
                        End If

                        strSQL = "SELECT * FROM ( " & _
                            "SELECT ROW_NUMBER() OVER (ORDER BY CONTRACTOR) AS [#], TABLE1.* FROM ( " & _
                            "SELECT DISTINCT (case when l.BRANCHNAME is null then d.COMPANYNAME + ' : ' + d.Address1 + ' ' + d.Address2 + ' ' + d.Address3 + ' ' + d.Address4 + ' ' + d.PostalCode else l.BRANCHNAME + ' : ' + l.Address1 + ' ' + l.Address2 + ' ' + l.Address3 + ' ' + l.Address4 + ' ' + l.PostalCode  end) AS CONTRACTOR,c.CONTRACTNO AS [LICENSE NO],cm.CODEDESC + ' : ' + lc.LICENSEDESC AS [TYPE OF LICENSE],(SELECT Case ContType When 'R' then (select dbo.GetWasteCode(c.ContractNo)) When 'T' then (SELECT dbo.GetRegNoVehicle(c.CompanyID)) End ) AS [WASTE GROUP],e.TelNo, e.FaxNo, CONVERT(VARCHAR(24),c.ValidityEnd,103) as ValidityEnd " & _
                            "FROM LICENSE c WITH (NOLOCK) " & _
                            "LEFT JOIN  BIZENTITY e WITH (NOLOCK) ON e.BIZREGID = c.COMPANYID " & _
                            "LEFT JOIN BIZLOCATE l WITH (NOLOCK) on l.BizRegID = e.BizRegID AND c.LOCID = l.BIZLOCID  " & _
                            "LEFT JOIN DOEFILENOREG d WITH (NOLOCK) on d.DOEFileNo=c.FileNo and d.IDPREMIS=c.CompanyID " & _
                            "LEFT JOIN LICENSECATEGORY lc WITH (NOLOCK) ON lc.LICENSECODE = c.CONTCATEGORY " & _
                            "LEFT JOIN STATE s WITH (NOLOCK) ON s.STATECODE = e.STATE AND s.COUNTRYCODE = e.COUNTRY " & _
                            "LEFT JOIN AREA a WITH (NOLOCK) ON a.AreaCode = e.Area AND a.StateCode = e.State AND a.CountryCode = e.COUNTRY " & _
                            "LEFT JOIN CODEMASTER cm WITH (NOLOCK) ON cm.CODETYPE = 'LTY' AND cm.CODE = c.CONTTYPE " & _
                            "LEFT JOIN VEHICLE v WITH (NOLOCK) on c.CompanyID = v.CompanyID " & _
                            "LEFT JOIN LICENSEITEM I WITH (NOLOCK) on c.ContractNo = I.ContractNo " & _
                            "WHERE c.ValidityStart<='" & String.Format("{0:yyyy-MM-dd}", Now()) & " 23:59:59' AND c.ValidityEnd>='" & String.Format("{0:yyyy-MM-dd}", Now()) & " 00:00:00' AND c.Flag=1 and c.ContType is not null and (l.BRANCHNAME is not null OR d.CompanyName is not null) " & strFilter & _
                             ") AS TABLE1 " & _
                             ") AS RESULT"
                        'End If

                        Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                    End With
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Company", ex.Message & " " & strSQL, ex.StackTrace)
            Finally
                EndSQLControl()
                EndConnection()
            End Try
            Return Nothing
        End Function

        Public Overloads Function GetCompanyList2(Optional ByVal Flg As String = Nothing, Optional ByVal order As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With CompanyInfo.MyInfo
                    strSQL = "SELECT DISTINCT e.*" & _
                             " FROM BIZENTITY e WITH (NOLOCK)" & _
                             " LEFT JOIN BIZLOCATE l WITH (NOLOCK) on e.BizRegID=l.BizRegID" & _
                             " LEFT JOIN LICENSE c WITH (NOLOCK) ON e.BIZREGID = c.COMPANYID AND c.LOCID = l.BIZLOCID" & _
                             " LEFT JOIN LICENSECATEGORY lc WITH (NOLOCK) ON lc.LICENSECODE = c.CONTCATEGORY" & _
                             " LEFT JOIN STATE s WITH (NOLOCK) ON s.STATECODE = e.STATE AND s.COUNTRYCODE = e.COUNTRY" & _
                             " LEFT JOIN CODEMASTER cm WITH (NOLOCK) ON cm.CODETYPE = 'LTY' AND cm.CODE = c.CONTTYPE" & _
                             " LEFT JOIN VEHICLE v WITH (NOLOCK) on c.CompanyID = v.CompanyID" & _
                             " LEFT JOIN LICENSEITEM I WITH (NOLOCK) on c.ContractNo = I.ContractNo "
                    If Flg IsNot Nothing AndAlso order IsNot Nothing Then
                        strSQL &= " WHERE "
                    End If

                    If Not Flg Is Nothing Then
                        strSQL &= " e.Flag = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Flg) & "' "
                    End If

                    If Not order Is Nothing Then
                        strSQL &= " ORDER BY " & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Flg)
                    End If

                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetCountry(Optional ByVal Condition As String = Nothing) As Data.DataTable
            Try
                If StartConnection() = True Then
                    With CompanyInfo.MyInfo
                        StartSQLControl()
                        strSQL = "select s.StateDesc " & _
                                "from bizlocate b " & _
                                "INNER JOIN State s WITH (NOLOCK) ON s.StateCode = b.State " & _
                                "WHERE  BizRegID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, Condition) & "'"

                        Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                    End With
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Company", ex.Message & " " & strSQL, ex.StackTrace)
            Finally
                EndSQLControl()
                EndConnection()
            End Try
            Return Nothing
        End Function

        Public Overloads Function GetCompanyPendingApprovalByUserID(ByVal UserID As String) As Boolean
            Dim rCompany As Container.Company = Nothing
            Dim lstCompany As List(Of Container.Company) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""

            If StartConnection() = True Then
                StartSQLControl()
                With CompanyInfo.MyInfo
                    strSQL = "SELECT BL.BranchName AS 'BranchName',BL.AccNo as 'DOEFileNo' , *, " & _
                                " ISNULL(S.StateDesc,'') as StateDesc, " & _
                                " ISNULL(P.PBTDesc,'') as PBTDesc, " & _
                                " ISNULL(C.CityDesc,'') as CityDesc, " & _
                                " ISNULL(CO.CountryDesc,'') as CountryDesc, " & _
                                " ROW_NUMBER() OVER (ORDER BY CompanyName asc) as RecNo, " & _
                                " ISNULL(SIC.SICDesc,'') as Industry " & _
                                " FROM BIZENTITY BE WITH (NOLOCK) " & _
                                " INNER JOIN BIZLOCATE BL WITH (NOLOCK) ON BL.BizRegID =BE.BizRegID " & _
                                " LEFT JOIN STATE S WITH (NOLOCK) ON S.StateCode=BE.State and S.CountryCode=BE.Country " & _
                                " LEFT JOIN PBT P WITH (NOLOCK) ON P.PBTCode=BE.PBT and P.CountryCode=BE.Country and P.StateCode=BE.State " & _
                                " LEFT JOIN CITY C WITH (NOLOCK) ON C.CityCode=BE.City and C.CountryCode=BE.Country and C.StateCode=BE.State " & _
                                " LEFT JOIN COUNTRY CO WITH (NOLOCK) ON CO.CountryCode=BE.Country " & _
                                " LEFT JOIN SIC WITH (NOLOCK) ON SIC.SICCode=BE.IndustryType " & _
                                " INNER JOIN USRPROFILE USR WITH (NOLOCK) ON USR.ParentID = BE.BizRegID" & _
                                " WHERE BE.Flag='1' AND BE.Status=0 AND USR.UserID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, UserID) & "'"
                    dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                    Dim rowCount As Integer = 0
                    If dtTemp IsNot Nothing And dtTemp.Rows.Count > 0 Then
                        Return True
                    Else
                        Return False
                    End If
                End With
            Else
                Return False
            End If
            EndSQLControl()
            EndConnection()
        End Function

        Public Overloads Function GetWGAllList(Optional ByVal BranchName As String = "") As Data.DataTable
            Try
                If StartConnection() = True Then
                    With CompanyInfo.MyInfo
                        StartSQLControl()
                        strSQL = "Select BIZLOCATE.BranchName, BIZLOCATE.BizRegID, BIZLOCATE.BizLocID, BIZLOCATE.ContactEmail from BIZLOCATE INNER JOIN BIZENTITY ON BIZLOCATE.BIZREGID=BIZENTITY.BIZREGID WHERE CompanyType IN(2,5,6,9) AND BIZLOCATE.ACTIVE=1 AND BIZLOCATE.FLAG=1"
                        If BranchName <> "" Then
                            strSQL &= " AND BIZLOCATE.BranchName='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, BranchName) & "'"
                        End If
                        Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                    End With
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Profiles/Company", ex.Message & " " & strSQL, ex.StackTrace)
                'Throw ex
            Finally
                EndSQLControl()
                EndConnection()
            End Try
            Return Nothing
        End Function

        Public Overloads Function GetCompanyByUserID(ByVal UserID As System.String, ByVal LocID As System.String, ByVal CompanyType As System.String, Optional ByRef ErrCode As Integer = 0) As Container.Company
            Dim rCompany As Container.Company = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With CompanyInfo.MyInfo

                        strSQL = "Select be.BizRegID, be.CompanyName, be.CompanyType, be.ContactPerson, be.ContactDesignation FROM USRPROFILE u WITH (NOLOCK) " & _
                                " INNER JOIN BIZENTITY be WITH (NOLOCK) ON be.BizRegID=u.ParentID" & _
                                " INNER JOIN BIZLOCATE bl WITH (NOLOCK) ON be.BizRegID=bl.BizRegID"
                        strSQL &= " WHERE u.STATUS<>0 AND u.UserID= '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, UserID) & "' AND bl.BizLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, LocID) & "'"

                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)

                        If dtTemp IsNot Nothing AndAlso dtTemp.Rows.Count > 0 Then
                            Dim drRow = dtTemp.Rows(0)

                            If CompanyType = "WG" Then
                                If drRow.Item("CompanyType") = "2" Or drRow.Item("CompanyType") = "5" Or drRow.Item("CompanyType") = "6" Or drRow.Item("CompanyType") = "9" Then
                                    ErrCode = 0
                                Else
                                    ErrCode = 1
                                End If
                            ElseIf CompanyType = "WT" Then
                                If drRow.Item("CompanyType") = "3" Or drRow.Item("CompanyType") = "5" Or drRow.Item("CompanyType") = "7" Or drRow.Item("CompanyType") = "9" Then
                                    ErrCode = 0
                                Else
                                    ErrCode = 1
                                End If
                            ElseIf CompanyType = "WR" Then
                                If drRow.Item("CompanyType") = "4" Or drRow.Item("CompanyType") = "6" Or drRow.Item("CompanyType") = "7" Or drRow.Item("CompanyType") = "9" Then
                                    ErrCode = 0
                                Else
                                    ErrCode = 1
                                End If
                            End If

                            If ErrCode = 0 Then
                                rCompany = New Container.Company
                                rCompany.BizRegID = drRow.Item("BizRegID")
                                rCompany.CompanyName = drRow.Item("CompanyName")
                                rCompany.CompanyType = drRow.Item("CompanyType")
                                rCompany.ContactPerson = drRow.Item("ContactPerson")
                                rCompany.ContactDesignation = drRow.Item("ContactDesignation")
                            ElseIf ErrCode = 1 Then
                                rCompany = Nothing
                            End If
                        Else
                            rCompany = Nothing
                        End If
                    End With
                    Return rCompany
                End If

            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Security/UserProfile", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rCompany = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

#End Region
    End Class

    Public Class container_company
        Private _id As String
        Private _name As String
        Private _address As String
        Private _licenseno As String
        Private _typeoflicense As String
        Private _wastegroup As String
        Private _telno As String
        Private _faxno As String
        Private _expireddate As String
        Private _postalcode As String
        Private _state As String

        Private _locid As String
        Private _referenceno As String
        Private _date As String
        Private _wastecode As String
        Private _wastename As String
        Private _wastecomponent As String
        Private _wastetype As String
        Private _sourceofwaste As String
        Private _qty As String
        Private _packagetype As String

        Private _country As String
        Private _pic As String
        Private _mobileno As String
        Private _email As String

        Public Property ID() As String
            Get
                Return _id
            End Get
            Set(ByVal value As String)
                _id = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property Address() As String
            Get
                Return _address
            End Get
            Set(ByVal value As String)
                _address = value
            End Set
        End Property

        Public Property LicenseNo() As String
            Get
                Return _licenseno
            End Get
            Set(ByVal value As String)
                _licenseno = value
            End Set
        End Property

        Public Property TypeOfLicense() As String
            Get
                Return _typeoflicense
            End Get
            Set(ByVal value As String)
                _typeoflicense = value
            End Set
        End Property

        Public Property WasteGroup() As String
            Get
                Return _wastegroup
            End Get
            Set(ByVal value As String)
                _wastegroup = value
            End Set
        End Property

        Public Property TelNo() As String
            Get
                Return _telno
            End Get
            Set(ByVal value As String)
                _telno = value
            End Set
        End Property

        Public Property FaxNo() As String
            Get
                Return _faxno
            End Get
            Set(ByVal value As String)
                _faxno = value
            End Set
        End Property

        Public Property ExpiredDate() As String
            Get
                Return _expireddate
            End Get
            Set(ByVal value As String)
                _expireddate = value
            End Set
        End Property

        Public Property PostalCode() As String
            Get
                Return _postalcode
            End Get
            Set(ByVal value As String)
                _postalcode = value
            End Set
        End Property

        Public Property State() As String
            Get
                Return _state
            End Get
            Set(ByVal value As String)
                _state = value
            End Set
        End Property

        Public Property LocID() As String
            Get
                Return _locid
            End Get
            Set(ByVal value As String)
                _locid = value
            End Set
        End Property

        Public Property ReferenceNo() As String
            Get
                Return _referenceno
            End Get
            Set(ByVal value As String)
                _referenceno = value
            End Set
        End Property

        Public Property Dates() As String
            Get
                Return _date
            End Get
            Set(ByVal value As String)
                _date = value
            End Set
        End Property

        Public Property WasteCode() As String
            Get
                Return _wastecode
            End Get
            Set(ByVal value As String)
                _wastecode = value
            End Set
        End Property

        Public Property WasteName() As String
            Get
                Return _wastename
            End Get
            Set(ByVal value As String)
                _wastename = value
            End Set
        End Property

        Public Property WasteComponent() As String
            Get
                Return _wastecomponent
            End Get
            Set(ByVal value As String)
                _wastecomponent = value
            End Set
        End Property

        Public Property WasteType() As String
            Get
                Return _wastetype
            End Get
            Set(ByVal value As String)
                _wastetype = value
            End Set
        End Property

        Public Property SourceOfWaste() As String
            Get
                Return _sourceofwaste
            End Get
            Set(ByVal value As String)
                _sourceofwaste = value
            End Set
        End Property

        Public Property Qty() As String
            Get
                Return _qty
            End Get
            Set(ByVal value As String)
                _qty = value
            End Set
        End Property

        Public Property PackageType() As String
            Get
                Return _packagetype
            End Get
            Set(ByVal value As String)
                _packagetype = value
            End Set
        End Property

        Public Property Country() As String
            Get
                Return _country
            End Get
            Set(ByVal value As String)
                _country = value
            End Set
        End Property

        Public Property PIC() As String
            Get
                Return _pic
            End Get
            Set(ByVal value As String)
                _pic = value
            End Set
        End Property

        Public Property MobileNo() As String
            Get
                Return _mobileno
            End Get
            Set(ByVal value As String)
                _mobileno = value
            End Set
        End Property

        Public Property Email() As String
            Get
                Return _email
            End Get
            Set(ByVal value As String)
                _email = value
            End Set
        End Property
    End Class

    Namespace Container
#Region "Class Container"
        Public Class Company
            Public fBizRegID As System.String = "BizRegID"
            Public fCompanyName As System.String = "CompanyName"
            Public fCompanyType As System.String = "CompanyType"
            Public fIndustryType As System.String = "IndustryType"
            Public fBusinesstype As System.String = "Businesstype"
            Public fRegNo As System.String = "RegNo"
            Public fAcctNo As System.String = "AcctNo"
            Public fAddress1 As System.String = "Address1"
            Public fAddress2 As System.String = "Address2"
            Public fAddress3 As System.String = "Address3"
            Public fAddress4 As System.String = "Address4"
            Public fPostalCode As System.String = "PostalCode"
            Public fState As System.String = "State"
            Public fCountry As System.String = "Country"
            Public fTelNo As System.String = "TelNo"
            Public fFaxNo As System.String = "FaxNo"
            Public fEmail As System.String = "Email"
            Public fCoWebsite As System.String = "CoWebsite"
            Public fContactPerson As System.String = "ContactPerson"
            Public fContactDesignation As System.String = "ContactDesignation"
            Public fContactPersonEmail As System.String = "ContactPersonEmail"
            Public fContactPersonTelNo As System.String = "ContactPersonTelNo"
            Public fContactPersonFaxNo As System.String = "ContactPersonFaxNo"
            Public fContactPersonMobile As System.String = "ContactPersonMobile"
            Public fRemark As System.String = "Remark"
            Public fActive As System.String = "Active"
            Public fInuse As System.String = "Inuse"
            Public fFlag As System.String = "Flag"
            Public fBizGrp As System.String = "BizGrp"
            Public fCreateDate As System.String = "CreateDate"
            Public fCreateBy As System.String = "CreateBy"
            Public fLastUpdate As System.String = "LastUpdate"
            Public fUpdateBy As System.String = "UpdateBy"
            Public frowguid As System.String = "rowguid"
            Public fPBT As System.String = "PBT"
            Public fCity As System.String = "City"
            Public fArea As System.String = "Area"
            Public fStatus As System.String = "Status"
            Public fStateDesc As System.String = "StateDesc"
            Public fPBTDesc As System.String = "PBTDesc"
            Public fCityDesc As System.String = "CityDesc"
            Public fAreaDesc As System.String = "AreaDesc"
            Public fCountryDesc As System.String = "CountryDesc"
            Public fDesignation As System.String = "Designation"
            Public fIndustrytypeDesc As System.String = "Industrytype"
            Public fBusinessTypeDesc As System.String = "BusinessType"


            Protected _BizRegID As System.String
            Private _CompanyName As System.String
            Private _CompanyType As System.String
            Public _IndustryType As System.String
            Public _Businesstype As System.String
            Private _RegNo As System.String
            Private _AcctNo As System.String
            Private _Address1 As System.String
            Private _Address2 As System.String
            Private _Address3 As System.String
            Private _Address4 As System.String
            Private _PostalCode As System.String
            Private _State As System.String
            Private _Country As System.String
            Private _PBT As System.String
            Private _City As System.String
            Private _Area As System.String
            Private _TelNo As System.String
            Private _FaxNo As System.String
            Private _Email As System.String
            Private _CoWebsite As System.String
            Private _ContactPerson As System.String
            Private _ContactDesignation As System.String
            Private _ContactPersonEmail As System.String
            Private _ContactPersonTelNo As System.String
            Private _ContactPersonFaxNo As System.String
            Private _ContactPersonMobile As System.String
            Private _Remark As System.String
            Private _Active As System.Byte
            Private _Inuse As System.Byte
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _rowguid As System.Guid
            Private _flag As System.Byte
            Private _BizGrp As System.Byte
            Public _Status As System.Byte
            Public _StateDesc As System.String
            Public _PBTDesc As System.String
            Public _CityDesc As System.String
            Public _AreaDesc As System.String
            Public _CountryDesc As System.String
            Public _Designation As System.String
            Public _IndustrytypeDesc As System.String
            Public _BusinessTypeDesc As System.String
            Public _RefID As System.String
            Public _KKM As System.Byte
            'Remark By Mei
            'Public _SubGrp As System.String

            Public Property KKM As System.Byte
                Get
                    Return _KKM
                End Get
                Set(ByVal Value As System.Byte)
                    _KKM = Value
                End Set
            End Property
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
            Public Property BusinessTypeDesc As System.String
                Get
                    Return _BusinessTypeDesc
                End Get
                Set(ByVal Value As System.String)
                    _BusinessTypeDesc = Value
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
            Public Property CompanyType As System.String
                Get
                    Return _CompanyType
                End Get
                Set(ByVal Value As System.String)
                    _CompanyType = Value
                End Set
            End Property


            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Industrytype As System.String
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
                    Return _Businesstype
                End Get
                Set(ByVal Value As System.String)
                    _Businesstype = Value
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
            Public Property AcctNo As System.String
                Get
                    Return _AcctNo
                End Get
                Set(ByVal Value As System.String)
                    _AcctNo = Value
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
            Public Property TelNo As System.String
                Get
                    Return _TelNo
                End Get
                Set(ByVal Value As System.String)
                    _TelNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property FaxNo As System.String
                Get
                    Return _FaxNo
                End Get
                Set(ByVal Value As System.String)
                    _FaxNo = Value
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
            Public Property CoWebsite As System.String
                Get
                    Return _CoWebsite
                End Get
                Set(ByVal Value As System.String)
                    _CoWebsite = Value
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
            Public Property ContactPersonEmail As System.String
                Get
                    Return _ContactPersonEmail
                End Get
                Set(ByVal Value As System.String)
                    _ContactPersonEmail = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ContactPersonTelNo As System.String
                Get
                    Return _ContactPersonTelNo
                End Get
                Set(ByVal Value As System.String)
                    _ContactPersonTelNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ContactPersonFaxNo As System.String
                Get
                    Return _ContactPersonFaxNo
                End Get
                Set(ByVal Value As System.String)
                    _ContactPersonFaxNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ContactPersonMobile As System.String
                Get
                    Return _ContactPersonMobile
                End Get
                Set(ByVal Value As System.String)
                    _ContactPersonMobile = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
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
            Public Property BizGrp As System.Byte
                Get
                    Return _BizGrp
                End Get
                Set(ByVal Value As System.Byte)
                    _BizGrp = Value
                End Set
            End Property

            'Remark By Mei
            ' ''' <summary>
            ' ''' Mandatory, Not Allow Null
            ' ''' </summary>
            'Public Property SubGrp As System.String
            '    Get
            '        Return _SubGrp
            '    End Get
            '    Set(ByVal Value As System.String)
            '        _SubGrp = Value
            '    End Set
            'End Property

        End Class
#End Region
    End Namespace

#Region "Class Info"
    Public Class CompanyInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "BizRegID,BizGrp,CompanyName,CompanyType,Industrytype,BusinessType,RegNo,AcctNo,Address1,Address2,Address3,Address4,PostalCode,State, ISNULL((SELECT TOP 1 StateDesc FROM State WITH (NOLOCK) WHERE StateCode=BIZENTITY.State and COUNTRY=BIZENTITY.Country),'') AS StateDesc,Country,TelNo,FaxNo,Email,CoWebsite,ContactPerson,ContactDesignation,ContactPersonEmail,ContactPersonTelNo,ContactPersonFaxNo,ContactPersonMobile,Remark,Active,Inuse,Flag,CreateDate,CreateBy,LastUpdate,UpdateBy,rowguid, PBT, ISNULL((SELECT TOP 1 PBTDesc FROM PBT WITH (NOLOCK) WHERE PBTCode=BIZENTITY.PBT and StateCode =BIZENTITY.State and COUNTRY =BIZENTITY.Country),'') AS PBTDesc, City, ISNULL((SELECT TOP 1 CityDesc FROM City WITH (NOLOCK) WHERE CityCode=BIZENTITY.City and StateCode =BIZENTITY.State and COUNTRY =BIZENTITY.Country),'') AS CityDesc, Area, ISNULL((SELECT TOP 1 AreaDesc FROM Area WITH (NOLOCK) WHERE CityCode=BIZENTITY.City and StateCode =BIZENTITY.State and COUNTRY =BIZENTITY.Country AND AreaCode=BIZENTITY.Area),'') AS AreaDesc, ISNULL((SELECT TOP 1 CountryDesc FROM Country WITH (NOLOCK) WHERE CountryCode=BIZENTITY.Country),'') AS CountryDesc,Status, ISNULL((SELECT TOP 1 CodeDesc FROM CodeMaster WITH (NOLOCK) WHERE Code=BIZENTITY.ContactDesignation and CodeType='DSN'),'') as Designation, ISNULL((SELECT TOP 1 SICDesc From SIC WITH (NOLOCK) Where SICCode=BIZENTITY.Industrytype),'') as IndustryTypeDesc, ISNULL((SELECT TOP 1 SubSICDesc From SUBSIC WITH (NOLOCK) Where SubSICCode=BIZENTITY.BusinessType AND SICCode=BIZENTITY.Industrytype),'') as BusinessTypeDesc, KKM "
                .CheckFields = "Active,Inuse,Flag"
                .TableName = "BIZENTITY WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "BizRegID,BizGrp,CompanyName,CompanyType,Industrytype,BusinessType,RegNo,AcctNo,Address1,Address2,Address3,Address4,PostalCode,State, ISNULL((SELECT TOP 1 StateDesc FROM State WITH (NOLOCK) WHERE StateCode=BIZENTITY.State and COUNTRY=BIZENTITY.Country),'') AS StateDesc,Country,TelNo,FaxNo,Email,CoWebsite,ContactPerson,ContactDesignation,ContactPersonEmail,ContactPersonTelNo,ContactPersonFaxNo,ContactPersonMobile,Remark,Active,Inuse,Flag,CreateDate,CreateBy,LastUpdate,UpdateBy,rowguid, PBT, ISNULL((SELECT TOP 1 PBTDesc FROM PBT WITH (NOLOCK) WHERE PBTCode=BIZENTITY.PBT and StateCode =BIZENTITY.State and COUNTRY =BIZENTITY.Country),'') AS PBTDesc, City, ISNULL((SELECT TOP 1 CityDesc FROM City WITH (NOLOCK) WHERE CityCode=BIZENTITY.City and StateCode =BIZENTITY.State and COUNTRY =BIZENTITY.Country),'') AS CityDesc, Area, ISNULL((SELECT TOP 1 AreaDesc FROM Area WITH (NOLOCK) WHERE CityCode=BIZENTITY.City and StateCode =BIZENTITY.State and COUNTRY =BIZENTITY.Country AND AreaCode=BIZENTITY.Area),'') AS AreaDesc, ISNULL((SELECT TOP 1 CountryDesc FROM Country WITH (NOLOCK) WHERE CountryCode=BIZENTITY.Country),'') AS CountryDesc,Status, ISNULL((SELECT TOP 1 CodeDesc FROM CodeMaster WITH (NOLOCK) WHERE Code=BIZENTITY.ContactDesignation and CodeType='DSN'),'') as Designation, ISNULL((SELECT TOP 1 SICDesc From SIC WITH (NOLOCK) Where SICCode=BIZENTITY.Industrytype),'') as IndustryTypeDesc, ISNULL((SELECT TOP 1 SubSICDesc From SUBSIC WITH (NOLOCK) Where SubSICCode=BIZENTITY.BusinessType AND SICCode=BIZENTITY.Industrytype),'') as BusinessTypeDesc, KKM "
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
    Public Class CompanyScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "BizRegID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "CompanyName"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CompanyType"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "RegNo"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "AcctNo"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Address1"
                .Length = 255
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Address2"
                .Length = 100
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Address3"
                .Length = 40
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Address4"
                .Length = 40
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "PostalCode"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "State"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Country"
                .Length = 2
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "TelNo"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "FaxNo"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Email"
                .Length = 80
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "CoWebsite"
                .Length = 80
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ContactPerson"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ContactDesignation"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ContactPersonEmail"
                .Length = 255
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ContactPersonTelNo"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ContactPersonMobile"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Remark"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(21, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(22, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Inuse"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(23, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(24, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(25, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(26, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(27, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(28, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(29, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "PBT"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(30, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "City"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(31, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Area"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(32, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ContactPersonFaxNo"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(33, this)

            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "BizGrp"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(34, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "IndustryType"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(35, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "BusinessType"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(36, this)
        End Sub

        Public ReadOnly Property BizRegID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property CompanyName As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property CompanyType As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property RegNo As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property AcctNo As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property Address1 As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property Address2 As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property Address3 As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property Address4 As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property PostalCode As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property State As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property Country As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property TelNo As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property FaxNo As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property Email As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property CoWebsite As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property ContactPerson As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property ContactDesignation As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property ContactPersonEmail As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property ContactPersonTelNo As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property ContactPersonMobile As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property
        Public ReadOnly Property Remark As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(22)
            End Get
        End Property
        Public ReadOnly Property Inuse As StrucElement
            Get
                Return MyBase.GetItem(23)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(24)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(25)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(26)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(27)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(28)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(29)
            End Get
        End Property
        Public ReadOnly Property PBT As StrucElement
            Get
                Return MyBase.GetItem(30)
            End Get
        End Property
        Public ReadOnly Property City As StrucElement
            Get
                Return MyBase.GetItem(31)
            End Get
        End Property
        Public ReadOnly Property Area As StrucElement
            Get
                Return MyBase.GetItem(32)
            End Get
        End Property
        Public ReadOnly Property ContactPersonFaxNo As StrucElement
            Get
                Return MyBase.GetItem(33)
            End Get
        End Property

        Public ReadOnly Property BizGrp As StrucElement
            Get
                Return MyBase.GetItem(34)
            End Get
        End Property
        Public ReadOnly Property IndustryType As StrucElement
            Get
                Return MyBase.GetItem(35)
            End Get
        End Property
        Public ReadOnly Property BusinessType As StrucElement
            Get
                Return MyBase.GetItem(36)
            End Get
        End Property
        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function

    End Class
#End Region

End Namespace