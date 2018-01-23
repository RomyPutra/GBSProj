Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared

Namespace Profiles
#Region "SIC Class"
    Public NotInheritable Class SIC
        Inherits Core.CoreControl
        Private SicInfo As SicInfo = New SicInfo

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal SicCont As Container.Sic, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If SicCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With SicInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "SICCode = '" & SicCont.SICCode & "'")
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
                                .TableName = "Sic"
                                .AddField("SICDesc", SicCont.SICDesc, SQLControl.EnumDataType.dtStringN)
                                .AddField("SICDescEng", SicCont.SICDescEng, SQLControl.EnumDataType.dtStringN)
                                .AddField("SICType", SicCont.SICType, SQLControl.EnumDataType.dtString)
                                .AddField("CapacityLevel", SicCont.CapacityLevel, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", SicCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", SicCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", SicCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", SicCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", SicCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", SicCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("rowguid", SicCont.rowguid, SQLControl.EnumDataType.dtString)
                                .AddField("SyncCreate", SicCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("SyncLastUpd", SicCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                                .AddField("IsHost", SicCont.IsHost, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LastSyncBy", SicCont.LastSyncBy, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "SICCode = '" & SicCont.SICCode & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("SICCode", SicCont.SICCode, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "SICCode = '" & SicCont.SICCode & "'")
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
                SicCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Private Function SaveList(ByVal ListSIC As List(Of Container.Sic), ByRef message As String) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Dim ListSQL As ArrayList = New ArrayList()
            SaveList = False
            Try
                If ListSIC Is Nothing Then
                    'Message return
                Else
                    StartSQLControl()
                    For Each SICCont In ListSIC
                        With objSQL
                            strSQL = BuildUpdate(SicInfo.MyInfo.TableName, " SET CapacityLevel = " & SICCont.CapacityLevel & _
                            " , LastUpdate = '" & SICCont.LastUpdate & "' , UpdateBy = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, SICCont.UpdateBy) & "', LastSyncBy = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, SICCont.LastSyncBy) & "'" & _
                            " WHERE " & _
                            "SICCode = '" & SICCont.SICCode & "'")
                        End With
                        ListSQL.Add(strSQL)
                    Next
                End If

                Try
                    objConn.BatchExecute(ListSQL, DataAccess.EnumRtnType.rtNone, CommandType.Text)
                    Return True
                Catch exExecute As Exception
                    message = exExecute.Message.ToString()
                    Return False
                End Try

            Catch axExecute As Exception
                message = axExecute.Message.ToString()
                Throw New ApplicationException("210004 " & axExecute.Message.ToString())

            Finally
                ListSIC = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal SicCont As Container.Sic, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(SicCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
        End Function

        'AMEND
        Public Function Update(ByVal SicCont As Container.Sic, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(SicCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        'AMEND LIST
        Public Function UpdateList(ByVal ListSIC As List(Of Container.Sic), ByRef message As String) As Boolean
            Return SaveList(ListSIC, message)
        End Function

        Public Function Delete(ByVal SicCont As Container.Sic, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If SicCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With SicInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "SICCode = '" & SicCont.SICCode & "'")
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
                                strSQL = BuildUpdate(SicInfo.MyInfo.TableName, " SET Flag = 0" & _
                                " , LastUpdate = '" & SicCont.LastUpdate & "' , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, SicCont.UpdateBy) & "' WHERE" & _
                                "SICCode = '" & SicCont.SICCode & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(SicInfo.MyInfo.TableName, "SICCode = '" & SicCont.SICCode & "'")
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
                SicCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetSIC(ByVal SICCode As System.String) As Container.Sic
            Dim rSic As Container.Sic = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With SicInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "SICCode = '" & SICCode & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rSic = New Container.Sic
                                rSic.SICCode = drRow.Item("SICCode")
                                rSic.SICDesc = drRow.Item("SICDesc")
                                rSic.SICDescEng = drRow.Item("SICDescEng")
                                rSic.SICType = drRow.Item("SICType")
                                rSic.CapacityLevel = drRow.Item("CapacityLevel")
                                If Not IsDBNull(drRow.Item("CreateDate")) Then
                                    rSic.CreateDate = drRow.Item("CreateDate")
                                End If
                                rSic.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rSic.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rSic.UpdateBy = drRow.Item("UpdateBy")
                                rSic.Active = drRow.Item("Active")
                                rSic.Inuse = drRow.Item("Inuse")
                                rSic.rowguid = drRow.Item("rowguid")
                                rSic.SyncCreate = drRow.Item("SyncCreate")
                                rSic.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rSic.IsHost = drRow.Item("IsHost")
                                rSic.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rSic = Nothing
                            End If
                        Else
                            rSic = Nothing
                        End If
                    End With
                End If
                Return rSic
            Catch ex As Exception
                Throw ex
            Finally
                rSic = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetSIC(ByVal SICCode As System.String, DecendingOrder As Boolean) As List(Of Container.Sic)
            Dim rSic As Container.Sic = Nothing
            Dim lstSic As List(Of Container.Sic) = New List(Of Container.Sic)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With SicInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by SICCode DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "SICCode = '" & SICCode & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rSic = New Container.Sic
                                rSic.SICCode = drRow.Item("SICCode")
                                rSic.SICDesc = drRow.Item("SICDesc")
                                rSic.SICDescEng = drRow.Item("SICDescEng")
                                rSic.SICType = drRow.Item("SICType")
                                rSic.CapacityLevel = drRow.Item("CapacityLevel")
                                rSic.CreateBy = drRow.Item("CreateBy")
                                rSic.UpdateBy = drRow.Item("UpdateBy")
                                rSic.Active = drRow.Item("Active")
                                rSic.Inuse = drRow.Item("Inuse")
                                rSic.rowguid = drRow.Item("rowguid")
                                rSic.SyncCreate = drRow.Item("SyncCreate")
                                rSic.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rSic.IsHost = drRow.Item("IsHost")
                                rSic.LastSyncBy = drRow.Item("LastSyncBy")
                                lstSic.Add(rSic)
                            Next

                        Else
                            rSic = Nothing
                        End If
                        Return lstSic
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rSic = Nothing
                lstSic = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetSICByLocID(ByVal LocID As String, ByVal GenLocID As String, ByVal WasteCode As String, Optional ByVal WasteType As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With SicInfo.MyInfo
                    StartSQLControl()
                    strSQL = "SELECT"

                    If WasteType IsNot Nothing AndAlso WasteType <> "" Then
                        strSQL &= " sum(LI.Qty) - Orange.ExpectedQty " & _
                        " AS Capacity,"
                    End If

                   strSQL &= " sum(LI.Qty) - Green.ExpectedQty AS BalanceQty, sum(LI.Qty) - Orange.ExpectedQty AS QuotaQty, sum(LI.Qty) AS CapacityLevel" & _
                            " FROM BIZLOCATE B INNER JOIN LICENSE LS WITH(NOLOCK) ON B.BizLocID = LS.LocID AND B.BizRegID =  LS.CompanyID INNER JOIN " & _
                            " LICENSEITEM LI WITH(NOLOCK) ON LS.ContractNo = LI.ContractNo /*LEFT JOIN TWG_SUBMISSIONHDR c ON  b.BizLocID = B.BizLocID INNER" & _
                            " JOIN TWG_SUBMISSIONDTL d ON d.SubmissionID = c.SubmissionID AND LI.ItemCode=li.ItemCode*/ " & _
                            " OUTER APPLY " & _
                            " (SELECT ISNULL(SUM(DT.ExpectedQty),0) AS ExpectedQty FROM TWG_SUBMISSIONHDR HD LEFT JOIN TWG_SUBMISSIONDTL DT ON HD.SubmissionID = DT.SubmissionID" & _
                            " WHERE HD.ReceiverLocID = b.BizLocID AND HD.Status = 3 AND ((HD.StatusTWG = 1 AND HD.SubStatus = 0) OR (HD.StatusTWG IN (0,1) AND HD.SubStatus = 1) OR (HD.StatusTWG = 2 AND HD.SubStatus = 1)) AND DT.WasteCode = li.ItemCode) Green" & _
                            " OUTER APPLY " & _
                            " (SELECT ISNULL(SUM(DT.ExpectedQty),0) AS ExpectedQty FROM TWG_SUBMISSIONHDR HD LEFT JOIN TWG_SUBMISSIONDTL DT ON HD.SubmissionID = DT.SubmissionID " & _
                            " WHERE HD.ReceiverLocID = b.BizLocID AND HD.Status IN (1,3) AND ((HD.StatusTWG IN (0,1,2) AND HD.SubStatus IN (0,1))) AND DT.WasteCode = li.ItemCode) Orange " & _
                            " WHERE li.ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteCode) & "' AND b.BizLocID='" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "'" & _
                            " GROUP BY b.BizLocID, Green.ExpectedQty, Orange.ExpectedQty"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
            EndSQLControl()
        End Function

        Public Overloads Function GetSICByItem(ByVal LocID As String, ByVal WasteCode As String) As Data.DataTable
            If StartConnection() = True Then
                With SicInfo.MyInfo
                    StartSQLControl()
                    strSQL = "SELECT Sum(LI.Qty) CapacityLevel, Sum(ISNULL(Green.ExpectedQty,0)) BalanceQty FROM BIZLOCATE BL WITH(NOLOCK) " & _
                        "LEFT JOIN BIZENTITY BE WITH(NOLOCK) ON BL.Bizregid = BE.Bizregid LEFT JOIN CITY C ON BL.City = C.CityCode " & _
                        "LEFT JOIN State S ON BL.State = S.StateCode LEFT JOIN COUNTRY CN ON BL.Country = CN.CountryCode INNER JOIN " & _
                        "LICENSE LS WITH(NOLOCK) ON BL.BizLocID = LS.LocID AND BL.BizRegID = LS.CompanyID  INNER JOIN LICENSEITEM LI " & _
                        "WITH(NOLOCK) ON LS.ContractNo = LI.ContractNo " & _
                        "OUTER APPLY " & _
                        "(SELECT ISNULL(SUM(DT.ExpectedQty),0) AS ExpectedQty FROM TWG_SUBMISSIONHDR HD LEFT JOIN TWG_SUBMISSIONDTL DT " & _
                        "ON HD.SubmissionID = DT.SubmissionID WHERE HD.ReceiverID = BL.BizLocID AND HD.Status = 3 AND " & _
                        "((HD.StatusTWG = 1 AND HD.SubStatus = 0) OR (HD.StatusTWG IN (0,1) AND HD.SubStatus = 1) OR (HD.StatusTWG = 2 AND HD.SubStatus = 1)) " & _
                        "AND DT.WasteCode = LI.ItemCode) Green " & _
                        "WHERE BL.BizLocID = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, LocID) & "' AND LI.ItemCode = '" & objSQL.ParseValue(SEAL.Data.SQLControl.EnumDataType.dtString, WasteCode) & "'"

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
#Region "Sic Container"
        Public Class Sic_FieldName
            Public SICCode As System.String = "SICCode"
            Public SICDesc As System.String = "SICDesc"
            Public SICDescEng As System.String = "SICDescEng"
            Public SICType As System.String = "SICType"
            Public CapacityLevel As System.String = "CapacityLevel"
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
            Public IsHost As System.String = "IsHost"
            Public LastSyncBy As System.String = "LastSyncBy"
        End Class

        Public Class Sic
            Protected _SICCode As System.String
            Private _SICDesc As System.String
            Private _SICDescEng As System.String
            Private _SICType As System.String
            Private _CapacityLevel As System.Decimal
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _Active As System.Byte
            Private _Inuse As System.Byte
            Private _rowguid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpd As System.DateTime
            Private _IsHost As System.Byte
            Private _LastSyncBy As System.String

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property SICCode As System.String
                Get
                    Return _SICCode
                End Get
                Set(ByVal Value As System.String)
                    _SICCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property SICDesc As System.String
                Get
                    Return _SICDesc
                End Get
                Set(ByVal Value As System.String)
                    _SICDesc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property SICDescEng As System.String
                Get
                    Return _SICDescEng
                End Get
                Set(ByVal Value As System.String)
                    _SICDescEng = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property SICType As System.String
                Get
                    Return _SICType
                End Get
                Set(ByVal Value As System.String)
                    _SICType = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CapacityLevel As System.Decimal
                Get
                    Return _CapacityLevel
                End Get
                Set(ByVal Value As System.Decimal)
                    _CapacityLevel = Value
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
#End Region

#Region "Class Info"
#Region "Sic Info"
    Public Class SicInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "SICCode,SICDesc,SICDescEng,SICType,CapacityLevel,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
                .CheckFields = "Active,Inuse,Flag,IsHost"
                .TableName = "Sic"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "SICCode,SICDesc,SICDescEng,SICType,CapacityLevel,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
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
#Region "SIC Scheme"
    Public Class SICScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "SICCode"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "SICDesc"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "SICDescEng"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "SICType"
                .Length = 3
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "CapacityLevel"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Inuse"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsHost"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)

        End Sub

        Public ReadOnly Property SICCode As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property SICDesc As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property SICDescEng As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property SICType As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property CapacityLevel As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property LastUpdate As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property UpdateBy As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property Active As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property Inuse As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property Flag As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpd As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property IsHost As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region
#End Region

End Namespace