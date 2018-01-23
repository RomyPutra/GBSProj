
Imports SEAL.Data
Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports eSWIS.Shared

Namespace Actions
#Region "WAC_UNIVCOMP Class"
    Public NotInheritable Class WAC_UNIVCOMP
        Inherits Core.CoreControl
        Private Wac_univcompInfo As Wac_univcompInfo = New Wac_univcompInfo

        Public Sub New(ByVal Conn As String)
            ConnectionString = Conn
            ConnectionSetup()
        End Sub

#Region "Data Manipulation-Add,Edit,Del"
        Private Function Save(ByVal Wac_univcompCont As Container.Wac_univcomp, ByVal pType As SQLControl.EnumSQLType, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Dim strSQL As String = ""
            Dim blnExec As Boolean, blnFound As Boolean, blnFlag As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Save = False
            Try
                If Wac_univcompCont Is Nothing Then
                    'Message return
                Else
                    blnExec = False
                    blnFound = False
                    blnFlag = False
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Wac_univcompInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "COMPCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_univcompCont.COMPCode) & "' AND DEFUOM = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_univcompCont.DEFUOM) & "'")
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
                                .TableName = "Wac_univcomp"
                                .AddField("COMPDesc", Wac_univcompCont.COMPDesc, SQLControl.EnumDataType.dtStringN)
                                .AddField("CPMIN", Wac_univcompCont.CPMIN, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CPMAX", Wac_univcompCont.CPMAX, SQLControl.EnumDataType.dtNumeric)
                                .AddField("CreateDate", Wac_univcompCont.CreateDate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("CreateBy", Wac_univcompCont.CreateBy, SQLControl.EnumDataType.dtString)
                                .AddField("LastUpdate", Wac_univcompCont.LastUpdate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("UpdateBy", Wac_univcompCont.UpdateBy, SQLControl.EnumDataType.dtString)
                                .AddField("Active", Wac_univcompCont.Active, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Inuse", Wac_univcompCont.Inuse, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Flag", Wac_univcompCont.flag, SQLControl.EnumDataType.dtNumeric)
                                .AddField("SyncCreate", Wac_univcompCont.SyncCreate, SQLControl.EnumDataType.dtDateTime)
                                .AddField("SyncLastUpd", Wac_univcompCont.SyncLastUpd, SQLControl.EnumDataType.dtDateTime)
                                .AddField("IsHost", Wac_univcompCont.IsHost, SQLControl.EnumDataType.dtNumeric)
                                .AddField("LastSyncBy", Wac_univcompCont.LastSyncBy, SQLControl.EnumDataType.dtString)
                                .AddField("DEFUOM", Wac_univcompCont.DEFUOM, SQLControl.EnumDataType.dtString)
                                .AddField("AllowDec", Wac_univcompCont.AllowDec, SQLControl.EnumDataType.dtNumeric)
                                .AddField("Precision", Wac_univcompCont.Precision, SQLControl.EnumDataType.dtNumeric)
                                Select Case pType
                                    Case SQLControl.EnumSQLType.stInsert
                                        If blnFound = True And blnFlag = False Then
                                            strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "COMPCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_univcompCont.COMPCode) & "'")
                                        Else
                                            If blnFound = False Then
                                                .AddField("COMPCode", Wac_univcompCont.COMPCode, SQLControl.EnumDataType.dtNumeric)
                                                strSQL = .BuildSQL(SQLControl.EnumSQLType.stInsert)
                                            End If
                                        End If
                                    Case SQLControl.EnumSQLType.stUpdate
                                        strSQL = .BuildSQL(SQLControl.EnumSQLType.stUpdate, .TableName, "COMPCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_univcompCont.COMPCode) & "'")
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
                Wac_univcompCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        'ADD
        Public Function Insert(ByVal Wac_univcompCont As Container.Wac_univcomp, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(Wac_univcompCont, SQLControl.EnumSQLType.stInsert, message, BatchExecute, BatchList, Commit)
        End Function

        'AMEND
        Public Function Update(ByVal Wac_univcompCont As Container.Wac_univcomp, ByRef message As String, Optional ByVal BatchExecute As Boolean = False, Optional ByRef BatchList As ArrayList = Nothing, Optional ByVal Commit As Boolean = False) As Boolean
            Return Save(Wac_univcompCont, SQLControl.EnumSQLType.stUpdate, message, BatchExecute, BatchList, Commit)
        End Function

        Public Function Delete(ByVal Wac_univcompCont As Container.Wac_univcomp, ByRef message As String) As Boolean
            Dim strSQL As String
            Dim blnFound As Boolean
            Dim blnInUse As Boolean
            Dim rdr As System.Data.SqlClient.SqlDataReader
            Delete = False
            blnFound = False
            blnInUse = False
            Try
                If Wac_univcompCont Is Nothing Then
                    'Error Message
                Else
                    If StartConnection(EnumIsoState.StateUpdatetable) = True Then
                        StartSQLControl()
                        With Wac_univcompInfo.MyInfo
                            strSQL = BuildSelect(.CheckFields, .TableName, "COMPCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_univcompCont.COMPCode) & "' AND DEFUOM = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_univcompCont.DEFUOM) & "'")
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
                                strSQL = BuildUpdate(Wac_univcompInfo.MyInfo.TableName, " SET Flag = 0" & _
                                " , LastUpdate = GETDATE() , UpdateBy = '" & _
                                objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_univcompCont.UpdateBy) & "' WHERE" & _
                                "COMPCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_univcompCont.COMPCode) & "' AND DEFUOM = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_univcompCont.DEFUOM) & "'")
                            End With
                        End If

                        If blnFound = True And blnInUse = False Then
                            strSQL = BuildDelete(Wac_univcompInfo.MyInfo.TableName, "COMPCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_univcompCont.COMPCode) & "' AND DEFUOM = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, Wac_univcompCont.DEFUOM) & "'")
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
                Wac_univcompCont = Nothing
                rdr = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function
#End Region

#Region "Data Selection"
        Public Overloads Function GetWAC_UNIVCOMP(ByVal COMPCode As System.Int32) As Container.Wac_univcomp
            Dim rWac_univcomp As Container.Wac_univcomp = Nothing
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing

            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With Wac_univcompInfo.MyInfo
                        strSQL = BuildSelect("unv.*, cc.COMPGroup", "wac_univcomp unv left join wac_compchart cc ON unv.COMPCode = cc.COMPCode", "unv.COMPCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, COMPCode) & "'")
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            If dtTemp.Rows.Count > 0 Then
                                Dim drRow = dtTemp.Rows(0)
                                rWac_univcomp = New Container.Wac_univcomp
                                rWac_univcomp.COMPCode = drRow.Item("COMPCode")
                                rWac_univcomp.DEFUOM = drRow.Item("DEFUOM")
                                rWac_univcomp.COMPDesc = drRow.Item("COMPDesc")
                                rWac_univcomp.CPMIN = drRow.Item("CPMIN")
                                rWac_univcomp.CPMAX = drRow.Item("CPMAX")
                                If Not IsDBNull(drRow.Item("CreateDate")) Then
                                    rWac_univcomp.CreateDate = drRow.Item("CreateDate")
                                End If
                                rWac_univcomp.CreateBy = drRow.Item("CreateBy")
                                If Not IsDBNull(drRow.Item("LastUpdate")) Then
                                    rWac_univcomp.LastUpdate = drRow.Item("LastUpdate")
                                End If
                                rWac_univcomp.UpdateBy = drRow.Item("UpdateBy")
                                rWac_univcomp.Active = drRow.Item("Active")
                                rWac_univcomp.Inuse = drRow.Item("Inuse")
                                rWac_univcomp.rowguid = drRow.Item("rowguid")
                                rWac_univcomp.SyncCreate = drRow.Item("SyncCreate")
                                rWac_univcomp.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rWac_univcomp.IsHost = drRow.Item("IsHost")
                                rWac_univcomp.LastSyncBy = drRow.Item("LastSyncBy")
                                rWac_univcomp.WasteType = drRow.Item("COMPGroup")
                                rWac_univcomp.AllowDec = drRow.Item("AllowDec")
                                rWac_univcomp.Precision = drRow.Item("Precision")
                            Else
                                rWac_univcomp = Nothing
                            End If
                        Else
                            rWac_univcomp = Nothing
                        End If
                    End With
                End If
                Return rWac_univcomp
            Catch ex As Exception
                Throw ex
            Finally
                rWac_univcomp = Nothing
                dtTemp = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWAC_UNIVCOMP(ByVal COMPCode As System.Int32, ByVal DEFUOM As System.String, DecendingOrder As Boolean) As List(Of Container.Wac_univcomp)
            Dim rWac_univcomp As Container.Wac_univcomp = Nothing
            Dim lstWac_univcomp As List(Of Container.Wac_univcomp) = New List(Of Container.Wac_univcomp)
            Dim dtTemp As DataTable = Nothing
            Dim lstField As New List(Of String)
            Dim strSQL As String = Nothing
            Dim strDesc As String = ""
            Try
                If StartConnection() = True Then
                    StartSQLControl()
                    With Wac_univcompInfo.MyInfo
                        If DecendingOrder Then
                            strDesc = " Order by COMPCode, DEFUOM DESC"
                        End If
                        strSQL = BuildSelect(.FieldsList, .TableName, "COMPCode = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, COMPCode) & "' AND DEFUOM = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, DEFUOM) & "'" & strDesc)
                        dtTemp = CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                        Dim rowCount As Integer = 0
                        If dtTemp Is Nothing = False Then
                            For Each drRow As DataRow In dtTemp.Rows
                                rWac_univcomp = New Container.Wac_univcomp
                                rWac_univcomp.COMPCode = drRow.Item("COMPCode")
                                rWac_univcomp.DEFUOM = drRow.Item("DEFUOM")
                                rWac_univcomp.COMPDesc = drRow.Item("COMPDesc")
                                rWac_univcomp.CPMIN = drRow.Item("CPMIN")
                                rWac_univcomp.CPMAX = drRow.Item("CPMAX")
                                rWac_univcomp.CreateBy = drRow.Item("CreateBy")
                                rWac_univcomp.UpdateBy = drRow.Item("UpdateBy")
                                rWac_univcomp.Active = drRow.Item("Active")
                                rWac_univcomp.Inuse = drRow.Item("Inuse")
                                rWac_univcomp.rowguid = drRow.Item("rowguid")
                                rWac_univcomp.SyncCreate = drRow.Item("SyncCreate")
                                rWac_univcomp.SyncLastUpd = drRow.Item("SyncLastUpd")
                                rWac_univcomp.IsHost = drRow.Item("IsHost")
                                rWac_univcomp.LastSyncBy = drRow.Item("LastSyncBy")
                                lstWac_univcomp.Add(rWac_univcomp)
                            Next

                        Else
                            rWac_univcomp = Nothing
                        End If
                        Return lstWac_univcomp
                    End With
                End If
            Catch ex As Exception
                Throw ex
            Finally
                rWac_univcomp = Nothing
                lstWac_univcomp = Nothing
                lstField = Nothing
                EndSQLControl()
                EndConnection()
            End Try
        End Function

        Public Overloads Function GetWAC_UNIVCOMPList(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With Wac_univcompInfo.MyInfo
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

        Public Overloads Function GetWAC_UNIVCOMPChart(Optional ByVal FieldCond As String = Nothing, Optional ByVal SQL As String = Nothing) As Data.DataTable
            If StartConnection() = True Then
                With Wac_univcompInfo.MyInfo
                    strSQL = "select unv.*, cm.CodeDesc, iif(allowdec=1, 'Allow', 'Not Allow') as AllowDecDesc,  Case when unv.Precision = 0 then '-' else cast(unv.Precision as varchar(10)) end as PrecisionDesc from wac_univcomp unv WITH (NOLOCK)" & _
                        " left join wac_compchart cc WITH (NOLOCK) ON unv.COMPCode = cc.COMPCode" & _
                        " left join CODEMASTER cm WITH (NOLOCK) ON cc.COMPGroup = cm.Code AND cm.CodeType='WTY' WHERE unv.Flag=1"
                    If FieldCond IsNot Nothing AndAlso FieldCond <> "" Then
                        strSQL &= " AND " & FieldCond
                    End If
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With

            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetAllowDec(ByVal CompDesc As String, ByVal WasteType As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_univcompInfo.MyInfo
                    strSQL = "Select * from wac_univcomp wi inner join wac_compchart wc on wi.compcode = wc.compcode WHERE wc.CompDescExt LIKE ('%" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, CompDesc) & "%') and wc.CompGroup = '" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, WasteType) & "'"
                    Return CType(objConn.Execute(strSQL, DataAccess.EnumRtnType.rtDataTable, CommandType.Text, .TableName), Data.DataTable)
                End With
            Else
                Return Nothing
            End If
            EndConnection()
        End Function

        Public Overloads Function GetBindComponent(ByVal type As String) As Data.DataTable
            If StartConnection() = True Then
                StartSQLControl()
                With Wac_univcompInfo.MyInfo
                    strSQL = "SELECT C.COMPCode, C.COMPDesc, C.CompDescExt FROM WAC_COMPCHART C WITH (NOLOCK) " & _
                        "WHERE C.COMPGroup='" & objSQL.ParseValue(SQLControl.EnumDataType.dtString, type) & "' AND c.COMPCode not in (select COMPCode from WAC_UNIVCOMP) ORDER BY C.CompDescExt ASC"
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
#End Region

#Region "Container"
    Namespace Container
#Region "Wac_univcomp Container"

        Public Class Wac_univcomp
            Public fCOMPCode As System.String = "COMPCode"
            Public fDEFUOM As System.String = "DEFUOM"
            Public fCOMPDesc As System.String = "COMPDesc"
            Public fCPMIN As System.String = "CPMIN"
            Public fCPMAX As System.String = "CPMAX"
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
            Public fAllowDec As System.String = "AllowDec"
            Public fPrecision As System.String = "Precision"

            Protected _COMPCode As System.Int32
            Protected _DEFUOM As System.String
            Private _COMPDesc As System.String
            Private _CPMIN As System.Decimal
            Private _CPMAX As System.Decimal
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
            Private _WasteType As System.String
            Private _AllowDec As System.Int32
            Private _Precision As System.Int32

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property AllowDec As System.Int32
                Get
                    Return _AllowDec
                End Get
                Set(ByVal Value As System.Int32)
                    _AllowDec = Value
                End Set
            End Property
            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property Precision As System.Int32
                Get
                    Return _Precision
                End Get
                Set(ByVal Value As System.Int32)
                    _Precision = Value
                End Set
            End Property
            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property Flag As System.Int32
                Get
                    Return _Flag
                End Get
                Set(ByVal Value As System.Int32)
                    _Flag = Value
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
            Public Property COMPCode As System.Int32
                Get
                    Return _COMPCode
                End Get
                Set(ByVal Value As System.Int32)
                    _COMPCode = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory
            ''' </summary>
            Public Property DEFUOM As System.String
                Get
                    Return _DEFUOM
                End Get
                Set(ByVal Value As System.String)
                    _DEFUOM = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property COMPDesc As System.String
                Get
                    Return _COMPDesc
                End Get
                Set(ByVal Value As System.String)
                    _COMPDesc = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CPMIN As System.Decimal
                Get
                    Return _CPMIN
                End Get
                Set(ByVal Value As System.Decimal)
                    _CPMIN = Value
                End Set
            End Property

            ''' <summary>
            ''' Mandatory, Not Allow Null
            ''' </summary>
            Public Property CPMAX As System.Decimal
                Get
                    Return _CPMAX
                End Get
                Set(ByVal Value As System.Decimal)
                    _CPMAX = Value
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
#Region "Wac_univcomp Info"
    Public Class Wac_univcompInfo
        Inherits Core.CoreBase
        Protected Overrides Sub InitializeClassInfo()
            With MyInfo
                .FieldsList = "COMPCode,DEFUOM,COMPDesc,CPMIN,CPMAX,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
                .CheckFields = "Active,Inuse,Flag,IsHost"
                .TableName = "Wac_univcomp"
                .DefaultCond = Nothing
                .DefaultOrder = Nothing
                .Listing = "COMPCode,DEFUOM,COMPDesc,CPMIN,CPMAX,CreateDate,CreateBy,LastUpdate,UpdateBy,Active,Inuse,Flag,rowguid,SyncCreate,SyncLastUpd,IsHost,LastSyncBy"
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
#Region "WAC_UNIVCOMP Scheme"
    Public Class WAC_UNIVCOMPScheme
        Inherits Core.SchemeBase
        Protected Overrides Sub InitializeInfo()

            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "COMPCode"
                .Length = 4
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(0, this)

            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "DEFUOM"
                .Length = 10
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(1, this)
            With this
                .DataType = SQLControl.EnumDataType.dtStringN
                .FieldName = "COMPDesc"
                .Length = 50
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(2, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "CPMIN"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(3, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "CPMAX"
                .Length = 9
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(4, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "CreateDate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(5, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "CreateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(6, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "LastUpdate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = False
                .AllowNegative = False
            End With
            MyBase.AddItem(7, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "UpdateBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(8, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Active"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(9, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Inuse"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(10, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "Flag"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(11, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "rowguid"
                .Length = 16
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(12, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncCreate"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(13, this)
            With this
                .DataType = SQLControl.EnumDataType.dtDateTime
                .FieldName = "SyncLastUpd"
                .Length = 8
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(14, this)
            With this
                .DataType = SQLControl.EnumDataType.dtNumeric
                .FieldName = "IsHost"
                .Length = 1
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(15, this)
            With this
                .DataType = SQLControl.EnumDataType.dtString
                .FieldName = "LastSyncBy"
                .Length = 20
                .DecPlace = Nothing
                .RegExp = String.Empty
                .isMandatory = True
                .AllowNegative = False
            End With
            MyBase.AddItem(16, this)

        End Sub

        Public ReadOnly Property COMPCode As StrucElement
            Get
                Return MyBase.GetItem(0)
            End Get
        End Property
        Public ReadOnly Property DEFUOM As StrucElement
            Get
                Return MyBase.GetItem(1)
            End Get
        End Property

        Public ReadOnly Property COMPDesc As StrucElement
            Get
                Return MyBase.GetItem(2)
            End Get
        End Property
        Public ReadOnly Property CPMIN As StrucElement
            Get
                Return MyBase.GetItem(3)
            End Get
        End Property
        Public ReadOnly Property CPMAX As StrucElement
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