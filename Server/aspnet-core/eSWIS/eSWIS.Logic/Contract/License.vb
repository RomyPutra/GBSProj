Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared
Imports eSWIS.Shared.General

Namespace Contract
    Public NotInheritable Class License
        Inherits Core.CoreControl
        Private ContractInfo As ContractInfo = New ContractInfo
        Private Log As New SystemLog()


        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal ContractCont As Container.License, ByVal pType As SQLControl.EnumSQLType, ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If ContractCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ContractInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ContractCont.ContractNo) & "'")
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
                                .TableName = "LICENSE WITH (ROWLOCK)"
                                .AddField("ContType", ContractCont.ContType, SQLControl.EnumDataType.dtString)
                                .AddField("ContCategory", ContractCont.ContCategory, SQLControl.EnumDataType.dtString)
                                .AddField("InquiryDate", ContractCont.InquiryDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CompletedDate", ContractCont.CompletedDate, SQLControl.EnumDataType.dtDateTime)

                                .AddField("ValidityStart", ContractCont.ValidityStart, SQLControl.EnumDataType.dtDateTime)
                                .AddField("ValidityEnd", ContractCont.ValidityEnd, SQLControl.EnumDataType.dtDateTime)
                                .AddField("FileNo", ContractCont.FileNo, SQLControl.EnumDataType.dtStringN)
                                .AddField("SchemeNo", ContractCont.SchemeNo, SQLControl.EnumDataType.dtStringN)
                                .AddField("RefNo", ContractCont.RefNo, SQLControl.EnumDataType.dtStringN)
                                .AddField("CompanyID", ContractCont.CompanyID, SQLControl.EnumDataType.dtString)
                                .AddField("LocID", ContractCont.LocID, SQLControl.EnumDataType.dtString)
                                .AddField("Country", ContractCont.Country, SQLControl.EnumDataType.dtString)
                                .AddField("State", ContractCont.State, SQLControl.EnumDataType.dtString)
                                .AddField("City", ContractCont.City, SQLControl.EnumDataType.dtString)
                                .AddField("Area", ContractCont.Area, SQLControl.EnumDataType.dtString)
                                .AddField("Remark1", ContractCont.Remark1, SQLControl.EnumDataType.dtStringN)
                                .AddField("Remark2", ContractCont.Remark2, SQLControl.EnumDataType.dtStringN)
                                .AddField("Remark3", ContractCont.Remark3, SQLControl.EnumDataType.dtStringN)
                                .AddField("Remark4", ContractCont.Remark4, SQLControl.EnumDataType.dtStringN)
                                .AddField("Remark5", ContractCont.Remark5, SQLControl.EnumDataType.dtStringN)
                                .AddField("AuthID", ContractCont.AuthID, SQLControl.EnumDataType.dtString)
                                .AddField("AuthPOS", ContractCont.AuthPOS, SQLControl.EnumDataType.dtString)
                                .AddField("CreateDate", ContractCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("ApproveDate", ContractCont.ApproveDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("ApproveBy", ContractCont.ApproveBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", ContractCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", ContractCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", ContractCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", ContractCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", ContractCont.Flag, SQLControl.EnumDataType.dtNumeric)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            .AddField("CreateBy", ContractCont.CreateBy, SQLControl.EnumDataType.dtString)
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ContractCont.ContractNo) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("ContractNo", ContractCont.ContractNo, SQLControl.EnumDataType.dtStringN)
                                                .AddField("CreateBy", ContractCont.CreateBy, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ContractCont.ContractNo) & "'")
                                End Select
                            End With
                            Try
                                'execute
                                objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                                Return True

                            Catch axExecute As Exception
                                If pType = SQLControl.EnumSQLType.stInsert Then
                                    message = axExecute.Message.ToString()
                                Else
                                    message = axExecute.Message.ToString()
                                End If
                                Log.Notifier.Notify(axExecute)
                                Gibraltar.Agent.Log.Error("eSWISLogic/Contract/License", axExecute.Message & strSQL, axExecute.StackTrace)
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
                Gibraltar.Agent.Log.Error("eSWISLogic/Contract/License", axAssign.Message, axAssign.StackTrace)
                Return False
            Catch exAssign As SystemException
                message = exAssign.Message.ToString()
                Log.Notifier.Notify(exAssign)
                Gibraltar.Agent.Log.Error("eSWISLogic/Contract/License", exAssign.Message, exAssign.StackTrace)
                Return False
            Finally
                ContractCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal ContractCont As Container.License, ByRef message As String) As Boolean
            Return Save(ContractCont, SQLControl.EnumSQLType.stInsert, message)
        End Function

        'AMEND
        Public Function Update(ByVal ContractCont As Container.License, ByRef message As String) As Boolean
            Return Save(ContractCont, SQLControl.EnumSQLType.stUpdate, message)
        End Function

        Public Function Delete(ByVal ContractCont As Container.License, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If ContractCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ContractInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ContractCont.ContractNo) & "'")
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
                                strSQL = BuildUpdate("LICENSE WITH (ROWLOCK)", " SET Flag = 0" & _
                                " , LastUpdate = '" & ContractCont.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, ContractCont.UpdateBy) & "' WHERE " & _
                                " ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ContractCont.ContractNo) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete("LICENSE WITH (ROWLOCK)", "ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ContractCont.ContractNo) & "'")
                        End If

                        Try
                            'execute
                            objConn.Execute(strSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                            Return True
                        Catch exExecute As Exception
                            message = exExecute.Message.ToString()
                            Log.Notifier.Notify(exExecute)
                            Gibraltar.Agent.Log.Error("eSWISLogic/Contract/License", exExecute.Message & strSQL, exExecute.StackTrace)
                            Return False
                        End Try
                    End If
                End If

            Catch axDelete As ApplicationException
                message = axDelete.Message.ToString()
                Log.Notifier.Notify(axDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Contract/License", axDelete.Message, axDelete.StackTrace)
                Return False
            Catch exDelete As Exception
                message = exDelete.Message.ToString()
                Log.Notifier.Notify(exDelete)
                Gibraltar.Agent.Log.Error("eSWISLogic/Contract/License", exDelete.Message, exDelete.StackTrace)
                Return False
            Finally
                ContractCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetLicense(ByVal ContractNo As System.String) As Container.License
            Dim rContract As Container.License = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With ContractInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ContractNo) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rContract = New Container.License
                                rContract.ContractNo = drRow.Item("ContractNo")
                                rContract.ContType = drRow.Item("ContType")
                                rContract.ContCategory = drRow.Item("ContCategory")
                                rContract.InquiryDate = drRow.Item("InquiryDate")
                                rContract.CompletedDate = drRow.Item("CompletedDate")

                                rContract.ValidityStart = drRow.Item("ValidityStart")
                                rContract.ValidityEnd = drRow.Item("ValidityEnd")
                                rContract.FileNo = drRow.Item("FileNo")
                                rContract.SchemeNo = drRow.Item("SchemeNo")
                                rContract.RefNo = drRow.Item("RefNo")
                                rContract.CompanyID = drRow.Item("CompanyID")
                                rContract.LocID = drRow.Item("LocID")
                                rContract.Country = drRow.Item("Country")
                                rContract.State = drRow.Item("State")
                                rContract.City = drRow.Item("City")
                                rContract.Area = drRow.Item("Area")
                                rContract.Remark1 = drRow.Item("Remark1")
                                rContract.Remark2 = drRow.Item("Remark2")
                                rContract.Remark3 = drRow.Item("Remark3")
                                rContract.Remark4 = drRow.Item("Remark4")
                                rContract.Remark5 = drRow.Item("Remark5")
                                rContract.AuthID = drRow.Item("AuthID")
                                rContract.AuthPOS = drRow.Item("AuthPOS")
                                rContract.CreateDate = drRow.Item("CreateDate")
                                rContract.CreateBy = drRow.Item("CreateBy")
                                rContract.ApproveDate = drRow.Item("ApproveDate")
                                rContract.ApproveBy = drRow.Item("ApproveBy")
                                rContract.LastUpdate = drRow.Item("LastUpdate")
                                rContract.UpdateBy = IIf(IsDBNull(drRow.Item("UpdateBy")), "", drRow.Item("UpdateBy"))
                                rContract.Active = drRow.Item("Active")
                                rContract.Inuse = drRow.Item("Inuse")
                                rContract.Rowguid = drRow.Item("Rowguid")
                                rContract.SyncCreate = drRow.Item("SyncCreate")
                                rContract.SyncLastUpd = drRow.Item("SyncLastUpd")
                            Else
                                rContract = Nothing
                            End If
                        Else
                            rContract = Nothing
                        End If
                    End With
                End If
                Return rContract
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Contract/License", ex.Message, ex.StackTrace)
            Finally
                rContract = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetLicense(ByVal ContractNo As System.String, DecendingOrder As Boolean) As List(Of Container.License)
            Dim rContract As Container.License = Nothing
            Dim lstContract As List(Of Container.License) = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With ContractInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ByVal ContractNo As System.String DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "ContractNo = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, ContractNo) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rContract = New Container.License
                                rContract.ContractNo = drRow.Item("ContractNo")
                                rContract.ContType = drRow.Item("ContType")
                                rContract.ContCategory = drRow.Item("ContCategory")
                                rContract.InquiryDate = drRow.Item("InquiryDate")
                                rContract.CompletedDate = drRow.Item("CompletedDate")

                                rContract.ValidityStart = drRow.Item("ValidityStart")
                                rContract.ValidityEnd = drRow.Item("ValidityEnd")
                                rContract.FileNo = drRow.Item("FileNo")
                                rContract.SchemeNo = drRow.Item("SchemeNo")
                                rContract.RefNo = drRow.Item("RefNo")
                                rContract.CompanyID = drRow.Item("CompanyID")
                                rContract.LocID = drRow.Item("LocID")
                                rContract.Country = drRow.Item("Country")
                                rContract.State = drRow.Item("State")
                                rContract.City = drRow.Item("City")
                                rContract.Area = drRow.Item("Area")
                                rContract.Remark1 = drRow.Item("Remark1")
                                rContract.Remark2 = drRow.Item("Remark2")
                                rContract.Remark3 = drRow.Item("Remark3")
                                rContract.Remark4 = drRow.Item("Remark4")
                                rContract.Remark5 = drRow.Item("Remark5")
                                rContract.AuthID = drRow.Item("AuthID")
                                rContract.AuthPOS = drRow.Item("AuthPOS")
                                rContract.CreateDate = drRow.Item("CreateDate")
                                rContract.CreateBy = drRow.Item("CreateBy")
                                rContract.ApproveDate = drRow.Item("ApproveDate")
                                rContract.ApproveBy = drRow.Item("ApproveBy")
                                rContract.LastUpdate = drRow.Item("LastUpdate")
                                rContract.UpdateBy = IIf(IsDBNull(drRow.Item("UpdateBy")), "", drRow.Item("UpdateBy"))
                                rContract.Active = drRow.Item("Active")
                                rContract.Inuse = drRow.Item("Inuse")
                                rContract.Rowguid = drRow.Item("Rowguid")
                                rContract.SyncCreate = drRow.Item("SyncCreate")
                                rContract.SyncLastUpd = drRow.Item("SyncLastUpd")
                            Next
                            lstContract.Add(rContract)
                        Else
                            rContract = Nothing
                        End If
                        Return lstContract
                    End With
                End If
            Catch ex As Exception
                Log.Notifier.Notify(ex)
                Gibraltar.Agent.Log.Error("eSWISLogic/Contract/License", ex.Message, ex.StackTrace)
                'Throw ex
            Finally
                rContract = Nothing
                lstContract = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetLicenseList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ContractInfo.MyInfo
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

        Public Overloads Function GetCapacityIndividual(Optional ByVal strFilter As String = Nothing, Optional ByVal District As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With ContractInfo.MyInfo
                    StartSQLControl()

                    strSQL = "SELECT BL.AccNo, BL.BizRegID, BL.BizLocID, BL.BranchName, LC.UpdateBy, MAX(LC.LastUpdate) " & _
                        "As LastUpdate FROM BIZLOCATE BL WITH(NOLOCK) LEFT JOIN BIZENTITY BE WITH(NOLOCK) ON BL.Bizregid = BE.Bizregid " & _
                        "INNER JOIN LICENSE L ON BL.BizRegID = L.CompanyID AND BL.BizLocID = L.LocID INNER JOIN LICENSEITEM LI " & _
                        "ON L.ContractNo = LI.ContractNo LEFT JOIN LICENSECAPACITY LC ON BL.BizRegID = LC.ReceiverID AND BL.BizLocID = LC.ReceiverLocID " & _
                        "WHERE be.CompanyType IN ('4','6','7','9') AND LI.ItemCode LIKE 'SW%' "
                    If Not District Is Nothing AndAlso District <> "" Then
                        strSQL &= " AND bl.City='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, District) & "' "
                    End If
                    If Not strFilter Is Nothing AndAlso strFilter <> "" Then
                        strSQL &= strFilter
                    End If
                    strSQL &= "GROUP BY BL.AccNo, BL.BizRegID, BL.BizLocID, BL.BranchName, LC.UpdateBy ORDER BY MAX(LC.LastUpdate) DESC"
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
                With ContractInfo.MyInfo
                    StartSQLControl()
                    strSQL = "SELECT BL.BizRegID, BL.ContactPerson, BL.Tel, BL.Fax, BL.AccNo, BL.BizLocID, BL.BranchName, LI.ItemCode, BL.Address1, BL.Address2, BL.Address3, BL.Address4, C.CityDesc, S.StateDesc, CN.CountryDesc, " & _
                        "SUM(LI.Qty) AS CapacityLevel, LI.Qty - Green.ExpectedQty AS BalanceQty FROM BIZLOCATE BL WITH(NOLOCK) LEFT JOIN BIZENTITY BE WITH(NOLOCK) ON BL.Bizregid = BE.Bizregid LEFT JOIN CITY C ON BL.City = C.CityCode " & _
                        "LEFT JOIN State S ON BL.State = S.StateCode LEFT JOIN COUNTRY CN ON BL.Country = CN.CountryCode INNER JOIN LICENSE LS WITH(NOLOCK) ON BL.BizLocID = LS.LocID AND BL.BizRegID = LS.CompanyID INNER JOIN " & _
                        "LICENSEITEM LI WITH(NOLOCK) ON LS.ContractNo = LI.ContractNo " & _
                        "OUTER APPLY " & _
                        "(SELECT ISNULL(SUM(DT.ExpectedQty),0) AS ExpectedQty FROM TWG_SUBMISSIONHDR HD LEFT JOIN TWG_SUBMISSIONDTL DT ON HD.SubmissionID = DT.SubmissionID WHERE HD.ReceiverLocID = BL.BizLocID AND HD.Status = 3 " & _
                        "AND ((HD.StatusTWG = 1 AND HD.SubStatus = 0) OR (HD.StatusTWG IN (0,1) AND HD.SubStatus = 1) OR (HD.StatusTWG = 2 AND HD.SubStatus = 1)) AND DT.WasteCode = LI.ItemCode) Green " & _
                        "WHERE LS.ContType = 'R' AND BL.BizLocID = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, BizLocID) & "' GROUP BY BL.BizRegID, BL.ContactPerson, BL.BizLocID, BL.BranchName, BL.AccNo, LI.Qty, LI.ItemCode, BL.Tel, BL.Fax, BL.Address1, " & _
                        "BL.Address2, BL.Address3, BL.Address4, C.CityDesc, S.StateDesc, CN.CountryDesc, Green.ExpectedQty"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetCapacityIndividualbyBizLocIDAndWasteCode(ByVal BizLocID As String, ByVal WasteCode As String) As Data.DataTable
            If StartConnection() = True Then
                With ContractInfo.MyInfo
                    StartSQLControl()
                    strSQL = "SELECT CompanyID, LocID, ItemCode, Sum(Qty) as Qty " &
                        " FROM LICENSEITEM i WITH(NOLOCK) INNER JOIN LICENSE l WITH(NOLOCK) ON l.ContractNo=i.ContractNo and l.Active=1 and l.Flag=1 " &
                        " WHERE  l.LocID='" & BizLocID & "' and ItemCode='" & WasteCode & "' and CONVERT(date,l.ValidityEnd)>=CONVERT(date,GETDATE()) group by CompanyID, LocID, ItemCode"
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
        Public Class License
            Public fContractNo As System.String = "ContractNo"
            Public fContType As System.String = "ContType"
            Public fContCategory As System.String = "ContCategory"
            Public fInquiryDate As System.String = "InquiryDate"
            Public fAgreeDate As System.String = "AgreeDate"
            Public fValidityStart As System.String = "ValidityStart"
            Public fValidityEnd As System.String = "ValidityEnd"
            Public fFileNo As System.String = "FileNo"
            Public fSchemeNo As System.String = "SchemeNo"
            Public fRefNo As System.String = "RefNo"
            Public fCompanyID As System.String = "CompanyID"
            Public fLocID As System.String = "LocID"
            Public fCountry As System.String = "Country"
            Public fState As System.String = "State"
            Public fCity As System.String = "City"
            Public fArea As System.String = "Area"
            Public fRemark1 As System.String = "Remark1"
            Public fRemark2 As System.String = "Remark2"
            Public fRemark3 As System.String = "Remark3"
            Public fRemark4 As System.String = "Remark4"
            Public fRemark5 As System.String = "Remark5"
            Public fAuthID As System.String = "AuthID"
            Public fAuthPOS As System.String = "AuthPOS"
            Public fCreateDate As System.String = "CreateDate"
            Public fCreateBy As System.String = "CreateBy"
            Public fApproveDate As System.String = "ApproveDate"
            Public fApproveBy As System.String = "ApproveBy"
            Public fLastUpdate As System.String = "LastUpdate"
            Public fUpdateBy As System.String = "UpdateBy"
            Public fActive As System.String = "Active"
            Public fInuse As System.String = "Inuse"
            Public fFlag As System.String = "Flag"
            Public fRowguid As System.String = "Rowguid"
            Public fSyncCreate As System.String = "SyncCreate"
            Public fSyncLastUpd As System.String = "SyncLastUpd"
            Public fContCategoryDesc As System.String = "ContCategoryDesc"
            Public fContTypeDesc As System.String = "ContTypeDesc"
            Public fCompletedDate As System.String = "CompletedDate"

            Protected _ContractNo As System.String
            Private _ContType As System.String
            Private _ContCategory As System.String
            Private _InquiryDate As System.DateTime
            Private _AgreeDate As System.DateTime
            Private _ValidityStart As System.DateTime
            Private _ValidityEnd As System.DateTime
            Private _FileNo As System.String
            Private _SchemeNo As System.String
            Private _RefNo As System.String
            Private _CompanyID As System.String
            Private _LocID As System.String
            Private _Country As System.String
            Private _State As System.String
            Private _City As System.String
            Private _Area As System.String
            Private _Remark1 As System.String
            Private _Remark2 As System.String
            Private _Remark3 As System.String
            Private _Remark4 As System.String
            Private _Remark5 As System.String
            Private _AuthID As System.String
            Private _AuthPOS As System.String
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _ApproveDate As System.DateTime
            Private _ApproveBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _Active As System.Byte
            Private _Inuse As System.Byte
            Private _flag As System.Byte
            Private _Rowguid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _CompletedDate As System.DateTime

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ContractNo As System.String
                Get
                    Return _ContractNo
                End Get
                Set(ByVal Value As System.String)
                    _ContractNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ContType As System.String
                Get
                    Return _ContType
                End Get
                Set(ByVal Value As System.String)
                    _ContType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ContCategory As System.String
                Get
                    Return _ContCategory
                End Get
                Set(ByVal Value As System.String)
                    _ContCategory = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property InquiryDate As System.DateTime
                Get
                    Return _InquiryDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _InquiryDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property AgreeDate As System.DateTime
                Get
                    Return _AgreeDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _AgreeDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CompletedDate As System.DateTime
                Get
                    Return _CompletedDate
                End Get
                Set(ByVal Value As System.DateTime)
                    _CompletedDate = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ValidityStart As System.DateTime
                Get
                    Return _ValidityStart
                End Get
                Set(ByVal Value As System.DateTime)
                    _ValidityStart = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ValidityEnd As System.DateTime
                Get
                    Return _ValidityEnd
                End Get
                Set(ByVal Value As System.DateTime)
                    _ValidityEnd = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property FileNo As System.String
                Get
                    Return _FileNo
                End Get
                Set(ByVal Value As System.String)
                    _FileNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property SchemeNo As System.String
                Get
                    Return _SchemeNo
                End Get
                Set(ByVal Value As System.String)
                    _SchemeNo = Value
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
            Public Property Remark1 As System.String
                Get
                    Return _Remark1
                End Get
                Set(ByVal Value As System.String)
                    _Remark1 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Remark2 As System.String
                Get
                    Return _Remark2
                End Get
                Set(ByVal Value As System.String)
                    _Remark2 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Remark3 As System.String
                Get
                    Return _Remark3
                End Get
                Set(ByVal Value As System.String)
                    _Remark3 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Remark4 As System.String
                Get
                    Return _Remark4
                End Get
                Set(ByVal Value As System.String)
                    _Remark4 = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property Remark5 As System.String
                Get
                    Return _Remark5
                End Get
                Set(ByVal Value As System.String)
                    _Remark5 = Value
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
            Public Property Rowguid As System.Guid
                Get
                    Return _Rowguid
                End Get
                Set(ByVal Value As System.Guid)
                    _Rowguid = Value
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
                    Return _flag
                End Get
                Set(ByVal Value As System.Byte)
                    _flag = Value
                End Set
            End Property


        End Class
#End Region

    End Namespace

#Region "Class Info"
    Public Class ContractInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "ContractNo,ContType,(SELECT CodeDesc FROM CODEMASTER WITH (NOLOCK) WHERE codetype='LTY' AND Code=LICENSE.ContType) AS ContTypeDesc,ContCategory,(SELECT LicenseDesc FROM LICENSECATEGORY WITH (NOLOCK) WHERE LicenseCode=LICENSE.ContCategory) AS ContCategoryDesc,InquiryDate,CompletedDate,ValidityStart,ValidityEnd,FileNo,SchemeNo,RefNo,CompanyID,LocID,Country,State,City,Area,Remark1,Remark2,Remark3,Remark4,Remark5,AuthID,AuthPOS,CreateDate,CreateBy,ApproveDate,ApproveBy,LastUpdate,UpdateBy,Active,Inuse,Flag,Rowguid,SyncCreate,SyncLastUpd"
                .CheckFields = "Active,Inuse,Flag"
                .TableName = "LICENSE WITH (NOLOCK)"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "ContractNo,ContType,(SELECT CodeDesc FROM CODEMASTER WITH (NOLOCK) WHERE codetype='LTY' AND Code=LICENSE.ContType) AS ContTypeDesc,ContCategory,(SELECT LicenseDesc FROM LICENSECATEGORY WITH (NOLOCK) WHERE LicenseCode=LICENSE.ContCategory) AS ContCategoryDesc,InquiryDate,CompletedDate,ValidityStart,ValidityEnd,FileNo,SchemeNo,RefNo,CompanyID,LocID,Country,State,City,Area,Remark1,Remark2,Remark3,Remark4,Remark5,AuthID,AuthPOS,CreateDate,CreateBy,ApproveDate,ApproveBy,LastUpdate,UpdateBy,Active,Inuse,Flag,Rowguid,SyncCreate,SyncLastUpd"
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
    Public Class LicenseScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ContractNo"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ContType"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ContCategory"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "InquiryDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "AgreeDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "ValidityStart"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "ValidityEnd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "FileNo"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "SchemeNo"
                .Length = 50
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
                .FieldName = "CompanyID"
                .Length = 10
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
                .FieldName = "State"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "City"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Area"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Remark1"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Remark2"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Remark3"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Remark4"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Remark5"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "AuthID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(20, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "AuthPOS"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(21, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(22, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(23, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "ApproveDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(24, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ApproveBy"
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
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(28, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Inuse"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(29, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(30, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(31, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(32, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(33, this)

        End Sub

        Public ReadOnly Property ContractNo As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property ContType As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property ContCategory As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property InquiryDate As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property AgreeDate As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property ValidityStart As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property ValidityEnd As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property FileNo As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property SchemeNo As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property RefNo As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property CompanyID As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property Country As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property State As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property City As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property Area As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property Remark1 As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property Remark2 As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property Remark3 As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property Remark4 As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property Remark5 As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property AuthID As StrucElement
            Get
                Return MyBase.GetItem(20)
            End Get
        End Property
        Public ReadOnly Property AuthPOS As StrucElement
            Get
                Return MyBase.GetItem(21)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(22)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(23)
            End Get
        End Property
        Public ReadOnly Property ApproveDate As StrucElement
            Get
                Return MyBase.GetItem(24)
            End Get
        End Property
        Public ReadOnly Property ApproveBy As StrucElement
            Get
                Return MyBase.GetItem(25)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(26)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(27)
            End Get
        End Property
        Public ReadOnly Property Inuse As StrucElement
            Get
                Return MyBase.GetItem(28)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(29)
            End Get
        End Property
        Public ReadOnly Property Rowguid As StrucElement
            Get
                Return MyBase.GetItem(30)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(31)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
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
