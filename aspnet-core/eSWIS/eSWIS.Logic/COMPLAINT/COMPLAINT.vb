Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared

Namespace COMPLAINTS
#Region "COMPLAINT Class"
    Public NotInheritable Class COMPLAINT
        Inherits Core.CoreControl
        Private ComplaintInfo As ComplaintInfo = New ComplaintInfo

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"

        Private Function Save(ByVal ComplaintCont As Container.Complaint, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If ComplaintCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ComplaintInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "ComplainID = '" & ComplaintCont.ComplainID & "'")
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
                                .TableName = "Complaint"
                                .AddField("DOEFileNo", ComplaintCont.DOEFileNo, SQLControl.EnumDataType.dtStringN)
                                .AddField("ComplainType", ComplaintCont.ComplainType, SQLControl.EnumDataType.dtStringN)
                                .AddField("FirstName", ComplaintCont.FirstName, SQLControl.EnumDataType.dtStringN)
                                .AddField("LastName", ComplaintCont.LastName, SQLControl.EnumDataType.dtStringN)
                                .AddField("BranchName", ComplaintCont.BranchName, SQLControl.EnumDataType.dtStringN)
                                .AddField("Country", ComplaintCont.Country, SQLControl.EnumDataType.dtString)
                                .AddField("State", ComplaintCont.State, SQLControl.EnumDataType.dtString)
                                .AddField("PBT", ComplaintCont.PBT, SQLControl.EnumDataType.dtString)
                                .AddField("City", ComplaintCont.City, SQLControl.EnumDataType.dtString)
                                .AddField("Area", ComplaintCont.Area, SQLControl.EnumDataType.dtString)
                                .AddField("Address", ComplaintCont.Address, SQLControl.EnumDataType.dtString)
                                .AddField("PostalCode", ComplaintCont.PostalCode, SQLControl.EnumDataType.dtString)
                                .AddField("TelNo", ComplaintCont.TelNo, SQLControl.EnumDataType.dtString)
                                .AddField("FaxNo", ComplaintCont.FaxNo, SQLControl.EnumDataType.dtString)
                                .AddField("Email", ComplaintCont.Email, SQLControl.EnumDataType.dtStringN)
                                .AddField("ComplainDetails", ComplaintCont.ComplainDetails, SQLControl.EnumDataType.dtStringN)
                                .AddField("Remark", ComplaintCont.Remark, SQLControl.EnumDataType.dtStringN)
                                .AddField("Status", ComplaintCont.Status, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", ComplaintCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", ComplaintCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", ComplaintCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", ComplaintCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("SyncCreate", ComplaintCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("SyncLastUpdate", ComplaintCont.SyncLastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("IsHost", ComplaintCont.IsHost, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LastSyncBy", ComplaintCont.LastSyncBy, SQLControl.EnumDataType.dtString)

                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ComplainID = '" & ComplaintCont.ComplainID & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("ComplainID", ComplaintCont.ComplainID, SQLControl.EnumDataType.dtString)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "ComplainID = '" & ComplaintCont.ComplainID & "'")
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
                ComplaintCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal ComplaintCont As Container.Complaint, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(ComplaintCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
        End Function

        'AMEND
        Public Function Update(ByVal ComplaintCont As Container.Complaint, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(ComplaintCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        Public Function Delete(ByVal ComplaintCont As Container.Complaint, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If ComplaintCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With ComplaintInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "ComplainID = '" & ComplaintCont.ComplainID & "'")
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
                                strSQL = BuildUpdate(ComplaintInfo.MyInfo.TableName, " SET Flag = 0" &
                                " , LastUpdate = '" & ComplaintCont.LastUpdate & "' , UpdateBy = '" &
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, ComplaintCont.UpdateBy) & "' WHERE" &
                                "ComplainID = '" & ComplaintCont.ComplainID & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(ComplaintInfo.MyInfo.TableName, "ComplainID = '" & ComplaintCont.ComplainID & "'")
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
                ComplaintCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetCOMPLAINT(ByVal ComplainID As System.String) As Container.Complaint
            Dim rComplaint As Container.Complaint = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    With ComplaintInfo.MyInfo
                        strSQL = BuildSelect(.FieldsList, .TableName, "ComplainID = '" & ComplainID & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rComplaint = New Container.Complaint
                                rComplaint.ComplainID = drRow.Item("ComplainID")
                                rComplaint.DOEFileNo = drRow.Item("DOEFileNo")
                                rComplaint.ComplainType = drRow.Item("ComplainType")
                                rComplaint.FirstName = drRow.Item("FirstName")
                                rComplaint.LastName = drRow.Item("LastName")
                                rComplaint.BranchName = drRow.Item("BranchName")
                                rComplaint.Country = drRow.Item("Country")
                                rComplaint.State = drRow.Item("State")
                                rComplaint.PBT = drRow.Item("PBT")
                                rComplaint.City = drRow.Item("City")
                                rComplaint.Area = drRow.Item("Area")
                                rComplaint.Address = drRow.Item("Address")
                                rComplaint.PostalCode = drRow.Item("PostalCode")
                                rComplaint.TelNo = drRow.Item("TelNo")
                                rComplaint.FaxNo = drRow.Item("FaxNo")
                                rComplaint.Email = drRow.Item("Email")
                                rComplaint.ComplainDetails = drRow.Item("ComplainDetails")
                                rComplaint.Remark = drRow.Item("Remark")
                                rComplaint.Status = drRow.Item("Status")
                                If Not IsDBNull(drRow.Item("CreateDate")) Then
                                    rComplaint.CreateDate = drRow.Item("CreateDate")
                                End If
                                rComplaint.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rComplaint.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rComplaint.UpdateBy = drRow.Item("UpdateBy")
                                rComplaint.rowguid = drRow.Item("rowguid")
                                rComplaint.SyncCreate = drRow.Item("SyncCreate")
                                rComplaint.SyncLastUpdate = drRow.Item("SyncLastUpdate")
                                rComplaint.IsHost = drRow.Item("IsHost")
                                rComplaint.LastSyncBy = drRow.Item("LastSyncBy")
                            Else
                                rComplaint = Nothing
                            End If
                        Else
                            rComplaint = Nothing
                        End If
                    End With
                End If
                Return rComplaint
            Catch ex As Exception
                Throw ex
            Finally
                rComplaint = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetCOMPLAINT(ByVal ComplainID As System.String, DecendingOrder As Boolean) As List(Of Container.Complaint)
            Dim rComplaint As Container.Complaint = Nothing
            Dim lstComplaint As List(Of Container.Complaint) = New List(Of Container.Complaint)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    With ComplaintInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by ComplainID DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "ComplainID = '" & ComplainID & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rComplaint = New Container.Complaint
                                rComplaint.ComplainID = drRow.Item("ComplainID")
                                rComplaint.DOEFileNo = drRow.Item("DOEFileNo")
                                rComplaint.ComplainType = drRow.Item("ComplainType")
                                rComplaint.FirstName = drRow.Item("FirstName")
                                rComplaint.LastName = drRow.Item("LastName")
                                rComplaint.BranchName = drRow.Item("BranchName")
                                rComplaint.Country = drRow.Item("Country")
                                rComplaint.State = drRow.Item("State")
                                rComplaint.PBT = drRow.Item("PBT")
                                rComplaint.City = drRow.Item("City")
                                rComplaint.Area = drRow.Item("Area")
                                rComplaint.Address = drRow.Item("Address")
                                rComplaint.PostalCode = drRow.Item("PostalCode")
                                rComplaint.TelNo = drRow.Item("TelNo")
                                rComplaint.FaxNo = drRow.Item("FaxNo")
                                rComplaint.Email = drRow.Item("Email")
                                rComplaint.ComplainDetails = drRow.Item("ComplainDetails")
                                rComplaint.Remark = drRow.Item("Remark")
                                rComplaint.Status = drRow.Item("Status")
                                rComplaint.CreateBy = drRow.Item("CreateBy")
                                rComplaint.UpdateBy = drRow.Item("UpdateBy")
                                rComplaint.rowguid = drRow.Item("rowguid")
                                rComplaint.SyncCreate = drRow.Item("SyncCreate")
                                rComplaint.SyncLastUpdate = drRow.Item("SyncLastUpdate")
                                rComplaint.IsHost = drRow.Item("IsHost")
                                rComplaint.LastSyncBy = drRow.Item("LastSyncBy")
                                lstComplaint.Add(rComplaint)
                            Next

                        Else
                            rComplaint = Nothing
                        End If
                        Return lstComplaint
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rComplaint = Nothing
                lstComplaint = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

#End Region
    End Class
#End Region

#Region "Container"
    Namespace Container
#Region "Complaint Container"
        Public Class Complaint_FieldName
            Public ComplainID As System.String = "ComplainID"
            Public DOEFileNo As System.String = "DOEFileNo"
            Public ComplainType As System.String = "ComplainType"
            Public FirstName As System.String = "FirstName"
            Public LastName As System.String = "LastName"
            Public BranchName As System.String = "BranchName"
            Public Country As System.String = "Country"
            Public State As System.String = "State"
            Public PBT As System.String = "PBT"
            Public City As System.String = "City"
            Public Area As System.String = "Area"
            Public Address As System.String = "Address"
            Public PostalCode As System.String = "PostalCode"
            Public TelNo As System.String = "TelNo"
            Public FaxNo As System.String = "FaxNo"
            Public Email As System.String = "Email"
            Public ComplainDetails As System.String = "ComplainDetails"
            Public Remark As System.String = "Remark"
            Public Status As System.String = "Status"
            Public CreateDate As System.String = "CreateDate"
            Public CreateBy As System.String = "CreateBy"
            Public LastUpdate As System.String = "LastUpdate"
            Public UpdateBy As System.String = "UpdateBy"
            Public rowguid As System.String = "rowguid"
            Public SyncCreate As System.String = "SyncCreate"
            Public SyncLastUpdate As System.String = "SyncLastUpdate"
            Public IsHost As System.String = "IsHost"
            Public LastSyncBy As System.String = "LastSyncBy"
        End Class

        Public Class Complaint
            Protected _ComplainID As System.String
            Private _DOEFileNo As System.String
            Private _ComplainType As System.String
            Private _FirstName As System.String
            Private _LastName As System.String
            Private _BranchName As System.String
            Private _Country As System.String
            Private _State As System.String
            Private _PBT As System.String
            Private _City As System.String
            Private _Area As System.String
            Private _Address As System.String
            Private _PostalCode As System.String
            Private _TelNo As System.String
            Private _FaxNo As System.String
            Private _Email As System.String
            Private _ComplainDetails As System.String
            Private _Remark As System.String
            Private _Status As System.Byte
            Private _CreateDate As System.DateTime
            Private _CreateBy As System.String
            Private _LastUpdate As System.DateTime
            Private _UpdateBy As System.String
            Private _rowguid As System.Guid
            Private _SyncCreate As System.DateTime
            Private _SyncLastUpdate As System.DateTime
            Private _IsHost As System.Byte
            Private _LastSyncBy As System.String

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property ComplainID As System.String
                Get
                    Return _ComplainID
                End Get
                Set(ByVal Value As System.String)
                    _ComplainID = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property DOEFileNo As System.String
                Get
                    Return _DOEFileNo
                End Get
                Set(ByVal Value As System.String)
                    _DOEFileNo = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property ComplainType As System.String
                Get
                    Return _ComplainType
                End Get
                Set(ByVal Value As System.String)
                    _ComplainType = Value
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
            Public Property LastName As System.String
                Get
                    Return _LastName
                End Get
                Set(ByVal Value As System.String)
                    _LastName = Value
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
            Public Property Address As System.String
                Get
                    Return _Address
                End Get
                Set(ByVal Value As System.String)
                    _Address = Value
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
            Public Property ComplainDetails As System.String
                Get
                    Return _ComplainDetails
                End Get
                Set(ByVal Value As System.String)
                    _ComplainDetails = Value
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
            Public Property SyncLastUpdate As System.DateTime
                Get
                    Return _SyncLastUpdate
                End Get
                Set(ByVal Value As System.DateTime)
                    _SyncLastUpdate = Value
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
#Region "Complaint Info"
    Public Class ComplaintInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "ComplainID,DOEFileNo,ComplainType,FirstName,LastName,BranchName,Country,State,PBT,City,Area,Address,PostalCode,TelNo,FaxNo,Email,ComplainDetails,Remark,Status,CreateDate,CreateBy,LastUpdate,UpdateBy,rowguid,SyncCreate,SyncLastUpdate,IsHost,LastSyncBy"
                .CheckFields = "ComplainType,Status,IsHost"
                .TableName = "Complaint"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "ComplainID,DOEFileNo,ComplainType,FirstName,LastName,BranchName,Country,State,PBT,City,Area,Address,PostalCode,TelNo,FaxNo,Email,ComplainDetails,Remark,Status,CreateDate,CreateBy,LastUpdate,UpdateBy,rowguid,SyncCreate,SyncLastUpdate,IsHost,LastSyncBy"
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
#Region "COMPLAINT Scheme"
    Public Class COMPLAINTScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "ComplainID"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "DOEFileNo"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "ComplainType"
                .Length = 1
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
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "LastName"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "BranchName"
                .Length = 200
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Country"
                .Length = 2
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "State"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "PBT"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "City"
                .Length = 5
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Area"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "Address"
                .Length = 40
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "PostalCode"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "TelNo"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "FaxNo"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Email"
                .Length = 80
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "ComplainDetails"
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "Remark"
                .Length = 4000
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(17, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Status"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(18, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(19, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
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
                .IsMandatory = False
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
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(23, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(24, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(25, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsHost"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(26, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .IsMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(27, this)

        End Sub

        Public ReadOnly Property ComplainID As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property

        Public ReadOnly Property DOEFileNo As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property
        Public ReadOnly Property ComplainType As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property FirstName As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property LastName As StrucElement
            Get
                Return MyBase.GetItem(4)
            End Get
        End Property
        Public ReadOnly Property BranchName As StrucElement
            Get
                Return MyBase.GetItem(5)
            End Get
        End Property
        Public ReadOnly Property Country As StrucElement
            Get
                Return MyBase.GetItem(6)
            End Get
        End Property
        Public ReadOnly Property State As StrucElement
            Get
                Return MyBase.GetItem(7)
            End Get
        End Property
        Public ReadOnly Property PBT As StrucElement
            Get
                Return MyBase.GetItem(8)
            End Get
        End Property
        Public ReadOnly Property City As StrucElement
            Get
                Return MyBase.GetItem(9)
            End Get
        End Property
        Public ReadOnly Property Area As StrucElement
            Get
                Return MyBase.GetItem(10)
            End Get
        End Property
        Public ReadOnly Property Address As StrucElement
            Get
                Return MyBase.GetItem(11)
            End Get
        End Property
        Public ReadOnly Property PostalCode As StrucElement
            Get
                Return MyBase.GetItem(12)
            End Get
        End Property
        Public ReadOnly Property TelNo As StrucElement
            Get
                Return MyBase.GetItem(13)
            End Get
        End Property
        Public ReadOnly Property FaxNo As StrucElement
            Get
                Return MyBase.GetItem(14)
            End Get
        End Property
        Public ReadOnly Property Email As StrucElement
            Get
                Return MyBase.GetItem(15)
            End Get
        End Property
        Public ReadOnly Property ComplainDetails As StrucElement
            Get
                Return MyBase.GetItem(16)
            End Get
        End Property
        Public ReadOnly Property Remark As StrucElement
            Get
                Return MyBase.GetItem(17)
            End Get
        End Property
        Public ReadOnly Property Status As StrucElement
            Get
                Return MyBase.GetItem(18)
            End Get
        End Property
        Public ReadOnly Property CreateDate As StrucElement
            Get
                Return MyBase.GetItem(19)
            End Get
        End Property
        Public ReadOnly Property CreateBy As StrucElement
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
        Public ReadOnly Property rowguid As StrucElement
            Get
                Return MyBase.GetItem(23)
            End Get
        End Property
        Public ReadOnly Property SyncCreate As StrucElement
            Get
                Return MyBase.GetItem(24)
            End Get
        End Property
        Public ReadOnly Property SyncLastUpdate As StrucElement
            Get
                Return MyBase.GetItem(25)
            End Get
        End Property
        Public ReadOnly Property IsHost As StrucElement
            Get
                Return MyBase.GetItem(26)
            End Get
        End Property
        Public ReadOnly Property LastSyncBy As StrucElement
            Get
                Return MyBase.GetItem(27)
            End Get
        End Property

        Public Function GetElement(ByVal Key As Integer) As StrucElement
            Return MyBase.GetItem(Key)
        End Function
    End Class
#End Region
#End Region

End Namespace